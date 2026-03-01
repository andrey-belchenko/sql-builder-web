using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using Devart.Data.Oracle;
//using infoenergo.core.Extensions;
//using infoenergo.ui.win;
using sql.builder.asuse;
using sql.builder.Clean;
using sql.builder.Core;
//using sql.builder.Test;
using sql.builder.UI;
using sql.builder.XmlHelpers;
//using Vertica.Data.VerticaClient;
using SqlBuilderLib.DevTools;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.DataApi
{
    public partial class VDataSet : DataSet
    {
        private bool _refreshed;
        private XElement _scheme;
        private List<VDataTable> _top_tables;
        public XElement Scheme
        {
            get
            {
                return this._scheme;
            }
            set
            {
                this._scheme = value;
            }
        }
        public VReport Report;
        public bool UseTempTable;
        public bool UpdateTempTable;
        public string ProcedureText;
        public VDataTable ParamsTable;
        public List<string> MatQueriesNames;
        public IList<VDataTable> TopTable { get { return this._top_tables; } }
        public VDataTable ParentDataTable;
        public VDataSet ParentDataSet;
        // для web отчетов
        public VDataSet ParamsDataSet = null;
        public string ParentColumnName;
        public UIFormC Form;
        public bool IsVisibleInLayout()
        {
            if (this.Form == null)
            {
                return false;
            }
            else
            {
                return this.Form.IsVisibleInLayout();
            }
        }
        //private SortedList<string, VDataColumn> columns;
        private SortedList<string, XElement> formInfoFieldsTextNodes;  // инициализируется в методе GetParamsAsXml()
        public XmlDocument CompiledReport;
        public VDataColumn OwnerColumn;
        public Dictionary<string, VOracleParameter> InputParams;
        public SortedList<string, object> InputParamsValues;
        public string KeyParamName;
        public SortedList<int, string> InputParamsNames;
        public bool IsRefreshed { get { return this._refreshed; } }
        //    private OracleTransaction transaction;
        static readonly char _num_separator = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
        public VXElement schemePreset;
        public VXElement SchemePreset
        {
            get
            {
                if (this.schemePreset == null)
                {
                    this.schemePreset = new VXElement(Report.Scheme);
                }
                return this.schemePreset;
            }
            set
            {
                this.schemePreset = value;
            }
        }
        public FCustomRefresh CustomRefresh;
        public bool IsVertica { get; set; }
        public VOracleConnection Connection = null;
        public event EventHandler Changed;
        public event EventHandler SchemeChanged; // вызывается вручную , для обновления списка с изменением состава колонок см. VConst
        public event EventHandler TopTableRefreshed;
        public event EventHandler TopTableCommited;
        public event EventHandler NeedSelection;
        public void AddTopTable(VDataTable table)
        {
            if (this._top_tables == null)
            {
                this._top_tables = new List<VDataTable>(1);
            }
            this._top_tables.Add(table);
        }
        public void RefreshTopTableIfClear()
        {
            if (this.isClear)
            {
                this.RefreshTopTable(false);
            }
        }
        private bool _refreshing;
        public bool IsRefreshing()
        {
            return this._refreshing;
        }
        public void RefreshTopTable(bool isCreation)
        {
            this._refreshing = true;
            if (!isCreation)
            {
                this.ClearData();
            }
            this.isClear = false;
            if (this.ParentDataTable != null)
            {
                List<object> pars = new List<object>();
                if (this.ParentDataTable.CurrentRow != null)
                {
                    pars.Add(ParentDataTable.CurrentRow[ParentColumnName]);
                }
                else
                {
                    pars.Add(null);
                }
                this.SetParamsValues(pars);
            }
            if (this._top_tables != null)
            {
                for (int index = 0; index < this._top_tables.Count; index++)
                {
                    VDataTable table = this._top_tables[index];
                    if (!isCreation || index > 0)
                    {
                        table.Refresh();
                        if (table.SelectedRows.Count > 0)
                        {
                            table.RaiseCurrentRowChanged();
                        }
                        else if (table.Rows.Count > 0 && (table.Grid == null || !table.Grid.IsTree()))
                        {
                            table.CurrentRow = table.Rows[0];
                        }
                        else
                        {
                            table.RaiseCurrentRowChanged();
                        }
                    }
                }
            }
            if (this.TopTableRefreshed != null)
            {
                this.TopTableRefreshed(this, EventArgs.Empty);
            }
            for (int index = 0; index < this.Tables.Count; index++)
            {
                VDataTable tbl = (VDataTable)this.Tables[index];
                if (!string.IsNullOrEmpty(tbl.MultiselectSource()))
                {
                    tbl.SyncTargetSelection();
                }
            }
            this._refreshing = false;
        }
        public IList<VDataTable> MultiselectSourceTables()
        {
            var list = new List<VDataTable>();
            for (int index = 0; index < this.Tables.Count; index++)
            {
                VDataTable dt = (VDataTable)this.Tables[index];
                if (!string.IsNullOrEmpty(dt.MultiselectTargetName))
                {
                    list.Add(dt);
                }
            }
            return list;
        }
        public IList<VDataTable> MultiselectTargetTables()
        {
            var list = new List<VDataTable>();
            for (int index = 0; index < this.Tables.Count; index++)
            {
                VDataTable dt = (VDataTable)this.Tables[index];
                if (!string.IsNullOrEmpty(dt.MultiselectSource()))
                {
                    list.Add(dt);
                }
            }
            return list;
        }
        private bool isClear = true;
        public void ClearData()
        {
            if (this._top_tables != null)
            {
                for (int index = 0; index < this._top_tables.Count; index++)
                {
                    this._top_tables[index].ClearData();
                }
            }
            this.VariableColumnHasValue = null;
            this.isClear = true;
        }
        public SaveResult Save()
        {
            var result = new SaveResult();
            foreach (VDataTable table in this._top_tables)
            {
                result.AppendResult(table.SaveWithChilds());
            }
            if (result.Success && TopTableCommited != null)
            {
                TopTableCommited(this, EventArgs.Empty);
            }
            return result;
        }

        public class ValidationResult
        {
            public string Error = "";
            public List<string> Warning = new List<string>();
        }
        public ValidationResult Validate()
        {
            var result = new ValidationResult();
            var stack = new Stack<VDataTable>();

            if (this._top_tables != null)
            {
                foreach (var t in this._top_tables)
                {
                    stack.Push(t);
                }
                //!!! Раньше был поиск всех ошибок, оставил до первой
                //  var errors_info = new Dictionary<DataRow, string>();

                while (stack.Count > 0)
                {
                    var table = stack.Pop();
                    //errors_info.Clear();

                    var tableResult = table.CheckValidation();
                    result.Error = tableResult.Error;
                    if (result.Error != "")
                    {
                        return result;
                    }
                    else
                    {
                        foreach (var s in tableResult.Warning)
                        {
                            result.Warning.Add(s);
                        }
                    }

                    if (table.childDataSets != null)
                    {
                        foreach (var ds in table.childDataSets)
                        {
                            foreach (var t in ds.TopTable)
                            {
                                stack.Push(t);
                            }
                        }
                    }
                    foreach (var r in table.ChildRelations.Cast<DataRelation>()) stack.Push((VDataTable)r.ChildTable);
                }
            }

            result.Error = ParamsTable.CheckValidation().Error;
            if (result.Error != "")
            {
                return result;
            }
            return result;
        }
        //int mainThresdId = System.Threading.Thread.CurrentThread.ManagedThreadId;

        //public bool IsMainTheread()
        //{
        //    return mainThresdId == System.Threading.Thread.CurrentThread.ManagedThreadId;
        //}
        public VDataSet()
        {
        }
        /*public VDataColumn GetColumnByName(string name)
        {
            if (columns == null) {
                columns = new SortedList<string, VDataColumn>();
            }
            if (!columns.ContainsKey(name)) {
                foreach (DataTable table in Tables) {
                    foreach (DataColumn column in table.Columns) {
                        if (column.ColumnName == name) {
                            columns.Add(column.ColumnName, (VDataColumn)column);
                        }
                    }
                }
            }
            return columns[name];
        }*/
        public VDataTable GetTable(string tablename)
        {
            if (string.IsNullOrEmpty(tablename))
            {
                return this.ParamsTable;
            }
            else
            {
                return (VDataTable)this.Tables[tablename];
            }
        }
        public void changed(object sender, EventArgs e)
        {
            if (Changed != null)
            {
                Changed(sender, e);
            }
        }

        public void RaiseNeedSelection()
        {
            if (NeedSelection != null)
            {
                NeedSelection(this, null);
            }
        }

        public void RaiseSchemeChanged()
        {
            if (SchemeChanged != null)
            {
                SchemeChanged(this, null);
            }
        }

        public delegate void FCustomRefresh(VDataSet dataSet);

        public void Refresh(int useRepository = 2)
        {
            Refresh(null, useRepository);
        }
        public VOracleConnection GetConnection()
        {
            if (this.Connection == null)
            {
                //return (IsVertica) ? (DbConnection)Report.environment.VConnection : Report.environment.Connection;
                return XmlReports.Environment.Connection;
            }
            else
            {
                return this.Connection;
            }
        }
        public static XElement ParsObjectArrayToXelement(object[] pars, XElement xformalParams)
        {
            //пока только для числовых параметров;
            if (xformalParams == null)
            {
                return null;
            }
            XElement xpars = new XElement(xformalParams);
            xpars.Elements().Elements().Remove();
            if (pars != null)
            {
                for (int index = 0; index < pars.Length; index++)
                {
                    object obj = pars[index];
                    XElement xformalParam = xpars.Elements().ElementAt(index);
                    XElement xval;
                    object[] array = obj as object[];
                    if (array != null)
                    {
                        xval = Factory.NewCall(TextConst.AVFunction.Array);
                        for (int index_2 = 0; index_2 < array.Length; index_2++)
                        {
                            string val = Cmn.ToOracleString(array[index_2]);
                            XElement xval1 = Factory.NewConst(val);
                            xval.Add(xval1);
                        }
                    }
                    else
                    {
                        if (Cmn.undefinedString.Equals(obj))
                        {
                            xval = new XElement(EName.undefined);
                        }
                        else
                        {
                            string val = Cmn.ToOracleString(obj);
                            xval = Factory.NewConst(val);
                        }
                    }
                    xformalParam.Add(xval);
                }
            }
            return xpars;
        }
        public void Refresh(object[] pars)
        {
            XElement xpars = ParsObjectArrayToXelement(pars, this.Report.Element(EName.@params));
            this.Refresh(xpars);
        }
        public void ClearInputParams()
        {
            this.InputParams = null;
            this.InputParamsNames = null;
        }
        public void AddInputParam(string name, VOracleParameter dbPar, object value)
        {
            if (this.InputParams == null)
            {
                this.InputParams = new Dictionary<string, VOracleParameter>();
                this.InputParamsNames = new SortedList<int, string>();
                this.InputParamsValues = new SortedList<string, object>();
            }
            this.InputParams.Add(name, dbPar);
            this.InputParamsValues.Add(name, value);
            this.InputParamsNames.Add(this.InputParamsNames.Count, name);
        }
        private VOracleParameter GetInputParam(int index)
        {
            string name = this.InputParamsNames[index];
            return this.InputParams[name];
        }
        public void SetParamsValues(IList<object> values)
        {
            if (values == null)
            {
                return;
            }
            for (int index = 0; index < values.Count; index++)
            {
                object val = values[index];
                VOracleParameter par = this.GetInputParam(index);
                if (par.GetOracleDbType() == VOracleDbType.Array)
                {
                    ArrayStorage arrayStorage = new ArrayStorage(par.ParameterName);
                    arrayStorage.SetValues(val as object[]);
                    val = arrayStorage.GetSql();
                }
                par.Value = val;
            }
        }
        private void SetParams(XElement pars)
        {
            object val;
            foreach (VOracleParameter dbPar in this.InputParams.Values)
            {
                XElement factParam = pars.Elements().SearchByAttribute(AName.name, dbPar.ParameterName);
                // ищем параметр среди глобальных
                //if (SqlBuilder.InputParams != null && factParam == null)
                //{
                //    factParam = SqlBuilder.InputParams.Elements().FirstOrDefault(e => e.Attribute("name").Value == dbPar.ParameterName);
                //}
                if (factParam == null || factParam.Value == Cmn.undefinedString)
                {
                    val = Cmn.undefinedString;
                }
                else
                {
                    if (!factParam.HasElements || factParam.Elements().First().Name == EName.undefined)
                    {
                        val = Cmn.undefinedString;
                    }
                    else
                    {
                        if (dbPar.DbType == DbType.Decimal)
                        {
                            val = Cmn.ToDecimal(factParam.Value);
                        }
                        else
                        {
                            if (dbPar.OracleDbType == VOracleDbType.Array)
                            {
                                object[] arrVal = ArrayParamXElementContentToObjectArray(factParam.Elements().First());
                                ArrayStorage arrayStorage = new ArrayStorage(factParam.Attribute(AName.name).Value);
                                arrayStorage.SetValues(arrVal);
                                val = arrayStorage.GetSql();
                            }
                            else
                            {
                                if (dbPar.OracleDbType == VOracleDbType.Date)
                                {
                                    val = Cmn.ExtractDateFromOracleToDateString(factParam.Value);
                                }
                                else if (dbPar.OracleDbType == VOracleDbType.VarChar)
                                {
                                    val = Cmn.ExtractStringFromOracleString(factParam.Value);
                                }
                                else
                                {
                                    val = factParam.Value;
                                }
                            }
                        }
                    }
                }
                dbPar.Value = val;
            }
        }
        private void ExecuteReportProc(ref string retSql, bool onlyGetSql)
        {
            VReportProc repProc = this.Report.GetReportProc();
            if (repProc != null)
            {
                //WaitUIHelper.LastUsedUIHelper.Show("Загрузка данных", WaitUIMode.WaitCursor);
                OracleCommand procCmd = new VOracleCommand();
                procCmd.Connection = this.GetConnection();
                procCmd.ParameterCheck = true; // чтобы коллекция Parameters заполнилась при установке CommandText
                procCmd.CommandText = repProc.Value;
                string[] ParamNames = Cmn.GetParameterNames(procCmd.Parameters);
                procCmd.ParameterCheck = false;
                procCmd.Parameters.Clear();
                if (this.Report.IsSimpleParams)
                {
                    VDataTable.SetCommandParams(this, null, procCmd, ParamNames);
                }
                else
                {
                    foreach (string name in ParamNames)
                    {
                        XElement par = this.Report.Elements(EName.@params).Elements(EName.param).First(e => e.Attribute(AName.name).Value == name);
                        string parVal;
                        if (par.Element(EName.undefined) != null)
                        {
                            parVal = Cmn.undefinedString;
                        }
                        else
                        {
                            parVal = par.Value;
                        }
                        procCmd.CommandText = procCmd.CommandText.Replace(":" + name, parVal);
                    }
                }
                if (onlyGetSql)
                {
                    procCmd.CommandText = Cmn.ClearUndefined(procCmd.CommandText); // Для совместимости
                    retSql = retSql + Environment.NewLine
                                    + VDBSelectCommand.GetCmdParametrizedText(procCmd)
                                    + Environment.NewLine
                                    + "/";
                }
                else
                {
                    WaitUIHelper.LastUsedUIHelper.SetDescription("Выполнение хранимой процедуры...");
                    bool undefined_without_brace;
                    procCmd.CommandText = Cmn.ClearUndefined(procCmd.CommandText, out undefined_without_brace);
#if DEBUG
                    Stopwatch sw = new Stopwatch();
                    sw.Start();
#endif
                    // DevAnalyzer.AnalyzePrepSql(procCmd.CommandText);

                    if (!DevUtilsProvider.Instance.IsPrepareOnly())
                    {
                        procCmd.ExecuteNonQuery();
                    }

#if DEBUG
                    sw.Stop();
                    // Debug.WriteLine("OracleCommnad.ExecuteNonQuery(): report \"" + this.Report.AttrOrEmpty(AName.name) + "\" за " + sw.ElapsedTicks.ToString() + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
#endif
                    WaitUIHelper.LastUsedUIHelper.SetDescription(WaitUIHelper.DESCRIPTION_DEFAULT);
                }
                //var connection=GetConnection();
                //var cmdProc = connection.CreateCommand("BEGIN  hg_otg_report.REPORT_F003(2008.12); END;");
                //cmdProc.ExecuteNonQuery();
                //var cmdSel = connection.CreateCommand("select count(0) from rr_rab_test1");
                //var val = cmdSel.ExecuteScalar();
                //var a = db.ExecuteObject("select count(1) from rr_rab_test1", procCmd.Connection);
                //WaitUIHelper.LastUsedUIHelper.Hide();
            }
        }
        //IEnumerable<string> ProcParamNames = null;
        public void Refresh(XElement pars, int useRepository = 2, bool onlyProc = false)
        {
            string s = string.Empty;
            Refresh(pars, ref s, useRepository, onlyProc);
        }
        public void Refresh(XElement pars, ref string retSql, int useRepository = 2, bool onlyProc = false, bool onlyGetSql = false)
        {
            // Значительная часть алгоритма ниже заявязана на Report , который равен null для DataSet полученного не из Report
            // Пока закрыто заглушками и проверками 
            if (this.CustomRefresh != null)
            {
                this.CustomRefresh.Invoke(this);
                this._refreshed = true;
                return;
            }
            if (XmlReports.SourceFolder == null)
            {
                WaitUIHelper.LastUsedUIHelper.Show("Загрузка данных", WaitUIMode.WaitCursor);
            }
            try
            {
                if (this.Report != null && this.Report.Pivot)
                {
                    this.ExecuteReportProc(ref retSql, onlyGetSql);
                    this.Report.Result(pars, useRepository, this, false, this.SchemePreset);
                    this.addBandsForTransposed();
                }
                if (pars != null && this.InputParams != null)
                {
                    this.SetParams(pars);
                }
                int index;
                // !! Организовать транзакции
                //if (!IsVertica) ((OracleConnection)GetConnection()).AutoCommit = false;
                if (this.Report != null && this.Report.Complicated)
                {
                    XmlDocument result = XmlReports.executeReportOld(this.Report.Attribute(AName.name).Value, false, this.CompiledReport);
                    this.Scheme.Elements().Remove();
                    this.Scheme.Add(XDocument.Parse(result.SelectSingleNode("root/scheme").OuterXml).Root.Elements());
                    Parser.LoadReportDataFromXml(XDocument.Parse(result.InnerXml).Root, this);
                    foreach (VDataTable table in this.Tables)
                    {
                        table.ClientCalculations();
                    }
                }
                else
                {
                    if (this.Report != null && !this.Report.Pivot)
                    {
                        this.ExecuteReportProc(ref retSql, onlyGetSql);
                    }
                    if (this.UseTempTable)
                    {
                        if (!onlyGetSql)
                        {
                            WaitUIHelper.LastUsedUIHelper.SetDescription("Заполнение временной таблицы...");
                        }
                        if (this.MatQueriesNames.Count != 0)
                        {
                            string namesToClear = "'" + this.MatQueriesNames[0] + "'";
                            for (index = 1; index < this.MatQueriesNames.Count; index++)
                            {
                                namesToClear = namesToClear + ",'" + this.MatQueriesNames[index] + "'";
                            }
                            XmlReports.executeNonQuery("delete from rr_temp where skod in (" + namesToClear + ")", GetConnection(), null, false);
                        }
                        //IEnumerable<string> ProcParamNames = VReport.ExtractParamsFromSqlText(this.ProcedureText).OrderByDescending(Cmn.LengthOfString);
                        VOracleParameter[] parsList;
                        VOracleCommand cmd = new VOracleCommand();
                        cmd.ParameterCheck = true; // чтобы коллекция Parameters заполнилась при установке CommandText
                        cmd.CommandText = this.ProcedureText;
                        string[] ProcParamNames = Cmn.GetParameterNames(cmd.Parameters);
                        cmd.ParameterCheck = false;
                        cmd.Parameters.Clear();
                        if (ProcParamNames.Length != 0)
                        {
                            // Сортируем так, чтобы подстановка значений параметров прошла в правильном порядке (сначала kodd_flat, затем kodd, см. 71061 и 71118 в SD)
                            System.Array.Sort<string>(ProcParamNames, Cmn.DescComparsionByLength);
                            VDataTable.SetCommandParams(this, null, cmd, ProcParamNames);
                            parsList = new VOracleParameter[cmd.Parameters.Count];
                            cmd.Parameters.CopyTo(parsList, 0);
                            for (index = cmd.Parameters.Count - 1; index >= 0; index--)
                            {
                                cmd.Parameters.RemoveAt(index);
                            }
                        }
                        else
                        {
                            parsList = null;
                        }
                        if (onlyGetSql)
                        {
                            retSql += Environment.NewLine + VDBSelectCommand.GetCmdParametrizedText(cmd) + Environment.NewLine + "/";
                        }
                        else
                        {
                            // DevAnalyzer.AnalyzePrepSql(cmd.CommandText);
                            // DevAnalyzer.AnalyzeSuppressedSql(cmd.CommandText);
                            if (!DevUtilsProvider.Instance.IsPrepareOnly())
                            {
                                XmlReports.executeNonQuery(cmd.CommandText, GetConnection(), parsList);
                            }
                        }
                        if (this.UpdateTempTable)
                        {
                            var tbl = (VDataTable)this.Tables[0];
                            VDataColumn col_dog = tbl.GetColumn("kod_dog");
                            VDataColumn col_kodp = tbl.GetColumn("kodp");
                            if (col_dog != null)
                            {
                                SqlUslPoisk.FillDogovorDataByt(col_dog.TempColumnName, tbl.QueryName, GetConnection());
                            }
                            else if (col_kodp != null)
                            {
                                SqlUslPoisk.FillAbonentDataByt(col_kodp.TempColumnName, tbl.QueryName, GetConnection());
                            }
                        }
                        if (!onlyGetSql)
                        {
                            WaitUIHelper.LastUsedUIHelper.SetDescription(WaitUIHelper.DESCRIPTION_DEFAULT);
                        }
                    }
                    if (!this.IsVertica)
                    {
                        for (index = 0; index < this.Tables.Count; index++)
                        {
                            VDataTable table = (VDataTable)this.Tables[index];
                            string cmd_text = table.DataAdapter.SelectCommand.CommandText;
                            table.DataAdapter.SelectCommand.Dispose();
                            table.DataAdapter.SelectCommand = null;
                            Cmn.DisposeAndSetNull(ref table.DataAdapter);
                            table.DataAdapter = new VOracleDataAdapter();
                            table.DataAdapter.SelectCommand = new VOracleCommand(cmd_text);
                            // DevAnalyzer.AnalyzePrepSql(cmd_text);
                        }
                    }
                    // Емцов. иногда данные не нужно грузить на клиент
                    for (index = 0; index < this.Tables.Count; index++)
                    {
                        VDataTable table = (VDataTable)this.Tables[index];
                        if (!VDataTable.IsDependantRefresh(table))
                        {  // См. SD 71981 в тепловой Казани 
                            VDataTable vdt = table as VDataTable;
                            Contract.Assume(vdt != null);
                            vdt.Refresh(ref retSql, onlyProc || onlyGetSql);
                        }
                    }
                    for (index = 0; index < this.Tables.Count; index++)
                    {
                        DataTable table = this.Tables[index];
                        if (!VDataTable.IsDependantRefresh(table))
                        {  // См. SD 71981 в тепловой Казани 
                            VDataTable vdt = table as VDataTable;
                            Contract.Assume(vdt != null);
                            VDataTableTransposeUtils.TransposeIfNeed(vdt);
                            vdt.DoClientCalculations();
                        }
                    }
                }
                this._refreshed = true;
            }
            finally
            {
                if (XmlReports.SourceFolder == null)
                {
                    WaitUIHelper.LastUsedUIHelper.Hide();
                }
            }
            if (this.Scheme != null)
            {
                this.Scheme.SetAttrValue(AName.timestamp, DateTime.Now);
            }
        }
        public void addBandsForTransposedPre()
        {
            foreach (XElement col in this.Scheme.Descendants(EName.table).Elements(EName.viewcolumns).Descendants(EName.column).Where(e => e.AttrOrEmpty(AName.pivot) == "1").ToList())
            { // Множественные колонки меняем на бенды
                if (!col.AncestorsAndSelf().Any(e => e.Attribute(AName.dimension) != null))
                {
                    XElement band = new XElement(EName.band);
                    band.CopyAttributes(col.Attributes());
                    //XElement newcol = new XElement(col);
                    //newcol.SetAttributeValue("title", "...");
                    //newcol.Attributes("pivot").Remove();
                    //band.Add(newcol);
                    col.ReplaceWith(band);
                }
            }
        }
        private void addBandsForTransposed()
        {

            var dimNames = new SortedList<string, List<string>>();


            foreach (XElement tbl in Scheme.Descendants("table").ToArray()) //Меняем обратно, чтобы не переписывать то, что ниже
            {
                dimNames.Add(tbl.Attribute(TextConst.AName.As).Value, new List<string>());
                foreach (XElement band in tbl.Elements("viewcolumns").Descendants("band").Where(e => Cmn.GetAttrValue(e, "pivot") == "1").ToArray()) //Меняем обратно, чтобы не переписывать то, что ниже
                {

                    if (!dimNames[tbl.Attribute(TextConst.AName.As).Value].Contains(band.Attribute("dimname").Value))
                    {
                        dimNames[tbl.Attribute(TextConst.AName.As).Value].Add(band.Attribute("dimname").Value);
                    }
                    XElement col = new XElement("column");
                    Cmn.copyAttributes(band, col);
                    band.ReplaceWith(col);
                }

            }
            foreach (XElement table in Scheme.Descendants("table"))
            {
                XElement viewColumns = table.Element("viewcolumns");


                XElement columns = table.Element("columns");
                IEnumerable<XElement> pivColumns = columns.Elements().Where(e1 => Cmn.GetAttrValue(e1, "dimension-column") != "").ToArray();
                if (pivColumns.Count() > 0 || dimNames[table.Attribute(AName.@as).Value].Count > 0)
                {


                    List<string> dimNames1 = pivColumns.Select(e1 => e1.Attribute("dimension-column").Value).Where(e1 => !dimNames[table.Attribute(TextConst.AName.As).Value].Contains(e1)).Distinct().ToList();
                    dimNames[table.Attribute(TextConst.AName.As).Value].AddRange(dimNames1);

                    foreach (string dimName in dimNames[table.Attribute(TextConst.AName.As).Value])
                    {
                        IEnumerable<XElement> pivColumns1 = pivColumns.Where(e => Cmn.GetAttrValue(e.Attribute("dimension-column")) == dimName);
                        List<string> colNames = pivColumns1.Select(e1 => e1.Attribute("value-column").Value).Distinct().ToList();
                        List<string> dimValues = pivColumns1.Select(e1 => e1.Attribute("dimension-value").Value).Distinct().ToList();
                        foreach (XElement dimBand in viewColumns.Descendants("band").Where(e => Cmn.GetAttrValue(e, "dimension") == dimName).ToArray())
                        {
                            foreach (string dimValue in dimValues)
                            {
                                XElement newDimBand = new XElement(dimBand);
                                newDimBand.SetAttributeValue("dimension-value", dimValue);
                                dimBand.AddBeforeSelf(newDimBand);
                            }
                            dimBand.Remove();

                        }

                        foreach (string colName in colNames)
                        {
                            List<XElement> cols = pivColumns1.Where(e => Cmn.GetAttrValue(e.Attribute("value-column")) == colName).ToList();
                            XElement plCol = viewColumns.Descendants("column").Where(e => Cmn.GetAttrValue(e, "name") == colName).FirstOrDefault();
                            IEnumerable<XElement> plCols = null;
                            if (plCol != null)
                            {
                                if (plCol.Ancestors("band").FirstOrDefault(e => Cmn.GetAttrValue(e, "dimension") == dimName) != null)
                                {
                                    plCols = viewColumns.Descendants("column").Where(e => Cmn.GetAttrValue(e, "name") == colName).ToArray();
                                }
                            }
                            if (cols.Count > 0)
                            {
                                XElement band = null;
                                if (plCols == null)
                                {
                                    var title = "";

                                    if (plCol != null)
                                    {
                                        title = plCol.Attribute("title").Value;
                                    }
                                    else
                                    {
                                        title = Cmn.GetAttrValue(cols[0], "value-title");
                                    }
                                    band = new XElement("band", new XAttribute("title", title)
                                       , new XAttribute("value-column", cols[0].Attribute("value-column").Value)
                                        );
                                    if (plCol != null)
                                    {
                                        plCol.AddBeforeSelf(band);

                                    }
                                }


                                int i = 0;
                                foreach (XElement col in cols)
                                {
                                    XElement viewCol = viewColumns.Descendants("column").Where(e => e.Attribute("name").Value == col.Attribute("name").Value).First();

                                    if (plCols != null)
                                    {
                                        viewCol.SetAttributeValue("title", col.Attribute("value-title").Value);
                                        plCol = plCols.Where(e => e.Ancestors("band").Where(e1 => Cmn.GetAttrValue(e1, "dimension") == dimName && Cmn.GetAttrValue(e1, "dimension-value") == Cmn.GetAttrValue(col, "dimension-value")).FirstOrDefault() != null).FirstOrDefault();
                                        if (plCol != null)
                                        {
                                            band = plCol.Ancestors("band").Where(e1 => Cmn.GetAttrValue(e1, "dimension") == dimName && Cmn.GetAttrValue(e1, "dimension-value") == Cmn.GetAttrValue(col, "dimension-value")).First();
                                            band.SetAttributeValue("title", col.Attribute("band-title").Value);
                                            Cmn.CopyAttribute(plCol, viewCol, "agg");
                                            Cmn.CopyAttribute(plCol, col, "agg");
                                            viewCol.Remove();
                                            plCol.AddBeforeSelf(viewCol);
                                            plCol.Remove();
                                            viewCol.SetAttributeValue("visible", "1");
                                            viewCol.SetAttributeValue("title", plCol.Attribute("title").Value);
                                        }
                                    }
                                    else
                                    {
                                        viewCol.SetAttributeValue("title", col.Attribute("band-title").Value.SubstringAfter('|'));
                                        if (plCol != null)
                                        {
                                            viewCol.SetAttributeValue("visible", "1");
                                            Cmn.CopyAttribute(plCol, viewCol, "agg");
                                            Cmn.CopyAttribute(plCol, col, "agg");
                                        }
                                        if (band.Parent == null)
                                        {
                                            viewCol.AddBeforeSelf(band);
                                        }
                                        viewCol.Remove();
                                        XElement tgBand = getBandForMultidimColumn(band, col.Attribute("band-title").Value);
                                        tgBand.Add(viewCol);
                                    }
                                    i++;
                                }
                            }
                            if (plCol != null)
                            {
                                if (plCol.Parent != null)
                                {
                                    plCol.Remove();
                                }
                            }

                        }

                        XElement dimValuesEl = new XElement("dimension-values");
                        dimValuesEl.SetAttributeValue("table", dimName);


                        List<string> vals = new List<string>();
                        foreach (XElement col in pivColumns1)
                        {
                            if (!vals.Contains(col.Attribute("dimension-value").Value))
                            {
                                XElement dimVal = new XElement("val");
                                dimVal.SetAttributeValue("value", col.Attribute("dimension-value").Value);
                                dimVal.SetAttributeValue("title", col.Attribute("band-title").Value);
                                dimValuesEl.Add(dimVal);
                                vals.Add(col.Attribute("dimension-value").Value);
                            }
                        }

                        table.Add(dimValuesEl);

                    }
                }
            }
        }
        private static XElement getBandForMultidimColumn(XElement rootBand, string title)
        {
            List<string> titleArray = title.Split('|').ToList();
            XElement tgBand = rootBand;
            XElement newBand = null;
            for (int i = 0; i < titleArray.Count - 1; i++)
            {
                newBand = tgBand.Elements(EName.band).FirstOrDefault(e => e.AttrOrDefault(AName.title, string.Empty) == titleArray[i]);
                if (newBand == null)
                {
                    newBand = new XElement(EName.band, new XAttribute(AName.title, titleArray[i]));
                    tgBand.Add(newBand);
                }
                tgBand = newBand;
            }
            return tgBand;
        }
        public List<string> GetParNames()
        {
            var ss = new List<string>();
            if (InputParams != null)
            {
                ss.AddRange(InputParams.Values.Select(Cmn.GetParameterName).Distinct());
            }
            return ss;
        }
        public void SetParamInfo(string paramName, string value)
        {
            VDataTable tbl = this.GetTable("params_info");
            DataColumn col_name, col_text;
            if (tbl == null)
            {
                tbl = new VDataTable(false);
                tbl.TableName = "params_info";
                tbl.StructureType = StructureType.Info;
                col_name = new VDataColumn("param_name", typeof(string));
                tbl.Columns.Add(col_name);
                col_text = new VDataColumn("text", typeof(string));
                tbl.Columns.Add(col_text);
                tbl.PrimaryKey = new DataColumn[1] { col_name };
                this.Tables.Add(tbl);
            }
            else
            {
                col_name = tbl.Columns["param_name"];
                col_text = tbl.Columns["text"];
            }
            DataRow row = tbl.Rows.Find(paramName);
            if (row == null)
            {
                row = tbl.NewRow();
                row[col_name] = paramName;
                row[col_text] = value;
                tbl.Rows.Add(row);
            }
            else
            {
                row[col_text] = value;
            }
        }
        private IList<VDataTable> ArrayValueTables()
        {
            List<VDataTable> list = new List<VDataTable>();
            for (int index = 0; index < this.Tables.Count; index++)
            {
                VDataTable dt = (VDataTable)this.Tables[index];
                if (dt.StructureType == StructureType.Array)
                {
                    list.Add(dt);
                }
            }
            return list;
        }
        public VDataTable ArrayValueTable(string name)
        {
            VDataTable dt = (VDataTable)this.Tables[name];
            if (dt != null && dt.StructureType == StructureType.Array)
            {
                return dt;
            }
            else
            {
                return null;
            }
        }
        /// <seealso cref="Compiler.addFormInfoQueries"/>
        public VXElement GetParamsAsXml(ICollection<string> fieldNames, string formName, bool onlyForSelectedValues)
        {
            Contract.Assert(fieldNames != null);
            int index;
            /* т.к. параметр fieldNames всегда не null
            IList<VDataTable> list = this.ArrayValueTables();
            if (fieldNames == null) {
                fieldNames = new HashSet<string>();
                DataColumnCollection cols = this.ParamsTable.Columns;
                for (index = 0; index < cols.Count; index++) {
                    VDataColumn col = (VDataColumn)cols[index];
                    if (col.ParamUsed) {
                        fieldNames.Add(col.ColumnName);
                    }
                }
                for (index = 0; index < list.Count; index++) {
                    VDataTable dt = list[index];
                    if (dt.ParamUsed) {
                        fieldNames.Add(dt.TableName);
                    }
                }
            }*/
            if (!string.IsNullOrEmpty(formName))
            {
                DataTable params_info = this.Tables["params_info"];
                if (params_info != null)
                {
                    if (this.formInfoFieldsTextNodes == null)
                    {
                        XElement formInfoQuery = XmlReports.Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).SearchByAttribute(AName.name, "form:" + formName);
                        if (formInfoQuery == null)
                        {
                            formInfoQuery = XmlReports.Environment.Manager.GetOldScheme().Elements(EName.queries).Elements(EName.query).SearchByAttribute(AName.name, "form:" + formName);
                        }
                        if (formInfoQuery != null)
                        {
                            // Здесь анализируется query, созданный в Compiler.addFormInfoQueries()
                            // 1. Определяем позицию колонок "name" и "text" внутри select
                            XElement select = formInfoQuery.Element(EName.select);
                            Contract.Assert(select != null);
                            IEnumerable<XElement> cols = select.Elements();
                            XElement col_name = cols.SearchByAttribute(AName.@as, "name");
                            Contract.Assert(col_name != null);
                            XElement col_text = cols.SearchByAttribute(AName.@as, "text");
                            Contract.Assert(col_text != null);
                            int name_index = col_name.ElementsBeforeSelf().Count();
                            int text_index = col_text.ElementsBeforeSelf().Count();
                            //
                            IList<XElement> sub_queries = formInfoQuery.Element(EName.from).Elements().Elements(EName.union).Elements(EName.query).ToList();
                            int count = sub_queries.Count;
                            if (count == 0)
                            {
                                sub_queries = new XElement[1] { formInfoQuery };
                                count = 1;
                            }
                            // 2. Формируем коллекцию this.formInfoFieldsTextNodes
                            this.formInfoFieldsTextNodes = new SortedList<string, XElement>(count);
                            for (index = 0; index < count; index++)
                            {
                                select = sub_queries[index].Element(EName.select);
                                Contract.Assert(select != null);
                                cols = select.Elements();
                                col_name = cols.ElementAt(name_index);
                                Contract.Assert(col_name != null && col_name.Name == EName.@const);
                                col_text = cols.ElementAt(text_index);
                                Contract.Assert(col_text != null && col_text.Name == EName.@const);
                                string name = col_name.Value;
                                Contract.Assert(name != null && name.Length >= 2 && name[0] == '\'' && name[name.Length - 1] == '\'');
                                name = name.Substring(1, name.Length - 2);
                                this.formInfoFieldsTextNodes.Add(name, col_text);
                            }
                        }
                    }
                    if (this.formInfoFieldsTextNodes != null)
                    {
                        // заполняем теги из this.formInfoFieldsTextNodes значениями полей формы
                        DataColumn name_column = params_info.Columns["param_name"];
                        Contract.Assert(name_column != null && name_column.DataType == typeof(string));
                        DataColumn text_column = params_info.Columns["text"];
                        Contract.Assert(text_column != null && text_column.DataType == typeof(string));
                        for (index = 0; index < params_info.Rows.Count; index++)
                        {
                            DataRow row = params_info.Rows[index];
                            string param_name = row.Field<string>(name_column);
                            XElement node;
                            if (this.formInfoFieldsTextNodes.TryGetValue(param_name, out node))
                            {
                                string text = row.Field<string>(text_column);
                                if (string.IsNullOrEmpty(text))
                                {
                                    node.Value = "''";
                                }
                                else
                                {
                                    if (text.Length > 4000)
                                    {
                                        text = "'" + text.Substring(0, 3995) + "...'";
                                    }
                                    else
                                    {
                                        text = "'" + text + "'";
                                    }
                                    node.Value = text;
                                }
                            }
                        }
                    }
                }
            }
            VXElement pars = new VXElement(EName.@params);
            IList<VDataTable> list = this.ArrayValueTables();
            for (index = 0; index < list.Count; index++)
            {
                VDataTable tbl = list[index];
                if (fieldNames.Contains(tbl.TableName))
                {
                    VXElement param = new VXElement(EName.param);
                    param.Add(new XAttribute(AName.name, tbl.TableName));
                    param.Tag = tbl; // зачем нужен tag?
                    if (!tbl.ParamUsed && !onlyForSelectedValues)
                    {
                        param.Add(new XElement(EName.undefined));
                    }
                    else
                    {
                        param.Add(ArrayTableParamValueAsParamXelementContent(tbl));
                    }
                    pars.Add(param);
                }
            }
            DataColumnCollection columns = this.ParamsTable.Columns;
            for (index = 0; index < columns.Count; index++)
            {
                VDataColumn col = (VDataColumn)columns[index];
                if (VDataColumn.HasBoundControl(col))
                {
                    UIBase control = col.BoundControls[0];
                    if (fieldNames.Contains(control.FieldName) && control.SourceType != ReturnType.Array)
                    {
                        VXElement param = new VXElement(EName.param);
                        param.Add(new XAttribute(AName.name, col.ColumnName));
                        if (!col.ParamUsed && !onlyForSelectedValues)
                        { // 20171030 Бельченко, раньше этого условия не было, странно, как вообще работало ...
                            param.Add(new XElement(EName.undefined));
                        }
                        else
                        {
                            // Емцов - обработка null-значений
                            object val = ParamsTable.Rows[0][col];
                            if (col.BoundControls.First().ShowNulls)
                            {
                                if (Cmn.IsNullOrDBNull(val))
                                {
                                    if (col.DataType == typeof(decimal))
                                    {
                                        val = Cmn.ToDecimal(TextConst.NullConsts.NNULL);
                                    }
                                    else if (col.DataType == typeof(string))
                                    {
                                        val = TextConst.NullConsts.SNULL;
                                    }
                                    else if (col.DataType == typeof(DateTime))
                                    {
                                        val = DateTime.Parse(TextConst.NullConsts.DNULL);
                                    }
                                }
                            }
                            param.Tag = val;
                            param.Add(Factory.NewConst(Cmn.ToOracleString(val)));
                        }
                        pars.Add(param);
                    }
                }
            }
            if (this.InputParams != null)
            {
                foreach (VOracleParameter par in this.InputParams.Values)
                {
                    if (fieldNames.Contains(par.ParameterName))
                    {
                        VXElement param = new VXElement(EName.param);
                        param.Add(new XAttribute(AName.name, par.ParameterName));
                        param.Add(Factory.NewConst(Cmn.ToOracleString(par.Value)));
                        pars.Add(param);
                    }
                }
            }
            return pars;
        }
        private static XElement ArrayTableParamValueAsParamXelementContent(VDataTable tbl)
        {
            XElement call;
            if (!tbl.IsArrayParamStringUse)
            {
                call = Factory.NewCall(TextConst.AVFunction.Array);
            }
            else
            {
                call = Factory.NewCall("sarray");
            }
            DataColumn col = tbl.Columns[0];
            for (int index = 0; index < tbl.Rows.Count; index++)
            {
                string val = Cmn.ToOracleString(tbl.Rows[index][col]);
                call.Add(Factory.NewConst(val));
            }
            return call;
        }
        public static object[] ArrayParamXElementContentToObjectArray(XElement factParamContent)
        {
            bool isStringArray = factParamContent.AttrOrEmpty(AName.function) == "sarray";
            string s = string.Empty;
            string q = string.Empty;
            int isString = -1;
            var list = new List<object>();
            foreach (string val in factParamContent.DescendantsAndSelf(EName.@const).Select(EPredicate.ElementValue))
            {
                string val1 = val;
                if (isStringArray)
                {
                    s += q + val1;
                }
                else
                {
                    if (isString == -1)
                    {
                        if (val1.StartsWith("'"))
                        {
                            isString = 1;
                        }
                        else
                        {
                            isString = 0;
                        }
                    }
                    if (isString == 0)
                    {
                        list.Add(Cmn.ToDecimal(val1));
                    }
                    else
                    {
                        val1 = val1.Substring(1, val1.Length - 2);
                        list.Add(val1);
                    }
                }
                q = ",";
            }
            if (isStringArray)
            {
                list.Add(s);
            }
            return list.ToArray();
        }
        private static object[] ArrayTableParamValueToObjectArray(DataTable tbl)
        {
            int count = tbl.Rows.Count;
            if (count == 0)
            {
                return Array.Empty<object>();
            }
            object[] array = new object[count];
            DataColumn col = tbl.Columns[0];
            for (int index = 0; index < count; index++)
            {
                array[index] = tbl.Rows[index][col];
            }
            return array;
        }
        public DataColumn GetParamColumn(string paramName)
        {
            string[] ss = paramName.Split('.');
            if (ss.Length == 2)
            {
                DataTable tbl = this.Tables[ss[0]];
                DataColumn col = tbl.Columns[ss[1]];
                return col;
            }
            else
            {
                return this.GetVariableColumn(paramName);
            }
        }
        public object GetParamValueByName(string paramName, DataRow row)
        {
            if (this.InputParams != null)
            {
                VOracleParameter param;
                if (this.InputParams.TryGetValue(paramName, out param))
                {
                    if (param.GetOracleDbType() == VOracleDbType.Array)
                    { // может что то словмать, пока оставлю только для array
                        return this.InputParamsValues[paramName];
                    }
                    else
                    {
                        return param.Value;
                    }
                }
            }
            VDataTable paramsTable = this.ParamsTable;
            if (paramsTable != null)
            {
                VDataTable paramTable = this.ArrayValueTable(paramName);
                if (paramTable != null)
                {
                    if (paramTable.ParamUsed)
                    {
                        object[] arrVal = ArrayTableParamValueToObjectArray(paramTable);
                        //ArrayStorage arrayStorage = new ArrayStorage(paramName);
                        //arrayStorage.SetValues(arrVal);
                        //var val = arrayStorage.GetSql();
                        return arrVal;
                        //ArrayTableParamValueToString(paramTable);
                    }
                    else
                    {
                        return Cmn.undefinedString;
                    }
                }
                else if (paramsTable.Columns.Contains(paramName))
                {
                    VDataColumn parCol = paramsTable.GetColumn(paramName);
                    if (parCol.ParamUsed)
                    {
                        return paramsTable.CurrentRow[parCol];
                    }
                    else
                    {
                        return Cmn.undefinedString;
                    }
                }
            }
            DataColumn col = GetParamColumn(paramName);
            if (col != null)
            {
                DataRow row1 = (col.Table as VDataTable).CurrentRow;
                if (row != null)
                {
                    if (row.Table == col.Table)
                    {
                        row1 = row;
                    }
                }
                if (row1 == null)
                {
                    return null;
                }
                return (col as VDataColumn).GetValue(row1);// row1[col];
            }
            return null;
        }
        public VOracleParameter GetParamAsOracleParametr(string paramName)
        {
            VOracleParameter dbPar;
            if (!VDBSelectCommand.TryGetGlobalDbParam(paramName, out dbPar))
            {
                VDataTable paramsTable = this.ParamsTable;
                if (paramsTable != null)
                {
                    VDataTable paramTable = (paramsTable.DataSet as VDataSet).ArrayValueTable(paramName);
                    if (paramTable != null)
                    {
                        dbPar = new VOracleParameter(paramName, VOracleDbType.Array);
                        if (paramTable.ParamUsed)
                        {
                            object[] arrVal = ArrayTableParamValueToObjectArray(paramTable);
                            ArrayStorage arrayStorage = new ArrayStorage(paramName);
                            arrayStorage.SetValues(arrVal);
                            string val = arrayStorage.GetSql();
                            dbPar.Value = val;
                        }
                        else
                        {
                            dbPar.Value = Cmn.undefinedString;
                        }
                    }
                    else
                    {
                        VDataColumn parCol = this.GetVariableColumn(paramName);
                        //var parCol = (VDataColumn)paramsTable.Columns[paramName];
                        dbPar = new VOracleParameter(paramName, Cmn.GetDBType(parCol.DataType));
                        if (paramsTable.Columns.Contains(paramName))
                        {
                            if (parCol.ParamUsed || TextConst.AVParamArray.FormExtPars.Contains(paramName) || TextConst.AVParamArray.TableExtPars.Contains(paramName))
                            {
                                object val = paramsTable.CurrentRow[parCol];
                                if (VDataColumn.HasBoundControl(parCol) && parCol.BoundControls[0].IsStringToArray())
                                {
                                    dbPar.OracleDbType = VOracleDbType.Array;
                                    if (Cmn.IsNullOrDBNull(val))
                                    {
                                        val = string.Empty;
                                    }
                                    List<object> arrVal = Cmn.SplitString((string)val).ToList<object>();
                                    if (arrVal.Count == 0)
                                    {
                                        arrVal.Add(" ");
                                    }
                                    ArrayStorage arrayStorage = new ArrayStorage(paramName);
                                    arrayStorage.SetValues(arrVal.ToArray());
                                    val = arrayStorage.GetSql();
                                }
                                dbPar.Value = val;
                            }
                            else
                            {
                                dbPar.Value = Cmn.undefinedString;
                            }
                        }
                        else
                        {
                            dbPar.Value = this.GetVariableValue(paramName);
                        }
                    }
                }
            }
            return dbPar;
        }
        public static VDataSet FromXml(XElement xParams, VDataSet dataSet = null, bool clean_ds = true)
        {
            VDataSet dsReport = dataSet;
            if (dsReport == null)
            {
                dsReport = new VDataSet();
            }
            else if (clean_ds)
            {
                // чистим констрэйнты
                dsReport.EnforceConstraints = false;
                dsReport.Relations.Clear();
                foreach (DataTable dt in dsReport.Tables)
                {
                    dt.ChildRelations.Clear();
                    dt.ParentRelations.Clear();
                    var constraints = dt.Constraints.OfType<ForeignKeyConstraint>().Reverse();
                    foreach (ForeignKeyConstraint constraint in constraints)
                    {
                        dt.Constraints.Remove(constraint);
                    }
                }
                foreach (DataTable dt in dsReport.Tables)
                {
                    dt.Constraints.Clear();
                }
                dsReport.Tables.Clear();
            }
            // ищем узел с данными 
            XElement xData = xParams.Element(EName.data);
            // рекурсивно заполняем 
            foreach (var xTable in xParams.Element(EName.scheme).Elements(EName.table))
            {
                getDataTableFromXml(dsReport, xTable, xData);
            }
            return dsReport;
        }
        public static XElement ToXml(VDataSet ds, XName root_name)
        {
            if (ds.Scheme == null || !ds.Scheme.HasElements)
            {
                ds.Scheme = GetXmlSchemeFromDataSet(ds);
            }
            XElement xRoot = new XElement(root_name, ds.Scheme);
            XElement xData = new XElement(EName.data);
            // перебираем описания таблиц
            foreach (XElement xSchemeTable in xRoot.Element(EName.scheme).Elements(EName.table))
            {
                string alias = xSchemeTable.Attribute(AName.@as).Value;
                // каждому описанию сопоставляем DataTable
                VDataTable dt = (VDataTable)ds.Tables[alias];
                // создаем описание данных таблицы
                XElement xTable = new XElement(EName.table, new XAttribute(AName.@as, alias));
                // записываем данные
                putDataTableToXml(dt, xSchemeTable, xTable);
                xData.Add(xTable);
            }
            xRoot.Add(xData);
            return xRoot;
        }
        private static DataTable getDataTableFromXml(VDataSet ds, XElement xTable, XElement xData)
        {
            bool is_new_dt = false;
            string table_name = xTable.Attribute(AName.@as).Value;
            // ищем таблицу с таким псевдонимом
            VDataTable dt = (VDataTable)ds.Tables[table_name];
            // если таблица не найдена - создаем новую
            if (dt == null)
            {
                is_new_dt = true;
                // формируем DataTable с псевдонимом таблицы
                dt = new VDataTable(xTable, false, table_name);
                dt.QueryName = xTable.AttrOrDefault(AName.name, null);
                dt.ExtendedProperties.Add("title", xTable.AttrOrEmpty(AName.title));
                // получаем описание колонок и заполняем по нему колонки в DataTable
                fillTableColumnsFromXml(dt, xTable.Element(EName.columns), ds);
                // добавляем таблицу в DataSet
                ds.Tables.Add(dt);
            }
            // получаем данные и заполняем ими DataTable
            if (xData != null)
            {
                // дополнительные сведения о колонках
                XElement xColumns = xTable.Element(EName.columns);
                fillTableDataFromXml(ref dt, xData, xColumns);
            }
            // если есть дочерние таблицы
            XElement xChilds = xTable.Element(EName.childs);
            if (xChilds != null)
            {
                // перебираем все дочерние таблицы
                foreach (XElement xchild_table in xChilds.Elements(EName.table))
                {
                    // формируем дочернюю таблицу
                    DataTable dt_child = getDataTableFromXml(ds, xchild_table, xData);
                    // добавляем связь в DataSet
                    if (is_new_dt)
                    {
                        DataRelation dr = ds.Relations.Add(dt.Columns[TextConst.AVColumn.Sid], dt_child.Columns[TextConst.AVColumn.SparentId]);
                        dr.RelationName = dt_child.TableName;
                    }
                }
            }
            return dt;
        }
        public static VXElement GetXmlSchemeFromDataTable(DataTable dt)
        {
            bool isDs = true;
            if (dt.DataSet == null)
            {
                isDs = false;
                new VDataSet().Tables.Add(dt);
            }

            VXElement el = GetXmlSchemeFromDataSet(dt.DataSet as VDataSet);

            if (!isDs)
            {
                dt.DataSet.Tables.Remove(dt);
            }
            return el;

        }
        public static VXElement GetXmlSchemeFromDataSet(VDataSet ds)
        {
            var xScheme = new VXElement(EName.scheme);
            foreach (VDataTable vdt in ds.Tables)
            {
                if (vdt.StructureType == StructureType.Info) continue;

                var xTable = new XElement("table",
                    vdt.QueryName != null ? new XAttribute("name", vdt.QueryName) : null,
                    new XAttribute("as", vdt.TableName));

                var xSchemeColumns = new XElement("columns");
                xSchemeColumns = new XElement("columns");
                foreach (DataColumn column in vdt.Columns)
                {
                    // дополнительное общее описание колонок
                    var xSchemeColumn = new XElement("column",
                        new XAttribute("name", column.ColumnName),
                        column.ColumnName != column.Caption ? new XAttribute("title", column.Caption) : null,
                        new XAttribute("type",
                            (column.DataType == typeof(Decimal))
                                ? "number"
                                : (column.DataType == typeof(DateTime) ? "date" : "string")),
                        new XAttribute("key", vdt.PrimaryKey.Contains(column) ? "1" : "0"));
                    xSchemeColumns.Add(xSchemeColumn);

                    if (column is VDataColumn)// не проверял
                    {
                        var vcol = (VDataColumn)column;
                        if (vcol.IsClientCalculations)
                        {
                            xSchemeColumn.SetAttributeValue(TextConst.AName.ClientCalulation, TextConst.AVBool.True);
                        }

                        if (vcol.IsExcelCalculations)
                        {
                            xSchemeColumn.SetAttributeValue(TextConst.AName.ExcelCalulation, TextConst.AVBool.True);
                        }

                    }
                }
                xTable.Add(xSchemeColumns);

                xScheme.Add(xTable);
            }

            return xScheme;
        }

        private static void putDataTableToXml(VDataTable dt, XElement xSchemeTable, XElement xTable, string sparent = null)
        {
            // колонки для связи
            string key_column = dt.HasPrimaryKey() ? dt.PrimaryKey[0].ColumnName : null;
            string parent_column = dt.ParentRelations.Count > 0
                ? dt.ParentRelations[0].ParentColumns[0].ColumnName
                : null;

            var xData = new XElement("data");
            // если колонки sparent нет, просто берем все строки, иначе только те, у которых sparent соответствует указаному
            var rows = parent_column != null
                ? dt.AsEnumerable().Where(row => row[parent_column].Equals((object)sparent ?? DBNull.Value))
                : dt.AsEnumerable();

            // перебираем строки 
            foreach (var row in rows)
            {
                // создаем описание строки данных
                var xTr = new XElement("tr",
                    key_column != null ? new XAttribute("id", row[key_column]) : null);

                // создаем описание ячеек
                var xCells = new XElement("cells");

                // перебираем колонки из описания схемы таблицы и берем данные из нужной ячейки
                foreach (var xSchemeColumn in xSchemeTable.Element("columns").Elements("column"))
                {
                    var value = row[xSchemeColumn.Attribute("name").Value];
                    if (value != DBNull.Value)
                    {
                        // преобразуем в стоку нужного типа
                        switch (xSchemeColumn.Attribute("type").Value)
                        {
                            case "number":
                                value = value.ToString().Replace(',', _num_separator);
                                break;
                            case "date":
                                value = ((DateTime)value).ToString(CultureInfo.CurrentCulture);
                                break;
                        }
                    }
                    xCells.Add(new XElement("td", value));
                }
                xTr.Add(xCells);

                if (key_column != null)
                {
                    // Перебираем описания дочерних таблиц, если они есть
                    var xSchemeChilds = xSchemeTable.Element("childs");
                    if (xSchemeChilds != null)
                    {
                        var xChilds = new XElement("childs");
                        foreach (var xSchemeChildTable in xSchemeChilds.Elements("table"))
                        {
                            // Получаем дочерний DataTable
                            var child_dt = (VDataTable)dt.DataSet.Tables[xSchemeChildTable.Attribute("as").Value];
                            // создаем описание данных таблицы
                            var xChildTable = new XElement("table",

                                new XAttribute(("as"), child_dt.TableName));
                            // рекурсивно заполняем данные в дочерних таблицах
                            putDataTableToXml(child_dt, xSchemeChildTable, xChildTable, (string)row[key_column]);
                            xChilds.Add(xChildTable);
                        }
                        xTr.Add(xChilds);
                    }
                }

                xData.Add(xTr);
            }
            xTable.Add(xData);
        }
        private static void fillTableColumnsFromXml(VDataTable dt, XElement xColumns, VDataSet ds)
        {
            foreach (XElement xcolumn in xColumns.Elements())
            {
                if (xcolumn.Name == EName.column)
                {
                    VDataColumn col = VDataColumn.Create(xcolumn);
                    XAttribute attr = xcolumn.Attribute(AName.into);
                    if (attr != null)
                    {
                        col.TempColumnName = attr.Value;
                    }
                    attr = xcolumn.Attribute(TextConst.AName.ValueColumn);
                    if (attr != null)
                    {
                        col.OriginalNameForPivotColumn = attr.Value;
                    }
                    attr = xcolumn.Attribute(TextConst.AName.DimensionValue);
                    if (attr != null)
                    {
                        object val = attr.Value;
                        if (Cmn.IsNumeric(val))
                        {
                            col.PivotDimensionValue = Cmn.ToDecimal(val.ToString()); // Пока только числовые, если нужно доделать
                        }
                    }
                    attr = xcolumn.Attribute(TextConst.AName.DimensionColumn);
                    if (attr != null)
                    {
                        col.PivotDimensionName = attr.Value;
                    }
                    dt.Columns.Add(col);
                    attr = xcolumn.Attribute(AName.@default);
                    if (attr != null)
                    {
                        col.DefaultValue = attr.Value;
                    }
                    if (xcolumn.AttrOrDefault(AName.client_calc, false))
                    {
                        col.IsClientCalculations = true; // можно устанавливать только после добавления колонки в DataTable
                    }
                    if (xcolumn.AttrOrDefault(AName.excel_calc, false))
                    {
                        col.IsExcelCalculations = true; // можно устанавливать только после добавления колонки в DataTable
                    }
                    attr = xcolumn.Attribute(AName.color);
                    if (attr != null)
                    {
                        col.BackColorSource = attr.Value;
                    }
                    attr = xcolumn.Attribute(AName.font_color);
                    if (attr != null)
                    {
                        col.FontColorSource = attr.Value;
                    }
                    attr = xcolumn.Attribute(AName.merge_key);
                    if (attr != null)
                    {
                        col.MergeKey = attr.Value;
                    }
                    if (xcolumn.Attribute(TextConst.AName.ParentNodeId) != null)
                    {
                        dt.TreeParentFieldName = col.ColumnName;
                    }
                    attr = xcolumn.Attribute(AName.parname);
                    if (attr != null)
                    {
                        ds.AddVariableColumn(attr.Value, col);
                    }
                }
                else if (xcolumn.Name == EName.band)
                {
                    fillTableColumnsFromXml(dt, xcolumn, ds);
                }
            }
        }
        private static void fillTableDataFromXml(ref VDataTable dt, XElement xData, XElement xColumns)
        {
            dt.ClearData();
            var table_name = dt.TableName;

            // Емцов - вернул, т.к. некорректно работала загрузка дефолтных значений

            dt.SuppressChangeEvent();
            // Достаем описание всех строк для таблицы
            var xtrs = (xData.Descendants("table")
                .Where(el => el.Attribute("as").Value == table_name)
                .Select(el => el.Element("data").Elements("tr")))
                .SelectMany(tr => tr);

            var column_names = (xColumns != null)
                ? xColumns.Elements("column").Select(c => c.Attribute("name").Value).ToArray()
                : null;

            // заполняем таблицу данными
            foreach (var xtr in xtrs)
            {
                var row = dt.NewRow();



                bool isFormatError = false;

                int col_num = 0;

                // Перебираем данные, добавляя их по порядку в DataRow, попутно приводя к типу колонки
                foreach (var xtd in xtr.Element("cells").Elements("td"))
                {
                    // если есть информация о имени колонки
                    var column = (column_names != null)
                        ? dt.Columns[column_names[col_num]]
                        : dt.Columns[col_num];

                    if (!xtd.Value.Equals(""))
                    {
                        if (column.DataType == XmlReports.numberType)
                        {
                            // для корректной обработки разделителей
                            if (Cmn.IsDecimal(xtd.Value) != Cmn.CheckResultTFU.False)
                            {
                                row[column] = Cmn.ToDecimal(xtd.Value);
                            }
                            else
                            {
                                isFormatError = true;
                                break;
                            }
                        }
                        else
                        {
                            row[column] = Convert.ChangeType(xtd.Value, column.DataType);
                        }

                    }
                    col_num++;
                }

                if (!isFormatError)
                {
                    dt.Rows.Add(row);
                }
            }

            //if (dt.StructureType == "table" && dt.TableName == "Table1" && dt.Rows.Count == 0)
            //{
            //    dt.Rows.Add(dt.NewRow());
            //}

            //dt.AcceptChanges();
            dt.ResumeChangeEvent();
            dt.changed(dt, EventArgs.Empty);
        }

        void CreateBinding(UIBase ctrl)
        {
            if (ctrl.SourceType == ReturnType.Simple)
            {
                var dt = GetTable(ctrl.TableName);
                var vcol = dt.Columns.Cast<VDataColumn>().FirstOrDefault(e => e.ColumnName == ctrl.FieldName);
                if (vcol == null)
                {
                    dt.AddColumn(ctrl.FieldName, ctrl.ValueType);
                }

                //if(VDataTable)
                //dt.ColumnChanged += OnColumnChanged;
            }
        }

        public IEnumerable<VDataTable> GetAllTables()
        {
            var stack = new Stack<VDataTable>();
            foreach (var t in Tables.Cast<VDataTable>()) stack.Push(t);

            while (stack.Count > 0)
            {
                var table = stack.Pop();
                foreach (var t in table.GetChildTables()) stack.Push(t);

                yield return table;
            }

        }
    }

    public class SaveResult
    {
        Dictionary<DataRow, OracleException> _rowsExceptions = new Dictionary<DataRow, OracleException>();
        bool _success = true;

        public Dictionary<DataRow, OracleException> RowsExceptions { get { return _rowsExceptions; } }
        public bool Success { get { return _success; } }

        public void AppendResult(SaveResult result)
        {
            if (!result.RowsExceptions.Any()) return;

            foreach (var ce in result.RowsExceptions) _rowsExceptions.Add(ce.Key, ce.Value);

            if (!result.Success) _success = false;
        }

        public void AddRowException(DataRow row, OracleException ex)
        {
            _rowsExceptions.Add(row, ex);
            _success = false;
        }
    }
}
