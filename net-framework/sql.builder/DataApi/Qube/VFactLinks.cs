using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VFactLinks : VSXElement, IVParent
    {
        internal VFactLinks()
            : base(EName.factlinks)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Link, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}