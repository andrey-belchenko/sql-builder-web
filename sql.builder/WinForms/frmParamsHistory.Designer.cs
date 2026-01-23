//namespace sql.builder.WinForms
//{
//    partial class frmParamsHistory
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
//            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
//            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
//            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
//            this.pFooter = new DevExpress.XtraEditors.PanelControl();
//            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
//            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
//            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).BeginInit();
//            this.pFooter.SuspendLayout();
//            this.SuspendLayout();
            // 
            // gridControl1
            // 
//            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.gridControl1.Location = new System.Drawing.Point(0, 0);
//            this.gridControl1.MainView = this.gridView1;
//            this.gridControl1.Name = "gridControl1";
//            this.gridControl1.Size = new System.Drawing.Size(708, 465);
//            this.gridControl1.TabIndex = 0;
//            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.gridView1});
            // 
            // gridView1
            // 
//            this.gridView1.GridControl = this.gridControl1;
//            this.gridView1.Name = "gridView1";
//            this.gridView1.OptionsBehavior.Editable = false;
//            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.gridView1.OptionsView.ColumnAutoWidth = false;
//            this.gridView1.OptionsView.ShowAutoFilterRow = true;
//            this.gridView1.OptionsView.ShowGroupPanel = false;
//            this.gridView1.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
//            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // memoEdit1
            // 
//            this.memoEdit1.Dock = System.Windows.Forms.DockStyle.Right;
//            this.memoEdit1.Location = new System.Drawing.Point(713, 0);
//            this.memoEdit1.Name = "memoEdit1";
//            this.memoEdit1.Size = new System.Drawing.Size(397, 465);
//            this.memoEdit1.TabIndex = 1;
            // 
            // splitterControl1
            // 
//            this.splitterControl1.Dock = System.Windows.Forms.DockStyle.Right;
//            this.splitterControl1.Location = new System.Drawing.Point(708, 0);
//            this.splitterControl1.Name = "splitterControl1";
//            this.splitterControl1.Size = new System.Drawing.Size(5, 465);
//            this.splitterControl1.TabIndex = 2;
//            this.splitterControl1.TabStop = false;
            // 
            // pFooter
            // 
//            this.pFooter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.pFooter.Controls.Add(this.labelControl1);
//            this.pFooter.Controls.Add(this.btnCancel);
//            this.pFooter.Controls.Add(this.btnAccept);
//            this.pFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.pFooter.Location = new System.Drawing.Point(0, 465);
//            this.pFooter.Name = "pFooter";
//            this.pFooter.Size = new System.Drawing.Size(1110, 34);
//            this.pFooter.TabIndex = 3;
            // 
            // labelControl1
            // 
//            this.labelControl1.Location = new System.Drawing.Point(13, 7);
//            this.labelControl1.Name = "labelControl1";
//            this.labelControl1.Size = new System.Drawing.Size(116, 13);
//            this.labelControl1.TabIndex = 2;
//            this.labelControl1.Text = "Показано записей: 100";
            // 
            // btnCancel
            // 
//            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnCancel.Location = new System.Drawing.Point(1025, 5);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(75, 24);
//            this.btnCancel.TabIndex = 1;
//            this.btnCancel.Text = "Отмена";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
//            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnAccept.Enabled = false;
//            this.btnAccept.Location = new System.Drawing.Point(934, 5);
//            this.btnAccept.Name = "btnAccept";
//            this.btnAccept.Size = new System.Drawing.Size(75, 24);
//            this.btnAccept.TabIndex = 0;
//            this.btnAccept.Text = "Готово";
//            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // frmParamsHistory
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(1110, 499);
//            this.Controls.Add(this.gridControl1);
//            this.Controls.Add(this.splitterControl1);
//            this.Controls.Add(this.memoEdit1);
//            this.Controls.Add(this.pFooter);
//            this.Name = "frmParamsHistory";
//            this.Text = "Загрузка параметров отчёта из истории формирования";
//            this.UserSettings.SaveFormSize = false;
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).EndInit();
//            this.pFooter.ResumeLayout(false);
//            this.pFooter.PerformLayout();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraGrid.GridControl gridControl1;
//        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
//        private DevExpress.XtraEditors.MemoEdit memoEdit1;
//        private DevExpress.XtraEditors.SplitterControl splitterControl1;
//        private DevExpress.XtraEditors.PanelControl pFooter;
//        private DevExpress.XtraEditors.SimpleButton btnCancel;
//        private DevExpress.XtraEditors.SimpleButton btnAccept;
//        private DevExpress.XtraEditors.LabelControl labelControl1;
//    }
//}
