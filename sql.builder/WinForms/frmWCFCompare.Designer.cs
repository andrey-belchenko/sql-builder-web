//namespace sql.builder.WinForms
//{
//    partial class frmWCFCompare
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
//            this.layout = new DevExpress.XtraLayout.LayoutControl();
//            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
//            this.btnStart = new DevExpress.XtraEditors.SimpleButton();
//            this.grLog = new DevExpress.XtraGrid.GridControl();
//            this.viewLog = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colTime = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colText = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.beClientPath = new DevExpress.XtraEditors.ButtonEdit();
//            this.lgMain = new DevExpress.XtraLayout.LayoutControlGroup();
//            this.liClientPath = new DevExpress.XtraLayout.LayoutControlItem();
//            this.liLog = new DevExpress.XtraLayout.LayoutControlItem();
//            this.liStart = new DevExpress.XtraLayout.LayoutControlItem();
//            this.liCancel = new DevExpress.XtraLayout.LayoutControlItem();
//            ((System.ComponentModel.ISupportInitialize)(this.layout)).BeginInit();
//            this.layout.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.grLog)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewLog)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.beClientPath.Properties)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.lgMain)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.liClientPath)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.liLog)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.liStart)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.liCancel)).BeginInit();
//            this.SuspendLayout();
            // 
            // layout
            // 
//            this.layout.Controls.Add(this.btnCancel);
//            this.layout.Controls.Add(this.btnStart);
//            this.layout.Controls.Add(this.grLog);
//            this.layout.Controls.Add(this.beClientPath);
//            this.layout.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.layout.Location = new System.Drawing.Point(0, 0);
//            this.layout.Name = "layout";
//            this.layout.OptionsCustomizationForm.DesignTimeCustomizationFormPositionAndSize = new System.Drawing.Rectangle(431, 296, 567, 455);
//            this.layout.Root = this.lgMain;
//            this.layout.Size = new System.Drawing.Size(626, 495);
//            this.layout.TabIndex = 0;
//            this.layout.Text = "layoutControl1";
            // 
            // btnCancel
            // 
//            this.btnCancel.Enabled = false;
//            this.btnCancel.Location = new System.Drawing.Point(315, 436);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Size = new System.Drawing.Size(299, 47);
//            this.btnCancel.StyleController = this.layout;
//            this.btnCancel.TabIndex = 7;
//            this.btnCancel.Text = "Отмена";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnStart
            // 
//            this.btnStart.Location = new System.Drawing.Point(12, 436);
//            this.btnStart.Name = "btnStart";
//            this.btnStart.Size = new System.Drawing.Size(299, 47);
//            this.btnStart.StyleController = this.layout;
//            this.btnStart.TabIndex = 6;
//            this.btnStart.Text = "Начать";
//            this.btnStart.Click += new System.EventHandler(this.btnStart_Click);
            // 
            // grLog
            // 
//            this.grLog.Location = new System.Drawing.Point(12, 36);
//            this.grLog.MainView = this.viewLog;
//            this.grLog.Name = "grLog";
//            this.grLog.Size = new System.Drawing.Size(602, 396);
//            this.grLog.TabIndex = 5;
//            this.grLog.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.viewLog});
            // 
            // viewLog
            // 
//            this.viewLog.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colTime,
//            this.colText});
//            this.viewLog.GridControl = this.grLog;
//            this.viewLog.Name = "viewLog";
//            this.viewLog.OptionsView.ShowGroupPanel = false;
//            this.viewLog.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
//            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colTime, DevExpress.Data.ColumnSortOrder.Descending)});
            // 
            // colTime
            // 
//            this.colTime.Caption = "Время";
//            this.colTime.DisplayFormat.FormatString = "HH:mm:ss";
//            this.colTime.DisplayFormat.FormatType = DevExpress.Utils.FormatType.DateTime;
//            this.colTime.FieldName = "time";
//            this.colTime.Name = "colTime";
//            this.colTime.OptionsColumn.FixedWidth = true;
//            this.colTime.Visible = true;
//            this.colTime.VisibleIndex = 0;
            // 
            // colText
            // 
//            this.colText.Caption = "Информация";
//            this.colText.FieldName = "text";
//            this.colText.Name = "colText";
//            this.colText.Visible = true;
//            this.colText.VisibleIndex = 1;
//            this.colText.Width = 668;
            // 
            // beClientPath
            // 
//            this.beClientPath.Location = new System.Drawing.Point(195, 12);
//            this.beClientPath.Name = "beClientPath";
//            this.beClientPath.Properties.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton()});
//            this.beClientPath.Properties.ReadOnly = true;
//            this.beClientPath.Size = new System.Drawing.Size(419, 20);
//            this.beClientPath.StyleController = this.layout;
//            this.beClientPath.TabIndex = 4;
//            this.beClientPath.ButtonClick += new DevExpress.XtraEditors.Controls.ButtonPressedEventHandler(this.beClientPath_ButtonClick);
            // 
            // lgMain
            // 
//            this.lgMain.EnableIndentsWithoutBorders = DevExpress.Utils.DefaultBoolean.True;
//            this.lgMain.GroupBordersVisible = false;
//            this.lgMain.Items.AddRange(new DevExpress.XtraLayout.BaseLayoutItem[] {
//            this.liClientPath,
//            this.liLog,
//            this.liStart,
//            this.liCancel});
//            this.lgMain.Location = new System.Drawing.Point(0, 0);
//            this.lgMain.Name = "Root";
//            this.lgMain.Size = new System.Drawing.Size(626, 495);
//            this.lgMain.TextVisible = false;
            // 
            // liClientPath
            // 
//            this.liClientPath.Control = this.beClientPath;
//            this.liClientPath.Location = new System.Drawing.Point(0, 0);
//            this.liClientPath.Name = "liClientPath";
//            this.liClientPath.Size = new System.Drawing.Size(606, 24);
//            this.liClientPath.Text = "Путь к приложению для сравнения";
//            this.liClientPath.TextSize = new System.Drawing.Size(180, 13);
            // 
            // liLog
            // 
//            this.liLog.Control = this.grLog;
//            this.liLog.Location = new System.Drawing.Point(0, 24);
//            this.liLog.Name = "liLog";
//            this.liLog.Size = new System.Drawing.Size(606, 400);
//            this.liLog.TextSize = new System.Drawing.Size(0, 0);
//            this.liLog.TextVisible = false;
            // 
            // liStart
            // 
//            this.liStart.Control = this.btnStart;
//            this.liStart.Location = new System.Drawing.Point(0, 424);
//            this.liStart.MinSize = new System.Drawing.Size(50, 26);
//            this.liStart.Name = "liStart";
//            this.liStart.Size = new System.Drawing.Size(303, 51);
//            this.liStart.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
//            this.liStart.TextSize = new System.Drawing.Size(0, 0);
//            this.liStart.TextVisible = false;
            // 
            // liCancel
            // 
//            this.liCancel.Control = this.btnCancel;
//            this.liCancel.Location = new System.Drawing.Point(303, 424);
//            this.liCancel.MinSize = new System.Drawing.Size(51, 26);
//            this.liCancel.Name = "liCancel";
//            this.liCancel.Size = new System.Drawing.Size(303, 51);
//            this.liCancel.SizeConstraintsType = DevExpress.XtraLayout.SizeConstraintsType.Custom;
//            this.liCancel.TextSize = new System.Drawing.Size(0, 0);
//            this.liCancel.TextVisible = false;
            // 
            // frmWCFCompare
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(626, 495);
//            this.Controls.Add(this.layout);
//            this.Name = "frmWCFCompare";
//            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
//            this.Text = "Сравнение отчётов";
//            this.Load += new System.EventHandler(this.frmWCFCompare_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.layout)).EndInit();
//            this.layout.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.grLog)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.viewLog)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.beClientPath.Properties)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.lgMain)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.liClientPath)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.liLog)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.liStart)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.liCancel)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraLayout.LayoutControl layout;
//        private DevExpress.XtraEditors.ButtonEdit beClientPath;
//        private DevExpress.XtraLayout.LayoutControlGroup lgMain;
//        private DevExpress.XtraLayout.LayoutControlItem liClientPath;
//        private DevExpress.XtraEditors.SimpleButton btnCancel;
//        private DevExpress.XtraEditors.SimpleButton btnStart;
//        private DevExpress.XtraGrid.GridControl grLog;
//        private DevExpress.XtraGrid.Views.Grid.GridView viewLog;
//        private DevExpress.XtraGrid.Columns.GridColumn colTime;
//        private DevExpress.XtraGrid.Columns.GridColumn colText;
//        private DevExpress.XtraLayout.LayoutControlItem liLog;
//        private DevExpress.XtraLayout.LayoutControlItem liStart;
//        private DevExpress.XtraLayout.LayoutControlItem liCancel;
//    }
//}
