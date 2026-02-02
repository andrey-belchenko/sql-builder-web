using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using sql.builder.DataApi;
using sql.builder.UI;
namespace sql.builder.XmlHelpers
{
    internal static class SqlRepository
    {
        // пока сделаю для одного хранилища - VR_IPR, 
        // но дальше, возможно, понадобится генерировать для всех
        private static string query_name = "ipr_fin_body_united";
        private static string query_changed_name;
        private static string dim_tab_name;

        private static XElement changes_scheme;
        private static VQuery query;
        private static XElement query_changed;
        private static XElement query_for_update;

      //  private static string key_changed_name;

        private static string query_sql;
        private static string query_upd_sql;
        private static string delete_cond;
        private static string query_changed_sql;

        private static string rep_name;
        private static string dateChangeName = "date_change";
        private static StringBuilder sql;

        static void Initialize()
        {
           
            UIStatic.LoadProject("ipr");
            query = XmlReports.Environment.GetPrecompiledQuery(query_name);
          
            query_changed_name = query.GetAttrValue("changes");

            rep_name = query.GetAttrValue("stored").ToLower();
            dim_tab_name=rep_name + "_dims";
            query_changed = GetQueryChanged();
            changes_scheme = XmlReports.Environment.GetPrecompiledReport(query_changed).Scheme;
            query_sql = RepositoriesHelper.GetSelectSql(query);
            query_changed_sql = RepositoriesHelper.GetSelectSql(query_changed);
            query_for_update = GetQueryForUpdate();
         //   key_changed_name = query_changed.Element("select").Elements().First().Attribute("as").Value;

            query_upd_sql = RepositoriesHelper.GetSelectSql(query_for_update);
            delete_cond = GetDeleteCond();
            sql = new StringBuilder();
        }

        public static string GetDeleteCond()
        {
            string sql = "exists (select * from "+dim_tab_name+" dim where ";
            string a = "";
            foreach (XElement col in changes_scheme.Descendants("table").First().Element("columns").Elements())
            {
                if (col.Attribute("name").Value != dateChangeName && col.Attribute("name").Value != "name")
                {
                    sql += a + "dim." + col.Attribute("name").Value + "=" + "a." + col.Attribute("name").Value;
                   
                     a = " and ";
                }


            }
            sql += ")";
            return sql;
        }

        public static string GetPackageSql()
        {
            Initialize();

            sql.AppendLine("CREATE OR REPLACE PACKAGE VG_DWH_" + rep_name + " AUTHID CURRENT_USER");
            sql.AppendLine("IS");
            sql.AppendLine("\ts_tmp varchar2(200);");
            sql.AppendLine("\tn_tmp number;");
            sql.AppendLine();
            sql.AppendLine("\tPROCEDURE FULL_REFRESH;");
            sql.AppendLine("\tPROCEDURE PARTIAL_REFRESH;");
            //sql.AppendLine("\tPROCEDURE " + rep_name + "_UPD;");
            sql.AppendLine("END;");
            sql.AppendLine();
            sql.AppendLine("CREATE OR REPLACE PACKAGE BODY VG_DWH_" + rep_name + "");
            sql.AppendLine("IS");

            // полностью
            sql.AppendLine("\tPROCEDURE FULL_REFRESH");
            sql.AppendLine("\tIS");
            sql.AppendLine("\t\ts_result varchar2(200);");
            sql.AppendLine("\t\td_start date;");
            sql.AppendLine("\t\tv_tablespace varchar2(100);");
            sql.AppendLine("\t\tv_exists number;");
            sql.AppendLine("\tBEGIN");          
           
            AddLockRepositorySql("s_result", 2);

            sql.AppendLine("\t\t--Если хранилище уже заблокировано - ничего не делаем");
            sql.AppendLine("\t\tif (s_result is not null) then");

            AddLogSql("Блокировка не установлена", "'Хранилище уже заблокировано пользователем '||s_result", 3);

            sql.AppendLine("\t\t\treturn;");
            sql.AppendLine("\t\tend if;");

            AddLogSql("Блокировка установлена", "''", 2);
            AddLogSql("Начало формирования", "''", 2);

            sql.AppendLine("\t\td_start := sysdate;");
            sql.AppendLine("\t\t--Формирование");

            AddDropTableSql(rep_name + "_NEW", "v_exists", 2);

            sql.AppendLine("\t\tselect max(tbs) into v_tablespace from");
            sql.AppendLine("\t\t\t(select 'OTHER' as tbs from dual union select 'ASUSE_TBL' as tbs from dual) a,");
            sql.AppendLine("\t\t\t(select * from all_tables where table_name='" + rep_name + "') b");
            sql.AppendLine("\t\t\twhere a.tbs = b.tablespace_name(+) and TABLE_NAME is null;");
            sql.AppendLine("\t\t--из за нехватки места чередуем tablespace 'ASUSE_TBL','OTHER', временно");
            sql.AppendLine("\t\tv_tablespace:='ASUSE_IDX'");
            sql.AppendLine(String.Format("\t\texecute immediate 'create table {0}_NEW tablespace '|| v_tablespace ||' as (select * from {0}_VIEW)';", rep_name));

            AddDropTableSql(rep_name, "v_exists", 2);

            sql.AppendLine(String.Format("\t\texecute immediate 'rename {0}_NEW to {0}';", rep_name));
            sql.AppendLine(String.Format("\t\texecute immediate 'grant select on {0} to public';", rep_name));

            AddIndexesExprSql(2);
            AddRepFormInfoSql("d_start", 2);
            AddLogSql("Успешно сформировано", "''", 2);
            AddUnlockRepositorySql(2);
            AddLogSql("Блокировка снята", "''", 2);

            sql.AppendLine();
            sql.AppendLine("\t\tEXCEPTION");
            sql.AppendLine("\t\t\tWHEN OTHERS THEN");
            sql.AppendLine("\t\t\t\trollback;");
            sql.AppendLine("\t\t\t\ts_result := SUBSTR (SQLERRM, 1, 500);");

            AddLogSql("Ошибка формирования", "s_result", 4);
            AddUnlockRepositorySql(4);
            AddLogSql("Блокировка снята", "''", 4);

            sql.AppendLine("\tEND;");


            // по изменениям



            sql.AppendLine("\tPROCEDURE PARTIAL_REFRESH");
            sql.AppendLine("\tIS");
            sql.AppendLine("\t\ts_result varchar2(200);");
            sql.AppendLine("\t\td_start date;");
            sql.AppendLine("\t\tv_tablespace varchar2(100);");
            sql.AppendLine("\t\tv_exists number;");
            sql.AppendLine("\t\ts_info varchar2(470);");
            sql.AppendLine("\tBEGIN");

            AddLockRepositorySql("s_result", 2);

            sql.AppendLine("\t\t--Если хранилище уже заблокировано - ничего не делаем");
            sql.AppendLine("\t\tif (s_result is not null) then");

            AddLogSql("Блокировка не установлена", "'Хранилище уже заблокировано пользователем '||s_result", 3);

            sql.AppendLine("\t\t\treturn;");
            sql.AppendLine("\t\tend if;");

            AddLogSql("Блокировка установлена", "''", 2);
            sql.AppendLine("\t\td_start := sysdate;");
            sql.AppendLine("\t\t--Получаем список измененных кодов");
            AddDropTableSql(dim_tab_name, "v_exists", 2);
            sql.AppendLine(String.Format("\t\texecute immediate 'create table {0}  as (select * from {1}_view_dim)';",dim_tab_name ,rep_name));
            sql.AppendLine("\t\t--Если хранилище ни разу не формировалось полностью - список измененных кодов так же будет пустым");
            sql.AppendLine("\t\texecute immediate 'select substr(stragg_dist(name), 1, 470)  from " + dim_tab_name + "' into s_info;");
            sql.AppendLine("\t\t--Если кодов нет, значит и обновлять нечего");
            sql.AppendLine("\t\tif(s_info is null) then");
            sql.AppendLine("\t\tupdate vr_repository_info set DATE_START_UPD=d_start where upper (rep_table)=upper('" + rep_name + "');");
            AddLogSql("Обновление прервано", "'Нет измененных данных - обновление не требуется'", 3);
            AddUnlockRepositorySql(3);
            AddLogSql("Блокировка снята", "''", 3);

            sql.AppendLine("\t\t\treturn;");
            sql.AppendLine("\t\tend if;");


            //////////////
            AddLogSql("Начало обновления", "s_info", 2);
            sql.AppendLine("\t\t--Удаление старых данных по условию");
            sql.AppendLine(String.Format("\t\texecute immediate 'delete {0} a where {1}';", rep_name, delete_cond));

            sql.AppendLine("\t\texecute immediate 'insert into " + rep_name + " (select * from " + rep_name + "_view_upd)';");
       
        


            sql.AppendLine("\t\tupdate vr_repository_info set DATE_START_UPD=d_start where upper (rep_table)=upper('" + rep_name + "');");
            sql.AppendLine("\t\tcommit;");
            AddLogSql("Успешно обновлено", "''", 2);
            AddUnlockRepositorySql(2);
            AddLogSql("Блокировка снята", "''", 2);

           
            /////////////////

            
            sql.AppendLine();
            sql.AppendLine("\t\tEXCEPTION");
            sql.AppendLine("\t\t\tWHEN OTHERS THEN");
            sql.AppendLine("\t\t\t\trollback;");
            sql.AppendLine("\t\t\t\ts_result := SUBSTR (SQLERRM, 1, 500);");

            AddLogSql("Ошибка формирования", "s_result", 4);
            AddUnlockRepositorySql(4);
            AddLogSql("Блокировка снята", "''", 4);

            sql.AppendLine("\tEND;");





            sql.AppendLine("END;");
            sql.AppendLine("/");
            sql.AppendLine();
            AddPackageGrantsSql();

            return sql.ToString();
        }

        public static string GetViewSql()
        {
            Initialize();
            query_sql = query_sql.Replace("stragg_dist", "max"); // заплатка , валится ORA-01467: sort key too long
            return String.Format("CREATE OR REPLACE VIEW {0}_VIEW AS {1}", rep_name, query_sql);
        }

        public static string GetViewForUpdSql()
        {
            Initialize();
            query_upd_sql = query_upd_sql.Replace("stragg_dist", "max"); // заплатка , валится ORA-01467: sort key too long
            return String.Format("CREATE OR REPLACE VIEW {0}_VIEW_UPD AS {1}", rep_name, query_upd_sql);
        }

        public static string GetViewChangedSql()
        {
            Initialize();
            return String.Format("CREATE OR REPLACE VIEW {0}_VIEW_DIM AS {1}", rep_name, query_changed_sql);
        }

        private static XElement GetQueryForUpdate()
        {
            XElement qry = new XElement(query);
            qry.Element("push").Element("from").Add(GetChangeJoinElement());
            return qry;
        }

        private static XElement GetQueryChanged()
        {
            XElement qry = XmlReports.Environment.GetPrecompiledQuery(query_changed_name);
            qry=new XElement(qry);

            qry.SetAttributeValue("as","a");
            qry.Attributes("name").Remove();
            XElement qry1=new XElement("query",new XElement("select"),new XElement("from",qry),new XElement("where"
                , new XElement("call", new XAttribute("function", "gt")
                   , new XElement("column", new XAttribute("table", "a"), new XAttribute("column", "date_change"))
                   , new XElement("const", new XText("(select nvl (DATE_START_UPD,DATE_START) from vr_repository_info where upper(REP_TABLE)=upper('"+rep_name+"'))"))
                )
                
                ));
            XElement splitter=null;
            XElement nameExpr = new XElement("call", new XAttribute("function", "||"), new XAttribute("as", "name"));
            foreach (XElement col in qry.Element("select").Elements())
            {
                XElement col1 = new XElement("column", new XAttribute("table", "a"), new XAttribute("column", col.Attribute("as").Value));
                if (col.Attribute("as").Value == "date_change")
                {
                    col1.SetAttributeValue("group", "max");
                }
                else
                {
                    col1.SetAttributeValue("group", "1");
                }
                qry1.Element("select").Add(col1);
                if (col.Attribute("as").Value != dateChangeName)
                {
                    nameExpr.Add(splitter, new XElement(col1));
                    splitter = new XElement("const", new XText("'#'"));
                }
            }
            qry1.Element("select").Add(nameExpr);
            return qry1;
        }
        private static XElement GetChangeJoinElement()
        {
            XElement qry = new XElement("query",new XElement("select"),new XElement("from"));
            qry.SetAttributeValue("as", "changes");
            qry.SetAttributeValue("join", "inner");
            qry.Add(new XElement("call", new XAttribute("function", "and")));
            qry.Element("from").Add(new XElement("table", new XAttribute("name", dim_tab_name), new XAttribute("as", "a")));
            foreach (XElement col in changes_scheme.Descendants("table").First().Element("columns").Elements())
            {
                if (col.Attribute("name").Value != dateChangeName && col.Attribute("name").Value !="name")
                {
                    XElement srcCol = query.Element("select").Elements().Where(e => e.Attribute("as").Value == col.Attribute("name").Value).First();
                    XElement chCol = new XElement("column", new XAttribute("table", "a"), new XAttribute("column", col.Attribute("name").Value));
                    qry.Element("select").Add(chCol);
                    XElement chCol1 = new XElement(chCol);
                    chCol1.SetAttributeValue("table", "changes");
                    srcCol = new XElement(srcCol);

                    srcCol.Attributes().Where(e => !(new string[] { "table", "column" }).Contains(e.Name.LocalName)).Remove();

                    XElement el = new XElement("call", new XAttribute("function", "="));
                    el.Add(srcCol);
                    el.Add(chCol1);
                    qry.Element("call").Add(el);
                }

            }


            return qry;
        }


        private static void AddLogSql(string action, string text, int tabs = 0)
        {
            // если text не переменная - обернуть в ''
            var stabs = new String('\t', tabs);
            sql.AppendLine(stabs + "--Логирование");
            sql.AppendLine(stabs + String.Format("insert into vr_repository_log(rep_table,action,text) values('{0}','{1}',{2});", rep_name, action, text));
            sql.AppendLine(stabs + "commit;");
        }
        private static void AddLockRepositorySql(string result_var_name, int tabs = 0)
        {
            var stabs = new String('\t', tabs);
            sql.AppendLine(stabs + "--Попытка заблокировать хранилище");
            sql.AppendLine(stabs + String.Format("{0} := kg_common.lock_dog('sql.builder_{1}',0,0);", 
                result_var_name, rep_name));
        }
        private static void AddUnlockRepositorySql(int tabs = 0)
        {
            var stabs = new String('\t', tabs);
            sql.AppendLine(stabs + "--Разблокируем хранилище");
            sql.AppendLine(stabs + String.Format("kg_common.unlock_dog('sql.builder_{0}',0);", rep_name));
        }
        private static void AddRepFormInfoSql(string dstart_var_name, int tabs = 0)
        {
            var stabs = new String('\t', tabs);
            sql.AppendLine(stabs + "--Фиксация информации о хранилище в vr_repository_info, если сформировано");
            sql.AppendLine(stabs + String.Format("merge into vr_repository_info m using dual on (upper(rep_table) = upper('{0}')) ", rep_name));
            sql.AppendLine(stabs + String.Format("when not matched then insert (rep_table, date_start, date_end) values ('{0}', {1}, sysdate)", rep_name, dstart_var_name));
            sql.AppendLine(stabs + String.Format("when matched then update set date_start = {0}, date_end = sysdate;", dstart_var_name));
            sql.AppendLine(stabs + "commit;");
        }
        private static void AddRepUpdInfoSql(string dstart_var_name, int tabs = 0)
        {
            var stabs = new String('\t', tabs);
            sql.AppendLine(stabs + "--Фиксация информации о хранилище в vr_repository_info, если обновлено");
            sql.AppendLine(stabs + String.Format("merge into vr_repository_info m using dual on (upper(rep_table) = upper('{0}')) ", rep_name));
            sql.AppendLine(stabs + String.Format("when not matched then insert (rep_table, date_start, date_end, date_start_upd, date_end_upd) values ('{0}', {1}, sysdate, {1}, sysdate)", rep_name, dstart_var_name));
            sql.AppendLine(stabs + String.Format("when matched then update set date_start_upd = {0}, date_end_upd = sysdate;", dstart_var_name));
            sql.AppendLine(stabs + "commit;");
        }
        private static void AddPackageGrantsSql()
        {


            sql.AppendLine("--Grants for Package");
            sql.AppendLine("GRANT DEBUG ON VG_DWH_" + rep_name + " TO public");
            sql.AppendLine("/");
            sql.AppendLine("GRANT EXECUTE ON VG_DWH_" + rep_name + " TO public");
            sql.AppendLine("/");
        }
        private static void AddDropTableSql(string table_name, string exists_var_name, int tabs = 0)
        {
            var stabs = new String('\t', tabs);
            sql.AppendLine(stabs + String.Format("select nvl((select 1 from all_tables where table_name=upper('{0}')),0) into {1} from dual;", table_name, exists_var_name));
            sql.AppendLine(stabs + String.Format("if ({0} = 1) then", exists_var_name));
            sql.AppendLine(stabs + String.Format("\texecute immediate 'drop table {0} purge';", table_name));
            sql.AppendLine(stabs + "end if;");
        }
        private static void AddIndexesExprSql(int tabs = 0)
        {
            var stabs = new string('\t', tabs);
            foreach (VSXElement dim in query.DimensionsOld()) {
                sql.AppendLine(String.Format(stabs + "execute immediate 'CREATE INDEX {0}_{1} ON {0} ({1} ASC)';", rep_name, dim.XName));
            }
        }
    }
}
