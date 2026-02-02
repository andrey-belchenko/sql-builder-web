
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Threading;
using Devart.Data.Oracle;
using sql.builder.UI;
namespace sql.builder.DataApi
{
    internal partial class VDataSet
    {
       
        public SortedList<string, VDataColumn > VariableColumns = null;
        public HashSet<string> VariableColumnHasValue = null;
        public VWithParams FactParamsElement = null; // параметры listquery, описанного для для колонки в форме
      //  public DataColumnChangeEventHandler VariableChanged = null;
        public VVariableDepandantceController VariableDepandantceController = null;
        public EventHandler ChangeCompleted = null;
        public VDataTable ChoiceSource = null;
        public bool WasRefresh = false;
        public void OnVariableValueChanged(string variableName)
        {
            if (this.VariableColumnHasValue == null) {
                this.VariableColumnHasValue = new HashSet<string>();
            }
            if (!this.VariableColumnHasValue.Contains(variableName)) {
                this.VariableColumnHasValue.Add(variableName);
            }
            //foreach (VDataTable tbl in this.Tables) { //!!! не оптимизированно + если зависит от нескольких переменных будет обновляться несколько раз
            //    if (tbl.DataAdapter.SelectCommand != null && !string.IsNullOrEmpty(tbl.DataAdapter.SelectCommand.CommandText)) {
            //        if (tbl.GetParamsNames().Contains(variableName)) {
            //            tbl.Refresh();
            //        }
            //    }
            //}
        }
        private HashSet<string> _tablesToResresh = null;
        //чтобы при изменении нескольких переменных одна и та же таблица не обновлялась несколько раз
        public void PrcessRefreshQueue()
        {
            if (_tablesToResresh==null) return;
            var tt = _tablesToResresh;
            _tablesToResresh = null;
            foreach (var tableName in tt)
            {
                var tbl = GetTable(tableName);
                tbl.Refresh();
                tbl.RaiseCurrentRowChanged();
            }
           // _tablesToResresh.Clear();
        }

        public void EnqueueTableRefresh(string tableName)
        {
            if (_tablesToResresh == null)
            {
                _tablesToResresh=new HashSet<string>();
            }
            if (!_tablesToResresh.Contains(tableName))
            {
                _tablesToResresh.Add(tableName);
            }
        }
        
        public bool IsVariableHasValue(string variableName)
        {
            
            if (VariableColumnHasValue == null)
            {
                return false;
            }
            return VariableColumnHasValue.Contains(variableName);
            
        }

        public bool IsValid = true;

        private List<string> InvalidTables = null;

        public void ResetValidation()
        {
            InvalidTables = null;
            IsValid = true;
            SetVariableValue(TextConst.AVParam.FormValid, 1,true);
            SetVariableValue(TextConst.AVParam.FormValidNot, 0,true);
        }

        public void AddInvalidTable(string name)
        {

            if (InvalidTables == null)
            {
                InvalidTables = new List<string>();
            }
            if (!InvalidTables.Contains(name))
            {
                InvalidTables.Add(name);
            }
            if (IsValid)
            {
                IsValid = false;
                SetVariableValue(TextConst.AVParam.FormValid, 0 ,true);
                SetVariableValue(TextConst.AVParam.FormValidNot, 1,true);
            }
        }
        public void RemoveInvalidTable(string name)
        {
            if (InvalidTables == null) {
                return;
            }
            if (InvalidTables.Contains(name)) {
                InvalidTables.Remove(name);
            }
            if (InvalidTables.Count == 0) {
                ResetValidation();
            }
        }
        public bool ChangesNotCompleted = false; // !!! очередная манипуляция чтобы обновить layout в нужное время, но безлишних обновлений
        public void RaiseChangeCompleted()
        {
            ChangesNotCompleted = false;
            ((OracleConnection)GetConnection()).Commit();// !!! Не уверен что здесь это корректно
            if (ChangeCompleted != null)
            {
                ChangeCompleted(this, null);
            }
        }


        public void AddVariableColumn(string variableName,VDataColumn column)
        {
            if (VariableColumns == null)
            {
                VariableColumns = new SortedList<string, VDataColumn>();
            }
            VariableColumns.Add(variableName, column);
            column.VariableName = variableName;
            (column.Table as VDataTable).AttachBehaviorEvent();
        }
        public void SetVariableValue(string variableName,object value, bool isDataChanged)
        {
            if (VariableColumns==null) return;
            var col = VariableColumns[variableName];
            var tbl = col.Table as VDataTable;
            if (tbl.CurrentRow != null && tbl.CurrentRow.RowState != DataRowState.Deleted)
            {
                if (col.SetValue(tbl.CurrentRow, value))
                {
                   // tbl.ProcessBehaviorChanges(col, tbl.CurrentRow, isDataChanged); // вроде не нужно событие и так срабатывает от setvalue дает повтор
                }
                
            }
            

        }

        public object GetVariableValue(string variableName)
        {
           return GetVariableValue( variableName,null);
        }

        public object GetVariableValue(string variableName,DataRow row)
        {
            bool invert;
            if (variableName[0] == '!') {
                invert = true;
                variableName = variableName.Substring(1);
            } else {
                invert = false;
            }
            object val = null;
            if (variableName == TextConst.AVBool.False) { // заплатка для стыковки состарым вариантом
                val = DBNull.Value;
            } else if (variableName == TextConst.AVBool.True) {
                val = variableName;
            } else {
                if (this.VariableColumns == null) {
                    return null;
                }
                VDataColumn col;
                if (!this.VariableColumns.TryGetValue(variableName, out col)) {
                    return null;
                }
                var tbl = col.Table as VDataTable;
                DataRow row1;
                if (row != null && row.Table == tbl) {
                    row1 = row;
                } else {
                    row1 = tbl.CurrentRow;
                }
                if (row1 != null && row1.RowState != DataRowState.Deleted) {
                    val = row1[col];
                } else {
                    val = DBNull.Value;
                }
                if (invert) {
                    if (Cmn.IsNullOrDBNull(val) || val.ToString() == TextConst.AVBool.False) {
                        val = 1;
                    } else {
                        val = 0;
                    }
                }
            }
            return val;
        }
        public VDataColumn GetVariableColumn(string variableName)
        {
            if (VariableColumns != null)
            {
                if (VariableColumns.ContainsKey(variableName))
                {
                    return VariableColumns[variableName];
                }
            }
            return null;
        }
        List<VDataColumn> changingColumns = null;

        public void AddChangingColumn(VDataColumn column)
        {
            if (column == null)
            {
                return;
            }
            if (changingColumns == null)
            {
                changingColumns = new List<VDataColumn>();
            }

            //if (VDataColumn.backgroundWorkers==null || VDataColumn.backgroundWorkers.Count == 0)
            //{
            //    ClearChangingColumns();
            //}
          //  return;
            if (!changingColumns.Contains(column))
            {
                if (column.ColumnName == "dat_do")
                {
                }
                changingColumns.Add(column);
            }
        }

        public void RemoveChangingColumn(VDataColumn column)
        {
            if (column == null)
            {
                return;
            }
            if (changingColumns.Contains(column))
            {
               
                changingColumns.Remove(column);
            }
            
        }

        public bool IsColumnChanging(VDataColumn column)
        {
            if (changingColumns == null) return false;
            return changingColumns.Contains(column);
        }

    

        //public bool IsColumnsChanging()
        //{
        //    if (changingColumns == null) return false;
        //    return changingColumns.Count>0;
        //}

        public void ClearChangingColumns()
        {
            if (changingColumns != null)
            {
                changingColumns.Clear();
            }
        }
        private int dataSetId = 0;
        public int GetFormId()
        {
            if (dataSetId > 0)
            {
                return dataSetId;
            }
            int id = 0;
            if (ParentDataTable != null)
            {
                id = ParentDataTable.GetDataSet().GetFormId();
            }
            else if (ParentDataSet != null)
            {
                id = ParentDataSet.GetFormId();
            }
            else
            {
                id = this.GetHashCode();
            }
            dataSetId = id;

            return id;
        }
        internal OracleParameter CreateFormIdParametr()
        {
            return new OracleParameter(TextConst.DBParams.FormId, OracleDbType.Number, (object)this.GetFormId(), ParameterDirection.Input);
        }
    }
}