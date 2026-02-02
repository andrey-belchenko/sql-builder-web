using System;
using System.Xml.Linq;
using System.Data;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal sealed class VUseGlobParam : VSXElement
    {
        internal VUseGlobParam()
            : base(EName.useglobparam)
        {
        }
        public override object GetRuntimeValue(VDataSet dataSet, DataRow row, VDataColumn col)
        {
            object val = dataSet.GetParamValueByName(this.P_UsedParName, row);
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
            foreach (VSXElement el in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.globalparams).Elements(EName.param)) {
                table.AddRow(el.AttrOrEmpty(AName_.name));
            }
        }
        #endregion
    }
}