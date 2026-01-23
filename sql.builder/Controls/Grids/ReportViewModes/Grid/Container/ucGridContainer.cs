//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using Devart.Data.Oracle;
//using DevExpress.Data;
//using DevExpress.Utils;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Controls;
//using DevExpress.XtraBars.Utils;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Repository;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.BandedGrid.ViewInfo;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraGrid.Views.Grid.ViewInfo;
//using DevExpress.XtraPrinting;
//using DevExpress.XtraVerticalGrid;
//using infoenergo.core.Extensions;
//using sql.builder.DataApi;
//using sql.builder.DataApi.DataObjects;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
//using BandEventArgs = DevExpress.XtraGrid.Views.BandedGrid.BandEventArgs;
//using ShowButtonModeEnum = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucGridContainer :IGridContainer
//    {

//        public ucGridContainer(ControlMode mode,bool isTree)
//        {
//            //_control = new ucGridWF(mode);


//            _control = UIStatic.GetControlsfactory().CreateGridContainer(isTree);
//            _control.SetController(this);
//            createSpecialBarButtons();
//            attachSpecialBarButtonsEvents();
//            //GetControlAsWFControl().grid.Resize += new System.EventHandler(this.grid_Resize);
//           // GetControlAsWFControl().tooltip.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.tooltip_GetActiveObjectInfo);
//            //GetControlAsWFControl().ButtonSaveSettings.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonSaveSettings_ItemClick);
//            //GetControlAsWFControl().ButtonRestoreSettings.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonRestoreSettings_ItemClick);
//            //GetControlAsWFControl().ButtonUp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonUp_ItemClick);
//            //GetControlAsWFControl().ButtonDown.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonDown_ItemClick);
//            //GetControlAsWFControl().ButtonChoiceRow.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonChoiceRow_ItemClick);
//            //GetControlAsWFControl().ButtonRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonRefresh_ItemClick);
//            //GetControlAsWFControl().ButtonAddRow.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonAddRow_ItemClick);
//            //GetControlAsWFControl().ButtonDeleteRow.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonDeleteRow_ItemClick);
//            //GetControlAsWFControl().ButtonCommit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonCommit_ItemClick);
//            //GetControlAsWFControl().ButtonExportExcel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonExportExcel_ItemClick);
//            //InitializeComponent();
//            //GetControlAsWFControl().Visible = false;
//            //GetControlAsWFControl().Dock = DockStyle.Fill;
//            _mode = mode;



//            attachControlEvents();

//            //if (_mode == ControlMode.Data)
//            //{
//            //    GetControlAsWFControl().grid.ShowOnlyPredefinedDetails = true;
//            //}

//            //GetControlAsWFControl().barManager.ForceLinkCreate(); // вроде и без этого работает.
//            getSpecialBarButton(TextConst.AVGridButtonType.ChoiceRow).SetVisible(false);
//            //GetControlAsWFControl().ButtonChoiceRow.Links.First().Visible = false;
//        }

//        public void SetColumnTitle(string tableName, string columnName,string value)
//        {
//            GetControl().GetGrid().SetColumnTitle(tableName, columnName, value);
//        }
//        private void attachControlEvents()// может быть нужен detach
//        {
//            GetControl().VDisposed += OnDisposed;
//            GetControl().SelectionChanged += ucGrid_SelectionChanged;
//        }

//        void ucGrid_SelectionChanged()
//        {
//            if (_footer_visible)
//            {

//                var selection = GetControl().GetSelection();
//                decimal sum = 0;
//                int count = 0;
//                foreach (var val in selection.GetSelectedCellsValues())
//                {
//                    if (val == null || val == DBNull.Value) continue;

//                    // зачем ?
//                    //if (Cmn.IsNumeric(val))
//                    if (val is decimal)
//                    {
//                        sum += (decimal)val;
//                    }

//                    count++;
//                }

//                GetControlAsWFControl().teTotalSum.EditValue = sum;
//                GetControlAsWFControl().teTotalCount.EditValue = count;
//            }
//            MainControl.ProcessSelectionChange();
          
            

//           // GetTopTable().RaiseCurrentRowChanged();// не всегда ест смена фокуса при смене выделения
//        }

//        //private void view_SelectionChanged(object sender, SelectionChangedEventArgs args)
//        //{
//        //    if (_footer_visible)
//        //    {
//        //        GridView view = (GridView)sender;
//        //        GridCell[] cells = view.GetSelectedCells();

//        //        decimal sum = 0;
//        //        int count = 0;

//        //        for (int i = 0; i < cells.Length; i++)
//        //        {
//        //            object val = view.GetRowCellValue(cells[i].RowHandle, cells[i].Column);
//        //            if (val == null || val == DBNull.Value) continue;

//        //            if (cells[i].Column.ColumnType == XmlReports.numberType)
//        //            {
//        //                sum += (decimal)val;
//        //            }

//        //            count++;
//        //        }

//        //        GetControlAsWFControl().teTotalSum.EditValue = sum;
//        //        GetControlAsWFControl().teTotalCount.EditValue = count;
//        //    }
//        //    MainControl.ProcessSelectionChange();
//        //    // 14 03 17 Емцов - убрал, чтобы после выбора не всплывало окошко сохранить изменения
//        //    //if (_mode == ControlMode.Select)
//        //    //{
//        //    //    GetTopTable().ManualUserChangedData();
//        //    //}
//        //}

//        private void createSpecialBarButtons()
//        {
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucGridContainerWF));
//            addBarButton(TextConst.AVGridButtonType.ChoiceRow, "Выбрать", global::sql.builder.Properties.Resources.GridChoiceRow, false);
//            addBarButton(TextConst.AVGridButtonType.Refresh, "Обновить", global::sql.builder.Properties.Resources.Refresh_24, true);
//            addBarButton(TextConst.AVGridButtonType.AddRow, "Добавить строку", global::sql.builder.Properties.Resources.GridRowAdd_24, true);
//            addBarButton(TextConst.AVGridButtonType.DeleteRow, "Удалить строку", global::sql.builder.Properties.Resources.GridRowDelete_24, false);
//            addBarButton(TextConst.AVGridButtonType.Commit, "Сохранить", global::sql.builder.Properties.Resources.Commit_24, true);
//            addBarButton(TextConst.AVGridButtonType.Up, "Переместить выбранные строки выше", ((System.Drawing.Image)(resources.GetObject("ButtonUp.Glyph"))), true, false);
//            addBarButton(TextConst.AVGridButtonType.Down, "Переместить выбранные строки ниже", ((System.Drawing.Image)(resources.GetObject("ButtonDown.Glyph"))), false, false);
//            addBarButton(TextConst.AVGridButtonType.CopyToCB, "Копировать в буфер обмена", global::sql.builder.Properties.Resources.copy_16, true, false);
//            addBarButton(TextConst.AVGridButtonType.Paste, "Вставить из буфера обмена", global::sql.builder.Properties.Resources.Paste_16, true, false);

            
//            var grp = addBarMenu( "Экспорт", global::sql.builder.Properties.Resources.GridExportToFile_24, false);


//            grp.SetIsRight(true);
//            var btn = createBarButton(TextConst.AVGridButtonType.ExportExcel, "Экспорт в Excel", global::sql.builder.Properties.Resources.ExportXls_161, true);
//            grp.AddButton(btn, false);


//            grp = addBarMenu("Настройки колонок", global::sql.builder.Properties.Resources.Settings_24, false);
//            grp.SetIsRight(true);
//            btn = createBarButton(TextConst.AVGridButtonType.SaveSettings, "Сохранить настройки колонок", null);
//            grp.AddButton(btn, false);
//            btn.SetVisible(false);

//            btn = createBarButton(TextConst.AVGridButtonType.RestoreSettings, "Сбросить настройки колонок", null);
//            grp.AddButton(btn, false);
//            btn.SetVisible(true);
//            btn.SetEnabled(false);
           
//        }
//        private SortedList<string, IVBarButton> specialButtons = new SortedList<string, IVBarButton>();
//        private IVBarButton getSpecialBarButton(string buttonType)
//        {
//            if (!specialButtons.ContainsKey(buttonType))
//            {
//                return null;
//            }
//            return specialButtons[buttonType];
//        }
//        private void attachSpecialBarButtonsEvents()
//        {
//            getSpecialBarButton(TextConst.AVGridButtonType.SaveSettings).ButtonClick += this.ButtonSaveSettings_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.RestoreSettings).ButtonClick += this.ButtonRestoreSettings_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.Up).ButtonClick += this.ButtonUp_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.Down).ButtonClick += this.ButtonDown_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.ChoiceRow).ButtonClick += this.ButtonChoiceRow_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.Refresh).ButtonClick += this.ButtonRefresh_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.AddRow).ButtonClick += this.ButtonAddRow_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.DeleteRow).ButtonClick += this.ButtonDeleteRow_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.Commit).ButtonClick += this.ButtonCommit_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.ExportExcel).ButtonClick += this.ButtonExportExcel_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.Paste).ButtonClick += this.ButtonPaste_ItemClick;
//            getSpecialBarButton(TextConst.AVGridButtonType.CopyToCB).ButtonClick += CopyToCB_ButtonClick;

//        }

//        void CopyToCB_ButtonClick(object sender)
//        {
//            Clipboard.SetText(UIStatic.ClipboardDummy);
//            var tbl = GetTopTable();
//            var ds = new VDataSet();
//            copyTableStructWidthChildsToNewDS(ds, tbl);
//            UpdateDataSourceSelectedRows();
//            copyTableDataWidthChildsToNewDS(ds, tbl,true);
//            UIStatic.MyClipboard = VDataSet.ToXml(ds, EName.root);
//        }

//        private static void copyTableStructWidthChildsToNewDS(VDataSet ds, VDataTable tbl)
//        {
//            var tbl1 = Cmn.CopyTableStructure(tbl);
//            tbl1.TableName = tbl.TableName;
//            ds.Tables.Add(tbl1);
//            foreach (var ctbl in tbl.GetChildTables())
//            {
//                copyTableStructWidthChildsToNewDS( ds, ctbl);
//            }
//        }
//        private static void copyTableDataWidthChildsToNewDS(VDataSet ds, VDataTable tbl, bool isTop)
//        {
//            VDataTable tbl1 = ds.GetTable(tbl.TableName);
//            DataRow[] rows;
//            if (isTop) {
//                rows = tbl.SelectedRows.ToArray();
//            } else {
//                rows = tbl.Rows.ToArray();
//            }
//            foreach (DataRow row in rows) {
//                DataRow row1 = tbl1.NewRow();
//                foreach (VDataColumn col in tbl.Columns) {
//                    row1[col.ColumnName] = row[col];
//                }
//                tbl1.Rows.Add(row1);
//                tbl.CurrentRow = row;
//                foreach (var ctbl in tbl.GetChildTables()) {
//                    copyTableDataWidthChildsToNewDS(ds, ctbl,false);
//                }
//            }
//        }
//        private void ButtonPaste_ItemClick(object sender)
//        {
//            if (UIStatic.GetControlsfactory().GetClipboardText() == UIStatic.ClipboardDummy)
//            {
//                pasteXMLfromCB();
//            }
//            else
//            {
//                pasteTSVfromCB();
//            }
          
//        }
//        private void pasteXMLfromCB()
//        {
//            var res = ShowMessage.ShowQuestion("Добавляемые данные будут автоматически сохранены! Продолжить?");
//            if (res != DialogResult.Yes) return;

//            var newValsDS = VDataSet.FromXml(UIStatic.MyClipboard);
//            var tbl = GetTopTable();
    
           
//            pasteRowsToTable(newValsDS, tbl);
       

           
//        }

//        private static void pasteRowsToTable(VDataSet ds, VDataTable tbl, object parentKeyVal=null)
//        {
//            bool aar = tbl.AllowAsyncRefresh;
//            tbl.AllowAsyncRefresh = false;
//            VDataTable tbl1 = ds.GetTable(tbl.TableName);
//            DataRow[] rows = tbl1.Rows.ToArray();
//            if (parentKeyVal != null) {
//                rows = rows.Where(r => r[tbl.GetFKColName()].Equals(parentKeyVal)).ToArray();
//            }
//            foreach (DataRow r in rows) {
//                object pkv;
//                if (tbl.HasPrimaryKey()) {
//                    pkv = r[tbl.PrimaryKey[0]];
//                } else {
//                    pkv = DBNull.Value;
//                }
//                //r[tbl.PrimaryKey[0].ColumnName] = DBNull.Value; // так проблема с PK, получается ""
//                //tbl.AddNewRowsWithValues(new DataRow[] { r });
//                SortedList<string, object> ra = VDataTable.RowToArray(r);
//                ra.Remove(tbl.PrimaryKey[0].ColumnName);
//                if (!string.IsNullOrEmpty( tbl.GetFKColName())) {
//                    ra.Remove(tbl.GetFKColName());
//                }
//                var tblData = new List<SortedList<string, object>>();
//                tblData.Add(ra);
//                DataRow newR = tbl.AddNewRowsWithValues(tblData)[0];
//                tbl.GetDataSet().Save();
//                tbl.CurrentRow = newR;
//                tbl.RaiseCurrentRowChanged();
//                foreach (var ctbl in tbl.GetChildTables()) { // не доделано, нельзя добавлять дочерние к несохраненной записи, сейчас можно копировать 1 раз а вставку делать поочереди во все таблицы
//                    pasteRowsToTable(ds, ctbl, pkv);
//                }
//            }
//            tbl.AllowAsyncRefresh = aar;
//            //if (rows.Any())
//            //{
//            //    tbl.ManualUserChangedData();
//            //}
//        }
//        private void pasteTSVfromCB()
//        {
//            var tbl = GetTopTable();
//            var visColsNames = GetControl().GetGrid().GetVisibleColumnsNames(tbl.TableName)
//                .Where(c => !string.IsNullOrEmpty(c)).ToArray();
//            var ctext = UIStatic.GetControlsfactory().GetClipboardText();

//            var clbData = new List<List<object>>();
//            var tblData = new List<SortedList<string, object>>();
//            ctext = ctext.Replace("\n", " ");
//            while (ctext.Contains("  "))
//            {
//                ctext = ctext.Replace("  ", " ");
//            }
//            ctext = ctext.Replace("\r ", "\r");
//            ctext = ctext.Replace(" \r ", "\r");
//            ctext = ctext.Replace("\t \r", "\t\r");
//            ctext = ctext.Replace("\t\r", "\r");
//            while (ctext.Contains("\r\r"))
//            {
//                ctext = ctext.Replace("\r\r", "\r");
//            }

//            if (ctext.EndsWith("\t"))
//            {
//                ctext = ctext.Substring(0, ctext.Length - 1);
//            }

//            int i = 0;
//            foreach (var sr in ctext.Split('\r'))
//            {
//                if (sr != "")
//                {
//                    i = 0;
//                    var rd = new List<object>();
//                    foreach (var sc in sr.Split('\t'))
//                    {
//                        rd.Add(sc);
//                        i++;
//                    }
//                    clbData.Add(rd);
//                }
//            }
//            i = 0;
//            int nameColIndex = 1; // может быть не всегда так;
//            foreach (var colName in visColsNames.ToArray()) {
//                var col = tbl.GetColumn(colName);
//                if (!List.IsNullOrEmpty(col.DependantsTextSource)) {

//                    var col1 = col.DependantsTextSource[0];
//                    col1.BoundControls[0].CancelRowsLimit();
//                    col1.BoundControls[0].ReloadListData(false, false);


//                    visColsNames[i] = col1.ColumnName;
//                    var listTbl = col1.BoundControls[0].DataTableList;
//                    var idsByName = new SortedList<string, object>();
//                    var idColName = listTbl.PrimaryKey[0].ColumnName;

//                    foreach (DataRow row in listTbl.Rows)
//                    {
//                        var name = row[nameColIndex].ToString();
//                        if (!idsByName.ContainsKey(name))
//                        {
//                            idsByName.Add(name, row[idColName]);
//                        }
//                    }

//                    foreach (var sr in clbData)
//                    {
//                        object val = DBNull.Value;
//                        if (sr.Count > i)
//                        {
//                            var v = sr[i].ToString();
//                            if (idsByName.ContainsKey(v))
//                            {
//                                val = idsByName[v];
//                            }
//                            sr[i] = val;
//                        }
//                    }
//                }
//                i++;
//            }

//            foreach (var sr in clbData)
//            {
//                i = 0;
//                var rd = new SortedList<string, object>();

//                foreach (var sc in sr)
//                {
//                    var v = sc;

//                    if (v is string && v.ToString() == "")
//                    {
//                        v = null;
//                    }
//                    rd.Add(visColsNames[i], v);
//                    i++;
//                }
//                tblData.Add(rd);
//            }

//            tbl.AddNewRowsWithValues(tblData);
//            tbl.ManualUserChangedData();

//        }

//        private IVBarButton createBarButton(string buttonType, string caption, Image image, bool visible = true)
//        {
//            var btn = UIStatic.GetControlsfactory().CreateBarButton();
//            specialButtons.Add(buttonType, btn);
//            //Cmn.SetProperty(getControl(), buttonType, btn);
//            //(btn as VBarButton).Name = buttonType;
//            btn.SetCaption(caption);
//            btn.SetImage(image);

//            return btn;

//        }

//        private void addBarButton(string buttonType, string caption, Image image, bool beginGroup, bool visible = true)
//        {
//            var btn = createBarButton(buttonType, caption, image, visible);
//            GetControl().GetTopToolBar().AddBarButton(btn, beginGroup);
//            btn.SetVisible(visible);

//        }

//        private IVBarMenu addBarMenu(/*string buttonType,*/ string caption, Image image, bool beginGroup, bool visible = true)
//        {
//            var btn = UIStatic.GetControlsfactory().CreateBarMenu();
//            //Cmn.SetProperty(getControl(), buttonType, btn);
//            //(btn as VBarMenu).Name = buttonType;
//            GetControl().GetTopToolBar().AddBarButton(btn, beginGroup);
//            btn.SetCaption(caption);
//            btn.SetImage(image);
//            btn.SetVisible(visible);
//            return btn;
//        }
        

//        //private SortedList<string, IVBarButton> specialButtons = new SortedList<string, IVBarButton>();
//        //private IVBarButton GetBarButton(string buttonType)
//        //{
//        //    return specialButtons[buttonType];
//        //}
//        //private void AddBarButton(string buttonType, string caption)
//        //{
//        //    var btn = UIStatic.GetControlsfactory().CreateBarButton();
//        //    _control.GetTopToolBar().AddBarButton(btn);
//        //    btn.SetCaption(caption);
//        //    specialButtons.Add(buttonType, btn);
//        //}


//        ControlMode _mode;
//        string _top_table_name = null;
//        XElement _xscheme;

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
//                    UpdateButtonStates();
//                    UpdateGrid();
//                    AttachTopTableEvents();
//                }
                
//            }

//            if (!isWeb())
//            {
//              //  var views = grid.ViewCollection.Cast<GridView>();
//                if (_source != null)
//                {
//                    ucGridContainerWFUtils.SetComplexColumnsCaptions(this, source);
//                   // GridDesigner.SetComplexColumnsCaptions(source, views);
//                }

                

//                GetGrid().DxUpdateSummary();
//                GetGrid().DxExpandAllGroups();
//                //foreach (var view in views)
//                //{
//                //    // Чтобы формулы корректно посчитались
//                //    view.UpdateSummary();
//                //    view.ExpandAllGroups();
//                //}
//            }

//            UpdateActions(); // rowactions - устаревший вариант, новый по аналогии с редактором данных events menu
//            //GetGrid().ClearSelection();
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

//        public void SetFocusedCell(string columnName, DataRow row)
//        {
//            //GetMainView().FocusedColumn = GetMainView().Columns[columnName];
//            //GetMainView().CloseEditor();
//            //GetMainView().ShowEditor();
//            GetGrid().SetFocusedCell(columnName, row);
//        }

//        private void AttachDataSourceEvents()
//        {
//            _source.Changed += OnSourceOnChanged;
//            _source.NeedSelection += OnSourceOnNeedSelection;
//        }

//        public void DetachDataSourceEvents()
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
//            table.RefreshBegin += table_BeginRefresh;
//            table.RefreshEnd += table_EndRefresh;
//            table.DataSourceSelectionChanged += table_DataSourceSelectionChanged;
//            table.AsyncLoadStart += table_AsyncLoadStart;
//            table.AsyncLoadComplete += table_AsyncLoadComplete;
//            table.AsyncLoadCanceled += table_AsyncLoadCanceled;
//        }

//        void table_EndRefresh(object sender, EventArgs e)
//        {
//            GetGrid().DxUnlockReloadNodes();
//            GetGrid().DefaultSelection();
//        }

//        private void table_BeginRefresh(object sender, EventArgs e)
//        {
//            GetGrid().DxLockReloadNodes();
//        }




//        private string tit = "";
//        private Form frm = null;
//        private Form fndfrm()
//        {
//            frm = (GetControl() as Control).FindForm();
//            if (frm == null)
//            {
//                frm = Application.OpenForms[0];
//            }
//            return frm;
//        }
//        void table_AsyncLoadComplete(object sender, EventArgs e)
//        {
//			this.GetGrid().HideLoadingControl();
//            //frm.Text = tit;
//        }

//        void table_AsyncLoadCanceled(object sender, EventArgs e)
//        {
//			this.GetGrid().HideLoadingControl();
//            //frm.Text = tit;
//        }

//        void table_AsyncLoadStart(object sender, EventArgs e)
//        {

//			this.GetGrid().ShowLoadingControl();
//			//fndfrm();
//			//tit =frm.Text;
//			//frm.Text = "загрузка...";
           
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
//            table.RefreshBegin -= table_BeginRefresh;
//            table.RefreshEnd -= table_EndRefresh;
//            table.AsyncLoadStart -= table_AsyncLoadStart;
//            table.AsyncLoadComplete -= table_AsyncLoadComplete;
//            table.AsyncLoadCanceled -= table_AsyncLoadCanceled;
//        }

//        private void Table_OnUserChangedData(object sender, DataColumnChangeEventArgs args)
//        {
//            UpdateButtonStates();
//        }
//        private bool Table_UIEvent(object sender, UIEventArgs e)
//        {
//            return RaiseUIEvent2("", e.EventName, e.Row, e.Column);
//        }

//        private void OnSourceOnNeedSelection(object sender, EventArgs args)
//        {
//            UpdateDataSourceSelectedRows();
//        }

//        void Table_TableRefreshed(object sender, EventArgs args)
//        {
//            UpdateButtonStates();
//            GetGrid().ExpandAllNodes();
//        }

//        void Table_TableCommited(object sender, EventArgs args)
//        {
//            //
//        }

//        private void OnSourceOnChanged(object sender, EventArgs args)
//        {
//            if (isWeb()) return;
//            GetGrid().DxSetEditingValEqFocusedVal();
//        }

//        public void UpdateDataSourceSelectedRows()
//        {
//            if (isWeb()) return;
//            //GridView view = GetMainView();
//            //var list =  view.GetSelectedRows().Select(view.GetDataRow).ToList();

//            MainControl.SetDataSourceSelectedRows(GetGrid().GetSelectedRows().ToList());
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
//        #region OrderedMode
//        private string _order_field_name;

//        public void SetOrderFieldName(string order_field_name)
//        {
//            if (order_field_name == null) return;

//            _order_field_name = order_field_name;

//            getSpecialBarButton(TextConst.AVGridButtonType.Up).SetVisible(true);
//            getSpecialBarButton(TextConst.AVGridButtonType.Down).SetVisible(true);
//             //GetControlAsWFControl(). ButtonUp.Visibility = BarItemVisibility.Always;
//             //GetControlAsWFControl().ButtonDown.Visibility = BarItemVisibility.Always;

//            if (_source != null) UpdateOrderedMode();
//        }
//        string _parent_field_name = null;
//        public bool IsTree()
//        {
//            return _parent_field_name != null;
//        }
//        public void SetParentFieldName(string parent_field_name)
//        {
//            _parent_field_name = parent_field_name;
//            //if (_mode != ControlMode.Report) tree.ParentFieldName = parent_field_name;
//            GetGrid().SetParentFieldName(parent_field_name);
//            UpdateTree_KeyFieldName();
//        }

//        public void SetExpandAllNodes(bool expand_all_nodes)
//        {
//            //_expand_all_nodes = expand_all_nodes;
//            //if (expand_all_nodes && _source != null)
//            //{
//            //    tree.ForceInitialize();
//            //    tree.ExpandAll();
//            //}
//        }

//        //private void UpdateOrderedMode()
//        //{
//        //    var view = GetMainView();
//        //    foreach (var c in view.Columns.Where(c => c.Name != GridDesigner.GetDummyColumnName() && c.FieldName != _order_field_name))
//        //    {
//        //        c.OptionsColumn.AllowSort = DefaultBoolean.False;
//        //    }

//        //    GridColumn col = view.Columns[_order_field_name];
//        //    col.SortOrder = ColumnSortOrder.Ascending;
//        //    col.SortIndex = 0;
//        //}

//        private void UpdateOrderedMode()
//        {
//            var gr = GetGrid();
//            foreach (var c in gr.GetColumns()/*.Where(c => gr.GetColumnName(c) != GridDesigner.GetDummyColumnName()  && gr.GetColumnFieldName(c) != _order_field_name)*/)
//            {
//                gr.SetColumnAllowSort(c,false);
              
//            }
//            var col=gr.GetColumnByFieldName(_order_field_name);
//            gr.SetColumnSortOrder(col, "ascending");
//            gr.SetColumnSortIndex(col, 0);
        
//        }
//        private List<OrderedBlock> GetSelectionBlocks()
//        {
//           // var view = GetMainView();
//            var blocks = new List<OrderedBlock>();
//            OrderedBlock block = null;
//            int rowLastIndex = -1;

//            //var selected_rows = view.GetSelectedRows()
//            //    .Select(i => new Tuple<DataRow, int>(view.GetDataRow(i), i))
//            //    .OrderBy(t => t.Item1[_order_field_name]);

//            var gr = GetGrid();
//            var selected_rows = gr.GetSelectedRowsHandles()
//                .Select(i => new Tuple<DataRow, int>(gr.GetRowByHandle(i), i))
//                .OrderBy(t => t.Item1[_order_field_name]);

//            foreach (Tuple<DataRow, int> rowCurrent in selected_rows)
//            {
//                if (rowLastIndex == -1 || rowCurrent.Item2 > rowLastIndex + 1)
//                {
//                    if (block != null && rowLastIndex + 1 < gr.GetRowsCount())
//                    {
//                        block.Next = gr.GetRowByHandle(rowLastIndex + 1);
//                    }

//                    block = new OrderedBlock();
//                    blocks.Add(block);

//                    if (rowCurrent.Item2 - 1 >= 0)
//                    {
//                        block.Prev =gr.GetRowByHandle(rowCurrent.Item2 - 1);
//                    }
//                }

//                block.Rows.Add(rowCurrent.Item1);

//                rowLastIndex = rowCurrent.Item2;
//            }

//            if (block != null && rowLastIndex + 1 < gr.GetRowsCount())
//            {
//                block.Next = gr.GetRowByHandle(rowLastIndex + 1);
//            }

//            return blocks;
//        }
//        private void MoveBlocks(List<OrderedBlock> blocks, string direction)
//        {
//            foreach (OrderedBlock block in blocks)
//            {
//                if (direction == "up")
//                {
//                    // некуда двигать
//                    if (block.Prev == null) continue;

//                    object orderLast = null;
//                    for (int i = block.Rows.Count - 1; i >= 0; i--)
//                    {
//                        if (orderLast == null) orderLast = block.Rows[i][_order_field_name];

//                        block.Rows[i][_order_field_name] = (i - 1 >= 0)
//                            ? block.Rows[i - 1][_order_field_name]
//                            : block.Prev[_order_field_name];
//                    }

//                    block.Prev[_order_field_name] = orderLast;
//                }
//                else if (direction == "down")
//                {
//                    // некуда двигать
//                    if (block.Next == null) continue;

//                    object orderFirst = null;
//                    for (int i = 0; i < block.Rows.Count; i++)
//                    {
//                        if (orderFirst == null) orderFirst = block.Rows[i][_order_field_name];

//                        block.Rows[i][_order_field_name] = (i + 1 < block.Rows.Count)
//                            ? block.Rows[i + 1][_order_field_name]
//                            : block.Next[_order_field_name];
//                    }

//                    block.Next[_order_field_name] = orderFirst;
//                }
//            }
//        }

//        private void MoveRowsDown(DataRow row_first)
//        {
//            if (row_first == null) return;

//            DataRow[] rows = GetTopTable().AsEnumerable()
//                .OrderBy(r => r[_order_field_name])
//                .SkipWhile(r => r != row_first)
//                .ToArray();

//            for (int i = 0; i < rows.Length; i++)
//            {
//                if (i != rows.Length - 1)
//                {
//                    rows[i][_order_field_name] = rows[i + 1][_order_field_name];
//                }
//                else
//                {
//                    rows[i][_order_field_name] = (decimal)rows[i][_order_field_name] + 1;
//                }
//            }
//        }

//        /// <summary>
//        /// Приходится использовать вместе с BeginSort(), чтобы дерево не перестраивалось в процессе изменения поля order
//        /// При определенных условиях без этих блокировок можно получить дублированое значение order у соседних узлов
//        /// https://www.devexpress.com/Support/Center/Question/Details/Q240528
//        /// </summary>
//        System.Reflection.FieldInfo _lockSortField;
//        void PreventViewRebuild()
//        {
//            //if (_lockSortField == null)
//            //{
//            //    _lockSortField = typeof(TreeList).GetField("lockSort", BindingFlags.Instance | BindingFlags.NonPublic);
//            //}

//            //int lockSort = (int)_lockSortField.GetValue(tree);
//            //_lockSortField.SetValue(tree, ++lockSort);
//        }
//        void ResumeViewRebuild()
//        {
//            //int lockSort = (int)_lockSortField.GetValue(tree);
//            //_lockSortField.SetValue(tree, --lockSort);
//        }

//        //private void ButtonUp_ItemClick()
//        //{
//        //    var view = GetMainView();

//        //    if (view.SelectedRowsCount == 0) return;

//        //    view.BeginUpdate();
//        //    view.BeginSort();
//        //    PreventViewRebuild();

//        //    // разделяем выделенные строки на блоки последовательно идущих строк, которые будут двигаться вместе
//        //    List<OrderedBlock> blocks = GetSelectionBlocks();
//        //    // двигаем блоки
//        //    MoveBlocks(blocks, "up");

//        //    ResumeViewRebuild();
//        //    view.EndSort();
//        //    view.EndUpdate();
//        //}
//        //private void ButtonDown_ItemClick()
//        //{
//        //    var view = GetMainView();

//        //    if (view.SelectedRowsCount == 0) return;

//        //    view.BeginUpdate();
//        //    view.BeginSort();
//        //    PreventViewRebuild();

//        //    // разделяем выделенные строки на блоки последовательно идущих строк, которые будут двигаться вместе
//        //    List<OrderedBlock> blocks = GetSelectionBlocks();
//        //    // двигаем блоки
//        //    MoveBlocks(blocks, "down");

//        //    ResumeViewRebuild();
//        //    view.EndSort();
//        //    view.EndUpdate();
//        //}

//        private void ButtonUp_ItemClick(object sender)
//        {
//            var gr = GetGrid();

//            if (gr.GetSelectedRows().Length == 0) return;

//            gr.BeginSort();
//            PreventViewRebuild();

//            // разделяем выделенные строки на блоки последовательно идущих строк, которые будут двигаться вместе
//            List<OrderedBlock> blocks = GetSelectionBlocks();
//            // двигаем блоки
//            MoveBlocks(blocks, "up");

//            ResumeViewRebuild();
//            gr.EndSort();
//        }
//        private void ButtonDown_ItemClick(object sender)
//        {
//            var gr = GetGrid();

//            if (gr.GetSelectedRows().Length == 0) return;

//            gr.BeginSort();
//            PreventViewRebuild();

//            // разделяем выделенные строки на блоки последовательно идущих строк, которые будут двигаться вместе
//            List<OrderedBlock> blocks = GetSelectionBlocks();
//            // двигаем блоки
//            MoveBlocks(blocks, "down");

//            ResumeViewRebuild();
//            gr.EndSort();
//        }
//        #endregion

//        RepositoryManager _rm=null;

//        public RepositoryManager repositories()
//        {
//            if (_rm == null)
//            {
//                if (GetGrid() is ucGridWF)
//                {
//                    _rm = new RepositoryManager(grid_temp);
//                }
//                else
//                {
//                    _rm = new RepositoryManager(tree);
//                }
//            }
//            return _rm;
//        }

//        public bool IsCompareMode { get;  set; }
//        public bool ShowChildTabs { get; private set; }

//        private bool _toolbar_top_visible = true;
//        public void SetTopToolbarVisible(bool visible)
//        {
//            _toolbar_top_visible = visible;
//            GetControl().SetTopToolBarVisible(visible);
//            //GetControlAsWFControl().barTopToolbar.Visible = visible;
//        }

//        private bool _toolbar_bottom_visible = false;
//        public void SetBottomToolbarVisible(bool visible)
//        {
//            _toolbar_bottom_visible = visible;
//            GetControl().SetBottomToolBarVisible(visible);
//            //GetControlAsWFControl().barTopToolbar.Visible = visible;
//        }

//        private bool _footer_visible = true;
//        public void SetFooterVisible(bool visible)
//        {
//            _footer_visible = visible;
//            GetControl().SetFooterVisible(visible);
//            //GetControlAsWFControl().barFooter.Visible = visible;
//        }

//        private bool _summary_visible = true;
//        public void SetSummaryVisible(bool visible)
//        {
          
//            _summary_visible = visible;
//            GetControl().SetSummaryVisible(visible);
//            //foreach (GridView v in grid.ViewCollection.Cast<GridView>())
//            //{
//            //    v.OptionsView.ShowFooter = _summary_visible;
//            //}
//        }
//        private bool _filt_row_visible = false;
//        public void SetFilterRowVisible(bool visible)
//        {
//            _filt_row_visible = visible;
//            GetGrid().SetShowAutoFilterRow(visible);
           
//        }
//		private bool _allowSelectMoveColumns = false;
//		private bool _manualColumnsConfigurationLoaded = false;
//		public void SetAllowSelectMoveColumns(bool value)
//		{
//			_allowSelectMoveColumns = value;
//			GetGrid().SetAllowSelectMoveColumns(value);
//			if (_allowSelectMoveColumns) getSpecialBarButton(TextConst.AVGridButtonType.SaveSettings).SetVisible(true);
//		}
//        private bool _multiselect = true;
//        public void SetMultiselect(bool multiselect)
//        {
//            _multiselect = multiselect;
//            GetControl().SetMultiselect(multiselect);
//            //foreach (GridView v in grid.ViewCollection.Cast<GridView>())
//            //{
//            //    v.OptionsSelection.MultiSelect = _multiselect;
//            //}
//        }

//        public void SetMultiselectMode(bool isCell)
//        {
//            GetControl().SetMultiselectMode(isCell);
//        }

//        private IucGridContainer _control = null;

//        public IucGridContainer GetControl()
//        {
//            return _control;
//        }

//        public ucGridContainerWF GetControlAsWFControl()
//        {
//            return (ucGridContainerWF)_control;
//        }

//        //public ucGridWF GetControlAsWFControl1()
//        //{
//        //    return (ucGridWF)_control;
//        //}

     
//        //private GridView GetMainView()
//        //{
//        //    return GetControlAsWFControl().GetGridAsGridControl().MainView as GridView;
//        //}

//        public void BeginUpdate()
//        {

//            DetachViewEvents();
//        }
//        public void EndUpdate()
//        {
//            AttachViewEvents();
//            UpdateGridViews();
//            GetControl().GetGrid().EndUpdateData();
            
//        }

//        private void AttachViewEvents()
//        {
//            GetControl().AttachViewEvents();
//            MainControl.SetAllowMerge();
//            AttachViewEventsNew();
//            if (isWeb()) return;
//            if (GetGrid() is GridControl)
//            {
//                AttachViewEvents_Grid();
//            }
//            else
//            {
//                AttachViewEvents_Tree();
//            }
//        }
//        private void DetachViewEvents()
//        {
//            GetControl().DetachViewEvents();
//            DetachViewEventsNew();
//            if (GetGrid() is GridControl)
//            {
//                DetachViewEvents_Grid();
//            }
//            else
//            {
//                DettachViewEvents_Tree();
//            }
//        }

//        private void UpdateGridViews()
//        {
           
//            if (isWeb()) return;

//            int imode = -1;
//            if (_mode == ControlMode.Report)
//            {
//                imode = 0;
//            }
//            else if (_mode == ControlMode.Data)
//            {
//                imode = 1;
//            }
//            else if (_mode == ControlMode.Select)
//            {
//                imode = 2;
//            }
//            GetGrid().SetSelectionMode(imode);
//            GetGrid().SetMultiSelect(_multiselect);
//            GetGrid().SetShowFooter(_summary_visible);
//            //foreach (GridView view in grid.ViewCollection)
//            //{
//            //    //if (_mode == ControlMode.Report)
//            //    //{
//            //    //    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
//            //    //}
//            //    //else if (_mode == ControlMode.Data)
//            //    //{
//            //    //    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
//            //    //    view.OptionsView.ShowButtonMode = ShowButtonModeEnum.ShowAlways;
//            //    //    view.OptionsNavigation.AutoFocusNewRow = true;
//            //    //    view.OptionsBehavior.Editable = true;
//            //    //    view.OptionsBehavior.ReadOnly = false;
//            //    //}
//            //    //else if (_mode == ControlMode.Select)
//            //    //{
//            //    //    view.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
//            //    //    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
//            //    //}

//            //    //view.OptionsSelection.MultiSelect = _multiselect;
//            //    //view.OptionsView.ShowFooter = _summary_visible;
//            //    //view.UpdateTotalSummary();

//            //    //UpdateColumnsPanelHeight(view);
//            //    //GetControl().ApplyHeaderLayout(view.Name);
//            //    //UpdateDummyColumnWidth(view);
//            //}
//            foreach (var view in GetGrid().GetViews())
//            {

//                UpdateColumnsPanelHeight(view);
//                GetControl().ApplyHeaderLayout(GetGrid().GetViewName(view));
//            }
//        }

//        private void UpdateColumnsPanelHeight(object view)
//        {
//            GetGrid().FitHeaderHeight(view);
//            //if (view is BandedGridView) GridDesigner.GridColumnsBestHeight(view);
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

//                UpdateGrid();
//                UpdateButtonStates();
//            }

//        }

//        private void setFetchAll()
//        {
//            if (_source != null && _top_table_name != null)
//            {
//                (_source.Tables[_top_table_name] as VDataTable).UseDeferredFetch = false;
//            }


//        }
//        public void UpdateGrid()
//        {
//            //grid.BeginUpdate();
//            var grd=GetGrid();
//            if (!isWeb())
//            {
//                bool allowDetail = (_mode == ControlMode.Report);
//               // GridDesigner.SetGridTopTable(grid, _source.Tables[_top_table_name]);
//                grd.SetGridTopTable(_source.Tables[_top_table_name]);
//               grd.SetShowDetailTabs(allowDetail);
//               grd.SetEnableMasterViewMode(allowDetail);
//                //foreach (var v in grid.ViewCollection.Cast<GridView>())
//                //{
//                //    v.OptionsDetail.ShowDetailTabs = ShowChildTabs;
//                //    v.OptionsDetail.EnableMasterViewMode = allowDetail;
//                //}
//            }
//            UpdateTree_KeyFieldName();
//          var tbl=_source.Tables[_top_table_name];
//            IVTableDataAdapter vtda = new sql.builder.DataApi.TableDataAccessor.Adapters.VTDADataTable(tbl);
//           grd.SetDataSource(vtda);
//            grd.ExpandAllNodes();
//            //grid.DataSource = _source.Tables[_top_table_name];
//            if (!isWeb())
//            {
//                //grid.ForceInitialize();
//               grd.ControlForceInitialize();
//                if (_order_field_name != null) UpdateOrderedMode();
//            }
//			if (!_manualColumnsConfigurationLoaded)
//			{
//				foreach (var col in GetGrid().GetColumns())
//				{
//					var fldName = grd.GetColumnFieldName(col);
//					var dataCol = (tbl as VDataTable).GetColumn(fldName);
//					if (dataCol != null)
//					{

//						var vis = dataCol.GetVisibility(null, false, true);
//						if (!vis)
//						{
//							grd.SetColumnVisible(col, false);
//						}
//					}
//				}
//			}
//            //grid.EndUpdate();
//        }
//        private void UpdateTree_KeyFieldName()
//        {
//            // старый вариант из ReportGrid

//            //var node_id = GetTopTable().Columnsю.FirstOrDefault(col => col.Name == "node-id");
//            var table = GetTopTable();
//            if (table == null) return;
//            // вариант для Data
//            var keyFieldName = "";
//            if (table.HasPrimaryKey()) {
//                keyFieldName = table.PrimaryKey[0].ColumnName;
//                GetGrid().SetKeyFieldName(keyFieldName);
//            }
          
//            if (_parent_field_name != null)
//            {
               

//                var col = GetGrid().GetColumnByFieldName(_parent_field_name);// костыль, раньше такие колонки вообще не создавались в TreeList - опция по умолчанию DX
//                if (col != null)
//                {
//                    GetGrid().SetColumnVisible(col, false);
//                }
//                col = GetGrid().GetColumnByFieldName(keyFieldName);
//                if (col != null)
//                {
//                    GetGrid().SetColumnVisible(col, false);
//                }

//                setFetchAll();
//            }
//            //if (node_id == null && table.PrimaryKey != null && table.PrimaryKey.Any()) node_id = _views[_top_table_name].Columns.FirstOrDefault(col => col.FieldName == table.PrimaryKey[0].ColumnName);
//            //if (node_id != null) tree.KeyFieldName = node_id.FieldName;

//            //var node_pid = _views[_top_table_name].Columns.FirstOrDefault(col => col.Name == "parent-node-id");
//            //if (node_pid == null && _parent_field_name != null) node_pid = _views[_top_table_name].Columns.FirstOrDefault(col => col.FieldName == _parent_field_name);
//            //if (node_pid != null) tree.ParentFieldName = node_pid.FieldName;

//        }

//        //private void UpdateTree()
//        //{
//        //    tree.BeginUpdate();

//        //    tree.Bands.Clear();
//        //    tree.Columns.Clear();

//        //    var ti = _views[_top_table_name];
//        //    tree.Bands.AddRange(ti.Bands.ToArray());
//        //    tree.Columns.AddRange(ti.Columns.ToArray());
//        //    tree.OptionsView.ShowBandsMode = ti.ShowBands ? DefaultBoolean.True : DefaultBoolean.False;
//        //    tree.Caption = ti.Title;

//        //    var table = GetTopTable();

//        //    // старый вариант из ReportGrid
//        //    var node_id = _views[_top_table_name].Columns.FirstOrDefault(col => col.Name == "node-id");
//        //    // вариант для Data
//        //    if (node_id == null && table.PrimaryKey != null && table.PrimaryKey.Any()) node_id = _views[_top_table_name].Columns.FirstOrDefault(col => col.FieldName == table.PrimaryKey[0].ColumnName);
//        //    if (node_id != null) tree.KeyFieldName = node_id.FieldName;

//        //    var node_pid = _views[_top_table_name].Columns.FirstOrDefault(col => col.Name == "parent-node-id");
//        //    if (node_pid == null && _parent_field_name != null) node_pid = _views[_top_table_name].Columns.FirstOrDefault(col => col.FieldName == _parent_field_name);
//        //    if (node_pid != null) tree.ParentFieldName = node_pid.FieldName;

//        //    tree.EndUpdate();

//        //    tree.DataSource = table;

//        //    if (_expand_all_nodes)
//        //    {
//        //        tree.ForceInitialize();
//        //        tree.ExpandAll();
//        //    }

//        //    if (_order_field_name != null) UpdateOrderedMode();
//        //    UpdateColumnsPanelHeight();
//        //}


//        //private void UpdateButtonStates()
//        //{
//        //    var table = GetTopTable();

//        //    // если родительская строка добавлена, но не сохранена либо вообще отсутствует - дочерние редактировать нельзя
//        //    bool no_parent = table.ParentRelations.Cast<DataRelation>()
//        //        .Select(rel => (rel.ParentTable as VDataTable).CurrentRow)
//        //        .Any(r => r == null || r.RowState == DataRowState.Added);

//        //    if (!isWeb())
//        //    {
//        //        var bc = ((IDockableObject)GetControlAsWFControl().barTopToolbar).BarControl;
//        //        if (bc == null) return;

//        //        if (no_parent)
//        //        {
//        //            ((DockedBarControl)bc).Enabled = false;
//        //        }
//        //        else
//        //        {
//        //            ((DockedBarControl)bc).Enabled = true;
//        //            getSpecialBarButton(TextConst.AVGridButtonType.Commit).SetEnabled(table.HasUserChanges);
//        //            //GetControlAsWFControl().ButtonCommit.Enabled = table.HasUserChanges;
//        //        }

//        //    }
//        //}



//        private void UpdateButtonStates()
//        {
//            if (isWeb()) return;
//            var table = GetTopTable();

//            // если родительская строка добавлена, но не сохранена либо вообще отсутствует - дочерние редактировать нельзя
//            bool no_parent = table.ParentRelations.Cast<DataRelation>()
//                .Select(rel => (rel.ParentTable as VDataTable).CurrentRow)
//                .Any(r => r == null || r.RowState == DataRowState.Added);


//            var bc = ((IDockableObject)GetControlAsWFControl().barTopToolbar).BarControl;
//            if (bc == null) return;

//            if (no_parent)
//            {
               
//                //((DockedBarControl)bc).Enabled = false; 
//                // убрал т.к. иногда нужна возможность работы с кнопками, сделать опционально при необходимости, пока ставлю enabled=true(в холостую) на кнопках к которым нужен доступ при no_parent
//            }
//            else
//            {
//               // ((DockedBarControl)bc).Enabled = true;
             
//                getSpecialBarButton(TextConst.AVGridButtonType.Commit).SetEnabled(table.HasUserChanges);
//                //GetControlAsWFControl().ButtonCommit.Enabled = table.HasUserChanges;
//            }
//        }
//        //private void grid_Resize(object sender, EventArgs e)
//        //{
//        //    foreach (var bview in grid.ViewCollection.OfType<BandedGridView>())
//        //    {
//        //        UpdateDummyColumnWidth(bview);
//        //    }
//        //}
//        //public void UpdateDummyColumnWidth(GridView view)
//        //{
//        //    GridColumn colDummy = view.Columns.ColumnByName(GridDesigner.GetDummyColumnName());
//        //    if (colDummy == null) return;

//        //    GridViewInfo info = view.GetViewInfo() as GridViewInfo;
//        //    int width_free = info.ViewRects.ColumnPanelWidth - info.ViewRects.ColumnTotalWidth + colDummy.Width - 20;// 20 чтобы не глючило при порявлении скрола
//        //    int width_min = GridDesigner.GetMinDummyWidth();

//        //    colDummy.Width = (width_free < width_min) ? width_min : width_free;
//        //}
//        void CustomLastColumnResize(BandedGridView bview, Point cords)
//        {
//            // чтобы изменение ширины последней колонки не растягивало все остальные в бэнде
//            BandedGridHitInfo hitInfo = bview.CalcHitInfo(cords);
//            if (hitInfo.HitTest == BandedGridHitTest.ColumnEdge)
//            {
//                GridBand ownerBand = hitInfo.Column.OwnerBand;
//                if (ownerBand.Columns.VisibleColumnCount - 1 == hitInfo.Column.ColVIndex)
//                {
//                    foreach (GridBand band in bview.Bands)
//                    {
//                        band.OptionsBand.FixedWidth = (band != ownerBand);
//                    }
//                    foreach (BandedGridColumn col in ownerBand.Columns)
//                    {
//                        if (col != hitInfo.Column)
//                        {
//                            if (col.VisibleIndex != ownerBand.Columns.Count)
//                            {
//                                col.OptionsColumn.FixedWidth = true;
//                            }
//                        }
//                        else
//                        {
//                            col.OptionsColumn.FixedWidth = false;
//                        }
//                    }
//                }
//                else
//                {
//                    foreach (GridBand band in bview.Bands)
//                    {
//                        band.OptionsBand.FixedWidth = false;
//                    }
//                    foreach (BandedGridColumn col in ownerBand.Columns)
//                    {
//                        col.OptionsColumn.FixedWidth = false;
//                    }
//                }
//            }
//            else if (hitInfo.HitTest == BandedGridHitTest.BandEdge)
//            {
//                GridBand ownerBand = hitInfo.Band;
//                foreach (BandedGridColumn col in ownerBand.Columns)
//                {
//                    col.OptionsColumn.FixedWidth = false;
//                }
//            }
//        }


     
//        private bool isWeb()
//        {
//            return UIStatic.IsWeb();
//        }
    

      

//        private DevExpress.XtraGrid.GridControl grid_temp
//        {
//            get
//            {
//                return (ucGridWF)GetGrid();
//            }
//        }

//        private DevExpress.XtraTreeList.TreeList tree
//        {
//            get
//            {
//                return (ucTreeWF)GetGrid();
//            }
//        }



//        public IucGrid GetGrid()
//        {
//            return GetControl().GetGrid();
//        }
//        public ucTableViewerContainer MainControl = null;
       
     
//        public void ApplyHeaderLayout()
//        {
//            GetControl().ApplyHeaderLayout(null);
//        }



//        private void UpdateActions()
//        {
//            //if (isWeb()) return;
//            if (_menu == null)
//            {

//                //_menu = new PopupMenu(GetControlAsWFControl().barManager);
//                _menu = UIStatic.GetControlsfactory().CreatePopupMenu();
//                if (!UIStatic.IsWeb())
//                {
//                    (_menu as sql.builder.UI.WinForms.VPopupMenu).SetBarManager(GetControlAsWFControl().barManager);
//                }
          
//            }

//            if (_source != null && _source.Report != null)
//            {
//                var actions = _source.Report.RowActions().Where(a => a.Attribute(TextConst.AName.CallType).Value == "popupmenu");
//                if (!actions.Any()) return;

//                //_menu.BeginUpdate();
//                _menu.Clear();

//                foreach (var action in actions)
//                {
//                    //var btn = new BarButtonItem(GetControlAsWFControl().barManager, action.Action().Attribute(TextConst.AName.Title).Value)
//                    //{
//                    //    Tag = action
//                    //};
//                    var btn = UIStatic.GetControlsfactory().CreateBarButton();
//                    btn.SetCaption(action.Action().Attribute(TextConst.AName.Title).Value);
//                    btn.Tag = action;
//                    btn.ButtonClick += btnActionMenu_ItemClick;
//                    _menu.AddButton(btn,false);
//                }

//                //_menu.EndUpdate();
//            }

//            if (XmlReports.IsDeveloperMode()) AddDebugButtons();
//        }
//        private void btnActionMenu_ItemClick(object sender)
//        {
//            var btn = (sender as IVTagControl);
//            if (_source == null || _source.Report == null) return;

//            //var row = ((GridView)grid.FocusedView).GetFocusedDataRow();
//            var row = GetGrid().GetFocusetDataRow();
//            if (row == null) return;

//            ((VUseAction)btn.Tag).Execute((VDataSet)row.Table.DataSet, null, (row.Table as VDataTable), row, null);
//        }

   
//        public void ExportToXlsx(string fullpath, string caption = null)
//        {
//          //  var wi1=Wait.ShowCursor(true);
//            var wi1 = Wait.ShowPanel("Экспорт данных", true,3000);
//            try
//            {
//                if (GetTopTable().UseDeferredFetch)
//                {
//                    GetTopTable().FetchTo(int.MaxValue);
//                }
//                GetGrid().ExportToXlsx(fullpath, IsDxExport, caption);
//            }
//            finally
//            {
//                Wait.Hide(wi1);
//            }
            
          
          
//            //var view = GetMainView();

//            //view.BeginUpdate();

//            //var table_name = view.ViewCaption;
//            //if (!string.IsNullOrEmpty(caption))
//            //{
//            //    view.ViewCaption = caption;
//            //    view.OptionsView.ShowViewCaption = true;
//            //}

//            //GridColumn colDummy = view.Columns.FirstOrDefault(c => c.Name == GridDesigner.GetDummyColumnName());
//            //GridBand bandDummy = null;
//            //if (colDummy != null)
//            //{

//            //    if (colDummy is BandedGridColumn)
//            //    {
//            //        bandDummy = (colDummy as BandedGridColumn).OwnerBand;
//            //    }
//            //    colDummy.Visible = false;
//            //}
//            //if (bandDummy != null)
//            //{
//            //    bandDummy.Visible = false;
//            //}


//            //DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;

//            //var options = new XlsxExportOptions();
//            //grid.ExportToXlsx(fullpath, options);

//            //view.OptionsView.ShowViewCaption = false;
//            //view.ViewCaption = table_name;
//            //if (colDummy != null) colDummy.Visible = true;
//            //if (bandDummy != null)
//            //{
//            //    bandDummy.Visible = true;
//            //}
//            //view.EndUpdate();
//        }

//        //public void CompareDataSets(VDataSet compared_ds)
//        //{


//        //    if (_source == null) return;

//        //    BeginUpdate();

//        //    DetachDataSourceEvents();

//        //    var dict = new Dictionary<string, string[]>();
//        //    foreach (DataTable table in _source.Tables)
//        //    {
//        //        dict.Add(table.TableName, table.PrimaryKey.Select(col => col.ColumnName).ToArray());
//        //    }

//        //    // заплатка для сравенения через WCF
//        //    string caption2 = null;
//        //    if (compared_ds.Scheme != null)
//        //    {
//        //        caption2 = compared_ds.Scheme.Attribute("timestamp").Value;
//        //    }
//        //    else caption2 = "***";

//        //    GridDesigner.SetComparedGridView(grid, dict,
//        //        _source.Scheme.Attribute("timestamp").Value,
//        //        caption2);

//        //    _source = GridDesigner.CompareDataSets(_source, compared_ds);
//        //    grid.DataSource = GetTopTable();

//        //    // выводим колличество различий
//        //    var divergences_count = 0;
//        //    foreach (GridView view in grid.ViewCollection)
//        //    {
//        //        var result_columns = view.Columns.Where(col => col.FieldName.EndsWith("_result") && !String.IsNullOrEmpty(col.UnboundExpression));
//        //        for (int i = 0; i < view.RowCount; i++)
//        //        {
//        //            divergences_count += result_columns.Count(result_column => (decimal)view.GetRowCellValue(i, result_column.FieldName) != 0M);
//        //        }
//        //    }

//        //    SetFooterVisible(true);
//        //    GetControlAsWFControl().lCompareResult.Visibility = BarItemVisibility.Always;
//        //    GetControlAsWFControl().lCompareResult.Caption = "Расхождений всего: " + divergences_count;

//        //    EndUpdate();

//        //    IsCompareMode = true;
//        //}

//        void SelectCells(GridColumn column)
//        {
//            var view = column.View as GridView;
//            if (view == null) return;

//            for (int i = 0; i < column.View.RowCount; i++)
//            {
//                view.SelectCell(i, column);
//            }
//        }
//        void SelectCells(GridBand band)
//        {
//            foreach (BandedGridColumn column in band.Columns)
//            {
//                SelectCells(column);
//            }
//        }

//        public void LoadFromXml(XElement xscheme, bool update = false, bool force=false)
//        {
//            if (_xscheme == xscheme && !force) return;
//            _xscheme = xscheme;

//            // с bandedgridview не отображается колонка с чекбоксами в режиме GridMultiSelectMode.CheckBoxRowSelect Q570331
//            bool banded = (_mode != ControlMode.Select);
//            //bool banded = true;
//            //Parser.LoadGridSettingsFromXml(xscheme.Parent, grid, banded);
//            Parser.LoadGridSettingsFromXml(xscheme, GetControl().GetGrid(), banded);
//            if (!isWeb())
//            {
//                GetGrid().ControlForceInitialize();
//               // grid.ForceInitialize();
//            }

//            if (update) UpdateGrid();

//            if (!isWeb())
//            {
//                LoadSettingsFromRegistry();
//            }
//        }
//        public void SaveToXml(XElement xscheme)
//        {
//            // отображать закладки для дочерних таблиц
//            xscheme.SetAttributeValue("child_tabs", ShowChildTabs ? "1" : "0");
//            if (GetGrid() is ucGridWF)
//            {
//                Parser.SaveGridSettingsToXml(xscheme, GetGrid() as ucGridWF);
//            }
//            //else
//            //{
//            //    Parser.SaveTreeSettingsToXml(xscheme, GetGrid() as ucTreeWF);
//            //}
//        }

//        public void SetTitle(string title)
//        {
//            GetControl().SetTitle(title);
//            //var view = grid.MainView as GridView;
//            //view.ViewCaption = title;
//        }

//        public void UpdateToolbar(XElement xtoolbar, IVBarItem[] items, bool bottom = false)
//        {
//            if (xtoolbar == null || items == null ) return;

//            var toolbar = (bottom) 
//                ? GetControl().GetBottomToolBar()
//                : GetControl().GetTopToolBar();

//           //( toolbar as sql.builder.UI.WinForms.VBar).BeginUpdate();

//            if (Cmn.GetAttrValue(xtoolbar, TextConst.AName.ColumnVisible) == TextConst.AVBool.False)
//            {
//                //foreach (BarItemLink link in toolbar.ItemLinks)
//                //{
                  
//                //    link.Item.Visibility = BarItemVisibility.Never;
                    
//                //}

//                toolbar.HideAllItems();
               
//            }

//            // настраиваем дефолтные кнопки
//            foreach (var xcmd in xtoolbar.Elements(TextConst.EName.UICommand).Where(xcmd => xcmd.Attribute(TextConst.AName.ControlName) != null))
//            {
//                var visible = (Cmn.GetAttrValue(xcmd, TextConst.AName.ColumnVisible) == TextConst.AVBool.True);
//                SetToolbarButtonVisible(xcmd.Attribute(TextConst.AName.ControlName).Value, visible);
//            }
//            foreach (var item in items)
//            {
//                toolbar.AddBarButton(item,false);
//                item.SetVisible(true);
//            }
//            //toolbar.AddItems(items);
//           // (toolbar as sql.builder.UI.WinForms.VBar).EndUpdate();
//        }
//        public void SetToolbarButtonVisible(string name, bool visible)
//        {
//           var btn = getSpecialBarButton(name);

//           if (btn != null)
//           {
//               btn.SetVisible(visible);
//           }
//            //BarItemLink item = GetControlAsWFControl().barToolbar.ItemLinks.FirstOrDefault(il => il.Item.Name == name);
//            //if (item == null) return;

//            //item.Visible = visible;
//        }

//        private List<string> _colsSummaryChanged = null;
//        private bool _allowMerge = false;
//        private void applyAllowMerge()
//        {
//            if (isWeb()) return;

//            if (GetTopTable() != null && GetTopTable().Merged)
//            {
//                GetGrid().SetAllowCellMerge(_allowMerge);
//                //GetMainView().OptionsView.AllowCellMerge = _allowMerge;
//            }
//            else
//            {
//                GetGrid().SetAllowCellMerge(false);
//                //GetMainView().OptionsView.AllowCellMerge = false;

//            }

//        }
//        public void SetAllowMerge(bool value)
//        {

//            _allowMerge = value;
//            applyAllowMerge();

//        }
//        //public void SetMerged(bool value)
//        //{
//        //    // GetMainView().OptionsView.AllowCellMerge = value; // пока отключаю, но суммирование без дублей остается
           
//        //    if (value)
//        //    {
//        //        _colsSummaryChanged = new List<string>();
//        //        foreach (BandedGridColumn col in GetMainView().Columns)
//        //        {
//        //            if (col.SummaryItem.SummaryType == SummaryItemType.Sum)
//        //            {
//        //                if (!_colsSummaryChanged.Contains(col.FieldName))
//        //                {

//        //                    _colsSummaryChanged.Add(col.FieldName);
//        //                }
//        //                col.SummaryItem.SummaryType = SummaryItemType.Custom;
//        //            }
//        //        }
//        //    }
//        //    else
//        //    {
//        //        if (_colsSummaryChanged != null)
//        //        {
//        //            foreach (string colName in _colsSummaryChanged)
//        //            {
//        //                if (GetMainView().Columns[colName] != null)
//        //                {
//        //                    GetMainView().Columns[colName].SummaryItem.SummaryType = SummaryItemType.Sum;
//        //                }
//        //            }
//        //        }
//        //    }
//        //    applyAllowMerge();
//        //}

//        public void SetMerged(bool value)
//        {
//            var gr = GetGrid();

//            if (value)
//            {
//                _colsSummaryChanged = new List<string>();
//                foreach (object col in gr.GetColumns())
//                {
//                    if (gr.IsColumnSumSummaryType(col))
//                    {
//                        var fn = gr.GetColumnFieldName(col);
//                        if (!_colsSummaryChanged.Contains(fn))
//                        {

//                            _colsSummaryChanged.Add(fn);
//                        }
//                        gr.SetColumnCustomSummaryType(col);
                       
//                    }
//                }
//            }
//            else
//            {
//                if (_colsSummaryChanged != null)
//                {
//                    foreach (string colName in _colsSummaryChanged)
//                    {
//                        var col = gr.GetColumnByFieldName(colName);
//                        if (col != null)
//                        {
//                            gr.SetColumnSumSummaryType(col);
                           
//                        }
//                    }
//                }
//            }
//            applyAllowMerge();
//        }


//        IVPopupMenu _menu;
//        public void UpdatePopupMenu(XElement xmenu, IVBarItem[] items)
//        {
//            if (xmenu == null || items == null) return;

//            if (_menu == null)
//            {
//                //_menu = new PopupMenu(GetControlAsWFControl().barManager);
//                _menu = UIStatic.GetControlsfactory().CreatePopupMenu();
//                if (!UIStatic.IsWeb())
//                {
//                    (_menu as sql.builder.UI.WinForms.VPopupMenu).SetBarManager(GetControlAsWFControl().barManager);

//                }
//            }
//            _menu.Clear();
//            foreach (var item in items)
//            {
//                _menu.AddButton(item,false);
                
//            }

//          //  _menu.BeginUpdate();
//            //_menu.ClearLinks();
//            //_menu.AddItems(items);
//            //_menu.EndUpdate();

//            if (XmlReports.IsDeveloperMode()) AddDebugButtons();
//        }

//        private void ButtonAddRow_ItemClick(object sender)
//        {
//            //var view = grid.FocusedView as GridView;
//            //if (view == null) return;
//            //view.CloseEditor();

//            GetGrid().CloseEditor();
//            var table = GetTopTable();
//			if (!table.IsForeignKeyAvailable())
//			{
//				RaiseHasMessage(this, new HasMessageArgs() { Message = "Выберите существующий элемент в родительской таблице" });
//				return;
//			}
//            var row = table.NewRow();
//            PrepareNewRow(row);
//			table.Rows.Add(row);
          
           

//            SetSelection((new object[] { row[table.PrimaryKey[0]] }));
//        }
//        private void ButtonRefresh_ItemClick(object sender)
//        {
//            RefreshData();
//            GetGrid().ExpandAllNodes(); //чтоб чекбоксы не ломались при обновлении грида (кнопка (grid-refresh)); для TreeList можно использоваться devexpress method ExpandAll()
//			GetGrid().ResetFocus();

//        }

//        private void RefreshData()
//        {
//            if (_source == null) return;

//            VDataTable table = GetTopTable();

//            if (table.HasChildrenUserChanges())
//            {
//                RaiseHasMessage(this, new HasMessageArgs() { Message = "Имеются несохранённые изменения в дочерних таблицах" });
//                return;
//            }

//            try
//            {
//                Cursor.Current = Cursors.WaitCursor;

//                if (table.HasUserChanges)
//                {
//                    var result = ShowMessage.Show(ShowMessage.MType.UnsavedChangesQuestion);
//                    if (result == DialogResult.Cancel)
//                    {
//                        return;
//                    }
//                    //else if (result == DialogResult.Yes)
//                    //{
//                    //    grReference.CommitChanges();
//                    //}
//                }
//                GetTopTable().SetForceRefresh();
//                GetTopTable().Refresh();
//                UpdateButtonStates();
//            }
//            finally
//            {
//                Cursor.Current = Cursors.Default;
//            }
//        }

//        private void ButtonDeleteRow_ItemClick(object sender)
//        {
//            //var view = grid.FocusedView as GridView;
//            //if (view == null) return;

//            //int[] indexes =  view.GetSelectedRows().ToArray();
//            //var rows = indexes.Select(i => view.GetDataRow(i)).ToArray();
//            //if (rows.IsEmpty()) return;
//            IucGrid grid = this.GetGrid();
//            int[] indexes = grid.GetSelectedRowsHandles();
//            if (indexes.Length == 0) {
//                return;
//            }
//            DataRow[] rows = indexes.Select<int, DataRow>(grid.GetRowByHandle);
  
//            var table = GetTopTable();
           
//            foreach (DataRow row in rows)
//            {
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
//                    else table.DeleteRow(row);
//                }
//            }

//            foreach (var i in indexes) GetGrid().DxRefreshRow(i);
//        }
//        private void ButtonCommit_ItemClick(object sender)
//        {
//            var table = GetTopTable();

//            AcceptChanges();

//            // если строка удаляется - надо сохранить в дочерних иначе глючит
//            if (table.CurrentRow != null && table.IsRowDeleted(table.CurrentRow) && table.HasChildrenUserChanges())
//            {
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
//            for (int index = 0; index < table.Rows.Count; index++) {
//                string msg = table.GetRowErrorText(table.Rows[index]).Error;
//                if (msg != "") {
//                    msg = "Сохранение невозможно! " + msg;
//                    RaiseHasMessage(this, new HasMessageArgs() { Message = msg });
//                    return;
//                }
//            }

//            UpdateButtonStates();
//            GetGrid().DxRefreshViewData();
//            //foreach (GridView view in grid.ViewCollection)
//            //{
//            //    view.RefreshData();
//            //}
//        }

//        public bool IsDxExport = true;
//        public string CaptionForExport =null;
//        private void ButtonExportExcel_ItemClick(object sender)
//        {
//            string path = "";
//            if (CaptionForExport != null)
//            {

//                path = Printing.GetFreeName(Printing.outputFolder, CaptionForExport, "xlsx");
//            }
//            else
//            {
//                path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xlsx");
//            }
//            var d0 = DateTime.Now;
//            ExportToXlsx(path, CaptionForExport);
//            var d1 = DateTime.Now;

//            if ((d1 - d0).Seconds > 5)
//            {
//                ShowMessage.ShowInformation("Экспорт завершен");
//            }
//            Process.Start(path);

//        }

    

//        public void AcceptChanges()
//        {
//            //GetMainView().PostEditor();
//            //GetMainView().UpdateCurrentRow();

//            GetGrid().EndEdit();
//            UpdateDataSourceSelectedRows();
//            // не уверен что это нужно в новой версии
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
//        //public void SetSelection(IEnumerable<object> values)
//        //{
//        //    if (isWeb()) return;
//        //    bool fetched = false;

//        //    var view = GetMainView();
//        //    view.ClearSelection();
//        //    var table = GetTopTable();
//        //    foreach (var value in values)
//        //    {
//        //        var row = table.Rows.Find(value);
//        //        if (row == null && !fetched)
//        //        {
//        //            grid.BeginUpdate();
//        //            table.FetchTo(int.MaxValue);
//        //            grid.EndUpdate();
//        //            fetched = true;

//        //            row = table.Rows.Find(value);
//        //        }

//        //        if (row != null)
//        //        {

//        //            view.SelectRow(view.GetRowHandle(table.Rows.IndexOf(row)));
//        //            if (values.Count()==1){
//        //                view.FocusedRowHandle = view.GetRowHandle(table.Rows.IndexOf(row));
//        //            }
//        //        }
//        //    }

//        //    UpdateDataSourceSelectedRows();
//        //}


//        public void SetSelection(IEnumerable<object> values)
//        {
//            if (isWeb()) return;
//            bool fetched = false;
//            var gr = GetGrid();
        
//            gr.ClearSelection();
//            var table = GetTopTable();
       
//            foreach (var value in values)
//            {
//                var row = table.Rows.Find(value);
//                if (row == null && !fetched)
//                {
//                    GetGrid().BeginControlUpdate();
//                    //grid.BeginUpdate();
//                    table.FetchTo(int.MaxValue);
//                    GetGrid().EndControlUpdate();
//                    //grid.EndUpdate();
//                    fetched = true;

//                    row = table.Rows.Find(value);
//                }

//                if (row != null)
//                {
//                    gr.SelectRow(row);
                    
//                    if (values.Count() == 1)
//                    {
//                        gr.SetFocusedRow(row);
                       
//                    }
//                }
//            }

//            UpdateDataSourceSelectedRows();
//        }


//        public VToolTipInfo GetToolTipInfo(DataRow row, string columnName,bool checkWarning)
//        {
            
           

//            //var view = grid.FocusedView as GridView;
//            //if (view == null) return;

//            //GridHitInfo hitInfo = view.CalcHitInfo(args.ControlMousePosition);
//            //if (!hitInfo.InRowCell || hitInfo.HitTest != GridHitTest.RowCell) return;

//            //var row = view.GetDataRow(hitInfo.RowHandle);
//            //if (row == null) return;

//            //GridViewInfo vinfo = (GridViewInfo)view.GetViewInfo();
//            //GridCellInfo cell = vinfo.GetGridCellInfo(hitInfo.RowHandle, hitInfo.Column);

//            var table = (row.Table as VDataTable);
//            int row_index = table.Rows.IndexOf(row);
//            var colName = table.GetNameForText(columnName);
//            //var row_index = view.GetDataSourceRowIndex(hitInfo.RowHandle);// !!! проконтролировать время
//            var rep = repositories().GetCellRepository(table, colName, row_index/*, true*/);// !!! проконтролировать время

//            VToolTipInfo ti = null;
//            if (checkWarning)
//            {
//                var msg = table.GetCellErrorForText(row, columnName);
               
//                if (!string.IsNullOrEmpty(msg))
//                {
//                    ti = new VToolTipInfo();

//                    ti.image = Cmn.ImageWarning14;
//                    ti.text = msg;
//                    ti.Id = table.GetRowId(row).ToString() + colName;

//                }
//            }

//            if (ti==null)
//            {
//                if (rep is RepositoryItemHyperLinkEdit)// проверять как нибудь по другому
//                {
//                    ti = new VToolTipInfo();


//                    ti.text = UILink.TooltipText;
//                    ti.Id = table.GetRowId(row).ToString() + colName;
//                }
//            }

//            return ti;

//        }


//        public bool IsToolTipForWarning(GridHitInfo hitInfo, GridView view,DataRow row) // промежуточный вариант, только для winForms
//        {
//            GridViewInfo vinfo = (GridViewInfo)view.GetViewInfo();
//            GridCellInfo cell = vinfo.GetGridCellInfo(hitInfo.RowHandle, hitInfo.Column);

//            var table = (row.Table as VDataTable);
//            var colName = table.GetNameForText(hitInfo.Column.FieldName);
//            var row_index = view.GetDataSourceRowIndex(hitInfo.RowHandle);// !!! проконтролировать время
//            var rep = repositories().GetCellRepository(table, colName, row_index/*, true*/);// !!! проконтролировать время

//            var shift = Cmn.GetLeftButtonsSize(colName, rep);
//            var l = (hitInfo.HitPoint.X - cell.Bounds.Left);

//            var checkWarning = false;
//            if ((l > shift) && (l < 15 + shift))
//            {
//                checkWarning = true;
//            }
//            return checkWarning;
//        }

//        //private void tooltip_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs args)
//        //{
//        //    if (args.Info != null || args.SelectedControl != grid) return;

//        //    var view = grid.FocusedView as GridView;
//        //    if (view == null) return;

//        //    GridHitInfo hitInfo = view.CalcHitInfo(args.ControlMousePosition);
//        //    if (!hitInfo.InRowCell || hitInfo.HitTest != GridHitTest.RowCell) return;

//        //    var row = view.GetDataRow(hitInfo.RowHandle);
//        //    if (row == null) return;

//        //    var checkWarning = IsToolTipForWarning(hitInfo,view,row);
          
//        //    var ti = GetToolTipInfo(row, hitInfo.Column.FieldName, checkWarning);
//        //    if (ti != null)
//        //    {
//        //        SuperToolTip toolTip = new SuperToolTip();
//        //        ToolTipItem item = new ToolTipItem();
//        //        item.Image = ti.image;
//        //        item.Text = ti.text;
//        //        toolTip.Items.Add(item);
//        //        args.Info = new ToolTipControlInfo(ti.Id, null);
//        //        args.Info.SuperTip = toolTip;
//        //    }
             
//        //}

//        //private void tooltip_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs args)
//        //{
//        //    if (args.Info != null || args.SelectedControl != grid) return;

//        //    var view = grid.FocusedView as GridView;
//        //    if (view == null) return;

//        //    GridHitInfo hitInfo = view.CalcHitInfo(args.ControlMousePosition);
//        //    if (!hitInfo.InRowCell || hitInfo.HitTest != GridHitTest.RowCell) return;

//        //    var row = view.GetDataRow(hitInfo.RowHandle);
//        //    if (row == null) return;

//        //    GridViewInfo vinfo = (GridViewInfo)view.GetViewInfo();
//        //    GridCellInfo cell = vinfo.GetGridCellInfo(hitInfo.RowHandle, hitInfo.Column);

//        //    var table = (row.Table as VDataTable);
//        //    var colName = table.GetNameForText(hitInfo.Column.FieldName);
//        //    var row_index = view.GetDataSourceRowIndex(hitInfo.RowHandle);// !!! проконтролировать время
//        //    var rep = _repositories.GetCellRepository(table, colName, row_index/*, true*/);// !!! проконтролировать время

//        //    var shift = Cmn.GetLeftButtonsSize( colName, rep);
//        //    var l = (hitInfo.HitPoint.X - cell.Bounds.Left);

//        //    var iswarningToolTip = false;
//        //    if ((l > shift) && (l < 15 + shift))
//        //    {
//        //        var msg = table.GetCellErrorForText(row, hitInfo.Column.FieldName);
//        //        if (!string.IsNullOrEmpty(msg))
//        //        {
//        //            SuperToolTip toolTip = new SuperToolTip();

//        //            ToolTipItem item = new ToolTipItem();
//        //            item.Image = Cmn.ImageWarning14;
//        //            item.Text = msg;
//        //            toolTip.Items.Add(item);
//        //            args.Info = new ToolTipControlInfo(hitInfo.RowHandle + hitInfo.Column.FieldName, null);
//        //            args.Info.SuperTip = toolTip;
//        //            iswarningToolTip = true;
//        //        }
//        //    }

//        //    if (!iswarningToolTip)
//        //    {
//        //        if (rep is RepositoryItemHyperLinkEdit)
//        //        {
//        //            SuperToolTip toolTip = new SuperToolTip();

//        //            ToolTipItem item = new ToolTipItem();
//        //            item.Text = UILink.TooltipText;
//        //            toolTip.Items.Add(item);
//        //            args.Info = new ToolTipControlInfo(hitInfo.RowHandle + hitInfo.Column.FieldName, null);
//        //            args.Info.SuperTip = toolTip;
//        //        }
//        //    }
//        //}


       


//        public event HasMessageHandler HasMessage;
//        public void RaiseHasMessage(object sender, HasMessageArgs args)
//        {
//            if (HasMessage != null)
//            {
//                HasMessage(sender, args);
//            }
//        }

//        private void OnDisposed()
//        {
//            _menu = null;
//            // от утечек
//            // https://www.devexpress.com/Support/Center/Question/Details/Q534989

            
//            //foreach (var gv in grid.ViewCollection.Cast<GridView>()) gv.Dispose();
//            GetGrid().DisposeViews();
//            repositories().Dispose();
//        }

//        #region ViewSettings
//        string _form_name;
//        public void SetFormName(string form_name)
//        {
//            _form_name = form_name;
//        }

//        //public XElement GetViewSettings()
//        //{
//        //    XElement xroot = new XElement("root");
//        //    foreach (GridView view in grid.ViewCollection)
//        //    {
//        //        var xtable = new XElement("table", new XAttribute("name", view.Name));
//        //        xroot.Add(xtable);

//        //        var xcolumns = new XElement("columns");
//        //        xtable.Add(xcolumns);

//        //        foreach (GridColumn col in view.Columns.Where(c => c.Name != GridDesigner.GetDummyColumnName()))
//        //        {

//        //            var xcolumn = new XElement("column",
//        //                new XAttribute("name", col.FieldName),
//        //                new XAttribute("width", col.Width),
//        //                new XAttribute("visible", col.Visible ? 1 : 0),
//        //                new XAttribute("visible_index", col.VisibleIndex),
//        //                new XAttribute("sort", col.SortOrder.ToString().ToLower()),
//        //                new XAttribute("group", col.GroupIndex));

//        //            xcolumns.Add(xcolumn);
//        //        }
//        //    }

//        //    return xroot;
//        //}

//        public XElement GetViewSettings()
//        {
//            XElement xroot = new XElement("root");
//            var gr = GetGrid();
//            foreach (var view in gr.GetViews())
//            {
//                var xtable = new XElement("table", new XAttribute("name",GetGrid().GetViewName(view)));
//                xroot.Add(xtable);

//                var xcolumns = new XElement("columns");
//                xtable.Add(xcolumns);

//                foreach (var col in gr.GetColumns(view))
//                {
//                    var colName = gr.GetColumnName(col);
//                    if (colName != GridDesigner.GetDummyColumnName())
//                    {
//                        var xcolumn = new XElement("column",
//                            new XAttribute("name",  gr.GetColumnFieldName(col)),
//                            new XAttribute("width",gr.GetColumnWidth(col)),
//                            new XAttribute("visible", gr.GetColumnVisible(col) ? 1 : 0),
//                            new XAttribute("visible_index", gr.GetColumnVisibleIndex(col)),
//                            new XAttribute("sort", gr.GetColumnSortOrder(col)),
//                            new XAttribute("group",  gr.GetColumnGroupIndex(col))
//                            );
//                        xcolumns.Add(xcolumn);
//                    }

                   
//                }
//            }

//            return xroot;
//        }
//        public void SetViewSettings(XElement xroot)
//        {
//            //grid.BeginUpdate();
//            var gr = GetGrid();
//            gr.BeginControlUpdate();
//            foreach (XElement xtable in xroot.Elements("table"))
//            {
//                //var view = grid.ViewCollection.Cast<GridView>().FirstOrDefault(v => v.Name == xtable.Attribute("name").Value);
//                //if (view == null) continue;

//                //view.BeginUpdate();
//                var  tname=xtable.Attribute("name").Value;

//                var view = gr.GetViewByName(tname);

//                if (view == null) continue;
//                gr.BeginViewUpdate(tname);

//                var names = xtable.Element("columns").Elements("column").Select(c => c.Attribute("name").Value).ToArray();
//                //foreach (var c in view.Columns.Where(c => names.Contains(c.FieldName))) c.Visible = false;

//                //foreach (var c in gr.GetColumns(view))
//                //{
//                //    if (names.Contains(gr.GetColumnFieldName(c)))
//                //    {
//                //        gr.SetColumnVisible(c, false);
//                //    }
//                //}
//				foreach (var xcolumn in xtable.Element("columns").Elements("column").OrderBy(c => int.Parse(c.Attribute("visible_index").Value)))
//                {
//                    //var col =   view.Columns.FirstOrDefault(c => c.FieldName == xcolumn.Attribute("name").Value);
//                    var col = gr.GetColumnByFieldName(tname,xcolumn.Attribute("name").Value);
//                    if (col == null) continue;

//                    //col.Width = int.Parse(xcolumn.Attribute("width").Value);
//                    //col.Visible = (xcolumn.Attribute("visible").Value == "1");
//                    //col.VisibleIndex = int.Parse(xcolumn.Attribute("visible_index").Value);
//                    //col.SortOrder = (ColumnSortOrder)Enum.Parse(typeof(ColumnSortOrder), xcolumn.Attribute("sort").Value, true);
//                    //col.GroupIndex = int.Parse(xcolumn.Attribute("group").Value);

//                    gr.SetColumnWidth(col, int.Parse(xcolumn.Attribute("width").Value));
                    

//                    //убираю т.к. сохраняется видимость при обработке переменных, а это не нужно, пользователь не может управлять видимостью
//					//добавлено для функционала выбора колонок ползователем
//					if (_allowSelectMoveColumns)
//					{
//						gr.SetColumnVisible(col, (xcolumn.Attribute("visible").Value == "1"));
//						gr.SetColumnVisibleIndex(col, int.Parse(xcolumn.Attribute("visible_index").Value));
//					}


                
                
//                    gr.SetColumnSortOrder(col, xcolumn.Attribute("sort").Value.ToLower());
//                   // gr.SetColumnGroupIndex(col, int.Parse(xcolumn.Attribute("group").Value)); 
                    
//                }

//                GetGrid().EndViewUpdate(tname);
//			}
//            GetGrid().EndControlUpdate();
//			if (_allowSelectMoveColumns)  _manualColumnsConfigurationLoaded = true;
//            //grid.EndUpdate();
//        }
//        public void RestoreViewSettings()
//        {
//            // если не на UIFormC - пока не работает
//            if (_form_name == null) return;

//            string reg_path = String.Format(@"forms\{0}\_grids", _form_name);
//            Cmn.WriteXElementToRegistry(reg_path, "default", null);
//            getSpecialBarButton(TextConst.AVGridButtonType.RestoreSettings).SetEnabled(false);
//            //GetControlAsWFControl().ButtonRestoreSettings.Enabled = false;

//            BeginUpdate();
//            LoadFromXml(_xscheme, true,true);
//            EndUpdate();
//        }

//        public void LoadSettingsFromRegistry()
//        {
//            // если не на UIFormC - пока не работает
//            // в режиме разработки настройки не сохраняем и не загружаем
//            if (_form_name == null || XmlReports.IsDeveloperMode()) return;

//            string reg_path = String.Format(@"forms\{0}\_grids", _form_name);
//            var xsettings = Cmn.ReadXElementFromRegistry(reg_path, "default");
//            getSpecialBarButton(TextConst.AVGridButtonType.RestoreSettings).SetEnabled(xsettings != null);
//            //GetControlAsWFControl().ButtonRestoreSettings.Enabled = (xsettings != null);
//            if (xsettings == null) return;

//            SetViewSettings(xsettings);
//        }
//        public void SaveSettingsToRegistry()
//        {
//            // если не на UIFormC - пока не работает
//            // в режиме разработки настройки не сохраняем и не загружаем
//            if (_form_name == null || XmlReports.IsDeveloperMode()) return;

//            var xsettings = GetViewSettings();
//            getSpecialBarButton(TextConst.AVGridButtonType.RestoreSettings).SetEnabled(xsettings != null);
//            //GetControlAsWFControl().ButtonRestoreSettings.Enabled = (xsettings != null);

//            string reg_path = String.Format(@"forms\{0}\_grids", _form_name);
//            Cmn.WriteXElementToRegistry(reg_path, "default", xsettings);
//        }

//        private void ButtonSaveSettings_ItemClick(object sender)
//        {
//            SaveSettingsToRegistry();
//        }
//        private void ButtonRestoreSettings_ItemClick(object sender)
//        {
//            var result = ShowMessage.ShowQuestion("Вы уверены, что хотите сбросить сохранённые настройки колонок?");
//            if (result == DialogResult.Yes) RestoreViewSettings();
//        }

//        XElement _viewChanges;
//        public void HoldViewChanges()
//        {
//            _viewChanges = GetViewSettings();
//        }
//        //public void RestoreViewChanges()
//        //{
//        //    if (_viewChanges == null) return;

//        //    SetViewSettings(_viewChanges);
//        //    _viewChanges = null;
//        //}
//        #endregion



//        private void ButtonChoiceRow_ItemClick(object sender)
//        {
//            MainControl.AcceptSelection();
//        }

//        void PrepareNewRow(DataRow row)
//        {
//            if (_order_field_name == null) return;

//            //var view = GetMainView();
//            //var focusedRow = view.GetFocusedDataRow();
//            var focusedRow = GetGrid().GetFocusetDataRow();
//            if (focusedRow != null)
//            {
//                var rowNext = GetTopTable().AsEnumerable()
//                    .OrderBy(r => r[_order_field_name])
//                    .SkipWhile(r => r != focusedRow)
//                    .Skip(1)
//                    .FirstOrDefault();

//                if (rowNext != null)
//                {
//                    row[_order_field_name] = rowNext[_order_field_name];
//                    MoveRowsDown(rowNext);
//                }
//                else
//                {
//                    row[_order_field_name] = (decimal)focusedRow[_order_field_name] + decimal.One;
//                }
//            }
//            else
//            {
//                row[_order_field_name] = Cmn.DECIMAL_ONE;
//            }
//        }

//        void AddDebugButtons()
//        {
//            var menu = (_menu as PopupMenu);
//            if (_top_table_name == null) return;

//            menu.AddItem(new BarHeaderItem() { Caption = _top_table_name });
//            var btn = new BarButtonItem(GetControlAsWFControl().barManager, "Сохранить настройки колонок (в проекте)");
//            btn.ItemClick += ButtonSaveDefaultColumnsSettingsClick;
//            menu.AddItem(btn);
//            btn = new BarButtonItem(GetControlAsWFControl().barManager, "Сбросить настройки колонок");
//            btn.ItemClick += ButtonClearDefaultColumnsSettingsClick;
//            menu.AddItem(btn);
//        }

//        //private void ButtonSaveDefaultColumnsSettingsClick(object sender, ItemClickEventArgs args)
//        //{
//        //    VForm vform = XmlReports.Environment.GetForm(_form_name);
//        //    if (vform == null) return;

//        //    var vitems = vform.GetDescedantsApplyingParts(TextConst.EName.Grid)
//        //        .First(g => g.P_Table == _top_table_name)
//        //        .GetElementsApplyingParts(TextConst.EName.Columns)
//        //        .First()
//        //        .GetDescedantsApplyingParts();

//        //    foreach (GridColumn column in GetMainView().VisibleColumns)
//        //    {
//        //        string colName = column.FieldName.Replace(TextConst.Pfx.ExtValName, "");
//        //        if(colName == "") continue;

//        //        var vitem = vitems.FirstOrDefault(vc => vc.P_Alias == colName) 
//        //                 ?? vitems.FirstOrDefault(vc => vc.P_Column == colName);

//        //        if (vitem == null) continue;

//        //        if (column.Width != GridDesigner.GetDefaultGridColumnWidth())
//        //        {
//        //            vitem.P_ColumnWidth = column.Width.ToString();
//        //        }
//        //        else
//        //        {
//        //            vitem.P_ColumnWidth = "";
//        //        }
//        //    }

//        //    XmlReports.UpdateElementInCompiledScheme(vform);

//        //    vform.SourceFileName = ucQueryEditor.AddPathToFilename(Cmn.GetAttrValue(vform, "file"));
//        //    vform.ParentName = TextConst.EName.Forms;
//        //    vform.SaveInSourceFile();
//        //}

//        private void ButtonSaveDefaultColumnsSettingsClick(object sender, ItemClickEventArgs args)
//        {
//            VForm vform = XmlReports.Environment.GetForm(_form_name);
//            if (vform == null) return;

//            var vitems = VSXElement.GetDescedantsP(vform.GetDescedantsP(EName.grid)
//                .First(g => g.P_Table == _top_table_name)
//                .GetElementsP(EName.columns)
//                .First());
//            var gr = GetGrid();
//            var checkList = new HashSet<string>();
//            foreach (object column in GetGrid().GetColumns())
//            {
//                string colName = gr.GetColumnFieldName(column).Replace(TextConst.Pfx.ExtValName, "");
//                if (colName == "") continue;

//                var vitem = vitems.FirstOrDefault(vc => vc.P_Alias == colName)
//                         ?? vitems.FirstOrDefault(vc => vc.P_Column == colName);

//                if (vitem == null) continue;
//                var wd = gr.GetColumnWidth(column);
//                if (wd != GridDesigner.GetDefaultGridColumnWidth())
//                {
//                    vitem.P_ColumnWidth = wd.ToString();
//                    if (!checkList.Contains(vitem.XName))
//                    {
//                        checkList.Add(vitem.XName);
//                    }
                    
//                }
//                else
//                {
//                    if (!checkList.Contains(vitem.XName))
//                    {
//                        vitem.P_ColumnWidth = "";
//                    }
                  
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
//    }



//    class OrderedBlock
//    {
//        public DataRow Prev { get; set; }
//        public List<DataRow> Rows { get; private set; }
//        public DataRow Next { get; set; }

//        public OrderedBlock()
//        {
//            Rows = new List<DataRow>();
//        }
//    }

//    public class VToolTipInfo
//    {
//       public string Id;
//       public Image image;
//       public string text;
//    }
//}
