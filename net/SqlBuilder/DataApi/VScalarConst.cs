using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VScalarConst: VConst
    {
        internal VScalarConst()
            : base(EName.@const)
        {
        }
        #region ConstComboValue
        public override string P_ConstComboValue {
            get {
                return this.Value;
            }
            set {
                this.Value = value ?? string.Empty;
            }
        }
        public override bool P_ConstComboValue_Exists()
        {
            return true;
        }
        public void P_ConstComboValue_List(VDataTable table)
        {
        }
        public void P_ConstComboValue_ListRefresh(VDataTable table)
        {
            VDataTable tbl = this.SelectionListTable();
            if (tbl != null) {
                tbl.Refresh();
                Cmn.CopyTable(tbl, table);
            }
        }
        #endregion
    }
}