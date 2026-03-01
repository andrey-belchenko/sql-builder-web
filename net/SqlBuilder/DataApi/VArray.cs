using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VArray : VConst, IVParent
    {
        public VArray()
            : base(EName.array)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Const, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            return "(" + this.P_ConstValue + ")";
        }
        #endregion
        #region ConstListValue
        public override string P_ConstListValue
        {
            get
            {
                return this.Value;
            }
            set
            {
                this.Value = value ?? string.Empty;
            }
        }
        public override bool P_ConstListValue_Exists()
        {
            return true;
        }
        /*public void P_ConstListValue_ListRefresh(VDataTable table)
        {
            VDataTable tbl = SelectionListTable();
            tbl.Refresh();
            Cmn.CopyTable(tbl, table);
        }*/
        #endregion
    }
}