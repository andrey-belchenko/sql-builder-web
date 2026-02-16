using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using System.Text;
using Devart.Data.Oracle;
//using infoenergo.core.Data.Tables;
using infoenergo.sys;
using sql.builder.Core;
using sql.builder.DataApi;
using DataHelper = infoenergo.core.Data.DataHelper;
using SqlBuilderLib.DevTools;

namespace sql.builder
{
    public static class db
    {
        private static OracleConnection _connection;
        public static OracleConnection Connection {
            get => Global.RequestConnection.Value != null ? Global.Connection : (_connection ?? Global.Connection);
            set { _connection = value; }
        }

        #region vr_grid_settings
        /*public static DataTable SelectAllVisibleSettings()
        {
            var args = new[]
            {
                new SqlArg("visible", 1, SqlDestination.Where),

                new SqlArg("kod_gs", "kod_gs", SqlDestination.Select),
                new SqlArg("original_name", "repname", SqlDestination.Select),
                new SqlArg("name", "repname||'_gs'||to_char(kod_gs)", SqlDestination.Select),
                new SqlArg("title", "name", SqlDestination.Select),
                new SqlArg("parent", "repname", SqlDestination.Select),
                new SqlArg("item_type", TextConst.EName.UseTemplate, SqlDestination.Select, SqlType.String),
                new SqlArg("image_id", "3", SqlDestination.Select),
                //new SqlArg("kod_menu", "null", SqlDestination.Select),

                new SqlArg("repname", "repname", SqlDestination.Order),
                new SqlArg("kod_gs", "kod_gs", SqlDestination.Order)
            };

            var dt = SqlMethods.Select("vr_grid_settings", args, Connection);
            foreach (var c in dt.Columns.Cast<DataColumn>())
            {
                c.ColumnName = c.ColumnName.ToLower();
                c.AllowDBNull = true;
            }
            return dt;
        }*/
        public static string SelectSettingData(decimal kod_gs)
        {
            OracleParameter[] parameters = new OracleParameter[1] { new OracleParameter("kod_gs", OracleDbType.Number, kod_gs, ParameterDirection.Input) };
            return DataHelper.SqlGetString("SELECT data FROM vr_grid_settings WHERE kod_gs = :kod_gs", parameters, Connection);
        }
        public static decimal InsertReportSetting(string repname, string name, string data)
        {
            OracleParameter kod_gs = new OracleParameter("kod_gs", OracleDbType.Number, null, ParameterDirection.Output);
            OracleParameter[] parameters = new OracleParameter[4] {
                new OracleParameter("repname", OracleDbType.VarChar, repname, ParameterDirection.Input),
                new OracleParameter("name",    OracleDbType.VarChar, name, ParameterDirection.Input),
                new OracleParameter("data",    OracleDbType.Clob, data, ParameterDirection.Input),
                kod_gs
            };
            DataHelper.SqlExecute("INSERT INTO vr_grid_settings (repname, name, data) VALUES (:repname, :name, :data) RETURNING kod_gs INTO :kod_gs", parameters, Connection);
            Connection.Commit();
            return Convert.ToDecimal(kod_gs.Value);
        }
        public static void UpdateReportSettingData(decimal kod_gs, string data)
        {
            OracleParameter[] parameters = new OracleParameter[2] {
                new OracleParameter("kod_gs", OracleDbType.Number, (object)kod_gs, ParameterDirection.Input),
                new OracleParameter("data",   OracleDbType.Clob, data, ParameterDirection.Input)
            };
            DataHelper.SqlExecute("UPDATE vr_grid_settings SET data = :data WHERE kod_gs = :kod_gs", parameters, Connection);
            Connection.Commit();
        }
        public static void UpdateReportSettingName(decimal kod_gs, string name)
        {
            OracleParameter[] parameters = new OracleParameter[2] {
                new OracleParameter("kod_gs", OracleDbType.Number, (object)kod_gs, ParameterDirection.Input),
                new OracleParameter("name",   OracleDbType.VarChar, name, ParameterDirection.Input)
            };
            DataHelper.SqlExecute("UPDATE vr_grid_settings SET name = :name WHERE kod_gs = :kod_gs", parameters, Connection);
            Connection.Commit();
        }
        public static void DeleteReportSetting(decimal kod_gs)
        {
            OracleParameter[] parameters = new OracleParameter[1] { new OracleParameter("kod_gs", OracleDbType.Number, (object)kod_gs, ParameterDirection.Input) };
            DataHelper.SqlExecute("DELETE FROM vr_grid_settings WHERE kod_gs = :kod_gs", parameters, Connection);
            Connection.Commit();
        }
        public static string SelectDefaultSettingData(string repname)
        {
            OracleParameter[] parameters = new OracleParameter[1] { new OracleParameter("repname", OracleDbType.VarChar, repname, ParameterDirection.Input) };
            DataTable dt = DataHelper.SqlGetTable("SELECT data FROM vr_grid_settings WHERE repname = :repname AND name = 'default' AND visible = 0 AND u_m = USER", parameters, Connection, false);
            if (dt.Rows.Count == 0) {
                return null;
            } else {
                return dt.Rows[0]["data"].ToString();
            }
        }
        public static void MergeDefaultReportSetting(string repname, string data)
        {
            OracleParameter[] parameters = new OracleParameter[2] {
                new OracleParameter("repname", OracleDbType.VarChar, repname, ParameterDirection.Input),
                new OracleParameter("data",    OracleDbType.NClob,   data,    ParameterDirection.Input)
            };
            DataHelper.SqlExecute("MERGE INTO vr_grid_settings USING dual ON (repname = :repname AND name = 'default' AND visible = 0 AND u_m = USER) WHEN MATCHED THEN UPDATE SET data = :data WHEN NOT MATCHED THEN INSERT (repname, name, visible, data) VALUES(:repname, 'default', 0, :data)", parameters, Connection); 
            Connection.Commit();
        }
        public static void DeleteDefaultReportSetting(string repname)
        {
            OracleParameter[] parameters = new OracleParameter[1] { new OracleParameter("repname", OracleDbType.VarChar, repname, ParameterDirection.Input) };
            DataHelper.SqlExecute("DELETE FROM vr_grid_settings WHERE repname = :repname AND name = 'default' AND visible = 0 AND u_m = USER", parameters, Connection);
            Connection.Commit();
        }
        #endregion
        /*#region vr_layout_settings
        public static string SelectDefaultLayoutData()
        {
            var args = new[]
            {
                new SqlArg("puser", "user", SqlDestination.Where, SqlType.Const),

                new SqlArg("data", "data", SqlDestination.Select)
            };

            var dt = SqlMethods.Select("vr_layout_settings", args, Connection);

            return dt.Rows.Count > 0 ? dt.Rows[0]["data"].ToString() : null;
        }
        public static string SelectDefaultLayoutName()
        {
            var args = new[]
            {
                new SqlArg("puser", "user", SqlDestination.Where, SqlType.Const),

                new SqlArg("name", "name", SqlDestination.Select)
            };

            var dt = SqlMethods.Select("vr_layout_settings", args, Connection);

            return dt.Rows.Count > 0 ? dt.Rows[0]["name"].ToString() : null;
        }
        public static void MergeLayout(string name, string data = null)
        {
            SqlArg[] args = null;
            if (data != null)
            {
                args = new[]
                {
                    new SqlArg("puser", "user", SqlDestination.Where, SqlType.Const),

                    new SqlArg("name", name, SqlDestination.Insert, SqlType.String),
                    new SqlArg("data", data, SqlDestination.Insert, SqlType.Parameter, ParameterDirection.Input, OracleDbType.NClob),
                    new SqlArg("puser", "user", SqlDestination.Insert, SqlType.Const),


                    new SqlArg("name", name, SqlDestination.Update, SqlType.String),
                    new SqlArg("data", data, SqlDestination.Update, SqlType.Parameter, ParameterDirection.Input, OracleDbType.NClob),
                };
            }
            else
            {
                args = new[]
                {
                    new SqlArg("puser", "user", SqlDestination.Where, SqlType.Const),

                    new SqlArg("name", name, SqlDestination.Insert, SqlType.String),
                    new SqlArg("puser", "user", SqlDestination.Insert, SqlType.Const),


                    new SqlArg("name", name, SqlDestination.Update, SqlType.String),
                };
            }

            SqlMethods.Merge("vr_layout_settings", args, Connection);
            Connection.Commit();
        }
        #endregion*/
        #region vr_reports_log
        //public static string repLogTableName = "vr_reports_log";
        //public static string repLogTimeTotalColName = "time_total";
        //public static string repLogRepnameColName = "repname";
        /*public static DataTable SelectReportLogWithoutError(string repname)
        {
            var args = new[]
            {
                new SqlArg("kod_log", "kod_log", SqlDestination.Select),
                new SqlArg("puser", "puser", SqlDestination.Select),
                new SqlArg("date_start", "date_start", SqlDestination.Select),
                new SqlArg("date_finish", "date_finish", SqlDestination.Select),
                new SqlArg(repLogTimeTotalColName, repLogTimeTotalColName, SqlDestination.Select),

                new SqlArg(repLogRepnameColName, repname, SqlDestination.Where, SqlType.String),
                new SqlArg("time_total", "not null", SqlDestination.Where, SqlType.Const),
                new SqlArg("error_text", "null", SqlDestination.Where, SqlType.Const),
                new SqlArg("stack_text", "null", SqlDestination.Where, SqlType.Const),
            };

            return SqlMethods.Select(repLogTableName, args, Connection);
        }*/
        public static decimal InsertReportLog(string repname, string report_params)
        {
            OracleParameter kod_log = new OracleParameter("kod_log", OracleDbType.Number, null, ParameterDirection.Output);
            OracleParameter[] parameters = new OracleParameter[3] {
                new OracleParameter("repname", OracleDbType.VarChar, repname, ParameterDirection.Input),
                new OracleParameter("params", OracleDbType.NClob, report_params, ParameterDirection.Input),
                kod_log
            };
            DataHelper.SqlExecute("INSERT INTO vr_reports_log (repname, params) VALUES (:repname, :params) RETURNING kod_log INTO :kod_log", parameters, Connection,false);
            Connection.Commit();
            return Convert.ToDecimal(kod_log.Value);
        }
        public static void UpdateReportLog(decimal kod_log, string error_text, string stack_text)
        {
            OracleParameter[] parameters = new OracleParameter[3] {
                new OracleParameter("error_text", OracleDbType.VarChar, error_text, ParameterDirection.Input),
                new OracleParameter("stack_text", OracleDbType.VarChar, stack_text, ParameterDirection.Input),
                new OracleParameter("kod_log", OracleDbType.Number, (object)kod_log, ParameterDirection.Input)
            };
            DataHelper.SqlExecute("UPDATE vr_reports_log SET error_text = :error_text, stack_text = :stack_text WHERE kod_log = :kod_log", parameters, Connection, false);
            Connection.Commit();
        }
        public static TimeSpan? AverageReportFormingTime(string report_name)
        {
            OracleParameter p_repname  = new OracleParameter("p_repname", OracleDbType.VarChar, report_name, ParameterDirection.Input);
            OracleParameter p_avg_time = new OracleParameter("p_avg_time", OracleDbType.IntervalDS, ParameterDirection.Output);
            DataHelper.SqlExecute(@"DECLARE
  p_repname     vr_reports_log.repname%type;
  n_count       PLS_INTEGER;
  i_total_time  INTERVAL DAY TO SECOND;
  i_time        INTERVAL DAY TO SECOND;
  i_avg_time    INTERVAL DAY TO SECOND;
  CURSOR cur IS
    SELECT /*+ FIRST_ROWS(20) */ time_total
    FROM   vr_reports_log
    WHERE  repname = p_repname
       AND time_total IS NOT NULL
    ORDER BY date_start DESC;
BEGIN
  p_repname := :p_repname;
  OPEN cur;
  FETCH cur INTO i_total_time;
  IF cur%NOTFOUND THEN
    i_avg_time := NULL;
  ELSE
    n_count := 1;
    WHILE n_count < 20 LOOP
      FETCH cur INTO i_time;
      EXIT WHEN cur%NOTFOUND;
      i_total_time := i_total_time + i_time;
      n_count := n_count + 1;
    END LOOP;
    i_avg_time := i_total_time / n_count;
  END IF; 
  CLOSE cur;
  :p_avg_time := i_avg_time;
END;", new OracleParameter[2] { p_repname, p_avg_time }, Connection);
            // Здесь p_avg_time.OracleValue is Devart.Data.Oracle.OracleIntervalDS
            object value = p_avg_time.Value;
            if (Cmn.IsNullOrDBNull(value)) {
                return null;
            } else {
                return (TimeSpan)value;
            }
        }
        #endregion
        public static DataTable SelectConstraintTableColumns(string schema_name, string constraint_name)
        {
            OracleParameter[] parameters = new OracleParameter[2] {
                new OracleParameter("schema", OracleDbType.VarChar, schema_name, ParameterDirection.Input),
                new OracleParameter("constraint_name", OracleDbType.VarChar, constraint_name, ParameterDirection.Input)
            };
            return DataHelper.SqlGetTable("SELECT table_name, column_name FROM all_cons_columns WHERE owner = :schema AND constraint_name = :constraint_name", parameters, Connection);
        }
        public static DataTable SelectMenuInfo(decimal[] kod_menu_array)
        {
            string sql = "select * from rk_menu where kod_menu in (" + string.Join(",", kod_menu_array) + ")";

            var dt = ExecuteDataTable(sql, db.Connection);
            return dt;
        }

        public static bool HasInvestproInstruction(string report_name)
        {
            var cnt = DataHelper.SqlGetDecimal(string.Format("select count(1) from ips_s where kod_name = '{0}'", report_name), Connection);
            return (cnt > 0);
        }
        #region Вспомогательные функции
        public static DataTable ExecuteDataTable(string sql, DbConnection conn = null,bool analyze=true)
        {
            conn = conn ?? Connection;

            var dt = new DataTable();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
             
                DbDataReader reader = null;
                try
                {
                    if (analyze){
                        DevUtilsProvider.Instance.AnalyzeExecSql(sql);
                    }
                   
                    reader = cmd.ExecuteReader();
                }
                catch (OracleException e)
                {
                    //throw new infoenergo.core.Data.OracleSqlException(e, sql);
                    throw e;
                }

                dt.Load(reader);
            }
            return dt;
            //return DataHelper.SqlGetTable(sql, conn);
        }
        public static VDataTable ExecuteVDataTable(string sql, DbConnection conn = null)
        {
            conn = conn ?? Connection;

            var dt = new VDataTable();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                DevUtilsProvider.Instance.AnalyzeExecSql(sql);
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            return dt;
            //return DataHelper.SqlGetTable(sql, conn);
        }

        public static object ExecuteObject(string sql, DbConnection conn = null, Dictionary<string,object> pars =null)
        {
            conn = conn ?? Connection;

            var dt = new DataTable();

         


            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                if (pars != null)// не проверено
                {
                    foreach (var par in pars)
                    {
                        var dbpar = new OracleParameter(par.Key, par.Value);
                        cmd.Parameters.Add(dbpar);
                    }
                    
                }
                
                DevUtilsProvider.Instance.AnalyzeExecSql(sql);
                var reader = cmd.ExecuteReader();
                dt.Load(reader);
            }
            if (dt.Rows.Count == 0)
            {
                return null;
            }
            return dt.Rows[0][0];
            //return DataHelper.SqlGetTable(sql, conn);
        }

     
        public static int ExecuteNonQuery(string sql, DbConnection conn = null)
        {
            conn = conn ?? Connection;

            var dt = new DataTable();
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = sql;
                DevUtilsProvider.Instance.AnalyzeExecSql(sql);
               return cmd.ExecuteNonQuery();
             
            }
        
            //return DataHelper.SqlGetTable(sql, conn);
        }
        #endregion

        #region ArrayStorage
        /*public static void AddToArrayStorage(string array_id, object[] values, string datatype = "number")
        {
            // 100 000 в одном insert крашат базу - проверено на asuse :)
            int maxValuesInInsert = 5000;

            // разбиваем на отдельные инсерты по 5000 значений максимум
            foreach (var values2 in values.Batch(maxValuesInInsert))
            {
                var sb = new StringBuilder();
                sb.AppendFormat("insert into vr_array_storage(array_id, {0})", ArrayStorage.DataColumnName(datatype));
                sb.AppendLine();
                sb.AppendLine("(");

                bool first = true;
                foreach (object value in values2)
                {
                    if (first)
                    {
                        sb.AppendFormat("select :array_id as array_id, {0} as {1} from dual",
                            ArrayStorage.TypedValueString(value, datatype),
                            ArrayStorage.DataColumnName(datatype));

                        first = false;
                    }
                    else
                    {
                        sb.AppendLine("union all");
                        sb.AppendFormat("select :array_id, {0} from dual",
                            ArrayStorage.TypedValueString(value, datatype));
                    }

                    sb.AppendLine();
                }

                // нет данных
                if (first) continue; 

                sb.AppendLine(")");

                var par = new OracleParameter("array_id", OracleDbType.NVarChar) { Value = array_id };
                DataHelper.SqlExecute(sb.ToString(), new[] { par }, _connection);   
            }
        }
        public static void ClearArrayStorage(string array_id)
        {
            var sb = new StringBuilder();
            sb.AppendLine("delete from vr_array_storage where array_id = :array_id");

            var par = new OracleParameter("array_id", OracleDbType.NVarChar) { Value = array_id };
            DataHelper.SqlExecute(sb.ToString(), new[] { par }, _connection);
        }
        public static DataTable GetFromArrayStorage(string array_id, string datatype = "number")
        {
            var sb = new StringBuilder();
            sb.AppendFormat("select {0} from vr_array_storage where array_id = :array_id", ArrayStorage.DataColumnName(datatype));
            sb.AppendLine();

            var par = new OracleParameter("array_id", OracleDbType.NVarChar) { Value = array_id };
            var dt = DataHelper.SqlGetTable(sb.ToString(), new[] { par }, _connection);

            return dt;
        }*/
        #endregion

        public static bool CheckTableExists(string table_name, string owner = null)
        {
            if (owner == null) owner = "user";
            else owner = "upper('" + owner + "')";

            var sql = string.Format("select count(1) from all_tables where table_name = '{0}' and owner = {1}", table_name.ToUpper(), owner.ToUpper());
            var res = DataHelper.SqlGetDecimal(sql, Connection);
            return (res > 0);
        }
        /*public static string[] GetSchemeNameByTable(string table_name)
        {
           

            var sql = string.Format("select distinct owner v from all_tables where table_name = '{0}'", table_name.ToUpper());
            var res = DataHelper.SqlGetTable(sql, Connection);
            return res.AsEnumerable().Select(r=>(string)r["v"]).ToArray();
        }*/
        /*
        public static bool CheckViewExists(string view_name, string owner = null)
        {
            if (owner == null) owner = "user";
            else owner = "upper('" + owner + "')";

            var sql = string.Format("select count(1) from all_views where view_name = '{0}' and owner = {1}", view_name.ToUpper(), owner.ToUpper());
            var res = DataHelper.SqlGetDecimal(sql, Connection);
            return (res > 0);
        }
        */
        /*public static DataTable GetTables(string owner = null)
        {
            if (owner == null) owner = "user";
            else owner = "upper('" + owner + "')";

            var sql = string.Format("select * from all_tables where owner = {0} order by table_name", owner);
            var res = DataHelper.SqlGetTable(sql, Connection);
            return res;
        }*/

        /*public static DataTable GetViews(string owner = null)
        {
            if (owner == null) owner = "user";
            else owner = "upper('" + owner + "')";

            var sql = string.Format("select * from all_views where owner = {0} order by view_name", owner);
            var res = DataHelper.SqlGetTable(sql, Connection);
            return res;
        }*/

        public static DataTable GetTableColumns(string table_name, string owner = null)
        {
            if (owner == null) owner = "user";
            else owner = "upper('" + owner + "')";

            var sql = string.Format("select * from all_tab_columns where table_name = '{0}' and owner = {1} order by column_id", table_name.ToUpper(), owner);
            var res = DataHelper.SqlGetTable(sql, Connection);
            return res;
        }

        public static DataTable GetTableConstraints(string table_name, string owner = null)
        {
            if (owner == null) owner = "user";
            else owner = "upper('" + owner + "')";

            var sql = string.Format("select * from all_constraints where table_name = '{0}' and owner = {1}", table_name.ToUpper(), owner.ToUpper());
            var res = DataHelper.SqlGetTable(sql, Connection);
            return res;
        }
        /*public static DataTable GetConstraintColumns(string constraint_name, string owner = null)
        {
            if (owner == null) owner = "user";
            else owner = "upper('" + owner + "')";

            var sql = string.Format("select * from all_cons_columns where constraint_name = '{0}' " +
                                   // " and owner = {1} " +
                                    "order by position", constraint_name.ToUpper(), owner.ToUpper());
            var res = DataHelper.SqlGetTable(sql, Connection);
            return res;
        }*/

        /*public static DataTable GetTableConstraintColumns(string table_name, string owner = null)
        {
            if (owner == null) owner = "user";
            else owner = "upper('" + owner + "')";

            var sql = string.Format("select * from all_cons_columns where table_name = '{0}' and owner = {1} order by position", table_name.ToUpper(), owner.ToUpper());
            var res = DataHelper.SqlGetTable(sql, Connection);
            return res;
        }*/

        /*public static DataTable GetTablePublicSynonyms(string table_name)
        {
            var sql = string.Format("select * from all_synonyms where table_name = '{0}' and owner = 'PUBLIC'", table_name.ToUpper());
            var res = DataHelper.SqlGetTable(sql, Connection);
            return res;
        }*/

        public static DataTable GetTableStuct(string table, string owner)
        {
            string sql = string.Format(
            @"select t.table_name, t.column_name, t.data_type, c.comments,
                     (select 1 from user_constraints con, user_cons_columns coll 
                      where con.table_name = t.table_name
                      and coll.column_name = t.column_name
                      and con.constraint_type = 'P'
                      and con.constraint_name = coll.constraint_name
                     ) is_pk, t.nullable,t.data_length
             from all_tab_columns t, all_col_comments c
             where t.table_name = '{0}' and t.owner = '{1}' and t.table_name=c.table_name and t.column_name = c.column_name and c.owner = '{1}'
             order by 5, t.column_id", table.ToUpper(), owner.ToUpper());

            return DataHelper.SqlGetTable(sql, Connection);
        }

        /*public static DataTable GetTableStuctConstraints(string table, string owner)
        {
            string sql = string.Format(
                @"select c.constraint_type, c.search_condition, nvl(cc2.table_name,cc.table_name) as table_name, cc.column_name col1, cc2.column_name col2
                from all_constraints c, all_cons_columns cc, all_constraints c2 , all_cons_columns cc2
                where c.table_name = '{0}' and c.owner = '{1}'
                and c.constraint_name = cc.constraint_name and cc.owner = '{1}'
                and c.r_constraint_name = c2.constraint_name(+) and c2.owner = '{1}'
                and c2.constraint_name = cc2.constraint_name(+)  and cc2.owner = '{1}'", table.ToUpper(), owner.ToUpper());

            return DataHelper.SqlGetTable(sql, Connection);
        }*/

        #region vr_packages
        /*public static DataTable GetPackages()
        {
            return SqlMethods.Select("vr_packages", Connection);
        }*/
        #endregion
        #region vr_package_reposrts
        /*public static DataTable GetPackageReports()
        {
            return SqlMethods.Select("vr_package_reports", Connection);
        }*/
        #endregion
    }
}
