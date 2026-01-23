//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Xml.Linq;
////using System.Windows.Forms;
//using DevExpress.Data;
//using DevExpress.Skins;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraPivotGrid;

//using DevExpress.XtraTreeList;
//using DevExpress.XtraTreeList.Columns;
//using DevExpress.XtraTreeList.Nodes;
//using infoenergo.core.Extensions;
//using infoenergo.sys;
//using sql.builder.Controls.Grids;
//using sql.builder.Controls.Grids.ReportViewModes;
//using sql.builder.Controls.Grids.ReportViewModes.Dashboard;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls
//{
//   //  !!! все, работать не будет, класс не актуален
//    internal partial class ucReportGridOld : ucGridBase/* IReportGrid,*/
//    {
//        public VVariableDepandantceController GetVariableDepandantceController()
//        {
//            return null;
//        }

//        public event EventHandler OpenExcel;
//        public void OnOpenExcel(object sender, EventArgs args)
//        {
//            throw new NotImplementedException();
//        }

//        public void UpdateDummyColumnWidth()
//        {

//        }
//        #region Закрытые переменные
//        // контролы подгружаются динамически
//        internal ucGridOld grid;
//        internal ucPivot pivot;
//        internal ucTree tree;
//        internal ucExcel excel;
//        internal ucDashboardDesigner ddesigner;
//        internal ucDashboardViewer dviewer;

//        private VGCcbItem[] _viewModes;
//        private VGCcbItem[] _tableLevels;

//        private string _top_table_name;
//        private TableViewMode _view_mode = TableViewMode.None;

//        private bool _child_tabs;
//        private bool _table_select;
//        private bool _data_edit;

//        public Dictionary<string, IEnumerable<PivotGridField>> pivot_fields;
//        public Dictionary<string, IEnumerable<TreeListColumn>> tree_columns;

//        XElement _xroot;
//        Dictionary<string, string> _settings;
//        #endregion
//        #region Свойства
//        public void RemoveLayoutChangedHandler(Action action, EventHandler action2)
//        {
//            LayoutChanged -= action;
//        }
//        public void AddHasMessageHandler(Action<string> action, HasMessageHandler action2)
//        {
//            HasMessage += action;
//        }
//        public void RemoveHasMessageHandler(Action<string> action, HasMessageHandler action2)
//        {
//            HasMessage -= action;
//        }
//        /// <summary>
//        /// Текущий режим отображения данных 
//        /// </summary>
//        public TableViewMode ViewMode
//        {
//            get { return (TableViewMode)((VGCcbItem)cbTableViewMode.EditValue).ID; }
//        }
//        public string TopTableName
//        {
//            get { return _top_table_name; }
//        }
//        public GridView CurrentView
//        {
//            get { return (grid != null) ? (GridView)grid.Grid.Views.First(v => v.Name == _top_table_name) : null; }
//        }
//        #endregion
//        #region События
//        //
//        #endregion
//        #region Открытые методы
//        public ucReportGridOld()
//        {
//            InitializeComponent();
//            pivot_fields = new Dictionary<string, IEnumerable<PivotGridField>>();
//            tree_columns = new Dictionary<string, IEnumerable<TreeListColumn>>();
//            Disposed += OnDisposed;
//        }

//        private void OnDisposed(object sender, EventArgs event_args)
//        {
//            if (grid != null)
//            {
//                // от утечек
//                // https://www.devexpress.com/Support/Center/Question/Details/Q534989
//                foreach (var gv in grid.Grid.ViewCollection.Cast<GridView>()) gv.Dispose();
//            }
//        }

//        public override void Initialize(Dictionary<string, string> report_info, bool from_file)
//        {
//            base.Initialize(report_info, from_file);
//            if (XmlReports.IsDeveloperMode() && ReportName == "test-for-dxdashboard")
//            {
//                ddesigner = new ucDashboardDesigner() { Dock = DockStyle.Fill, Visible = false };
//                Controls.Add(ddesigner);
//                dviewer = new ucDashboardViewer() { Dock = DockStyle.Fill, Visible = false };
//                Controls.Add(dviewer);
//            }

//            //FillTableViewModes();
//        }
//        public Control GetControl()
//        {
//            return this;
//        }
//        public void SetDock(DockStyle dock)
//        {
//            Dock = dock;
//        }

//        public void ShowExcel(string path)
//        {
//            excel.Excel.LoadDocument(path);
//            SetViewMode(TableViewMode.Excel);
//        }

//        public void AllowOpenExcel()
//        {
//            throw new NotImplementedException();
//        }

//        public void SetText(string text)
//        {
//            //
//        }
//        public void SetParent(ucTableViewerContainer parent)
//        {
//            //
//        }
//        public void SetSelectMode()
//        {
//            throw new NotImplementedException();
//        }
//        public bool IsSelectMode()
//        {
//            throw new NotImplementedException();
//        }

//        public string CurrentViewMode()
//        {
//            // режим отображения: обычный/сводный
//            switch ((TableViewMode)((VGCcbItem)cbTableViewMode.EditValue).ID)
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

//        public static TableViewMode ConvertViewMode(string mode)
//        {
//            // режим отображения: обычный/сводный
//            switch (mode)
//            {
//                case TextConst.AVViewModes.Default: return TableViewMode.Default;
//                case TextConst.AVViewModes.Pivot: return TableViewMode.Pivot;
//                case TextConst.AVViewModes.Tree: return TableViewMode.Tree;
//                case TextConst.AVViewModes.Excel: return TableViewMode.Excel;
//                case TextConst.AVViewModes.DashboardDesigner: return TableViewMode.DashboardDesigner;
//                case TextConst.AVViewModes.DashboardViewer: return TableViewMode.DashboardViewer;
//            }

//            return TableViewMode.Default;
//        }

//        public override void BeginUpdate()
//        {
//            _layout_reloading = true;

//            if (grid != null)
//            {
//                foreach (GridView view in grid.Grid.ViewCollection)
//                {
//                    view.SelectionChanged -= view_SelectionChanged;
//                    view.MouseDown -= view_MouseDown;
//                    view.Layout -= view_Layout;
//                    view.RowCellClick -= view_RowCellClick;
//                    view.CustomDrawCell -= view_CustomDrawCell;
//                    view.RowCellStyle -= view_RowCellStyle;
//                    //cur_view.CustomDrawFooterCell -= this.view_CustomDrawFooterCell;
//                    //cur_view.CustomDrawRowFooterCell -= this.view_CustomDrawRowFooterCell;
//                    view.CustomSummaryCalculate -= view_CustomSummaryCalculate;
//                    //cur_view.MouseUp -= this.view_MouseUp;
//                    //cur_view.CustomDrawColumnHeader -= this.view_CustomDrawColumnHeader;
//                }
//            }
//        }
//        public override void EndUpdate()
//        {
//            //if (_tableLevels != null)
//            //{
//            //    // даже если текущая таблица не поменялась, надо перезагрузить данные
//            //    var table_level = (grid != null) ?_tableLevels.First(lvl => lvl.Name == grid.Grid.MainView.Name) : _tableLevels.First();
//            //    if (cbTableLevels.EditValue != table_level)
//            //    {
//            //        cbTableLevels.EditValue = table_level;
//            //    }
//            //    else
//            //    {
//            //        cbTableLevels_EditValueChanged(null, null);
//            //    }
//            //}

//            if (grid != null)
//            {
//                foreach (GridView cur_view in grid.Grid.ViewCollection)
//                {
//                    cur_view.SelectionChanged += this.view_SelectionChanged;
//                    cur_view.MouseDown += this.view_MouseDown;
//                    cur_view.Layout += this.view_Layout;
//                    cur_view.RowCellClick += this.view_RowCellClick;
//                    cur_view.CustomDrawCell += this.view_CustomDrawCell;
//                    cur_view.RowCellStyle += view_RowCellStyle;
//                    //cur_view.CustomDrawFooterCell += this.view_CustomDrawFooterCell;
//                    //cur_view.CustomDrawRowFooterCell += this.view_CustomDrawRowFooterCell;
//                    cur_view.CustomSummaryCalculate += view_CustomSummaryCalculate;

//                    //cur_view.MouseUp += this.view_MouseUp;
//                    //cur_view.CustomDrawColumnHeader += this.view_CustomDrawColumnHeader;
//                    cur_view.UpdateTotalSummary();

//                    attachViewEvents(cur_view);
//                }
//            }
//            _layout_reloading = false;
//        }
//        public override void ExportToXlsx(string fullpath, string caption = null)
//        {
//            switch ((TableViewMode)((VGCcbItem)cbTableViewMode.EditValue).ID)
//            {
//                case TableViewMode.Default:
//                    var view = (BandedGridView)grid.Grid.MainView;

//                    var table_name = view.ViewCaption;
//                    view.ViewCaption = caption;
//                    view.OptionsView.ShowViewCaption = true;

//                    grid.Grid.ExportToXlsx(fullpath);

//                    view.OptionsView.ShowViewCaption = false;
//                    view.ViewCaption = table_name;


//                    break;

//                case TableViewMode.Pivot:
//                    pivot.Pivot.ExportToXlsx(fullpath);
//                    break;

//                case TableViewMode.Tree:
//                    tree.Tree.ExportToXlsx(fullpath);
//                    break;
//            }
//        }
//        public GridView GetMainView()
//        {
//            return grid.Grid.MainView as GridView;
//        }
//        public override void SetComparedMode(VDataSet compared_ds)
//        {
//            _layout_reloading = true;
//            switch ((TableViewMode)((VGCcbItem)cbTableViewMode.EditValue).ID)
//            {
//                case TableViewMode.Default:
//                    grid.Grid.DataSource = null;
//                    //if (_compare_mode)
//                    //{
//                    //    GridDesigner.RestoreComparedGridView(defaultGrid);
//                    //}
//                    var dict = new Dictionary<string, string[]>();
//                    foreach (DataTable table in _source.Tables) {
//                        dict.Add(table.TableName, table.PrimaryKey.Select<DataColumn, string>(Cmn.GetDataColumnName));
//                    }
//                    GridDesigner.SetComparedGridView(grid.Grid, dict, _source.Scheme.Attribute(AName.timestamp).Value, compared_ds.Scheme.Attribute(AName.timestamp).Value);

//                    _source = GridDesigner.CompareDataSets(_source, compared_ds);
//                    grid.Grid.DataSource = _source.Tables[_top_table_name];

//                    _compare_mode = true;
//                    break;
//                case TableViewMode.Pivot:
//                case TableViewMode.Tree:
//                    break;
//            }

//            // выводим колличество различий
//            var divergences_count = 0;
//            foreach (BandedGridView view in grid.Grid.ViewCollection)
//            {
//                var result_columns = view.Columns.Cast<GridColumn>()
//                    .Where(col => col.FieldName.EndsWith("_result") && !String.IsNullOrEmpty(col.UnboundExpression));
//                for (int i = 0; i < view.RowCount; i++) {
//                    divergences_count += result_columns.Count(result_column => !Cmn.DECIMAL_ZERO.Equals(view.GetRowCellValue(i, result_column.FieldName)));
//                }
//            }
//            lCompareResult.Caption = "Расхождений всего: " + divergences_count;
//            lCompareResult.Visibility = BarItemVisibility.Always;

//            _layout_reloading = false;
//        }
//        public void AddDataSourceChangedHandler(Action action, EventHandler action2)
//        {
//            DataSourceChanged += action;
//        }
//        public void RemoveDataSourceChangedHandler(Action action, EventHandler action2)
//        {
//            DataSourceChanged -= action;
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
//            LayoutChanged += action;
//        }
//        public override void SetFormingTime(string time)
//        {
//            lFormingTime.Caption = time != null
//                    ? "Сформирован за " + time
//                    : String.Empty;
//        }

//        public void SetPrintingTime(string time)
//        {
//            throw new NotImplementedException();
//        }

//        public string GetFormingTime()
//        {
//            throw new NotImplementedException();
//        }

//        public override void SetAvgFormingTime(string time)
//        {
//            lAvgFormingTime.Caption = time != null
//                    ? "Среднее время формирования " + time
//                    : String.Empty;
//        }
//        public override void GenerateLayoutChanged()
//        {
//            if (_layout_reloading) return;

//            if (IsTemplate && !_from_file)
//            {
//                SetLayoutChanged();
//            }
//        }
//        public override void LoadSchemeSettingsFromXml(XElement xRoot)
//        {
//            _xroot = xRoot;
//            var xScheme = xRoot.Element(TextConst.EName.Scheme);

//            _table_select = Parser.TableSelectFromScheme(xScheme);
//            cbTableLevels.Visibility = _table_select ? BarItemVisibility.Always : BarItemVisibility.Never;
//            _child_tabs = Parser.ChildTabsFromScheme(xScheme);

//            if (grid != null)
//            {

//                foreach (GridView view in grid.Grid.ViewCollection)
//                {
//                    detachViewEvents(view);
//                }
//                //Parser.LoadGridSettingsFromXml(_xroot, grid.Grid); // все, работать не будет
//                UpdateGrid();
//            }
//            if (pivot != null)
//            {
//                Parser.LoadPivotSettingsFromXml(_xroot, pivot_fields);
//                UpdatePivot();
//            }
//            if (tree != null)
//            {
//                Parser.LoadTreeSettingsFromXmlOld(_xroot, tree_columns);
//                UpdateTree();
//            }
//        }
//        public override void SaveSchemeSettingsToXml(XElement xRoot)
//        {
//            var xScheme = xRoot.Element("scheme");
//            xScheme.SetAttributeValue(TextConst.AName.ViewMode, CurrentViewMode());
//            // главная таблица
//            xScheme.SetAttributeValue("main", _top_table_name);
//            // возможность менять главную таблицу
//            xScheme.SetAttributeValue("table_select", _table_select ? "1" : "0");
//            // отображать закладки для дочерних таблиц
//            xScheme.SetAttributeValue("child_tabs", _child_tabs ? "1" : "0");

//            if (grid != null) Parser.SaveGridSettingsToXml(xScheme, grid.Grid);
//            else xScheme.ReplaceWith(_xroot.Element("scheme"));

//            if(pivot != null)Parser.SavePivotSettingsToXml(xRoot, pivot_fields);
//            if (tree != null) Parser.SaveTreeSettingsToXml(_xroot.Element("scheme"), tree_columns);
//        }
//        public override GridView GetMainGridView()
//        {
//            return (grid != null) ? (grid.Grid.MainView as GridView) : null;
//        }

//        public override void UpdateRibbon()
//        {
//            var ctrl = GetRibbonSource<ucReportGridOld>();
//            ctrl.rpgActionDemo.Visible = XmlReports.IsDeveloperMode();
//            popup.Ribbon = ctrl.SearchTopRibbon();
//        }
//        #endregion
//        #region Закрытые методы
//        void ViewModeChanged(TableViewMode mode)
//        {
//            _view_mode = mode;

//            teTotalCount.EditValue = 0;
//            teTotalSum.EditValue = Cmn.DECIMAL_ZERO;

//            var ctrls = new List<Control>();
//            if(grid != null) ctrls.Add(grid);
//            if(pivot != null) ctrls.Add(pivot);
//            if(tree != null) ctrls.Add(tree);
//            if(excel != null) ctrls.Add(excel);
//            if(ddesigner != null) ctrls.Add(ddesigner);
//            if(dviewer != null) ctrls.Add(dviewer);

//            var bars = new Bar[] { barFooter }.ToList();

//            var visible_ctrls = new List<Control>();
//            var visible_bars = new List<Bar>();

//            var tmp = _layout_reloading;
//            _layout_reloading = true;

//            switch (mode)
//            {
//                case TableViewMode.Default:
//                    if (grid == null)
//                    {
//                        grid = new ucGridOld() {Visible = false, Dock = DockStyle.Fill};
//                        grid.Grid.Paint += grid_Paint;
//                        //Parser.LoadGridSettingsFromXml(_xroot, grid.Grid); !!! все работать не будет
//                        UpdateGrid();
//                        Controls.Add(grid);
//                        ctrls.Add(grid);
//                    }
//                    visible_ctrls.Add(grid);
//                    visible_bars.Add(barFooter);
//                    break;

//                case TableViewMode.Pivot:
//                    if (pivot == null)
//                    {
//                        pivot = new ucPivot() { Visible = false, Dock = DockStyle.Fill };
//                        pivot.Pivot.Layout += view_Layout;
//                        pivot.Pivot.CellSelectionChanged += pivotGrid_CellSelectionChanged;
//                        Parser.LoadPivotSettingsFromXml(_xroot, pivot_fields);
//                        UpdatePivot();
//                        Controls.Add(pivot);
//                        ctrls.Add(pivot);
//                    }
//                    visible_ctrls.Add(pivot);
//                    visible_bars.Add(barFooter);
//                    break;

//                case TableViewMode.Tree:
//                    if (tree == null)
//                    {
//                        tree = new ucTree() { Visible = false, Dock = DockStyle.Fill };
//                        tree.Tree.Layout += view_Layout;
//                        tree.Tree.SelectionChanged += treeGrid_SelectionChanged;
//                        tree.Tree.Paint += tree_Paint;
//                        Parser.LoadTreeSettingsFromXmlOld(_xroot, tree_columns);
//                        UpdateTree();
//                        Controls.Add(tree);
//                        ctrls.Add(tree);
//                    }
//                    visible_ctrls.Add(tree);
//                    visible_bars.Add(barFooter);
//                    break;
//                case TableViewMode.Excel:
//                    if (excel == null)
//                    {
//                        excel = new ucExcel() { Visible = false, Dock = DockStyle.Fill };
//                        excel.Excel.HyperlinkClick += excelGrid_HyperlinkClick;
//                        Controls.Add(excel);
//                        ctrls.Add(excel);
//                    }
//                    visible_ctrls.Add(excel);
//                    visible_bars.Add(barFooter);
//                    break;
//                case TableViewMode.DashboardDesigner:
//                    if (ddesigner == null)
//                    {
//                        ddesigner = new ucDashboardDesigner() { Visible = false, Dock = DockStyle.Fill };
//                        Controls.Add(ddesigner);
//                        ctrls.Add(ddesigner);
//                    }
//                    if (!ddesigner.Loaded)
//                    {
//                        ddesigner.ReportName = ReportName;
//                        ddesigner.LoadDashboard();
//                    }
//                    ddesigner.SetData(DataSource.Tables[0]);
//                    visible_ctrls.Add(ddesigner);
//                    //ctrl.ReloadData();
//                    break;
//                case TableViewMode.DashboardViewer:
//                    if (dviewer == null)
//                    {
//                        dviewer = new ucDashboardViewer() { Visible = false, Dock = DockStyle.Fill };
//                        Controls.Add(dviewer);
//                        ctrls.Add(dviewer);
//                    }
//                    if (!dviewer.Loaded)
//                    {
//                        dviewer.ReportName = ReportName;
//                        dviewer.LoadDashboard();
//                    }
//                    dviewer.SetData(DataSource.Tables[0]);
//                    visible_ctrls.Add(dviewer);
//                    break;
//            }

//            _layout_reloading = tmp;

//            foreach (var c in ctrls) c.Visible = visible_ctrls.Contains(c);
//            foreach (var b in bars) b.Visible = visible_bars.Contains(b);
//        }
//        void TableLevelChanged(string table_level)
//        {
//            _top_table_name = table_level;

//            teTotalCount.EditValue = 0;
//            teTotalSum.EditValue = Cmn.DECIMAL_ZERO;

//            if (grid != null) UpdateGrid();
//            if (pivot != null) UpdatePivot();
//            if (tree != null) UpdateTree();
//        }

//        public void SetViewMode(TableViewMode mode)
//        {
//            VGCcbItem mode_new = _viewModes.First(item => item.ID == (int)_view_mode);
//            cbTableViewMode.EditValue = mode_new;
//        }
//        void SetTableLevel(string table_level)
//        {
//            VGCcbItem table_level_new = _tableLevels.First(item => item.Name == table_level);
//            cbTableLevels.EditValue = table_level_new;
//        }

//        void UpdateGrid()
//        {
//            //GridDesigner.SetGridTopTable(grid.Grid, _source.Tables[_top_table_name]);
//            foreach (var v in grid.grid.ViewCollection.Cast<BandedGridView>()) v.OptionsDetail.ShowDetailTabs = _child_tabs;
//            grid.Grid.DataSource = _source.Tables[_top_table_name];
//        }

//        void UpdatePivot()
//        {
//            var tmp = _layout_reloading;
//            _layout_reloading = true;

//            pivot.Pivot.BeginUpdate();
//            pivot.Pivot.Fields.Clear();
//            pivot.Pivot.Fields.AddRange(pivot_fields[_top_table_name].ToArray());
//            pivot.Pivot.DataSource = _source.Tables[_top_table_name];
//            pivot.Pivot.EndUpdate();

//            _layout_reloading = tmp;
//        }
//        void UpdateTree()
//        {
//            tree.Tree.Columns.Clear();
//            tree.Tree.Columns.AddRange(tree_columns[_top_table_name].ToArray());

//            var node_id = tree_columns[_top_table_name].FirstOrDefault(col => col.Name == "node-id");
//            if (node_id != null) tree.Tree.KeyFieldName = node_id.FieldName;
//            var node_pid = tree_columns[_top_table_name].FirstOrDefault(col => col.Name == "parent-node-id");
//            if (node_pid != null) tree.Tree.ParentFieldName = node_pid.FieldName;
//            tree.Tree.DataSource = _source.Tables[_top_table_name];
//        }

//        void FillTableLevels()
//        {
//            if (_source == null) return;

//            _top_table_name = _top_table_name ?? Parser.MainTableFromScheme(_xroot.Element("scheme"));

//            rcbTableLevels.Items.Clear();
//            _tableLevels = new VGCcbItem[DataSource.Tables.Count];
//            for (int i = 0; i < DataSource.Tables.Count; i++)
//            {
//                var dt = DataSource.Tables[i];

//                int dt_level = 0;
//                var tmp_dt = dt;
//                while (tmp_dt.ParentRelations.Count > 0)
//                {
//                    tmp_dt = tmp_dt.ParentRelations[0].ParentTable;
//                    dt_level++;
//                }

//                _tableLevels[i] = new VGCcbItem()
//                {
//                    ID = i,
//                    Name = dt.TableName,
//                    Caption = (string)dt.ExtendedProperties["title"],
//                    Level = dt_level
//                };
//            }

//            rcbTableLevels.Items.AddRange(_tableLevels);

//            SetTableLevel(_top_table_name);
//        }
//        void FillTableViewModes()
//        {
//            if (_source == null) return;

//            if (_view_mode == TableViewMode.None)
//            {
//                string mode = _xroot.Element("scheme").AttrOrDef(TextConst.AName.ViewMode, TextConst.AVViewModes.Default);
//                if(mode != "") Enum.TryParse(mode, true, out _view_mode);
//                else _view_mode = TableViewMode.Default;
//            }

//            var list = new List<VGCcbItem>();
//            list.Add(new VGCcbItem() { ID = (int)TableViewMode.Default, Name = "Обычный" });
//            list.Add(new VGCcbItem() { ID = (int)TableViewMode.Pivot, Name = "Сводная таблица" });
//            //if (!string.IsNullOrEmpty(treeGrid.ParentFieldName))
//            {
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.Tree, Name = "Дерево" });
//            }
//            list.Add(new VGCcbItem() { ID = (int)TableViewMode.Excel, Name = "Excel" });
//            if (XmlReports.IsDeveloperMode())
//            {
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.DashboardDesigner, Name = "Dashboard Designer" });
//                list.Add(new VGCcbItem() { ID = (int)TableViewMode.DashboardViewer, Name = "Dashboard Viewer" });
//            }

//            _viewModes = list.ToArray();

//            rcbTableViewMode.Items.Clear();
//            rcbTableViewMode.Items.AddRange(_viewModes);

//            SetViewMode(_view_mode);
//        }

//        public override string GetTopTableName()
//        {
//            return _top_table_name;
//        }

//        void SelectCells(BandedGridColumn column)
//        {
//            for (int i = 0; i < column.View.RowCount; i++)
//                column.View.SelectCell(i, column);
//        }
//        void SelectCells(GridBand band)
//        {
//            foreach (BandedGridColumn column in band.Columns)
//                SelectCells(column);
//        }

//        private void UpdateActions()
//        {
//            popup.ItemLinks.Clear();

//            if (DataSource.Report != null)
//            {

//                var actions = DataSource.Report.RowActions().Where(a => a.Attribute(TextConst.AName.CallType).Value == "popupmenu");
//                foreach (var action in actions)
//                {
//                    var btn = new BarButtonItem(barManager, action.Action().Attribute(TextConst.AName.Title).Value)
//                    {
//                        Tag = action
//                    };
//                    btn.ItemClick += btnActionMenu_ItemClick;
//                    popup.ItemLinks.Add(btn);
//                }
//            }
//        }
//        #endregion
//        #region Обработчики событий
//        private void ucReportGrid_Load(object sender, EventArgs e)
//        {
//            //
//        }

//        public override BarManager GetBarManager()
//        {
//            return barManager;
//        }
//        public PopupMenu GetMenu()
//        {
//            return Menu;
//        }
//        public void SetMenu(PopupMenu menu)
//        {
//            Menu = menu;
//        }
//        protected override void OnDataSourceChanged()
//        {
//            base.OnDataSourceChanged();

//            if (_source == null) return;

//            var tmp = _layout_reloading;
//            _layout_reloading = true;

//            FillTableLevels();
//            FillTableViewModes();

//            if (grid != null)
//            {
//                var views = grid.Grid.ViewCollection.Cast<BandedGridView>();
//                // Формируем сложные заголовки колонок, типа "Месяц [a.month]"
//                //if (ViewMode == TableViewMode.Default)
//                {
//                    //GridDesigner.SetComplexColumnsCaptions(_source, views);
//                }

//                foreach (var view in views)
//                {
//                    // Чтобы формулы корректно посчитались
//                    view.UpdateSummary();
//                    view.ExpandAllGroups();
//                }
//            }
            

//            UpdateActions(); // rowactions - устаревший вариант, новый по аналогии с редактором данных events menu

//            if (ddesigner != null) ddesigner.SetData(DataSource.Tables[0]);
//            if (dviewer != null) dviewer.SetData(DataSource.Tables[0]);

//            _layout_reloading = tmp;
//        }

//        private void view_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            if (_layout_reloading) return;

//            GridView view = (GridView)sender;
//            GridCell[] cells = view.GetSelectedCells();

//            decimal sum = 0;
//            int count = 0;

//            for (int i = 0; i < cells.Length; i++)
//            {
//                object val = view.GetRowCellValue(cells[i].RowHandle, cells[i].Column);
//                if (val == null || val == DBNull.Value) continue;

//                if (cells[i].Column.ColumnType == XmlReports.numberType)
//                {
//                    sum += (decimal)val;
//                }

//                count++;
//            }

//            teTotalSum.EditValue = sum;
//            teTotalCount.EditValue = count;
//        }
//        private void view_MouseDown(object sender, MouseEventArgs e)
//        {
//            if (_layout_reloading) return;

//            var view = (BandedGridView)sender;
//            var hInfo = view.CalcHitInfo(e.Location);
//            if (e.Button == MouseButtons.Left && e.Clicks == 1)
//            {
//                view.BeginSelection();
//                if (hInfo.InColumn)
//                {
//                    if (ModifierKeys != (Keys.Control)) view.ClearSelection();
//                    SelectCells(hInfo.Column);
//                }
//                else if (hInfo.InBandPanel && hInfo.Band != null)
//                {
//                    if (ModifierKeys != (Keys.Control)) view.ClearSelection();
//                    SelectCells(hInfo.Band);
//                }
//                view.EndSelection();
//            }

//            if (hInfo.InRowCell)
//            {
//                if (e.Clicks == 1 && e.Button == MouseButtons.Right && popup.ItemLinks.Count > 0)
//                {
//                    popup.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
//                }
//                else if (e.Clicks == 2 && e.Button == MouseButtons.Left)
//                {
//                    if (DataSource == null || DataSource.Report == null) return;

//                    var row = ((BandedGridView)grid.Grid.FocusedView).GetFocusedDataRow();
                    
//                    if (row == null) return;
//                    var gcol = ((BandedGridView)grid.Grid.FocusedView).FocusedColumn;
//                    VDataColumn col = null;
//                    if (gcol != null)
//                    {
//                       col= (VDataColumn)row.Table.Columns[gcol.FieldName];
//                    }

//                    RaiseUIEvent(((BandedGridView)grid.Grid.FocusedView).Name, TextConst.AVEventName.DoubleClick, row, col);
//                }
//            }
//        }
//        private void view_Layout(object sender, EventArgs e)
//        {
//            GenerateLayoutChanged();
//        }
//        private void view_RowCellClick(object sender, RowCellClickEventArgs e)
//        {
//            if (e.Clicks == 1 && e.Button == MouseButtons.Right)
//            {
//                RaiseCellRightClick( GetMainGridView().GetDataRow(e.RowHandle), e.Column.FieldName);
//            }
//        }
//        private void view_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
//        {
//            var view = sender as BandedGridView;
           
//            //var rnd = new Random();
//            //e.Appearance.BackColor = Color.FromArgb(rnd.Next(255), rnd.Next(255), rnd.Next(255));

//            if (!_compare_mode) return;

//            if (e.Column.FieldName.EndsWith("_row_comp"))
//            {
//                e.Appearance.BackColor = Color.LightGray;
//                return;
//            }

//            if (!e.Column.FieldName.EndsWith("_result"))
//            {
//                // ищем колонку с результатом, если она есть 
//                var result_column = ((BandedGridColumn)e.Column).OwnerBand.Columns.Cast<BandedGridColumn>()
//                    .FirstOrDefault(
//                        col => col.FieldName.EndsWith("_result") && !String.IsNullOrEmpty(col.UnboundExpression));
//                if (result_column != null)
//                {
//                    var value = view.GetRowCellValue(e.RowHandle, result_column.FieldName);
//                    // и результат отрицательный - выделяем зависимые ячейки
//                    if ((!Cmn.IsNullOrDBNull(value)) && !Cmn.DECIMAL_ZERO.Equals(value)) {
//                        e.Appearance.ForeColor = Color.Crimson;
//                    }
//                }
//            }

//            // подкрашиваем только если нет группировок
//            if (view.GroupCount == 0)
//            {
//                bool err_in_col = e.Column.FieldName.EndsWith("_result") && Enumerable.Range(0, view.RowCount - 1).Any(ind => Cmn.DECIMAL_ONE.Equals(view.GetRowCellValue(ind, e.Column)));
//                bool err_in_row = view.Columns.Any(col => col.FieldName.EndsWith("_result") && Cmn.DECIMAL_ONE.Equals(view.GetRowCellValue(e.RowHandle, col.FieldName)));
//                if (err_in_col && err_in_row && Cmn.DECIMAL_ONE.Equals(view.GetRowCellValue(e.RowHandle, e.Column.FieldName)))
//                {
//                    e.Appearance.BackColor = Color.Crimson;
//                }
//                else if (err_in_col)
//                {
//                    e.Appearance.BackColor = Color.FromArgb(200, 200, 255);
//                }
//                else if (err_in_row)
//                {
//                    e.Appearance.BackColor = Color.FromArgb(255, 200, 200);
//                }
//            }
//        }
//        private void view_RowCellStyle(object sender, RowCellStyleEventArgs e)
//        {
//            var view = sender as BandedGridView;
//            var row = view.GetDataRow(e.RowHandle);
//            if (row != null)
//            {
//                var vcol = row.Table.Columns[e.Column.FieldName] as VDataColumn;
//                if (vcol != null)
//                {
//                    var scolor = vcol.GetBackColor(row);
//                    if (scolor != null)
//                    {
//                        var items = scolor.Split(',');
//                        int red = int.Parse(items[0]);
//                        int green = int.Parse(items[1]);
//                        int blue = int.Parse(items[2]);
//                        if (view.IsCellSelected(e.RowHandle, e.Column))
//                        {
//                            var focused_color = CommonSkins.GetSkin(LookAndFeel)[CommonSkins.SkinSelection].Color.BackColor;
//                            e.Appearance.BackColor = focused_color.MixColors(Color.FromArgb(red, green, blue), 0.5F);
//                        }
//                        else
//                        {
//                            e.Appearance.BackColor = Color.FromArgb(red, green, blue);
//                        }
//                    }
//                }
//            }
//        }

//        private void view_CustomSummaryCalculate(object sender, CustomSummaryEventArgs args)
//        {
//            if (args.SummaryProcess == CustomSummaryProcess.Start)
//            {
//                args.TotalValueReady = true;
//                return;
//            }

//            var current_view = sender as GridView;

//            // формируем из всех summary пары "имя_колонки,значение"
//            var summaries_info = args.IsTotalSummary
//                ? current_view.Columns.Where(col => col.SummaryItem != null).Select(col => new { col.FieldName, col.SummaryItem.SummaryValue })
//                // Для группировок
//                : current_view.GroupSummary.Select(si => new { si.FieldName, SummaryValue = args.GetGroupSummary(args.GroupRowHandle, si) });

//            // Создаем DataTable для расчёта по формуле
//            var dt = new DataTable();
//            // Добавляем колонки для всех summary
//            dt.Columns.AddRange(summaries_info.Select(si => new DataColumn()
//            {
//                ColumnName = si.FieldName,
//                DataType = XmlReports.numberType
//            }).ToArray());

//            // Добавляем в таблицу значения всех summary
//            dt.Rows.Add(summaries_info.Select(si => si.SummaryValue).ToArray());

//            var column = current_view.Columns[((GridSummaryItem)args.Item).FieldName];
//            var expression = ((Dictionary<string, string>)column.Tag)["agg"];
//            dt.Columns.Add(new DataColumn("expression_result")
//            {
//                Expression = expression,
//                DataType = XmlReports.numberType
//            });

//            args.TotalValue = dt.Rows[0]["expression_result"];
//        }

//        private void pivotGrid_CellSelectionChanged(object sender, EventArgs e)
//        {
//            if (_layout_reloading) return;

//            var pivot_grid = (PivotGridControl)sender;

//            decimal sum = 0;
//            int count = 0;

//            for (int i = 0; i < pivot_grid.Cells.RowCount; i++)
//            {
//                for (int j = 0; j < pivot_grid.Cells.ColumnCount; j++)
//                {
//                    var cell = pivot_grid.Cells.GetCellInfo(j, i);
//                    if (!cell.Selected) continue;

//                    if (cell.Value is Decimal)
//                    {
//                        sum += (decimal)cell.Value;
//                    }
//                    count++;
//                }
//            }

//            teTotalSum.EditValue = sum;
//            teTotalCount.EditValue = count;
//        }
//        private void treeGrid_SelectionChanged(object sender, EventArgs e)
//        {
//            if (_layout_reloading) return;

//            var tree = (TreeList)sender;
//            var nodes = tree.Selection.Cast<TreeListNode>();
//            var columns = tree.VisibleColumns.Cast<TreeListColumn>();

//            teTotalSum.EditValue = nodes.Sum(node => columns.Sum(column =>
//            {
//                return node[column.FieldName] is decimal ? (decimal)node[column.FieldName] : decimal.Zero;
//            }));

//            teTotalCount.EditValue = nodes.Count() * tree.VisibleColumns.Count;
//        }

//        private void cbTableViewMode_EditValueChanged(object sender, EventArgs e)
//        {
//            ViewModeChanged((TableViewMode)((VGCcbItem)cbTableViewMode.EditValue).ID);

//            if (IsTemplate && !_from_file) SetLayoutChanged();
//        }

//        private void cbTableLevels_EditValueChanged(object sender, EventArgs e)
//        {
//            TableLevelChanged(((VGCcbItem)cbTableLevels.EditValue).Name);

//            if (IsTemplate && !_from_file) SetLayoutChanged();
//        }
//        private void rcbTableLevels_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
//        {
//            e.DisplayText = e.DisplayText.Trim();
//        }

//        private void grid_Paint(object sender, PaintEventArgs e)
//        {
//            int max_width = 200;
//            int max_height = 80;

//            _layout_reloading = true;
//            var view = (sender as GridControl).FocusedView as BandedGridView;
//            if (view == null) return;

//            //foreach (GridColumn column in view.Columns)
//            //{
//            //    column.AppearanceHeader.Font = new Font("Times New Roman", 12, FontStyle.Regular);
//            //}

//            StringFormat stringFormat = new StringFormat() { Trimming = StringTrimming.None };

//            int nHeightHeader = 13;
//            int nSubtrahend = 0;
//            int nWidth = 0;
//            int nHeight = 0;
//            foreach (GridColumn col in view.Columns)
//            {
//                if (!col.Visible) continue;

//                nWidth = 0;
//                nSubtrahend = 23;
//                nWidth = col.Width > nSubtrahend ? col.Width - nSubtrahend : col.Width;
//                if (nWidth > max_width) nWidth = max_width;

//                nHeight = (int)col.AppearanceHeader.CalcTextSize(e.Graphics, stringFormat, col.Caption, nWidth).Height;
//                if (nHeight > max_width) nHeight = max_height;

//                nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//            }
//            view.ColumnPanelRowHeight = nHeightHeader + 10;

//            nHeightHeader = 13;
//            nSubtrahend = 0;
//            nWidth = 0;
//            nHeight = 0;
//            foreach (GridBand col in view.Bands)
//            {
//                if (!col.Visible) continue;

//                nWidth = 0;
//                nSubtrahend = 23;
//                nWidth = col.Width > nSubtrahend ? col.Width - nSubtrahend : col.Width;
//                if (nWidth > max_width) nWidth = max_width;

//                nHeight = (int)col.AppearanceHeader.CalcTextSize(e.Graphics, stringFormat, col.Caption, nWidth).Height;
//                if (nHeight > max_width) nHeight = max_height;

//                nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//            }
//            view.BandPanelRowHeight = nHeightHeader + 10;
//            _layout_reloading = false;
//        }
//        private void tree_Paint(object sender, PaintEventArgs e)
//        {
//            int max_width = 200;
//            int max_height = 80;

//            _layout_reloading = true;
//            var tree = (sender as TreeList);
//            if (tree == null) return;

//            var stringFormat = new StringFormat() { Trimming = StringTrimming.None };

//            int nHeightHeader = 13;
//            int nSubtrahend = 0;
//            int nWidth = 0;
//            int nHeight = 0;
//            foreach (TreeListColumn col in tree.Columns)
//            {
//                if (!col.Visible) continue;

//                nWidth = 0;
//                nSubtrahend = 23;
//                nWidth = col.Width > nSubtrahend ? col.Width - nSubtrahend : col.Width;
//                if (nWidth > max_width) nWidth = max_width;

//                nHeight = (int)col.AppearanceHeader.CalcTextSize(e.Graphics, stringFormat, col.Caption, nWidth).Height;
//                if (nHeight > max_width) nHeight = max_height;

//                nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//            }
//            tree.ColumnPanelRowHeight = nHeightHeader + 10;
//            _layout_reloading = false;
//        }

//        private void excelGrid_HyperlinkClick(object sender, DevExpress.XtraSpreadsheet.HyperlinkClickEventArgs e)
//        {
//            e.Handled = true;

//            var ss = e.TargetUri.Replace("http://", "").Split('[');
//            var ss1 = ss[0].Split('.');
//            var tableName = ss1[0];
//            var columnName = ss1[1];
//            var keyValue = ss[1].Replace("]", "").Replace("/", "");

//            var table = (VDataTable)DataSource.Tables[tableName];
//            var column = table.GetColumn(columnName);
//            var row = table.Rows.Find(keyValue);
//            RaiseUIEvent(tableName, TextConst.AVEventName.DoubleClick, row, column);
//        }
        
//        private void btnActionMenu_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            if (DataSource == null || DataSource.Report == null) return;
//            var row = ((BandedGridView)grid.Grid.FocusedView).GetFocusedDataRow();
//            if (row == null) return;

//            ((VUseAction)e.Item.Tag).Execute((VDataSet)row.Table.DataSet, null, (row.Table as VDataTable), row, null);
//        }
//        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucReportGridOld>();
//            if (ctrl.grid != null) (ctrl.grid.Grid.DataSource as VDataTable).Refresh();
//        }
//        #endregion


//        public void SetColumnVisibility(string columnName, bool value)
//        {
           
//        }
//    }
//}
