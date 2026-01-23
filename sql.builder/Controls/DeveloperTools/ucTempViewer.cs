//using System;
//using System.Data;
//using System.Text;
//using DevExpress.XtraEditors;
//using sql.builder.DataApi;

//namespace sql.builder
//{
//    internal partial class ucTempViewer : XtraUserControl
//    {
//        public ucTempViewer()
//        {
//            InitializeComponent();
//        }
//        private VDataSet dataSource = null;
//        public void SetSource(VDataSet dataSet)
//        {
//            dataSource = dataSet;


//           // var tblTbl = new VDataTable();

//           // tblTbl.AddColumn("id");
//           //tblTbl.AddColumn("name");


//           foreach (VDataTable tbl in dataSource.Tables)
//           {
//             comboBoxEdit1.Properties.Items.Add(tbl.TableName);
//           }
          

//            //comboBoxEdit1.DataBindings.

//        }

//        private void comboBoxEdit1_EditValueChanged(object sender, EventArgs e)
//        {
//            var tbl = dataSource.Tables[comboBoxEdit1.EditValue.ToString()] as VDataTable;

//            var sel = "select ";
//            var q = "";

//            foreach (string s in TextConst.DBObjectsArray.TempTableSpecCols)
//            {
               
                
//                    sel += q + s;
//                    q = ",";
                
//            }
//            foreach (VDataColumn col in tbl.Columns)
//            {
//                if ( col.TempColumnName!=null ){
//                    sel += q + col.TempColumnName + " as " + col.ColumnName;
//                    q = ",";
//                }
//            }


//            sel += "  from " + TextConst.DBObjects.TempTable;

//            sel += "  where  " + TextConst.DBObjects.TempTableFormIdColumn + "=" + dataSource.GetFormId();
//            sel += "  and  " + TextConst.DBObjects.TempTableTableIdColumn + "='" + comboBoxEdit1.EditValue.ToString() + "'";

//            var dt = db.ExecuteDataTable(sel, dataSource.GetConnection());


//            gridView1.Columns.Clear();

//            gridControl1.DataSource = dt;

//        }

//        private void btn1_Click(object sender, EventArgs e)
//        {
//            string sql = "";
//            var parTbl = dataSource.ParamsTable;
//            foreach (VDataTable tbl in dataSource.Tables)
//            {
//                if (tbl != parTbl)
//                {
//                    foreach (DataRow r in tbl.ModifiedRows)
//                    {
                        
//                            tbl.SetUpdateTempRowParams(r);
//                            var s1 = VDBSelectCommand.GetCmdParametrizedText(tbl.UpdateTempCommand);
//                            sql += Environment.NewLine + s1;
                        
//                    }
//                }
//            }
//            Cmn.SaveText(sql, Properties.Settings.Default.testQueryS2, Encoding.Unicode);
//            System.Diagnostics.Process.Start(Properties.Settings.Default.testQueryS2);
//        }
//        public static string GetTempSql()
//        {
//            var dt = db.ExecuteDataTable("select * from " + TextConst.DBObjects.TempTable);
//            var sql = "delete " + TextConst.DBObjects.TempTable+";"+Environment.NewLine;
//            foreach (DataRow r in dt.Rows)
//            {
//                sql += "insert into " + TextConst.DBObjects.TempTable + " (select ";
//                var q = "";

//                foreach (DataColumn col in dt.Columns)
//                {
//                    sql += q + Cmn.ToOracleString(r[col]);
//                    q = ",";
//                }
//                sql += " from dual);" + Environment.NewLine;
//            }
//            return sql;
//        }
//    }
//}
