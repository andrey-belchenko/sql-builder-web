//namespace sql.builder.WinForms
//{
//    internal partial class frmExpressReport
//    {
//        /// <summary>
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

//        #region Windows Form Designer generated code

//        /// <summary>
//        /// Required method for Designer support - do not modify
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            this.btnWorkFolderPath = new DevExpress.XtraBars.BarEditItem();
//            this.rbtnWorkFolderPath = new DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit();
//            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
//            this.barMenu = new DevExpress.XtraBars.Bar();
//            this.bePrintForm = new DevExpress.XtraBars.BarEditItem();
//            this.rlePrintForm = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
//            this.btnReportParams = new DevExpress.XtraBars.BarButtonItem();
//            this.btnRepParsReset = new DevExpress.XtraBars.BarButtonItem();
//            this.ceStoreDefaultParams = new DevExpress.XtraBars.BarEditItem();
//            this.rceStoreDefaultParams = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            this.repositoryItemTextEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.repositoryItemTextEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.repositoryItemTextEdit3 = new DevExpress.XtraEditors.Repository.RepositoryItemTextEdit();
//            this.btnCancel = new DevExpress.XtraEditors.SimpleButton();
//            this.btnAccept = new DevExpress.XtraEditors.SimpleButton();
//            this.gcRepositories = new DevExpress.XtraEditors.GroupControl();
//            this.tlRepositories = new DevExpress.XtraTreeList.TreeList();
//            this.colNeedForm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.rceNeedForm = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.colQueryTitle = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.colDatForm = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.pParams = new DevExpress.XtraEditors.GroupControl();
//            this.pFooter = new System.Windows.Forms.TableLayoutPanel();
//            ((System.ComponentModel.ISupportInitialize)(this.rbtnWorkFolderPath)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rlePrintForm)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceStoreDefaultParams)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gcRepositories)).BeginInit();
//            this.gcRepositories.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.tlRepositories)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceNeedForm)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pParams)).BeginInit();
//            this.pFooter.SuspendLayout();
//            this.SuspendLayout();
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
//            this.btnReportParams,
//            this.btnRepParsReset,
//            this.btnWorkFolderPath,
//            this.bePrintForm,
//            this.ceStoreDefaultParams});
//            this.barManager1.MaxItemId = 7;
//            this.barManager1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.repositoryItemTextEdit1,
//            this.repositoryItemTextEdit2,
//            this.rbtnWorkFolderPath,
//            this.rlePrintForm,
//            this.repositoryItemTextEdit3,
//            this.rceStoreDefaultParams});
            // 
            // barMenu
            // 
//            this.barMenu.BarName = "Сервис";
//            this.barMenu.DockCol = 0;
//            this.barMenu.DockRow = 0;
//            this.barMenu.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.barMenu.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.bePrintForm),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnReportParams, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnRepParsReset),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnWorkFolderPath),
//            new DevExpress.XtraBars.LinkPersistInfo(this.ceStoreDefaultParams)});
//            this.barMenu.OptionsBar.AllowQuickCustomization = false;
//            this.barMenu.OptionsBar.DisableClose = true;
//            this.barMenu.OptionsBar.DisableCustomization = true;
//            this.barMenu.OptionsBar.DrawBorder = false;
//            this.barMenu.OptionsBar.DrawDragBorder = false;
//            this.barMenu.OptionsBar.MultiLine = true;
//            this.barMenu.OptionsBar.UseWholeRow = true;
//            this.barMenu.Text = "Сервис";
            // 
            // bePrintForm
            // 
//            this.bePrintForm.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.bePrintForm.AutoFillWidth = true;
//            this.bePrintForm.Caption = "Печатная форма";
//            this.bePrintForm.Edit = this.rlePrintForm;
//            this.bePrintForm.EditWidth = 190;
//            this.bePrintForm.Id = 4;
//            this.bePrintForm.Name = "bePrintForm";
//            this.bePrintForm.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.Caption;
//            this.bePrintForm.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
            // 
            // rlePrintForm
            // 
//            this.rlePrintForm.AutoHeight = false;
//            this.rlePrintForm.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rlePrintForm.Columns.AddRange(new DevExpress.XtraEditors.Controls.LookUpColumnInfo[] {
//            new DevExpress.XtraEditors.Controls.LookUpColumnInfo("title", 180, "Форма")});
//            this.rlePrintForm.DisplayMember = "title";
//            this.rlePrintForm.Name = "rlePrintForm";
//            this.rlePrintForm.ValueMember = "name";
            // 
            // btnReportParams
            // 
//            this.btnReportParams.Caption = "Дополнительные параметры";
//            this.btnReportParams.Id = 0;
//            this.btnReportParams.Name = "btnReportParams";
//            this.btnReportParams.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.btnReportParams.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnReportParams_ItemClick);
            // 
            // btnRepParsReset
            // 
//            this.btnRepParsReset.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.btnRepParsReset.Caption = "Сбросить параметры";
//            this.btnRepParsReset.Id = 1;
//            this.btnRepParsReset.Name = "btnRepParsReset";
//            this.btnRepParsReset.Visibility = DevExpress.XtraBars.BarItemVisibility.Never;
//            this.btnRepParsReset.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnRepParsReset_ItemClick);
            // 
            // ceStoreDefaultParams
            // 
//            this.ceStoreDefaultParams.Caption = "Запомнить параметры";
//            this.ceStoreDefaultParams.Edit = this.rceStoreDefaultParams;
//            this.ceStoreDefaultParams.Id = 6;
//            this.ceStoreDefaultParams.ItemAppearance.Normal.BackColor = System.Drawing.Color.Transparent;
//            this.ceStoreDefaultParams.ItemAppearance.Normal.Options.UseBackColor = true;
//            this.ceStoreDefaultParams.ItemInMenuAppearance.Normal.BackColor = System.Drawing.Color.Transparent;
//            this.ceStoreDefaultParams.ItemInMenuAppearance.Normal.Options.UseBackColor = true;
//            this.ceStoreDefaultParams.Name = "ceStoreDefaultParams";
//            this.ceStoreDefaultParams.PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
            // 
            // rceStoreDefaultParams
            // 
//            this.rceStoreDefaultParams.Appearance.BackColor = System.Drawing.Color.Transparent;
//            this.rceStoreDefaultParams.Appearance.Options.UseBackColor = true;
//            this.rceStoreDefaultParams.AutoHeight = false;
//            this.rceStoreDefaultParams.Name = "rceStoreDefaultParams";
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(548, 92);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 468);
//            this.barDockControlBottom.Size = new System.Drawing.Size(548, 0);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 92);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 376);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(548, 92);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 376);
            // 
            // repositoryItemTextEdit1
            // 
//            this.repositoryItemTextEdit1.AutoHeight = false;
//            this.repositoryItemTextEdit1.Name = "repositoryItemTextEdit1";
            // 
            // repositoryItemTextEdit2
            // 
//            this.repositoryItemTextEdit2.AutoHeight = false;
//            this.repositoryItemTextEdit2.Name = "repositoryItemTextEdit2";
            // 
            // repositoryItemTextEdit3
            // 
//            this.repositoryItemTextEdit3.AutoHeight = false;
//            this.repositoryItemTextEdit3.Name = "repositoryItemTextEdit3";
            // 
            // btnCancel
            // 
//            this.btnCancel.Location = new System.Drawing.Point(172, 8);
//            this.btnCancel.Margin = new System.Windows.Forms.Padding(8);
//            this.btnCancel.Name = "btnCancel";
//            this.btnCancel.Padding = new System.Windows.Forms.Padding(5);
//            this.btnCancel.Size = new System.Drawing.Size(75, 29);
//            this.btnCancel.TabIndex = 1;
//            this.btnCancel.Text = "Отмена";
//            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAccept
            // 
//            this.btnAccept.Location = new System.Drawing.Point(8, 8);
//            this.btnAccept.Margin = new System.Windows.Forms.Padding(8);
//            this.btnAccept.Name = "btnAccept";
//            this.btnAccept.Padding = new System.Windows.Forms.Padding(5);
//            this.btnAccept.Size = new System.Drawing.Size(148, 29);
//            this.btnAccept.TabIndex = 0;
//            this.btnAccept.Text = "Сформировать отчет";
//            this.btnAccept.Click += new System.EventHandler(this.btnAccept_Click);
            // 
            // gcRepositories
            // 
//            this.gcRepositories.Controls.Add(this.tlRepositories);
//            this.gcRepositories.Dock = System.Windows.Forms.DockStyle.Top;
//            this.gcRepositories.Location = new System.Drawing.Point(0, 92);
//            this.gcRepositories.Name = "gcRepositories";
//            this.gcRepositories.Size = new System.Drawing.Size(548, 100);
//            this.gcRepositories.TabIndex = 10;
//            this.gcRepositories.Text = "Отчёт использует хранилища данных";
//            this.gcRepositories.Visible = false;
            // 
            // tlRepositories
            // 
//            this.tlRepositories.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.tlRepositories.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
//            this.colNeedForm,
//            this.colQueryTitle,
//            this.colDatForm});
//            this.tlRepositories.Cursor = System.Windows.Forms.Cursors.Default;
//            this.tlRepositories.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.tlRepositories.KeyFieldName = "query_name";
//            this.tlRepositories.Location = new System.Drawing.Point(2, 20);
//            this.tlRepositories.Name = "tlRepositories";
//            this.tlRepositories.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.tlRepositories.OptionsSelection.EnableAppearanceFocusedRow = false;
//            this.tlRepositories.OptionsView.FocusRectStyle = DevExpress.XtraTreeList.DrawFocusRectStyle.None;
//            this.tlRepositories.OptionsView.ShowIndicator = false;
//            this.tlRepositories.OptionsView.ShowRoot = false;
//            this.tlRepositories.ParentFieldName = "parent_query_name";
//            this.tlRepositories.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rceNeedForm});
//            this.tlRepositories.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.Default;
//            this.tlRepositories.Size = new System.Drawing.Size(544, 78);
//            this.tlRepositories.TabIndex = 0;
            // 
            // colNeedForm
            // 
//            this.colNeedForm.AppearanceCell.Options.UseTextOptions = true;
//            this.colNeedForm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colNeedForm.AppearanceHeader.Options.UseTextOptions = true;
//            this.colNeedForm.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colNeedForm.Caption = "Формировать";
//            this.colNeedForm.ColumnEdit = this.rceNeedForm;
//            this.colNeedForm.FieldName = "need_form";
//            this.colNeedForm.MinWidth = 32;
//            this.colNeedForm.Name = "colNeedForm";
//            this.colNeedForm.OptionsColumn.AllowMove = false;
//            this.colNeedForm.OptionsColumn.AllowMoveToCustomizationForm = false;
//            this.colNeedForm.OptionsColumn.AllowSize = false;
//            this.colNeedForm.OptionsColumn.AllowSort = false;
//            this.colNeedForm.OptionsColumn.FixedWidth = true;
//            this.colNeedForm.Width = 80;
            // 
            // rceNeedForm
            // 
//            this.rceNeedForm.AutoHeight = false;
//            this.rceNeedForm.Name = "rceNeedForm";
            // 
            // colQueryTitle
            // 
//            this.colQueryTitle.AppearanceCell.Options.UseTextOptions = true;
//            this.colQueryTitle.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colQueryTitle.AppearanceHeader.Options.UseTextOptions = true;
//            this.colQueryTitle.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colQueryTitle.Caption = "Хранилище";
//            this.colQueryTitle.FieldName = "query_title";
//            this.colQueryTitle.MinWidth = 32;
//            this.colQueryTitle.Name = "colQueryTitle";
//            this.colQueryTitle.OptionsColumn.AllowEdit = false;
//            this.colQueryTitle.OptionsColumn.AllowFocus = false;
//            this.colQueryTitle.OptionsColumn.AllowMove = false;
//            this.colQueryTitle.OptionsColumn.AllowMoveToCustomizationForm = false;
//            this.colQueryTitle.OptionsColumn.AllowSize = false;
//            this.colQueryTitle.OptionsColumn.AllowSort = false;
//            this.colQueryTitle.OptionsColumn.ReadOnly = true;
//            this.colQueryTitle.Visible = true;
//            this.colQueryTitle.VisibleIndex = 0;
//            this.colQueryTitle.Width = 178;
            // 
            // colDatForm
            // 
//            this.colDatForm.AppearanceCell.Options.UseTextOptions = true;
//            this.colDatForm.AppearanceCell.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colDatForm.AppearanceHeader.Options.UseTextOptions = true;
//            this.colDatForm.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colDatForm.Caption = "Последнее обновление";
//            this.colDatForm.FieldName = "date_form";
//            this.colDatForm.Format.FormatString = "dd.MM.yyyy HH:mm:ss";
//            this.colDatForm.Format.FormatType = DevExpress.Utils.FormatType.DateTime;
//            this.colDatForm.Name = "colDatForm";
//            this.colDatForm.OptionsColumn.AllowEdit = false;
//            this.colDatForm.OptionsColumn.AllowFocus = false;
//            this.colDatForm.OptionsColumn.AllowMove = false;
//            this.colDatForm.OptionsColumn.AllowMoveToCustomizationForm = false;
//            this.colDatForm.OptionsColumn.AllowSize = false;
//            this.colDatForm.OptionsColumn.AllowSort = false;
//            this.colDatForm.OptionsColumn.FixedWidth = true;
//            this.colDatForm.OptionsColumn.ReadOnly = true;
//            this.colDatForm.ToolTip = "Дата и время последнего формирования";
//            this.colDatForm.Visible = true;
//            this.colDatForm.VisibleIndex = 1;
//            this.colDatForm.Width = 130;
            // 
            // pParams
            // 
//            this.pParams.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.pParams.Location = new System.Drawing.Point(0, 192);
//            this.pParams.Name = "pParams";
//            this.pParams.Size = new System.Drawing.Size(548, 228);
//            this.pParams.TabIndex = 25;
//            this.pParams.Text = "Параметры отчёта";
            // 
            // pFooter
            // 
//            this.pFooter.ColumnCount = 3;
//            this.pFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
//            this.pFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
//            this.pFooter.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
//            this.pFooter.Controls.Add(this.btnCancel, 1, 0);
//            this.pFooter.Controls.Add(this.btnAccept, 0, 0);
//            this.pFooter.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.pFooter.Location = new System.Drawing.Point(0, 420);
//            this.pFooter.Name = "pFooter";
//            this.pFooter.RowCount = 1;
//            this.pFooter.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 100F));
//            this.pFooter.Size = new System.Drawing.Size(548, 48);
//            this.pFooter.TabIndex = 0;
            // 
            // frmExpressReport
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(548, 468);
//            this.Controls.Add(this.pParams);
//            this.Controls.Add(this.gcRepositories);
//            this.Controls.Add(this.pFooter);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.DoubleBuffered = true;
//            this.MinimumSize = new System.Drawing.Size(400, 400);
//            this.Name = "frmExpressReport";
//            this.Text = "Отчёт";
//            this.UserSettings.SaveFormSize = false;
//            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmExpressReport_FormClosing);
//            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmExpressReport_FormClosed);
//            this.Load += new System.EventHandler(this.frmExpressReport_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.rbtnWorkFolderPath)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rlePrintForm)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceStoreDefaultParams)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemTextEdit3)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gcRepositories)).EndInit();
//            this.gcRepositories.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.tlRepositories)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceNeedForm)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.pParams)).EndInit();
//            this.pFooter.ResumeLayout(false);
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraBars.BarEditItem btnWorkFolderPath;
//        private DevExpress.XtraEditors.Repository.RepositoryItemButtonEdit rbtnWorkFolderPath;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.Bar barMenu;
//        private DevExpress.XtraBars.BarButtonItem btnReportParams;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraBars.BarButtonItem btnRepParsReset;
//        private DevExpress.XtraEditors.SimpleButton btnCancel;
//        private DevExpress.XtraEditors.SimpleButton btnAccept;
//        private DevExpress.XtraBars.BarEditItem bePrintForm;
//        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rlePrintForm;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit1;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit2;
//        private DevExpress.XtraEditors.GroupControl gcRepositories;
//        private DevExpress.XtraTreeList.TreeList tlRepositories;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colQueryTitle;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colDatForm;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn colNeedForm;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceNeedForm;
//        private DevExpress.XtraBars.BarEditItem ceStoreDefaultParams;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceStoreDefaultParams;
//        private DevExpress.XtraEditors.Repository.RepositoryItemTextEdit repositoryItemTextEdit3;
//        private DevExpress.XtraEditors.GroupControl pParams;
//        private System.Windows.Forms.TableLayoutPanel pFooter;
//    }
//}
