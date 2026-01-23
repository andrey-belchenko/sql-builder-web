//using System.IO;
////using System.Windows.Forms;
////using DevExpress.XtraEditors;
//using sql.builder.MP;
//using sql.builder.MP.Tools;


//namespace sql.builder.MP.Forms
//{
//    public partial class frmUpdateData : XtraForm
//    {
//        UpdateDataController Controller;

//        public frmUpdateData()
//        {
//            InitializeComponent();
//        }

//        public frmUpdateData(UpdateDataController controller): this()
//        {
//            Controller = controller;
//            Initialize();
//        }

//        private void Initialize()
//        {
//            Text = "Обновление данных в ";// + Controller.DestTableName;

//            teTempTableName.DataBindings.Add("EditValue", Controller, "SrcTableName");
//            teMainTableName.DataBindings.Add("EditValue", Controller, "DestTableName");
//            grColumns.DataBindings.Add("DataSource", Controller, "Columns");
//            grLog.DataBindings.Add("DataSource", Controller, "LogTable");

//            Controller.ChangeUIMode += (sender, mode) => InvokeIfNeed(() => SetUIMode(mode));
//        }

//        private void btnUpdateTableFromExcel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            var filePath = SelectExcelFile();
//            if (filePath == null) return;
//           //сломал
//           // Controller.UpdateTempTableFromExcel(filePath);
//            //Controller.UpdateTableFromExcelAsync(filePath, 4);
//        }

//        public static string SelectExcelFile()
//        {
//            using (var dlg = new OpenFileDialog())
//            {
//                dlg.InitialDirectory = UserSettings.ExcelPath;
//                dlg.Filter = "(*.xlsx)|*.xlsx|(*.xlsm)|*.xlsm|(*.*)|*.*";
//                dlg.DefaultExt = "xlsm";

//                var result = dlg.ShowDialog();
//                if (result != DialogResult.OK) return null;

//                UserSettings.ExcelPath =  Path.GetDirectoryName(dlg.FileName);

//                return dlg.FileName;
//            }
//        }

//        private void InvokeIfNeed(MethodInvoker action)
//        {
//            if (IsDisposed) return;

//            if (InvokeRequired) Invoke(action);
//            else action();
//        }

//        private void SetUIMode(UIMode mode)
//        {
//            if (mode == UIMode.Ready)
//            {
//                btnUpdateTableFromExcel.Enabled = true;
//                rpbProgress.Stopped = true;
//            }
//            else if (mode == UIMode.InProgress)
//            {
//                btnUpdateTableFromExcel.Enabled = false;
//                rpbProgress.Stopped = false;
//            }
//        }
//    }
//}
