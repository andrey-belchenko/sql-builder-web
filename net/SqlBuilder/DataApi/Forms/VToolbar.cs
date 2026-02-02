using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VToolbar : VSXElement, IVParent
    {
        public VToolbar()
            : base(EName.toolbar)
        {
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            return VMenu.child_nodes;
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region EditorButtonType
        public override bool P_EditorButtonSide_Exists()
        {
            return true;
        }
        #endregion
        #region ColumnVisible
        public override bool P_ColumnVisible_Exists()
        {
            return true;
        }
        #endregion
    }
}