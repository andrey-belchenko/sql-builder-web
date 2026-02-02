using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VGrouping : VSXElement, IVParent
    {
        public VGrouping()
            : base(EName.grouping)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Grset, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}