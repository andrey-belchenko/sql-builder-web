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
//using DevExpress.XtraVerticalGrid.Native;
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
//        List<int> eventsAttached = new List<int>();

        
//        private void AttachViewEvents_Grid()
//        {
          
//            if (isWeb()) return;
//            var views = grid_temp.ViewCollection.Cast<GridView>();
         
//            var eventsAttachedNew = new List<int>();

            
           
//            foreach (GridView view in views)
//            {
//                if (!eventsAttached.Contains(view.GetHashCode()))
//                {

//                    //view.CustomRowFilter += view_CustomRowFilter;
//                    view.MouseDown += view_MouseDown;
//                  //  view.DoubleClick += view_DoubleClick;
//                    view.CustomDrawCell += view_CustomDrawCell;
//                    view.CustomSummaryCalculate += view_CustomSummaryCalculate;
//                    //view.FocusedRowChanged += view_FocusedRowChanged;
//                    view.CustomRowCellEdit += view_CustomRowCellEdit;
//                    view.ValidatingEditor += view_ValidatingEditor;
//                    view.CustomDrawRowIndicator += view_CustomDrawRowIndicator;
//                    view.BeforeLeaveRow += view_BeforeLeaveRow;
//                    view.ColumnWidthChanged += view_ColumnWidthChanged;
//                    view.TopRowChanged += view_TopRowChanged;
//                    view.StartSorting += view_StartSorting;
                  
//                    view.ColumnFilterChanged += view_ColumnFilterChanged;
                   
//                    view.CellMerge += view_CellMerge;
//                    view.CustomColumnDisplayText += view_CustomColumnDisplayText;


                    
//                    BandedGridView bview = view as BandedGridView;
//                    if (bview != null)
//                    {
//                        bview.BandWidthChanged += view_BandWidthChanged;
//                    }
//                }
//                else
//                {

//                }
//                eventsAttachedNew.Add(view.GetHashCode());
//            }
//            eventsAttached = eventsAttachedNew;
//        }

        
//        private void DetachViewEvents_Grid()
//        {
           

//            if (isWeb()) return;
//            var views = grid_temp.ViewCollection.Cast<GridView>();

         
//            foreach (GridView view in views)
//            {
//                //view.CustomRowFilter -= view_CustomRowFilter;
//                view.MouseDown -= view_MouseDown;
//               // view.DoubleClick -= view_DoubleClick;
//                view.CustomDrawCell -= view_CustomDrawCell;
//                view.CustomSummaryCalculate -= view_CustomSummaryCalculate;
//                //view.FocusedRowChanged -= view_FocusedRowChanged;
//                view.CustomRowCellEdit -= view_CustomRowCellEdit;
//                view.ValidatingEditor -= view_ValidatingEditor;
//                view.CustomDrawRowIndicator -= view_CustomDrawRowIndicator;
//                view.BeforeLeaveRow -= view_BeforeLeaveRow;
//                view.ColumnWidthChanged -= view_ColumnWidthChanged;
//                view.TopRowChanged -= view_TopRowChanged;
//                view.StartSorting -= view_StartSorting;
//                view.ColumnFilterChanged -= view_ColumnFilterChanged;
              
//                view.CellMerge -= view_CellMerge;
//                view.CustomColumnDisplayText -= view_CustomColumnDisplayText;
//                BandedGridView bview = view as BandedGridView;
//                if (bview != null)
//                {
//                    bview.BandWidthChanged -= view_BandWidthChanged;
//                }
//            }
//            eventsAttached = new List<int>();
//        }

        

//        void view_CustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs e)
//        {
//            if (GetTopTable() == null) return;
//            if (GetTopTable().AllowMerge)
//            {
//                if (e.Column.ColumnType == XmlReports.numberType)
//                {
//                    if (Cmn.Nvl(e.Value, null) != null)
//                    {
//                        if (Cmn.ToDecimal(e.Value) == Cmn.ToDecimal(0))
//                        {
//                            e.DisplayText = "";
//                        }
//                    }
//                }
//            }

//        }

//        void view_CellMerge(object sender, CellMergeEventArgs e)
//        {

//            //e.Merge = true;
//            //e.Handled = true;
//            //var view = GetMainView();

//            //  return;
//            e.Merge = false;
//            e.Handled = true;

//            //return;
//            var row1 = GetGrid().GetRowByHandle(e.RowHandle1);
//            if (row1 == null) return;
//            var row2 = GetGrid().GetRowByHandle(e.RowHandle2);
//            if (row2 == null) return;
//            var vcol = row1.Table.Columns[e.Column.FieldName] as VDataColumn;

//            if (vcol != null)
//            {
//                e.Merge = vcol.IsMerged(row1, row2);
//            }


//        }


//        private void view_ColumnFilterChanged(object sender, EventArgs e)
//        {

//            var table = GetTopTable();
//            if (_mode == ControlMode.Report)
//            {
//                if (table.Reader == null)
//                {
//                    return;
//                }
//            }

         

//            if (table.UseDeferredFetch)
//            {
//                table.FetchTo(int.MaxValue);
//            }
//        }

//        private void view_StartSorting(object sender, EventArgs args)
//        {
            
//            var table = GetTopTable();

//            if (_mode == ControlMode.Report)
//            {
//                if (table.Reader == null)
//                {
//                    return;
//                }
//            }

//            if (table.UseDeferredFetch)
//            {
//                table.FetchTo(int.MaxValue);
//            }
//        }
//        private void view_TopRowChanged(object sender, EventArgs args)
//        {
//            var table = GetTopTable();
//            if (_mode == ControlMode.Report)
//            {
//                if ( (table == null) || (table.Reader == null))
//                {
//                    return;
//                }
//            }

           

//            if (table.UseDeferredFetch)
//            {
//                var view = sender as GridView;

//                // Креss Style Fetch
//                if (view.DataRowCount > 50)
//                {
//                    var vi = (view.GetViewInfo() as GridViewInfo);
//                    if (view.TopRowIndex > view.DataRowCount - vi.RowsInfo.Count - 10)
//                    {
//                        table.FetchNext(100);
//                    }
//                }
//            }
//        }

//        private void view_MouseDown(object sender, MouseEventArgs args)
//        {
//            var view = sender as GridView;
//            GridHitInfo hi = view.CalcHitInfo(args.Location);

//            if (_multiselect && view.OptionsSelection.MultiSelectMode != GridMultiSelectMode.CheckBoxRowSelect)
//            {
//                if (args.Clicks == 1)
//                {
//                    if (args.Button == MouseButtons.Left)
//                    {
//                        view.BeginSelection();

//                        var bhi = hi as BandedGridHitInfo;

//                        if (hi.InColumn)
//                        {
//                            if (Control.ModifierKeys != (Keys.Control)) view.ClearSelection();
//                            SelectCells(hi.Column);
//                        }
//                        else if (bhi != null && bhi.InBandPanel && bhi.Band != null)
//                        {
//                            if (Control.ModifierKeys != (Keys.Control)) view.ClearSelection();
//                            SelectCells(bhi.Band);
//                        }
//                        view.EndSelection();
//                    }
                   
//                }
//            }

//            if (args.Button == MouseButtons.Right)
//            {
//                if (_menu == null || _menu.IsEmpty()) return;

//                _menu.Show();
//            }

//            BandedGridView bview = view as BandedGridView;
//            if (bview != null)
//            {
//                CustomLastColumnResize(bview, args.Location);
//            }
//        }

//        private void view_CustomDrawCell(object sender, RowCellCustomDrawEventArgs args)
//        {
//            var view = sender as GridView;

//            var table=GetTopTable();
            
//            var row = view.GetDataRow(args.RowHandle);
          
//           // var table = row.Table as VDataTable;

//            if (table.ClientCanBeCheckedSource != null && view.OptionsSelection.MultiSelectMode == GridMultiSelectMode.CheckBoxRowSelect && args.Column.VisibleIndex == 0)
//            {
//                if (row == null || !table.GetCanBeChecked(row))
//                {
//                    args.Handled = true;
//                    return;
//                }
//            }
            
//            if (row == null) return;
//            // цвет текста
           

//            // перенес из события RowCellStyle для грида с отчётами
//            var vcol = row.Table.Columns[args.Column.FieldName] as VDataColumn;
//            //if (vcol != null)
//            //{
//            Color color;
//            string rgb = table.GetBackColor(row, vcol);
//            if (VColor.ParseRGB(rgb, out color)) {
//                args.Appearance.BackColor = color;
//            }
//            if (vcol != null && !this.IsEditorMode()) {
//                rgb = vcol.GetFontColor(row);
//                if (VColor.ParseRGB(rgb, out color)) {
//                    args.Appearance.ForeColor = color;
//                }
//            }
//            if (this.IsEditorMode()) {
//                string colName = table.GetNameForText(args.Column.FieldName);
//                if (colName == null) return;
//                //if (colName == "dolg_on_il_astr")
//                //{

//                //}

//                //if (colName == "ur_opl_posl_il_hnd")
//                //{

//                //}

//                var column = table.Columns[colName] as VDataColumn;

//                // кешировать все значения - валидация уже

//                bool drawwarning = false;
//                bool drawedit = false;
//                RepositoryItem rep = null;

//                rgb = column.GetFontColor(row);
//                if (VColor.ParseRGB(rgb, out color)) {
//                    args.Appearance.ForeColor = color;
//                }

//                var msg = table.GetCellError(row, colName);

//                if (!string.IsNullOrEmpty(msg))
//                {
//                    drawwarning = true;
//                }

//                if (column.GetEditable(row) || column.HasAdditionalButtons())// !!! проконтролировать время
//                {
//                    var row_index = table.Rows.IndexOf(row); // !!! проконтролировать время
//                    rep = repositories().GetCellRepository(GetTopTable(), colName, row_index/*, true*/);// !!! проконтролировать время
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
//                    string text = args.DisplayText;

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

//                    args.DisplayText = text;
//                    args.DefaultDraw();

//                    if (drawwarning)
//                    {
//                        if (rep == null)
//                        {
//                            var row_index = table.Rows.IndexOf(row); // !!! проконтролировать время
//                            rep = repositories().GetCellRepository(table, colName, row_index/*, true*/);// !!! проконтролировать время
//                        }
//                        var shift = Cmn.GetLeftButtonsSize(colName, rep);
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

//            if (IsCompareMode)
//            {
//                if (args.Column.FieldName.EndsWith("_row_comp"))
//                {
//                    args.Appearance.BackColor = Color.LightGray;
//                    return;
//                }

//                if (!args.Column.FieldName.EndsWith("_result"))
//                {
//                    // ищем колонку с результатом, если она есть 
//                    var result_column = ((BandedGridColumn)args.Column).OwnerBand.Columns.Cast<BandedGridColumn>()
//                        .FirstOrDefault(col => col.FieldName.EndsWith("_result") && !String.IsNullOrEmpty(col.UnboundExpression));
//                    if (result_column != null)
//                    {
//                        var value = view.GetRowCellValue(args.RowHandle, result_column.FieldName);
//                        // и результат отрицательный - выделяем зависимые ячейки
//                        if (value != null) {
//                            if (value != DBNull.Value && !Cmn.DECIMAL_ZERO.Equals(value)) {
//                                args.Appearance.ForeColor = Color.Crimson;
//                            }
//                        }
//                    }
//                }

//                // подкрашиваем только если нет группировок
//                if (view.GroupCount == 0)
//                {
//                    bool err_in_col = args.Column.FieldName.EndsWith("_result")
//                        && Enumerable.Range(0, view.RowCount - 1).Any(ind => Cmn.DECIMAL_ONE.Equals(view.GetRowCellValue(ind, args.Column)));

//                    bool err_in_row = view.Columns.Any(col => col.FieldName.EndsWith("_result") && Cmn.DECIMAL_ONE.Equals(view.GetRowCellValue(args.RowHandle, col.FieldName)));
//                    if (err_in_col && err_in_row && Cmn.DECIMAL_ONE.Equals(view.GetRowCellValue(args.RowHandle, args.Column.FieldName))) {
//                        args.Appearance.BackColor = Color.Crimson;
//                    } else if (err_in_col) {
//                        args.Appearance.BackColor = Color.FromArgb(200, 200, 255);
//                    }
//                    else if (err_in_row)
//                    {
//                        args.Appearance.BackColor = Color.FromArgb(255, 200, 200);
//                    }
//                }
//            }
//        }
//        private void view_CustomSummaryCalculate(object sender, CustomSummaryEventArgs args)
//        {

//            if (GetTopTable() == null) return;

//            if (GetTopTable().Merged)
//            {

//                if (args.SummaryProcess == CustomSummaryProcess.Start)
//                {
//                    //if (vcol.ColumnName == "dlg_isk_all")
//                    //{
//                    //}
//                    args.TotalValue = Cmn.DECIMAL_ZERO;
//                }
//                else if (args.SummaryProcess == CustomSummaryProcess.Calculate)
//                {
//                    //var view = GetMainView();
//                    var row = GetGrid().GetRowByHandle(args.RowHandle);
//                    if (row == null) return;

//                    var titem = (GridSummaryItem)args.Item;

//                    var vcol = row.Table.Columns[titem.FieldName] as VDataColumn;
//                    //if (vcol.ColumnName == "dlg_isk_all")
//                    //{
//                    //}
//                    if (vcol != null)
//                    {
//                        if (!vcol.IsMerged(row))
//                        {
//                            args.TotalValue = (decimal)args.TotalValue + Cmn.ToDecimal(args.FieldValue);
//                        }
//                    }
//                    //args.TotalValue = (decimal)args.TotalValue + Cmn.ToDecimal(args.FieldValue);

//                }
//                //else
//                //{
//                //    if (vcol.ColumnName == "dlg_isk_all")
//                //    {
//                //    }
//                //}
//                return;
//            }

//            if (GetTopTable().AllowMerge)
//            {
//                return;
//            }

//            // получается что невозможно использовать одновременно merge и custom summary через agg, скорее всего не пригодится

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
//        //private void view_FocusedRowChanged(object sender, FocusedRowChangedEventArgs args)
//        //{
//        //    var view = (sender as GridView);
//        //    GetTopTable().CurrentRow = view.GetFocusedDataRow();
//        //}

//        private RepositoryItem dummy = null;
//        private void view_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs args)
//        {
//            if (IsEditorMode())
//            {
//                var view = sender as GridView;
                
//                if (!view.IsValidRowHandle(args.RowHandle) || args.RowHandle < 0) return;
//                var table = GetTopTable();
//                var column_name = args.Column.FieldName;

//                //if (column_name == "vc_user_login_cnt")
//                //{

//                //}

//                //if (column_name == "vc_user_login_reg")
//                //{

//                //}
//                column_name = table.GetNameForText(column_name);
//                //var column_name = args.Column.FieldName.Replace(TextConst.Pfx.ExtValName, "");
//                var row_index = view.GetDataSourceRowIndex(args.RowHandle);

//                RepositoryItem rep = repositories().GetCellRepository(GetTopTable(), column_name, row_index);
//                args.RepositoryItem = rep;
//            }
//        }
//        private void view_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs args)
//        {
//            if (IsEditorMode()) args.Valid = true;
//        }
//        private void view_CustomDrawRowIndicator(object sender, RowIndicatorCustomDrawEventArgs args)
//        {
//            if (IsEditorMode())
//            {
//                if (args.Info.IsRowIndicator && args.RowHandle >= 0)
//                {
//                    var table = GetTopTable();
//                    var view = sender as GridView;
//                    var row = view.GetDataRow(args.RowHandle);

//                    if (row == null) return;

//                    if (table.IsRowDeleted(row)) args.Info.ImageIndex = 4;
//                    else if (row.RowState == DataRowState.Added) args.Info.ImageIndex = 2;
//                    else if (row.RowState == DataRowState.Modified) args.Info.ImageIndex = 1;
//                    else args.Info.ImageIndex = -1;

//                    // для выбранных строк красим индикатор в цвет самой строки
//                    if (view.IsRowSelected(args.RowHandle) && table.ClientBackColorSource != null)
//                    {
//                        var color = Cmn.GetHighlightColor();
//                        var backBrush = new SolidBrush(color);

//                        args.DefaultDraw();
//                        var bounds = new Rectangle(args.Bounds.X + 1, args.Bounds.Y + 1, args.Bounds.Width - 2, args.Bounds.Height - 2);


//                        args.Graphics.FillRectangle(backBrush, bounds);

//                        //ControlPaint.DrawBorder3D(args.Graphics, args.Bounds, Border3DStyle.RaisedInner);



//                        if (args.Info.ImageIndex > -1)
//                        {
//                            var images = args.Info.ImageCollection as ImageCollection;
//                            Image indImage = images.Images[args.Info.ImageIndex];
//                            int imageLeft = args.Bounds.Left + (args.Bounds.Width - indImage.Width) / 2;
//                            int imageTop = args.Bounds.Top + (args.Bounds.Height - indImage.Height) / 2;
//                            args.Cache.Graphics.DrawImage(indImage, new Point(imageLeft, imageTop));
//                        }

//                        args.Handled = true;
//                    }
//                }
//            }
//        }
//        private void view_BeforeLeaveRow(object sender, RowAllowEventArgs args)
//        {
//            if (IsEditorMode())
//            {
//                if (GetTopTable().HasChildrenUserChanges())
//                {
//                    RaiseHasMessage(this, new HasMessageArgs() { Message = "Имеются несохранённые изменения в дочерних таблицах" });
//                    args.Allow = false;
//                    return;
//                }
//            }
//        }
//        private void view_ColumnWidthChanged(object sender, ColumnEventArgs args)
//        {
        
//            ProcessColWidthChange(sender, args.Column.Name);
            
//        }

//        bool _colWidthChangeProcessing = false;
//        private void ProcessColWidthChange(object view, string columnName)
//        {
           
//            if (columnName == GridDesigner.GetDummyColumnName()) return;
//            if (_colWidthChangeProcessing) return;
//            _colWidthChangeProcessing = true;
//            GetControl().ApplyHeaderLayout(GetGrid().GetViewName(view));
        
//            UpdateColumnsPanelHeight(view);

//            if (IsEditorMode() && (!_allowSelectMoveColumns))
//            {
//                SaveSettingsToRegistry();
//            }
//            _colWidthChangeProcessing = false;
//        }

//        private void view_BandWidthChanged(object sender, BandEventArgs args)
//        {
//            ProcessColWidthChange(sender, null);
//        }
//        //private void view_DoubleClick(object sender, EventArgs e)
//        //{
//        //    GridView view = sender as GridView;
//        //    Point pt = view.GridControl.PointToClient(Control.MousePosition);
//        //    GridHitInfo info = view.CalcHitInfo(pt);

//        //    if (info.InRow || info.InRowCell)
//        //    {
//        //        // из ucReferenceGrid
//        //        view.PostEditor();
//        //        view.CloseEditor();
//        //        RaiseUIEvent2("", TextConst.AVEventName.DoubleClick, null, null);

//        //        DataRow row = view.GetFocusedDataRow();
//        //        if (row == null) return;

//        //        GridColumn gcol = view.FocusedColumn;
//        //        VDataColumn col = null;
//        //        if (gcol != null)
//        //        {
//        //            col = (VDataColumn)row.Table.Columns[gcol.FieldName];
//        //        }

//        //        // из ucReportGrid

//        //        RaiseUIEvent2(view.Name, TextConst.AVEventName.DoubleClick, row, col);
//        //    }
//        //}
      
    
//    }



    
//}
