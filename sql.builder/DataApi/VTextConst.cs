using System.Xml.Linq;

namespace sql.builder.DataApi
{

    internal static partial class TextConst
    {
        //public static class VTextConst
        //{
        //    public enum EelsNames
        //    {
        //        Form = "form",
        //        Forms = "forms",
        //        Query = "query",
        //        Field = "field",
        //        From = "from",
        //        Content = "content",
        //        Panel = "panel",
        //        Column = "column",
        //        Select = "select",
        //        Root = "root"
        //    }

        //}


        //internal static class Images
        //{
        //    public const string Warning16 = "Warning_16";
        //}

        internal static class MsgTypePref
        {
            public const string Information = "[inf]"; // только значок
            public const string Warning = "[wrn]"; //  значок и предупреждение но можно сохранить// переделал, теперь предупреждение без значка // для гридов не реализовано
            public const string Error = "[err]"; // Емцов 43400(1) как wrn, только нельзя сохранить
        }
        internal static class MsgTypePrefArray
        {
            public static string[] All = { MsgTypePref.Information,MsgTypePref.Warning,MsgTypePref.Error};
            public static string[] CanSave = { MsgTypePref.Information, MsgTypePref.Warning };
            public static string[] Alert = { MsgTypePref.Warning};
           

        }
        internal static class EName
        {
            public const string Function = "function";
            public const string Insert = "insert";
            public const string ChangeSources = "change-sources";
            public const string ChangeSource = "change-source";
            public const string Navigators = "navigators";
            public const string Navigator = "navigator";
            public const string Multireference = "multireference";
            public const string Visualizers = "visualizers";
            public const string Functions = "functions";
            public const string DimQuery = "dimquery";
            public const string OnColumns = "on-columns";
            public const string OnRows = "on-rows";
            public const string Storages = "storages"; // очередной пробный вариант с хранилищем
            public const string Connect = "connect";
            public const string Start = "start";
            public const string SecurityPackage = "security-package";
            public const string Role = "role";
            public const string UseObject = "use-object";
            public const string UseRole = "use-role";
            public const string Factlinks = "factlinks";
            public const string Undefined = "undefined";
            public const string EmptyItem = "empty-item";
            public const string With = "with";
            public const string Color = "color";
          
            public const string UseColor = "use-color";
            public const string ColorPackages = "color-packages";
            public const string ColorPackage = "color-package";
            public const string Text = "text";
            public const string Form = "form";
            public const string DimSet = "dimset";
            public const string Forms = "forms";
            public const string Query = "query";
            public const string Queries = "queries";
            public const string Field = "field";
            public const string Fields = "fields";
            public const string From = "from";
            public const string Format = "format";
            public const string FormatPackages = "format-packages";
            public const string FormatPackage = "format-package";
            public const string Content = "content";
            public const string FieldGroup = "fieldgroup";
            public const string Label = "label";
            public const string ScrollArea = "scrollarea";
            public const string Table = "table";
            public const string Columns = "columns";
            public const string ViewColumns = "viewcolumns";
            public const string PivotFields = "pivot_fields";
            public const string FilterArea = "filter_area";
            public const string ColumnArea = "column_area";
            public const string RowArea = "row_area";
            public const string DataArea = "data_area";
            public const string LayoutColumns = "layoutcolumns";
            public const string LayoutColumn = "layoutcolumn";
            public const string Column = "column";
            public const string Select = "select";
            public const string Root = "root";
            public const string PrintTemplates = "print-templates";
            public const string Template = "template";
            public const string Excel = "excel";
            public const string Word = "word";
            //public const string ExcelTemplates = "excel-templates";
            //public const string ExcelTemplate = "excel-template";
            //public const string ExcelColumn = "excel-column";
            //public const string ExcelSheet = "excel-sheet";
            public const string Report = "report";
            public const string Reports = "reports";
            public const string Data = "data";
            public const string Tr = "tr";
            public const string Td = "td";
            public const string Cells = "cells";
            //public const string NavigationItem = "navigationitem";
            //public const string NavigationItems = "navigationitems";
            //public const string NavigationField = "navigationfield";

            public const string DefaultQuery = "defaultquery";
            public const string Default = "default";
            public const string Layout = "layout";
            public const string TabLayout = "tablayout";
            public const string TabGroupLayout = "tabgrouplayout";
            public const string Action = "action";
            public const string Actions = "actions";
            public const string Rowactions = "rowactions";
            public const string UseAction = "useaction";
            public const string Param = "param";
            public const string Params = "params";
            public const string UseParam = "useparam";
            public const string UseGlobParam = "useglobparam";
            public const string GlobalParams = "globalparams";

            public const string Link = "link";

            public const string ELink = "elink";
            public const string DimLink = "dimlink";
            public const string DLink = "dlink";
            public const string SLink = "slink";
            public const string Grid = "grid";
            public const string Where = "where";
            public const string ExtendWhere = "extendwhere";
            public const string ExtendLinks = "extendlinks";
            public const string Call = "call";
            public const string Scheme = "scheme";
            public const string Childs = "childs";
            public const string UseField = "usefield";
            public const string TabContainer = "tabcontainer";
            public const string SplitContainer = "splitcontainer";
            public const string Push = "push";
            public const string Const = "const";
            public const string Array = "array";
            public const string UsePart = "usepart";
            public const string Using = "using";

            public const string If = "if";
            public const string Pivot = "pivot";
            public const string Having = "having";
            public const string Union = "union";
            public const string Links = "links";
            public const string WithParams = "withparams";
          //  public const string WithParams2 = "withparams2"; // дурацкая заплатка, элемент withparams удаляется при компиляции а иногда нужно оставить
            public const string ReportProc = "procedure";

            public const string Customers = "customers";
            public const string Customer = "customer";
            public const string Folders = "folders";
            public const string Folder = "folder";
            public const string UseForm = "useform";
            public const string Section = "section";
            public const string Band = "band";
            public const string ColumnsPreset = "columnspreset";
            public const string ListQuery = "listquery";
            public const string ExpressionPackage = "expression-package";
            public const string Fact = "fact";
            public const string Facts = "facts";
            public const string ExpressionPackages = "expression-packages";
            public const string SecurityPackages = "security-packages";
            public const string Qube = "qube";
            public const string QubeContent = "qubecontent";
            public const string Menu = "menu";
            public const string Toolbar = "toolbar";
            public const string Events = "events";
            public const string UICommand = "uicommand";
            public const string ButtonType = "button-type";
            public const string ButtonTypes = "button-types";
            public const string Splitter = "splitter";
            public const string Dimension = "dimension";
            public const string Dimensions = "dimensions";
            public const string DimensionValues= "dimension-values";
            public const string DimensionPackage = "dimension-package";
            public const string DimensionPackages = "dimension-packages";
            public const string Scope = "scope";
            public const string Parts = "parts";
            public const string Part = "part";
            public const string Compiled = "compiled";
            public const string Buttons = "buttons";
            public const string Grsets = "grsets";
            public const string Grouping = "grouping";
            public const string Grset = "grset";
            public const string Group = "group";
            public const string SourceLink = "sourcelink";
            public const string Expressions = "expressions";
            public const string Additions = "additions";
            public const string Addition = "addition";
            public const string ColDimVal = "col-dim-val";
            public const string Val = "val";
            //public const string GlobConsts = "globalconsts";
            //public const string GlobConst = "globalconst";
            public const string UseReport = "usereport";
            public const string UseTemplate = "usetemplate";
            public const string Projects = "projects";
            public const string Project = "project";
            public const string References = "references";
            public const string Reference = "reference";
            public const string Measures = "measures";
        }

        internal static class ENameArray
        {
            public static string[] ALinks = { EName.Link, EName.ELink, EName.DLink, EName.SLink };
            public static string[] ALinksAndQuery = { EName.Query, EName.Link, EName.ELink, EName.DLink, EName.SLink };
            public static string[] ALinksButElink = { EName.Link, EName.DLink, EName.SLink };
            public static string[] ANewQueryAttributes = { AName.MaterializeId, AName.Dimension, AName.MultiplicatePoint, AName.LinkMultiplicatePoint,AName.NoGrouping };
            public static string[] ColumnAndFact = { EName.Column, EName.Fact };
            public static string[] ColumnAndFactAndCall = { EName.Column, EName.Fact,EName.Call };
            public static string[] AllowTextMode = { EName.Column, EName.Field, EName.Fact, EName.Label };
            public static string[] AllowLayoutMode = { EName.FieldGroup };
        }
        internal static class ANameSpec
        {
            public const string IsFromTemp = "is-from-temp";
           
        }


        internal static class AName
        {
            public const string Async = "async";
            public const string InsByLoop = "ins-by-loop";
            public const string CondSource = "condsource";
            public const string PostProcess = "post-process"; 
            public const string IsScalar = "is-scalar"; 
            public const string Interval = "intrval";
            public const string CanBeChecked = "can-be-checked";
            public const string Old = "old"; 
            public const string Index = "index"; 
            public const string ClientView = "client-view";
            public const string FormatSource = "format-source"; 
            public const string UseColPreset = "use-col-preset"; 
            public const string AutoFilter = "auto-filter";
			public const string AllowSelectMoveColumns = "allow-select-move-columns";
			public const string InvisibleInColumnChooser = "invisible-in-column-chooser";
            public const string MergeDimsets = "merge-dimsets";
            public const string StarScheme = "star-scheme";
            public const string SingleWay = "single-way";
            public const string ClearOnListChange = "clear-on-list-change";
            public const string Removeable = "removeable";
            public const string Removeable2 = "removeable2";
            public const string AutoMerge = "auto-merge";
            public const string MergeKey = "merge-key";
            public const string ConstrDelOption = "constr-del-option";
            public const string Nvl = "nvl";
            public const string Project = "project";
            public const string Navigator = "navigator";
            public const string CanUseSimpleParams = "can-use-simple-params"; 
            // способ подстановыки параметров 1. через oracleparametr, 2. подстановка константы в текст при компиляции.
            // 1. включается если для вех параметров указаны типы
            // если этот признак НЕ установлен то типы параметров не указываются
            // пришлось так сделать потому, что для некоторых запросов способ 1 не подходит.
            public const string DontPush = "dont-push";
            public const string TreeLevel = "tree-level";
            public const string DontUseForGroupingKey = "dont-use-for-gr-key";
            public const string Prompt = "prompt";
            public const string Message = "message";
			public const string Notification = "notification";
            public const string Groupingid = "groupingid";
            public const string ParentGroupingid = "parent-groupingid";
            public const string RowSelector = "row-selector";
            public const string ForRows = "for-rows";
            public const string Intervals = "intervals";
            public const string ClientCalulation = "client-calc";
            public const string ExcelCalulation = "excel-calc";
            public const string OnColumns = "on-columns";
            public const string OnRowsGrsetId = "on-rows-grset-id";
            public const string OnColsGrsetId = "on-cols-grset-id";
            public const string OrigGrsetId = "orig-grset-id";// для ситуации сроки-колонки, у дерева что то дркгое похожее - разобраться
            public const string WithBehavior = "with-behavior"; // по умолчанию будет новый вариант формы с поведением, для старых форм ставлю 0 чтобы не сломалось - временно.
            public const string SaveCompiled = "save-compiled";
            public const string MaterializeType = "materialize-type";
            public const string Condition = "condition";
            public const string ConvertToOpenXml = "conv-to-openxml";
            public const string IsFinalDimension = "is-final-dimension";
            public const string IsPrivateDimension = "is-private-dimension";
            public const string Role = "role";
            public const string SecurityId = "security-id";
            public const string Multiplicer = "mp";
            public const string AutoRefresh = "auto-refresh";
            public const string OnlyVisibleRefresh = "only-visible-refresh";
            public const string OnlyForceRefresh = "only-force-refresh";
            public const string IsDone = "is-done";
            public const string Link = "link";
            public const string Window = "window";
            public const string ValueColumn = "value-column";
            public const string DimensionValue = "dimension-value";
            public const string DimensionColumn = "dimension-column";
            public const string Addition = "addition";
            public const string UseTemp = "use-temp";
            //public const string FromTemp = "from-temp";
            public const string OnlyForCond = "only-for-cond";
            public const string With = "with";
            public const string Rgb = "rgb";
            public const string Color = "color";
            public const string HAlign = "halign";
            public const string FontColor = "font-color";
            public const string MultiplicatePoint = "multiplicate-point";
            public const string LinkMultiplicatePoint = "link-mp-point";
            public const string ParentNodeId = "parent-node-id";
            public const string NodeId = "node-id";
            public const string Level = "level";
            public const string ParentLevel = "parent-level";
            public const string IsVertical = "is-vertical";
            public const string IsForm = "is-form";
            public const string Into = "into";
            public const string Order = "order";
            public const string PrintXlsx = "print-xlsx";
            public const string Master = "master";
            public const string Colset = "colset";
            public const string CMaster = "c-master";
            public const string CMasterKey = "c-master-key";
            public const string Id = "id";
            public const string Name = "name";
            public const string OldNames = "oldnames";
            public const string View = "view";
            public const string As = "as";
            public const string IsListColumn = "vid";
            public const string IsNameColumn = "is-name";
            public const string Visible = "visible";
            public const string ColumnVisible = "column-visible";
            public const string ColumnExists = "column-exists";
            public const string Title = "title";
            public const string Width = "width";
           
            public const string Comment = "comment";
            public const string ClassTitle = "class-title";
            public const string DataType = "type";
            public const string Table = "table";
            public const string Column = "column";
            public const string File = "file";
            public const string ControlType = "controlType";
            public const string Control = "control";
            public const string ControlName = "control-name";
            public const string Join = "join";
            public const string JoinExp = "joinexp";
            public const string RowsLimit = "rows-limit";
            public const string Step = "step";
            public const string NullIf = "nullif";
            public const string NullAsUndefined = "null-as-undefined";
            public const string Report = "report";
            public const string Description = "description";
            public const string Form = "form";
            public const string Folder = "folder";
            public const string Size = "size";
            public const string DataSize = "data-size";
            public const string MinSize = "min-size";
            public const string MaxSize = "max-size";
            public const string Position = "position";
            public const string TextLocation = "text-location";
            public const string TextVisible = "text-visible";
           // public const string LayoutMode = "layout-mode";
            public const string TemplateName = "template-name";
            public const string ChildName = "child-name";
            public const string KeyName = "key-name";
            public const string CallType = "call-type";
            public const string Call = "call";
            public const string Type = "type";
            public const string Side = "side";
            public const string FormSize = "form-size";
            public const string Value = "value";
            public const string WidthPerc = "width-perc";
            public const string WidthFixed = "width-fixed";
            public const string FixedSide = "fixed-side";
            public const string TableCode = "table-code";
            public const string FillHeight = "fill-height";
            public const string ParName = "parname";
            public const string DName = "dname";
            public const string DTitle = "dtitle";
            public const string Key = "key";
            public const string ParentKey = "parent-key";
            public const string Parent= "parent";
            public const string If = "if";
            public const string Function = "function";
            public const string Field = "field";
            public const string EditColumns = "edit-columns";
            public const string AllowSave = "allow-save";
            public const string ParamsCustomization = "params-customization";
            public const string ActionType = "action-type";
            public const string Group = "group";
            public const string GroupingSource = "grouping-source";
            public const string NoGrouping = "nogrouping";
            public const string NoGrid = "nogrid";
            public const string Dgroup = "dgroup";
            public const string Pushpred = "pushpred";
            public const string DontPushpred = "dontpushpred";
            public const string Exclude = "exclude";
            //public const string Prime = "prime";
            public const string Prior = "prior";
            public const string ClassType = "class-type";
            public const string Flag = "flag";
            public const string Fixed = "fixed";
            public const string Target = "target";
            public const string Hint = "hint";
            public const string Materialize = "materialize";
            public const string MaterializeId = "materialize-id";
            public const string MultiSelectTarget = "multi-select-target";
            public const string MultiSelectColumn = "multi-select-column";
            public const string All = "all";
            public const string CurrentNode = "current-node";
            public const string Expanded = "expanded";
            public const string Uncollapsible = "uncollapsible";
            public const string NoBorder = "noborder";
            public const string IsLayoutBlock = "is-layout-block";
            public const string Optional = "optional";
            public const string IsHyperlink = "hyperlink";
            public const string UseOnlyWithOther = "use-only-with-other";
            public const string Extend = "extend";
            public const string UpdateTarget = "update-target";
            public const string Updateable = "updateable";
            public const string Default = "default";
            public const string ColumnDefault = "column-default";
            public const string Editable = "editable";
            public const string DeleteValidation = "delete-validation";
            public const string ColumnEditable = "column-editable";
            public const string KodMenu = "kod-menu";
            public const string Mandatory = "mandatory";
            public const string ColumnMandatory = "column-mandatory";
            public const string Object = "object";
            public const string Selective = "selective";
            public const string IsReport = "is-report";
            public const string UseFlexCel = "use-flexcel";
            public const string AddNames = "add-names";
            public const string Stored = "stored";
            public const string ParentFieldName = "parent-field-name";
            public const string OrderFieldName = "order-field-name";
            public const string CheckFieldName = "check-field-name";
            public const string Pfx = "pfx";
            public const string UseRepository = "use-repository";
            public const string Dimname = "dimname";
            public const string SpecialType = "special-type";
            public const string ValueQuery = "valuequery";
            public const string AutoCheck = "auto-check";
            public const string ShowNulls = "show-nulls";
            public const string ExpandAll = "expand-all";
            public const string ValFieldName = "val-field-name";
            public const string NameFieldName = "name-field-name";
            public const string SearchFieldName = "search-field-name";
            public const string EditMask = "edit-mask";
            public const string MaxLength = "max-length";
            public const string Agg = "agg";
            public const string AggCml = "agg-cml";
            public const string Format = "format";
            public const string Scrollable = "scrollable";
            public const string Valid = "valid";
            public const string TextSource = "textsource";
            public const string Fact = "fact";
            public const string IsFact = "is-fact";
            public const string IsFactUse = "is-fact-use";
            public const string Dimension = "dimension";
            public const string FactDimension = "fact-dimension";
            public const string KeyDimension = "key-dimension";
            public const string AllowBackReference = "allow-back-reference";
            public const string AllRows = "all-rows";
            public const string ButtonType = "button-type";
           // public const string Listquery = "listquery";
            public const string ShowToolBar = "show-toolbar";
            public const string ShowBottomToolBar = "show-bottom-toolbar";
            public const string ShowFooter = "show-footer";
            public const string ShowAggPanel = "show-agg-panel";
            public const string MultiSelect = "multi-select";
            public const string Sys = "sys";
            public const string EventName = "event-name";
            public const string Modal = "modal";
            public const string UseParentDsId = "use-parent-ds-id";
            public const string ShowCheckbox = "show-checkbox";
            public const string Icon = "icon";
            public const string Timeline = "timeline";
            public const string TimeType = "time-type";
            public const string TimeStamp = "timestamp";
            public const string Cumulate = "cumulate";
            public const string CumulateAgg = "cumulate-agg";
            public const string Checked = "checked";
            public const string NewVal = "new-val";
            public const string VisibleInvert = "visible-invert";
            public const string EditableInvert = "editable-invert";
            public const string ExistsInvert = "exists-invert";
            public const string MandatoryInvert = "mandatory-invert";
            public const string SpecTable = "spec-table";
            public const string SpecColumn = "spec-column";
            public const string Inherit = "inherit";
            public const string Intern = "intern";
            public const string Invert = "invert";
            public const string PartId = "part-id";
			public const string Part = "part";
            public const string Multiple = "multiple";
            public const string Pth = "pth";
            public const string NewRowsVisForOtherTbls = "new-rows-vis-for-other-tbls";
            
            public const string ActionRows = "action-rows";
            public const string Exists = "exists";
            public const string CalculateTree = "calctree";
            public const string IsTree = "is-tree";
            public const string IsTreeSplitCols = "is-tree-split-cols";
            public const string TreeOriginalColumn = "tree-original-column";
            public const string TreeLevelColumn = "tree-level-column";




            //public const string StoreInDB = "store-in-db"; // убираю , метод передачи параметра определяется автоматически по к-ыу значений
            public const string PrepareMerge = "prep-merge";

            public const string HeadMarker = "headmarker";
            public const string CellsMerge = "cells-merge";
            public const string CellsDelete = "cells-delete";
            public const string RowsHeight = "rows-height";
            public const string PagesBreak = "pages-break";

            public const string DxExport = "dx-export";
            public const string IsRet = "is-ret";

            public const string ViewMode = "mode";
            public const string ParamType = "param-type";

            public const string DelCols = "del-cols";
            public const string DataReader = "datareader";

            public const string ColumnWidth = "column-width";
			public const string DetailsUseZeros = "details-use-zeros";
            public const string EnableShowHiddenCollumnsOption = "Enable-Show-Hidden-Columns-Option";
        }
        internal static class AVHAlign
        {
            public const string Left = "left";
            public const string Right = "right";
            public const string Center = "center";
            
        }

        internal static class AVFormButtonType
        {


            public const string Refresh = "ButtonRefresh";
            public const string Save = "ButtonSave";
            public const string SaveAndClose = "ButtonSaveAndClose";
            public const string Delete = "ButtonDelete";
            public const string Choice = "ButtonChoice";
            public const string ExtParams = "btnExtParams";
            public const string SaveSettings = "btnSaveSettings";
            public const string LoadSettings = "btnLoadSettings";
        }

         

        internal static class AVFormButtonTypeArray
        {


            public static string[] All = { AVFormButtonType.Refresh, AVFormButtonType.Save, AVFormButtonType.SaveAndClose,
                                             AVFormButtonType.Delete, AVFormButtonType.Choice, AVFormButtonType.ExtParams, AVFormButtonType.SaveSettings, AVFormButtonType.LoadSettings, };
        }



        internal static class AVGridButtonType
        {
            public const string SaveSettings = "ButtonSaveSettings";
            public const string RestoreSettings = "ButtonRestoreSettings";
            public const string Up = "ButtonUp";
            public const string Down = "ButtonDown";
            public const string ChoiceRow = "ButtonChoiceRow";
            public const string Refresh = "ButtonRefresh";
            public const string AddRow = "ButtonAddRow";
            public const string DeleteRow = "ButtonDeleteRow";
            public const string Commit = "ButtonCommit";
            public const string ExportExcel = "ButtonExportExcel";
            public const string Paste = "ButtonPaste";
            public const string CopyToCB = "ButtonCopyToCB";
        }

        //public enum FormBarButtonType
        //{
        //    Refresh,
        //    Save,
        //    SaveAndClose,
        //    Delete,
        //    Choice,
        //    //Test,

        //    ExtParams,
        //    SaveSettings,
        //    LoadSettings
        //    //  , ViewTemp
        //}


        //this.tbMain.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
        //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonRefresh),
        //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonSave),
        //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonSaveAndClose),
        //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonDelete),
        //new DevExpress.XtraBars.LinkPersistInfo(this.ButtonChoice),
        //new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.None, false, this.ButtonTest, false),
        //new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4),
        //new DevExpress.XtraBars.LinkPersistInfo(this.btnSaveSettings),
        //new DevExpress.XtraBars.LinkPersistInfo(this.btnLoadSettings),
        //new DevExpress.XtraBars.LinkPersistInfo(this.btnExtParams)});

        internal static class AVEditMask
        {
            public const string N2 = "N2";
        }
        internal static class AVHAlignArray
        {
            public static string[] All = { AVHAlign.Left, AVHAlign.Right, AVHAlign.Center };
        }
        internal static class AVRowSelector
        {
            public const string Parent = "parent";
            public const string IsGrset = "isgrset";
            public const string Prev = "prev";
            public const string PrevSibling = "prev-sibling";
            public const string PrevRow = "prev-row";
        }

        internal static class AVParam
        {
            public const string FormValid = "is_form_valid";
            public const string RowsLimit = "p_rows_limit";
            public const string FormValidNot = "is_form_valid_not";
            public const string HasChanges = "_has_changes";
        }
        internal static class AVArrayParamModes
        {
            internal const string Auto   = "auto";
            internal const string Inline = "inline";
            internal const string Store  = "store";
        }
        internal static class AVParamTypes
        {
            public const string Condition = "condition";
        }

        internal static class AVConstrDelOptionsArray
        {
            public static string[] All = { AVConstrDelOptions.Cascade, AVConstrDelOptions.SetNull };
        }

        internal static class AVConstrDelOptions
        {
            public const string Cascade = "cascade";
            public const string SetNull = "set null";
        }

        internal static class AVParamTypesArray
        {
            public static string[] All = { AVParamTypes.Condition };
        }

        internal static class AVViewModes
        {
            public const string None = "none";
            public const string Empty = "empty";
            public const string Default = "default";
            public const string Pivot = "pivot";
            public const string Tree = "tree";
            public const string Excel = "excel";
            public const string DashboardDesigner = "ddesigner";
            public const string DashboardViewer = "dviewer";
        }

        internal static class AVViewModesArray
        {
            public static string[] All = {AVViewModes.Default, AVViewModes.Pivot, AVViewModes.Tree, AVViewModes.Excel, AVViewModes.DashboardDesigner, AVViewModes.DashboardViewer};
        }

        internal static class AVParamArray
        {
            public static string[] FormExtPars = new string[] { AVParam.FormValid, AVParam.FormValidNot };
            public static string[] TableExtPars = new string[] { AVParam.HasChanges };

        }
        internal static class AVActionRows
        {
            public const string All = "all";
            public const string Selected = "selected";
            public const string Current = "current";

        }
        internal static class AVActionRowsArray
        {
            public static string[] All = new string[] { AVActionRows.All, AVActionRows.Current, AVActionRows.Selected };

        }
        internal static class AVFuncArray
        {
            public static string[] AndOr = new string[] { AVFunction.Or, AVFunction.And };
            public static string[] TrueFalse = new string[] { AVFunction.True, AVFunction.False };

        }

        internal static class AVSpecType
        {
            public const string ColSets = "colsets";
            public const string SelectRep = "selectrep";

        }

        internal static class AVSpecTable
        {
            public const string File = "file";
        }

        internal static class AVSides
        {
            public const string Left = "Left";
            public const string Right = "Right";
        }

        internal static class AVSidesArray
        {
            public static string[] All = { AVSides.Left, AVSides.Right };
        }



        internal static class AVSpecColumn
        {
            public const string FileName = "file-name";
            public const string FileSize = "file-size";
            public const string FileData = "file-data";
            public const string RowId = "rowid";
        }
      
        internal static class AVSpecColumnGrset
        {
            //с именами этих колонок путница, навести порядок, не везде используются эти константы
            public const string GroupingId = "groupingid";
            public const string ParentGroupingId = "par_groupingid";
            public const string GrSetName = "grsetid";
            public const string OrigGrSetName = "origgrsetid";
            public const string ParentGrSetId = "parent_grsetid";
            public const string GrRowId = "growid";
            public const string GrRowNum = "gr_rn";
            public const string ParentGrRowId = "parent_growid";
            public const string OnRowsGrSetId = "onrowsgrsetid";
            public const string OnRowsGrRowId = "onrowsgrowid";
            public const string OnColsColId = "oncolscolid";
            public const string OnColsGrSetId = "oncolsgrsetid";
            public const string GrSetTitle = "grsetname";
            public const string GrTreeLevel = "gr_tree_level";
        }

      
        internal static class TreeSources
        {
            public const string Self = "self";
            public const string Child = "child";
            
        }

        internal static class TreeSourcesArray
        {

            public static string[] All =
            {
                TreeSources.Self, TreeSources.Child
            };
            
        }

        internal static class AVEventName
        {
            public const string Click = "click";
            public const string DoubleClick = "double-click";
            public const string CheckedRowSave = "checked-row-save";
            public const string RowSave = "row-save";
            public const string NewRowSave = "new-row-save";
            public const string SaveOrSaveAndClose = "save-or-saveclose";
            public const string Save = "save";
            public const string FormLoaded = "form-loaded";
            public const string ObjectSave = "object-save";
            public const string InsteadObjectSave = "instead-object-save";
            public const string InsteadObjectDelete = "instead-object-delete";
            public const string ObjectRangeSave = "object-range-save";
        }

        internal static class AVEventNameArray
        {
            public static string[] All =
            {
                AVEventName.Click, AVEventName.DoubleClick,AVEventName.RowSave,AVEventName.NewRowSave,AVEventName.CheckedRowSave,AVEventName.FormLoaded,AVEventName.Save,AVEventName.SaveOrSaveAndClose,AVEventName.ObjectSave,AVEventName.InsteadObjectSave,AVEventName.InsteadObjectDelete,AVEventName.ObjectRangeSave
            };
        }
        internal static class ANameArray
        {
            // См. APredicate.IsLayoutOptions
            internal static string[] AllLayoutOptions = {
                AName.Size, AName.MinSize, AName.MaxSize, AName.Position, AName.TextLocation, AName.TextVisible, AName.IsLayoutBlock, AName.WidthPerc, AName.WidthFixed, AName.FixedSide, AName.FillHeight
            };
            /*
            // См. APredicate.IsCustomLayoutOptions
            internal static string[] CustomLayoutOptions = {
                AName.Size, AName.MinSize, AName.MaxSize, AName.Position
            };*/
            // См. APredicate.IsBehaviorColumns
            internal static string[] BehaviorColumns = {
                AName.Default, AName.Editable, AName.Valid, AName.TextSource,AName.Mandatory, AName.Visible,AName.NewVal, AName.FontColor
            };
            internal static string[] RoBehaviorColumns = {
                AName.Visible, AName.FontColor
            };
            // См. APredicate.IsColumnRecoveredAttribute
            /*internal static string[] ColumnRecoveredAttributes = {
                AName.Title, AName.Agg, AName.ClassTitle // дополнить
            };*/
        }
        internal static class AVHint
        {
            public const string Materialize = "materialize";

        }

        internal static class AVDataType
        {
            public const string Date = "date";
            public const string Number = "number";
            public const string Bool = "bool";
            public const string String = "string";
            public const string Clob = "clob";
			public const string Blob = "blob";
            public const string Array = "array";
            public const string Variant = "variant";
        }

        internal static class AVMaterializeType
        {
            public const string Hint = "hint";
            public const string TempTable = "temp-table";
           
        }

        internal static class AVControlType
        {
            public const string Check = "UICheck";
            public const string Text = "UIText";
            public const string TextEx = "UITextEx";
            public const string Date = "UIDate";
            public const string DateTime = "UIDateTime";
            public const string Number = "UINumber";
            public const string Combo = "UICombo";
            public const string List = "UIList";
            public const string File = "UIFile";
            public const string Link = "UILink";
            public const string Custom = "UICustom";
            public const string TextArray = "UITextArray";
        }

        internal static class AVControlTypeArray
        {
            public static string[] All =
            {
                AVControlType.Check, AVControlType.Text, AVControlType.TextEx,AVControlType.TextArray, AVControlType.Date, AVControlType.DateTime,
                AVControlType.Number, AVControlType.Combo, AVControlType.List, AVControlType.File, AVControlType.Link
            };
        }

        internal static class AVTable
        {
            public const string Dual = "dual";
            public const string Ths = "this";
            public const string Pars = "pars";
        }



        internal static class AVTypeArray
        {
            public static string[] Real =
            {
                AVDataType.Date, AVDataType.Number, AVDataType.String, AVDataType.Clob, AVDataType.Blob
            };
        }

        internal static class AVJoin
        {
            public const string Inner = "inner";
            public const string LeftOuter = "left outer";
            public const string Cross = "cross";
        }

        internal static class AVActionType
        {
            public const string ExecuteAdd = "execute-add";
            public const string ExecuteUpdate = "execute-update";
            public const string ExecuteDelete = "execute-delete";
            public const string ExecuteCopyByReport = "execute-copy-by-report";
            public const string ExecuteInsertByReport = "execute-insert-by-report";
            public const string DynamicForm = "dynamic-form";
            public const string DynamicFormCreate = "dynamic-form-create";
            public const string DynamicFormCreateMultiple = "dynamic-form-create-multiple";
            public const string DynamicFormForSelect = "dynamic-form-for-select";
            public const string Form = "form";
            public const string Refill = "refill";
            public const string AcceptSelection = "accept-selection";
            public const string ClientUpdate = "client-update";
            public const string ClientAddByForm = "client-add-by-form";
            public const string AddByClientMethod = "add-by-client-method";// не использовалось, не проверено
            public const string CreateByClientMethod = "create-by-client-method";
            public const string ClientDeleteRow = "client-delete-row"; // удаляет строку визуально, на сохранение не влияет
            public const string ClientRemoveRow = "client-remove-row"; // тоже самое что нажание на кнопку удаления в тулбаре, можно сохранить
            public const string SaveAndClose = "save-and-close";
            public const string Save = "save";
            public const string Close = "close";
            public const string RefreshForm = "refresh-form";
            public const string RefreshTable = "refresh-table";
            public const string RefreshColumn = "refresh-column"; // с учетом не сохраненных изменений 
            public const string ResetColumn = "reset-column"; // только из БД
            public const string Custom = "custom";
            public const string ShowSubForm = "show-sub-form";
            public const string HideSubForm = "hide-sub-form";
            public const string CallClientMethod = "call-client-method";
            public const string GetValWithClientMethod = "get-val-with-client-method"; // заполнение значения UIList, вроде еще не используется
            public const string CallPlsql = "call-plsql";
            public const string CallPlsqlAdd = "call-plsql-add";// возвращает код
            public const string OpenReport = "open-report";
            public const string OpenExpressReport = "open-express-report";
            public const string OpenGrDetailReport = "open-grouping-detail";
            public const string OpenColGrDetailReport = "open-column-grouping-detail";
			public const string AddSelected = "add-selected";
			public const string RemoveSelected = "remove-selected";
			public const string CopyFieldToClipboard = "save-field-to-clipboard";
			public const string FillFieldFromClipboard = "fill-field-from-clipboard";
			public const string ShowPopupField = "show-popup";
           
        }

        internal static class AVTitle
        {
            public const string Add = "{+}";
            public const string No = "-";

        }

        internal static class AVActionTypeArray
        {
            public static string[] All =
        {
            AVActionType.DynamicForm,AVActionType.DynamicFormCreate,AVActionType.DynamicFormCreateMultiple,AVActionType.DynamicFormForSelect, AVActionType.Form, AVActionType.Refill, AVActionType.AcceptSelection,  AVActionType.Custom
            ,AVActionType.ClientUpdate,  AVActionType.ClientAddByForm,AVActionType.Save,  AVActionType.Close,  AVActionType.SaveAndClose,  AVActionType.RefreshForm
            ,  AVActionType.RefreshTable
              ,  AVActionType.RefreshColumn
               ,  AVActionType.ResetColumn
            ,AVActionType.ExecuteAdd
            ,AVActionType.ExecuteDelete
            ,AVActionType.ClientDeleteRow
              ,AVActionType.ClientRemoveRow
            ,AVActionType.ShowSubForm
            ,AVActionType.HideSubForm
            ,AVActionType.CallClientMethod
             ,AVActionType.AddByClientMethod
                ,AVActionType.CreateByClientMethod
              ,AVActionType.GetValWithClientMethod
            ,AVActionType.CallPlsql
            ,  AVActionType.CallPlsqlAdd
            ,AVActionType.OpenReport
            ,AVActionType.OpenExpressReport
             ,AVActionType.OpenGrDetailReport
            ,AVActionType.OpenColGrDetailReport
            ,AVActionType.ExecuteUpdate
           ,AVActionType.AddSelected
           ,AVActionType.RemoveSelected
           ,AVActionType.ExecuteCopyByReport
		   ,AVActionType.CopyFieldToClipboard
		   ,AVActionType.FillFieldFromClipboard
		   ,AVActionType.ShowPopupField
            ,AVActionType.ExecuteInsertByReport
        };
            public static string[] WithThisFormGroupControl =
        {
             AVActionType.ShowSubForm
        };
			public static string[] WithThisFormFieldControl =
        {
             AVActionType.FillFieldFromClipboard, AVActionType.CopyFieldToClipboard, AVActionType.ShowPopupField
        };

            public static string[] Outer =
        {
             AVActionType.Form
        };
            public static string[] WithQuery =
        {
             AVActionType.Refill,AVActionType.ClientUpdate,AVActionType.ExecuteAdd,AVActionType.ExecuteUpdate,AVActionType.ExecuteDelete
        };

            public static string[] WithTargetTable =
        {
            AVActionType.ExecuteAdd,AVActionType.DynamicFormForSelect
        };
            public static string[] WithForm =
        {
             AVActionType.ClientAddByForm, AVActionType.DynamicForm, AVActionType.DynamicFormCreate,AVActionType.DynamicFormCreateMultiple,AVActionType.DynamicFormForSelect
        };

        public static string[] WithReport =
        {
             AVActionType.OpenReport,AVActionType.OpenGrDetailReport,AVActionType.OpenColGrDetailReport,AVActionType.OpenExpressReport,AVActionType.ExecuteCopyByReport,AVActionType.ExecuteInsertByReport
        };
        
        public static string[] Custom =
        {
             AVActionType.Custom
        };

            public static string[] WithColumn =
        {
             AVActionType.RefreshColumn,AVActionType.ResetColumn
        };

            public static string[] RowActions =  // Первый параметр первичный ключ
        {
             AVActionType.ClientUpdate,AVActionType.DynamicForm
        };
			public static string[] DetailReport =
			{
				AVActionType.OpenGrDetailReport, AVActionType.OpenColGrDetailReport
			};
        }

        internal static class AVFunction
        {
            public const string Bitand = "bitand";
            public const string Array = "array";
            public const string Max = "max";
            public const string Decode = "decode";
            public const string Equal = "=";
            public const string EqualNvl = "=nvl";
            public const string NotEqual = "!=";
            public const string NotEqualNvl = "!=nvl";
            public const string Dummy = "()";
            public const string Exists = "exists";
            public const string IsNull = "is null";
            public const string IsNotNull = "is not null";
            public const string NullIf = "nullif";
            public const string And = "and";
            public const string Or = "or";
            public const string True = "true";
            public const string False = "false";
            public const string In = "in";
            public const string NotIn = "not in";
            public const string InSNull = "in snull";
            public const string LikeSNull = "like snull";
            public const string InNNull = "in nnull";
            public const string Less = "lt";
            public const string LessOrEqual = "le";
            public const string Greater = "gt";
            public const string GreaterOrEqual = "ge";
            public const string If = "if";
            public const string Concat = "||";
            public const string ToChar = "to_char";
            public const string YmToChar = "ym to char";
            public const string Ym2ToChar = "ym2 to char";
            public const string Over = "over";
            public const string DenseRank = "dense_rank";
            public const string RowId = "rowid";
            public const string RowNum = "rownum";
            public const string RowNumber = "row_number";
            public const string PartitionBy = "partition by";
            public const string Grouping = "grouping";
            public const string GroupingId = "grouping_id";
            public const string OrderBySimple = "order by simple";
            public const string OrderBy2 = "order by 2";
            public const string Lead = "lead";
            public const string Sum = "sum";
            public const string LastValue = "last_value";
            public const string Coalesce = "coalesce";
            public const string Case = "case";
            public const string When = "when";
            public const string Elese = "else";
            public const string PlusNvl = "+nvl";
            public const string MinusNvl = "-nvl";
            public const string Div = "/";
            public const string Multiply = "*";
            public const string Neg = "0-";
            public const string Listagg = "listagg";
        }

        internal static class AVBool
        {
            public const string True = "1";
            public const string False = "0";
        }

        internal static class AVGroup
        {
            public const string Sum = "sum";
            public const string Max = "max";
            public const string Min = "min";

            public const string Empty = "";
            public const string No = "no";
            public const string StrAgg = "stragg";
            public const string StrAggDist = "stragg_dist";
            public const string Outer = "outer";
            public const string Group = "1";
            public const string Inner = "/*inner*/"; // "1"- на колонке и "/*inner*/" вместе с функцией listagg в выражении - реализация перечисления через разделитель
            public const string List = "list"; // цель таже но с предопределенным разделителем "; " и сортировкой - по значению

            //public const string SumKeepLast = "sum keep last";
        }

        internal static class AVAggArray
        {
            public static string[] ForAgg = { AVGroup.Sum, AVGroup.Max, AVGroup.Min, AVGroup.No, AVGroup.StrAgg, AVGroup.StrAggDist, AVGroup.Group, AVGroup.List };
            public static string[] ForAggCml = { AVGroup.Sum, AVGroup.Max, AVGroup.Min, AVFunction.LastValue };
        }

        internal static class AVCallType
        {
            public const string Doubleclick = "doubleclick";
            public const string PopupMenu = "popupmenu";
        }


        internal static class TVType
        {
            public const string Date = "Дата (ДД.ММ.ГГГГ)";
            public const string Link = "Ссылка";
            public const string String = "Строка";
            public const string Number = "Число";
            public const string Check = "Признак";
        }

        internal static class TVSource
        {
            public const string ValList = "Список значений";

        }

        internal static class TVValue
        {
            public const string Yes = "Да";
            public const string No = "Нет";
            public const string Checked = "Установлен";
            public const string NotChecked = "Не установлен";
        }
        internal static class AVColumn
        {
            internal const string All = "*";
            internal const string IsNew = "is_new";
            internal const string IsNotNew = "is_not_new";
            internal const string Dummy = "dummy";
            internal const string Sid = "sid";
            internal const string SparentId = "sparentid";
            internal static bool IsNotRepDsSysColumn(string col_name)
            {
                return (col_name != Sid) && (col_name != SparentId);
            }
            internal static bool IsNotSysColumn(string col_name)
            {
                return IsNotRepDsSysColumn(col_name) && (col_name != IsNew) && (col_name != IsNotNew);
            }
        }
        internal static class AVColumnArray
        {
            public static string[] SysColNamesForEditedObject =
            {
                AVColumn.IsNew, AVColumn.IsNotNew
            };
            /*public static string[] RepDsSysColumns =
            {
                AVColumn.Sid, AVColumn.SparentId
            };*/
            /*public static string[] SysColumns =
            {
                AVColumn.Sid, AVColumn.SparentId, AVColumn.IsNew, AVColumn.IsNotNew
            };*/
        }
        internal static class PInfo
        {
            //public const string Search = "Search";
            //public const string Lookup = "Lookup";
            //public const string Self = "Self";
            //public const string TSelf = "(Собст.)";
            public const string Title = "Title";
            public const string IsHtml = "IsHtml";
            public const string Editable = "Editable";
            public const string UsedEl = "UsedEl";
            public const string Source = "Source";
        }

        internal static class RegPath
        {
            public const string SchemeEditor = "schemeeditor";
            public const string SchemeOpenItems = SchemeEditor + @"\openitems";
            public const string ScemeEditorLayout = SchemeEditor + @"\layout";
            public const string Forms = "forms";
        }

        internal static class RegVal
        {
            //public const string OpenItems = "openitems";
            public const string Global = "global";
            public const string Size = "size";
        }


        internal static class Pfx
        {

            public const string ExtValName = "_x_n";
            public const string AddDim = "_a_d";
            public const string AddDim1 = "_a_d1";
            public const string Param = ":";
            public const string GlobParam = "glbl_";
            public const string QubeCounter = "_q_c";
            public const string DubDlinkCond = "_cnd";
            public const string DubDlinkPush = "_psh";
            public const string ExtLink = "_ext";
            public const string Materialized = "_mat_";
            public const string PrimaryKeyParam = "_prm";

            public const string ForegnKeyParam = "fk_";
            public const string Id = "id_";
            public const string CurValParam = "_cv";
            public const string FieldStateColumn = "_fst";
            public const string CutedId = "z23df_";
            public const string QubeQueryAlias = "qube";
            public const string CumulNext = "_nxt";
            public const string ScopeExp = "_sce_";
            public const string DsExp = "_ds_";
            public const string MpVariant = "_mpv_";
            public const string Dimset = "dimset";
            public const string BehaviorPropCol = "Column";
            public const string BehaviorPropInv = "Invert";
            public const string BehaviorPropRes = "Result";
            public const string BehaviorClient = "Client";
            public const string BehaviorSource = "Source";
            public const string BehaviorDependants = "Dependants";
            public const string Level = "_lvl";
            public const string ParamVar = "p_";
            public const string Ovr = "ovr_";

        }


        internal static class DBParams
        {


            public const string IsNewRowParam = "new_row";
            public const string FormId = "form_id";
            public const string RowStateId = "row_state_id";
            public const string TempRowId = "temp_row_id";
            public const string FileId = "file_id";
            public const string FileSize = "file_size";
            public const string FileName = "file_name";
            public const string FileData = "file_data";
            public const string PrimaryKeyParam = "pk_prm";
            public const string ObjNameParam = "obj_name";
        }

        internal static class DBObjects
        {
            public const string TempTable = "rr_temp";
            public const string TempTableTableIdColumn = "skod";
            public const string TempTableRowIdColumn = "f2";
            public const string TempTableStateColumn = "f3";
            public const string TempTableFormIdColumn = "names";
        }

        internal static class DBObjectsArray
        {
            public static string[] TempTableSpecCols =
        {
             DBObjects.TempTableFormIdColumn,  DBObjects.TempTableRowIdColumn,  DBObjects.TempTableStateColumn,  DBObjects.TempTableTableIdColumn
        };

        }


        internal static class SpecCols
        {

            public const string Check = "sp_col_check";
            public const string Name = "sp_col_name";
            public const string ParentGRowId = "parent_growid";// дубль ?
            public const string GRowId = "growid";// дубль ?


        }

        internal static class SpecColsTitle
        {

            public const string Check = "Выбор";

        }

        internal static class GridButton
        {
            public const string Add = "grid-add";
            public const string Copy = "grid-copy";
        }
        internal static class GridButtonControl
        {
            public const string Commit = "ButtonCommit";
            public const string Choice = "ButtonChoiceRow";

        }

        internal static class SchEdirorFieldGr
        {
            public const string Main = "Главная";
            public const string MainOther = "Главная/Другие свойства";
            public const string MainMain = "Главная/Основные свойства";
            public const string Cube = "Cube";
            public const string Layout = "Layout";
            public const string Part = "Part";
            public const string Behavior = "Поведение";
            public const string BehaviorVisibile = "Поведение/Видимость";
            public const string BehaviorEditable = "Поведение/Активность";
            public const string BehaviorColor = "Поведение/Цвет";
            public const string BehaviorSelect = "Поведение/Выбор строк";
            public const string BehaviorRequired = "Поведение/Обязательность";
            public const string BehaviorDefault = "Поведение/Значение по умолчанию";
            public const string BehaviorNewVal = "Поведение/Вычисляемое значение";
            public const string BehaviorValid = "Поведение/Валидация";
            public const string BehaviorTextSource = "Поведение/Текст";
            public const string BehaviorExists = "Поведение/Доступ";
            public const string Documenting = "Документация";
            public const string Appearance = "Внешний вид";
        }

        internal static class AVTimeAttr
        {

            public const string Name = "name";
            public const string BeginTime = "beg_time";
            public const string EndTime = "end_time";
            public const string Val = "val";
           

        }

        internal static class AVTimeAttrArray
        {
            public static string[] All = { AVTimeAttr.Val, AVTimeAttr.Name, AVTimeAttr.BeginTime, AVTimeAttr.EndTime };


        }

        internal static class AVTimeType // задумывалось для времени, но может использоваться для любых констант
        {

            public const string Day = "day";
            public const string Month = "month";
            public const string Month2 = "month2";
            public const string Year = "year";
            public const string Str = "string";
            public const string Num = "number";

        }

        internal static class AVTimeTypeArray
        {
            public static string[] All = { AVTimeType.Day, AVTimeType.Month, AVTimeType.Month2, AVTimeType.Year, AVTimeType.Str, AVTimeType.Num };
        }

        internal static class AVFixedSide
        {
            public const string Left = "left";
            public const string Right = "right";
        }

        internal static class AVFixedSideArray
        {
            public static string[] All = { AVFixedSide.Left, AVFixedSide.Right };
        }

        //internal const string FormCacheFileName = "cache.zip";
        internal static class APostReportArray
        {
            public static string[] All = { AName.CellsMerge, AName.CellsDelete, AName.RowsHeight, AName.PagesBreak };
        }

        internal static class ExcelMarks
        {
            internal const string MergeDown = "[merge_down]";
            internal const string MergeStart = "[merge_start]";
            internal const string MergeRight = "[merge_right]";
            internal const string HeadMarker = "headmarker";
            internal const string DeleteRanges = "!delete";
            internal const string DeleteRow = "!deleterow";
            /// <summary>
            /// Синтаксис: !columnwidth:&lt;число&gt;
            /// </summary>
            /// <seealso cref="Printing.SetColumnsWidth"/>
            internal const string ColumnWidth = "!columnwidth";
            /// <summary>
            /// Синтаксис: !rowheight:&lt;число&gt;
            /// </summary>
            /// <seealso cref="Printing.SetRowsHeight"/>
            internal const string RowHeight = "!rowheight";
            internal const string AutoRowHeight = "!autorowheight";
            internal const string NoAutoRowHeight = "!noautorowheight";
            internal const string AllColsAutoFit = "!allcolumsautofit";
            internal const string RowID = "!rowid";
            internal const string ProtectSheet = "!protectsheet";
            internal const string PageBreak = "!pagebreak";
            internal const string MergeIgnore = "!mergeignore";
            internal const string PrintTitleRows = "!printtitlerows";
        }

        internal static class NullConsts
        {
            public const string SNULL = "~~~~";
            // потому что ym
            public const string NNULL = "4178.05";
            public const decimal nnullVal = 4178.05m;
            public const string DNULL = "11.11.1112";
        }

        public static string NullPlaceholder = "(пустые)";
    }
}
