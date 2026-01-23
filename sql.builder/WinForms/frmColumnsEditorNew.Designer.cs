//namespace sql.builder.WinForms
//{
//    internal partial class frmColumnsEditorNew
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmColumnsEditorNew));
//            this.barManager1 = new DevExpress.XtraBars.BarManager();
//            this.bar2 = new DevExpress.XtraBars.Bar();
//            this.btnCheckAll = new DevExpress.XtraBars.BarLargeButtonItem();
//            this.btnClearAll = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCheckSelected = new DevExpress.XtraBars.BarButtonItem();
//            this.btnClearSelected = new DevExpress.XtraBars.BarButtonItem();
//            this.btnShowMerge = new DevExpress.XtraBars.BarButtonItem();
//            this.btnMoveBandLeft = new DevExpress.XtraBars.BarButtonItem();
//            this.btnMoveBandRight = new DevExpress.XtraBars.BarButtonItem();
//            this.btnInSelOrdr = new DevExpress.XtraBars.BarButtonItem();
//            this.btnColumnUp = new DevExpress.XtraBars.BarButtonItem();
//            this.btnColumnDown = new DevExpress.XtraBars.BarButtonItem();
//            this.btnAccept = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
//            this.btnColGrp = new DevExpress.XtraBars.BarButtonItem();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            this.grAll = new DevExpress.XtraGrid.GridControl();
//            this.viewAll = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colCheck = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rceCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.grSelected = new DevExpress.XtraGrid.GridControl();
//            this.viewSelected = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colOrder = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
//            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grAll)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewAll)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceCheck)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grSelected)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewSelected)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
//            this.SuspendLayout();
            // 
            // barManager1
            // 
//            this.barManager1.AllowCustomization = false;
//            this.barManager1.AllowMoveBarOnToolbar = false;
//            this.barManager1.AllowQuickCustomization = false;
//            this.barManager1.AllowShowToolbarsPopup = false;
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.bar2});
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.btnAccept,
//            this.btnCancel,
//            this.btnCheckAll,
//            this.btnClearAll,
//            this.btnColumnUp,
//            this.btnColumnDown,
//            this.btnCheckSelected,
//            this.btnClearSelected,
//            this.btnMoveBandLeft,
//            this.btnMoveBandRight,
//            this.btnShowMerge,
//            this.btnInSelOrdr,
//            this.btnColGrp});
//            this.barManager1.MaxItemId = 21;
            // 
            // bar2
            // 
//            this.bar2.BarName = "Пользовательская 2";
//            this.bar2.DockCol = 0;
//            this.bar2.DockRow = 0;
//            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.bar2.FloatLocation = new System.Drawing.Point(440, 536);
//            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCheckAll),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnClearAll),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCheckSelected),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnClearSelected),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnShowMerge),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnMoveBandLeft, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnMoveBandRight),
//            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnInSelOrdr, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.Standard),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnColumnUp),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnColumnDown),
//            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAccept, "", true, true, true, 0, null, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCancel),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnColGrp, true)});
//            this.bar2.OptionsBar.AllowQuickCustomization = false;
//            this.bar2.OptionsBar.AutoPopupMode = DevExpress.XtraBars.BarAutoPopupMode.All;
//            this.bar2.OptionsBar.DisableCustomization = true;
//            this.bar2.OptionsBar.DrawBorder = false;
//            this.bar2.OptionsBar.UseWholeRow = true;
//            this.bar2.Text = "Пользовательская 2";
            // 
            // btnCheckAll
            // 
//            this.btnCheckAll.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.btnCheckAll.Caption = "Добавить все";
//            this.btnCheckAll.Id = 5;
//            this.btnCheckAll.Name = "btnCheckAll";
//            this.btnCheckAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCheckAll_ItemClick);
            // 
            // btnClearAll
            // 
//            this.btnClearAll.Caption = "Убрать все";
//            this.btnClearAll.Id = 7;
//            this.btnClearAll.Name = "btnClearAll";
//            this.btnClearAll.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClearAll_ItemClick);
            // 
            // btnCheckSelected
            // 
//            this.btnCheckSelected.Caption = "Добавить выделенные";
//            this.btnCheckSelected.Id = 11;
//            this.btnCheckSelected.Name = "btnCheckSelected";
//            this.btnCheckSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCheckSelected_ItemClick);
            // 
            // btnClearSelected
            // 
//            this.btnClearSelected.Caption = "Убрать выделенные";
//            this.btnClearSelected.Id = 12;
//            this.btnClearSelected.Name = "btnClearSelected";
//            this.btnClearSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClearSelected_ItemClick);
            // 
            // btnShowMerge
            // 
//            this.btnShowMerge.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnShowMerge.ButtonStyle = DevExpress.XtraBars.BarButtonStyle.Check;
//            this.btnShowMerge.Caption = "Режим просмотра";
//            this.btnShowMerge.Glyph = ((System.Drawing.Image)(resources.GetObject("btnShowMerge.Glyph")));
//            this.btnShowMerge.Id = 17;
//            this.btnShowMerge.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnShowMerge.LargeGlyph")));
//            this.btnShowMerge.Name = "btnShowMerge";
//            this.btnShowMerge.DownChanged += new DevExpress.XtraBars.ItemClickEventHandler(this.btnShowMode_DownChanged);
            // 
            // btnMoveBandLeft
            // 
//            this.btnMoveBandLeft.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnMoveBandLeft.Caption = "Переместить группу влево";
//            this.btnMoveBandLeft.Glyph = ((System.Drawing.Image)(resources.GetObject("btnMoveBandLeft.Glyph")));
//            this.btnMoveBandLeft.Id = 13;
//            this.btnMoveBandLeft.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnMoveBandLeft.LargeGlyph")));
//            this.btnMoveBandLeft.Name = "btnMoveBandLeft";
//            this.btnMoveBandLeft.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnMoveBandLeft_ItemClick);
            // 
            // btnMoveBandRight
            // 
//            this.btnMoveBandRight.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnMoveBandRight.Caption = "Переместить группу вправо";
//            this.btnMoveBandRight.Glyph = ((System.Drawing.Image)(resources.GetObject("btnMoveBandRight.Glyph")));
//            this.btnMoveBandRight.Id = 14;
//            this.btnMoveBandRight.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnMoveBandRight.LargeGlyph")));
//            this.btnMoveBandRight.Name = "btnMoveBandRight";
//            this.btnMoveBandRight.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnMoveBandRight_ItemClick);
            // 
            // btnInSelOrdr
            // 
//            this.btnInSelOrdr.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnInSelOrdr.Caption = "Расположить в порядке выделения";
//            this.btnInSelOrdr.Glyph = ((System.Drawing.Image)(resources.GetObject("btnInSelOrdr.Glyph")));
//            this.btnInSelOrdr.Id = 19;
//            this.btnInSelOrdr.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnInSelOrdr.LargeGlyph")));
//            this.btnInSelOrdr.Name = "btnInSelOrdr";
//            this.btnInSelOrdr.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnInSelOrdr_ItemClick);
            // 
            // btnColumnUp
            // 
//            this.btnColumnUp.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnColumnUp.Caption = "Вверх";
//            this.btnColumnUp.Glyph = ((System.Drawing.Image)(resources.GetObject("btnColumnUp.Glyph")));
//            this.btnColumnUp.Id = 8;
//            this.btnColumnUp.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnColumnUp.LargeGlyph")));
//            this.btnColumnUp.Name = "btnColumnUp";
//            this.btnColumnUp.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnColumnUp_ItemClick);
            // 
            // btnColumnDown
            // 
//            this.btnColumnDown.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnColumnDown.Caption = "Вниз";
//            this.btnColumnDown.Glyph = ((System.Drawing.Image)(resources.GetObject("btnColumnDown.Glyph")));
//            this.btnColumnDown.Id = 9;
//            this.btnColumnDown.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnColumnDown.LargeGlyph")));
//            this.btnColumnDown.Name = "btnColumnDown";
//            this.btnColumnDown.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnColumnDown_ItemClick);
            // 
            // btnAccept
            // 
//            this.btnAccept.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnAccept.Caption = "Готово";
//            this.btnAccept.Glyph = global::sql.builder.Properties.Resources.Ok_16;
//            this.btnAccept.Id = 3;
//            this.btnAccept.Name = "btnAccept";
//            this.btnAccept.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAccept_ItemClick);
            // 
            // btnCancel
            // 
//            this.btnCancel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnCancel.Caption = "Отмена";
//            this.btnCancel.Id = 4;
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // btnColGrp
            // 
//            this.btnColGrp.Caption = "Группировка в колонках";
//            this.btnColGrp.Id = 20;
//            this.btnColGrp.Name = "btnColGrp";
//            this.btnColGrp.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(943, 0);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 471);
//            this.barDockControlBottom.Size = new System.Drawing.Size(943, 31);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 471);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(943, 0);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 471);
            // 
            // grAll
            // 
//            this.grAll.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.grAll.Location = new System.Drawing.Point(0, 0);
//            this.grAll.MainView = this.viewAll;
//            this.grAll.MenuManager = this.barManager1;
//            this.grAll.MinimumSize = new System.Drawing.Size(200, 200);
//            this.grAll.Name = "grAll";
//            this.grAll.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rceCheck});
//            this.grAll.Size = new System.Drawing.Size(438, 471);
//            this.grAll.TabIndex = 7;
//            this.grAll.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewAll,
//            this.gridView1});
            // 
            // viewAll
            // 
//            this.viewAll.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colCheck});
//            this.viewAll.GridControl = this.grAll;
//            this.viewAll.Name = "viewAll";
//            this.viewAll.OptionsBehavior.AutoPopulateColumns = false;
//            this.viewAll.OptionsCustomization.AllowColumnMoving = false;
//            this.viewAll.OptionsCustomization.AllowGroup = false;
//            this.viewAll.OptionsCustomization.AllowQuickHideColumns = false;
//            this.viewAll.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.viewAll.OptionsSelection.MultiSelect = true;
//            this.viewAll.OptionsView.ShowAutoFilterRow = true;
//            this.viewAll.OptionsView.ShowGroupPanel = false;
//            this.viewAll.OptionsView.ShowViewCaption = true;
//            this.viewAll.ViewCaption = "Доступные";
//            this.viewAll.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.view_CustomDrawRowIndicator);
//            this.viewAll.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.viewAll_CustomDrawCell);
//            this.viewAll.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.viewAll_SelectionChanged);
//            this.viewAll.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.viewAll_FocusedRowChanged);
            // 
            // colCheck
            // 
//            this.colCheck.AppearanceHeader.Options.UseTextOptions = true;
//            this.colCheck.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colCheck.Caption = "Выбор";
//            this.colCheck.ColumnEdit = this.rceCheck;
//            this.colCheck.FieldName = "check";
//            this.colCheck.Name = "colCheck";
//            this.colCheck.OptionsColumn.AllowSize = false;
//            this.colCheck.OptionsColumn.FixedWidth = true;
//            this.colCheck.Visible = true;
//            this.colCheck.VisibleIndex = 0;
//            this.colCheck.Width = 50;
            // 
            // rceCheck
            // 
//            this.rceCheck.AutoHeight = false;
//            this.rceCheck.Name = "rceCheck";
//            this.rceCheck.ValueUnchecked = null;
//            this.rceCheck.EditValueChanged += new System.EventHandler(this.rceCheck_EditValueChanged);
            // 
            // gridView1
            // 
//            this.gridView1.GridControl = this.grAll;
//            this.gridView1.Name = "gridView1";
            // 
            // grSelected
            // 
//            this.grSelected.Dock = System.Windows.Forms.DockStyle.Right;
//            this.grSelected.Location = new System.Drawing.Point(443, 0);
//            this.grSelected.MainView = this.viewSelected;
//            this.grSelected.MenuManager = this.barManager1;
//            this.grSelected.MinimumSize = new System.Drawing.Size(200, 200);
//            this.grSelected.Name = "grSelected";
//            this.grSelected.Size = new System.Drawing.Size(500, 471);
//            this.grSelected.TabIndex = 8;
//            this.grSelected.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewSelected,
//            this.gridView2});
            // 
            // viewSelected
            // 
//            this.viewSelected.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colOrder});
//            this.viewSelected.GridControl = this.grSelected;
//            this.viewSelected.Name = "viewSelected";
//            this.viewSelected.OptionsBehavior.AutoPopulateColumns = false;
//            this.viewSelected.OptionsBehavior.Editable = false;
//            this.viewSelected.OptionsCustomization.AllowColumnMoving = false;
//            this.viewSelected.OptionsCustomization.AllowFilter = false;
//            this.viewSelected.OptionsCustomization.AllowGroup = false;
//            this.viewSelected.OptionsCustomization.AllowQuickHideColumns = false;
//            this.viewSelected.OptionsCustomization.AllowSort = false;
//            this.viewSelected.OptionsSelection.MultiSelect = true;
//            this.viewSelected.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
//            this.viewSelected.OptionsView.ShowAutoFilterRow = true;
//            this.viewSelected.OptionsView.ShowGroupPanel = false;
//            this.viewSelected.OptionsView.ShowViewCaption = true;
//            this.viewSelected.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
//            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colOrder, DevExpress.Data.ColumnSortOrder.Ascending)});
//            this.viewSelected.ViewCaption = "Выбранные";
//            this.viewSelected.CellMerge += new DevExpress.XtraGrid.Views.Grid.CellMergeEventHandler(this.viewSelected_CellMerge);
//            this.viewSelected.CustomDrawRowIndicator += new DevExpress.XtraGrid.Views.Grid.RowIndicatorCustomDrawEventHandler(this.view_CustomDrawRowIndicator);
//            this.viewSelected.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.viewSelected_CustomDrawCell);
//            this.viewSelected.SelectionChanged += new DevExpress.Data.SelectionChangedEventHandler(this.viewSelected_SelectionChanged);
            // 
            // colOrder
            // 
//            this.colOrder.Caption = "Порядок";
//            this.colOrder.FieldName = "ord";
//            this.colOrder.Name = "colOrder";
//            this.colOrder.OptionsColumn.AllowEdit = false;
//            this.colOrder.Visible = true;
//            this.colOrder.VisibleIndex = 0;
            // 
            // gridView2
            // 
//            this.gridView2.GridControl = this.grSelected;
//            this.gridView2.Name = "gridView2";
            // 
            // splitterControl1
            // 
//            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Right;
//            this.splitterControl1.Location = new System.Drawing.Point(438, 0);
//            this.splitterControl1.Name = "splitterControl1";
//            this.splitterControl1.Size = new System.Drawing.Size(5, 471);
//            this.splitterControl1.TabIndex = 13;
//            this.splitterControl1.TabStop = false;
            // 
            // barButtonItem1
            // 
//            this.barButtonItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.barButtonItem1.Caption = "Готово";
//            this.barButtonItem1.Glyph = global::sql.builder.Properties.Resources.Ok_16;
//            this.barButtonItem1.Id = 3;
//            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
//            this.barButtonItem2.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.barButtonItem2.Caption = "Вверх";
//            this.barButtonItem2.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.Glyph")));
//            this.barButtonItem2.Id = 8;
//            this.barButtonItem2.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem2.LargeGlyph")));
//            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // frmColumnsEditorNew
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(943, 502);
//            this.Controls.Add(this.grAll);
//            this.Controls.Add(this.splitterControl1);
//            this.Controls.Add(this.grSelected);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.DoubleBuffered = true;
//            this.Name = "frmColumnsEditorNew";
//            this.Text = "Настройка колонок";
//            this.UserSettings.SaveFormSize = true;
//            this.Load += new System.EventHandler(this.frmColumnsEditorNew_Load);
//            this.ResizeEnd += new System.EventHandler(this.frmColumnsEditorNew_ResizeEnd);
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grAll)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewAll)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceCheck)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grSelected)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewSelected)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraBars.Bar bar2;
//        private DevExpress.XtraBars.BarButtonItem btnAccept;
//        private DevExpress.XtraBars.BarButtonItem btnCancel;
//        private DevExpress.XtraBars.BarLargeButtonItem btnCheckAll;
//        private DevExpress.XtraBars.BarButtonItem btnClearAll;
//        private DevExpress.XtraBars.BarButtonItem btnColumnUp;
//        private DevExpress.XtraBars.BarButtonItem btnColumnDown;
//        private DevExpress.XtraBars.BarButtonItem btnCheckSelected;
//        private DevExpress.XtraBars.BarButtonItem btnClearSelected;
//        private DevExpress.XtraBars.BarButtonItem btnMoveBandLeft;
//        private DevExpress.XtraBars.BarButtonItem btnMoveBandRight;
//        private DevExpress.XtraBars.BarButtonItem btnShowMerge;
//        private DevExpress.XtraGrid.GridControl grAll;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewAll;
//        private DevExpress.XtraGrid.Columns.GridColumn colCheck;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceCheck;
//        private DevExpress.XtraEditors.SplitterControl splitterControl1;
//        private DevExpress.XtraGrid.GridControl grSelected;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewSelected;
//        private DevExpress.XtraGrid.Columns.GridColumn colOrder;
//        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
//        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
//        private DevExpress.XtraBars.BarButtonItem btnInSelOrdr;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
//        private DevExpress.XtraBars.BarButtonItem btnColGrp;
//    }
//}
