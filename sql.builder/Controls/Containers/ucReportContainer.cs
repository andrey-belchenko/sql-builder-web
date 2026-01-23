//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.Linq;
//////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.Data;
////using DevExpress.XtraBars;
////using DevExpress.XtraGrid.Views.BandedGrid;
//using infoenergo.core.Extensions;
//using sql.builder.Controls.Grids;
//using sql.builder.Controls;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;
//using sql.builder.WebReports;

//namespace sql.builder.Controls.Containers
//{
//    internal partial class ucReportContainer : ucBaseContainer
//    {
//        #region Закрытые переменные
//        // режим грида
//        private GridMode _mode;
//        // грид

//        //private IReportGrid _grid;
//        private ucTableViewerContainer _grid;
//        // форма с параметрами грида
//        private UIForm _params;
//        private XElement _custom_params;

//        // Показывает изменялись ли параметры после последнего формирования отчёта
//        private bool _actual;

//        private Stopwatch _timer;

//        //private Dictionary<string, string> _report_info;
//        private Dictionary<string, Tuple<int, string, int>> _columns_settings;

//        bool _events_loaded = false;

//        // private bool form_initialized;
//        #endregion
//        #region Свойства
//        /// <summary>
//        /// Режим грида: отчет/справочник
//        /// </summary>
//        internal GridMode Mode {
//            get { return _mode; }
//        }
//        /// <summary>
//        /// Грид
//        /// </summary>
//        internal ucTableViewerContainer Grid
//        {
//            get { return this._grid; }
//        }
//        /// <summary>
//        /// Форма с параметрами
//        /// </summary>
//        internal UIForm ParamForm
//        {
//            get { return this._params; }
//        }
//        internal XElement CustomParams
//        {
//            get { return this._custom_params; }
//        }
//        internal bool Actual
//        {
//            get { return this._actual; }
//        }
//        internal int UseRepository = 2;
//        #endregion
//        #region Открытые методы
//        public ucReportContainer()
//        {
//            //InitializeComponent();
//            //Disposed += OnDisposed;

//            _timer = new Stopwatch();
//            _columns_settings = new Dictionary<string, Tuple<int, string, int>>();
//        }
//        private void OnDisposed(object sender, EventArgs args)
//        {
            
//            if (_paramsC != null)
//            {
//                _paramsC.NeedReportScheme -= GetReportScheme;
//                _paramsC.NeedReport -= GetReport;
//                _paramsC.SpecialTypeChanged -= UIFormC_SpecialTypeChanged;
//                if (_paramsC.DataSource != null)
//                {
//                    _paramsC.DataSource.Changed -= OnDataSourceOnChanged;
//                    _paramsC.DataSource.Clear();
//                }
//            }

//            _grid.RemoveDataSourceChangedHandler(OnGridOnDataSourceChanged, OnGridOnDataSourceChanged2);
//            if (_grid.DataSource != null) _grid.DataSource.Clear();
//        }
//        public bool IsHiddenExcel = false; // еще одна опция, чтобы организавать работу при excel и noGrid одновременно (чтобы не грузить грид заранее)

//        public void ResetDataSource(VReport report)
//        {
//            if (report != null && report.Elements(TextConst.EName.Queries).Elements().Elements(TextConst.EName.Select).Any())
//            {
//                Grid.DataSource = report.Result(UseRepository, true);
//            }
//            else
//            {
//                Grid.DataSource = XmlReports.Environment.GetPrecompiledReport(_grid.OriginalName).Result(UseRepository, true);
//            }
//        }
//        public void Initialize(Dictionary<string, string> report_info, VReport report, bool from_file = false, XElement xdoc = null)
//        {
//            XmlReports.Environment.Manager.PushOldOnly((report_info["old"] == "True"));
//            // Емцов - чтобы данные не терялись
//            //if (from_file && (report.P_NoGrid == "1")) report_info["item_type"] = TextConst.EName.UseReport;

//            string form_name = null;
//            bool no_grid;

//            XElement xscheme = null;

//            bool isWebReport = WebReportsAdapter.IsWebReport(report_info);
//            if (!isWebReport)
//            {
//                xscheme = xdoc.Element(EName.scheme);

//                if (report != null)
//                {
//                    no_grid = report.AttrOrDefault(AName.nogrid, false);
//                    form_name = report.AttrOrDefault(AName.form, null);
//                }
//                else
//                {
//                    no_grid = xscheme.AttrOrDefault(AName.nogrid, false);
//                    form_name = xscheme.AttrOrDefault(AName.form, null);
//                }
//            }
//            else
//            {
//                no_grid = true;
//            }

            
            
//            this._mode = no_grid ? GridMode.NoGrid : GridMode.ReportGrid;
//            ucTableViewerContainer grid = new ucTableViewerContainer(ControlMode.Report);
//            this._grid = grid;
//            grid.Initialize(report_info, from_file);
//            if (this._mode == GridMode.NoGrid) {
//                grid.SetViewMode(TableViewMode.Empty);
//                grid.ViewSelect = false;
//                if (!isWebReport)
//                {
//                    if (report.P_ViewMode == TextConst.AVViewModes.Excel)
//                    {
//                        this.IsHiddenExcel = true;
//                    }
//                }
                    
//            }
//            grid.SetExpandAllNodes(true);
//            grid.SetTopToolbarVisible(false);
//            grid.AddDataSourceChangedHandler(OnGridOnDataSourceChanged, OnGridOnDataSourceChanged2);
//            // Загружаем датасет
//            if (this._mode != GridMode.NoGrid || from_file) {
//                if (xdoc != null && from_file) {
//                    this.LoadFromXml(xdoc, SettingsType.SchemeAndData);
//                } else {
//                    this.LoadFromXml(xdoc, SettingsType.Scheme);
//                    this.ResetDataSource(report);
//                    // чтобы не использовать старую схему
//                    // корректируем новую с учетом старой
//                    if (Grid.IsTemplate) {
//                        var xscheme2 = Parser.RepairScheme(xscheme, new XElement(Grid.DataSource.Scheme));
//                        xscheme.ReplaceWith(xscheme2);
//                        xscheme = xscheme2;
//                    }
//                }
//                grid.DataSource.SchemePreset = new VXElement(xscheme);
//                if (report != null) {
//                    this.UpdateGridEvents(_grid, report);
//                }
//            }
//            // Параметры по умолчанию
//            XElement xparams = xdoc != null ? xdoc.Element(EName.@params) : null;
//            if (xparams == null && !grid.IsTemplate) {
//                xparams = Cmn.LoadDefaultReportParams(grid.ReportName);
//            }
//            this.InitForm(form_name, grid.OriginalName, xparams);
//            //_grid.GetControl().Dock = DockStyle.Fill;
//            //this.dpGrid.Controls.Add((ucTableViewerContainerWF)_grid.GetControl());
//            this.ContainerTitle = grid.ReportTitle + (grid.FromFile ? " (Загружено)" : "");
//            this.ContainerName = grid.ReportName + (grid.FromFile ? "_loaded" : "");
//            if (grid != null && _paramsC != null) {
//                _grid.UpdateTitles(_paramsC.DataSource,false);
//            }
//            #if DEBUG
//            this.UpdateAvgFormingTime();
//            #endif
//            XmlReports.Environment.Manager.PopOldOnly();
//        }
//        private void OnGridOnDataSourceChanged2(object sender, EventArgs args)
//        {
//            OnGridOnDataSourceChanged();
//        }

//        private void OnGridOnDataSourceChanged()
//        {
//            _actual = true;
//        }
//        public void SetCustomParams(XElement xparams)
//        {
//            _custom_params = xparams;
//        }

//        private bool Grid_UIEvent(object sender, UIEventArgs e)
//        {
//            //var reportsForm = Parent.Parent.Parent as ucMainReports;

//            //VUseAction.ExecuteAction(null, (VUseAction)e.ActionInfo, (VDataSet)e.Row.Table.DataSet, null, (VDataTable)e.Row.Table, e.Row, e.Column, reportsForm, _paramsC);

//            //return true;

//            return false;


//        }
//        private void Command_ItemClick(object sender)
//        {
//            //var btn = sender as IVTagControl;
//            //var reportsForm = Parent.Parent.Parent as ucMainReports;
//            //var ds = _grid.DataSource;
//            //var dt = (VDataTable)ds.Tables[0];
//            //var row = dt.CurrentRow;
//            //VUseAction.ExecuteAction(null, (VUseAction)btn.Tag, ds, null, dt, row, null, reportsForm);
//        }

//        internal void UpdateGridEvents()
//        {
//            if (Grid.DataSource == null) return;

//            UpdateGridEvents(Grid, Grid.DataSource.Report);
//        }

 
//        void UpdateGridEvents(ucTableViewerContainer grid, VReport report)
//        {
//            //if (_events_loaded) return;
//            //var  event_attached = false;
//            //foreach (VQueryCall queryCall in report.Queries())
//            //{
//            //    var events = queryCall.Events();

//            //    if (events.Count > 0 && !event_attached)
//            //    {
//            //        grid.AddUIEventHandler(Grid_UIEvent);
//            //        event_attached = true;
//            //    }

//            //    foreach (XElement xcmd in events)
//            //    {

//            //        var action = VSXElement.Get(new XElement(xcmd));
//            //        var eventFullName = xcmd.Attribute(TextConst.AName.EventName).Value;
//            //        if (xcmd.Attribute(TextConst.AName.Column) != null)
//            //        {
//            //            eventFullName = xcmd.Attribute(TextConst.AName.Column).Value + "|" + eventFullName;
//            //        }
//            //        grid.AddEventTag(eventFullName, action, queryCall.XName);
//            //    }
//            //    var xmenu = queryCall.Element(TextConst.EName.Menu);
//            //    if (xmenu != null)
//            //    {
//            //        IVBarItem[] items = Cmn.CreateBarItems(xmenu, grid.GetBarManager(), Command_ItemClick, null, null, null, grid.GetVariableDepandantceController());
//            //        grid.UpdatePopupMenu(xmenu, items);
//            //    }

//            //    _events_loaded = true;
//            //}
//        }

//        public string GetParamsValidation()
//        {
//            return _paramsC.GetValidation();
//        }
//        private void InitForm(string form_name, string repname, XElement xparams = null)
//        {
//            bool isWithBehavior = XmlReports.IsFormWithBehavior(form_name, repname); // чтобы не делалось лишних действий когда форма из кеша
//            XElement xform;
//            bool is_old_form;
//            if (!isWithBehavior) {
//                //var xform = XmlReports.GetForm(_report_info["form"], _report_info["repname"]);
//                xform = XmlReports.GetForm(form_name, repname); // Бельченко 12.05.2017
//                is_old_form = xform.Descendants().Attributes(AName.controlType).Any(ct => ct.Value.StartsWith("UIControl"));
//            } else {
//                xform = null;
//                is_old_form = false;
//            }
//            // Старая форма еще где-то используется
//            if (is_old_form) {
//                //_params = new UIForm(pParams, xform);
//                throw new NotImplementedException("Поддержка old_form для web не реализована");
//            } else if (_mode != GridMode.ReferenceGrid) {
//                // заполняем панельку с параметрами
//                //pParams.Controls.Clear();
//                // _paramsC = (XmlReports.UseNewForms) ? (UIFormC)new UIFormC2() : (UIFormC)new UIFormC();
//                if (isWithBehavior) {
//                    // Емцов - добавил параметр-делегат, т.к. падали формы с colsets
//                    string name;
//                    if (string.IsNullOrEmpty(form_name)) {
//                        name = repname;
//                    } else {
//                        name = form_name;
//                    }
//                    _paramsC = UIStatic.CreateForm(name, null, false, false, false, GetReportScheme, xparams);
//                    // if (_paramsC.Init) UIStatic.UpdateForm(_paramsC, null, false); 12/05/2017 Бельченко вроде не нужно , уже есть в create, выполняется дважды
//                } else {
//                    _paramsC = (UIFormC)new UIFormC();
//                    _paramsC.NeedReportScheme += GetReportScheme;
//                }
//                _paramsC.NeedReport += GetReport;
//                _paramsC.SpecialTypeChanged += UIFormC_SpecialTypeChanged;
//                if (!isWithBehavior) {
//                    _paramsC.Initialize(xform);
//                }
//                _paramsC.DataSource.Changed += OnDataSourceOnChanged;
//                if (!isWithBehavior) {
//                    if (xparams != null) _paramsC.SetDefaultParams(xparams);
//                }
//                //pParams.Controls.Add(_paramsC.TmpGetControlAsWinFormCtrl() as Control);
//            }
//            // если у отчета нет параметров - прячем панельку 
//            if ((_params != null && _params.controls.Count == 0) || (_paramsC != null && _paramsC.UIControlsCount == 0)) {
//                if (this._mode == GridMode.NoGrid) {
//                    this._grid.SetText("Чтобы вывести отчёт, нажмите кнопку \"Печатная форма\"");
//                }
//                //dockManager.RemovePanel(dpParams);
//            }
//        }
//        private void OnDataSourceOnChanged(object sender, EventArgs args)
//        {
//            _actual = false;
//            if (_grid != null && _paramsC != null)
//            {
//                _grid.UpdateTitles(_paramsC.DataSource,true);
//            }
//        }
//        public void BeginForming()
//        {
//            _grid.SetFormingTime(null);
//            _timer.Restart();
//        }
//        public void EndForming()
//        {
//            _timer.Stop();
//            var ts = _timer.Elapsed;
//            var forming_time = String.Format("{0}:{1:00}:{2:00}.{3:00}",
//            ts.Hours, ts.Minutes, ts.Seconds,
//            ts.Milliseconds / 10);
//            _grid.SetFormingTime(forming_time);
//        }
//        #if DEBUG
//        internal void BeginPrinting()
//        {
//            //_grid.SetFormingTime(null);
//            _timer.Restart();
//        }
//        internal void EndPrinting()
//        {
//            _timer.Stop();
//            var ts = _timer.Elapsed;
//            var printing_time = String.Format("{0}:{1:00}:{2:00}.{3:00}",
//            ts.Hours, ts.Minutes, ts.Seconds,
//            ts.Milliseconds / 10);
//            _grid.SetPrintingTime(printing_time);
//        }
//        #endif
//        public void ChangeGridMode(GridMode new_mode,TableViewMode tableViewMode= TableViewMode.Default)
//        {
//            if (new_mode == _mode) return;

//            _mode = new_mode;

//            VDataSet dataSet = _grid.DataSource;
//            if (dataSet == null && new_mode != GridMode.NoGrid)
//            {
//                var report = XmlReports.Environment.GetPrecompiledReport(_grid.ReportName);
//                dataSet = report.Result(UseRepository, true);
//                _grid.DataSource = dataSet;
//            }

//            var grid = _grid as ucTableViewerContainer;
//            if (new_mode == GridMode.ReportGrid)
//            {
//                if (tableViewMode != TableViewMode.Excel)
//                {
//                    grid.ViewSelect = true;
//                }
//                LoadFromXml(new XElement("root", dataSet.Scheme), SettingsType.Scheme,tableViewMode);
                
//               grid.SetViewMode(tableViewMode);
                
//            }
//            else
//            {
//                grid.SetViewMode(TableViewMode.Empty);
//            }
//        }
//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="fixed_scheme"> false - SchemeNative, true - SchemePreset</param>
//        public void UpdateGridSettings(bool fixed_scheme, bool layout_changed = true)
//        {
//            if (_mode != GridMode.ReportGrid) return;

//            // TODO: сделать обновление колонок для pivot когда понадобится

//            //var grid_old = Grid as ucReportGridOld;

//            //if (grid_old != null)
//            //{
//            //    var xRoot = SaveToXml(SettingsType.Scheme, fixed_scheme);
//            //    var view = (grid_old.grid != null) ? (BandedGridView)grid_old.grid.Grid.MainView : null;

//            //    if (grid_old.grid != null)
//            //    {
//            //        // предварительно сохраняем состояние колонок
//            //        foreach (BandedGridColumn col in view.Columns)
//            //        {
//            //            if (_columns_settings.ContainsKey(col.FieldName)) _columns_settings.Remove(col.FieldName);

//            //            _columns_settings.Add(col.FieldName,
//            //                new Tuple<int, string, int>(col.Width, col.SortOrder.ToString().ToLower(), col.GroupIndex));
//            //        }
//            //    }

//            //    LoadFromXml(xRoot, SettingsType.Scheme);

//            //    if (grid_old.grid != null)
//            //    {
//            //        grid_old.BeginUpdate();
//            //        // загружаем состояние колонок
//            //        foreach (BandedGridColumn column in view.Columns)
//            //        {
//            //            if (_columns_settings.ContainsKey(column.FieldName))
//            //            {
//            //                var column_info = _columns_settings[column.FieldName];
//            //                column.Width = column_info.Item1;
//            //                column.GroupIndex = column_info.Item3;

//            //                switch (column_info.Item2)
//            //                {
//            //                    case "ascending":
//            //                        column.SortOrder = ColumnSortOrder.Ascending;
//            //                        break;
//            //                    case "descending":
//            //                        column.SortOrder = ColumnSortOrder.Descending;
//            //                        break;
//            //                    default:
//            //                        column.SortOrder = ColumnSortOrder.None;
//            //                        break;
//            //                }
//            //            }
//            //        }
//            //        grid_old.EndUpdate();
//            //    }
//            //}
//            //else
//            {
//                // сохранение/восстановление состояния повесил на Begin/End Update грида
//                var xRoot = SaveToXml(SettingsType.Scheme, fixed_scheme);
//                LoadFromXml(xRoot, SettingsType.Scheme);
//            }

//            if (layout_changed) Grid.GenerateLayoutChanged();
//        }

//        // fixed_scheme true - колонки изначальные
//        // fixed_scheme false - колонки уже размноженые 
//        public XElement SaveToXml(SettingsType type = SettingsType.All, bool fixed_scheme = true)
//        {
//            var xRoot = new XElement("root");

//            if (type != SettingsType.Params)
//            {
//                Parser.SaveReportInfoToXml(xRoot, Grid.GetReportInfo(),
//                    fixed_scheme
//                        ? Grid.DataSource.schemePreset
//                        : Grid.DataSource.Scheme);
//            }

//            if (type.HasFlag(SettingsType.Scheme))
//            {
//                Grid.SaveSchemeSettingsToXml(xRoot);
//            }

//            if (type.HasFlag(SettingsType.Params) && this.ParamFormC != null)
//            {
//                Parser.SaveReportParamsToXml(xRoot, this.ParamFormC);
//            }

//            if (type.HasFlag(SettingsType.Data) && this.Grid.DataSource != null)
//            {
//                Parser.SaveReportDataToXml(xRoot, this.Grid.DataSource);
//            }

//            return xRoot;
//        }
//        public void LoadFromXml(XElement xRoot, SettingsType type = SettingsType.All, TableViewMode mode=TableViewMode.Default)
//        {
//            Grid.BeginUpdate();
//           // Grid.HoldViewChanges(); // не понятно  зачем нужно, при выборе колонок они получаются невидимыми, пока уберу

//            if (type.HasFlag(SettingsType.Scheme))
//            {
//                VDataSet parsDs = null;

//                if (_paramsC != null)
//                {
//                    parsDs = _paramsC.DataSource;
//                }
//                Grid.LoadSchemeSettingsFromXml(xRoot, parsDs);
//            }

//            if (type.HasFlag(SettingsType.Params) && this.ParamFormC != null)
//            {
//                Parser.LoadReportParamsFromXml(xRoot, this.ParamFormC);
//            }

//            if (type.HasFlag(SettingsType.Data))
//            {
//                var repname = xRoot.Element("scheme").Attribute("original_name") != null
//                              ? xRoot.Element("scheme").Attribute("original_name").Value
//                              : xRoot.Element("scheme").Attribute("repname").Value;

//                this.Grid.DataSource = Parser.LoadReportDataFromXml(xRoot, null, repname);
//            }

//            //Grid.RestoreViewChanges(); // не понятно  зачем нужно, при выборе колонок они получаются невидимыми, пока уберу
//            Grid.EndUpdate();
//        }
//        #endregion
//        #if DEBUG
//        private void UpdateAvgFormingTime()
//        {
//            string time;
//            if (XmlReports.IsDeveloperMode()) {
//                time = Cmn.GetAvgReportFormingTime(this._grid.OriginalName);
//            } else {
//                time = null;
//            }
//            this._grid.SetAvgFormingTime(time);
//        }
//        #endif
//        #region Обработчики событий
//        private void UIFormC_SpecialTypeChanged(UIFormC sender, string special_type, object data)
//        {
//            switch (special_type)
//            {
//                case "colsets":
//                    if (Grid.DataSource == null) return;

//                    var visible_colset_names = (IEnumerable<string>)data;
//                    XmlReports.SetColsetsVisible(Grid.DataSource.Report.Scheme, Grid.DataSource.SchemePreset, visible_colset_names);

//                    UpdateGridSettings(fixed_scheme: true);
//                    break;
//                case TextConst.AVSpecType.SelectRep:
//                    ChangeReport(data.ToString());

//                    break;
//            }
//        }


//        //Заплатка для подмены отчета 32274
//        public string ChangeReportName = null;
//        public object ChangeTemplateInfo = null;
//        private void ChangeReport(string newReportName)
//        {
//            //if (!(this.Grid is ucNoGrid)) return;

//            if (newReportName == Grid.ReportName) return;

//            Grid.ReportName = newReportName;
//            Grid.DataSource = null;
//            ChangeReportName = newReportName;
//            // var rep = XmlReports.Environment.GetPrecompiledReport(newReportName);
//            //ChangeReportName = newReportName;
//        }

//        public XElement GetReportScheme(UIFormC sender)
//        {
//            if (Grid.DataSource == null)
//            {
//                Grid.DataSource = XmlReports.Environment.GetPrecompiledReport(Grid.OriginalName).Result(UseRepository, true);
//            }

//            return Grid.DataSource.Scheme;
//        }

//        public VReport GetReport(UIFormC sender)
//        {
//            return XmlReports.Environment.GetPrecompiledReport(Grid.OriginalName);
//        }

//        private sql.builder.VForms.VFrmGroupingEditor _frmGroupingEditor = null;
    
//        public void ShowGroupsEditor()
//        {

//            if (_frmGroupingEditor == null)
//            {
//                 _frmGroupingEditor = new sql.builder.VForms.VFrmGroupingEditor(Grid.DataSource.Report.P_IdName);
            
//            }
//            var current_table_name = Grid.GetTopTableName();
//            var xTblSch = Grid.DataSource.SchemePreset
//                    .Elements("table")
//                    .First(tbl => tbl.Attribute("as").Value == current_table_name);
               
//            var xGrouping = xTblSch.Element(TextConst.EName.Grouping);
          
//            _frmGroupingEditor.SetGroupInfo(xGrouping);
//            _frmGroupingEditor.Show();
//            var  xGrouping1 = _frmGroupingEditor.GetGroupInfo();
//            if (xGrouping1!=null)
//            {
//                xTblSch.Elements(TextConst.EName.Grouping).Remove();
//                xTblSch.Add(xGrouping1);
//            }
         
//        }

//        #endregion
//        public enum GridMode
//        {
//            ReportGrid,
//            ReferenceGrid,
//            NoGrid
//        }

//        //private void dockManager_EndSizing(object sender, DevExpress.XtraBars.Docking.EndSizingEventArgs e)
//        //{

//        //}

//        //private void dockManager_StartSizing(object sender, DevExpress.XtraBars.Docking.StartSizingEventArgs e)
//        //{

//        //}

//        //private void dockManager_ShowingDockGuides(object sender, DevExpress.XtraBars.Docking.ShowingDockGuidesEventArgs e)
//        //{

//        //}

//        //private void dockManager_Sizing(object sender, DevExpress.XtraBars.Docking.SizingEventArgs e)
//        //{

//        //}

        
//    }

//    [Flags]
//    public enum SettingsType : short
//    {
//        None = 0,
//        Scheme = 1,
//        Params = 2,
//        Data = 4,

//        All = Scheme | Params | Data,
//        SchemeAndParams = Scheme | Params,
//        SchemeAndData = Scheme | Data
//    }
//}
