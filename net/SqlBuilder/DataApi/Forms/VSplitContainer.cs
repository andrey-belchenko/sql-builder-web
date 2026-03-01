using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VSplitContainer : VSXElement, IVParent
    {
        public VSplitContainer()
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