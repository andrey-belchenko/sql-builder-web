namespace sql.builder
{
    internal partial class ucQueryEditor //: ucBase
    {
        internal static string remPathFromFilename(string filename)
        {
            //return filename.Replace(Cmn.SourcePath(), "");
            return filename.Replace(XmlReports.GetRootPath() + "\\", "");
        }
    }
}
