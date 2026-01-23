//using System.Collections.Generic;
//using System.Linq;
//using System.Xml.Linq;


//namespace sql.builder.Controls
//{
//    internal partial class ucProjects
//    {
//        /// <summary> 
//        /// Required designer variable.
//        /// </summary>
//        private System.ComponentModel.IContainer components = null;

       

//        #region Component Designer generated code

//        /// <summary> 
//        /// Required method for Designer support - do not modify 
//        /// the contents of this method with the code editor.
//        /// </summary>
//        private void InitializeComponent()
//        {
//            this.components = new System.ComponentModel.Container();
//            this.view = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colCheck = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rceCheck = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.colProjectName = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colStatus = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.rleStatus = new DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit();
//            this.colReferences = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colLinks = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colDefaultLoaded = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.grid = new DevExpress.XtraGrid.GridControl();
//            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
//            this.bar1 = new DevExpress.XtraBars.Bar();
//            this.btnAcceptChanges = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCreateProject = new DevExpress.XtraBars.BarButtonItem();
//            this.btnEditProject = new DevExpress.XtraBars.BarButtonItem();
//            this.btnCheckSelected = new DevExpress.XtraBars.BarButtonItem();
//            this.btnUnloadAllProjects = new DevExpress.XtraBars.BarButtonItem();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            this.btnCopyFullName = new DevExpress.XtraBars.BarButtonItem();
//            this.tooltip = new DevExpress.Utils.ToolTipController(this.components);
//            ((System.ComponentModel.ISupportInitialize)(this.view)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceCheck)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleStatus)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            this.SuspendLayout();
            // 
            // view
            // 
//            this.view.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colCheck,
//            this.colProjectName,
//            this.colStatus,
//            this.colReferences,
//            this.colLinks,
//            this.colDefaultLoaded});
//            this.view.GridControl = this.grid;
//            this.view.Name = "view";
//            this.view.OptionsBehavior.EditorShowMode = DevExpress.Utils.EditorShowMode.MouseDown;
//            this.view.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.view.OptionsSelection.MultiSelect = true;
//            this.view.OptionsSelection.MultiSelectMode = DevExpress.XtraGrid.Views.Grid.GridMultiSelectMode.CellSelect;
//            this.view.OptionsView.ShowAutoFilterRow = true;
//            this.view.OptionsView.ShowGroupPanel = false;
//            this.view.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.view_CustomDrawCell);
            // 
            // colCheck
            // 
//            this.colCheck.Caption = " ";
//            this.colCheck.ColumnEdit = this.rceCheck;
//            this.colCheck.FieldName = "Checked";
//            this.colCheck.Name = "colCheck";
//            this.colCheck.OptionsColumn.FixedWidth = true;
//            this.colCheck.Visible = true;
//            this.colCheck.VisibleIndex = 0;
//            this.colCheck.Width = 30;
            // 
            // rceCheck
            // 
//            this.rceCheck.AutoHeight = false;
//            this.rceCheck.Name = "rceCheck";
//            this.rceCheck.EditValueChanged += new System.EventHandler(this.rceCheck_EditValueChanged);
            // 
            // colProjectName
            // 
//            this.colProjectName.AppearanceHeader.Options.UseTextOptions = true;
//            this.colProjectName.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colProjectName.Caption = "Проект";
//            this.colProjectName.FieldName = "Name";
//            this.colProjectName.Name = "colProjectName";
//            this.colProjectName.OptionsColumn.AllowEdit = false;
//            this.colProjectName.Visible = true;
//            this.colProjectName.VisibleIndex = 1;
//            this.colProjectName.Width = 141;
            // 
            // colStatus
            // 
//            this.colStatus.AppearanceHeader.Options.UseTextOptions = true;
//            this.colStatus.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colStatus.Caption = "Статус";
//            this.colStatus.ColumnEdit = this.rleStatus;
//            this.colStatus.FieldName = "Status";
//            this.colStatus.Name = "colStatus";
//            this.colStatus.OptionsColumn.AllowEdit = false;
//            this.colStatus.Visible = true;
//            this.colStatus.VisibleIndex = 4;
//            this.colStatus.Width = 131;
            // 
            // rleStatus
            // 
//            this.rleStatus.AutoHeight = false;
//            this.rleStatus.Buttons.AddRange(new DevExpress.XtraEditors.Controls.EditorButton[] {
//            new DevExpress.XtraEditors.Controls.EditorButton(DevExpress.XtraEditors.Controls.ButtonPredefines.Combo)});
//            this.rleStatus.DisplayMember = "Value";
//            this.rleStatus.Name = "rleStatus";
//            this.rleStatus.ValueMember = "Key";
            // 
            // colReferences
            // 
//            this.colReferences.AppearanceHeader.Options.UseTextOptions = true;
//            this.colReferences.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colReferences.Caption = "Загружает";
//            this.colReferences.FieldName = "References";
//            this.colReferences.Name = "colReferences";
//            this.colReferences.OptionsColumn.AllowEdit = false;
//            this.colReferences.Visible = true;
//            this.colReferences.VisibleIndex = 2;
//            this.colReferences.Width = 212;
            // 
            // colLinks
            // 
//            this.colLinks.AppearanceHeader.Options.UseTextOptions = true;
//            this.colLinks.AppearanceHeader.TextOptions.HAlignment = DevExpress.Utils.HorzAlignment.Center;
//            this.colLinks.Caption = "Загружается с";
//            this.colLinks.FieldName = "Masters";
//            this.colLinks.Name = "colLinks";
//            this.colLinks.OptionsColumn.AllowEdit = false;
//            this.colLinks.Visible = true;
//            this.colLinks.VisibleIndex = 3;
//            this.colLinks.Width = 224;
            // 
            // colDefaultLoaded
            // 
//            this.colDefaultLoaded.Caption = "Умолч.";
//            this.colDefaultLoaded.FieldName = "DefaultLoaded";
//            this.colDefaultLoaded.Name = "colDefaultLoaded";
//            this.colDefaultLoaded.OptionsColumn.AllowEdit = false;
//            this.colDefaultLoaded.OptionsColumn.FixedWidth = true;
//            this.colDefaultLoaded.Visible = true;
//            this.colDefaultLoaded.VisibleIndex = 5;
//            this.colDefaultLoaded.Width = 45;
            // 
            // grid
            // 
//            this.grid.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.grid.EmbeddedNavigator.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.grid.Location = new System.Drawing.Point(0, 49);
//            this.grid.MainView = this.view;
//            this.grid.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.grid.Name = "grid";
//            this.grid.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.rceCheck,
//            this.rleStatus});
//            this.grid.Size = new System.Drawing.Size(922, 353);
//            this.grid.TabIndex = 0;
//            this.grid.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.view});
            // 
            // barManager1
            // 
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.bar1});
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.btnCopyFullName,
//            this.btnAcceptChanges,
//            this.btnCreateProject,
//            this.btnEditProject,
//            this.btnUnloadAllProjects,
//            this.btnCheckSelected});
//            this.barManager1.MaxItemId = 6;
            // 
            // bar1
            // 
//            this.bar1.BarName = "Custom 2";
//            this.bar1.DockCol = 0;
//            this.bar1.DockRow = 0;
//            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnAcceptChanges),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCreateProject),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnEditProject),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnCheckSelected),
//            new DevExpress.XtraBars.LinkPersistInfo(this.btnUnloadAllProjects)});
//            this.bar1.OptionsBar.AllowQuickCustomization = false;
//            this.bar1.OptionsBar.DrawBorder = false;
//            this.bar1.OptionsBar.DrawDragBorder = false;
//            this.bar1.OptionsBar.UseWholeRow = true;
//            this.bar1.Text = "Custom 2";
            // 
            // btnAcceptChanges
            // 
//            this.btnAcceptChanges.Caption = "Сохранить";
//            this.btnAcceptChanges.Id = 1;
//            this.btnAcceptChanges.ImageOptions.Image = global::sql.builder.Properties.Resources.Commit_24;
//            this.btnAcceptChanges.Name = "btnAcceptChanges";
//            this.btnAcceptChanges.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnAcceptChanges_ItemClick);
            // 
            // btnCreateProject
            // 
//            this.btnCreateProject.Caption = "Создать проект";
//            this.btnCreateProject.Id = 2;
//            this.btnCreateProject.Name = "btnCreateProject";
//            this.btnCreateProject.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCreateProject_ItemClick);
            // 
            // btnEditProject
            // 
//            this.btnEditProject.Caption = "Редактировать проект";
//            this.btnEditProject.Id = 3;
//            this.btnEditProject.Name = "btnEditProject";
//            this.btnEditProject.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnEditProject_ItemClick);
            // 
            // btnCheckSelected
            // 
//            this.btnCheckSelected.Caption = "Отметить выделенные";
//            this.btnCheckSelected.Id = 5;
//            this.btnCheckSelected.Name = "btnCheckSelected";
//            this.btnCheckSelected.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnCheckSelected_ItemClick);
            // 
            // btnUnloadAllProjects
            // 
//            this.btnUnloadAllProjects.Caption = "Снять все отметки";
//            this.btnUnloadAllProjects.Id = 4;
//            this.btnUnloadAllProjects.Name = "btnUnloadAllProjects";
//            this.btnUnloadAllProjects.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.btnUnloadAllProjects_ItemClick);
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Manager = this.barManager1;
//            this.barDockControlTop.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlTop.Size = new System.Drawing.Size(922, 49);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 402);
//            this.barDockControlBottom.Manager = this.barManager1;
//            this.barDockControlBottom.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlBottom.Size = new System.Drawing.Size(922, 0);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 49);
//            this.barDockControlLeft.Manager = this.barManager1;
//            this.barDockControlLeft.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 353);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(922, 49);
//            this.barDockControlRight.Manager = this.barManager1;
//            this.barDockControlRight.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 353);
            // 
            // btnCopyFullName
            // 
//            this.btnCopyFullName.Caption = "Копировать полное имя";
//            this.btnCopyFullName.Id = 0;
//            this.btnCopyFullName.Name = "btnCopyFullName";
            // 
            // ucProjects
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 16F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.Controls.Add(this.grid);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
//            this.Name = "ucProjects";
//            this.Size = new System.Drawing.Size(922, 402);
//            ((System.ComponentModel.ISupportInitialize)(this.view)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rceCheck)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.rleStatus)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.grid)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraGrid.Views.Grid.GridView view;
//        private DevExpress.XtraGrid.GridControl grid;
//        private DevExpress.XtraGrid.Columns.GridColumn colCheck;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit rceCheck;
//        private DevExpress.XtraGrid.Columns.GridColumn colProjectName;
//        private DevExpress.XtraGrid.Columns.GridColumn colReferences;
//        private DevExpress.XtraGrid.Columns.GridColumn colLinks;
//        private DevExpress.XtraGrid.Columns.GridColumn colStatus;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.Bar bar1;
//        private DevExpress.XtraBars.BarButtonItem btnAcceptChanges;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraBars.BarButtonItem btnCopyFullName;
//        private DevExpress.XtraGrid.Columns.GridColumn colDefaultLoaded;
//        private DevExpress.XtraBars.BarButtonItem btnCreateProject;
//        private DevExpress.XtraBars.BarButtonItem btnEditProject;
//        private DevExpress.XtraEditors.Repository.RepositoryItemLookUpEdit rleStatus;
//        private DevExpress.Utils.ToolTipController tooltip;
//        private DevExpress.XtraBars.BarButtonItem btnUnloadAllProjects;
//        private DevExpress.XtraBars.BarButtonItem btnCheckSelected;
//    }
//}
