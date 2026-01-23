//using System;
//using System.Linq;
////using System.Windows.Forms;
//using DevExpress.Utils;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraGrid.Views.Grid.ViewInfo;

//using sql.builder.UI;
//using sql.builder.XmlHelpers;
//using sql.builder.UI.WinForms;
//using sql.builder.DataApi;
//using System.Data;
//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucGridContainerWF : XtraUserControl,IucGridContainer
//    {
        
        
//        //private DevExpress.XtraGrid.GridControl grid;
//        IucGrid grid;
//        public ucGridContainerWF(bool isTree)
//        {
//            InitializeComponent();
//            InitializeToolBars();
//            InitializeGrid( isTree);
//            Visible = false;
//            Dock = DockStyle.Fill;

//            //GetGridAsGridControl().ShowOnlyPredefinedDetails = true;
//            Disposed += OnDisposed;
//            //InitializeComponent2();
//        }

//        private void OnDisposed(object sender, EventArgs args)
//        {
//            //_menu = null;
//            // от утечек

//            GetGrid().DisposeViews();

//            //// https://www.devexpress.com/Support/Center/Question/Details/Q534989
//            //foreach (var gv in GetGridAsGridControl().ViewCollection.Cast<GridView>()) gv.Dispose();

//            if (VDisposed != null)
//            {
//                VDisposed();
//            }

//            //_repositories.Dispose();
//        }
       
//        //public void UpdateDummyColumnWidth(GridView view)
//        //{
//        //    GridColumn colDummy = view.Columns.ColumnByName(GridDesigner.GetDummyColumnName());
//        //    if (colDummy == null) return;

//        //    GridViewInfo info = view.GetViewInfo() as GridViewInfo;
//        //    int width_free = info.ViewRects.ColumnPanelWidth - info.ViewRects.ColumnTotalWidth + colDummy.Width - 20;// 20 чтобы не глючило при порявлении скрола
//        //    int width_min = GridDesigner.GetMinDummyWidth();

//        //    colDummy.Width = (width_free < width_min) ? width_min : width_free;
//        //}

        

//        private void ucGridWF_Resize(object sender, EventArgs e)
//        {
//			if (GetGrid() != null)
//			{
//				GetGrid().UpdateDummyColumnWidth();
//			}
//            //foreach (var bview in GetGridAsGridControl().ViewCollection.OfType<BandedGridView>())
//            //{
              
//            //    UpdateDummyColumnWidth(bview);
//            //}
//        }

//        //private GridView getViewByName(string name)
//        //{
//        //    if (name == null)
//        //    {
//        //        return GetGridAsGridControl().MainView as GridView;
//        //    }
//        //    return GetGridAsGridControl().ViewCollection.Cast<GridView>().First(v => v.Name == name);
//        //}

//        public void ApplyHeaderLayout(string viewName)
//        {
//            GetGrid().UpdateDummyColumnWidth();
//            //UpdateDummyColumnWidth(getViewByName(viewName));
//        }

//        private void tooltip_GetActiveObjectInfo(object sender, ToolTipControllerGetActiveObjectInfoEventArgs args)
//        {
//            if (GetGrid() is ucGridWF)
//            {
//                tooltip_GetActiveObjectInfo_Grid(sender, args);
//            }
//            else
//            {
//                tooltip_GetActiveObjectInfo_Tree(sender, args);
//            }

//        }


//        private void tooltip_GetActiveObjectInfo_Grid(object sender, ToolTipControllerGetActiveObjectInfoEventArgs args)
//        {
//            if (args.Info != null || args.SelectedControl != grid) return;

//            var view = (GetGrid() as ucGridWF).FocusedView as GridView;
//            if (view == null) return;

//            GridHitInfo hitInfo = view.CalcHitInfo(args.ControlMousePosition);
//            if (!hitInfo.InRowCell || hitInfo.HitTest != GridHitTest.RowCell) return;

//            var row = view.GetDataRow(hitInfo.RowHandle);
//            if (row == null) return;

//            var checkWarning = _controller.IsToolTipForWarning(hitInfo, view, row);

//            var ti = _controller.GetToolTipInfo(row, hitInfo.Column.FieldName, checkWarning);
//            if (ti != null)
//            {
//                SuperToolTip toolTip = new SuperToolTip();
//                ToolTipItem item = new ToolTipItem();
//                item.Image = ti.image;
//                item.Text = ti.text;
//                toolTip.Items.Add(item);
//                args.Info = new ToolTipControlInfo(ti.Id, null);
//                args.Info.SuperTip = toolTip;
//            }

//        }

       
//        private void tooltip_GetActiveObjectInfo_Tree(object sender, ToolTipControllerGetActiveObjectInfoEventArgs args)
//        {
//            var cc = (_controller as ucGridContainer);

//            var tree = (GetGrid() as ucTreeWF);
//            if (args.Info != null || args.SelectedControl != tree) return;

//            DevExpress.XtraTreeList.TreeListHitInfo hitInfo = tree.CalcHitInfo(args.ControlMousePosition);
//            if (hitInfo.Node == null || hitInfo.HitTest.CellInfo == null) return;

//            DataRow row = cc.GetNodeData(hitInfo.Node);
//            if (row == null) return;

//            DevExpress.XtraTreeList.ViewInfo.CellInfo cell = hitInfo.HitTest.CellInfo;

//            var table = (row.Table as VDataTable);
//            var colName = table.GetNameForText(hitInfo.Column.FieldName);
//            var row_index = table.Rows.IndexOf(row);
//            var rep = cc.repositories().GetCellRepository(table, colName, row_index/*, true*/);// !!! проконтролировать время

//            var shift = Cmn.GetLeftButtonsSize(colName, rep);
//            var l = (hitInfo.MousePoint.X - cell.Bounds.Left);

//            var iswarningToolTip = false;
//            if ((l > shift) && (l < 15 + shift))
//            {
//                var msg = table.GetCellErrorForText(row, hitInfo.Column.FieldName);
//                if (!string.IsNullOrEmpty(msg))
//                {
//                    SuperToolTip toolTip = new SuperToolTip();

//                    ToolTipItem item = new ToolTipItem();
//                    item.Image = Cmn.ImageWarning14;
//                    item.Text = msg;
//                    toolTip.Items.Add(item);
//                    args.Info = new ToolTipControlInfo(0 + hitInfo.Column.FieldName, null);
//                    args.Info.SuperTip = toolTip;
//                    iswarningToolTip = true;
//                }
//            }

//            if (!iswarningToolTip)
//            {
//                if (rep is DevExpress.XtraEditors.Repository.RepositoryItemHyperLinkEdit)
//                {
//                    SuperToolTip toolTip = new SuperToolTip();

//                    ToolTipItem item = new ToolTipItem();
//                    item.Text = UILink.TooltipText;
//                    toolTip.Items.Add(item);
//                    args.Info = new ToolTipControlInfo(0 + hitInfo.Column.FieldName, null);
//                    args.Info.SuperTip = toolTip;
//                }
//            }
//        }


//        private IGridContainer _controller=null;
//        public void SetController(IGridContainer grid)
//        {
//            _controller = grid;
//        }


       


//        //private void InitializeComponent2()
//        //{
//        //   ((System.ComponentModel.ISupportInitialize)(this.barManager)).BeginInit();



//        //    System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ucGridWF));
//        //    AddBarButton(TextConst.AVGridButtonType.ChoiceRow, "Выбрать", global::sql.builder.Properties.Resources.GridChoiceRow, false);
//        //    AddBarButton(TextConst.AVGridButtonType.Refresh, "Обновить", global::sql.builder.Properties.Resources.Refresh_24, true);
//        //    AddBarButton(TextConst.AVGridButtonType.AddRow, "Добавить строку", global::sql.builder.Properties.Resources.GridRowAdd_24, true);
//        //    AddBarButton(TextConst.AVGridButtonType.DeleteRow, "Удалить строку", global::sql.builder.Properties.Resources.GridRowDelete_24, false);
//        //    AddBarButton(TextConst.AVGridButtonType.Commit, "Сохранить", global::sql.builder.Properties.Resources.Commit_24, true);
//        //    AddBarButton(TextConst.AVGridButtonType.Up, "Переместить выбранные строки выше", ((System.Drawing.Image)(resources.GetObject("ButtonUp.Glyph"))), true, false);
//        //    AddBarButton(TextConst.AVGridButtonType.Down, "Переместить выбранные строки ниже", ((System.Drawing.Image)(resources.GetObject("ButtonDown.Glyph"))), false, false);
            
          
//        //    var grp = AddBarMenu("lcExport", null, global::sql.builder.Properties.Resources.GridExportToFile_24, false);
//        //    grp.SetIsRight(true);
//        //    var btn = CreateBarButton(TextConst.AVGridButtonType.ExportExcel, "Экспорт в Excel", global::sql.builder.Properties.Resources.ExportXls_161, true);
//        //    grp.AddBarButton(btn, false);


//        //    grp = AddBarMenu("lcSettings", null, global::sql.builder.Properties.Resources.Settings_24, false);
//        //    grp.SetIsRight(true);
//        //    btn = CreateBarButton(TextConst.AVGridButtonType.SaveSettings, "Сохранить настройки колонок", null);
//        //    grp.AddBarButton(btn, false);
//        //    btn.SetVisible(false);
         
//        //    btn = CreateBarButton(TextConst.AVGridButtonType.RestoreSettings, "Сбросить настройки колонок", null);
//        //    grp.AddBarButton(btn, false);
//        //    btn.SetVisible(true);
//        //    btn.SetEnabled(false);
//        //    ((System.ComponentModel.ISupportInitialize)(this.barManager)).EndInit();
          
//        //}


//        //private IVBarButton CreateBarButton(string buttonType, string caption, Image image, bool visible = true)
//        //{
//        //    var btn = UIStatic.GetControlsfactory().CreateBarButton();
//        //    Cmn.SetProperty(this, buttonType, btn);
//        //    (btn as VBarButton).Name = buttonType;
//        //    btn.SetCaption(caption);
//        //    btn.SetImage(image);
            
//        //    return btn;

//        //}

//        //private void AddBarButton(string buttonType, string caption,Image image,bool beginGroup,bool visible=true)
//        //{
//        //    var btn = CreateBarButton(buttonType, caption, image, visible);
//        //    GetTopToolBar().AddBarButton(btn, beginGroup);
//        //    btn.SetVisible(visible);
         
//        //}

//        //private IVBarMenu AddBarMenu(string buttonType, string caption, Image image, bool beginGroup, bool visible = true)
//        //{
//        //    var btn = UIStatic.GetControlsfactory().CreateBarMenu();
//        //    Cmn.SetProperty(this, buttonType, btn);
//        //    (btn as VBarMenu).Name = buttonType;
//        //    GetTopToolBar().AddBarButton(btn, beginGroup);
//        //    btn.SetCaption(caption);
//        //    btn.SetImage(image);
//        //    btn.SetVisible(visible);
//        //    return btn;
//        //}

//        private void InitializeToolBars()
//        {
//            barTopToolbar = new VBar() {DockStyle = BarDockStyle.Top};
//            barBottomToolbar = new VBar() { DockStyle = BarDockStyle.Bottom, Visible = false};

//            foreach (VBar toolbar in new[] { barTopToolbar, barBottomToolbar })
//            {
//                toolbar.DockCol = 0;
//                toolbar.DockRow = 0;
//                toolbar.OptionsBar.AllowQuickCustomization = false;
//                toolbar.OptionsBar.DrawBorder = false;
//                toolbar.OptionsBar.DrawDragBorder = false;
//                toolbar.OptionsBar.MultiLine = true;
//                toolbar.OptionsBar.UseWholeRow = true;
//                barManager.Bars.Add(toolbar);
//            }
//        }


//        public IVBar GetTopToolBar()
//        {
//            return this.barTopToolbar;
//        }

//        public IVBar GetBottomToolBar()
//        {
//            return this.barBottomToolbar;
//        }

//        public event SimpleEventHandler VDisposed;


//        public void SetTopToolBarVisible(bool value)
//        {
//            barTopToolbar.Visible = value;
//        }

//        public void SetBottomToolBarVisible(bool value)
//        {
//            barBottomToolbar.Visible = value;
//        }

//        public void SetFooterVisible(bool value)
//        {
//            barFooter.Visible = value;
//        }




//        public void SetSummaryVisible(bool value)
//        {

//            GetGrid().SetShowFooter(value);
//            //foreach (GridView v in GetGridAsGridControl().ViewCollection.Cast<GridView>())
//            //{
//            //    v.OptionsView.ShowFooter = value;
//            //}
//        }


//        public void SetMultiselect(bool value)
//        {
//            GetGrid().SetMultiSelect(value);
//            //foreach (GridView v in GetGridAsGridControl().ViewCollection.Cast<GridView>())
//            //{
//            //    v.OptionsSelection.MultiSelect = value;
//            //}
//        }


//        public void SetMultiselectMode(bool isCell)
//        {
//            if (isCell)
//            {
//                GetGrid().SetSelectionMode(0);
//            }
//            else
//            {
//                GetGrid().SetSelectionMode(1);

//            }
           
//            //foreach (GridView v in GetGridAsGridControl().ViewCollection.Cast<GridView>())
//            //{
                
//            //    if (isCell)
//            //    {
//            //        v.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
//            //    }
//            //    else
//            //    {
//            //        v.OptionsSelection.MultiSelectMode = GridMultiSelectMode.RowSelect;

//            //    }
//            //}
//        }


//        public void SetTitle(string value)
//        {
//            GetGrid().SetViewTitle(null, value);
//            //var view = GetGridAsGridControl().MainView as GridView;
//            //view.ViewCaption = value;
//        }


//        private void InitializeGrid(bool isTree)
//        {
//            this.SuspendLayout();
//            if (isTree)
//            {
//                this.grid = UIStatic.GetControlsfactory().CreateTree();
//            }
//            else
//            {
//                this.grid = UIStatic.GetControlsfactory().CreateGrid();
//            }
//            grid.PrepareForData();
//            //this.grid = new ucGridWF();
//            ((System.ComponentModel.ISupportInitialize)(this.grid)).BeginInit();

//            GetGridAsDxEditorContainerControl().Dock = System.Windows.Forms.DockStyle.Fill;
//            GetGridAsDxEditorContainerControl().Location = new System.Drawing.Point(0, 39);
//            //this.grid.MainView = this.gridView;
//            GetGridAsDxEditorContainerControl().Name = "grid";
//            GetGridAsDxEditorContainerControl().Size = new System.Drawing.Size(686, 319);
//            GetGridAsDxEditorContainerControl().TabIndex = 5;
//            GetGridAsDxEditorContainerControl().ToolTipController = this.tooltip;
//            this.Controls.Add(GetGridAsDxEditorContainerControl());
//            ((System.ComponentModel.ISupportInitialize)(GetGridAsDxEditorContainerControl())).EndInit();
//            this.ResumeLayout(false);
//            this.PerformLayout();
//        }
//        //временно
//        //public DevExpress.XtraGrid.GridControl GetGridAsGridControl()
//        //{
           
//        //    return (DevExpress.XtraGrid.GridControl)GetGrid();
//        //}

//        public DevExpress.XtraEditors.Container.EditorContainer GetGridAsDxEditorContainerControl()
//        {

//            return (DevExpress.XtraEditors.Container.EditorContainer)GetGrid();
//        }
//        public IucGrid GetGrid()
//        {
//            return (IucGrid)grid;
//        }
//    }



    
//}
