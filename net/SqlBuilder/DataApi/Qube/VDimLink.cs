using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;

namespace sql.builder.DataApi
{
    public sealed class VDimLink : VQueryCall
    {
        public VDimLink()
            : base(EName.dimlink)
        {
        }
        public override VQuery Query()
        {
            VQuery dqry = XmlReports.Environment.GetQueryByKeyDimensionName(this.P_Name);
            return dqry;
        }
        public override VRelation GetRelation()
        {
            return null;
        }
        public override List<VSXElement> GetUsedElements()
        {
            var qry = this.LinkParentQuery().GetMainE() as VQuery;
            var dims = qry.EntityType.AllDimensionLinks().Where(e=>e.P_Dimension==this.P_Name).ToList();
            return dims;
        }
        #region CalledQuery
        public override void P_CalledQuery_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            var qry = this.LinkParentQuery().GetMainE() as VQuery;
            var dims = qry.EntityType.AllDimensionLinks();
            var ids = new List<string>();
            foreach (VSXElement el in dims) {
                var dname = el.P_Dimension;
                if (!ids.Contains(dname)) {
                    var dqry = XmlReports.Environment.GetQueryByKeyDimensionName(dname);
                    if (dqry != null) {
                        table.Rows.Add(dname, dname, dqry.P_Title, dqry.P_Name);
                    } else {
                        table.Rows.Add(dname, dname);
                    }
                    ids.Add(dname);
                }
            }
        }
        #endregion
        #region Dimension
        public override string P_Dimension {
            get {
                if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                    return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as string);
                }
                string t = string.Empty;
                if (this.P_PrDimension != "") {
                    VQuery qry = this.Query();
                    if (qry != null) {
                        t = base.P_Dimension;
                    } else {
                        t = this.P_CalledQuery;
                    }
                }
                AddCashValue(t, MethodBase.GetCurrentMethod().ToString(), null);
                return t;
            }
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeTypeInfo() + " " + this.GetNodeOtherInfo();
        }
        #endregion
    }
}