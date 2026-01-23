//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Reflection;
//using System.Xml.Linq;
//using DevExpress.Utils;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraGrid.Views.Grid.ViewInfo;
//using DevExpress.XtraEditors.Repository;
//using infoenergo.ui.win.Base;
//using infoenergo.ui.win.Grid;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using GridControl = DevExpress.XtraGrid.GridControl;
////using System.Windows.Forms;
//using System.Drawing;
//using DevExpress.Data;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid;

//using infoenergo.interfaces;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
//using System.Text;
//using System.Text.RegularExpressions;
//using Devart.Data.Oracle;
//using infoenergo.sys;
//using sql.builder.Controls.Grids;
//using sql.builder.DataApi.DataObjects;
//using EnumExportFormat = infoenergo.ui.win.Bars.EnumExportFormat;

//namespace sql.builder.Controls
//{
//    internal partial class ucReferenceGrid : ucGridBase/* IReportGrid,*/
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
//        bool _tableEventsAttached;
//        bool _cancel_refresh;
//        bool _need_refresh;
//        #endregion
//        #region Свойства
//        //
//        #endregion
//        #region События
//        //
//        #endregion
//        #region Открытые методы
//        //public override void RaiseLoad()
//        //{
//        //    grReference.Show();
//        //}
//        public ucReferenceGrid()
//        {
//            InitializeComponent();

//            grReference.BeforeRowNew += grReference_BeforeRowNew;
//            grReference.OptionsInternalToolBar.ButtonRefresh.Click += ButtonRefreshOnClick;
//            grReference.OptionsInternalToolBar.ButtonChoiceRow.Click += ButtonChoiceRowOnClick;
//            grReference.ModifiedData += RaiseModifiedData;
//            grReference.AfterRefreshRows += RaiseRefreshedData;
//            grReference.AfterAccept += RaiseCommitedData;

//            MainView.GridControl.ToolTipController = tooltip;

//            //MainView.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
//            //MainView.OptionsSelection.MultiSelect = false;

//            //if (XmlReports.IsDeveloperMode() && Environment.UserName == "vemtsov") grReference.StateColorize = true;
//        }

//        protected override void OnDisposed(object sender, EventArgs args)
//        {
//            // чтобы разорвать связь с обработчиками из UIFormC
//            grReference.OptionsInternalToolBar.UserItems.Clear();

//            var table = (VDataTable)_source.Tables[MainView.Name];
//            detachTableEvents(table);

//            base.OnDisposed(sender, args);
//        }

//        public override void SetTitle(string title)
//        {
//            if (title != "")
//            {
//                grReference.ShowCaption = true;
//            }
//            else
//            {
//                grReference.ShowCaption = false;
//            }

//            grReference.Caption = title;
//        }

//        public override void SetToolbarVisible(bool val)
//        {
//            grReference.OptionsInternalToolBar.ShowInternalToolBar = val;
//        }

//        public override void SetToolbarButtonVisible(string name, bool visible)
//        {
//            GetToolBarButton(name).Visible = visible;
//        }

//        public override void HideToolbarButtons()
//        {
//            grReference.OptionsInternalToolBar.ButtonAddRow.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonChoiceRow.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonCommit.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonCoppyRow.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonDeleteRow.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonEditMode.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonExport.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonFetchAll.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonPrintPreview.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonRefresh.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonSettings.Visible = false;
//            grReference.OptionsInternalToolBar.ButtonViewMode.Visible = false;
//        }

//        public override void AddToolBarItem(BarItem item)
//        {
//            //var ff = grReference.OptionsInternalToolBar.InsertItem(button, 99);
//            grReference.OptionsInternalToolBar.UserItems.Add(item);
//        }

//        public override void ExportToXlsx(string fullpath, string caption = null)
//        {
//            grReference.ExportTo(EnumExportFormat.Xls);
//        }
//        public GridView CurrentView { get; private set; }
//        public GridView GetMainView()
//        {
//            return MainView;
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
//        public void RemoveLayoutChangedHandler(Action action, EventHandler action2)
//        {
//            LayoutChanged -= action;
//        }
//        public TableViewMode ViewMode { get; private set; }
//        public Control GetControl()
//        {
//            return this;
//        }

//        public void SetPrintingTime(string time)
//        {
//            throw new NotImplementedException();
//        }

//        public string GetFormingTime()
//        {
//            throw new NotImplementedException();
//        }

//        public PopupMenu GetMenu()
//        {
//            return Menu;
//        }
//        public void SetMenu(PopupMenu menu)
//        {
//            Menu = menu;
//        }
//        //private void ValidateData()
//        //{



//        //    var dataView = GetMainGridView().DataSource as DataView;

//        //    List<DataRowView> dataRowsView = dataView.Cast<DataRowView>().ToList();
//        //    var data1 = dataRowsView.Select(r => r as IDataErrorInfo).ToList();
//        //    var data = data1.Select(r => (MyModel)r).ToList();





//        //    if (data != null)
//        //    {
//        //        foreach (var item in data)
//        //        {
//        //            //item.ClearErrors();
//        //            //item.NoteError = "Required fields.";
//        //            item.NoteError = "Required fields.";
//        //            item.SetColumnError("NameOfColumnA", "Required field.");     
//        //        }
//        //    }
//        //}

//        //private void gc_view_ValidateRow(object sender, DevExpress.XtraGrid.Views.Base.ValidateRowEventArgs e)
//        //{
//        //    ValidateData();
//        //}
//        public override void LoadSchemeSettingsFromXml(XElement xRoot)
//        {
//            //var grid = new GridControl();
//            var grid = new sql.builder.Controls.Grids.ReportViewModes.ucGridWF();
//            Parser.LoadGridSettingsFromXml(xRoot.Element("scheme"), grid);

//            MainView.Name = grid.MainView.Name;
//            var view = grid.MainView as GridView;

//            //view.OptionsSelection.EnableAppearanceFocusedCell = false;
//            //view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
//            foreach (GridColumn column in view.Columns)
//            {
//                var newCol = new infoenergo.ui.win.Grid.Column.GridColumn
//                {
//                    Name = column.Name,
//                    FieldName = column.FieldName,
//                    Caption = column.Caption,// + "(" + column.FieldName + ")",
//                    Visible = column.Visible,
//                    UnboundType = column.UnboundType,
//                    Tag = column.Tag
//                };

//                if (column.Tag != null)
//                {
//                    var ext_options = column.Tag as Dictionary<string, string>;
//                    if (ext_options.ContainsKey(TextConst.AName.FixedSide))
//                    {
//                        newCol.Fixed = (ext_options[TextConst.AName.FixedSide] == TextConst.AVFixedSide.Left) ? FixedStyle.Left :
//                                       (ext_options[TextConst.AName.FixedSide] == TextConst.AVFixedSide.Right) ? FixedStyle.Right :
//                                        FixedStyle.None;
//                    }
//                }

//                newCol.DisplayFormat.FormatType = column.DisplayFormat.FormatType;
//                newCol.DisplayFormat.FormatString = column.DisplayFormat.FormatString;
//                newCol.SummaryItem.SummaryType = column.SummaryItem.SummaryType;
//                newCol.SummaryItem.DisplayFormat = column.SummaryItem.DisplayFormat;

//                view.GroupSummary.Add(new GridGroupSummaryItem(column.SummaryItem.SummaryType, column.FieldName, column, column.SummaryItem.DisplayFormat));


//                // чтобы не вылетал автофильтр для числовых значений
//                if (column.UnboundType != UnboundColumnType.String)
//                {
//                    newCol.AutoFilterCondition = AutoFilterCondition.Equals;
//                }

//                grReference.Columns.Add(newCol);
//            }

//            colDummy = new infoenergo.ui.win.Grid.Column.GridColumn();
//            colDummy.Visible = true;
//            grReference.Columns.Add(colDummy);

//        }
//        public void ShowExcel(string path)
//        {
//            throw new NotImplementedException();
//        }

//        public void AllowOpenExcel()
//        {
//            throw new NotImplementedException();
//        }

//        public void SetText(string text)
//        {
//            //
//        }
//        GridColumn colDummy = null;
//        public override GridView GetMainGridView()
//        {
//            var grid = grReference;
//            var baseGridControlInfo = typeof(infoenergo.ui.win.Grid.GridControl).GetField("baseGridControl", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
//            var gridViewInfo = baseGridControlInfo.FieldType.GetField("View", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
//            var baseGridControl = baseGridControlInfo.GetValue(grid);
//            return (GridView)gridViewInfo.GetValue(baseGridControl);
//        }


//        public override void AcceptChanges()
//        {
//            MainView.PostEditor();
//            MainView.UpdateCurrentRow();

//            // помечаем удаленные строки как реально удаленные
//            var deleted_rows = grReference.ListRowsToDelete;
//            if (deleted_rows.Count > 0) grReference.SetDeleteItem(deleted_rows);
//            foreach (var row in deleted_rows)
//            {
//                row.Delete();
//            }

//            // Емцов - удаляем сразу. в случае ошибки не востановятся! ну и ладно
//            var table = (VDataTable)_source.Tables[MainView.Name];
//            var rows = table.Rows.Cast<DataRow>().Where(row => grReference.GetRowState(row).AddAndDelete).ToArray();
//            foreach (var r in rows) table.Rows.Remove(r);

//            base.AcceptChanges();
//        }
//        public override void DismissChanges(Dictionary<DataRow, OracleException> error_rows = null, bool get_my_errors = false)
//        {
//            if (error_rows != null && error_rows.Count > 0)
//            {
//                if (get_my_errors)
//                {
//                    var my_error_rows = new Dictionary<DataRow, OracleException>();
//                    foreach (var r in error_rows.Where(er => er.Key.Table.TableName == MainView.Name)) my_error_rows.Add(r.Key, r.Value);
//                    error_rows = my_error_rows;
//                }

//                bool error_is_shown = false;
//                foreach (var error_row in error_rows)
//                {
//                    if (error_row.Key.RowState == DataRowState.Deleted)
//                    {
//                        _deleted_error_rows.Add(error_row.Key);
//                        error_row.Key.RejectChanges();
//                    }

//                    if (error_row.Value != null && !error_is_shown)
//                    {
//                        CustomOracleError.HandleIfNeed(error_row.Key, error_row.Value);
//                        error_is_shown = true;
//                    }
//                }

//                //var error_message = Cmn.GetErrorRowsMessage(error_rows);
//                //ShowMessage.Show(ShowMessage.MType.ErrorSaveRowsChanges, error_message);
//            }

//            base.DismissChanges(error_rows);
//        }
//        public override void SetEditable(bool val)
//        {
//            if (!val) grReference.SetViewMode();
//            else grReference.SetEditMode();

//            base.SetEditable(val);
//        }

//        public override bool IsModifiedData()
//        {
//            return grReference.IsModifiedData;
//        }
//        public override bool SetUnmodified()
//        {
//            return grReference.SetGridUnModifed();
//        }

//        public override void SetSummaryVisible(bool val)
//        {
//            grReference.ShowFooter = val;
//        }

//        public override void SetMultiselect(bool val)
//        {
//            grReference.MultiSelect = val;
//        }


//        //public override void SetEditable(bool val)
//        //{
//        //    base.SetEditable(val);
//        //    if (val)
//        //    {
//        //        grReference.SetEditMode();
//        //    }
//        //    else
//        //    {
//        //        grReference.SetViewMode();
//        //    }
//        //}

//        #endregion
//        #region Закрытые методы
//        private void AfterModeChange(Mode mode)
//        {
//            //
//        }

//        public override void SetMasterControl(ucGridBase parentControl)
//        {
//            grReference.MasterControl = (parentControl as ucReferenceGrid).grReference;

//        }

//        private void attachTableEvents(VDataTable table)
//        {
//            if (_tableEventsAttached) return;

//            _tableEventsAttached = true;
//            table.UIEvent += Table_UIEvent;

//            table.TableRefreshed += DataTable_OnTableRefreshed;
//            table.TableCommited += DataTable_OnTableCommited;
//            table.MyRowChanged += DataTable_OnRowChanged;
//            table.MyColumnChanged += DataTable_OnColumnChanged;
//            table.RowStateChanged += DataTable_OnRowStateChanged;
//            table.ColumnVisibleChanged += DataTable_onColumnVisibleChanged;
//        }
//        private void detachTableEvents(VDataTable table)
//        {
//            if (!_tableEventsAttached) return;

//            _tableEventsAttached = false;
//            table.UIEvent -= Table_UIEvent;

//            table.TableRefreshed -= DataTable_OnTableRefreshed;
//            table.TableCommited -= DataTable_OnTableCommited;
//            table.MyRowChanged -= DataTable_OnRowChanged;
//            table.MyColumnChanged -= DataTable_OnColumnChanged;
//            table.RowStateChanged -= DataTable_OnRowStateChanged;
//            table.ColumnVisibleChanged -= DataTable_onColumnVisibleChanged;
//        }

//        private void DataTable_OnTableRefreshed(object sender, EventArgs args)
//        {
//            //_repositories.Clear();
//            _repositoriesEditable.Clear();
//            _repositoriesReadonly.Clear();
//            //_repositoriesEditable.Clear();
//            grReference.SetGridUnModifed();
//        }
//        private void DataTable_OnTableCommited(object sender, EventArgs args)
//        {
//            grReference.SetGridUnModifed();
//        }
//        private infoenergo.ui.win.Grid.GridControl.OptionsInternalToolBarClass.ButtonClass GetToolBarButton(string name)
//        {
//            return
//                (infoenergo.ui.win.Grid.GridControl.OptionsInternalToolBarClass.ButtonClass)
//            Cmn.GetProperty(grReference.OptionsInternalToolBar, name);
//        }
//        public override void AcceptSelection()
//        {
//            UpdateDataSourceSelectedRows();
//            SourceTable().GetDataSet().ChoiceSource = SourceTable();
//            FindForm().Close();
//            // SourceTable().AcceptSelection();
//        }

//        public override void SetSelection(IEnumerable<object> values)
//        {
//            bool fetched = false;
//            var table = SourceTable();
//            foreach (var value in values)
//            {
//                var row = table.Rows.Find(value);
//                if (row == null && !fetched)
//                {
//                    grReference.BeginUpdate();
//                    table.FetchTo(int.MaxValue);
//                    grReference.EndUpdate();
//                    fetched = true;

//                    row = table.Rows.Find(value);
//                }

//                if (row != null)
//                {
//                    MainView.SelectRow(MainView.GetRowHandle(table.Rows.IndexOf(row)));
//                }
//            }

//            UpdateDataSourceSelectedRows();
//        }
//        public void SetSelectMode()
//        {
//            GetMainView().OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
//        }
//        public bool IsSelectMode()
//        {
//            return (GetMainView().OptionsSelection.MultiSelectMode == GridMultiSelectMode.CheckBoxRowSelect);
//        }
//        #endregion
//        #region Обработчики событий
//        private void uсReferenceGrid_Load(object sender, EventArgs e)
//        {
//            // для корректной работы в режиме дизайнера
//            if (Global.Connection == null) return;

//            //if (Editable)
//            //{
//                //grReference.SetEditMode();
//            //}

//            // AfterModeChange(grReference.Mode);
//            grReference.BorderStyle = BorderStyle.None;

//            var view = MainView;
//            view.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;
//            view.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
//            view.OptionsNavigation.AutoFocusNewRow = true;
//            view.GridControl.ShowOnlyPredefinedDetails = true;
//            view.OptionsView.ColumnHeaderAutoHeight = DefaultBoolean.True;
//            view.DoubleClick += gridView_DoubleClick;
//            view.ColumnWidthChanged += gridView_OnColumnWidthChanged;
//            view.ColumnPositionChanged += gridView_OnColumnPositionChanged;
//            view.SelectionChanged += gridView_OnSelectionChanged;
//            view.CustomDrawCell += view_CustomDrawCell;
//            //view.CustomColumnDisplayText += view_CustomColumnDisplayText;
//            //view.Layout += view_Layout;//!! почему то не срабатывает
//            view.ColumnWidthChanged += view_Layout;

//            grReference.Resize += view_Layout;
//            //view.MouseMove += gridView_MouseMove;
//            //view.MouseEnter += gridView_MouseEnterLeave;
//            //view.MouseLeave += gridView_MouseEnterLeave;


//            //   ButtonSaveAndClose = new BarButtonItem(null, "Сохранить и закрыть");
//            ////   ButtonSaveAndClose.Visibility = ;
//            //   AddToolBarButton(ButtonSaveAndClose);  

//        }

//        //void view_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
//        //{

//        //    var rowHandle = (MainView as GridView).GetRowHandle(e.ListSourceRowIndex);
//        //    var r = grReference.GetDataRow(rowHandle);
//        //    if (r == null) return;

//        //    var colName = e.Column.FieldName;
//        //    if (!string.IsNullOrEmpty(colName))
//        //    {
//        //        var table = r.Table as VDataTable;
//        //        var column = table.Columns[colName] as VDataColumn;

//        //        // кешировать
//        //        var vis = column.GetVisibility(r);
//        //        if (!vis)
//        //        {
//        //            e.DisplayText = "";
//        //        }
//        //    }
//        //}


//        void view_Layout(object sender, EventArgs e)
//        {// Чтобы удобно было менять ширину последней колонки
//            GridViewInfo info = MainView.GetViewInfo() as GridViewInfo;
//            var w = colDummy.Width + info.ViewRects.ColumnPanelWidth - info.ViewRects.ColumnTotalWidth;
//            if (w < 100)
//            {
//                w = 100;
//            }
//            colDummy.Width = w;
//            // MainView.sc
//        }
//        //private BarButtonItem ButtonSaveAndClose = null;

        




//        void view_CustomDrawCell(object sender, RowCellCustomDrawEventArgs e)
//        {
//            var r = grReference.GetDataRow(e.RowHandle);
//            if (r == null) return;

//            // цвет текста
//            var table = r.Table as VDataTable;


//            var colName = table.GetNameForText(e.Column.FieldName);
//            if (colName == null) return;
//            var column = table.Columns[colName] as VDataColumn;

//            // кешировать все значения - валидация уже

//            bool drawwarning = false;
//            bool drawedit = false;
//            RepositoryItem rep = null;

//            string rgb = column.GetFontColor(r);
//            Color color;
//            if (VColor.ParseRGB(rgb, out color)) {
//                e.Appearance.ForeColor = color;
//            }

//            var msg = table.GetCellError(r, colName);

//            if (!string.IsNullOrEmpty(msg))
//            {
//                drawwarning = true;
//            }



//            if (Editable)
//            {
//                if (column.GetEditable(r))// !!! проконтролировать время
//                {
//                    var row_index = MainView.GetDataSourceRowIndex(e.RowHandle);// !!! проконтролировать время
//                    rep = GetCellRepository(colName, row_index/*, true*/);// !!! проконтролировать время
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
//            }




//            if (drawwarning || drawedit)
//            {

//                string text = e.DisplayText;

//                if (drawwarning)
//                {
//                    var s = "     ";
//                    if (text != "" && !text.StartsWith(s))
//                    {
//                        text = s + text;
//                    }

//                }
//                if (drawedit)
//                {
//                    if (!(rep is RepositoryItemHyperLinkEdit))
//                    {
//                        var s = "    ";
//                        if (text != "" && !text.EndsWith(s))
//                        {
//                            text = text + s;
//                        }
//                    }
//                }
//                e.DisplayText = text;
//                e.DefaultDraw();
//                if (drawwarning)
//                {
//                    if (rep == null)
//                    {
//                        var row_index = MainView.GetDataSourceRowIndex(e.RowHandle);// !!! проконтролировать время
//                        rep = GetCellRepository(colName, row_index/*, true*/);// !!! проконтролировать время
//                    }
//                    var shift = Cmn.GetLeftButtonsSize( colName, rep);
//                    Image im = Cmn.ImageWarning14;
//                    e.Graphics.DrawImage(im, new Point(e.Bounds.Location.X + 1 + shift, e.Bounds.Location.Y + 2));
//                }
//                if (drawedit)
//                {

//                    Image im = Cmn.ImageEdit12;
//                    e.Graphics.DrawImage(im, new Point(e.Bounds.Location.X + e.Bounds.Width - 16, e.Bounds.Location.Y + 1));

//                }
//                e.Handled = true;
//            }




//        }

//        private void gridView_DoubleClick(object sender, EventArgs e)
//        {

//            // чтобы обрабатывался double click в первой колонке
//            GridView view = (GridView)sender;
//            Point pt = view.GridControl.PointToClient(MousePosition);
//            GridHitInfo info = view.CalcHitInfo(pt);
//            if (info.InRow || info.InRowCell)
//            {
//                RaiseUIEvent("", TextConst.AVEventName.DoubleClick, null, null);
//                //string colCaption = info.Column == null ? "N/A" : info.Column.GetCaption();
//                //MessageBox.Show(string.Format("DoubleClick on row: {0}, column: {1}.", info.RowHandle, colCaption));
//            }

//        }
//        private void gridView_OnColumnWidthChanged(object sender, ColumnEventArgs args)
//        {
//              grReference.SaveSettings();
//        }
//        private void gridView_OnColumnPositionChanged(object sender, EventArgs args)
//        {
//            //  grReference.SaveSettings();
//        }
//        private void gridView_OnSelectionChanged(object sender, SelectionChangedEventArgs args)
//        {
//            if (grReference.MultiSelectMode != GridMultiSelectMode.CheckBoxRowSelect) return;

//            var table = (VDataTable)_source.Tables[MainView.Name];
//            table.ManualUserChangedData();
//        }
//        //private void gridView_MouseMove(object sender, MouseEventArgs e)
//        //{
//        //    var hi = (sender as GridView).CalcHitInfo(new Point(e.X, e.Y));
//        //    if (hi.Column != null)
//        //    {
//        //        Cursor = Cursors.Default;
//        //    }
//        //}
//        //private void gridView_MouseEnterLeave(object sender, EventArgs e)
//        //{
//        //    if (Cursor == Cursors.Hand) Cursor = Cursors.Default;
//        //}

//        private void ButtonRefreshOnClick(object sender, EventArgs args)
//        {
//            if (_source == null) return;
//            if (grReference.IsModifiedData)
//            {
//                var result = ShowMessage.Show(ShowMessage.MType.UnsavedChangesQuestion);
//                if (result == DialogResult.Cancel)
//                {
//                    _cancel_refresh = true;
//                    return;
//                }
//                else if (result == DialogResult.Yes)
//                {
//                    grReference.CommitChanges();
//                }
//            }


//            var table = (VDataTable)_source.Tables[MainView.Name];
//            table.Refresh();
//            //ValidateData();
//        }
//        private void ButtonChoiceRowOnClick(object sender, EventArgs args)
//        {

//            AcceptSelection();
//            return;
//            // Емцов - тест read_only
//            //var column_name = MainView.FocusedColumn.FieldName.Replace(TextConst.Pfx.ExtValName, "");
//            //var row_index = MainView.GetDataSourceRowIndex(MainView.FocusedRowHandle);

//            //var read_only = GetCellReadOnly(column_name, row_index);
//            //SetCellReadOnly(column_name, row_index, !read_only);
//        }

//        private bool Table_UIEvent(object sender, UIEventArgs e)
//        {
//            return RaiseUIEvent("", e.EventName, e.Row, e.Column);
//        }

//        private void grReference_BeforeRefreshRows(object sender, GridDataRefreshEventArgs e)
//        {
//            // пока так
//            if (_cancel_refresh)
//            {
//                _cancel_refresh = false;
//                e.cancel = true;
//                return;
//            }

//            var table = (VDataTable)_source.Tables[MainView.Name];
//            e.dataTable = table;
//            attachTableEvents(table);

//            if (table.IsDeferredFetch())
//            {
//                if (grReference.AllRowsFetchedFlag)
//                {
//                    grReference.BeginUpdate();
//                    table.FetchTo(int.MaxValue);
//                    grReference.EndUpdate();
//                }
//                else
//                {
//                    //table.FetchTo(10);

//                }
//            }
//            // все равно сохранение фокуса не работает
//            if (grReference.KeyColumn == null && table.HasPrimaryKey()) {
//                grReference.KeyColumn = (IUIColumn)grReference.Columns[table.PrimaryKey[0].ColumnName];
//            }
//        }

//        List<DataRow> _error_rows = new List<DataRow>();
//        List<DataRow> _deleted_error_rows = new List<DataRow>();
//        private void grReference_BeforeAccept(object sender, EventArgs e)
//        {
//            //
//        }
//        private void grReference_AfterModeChange(object sender, BaseGridControl.GridModeChangeEventArgs e)
//        {
//            AfterModeChange(e.Mode);
//        }
//        private void grReference_BeforeRowNew(object sender, BeforeRowNewEventArgs e)
//        {
//            //e.Cancel = true;
//            //e.Row[grReference.KeyColumn.FieldName] = e.NewID;

//            //e.Cancel = RaiseToolbarButtonClick(TextConst.GridButton.Add, GetButtonTag(TextConst.GridButton.Add));


//        }
//        private void grReference_GetFetchAllEvent(object sender, System.ComponentModel.HandledEventArgs e)
//        {
//            e.Handled = true;

//            var table = (VDataTable)_source.Tables[MainView.Name];

//            //if (table.SuppressChangedEvent) return;


//            table.FetchNext(100);

//        }
//        private void grReference_RowCellClick(object sender, RowCellClickEventArgs e)
//        {
//            if (e.Clicks == 1 && e.Button == MouseButtons.Right)
//            {
//                RaiseCellRightClick(grReference.GetDataRow(e.RowHandle), e.Column.FieldName);
//            }
//        }
//        private void grReference_BeforeRowSetDelete(object sender, RowValueChangedCanCancelEventArgs e)
//        {
//            var table = (VDataTable)_source.Tables[MainView.Name];
//            string delValid = table.GetDeleteValidation(e.Row);
//            if (delValid != null)
//            {
//                RaiseHasMessage("Удаление невозможно. " + delValid);
//                e.Cancel = true;
//            }
//            else
//            {
//                // пользователь нажал удаление - загорается кнопка сохранить
//                table.ManualUserChangedData();
//            }
//        }

//        private void grReference_RowCellDoubleClick(object sender, RowCellClickEventArgs e)
//        {
//            RaiseUIEvent("", TextConst.AVEventName.DoubleClick, null, null);
//        }
//        private void grReference_RowValidate(object sender, RowValidateEventArgs e)
//        {
//            if (_deleted_error_rows.Contains(e.Row))
//            {
//                grReference.SetDeleteItem(new[] { e.Row }.ToList());
//                _deleted_error_rows.Remove(e.Row);
//            }

//            if (_error_rows.Contains(e.Row))
//            {
//                _error_rows.Remove(e.Row);
//                e.Cancel = true;
//            }
//        }
//        private void grReference_RowInsert(object sender, infoenergo.ui.win.Base.RowEventArgs e)
//        {
//            var msg = (e.Row.Table as VDataTable).GetRowErrorText(e.Row).Error;
//            if (msg != "")
//            {
//                e.Cancel = true;
//                //var message = e.Row.GetColumnError(e.Row.GetColumnsInError().First());
//                RaiseHasMessage(msg);
//            }
//        }
//        private void grReference_RowUpdate(object sender, infoenergo.ui.win.Base.RowEventArgs e)
//        {

//            var msg = (e.Row.Table as VDataTable).GetRowErrorText(e.Row).Error;
//            if (msg != "")
//            {
//                e.Cancel = true;
//                //var message = e.Row.GetColumnError(e.Row.GetColumnsInError().First());
//                RaiseHasMessage(msg);
//            }
//        }

//        //private void ButtonRefreshOnClick(object sender, EventArgs args)
//        //{
//        //    if (_source == null) return;
//        //    var table = (VDataTable)_source.Tables[MainView.Name];
//        //    table.Refresh(false);
//        //    grReference.RefreshData();
//        //}



//        //private void ButtonChoiceRowOnClick(object sender, EventArgs args)
//        //{
//        //    // Емцов - тест read_only
//        //    var column_name = MainView.FocusedColumn.FieldName.Replace(Pfx.ExtValName, "");
//        //    var row_index = MainView.GetDataSourceRowIndex(MainView.FocusedRowHandle);

//        //    var read_only = GetCellReadOnly(column_name, row_index);
//        //    SetCellReadOnly(column_name, row_index, !read_only);
//        //}

//        private void DataTable_OnRowChanged(object sender, DataRowChangeEventArgs args)
//        {
//            //if ((args.Row.Table as VDataTable).SuppressChangedEvent) return;
//            //args.Row.Table.ColumnChanged += DataTable_OnColumnChanged;
//            //if (args.Action == DataRowAction.Commit) return;
//            //var state = grReference.GetRowState(args.Row);
//            //var unchanged = args.Row.RowState != DataRowState.Added && !state.AddAndDelete && !state.Delete && !state.Insert && !state.Update;
//            //if (unchanged) grReference.SetRowModifed(args.Row);

//        }
//        private void DataTable_onColumnVisibleChanged(object sender, DataColumnChangeEventArgs args)
//        {
//            // !!! это пока ни где не применено
//            // var col= (MainView as GridView).Columns[args.Column.ColumnName];
//            // col.Visible = (args.Column as VDataColumn).GetVisibility(args.Row);
//        }
//        private void DataTable_OnRowStateChanged(object sender, DataRowChangeEventArgs e)
//        {
//            var state = grReference.GetRowState(e.Row);
//            var unchanged = /*args.Row.RowState != DataRowState.Added && */!state.AddAndDelete && !state.Delete && !state.Insert && !state.Update;
//            if (unchanged)
//            {
//                if (e.Row.RowState == DataRowState.Added)
//                {
//                    grReference.SetRowNew(e.Row);

//                }
//                else if (e.Row.RowState == DataRowState.Modified)
//                {
//                    grReference.SetRowModifed(e.Row);
//                }

//                grReference.OptionsInternalToolBar.ButtonCommit.Enabled = true;
//                //else if (e.Row.RowState == DataRowState.Deleted)
//                //{
//                //    //var table = (VDataTable)_source.Tables[MainView.Name];
//                //    //table.ManualUserChangedData();
//                //    //e.Row.RejectChanges();
//                //    //var list = new List<DataRow>();
//                //    //list.Add(e.Row);
//                //    //grReference.SetDeleteItem(list);
//                //}

//            }

//        }
//        private void DataTable_OnColumnChanged(object sender, DataColumnChangeEventArgs args)
//        {
//            var table = (args.Row.Table as VDataTable);
//            var column = (args.Column as VDataColumn);
//            if (/*table.SuppressChangedEvent ||*/ !column.IsUpdateable) return;

//            var state = grReference.GetRowState(args.Row);
//            var unchanged = /*args.Row.RowState != DataRowState.Added && */!state.AddAndDelete && !state.Delete && !state.Insert && !state.Update;
//            if (unchanged)
//            {
//                if (args.Row.RowState == DataRowState.Added)
//                {
//                    // grReference.SetRowNew(args.Row);// Дает ошибку у Кресса из за того что состояние установливается дважды 

//                }
//                else
//                {
//                    grReference.SetRowModifed(args.Row);
//                }

//            }
//            //grReference.Refresh();
//            //var view = MainView as GridView;
//            // MessageBox.Show(
//            // view.GetDisplayTextByColumnValue(view.Columns[args.Column.ColumnName], args.Row[args.Column]);
//            // );
//            // MainView.Controller.FocusedRecord.Invalidate(AColumn);
//            MainView.PostEditor();
//        }

//        protected override void OnDataSourceChanged()
//        {
//            base.OnDataSourceChanged();
//            grReference.RefreshData();

//        }
//        protected override void OnBeforeDataSourceChanged()
//        {
//            base.OnBeforeDataSourceChanged();

//            if (_source == null) return;
//            var table = (VDataTable)_source.Tables[MainView.Name];
//            detachTableEvents(table);
//        }
//        #endregion
//        //   private int i = 1;
//        private void tooltip_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
//        {

//            if (e.Info == null && e.SelectedControl == GetMainGridView().GridControl)
//            {
//                GridView view = GetMainGridView();

//                GridHitInfo info = view.CalcHitInfo(e.ControlMousePosition);
//                if (info.InRowCell)
//                {
//                    //   int i = Convert.ToInt32(view.GetRowCellDisplayText(info.RowHandle, info.Column));
//                    GridHitInfo hitInfo = view.CalcHitInfo(e.ControlMousePosition);
//                    if (hitInfo.HitTest != GridHitTest.RowCell)
//                        return;
//                    var row = view.GetDataRow(hitInfo.RowHandle);
//                    if (row == null)
//                    {
//                        return;
//                    }
//                    GridViewInfo vinfo = (GridViewInfo)view.GetViewInfo();

//                    GridCellInfo cell = vinfo.GetGridCellInfo(hitInfo.RowHandle, hitInfo.Column);
//                    var colName = (row.Table as VDataTable).GetNameForText(hitInfo.Column.FieldName);
//                    var row_index = MainView.GetDataSourceRowIndex(hitInfo.RowHandle);// !!! проконтролировать время
//                    var rep = GetCellRepository(colName, row_index/*, true*/);// !!! проконтролировать время

//                    var shift = Cmn.GetLeftButtonsSize( colName, rep);
//                    var l = (hitInfo.HitPoint.X - cell.Bounds.Left);
//                    var iswarningToolTip = false;
//                    if ((l > shift) && (l < 15 + shift))
//                    {




//                        var msg = (row.Table as VDataTable).GetCellErrorForText(row, hitInfo.Column.FieldName);
//                        if (!string.IsNullOrEmpty(msg))
//                        {
//                            // i++;
//                            SuperToolTip toolTip = new SuperToolTip();

//                            ToolTipItem item1 = new ToolTipItem();
//                            item1.Image = Cmn.ImageWarning14;
//                            item1.Text = msg;
//                            toolTip.Items.Add(item1);
//                            e.Info = new ToolTipControlInfo(hitInfo.RowHandle.ToString() + hitInfo.Column.FieldName, null);
//                            e.Info.SuperTip = toolTip;
//                            iswarningToolTip = true;

//                        }
//                        else
//                        {

//                        }
//                    }
//                    //var lastInfo = new ToolTipControlInfo("111", "yep");//(new GridToolTipInfo(view, new CellToolTipInfo(info.RowHandle, info.Column, "Text")), i.ToString());

//                    // e.Info=lastInfo;
//                    if (!iswarningToolTip)
//                    {
//                        if ((rep is RepositoryItemHyperLinkEdit))
//                        {
//                            SuperToolTip toolTip = new SuperToolTip();

//                            ToolTipItem item1 = new ToolTipItem();

//                            item1.Text = UILink.TooltipText;
//                            toolTip.Items.Add(item1);
//                            e.Info = new ToolTipControlInfo(hitInfo.RowHandle.ToString() + hitInfo.Column.FieldName, null);
//                            e.Info.SuperTip = toolTip;
//                        }
//                    }
//                }

//            }

//        }

//        private void grReference_CanAccept(object sender, System.ComponentModel.CancelEventArgs e)
//        {
//            var table = (VDataTable)_source.Tables[MainView.Name];

//            AcceptChanges();
//            if (table.CheckValidation().Error == "")
//            {
//                var result = table.Save();
//                if (!result.Success)
//                {
//                    DismissChanges(result.RowsExceptions);
//                    _error_rows = result.RowsExceptions.Keys.ToList();

//                    // Пока так - если нет права на запись строки, то в exception пишется null
//                    if (result.RowsExceptions.Any(r => r.Value == null))
//                    {
//                        frmDataError.Show(frmDataError.ErrorType.NoWriteAccess, (table.DataSet as VDataSet).Form.GetSecurityID(),"");
//                    }
//                }
//            }
//            else
//            {
//                DismissChanges();
//            }
//        }

//        private void grReference_AfterRefreshRows(object sender, GridDataRefreshEventArgs e)
//        {
//            //_error_rows.Clear();
//            //if(_deleted_error_rows.Count > 0) grReference.SetDeleteItem(_deleted_error_rows);
//            //_deleted_error_rows.Clear();
//        }

//        //private void toolTipController1_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs e)
//        //{

//        //}

//        public void AddHasMessageHandler(Action<string> action, HasMessageHandler action2)
//        {
//            HasMessage += action;
//        }
//        public void RemoveHasMessageHandler(Action<string> action, HasMessageHandler action2)
//        {
//            HasMessage -= action;
//        }

//        public void SetParent(ucTableViewerContainer parent)
//        {
//           // this.SetMasterControl(parent as ucReferenceGrid);
//        }

//        public override void UpdatePopupMenu(XElement xmenu, BarItem[] items)
//        {
//            if (Menu == null)
//            {
//                Menu = new PopupMenu(GetBarManager());
//            }

//            Menu.BeginUpdate();
//            Menu.ClearLinks();
//            Menu.AddItems(items);
//            Menu.EndUpdate();
//        }


//        public void SetColumnVisibility(string columnName, bool value)
//        {
          
//        }
//    }
    

//}
