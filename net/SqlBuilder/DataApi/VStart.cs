using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VStart : VSXElement, IVParent
    {
        public VStart()
            : base(EName.start)
        {
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            return VWhere.child_nodes;
        }
    }
}