//namespace sql.builder.TFS.AutoCheckIn
//{
//    internal partial class frmAutoCheckIn
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary>
//        /// Clean up any resources being used.
//        /// </summary>
//        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
//        protected override void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAutoCheckIn));
//            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
//            this.grBranchFolders = new DevExpress.XtraGrid.GridControl();
//            this.viewBranchFolders = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colBranchName = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colBranchCheckIn = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rceCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.colBranchMerge = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colBranchQueueBuild = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.grMainFolders = new DevExpress.XtraGrid.GridControl();
//            this.viewMainFolders = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colMainFolderName = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colMainGetChanges = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colMainRebuildScheme = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rleRebuildScheme = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
//            this.colMainCheckIn = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colMainQueueBuild = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.meComment = new DevExpress.XtraEditors.MemoEdit();
//            this.grLog = new DevExpress.XtraGrid.GridControl();
//            this.viewLog = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rtiTime = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
//            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.riiStatus = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
//            this.ic2 = new DevExpress.Utils.ImageCollection(this.components);
//            this.colText = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rmeText = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
//            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
//            this.layoutControlGroup2 = new DevExpress.XtraLayout.LayoutControlGroup();
//            this.layoutControlGroup4 = new DevExpress.XtraLayout.LayoutControlGroup();
//            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
//            this.layoutControlGroup5 = new DevExpress.XtraLayout.LayoutControlGroup();
//            this.layoutControlItem6 = new DevExpress.XtraLayout.LayoutControlItem();
//            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
//            this.layoutControlGroup3 = new DevExpress.XtraLayout.LayoutControlGroup();
//            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
//            this.splitterItem1 = new DevExpress.XtraLayout.SplitterItem();
//            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
//            this.barFooter = new DevExpress.XtraBars.Bar();
//            this.btnStart = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
//            this.blScenarios = new DevExpress.XtraBars.BarLinkContainerItem();
//            this.btnScenario1 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnScenario2 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnScenario7 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnScenario4 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnScenario5 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnScenario6 = new DevExpress.XtraBars.BarButtonItem();
//            this.pbProgress = new DevExpress.XtraBars.BarEditItem();
//            this.rpbProgress = new DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar();
//            this.blProjects = new DevExpress.XtraBars.BarLinkContainerItem();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            this.repositoryItemCheckedComboBoxEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
//            this.repositoryItemPopupContainerEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit();
//            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.tba = new DevExpress.Utils.Taskbar.TaskbarAssistant();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
//            this.layoutControl1.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.grBranchFolders)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewBranchFolders)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceCheck)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grMainFolders)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewMainFolders)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleRebuildScheme)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.meComment.Properties)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grLog)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewLog)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rtiTime)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.riiStatus)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeText)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rpbProgress)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
//            this.SuspendLayout();
            // 
            // layoutControl1
            // 
//            this.layoutControl1.Controls.Add(this.grBranchFolders);
//            this.layoutControl1.Controls.Add(this.grMainFolders);
//            this.layoutControl1.Controls.Add(this.meComment);
//            this.layoutControl1.Controls.Add(this.grLog);
//            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.layoutControl1.Location = new System.Drawing.Point(0, 39);
//            this.layoutControl1.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.layoutControl1.Name = "layoutControl1";
//            this.layoutControl1.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(769, 130, 545, 591);
//            this.layoutControl1.Root = this.layoutControlGroup1;
//            this.layoutControl1.Size = new System.Drawing.Size(1151, 622);
//            this.layoutControl1.TabIndex = 9;
//            this.layoutControl1.Text = "layoutControl1";
            // 
            // grBranchFolders
            // 
//            this.grBranchFolders.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
//            this.grBranchFolders.Location = new System.Drawing.Point(11, 262);
//            this.grBranchFolders.MainView = this.viewBranchFolders;
//            this.grBranchFolders.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.grBranchFolders.Name = "grBranchFolders";
//            this.grBranchFolders.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rceCheck});
//            this.grBranchFolders.Size = new System.Drawing.Size(699, 349);
//            this.grBranchFolders.TabIndex = 16;
//            this.grBranchFolders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewBranchFolders});
            // 
            // viewBranchFolders
            // 
//            this.viewBranchFolders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colBranchName,
//            this.colBranchCheckIn,
//            this.colBranchMerge,
//            this.colBranchQueueBuild});
//            this.viewBranchFolders.GridControl = this.grBranchFolders;
//            this.viewBranchFolders.Name = "viewBranchFolders";
//            this.viewBranchFolders.OptionsBehavior.AutoPopulateColumns = false;
//            this.viewBranchFolders.OptionsCustomization.AllowColumnMoving = false;
//            this.viewBranchFolders.OptionsCustomization.AllowFilter = false;
//            this.viewBranchFolders.OptionsCustomization.AllowGroup = false;
//            this.viewBranchFolders.OptionsCustomization.AllowQuickHideColumns = false;
//            this.viewBranchFolders.OptionsCustomization.AllowSort = false;
//            this.viewBranchFolders.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.viewBranchFolders.OptionsSelection.EnableAppearanceFocusedRow = false;
//            this.viewBranchFolders.OptionsView.ColumnAutoWidth = false;
//            this.viewBranchFolders.OptionsView.ShowGroupPanel = false;
//            this.viewBranchFolders.OptionsView.ShowIndicator = false;
//            this.viewBranchFolders.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            // 
            // colBranchName
            // 
//            this.colBranchName.Caption = "Ветвь";
//            this.colBranchName.FieldName = "Name";
//            this.colBranchName.Name = "colBranchName";
//            this.colBranchName.OptionsColumn.AllowEdit = false;
//            this.colBranchName.Visible = true;
//            this.colBranchName.VisibleIndex = 0;
//            this.colBranchName.Width = 120;
            // 
            // colBranchCheckIn
            // 
//            this.colBranchCheckIn.Caption = "Вернуть изменения";
//            this.colBranchCheckIn.ColumnEdit = this.rceCheck;
//            this.colBranchCheckIn.FieldName = "CheckIn";
//            this.colBranchCheckIn.Name = "colBranchCheckIn";
//            this.colBranchCheckIn.Visible = true;
//            this.colBranchCheckIn.VisibleIndex = 2;
//            this.colBranchCheckIn.Width = 120;
            // 
            // rceCheck
            // 
//            this.rceCheck.AutoHeight = false;
//            this.rceCheck.Name = "rceCheck";
//            this.rceCheck.EditValueChanged += new System.EventHandler(this.rceCheck_EditValueChanged);
            // 
            // colBranchMerge
            // 
//            this.colBranchMerge.Caption = "Протянуть изменения";
//            this.colBranchMerge.ColumnEdit = this.rceCheck;
//            this.colBranchMerge.FieldName = "GetChanges";
//            this.colBranchMerge.Name = "colBranchMerge";
//            this.colBranchMerge.Visible = true;
//            this.colBranchMerge.VisibleIndex = 1;
//            this.colBranchMerge.Width = 120;
            // 
            // colBranchQueueBuild
            // 
//            this.colBranchQueueBuild.Caption = "Запустить билд";
//            this.colBranchQueueBuild.ColumnEdit = this.rceCheck;
//            this.colBranchQueueBuild.FieldName = "QueueBuild";
//            this.colBranchQueueBuild.Name = "colBranchQueueBuild";
//            this.colBranchQueueBuild.Visible = true;
//            this.colBranchQueueBuild.VisibleIndex = 3;
//            this.colBranchQueueBuild.Width = 120;
            // 
            // grMainFolders
            // 
//            this.grMainFolders.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
//            this.grMainFolders.Location = new System.Drawing.Point(11, 202);
//            this.grMainFolders.MainView = this.viewMainFolders;
//            this.grMainFolders.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.grMainFolders.Name = "grMainFolders";
//            this.grMainFolders.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rleRebuildScheme});
//            this.grMainFolders.Size = new System.Drawing.Size(699, 54);
//            this.grMainFolders.TabIndex = 15;
//            this.grMainFolders.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewMainFolders});
            // 
            // viewMainFolders
            // 
//            this.viewMainFolders.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colMainFolderName,
//            this.colMainGetChanges,
//            this.colMainRebuildScheme,
//            this.colMainCheckIn,
//            this.colMainQueueBuild});
//            this.viewMainFolders.GridControl = this.grMainFolders;
//            this.viewMainFolders.Name = "viewMainFolders";
//            this.viewMainFolders.OptionsBehavior.AutoPopulateColumns = false;
//            this.viewMainFolders.OptionsCustomization.AllowColumnMoving = false;
//            this.viewMainFolders.OptionsCustomization.AllowFilter = false;
//            this.viewMainFolders.OptionsCustomization.AllowGroup = false;
//            this.viewMainFolders.OptionsCustomization.AllowQuickHideColumns = false;
//            this.viewMainFolders.OptionsCustomization.AllowSort = false;
//            this.viewMainFolders.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.viewMainFolders.OptionsSelection.EnableAppearanceFocusedRow = false;
//            this.viewMainFolders.OptionsView.ColumnAutoWidth = false;
//            this.viewMainFolders.OptionsView.ShowGroupPanel = false;
//            this.viewMainFolders.OptionsView.ShowIndicator = false;
//            this.viewMainFolders.VertScrollVisibility = DevExpress.XtraGrid.Views.Base.ScrollVisibility.Never;
            // 
            // colMainFolderName
            // 
//            this.colMainFolderName.Caption = "Главная";
//            this.colMainFolderName.FieldName = "Name";
//            this.colMainFolderName.Name = "colMainFolderName";
//            this.colMainFolderName.OptionsColumn.AllowEdit = false;
//            this.colMainFolderName.Visible = true;
//            this.colMainFolderName.VisibleIndex = 0;
//            this.colMainFolderName.Width = 120;
            // 
            // colMainGetChanges
            // 
//            this.colMainGetChanges.Caption = "Забрать изменения";
//            this.colMainGetChanges.FieldName = "GetChanges";
//            this.colMainGetChanges.Name = "colMainGetChanges";
//            this.colMainGetChanges.Visible = true;
//            this.colMainGetChanges.VisibleIndex = 1;
//            this.colMainGetChanges.Width = 120;
            // 
            // colMainRebuildScheme
            // 
//            this.colMainRebuildScheme.Caption = "Перестроить схему";
//            this.colMainRebuildScheme.ColumnEdit = this.rleRebuildScheme;
//            this.colMainRebuildScheme.FieldName = "RebuildScheme";
//            this.colMainRebuildScheme.Name = "colMainRebuildScheme";
//            this.colMainRebuildScheme.Visible = true;
//            this.colMainRebuildScheme.VisibleIndex = 2;
//            this.colMainRebuildScheme.Width = 120;
            // 
            // rleRebuildScheme
            // 
//            this.rleRebuildScheme.AutoHeight = false;
//            this.rleRebuildScheme.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rleRebuildScheme.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
//            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("name", "name")});
//            this.rleRebuildScheme.Name = "rleRebuildScheme";
//            this.rleRebuildScheme.ShowFooter = false;
//            this.rleRebuildScheme.ShowHeader = false;
            // 
            // colMainCheckIn
            // 
//            this.colMainCheckIn.Caption = "Вернуть изменения";
//            this.colMainCheckIn.FieldName = "CheckIn";
//            this.colMainCheckIn.Name = "colMainCheckIn";
//            this.colMainCheckIn.Visible = true;
//            this.colMainCheckIn.VisibleIndex = 3;
//            this.colMainCheckIn.Width = 120;
            // 
            // colMainQueueBuild
            // 
//            this.colMainQueueBuild.Caption = "Запустить билд";
//            this.colMainQueueBuild.FieldName = "QueueBuild";
//            this.colMainQueueBuild.Name = "colMainQueueBuild";
//            this.colMainQueueBuild.Visible = true;
//            this.colMainQueueBuild.VisibleIndex = 4;
//            this.colMainQueueBuild.Width = 120;
            // 
            // meComment
            // 
//            this.meComment.Location = new System.Drawing.Point(11, 34);
//            this.meComment.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.meComment.Name = "meComment";
//            this.meComment.Size = new System.Drawing.Size(699, 131);
//            this.meComment.StyleController = this.layoutControl1;
//            this.meComment.TabIndex = 10;
//            this.meComment.EditValueChanged += new System.EventHandler(this.meComment_EditValueChanged);
            // 
            // grLog
            // 
//            this.grLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
//            this.grLog.Location = new System.Drawing.Point(734, 30);
//            this.grLog.MainView = this.viewLog;
//            this.grLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.grLog.Name = "grLog";
//            this.grLog.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rtiTime,
//            this.riiStatus,
//            this.rmeText});
//            this.grLog.Size = new System.Drawing.Size(410, 585);
//            this.grLog.TabIndex = 9;
//            this.grLog.UseDisabledStatePainter = false;
//            this.grLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewLog});
            // 
            // viewLog
            // 
//            this.viewLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colTime,
//            this.colStatus,
//            this.colText});
//            this.viewLog.GridControl = this.grLog;
//            this.viewLog.Name = "viewLog";
//            this.viewLog.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDownFocused;
//            this.viewLog.OptionsBehavior.ReadOnly = true;
//            this.viewLog.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.viewLog.OptionsSelection.MultiSelect = true;
//            this.viewLog.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
//            this.viewLog.OptionsView.RowAutoHeight = true;
//            this.viewLog.OptionsView.ShowGroupPanel = false;
//            this.viewLog.OptionsView.ShowIndicator = false;
//            this.viewLog.ViewCaption = " ";
            // 
            // colTime
            // 
//            this.colTime.AppearanceHeader.Options.UseTextOptions = true;
//            this.colTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colTime.Caption = "Время";
//            this.colTime.ColumnEdit = this.rtiTime;
//            this.colTime.FieldName = "time";
//            this.colTime.Name = "colTime";
//            this.colTime.OptionsColumn.FixedWidth = true;
//            this.colTime.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowOnlyInEditor;
//            this.colTime.Visible = true;
//            this.colTime.VisibleIndex = 1;
//            this.colTime.Width = 60;
            // 
            // rtiTime
            // 
//            this.rtiTime.AutoHeight = false;
//            this.rtiTime.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rtiTime.Name = "rtiTime";
            // 
            // colStatus
            // 
//            this.colStatus.AppearanceHeader.Options.UseTextOptions = true;
//            this.colStatus.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colStatus.Caption = " ";
//            this.colStatus.ColumnEdit = this.riiStatus;
//            this.colStatus.FieldName = "status";
//            this.colStatus.Name = "colStatus";
//            this.colStatus.OptionsColumn.FixedWidth = true;
//            this.colStatus.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowOnlyInEditor;
//            this.colStatus.Visible = true;
//            this.colStatus.VisibleIndex = 0;
//            this.colStatus.Width = 24;
            // 
            // riiStatus
            // 
//            this.riiStatus.AutoHeight = false;
//            this.riiStatus.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.riiStatus.GlyphAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.riiStatus.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
//            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 0, 0),
//            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 1, 1)});
//            this.riiStatus.Name = "riiStatus";
//            this.riiStatus.SmallImages = this.ic2;
            // 
            // ic2
            // 
//            this.ic2.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("ic2.ImageStream")));
//            this.ic2.Images.SetKeyName(0, "information.png");
//            this.ic2.Images.SetKeyName(1, "exclamation-red.png");
            // 
            // colText
            // 
//            this.colText.AppearanceHeader.Options.UseTextOptions = true;
//            this.colText.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colText.Caption = "Сообщение";
//            this.colText.ColumnEdit = this.rmeText;
//            this.colText.FieldName = "text";
//            this.colText.Name = "colText";
//            this.colText.ShowButtonMode = DevExpress.XtraGrid.Views.Base.ShowButtonModeEnum.ShowOnlyInEditor;
//            this.colText.Visible = true;
//            this.colText.VisibleIndex = 2;
//            this.colText.Width = 520;
            // 
            // rmeText
            // 
//            this.rmeText.Name = "rmeText";
            // 
            // layoutControlGroup1
            // 
//            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
//            this.layoutControlGroup1.GroupBordersVisible = false;
//            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
//            this.layoutControlGroup2,
//            this.layoutControlGroup3,
//            this.splitterItem1});
//            this.layoutControlGroup1.Name = "Root";
//            this.layoutControlGroup1.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
//            this.layoutControlGroup1.Size = new System.Drawing.Size(1151, 622);
//            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlGroup2
            // 
//            this.layoutControlGroup2.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
//            this.layoutControlGroup4,
//            this.layoutControlGroup5});
//            this.layoutControlGroup2.Location = new System.Drawing.Point(0, 0);
//            this.layoutControlGroup2.Name = "layoutControlGroup2";
//            this.layoutControlGroup2.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
//            this.layoutControlGroup2.Size = new System.Drawing.Size(721, 622);
//            this.layoutControlGroup2.TextVisible = false;
            // 
            // layoutControlGroup4
            // 
//            this.layoutControlGroup4.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
//            this.layoutControlItem2});
//            this.layoutControlGroup4.Location = new System.Drawing.Point(0, 0);
//            this.layoutControlGroup4.Name = "layoutControlGroup4";
//            this.layoutControlGroup4.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
//            this.layoutControlGroup4.Size = new System.Drawing.Size(713, 168);
//            this.layoutControlGroup4.Text = "Комментарий";
            // 
            // layoutControlItem2
            // 
//            this.layoutControlItem2.Control = this.meComment;
//            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
//            this.layoutControlItem2.MinSize = new System.Drawing.Size(14, 20);
//            this.layoutControlItem2.Name = "layoutControlItem2";
//            this.layoutControlItem2.Size = new System.Drawing.Size(705, 137);
//            this.layoutControlItem2.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
//            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
//            this.layoutControlItem2.TextVisible = false;
            // 
            // layoutControlGroup5
            // 
//            this.layoutControlGroup5.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
//            this.layoutControlItem6,
//            this.layoutControlItem3});
//            this.layoutControlGroup5.Location = new System.Drawing.Point(0, 168);
//            this.layoutControlGroup5.Name = "layoutControlGroup5";
//            this.layoutControlGroup5.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
//            this.layoutControlGroup5.Size = new System.Drawing.Size(713, 446);
//            this.layoutControlGroup5.Text = "Настройки";
            // 
            // layoutControlItem6
            // 
//            this.layoutControlItem6.Control = this.grMainFolders;
//            this.layoutControlItem6.Location = new System.Drawing.Point(0, 0);
//            this.layoutControlItem6.MaxSize = new System.Drawing.Size(0, 60);
//            this.layoutControlItem6.MinSize = new System.Drawing.Size(104, 60);
//            this.layoutControlItem6.Name = "layoutControlItem6";
//            this.layoutControlItem6.Size = new System.Drawing.Size(705, 60);
//            this.layoutControlItem6.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
//            this.layoutControlItem6.TextSize = new System.Drawing.Size(0, 0);
//            this.layoutControlItem6.TextVisible = false;
            // 
            // layoutControlItem3
            // 
//            this.layoutControlItem3.Control = this.grBranchFolders;
//            this.layoutControlItem3.Location = new System.Drawing.Point(0, 60);
//            this.layoutControlItem3.MinSize = new System.Drawing.Size(104, 24);
//            this.layoutControlItem3.Name = "layoutControlItem3";
//            this.layoutControlItem3.Size = new System.Drawing.Size(705, 355);
//            this.layoutControlItem3.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
//            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
//            this.layoutControlItem3.TextVisible = false;
            // 
            // layoutControlGroup3
            // 
//            this.layoutControlGroup3.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
//            this.layoutControlItem1});
//            this.layoutControlGroup3.Location = new System.Drawing.Point(727, 0);
//            this.layoutControlGroup3.Name = "layoutControlGroup3";
//            this.layoutControlGroup3.Padding = new DevExpress.XtraLayout.Utils.Padding(0, 0, 0, 0);
//            this.layoutControlGroup3.Size = new System.Drawing.Size(424, 622);
//            this.layoutControlGroup3.Text = "Лог";
            // 
            // layoutControlItem1
            // 
//            this.layoutControlItem1.Control = this.grLog;
//            this.layoutControlItem1.Location = new System.Drawing.Point(0, 0);
//            this.layoutControlItem1.Name = "layoutControlItem1";
//            this.layoutControlItem1.Size = new System.Drawing.Size(416, 591);
//            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
//            this.layoutControlItem1.TextVisible = false;
            // 
            // splitterItem1
            // 
//            this.splitterItem1.AllowHotTrack = true;
//            this.splitterItem1.Location = new System.Drawing.Point(721, 0);
//            this.splitterItem1.Name = "splitterItem1";
//            this.splitterItem1.Size = new System.Drawing.Size(6, 622);
            // 
            // barManager1
            // 
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.barFooter});
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.btnStart,
//            this.btnCancel,
//            this.blScenarios,
//            this.btnScenario1,
//            this.pbProgress,
//            this.btnScenario2,
//            this.btnScenario4,
//            this.btnScenario5,
//            this.btnScenario6,
//            this.blProjects,
//            this.btnScenario7});
//            this.barManager1.MaxItemId = 16;
//            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rpbProgress,
//            this.repositoryItemCheckedComboBoxEdit1,
//            this.repositoryItemPopupContainerEdit1,
//            this.repositoryItemCheckEdit1});
            // 
            // barFooter
            // 
//            this.barFooter.BarName = "Tools";
//            this.barFooter.DockCol = 0;
//            this.barFooter.DockRow = 0;
//            this.barFooter.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.barFooter.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnStart),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCancel, true),
//            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Caption, this.blScenarios, "Сценарии", true),
//            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.Width, this.pbProgress, "", false, true, true, 97),
//            new DevExpress.XtraBars.LinkPersistInfo(this.blProjects, true)});
//            this.barFooter.OptionsBar.AllowQuickCustomization = false;
//            this.barFooter.OptionsBar.DisableClose = true;
//            this.barFooter.OptionsBar.DisableCustomization = true;
//            this.barFooter.OptionsBar.DrawDragBorder = false;
//            this.barFooter.OptionsBar.UseWholeRow = true;
//            this.barFooter.Text = "Tools";
            // 
            // btnStart
            // 
//            this.btnStart.Caption = "Мне повезёт";
//            this.btnStart.Id = 0;
//            this.btnStart.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnStart.ImageOptions.Image")));
//            this.btnStart.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnStart.ImageOptions.LargeImage")));
//            this.btnStart.Name = "btnStart";
//            this.btnStart.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.btnStart.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnStart_ItemClick);
            // 
            // btnCancel
            // 
//            this.btnCancel.Caption = "Отмена";
//            this.btnCancel.Enabled = false;
//            this.btnCancel.Id = 1;
//            this.btnCancel.ImageOptions.Image = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.Image")));
//            this.btnCancel.ImageOptions.LargeImage = ((System.Drawing.Image)(resources.GetObject("btnCancel.ImageOptions.LargeImage")));
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // blScenarios
            // 
//            this.blScenarios.Caption = "Сценарии";
//            this.blScenarios.Id = 2;
//            this.blScenarios.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnScenario1),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnScenario2),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnScenario7),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnScenario4, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnScenario5),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnScenario6, true)});
//            this.blScenarios.Name = "blScenarios";
            // 
            // btnScenario1
            // 
//            this.btnScenario1.Caption = "Вернуть в main";
//            this.btnScenario1.Id = 3;
//            this.btnScenario1.Name = "btnScenario1";
//            this.btnScenario1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnScenario1_ItemClick);
            // 
            // btnScenario2
            // 
//            this.btnScenario2.Caption = "Вернуть в main + протянуть в кидо/инвестпро";
//            this.btnScenario2.Id = 5;
//            this.btnScenario2.Name = "btnScenario2";
//            this.btnScenario2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnScenario2_ItemClick);
            // 
            // btnScenario7
            // 
//            this.btnScenario7.Caption = "Вернуть в main + протянуть в 3.9.28.* и 3.9.29.*";
//            this.btnScenario7.Id = 15;
//            this.btnScenario7.Name = "btnScenario7";
//            this.btnScenario7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnScenario7_ItemClick);
            // 
            // btnScenario4
            // 
//            this.btnScenario4.Caption = "Протянуть в кидо/инвестпро";
//            this.btnScenario4.Id = 7;
//            this.btnScenario4.Name = "btnScenario4";
//            this.btnScenario4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnScenario4_ItemClick);
            // 
            // btnScenario5
            // 
//            this.btnScenario5.Caption = "Протянуть в 3.9.28.* и 3.9.29.*";
//            this.btnScenario5.Id = 8;
//            this.btnScenario5.Name = "btnScenario5";
//            this.btnScenario5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnScenario5_ItemClick);
            // 
            // btnScenario6
            // 
//            this.btnScenario6.Caption = "Загрузить последние изменения main";
//            this.btnScenario6.Id = 9;
//            this.btnScenario6.Name = "btnScenario6";
//            this.btnScenario6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnScenario6_ItemClick);
            // 
            // pbProgress
            // 
//            this.pbProgress.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.pbProgress.Edit = this.rpbProgress;
//            this.pbProgress.Id = 4;
//            this.pbProgress.Name = "pbProgress";
            // 
            // rpbProgress
            // 
//            this.rpbProgress.Name = "rpbProgress";
//            this.rpbProgress.Stopped = true;
            // 
            // blProjects
            // 
//            this.blProjects.Caption = "Проекты";
//            this.blProjects.Id = 12;
//            this.blProjects.Name = "blProjects";
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Manager = this.barManager1;
//            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlTop.Size = new System.Drawing.Size(1151, 39);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 661);
//            this.barDockControlBottom.Manager = this.barManager1;
//            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlBottom.Size = new System.Drawing.Size(1151, 0);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
//            this.barDockControlLeft.Manager = this.barManager1;
//            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 622);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(1151, 39);
//            this.barDockControlRight.Manager = this.barManager1;
//            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 622);
            // 
            // repositoryItemCheckedComboBoxEdit1
            // 
//            this.repositoryItemCheckedComboBoxEdit1.AutoHeight = false;
//            this.repositoryItemCheckedComboBoxEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemCheckedComboBoxEdit1.Name = "repositoryItemCheckedComboBoxEdit1";
            // 
            // repositoryItemPopupContainerEdit1
            // 
//            this.repositoryItemPopupContainerEdit1.AutoHeight = false;
//            this.repositoryItemPopupContainerEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemPopupContainerEdit1.Name = "repositoryItemPopupContainerEdit1";
            // 
            // repositoryItemCheckEdit1
            // 
//            this.repositoryItemCheckEdit1.AutoHeight = false;
//            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // tba
            // 
//            this.tba.ParentControl = this;
            // 
            // frmAutoCheckIn
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(1151, 661);
//            this.Controls.Add(this.layoutControl1);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.Name = "frmAutoCheckIn";
//            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
//            this.Text = "TFS Auto CheckIn";
//            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAutoCheckIn_FormClosing);
//            this.Load += new System.EventHandler(this.frmAutoCheckIn_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
//            this.layoutControl1.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.grBranchFolders)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewBranchFolders)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceCheck)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grMainFolders)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewMainFolders)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleRebuildScheme)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.meComment.Properties)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grLog)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewLog)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rtiTime)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.riiStatus)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeText)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup4)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup5)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem6)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup3)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.splitterItem1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rpbProgress)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemPopupContainerEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraLayout.LayoutControl layoutControl1;
//        private DevExpress.XtraEditors.MemoEdit meComment;
//        private DevExpress.XtraGrid.GridControl grLog;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewLog;
//        private DevExpress.XtraGrid.Columns.GridColumn colTime;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit rtiTime;
//        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
//        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox riiStatus;
//        private DevExpress.XtraGrid.Columns.GridColumn colText;
//        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit rmeText;
//        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
//        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
//        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup2;
//        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
//        private DevExpress.XtraLayout.SplitterItem splitterItem1;
//        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup3;
//        private DevExpress.Utils.ImageCollection ic2;
//        private DevExpress.XtraGrid.GridControl grMainFolders;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewMainFolders;
//        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem6;
//        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup4;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.Bar barFooter;
//        private DevExpress.XtraBars.BarButtonItem btnStart;
//        private DevExpress.XtraBars.BarButtonItem btnCancel;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraGrid.Columns.GridColumn colMainFolderName;
//        private DevExpress.XtraGrid.Columns.GridColumn colMainGetChanges;
//        private DevExpress.XtraGrid.Columns.GridColumn colMainRebuildScheme;
//        private DevExpress.XtraGrid.Columns.GridColumn colMainCheckIn;
//        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rleRebuildScheme;
//        private DevExpress.XtraBars.BarLinkContainerItem blScenarios;
//        private DevExpress.XtraBars.BarButtonItem btnScenario1;
//        private DevExpress.XtraBars.BarEditItem pbProgress;
//        private DevExpress.XtraEditors.Repository.RepositoryItemMarqueeProgressBar rpbProgress;
//        private DevExpress.XtraGrid.Columns.GridColumn colMainQueueBuild;
//        private DevExpress.XtraGrid.GridControl grBranchFolders;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewBranchFolders;
//        private DevExpress.XtraGrid.Columns.GridColumn colBranchName;
//        private DevExpress.XtraGrid.Columns.GridColumn colBranchCheckIn;
//        private DevExpress.XtraGrid.Columns.GridColumn colBranchMerge;
//        private DevExpress.XtraGrid.Columns.GridColumn colBranchQueueBuild;
//        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
//        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup5;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceCheck;
//        private DevExpress.XtraBars.BarButtonItem btnScenario2;
//        private DevExpress.XtraBars.BarButtonItem btnScenario4;
//        private DevExpress.XtraBars.BarButtonItem btnScenario5;
//        private DevExpress.XtraBars.BarButtonItem btnScenario6;
//        private DevExpress.XtraBars.BarLinkContainerItem blProjects;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit1;
//        private DevExpress.XtraEditors.Repository.RepositoryItemPopupContainerEdit repositoryItemPopupContainerEdit1;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
//        private DevExpress.Utils.Taskbar.TaskbarAssistant tba;
//        private DevExpress.XtraBars.BarButtonItem btnScenario7;
//    }
//}
