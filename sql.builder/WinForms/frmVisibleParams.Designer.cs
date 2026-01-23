//namespace sql.builder.WinForms
//{
//    internal partial class frmVisibleParams
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
//            this.tlParamsVisible = new DevExpress.XtraTreeList.TreeList();
//            this.colParamVisible = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.rceParamVisible = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.colParamTitle = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).BeginInit();
//            this.pFooter.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.tlParamsVisible)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceParamVisible)).BeginInit();
//            this.SuspendLayout();
            // 
            // pFooter
            // 
//            this.pFooter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.pFooter.Controls.Add(this.btnCancel);
//            this.pFooter.Controls.Add(this.btnAccept);
//            this.pFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.pFooter.Location = new System.Drawing.Point(0, 473);
//            this.pFooter.Name = "pFooter";
//            this.pFooter.Size = new System.Drawing.Size(383, 34);
//            this.pFooter.TabIndex = 0;
            // 
            // btnCancel
            // 
//            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnCancel.Location = new System.Drawing.Point(298, 5);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(75, 24);
//            this.btnCancel.TabIndex = 1;
//            this.btnCancel.Text = "Отмена";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
//            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnAccept.Location = new System.Drawing.Point(207, 5);
//            this.btnAccept.Name = "btnAccept";
//            this.btnAccept.Size = new System.Drawing.Size(75, 24);
//            this.btnAccept.TabIndex = 0;
//            this.btnAccept.Text = "Готово";
//            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // tlParamsVisible
            // 
//            this.tlParamsVisible.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
//            this.colParamVisible,
//            this.colParamTitle});
//            this.tlParamsVisible.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tlParamsVisible.ImageIndexFieldName = "";
//            this.tlParamsVisible.KeyFieldName = "name";
//            this.tlParamsVisible.Location = new System.Drawing.Point(0, 0);
//            this.tlParamsVisible.Name = "tlParamsVisible";
//            this.tlParamsVisible.OptionsBehavior.EnableFiltering = true;
//            this.tlParamsVisible.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.tlParamsVisible.OptionsSelection.MultiSelect = true;
//            this.tlParamsVisible.OptionsView.ShowAutoFilterRow = true;
//            this.tlParamsVisible.OptionsView.ShowColumns = false;
//            this.tlParamsVisible.OptionsView.ShowIndicator = false;
//            this.tlParamsVisible.ParentFieldName = "parent_name";
//            this.tlParamsVisible.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rceParamVisible});
//            this.tlParamsVisible.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowAlways;
//            this.tlParamsVisible.Size = new System.Drawing.Size(383, 473);
//            this.tlParamsVisible.TabIndex = 1;
            // 
            // colParamVisible
            // 
//            this.colParamVisible.AllNodesSummary = true;
//            this.colParamVisible.ColumnEdit = this.rceParamVisible;
//            this.colParamVisible.FieldName = "check";
//            this.colParamVisible.MinWidth = 33;
//            this.colParamVisible.Name = "colParamVisible";
//            this.colParamVisible.OptionsColumn.FixedWidth = true;
//            this.colParamVisible.Visible = true;
//            this.colParamVisible.VisibleIndex = 0;
//            this.colParamVisible.Width = 60;
            // 
            // rceParamVisible
            // 
//            this.rceParamVisible.AutoHeight = false;
//            this.rceParamVisible.Caption = "Check";
//            this.rceParamVisible.Name = "rceParamVisible";
//            this.rceParamVisible.EditValueChanged += new System.EventHandler(this.rceParamVisible_EditValueChanged);
            // 
            // colParamTitle
            // 
//            this.colParamTitle.Caption = "Параметр";
//            this.colParamTitle.FieldName = "title";
//            this.colParamTitle.Name = "colParamTitle";
//            this.colParamTitle.OptionsColumn.AllowEdit = false;
//            this.colParamTitle.Visible = true;
//            this.colParamTitle.VisibleIndex = 1;
            // 
            // frmVisibleParams
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(383, 507);
//            this.Controls.Add(this.tlParamsVisible);
//            this.Controls.Add(this.pFooter);
//            this.DoubleBuffered = true;
//            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.SizableToolWindow;
//            this.Name = "frmVisibleParams";
//            this.Text = "Выбор параметров отчёта";
//            this.UserSettings.SaveFormSize = false;
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).EndInit();
//            this.pFooter.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.tlParamsVisible)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceParamVisible)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraEditors.PanelControl pFooter;
//        private DevExpress.XtraEditors.SimpleButton btnCancel;
//        private DevExpress.XtraEditors.SimpleButton btnAccept;
//        private DevExpress.XtraTreeList.TreeList tlParamsVisible;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colParamVisible;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceParamVisible;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colParamTitle;
//    }
//}
