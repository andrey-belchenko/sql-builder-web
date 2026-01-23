//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Diagnostics.Contracts;
//using System.Drawing;
//using System.Drawing.Drawing2D;
//using System.IO;
//using System.Linq;
//using System.Reflection;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using Devart.Data.Oracle;
//using DevExpress.Skins;
//using DevExpress.Utils;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Controls;
//using DevExpress.XtraBars.Utils;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Repository;

//using DevExpress.XtraTreeList;
//using DevExpress.XtraTreeList.Columns;
//using DevExpress.XtraTreeList.Nodes;
//using DevExpress.XtraTreeList.ViewInfo;
//using infoenergo.core.Extensions;
//using sql.builder.DataApi;
//using sql.builder.DataApi.DataObjects;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    // этот класс не используется
//    internal partial class ucTreeNew : XtraUserControl
//    {
//        private ControlMode _mode;
//        private string _top_table_name;
//        private XElement _xscheme;
//        private Dictionary<string, TreeViewInfo> _views;
//        private ucTableViewerContainer MainControl = null;

//        public void SetFocusedCell(string columnName, DataRow row)
//        {
           
//            GetMainView().FocusedColumn = GetMainView().Columns[columnName];
//            GetMainView().CloseEditor();
//            GetMainView().ShowEditor();
//        }

//        #region DataSource
//        private VDataSet _source;

//        public void SetDataSource(VDataSet source)
//        {
//            // чтобы небыло утечек
//            if (_source != null)
//            {
//                DetachDataSourceEvents();
//                if (_top_table_name != null) DetachTopTableEvents();
//            }

//            _source = source;

//            if (_source != null)
//            {
//                AttachDataSourceEvents();
//                if (_top_table_name != null)
//                {
//                    UpdateTree();
//                    UpdateButtonStates();
//                    AttachTopTableEvents();
//                }
//                //GridDesigner.SetComplexColumnsCaptions(source, views);
//            }

//            tree.ViewInfo.SummaryFooterInfo.NeedsRecalcAll = true;
//            tree.ViewInfo.CalcSummaryFooterInfo();
//            tree.InvalidateSummaryFooterPanel();

//            UpdateActions(); // rowactions - устаревший вариант, новый по аналогии с редактором данных events menu
//            setFetchAll();
//        }
//        private void UpdateButtonStates()
//        {
//            var table = GetTopTable();

//            // если родительская строка добавлена, но не сохранена либо вообще отсутствует - дочерние редактировать нельзя
//            bool no_parent = table.ParentRelations.Cast<DataRelation>()
//                .Select(rel => (rel.ParentTable as VDataTable).CurrentRow)
//                .Any(r => r == null || r.RowState == DataRowState.Added);

//            var bc = ((IDockableObject)barTopToolbar).BarControl;
//            if (bc == null) return;

//            if (no_parent)
//            {
//                ((DockedBarControl)bc).Enabled = false;
//            }
//            else
//            {
//                ((DockedBarControl)bc).Enabled = true;
//                ButtonCommit.Enabled = table.HasUserChanges;
//            }
//        }
//        public VDataSet GetDataSource()
//        {
//            return _source;
//        }
//        public VDataTable GetTopTable()
//        {
//            if (_source == null) return null;

//            return (VDataTable)_source.Tables[_top_table_name];
//        }

//        private void AttachDataSourceEvents()
//        {
//            _source.Changed += OnSourceOnChanged;
//            _source.NeedSelection += OnSourceOnNeedSelection;
//        }

//        private void DetachDataSourceEvents()
//        {
//            _source.Changed -= OnSourceOnChanged;
//            _source.NeedSelection -= OnSourceOnNeedSelection;
//        }

//        private void AttachTopTableEvents()
//        {
//            MainControl.UpdateDataTableProperties();
//            VDataTable table = GetTopTable();
//            table.UIEvent += Table_UIEvent;
//            table.UserChangedData += Table_OnUserChangedData;
//            table.TableRefreshed += Table_TableRefreshed;
//            table.TableCommited += Table_TableCommited;
//            table.DataSourceSelectionChanged += table_DataSourceSelectionChanged;


//        }

//        void table_DataSourceSelectionChanged()
//        {
//            MainControl.SetSelectionFromSource();
//        }

//        private void DetachTopTableEvents()
//        {
//            VDataTable table = GetTopTable();
//            table.UIEvent -= Table_UIEvent;
//            table.UserChangedData -= Table_OnUserChangedData;
//            table.TableRefreshed -= Table_TableRefreshed;
//            table.TableCommited -= Table_TableCommited;
//            table.DataSourceSelectionChanged -= table_DataSourceSelectionChanged;
//        }

//        private void Table_OnUserChangedData(object sender, DataColumnChangeEventArgs args)
//        {
//            UpdateButtonStates();
//        }
//        private bool Table_UIEvent(object sender, UIEventArgs e)
//        {
//            return RaiseUIEvent2("", e.EventName, e.Row, e.Column);
//        }

//        void Table_TableRefreshed(object sender, EventArgs args)
//        {
//            UpdateButtonStates();
//            if (_expand_all_nodes)
//            {
//                tree.ForceInitialize();
//                tree.ExpandAll();
//            }

//            // пляски с бубном - если изначально присвоить пустой DataTable и так не сделать, сортировка не работает...
//            if (_order_field_name != null)
//            {
//                tree.BeginSort();
//                var col = tree.Columns[_order_field_name];
//                col.SortOrder = SortOrder.None;
//                col.SortOrder = SortOrder.Ascending;
//                tree.EndSort();
//            }
//        }

//        void Table_TableCommited(object sender, EventArgs args)
//        {
//            //
//        }

//        #endregion
//        #region EventTags
//        public event UIEventHandler2 UIEvent2;
//        protected bool RaiseUIEvent2(string tableName, string name, DataRow row, VDataColumn col)
//        {
//            if (UIEvent2 != null)
//            {
//                var args = new UIEventArgs2()
//                {
//                    TableName = tableName,
//                    Name = name,
//                    Table = GetTopTable(),
//                    Column = col,
//                    Row = row
//                };

//                return UIEvent2(this, args);
//            }

//            return false;
//        }
//        #endregion

//        private void UpdateActions()
//        {
//            if (_menu == null)
//            {
//                _menu = new PopupMenu(barManager);
//            }

//            _menu.BeginUpdate();
//            _menu.ItemLinks.Clear();
//            if (_source != null && _source.Report != null)
//            {
//                var actions = _source.Report.RowActions().Where(a => a.Attribute(TextConst.AName.CallType).Value == "popupmenu");
//                foreach (var action in actions)
//                {
//                    var btn = new BarButtonItem(barManager, action.Action().Attribute(TextConst.AName.Title).Value)
//                    {
//                        Tag = action
//                    };
//                    btn.ItemClick += btnActionMenu_ItemClick;
//                    _menu.ItemLinks.Add(btn);
//                }
//            }
//            _menu.EndUpdate();

//            if (XmlReports.IsDeveloperMode()) AddDebugButtons();
//        }
//        private void btnActionMenu_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            if (_source == null || _source.Report == null) return;

//            var node = tree.FocusedNode;
//            if (node == null) return;

//            DataRow row = GetNodeData(node);
//            ((VUseAction)e.Item.Tag).Execute((VDataSet)row.Table.DataSet, null, (row.Table as VDataTable), row, null);
//        }

//        RepositoryManager _repositories;

//        private void OnSourceOnNeedSelection(object sender, EventArgs args)
//        {
//            UpdateDataSourceSelectedRows();
//        }
//        private void OnSourceOnChanged(object sender, EventArgs args)
//        {
//            // аналогично как у грида, но реализуется сложнее
//            // вроде работает

//            if (tree.ActiveEditor == null) return;

//            var args1 = args as DataColumnChangeEventArgs;
//            if (args1 == null) return;

//            if (Cmn.GetNodeRow(tree.FocusedNode) == args1.Row && tree.FocusedColumn.FieldName == args1.Column.ColumnName)
//            {
//                tree.EditingValue = args1.ProposedValue;
//            }
//        }
//        public void UpdateDataSourceSelectedRows()
//        {
//            List<DataRow> list = new List<DataRow>();

//            var nodes = tree.Selection.ToArray();

//            foreach (TreeListNode node in nodes)
//            {
//                DataRow row = GetNodeData(node);
//                list.Add(row);
//            }

//            MainControl.SetDataSourceSelectedRows(list);
//        }
//        public TreeList GetMainView()
//        {
//            return tree;
//        }
//        private bool _toolbar_top_visible = true;
//        public void SetTopToolbarVisible(bool visible)
//        {
//            _toolbar_top_visible = visible;
//            barTopToolbar.Visible = visible;
//        }

//        private bool _toolbar_bottom_visible = true;
//        public void SetBottomToolbarVisible(bool visible)
//        {
//            _toolbar_bottom_visible = visible;
//            barBottomToolbar.Visible = visible;
//        }

//        private bool _footer_visible = true;
//        public void SetFooterVisible(bool visible)
//        {
//            _footer_visible = visible;
//            barFooter.Visible = visible;
//        }

//        private bool _multiselect = true;
//        public void SetMultiselect(bool multiselect)
//        {
//            _multiselect = multiselect;

//            tree.OptionsSelection.MultiSelect = _multiselect;
//        }


//        public void SetMultiselectMode(bool isCell)
//        {
//            if (isCell)
//            {
//                tree.OptionsSelection.MultiSelectMode = TreeListMultiSelectMode.CellSelect;
//            }
//            else
//            {
//                tree.OptionsSelection.MultiSelectMode = TreeListMultiSelectMode.RowSelect;

//            }
//        }

//        public bool _summary_visible = true;
//        public void SetSummaryVisible(bool visible)
//        {
//            _summary_visible = visible;

//            tree.OptionsView.ShowSummaryFooter = _summary_visible;
//        }

//        string _parent_field_name;
//        public void SetParentFieldName(string parent_field_name)
//        {
//            _parent_field_name = parent_field_name;
//            if (_mode != ControlMode.Report) tree.ParentFieldName = parent_field_name;
//        }

//        public void SetExpandAllNodes(bool expand_all_nodes)
//        {
//            _expand_all_nodes = expand_all_nodes;
//            if (expand_all_nodes && _source != null)
//            {
//                tree.ForceInitialize();
//                tree.ExpandAll();
//            }
//        }

//        public ucTreeNew(ControlMode mode)
//        {
//            InitializeComponent();

//            _mode = mode;

//            _repositories = new RepositoryManager(tree);

//            Disposed += OnDisposed;

//            _views = new Dictionary<string, TreeViewInfo>();
//            AttachTreeEvents();

//            if (_mode == ControlMode.Report)
//            {
//                tree.OptionsSelection.MultiSelectMode = TreeListMultiSelectMode.CellSelect;
//            }
//            else if (_mode == ControlMode.Data)
//            {
//                tree.OptionsSelection.MultiSelectMode = TreeListMultiSelectMode.RowSelect;
//                tree.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
//                tree.OptionsNavigation.AutoFocusNewNode = true;
//                tree.OptionsBehavior.Editable = true;
//                tree.OptionsBehavior.ReadOnly = false;
//            }

//            barManager.ForceLinkCreate();
//            ButtonChoiceRow.Links.First().Visible = false;
//        }

//        public void BeginUpdate()
//        {
//            tree.BeginUpdate();
//        }
//        public void EndUpdate()
//        {
//            tree.EndUpdate();
//            UpdateColumnsPanelHeight();
//            UpdateDummyColumnWidth();
//        }
//        public void ExportToXlsx(string fullpath, string caption = null)
//        {
//            tree.BeginUpdate();

//            var table_name = tree.Caption;
//            tree.Caption = caption;
//            tree.OptionsView.ShowCaption = true;

//            TreeListColumn colDummy = tree.Columns.FirstOrDefault(c => c.Name == GridDesigner.GetDummyColumnName());
//            if (colDummy != null) colDummy.Visible = false;

//            tree.ExportToXlsx(fullpath);

//            tree.OptionsView.ShowCaption = false;
//            tree.Caption = table_name;
//            if (colDummy != null) colDummy.Visible = true;

//            tree.EndUpdate();
//        }
//        public void LoadFromXml(XElement xscheme, bool update)
//        {
//            this._xscheme = xscheme;
//            this._views.Clear();
//            foreach (var xTable in xscheme.Descendants(EName.table)) {
//                var ti = new TreeViewInfo(this.tree, xTable);
//                FillTreeFromXml(ti);
//                this._views.Add(xTable.Attribute(AName.@as).Value, ti);
//            }
//            if (update) {
//                UpdateTree();
//            }
//            LoadSettingsFromRegistry();
//        }
//        public void SaveToXml(XElement xscheme)
//        {
//            //Parser.SaveTreeSettingsToXml(xscheme.Parent, views);
//        }


//        private void setFetchAll()
//        {
//            if (_source != null && _top_table_name != null)
//            {
//                (_source.Tables[_top_table_name] as VDataTable).UseDeferredFetch = false;
//            }


//        }

//        public void SetTopTable(string top_table_name)
//        {
//            if (_source != null && _top_table_name != null)
//            {
//                DetachTopTableEvents();
//            }

//            _top_table_name = top_table_name;

//            if (_source != null)
//            {
//                AttachTopTableEvents();

//                UpdateTree();
//                UpdateButtonStates();
//            }
//            setFetchAll();
//        }

//        private void UpdateTree()
//        {
//            tree.BeginUpdate();

//            tree.Bands.Clear();
//            tree.Columns.Clear();

//            var ti = _views[_top_table_name];
//            tree.Bands.AddRange(ti.Bands.ToArray());
//            tree.Columns.AddRange(ti.Columns.ToArray());
//            tree.OptionsView.ShowBandsMode = ti.ShowBands ? DefaultBoolean.True : DefaultBoolean.False;
//            tree.Caption = ti.Title;

//            var table = GetTopTable();

//            // старый вариант из ReportGrid
//            var node_id = _views[_top_table_name].Columns.FirstOrDefault(col => col.Name == "node-id");
//            // вариант для Data
//            if (node_id == null && table.HasPrimaryKey()) node_id = _views[_top_table_name].Columns.FirstOrDefault(col => col.FieldName == table.PrimaryKey[0].ColumnName);
//            if (node_id != null) tree.KeyFieldName = node_id.FieldName;

//            var node_pid = _views[_top_table_name].Columns.FirstOrDefault(col => col.Name == "parent-node-id");
//            if (node_pid == null && _parent_field_name != null) node_pid = _views[_top_table_name].Columns.FirstOrDefault(col => col.FieldName == _parent_field_name);
//            if (node_pid != null) tree.ParentFieldName = node_pid.FieldName;

//            tree.EndUpdate();

//            tree.DataSource = table;

//            if (_expand_all_nodes)
//            {
//                tree.ForceInitialize();
//                tree.ExpandAll();
//            }

//            if (_order_field_name != null) UpdateOrderedMode();
//            UpdateColumnsPanelHeight();
//        }

//        public void SetTitle(string title)
//        {
//            tree.Caption = title;
//        }


//        public void UpdateToolbar(XElement xtoolbar, BarItem[] items, bool bottom = false)
//        {
//            if (xtoolbar == null || items == null /*|| isWeb()*/) return;

//            var toolbar = (bottom)
//                ? barBottomToolbar
//                : barTopToolbar;

//            toolbar.BeginUpdate();

//            if (Cmn.GetAttrValue(xtoolbar, TextConst.AName.ColumnVisible) == TextConst.AVBool.False)
//            {
//                foreach (BarItemLink link in toolbar.ItemLinks)
//                {
//                    link.Item.Visibility = BarItemVisibility.Never;
//                }
//            }

//            // настраиваем дефолтные кнопки
//            foreach (var xcmd in xtoolbar.Elements(TextConst.EName.UICommand).Where(xcmd => xcmd.Attribute(TextConst.AName.ControlName) != null))
//            {
//                var visible = (Cmn.GetAttrValue(xcmd, TextConst.AName.ColumnVisible) == TextConst.AVBool.True);
//                SetToolbarButtonVisible(xcmd.Attribute(TextConst.AName.ControlName).Value, visible);
//            }

//            toolbar.AddItems(items);
//            toolbar.EndUpdate();
//        }
//        public void SetToolbarButtonVisible(string name, bool visible)
//        {

//            var names = new List<string>();

//            if (name == "ButtonAddRow")
//            {
//                names.Add(ButtonAddChild.Name);
//                names.Add(ButtonAddSibling.Name);
//            }
//            else
//            {
//                names.Add(name);
//            }

//            var items = barTopToolbar.ItemLinks.Where(il =>names.Contains(il.Item.Name));
//            foreach (var item in items)
//            {
                

//                item.Item.Visibility = visible ? BarItemVisibility.Always : BarItemVisibility.Never;
//            }
//        }

//        PopupMenu _menu;
//        public void UpdatePopupMenu(XElement xmenu, BarItem[] items)
//        {
//            if (xmenu == null || items == null) return;

//            if (_menu == null)
//            {
//                _menu = new PopupMenu(barManager);
//            }

//            _menu.BeginUpdate();
//            _menu.ClearLinks();
//            _menu.AddItems(items);
//            _menu.EndUpdate();

//            if (XmlReports.IsDeveloperMode()) AddDebugButtons();
//        }


//        public void UpdateDummyColumnWidth()
//        {
//            TreeListColumn colDummy = tree.Columns.ColumnByName(GridDesigner.GetDummyColumnName());
//            if (colDummy == null) return;

//            TreeListViewInfo info = tree.ViewInfo;

//            int width_free = info.ViewRects.IndicatorWidth + info.ViewRects.ColumnPanelWidth - info.ViewRects.ColumnTotalWidth + colDummy.Width - 20;// 20 чтобы не глючило при порявлении скрола
//            int width_min = GridDesigner.GetMinDummyWidth();
//            if (width_free < width_min)
//            {
//                colDummy.Width = width_min;
 
//            }
//            else
//            {
//                colDummy.Width =  width_free;

//            }
//        }
//        private void UpdateColumnsPanelHeight()
//        {
//            GridDesigner.TreeColumnsBestHeight(tree);
//        }
//        #region TreeEvents
//        private void AttachTreeEvents()
//        {
//            tree.GetCustomSummaryValue += tree_GetCustomSummaryValue;
//            tree.CustomNodeCellEdit += tree_CustomNodeCellEdit;
//            tree.BeforeFocusNode += tree_BeforeFocusNode;
//            tree.FocusedNodeChanged += tree_FocusedNodeChanged;
//            tree.ValidatingEditor += tree_ValidatingEditor;
//            tree.SelectionChanged += tree_SelectionChanged;
//            tree.CustomDrawNodeIndicator += tree_CustomDrawNodeIndicator;
//            tree.CustomDrawNodeCell += tree_CustomDrawNodeCell;
//            tree.DoubleClick += tree_DoubleClick;
//            tree.MouseDown += tree_MouseDown;


//            tree.ColumnWidthChanged += tree_ColumnWidthChanged;
//            tree.BandWidthChanged += tree_BandWidthChanged;
//            tree.Resize += tree_Resize;

//            tree.DragDrop += tree_DragDrop;
//            tree.DragLeave += tree_DragLeave;
//            tree.DragOver += tree_DragOver;
//            tree.GiveFeedback += tree_GiveFeedback;
//        }



//        private void tree_SelectionChanged(object sender, EventArgs args)
//        {
//            if (barFooter.Visible)
//            {
//                //var nodes = tree.Selection.ToArray();
//                var cells = tree.GetSelectedCells();
//                //var columns = tree.VisibleColumns.ToArray();

//                //decimal sum = nodes.Sum(node => columns.Sum(column => node[column.FieldName] is decimal ? (decimal)node[column.FieldName] : decimal.Zero));

//                decimal sum = cells.Sum(cell => cell.Node[cell.Column.FieldName] is decimal ? (decimal)cell.Node[cell.Column.FieldName] : decimal.Zero);
//                decimal cnt = cells.Sum(cell => cell.Node[cell.Column.FieldName] == DBNull.Value ? decimal.Zero : decimal.One);

//                teTotalSum.EditValue = sum;
//                teTotalCount.EditValue = cnt;
//            }

//            if (_mode == ControlMode.Select)
//            {
//                GetTopTable().ManualUserChangedData();
//            }
//            MainControl.ProcessSelectionChange();
//        }
//        private void tree_MouseDown(object sender, MouseEventArgs args)
//        {
//            var hi = tree.CalcHitInfo(args.Location);

//            if (_multiselect)
//            {
//                if (args.Clicks == 1)
//                {
//                    if (args.Button == MouseButtons.Left)
//                    {
//                        tree.BeginSelection();

//                        if (hi.HitInfoType == HitInfoType.Column) {
//                            if (Control.ModifierKeys != Keys.Control) {
//                                tree.Selection.Clear();
//                            }
//                            var nodes = tree.GetNodeList().ToArray();
//                            if (nodes.Length != 0) {
//                                tree.SelectCells(nodes[0], hi.Column, nodes[nodes.Length - 1], hi.Column);
//                            }
//                        }
//                        tree.EndSelection();
//                    }
//                    else if (args.Button == MouseButtons.Right)
//                    {
//                        if (_menu == null || _menu.ItemLinks.IsEmpty()) return;

//                        _menu.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
//                    }
//                }
//            }
//        }
//        private void tree_DoubleClick(object sender, EventArgs e)
//        {
//            Point pt = tree.PointToClient(Control.MousePosition);
//            TreeListHitInfo info = tree.CalcHitInfo(pt);

//            if (info.Node != null)
//            {
//                // из ucReferenceGrid
//                RaiseUIEvent2("", TextConst.AVEventName.DoubleClick, null, null);

//                if (tree.FocusedNode == null) return;

//                DataRow row = GetNodeData(tree.FocusedNode);

//                VDataColumn col = null;
//                if (info.Column != null)
//                {
//                    col = (VDataColumn)row.Table.Columns[info.Column.FieldName];
//                }

//                // из ucReportGrid
//                RaiseUIEvent2(_top_table_name, TextConst.AVEventName.DoubleClick, row, col);
//            }
//        }
//        private void tree_CustomDrawNodeCell(object sender, CustomDrawNodeCellEventArgs args)
//        {
//            var row = GetNodeData(args.Node);
//            if (row == null) return;

//            // цвет текста
//            var table = row.Table as VDataTable;

//            // перенес из события RowCellStyle для грида с отчётами
//            var vcol = row.Table.Columns[args.Column.FieldName] as VDataColumn;
//            //if (vcol != null)
//            //{
//            var scolor = table.GetBackColor(row, vcol);
//            //var scolor = vcol.GetBackColor(row);

//            if (scolor != null)
//            {

//                //if (args.Node.Selected)
//                //{

//                    //args.Appearance.BackColor = VColorUtils.GetSelectionColorFromRGBString(scolor, args.Appearance.BackColor);
//                    //args.Appearance.ForeColor = VColorUtils.GetSelectionForeColor(args.Appearance.ForeColor);
//                //}
//                //else
//                //{
//                    args.Appearance.BackColor = VColorUtils.GetColorFromRGBString(scolor);
//                //}
//            }
//            //}

//            if (_mode == ControlMode.Data)
//            {
//                var colName = table.GetNameForText(args.Column.FieldName);
//                if (colName == null) return;

//                var column = table.Columns[colName] as VDataColumn;

//                // кешировать все значения - валидация уже

//                bool drawwarning = false;
//                bool drawedit = false;
//                RepositoryItem rep = null;

//                string rgb = column.GetFontColor(row);
//                Color color;
//                if (VColor.ParseRGB(rgb, out color)) {
//                    args.Appearance.ForeColor = color;
//                }

//                var msg = table.GetCellError(row, colName);

//                if (!string.IsNullOrEmpty(msg))
//                {
//                    drawwarning = true;
//                }

//                if (column.GetEditable(row) || column.HasAdditionalButtons()) // !!! проконтролировать время 
//                {
//                    var row_index = table.Rows.IndexOf(row); // !!! проконтролировать время
//                    rep = _repositories.GetCellRepository(table, colName, row_index /*, true*/); // !!! проконтролировать время
//                    var isNoButtons = false;
//                    if (rep == null)
//                    {
//                        isNoButtons = true;
//                    }
//                    else
//                    {
//                        var repb = rep as RepositoryItemButtonEdit;
//                        if (repb != null)
//                        {
//                            isNoButtons = repb.Buttons.Count == 0;
//                        }
//                    }
//                    if (isNoButtons)
//                    {
//                        drawedit = true;
//                    }
//                }

//                if (drawwarning || drawedit)
//                {
//                    string text = args.CellText;

//                    if (drawwarning)
//                    {
//                        var s = "     ";
//                        if (text != "" && !text.StartsWith(s))
//                        {
//                            text = s + text;
//                        }
//                    }
//                    if (drawedit)
//                    {
//                        if (!(rep is RepositoryItemHyperLinkEdit))
//                        {
//                            var s = "    ";
//                            if (text != "" && !text.EndsWith(s))
//                            {
//                                text = text + s;
//                            }
//                        }
//                    }

//                    args.CellText = text;
//                    args.DefaultDraw();

//                    if (drawwarning)
//                    {
//                        if (rep == null)
//                        {
//                            var row_index = table.Rows.IndexOf(row); // !!! проконтролировать время
//                            rep = _repositories.GetCellRepository(table, colName, row_index /*, true*/); // !!! проконтролировать время
//                        }
//                        var shift = Cmn.GetLeftButtonsSize( colName, rep);
//                        Image im = Cmn.ImageWarning14;
//                        args.Graphics.DrawImage(im, new Point(args.Bounds.Location.X + 1 + shift, args.Bounds.Location.Y + 2));
//                    }
//                    if (drawedit)
//                    {
//                        Image im = Cmn.ImageEdit12;
//                        args.Graphics.DrawImage(im, new Point(args.Bounds.Location.X + args.Bounds.Width - 16, args.Bounds.Location.Y + 1));
//                    }

//                    args.Handled = true;
//                }
//            }
//        }
//        private void tree_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs args)
//        {
//            GetTopTable().CurrentRow = GetNodeData(tree.FocusedNode);
//        }
//        private void tree_BeforeFocusNode(object sender, BeforeFocusNodeEventArgs args)
//        {
//            if (args.Node == args.OldNode) return;
//            if (_mode == ControlMode.Data)
//            {

//                if (!GetTopTable().IsChangeAccepting/*событие почему то срабатывает при акцепте*/ &&  GetTopTable().HasChildrenUserChanges())
//                {
//                    RaiseHasMessage(this, new HasMessageArgs() { Message = "Имеются несохранённые изменения в дочерних таблицах" });
//                    args.CanFocus = false;
//                    return;
//                }
//            }
//        }
//        private void tree_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs args)
//        {
//            if (_mode == ControlMode.Data) args.Valid = true;
//        }
//        private void tree_CustomDrawNodeIndicator(object sender, CustomDrawNodeIndicatorEventArgs args)
//        {
//            if (_mode == ControlMode.Data)
//            {
//                if (args.IsNodeIndicator && args.Node != null)
//                {
//                    var table = GetTopTable();
//                    var row = GetNodeData(args.Node);

//                    if (row == null) return;

//                    if (table.IsRowDeleted(row)) args.ImageIndex = 4;
//                    else if (row.RowState == DataRowState.Added) args.ImageIndex = 2;
//                    else if (row.RowState == DataRowState.Modified) args.ImageIndex = 1;
//                    else args.ImageIndex = -1;

//                    // для выбранных строк красим индикатор в цвет самой строки
//                    if (args.Node.Selected && table.ClientBackColorSource!=null)
//                    {
//                        var color = Cmn.GetHighlightColor();
//                        var backBrush = new SolidBrush(color);
//                        args.DefaultDraw();
//                        var bounds = new Rectangle(args.Bounds.X + 1, args.Bounds.Y + 1, args.Bounds.Width - 2, args.Bounds.Height - 2);
                       
                        
//                        args.Graphics.FillRectangle(backBrush, bounds);

                       
//                        //ControlPaint.DrawBorder3D(args.Graphics, args.Bounds, Border3DStyle.RaisedInner);
                       
//                        if (args.ImageIndex > -1)
//                        {
//                            Image indImage = tree.Painter.IndicatorImages.Images[args.ImageIndex];
//                            int imageLeft = args.Bounds.Left + (args.Bounds.Width - indImage.Width) / 2;
//                            int imageTop = args.Bounds.Top + (args.Bounds.Height - indImage.Height) / 2;
//                            args.Cache.Graphics.DrawImage(indImage, new Point(imageLeft, imageTop));
//                        }

//                        args.Handled = true;
//                    }
//                }
//            }
//        }
//        private void tree_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs args)
//        {
//            if (_mode == ControlMode.Data)
//            {
//                var table = GetTopTable();
//              //  if (table.IsManualDeleteIgnore()) return; // почему то виснет на удалении многократно выполняется этот обработчик
//                var column_name = args.Column.FieldName;
//                column_name = table.GetNameForText(column_name);


//                //var column_name = args.Column.FieldName.Replace(TextConst.Pfx.ExtValName, "");
//                var row = GetNodeData(args.Node);
//                if (row == null) return;
//                var row_index = table.Rows.IndexOf(row);

//                RepositoryItem rep = _repositories.GetCellRepository(table, column_name, row_index);
//                args.RepositoryItem = rep;
//            }
//        }
//        private void tree_GetCustomSummaryValue(object sender, GetCustomSummaryValueEventArgs args)
//        {
//            // не работает
//        }
//        private void tree_ColumnWidthChanged(object sender, ColumnChangedEventArgs args)
//        {
//            if (args.Column.Name == GridDesigner.GetDummyColumnName()) return;

//            UpdateDummyColumnWidth();
//            UpdateColumnsPanelHeight();

//            if (_mode == ControlMode.Data)
//            {
//                SaveSettingsToRegistry();
//            }
//        }
//        private void tree_BandWidthChanged(object sender, BandEventArgs args)
//        {
//            UpdateDummyColumnWidth();
//            UpdateColumnsPanelHeight();

//            if (_mode == ControlMode.Data)
//            {
//                SaveSettingsToRegistry();
//            }
//        }
//        private void tree_Resize(object sender, EventArgs args)
//        {
//            UpdateDummyColumnWidth();
//        }

//        private void tree_GiveFeedback(object sender, GiveFeedbackEventArgs args)
//        {
//            args.UseDefaultCursors = false;
//        }
//        private void tree_DragOver(object sender, DragEventArgs args)
//        {
//            DXDragEventArgs args2 = tree.GetDXDragEventArgs(args);
//            if (args2.Node == null)
//            {
//                if (args2.HitInfo.HitInfoType == HitInfoType.Empty || args2.TargetNode != null)
//                {
//                    args2.Effect = DragDropEffects.Copy;
//                }
//                else
//                {
//                    args2.Effect = DragDropEffects.None;
//                }
//            }

//            if (args2.Effect == DragDropEffects.Move) Cursor = new Cursor(Properties.Resources.move.Handle);
//            else if (args2.Effect == DragDropEffects.Copy) Cursor = new Cursor(Properties.Resources.copy.Handle);
//            else if (args2.Effect == DragDropEffects.None) Cursor = Cursors.No;
//        }
//        private void tree_DragLeave(object sender, EventArgs args)
//        {
//            Cursor = Cursors.Default;
//        }

//        TreeListNode before = null;
//        TreeListNode after = null;
//        TreeListNode parent = null;
//        private void tree_DragDrop(object sender, DragEventArgs args)
//        {
//            DXDragEventArgs args2 = tree.GetDXDragEventArgs(args);

//            if (args2.Node != null)
//            {


//                if (args2.DragInsertPosition == DragInsertPosition.Before)
//                {
//                    before = args2.TargetNode;
//                }
//                else if (args2.DragInsertPosition == DragInsertPosition.After)
//                {
//                    after = args2.TargetNode;

//                }
//                else if (args2.DragInsertPosition == DragInsertPosition.AsChild)
//                {
//                    parent = args2.TargetNode;
//                }

//                tree.SetNodeIndex(args2.Node, tree.GetNodeIndex(args2.TargetNode));
//            }

//            Cursor = Cursors.Default;
//        }
//        private void tree_AfterDropNode(object sender, AfterDropNodeEventArgs args)
//        {
//            if (!args.IsSuccess) return;

//            //tree.BeginUpdate();
//            //PreventTreeRebuild();

//            if (before != null)
//            {
//                args.Node[_order_field_name] = before[_order_field_name];
//                MoveNodesDownInParent(before);

//                before = null;
//            }
//            else if (after != null)
//            {
//                if (after.NextNode != null)
//                {
//                    args.Node[_order_field_name] = after.NextNode[_order_field_name];
//                    MoveNodesDownInParent(after.NextNode);
//                }
//                else
//                {
//                    args.Node[_order_field_name] = (decimal)after[_order_field_name] + 1M;
//                }

//                after = null;
//            }
//            else if (parent != null)
//            {
//                if (parent.HasChildren)
//                {
//                    args.Node[_order_field_name] = (decimal)parent.Nodes.LastNode[_order_field_name] + 1M;
//                }
//                else
//                {
//                    args.Node[_order_field_name] = 1M;
//                }
//                parent = null;
//            }

//            //ResumeTreeRebuild();
//            //tree.EndUpdate();
//        }
//        #endregion

//        public void AcceptChanges()
//        {
//            tree.PostEditor();

//            UpdateDataSourceSelectedRows();
//        }

//        public void AcceptSelection()
//        {
//            UpdateDataSourceSelectedRows();
//            GetTopTable().GetDataSet().ChoiceSource = GetTopTable();
//            //
//            FindForm().Close();
//        }
//        public void DismissChanges(Dictionary<DataRow, OracleException> error_rows = null, bool get_my_errors = false)
//        {
//            if (error_rows != null && error_rows.Count > 0)
//            {
//                if (get_my_errors)
//                {
//                    var table = GetTopTable();
//                    var my_error_rows = new Dictionary<DataRow, OracleException>();
//                    foreach (var r in error_rows.Where(er => er.Key.Table == table)) my_error_rows.Add(r.Key, r.Value);
//                    error_rows = my_error_rows;
//                }

//                bool error_is_shown = false;
//                foreach (var error_row in error_rows)
//                {
//                    //if (error_row.Key.RowState == DataRowState.Deleted)
//                    //{
//                    //    _deleted_error_rows.Add(error_row.Key);
//                    //    error_row.Key.RejectChanges();
//                    //}

//                    if (error_row.Value != null && !error_is_shown)
//                    {
//                        CustomOracleError.HandleIfNeed(error_row.Key, error_row.Value);
//                        error_is_shown = true;
//                    }
//                }

//                //var error_message = Cmn.GetErrorRowsMessage(error_rows);
//                //ShowMessage.Show(ShowMessage.MType.ErrorSaveRowsChanges, error_message);
//            }
//        }
//        public void SetSelection(IEnumerable<object> values)
//        {
//            bool fetched = false;
//            var table = GetTopTable();
//            tree.Selection.Clear();
//            var i = 0;
//            foreach (var value in values)
//            {
//                var row = table.Rows.Find(value);
//                if (row == null && !fetched)
//                {
//                    tree.BeginUpdate();
//                    table.FetchTo(int.MaxValue);
//                    tree.EndUpdate();
//                    fetched = true;

//                    row = table.Rows.Find(value);
//                }

//                if (row != null)
//                {
//                    try // глючит tree, дает ошибки когда все корректно
//                    {
//                        var node = tree.FindNodeByKeyID(row[table.PrimaryKey[0]]);
//                        if (i == 0)
//                        {
//                            tree.SetFocusedNode(node);
//                        }
//                        tree.SelectNode(node);
//                    }
//                    finally
//                    {
//                    }
//                    i++;
//                }
//            }

//            UpdateDataSourceSelectedRows();
//        }

//        private void tooltip_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs args)
//        {
//            if (args.Info != null || args.SelectedControl != tree) return;

//            TreeListHitInfo hitInfo = tree.CalcHitInfo(args.ControlMousePosition);
//            if (hitInfo.Node == null || hitInfo.HitTest.CellInfo == null) return;

//            DataRow row = GetNodeData(hitInfo.Node);
//            if (row == null) return;

//            CellInfo cell = hitInfo.HitTest.CellInfo;

//            var table = (row.Table as VDataTable);
//            var colName = table.GetNameForText(hitInfo.Column.FieldName);
//            var row_index = table.Rows.IndexOf(row);
//            var rep = _repositories.GetCellRepository(table, colName, row_index/*, true*/);// !!! проконтролировать время

//            var shift = Cmn.GetLeftButtonsSize( colName, rep);
//            var l = (hitInfo.MousePoint.X - cell.Bounds.Left);

//            var iswarningToolTip = false;
//            if ((l > shift) && (l < 15 + shift))
//            {
//                var msg = table.GetCellErrorForText(row, hitInfo.Column.FieldName);
//                if (!string.IsNullOrEmpty(msg))
//                {
//                    SuperToolTip toolTip = new SuperToolTip();

//                    ToolTipItem item = new ToolTipItem();
//                    item.Image = Cmn.ImageWarning14;
//                    item.Text = msg;
//                    toolTip.Items.Add(item);
//                    args.Info = new ToolTipControlInfo(0 + hitInfo.Column.FieldName, null);
//                    args.Info.SuperTip = toolTip;
//                    iswarningToolTip = true;
//                }
//            }

//            if (!iswarningToolTip)
//            {
//                if (rep is RepositoryItemHyperLinkEdit)
//                {
//                    SuperToolTip toolTip = new SuperToolTip();

//                    ToolTipItem item = new ToolTipItem();
//                    item.Text = UILink.TooltipText;
//                    toolTip.Items.Add(item);
//                    args.Info = new ToolTipControlInfo(0 + hitInfo.Column.FieldName, null);
//                    args.Info.SuperTip = toolTip;
//                }
//            }
//        }

//        public event HasMessageHandler HasMessage;
//        public void RaiseHasMessage(object sender, HasMessageArgs args)
//        {
//            if (HasMessage != null)
//            {
//                HasMessage(sender, args);
//            }
//        }


//        private void OnDisposed(object sender, EventArgs args)
//        {
//            _menu = null;

//            _repositories.Dispose();
//        }
//        DataRow GetNodeData(TreeListNode node)
//        {
//            DataRowView rowView = tree.GetDataRecordByNode(node) as DataRowView;
//            return (rowView != null) ? rowView.Row : null;
//        }

//        private void ButtonRefresh_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            if (_source == null) return;

//            VDataTable table = GetTopTable();

//            if (table.HasChildrenUserChanges())
//            {
//                RaiseHasMessage(this, new HasMessageArgs() { Message = "Имеются несохранённые изменения в дочерних таблицах" });
//                return;
//            }

//            if (table.HasUserChanges)
//            {
//                var result = ShowMessage.Show(ShowMessage.MType.UnsavedChangesQuestion);
//                if (result == DialogResult.Cancel)
//                {
//                    return;
//                }
//                //else if (result == DialogResult.Yes)
//                //{
//                //    grReference.CommitChanges();
//                //}
//            }
//            GetTopTable().SetForceRefresh();
//            GetTopTable().Refresh();
//            UpdateButtonStates();
//        }
//        private void ButtonAddSibling_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var table = GetTopTable();
//            var row = table.NewRow();

//            tree.BeginUpdate();

//            PrepareNewRow(row, "sibling");

//            table.Rows.Add(row);
           
//            tree.EndUpdate();

//            MainControl.RaiseChangeActionComplete();

//        }

//        private void ButtonAddChild_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            if (tree.FocusedNode == null) return;

//            var table = GetTopTable();
//            var row = table.NewRow();

//            tree.BeginUpdate();

//            PrepareNewRow(row, "child");

//            table.Rows.Add(row);

//            tree.EndUpdate();
//            MainControl.RaiseChangeActionComplete();
//        }
//        private void ButtonDeleteRow_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var nodes = tree.Selection.ToArray();
//            if (nodes.IsEmpty()) return;

//            var table = GetTopTable();

//            tree.BeginUpdate();
//            foreach (TreeListNode node in nodes)
//            {
//                DataRow row = GetNodeData(node);
//                if (row == null) return;

//                if (table.IsRowDeleted(row))
//                {
//                    table.RestoreRow(row);
//                }
//                else
//                {
//                    string invalid_text = table.GetDeleteValidation(row);
//                    if (invalid_text != null)
//                    {
//                        RaiseHasMessage(this, new HasMessageArgs() { Message = "Удаление невозможно. " + invalid_text });
//                    }
//                    else
//                    {
//                        table.DeleteRow(row);
//                    }
//                }
//            }
//            tree.EndUpdate();
//            MainControl.RaiseChangeActionComplete();
//        }
//        private void ButtonCommit_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            tree.CloseEditor();
//            tree.BeginUpdate();
//            tree.EndUpdate();

//            var table = GetTopTable();

//            AcceptChanges();

//            // если строка удаляется - надо сохранить в дочерних иначе глючит
//            if (table.CurrentRow != null && table.IsRowDeleted(table.CurrentRow) && table.HasChildrenUserChanges())
//            {
//                tree.EndUpdate();
//                RaiseHasMessage(this, new HasMessageArgs() { Message = "Сохраните изменения в дочерних таблицах" });
//                return;
//            }

//            if (table.CheckValidation().Error == "")
//            {
//                var result = table.Save();
//                if (!result.Success)
//                {
//                    DismissChanges(result.RowsExceptions);
//                    //_error_rows = result.RowsExceptions.Keys.ToList();

//                    // Пока так - если нет права на запись строки, то в exception пишется null
//                    if (result.RowsExceptions.Any(r => r.Value == null))
//                    {
//                        frmDataError.Show(frmDataError.ErrorType.NoWriteAccess, (table.DataSet as VDataSet).Form.GetSecurityID(), "");
//                    }
//                }
//            }
//            else
//            {
//                DismissChanges();
//            }

//            foreach (var row in table.AsEnumerable())
//            {
//                var msg = table.GetRowErrorText(row).Error;
//                if (msg != "")
//                {
//                    tree.EndUpdate();
//                    RaiseHasMessage(this, new HasMessageArgs() { Message = msg });
//                    return;
//                }
//            }

//            UpdateButtonStates();
//            tree.RefreshDataSource();
//            tree.EndUpdate();
//        }
//        private void ButtonExportExcel_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xlsx");
//            ExportToXlsx(path);
//            Process.Start(path);
//        }

//        #region ViewSettings
//        string _form_name;
//        public void SetFormName(string form_name)
//        {
//            _form_name = form_name;
//        }

//        public XElement GetViewSettings()
//        {
//            XElement xroot = new XElement("root");
//            foreach (var ti in _views)
//            {
//                var xtable = new XElement("table", new XAttribute("name", ti.Key));
//                xroot.Add(xtable);

//                var xcolumns = new XElement("columns");
//                xtable.Add(xcolumns);

//                foreach (TreeListColumn col in ti.Value.Columns)
//                {
//                    var xcolumn = new XElement("column",
//                        new XAttribute("name", col.FieldName),
//                        new XAttribute("width", col.Width),
//                        new XAttribute("visible", col.Visible ? 1 : 0),
//                        new XAttribute("visible_index", col.VisibleIndex),
//                        new XAttribute("sort", col.SortOrder.ToString().ToLower()));

//                    xcolumns.Add(xcolumn);
//                }
//            }

//            return xroot;
//        }
//        public void SetViewSettings(XElement xroot)
//        {
//            // BeginUpdate не блокирует события изменения ширины и вообще глючит
//            tree.BeginInit();

//            foreach (XElement xtable in xroot.Elements("table"))
//            {
//                var ti = _views.FirstOrDefault(v => v.Key == xtable.Attribute("name").Value);
//                if (Equals(ti, default(KeyValuePair<string, TreeViewInfo>))) continue;

//                var names = xtable.Element("columns").Elements("column").Select(c => c.Attribute("name").Value).ToArray();
//                foreach (var c in ti.Value.Columns.Where(c => names.Contains(c.FieldName))) c.Visible = false;
//                foreach (var xcolumn in xtable.Element("columns").Elements("column"))
//                {
//                    var col = ti.Value.Columns.FirstOrDefault(c => c.FieldName == xcolumn.Attribute("name").Value);
//                    if (col == null) continue;

//                    col.Visible = (xcolumn.Attribute("visible").Value == "1");
//                    col.VisibleIndex = int.Parse(xcolumn.Attribute("visible_index").Value);
//                    col.SortOrder = (SortOrder)Enum.Parse(typeof(SortOrder), xcolumn.Attribute("sort").Value, true);
//                    col.Width = int.Parse(xcolumn.Attribute("width").Value);
//                }
//            }

//            tree.EndInit();
//        }
//        public void RestoreViewSettings()
//        {
//            // если не на UIFormC - пока не работает
//            if (_form_name == null) return;

//            string reg_path = String.Format(@"forms\{0}\_grids", _form_name);
//            Cmn.WriteXElementToRegistry(reg_path, "tree", null);

//            ButtonRestoreSettings.Enabled = false;

//            BeginUpdate();
//            LoadFromXml(_xscheme, true);
//            EndUpdate();
//        }

//        public void LoadSettingsFromRegistry()
//        {
//            // если не на UIFormC - пока не работает
//            // в режиме разработки настройки не сохраняем и не загружаем
//            if (_form_name == null || XmlReports.IsDeveloperMode()) return;

//            string reg_path = String.Format(@"forms\{0}\_grids", _form_name);
//            var xsettings = Cmn.ReadXElementFromRegistry(reg_path, "tree");

//            ButtonRestoreSettings.Enabled = (xsettings != null);
//            if (xsettings == null) return;

//            SetViewSettings(xsettings);
//        }
//        public void SaveSettingsToRegistry()
//        {
//            // если не на UIFormC - пока не работает
//            // в режиме разработки настройки не сохраняем и не загружаем
//            if (_form_name == null || XmlReports.IsDeveloperMode()) return;

//            var xsettings = GetViewSettings();
//            ButtonRestoreSettings.Enabled = (xsettings != null);

//            string reg_path = String.Format(@"forms\{0}\_grids", _form_name);
//            Cmn.WriteXElementToRegistry(reg_path, "tree", xsettings);
//        }

//        private void ButtonSaveSettings_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            SaveSettingsToRegistry();
//        }
//        private void ButtonRestoreSettings_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var result = ShowMessage.ShowQuestion("Вы уверены, что хотите сбросить сохранённые настройки колонок?");
//            if (result == DialogResult.Yes) RestoreViewSettings();
//        }

//        XElement _viewChanges;
//        public void HoldViewChanges()
//        {
//            _viewChanges = GetViewSettings();
//        }
//        public void RestoreViewChanges()
//        {
//            if (_viewChanges == null) return;

//            SetViewSettings(_viewChanges);
//            _viewChanges = null;
//        }
//        #endregion
//        #region OrderedMode
//        private string _order_field_name;
//        public void SetOrderFieldName(string order_field_name)
//        {
//            if (order_field_name == null) return;

//            _order_field_name = order_field_name;

//            ButtonUp.Visibility = BarItemVisibility.Always;
//            ButtonDown.Visibility = BarItemVisibility.Always;

//            if (_source != null) UpdateOrderedMode();
//        }

//        private void UpdateOrderedMode()
//        {
//            foreach (var c in tree.Columns.Where(c => c.Name != GridDesigner.GetDummyColumnName() && c.FieldName != _order_field_name)) c.OptionsColumn.AllowSort = false;

//            TreeListColumn col = tree.Columns[_order_field_name];
//            col.SortOrder = SortOrder.Ascending;
//        }
//        private List<List<TreeListNode>> GetSelectionBlocks()
//        {
//            TreeListNode[] selected_nodes = tree.Selection
//                .OrderBy(n => Cmn.Nvl(n[_parent_field_name], null))
//                .ThenBy(n => n[_order_field_name])
//                .ToArray();

//            var blocks = new List<List<TreeListNode>>();
//            List<TreeListNode> block = null;

//            int cur_index = 0;
//            TreeListNode nodeLast = null;
//            TreeListNode nodeCurrent = null;
//            while (cur_index < selected_nodes.Length)
//            {
//                nodeCurrent = selected_nodes[cur_index];
//                if (nodeLast == null
//                    || nodeLast.ParentNode != nodeCurrent.ParentNode
//                    || nodeLast.NextNode != nodeCurrent)
//                {
//                    block = new List<TreeListNode>();
//                    blocks.Add(block);
//                }

//                block.Add(nodeCurrent);
//                nodeLast = nodeCurrent;
//                cur_index++;
//            }

//            return blocks;
//        }
//        private void MoveBlocksUp(List<List<TreeListNode>> blocks)
//        {
//            for (int index = 0; index < blocks.Count; index++) {
//                List<TreeListNode> nodes = blocks[index];
//                Contract.Assume(nodes.Count > 0);
//                TreeListNode nodeToMove = nodes[0].PrevNode;
//                if (nodeToMove != null) {
//                    object orderLast = null;
//                    for (int node_index = nodes.Count - 1; node_index >= 0; node_index--) {
//                        TreeListNode node = nodes[node_index];
//                        if (orderLast == null) {
//                            orderLast = node[this._order_field_name];
//                        }
//                        node[this._order_field_name] = node.PrevNode[this._order_field_name];
//                    }
//                    nodeToMove[this._order_field_name] = orderLast;
//                }
//            }
//        }
//        private void MoveBlocksDown(List<List<TreeListNode>> blocks)
//        {
//            for (int index = 0; index < blocks.Count; index++) {
//                List<TreeListNode> nodes = blocks[index];
//                Contract.Assume(nodes.Count > 0);
//                TreeListNode nodeToMove = nodes[nodes.Count - 1].NextNode;
//                if (nodeToMove != null) {
//                    object orderFirst = null;
//                    for (int node_index = 0; node_index < nodes.Count; node_index++) {
//                        TreeListNode node = nodes[node_index];
//                        if (orderFirst == null) {
//                            orderFirst = node[this._order_field_name];
//                        }
//                        node[this._order_field_name] = node.NextNode[this._order_field_name];
//                    }
//                    nodeToMove[this._order_field_name] = orderFirst;
//                }
//            }
//        }
//        private void MoveNodesDownInParent(TreeListNode node_first)
//        {
//            if (node_first == null) return;

//            TreeListNode node = node_first;
//            while (node != null)
//            {
//                if (node.NextNode != null)
//                {
//                    node[_order_field_name] = node.NextNode[_order_field_name];
//                }
//                else
//                {
//                    node[_order_field_name] = (decimal)node[_order_field_name] + 1M;
//                }

//                node = node.NextNode;
//            }
//        }

//        /// <summary>
//        /// Приходится использовать вместе с BeginSort(), чтобы дерево не перестраивалось в процессе изменения поля order
//        /// При определенных условиях без этих блокировок можно получить дублированое значение order у соседних узлов
//        /// https://www.devexpress.com/Support/Center/Question/Details/Q240528
//        /// </summary>
//        System.Reflection.FieldInfo _lockSortField;
//        void PreventTreeRebuild()
//        {
//            if (_lockSortField == null)
//            {
//                _lockSortField = typeof(TreeList).GetField("lockSort", BindingFlags.Instance | BindingFlags.NonPublic);
//            }

//            int lockSort = (int)_lockSortField.GetValue(tree);
//            _lockSortField.SetValue(tree, ++lockSort);
//        }
//        void ResumeTreeRebuild()
//        {
//            int lockSort = (int)_lockSortField.GetValue(tree);
//            _lockSortField.SetValue(tree, --lockSort);
//        }

//        private void ButtonUp_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            if (tree.Selection.Count == 0) return;

//            tree.BeginUpdate();
//            tree.BeginSort();
//            PreventTreeRebuild();

//            // разделяем выделенные строки на блоки последовательно идущих строк, которые будут двигаться вместе
//            List<List<TreeListNode>> blocks = GetSelectionBlocks();
//            // двигаем блоки
//            MoveBlocksUp(blocks);

//            ResumeTreeRebuild();
//            tree.EndSort();
//            tree.EndUpdate();

//            //var col = tree.Columns[_order_field_name];
//            //col.SortOrder = SortOrder.None;
//            //col.SortOrder = SortOrder.Ascending;
//            MainControl.RaiseChangeActionComplete();
//        }
//        private void ButtonDown_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            if (tree.Selection.Count == 0) return;

//            tree.BeginUpdate();
//            tree.BeginSort();
//            PreventTreeRebuild();

//            // разделяем выделенные строки на блоки последовательно идущих строк, которые будут двигаться вместе
//            List<List<TreeListNode>> blocks = GetSelectionBlocks();
//            // двигаем блоки
//            MoveBlocksDown(blocks);

//            ResumeTreeRebuild();
//            tree.EndSort();
//            tree.EndUpdate();
//            MainControl.RaiseChangeActionComplete();
//        }

//        private bool _allow_drag_and_drop;
//        public void SetAllowDragAndDrop(bool allow_drag_and_drop)
//        {
//            _allow_drag_and_drop = allow_drag_and_drop;
//            tree.OptionsDragAndDrop.DragNodesMode = (allow_drag_and_drop) ? DragNodesMode.Single : DragNodesMode.None;
//        }
//        #endregion

//        private bool _expand_all_nodes;
       
//        void PrepareNewRow(DataRow row, string insert_mode)
//        {
//            if (insert_mode == "sibling")
//            {
//                if (tree.FocusedNode != null)
//                {
//                    row[tree.ParentFieldName] = tree.FocusedNode[tree.ParentFieldName];
//                    if (_order_field_name != null)
//                    {
//                        if (tree.FocusedNode.NextNode != null)
//                        {
//                            row[_order_field_name] = tree.FocusedNode.NextNode[_order_field_name];
//                            MoveNodesDownInParent(tree.FocusedNode.NextNode);
//                        }
//                        else
//                        {
//                            row[_order_field_name] = (decimal)tree.FocusedNode[_order_field_name] + 1M;
//                        }
//                    }
//                }
//                else
//                {
//                    row[tree.ParentFieldName] = DBNull.Value;
//                    if (_order_field_name != null)
//                    {
//                        row[_order_field_name] = 1M;
//                    }
//                }
//            }
//            else if (insert_mode == "child" && tree.FocusedNode != null)
//            {
//                row[tree.ParentFieldName] = tree.FocusedNode[tree.KeyFieldName];

//                if (_order_field_name != null)
//                {
//                    if (tree.FocusedNode.Nodes.Count > 0)
//                    {
//                        row[_order_field_name] = (decimal)tree.FocusedNode.Nodes.LastNode[_order_field_name] + 1M;
//                    }
//                    else
//                    {
//                        row[_order_field_name] = 1M;
//                    }

//                }
//            }
//        }

//        private void ButtonChoiceRow_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            AcceptSelection();
//        }

//        void AddDebugButtons()
//        {
//            if (_top_table_name == null) return;

//            _menu.AddItem(new BarHeaderItem() { Caption = _top_table_name });
//            var btn = new BarButtonItem(barManager, "Сохранить настройки колонок");
//            btn.ItemClick += ButtonSaveDefaultColumnsSettingsClick;
//            _menu.AddItem(btn);
//            btn = new BarButtonItem(barManager, "Сбросить настройки колонок");
//            btn.ItemClick += ButtonClearDefaultColumnsSettingsClick;
//            _menu.AddItem(btn);
//        }

//        private void ButtonSaveDefaultColumnsSettingsClick(object sender, ItemClickEventArgs args)
//        {
//            VForm vform = XmlReports.Environment.GetForm(_form_name);
//            if (vform == null) return;

//            var vitems = VSXElement.GetDescedantsP(vform.GetDescedantsP(EName.grid)
//                .First(g => g.P_Table == _top_table_name)
//                .GetElementsP(EName.columns)
//                .First());

//            foreach (TreeListColumn column in GetMainView().VisibleColumns)
//            {
//                string colName = column.FieldName.Replace(TextConst.Pfx.ExtValName, "");
//                var vitem = vitems.FirstOrDefault(vc => vc.P_Alias == colName)
//                         ?? vitems.FirstOrDefault(vc => vc.P_Column == colName);

//                if (vitem == null) continue;

//                if (column.Width != GridDesigner.GetDefaultGridColumnWidth())
//                {
//                    vitem.P_ColumnWidth = column.Width.ToString();
//                }
//                else
//                {
//                    vitem.P_ColumnWidth = "";
//                }
//            }

//            XmlReports.UpdateElementInCompiledScheme(vform);

//            vform.SourceFileName = ucQueryEditor.AddPathToFilename(Cmn.GetAttrValue(vform, "file"));
//            vform.ParentName = TextConst.EName.Forms;
//            vform.SaveInSourceFile();
//        }

//        private void ButtonClearDefaultColumnsSettingsClick(object sender, ItemClickEventArgs args)
//        {
//            VForm vform = XmlReports.Environment.GetForm(_form_name);
//            if (vform == null) return;

//            var vitems = VSXElement.GetDescedantsP(vform.GetDescedantsP(EName.grid)
//                .First(g => g.P_Table == _top_table_name)
//                .GetElementsP(EName.columns)
//                .First());

//            foreach (var vc in vitems)
//            {
//                vc.P_ColumnWidth = "";
//            }

//            XmlReports.UpdateElementInCompiledScheme(vform);

//            vform.SourceFileName = ucQueryEditor.AddPathToFilename(Cmn.GetAttrValue(vform, "file"));
//            vform.ParentName = TextConst.EName.Forms;
//            vform.SaveInSourceFile();
//        }
//        #region Parser
//        private static void FillTreeFromXml(TreeViewInfo ti)
//        {
//            if (ti.XTable.Attribute("title") != null) {
//                ti.Title = ti.XTable.Attribute("title").Value;
//            } else {
//                ti.Title = ti.XTable.Attribute("name").Value;
//            }

//            TreeListBand empty_band = null;
//            foreach (XElement xElement in ti.XTable.Element("viewcolumns").Elements()) {
//                if (xElement.Name.LocalName == "column") {
//                    if (empty_band == null) {
//                        empty_band = new TreeListBand();
//                        ti.Bands.Add(empty_band);
//                    }

//                    var column = new TreeListColumn();
//                    empty_band.Columns.Add(column);
//                    ti.Columns.Add(column);

//                    FillTreeColumnFromXml(column, ti, xElement);
//                } else if (xElement.Name.LocalName == "band") {
//                    empty_band = null;
//                    ti.ShowBands = true;

//                    var band = new TreeListBand();
//                    ti.Bands.Add(band);

//                    FillTreeBandFromXml(band, ti, xElement);
//                }
//            }

//            var colLast = ti.Columns.LastOrDefault();
//            if (colLast != null) {
//                var colDummy = GridDesigner.CreateDummyTreeColumn();
//                colLast.ParentBand.Columns.Add(colDummy);
//                ti.Columns.Add(colDummy);
//            }

//            // прячем бэнды без видимых элементов
//            foreach (TreeListBand band in ti.Bands) {
//                if (String.IsNullOrEmpty(band.Caption)) {
//                    if (band.Columns.VisibleCount == 0 && band.Bands.VisibleCount == 0) {
//                        band.Visible = false;
//                    }
//                }
//            }

//            // отдельно заполняем группировки и сортировки
//            FillGroupsAndSortingFromXml(ti.Columns, ti.XTable.Element("viewcolumns"));
//        }
//        private static void FillTreeBandFromXml(TreeListBand band, TreeViewInfo ti, XElement xBand)
//        {
//            band.Caption = xBand.Attribute("title").Value;

//            band.AppearanceHeader.Options.UseTextOptions = true;
//            band.AppearanceHeader.TextOptions.HAlignment = HorzAlignment.Center;
//            band.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
//            if (xBand.Attribute("width") != null) {
//                band.Width = Convert.ToInt32(xBand.Attribute("width").Value);
//            }

//            bool is_parent = xBand.Elements("band").Any();

//            TreeListBand empty_band = null;
//            foreach (XElement xElement in xBand.Elements()) {
//                if (xElement.Name.LocalName == "column") {
//                    if (is_parent && empty_band == null) {
//                        empty_band = new TreeListBand();
//                        band.Bands.Add(empty_band);
//                    }

//                    var column = new TreeListColumn();
//                    ti.Columns.Add(column);

//                    FillTreeColumnFromXml(column, ti, xElement);

//                    if (!is_parent) band.Columns.Add(column);
//                    else empty_band.Columns.Add(column);
//                } else if (xElement.Name.LocalName == "band") {
//                    empty_band = null;

//                    var child_band = new TreeListBand();
//                    band.Bands.Add(child_band);
//                    //ti.Tree.Bands.Add(child_band);
//                    FillTreeBandFromXml(child_band, ti, xElement);
//                }

//                band.Visible = (band.Columns.VisibleCount > 0 || band.Bands.VisibleCount > 0);
//            }
//        }
//        private static void FillTreeColumnFromXml(TreeListColumn column, TreeViewInfo ti, XElement xViewColumn)
//        {
//            column.FieldName = xViewColumn.Attribute("name").Value;

//            var ext_options = (Dictionary<string, string>)(column.Tag ?? (column.Tag = new Dictionary<string, string>()));

//            //ext_options.Add("table", xViewColumn.Parent.Parent.Attribute("name").Value);

//            if (xViewColumn.Attribute("width") != null) {
//                column.Width = Convert.ToInt32(xViewColumn.Attribute("width").Value);
//            }

//            var format = xViewColumn.AttrOrDef(TextConst.AName.Format, null);
//            switch (Cmn.GetAttrValue(xViewColumn.Attribute("type"))) {
//                case "number":
//                    column.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.Decimal;
//                    column.Format.FormatType = FormatType.Numeric;

//                    column.AllNodesSummary = true;
//                    column.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
//                    if (xViewColumn.Attribute(TextConst.AName.Format) != null) {
//                        format = format ?? "n2";
//                        column.SummaryFooterStrFormat = (format.StartsWith("{0:") ? format : string.Format("{{0:{0}}}", format));
//                    }

//                    break;
//                case "date":
//                    column.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.DateTime;
//                    column.Format.FormatType = FormatType.DateTime;
//                    break;
//                default:
//                    column.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.String;
//                    break;
//            }

//            if (Cmn.GetAttrValue(xViewColumn.Attribute("type")) == "number") {
//                if (xViewColumn.Attribute("agg") != null) {
//                    var agg = xViewColumn.Attribute("agg").Value;
//                    // Сохраняем значение признака колонки agg
//                    ext_options.Add("agg", agg);

//                    if (agg != "no") {
//                        DevExpress.XtraTreeList.SummaryItemType summary_type = DevExpress.XtraTreeList.SummaryItemType.None;
//                        switch (agg) {
//                            case "sum": summary_type = DevExpress.XtraTreeList.SummaryItemType.Sum; break;
//                            case "avg": summary_type = DevExpress.XtraTreeList.SummaryItemType.Average; break;
//                            case "count": summary_type = DevExpress.XtraTreeList.SummaryItemType.Count; break;
//                            case "max": summary_type = DevExpress.XtraTreeList.SummaryItemType.Max; break;
//                            case "min": summary_type = DevExpress.XtraTreeList.SummaryItemType.Min; break;
//                            default: if (!Compiler.aggFuncsNames.Contains(agg)) summary_type = DevExpress.XtraTreeList.SummaryItemType.Custom; break;
//                        }
//                        column.SummaryFooter = summary_type;
//                    }

//                } else {
//                    column.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
//                }
//            }

//            if (format != null) {
//                column.Format.FormatString = format;
//            }

//            if (Cmn.GetAttrValue(xViewColumn, TextConst.AName.Title) != "") {
//                column.Caption = xViewColumn.Attribute("title").Value;
//            } else {
//                column.Caption = xViewColumn.Attribute("name").Value;
//            }

//            column.Visible = (xViewColumn.AttrOrDef("visible", "1") != "0");
//            column.OptionsColumn.AllowEdit = true;// (xViewColumn.AttrOrDef("editable", "1") != "1");

//            if (xViewColumn.Attribute(TextConst.AName.FixedSide) != null) {
//                var val = xViewColumn.Attribute(TextConst.AName.FixedSide).Value;
//                ext_options.Add(TextConst.AName.FixedSide, val);
//                // не работает для BandedGridView
//                column.Fixed = (val == TextConst.AVFixedSide.Left) ? DevExpress.XtraTreeList.Columns.FixedStyle.Left :
//                               (val == TextConst.AVFixedSide.Right) ? DevExpress.XtraTreeList.Columns.FixedStyle.Right :
//                               DevExpress.XtraTreeList.Columns.FixedStyle.None;
//            }

//            if (XmlReports.GetXAttributeValue(xViewColumn, "node-id") == "1") {
//                column.Name = "node-id";
//            }

//            if (XmlReports.GetXAttributeValue(xViewColumn, "parent-node-id") == "1") {
//                column.Name = "parent-node-id";
//            }

//            if (xViewColumn.Attribute(TextConst.AName.HAlign) != null) {
//                var val = xViewColumn.Attribute(TextConst.AName.HAlign).Value;

//                column.AppearanceCell.TextOptions.HAlignment = Parser.SHAllignToDxHallign(val);
//            }
//        }
//        private static void FillGroupsAndSortingFromXml(IEnumerable<TreeListColumn> columns, XElement xColumns)
//        {
//            // !!! Сделал так, чтобы коректно обрабатывалась ситуация, когда индексы групп колонок идут не по порядку

//            // формируем массив картежей с информацией о колонках (имя, индекс группы, сортировка),
//            // упорядоченный по возрастанию индекса группы
//            var columns_info = xColumns.Descendants("column")
//                .Select(xcol => new Tuple<string, string>(
//                    xcol.Attribute("name").Value,
//                    xcol.Attribute("sort") != null ? xcol.Attribute("sort").Value : "none"));

//            foreach (var column_info in columns_info) {
//                var column = columns.First(col => col.FieldName == column_info.Item1);

//                switch (column_info.Item2) {
//                    case "ascending": column.SortOrder = System.Windows.Forms.SortOrder.Ascending; break;
//                    case "descending": column.SortOrder = System.Windows.Forms.SortOrder.Descending; break;
//                    default: column.SortOrder = System.Windows.Forms.SortOrder.None; break;
//                }
//            }
//        }
//        #endregion
//    }
//    internal class TreeViewInfo
//    {
//        private TreeList tree;
//        private XElement xtable;
//        private List<TreeListBand> bands;
//        private List<TreeListColumn> сolumns;
//        private string title;
//        private bool show_bands;
//        internal TreeList Tree { get { return this.tree; } }
//        internal XElement XTable { get { return this.xtable; } }
//        internal List<TreeListBand> Bands { get { return this.bands; } }
//        internal List<TreeListColumn> Columns { get { return this.сolumns; } }
//        internal string Title { get { return this.title; } set { this.title = value; } }
//        internal bool ShowBands { get { return this.show_bands; } set { this.show_bands = value; } }
//        internal TreeViewInfo(TreeList tree, XElement xtable)
//        {
//            this.tree = tree;
//            this.xtable = xtable;
//            this.bands = new List<TreeListBand>();
//            this.сolumns = new List<TreeListColumn>();
//            this.title = null;
//            this.show_bands = false;
//        }
//    }
//}