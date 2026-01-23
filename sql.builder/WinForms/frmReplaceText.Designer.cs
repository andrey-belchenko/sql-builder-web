//namespace sql.builder.WinForms
//{
//    partial class frmReplaceText
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
//            this.pFooter = new DevExpress.XtraEditors.PanelControl();
//            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
//            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
//            this.layoutControl1 = new DevExpress.XtraLayout.LayoutControl();
//            this.layoutControlGroup1 = new DevExpress.XtraLayout.LayoutControlGroup();
//            this.layoutControlItem1 = new DevExpress.XtraLayout.LayoutControlItem();
//            this.layoutControlItem2 = new DevExpress.XtraLayout.LayoutControlItem();
//            this.checkEdit1 = new DevExpress.XtraEditors.CheckEdit();
//            this.layoutControlItem3 = new DevExpress.XtraLayout.LayoutControlItem();
//            this.memoExEdit1 = new DevExpress.XtraEditors.TextEdit();
//            this.memoExEdit2 = new DevExpress.XtraEditors.TextEdit();
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).BeginInit();
//            this.pFooter.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).BeginInit();
//            this.layoutControl1.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.memoExEdit1.Properties)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.memoExEdit2.Properties)).BeginInit();
//            this.SuspendLayout();
            // 
            // pFooter
            // 
//            this.pFooter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.pFooter.Controls.Add(this.btnCancel);
//            this.pFooter.Controls.Add(this.btnAccept);
//            this.pFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.pFooter.Location = new System.Drawing.Point(0, 100);
//            this.pFooter.Name = "pFooter";
//            this.pFooter.Size = new System.Drawing.Size(357, 34);
//            this.pFooter.TabIndex = 1;
            // 
            // btnCancel
            // 
//            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnCancel.Location = new System.Drawing.Point(272, 5);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(75, 24);
//            this.btnCancel.TabIndex = 3;
//            this.btnCancel.Text = "Отмена";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
//            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnAccept.Location = new System.Drawing.Point(181, 5);
//            this.btnAccept.Name = "btnAccept";
//            this.btnAccept.Size = new System.Drawing.Size(75, 24);
//            this.btnAccept.TabIndex = 2;
//            this.btnAccept.Text = "Заменить";
//            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // layoutControl1
            // 
//            this.layoutControl1.Controls.Add(this.checkEdit1);
//            this.layoutControl1.Controls.Add(this.memoExEdit1);
//            this.layoutControl1.Controls.Add(this.memoExEdit2);
//            this.layoutControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.layoutControl1.Location = new System.Drawing.Point(0, 0);
//            this.layoutControl1.Name = "layoutControl1";
//            this.layoutControl1.Root = this.layoutControlGroup1;
//            this.layoutControl1.Size = new System.Drawing.Size(357, 100);
//            this.layoutControl1.TabIndex = 2;
//            this.layoutControl1.Text = "layoutControl1";
            // 
            // layoutControlGroup1
            // 
//            this.layoutControlGroup1.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
//            this.layoutControlGroup1.GroupBordersVisible = false;
//            this.layoutControlGroup1.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
//            this.layoutControlItem1,
//            this.layoutControlItem2,
//            this.layoutControlItem3});
//            this.layoutControlGroup1.Location = new System.Drawing.Point(0, 0);
//            this.layoutControlGroup1.Name = "Root";
//            this.layoutControlGroup1.Size = new System.Drawing.Size(357, 100);
//            this.layoutControlGroup1.TextVisible = false;
            // 
            // layoutControlItem1
            // 
//            this.layoutControlItem1.Control = this.memoExEdit1;
//            this.layoutControlItem1.Location = new System.Drawing.Point(0, 23);
//            this.layoutControlItem1.Name = "layoutControlItem1";
//            this.layoutControlItem1.Size = new System.Drawing.Size(337, 24);
//            this.layoutControlItem1.Text = "Исходный текст";
//            this.layoutControlItem1.TextSize = new System.Drawing.Size(83, 13);
            // 
            // layoutControlItem2
            // 
//            this.layoutControlItem2.Control = this.memoExEdit2;
//            this.layoutControlItem2.CustomizationFormText = "Текст";
//            this.layoutControlItem2.Location = new System.Drawing.Point(0, 47);
//            this.layoutControlItem2.Name = "layoutControlItem2";
//            this.layoutControlItem2.Size = new System.Drawing.Size(337, 33);
//            this.layoutControlItem2.Text = "Заменить на";
//            this.layoutControlItem2.TextSize = new System.Drawing.Size(83, 13);
            // 
            // checkEdit1
            // 
//            this.checkEdit1.EditValue = true;
//            this.checkEdit1.Location = new System.Drawing.Point(12, 12);
//            this.checkEdit1.Name = "checkEdit1";
//            this.checkEdit1.Properties.Caption = "Учитывать регистр";
//            this.checkEdit1.Size = new System.Drawing.Size(333, 19);
//            this.checkEdit1.StyleController = this.layoutControl1;
//            this.checkEdit1.TabIndex = 4;
            // 
            // layoutControlItem3
            // 
//            this.layoutControlItem3.Control = this.checkEdit1;
//            this.layoutControlItem3.Location = new System.Drawing.Point(0, 0);
//            this.layoutControlItem3.Name = "layoutControlItem3";
//            this.layoutControlItem3.Size = new System.Drawing.Size(337, 23);
//            this.layoutControlItem3.TextSize = new System.Drawing.Size(0, 0);
//            this.layoutControlItem3.TextVisible = false;
            // 
            // memoExEdit1
            // 
//            this.memoExEdit1.Location = new System.Drawing.Point(99, 35);
//            this.memoExEdit1.Name = "memoExEdit1";
//            this.memoExEdit1.Size = new System.Drawing.Size(246, 20);
//            this.memoExEdit1.StyleController = this.layoutControl1;
//            this.memoExEdit1.TabIndex = 0;
            // 
            // memoExEdit2
            // 
//            this.memoExEdit2.Location = new System.Drawing.Point(99, 59);
//            this.memoExEdit2.Name = "memoExEdit2";
//            this.memoExEdit2.Size = new System.Drawing.Size(246, 20);
//            this.memoExEdit2.StyleController = this.layoutControl1;
//            this.memoExEdit2.TabIndex = 1;
            // 
            // frmReplaceText
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(357, 134);
//            this.Controls.Add(this.layoutControl1);
//            this.Controls.Add(this.pFooter);
//            this.Name = "frmReplaceText";
//            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
//            this.Text = "Замена текста";
//            this.Activated += new System.EventHandler(this.frmReplaceText_Activated);
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).EndInit();
//            this.pFooter.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControl1)).EndInit();
//            this.layoutControl1.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlGroup1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.checkEdit1.Properties)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.layoutControlItem3)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.memoExEdit1.Properties)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.memoExEdit2.Properties)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraEditors.PanelControl pFooter;
//        private DevExpress.XtraEditors.SimpleButton btnCancel;
//        private DevExpress.XtraEditors.SimpleButton btnAccept;
//        private DevExpress.XtraLayout.LayoutControl layoutControl1;
//        private DevExpress.XtraLayout.LayoutControlGroup layoutControlGroup1;
//        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem1;
//        private DevExpress.XtraEditors.CheckEdit checkEdit1;
//        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem2;
//        private DevExpress.XtraLayout.LayoutControlItem layoutControlItem3;
//        private DevExpress.XtraEditors.TextEdit memoExEdit1;
//        private DevExpress.XtraEditors.TextEdit memoExEdit2;
//    }
//}
