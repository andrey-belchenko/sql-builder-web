using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VEvents : VSXElement, IVParent
    {
        public VEvents()
            : base(EName.events)
        {
        }
        private static string[] child_nodes = { TextConst.EName.UseAction, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
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
    }
}
