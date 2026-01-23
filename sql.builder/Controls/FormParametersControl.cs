//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
////using System.Windows.Forms; // FolderBrowserDialog
//using System.Xml.Linq;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using sql.builder.Controls.FormFields;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls
//{
//    /// <summary>
//    /// Форма для динамической генерации условий поиска из xml
//    /// </summary>
//    public partial class FormParametersControl : XtraUserControl
//    {
//        #region static
//        private void ParseDottedName(string dotted_name, out string project, out string name)
//        {
//            int n_pos = dotted_name.IndexOf('.');
//            if (n_pos >= 0) {
//                project = dotted_name.Substring(0, n_pos);
//                name = dotted_name.Substring(n_pos + 1);
//            } else {
//                project = "asuse2";
//                name = dotted_name;
//            }
//        }
//        #endregion
//        #region поля
//        private string query_name, form_name, report_name;
//        private UIFormC _form;
//        private VReport _report;
//        private HashSet<string> _column_names;
//        #endregion
//        //escalate event AnyValueChanged
//		private bool loadFromXmlFlag = false;
//		public event EventHandler FormValueChanged;

//        /// <summary>
//        /// проброс внутренней формы, чтобы иметь доступ к структуре 
//        /// </summary>
//        public UIFormC FormLayout
//        {
//            get { return _form; }
//        }

//		private void escalateEvent(object sender, EventArgs e)
//		{
//			if (!loadFromXmlFlag && (FormValueChanged != null))
//			{
//				FormValueChanged(this, e);
//			}
//		}

//        /// <summary>
//        /// Имя запроса из asuse2.xml на основе которого будут генерироваться параметры
//        /// </summary>
//        public string QueryName {
//            get {
//                return this.query_name;
//            }
//            set {
//                this.query_name = value;
//            }
//        }
//        /// <summary>
//        /// Имя формы из asuse2.xml на основе которой будут генерироваться параметры
//        /// </summary>
//        public string FormName {
//            get {
//                return this.form_name;
//            }
//            set {
//                this.form_name = value;
//            }
//        }
//        /// <summary>
//        /// Имя отчёта из asuse2.xml на основе которого будут генерироваться параметры
//        /// </summary>
//        public string ReportName {
//            get {
//                return this.report_name;
//            }
//            set {
//                this.report_name = value;
//            }
//        }
//        public FormParametersControl()
//        {
//            this.InitializeComponent();
//        }
//        /// <summary>
//        /// Инициализация: заполнение формы из xml
//        /// </summary>
//        public void Initialize()
//        {
//            if (string.IsNullOrEmpty(this.report_name) && string.IsNullOrEmpty(this.form_name) && string.IsNullOrEmpty(this.query_name)) {
//                XtraMessageBox.Show("Не удалось инициализировать форму. Хотя бы одно из свойств ReportName, FormName, QueryName должно быть заполнено.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                return;
//            }
//            if (DesignMode) {
//                return;
//            }
//            bool sp = SqlBuilder.ShowPopupWaitForms;
//            SqlBuilder.ShowPopupWaitForms = false;
//            try {
//                //  _form = (UIFormC) new UIFormC();
//                //  _form = (XmlReports.UseNewForms) ? (UIFormC) new UIFormC2() : (UIFormC) new UIFormC();
//                string project;
//                string name;
//                if (!string.IsNullOrEmpty(this.report_name)) {
//                    ParseDottedName(this.report_name, out project, out name);
//                    // подгружаем проект
//                    XmlReports.Environment.Manager.LoadProjectIfNeed(project);
//                    XElement xreport = XmlReports.Environment.Manager.GetScheme().Elements(EName.reports).Elements(EName.report).SearchByAttribute(AName.name, name);
//                    //присваиваем FormName новое имя
//                    if (xreport != null) {
//                        this.form_name = xreport.Attribute(AName.form).Value;
//                    } else {
//                        XtraMessageBox.Show(string.Format("Не удалось инициализировать форму. Убедитесь что параметр ReportName = {0} указан верно.", this.report_name), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        return;
//                    }
//                    this._report = XmlReports.Environment.GetPrecompiledReport(name);
//                }
//                XElement xform = null;
//                if (!string.IsNullOrEmpty(this.form_name)) {
//                    ParseDottedName(this.form_name, out project, out name);
//                    // подгружаем проект
//                    XmlReports.Environment.Manager.LoadProjectIfNeed(project);
//                    xform = XmlReports.Environment.Manager.GetScheme().Elements(EName.forms).Elements(EName.form).SearchByAttribute(AName.name, name);
//                    if (xform == null) {
//                        XtraMessageBox.Show(string.Format("Не удалось инициализировать форму. Убедитесь что параметр FormName = {0} указан верно.", this.form_name), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        return;
//                    }
//                }
//                if (!string.IsNullOrEmpty(this.query_name)) {
//                    ParseDottedName(this.query_name, out project, out name);
//                    // подгружаем проект
//                    XmlReports.Environment.Manager.LoadProjectIfNeed(project);
//                    VQuery qry = XmlReports.Environment.GetQuery(name);
//                    if (qry == null) {
//                        XtraMessageBox.Show(string.Format("Не удалось инициализировать форму. Убедитесь что параметр QueryName = {0} указан верно.", this.query_name), "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                        return;
//                    }
//                    xform = qry.GetFormXElement();
//                    this._report = XmlReports.Environment.GetPrecompiledReport(name);
//                }
//                bool isWithBehavior = xform.AttrOrDefault(TextConst.AName.WithBehavior, true);
//                if (isWithBehavior) {
//                    this._form = UIStatic.CreateForm(xform.Attribute(AName.name).Value, null, false, false, false);
//                } else {
//                    this._form = new UIFormC();
//                    this._form.Initialize(xform);
//                    this._form.RefreshData();
//                    this._form.ApplyVisibitlity();
//                }
//                Control ctrl = this._form.TmpGetControlAsWinFormCtrl() as Control;
//				this._form.AnyValueChanged += escalateEvent;
//                this.Controls.Clear();
//                ctrl.Dock = DockStyle.Fill;
//                this.Controls.Add(ctrl);
//                this.FillWorkFolder();
//            } finally {
//                SqlBuilder.ShowPopupWaitForms = sp;
//            }
//        }
//        /// <summary>
//        /// Сбросить значения параметров в исходное состояние
//        /// </summary>
//        public void RefreshParams()
//        {
//            bool sp = SqlBuilder.ShowPopupWaitForms;
//            SqlBuilder.ShowPopupWaitForms = false;
//            try {
//                this._form.RefreshData();
//            } finally {
//                SqlBuilder.ShowPopupWaitForms = sp;
//            }
//        }
//        /// <summary>
//        /// Загрузка порции строк в указанный DataTable
//        /// </summary>
//        /// <returns></returns>
//        public void FetchNext(DataTable table, int rowsCount)
//        {
//            var vtable = (VDataTable)table;
//            vtable.FetchNext(rowsCount);
//        }
//        /*
//        /// <summary>
//        /// Текст запроса которым заполняется указанный DataTable
//        /// </summary>
//        /// <returns></returns>
//        public string GetTableSelectText(DataTable table)
//        {
//            return (table as VDataTable).DataAdapter.SelectCommand.CommandText;
//        }
//        */
//        /// <summary>
//        /// DataSet с результатом по ReportName
//        /// </summary>
//        /// <returns>DataSet</returns>
//        public DataSet GetDataSet(bool defferedFetch = false)
//        {
//            bool sp = SqlBuilder.ShowPopupWaitForms;
//            SqlBuilder.ShowPopupWaitForms = false;

//            try
//            {
//                var rep_params = _form.GetValue();

//                var ds = PrepareDataSet(rep_params);
//                if (ds == null) return new DataSet();

//                if (defferedFetch)
//                {
//                    if (ds.Tables.Count == 1)
//                    {
//                        (ds.Tables[0] as VDataTable).UseDeferredFetch = true;
//                    }

//                }

//                ds.Refresh(rep_params);

//                return ds;
//            }
//            finally
//            {
//                SqlBuilder.ShowPopupWaitForms = sp;
//            }
//        }
//        /// <summary>
//        /// Сохраняет результат во временной таблице rr_temp в базе 
//        /// и возвращает sql для получения данных из этой таблицы
//        /// </summary>
//        /// <returns></returns>
//        public string GetSqlData()
//        {
//            bool sp = SqlBuilder.ShowPopupWaitForms;
//            SqlBuilder.ShowPopupWaitForms = false;
//            try
//            {
//                var rep_params = _form.GetValue();

//                var ds = PrepareDataSet(rep_params);
//                //if (!ds.UseTempTable) return null;
//                var sql = "";
//                ds.Refresh(rep_params, ref sql, onlyProc: true);

//                var cmd1 = (ds.Tables[0] as VDataTable).DataAdapter.SelectCommand;

//                var cmd = VDBSelectCommand.CopyCommand((Devart.Data.Oracle.OracleCommand)cmd1);
//                cmd.CommandText = sql;
//                sql = VDBSelectCommand.GetCmdParametrizedText(cmd);

//                return sql;
//                //return (ds.Tables[0] as VDataTable).DataAdapter.SelectCommand.CommandText;
//            }
//            finally
//            {
//                SqlBuilder.ShowPopupWaitForms = sp;
//            }
//        }
//        /// <summary>
//        /// Возвращает Command для получения данных
//        /// </summary>
//        /// <returns></returns>
//        public System.Data.Common.DbCommand GetSelectCommand()
//        {
//            bool sp = SqlBuilder.ShowPopupWaitForms;
//            SqlBuilder.ShowPopupWaitForms = false;
//            try
//            {
//                var rep_params = _form.GetValue();

//                var ds = PrepareDataSet(rep_params);
//                //if (!ds.UseTempTable) return null;
//                var sql = "";
//                ds.Refresh(rep_params, ref sql, onlyProc: true);
//                var cmd1 = (ds.Tables[0] as VDataTable).DataAdapter.SelectCommand;

//                var cmd = VDBSelectCommand.CopyCommand((Devart.Data.Oracle.OracleCommand)cmd1);
//                cmd.CommandText = sql;
//                return cmd;
//                //return (ds.Tables[0] as VDataTable).DataAdapter.SelectCommand.CommandText;
//            }
//            finally
//            {
//                SqlBuilder.ShowPopupWaitForms = sp;
//            }
//        }
//        /// <summary>
//        /// Возвращает экземпляр поля для манипуляций с ним извне
//        /// </summary>
//        /// <param name="name">Имя параметра из xml-описания формы</param>
//        /// <returns></returns>
//        public ParamField GetParamField(string name)
//        {
//            return _form.GetParamField(name);
//        }
//        /// <summary>
//        /// Позволяет указать, какие колонки нужны. Остальные будут исключены из запроса.
//        /// </summary>
//        /// <param name="column_names">Список имен колонок, которые должен вернуть запрос</param>
//        public void SelectResultColumns(IList<string> column_names)
//        {
//            if (List.IsNullOrEmpty<string>(column_names)) {
//                this._column_names = null;
//            } else {
//                this._column_names = new HashSet<string>(column_names, StringComparer.InvariantCultureIgnoreCase);
//            }
//        }
//		/// <summary>
//		/// Сохранение шаблона отображаемых параметров
//		/// </summary>
//		public void SaveParametersTemplate()
//		{
//            this._form.SaveGS();
//		}
//		/// <summary>
//		/// Загрузка шаблона отображаемых параметров
//		/// </summary>
//		public void LoadParametersTemplate()
//		{
//            this._form.LoadGS();
//		}
//		/// <summary>
//		/// Сохранение шаблона отображаемых параметров в xml
//		/// </summary>
//		public XElement GetParametersXml()
//		{
//			return this._form.SaveParamsXML();
//		}
//		/// <summary>
//		/// Загрузка шаблона отображаемых параметров из xml
//		/// </summary>
//		/// <param name="reportParams">xml с параметрами, сохраненный через GetParametersXml()</param>
//		public void SetParametersXml(XElement reportParams)
//		{
//			loadFromXmlFlag = true;
//			_form.LoadParamsXML(reportParams);
//			loadFromXmlFlag = false;
//			//_form.LoadGS();
//		}
//        /// <summary>
//        /// Настройка отображения параметров на форме
//        /// </summary>
//        public void ChooseParameters()
//        {
//            string title;
//            if (this.query_name.StartsWith("poisk_ul")) {
//                title = "Выбор условий для поиска";
//            } else {
//                title = null;
//            }
//            this._form.ChooseReportParams(title);
//        }
//        /// <summary>
//        /// Сохранить параметры по умолчанию
//        /// </summary>
//        public void SaveDefaultParameters()
//        {
//            XElement xroot = new XElement(EName.root);
//            Parser.SaveReportParamsToXml(xroot, this._form);
//            db.MergeDefaultReportSetting(this.GetName(), xroot.ToString());
//        }
//        /// <summary>
//        /// Загрузить параметры по умолчанию, если они есть
//        /// </summary>
//        public void LoadDefaultParameters()
//        {
//            XElement xparams = Cmn.LoadDefaultReportParams(this.GetName());
//            if (xparams != null) {
//                this._form.SetDefaultParams(xparams);
//                this._form.RefreshData();
//            }
//        }
//        /// <summary>
//        /// Очистить параметры по умолчанию
//        /// </summary>
//        internal void ClearDefaultParameters()
//        {
//            db.DeleteDefaultReportSetting(this.GetName());
//        }
//        /// <summary>
//        /// Возвращает параметры, введенные на контроле, в виде DataSet
//        /// </summary>
//        /// <returns></returns>
//        public DataSet GetParamsDataSet()
//        {
//            return Cmn.ToDataSet(this._form.DataSource);
//        }
//        /// <summary>
//        /// Возвращает пустую строку если все параметры указаны корректно, в противном случае возвращает сообщение с информацией
//        /// </summary>
//        /// <returns></returns>
//        public string ValidateParams()
//        {
//            return this._form.GetValidation() ?? string.Empty;
//        }
//        private VDataSet PrepareDataSet(XElement rep_params)
//        {
//            VDataSet ds = this._report.Result(rep_params, 2, null, true, null);
//            if (this._column_names != null) {
//                XElement xViewColumns_preset = ds.SchemePreset.Elements(EName.table).First().Element(EName.viewcolumns);
//                XElement xViewColumns_old = ds.Report.Scheme.Elements(EName.table).First().Element(EName.viewcolumns);
//                XElement xViewColumns_new = new XElement(xViewColumns_old);
//                xViewColumns_new.Elements(EName.column).Where(this.IsNotResultColumn).Remove();
//                XmlReports.CorrectScheme(xViewColumns_new, xViewColumns_old);
//                xViewColumns_preset.ReplaceWith(xViewColumns_new);
//                ds = this._report.Result(rep_params, 2, null, true, ds.SchemePreset);
//            }
//            return ds;
//        }
//        private bool IsNotResultColumn(XElement column)
//        {
//            return !this._column_names.Contains(column.Attribute(AName.name).Value);
//        }
//        private string GetName()
//        {
//            if (!string.IsNullOrEmpty(this.query_name)) {
//                return this.query_name;
//            } else if (!string.IsNullOrEmpty(this.report_name)) {
//                return this.report_name;
//            } else if (!string.IsNullOrEmpty(this.form_name)) {
//                return this.form_name;
//            } else {
//                return null;
//            }
//        }
//        #region WorkFolder
//        private const string WORK_FOLDER_PROMPT = "Выберите папку, в которую будут сохраняться отчёты";
//        private bool _work_folder_visible;
//        /// <summary>
//        /// Видимость текущей рабочей папки и ее настроки
//        /// </summary>
//        public bool WorkFolderVisible {
//            get {
//                return this._work_folder_visible;
//            }
//            set {
//                this._work_folder_visible = value;
//                this.btnWorkFolderPath.Visibility = this._work_folder_visible ? BarItemVisibility.Always : BarItemVisibility.Never;
//                this.barMenu.Visible = this._work_folder_visible;
//            }
//        }
//        private bool _folder_button_pressed;
//        private void rbtnWorkFolderPath_Click(object sender, EventArgs e)
//        {
//            if (this._folder_button_pressed || this.GetWorkFolder() == string.Empty) {
//                this.SelectWorkFolder();
//            } else {
//                this.ShowWorkFolder();
//            }
//            this._folder_button_pressed = false;
//        }
//        private void rbtnWorkFolderPath_ButtonPressed(object sender, ButtonPressedEventArgs e)
//        {
//            this._folder_button_pressed = true;
//        }
//        private void rbtnWorkFolderPath_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
//        {
//            if (e.Value == null) {
//                e.DisplayText = string.Empty;
//            } else {
//                e.DisplayText = Cmn.CutString((string)e.Value, 48);
//            }
//        }
//        private void ShowWorkFolder()
//        {
//            Process.Start(this.GetWorkFolder());
//        }
//        private void SelectWorkFolder()
//        {
//            using (FolderBrowserDialog dlg = new FolderBrowserDialog()) {
//                dlg.Description = WORK_FOLDER_PROMPT;
//                dlg.SelectedPath = this.GetWorkFolder();
//                if (dlg.ShowDialog() == DialogResult.OK) {
//                    this.SetWorkFolder(dlg.SelectedPath);
//                }
//            }
//        }
//        private void SetWorkFolder(string path)
//        {
//            SettingsHelper.WorkFolder = path;
//            Printing.outputFolder = path;
//            this.FillWorkFolder();
//        }
//        private string GetWorkFolder()
//        {
//            string work_folder = (string)this.btnWorkFolderPath.EditValue;
//            if (work_folder == null) {
//                return string.Empty;
//            } else {
//                return work_folder;
//            }
//        }
//        private void FillWorkFolder()
//        {
//            string work_folder = SqlBuilder.GetWorkFolderPath();
//            this.btnWorkFolderPath.EditValue = work_folder;
//            Printing.outputFolder = work_folder;
//        }
//        private bool CheckWorkFolder()
//        {
//            string work_folder = this.GetWorkFolder();
//            if (!Directory.Exists(work_folder)) {
//                using (FolderBrowserDialog dlg = new FolderBrowserDialog()) {
//                    dlg.Description = WORK_FOLDER_PROMPT;
//                    if (dlg.ShowDialog() == DialogResult.OK) {
//                        work_folder = dlg.SelectedPath;
//                        this.SetWorkFolder(work_folder);
//                        return true;
//                    }
//                    return false;
//                }
//            }
//            return true;
//        }
//        #endregion
//    }
//}