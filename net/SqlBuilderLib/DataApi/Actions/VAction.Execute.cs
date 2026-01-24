using System;
using System.Diagnostics;
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
////using System.Windows.Forms;
using Oracle.ManagedDataAccess.Client;
using sql.builder.WinForms;
using sql.builder.XmlHelpers;
using sql.builder.UI;
using sql.builder.Controls.FormFields; // ListField
//using DevExpress.XtraBars.Ribbon;
//using DevExpress.XtraEditors;
using infoenergo.sys;
using sql.builder.Controls;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal partial class VAction : VSXElement
    {
        private void ExecuteCustomAction(VDataSet dataSet, UIFormC senderForm, VDataTable table, DataRow row,object[] pars)
        {
            //IList<VSXElement> actions = this.GetElementsP(EName.useaction);
            //for (int index = 0; index < actions.Count; index++) {
            //    VUseAction ua = (VUseAction)actions[index];
            //    ua.Execute(dataSet, senderForm, table, row, null,pars);
            //}
        }
        public virtual VAction Action()
        {
            return this;
        }
        internal VAction ActionOrSelf()
        {
            VAction action = this.Action();
            if (action == null) {
                action = this;
            }
            return action;
        }
        private string ActionType()
        {
            string action_type = this.P_ActionType;
            if (!string.IsNullOrEmpty(action_type)) {
                return action_type;
            }
            if (!string.IsNullOrEmpty(this.P_CalledAction)) {
                return this.Action().P_ActionType;
            } else {
                return null;
            }
        }
        internal List<object> GetParamsRuntimeValues(VDataSet dataSet, DataRow row, VDataColumn col)
        {
            var list = new List<object>();
            var factPars = this.Params();
            int ind = 0;
            foreach (VParam formalParam in this.ActionOrSelf().FormalParams()) {
                list.Add(formalParam.GetRuntimeValue(factPars, dataSet, row, col, ind));
                ind++;
            }
            return list;
        }
        protected VQueryCall GetObject()
        {
            VForm r = (this.RootQuery() as VForm);
            if (r != null) {
                return r.MainAndRelatedQueries().FirstOrDefault(e => e.XName == P_CalledObject);
            } else {
                return null;
            }
        }
        internal List<VSXElement> Params()
        {
            var pars = new List<VSXElement>();
            if (this.ActionOrSelf().IsRowAction() && this.P_CalledObject != "") {
                //<useparam name="ur_dogplat_pp.kod_dogplat" />
                VSXElement par = VSXElement.Get(new XElement(EName.useparam));
                VQueryCall obj = this.GetObject();
                string keyColName = obj.Query().KeyColumn().XName;
                VColumn col = obj.UsedColumns().FirstOrDefault(e => e.P_Column == keyColName);
                if (col != null) {
                    keyColName = col.XName;
                }
                par.P_UsedParName = this.P_CalledObject + "." + keyColName;
                pars.Add(par);
            }
            pars.AddRange(this.GetElementsP());
            return pars;
        }
        private void executeClientRefill(VDataSet dataSet, List<object> pars)
        {
            VDBSelectCommand cmd = this.Action().CalledQuery().GetSelectCommand(true);
            var valTbl = cmd.ExecuteDataTable(pars, (OracleConnection)dataSet.GetConnection());
            var targTbl = (VDataTable)dataSet.Tables[P_CalledObject];
            targTbl.DeleteRows(targTbl.Rows.ToArray());
            targTbl.AddNewRowsWithValues(valTbl.Rows.ToArray());
            targTbl.ManualUserChangedData();
        }
        private void executeClientUpdate(VDataSet dataSet, List<object> pars)
        {
            VDataTable targTbl = (VDataTable)dataSet.Tables[this.P_CalledObject];
            //var scheme = XmlReports.Environment.Manager.GetScheme(); // может ли быть Old??
            VDBSelectCommand cmd = this.Action().CalledQuery().GetSelectCommandWithTemp(dataSet, targTbl.KeyDimension);
            IList<DataRow> rows;
            switch (this.P_ActionRows) {
                case TextConst.AVActionRows.Selected:
                    targTbl.GetDataSet().RaiseNeedSelection();
                    rows = targTbl.SelectedRows;
                    break;
                case TextConst.AVActionRows.All:
                    rows = targTbl.Rows.ToArray();
                    break;
                case TextConst.AVActionRows.Current:
                    rows = new DataRow[1] { targTbl.CurrentRow };
                    break;
                default:
                    targTbl.GetDataSet().RaiseNeedSelection();
                    rows = targTbl.SelectedRows;
                    if (List.IsNullOrEmpty(rows)) {
                        if (targTbl.CurrentRow != null) {
                            rows = new DataRow[1] { targTbl.CurrentRow };
                        } else {
                            rows = Array.Empty<DataRow>();
                        }
                    }
                    break;
            }
            //var cmd = Action().CalledQuery().GetSelectCommand();
            foreach (DataRow row in rows) {
                pars[0] = row[targTbl.PrimaryKey[0]];
                var pars1 = cmd.ObjParsToOraclePars(pars);
                // pars1.Add(VDBSelectCommand.CreateKeyDBParameter(targTbl, row));
                pars1.Add(VDBSelectCommand.TempRowIdParametr(targTbl, row));
                pars1.Add(VDBSelectCommand.CreateNewRowDBParameter(row));
                //pars.AddRange(VDBSelectCommand.CreateForegnKeyDBParameter(tbl, row));
                //  pars.AddRange(col.ValueRefreshCommand.CreateCurValDBParameters(row));
                pars1.Add(dataSet.CreateFormIdParametr());
                var valTbl = cmd.ExecuteDataTable(pars1.ToArray(), (OracleConnection)dataSet.GetConnection());
                foreach (DataColumn col in valTbl.Columns) {
                    if (targTbl.Columns.Contains(col.ColumnName)) {
                        if (valTbl.Rows.Count > 0) {
                            row[col.ColumnName] = valTbl.Rows[0][col];
                        } else {
                            row[col.ColumnName] = DBNull.Value;
                        }
                    }
                }
            }
        }
        //private void executeClientAddByForm(VDataSet dataSet, List<object> pars)
        //{
        //    VAction action = this.Action();
        //    using (var form = new frmDynamicEditorOld()) {
        //        VDataTable targTbl = (VDataTable)dataSet.Tables[P_CalledObject];
        //        form.OpenFormForSimpleSelect(action.P_Form, pars.ToArray(), targTbl);
        //        form.ShowDialog();
        //    }
        //}
        internal static void TableUpdate_Add(DataTable target, List<DataRow> sourceRows)
        {
            if (sourceRows.Count == 0) return;

            var srcTable = sourceRows.First().Table;
            var columnsIndexes = new SortedList<int, int>();
            var values = new SortedList<int, object>();
            int i = 0;

            foreach (DataColumn tagCol in target.Columns)
            {
                if (srcTable.Columns.Contains(tagCol.ColumnName))
                {
                    columnsIndexes.Add(i, srcTable.Columns.IndexOf(tagCol.ColumnName));
                }
                values.Add(i, DBNull.Value);
                i++;
            }

            foreach (DataRow srcRow in sourceRows)
            {

                foreach (int i1 in columnsIndexes.Keys)
                {
                    values[i1] = srcRow[columnsIndexes[i1]];
                }

                target.Rows.Add(values.Values.ToArray());
            }
        }
        private static OracleConnection GetConnection(VDataSet ds)
        {
            if (ds != null) {
                return ds.GetConnection();
            } else {
                return XmlReports.Environment.Connection;
            }
        }
        // Реализация action-type="execute-update" call="<имя query>" [ update-target="<имя query-table>" ] [ is-ret="0" ]
		private void executeUpdate(VDataSet dataSet, List<object> pars, VDataTable dataTable, DataRow row)
		{
            VAction act = this.ActionOrSelf();
            VQuery query = act.CalledQuery();
            Contract.Assume(query != null);
            string update_target = act.P_UpdateTarget;
            if (string.IsNullOrEmpty(update_target)) {
                update_target = query.P_UpdateTarget;
            }
            VDBSelectCommand cmd = query.GetUpdateCommand(update_target);
            cmd.ExecuteNonQuery(pars, GetConnection(dataSet));
		    if (row != null) {
                if (row.RowState != DataRowState.Deleted && this.P_IsRet == TextConst.AVBool.True) {
                    dataTable.AddCreatedItem(row[dataTable.PrimaryKey[0]]);
                }
		    }
		}
        // Реализация action-type="execute-add" call="<имя query>" [ update-target="<имя query-table>" ] [ is-ret="0" ]
        private void executeAdd(VDataSet dataSet, List<object> pars, VDataTable dataTable)
        {
            VAction act = this.ActionOrSelf();
            VQuery query = act.CalledQuery();
            Contract.Assume(query != null);
            string update_target = act.P_UpdateTarget;
            if (string.IsNullOrEmpty(update_target)) {
                update_target = query.P_UpdateTarget;
            }
            VDBSelectCommand cmd = query.GetInsertCommand(update_target);
            cmd.ExecuteNonQuery(pars, GetConnection(dataSet));
            if (dataTable != null && this.P_IsRet == TextConst.AVBool.True) {
                dataTable.AddCreatedItem(cmd.GetRetValue());
            }
        }
        // Реализация action-type="execute-delete" call="<имя query>" [ update-target="<имя query-table>" ]
        private void executeDelete(VDataSet dataSet, List<object> pars)
        {
            VAction act = this.ActionOrSelf();
            VQuery query = act.CalledQuery();
            Contract.Assume(query != null);
            string update_target = act.P_UpdateTarget;
            if (string.IsNullOrEmpty(update_target)) {
                update_target = query.P_UpdateTarget;
            }
            VDBSelectCommand cmd = query.GetDeleteCommand(update_target);
            cmd.ExecuteNonQuery(pars, GetConnection(dataSet));
        }
        private void executeCopyByReport(VDataSet dataSet, List<object> pars, VDataTable dataTable)
        {
            VAction act = this.ActionOrSelf();
            VDBSelectCommand cmd = (act.CalledReport() as VReport).GetRepInsertCommand();
            cmd.ExecuteNonQuery(pars, dataSet.GetConnection());
            if (dataTable != null) {
               // dataTable.AddCreatedItem(cmd.GetRetValue());
                dataTable.Refresh();
            }
        }
        private void executeInsertByReport(VDataSet dataSet, List<object> pars, VDataTable dataTable)
        {
            VAction act = this.ActionOrSelf();
            VDBSelectCommand cmd = (act.CalledReport() as VReport).GetRepInsertCommand();
            cmd.ExecuteNonQuery(pars, dataSet.GetConnection());
            if (dataTable != null && this.P_IsRet == TextConst.AVBool.True) {
                dataTable.AddCreatedItem(cmd.GetRetValue());
            }
        }
        // Реализация <action action-type="call-client-method" assembly="..." type-name="..." function="..." />
        // (а так же: action-type="create-by-client-method", action-type="add-by-client-method" и action-type="get-val-with-client-method")
        private object executeClientMethod(IList<object> parameters, VDataSet ds)
        {
            string assembly_name, type_name;
            string method_name = this.AttrOrDefault(AName_.function, null);
            VAction act;
            if (!string.IsNullOrEmpty(method_name)) {
                act = this;
            } else {
                act = this.Action();
                method_name = act.AttrOrEmpty(AName_.function);
                Contract.Assert(!string.IsNullOrEmpty(method_name));
            }
            //Contract.Assert(act.AttrOrDefault(AName_.action_type, null) == TextConst.AVActionType.CallClientMethod);
            type_name = act.AttrOrEmpty(AName_.type_name);
            assembly_name = act.AttrOrEmpty(AName_.assembly);
            /*if (string.IsNullOrEmpty(type_name)) {
                // Для совместимости, чтобы работало указание класса в атрибуте function:
                // <action action-type="call-client-method" [assembly="arbitrage.lib"] function="arbitrage.lib.Run.ShowErrmsg" />
                int pos = method_name.LastIndexOf('.');
                Contract.Assert(pos >= 0); 
                type_name = method_name.Substring(0, pos);
                method_name = method_name.Substring(pos + 1);
            }*/
            return ReflectionHelper.ExecuteStaticMethod(assembly_name, type_name, method_name, parameters, ds);
        }
        private void addByClientMethod(VDataSet dataSet, List<object> pars)
        {
            DataTable valTbl = (DataTable)this.executeClientMethod(pars, dataSet);
            VDBSelectCommand cmd = this.Action().CalledQuery().GetSelectCommand(true);
            // var valTbl = cmd.ExecuteDataTable(pars.ToArray(), (OracleConnection)dataSet.GetConnection());
            VDataTable targTbl = (VDataTable)dataSet.Tables[this.P_CalledObject];
            //targTbl.DeleteRows(targTbl.Rows.ToArray());
            targTbl.AddNewRowsWithValues(valTbl.Rows.ToArray());
            targTbl.ManualUserChangedData();
        }
        /*public static IEnumerable<Tuple<object, string>>  getValueWithClientMethodTest()
        {
            var list = new List<Tuple<object, string>>();

            list.Add(new Tuple<object, string>(1,"Привет!"));
            list.Add(new Tuple<object, string>(2, "Ура!"));
            MessageBox.Show("1");
            return list.ToArray();
        }*/
        private void getValueWithClientMethod(List<object> pars, VDataTable table)
        {
            //var method = this.CalledMethod();
            //var ind = method.LastIndexOf(".");
            //var class_name = method.Substring(0, ind);
            //var method_name = method.Substring(ind + 1, method.Length - (ind + 1));
            //var list= (object[])ReflectionHelper.ExecuteStaticMethod(class_name, method_name, pars.ToArray());
            var list = (object[])executeClientMethod(pars,table.GetDataSet());
            table.Rows.Clear();
            foreach (Tuple<object, string> row in list) {
                table.Rows.Add(row.Item1, row.Item2);
            }
        }
    }

 
}
