using Oracle.ManagedDataAccess.Client;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper enum for Oracle.ManagedDataAccess.Client.OracleDbType - isolates provider dependency.
    /// Named VOracleDbType to avoid ambiguity with provider's OracleDbType.
    /// </summary>
    public enum VOracleDbType
    {
        Number = 0,
        VarChar = 1,
        Date = 2,
        Clob = 3,
        Blob = 4,
        NClob = 5,
        NVarChar = 6,
        Array = 7,
        IntervalDS = 8,
        Integer = 9,
    }

    /// <summary>
    /// Conversion for interop with external code that requires Oracle.ManagedDataAccess OracleDbType (e.g. SqlArg).
    /// </summary>
    public static class VOracleDbTypeExtensions
    {
        public static OracleDbType ToOracle(this VOracleDbType dbType)
        {
            return dbType switch
            {
                VOracleDbType.Number => OracleDbType.Decimal,
                VOracleDbType.VarChar => OracleDbType.Varchar2,
                VOracleDbType.Date => OracleDbType.Date,
                VOracleDbType.Clob => OracleDbType.Clob,
                VOracleDbType.Blob => OracleDbType.Blob,
                VOracleDbType.NClob => OracleDbType.NClob,
                VOracleDbType.NVarChar => OracleDbType.NVarchar2,
                //VOracleDbType.Array => throw new System.NotSupportedException("OracleDbType.Array is not supported in ODP.NET Managed Driver. Use UDT custom types for nested tables."),

                VOracleDbType.Array=> OracleDbType.Array,
                VOracleDbType.IntervalDS => OracleDbType.IntervalDS,
                VOracleDbType.Integer => OracleDbType.Int32,
                _ => OracleDbType.Varchar2,
            };
        }
    }
}
