//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
//using DevExpress.XtraEditors;
//using System.Xml;
//using System.Xml.Linq;
//using DevExpress.XtraTreeList.Columns;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraGrid;
//using sql.builder.DataApi;
//using sql.builder.WinForms;

//namespace sql.builder
//{
//    internal partial class ucQueryEditorContextList : ucBase
//    {
//        public ucQueryEditorContextList()
//        {
//            InitializeComponent();
//        }

//        public ucQueryEditorContextManager Manager=null;


//        public override void OnEnter()
//        {
//            UpdateData();
//        }


//        public void UpdateData()
//        {
//            if (Info == null)
//            {
//                return;
//            }
//            DataTable dt = Info.GetContent();
//           // MessageBox.Show(IsTabbedVisible().ToString());
//            var textCol = (VDataColumn)dt.Columns["NodeText"];
//            bool isNew = true;
//            if (gridView.Columns.Count > 0)
//            {
//                isNew = false;
//            }
//            var dtOld = gridControl.DataSource;
//            gridControl.DataSource = null;

//            gridControl.DataSource = dt;
//            //this.treeList.DataSource = null;
//            //this.treeList.DataSource = dt;


//            //foreach ( TreeListColumn col in  this.treeList.Columns.Cast<TreeListColumn>() .Where(e=> Cmn.XElementsToDataTableSysFieldsNames.Contains( e.FieldName)))
//            //{
//            //   col.Visible = false;
//            //}


//            //gridView.Columns["NodeName"].Visible = false;
//            //gridView.Columns["node"].Visible = false;
//            //gridView.Columns["id"].Visible = false;
//            //gridView.Columns["parent_id"].Visible = false;

//            textCol.IsHtml = true;
//            if (isNew)
//            {
//                foreach (GridColumn col in gridView.Columns)
//                {
//                    if (col.FieldName != textCol.ColumnName)
//                    {
//                        col.Visible = false;
//                    }
//                }


//                Cmn.AddHtmlEditors(gridControl, dt);

//            }

//            if (dtOld != null)
//            {
//                UseOldVisibility((DataTable)dtOld, dt);
//            }

//            Info = null;
//        }
//        VContextListsInfo Info = null;

//        public void TryUpdateData(VContextListsInfo info)
//        {
//            Info = info;
//            if (IsTabbedVisible())
//            {
//                UpdateData();
//            }
            
//        }

//        //private void treeList_DoubleClick(object sender, EventArgs e)
//        //{
//        //    if (treeList.Selection.Count > 0)
//        //    {
                
                
//        //        DataRow r = Cmn.GetNodeRow(treeList.Selection[0]);

//        //        if (r != null)
//        //        {
//        //            XElement el = (XElement)r["node"];
//        //            Manager.RaiseItemSelected(el);
//        //        }
                
//        //    }
//        //}

//        private void gridView_DoubleClick(object sender, EventArgs e)
//        {
//            this.GetCurrentControl<ucQueryEditorContextList>().AddSelected();
//            //if (gridView.SelectedRowsCount>0)
//            //{

//            //    foreach (int i in gridView.GetSelectedRows())
//            //    {
//            //        var el = (XElement)gridView.GetRowCellValue(i, "node");
//            //        Manager.RaiseItemSelected(el);
//            //    }
//            //}
//        }
//        public string GetInfo()
//        {
//            return (gridControl.DataSource as DataTable).TableName;
//        }


//        private string  hdtCpt = "";

//        private string getHdrCpt()
//        {
//            if (hdtCpt == "")
//            {
//                hdtCpt =  hdrContextName.Caption;
//            }
//            return hdtCpt;
//        }
//        public override void UpdateRibbon()
//        {
//            ucQueryEditorContextList rib = this.GetRibbonSource<ucQueryEditorContextList>();
//            rib.hdrContextName.Caption = rib.getHdrCpt() + Manager.Info + "/" + GetInfo();
//            rib.barButtonItem1.Enabled = true;
//            rib.btnColsSelect.Enabled = true;
//        }
//        public override void OnDeactivate()
//        {
//            ucQueryEditorContextList rib = this.GetRibbonSource<ucQueryEditorContextList>();
//            rib.hdrContextName.Caption = rib.getHdrCpt();
//            rib.barButtonItem1.Enabled = false;
//            rib.btnColsSelect.Enabled = false;
//        }
//        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucQueryEditorContextList>().AddSelected();
//        }

//        public void AddSelected()
//        {
//            if (gridView.SelectedRowsCount > 0)
//            {
//                List<XElement> selectedElements = new List<XElement>();
//                foreach (int i in gridView.GetSelectedRows())
//                {
//                    var el = (XElement)gridView.GetRowCellValue(i, "node");
//                    selectedElements.Add(el);
                 
//                }
//                Manager.RaiseItemSelected(selectedElements);
//            }
//        }

//        private void btnColsSelect_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucQueryEditorContextList>().selectColumns();
//        }

//        public void RefreshQueryColumn(string name)
//        {
//            var dt = (gridControl.DataSource as VDataTable);
//            foreach (DataRow r in dt.Rows)
//            {
//                var el = (r["node"] as VSXElement);
//                el.UpdateDataCell(name);
//            }

//        }

//        private void UseOldVisibility(DataTable dtOld, DataTable dtNew)
//        {
//            foreach (VDataColumn colNew  in dtNew.Columns)
//            {
//                var colOld = dtOld.Columns.Cast<VDataColumn>().FirstOrDefault(e => e.ColumnName ==colNew.ColumnName);

//                if (colOld != null)
//                {
//                    if (colOld.Visible!=colNew.Visible)
//                    {
//                        colNew.Visible = colOld.Visible;
//                        if (colNew.Visible)
//                        {
//                            RefreshQueryColumn(colNew.ColumnName);
//                        }

//                    }
//                }

//            }


//        }


//        private void selectColumns()
//        {

//            GridView  tree = gridView;
//            VDataTable tbl = (VDataTable)gridControl.DataSource;
//            //VDataSet dataSet = (VDataSet)tbl.DataSet;

//            XElement scheme = VDataSet.GetXmlSchemeFromDataTable(tbl);
//            var xViewColumns_old = scheme.Element(TextConst.EName.Table).Element(TextConst.EName.Columns);
//            var xViewColumns_preset = new XElement(xViewColumns_old);


//            foreach (XElement xcol in xViewColumns_preset.Elements(TextConst.EName.Column).ToList())
//            {
//                var dataColumn = (VDataColumn)tbl.Columns[xcol.Attribute(TextConst.AName.Name).Value];
//                if (!dataColumn.Visible)
//                {
//                    xcol.Remove();
//                }
//            }


//            using (var frm = new frmColumnsEditor())
//            {
//                frm.Initialize(xViewColumns_old, xViewColumns_preset);
//                if (frm.ShowDialog() != DialogResult.OK) return;

//                var xViewColumns_new = frm.GetColumnsXml();
//                foreach (XElement xcol in xViewColumns_old.Elements(TextConst.EName.Column).ToList())
//                {
//                    var dataColumn = (VDataColumn)tbl.Columns[xcol.Attribute(TextConst.AName.Name).Value];
//                    var newXCol = xViewColumns_new.Elements(TextConst.EName.Column).FirstOrDefault(e => e.Attribute(TextConst.AName.Name).Value == dataColumn.ColumnName);
//                    var gCol = tree.Columns[dataColumn.ColumnName];
//                    var oldVisible = dataColumn.Visible;
//                    if (newXCol != null)
//                    {
//                        dataColumn.Visible = true;
//                        if (!oldVisible)
//                        {
//                            RefreshQueryColumn(dataColumn.ColumnName);
//                        }
//                        gCol.Visible = true;
//                    }
//                    else
//                    {
//                        dataColumn.Visible = false;
//                        gCol.Visible = false;
//                    }
//                }

//                var i = 0;
//                foreach (XElement xcol in xViewColumns_new.Elements(TextConst.EName.Column).ToList())
//                {
//                    var gCol = tree.Columns[xcol.Attribute(TextConst.AName.Name).Value];
//                    gCol.VisibleIndex = i;
//                    i++;
//                }

//                // xViewColumns_preset.ReplaceWith(xViewColumns_new);

//                // current_gc.UpdateGridSettings(fixed_scheme: true);
//            }
//        }

      
         
//    }

   
//}
