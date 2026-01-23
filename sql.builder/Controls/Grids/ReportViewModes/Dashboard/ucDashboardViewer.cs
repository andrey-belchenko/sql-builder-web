//using System.Drawing;
////using System.Windows.Forms;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid;

//namespace sql.builder.Controls.Grids.ReportViewModes.Dashboard
//{
//    internal partial class ucDashboardViewer : ucDashboardBase
//    {
//        public ucDashboardViewer()
//        {
//            InitializeComponent();
//            Dashboard = dashboardViewer1.Dashboard = new DevExpress.DashboardCommon.Dashboard();
//        }

//        private void btnReload_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            LoadDashboard();
//        }

//        internal override void ReloadData()
//        {
//            dashboardViewer1.ReloadData();
//            dashboardViewer1.Dashboard.CustomFilterExpression += Dashboard_CustomFilterExpression;
//        }

//        void Dashboard_CustomFilterExpression(object sender, DevExpress.DashboardCommon.DashboardCustomFilterExpressionEventArgs e)
//        {
//            throw new System.NotImplementedException();
//        }

//        private void dashboardViewer1_DashboardItemClick(object sender, DevExpress.DashboardWin.DashboardItemMouseActionEventArgs e)
//        {
//            var ll = e.Data;
//            var dd = e.GetUnderlyingData();

//            var frm = new XtraForm() {Size = new Size(500,500), StartPosition = FormStartPosition.CenterScreen};
//            var grid = new GridControl() {Dock = DockStyle.Fill};
//            frm.Controls.Add(grid);
//            grid.DataSource = dd;
//            frm.ShowDialog();
//        }
//    }
//}
