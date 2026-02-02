using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VBand : VOutputElement, IVParent
    {
        internal VBand()
            : base(EName.band)
        {
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            if (this.GetAncestorsAndSelf(EName.select).Count != 0) {
                return VGridBand.child_nodes;
            } else {
                return VColumns.child_nodes;
            }
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            return Italic(this.P_SelfTitle);
        }
        #endregion
    }
}