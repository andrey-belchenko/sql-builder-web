//namespace sql.builder.Controls.Testing
//{
//    partial class ucReportsAndParams
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
//            this.components = new System.ComponentModel.Container();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucReportsAndParams));
//            this.gcParams = new DevExpress.XtraEditors.GroupControl();
//            this.tlReports = new DevExpress.XtraTreeList.TreeList();
//            this.band1 = new DevExpress.XtraTreeList.Columns.TreeListBand();
//            this.colReport = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.rmeReport = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
//            this.bandTime = new DevExpress.XtraTreeList.Columns.TreeListBand();
//            this.colTimeNew = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.colTimeOld = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.bandResult = new DevExpress.XtraTreeList.Columns.TreeListBand();
//            this.colResultNew = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.colResultOld = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.rreItem = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
//            this.ic = new DevExpress.Utils.ImageCollection(this.components);
//            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
//            ((System.ComponentModel.ISupportInitialize)(this.gcParams)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.tlReports)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeReport)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rreItem)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic)).BeginInit();
//            this.SuspendLayout();
            // 
            // gcParams
            // 
//            this.gcParams.AppearanceCaption.Options.UseTextOptions = true;
//            this.gcParams.AppearanceCaption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.gcParams.Dock = System.Windows.Forms.DockStyle.Right;
//            this.gcParams.Location = new System.Drawing.Point(403, 0);
//            this.gcParams.Name = "gcParams";
//            this.gcParams.Size = new System.Drawing.Size(400, 447);
//            this.gcParams.TabIndex = 11;
//            this.gcParams.Text = "Параметры отчёта";
            // 
            // tlReports
            // 
//            this.tlReports.Bands.AddRange(new DevExpress.XtraTreeList.Columns.TreeListBand[] {
//            this.band1,
//            this.bandTime,
//            this.bandResult});
//            this.tlReports.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
//            this.colReport,
//            this.colTimeNew,
//            this.colTimeOld,
//            this.colResultNew,
//            this.colResultOld});
//            this.tlReports.Cursor = System.Windows.Forms.Cursors.Default;
//            this.tlReports.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tlReports.ImageIndexFieldName = "image_id";
//            this.tlReports.KeyFieldName = "name";
//            this.tlReports.Location = new System.Drawing.Point(0, 0);
//            this.tlReports.Name = "tlReports";
//            this.tlReports.OptionsBehavior.AllowRecursiveNodeChecking = true;
//            this.tlReports.OptionsBehavior.EnableFiltering = true;
//            this.tlReports.OptionsCustomization.AllowQuickHideColumns = false;
//            this.tlReports.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Extended;
//            this.tlReports.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.tlReports.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None;
//            this.tlReports.OptionsView.ShowAutoFilterRow = true;
//            this.tlReports.OptionsView.ShowCheckBoxes = true;
//            this.tlReports.OptionsView.ShowPreview = true;
//            this.tlReports.ParentFieldName = "parent";
//            this.tlReports.PreviewLineCount = 2;
//            this.tlReports.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rmeReport,
//            this.rreItem});
//            this.tlReports.SelectImageList = this.ic;
//            this.tlReports.Size = new System.Drawing.Size(398, 447);
//            this.tlReports.TabIndex = 12;
//            this.tlReports.UseDisabledStatePainter = false;
            // 
            // band1
            // 
//            this.band1.Columns.Add(this.colReport);
//            this.band1.MinWidth = 52;
//            this.band1.Name = "band1";
            // 
            // colReport
            // 
//            this.colReport.AppearanceHeader.Options.UseTextOptions = true;
//            this.colReport.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colReport.Caption = "Отчёт";
//            this.colReport.ColumnEdit = this.rmeReport;
//            this.colReport.FieldName = "title";
//            this.colReport.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
//            this.colReport.MinWidth = 52;
//            this.colReport.Name = "colReport";
//            this.colReport.OptionsColumn.AllowEdit = false;
//            this.colReport.Visible = true;
//            this.colReport.VisibleIndex = 0;
//            this.colReport.Width = 288;
            // 
            // rmeReport
            // 
//            this.rmeReport.Name = "rmeReport";
            // 
            // bandTime
            // 
//            this.bandTime.AppearanceHeader.Options.UseTextOptions = true;
//            this.bandTime.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.bandTime.Caption = "Время";
//            this.bandTime.Columns.Add(this.colTimeNew);
//            this.bandTime.Columns.Add(this.colTimeOld);
//            this.bandTime.MinWidth = 160;
//            this.bandTime.Name = "bandTime";
//            this.bandTime.OptionsBand.FixedWidth = true;
//            this.bandTime.Width = 160;
            // 
            // colTimeNew
            // 
//            this.colTimeNew.AppearanceHeader.Options.UseTextOptions = true;
//            this.colTimeNew.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colTimeNew.Caption = "Новый";
//            this.colTimeNew.FieldName = "time_new";
//            this.colTimeNew.Name = "colTimeNew";
//            this.colTimeNew.OptionsColumn.AllowEdit = false;
//            this.colTimeNew.OptionsColumn.FixedWidth = true;
//            this.colTimeNew.Visible = true;
//            this.colTimeNew.VisibleIndex = 1;
//            this.colTimeNew.Width = 80;
            // 
            // colTimeOld
            // 
//            this.colTimeOld.AppearanceHeader.Options.UseTextOptions = true;
//            this.colTimeOld.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colTimeOld.Caption = "Старый";
//            this.colTimeOld.FieldName = "time_old";
//            this.colTimeOld.Name = "colTimeOld";
//            this.colTimeOld.OptionsColumn.AllowEdit = false;
//            this.colTimeOld.OptionsColumn.FixedWidth = true;
//            this.colTimeOld.Visible = true;
//            this.colTimeOld.VisibleIndex = 2;
//            this.colTimeOld.Width = 80;
            // 
            // bandResult
            // 
//            this.bandResult.AppearanceHeader.Options.UseTextOptions = true;
//            this.bandResult.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.bandResult.Caption = "Результат";
//            this.bandResult.Columns.Add(this.colResultNew);
//            this.bandResult.Columns.Add(this.colResultOld);
//            this.bandResult.MinWidth = 160;
//            this.bandResult.Name = "bandResult";
//            this.bandResult.OptionsBand.FixedWidth = true;
//            this.bandResult.Width = 160;
            // 
            // colResultNew
            // 
//            this.colResultNew.AppearanceHeader.Options.UseTextOptions = true;
//            this.colResultNew.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colResultNew.Caption = "Новый";
//            this.colResultNew.FieldName = "result_new";
//            this.colResultNew.Name = "colResultNew";
//            this.colResultNew.OptionsColumn.AllowEdit = false;
//            this.colResultNew.OptionsColumn.FixedWidth = true;
//            this.colResultNew.Visible = true;
//            this.colResultNew.VisibleIndex = 3;
//            this.colResultNew.Width = 80;
            // 
            // colResultOld
            // 
//            this.colResultOld.AppearanceHeader.Options.UseTextOptions = true;
//            this.colResultOld.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colResultOld.Caption = "Старый";
//            this.colResultOld.FieldName = "result_old";
//            this.colResultOld.Name = "colResultOld";
//            this.colResultOld.OptionsColumn.AllowEdit = false;
//            this.colResultOld.OptionsColumn.FixedWidth = true;
//            this.colResultOld.Visible = true;
//            this.colResultOld.VisibleIndex = 4;
//            this.colResultOld.Width = 80;
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
            // 
            // splitterControl1
            // 
//            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Right;
//            this.splitterControl1.Location = new System.Drawing.Point(398, 0);
//            this.splitterControl1.Name = "splitterControl1";
//            this.splitterControl1.Size = new System.Drawing.Size(5, 447);
//            this.splitterControl1.TabIndex = 13;
//            this.splitterControl1.TabStop = false;
            // 
            // ucReportsWithParams
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.tlReports);
//            this.Controls.Add(this.splitterControl1);
//            this.Controls.Add(this.gcParams);
//            this.Name = "ucReportsWithParams";
//            this.Size = new System.Drawing.Size(803, 447);
//            ((System.ComponentModel.ISupportInitialize)(this.gcParams)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.tlReports)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeReport)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rreItem)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraEditors.GroupControl gcParams;
//        private DevExpress.XtraTreeList.TreeList tlReports;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colReport;
//        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit rmeReport;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colTimeNew;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colTimeOld;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colResultNew;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colResultOld;
//        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit rreItem;
//        private DevExpress.XtraEditors.SplitterControl splitterControl1;
//        private DevExpress.XtraTreeList.Columns.TreeListBand band1;
//        private DevExpress.XtraTreeList.Columns.TreeListBand bandTime;
//        private DevExpress.XtraTreeList.Columns.TreeListBand bandResult;
//        private DevExpress.Utils.ImageCollection ic;
//    }
//}
