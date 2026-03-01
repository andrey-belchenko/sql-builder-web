using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VMenu : VSXElement, IVParent
    {
        public VMenu()
            : base(EName.menu)
        {
        }
        public static string[] child_nodes = { TextConst.EName.UICommand, TextConst.EName.Menu, TextConst.EName.UsePart };
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