using System;
using System.Collections.Generic;
using System.Xml.Linq;

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
