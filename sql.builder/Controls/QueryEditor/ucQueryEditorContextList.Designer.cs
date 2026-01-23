//namespace sql.builder
//{
//    internal partial class ucQueryEditorContextList
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
//            this.gridControl = new DevExpress.XtraGrid.GridControl();
//            this.gridView = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.ribbonControl1 = new DevExpress.XtraBars.Ribbon.RibbonControl();
//            this.hdrContextName = new DevExpress.XtraBars.BarHeaderItem();
//            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
//            this.btnColsSelect = new DevExpress.XtraBars.BarButtonItem();
//            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
//            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
//            this.SuspendLayout();
            // 
            // gridControl
            // 
//            this.gridControl.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.gridControl.Location = new System.Drawing.Point(0, 0);
//            this.gridControl.MainView = this.gridView;
//            this.gridControl.Name = "gridControl";
//            this.gridControl.Size = new System.Drawing.Size(970, 486);
//            this.gridControl.TabIndex = 5;
//            this.gridControl.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.gridView});
            // 
            // gridView
            // 
//            this.gridView.GridControl = this.gridControl;
//            this.gridView.Name = "gridView";
//            this.gridView.OptionsBehavior.Editable = false;
//            this.gridView.OptionsSelection.MultiSelect = true;
//            this.gridView.OptionsView.ShowAutoFilterRow = true;
//            this.gridView.DoubleClick += new System.EventHandler(this.gridView_DoubleClick);
            // 
            // ribbonControl1
            // 
//            this.ribbonControl1.ExpandCollapseItem.Id = 0;
//            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.ribbonControl1.ExpandCollapseItem,
//            this.hdrContextName,
//            this.barButtonItem1,
//            this.btnColsSelect});
//            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
//            this.ribbonControl1.MaxItemId = 4;
//            this.ribbonControl1.Name = "ribbonControl1";
//            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
//            this.ribbonPage1});
//            this.ribbonControl1.Size = new System.Drawing.Size(970, 141);
//            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
            // 
            // hdrContextName
            // 
//            this.hdrContextName.Caption = "Текущий список: ";
//            this.hdrContextName.Id = 1;
//            this.hdrContextName.Name = "hdrContextName";
          //  this.hdrContextName.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.hdrContextName_ItemClick);
            // 
            // barButtonItem1
            // 
//            this.barButtonItem1.Caption = "Добавить выделенные";
//            this.barButtonItem1.Enabled = false;
//            this.barButtonItem1.Id = 2;
//            this.barButtonItem1.Name = "barButtonItem1";
//            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // btnColsSelect
            // 
//            this.btnColsSelect.Caption = "Настройка колонок";
//            this.btnColsSelect.Enabled = false;
//            this.btnColsSelect.Id = 3;
//            this.btnColsSelect.Name = "btnColsSelect";
//            this.btnColsSelect.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnColsSelect_ItemClick);
            // 
            // ribbonPage1
            // 
//            this.ribbonPage1.Groups.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPageGroup[] {
//            this.ribbonPageGroup1});
//            this.ribbonPage1.Name = "ribbonPage1";
//            this.ribbonPage1.Text = "Редактор схемы";
            // 
            // ribbonPageGroup1
            // 
//            this.ribbonPageGroup1.ItemLinks.Add(this.hdrContextName);
//            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem1);
//            this.ribbonPageGroup1.ItemLinks.Add(this.btnColsSelect);
//            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
//            this.ribbonPageGroup1.Text = "Списки контекстного добавления элементов";
            // 
            // ucQueryEditorContextList
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.ribbonControl1);
//            this.Controls.Add(this.gridControl);
//            this.Name = "ucQueryEditorContextList";
//            this.Size = new System.Drawing.Size(970, 486);
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraGrid.GridControl gridControl;
//        private DevExpress.XtraGrid.Views.Grid.GridView gridView;
//        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
//        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
//        private DevExpress.XtraBars.BarHeaderItem hdrContextName;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
//        private DevExpress.XtraBars.BarButtonItem btnColsSelect;
//    }
//}
