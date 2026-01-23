using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VSplitContainer : VSXElement, IVParent
    {
        internal VSplitContainer()
            : base(EName.splitcontainer)
        {
        }
        private static string[] child_nodes = { TextConst.EName.FieldGroup, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region IsVertical
        public override bool P_IsVertical_Exists()
        {
            return true;
        }
        #endregion
    }
}