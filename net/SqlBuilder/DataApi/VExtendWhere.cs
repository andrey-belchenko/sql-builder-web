using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VExtendWhere : VSXElement, IVParent
    {
        public VExtendWhere()
            : base(EName.extendwhere)
        {
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            return VWhere.child_nodes;
        }
    }
}
