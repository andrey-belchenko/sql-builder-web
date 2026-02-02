using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;print-templates /&gt;
    /// </summary>
    public sealed class VPrintTemplates : VSXElement, IVParent
    {
        public VPrintTemplates()
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
    public sealed class VExcel : VSXElement, IVParent
    {
        public VExcel()
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
    public sealed class VWord : VSXElement, IVParent
    {
        public VWord()
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
