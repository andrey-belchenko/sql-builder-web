using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace SqlBuilder.Sandbox
{
    static class OracleExample
    {
        // Uses tnsnames.ora from instant client; REALRYAZ.WORLD -> ryazan-ora.infoenergo.loc
        private const string ConnectionString =
            "User Id=asuse;Password=kl0pik;Data Source=REALRYAZ.WORLD;Tns_Admin=C:\\oracle\\instantclient_11_2\\network\\admin";

        public static DataTable LoadRsEsys()
        {
            var dt = new DataTable();
            using (var conn = new OracleConnection(ConnectionString))
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = "SELECT * FROM rs_esys";
                    using (var adapter = new OracleDataAdapter(cmd))
                    {
                        adapter.Fill(dt);
                    }
                }
            }
            return dt;
        }
    }
}
