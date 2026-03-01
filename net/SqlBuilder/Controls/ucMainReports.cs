using System.Data;
////using System.Windows.Forms;
//using sql.builder.Controls.Grids;
using sql.builder.DataApi;
//using sql.builder.Test;
//using sql.builder.TFS;
//using sql.builder.TFS.AutoCheckIn;
//using sql.builder.TFS.AutoCheckIn.Commands;
using sql.builder.UI;
//using sql.builder.WebReports;

namespace sql.builder.Controls
{
    public partial class ucMainReports //: ucBase
    {
        public static void CopyParsToResult(UIFormC form, VDataSet result) //20171201 Новое , возможны ошибки
        {
            if (form == null)
            {
                return;
            }
            VDataSet ds = form.DataSource;
            if (ds == null)
            {
                return;
            }
            VDataTable params_table = ds.ParamsTable;
            if (params_table == null)
            {
                return;
            }
            if (result.Tables[TextConst.AVTable.Pars] == null)
            {
                VDataTable pars_table = new VDataTable();
                //Cmn.CopyVTable(params_table, pars_table);
                pars_table.TableName = TextConst.AVTable.Pars;
                DataColumnCollection cols = params_table.Columns;
                int index;
                for (index = 0; index < cols.Count; index++)
                {
                    DataColumn col = cols[index];
                    pars_table.AddColumn(col.ColumnName.ToUpper(), col.DataType, col.Caption);
                }
                DataRow row;
                if (params_table.Rows.Count == 0)
                {
                    row = null;
                }
                else
                {
                    DataRow src_row = params_table.Rows[0];
                    row = pars_table.NewRow();
                    for (index = 0; index < cols.Count; index++)
                    {
                        row[index] = src_row[index];
                    }
                    pars_table.Rows.Add(row);
                }
                foreach (UIBase f in form.controls.Values)
                {
                    string field_name = f.FieldName.ToUpper();
                    IRange range = f as IRange;
                    if (range != null)
                    {
                        VDataColumn col_1 = pars_table.AddColumn(field_name + "1_TEXT", typeof(string));
                        VDataColumn col_2 = pars_table.AddColumn(field_name + "2_TEXT", typeof(string));
                        if (row != null)
                        {
                            string value_1, value_2;
                            range.GetText(out value_1, out value_2);
                            row[col_1] = value_1;
                            row[col_2] = value_2;
                        }
                    }
                    else
                    {
                        VDataColumn col = pars_table.AddColumn(field_name + "_TEXT", typeof(string));
                        if (row != null)
                        {
                            if (f is UIList)
                            {
                                row[col] = (f as UIList).FullText;
                            }
                            else
                            {
                                row[col] = f.GetText();
                            }
                        }
                    }
                }
                result.Tables.Add(pars_table);
            }
        }
    }
}