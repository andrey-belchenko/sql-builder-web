using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VActions : VSXElement, IVParent
    {
        internal VActions()
            : base(EName.actions)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Action, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}