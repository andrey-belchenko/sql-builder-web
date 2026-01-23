//namespace sql.builder.Controls
//{
//    partial class ucSaveLoadForm
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

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.pFooter = new DevExpress.XtraEditors.PanelControl();
//            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
//            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
//            this.grSettings = new DevExpress.XtraGrid.GridControl();
//            this.viewSettings = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colSettingsName = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.pSettingName = new DevExpress.XtraEditors.PanelControl();
//            this.lSettingName = new DevExpress.XtraEditors.LabelControl();
//            this.teSettingName = new DevExpress.XtraEditors.TextEdit();
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).BeginInit();
//            this.pFooter.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.grSettings)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewSettings)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pSettingName)).BeginInit();
//            this.pSettingName.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.teSettingName.Properties)).BeginInit();
//            this.SuspendLayout();
            // 
            // pFooter
            // 
//            this.pFooter.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.pFooter.Controls.Add(this.btnCancel);
//            this.pFooter.Controls.Add(this.btnAccept);
//            this.pFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.pFooter.Location = new System.Drawing.Point(0, 222);
//            this.pFooter.Name = "pFooter";
//            this.pFooter.Size = new System.Drawing.Size(276, 34);
//            this.pFooter.TabIndex = 1;
            // 
            // btnCancel
            // 
//            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnCancel.Location = new System.Drawing.Point(191, 5);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(75, 24);
//            this.btnCancel.TabIndex = 1;
//            this.btnCancel.Text = "Отмена";
            // 
            // btnAccept
            // 
//            this.btnAccept.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
//            this.btnAccept.Location = new System.Drawing.Point(100, 5);
//            this.btnAccept.Name = "btnAccept";
//            this.btnAccept.Size = new System.Drawing.Size(75, 24);
//            this.btnAccept.TabIndex = 0;
//            this.btnAccept.Text = "Готово";
            // 
            // grSettings
            // 
//            this.grSettings.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.grSettings.Location = new System.Drawing.Point(0, 0);
//            this.grSettings.MainView = this.viewSettings;
//            this.grSettings.Name = "grSettings";
//            this.grSettings.Size = new System.Drawing.Size(276, 222);
//            this.grSettings.TabIndex = 2;
//            this.grSettings.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewSettings});
            // 
            // viewSettings
            // 
//            this.viewSettings.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colSettingsName});
//            this.viewSettings.GridControl = this.grSettings;
//            this.viewSettings.Name = "viewSettings";
//            this.viewSettings.OptionsBehavior.Editable = false;
//            this.viewSettings.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.viewSettings.OptionsView.ShowColumnHeaders = false;
//            this.viewSettings.OptionsView.ShowGroupPanel = false;
//            this.viewSettings.OptionsView.ShowIndicator = false;
//            this.viewSettings.OptionsView.ShowViewCaption = true;
//            this.viewSettings.ViewCaption = "Доступные настройки";
            // 
            // colSettingsName
            // 
//            this.colSettingsName.Caption = "Настройка";
//            this.colSettingsName.FieldName = "title";
//            this.colSettingsName.Name = "colSettingsName";
//            this.colSettingsName.Visible = true;
//            this.colSettingsName.VisibleIndex = 0;
            // 
            // pSettingName
            // 
//            this.pSettingName.Controls.Add(this.lSettingName);
//            this.pSettingName.Controls.Add(this.teSettingName);
//            this.pSettingName.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.pSettingName.Location = new System.Drawing.Point(0, 181);
//            this.pSettingName.Name = "pSettingName";
//            this.pSettingName.Size = new System.Drawing.Size(276, 41);
//            this.pSettingName.TabIndex = 2;
            // 
            // lSettingName
            // 
//            this.lSettingName.Location = new System.Drawing.Point(14, 15);
//            this.lSettingName.Name = "lSettingName";
//            this.lSettingName.Size = new System.Drawing.Size(79, 13);
//            this.lSettingName.TabIndex = 1;
//            this.lSettingName.Text = "Имя настройки:";
            // 
            // teSettingName
            // 
//            this.teSettingName.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
//            | System.Windows.Forms.AnchorStyles.Right)));
//            this.teSettingName.Location = new System.Drawing.Point(99, 12);
//            this.teSettingName.Name = "teSettingName";
//            this.teSettingName.Size = new System.Drawing.Size(168, 20);
//            this.teSettingName.TabIndex = 0;
            // 
            // ucSaveLoadForm
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.pSettingName);
//            this.Controls.Add(this.grSettings);
//            this.Controls.Add(this.pFooter);
//            this.DoubleBuffered = true;
//            this.MinimumSize = new System.Drawing.Size(200, 200);
//            this.Name = "ucSaveLoadForm";
//            this.Size = new System.Drawing.Size(276, 256);
//            ((System.ComponentModel.ISupportInitialize)(this.pFooter)).EndInit();
//            this.pFooter.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.grSettings)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewSettings)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pSettingName)).EndInit();
//            this.pSettingName.ResumeLayout(false);
//            this.pSettingName.PerformLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.teSettingName.Properties)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraEditors.PanelControl pFooter;
//        private DevExpress.XtraEditors.SimpleButton btnCancel;
//        private DevExpress.XtraEditors.SimpleButton btnAccept;
//        private DevExpress.XtraGrid.GridControl grSettings;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewSettings;
//        private DevExpress.XtraGrid.Columns.GridColumn colSettingsName;
//        private DevExpress.XtraEditors.PanelControl pSettingName;
//        private DevExpress.XtraEditors.LabelControl lSettingName;
//        private DevExpress.XtraEditors.TextEdit teSettingName;
//    }
//}
