namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for Oracle UDT type metadata - isolates Oracle.ManagedDataAccess dependency in wrapper layer.
    /// ODP.NET Managed Driver does not provide GetObjectType equivalent for schema-defined nested tables.
    /// TryGetObjectType always returns false; ArrayStorage uses row-by-row fallback.
    /// </summary>
    public class VOracleType
    {
        /// <summary>
        /// ODP.NET Managed has no equivalent to Devart's OracleType.GetObjectType.
        /// Always returns false - ArrayStorage will use row-by-row INSERT fallback.
        /// </summary>
        public static bool TryGetObjectType(string typeName, VOracleConnection connection, out VOracleType type)
        {
            type = null;
            return false;
        }
    }
}
