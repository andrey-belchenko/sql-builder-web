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
using Devart.Data.Oracle;
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
            //foreach (DataRow r in valTbl.Rows)
            //{
            //    var newRow = targTbl.AddNewRowWithValues(r);
            //   // targTbl.UpdateRowValues(newRow, r);
            //    //foreach (DataColumn col in valTbl.Columns)
            //    //{
            //    //    if (targTbl.Columns.Contains(col.ColumnName))
            //    //    {
            //    //        newRow[col.ColumnName] = r[col];
            //    //    }
            //    //}
            //}
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

   //     private void openReportColGrDetail(List<object> pars, VDataTable table, DataRow row,DataColumn col, bool applyReportConds, ucMainReports reportsForm, UIFormC paramsForm)
   //     {
   //         XElement query = XmlReports.Environment.GetPrecompiledQuery(this.ActionOrSelf().P_Report);
   //         query = new XElement(query);
   //         query.Attributes(TextConst.AName.Name).Remove();



   //         var grsetid = row[TextConst.AVSpecColumnGrset.GrSetName].ToString();

   //         var xgroupingQuery = XmlReports.Environment.GetQuery(table.QueryName).AsXElementApplyingParts();

   //         while (!xgroupingQuery.Elements(TextConst.EName.Grouping).Any() && !xgroupingQuery.Descendants(TextConst.EName.Grsets).Any())
   //         {
   //             var srcCall = xgroupingQuery.Elements(TextConst.EName.From).Elements().First();
   //             var xfactPars = srcCall.Elements(TextConst.EName.WithParams).FirstOrDefault();
   //             xgroupingQuery = XmlReports.Environment.GetQuery(Cmn.GetAttrValue(srcCall, TextConst.AName.Name)).AsXElementApplyingParts();
   //             if (xfactPars != null)
   //             {
   //                 xgroupingQuery = (XElement)Compiler.applyParams(xgroupingQuery, xfactPars, false).FirstOrDefault();
   //             }
               
   //         }
   //         Compiler.PreCompileQuery(xgroupingQuery, true);
          
   //         if (xgroupingQuery.Element(TextConst.EName.Grouping) != null)
   //         {
   //             xgroupingQuery = new XElement(xgroupingQuery);
   //             Compiler.addColumnsAlias(xgroupingQuery, false, true);
   //             Compiler.prepareSelfGrsets(xgroupingQuery, null);
   //         }

			//bool DetailsUseZeros;
			//if (this.ActionOrSelf().P_DetailsUseZeros == TextConst.AVBool.True)
			//{
			//	DetailsUseZeros = true;
			//}
			//else
			//{
			//	DetailsUseZeros = false;
			//}

			//VGroupingUtils.ExtendQueryAsGroupingDetail(query, xgroupingQuery, grsetid, row, col, applyReportConds, DetailsUseZeros);
   //         XElement report = VReport.getReportOrQuery(query);
   //         report.SetAttributeValue(TextConst.AName.Name, this.ActionOrSelf().P_Report);
   //         reportsForm.OpenReport(report, pars.ToArray(),parentParamsForm:paramsForm);
   //     }

        //private void openReportGrDetail(List<object> pars, VDataTable table, DataRow row, ucMainReports reportsForm,UIFormC paramsForm=null)
        //{

        //    openReportColGrDetail(pars, table, row, null,false, reportsForm,paramsForm);
            
        //}


   //     private void openExpressReport(List<object> pars)
   //     {
   //         var report = new ExpressReport();
   //         report.Show(ActionOrSelf().P_Report);
   //     }

   //     private void openReport(List<object> pars, ucMainReports reportsForm, UIFormC paramsForm = null)
   //     {
   //         reportsForm.OpenReport(this.ActionOrSelf().P_Report, pars.ToArray(), true);
   //     }
   //     private void executePlSqlMethod(VDataSet dataSet, List<object> pars, VDataTable dataTable = null, bool isInsert = false)
   //     {
   //         VAction act = this.ActionOrSelf();
   //         var cmd = new VDBSelectCommand(act.P_Text, act);
   //         OracleConnection connection = GetConnection(dataSet);
   //         cmd.ExecuteNonQuery(pars, connection);
   //         connection.Commit();
   //         SortedList<int, object> retVals = cmd.GetRetValues();
   //         if (!isInsert) {
   //             if (dataSet != null) {
   //                 foreach (int i in retVals.Keys) {
   //                     XElement xel = this.Elements().ElementAt(i);
   //                     if (xel.Name == EName.useparam) {
   //                         string parName = xel.AttrOrEmpty(AName_.name);
   //                         dataSet.SetVariableValue(parName, retVals[i], true);
   //                     }
   //                 }
   //             }
   //         } else {
   //             if (dataTable != null && this.P_IsRet == TextConst.AVBool.True) {
   //                 dataTable.AddCreatedItem(retVals.First().Value);
   //             }
   //         }
   //         // dataTable.AddCreatedItem(cmd.GetRetValue());
   //         //var method = this.CalledMethod();
   //         //var ind = method.LastIndexOf(".");
   //         //var class_name = method.Substring(0, ind);
   //         //var method_name = method.Substring(ind + 1, method.Length - (ind + 1));
   //         //XmlReflection.ExecuteStaticMethod(class_name, method_name, pars.ToArray());
   //         //MessageBox.Show();
   //     }
   //     #region Infoenergo.exe
   //     private bool InfoenergoFindAndRefreshForm(frmDynamicEditor dform)
   //     {
   //         if (dform == null) return false;
   //         UIFormC[] uiforms = Cmn.GetChildControlsOfType<UI.WinForms.UIFormControl>(dform).Select(c => c.GetVForm()).ToArray();
   //         bool form_exists = uiforms.Length != 0 && Global.MainForm.MdiChildren.OfType<frmDynamicEditor>().Any(f => f == dform);
   //         if (form_exists) {
   //             foreach (var f in uiforms.SelectMany(f => f.GetRelativeFormsAndSelf())
   //                 .Where(f => f.AutoRefresh)
   //                 .Distinct()) f.RefreshData();

   //             return true;
   //         } else {
   //             return false;
   //         }
   //     }
   //     private bool InfoenergoFindAndActivateTab(UIFormC uiform, object[] pars, bool isCreation)
   //     {
   //         var form_ctrl = uiform.TmpGetControlAsWinFormCtrl() as Control;
   //         frmDynamicEditor dform = Global.MainForm.MdiChildren.OfType<frmDynamicEditor>().FirstOrDefault(f => (f == form_ctrl.Parent));
   //         if (dform != null)
   //         {
   //             Form active_form = Global.MainForm.ActiveMdiChild;

   //             dform.Activate();
   //             bool success = uiform.SaveDataWithCheckModified();
   //             if (uiform.Init && success)
   //             {
   //                 if (isCreation)
   //                 {
   //                     uiform.LayoutSuspend();
   //                     var calledTbl = uiform.DataSource.TopTable.First();
   //                     var row = calledTbl.Rows.Add();
   //                     calledTbl.CurrentRow = row;
   //                     uiform.DataSource.KeyParamName = uiform.DataSource.InputParams.Keys.First();
   //                     uiform.LayoutResume();
   //                 }
   //                 UIStatic.UpdateForm(uiform, pars, isCreation, false);

   //                 //if (uiform is UIFormC2)
   //                 //{
   //                 //    Cmn.GetChildControlsOfType<UIFormC2Control>(dform)
   //                 //      .SelectMany(c => c.Form.GetRelativeFormsAndSelf())
   //                 //      .Where(f => f.AutoRefresh)
   //                 //      .Distinct()
   //                 //      .ForEach(f => f.RefreshData());
   //                 //}
   //                 //else
   //                 //{
   //                 foreach (var f in Cmn.GetChildControlsOfType<sql.builder.UI.WinForms.UIFormControl>(dform)
   //                     .SelectMany(f => f.GetVForm().GetRelativeFormsAndSelf())
   //                     .Where(f => f.AutoRefresh)
   //                     .Distinct()) f.RefreshData();
   //                 //}
   //             }
   //             else
   //             {
   //                 if (active_form != null) active_form.Activate();
   //             }

   //             return true;
   //         }

   //         return false;
   //     }
   //     #endregion
   //     public object Execute(VDataSet dataSet, UIFormC senderForm, VDataTable table, DataRow inrow, VDataColumn col, object[] pars = null, frmDynamicEditor dform = null, ucMainReports reportsForm = null,UIFormC paramsForm=null,Form mdiParentForm=null)
   //     {
   //         object ret = null;
   //         string prompt = this.P_Prompt;
   //         if (!string.IsNullOrEmpty(prompt)) {
   //             DialogResult result = ShowMessage.ShowQuestion(senderForm.ReplaceParameters(prompt));
   //             if (result != DialogResult.Yes) {
   //                 return ret;
   //             }
   //         }
   //         // чтобы dform не заполнялся повторно в инфоэнерго exe
   //         if (dform != null && XmlReports.IsInfoenergo) {
   //             if (InfoenergoFindAndRefreshForm(dform)) return ret;
   //         }
   //         WaitUIHelper.LastUsedUIHelper.Show("Выполнение операции", WaitUIMode.WaitCursor);
   //         if (this.P_ActionType == TextConst.AVActionType.Custom) {
   //             ExecuteCustomAction(dataSet, senderForm, table, inrow,pars);
   //             goto Finish;
   //         }
   //         VAction action = this.ActionOrSelf();
   //         // ищем тип среди всех загруженных на данный момент
   //         string actionType = this.ActionType();
   //         if (actionType == TextConst.AVActionType.Custom) {
   //             action.ExecuteCustomAction(dataSet, senderForm, table, inrow,pars);
   //             goto Finish;
   //         }
   //         List<object> pars1 = null;
   //         if (pars != null) {
   //             pars1 = pars.ToList();
   //         } else {
   //             pars1 = this.GetParamsRuntimeValues(dataSet, inrow, col);
   //         }
   //         Form form = null;
   //         UIFormC uiform = null;
   //         VDataTable calledTbl = null;
   //         bool isItemCreate = false;
   //         bool isModalForm = this.P_Modal == TextConst.AVBool.True;
   //         bool isMultipleCreate = false;
   //         bool isSelectByForm = false;
   //         object[] createdKeys = null;
			//var ctrlName = CalledControlX();
   //         VDataTable tbl = null;
   //         switch (actionType) {
   //             case TextConst.AVActionType.Form:
   //                 string type_name = this.AttrOrEmpty(AName_.type_name);
   //                 string assembly_name = this.AttrOrEmpty(AName_.assembly);
   //                 Type formType;
   //                 ReflectionHelper.ResolveAssemblyType(assembly_name, type_name, out formType);
   //                 if (formType != null) {
   //                     form = (Form)Activator.CreateInstance(formType, pars1.ToArray());
   //                 } else {
   //                     ShowMessage.ShowError(string.Format("Не удалось запустить приложение \"{0}\"", action.P_Title));
   //                 }
   //                 break;
   //             case TextConst.AVActionType.DynamicForm:
   //                 //uiform = ucDataEditorMain.CreateForm(action.P_Call, pars1.ToArray(), false, isModalForm, isUnique);
   //                 uiform = UIStatic.GetForm(action.P_Call, isModalForm, true);
   //                 // если уже открыто в infoenergo exe - просто активируем вкладку
   //                 if (dform == null && XmlReports.IsInfoenergo) {
   //                     if (InfoenergoFindAndActivateTab(uiform, pars1.ToArray(), false)) {
   //                         WaitUIHelper.LastUsedUIHelper.Hide();
   //                         return ret;
   //                     }
   //                 }
   //                 if (uiform.Init) {
   //                     //if (!isModalForm && ucDataEditorMain.GetInstance() != null) ucDataEditorMain.GetInstance().ShowForm(uiform);
   //                     bool success = UIStatic.UpdateForm(uiform, pars1.ToArray(), false);
   //                     if (!success) {
   //                         WaitUIHelper.LastUsedUIHelper.Hide();
   //                         return ret;
   //                     }
   //                 }
   //                 break;
   //             case TextConst.AVActionType.DynamicFormCreate:
   //                 //uiform = ucDataEditorMain.CreateForm(action.P_Call, pars1.ToArray(), false, isModalForm, isUnique);
   //                 uiform = UIStatic.GetForm(action.P_Call, isModalForm, true);
   //                 isItemCreate = true;
   //                 calledTbl = uiform.DataSource.TopTable.First();
   //                 // если уже открыто в infoenergo exe - просто активируем вкладку
   //                 if (dform == null && XmlReports.IsInfoenergo) {
   //                     if (InfoenergoFindAndActivateTab(uiform, pars1.ToArray(), true)) {
   //                         WaitUIHelper.LastUsedUIHelper.Hide();
   //                         return ret;
   //                     }
   //                 }
   //                 if (uiform.Init) {
   //                    // if (ucDataEditorMain.GetInstance() != null) ucDataEditorMain.GetInstance().ShowForm(uiform);
   //                     bool success = uiform.SaveDataWithCheckModified();
   //                     if (!success) {
   //                         WaitUIHelper.LastUsedUIHelper.Hide();
   //                         return ret;
   //                     }
   //                     uiform.LayoutSuspend();
   //                     uiform.DataSource.SetParamsValues(pars1);
   //                     uiform.DataSource.KeyParamName = uiform.DataSource.InputParams.Keys.First();
   //                     uiform.LayoutResume();
   //                     UIStatic.UpdateForm(uiform, pars1.ToArray(), true, false);
   //                     DataRow row = calledTbl.Rows.Add();
   //                     calledTbl.CurrentRow = row;
   //                 }
   //                 break;
   //             case TextConst.AVActionType.DynamicFormForSelect:
   //                 uiform = UIStatic.CreateForm(action.P_Call, pars1.ToArray(), false, isModalForm, true);
   //                 // устанавливаем ранее выбранные значение
   //                 object[] values;
   //                 if (col != null && inrow != null) {
   //                     values = new object[1] { inrow[col] };
   //                 } else {
   //                     DataColumn value_column = table.Columns["value"];
   //                     values = new object[table.Rows.Count];
   //                     for (int index = 0; index < table.Rows.Count; index++) {
   //                         values[index] = table.Rows[index][value_column];
   //                     }
   //                 }
   //                 uiform.InitSelection(values);
   //                 isModalForm = true;
   //                 isSelectByForm = true;
   //                 break;
   //             case TextConst.AVActionType.DynamicFormCreateMultiple:
   //                 uiform = UIStatic.CreateForm(action.P_Call, pars1.ToArray(), false, isModalForm, true);
   //                 isMultipleCreate = true;
   //                 break;
   //             case TextConst.AVActionType.CreateByClientMethod:
   //                 createdKeys = (object[])executeClientMethod(pars, dataSet);
   //                 isMultipleCreate = true;
   //                 break;
   //             case TextConst.AVActionType.Refill:
   //                 executeClientRefill(dataSet, pars1);
   //                 goto Finish;
   //             case TextConst.AVActionType.ClientUpdate:
   //                 executeClientUpdate(dataSet, pars1);
   //                 goto Finish;
   //             case TextConst.AVActionType.ClientAddByForm:
   //                 executeClientAddByForm(dataSet, pars1);
   //                 goto Finish;
   //             case TextConst.AVActionType.SaveAndClose:
   //                 senderForm.SaveDataAndClose();
   //                 goto Finish;
   //             case TextConst.AVActionType.Save:
   //                 senderForm.SaveData(false);
   //                 goto Finish;
   //             case TextConst.AVActionType.Close:
   //                 senderForm.Close();
   //                 goto Finish;
   //             case TextConst.AVActionType.RefreshForm:
   //                 senderForm.DataSource.RefreshTopTable(false);
   //                 goto Finish;
   //             case TextConst.AVActionType.RefreshTable:
   //                 var tbl1 = (VDataTable)dataSet.Tables[this.P_CalledObject];
   //                 tbl1.Refresh();
   //                 goto Finish;
   //             case TextConst.AVActionType.RefreshColumn:
   //                 var tbl3 = (VDataTable)dataSet.Tables[this.P_CalledObject];
   //                 var col1 = tbl3.GetColumn(P_Column);
   //                 col1.RefreshAllColumn();
   //                 goto Finish;
   //             case TextConst.AVActionType.ResetColumn:
   //                 tbl3 = (VDataTable)dataSet.Tables[this.P_CalledObject];
   //                 col1 = tbl3.GetColumn(P_Column);
   //                 col1.ResetAllColumn();
   //                 goto Finish;
   //             case TextConst.AVActionType.ClientDeleteRow:
   //                 //if (inrow == null)
   //                 //{
   //                 //    table.ActulalizeSelection();
   //                 //    foreach (var r in table.SelectedRows)
   //                 //    {
   //                 //        table.Rows.Remove(r);
   //                 //    }
   //                 //}
   //                 //else
   //                 //{
   //                     table.Rows.Remove(inrow);
   //                 //}
   //                 //inrow.AcceptChanges();
   //                 goto Finish;
   //             case TextConst.AVActionType.ClientRemoveRow:
   //                 //if (inrow == null)
   //                 //{
   //                     table.ActulalizeSelection();
   //                     foreach (var r in table.SelectedRows) {
   //                         table.DeleteRow(r);
   //                     }
   //                 //}
   //                 //else
   //                 //{
   //                 //    table.Rows.Remove(inrow);
   //                 //}
   //                 //inrow.AcceptChanges();
   //                 goto Finish;
   //             case TextConst.AVActionType.ExecuteUpdate:
   //                 if (inrow != null) {
   //                     tbl = (VDataTable)inrow.Table;
   //                 }
   //                 executeUpdate(dataSet, pars1, tbl, inrow);
   //                 goto Finish;
			//	case TextConst.AVActionType.ShowPopupField:
			//		if (ctrlName == "" && table != null) {
			//			ctrlName = table.TableName; //если кнопка находится внутри элемента выпадающего списка, то использовать его в качестве ctrlName
			//		}
			//		if (ctrlName != "") {
			//			var fld = senderForm.GetParamField(ctrlName);
			//			fld.ShowPopupList();
			//		}
			//		goto Finish;
			//	case TextConst.AVActionType.CopyFieldToClipboard:
			//		if (ctrlName == string.Empty && table != null) {
			//			ctrlName = table.TableName;//если кнопка находится внутри элемента выпадающего списка, то использовать его в качестве ctrlName
			//		}
			//		if (ctrlName != string.Empty) {
   //                     string textForClipboard;
   //                     object paramsObject = senderForm.GetParamField(ctrlName).GetValueNames();
   //                     IList<string> names = paramsObject as IList<string>;
			//			if (names != null) {
   //                         StringBuilder sb = new StringBuilder();
   //                         for (int index = 0; index < names.Count; index++) {
   //                             sb.AppendLine(names[index]);
			//				}
   //                         textForClipboard = sb.ToString();
   //                         sb.Clear();
			//			} else {
			//				textForClipboard = (string)paramsObject;
			//			}
			//			if (!string.IsNullOrEmpty(textForClipboard)) {
			//				UIStatic.GetControlsfactory().SetClipboardText(textForClipboard);
			//			}
			//		}
			//		goto Finish;
			//	case TextConst.AVActionType.FillFieldFromClipboard:
			//		if (ctrlName == "" && table != null) {
			//			ctrlName = table.TableName;//если кнопка находится внутри элемента выпадающего списка, то использовать его в качестве ctrlName
			//		}
			//		if (ctrlName != "") {
   //                     ListField field = senderForm.GetParamField(ctrlName) as ListField;
   //                     if (field != null) {
   //                         string[] textFromClipboard = UIStatic.GetControlsfactory().GetClipboardText().Replace("\r", "").Split('\n');
   //                         field.SetValueByNames(textFromClipboard);
   //                     }
			//		}
			//		goto Finish;
   //             case TextConst.AVActionType.ShowSubForm:
   //                 senderForm.ShowInnerSubformNew(CalledControlX());
   //                 goto Finish;
   //             case TextConst.AVActionType.HideSubForm:
   //                 senderForm.HideInnerSubform();
   //                 goto Finish;
   //             case TextConst.AVActionType.CallClientMethod:
   //                 executeClientMethod(pars1,dataSet);
   //                 goto Finish;
   //             case TextConst.AVActionType.AddByClientMethod:
   //                 tbl = null;
   //                 if (inrow != null) {
   //                     tbl = (VDataTable)inrow.Table;
   //                 }
   //                 addByClientMethod(dataSet, pars1);
   //                 goto Finish;
   //             case TextConst.AVActionType.GetValWithClientMethod:
   //                 getValueWithClientMethod (pars1,table);
   //                 goto Finish;
   //             case TextConst.AVActionType.CallPlsql:
   //                 if (senderForm != null) {
   //                     senderForm.WasChanges = true;
   //                 }
   //                 executePlSqlMethod(dataSet, pars1);
   //                 goto Finish;
   //             case TextConst.AVActionType.CallPlsqlAdd:
   //                 if (senderForm != null) {
   //                     senderForm.WasChanges = true;
   //                 }
   //                 if (inrow != null) {
   //                     tbl = (VDataTable)inrow.Table;
   //                 }
   //                 executePlSqlMethod(dataSet, pars1, tbl, true);
   //                 goto Finish;
   //             case TextConst.AVActionType.OpenReport:
   //                 openReport(pars1, reportsForm);
   //                 goto Finish;
   //             case TextConst.AVActionType.OpenExpressReport:
   //                 openExpressReport(pars1);
   //                 goto Finish;
   //             case TextConst.AVActionType.OpenGrDetailReport:
   //                 openReportGrDetail(pars1, table, inrow, reportsForm,paramsForm);
   //                 goto Finish;
   //             case TextConst.AVActionType.OpenColGrDetailReport:
   //                 openReportColGrDetail(pars1, table, inrow, col,true, reportsForm,paramsForm);
   //                 goto Finish;
   //             case TextConst.AVActionType.AcceptSelection:
   //                 senderForm.AcceptSelection(table.TableName);
   //                 goto Finish;
   //             case TextConst.AVActionType.AddSelected:
   //                 var tbl5 = dataSet.MultiselectSourceTables().First();
   //                 tbl5.SetRowsChecked(tbl5.SelectedRows.ToArray());
   //                 //foreach (DataRow r in tbl5.SelectedRows)
   //                 //{
   //                 //    var val = r[tbl5.MultiselectColumnName].ToString();
   //                 //    if (val != "1")
   //                 //    {
   //                 //        tbl5.GetColumn(tbl5.MultiselectColumnName).SetValue(r, 1);
   //                 //    }
   //                 //}
   //                 goto Finish;
   //             case TextConst.AVActionType.RemoveSelected:
   //                 // var tbl4 = dataSet.MultiselectSourceTables().First();
   //                 //tbl4.SetRowsUnChecked(tbl4.SelectedRows.ToArray());
   //                 var tbl4 = dataSet.MultiselectTargetTables().First();
   //                 tbl4.SetRowsUnCheckedOnTarget(tbl4.SelectedRows.ToArray());
   //                 goto Finish;
   //             case TextConst.AVActionType.ExecuteAdd:
   //                 if (inrow != null) {
   //                     tbl = (VDataTable)inrow.Table;
   //                 }
   //                 executeAdd(dataSet, pars1, tbl);
   //                 goto Finish;
   //             case TextConst.AVActionType.ExecuteDelete:
   //                 //if (inrow != null)
   //                 //{
   //                 //    tbl = (VDataTable)inrow.Table;
   //                 //}
   //                 executeDelete(dataSet, pars1);
   //                 goto Finish;
   //             case TextConst.AVActionType.ExecuteCopyByReport:
   //                 executeCopyByReport(dataSet, pars1, table);
   //                 goto Finish;
   //             case TextConst.AVActionType.ExecuteInsertByReport:
   //                 if (inrow != null) {
   //                     tbl = (VDataTable)inrow.Table;
   //                 }
   //                 executeInsertByReport(dataSet, pars1, tbl);
   //                 goto Finish;
   //             default:
   //                 break;
   //         }
   //         VDataTable contextTable;
   //         if (P_CalledObject != "") {
   //             contextTable = (VDataTable)dataSet.Tables[P_CalledObject];
   //         } else {
   //             contextTable = table;
   //         }
   //         if (uiform != null) {
   //             if (UIStatic.IsWeb()) {
   //                 uiform.UpdateTitle();
   //                 uiform.ApplyVisibitlity();
   //                 uiform.GetControl().ShowForm();
   //             } else {
   //                 if (isModalForm) {
   //                     uiform.WasChanges = false;
   //                     uiform.ClearDataOnClose = false;
   //                     if (this.P_UseParentDsId == TextConst.AVBool.True) {
   //                         uiform.DataSource.ParentDataSet = dataSet;
   //                     }
   //                     //uiform.LayoutSuspend();
   //                     //dform = dform ?? new frmDynamicEditor(uiform.FormName);
   //                     //dform.Controls.Add(uiform.TmpGetControlAsWinFormCtrl() as Control);
   //                     //dform.Text = uiform.Title;
   //                     //dform.Name = uiform.FormName;
   //                     //dform.MdiParent = null;
   //                     //dform.StartPosition = FormStartPosition.CenterParent;
   //                     //dform.ShowDialog(senderForm.TmpGetControlAsWinFormCtrl() as Control);
   //                     //if (dform == null)
   //                     //{
   //                     //    dform = uiform.GetDialogContainer() as frmDynamicEditor;
   //                     //}
   //                     uiform.SetDialogContainer(dform);
   //                     IVForm owner = null;
   //                     if (senderForm != null) {
   //                         owner = senderForm.GetControl();
   //                     }
   //                     uiform.GetControl().ShowDialog(owner);
   //                     dform = uiform.GetDialogContainer() as frmDynamicEditor;
   //                     // Обновление ячеек в исходной строке после внесения изменений через диалог
   //                     updateContextTable(contextTable, calledTbl, ref col, ref createdKeys, inrow, isItemCreate, isMultipleCreate, isSelectByForm, senderForm, table, uiform);
   //                     // uiform.LayoutResume();
   //                     var tblR = uiform.DataSource.TopTable.First();
   //                     if (tblR != null) {
   //                         if (tblR.HasPrimaryKey() && tblR.Rows.Count == 1) {
   //                             ret = tblR.Rows[0][tblR.PrimaryKey[0]];
   //                         }
   //                     }
   //                     uiform.DataSource.ClearData();
   //                     if (senderForm != null) {
   //                         if (uiform.WasChanges) {
   //                             senderForm.WasChanges = true;
   //                         }
   //                     }
   //                 } else {
   //                     //if (dform == null)
   //                     //{
   //                     //    dform = uiform.GetDialogContainer() as frmDynamicEditor;
   //                     //}
   //                     uiform.SetDialogContainer(dform);
   //                     if (mdiParentForm == null) {
   //                         uiform.GetControl().ShowForm();
   //                     }
   //                     dform = uiform.GetDialogContainer() as frmDynamicEditor;
   //                     //if (senderForm != null && ucDataEditorMain.GetInstance() != null)
   //                     //{
   //                     //    ucDataEditorMain.GetInstance().ShowForm(uiform);
   //                     //}
   //                     //else
   //                     //{
   //                     //    dform = dform ?? new frmDynamicEditor(uiform.FormName);
   //                     //    dform.Text = uiform.Title;
   //                     //    dform.Name = uiform.FormName;
   //                     //    dform.Controls.Add(uiform.TmpGetControlAsWinFormCtrl() as Control);
   //                     //    dform.Show();
   //                     //}
   //                     if (mdiParentForm != null) {
   //                         dform = dform ?? new frmDynamicEditor(uiform.GetFormName());
   //                         dform.Controls.Add(uiform.TmpGetControlAsWinFormCtrl() as Control);
   //                         dform.Text = uiform.GetTitle();
   //                         dform.Name = uiform.GetFormName();
   //                         dform.MdiParent = mdiParentForm;
   //                         //dform.WindowState = FormWindowState.Normal;
   //                         //mdiParentForm.WindowState = FormWindowState.Normal;
   //                         dform.Show();
   //                     }
   //                 }
   //                 string calledCtrl = CalledControlX();
   //                 if (calledCtrl != "") {
   //                     uiform.ActivateTab(calledCtrl);
   //                 }
   //             }
   //         }
   //         if (createdKeys != null) {
   //             if (senderForm != null) {
   //                 senderForm.WasChanges = true;
   //             }
   //             if (senderForm.Grids.ContainsKey(contextTable.TableName)) {
   //                 var view = senderForm.Grids[contextTable.TableName].GetGridControl();
   //                 //view.BeginViewUpdate.GridControl.BeginUpdate();
   //                 view.BeginControlUpdate();
   //             }
   //             var rows = contextTable.AddExistingRow(createdKeys);
   //             if (senderForm.Grids.ContainsKey(contextTable.TableName)) {
   //                 var view = senderForm.Grids[contextTable.TableName].GetGridControl();
   //                 //view.GridControl.EndUpdate();
   //                 view.EndControlUpdate();
   //                 senderForm.Grids[contextTable.TableName].SetSelection(createdKeys);
   //             }
   //         }
   //         if (form != null) {
   //             var mdi = Application.OpenForms.OfType<RibbonForm>().FirstOrDefault();
   //             if (mdi != null) form.MdiParent = mdi;
   //             form.Show();
   //         }
   //      Finish:
   //         WaitUIHelper.LastUsedUIHelper.Hide();
   //         string message = this.P_Message;
   //         if (!string.IsNullOrEmpty(message)) {
   //             ShowMessage.ShowInformation(senderForm.ReplaceParameters(message));
   //         }
   //         string notification = this.P_Notification;
   //         if (!string.IsNullOrEmpty(notification)) {
   //             ShowMessage.ShowNotification(senderForm.ReplaceParameters(notification));
			//}
   //         if (senderForm != null) {
   //             senderForm.ApplyVisibitlity();
   //         }
   //         if (senderForm != null) {
   //             senderForm.RaiseChangeActionComplete();
   //             senderForm.RaiseButtonClick(this);// получается  не только кнопки но и другие действия
   //         }
   //         return ret;
   //     }
   //     private void updateContextTable(VDataTable contextTable,VDataTable calledTbl,ref VDataColumn col,ref object[] createdKeys,DataRow inrow,bool isItemCreate,bool isMultipleCreate,bool isSelectByForm,UIFormC senderForm,VDataTable table,UIFormC uiform)
   //     {
   //         if (contextTable != null)
   //         {


   //             object keyVal = null;
   //             if (isItemCreate && col == null) // если вызвали кнопкой в ячейке то не добавляем строку
   //             {

   //                 if (calledTbl.CurrentRow != null)
   //                 {
   //                     if (calledTbl.CurrentRow.RowState != DataRowState.Added)
   //                     {
   //                         keyVal = calledTbl.CurrentRow[calledTbl.PrimaryKey[0]];
   //                         var rows = contextTable.AddExistingRow(new object[] {keyVal});
   //                         if (contextTable.CurrentRow == rows[0])
   //                         {
   //                             contextTable.RaiseCurrentRowChanged();
   //                         }
   //                         else
   //                         {
   //                             contextTable.CurrentRow = rows[0];
   //                         }
   //                     }

   //                 }


   //                 //var sce=tbl.SuppressChangedEvent;
   //                 //tbl.SuppressChangedEvent
   //                 //var newRow=tbl.Rows.Add
   //             }
   //             else if (isMultipleCreate)
   //             {
   //                 if (createdKeys == null)
   //                 {
   //                     var creationTable = uiform.DataSource.Tables.Cast<VDataTable>().FirstOrDefault(t => t.CreatedItems != null);

   //                     if (creationTable != null)
   //                     {

   //                         List<object> keys = new List<object>();
   //                         foreach (object createdKey in creationTable.CreatedItems)
   //                         {
   //                             keys.Add(createdKey);

   //                         }
   //                         createdKeys = keys.ToArray();


   //                     }
   //                 }

   //             }
   //             else if (isSelectByForm)
   //             {
   //                 var srtbl = uiform.DataSource.ChoiceSource;
   //                 if (srtbl != null)
   //                 {
   //                     if (col != null && inrow != null)
   //                     {
   //                         // Емцов. строка же может быть не выбрана. для еденичного выбора оставил так
   //                         if (srtbl.SelectedRows != null && srtbl.SelectedRows.Count != 0) {
   //                             var row = srtbl.SelectedRows.First();
   //                             var val = srtbl.SelectedRows.First()[srtbl.PrimaryKey[0]];
   //                             col.SetValue(inrow, val);
   //                             senderForm.ApplyVisibitlity();// вроде должно работать без этого, но нет.

   //                         }
   //                     }
   //                     // множественный выбор
   //                     else
   //                     {
   //                         table.Clear();
   //                         if (srtbl.SelectedRows != null && srtbl.SelectedRows.Count != 0) {
   //                             var key = srtbl.PrimaryKey[0].ColumnName;
   //                             var name = srtbl.Columns.Cast<DataColumn>().First(c => !string.IsNullOrEmpty(c.Caption)).ColumnName;
   //                             foreach (var row in srtbl.SelectedRows)
   //                             {
   //                                 table.Rows.Add(row[key], row[name]);
   //                             }
   //                         }
   //                     }
   //                 }
   //             }
   //             else
   //             {
   //                 if (contextTable != null)
   //                 {
   //                     if (contextTable.CurrentRow != null)
   //                     {
   //                         bool async = true;

   //                         if (col != null)
   //                         {
   //                             async = false;
   //                         }
   //                         if (uiform.WasChanges)
   //                         {
   //                             contextTable.RefreshRowWithParents(new DataRow[] { contextTable.CurrentRow }, async);
   //                         }
   //                         if (col != null)
   //                         {
   //                             contextTable.SendFocusedCellToUI(col.ColumnName, contextTable.CurrentRow);
   //                         }
   //                     }
   //                 }
   //             }
   //         }
   //     }
    
    }

 
}
