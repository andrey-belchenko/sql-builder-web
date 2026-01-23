//namespace sql.builder.WinForms
//{
//    internal partial class frmTestReports
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTestReports));
//            this.pbProgress = new DevExpress.XtraEditors.ProgressBarControl();
//            this.btnDebug = new DevExpress.XtraEditors.SimpleButton();
//            this.lDebug = new DevExpress.XtraEditors.LabelControl();
//            this.lTime = new DevExpress.XtraEditors.LabelControl();
//            this.timer = new System.Windows.Forms.Timer(this.components);
//            this.grLog = new DevExpress.XtraGrid.GridControl();
//            this.viewLog = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rtiTime = new DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit();
//            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.riiStatus = new DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox();
//            this.imageCollection = new DevExpress.Utils.ImageCollection(this.components);
//            this.colText = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rmeText = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
//            this.colReport = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.pHeader = new DevExpress.XtraEditors.PanelControl();
//            ((System.ComponentModel.ISupportInitialize)(this.pbProgress.Properties)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grLog)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewLog)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rtiTime)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.riiStatus)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.imageCollection)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeText)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pHeader)).BeginInit();
//            this.pHeader.SuspendLayout();
//            this.SuspendLayout();
            // 
            // pbProgress
            // 
//            this.pbProgress.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.pbProgress.Location = new System.Drawing.Point(117, 14);
//            this.pbProgress.Name = "pbProgress";
//            this.pbProgress.Properties.ShowTitle = true;
//            this.pbProgress.Size = new System.Drawing.Size(677, 43);
//            this.pbProgress.TabIndex = 1;
            // 
            // btnDebug
            // 
//            this.btnDebug.Location = new System.Drawing.Point(12, 14);
//            this.btnDebug.Name = "btnDebug";
//            this.btnDebug.Size = new System.Drawing.Size(87, 43);
//            this.btnDebug.TabIndex = 2;
//            this.btnDebug.Text = "Начать";
//            this.btnDebug.Click += new System.EventHandler(this.btnBeginTest_Click);
            // 
            // lDebug
            // 
//            this.lDebug.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
//            this.lDebug.Appearance.ForeColor = System.Drawing.Color.DarkSlateBlue;
//            this.lDebug.Appearance.Options.UseFont = true;
//            this.lDebug.Appearance.Options.UseForeColor = true;
//            this.lDebug.Appearance.Options.UseTextOptions = true;
//            this.lDebug.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
//            this.lDebug.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.lDebug.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.lDebug.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
//            this.lDebug.Location = new System.Drawing.Point(12, 63);
//            this.lDebug.Name = "lDebug";
//            this.lDebug.Size = new System.Drawing.Size(604, 25);
//            this.lDebug.TabIndex = 4;
//            this.lDebug.Text = "Текущий отчёт";
            // 
            // lTime
            // 
//            this.lTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.lTime.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
//            this.lTime.Appearance.ForeColor = System.Drawing.Color.DarkSlateBlue;
//            this.lTime.Appearance.Options.UseFont = true;
//            this.lTime.Appearance.Options.UseForeColor = true;
//            this.lTime.Appearance.Options.UseTextOptions = true;
//            this.lTime.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Far;
//            this.lTime.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.lTime.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.lTime.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
//            this.lTime.Location = new System.Drawing.Point(508, 63);
//            this.lTime.Name = "lTime";
//            this.lTime.Size = new System.Drawing.Size(286, 25);
//            this.lTime.TabIndex = 5;
//            this.lTime.Text = "Затраченное время 00:00:00";
            // 
            // timer
            // 
//            this.timer.Interval = 1000;
//            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            // 
            // grLog
            // 
//            this.grLog.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.grLog.Location = new System.Drawing.Point(0, 100);
//            this.grLog.MainView = this.viewLog;
//            this.grLog.Name = "grLog";
//            this.grLog.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rtiTime,
//            this.riiStatus,
//            this.rmeText});
//            this.grLog.Size = new System.Drawing.Size(806, 347);
//            this.grLog.TabIndex = 6;
//            this.grLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewLog});
            // 
            // viewLog
            // 
//            this.viewLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colTime,
//            this.colStatus,
//            this.colText,
//            this.colReport});
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
//            this.viewLog.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
//            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colTime, DevExpress.Data.ColumnSortOrder.Descending)});
//            this.viewLog.ShowFilterPopupListBox += new DevExpress.XtraGrid.Views.Grid.FilterPopupListBoxEventHandler(this.viewLog_ShowFilterPopupListBox);
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
//            this.riiStatus.HtmlImages = this.imageCollection;
//            this.riiStatus.Items.AddRange(new DevExpress.XtraEditors.Controls.ImageComboBoxItem[] {
//            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 0, 0),
//            new DevExpress.XtraEditors.Controls.ImageComboBoxItem("", 1, 1)});
//            this.riiStatus.LargeImages = this.imageCollection;
//            this.riiStatus.Name = "riiStatus";
//            this.riiStatus.SmallImages = this.imageCollection;
            // 
            // imageCollection
            // 
//            this.imageCollection.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection.ImageStream")));
//            this.imageCollection.Images.SetKeyName(0, "information.png");
//            this.imageCollection.Images.SetKeyName(1, "exclamation-red.png");
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
            // colReport
            // 
//            this.colReport.AppearanceHeader.Options.UseTextOptions = true;
//            this.colReport.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colReport.Caption = "Отчёт";
//            this.colReport.FieldName = "report";
//            this.colReport.Name = "colReport";
//            this.colReport.Visible = true;
//            this.colReport.VisibleIndex = 2;
//            this.colReport.Width = 200;
            // 
            // pHeader
            // 
//            this.pHeader.Controls.Add(this.btnDebug);
//            this.pHeader.Controls.Add(this.pbProgress);
//            this.pHeader.Controls.Add(this.lTime);
//            this.pHeader.Controls.Add(this.lDebug);
//            this.pHeader.Dock = System.Windows.Forms.DockStyle.Top;
//            this.pHeader.Location = new System.Drawing.Point(0, 0);
//            this.pHeader.Name = "pHeader";
//            this.pHeader.Size = new System.Drawing.Size(806, 100);
//            this.pHeader.TabIndex = 7;
            // 
            // frmTestReports
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(806, 447);
//            this.Controls.Add(this.grLog);
//            this.Controls.Add(this.pHeader);
//            this.DoubleBuffered = true;
//            this.Name = "frmTestReports";
//            this.Text = "Проверка отчётов";
//            this.UserSettings.SaveFormSize = true;
//            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmTestReports_FormClosed);
//            ((System.ComponentModel.ISupportInitialize)(this.pbProgress.Properties)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grLog)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewLog)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rtiTime)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.riiStatus)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.imageCollection)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeText)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pHeader)).EndInit();
//            this.pHeader.ResumeLayout(false);
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraEditors.ProgressBarControl pbProgress;
//        private DevExpress.XtraEditors.SimpleButton btnDebug;
//        private DevExpress.XtraEditors.LabelControl lDebug;
//        private DevExpress.XtraEditors.LabelControl lTime;
//        private System.Windows.Forms.Timer timer;
//        private DevExpress.XtraGrid.GridControl grLog;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewLog;
//        private DevExpress.XtraGrid.Columns.GridColumn colTime;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTimeEdit rtiTime;
//        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
//        private DevExpress.XtraEditors.Repository.RepositoryItemImageComboBox riiStatus;
//        private DevExpress.XtraGrid.Columns.GridColumn colText;
//        private DevExpress.Utils.ImageCollection imageCollection;
//        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit rmeText;
//        private DevExpress.XtraEditors.PanelControl pHeader;
//        private DevExpress.XtraGrid.Columns.GridColumn colReport;
//    }
//}
