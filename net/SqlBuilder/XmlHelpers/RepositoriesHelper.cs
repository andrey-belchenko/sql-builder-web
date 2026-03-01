using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using Devart.Data.Oracle;

using sql.builder.DataApi;
using SqlBuilderLib.DevTools;

namespace sql.builder.XmlHelpers
{
    public static class RepositoriesHelper
    {
        private static DataTable _dt_repositories;
        private static List<string> _excepted_queries;

        // !!!ВЕмцов Генерацию Sql пакета и view перенес в класс SqlRepository

        public static DataTable GetQueryRepositories(XElement query)
        {
            _dt_repositories = new DataTable();
            _excepted_queries = new List<string>();

            _dt_repositories.Columns.AddRange(new[]
            {
                new DataColumn("rep_table", typeof (string)),
                new DataColumn("query_name", typeof (string)),
                new DataColumn("parent_query_name", typeof (string)),
                new DataColumn("query_title", typeof (string)),
                new DataColumn("date_form", typeof (DateTime))
            });

            foreach (var que in query.Element("queries").Descendants("query")) FillQueryRepository(que, "");

            return _dt_repositories;
        }




        public static string GetSelectSql(string query_name)
        {
            return ((VDataTable)XmlReports.Environment.GetPrecompiledReport(query_name).Result(2, false).Tables[0]).DataAdapter.SelectCommand.CommandText;
        }

        public static string GetSelectSql(XElement xquery)
        {
            xquery = new XElement(xquery);



            XElement xquery1 = null;
            if (xquery.Elements("select").Elements().Any(e => Cmn.GetAttrValue(e, "stored") == "0") ||
            xquery.Elements("select").Elements().Any(e => Cmn.GetAttrValue(e, "virtual") == "1"))
            {

                var unusedCols = xquery.Elements("select").Elements().Where(e => Cmn.GetAttrValue(e, "stored") == "0").Select(e1 => e1.Attribute("as").Value).ToList();
                unusedCols.AddRange(
                     xquery.Elements("select").Elements().Where(e => Cmn.GetAttrValue(e, "virtual") == "1").Select(e1 => e1.Attribute("as").Value).ToList()
                    );

                var xquery2 = Compiler.GetCompiledQuery(xquery).Elements().First();

                xquery.SetAttributeValue(TextConst.AName.As, "a");
                xquery.Attributes("name").Remove();
                xquery1 = new XElement(TextConst.EName.Query
                   , new XElement(TextConst.EName.Select)
                   , new XElement(TextConst.EName.From
                        //,  new XElement (TextConst.EName.Query
                        //,new XAttribute(TextConst.AName.Name,xquery.Attribute(TextConst.AName.Name).Value)
                        // , new XAttribute(TextConst.AName.As, "a")
                        //)
                        , xquery
                 )

                   );




                foreach (XElement col in xquery2.Elements(TextConst.EName.Select).Elements().ToList())
                {
                    //if ((new string[] { "kod_ipr", "nzs_itog" }).Contains(col.Attribute(TextConst.AName.As).Value))

                    if (!unusedCols.Contains(col.Attribute(TextConst.AName.As).Value))
                    {
                        xquery1.Element(TextConst.EName.Select).Add(new XElement(TextConst.EName.Column
                            , new XAttribute(TextConst.AName.Table, "a")
                                , new XAttribute(TextConst.AName.Column, col.Attribute(TextConst.AName.As).Value)
                                  , new XAttribute(TextConst.AName.As, col.Attribute(TextConst.AName.As).Value)
                            ));
                    }
                }
            }
            else
            {
                xquery.Attributes("name").Remove();
                xquery1 = xquery;
            }
            //xquery.Elements("select").Elements().Where(e => Cmn.GetAttrValue(e, "stored") == "0").Remove();
            //xquery.Elements("select").Elements().Where(e => Cmn.GetAttrValue(e, "virtual") == "1").Remove();

            return ((VDataTable)XmlReports.Environment.GetPrecompiledReport(xquery1).Result(2, false).Tables[0]).DataAdapter.SelectCommand.CommandText;
        }
        #region Закрытые методы
        private static void FillQueryRepository(XElement query_link, string parent_query_name)
        {
            // Имя запроса для хранилища
            var query_name = query_link.Attribute("name").Value;
            _excepted_queries.Add(query_name);

            // Достаем описание запроса
            var query = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements()
                .FirstOrDefault(que => que.Attribute("name").Value == query_link.Attribute("name").Value);
            if (query == null) return;

            var rep_table = XmlReports.GetXAttributeValue(query, "stored").ToUpper();
            if (rep_table != "" && _dt_repositories.AsEnumerable().All(row => (string)row["rep_table"] != rep_table))
            {
                // Имя хранилища
                var query_title = query_link.Attribute("title") != null
                    ? query_link.Attribute("title").Value
                    : (query.Attribute("title") != null
                        ? query_link.Attribute("title").Value
                        : query_link.Attribute("name").Value);
                // Дата последнего заполнения хранилища
                string sql = String.Format(@"select date_start, date_end, date_start_upd, date_end_upd from vr_repository_info where upper(rep_table) = upper('{0}')", rep_table);
                var dt_report_info = db.ExecuteDataTable(sql, db.Connection);

                object date_form = null;
                if (dt_report_info.Rows.Count == 1)
                {
                    date_form = dt_report_info.Rows[0]["date_start_upd"];
                    if (date_form == DBNull.Value) date_form = dt_report_info.Rows[0]["date_start"];
                }

                _dt_repositories.Rows.Add(rep_table, query_name, parent_query_name, query_title, date_form);
            }

            // то же самое проделываем для подзапросов, на которые ссылается текущий запрос
            var subquery_links = query.Element("from").Descendants("query").Where(que => que.Attribute("name") != null);
            foreach (var subquery_link in subquery_links)
            {
                if (_excepted_queries.Contains(subquery_link.Attribute("name").Value)) continue;
                FillQueryRepository(subquery_link, query_name);
            }
        }
        #endregion

        #region Устаревшее
        public static Tuple<bool, string> ExecuteRepository(string query_name, string rep_table)
        {
            var result = LockRepository(rep_table);
            if (!result.Item1) return result;
            try
            {
                AddLog(rep_table, "Начало формирования", "");

                // Текст select-а для заполнения хранилища
                var select_sql = ((VDataTable)XmlReports.Environment.GetPrecompiledReport(query_name).Result(2, false).Tables[0]).DataAdapter.SelectCommand.CommandText;
                // Список колонок для insert-а в таблицу
                var into_columns = String.Join(",", XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements()
                                       .First(que => que.Attribute("name").Value == query_name)
                                       .Element("select").Elements()
                                       .Select(col => col.Attribute("as").Value));

                // Скрипт, обновляющий хранилище
                string sql = String.Format(
                @"declare
                    d_start date;
                  begin
                    d_start := sysdate;

                    delete {0}; 

                    insert into {0}({1})
                    (
                        {2}
                    );

                    -- Фиксация информации о хранилище в vr_repository_info, если сформировано
                    merge into vr_repository_info m using dual on (rep_table = '{0}')  
                    when not matched 
                        then insert (rep_table, date_start, date_end) values ('{0}', d_start, sysdate)   
                    when matched 
                        then update set date_start = d_start, date_end = sysdate;
                  end;", rep_table, into_columns, select_sql);

                XmlReports.executeNonQuery(sql, db.Connection);
                db.Connection.Commit();

                AddLog(rep_table, "Окончание формирования", "");

                return new Tuple<bool, string>(true, "ok");
            }
            catch (Exception ex)
            {
                db.Connection.Rollback();
                AddLog(rep_table, "Ошибка формирования", ex.Message);
                throw;
            }
            finally
            {
                UnlockRepository(rep_table);
            }
        }
        private static void AddLog(string rep_table, string action, string text)
        {
            var sql = String.Format(@"insert into vr_repository_log(rep_table,action,text)
                                      values('{0}','{1}','{2}')", rep_table, action, text);

            XmlReports.executeNonQuery(sql, db.Connection);
            db.Connection.Commit();
        }
        private static Tuple<bool, string> LockRepository(string rep_table)
        {
            using (var cmd = db.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "kg_common.lock_dog";
                var parameters = new[]
                {
                    new OracleParameter("s_pref"  , OracleDbType.NVarChar) { Value = "sql.builder_" + rep_table},
                    new OracleParameter("nkod_dog", OracleDbType.Number)   { Value = 0M },
                    new OracleParameter("nwait"   , OracleDbType.NVarChar) { Value = 0M },
                    new OracleParameter("return"  , OracleDbType.NVarChar) { Direction = ParameterDirection.ReturnValue}
                };
                cmd.Parameters.AddRange(parameters);
                DevUtilsProvider.Instance.AnalyzeExecSql(cmd.CommandText);
                cmd.ExecuteNonQuery();

                var result = parameters[3].Value;
                if (result != DBNull.Value)
                {
                    AddLog(rep_table, "Блокировка не установлена", "Хранилище уже заблокировано пользователем " + result);

                    var date_blocked = GetLastTimeBlocked(rep_table);
                    result = String.Format("{0}Пользователь: {2}{0}Время блокировки хранилища: {1}",
                           Environment.NewLine, ((DateTime)date_blocked).ToString("HH:mm:ss dd.MM.yyyy"), result);

                    return new Tuple<bool, string>(false, (string)result);
                }
            }

            AddLog(rep_table, "Блокировка установлена", "");
            return new Tuple<bool, string>(true, "ok");
        }
        private static void UnlockRepository(string rep_table)
        {
            using (var cmd = db.Connection.CreateCommand())
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.CommandText = "kg_common.unlock_dog";
                var parameters = new[]
                {
                    new OracleParameter("s_pref"  , OracleDbType.NVarChar) { Value = "sql.builder_" + rep_table},
                    new OracleParameter("nkod_dog", OracleDbType.Number)   { Value = 0M },
                };
                cmd.Parameters.AddRange(parameters);
                DevUtilsProvider.Instance.AnalyzeExecSql(cmd.CommandText);
                cmd.ExecuteNonQuery();

                AddLog(rep_table, "Блокировка снята", "");
            }
        }
        private static DateTime? GetLastTimeBlocked(string rep_table)
        {
            var sql = String.Format(@"select max(date_log) from vr_repository_log 
                                       where rep_table = '{0}' and action = 'Блокировка установлена'", rep_table);

            var date_log = db.ExecuteDataTable(sql, db.Connection).Rows[0][0];
            return date_log != DBNull.Value ? (DateTime?)date_log : null;
        }
        public static string LongStringToVariable(string str, string varName)
        {
            string s = "";
            int i = 0;

            while (i < str.Length - 3995)
            {
                string ss = s.Substring(i, 3995);
                i = i + 3995;
                s += varName + ":=" + varName + "+'" + ss + "';";
                s += Environment.NewLine;
            }
            return s;
        }
        public static string RefreshByInsertExpr(string query_name, string rep_table, int insertPortionSize = 0)
        {
            // не хватает места в undotbl чтобы выполнить вставку сразу всех записей, insertPortionSize - к-во записей после которого выполняется commit
            //var select_sql = GetSelectSql(query_name, rep_table);

            var into_columns = String.Join(",", XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements()
                                         .First(que => que.Attribute("name").Value == query_name)
                                         .Element("select").Elements()
                                         .Select(col => col.Attribute("as").Value));

            //   string rec_columns;
            string sql = "";
            {
                // !!! Дописать если понадобится создание простого insert...select всех записей без цикла 
            }

            return sql;



        }
        #endregion
    }
}
