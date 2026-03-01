using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VActions : VSXElement, IVParent
    {
        public VActions()
            : base(EName.actions)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Action, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}