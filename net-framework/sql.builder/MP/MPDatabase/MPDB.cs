using System.Data;
using Devart.Data.Oracle;

namespace sql.builder.MP
{
    public static class MPDB
    {
        public static bool FillData(string srcTableName, string destTablename, string[] mainCols)
        {
            var pars = new[]
            {
                new OracleParameter("p_src_table_name", OracleDbType.VarChar) { Value = srcTableName },
                new OracleParameter("p_dest_table_name", OracleDbType.VarChar) { Value = destTablename },
                new OracleParameter("p_main_cols", OracleDbType.VarChar) { Value = mainCols, ArrayLength = mainCols.Length},
                new OracleParameter("res", OracleDbType.Integer) { Direction = ParameterDirection.ReturnValue }
            };

            using (var cmd = MPEnvironment.Connection.CreateCommand())
            {
                cmd.CommandText = "imp_pg.fillData";
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddRange(pars);
                cmd.ExecuteNonQuery();

                return cmd.Parameters["res"].Value.Equals(1);
            }
        }
    }
}