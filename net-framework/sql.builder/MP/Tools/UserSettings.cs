namespace sql.builder.MP.Tools
{
    public static class UserSettings
    {
        public static string ExcelPath
        {
            get { return RegistryHelper.GetStringValue("excelPath"); }
            set { RegistryHelper.SetStringValue("excelPath", value); }
        }
    }
}