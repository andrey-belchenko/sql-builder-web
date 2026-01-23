//using System;
//using System.Collections.Generic;
//using System.Linq;
////using System.Windows.Forms;
//using DevExpress.Data;
//using DevExpress.Utils;

//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;
//using System.Data;
//using Clipboard = System.Windows.Clipboard;

//using DevExpress.XtraTreeList;
//using DevExpress.XtraTreeList.Columns;
//using DevExpress.XtraTreeList.Nodes;
//using DevExpress.XtraTreeList.ViewInfo;
//using  System.Drawing;
//namespace sql.builder.Controls.Grids.ReportViewModes
//{

//    internal partial class ucTreeWF : TreeList, IucGrid
//    {
//        bool eventsAttached = false;

//        private VDataTable GetTopTable()
//        {
//            var vdt = _top_table as VDataTable;
//            return vdt;
//        }

//        public void WfAttachViewEvents()
//        {
           
            
//                if (!eventsAttached)
//                {

//                    eventsAttached = true;
//                    this.CustomRowFilter += view_CustomRowFilter;
//                    this.SelectionChanged += view_SelectionChanged;
//                    this.FocusedNodeChanged += tree_FocusedNodeChanged;
//                    this.DoubleClick+=ucTreeWF_DoubleClick; 
//                }
//        }

//        private void ucTreeWF_DoubleClick(object sender, EventArgs e)
//        {

//            Point pt = this.PointToClient(Control.MousePosition);
//            TreeListHitInfo info = this.CalcHitInfo(pt);
//            int rowIndex = -1;
//            if (info.Node != null)
//            {
//                if (this.FocusedNode != null)
//                {
//                    DataRow dr = GetNodeData(this.FocusedNode);
//                    if (dr != null)
//                    {
//                        rowIndex = dr.Table.Rows.IndexOf(dr);
//                    }
//                }
//            }
//            string colName = null;
//            if (info.Column != null)
//            {
//                colName = info.Column.FieldName;
//            }
//            CellDoubleClick(null, rowIndex, colName);
//        }

        
        
//        public void WfDetachViewEvents()
//        {
//            if (eventsAttached)
//            {
//                this.CustomRowFilter -= view_CustomRowFilter;
//                this.SelectionChanged -= view_SelectionChanged;
//                this.FocusedNodeChanged -= tree_FocusedNodeChanged;
//                this.DoubleClick -= ucTreeWF_DoubleClick; 
//                eventsAttached = false;
//            }
//        }

//        public event CustomProcessEventHandler ViewCustomRowFilter;
//        void view_CustomRowFilter(object sender, CustomRowFilterEventArgs e)
//        {
//            if (ViewCustomRowFilter != null)
//            {
//                var viewArgs = new TableViewerEventArgs();
//                viewArgs.Row = e.Node;
//                var cpArgs = new CustomProcessEventArgs();
//                ViewCustomRowFilter(viewArgs, cpArgs);
                
//                if (cpArgs.Handled)
//                {
//                    e.Handled = true;
//                    e.Visible = cpArgs.BoolValue;
//                }

//            }
//        }
//        private DataRow GetNodeData(TreeListNode node)
//        {
//            var rowView = this.GetDataRecordByNode(node) as DataRowView;
//            return (rowView != null) ? rowView.Row : null;
//        }
//        private void tree_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs args)
//        {
//            GetTopTable().CurrentRow = GetNodeData(this.FocusedNode);
//        }
//        public event ValueChangeEventHandler ViewSelectionChanged;
       
//        void view_SelectionChanged(object sender, EventArgs e)
//        {
//            if (ViewSelectionChanged != null)
//            {
//                ViewSelectionChanged(sender);
//            }
//        }
//    }
//}
