//using System;
//using System.Drawing;
//using DevExpress.XtraEditors;
//using sql.builder.DataApi;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucEmpty : XtraUserControl
//    {
//        #region DataSource
//        private VDataSet _source;
//        public void SetDataSource(VDataSet source)
//        {
//            // чтобы небыло утечек
//            if (_source != null) DetachDataSourceEvents();

//            _source = source;

//            if (_source != null) AttachDataSourceEvents();
//        }
//        public VDataSet GetDataSource()
//        {
//            return _source;
//        }

//        private void AttachDataSourceEvents()
//        {
//            //
//        }
//        private void DetachDataSourceEvents()
//        {
//            //
//        }


//        #endregion

//        public ucEmpty()
//        {
//            InitializeComponent();
//        }
//        public void SetText(string text)
//        {
//            lText.Text = text;
//        }

//        private void lText_Resize(object sender, EventArgs e)
//        {
//            if (btnOpenExcel.Visible == false) return;

//            UpdateOpenExcelButtonLocation();
//        }

//        private void UpdateOpenExcelButtonLocation()
//        {
//            btnOpenExcel.Location = new Point((lText.Width - btnOpenExcel.Width) / 2, (lText.Height - btnOpenExcel.Height) / 2 + 30);
//        }

//        public void SetOpenExcelButtonVisible(bool visible)
//        {
//            if (btnOpenExcel.Visible == visible) return;

//            if (visible)
//            {
//                UpdateOpenExcelButtonLocation();
//            }

//            btnOpenExcel.Visible = visible;
//        }

//        public event EventHandler OpenExcel;

//        private void btnOpenExcel_Click(object sender, EventArgs e)
//        {
//            if (OpenExcel == null) return;

//            OpenExcel(this, EventArgs.Empty);
//        }
//    }
//}
