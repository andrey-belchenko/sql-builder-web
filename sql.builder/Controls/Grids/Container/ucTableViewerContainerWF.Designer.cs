//namespace sql.builder.Controls
//{
//    internal partial class ucTableViewerContainerWF
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
//            this.components = new System.ComponentModel.Container();
//            this.barToolbar = new DevExpress.XtraBars.Bar();
//            this.cbViewMode = new DevExpress.XtraBars.BarEditItem();
//            this.rcbViewMode = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            this.cbTableLevels = new DevExpress.XtraBars.BarEditItem();
//            this.rcbTableLevels = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            this.beMerge = new DevExpress.XtraBars.BarEditItem();
//            this.ricMerge = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.barFooter = new DevExpress.XtraBars.Bar();
//            this.lPrintingTime = new DevExpress.XtraBars.BarStaticItem();
//            this.lFormingTime = new DevExpress.XtraBars.BarStaticItem();
//            this.lAvgFormingTime = new DevExpress.XtraBars.BarStaticItem();
//            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
//            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
//            this.repositoryItemCheckedComboBoxEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit();
//            this.popup = new DevExpress.XtraBars.PopupMenu(this.components);
//            ((System.ComponentModel.ISupportInitialize)(this.rcbViewMode)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rcbTableLevels)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ricMerge)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popup)).BeginInit();
//            this.SuspendLayout();
            // 
            // barTopToolbar
            // 
//            this.barToolbar.BarName = "Сервис";
//            this.barToolbar.DockCol = 0;
//            this.barToolbar.DockRow = 0;
//            this.barToolbar.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.barToolbar.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.cbViewMode),
//            new DevExpress.XtraBars.LinkPersistInfo(this.cbTableLevels),
//            new DevExpress.XtraBars.LinkPersistInfo(DevExpress.XtraBars.BarLinkUserDefines.PaintStyle, this.beMerge, DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph)});
//            this.barToolbar.OptionsBar.AllowQuickCustomization = false;
//            this.barToolbar.OptionsBar.DisableClose = true;
//            this.barToolbar.OptionsBar.DisableCustomization = true;
//            this.barToolbar.OptionsBar.DrawBorder = false;
//            this.barToolbar.OptionsBar.DrawDragBorder = false;
//            this.barToolbar.OptionsBar.UseWholeRow = true;
//            this.barToolbar.Text = "Сервис";
            // 
            // cbViewMode
            // 
//            this.cbViewMode.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.cbViewMode.Border = DevExpress.XtraEditors.Controls.BorderStyles.Default;
//            this.cbViewMode.Caption = "Режим отображения";
//            this.cbViewMode.Edit = this.rcbViewMode;
//            this.cbViewMode.EditWidth = 127;
//            this.cbViewMode.Id = 1;
//            this.cbViewMode.Name = "cbViewMode";
//            this.cbViewMode.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.cbViewMode.EditValueChanged += new System.EventHandler(this.cbViewMode_EditValueChanged);
            // 
            // rcbViewMode
            // 
//            this.rcbViewMode.AutoHeight = false;
//            this.rcbViewMode.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rcbViewMode.Name = "rcbViewMode";
//            this.rcbViewMode.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
            // 
            // cbTableLevels
            // 
//            this.cbTableLevels.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.cbTableLevels.Caption = "Отображена таблица";
//            this.cbTableLevels.Edit = this.rcbTableLevels;
//            this.cbTableLevels.EditWidth = 243;
//            this.cbTableLevels.Id = 8;
//            this.cbTableLevels.Name = "cbTableLevels";
//            this.cbTableLevels.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.cbTableLevels.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.cbTableLevels.EditValueChanged += new System.EventHandler(this.cbTableLevels_EditValueChanged);
            // 
            // rcbTableLevels
            // 
//            this.rcbTableLevels.AutoHeight = false;
//            this.rcbTableLevels.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rcbTableLevels.Name = "rcbTableLevels";
//            this.rcbTableLevels.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
//            this.rcbTableLevels.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.rcbTableLevels_CustomDisplayText);
            // 
            // beMerge
            // 
//            this.beMerge.Caption = "Объединение ячеек";
//            this.beMerge.Edit = this.ricMerge;
//            this.beMerge.EditValue = new decimal(new int[] {
//            0,
//            0,
//            0,
//            0});
//            this.beMerge.Id = 5;
//            this.beMerge.Name = "beMerge";
//            this.beMerge.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // ricMerge
            // 
//            this.ricMerge.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.ricMerge.Appearance.Options.UseBackColor = true;
//            this.ricMerge.AutoHeight = false;
//            this.ricMerge.Name = "ricMerge";
//            this.ricMerge.ValueChecked = new decimal(new int[] {
//            1,
//            0,
//            0,
//            0});
//            this.ricMerge.ValueUnchecked = new decimal(new int[] {
//            0,
//            0,
//            0,
//            0});
//            this.ricMerge.CheckedChanged += new System.EventHandler(this.ricMerge_CheckedChanged);
            // 
            // barFooter
            // 
//            this.barFooter.BarName = "Строка состояния";
//            this.barFooter.DockCol = 0;
//            this.barFooter.DockRow = 0;
//            this.barFooter.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.barFooter.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.lPrintingTime),
//            new DevExpress.XtraBars.LinkPersistInfo(this.lFormingTime),
//            new DevExpress.XtraBars.LinkPersistInfo(this.lAvgFormingTime)});
//            this.barFooter.OptionsBar.AllowQuickCustomization = false;
//            this.barFooter.OptionsBar.DisableClose = true;
//            this.barFooter.OptionsBar.DisableCustomization = true;
//            this.barFooter.OptionsBar.DrawBorder = false;
//            this.barFooter.OptionsBar.DrawDragBorder = false;
//            this.barFooter.OptionsBar.UseWholeRow = true;
//            this.barFooter.Text = "Строка состояния";
//            this.barFooter.Visible = false;
            // 
            // lPrintingTime
            // 
//            this.lPrintingTime.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.lPrintingTime.Caption = "";
//            this.lPrintingTime.Id = 6;
//            this.lPrintingTime.Name = "lPrintingTime";
//            this.lPrintingTime.TextAlignment = System.Drawing.StringAlignment.Near;
//            this.lPrintingTime.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // lFormingTime
            // 
//            this.lFormingTime.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.lFormingTime.Caption = "";
//            this.lFormingTime.Id = 1;
//            this.lFormingTime.Name = "lFormingTime";
//            this.lFormingTime.TextAlignment = System.Drawing.StringAlignment.Near;
//            this.lFormingTime.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // lAvgFormingTime
            // 
//            this.lAvgFormingTime.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.lAvgFormingTime.Caption = "";
//            this.lAvgFormingTime.Id = 2;
//            this.lAvgFormingTime.Name = "lAvgFormingTime";
//            this.lAvgFormingTime.TextAlignment = System.Drawing.StringAlignment.Near;
//            this.lAvgFormingTime.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // barManager
            // 
//            this.barManager.AllowCustomization = false;
//            this.barManager.AllowQuickCustomization = false;
//            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.barToolbar,
//            this.barFooter});
//            this.barManager.DockControls.Add(this.barDockControl1);
//            this.barManager.DockControls.Add(this.barDockControl2);
//            this.barManager.DockControls.Add(this.barDockControl3);
//            this.barManager.DockControls.Add(this.barDockControl4);
//            this.barManager.Form = this;
//            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.cbViewMode,
//            this.cbTableLevels,
//            this.lFormingTime,
//            this.lAvgFormingTime,
//            this.beMerge,
//            this.lPrintingTime});
//            this.barManager.MaxItemId = 7;
//            this.barManager.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.repositoryItemCheckedComboBoxEdit1,
//            this.ricMerge});
            // 
            // barDockControl1
            // 
//            this.barDockControl1.CausesValidation = false;
//            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
//            this.barDockControl1.Size = new System.Drawing.Size(1006, 29);
            // 
            // barDockControl2
            // 
//            this.barDockControl2.CausesValidation = false;
//            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControl2.Location = new System.Drawing.Point(0, 335);
//            this.barDockControl2.Size = new System.Drawing.Size(1006, 29);
            // 
            // barDockControl3
            // 
//            this.barDockControl3.CausesValidation = false;
//            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControl3.Location = new System.Drawing.Point(0, 29);
//            this.barDockControl3.Size = new System.Drawing.Size(0, 306);
            // 
            // barDockControl4
            // 
//            this.barDockControl4.CausesValidation = false;
//            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControl4.Location = new System.Drawing.Point(1006, 29);
//            this.barDockControl4.Size = new System.Drawing.Size(0, 306);
            // 
            // repositoryItemCheckedComboBoxEdit1
            // 
//            this.repositoryItemCheckedComboBoxEdit1.AutoHeight = false;
//            this.repositoryItemCheckedComboBoxEdit1.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.repositoryItemCheckedComboBoxEdit1.Name = "repositoryItemCheckedComboBoxEdit1";
            // 
            // popup
            // 
//            this.popup.MenuCaption = "Действие";
//            this.popup.Name = "popup";
//            this.popup.ShowCaption = true;
            // 
            // ucGridContainerWF
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.barDockControl3);
//            this.Controls.Add(this.barDockControl4);
//            this.Controls.Add(this.barDockControl2);
//            this.Controls.Add(this.barDockControl1);
//            this.Name = "ucGridContainerWF";
//            this.Size = new System.Drawing.Size(1006, 364);
//            ((System.ComponentModel.ISupportInitialize)(this.rcbViewMode)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rcbTableLevels)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ricMerge)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckedComboBoxEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popup)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        public DevExpress.XtraBars.Bar barToolbar;
//        public DevExpress.XtraBars.BarEditItem cbViewMode;
//        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox rcbViewMode;
//        public DevExpress.XtraBars.Bar barFooter;
//        public DevExpress.XtraBars.BarEditItem cbTableLevels;
//        public DevExpress.XtraEditors.Repository.RepositoryItemComboBox rcbTableLevels;
//        public DevExpress.XtraBars.BarManager barManager;
//        public DevExpress.XtraBars.BarDockControl barDockControl1;
//        public DevExpress.XtraBars.BarDockControl barDockControl2;
//        public DevExpress.XtraBars.BarDockControl barDockControl3;
//        public DevExpress.XtraBars.BarDockControl barDockControl4;
//        public DevExpress.XtraBars.BarStaticItem lFormingTime;
//        public DevExpress.XtraBars.PopupMenu popup;
//        public DevExpress.XtraBars.BarStaticItem lAvgFormingTime;
//        public DevExpress.XtraBars.BarEditItem beMerge;
//        public DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit ricMerge;
//        public DevExpress.XtraEditors.Repository.RepositoryItemCheckedComboBoxEdit repositoryItemCheckedComboBoxEdit1;
//        public DevExpress.XtraBars.BarStaticItem lPrintingTime;
//    }
//}
