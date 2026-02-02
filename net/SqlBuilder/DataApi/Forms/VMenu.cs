using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VMenu : VSXElement, IVParent
    {
        internal VMenu()
            : base(EName.menu)
        {
        }
        internal static string[] child_nodes = { TextConst.EName.UICommand, TextConst.EName.Menu, TextConst.EName.UsePart };
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