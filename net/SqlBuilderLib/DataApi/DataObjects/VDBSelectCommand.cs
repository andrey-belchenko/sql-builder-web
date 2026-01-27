using System;
using System.Diagnostics.Contracts;
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
using System.IO;
using System.Data.Common;
////using System.Windows.Forms;
using Devart.Data.Oracle;
//using DevExpress.XtraVerticalGrid;
using sql.builder.Core;
//using sql.builder.Test;
using SqlBuilderLib.DevTools;
using sql.builder.Clean;

namespace sql.builder.DataApi
{
    internal sealed class VDBSelectCommand : IDisposable
    {
        #region static stuff
        private static Dictionary<string, object> _queryResults = new Dictionary<string, object>();
        internal static object GetQueryScalarResult(string queryName, bool useChash = true)
        {
            if (useChash && _queryResults.ContainsKey(queryName)) {
                return _queryResults[queryName];
            }
            var res = db.ExecuteObject( XmlReports.Environment.GetQuery(queryName).GetSql());
            if (useChash) {
                _queryResults.Add(queryName, res);
            }
            return res;
        }
        #endregion
        #region поля
        private OracleCommand mainCommand;
        private OracleCommand procedureCommand;
        private SortedList<int, string> orderedParams;
        #endregion
        internal string GetCommandText()
        {
            return this.mainCommand.CommandText;
        }
        internal void SetCommandText(string value)
        {
            this.mainCommand.CommandText = value;
        }
        internal void CreateRetParam()
        {
            var par = new OracleParameter(TextConst.DBParams.PrimaryKeyParam, OracleDbType.Number, ParameterDirection.Output);
            this.mainCommand.Parameters.Add(par);
        }
        internal object GetRetValue()
        {
            return this.mainCommand.Parameters[TextConst.DBParams.PrimaryKeyParam].Value;
        }
        internal int GetParamIndex(string paramName)
        {
            foreach (KeyValuePair<int, string> p in this.orderedParams) {
                if (p.Value == paramName) {
                    return p.Key;
                }
            }
            return -1;
        }
        internal SortedList<int, object> GetRetValues()
        {
            var list = new SortedList<int, object>();
            foreach (DbParameter par in this.mainCommand.Parameters) {
                if (par.Direction == ParameterDirection.Output || par.Direction == ParameterDirection.InputOutput) {
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
            DevAnalyzer.AnalyzePrepSql(commandText);
            DevAnalyzer.AnalyzePrepSql(procedureText);
            IList<XElement> query_params = formalParsSource.Elements(EName.@params).Elements(EName.param).ToList();
            this.mainCommand = CreateCommand(commandText, query_params);
            if (!string.IsNullOrEmpty(procedureText)) {
                this.procedureCommand = CreateCommand(procedureText, query_params);
            } else {
                this.procedureCommand = null;
            }
            this.setOrderedParams(query_params);
        }
        internal VDBSelectCommand(string commandText, XElement formalParsSource)
            : this(commandText, null, formalParsSource)
        {
        }
        internal static VDBSelectCommand CreateFromCompiledQuery(XElement query, XElement compiledQuery)
        {
            string selectText = Compiler.GetQuerySelectStatmentFromCompiledQuery(compiledQuery);
            string procedureText = Compiler.GetQuerProcedureFromCompiledQuery(compiledQuery);
            return new VDBSelectCommand(selectText, procedureText, query);
        }
        #endregion
        private void setOrderedParams(IList<XElement> xparams)
        {
            int index = 0;
            while (index < xparams.Count) {
                XElement xpar = xparams[index];
                string param_name = xpar.Attribute(AName.name).Value;
                this.orderedParams.Add(index, param_name);
                index++;
            }
            for (int index_2 = 0; index_2 < this.mainCommand.Parameters.Count; index_2++) {
                string param_name = this.mainCommand.Parameters[index_2].ParameterName;
                if (!this.orderedParams.ContainsValue(param_name)) {
                    this.orderedParams.Add(index, param_name);
                    index++;
                }
            }
        }
        internal XElement ToXml()
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
            foreach (string paramName in this.orderedParams.Values) {
                XElement xpar = new XElement(EName.param, new XAttribute(AName.name, paramName));
                OracleParameter dbPar = null;
                bool isSel = false;
                bool isProc = false;
                if (this.mainCommand != null) {
                    if (this.mainCommand.Parameters.TryGetParameter(paramName, out dbPar)) {
                        dbPar = mainCommand.Parameters[paramName];
                        isSel = true;
                    }
                    VForm.WriteAttrAsElem(xselect, EName.query, this.mainCommand.CommandText);
                }
                if (this.procedureCommand != null) {
                    if (this.procedureCommand.Parameters.TryGetParameter(paramName, out dbPar)) {
                        isProc = true;
                    }
                    VForm.WriteAttrAsElem(xselect, EName.query, this.procedureCommand.CommandText);
                }
                // Cmn.str
                if (dbPar != null) {
                    xpar.SetAttributeValue(AName.type, VReport.GetStringType(dbPar.OracleDbType));
                    if (dbPar.SourceColumn != null) {
                        xpar.SetAttributeValue(AName.column, dbPar.SourceColumn);
                    }
                    xpars.Add(xpar);
                    if (isSel) {
                        xSelPars.Add(xpar);
                    }
                    if (isProc) {
                        xProcPars.Add(xpar);
                    }
                }
            }
            return xroot;
        }
        internal static VDBSelectCommand FromXml(XElement xroot)
        {
            var xpars = xroot.Element(EName.@params);
            var xselect = xroot.Element(EName.select);
            var xSelPars = xselect.Element(EName.@params);
            var xproc = xroot.Element(EName.procedure);
            var xProcPars = xproc.Element(EName.@params);
            var cmd = new VDBSelectCommand();
            int i = 0;
            foreach (XElement xpar in xpars.Elements()) {
                cmd.orderedParams.Add(i, xpar.Attribute(AName.name).Value);
                i++;
            }
            string selectText = VForm.ReadElementAsString(xselect, EName.query);
            if (selectText != null) {
                cmd.mainCommand = new VOracleCommand(selectText);
                foreach (XElement xpar in xSelPars.Elements()) {
                    OracleParameter par = CreateDBParameter(xpar.Attribute(AName.name).Value, xpar.Attribute(AName.type).Value);
                    if (xpar.Attribute(AName.column) != null) {
                        par.SourceColumn = xpar.Attribute(AName.column).Value;
                    }
                    cmd.mainCommand.Parameters.Add(par);
                }
            }
            string procText = VForm.ReadElementAsString(xproc, EName.query);
            if (procText != null) {
                cmd.procedureCommand = new VOracleCommand(procText);
                foreach (XElement xpar in xSelPars.Elements()) {
                    OracleParameter par = CreateDBParameter(xpar.Attribute(AName.name).Value, xpar.Attribute(AName.type).Value);
                    XAttribute attr = xpar.Attribute(AName.column);
                    if (attr != null) {
                        par.SourceColumn = attr.Value;
                    }
                    cmd.procedureCommand.Parameters.Add(par);
                }
            }
            return cmd;
        }
        private static OracleCommand CreateCommand(string command_text, IList<XElement> query_params)
        {
            Contract.Assert(query_params != null);
            OracleCommand cmd = null;
            try {
                cmd = new VOracleCommand();
                cmd.ParameterCheck = true; // чтобы коллекция Parameters заполнилась при установке CommandText
                cmd.CommandText = command_text;
                DevAnalyzer.AnalyzePrepSql(command_text);
                // Устанавливаем параметры
                for (int index = 0; index < cmd.Parameters.Count; index++) {
                    OracleParameter param = cmd.Parameters[index];
                    Contract.Assert(param.Direction == ParameterDirection.Input);
                    string param_name = param.ParameterName;
                    //if (param_name.StartsWith(TextConst.Pfx.GlobParam)) {
                    //    string global_param_name = param_name.Substring(TextConst.Pfx.GlobParam.Length);
                    //    param.OracleDbType = OracleDbType.Number; // Пока все глобальные параметры числовые
                    //    param.Value = XmlReports.GetGlobalParValue(global_param_name); // !!!временно. нужно изменить чтобы устанавливался во время выполнения
                    //} else {
                        XElement query_param = query_params.SearchByAttribute(AName.name, param_name);
                        if (query_param != null) {
                            Contract.Assert(query_param.Attribute(AName.type) != null);
                            string datatype = query_param.Attribute(AName.type).Value;
                            param.OracleDbType = Cmn.GetDBType(datatype);
                            if (query_param.Attribute(AName.is_ret) != null) {
                                param.Direction = ParameterDirection.InputOutput;
                            }
                            XAttribute attr = query_param.Attribute(AName.column);
                            if (attr != null) {
                                param.SourceColumn = attr.Value;
                            }
                        }
                    //}
                }
            } catch (Exception) {
                if (cmd != null) {
                    Cmn.DisposeAndSetNull<OracleCommand>(ref cmd);
                }
                throw;
            }
            return cmd;
        }
        /*private void addCommandDBParams(OracleCommand command, XElement query)
        {
            string[] names = Cmn.ExtractParameterNamesFromSQL(command.CommandText);
            OracleParameter par;
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
        private void setCommandParamsValues(OracleCommand command, OracleParameter[] pars)
        {
            foreach (OracleParameter par in command.Parameters) {
                OracleParameter srcPar = pars.Where(p => p.ParameterName == par.ParameterName).First();
                par.Value = srcPar.Value;
            }
        }
        internal DataTable ExecuteDataTable(IList<object> pars, OracleConnection connection)
        {
            SetParamsValues(pars);
            return ExecuteDataTable(connection);
        }
        internal DataTable ExecuteDataTable(OracleParameter[] pars, OracleConnection connection)
        {
            if (procedureCommand != null) {
                setCommandParamsValues(procedureCommand, pars);
            }
            setCommandParamsValues(mainCommand, pars);
            return ExecuteDataTable(connection);
        }
        internal string GetText(OracleParameter[] pars)
        {
            if (procedureCommand != null) {
                setCommandParamsValues(procedureCommand, pars);
            }
            setCommandParamsValues(mainCommand, pars);
            return GetCmdParametrizedText(procedureCommand) + Environment.NewLine + GetCmdParametrizedText(mainCommand);
        }
        private OracleCommand PrepareToExecute(OracleConnection connection)
        {
            if (this.procedureCommand != null) {
                using (OracleCommand procedure = GetParametrizedCommand(this.procedureCommand)) {
                    procedure.Connection = connection;
                    DevAnalyzer.AnalyzeSql(procedure.CommandText);
                    procedure.ExecuteNonQuery();
                }
            }
            OracleCommand cmd = GetParametrizedCommand(this.mainCommand);
            cmd.Connection = connection;
            return cmd;
        }
        internal static bool TryGetGlobalDbParam(string param_name, out OracleParameter db_param)
        {
            if (param_name.StartsWith(TextConst.Pfx.GlobParam))
            {
                string global_param_name = param_name.Substring(TextConst.Pfx.GlobParam.Length);
                db_param = new OracleParameter(param_name, OracleDbType.Number, XmlReports.GetGlobalParValue(global_param_name) /* !!!временно. нужно изменить чтобы устанавливался во время выполнения */, ParameterDirection.Input);
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
        internal DataTable ExecuteDataTable(OracleConnection connection)
        {
            DataTable tbl;
            OracleCommand preparedCmd = this.PrepareToExecute(connection);
            DevAnalyzer.AnalyzeSql(preparedCmd.CommandText);
            using (OracleDataAdapter da = new OracleDataAdapter(preparedCmd)) {
                tbl = new DataTable();
                da.Fill(tbl);
            }
            return tbl;
        }
        internal void ExecuteNonQuery(OracleConnection connection)
        {
            if (string.IsNullOrEmpty(this.mainCommand.CommandText)) {
                return;
            }
            using (OracleCommand command = this.PrepareToExecute(connection)) {
                DevAnalyzer.AnalyzeSql(command.CommandText);
                command.ExecuteNonQuery();
                foreach (OracleParameter par in command.Parameters) {
                    if (par.Direction == ParameterDirection.Output || par.Direction == ParameterDirection.InputOutput) {
                        this.mainCommand.Parameters[par.ParameterName].Value = par.Value;
                    }
                }
            }
        }
        internal void ExecuteNonQuery(IList<object> pars, OracleConnection connection)
        {
            this.SetParamsValues(pars);
            this.ExecuteNonQuery(connection);
        }
        private static OracleCommand GetParametrizedCommand(OracleCommand command)
        {
            OracleCommand newCmd = null;
            newCmd = CopyCommand(command);
            foreach (OracleParameter par in command.Parameters) {
                if (par.Value is string) {
                    if (par.OracleDbType != OracleDbType.VarChar || (string)par.Value == Cmn.undefinedString) {
                        newCmd.Parameters.Remove(newCmd.Parameters[par.ParameterName]);
                        newCmd.CommandText = newCmd.CommandText.Replace(TextConst.Pfx.Param + par.ParameterName + " ", par.Value.ToString());
                    }
                }
            }
            newCmd.CommandText = Cmn.ClearUndefined(newCmd.CommandText);
            return newCmd;
        }
        internal static OracleCommand CopyCommand(OracleCommand other)
        {
            var newCmd = new VOracleCommand(other.CommandText, other.Connection);
            for (int index = 0; index < other.Parameters.Count; index++) {
                OracleParameter param = other.Parameters[index];
                newCmd.Parameters.Add(param.ParameterName, param.OracleDbType, param.Value, param.Direction);
            }
            return newCmd;
        }
        internal static OracleParameter CreateKeyDBParameter(DataTable table, DataRow row)
        {
            DataColumn column = table.PrimaryKey[0];
            OracleParameter par = CreateDBParameter(column.ColumnName + TextConst.Pfx.PrimaryKeyParam, column.DataType);
            par.Value = row[column];
            return par;
        }
        internal static IList<OracleParameter> CreateExtensionKeysDBParameters(VDataTable table, DataRow row)
        {
            if (table.ExtensionKeys == null) {
                return Array.Empty<OracleParameter>();
            }
            var list = new List<OracleParameter>(table.ExtensionKeys.Count);
            foreach (KeyValuePair<string, string> col in table.ExtensionKeys) {
                OracleParameter par = CreateDBParameter(col.Key + TextConst.Pfx.PrimaryKeyParam, table.Columns[col.Value].DataType);
                par.Value = row[col.Value];
                list.Add(par);
            }
            return list;
        }
        internal static OracleParameter CreateKeysDBParameter(DataTable table, DataRow[] rows)// !!! Тест
        {
            DataColumn column = table.PrimaryKey[0];
            OracleParameter par = new OracleParameter(column.ColumnName + TextConst.Pfx.PrimaryKeyParam, OracleDbType.Array);
            object[] arr;
            if (rows.Length == 0) {
                arr = Array.Empty<object>();
            } else {
                arr = new object[rows.Length];
                for (int index = 0; index < rows.Length; index++) {
                    arr[index] = rows[index][column];
                }
            }
            var arrayStorage = new ArrayStorage(par.ParameterName);
            arrayStorage.SetValues(arr);
            string val = arrayStorage.GetSql();
            par.Value = val;
            return par;
        }
        internal static IList<OracleParameter> CreateForegnKeyDBParameter(DataTable table, DataRow row)
        {
            if (table.ParentRelations.Count == 0) {
                return Array.Empty<OracleParameter>();
            } else {
                DataColumn pcolumn = table.ParentRelations[0].ParentColumns[0];
                OracleParameter par = CreateDBParameter(TextConst.Pfx.ForegnKeyParam + table.ParentRelations[0].ChildColumns[0].ColumnName, pcolumn.DataType);
                par.Value = ((VDataTable)pcolumn.Table).CurrentRow[pcolumn];
                return new OracleParameter[1] { par };
            }
        }
        internal static OracleParameter CreateNewRowDBParameter(DataRow row)
        {
            object value;
            if (row.RowState == DataRowState.Added) {
                value = Cmn.DECIMAL_ONE;
            } else {
                value = Cmn.DECIMAL_ZERO;
            }
            return new OracleParameter(TextConst.Pfx.Param + TextConst.DBParams.IsNewRowParam, OracleDbType.Number, value, ParameterDirection.Input);
        }
        internal static OracleParameter TempRowIdParametr(DataTable table, DataRow row)
        {
            DataColumn column = table.PrimaryKey[0];
            OracleParameter par = CreateDBParameter(TextConst.DBParams.TempRowId, column.DataType);
            if (row.RowState == DataRowState.Added) {
                par.Value = row[column];
            } else {
                par.Value = DBNull.Value;
            }
            return par;
        }
        internal string[] GetParamsNames()
        {
            return Cmn.GetParameterNames(this.mainCommand.Parameters);
        }
        internal IList<OracleParameter> CreateCurValDBParameters(DataRow row)
        {
            var pars = new SortedList<string, OracleParameter>();
            foreach (OracleParameter par in mainCommand.Parameters) {
                if (!string.IsNullOrEmpty(par.SourceColumn)) {
                    DataColumn col = row.Table.Columns[par.SourceColumn];
                    object value;
                    if (row.RowState == DataRowState.Added && row.Table.PrimaryKey.Contains(col)) {
                        value = DBNull.Value;
                    } else {
                        value = row[par.SourceColumn];
                    }
                    pars.Add(par.ParameterName, new OracleParameter(par.ParameterName, par.OracleDbType, value, ParameterDirection.Input));
                }
            }
            if (procedureCommand != null) {
                foreach (OracleParameter par in procedureCommand.Parameters) {
                    if (!string.IsNullOrEmpty(par.SourceColumn)) {
                        if (!pars.ContainsKey(par.ParameterName)) {
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
                            pars.Add(par.ParameterName, new OracleParameter(par.ParameterName, par.OracleDbType, row[par.SourceColumn], ParameterDirection.Input));
                        }
                    }
                }
            }
            return pars.Values;
        }
        private static OracleParameter CreateDBParameter(string name, Type type)
        {
            OracleDbType db_type = Cmn.GetDBType(type);
            return new OracleParameter(name, db_type, ParameterDirection.Input);
        }
        private static OracleParameter CreateDBParameter(string name, string type)
        {
            OracleDbType db_type = Cmn.GetDBType(type);
            return new OracleParameter(name, db_type, ParameterDirection.Input);
        }        
        internal List<OracleParameter> ObjParsToOraclePars(IList<object> pars)
        {
            var list = new List<OracleParameter>(pars.Count);
            for (int index = 0; index < pars.Count; index++) {
                object val = pars[index];
                string name = this.orderedParams[index];
                list.Add(new OracleParameter(name, OracleDbType.Number, val, ParameterDirection.Input));
            }
            return list;
        }
        private void SetParamsValues(IList<object> pars)
        {
            for (int index = 0; index < pars.Count; index++) {
                object val = pars[index];
                this.setParamValue(orderedParams[index], val);
            }
        }
        private void setParamValue(string paramName, object value)
        {
            OracleParameter dbPar;
            if (this.mainCommand != null) {
                if (this.mainCommand.Parameters.TryGetParameter(paramName, out dbPar)) {
                    bool is_undefined = (!Cmn.IsNullOrDBNull(value)) && value.ToString() == Cmn.undefinedString;
                    if (dbPar.OracleDbType == OracleDbType.Array && !is_undefined) {
                        var arrayStorage = new ArrayStorage(paramName);
                        arrayStorage.SetValues((object[])value);
                        value = arrayStorage.GetSql();
                    }
                    if (!is_undefined) {
                        dbPar.Value = value;
                    } else if (dbPar.Direction != ParameterDirection.InputOutput) {
                        dbPar.Value = DBNull.Value;
                    }
                }
            }
            if (this.procedureCommand != null) {
                if (procedureCommand.Parameters.TryGetParameter(paramName, out dbPar)) {
                    dbPar.Value = value;
                }
            }
        }
        //для отладки
        internal static string GetCmdParametrizedText(DbCommand cmd)
        {
            if (cmd == null) {
                return string.Empty;
            }
            string s = cmd.CommandText;
            foreach (OracleParameter par in cmd.Parameters.Cast<OracleParameter>().OrderByDescending(Cmn.GetParameterName).ToArray()) {
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
            if (this.procedureCommand == null) {
                Cmn.DisposeAndSetNull<OracleCommand>(ref this.procedureCommand);
            }
            if (this.mainCommand == null) {
                Cmn.DisposeAndSetNull<OracleCommand>(ref this.procedureCommand);
            }
        }
    }
}