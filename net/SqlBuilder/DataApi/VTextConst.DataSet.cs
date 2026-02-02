namespace sql.builder.DataApi
{

    internal static partial class TextConst
    {
        internal static class DsAName
        {
            public const string SourceTable = "source-table";
            public const string TextSourceFor = "text-source-for";
            public const string IsUserEditable = "is-user-editable";
            public const string IsUpdateable = "is-updateable";
            //public const string IsOtherSource = "is-other-source";
            public const string IsRefreshed = "is-refreshed";
            public const string IsUpdateableExt = "is-updateable-ext";
            public const string IsMainSourceUpdateable = "is-ms-upd";
            public const string IsJoinCol = "is-join-col";
            public const string RefColumn = "ref-column";
            public const string MultiselectColumn = "multiselect-column";
            public const string NonDb = "non-db";
            public const string MultiselectTarget = "multiselect-target";
            //public const string ColumnHasButtons = "column-has-buttons";

            public const string IsTop = "is-top";
            public const string TempColumnName = "temp-col-name";
            public const string ParentTable = "parent-table";
            public const string ParentKey = "parent-key";
            public const string Visible = "Visible";
            public const string Mandatory = "Mandatory";
            public const string Editable = "Editable";
            public const string Exists = "Exists";
            public const string Default = "Default";
            public const string NewVal = "NewVal";
            public const string TextSource = "TextSource";
            public const string Valid = "Valid";
            public const string FontColor = "FontColor";
            public const string BackColor = "BackColor";
        
        }

        internal static class DsEName
        {
            public const string DataSet = "dataset";
            public const string SelListPars = "sel-list-pars";
            public const string SelListClFactPars = "sel-list-cl-fact-pars";
            public const string SelListReport = "sel-list-report";
            public const string SelListCompiled = "sel-list-compiled";
            public const string SelListParentFieldName = "sel-list-parent-field-name";
            public const string SelectText = "select-text";
            public const string ProcText = "proc-text";
            public const string UpdateText = "update-text";
            public const string InsertText = "insert-text";
            public const string DeleteText = "delete-text";
            public const string UptadeTempText = "update-temp-text";
            public const string ClearTempText = "clear-temp-text";
            public const string SingleRowRefreshCmd = "single-row-refresh-cmd";
            public const string ValueRefreshCmd = "value-refresh-cmd";
            public const string ValueResetCmd = "value-reset-cmd";
            public const string DepRefreshCommand = "dep-refresh-cmd";
            public const string Dependants = "dependants";
            public const string Dependant = "dependant";
            public const string ExtraKey = "extra-key";

        }

        internal static class DsANameArray
        {
            public static string[] AllBehProps = {
            DsAName.Editable,
            DsAName.Exists,
            DsAName.Mandatory,
            DsAName.NewVal,
            DsAName.Valid,
            DsAName.TextSource,
            DsAName.Visible,
            DsAName.Default,
            DsAName.FontColor,
            DsAName.BackColor

        };

        }
    }
}
