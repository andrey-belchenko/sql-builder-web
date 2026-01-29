---
name: Dependency Loader Implementation
overview: Implement a dependency loader that processes unprocessed database objects, extracts their dependencies recursively, and updates the database until no new dependencies are found.
todos:
  - id: add_storage_methods
    content: Add GetUnprocessedDbObjects(), UpdateDbObjectProcessed(), and UpdateDbObjectType() methods to AnalyzerStorage.cs
    status: pending
  - id: add_dependency_loader
    content: Add LoadDependencies() main method to DevAnalyzer.cs that implements the recursive processing loop
    status: pending
    dependencies:
      - add_storage_methods
  - id: add_process_object
    content: Add ProcessDbObject() method to DevAnalyzer.cs that handles type resolution, DDL retrieval, and dependency extraction
    status: pending
    dependencies:
      - add_storage_methods
  - id: implement_view_processing
    content: "Implement view/matview processing: get DDL, extract dependencies using DevSqlParserAntlr, save dependencies"
    status: pending
    dependencies:
      - add_process_object
  - id: implement_procedure_processing
    content: "Implement procedure processing: get package DDL, extract procedure name, analyze dependencies for specific procedure, save dependencies"
    status: pending
    dependencies:
      - add_process_object
  - id: add_error_handling
    content: Add error handling for missing objects, DDL retrieval failures, and parsing errors
    status: pending
    dependencies:
      - implement_view_processing
      - implement_procedure_processing
---

# Dependency Loader Implementation

## Overview

Implement a dependency loader that processes unprocessed items from `db_objects` table, extracts dependencies from views, materialized views, and procedures, and recursively processes newly discovered dependencies until no new items are found.

## Implementation Details

### Flow Diagram

```mermaid
flowchart TD
    Start([Start]) --> GetUnprocessed[Get Unprocessed Items]
    GetUnprocessed --> CheckEmpty{Any Unprocessed?}
    CheckEmpty -->|No| End([End - No New Dependencies])
    CheckEmpty -->|Yes| ProcessItem[Process Next Item]
    ProcessItem --> CheckType{Object Type?}
    CheckType -->|TableOrView| ResolveType[Resolve Real Type via DevOracleSheme]
    CheckType -->|Table| MarkProcessed[Mark as Processed]
    CheckType -->|View/MatView| GetViewDDL[Get View DDL]
    CheckType -->|Procedure| GetPackageDDL[Get Package DDL]
    ResolveType --> UpdateType[Update Object Type in DB]
    UpdateType --> CheckType
    GetViewDDL --> ExtractDeps[Extract Dependencies via DevSqlParserAntlr]
    GetPackageDDL --> ExtractProcDeps[Extract Procedure-Specific Dependencies]
    ExtractDeps --> SaveDeps[Save Dependencies]
    ExtractProcDeps --> SaveDeps
    SaveDeps --> MarkProcessed
    MarkProcessed --> GetUnprocessed
```

### Key Components

#### 1. Add Methods to `AnalyzerStorage.cs`

- **`GetUnprocessedDbObjects()`**: Query database for items where `processed = false`
- **`UpdateDbObjectProcessed(string objectName, bool processed)`**: Update the `processed` flag for a specific object
- **`UpdateDbObjectType(string objectName, DbObjectType newType)`**: Update object type when resolving TableOrView

#### 2. Create Dependency Loader in `DevAnalyzer.cs`

- **`LoadDependencies()`**: Main entry point that runs the dependency loading loop
- **`ProcessDbObject(AnalyzerDbObject dbObject)`**: Process a single database object:
  - Resolve TableOrView type using `DevOracleSheme.GetTableInfo()`
  - For Views/MatViews: Get DDL, extract dependencies, save them
  - For Procedures: Get package DDL, extract procedure-specific dependencies, save them
  - Mark as processed

#### 3. Dependency Extraction Logic

- **For Views/MatViews**: Use `DevSqlParserAntlr.GetSourceTables(ddl)` and `GetSourceProcedures(ddl)` similar to `AnalyzeCmdSql()` (lines 297-319)
- **For Procedures**: 
  - Parse procedure name format: `package.procedure` (extract procedure part after dot)
  - Use `DevSqlParserAntlr.GetSourceTables(packageDdl, procedureName)` and `GetSourceProcedures(packageDdl, procedureName)`
  - This extracts dependencies only from that specific procedure within the package

#### 4. Recursive Processing

- After processing all unprocessed items, check for new unprocessed items
- Continue loop until no new dependencies are discovered (no new unprocessed items appear)

## Files to Modify

### [`SqlBuilderLib/DevTools/AnalyzerStorage.cs`](SqlBuilderLib/DevTools/AnalyzerStorage.cs)

Add methods:

- `GetUnprocessedDbObjects()` - returns `List<AnalyzerDbObject>`
- `UpdateDbObjectProcessed(string objectName, bool processed)` - updates processed flag
- `UpdateDbObjectType(string objectName, DbObjectType newType)` - updates object type

### [`SqlBuilderLib/DevTools/DevAnalyzer.cs`](SqlBuilderLib/DevTools/DevAnalyzer.cs)

Add methods:

- `LoadDependencies()` - main entry point, implements the recursive loop
- `ProcessDbObject(AnalyzerDbObject dbObject)` - processes single object and extracts dependencies
- Helper method to extract procedure name from `package.procedure` format

## Implementation Notes

1. **Type Resolution**: When object type is `TableOrView`, use `DevOracleSheme.GetTableInfo()` to determine if it's Table, View, or MatView, then update the database record.

2. **Procedure Name Parsing**: Procedure names in `db_objects` may be stored as `package.procedure`. Extract the procedure name (part after the last dot) for use with `GetSourceTables/GetSourceProcedures` `procedureName` parameter.

3. **Error Handling**: Handle cases where:

   - Object doesn't exist in Oracle database
   - DDL retrieval fails
   - SQL parsing fails
   - Continue processing other items even if one fails

4. **Cache Updates**: After saving new dependencies that create new `db_objects` entries, refresh the cache or ensure cache consistency.

5. **Logging**: Add console output to track progress (similar to `AnalyzeReports()` method).