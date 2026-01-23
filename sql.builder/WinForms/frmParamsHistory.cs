//using System;
//using System.Data;
//using System.Drawing;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using Devart.Data.Oracle;
////using DevExpress.Skins;
//using infoenergo.core.Data;
//using infoenergo.ui.win.Forms;

//namespace sql.builder.WinForms
//{
//    internal partial class frmParamsHistory : FormBase
//    {
//        public frmParamsHistory()
//        {
//            InitializeComponent();
//        }

//        public void Initialize(string repname)
//        {
//            var dt = DataHelper.SqlGetTable("select /*+ INDEX (vr_reports_log ind1vr_reports_log)*/ * from vr_reports_log where rownum < 100 and repname = :repname order by date_start desc", 
//                new []{new OracleParameter("repname",OracleDbType.NVarChar) {Value = repname}, }, db.Connection);

//            gridControl1.DataSource = dt;
//            gridControl1.ForceInitialize();

//            gridView1.Columns["DATE_START"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
//            gridView1.Columns["DATE_START"].DisplayFormat.FormatString = "g";
//            gridView1.Columns["DATE_START"].VisibleIndex = 0;
//            gridView1.Columns["DATE_FINISH"].DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
//            gridView1.Columns["DATE_FINISH"].DisplayFormat.FormatString = "g";
//            gridView1.Columns["DATE_FINISH"].VisibleIndex = 1;

//            gridView1.Columns["PUSER"].VisibleIndex = 2;

//            var col = gridView1.Columns.FirstOrDefault(c => c.FieldName == "TERMINAL");
//            if (col != null) col.VisibleIndex = 3;

//            gridView1.Columns["KOD_LOG"].Visible = false;
//            gridView1.Columns["PARAMS"].Visible = false;
//            gridView1.Columns["REPNAME"].Visible = false;

//            gridView1.BestFitColumns();

//            gridView1.Columns["ERROR_TEXT"].Width = 150;
//            gridView1.Columns["STACK_TEXT"].Width = 150;
//        }

//        private void btnAccept_Click(object sender, System.EventArgs e)
//        {
//            DialogResult = DialogResult.Yes;
//        }

//        private void btnCancel_Click(object sender, System.EventArgs e)
//        {
//            DialogResult = DialogResult.No;
//        }

//        internal XElement GetSelectedParams()
//        {
//            var row = gridView1.GetFocusedDataRow();
//            if (row == null) return null;

//            return XElement.Parse(row["PARAMS"].ToString());
//        }

//        private void gridView1_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
//        {
//            var row = gridView1.GetFocusedDataRow();
//            if (row == null) return;

//            memoEdit1.EditValue = row["PARAMS"].ToString();

//            btnAccept.Enabled = AllowableParams(row);
//        }

//        private void gridView1_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
//        {
//            var row = gridView1.GetDataRow(e.RowHandle);

//            Color color = AllowableParams(row) ? Color.FromArgb(220, 255, 220) : Color.FromArgb(255, 220, 220);

//            if (gridView1.FocusedRowHandle == e.RowHandle)
//            {
//                var focused_color = Cmn.GetFocusedBackColor();
//                e.Appearance.BackColor = focused_color.MixColors(color, 0.5F);
//            }
//            else
//            {
//                e.Appearance.BackColor = color;
//            }
           
//        }

//        bool AllowableParams(DataRow row)
//        {
//            if (row == null || row["PARAMS"] == DBNull.Value) return false;

//            XElement xroot = null;
//            try
//            {
//                xroot = XElement.Parse(row["PARAMS"].ToString());
//            }
//            catch (System.Xml.XmlException)
//            {
//                return false;
//            }

//            return (xroot.Name == "root");
//        }
//    }
//}
