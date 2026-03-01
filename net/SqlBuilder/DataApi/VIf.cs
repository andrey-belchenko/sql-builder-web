using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VIf : VSXElement, IVParent
    {
        public VIf()
            : base(EName.@if)
        {
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            return VWhere.child_nodes;
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
        #region AliasPfx
        public override bool P_AliasPfx_Exists()
        {
            return true;
        }
        #endregion
    }
}