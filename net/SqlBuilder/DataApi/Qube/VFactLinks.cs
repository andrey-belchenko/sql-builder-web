using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VFactLinks : VSXElement, IVParent
    {
        public VFactLinks()
            : base(EName.factlinks)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Link, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}