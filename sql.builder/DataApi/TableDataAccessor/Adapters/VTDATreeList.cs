//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using Devart.Data.Oracle;
////using DevExpress.Data;
////using DevExpress.Utils;
////using DevExpress.XtraBars;
////using DevExpress.XtraBars.Controls;
////using DevExpress.XtraBars.Utils;
////using DevExpress.XtraEditors;
////using DevExpress.XtraEditors.Controls;
////using DevExpress.XtraEditors.Repository;
////using DevExpress.XtraTreeList;
////using DevExpress.XtraTreeList.Columns;
////using DevExpress.XtraTreeList.Nodes;
////using DevExpress.XtraTreeList.ViewInfo;

////using DevExpress.XtraPrinting;

//using sql.builder.DataApi;
//using sql.builder.DataApi.DataObjects;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;


//using sql.builder.UI.WinForms;

//namespace sql.builder.DataApi.TableDataAccessor.Adapters
//{
//    internal class VTDATreeList:IVTableDataAdapter
//    {
//        private TreeList _data;
//        public VTDATreeList(TreeList data)
//        {
//            _data = data;
//        }
//        public object[] GetSelectedCellsValues()
//        {
//            TreeList view = _data;
//            var cells = _data.GetSelectedCells();
//            var vals = new List<object>();
          
//            for (int i = 0; i < cells.Count; i++)
//            {
                

//                var val = Cmn.Nvle(view.GetRowCellValue(cells[i].Node, cells[i].Column), null);
//                vals.Add(val);
//            }
//            return vals.ToArray();
//        }

//        public object GetSource()
//        {
//            throw new NotImplementedException();
//        }


//        public int GetColumnsCount()
//        {
//            throw new NotImplementedException();
//        }

//        public int GetRowsCount()
//        {
//            throw new NotImplementedException();
//        }

//        public string GetFieldName(int columnIndex)
//        {
//            throw new NotImplementedException();
         
//        }

//        public object GetValue(int rowIndex, int columnIndex)
//        {
//            throw new NotImplementedException();
         
//        }


//        public void SetValue(int rowIndex, string columnName, object value)
//        {
//            throw new NotImplementedException();
//        }


//        public event CellChangeEventHandler CellValueChanged;





//        public void SetFocusedRow(int rowIndex)
//        {
//            throw new NotImplementedException();
//        }


//        public event RowEventHandler RowAdded;


//        public VTDARowState GetRowState(int rowIndex)
//        {
//            throw new NotImplementedException();
//        }
//    }
//}
