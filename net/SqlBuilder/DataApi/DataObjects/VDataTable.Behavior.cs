using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using Devart.Data.Oracle;
using sql.builder.UI;// !!! перенести используемые классы и убрать
using System.Threading;
using System.ComponentModel;
using SqlBuilderLib.DevTools;
namespace sql.builder.DataApi
{
    public partial class VDataTable
    {
        private bool _allowMerge = false;
        public bool Merged = false;

        public Dictionary<string,string> ExtensionKeys = null; 

        public bool AllowMerge
        {
            get
            {
                return _allowMerge;
            }
            set
            {
                _allowMerge = value;
                if (_allowMerge)
                {
                    mergeKeysValues = new SortedList<string, SortedList<string, string>>();
                }
            }
        }

        private SortedList<string,HashSet<string>> _notMergedCashRows = null;
        private SortedList<string, HashSet<string>> _notMergedCashReadyGroups = null;

        private void clearNotMergedCash()
        {
            _notMergedCashRows = null;
            _notMergedCashReadyGroups = null;
        }
        private void addNotMergedCashValue(VDataColumn col, DataRow row)
        {
            if (string.IsNullOrEmpty(col.MergeKey)) return;
            if (_notMergedCashRows == null)
            {
                _notMergedCashRows = new SortedList<string, HashSet<string>>();
                _notMergedCashReadyGroups = new SortedList<string, HashSet<string>>();
            }

            if (!_notMergedCashRows.ContainsKey(col.ColumnName))
            {
                _notMergedCashRows.Add(col.ColumnName, new HashSet<string>());
                _notMergedCashReadyGroups.Add(col.ColumnName, new HashSet<string>());
            }
          
            var mergeKeyValue = GetMergeKeyValue(col, row);
            if (!_notMergedCashReadyGroups[col.ColumnName].Contains(mergeKeyValue))
            {
                _notMergedCashReadyGroups[col.ColumnName].Add(mergeKeyValue);
                var rid = GetRowId(row);

                _notMergedCashRows[col.ColumnName].Add(rid);
            }

        }

        public bool IsNotMerged(VDataColumn col, DataRow row)
        {
                if (string.IsNullOrEmpty(col.MergeKey))
                {
                    return true;
                }
              var rid = GetRowId(row);
              if (_notMergedCashRows.ContainsKey(col.ColumnName))
              {
                  if (_notMergedCashRows[col.ColumnName].Contains(rid))
                  {
                      return true;
                  }
              }
              return false;
        }

        private SortedList<string, SortedList<string, string>> mergeKeysValues = null;

        private HashSet<string> _mergeKeyColumnsNames = null;
        private HashSet<string> GetMergeKeyColumnsNames()
        {
            if (_mergeKeyColumnsNames == null)
            {
                _mergeKeyColumnsNames = new HashSet<string>();
                
                foreach (VDataColumn col in Columns)
                {
                    //if (col.MergeKey != null)
                    //{
                        var ss = col.MergeKey.Split(',');

                        foreach (var s in ss)
                        {
                            if (!_mergeKeyColumnsNames.Contains(s))
                            {
                                _mergeKeyColumnsNames.Add(s);
                            }
                        }
                    //}
                }
            }
            return _mergeKeyColumnsNames;
        }

        public string GetMergeKeyValue(VDataColumn col, DataRow row)
        {
            
            if (!mergeKeysValues.ContainsKey(col.MergeKey))
            {
                mergeKeysValues[col.MergeKey] = new SortedList<string, string>();
            }
            var rid = GetRowId(row);

            if (!mergeKeysValues[col.MergeKey].ContainsKey(rid))
            {
                var ss = col.MergeKey.Split(',').ToList();
                if (col.MergeKey == "")
                {
                    ss.Clear();
                }
                string val = "";
                bool isNull = true;
                foreach (var s in ss)
                {
                    var v = GetColumn(s).GetValue(row);
                    if (Cmn.Nvl(v, null) != null)
                    {
                        isNull = false;
                    }
                    val += "-" + GetColumn(s).GetValue(row);
                }
                if (!isNull)
                {
                    mergeKeysValues[col.MergeKey][rid] = val;
                }
                else
                {
                    mergeKeysValues[col.MergeKey][rid] = null;
                }
            }
            return mergeKeysValues[col.MergeKey][rid];
        }

        private void processMergeArea(VDataColumn col, List<DataRow> mergedRows, object mergedVal, DataRow prevRow, object prevVal, HashSet<string> dontDeleteRows)
        {

            if (mergedRows.Count > 1)
            {
                foreach (var row1 in mergedRows)
                {
                    if (col.SetValue(row1, mergedVal)) // вроде не нужно добавил max over в запросе куба, но пока оставлю
                    {

                    }
                }
            }
            else if (mergedRows.Count == 1)
            {
                if (GetMergeKeyColumnsNames().Contains(col.ColumnName))
                {
                    if (Cmn.Nvl(prevVal, null) != null) 
                    {
                        var rid = GetRowId(prevRow);
                        if (!dontDeleteRows.Contains(rid))
                        {
                            dontDeleteRows.Add(rid);
                        }
                    }
                }
            }
        }

        private void PrepareMerge()
        {
          //  return;
            if (!AllowMerge) return;
            clearNotMergedCash();
            Merged = false;
           // return;

            SuppressChangeEvent();
            var dontDeleteRows = new HashSet<string>();
            
            foreach (VDataColumn col in Columns)
            {
                if (col.MergeKey != null)
                {

                    //if (col.ColumnName == "dolg_tek_peni_ip")
                    //{
                    //}
                    DataRow prevRow = null;
                    var mergedRows = new List<DataRow>();
                    object mergedVal = DBNull.Value;
                    object prevVal = DBNull.Value;
                    foreach (DataRow row in Rows)
                    {
                        var val = col.GetValue(row);
                        bool isMerged = false;
                        
                        if (prevRow != null)
                        {
                            isMerged=col.IsMerged(prevRow, row);
                            if (isMerged)
                            {
                                Merged = true;
                                
                            }
                            else
                            {
                                //if (((decimal)row["dlg_ik_all"]) > 0)
                                //{
                                //}
                                processMergeArea(col, mergedRows, mergedVal, prevRow, prevVal, dontDeleteRows);
                                mergedRows = new List<DataRow>();
                                mergedVal = DBNull.Value;

                            }
                        }
                        mergedRows.Add(row);


                        if (Cmn.Nvl(mergedVal, null) == null)
                        {
                            if (Cmn.Nvl(val, null) != null)
                            {
                                mergedVal = val;
                            }
                        }
                        else if (col.DataType == XmlReports.numberType && Cmn.NumToDecimal(mergedVal) == 0m)
                        {
                            if (Cmn.NumToDecimal(val) != 0m)
                            {
                                mergedVal = val;

                            }
                        }


                        prevRow = row;




                        prevVal = val;

                        
                    }

                    processMergeArea(col, mergedRows, mergedVal, prevRow, prevVal, dontDeleteRows);
                }
                

              

                
            }

            if (Merged)
            {
                var rowsToDelete = new List<DataRow>();
                bool changes = false;
                foreach (DataRow row in this.Rows.ToArray()) {
                    var rid = GetRowId(row);
                    var del = false;
                    if (!dontDeleteRows.Contains(rid))
                    {

                        bool allNulls = true;
                        foreach (var name in GetMergeKeyColumnsNames())
                        {
                            var val1 = Cmn.Nvl(row[name], null);
                            if (val1 != null)
                            {
                                allNulls = false;
                                break;
                            }
                        }
                        if (!allNulls)
                        {
                            del = true;
                        }

                     
                    }
                    if (!del)
                    {
                        foreach (VDataColumn col in Columns) // не оптимально????
                        {
                            addNotMergedCashValue(col, row);
                        }
                    }
                    else
                    {
                        changes = true;
                        rowsToDelete.Add(row);
                    }

                }

                foreach (var row in rowsToDelete)
                {
                    row.Delete();
                }
                if (changes)
                {
                    AcceptChanges();
                }
               
            }
            if (Grid != null)
            {
                Grid.SetAllowMerge();
            }
            ResumeChangeEvent();
        }



        public VDataTable SelectionTarget = null;

        public string MultiselectColumnName = null;
        public string MultiselectTargetName = null;

        private List<DataRow> _selectedRows = null;
        public string ClientBackColorSource = null;
        public string ClientCanBeCheckedSource = null;

        public string GetBackColor(DataRow row,VDataColumn col)
        {
            object val = null;

            if (col != null)
            {
                val = col.GetBackColor(row);
            }

            if (val==null && ClientBackColorSource != null)
            {
                val = GetDataSet().GetVariableValue(ClientBackColorSource,row);
            }
            
            return (string)Cmn.Nvle(val, null);

        }


        public bool GetCanBeChecked(DataRow row)
        {
            object val = null;

            

            if ( ClientCanBeCheckedSource != null)
            {
                val = GetDataSet().GetVariableValue(ClientCanBeCheckedSource, row);
            }

            return Cmn.Nvle(val, null)==null;

        }
        private List<object> _selectedKeys = null;

        public void ActulalizeSelection()
        {
            if (Grid != null)
            {
                Grid.UpdateDataSourceSelectedRows();
            }
        }
        private void rememberSelection()
        {
            if (Grid == null) return;

            if (PrimaryKey.Length != 1) return;
            Grid.UpdateDataSourceSelectedRows();
            _selectedKeys = new List<object>();

            if (_selectedRows == null) return;
            foreach (var r in _selectedRows)
            {
                _selectedKeys.Add(r[PrimaryKey[0]]);
            }
        }
        private void recollectSelection()
        {
      
            var selRows=new List<DataRow>();
            if (_selectedKeys != null)
            {
                foreach (var v in _selectedKeys)
                {
                    var r = Rows.Find(v);
                    if (r != null)
                    {
                        selRows.Add(r);
                    }

                }
                 _selectedRows = selRows;
                RaiseDataSourceSelectionChanged();
            }
            else
            {
                if (Grid != null && Grid.IsCheckBoxSelection())
                {
                    _selectedRows = selRows;
                    RaiseDataSourceSelectionChanged();
                }
            }
            
           
        }
       
        
        public List<DataRow> SelectedRows
        {
            get
            {
                if (_selectedRows == null)
                {
                    return new List<DataRow>();
                }
                return _selectedRows;
            }
            set
            {
                if (!_checkingRows)
                {
                    _selectedRows = value;
                    processSelection();

                    if (this.Grid != null && this.Grid.IsCheckBoxSelection())
                    {
                        RaiseCurrentRowChanged();
                    }
                }
                
            }
        }

        private void SetRowsChecking(bool value)
        {
            _checkingRows = value;
            if (MultiselectTargetName != null)
            {
                GetDataSet().GetTable(MultiselectTargetName)._checkingRows = value;
            }
            else
            {
                GetDataSet().GetTable(MultiselectSource())._checkingRows = value;
            }
        }

        private void processSelection()
        {
            if (!GetDataSet().IsRefreshing())
            {
                if (!string.IsNullOrEmpty(MultiselectSource()))
                {
                    var tbl = GetDataSet().GetTable(MultiselectSource());

                    tbl.SyncSelection(TableName, ArrayEditValueRefColumn, tbl.PrimaryKey[0].ColumnName);
                }
                else if (!string.IsNullOrEmpty(MultiselectTargetName))
                {
                    var tbl = GetDataSet().GetTable(MultiselectTargetName);
                    tbl.SyncTargetSelection();
                    //tbl.SyncSelection(TableName, tbl.ArrayEditValueRefColumn, PrimaryKey[0].ColumnName);
                }
            }
        }
        public bool IsMultiselectionMember()
        {
            if (!string.IsNullOrEmpty(MultiselectSource()) || !string.IsNullOrEmpty(MultiselectTargetName))
            {
               return true;
            }
            else 
            {
                return false;
            }
        }
        public void SyncTargetSelection()
        {
            var src = GetDataSet().GetTable(MultiselectSource());
            SyncSelection(src.TableName, src.PrimaryKey[0].ColumnName, ArrayEditValueRefColumn);
        }
        private  void SyncSelection(string sourceName,string sourceColumnName,string columnName)
        {
            
            if (this._checkingRows) return;
            var src = GetDataSet().GetTable(sourceName);
            if (src._checkingRows) return;
            var allRows = new SortedList<string,DataRow>();
            var rowsToSelect = new List<DataRow>();
            foreach (DataRow r in ExistingRows())
            {
                allRows.Add(r[columnName].ToString(), r);
            }
            foreach (DataRow r in src.SelectedRows)
            {
                if (r.RowState != DataRowState.Deleted && r.RowState != DataRowState.Detached)
                {
                    var val = r[sourceColumnName].ToString();
                    if (allRows.ContainsKey(val))
                    {
                        rowsToSelect.Add(allRows[val]);
                    }
                }
            }
            _selectedRows = rowsToSelect;
            RaiseDataSourceSelectionChanged();
        }
        public event SimpleEventHandler DataSourceSelectionChanged;
        private void RaiseDataSourceSelectionChanged()
        {
            if (DataSourceSelectionChanged != null)
            {
                DataSourceSelectionChanged();
            }
        }

        private void SyncChecksIfNeed()
        {
            VDataTable src = null;
            VDataTable trg = null;
            if (!string.IsNullOrEmpty(MultiselectSource()))
            {
                src = GetDataSet().GetTable(MultiselectSource());
                trg = this;
                //tbl.SyncChecks();
            }
            else if (!string.IsNullOrEmpty(MultiselectTargetName))
            {

                trg = GetDataSet().GetTable(MultiselectTargetName);
                src = this;
                //SyncChecks();
            }

            
            if (trg!=null && trg._refreshed && src._refreshed)
            {
                src.SyncChecks();
            }
        }

        private void SyncChecks()
        {
           
            var tbl= GetDataSet().GetTable(MultiselectTargetName);
            //var rows = new List<DataRow>();
            SuppressChangeEvent();
            SendBeginUpdateToUI();
            foreach (DataRow row in ExistingRows())
            {
                var rst = row.RowState;
                GetColumn(MultiselectColumnName).SetValue(row, 0m);
                if (rst == DataRowState.Unchanged)
                {
                    row.AcceptChanges();
                }
            }
            foreach (DataRow r in tbl.ExistingRows())
            {
              
                var row = this.Rows.Find(r[tbl.ArrayEditValueRefColumn]);
                if (row != null)
                {
                    var rst = row.RowState;
                    //rows.Add(row);
                    GetColumn(MultiselectColumnName).SetValue(row, 1);
                    if (rst == DataRowState.Unchanged)
                    {
                        row.AcceptChanges();
                    }

                }
                
                
            }
            ResumeChangeEvent();
            SendEndUpdateToUI();
           // SetRowsChecked(rows.ToArray());
            
        }


        public bool IsValid = true;
        public bool AutoRefresh = false;
        public bool OnlyForceRefresh = false;
        public bool OnlyVisibleRefresh = false;
        private SortedList<string,SortedList<string,string>> InvalidFields = null;
        private SortedList<string,DataRow> InvalidRows = null;
        private HashSet<string> ValidatedRows = null;
        private HashSet<string> CheckedRows = null;


        public IControlWithTableSource Grid = null;
        private bool isVisibleInLayout()
        {
            var val = Grid.IsVisibleInLayout();
            _isVisibleInLayout = val;
            return val;
        }
        private bool _isVisibleInLayout=false;
        public void SetVisibleInLayout(bool value)
        {
            var oldvis = _isVisibleInLayout;
            _isVisibleInLayout = value;

            //if (this.TableName == "ur_graf_fact_opl" && _isVisibleInLayout)
            //{

            //}
            if (!oldvis && _isVisibleInLayout && _refreshWhenVisible)
            {

                _refreshWhenVisible = false;
                Refresh();
            }
          
        }
        public void UpdateValidation(DataRow row)
        {
            foreach (VDataColumn col in Columns)
            {
                //if (col.ColumnName == "kod_result")
                //{
                //    col.GetValidation(row);
                //}
            }
        }

        public void SetCellError(DataRow row, string columnName, string message)
        {
            //if (!PrimaryKey.Any()) return;
            //var rowid = row[PrimaryKey[0]].ToString();

            //if (rowid == "")
            //{
            //    return;
            //}
            if (ValidatedRows == null)
            {
                ValidatedRows = new HashSet<string>();

            }
            var rid = GetRowId(row);
            if (!ValidatedRows.Contains(rid))
            {
                ValidatedRows.Add(rid);
            }
            if (string.IsNullOrEmpty(message))
            {
                //if (columnName == "data_for_report")
                //{
                //}
                    RemoveInvalidFieldColumn(row, columnName);
                
                //if (!string.IsNullOrEmpty(GetCellError(row, columnName)))
                //{
                   
                //    var id = new Tuple<string, string>(rowid, columnName);
                //    CellErrors.Remove(id);
                //}
                
                
            }
            else
            {
                //if (columnName == "data_for_report")
                //{
                //}
                
             //   var id = new Tuple<string, string>(rowid, columnName);
                AddInvalidFieldColumn(row, columnName, message);
                //if (CellErrors == null)
                //{
                //    CellErrors = new SortedList<Tuple<string, string>, string>();
                //}
                //CellErrors[id] = message;
            }
        }
        public string GetNameForText(string columnName)
        {
            var col = GetColumn(columnName);
            if (col == null)
            {
                return null;
            }
            if (col.DependantsTextSource == null)
            {
                return columnName;
            }
            else
            {
                return col.DependantsTextSource.First().ColumnName;
            }
        }

        public string GetCellErrorForText(DataRow row, string columnName)
        {
            
            return GetCellError(row, GetNameForText( columnName));
        }
        
        public string GetCellError(DataRow row,string columnName)
        {
            if (columnName == null)
            {
                return null;
            }

            if (columnName == "ur_dp_pr_sch_gp")
            {

            }

            var rowid = GetRowId(row);

            if (ValidatedRows == null || !ValidatedRows.Contains(rowid))
            {
                foreach (DataColumn col1 in Columns)
                {
                    var col = col1 as VDataColumn;
                    if (col != null)
                    {
                        //if (col.ColumnName == "ur_dp_pr_sch_gp")
                        //{

                        //}
                        col.GetValidation(row); //!!! спорно! не слишком ли часто будет вызываться? время?
                    }
                }
            }
            if (InvalidRows == null)
            {
                return "";
            }


            

            if (!InvalidRows.ContainsKey (rowid))
            {
                return "";
            }

            
           

            if (!InvalidFields[rowid].ContainsKey(columnName))
            {
                return "";
               
            }
           // var id = new Tuple<string, string>(rowid, columnName);
            return  VDataTable.ClearValidationMessage( InvalidFields[rowid][columnName]);
           // return GetColumn(InvalidFields[rowid][0]).GetValidation(row);

        }

        public VDataSet.ValidationResult GetRowErrorText(DataRow row)
        {
            var result = new VDataSet.ValidationResult();
            if (InvalidRows == null)
            {
                return result;
            }
            var rowid = GetRowId(row); 
            if (!InvalidRows.ContainsKey (rowid))
            {
                return result;
            }

            if (row.RowState == DataRowState.Unchanged && (GetDataSet().ParamsTable!=this || GetDataSet().TopTable!=null))
            {
                return result;
            }

            foreach (var a in InvalidFields[rowid])
            {
                if (!IsValidationMessgeCanSave(a.Value))
                {
                    result.Error = ClearValidationMessage(a.Value);
                    return result;
                }
                // убрал else
                if (IsValidationMessgeNeedAlert(a.Value))
                {
                    result.Warning.Add(ClearValidationMessage(a.Value));
                }
               
            }
          //  var id = new Tuple<string, string>(rowid, InvalidFields[rowid].First().Key);
            return result;
        }

        public static string GetValidationMessagePref(string msg) //!! заплатка, переделать если будет имет развитие
        {
            var pref = "";
            if (msg.Length > 4)
            {
                 pref = msg.Substring(0, 5);
            }
            return pref;
        }
        public static string ClearValidationMessage(string msg)
        {
            var pref = GetValidationMessagePref(msg);
            if (TextConst.MsgTypePrefArray.All.Contains(pref))
            {
                return msg.Substring(5, msg.Length - 5);
            }
            return msg;
        }
        public static bool IsValidationMessgeCanSave(string msg)
        {
            var pref = GetValidationMessagePref(msg);
            if (TextConst.MsgTypePrefArray.CanSave.Contains(pref))
            {
                return true;
            }
            return false;
        }

        public static bool IsValidationMessgeNeedAlert(string msg)
        {
            var pref = GetValidationMessagePref(msg);
            if (TextConst.MsgTypePrefArray.Alert.Contains(pref))
            {
                return true;
            }
            return false;
        }
        public void ResetValidation()
        {
            InvalidFields = null;
            InvalidRows = null;
            IsValid = true;
            ValidatedRows = null;
            GetDataSet().RemoveInvalidTable(TableName);
        }

        public string GetRowId(DataRow row)
        {

          //// при удалении индекс изменяется, наверное нужно переделать;
          // var id= Rows.IndexOf(row).ToString();
          // if (id == "-1")
          // {
          //     throw new IndexOutOfRangeException();
          // }

            var id = row.GetHashCode().ToString();// может так сойдет
           return id;
        }
        public void AddInvalidFieldColumn(DataRow row, string columnName,string message)
        {
           var  rowid = GetRowId(row);
           
            //if (rowid == "")
            //{
            //    return;
            //}
            
            if (InvalidFields == null)
            {
                InvalidFields = new  SortedList<string, SortedList<string,string>>();
                InvalidRows = new SortedList<string, DataRow>();
            }
          

            //if (PrimaryKey.Any())
            //{
               
            //}
            if (!InvalidFields.ContainsKey(rowid))
            {
                InvalidFields.Add(rowid,new SortedList<string,string>());
                InvalidRows.Add(rowid,row);
            }
            InvalidFields[rowid][columnName]=message;
            if (IsValid)
            {
                IsValid = false;
                GetDataSet().AddInvalidTable(TableName);
            }
        }
        public void RemoveInvalidRow(DataRow row)
        {
            var rowid = GetRowId(row);
            if (!InvalidFields[rowid].Any())
            {
                InvalidFields.Remove(rowid);
                InvalidRows.Remove(rowid);
            }
            if (!InvalidFields.Any())
            {
                ResetValidation();
            }
        }

        public void RemoveInvalidFieldColumn(DataRow row,string name)
        {

            if (InvalidFields == null)
            {
                return;
            }


            string rowid = GetRowId(row);
            if (InvalidFields.ContainsKey(rowid))
            {
                if (InvalidFields[rowid].ContainsKey(name))
                {
                    InvalidFields[rowid].Remove(name);
                }
                if (!InvalidFields[rowid].Any())
                {
                    InvalidFields.Remove(rowid);
                    InvalidRows.Remove(rowid);
                }
            }
            
            if (!InvalidFields.Any())
            {
                ResetValidation();
            }
            
        }

        public void AcceptSelection()
        {
            VUseAction.TableUpdate_Add(SelectionTarget, SelectedRows);
        }
        public bool NewRowsVisForOtherTbls = false;
        private bool behaviorEventAttached = false;
        public void AttachBehaviorEvent()
        {
            if (!behaviorEventAttached)
            {
                this.MyColumnChanged += onColumnChangedForBehavior;
                behaviorEventAttached = true;
            }
        }
      

        //public string NameFieldName = null;
        //public string KeyFieldName = null;

        public VDBSelectCommand SingleRowRefreshCommand = null;
        //public event DataColumnChangeEventHandler ColumnEditableChanged;
        //public event DataColumnChangeEventHandler ColumnValidChanged;
        public event DataColumnChangeEventHandler ColumnVisibleChanged; // оставлено дл грида
        //public event DataColumnChangeEventHandler ColumnSelListChanged;
        //public event DataColumnChangeEventHandler ColumnTextChanged;
        //public event DataColumnChangeEventHandler ColumnFontColorChanged;
        
        public event DataColumnChangeEventHandler UserChangedData;
        private string _multiselectSource = null;
        public string MultiselectSource()
        {
            if (_multiselectSource == null)
            {
                var tbl = GetDataSet().Tables.Cast<VDataTable>().FirstOrDefault(t => t.MultiselectTargetName == this.TableName);
                if (tbl == null)
                {
                    _multiselectSource = "";
                }
                else
                {
                    _multiselectSource = tbl.TableName;
                }
            }
            return _multiselectSource;

        }
        private IList<DataRow> ExistingRows()
        {
            List<DataRow> rows = new List<DataRow>();
            for (int index = 0; index < this.Rows.Count; index++) {
                DataRow row = this.Rows[index];
                if (row.RowState != DataRowState.Deleted && row.RowState != DataRowState.Detached) {
                    rows.Add(row);
                }
            }
            return rows;
        }
        private bool _checkingRows = false;

        public void SendFocusedCellToUI(string columnName,DataRow row)
        {
            if (Grid != null)
            {
                Grid.SetFocusedCell(columnName, row);

            }
        }
        private void SendBeginUpdateToUI()// вроде ничего не дает, но оставлю
        {
            if (Grid != null)
            {
                Grid.BeginUpdate();
               
            }
        }
        private void SendEndUpdateToUI() // вроде ничего не дает, но оставлю
        {
            if (Grid != null)
            {
                Grid.EndUpdate();
            }
        }
        public void SetRowsChecked(DataRow[] rows, bool useAll=false)
        {
            //_checkingRows = true;
            SetRowsChecking(true);
            var tbl = GetDataSet().GetTable(MultiselectTargetName);
            //SendBeginUpdateToUI();
            //tbl.SendBeginUpdateToUI();
            var rowsVals = new List<SortedList<string, object>>();
            SuppressChangeEvent();
            foreach (var row in rows)
            {
                bool use = false;
                if (useAll)
                {
                    use = true;
                }
                if (row[MultiselectColumnName].ToString() != "1")
                {
                    var rst = row.RowState;
                    GetColumn(MultiselectColumnName).SetValue(row, 1);
                    if (rst == DataRowState.Unchanged)
                    {
                        row.AcceptChanges();
                    }
                
                  
                    use = true;
                }
                if (use)
                {
                    var rowVal = new SortedList<string, object>();
                    rowVal.Add(tbl.ArrayEditValueRefColumn, row[PrimaryKey[0]]);
                    rowsVals.Add(rowVal);
                }
            }
            ResumeChangeEvent();
            tbl.AddNewRowsWithValues(rowsVals);
            tbl.RaiseUserChangedData(null, null);

            SetRowsChecking(false);
            Grid.EndUpdate();

            processSelection();
            //tbl.SendBeginUpdateToUI();
            //tbl.SendBeginUpdateToUI();
        }

        public void SetRowsUnCheckedOnTarget(DataRow[] rows, bool useAll = false)
        {
            var src = GetDataSet().GetTable(MultiselectSource());
         

            var srcRows = new List<DataRow>();
            foreach (var row in rows)
            {
                var srcRow = src.Rows.Find(row[ArrayEditValueRefColumn]);
                if (srcRow != null)
                {
                    srcRows.Add(srcRow);
                }
                else
                {
                    row.Delete();
                    RaiseUserChangedData(null, null);
                }
            }

            src.SetRowsUnChecked(srcRows.ToArray(), useAll);
           
              
               
        }

        public void SetRowsUnChecked(DataRow[] rows, bool useAll = false)
        {
            //_checkingRows = true;
            SetRowsChecking(true);
            var tbl = GetDataSet().GetTable(MultiselectTargetName);
            //SendBeginUpdateToUI();
            //tbl.SendBeginUpdateToUI();
            var selRws = _selectedRows.ToList();
            foreach (var row in rows)
            {
                bool use = false;
                if (useAll)
                {
                    use = true;
                }
                if (row[MultiselectColumnName].ToString() == "1")
                {
                    GetColumn(MultiselectColumnName).SetValue(row, 0m);
                    use = true;
                }
                if (use)
                {
                    var refrow = tbl.ExistingRows().Where(r => r[tbl.ArrayEditValueRefColumn].ToString() == row[PrimaryKey[0]].ToString()).FirstOrDefault();
                    if (refrow != null)
                    {
                        refrow.Delete();
                        tbl.RaiseUserChangedData(null, null);
                    }
                }
            }
            //SendBeginUpdateToUI();
            //tbl.SendBeginUpdateToUI();
            //_checkingRows = false;
            SetRowsChecking(false);
            _selectedRows = selRws;//т.к. слетает выделение по непонятным причинам
            RaiseDataSourceSelectionChanged();
            //tbl.SyncTargetSelection();
        }


        private void AddCheckedRow(DataRow row)
        {
            if (CheckedRows == null)
            {
                CheckedRows = new HashSet<string>();
            }
            var id = GetRowId(row);
            if (!CheckedRows.Contains(id))
            {
                CheckedRows.Add(id);
            }
        }

        private void RemoveCheckedRow(DataRow row)
        {
            if (CheckedRows == null)
            {
                return;
            }
            var id = GetRowId(row);
            if (CheckedRows.Contains(id))
            {
                CheckedRows.Remove(id);
            }
        }
        private bool IsRowChecked(DataRow row)// приходится храннить дополнительно из за того что при изменении значения в check ячейке событие срабатывает дважды
        {
            if (CheckedRows == null)
            {
                return false;
            }
            var id = GetRowId(row);
            return CheckedRows.Contains(id);
        }
        public void ProcessCheckedRow(DataRow row)
        {
            if (_checkingRows) return;
            var tbl = GetDataSet().GetTable(MultiselectTargetName);
         
            
            if (row[MultiselectColumnName].ToString() == "1")
            {
                if (!IsRowChecked(row))
                {
                    AddCheckedRow(row);
                    SetRowsChecked(new DataRow[] { row }, true);
                }
                //if (refrow == null)
                //{
                //    var rowsVals = new List<SortedList<string, object>>();

                //    var rowVal = new SortedList<string, object>();
                //    rowVal.Add(tbl.ArrayEditValueRefColumn, row[PrimaryKey[0]]);
                //    rowsVals.Add(rowVal);
                //    tbl.AddNewRowsWithValues(rowsVals);
                //    tbl.RaiseUserChangedData(null, null);
                //}
            }
            else
            {
                RemoveCheckedRow(row);
                SetRowsUnChecked(new DataRow[] { row }, true);
                
            }
        }

        public void RaiseUserChangedData(object sender, DataColumnChangeEventArgs args)
        {
          
            if (args != null)
            {
                var vcol = args.Column as VDataColumn;
                var id = GetRowId(args.Row);
                if (MultiselectColumnName == vcol.ColumnName )
                {
                    if (!IsRowChanged(id))
                    {
                        args.Row.AcceptChanges();
                    }
                    ProcessCheckedRow(args.Row);
                }
                else
                {
                    if (vcol.IsUpdateable)
                    {
                        AddChangedRow(id);
                    }
                }

                if (!vcol.IsUpdateable)
                {
                    if (!IsRowChanged(id) && GetDataSet()!=null/*не редактор схемы*/)
                    {
                        args.Row.AcceptChanges();
                    }
                    if (vcol.ColumnEditableSource != TextConst.AVBool.True)
                    {
                        return;
                    }
                }
                //if (!vcol.IsUpdateable 
                //    && vcol.ColumnEditableSource!=TextConst.AVBool.True // не универсально
                //    ) return;

               
            }



            this._has_user_changes1 = true;
            GetDataSet().SetVariableValue(TableName + TextConst.AVParam.HasChanges, 1, false);

            if (UserChangedData != null)
            {
                UserChangedData(this, args);
            }


        }


        public void ManualUserChangedData()
        {
            RaiseUserChangedData(this, null);
        }

        private void onColumnChangedForBehavior(object sender, DataColumnChangeEventArgs args)
        {

           // if (SuppressChangedEvent) return;
            var column = args.Column as VDataColumn;
            if (column == null) return;
            ProcessBehaviorChanges(column, args.Row,true);
            GetDataSet().PrcessRefreshQueue();
        }

        public void ProcessBehaviorChanges(VDataColumn column, DataRow row,bool isDataChanged) // Перенести все сюда из onColumnChangedForBehavior
        {

            if (IsNonDb)
            {
                if (CustomCellValueChanged != null)
                {
                    CustomCellValueChanged(row, column);
                }
            }


            if (/*ColumnVisibleChanged != null && */column.DependantsExists != null)
            {
                foreach (VDataColumn col in column.DependantsExists)
                {
                    var depColumn = col;//(column.Table.Columns[name] as VDataColumn);
                    var row1 = col.GetInOrCurrentRow(row);
                    
                    foreach (var ctrl in depColumn.BoundControls)
                    {
                        ctrl.ColumnVisibleChanged(row1);
                        ctrl.ColumnValidChanged(row1);
                    }
                    if (ColumnVisibleChanged != null)
                    {

                        var args1 = new DataColumnChangeEventArgs(row1, depColumn, null);
                        depColumn.GetTable().ColumnVisibleChanged(this, args1);
                    }

                    
                       depColumn.ApplyDefaultValue(row1);
                    


                    //
                    //depColumn.GetTable().ColumnValidChanged(this, args1);

                }
            }

            if (/*ColumnSelListChanged != null &&*/ column.DependantsSelList != null)
            {
                foreach (VDataColumn depColumn in column.DependantsSelList)
                {
                    //var depColumn = (column.Table.Columns[name] as VDataColumn);
                    var row1 = depColumn.GetInOrCurrentRow(row);
                    foreach (var ctrl in depColumn.BoundControls)
                    {
                        ctrl.ColumnSelListChanged(row1);
                    }
                    //args.Row.SetColumnError(name,depColumn.GetVisibleation(args.Row));
                    //var args1 = new DataColumnChangeEventArgs(args.Row, depColumn, null);
                    //depColumn.GetTable().ColumnSelListChanged(this, args1);
                }
            }


            if (/*ColumnTextChanged != null &&*/ column.DependantsTextSource != null)
            {
                foreach (VDataColumn depColumn in column.DependantsTextSource)
                {
                    //var depColumn = (column.Table.Columns[name] as VDataColumn);
                    var row1 = depColumn.GetInOrCurrentRow(row);
                    foreach (var ctrl in depColumn.BoundControls)
                    {
                        ctrl.ColumnTextChanged(row1);
                    }
                    //args.Row.SetColumnError(name,depColumn.GetVisibleation(args.Row));
                    //var args1 = new DataColumnChangeEventArgs(args.Row, depColumn, null);
                    //depColumn.GetTable().ColumnTextChanged(this, args1);
                }
            }

            if (/*ColumnFontColorChanged != null && */ column.DependantsFontColor != null)
            {
                foreach (VDataColumn col in column.DependantsFontColor)
                {
                    var depColumn = col;
                    var row1 = depColumn.GetInOrCurrentRow(row);
                    foreach (var ctrl in depColumn.BoundControls)
                    {
                        ctrl.ColumnFontColorChanged(row1);
                    }
                    //args.Row.SetColumnError(name,depColumn.GetVisibleation(args.Row));
                    //var args1 = new DataColumnChangeEventArgs(args.Row, depColumn, null);
                    //depColumn.GetTable().ColumnFontColorChanged(this, args1);
                }
            }


            if (/*ColumnValidChanged != null && */column.DependantsValid != null)
            {
                foreach (VDataColumn depColumn in column.DependantsValid)
                {
                    // var depColumn = (column.Table.Columns[name] as VDataColumn);
                    var row1 = depColumn.GetInOrCurrentRow(row);
                    //args.Row.SetColumnError(name,depColumn.GetValidation(args.Row));
                    foreach (var ctrl in depColumn.BoundControls)
                    {

                        ctrl.ColumnValidChanged(row1);
                    }
                    //var args1 = new DataColumnChangeEventArgs(args.Row, depColumn, null);
                    //depColumn.GetTable().ColumnValidChanged(this, args1);
                }
            }

            if (/*ColumnValidChanged != null && */column.DependantsMandatory != null)
            {
                
                foreach (VDataColumn depColumn in column.DependantsMandatory)
                {

                    var row1 = depColumn.GetInOrCurrentRow(row);
                    //var depColumn = (column.Table.Columns[name] as VDataColumn);
                    foreach (var ctrl in depColumn.BoundControls)
                    {

                        ctrl.ColumnValidChanged(row1);
                    }
                    //args.Row.SetColumnError(name,depColumn.GetValidation(args.Row));
                    //var args1 = new DataColumnChangeEventArgs(args.Row, depColumn, null);
                    //depColumn.GetTable().ColumnValidChanged(this, args1);
                }
            }



            if (/*ColumnEditableChanged != null && */column.DependantsEditable != null)
            {
                foreach (VDataColumn depColumn in column.DependantsEditable)
                {

                    //var depColumn = (column.Table.Columns[name] as VDataColumn);
                    var row1 = depColumn.GetInOrCurrentRow(row);
                    foreach (var ctrl in depColumn.BoundControls)
                    {
                        ctrl.ColumnEditableChanged(row1);
                    }
                    //var args1 = new DataColumnChangeEventArgs(row, depColumn, null);
                    //depColumn.GetTable().ColumnEditableChanged(this, args1);
                }
            }

            if (/*ColumnVisibleChanged != null && */column.DependantsVisible != null)
            {
                foreach (VDataColumn col in column.DependantsVisible)
                {
                    var depColumn = col;//(column.Table.Columns[name] as VDataColumn);
                    if (depColumn.ColumnName == "peni")
                    {
                    }
                    var row1 = depColumn.GetInOrCurrentRow(row);
                    foreach (var ctrl in depColumn.BoundControls)
                    {
                        ctrl.ColumnVisibleChanged(row1);
                    }
                    if (ColumnVisibleChanged != null)
                    {

                        var args1 = new DataColumnChangeEventArgs(row1, depColumn, null);
                        depColumn.GetTable().ColumnVisibleChanged(this, args1);
                    }
                    //args.Row.SetColumnError(name,depColumn.GetVisibleation(args.Row));
                   // var args1 = new DataColumnChangeEventArgs(row, depColumn, null);
                   //depColumn.GetTable().ColumnVisibleChanged(this, args1);
                }
            }

            var vdc = column.GetTable().GetDataSet().VariableDepandantceController;
            if (vdc != null)
            {
                if (vdc.Form != null)
                {
                    if (column.VariableName != null)
                    {
                        column.GetTable().GetDataSet().OnVariableValueChanged(column.VariableName);
                       
                        vdc.DataSourceVariableChanged(column.VariableName);
                        
                        //if ((column.Table.DataSet as VDataSet).VariableChanged != null)
                        //{
                        //    // !!! при смене current row тоже нужно вызывать
                        //    var args1 = new DataColumnChangeEventArgs(row, column, null);

                        //    (column.Table.DataSet as VDataSet).VariableChanged(this, args1);


                        //}
                    }
                }
                //else if (vdc.Grid!=null)
                //{
                //    vdc.DataSourceVariableChanged(column.ColumnName); // всякая химия чтобы использовать функционал разработанный для формы в отчетом гриде
                //    // здесь любую колонку считаем переменной
                //}
            }
            
            //if (column.ColumnName == "kr_dogovor_kod_dog")
            //{
               
            //}
            if (column.DependantsNewVal != null)
            {
                foreach (VDataColumn col in column.DependantsNewVal)
                {
                    if (isDataChanged || col.Table==GetDataSet().ParamsTable)
                    {
                      
                        var depColumn = col;
                        var row1 = depColumn.GetInOrCurrentRow(row);
                        depColumn.ApplyNewValue(row1);
                       
                    }
                   
                }
            }
           
        }

        public void DeleteRows(DataRow[] rows)
        {
            foreach (var row in rows)
            {
                row.Delete();
                RaiseRowStateChanged(row);
            }
        }

        public DataRow[] AddNewRowsWithValues(List<SortedList<string, object>> source)
        {
            SuppressChangeEvent();
            //var ctu = CancelTempUpdate;
            //CancelTempUpdate = true;

            var newRows = new List<DataRow>();
            foreach (var row in source)
            {
                var newRow = Rows.Add();
                ProcessNewRow(newRow,false);
                UpdateRowValues(newRow, row,false);
                AddRowToUpdateTemp(GetRowId(newRow));
              //  UpdateTempRow(newRow);
                newRows.Add(newRow);
            }
           
            //foreach (var row in source)
            //{
            //    var newRow = Rows.Add();
            //    ProcessNewRow(newRow);
            //    UpdateRowValues(newRow, row);
            //    newRows.Add(newRow);
            //}

            ResumeChangeEvent();

            //CancelTempUpdate = ctu;
            var newRowsA=newRows.ToArray();
            foreach (var r in newRowsA)
            {
                RaiseRowStateChanged(r);
              
            }


            //if (!IsNonDb)
            //{
                EnqueueBackgroundRefresh(newRowsA/*,null,null,d*/);
            //}
          
           
            return newRowsA;
        }

        public DataRow[] AddNewRowsWithValues(DataRow[] source)
        {

            var list = new List<SortedList<string, object>>();

            foreach (var r in source)
            {
                list.Add(RowToArray(r));
            }
            var newRowsA = AddNewRowsWithValues(list);
            return newRowsA;
        }

        //public void ProcessNewRows(List<DataRow> rows)
        //{
        //    foreach (var row in rows)
        //    {
        //        ProcessNewRow(row, false);
        //    }
        //    EnqueueBackgroundRefresh(rows.ToArray());
        //}
        public void ProcessNewRow(DataRow row, bool doRefreshCalulatedValues=true)
        {
            //if (SuppressChangedEvent) return;
            if (!this.HasPrimaryKey()) {
                return;
            }
            if (row[row.Table.PrimaryKey[0]] == DBNull.Value) {
                var ctu = CancelTempUpdate;
                CancelTempUpdate = true;
                var drd = DontRefreshDependats;
                DontRefreshDependats = true;
                SuppressChangeEvent();
                row[row.Table.PrimaryKey[0]] = (row.Table as VDataTable).KeyCounter;
                row[TextConst.AVColumn.IsNew] = 1;
                row[TextConst.AVColumn.IsNotNew] = 0;
                ResumeChangeEvent();
                if (MyRowAdded != null) {
                    var a = new DataRowChangeEventArgs(row, DataRowAction.Add);
                    MyRowAdded(this, a);
                }
                KeyCounter--;
                SetForeignKey(row);

                ApplyDefaultValues(row);
                CancelTempUpdate = ctu;
                DontRefreshDependats = drd;
              //  UpdateTempRow(row);

                if (!this.IsArrayEditValue)
                {
                    if (doRefreshCalulatedValues)
                    {
                        UpdateTempRowIfNew(row);
                        RefreshCalulatedValues(row);
                    }
                }
                else
                {
                    UpdateTempRow(row);
                }
              
               // UpdateValidation(row);


            }
        }

        public string ColumnEditableSource = null;
        public string DeleteValidationSource = null;

        public void ApplyDefaultValues(DataRow row)
        {
            foreach (VDataColumn col in this.Columns)
            {
                col.ApplyDefaultValue(row);
            }

        }

        public string GetDeleteValidation(DataRow row)
        {
            if (DeleteValidationSource != null && Cmn.Nvl(row[DeleteValidationSource], null) != null)
            {
                return row[DeleteValidationSource].ToString();
            }

            return null;
        }

        
        public VDataTable GetParentTable()
        {
            if (this.ParentRelations != null && ParentRelations.Count!=0)
             {

                 return (ParentRelations[0].ParentColumns[0].Table as VDataTable);
             }
            return null;
        }

        public void RefreshRowWithParents(DataRow[] rows,bool allowAsyncRefresh=true)
        {
            EnqueueBackgroundRefresh(rows,allowAsyncRefresh:allowAsyncRefresh);
            RefreshParents();
         
        }

        public void RefreshParents()
        {
            var parentTable = GetParentTable();
            
            if (parentTable != null)
            {

                parentTable.RefreshRowWithParents(new DataRow[] { parentTable.currentRow });
            }
            else
            {
                if (GetDataSet().ParentDataTable != null)
                {
                    GetDataSet().ParentDataTable.RefreshRowWithParents(new DataRow[] { GetDataSet().ParentDataTable.currentRow });
                }
            }
        }

        public event UIEventHandler UIEvent;
        public bool RaiseUIEvent(string name, DataRow row, VDataColumn col)
        {
            if (UIEvent != null)
            {
                return UIEvent(this, new UIEventArgs(name, null, this, row, col));
                
            }
            return false;
            
        }

        public void RefreshCalulatedValues(DataRow row)
        {
           
            if (row == null) return;

            EnqueueBackgroundRefreshRow(row);// !!! тест

            //foreach (VDataColumn col in Columns)
            //{
            //    col.RefreshCalulatedValue(row,null,true);
            //}

        }

        private HashSet<string> addedRows = null;

        private void addAddedRow(DataRow row)
        {
            var id = GetRowId(row);
            if (addedRows == null)
            {
                addedRows = new HashSet<string>();
            }
            addedRows.Add(id);
        }


        private void removeAddedRow(DataRow row)
        {
            if (addedRows == null)
            {
                return;
            }
            var id = GetRowId(row);
            if (addedRows.Contains(id))
            {
                addedRows.Remove(id);
            }
        }

        public bool   IsRowAdded(DataRow row)
        {
            if (addedRows == null)
            {
                return false;
            }
            var id = GetRowId(row);
            return addedRows.Contains(id);
        }

        public DataRow[] AddExistingRow(object[] keys)
        {
            SuppressChangeEvent();
           // this.BeginLoadData();
            List<DataRow> rows = new List<DataRow>();
            foreach (object key in keys) {
                var row = this.Rows.Add();
                row[PrimaryKey[0]] = key;
                SetForeignKey(row);
                if (this.GetColumn(TextConst.AVColumn.IsNew) != null) {
                    row[TextConst.AVColumn.IsNew] = Cmn.DECIMAL_ZERO;
                }
                if (this.GetColumn(TextConst.AVColumn.IsNotNew) != null) {
                    row[TextConst.AVColumn.IsNotNew] = Cmn.DECIMAL_ONE;
                }
                row.AcceptChanges();
                rows.Add(row);
                addAddedRow(row);
            }

            ResumeChangeEvent();
         //   System.Windows.Forms.MessageBox.Show("1");
            RefreshRowWithParents(rows.ToArray());
           
            return rows.ToArray();
           // RaiseCurrentRowChanged();
        }

        //private static DataTable ExecuteCmd(VDBSelectCommand cmd, OracleConnection connection, List<OracleParameter> pars)
        //{
        //    return cmd.ExecuteDataTable(pars.ToArray(), connection);
        //}
        private static Semaphore semaphore1 = new Semaphore(1,1);

        private static Queue<RefreshInfo> refreshQueue = new Queue<RefreshInfo>();
        private static bool completingWork  = false;
        private static RefreshInfo _refreshInWork;
        private static RefreshInfo refreshInWork
        {
            get
            {
                return _refreshInWork;
            }
            set
            {
                //if (value == null)
                //{
                //}
                _refreshInWork = value;
            }
        }

        public delegate void SimpleDelegate();
        private class RefreshInfo
        {
            public VDataTable Table;
            public DataRow[] Rows;
            public VDBSelectCommand Command;
            public VDataColumn ChangingColumn;
            public Queue<RefreshInfo> Childs = new Queue<RefreshInfo>();
            public RefreshInfo Parent=null;
            public SimpleDelegate OnComplete;
            public bool CanDoAsync = true;
        }

       
        public void EnqueueBackgroundRefreshRow(DataRow row, VDBSelectCommand cmd = null, VDataColumn changingColumn = null, SimpleDelegate onComplete=null)
        {
            var rows = new DataRow[] { row };

            EnqueueBackgroundRefresh(rows, cmd, changingColumn,onComplete);
            
        }



        private bool _allowAsyncRefresh = true;

        public bool AllowAsyncRefresh
        {
            get
            {
                //return false;
                return _allowAsyncRefresh && !IsNonDb;
            }
            set
            {
                _allowAsyncRefresh = value;
            }
        }
        public bool IsArrayEditValue = false;
        public string ArrayEditValueRefColumn = null;
        public void EnqueueBackgroundRefresh(DataRow[] rows, VDBSelectCommand cmd = null, VDataColumn changingColumn = null, SimpleDelegate onComplete = null, bool allowAsyncRefresh=true)
        {
            //if (this.IsArrayEditValue)
            //{
            //    return;
            //}
            var async = allowAsyncRefresh;
            if (async)
            {
                async = AllowAsyncRefresh;
            }
            var info = new RefreshInfo
            {
                Table=this,
                Rows=rows,
                Command=cmd,
                ChangingColumn=changingColumn,
                OnComplete=onComplete,
                CanDoAsync = async
            };
            if (completingWork)
            {
                info.Parent = refreshInWork;
                refreshInWork.Childs.Enqueue(info);
            }
            else
            {
                if (refreshQueue.Any() && !async)
                {
                    BackgroundRefresh(info,false);
                }
                else
                {

                    refreshQueue.Enqueue(info);
                    if (refreshQueue.Count == 1)
                    {
                        BackgroundRefresh(refreshQueue.Peek(),true);
                    }
                }
            }
            
        }
       
        private void BackgroundRefresh(RefreshInfo info, bool withQueue)
        {
             refreshInWork = info;
             info.Table.GetDataSet().AddChangingColumn(info.ChangingColumn);
             if (info.Rows != null)
             {
                 foreach (DataRow row in info.Rows)
                 {
                     if (row.RowState != DataRowState.Unchanged)
                     {
                         info.Table.UpdateTempRow(row);
                     }
                 }
             }
             info.Table.RefreshRows(info.Rows, info.Command, info.ChangingColumn, info.CanDoAsync, withQueue);
            
        }

        public bool IsBackgroundRefreshProcessing()
        {
            if (refreshQueue == null)
            {
                return false;
            }
            return refreshQueue.Any();
        }
        public void BackgroundRefreshNext()
        {
            
            var queue = refreshInWork.Childs;
            var ds = refreshInWork.Table.GetDataSet();
            RefreshInfo completed = null;
            while (!queue.Any() && refreshQueue.Any())
            {
                var parent = refreshInWork.Parent;
                if (parent != null)
                {
                    queue = parent.Childs;
                }
                else
                {
                    queue = refreshQueue;
                }
                completed = queue.Dequeue();
                completed.Table.GetDataSet().RemoveChangingColumn(completed.ChangingColumn);
                if (completed.Rows != null)
                {
                    foreach (DataRow r in completed.Rows)
                    {
                        removeAddedRow(r);
                    }
                }
                refreshInWork = parent;
                 
               
            }
            if (queue.Any())
            {
                BackgroundRefresh(queue.Peek(),true);
            }
            else
            {
                ds.ClearChangingColumns();
                ds.RaiseChangeCompleted();
               // this.EndLoadData();
            }
            if (completed != null)
            {
                if (completed.OnComplete != null)
                {
                    completed.OnComplete();
                    completed.OnComplete = null;
                }
            }
            

        }
       
        public  void RefreshRows(DataRow[] rows ,VDBSelectCommand cmd=null,VDataColumn changingColumn=null,bool canDoAsync=true,bool withQueue=true) //!!! Реализовать передачу массивов без изменения текста запроса.
        {
            //if (SuppressChangedEvent)/// !!! может быть нужно
            //{
            //    BackgroundRefreshNext();
            //    return;
            //}
            if (rows == null) {
                rows = this.Rows.ToArray();
            }
            if (IsNonDb) {
                if (CustomRowRefresh != null)
                {
                    foreach (var row in rows)
                    {
                        CustomRowRefresh(row);
                    }
                }
                return;
            }
            if (rows.Length == 0) {
                BackgroundRefreshNext();
                return;
            } else if (rows.Length == 1) {
                RefreshRow(rows[0], cmd, changingColumn, canDoAsync, withQueue);
                return;
            }
            if (cmd == null)
            {
                DontRefreshDependats = true;
                cmd = SingleRowRefreshCommand;
            }
            
            
            List<OracleParameter> pars;
            var tbl = this;
            var ds = tbl.GetDataSet();

            if (ds.InputParams != null)
            {
                pars = ds.InputParams.Values.Select(p => p).ToList();
            }
            else
            {
                pars = new List<OracleParameter>();



            }


            // !!! есть лишние манипуляции, разобраться 
            pars.Add(VDBSelectCommand.CreateKeysDBParameter(tbl, rows));

            pars.AddRange(VDBSelectCommand.CreateForegnKeyDBParameter(tbl, rows[0]));

            // pars.Add(VDBSelectCommand.TempRowIdParametr(tbl, row));

            // pars.Add(VDBSelectCommand.CreateNewRowDBParameter(row));
            pars.Add(ds.CreateFormIdParametr());
            string[] parNames = Cmn.GetParameterNames(pars);
            var needParNames = cmd.GetParamsNames();
            foreach (string needParName in needParNames)
            {
                if (!parNames.Contains(needParName))
                {
                    pars.Add(ds.GetParamAsOracleParametr(needParName));
                }
            }

            //int procId = VDataSet.GetBackgroundProcessId();
            if (UIStatic.IsAsync && canDoAsync)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += delegate(object o, DoWorkEventArgs args)
                {
                    WaitSemaphore();
                 
                    args.Result = RefreshRows_DoWork(cmd, pars, rows);

                };


                bw.RunWorkerCompleted += delegate(object o, RunWorkerCompletedEventArgs args)
                {
                    RefreshRows_Complete(args.Result, changingColumn);
                    ReleaseSemaphore();
                };
                bw.RunWorkerAsync();
            }
            else
            {
                var res = RefreshRows_DoWork(cmd, pars, rows);
                RefreshRows_Complete(res, changingColumn,withQueue);
            }


        }

        private object RefreshRows_DoWork(VDBSelectCommand cmd, List<OracleParameter> pars, DataRow[] rows)
        {
            return new  Tuple<DataRow[], DataTable>(
                    rows,
                    cmd.ExecuteDataTable(pars.ToArray(), (OracleConnection)GetDataSet().GetConnection())
                    );
        }

        private void RefreshRows_Complete(object result, VDataColumn changingColumn = null,bool withQueue=true)
        {
            completingWork = true;
            var tbl = this;
            var res = (Tuple<DataRow[], DataTable>)result;
            var rows2 = res.Item1;
            var valTbl = (DataTable)res.Item2;
          
            tbl.SuppressChangeEvent();
           
            tbl.GetDataSet().ChangesNotCompleted = true;
            foreach (DataRow row1 in valTbl.Rows)
            {
               
                var row2 = Rows.Find(row1[PrimaryKey[0].ColumnName]);

                tbl.UpdateRowValues(row2, row1);
              


            }
            tbl.ResumeChangeEvent();
            //VDataSet.RemoveWorkingProcess(procId);
            //if (!VDataSet.HasWorkingProcesses())
            //{
            //    tbl.GetDataSet().ClearChangingColumns();
                
            //}
            //else
            //{
            //}
            DontRefreshDependats = false;
            if (withQueue)
            {
                BackgroundRefreshNext();
            }
            completingWork = false;
        }

        public void RefreshRow(DataRow row, VDBSelectCommand cmd = null, VDataColumn changingColumn = null, bool canDoAsync = true, bool withQueue=true)
        {
            //if (SuppressChangedEvent)   // !!! может быть нужно?
            //{
            //    BackgroundRefreshNext();
            //    return;
            //}
            if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached)
            {
                BackgroundRefreshNext();
                return;
            }
            if (cmd == null)
            {

                DontRefreshDependats = true;
                cmd = SingleRowRefreshCommand;

            }

            if (cmd == null)
            {
                return;
                
            }
          
            List<OracleParameter> pars;
            var tbl = this;
            var ds = tbl.GetDataSet();

            if (ds.InputParams != null)
            {
                pars = ds.InputParams.Values.Select(p => p).ToList();
            }
            else
            {
                pars = new List<OracleParameter>();


                
            }


            // !!! есть лишние манипуляции, разобраться 
            pars.Add(VDBSelectCommand.CreateKeyDBParameter(tbl, row));

            pars.AddRange(VDBSelectCommand.CreateForegnKeyDBParameter(tbl, row));

            pars.Add(VDBSelectCommand.TempRowIdParametr(tbl, row));

            pars.Add(VDBSelectCommand.CreateNewRowDBParameter(row));
            pars.Add(ds.CreateFormIdParametr());
            string[] parNames = Cmn.GetParameterNames(pars);
            var needParNames = cmd.GetParamsNames();
            foreach (string needParName in needParNames)
            {
                if (!parNames.Contains(needParName))
                {
                    pars.Add(ds.GetParamAsOracleParametr(needParName));
                }
            }
           

           
            //int procId = VDataSet.GetBackgroundProcessId();
            //VDataSet.AddWorkingProcess(procId);
            if (UIStatic.IsAsync && canDoAsync)
            {
                BackgroundWorker bw = new BackgroundWorker();
                bw.DoWork += delegate(object o, DoWorkEventArgs args)
                {
                    WaitSemaphore();
                    args.Result = RefreshRow_DoWork(cmd, pars, row);

                };


                bw.RunWorkerCompleted += delegate(object o, RunWorkerCompletedEventArgs args)
                {
                    RefreshRow_Complete(args.Result, changingColumn);
                    ReleaseSemaphore();
                };
                bw.RunWorkerAsync();
            }
            else
            {
                var res = RefreshRow_DoWork(cmd, pars, row);
                RefreshRow_Complete(res,  changingColumn,withQueue);
            }



        }

        

        private  object RefreshRow_DoWork( VDBSelectCommand cmd, List<OracleParameter> pars, DataRow row)
        {
            if (IsNonDb)
            {
                return null;
            }
            return  new Tuple<DataRow, DataTable>(
                    row,
                    cmd.ExecuteDataTable(pars.ToArray(),(OracleConnection) GetDataSet().GetConnection())
                    );
        }
        private void RefreshRow_Complete(object result, VDataColumn changingColumn = null, bool withQueue=true)
        {
            if (IsNonDb && result == null) return;
            completingWork = true;
            var tbl = this;
            var res = (Tuple<DataRow, DataTable>)result;
            var row2 = res.Item1;
            
            var valTbl = (DataTable)res.Item2;
            List<VDataColumn> changedColumns = new List<VDataColumn>();
            tbl.GetDataSet().ChangesNotCompleted = true;
            if (row2.RowState != DataRowState.Detached)
            {

                if (valTbl.Rows.Count > 0)
                {


                    tbl.UpdateRowValues(row2, valTbl.Rows[0]);



                }
            }
            
            //VDataSet.RemoveWorkingProcess(procId);
            //if (!VDataSet.HasWorkingProcesses())
            //{
            //    tbl.GetDataSet().ClearChangingColumns();
            //    tbl.GetDataSet().RaiseChangeCompleted();
            //}
            //else
            //{
            //}
            DontRefreshDependats = false;
            completingWork = false;
            if (withQueue)
            {
                BackgroundRefreshNext();
            }
        }
        public static SortedList<string, object> RowToArray(DataRow row)
        {
            var sourceArray = new SortedList<string, object>();
            foreach (DataColumn col in row.Table.Columns)
            {
                sourceArray.Add(col.ColumnName, row[col]);
            }
            return sourceArray;
        }
        public void UpdateRowValues(DataRow targetRow, DataRow sourceRow)
        {
             var sourceArray = RowToArray(sourceRow);
             UpdateRowValues(targetRow, sourceArray);
        }
        public void UpdateRowValues(DataRow targetRow , SortedList<string,object> sourceRow,bool raiseEvents=true)
        {
            if (targetRow==null) return; // происходит если включена асинхронность
            SuppressChangeEvent();
            var rst = targetRow.RowState;
            //var sourceTbl = sourceRow.Table;
            List<VDataColumn> changedColumns = new List<VDataColumn>();
            foreach (string col in sourceRow.Keys)
            {
                if (!TextConst.AVColumnArray.SysColNamesForEditedObject.Contains(col.ToLower()))
                {
                    //!!! заплатка . почему то приходит is_new= 0 для новой строки при первом рефреше
                    var targetColumn = (VDataColumn)Columns[col];
					//тут происходит слияние рядов обновленного и оригинального.
					if (targetColumn.SetValue(targetRow, sourceRow[col]))
					{
						changedColumns.Add(targetColumn);
					}
                }


               
            }

            ResumeChangeEvent();
            if (raiseEvents)
            {
                if (changedColumns.Count != 0) {
                    //UpdateTempRow(targetRow);
                    CancelTempUpdate = true;
                    foreach (var col in changedColumns)
                    {
                        var drd = DontRefreshDependats;
                        var ctu = CancelTempUpdate;
                        var nv = false;
                        if (col.DependantsNewVal != null && col.DependantsNewVal.Count != 0) {
                            UnsuppressChangeEvent();
                            CancelTempUpdate = false;
                            DontRefreshDependats = false;
                        }
                        col.RaiseDataChangeForUI(targetRow);
                        ProcessBehaviorChanges(col, targetRow, true);
                        GetDataSet().PrcessRefreshQueue();
                        col.ProcessChanges(targetRow);
                        CancelTempUpdate = ctu;
                        if (nv)
                        {
                            ResumeChangeEvent();
                        }
                        DontRefreshDependats = drd;
                    }

                    CancelTempUpdate = false;
                }
            }
            //else
            //{
            //    UpdateTempRow(targetRow);
            //}
            if (rst == DataRowState.Unchanged)
            {
                targetRow.AcceptChanges();
            }
        }
        
        public void SetForeignKey(DataRow row)
        {
            if (ParentRelations.Count > 0)
            {

                var parentCol = ParentRelations[0].ParentColumns[0];
                var val = (parentCol.Table as VDataTable).CurrentRow[parentCol];
                row[ParentRelations[0].ChildColumns[0]] = val;

                //    e.Row.Table.ParentRelations[0].ParentColumns[0]
            }
        }

		public bool IsForeignKeyAvailable()
		{
			if (ParentRelations.Count > 0)
			{

				var parentCol = ParentRelations[0].ParentColumns[0];
				var a = (parentCol.Table as VDataTable);
				
				return (!(a.CurrentRow == null) && ((a.CurrentRow.RowState == DataRowState.Modified) || (a.CurrentRow.RowState == DataRowState.Unchanged))) ;
			}
			else
			{
				return true;
			}
		}
        public bool CancelTempUpdate=false;


       public static  int delStateVal=3;
       public static int addStateVal = 1;
        public void SetUpdateTempRowParams(DataRow row, bool deleted=false)
        {
           

            var rowSatate = 0;

            if (deleted)
            {
                rowSatate = delStateVal;
            }
            else
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        rowSatate = addStateVal;
                        break;
                    case DataRowState.Modified:
                        rowSatate = 2;
                        break;
                    case DataRowState.Deleted:
                        rowSatate = delStateVal;// не будет работать
                        break;
                }
            }

            UpdateTempCommand.Connection = (OracleConnection)GetConnection();

            ApplyRowValuesToParams(row, UpdateTempCommand.Parameters.Cast<OracleParameter>().Where(p => p.SourceColumn != "").ToList(), false);
            UpdateTempCommand.Parameters[TextConst.DBParams.FormId].Value = GetDataSet().GetFormId();
            UpdateTempCommand.Parameters[TextConst.DBParams.RowStateId].Value = rowSatate;
        

            // var s = VDBSelectCommand.GetCmdParametrizedText(UpdateTempCommand);



        }



        public List<DataRow> ModifiedRows = null; // вроде используется толко при отладке

        public void AddModifiedRow(DataRow r)
        {
            if (ModifiedRows == null)
            {
                ModifiedRows = new List<DataRow>();
            }
            
            if (!ModifiedRows.Contains(r))
            {
                ModifiedRows.Add(r);
            }
        }
        private void WaitSemaphore()
        {
            semaphore1.WaitOne();
            
        }
        private void ReleaseSemaphore()
        {
            semaphore1.Release();
        }


        private HashSet<string> rowsToUpdateTemp = null;
        public void ClearRowsToUpdateTemp()
        {
            rowsToUpdateTemp = null;
        }
        public void AddRowToUpdateTemp(string id)
        {
            if (rowsToUpdateTemp == null)
            {
                rowsToUpdateTemp = new HashSet<string>();
            }
            if (!rowsToUpdateTemp.Contains(id))
            {
                rowsToUpdateTemp.Add(id);
            }
        }

        public void RemoveRowToUpdateTemp(string id)
        {
            if (rowsToUpdateTemp == null)
            {
                return;
            }
            if (rowsToUpdateTemp.Contains(id))
            {
                rowsToUpdateTemp.Remove(id);
            }
        }

        public bool IsRowToUpdateTemp(string id)
        {
            if (rowsToUpdateTemp == null)
            {
                return false;
            }

            return rowsToUpdateTemp.Contains(id);
            
        }





        private HashSet<string> ChangedRows = null;
        public void ClearChangedRows()
        {
            ChangedRows = null;
        }
        public void AddChangedRow(string id)
        {
            if (ChangedRows == null)
            {
                ChangedRows = new HashSet<string>();
            }
            if (!ChangedRows.Contains(id))
            {
                ChangedRows.Add(id);
            }
        }

        public void RemoveChangedRow(string id)
        {
            if (ChangedRows == null)
            {
                return;
            }
            if (ChangedRows.Contains(id))
            {
                ChangedRows.Remove(id);
            }
        }

        public bool IsRowChanged(string id)
        {
            if (ChangedRows == null)
            {
                return false;
            }

            return ChangedRows.Contains(id);

        }



        private HashSet<string> processedParents = null;

        public void ClearProcessedParents()
        {
            processedParents = null;
        }
        public void AddProcessedParent(string id)
        {
            if (processedParents == null)
            {
                processedParents = new HashSet<string>();
            }
            if (!processedParents.Contains(id))
            {
                processedParents.Add(id);
            }
        }

        public void RemoveProcessedParent(string id)
        {
            if (processedParents == null)
            {
                return;
            }
            if (processedParents.Contains(id))
            {
                processedParents.Remove(id);
            }
        }

        public bool IsParentProcessed(string id)
        {
            if (processedParents == null)
            {
                return false;
            }
            return processedParents.Contains(id);
        }


       

        //public void UpdateArrayTempTable(DataColumn col)
        //{
        //    GetDataSet().ArrayValueTable(col.ColumnName);
        //}
        private bool hasTemp = false;

        public void UpdateTempRowIfNew(DataRow row)
        {
            if (!hasTemp)
            {
                AddRowToUpdateTemp(GetRowId(row));
                UpdateTempRow(row);
            }
        }

        public void UpdateTempRow(DataRow row,bool deleted=false)
        {

            if (CancelTempUpdate) return;
            if (row.RowState == DataRowState.Detached) return;
            var rid = GetRowId(row);
            if (!IsRowToUpdateTemp(rid)) return ;
            RemoveRowToUpdateTemp(rid);
           // WaitSemaphore();
            SetUpdateTempRowParams(row,deleted);

            if (!IsNonDb)
            {
                hasTemp = true;
                DevAnalyzer.AnalyzeExecSql(UpdateTempCommand.CommandText);
                UpdateTempCommand.ExecuteNonQuery();

            }

           // ReleaseSemaphore();
            AddModifiedRow(row);
           // var s = VDBSelectCommand.GetCmdParametrizedText(UpdateTempCommand);
        }

        public void CrearTemp()
        {
            if (ClearTempCommand != null && hasTemp)
            {
                ClearTempCommand.Connection = (OracleConnection)GetConnection();
                ClearTempCommand.Parameters[TextConst.DBParams.FormId].Value = GetDataSet().GetFormId();
                DevAnalyzer.AnalyzeExecSql(ClearTempCommand.CommandText);
                ClearTempCommand.ExecuteNonQuery();
                hasTemp = false;
            }
        }
        //public void ClearTempRow(DataRow row)
        //{
        //    if (CancelTempUpdate) return;
        //    SetUpdateTempRowParams(row);
        //    UpdateTempCommand.ExecuteNonQuery();
        //    AddModifiedRow(row);
        //}


        public bool HasRowEvents = false;// Для создания линков при экспорте в excel
        private Dictionary<string, VSXElement> eventsTags;
        public void SetEvents(XElement xevents)
        {
            if (xevents == null)
            {
                return;
            }
            foreach (XElement xcmd in xevents.Elements(TextConst.EName.UseAction))
            {
                if (eventsTags == null)
                {
                    eventsTags = new Dictionary<string, VSXElement>();
                }
                var action = VSXElement.Get(new XElement(xcmd));
                eventsTags.Add(xcmd.Attribute(TextConst.AName.EventName).Value, action);
            }
        }


        public bool ProcessEvent(string name, DataRow row)
        {
            throw new NotImplementedException();
            //if (eventsTags != null)
            //{
            //    if (eventsTags.ContainsKey(name))
            //    {
            //        VUseAction.ExecuteAction(null, (VUseAction)eventsTags[name], GetDataSet(), null, this, row,null);
            //        return true;
            //    }
            //}
            //return false;
        }

       


    }
}
