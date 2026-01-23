//using System;
//using System.Collections.Generic;
//using System.Linq;
////using System.Windows.Forms;

//using DevExpress.Utils;


//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;
//using Clipboard = System.Windows.Clipboard;

//using DevExpress.XtraTreeList;
//using DevExpress.XtraTreeList.Columns;
//using DevExpress.XtraTreeList.Nodes;
//using DevExpress.XtraTreeList.ViewInfo;
//using DevExpress.XtraTreeList.Data;

//using DevExpress.XtraEditors.Repository;
//namespace sql.builder.Controls.Grids.ReportViewModes
//{

//    internal partial class ucTreeWF : TreeList, IucGrid
//    {

//        public ucTreeWF()
//        {
            
//           // this.ToolTipController = this.tooltip;
//           // this.AfterDropNode += new DevExpress.XtraTreeList.AfterDropNodeEventHandler(this_AfterDropNode);
            
            
//        }

//        public void SetShowAutoFilterRow(bool value)
//        {
//            this.OptionsView.ShowAutoFilterRow = value;
//        }
//		public void SetAllowSelectMoveColumns(bool value)
//		{
//			this.OptionsCustomization.AllowColumnMoving = XmlReports.IsDeveloperMode() || value;
//		}

//        public Dictionary<string, int> GetColumnsWidth()
//        {
//            var res = new Dictionary<string, int>();
//            foreach (TreeListColumn col in this.Columns)
//            {
//                if (!res.ContainsKey(col.FieldName)) // была ошибка при закрытии окон в редакторе схемы
//                {
//                    res.Add(col.FieldName, col.Width);
//                }
//            }
//            return res;
//        }

//        public void SetColumnEditable(object column, bool value)
//        {
//            var col = getColumn(column);
//            //if (col == null) return;
//            col.OptionsColumn.AllowEdit = value;
//        }

//        public Dictionary<string, string> GetFilterValues()
//        {
//            var res = new Dictionary<string, string>();
//            foreach (TreeListColumn col in this.VisibleColumns)
//            {
//                if (col.FieldName == "check") continue;

//#if DX15
//                var val = tree.Nodes.AutoFilterNode[col.FieldName]; 
//#else
//                var val = col.FilterInfo.Value;
//#endif
//                //

//                if (val != null)
//                {
//                    res.Add(col.FieldName, val.ToString());
//                }
//            }
//            return res;
//        }




//        public void PrepareForData()
//        {
//            this.OptionsView.ShowBandsMode = DefaultBoolean.False;
//            this.Appearance.BandPanel.Options.UseTextOptions = true;
//            this.Appearance.BandPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
//            this.Appearance.BandPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.Appearance.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.Appearance.Caption.Options.UseTextOptions = true;
//            this.Appearance.Caption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.Appearance.Caption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.Appearance.Caption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.Appearance.HeaderPanel.Options.UseTextOptions = true;
//            this.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
//            this.AppearancePrint.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.AppearancePrint.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.AppearancePrint.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.Cursor = System.Windows.Forms.Cursors.Default;
//            this.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.Location = new System.Drawing.Point(0, 39);
//            this.Name = "tree";
//            this.OptionsBehavior.AllowExpandOnDblClick = false;
//            this.OptionsBehavior.AutoPopulateColumns = false;
//            this.OptionsBehavior.Editable = false;
//            this.OptionsBehavior.EnableFiltering = true;
//            this.OptionsBehavior.KeepSelectedOnClick = false;
//            this.OptionsBehavior.ReadOnly = true;
//            this.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Extended;
//            this.OptionsFilter.ShowAllValuesInFilterPopup = true;
//            this.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.OptionsSelection.MultiSelect = true;
//            this.OptionsSelection.MultiSelectMode = DevExpress.XtraTreeList.TreeListMultiSelectMode.CellSelect;
//            this.OptionsSelection.UseIndicatorForSelection = true;
//            this.OptionsView.AutoWidth = false;
//            this.OptionsView.ShowSummaryFooter = true;
//            this.ParentFieldName = "";
//            this.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowOnlyInEditor;
//            this.Size = new System.Drawing.Size(786, 315);
//            this.TabIndex = 11;
//            this.OptionsBehavior.PopulateServiceColumns = true;
//            KeyDown += (sender, args) =>
//            {
//                if (args.Control && args.KeyCode == Keys.C)
//                {


//                    Clipboard.SetText(this.FocusedValue.ToString());
//                    args.Handled = true;
//                }
//            };

//        }
//        public void SetColumnAutoFilterCondition_BeginsWith(object column)
//        {
//            getColumn(column).OptionsFilter.AutoFilterCondition = AutoFilterCondition.BeginsWith;
//        }

//        public void SetColumnAutoFilterCondition_Contains(object column)
//        {
//            getColumn(column).OptionsFilter.AutoFilterCondition = AutoFilterCondition.Contains;
//        }

//        public void PrepareForList()
//        {
//            this.Appearance.HeaderPanel.Options.UseTextOptions = true;
//            this.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.Cursor = System.Windows.Forms.Cursors.Default;
//            this.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.KeyFieldName = "";
//            this.Location = new System.Drawing.Point(3, 3);
//            this.Name = "tree";
//            this.OptionsBehavior.AutoSelectAllInEditor = false;
//            this.OptionsBehavior.EnableFiltering = true;
//            this.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Smart;
//            this.OptionsFilter.ShowAllValuesInFilterPopup = true;
//            this.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.OptionsSelection.MultiSelect = true;
//            //this.OptionsSelection.UseIndicatorForSelection = true;
//            this.OptionsView.ShowAutoFilterRow = false;
//            this.OptionsView.ShowColumns = false;
//            this.OptionsView.ShowIndicator = false;
//            this.OptionsView.ShowRoot = false;
//            this.ParentFieldName = "";
            
//            this.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowAlways;
//            this.Size = new System.Drawing.Size(369, 244);
//            this.TabIndex = 2;

//        }
//        public void ExpandAllNodes()
//        {
//            this.DxUnlockReloadNodes();
//            this.ForceInitialize();

//            this.ExpandAll();
//        }

        
//        public void SetParentFieldName(string value)
//        {
//            this.ParentFieldName = value;
//            if (!string.IsNullOrEmpty(value))
//            {
//                this.OptionsFilter.FilterMode = FilterMode.Extended;
//            }
//        }

//        public void SetShowRoot(bool value)
//        {
            
//            this.OptionsView.ShowRoot = value;

//        }
//        public void SetKeyFieldName(string value)
//        {
//            this.KeyFieldName = value;

//        }
//        public void Clear()
//        {
//            // от утечек
//            // https://www.devexpress.com/Support/Center/Question/Details/Q534989

//            //foreach (var v in ViewCollection.Cast<GridView>().ToArray()) v.Dispose();
//            //_views.Clear();
//        }




//       // private Dictionary<string, object> _views = new Dictionary<string, object>();
//        public void CreateView(string name, bool banded)
//        {
//            //GridView view = (banded) ? new BandedGridView() : new GridView();
//            //GridDesigner.SetViewSettings(view);
//            //view.Name = name;
//            //this.ViewCollection.Add(view);
//            //if (this.MainView == null) this.MainView = view;
//            //_views.Add(name, view);
//        }

        
        
//        public void BeginViewUpdate(string name)
//        {
//            this.BeginUpdate();
           
//        }
//        public void EndViewUpdate(string name)
//        {
//            this.EndUpdate();
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
//            this.Caption = value;
//        }


//        public void SetMainView(string name)
//        {
            
//        }


//        public void ClearViewContent(string name)
//        {
//            this.Columns.Clear();
//            this.Bands.Clear();
//        }


       

//        private TreeListBand getBand(object band)
//        {

//            return band as TreeListBand;
//        }

//        private TreeListColumn getBandColumn(object column)
//        {
//            return column as TreeListColumn;
//        }

//        private TreeListColumn getColumn(object column)
//        {
//            return column as TreeListColumn;
//        }

//        public object CreateBandColumn()
//        {
//            return CreateColumn();
           
          
//        }
//        public void AddColumnToBand(object band,object column)
//        {

//             getBand(band).Columns.Add(getBandColumn(column));

            
//        }


//        public void SetShowBands(string viewName, bool value)
//        {
//            this.OptionsView.ShowBandsMode = DefaultBoolean.True;
           
      
//        }


//        public void AddBandedDummyColumnAndHideEmptyBands(string viewName)
//        {
           
//            var colLast = this.Columns.LastOrDefault() ;
//            if (colLast != null)
//            {
//                // только если нет бэндов, разобраться как сделать для дерева

//                var band = new TreeListBand();
//                this.Bands.Add(band);

//                var colDummy = GridDesigner.CreateDummyTreeColumn();
//                this.Columns.Add(colDummy);
//                band.Columns.Add(colDummy);
              
//            }

//            // прячем бэнды без видимых элементов
//            foreach (TreeListBand band in this.Bands)
//            {
//                if (String.IsNullOrEmpty(band.Caption))
//                {
//                    if (band.Columns.VisibleCount == 0 && band.Bands.VisibleCount == 0)// какой то бред, но оставлю, так было
//                    {
//                        band.Visible = band.Columns.VisibleCount > 0 || band.Bands.VisibleCount > 0;
//                    }
//                }
//            }
//        }


//        public object CreateBand()
//        {
//            var band = new TreeListBand();
//            band.AppearanceHeader.Options.UseTextOptions = true;
//            band.AppearanceHeader.TextOptions.WordWrap = WordWrap.Wrap;
//            band.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
  
//            return band;
//        }
//        public void AddBandToView(string viewName, object band)
//        {
//            this.Bands.Add(getBand(band));
//        }

     
//        public void AddBandToBand(object parenBand, object childBand)
//        {
//            var band = new TreeListBand();
//            band.AppearanceHeader.Options.UseTextOptions = true;
//            band.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            getBand(parenBand).Bands.Add(getBand(childBand));
         
//        }

//        public object CreateColumn()
//        {
//           var col=new TreeListColumn();
//           col.OptionsColumn.AllowEdit = true;
//           col.AppearanceHeader.Options.UseTextOptions = true;
//           col.AppearanceHeader.TextOptions.HAlignment =DevExpress.Utils.HorzAlignment.Center;

//           return col;
//        }


//        public void AddColumnToView(string viewName, object column)
//        {
//           this.Columns.Add( getColumn( column));
//        }


//        /*public void AddDummyColumn(string viewName)
//        {
//            //// чтобы удобно было менять ширину последней колонки
//            var colDummy = GridDesigner.CreateDummyTreeColumn();
//            this.Columns.Add(colDummy);
//        }*/


//        public void SetParentView(string childViewName, string parentViewName)
//        {
//            //GetViewByName(childViewName).ParentView = GetViewByName(parentViewName);
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
//            band1.Visible = band1.Columns.VisibleCount > 0 || band1.Bands.VisibleCount > 0;
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
//            var view = this as TreeList;
//            switch (type)
//            {
//                case "number":
//                    column.UnboundType = UnboundColumnType.Decimal;
//                    column.Format.FormatType = FormatType.Numeric;
//                    //     column.SummaryItem.SummaryType = SummaryItemType.Sum;

//                    format = format ?? "n2";

//                    //???
//                    //column.SummaryFooterStrFormat = (format.StartsWith("{0:") ? format : string.Format("{{0:{0}}}", format));

//                    break;
//                case "date":
//                    column.UnboundType = UnboundColumnType.DateTime;
//                    //???
//                    //column.DisplayFormat.FormatType = FormatType.DateTime;
//                    //column.DisplayFormat.FormatString = "dd.MM.yyyy";
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
//                        column.RowFooterSummary = summary_type;

//                        //view.GroupSummary.Add(new GridGroupSummaryItem(summary_type, column.FieldName, column, (format.StartsWith("{0:") ? format : string.Format("{{0:{0}}}", format))));
//                    }

//                }
//                else
//                {
//                    column.RowFooterSummary = SummaryItemType.Sum;
//                    //view.GroupSummary.Add(new GridGroupSummaryItem(SummaryItemType.Sum, column.FieldName, column, (format.StartsWith("{0:") ? format : string.Format("{{0:{0}}}", format))));
//                }
//            }

//            if (format != null)
//            {
//                column.Format.FormatString = format;
//            }
//        }


//        public void SetColumnTitle(object column, string value)
//        {
//            getColumn(column).Caption = value;
//        }

//        public void SetColumnTitle(string tableName,string  columnName, string value)
//        {
//            var col = this.Columns.ColumnByFieldName(columnName);
//            if (col != null)
//            {
//                col.Caption = value;
//            }
//            else
//            {
//                this.Bands.Where(b => b.Name == columnName).First().Caption = value;
//            }
//        }

//        public void SetColumnVisible(object column, bool value)
//        {
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
//                return;
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
//           return this.VisibleColumns.Select(c => c.FieldName).ToArray();
//        }


//        public void SetColumnGroupIndex(object column, int value)
//        {
           
//           // getColumn(column).GroupIndex = value;
//        }

//        public void SetColumnSortOrder(object column, string value)
//        {
//            switch (value)
//            {

//                case "ascending": getColumn(column).SortOrder = SortOrder.Ascending; break;
//                case "descending": getColumn(column).SortOrder = SortOrder.Descending; break;
//                default: getColumn(column).SortOrder = SortOrder.None; break;
//            }
//        }

//        public object GetColumnByFieldName(string tableName, string columnName)
//        {
//            var col = this.Columns.ColumnByFieldName(columnName);
//            return col;
//        }
//        public object GetColumnByFieldName(string columnName)
//        {
//            return this.Columns.ColumnByFieldName(columnName);
//        }
//        public void EndEdit()
//        {
           
//            var view = this as TreeList;
//            //view.PostEditor();
//            //view.UpdateCurrentRow();
//            view.CloseEditor();
            
//        }

//        public void CloseEditor()
//        {
//            //var view = GetMainView();
//            //if (view == null) return;
//            var view = this as TreeList;
//            view.CloseEditor();
//        }

//        public System.Data.DataRow GetRowByHandle(int handle)
//        {
//            var view = this as TreeList;
//            return view.GetDataRow(handle);
//           // return GetMainView().GetDataRow(handle);
          
//        }


//        public void ExportToXlsx(string fullpath, bool dxExport, string caption = null)
//        {
//             base.ExportToXlsx(fullpath);
//        }
//        public void SetAllowCellMerge(bool value)
//        {
//            //GetMainView().OptionsView.AllowCellMerge = value;
//        }


//        public object[] GetColumns(object view=null)
//        {
//            var view1 = this as TreeList;
            
//            return view1.Columns.Cast<object>().ToArray();
//        }

//        public object[] GetViews()
//        {
//            var l = new List<object>();
//            l.Add(this);
//            return l.ToArray();
            
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
//            return this.Name;
//        }

//        public bool IsColumnSumSummaryType(object column)
//        {
//            return getColumn(column).RowFooterSummary ==  SummaryItemType.Custom;

//        }

//        public void SetColumnCustomSummaryType(object column)
//        {
//            getColumn(column).RowFooterSummary = SummaryItemType.Custom;
//        }

//        public void SetColumnSumSummaryType(object column)
//        {
//            getColumn(column).RowFooterSummary = SummaryItemType.Sum;
//        }


//        public void ClearSelection()
//        {
//            var v=this as TreeList;
//            v.Selection.Clear();
//            //???
//        }

//        public void DefaultSelection()
//        {
//            var v = this as TreeList;

//            // фокус есть всегда
//            //v.FocusedNode = null;
//            //if (v.FocusedNode == null)
//            //{
//            //    v.FocusedNode = v.Nodes.FirstNode;
              
//            //}

//            if (!v.Selection.Any())
//            {
              
//                v.SelectNode(v.FocusedNode);
//                GetTopTable().CurrentRow = GetNodeData(this.FocusedNode);
//            }

           
          
//            //v.SelectNode(Cmn.GetNodeByRowRecursive(v, row));
//            ////???
//        }

//        public void SelectRow(System.Data.DataRow row)
//        {
//            var v = this as TreeList;
//            v.SelectNode(Cmn.GetNodeByRowRecursive(v, row));
//            //???
//        }

//        public void SetFocusedRow(System.Data.DataRow row)
//        {
//            //if (row == null)
//            //{
//            //    var v = this as TreeList;
//            //    v.FocusedNode = null;
//            //    v.Selection.Clear();
//            //}
//        }
//        public System.Data.DataRow GetFocusetDataRow()
//        {
//            return (this as TreeList).GetFocusedDataRow();
//        }
//        public int GetColumnWidth(object column)
//        {
//            return getColumn(column).Width;
//        }

//        public void SetFocusedCell(string columnName, System.Data.DataRow row)
//        {
//           //???
//        }
//        public void DxSetEditingValEqFocusedVal()
//        {
//            //???
//        }


//        private TreeList GetMainView()
//        {
//            return this;
//        }
//        public System.Data.DataRow[] GetSelectedRows()
//        {
//            var t=(this as TreeList);
//            var list = new List<System.Data.DataRow>();
//            foreach (TreeListNode n in t.Selection)
//            {
//                list.Add(t.GetDataRow(n.Id));
//            }
//            return list.ToArray();
            
//            //var view = GetMainView();
//            //var list = view.GetSelectedRows().Select(view.GetDataRow).ToArray();
//            //return list;
//        }

//        public int[] GetSelectedRowsHandles()
//        {
//            return GetMainView().Selection.Select(n => n.Id).ToArray();//???
//        }

//        public int GetRowsCount()
//        {
//            return GetMainView().Nodes.Count;
//        }

//        public void SetColumnAllowSort(object column, bool value)
//        {
//            getColumn(column).OptionsColumn.AllowSort = value;
//        }
//        /////////// временные
//        //public GridView GetMainView() // сделать приватным
//        //{
//        //    return this.MainView as GridView; 
//        //}







//        public void SetSelectionMode(int mode)
//        {

//            var view = this as TreeList;
            
//                if (mode == 0)
//                {
                   
//                    view.OptionsSelection.MultiSelectMode = TreeListMultiSelectMode.CellSelect;
//                }
//                else if (mode == 1)
//                {
//                    view.OptionsSelection.MultiSelectMode = TreeListMultiSelectMode.RowSelect;
//                    view.OptionsView.ShowButtons = true;
//                    view.OptionsNavigation.AutoFocusNewNode = true;
//                    view.OptionsBehavior.Editable = true;
//                    view.OptionsBehavior.ReadOnly = false;
//                }
//                else if (mode == 2)
//                {
//                    //view.OptionsSelection.CheckBoxSelectorColumnWidth = 30;
//                    //???
//                    view.OptionsSelection.MultiSelectMode = TreeListMultiSelectMode.RowSelect;
//                }
            
               
            
//        }
//        public void SetMultiSelect(bool value)
//        {
//              var view = this as TreeList;

//                view.OptionsSelection.MultiSelect = value;

            
//        }
//        public void SetShowFooter(bool value)
//        {
//            var view = this as TreeList;
//            view.OptionsView.ShowSummaryFooter = value;
           
//             //???   //view.UpdateTotalSummary();// может быть нужно вынести отдельно
            
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
//            //??? объединить с гридом
//            var bview = GetMainView();
//            if (bview == null) return;
//            int max_height = 80;
//            int nWidthDelta = 14;
//            int nHeightDelta = 0;

//            int nHeightHeader = 13;
//            int nSubtrahend = 23;
//            int nWidth = 0;
//            int nHeight = 0;

//            var stringFormat = new System.Drawing.StringFormat() { Trimming = System.Drawing.StringTrimming.None };

//            foreach (TreeListColumn col in bview.Columns)
//            {
//                if (!col.Visible) continue;

//                nWidth = col.Width > nSubtrahend ? col.Width - nSubtrahend : col.Width;

//                nHeight = (int)col.AppearanceHeader.CalcTextSize(bview.CreateGraphics(), stringFormat, col.Caption, nWidth + nWidthDelta).Height;
//                if (nHeight > max_height) nHeight = max_height;

//                nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//            }

//            nHeightDelta = (nHeightHeader == 13) ? 0 : 6;
//            bview.ColumnPanelRowHeight = nHeightHeader + nHeightDelta;

           
//            if (bview != null)
//            {
//                nHeightHeader = 13;
//                foreach (TreeListBand band in bview.Bands)
//                {
//                    if (!band.Visible) continue;

//                    nSubtrahend = 23;
//                    nWidth = band.Width > nSubtrahend ? band.Width - nSubtrahend : band.Width;

//                    nHeight = (int)band.AppearanceHeader.CalcTextSize(bview.CreateGraphics(), stringFormat, band.Caption, nWidth + nWidthDelta).Height;
//                    if (nHeight > max_height) nHeight = max_height;

//                    nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//                }

//                nHeightDelta = (nHeightHeader == 13) ? 0 : 6;
//                bview.BandPanelRowHeight = nHeightHeader + nHeightDelta;
//            }
//        }


//        public void SetEnableMasterViewMode(bool value)
//        {
//            //foreach (var v in ViewCollection.Cast<GridView>())
//            //{
             
//                //v.OptionsDetail.EnableMasterViewMode = value;
//            //}
//        }

//        public void SetShowDetailTabs(bool value)
//        {
//            //foreach (var v in ViewCollection.Cast<GridView>())
//            //{
//            //    v.OptionsDetail.ShowDetailTabs = value;
           
//            //}
//        }

//        public void ControlForceInitialize()
//        {
//            this.ForceInitialize();
//        }

//        public void DisposeViews()
//        {
//            //foreach (var gv in ViewCollection.Cast<GridView>()) gv.Dispose();
//        }

//        public void DxRefreshViewData()
//        {
//          //???
//            //foreach (GridView view in ViewCollection)
//            //{
//            //    view.RefreshData();
//            //}
//        }

//        public void DxRefreshRow(int handle)
//        {
//            //???
//            //GetMainView().RefreshRow(handle);
            
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
//            return 0;
//        }

//        public void SetColumnVisibleIndex(object column, int value)
//        {
//			if (getColumn(column).ParentBand != null) getColumn(column).ParentBand.Columns.SetColumnIndex(value, getColumn(column));
//			else getColumn(column).VisibleIndex = value;
//        }
//        //public void SetColumnGroupIndex(object column, int value)
//        //{
//        //    getColumn(column).GroupIndex = value;
//        //}



//        public void DxUpdateSummary()
//        {
//            //foreach (GridView view in ViewCollection)
//            //{
//            //    view.UpdateSummary();
//            //}
            
//        }

//        public void DxExpandAllGroups()
//        {
//            //foreach (GridView view in ViewCollection)
//            //{
//            //    view.ExpandAllGroups();
//            //}
//        }


//        public object DxGetListSourceRowCellValue(object row, string columnName)
//        {
//            return (row as TreeListNode)[columnName];
//        }

//        public object GetViewByName(string viewName)
//        {
//            return this;
//        }

//        public void UpdateDummyColumnWidth()
//        {

//            var tree = (this as TreeList);
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
//                colDummy.Width = width_free;

//            }
//        }

//        private bool _nodesLocked = false;

//        public event CellEventHandler CellDoubleClick;

//        public void DxLockReloadNodes()
//        {
//            if (!_nodesLocked)
//            {
//                _nodesLocked = true;
//                this.LockReloadNodes();
//            }

//        }

//        public void DxUnlockReloadNodes()
//        {
//            if (_nodesLocked)
//            {
//                _nodesLocked = false;
//                this.UnlockReloadNodes();
//            }
//        }


//        public void DxEndCurrentEdit()
//        {
//            this.EndCurrentEdit();
//        }



//        public void InitColumnsEditors()
//        {
         

//            foreach (TreeListColumn column in this.Columns)
//            {
//                if (column.ColumnType == typeof(DateTime))
//                {
//                    var rep = new RepositoryItemDateEdit();
//                    rep.Buttons[0].Visible = false;
//                    column.ColumnEdit = rep;
//                }
//                else if (column.ColumnType == typeof(String))
//                {
//                    var rep = new RepositoryItemTextEdit();
//                    column.ColumnEdit = rep;
//                }
//            }
//        }


//        public void SetColumnEditor(object column, IVCheckContainer editor)
//        {
//           // throw new NotImplementedException();
//        }

//		public void ShowLoadingControl()
//		{
//			//
//		}
//		public void HideLoadingControl()
//		{
//			//
//		}
//		public void ResetFocus()
//		{
//			//
//		}
//    }
//}
