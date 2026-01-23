//using System.Data;
//using System.Linq;
//using DevExpress.XtraEditors;

//namespace sql.builder.Controls
//{
//    internal partial class ucSaveLoadForm : XtraUserControl
//    {
//        private DataTable _dt_settings;

//        public ucSaveLoadForm()
//        {
//            InitializeComponent();
//        }

//        public void Initialize(string report_name, Mode mode)
//        {
//            grSettings.DataSource = _dt_settings = db.SelectReportVisibleSettings(report_name);

//            SetMode(mode);
//        }

//        private void SetMode(Mode mode)
//        {
//            if (mode == Mode.Load)
//            {
//                pSettingName.Visible = false;
//            }
//            else if (mode == Mode.Save)
//            {
//                pSettingName.Visible = true;
//                teSettingName.EditValue = GenerateFreeName();
//                teSettingName.Focus();
//                teSettingName.SelectAll();
//            }
//        }

//        private string GenerateFreeName()
//        {
//            string str = "Новая настройка ";
//            string[] titles = _dt_settings.AsEnumerable().Select(r => r["title"].ToString()).ToArray();

//            int ind = 1;
//            string title = null;
//            do
//            {
//                title = str + ind++;
//            } while (!titles.Contains(title));

//            return title;
//        }

//        internal enum Mode
//        {
//            Save,
//            Load
//        }       
//    }
//}
