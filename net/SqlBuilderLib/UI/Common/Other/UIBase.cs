using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Mask;
//using DevExpress.XtraEditors.Repository;

//using infoenergo.core.Extensions;
using Microsoft.Win32;
using sql.builder.DataApi;
using sql.builder.XmlHelpers;
using sql.builder.Exceptions;
//using sql.builder.WebReports;

namespace sql.builder.UI
{
    internal abstract partial class UIBase : IBase
    {
        #region поля
        private ReturnType source_type;
        private Type value_type;
        protected XElement xfield;
        private bool used;
        private bool allow_manual_used_set;
        private bool change_source_immediately; // при любом изменении в репозитории сразу сохранять значение в DataSource
        private string field_name;
        private string table_name;
        private string full_name;
        private string query_name;
        private string query_name_default;
        private string special_type;
        private string сaption;
        protected bool mandatory;
        //private bool default_visible;
        private bool null_as_undefined;
        private bool show_nulls;
        protected int rows_limit;
        private string condition_param_name;
        protected string key_field_name;
        protected string name_field_name;
        protected string search_field_name;
        protected string parent_field_name;
        protected string value_field_name;
        public UIFormC Form;
        public Dictionary<string, UIBase> Dependants;
        public Dictionary<string, UIBase> Masters;
        public bool isInGrid = false;
        #endregion
        public virtual bool IsBool()
        {
            return false;
        }
        public virtual bool IsStringToArray()
        {
            return false;
        }
        internal bool GetUsed()
        {
            if (!this.ShowCheck) {
                return true;
            } else {
                return Used;
            }
        }
        protected void MarkUsed(bool value)
        {
            if (SourceType == ReturnType.Array) {
                //if (Form.DataSource.ParamsTable.Columns.Contains(FieldName))
                //{
                //    (Form.DataSource.ParamsTable.Columns[FieldName] as VDataColumn).ParamUsed = value;
                //}
                var tbl = this.Form.DataSource.ArrayValueTable(this.field_name);

                if (tbl != null)
                {
                    this.Form.DataSource.ArrayValueTable(this.field_name).ParamUsed = value;
                }
               
            }
        }
        public bool Used {
            get {
                return this.used;
            }
            set {
                //if (this is UICheck)
                //{
                //    ceUsed.Checked = value;
                //    return;
                //}

                if (UseType != UIFormC.UseType.ParamEditor) return;
                this.used = value;
                if (!(this is UICheck)) SetChecked(value);
                bool value1 = this.used;
                MarkUsed(value1);
                
            }
        }
        public UIFormC.UseType UseType {
            get {
                if (this.Form.FormUseType == UIFormC.UseType.DataEditor && string.IsNullOrEmpty(this.table_name)) {
                    return UIFormC.UseType.ParamEditor;
                } else {
                    return this.Form.FormUseType;
                }
            }
        }
        private bool _show_check;
        public bool ShowCheck
        {
            get
            {
                return _show_check;
            }
            set
            {
                _show_check = value;
                if (this is UICheck) return;

                //SetCheckVisible(value);
                //ceUsed.Visible = value;
                //pSettings.Visible = value;
            }
        }
        // Общие
        internal ReturnType SourceType { get { return this.source_type; } }
        internal Type ValueType { get { return this.value_type; } }
        public XElement XField { get { return this.xfield; } }
        public string FullName { get { return this.full_name; } set { this.full_name = value; } }
        public string FieldName { get { return this.field_name; } }
        public string Caption { get { return this.сaption; } set { this.сaption = value; } }
        /// <summary>
        /// Признак обязательного поля
        /// </summary>
        public bool Mandatory { get { return this.mandatory; } }
        //public bool DefaultVisible { get { return this.default_visible; } }
        //public string GroupTitle { get; set; }
        public string SpecialType { get { return this.special_type; } }
        public string TableName { get { return this.table_name; } }
        protected string QueryName { get { return this.query_name; } }
        private bool useColPreset = false; // временно, если срачтется признак убрать оставить новую логику для всех случаев
        //public string QueryNameDefault { get { return this.query_name_default; } }

        public string EditMask { get; private set; }
        public string MaxLength { get; private set; }
        public bool UseDefaultQuery { get; set; }
        //public bool NullAsUndefined { get; private set; }
        //public bool StoreInDB { get; private set; }

        // Реестр
        protected Dictionary<string, object> UserSettings;
        private string _reg_path;

        // Array
        // protected readonly int _filter_delay = 200;
        public bool ShowNulls { get { return this.show_nulls; } }
        public int RowsLimit { get { return this.rows_limit; } }
        public bool _need_refresh = true;
        public bool _need_get_data = true;

        public void SetNeedGetData(bool value) //временно
        {
            _need_get_data = value;
        }

        public bool _need_put_data = true;

        protected Dictionary<string, string> FilterValues;

        public bool ChangeSourceImmediately { get { return this.change_source_immediately; } }
        
        #region События
        public delegate XElement NeedMasterValuesHandler(UIBase sender);
        public event NeedMasterValuesHandler NeedMasterValues;
        public event EventHandler EditValueChanged;
        public event Action<string, object> SpecialTypeChanged;
        #endregion
        #region Открытые методы
        public UIBase()
        {
            //InitializeComponent();
        }
        private void InitVaribles()
        {
            this.UserSettings = new Dictionary<string, object>();
            this.Dependants = new Dictionary<string, UIBase>();
            this.Masters = new Dictionary<string, UIBase>();
            //Bindings = new List<VBinding>();
            this.FilterValues = new Dictionary<string, string>();
            this.field_name = this.xfield.Attribute(AName.name).Value;
            this.сaption = this.xfield.AttrOrEmpty(AName.title);
            this.mandatory = this.xfield.AttrOrDefault(AName.mandatory, false);
            //this.default_visible = this.xfield.AttrOrDefault(AName.visible, true);
            this.special_type = this.xfield.AttrOrDefault(AName.special_type, "no");
            this.table_name = this.xfield.AttrOrEmpty(AName.table);
            this.null_as_undefined = this.xfield.AttrOrDefault(AName.null_as_undefined, false);
            //StoreInDB = (XField.AttrOrDef(TextConst.AName.StoreInDB, "0") == "1");
            if (string.IsNullOrEmpty(this.table_name)) {
                this.full_name = this.field_name;
            } else {
                this.full_name = this.table_name + "." + this.field_name;
            }
            //this.SetRootName(this.full_name);
            this.EditMask = this.xfield.AttrOrDefault(AName.edit_mask, null);
            this.MaxLength = this.xfield.AttrOrDefault(AName.max_length, null);
            //
            XElement xlistquery = this.xfield.Element(EName.listquery);
            if (xlistquery != null) {
                Cmn.CopyAttributesNoReplace(xlistquery, this.xfield); // val-field-name может быть на listquery
            }
            if (xlistquery != null) {
                this.query_name = xlistquery.Element(EName.query).AttrOrEmpty(AName.name);
                if (this.Form.FormUseType == UIFormC.UseType.DataEditor) {
                    this.useColPreset = true; // теперь попробуем для всех DataEditor
                }
                //useColPreset = (Cmn.GetAttrValue(xlistquery, TextConst.AName.UseColPreset) == TextConst.AVBool.True);
            } else {
                this.query_name = null;
            }
            //
            XElement xdefaultquery = this.xfield.Element(EName.defaultquery);
            if (xdefaultquery != null) {
                this.query_name_default = xdefaultquery.Element(EName.query).AttrOrEmpty(AName.name);
            } else {
                this.query_name_default = this.xfield.AttrOrDefault(AName.valuequery, null);
            }
            this.UseDefaultQuery = !string.IsNullOrEmpty(this.query_name_default);
            // Остальные анализировать не обязательно
            this.rows_limit = this.xfield.AttrOrDefault(AName.rows_limit, 0);
            this.value_field_name = this.xfield.AttrOrDefault(AName.val_field_name, null);
            this.name_field_name = this.xfield.AttrOrDefault(AName.name_field_name, null);
            this.search_field_name = this.xfield.AttrOrDefault(AName.search_field_name, null);
            this.parent_field_name = this.xfield.AttrOrDefault(AName.parent_field_name, null);
            this.show_nulls = this.xfield.AttrOrDefault(AName.show_nulls, false);
        }
        public void OnFocusLost()
        {
            if (_warningText != "")
            {
                var s = _warningText;
                _warningText = "";
                //sql.builder.WinForms.ShowMessage.ShowExclamation(s);
            }
        }
        private void InitControls()
        {
        }
        private bool ChangeProcessing = false;
        public void Changed()
        {
            Changed(!HasValue());   
        }
        public void Changed(bool is_null)
        {
            ChangeProcessing = true;
            if (!is_null)
            {
                Used = true;
            }
            else
            {
                if (this.GetType().Name == typeof(UIList).Name)
                {
                    Used = false;
                }
            }

            foreach (var dep in Dependants.Values)
            {
                if (!dep.ChangeProcessing)
                {
                    dep.RefreshData();
                }
            }
            ChangeProcessing = false;
        }
        #endregion
        #region Обработчики событий
        public void OnSpecialTypeChanged(string special_type, object data)
        {
            if (SpecialTypeChanged != null)
            {
                SpecialTypeChanged(special_type, data);
            }
        }


        public virtual void OnCheckedChanged()
        {
        }


        private void onCheckedChanged(/*object sender, EventArgs e*/)
        {
        }
        protected XElement OnNeedMasterValues(UIBase sender)
        {
            if (this.NeedMasterValues != null) {
                return this.NeedMasterValues(sender);
            } else {
                return new XElement(EName.@params);
            }
        }
        #endregion
        #region Закрытые методы
        protected virtual void BeginUpdate()
        {
        }
        protected virtual void EndUpdate()
        {
        }
        protected bool GetSourceReadOnly(DataRow row = null)
        {
            var column = GetBoundColumn();
            // для UIList в редакторе параметров
            if (column == null) return false;

            row = row ?? ((VDataTable)column.Table).CurrentRow;
            return !column.GetEditable(row);
        }

        public bool IsEditable()
        {
            return !GetSourceReadOnly();
        }
        protected void PrepareList(DataTable dt)
        {
            DataColumnCollection cols = dt.Columns;
            if (cols.Count == 0) {
                return;
            }
            if (dt.HasPrimaryKey()) {
                this.key_field_name = dt.PrimaryKey[0].ColumnName;
            } else {
                this.key_field_name = cols[0].ColumnName;
            }
            if (this.value_field_name == null) {
                this.value_field_name = this.key_field_name;
            }
            this.value_type = cols[this.value_field_name].DataType;
            if (this.name_field_name == null) {
                DataColumn column;
                int index;
                for (index = 0; index < cols.Count; index++) {
                    column = cols[index];
                    if (UIBase.IsColumnShouldBeVisible(column)) {
                        this.name_field_name = column.ColumnName;
                        break;
                    }
                }
                if (this.name_field_name == null) {
                    // первая колонка, которая не является ключевой
                    for (index = 0; index < cols.Count; index++) {
                        column = cols[index];
                        if (column.ColumnName != this.key_field_name) {
                            this.name_field_name = column.ColumnName;
                            break;
                        }
                    }
                }
            }
            if (this.search_field_name == null) {
                this.search_field_name = this.name_field_name;
            }
            // если колонка с именем не строкового типа - преобразуем ее в string
            //if (dt.Columns[NameFieldName].DataType != typeof(string))
            //{
            //    GridDesigner.ConvertColumnType(dt, NameFieldName, typeof(string));
            //}
            if (!dt.Columns.Contains("check")) {
                DataColumn column = new DataColumn("check", typeof(int));
                column.Caption = "Выбор";
                column.DefaultValue = Cmn.INT32_ONE;
                dt.Columns.Add(column);
            }
            if (!dt.Columns.Contains("absent")) {
                DataColumn column = new DataColumn("absent", typeof(bool));
                column.DefaultValue = Cmn.BOOLEAN_FALSE;
                dt.Columns.Add(column);
            }
            if (this.show_nulls) {
                AddNullValue(dt);
            }
        }
        protected static bool IsColumnShouldBeVisible(DataColumn col)
        {
            return (col.Caption != "" && (col.ColumnName != col.Caption));
        }
        //private bool _valueSourceLoaded = false;



        private VDataSet createListSourceColsets()
        {
            VDataSet data = null;

            var scheme = Form.UIForm_GetReportScheme();
            data = XmlReports.Environment.GetPrecompiledReport(CreateColSetsQuery(scheme)).Result(2, false);

            return data;
        }
        private VDataSet createListSourceForQueryEditor()
        {
            var vcol = (VDataColumn)(Form.DataSource.GetTable(this.table_name)).Columns[this.field_name];
            if (vcol != null) {
                return vcol.SelectionList;
            } else {
                return null;
            }
        }
        private VDataSet createListSource()
        {
            VDataSet data;

            // костыль для web
            if (this is UIList || this is UICombo)
            {
                if (this.query_name == null)
                {
                    this.query_name = this.query_name_default;
                }
            }

            if (this.special_type == TextConst.AVSpecType.ColSets && String.IsNullOrEmpty(this.query_name)) { // colsets
                data = this.createListSourceColsets();
            } else if (this.UseType == UIFormC.UseType.ParamEditor && !String.IsNullOrEmpty(this.query_name)) { // UIList 
                XElement xquery = XmlReports.Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).SearchByAttribute(AName.name, this.query_name);
                if (xquery == null) {
                    throw new VCompilerException("Не найден запрос с именем \"" + this.query_name + "\".", this.Form.XForm, this.xfield);
                }
                IList<XElement> keyCols = xquery.Elements(EName.select).Elements().Where(e => e.AttrOrEmpty(AName.key) == TextConst.AVBool.True).ToList();
                if (keyCols.Count > 1) {
                    throw new VCompilerException("Составной ключ для запроса \"" + this.query_name + "\" не допускается", xquery, keyCols[keyCols.Count - 1]);
                }
                if (this.rows_limit > 0) {
                    data = XmlReports.Environment.GetPrecompiledReport(VQuery.CreateFilteredQuery(xquery, this.rows_limit)).Result(2, false);
                    // достаем имя параметра с условием, если он есть
                    XElement cond_par = xquery.Elements(EName.@params).Elements(EName.param).SearchByAttribute(AName.param_type, TextConst.AVParamTypes.Condition);
                    if (cond_par != null) {
                        this.condition_param_name = cond_par.Attribute(AName.name).Value;
                    }
                } else {
                    data = XmlReports.Environment.GetPrecompiledReport(this.query_name).Result(2, false);
                }
            } else if (this.Form.FormUseType != UIFormC.UseType.ParamEditor) { // Редактора запросов
                data = this.createListSourceForQueryEditor();
            } else {
                data = null;
            }
            return data;
        }
        private VDataSet createListSourceNew()
        {
            VDataSet data;
            // colsets
            if (this.special_type == TextConst.AVSpecType.ColSets && String.IsNullOrEmpty(this.query_name)) {
                data = this.createListSourceColsets();
            // UIList  
            } else if (this.UseType == UIFormC.UseType.ParamEditor && !string.IsNullOrEmpty(this.query_name)) {
                VQuery qry = XmlReports.Environment.GetQuery(this.query_name);
                XElement xquery = qry.AsListQuery();
                var keyCols = xquery.Elements(EName.select).Elements().Where(e => e.AttrOrDefault(AName.key, false));
                if (keyCols.Count() > 1) {
                    throw new VCompilerException("Составной ключ для данного запроса не допускается", xquery, keyCols.Last());
                }
                if (this.rows_limit > 0) { // не проверено
                    xquery = VQuery.CreateFilteredQuery(xquery, this.rows_limit);
                    // достаем имя параметра с условием, если он есть
                    XElement cond_par = xquery.Elements(EName.@params).Elements(EName.param).FirstOrDefault(p => p.AttrOrDefault(AName.param_type, null) == TextConst.AVParamTypes.Condition);
                    if (cond_par != null) {
                        this.condition_param_name = cond_par.Attribute(AName.name).Value;
                    }
                }
                data = XmlReports.Environment.GetPrecompiledReport(xquery).Result(2, false);
            } else if (Form.FormUseType != UIFormC.UseType.ParamEditor) {             // Редактора запросов
                data = createListSourceForQueryEditor();
            } else {
                data = null;
            }
            return data;
        }
        public void PrepareListSource()
        {
            if (!this.Form.WithBehavior) {
                return;
            }
            if (this.data_set_list != null) {
                return;
            }
            this.prepareListSource();
            //if (WebReportsAdapter.IsWebItem(this.query_name)) {
            //    WebReportsAdapter.PrepareDataSet(this.data_set_list, this.query_name, this.Form.DataSource);
            //}
        }
        private void prepareListSource()
        {
            VDataSet data = null;
            if (useColPreset)
            {
                data = createListSourceNew();
            }
            else
            {
                data = createListSource();
            }

            if (data == null) return;

            data.SchemeChanged += DataSource_SchemeChanged;
            (data.Tables[0] as VDataTable).TableRefreshed += DataLocal_TableRefreshed;

            // настраиваем загруженый dataSource
            if (data.Tables[0].Columns.Count > 0)
            {
                PrepareList(data.Tables[0]);
            }

            if (this.data_set_list != null) {
                this.data_set_list.SchemeChanged -= DataSource_SchemeChanged;
                DataTableList.TableRefreshed -= DataLocal_TableRefreshed;
                if (SourceType == ReturnType.Array)
                {
                    DataTableList.MyRowChanged -= DataLocal_RowValueChanged;
                }
            }
            this.data_set_list = data;
            if (SourceType == ReturnType.Array)
            {
                DataTableList.MyRowChanged += DataLocal_RowValueChanged;
            }
            Form.UpdateControlDependence(this);
            InitControlList();

            RefreshAsyncMode();

        }
        private void DataLocal_TableRefreshed(object sender, EventArgs event_args)
        {
            if (this.show_nulls) {
                AddNullValue(DataTableList);
            }
        }
        private void PrepareDefaultSource()
        {
            if (!string.IsNullOrEmpty(this.query_name_default)) {
                this.data_set_default = XmlReports.Environment.GetPrecompiledReport(this.query_name_default).Result(2, false);
                //if (WebReportsAdapter.IsWebItem(this.query_name_default))
                //{
                //    WebReportsAdapter.PrepareDataSet(this.data_set_default, this.query_name_default, Form.DataSource);
                //}
            }
        }
        private void LoadStateFromRegistry()
        {
            if (UIStatic.IsWeb()) return;
            if (_reg_path == null)
            {
                _reg_path = String.Format(@"{0}\sql.builder\forms\{1}\{2}",
                    Environment.UserName,
                    this.Form.GetFormName(),
                    this.field_name);
            }

            using (var reg_settings = Registry.CurrentUser.CreateSubKey(_reg_path, RegistryKeyPermissionCheck.ReadSubTree))
            {
                UserSettings.Clear();
                foreach (var reg_name in reg_settings.GetValueNames()) UserSettings.Add(reg_name, reg_settings.GetValue(reg_name));
            }
        }
        private void SaveStateToRegistry()
        {
            SaveUserSettings();

            using (var reg_settings = Registry.CurrentUser.CreateSubKey(_reg_path, RegistryKeyPermissionCheck.ReadWriteSubTree))
            {
                foreach (var _user_setting in UserSettings) reg_settings.SetValue(_user_setting.Key, _user_setting.Value);
            }
        }



        private XElement CreateColSetsQuery(XElement xscheme)
        {
            var colsets = xscheme.Descendants("viewcolumns").Descendants()
                .Where(el => el.Attribute("colset") != null)
                .Select(el => el.Attribute("colset").Value)
                .Distinct();
            //  .OrderBy(el => el);

            XElement xcolsets_query = null;
            if (colsets.Any())
            {
                int rn = 0;
                var xqueries = colsets.Select(colset =>
                    new XElement("query",
                        new XElement("select",
                            new XElement("const",
                                new XAttribute("as", "kod"),
                                new XAttribute("type", "string"),
                                new XText(String.Format("'{0}'", colset))),
                            new XElement("const",
                                new XAttribute("as", "name"),
                                new XAttribute("type", "string"),
                                new XText(String.Format("'{0}'", colset))),
                            new XElement("const",
                                new XAttribute("as", "ord"),
                                new XAttribute("type", "number"),
                                new XText(rn++.ToString()))),
                        new XElement("from",
                            new XElement("table",
                                new XAttribute("name", "dual")))));

                xcolsets_query =
                    new XElement("query", new XAttribute("order", "ord"),
                        new XElement("select",
                            new XElement("column",
                                new XAttribute("table", "a"),
                                new XAttribute("column", "kod"),
                                new XAttribute("key", "1")),
                            new XElement("column",
                                new XAttribute("table", "a"),
                                new XAttribute("column", "name"),
                                new XAttribute("title", "Группа колонок")),
                            new XElement("column",
                                new XAttribute("table", "a"),
                                new XAttribute("column", "ord"))),
                        new XElement("from",
                            new XElement("query",
                                new XAttribute("as", "a"),
                                new XElement("union", xqueries))));
            }
            else
            {
                xcolsets_query =
                    new XElement("query",
                        new XElement("select",
                            new XElement("const",
                                new XAttribute("as", "kod"),
                                new XAttribute("type", "number"),
                                new XAttribute("key", "1"),
                                new XText("0")),
                            new XElement("const",
                                new XAttribute("as", "name"),
                                new XAttribute("type", "string"),
                                new XAttribute("title", "Группа колонок"),
                                new XText("null"))),
                        new XElement("from",
                            new XElement("table",
                                new XAttribute("name", "dual"),
                                new XAttribute("as", "a"))),
                        new XElement("where",
                            new XElement("call",
                                new XAttribute("function", "false"))));
            }


            return xcolsets_query;
        }
        private void ApplyFilterParams(XElement xparams, bool onlyForselectedValue,IEnumerable<string> names )
        {
            var pars = new List<XElement>();
            if (names != null) {
                xparams.Elements().Remove();
                XElement parVal = Factory.NewCall(TextConst.AVFunction.Array);
                foreach (string name in names) {
                    parVal.Add(new XElement(EName.@const, new XText("'" + name.Replace("'","''") + "'")));
                }
                pars.Add(new XElement(EName.param, new XAttribute(AName.name, TextConst.DBParams.ObjNameParam), parVal));


                //pars.Add(new XElement("param",
                //    new XAttribute("name", TextConst.AVParam.RowsLimit),
                //    new XElement("const", int.MaxValue)));

            }
            if (onlyForselectedValue)
            {
                xparams.Elements().Remove();
                XElement parVal =
                    Form.DataSource.GetParamsAsXml(new string[1] { this.field_name }, null, true)
                        .Elements()
                        .First();



                pars.Add(new XElement("param",
                    new XAttribute("name", TextConst.DBParams.PrimaryKeyParam),
                    parVal.Elements().First()));


                pars.Add(new XElement("param",
                    new XAttribute("name", TextConst.AVParam.RowsLimit),
                    new XElement("const", int.MaxValue)));

            }
            else
            {
                foreach (var par in FilterValues.Where(val => val.Key.EndsWith("_filter")))
                {
                    // если нет * - ищем 'текст%', иначе просто ищем по введенной маске
                    var value = (par.Value.Contains("*")) ? par.Value.Replace("*", "%") : par.Value + "%";
                    pars.Add(new XElement("param",
                        new XAttribute("name", par.Key),
                        new XElement("const", value)));
                }

				if (condition_param_name != null)
				{
                    string value = "rownum <= " + this.rows_limit.ToString() + " ";
                    if (pars.Count != 0) {
						value += " and " +
								 string.Join(" and ",
									 pars.SelectAsArray(
										 p =>
											 string.Format("{0} like '{1}'",
												 p.Attribute("name").Value.Replace("_filter", ""),
												 p.Element("const").Value)));
					}

					xparams.Add(new XElement("param",
						new XAttribute("name", condition_param_name),
						new XElement("const", value)));
				}

				if (names != null)
				{
					pars.Add(new XElement("param",
						new XAttribute("name", TextConst.AVParam.RowsLimit),
						new XElement("const", int.MaxValue)));
				}
				else
				{
					pars.Add(new XElement("param",
						new XAttribute("name", TextConst.AVParam.RowsLimit),
                        new XElement("const", this.rows_limit)));
				}
				//pars.Add(new XElement("param",
				//	new XAttribute("name", TextConst.AVParam.RowsLimit),
				//	new XElement("const", RowsLimit)));// RowsLimit
                // формируем строку с условием в качестве отдельного параметра

            }

            xparams.Add(pars);
        }
        #endregion
        #region Virtual
        public void FullInitialize(XElement xfield, UIFormC form)
        {
            //this.InitializeComponent(xfield);
            this.Initialize(xfield, form);
        }
        public abstract void Initialize(XElement xfield, UIFormC form);
        protected void BaseInitialize(XElement xfield, UIFormC form, ReturnType source_type, Type value_type, bool allow_manual_used_set, bool change_source_immediately = false)
        {
            this.xfield = xfield;
            this.Form = form;
            this.source_type = source_type;
            this.value_type = value_type;
            this.allow_manual_used_set = allow_manual_used_set;
            this.change_source_immediately = change_source_immediately;
            //
            this.InitVaribles();
            this.InitControls();
            // 
            //SetValueUnchecked(DBNull.Value);
            // ceUsed.Properties.ValueUnchecked = DBNull.Value;


            if (this.xfield != null) {
                LoadStateFromRegistry();
            }
            if (!Form.NoData)
            {
                //ReloadListSource();
                if (!Form.WithBehavior)
                {
                    prepareListSource();
                }
                // PrepareListSource();
                PrepareDefaultSource();
            }

            if (!this.mandatory)
            {
                //if (checkContainer is sql.builder.UI.WinForms.VCheckContainer) //!!! скорее всего это вообще не нужно
                //{
                //    foreach (var ctrl in (checkContainer as sql.builder.UI.WinForms.VCheckContainer).GetBaseEditControls())
                //    {
                //        ctrl.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
                //    }
                //}
            }
        }

        protected virtual void InitControl()
        {

        }

        protected virtual void InitControlList()
        {

        }

        public virtual void RefreshData()
        {
            //
        }

        public virtual IEnumerable<string> GetParamsNames()
        {
            if (this.data_set_default != null) {
                return this.data_set_default.GetParNames();
            } else {
                return Enumerable.Empty<string>();
            }
        }
        public virtual string GetText() { return string.Empty; }
        protected virtual object GetNullObj() { return DBNull.Value; }

        protected virtual void ClearSelectionList() { }
        protected virtual void ResetSourceMembers() { }

        protected virtual void SaveUserSettings() { }

        //public virtual RepositoryItem GetRepositoryItem() { return null; }

      
        public virtual int GetHeight() { return 20; }

        public virtual void DataSource_SchemeChanged(object sender, EventArgs e)
        {
            ClearSelectionList();

            ResetSourceMembers();
        }
        public virtual void DataLocal_RowValueChanged(object sender, DataRowChangeEventArgs e) { }
        //protected List<EditorButton> additionalButtons = new List<EditorButton>();
        protected List<UIFormC.EditorButtonInfo> additionalButtons = new List<UIFormC.EditorButtonInfo>();
        public bool HasAdditionalButtons()
        {
            return this.additionalButtons.Count != 0;
        }
        public string GetButtonsVisibilityString(DataRow row) // для органиизации видимости дополнительных кнопок в полях грида
        {
            var s = "";
            if (this is UINumber || this is UIDate || this is UIDateTime || this is UIDateRange)// костыль в uiNumber, вроде есть кнопка по умолчанию
            {
                var val = GetBoundColumn().GetEditable(row);
                var v = "1";
                if (!val)
                {
                    v = "0";
                }
                s+= v;
            }
            
         
            foreach (var btn in additionalButtons)
            {
                var vv = "1";
                object newVal = null;
                if (btn.VisibilitySource != null)
                {
                    newVal = Form.DataSource.GetVariableValue(btn.VisibilitySource, row);


                }
                else
                {
                    var bval = GetBoundColumn().GetEditable(row);
                    if (!bval)
                    {
                        newVal = DBNull.Value;
                    }
                    else
                    {
                        newVal = "1";
                    }
                }
                if (Cmn.Nvl(newVal, 0).ToString() == "0")
                {
                    vv = "0";
                }
                s += vv;
            }
            return s;
        }
        public void AddNullValue(DataTable dt)
        {
            DataColumn key_column = dt.Columns[this.key_field_name];
            key_column.AllowDBNull = true;
            DataRow empty_row = dt.NewRow();
            Type type = key_column.DataType;
            if (type == typeof(decimal)) {
                empty_row[key_column] = Cmn.ToDecimal(TextConst.NullConsts.NNULL);
            } else if (type == typeof(string)) {
                empty_row[key_column] = TextConst.NullConsts.SNULL;
            } else {
                throw new ArgumentException("Неподдерживаемый тип пустого значения " + type.FullName);
            }
            empty_row[name_field_name] = TextConst.NullPlaceholder;
            dt.Rows.InsertAt(empty_row, 0);
        }
        protected void RefreshAsyncMode()
        {
            if (DataTableList != null) {
                bool async = (this.rows_limit > 0);
                DataTableList.AsyncLoad = async;
                if (async) {
                    DataTableList.AsyncLoadComplete += VDataTable_OnAsyncLoadComplete;
                    DataTableList.AsyncLoadCanceled += VDataTable_OnAsyncLoadCanceled;
                } else {
                    DataTableList.AsyncLoadComplete -= VDataTable_OnAsyncLoadComplete;
                    DataTableList.AsyncLoadCanceled -= VDataTable_OnAsyncLoadCanceled;
                }
            }
        }
        #endregion
        #region Static
        public static Type GetConcreteType(string type_name)
        {
            return Type.GetType(string.Format("{0}.{1}", typeof(UIBase).Namespace, type_name));
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }


        private object ctrlValue = null;

        protected object GetCtrlValue()
        {
            return ctrlValue;
        }

        protected void SetCtrlValue(object value)
        {
            this.ctrlValue = value;
        }


        private string ctrlText = null;

        protected string GetCtrlText()
        {
            return ctrlText;
        }

        protected void SetCtrlText(string value)
        {
            this.ctrlText = value;
        }

        #endregion
    }

    internal enum ReturnType
    {
        Simple,
        SimpleRange,
        Array
    }
}
