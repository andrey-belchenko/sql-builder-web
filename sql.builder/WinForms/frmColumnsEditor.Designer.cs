//namespace sql.builder.WinForms
//{
//    internal partial class frmColumnsEditor
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
//            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmColumnsEditor));
//            this.treeList2 = new DevExpress.XtraTreeList.TreeList();
//            this.treeListColumn1 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.treeListColumn3 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.repositoryItemCheckEdit2 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.treeList1 = new DevExpress.XtraTreeList.TreeList();
//            this.treeListColumn2 = new DevExpress.XtraTreeList.Columns.TreeListColumn();
//            this.repositoryItemCheckEdit1 = new DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit();
//            this.splitContainerControl1 = new DevExpress.XtraEditors.SplitContainerControl();
//            this.barManager1 = new DevExpress.XtraBars.BarManager(this.components);
//            this.bar2 = new DevExpress.XtraBars.Bar();
//            this.barLargeButtonItem1 = new DevExpress.XtraBars.BarLargeButtonItem();
//            this.barButtonItem7 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem8 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem9 = new DevExpress.XtraBars.BarButtonItem();
//            this.barStaticItem1 = new DevExpress.XtraBars.BarStaticItem();
//            this.barButtonItem4 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem5 = new DevExpress.XtraBars.BarButtonItem();
//            this.barDockControlTop = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlBottom = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlLeft = new DevExpress.XtraBars.BarDockControl();
//            this.barDockControlRight = new DevExpress.XtraBars.BarDockControl();
//            this.barButtonItem1 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem2 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem3 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem6 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem10 = new DevExpress.XtraBars.BarButtonItem();
//            this.barButtonItem11 = new DevExpress.XtraBars.BarButtonItem();
//            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).BeginInit();
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).BeginInit();
//            this.splitContainerControl1.SuspendLayout();
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).BeginInit();
//            this.SuspendLayout();
            // 
            // treeList2
            // 
//            this.treeList2.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
//            this.treeListColumn1,
//            this.treeListColumn3});
//            this.treeList2.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.treeList2.KeyFieldName = "elid";
//            this.treeList2.Location = new System.Drawing.Point(0, 0);
//            this.treeList2.Name = "treeList2";
//            this.treeList2.OptionsBehavior.Editable = false;
//            this.treeList2.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.treeList2.OptionsSelection.MultiSelect = true;
//            this.treeList2.OptionsView.ShowAutoFilterRow = true;
//            this.treeList2.ParentFieldName = "pelid";
//            this.treeList2.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.repositoryItemCheckEdit2});
//            this.treeList2.Size = new System.Drawing.Size(454, 390);
//            this.treeList2.TabIndex = 7;
//            this.treeList2.NodeChanged += new DevExpress.XtraTreeList.NodeChangedEventHandler(this.treeList2_NodeChanged);
//            this.treeList2.SelectionChanged += new System.EventHandler(this.treeList2_SelectionChanged);
            // 
            // treeListColumn1
            // 
//            this.treeListColumn1.Caption = "Выбранные колонки";
//            this.treeListColumn1.FieldName = "title";
//            this.treeListColumn1.MinWidth = 32;
//            this.treeListColumn1.Name = "treeListColumn1";
//            this.treeListColumn1.OptionsColumn.AllowEdit = false;
//            this.treeListColumn1.OptionsColumn.AllowSort = false;
//            this.treeListColumn1.Visible = true;
//            this.treeListColumn1.VisibleIndex = 0;
            // 
            // treeListColumn3
            // 
//            this.treeListColumn3.Caption = "Порядок";
//            this.treeListColumn3.FieldName = "ord";
//            this.treeListColumn3.Name = "treeListColumn3";
//            this.treeListColumn3.SortOrder = System.Windows.Forms.SortOrder.Ascending;
            // 
            // repositoryItemCheckEdit2
            // 
//            this.repositoryItemCheckEdit2.AutoHeight = false;
//            this.repositoryItemCheckEdit2.Caption = "Check";
//            this.repositoryItemCheckEdit2.Name = "repositoryItemCheckEdit2";
            // 
            // treeList1
            // 
//            this.treeList1.Columns.AddRange(new DevExpress.XtraTreeList.Columns.TreeListColumn[] {
//            this.treeListColumn2});
//            this.treeList1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.treeList1.KeyFieldName = "elid";
//            this.treeList1.Location = new System.Drawing.Point(0, 0);
//            this.treeList1.Name = "treeList1";
//            this.treeList1.OptionsBehavior.EnableFiltering = true;
//            this.treeList1.OptionsSelection.EnableAppearanceFocusedCell = false;
//            this.treeList1.OptionsSelection.MultiSelect = true;
//            this.treeList1.OptionsView.ShowAutoFilterRow = true;
//            this.treeList1.OptionsView.ShowCheckBoxes = true;
//            this.treeList1.ParentFieldName = "pelid";
//            this.treeList1.RepositoryItems.AddRange(new DevExpress.XtraEditors.Repository.RepositoryItem[] {
//            this.repositoryItemCheckEdit1});
//            this.treeList1.Size = new System.Drawing.Size(458, 390);
//            this.treeList1.TabIndex = 6;
//            this.treeList1.AfterCheckNode += new DevExpress.XtraTreeList.NodeEventHandler(this.treeList1_AfterCheckNode);
//            this.treeList1.SelectionChanged += new System.EventHandler(this.treeList1_SelectionChanged);
            // 
            // treeListColumn2
            // 
//            this.treeListColumn2.Caption = "Все колонки";
//            this.treeListColumn2.FieldName = "title";
//            this.treeListColumn2.MinWidth = 32;
//            this.treeListColumn2.Name = "treeListColumn2";
//            this.treeListColumn2.OptionsColumn.AllowEdit = false;
//            this.treeListColumn2.OptionsColumn.AllowSort = false;
//            this.treeListColumn2.Visible = true;
//            this.treeListColumn2.VisibleIndex = 0;
            // 
            // repositoryItemCheckEdit1
            // 
//            this.repositoryItemCheckEdit1.AutoHeight = false;
//            this.repositoryItemCheckEdit1.Caption = "Check";
//            this.repositoryItemCheckEdit1.Name = "repositoryItemCheckEdit1";
            // 
            // splitContainerControl1
            // 
//            this.splitContainerControl1.Dock = System.Windows.Forms.DockStyle.Fill;
//            this.splitContainerControl1.Location = new System.Drawing.Point(0, 0);
//            this.splitContainerControl1.Name = "splitContainerControl1";
//            this.splitContainerControl1.Panel1.Controls.Add(this.treeList1);
//            this.splitContainerControl1.Panel1.Text = "Panel1";
//            this.splitContainerControl1.Panel2.Controls.Add(this.treeList2);
//            this.splitContainerControl1.Panel2.Text = "Panel2";
//            this.splitContainerControl1.Size = new System.Drawing.Size(917, 390);
//            this.splitContainerControl1.SplitterPosition = 458;
//            this.splitContainerControl1.TabIndex = 8;
//            this.splitContainerControl1.Text = "splitContainerControl1";
            // 
            // barManager1
            // 
//            this.barManager1.AllowCustomization = false;
//            this.barManager1.AllowMoveBarOnToolbar = false;
//            this.barManager1.AllowQuickCustomization = false;
//            this.barManager1.AllowShowToolbarsPopup = false;
//            this.barManager1.Bars.AddRange(new DevExpress.XtraBars.Bar[] {
//            this.bar2});
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
//            this.barLargeButtonItem1,
//            this.barButtonItem6,
//            this.barButtonItem7,
//            this.barButtonItem8,
//            this.barButtonItem9,
//            this.barStaticItem1,
//            this.barButtonItem10,
//            this.barButtonItem11});
//            this.barManager1.MaxItemId = 13;
            // 
            // bar2
            // 
//            this.bar2.BarName = "Пользовательская 2";
//            this.bar2.DockCol = 0;
//            this.bar2.DockRow = 0;
//            this.bar2.DockStyle = DevExpress.XtraBars.BarDockStyle.Bottom;
//            this.bar2.FloatLocation = new System.Drawing.Point(440, 536);
//            this.bar2.LinksPersistInfo.AddRange(new DevExpress.XtraBars.LinkPersistInfo[] {
//            new DevExpress.XtraBars.LinkPersistInfo(this.barLargeButtonItem1),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem7),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem10),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem11),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem8),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem9),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barStaticItem1),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem4),
//            new DevExpress.XtraBars.LinkPersistInfo(this.barButtonItem5)});
//            this.bar2.OptionsBar.AllowQuickCustomization = false;
//            this.bar2.OptionsBar.AutoPopupMode = DevExpress.XtraBars.BarAutoPopupMode.All;
//            this.bar2.OptionsBar.DisableCustomization = true;
//            this.bar2.OptionsBar.DrawBorder = false;
//            this.bar2.OptionsBar.UseWholeRow = true;
//            this.bar2.Text = "Пользовательская 2";
            // 
            // barLargeButtonItem1
            // 
//            this.barLargeButtonItem1.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.barLargeButtonItem1.Caption = "Выбрать все";
//            this.barLargeButtonItem1.Id = 5;
//            this.barLargeButtonItem1.Name = "barLargeButtonItem1";
//            this.barLargeButtonItem1.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barLargeButtonItem1_ItemClick);
            // 
            // barButtonItem7
            // 
//            this.barButtonItem7.Caption = "Снять все";
//            this.barButtonItem7.Id = 7;
//            this.barButtonItem7.Name = "barButtonItem7";
//            this.barButtonItem7.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem7_ItemClick);
            // 
            // barButtonItem8
            // 
//            this.barButtonItem8.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.barButtonItem8.Caption = "Вверх";
//            this.barButtonItem8.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem8.Glyph")));
//            this.barButtonItem8.Id = 8;
//            this.barButtonItem8.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem8.LargeGlyph")));
//            this.barButtonItem8.Name = "barButtonItem8";
//            this.barButtonItem8.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem8_ItemClick);
            // 
            // barButtonItem9
            // 
//            this.barButtonItem9.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.barButtonItem9.Caption = "Вниз";
//            this.barButtonItem9.Glyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem9.Glyph")));
//            this.barButtonItem9.Id = 9;
//            this.barButtonItem9.LargeGlyph = ((System.Drawing.Image)(resources.GetObject("barButtonItem9.LargeGlyph")));
//            this.barButtonItem9.Name = "barButtonItem9";
//            this.barButtonItem9.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem9_ItemClick);
            // 
            // barStaticItem1
            // 
//            this.barStaticItem1.AutoSize = DevExpress.XtraBars.BarStaticItemSize.None;
//            this.barStaticItem1.Border = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;
//            this.barStaticItem1.Id = 10;
//            this.barStaticItem1.Name = "barStaticItem1";
//            this.barStaticItem1.TextAlignment = System.Drawing.StringAlignment.Near;
//            this.barStaticItem1.Width = 40;
            // 
            // barButtonItem4
            // 
//            this.barButtonItem4.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.barButtonItem4.Caption = "Готово";
//            this.barButtonItem4.Id = 3;
//            this.barButtonItem4.Name = "barButtonItem4";
//            this.barButtonItem4.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem4_ItemClick);
            // 
            // barButtonItem5
            // 
//            this.barButtonItem5.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            this.barButtonItem5.Caption = "Отмена";
//            this.barButtonItem5.Id = 4;
//            this.barButtonItem5.Name = "barButtonItem5";
//            this.barButtonItem5.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem5_ItemClick);
            // 
            // barDockControlTop
            // 
//            this.barDockControlTop.CausesValidation = false;
//            this.barDockControlTop.Dock = System.Windows.Forms.DockStyle.Top;
//            this.barDockControlTop.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlTop.Size = new System.Drawing.Size(917, 0);
            // 
            // barDockControlBottom
            // 
//            this.barDockControlBottom.CausesValidation = false;
//            this.barDockControlBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
//            this.barDockControlBottom.Location = new System.Drawing.Point(0, 390);
//            this.barDockControlBottom.Size = new System.Drawing.Size(917, 31);
            // 
            // barDockControlLeft
            // 
//            this.barDockControlLeft.CausesValidation = false;
//            this.barDockControlLeft.Dock = System.Windows.Forms.DockStyle.Left;
//            this.barDockControlLeft.Location = new System.Drawing.Point(0, 0);
//            this.barDockControlLeft.Size = new System.Drawing.Size(0, 390);
            // 
            // barDockControlRight
            // 
//            this.barDockControlRight.CausesValidation = false;
//            this.barDockControlRight.Dock = System.Windows.Forms.DockStyle.Right;
//            this.barDockControlRight.Location = new System.Drawing.Point(917, 0);
//            this.barDockControlRight.Size = new System.Drawing.Size(0, 390);
            // 
            // barButtonItem1
            // 
//            this.barButtonItem1.Caption = "Выбрать все";
//            this.barButtonItem1.Id = 0;
//            this.barButtonItem1.Name = "barButtonItem1";
            // 
            // barButtonItem2
            // 
//            this.barButtonItem2.Caption = "Снять выбор";
//            this.barButtonItem2.Id = 1;
//            this.barButtonItem2.Name = "barButtonItem2";
            // 
            // barButtonItem3
            // 
//            this.barButtonItem3.Caption = "Выбрать все";
//            this.barButtonItem3.Id = 2;
//            this.barButtonItem3.Name = "barButtonItem3";
            // 
            // barButtonItem6
            // 
//            this.barButtonItem6.Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            this.barButtonItem6.Caption = "Снять выбор";
//            this.barButtonItem6.Id = 6;
//            this.barButtonItem6.Name = "barButtonItem6";
            // 
            // barButtonItem10
            // 
//            this.barButtonItem10.Caption = "Выбрать выделенные";
//            this.barButtonItem10.Id = 11;
//            this.barButtonItem10.Name = "barButtonItem10";
//            this.barButtonItem10.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem10_ItemClick);
            // 
            // barButtonItem11
            // 
//            this.barButtonItem11.Caption = "Снять выделенные";
//            this.barButtonItem11.Id = 12;
//            this.barButtonItem11.Name = "barButtonItem11";
//            this.barButtonItem11.ItemClick += new DevExpress.XtraBars.ItemClickEventHandler(this.barButtonItem11_ItemClick);
            // 
            // frmColumnsEditor
            // 
//            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
//            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
//            this.ClientSize = new System.Drawing.Size(917, 421);
//            this.Controls.Add(this.splitContainerControl1);
//            this.Controls.Add(this.barDockControlLeft);
//            this.Controls.Add(this.barDockControlRight);
//            this.Controls.Add(this.barDockControlBottom);
//            this.Controls.Add(this.barDockControlTop);
//            this.Name = "frmColumnsEditor";
//            this.Text = "Настройка колонок";
//            this.UserSettings.SaveFormSize = true;
//            this.Load += new System.EventHandler(this.frmColumnsEditor_Load);
//            ((System.ComponentModel.ISupportInitialize)(this.treeList2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit2)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.treeList1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.repositoryItemCheckEdit1)).EndInit();
//            ((System.ComponentModel.ISupportInitialize)(this.splitContainerControl1)).EndInit();
//            this.splitContainerControl1.ResumeLayout(false);
//            ((System.ComponentModel.ISupportInitialize)(this.barManager1)).EndInit();
//            this.ResumeLayout(false);

//        }

//        #endregion

//        private DevExpress.XtraTreeList.TreeList treeList1;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit1;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn2;
//        private DevExpress.XtraTreeList.TreeList treeList2;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn1;
//        private DevExpress.XtraEditors.Repository.RepositoryItemCheckEdit repositoryItemCheckEdit2;
//        private DevExpress.XtraTreeList.Columns.TreeListColumn treeListColumn3;
//        private DevExpress.XtraEditors.SplitContainerControl splitContainerControl1;
//        private DevExpress.XtraBars.BarManager barManager1;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem1;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem2;
//        private DevExpress.XtraBars.BarDockControl barDockControlTop;
//        private DevExpress.XtraBars.BarDockControl barDockControlBottom;
//        private DevExpress.XtraBars.BarDockControl barDockControlLeft;
//        private DevExpress.XtraBars.BarDockControl barDockControlRight;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem3;
//        private DevExpress.XtraBars.Bar bar2;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem4;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem5;
//        private DevExpress.XtraBars.BarLargeButtonItem barLargeButtonItem1;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem7;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem6;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem8;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem9;
//        private DevExpress.XtraBars.BarStaticItem barStaticItem1;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem10;
//        private DevExpress.XtraBars.BarButtonItem barButtonItem11;
//    }
//}
