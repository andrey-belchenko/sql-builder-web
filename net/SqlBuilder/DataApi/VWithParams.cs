using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VWithParams : VSXElement, IVParent
    {
        public VWithParams()
            : base(EName.withparams)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Call, TextConst.EName.Const, TextConst.EName.Array, TextConst.EName.UseParam, TextConst.EName.Column, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}