using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VWhere : VSXElement, IVParent
    {
        internal VWhere()
            : base(EName.where)
        {
        }
        internal static string[] child_nodes = { TextConst.EName.Call, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}
