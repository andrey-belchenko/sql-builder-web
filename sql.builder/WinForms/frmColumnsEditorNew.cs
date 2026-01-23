//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.Utils;
////using DevExpress.XtraBars;
////using DevExpress.XtraGrid.Columns;
////using DevExpress.XtraGrid.Views.Grid;
////using DevExpress.XtraGrid.Views.Grid.ViewInfo;

//using infoenergo.core.Extensions;
//using infoenergo.ui.win.Forms;
//using sql.builder.DataApi;

//namespace sql.builder.WinForms
//{
//    internal partial class frmColumnsEditorNew : FormBase
//    {
//        #region Закрытые переменные
//        readonly List<string> titleColsNames = new List<string>();

//        DataTable _dt_all_columns;
//        DataTable _dt_selected_columns;

//        private GridHitInfo _downHitInfo;
//        private bool _supress_sync_selection;
//        private bool _is_fact_use;
//        #endregion
//        #region Открытые методы
//        public frmColumnsEditorNew()
//        {
//            InitializeComponent();
//            rceCheck.ValueUnchecked = DBNull.Value;
           
//        }




//        private SortedList<string, string> _textDecode = null;

//        public void Initialize(XElement xAllColumns, XElement xSelectedColumns,SortedList<string, string> textDecode=null)
//        {
//            _textDecode = textDecode;
//            if (XmlReports.IsDeveloperMode())
//            {
//                btnColGrp.Visibility = BarItemVisibility.Always;
//            }
//            GenerateColumnsData(xAllColumns, xSelectedColumns);
//            (grAll.MainView as GridView).ShowFindPanel();
//        }

//        public XElement GetColumnsXml()
//        {
//            var orderedTitColsNames = new List<string>();

//            foreach (var col in viewSelected.VisibleColumns.OrderBy(c => c.VisibleIndex))
//            {
//                orderedTitColsNames.Add(col.FieldName);
//            }

//            var xViewColumns1 = new XElement(TextConst.EName.ViewColumns, Cmn.DataTableToXElements2(_dt_selected_columns));
//            var xViewColumns = new XElement(TextConst.EName.ViewColumns);

//            var bandsByLevel = new Dictionary<int, XElement>();

//            for (int i = 0; i < orderedTitColsNames.Count; i++)
//            {
//                bandsByLevel.Add(i, null);
//            }

//            XElement parent = null;
//            foreach (XElement col in xViewColumns1.Elements())
//            {
//                int level = -1;
//                for (int i = 0; i < orderedTitColsNames.Count; i++)
//                {
//                    var titName = orderedTitColsNames[i];
//                    var colName = Cmn.GetAttrValue(col, titName);
//                    bool next = false;
//                    if (bandsByLevel[i] != null)
//                    {
//                        if (Cmn.GetAttrValue(bandsByLevel[i], TextConst.AName.Title) == colName)
//                        {
//                            level++;
//                            next = true;
//                        }
//                    }
//                    if (!next)
//                    {
//                        break;
//                    }
//                }


//                if (level == -1)
//                {
//                    parent = xViewColumns;
//                }
//                else
//                {
//                    parent = bandsByLevel[level];
//                }


//                for (int i = level + 1; i < orderedTitColsNames.Count; i++)
//                {
//                    var newParent = new XElement(TextConst.EName.Band);
//                    var bandTitle = Cmn.GetAttrValue(col, orderedTitColsNames[i]);
//                    if (bandTitle != "")
//                    {
//                        if (bandTitle == Cmn.GetAttrValue(col, "dim-title"))
//                        {
//                            newParent.SetAttributeValue("dimension", Cmn.GetAttrValue(col, "dim-name"));
//                            col.Attributes("dimension").Remove();
//                        }
//                        newParent.SetAttributeValue(TextConst.AName.Title, bandTitle);
//                        if (parent != null)
//                        {
//                            parent.Add(newParent);
//                        }
//                        else
//                        {
//                            xViewColumns.Add(newParent);
//                        }
//                        parent = newParent;
//                    }
//                    bandsByLevel[i] = parent;
//                    level++;
//                }

//                col.SetAttributeValue(TextConst.AName.Title, parent.Attribute(TextConst.AName.Title).Value);
//                if (parent.Attribute("dimension") != null)
//                {
//                    col.SetAttributeValue("dimension", parent.Attribute("dimension").Value);
//                }
//                parent.ReplaceWith(new XElement(col));
//            }

//            xViewColumns.Descendants().Attributes().Where(a => orderedTitColsNames.Contains(a.Name.LocalName)).Remove();
//            return xViewColumns;
//        }
//        #endregion
//        #region Закрытые методы
//        private void GenerateColumnsData(XElement xAllColumns, XElement xSelectedColumns)
//        {
//            var cols_all = GetProcessedColumns(xAllColumns);
//            _dt_all_columns = CreateTable(cols_all);
//            var cols_selected = GetProcessedColumns(xSelectedColumns);
//            _dt_selected_columns = CreateTable(cols_selected, _dt_all_columns);
//            // сразу отмечаем выбранные
//            foreach (DataRow row_selected in _dt_selected_columns.Rows) {
//                DataRow row = _dt_all_columns.Rows.Find(row_selected["name"]);
//                if (row != null) {
//                    row["check"] = true;
//                    if (!allowChangeOrder) {
//                        row_selected["ord"] = row["ord"];
//                    }
//                }
//            }
//            UpdateGridColumns();
//            grAll.DataSource = _dt_all_columns;
//            grSelected.DataSource = _dt_selected_columns;
//            _is_fact_use = _dt_all_columns.Columns.Contains(TextConst.AName.IsFactUse);
//        }
//        private void AddColumnsToGrid(GridView view, List<string> newColsNames)
//        {
//            int i = 1;
//            foreach (string colName in newColsNames)
//            {
//                var gridCol = new GridColumn()
//                {
//                    FieldName = colName,
//                    Visible = true
//                };
//                gridCol.OptionsColumn.AllowEdit = false;
//                gridCol.OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
//                if (newColsNames.Count > 2) {
//                    gridCol.Caption = " ";
//                } else {
//                    gridCol.Caption = i < newColsNames.Count ? "Группа" : "Колонка";
//                }
//                view.Columns.Add(gridCol);
//                i++;
//            }
//        }
//        private static string SEL_ORDER = "sel_order";
//        private int selOrderCounter = 0;
//        private DataTable CreateTable(IEnumerable<XElement> xColumnsList, DataTable dt_clone = null)
//        {
//            DataTable dt;
//            if (dt_clone != null) {
//                dt = dt_clone.Clone();
//                foreach (DataColumn c in dt.Columns) {
//                    c.AllowDBNull = true;
//                }
//                DataTable data = Cmn.XElementsToDataTable(xColumnsList);
//                foreach (DataRow row in data.Rows) {
//                    dt.ImportRow(row);
//                }
//            } else {
//                dt = Cmn.XElementsToDataTable(xColumnsList);
//                dt.Columns.Add("check", typeof(bool));
//                dt.PrimaryKey = new DataColumn[1] { dt.Columns["name"] };
//                dt.Columns.Add(SEL_ORDER, typeof(int));
//            }
//            //foreach (DataRow row in dt.Rows)
//            //{
//            //    row["selid"] = row["elid"];
//            //    row["spelid"] = row["pelid"];
//            //}
//            return dt;
//        }
//        private IEnumerable<XElement> GetProcessedColumns(XElement xColumns)
//        {
//            xColumns = new XElement(xColumns);

//            var xleaves = xColumns.Descendants().Where(e => !e.HasElements).ToArray();
//            xleaves.Where(e => Cmn.GetAttrValue(e, TextConst.AName.Title) == "" || e.AttrOrDef("visible", "1") != "1").Remove();
//            xleaves = xleaves.Where(e => e.Parent != null).ToArray();

//            if (titleColsNames.Count == 0) UpdateTitleColsNames(xleaves);

//            foreach (var xleaf in xleaves)
//            {
//                int i = 0;
//                foreach (var xnode in xleaf.AncestorsAndSelf().Reverse())
//                {
//                    if (xnode == xColumns) continue;
//                    xleaf.SetAttributeValue(titleColsNames[i++], xnode.Attribute(TextConst.AName.Title).Value);
//                    var dimension = Cmn.GetAttrValue(xnode, "dimension");

//                    if (dimension != "")
//                    {
//                        xleaf.SetAttributeValue("dim-name", dimension);
//                        xleaf.SetAttributeValue("dim-title", xnode.Attribute(TextConst.AName.Title).Value);
//                    }

//                }
//            }

//            return xleaves;
//        }
//        private void UpdateGridColumns()
//        {
//            viewAll.Columns.Clear();
//            viewAll.Columns.Add(colCheck);
//            colCheck.VisibleIndex = 0;

//            viewSelected.Columns.Clear();
//            viewSelected.Columns.Add(colOrder);
//            // отладка
//            //colOrder.VisibleIndex = 0;
     
//            AddColumnsToGrid(viewAll, titleColsNames);
//            AddColumnsToGrid(viewSelected, titleColsNames);
//        }
//        private void UpdateTitleColsNames(IEnumerable<XElement> xColumnsList)
//        {
//            int max_depth = xColumnsList.Max(c => c.Ancestors().Count());
                
//            titleColsNames.Clear();
//            for (int i = max_depth; i > 0; i--)
//            {
//                titleColsNames.Add(TextConst.AName.Title + i);
//            }
//        }

//        private void ChangeRowsCheck(bool selected_only, bool state)
//        {
//            IEnumerable<DataRow> rows = null;
//            var ordCol = "ord";
//            if (selected_only)
//            {
//                rows = viewAll.GetSelectedRows().Select<int, DataRow>(viewAll.GetDataRow);
//                ordCol = SEL_ORDER;
//            }
//            else
//            {
//                if (state)
//                {
//                    rows = Enumerable.Range(0, viewAll.RowCount).Select<int, DataRow>(viewAll.GetDataRow);
//                }
//                else
//                {

//                    rows = _dt_all_columns.Rows.ToArray();
//                }
//            }
//            //var rows = (selected_only)
//            //    // только выбранные
//            //    ? viewAll.GetSelectedRows().Select(ind => (viewAll.GetDataRow(ind)))
//            //    // все видимые
//            //    : Enumerable.Range(0, viewAll.RowCount).Select(ind => (viewAll.GetDataRow(ind)));

//            rows = (state)
//                ? rows.Where(r => r["check"] == DBNull.Value)//.OrderByDescending(r => (int)r["ord"])
//                : rows.Where(r => r["check"] != DBNull.Value)//.OrderByDescending(r => (int)r["ord"])
//                ;
//            rows = rows.OrderByDescending(r => (int)Cmn.Nvl(r[ordCol], 999999999));

//            viewAll.BeginUpdate();
//            viewSelected.BeginUpdate();

//            bool insert_first = (state && _dt_selected_columns.Rows.Count == 0);
//            DataRow first_row = null;
//            foreach (var row in rows)
//            {
//                row["check"] = (state) ? (object)1 : DBNull.Value;
//                Check(row, state, insert_first);
//                if (first_row == null) first_row = row;
//            }

//            viewSelected.EndUpdate();
//            viewAll.EndUpdate();

//            if (state) SyncSelection(viewAll, viewSelected);

//            if (first_row != null) viewSelected.FocusedRowHandle = viewSelected.LocateByValue("name", first_row["name"]);
//            DoSort();
//        }
//        private static int RowComparisonAsc(DataRow row1, DataRow row2)
//        {
//            return (int)row1["ord"] - (int)row2["ord"];
//        }
//        private static int RowComparisonDesc(DataRow row1, DataRow row2)
//        {
//            return (int)row2["ord"] - (int)row1["ord"];
//        }
//        private void MoveRow(int delta)
//        {
//            int min = 0;
//            int max = this._dt_selected_columns.Rows.Count - 1;
//            int[] selected = viewSelected.GetSelectedRows();
//            int count = selected.Length;
//            if (count == 0) {
//                return;
//            }
//            DataRow[] rows = new DataRow[count];
//            for (int index = 0; index < count; index++) {
//                rows[index] = viewSelected.GetDataRow(selected[index]);
//            }
//            if (delta < 0) {
//                System.Array.Sort<DataRow>(rows, RowComparisonAsc);
//            } else {
//                System.Array.Sort<DataRow>(rows, RowComparisonDesc);
//            }
//            // проверяем, что ord крайней строки не выходит за допустимые пределы
//            int first_ord = (int)rows[0]["ord"] + delta;
//            if (first_ord < min || first_ord > max) return;
//            viewSelected.BeginSort();
//            viewSelected.BeginUpdate();

//            //var firstRow = rows.First();

//            foreach (var row in rows)
//            {
//                var old_order = (int)row["ord"];
//                var new_order = old_order + delta;


//                DataRow next_row = _dt_selected_columns.AsEnumerable().FirstOrDefault(r => r["ord"].Equals(new_order));
//                if (next_row != null) next_row["ord"] = old_order;

//                row["ord"] = new_order;
//            }
//            DoSort();
//            viewSelected.EndUpdate();
//            viewSelected.EndSort();
//        }


//        private void DoSort()
//        {
//            viewSelected.SortInfo.Clear(); // !!! перестало автоматически сортировать. Это помогло.
//            viewSelected.SortInfo.Add(new GridColumnSortInfo(colOrder, DevExpress.Data.ColumnSortOrder.Ascending));

//        }
//        private void MoveBand(int delta)
//        {
//            var min = titleColsNames.Min(t => Convert.ToInt32(t.Last().ToString()));
//            var max = titleColsNames.Max(t => Convert.ToInt32(t.Last().ToString())); ;

//            var cells_info = viewSelected.GetSelectedCells()
//                .Where(c => titleColsNames.Contains(c.Column.FieldName))
//                .Select(c => new { Cell = c, ColumnNum = Convert.ToInt32(c.Column.FieldName.Last().ToString()) });
//            if (!cells_info.Any()) return;

//            cells_info = (delta < 0)
//                ? cells_info.OrderBy(ci => ci.ColumnNum)
//                : cells_info.OrderByDescending(ci => ci.ColumnNum);

//            // проверяем, что позиция крайней ячейки не выходит за допустимые пределы
//            var first_pos = cells_info.First().ColumnNum + delta;
//            if (first_pos < min || first_pos > max) return;

//            viewSelected.BeginUpdate();
//            foreach (var cell_info in cells_info)
//            {
//                var old_col_name = TextConst.AName.Title + cell_info.ColumnNum;
//                var new_col_name = TextConst.AName.Title + (cell_info.ColumnNum + delta);

//                var row = viewSelected.GetDataRow(cell_info.Cell.RowHandle);

//                var value = row[old_col_name];
//                row[old_col_name] = row[new_col_name];
//                row[new_col_name] = value;

//                // выделение ячеек сдвигаем вместе со значениями
//                viewSelected.UnselectCell(cell_info.Cell);
//                viewSelected.SelectCell(cell_info.Cell.RowHandle, viewSelected.Columns[new_col_name]);
//            }
//            viewSelected.EndUpdate();
//        }

//        private bool allowChangeOrder = true;
//        public void SetCantChangeOrder()
//        {
//            allowChangeOrder = false;
//            btnMoveBandLeft.Visibility = BarItemVisibility.Never;
            
//            btnMoveBandRight.Visibility = BarItemVisibility.Never;
            
//            btnShowMerge.Visibility = BarItemVisibility.Never;
//            btnColumnUp.Visibility = BarItemVisibility.Never;
//            btnColumnDown.Visibility = BarItemVisibility.Never;
//            btnInSelOrdr.Visibility = BarItemVisibility.Never;
            
//            this.bar2.LinksPersistInfo.Cast<DevExpress.XtraBars.LinkPersistInfo>().Where(l => l.Item == btnAccept).First().BeginGroup = false;
//            //btnAccept.Links[0].BeginGroup = false;
     
//        }
//        private void Check(DataRow row, bool state, bool insert_first)
//        {
//            // insert_first нужен, тк если изначально в _dt_selected_columns не было строк,
//            // при добавлении первой фокус устанавливается на нее и порядок первой строки съезжает

//            _supress_sync_selection = true;
//            // добавляем строку просле строки в фокусе и сдвигаем все под ней на 1 вниз
//            int ord=0;
//            if (state)
//            {
//                if (allowChangeOrder)
//                {
//                    var focused_row = viewSelected.GetFocusedDataRow();
//                     ord = (insert_first) ? 0 :
//                              (focused_row != null) ? (int)focused_row["ord"] + 1 :
//                              _dt_selected_columns.Rows.Count;

//                    var next_rows = _dt_selected_columns.AsEnumerable().Where(r => (int)r["ord"] >= ord);
//                    foreach (var next_row in next_rows)
//                    {
//                        next_row["ord"] = (int)next_row["ord"] + 1;
//                    }
//                }

               
//                _dt_selected_columns.ImportRow(row);
//                if (allowChangeOrder)
//                {
//                    _dt_selected_columns.Rows[_dt_selected_columns.Rows.Count - 1]["ord"] = ord;
//                }
//            }
//            // удаляем строку и сдвигаем все под ней на 1 вверх
//            else
//            {

//                var row_to_del = _dt_selected_columns.Rows.Find(row["name"]);
//                if (allowChangeOrder)
//                {
//                    ord = (int)row_to_del["ord"];

//                    var next_rows = _dt_selected_columns.AsEnumerable().Where(r => (int)r["ord"] > ord);
//                    foreach (var next_row in next_rows)
//                    {
//                        next_row["ord"] = (int)next_row["ord"] - 1;
//                    }
//                }

//                _dt_selected_columns.Rows.Remove(row_to_del);
//            }
//            _supress_sync_selection = false;

//        }

//        private void UpdateControlsSize()
//        {
//            grSelected.Width = this.Width / 2;
          
//        }

//        private void SyncSelection(GridView src, GridView dest)
//        {
//            if (_supress_sync_selection) return;
//            _supress_sync_selection = true;

//            dest.BeginUpdate();

//            var rows_src = src.GetSelectedRows().Select<int, DataRow>(src.GetDataRow);
//            var rows_dest = dest.GetSelectedRows().Select<int, DataRow>(dest.GetDataRow);

//            foreach (var row_src in rows_src)
//            {
//                if (rows_dest.Any(row2 => row2["name"].Equals(row_src["name"]))) continue;
//                var index = dest.LocateByValue("name", row_src["name"]);
//                dest.SelectRow(index);
//            }

//            foreach (var row_dest in rows_dest)
//            {
//                if (rows_src.Any(row1 => row1["name"].Equals(row_dest["name"]))) continue;
//                var index = dest.LocateByValue("name", row_dest["name"]);
//                dest.UnselectRow(index);
//            }
//            dest.EndUpdate();
//            int[] indexes = dest.GetSelectedRows();
//            if (indexes.Length != 0) {
//                dest.MakeRowVisible(indexes[indexes.Length - 1]);
//            }
//            DoSort();
//            indexes = dest.GetSelectedRows();
//            if (indexes.Length != 0) {
//                dest.FocusedRowHandle = indexes[0];
//            }
//            _supress_sync_selection = false;
//        }
//        #endregion
//        #region Обработчики событий
//        private void rceCheck_EditValueChanged(object sender, EventArgs e)
//        {
//            viewAll.PostEditor();
//            var row = viewAll.GetFocusedDataRow();
//            if (row == null) return;
//            var state = (row["check"] != DBNull.Value);

//            viewSelected.BeginUpdate();
//            Check(row, state, false);
//            viewSelected.EndUpdate();

//            SyncSelection(viewAll, viewSelected);

//            viewSelected.FocusedRowHandle = viewSelected.LocateByValue("name", row["name"]);
//        }

//        private void viewSelected_CellMerge(object sender, CellMergeEventArgs e)
//        {
//            GridView view = sender as GridView;

//            bool merge = true;
//            foreach (GridColumn col in view.VisibleColumns)
//            {
//                if (col.VisibleIndex <= e.Column.VisibleIndex)
//                {
//                    object val1 = view.GetRowCellValue(e.RowHandle1, col);
//                    object val2 = view.GetRowCellValue(e.RowHandle2, col);
//                    if (!val1.Equals(val2))
//                    {
//                        merge = false;
//                        break;
//                    }
//                }
//            }
//            e.Merge = merge;
//            e.Handled = true;
//        }

//        private void btnCheckAll_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ChangeRowsCheck(false, true);
//        }
//        private void btnClearAll_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ChangeRowsCheck(false, false);
//        }
//        private void btnCheckSelected_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ChangeRowsCheck(true, true);
//        }
//        private void btnClearSelected_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ChangeRowsCheck(true, false);
//        }

//        private void btnColumnDown_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            MoveRow(+1);
//        }
//        private void btnColumnUp_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            MoveRow(-1);
//        }
//        private void btnMoveBandLeft_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            MoveBand(+1);
//        }
//        private void btnMoveBandRight_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            MoveBand(-1);
//        }

//        private void btnAccept_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            DialogResult = DialogResult.OK;
//        }
//        private void btnCancel_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            DialogResult = DialogResult.Cancel;
//        }

//        private void btnShowMode_DownChanged(object sender, ItemClickEventArgs e)
//        {
//            bool show_mode = btnShowMerge.Down;
//            barManager1.BeginUpdate();
//            btnColumnUp.Enabled = !show_mode;
//            btnColumnDown.Enabled = !show_mode;
//            btnInSelOrdr.Enabled = !show_mode;
//            btnMoveBandLeft.Enabled = !show_mode;
//            btnMoveBandRight.Enabled = !show_mode;
//            barManager1.EndUpdate();

//            viewSelected.OptionsView.AllowCellMerge = show_mode;
//            if (!show_mode) SyncSelection(viewAll, viewSelected);
//        }

//        private void frmColumnsEditorNew_Load(object sender, EventArgs e)
//        {
//            UpdateControlsSize();
//        }
//        private void frmColumnsEditorNew_ResizeEnd(object sender, EventArgs e)
//        {
//            UpdateControlsSize();
//        }
//        #endregion
//        #region DragAndDrop
//        private void view_MouseDown(object sender, MouseEventArgs e)
//        {
//            var view = (GridView)sender;

//            _downHitInfo = null;

//            var hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));

//            if (Control.ModifierKeys != Keys.None) return;

//            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
//            {
//                _downHitInfo = hitInfo;
//            }
//        }
//        private void view_MouseMove(object sender, MouseEventArgs e)
//        {
//            var view = sender as GridView;

//            if (e.Button == MouseButtons.Left && _downHitInfo != null)
//            {
//                var dragSize = new Size(250, 50);//SystemInformation.DragSize;
//                var dragRect = new Rectangle(new Point(_downHitInfo.HitPoint.X - dragSize.Width / 2,
//                    _downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

//                if (!dragRect.Contains(new Point(e.X, e.Y)))
//                {
//                    DataRow[] data = view.GetSelectedRows().Select(view.GetDataRow);
//                    view.GridControl.DoDragDrop(data, DragDropEffects.Move);

//                    _downHitInfo = null;
//                    DXMouseEventArgs.GetMouseArgs(e).Handled = true;
//                }
//            }
//        }
//        #endregion

//        private HashSet<int> viewAllSelectesRows = new HashSet<int>();
//        private int viewAllLastPos = 0;

//        private HashSet<int> viewSelectedSelectesRows = new HashSet<int>();
//        private int viewSelectedLastPos = 0;
//        private void RemSelectOrder(GridView gv, HashSet<int> lastSelected, IEnumerable<int> nowSelected,ref int lastPos)
//        {
//            int curPos = 0;
//            bool first = true;
//            bool reverse = false;
//            var newSelected = new List<int>();
//            foreach (int i in nowSelected)
//            {

//                if (!lastSelected.Contains(i))
//                {
//                    if (first)
//                    {
//                        var row = gv.GetDataRow(i);
//                        curPos = (int)row["ord"];
//                        first = false;
//                        if (curPos < lastPos)
//                        {
//                            reverse = true;
//                        }
//                    }
//                    selOrderCounter++;
//                    newSelected.Add(i);
//                }

//            }
//            lastPos = curPos;
//            if (reverse)
//            {
//                newSelected.Reverse();
//            }
//            foreach (int i in newSelected)
//            {

//                var row = gv.GetDataRow(i);

//                row[SEL_ORDER] = selOrderCounter;

//                selOrderCounter++;
               
//            }
//            lastSelected.Clear();
//            foreach (var i in nowSelected) lastSelected.Add(i);
         
//        }

//        private void viewAll_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
//        {
//            //var row = viewAll.GetDataRow(e.ControllerRow);
//            //if (row != null)
//            //{
//            //    row[SEL_ORDER] = selOrderCounter;
               
//            //}
//            RemSelectOrder(viewAll, viewAllSelectesRows, viewAll.GetSelectedRows(),ref viewAllLastPos);
//            //this.Text = selOrderCounter.ToString();
//            // selOrderCounter++;
//            SyncSelection(viewAll, viewSelected);
//        }

//        private void viewSelected_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
//        {
//            RemSelectOrder(viewSelected, viewSelectedSelectesRows,viewSelected.GetSelectedRows(), ref viewSelectedLastPos);
//            SyncSelection(viewSelected, viewAll);
//        }

//        private void viewAll_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
//        {
            
            
//        }

//        private void btnInSelOrdr_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            OrderBySelection();
//        }
//        private static int RowComparison(DataRow row1, DataRow row2)
//        {
//            return (int)row1[SEL_ORDER] - (int)row2[SEL_ORDER];
//        }
//        private void OrderBySelection()
//        {
//            //var rows1 = viewSelected.GetSelectedRows().Select(i => viewSelected.GetDataRow(i));
//            //if (!rows1.Any()) return;
//            //var indexes = rows1.Select(r => (int)r["ord"]).ToList();
//            //var max = indexes.Max();
//            //var min = indexes.Min();
//            //var rows2 = _dt_selected_columns.AsEnumerable().Where(r => (int)r["ord"] <= max && (int)r["ord"] >= min).ToList();
//            //var rows3 = rows2.Where(r => !rows1.Contains(r));
//            //rows1 = rows1.OrderBy(r => (int)r[SEL_ORDER]);
//            int[] selected = this.viewSelected.GetSelectedRows();
//            int count = selected.Length;
//            if (count <= 0) {
//                return;
//            }
//            DataRow[] rows1 = new DataRow[count];
//            DataRow row;
//            int min = 0;
//            int max = 0;
//            int index, ord;
//            for (index = 0; index < count; index++) {
//                row = this.viewSelected.GetDataRow(selected[index]);
//                rows1[index] = row;
//                ord = (int)row["ord"];
//                //
//                if (index == 0) {
//                    max = ord;
//                    min = ord;
//                } else {
//                    if (ord < min) {
//                        min = ord;
//                    }
//                    if (ord > max) {
//                        max = ord;
//                    }
//                }
//            }
//            System.Array.Sort<DataRow>(rows1, RowComparison);
//            List<DataRow> rows2 = new List<DataRow>();
//            for (index = 0; index < _dt_selected_columns.Rows.Count; index++) {
//                row = _dt_selected_columns.Rows[index];
//                ord = (int)row["ord"];
//                if (ord <= max && ord >= min) {
//                    rows2.Add(row);
//                }
//            }
//            //var rows3 = rows2.Where(r => !rows1.Contains(r));
//            int i1 = min;
//            viewSelected.BeginSort();
//            viewSelected.BeginUpdate();
//            for (index = 0; index < rows1.Length; index++) {
//                rows1[index]["ord"] = i1;
//                i1++;
//            }
//            for (index = 0; index < rows2.Count; index++) {
//                row = rows2[index];
//                if (!rows1.Contains(row)) {
//                    row["ord"] = i1;
//                    i1++;
//                }
//            }
//            DoSort();
//            viewSelected.EndUpdate();
//            viewSelected.EndSort();
//        }
//        private void viewAll_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
//        {

//            //if (_dt_all_columns.Columns.Contains(TextConst.AName.IsFactUse) && e.Column.FieldName == titleColsNames.Last())
//            //{

              
//            //        var row = viewAll.GetDataRow(e.RowHandle);
//            //        if (row != null)
//            //        {
//            //            if (row[TextConst.AName.IsFactUse].ToString() == TextConst.AVBool.True)
//            //            {
//            //                e.DisplayText = "• " + row[titleColsNames.Last()];
//            //            }
//            //        }

                    
               

//            //}

//            if (_textDecode != null)
//            {
//                if (_textDecode.ContainsKey(e.DisplayText))
//                {
//                    e.DisplayText = _textDecode[e.DisplayText];
//                }
//            }
//        }

//        private void viewSelected_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
//        {
//            if (_textDecode != null)
//            {
//                if (_textDecode.ContainsKey(e.DisplayText))
//                {
//                    e.DisplayText = _textDecode[e.DisplayText];
//                }
//            }
//        }

//        private void view_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs e)
//        {
//            if (!_is_fact_use) return;

//            var row = ((GridView)sender).GetDataRow(e.RowHandle);
//            if (row == null) return;

//           // e.Info.ImageIndex = (row[TextConst.AName.IsFactUse].ToString() == TextConst.AVBool.True) ? 2 : 5;
//        }
//    }
//}
