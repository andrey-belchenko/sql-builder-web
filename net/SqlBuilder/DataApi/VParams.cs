using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VParams : VSXElement, IVParent
    {
        public VParams()
            : base(EName.@params)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Param, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}
