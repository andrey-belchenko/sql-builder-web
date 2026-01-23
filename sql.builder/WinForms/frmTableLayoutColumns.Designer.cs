//namespace sql.builder.WinForms
//{
//    partial class frmTableLayoutColumns
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmTableLayoutColumns));
//            this.grColsSettings = new DevExpress.XtraGrid.GridControl();
//            this.viewColsSettings = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colColIndex = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colType = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rleType = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
//            this.colValue = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rseValue = new DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit();
//            this.barManager1 = new DevExpress.XtraBars.BarManager();
//            this.bar3 = new DevExpress.XtraBars.Bar();
//            this.btnAccept = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
//            this.bar1 = new DevExpress.XtraBars.Bar();
//            this.btnAddCol = new DevExpress.XtraBars.BarButtonItem();
//            this.btnRemoveCol = new DevExpress.XtraBars.BarButtonItem();
//            this.btnSetDefault = new DevExpress.XtraBars.BarButtonItem();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            ((System.ComponentModel.ISupportInitialize)(this.grColsSettings)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewColsSettings)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleType)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rseValue)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            this.SuspendLayout();
            // 
            // grColsSettings
            // 
//            this.grColsSettings.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.grColsSettings.Location = new System.Drawing.Point(0, 29);
//            this.grColsSettings.MainView = this.viewColsSettings;
//            this.grColsSettings.Name = "grColsSettings";
//            this.grColsSettings.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rseValue,
//            this.rleType});
//            this.grColsSettings.Size = new System.Drawing.Size(230, 263);
//            this.grColsSettings.TabIndex = 0;
//            this.grColsSettings.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewColsSettings});
            // 
            // viewColsSettings
            // 
//            this.viewColsSettings.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colColIndex,
//            this.colType,
//            this.colValue});
//            this.viewColsSettings.GridControl = this.grColsSettings;
//            this.viewColsSettings.Name = "viewColsSettings";
//            this.viewColsSettings.OptionsView.ColumnAutoWidth = false;
//            this.viewColsSettings.OptionsView.ColumnHeaderAutoHeight = DevExpress.Utils.DefaultBoolean.True;
//            this.viewColsSettings.OptionsView.ShowGroupPanel = false;
//            this.viewColsSettings.CellValueChanged += new DevExpress.XtraGrid.Views.Base.CellValueChangedEventHandler(this.viewColsSettings_CellValueChanged);
            // 
            // colColIndex
            // 
//            this.colColIndex.AppearanceHeader.Options.UseTextOptions = true;
//            this.colColIndex.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colColIndex.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colColIndex.Caption = "Позиция";
//            this.colColIndex.FieldName = "index";
//            this.colColIndex.Name = "colColIndex";
//            this.colColIndex.OptionsColumn.FixedWidth = true;
//            this.colColIndex.Visible = true;
//            this.colColIndex.VisibleIndex = 0;
//            this.colColIndex.Width = 60;
            // 
            // colType
            // 
//            this.colType.AppearanceHeader.Options.UseTextOptions = true;
//            this.colType.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colType.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colType.Caption = "Тип";
//            this.colType.ColumnEdit = this.rleType;
//            this.colType.FieldName = "type";
//            this.colType.Name = "colType";
//            this.colType.OptionsColumn.FixedWidth = true;
//            this.colType.Visible = true;
//            this.colType.VisibleIndex = 1;
            // 
            // rleType
            // 
//            this.rleType.AutoHeight = false;
//            this.rleType.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rleType.DisplayMember = "name";
//            this.rleType.Name = "rleType";
//            this.rleType.ValueMember = "name";
            // 
            // colValue
            // 
//            this.colValue.AppearanceHeader.Options.UseTextOptions = true;
//            this.colValue.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colValue.AppearanceHeader.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.colValue.Caption = "Ширина или процент";
//            this.colValue.ColumnEdit = this.rseValue;
//            this.colValue.FieldName = "value";
//            this.colValue.Name = "colValue";
//            this.colValue.OptionsColumn.FixedWidth = true;
//            this.colValue.Visible = true;
//            this.colValue.VisibleIndex = 2;
            // 
            // rseValue
            // 
//            this.rseValue.AutoHeight = false;
//            this.rseValue.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rseValue.Name = "rseValue";
            // 
            // barManager1
            // 
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.bar3,
//            this.bar1});
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.btnAddCol,
//            this.btnRemoveCol,
//            this.btnSetDefault,
//            this.btnAccept,
//            this.btnCancel});
//            this.barManager1.MaxItemId = 5;
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
//            this.btnAccept.Caption = "Готово";
//            this.btnAccept.Glyph = ((System.Drawing.Image)(resources.GetObject("btnAccept.Glyph")));
//            this.btnAccept.Id = 3;
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
//            this.btnCancel.Id = 4;
//            this.btnCancel.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnCancel.LargeGlyph")));
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // bar1
            // 
//            this.bar1.BarName = "Custom 3";
//            this.bar1.DockCol = 0;
//            this.bar1.DockRow = 0;
//            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnAddCol),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnRemoveCol),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnSetDefault, true)});
//            this.bar1.OptionsBar.AllowQuickCustomization = false;
//            this.bar1.OptionsBar.DrawBorder = false;
//            this.bar1.OptionsBar.DrawDragBorder = false;
//            this.bar1.OptionsBar.UseWholeRow = true;
//            this.bar1.Text = "Custom 3";
            // 
            // btnAddCol
            // 
//            this.btnAddCol.Caption = "Добавить";
//            this.btnAddCol.Id = 0;
//            this.btnAddCol.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnAddCol.LargeGlyph")));
//            this.btnAddCol.Name = "btnAddCol";
//            this.btnAddCol.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAddCol_ItemClick);
            // 
            // btnRemoveCol
            // 
//            this.btnRemoveCol.Caption = "Удалить";
//            this.btnRemoveCol.Id = 1;
//            this.btnRemoveCol.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("btnRemoveCol.LargeGlyph")));
//            this.btnRemoveCol.Name = "btnRemoveCol";
//            this.btnRemoveCol.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRemoveCol_ItemClick);
            // 
            // btnSetDefault
            // 
//            this.btnSetDefault.Caption = "По умолчанию";
//            this.btnSetDefault.Id = 2;
//            this.btnSetDefault.Name = "btnSetDefault";
//            this.btnSetDefault.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnSetDefault_ItemClick);
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(230, 29);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 292);
//            this.barDockControlBottom.Size = new System.Drawing.Size(230, 27);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 263);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(230, 29);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 263);
            // 
            // frmTableLayoutColumns
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(230, 319);
//            this.Controls.Add(this.grColsSettings);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Name = "frmTableLayoutColumns";
//            this.Text = "Настройка колонок";
//            this.UserSettings.SaveFormSize = true;
//            ((System.ComponentModel.ISupportInitialize)(this.grColsSettings)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewColsSettings)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleType)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rseValue)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraGrid.GridControl grColsSettings;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewColsSettings;
//        private DevExpress.XtraGrid.Columns.GridColumn colColIndex;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.Bar bar3;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraGrid.Columns.GridColumn colType;
//        private DevExpress.XtraGrid.Columns.GridColumn colValue;
//        private DevExpress.XtraBars.BarButtonItem btnAccept;
//        private DevExpress.XtraBars.BarButtonItem btnCancel;
//        private DevExpress.XtraBars.Bar bar1;
//        private DevExpress.XtraBars.BarButtonItem btnAddCol;
//        private DevExpress.XtraBars.BarButtonItem btnRemoveCol;
//        private DevExpress.XtraBars.BarButtonItem btnSetDefault;
//        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rleType;
//        private DevExpress.XtraEditors.Repository.RepositoryItemSpinEdit rseValue;
//    }
//}
