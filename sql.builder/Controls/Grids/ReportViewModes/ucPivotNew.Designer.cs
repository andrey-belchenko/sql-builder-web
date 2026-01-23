//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucPivotNew
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
//            this.pivot = new DevExpress.XtraPivotGrid.PivotGridControl();
//            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
//            this.barToolbar = new DevExpress.XtraBars.Bar();
//            this.blExport = new DevExpress.XtraBars.BarLinkContainerItem();
//            this.ButtonExportExcel = new DevExpress.XtraBars.BarButtonItem();
//            this.barFooter = new DevExpress.XtraBars.Bar();
//            this.teTotalSum = new DevExpress.XtraBars.BarEditItem();
//            this.rteTotalSum = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.teTotalCount = new DevExpress.XtraBars.BarEditItem();
//            this.rteTotalCount = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            ((System.ComponentModel.ISupportInitialize)(this.pivot)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).BeginInit();
//            this.SuspendLayout();
            // 
            // pivot
            // 
//            this.pivot.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.pivot.Location = new System.Drawing.Point(0, 39);
//            this.pivot.Name = "pivot";
//            this.pivot.Size = new System.Drawing.Size(779, 364);
//            this.pivot.TabIndex = 6;
//            this.pivot.CellSelectionChanged += new System.EventHandler(this.pivot_CellSelectionChanged);
            // 
            // barManager
            // 
//            this.barManager.AllowCustomization = false;
//            this.barManager.AllowMoveBarOnToolbar = false;
//            this.barManager.AllowQuickCustomization = false;
//            this.barManager.AllowShowToolbarsPopup = false;
//            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.barToolbar,
//            this.barFooter});
//            this.barManager.DockControls.Add(this.barDockControlTop);
//            this.barManager.DockControls.Add(this.barDockControlBottom);
//            this.barManager.DockControls.Add(this.barDockControlLeft);
//            this.barManager.DockControls.Add(this.barDockControlRight);
//            this.barManager.Form = this;
//            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.teTotalSum,
//            this.teTotalCount,
//            this.blExport,
//            this.ButtonExportExcel});
//            this.barManager.MaxItemId = 1;
//            this.barManager.StatusBar = this.barFooter;
            // 
            // barTopToolbar
            // 
//            this.barToolbar.BarName = "Tools";
//            this.barToolbar.DockCol = 0;
//            this.barToolbar.DockRow = 0;
//            this.barToolbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.barToolbar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.blExport, true)});
//            this.barToolbar.OptionsBar.AllowQuickCustomization = false;
//            this.barToolbar.OptionsBar.DrawBorder = false;
//            this.barToolbar.OptionsBar.DrawDragBorder = false;
//            this.barToolbar.OptionsBar.MultiLine = true;
//            this.barToolbar.OptionsBar.UseWholeRow = true;
//            this.barToolbar.Text = "Tools";
            // 
            // blExport
            // 
//            this.blExport.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.blExport.Glyph = global::sql.builder.Properties.Resources.GridExportToFile_24;
//            this.blExport.Id = 6;
//            this.blExport.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonExportExcel)});
//            this.blExport.Name = "blExport";
//            this.blExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.blExport.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.False;
            // 
            // ButtonExportExcel
            // 
//            this.ButtonExportExcel.Caption = "Экспорт в Excel";
//            this.ButtonExportExcel.Glyph = global::sql.builder.Properties.Resources.ExportXls_161;
//            this.ButtonExportExcel.Id = 7;
//            this.ButtonExportExcel.Name = "ButtonExportExcel";
//            this.ButtonExportExcel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonExportExcel_ItemClick);
            // 
            // barFooter
            // 
//            this.barFooter.BarName = "Status bar";
//            this.barFooter.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
//            this.barFooter.DockCol = 0;
//            this.barFooter.DockRow = 0;
//            this.barFooter.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.barFooter.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.teTotalSum),
//            new DevExpress.XtraBars.LinkPersistInfo(this.teTotalCount)});
//            this.barFooter.OptionsBar.AllowQuickCustomization = false;
//            this.barFooter.OptionsBar.DrawBorder = false;
//            this.barFooter.OptionsBar.DrawDragBorder = false;
//            this.barFooter.OptionsBar.UseWholeRow = true;
//            this.barFooter.Text = "Status bar";
            // 
            // teTotalSum
            // 
//            this.teTotalSum.Border = DevExpress.XtraEditors.Controls.BorderStyles.Default;
//            this.teTotalSum.Caption = "Сумма";
//            this.teTotalSum.Edit = this.rteTotalSum;
//            this.teTotalSum.EditValue = "0";
//            this.teTotalSum.EditWidth = 140;
//            this.teTotalSum.Id = 4;
//            this.teTotalSum.Name = "teTotalSum";
//            this.teTotalSum.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // rteTotalSum
            // 
//            this.rteTotalSum.AutoHeight = false;
//            this.rteTotalSum.Name = "rteTotalSum";
//            this.rteTotalSum.ReadOnly = true;
            // 
            // teTotalCount
            // 
//            this.teTotalCount.Border = DevExpress.XtraEditors.Controls.BorderStyles.Default;
//            this.teTotalCount.Caption = "Количество";
//            this.teTotalCount.Edit = this.rteTotalCount;
//            this.teTotalCount.EditValue = "0";
//            this.teTotalCount.EditWidth = 140;
//            this.teTotalCount.Id = 6;
//            this.teTotalCount.Name = "teTotalCount";
//            this.teTotalCount.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // rteTotalCount
            // 
//            this.rteTotalCount.AutoHeight = false;
//            this.rteTotalCount.Name = "rteTotalCount";
//            this.rteTotalCount.ReadOnly = true;
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(779, 39);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 403);
//            this.barDockControlBottom.Size = new System.Drawing.Size(779, 25);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 364);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(779, 39);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 364);
            // 
            // ucPivotNew
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.pivot);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Name = "ucPivotNew";
//            this.Size = new System.Drawing.Size(779, 428);
//            ((System.ComponentModel.ISupportInitialize)(this.pivot)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraPivotGrid.PivotGridControl pivot;
//        private DevExpress.XtraBars.BarManager barManager;
//        private DevExpress.XtraBars.Bar barToolbar;
//        private DevExpress.XtraBars.Bar barFooter;
//        private DevExpress.XtraBars.BarEditItem teTotalSum;
//        private DevExpress.XtraBars.BarEditItem teTotalCount;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteTotalSum;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteTotalCount;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraBars.BarLinkContainerItem blExport;
//        private DevExpress.XtraBars.BarButtonItem ButtonExportExcel;
//    }
//}
