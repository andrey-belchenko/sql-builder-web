//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucTreeNew
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucTreeNew));
//            this.tree = new DevExpress.XtraTreeList.TreeList();
//            this.tooltip = new DevExpress.Utils.ToolTipController(this.components);
//            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
//            this.barTopToolbar = new DevExpress.XtraBars.Bar();
//            this.ButtonChoiceRow = new DevExpress.XtraBars.BarButtonItem();
//            this.ButtonRefresh = new DevExpress.XtraBars.BarButtonItem();
//            this.ButtonAddSibling = new DevExpress.XtraBars.BarButtonItem();
//            this.ButtonAddChild = new DevExpress.XtraBars.BarButtonItem();
//            this.ButtonDeleteRow = new DevExpress.XtraBars.BarButtonItem();
//            this.ButtonCommit = new DevExpress.XtraBars.BarButtonItem();
//            this.lcExport = new DevExpress.XtraBars.BarLinkContainerItem();
//            this.ButtonExportExcel = new DevExpress.XtraBars.BarButtonItem();
//            this.lcSettings = new DevExpress.XtraBars.BarLinkContainerItem();
//            this.ButtonSaveSettings = new DevExpress.XtraBars.BarButtonItem();
//            this.ButtonRestoreSettings = new DevExpress.XtraBars.BarButtonItem();
//            this.ButtonUp = new DevExpress.XtraBars.BarButtonItem();
//            this.ButtonDown = new DevExpress.XtraBars.BarButtonItem();
//            this.barBottomToolbar = new DevExpress.XtraBars.Bar();
//            this.barFooter = new DevExpress.XtraBars.Bar();
//            this.teTotalSum = new DevExpress.XtraBars.BarEditItem();
//            this.rteTotalSum = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.teTotalCount = new DevExpress.XtraBars.BarEditItem();
//            this.rteTotalCount = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            ((System.ComponentModel.ISupportInitialize)(this.tree)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).BeginInit();
//            this.SuspendLayout();
            // 
            // tree
            // 
//            this.tree.Appearance.BandPanel.Options.UseTextOptions = true;
//            this.tree.Appearance.BandPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Near;
//            this.tree.Appearance.BandPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.tree.Appearance.BandPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.tree.Appearance.Caption.Options.UseTextOptions = true;
//            this.tree.Appearance.Caption.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.tree.Appearance.Caption.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.tree.Appearance.Caption.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.tree.Appearance.HeaderPanel.Options.UseTextOptions = true;
//            this.tree.Appearance.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.tree.Appearance.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.tree.Appearance.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.tree.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
//            this.tree.AppearancePrint.HeaderPanel.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.tree.AppearancePrint.HeaderPanel.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.tree.AppearancePrint.HeaderPanel.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.tree.Cursor = System.Windows.Forms.Cursors.Default;
//            this.tree.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tree.Location = new System.Drawing.Point(0, 39);
//            this.tree.Name = "tree";
//            this.tree.OptionsBehavior.AllowExpandOnDblClick = false;
//            this.tree.OptionsBehavior.AutoPopulateColumns = false;
//            this.tree.OptionsBehavior.Editable = false;
//            this.tree.OptionsBehavior.EnableFiltering = true;
//            this.tree.OptionsBehavior.KeepSelectedOnClick = false;
//            this.tree.OptionsBehavior.ReadOnly = true;
//            this.tree.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Extended;
//            this.tree.OptionsFilter.ShowAllValuesInFilterPopup = true;
//            this.tree.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.tree.OptionsSelection.MultiSelect = true;
//            this.tree.OptionsSelection.MultiSelectMode = DevExpress.XtraTreeList.TreeListMultiSelectMode.CellSelect;
//            this.tree.OptionsSelection.UseIndicatorForSelection = true;
//            this.tree.OptionsView.AutoWidth = false;
//            this.tree.OptionsView.ShowSummaryFooter = true;
//            this.tree.ParentFieldName = "";
//            this.tree.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowOnlyInEditor;
//            this.tree.Size = new System.Drawing.Size(786, 315);
//            this.tree.TabIndex = 11;
//            this.tree.ToolTipController = this.tooltip;
//            this.tree.AfterDropNode += new DevExpress.XtraTreeList.AfterDropNodeEventHandler(this.tree_AfterDropNode);
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
//            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.barTopToolbar,
//            this.barBottomToolbar,
//            this.barFooter});
//            this.barManager.DockControls.Add(this.barDockControlTop);
//            this.barManager.DockControls.Add(this.barDockControlBottom);
//            this.barManager.DockControls.Add(this.barDockControlLeft);
//            this.barManager.DockControls.Add(this.barDockControlRight);
//            this.barManager.Form = this;
//            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.teTotalSum,
//            this.teTotalCount,
//            this.ButtonRefresh,
//            this.ButtonAddSibling,
//            this.ButtonAddChild,
//            this.ButtonDeleteRow,
//            this.ButtonCommit,
//            this.lcExport,
//            this.ButtonExportExcel,
//            this.lcSettings,
//            this.ButtonSaveSettings,
//            this.ButtonRestoreSettings,
//            this.ButtonUp,
//            this.ButtonDown,
//            this.ButtonChoiceRow});
//            this.barManager.MaxItemId = 8;
//            this.barManager.StatusBar = this.barFooter;
            // 
            // barTopToolbar
            // 
//            this.barTopToolbar.BarName = "Пользовательская 2";
//            this.barTopToolbar.DockCol = 0;
//            this.barTopToolbar.DockRow = 0;
//            this.barTopToolbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.barTopToolbar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonChoiceRow),
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonRefresh, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonAddSibling, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonAddChild),
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonDeleteRow),
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonCommit, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.lcExport),
//            new DevExpress.XtraBars.LinkPersistInfo(this.lcSettings, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonUp, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonDown)});
//            this.barTopToolbar.OptionsBar.AllowQuickCustomization = false;
//            this.barTopToolbar.OptionsBar.DrawBorder = false;
//            this.barTopToolbar.OptionsBar.DrawDragBorder = false;
//            this.barTopToolbar.OptionsBar.MultiLine = true;
//            this.barTopToolbar.OptionsBar.UseWholeRow = true;
//            this.barTopToolbar.Text = "Пользовательская 2";
            // 
            // ButtonChoiceRow
            // 
//            this.ButtonChoiceRow.Caption = "Выбрать";
//            this.ButtonChoiceRow.Glyph = global::sql.builder.Properties.Resources.GridChoiceRow;
//            this.ButtonChoiceRow.Id = 12;
//            this.ButtonChoiceRow.Name = "ButtonChoiceRow";
//            this.ButtonChoiceRow.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonChoiceRow_ItemClick);
            // 
            // ButtonRefresh
            // 
//            this.ButtonRefresh.Caption = "Обновить";
//            this.ButtonRefresh.Glyph = global::sql.builder.Properties.Resources.Refresh_24;
//            this.ButtonRefresh.Id = 1;
//            this.ButtonRefresh.Name = "ButtonRefresh";
//            this.ButtonRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonRefresh_ItemClick);
            // 
            // ButtonAddSibling
            // 
//            this.ButtonAddSibling.Caption = "Добавить";
//            this.ButtonAddSibling.Glyph = global::sql.builder.Properties.Resources.AddItem_24;
//            this.ButtonAddSibling.Id = 2;
//            this.ButtonAddSibling.Name = "ButtonAddSibling";
//            this.ButtonAddSibling.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonAddSibling_ItemClick);
            // 
            // ButtonAddChild
            // 
//            this.ButtonAddChild.Caption = "Добавить дочерний";
//            this.ButtonAddChild.Glyph = global::sql.builder.Properties.Resources.AddChildItem_24;
//            this.ButtonAddChild.Id = 3;
//            this.ButtonAddChild.Name = "ButtonAddChild";
//            this.ButtonAddChild.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonAddChild_ItemClick);
            // 
            // ButtonDeleteRow
            // 
//            this.ButtonDeleteRow.Caption = "Удалить";
//            this.ButtonDeleteRow.Glyph = global::sql.builder.Properties.Resources.DeleteItem_24;
//            this.ButtonDeleteRow.Id = 4;
//            this.ButtonDeleteRow.Name = "ButtonDeleteRow";
//            this.ButtonDeleteRow.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonDeleteRow_ItemClick);
            // 
            // ButtonCommit
            // 
//            this.ButtonCommit.Caption = "Сохранить";
//            this.ButtonCommit.Glyph = global::sql.builder.Properties.Resources.Commit_24;
//            this.ButtonCommit.Id = 5;
//            this.ButtonCommit.Name = "ButtonCommit";
//            this.ButtonCommit.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonCommit_ItemClick);
            // 
            // lcExport
            // 
//            this.lcExport.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.lcExport.Glyph = global::sql.builder.Properties.Resources.GridExportToFile_24;
//            this.lcExport.Id = 6;
//            this.lcExport.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonExportExcel)});
//            this.lcExport.Name = "lcExport";
//            this.lcExport.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.lcExport.ShowItemShortcut = DevExpress.Utils.DefaultBoolean.False;
            // 
            // ButtonExportExcel
            // 
//            this.ButtonExportExcel.Caption = "Экспорт в Excel";
//            this.ButtonExportExcel.Glyph = global::sql.builder.Properties.Resources.ExportXls_161;
//            this.ButtonExportExcel.Id = 7;
//            this.ButtonExportExcel.Name = "ButtonExportExcel";
//            this.ButtonExportExcel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonExportExcel_ItemClick);
            // 
            // lcSettings
            // 
//            this.lcSettings.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.lcSettings.Glyph = global::sql.builder.Properties.Resources.Settings_24;
//            this.lcSettings.Id = 9;
//            this.lcSettings.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonSaveSettings),
//            new DevExpress.XtraBars.LinkPersistInfo(this.ButtonRestoreSettings)});
//            this.lcSettings.Name = "lcSettings";
//            this.lcSettings.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // ButtonSaveSettings
            // 
//            this.ButtonSaveSettings.Caption = "Сохранить настройки колонок";
//            this.ButtonSaveSettings.Id = 10;
//            this.ButtonSaveSettings.Name = "ButtonSaveSettings";
//            this.ButtonSaveSettings.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonSaveSettings_ItemClick);
            // 
            // ButtonRestoreSettings
            // 
//            this.ButtonRestoreSettings.Caption = "Сбросить настройки колонок";
//            this.ButtonRestoreSettings.Enabled = false;
//            this.ButtonRestoreSettings.Id = 11;
//            this.ButtonRestoreSettings.Name = "ButtonRestoreSettings";
//            this.ButtonRestoreSettings.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonRestoreSettings_ItemClick);
            // 
            // ButtonUp
            // 
//            this.ButtonUp.Caption = "Переместить выбранные строки выше";
//            this.ButtonUp.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonUp.Glyph")));
//            this.ButtonUp.Id = 6;
//            this.ButtonUp.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonUp.LargeGlyph")));
//            this.ButtonUp.Name = "ButtonUp";
//            this.ButtonUp.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.ButtonUp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonUp_ItemClick);
            // 
            // ButtonDown
            // 
//            this.ButtonDown.Caption = "Переместить выбранные строки ниже";
//            this.ButtonDown.Glyph = ((System.Drawing.Image)(resources.GetObject("ButtonDown.Glyph")));
//            this.ButtonDown.Id = 7;
//            this.ButtonDown.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("ButtonDown.LargeGlyph")));
//            this.ButtonDown.Name = "ButtonDown";
//            this.ButtonDown.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.ButtonDown.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.ButtonDown_ItemClick);
            // 
            // barBottomToolbar
            // 
//            this.barBottomToolbar.BarName = "Пользовательская 3";
//            this.barBottomToolbar.DockCol = 0;
//            this.barBottomToolbar.DockRow = 1;
//            this.barBottomToolbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.barBottomToolbar.OptionsBar.AllowQuickCustomization = false;
//            this.barBottomToolbar.OptionsBar.DrawBorder = false;
//            this.barBottomToolbar.OptionsBar.DrawDragBorder = false;
//            this.barBottomToolbar.OptionsBar.MultiLine = true;
//            this.barBottomToolbar.OptionsBar.UseWholeRow = true;
//            this.barBottomToolbar.Text = "Пользовательская 3";
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
//            this.barDockControlTop.Size = new System.Drawing.Size(786, 39);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 354);
//            this.barDockControlBottom.Size = new System.Drawing.Size(786, 54);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 39);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 315);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(786, 39);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 315);
            // 
            // ucTreeNew
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.tree);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Name = "ucTreeNew";
//            this.Size = new System.Drawing.Size(786, 408);
//            ((System.ComponentModel.ISupportInitialize)(this.tree)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraTreeList.TreeList tree;
//        private DevExpress.XtraBars.BarManager barManager;
//        private DevExpress.XtraBars.Bar barTopToolbar;
//        private DevExpress.XtraBars.Bar barBottomToolbar;
//        private DevExpress.XtraBars.Bar barFooter;
//        private DevExpress.XtraBars.BarEditItem teTotalSum;
//        private DevExpress.XtraBars.BarEditItem teTotalCount;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteTotalSum;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteTotalCount;
//        private DevExpress.XtraBars.BarButtonItem ButtonRefresh;
//        private DevExpress.XtraBars.BarButtonItem ButtonAddSibling;
//        private DevExpress.XtraBars.BarButtonItem ButtonAddChild;
//        private DevExpress.XtraBars.BarButtonItem ButtonDeleteRow;
//        private DevExpress.XtraBars.BarButtonItem ButtonCommit;
//        private DevExpress.Utils.ToolTipController tooltip;
//        private DevExpress.XtraBars.BarLinkContainerItem lcExport;
//        private DevExpress.XtraBars.BarButtonItem ButtonExportExcel;
//        private DevExpress.XtraBars.BarLinkContainerItem lcSettings;
//        private DevExpress.XtraBars.BarButtonItem ButtonSaveSettings;
//        private DevExpress.XtraBars.BarButtonItem ButtonRestoreSettings;
//        private DevExpress.XtraBars.BarButtonItem ButtonUp;
//        private DevExpress.XtraBars.BarButtonItem ButtonDown;
//        private DevExpress.XtraBars.BarButtonItem ButtonChoiceRow;
//    }
//}
