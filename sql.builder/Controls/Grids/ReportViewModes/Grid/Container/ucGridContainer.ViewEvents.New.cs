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
//        bool eventsAttachedNew = false;
//        private void AttachViewEventsNew()
//        {
//            var grid = GetGrid();
//            grid.WfAttachViewEvents();
//            if (!eventsAttachedNew)
//            {
//                grid.ViewCustomRowFilter += grid_ViewCustomRowFilter;
//                grid.CellDoubleClick+=grid_CellDoubleClick;
//                eventsAttachedNew = true;
//            }
            
//        }

//        private void grid_CellDoubleClick(string view, int row, string column)
//        {
//            //GridView view = sender as GridView;
//            //Point pt = view.GridControl.PointToClient(Control.MousePosition);
//            //GridHitInfo info = view.CalcHitInfo(pt);

//            //if (info.InRow || info.InRowCell)
//            //{
//            //    // из ucReferenceGrid
//            //    view.PostEditor();
//            //    view.CloseEditor();
//            //    RaiseUIEvent2("", TextConst.AVEventName.DoubleClick, null, null);

//            //    DataRow row = view.GetFocusedDataRow();
//            //    if (row == null) return;

//            //    GridColumn gcol = view.FocusedColumn;
//            //    VDataColumn col = null;
//            //    if (gcol != null)
//            //    {
//            //        col = (VDataColumn)row.Table.Columns[gcol.FieldName];
//            //    }

//            //    // из ucReportGrid

//            //    RaiseUIEvent2(view.Name, TextConst.AVEventName.DoubleClick, row, col);
//            //}
//            DataTable tbl = null;
//            if (!string.IsNullOrEmpty(view))
//            {
//                tbl = GetDataSource().GetTable(view);
//            }
//            else
//            {
//                tbl = GetTopTable();
//            }
//            DataRow dataRow = null;
//            if (row > 0)
//            {
//                dataRow = tbl.Rows[row];
//            }
//            VDataColumn col = null;
//            if (column != null)
//            {
//                col = (VDataColumn)tbl.Columns[column];
//            }
//            RaiseUIEvent2("", TextConst.AVEventName.DoubleClick, null, null);
//            if (dataRow != null)
//            {
//                RaiseUIEvent2(tbl.TableName, TextConst.AVEventName.DoubleClick, dataRow, col);
//            }
        
//        }
//        private void DetachViewEventsNew()
//        {
//            var grid = GetGrid();
//            grid.WfDetachViewEvents();
//            if (eventsAttachedNew)
//            {
//                grid.ViewCustomRowFilter -= grid_ViewCustomRowFilter;
//                grid.CellDoubleClick -= grid_CellDoubleClick;
//                eventsAttachedNew = false;
//            }
//        }
//        private void grid_ViewCustomRowFilter(TableViewerEventArgs viewEventArgs, CustomProcessEventArgs customProcessArgs)
//        {
//            if (GetTopTable() != null && GetTopTable().HasUserChanges) { // может быть так побыстрее
//                var gr = GetGrid();
//                object val = gr.DxGetListSourceRowCellValue(viewEventArgs.Row,TextConst.AVColumn.IsNew);
//                if (Cmn.DECIMAL_ONE.Equals(val)) {
//                    customProcessArgs.Handled = true;
//                    customProcessArgs.BoolValue = true;
//                }
//            }
//        }
//    }
//}