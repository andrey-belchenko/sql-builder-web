//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using System.Xml.XPath;
//using DevExpress.Utils;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Docking2010.Views;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraGrid.Columns;
//using sql.builder.DataApi;
//using sql.builder.XmlHelpers;
//using sql.builder.WinForms;
//using sql.builder.UI;

//namespace sql.builder
//{
//    internal partial class ucQueriesEditorUses : ucBase
//    {
//        private VSXElement Element = null;
//        private ucQueryEditor _queryEditor = null;

//        public ucQueriesEditorUses()
//        {
//            InitializeComponent();
//        }
//        public void SetCurrentElement(ucQueryEditor queryEditor, VSXElement element)
//        {
//            _queryEditor = queryEditor;
//            Element = element;
//        }

//        public new void Refresh()
//        {
//            WaitUIHelper.LastUsedUIHelper.Show("Поиск ссылок ",WaitUIMode.WaitPanel);
//            DataTable tbl = Element.GetUsesAsDataTable();
//            gridControl2.DataSource = tbl;
//            Cmn.AddHtmlEditors(gridControl2, tbl);
//            (gridControl2.MainView as DevExpress.XtraGrid.Views.Grid.GridView).BestFitColumns();
//            setButtensState();
//            shownElement = Element;
//            WaitUIHelper.LastUsedUIHelper.Hide();
//        }
//        VSXElement shownElement = null;
//        public override void OnEnter()
//        {
//            if (Element != shownElement)
//            {
//                Refresh();
//            }

//        } 
//        private bool isRenamed()
//        {

//            if (Element != null)
//            {
//               return Element.IsRenamed();
//            }
//            return false;
//        }

//        private void setButtensState()
//        {
//            //if (isRenamed())
//            //{
//            //    ribSrcControl().btnRenameUses.Enabled = true;
//            //}
//            //else
//            //{
//            //    ribSrcControl().btnRenameUses.Enabled = false;
//            //}
//        }

//        private void gridView2_DoubleClick(object sender, System.EventArgs e)
//        {
//           var h= gridView2.FocusedRowHandle;
//           if (h != -1)
//           {
//               object node = gridView2.GetRowCellValue(h, "node");

//               VSXElement el = (VSXElement)node;
//               //gridView2.SetRowCellValue(h, "node", el);
//               if (node != null)
//               {
//                   ucQueriesEditor.GetInstance().OpenQuery(el.GetMainParent(), el);
//                  // OpenQuery(el, null);
//               }
//           }
//        }

//        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            this.GetCurrentControl<ucQueriesEditorUses>().Refresh();
//        }
//    }
//}