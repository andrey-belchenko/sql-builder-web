using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VFormContent : VOutputElement, IVParent
    {
        internal VFormContent()
            : base(EName.content)
        {
        }
        private static string[] child_nodes = { TextConst.EName.FieldGroup, TextConst.EName.Column, TextConst.EName.Fact, TextConst.EName.Call, TextConst.EName.Grid, 
            TextConst.EName.TabContainer, TextConst.EName.SplitContainer, TextConst.EName.Field,
            TextConst.EName.UseForm, TextConst.EName.Splitter, TextConst.EName.Label, TextConst.EName.Menu,
            TextConst.EName.UseField, TextConst.EName.UICommand, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}