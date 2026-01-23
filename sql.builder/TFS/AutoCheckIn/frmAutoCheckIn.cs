//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;
////using DevExpress.Utils;
////using DevExpress.Utils.Taskbar.Core;
////using DevExpress.XtraBars;
////using DevExpress.XtraEditors;
////using DevExpress.XtraEditors.Repository;

//namespace sql.builder.TFS.AutoCheckIn
//{
//    internal partial class frmAutoCheckIn : XtraForm
//    {
//        private AutoCheckInController _controller;
//        private DataTable _logTable;
//        private DataTable _schemeConditionsTable;
//        public frmAutoCheckIn()
//        {
//            InitializeComponent();
//        }
//        public frmAutoCheckIn(AutoCheckInController controller)
//            : this()
//        {
//            this._controller = controller;
//            this._controller.Announcer.SomethingHappened += controller_SomethingHappened;
//            this._logTable = new DataTable();
//            this._logTable.Columns.Add("status", typeof(int));
//            this._logTable.Columns.Add("time", typeof(DateTime));
//            this._logTable.Columns.Add("text", typeof(string));
//            this.grLog.DataSource = _logTable;
//            this._schemeConditionsTable = new DataTable();
//            this._schemeConditionsTable.Columns.Add("id", typeof(int));
//            this._schemeConditionsTable.Columns.Add("name", typeof(string));
//            this._schemeConditionsTable.AddRow(Cmn.INT32_ZERO, "Нет");
//            this._schemeConditionsTable.AddRow(Cmn.INT32_ONE, "Если были изменения");
//            this._schemeConditionsTable.AddRow(Cmn.INT32_TWO, "Да");
//            this.rleRebuildScheme.DataSource = _schemeConditionsTable;
//            this.rleRebuildScheme.ValueMember = "id";
//            this.rleRebuildScheme.DisplayMember = "name";
//            this.grMainFolders.DataSource = new AutoCheckInFolderSetting[1] { controller.MainFoldersSettings };
//            this.grBranchFolders.DataSource = controller.BranchFoldersSettings;
//            this.InitProjectsList();
//            this.meComment.Text = controller.Comment;
//        }
//        private void controller_SomethingHappened(object sender, AutoCheckInControllerEventArgs e)
//        {
//            this.InvokeIfNeed(() => {
//                if(e.Status != AutoCheckInControllerStatus.Completed) {
//                    DataRow row = _logTable.NewRow();
//                    row["status"] = (e.Status == AutoCheckInControllerStatus.Error) ? Cmn.INT32_ONE : Cmn.INT32_ZERO;
//                    row["time"] = e.Time;
//                    row["text"] = e.Message;
//                    _logTable.Rows.InsertAt(row, 0);
//                    viewLog.TopRowIndex = 0;
//                }
//                if (e.Status == AutoCheckInControllerStatus.Error || e.Status == AutoCheckInControllerStatus.Completed) {
//                    SetUIMode(UIMode.Ready);
//                } 
//            });
//        }
//        private void meComment_EditValueChanged(object sender, EventArgs e)
//        {
//            this._controller.Comment = meComment.Text;
//        }
//        private void SetUIMode(UIMode mode)
//        {
//            if (mode == UIMode.Ready) {
//                this.btnStart.Enabled = true;
//                this.btnCancel.Enabled = false;
//                this.blScenarios.Enabled = true;
//                this.blProjects.Enabled = true;
//                this.rpbProgress.Stopped = true;
//                this.meComment.ReadOnly = false;
//                this.viewMainFolders.OptionsBehavior.ReadOnly = false;
//                this.viewBranchFolders.OptionsBehavior.ReadOnly = false;
//                this.tba.ProgressMode = TaskbarButtonProgressMode.NoProgress;
//            } else if (mode == UIMode.InProgress) {
//                this.btnStart.Enabled = false;
//                this.btnCancel.Enabled = true;
//                this.blScenarios.Enabled = false;
//                this.blProjects.Enabled = false;
//                this.rpbProgress.Stopped = false;
//                this.meComment.ReadOnly = true;
//                this.viewMainFolders.OptionsBehavior.ReadOnly = true;
//                this.viewBranchFolders.OptionsBehavior.ReadOnly = true;
//                this.tba.ProgressMode = TaskbarButtonProgressMode.Indeterminate;
//            } else if (mode == UIMode.Loading) {
//                this.btnStart.Enabled = false;
//                this.btnCancel.Enabled = false;
//                this.blScenarios.Enabled = true;
//                this.blProjects.Enabled = true;
//                this.rpbProgress.Stopped = false;
//                this.meComment.ReadOnly = false;
//                this.viewMainFolders.OptionsBehavior.ReadOnly = false;
//                this.viewBranchFolders.OptionsBehavior.ReadOnly = false;
//                this.tba.ProgressMode = TaskbarButtonProgressMode.Indeterminate;
//            }
//        }
//        private void InvokeIfNeed(MethodInvoker action)
//        {
//            if (this.IsDisposed) {
//                return;
//            }
//            if (this.InvokeRequired) {
//                this.Invoke(action);
//            } else {
//                action();
//            }
//        }
//        private void frmAutoCheckIn_FormClosing(object sender, FormClosingEventArgs e)
//        {
//            this._controller.Cancel();
//            this._controller.SaveState();
//        }
//        private void frmAutoCheckIn_Load(object sender, EventArgs e)
//        {
//            this.SetUIMode(UIMode.Loading);
//            this._controller.InitializeAsync();
//        }
//        private void btnStart_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this.viewMainFolders.CloseEditor();
//            this.viewBranchFolders.CloseEditor();
//            this.SetUIMode(UIMode.InProgress);
//            this._controller.StartAsync();
//            this._controller.SaveState();
//        }
//        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this.SetUIMode(UIMode.Loading);
//            this._controller.Cancel();
//        }
//        private void rceCheck_EditValueChanged(object sender, EventArgs e)
//        {
//            this.viewBranchFolders.CloseEditor();
//        }
//        //private void viewBranchFolders_CustomDrawCell(object sender, DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventArgs e)
//        //{
//            //if(e.Column.FieldName == "QueueBuild")
//            //{
//            //    var setting = viewBranchFolders.GetRow(e.RowHandle) as AutoCheckInFolderSetting;
//            //    if(setting.Config.GatedCheckIn == "true")
//            //    {
//            //        e.Appearance.BackColor = Color.LightGray;
//            //    }
//            //}
//        //}
//        private void btnScenario1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this._controller.ClearFoldersSettings();
//            AutoCheckInFolderSetting fold = this._controller.MainFoldersSettings;
//            fold.GetChanges = true;
//            fold.RebuildScheme = 1;
//            fold.CheckIn = true;
//            this.viewMainFolders.RefreshData();
//            this.viewBranchFolders.RefreshData();
//        }
//        private void btnScenario2_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this._controller.ClearFoldersSettings();
//            AutoCheckInFolderSetting fold = this._controller.MainFoldersSettings;
//            fold.GetChanges = true;
//            fold.RebuildScheme = 1;
//            fold.CheckIn = true;
//            fold = this._controller.BranchFoldersSettings.First(f => f.Name == "kido.release");
//			fold.GetChanges = true;
//			fold.CheckIn = true;
//			fold.QueueBuild = true;
//			//foreach (var fold2 in _controller.BranchFoldersSettings.Where(f => f.Name.StartsWith("xdev.kido")))
//			//{
//			//	fold2.GetChanges = true;
//			//	fold2.CheckIn = true;
//			//	fold2.QueueBuild = true;
//			//}
//            this.viewMainFolders.RefreshData();
//            this.viewBranchFolders.RefreshData();
//        }
//        private void btnScenario4_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this._controller.ClearFoldersSettings();
//            AutoCheckInFolderSetting fold = this._controller.BranchFoldersSettings.First(f => f.Name == "kido.release");
//			fold.GetChanges = true;
//			fold.CheckIn = true;
//			fold.QueueBuild = true;
//			//foreach (var fold2 in _controller.BranchFoldersSettings.Where(f => f.Name.StartsWith("xdev.kido")))
//			//{
//			//	fold2.GetChanges = true;
//			//	fold2.CheckIn = true;
//			//	fold2.QueueBuild = true;
//			//}
//            this.viewMainFolders.RefreshData();
//            this.viewBranchFolders.RefreshData();
//        }
//        private void btnScenario5_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this._controller.ClearFoldersSettings();
//            foreach (AutoCheckInFolderSetting fold2 in this._controller.BranchFoldersSettings) {
//                if (fold2.Name.StartsWith("3.9.28.") || fold2.Name.StartsWith("3.9.29.")) {
//                    fold2.GetChanges = true;
//                    fold2.CheckIn = true;
//                    fold2.QueueBuild = true;
//                }
//            }
//            this.viewMainFolders.RefreshData();
//            this.viewBranchFolders.RefreshData();
//        }
//        private void btnScenario6_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this._controller.ClearFoldersSettings();
//            AutoCheckInFolderSetting fold = this._controller.MainFoldersSettings;
//            fold.GetChanges = true;
//            fold.RebuildScheme = 1;
//            this.viewMainFolders.RefreshData();
//            this.viewBranchFolders.RefreshData();
//        }
//        private void InitProjectsList()
//        {
//            var re = new RepositoryItemCheckEdit();
//            re.GlyphAlignment = HorzAlignment.Far;
//            re.Caption = string.Empty;
//            foreach (var ps in this._controller.ProjectsSettings) {
//                var item = new BarEditItem(this.barManager1, re);
//                item.Caption = ps.Name;
//                item.DataBindings.Add("EditValue", ps, "Selected", false, DataSourceUpdateMode.OnPropertyChanged);
//                blProjects.AddItem(item);
//            }
//        }
//        private void btnScenario7_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            this._controller.ClearFoldersSettings();
//            AutoCheckInFolderSetting fold = this._controller.MainFoldersSettings;
//            fold.GetChanges = true;
//            fold.RebuildScheme = 1;
//            fold.CheckIn = true;
//            foreach (AutoCheckInFolderSetting fold2 in this._controller.BranchFoldersSettings) {
//                if (fold2.Name.StartsWith("3.9.28.") || fold2.Name.StartsWith("3.9.29.")) {
//                    fold2.GetChanges = true;
//                    fold2.CheckIn = true;
//                    fold2.QueueBuild = true;
//                }
//            }
//            this.viewMainFolders.RefreshData();
//            this.viewBranchFolders.RefreshData();
//        }
//    }

//    internal enum UIMode
//    {
//        Ready,
//        InProgress,
//        Loading
//    }
//}
