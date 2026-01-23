//namespace sql.builder.Controls.Containers
//{
//    internal partial class ucReportContainer
//    {
//        /// <summary> 
//        /// Требуется переменная конструктора.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Освободить все используемые ресурсы.
//        /// </summary>
//        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
//        protected void Dispose(bool disposing)
//        {
//            if (disposing && (components != null))
//            {
//                components.Dispose();
//            }
//            base.Dispose(disposing);
//        }

//        #region Код, автоматически созданный конструктором компонентов

//        /// <summary> 
//        /// Обязательный метод для поддержки конструктора - не изменяйте 
//        /// содержимое данного метода при помощи редактора кода.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer dockingContainer1 = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer();
//            this.dgReport = new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup(this.components);
//            this.docReport = new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document(this.components);
//            this.dockManager = new DevExpress.XtraBars.Docking.DockManager(this.components);
//            this.dpParams = new DevExpress.XtraBars.Docking.DockPanel();
//            this.dpReportContainer = new DevExpress.XtraBars.Docking.ControlContainer();
//            this.pParams = new DevExpress.XtraEditors.PanelControl();
//            this.dpGrid = new DevExpress.XtraBars.Docking.DockPanel();
//            this.dmView = new DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView(this.components);
//            this.dmGrid = new DevExpress.XtraBars.Docking2010.DocumentManager(this.components);
//            ((System.ComponentModel.ISupportInitialize)(this.dgReport)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.docReport)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).BeginInit();
//            this.dpParams.SuspendLayout();
//            this.dpReportContainer.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.pParams)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dmView)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dmGrid)).BeginInit();
//            this.SuspendLayout();
//            // 
//            // dgReport
//            // 
//            this.dgReport.Items.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.Document[] {
//            this.docReport});
//            this.dgReport.Properties.HeaderButtonsShowMode = DevExpress.XtraTab.TabButtonShowMode.Never;
//            this.dgReport.Properties.HeaderLocation = DevExpress.XtraTab.TabHeaderLocation.Top;
//            this.dgReport.Properties.ShowDocumentSelectorButton = DevExpress.Utils.DefaultBoolean.False;
//            this.dgReport.Properties.ShowTabHeader = DevExpress.Utils.DefaultBoolean.False;
//            // 
//            // docReport
//            // 
//            this.docReport.Caption = "Отчёт";
//            this.docReport.ControlName = "dpGrid";
//            this.docReport.FloatLocation = new System.Drawing.Point(566, 380);
//            this.docReport.FloatSize = new System.Drawing.Size(200, 200);
//            this.docReport.Header = "";
//            this.docReport.Properties.AllowAnimation = DevExpress.Utils.DefaultBoolean.True;
//            this.docReport.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.True;
//            this.docReport.Properties.AllowDock = DevExpress.Utils.DefaultBoolean.False;
//            this.docReport.Properties.AllowDockFill = DevExpress.Utils.DefaultBoolean.False;
//            this.docReport.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.True;
//            this.docReport.Properties.AllowFloatOnDoubleClick = DevExpress.Utils.DefaultBoolean.True;
//            this.docReport.Properties.ShowPinButton = DevExpress.Utils.DefaultBoolean.True;
//            // 
//            // dockManager
//            // 
//            this.dockManager.Form = this;
//            this.dockManager.RootPanels.AddRange(new DevExpress.XtraBars.Docking.DockPanel[] {
//            this.dpParams,
//            this.dpGrid});
//            this.dockManager.TopZIndexControls.AddRange(new string[] {
//            "DevExpress.XtraBars.BarDockControl",
//            "DevExpress.XtraBars.StandaloneBarDockControl",
//            "System.Windows.Forms.StatusBar",
//            "System.Windows.Forms.MenuStrip",
//            "System.Windows.Forms.StatusStrip",
//            "DevExpress.XtraBars.Ribbon.RibbonStatusBar",
//            "DevExpress.XtraBars.Ribbon.RibbonControl"});
//            this.dockManager.StartSizing += new DevExpress.XtraBars.Docking.StartSizingEventHandler(this.dockManager_StartSizing);
//            this.dockManager.Sizing += new DevExpress.XtraBars.Docking.SizingEventHandler(this.dockManager_Sizing);
//            this.dockManager.EndSizing += new DevExpress.XtraBars.Docking.EndSizingEventHandler(this.dockManager_EndSizing);
//            this.dockManager.ShowingDockGuides += new DevExpress.XtraBars.Docking.ShowingDockGuidesEventHandler(this.dockManager_ShowingDockGuides);
//            // 
//            // dpParams
//            // 
//            this.dpParams.Controls.Add(this.dpReportContainer);
//            this.dpParams.Dock = DevExpress.XtraBars.Docking.DockingStyle.Left;
//            this.dpParams.ID = new System.Guid("7bc8a1dd-63c4-427a-8b71-021d7d4f77d1");
//            this.dpParams.Location = new System.Drawing.Point(0, 0);
//            this.dpParams.Name = "dpParams";
//            this.dpParams.Options.AllowDockAsTabbedDocument = false;
//            this.dpParams.Options.ShowCloseButton = false;
//            this.dpParams.OriginalSize = new System.Drawing.Size(265, 200);
//            this.dpParams.Size = new System.Drawing.Size(265, 532);
//            this.dpParams.Text = "Параметры отчёта";
//            // 
//            // dpReportContainer
//            // 
//            this.dpReportContainer.Controls.Add(this.pParams);
//            this.dpReportContainer.Location = new System.Drawing.Point(4, 23);
//            this.dpReportContainer.Name = "dpReportContainer";
//            this.dpReportContainer.Size = new System.Drawing.Size(257, 505);
//            this.dpReportContainer.TabIndex = 0;
//            // 
//            // pParams
//            // 
//            this.pParams.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.pParams.Appearance.Options.UseBackColor = true;
//            this.pParams.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.pParams.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.pParams.Location = new System.Drawing.Point(0, 0);
//            this.pParams.Margin = new System.Windows.Forms.Padding(0);
//            this.pParams.Name = "pParams";
//            this.pParams.Size = new System.Drawing.Size(257, 505);
//            this.pParams.TabIndex = 0;
            
//            // 
//            // dpGrid
//            // 
//            this.dpGrid.DockedAsTabbedDocument = true;
//            this.dpGrid.FloatLocation = new System.Drawing.Point(566, 380);
//            this.dpGrid.ID = new System.Guid("51e3534a-b1b1-4fcc-8a5b-4efe06faeab8");
//            this.dpGrid.Name = "dpGrid";
//            this.dpGrid.OriginalSize = new System.Drawing.Size(300, 300);
//            this.dpGrid.SavedIndex = 1;
//            this.dpGrid.SavedMdiDocument = true;
//            this.dpGrid.Text = "Отчет";
//            // 
//            // dmView
//            // 
//            this.dmView.DocumentGroups.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup[] {
//            this.dgReport});
//            this.dmView.Documents.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseDocument[] {
//            this.docReport});
//            dockingContainer1.Element = this.dgReport;
//            this.dmView.RootContainer.Nodes.AddRange(new DevExpress.XtraBars.Docking2010.Views.Tabbed.DockingContainer[] {
//            dockingContainer1});
//            this.dmView.UseDocumentSelector = DevExpress.Utils.DefaultBoolean.False;
//            this.dmView.UseLoadingIndicator = DevExpress.Utils.DefaultBoolean.False;
//            // 
//            // dmGrid
//            // 
//            this.dmGrid.ContainerControl = this;
//            this.dmGrid.View = this.dmView;
//            this.dmGrid.ViewCollection.AddRange(new DevExpress.XtraBars.Docking2010.Views.BaseView[] {
//            this.dmView});
//            // 
//            // ucReportContainer
//            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.dpParams);
//            this.Name = "ucReportContainer";
//            this.Size = new System.Drawing.Size(896, 532);
//            ((System.ComponentModel.ISupportInitialize)(this.dgReport)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.docReport)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dockManager)).EndInit();
//            this.dpParams.ResumeLayout(false);
//            this.dpReportContainer.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.pParams)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dmView)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.dmGrid)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraBars.Docking.DockManager dockManager;
//        private DevExpress.XtraBars.Docking.DockPanel dpParams;
//        private DevExpress.XtraBars.Docking.ControlContainer dpReportContainer;
//        private DevExpress.XtraBars.Docking.DockPanel dpGrid;
//        private DevExpress.XtraEditors.PanelControl pParams;
//        private DevExpress.XtraBars.Docking2010.Views.Tabbed.TabbedView dmView;
//        private DevExpress.XtraBars.Docking2010.Views.Tabbed.DocumentGroup dgReport;
//        private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document docReport;
//        //private DevExpress.XtraBars.Docking2010.Views.Tabbed.Document document1;
//        private DevExpress.XtraBars.Docking2010.DocumentManager dmGrid;
//    }
//}
