using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VListQuery : VSXElement, IVParent
    {
        public VListQuery(XName name)
            : base(name)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Query, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}