using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VCustomers : VSXElement, IVParent
    {
        public VCustomers()
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