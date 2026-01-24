using System.Collections.Generic;
using System.Data;
using Devart.Data.Oracle;
using System;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.Linq;
using System.ComponentModel;
using System.Threading;
////using System.Windows.Forms;
namespace sql.builder.DataApi
{
    internal partial class VDataColumn
    {
        public bool ParamUsed {
            get {
                if (!HasBoundControl(this)) {
                    return false;
                } else {
                    return this._bound_controls[0].GetUsed();
                }
            }
        }

       
        public VDBSelectCommand ValueRefreshCommand = null;
        public VDBSelectCommand ValueResetCommand = null;
        public SortedList<string,VDBSelectCommand> DependantsRefreshCommands = null;

        public void AddDependantsRefreshCommand(string tableName,VDBSelectCommand cmd)
        {
            if (DependantsRefreshCommands == null)
            {
                DependantsRefreshCommands = new SortedList<string, VDBSelectCommand>();
            }
            DependantsRefreshCommands.Add(tableName,cmd);
        }
        //public VDBSelectCommand DefaultValueCommand = null;
        public bool HasCellEvents = false;// Для создания линков при экспорте в excel
        public bool IsEmptyEvent = false;
        //public bool HasExtraButtons = false;
        public List<VDataColumn> Dependants = null;
        public bool HasAdditionalButtons()
        {
            if (!HasBoundControl(this)) {
                return false;
            } else {
                return this._bound_controls[0].HasAdditionalButtons();
            }
        }
        private string _mergeKey = null;
        public string MergeKey
        {
            get
            {
                return _mergeKey;
            }
            set
            {
                _mergeKey = value;
                if (!string.IsNullOrEmpty(_mergeKey))
                {
                    GetTable().AllowMerge = true;
                }
            }
        }


        public bool IsMerged(DataRow row)
        {
            return !GetTable().IsNotMerged(this, row);
        }
       
        public bool IsMerged(DataRow row1, DataRow row2)
        {
            if (row1 == null || row2 == null) return false;
            //if (this.ColumnName == "ur_dp_sum_all")
            //{
            //}

            var tbl = GetTable();
            var val1 = tbl.GetMergeKeyValue(this, row1);
            var val2 = tbl.GetMergeKeyValue(this, row2);
            if (val1 == null || val2 == null)
            {
                return false;
            }
            return val1.Equals(val2);
           
        }


       

        public string VariableName = null;
        public void AddDependant(VDataColumn col)
        {
            if (col.Table != this.Table && !this.GetTable().NewRowsVisForOtherTbls) return;
            if (Dependants == null)
            {
                Dependants = new List<VDataColumn>();
            }

           
           
            Dependants.Add(col);
        }

        public bool IsUpdateable = false;

        public string ColumnEditableSource = null;
        public string ColumnMandatorySource = null;
        public string ColumnVisibleSource = null;
        public string ColumnExistsSource = null;
        public string ColumnDefaultSource = null;


        public string EditableSource = null;
        public string VisibleSource = null;
        public string ExistsSource = null;
        public string MandatorySource = null;
        public string ValidSource = null;
        public string DefaultSource = null;
        public string TextSourceSource = null;
        public string NewValSource = null;
        public string BackColorSource = null;
        public string FontColorSource = null;
      


        public bool EditableInvert = false;
        public bool VisibleInvert = false;
        public bool ExistsInvert = false;
        public bool MandatoryInvert = false;
        public bool ValidInvert = false;


       
        public string ClientEditableSource =null;
        public string ClientMandatorySource = null;
        public string ClientDefaultSource = null;
        public string ClientVisibleSource = null;
        private string _clientValidSource = null;
        public string ClientValidSource
        {
            get
            {
                return _clientValidSource;
            }
            set
            {
                _clientValidSource = value;
                if (_clientValidSource == "null")// чтобы убирать валидацию где она не нужна 
                {
                    _clientValidSource = null;
                    
                }
            }
            
        }
        public string ClientExistsSource = null;
        public string ClientNewValSource = null;
        public string ClientFontColorSource = null;
        public string ClientBackColorSource = null;
        public string ClientSourceSource = null;

        public List<VDataColumn> DependantsEditable = null;
        public List<VDataColumn> DependantsMandatory = null;
        public List<VDataColumn> DependantsVisible = null;
        public List<VDataColumn> DependantsExists = null;
        public List<VDataColumn> DependantsDefault = null;
        public List<VDataColumn> DependantsValid = null;
        public List<VDataColumn> DependantsSelList = null;
        public List<VDataColumn> DependantsTextSource = null;
        public List<VDataColumn> DependantsNewVal = null;
        public List<VDataColumn> DependantsFontColor = null;
        public List<VDataColumn> DependantsBackColor = null;

        public string GetTextSourceName()
        {
            if (TextSourceSource != null)
            {
                return TextSourceSource;
            }
            else
            {
                return ColumnName;
            }
            
        }

        public void AddDependantProp(VDataColumn col, string propName)
        {
            var lPropName = TextConst.Pfx.BehaviorDependants + propName;

            var list = (Cmn.GetProperty(this, lPropName) as List<VDataColumn>);
            if (list == null)
            {
                list = new List<VDataColumn>();
                Cmn.SetProperty(this, lPropName, list);
                (Table as VDataTable).AttachBehaviorEvent();
            }
            list.Add(col);
        }
        public void AddDependantSelList(VDataColumn col)
        {
            if (DependantsSelList == null)
            {
                DependantsSelList = new List<VDataColumn>();
                (Table as VDataTable).AttachBehaviorEvent();
            }
            DependantsSelList.Add(col);
        }


        public void AddDependantText(VDataColumn col)
        {
            if (DependantsTextSource == null)
            {
                DependantsTextSource = new List<VDataColumn>();
                (Table as VDataTable).AttachBehaviorEvent();
            }
            DependantsTextSource.Add(col);
        }
        private int columnElitable = -1;

        private bool GetClientSourceBoolValue(DataRow row,string variableName,bool invert)
        {
            object editable = null;
            object val = null;
            if (variableName == "1")
            {
                val = 1;
            }
            else if (variableName == "0")
            {
                val = 0;
            }
            else
            {
                var srcCol = GetTable().GetDataSet().GetVariableColumn(variableName);


                if (srcCol != null)
                {

                    DataRow srcRow = null;
                    if (this.Table == srcCol.Table)
                    {
                        srcRow = row;
                    }
                    else
                    {
                        srcRow = (srcCol.Table as VDataTable).CurrentRow;
                    }
                    if (srcRow != null)
                    {
                        val = srcRow[srcCol];
                    }

                }
                else
                {
                    val = GetTable().GetDataSet().InputParams[variableName].Value;
                }
            }
            columnElitable = Convert.ToInt32(Cmn.Nvl(val, 0));
            if (columnElitable == 0)
            {
                editable = invert;
            }
            else
            {
                editable = !invert;
            }

            return (bool)editable;
        }

        public bool GetEditable(DataRow row)
        {
            object editable = null;
            
            if (row.RowState == DataRowState.Deleted) return false;

            if (GetTable().IsRowAdded(row))
            {
                return false;
            }
            if (ClientEditableSource != null)
            {
                editable = GetClientSourceBoolValue(row, ClientEditableSource,EditableInvert);

            }
            if (editable == null)
            {
                if (ColumnEditableSource != null)
                {
                    if (ColumnEditableSource == TextConst.AVBool.False)
                    {
                        editable = EditableInvert;
                    }
                    else if (ColumnEditableSource == TextConst.AVBool.True)
                    {
                        editable = !EditableInvert;
                    }
                    else
                    {
                        if (columnElitable == -1)
                        {
                            var val = VQuery.ExecuteQueryReturnScalar(ColumnEditableSource);
                            columnElitable = Convert.ToInt32(Cmn.Nvl(val, 0));

                        }
                        if (columnElitable == 0)
                        {
                            editable = EditableInvert;
                        }
                        else
                        {
                            editable = !EditableInvert;
                        }
                    }
                }
            }
            if (editable == null)
            {
                var dataTable = (VDataTable)Table;
                if (dataTable.ColumnEditableSource != null)
                {
                    if (dataTable.ColumnEditableSource == TextConst.AVBool.False)
                    {
                        editable = false;
                    }
                    if (dataTable.ColumnEditableSource == TextConst.AVBool.True)
                    {
                        editable = true;
                    }
                }
            }

            if (editable == null)
            {
                if (EditableSource != null)
                {
                    if (Cmn.Nvl(row[EditableSource], null) == null)
                    {
                        editable = EditableInvert;
                    }
                    else
                    {
                        if (row[EditableSource].ToString() == TextConst.AVBool.False)
                        {
                            editable = EditableInvert;
                        }
                        else
                        {
                            editable = !EditableInvert;
                        }
                    }
                }
            }
            if (editable == null)
            {
                editable = true;
            }
            bool val1 = (bool)editable;


            return val1;
        }
        public string GetFontColor(DataRow row)
        {
            if (this.ClientFontColorSource != null) {
                VDataColumn srcCol = GetTable().GetDataSet().GetVariableColumn(ClientFontColorSource);
                return row[srcCol].ToString();
            } else if (this.FontColorSource != null) {
                return row[this.FontColorSource].ToString();
            } else {
                return null;
            }
        }
        public bool GetMandatory(DataRow row)
        {
            if (ClientMandatorySource != null)
            {


                return GetClientSourceBoolValue(row, ClientMandatorySource,MandatoryInvert);

            }

            if (ColumnMandatorySource != null)
            {
                if (ColumnMandatorySource == TextConst.AVBool.False)
                {
                    return false;
                }
                if (ColumnMandatorySource == TextConst.AVBool.True)
                {
                    return true;
                }
            }


            if (MandatorySource != null)
            {
                if (Cmn.Nvl(row[MandatorySource], null) == null)
                {
                    return false;
                }
                else
                {
                    if (row[MandatorySource].ToString() == TextConst.AVBool.False)
                    {
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            return false;
        }

        public void UpdateDidplayValue(DataRow row)
        {
            var e = GetExists(row);

            ChangeValueForDisplay(e, row);
        }
        public bool GetExists(DataRow row)
        {
            bool propVal = true;
            if (row != null)
            {
                if (row.RowState == DataRowState.Deleted)
                {
                    return false;
                }
            }


            if (ColumnExistsSource != null)
            {
                if (ColumnExistsSource == TextConst.AVBool.False)
                {
                    propVal = false;
                }
                if (ColumnExistsSource == TextConst.AVBool.True)
                {
                    propVal = true;
                }
            }
            else
            {


                object val = true;

                if (ClientExistsSource != null)
                {
                    var srcCol = GetTable().GetDataSet().GetVariableColumn(ClientExistsSource);
                    if (srcCol != null)
                    {
                        DataRow srcRow = srcCol.GetInOrCurrentRow(row);
                        
                       // 
                        if (srcRow != null)
                        {
                            val = srcRow[srcCol];
                        }
                    }
                    else
                    {
                        val = GetTable().GetDataSet().InputParams[ClientExistsSource].Value;
                    }

                }
                else
                {
                    if (ExistsSource != null)
                    {
                        if (row == null)
                        {
                            val = false;
                        }
                        else
                        {
                            val = row[ExistsSource];
                        }

                    }
                }

                if (Cmn.Nvl(val, null) == null)
                {
                    propVal = false;
                }
                else
                {
                    if (val.ToString() == TextConst.AVBool.False)
                    {
                        propVal = false;
                    }
                }
            }


            if (ExistsInvert)
            {
                propVal = !propVal;
            }
            return propVal;
        }

        public  DataRow GetInOrCurrentRow(DataRow row )
        {
            
            DataRow row1 = null;
            if (row !=null && row.Table == Table)
            {
                row1 = row;
            }
            else
            {
                row1 = GetTable().CurrentRow;
            }
            return row1;
        }



        public bool GetVisibility(DataRow row, bool useExists = true, bool staticOnly = false)
        {
            bool propVal = true;

            //if (this.ColumnName == "not_vvod")
            //{
            //}
            bool noVal = false;

            if (useExists && !GetExists(row) && !staticOnly)
            {
                propVal = false;
            }
            else
            {
                object val = true;

                if (ClientVisibleSource != null && !staticOnly)
                {
                    var srcCol = GetTable().GetDataSet().GetVariableColumn(ClientVisibleSource);
                    if (srcCol != null)
                    {
                        DataRow srcRow = null;
                        if (this.Table == srcCol.Table)
                        {
                            srcRow = row;
                        }
                        else
                        {
                            srcRow = (srcCol.Table as VDataTable).CurrentRow;
                        }
                        if (srcRow != null)
                        {
                            val = srcRow[srcCol];
                        }
                        else
                        {
                            val = DBNull.Value;
                        }
                    }
                    else
                    {
                        if (ClientVisibleSource == TextConst.AVBool.False) // заплатка для стыковки состарым вариантом
                        {
                            val = DBNull.Value;
                        }
                        else if (ClientVisibleSource == TextConst.AVBool.True)
                        {
                            val = ClientVisibleSource;
                        }
                        else
                        {
                            val = GetTable().GetDataSet().InputParams[ClientVisibleSource].Value;
                        }
                    }

                }
                else if (VisibleSource != null && !staticOnly)
                {
                    if (row == null)
                    {
                        val = true;
                    }
                    else
                    {
                        val = row[VisibleSource];
                    }
                }
                else if (ColumnVisibleSource != null)
                {
                    if (ColumnVisibleSource == TextConst.AVBool.False)
                    {
                        propVal = false;
                    }
                    else if (ColumnVisibleSource == TextConst.AVBool.True)
                    {
                        propVal = true;
                    }
                    else
                    {
                        val = VDBSelectCommand.GetQueryScalarResult(ColumnVisibleSource);

                    }
                }
                else
                {
                    noVal = true;
                }



                if (Cmn.Nvl(val, null) == null)
                {
                    propVal = false;
                }
                else
                {
                    if (val.ToString() == TextConst.AVBool.False)
                    {
                        propVal = false;
                    }
                }



                if (VisibleInvert && !noVal)
                {
                    propVal = !propVal;
                }
            }

            return propVal;
        }

        public string GetBackColor(DataRow row)
        {
            object val = null;
            if (ClientBackColorSource != null)
            {
                var srcCol = GetTable().GetDataSet().GetVariableColumn(ClientBackColorSource);

                val = row[srcCol];
            }
            else if (BackColorSource != null)
            {

                val = row[BackColorSource];

            }
           

            
            
            return (string)Cmn.Nvle(val, null);

        }

        
        public string GetValidation(DataRow row)
        {
            string val = "";

            if (GetTable().IsRowAdded(row))
            {
                val = "";
            }
            else
            {

                if (row.RowState == DataRowState.Deleted || row.RowState == DataRowState.Detached)
                {
                    val = "";
                }
                else
                {
                    
                    if (!GetExists(row))
                    {
                        val = "";
                    }


                    else if (GetMandatory(row) == true && (
                        Cmn.Nvl(row[this], null) == null
                        || (VDataColumn.HasBoundControl(this) && this._bound_controls[0] is UI.UICheck && Cmn.ToDecimal(row[this]) == 0m)
                        )
                        ) 
                    {
                        //else if (GetMandatory(row) == true && Cmn.Nvl(row[GetTextSourceName()], null) == null)//  не прогружается textsource вовремя
                    //{
                  

                        val = "Поле \"" + this.Caption + "\" должно быть заполнено";

                    }

                    else if (ClientValidSource != null)
                    {
                        var srcCol = GetTable().GetDataSet().GetVariableColumn(ClientValidSource);
                        var srcRow = srcCol.GetInOrCurrentRow(row);
                        if (srcRow != null)
                        {
                            val = srcRow[srcCol].ToString();
                        }
                        //if (!string.IsNullOrEmpty(val))
                        //{
                        //    return val;
                        //}
                    }

                    else if (ValidSource != null) //if (string.IsNullOrEmpty(val))
                    {

                        //if (ValidSource != null)
                        //{
                        if (Cmn.Nvl(row[ValidSource], null) == null)
                        {
                            val = "";
                        }
                        else
                        {
                            val = row[ValidSource].ToString();
                        }
                        //}
                    }
                }
            }

            GetTable().SetCellError(row, ColumnName, val);
            //if (string.IsNullOrEmpty(val))
            //{
            //    GetTable().RemoveInvalidFieldColumn(row, ColumnName);
            //}
            //else
            //{
            //    GetTable().AddInvalidFieldColumn(row, ColumnName);
            //}
            return val;
        }

        private static Semaphore semaphore1 = new Semaphore(1, 1);
       
       
   


        public static List<BackgroundWorker> backgroundWorkers = null;
        public class RefreshInfo //Бельченко: всякая химия с потоками , чтобы не завичал интерфейс при обновлении полей
        //, и при этом корректно обновлялись поля c циклической зависимостью
        // если поле B зависит от поля А, поле С от B , А от С обновление при изменении А:  A->B->C->стоп (А не обновится) 
        //т.к. А уже в списке полей изменение которых обрабатывается.
        {
            public RefreshInfo(VDataColumn col, DataRow row,bool isAsync)
            {
                Col = col;
                Row = row;
                IsAsync = isAsync;
            }
            public VDataColumn Col = null;
            public DataRow Row = null;
            public RefreshInfo Next = null;
            public Boolean IsAsync = false;
        }
        public void RaiseDataChangeForUI(DataRow row)
        {
            if (this._bound_controls != null) {
                for (int index = 0; index < this._bound_controls.Count; index++) {
                    this._bound_controls[index].ColumnChanged(row);
                }
            }
        }
        public void ProcessChanges(DataRow row)
        {
            var tbl = GetTable();
            var rid = tbl.GetRowId(row);

            if (this.TempColumnName != null)
            {
                tbl.AddRowToUpdateTemp(rid);
            }

            if (GetTable().IsChangeEventSuppressed()) return;
            if (GetTable().DontRefreshDependats) return;
            var column = this;

            


           // column.GetTable().GetDataSet().AddChangingColumn(column);

            if (column.TempColumnName != null)
            {
                //column.GetTable().UpdateTempRow(row);
            }



           
            var ds = tbl.GetDataSet();
            var changesRuning = false;
            var hasTextSource = false;
            if (column.TextSourceSource != null)
            {
                column.GetTable().UpdateTempRow(row);
                var textSourceCol = tbl.GetColumn(column.TextSourceSource);
                //var ri = new RefreshInfo(textSourceCol, row, false);
                textSourceCol.RefreshCalulatedValueNewSimple(row);
                hasTextSource = true;
            }

            var depOnlyText = false;
            if (hasTextSource && Dependants==null/*&&  Dependants.Count == 1*/)// textsource нет в dependance?
            {
                depOnlyText = true;
            }
            // return;

            if (!depOnlyText && column.DependantsRefreshCommands != null)
            {
                changesRuning = true;
                foreach (var DepRefrCmd in column.DependantsRefreshCommands)
                {
                    var depTbl = (VDataTable)ds.Tables[DepRefrCmd.Key];
                    if (depTbl == tbl)
                    {
                        depTbl.EnqueueBackgroundRefreshRow(row, DepRefrCmd.Value, column);
                    }
                    else
                    {
                        depTbl.EnqueueBackgroundRefresh(null, DepRefrCmd.Value, column);
                    }

                }
            }

            //if (!changesRuning)
            //{
            //    column.GetTable().GetDataSet().RemoveChangingColumn(column);
            //}
            
        }
        public static void DataTableColumnChanged(object sender, DataColumnChangeEventArgs e)
        {
            var column = (e.Column as VDataColumn);
            column.ProcessChanges(e.Row);
        }
        public void ChangeValueForDisplay(bool IsExists,DataRow row)
        {
            if (row == null)
            {
                return;
            }

           
            var row1 = row;
            bool newVal = false;
            object val = null;
            // !!! плохо проверенная часть, производительность?
            if (IsExists) {
                if (HasBoundControl(this) && this._bound_controls[0].IsBool()) {
                    decimal val1;
                    if (row[this] == DBNull.Value) {
                        newVal = true;
                        val1 = decimal.Zero;
                    } else {
                        val1 = (decimal)row[this];
                        if (val1 != decimal.Zero && val1 != decimal.One) {
                            newVal = true;
                            val1 = decimal.One;
                        }
                    }
                    val = val1;
                }
            } else if (row[this] != DBNull.Value) {
                val = DBNull.Value;
                newVal = true;
            }

            if (newVal)
            {
                var state = row.RowState;
                GetTable().SuppressChangeEvent();

                row[this] = val;
                if (state == DataRowState.Unchanged)
                {
                    row.AcceptChanges();
                }
                GetTable().ResumeChangeEvent();
            }
        }

        public void RefreshCalulatedValue(DataRow row, RefreshInfo ri,bool isAsync)
        {
            if (GetTable().CancelTempUpdate) return;
            var col = this;
            if (col.ValueRefreshCommand != null)
            {



                List<OracleParameter> pars;
                var tbl = GetTable();
                var ds = tbl.GetDataSet();

                if (ds.InputParams != null)
                {
                    pars = ds.InputParams.Values.Select(p => p).ToList();
                }
                else
                {
                    pars = new List<OracleParameter>();
                }
                BackgroundWorker bw = new BackgroundWorker();
                bw.WorkerSupportsCancellation = true;
                if (backgroundWorkers == null)
                {
                    backgroundWorkers = new List<BackgroundWorker>();
                }
                backgroundWorkers.Add(bw);

                if (isAsync)
                {

                    //BackgroundWorker bw = col.ValueRefreshCommand.GetBackgroundWorker(row[tbl.PrimaryKey[0]],ds);



                    bw.DoWork += new DoWorkEventHandler(
                    delegate(object o, DoWorkEventArgs args)
                    {
                        // mutex1.WaitOne();
                        semaphore1.WaitOne();


                        RefreshCalulatedValue_AddParams(pars, ds, tbl, row, col);



                        if (bw.CancellationPending == true)
                        {
                            args.Cancel = true;

                        }
                        else
                        {
                            args.Result = col.ValueRefreshCommand.ExecuteDataTable(pars.ToArray(), (OracleConnection)ds.GetConnection());
                        }

                        if (bw.CancellationPending == true)
                        {
                            args.Cancel = true;

                        }
                        //mutex1.ReleaseMutex();
                    });


                    bw.RunWorkerCompleted += new RunWorkerCompletedEventHandler(
                    delegate(object o, RunWorkerCompletedEventArgs args)
                    {

                        //mutex2.WaitOne();  //!!!Мьютексы, чтобы не зацикливалось, например, при изменении суммы гп. Все равно, сделано криво (
                        if (!args.Cancelled)
                        {
                            var valTbl = (DataTable)args.Result;
                            RefreshCalulatedValue_UseVal(valTbl, col, row);

                        }
                        else
                        {
                        }
                        RefreshCalulatedValue_Complete(bw, ri, col);

                        //mutex2.ReleaseMutex();
                        semaphore1.Release();
                    });

                    bw.RunWorkerAsync();


                }
                else
                {
                    RefreshCalulatedValue_AddParams(pars, ds, tbl, row, col);
                    var valTbl = col.ValueRefreshCommand.ExecuteDataTable(pars.ToArray(), (OracleConnection)ds.GetConnection());
                    RefreshCalulatedValue_UseVal(valTbl, col, row);
                    RefreshCalulatedValue_Complete(bw, ri, col);
                }




            }
        }
        internal void RefreshAllColumn()
        {
            VDataTable tbl = this.GetTable();
            if (this.ValueRefreshCommand == null) {
                throw new NullReferenceException();
            }
            tbl.EnqueueBackgroundRefresh(tbl.Rows.ToArray(), this.ValueRefreshCommand, null, null, false);
        }
        internal void ResetAllColumn()
        {
            VDataTable tbl = this.GetTable();
            if (this.ValueResetCommand == null) {
                throw new NullReferenceException();
            }
            tbl.EnqueueBackgroundRefresh(tbl.Rows.ToArray(), this.ValueResetCommand, null, null, false);
        }
        public void RefreshCalulatedValueNewSimple(DataRow row)
        {
            if (this.ValueRefreshCommand != null) {
                List<OracleParameter> pars;
                VDataTable tbl = this.GetTable();
                VDataSet ds = tbl.GetDataSet();
                if (ds.InputParams != null) {
                    pars = ds.InputParams.Values.ToList<OracleParameter>();
                } else {
                    pars = new List<OracleParameter>(0);
                }
                RefreshCalulatedValue_AddParams(pars, ds, tbl, row, this);
                var valTbl = this.ValueRefreshCommand.ExecuteDataTable(pars.ToArray(), (OracleConnection)ds.GetConnection());
                RefreshCalulatedValue_UseVal(valTbl, this, row);
            }
        }
        private void RefreshCalulatedValue_AddParams(List<OracleParameter> pars, VDataSet ds, VDataTable tbl, DataRow row, VDataColumn col)
        {
            pars.Add(VDBSelectCommand.CreateKeyDBParameter(tbl, row));
            pars.AddRange(VDBSelectCommand.CreateExtensionKeysDBParameters(tbl, row));
            pars.Add(VDBSelectCommand.TempRowIdParametr(tbl, row));
            pars.Add(VDBSelectCommand.CreateNewRowDBParameter(row));
            pars.AddRange(VDBSelectCommand.CreateForegnKeyDBParameter(tbl, row));
            pars.AddRange(col.ValueRefreshCommand.CreateCurValDBParameters(row));
            pars.Add(ds.CreateFormIdParametr());
        }
        private void RefreshCalulatedValue_UseVal(DataTable valTbl, VDataColumn col, DataRow row)
        {
            if (valTbl.Rows.Count == 1)
            {
                object val = valTbl.Rows[0][col.ColumnName];
                col.SetValue(row, val);
            }
            else
            {
                if (valTbl.Rows.Count > 1)
                {
                    throw new System.InvalidOperationException("Некрректно построен запрос для обновения поля "+col.Caption);
                               
                }
            }
        }
        private static void RefreshCalulatedValue_Complete(BackgroundWorker bw, RefreshInfo ri, VDataColumn col)
        {
            backgroundWorkers.Remove(bw);
            if (ri != null && ri.Next != null) {
                ri.Next.Col.RefreshCalulatedValue(ri.Next.Row, ri.Next, ri.IsAsync);
            } else if (backgroundWorkers.Count == 0) {
                col.GetTable().GetDataSet().RaiseChangeCompleted();
            }
        }
        public void ApplyDefaultValue(DataRow row)
        {
            //if (this.ColumnName == "nach")
            //{
            //}
            if (!this.GetExists(row))
            {
              //  Undo(row);
            }
            if (row == null || row.RowState==DataRowState.Deleted)
            {
                return;
            }
            if (Cmn.Nvl(row[this], null) != null) return;
            if (ClientDefaultSource != null)
            {
                var srcCol = GetTable().GetDataSet().GetVariableColumn(ClientDefaultSource);
                if (srcCol != null)
                {
                    var srcRow = (srcCol.Table as VDataTable).CurrentRow;
                    if (srcRow != null)
                    {
                        row[this] = srcRow[srcCol];
                    }
                }
                else
                {
                    var sVal = Cmn.Nvl( GetTable().GetDataSet().InputParams[ClientDefaultSource].Value,"").ToString();

                    if (sVal != Cmn.undefinedString)
                    {

                        row[this] = Cmn.Nvl( GetTable().GetDataSet().InputParams[ClientDefaultSource].Value,DBNull.Value);
                    }
                }
                return;
            }
            else if (DefaultSource != null)
            {
                if (Cmn.Nvl(row[DefaultSource], null) != null)
                {
                    SetValue(row, row[DefaultSource]);
                }
            }
            else if (ColumnDefaultSource != null)
            {
                var val = VQuery.ExecuteQueryReturnScalar(ColumnDefaultSource);
                SetValue(row, val);
               
            }



        }

        public void ApplyNewValue(DataRow row)
        {
            if (row != null && row.RowState != DataRowState.Deleted)
            {
                
                if (ClientNewValSource != null)
                {
                    if (this.GetTable().CurrentRow != null)
                    {
                        var val = this.GetTable().GetDataSet().GetVariableValue(ClientNewValSource);
                        SetValue(this.GetTable().CurrentRow, val);
                    }
                }
                else if (NewValSource != null)
                {
                    if (Cmn.Nvl(row[NewValSource], null) != null)
                    {
                        SetValue(row, row[NewValSource]);
                    }
                }
            }



        }
    }
}
