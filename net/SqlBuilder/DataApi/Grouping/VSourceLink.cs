using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VSourceLink : VSXElement, IVParent
    {
        public VSourceLink()
            : base(EName.sourcelink)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Group, TextConst.EName.Grset, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            return this.P_Table;
        }
        #endregion
        #region Table
        public override bool P_Table_Exists()
        {
            return true;
        }
        public override void P_Table_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            var names = new HashSet<string>();
            foreach (VQueryCall el in this.ExtendedOrRootQuery().AllSources())
            {
                if (!names.Contains(el.XName))
                {
                    TableListRowFromElement(table, el);
                    names.Add(el.XName);
                }
            }
        }
        #endregion
    }
}