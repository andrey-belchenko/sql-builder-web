using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VColumns : VOutputElement, IVParent
    {
        public VColumns()
            : base(EName.columns)
        {
        }
        public static string[] child_nodes = { TextConst.EName.Band, TextConst.EName.Column, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}