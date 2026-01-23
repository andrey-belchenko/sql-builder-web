//using System.Data;
//using System.Diagnostics;
//using System.IO;
////using System.Windows.Forms;
//using infoenergo.ui.win.Forms;
//using Microsoft.WindowsAPICodePack.Dialogs;
//using sql.builder.XmlHelpers;

//namespace sql.builder.WinForms
//{
//    internal partial class frmScriptingTest : FormBase
//    {
//        DataTable _dt;
//        public frmScriptingTest()
//        {
//            InitializeComponent();

//            _dt = new DataTable();
//            _dt.Columns.Add("file");
//            gridControl1.DataSource = _dt;

//            string fn = SettingsHelper.TestSettingsFolder;
//            if (string.IsNullOrEmpty(fn))
//            {
//                SettingsHelper.TestSettingsFolder = fn = @"\\DFS01\Common\sql.builder\Тестирование";
//            }

//            if(Directory.Exists(fn))
//            {
//                LoadSettingsList(fn);
//            }
//        }

//        private void btnChoseSettingsFolder_Click(object sender, System.EventArgs e)
//        {
//            using (var dlg = new CommonOpenFileDialog())
//            {
//                dlg.InitialDirectory = SettingsHelper.TestSettingsFolder;
//                dlg.IsFolderPicker = true;
//                if (dlg.ShowDialog() == CommonFileDialogResult.Ok)
//                {
//                   SettingsHelper.TestSettingsFolder = dlg.FileName;
//                   LoadSettingsList(dlg.FileName);
//                }
//            }

//            Activate();
//        }

//        private void LoadSettingsList(string folder_path)
//        {
//            var files = Directory.GetFiles(folder_path, "*.xml");
//            _dt.Rows.Clear();
//            foreach (var file in files)
//            {
//                _dt.Rows.Add(file);
//            }
//        }

//        private void btnStart_Click(object sender, System.EventArgs e)
//        {
//            if (_dt.Rows.Count == 0) return;

//            foreach (var row in _dt.AsEnumerable())
//            {
//                var process = new Process();
//                process.StartInfo.FileName = Path.Combine(Application.StartupPath, "sql.builder.exe");
//                process.StartInfo.Arguments = string.Format("-server -file=\"{0}\" -compare={1}", row["file"], checkEdit1.Checked ? "1" : "0");
//                process.Start();
//            }

//            Close();
//        }

//        private void checkEdit1_EditValueChanging(object sender, DevExpress.XtraEditors.Controls.ChangingEventArgs e)
//        {
//            if (!Cmn.IsAdministrator() && (bool)e.NewValue)
//            {
//                e.Cancel = true;
//                ShowMessage.ShowExclamation("Для сравнения с другой версией запустите приложение с правами администратора");
//            }
//        }
//    }
//}
