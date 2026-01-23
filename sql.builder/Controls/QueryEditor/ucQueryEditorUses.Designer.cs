//namespace sql.builder
//{
//    internal partial class ucQueriesEditorUses
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
//            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
//            this.ribbonPage1 = new DevExpress.XtraBars.Ribbon.RibbonPage();
//            this.ribbonPageGroup1 = new DevExpress.XtraBars.Ribbon.RibbonPageGroup();
//            this.gridControl2 = new DevExpress.XtraGrid.GridControl();
//            this.gridView2 = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.gridColumn1 = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.gridColumn5 = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.gridColumn2 = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.gridColumn3 = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.gridColumn4 = new DevExpress.XtraGrid.Columns.GridColumn();
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).BeginInit();
//            this.SuspendLayout();
            // 
            // ribbonControl1
            // 
//            this.ribbonControl1.ExpandCollapseItem.Id = 0;
//            this.ribbonControl1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.ribbonControl1.ExpandCollapseItem,
//            this.barButtonItem2});
//            this.ribbonControl1.Location = new System.Drawing.Point(0, 0);
//            this.ribbonControl1.MaxItemId = 3;
//            this.ribbonControl1.Name = "ribbonControl1";
//            this.ribbonControl1.Pages.AddRange(new DevExpress.XtraBars.Ribbon.RibbonPage[] {
//            this.ribbonPage1});
//            this.ribbonControl1.ShowApplicationButton = DevExpress.Utils.DefaultBoolean.False;
//            this.ribbonControl1.ShowToolbarCustomizeItem = false;
//            this.ribbonControl1.Size = new System.Drawing.Size(1194, 116);
//            this.ribbonControl1.Toolbar.ShowCustomizeItem = false;
//            this.ribbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden;
            // 
            // barButtonItem2
            // 
//            this.barButtonItem2.Caption = "Обновить";
//            this.barButtonItem2.Id = 2;
//            this.barButtonItem2.Name = "barButtonItem2";
//            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
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
//            this.ribbonPageGroup1.ItemLinks.Add(this.barButtonItem2);
//            this.ribbonPageGroup1.Name = "ribbonPageGroup1";
//            this.ribbonPageGroup1.Text = "Менеджер ссылок";
            // 
            // gridControl2
            // 
//            this.gridControl2.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.gridControl2.Location = new System.Drawing.Point(0, 116);
//            this.gridControl2.MainView = this.gridView2;
//            this.gridControl2.MenuManager = this.ribbonControl1;
//            this.gridControl2.Name = "gridControl2";
//            this.gridControl2.Size = new System.Drawing.Size(1194, 335);
//            this.gridControl2.TabIndex = 2;
//            this.gridControl2.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.gridView2});
            // 
            // gridView2
            // 
//            this.gridView2.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.gridColumn1,
//            this.gridColumn5,
//            this.gridColumn2,
//            this.gridColumn3,
//            this.gridColumn4});
//            this.gridView2.GridControl = this.gridControl2;
//            this.gridView2.Name = "gridView2";
//            this.gridView2.OptionsBehavior.Editable = false;
//            this.gridView2.OptionsView.RowAutoHeight = true;
//            this.gridView2.OptionsView.ShowAutoFilterRow = true;
//            this.gridView2.OptionsView.ShowGroupPanel = false;
//            this.gridView2.DoubleClick += new System.EventHandler(this.gridView2_DoubleClick);
            // 
            // gridColumn1
            // 
//            this.gridColumn1.Caption = "Тип элемента";
//            this.gridColumn1.FieldName = "etype";
//            this.gridColumn1.Name = "gridColumn1";
//            this.gridColumn1.Visible = true;
//            this.gridColumn1.VisibleIndex = 0;
            // 
            // gridColumn5
            // 
//            this.gridColumn5.Caption = "Узел";
//            this.gridColumn5.FieldName = "node_name";
//            this.gridColumn5.Name = "gridColumn5";
//            this.gridColumn5.Visible = true;
//            this.gridColumn5.VisibleIndex = 2;
            // 
            // gridColumn2
            // 
//            this.gridColumn2.Caption = "Элемент";
//            this.gridColumn2.FieldName = "etext";
//            this.gridColumn2.Name = "gridColumn2";
//            this.gridColumn2.Visible = true;
//            this.gridColumn2.VisibleIndex = 1;
            // 
            // gridColumn3
            // 
//            this.gridColumn3.Caption = "Текст";
//            this.gridColumn3.FieldName = "text";
//            this.gridColumn3.Name = "gridColumn3";
//            this.gridColumn3.Visible = true;
//            this.gridColumn3.VisibleIndex = 4;
            // 
            // gridColumn4
            // 
//            this.gridColumn4.Caption = "Атрибут";
//            this.gridColumn4.FieldName = "attr";
//            this.gridColumn4.Name = "gridColumn4";
//            this.gridColumn4.Visible = true;
//            this.gridColumn4.VisibleIndex = 3;
            // 
            // ucQueriesEditorUses
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.gridControl2);
//            this.Controls.Add(this.ribbonControl1);
//            this.Name = "ucQueriesEditorUses";
//            this.Size = new System.Drawing.Size(1194, 451);
//            ((System.ComponentModel.ISupportInitialize)(this.ribbonControl1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView2)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraBars.Ribbon.RibbonControl ribbonControl1;
//        private DevExpress.XtraBars.Ribbon.RibbonPage ribbonPage1;
//        private DevExpress.XtraBars.Ribbon.RibbonPageGroup ribbonPageGroup1;
//        private DevExpress.XtraGrid.GridControl gridControl2;
//        private DevExpress.XtraGrid.Views.Grid.GridView gridView2;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
//        private DevExpress.XtraGrid.Columns.GridColumn gridColumn1;
//        private DevExpress.XtraGrid.Columns.GridColumn gridColumn2;
//        private DevExpress.XtraGrid.Columns.GridColumn gridColumn3;
//        private DevExpress.XtraGrid.Columns.GridColumn gridColumn4;
//        private DevExpress.XtraGrid.Columns.GridColumn gridColumn5;
//    }
//}
