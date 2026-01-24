using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VAdditions : VSXElement, IVParent
    {
        internal VAdditions()
            : base(EName.additions)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Addition, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}