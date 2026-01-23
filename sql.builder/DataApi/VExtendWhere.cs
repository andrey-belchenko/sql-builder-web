using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VExtendWhere : VSXElement, IVParent
    {
        internal VExtendWhere()
            : base(EName.extendwhere)
        {
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            return VWhere.child_nodes;
        }
    }
}
