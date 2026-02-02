namespace sql.builder
{
    public partial class ucQueryEditor //: ucBase
    {
        public static string remPathFromFilename(string filename)
        {
            //return filename.Replace(Cmn.SourcePath(), "");
            return filename.Replace(XmlReports.GetRootPath() + "\\", "");
        }
    }
}
