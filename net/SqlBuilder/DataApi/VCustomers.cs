using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VCustomers : VSXElement, IVParent
    {
        internal VCustomers()
            : base(EName.customers)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Customer };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}