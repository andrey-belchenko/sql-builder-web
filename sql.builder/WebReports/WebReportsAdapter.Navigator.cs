
//using sql.builder.DataApi;
//using sql.builder.WebReports.Client;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics.Contracts;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using System.Xml.Linq;

//namespace sql.builder.WebReports
//{
//    internal static partial class WebReportsAdapter
//    {
//        public static bool Enabled { get; set; } 

//        static WebReportsAdapter()
//        {
//            Enabled = false;
//        }


//        private static void AddPath(Dictionary<string, string> folderPathByName, Dictionary<string, DataRow> folderByName , DataRow row)
//        {
//            var name = row["name"].ToString();
//            if (folderPathByName.ContainsKey(name)) return;
//            var parent = row["parent"].ToString();
//            var title = row["title"].ToString();
//            var path = title;
//            if (parent != string.Empty)
//            {
//                AddPath(folderPathByName, folderByName, folderByName[parent]);
//                path = folderPathByName[parent] + "/" + title;

//            }
//            folderPathByName.Add(name, path);
//        }
//        public static void FillReportsDataTable(DataTable table)
//        {
//            if (!Enabled)
//            {
//                return;
//            }

//            var folderByName = new Dictionary<string, DataRow>();

//            foreach (DataRow row in table.Rows)
//            {
//                if (row["item_type"].ToString() == "folder")
//                {
//                    folderByName.Add(row["name"].ToString(), row);
//                }
//            }


//            var folderPathByName = new Dictionary<string, string>();

            

//            foreach (DataRow row in folderByName.Values)
//            {
//                AddPath(folderPathByName, folderByName, row);
//            }

//            var folderNameByPath = folderPathByName.ToDictionary(it => it.Value, it => it.Key);


//            string customerId;
//            using (DataTable dt = db.ExecuteDataTable("select customer_id from rs_rep_sets"))
//            {
//                DataRow row = dt.Rows[0];
//                customerId = row["customer_id"].ToString();
//            }


//            var navigator = WebReportsClient.GetNavigator(customerId.ToString());
//            if (navigator == null) return;

//            var items = navigator.AllItems();
//            foreach (var item in items)
//            {
//                var row = table.NewRow();

//                if (folderNameByPath.ContainsKey(item.Path())) continue;
//                row["name"] = CreateTag(item.Id);
//                row["title"] = item.Title;
//                row["item_type"] = (item is Folder) ? EName.folder.LocalName : "nogrid";

//                if (item.Parent != null)
//                {
//                    if (folderNameByPath.ContainsKey(item.Parent.Path()))
//                    {
//                        row["parent"] = folderNameByPath[item.Parent.Path()];
//                    }
//                    else
//                    {
//                        row["parent"] = CreateTag(item.Parent.Id);
//                    }
//                }
//                else
//                {
//                    row["parent"] = string.Empty;
//                }

//                row["visible"] = true;
//                row["kod_menu"] = DBNull.Value;
//                row["has_access"] = Cmn.BOOLEAN_TRUE;
//                row["original_name"] = CreateTag(item.Id);
//                row["changed"] = Cmn.DECIMAL_ZERO;
//                row["image_id"] = (item is Folder) ? Cmn.DECIMAL_ZERO : Cmn.DECIMAL_ONE;
//                row["project"] = string.Empty;
//                row["old"] = false;
//                table.Rows.Add(row);

//            }
//        }
//    }
//}
