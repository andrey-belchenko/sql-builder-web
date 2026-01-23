//namespace sql.builder.WinForms
//{
//    partial class frmSaveLoadGS
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmSaveLoadGS));
//            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
//            this.pFooter = new DevExpress.XtraEditors.PanelControl();
//            this.btnDeleteSetting = new DevExpress.XtraEditors.SimpleButton();
//            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
//            this.grSettings = new DevExpress.XtraGrid.GridControl();
//            this.viewSettings = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colSettingsTitle = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.pSettingTitle = new DevExpress.XtraEditors.PanelControl();
//            this.lSettingTitle = new DevExpress.XtraEditors.LabelControl();
//            this.teSettingTitle = new DevExpress.XtraEditors.TextEdit();
//            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
//            this.btnDeleteSettingT = new DevExpress.XtraBars.BarButtonItem();
//            this.btnAcceptT = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCancelT = new DevExpress.XtraBars.BarButtonItem();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            this.bar1 = new DevExpress.XtraBars.Bar();
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).BeginInit();
//            this.pFooter.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.grSettings)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewSettings)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pSettingTitle)).BeginInit();
//            this.pSettingTitle.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.teSettingTitle.Properties)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            this.SuspendLayout();
            // 
            // btnAccept
            // 
//            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnAccept.Location = new System.Drawing.Point(408, 5);
//            this.btnAccept.Name = "btnAccept";
//            this.btnAccept.Size = new System.Drawing.Size(75, 24);
//            this.btnAccept.TabIndex = 0;
//            this.btnAccept.Text = "Готово";
//            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // pFooter
            // 
//            this.pFooter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.pFooter.Controls.Add(this.btnDeleteSetting);
//            this.pFooter.Controls.Add(this.btnCancel);
//            this.pFooter.Controls.Add(this.btnAccept);
//            this.pFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.pFooter.Location = new System.Drawing.Point(0, 248);
//            this.pFooter.Name = "pFooter";
//            this.pFooter.Size = new System.Drawing.Size(584, 34);
//            this.pFooter.TabIndex = 3;
//            this.pFooter.Visible = false;
            // 
            // btnDeleteSetting
            // 
//            this.btnDeleteSetting.Image = ((System.Drawing.Image)(resources.GetObject("btnDeleteSetting.Image")));
//            this.btnDeleteSetting.ImageLocation = DevExpress.XtraEditors.ImageLocation.MiddleCenter;
//            this.btnDeleteSetting.Location = new System.Drawing.Point(3, 6);
//            this.btnDeleteSetting.Name = "btnDeleteSetting";
//            this.btnDeleteSetting.Size = new System.Drawing.Size(25, 23);
//            this.btnDeleteSetting.TabIndex = 2;
//            this.btnDeleteSetting.Text = "Удалить шаблон";
//            this.btnDeleteSetting.ToolTip = "Удалить шаблон";
//            this.btnDeleteSetting.Click += new System.EventHandler(this.btnDeleteSetting_Click);
            // 
            // btnCancel
            // 
//            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnCancel.Location = new System.Drawing.Point(499, 5);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(75, 24);
//            this.btnCancel.TabIndex = 1;
//            this.btnCancel.Text = "Отмена";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // grSettings
            // 
//            this.grSettings.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.grSettings.Location = new System.Drawing.Point(0, 0);
//            this.grSettings.MainView = this.viewSettings;
//            this.grSettings.Name = "grSettings";
//            this.grSettings.Size = new System.Drawing.Size(584, 219);
//            this.grSettings.TabIndex = 4;
//            this.grSettings.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewSettings});
            // 
            // viewSettings
            // 
//            this.viewSettings.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colSettingsTitle});
//            this.viewSettings.GridControl = this.grSettings;
//            this.viewSettings.Name = "viewSettings";
//            this.viewSettings.OptionsBehavior.Editable = false;
//            this.viewSettings.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.viewSettings.OptionsView.ShowColumnHeaders = false;
//            this.viewSettings.OptionsView.ShowGroupPanel = false;
//            this.viewSettings.OptionsView.ShowIndicator = false;
//            this.viewSettings.OptionsView.ShowViewCaption = true;
//            this.viewSettings.ViewCaption = "Доступные шаблоны";
//            this.viewSettings.RowCellClick += new DevExpress.XtraGrid.Views.Grid.RowCellClickEventHandler(this.viewSettings_RowCellClick);
//            this.viewSettings.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.viewSettings_FocusedRowChanged);
            // 
            // colSettingsTitle
            // 
//            this.colSettingsTitle.Caption = "Настройка";
//            this.colSettingsTitle.FieldName = "TITLE";
//            this.colSettingsTitle.Name = "colSettingsTitle";
//            this.colSettingsTitle.Visible = true;
//            this.colSettingsTitle.VisibleIndex = 0;
            // 
            // pSettingTitle
            // 
//            this.pSettingTitle.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.pSettingTitle.Controls.Add(this.lSettingTitle);
//            this.pSettingTitle.Controls.Add(this.teSettingTitle);
//            this.pSettingTitle.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.pSettingTitle.Location = new System.Drawing.Point(0, 219);
//            this.pSettingTitle.Name = "pSettingTitle";
//            this.pSettingTitle.Size = new System.Drawing.Size(584, 29);
//            this.pSettingTitle.TabIndex = 5;
            // 
            // lSettingTitle
            // 
//            this.lSettingTitle.Location = new System.Drawing.Point(12, 8);
//            this.lSettingTitle.Name = "lSettingTitle";
//            this.lSettingTitle.Size = new System.Drawing.Size(70, 13);
//            this.lSettingTitle.TabIndex = 1;
//            this.lSettingTitle.Text = "Имя шаблона:";
            // 
            // teSettingTitle
            // 
//            this.teSettingTitle.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.teSettingTitle.Location = new System.Drawing.Point(98, 5);
//            this.teSettingTitle.Name = "teSettingTitle";
//            this.teSettingTitle.Size = new System.Drawing.Size(476, 20);
//            this.teSettingTitle.TabIndex = 0;
            // 
            // barManager1
            // 
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.bar1});
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.btnDeleteSettingT,
//            this.btnAcceptT,
//            this.btnCancelT});
//            this.barManager1.MaxItemId = 3;
            // 
            // btnDeleteSettingT
            // 
//            this.btnDeleteSettingT.Caption = "Удалить шаблон";
//            this.btnDeleteSettingT.Glyph = global::sql.builder.Properties.Resources.delete_16x16;
//            this.btnDeleteSettingT.Id = 0;
//            this.btnDeleteSettingT.Name = "btnDeleteSettingT";
//            this.btnDeleteSettingT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnDeleteSettingT_ItemClick);
            // 
            // btnAcceptT
            // 
//            this.btnAcceptT.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnAcceptT.Caption = "Готово";
//            this.btnAcceptT.Glyph = global::sql.builder.Properties.Resources.Ok_16;
//            this.btnAcceptT.Id = 1;
//            this.btnAcceptT.Name = "btnAcceptT";
//            this.btnAcceptT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAcceptT_ItemClick);
            // 
            // btnCancelT
            // 
//            this.btnCancelT.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.btnCancelT.Caption = "Отмена";
//            this.btnCancelT.Id = 2;
//            this.btnCancelT.Name = "btnCancelT";
//            this.btnCancelT.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancelT_ItemClick);
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(584, 0);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 282);
//            this.barDockControlBottom.Size = new System.Drawing.Size(584, 31);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 282);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(584, 0);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 282);
            // 
            // bar1
            // 
//            this.bar1.BarName = "Пользовательская 2";
//            this.bar1.DockCol = 0;
//            this.bar1.DockRow = 0;
//            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.bar1.FloatLocation = new System.Drawing.Point(54, 442);
//            this.bar1.FloatSize = new System.Drawing.Size(1000, 0);
//            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnDeleteSettingT, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
//            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.btnAcceptT, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCancelT)});
//            this.bar1.OptionsBar.AllowQuickCustomization = false;
//            this.bar1.OptionsBar.AutoPopupMode = DevExpress.XtraBars.BarAutoPopupMode.All;
//            this.bar1.OptionsBar.DisableCustomization = true;
//            this.bar1.OptionsBar.DrawBorder = false;
//            this.bar1.OptionsBar.UseWholeRow = true;
//            this.bar1.Text = "Пользовательская 2";
            // 
            // frmSaveLoadGS
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(584, 313);
//            this.Controls.Add(this.grSettings);
//            this.Controls.Add(this.pSettingTitle);
//            this.Controls.Add(this.pFooter);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.MinimumSize = new System.Drawing.Size(200, 200);
//            this.Name = "frmSaveLoadGS";
//            this.Text = "";
//            this.UserSettings.SaveFormSize = true;
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).EndInit();
//            this.pFooter.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.grSettings)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewSettings)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pSettingTitle)).EndInit();
//            this.pSettingTitle.ResumeLayout(false);
//            this.pSettingTitle.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.teSettingTitle.Properties)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraEditors.SimpleButton btnAccept;
//        private DevExpress.XtraEditors.PanelControl pFooter;
//        private DevExpress.XtraEditors.SimpleButton btnCancel;
//        private DevExpress.XtraGrid.GridControl grSettings;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewSettings;
//        private DevExpress.XtraGrid.Columns.GridColumn colSettingsTitle;
//        private DevExpress.XtraEditors.PanelControl pSettingTitle;
//        private DevExpress.XtraEditors.LabelControl lSettingTitle;
//        private DevExpress.XtraEditors.TextEdit teSettingTitle;
//        private DevExpress.XtraEditors.SimpleButton btnDeleteSetting;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.BarButtonItem btnDeleteSettingT;
//        private DevExpress.XtraBars.BarButtonItem btnAcceptT;
//        private DevExpress.XtraBars.BarButtonItem btnCancelT;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraBars.Bar bar1;
//    }
//}
