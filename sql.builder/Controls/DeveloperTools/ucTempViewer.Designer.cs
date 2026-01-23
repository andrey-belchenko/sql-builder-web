//namespace sql.builder
//{
//    partial class ucTempViewer
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
//            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
//            this.btn1 = new DevExpress.XtraEditors.SimpleButton();
//            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
//            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.comboBoxEdit1 = new DevExpress.XtraEditors.ComboBoxEdit();
//            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
//            this.Таблица = new DevExpress.XtraLayout.LayoutControlItem();
//            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
//            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
//            this.layoutControl1.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
//            this.SuspendLayout();
            // 
            // layoutControl1
            // 
//            this.layoutControl1.Controls.Add(this.btn1);
//            this.layoutControl1.Controls.Add(this.gridControl1);
//            this.layoutControl1.Controls.Add(this.comboBoxEdit1);
//            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
//            this.layoutControl1.Name = "layoutControl1";
//            this.layoutControl1.OptionsView.UseDefaultDragAndDropRendering = false;
//            this.layoutControl1.Root = this.layoutControlGroup1;
//            this.layoutControl1.Size = new System.Drawing.Size(1063, 409);
//            this.layoutControl1.TabIndex = 0;
//            this.layoutControl1.Text = "layoutControl1";
            // 
            // btn1
            // 
//            this.btn1.Location = new System.Drawing.Point(12, 12);
//            this.btn1.Name = "btn1";
//            this.btn1.Size = new System.Drawing.Size(78, 22);
//            this.btn1.StyleController = this.layoutControl1;
//            this.btn1.TabIndex = 6;
//            this.btn1.Text = "SQL";
//            this.btn1.Click += new System.EventHandler(this.btn1_Click);
            // 
            // gridControl1
            // 
//            this.gridControl1.Location = new System.Drawing.Point(12, 38);
//            this.gridControl1.MainView = this.gridView1;
//            this.gridControl1.Name = "gridControl1";
//            this.gridControl1.Size = new System.Drawing.Size(1039, 359);
//            this.gridControl1.TabIndex = 5;
//            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.gridView1});
            // 
            // gridView1
            // 
//            this.gridView1.GridControl = this.gridControl1;
//            this.gridView1.Name = "gridView1";
            // 
            // comboBoxEdit1
            // 
//            this.comboBoxEdit1.Location = new System.Drawing.Point(139, 12);
//            this.comboBoxEdit1.Name = "comboBoxEdit1";
//            this.comboBoxEdit1.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.comboBoxEdit1.Size = new System.Drawing.Size(912, 20);
//            this.comboBoxEdit1.StyleController = this.layoutControl1;
//            this.comboBoxEdit1.TabIndex = 4;
//            this.comboBoxEdit1.EditValueChanged += new System.EventHandler(this.comboBoxEdit1_EditValueChanged);
            // 
            // layoutControlGroup1
            // 
//            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
//            this.layoutControlGroup1.GroupBordersVisible = false;
//            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
//            this.Таблица,
//            this.layoutControlItem1,
//            this.layoutControlItem2});
//            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
//            this.layoutControlGroup1.Name = "layoutControlGroup1";
//            this.layoutControlGroup1.Size = new System.Drawing.Size(1063, 409);
//            this.layoutControlGroup1.TextVisible = false;
            // 
            // Таблица
            // 
//            this.Таблица.Control = this.comboBoxEdit1;
//            this.Таблица.Location = new System.Drawing.Point(82, 0);
//            this.Таблица.Name = "Таблица";
//            this.Таблица.Size = new System.Drawing.Size(961, 26);
//            this.Таблица.TextSize = new System.Drawing.Size(42, 13);
            // 
            // layoutControlItem1
            // 
//            this.layoutControlItem1.Control = this.gridControl1;
//            this.layoutControlItem1.Location = new System.Drawing.Point(0, 26);
//            this.layoutControlItem1.Name = "layoutControlItem1";
//            this.layoutControlItem1.Size = new System.Drawing.Size(1043, 363);
//            this.layoutControlItem1.TextSize = new System.Drawing.Size(0, 0);
//            this.layoutControlItem1.TextVisible = false;
            // 
            // layoutControlItem2
            // 
//            this.layoutControlItem2.Control = this.btn1;
//            this.layoutControlItem2.Location = new System.Drawing.Point(0, 0);
//            this.layoutControlItem2.Name = "layoutControlItem2";
//            this.layoutControlItem2.Size = new System.Drawing.Size(82, 26);
//            this.layoutControlItem2.TextSize = new System.Drawing.Size(0, 0);
//            this.layoutControlItem2.TextVisible = false;
            // 
            // ucTempViewer
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.layoutControl1);
//            this.Name = "ucTempViewer";
//            this.Size = new System.Drawing.Size(1063, 409);
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
//            this.layoutControl1.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.comboBoxEdit1.Properties)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.Таблица)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraLayout.LayoutControl layoutControl1;
//        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
//        private DevExpress.XtraGrid.GridControl gridControl1;
//        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
//        private DevExpress.XtraEditors.ComboBoxEdit comboBoxEdit1;
//        private DevExpress.XtraLayout.LayoutControlItem Таблица;
//        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
//        private DevExpress.XtraEditors.SimpleButton btn1;
//        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
//    }
//}
