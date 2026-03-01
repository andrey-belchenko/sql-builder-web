using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Xml.Linq;
using Oracle.ManagedDataAccess.Client;
using sql.builder.Clean;
////using System.Windows.Forms;
//using DevExpress.XtraVerticalGrid;
using sql.builder.Core;
//using sql.builder.Test;
using SqlBuilderLib.DevTools;

namespace sql.builder.DataApi
{
    public sealed class VDBSelectCommand : IDisposable
    {
        #region static stuff
        private static Dictionary<string, object> _queryResults = new Dictionary<string, object>();
        public static object GetQueryScalarResult(string queryName, bool useChash = true)
        {
            if (useChash && _queryResults.ContainsKey(queryName))
            {
                return _queryResults[queryName];
            }
            var res = db.ExecuteObject(XmlReports.Environment.GetQuery(queryName).GetSql());
            if (useChash)
            {
                _queryResults.Add(queryName, res);
            }
            return res;
        }
        #endregion
        #region поля
        private VOracleCommand mainCommand;
        private VOracleCommand procedureCommand;
        private SortedList<int, string> orderedParams;
        #endregion
        public string GetCommandText()
        {
            return this.mainCommand.CommandText;
        }
        public void SetCommandText(string value)
        {
            this.mainCommand.CommandText = value;
        }
        public void CreateRetParam()
        {
            var par = new VOracleParameter(TextConst.DBParams.PrimaryKeyParam, VOracleDbType.Number, ParameterDirection.Output);
            this.mainCommand.Parameters.Add(par);
        }
        public object GetRetValue()
        {
            return this.mainCommand.Parameters[TextConst.DBParams.PrimaryKeyParam].Value;
        }
        public int GetParamIndex(string paramName)
        {
            foreach (KeyValuePair<int, string> p in this.orderedParams)
            {
                if (p.Value == paramName)
                {
                    return p.Key;
                }
            }
            return -1;
        }
        public SortedList<int, object> GetRetValues()
        {
            var list = new SortedList<int, object>();
            foreach (DbParameter par in this.mainCommand.Parameters)
            {
                if (par.Direction == ParameterDirection.Output || par.Direction == ParameterDirection.InputOutput)
                {
                    list.Add(GetParamIndex(par.ParameterName), par.Value);
                }
            }
            return list;
        }
        #region конструкторы
        private VDBSelectCommand()
        {
            this.orderedParams = new SortedList<int, string>();
        }
        private VDBSelectCommand(string commandText, string procedureText, XElement formalParsSource)
            : this()
        {
            // DevAnalyzer.AnalyzePrepSql(commandText);
            // DevAnalyzer.AnalyzePrepSql(procedureText);
            IList<XElement> query_params = formalParsSource.Elements(EName.@params).Elements(EName.param).ToList();
            this.mainCommand = CreateCommand(commandText, query_params);
            if (!string.IsNullOrEmpty(procedureText))
            {
                this.procedureCommand = CreateCommand(procedureText, query_params);
            }
            else
            {
                this.procedureCommand = null;
            }
            this.setOrderedParams(query_params);
        }
        public VDBSelectCommand(string commandText, XElement formalParsSource)
            : this(commandText, null, formalParsSource)
        {
        }
        public static VDBSelectCommand CreateFromCompiledQuery(XElement query, XElement compiledQuery)
        {
            string selectText = Compiler.GetQuerySelectStatmentFromCompiledQuery(compiledQuery);
            string procedureText = Compiler.GetQuerProcedureFromCompiledQuery(compiledQuery);
            return new VDBSelectCommand(selectText, procedureText, query);
        }
        #endregion
        private void setOrderedParams(IList<XElement> xparams)
        {
            int index = 0;
            while (index < xparams.Count)
            {
                XElement xpar = xparams[index];
                string param_name = xpar.Attribute(AName.name).Value;
                this.orderedParams.Add(index, param_name);
                index++;
            }
            for (int index_2 = 0; index_2 < this.mainCommand.Parameters.Count; index_2++)
            {
                string param_name = this.mainCommand.Parameters[index_2].ParameterName;
                if (!this.orderedParams.ContainsValue(param_name))
                {
                    this.orderedParams.Add(index, param_name);
                    index++;
                }
            }
        }
        public XElement ToXml()
        {
            var xroot = new XElement(EName.root);
            var xpars = new XElement(EName.@params);
            xroot.Add(xpars);
            var xselect = new XElement(EName.select);
            xroot.Add(xselect);
            var xSelPars = new XElement(EName.@params);
            xselect.Add(xSelPars);
            var xproc = new XElement(EName.procedure);
            xroot.Add(xproc);
            var xProcPars = new XElement(EName.@params);
            xproc.Add(xProcPars);
            foreach (string paramName in this.orderedParams.Values)
            {
                XElement xpar = new XElement(EName.param, new XAttribute(AName.name, paramName));
                VOracleParameter dbPar = null;
                bool isSel = false;
                bool isProc = false;
                if (this.mainCommand != null)
                {
                    if (this.mainCommand.TryGetParameter(paramName, out dbPar))
                    {
                        dbPar = new VOracleParameter((OracleParameter)mainCommand.Parameters[paramName]);
                        isSel = true;
                    }
                    VForm.WriteAttrAsElem(xselect, EName.query, this.mainCommand.CommandText);
                }
                if (this.procedureCommand != null)
                {
                    if (this.procedureCommand.TryGetParameter(paramName, out dbPar))
                    {
                        isProc = true;
                    }
                    VForm.WriteAttrAsElem(xselect, EName.query, this.procedureCommand.CommandText);
                }
                // Cmn.str
                if (dbPar != null)
                {
                    xpar.SetAttributeValue(AName.type, VReport.GetStringType(dbPar.GetOracleDbType()));
                    if (dbPar.SourceColumn != null)
                    {
                        xpar.SetAttributeValue(AName.column, dbPar.SourceColumn);
                    }
                    xpars.Add(xpar);
                    if (isSel)
                    {
                        xSelPars.Add(xpar);
                    }
                    if (isProc)
                    {
                        xProcPars.Add(xpar);
                    }
                }
            }
            return xroot;
        }
        public static VDBSelectCommand FromXml(XElement xroot)
        {
            var xpars = xroot.Element(EName.@params);
            var xselect = xroot.Element(EName.select);
            var xSelPars = xselect.Element(EName.@params);
            var xproc = xroot.Element(EName.procedure);
            var xProcPars = xproc.Element(EName.@params);
            var cmd = new VDBSelectCommand();
            int i = 0;
            foreach (XElement xpar in xpars.Elements())
            {
                cmd.orderedParams.Add(i, xpar.Attribute(AName.name).Value);
                i++;
            }
            string selectText = VForm.ReadElementAsString(xselect, EName.query);
            if (selectText != null)
            {
                cmd.mainCommand = new VOracleCommand(selectText);
                foreach (XElement xpar in xSelPars.Elements())
                {
                    VOracleParameter par = CreateDBParameter(xpar.Attribute(AName.name).Value, xpar.Attribute(AName.type).Value);
                    if (xpar.Attribute(AName.column) != null)
                    {
                        par.SourceColumn = xpar.Attribute(AName.column).Value;
                    }
                    cmd.mainCommand.Parameters.Add(par);
                }
            }
            string procText = VForm.ReadElementAsString(xproc, EName.query);
            if (procText != null)
            {
                cmd.procedureCommand = new VOracleCommand(procText);
                foreach (XElement xpar in xSelPars.Elements())
                {
                    VOracleParameter par = CreateDBParameter(xpar.Attribute(AName.name).Value, xpar.Attribute(AName.type).Value);
                    XAttribute attr = xpar.Attribute(AName.column);
                    if (attr != null)
                    {
                        par.SourceColumn = attr.Value;
                    }
                    cmd.procedureCommand.Parameters.Add(par);
                }
            }
            return cmd;
        }
        private static VOracleCommand CreateCommand(string command_text, IList<XElement> query_params)
        {
            Contract.Assert(query_params != null);
            VOracleCommand cmd = null;
            try
            {
                cmd = new VOracleCommand();
                cmd.CommandText = command_text;
                cmd.BindByName = true;
                string[] paramNames = Cmn.ExtractParameterNamesFromSQL(command_text);
                foreach (string param_name in paramNames)
                {
                    XElement query_param = query_params.SearchByAttribute(AName.name, param_name);
                    VOracleParameter param;
                    if (query_param != null)
                    {
                        if (!DevUtilsProvider.Instance.IsBuildingTs())
                        {
                            Contract.Assert(query_param.Attribute(AName.type) != null);
                        }

                        string datatype = query_param.Attribute(AName.type)?.Value ?? "Varchar2";
                        param = CreateDBParameter(param_name, datatype);
                        if (query_param.Attribute(AName.is_ret) != null)
                        {
                            param.Direction = ParameterDirection.InputOutput;
                        }
                        XAttribute attr = query_param.Attribute(AName.column);
                        if (attr != null)
                        {
                            param.SourceColumn = attr.Value;
                        }
                    }
                    else
                    {
                        param = CreateDBParameter(param_name, "Varchar2");
                    }
                    cmd.Parameters.Add(param);
                }
            }
            catch (Exception)
            {
                if (cmd != null)
                {
                    Cmn.DisposeAndSetNull<VOracleCommand>(ref cmd);
                }
                throw;
            }
            return cmd;
        }
        /*private void addCommandDBParams(OracleCommand command, XElement query)
        {
            string[] names = Cmn.ExtractParameterNamesFromSQL(command.CommandText);
            VOracleParameter par;
            foreach (XElement xpar in query.Elements(EName.@params).Elements(EName.@param)) {
                string param_name = xpar.Attribute(AName.name).Value;
                if (names.Contains(param_name)) {
                    string datatype = xpar.Attribute(AName.type).Value;
                    par = CreateDBParameter(param_name, datatype);
                    XAttribute attr = xpar.Attribute(AName.column);
                    if (attr != null) {
                        par.SourceColumn = attr.Value;
                    }
                    command.Parameters.Add(par);
                }
            }
            foreach (string name in names) {
                if (TryGetGlobalDbParam(name, out par)) {
                    command.Parameters.Add(par);
                }
            }
        }*/
        private void setCommandParamsValues(VOracleCommand command, VOracleParameter[] pars)
        {
            foreach (VOracleParameter par in command.Parameters.AsVOracleParameters())
            {
                VOracleParameter srcPar = pars.Where(p => p.ParameterName == par.ParameterName).First();
                par.Value = srcPar.Value;
            }
        }
        public DataTable ExecuteDataTable(IList<object> pars, VOracleConnection connection)
        {
            SetParamsValues(pars);
            return ExecuteDataTable(connection);
        }
        public DataTable ExecuteDataTable(VOracleParameter[] pars, VOracleConnection connection)
        {
            if (procedureCommand != null)
            {
                setCommandParamsValues(procedureCommand, pars);
            }
            setCommandParamsValues(mainCommand, pars);
            return ExecuteDataTable(connection);
        }
        public string GetText(VOracleParameter[] pars)
        {
            if (procedureCommand != null)
            {
                setCommandParamsValues(procedureCommand, pars);
            }
            setCommandParamsValues(mainCommand, pars);
            return GetCmdParametrizedText(procedureCommand) + Environment.NewLine + GetCmdParametrizedText(mainCommand);
        }
        private VOracleCommand PrepareToExecute(VOracleConnection connection)
        {
            if (this.procedureCommand != null)
            {
                using (VOracleCommand procedure = GetParametrizedCommand(this.procedureCommand))
                {
                    procedure.Connection = connection;
                    DevUtilsProvider.Instance.AnalyzeExecSql(procedure.CommandText);
                    procedure.ExecuteNonQuery();
                }
            }
            VOracleCommand cmd = GetParametrizedCommand(this.mainCommand);
            cmd.Connection = connection;
            return cmd;
        }
        public static bool TryGetGlobalDbParam(string param_name, out VOracleParameter db_param)
        {
            if (param_name.StartsWith(TextConst.Pfx.GlobParam))
            {
                string global_param_name = param_name.Substring(TextConst.Pfx.GlobParam.Length);
                db_param = new VOracleParameter(param_name, VOracleDbType.Number, XmlReports.GetGlobalParValue(global_param_name) /* !!!временно. нужно изменить чтобы устанавливался во время выполнения */, ParameterDirection.Input);
                return true;
            }
            else
            {
                db_param = null;
                return false;
            }
        }
        //private static void SetGlobalParams(OracleCommand cmd)
        //{
        //var dbPar = new OracleParameter();
        //dbPar.ParameterName = paramName;
        //dbPar.OracleDbType = OracleDbType.Number;
        //var parName2 = paramName.Substring(TextConst.Pfx.GlobParam.Length, paramName.Length - TextConst.Pfx.GlobParam.Length);
        //dbPar.Value = XmlReports.GetGlobalParValue(parName2);
        //}
        public DataTable ExecuteDataTable(VOracleConnection connection)
        {
            DataTable tbl;
            VOracleCommand preparedCmd = this.PrepareToExecute(connection);
            DevUtilsProvider.Instance.AnalyzeExecSql(preparedCmd.CommandText);
            using (VOracleDataAdapter da = new VOracleDataAdapter(preparedCmd))
            {
                tbl = new DataTable();
                da.Fill(tbl);
            }
            return tbl;
        }
        public void ExecuteNonQuery(VOracleConnection connection)
        {
            if (string.IsNullOrEmpty(this.mainCommand.CommandText))
            {
                return;
            }
            using (VOracleCommand command = this.PrepareToExecute(connection))
            {
                DevUtilsProvider.Instance.AnalyzeExecSql(command.CommandText);
                command.ExecuteNonQuery();
                foreach (VOracleParameter par in command.Parameters.AsVOracleParameters())
                {
                    if (par.Direction == ParameterDirection.Output || par.Direction == ParameterDirection.InputOutput)
                    {
                        this.mainCommand.Parameters[par.ParameterName].Value = par.Value;
                    }
                }
            }
        }
        public void ExecuteNonQuery(IList<object> pars, VOracleConnection connection)
        {
            this.SetParamsValues(pars);
            this.ExecuteNonQuery(connection);
        }
        private static VOracleCommand GetParametrizedCommand(VOracleCommand command)
        {
            VOracleCommand newCmd = null;
            newCmd = CopyCommand(command);
            foreach (VOracleParameter par in command.Parameters.AsVOracleParameters())
            {
                if (par.Value is string)
                {
                    if (par.OracleDbType != VOracleDbType.VarChar || (string)par.Value == Cmn.undefinedString)
                    {
                        newCmd.Parameters.Remove(newCmd.Parameters[par.ParameterName]);
                        newCmd.CommandText = newCmd.CommandText.Replace(TextConst.Pfx.Param + par.ParameterName + " ", par.Value.ToString());
                    }
                }
            }
            newCmd.CommandText = Cmn.ClearUndefined(newCmd.CommandText);
            return newCmd;
        }
        public static VOracleCommand CopyCommand(VOracleCommand other)
        {
            var newCmd = new VOracleCommand(other.CommandText, (VOracleConnection)other.Connection);
            for (int index = 0; index < other.Parameters.Count; index++)
            {
                VOracleParameter param = new VOracleParameter((OracleParameter)other.Parameters[index]);
                newCmd.Parameters.Add(new VOracleParameter(param.ParameterName, param.OracleDbType, param.Value, param.Direction));
            }
            return newCmd;
        }
        public static VOracleParameter CreateKeyDBParameter(DataTable table, DataRow row)
        {
            DataColumn column = table.PrimaryKey[0];
            VOracleParameter par = CreateDBParameter(column.ColumnName + TextConst.Pfx.PrimaryKeyParam, column.DataType);
            par.Value = row[column];
            return par;
        }
        public static IList<VOracleParameter> CreateExtensionKeysDBParameters(VDataTable table, DataRow row)
        {
            if (table.ExtensionKeys == null)
            {
                return Array.Empty<VOracleParameter>();
            }
            var list = new List<VOracleParameter>(table.ExtensionKeys.Count);
            foreach (KeyValuePair<string, string> col in table.ExtensionKeys)
            {
                VOracleParameter par = CreateDBParameter(col.Key + TextConst.Pfx.PrimaryKeyParam, table.Columns[col.Value].DataType);
                par.Value = row[col.Value];
                list.Add(par);
            }
            return list;
        }
        public static VOracleParameter CreateKeysDBParameter(DataTable table, DataRow[] rows)// !!! Тест
        {
            DataColumn column = table.PrimaryKey[0];
            VOracleParameter par = new VOracleParameter(column.ColumnName + TextConst.Pfx.PrimaryKeyParam, VOracleDbType.Array);
            object[] arr;
            if (rows.Length == 0)
            {
                arr = Array.Empty<object>();
            }
            else
            {
                arr = new object[rows.Length];
                for (int index = 0; index < rows.Length; index++)
                {
                    arr[index] = rows[index][column];
                }
            }
            var arrayStorage = new ArrayStorage(par.ParameterName);
            arrayStorage.SetValues(arr);
            string val = arrayStorage.GetSql();
            par.Value = val;
            return par;
        }
        public static IList<VOracleParameter> CreateForegnKeyDBParameter(DataTable table, DataRow row)
        {
            if (table.ParentRelations.Count == 0)
            {
                return Array.Empty<VOracleParameter>();
            }
            else
            {
                DataColumn pcolumn = table.ParentRelations[0].ParentColumns[0];
                VOracleParameter par = CreateDBParameter(TextConst.Pfx.ForegnKeyParam + table.ParentRelations[0].ChildColumns[0].ColumnName, pcolumn.DataType);
                par.Value = ((VDataTable)pcolumn.Table).CurrentRow[pcolumn];
                return new VOracleParameter[1] { par };
            }
        }
        public static VOracleParameter CreateNewRowDBParameter(DataRow row)
        {
            object value;
            if (row.RowState == DataRowState.Added)
            {
                value = Cmn.DECIMAL_ONE;
            }
            else
            {
                value = Cmn.DECIMAL_ZERO;
            }
            return new VOracleParameter(TextConst.Pfx.Param + TextConst.DBParams.IsNewRowParam, VOracleDbType.Number, value, ParameterDirection.Input);
        }
        public static VOracleParameter TempRowIdParametr(DataTable table, DataRow row)
        {
            DataColumn column = table.PrimaryKey[0];
            VOracleParameter par = CreateDBParameter(TextConst.DBParams.TempRowId, column.DataType);
            if (row.RowState == DataRowState.Added)
            {
                par.Value = row[column];
            }
            else
            {
                par.Value = DBNull.Value;
            }
            return par;
        }
        public string[] GetParamsNames()
        {
            return Cmn.GetParameterNames(this.mainCommand.Parameters);
        }
        public IList<VOracleParameter> CreateCurValDBParameters(DataRow row)
        {
            var pars = new SortedList<string, VOracleParameter>();
            foreach (VOracleParameter par in mainCommand.Parameters.AsVOracleParameters())
            {
                if (!string.IsNullOrEmpty(par.SourceColumn))
                {
                    DataColumn col = row.Table.Columns[par.SourceColumn];
                    object value;
                    if (row.RowState == DataRowState.Added && row.Table.PrimaryKey.Contains(col))
                    {
                        value = DBNull.Value;
                    }
                    else
                    {
                        value = row[par.SourceColumn];
                    }
                    pars.Add(par.ParameterName, new VOracleParameter(par.ParameterName, par.OracleDbType, value, ParameterDirection.Input));
                }
            }
            if (procedureCommand != null)
            {
                foreach (VOracleParameter par in procedureCommand.Parameters.AsVOracleParameters())
                {
                    if (!string.IsNullOrEmpty(par.SourceColumn))
                    {
                        if (!pars.ContainsKey(par.ParameterName))
                        {
                            /*var par1 = new OracleParameter(par.ParameterName, par.OracleDbType);
                            DataColumn col = row.Table.Columns[par.SourceColumn];
                            if (row.RowState == DataRowState.Added && row.Table.PrimaryKey.Contains(col)) {
                                par1.Value = DBNull.Value;
                            } else {
                                par1.Value = row[par.SourceColumn];
                            }
                            par1.Value = row[par.SourceColumn];
                            pars.Add(par.ParameterName, par1);
                             */
                            pars.Add(par.ParameterName, new VOracleParameter(par.ParameterName, par.OracleDbType, row[par.SourceColumn], ParameterDirection.Input));
                        }
                    }
                }
            }
            return pars.Values;
        }
        private static VOracleParameter CreateDBParameter(string name, Type type)
        {
            VOracleDbType db_type = Cmn.GetDBType(type);
            return new VOracleParameter(name, db_type, ParameterDirection.Input);
        }
        private static VOracleParameter CreateDBParameter(string name, string type)
        {
            VOracleDbType db_type = Cmn.GetDBType(type);
            return new VOracleParameter(name, db_type, ParameterDirection.Input);
        }
        public List<VOracleParameter> ObjParsToOraclePars(IList<object> pars)
        {
            var list = new List<VOracleParameter>(pars.Count);
            for (int index = 0; index < pars.Count; index++)
            {
                object val = pars[index];
                string name = this.orderedParams[index];
                list.Add(new VOracleParameter(name, VOracleDbType.Number, val, ParameterDirection.Input));
            }
            return list;
        }
        private void SetParamsValues(IList<object> pars)
        {
            for (int index = 0; index < pars.Count; index++)
            {
                object val = pars[index];
                this.setParamValue(orderedParams[index], val);
            }
        }
        private void setParamValue(string paramName, object value)
        {
            VOracleParameter dbPar;
            if (this.mainCommand != null)
            {
                if (this.mainCommand.TryGetParameter(paramName, out dbPar))
                {
                    bool is_undefined = (!Cmn.IsNullOrDBNull(value)) && value.ToString() == Cmn.undefinedString;
                    if (dbPar.OracleDbType == VOracleDbType.Array && !is_undefined)
                    {
                        var arrayStorage = new ArrayStorage(paramName);
                        arrayStorage.SetValues((object[])value);
                        value = arrayStorage.GetSql();
                    }
                    if (!is_undefined)
                    {
                        dbPar.Value = value;
                    }
                    else if (dbPar.Direction != ParameterDirection.InputOutput)
                    {
                        dbPar.Value = DBNull.Value;
                    }
                }
            }
            if (this.procedureCommand != null)
            {
                if (procedureCommand.TryGetParameter(paramName, out dbPar))
                {
                    dbPar.Value = value;
                }
            }
        }
        //для отладки
        public static string GetCmdParametrizedText(DbCommand cmd)
        {
            if (cmd == null)
            {
                return string.Empty;
            }
            string s = cmd.CommandText;
            var oraCmd = cmd as OracleCommand;
            var parameters = oraCmd != null ? oraCmd.Parameters.AsVOracleParameters() : Enumerable.Empty<VOracleParameter>();
            foreach (VOracleParameter par in parameters.OrderByDescending(p => p.ParameterName).ToArray())
            {
                string sval = Cmn.ToOracleString(par.Value);
                s = s.Replace(TextConst.Pfx.Param + par.ParameterName, sval);
            }
            return s;
        }
        /// <summary>
        /// Реализация IDisposable
        /// </summary>
        public void Dispose()
        {
            if (this.procedureCommand != null)
            {
                Cmn.DisposeAndSetNull<VOracleCommand>(ref this.procedureCommand);
            }
            if (this.mainCommand != null)
            {
                Cmn.DisposeAndSetNull<VOracleCommand>(ref this.mainCommand);
            }
        }
    }
}