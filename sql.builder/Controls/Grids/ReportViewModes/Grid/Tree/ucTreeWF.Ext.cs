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
//namespace sql.builder.Controls.Grids.ReportViewModes
//{

//    internal partial class ucTreeWF : TreeList, IucGrid
//    {


//        private DataTable _top_table = null;
//        public  void SetGridTopTable(DataTable dt)
//        {
//            _top_table = dt;
//            //createLevelTreeNode( dt, null);
//        }

//        //private  void createLevelTreeNode( DataTable dt, GridLevelNode parent_node)
//        //{
//        //    //???
//        //    var view = this.ViewCollection.Cast<GridView>().FirstOrDefault(v => v.Name == dt.TableName);

//        //    if (view == null) return;

//        //    GridLevelNode node = null;
//        //    if (parent_node != null)
//        //    {
//        //        node = parent_node.Nodes.Add(dt.TableName, view);
//        //    }
//        //    else
//        //    {
//        //        this.MainView = view;

//        //        node = this.LevelTree.Find(this.MainView);
//        //        node.Nodes.Clear();
//        //    }

//        //    foreach (DataRelation relation in dt.ChildRelations)
//        //    {
//        //        createLevelTreeNode( relation.ChildTable, node);
//        //    }
//        //}
//    }
//}
