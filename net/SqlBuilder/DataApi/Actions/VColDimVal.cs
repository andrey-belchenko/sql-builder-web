using System.Data;
using System.Linq;
using System.Xml.Linq;
////using System.Windows.Forms;

namespace sql.builder.DataApi
{
    public sealed class VColDimVal : VSXElement
    {
        public VColDimVal()
            : base(EName.col_dim_val)
        {
        }
        public override object GetRuntimeValue(VDataSet dataSet, DataRow row, VDataColumn col)
        {
            object val = null;
            if (col != null)
            {
                if (this.P_UsedParName == col.PivotDimensionName)
                {
                    val = col.PivotDimensionValue;
                }
            }
            return val;
        }
        #region NodeText
        public override string GetNodeInfo()
        {
            return GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            return ":" + P_UsedParName;
        }
        #endregion
        #region UsedParName
        public override bool P_UsedParName_Exists()
        {
            return true;
        }
        public void P_UsedParName_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public void P_UsedParName_ListRefresh(VDataTable table)
        {
            VQuery query = null;
            VSourcedElement root_query = this.RootQuery();
            if (root_query is VReport)
            {
                query = ((VQueryCall)this.GetAncestorsAndSelf(EName.query).First()).Query();
            }
            else
            {
                query = (VQuery)root_query;
            }
            foreach (string name in query.Columns().Attributes(TextConst.AName.Dimname).Select(APredicate.AttributeValue).ToList().Distinct())
            {
                table.Rows.Add(name, name);
            }
        }
        #endregion
    }
}