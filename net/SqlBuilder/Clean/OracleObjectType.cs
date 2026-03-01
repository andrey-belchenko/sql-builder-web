namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper enum for Oracle object type - isolates provider dependency.
    /// ODP.NET has different UDT model; minimal enum for compatibility.
    /// </summary>
    public enum OracleObjectType
    {
        Unknown = 0,
    }
}
