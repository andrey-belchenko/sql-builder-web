namespace sql.builder.TFS
{
    internal class TFSGetResult
    {
        internal bool Success { get; set; }
        internal string ErrorMessage { get; set; }

        internal int NumUpdated { get; set; }
        internal int NumConflicts { get; set; }
        internal int NumFailures { get; set; }
        internal TFSFileInfo[] UnresolvedFiles { get; set; }

        internal string FailuresMessage { get; set; }
    }
}