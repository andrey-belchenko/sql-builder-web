using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VColumns : VOutputElement, IVParent
    {
        internal VColumns()
            : base(EName.columns)
        {
        }
        internal static string[] child_nodes = { TextConst.EName.Band, TextConst.EName.Column, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}