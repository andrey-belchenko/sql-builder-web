using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VButtons : VSXElement, IVParent
    {
        public VButtons()
            : base(EName.buttons)
        {
        }
        private static string[] child_nodes = { TextConst.EName.UICommand, TextConst.EName.Menu, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}