//namespace sql.builder.Controls
//{
//    internal partial class ucNoGrid
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
//            this.lInfo = new DevExpress.XtraEditors.LabelControl();
//            this.barManager = new DevExpress.XtraBars.BarManager(this.components);
//            this.barFooter = new DevExpress.XtraBars.Bar();
//            this.lFormingTime = new DevExpress.XtraBars.BarStaticItem();
//            this.lAvgFormingTime = new DevExpress.XtraBars.BarStaticItem();
//            this.barDockControl1 = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControl2 = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControl3 = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControl4 = new DevExpress.XtraBars.BarDockControl();
//            this.cbTableViewMode = new DevExpress.XtraBars.BarEditItem();
//            this.cbTableLevels = new DevExpress.XtraBars.BarEditItem();
//            this.lCurrentSetting1 = new DevExpress.XtraBars.BarStaticItem();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();
//            this.SuspendLayout();
            // 
            // lInfo
            // 
//            this.lInfo.Appearance.Font = new System.Drawing.Font("Tahoma", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
//            this.lInfo.Appearance.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.lInfo.Appearance.TextOptions.VAlignment = DevExpress.Utils.VertAlignment.Center;
//            this.lInfo.Appearance.TextOptions.WordWrap = DevExpress.Utils.WordWrap.Wrap;
//            this.lInfo.AutoSizeMode = DevExpress.XtraEditors.LabelAutoSizeMode.None;
//            this.lInfo.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.lInfo.Location = new System.Drawing.Point(0, 0);
//            this.lInfo.Name = "lInfo";
//            this.lInfo.Size = new System.Drawing.Size(653, 510);
//            this.lInfo.TabIndex = 0;
//            this.lInfo.Text = "Чтобы вывести отчёт, задайте параметры и нажмите кнопку \"Печатная форма\"";
            // 
            // barManager
            // 
//            this.barManager.AllowCustomization = false;
//            this.barManager.AllowQuickCustomization = false;
//            this.barManager.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.barFooter});
//            this.barManager.DockControls.Add(this.barDockControl1);
//            this.barManager.DockControls.Add(this.barDockControl2);
//            this.barManager.DockControls.Add(this.barDockControl3);
//            this.barManager.DockControls.Add(this.barDockControl4);
//            this.barManager.Form = this;
//            this.barManager.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.cbTableViewMode,
//            this.cbTableLevels,
//            this.lCurrentSetting1,
//            this.lFormingTime,
//            this.lAvgFormingTime});
//            this.barManager.MaxItemId = 3;
            // 
            // barFooter
            // 
//            this.barFooter.BarName = "Строка состояния";
//            this.barFooter.DockCol = 0;
//            this.barFooter.DockRow = 0;
//            this.barFooter.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.barFooter.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.lFormingTime),
//            new DevExpress.XtraBars.LinkPersistInfo(this.lAvgFormingTime)});
//            this.barFooter.OptionsBar.AllowQuickCustomization = false;
//            this.barFooter.OptionsBar.DisableClose = true;
//            this.barFooter.OptionsBar.DisableCustomization = true;
//            this.barFooter.OptionsBar.DrawDragBorder = false;
//            this.barFooter.OptionsBar.UseWholeRow = true;
//            this.barFooter.Text = "Строка состояния";
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
            // barDockControl1
            // 
//            this.barDockControl1.CausesValidation = false;
//            this.barDockControl1.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControl1.Location = new System.Drawing.Point(0, 0);
//            this.barDockControl1.Size = new System.Drawing.Size(653, 0);
            // 
            // barDockControl2
            // 
//            this.barDockControl2.CausesValidation = false;
//            this.barDockControl2.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControl2.Location = new System.Drawing.Point(0, 510);
//            this.barDockControl2.Size = new System.Drawing.Size(653, 29);
            // 
            // barDockControl3
            // 
//            this.barDockControl3.CausesValidation = false;
//            this.barDockControl3.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControl3.Location = new System.Drawing.Point(0, 0);
//            this.barDockControl3.Size = new System.Drawing.Size(0, 510);
            // 
            // barDockControl4
            // 
//            this.barDockControl4.CausesValidation = false;
//            this.barDockControl4.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControl4.Location = new System.Drawing.Point(653, 0);
//            this.barDockControl4.Size = new System.Drawing.Size(0, 510);
            // 
            // cbTableViewMode
            // 
//            this.cbTableViewMode.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.cbTableViewMode.Border = DevExpress.XtraEditors.Controls.BorderStyles.Default;
//            this.cbTableViewMode.Caption = "Тип таблицы";
//            this.cbTableViewMode.Edit = null;
//            this.cbTableViewMode.EditWidth = 127;
//            this.cbTableViewMode.Id = 1;
//            this.cbTableViewMode.Name = "cbTableViewMode";
//            this.cbTableViewMode.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // cbTableLevels
            // 
//            this.cbTableLevels.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.cbTableLevels.Caption = "Отображена таблица";
//            this.cbTableLevels.Edit = null;
//            this.cbTableLevels.EditWidth = 243;
//            this.cbTableLevels.Id = 8;
//            this.cbTableLevels.Name = "cbTableLevels";
//            this.cbTableLevels.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.cbTableLevels.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // lCurrentSetting1
            // 
//            this.lCurrentSetting1.Caption = "Текущая настройка:  по умолчанию";
//            this.lCurrentSetting1.Id = 15;
//            this.lCurrentSetting1.Name = "lCurrentSetting1";
//            this.lCurrentSetting1.TextAlignment = System.Drawing.StringAlignment.Near;
            // 
            // ucNoGrid
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.lInfo);
//            this.Controls.Add(this.barDockControl3);
//            this.Controls.Add(this.barDockControl4);
//            this.Controls.Add(this.barDockControl2);
//            this.Controls.Add(this.barDockControl1);
//            this.Name = "ucNoGrid";
//            this.Size = new System.Drawing.Size(653, 539);
//            this.Controls.SetChildIndex(this.barDockControl1, 0);
//            this.Controls.SetChildIndex(this.barDockControl2, 0);
//            this.Controls.SetChildIndex(this.barDockControl4, 0);
//            this.Controls.SetChildIndex(this.barDockControl3, 0);
//            this.Controls.SetChildIndex(this.lInfo, 0);
//            ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraEditors.LabelControl lInfo;
//        private DevExpress.XtraBars.BarManager barManager;
//        private DevExpress.XtraBars.Bar barFooter;
//        private DevExpress.XtraBars.BarStaticItem lFormingTime;
//        private DevExpress.XtraBars.BarStaticItem lAvgFormingTime;
//        private DevExpress.XtraBars.BarDockControl barDockControl1;
//        private DevExpress.XtraBars.BarDockControl barDockControl2;
//        private DevExpress.XtraBars.BarDockControl barDockControl3;
//        private DevExpress.XtraBars.BarDockControl barDockControl4;
//        private DevExpress.XtraBars.BarEditItem cbTableViewMode;
//        private DevExpress.XtraBars.BarEditItem cbTableLevels;
//        private DevExpress.XtraBars.BarStaticItem lCurrentSetting1;
//    }
//}
