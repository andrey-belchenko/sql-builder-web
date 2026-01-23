//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;

//using sql.builder.DataApi;
//using sql.builder.DataApi.DataObjects;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;


//using sql.builder.UI.WinForms;

//namespace sql.builder.DataApi.TableDataAccessor.Adapters
//{
//    internal class VTDADataTable:IVTableDataAdapter
//    {
//        private DataTable _data;

//        private event CellChangeEventHandler _cellValueChanged;
//        private bool _cellValueChangedAtc=false;
//        public event CellChangeEventHandler CellValueChanged
//        {
//            add
//            {
//                var vdt = _data as VDataTable;
//                if (!_cellValueChangedAtc)
//                {
//                    _cellValueChangedAtc = true;
//                    vdt.MyColumnChanged += vdt_MyColumnChanged;
//                }
//                _cellValueChanged+=value;
//               // vdt.Changed
//            }
//            remove
//            {
//                var vdt = _data as VDataTable;
//                vdt.MyColumnChanged -= vdt_MyColumnChanged;
//                _cellValueChanged -= value;

//            }
//        }

//        public event RowEventHandler _rowAdded;
//        private bool _rowAddedChangedAtc = false;
//        public event RowEventHandler RowAdded
//        {
//            add
//            {
//                var vdt = _data as VDataTable;
//                if (!_rowAddedChangedAtc)
//                {
//                    _rowAddedChangedAtc = true;
//                    vdt.MyRowAdded += vdt_MyRowAdded;
//                }
             
//                _rowAdded += value;
//                // vdt.Changed
//            }
//            remove
//            {
//                var vdt = _data as VDataTable;
//                vdt.MyRowAdded -= vdt_MyRowAdded;
//                _rowAdded -= value;

//            }
//        }

//        void vdt_MyRowAdded(object sender, DataRowChangeEventArgs e)
//        {
//            var vdt = _data as VDataTable;
//            var rowI = _data.Rows.IndexOf(e.Row);
//            _rowAdded(rowI);
//        }

//        void vdt_MyColumnChanged(object sender, DataColumnChangeEventArgs e)
//        {
//            var vdt = _data as VDataTable;
//           var rowI=_data.Rows.IndexOf(e.Row);
//           _cellValueChanged(rowI, e.Column.ColumnName, e.ProposedValue);
//        }
//        public VTDADataTable(DataTable data)
//        {
//            _data = data;
//        }
//        public object[] GetSelectedCellsValues()
//        {
//            throw new NotImplementedException();
//        }


//        public object GetSource()
//        {
//            return _data;
//        }


//        public int GetColumnsCount()
//        {
//            return _data.Columns.Count;
//        }

//        public int GetRowsCount()
//        {
//            return _data.Rows.Count;
//        }

//        public string GetFieldName(int columnIndex)
//        {
//            return _data.Columns[columnIndex].ColumnName;
//        }

//        public object GetValue(int rowIndex, int columnIndex)
//        {
//            return _data.Rows[rowIndex][columnIndex];
//        }


//        public void SetValue(int rowIndex, string columnName,object value)
//        {
//            var vdata = (_data as VDataTable);
//            bool isV = false;
//            if (vdata != null)
//            {
//                if (vdata.Columns[columnName] is VDataColumn)
//                {
//                    vdata.GetColumn(columnName).SetValue(_data.Rows[rowIndex], value);
//                    return;
//                }
//            }
            
//            _data.Rows[rowIndex][columnName] = value;
            
//           //_data.Rows[rowIndex].
//        }





//        public void SetFocusedRow(int rowIndex)
//        {
//            var vdata = (_data as VDataTable);
          
//            if (vdata != null)
//            {
//                DataRow row = null;
//                if (rowIndex > 0)
//                {
//                     row = _data.Rows[rowIndex];
//                }
//                vdata.CurrentRow = row;
//            }

           
//        }





//        public VTDARowState GetRowState(int rowIndex)
//        {
           
//            DataRow row = _data.Rows[rowIndex];
//            switch (row.RowState)
//            {
//                case DataRowState.Added:
//                    return VTDARowState.Added;
//                case DataRowState.Deleted:
//                    return VTDARowState.Deleted;
//                case DataRowState.Modified:
//                    return VTDARowState.Modified;

//            }
//            return VTDARowState.Unchanged;
//        }
//    }
//}
