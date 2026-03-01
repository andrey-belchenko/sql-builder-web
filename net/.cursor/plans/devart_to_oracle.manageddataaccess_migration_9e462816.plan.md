---
name: Devart to Oracle.ManagedDataAccess Migration
overview: Replace Devart.Data.Oracle with Oracle.ManagedDataAccess.Client (ODP.NET Managed) across the SqlBuilder project by rewriting all 15 wrapper classes to use ODP.NET types, with special handling for Oracle Array/UDT support and connection string mapping.
todos:
  - id: sealed-wrappers
    content: VOracleDataAdapter and VOracleParameterCollection - use composition (ODP.NET types are sealed)
    status: completed
  - id: remaining-errors
    content: Fix remaining migration errors (VOracleParameter SourceColumn/DbType, VOracleCommand ParameterCheck, OracleConnectionStringBuilder UserId, C# 8 patterns, etc.)
    status: completed
isProject: false
---

# Devart to Oracle.ManagedDataAccess Migration Plan

## Scope

**Projects to update:**

- [SqlBuilder/SqlBuilder.csproj](C:\Repos\github\sql-builder-web\net\SqlBuilder\SqlBuilder.csproj) - replace Devart with Oracle.ManagedDataAccess.Core
- [SqlBuilder.DevTools/SqlBuilder.DevTools.csproj](C:\Repos\github\sql-builder-web\net\SqlBuilder.DevTools\SqlBuilder.DevTools.csproj) - remove Devart (inherits from SqlBuilder)

**Wrapper files to rewrite** (all in [SqlBuilder/Clean/](C:\Repos\github\sql-builder-web\net\SqlBuilder\Clean)):


| File                                    | Strategy                                                                 |
| --------------------------------------- | ------------------------------------------------------------------------ |
| VOracleConnection.cs                    | Inherit from `Oracle.ManagedDataAccess.Client.OracleConnection`          |
| VOracleCommand.cs                       | Inherit from `Oracle.ManagedDataAccess.Client.OracleCommand`             |
| VOracleTransaction.cs                   | Wrap `Oracle.ManagedDataAccess.Client.OracleTransaction`                 |
| VOracleParameter.cs                     | Inherit from `Oracle.ManagedDataAccess.Client.OracleParameter`           |
| VOracleParameterCollection.cs           | Inherit from `Oracle.ManagedDataAccess.Client.OracleParameterCollection` |
| VOracleDataReader.cs                    | Wrap `Oracle.ManagedDataAccess.Client.OracleDataReader`                  |
| VOracleDataAdapter.cs                   | Inherit from `Oracle.ManagedDataAccess.Client.OracleDataAdapter`         |
| VOracleException.cs                     | Wrap `Oracle.ManagedDataAccess.Client.OracleException`                   |
| VOracleLob.cs                           | Wrap `OracleBlob` or `OracleClob` (ODP.NET has separate types)           |
| VOracleConnectionStringBuilder.cs       | Map Devart-style props to ODP.NET connection string                      |
| VOracleParameterCollectionExtensions.cs | Replace `OracleBinary` with `byte[]` for BLOB display                    |
| OracleDbType.cs (VOracleDbType)         | Map to `Oracle.ManagedDataAccess.Client.OracleDbType`                    |
| OracleObjectType.cs                     | Minimal enum (ODP.NET has different UDT model)                           |
| VOracleArray.cs                         | **Critical** - ODP.NET uses UDT custom types, not OracleArray            |
| VOracleType.cs                          | **Critical** - ODP.NET uses `OracleCustomTypeMapping` / `OracleUdt`      |


---

## Critical API Differences

### 1. OracleDbType Enum Mapping

ODP.NET uses different names. Map VOracleDbType as follows:


| VOracleDbType | Oracle.ManagedDataAccess.Client.OracleDbType  |
| ------------- | --------------------------------------------- |
| VarChar       | Varchar2                                      |
| NVarChar      | NVarchar2                                     |
| Number        | Decimal                                       |
| Integer       | Int32                                         |
| Date          | Date                                          |
| Clob          | Clob                                          |
| Blob          | Blob                                          |
| NClob         | NClob                                         |
| IntervalDS    | IntervalDS                                    |
| Array         | **Not in Managed Driver** - see Array section |


### 2. Oracle Array / VARRAY / Nested Table (High Risk)

**Devart:** `OracleArray` class with `OracleArray(typeName, connection)` and `Add()`.

**ODP.NET Managed:** Does NOT have `OracleDbType.Array`. ODP.NET 21.3+ supports UDTs via:

- Custom .NET classes with `[OracleCustomTypeMapping("SCHEMA.TYPE_NAME")]`
- `OracleUdt.GetValue` / `SetValue` for attribute access
- See [Oracle UDT samples](https://github.com/oracle/dotnet-db-samples/tree/master/samples/udt) (Nested-Table.cs, VArray.cs)

**Affected code:**

- [ArrayStorage.cs](C:\Repos\github\sql-builder-web\net\SqlBuilder\Core\ArrayStorage.cs) - uses `VOracleArray` with `ASUSETYPES.NUMBER$TABLE` and `ASUSETYPES.VARCHAR2$TABLE` for FORALL bulk insert
- [DataHelper.cs](C:\Repos\github\sql-builder-web\net\SqlBuilder\Clean\DataHelper.cs) - `ConvertDecimalArrayToOracle`, `ConvertStringArrayToOracle`

**Options:**

- **A) UDT approach:** Create custom types `NumberTableUDT` and `Varchar2TableUDT` with `OracleCustomTypeMapping`, implement `FromCustomObject`/`ToCustomObject`. Most faithful to current design.
- **B) Fallback approach:** When `array_type == null` (VOracleType.TryGetObjectType fails), ArrayStorage already falls back to row-by-row INSERT. ODP.NET has no `GetObjectType` equivalent for schema-defined nested tables in the same way. We could use fallback-only for ArrayStorage and remove FORALL optimization.
- **C) Unmanaged ODP.NET:** Use `Oracle.DataAccess` (unmanaged) which has `OracleDbType.Array` - requires Oracle Client installation, defeats "managed" goal.

**Recommendation:** Implement Option A (UDT custom types) for full parity. If blocked, use Option B as interim.

### 3. OracleLob vs OracleBlob/OracleClob

**Devart:** Single `OracleLob` for both BLOB and CLOB; `GetOracleLob(ordinal)`.

**ODP.NET:** `GetOracleBlob(ordinal)` and `GetOracleClob(ordinal)` - separate methods.

**Change:** `VOracleLob` must wrap `object` (OracleBlob | OracleClob). `VOracleDataReader.GetOracleLob` should check column type via `GetDataTypeName(ordinal)` and call appropriate method. Expose common interface (Length, Read, etc.) or keep as opaque wrapper.

### 4. OracleBinary (BLOB parameter value)

**Devart:** `OracleBinary` type for BLOB parameters; `GetParameterValueDisplay` checks `val is OracleBinary`.

**ODP.NET:** BLOB parameters use `byte[]` directly.

**Change:** In [VOracleParameterCollectionExtensions.cs](C:\Repos\github\sql-builder-web\net\SqlBuilder\Clean\VOracleParameterCollectionExtensions.cs), replace `OracleBinary` check with `byte[]`: `if (val is byte[] bytes) return "[BLOB length=" + bytes.Length + "]";`

### 5. Connection String

**Devart** `VOracleConnectionStringBuilder` exposes: Direct, Server, ServiceName, Port, Sid, UserId, Password, Pooling.

**ODP.NET** uses: `User Id`, `Password`, `Data Source`. Data Source can be:

- TNS name: `Data Source=REALRYAZ.WORLD`
- Easy Connect: `Data Source=host:port/service_name`

**Change:** Build `Data Source` from Server/Port/ServiceName or Sid. If `Direct=true`, use Easy Connect format. Remove or map `Direct` (ODP.NET always uses TNS/Easy Connect).

### 6. OracleException

**Devart:** `OracleException` with `Code`, `ErrorCode`, `Errors`.

**ODP.NET:** `OracleException` with `Number` (error code), `Errors` collection. Map `Code` to `Number`.

**Change:** [VOracleException.cs](C:\Repos\github\sql-builder-web\net\SqlBuilder\Clean\VOracleException.cs) wrap `Oracle.ManagedDataAccess.Client.OracleException`, expose `Code => _inner.Number`, `ErrorCode`, `Errors`.

### 7. VOracleCommand constructors

Current constructors take `OracleConnection`/`OracleTransaction`. Change to `Oracle.ManagedDataAccess.Client.OracleConnection`/`OracleTransaction`. `VOracleTransaction.Inner` must expose ODP.NET's `OracleTransaction`.

### 8. VOracleParameter UnwrapValue

`UnwrapValue` unwraps `VOracleArray` to pass inner array to parameter. With ODP.NET UDT, the value would be the custom UDT instance, not VOracleArray. Adjust `UnwrapValue` for new array representation.

---

## DataHelper.OracleException and CustomOracleError

[DataHelper.cs](C:\Repos\github\sql-builder-web\net\SqlBuilder\Clean\DataHelper.cs) defines `infoenergo.core.Data.OracleException` (custom class, not Devart). [CustomOracleError.cs](C:\Repos\github\sql-builder-web\net\SqlBuilder\DataApi\DataObjects\CustomOracleError.cs) checks `ex is OracleException` - that refers to the custom `OracleException` in DataHelper, not the provider. No change needed for exception handling beyond wrapping ODP.NET's `OracleException` in `VOracleException`.

---

## SqlArg and ToDevart()

All `SqlArg` usages with `VOracleDbType.NClob.ToDevart()` are inside **commented blocks** in [db.cs](C:\Repos\github\sql-builder-web\net\SqlBuilder\db.cs). Active code uses `VOracleParameter` directly. Replace `ToDevart()` with `ToOracle()` (or equivalent) that returns `Oracle.ManagedDataAccess.Client.OracleDbType` for when/if commented code is re-enabled. Remove `ToDevart` extension; add `ToOracle()`.

---

## Implementation Order

1. **Package swap:** SqlBuilder.csproj - replace `Devart.Data.Oracle` with `Oracle.ManagedDataAccess.Core` (matches net8.0; Sandbox already uses it).
2. **Simple wrappers first:** VOracleException, VOracleTransaction, VOracleConnectionStringBuilder, OracleDbType (VOracleDbType + ToOracle), OracleObjectType.
3. **Core ADO.NET wrappers:** VOracleConnection, VOracleCommand, VOracleParameter, VOracleParameterCollection, VOracleDataReader, VOracleDataAdapter.
4. **LOBs and extensions:** VOracleLob, VOracleParameterCollectionExtensions (OracleBinary -> byte[]).
5. **Array/UDT (complex):** VOracleType, VOracleArray - implement UDT custom types for NUMBER$TABLE and VARCHAR2$TABLE, or fallback to row-by-row.
6. **SqlBuilder.DevTools:** Remove Devart package reference (inherits Oracle types from SqlBuilder).
7. **Verification:** Build, run existing tests, verify ArrayStorage and DataHelper flows.

---

## Files to Modify (Summary)


| File                                    | Changes                                                            |
| --------------------------------------- | ------------------------------------------------------------------ |
| SqlBuilder.csproj                       | Devart -> Oracle.ManagedDataAccess.Core                            |
| SqlBuilder.DevTools.csproj              | Remove Devart reference                                            |
| VOracleConnection.cs                    | Base: OracleConnection (ODP.NET)                                   |
| VOracleCommand.cs                       | Base: OracleCommand; catch OracleException (ODP.NET)               |
| VOracleTransaction.cs                   | Wrap OracleTransaction (ODP.NET)                                   |
| VOracleParameter.cs                     | Base: OracleParameter; ToOracle(VOracleDbType)                     |
| VOracleParameterCollection.cs           | Base: OracleParameterCollection                                    |
| VOracleDataReader.cs                    | Wrap OracleDataReader; GetOracleLob -> GetOracleClob/GetOracleBlob |
| VOracleDataAdapter.cs                   | Base: OracleDataAdapter                                            |
| VOracleException.cs                     | Wrap OracleException; Code = Number                                |
| VOracleLob.cs                           | Wrap OracleBlob                                                    |
| VOracleConnectionStringBuilder.cs       | Build Data Source from Server/Port/ServiceName                     |
| VOracleParameterCollectionExtensions.cs | OracleBinary -> byte[]                                             |
| OracleDbType.cs                         | VOracleDbType values map to ODP.NET; ToOracle()                    |
| OracleObjectType.cs                     | Keep minimal enum                                                  |
| VOracleArray.cs                         | UDT custom types or fallback                                       |
| VOracleType.cs                          | UDT metadata via OracleUdt or remove if fallback                   |
| ArrayStorage.cs                         | Adjust for new VOracleArray/VOracleType                            |
| DataHelper.cs                           | No structural changes if wrappers preserve API                     |
| db.cs                                   | No active changes (SqlArg in comments)                             |


---

## Risk: ArrayStorage FORALL Performance

If UDT implementation is deferred, ArrayStorage will use the existing fallback (row-by-row INSERT) for all cases. This may cause performance regression for large arrays. Document this and prioritize UDT implementation if ArrayStorage is used with large datasets.