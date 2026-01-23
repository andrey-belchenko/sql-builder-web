//using System;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.XtraEditors;
//using infoenergo.ui.win.Forms;

//namespace sql.builder.WinForms
//{
//    internal partial class frmSaveLoadGS : FormBase
//    {
//        public frmSaveLoadGS()
//        {
//            InitializeComponent();
//        }

//        public XElement Setting { set; get; }
//        public int SettingsCount { get { return _dt_settings.Rows.Count; } }

//        private string _report_name;
//        private Mode _mode;
//        private DataTable _dt_settings;
//        private decimal _current_kod_gs;

//        public void Initialize(string report_name, Mode mode, decimal selected_kod_gs = 0M)
//        {
//            _report_name = report_name;

//            LoadData();

//            var index = viewSettings.LocateByValue("KOD_GS", selected_kod_gs);
//            if (index >= 0)
//            {
//                viewSettings.FocusedRowHandle = index;
//                _current_kod_gs = selected_kod_gs;
//            }
//            else _current_kod_gs = 0M;

//            SetMode(mode);
//        }
//        public decimal GetCurrentKodGs()
//        {
//            return _current_kod_gs;
//        }

//        private void LoadData()
//        {
//            _dt_settings = db.SelectReportVisibleSettings(_report_name);
//            _dt_settings.PrimaryKey = new[] {_dt_settings.Columns["KOD_GS"]};
//            grSettings.DataSource = _dt_settings;
//        }

//        private void SetMode(Mode mode)
//        {
//            _mode = mode;

//            if (mode == Mode.Load)
//            {
//                this.Text = "Загрузка шаблона";
//                pSettingTitle.Visible = false;
//            }
//            else if (mode == Mode.Save)
//            {
//                this.Text = "Сохранение шаблона";
//                pSettingTitle.Visible = true;
//                if(_current_kod_gs == 0M) teSettingTitle.EditValue = GenerateFreeName();
//                teSettingTitle.Focus();
//                teSettingTitle.SelectAll();
//            }
//        }

//        private string GenerateFreeName()
//        {
//            string str = "Новый шаблон ";
//            string[] titles = _dt_settings.AsEnumerable().Select(r => r["title"].ToString()).ToArray();

//            int ind = 1;
//            string title = null;
//            do
//            {
//                title = str + ind++;
//            } while (titles.Contains(title));

//            return title;
//        }

//        private void btnAccept_Click(object sender, EventArgs e)
//        {
//            if (_mode == Mode.Load)
//            {
//                var row = viewSettings.GetFocusedDataRow();
//                if (row == null) return;

//                _current_kod_gs = (decimal)row["kod_gs"];
//                Setting = XElement.Parse(db.SelectSettingData(_current_kod_gs));
//            }
//            else if (_mode == Mode.Save)
//            {
//                var title = teSettingTitle.Text;

//                if (title == "")
//                {
//                    XtraMessageBox.Show("Недопустимое имя шаблона", "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
//                    return;
//                }

//                var row = _dt_settings.AsEnumerable().FirstOrDefault(r => r["title"].ToString() == title);
//                if (row != null)
//                {
//                    var result = XtraMessageBox.Show(string.Format("Шаблон \"{0}\" уже существует. Перезаписать?", title), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//                    if (result != DialogResult.Yes) return;

//                    _current_kod_gs = (decimal) row["kod_gs"];
//                    db.UpdateReportSettingData(_current_kod_gs, Setting.ToString());
//                }
//                else
//                {
//                    _current_kod_gs = db.InsertReportSetting(_report_name, title, Setting.ToString());
//                }
//            }

//            this.DialogResult = DialogResult.OK;
//        }

//        private void btnCancel_Click(object sender, EventArgs e)
//        {
//            this.DialogResult = DialogResult.Cancel;
//        }

//        private void viewSettings_FocusedRowChanged(object sender, DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventArgs e)
//        {
//            var row = viewSettings.GetFocusedDataRow();
//            if (row == null) return;

//            if (_mode == Mode.Save)
//            {
//                teSettingTitle.Text = row["TITLE"].ToString();
//            }
//        }

//        private void btnDeleteSetting_Click(object sender, EventArgs e)
//        {
//            var row = viewSettings.GetFocusedDataRow();
//            if (row == null) return;

//            var result = XtraMessageBox.Show(string.Format("Вы уверены, что хотите удалить \"{0}\"?", row["TITLE"]), "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//            if (result != DialogResult.Yes) return;

//            db.DeleteReportSetting((decimal)row["kod_gs"]);
//            LoadData();
//        }

//        internal enum Mode
//        {
//            Save,
//            Load
//        }

//        private void viewSettings_RowCellClick(object sender, DevExpress.XtraGrid.Views.Grid.RowCellClickEventArgs e)
//        {
//            if (e.Button == MouseButtons.Left && e.Clicks > 1)
//            {
//                btnAccept.PerformClick();
//                btnAcceptT.PerformClick();
//            }
//        }

//        private void btnDeleteSettingT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            btnDeleteSetting_Click(sender, null);
//        }

//        private void btnAcceptT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            btnAccept_Click(sender, null);
//        }

//        private void btnCancelT_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            btnCancel_Click(sender, null);
//        }
//    }
//}
