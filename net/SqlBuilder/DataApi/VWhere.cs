using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VWhere : VSXElement, IVParent
    {
        public VWhere()
            : base(EName.where)
        {
        }
        public static string[] child_nodes = { TextConst.EName.Call, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}
