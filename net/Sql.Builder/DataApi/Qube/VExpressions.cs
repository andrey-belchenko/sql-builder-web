using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VExpressions : VOutputElement, IVParent
    {
        internal VExpressions()
            : base(EName.expressions)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Call, TextConst.EName.UsePart };
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
            return " " + Italic(this.P_Comment);
        }
        #endregion
    }
}