using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VLinks : VSXElement, IVParent
    {
        public VLinks()
            : base(EName.links)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Link, TextConst.EName.DLink };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}