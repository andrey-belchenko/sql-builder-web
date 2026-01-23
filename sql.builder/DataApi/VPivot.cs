using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VPivot : VSXElement, IVParent
    {
        internal VPivot()
            : base(EName.pivot)
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