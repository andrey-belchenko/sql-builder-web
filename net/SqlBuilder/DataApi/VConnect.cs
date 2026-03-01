using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VConnect : VSXElement, IVParent
    {
        public VConnect()
            : base(EName.connect)
        {
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            return VWhere.child_nodes;
        }
    }
}