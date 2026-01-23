//using sql.builder.UI.WinForms;
//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    partial class ucGridContainerWF
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
        //private void InitializeComponent()
        //{
        //    this.components = new System.ComponentModel.Container();
        //    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucGridWF));
        //    this.grid = new DevExpress.XtraGrid.GridControl();
        //    this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
        //    this.tooltip = new DevExpress.Utils.ToolTipController(this.components);
        //    this.barManager = new DevExpress.XtraBars.BarManager(this.components);
        //    this.barTopToolbar = new DevExpress.XtraBars.Bar();
        //    this.ButtonChoiceRow = new DevExpress.XtraBars.BarButtonItem();
        //    this.ButtonRefresh = new DevExpress.XtraBars.BarButtonItem();
        //    this.ButtonAddRow = new DevExpress.XtraBars.BarButtonItem();
        //    this.ButtonDeleteRow = new DevExpress.XtraBars.BarButtonItem();
        //    this.ButtonCommit = new DevExpress.XtraBars.BarButtonItem();
        //    this.lcExport = new DevExpress.XtraBars.BarLinkContainerItem();
        //    this.ButtonExportExcel = new DevExpress.XtraBars.BarButtonItem();
        //    this.lcSettings = new DevExpress.XtraBars.BarLinkContainerItem();
        //    this.ButtonSaveSettings = new DevExpress.XtraBars.BarButtonItem();
        //    this.ButtonRestoreSettings = new DevExpress.XtraBars.BarButtonItem();
        //    this.ButtonUp = new DevExpress.XtraBars.BarButtonItem();
        //    this.ButtonDown = new DevExpress.XtraBars.BarButtonItem();
        //    this.barFooter = new DevExpress.XtraBars.Bar();
        //    this.teTotalSum = new DevExpress.XtraBars.BarEditItem();
        //    this.rteTotalSum = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
        //    this.teTotalCount = new DevExpress.XtraBars.BarEditItem();
        //    this.rteTotalCount = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
        //    this.lCompareResult = new DevExpress.XtraBars.BarStaticItem();
        //    this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
        //    this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
        //    this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
        //    this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
        //    ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).BeginInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).BeginInit();
        //    this.SuspendLayout();
        //    // 
        //    // grid
        //    // 
        //    this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
        //    this.grid.Location = new System.Drawing.Point(0, 39);
        //    this.grid.MainView = this.gridView;
        //    this.grid.Name = "grid";
        //    this.grid.Size = new System.Drawing.Size(686, 319);
        //    this.grid.TabIndex = 5;
        //    this.grid.ToolTipController = this.tooltip;
        //    this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
        //    this.gridView});
        //    // 
        //    // gridView
        //    // 
        //    this.gridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
        //    this.gridView.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        //    this.gridView.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
        //    this.gridView.AppearancePrint.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
        //    this.gridView.GridControl = this.grid;
        //    this.gridView.Name = "gridView";
        //    this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
        //    this.gridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
        //    this.gridView.OptionsBehavior.Editable = false;
        //    this.gridView.OptionsBehavior.ReadOnly = true;
        //    this.gridView.OptionsDetail.AllowZoomDetail = false;
        //    this.gridView.OptionsDetail.ShowDetailTabs = false;
        //    this.gridView.OptionsPrint.AutoWidth = false;
        //    this.gridView.OptionsSelection.MultiSelect = true;
        //    this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
        //    this.gridView.OptionsView.AllowHtmlDrawHeaders = true;
        //    this.gridView.OptionsView.ColumnAutoWidth = false;
        //    this.gridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.False;
        //    this.gridView.OptionsView.RowAutoHeight = true;
        //    this.gridView.OptionsView.ShowFooter = true;
        //    this.gridView.OptionsView.ShowGroupPanel = false;
        //    // 
        //    // tooltip
        //    // 
        //    this.tooltip.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.tooltip_GetActiveObjectInfo);
        //    // 
        //    // barManager
        //    // 
        //    this.barManager.AllowCustomization = false;
        //    this.barManager.AllowMoveBarOnToolbar = false;
        //    this.barManager.AllowQuickCustomization = false;
        //    this.barManager.AllowShowToolbarsPopup = false;
        //    this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
        //    this.barTopToolbar,
        //    this.barFooter});
        //    this.barManager.DockControls.Add(this.barDockControlTop);
        //    this.barManager.DockControls.Add(this.barDockControlBottom);
        //    this.barManager.DockControls.Add(this.barDockControlLeft);
        //    this.barManager.DockControls.Add(this.barDockControlRight);
        //    this.barManager.Form = this;
        //    this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
        //    this.teTotalSum,
        //    this.teTotalCount,
        //    this.lCompareResult,
        //    this.ButtonRefresh,
        //    this.ButtonAddRow,
        //    this.ButtonDeleteRow,
        //    this.ButtonCommit,
        //    this.ButtonUp,
        //    this.ButtonDown,
        //    this.lcExport,
        //    this.ButtonExportExcel,
        //    this.lcSettings,
        //    this.ButtonSaveSettings,
        //    this.ButtonRestoreSettings,
        //    this.ButtonChoiceRow});
        //    this.barManager.MaxItemId = 13;
        //    this.barManager.StatusBar = this.barFooter;
        //    // 
        //    // barTopToolbar
        //    // 
        //    this.barTopToolbar.BarName = "Tools";
        //    this.barTopToolbar.DockCol = 0;
        //    this.barTopToolbar.DockRow = 0;
        //    this.barTopToolbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
        //    this.barTopToolbar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonChoiceRow),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonRefresh, true),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonAddRow, true),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonDeleteRow),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonCommit, true),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.lcExport),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.lcSettings, true),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonUp, true),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonDown)});
        //    this.barTopToolbar.OptionsBar.AllowQuickCustomization = false;
        //    this.barTopToolbar.OptionsBar.DrawBorder = false;
        //    this.barTopToolbar.OptionsBar.DrawDragBorder = false;
        //    this.barTopToolbar.OptionsBar.MultiLine = true;
        //    this.barTopToolbar.OptionsBar.UseWholeRow = true;
        //    this.barTopToolbar.Text = "Tools";
        //    // 
        //    // ButtonChoiceRow
        //    // 
        //    this.ButtonChoiceRow.Caption = "Выбрать";
        //    this.ButtonChoiceRow.Glyph = global::sql.builder.Properties.Resources.GridChoiceRow;
        //    this.ButtonChoiceRow.Id = 12;
        //    this.ButtonChoiceRow.Name = "ButtonChoiceRow";
        //    // 
        //    // ButtonRefresh
        //    // 
        //    this.ButtonRefresh.Caption = "Обновить";
        //    this.ButtonRefresh.Glyph = global::sql.builder.Properties.Resources.Refresh_24;
        //    this.ButtonRefresh.Id = 1;
        //    this.ButtonRefresh.Name = "ButtonRefresh";
        //    // 
        //    // ButtonAddRow
        //    // 
        //    this.ButtonAddRow.Caption = "Добавить строку";
        //    this.ButtonAddRow.Glyph = global::sql.builder.Properties.Resources.GridRowAdd_24;
        //    this.ButtonAddRow.Id = 2;
        //    this.ButtonAddRow.Name = "ButtonAddRow";
        //    // 
        //    // ButtonDeleteRow
        //    // 
        //    this.ButtonDeleteRow.Caption = "Удалить строку";
        //    this.ButtonDeleteRow.Glyph = global::sql.builder.Properties.Resources.GridRowDelete_24;
        //    this.ButtonDeleteRow.Id = 3;
        //    this.ButtonDeleteRow.Name = "ButtonDeleteRow";
        //    // 
        //    // ButtonCommit
        //    // 
        //    this.ButtonCommit.Caption = "Сохранить";
        //    this.ButtonCommit.Glyph = global::sql.builder.Properties.Resources.Commit_24;
        //    this.ButtonCommit.Id = 4;
        //    this.ButtonCommit.Name = "ButtonCommit";
        //    // 
        //    // lcExport
        //    // 
        //    this.lcExport.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
        //    this.lcExport.Glyph = global::sql.builder.Properties.Resources.GridExportToFile_24;
        //    this.lcExport.Id = 6;
        //    this.lcExport.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonExportExcel)});
        //    this.lcExport.Name = "lcExport";
        //    this.lcExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
        //    this.lcExport.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.False;
        //    // 
        //    // ButtonExportExcel
        //    // 
        //    this.ButtonExportExcel.Caption = "Экспорт в Excel";
        //    this.ButtonExportExcel.Glyph = global::sql.builder.Properties.Resources.ExportXls_161;
        //    this.ButtonExportExcel.Id = 7;
        //    this.ButtonExportExcel.Name = "ButtonExportExcel";
        //    // 
        //    // lcSettings
        //    // 
        //    this.lcSettings.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
        //    this.lcSettings.Glyph = global::sql.builder.Properties.Resources.Settings_24;
        //    this.lcSettings.Id = 9;
        //    this.lcSettings.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonSaveSettings),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.ButtonRestoreSettings)});
        //    this.lcSettings.Name = "lcSettings";
        //    this.lcSettings.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
        //    // 
        //    // ButtonSaveSettings
        //    // 
        //    this.ButtonSaveSettings.Caption = "Сохранить настройки колонок";
        //    this.ButtonSaveSettings.Id = 10;
        //    this.ButtonSaveSettings.Name = "ButtonSaveSettings";
        //    this.ButtonSaveSettings.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //    // 
        //    // ButtonRestoreSettings
        //    // 
        //    this.ButtonRestoreSettings.Caption = "Сбросить настройки колонок";
        //    this.ButtonRestoreSettings.Enabled = false;
        //    this.ButtonRestoreSettings.Id = 11;
        //    this.ButtonRestoreSettings.Name = "ButtonRestoreSettings";
        //    // 
        //    // ButtonUp
        //    // 
        //    this.ButtonUp.Caption = "Переместить выбранные строки выше";
        //    this.ButtonUp.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonUp.Glyph")));
        //    this.ButtonUp.Id = 6;
        //    this.ButtonUp.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonUp.LargeGlyph")));
        //    this.ButtonUp.Name = "ButtonUp";
        //    this.ButtonUp.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //    // 
        //    // ButtonDown
        //    // 
        //    this.ButtonDown.Caption = "Переместить выбранные строки ниже";
        //    this.ButtonDown.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonDown.Glyph")));
        //    this.ButtonDown.Id = 7;
        //    this.ButtonDown.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonDown.LargeGlyph")));
        //    this.ButtonDown.Name = "ButtonDown";
        //    this.ButtonDown.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //    // 
        //    // barFooter
        //    // 
        //    this.barFooter.BarName = "Status bar";
        //    this.barFooter.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
        //    this.barFooter.DockCol = 0;
        //    this.barFooter.DockRow = 0;
        //    this.barFooter.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
        //    this.barFooter.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
        //    new DevExpress.XtraBars.LinkPersistInfo(this.teTotalSum),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.teTotalCount),
        //    new DevExpress.XtraBars.LinkPersistInfo(this.lCompareResult, true)});
        //    this.barFooter.OptionsBar.AllowQuickCustomization = false;
        //    this.barFooter.OptionsBar.DrawBorder = false;
        //    this.barFooter.OptionsBar.DrawDragBorder = false;
        //    this.barFooter.OptionsBar.UseWholeRow = true;
        //    this.barFooter.Text = "Status bar";
        //    // 
        //    // teTotalSum
        //    // 
        //    this.teTotalSum.Border = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        //    this.teTotalSum.Caption = "Сумма";
        //    this.teTotalSum.Edit = this.rteTotalSum;
        //    this.teTotalSum.EditValue = "0";
        //    this.teTotalSum.EditWidth = 140;
        //    this.teTotalSum.Id = 4;
        //    this.teTotalSum.Name = "teTotalSum";
        //    this.teTotalSum.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
        //    // 
        //    // rteTotalSum
        //    // 
        //    this.rteTotalSum.AutoHeight = false;
        //    this.rteTotalSum.Name = "rteTotalSum";
        //    this.rteTotalSum.ReadOnly = true;
        //    // 
        //    // teTotalCount
        //    // 
        //    this.teTotalCount.Border = DevExpress.XtraEditors.Controls.BorderStyles.Default;
        //    this.teTotalCount.Caption = "Количество";
        //    this.teTotalCount.Edit = this.rteTotalCount;
        //    this.teTotalCount.EditValue = "0";
        //    this.teTotalCount.EditWidth = 140;
        //    this.teTotalCount.Id = 6;
        //    this.teTotalCount.Name = "teTotalCount";
        //    this.teTotalCount.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
        //    // 
        //    // rteTotalCount
        //    // 
        //    this.rteTotalCount.AutoHeight = false;
        //    this.rteTotalCount.Name = "rteTotalCount";
        //    this.rteTotalCount.ReadOnly = true;
        //    // 
        //    // lCompareResult
        //    // 
        //    this.lCompareResult.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
        //    this.lCompareResult.Caption = "Всего расхождений";
        //    this.lCompareResult.Id = 0;
        //    this.lCompareResult.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
        //    this.lCompareResult.ItemAppearance.Normal.Options.UseFont = true;
        //    this.lCompareResult.Name = "lCompareResult";
        //    this.lCompareResult.TextAlignment = System.Drawing.StringAlignment.Near;
        //    this.lCompareResult.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
        //    // 
        //    // barDockControlTop
        //    // 
        //    this.barDockControlTop.CausesValidation = false;
        //    this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
        //    this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
        //    this.barDockControlTop.Size = new System.Drawing.Size(686, 39);
        //    // 
        //    // barDockControlBottom
        //    // 
        //    this.barDockControlBottom.CausesValidation = false;
        //    this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
        //    this.barDockControlBottom.Location = new System.Drawing.Point(0, 358);
        //    this.barDockControlBottom.Size = new System.Drawing.Size(686, 25);
        //    // 
        //    // barDockControlLeft
        //    // 
        //    this.barDockControlLeft.CausesValidation = false;
        //    this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
        //    this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
        //    this.barDockControlLeft.Size = new System.Drawing.Size(0, 319);
        //    // 
        //    // barDockControlRight
        //    // 
        //    this.barDockControlRight.CausesValidation = false;
        //    this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
        //    this.barDockControlRight.Location = new System.Drawing.Point(686, 39);
        //    this.barDockControlRight.Size = new System.Drawing.Size(0, 319);
        //    // 
        //    // ucGridWF
        //    // 
        //    this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
        //    this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
        //    this.Controls.Add(this.grid);
        //    this.Controls.Add(this.barDockControlLeft);
        //    this.Controls.Add(this.barDockControlRight);
        //    this.Controls.Add(this.barDockControlBottom);
        //    this.Controls.Add(this.barDockControlTop);
        //    this.Name = "ucGridWF";
        //    this.Size = new System.Drawing.Size(686, 383);
        //    this.Resize += new System.EventHandler(this.ucGridWF_Resize);
        //    ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).EndInit();
        //    ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).EndInit();
        //    this.ResumeLayout(false);
        //    this.PerformLayout();

        //}


//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucGridContainerWF));
            //this.grid = new DevExpress.XtraGrid.GridControl();
            //this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.tooltip = new DevExpress.Utils.ToolTipController(this.components);
//            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
            //this.barTopToolbar = new DevExpress.XtraBars.Bar();
           
//            this.barFooter = new DevExpress.XtraBars.Bar();
//            this.teTotalSum = new DevExpress.XtraBars.BarEditItem();
//            this.rteTotalSum = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.teTotalCount = new DevExpress.XtraBars.BarEditItem();
//            this.rteTotalCount = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.lCompareResult = new DevExpress.XtraBars.BarStaticItem();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
            //((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
            //((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).BeginInit();
//            this.SuspendLayout();
            // 
            // grid
            // 
            //this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
            //this.grid.Location = new System.Drawing.Point(0, 39);
//            ////this.grid.MainView = this.gridView;
            //this.grid.Name = "grid";
            //this.grid.Size = new System.Drawing.Size(686, 319);
            //this.grid.TabIndex = 5;
            //this.grid.ToolTipController = this.tooltip;
            //this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
            //this.gridView
            //}
            
            //);
            // 
            // gridView
            // 
            //this.gridView.Appearance.HeaderPanel.Options.UseTextOptions = true;
            //this.gridView.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            //this.gridView.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            //this.gridView.AppearancePrint.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
            //this.gridView.GridControl = this.grid;
            //this.gridView.Name = "gridView";
            //this.gridView.OptionsBehavior.AllowAddRows = DevExpress.Utils.DefaultBoolean.False;
            //this.gridView.OptionsBehavior.AllowDeleteRows = DevExpress.Utils.DefaultBoolean.False;
            //this.gridView.OptionsBehavior.Editable = false;
            //this.gridView.OptionsBehavior.ReadOnly = true;
            //this.gridView.OptionsDetail.AllowZoomDetail = false;
            //this.gridView.OptionsDetail.ShowDetailTabs = false;
            //this.gridView.OptionsPrint.AutoWidth = false;
            //this.gridView.OptionsSelection.MultiSelect = true;
            //this.gridView.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
            //this.gridView.OptionsView.AllowHtmlDrawHeaders = true;
            //this.gridView.OptionsView.ColumnAutoWidth = false;
            //this.gridView.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.False;
            //this.gridView.OptionsView.RowAutoHeight = true;
            //this.gridView.OptionsView.ShowFooter = true;
            //this.gridView.OptionsView.ShowGroupPanel = false;
            // 
            // tooltip
            // 
//            this.tooltip.GetActiveObjectInfo += new DevExpress.Utils.ToolTipControllerGetActiveObjectInfoEventHandler(this.tooltip_GetActiveObjectInfo);
            // 
            // barManager
            // 
//            this.barManager.AllowCustomization = false;
//            this.barManager.AllowMoveBarOnToolbar = false;
//            this.barManager.AllowQuickCustomization = false;
//            this.barManager.AllowShowToolbarsPopup = false;
//            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {this.barFooter});
            //this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {this.barTopToolbar});
//            this.barManager.DockControls.Add(this.barDockControlTop);
//            this.barManager.DockControls.Add(this.barDockControlBottom);
//            this.barManager.DockControls.Add(this.barDockControlLeft);
//            this.barManager.DockControls.Add(this.barDockControlRight);
//            this.barManager.Form = this;
//            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.teTotalSum,
//            this.teTotalCount,
//            this.lCompareResult
//            });

        

//            this.barManager.MaxItemId = 13;
//            this.barManager.StatusBar = this.barFooter;
            // 
            // barTopToolbar
            // 
            //this.barTopToolbar.BarName = "Tools";
            //this.barTopToolbar.DockCol = 0;
            //this.barTopToolbar.DockRow = 0;
            //this.barTopToolbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
          
            //this.barTopToolbar.OptionsBar.AllowQuickCustomization = false;
            //this.barTopToolbar.OptionsBar.DrawBorder = false;
            //this.barTopToolbar.OptionsBar.DrawDragBorder = false;
            //this.barTopToolbar.OptionsBar.MultiLine = true;
            //this.barTopToolbar.OptionsBar.UseWholeRow = true;
            //this.barTopToolbar.Text = "Tools";
           
//            this.barFooter.BarName = "Status bar";
//            this.barFooter.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
//            this.barFooter.DockCol = 0;
//            this.barFooter.DockRow = 0;
//            this.barFooter.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.barFooter.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.teTotalSum),
//            new DevExpress.XtraBars.LinkPersistInfo(this.teTotalCount),
//            new DevExpress.XtraBars.LinkPersistInfo(this.lCompareResult, true)});
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
            // lCompareResult
            // 
//            this.lCompareResult.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.lCompareResult.Caption = "Всего расхождений";
//            this.lCompareResult.Id = 0;
//            this.lCompareResult.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold);
//            this.lCompareResult.ItemAppearance.Normal.Options.UseFont = true;
//            this.lCompareResult.Name = "lCompareResult";
//            this.lCompareResult.TextAlignment = System.Drawing.StringAlignment.Near;
//            this.lCompareResult.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(686, 39);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 358);
//            this.barDockControlBottom.Size = new System.Drawing.Size(686, 25);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 319);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(686, 39);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 319);
            // 
            // ucGridWF
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            //this.Controls.Add(this.grid);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Name = "ucGridWF";
//            this.Size = new System.Drawing.Size(686, 383);
//            this.Resize += new System.EventHandler(this.ucGridWF_Resize);
            //((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
            //((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }



//        #endregion

//        public DevExpress.XtraBars.BarManager barManager;
//        public VBar barTopToolbar;
//        public VBar barBottomToolbar;
//        public DevExpress.XtraBars.Bar barFooter;
//        public DevExpress.XtraBars.BarDockControl barDockControlTop;
//        public DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        public DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        public DevExpress.XtraBars.BarDockControl barDockControlRight;
//        public DevExpress.XtraBars.BarStaticItem lCompareResult;
//        public DevExpress.XtraBars.BarEditItem teTotalSum;
//        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteTotalSum;
//        public DevExpress.XtraBars.BarEditItem teTotalCount;
//        public DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteTotalCount;
        //public DevExpress.XtraBars.BarButtonItem ButtonRefresh;
        //public DevExpress.XtraBars.BarButtonItem ButtonAddRow;
        //public DevExpress.XtraBars.BarButtonItem ButtonDeleteRow;
        //public DevExpress.XtraBars.BarButtonItem ButtonCommit;
//        public DevExpress.Utils.ToolTipController tooltip;
       // public DevExpress.XtraGrid.Views.Grid.GridView gridView;
       
        //private DevExpress.XtraBars.BarLinkContainerItem lcExport;
        //public DevExpress.XtraBars.BarButtonItem ButtonExportExcel;
        //private DevExpress.XtraBars.BarLinkContainerItem lcSettings;
        //public DevExpress.XtraBars.BarButtonItem ButtonSaveSettings;
        //public DevExpress.XtraBars.BarButtonItem ButtonRestoreSettings;
        //public DevExpress.XtraBars.BarButtonItem ButtonUp;
        //public DevExpress.XtraBars.BarButtonItem ButtonDown;
        //public DevExpress.XtraBars.BarButtonItem ButtonChoiceRow;
//    }
//}
