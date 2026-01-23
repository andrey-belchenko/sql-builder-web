//namespace sql.builder.Controls
//{
//    internal partial class ucReportGridOld
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
//            this.barMenu = new DevExpress.XtraBars.Bar();
//            this.cbTableViewMode = new DevExpress.XtraBars.BarEditItem();
//            this.rcbTableViewMode = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            this.cbTableLevels = new DevExpress.XtraBars.BarEditItem();
//            this.rcbTableLevels = new DevExpress.XtraEditors.Repository.RepositoryItemComboBox();
//            this.barFooter = new DevExpress.XtraBars.Bar();
//            this.teTotalSum = new DevExpress.XtraBars.BarEditItem();
//            this.rteTotalSum = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.teTotalCount = new DevExpress.XtraBars.BarEditItem();
//            this.rteTotalCount = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.lCompareResult = new DevExpress.XtraBars.BarStaticItem();
//            this.lFormingTime = new DevExpress.XtraBars.BarStaticItem();
//            this.lAvgFormingTime = new DevExpress.XtraBars.BarStaticItem();
//            this.lCurrentSetting1 = new DevExpress.XtraBars.BarStaticItem();
//            this.barEditItem1 = new DevExpress.XtraBars.BarEditItem();
//            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
//            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
//            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
//            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
//            this.btnRefresh = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
//            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
//            this.rpgActionDemo = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.popup = new DevExpress.XtraBars.PopupMenu(this.components);
//            ((System.ComponentModel.ISupportInitialize)(this.rcbTableViewMode)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rcbTableLevels)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popup)).BeginInit();
//            this.SuspendLayout();
            // 
            // barMenu
            // 
//            this.barMenu.BarName = "Сервис";
//            this.barMenu.DockCol = 0;
//            this.barMenu.DockRow = 0;
//            this.barMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.barMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.cbTableViewMode),
//            new DevExpress.XtraBars.LinkPersistInfo(this.cbTableLevels)});
//            this.barMenu.OptionsBar.AllowQuickCustomization = false;
//            this.barMenu.OptionsBar.DisableClose = true;
//            this.barMenu.OptionsBar.DisableCustomization = true;
//            this.barMenu.OptionsBar.DrawDragBorder = false;
//            this.barMenu.OptionsBar.UseWholeRow = true;
//            this.barMenu.Text = "Сервис";
            // 
            // cbTableViewMode
            // 
//            this.cbTableViewMode.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.cbTableViewMode.Border = DevExpress.XtraEditors.Controls.BorderStyles.Default;
//            this.cbTableViewMode.Caption = "Тип таблицы";
//            this.cbTableViewMode.Edit = this.rcbTableViewMode;
//            this.cbTableViewMode.EditWidth = 127;
//            this.cbTableViewMode.Id = 1;
//            this.cbTableViewMode.Name = "cbTableViewMode";
//            this.cbTableViewMode.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.cbTableViewMode.EditValueChanged += new System.EventHandler(this.cbTableViewMode_EditValueChanged);
            // 
            // rcbTableViewMode
            // 
//            this.rcbTableViewMode.AutoHeight = false;
//            this.rcbTableViewMode.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rcbTableViewMode.Name = "rcbTableViewMode";
//            this.rcbTableViewMode.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
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
            // barFooter
            // 
//            this.barFooter.BarName = "Строка состояния";
//            this.barFooter.DockCol = 0;
//            this.barFooter.DockRow = 0;
//            this.barFooter.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.barFooter.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.teTotalSum),
//            new DevExpress.XtraBars.LinkPersistInfo(this.teTotalCount),
//            new DevExpress.XtraBars.LinkPersistInfo(this.lCompareResult),
//            new DevExpress.XtraBars.LinkPersistInfo(this.lFormingTime),
//            new DevExpress.XtraBars.LinkPersistInfo(this.lAvgFormingTime)});
//            this.barFooter.OptionsBar.AllowQuickCustomization = false;
//            this.barFooter.OptionsBar.DisableClose = true;
//            this.barFooter.OptionsBar.DisableCustomization = true;
//            this.barFooter.OptionsBar.DrawDragBorder = false;
//            this.barFooter.OptionsBar.UseWholeRow = true;
//            this.barFooter.Text = "Строка состояния";
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
//            this.lCompareResult.Caption = "Различий: 0";
//            this.lCompareResult.Id = 0;
//            this.lCompareResult.ItemAppearance.Normal.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
//            this.lCompareResult.ItemAppearance.Normal.Options.UseFont = true;
//            this.lCompareResult.Name = "lCompareResult";
//            this.lCompareResult.TextAlignment = System.Drawing.StringAlignment.Near;
//            this.lCompareResult.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // lFormingTime
            // 
//            this.lFormingTime.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.lFormingTime.Id = 1;
//            this.lFormingTime.Name = "lFormingTime";
//            this.lFormingTime.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // lAvgFormingTime
            // 
//            this.lAvgFormingTime.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.lAvgFormingTime.Id = 2;
//            this.lAvgFormingTime.Name = "lAvgFormingTime";
//            this.lAvgFormingTime.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // lCurrentSetting1
            // 
//            this.lCurrentSetting1.Caption = "Текущая настройка:  по умолчанию";
//            this.lCurrentSetting1.Id = 15;
//            this.lCurrentSetting1.Name = "lCurrentSetting1";
//            this.lCurrentSetting1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // barEditItem1
            // 
//            this.barEditItem1.Caption = "barEditItem1";
//            this.barEditItem1.Edit = this.repositoryItemTextEdit1;
//            this.barEditItem1.Id = 14;
//            this.barEditItem1.Name = "barEditItem1";
            // 
            // repositoryItemTextEdit1
            // 
//            this.repositoryItemTextEdit1.AutoHeight = false;
//            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // barButtonItem1
            // 
//            this.barButtonItem1.Caption = "Окно настроек";
//            this.barButtonItem1.Id = 10;
//            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barManager
            // 
//            this.barManager.AllowCustomization = false;
//            this.barManager.AllowQuickCustomization = false;
//            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.barMenu,
//            this.barFooter});
//            this.barManager.DockControls.Add(this.barDockControl1);
//            this.barManager.DockControls.Add(this.barDockControl2);
//            this.barManager.DockControls.Add(this.barDockControl3);
//            this.barManager.DockControls.Add(this.barDockControl4);
//            this.barManager.Form = this;
//            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.cbTableViewMode,
//            this.cbTableLevels,
//            this.teTotalSum,
//            this.teTotalCount,
//            this.lCurrentSetting1,
//            this.lCompareResult,
//            this.lFormingTime,
//            this.lAvgFormingTime});
//            this.barManager.MaxItemId = 3;
            // 
            // barDockControl1
            // 
//            this.barDockControl1.CausesValidation = false;
//            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
//            this.barDockControl1.Size = new System.Drawing.Size(722, 29);
            // 
            // barDockControl2
            // 
//            this.barDockControl2.CausesValidation = false;
//            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControl2.Location = new System.Drawing.Point(0, 335);
//            this.barDockControl2.Size = new System.Drawing.Size(722, 29);
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
//            this.barDockControl4.Location = new System.Drawing.Point(722, 29);
//            this.barDockControl4.Size = new System.Drawing.Size(0, 306);
            // 
            // ribbonControl1
            // 
//            this.ribbonControl1.ExpandCollapseItem.Id = 0;
//            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.ribbonControl1.ExpandCollapseItem,
//            this.btnRefresh,
//            this.barButtonItem2});
//            this.ribbonControl1.Location = new System.Drawing.Point(0, 29);
//            this.ribbonControl1.MaxItemId = 3;
//            this.ribbonControl1.Name = "ribbonControl1";
//            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
//            this.ribbonPage1});
//            this.ribbonControl1.Size = new System.Drawing.Size(722, 141);
//            this.ribbonControl1.Visible = false;
//            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            // 
            // btnRefresh
            // 
//            this.btnRefresh.Caption = "Обновить";
//            this.btnRefresh.Id = 1;
//            this.btnRefresh.Name = "btnRefresh";
//            this.btnRefresh.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRefresh_ItemClick);
            // 
            // barButtonItem2
            // 
//            this.barButtonItem2.Caption = "barButtonItem2";
//            this.barButtonItem2.Id = 2;
//            this.barButtonItem2.Name = "barButtonItem2";
//            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnActionMenu_ItemClick);
            // 
            // ribbonPage1
            // 
//            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
//            this.rpgActionDemo});
//            this.ribbonPage1.Name = "ribbonPage1";
//            this.ribbonPage1.Text = "Редактор данных (тест)";
//            this.ribbonPage1.Visible = false;
            // 
            // rpgActionDemo
            // 
//            this.rpgActionDemo.ItemLinks.Add(this.btnRefresh);
//            this.rpgActionDemo.Name = "rpgActionDemo";
//            this.rpgActionDemo.Text = "Функции таблицы";
            // 
            // popup
            // 
//            this.popup.MenuCaption = "Действие";
//            this.popup.Name = "popup";
//            this.popup.Ribbon = this.ribbonControl1;
//            this.popup.ShowCaption = true;
            // 
            // ucReportGrid
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.ribbonControl1);
//            this.Controls.Add(this.barDockControl3);
//            this.Controls.Add(this.barDockControl4);
//            this.Controls.Add(this.barDockControl2);
//            this.Controls.Add(this.barDockControl1);
//            this.Name = "ucReportGrid";
//            this.Size = new System.Drawing.Size(722, 364);
//            this.Load += new System.EventHandler(this.ucReportGrid_Load);
//            this.Controls.SetChildIndex(this.barDockControl1, 0);
//            this.Controls.SetChildIndex(this.barDockControl2, 0);
//            this.Controls.SetChildIndex(this.barDockControl4, 0);
//            this.Controls.SetChildIndex(this.barDockControl3, 0);
//            this.Controls.SetChildIndex(this.ribbonControl1, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.rcbTableViewMode)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rcbTableLevels)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalSum)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rteTotalCount)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popup)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraBars.Bar barMenu;
//        private DevExpress.XtraBars.BarEditItem cbTableViewMode;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rcbTableViewMode;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
//        private DevExpress.XtraBars.Bar barFooter;
//        private DevExpress.XtraBars.BarEditItem teTotalSum;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteTotalSum;
//        private DevExpress.XtraBars.BarEditItem teTotalCount;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit rteTotalCount;
//        private DevExpress.XtraBars.BarEditItem cbTableLevels;
//        private DevExpress.XtraEditors.Repository.RepositoryItemComboBox rcbTableLevels;
//        private DevExpress.XtraBars.BarEditItem barEditItem1;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
//        private DevExpress.XtraBars.BarStaticItem lCurrentSetting1;
//        private DevExpress.XtraBars.BarManager barManager;
//        private DevExpress.XtraBars.BarDockControl barDockControl1;
//        private DevExpress.XtraBars.BarDockControl barDockControl2;
//        private DevExpress.XtraBars.BarDockControl barDockControl3;
//        private DevExpress.XtraBars.BarDockControl barDockControl4;
//        private DevExpress.XtraBars.BarStaticItem lCompareResult;
//        private DevExpress.XtraBars.BarStaticItem lFormingTime;
//        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
//        private DevExpress.XtraBars.BarButtonItem btnRefresh;
//        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgActionDemo;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
//        public DevExpress.XtraBars.PopupMenu popup;
//        private DevExpress.XtraBars.BarStaticItem lAvgFormingTime;
//    }
//}
