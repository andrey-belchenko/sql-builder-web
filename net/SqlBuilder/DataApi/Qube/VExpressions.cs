using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VExpressions : VOutputElement, IVParent
    {
        public VExpressions()
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