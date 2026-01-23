//namespace sql.builder.Controls.Grids.ReportViewModes.Dashboard
//{
//    partial class ucDashboardViewer
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucDashboardViewer));
//            this.dashboardViewer1 = new DevExpress.DashboardWin.DashboardViewer(this.components);
//            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
//            this.barCustom = new DevExpress.XtraBars.Bar();
//            this.btnReload = new DevExpress.XtraBars.BarButtonItem();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            ((System.ComponentModel.ISupportInitialize)(this.dashboardViewer1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            this.SuspendLayout();
            // 
            // dashboardViewer1
            // 
//            this.dashboardViewer1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.dashboardViewer1.Location = new System.Drawing.Point(0, 31);
//            this.dashboardViewer1.Name = "dashboardViewer1";
//            this.dashboardViewer1.PrintingOptions.FontInfo.GdiCharSet = ((byte)(0));
//            this.dashboardViewer1.PrintingOptions.FontInfo.Name = null;
//            this.dashboardViewer1.Size = new System.Drawing.Size(775, 442);
//            this.dashboardViewer1.TabIndex = 0;
//            this.dashboardViewer1.DashboardItemClick += new DevExpress.DashboardWin.DashboardItemMouseActionEventHandler(this.dashboardViewer1_DashboardItemClick);
            // 
            // barManager1
            // 
//            this.barManager1.AllowCustomization = false;
//            this.barManager1.AllowMoveBarOnToolbar = false;
//            this.barManager1.AllowQuickCustomization = false;
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.barCustom});
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.btnReload});
//            this.barManager1.MaxItemId = 1;
            // 
            // barCustom
            // 
//            this.barCustom.BarName = "Главное меню";
//            this.barCustom.DockCol = 0;
//            this.barCustom.DockRow = 0;
//            this.barCustom.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.barCustom.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnReload)});
//            this.barCustom.OptionsBar.AllowQuickCustomization = false;
//            this.barCustom.OptionsBar.DisableClose = true;
//            this.barCustom.OptionsBar.DisableCustomization = true;
//            this.barCustom.OptionsBar.DrawDragBorder = false;
//            this.barCustom.Text = "Главное меню";
            // 
            // btnReload
            // 
//            this.btnReload.Caption = "Перезагрузить";
//            this.btnReload.Glyph = ((System.Drawing.Image)(resources.GetObject("btnReload.Glyph")));
//            this.btnReload.Id = 0;
//            this.btnReload.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnReload.LargeGlyph")));
//            this.btnReload.Name = "btnReload";
//            this.btnReload.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReload_ItemClick);
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(775, 31);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 473);
//            this.barDockControlBottom.Size = new System.Drawing.Size(775, 0);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 31);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 442);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(775, 31);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 442);
            // 
            // ucDashboardViewer
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.dashboardViewer1);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Name = "ucDashboardViewer";
//            this.Size = new System.Drawing.Size(775, 473);
//            ((System.ComponentModel.ISupportInitialize)(this.dashboardViewer1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.DashboardWin.DashboardViewer dashboardViewer1;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.Bar barCustom;
//        private DevExpress.XtraBars.BarButtonItem btnReload;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//    }
//}
