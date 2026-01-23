//namespace sql.builder.Controls
//{
//    partial class FormParametersControl
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
//            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
//            this.barMenu = new DevExpress.XtraBars.Bar();
//            this.btnWorkFolderPath = new DevExpress.XtraBars.BarEditItem();
//            this.rbtnWorkFolderPath = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbtnWorkFolderPath)).BeginInit();
//            this.SuspendLayout();
            // 
            // barManager1
            // 
//            this.barManager1.AllowCustomization = false;
//            this.barManager1.AllowQuickCustomization = false;
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.barMenu});
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.btnWorkFolderPath});
//            this.barManager1.MaxItemId = 7;
//            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rbtnWorkFolderPath});
            // 
            // barMenu
            // 
//            this.barMenu.BarName = "Сервис";
//            this.barMenu.DockCol = 0;
//            this.barMenu.DockRow = 0;
//            this.barMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.barMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnWorkFolderPath)});
//            this.barMenu.OptionsBar.AllowQuickCustomization = false;
//            this.barMenu.OptionsBar.DisableClose = true;
//            this.barMenu.OptionsBar.DisableCustomization = true;
//            this.barMenu.OptionsBar.DrawBorder = false;
//            this.barMenu.OptionsBar.DrawDragBorder = false;
//            this.barMenu.OptionsBar.MultiLine = true;
//            this.barMenu.OptionsBar.UseWholeRow = true;
//            this.barMenu.Text = "Сервис";
//            this.barMenu.Visible = false;
            // 
            // btnWorkFolderPath
            // 
//            this.btnWorkFolderPath.AllowHtmlText = DevExpress.Utils.DefaultBoolean.False;
//            this.btnWorkFolderPath.Caption = "Рабочая папка";
//            this.btnWorkFolderPath.Edit = this.rbtnWorkFolderPath;
//            this.btnWorkFolderPath.EditValue = "";
//            this.btnWorkFolderPath.EditWidth = 240;
//            this.btnWorkFolderPath.Id = 11;
//            this.btnWorkFolderPath.Name = "btnWorkFolderPath";
//            this.btnWorkFolderPath.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//            this.btnWorkFolderPath.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // rbtnWorkFolderPath
            // 
//            this.rbtnWorkFolderPath.Appearance.Font = new System.Drawing.Font("Tahoma", 8.25F, System.Drawing.FontStyle.Underline);
//            this.rbtnWorkFolderPath.Appearance.Options.UseFont = true;
//            this.rbtnWorkFolderPath.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton()});
//            this.rbtnWorkFolderPath.Name = "rbtnWorkFolderPath";
//            this.rbtnWorkFolderPath.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor;
//            this.rbtnWorkFolderPath.ButtonPressed += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.rbtnWorkFolderPath_ButtonPressed);
//            this.rbtnWorkFolderPath.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.rbtnWorkFolderPath_CustomDisplayText);
//            this.rbtnWorkFolderPath.Click += new System.EventHandler(this.rbtnWorkFolderPath_Click);
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Manager = this.barManager1;
//            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlTop.Size = new System.Drawing.Size(519, 35);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 185);
//            this.barDockControlBottom.Manager = this.barManager1;
//            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlBottom.Size = new System.Drawing.Size(519, 0);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 35);
//            this.barDockControlLeft.Manager = this.barManager1;
//            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 150);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(519, 35);
//            this.barDockControlRight.Manager = this.barManager1;
//            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 150);
            // 
            // FormParametersControl
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.Name = "FormParametersControl";
//            this.Size = new System.Drawing.Size(519, 185);
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rbtnWorkFolderPath)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.Bar barMenu;
//        private DevExpress.XtraBars.BarEditItem btnWorkFolderPath;
//        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit rbtnWorkFolderPath;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;

//    }
//}
