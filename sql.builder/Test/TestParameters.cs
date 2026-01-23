//using System;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;

//using sql.builder.Controls.FormFields;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;

////using DevExpress.XtraEditors;
////using DevExpress.XtraGrid.Columns;
//using infoenergo.ui.win.Base;
//using System.Collections.Generic;
//using infoenergo.core.Data;

//namespace sql.builder.Test
//{
//    internal partial class TestParameters : XtraForm
//    {
//        public TestParameters()
//        {
//            InitializeComponent();

//            //this.ucFormParameters1.Initialize();
//            this.ucFormParameters1.QueryName = "poisk_byt2";
//            this.ucFormParameters1.FormName = "";
//            this.ucFormParameters1.ReportName = "";
//           // ucFormParameters1.WorkFolderVisible = true;

//            gridControl1.BeforeRefreshRows += gridControl1_BeforeRefreshRows;
//            gridControl1.AllowPreFetch = true;
//            gridControl1.GetFetchAllEvent += gridControl1_GetFetchAllEvent;
//        }

//        void gridControl1_GetFetchAllEvent(object sender, System.ComponentModel.HandledEventArgs e)
//        {
//            e.Handled = true;
//            //ucFormParameters1.FetchNext(tbl, 1000);
//            ((VDataTable)tbl).FetchNext(1000);
//            //   ucFormParameters1.FetchNext(tbl,int.MaxValue);// fetch до конца, у грида есть события сортировка фильтр, при которых это нужно сделать
//        }

//        public void Initialize()
//        {
//            ucFormParameters1.Initialize();
//			//ucFormParameters1.FormValueChanged += eventCheck;
//        }



//        private DataTable tbl = null;
//        void gridControl1_BeforeRefreshRows(object sender, GridDataRefreshEventArgs e)
//        {

//            var msg = ucFormParameters1.ValidateParams();
//            if (msg != "")
//            {
//                XtraMessageBox.Show(msg, "Не все параметры указаны корректно", MessageBoxButtons.OK, MessageBoxIcon.Warning);
//                return;
//            }

//            gridControl1.Columns.Clear();
//            bool defferedFetch = true;
//            var result = ucFormParameters1.GetDataSet(defferedFetch);
//            CreateColumns(result.Tables[0].Columns);
//            tbl = result.Tables[0];
//            e.dataTable = tbl;

//            lRowsCount.Caption = "Загружено строк: " + tbl.Rows.Count;
            
//            //throw new NotImplementedException();
//        }

//        public string QueryName { get { return this.ucFormParameters1.QueryName; } set { this.ucFormParameters1.QueryName = value; } }
//        public string FormName { get { return this.ucFormParameters1.FormName; } set { this.ucFormParameters1.FormName = value; } }
//        public string ReportName { get { return this.ucFormParameters1.ReportName; } set { this.ucFormParameters1.ReportName = value; } }

//        //private void button1_Click(object sender, EventArgs e)
//        //{
//        //    // тут собственно execute запроса

//        //    var result = ucFormParameters1.GetDataSet();
//        //    CreateColumns(result.Tables[0].Columns);
//        //    gridControl1.RefreshData(result.Tables[0]);
//        //}

//        void CreateColumns(DataColumnCollection columns)
//        {
//            gridControl1.Columns.Clear();

//            foreach (DataColumn col in columns)
//            {
//                GridColumn newcol = /*new GridColumn();*/ new infoenergo.ui.win.Grid.Column.GridColumn();
//                newcol.FieldName = col.ColumnName;
//                newcol.Name = col.Caption;
//                newcol.Visible = true;

//                gridControl1.Columns.Add(newcol);
//            }
//        }

//        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            ucFormParameters1.RefreshParams();
//        }

//        private void ucFormParameters1_Load(object sender, System.EventArgs e)
//        {

//        }

//        private void barButtonItem2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            //var nump_s = ucFormParameters1.GetTextFieldByName("nump_s");
//            //nump_s.SetValue("11");
//            //var nump_po = ucFormParameters1.GetTextFieldByName("nump_po");
//            //nump_po.SetValue("11");
//            //1
//            //var ff = ucFormParameters1.GetSqlData();
//            //1.1
//            //var sql = ucFormParameters1.GetSqlData();
//            /*return;
            
//            var cmd = ucFormParameters1.GetSelectCommand();

//            var da = new Devart.Data.Oracle.OracleDataAdapter((Devart.Data.Oracle.OracleCommand)cmd);

//            var tbl = new DataTable();
//            da.Fill(tbl);

//           //2
//            //var list = new List<decimal>();
//            //list.Add(600000021);
//            //list.Add(600000041);
//            //ucFormParameters1.GetParamField("dep").SetValue(list);
//            */
//        }

//        private void barButtonItem3_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            ucFormParameters1.ChooseParameters();
//        }

//        private void barButtonItem4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            ucFormParameters1.SaveParametersTemplate();
//        }

//        private void barButtonItem5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            ucFormParameters1.LoadParametersTemplate();
//        }

//        private void barButtonItem6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            ucFormParameters1.SaveDefaultParameters();
//        }

//        private void barButtonItem7_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            ucFormParameters1.LoadDefaultParameters();
//        }

//        private void barButtonItem8_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            ucFormParameters1.ClearDefaultParameters();
//        }

//        private void barButtonItem9_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            //DataHelper.SqlExecutePLSQL("begin rg_glob.set_glob(n_podr=> 12985, n_ym=> 2016.09); end;", db.Connection);
//            //var s = ucFormParameters1.GetTableSelectText(tbl);

//            //var cmd = new Devart.Data.Oracle.OracleCommand("create table bav_test1 as select * from  (" + s + ")");
//            //cmd.Connection = db.Connection;

//            //cmd.ExecuteNonQuery();
//            //var a = ucFormParameters1.GetParamField("nump");
//            //a.SetValue("1234");

//            // выбор населенных пунктов в фильтре
//            // один или несколько kod_m (adr_m)

//            ParamField field = ucFormParameters1.GetParamField("dep");
//            field.SetValue(new[] { 2199m }); // Центральное отделение
//            field.LoadValueText();

//            // Контрол участков в форме поиска
//            field = ucFormParameters1.GetParamField("kod_podr");
//            field.SetValue(new[] { 2572m }); // Пригородный участок
//            field.LoadValueText();
//            /*return;
            
//            //var b = Enumerable.Range(1, 100001).Select(a => a.ToString());
//            //ucFormParameters1.GetParamField("id_adres_list").SetValue(b);
//            field = ucFormParameters1.GetParamField("kod_street");
//            field.SetValue(new[] { 5011971 });
//            //ucFormParameters1.GetParamField("kod_street").LoadList();
//            field.LoadValueText();

//            //string[] id_adres_array = new[] { "11111", "222222" };
//            //FormParametersControl1.GetParamField("id_adres_list").SetValue(id_adres_array);

//            //var aa = ucFormParameters1.GetSqlData();
//            return;

//            decimal[] values = new decimal[]{ 5033915 };
//            field = ucFormParameters1.GetParamField("kod_adr_m");
//            field.SetValue(values);
//            field.LoadValueText();
//            //field.LoadList();

//            // выбор улиц в фильтре
//            // один или несколько kod (k_strits)
//            values = new decimal[]{ 11338, 10144523 };
//            field = ucFormParameters1.GetParamField("kod_street");
//            field.SetValue(values);
//            field.LoadValueText();
//            //field.LoadList();
//            // выбор домов в фильтре
//            // один или несколько kodd (k_house)
//            values = new decimal[] { 36030, 36034 };
//            field = ucFormParameters1.GetParamField("kodd");
//            field.SetValue(values);
//            //field.LoadList();
//            field.LoadValueText();
//            */
//        }
//        private void btnLoadAll_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
            
//        }

//		private static XElement reportParams;

//		private void barButtonItem10_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//		{
//			TestParameters.reportParams = ucFormParameters1.GetParametersXml();
//		}

//		private void barButtonItem11_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//		{
//			ucFormParameters1.SetParametersXml(TestParameters.reportParams);
//		}

//        private void TestParameters_Load(object sender, EventArgs e)
//        {

//        }

//		private void eventCheck(object sender, EventArgs e)
//		{
//			MessageBox.Show("что-то произошло");
//		}

//    }
//}
