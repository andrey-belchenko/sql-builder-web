using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;print-templates /&gt;
    /// </summary>
    internal sealed class VPrintTemplates : VSXElement, IVParent
    {
        internal VPrintTemplates()
            : base(EName.print_templates)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Excel, TextConst.EName.Word };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        public override bool P_Comment_Exists()
        {
            return false;
        }
    }
    internal sealed class VExcel : VSXElement, IVParent
    {
        internal VExcel()
            : base(EName.excel)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Template };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        public override bool P_Comment_Exists()
        {
            return false;
        }
    }
    internal sealed class VWord : VSXElement, IVParent
    {
        internal VWord()
            : base(EName.word)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Template };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        public override bool P_Comment_Exists()
        {
            return false;
        }
    }
}
