using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VLinks : VSXElement, IVParent
    {
        internal VLinks()
            : base(EName.links)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Link, TextConst.EName.DLink };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}