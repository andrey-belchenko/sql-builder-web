using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VHaving : VSXElement, IVParent
    {
        public VHaving()
            : base(EName.having)
        {
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            return VWhere.child_nodes;
        }
    }
}
