using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VTabContainer : VSXElement, IVParent
    {
        internal VTabContainer()
            : base(EName.tabcontainer)
        {
        }
        private static string[] child_nodes = { TextConst.EName.FieldGroup, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}