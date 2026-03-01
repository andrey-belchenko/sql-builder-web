using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VTabContainer : VSXElement, IVParent
    {
        public VTabContainer()
            : base(EName.tabcontainer)
        {
        }
        private static string[] child_nodes = { TextConst.EName.FieldGroup, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}