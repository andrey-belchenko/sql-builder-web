using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VAdditions : VSXElement, IVParent
    {
        public VAdditions()
            : base(EName.additions)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Addition, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}