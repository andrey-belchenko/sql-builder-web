//namespace sql.builder.PackagePrint
//{
//    internal partial class ucPackagePrint
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

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucPackagePrint));
//            this.tlReports = new DevExpress.XtraTreeList.TreeList();
//            this.tcolItem = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.rmeItem = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
//            this.rreItem = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
//            this.ic = new DevExpress.Utils.ImageCollection();
//            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
//            this.gcParams = new DevExpress.XtraEditors.GroupControl();
//            this.splitterControl2 = new DevExpress.XtraEditors.SplitterControl();
//            this.gcLog = new DevExpress.XtraEditors.GroupControl();
//            this.grLog = new DevExpress.XtraGrid.GridControl();
//            this.viewLog = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rtiTime = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
//            this.colTitle = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.riiStatus = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
//            this.colText = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rmeText = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
//            ((System.ComponentModel.ISupportInitialize)(this.tlReports)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeItem)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rreItem)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gcParams)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gcLog)).BeginInit();
//            this.gcLog.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.grLog)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewLog)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rtiTime)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.riiStatus)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeText)).BeginInit();
//            this.SuspendLayout();
            // 
            // tlReports
            // 
//            this.tlReports.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
//            this.tcolItem});
//            this.tlReports.Cursor = System.Windows.Forms.Cursors.Default;
//            this.tlReports.Dock = System.Windows.Forms.DockStyle.Left;
//            this.tlReports.ImageIndexFieldName = "image_id";
//            this.tlReports.KeyFieldName = "name";
//            this.tlReports.Location = new System.Drawing.Point(0, 0);
//            this.tlReports.Name = "tlReports";
//            this.tlReports.OptionsBehavior.EnableFiltering = true;
//            this.tlReports.OptionsCustomization.AllowQuickHideColumns = false;
//            this.tlReports.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Extended;
//            this.tlReports.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.tlReports.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None;
//            this.tlReports.OptionsView.ShowAutoFilterRow = true;
//            this.tlReports.OptionsView.ShowColumns = false;
//            this.tlReports.OptionsView.ShowPreview = true;
//            this.tlReports.ParentFieldName = "Children";
//            this.tlReports.PreviewLineCount = 2;
//            this.tlReports.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rmeItem,
//            this.rreItem});
//            this.tlReports.SelectImageList = this.ic;
//            this.tlReports.Size = new System.Drawing.Size(335, 585);
//            this.tlReports.TabIndex = 1;
            // 
            // tcolItem
            // 
//            this.tcolItem.Caption = "Отчёт";
//            this.tcolItem.ColumnEdit = this.rmeItem;
//            this.tcolItem.FieldName = "Name";
//            this.tcolItem.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
//            this.tcolItem.MinWidth = 34;
//            this.tcolItem.Name = "tcolItem";
//            this.tcolItem.OptionsColumn.AllowEdit = false;
//            this.tcolItem.Visible = true;
//            this.tcolItem.VisibleIndex = 0;
            // 
            // rmeItem
            // 
//            this.rmeItem.Name = "rmeItem";
            // 
            // rreItem
            // 
//            this.rreItem.AllowHtmlDraw = DevExpress.Utils.DefaultBoolean.True;
//            this.rreItem.DocumentFormat = DevExpress.XtraRichEdit.DocumentFormat.Html;
//            this.rreItem.EncodingWebName = "utf-8";
//            this.rreItem.Name = "rreItem";
//            this.rreItem.ShowCaretInReadOnly = false;
            // 
            // ic
            // 
//            this.ic.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("ic.ImageStream")));
//            this.ic.InsertGalleryImage("open_16x16.png", "images/actions/open_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/open_16x16.png"), 0);
//            this.ic.Images.SetKeyName(0, "open_16x16.png");
//            this.ic.InsertGalleryImage("report_16x16.png", "images/reports/report_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/reports/report_16x16.png"), 1);
//            this.ic.Images.SetKeyName(1, "report_16x16.png");
//            this.ic.InsertGalleryImage("edittask_16x16.png", "images/tasks/edittask_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/tasks/edittask_16x16.png"), 2);
//            this.ic.Images.SetKeyName(2, "edittask_16x16.png");
//            this.ic.InsertGalleryImage("template_16x16.png", "images/support/template_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/support/template_16x16.png"), 3);
//            this.ic.Images.SetKeyName(3, "template_16x16.png");
//            this.ic.InsertGalleryImage("hide_16x16.png", "images/actions/hide_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/hide_16x16.png"), 4);
//            this.ic.Images.SetKeyName(4, "hide_16x16.png");
//            this.ic.InsertGalleryImage("morefunctions_16x16.png", "images/function%20library/morefunctions_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/function%20library/morefunctions_16x16.png"), 5);
//            this.ic.Images.SetKeyName(5, "morefunctions_16x16.png");
            // 
            // splitterControl1
            // 
//            this.splitterControl1.Location = new System.Drawing.Point(335, 0);
//            this.splitterControl1.Name = "splitterControl1";
//            this.splitterControl1.Size = new System.Drawing.Size(5, 585);
//            this.splitterControl1.TabIndex = 2;
//            this.splitterControl1.TabStop = false;
            // 
            // gcParams
            // 
//            this.gcParams.AppearanceCaption.Options.UseTextOptions = true;
//            this.gcParams.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.gcParams.Dock = System.Windows.Forms.DockStyle.Left;
//            this.gcParams.Location = new System.Drawing.Point(340, 0);
//            this.gcParams.Name = "gcParams";
//            this.gcParams.Size = new System.Drawing.Size(682, 585);
//            this.gcParams.TabIndex = 12;
//            this.gcParams.Text = "Параметры отчёта";
            // 
            // splitterControl2
            // 
//            this.splitterControl2.Location = new System.Drawing.Point(1022, 0);
//            this.splitterControl2.Name = "splitterControl2";
//            this.splitterControl2.Size = new System.Drawing.Size(5, 585);
//            this.splitterControl2.TabIndex = 13;
//            this.splitterControl2.TabStop = false;
            // 
            // gcLog
            // 
//            this.gcLog.Appearance.Options.UseTextOptions = true;
//            this.gcLog.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.gcLog.AppearanceCaption.Options.UseTextOptions = true;
//            this.gcLog.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.gcLog.Controls.Add(this.grLog);
//            this.gcLog.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.gcLog.Location = new System.Drawing.Point(1027, 0);
//            this.gcLog.Name = "gcLog";
//            this.gcLog.Size = new System.Drawing.Size(0, 585);
//            this.gcLog.TabIndex = 14;
//            this.gcLog.Text = " Лог";
            // 
            // grLog
            // 
//            this.grLog.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.grLog.Location = new System.Drawing.Point(1, 20);
//            this.grLog.MainView = this.viewLog;
//            this.grLog.Name = "grLog";
//            this.grLog.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rtiTime,
//            this.riiStatus,
//            this.rmeText});
//            this.grLog.Size = new System.Drawing.Size(0, 563);
//            this.grLog.TabIndex = 7;
//            this.grLog.UseDisabledStatePainter = false;
//            this.grLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewLog});
            // 
            // viewLog
            // 
//            this.viewLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colTime,
//            this.colTitle,
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
            // colTitle
            // 
//            this.colTitle.AppearanceHeader.Options.UseTextOptions = true;
//            this.colTitle.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colTitle.Caption = "Отчёт";
//            this.colTitle.FieldName = "Name";
//            this.colTitle.MinWidth = 100;
//            this.colTitle.Name = "colTitle";
//            this.colTitle.Visible = true;
//            this.colTitle.VisibleIndex = 2;
//            this.colTitle.Width = 100;
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
//            this.colText.VisibleIndex = 3;
//            this.colText.Width = 520;
            // 
            // rmeText
            // 
//            this.rmeText.Name = "rmeText";
            // 
            // ucPackagePrint
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.gcLog);
//            this.Controls.Add(this.splitterControl2);
//            this.Controls.Add(this.gcParams);
//            this.Controls.Add(this.splitterControl1);
//            this.Controls.Add(this.tlReports);
//            this.Name = "ucPackagePrint";
//            this.Size = new System.Drawing.Size(986, 585);
//            ((System.ComponentModel.ISupportInitialize)(this.tlReports)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeItem)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rreItem)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gcParams)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gcLog)).EndInit();
//            this.gcLog.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.grLog)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewLog)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rtiTime)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.riiStatus)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeText)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraTreeList.TreeList tlReports;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn tcolItem;
//        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit rmeItem;
//        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit rreItem;
//        private DevExpress.XtraEditors.SplitterControl splitterControl1;
//        private DevExpress.XtraEditors.GroupControl gcParams;
//        private DevExpress.XtraEditors.SplitterControl splitterControl2;
//        private DevExpress.XtraEditors.GroupControl gcLog;
//        private DevExpress.XtraGrid.GridControl grLog;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewLog;
//        private DevExpress.XtraGrid.Columns.GridColumn colTime;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit rtiTime;
//        private DevExpress.XtraGrid.Columns.GridColumn colTitle;
//        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
//        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox riiStatus;
//        private DevExpress.XtraGrid.Columns.GridColumn colText;
//        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit rmeText;
//        private DevExpress.Utils.ImageCollection ic;
//    }
//}
