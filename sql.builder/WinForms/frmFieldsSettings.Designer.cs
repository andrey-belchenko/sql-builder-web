//using DevExpress.XtraEditors.Repository;

//namespace sql.builder.WinForms
//{
//    partial class frmFieldsSettings
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFieldsSettings));
//            this.tl = new DevExpress.XtraTreeList.TreeList();
//            this.treeListBand1 = new DevExpress.XtraTreeList.Columns.TreeListBand();
//            this.colTitle = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.colWidthPerc = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.colWidthFixed = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.treeListBand2 = new DevExpress.XtraTreeList.Columns.TreeListBand();
//            this.colTextVisible = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.colTextLocation = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.treeListBand3 = new DevExpress.XtraTreeList.Columns.TreeListBand();
//            this.colLayoutMode = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.colColsCount = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.rceCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.rceTextVisible = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.rseWidthFixed = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
//            this.rleTextLocation = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
//            this.rleLayoutMode = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
//            this.rseWidthPerc = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
//            this.rteEmpty = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.rbeCols = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
//            this.imageCollection1 = new DevExpress.Utils.ImageCollection(this.components);
//            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
//            this.bar3 = new DevExpress.XtraBars.Bar();
//            this.btnAccept = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            this.btnSelectAll2 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnUnselectAll2 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnSelect2 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnUnselect2 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCalculateWidthPerc = new DevExpress.XtraBars.BarButtonItem();
//            this.btnClearWidthPerc = new DevExpress.XtraBars.BarButtonItem();
//            this.btnHideText = new DevExpress.XtraBars.BarButtonItem();
//            this.btnShowText = new DevExpress.XtraBars.BarButtonItem();
//            this.btnTextLeftLocation = new DevExpress.XtraBars.BarButtonItem();
//            this.btnTextTopLocation = new DevExpress.XtraBars.BarButtonItem();
//            this.btnClearWidthFixed = new DevExpress.XtraBars.BarButtonItem();
//            this.menu = new DevExpress.XtraBars.PopupMenu(this.components);
//            ((System.ComponentModel.ISupportInitialize)(this.tl)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceCheck)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceTextVisible)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rseWidthFixed)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleTextLocation)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleLayoutMode)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rseWidthPerc)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteEmpty)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbeCols)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.menu)).BeginInit();
//            this.SuspendLayout();
            // 
            // tl
            // 
//            this.tl.Bands.AddRange(new DevExpress.XtraTreeList.Columns.TreeListBand[] {
//            this.treeListBand1,
//            this.treeListBand2,
//            this.treeListBand3});
//            this.tl.ColumnPanelRowHeight = 34;
//            this.tl.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
//            this.colTitle,
//            this.colTextVisible,
//            this.colTextLocation,
//            this.colLayoutMode,
//            this.colWidthPerc,
//            this.colWidthFixed,
//            this.colColsCount});
//            this.tl.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tl.ImageIndexFieldName = "image_index";
//            this.tl.KeyFieldName = "key";
//            this.tl.Location = new System.Drawing.Point(0, 0);
//            this.tl.Name = "tl";
//            this.tl.OptionsBehavior.EnableFiltering = true;
//            this.tl.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.tl.OptionsSelection.MultiSelect = true;
//            this.tl.OptionsView.ShowAutoFilterRow = true;
//            this.tl.OptionsView.ShowCheckBoxes = true;
//            this.tl.ParentFieldName = "parent";
//            this.tl.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rceCheck,
//            this.rceTextVisible,
//            this.rseWidthFixed,
//            this.rleTextLocation,
//            this.rleLayoutMode,
//            this.rseWidthPerc,
//            this.rteEmpty,
//            this.rbeCols});
//            this.tl.SelectImageList = this.imageCollection1;
//            this.tl.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowAlways;
//            this.tl.Size = new System.Drawing.Size(573, 656);
//            this.tl.TabIndex = 0;
//            this.tl.CustomNodeCellEdit += new DevExpress.XtraTreeList.GetCustomNodeCellEditEventHandler(this.tl_CustomNodeCellEdit);
//            this.tl.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tl_MouseClick);
            // 
            // treeListBand1
            // 
//            this.treeListBand1.AppearanceHeader.Options.UseTextOptions = true;
//            this.treeListBand1.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.treeListBand1.Caption = "Общее";
//            this.treeListBand1.Columns.Add(this.colTitle);
//            this.treeListBand1.Columns.Add(this.colWidthPerc);
//            this.treeListBand1.Columns.Add(this.colWidthFixed);
//            this.treeListBand1.MinWidth = 51;
//            this.treeListBand1.Name = "treeListBand1";
//            this.treeListBand1.Width = 311;
            // 
            // colTitle
            // 
//            this.colTitle.AppearanceHeader.Options.UseTextOptions = true;
//            this.colTitle.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colTitle.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colTitle.Caption = "Элемент";
//            this.colTitle.FieldName = "title";
//            this.colTitle.MinWidth = 49;
//            this.colTitle.Name = "colTitle";
//            this.colTitle.OptionsColumn.AllowEdit = false;
//            this.colTitle.Visible = true;
//            this.colTitle.VisibleIndex = 0;
//            this.colTitle.Width = 240;
            // 
            // colWidthPerc
            // 
//            this.colWidthPerc.AppearanceHeader.Options.UseTextOptions = true;
//            this.colWidthPerc.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colWidthPerc.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colWidthPerc.Caption = "Относ. % ширины";
//            this.colWidthPerc.FieldName = "width_perc";
//            this.colWidthPerc.MinWidth = 49;
//            this.colWidthPerc.Name = "colWidthPerc";
//            this.colWidthPerc.OptionsColumn.FixedWidth = true;
//            this.colWidthPerc.Visible = true;
//            this.colWidthPerc.VisibleIndex = 1;
//            this.colWidthPerc.Width = 60;
            // 
            // colWidthFixed
            // 
//            this.colWidthFixed.AppearanceHeader.Options.UseTextOptions = true;
//            this.colWidthFixed.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colWidthFixed.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colWidthFixed.Caption = "Фиксированая ширина";
//            this.colWidthFixed.FieldName = "width_fixed";
//            this.colWidthFixed.Name = "colWidthFixed";
//            this.colWidthFixed.OptionsColumn.FixedWidth = true;
//            this.colWidthFixed.Visible = true;
//            this.colWidthFixed.VisibleIndex = 2;
//            this.colWidthFixed.Width = 60;
            // 
            // treeListBand2
            // 
//            this.treeListBand2.AppearanceHeader.Options.UseTextOptions = true;
//            this.treeListBand2.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.treeListBand2.Caption = "Поле";
//            this.treeListBand2.Columns.Add(this.colTextVisible);
//            this.treeListBand2.Columns.Add(this.colTextLocation);
//            this.treeListBand2.Name = "treeListBand2";
            // 
            // colTextVisible
            // 
//            this.colTextVisible.AppearanceHeader.Options.UseTextOptions = true;
//            this.colTextVisible.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colTextVisible.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colTextVisible.Caption = "Видимость текста";
//            this.colTextVisible.FieldName = "text_visible";
//            this.colTextVisible.Name = "colTextVisible";
//            this.colTextVisible.OptionsColumn.FixedWidth = true;
//            this.colTextVisible.Visible = true;
//            this.colTextVisible.VisibleIndex = 3;
//            this.colTextVisible.Width = 60;
            // 
            // colTextLocation
            // 
//            this.colTextLocation.AppearanceHeader.Options.UseTextOptions = true;
//            this.colTextLocation.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colTextLocation.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colTextLocation.Caption = "Позиция текста";
//            this.colTextLocation.FieldName = "text_location";
//            this.colTextLocation.Name = "colTextLocation";
//            this.colTextLocation.OptionsColumn.FixedWidth = true;
//            this.colTextLocation.Visible = true;
//            this.colTextLocation.VisibleIndex = 4;
//            this.colTextLocation.Width = 60;
            // 
            // treeListBand3
            // 
//            this.treeListBand3.AppearanceHeader.Options.UseTextOptions = true;
//            this.treeListBand3.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.treeListBand3.Caption = "Группа";
//            this.treeListBand3.Columns.Add(this.colLayoutMode);
//            this.treeListBand3.Columns.Add(this.colColsCount);
//            this.treeListBand3.Name = "treeListBand3";
            // 
            // colLayoutMode
            // 
//            this.colLayoutMode.AppearanceHeader.Options.UseTextOptions = true;
//            this.colLayoutMode.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colLayoutMode.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colLayoutMode.Caption = "Вид Layout";
//            this.colLayoutMode.FieldName = "layout_mode";
//            this.colLayoutMode.Name = "colLayoutMode";
//            this.colLayoutMode.OptionsColumn.FixedWidth = true;
//            this.colLayoutMode.Visible = true;
//            this.colLayoutMode.VisibleIndex = 5;
//            this.colLayoutMode.Width = 60;
            // 
            // colColsCount
            // 
//            this.colColsCount.AppearanceHeader.Options.UseTextOptions = true;
//            this.colColsCount.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colColsCount.AppearanceHeader.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.colColsCount.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colColsCount.Caption = "Кол-во колонок";
//            this.colColsCount.FieldName = "cols_count";
//            this.colColsCount.Name = "colColsCount";
//            this.colColsCount.OptionsColumn.FixedWidth = true;
//            this.colColsCount.Visible = true;
//            this.colColsCount.VisibleIndex = 6;
//            this.colColsCount.Width = 60;
            // 
            // rceCheck
            // 
//            this.rceCheck.AutoHeight = false;
//            this.rceCheck.Name = "rceCheck";
            // 
            // rceTextVisible
            // 
//            this.rceTextVisible.AutoHeight = false;
//            this.rceTextVisible.Name = "rceTextVisible";
            // 
            // rseWidthFixed
            // 
//            this.rseWidthFixed.AutoHeight = false;
//            this.rseWidthFixed.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rseWidthFixed.DisplayFormat.FormatString = "d";
//            this.rseWidthFixed.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
//            this.rseWidthFixed.EditFormat.FormatString = "d";
//            this.rseWidthFixed.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
//            this.rseWidthFixed.Increment = new decimal(new int[] {
//            5,
//            0,
//            0,
//            0});
//            this.rseWidthFixed.Mask.EditMask = "d";
//            this.rseWidthFixed.Name = "rseWidthFixed";
            // 
            // rleTextLocation
            // 
//            this.rleTextLocation.AutoHeight = false;
//            this.rleTextLocation.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rleTextLocation.DisplayMember = "name";
//            this.rleTextLocation.Name = "rleTextLocation";
//            this.rleTextLocation.ValueMember = "name";
            // 
            // rleLayoutMode
            // 
//            this.rleLayoutMode.AutoHeight = false;
//            this.rleLayoutMode.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rleLayoutMode.DisplayMember = "name";
//            this.rleLayoutMode.Name = "rleLayoutMode";
//            this.rleLayoutMode.ValueMember = "name";
            // 
            // rseWidthPerc
            // 
//            this.rseWidthPerc.AutoHeight = false;
//            this.rseWidthPerc.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rseWidthPerc.DisplayFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
//            this.rseWidthPerc.EditFormat.FormatType = DevExpress.Utils.FormatType.Numeric;
//            this.rseWidthPerc.Increment = new decimal(new int[] {
//            5,
//            0,
//            0,
//            0});
//            this.rseWidthPerc.Mask.EditMask = "P0";
//            this.rseWidthPerc.MaxValue = new decimal(new int[] {
//            100,
//            0,
//            0,
//            0});
//            this.rseWidthPerc.MinValue = new decimal(new int[] {
//            5,
//            0,
//            0,
//            0});
//            this.rseWidthPerc.Name = "rseWidthPerc";
            // 
            // rteEmpty
            // 
//            this.rteEmpty.AutoHeight = false;
//            this.rteEmpty.Name = "rteEmpty";
//            this.rteEmpty.ReadOnly = true;
            // 
            // rbeCols
            // 
//            this.rbeCols.AutoHeight = false;
//            this.rbeCols.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton()});
//            this.rbeCols.Name = "rbeCols";
//            this.rbeCols.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
//            this.rbeCols.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.rbeCols_ButtonClick);
            // 
            // imageCollection1
            // 
//            this.imageCollection1.ImageStream = ((DevExpress.Utils.ImageCollectionStreamer)(resources.GetObject("imageCollection1.ImageStream")));
//            this.imageCollection1.InsertGalleryImage("groupheader_16x16.png", "images/reports/groupheader_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/reports/groupheader_16x16.png"), 0);
//            this.imageCollection1.Images.SetKeyName(0, "groupheader_16x16.png");
//            this.imageCollection1.InsertGalleryImage("group_16x16.png", "images/actions/group_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/actions/group_16x16.png"), 1);
//            this.imageCollection1.Images.SetKeyName(1, "group_16x16.png");
//            this.imageCollection1.InsertGalleryImage("youtube_16x16.png", "images/media/youtube_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/media/youtube_16x16.png"), 2);
//            this.imageCollection1.Images.SetKeyName(2, "youtube_16x16.png");
//            this.imageCollection1.InsertGalleryImage("label_16x16.png", "images/toolbox%20items/label_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/toolbox%20items/label_16x16.png"), 3);
//            this.imageCollection1.Images.SetKeyName(3, "label_16x16.png");
//            this.imageCollection1.InsertGalleryImage("table_16x16.png", "images/toolbox%20items/table_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/toolbox%20items/table_16x16.png"), 4);
//            this.imageCollection1.Images.SetKeyName(4, "table_16x16.png");
//            this.imageCollection1.InsertGalleryImage("separator_16x16.png", "images/reports/separator_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/reports/separator_16x16.png"), 5);
//            this.imageCollection1.Images.SetKeyName(5, "separator_16x16.png");
//            this.imageCollection1.InsertGalleryImage("logical_16x16.png", "images/function%20library/logical_16x16.png", DevExpress.Images.ImageResourceCache.Default.GetImage("images/function%20library/logical_16x16.png"), 6);
//            this.imageCollection1.Images.SetKeyName(6, "logical_16x16.png");
            // 
            // barManager1
            // 
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.bar3});
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.btnSelectAll2,
//            this.btnUnselectAll2,
//            this.btnSelect2,
//            this.btnUnselect2,
//            this.btnCalculateWidthPerc,
//            this.btnAccept,
//            this.btnCancel,
//            this.btnClearWidthPerc,
//            this.btnHideText,
//            this.btnShowText,
//            this.btnTextLeftLocation,
//            this.btnTextTopLocation,
//            this.btnClearWidthFixed});
//            this.barManager1.MaxItemId = 17;
//            this.barManager1.StatusBar = this.bar3;
            // 
            // bar3
            // 
//            this.bar3.BarName = "Status bar";
//            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
//            this.bar3.DockCol = 0;
//            this.bar3.DockRow = 0;
//            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.bar3.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnAccept),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCancel)});
//            this.bar3.OptionsBar.AllowQuickCustomization = false;
//            this.bar3.OptionsBar.DrawDragBorder = false;
//            this.bar3.OptionsBar.UseWholeRow = true;
//            this.bar3.Text = "Status bar";
            // 
            // btnAccept
            // 
//            this.btnAccept.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnAccept.Caption = "Принять";
//            this.btnAccept.Glyph = ((System.Drawing.Image)(resources.GetObject("btnAccept.Glyph")));
//            this.btnAccept.Id = 9;
//            this.btnAccept.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnAccept.LargeGlyph")));
//            this.btnAccept.Name = "btnAccept";
//            this.btnAccept.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.btnAccept.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAccept_ItemClick);
            // 
            // btnCancel
            // 
//            this.btnCancel.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnCancel.Caption = "Отмена";
//            this.btnCancel.Glyph = ((System.Drawing.Image)(resources.GetObject("btnCancel.Glyph")));
//            this.btnCancel.Id = 10;
//            this.btnCancel.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnCancel.LargeGlyph")));
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(573, 0);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 656);
//            this.barDockControlBottom.Size = new System.Drawing.Size(573, 27);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 656);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(573, 0);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 656);
            // 
            // btnSelectAll2
            // 
//            this.btnSelectAll2.Caption = "Выбрать все";
//            this.btnSelectAll2.Id = 4;
//            this.btnSelectAll2.Name = "btnSelectAll2";
//            this.btnSelectAll2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSelectAll_ItemClick);
            // 
            // btnUnselectAll2
            // 
//            this.btnUnselectAll2.Caption = "Снять все";
//            this.btnUnselectAll2.Id = 5;
//            this.btnUnselectAll2.Name = "btnUnselectAll2";
//            this.btnUnselectAll2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUnselectAll_ItemClick);
            // 
            // btnSelect2
            // 
//            this.btnSelect2.Caption = "Выбрать выделенные";
//            this.btnSelect2.Id = 6;
//            this.btnSelect2.Name = "btnSelect2";
//            this.btnSelect2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSelect_ItemClick);
            // 
            // btnUnselect2
            // 
//            this.btnUnselect2.Caption = "Снять выделенные";
//            this.btnUnselect2.Id = 7;
//            this.btnUnselect2.Name = "btnUnselect2";
//            this.btnUnselect2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUnselect_ItemClick);
            // 
            // btnCalculateWidthPerc
            // 
//            this.btnCalculateWidthPerc.Caption = "Рассчитать относительную ширину";
//            this.btnCalculateWidthPerc.Id = 8;
//            this.btnCalculateWidthPerc.Name = "btnCalculateWidthPerc";
//            this.btnCalculateWidthPerc.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCalculateWidthPerc_ItemClick);
            // 
            // btnClearWidthPerc
            // 
//            this.btnClearWidthPerc.Caption = "Очистить относительную ширину";
//            this.btnClearWidthPerc.Id = 11;
//            this.btnClearWidthPerc.Name = "btnClearWidthPerc";
//            this.btnClearWidthPerc.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClearWidthPerc_ItemClick);
            // 
            // btnHideText
            // 
//            this.btnHideText.Caption = "Убрать текст";
//            this.btnHideText.Id = 12;
//            this.btnHideText.Name = "btnHideText";
//            this.btnHideText.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnHideText_ItemClick);
            // 
            // btnShowText
            // 
//            this.btnShowText.Caption = "Показать текст";
//            this.btnShowText.Id = 13;
//            this.btnShowText.Name = "btnShowText";
//            this.btnShowText.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnShowText_ItemClick);
            // 
            // btnTextLeftLocation
            // 
//            this.btnTextLeftLocation.Caption = "Текст слева";
//            this.btnTextLeftLocation.Id = 14;
//            this.btnTextLeftLocation.Name = "btnTextLeftLocation";
//            this.btnTextLeftLocation.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTextLeftLocation_ItemClick);
            // 
            // btnTextTopLocation
            // 
//            this.btnTextTopLocation.Caption = "Текст сверху";
//            this.btnTextTopLocation.Id = 15;
//            this.btnTextTopLocation.Name = "btnTextTopLocation";
//            this.btnTextTopLocation.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnTextTopLocation_ItemClick);
            // 
            // btnClearWidthFixed
            // 
//            this.btnClearWidthFixed.Caption = "Очистить фиксированную ширину";
//            this.btnClearWidthFixed.Id = 16;
//            this.btnClearWidthFixed.Name = "btnClearWidthFixed";
//            this.btnClearWidthFixed.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnClearWidthFixed_ItemClick);
            // 
            // menu
            // 
//            this.menu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnSelectAll2),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnUnselectAll2),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnSelect2, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnUnselect2),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCalculateWidthPerc, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnClearWidthPerc),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnClearWidthFixed),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnShowText, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnHideText),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnTextTopLocation, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnTextLeftLocation)});
//            this.menu.Manager = this.barManager1;
//            this.menu.Name = "menu";
            // 
            // frmFieldsSettings
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(573, 683);
//            this.Controls.Add(this.tl);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Name = "frmFieldsSettings";
//            this.Text = "Выбор настраиваемых полей";
//            this.UserSettings.SaveFormSize = true;
//            this.WindowState = System.Windows.Forms.FormWindowState.Normal;
//            ((System.ComponentModel.ISupportInitialize)(this.tl)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceCheck)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceTextVisible)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rseWidthFixed)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleTextLocation)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleLayoutMode)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rseWidthPerc)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteEmpty)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbeCols)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.imageCollection1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.menu)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraTreeList.TreeList tl;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceCheck;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colTitle;
//        private DevExpress.Utils.ImageCollection imageCollection1;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.Bar bar3;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraBars.BarButtonItem btnSelectAll2;
//        private DevExpress.XtraBars.BarButtonItem btnUnselectAll2;
//        private DevExpress.XtraBars.BarButtonItem btnSelect2;
//        private DevExpress.XtraBars.BarButtonItem btnUnselect2;
//        private DevExpress.XtraBars.BarButtonItem btnCalculateWidthPerc;
//        private DevExpress.XtraBars.PopupMenu menu;
//        private DevExpress.XtraBars.BarButtonItem btnAccept;
//        private DevExpress.XtraBars.BarButtonItem btnCancel;
//        private DevExpress.XtraBars.BarButtonItem btnClearWidthPerc;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colTextVisible;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceTextVisible;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colWidthFixed;
//        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rseWidthFixed;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colTextLocation;
//        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rleTextLocation;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colWidthPerc;
//        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rseWidthPerc;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colLayoutMode;
//        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rleLayoutMode;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteEmpty;
//        private DevExpress.XtraBars.BarButtonItem btnHideText;
//        private DevExpress.XtraBars.BarButtonItem btnShowText;
//        private DevExpress.XtraBars.BarButtonItem btnTextLeftLocation;
//        private DevExpress.XtraBars.BarButtonItem btnTextTopLocation;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colColsCount;
//        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit rbeCols;
//        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand1;
//        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand2;
//        private DevExpress.XtraTreeList.Columns.TreeListBand treeListBand3;
//        private DevExpress.XtraBars.BarButtonItem btnClearWidthFixed;
//    }
//}
