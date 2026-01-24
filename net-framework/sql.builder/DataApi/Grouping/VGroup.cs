using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VGroup : VSXElement, IVParent
    {
        internal VGroup()
            : base(EName.group)
        {
        }
        internal List<VSourceLink> GroupLinks()
        {
            return this.GetElementsP(EName.sourcelink).Cast<VSourceLink>().ToList();
        }
        private static string[] child_nodes = { TextConst.EName.Column, TextConst.EName.SourceLink, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}