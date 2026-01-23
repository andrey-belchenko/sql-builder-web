//using System;
//using Contract = System.Diagnostics.Contracts.Contract;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.XtraEditors;
////using DevExpress.XtraTreeList;
////using DevExpress.XtraTreeList.Nodes;
//using infoenergo.core.Extensions;
//using Microsoft.Win32;
//using sql.builder.XmlHelpers;
//using sql.builder.DataApi;

//namespace sql.builder.Controls.Containers
//{
//    internal partial class ucReportsTree : IDisposable
//    {
//        #region Закрытые переменные
//        // отчеты и настройки
//        private DataTable _dt_reports;
//        // private TreeListNode _last_node;
//        private string[] RootFolders;
//        private bool _initialized;
//        private Dictionary<string, object> _user_settings;
//        private string _reg_path;
//        private bool SupressFocusChanging;
//        #endregion
//        #region События
//        internal event Action<bool> ReportLayoutChanged;
//        internal event Action CurrentNodeChanged;
//        internal event Action ReportNodeDeleted;
//        internal event Action ReportNeedUpdate;
//        internal event Action<XElement, string> LoadGridSettings;
//        internal event Func<SettingsType, XElement> NeedSettingData;
//        #endregion
//        #region Открытые методы
//        public ucReportsTree()
//        {
//            InitializeComponent();
//        }
//        internal void Initialize(string[] folders)
//        {
//            this.RootFolders = folders;
//            this._user_settings = new Dictionary<string, object>();
//            string user_name = Environment.UserName;
//            if (Array.IsNullOrEmpty(folders)) {
//                this._reg_path = user_name + @"\sql.builder\folders\noroot\";
//            } else {
//                this._reg_path = user_name + @"\sql.builder\folders\" + string.Join(",", folders) + @"\";
//            }
//            this.LoadStateFromRegistry();
//            this._initialized = true;
//        }
//        internal string GetReportTemplateTitle(string report_name)
//        {
//            TreeListNode node = this.tlReports.FindNodeByFieldValue("name", report_name);
//            if (!node.IsDBNull("kod_gs")) {
//                return node.Field<string>("title");
//            } else {
//                return "по умолчанию";
//            }
//        }
//        internal void SetData(string name, XElement data)
//        {
//            TreeListNode node = this.tlReports.FindNodeByFieldValue("name", name);
//            if (node != null) {
//                node["data"] = data;
//                if (this.tlReports.FocusedNode == node) {
//                    this.ReloadData(name);
//                }
//            }            
//        }
//        internal void SetFocusedNode(string report_name)
//        {
//            this.tlReports.SetFocusedNode(this.tlReports.FindNodeByFieldValue("name", report_name));
//            this.tlReports.RefreshNode(this.tlReports.FocusedNode);
//        }
//        //public void SelectFirstNodeIfNeed()
//        //{
//        //    var reports = _dt_reports.AsEnumerable().Where(row => (string)row["item_type"] == "report");
//        //    if (reports.Count() == 1)
//        //    {
//        //        this.SetFocusedNode((string)reports.First()["name"]);
//        //        Cursor.Current = Cursors.WaitCursor;
//        //        if (CurrentNodeChanged != null)
//        //        {
//        //            CurrentNodeChanged();
//        //        }
//        //        Cursor.Current = Cursors.Default;
//        //    }
//        //}
//        internal Dictionary<string, string> GetFocusedReport()
//        {
//            return this.GetReportSettings(this.tlReports.FocusedNode);
//        }
//        internal TreeListNode GetFocusedNode()
//        {
//            return this.tlReports.FocusedNode;
//        }
//        internal bool IsInFolder(string folderName)
//        {
//            TreeListNode node = this.tlReports.FocusedNode;
//            bool res = false;
//            while (node != null) {
//                if (node.Field<string>("original_name") == folderName) {
//                    res = true;
//                    break;
//                }
//                node = node.ParentNode;
//            }
//            return res;
//        }
//        internal void SetReportLayoutChanged(string report_name, bool changed)
//        {
//            TreeListNode node = tlReports.FindNodeByFieldValue("name", report_name);
//            if (node == null) return;
//            decimal cur_val = node.Field<decimal>("changed");
//            if (cur_val != (changed ? decimal.One : decimal.Zero)) {
//                node["changed"] = changed ? Cmn.DECIMAL_ONE : Cmn.DECIMAL_ZERO;
//                if (this.ReportLayoutChanged != null) {
//                    this.ReportLayoutChanged(changed);
//                }
//                this.tlReports.RefreshNode(node);
//            }
//        }
//        internal bool GetReportLayoutChanged(string report_name)
//        {
//            if (this.tlReports.DataSource == null) {
//                return false;
//            }
//            TreeListNode node = this.tlReports.FindNodeByFieldValue("name", report_name);
//            if (node == null) {
//                return false;
//            } else {
//                return node.Field<decimal>("changed") > decimal.Zero;
//            }
//        }
//        internal void UpdateReportTemplate(string report_name, XElement data)
//        {
//            TreeListNode node = this.tlReports.FindNodeByFieldValue("name", report_name);
//            string sdata = data.ToString();
//            Contract.Assume(!node.IsDBNull("kod_gs"));
//            db.UpdateReportSettingData(node.Field<decimal>("kod_gs"), sdata);
//            node["data"] = data;
//            this.SetReportLayoutChanged(report_name, false);
//            this.ReloadAllWithoutData(false);
//        }
//        internal void DeleteReportTemplate(string report_name)
//        {
//            TreeListNode node = this.tlReports.FindNodeByFieldValue("name", report_name);
//            if (XtraMessageBox.Show("Вы уверены, что хотите удалить шаблон \"" + node.Field<string>("title") + "\"", "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) {
//                return;
//            }
//            db.DeleteReportSetting(node.Field<decimal>("kod_gs"));
//            if (this.ReportNodeDeleted != null) {
//                this.ReportNodeDeleted();
//            }
//            this.tlReports.DeleteNode(this.tlReports.FocusedNode);
//            this._dt_reports.AcceptChanges();
//        }
//        internal void CreateReportTemplate(string report_name)
//        {
//            //TreeListNode node = this.GetMainReportNode(report_name);
//            //Contract.Assume(report_name == node.Field<string>("name"));
//            TreeListNode node = this.tlReports.FindNodeByFieldValue("name", report_name);
//            Contract.Assume(node != null);
//            Contract.Assume(node.IsDBNull("kod_gs"));
//            string title = node.Field<string>("title");
//            // Собираем все используемые заглавия
//            HashSet<string> template_titles = new HashSet<string>();
//            DataColumn col_parent = this._dt_reports.Columns["parent"];
//            DataColumn col_title = this._dt_reports.Columns["title"];
//            string template_title;
//            int index;
//            for (index = 0; index < this._dt_reports.Rows.Count; index++) {
//                DataRow row = this._dt_reports.Rows[index];
//                if ((!row.IsNull(col_parent)) && report_name == row.Field<string>(col_parent)) {
//                    template_title = row.Field<string>(col_title);
//                    if (!template_titles.Contains(template_title)) {
//                        template_titles.Add(template_title);
//                    }
//                }
//            }
//            // Определяем заглавие отчёта
//            index = 1;
//            while (true) {
//                template_title = title + " " + index.ToString();
//                if (!template_titles.Contains(template_title)) {
//                    break;
//                }
//                index++;
//            }
//            decimal kod_gs = db.InsertReportSetting(report_name, template_title, NeedSettingData(SettingsType.SchemeAndParams).ToString());
//            string template_name = report_name + "_gs" + kod_gs.ToString();
//            this.ReloadAllWithoutData(false);
//            this.tlReports.FocusedNode = this.tlReports.FindNodeByFieldValue("name", template_name);
//            this.tcolItem.OptionsColumn.AllowEdit = true;
//            this.tlReports.ShowEditor();
//        }
//        /*internal Dictionary<string, string> GetMainReport(string report_name)
//        {
//            TreeListNode node = this.tlReports.FindNodeByFieldValue("name", report_name);
//            if (!node.IsDBNull("kod_gs")) {
//                node = node.ParentNode;
//            }
//            return this.GetReportSettings(node);
//        }*/
//        /*private TreeListNode GetMainReportNode(string report_name)
//        {
//            TreeListNode node = this.tlReports.FindNodeByFieldValue("name", report_name);
//            if (node == null) {
//                return null;
//            } else if (!node.IsDBNull("kod_gs")) {
//                return node.ParentNode;
//            } else {
//                return node;
//            }
//        }*/
//        internal Dictionary<string, string> GetReport(string report_name)
//        {
//            return this.GetReportSettings(this.tlReports.FindNodeByFieldValue("name", report_name));
//        }
//        private static bool IsVisibleUseReport(TreeListNode node)
//        {
//            return node.Field<string>("item_type") == "usereport" && Cmn.BOOLEAN_TRUE.Equals(node["visible"]); // node["visible"].ToString() != "0"
//        }
//        internal Tuple<string, string>[] ReloadAllReports()
//        {
//            var errors = new List<Tuple<string, string>>();
//            foreach (TreeListNode rn in this.GetAllNodes()) {
//                if (IsVisibleUseReport(rn)) {
//                    try {
//                        ReloadData(rn.Field<string>("name"));
//                    } catch (Exception ex) {
//                        errors.Add(new Tuple<string, string>(rn.Field<string>("title"), ex.Message));
//                    }
//                }
//            }
//            return errors.ToArray();
//        }
//        internal Dictionary<string, string>[] GetAllVisibleReports()
//        {
//            var list = new List<Dictionary<string, string>>();
//            foreach (TreeListNode node in this.GetAllNodes()) {
//                if (IsVisibleUseReport(node)) {
//                    list.Add(this.GetReportSettings(node));
//                }
//            }
//            return list.ToArray();
//        }
//        internal void ReloadData(string report_name)
//        {
//            this.LoadData(report_name);
//            if (this.LoadGridSettings != null) {
//                TreeListNode node = this.tlReports.FindNodeByFieldValue("name", report_name);
//                this.LoadGridSettings(node.Field<XElement>("data"), report_name);
//            }
//        }
//        internal void ReloadAllWithoutData(bool full_reload)
//        {
//            this.SupressFocusChanging = true;
//            // загружаем информацию об отчетах
//            DataTable dt = SqlBuilder.GetReportsDataTable(this.RootFolders, true);
//            if (full_reload) {
//                this._dt_reports = dt;
//                this.tlReports.DataSource = dt;
//                this.tlReports.ForceInitialize();
//                // учтанавливаем параметры expanded из реестра
//                this.UpdateExpandedState();
//                // чтобы не глючило
//                TreeListNode node = this.tlReports.Nodes.FirstNode;
//                if (node != null) {
//                    this.tlReports.MakeNodeVisible(node);
//                }
//            } else {
//                dt.Columns.Remove("changed");
//                GridDesigner.DataSourceRightJoin(this._dt_reports, dt, "name");
//            }
//            this.SupressFocusChanging = false;
//        }
//        #endregion
//        #region Закрытые методы
//        private void LoadData(string name)
//        {
//            TreeListNode node = this.tlReports.FindNodeByFieldValue("name", name);
//            if (node.IsDBNull("data")) {
//                object kod_gs = node["kod_gs"];
//                XElement data;
//                if (!Convert.IsDBNull(kod_gs)) {
//                    data = XElement.Parse(db.SelectSettingData(Convert.ToDecimal(kod_gs)));
//                } else {
//                    string project = node.Field<string>("project");
//                    VReport report = XmlReports.Environment.GetPrecompiledReport(name, project);
//                    data = new XElement(EName.root, report.GetSchemeWithColumnsPreset());
//                }
//                node["data"] = data;
//            }
//        }
//        private Dictionary<string, string> GetReportSettings(TreeListNode node)
//        {
//            var dict = new Dictionary<string, string>();
//            dict.Add("original_name", node.Field<string>("original_name"));
//            string name = node.Field<string>("name");
//            dict.Add("repname", name);
//            dict.Add("title", node.Field<string>("title"));
//            dict.Add("item_type", node.Field<string>("item_type"));
//            dict.Add("visible", node["visible"].ToString());
//            dict.Add("old", node["old"].ToString());

//            //TreeListNode report_node = this.GetMainReportNode(name);
//            //dict.Add("project", report_node.Field<string>("project"));
//            dict.Add("project", node.Field<string>("project"));
//            return dict;
//        }
//        private IEnumerable<TreeListNode> GetAllNodes()
//        {
//            return tlReports.Nodes.SelectMany(Cmn.GetNodeBranch);
//        }
//        /*private static IEnumerable<TreeListNode> GetNodeBranch(TreeListNode node)
//        {
//            yield return node;
//            foreach (TreeListNode child in node.Nodes) {
//                foreach (TreeListNode childChild in GetNodeBranch(child)) {
//                    yield return childChild;
//                }
//            }
//        }*/
//        private void UpdateExpandedState()
//        {
//            foreach (TreeListNode node in this.GetAllNodes()) {
//                string item_type = node.Field<string>("item_type");
//                if (item_type == "folder" || item_type == "usereport") {
//                    object expanded;
//                    this._user_settings.TryGetValue(node.Field<string>("name") + "_expanded", out expanded);
//                    if (expanded != null) {
//                        node.Expanded = Convert.ToBoolean(expanded);
//                    }
//                }
//            }
//        }
//        internal void ReadExpandedState()
//        {
//            this._user_settings.Clear();
//            foreach (TreeListNode node in this.GetAllNodes()) {
//                string item_type = node.Field<string>("item_type");
//                if (item_type == "folder" || item_type == "usereport") {
//                    this._user_settings.Add(node.Field<string>("name") + "_expanded", node.Expanded);
//                }
//            }
//        }
//        private void LoadStateFromRegistry()
//        {
//            using (var reg_settings = Registry.CurrentUser.CreateSubKey(_reg_path, RegistryKeyPermissionCheck.ReadSubTree)) {
//                _user_settings.Clear();
//                foreach (var reg_name in reg_settings.GetValueNames()) _user_settings.Add(reg_name, reg_settings.GetValue(reg_name));
//            }
//        }
//        private void SaveStateToRegistry()
//        {
//            ReadExpandedState();
//            using (var reg_settings = Registry.CurrentUser.CreateSubKey(_reg_path, RegistryKeyPermissionCheck.ReadWriteSubTree)) {
//                foreach (var _user_setting in _user_settings) {
//                    reg_settings.SetValue(_user_setting.Key, _user_setting.Value);
//                }
//            }
//        }
//        #endregion
//        #region Обработчики событий
//        private void ucReportsTree_Load(object sender, EventArgs e)
//        {
//            this.UpdateExpandedState();
//        }
//        private void tlReports_DoubleClick(object sender, EventArgs e)
//        {
//            var tree = sender as DevExpress.XtraTreeList.TreeList;
//            var info = tree.CalcHitInfo(tree.PointToClient(MousePosition));
//            if (info.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Cell && !info.Node.IsDBNull("kod_gs")) {
//                tcolItem.OptionsColumn.AllowEdit = true;
//                tlReports.ShowEditor();
//            }
//        }
//        private void tlReports_BeforeFocusNode(object sender, DevExpress.XtraTreeList.BeforeFocusNodeEventArgs e)
//        {
//            // папки нельзя брать в фокус
//            if (SupressFocusChanging || e.Node.Field<string>("item_type") == "folder") {
//                e.CanFocus = false;
//                return;
//            }
//            if (e.OldNode != null && e.OldNode["name"] != DBNull.Value) {
//                if (e.OldNode.TreeList == null) return; //иначе при удалении шаблона ошибка, TreeList пустой
//                if (e.OldNode is TreeListAutoFilterNode) return;
//                if (!Cmn.DECIMAL_ZERO.Equals(e.OldNode["changed"])) {
//                    if (NeedSettingData != null) {
//                        var data = NeedSettingData(SettingsType.All);
//                        if (data != null) {
//                            e.OldNode["data"] = data;
//                        }
//                    }
//                }
//            }
//        }
//        private void tlReports_FocusedNodeChanged(object sender, DevExpress.XtraTreeList.FocusedNodeChangedEventArgs e)
//        {
//            if (SupressFocusChanging || tlReports.FocusedNode == null) return;
//            if (e.Node is TreeListAutoFilterNode) return;
//            Cursor.Current = Cursors.WaitCursor;
//            if (CurrentNodeChanged != null) {
//                CurrentNodeChanged();
//            }
//            //if (ReportLayoutChanged != null)
//            //{
//            //    ReportLayoutChanged((decimal)tlReportSettings.FocusedNode["CHANGED"] != decimal.Zero);
//            //}
//            Cursor.Current = Cursors.Default;
//        }
//        private void tlReports_Click(object sender, EventArgs e)
//        {
//            TreeListHitInfo info = this.tlReports.CalcHitInfo(this.tlReports.PointToClient(MousePosition));
//            if (info.HitInfoType == DevExpress.XtraTreeList.HitInfoType.Cell) {
//                TreeListNode node = this.tlReports.FocusedNode;
//                if (node != null && !(node is TreeListAutoFilterNode)) {
//                    if (this.CurrentNodeChanged != null) {
//                        Cursor.Current = Cursors.WaitCursor;
//                        this.CurrentNodeChanged();
//                        Cursor.Current = Cursors.Default;
//                    }
//                }
//            }
//        }
//        private void tlReports_HiddenEditor(object sender, EventArgs e)
//        {
//            this.tcolItem.OptionsColumn.AllowEdit = false;
//            TreeListNode node = this.tlReports.FocusedNode;
//            //if (node.TreeList.Nodes.AutoFilterNode != node) {
//            if (node != null && !(node is TreeListAutoFilterNode)) {
//                db.UpdateReportSettingName(node.Field<decimal>("kod_gs"), node.Field<string>("title"));
//                if (this.ReportNeedUpdate != null) {
//                    this.ReportNeedUpdate();
//                }
//            }
//        }
//        private void tlReports_CustomDrawNodeCell(object sender, DevExpress.XtraTreeList.CustomDrawNodeCellEventArgs e)
//        {
//            TreeListNode node = e.Node;
//            if ((!(node is TreeListAutoFilterNode)) && !node.IsDBNull("kod_gs")) {
//                e.Appearance.Font = new Font(e.Appearance.Font.FontFamily, e.Appearance.Font.Size, Cmn.DECIMAL_ONE.Equals(node["changed"]) ? FontStyle.Bold : FontStyle.Regular);
//            }
//        }
//        private void tlReports_KeyDown(object sender, KeyEventArgs e)
//        {
//            if (e.Control && e.KeyCode == Keys.Q) {
//                TreeListNode node = tlReports.FocusedNode;
//                if (node != null && this.NeedSettingData != null) {
//                    string fileName = String.Format(@"{0}\{1}_{2}.xml", Path.GetTempPath(), node["parent"] != DBNull.Value ? node["parent"] : node["name"], node["parent"] != DBNull.Value ? node["name"] : "по_умолчанию");
//                    File.WriteAllText(fileName, this.NeedSettingData(SettingsType.All).ToString());
//                    System.Diagnostics.Process.Start(fileName);
//                }
//            }
//        }
//        private void tlReports_GetNodeDisplayValue(object sender, DevExpress.XtraTreeList.GetNodeDisplayValueEventArgs e)
//        {
//            if (e.Column.FieldName == "title" && XmlReports.IsDeveloperMode() && !(e.Node is TreeListAutoFilterNode)) {
//                string pr = !string.IsNullOrEmpty(e.Node["project"].ToString()) ? e.Node["project"] + "." : "";
//                e.Value = e.Value + " ("  + pr + e.Node.Field<string>("name") + ")";
//            }
//        }
//        private void tlReports_AfterExpand(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
//        {
//            if (!_initialized || SupressFocusChanging) return;
//            SaveStateToRegistry();
//        }
//        private void tlReports_AfterCollapse(object sender, DevExpress.XtraTreeList.NodeEventArgs e)
//        {
//            if (!_initialized || SupressFocusChanging) return;
//            SaveStateToRegistry();
//        }
//        #endregion
//        private void btnCopyFullName_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            //TreeListHitInfo hitInfo = this.tlReports.CalcHitInfo(this.tlReports.PointToClient(Cursor.Position));
//            //if (hitInfo.Node == null || hitInfo.Node is TreeListAutoFilterNode) return;
//            //var sb = new StringBuilder();
//            //TreeListNode cur = hitInfo.Node;
//            //while (cur != null) {
//            //    string title = cur.Field<string>("title");
//            //    if (cur == hitInfo.Node) {
//            //        sb.Insert(0, title);
//            //    } else {
//            //        sb.Insert(0, title + @"\");
//            //    }
//            //    cur = cur.ParentNode;
//            //}
//            //Clipboard.SetText(sb.ToString());
//        }
//        private void tlReports_MouseClick(object sender, MouseEventArgs args)
//        {
//            if (args.Button == MouseButtons.Right) {
//                this.popupMenu.ShowPopup(this.tlReports.PointToScreen(args.Location));
//            }
//        }
//        internal void SetInvisibleReportsVisible(bool visible)
//        {
//            this.tlReports.ActiveFilterString = (visible) ? string.Empty : "visible != 0";
//        }

//        public void Dispose()
//        {
//            //throw new NotImplementedException();
//        }
//    }
//}