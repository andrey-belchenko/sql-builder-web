//using System;
//using System.Collections.Generic;
//using System.Linq;
////using System.Windows.Forms;
//using DevExpress.Data;
//using DevExpress.Utils;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraGrid.Views.Base;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;
//using System.Data;
//using System.Drawing;
//using   DevExpress.XtraGrid.Views.Grid.ViewInfo;
//using Clipboard = System.Windows.Clipboard;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
  
//    internal partial class ucGridWF : GridControl, IucGrid
//    {
//        List<int> eventsAttached = new List<int>();

//        private VDataTable GetTopTable()
//        {
//            var vdt = _top_table as VDataTable;
//            return vdt;
//        }

//        public void WfAttachViewEvents()
//        {
//            var views = ViewCollection.Cast<GridView>();
//            var eventsAttachedNew = new List<int>();
//            foreach (GridView view in views)
//            {
//                if (!eventsAttached.Contains(view.GetHashCode()))
//                {

                   
//                    view.CustomRowFilter += view_CustomRowFilter;
                
//                    view.SelectionChanged += view_SelectionChanged;
//                    view.FocusedRowChanged += view_FocusedRowChanged;

//                    view.DoubleClick += view_DoubleClick;
//                }
//                else
//                {

//                }
//                eventsAttachedNew.Add(view.GetHashCode());
//            }
//            eventsAttached = eventsAttachedNew;
//        }
//        public event CellEventHandler CellDoubleClick;
//        void view_DoubleClick(object sender, EventArgs e)
//        {
//            if (CellDoubleClick != null)
//            {
//                GridView view = sender as GridView;
//                Point pt = view.GridControl.PointToClient(Control.MousePosition);
//                GridHitInfo info = view.CalcHitInfo(pt);

//                if (info.InRow || info.InRowCell)
//                {
//                    view.PostEditor();
//                    view.CloseEditor();

//                    if (CellDoubleClick != null)
//                    {
//                        var rowIndex = -1;
//                        string colName = null;
//                        DataRow row = view.GetFocusedDataRow();
//                        if (row != null)
//                        {
//                            rowIndex = row.Table.Rows.IndexOf(row);
//                        }

//                        GridColumn gcol = view.FocusedColumn;

//                        if (gcol != null)
//                        {
//                            colName = gcol.FieldName;
//                        }
//                        CellDoubleClick(view.Name, rowIndex, colName);
//                    }
//                }
//            }
//        }

        
        
//        public void WfDetachViewEvents()
//        {
           
//            var views = ViewCollection.Cast<GridView>();
//            foreach (GridView view in views)
//            {
             
//                view.CustomRowFilter -= view_CustomRowFilter;
//                view.SelectionChanged -= view_SelectionChanged;
//                view.FocusedRowChanged -= view_FocusedRowChanged;
//                view.DoubleClick -= view_DoubleClick;
               
//            }
//            eventsAttached = new List<int>();
//        }
//        public object DxGetListSourceRowCellValue(object row, string columnName)
//        {
//            return GetMainView().GetListSourceRowCellValue((int)row, columnName);
//        }

//        private void view_FocusedRowChanged(object sender, FocusedRowChangedEventArgs args)
//        {
//            var view = (sender as GridView);
//            GetTopTable().CurrentRow = view.GetFocusedDataRow();
//        }
//        public event CustomProcessEventHandler ViewCustomRowFilter;
        
//        void view_CustomRowFilter(object sender, RowFilterEventArgs e)
//        {
//            if (ViewCustomRowFilter != null)
//            {
//                var viewArgs = new TableViewerEventArgs();
//                viewArgs.Row = e.ListSourceRow;
//                var cpArgs = new CustomProcessEventArgs();
//                ViewCustomRowFilter(viewArgs, cpArgs);
                
//                if (cpArgs.Handled)
//                {
//                    e.Handled = true;
//                    e.Visible = cpArgs.BoolValue;
//                }

//            }
            
//        }
//        public event ValueChangeEventHandler ViewSelectionChanged;
//        void view_SelectionChanged(object sender, SelectionChangedEventArgs e)
//        {
//            if (ViewSelectionChanged != null)
//            {
//                ViewSelectionChanged(sender);
//            }
//        }

//    }
//}
