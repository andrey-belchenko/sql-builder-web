using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VSection : VSXElement, IVParent
    {
        public VSection()
            : base(EName.section)
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