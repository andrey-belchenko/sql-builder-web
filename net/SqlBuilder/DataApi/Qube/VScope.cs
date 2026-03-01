using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VScope : VSXElement, IVParent
    {
        public VScope()
            : base(EName.scope)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Call, TextConst.EName.Const, TextConst.EName.UseParam, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        public override string GetNodeOtherInfo()
        {
            return Italic(this.P_SelfTitle);
        }
        #endregion
    }
}