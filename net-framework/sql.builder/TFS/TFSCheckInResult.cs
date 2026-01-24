namespace sql.builder.TFS
{
    internal class TFSCheckInResult
    {
        internal bool Success { get; set; }
        internal string ErrorMessage { get; set; }

        internal bool HasChanges { get; set; }
        internal int ChangesetId { get; set; }
        internal bool GatedBuild { get; set; }
        internal string GatedBuildName { get; set; }
    }
}