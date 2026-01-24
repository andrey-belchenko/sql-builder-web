using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Data;

namespace sql.builder.DataApi
{
    internal sealed class VUseParam : VSXElement
    {
        internal VUseParam()
            : base(EName.useparam)
        {
        }
        public override object GetRuntimeValue(VDataSet dataSet, DataRow row, VDataColumn col)
        {
            object val = dataSet.GetParamValueByName(P_UsedParName, row);
            return val;
        }
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            return Bold(TextConst.Pfx.Param + this.P_FormalParName);
        }
        #endregion
        #region UsedParName
        public override bool P_UsedParName_Exists()
        {
            return true;
        }
        public void P_UsedParName_List(VDataTable table)
        {
            table.AddColumn("id", "Параметр");
        }
        public void P_UsedParName_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            VExpression expr = this.GetSubMainParent() as VExpression;
            int index;
            if (expr != null) {
                IList<VParam> parameters = expr.FormalParams();
                for (index = 0; index < parameters.Count; index++) {
                    table.AddRow(parameters[index].P_FormalParNameS);
                }
                return;
            }
            VSourcedElement rootQuery = this.RootQuery();
            IList<VSXElement> list = rootQuery.FormalParams();
            for (index = 0; index < list.Count; index++) {
                table.AddRow(list[index].P_FormalParNameS);
            }
            VForm form = rootQuery as VForm;
            if (form != null) {
                list = form.ParamFields();
                for (index = 0; index < list.Count; index++) {
                    table.AddRow(list[index].P_FormalParNameS);
                }
                list = form.VariableColumns();
                for (index = 0; index < list.Count; index++) {
                    table.AddRow(list[index].P_ParName);
                }
                //foreach (string p in TextConst.AVParamArray.FormExtPars) {
                //    table.AddRow(p);
                //}
                table.AddRow(TextConst.AVParam.FormValid);
                table.AddRow(TextConst.AVParam.FormValidNot);
                //foreach (string p in TextConst.AVParamArray.TableExtPars) {
                //    foreach (VQueryCall qry in form.MainAndRelatedQueries()) {
                //        string name = qry.XName + p;
                //        table.AddRow(name);
                //    }
                //}
                IList<VQueryCall> queries = form.MainAndRelatedQueries();
                for (index = 0; index < queries.Count; index++) {
                    string name = queries[index].XName + TextConst.AVParam.HasChanges;
                    table.AddRow(name);
                }
            }
        }
        #endregion
    }
}