namespace sql.builder.XmlHelpers
{
    public static class SettingsHelper
    {
        public static string WorkFolder
        {
            get { return Cmn.ReadStringFromRegistry("all", "workfolder"); }
            set { Cmn.WriteStringToRegistry("all", "workfolder", value); }
        }

        public static string DevExpressSkinName
        {
            get
            {
                var skin_name = Cmn.ReadStringFromRegistry("all", "skinname");
                return string.IsNullOrEmpty(skin_name) ? "Office 2010 Silver" : skin_name;
            }
            set { Cmn.WriteStringToRegistry("all", "skinname", value); }
        }

        public static string CompareFolder
        {
            get { return Cmn.ReadStringFromRegistry("all", "compare_path"); }
            set { Cmn.WriteStringToRegistry("all", "compare_path", value); }
        }

        public static string TestSettingsFolder
        {
            get { return Cmn.ReadStringFromRegistry("all", "test_settings_folder"); }
            set { Cmn.WriteStringToRegistry("all", "test_settings_folder", value); }
        }

        public static string SaveLoadTestSettingsFolder
        {
            get { return Cmn.ReadStringFromRegistry("all", "save_load_test_settings_folder"); }
            set { Cmn.WriteStringToRegistry("all", "save_load_test_settings_folder", value); }
        }

        public static bool ShowInvisibleReports
        {
            get { return (Cmn.ReadStringFromRegistry("all", "show_invisible_reports") != false.ToString()); }
            set { Cmn.WriteStringToRegistry("all", "show_invisible_reports", value.ToString()); }
        }

        public static string ProjectsState
        {
            get { return (Cmn.ReadStringFromRegistry("all", "projects_state")); }
            set { Cmn.WriteStringToRegistry("all", "projects_state", value); }
        }

        public static bool UseFormCache
        {
            get { return (Cmn.ReadStringFromRegistry("all", "use_form_cache") != false.ToString()); }
            set { Cmn.WriteStringToRegistry("all", "use_form_cache", value.ToString()); }
        }

        public static bool ShowTestParamsPanel
        {
            get { return (Cmn.ReadStringFromRegistry("all", "show_test_params_panel") != false.ToString()); }
            set { Cmn.WriteStringToRegistry("all", "show_test_params_panel", value.ToString()); }
        }

        public static string TFSComment
        {
            get { return (Cmn.ReadStringFromRegistry("all", "tfs_comment")); }
            set { Cmn.WriteStringToRegistry("all", "tfs_comment", value); }
        }
    }
}
