using System.Data;
namespace sql.builder.DataApi
{
    public partial class VDataTable : DataTable
    {

        public bool IsNonDb = false;

        public delegate void VDataTableEventHandler(VDataTable table);
        public delegate void VDataRowEventHandler(DataRow row);
        public delegate void VDataCellEventHandler(DataRow row, VDataColumn column);
        public event VDataTableEventHandler CustomFill = null;
        public event VDataRowEventHandler CustomRowSave_Added = null;
        public event VDataRowEventHandler CustomRowSave_Modified = null;
        public event VDataRowEventHandler CustomRowSave_Deleted = null;
        public event VDataCellEventHandler CustomCellValueChanged = null;// не использовалось, не проверено
        public event VDataRowEventHandler CustomRowRefresh = null;
        private void raiseCustomFill()
        {

            if (CustomFill != null)
            {
                this.BeginLoadData();
                this.SuppressChangeEvent();// begin load не достаточно
                CustomFill(this);
                this.ResumeChangeEvent();
                this.AcceptChanges();
                this.EndLoadData();
            }
        }

        private void raiseCustomRowSave_Added(DataRow row)
        {
            if (CustomRowSave_Added != null)
            {
                CustomRowSave_Added(row);
            }

        }
        private void raiseCustomRowSave_Modified(DataRow row)
        {
            if (CustomRowSave_Modified != null)
            {
                CustomRowSave_Modified(row);
            }

        }
        private void raiseCustomRowSave_Deleted(DataRow row)
        {
            if (CustomRowSave_Deleted != null)
            {
                CustomRowSave_Deleted(row);
            }

        }
    }


}
