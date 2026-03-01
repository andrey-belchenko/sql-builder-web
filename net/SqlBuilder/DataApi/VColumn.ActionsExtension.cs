using System.Data;
//using System.Windows.Forms;


namespace sql.builder.DataApi
{
    public partial class VColumn
    {
        public override object GetRuntimeValue(VDataSet dataSet, DataRow row, VDataColumn col)
        {

            var tbl = (VDataTable)dataSet.Tables[P_Table];
            // пока только знаяение свойе таблицы, может понадобиться еще значение родительской
            if (row == null)
            {
                row = tbl.CurrentRow;
            }
            return (row.Table as VDataTable).GetColumn(P_Column).GetValue(row);
        }
    }
}
