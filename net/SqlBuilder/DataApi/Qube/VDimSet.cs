using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VDimSet : VQueryCall, IVParent
    {
        internal VDimSet()
            : base(EName.dimset)
        {
        }
        internal IList<VFact> GetFacts()
        {
            var list = (this.GetMainParent() as VQuery).AllUsedFacts().Where(f => f.P_Table == P_Alias).ToList();
            return list;
        }
        private static string[] child_nodes = { TextConst.EName.Link, TextConst.EName.Where, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeTypeInfo() + " " + this.GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            return Bold(this.P_Alias);
        }
        #endregion
    }
}