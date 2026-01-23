//namespace sql.builder.WinForms
//{
//    internal partial class frmCompileFormsCache
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCompileFormsCache));
//            this.pbProgress = new DevExpress.XtraEditors.ProgressBarControl();
//            this.btnDebug = new DevExpress.XtraEditors.SimpleButton();
//            this.lDebug = new DevExpress.XtraEditors.LabelControl();
//            this.lTime = new DevExpress.XtraEditors.LabelControl();
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
//            this.pbProgress.Location = new System.Drawing.Point(136, 17);
//            this.pbProgress.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.pbProgress.Name = "pbProgress";
//            this.pbProgress.Properties.ShowTitle = true;
//            this.pbProgress.Size = new System.Drawing.Size(790, 53);
//            this.pbProgress.TabIndex = 1;
            // 
            // btnDebug
            // 
//            this.btnDebug.Location = new System.Drawing.Point(14, 17);
//            this.btnDebug.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.btnDebug.Name = "btnDebug";
//            this.btnDebug.Size = new System.Drawing.Size(101, 53);
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
//            this.lDebug.Location = new System.Drawing.Point(14, 78);
//            this.lDebug.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.lDebug.Name = "lDebug";
//            this.lDebug.Size = new System.Drawing.Size(705, 31);
//            this.lDebug.TabIndex = 4;
//            this.lDebug.Text = "Текущая форма";
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
//            this.lTime.Location = new System.Drawing.Point(593, 78);
//            this.lTime.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.lTime.Name = "lTime";
//            this.lTime.Size = new System.Drawing.Size(334, 31);
//            this.lTime.TabIndex = 5;
//            this.lTime.Text = "Затраченное время 00:00:00";
            // 
            // grLog
            // 
//            this.grLog.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.grLog.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.grLog.Location = new System.Drawing.Point(0, 123);
//            this.grLog.MainView = this.viewLog;
//            this.grLog.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.grLog.Name = "grLog";
//            this.grLog.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rtiTime,
//            this.riiStatus,
//            this.rmeText});
//            this.grLog.Size = new System.Drawing.Size(940, 427);
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
//            this.colReport.Caption = "Форма";
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
//            this.pHeader.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.pHeader.Name = "pHeader";
//            this.pHeader.Size = new System.Drawing.Size(940, 123);
//            this.pHeader.TabIndex = 7;
            // 
            // frmCompileFormsCache
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(940, 550);
//            this.Controls.Add(this.grLog);
//            this.Controls.Add(this.pHeader);
//            this.DoubleBuffered = true;
//            this.Margin = new System.Windows.Forms.Padding(3, 5, 3, 5);
//            this.Name = "frmCompileFormsCache";
//            this.Text = "Компиляция форм";
//            this.UserSettings.SaveFormSize = true;
//            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmCompileFormsCache_FormClosed);
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
