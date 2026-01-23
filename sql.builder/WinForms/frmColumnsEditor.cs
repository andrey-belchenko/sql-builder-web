//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.Utils;
////using DevExpress.XtraBars;
////using DevExpress.XtraGrid.Views.BandedGrid;
////using DevExpress.XtraGrid.Views.Grid;
////using DevExpress.XtraGrid.Views.Grid.ViewInfo;

////using DevExpress.XtraTreeList;
////using DevExpress.XtraTreeList.Nodes;
//using infoenergo.core.Extensions;
//using infoenergo.ui.win.Forms;

//namespace sql.builder.WinForms
//{
//    internal partial class frmColumnsEditor : FormBase
//    {
//        #region Закрытые переменные
//  //      private DataTable _dt_invisible;
//        private DataTable _dt_visible=null;

//        private string _bands_sep = @" \\ ";

//        private GridHitInfo _downHitInfo;
//       // private int _dropTargetRowHandle = -1;

//	    private XElement _xAllColumns;
//        #endregion
//        #region Свойства
//        //
//        #endregion
//        #region События
//        //
//        /// <summary>
//        /// Выбрать все колонки
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void barLargeButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            CheckAll();
//        }

//        /// <summary>
//        /// Снять выбор с колонок 
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void barButtonItem7_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ClearAll();
//        }

//        /// <summary>
//        /// Закончили выбор колонок
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void barButtonItem4_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            DialogResult = DialogResult.OK;
//        }

//        /// <summary>
//        /// Отменили выбор колонок
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void barButtonItem5_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            DialogResult = DialogResult.Cancel;
//        }

//        /// <summary>
//        /// Передвигаем выбранную колонку вниз по списку
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void barButtonItem9_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            moveSelected(treeList2, 1);
//        }

//        /// <summary>
//        /// Передвигаем выбранную колонку вверх по списку
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            moveSelected(treeList2, -1);
//        }

//        /// <summary>
//        /// Отметить выбранные строки
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            CheckSelect();
//        }

//        /// <summary>
//        /// Снять выбор выделенных строк
//        /// </summary>
//        /// <param name="sender"></param>
//        /// <param name="e"></param>
//        private void barButtonItem11_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            ClearSelect();
//        }
//        #endregion
//        #region Открытые методы
//        public frmColumnsEditor()
//        {
//            InitializeComponent();
//        }

//        public void Initialize(XElement xAllColumns, XElement xSelectedColumns)
//        {
//            GenerateColumnsData(xAllColumns,xSelectedColumns);
//        }

//        public XElement GetColumnsXml()
//        {
//			/* if (viewShowCols.Bands.Count == 1 && viewShowCols.Bands[0].Caption == "")
//			 {
//				 AddColumnsToXml(xViewColumns, viewShowCols.Bands[0].Columns.Cast<BandedGridColumn>());
//			 }
//			 else 
//			 {
//				 AddBandsToXml(xViewColumns, viewShowCols.Bands.Cast<GridBand>());
//			 }*/
//			var xViewColumns = new XElement("viewcolumns", Cmn.DataTableToXElements(selectedColumns));
//			return xViewColumns;
//        }
//        #endregion
//        #region Закрытые методы
     
//        DataTable allColumns = null;
//        DataTable selectedColumns = null;
//        private void GenerateColumnsData(XElement xAllColumns, XElement xSelectedColumns)
//        {
//			//XElement xAllColumns, XElement xSelectedColumns
//			// treeList1.BeginUpdate();
//			_xAllColumns = xAllColumns = new XElement(xAllColumns);
//            xSelectedColumns = new XElement(xSelectedColumns);

//            xAllColumns.Descendants().Where(e => Cmn.GetAttrValue(e, "title") == "").Remove();

//            // объединяем бэнды с одинаковым title в один
//            var xbands = xAllColumns.Elements("band").ToArray();
//            foreach (var xband in xbands)
//            {
//                if (!xband.HasElements) continue;

//                var multiple_bands = xbands.Where(band => band != xband && xband.Attribute("title").Value == band.Attribute("title").Value);
//                foreach (var multiple_band in multiple_bands)
//                {
//                    xband.Add(multiple_band.Elements());

//                    multiple_band.RemoveNodes();
//                    multiple_band.Remove();
//                }
//            }

//            xSelectedColumns.Descendants().Where(e => Cmn.GetAttrValue(e, "title") == "").Remove();
//            xSelectedColumns.Descendants().Where(e => Cmn.GetAttrValue(e, "visible") == "0").Remove();
//            allColumns = Cmn.XElementsToDataTable(xAllColumns.Elements());
//            allColumns.Columns.Add("selid");
//            allColumns.Columns.Add("spelid");
//            foreach (DataRow row in allColumns.Rows)
//            {
//                row["selid"] = row["elid"];
//                row["spelid"] = row["pelid"];
//            }
//            treeList1.DataSource = allColumns;
//            treeList1.RefreshDataSource();
//          //  treeList1.PopulateColumns();
//            treeList1.ExpandAll();
//           // System.Windows.Forms.Application.DoEvents();
          
//            selectedColumns = allColumns.Clone();
           
//            treeList2.DataSource = selectedColumns;


//           // treeList1.EndUpdate();
//           // System.Windows.Forms.Application.DoEvents();
//         //  .._dt_visible.Rows.Clear();
//          //  _dt_invisible.Rows.Clear();


            
//            //List<DataRow> rows = allColumns.AsEnumerable().Where(r => xSelectedColumns.Descendants().Attributes("name").Select(a=>a.Value).Contains( r["name"].ToString() )).ToList();
//            List<TreeListNode> nodes = new List<TreeListNode>();
//            foreach (string name in xSelectedColumns.Descendants().Attributes("name").Select(a => a.Value).ToArray())
//            {
//                DataRow r1 = allColumns.AsEnumerable().First(r => r["name"].ToString() == name);
//                TreeListNode node = getNodeById(treeList1, r1["elid"].ToString());
//                node.Checked = true;
//                nodes.Add(node);
               
//            }
//            checkProcessing(null,nodes);
 
//        }

//        private void CheckAll()
//        {
//            List<TreeListNode> nodes=treeList1.Nodes.Cast<TreeListNode>().Where(n=>n.Visible==true).ToList();
//            foreach (TreeListNode node in nodes)
//            {
//                node.Checked = true;
//            }
//            checkProcessing(null, nodes );
//        }

//        private void CheckSelect()
//        {
//            List<TreeListNode> nodes = treeList1.Selection.Cast<TreeListNode>().ToList();
//            foreach (TreeListNode node in nodes)
//            {
//                node.Checked = true;
//            }
//            checkProcessing(null, nodes);
//        }

//        private void ClearAll()
//        {
//            List<TreeListNode> nodes = treeList1.Nodes.Cast<TreeListNode>().Where(n => n.Visible == true).ToList();
//            foreach (TreeListNode node in nodes)
//            {
//                node.Checked = false;
//            }
//            checkProcessing(null, nodes);
//        }

//        private void ClearSelect()
//        {
//            List<TreeListNode> nodes = treeList1.Selection.Cast<TreeListNode>().ToList();
//            foreach (TreeListNode node in nodes)
//            {
//                node.Checked = false;
//            }
//            checkProcessing(null, nodes);
//        }

//        private DataRow GetCopiedRow(DataRow source_row, DataRow copied_row)
//        {
//            for (int i = 0; i < source_row.ItemArray.Length; i++) {
//                copied_row[i] = source_row[i];
//            }
//            return copied_row;
//        }
//        private IEnumerable<string> GetBandsArray(string bands)
//        {
//            var result = bands.Replace("[пустой]", "").Split(new[] {_bands_sep}, StringSplitOptions.None);
//            return result.IsEmpty() ? new[] { "" } : result;
//        }

//        private BandedGridColumn GenerateColumn(DataRow column_data)
//        {
//            var column = new BandedGridColumn()
//            {
//                Caption = (string)column_data["title"],
//                Width = 100,
//                Tag = column_data
//            };

//            if (column_data.Table == _dt_visible)
//            {
//                column.VisibleIndex = 0;
//            }

//            column.OptionsColumn.FixedWidth = true;

//            return column;
//        }

//        private void AddBandsToXml(XElement element, IEnumerable<GridBand> bands)
//        {
//            foreach (var band in bands)
//            {
//                var xml_band = new XElement("band",
//                    new XAttribute("title", band.Caption));

//                if (band.Children.Count > 0)
//                {
//                    element.Add(xml_band);
//                    AddBandsToXml(xml_band, band.Children.Cast<GridBand>());
//                }
//                else if (band.Columns.Count > 0)
//                {
//                    element.Add(xml_band);
//                    AddColumnsToXml(xml_band, band.Columns.Cast<BandedGridColumn>());
//                }
//            }
//        }
//        private void AddColumnsToXml(XElement element, IEnumerable<BandedGridColumn> columns)
//        {
//            foreach (var column in columns)
//            {
//                var row = (DataRow) column.Tag;
//                var visible = row.Table == _dt_visible ? "1" : "0";
//                element.Add( new XElement("column", 
//                                new XAttribute("name"   , row["name"]),
//                                new XAttribute("title"  , row["title"]),
//                                new XAttribute("width"  , row["width"]),
//                                new XAttribute("type"   , row["type"]),
//                                new XAttribute("visible", visible),
//                                row["format"].ToString()   != "" ? new XAttribute("format"  , row["format"])   : null,
//                                row["group"].ToString()    != "" ? new XAttribute("group"   , row["group"]) : null,
//                                row["sort"].ToString()     != "" ? new XAttribute("sort"    , row["sort"]) : null,
//                                row["editable"].ToString() != "" ? new XAttribute("editable", row["editable"]) : null,
//                                row["editor"].ToString()   != "" ? new XAttribute("editor"  , row["editor"])   : null));
//            }
//        }

     
      
//        #endregion
//        #region Обработчики событий
//        private void btnUp_Click(object sender, EventArgs e)
//        {
//            //MoveVisibleColumns("up");
          
//        }
//        private void btnDown_Click(object sender, EventArgs e)
//        {
            
//        }

//        private void btnAccept_Click(object sender, EventArgs e)
//        {
            
//        }
//        private void btnCancel_Click(object sender, EventArgs e)
//        {
           
//        }
//        #endregion
//        #region DragAndDrop
//        private void view_MouseDown(object sender, MouseEventArgs e)
//        {
//            var view = (GridView) sender;

//            _downHitInfo = null;

//            var hitInfo = view.CalcHitInfo(new Point(e.X, e.Y));

//            if (Control.ModifierKeys != Keys.None) return;

//            if (e.Button == MouseButtons.Left && hitInfo.RowHandle >= 0)
//            {
//                _downHitInfo = hitInfo;
//            }
//        }
//        private void view_MouseMove(object sender, MouseEventArgs e)
//        {
//            var view = sender as GridView;

//            if (e.Button == MouseButtons.Left && _downHitInfo != null)
//            {
//                var dragSize = new Size(250, 50);//SystemInformation.DragSize;
//                var dragRect = new Rectangle(new Point(_downHitInfo.HitPoint.X - dragSize.Width / 2,
//                    _downHitInfo.HitPoint.Y - dragSize.Height / 2), dragSize);

//                if (!dragRect.Contains(new Point(e.X, e.Y)))
//                {
//                    DataRow[] data = view.GetSelectedRows().Select(view.GetDataRow);
//                    view.GridControl.DoDragDrop(data, DragDropEffects.Move);

//                    _downHitInfo = null;
//                    DXMouseEventArgs.GetMouseArgs(e).Handled = true;
//                }
//            }
//        }     
//        #endregion

//        private void frmColumnsEditor_Load(object sender, EventArgs e)
//        {

//        }

//        bool autoCheckProcessing = false;
//        private void treeList1_AfterCheckNode(object sender, NodeEventArgs e)
//        {          
//            checkProcessing(e.Node);
//        }

//        private void setChecked(TreeListNode node,bool ch,bool force=false)
//        {
//            if (node.Checked != ch || force)
//            {
//                node.Checked = ch;
//                if (node.Nodes.Count == 0)
//                {
//                    updateColumn(node);
//                }
//            }
//        }

//        private void updateColumn(TreeListNode node)
//        {
//            updateParent(node);
//            DataRow row;
           
//            row = selectedColumns.Rows.Cast<DataRow>().Where(r => r["selid"] == node.GetValue("elid")).FirstOrDefault();
//            if (node.Checked)
//            {
//                if (row == null)
//                {
                    
//                    row = (treeList1.GetDataRecordByNode(node) as DataRowView).Row;
//                    string ord = row["ord"].ToString();
//                   // row["ord"] = currentOrdIndex;
//                    selectedColumns.ImportRow(row);
//                    selectedColumns.Rows[selectedColumns.Rows.Count - 1]["ord"] = currentOrdIndex;
//                   // row["ord"] = ord;
//                    treeList2.ForceInitialize(); 
//             //       TreeListNode node1 = treeList2.Nodes.Cast<TreeListNode>().First(e => (treeList2.GetDataRecordByNode(e) as DataRowView).Row["elid"] == row["elid"]);
//               //     expandParents(node1);
//                }
//            }
//            else
//            {
//                if (row != null)
//                {

//                    row.Delete();
//                }
//            }
//        }

//        private void expandParents(TreeListNode node)
//        {
//            if (node != null)
//            {
//                node.Expanded = true;
//                expandParents(node.ParentNode);
//            }
//        }

//        private void updateParent(TreeListNode node)
//        {
//            if (node.ParentNode != null)
//            {
                
//                DataRow row;

//                row = selectedColumns.Rows.Cast<DataRow>().Where(r => r["elid"] == node.ParentNode.GetValue("elid")).FirstOrDefault();
//                if (node.ParentNode.Checked)
//                {
//                    if (row == null)
//                    {
//                        updateParent(node.ParentNode);
//                        row = (treeList1.GetDataRecordByNode(node.ParentNode) as DataRowView).Row;
//                       // selectedColumns.ImportRow(row);
//                    }
//                }
//                else
//                {
//                    if (row != null)
//                    {

//                        row.Delete();
//                    }
//                }

//            }
//        }

//        int currentOrdIndex = 0;
//        private int getCurrentOrdIndex()
//        {
//            int i =-1;
//            if (treeList2.Selection.Count > 0)
//            {
//                DataRow row = (treeList2.GetDataRecordByNode(treeList2.Selection[treeList2.Selection.Count - 1]) as DataRowView).Row;
//                i = Convert.ToInt32(row["ord"]);


//            }
//            else
//            {
//                if (selectedColumns.Rows.Count > 0)
//                {
//                    i = selectedColumns.AsEnumerable().Select(r => Convert.ToInt32(r["ord"])).Max();
//                }
//            }
//            return i;
//        }

//        private void checkProcessing(TreeListNode node, List<TreeListNode> nodes=null)
//        {

//            if (autoCheckProcessing) return;

//            currentOrdIndex = getCurrentOrdIndex()+1;

//            autoCheckProcessing = true;
//            selProcessing = true;
       
//            if (nodes == null)
//            {
//                if ((treeList1.Selection.Cast<TreeListNode>().FirstOrDefault(e => e.Equals(node)) == null))
//                {
//                    treeList1.Selection.Clear();

//                    node.Selected = true;
//                }
//                nodes = treeList1.Selection.Cast<TreeListNode>().ToList();
              
//            }


//            foreach (TreeListNode node1 in nodes)
//            {
//                checkParents(node1);
//                if (node != null)
//                {
//                    setChecked(node1, node.Checked, true);
//                }
//                else
//                {
//                    setChecked(node1, node1.Checked, true);
//                }
//                checkChilds(node1);
//                currentOrdIndex++;
                
//            }
//            setOrder(treeList2);
//        //    updateBranchesOrder(treeList2);
//            regroupLeafs(treeList2, treeList1);
//            List<DataRow> selectedRows = null;
//            if (node != null)
//            {
//                if (node.Checked)
//                {
//                    //treeList1.Selection.Set(nodes);
//                    selectedRows = treeList1.Selection.Cast<TreeListNode>().Select(n => ((DataRowView)treeList1.GetDataRecordByNode(n)).Row).Where(r => r["leaf"].ToString() == "1").ToList();

//                    treeList2.Selection.Set(getNodesByRows(treeList2, selectedRows));
//                }
//            }


//            selProcessing = false;
//            autoCheckProcessing = false;
//        }

//        private void setOrder(TreeList tree)
//        {

//            int i = 0;
//            foreach (TreeListNode node in getLeafs(tree))
//            {
//                node.SetValue("ord", i);
//                i++;
//            }

//        }

//        private void updateBranchesOrder(TreeList tree)
//        {
//            return;

//            //DataTable tbl = (tree.DataSource as DataTable).Clone();
//            //List<DataRow> rows = getAllNodes(tree).Select(n => (tree.GetDataRecordByNode(n) as DataRowView).Row).ToList();
//            //foreach (DataRow row in rows)
//            //{
//            //    tbl.ImportRow(row);
//            //}
//            //VDataTable.calculateTreeColumn(tbl, "ord", "min",true);
//            //foreach (TreeListNode node in getBranches(tree))
//            //{
//            //    DataRow row = tbl.AsEnumerable().First(r => r["elid"] == node.GetParamsXml("elid"));
//            //    node.SetValue("ord",row["ord"]);
//            //}
//        }

//        private void checkChilds(TreeListNode node)
//        {
            

//            foreach (TreeListNode node1 in node.Nodes)
//            {
//                setChecked( node1, node.Checked);
//                checkChilds(node1);
//            }

//        }

//        private void checkParents(TreeListNode node)
//        {

//                if (node.ParentNode != null)
//                {
//                    if ((node.ParentNode.Nodes.Cast<TreeListNode>().FirstOrDefault(e => e.Checked) != null))
//                    {
                      
//                        setChecked(node.ParentNode, true);
//                    }
//                    else
//                    {
//                        setChecked(node.ParentNode, false);
//                    }
                   
//                    checkParents(node.ParentNode);
//                }
            

//        }

//        private void treeList2_NodeChanged(object sender, NodeChangedEventArgs e)
//        {

            
//            expandParents(e.Node);


//            e.Node.TreeList.Selection.Clear();
//            e.Node.Selected = true;
//        }

//        private List<TreeListNode> getNodesByRows(TreeList tree,List<DataRow> rows)
//        {
//            HashSet<string> ids = new HashSet<string>();
//            for (int index = 0; index < rows.Count; index++) {
//                DataRow row = rows[index];
//                string id = row["elid"].ToString();
//                if (!ids.Contains(id)) {
//                    ids.Add(id);
//                }
//            }
//            List<TreeListNode> nodes = getAllNodes(tree).Where(n => ids.Contains(((DataRowView)tree.GetDataRecordByNode(n)).Row["elid"])).ToList();
//            return nodes;
//        }

//        private TreeListNode getNodeById(TreeList tree, string elid)
//        {
//            TreeListNode node = getAllNodes(tree).Where(n => elid == ((DataRowView)tree.GetDataRecordByNode(n)).Row["elid"].ToString()).FirstOrDefault();

//            return node;
//        }

//        private List<TreeListNode> getAllNodes(TreeList tree)
//        {
//            List<TreeListNode> nodes = new List<TreeListNode>();
//            foreach (TreeListNode node in tree.Nodes)
//            {
//                nodes.Add(node);
//                getAllNodes(node,  nodes);
//            }
//            return nodes;

//        }

//        private void getAllNodes(TreeListNode parent, List<TreeListNode> outNodes)
//        {
           
//            foreach (TreeListNode node in parent.Nodes)
//            {
//                outNodes.Add(node);
//                getAllNodes(node, outNodes);
//            }


//        }

//        private List<TreeListNode> getLeafs(TreeList tree)
//        {
          
//            return getAllNodes(tree).Where(n=>n.Nodes.Count==0).ToList();

//        }
//        private List<TreeListNode> getBranches(TreeList tree)
//        {
          
//            return getAllNodes(tree).Where(n=>n.Nodes.Count>0).ToList();

//        }
        
//        private void moveSelected(TreeList tree,int step)
//        {
//            List<DataRow> selectedRows = tree.Selection.Cast<TreeListNode>().Select(n => ((DataRowView)tree.GetDataRecordByNode(n)).Row).Where(r=>r["leaf"].ToString()=="1").ToList();
//            if (selectedRows.Count == 0) return;
//            int position = selectedRows.Min(r => (int)r["ord"]);
//            if (position + step >= 0 && position + step + selectedRows.Count <= getLeafs(tree).Count) {
//                position = position + step;
//            }
//            moveRowsTo(tree, selectedRows, position);

//            regroupLeafs(tree,treeList1);
//           // updateBranchesOrder(tree);

//            List<TreeListNode> nodes = getNodesByRows(tree, selectedRows);
//            treeList2.SetFocusedNode(nodes[0]);
//            treeList2.Selection.Set(nodes);

          

//        }

//        private void regroupLeafs(TreeList tree,TreeList sourceTree)
//        {
//            //tree.BeginUpdate();
//            DataTable tbl = (tree.DataSource as DataTable);
//            DataTable srcTbl = (sourceTree.DataSource as DataTable);
//            DataTable newTbl = tbl.Clone();
//            List<DataRow> rows = tbl.AsEnumerable().Where(r1 => r1["leaf"].ToString() == "1").OrderBy(r => r["ord"]).ToList();
                
//              //  getLeafs(tree).Select(n => (tree.GetDataRecordByNode(n) as DataRowView).Row).OrderBy(r=>r["ord"]).ToList();
         
//            foreach (DataRow row in rows)
//            {
//                newTbl.ImportRow(row);
//            }
//            List<DataRow> newRows = new List<DataRow>(); 
//            foreach (DataRow row in newTbl.Rows)
//            {
//                newRows.Add(row);
//            }

//            SortedList<int,DataRow> cursor =new SortedList<int,DataRow>();

//            int parentIndex=0;

//            foreach (DataRow row in newRows)
//            {
//                DataRow parent = row;
//                List<DataRow> parents = new List<DataRow>(); 
//                while (Cmn.Nvl(parent["pelid"],"").ToString() != "")
//                {
//                   parent= srcTbl.AsEnumerable().Where(r => r["elid"] == parent["spelid"]).FirstOrDefault();
//                   parents.Add(parent);
//                }
//                parents.Reverse();
//                parent = null;
//                for (int i = 0 ; i <parents.Count; i++)
//                {
//                    DataRow srcParent = parents[i];
//                    bool exists = false;
//                    if (cursor.ContainsKey(i))
//                    {
//                        if (cursor[i]["selid"] == srcParent["elid"])
//                        {
//                            exists = true;
//                        }
//                    }
//                    if (!exists)
//                    {
//                        newTbl.ImportRow(srcParent);
//                        parent = newTbl.Rows[newTbl.Rows.Count - 1];

//                        parent["ord"] = row["ord"];
//                        parent["elid"] = "p" + parentIndex.ToString();
//                        if (i > 0)
//                        {
//                            parent["pelid"] = cursor[i - 1]["elid"];
//                        }
//                        if (cursor.ContainsKey(i))
//                        {
//                            cursor.Remove(i);
//                        }
//                        cursor.Add(i, parent);
//                        parentIndex++;
//                    }
//                }

//                for (int i = parents.Count; i < cursor.Keys.Count; i++)
//                {
//                    cursor.Remove(i);
//                }
//                if (cursor.Keys.Count > 0)
//                {
//                    row["pelid"] = cursor[cursor.Keys.Count - 1]["elid"];
//                }
               
               
//            }
//            selectedColumns = newTbl;
//            tree.DataSource =  selectedColumns;
//           // tree.EndUpdate();
//            tree.RefreshDataSource();
//        }

//        private void moveRowsTo(TreeList tree, List<DataRow> rows,int position)
//        {
//            HashSet<string> selectedIds = new HashSet<string>();
//            int minI = 0;
//            int maxI = 0;
//            for (int index = 0; index < rows.Count; index++) {
//                DataRow row = rows[index];
//                string id = row["elid"].ToString();
//                if (!selectedIds.Contains(id)) {
//                    selectedIds.Add(id);
//                }
//                int ord = (int)row["ord"];
//                if (index == 0) {
//                    minI = ord;
//                    maxI = ord;
//                } else {
//                    if (ord < minI) {
//                        minI = ord;
//                    }
//                    if (ord > maxI) {
//                        maxI = ord;
//                    }
//                }
//            }
//            if (minI > position) {
//                minI = position;
//            }
//            if (maxI < position+rows.Count-1) {
//                maxI = position + rows.Count - 1;
//            }
//            List<DataRow> otherRows = new List<DataRow>();
//            foreach (TreeListNode node in this.getLeafs(tree)) {
//                DataRow row = ((DataRowView)tree.GetDataRecordByNode(node)).Row;
//                string id = row["elid"].ToString();
//                int ord = (int)row["ord"];
//                if (ord >= minI && ord <= maxI && !selectedIds.Contains(id)) {
//                    otherRows.Add(row);
//                }
//            }
//            int i = minI;

//            foreach (DataRow row in otherRows)
//            {
//                if (i == position)
//                {
//                    foreach (DataRow row1 in rows)
//                    {
//                        row1["ord"] = i;
//                        i++;
//                    }
//                }
//                row["ord"] = i;
//                i++;
//            }
//            if (i == position)
//            {
//                foreach (DataRow row1 in rows)
//                {
//                    row1["ord"] = i;
//                    i++;
//                }
//            }
//        }

//        bool selProcessing = false;
//        private void treeList2_SelectionChanged(object sender, EventArgs e)
//        {
//            return;
//            if (!selProcessing)
//            {
//                selProcessing = true;
//                treeList2.Selection.Set(collectChilds(treeList2.Selection.Cast<TreeListNode>().ToList()));
//                List<DataRow> selectedRows = treeList2.Selection.Cast<TreeListNode>().Select(n => ((DataRowView)treeList2.GetDataRecordByNode(n)).Row).Where(r => r["leaf"].ToString() == "1").ToList();
//                treeList1.Selection.Set(getNodesByRows(treeList1, selectedRows));
//                selProcessing = false;
//            }
          
            
            
//        }

//        private void treeList1_SelectionChanged(object sender, EventArgs e)
//        {
//            if (!selProcessing)
//            {
//                selProcessing = true;
//                treeList1.Selection.Set(collectChilds(treeList1.Selection.Cast<TreeListNode>().ToList()));
//                List<DataRow> selectedRows = treeList1.Selection.Cast<TreeListNode>().Select(n => ((DataRowView)treeList1.GetDataRecordByNode(n)).Row).Where(r => r["leaf"].ToString() == "1").ToList();
//                treeList2.Selection.Set(getNodesByRows(treeList2, selectedRows));
//                selProcessing = false;
//            }
            
//        }

//        private static List<TreeListNode> collectChilds(List<TreeListNode> nodes)
//        {
//            List<TreeListNode> rNodes = new List<TreeListNode>();
//            foreach (TreeListNode node in nodes)
//            {
//                rNodes.Add(node);
//                collectChilds(node, rNodes);

//            }
//            return rNodes;
//        }

//        private static void collectChilds(TreeListNode node, List<TreeListNode> nodes)
//        {
//            foreach (TreeListNode childNode in node.Nodes)
//            {
//                if (nodes.Cast<TreeListNode>().Where(n => n.Equals(childNode)).Count() == 0)
//                {
//                    nodes.Add(childNode);
//                    collectChilds(childNode, nodes);
//                }
//            }
//        }

//        private void pFooter_Paint(object sender, PaintEventArgs e)
//        {

//        }

//        private void simpleButton2_Click(object sender, EventArgs e)
//        {
            
//        }

//        private void simpleButton1_Click(object sender, EventArgs e)
//        {
            
//        }

//    }
//}
