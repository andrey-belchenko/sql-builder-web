using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VUsing : VSXElement, IVParent
    {
        internal VUsing()
            : base(EName.@using)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Column, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}