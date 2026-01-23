//using System.Collections.Generic;
//using System.Linq;
////using DevExpress.XtraTreeList;
//using sql.builder.UI;

//namespace sql.builder.PackagePrint
//{
//    internal partial class ucPackagePrint : ucBase
//    {
//        internal ucPackagePrint()
//        {
//            InitializeComponent();

//            XmlReports.Init();
//            XmlReports.Environment.Manager.LoadProjectIfNeed("asuse2");
//            var form = UIStatic.GetForm("vr_packages", false, true);
//            form.RefreshSource();
//            //var dt = form.DataSource.Tables["vr_packages"] as VDataTable;
//            //dt.Rows[0]["title"] = "Ля ля ля";

//            //var row = ds.Tables["vr_packages"].NewRow();
//            //row["title"] = "Add";
//            //dt.Rows.Add(row);

//            //(dt as VDataTable).DeleteRow(dt.Rows[1]);

//            //form.SaveData(false);
//            var ds = new aa() { Children = new[] { new aa { Name = "aa", Children = new[] { new aa() { Name = "child1" }, new aa() { Name = "child2" } } } } };
//            tlReports.DataSource = ds;
//        }

//        internal class aa : TreeList.IVirtualTreeListData
//        {
//            public string Name { get; set; }
//            public IEnumerable<aa> Children { get; set; }

//            public override string ToString()
//            {
//                return Name;
//            }

//            public void VirtualTreeGetChildNodes(VirtualTreeGetChildNodesInfo info)
//            {
//                if (Children == null) return;
//                info.Children = Children.ToList();
//            }

//            public void VirtualTreeGetCellValue(VirtualTreeGetCellValueInfo info)
//            {
//                if(info.Column.FieldName == "Name")
//                {
//                    info.CellData = Name;   
//                }
//            }

//            public void VirtualTreeSetCellValue(VirtualTreeSetCellValueInfo info)
//            {
//                //
//            }
//        }
//    }
//}
