using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper enum for Devart.Data.Oracle.OracleDbType - isolates Devart dependency.
    /// Named VOracleDbType to avoid ambiguity with Devart's OracleDbType.
    /// </summary>
    public enum VOracleDbType
    {
        Number = Devart.Data.Oracle.OracleDbType.Number,
        VarChar = Devart.Data.Oracle.OracleDbType.VarChar,
        Date = Devart.Data.Oracle.OracleDbType.Date,
        Clob = Devart.Data.Oracle.OracleDbType.Clob,
        Blob = Devart.Data.Oracle.OracleDbType.Blob,
        NClob = Devart.Data.Oracle.OracleDbType.NClob,
        NVarChar = Devart.Data.Oracle.OracleDbType.NVarChar,
        Array = Devart.Data.Oracle.OracleDbType.Array,
        IntervalDS = Devart.Data.Oracle.OracleDbType.IntervalDS,
    }
}
