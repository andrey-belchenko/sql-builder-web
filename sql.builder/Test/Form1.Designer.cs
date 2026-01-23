//namespace sql.builder.Test
//{
//    partial class Form1
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
//            this.components = new System.ComponentModel.Container();
//            this.gridControl1 = new DevExpress.XtraGrid.GridControl();
//            this.gridView1 = new DevExpress.XtraGrid.Views.Grid.GridView();
//            this.colName = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colType = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colAsuseRels = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colIprRels = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colFile = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.colCustomAsuse = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.colCustomIpr = new DevExpress.XtraGrid.Columns.GridColumn();
//            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
//            this.bar1 = new DevExpress.XtraBars.Bar();
//            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
//            this.bar3 = new DevExpress.XtraBars.Bar();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
//            this.splitterControl1 = new DevExpress.XtraEditors.SplitterControl();
//            this.memoEdit1 = new DevExpress.XtraEditors.MemoEdit();
//            this.popupMenu1 = new DevExpress.XtraBars.PopupMenu(this.components);
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).BeginInit();
//            this.SuspendLayout();
            // 
            // gridControl1
            // 
//            this.gridControl1.Dock = System.Windows.Forms.DockStyle.Left;
//            this.gridControl1.Location = new System.Drawing.Point(0, 29);
//            this.gridControl1.MainView = this.gridView1;
//            this.gridControl1.Name = "gridControl1";
//            this.gridControl1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.repositoryItemCheckEdit1});
//            this.gridControl1.Size = new System.Drawing.Size(755, 467);
//            this.gridControl1.TabIndex = 0;
//            this.gridControl1.ViewCollection.AddRange(new DevExpress.XtraGrid.Views.Base.BaseView[] {
//            this.gridView1});
            // 
            // gridView1
            // 
//            this.gridView1.Columns.AddRange(new DevExpress.XtraGrid.Columns.GridColumn[] {
//            this.colName,
//            this.colType,
//            this.colAsuseRels,
//            this.colIprRels,
//            this.colFile,
//            this.colCustomAsuse,
//            this.colCustomIpr});
//            this.gridView1.GridControl = this.gridControl1;
//            this.gridView1.GroupCount = 1;
//            this.gridView1.GroupSummary.AddRange(new DevExpress.XtraGrid.GridSummaryItem[] {
//            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "AsuseRelsCount", null, "AsuseRelsCount = {0}", ""),
//            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Sum, "IprRelsCount", null, "IprRelsCount = {0}"),
//            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Max, "CustomAsuse", null, " "),
//            new DevExpress.XtraGrid.GridGroupSummaryItem(DevExpress.Data.SummaryItemType.Max, "CustomIpr", null, " ")});
//            this.gridView1.Name = "gridView1";
//            this.gridView1.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.gridView1.OptionsSelection.EnableAppearanceFocusedRow = false;
//            this.gridView1.OptionsSelection.MultiSelect = true;
//            this.gridView1.OptionsView.ShowAutoFilterRow = true;
//            this.gridView1.OptionsView.ShowGroupPanel = false;
//            this.gridView1.OptionsView.ShowViewCaption = true;
//            this.gridView1.SortInfo.AddRange(new DevExpress.XtraGrid.Columns.GridColumnSortInfo[] {
//            new DevExpress.XtraGrid.Columns.GridColumnSortInfo(this.colFile, DevExpress.Data.ColumnSortOrder.Ascending)});
//            this.gridView1.ViewCaption = "ALL";
//            this.gridView1.RowClick += new DevExpress.XtraGrid.Views.Grid.RowClickEventHandler(this.gridView1_RowClick);
//            this.gridView1.CustomDrawCell += new DevExpress.XtraGrid.Views.Base.RowCellCustomDrawEventHandler(this.gridView1_CustomDrawCell);
//            this.gridView1.RowStyle += new DevExpress.XtraGrid.Views.Grid.RowStyleEventHandler(this.gridView1_RowStyle);
//            this.gridView1.FocusedRowChanged += new DevExpress.XtraGrid.Views.Base.FocusedRowChangedEventHandler(this.gridView1_FocusedRowChanged);
            // 
            // colName
            // 
//            this.colName.FieldName = "Name";
//            this.colName.Name = "colName";
//            this.colName.OptionsColumn.AllowEdit = false;
//            this.colName.Visible = true;
//            this.colName.VisibleIndex = 0;
            // 
            // colType
            // 
//            this.colType.Caption = "Type";
//            this.colType.FieldName = "Type";
//            this.colType.Name = "colType";
//            this.colType.OptionsColumn.AllowEdit = false;
//            this.colType.Visible = true;
//            this.colType.VisibleIndex = 1;
            // 
            // colAsuseRels
            // 
//            this.colAsuseRels.Caption = "ASUSE RELS";
//            this.colAsuseRels.FieldName = "AsuseRelsCount";
//            this.colAsuseRels.Name = "colAsuseRels";
//            this.colAsuseRels.OptionsColumn.AllowEdit = false;
//            this.colAsuseRels.Visible = true;
//            this.colAsuseRels.VisibleIndex = 2;
            // 
            // colIprRels
            // 
//            this.colIprRels.Caption = "IPR RELS";
//            this.colIprRels.FieldName = "IprRelsCount";
//            this.colIprRels.Name = "colIprRels";
//            this.colIprRels.OptionsColumn.AllowEdit = false;
//            this.colIprRels.Visible = true;
//            this.colIprRels.VisibleIndex = 3;
            // 
            // colFile
            // 
//            this.colFile.Caption = "File";
//            this.colFile.FieldName = "File";
//            this.colFile.Name = "colFile";
//            this.colFile.Visible = true;
//            this.colFile.VisibleIndex = 1;
            // 
            // colCustomAsuse
            // 
//            this.colCustomAsuse.Caption = "ASUSE";
//            this.colCustomAsuse.ColumnEdit = this.repositoryItemCheckEdit1;
//            this.colCustomAsuse.FieldName = "CustomAsuse";
//            this.colCustomAsuse.Name = "colCustomAsuse";
//            this.colCustomAsuse.Visible = true;
//            this.colCustomAsuse.VisibleIndex = 4;
            // 
            // repositoryItemCheckEdit1
            // 
//            this.repositoryItemCheckEdit1.AllowGrayed = true;
//            this.repositoryItemCheckEdit1.AutoHeight = false;
//            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
//            this.repositoryItemCheckEdit1.EditValueChanged += new System.EventHandler(this.repositoryItemCheckEdit1_EditValueChanged);
            // 
            // colCustomIpr
            // 
//            this.colCustomIpr.Caption = "IPR";
//            this.colCustomIpr.ColumnEdit = this.repositoryItemCheckEdit1;
//            this.colCustomIpr.FieldName = "CustomIpr";
//            this.colCustomIpr.Name = "colCustomIpr";
//            this.colCustomIpr.Visible = true;
//            this.colCustomIpr.VisibleIndex = 5;
            // 
            // barManager1
            // 
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.bar1,
//            this.bar3});
//            this.barManager1.DockControls.Add(this.barDockControlTop);
//            this.barManager1.DockControls.Add(this.barDockControlBottom);
//            this.barManager1.DockControls.Add(this.barDockControlLeft);
//            this.barManager1.DockControls.Add(this.barDockControlRight);
//            this.barManager1.Form = this;
//            this.barManager1.Items.AddRange(new DevExpress.XtraBars.BarItem[] {
//            this.barButtonItem1,
//            this.barButtonItem2,
//            this.barButtonItem3,
//            this.barButtonItem4,
//            this.barButtonItem5,
//            this.barButtonItem6,
//            this.barButtonItem7,
//            this.barButtonItem8,
//            this.barButtonItem9,
//            this.barButtonItem10,
//            this.barButtonItem11});
//            this.barManager1.MaxItemId = 11;
//            this.barManager1.StatusBar = this.bar3;
            // 
            // bar1
            // 
//            this.bar1.BarName = "Tools";
//            this.bar1.DockCol = 0;
//            this.bar1.DockRow = 0;
//            this.bar1.DockStyle = DevExpress.XtraBars.BarDockStyle.Top;
//            this.bar1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem1),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem2, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem3, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4, true),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5, true)});
//            this.bar1.Text = "Tools";
            // 
            // barButtonItem1
            // 
//            this.barButtonItem1.Caption = "Init";
//            this.barButtonItem1.Id = 0;
//            this.barButtonItem1.Name = "barButtonItem1";
//            this.barButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem1_ItemClick);
            // 
            // barButtonItem2
            // 
//            this.barButtonItem2.Caption = "Process";
//            this.barButtonItem2.Id = 1;
//            this.barButtonItem2.Name = "barButtonItem2";
//            this.barButtonItem2.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem2_ItemClick);
            // 
            // barButtonItem3
            // 
//            this.barButtonItem3.Caption = "SaveResult";
//            this.barButtonItem3.Id = 2;
//            this.barButtonItem3.Name = "barButtonItem3";
//            this.barButtonItem3.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem3_ItemClick);
            // 
            // barButtonItem4
            // 
//            this.barButtonItem4.Caption = "LoadResult";
//            this.barButtonItem4.Id = 3;
//            this.barButtonItem4.Name = "barButtonItem4";
//            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
//            this.barButtonItem5.Caption = "Separate Files";
//            this.barButtonItem5.Id = 4;
//            this.barButtonItem5.Name = "barButtonItem5";
//            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // bar3
            // 
//            this.bar3.BarName = "Status bar";
//            this.bar3.CanDockStyle = DevExpress.XtraBars.BarCanDockStyle.Bottom;
//            this.bar3.DockCol = 0;
//            this.bar3.DockRow = 0;
//            this.bar3.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.bar3.OptionsBar.AllowQuickCustomization = false;
//            this.bar3.OptionsBar.DrawDragBorder = false;
//            this.bar3.OptionsBar.UseWholeRow = true;
//            this.bar3.Text = "Status bar";
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(1157, 29);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 496);
//            this.barDockControlBottom.Size = new System.Drawing.Size(1157, 23);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 29);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 467);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(1157, 29);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 467);
            // 
            // barButtonItem6
            // 
//            this.barButtonItem6.Caption = "Все ASUSE";
//            this.barButtonItem6.Id = 5;
//            this.barButtonItem6.Name = "barButtonItem6";
//            this.barButtonItem6.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem6_ItemClick);
            // 
            // barButtonItem7
            // 
//            this.barButtonItem7.Caption = "Все !ASUSE";
//            this.barButtonItem7.Id = 6;
//            this.barButtonItem7.Name = "barButtonItem7";
//            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // barButtonItem8
            // 
//            this.barButtonItem8.Caption = "Очистить ASUSE";
//            this.barButtonItem8.Id = 7;
//            this.barButtonItem8.Name = "barButtonItem8";
//            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem8_ItemClick);
            // 
            // barButtonItem9
            // 
//            this.barButtonItem9.Caption = "Все IPR";
//            this.barButtonItem9.Id = 8;
//            this.barButtonItem9.Name = "barButtonItem9";
//            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
            // 
            // barButtonItem10
            // 
//            this.barButtonItem10.Caption = "Все !IPR";
//            this.barButtonItem10.Id = 9;
//            this.barButtonItem10.Name = "barButtonItem10";
//            this.barButtonItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem10_ItemClick);
            // 
            // barButtonItem11
            // 
//            this.barButtonItem11.Caption = "Очистить IPR";
//            this.barButtonItem11.Id = 10;
//            this.barButtonItem11.Name = "barButtonItem11";
//            this.barButtonItem11.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem11_ItemClick);
            // 
            // splitterControl1
            // 
//            this.splitterControl1.Location = new System.Drawing.Point(755, 29);
//            this.splitterControl1.Name = "splitterControl1";
//            this.splitterControl1.Size = new System.Drawing.Size(5, 467);
//            this.splitterControl1.TabIndex = 5;
//            this.splitterControl1.TabStop = false;
            // 
            // memoEdit1
            // 
//            this.memoEdit1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.memoEdit1.Location = new System.Drawing.Point(760, 29);
//            this.memoEdit1.MenuManager = this.barManager1;
//            this.memoEdit1.Name = "memoEdit1";
//            this.memoEdit1.Size = new System.Drawing.Size(397, 467);
//            this.memoEdit1.TabIndex = 6;
            // 
            // popupMenu1
            // 
//            this.popupMenu1.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem6),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem8),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem10),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem11)});
//            this.popupMenu1.Manager = this.barManager1;
//            this.popupMenu1.Name = "popupMenu1";
            // 
            // Form1
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(1157, 519);
//            this.Controls.Add(this.memoEdit1);
//            this.Controls.Add(this.splitterControl1);
//            this.Controls.Add(this.gridControl1);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Name = "Form1";
//            this.Text = "Form1";
//            ((System.ComponentModel.ISupportInitialize)(this.gridControl1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.gridView1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.memoEdit1.Properties)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.popupMenu1)).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();

//        }

//        #endregion

//        private DevExpress.XtraGrid.GridControl gridControl1;
//        private DevExpress.XtraGrid.Views.Grid.GridView gridView1;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.Bar bar1;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
//        private DevExpress.XtraBars.Bar bar3;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraGrid.Columns.GridColumn colName;
//        private DevExpress.XtraGrid.Columns.GridColumn colType;
//        private DevExpress.XtraGrid.Columns.GridColumn colAsuseRels;
//        private DevExpress.XtraGrid.Columns.GridColumn colIprRels;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
//        private DevExpress.XtraEditors.MemoEdit memoEdit1;
//        private DevExpress.XtraEditors.SplitterControl splitterControl1;
//        private DevExpress.XtraGrid.Columns.GridColumn colFile;
//        private DevExpress.XtraGrid.Columns.GridColumn colCustomAsuse;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
//        private DevExpress.XtraGrid.Columns.GridColumn colCustomIpr;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem11;
//        private DevExpress.XtraBars.PopupMenu popupMenu1;
//    }
//}

