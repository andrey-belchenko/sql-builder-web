using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using Devart.Data.Oracle;

using infoenergo.core.Data;
//using infoenergo.core.Extensions;
using sql.builder.DataApi.DataObjects;
using sql.builder.FieldInfo;
using sql.builder.UI;
using sql.builder.XmlHelpers;
using SqlBuilderLib.DevTools;
using sql.builder.Clean;

namespace sql.builder.DataApi
{
    public enum StructureType
    {
        Table = 0,
        Info = 1,
        Array = 2
    }
    public partial class VDataTable : DataTable
    {
        public VDataSet DataSetForFetch;
        private int defaultFetch = 100;
        public event EventHandler Changed;
        public event EventHandler TableRefreshed;
        public event EventHandler RefreshBegin;
        public event EventHandler RefreshEnd;
        public event EventHandler TableCommited;
        public event DataRowChangeEventHandler CurrentRowChanged;
        public event DataRowChangeEventHandler CurrentRowRefreshed;
        public OracleDataAdapter DataAdapter;
        public OracleCommand UpdateTempCommand;
        public OracleCommand ClearTempCommand;
        public bool HasControls;
        public XElement Scheme;
        public string QueryName;
        public bool editableOld;
        private StructureType structure_type;
        public UIBase Control;
        public bool AsyncLoad;
        private HashSet<string> paramNames; // кэш для GetParamsNames()
        private IEnumerable<string> ProcParamNames;
        private Stack<bool> suppressChangeEventStack = new Stack<bool>(new bool[] { false });
        public bool IsChangeEventSuppressed()
        {
            return this.suppressChangeEventStack.Peek();
        }
        public void SuppressChangeEvent()
        {
            this.suppressChangeEventStack.Push(true);
        }
        public void UnsuppressChangeEvent()
        {
            this.suppressChangeEventStack.Push(false);
        }
        public void ResumeChangeEvent()
        {
            this.suppressChangeEventStack.Pop();
        }
        public StructureType StructureType
        {
            get
            {
                return this.structure_type;
            }
            set
            {
                this.structure_type = value;
            }
        }
        private bool dontRefreshDependats;
        public bool DontRefreshDependats
        {
            get
            {
                return this.dontRefreshDependats;
            }
            set
            {
                this.dontRefreshDependats = value;
            }
        }
        public bool ParamUsed;
        public bool IsArrayParamStringUse;
        public string TreeParentFieldName;
        public string UpdateableTableName;
        public string KeyDimension;
        public List<object> CreatedItems;
        public void AddCreatedItem(object value)
        {
            if (this.CreatedItems == null)
            {
                this.CreatedItems = new List<object>(1);
            }
            this.CreatedItems.Add(value);
        }
        private bool _has_user_changes1;
        public bool HasUserChanges
        {
            get
            {
                return this._has_user_changes1;
            }
        }
        public bool HasChildrenUserChanges()
        {
            var queue = new Queue<VDataTable>();
            foreach (var r in ChildRelations.Cast<DataRelation>().Where(r1 => !((VDataTable)r1.ChildTable).IsArrayEditValue))
            {
                queue.Enqueue((VDataTable)r.ChildTable);
            }
            while (queue.Count > 0)
            {
                VDataTable child = queue.Dequeue();
                if (child.HasUserChanges)
                {
                    return true;
                }
                foreach (var r in child.ChildRelations.Cast<DataRelation>().Where(r1 => !((VDataTable)r1.ChildTable).IsArrayEditValue))
                {
                    queue.Enqueue((VDataTable)r.ChildTable);
                }
            }
            return false;
        }
        public VDataSet GetDataSet()
        {
            return (this.DataSet as VDataSet);
        }
        public IList<VDataColumn> GetColumnsByPivotOriginalName(string columnName)
        {
            VDataColumn col = this.GetColumn(columnName);
            if (col != null)
            {
                return new VDataColumn[1] { col };
            }
            else
            {
                var list = new List<VDataColumn>();
                for (int index = 0; index < this.Columns.Count; index++)
                {
                    col = (VDataColumn)this.Columns[index];
                    if (col.OriginalNameForPivotColumn == columnName)
                    {
                        list.Add(col);
                    }
                }
                return list;
            }
        }
        public VDataColumn GetColumn(string columnName)
        {
            return (VDataColumn)this.Columns[columnName];
        }
        public bool EditableOld
        {
            get
            {
                return this.editableOld;
            }
            set
            {
                this.editableOld = value;
                this.Scheme.SetAttrValue(AName.editable, value);
            }
        }
        #region Конструкторы
        /*public VDataTable()
            : base()
        {
            this.CaseSensitive = true;
            this.DataAdapter = new OracleDataAdapter();
            this.Scheme = null;
            this.attachEvents(false);
            this.InitManualDelete();
        }*/
        public VDataTable(bool withExtraEvents = false)
            : base()
        {
            this.CaseSensitive = true;
            this.DataAdapter = new OracleDataAdapter();
            this.DataAdapter.SelectCommand = new VOracleCommand();
            this.Scheme = null;
            this.attachEvents(withExtraEvents);
            this.InitManualDelete();
        }
        public VDataTable(XElement scheme, bool withExtraEvents, string table_name)
            : base(table_name)
        {
            this.CaseSensitive = true;
            this.DataAdapter = new OracleDataAdapter();
            this.DataAdapter.SelectCommand = new VOracleCommand();
            this.Scheme = scheme;
            this.EditableOld = false;
            this.attachEvents(withExtraEvents);
            this.InitManualDelete();
        }
        #endregion
        #region AddColumn
        public void AddColumn(string name)
        {
            VDataColumn col = new VDataColumn(name);
            this.Columns.Add(col);
        }
        public VDataColumn AddColumn(string name, Type type)
        {
            VDataColumn col = new VDataColumn(name, type);
            this.Columns.Add(col);
            return col;
        }
        public VDataColumn AddColumn(string name, Type type, string caption)
        {
            VDataColumn col = new VDataColumn(name, type, caption);
            this.Columns.Add(col);
            return col;
        }
        public void AddColumn(string name, string title)
        {
            VDataColumn col = new VDataColumn(name, typeof(string), title);
            this.Columns.Add(col);
        }
        #endregion
        private bool extraEventsAttached;
        private bool eventsAttached;
        public void AttachEvents()
        {
            attachEvents(true);
        }
        public void DetachEvents()
        {
            detachEvents(true);
        }
        private void attachEvents(bool withExtraEvents)
        {
            if (extraEventsAttached) return;
            //if (this.TableName == "spr_km")
            //{

            //}
            if (!eventsAttached)
            {
                this.ColumnChanged += VDataTable_ColumnChanged;
                this.RowChanged += VDataTable_RowChanged;

                this.RowDeleted += VDataTable_RowDeleted;
                this.TableCleared += VDataTable_TableCleared;
            }

            eventsAttached = true;
            if (withExtraEvents)
            {

                this.MyColumnChanged += changed;
                this.MyRowChanged += changed;
                this.MyRowDeleted += changed;
                this.MyTableCleared += changed;

                this.MyColumnChanged += RaiseUserChangedData;

                extraEventsAttached = true;
            }
        }
        public event DataTableClearEventHandler MyTableCleared = null;
        public event DataRowChangeEventHandler MyRowChanged = null;
        public event DataRowChangeEventHandler MyRowAdded = null;
        public event DataRowChangeEventHandler MyRowDeleted = null;
        public event DataRowChangeEventHandler RowStateChanged = null;
        public event DataColumnChangeEventHandler MyColumnChanged = null;
        //  повторяет ColumnChanged вся внешняя подписка только на него, и внутренняя тоже кроме одного обработчика
        // иначе фиг отследишь

        void VDataTable_TableCleared(object sender, DataTableClearEventArgs e)
        {


            if (MyTableCleared != null)
            {
                MyTableCleared(sender, e);
            }
        }

        void VDataTable_RowDeleted(object sender, DataRowChangeEventArgs e)
        {
            // if (IsChangeEventSuppressed()) return;
            if (MyRowDeleted != null)
            {
                MyRowDeleted(sender, e);
            }

        }

        void VDataTable_RowChanged(object sender, DataRowChangeEventArgs e)
        {
            isClear = false;
            if (IsChangeEventSuppressed())
            {
                return;
            }

            if (MyRowChanged != null)
            {
                MyRowChanged(sender, e);
            }
        }

        public void RaiseColumnChanged(DataColumn column, DataRow row)
        {
            var args = new DataColumnChangeEventArgs(row, column, row[column]);
            VDataTable_ColumnChanged(this, args);

        }


        private void RaiseRowStateChanged(DataRow row)
        {
            var args = new DataRowChangeEventArgs(row, DataRowAction.Change);
            if (RowStateChanged != null)
            {
                RowStateChanged(this, args);
            }

        }



        void VDataTable_ColumnChanged(object sender, DataColumnChangeEventArgs e)
        {

            if (e.Row.RowState == DataRowState.Detached)
            {
                return;
            }
            //if (_checkingRows)
            //{
            //    return; // иначе сбивается выделение, вроде реакция контрола на endedit
            //}

            e.Row.EndEdit();// если так не сделать строка Unchanged
            //, а должна быть Modified для дальнейшей обработки
            // далее происходит accept , после чего данные не сохраняются
            if (IsChangeEventSuppressed()) return;

            //if (e.Column.ColumnName == "kod_krit_minenergo")
            //{
            //}
            if (MyColumnChanged != null)
            {
                MyColumnChanged(sender, e);
            }
            if (e.Column is VDataColumn)
            {
                (e.Column as VDataColumn).RaiseDataChangeForUI(e.Row); // похоже срабатывает 2 раза при изменении некоторых колонок.
            }
        }






        private void detachEvents(bool withExtraEvents)
        {
            if (!extraEventsAttached) return;

            if (eventsAttached)
            {
                this.ColumnChanged -= VDataTable_ColumnChanged;
                this.RowChanged -= VDataTable_RowChanged;
                this.RowDeleted -= VDataTable_RowDeleted;
                if (_deletedRows != null)
                {
                    this.RowDeleting -= table_OnRowDeleting;
                }
                this.TableCleared -= VDataTable_TableCleared;
            }

            eventsAttached = false;
            if (withExtraEvents)
            {


                this.MyColumnChanged -= changed;
                this.MyRowChanged -= changed;
                this.MyRowDeleted -= changed;
                this.MyTableCleared -= changed;

                this.MyColumnChanged -= RaiseUserChangedData;

                extraEventsAttached = false;
            }
        }




        public void changed(object sender, EventArgs e)
        {

            if (IsChangeEventSuppressed() || this.StructureType == StructureType.Info) return;

            if (Changed != null)
            {
                Changed(sender, e);
            }
            if (this.DataSet != null)
            {
                ((VDataSet)this.DataSet).changed(sender, e);
            }

        }
        private DataRow _currentRow = null;

        private DataRow currentRow
        {
            get
            {
                return _currentRow;
            }
            set
            {
                if (value != null)
                {

                }
                _currentRow = value;
            }
        }

        public bool NewCurRowApplying = false;
        public DataRow CurrentRow
        {
            get
            {
                if (currentRow != null)
                {
                    if (currentRow.RowState == DataRowState.Detached)
                    {
                        currentRow = null;
                    }
                }
                if (currentRow == null)
                {
                    if (this.Grid == null || !this.Grid.IsTree())
                    {
                        if (this.Rows.Cast<DataRow>().Count(r => r.RowState != DataRowState.Detached) > 0)
                        {
                            NewCurRowApplying = true;
                            currentRow = this.Rows[0];
                            RaiseCurrentRowChanged();
                            NewCurRowApplying = false;
                        }
                    }
                }
                if (currentRow != null)
                {
                    if (currentRow.RowState == DataRowState.Detached)
                    {
                        currentRow = null;
                    }
                }

                return currentRow;
            }
            set
            {
                if (currentRow != value)
                {
                    NewCurRowApplying = true;
                    currentRow = value;

                    RaiseCurrentRowChanged();
                    NewCurRowApplying = false;
                }
            }
        }

        public void RaiseCurrentRowRefreshed(DataRow row)
        {
            if (row == currentRow)
            {
                if (CurrentRowRefreshed != null)
                {
                    CurrentRowRefreshed(this, new DataRowChangeEventArgs(currentRow, DataRowAction.Nothing));
                }


                foreach (DataColumn column in this.Columns)
                {

                    if (column is VDataColumn)
                    {
                        ProcessBehaviorChanges((VDataColumn)column, currentRow, true);
                    }
                }
                GetDataSet().PrcessRefreshQueue();
            }
        }
        public void RaiseCurrentRowChanged(DataRow row = null)
        {
            if (currentRow != null)
            {
                if (SelectedRows.Count == 0)
                {
                    if (Grid == null || !Grid.IsCheckBoxSelection())
                    {
                        _selectedRows = new List<DataRow> { currentRow };
                    }

                    //RaiseDataSourceSelectionChanged();
                }
            }

            if (this.IsChangeEventSuppressed()) return;
            if (this.Columns.Contains("node"))
            {
                var val = CurrentRow["node"] as VSXElement;
                if (val != null) val.UpdateDataRow();
            }

            if (CurrentRowChanged != null)
            {
                CurrentRowChanged(this, new DataRowChangeEventArgs(currentRow, DataRowAction.Nothing));
            }

            foreach (VDataTable tbl in GetChildTables())
            {
                tbl.parentCurrentRowChanged();
            }


            if (childDataSets != null)
            {
                foreach (VDataSet ds in childDataSets)
                {
                    if (ds.IsVisibleInLayout())
                    {
                        ds.RefreshTopTable(false);
                        if (ds.Form != this.GetDataSet().Form)
                        {
                            ds.Form.RaiseUIEvent(TextConst.AVEventName.FormLoaded);
                        }
                    }
                    else
                    {
                        ds.ClearData();
                    }
                }
            }

            foreach (DataColumn column in this.Columns)
            {

                if (column is VDataColumn)
                {
                    ProcessBehaviorChanges((VDataColumn)column, currentRow, false);
                }
            }
            GetDataSet().PrcessRefreshQueue();
            processSelection();


        }


        public List<VDataTable> GetChildTables()
        {
            return ChildRelations.Cast<DataRelation>().Select(r => (VDataTable)r.ChildColumns[0].Table).Distinct().ToList();
        }

        public List<VDataSet> childDataSets = null;

        public void AddChildDataset(VDataSet dataSet, string columnName)
        {
            if (childDataSets == null)
            {
                childDataSets = new List<VDataSet>();
            }
            childDataSets.Add(dataSet);
            dataSet.ParentColumnName = columnName;
            dataSet.ParentDataTable = this;
        }

        private VDataTable ParentTable()
        {
            if (ParentRelations.Count == 1)
            {
                return (VDataTable)ParentRelations[0].ParentColumns[0].Table;
            }
            return null;
        }
        private void parentCurrentRowChanged()
        {
            if (this.is_dependant_refresh && !this.IsArrayEditValue)
            {
                this.Refresh();
            }
        }
        //public void RaiseRowChanged(DataRow row)
        //{
        //    DataRowChangeEventArgs e = new DataRowChangeEventArgs(row, DataRowAction.Nothing);
        //    changed(this, e);
        //}
        public OracleDataReader Reader; // для печати
        public bool UseDeferredFetch;
        public bool IsDeferredFetch()
        {
            return this.UseDeferredFetch && this.HasPrimaryKey();
        }
        public int fetchedRowsCount = 0;
        private IDataReader otherReader = null; // для частичной загрузки в грид,  чтобы не пересекались функции

        public void FetchNext(int rowsCount)
        {
            if (rowsCount == int.MaxValue)
            {
                FetchTo(int.MaxValue);
            }
            else
            {
                FetchTo(fetchedRowsCount + rowsCount);
            }
        }

        private bool isFetching = false;

        public void ReadAll() // используется в отчетах
        {

            if (Reader == null) return;
            if (fetchedRowsCount > 0) return;



            this.SuppressChangeEvent();
            var tbl = Cmn.CopyTableStructure(this);




            bool isNewRows = false;
            while (true)
            {

                bool isread = Reader.Read();
                if (isread)
                {

                    DataRow r = tbl.Rows.Add();

                    foreach (DataColumn col in tbl.Columns)
                    {
                        var ind = Reader.GetOrdinal(col.ColumnName);
                        r[col] = Reader.GetValue(ind);

                    }
                    r.AcceptChanges();
                    fetchedRowsCount++;
                    isNewRows = true;
                }
                else
                {
                    break;
                }

            }


            if (isNewRows)
            {
                this.Merge(tbl);




            }



            this.ResumeChangeEvent();

        }


        public void FetchTo(int lastRowIndex) // используется в в редакторе данных
        {




            int lastRowIndex1 = lastRowIndex;
            IDataReader reader = otherReader;
            if (reader == null) return;

            if (isFetching) return;


            if (lastRowIndex == int.MaxValue)
            {
                WaitUIHelper.LastUsedUIHelper.Show("Закрузка записей..", WaitUIMode.WaitCursor);
            }

            isFetching = true;
            this.SuppressChangeEvent();
            var tbl = Cmn.CopyTableStructure(this);
            tbl.SuppressChangeEvent();
            //var tbl = this;
            bool isNewRows = false;
            //   tbl.BeginLoadData();
            bool zeroFetch = false;

            bool allowNewCurrentRow = false;
            if (fetchedRowsCount == 0)
            {
                allowNewCurrentRow = true;
            }


            int fetchedRowsCount1 = fetchedRowsCount;

            if (DataSetForFetch != null)
            {
                zeroFetch = true;
                var efKeyParVals = new Dictionary<string, List<object>>();
                var efAllParVals = new Dictionary<string, object>();
                foreach (var par in DataSetForFetch.InputParams)
                {
                    efAllParVals.Add(par.Key, null);

                    if (GetDataSet().InputParams.ContainsKey(par.Key))
                    {
                        efAllParVals[par.Key] = GetDataSet().InputParamsValues[par.Key];


                    }
                    else
                    {
                        efKeyParVals.Add(par.Key, new List<object>());
                        efKeyParVals[par.Key].Add(TextConst.NullConsts.nnullVal);// костыль , на случай если нет ни одного значения
                    }
                }

                for (int i = fetchedRowsCount; i <= lastRowIndex1; i++)
                {
                    Wait.Check();
                    bool isread = reader.Read();
                    if (isread)
                    {
                        zeroFetch = false;
                        isNewRows = true;
                        //DataRow r = tbl.Rows.Add();

                        var fieldNames = Enumerable.Range(0, reader.FieldCount).Select(n => reader.GetName(n).ToLower()).ToArray();
                        foreach (var col in efKeyParVals.Keys)
                        {
                            if (!fieldNames.Contains(col)) continue;
                            var ind = reader.GetOrdinal(col);
                            var val = reader.GetValue(ind);
                            if (Cmn.Nvl(val, null) != null)
                            {
                                efKeyParVals[col].Add(val);
                            }
                        }
                        //fetchedRowsCount++;
                    }
                    else
                    {
                        break;
                    }

                }

                if (!zeroFetch)
                {
                    DataSetForFetch.GetAllTables().First().defaultFetch = 0;

                    var efAllParVals1 = new List<object>();
                    foreach (var parName in DataSetForFetch.InputParamsNames.Values)
                    {
                        object val = null;
                        if (efKeyParVals.ContainsKey(parName))
                        {
                            var arrVal = (efKeyParVals[parName] as List<object>).ToArray();
                            if (arrVal.Length != 0)
                            {
                                val = (efKeyParVals[parName] as List<object>).ToArray();
                            }
                            else
                            {
                                val = Cmn.undefinedString;
                            }
                        }
                        else if (efAllParVals.ContainsKey(parName))
                        {
                            val = efAllParVals[parName];
                        }
                        efAllParVals1.Add(val);
                    }
                    DataSetForFetch.Refresh(efAllParVals1.ToArray());
                    reader = DataSetForFetch.GetAllTables().First().otherReader;
                    fetchedRowsCount1 = 0;
                    lastRowIndex1 = int.MaxValue;
                }


            }


            if (!zeroFetch)
            {
                for (int i = fetchedRowsCount1; i <= lastRowIndex1; i++)
                {
                    bool isread;
                    Wait.Check();
                    //if (isRead && i==0)
                    //{
                    //    isread = !reader.IsEmpty();
                    //}
                    //else
                    //{

                    isread = reader.Read();
                    //}
                    if (isread)
                    {
                        isNewRows = true;
                        DataRow r = tbl.Rows.Add();

                        // var fieldNames = Enumerable.Range(0, otherReader.FieldCount).Select(n => otherReader.GetName(n).ToLower()).ToArray();
                        foreach (DataColumn col in tbl.Columns)
                        {
                            //  if (!fieldNames.Contains(col.ColumnName)) continue;

                            var ind = reader.GetOrdinal(col.ColumnName);

                            r[col] = reader.GetValue(ind);

                        }
                        r.AcceptChanges();
                        //if (DataSetForFetch == null)
                        //{
                        fetchedRowsCount++;
                        //}
                    }
                    else
                    {
                        break;
                    }

                }
            }

            //    tbl.EndLoadData();

            if (isNewRows)
            {
                //var stra = new SortedList<string, string>();

                //foreach (DataRow r in tbl.Rows)
                //{
                //    stra.Add(r[this.PrimaryKey[0].ColumnName].ToString(),null);
                //}

                this.Merge(tbl);
                if (HasControls)
                {
                    var newRows = new List<DataRow>();

                    foreach (DataRow r in tbl.Rows)
                    {
                        var pkval = r[this.PrimaryKey[0].ColumnName];
                        var r1 = this.Rows.Find(pkval);
                        newRows.Add(r1);
                    }
                    UpdateDidplayValue(newRows);
                }

            }



            this.ResumeChangeEvent();
            isFetching = false;

            if (allowNewCurrentRow)
            {
                if (Rows.Count > 0)
                {
                    currentRow = Rows[0];

                }
                else if (currentRow != null)
                {
                    currentRow = null;

                }
                RaiseCurrentRowChanged();
            }

            if (lastRowIndex == int.MaxValue)
            {
                WaitUIHelper.LastUsedUIHelper.Hide();
            }
        }
        public OracleCommand cmd;
        #region IsDependantRefresh
        private bool is_dependant_refresh;
        public void SetDependantRefresh(bool value)
        {
            this.is_dependant_refresh = value;
        }
        public static bool IsDependantRefresh(DataTable dt)
        {
            VDataTable vdt = dt as VDataTable;
            if (vdt == null)
            {
                return false;
            }
            else
            {
                return vdt.is_dependant_refresh;
            }
        }
        #endregion
        public void ClearChildsData()
        {
            foreach (VDataTable tbl in GetChildTables())
            {
                tbl.ClearData();
            }
        }

        private bool isClear = true;
        public void ClearData()
        {
            if (isClear) return;
            _refreshed = false;
            ClearChildsData();
            Rows.Clear();
            for (int index = 0; index < this.Columns.Count; index++)
            {
                VDataColumn vcol = this.Columns[index] as VDataColumn;
                if (vcol != null)
                {
                    if (vcol.BoundControls != null)
                    {
                        for (int control_index = 0; control_index < vcol.BoundControls.Count; control_index++)
                        {
                            vcol.BoundControls[control_index].SetNeedRefreshList();
                        }
                    }
                }
            }

            CreatedItems = null;
            CrearTemp();
            ResetValidation();
            ClearProcessedParents();
            ClearRowsToUpdateTemp();
            CheckedRows = null;
            _selectedRows = null;

            ClearChangedRows();
            if (ModifiedRows != null)
            {
                ModifiedRows.Clear();
            }
            if (GetDataSet().ParamsTable != null && GetDataSet().ParamsTable != this && this.StructureType != StructureType.Array)
            {
                GetDataSet().SetVariableValue(TableName + TextConst.AVParam.HasChanges, 0, false);
            }
            this._has_user_changes1 = false;
            if (childDataSets != null)
            {
                foreach (VDataSet ds in childDataSets)
                {
                    ds.ClearData();

                }
            }
            isClear = true;
        }


        public OracleCommand ProcedureCommand = null;
        //public OracleCommand UpdateTempCommand = null;
        //public XElement XQuery = null;
        public string GetFKColName()
        {
            if (ParentRelations == null || ParentRelations.Count == 0)
            {
                return null;
            }

            return ParentRelations[0].ChildColumns[0].ColumnName;

        }
        public static void SetCommandParams(VDataSet ds, VDataTable dt, DbCommand command, IEnumerable<string> paramNames)
        {
            command.Parameters.Clear();
            DataRelation rel;
            VDataTable parentTable;
            string fkName;
            if (dt != null && dt.is_dependant_refresh)
            {
                rel = dt.ParentRelations[0];
                parentTable = (VDataTable)rel.ParentColumns[0].Table;
                fkName = TextConst.Pfx.ForegnKeyParam + rel.ChildColumns[0].ColumnName;
            }
            else
            {
                rel = null;
                parentTable = null;
                fkName = null;
            }
            foreach (string param_name in paramNames)
            {
                OracleParameter dbPar;
                if (!VDBSelectCommand.TryGetGlobalDbParam(param_name, out dbPar))
                {
                    if (param_name == fkName)
                    {
                        dbPar = new OracleParameter(fkName, Cmn.GetDBType(rel.ChildColumns[0].DataType));
                        if (parentTable.CurrentRow != null)
                        {
                            if (parentTable.CurrentRow.RowState == DataRowState.Added)
                            {
                                dbPar.Value = DBNull.Value;
                            }
                            else
                            {
                                dbPar.Value = (rel.ParentColumns[0] as VDataColumn).GetValue(parentTable.CurrentRow);
                            }
                        }
                    }
                    if (dbPar == null && ds.InputParams != null)
                    {
                        OracleParameter input_param;
                        if (ds.InputParams.TryGetValue(param_name, out input_param))
                        {
                            dbPar = new OracleParameter(input_param.ParameterName, input_param.OracleDbType, input_param.Value, ParameterDirection.Input);
                        }
                    }
                    if (dbPar == null)
                    {
                        dbPar = ds.GetParamAsOracleParametr(param_name);
                    }
                }
                if (dbPar.OracleDbType == OracleDbType.Array || Cmn.undefinedString.Equals(dbPar.Value))
                {
                    command.CommandText = command.CommandText.Replace(":" + param_name, dbPar.Value.ToString());
                }
                else
                {
                    command.Parameters.Add(dbPar);
                }
            }
        }
        public HashSet<string> GetParamsNames()
        {
            if (this.paramNames == null)
            {
                string sql = this.DataAdapter.SelectCommand.CommandText;
                this.paramNames = new HashSet<string>();
                foreach (string param_name in Cmn.ExtractParameterNamesFromSQL(sql).OrderByDescending(Cmn.LengthOfString))
                {
                    this.paramNames.Add(param_name);
                }
            }
            return paramNames;
        }
        public bool IsReader = false;
        public IList<DataRow> GetRowsForCurrentParent()
        {
            VDataTable parentTable = this.GetParentTable();
            if (parentTable == null)
            {
                return this.Rows.ToArray();
            }
            else if (parentTable.currentRow == null)
            {
                return Array.Empty<DataRow>();
            }
            else
            {
                DataRelation rel = this.ParentRelations[0];
                DataColumn pkColumn = rel.ParentColumns[0];
                DataColumn fkColumn = rel.ChildColumns[0];
                string pkVal = parentTable.CurrentRow[pkColumn].ToString();
                // return this.AsEnumerable().Where(r => r.RowState != DataRowState.Deleted && r[fkColumn].ToString() == pkVal).ToList();
                List<DataRow> rows = new List<DataRow>();
                for (int index = 0; index < this.Rows.Count; index++)
                {
                    DataRow row = this.Rows[index];
                    if (row.RowState != DataRowState.Deleted && row[fkColumn].ToString() == pkVal)
                    {
                        rows.Add(row);
                    }
                }
                return rows;
            }
        }
        private bool _refreshWhenVisible = false;
        private void RaiseBeginRefresh()
        {
            if (RefreshBegin != null)
            {
                RefreshBegin(this, null);
            }
        }
        private void RaiseEndRefresh()
        {
            if (RefreshEnd != null)
            {
                RefreshEnd(this, null);
            }
        }
        public void Refresh()
        {
            RaiseBeginRefresh();
            var s = "";
            Refresh(ref s);
            RaiseEndRefresh();
        }

        public void EnqueueRefresh()
        {
            GetDataSet().EnqueueTableRefresh(this.TableName);
        }

        bool _forceRefresh = false;
        public void SetForceRefresh()
        {
            _forceRefresh = true;
        }
        public void Refresh(ref string retSql, bool onlyProc = false)
        {

            WaitUIHelper.LastUsedUIHelper.Show("Обновление таблицы", WaitUIMode.WaitCursor);
            try
            {
                if (OnlyForceRefresh)
                {
                    if (!_forceRefresh)
                    {
                        return;
                    }
                }

                if (OnlyVisibleRefresh)
                {
                    if (!isVisibleInLayout())
                    {
                        _refreshWhenVisible = true;
                        return;
                    }
                }

                SendBeginUpdateToUI();
                bool refresh = false;
                if (!IsArrayEditValue)
                {
                    rememberSelection();
                    ClearData();
                    foreach (VDataTable tbl in GetChildTables())
                    {
                        if (tbl.IsArrayEditValue)
                        {
                            tbl.RaiseCurrentRowChanged(null); // чтобы обработалась видимость
                        }
                    }

                    refresh = true;
                }
                else
                {
                    var parentTable = GetParentTable();
                    var parentRow = parentTable.currentRow;
                    if (parentRow != null)
                    {
                        var id = parentTable.GetRowId(parentRow);
                        if (!IsParentProcessed(id))
                        {
                            AddProcessedParent(id);
                            refresh = true;
                        }
                    }
                }
                if (refresh)
                {
                    fill(ref retSql, onlyProc);
                    recollectSelection();
                }
                SyncChecksIfNeed();
                SendEndUpdateToUI();
            }
            finally
            {
                WaitUIHelper.LastUsedUIHelper.Hide();
            }
        }
        private void UpdateDidplayValue()
        {
            if (!this.HasControls) return;
            if (this.Grid == null) return; // затертое значение сейчас не возвращается, если ячейка становится существующей, пока отменю это для формы, потом реализовать возврат
            DataRowCollection rows = this.Rows;
            DataColumnCollection columns = this.Columns;
            for (int column = 0; column < columns.Count; column++)
            {
                VDataColumn dc = columns[column] as VDataColumn;
                if (dc != null && VDataColumn.HasBoundControl(dc))
                {
                    for (int row = 0; row < rows.Count; row++)
                    {
                        dc.UpdateDidplayValue(rows[row]);
                    }
                }
            }
        }
        private void UpdateDidplayValue(IList<DataRow> rows)
        {
            if (!this.HasControls) return;
            if (this.Grid == null) return; // затертое значение сейчас не возвращается, если ячейка становится существующей, пока отменю это для формы, потом реализовать возврат
            DataColumnCollection columns = this.Columns;
            for (int column = 0; column < columns.Count; column++)
            {
                VDataColumn dc = columns[column] as VDataColumn;
                if (dc != null && !List.IsNullOrEmpty(dc.BoundControls))
                {
                    for (int row = 0; row < rows.Count; row++)
                    {
                        dc.UpdateDidplayValue(rows[row]);
                    }
                }
            }
        }
        private void fill(ref string retSql, bool onlyProc = false)
        {
            VDataSet dataset = this.GetDataSet();
#if DEBUG
            string caller_name;
            if (dataset.Report != null)
            {
                caller_name = "report \"" + dataset.Report.AttrOrEmpty(AName.name) + "\", ";
            }
            else if (dataset.Form != null)
            {
                caller_name = "form \"" + dataset.Form.GetFormName() + "\", ";
            }
            else
            {
                caller_name = string.Empty;
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            this.isClear = false;
            this._forceRefresh = false;
            string buffer = this.DataAdapter.SelectCommand.CommandText;
            string procBuffer = null;
            if (this.ProcedureCommand != null)
            {
                procBuffer = this.ProcedureCommand.CommandText;
            }
            //для подстановки array параметров
            this.DataAdapter.SelectCommand.Connection = this.GetConnection();
            if (dataset.Report == null || dataset.Report.IsSimpleParams)
            {
                HashSet<string> parNames = this.GetParamsNames();
                if (dataset.VariableColumns != null)
                {
                    foreach (string name in parNames)
                    {
                        VDataColumn col;
                        if (dataset.VariableColumns.TryGetValue(name, out col))
                        {
                            if (col.Table != dataset.ParamsTable)
                            {
                                if (!dataset.IsVariableHasValue(name))
                                {
                                    return;
                                }
                            }
                        }
                    }
                }
                SetCommandParams(dataset, this, this.DataAdapter.SelectCommand, parNames);
                if (this.ProcedureCommand != null)
                {
                    if (this.ProcParamNames == null)
                    {
                        this.ProcParamNames = Cmn.ExtractParameterNamesFromSQL(this.ProcedureCommand.CommandText).OrderByDescending(Cmn.LengthOfString);
                    }
                    SetCommandParams(dataset, this, this.ProcedureCommand, this.ProcParamNames);
                }
                if (this.is_dependant_refresh)
                {
                    DataRelation rel = this.ParentRelations[0];
                    VDataTable parentTable = (VDataTable)rel.ParentColumns[0].Table;
                    if (parentTable.CurrentRow == null)
                    {
                        if (currentRow != null)
                        {
                            currentRow = null;
                        }
                        this.RaiseCurrentRowChanged();// 170213 Бельченко, чтобы не отображалось скрытое поле на вкладке исп листы.
                        return;
                    }
                }
            }
            OracleCommand sel_cmd = this.DataAdapter.SelectCommand;
            sel_cmd.CommandText = Cmn.ClearUndefined(sel_cmd.CommandText);
            if (sel_cmd.CommandText != string.Empty || this.IsNonDb)
            { //Бельченко 01082015 убрал, посмотрим что получится // вернул
                // Убираем неиспользуемые параметры
                OracleParameterCollection parameters = sel_cmd.Parameters;
                for (int index = parameters.Count - 1; index >= 0; index--)
                {
                    if (!sel_cmd.CommandText.Contains(":" + parameters[index].ParameterName))
                    {
                        parameters.RemoveAt(index);
                    }
                }
                //Пустой запрос может получиться если есть udnefined которые нельзя выкинуть
                try
                {
                    if (this.ProcedureCommand != null)
                    {
                        this.ProcedureCommand.Connection = sel_cmd.Connection;
                        DevUtilsProvider.Instance.AnalyzeExecSql(this.ProcedureCommand.CommandText);
                        this.ProcedureCommand.ExecuteNonQuery();
                    }
                    bool done = false;
                    if (this.AsyncLoad)
                    {
                        OracleCommand command = VDBSelectCommand.CopyCommand(sel_cmd);
                        this.AsyncExecuteReader(command);
                        done = true;
                    }
                    else if (onlyProc)
                    {
                        retSql += sel_cmd.CommandText;
                        done = true;
                    }
                    else if (dataset.Report != null && dataset.Report.AttrOrDefault(AName.datareader, false))
                    {
                        // значит заполняем read для построчного чтения
                        this.IsReader = true; // по идее, нужно где-то раньше заполнять и потом везде на этот признак ориентироваться, пока так
                        this.cmd = VDBSelectCommand.CopyCommand(this.DataAdapter.SelectCommand);
                        this.cmd.FetchSize = 100;
                        this.fetchedRowsCount = 0;
                        WaitUIHelper.LastUsedUIHelper.SetDescription("Выполнение запроса к БД...");
                        DevUtilsProvider.Instance.AnalyzeExecSql(this.cmd.CommandText);
                        this.Reader = this.cmd.ExecuteReader();// !!! выполняется при печати тут наверное не нужно, проверить/убрать
                        WaitUIHelper.LastUsedUIHelper.SetDescription(WaitUIHelper.DESCRIPTION_DEFAULT);
                        //Теперь нужно, при !UseTempTable см. PrintTableReferense.cs 408
                        done = true;
                        if (dataset.Report.AttrOrEmpty(AName.nogrid) != TextConst.AVBool.True)
                        {
                            done = false;
                            this.UseDeferredFetch = true;
                        }
                    }
                    if (!done)
                    {
                        if (!this.IsNonDb)
                        {
                            if (this.IsDeferredFetch())
                            {
                                DevUtilsProvider.Instance.AnalyzeExecSql(this.DataAdapter.SelectCommand.CommandText);
                                this.otherReader = this.DataAdapter.SelectCommand.ExecuteReader();
                                this.fetchedRowsCount = 0;
                                if (this.defaultFetch > 0)
                                {
                                    this.FetchTo(this.defaultFetch);
                                }
                            }
                            else
                            {
                                WaitUIHelper.LastUsedUIHelper.SetDescription("Выборка данных из БД...");
                                this.SuppressChangeEvent();
                                //this.FetchAllRows();
#if DEBUG
                                Stopwatch fetch_sw = new Stopwatch();
                                fetch_sw.Start();
#endif
                                // DevAnalyzer.AnalyzeSuppressedSql(this.DataAdapter.SelectCommand.CommandText);
                                if (!DevUtilsProvider.Instance.IsPrepareOnly())
                                {
                                    this.DataAdapter.Fill(this);
                                }
#if DEBUG
                                fetch_sw.Stop();
                                // Debug.WriteLine("OracleDataAdapter.Fill(): " + caller_name + "query \"" + this.TableName + "\", " + this.Columns.Count.ToString() + " колонок и " + this.Rows.Count.ToString() + " строк за " + fetch_sw.ElapsedTicks.ToString() + " тактов = " + fetch_sw.ElapsedMilliseconds.ToString() + " мс");
#endif
                                this.InternDataAsNeeded();
                                this.UpdateDidplayValue();
                                this.ResumeChangeEvent();
                                WaitUIHelper.LastUsedUIHelper.SetDescription(WaitUIHelper.DESCRIPTION_DEFAULT);
                            }
                        }
                        else
                        {
                            this.SuppressChangeEvent();
                            this.raiseCustomFill();
                            this.UpdateDidplayValue();
                            this.ResumeChangeEvent();
                        }
                        this.ClientLoadComplete();
                    }
                }
                catch (Exception e)
                {
                    throw e;
                    //throw new OracleSqlException(e, this.DataAdapter.SelectCommand.CommandText);
                }
            }
            this.RestoreCommands(buffer, procBuffer);
#if DEBUG
            sw.Stop();
            // Debug.WriteLine("VDataTable.fill(): " + caller_name + "query \"" + this.TableName + "\", " + sw.ElapsedTicks.ToString() + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
#endif
        }
        /*private void FetchAllRows()
        {
            #if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #endif
            //this.DataAdapter.Fill(this);
            int col_count;
            using (OracleDataReader reader = this.DataAdapter.SelectCommand.ExecuteReader()) {
                col_count = reader.FieldCount;
                int index;
                //int[] col_indexes = new int[col_count];
                #if DEBUG
                Contract.Assume(col_count <= this.Columns.Count);
                for (index = 0; index < col_count; index++) {
                    Contract.Assume(string.Compare(this.Columns[index].ColumnName, reader.GetName(index), true) == 0);
                }
                #endif
                while (reader.Read()) {
                    DataRow row = this.NewRow();
                    for (index = 0; index < col_count; index++) {
                        row[index] = reader.GetValue(index);
                    }
                    this.Rows.Add(row);
                }
            }
            #if DEBUG
            sw.Stop();
            Debug.WriteLine("VDataTable.FetchAllRows(): query \"" + this.TableName + "\", " + col_count.ToString() + " колонок и " + this.Rows.Count.ToString() + " строк за " + sw.ElapsedTicks.ToString() + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
            #endif
        }*/
        public void InternDataAsNeeded()
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            DataRowCollection rows = this.Rows;
            if (rows.Count <= 0)
            {
                return;
            }
            // Составляем список интернируемых колонок
            IList<VInternedStringDataColumn> columns = new List<VInternedStringDataColumn>(8);
            VInternedStringDataColumn column;
            int col_index;
            DataColumnCollection cols = this.Columns;
            for (col_index = 0; col_index < cols.Count; col_index++)
            {
                column = cols[col_index] as VInternedStringDataColumn;
                if (column != null)
                {
                    columns.Add(column);
                }
            }
            if (columns.Count <= 0)
            {
                return;
            }
            IDictionary<string, string> cache = new Dictionary<string, string>(StringComparer.InvariantCulture);
#if DEBUG
            long replace_count = 0;
            long characters_count = 0;
#endif
            for (int row_index = 0; row_index < rows.Count; row_index++)
            {
                DataRow row = rows[row_index];
                bool changed = false;
                for (col_index = 0; col_index < columns.Count; col_index++)
                {
                    column = columns[col_index];
                    if (!row.IsNull(column))
                    {
                        string old_value = (string)row[column];
                        string new_value;
                        if (!cache.TryGetValue(old_value, out new_value))
                        {
                            // Если строка интернирована, используем интернированное значение
                            new_value = string.IsInterned(old_value);
                            if (new_value == null)
                            {
                                // А если строка не интернирована, используем значение из DataTable,
                                // чтобы не забивать мусором таблицу интернированых строк
                                // и чтобы эти строки могли быть удалены сборщиком мусора.
                                new_value = old_value;
                            }
                            cache.Add(new_value, new_value);
                        }
                        if (!object.ReferenceEquals(old_value, new_value))
                        {
                            if (!changed)
                            {
                                row.BeginEdit();
                                changed = true;
                            }
                            row[column] = new_value;
#if DEBUG
                            replace_count++;
                            characters_count += new_value.Length;
#endif
                        }
                    }
                }
                if (changed)
                {
                    row.EndEdit();
                    row.AcceptChanges();
                }
            }
#if DEBUG
            sw.Stop();
            // Debug.WriteLine("VDataTable.InternDataAsNeeded(): интернировано " + replace_count.ToString() + " значений (" + characters_count.ToString() + " символов) в " + columns.Count.ToString() + " колонках и " + rows.Count.ToString() + " строках за " + sw.ElapsedTicks.ToString() + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
#endif
        }
        private void ClientLoadComplete()
        {
            PrepareMerge();


            if (this.Rows.Count > 0)
            {
                if (this.currentRow == null)
                {
                    if (Grid == null || !Grid.IsTree())
                    {
                        this.CurrentRow = this.Rows[0];
                    }

                }
            }
            else
            {
                this.CurrentRow = null;
            }
        }
        private void RestoreCommands(string buffer, string procBuffer)
        {
            DataAdapter.SelectCommand.CommandText = buffer;
            if (ProcedureCommand != null)
            {
                ProcedureCommand.CommandText = procBuffer;
            }

            ClientCalculations();

            this._has_user_changes1 = false;
            _refreshed = true;
            if (TableRefreshed != null)
            {
                TableRefreshed(this, EventArgs.Empty);
            }
        }

        private bool _refreshed = false;

        public void ClientCalculations()
        {
            calculateTree();
            prepareToMerge();
        }

        private void prepareToMerge()
        {
            if (this.Scheme == null || Cmn.GetAttrValue(this.Scheme.Attribute(TextConst.AName.PrepareMerge)) != "1")
            {
                return;
            }


            var keysNames = Scheme.Elements(TextConst.EName.Columns).Elements().Attributes(TextConst.AName.CMasterKey).Select(e => e.Value).Distinct().ToList();

            var keyColsNames = Scheme.Elements(TextConst.EName.Columns).Elements().Where(e => keysNames.Contains(Cmn.GetAttrValue(e, TextConst.AName.CMasterKey))).Select(e1 => e1.Attribute(TextConst.AName.Name).Value).Distinct().ToList();
            int colI = 0;
            foreach (string keyName in keysNames)
            {
                var keyColName = keyColsNames[colI];
                var depColNames = Scheme.Elements(TextConst.EName.Columns).Elements().Where(e => Cmn.GetAttrValue(e, TextConst.AName.CMaster) == keyName).Select(e1 => e1.Attribute(TextConst.AName.Name).Value).ToList();



                var rows = this.AsEnumerable().OrderBy(r => r[keyColName].ToString()).ToList();

                object val = null;
                object prevKey = null;
                int i = 0;
                foreach (var colName in depColNames)
                {
                    var col = GetColumn(colName);
                    string aggType = Cmn.GetAttrValue(col.Scheme, "agg");
                    int firstIndex = 0;
                    prevKey = null;
                    i = 0;
                    foreach (var r in rows)
                    {
                        object curKey = r[keyColName];
                        if (!curKey.Equals(prevKey))
                        {
                            for (int i1 = i - 1; i1 >= firstIndex; i1--)
                            {
                                rows[i1][colName] = Cmn.Nvl(val, DBNull.Value);
                            }
                            val = null;
                            firstIndex = i;
                            prevKey = curKey;
                        }

                        if (aggType == TextConst.AVGroup.Sum)
                        {
                            object v1 = Cmn.Nvl(r[colName], null);
                            if (v1 != null)
                            {
                                if (val == null)
                                {
                                    val = v1;
                                }
                                else
                                {
                                    val = ((decimal)val) + ((decimal)v1);
                                }
                            }
                        }
                        else
                        {
                            if (val == null)
                            {
                                val = Cmn.Nvl(r[colName], null);
                            }
                        }
                        i++;

                    }
                }


                if (colI < keyColsNames.Count - 1)
                {
                    DataRow rowToRemove = null;
                    i = 0;
                    prevKey = null;
                    bool removed = false;
                    foreach (var r in rows)
                    {
                        object curKey = r[keyColName];
                        if (rowToRemove == null)
                        {
                            bool doRemove = true;
                            for (int colI1 = colI + 1; colI1 < keyColsNames.Count; colI1++)
                            {
                                if (Cmn.Nvl(r[keyColsNames[colI1]], null) != null)
                                {
                                    doRemove = false;
                                    break;
                                }
                            }
                            if (doRemove)
                            {
                                rowToRemove = r;
                            }
                        }

                        if (i > 0 && rowToRemove != null && !removed)
                        {
                            r.Delete();
                            r.AcceptChanges();

                        }


                        if (!curKey.Equals(prevKey))
                        {
                            i = 0;
                            removed = false;
                            rowToRemove = null;
                            prevKey = curKey;

                        }
                        i++;
                    }
                }

                colI++;


            }

            var a = "";
        }

        private void calculateTree()
        {
            if (this.Scheme == null || Cmn.GetAttrValue(this.Scheme.Attribute(TextConst.AName.CalculateTree)) != "1")
            {
                return;
            }

            foreach (VDataColumn col in this.Columns)
            {
                if (col.DataType == XmlReports.numberType)
                {
                    string aggType = Cmn.GetAttrValue(col.Scheme, "agg");
                    calculateTreeColumn(col.ColumnName, aggType);
                }
            }


        }

        private void calculateTreeColumn(string columnName, string aggType)
        {
            calculateTreeColumn(this, columnName, aggType);
        }

        public static void calculateTreeColumn(DataTable tbl, string columnName, string aggType, bool leafsOnly = false)
        {
            if (aggType == "no" || aggType == "")
            {
                return;
            }
            if (columnName == "lvl" || columnName == "rwn")
            {
                return;
            }

            SortedList<int, object> levelSums = new SortedList<int, object>();

            int prevLevel = 100000000;

            for (int i = tbl.Rows.Count - 1; i > -1; i--)
            {
                DataRow r = tbl.Rows[i];
                object val = null;
                int level = Convert.ToInt32(r["lvl"]);

                if (level == prevLevel - 1 && leafsOnly)
                {
                    val = null;
                }
                else
                {
                    switch (aggType)
                    {
                        case "sum":
                            if (Cmn.Nvl(r[columnName], null) != null)
                            {
                                val = Convert.ToDecimal(r[columnName]);
                            }
                            else
                            {
                                val = null;
                            }

                            break;
                        default:
                            val = r[columnName];
                            break;
                    }
                }





                if (level < prevLevel)
                {
                    if (!levelSums.ContainsKey(level + 1))
                    {
                        switch (aggType)
                        {
                            case "sum":
                                // levelSums.Add(level + 1, 0);
                                levelSums.Add(level + 1, null);
                                break;
                            default:
                                levelSums.Add(level + 1, null);
                                break;
                        }
                    }
                    switch (aggType)
                    {
                        case "sum":
                            if (val != null || levelSums[level + 1] != null)
                            {
                                val = Convert.ToDecimal(Cmn.Nvl(val, 0)) + Convert.ToDecimal(Cmn.Nvl(levelSums[level + 1], 0));
                            }
                            break;
                        case "max":
                            //val = Convert.ToDecimal(val) + Convert.ToDecimal(levelSums[level + 1]);
                            val = Cmn.IsGreater(val, levelSums[level + 1]) ? val : levelSums[level + 1];
                            break;
                        case "min":
                            //val = Convert.ToDecimal(val) + Convert.ToDecimal(levelSums[level + 1]);
                            val = Cmn.IsLess(val, levelSums[level + 1]) ? val : levelSums[level + 1];
                            break;
                        default:
                            break;
                    }

                    r[columnName] = Cmn.Nvl(val, DBNull.Value);
                    switch (aggType)
                    {
                        case "sum":
                            //levelSums[level + 1] = 0;
                            levelSums[level + 1] = null;
                            break;
                        default:
                            levelSums[level + 1] = null;
                            break;
                    }



                }
                else
                {
                    r[columnName] = Cmn.Nvl(val, DBNull.Value);
                }
                if (!levelSums.ContainsKey(level))
                {

                    switch (aggType)
                    {
                        case "sum":
                            //levelSums.Add(level, 0);
                            levelSums.Add(level, null);
                            break;
                        default:
                            levelSums.Add(level, null);
                            break;
                    }
                }


                switch (aggType)
                {
                    case "sum":
                        if (val != null || levelSums[level] != null)
                        {
                            levelSums[level] = Convert.ToDecimal(Cmn.Nvl(val, 0)) + Convert.ToDecimal(Cmn.Nvl(levelSums[level], 0));
                        }
                        // levelSums[level] = Convert.ToDecimal(levelSums[level]) + Convert.ToDecimal(val);
                        break;
                    case "max":
                        //val = Convert.ToDecimal(val) + Convert.ToDecimal(levelSums[level + 1]);
                        levelSums[level] = Cmn.IsGreater(val, levelSums[level]) ? val : levelSums[level];
                        break;
                    case "min":
                        //val = Convert.ToDecimal(val) + Convert.ToDecimal(levelSums[level + 1]);
                        levelSums[level] = Cmn.IsLess(val, levelSums[level]) ? val : levelSums[level];
                        break;
                    default:
                        break;
                }
                prevLevel = level;
            }
        }

        public int KeyCounter = -1;

        public SaveResult SaveWithChilds()
        {
            SaveResult result = Save();

            foreach (VDataTable tbl in GetChildTables())
            {
                if (!tbl.IsArrayEditValue)
                {
                    result.AppendResult(tbl.SaveWithChilds());
                }
            }

            return result;
        }

        public void SaveFiles(DataRow row)
        {
            foreach (VDataColumn col in Columns)
            {
                col.SaveFile(row);
            }
        }

        public void DeleteOldFiles(DataRow row)
        {
            foreach (VDataColumn col in Columns)
            {
                col.DeleteOldFile(row);
            }
        }

        //foreach (VDataTable tbl in ChildRelations.Cast<DataRelation>()
        //        .Select(r => r.ChildColumns.First().Table).Distinct())
        //    {
        //        if (!tbl.IsArrayEditValue)
        //        {
        //            result.AppendResult(tbl.SaveWithChilds());
        //        }
        //    }
        public SaveResult Save()
        {
            var result = save();

            foreach (VDataTable tbl in GetChildTables())
            {
                if (tbl.IsArrayEditValue)
                {
                    tbl.save();
                    tbl.ClearData();
                }
            }
            return result;
        }
        private SaveResult save()
        {
            //rememberSelection(); // не помогло , все равно соскакивает выделение

            var res = save1();
            //recollectSelection();
            return res;
        }

        public bool IsChangeAccepting = false;


        private bool InsteadSave(DataRow row)
        {

            if (ProcessEvent(TextConst.AVEventName.InsteadObjectSave, row))
            {
                return true;
            }
            return false;
        }
        private bool InsteadDelete(DataRow row)
        {

            if (ProcessEvent(TextConst.AVEventName.InsteadObjectDelete, row))
            {
                return true;
            }
            return false;
        }
        private SaveResult save1()
        {
            var result = new SaveResult();




            if (DataAdapter.UpdateCommand == null)
            {
                return result;
            }

            SuppressChangeEvent();

            DataAdapter.UpdateCommand.Connection = GetConnection();
            DataAdapter.InsertCommand.Connection = GetConnection();
            DataAdapter.DeleteCommand.Connection = GetConnection();

            bool currentRowCreated = false;

            List<DataRow> modifiedRows = new List<DataRow>();
            List<DataRow> newRows = new List<DataRow>();

            // проверка прав на запись
            bool allow_write = true;
            UIFormC form = (DataSet as VDataSet).Form;
            //if (form != null)
            //{
            //    string security_id = form.GetSecurityID();
            //    if (security_id != "") allow_write = VSecurityUtils.HasWritePermission(security_id);
            //}

            if (_manualDelete)
            {
                // чтобы не сробатывал обработчик события
                BeginManualDeleteIgnore();
                bool childsCleared = false;
                //if (_deletedRows.Any())
                //{
                //    ClearChildsData();
                //}
                foreach (var r in _deletedRows)
                {
                    if (r.RowState != DataRowState.Detached)
                    {
                        if (!childsCleared)
                        {
                            ClearChildsData();
                            childsCleared = true;
                        }
                        r.Delete();
                    }

                }
                EndManualDeleteIgnore();
            }

            foreach (var row in Rows.Cast<DataRow>())
            {
                if (row.RowState == DataRowState.Added)
                {
                    try
                    {
                        if (!allow_write)
                        {
                            result.AddRowException(row, null);
                            continue;
                        }

                        SaveFiles(row);
                        OracleParameter retPar = ApplyRowValuesToParams(row,
                            DataAdapter.InsertCommand.Parameters.Cast<OracleParameter>().ToList(), true);

                        if (!IsNonDb)
                        {
                            DevUtilsProvider.Instance.AnalyzeExecSql(DataAdapter.InsertCommand.CommandText);
                            DataAdapter.InsertCommand.ExecuteNonQuery();
                            row[PrimaryKey[0]] = retPar.Value;
                        }
                        else
                        {
                            raiseCustomRowSave_Added(row);
                        }


                        if ((DataSet as VDataSet).KeyParamName != null)
                        {
                            (DataSet as VDataSet).InputParams[(DataSet as VDataSet).KeyParamName].Value = retPar.Value;
                        }
                        row[TextConst.AVColumn.IsNew] = 0;
                        row[TextConst.AVColumn.IsNotNew] = 1;

                        if (row == currentRow)
                        {
                            currentRowCreated = true;
                        }
                        DeleteOldFiles(row);
                        modifiedRows.Add(row);
                        newRows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        //result.AddRowException(row, ex);
                        //if (!CustomOracleError.Codes.Contains(ex.Code))
                        //{
                        //    ResumeChangeEvent();
                        //    throw;
                        //}
                    }
                }
                else if (row.RowState == DataRowState.Modified)
                {
                    try
                    {
                        if (!allow_write)
                        {
                            result.AddRowException(row, null);
                            continue;
                        }
                        if (!InsteadSave(row))
                        {

                            SaveFiles(row);
                            ApplyRowValuesToParams(row,
                                DataAdapter.UpdateCommand.Parameters.Cast<OracleParameter>().ToList(), true);

                            if (!IsNonDb)
                            {

                                DataAdapter.UpdateCommand.CommandText = DataAdapter.UpdateCommand.CommandText.Replace("\r", " ");
                                DevUtilsProvider.Instance.AnalyzeExecSql(DataAdapter.UpdateCommand.CommandText);
                                DataAdapter.UpdateCommand.ExecuteNonQuery();
                            }
                            else
                            {
                                raiseCustomRowSave_Modified(row);
                            }
                            DeleteOldFiles(row);
                        }
                        modifiedRows.Add(row);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        //result.AddRowException(row, ex);
                        //if (!CustomOracleError.Codes.Contains(ex.Code))
                        //{
                        //    ResumeChangeEvent();
                        //    throw;
                        //}
                    }
                }
                else if (row.RowState == DataRowState.Deleted)
                {
                    try
                    {
                        if (!allow_write)
                        {
                            result.AddRowException(row, null);
                            continue;
                        }
                        if (!InsteadDelete(row))
                        {
                            ApplyRowValuesToParams(row,
                                DataAdapter.DeleteCommand.Parameters.Cast<OracleParameter>().ToList(), true);
                            if (!IsNonDb)
                            {
                                DevUtilsProvider.Instance.AnalyzeExecSql(DataAdapter.DeleteCommand.CommandText);
                                DataAdapter.DeleteCommand.ExecuteNonQuery();
                            }
                            else
                            {
                                raiseCustomRowSave_Deleted(row);
                            }
                            DeleteOldFiles(row); // !!! Не будет работать - доделать
                        }
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                        //result.AddRowException(row, ex);
                        //if (!CustomOracleError.Codes.Contains(ex.Code))
                        //{
                        //    ResumeChangeEvent();
                        //    throw;
                        //}
                    }
                }
            }

            ResetValidation();
            if (result.Success)
            {
                GetDataSet().SetVariableValue(TableName + TextConst.AVParam.HasChanges, 0, false);
            }
            ResumeChangeEvent();

            List<DataRow> rowsToRefresh = new List<DataRow>();
            if (SelectedRows != null)
            {
                foreach (var row in SelectedRows.Where(r => !result.RowsExceptions.ContainsKey(r)))
                {
                    if (GetCanBeChecked(row))
                    {
                        RaiseUIEvent(TextConst.AVEventName.CheckedRowSave, row, null);
                    }

                }
            }

            // Емцов - заглушка для создания исков в исп. производстве
            // при синхронизации ur_dogplat должен видеть новые строки с исками из ur_mat
            if (result.Success &&
                (TableName == "ur_mat" && KeyDimension == "kod_mat_isp")
             || (TableName == "ur_kazn" && KeyDimension == "kod_kazn")
             || (TableName == "ur_inkasso" && KeyDimension == "kod_inkasso"))
            {
                ((OracleConnection)GetConnection()).Commit();
            }

            bool refreshAllNewRows = false;
            foreach (var name in TextConst.AVColumnArray.SysColNamesForEditedObject)
            {
                var col = GetColumn(name);
                if (col != null)
                {
                    if (col.Dependants != null)
                    {
                        if (col.Dependants.Count != 0)
                        {
                            refreshAllNewRows = true;// чтобы обновлялись данные зависимые от is_new , is_not_new
                            break;
                        }
                    }
                }
            }
            //event row-save
            foreach (var row in newRows.Where(r => !result.RowsExceptions.ContainsKey(r)))
            {
                var wasEventProcessing = RaiseUIEvent(TextConst.AVEventName.RowSave, row, null);
                if (wasEventProcessing || refreshAllNewRows)
                {
                    if (!rowsToRefresh.Contains(row))
                    {
                        rowsToRefresh.Add(row);
                    }
                }
            }
            foreach (var row in modifiedRows.Where(r => !result.RowsExceptions.ContainsKey(r)))
            {
                var wasEventProcessing = RaiseUIEvent(TextConst.AVEventName.RowSave, row, null);
                if (wasEventProcessing)
                {
                    if (!rowsToRefresh.Contains(row))
                    {
                        rowsToRefresh.Add(row);
                    }
                }
            }
            //event new-row-save
            foreach (var row in newRows.Where(r => !result.RowsExceptions.ContainsKey(r)))
            {
                var wasEventProcessing = RaiseUIEvent(TextConst.AVEventName.NewRowSave, row, null);
                if (wasEventProcessing || refreshAllNewRows)
                {
                    if (!rowsToRefresh.Contains(row))
                    {
                        rowsToRefresh.Add(row);
                    }
                }
            }

            foreach (var row in modifiedRows.Where(r => !result.RowsExceptions.ContainsKey(r)))
            {
                if (ProcessEvent(TextConst.AVEventName.ObjectSave, row))
                {
                    if (!rowsToRefresh.Contains(row))
                    {
                        rowsToRefresh.Add(row);
                    }
                }
                if (ExtensionKeys != null && ExtensionKeys.Any())
                {
                    if (!rowsToRefresh.Contains(row))
                    {
                        rowsToRefresh.Add(row);
                    }
                }
            }

            //if (currentRowCreated)
            //{
            //    RaiseCurrentRowChanged();
            //}

            if (!(DataSet as VDataSet).IsVertica) ((OracleConnection)GetConnection()).Commit();
            if (result.Success)
            {
                IsChangeAccepting = true;
                AcceptChanges();
                IsChangeAccepting = false;
            }
            else
            {
                foreach (var r in this.AsEnumerable().Where(r => !result.RowsExceptions.ContainsKey(r)).ToArray()) r.AcceptChanges();
            }

            // емцов - переместил, чтобы на событие CurrentRowChanged у сохраненных строк уже изменялся статус на Unchanged
            if (currentRowCreated)
            {
                RaiseCurrentRowChanged();
            }

            CrearTemp();
            //foreach (DataRow row in rowsToRefresh.Where(r => !error_rows.ContainsKey(r)))
            //{
            EnqueueBackgroundRefresh(rowsToRefresh.Where(r => !result.RowsExceptions.ContainsKey(r)).ToArray());
            //}
            RaiseUIEvent(TextConst.AVEventName.Save, null, null);

            if (result.Success)
            {
                this._has_user_changes1 = false;
                if (TableCommited != null)
                {
                    TableCommited(this, EventArgs.Empty);
                }
            }

            if (modifiedRows.Any(r => !result.RowsExceptions.ContainsKey(r)))
            {
                RefreshParents();
            }

            return result;
        }

        public VDataSet.ValidationResult CheckValidation()
        {
            var result = new VDataSet.ValidationResult();
            if (InvalidRows == null) return result;

            // Емцов - игнорируем ошибки в удаляемых строках
            var rows = InvalidRows.Where(p => _deletedRows == null || !_deletedRows.Contains(p.Value)).ToArray();
            foreach (var row in rows)
            {
                var rowResult = GetRowErrorText(row.Value);
                result.Error = rowResult.Error;
                if (result.Error != "")
                {
                    return result;
                }
                else
                {
                    foreach (var s in rowResult.Warning)
                    {
                        result.Warning.Add(s);
                    }
                }
            }
            return result;
        }


        public OracleParameter ApplyRowValuesToParams(DataRow row, List<OracleParameter> pars, bool nullKeyForNewRows)
        {
            OracleParameter retParam = null;
            foreach (OracleParameter par in pars)
            {
                if (row.RowState == DataRowState.Added && par.SourceColumn == row.Table.PrimaryKey[0].ColumnName && nullKeyForNewRows)
                {

                    par.Value = DBNull.Value;

                }
                else
                {
                    if (row.RowState == DataRowState.Deleted)
                    {
                        par.Value = row[par.SourceColumn, DataRowVersion.Original];
                    }
                    // емцов - падало при сохранении
                    else if (row.RowState != DataRowState.Detached)
                    {
                        par.Value = row[par.SourceColumn];
                    }
                }

                if (par.Direction == ParameterDirection.InputOutput)
                {
                    retParam = par;
                }
            }
            return retParam;



        }

        public OracleConnection GetConnection()
        {
            return ((VDataSet)this.DataSet).GetConnection();
        }

        public void ClearColumns()
        {
            this.PrimaryKey = null;
            this.Columns.Clear();
        }

        public object HtmlControl = null;
        #region Динамические свойства поля для создания интерфейса
        public delegate VFieldStateAndOtherInfo DFieldInfoForTableCell(VDataColumn column, DataRow row, VFieldInfo.InfoTypesToGet[] getWhat);
        public DFieldInfoForTableCell CustomFieldInfoProc = null;


        #endregion
        #region DeleteState
        bool _manualDelete = false;
        int _manualDeleteIgnoreFlag = 0;

        public bool IsManualDeleteIgnore()
        {
            return _manualDeleteIgnoreFlag > 0;
        }

        void BeginManualDeleteIgnore()
        {
            _manualDeleteIgnoreFlag++;

            foreach (var t in GetChildTables()) t.BeginManualDeleteIgnore();
        }

        void EndManualDeleteIgnore()
        {
            _manualDeleteIgnoreFlag--;

            foreach (var t in GetChildTables()) t.EndManualDeleteIgnore();
        }

        List<DataRow> _deletedRows;
        private void InitManualDelete()
        {
            // повторно не инициализируем
            if (_deletedRows != null) return;

            _manualDelete = true;
            _deletedRows = new List<DataRow>();
            this.RowDeleting += table_OnRowDeleting;

        }
        private void table_OnRowDeleting(object sender, DataRowChangeEventArgs args)
        {
            if (IsChangeEventSuppressed()) return;
            if (_manualDeleteIgnoreFlag > 0) return;
            if (this.UpdateTempCommand != null)
            {
                AddRowToUpdateTemp(GetRowId(args.Row));
                UpdateTempRow(args.Row, true);
            }
            if (args.Row.RowState != DataRowState.Added)
            {

                DeleteRow(args.Row);

                args.Row.RejectChanges();
            }


        }

        public void DeleteRow(DataRow row)
        {
            if (!_manualDelete) return;

            if (row.RowState == DataRowState.Added)
            {
                row.Delete();
            }
            else if (!_deletedRows.Contains(row))
            {
                _deletedRows.Add(row);
            }

            ManualUserChangedData();


        }
        public void RestoreRow(DataRow row)
        {
            if (!_manualDelete) return;

            if (_deletedRows.Contains(row))
            {
                _deletedRows.Remove(row);
            }
        }

        public bool IsRowDeleted(DataRow row)
        {
            if (!_manualDelete) return false;

            return _deletedRows.Contains(row);
        }
        #endregion

        #region Async
        private AsyncLoadInfo executedTaskInfo;
        private AsyncLoadInfo queuedTaskInfo;
        private const int SLEEP_TIME = 100;
        public void AsyncExecuteReader(OracleCommand command)
        {
            this.CancelAsyncExecuteReader(); // отмена предыдущего асинхронного чтения
            this.queuedTaskInfo = new AsyncLoadInfo(this, command);
            this.StartQueuedTask();
        }
        public void CancelAsyncExecuteReader()
        {
            if (this.executedTaskInfo != null)
            {
                this.executedTaskInfo.Cancel();
                this.executedTaskInfo = null;
                Cmn.RaiseEvent(ref this.AsyncLoadCanceled, this, EventArgs.Empty);
            }
        }
        public event EventHandler<EventArgs> AsyncLoadStart;
        public event EventHandler<EventArgs> AsyncLoadComplete;
        public event EventHandler<EventArgs> AsyncLoadCanceled;
        /// <summary>
        /// Этот метод вызывается в контексте главной нити при завершении операции асинхронного чтения
        /// Если <paramref name="data"/> равно null, то операция была отменена
        /// </summary>
        /// <param name="data"></param>
        /// <param name="reader"></param>
        private void LoadAsyncData(DataTable data, IDataReader reader)
        {
            if (reader != null)
            {
                if (this.otherReader != null)
                {
                    this.otherReader.Close();
                }
                this.otherReader = reader;
                this.fetchedRowsCount = this.defaultFetch; // = data.Rows.Count;
            }
            if (data != null)
            {
                try
                {
                    this.SuppressChangeEvent();
                    this.Merge(data, false, MissingSchemaAction.Ignore);
                }
                finally
                {
                    this.ResumeChangeEvent();
                }
                Cmn.DisposeAndSetNull(ref data);
                this.ClientLoadComplete();
                Cmn.RaiseEvent(ref this.AsyncLoadComplete, this, EventArgs.Empty);
            }
            this.executedTaskInfo = null;
            this.StartQueuedTask();
        }
        private void StartQueuedTask()
        {
            if (this.executedTaskInfo == null && this.queuedTaskInfo != null)
            {
                this.executedTaskInfo = this.queuedTaskInfo;
                this.queuedTaskInfo = null;
                Cmn.RaiseEvent(ref this.AsyncLoadStart, this, EventArgs.Empty);
                this.executedTaskInfo.Start();
                // асинхронное чтение данных
                // CancellationToken token = executedTaskInfo.Token;
                // Task.Factory.StartNew(() => this.ReadDataAsync(executedTaskInfo), token, TaskCreationOptions.LongRunning, TaskScheduler.Default)
                // заполнение данными в главном потоке при удачном чтении
                //    .ContinueWith(this.ReadDataComplete, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        /// <summary>
        /// Класс операции асинхронного чтения
        /// </summary>
        private class AsyncLoadInfo
        {
            private VDataTable parent;
            private CancellationTokenSource cancellation;
            private OracleCommand command;
            private int rows_to_fetch;
            private IDataReader reader;
            private Task<DataTable> task;
            public AsyncLoadInfo(VDataTable parent, OracleCommand сommand)
            {
                this.parent = parent;
                this.cancellation = new CancellationTokenSource();
                this.command = сommand;
                if (parent.IsDeferredFetch())
                {
                    this.rows_to_fetch = parent.defaultFetch;
                }
                else
                {
                    this.rows_to_fetch = int.MaxValue;
                }
                this.reader = null;
                this.task = null;
            }
            /// <summary>
            /// Отмена операции асинхронного чтения
            /// </summary>
            public void Cancel()
            {
                this.cancellation.Cancel();
                //this.command.Cancel();
                if (this.task != null)
                {
                    while (!(this.task.IsCompleted || this.task.IsCanceled || this.task.IsFaulted))
                    {
                        this.task.Wait(VDataTable.SLEEP_TIME);
                        //Thread.Sleep(VDataTable.SLEEP_TIME);
                    }
                    this.task = null;
                }
            }
            /// <summary>
            /// Выполняется в пуле планировщика потоков
            /// </summary>
            /// <returns>Выбранные данные или null, если задание было отменено</returns>
            private DataTable ReadDataAsync()
            {
                CancellationToken token = this.cancellation.Token;
                // Ожидание, когда соединение освободится
                while (this.command.Connection.State == ConnectionState.Executing)
                {
                    Thread.Sleep(VDataTable.SLEEP_TIME);
                    if (token.IsCancellationRequested)
                    {
                        return null;
                    }
                }
                DataTable dt = null;
                try
                {
                    // 1. Выполнение запроса
                    DevUtilsProvider.Instance.AnalyzeExecSql(this.command.CommandText);
                    IAsyncResult result = this.command.BeginExecuteReader(CommandBehavior.SingleResult);
                    while (!result.IsCompleted)
                    {
                        token.ThrowIfCancellationRequested();
                        Thread.Sleep(VDataTable.SLEEP_TIME);
                    }
                    this.reader = this.command.EndExecuteReader(result);
                    token.ThrowIfCancellationRequested();
                    result = null;
                    // 2. Создание DataTable
                    dt = new DataTable();
                    int field_count = reader.FieldCount;
                    for (int field = 0; field < field_count; field++)
                    {
                        string field_name = reader.GetName(field);
                        Type data_type = reader.GetFieldType(field);
                        dt.Columns.Add(field_name, data_type);
                        //DataColumn col = this.parent.Columns[field_name];
                        //Contract.Assume(col != null);
                        //Contract.Assume(col.DataType == data_type);
                        //DataColumn dest_col = dt.Columns.Add(field_name, data_type);
                        //dest_col.Caption = col.Caption;
                    }
                    token.ThrowIfCancellationRequested();
                    // 3. Fetch данных
                    object[] values = new object[field_count];
                    dt.BeginLoadData();
                    for (int row = 0; row <= this.rows_to_fetch; row++)
                    {
                        token.ThrowIfCancellationRequested();
                        if (!reader.Read())
                        {
                            break;
                        }
                        reader.GetValues(values);
                        DataRow r = dt.Rows.Add(values);
                        r.AcceptChanges();
                    }
                    dt.EndLoadData();
                    token.ThrowIfCancellationRequested();
                }
                catch (OperationCanceledException)
                {
                    if (dt != null)
                    {
                        Cmn.DisposeAndSetNull(ref dt);
                    }
                    if (this.reader != null)
                    {
                        Cmn.DisposeAndSetNull(ref this.reader);
                    }
                    Cmn.DisposeAndSetNull(ref this.command);
                }
                catch (Exception ex)
                {
                    throw ex;
                    //if (ex.Code == 1013) { // ORA-01013: user requested cancel of current operation
                    //    return null;
                    //} else {
                    //    throw new infoenergo.core.Data.OracleSqlException(ex, this.command.CommandText);
                    //}
                }
                finally
                {
                    if (this.reader != null && this.rows_to_fetch == int.MaxValue)
                    {
                        this.reader.Close();
                        this.reader = null;
                        Cmn.DisposeAndSetNull(ref this.command);
                    }
                }
                return dt;
            }
            /// <summary>
            /// Этот метод вызывается в контексте главной нити при завершении операции асинхронного чтения
            /// </summary>
            private void ReadDataComplete(Task<DataTable> task)
            {
                AggregateException e = task.Exception;
                if (e != null)
                {
                    foreach (Exception inE in e.InnerExceptions)
                    {
                        //Program.ProcessUnhandledException(inE);
                        //throw inE;// почему то так не обрабатывается
                    }
                }
                this.parent.LoadAsyncData(task.Result, this.reader);
                this.task = null;
            }
            /// <summary>
            /// Запускает операцию асинхронного чтения
            /// </summary>
            public void Start()
            {
                this.task = Task.Factory.StartNew<DataTable>(this.ReadDataAsync, this.cancellation.Token, TaskCreationOptions.LongRunning, TaskScheduler.Default);
                this.task.ContinueWith(this.ReadDataComplete, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }
        #endregion
    }
}