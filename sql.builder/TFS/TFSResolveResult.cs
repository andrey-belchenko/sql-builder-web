namespace sql.builder.TFS
{
    internal class TFSResolveResult
    {
        internal bool Success { get; set; }
        internal string ErrorMessage { get; set; }

        internal int NumResolvedConflicts { get; set; }
        internal TFSFileInfo[] UnresolvedFiles { get; set; }
    }
}