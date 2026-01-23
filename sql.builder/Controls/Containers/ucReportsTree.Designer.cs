//namespace sql.builder.Controls.Containers
//{
//    internal partial class ucReportsTree
//    {
//        /// <summary> 
//        /// Требуется переменная конструктора.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        /// <summary> 
//        /// Освободить все используемые ресурсы.
//        /// </summary>
//        /// <param name="disposing">истинно, если управляемый ресурс должен быть удален; иначе ложно.</param>
//        protected override void Dispose(bool disposing)
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucReportsTree));
//            this.tlReports = new DevExpress.XtraTreeList.TreeList();
//            this.tcolItem = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.rmeItem = new DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit();
//            this.rreItem = new DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit();
//            this.ic = new DevExpress.Utils.ImageCollection();
//            this.popupMenu = new DevExpress.XtraBars.PopupMenu();
//            this.btnCopyFullName = new DevExpress.XtraBars.BarButtonItem();
//            this.barManager1 = new DevExpress.XtraBars.BarManager();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            ((System.ComponentModel.ISupportInitialize)(this.tlReports)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeItem)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rreItem)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            this.SuspendLayout();
            // 
            // tlReports
            // 
//            this.tlReports.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
//            this.tcolItem});
//            this.tlReports.Cursor = System.Windows.Forms.Cursors.Default;
//            this.tlReports.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tlReports.ImageIndexFieldName = "image_id";
//            this.tlReports.KeyFieldName = "name";
//            this.tlReports.Location = new System.Drawing.Point(0, 0);
//            this.tlReports.Name = "tlReports";
//            this.tlReports.OptionsBehavior.EnableFiltering = true;
//            this.tlReports.OptionsCustomization.AllowQuickHideColumns = false;
//            this.tlReports.OptionsFilter.FilterMode = DevExpress.XtraTreeList.FilterMode.Extended;
//            this.tlReports.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.tlReports.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None;
//            this.tlReports.OptionsView.ShowAutoFilterRow = true;
//            this.tlReports.OptionsView.ShowColumns = false;
//            this.tlReports.OptionsView.ShowPreview = true;
//            this.tlReports.ParentFieldName = "parent";
//            this.tlReports.PreviewLineCount = 2;
//            this.tlReports.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rmeItem,
//            this.rreItem});
//            this.tlReports.SelectImageList = this.ic;
//            this.tlReports.Size = new System.Drawing.Size(372, 581);
//            this.tlReports.TabIndex = 0;
//            this.tlReports.GetNodeDisplayValue += new DevExpress.XtraTreeList.GetNodeDisplayValueEventHandler(this.tlReports_GetNodeDisplayValue);
//            this.tlReports.BeforeFocusNode += new DevExpress.XtraTreeList.BeforeFocusNodeEventHandler(this.tlReports_BeforeFocusNode);
//            this.tlReports.AfterExpand += new DevExpress.XtraTreeList.NodeEventHandler(this.tlReports_AfterExpand);
//            this.tlReports.AfterCollapse += new DevExpress.XtraTreeList.NodeEventHandler(this.tlReports_AfterCollapse);
//            this.tlReports.FocusedNodeChanged += new DevExpress.XtraTreeList.FocusedNodeChangedEventHandler(this.tlReports_FocusedNodeChanged);
//            this.tlReports.HiddenEditor += new System.EventHandler(this.tlReports_HiddenEditor);
//            this.tlReports.CustomDrawNodeCell += new DevExpress.XtraTreeList.CustomDrawNodeCellEventHandler(this.tlReports_CustomDrawNodeCell);
//            this.tlReports.Click += new System.EventHandler(this.tlReports_Click);
//            this.tlReports.DoubleClick += new System.EventHandler(this.tlReports_DoubleClick);
//            this.tlReports.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tlReports_KeyDown);
//            this.tlReports.MouseClick += new System.Windows.Forms.MouseEventHandler(this.tlReports_MouseClick);
            // 
            // tcolItem
            // 
//            this.tcolItem.Caption = "Отчёт";
//            this.tcolItem.ColumnEdit = this.rmeItem;
//            this.tcolItem.FieldName = "title";
//            this.tcolItem.FilterMode = DevExpress.XtraGrid.ColumnFilterMode.DisplayText;
//            this.tcolItem.MinWidth = 34;
//            this.tcolItem.Name = "tcolItem";
//            this.tcolItem.OptionsColumn.AllowEdit = false;
//            this.tcolItem.Visible = true;
//            this.tcolItem.VisibleIndex = 0;
            // 
            // rmeItem
            // 
//            this.rmeItem.Name = "rmeItem";
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
            // popupMenu
            // 
//            this.popupMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCopyFullName)});
//            this.popupMenu.Manager = this.barManager1;
//            this.popupMenu.Name = "popupMenu";
            // 
            // btnCopyFullName
            // 
//            this.btnCopyFullName.Caption = "Копировать полное имя";
//            this.btnCopyFullName.Id = 0;
//            this.btnCopyFullName.Name = "btnCopyFullName";
//            this.btnCopyFullName.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCopyFullName_ItemClick);
            // 
            // barManager1
            // 
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.btnCopyFullName});
//            this.barManager1.MaxItemId = 1;
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(372, 0);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 581);
//            this.barDockControlBottom.Size = new System.Drawing.Size(372, 0);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 581);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(372, 0);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 581);
            // 
            // ucReportsTree
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.tlReports);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Name = "ucReportsTree";
//            this.Size = new System.Drawing.Size(372, 581);
//            this.Load += new System.EventHandler(this.ucReportsTree_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.tlReports)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rmeItem)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rreItem)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ic)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraTreeList.TreeList tlReports;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn tcolItem;
//        private DevExpress.XtraEditors.Repository.RepositoryItemMemoEdit rmeItem;
//        private DevExpress.Utils.ImageCollection ic;
//        private DevExpress.XtraEditors.Repository.RepositoryItemRichTextEdit rreItem;
//        private DevExpress.XtraBars.PopupMenu popupMenu;
//        private DevExpress.XtraBars.BarButtonItem btnCopyFullName;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//    }
//}
