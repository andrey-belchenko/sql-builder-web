//namespace sql.builder
//{
//    internal partial class ucFormDesigner
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
//            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
//            this.btnAccept = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCancel = new DevExpress.XtraBars.BarButtonItem();
//            this.btnLoadDefault = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem12 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem14 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnFieldsSettings = new DevExpress.XtraBars.BarButtonItem();
//            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
//            this.rpgNavigation = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.panelControl1 = new DevExpress.XtraEditors.PanelControl();
//            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).BeginInit();
//            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
//            this.ribbonControl1.ExpandCollapseItem.Id = 0;
//            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.ribbonControl1.ExpandCollapseItem,
//            this.btnAccept,
//            this.btnCancel,
//            this.btnLoadDefault,
//            this.barButtonItem12,
//            this.barButtonItem14,
//            this.btnFieldsSettings});
//            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
//            this.ribbonControl1.MaxItemId = 18;
//            this.ribbonControl1.Name = "ribbonControl1";
//            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
//            this.ribbonPage1});
//            this.ribbonControl1.Size = new System.Drawing.Size(891, 141);
//            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            // 
            // btnAccept
            // 
//            this.btnAccept.Caption = "Принять";
//            this.btnAccept.Id = 6;
//            this.btnAccept.Name = "btnAccept";
//            this.btnAccept.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAccept_ItemClick);
            // 
            // btnCancel
            // 
//            this.btnCancel.Caption = "Отмена";
//            this.btnCancel.Id = 7;
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCancel_ItemClick);
            // 
            // btnLoadDefault
            // 
//            this.btnLoadDefault.Caption = "Вернуть дефолтный";
//            this.btnLoadDefault.Id = 8;
//            this.btnLoadDefault.Name = "btnLoadDefault";
//            this.btnLoadDefault.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnLoadDefault_ItemClick);
            // 
            // barButtonItem12
            // 
//            this.barButtonItem12.Caption = "Операция Выбор СФ";
//            this.barButtonItem12.Id = 14;
//            this.barButtonItem12.Name = "barButtonItem12";
            // 
            // barButtonItem14
            // 
//            this.barButtonItem14.Caption = "Форма ur_mat";
//            this.barButtonItem14.Id = 16;
//            this.barButtonItem14.Name = "barButtonItem14";
            // 
            // btnFieldsSettings
            // 
//            this.btnFieldsSettings.Caption = "Выбор настраиваемых полей";
//            this.btnFieldsSettings.Id = 17;
//            this.btnFieldsSettings.Name = "btnFieldsSettings";
//            this.btnFieldsSettings.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnFieldsSettings_ItemClick);
            // 
            // ribbonPage1
            // 
//            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
//            this.rpgNavigation});
//            this.ribbonPage1.Name = "ribbonPage1";
//            this.ribbonPage1.Text = "Форма";
            // 
            // rpgNavigation
            // 
//            this.rpgNavigation.ItemLinks.Add(this.btnAccept);
//            this.rpgNavigation.ItemLinks.Add(this.btnCancel);
//            this.rpgNavigation.ItemLinks.Add(this.btnLoadDefault);
//            this.rpgNavigation.ItemLinks.Add(this.btnFieldsSettings);
//            this.rpgNavigation.Name = "rpgNavigation";
//            this.rpgNavigation.Text = "Дизайн";
            // 
            // panelControl1
            // 
//            this.panelControl1.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.panelControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.panelControl1.Location = new System.Drawing.Point(0, 141);
//            this.panelControl1.Name = "panelControl1";
//            this.panelControl1.Size = new System.Drawing.Size(891, 365);
//            this.panelControl1.TabIndex = 1;
            // 
            // barButtonItem10
            // 
//            this.barButtonItem10.Caption = "Форма ur_mat";
//            this.barButtonItem10.Id = 5;
//            this.barButtonItem10.Name = "barButtonItem10";
            // 
            // ucFormDesigner
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.panelControl1);
//            this.Controls.Add(this.ribbonControl1);
//            this.Name = "ucFormDesigner";
//            this.Size = new System.Drawing.Size(891, 506);
//            this.Load += new System.EventHandler(this.ucDataEditorMain_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.panelControl1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
//        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
//        private DevExpress.XtraEditors.PanelControl panelControl1;
//        private DevExpress.XtraBars.BarButtonItem btnAccept;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup rpgNavigation;
//        private DevExpress.XtraBars.BarButtonItem btnCancel;
//        private DevExpress.XtraBars.BarButtonItem btnLoadDefault;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem12;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem14;
//        private DevExpress.XtraBars.BarButtonItem btnFieldsSettings;

//    }
//}
