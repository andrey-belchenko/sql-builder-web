//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Xml.Linq;
////using System.Windows.Forms;
//using Devart.Data.Oracle;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraGrid.Views.Grid;
//using infoenergo.core.Extensions;
//using sql.builder.Controls.Grids;
//using sql.builder.Controls.Grids.ReportViewModes;
//using sql.builder.Controls.Grids.ReportViewModes.Dashboard;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls
//{
//    internal partial class ucTableViewerContainer :/* IReportGrid,*/ IControlWithTableSource
//    {



//        public UIFormC GetForm()
//        {
//            var ds = GetDataSource();
//            if (ds != null)
//            {
//                return ds.Form;
//            }
//            return null;
//        }

//        public void RaiseChangeActionComplete()
//        {
//            GetForm().RaiseChangeActionComplete();
//        }
//        private ControlMode _mode = ControlMode.None;
//        private TableViewMode _view_mode = TableViewMode.None;
//        private string _top_table_name = null;

//        private XElement _xscheme;
//        private XElement _xevents;
//        private XElement _xtoolbarTop;
//        private XElement _xtoolbarBottom;
//        private XElement _xmenu;

//        private IVBarItem[] _menu_items;
//        private IVBarItem[] _toolbar_top_items;
//        private IVBarItem[] _toolbar_bottom_items;
//        private Dictionary<string, bool> _toolbar_buttons_visible;

//        // контролы подгружаются динамически
//        internal ucGridContainer grid;
//        internal ucPivotNew pivot;
//        //internal ucTreeNew tree;
//        internal ucGridContainer tree
//        {
//            get
//            {
//                return grid;
//            }
//        }
//        internal ucExcelNew excel;
//        internal ucEmpty empty;
//        internal ucDashboardDesigner ddesigner;
//        internal ucDashboardViewer dviewer;

//        private VGCcbItem[] _viewModes;
//        private VGCcbItem[] _tableLevels;

//        private bool _table_select;

//        public VDataTable GetDataTable()
//        {
//            return (GetDataSource().Tables[GetTopTableName()] as VDataTable);
//        }
//        public void SetVisibleInLayout(bool value)
//        {
//            GetDataTable().SetVisibleInLayout(value);
//        }

//        public bool TableSelect
//        {
//            set
//            {
//                _table_select = value;
//                setControlTableSelectVisibility(_table_select);

//            }

//            get { return _table_select; }
//        }
//        private bool _view_select;

//        public void UpdateDataTableProperties()
//        {
//            GetDataTable().Grid = this;
//        }

//        public bool ViewSelect
//        {
//            set
//            {
//                _view_select = value;
//                setControlViewSelectVisibility(_view_select);
//            }

//            get { return _view_select; }
//        }

//        private bool _global_footer_visible = true;
//        public void SetGlobalFooterVisible(bool visible)
//        {
//            _global_footer_visible = visible;
//            setControlFooterVisibility(visible);
//            //getControl().barFooter.Visible = visible;
//        }

//        private bool _toolbar_top_visible = true;
//        public void SetTopToolbarVisible(bool visible)
//        {
//            _toolbar_top_visible = visible;
//            if (grid != null) grid.SetTopToolbarVisible(_toolbar_top_visible);
//            if (pivot != null) pivot.SetToolbarVisible(_toolbar_top_visible);
//            //if (tree != null) tree.SetTopToolbarVisible(_toolbar_top_visible);
//        }

//        private bool _toolbar_bottom_visible = false;
//        public void SetBottomToolbarVisible(bool visible)
//        {
//            _toolbar_bottom_visible = visible;
//            if (grid != null) grid.SetBottomToolbarVisible(_toolbar_bottom_visible);
//            //if (tree != null) tree.SetBottomToolbarVisible(_toolbar_bottom_visible);
//        }

//        private bool _footer_visible = true;
//        public void SetFooterVisible(bool visible)
//        {
//            _footer_visible = visible;
//            if (grid != null) grid.SetFooterVisible(_footer_visible);
//            if (pivot != null) pivot.SetFooterVisible(_footer_visible);
//            //if (tree != null) tree.SetFooterVisible(_footer_visible);

//            if (_footer_visible)
//            {
//                SetMultiselectMode(_footer_visible);
//            }
//        }

//        private bool _summary_visible = true;
//        public void SetSummaryVisible(bool visible)
//        {
//            _summary_visible = visible;
//            if (grid != null) grid.SetSummaryVisible(visible);
//            //if (tree != null) tree.SetSummaryVisible(visible);
//        }

//        private bool _filt_row_visible = false;
//        public void SetFilterRowVisible(bool visible)
//        {
//            _filt_row_visible = visible;
//            if (grid != null) grid.SetFilterRowVisible(visible);
//        }
//		private bool _allowSelectMoveColumns = false;
//		public void SetAllowSelectMoveColumns(bool value)
//		{
//			_allowSelectMoveColumns = value;
//			if (grid != null) grid.SetAllowSelectMoveColumns(value);
//		}

//        private bool _multiselect = true;
//        public void SetMultiselect(bool multiselect)
//        {
//            _multiselect = multiselect;
//            if (grid != null) grid.SetMultiselect(_multiselect);
//            //if (tree != null) tree.SetMultiselect(_multiselect);

//            SetMultiselectMode(_isCellMultiselect);
//        }
//        private bool _isCellMultiselect = false;
//        public void SetMultiselectMode(bool isCell)
//        {
//            _isCellMultiselect = isCell;
//            if (grid != null) grid.SetMultiselectMode(isCell);
//            //if (tree != null) tree.SetMultiselectMode(isCell);
//        }


//        private string _title;
//        public void SetTitle(string title)
//        {
//            _title = title;
//            if (grid != null) grid.SetTitle(_title);
//            //if (tree != null) tree.SetTitle(_title);
//        }

//        private string _form_name;
//        public void SetFormName(string form_name)
//        {
//            _form_name = form_name;
//            if (grid != null) grid.SetFormName(_form_name);
//            //if (tree != null) tree.SetFormName(_form_name);
//        }

//        private string _parent_field_name;
//        public void SetParentFieldName(string parent_field_name)
//        {
//            _parent_field_name = parent_field_name;
//            if (tree != null)
//            {
//                tree.SetParentFieldName(_parent_field_name);
//            }
//        }

//        private bool _expand_all_nodes;
//        public void SetExpandAllNodes(bool expand_all_nodes)
//        {
//            _expand_all_nodes = expand_all_nodes;
//            if (tree != null) tree.SetExpandAllNodes(expand_all_nodes);
//        }


//        public bool IsTree()
//        {
//            return grid.IsTree();
//        }

//        private bool isDxExport = true;
//        public void SetIsDxExport(bool value)
//        {
         
//            isDxExport = value;
//            if (grid != null) grid.IsDxExport = isDxExport;
//        }

//        private string captionForExport = null;
//        public void SetCaptionForExport(string value)
//        {

//            captionForExport = value;
//            if (grid != null) grid.CaptionForExport = captionForExport;
//        }

//        private string _order_field_name;
//        public void SetOrderFieldName(string order_field_name)
//        {
//            _order_field_name = order_field_name;
//            if (grid != null) grid.SetOrderFieldName(_order_field_name);
//            //if (tree != null) tree.SetOrderFieldName(_order_field_name);
//        }

//        //private bool _allow_drag_and_drop;
//        //public void SetAllowDragAndDrop(bool allow_drag_and_drop)
//        //{
//        //    _allow_drag_and_drop = allow_drag_and_drop;
//        //    if (tree != null) tree.SetAllowDragAndDrop(_allow_drag_and_drop);
//        //}

//        public event EventHandler OpenExcel;

//        #region DataSource
//        private VDataSet _source;
//        public void SetDataSource(VDataSet source)
//        {
//            // чтобы небыло утечек
//            if (_source != null) DetachDataSourceEvents();

//            _source = source;

//            if (empty != null) empty.SetDataSource(source);
//            if (grid != null) grid.SetDataSource(source);
//            if (pivot != null) pivot.SetDataSource(source);
//            //if (tree != null) tree.SetDataSource(source);
//            if (excel != null) excel.SetDataSource(source);
//            if (ddesigner != null) ddesigner.SetData(source.Tables[0]);
//            if (dviewer != null) dviewer.SetData(source.Tables[0]);

//            // поменял порядок - проконтролировать
//            FillTableLevels();
//            if (_tableLevels != null) {// выкинуло ошибку 
//                if (_tableLevels.Length != 0) {
//                    SetTopTable(_top_table_name);
//                }
//            }

//            UpdateDataSourceEventOptions();
//            AttachDataSourceEvents();
//            //SetSelectionFromSource();
//            if (DataSourceChanged != null)
//            {
//                DataSourceChanged(this, EventArgs.Empty);
//            }

//        }
//        public VDataSet GetDataSource()
//        {
//            return _source;
//        }
//        public VDataTable GetSourceTable()
//        {
//            if (_source == null || _top_table_name == null) return null;

//            return (VDataTable)_source.Tables[_top_table_name];
//        }

//        public event EventHandler DataSourceChanged;

//        private void AttachDataSourceEvents()
//        {
//            if (_variableDepandantceController != null)
//            {
//                _variableDepandantceController.attachControlStateEvent();
//            }
//            SetAllowMerge();



//        }

//        public void SetAllowMerge()
//        {

//            var tbl = GetSourceTable();
//            if (tbl != null)
//            {

//                if (grid != null) grid.SetMerged(tbl.Merged);
//                setControlAllowMergeVisibility(tbl.AllowMerge);
//                //if (tbl.AllowMerge)
//                //{
//                //    getControl().beMerge.Visibility = BarItemVisibility.Always;
//                //}
//                //else
//                //{
//                //    getControl().beMerge.Visibility = BarItemVisibility.Never;
//                //}
//            }






//        }

//        private void DetachDataSourceEvents()
//        {
//            //
//        }

//        #endregion
//        #region EventTags
//        public event UIEventHandler UIEvent;
//        private bool RaiseUIEvent(object sender, UIEventArgs2 args2)
//        {
//            if (UIEvent != null)
//            {
//                if (eventsTags != null)
//                {
//                    if (eventsTags.ContainsKey(args2.TableName))
//                    {
//                        var fullName = args2.Name;
//                        if (args2.Column != null)
//                        {
//                            if (args2.Column.OriginalNameForPivotColumn != null)
//                            {
//                                fullName = args2.Column.OriginalNameForPivotColumn + "|" + fullName;
//                            }
//                            else
//                            {
//                                fullName = args2.Column.ColumnName + "|" + fullName;
//                            }

//                        }
//                        if (!eventsTags[args2.TableName].ContainsKey(fullName))
//                        {
//                            fullName = args2.Name;
//                        }

//                        if (eventsTags[args2.TableName].ContainsKey(fullName))
//                        {
//                            UIEvent(this, new UIEventArgs(args2.Name, eventsTags[args2.TableName][fullName], args2.Table, args2.Row, args2.Column));
//                            return true;
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        private Dictionary<string, Dictionary<string, VSXElement>> eventsTags;
//        public void AddEventTag(string eventName, VSXElement tag, string tableName = "")
//        {
//            if (eventsTags == null)
//            {
//                eventsTags = new Dictionary<string, Dictionary<string, VSXElement>>();
//            }

//            if (!eventsTags.ContainsKey(tableName))
//            {
//                eventsTags[tableName] = new Dictionary<string, VSXElement>();
//            }

//            eventsTags[tableName].Add(eventName, tag);

//            UpdateDataSourceEventOptions();
//        }

//        public void UpdateDataSourceEventOptions()// Для создания линков при экспорте в excel
//        {
//            if (eventsTags == null || _source == null) return;

//            foreach (var kv in eventsTags)
//            {
//                if (kv.Key == "") continue;

//                var table = (_source.Tables[kv.Key] as VDataTable);

//                foreach (string s in kv.Value.Keys)
//                {
//                    string[] ss = s.Split('|');
//                    if (ss.Length == 1)
//                    {
//                        table.HasRowEvents = true;
//                    }
//                    else
//                    {
//                        foreach (VDataColumn col in table.GetColumnsByPivotOriginalName(ss[0]))
//                        {
//                            if (kv.Value[s].Attribute(TextConst.AName.ActionType) == null && kv.Value[s].Attribute(TextConst.AName.Name) == null)
//                            {
//                                col.IsEmptyEvent = true;// чтобы отменить обработчик строки, не универсально, пока сойдет
//                            }
//                            else
//                            {
//                                col.HasCellEvents = true;
//                            }
//                        }
//                    }
//                }
//            }
//        }
//        #endregion
//        #region Layout
//        private int _layout_reloading = 0;
//        public void BeginLayoutReloading()
//        {
//            _layout_reloading++;
//        }
//        public void EndLayoutReloading()
//        {
//            _layout_reloading--;
//        }
//        private bool IsLayoutReloading()
//        {
//            return (_layout_reloading > 0);
//        }

//        public event EventHandler LayoutChangedEvent;
//        protected void RaiseLayoutChanged(object sender, EventArgs args)
//        {
//            // сгенерировать события, что визуальные настройки изменены
//            if (IsLayoutReloading()) return;

//            if (LayoutChangedEvent != null)
//            {
//                LayoutChangedEvent(this, EventArgs.Empty);
//            }
//        }
//        #endregion
//        #region ReportMode
//        private Dictionary<string, string> _report_info;

//        public string OriginalName
//        {
//            get { return _report_info["original_name"]; }
//        }
//        public string ReportName
//        {
//            get { return _report_info["repname"]; }
//            set { _report_info["repname"] = value; }
//        }
//        public string ReportTitle
//        {
//            get { return _report_info["title"]; }
//        }

//        public bool IsTemplate
//        {
//            get
//            {
//                return //_mode == ControlMode.Report && - зачем это?
//                    _report_info != null && _report_info["item_type"] == TextConst.EName.UseTemplate;
//            }
//        }
//        public bool FromFile { get; private set; }

//        public Dictionary<string, string> GetReportInfo()
//        {
//            return _report_info;
//        }

//        #endregion
//        public event HasMessageHandler HasMessage;
//        public void RaiseHasMessage(object sender, HasMessageArgs args)
//        {
//            if (HasMessage != null)
//            {
//                HasMessage(sender, args);
//            }
//        }
//        public void AddHasMessageHandler(Action<string> action, HasMessageHandler action2)
//        {
//            HasMessage += action2;
//        }
//        public void RemoveHasMessageHandler(Action<string> action, HasMessageHandler action2)
//        {
//            HasMessage -= action2;
//        }

//        public void SetParent(ucTableViewerContainer parent)
//        {
//            // реализовал на уровне связей VDataTable и ф-и HasChildrenUserChanges()
//        }

//        public void SetToolbarButtonVisible(string name, bool visible)
//        {
//            // возможно нужно сохранять состояния до инициализации тулбара
//            if (_toolbar_buttons_visible == null) return;

//            _toolbar_buttons_visible[name] = visible;

//            if (grid != null) grid.SetToolbarButtonVisible(name, visible);
//            //if (tree != null) tree.SetToolbarButtonVisible(name, visible);
//        }


//        public ucTableViewerContainer(ControlMode mode)
//        {
//            //_control = new ucReportGridWF(mode);
//            _control = UIStatic.GetControlsfactory().CreateTableViewer(this);
//            initControl(mode);

//            if (_mode == ControlMode.Data || _mode == ControlMode.Select)
//            {
//                ViewSelect = false;
//                TableSelect = false;

//            }

//            FillViewModes();
//        }

//        private void OnDisposed(/*object sender, EventArgs event_args*/)
//        {
//            if (empty != null)
//            {
//                empty.OpenExcel -= OnOpenExcel;
//                empty.Dispose();
//            }
//            if (grid != null) grid.GetControlAsWFControl().Dispose();
//            if (pivot != null) pivot.Dispose();
//            //if (tree != null) tree.Dispose();
//            //if (tree != null) tree.GetControlAsWFControl().Dispose();
//            if (excel != null) excel.Dispose();
//            if (ddesigner != null) ddesigner.Dispose();
//            if (dviewer != null) dviewer.Dispose();

//            if (_source != null) DetachDataSourceEvents();
//        }

//        public void ShowExcel(string path)
//        {
//            SetViewMode(TableViewMode.Excel);
//            excel.LoadDocument(path);
//        }
//        public void AllowOpenExcel()
//        {
//            SetViewMode(TableViewMode.Empty);
//            empty.SetOpenExcelButtonVisible(true);
//        }

//        public void OnOpenExcel(object sender, EventArgs args)
//        {
//            if (OpenExcel == null) return;

//            OpenExcel(this, EventArgs.Empty);
//        }

//        public void SetText(string text)
//        {
//            if (empty != null) empty.SetText(text);
//        }

//        public string CurrentViewMode()
//        {
//            // режим отображения: обычный/сводный
//            switch (getControlViewModeValueAsMode())
//            {
//                case TableViewMode.Default: return TextConst.AVViewModes.Default;
//                case TableViewMode.Pivot: return TextConst.AVViewModes.Pivot;
//                case TableViewMode.Tree: return TextConst.AVViewModes.Tree;
//                case TableViewMode.Excel: return TextConst.AVViewModes.Excel;
//                case TableViewMode.DashboardDesigner: return TextConst.AVViewModes.DashboardDesigner;
//                case TableViewMode.DashboardViewer: return TextConst.AVViewModes.DashboardViewer;
//            }

//            return null;
//        }

//        public void BeginUpdate()
//        {
//            BeginLayoutReloading();

//            if (grid != null) grid.BeginUpdate();
//            if (pivot != null) pivot.BeginUpdate();
//            //if (tree != null) tree.BeginUpdate();

//        }
//        public void EndUpdate()
//        {
//            if (grid != null) grid.EndUpdate();
//            if (pivot != null) pivot.EndUpdate();
//            //if (tree != null) tree.EndUpdate();

//            EndLayoutReloading();
//        }

//        public void HoldViewChanges()
//        {
//            if (grid != null) grid.HoldViewChanges();
//            //if (tree != null) tree.HoldViewChanges();
//        }
//        public void SetSelectMode()
//        {
//            // не используется
//        }
//        public bool IsSelectMode()
//        {
//            return (_mode == ControlMode.Select);
//        }
//        //public void RestoreViewChanges()
//        //{
//        //    if (grid != null) grid.RestoreViewChanges();
//        //    //if (tree != null) tree.RestoreViewChanges();
//        //}

//        public void ExportToXlsx(string fullpath, string caption = null)
//        {
//            switch (getControlViewModeValueAsMode())
//            {
//                case TableViewMode.Default:
//                    grid.ExportToXlsx(fullpath, caption);
//                    break;

//                case TableViewMode.Pivot:
//                    pivot.ExportToXlsx(fullpath, caption);
//                    break;

//                case TableViewMode.Tree:
//                    tree.ExportToXlsx(fullpath, caption);
//                    break;
//            }
//        }


//        public void CompareDataSets(VDataSet compared_ds)
//        {
//            BeginLayoutReloading();

//            if (getControlViewModeValueAsMode() != TableViewMode.Default)
//            {
//                SetViewMode(TableViewMode.Default);
//            }
//            ucGridContainerWFUtils.CompareDataSets(grid, compared_ds);
//           // grid.CompareDataSets(compared_ds);

//            EndLayoutReloading();
//        }
//        private static void addBandsIds(XElement xScheme)// пока здесь, нужно для подстановки значений переменных в заголовки
//        {
//            int i = 1;

//            foreach (XElement band in xScheme.Descendants(TextConst.EName.Band).ToArray())
//            {
//                if (band.Attribute(TextConst.AName.Id) == null)
//                {
//                    band.SetAttributeValue(TextConst.AName.Id, "band" + i.ToString());
//                    i++;
//                }
//            }
//        }

//        private class titVarTable
//        {
//            public string Name = null;
//            public List<titVarColumn> Columns = new List<titVarColumn>();
//        }

//        private class titVarColumn
//        {
//            public string Name = null;
//            public string originalTitle = null;
//            public List<string> VarNames = null;
//            // public XElement Element = null;
//        }

//        private List<titVarTable> _titVarInfo = null;

//        private static List<titVarTable> extractTitleVarNames(XElement scheme)
//        {
//            List<titVarTable> titVarInfo1 = null;

//            foreach (XElement xvc in scheme.DescendantsAndSelf(TextConst.EName.ViewColumns).ToArray())
//            {
//                var tabName = Cmn.GetAttrValue(xvc.Parent, TextConst.AName.As);
//                titVarTable tvtbl = null;
//                var xcols = xvc.Descendants(TextConst.EName.Column).ToList();
//                var xbands = xvc.Descendants(TextConst.EName.Band).ToList();
//                xcols.AddRange(xbands);

//                foreach (XElement xcol in xcols)
//                {
//                    var colName = Cmn.GetAttrValue(xcol, TextConst.AName.Name);
//                    if (colName == "")
//                    {
//                        colName = Cmn.GetAttrValue(xcol, TextConst.AName.Id);// для бендов name нельзя , удаляются доченние колонки при applyQube
//                    }
//                    if (colName == "")
//                    {
//                        addBandsIds(xvc);
//                        colName = Cmn.GetAttrValue(xcol, TextConst.AName.Id);
//                    }
//                    var tit = Cmn.GetAttrValue(xcol, TextConst.AName.Title);
//                    var tv = Cmn.ExtractParamsFromString(tit);
//                    if (tv != null)
//                    {
//                        if (titVarInfo1 == null)
//                        {
//                            titVarInfo1 = new List<titVarTable>();
//                        }
//                        if (tvtbl == null)
//                        {
//                            tvtbl = new titVarTable();
//                            tvtbl.Name = tabName;
//                            titVarInfo1.Add(tvtbl);
//                        }
//                        var tvcol = new titVarColumn();
//                        //  tvcol.Element = xcol;
//                        tvcol.Name = colName;
//                        tvcol.VarNames = tv;
//                        tvcol.originalTitle = tit;
//                        tvtbl.Columns.Add(tvcol);

//                    }
//                }
//            }
//            return titVarInfo1;

//        }

//        public static SortedList<string, string> GetTitlesTextDecode(XElement scheme, VDataSet pars)
//        {
//            var list = new SortedList<string, string>();
//            var tvi = extractTitleVarNames(new XElement(scheme));
//            if (tvi != null)
//            {
//                foreach (var tvtbl in tvi)
//                {
//                    foreach (var tvcol in tvtbl.Columns)
//                    {
//                        if (!list.ContainsKey(tvcol.originalTitle))
//                        {
//                            var newTit = tvcol.originalTitle;
//                            foreach (var s in tvcol.VarNames)
//                            {
//                                var val = pars.GetVariableValue(s).ToString();
//                                newTit = newTit.Replace("[:" + s + "]", val);

//                            }
//                            list.Add(tvcol.originalTitle, newTit);
//                        }
//                    }
//                }
//            }

//            return list;
//        }

//        public void UpdateTitles(VDataSet pars, bool onlyEmpty)
//        {
//            if (_titVarInfo == null) {
//                return;
//            }
//            if (this.GetDataTable() == null) {
//                return;
//            }
//            if (onlyEmpty && this.GetDataTable().Rows.Count != 0) { // иначе если сформировать отчет а потом изменить параметр,неправильные заголовки
//                return;
//            }
//            foreach (var tvtbl in _titVarInfo) {
//                foreach (var tvcol in tvtbl.Columns) {
//                    string newTit = tvcol.originalTitle;
//                    foreach (string s in tvcol.VarNames) {
//                        object val = pars.GetVariableValue(s);
//                        if (!Cmn.IsNullOrDBNull(val)) {
//                            newTit = newTit.Replace("[:" + s + "]", val.ToString());
//                        }
//                    }
//                    if (grid != null) {
//                        grid.SetColumnTitle(tvtbl.Name, tvcol.Name, newTit);
//                    }
//                }
//            }
//        }
//        private void LoadFromXml(XElement xScheme, VDataSet pars)
//        {
//            _xscheme = xScheme;
//            _titVarInfo = extractTitleVarNames(_xscheme);
//            //  addBandsNames(_xscheme);

//            var xroot = xScheme.Parent;
//            if (xroot == null)
//            {
//                xroot = new XElement("root");
//                xroot.Add(xScheme);
//            }

//            TableSelect = Parser.TableSelectFromScheme(xScheme);

//            if (_view_mode == TableViewMode.None)
//            {
//                string mode = _xscheme.AttrOrDef(TextConst.AName.ViewMode, TextConst.AVViewModes.Default);
//                if (mode != "") Enum.TryParse(mode, true, out _view_mode);
//                else _view_mode = TableViewMode.Default;
//            }

//            if (_top_table_name == null)
//            {
//                _top_table_name = Parser.MainTableFromScheme(_xscheme);
//            }

//            SetViewMode(_view_mode);

//            //BeginUpdate();
//            if (grid != null) grid.LoadFromXml(xScheme, (_source != null));
//            if (pivot != null) pivot.LoadFromXml(xScheme);
//            //if (tree != null) tree.LoadFromXml(xScheme, (_source != null));
//            if (pars != null)
//            {
//                UpdateTitles(pars,false);
//            }
//            //EndUpdate();
//        }
        
//        public void SaveToXml(XElement xScheme)
//        {
//            xScheme.SetAttributeValue(TextConst.AName.ViewMode, CurrentViewMode());
//            // главная таблица
//            xScheme.SetAttributeValue("main", _top_table_name);
//            // возможность менять главную таблицу
//            xScheme.SetAttributeValue("table_select", _table_select ? "1" : "0");

//            if (grid != null) grid.SaveToXml(xScheme);
//            else
//            {
//                var xScheme2 = new XElement(_xscheme);
//                xScheme.ReplaceWith(xScheme2);
//                xScheme = xScheme2;
//            }

//            if (pivot != null) pivot.SaveToXml(xScheme);
//            //if (tree != null) tree.SaveToXml(xScheme);
//        }
//       // TableViewMode _view_mode_old = null;
//        void ViewModeChanged(TableViewMode mode)
//        {
//            BeginLayoutReloading();

//            _view_mode = mode;

//            var ctrls = new List<object>();
//            if (empty != null) ctrls.Add(empty);
//            if (grid != null) ctrls.Add(grid.GetControlAsWFControl());
//            if (pivot != null) ctrls.Add(pivot);
//            //if (tree != null) ctrls.Add(tree);
//            //if (tree != null) ctrls.Add(tree.GetControlAsWFControl());
//            if (excel != null) ctrls.Add(excel);
//            if (ddesigner != null) ctrls.Add(ddesigner);
//            if (dviewer != null) ctrls.Add(dviewer);

//            var bars = new List<Bar>();

//            addControlBarFooterToList(bars);


//            var visible_ctrls = new List<object>();
//            var visible_bars = new List<Bar>();

//            switch (mode)
//            {
//                case TableViewMode.Empty:
//                    if (empty == null)
//                    {
//                        CreateEmpty();
//                        ctrls.Add(empty);
//                    }
//                    visible_ctrls.Add(empty);
//                    if (_global_footer_visible) addControlBarFooterToList(visible_bars);
//                    break;
//                case TableViewMode.Default:
//                    if (grid == null)
//                    {
//                        CreateGrid(false);
//                        ctrls.Add(grid.GetControl());
//                    }
//                    visible_ctrls.Add(grid.GetControl());
//                    if (_global_footer_visible) addControlBarFooterToList(visible_bars);
//                    break;

//                case TableViewMode.Pivot:
//                    if (pivot == null)
//                    {
//                        CreatePivot();
//                        ctrls.Add(pivot);
//                    }
//                    visible_ctrls.Add(pivot);
//                    if (_global_footer_visible) addControlBarFooterToList(visible_bars);
//                    break;

//                case TableViewMode.Tree:
//                    if (tree == null)
//                    {
//                       // CreateTree();
//                        CreateGrid(true);
//                        ctrls.Add(tree.GetControl());
//                    }
//                    visible_ctrls.Add(tree.GetControl());
//                    if (_global_footer_visible) addControlBarFooterToList(visible_bars);
//                    break;
//                case TableViewMode.Excel:
//                    if (excel == null)
//                    {
//                        CreateExcel();
//                        ctrls.Add(excel);
//                    }
//                    visible_ctrls.Add(excel);
//                    if (_global_footer_visible) addControlBarFooterToList(visible_bars);
//                    break;
//                case TableViewMode.DashboardDesigner:
//                    if (ddesigner == null)
//                    {
//                        ddesigner = new ucDashboardDesigner() { Visible = false, Dock = DockStyle.Fill };
//                        addChild(ddesigner);
//                        // Controls.Add(ddesigner);
//                        ctrls.Add(ddesigner);
//                    }
//                    if (!ddesigner.Loaded)
//                    {
//                        ddesigner.ReportName = ReportName;
//                        ddesigner.LoadDashboard();
//                    }
//                    ddesigner.SetData(_source.Tables[0]);
//                    visible_ctrls.Add(ddesigner);
//                    //ctrl.ReloadData();
//                    break;
//                case TableViewMode.DashboardViewer:
//                    if (dviewer == null)
//                    {
//                        dviewer = new ucDashboardViewer() { Visible = false, Dock = DockStyle.Fill };
//                        addChild(dviewer);
//                        //Controls.Add(dviewer);
//                        ctrls.Add(dviewer);
//                    }
//                    if (!dviewer.Loaded)
//                    {
//                        dviewer.ReportName = ReportName;
//                        dviewer.LoadDashboard();
//                    }
//                    dviewer.SetData(_source.Tables[0]);
//                    visible_ctrls.Add(dviewer);
//                    break;

//            }

//            EndLayoutReloading();

//            foreach (var ctrl in ctrls)
//            {
//                bool vis = visible_ctrls.Contains(ctrl);
//                if (ctrl is Control)// временно
//                {
//                    (ctrl as Control).Visible = vis;
//                }
//                else
//                {
//                    // (ctrl as IucTableViewer).SetVisible().. может не нужно пока не делвю
//                }
//            }
//            //ctrls.ForEach(c => c.Visible = visible_ctrls.Contains(c));
//            foreach (var b in bars) b.Visible = visible_bars.Contains(b);
//        }
//        void TopTableChanged(string top_table_name)
//        {
//            BeginLayoutReloading();

//            _top_table_name = top_table_name;

//            if (grid != null) grid.SetTopTable(top_table_name);
//            if (pivot != null) pivot.SetTopTable(top_table_name);
//            //if (tree != null) tree.SetTopTable(top_table_name);

//            EndLayoutReloading();
//        }

//        private void CreateEmpty()
//        {
//            empty = new ucEmpty() { Visible = false, Dock = DockStyle.Fill };
//            empty.SetDataSource(_source);
//            empty.OpenExcel += OnOpenExcel;
//            addChild(empty);
//            //Controls.Add(empty);
//        }
//        private void CreateGrid(bool isTree)
//        {
//            grid = new ucGridContainer(_mode, isTree);
//            grid.MainControl = this;//.getControl();
//            grid.BeginUpdate();
//            grid.SetFormName(_form_name);
//            grid.SetTopToolbarVisible(_toolbar_top_visible);
//            grid.SetFooterVisible(_footer_visible);
//            grid.SetSummaryVisible(_summary_visible);
//            grid.SetMultiselect(_multiselect);
//            grid.SetTitle(_title);
//            grid.SetOrderFieldName(_order_field_name);

//			grid.SetAllowSelectMoveColumns(_allowSelectMoveColumns);
//            grid.LoadFromXml(_xscheme);
//            grid.SetDataSource(_source);
//            grid.SetTopTable(_top_table_name);

//            grid.UpdateToolbar(_xtoolbarTop, _toolbar_top_items, false);
//            grid.UpdateToolbar(_xtoolbarBottom, _toolbar_bottom_items, true);

//            grid.UpdatePopupMenu(_xmenu, _menu_items);

//            grid.EndUpdate();

//            //grid.Layout += RaiseLayoutChanged;
//            grid.UIEvent2 += RaiseUIEvent;
//            grid.HasMessage += RaiseHasMessage;

//            grid.IsDxExport = isDxExport;
//            grid.CaptionForExport = captionForExport;
//            addChild(grid.GetControl());
//            //Controls.Add(grid);
//        }
//        private void CreatePivot()
//        {
//            pivot = new ucPivotNew() { Visible = false, Dock = DockStyle.Fill };
//            pivot.BeginUpdate();
//            pivot.LoadFromXml(_xscheme);
//            pivot.SetDataSource(_source);
//            pivot.SetTopTable(_top_table_name);

//            pivot.SetToolbarVisible(_toolbar_top_visible);
//            pivot.SetFooterVisible(_footer_visible);

//            pivot.EndUpdate();
//            //pivot.Layout += RaiseLayoutChanged;
//            //pivot.UIEvent2 += RaiseUIEvent;
//            addChild(pivot);
//            //Controls.Add(pivot);
//        }
//        //private Control.ControlCollection Controls
//        //{
//        //    get
//        //    {
//        //        return getControl().Controls;

//        //    }

//        //}

//        private void addChild(object control)
//        {
//            addChildToControl(control);
//        }
//        //private void CreateTree()
//        //{
//        //   // tree = new ucTreeNew(_mode) { Visible = false, Dock = DockStyle.Fill };

//        //    grid = new ucGridContainer(_mode);

//        //    tree.BeginUpdate();
//        //    tree.MainControl = this;//.getControl();
//        //    tree.SetFormName(_form_name);
//        //    tree.SetTopToolbarVisible(_toolbar_top_visible);
//        //    tree.SetFooterVisible(_footer_visible);
//        //    tree.SetSummaryVisible(_summary_visible);
//        //    tree.SetMultiselect(_multiselect);
//        //    tree.SetTitle(_title);
//        //    tree.SetOrderFieldName(_order_field_name);
//        //    tree.SetParentFieldName(_parent_field_name);
           
//        //    tree.SetExpandAllNodes(_expand_all_nodes);
//        //    //tree.SetAllowDragAndDrop(_allow_drag_and_drop);

//        //    tree.LoadFromXml(_xscheme);
//        //    tree.SetDataSource(_source);
//        //    tree.SetTopTable(_top_table_name);

//        //    tree.UpdateToolbar(_xtoolbarTop, _toolbar_top_items, false);
//        //    tree.UpdateToolbar(_xtoolbarBottom, _toolbar_bottom_items, true);

//        //    tree.UpdatePopupMenu(_xmenu, _menu_items);

//        //    tree.EndUpdate();

//        //    //tree.Layout += RaiseLayoutChanged;
//        //    tree.UIEvent2 += RaiseUIEvent;
//        //    tree.HasMessage += RaiseHasMessage;
//        //    addChild(tree.GetControl());
//        //    //Controls.Add(tree);
//        //}
//        private void CreateExcel()
//        {
//            excel = new ucExcelNew() { Visible = false, Dock = DockStyle.Fill };
//            excel.SetDataSource(_source);
//            excel.UIEvent2 += RaiseUIEvent;
//            addChild(excel);
//            //Controls.Add(excel);
//        }
//        public void ProcessSelectionChange()
//        {

//            if (GetDataTable().IsMultiselectionMember() || (_mode == ControlMode.Select))
//            {
//                UpdateDataSourceSelectedRows();
//            }
//        }
//        public void UpdateDataSourceSelectedRows()
//        {
//            if (_sourceSelectionProcessing) return;

//            switch (/*getControlViewModeValueAsMode()*/_view_mode)
//            {
//                case TableViewMode.Default:
//                    grid.UpdateDataSourceSelectedRows();
//                    break;



//                case TableViewMode.Tree:
//                    tree.UpdateDataSourceSelectedRows();
//                    break;
//            }
//        }
//        public void SetDataSourceSelectedRows(List<DataRow> rows)
//        {
//            if (_sourceSelectionProcessing) return;
//            GetDataTable().SelectedRows = rows;
//        }


//        public void SetViewMode(TableViewMode mode)
//        {
//            AddViewMode(mode);
//            VGCcbItem mode_new = _viewModes.First(item => item.ID == (int)mode);
//            if (!mode_new.Equals(/*getControlViewModeValue()*/_view_mode))
//            {

//                setControlViewModeValue(mode_new);
//                //getControl().cbViewMode.EditValue = mode_new;
//            }
//            else
//            {
//                // даже если значение не поменялось, событие должно сработать
//                //cbViewMode_EditValueChanged(null, null);
//                cbViewMode_EditValueChanged(mode);
//            }
//            if (mode == TableViewMode.Excel)
//            {
//                hideBars();
//            }
//        }
//        void SetTopTable(string top_table_name)
//        {
//            VGCcbItem top_table_new = (_top_table_name != null) ? _tableLevels.First(item => item.Name == top_table_name) : _tableLevels.First();
//            setControlTableLevelsValue(top_table_new);
//        }


//        void FillTableLevels()
//        {
//            //getControl().rcbTableLevels.Items.Clear();
//            clearControlTableLevels();
//            if (_source == null) return;
//            var tlList = new List<VGCcbItem>();
//            //_tableLevels = new VGCcbItem[_source.Tables.Count];
//            for (int i = 0; i < _source.Tables.Count; i++)
//            {
//                var dt = _source.Tables[i];
//                if (dt.TableName != TextConst.AVTable.Pars)
//                {
//                    int dt_level = 0;
//                    var tmp_dt = dt;
//                    while (tmp_dt.ParentRelations.Count > 0)
//                    {
//                        tmp_dt = tmp_dt.ParentRelations[0].ParentTable;
//                        dt_level++;
//                    }

//                    var tl = new VGCcbItem()
//                    {
//                        ID = i,
//                        Name = dt.TableName,
//                        Caption = (string)dt.ExtendedProperties["title"],
//                        Level = dt_level
//                    };
//                    tlList.Add(tl);
//                }
//            }
//            _tableLevels = tlList.ToArray();
//            setControlTableLevels(_tableLevels);
//            //getControl().rcbTableLevels.Items.AddRange(_tableLevels);
//        }
//        void FillViewModes()
//        {
//            var list = new List<VGCcbItem>();
            
//            if (XmlReports.IsDeveloperMode())
//            {
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.Empty, Name = GetViewModeName(TableViewMode.Empty) });
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.Default, Name = GetViewModeName(TableViewMode.Default) });
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.Tree, Name = GetViewModeName(TableViewMode.Tree) });
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.Pivot, Name = GetViewModeName(TableViewMode.Pivot) });
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.Excel, Name = GetViewModeName(TableViewMode.Excel) });
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.DashboardDesigner, Name = GetViewModeName(TableViewMode.DashboardDesigner) });
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.DashboardViewer, Name = GetViewModeName(TableViewMode.DashboardViewer) });
//            }

//            _viewModes = list.ToArray();
//            setControlViewModels(_viewModes);

//        }

//        string GetViewModeName(TableViewMode mode)
//        {
//            switch (mode)
//            {
//                case TableViewMode.Empty:
//                    return "Без данных";
//                case TableViewMode.Default:
//                    return "Обычный";
//                case TableViewMode.Tree:
//                    return "Дерево";
//                case TableViewMode.Pivot:
//                    return "Сводная таблица";
//                case TableViewMode.Excel:
//                    return "Excel";
//                case TableViewMode.DashboardDesigner:
//                    return "Dashboard Designer";
//                case TableViewMode.DashboardViewer:
//                    return "Dashboard Viewer";
//                default:
//                    return null;
//            }
//        }
//        void AddViewMode(TableViewMode mode)
//        {
//            if (_viewModes.Where(i => i.ID == (int)mode).Any()) return;
//             var list = _viewModes.ToList();

//            list.Add(new VGCcbItem() { ID = (int)mode, Name = GetViewModeName(mode) });
            

//            _viewModes = list.ToArray();
//            setControlViewModels(_viewModes);
//            if (mode == TableViewMode.Default)
//            {
//                AddViewMode(TableViewMode.Pivot);
//            }

//        }

//        public string GetTopTableName()
//        {
//            return _top_table_name;
//        }
//        public bool IsComparedMode()
//        {
//            return (grid != null && grid.IsCompareMode && _view_mode == TableViewMode.Default);
//        }

//        public void UpdateEvents(XElement xevents)
//        {
//            if (xevents == null) return;

//            _xevents = xevents;

//            foreach (XElement xcmd in xevents.Elements(TextConst.EName.UseAction))
//            {
//                var action = Cmn.GetActionInfo(xcmd);
//                AddEventTag(xcmd.Attribute(TextConst.AName.EventName).Value, action);
//            }
//        }
//        public void UpdateToolbar(XElement xtoolbar, IVBarItem[] items, bool bottom = false)
//        {
//            if (bottom)
//            {
//                _xtoolbarBottom = xtoolbar;
//                _toolbar_bottom_items = items;
//            }
//            else
//            {
//                _xtoolbarTop = xtoolbar;
//                _toolbar_top_items = items;
//            }

//            if (_toolbar_buttons_visible == null) _toolbar_buttons_visible = new Dictionary<string, bool>();
//            if (grid != null) grid.UpdateToolbar(xtoolbar, items, bottom);
//            //if (tree != null) tree.UpdateToolbar(xtoolbar, items, bottom);
//        }
//        public void UpdatePopupMenu(XElement xmenu, IVBarItem[] items)
//        {
//            _xmenu = xmenu;
//            _menu_items = items;

//            if (grid != null) grid.UpdatePopupMenu(xmenu, items);
//            //if (tree != null) tree.UpdatePopupMenu(xmenu, items);
//        }
//        #region Обработчики событий
//        //private void ucReportGridNew_Load(object sender, EventArgs e)
//        //{
//        //    //
//        //}

//        //private void cbViewMode_EditValueChanged(object sender, EventArgs e)
//        //{
//        //    ViewModeChanged(getControlViewModeValueAsMode());

//        //    if (IsTemplate && !FromFile) RaiseLayoutChanged(this, EventArgs.Empty);
//        //}

//        private void cbViewMode_EditValueChanged(object value)
//        {
//            ViewModeChanged((TableViewMode)value);

//            if (IsTemplate && !FromFile) RaiseLayoutChanged(this, EventArgs.Empty);
//        }

//        //private void cbTableLevels_EditValueChanged(object sender, EventArgs e)
//        //{
//        //    TopTableChanged(getControlTableLevelsValueName());

//        //    if (IsTemplate && !FromFile) RaiseLayoutChanged(this, EventArgs.Empty);
//        //}

//        private void cbTableLevels_EditValueChanged(object value)
//        {
//            TopTableChanged((string)value);

//            if (IsTemplate && !FromFile) RaiseLayoutChanged(this, EventArgs.Empty);
//        }
//        //private void rcbTableLevels_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
//        //{
//        //    e.DisplayText = e.DisplayText.Trim();
//        //}

//        #endregion
//        public void AcceptChanges()
//        {
//            if (grid != null) grid.AcceptChanges();
//        }
//        public void DismissChanges(Dictionary<DataRow, OracleException> error_rows, bool get_my_errors = false)
//        {
//            if (grid != null) grid.DismissChanges(error_rows, get_my_errors);
//        }


//        public void AcceptSelection()
//        {
//            if (grid != null) grid.UpdateDataSourceSelectedRows();
//            //if (tree != null) tree.UpdateDataSourceSelectedRows();

//            GetDataTable().GetDataSet().ChoiceSource = GetDataTable();

//            closeForm();
//        }
//        static bool _sourceSelectionProcessing = false;
//        public void SetSelectionFromSource()
//        {
//            _sourceSelectionProcessing = true;
//            List<object> values = new List<object>();
//            foreach (var r in GetDataTable().SelectedRows)
//            {
//                values.Add(r[GetDataTable().PrimaryKey[0]]);
//            }
//            SetSelection(values.ToArray());
//            _sourceSelectionProcessing = false;
//        }
//        public void SetSelection(IEnumerable<object> values)
//        {
//            if (grid != null) grid.SetSelection(values);
//            //if (tree != null) tree.SetSelection(values);
//        }
//        #region IReportGrid
//        public string ReportForm
//        {
//            get { return null; }
//        }
//        public bool IsCompareMode
//        {
//            get { return IsComparedMode(); }
//        }
//        //public bool IsVisible
//        //{
//        //    get { return true; }
//        //}
//        public VLayoutControlContainerInfo LayoutContainer { get; set; }
//        public bool IsVisibleInLayout()
//        {
//            return LayoutContainer.IsVisible();
//        }
//        public VDataSet DataSource
//        {
//            get { return GetDataSource(); }
//            set { SetDataSource(value); }
//        }

//        //public GridView CurrentView
//        //{
//        //    // вроде разницы нет
//        //    get
//        //    {

//        //        return (grid != null) ? grid.GetMainView() : null;
//        //    }
//        //}
//        public IucGrid GetGridControl()
//        {
//            if (grid == null) return null;
//            if (grid.GetControl() == null) return null;
           
//            return grid.GetControl().GetGrid();

//        }
//        //private GridView GetMainView()
//        //{
//        //    return (grid != null) ? grid.GetMainView() : null;
//        //}
//        public void SetComparedMode(VDataSet ds)
//        {
//            CompareDataSets(ds);
//        }
//        public void AddDataSourceChangedHandler(Action action, EventHandler action2)
//        {
//            DataSourceChanged += action2;
//        }
//        public void RemoveDataSourceChangedHandler(Action action, EventHandler action2)
//        {
//            DataSourceChanged -= action2;
//        }
//        public void AddUIEventHandler(UIEventHandler action)
//        {
//            UIEvent += action;
//        }
//        public void RemoveUIEventHandler(UIEventHandler action)
//        {
//            UIEvent -= action;
//        }
//        public void AddLayoutChangedHandler(Action action, EventHandler action2)
//        {
//            LayoutChangedEvent += action2;
//        }
//        public void RemoveLayoutChangedHandler(Action action, EventHandler action2)
//        {
//            LayoutChangedEvent -= action2;
//        }
//        public TableViewMode ViewMode
//        {
//            get { return _view_mode; }
//        }

//        // используется в mode = Report
//        public void Initialize(Dictionary<string, string> report_info, bool from_file)
//        {
//            _report_info = report_info;
//            FromFile = from_file;

//            if (XmlReports.IsDeveloperMode() && ReportName == "test-for-dxdashboard")
//            {
//                ddesigner = new ucDashboardDesigner() { Dock = DockStyle.Fill, Visible = false };
//                addChild(ddesigner);
//                //Controls.Add(ddesigner);
//                dviewer = new ucDashboardViewer() { Dock = DockStyle.Fill, Visible = false };
//                addChild(dviewer);
//                //Controls.Add(dviewer);
//            }
//        }

//        IucTableViewerContainer _control;
//        private IucTableViewerContainer getControl()
//        {
//            return _control;
//        }
//        //private ucReportGridNew getControl()
//        //{
//        //    return (ucReportGridNew)_control;
//        //}
//        public IucTableViewerContainer GetControl()
//        {
//            return getControl();
//            //return getControlAsWFControl();
//        }


//        private void hideBars()
//        {
//            GetControl().HideBars();
//        }
//        public BarManager GetBarManager()
//        {
//            return getControlAsWFControl().barManager;
//        }
//        public void GenerateLayoutChanged()
//        {
//            RaiseLayoutChanged(this, EventArgs.Empty);
//        }
//        public void SaveSchemeSettingsToXml(XElement xroot)
//        {
//            SaveToXml(xroot.Element(TextConst.EName.Scheme));
//        }
//        public void LoadSchemeSettingsFromXml(XElement xroot, VDataSet pars)
//        {
//            LoadFromXml(xroot.Element(TextConst.EName.Scheme), pars);
//        }

//        #endregion


//        public void UpdateDummyColumnWidth()
//        {
//            if (UIStatic.IsWeb()) return;
//            switch (getControlViewModeValueAsMode())
//            {
//                case TableViewMode.Default:
//                    //grid.UpdateDummyColumnWidth(grid.GetMainView());
//                    grid.ApplyHeaderLayout();

//                    break;



//                case TableViewMode.Tree:
//                    //tree.UpdateDummyColumnWidth();
//                    tree.ApplyHeaderLayout();
//                    break;
//            }
//        }
//        private SortedList<string, bool> _columnsVisibility = new SortedList<string, bool>();




//        private bool getColumnVisibility(string columnName)
//        {

//            if (_columnsVisibility.ContainsKey(columnName))
//            {
//                return _columnsVisibility[columnName];
//            }
//            else
//            {
//                return true;
//            }
//        }
//        public void SetColumnVisibility(string columnName, bool value)// временное решение - не продумано
//        {
//            if (getColumnVisibility(columnName) == value)
//            {
//                return;
//            }
//            else
//            {
//                _columnsVisibility[columnName] = value;
//            }
//            bool gridOrTree = false;
//            if (UIStatic.IsWeb())
//            {
//                gridOrTree = true;
//            }
//            else
//            {
//                switch (getControlViewModeValueAsMode())
//                {
//                    case TableViewMode.Default:
//                        //grid.GetMainView().Columns[columnName].Visible = value;

//                        gridOrTree = true;
//                        break;



//                    case TableViewMode.Tree:
//                        gridOrTree = true;
//                        //tree.GetMainView().Columns[columnName].Visible = value;
//                        break;
//                }
//            }
//            if (gridOrTree)
//            {
//                var col = grid.GetGrid().GetColumnByFieldName(columnName);
//                grid.GetGrid().SetColumnVisible(col, value);
//            }
//            UpdateDummyColumnWidth();
//        }


//        public void SetFocusedCell(string columnName, DataRow row)
//        {
//            switch (getControlViewModeValueAsMode())
//            {
//                case TableViewMode.Default:
//                    grid.SetFocusedCell(columnName, row);
//                    break;
//                case TableViewMode.Tree:
//                    tree.SetFocusedCell(columnName, row);
//                    break;
//            }
//        }




//        public bool IsCheckBoxSelection()
//        {
//            return _mode == ControlMode.Select;
//        }
//    }

//    public enum ControlMode
//    {
//        None,
//        Report,
//        Data,
//        Select
//    }
//    internal enum TableViewMode
//    {
//        None,
//        Empty,
//        Default,
//        Pivot,
//        Tree,
//        Excel,
//        DashboardDesigner,
//        DashboardViewer
//    }

//    internal class UIEventArgs2 : EventArgs
//    {
//        public string Name { get; set; }
//        public string TableName { get; set; }
//        public VDataTable Table { get; set; }
//        public VDataColumn Column { get; set; }
//        public DataRow Row { get; set; }
//    }
//    internal delegate bool UIEventHandler2(object sender, UIEventArgs2 args);

//    internal class HasMessageArgs : EventArgs
//    {
//        public string Message { get; set; }
//    }
//    internal delegate void HasMessageHandler(object sender, HasMessageArgs args);
//}

