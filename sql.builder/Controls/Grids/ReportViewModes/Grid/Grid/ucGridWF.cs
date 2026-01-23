//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
////using System.Windows.Forms;
//using DevExpress.Data;
//using DevExpress.Utils;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.Grid;

//using sql.builder.DataApi;
//using sql.builder.Print.Xlsx;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
//using DevExpress.XtraGrid.Views.Grid.ViewInfo;
//using Clipboard = System.Windows.Clipboard;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
  
//    internal partial class ucGridWF : GridControl, IucGrid
//    {
    
//        public ucGridWF()
//        {

//            this.ShowOnlyPredefinedDetails = true;
//            KeyDown += (sender, args) =>
//            {
//                if (args.Control && args.KeyCode == Keys.C)
//                {
//                    if (FocusedView == null) return;
//                    GridView view = FocusedView as GridView;
//                    if(view.GetSelectedCells().Length != 1) return;
//                    Clipboard.SetText(view.GetFocusedDisplayText());
//                    args.Handled = true;
//                }
//            };

//			loadingControl = new DevExpress.XtraWaitForm.ProgressPanel();
//			loadingControl.ShowCaption = false;
//			loadingControl.Description = "Загрузка данных...";
//			loadingControl.Dock = DockStyle.Bottom;
//			loadingControl.Visible = false;
//			loadingControl.ContentAlignment = System.Drawing.ContentAlignment.MiddleCenter;
//			this.Resize += setLoadingControlHeight;
//			loadingControl.Height = this.Height - 40;
//			this.Controls.Add(loadingControl);
//        }
//        public void ExpandAllNodes()
//        {
           
//        }
//        public void SetParentFieldName(string value)
//        {
//        }
//        public void SetKeyFieldName(string value)
//        {
          

//        }

//        public void Clear()
//        {
//            // от утечек
//            // https://www.devexpress.com/Support/Center/Question/Details/Q534989

//            foreach (var v in ViewCollection.Cast<GridView>().ToArray()) v.Dispose();
//            _views.Clear();
//        }




//        private Dictionary<string, object> _views = new Dictionary<string, object>();
//        bool _showAutoFilterRow = false;
//		bool _allowSelectMoveColumns = false;
//        public void SetShowAutoFilterRow(bool value)
//        {
//            _showAutoFilterRow = value;
//            foreach (GridView v in this.ViewCollection)
//            {
//                v.OptionsView.ShowAutoFilterRow = _showAutoFilterRow;
//            }
//        }
//		public void SetAllowSelectMoveColumns(bool value)
//		{
//			_allowSelectMoveColumns = value;
//			foreach (GridView v in this.ViewCollection)
//			{
//				v.OptionsCustomization.AllowColumnMoving = XmlReports.IsDeveloperMode() || _allowSelectMoveColumns;
//			}
//		}
//        public void CreateView(string name, bool banded)
//        {
//            GridView view = (banded) ? new BandedGridView() : new GridView();
           
//            GridDesigner.SetViewSettings(view);
//            view.Name = name;
//            view.OptionsView.ShowAutoFilterRow = _showAutoFilterRow;
//			view.OptionsCustomization.AllowColumnMoving = XmlReports.IsDeveloperMode() ||_allowSelectMoveColumns;
           
//            this.ViewCollection.Add(view);
//            if (this.MainView == null) this.MainView = view;
//            _views.Add(name, view);

//        }

//        private GridView getViewByName(string name)
//        {
//            if (name == null)
//            {
//                return GetMainView();
//            }
//            if (!_views.ContainsKey(name))
//            {
//                return null;
//            }
//            return (GridView)_views[name];
         
//        }
//        private BandedGridView getBandedViewByName(string name)
//        {
//            return getViewByName(name) as BandedGridView;
//        }
//        public void BeginViewUpdate(string name)
//        {
//            getViewByName(name).BeginUpdate();
//        }
//        public void EndViewUpdate(string name)
//        {
//            getViewByName(name).EndUpdate();
//        }

//        public void BeginControlUpdate()
//        {
//            this.BeginUpdate();
//        }

//        public void EndControlUpdate()
//        {
//            this.EndUpdate();
//        }


//        public void SetViewTitle(string name, string value)
//        {
//            getViewByName(name).ViewCaption =value;
         
//        }


//        public void SetMainView(string name)
//        {
//            this.MainView = getViewByName(name);
//        }


//        public void ClearViewContent(string name)
//        {
//            var view = getViewByName(name);

//            getViewByName(name).Columns.Clear();
//            if (view is BandedGridView)
//            {
//                (view as BandedGridView).Bands.Clear();
//            }
//            getViewByName(name).GroupSummary.Clear();
//        }


       

//        private GridBand getBand(object band)
//        {
//            return band as GridBand;
//        }

//        private BandedGridColumn getBandColumn(object column)
//        {
//            return column as BandedGridColumn;
//        }

//        private GridColumn getColumn(object column)
//        {
//            return column as GridColumn;
//        }

//        public object CreateBandColumn()
//        {
      
//            var col = new BandedGridColumn();
//            col.OptionsColumn.AllowEdit = true;
//            //SetColumnVisible(col, true);
//            return col;
//        }
//        public void AddColumnToBand(object band,object column)
//        {

//             getBand(band).Columns.Add(getBandColumn(column));

            
//        }


//        public void SetShowBands(string viewName, bool value)
//        {

//            getBandedViewByName(viewName).OptionsView.ShowBands = value;
      
//        }


//        public void AddBandedDummyColumnAndHideEmptyBands(string viewName)
//        {
//            var bview = getBandedViewByName(viewName);
//            BandedGridColumn colLast = bview.Columns.LastOrDefault() as BandedGridColumn;
//            if (colLast != null)
//            {
//                // только если нет бэндов

//                var band = new GridBand();
//                bview.Bands.Add(band);

//                //var colDummy = GridDesigner.CreateDummyBGridColumn();
//                //band.Columns.Add(colDummy);
//            }

//            // прячем бэнды без видимых элементов
//            foreach (GridBand band in bview.Bands)
//            {
//                if (String.IsNullOrEmpty(band.Caption))
//                {
//                    if (band.Columns.VisibleColumnCount == 0 && band.Children.VisibleBandCount == 0)
//                    {
//                        band.Visible = band.Columns.VisibleColumnCount > 0 || band.Children.VisibleBandCount > 0;
//                    }
//                }
//            }
//        }


//        public object CreateBand()
//        {
//            var band = new GridBand();
//            band.AppearanceHeader.Options.UseTextOptions = true;
//            band.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
//            band.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
  
//            return band;
//        }
//        public void AddBandToView(string viewName, object band)
//        {
          
//            getBandedViewByName(viewName).Bands.Add(getBand(band) );
          
//        }

     
//        public void AddBandToBand(object parenBand, object childBand)
//        {
//            var band = new GridBand();
//            band.AppearanceHeader.Options.UseTextOptions = true;
//            band.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            getBand(parenBand).Children.Add(getBand(childBand));
         
//        }

//        public object CreateColumn()
//        {
//           var col=new GridColumn();
//           col.OptionsColumn.AllowEdit = true;
//          // SetColumnVisible(col, true);
//           return col;
//        }


//        public void AddColumnToView(string viewName, object column)
//        {
//           getViewByName(viewName)  .Columns.Add( getColumn( column));
//        }


//        /*public void AddDummyColumn(string viewName)
//        {
//            //// чтобы удобно было менять ширину последней колонки
//            var colDummy = GridDesigner.CreateDummyGridColumn();
//           getViewByName(viewName).Columns.Add(colDummy);
//        }*/


//        public void SetParentView(string childViewName, string parentViewName)
//        {
//            getViewByName(childViewName).ParentView = getViewByName(parentViewName);
//        }


//        public void SetBandTitle(object band, string value)
//        {
//            getBand(band).Caption = value;
//        }

//        public void SetBandName(object band, string value)
//        {
//            getBand(band).Name = value;
//        }

//        public void SetBandWidth(object band, int value)
//        {
//            getBand(band).Width = value;
//        }

//        public void AnalizeBandVisibility(object band)
//        {
//            var band1 = getBand(band);
//            band1.Visible = band1.Columns.VisibleColumnCount > 0 || band1.Children.VisibleBandCount > 0;
//        }


//        public void SetColumnFieldName(object column, string value)
//        {
//            getColumn(column).FieldName = value;
//        }

//        public void SetColumnWidth(object column, int value)
//        {
//            getColumn(column).Width = value;
//        }

//        public void SetColumnExtOption(object column, string optionName, string value)
//        {
//            var column1 = getColumn(column);
//            var ext_options = (Dictionary<string, string>)(column1.Tag ?? (column1.Tag = new Dictionary<string, string>()));
//            ext_options[optionName] = value;
//        }

//        public void SetColumnTypeFormatAndSummary( string viewName,object column1, string type, string format, string agg)
//        {
//            var column = getColumn(column1);
//            var view = getViewByName(viewName);
//            switch (type)
//            {
//                case "number":
//                    column.UnboundType = UnboundColumnType.Decimal;
//                    column.DisplayFormat.FormatType = FormatType.Numeric;
//                    //     column.SummaryItem.SummaryType = SummaryItemType.Sum;

//                    format = format ?? "n2";
//                    column.SummaryItem.DisplayFormat = (format.StartsWith("{0:") ? format : string.Format("{{0:{0}}}", format));

//                    break;
//                case "date":
//                    column.UnboundType = UnboundColumnType.DateTime;
//                    column.DisplayFormat.FormatType = FormatType.DateTime;
//                    column.DisplayFormat.FormatString = "dd.MM.yyyy";
//                    //column.ShowButtonMode = ShowButtonModeEnum.ShowOnlyInEditor;
//                    break;
//                default:
//                    column.UnboundType = UnboundColumnType.String;
//                    break;
//            }

//            if (type == "number")
//            {


//                if (!string.IsNullOrEmpty(agg))
//                {

//                    // Сохраняем значение признака колонки agg

//                    SetColumnExtOption(column1, "agg", agg);
                

//                    if (agg != "no")
//                    {
//                        var summary_type = SummaryItemType.None;
//                        switch (agg)
//                        {
//                            case "sum": summary_type = SummaryItemType.Sum; break;
//                            case "avg": summary_type = SummaryItemType.Average; break;
//                            case "count": summary_type = SummaryItemType.Count; break;
//                            case "max": summary_type = SummaryItemType.Max; break;
//                            case "min": summary_type = SummaryItemType.Min; break;
//                            default: if (!Compiler.aggFuncsNames.Contains(agg)) summary_type = SummaryItemType.Custom; break;
//                        }
//                        column.SummaryItem.SummaryType = summary_type;

//                        view.GroupSummary.Add(new GridGroupSummaryItem(summary_type, column.FieldName, column, (format.StartsWith("{0:") ? format : string.Format("{{0:{0}}}", format))));
//                    }

//                }
//                else
//                {
//                    column.SummaryItem.SummaryType = SummaryItemType.Sum;
//                    view.GroupSummary.Add(new GridGroupSummaryItem(SummaryItemType.Sum, column.FieldName, column, (format.StartsWith("{0:") ? format : string.Format("{{0:{0}}}", format))));
//                }
//            }

//            if (format != null)
//            {
//                column.DisplayFormat.FormatString = format;
//            }
//        }


//        public void SetColumnTitle(object column, string value)
//        {
//            getColumn(column).Caption = value;
//        }

//        public void SetColumnTitle(string tableName,string  columnName, string value)
//        {
//            var col = getViewByName(tableName).Columns.ColumnByFieldName(columnName);
//            if (col != null)
//            {
//                col.Caption = value;
//            }
//            else
//            {
//                getBandedViewByName(tableName).Bands.Where(b => b.Name == columnName).First().Caption = value;
//            }
//        }

//        public void SetColumnVisible(object column, bool value)
//        {
//            //if (getColumn(column).FieldName == "dat_post")
//            //{

//            //}
//            //if (getColumn(column).FieldName == "ur_pretenz_pio_date")
//            //{

//            //}
//            getColumn(column).Visible = value;
//        }

//		public void SetColumnInvisibleInColumnChooser(object column)
//		{
//			getColumn(column).OptionsColumn.ShowInCustomizationForm = false;
//		}


//        public void SetColumnFixedLeft(object column)
//        {
//            getColumn(column).Fixed = FixedStyle.Left;
//        }

//        public void SetColumnFixedRight(object column)
//        {
//            getColumn(column).Fixed = FixedStyle.Right;
                        
//        }

//        public void SetColumnHAlign(object column, string value)
//        {
//            var col = getColumn(column);
//            col.AppearanceCell.TextOptions.HAlignment = Parser.SHAllignToDxHallign(value);
//            col.AppearanceCell.Options.UseTextOptions = true;
//        }


//        public void SetDataSource(IVTableDataAdapter dataSource)
//        {
//            if (dataSource == null)
//            {
//                this.DataSource = null;
//            }
//            this.DataSource = dataSource.GetSource() ;
//        }


//        public void EndUpdateData()
//        {
//            //throw new NotImplementedException();
//            // для веб
//        }


//        public string[] GetVisibleColumnsNames(string tableName)
//        {
//           return getViewByName(tableName).VisibleColumns.Select(c => c.FieldName).ToArray();
//        }


//        public void SetColumnGroupIndex(object column, int value)
//        {
           
//            getColumn(column).GroupIndex = value;
//        }

//        public void SetColumnSortOrder(object column, string value)
//        {
//            switch (value)
//            {

//                case "ascending": getColumn(column).SortOrder = ColumnSortOrder.Ascending; break;
//                case "descending": getColumn(column).SortOrder = ColumnSortOrder.Descending; break;
//                default: getColumn(column).SortOrder = ColumnSortOrder.None; break;
//            }
//        }

//        public object GetColumnByFieldName(string tableName, string columnName)
//        {
//            var col = getViewByName(tableName).Columns.ColumnByFieldName(columnName);
//            return col;
//        }
//        public object GetColumnByFieldName(string columnName)
//        {
//            return GetMainView().Columns.ColumnByFieldName(columnName);
//        }
//        public void EndEdit()
//        {
//            var view = GetMainView();
//            if (view == null) return;

//            view.PostEditor();
//            view.UpdateCurrentRow();
//        }

//        public void CloseEditor()
//        {
//            var view = GetMainView();
//            if (view == null) return;

//            view.CloseEditor();
//        }

//        public System.Data.DataRow GetRowByHandle(int handle)
//        {
//            return GetMainView().GetDataRow(handle);
          
//        }

//        private void VExportToXlsx(string fullpath, string caption)
//        {
            
           
//            var view = GetMainView();
//            var opts = ExcelPrintOptions.Default;
//            opts.NeedPostProcess = false;


            


//            Printing.Print(view, caption, fullpath, prepare: false, excelPrintOptions: opts);

//        }

//        public void ExportToXlsx(string fullpath, bool dxExport, string caption = null)
//        {

            
//            var view = GetMainView();
//            var table_name = view.ViewCaption;
//            if (dxExport)
//            {
//                view.BeginUpdate();
//                if (!string.IsNullOrEmpty(caption))
//                {
//                    view.ViewCaption = caption;
//                    view.OptionsView.ShowViewCaption = true;
//                }
//            }

           
            

//            GridColumn colDummy = view.Columns.FirstOrDefault(c => c.Name == GridDesigner.GetDummyColumnName());
//            GridBand bandDummy = null;
//            if (colDummy != null)
//            {

//                if (colDummy is BandedGridColumn)
//                {
//                    bandDummy = (colDummy as BandedGridColumn).OwnerBand;
//                }
//                colDummy.Visible = false;
//            }
//            if (bandDummy != null)
//            {
//                bandDummy.Visible = false;
//            }


          
//            if (dxExport)
//            {
//                DevExpress.Export.ExportSettings.DefaultExportType = DevExpress.Export.ExportType.WYSIWYG;

//                var options = new DevExpress.XtraPrinting.XlsxExportOptions();
//                ExportToXlsx(fullpath, options);
//                view.OptionsView.ShowViewCaption = false;
//                view.ViewCaption = table_name;
//            }
//            else
//            {
//                VExportToXlsx(fullpath,caption);
//            }
            

         
//            if (colDummy != null) colDummy.Visible = true;
//            if (bandDummy != null)
//            {
//                bandDummy.Visible = true;
//            }
//            if (dxExport)
//            {
//                view.EndUpdate();
//            }
//        }
//        public void SetAllowCellMerge(bool value)
//        {
//            GetMainView().OptionsView.AllowCellMerge = value;
//        }


//        public object[] GetColumns(object view=null)
//        {
//            var view1 = view as GridView;
//            if (view1 == null)
//            {
//                view1 = GetMainView();
//            }
//            return view1.Columns.Cast<object>().ToArray();
//        }

//        public object[] GetViews()
//        {
//            return this.ViewCollection.Cast<object>().ToArray() ;
//        }

//        public string GetColumnFieldName(object column)
//        {
//            return getColumn(column).FieldName;
//        }

//        public string GetColumnName(object column)
//        {
//            return getColumn(column).Name;
//        }

//        public string GetViewName(object view)
//        {
//            return (view as GridView).Name;
//        }

//        public bool IsColumnSumSummaryType(object column)
//        {
//            return getColumn(column).SummaryItem.SummaryType == SummaryItemType.Custom;

//        }

//        public void SetColumnCustomSummaryType(object column)
//        {
//            getColumn(column).SummaryItem.SummaryType = SummaryItemType.Custom;
//        }

//        public void SetColumnSumSummaryType(object column)
//        {
//            getColumn(column).SummaryItem.SummaryType = SummaryItemType.Sum;
//        }


//        public void ClearSelection()
//        {
//           // XtraMessageBox.Show("1");
//            GetMainView().ClearSelection();
//          //  XtraMessageBox.Show("2");
//        }

//        public void SelectRow(System.Data.DataRow row)
//        {
//            GetMainView().SelectRow(GetMainView().GetRowHandle(row.Table.Rows.IndexOf(row)));
//        }

//        public void SetFocusedRow(System.Data.DataRow row)
//        {
//            GetMainView().FocusedRowHandle = GetMainView().GetRowHandle(row.Table.Rows.IndexOf(row));
//        }
//        public System.Data.DataRow GetFocusetDataRow()
//        {
//          return  GetMainView().GetFocusedDataRow();
//        }
//        public int GetColumnWidth(object column)
//        {
//            return getColumn(column).Width;
//        }

//        public void SetFocusedCell(string columnName, System.Data.DataRow row)
//        {
//            GetMainView().CloseEditor();
//            GetMainView().FocusedColumn = GetMainView().VisibleColumns[0];// не перерисовывались кнопки
//            GetMainView().FocusedColumn = GetMainView().Columns[columnName];
           
//            GetMainView().ShowEditor();
//        }
//        public void DxSetEditingValEqFocusedVal()
//        {
//            var view = GetMainView();

//            if (view.IsEditing)
//            {
//                view.EditingValue = view.GetFocusedValue();
//            }
//        }

//        public DataRow[] GetSelectedRows()
//        {
//            var view = this.GetMainView();
//            DataRow[] list = view.GetSelectedRows().Select(view.GetDataRow);
//            return list;
//        }

//        public int[] GetSelectedRowsHandles()
//        {
//          return   GetMainView().GetSelectedRows();
//        }

//        public int GetRowsCount()
//        {
//            return GetMainView().RowCount;
//        }

//        public void SetColumnAllowSort(object column, bool value)
//        {
//            if (value)
//            {
//                getColumn(column).OptionsColumn.AllowSort = DefaultBoolean.True;
//            }
//            else
//            {
//                getColumn(column).OptionsColumn.AllowSort = DefaultBoolean.False;
//            }
//        }
//        /////////// временные
//        public GridView GetMainView() // сделать приватным
//        {
//            return this.MainView as GridView; 
//        }







//        public void SetSelectionMode(int mode)
//        {
//            foreach (GridView view in ViewCollection)
//            {
//                if (mode == 0)
//                {
//                    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
//                }
//                else if (mode == 1)
//                {
//                    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;
//                    view.OptionsView.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowAlways;
//                    view.OptionsNavigation.AutoFocusNewRow = true;
//                    view.OptionsBehavior.Editable = true;
//                    view.OptionsBehavior.ReadOnly = false;
//                }
//                else if (mode == 2)
//                {
//                    view.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
//                    view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CheckBoxRowSelect;
//                }
            
               
//            }
//        }

//        public void SetMultiSelect(bool value)
//        {
//            foreach (GridView view in ViewCollection)
//            {
               
//                view.OptionsSelection.MultiSelect = value;

//            }
//        }

//        public void SetShowFooter(bool value)
//        {
//            foreach (GridView view in ViewCollection)
//            {
//                view.OptionsView.ShowFooter = value;
//                view.UpdateTotalSummary();// может быть нужно вынести отдельно
//            }
//        }


//        //public void SetColumnSort(object column, bool ascending)
//        //{
//        //    if (ascending)
//        //    {
//        //        getColumn(column).SortOrder = ColumnSortOrder.Ascending;
//        //    }
//        //    else
//        //    {
//        //        getColumn(column).SortOrder = ColumnSortOrder.Descending;
//        //    }
         
//        //}

//        public void SetColumnSortIndex(object column, int value)
//        {
//            getColumn(column).SortIndex = value;
//        }


//        public void BeginSort()
//        {
//            var view = GetMainView();
//            view.BeginUpdate();
//            view.BeginSort();
           
//        }

//        public void EndSort()
//        {
//            var view = GetMainView();
           
//            view.EndSort();
//            view.EndUpdate();
//        }

//        public void FitHeaderHeight(object view)
//        {
//            var bview = (view as BandedGridView);
//            if (bview == null) return;
//            int max_height = 80;
//            int nWidthDelta = 14;
//            int nHeightDelta = 0;

//            int nHeightHeader = 13;
//            int nSubtrahend = 23;
//            int nWidth = 0;
//            int nHeight = 0;

//            var stringFormat = new System.Drawing.StringFormat() { Trimming = System.Drawing.StringTrimming.None };

//            foreach (GridColumn col in bview.Columns)
//            {
//                if (!col.Visible) continue;

//                nWidth = col.Width > nSubtrahend ? col.Width - nSubtrahend : col.Width;

//                nHeight = (int)col.AppearanceHeader.CalcTextSize(bview.GridControl.CreateGraphics(), stringFormat, col.Caption, nWidth + nWidthDelta).Height;
//                if (nHeight > max_height) nHeight = max_height;

//                nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//            }

//            nHeightDelta = (nHeightHeader == 13) ? 0 : 6;
//            bview.ColumnPanelRowHeight = nHeightHeader + nHeightDelta;

           
//            if (bview != null)
//            {
//                nHeightHeader = 13;
//                foreach (GridBand band in bview.Bands)
//                {
//                    if (!band.Visible) continue;

//                    nSubtrahend = 23;
//                    nWidth = band.Width > nSubtrahend ? band.Width - nSubtrahend : band.Width;

//                    nHeight = (int)band.AppearanceHeader.CalcTextSize(bview.GridControl.CreateGraphics(), stringFormat, band.Caption, nWidth + nWidthDelta).Height;
//                    if (nHeight > max_height) nHeight = max_height;

//                    nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//                }

//                nHeightDelta = (nHeightHeader == 13) ? 0 : 6;
//                bview.BandPanelRowHeight = nHeightHeader + nHeightDelta;
//            }
//        }


//        public void SetEnableMasterViewMode(bool value)
//        {
//            foreach (var v in ViewCollection.Cast<GridView>())
//            {
             
//                v.OptionsDetail.EnableMasterViewMode = value;
//            }
//        }

//        public void SetShowDetailTabs(bool value)
//        {
//            foreach (var v in ViewCollection.Cast<GridView>())
//            {
//                v.OptionsDetail.ShowDetailTabs = value;
           
//            }
//        }

//        public void ControlForceInitialize()
//        {
//            this.ForceInitialize();
//        }

//        public void DisposeViews()
//        {
//            foreach (var gv in ViewCollection.Cast<GridView>()) gv.Dispose();
//        }

//        public void DxRefreshViewData()
//        {
//            foreach (GridView view in ViewCollection)
//            {
//                view.RefreshData();
//            }
//        }

//        public void DxRefreshRow(int handle)
//        {

//            GetMainView().RefreshRow(handle);
            
//        }


//        public bool GetColumnVisible(object column)
//        {
//            return getColumn(column).Visible;
//        }

//        public int GetColumnVisibleIndex(object column)
//        {
//            return getColumn(column).VisibleIndex;
//        }

//        public string GetColumnSortOrder(object column)
//        {
//            return getColumn(column).SortOrder.ToString().ToLower();
//        }

//        public int GetColumnGroupIndex(object column)
//        {
//            return getColumn(column).GroupIndex;
//        }

//        public void SetColumnVisibleIndex(object column, int value)
//        {
//			if (column is BandedGridColumn) (column as BandedGridColumn).OwnerBand.Columns.MoveTo(value, column as BandedGridColumn);
//            else getColumn(column).VisibleIndex = value;
//        }
//        //public void SetColumnGroupIndex(object column, int value)
//        //{
//        //    getColumn(column).GroupIndex = value;
//        //}



//        public void DxUpdateSummary()
//        {
//            foreach (GridView view in ViewCollection)
//            {
//                view.UpdateSummary();
//            }
//        }

//        public void DxExpandAllGroups()
//        {
//            foreach (GridView view in ViewCollection)
//            {
//                view.ExpandAllGroups();
//            }
//        }

//        public object GetViewByName(string viewName)
//        {
//            return getViewByName(viewName);
//        }

//        public void UpdateDummyColumnWidth()
//        {
//            foreach (GridView view in this.ViewCollection)
//            {
//                GridColumn colDummy = view.Columns.ColumnByName(GridDesigner.GetDummyColumnName());
//                if (colDummy == null) return;

//                GridViewInfo info = view.GetViewInfo() as GridViewInfo;
//                int width_free = info.ViewRects.ColumnPanelWidth - info.ViewRects.ColumnTotalWidth + colDummy.Width - 20;// 20 чтобы не глючило при порявлении скрола
//                int width_min = GridDesigner.GetMinDummyWidth();

//                colDummy.Width = (width_free < width_min) ? width_min : width_free;
//            }
//        }

//        public void PrepareForData()
//        {
           
//        }

//        public void PrepareForList()
//        {
           
//        }

//        public Dictionary<string, int> GetColumnsWidth()
//        {
//            throw new NotImplementedException();
//        }
        
//        public void SetColumnEditable(object column, bool value)
//        {
//            throw new NotImplementedException();
//        }
//        public void SetShowRoot(bool value)
//        {
//            throw new NotImplementedException();
//        }
//        public void SetColumnAutoFilterCondition_BeginsWith(object column)
//        {
//            getColumn(column).OptionsFilter.AutoFilterCondition = AutoFilterCondition.BeginsWith;
//        }

//        public void SetColumnAutoFilterCondition_Contains(object column)
//        {
//            getColumn(column).OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
//        }


//        public void DxLockReloadNodes()
//        {
         
//        }

//        public void DxUnlockReloadNodes()
//        {
           
//        }


//        public void DxEndCurrentEdit()
//        {
            
//        }

//        public void InitColumnsEditors()
//        {
           
//        }


//        public void SetColumnEditor(object column, IVCheckContainer editor)
//        {
//            //throw new NotImplementedException();
//        }

//		private DevExpress.XtraWaitForm.ProgressPanel loadingControl;
//		int _loadDataCounter = 0;
//		public void ShowLoadingControl()
//		{
//			if (_loadDataCounter == 0)
//			{
//				this.loadingControl.Show();

//			}
//			_loadDataCounter++;
			
//		}
//		public void HideLoadingControl()
//		{
//			if (_loadDataCounter > 0)
//			{
//				_loadDataCounter--;
//			}

//			if (_loadDataCounter == 0)
//			{
//				this.loadingControl.Hide();
//			}
//		}

//		//не очень нужная штука, чтобы не перекрывалась шапка таблицы, можно заменить на просто this.Height - const (40, например)
//		private void setLoadingControlHeight(object sender, EventArgs e)
//		{
//			int maxHeaderHeight = 0;
//			foreach (GridView v in ViewCollection.Cast<GridView>().ToArray())
//			{
//				GridViewInfo vi = v.GetViewInfo() as GridViewInfo;
//				maxHeaderHeight = maxHeaderHeight > vi.ColumnRowHeight + 2 ? maxHeaderHeight : vi.ColumnRowHeight + 2;
//			}
//			loadingControl.Height = this.Height - maxHeaderHeight;
//		}




//        public void DefaultSelection()
//        {
           
//        }
//		public void ResetFocus()
//		{
//			if (this.GetTopTable().Rows.Count > 0)
//			{
//				ClearSelection();
//				SetFocusedCell(GetColumnName(GetColumns()[0]), null);
//				SetFocusedRow(GetTopTable().Rows[0]);
//			}
//		}
//    }
//}
