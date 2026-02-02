using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.XmlHelpers
{
    internal static class QlikView
    {
        #region Закрытые переменные
        // занятые заголовки
        private static List<string> qv_titles;
        #endregion

        // Обновляет view в базе, из которых будут брать данные скрипты qlikview
        public static void UpdateViews(string qvproject = null)
        {
            // список занятых заголовков
            qv_titles = new List<string>();

            // Получаем список xml-описаний всех запросов для QlikView
            var queries = GetQlikViewQueries(qvproject);

            PrepareQueries(queries);

            var vds_qlikview = queries.Select(que => GenerateQVReport(que).Result(2,false));

            foreach (VDataSet vds in vds_qlikview)
            {
                // Не забыть прописать для QLIKVIEW
                // GRANT SELECT ANY TABLE TO QLIKVIEW

                // Создаем view с префиксом "VR_"
                var sql = String.Format(@"CREATE OR REPLACE VIEW CLICK.{0} AS {1}",
                            vds.Report.Attribute("name").Value,
                            (vds.Tables[0] as VDataTable).DataAdapter.SelectCommand.CommandText);

                XmlReports.executeNonQuery(sql, db.Connection);
                //DataHelper.SqlExecute(sql, XmlReports.CurrentConnection);
            }
        }

        // Генерирует скрипт LoadData.qvs
        public static void UpdateScripts(string qvproject = null)
        {
            // список занятых заголовков
            qv_titles = new List<string>();

            // Получаем список xml-описаний всех запросов для QlikView
            var queries = GetQlikViewQueries(qvproject);

            PrepareQueries(queries);
            
            //скрипт для загрузки из файлов qvd 
            var scriptdata = new StringBuilder();

            var script = GenerateQVScript(queries, out scriptdata);

            //var cur_directory = Properties.Settings.Default.workFolder;
            //if (!Directory.Exists(cur_directory)) cur_directory = Environment.CurrentDirectory;
            var cur_directory = @"\\DFS01\Common\QlikView\infoenergo";
            //var cur_directory = @"C:\";

            using (var file_stream = new FileStream(Path.Combine(cur_directory, (qvproject ?? "infoenergo") + ".qvs"), FileMode.Create))
            {
                using (var writer = new StreamWriter(file_stream, Encoding.UTF8))
                {
                    writer.Write(script);
                }
            }

            // загрузка KIDO
            //using (var file_stream = new FileStream(Path.Combine(cur_directory, "infoenergo.kido.qvs"), FileMode.Create))
            ////using (var file_stream = new FileStream(Path.Combine(cur_directory, "infoenergo.kaz.teplo.qvs"), FileMode.Create))
            //{
            //    using (var writer = new StreamWriter(file_stream, Encoding.UTF8))
            //    {
            //        writer.Write(scriptdata);
            //    }
            //}
        }

        #region Закрытые методы
        private static void PrepareQueries(IEnumerable<XElement> queries)
        {
            // Заголовки для запросов
            foreach (var query in queries)
            {
                ShowInQlikView(query, XmlReports.GetXElementTitle(query));
            }

            // Единый список всех узлов column и call из всех запросов
            var columns = queries
                .SelectMany(que => que.Element("select").Elements()
                                    .Where(col => (col.Name.LocalName == "call" || col.Name.LocalName == "column") 
                                                   && XmlReports.GetXAttributeValue(col,"qlikview") != "0")).ToArray();

            foreach (var column in columns)
            {
                // если колонка уже обработана - пропускаем
                if (XmlReports.GetXAttributeValue(column, "qv_title") != "") continue;

                // Заголовок текущей колонки
                var qv_title = GenerateQVTitle(column);

                var ref_columns = column.Name.LocalName == "column"
                    ? GetRefColumns(column, columns)
                    : Enumerable.Empty<XElement>();

                if (ref_columns.Any())
                {
                    // Если есть колонки, коорые ссылаются на текущую,
                    // Объединяем текущую и связанные колонки одним заголовком QlikView
                    ShowInQlikView(column, qv_title);
                    qv_titles.Add(qv_title);

                    // Затычка, чтобы не создавать более одной связи между двумя таблицами (создается только первая)
                    var qv_tables = new List<string>();
                    foreach (var ref_column in ref_columns)
                    {
                        if (qv_tables.Contains(ref_column.Parent.Parent.Attribute("name").Value)) continue;

                        ShowInQlikView(ref_column, qv_title);
                        qv_tables.Add(ref_column.Parent.Parent.Attribute("name").Value);
                    }
                }
                else if (XmlReports.GetXAttributeValue(column, "key") == "1" 
                      || XmlReports.GetXAttributeValue(column, "title") != ""
                      || XmlReports.GetXAttributeValue(column, "qlikview") == "1")
                {
                    // Колонки без связей, но являющиеся ключами или с заголовком тоже помечаются для QlikView
                    ShowInQlikView(column, qv_title);
                    qv_titles.Add(qv_title);
                }
            }
        }

        private static string GenerateQVTitle(XElement column)
        {
            string qv_title = XmlReports.GetXElementTitle(column);

            // Если колонке еще не присвоен заголовок в QlikView
            if (XmlReports.GetXAttributeValue(column, "qv_title") == "")
            {
                int postfix = 1;
                string new_qv_title = qv_title;
                // Если заголовок уже занят, ищем свободный, добавляя цифру
                while (qv_titles.Contains(new_qv_title))
                {
                    new_qv_title = qv_title + (postfix++);
                }
                qv_title = new_qv_title;
            }
            return qv_title;
        }
        private static VReport GenerateQVReport(XElement query)
        {
            // Копируем все колонки запроса в новый элемент
            var qv_columns = new XElement("qv_columns",
                 query.Element("select").Elements()
                    .Where(col => (col.Name.LocalName == "column" || col.Name.LocalName == "call") 
                                  && XmlReports.GetXAttributeValue(col, "qlikview") == "1"));

            // Устанавливаем некоторые атрибуты для скопированых колонок
            foreach (var qv_column in qv_columns.Descendants("column"))
            {
                XmlReports.SetXElementAttribute(qv_column, "table", "a");
                XmlReports.SetXElementAttribute(qv_column, "column", XmlReports.GetXElementName(qv_column));
            }

            // Оборачиваем запрос
            var report_query =  new XElement("query", 
                      new XAttribute("title", XmlReports.GetXAttributeValue(query, "title")),
                      new XAttribute("qlikview", XmlReports.GetXAttributeValue(query, "qlikview")),
                      new XAttribute("qv_title", XmlReports.GetXAttributeValue(query, "qv_title")),

                      new XElement("select", 
                          qv_columns.Elements().Select(e=>new XElement("column",new XAttribute("table","a"),new XAttribute("column",e.Attribute("as").Value)

                              , Compiler.copyAttribute(e, "qlikview")
                              
                              ))), 
                      new XElement("from",
                         new XElement("query",
                            new XAttribute("name", query.Attribute("name").Value),
                            new XAttribute("as", "a"))));

            // Генерируем отчёт

            

            var report = XmlReports.Environment.GetPrecompiledReport(report_query);
            // Добавляем отчёту имя 
            XmlReports.SetXElementAttribute(report, "name", "vr_" + XmlReports.GetXAttributeValue(query, "name"));
            // для запроса из хранилища
            report.SetAttributeValue("use-repository", "1");
            return report;
        }
        private static string GenerateQVScript(IEnumerable<XElement> queries, out StringBuilder scriptdata)
        {
            var script = new StringBuilder();

            // скрипт загрузки данных для основного проекта
            scriptdata = new StringBuilder();

            // основные параметры
            scriptdata.AppendLine("SET ThousandSep=' ';");
            scriptdata.AppendLine("SET DecimalSep=',';");
            scriptdata.AppendLine("SET MoneyThousandSep=' ';");
            scriptdata.AppendLine("SET MoneyDecimalSep=',';");
            scriptdata.AppendLine("SET MoneyFormat='# ##0,00р.;-# ##0,00р.';");
            scriptdata.AppendLine("SET TimeFormat='h:mm:ss';");
            scriptdata.AppendLine("SET DateFormat='DD.MM.YYYY';");
            scriptdata.AppendLine("SET TimestampFormat='DD.MM.YYYY h:mm:ss[.fff]';");
            scriptdata.AppendLine("SET MonthNames='Январь;Февраль;Март;Апрель;Май;Июнь;Июль;Август;Сентябрь;Октябрь;Ноябрь;Декабрь';");
            scriptdata.AppendLine("SET DayNames='Пн;Вт;Ср;Чт;Пт;Сб;Вс';");
            scriptdata.AppendLine();
            scriptdata.AppendLine("Directory;");


            script.AppendLine("SET ThousandSep=' ';");
            script.AppendLine("SET DecimalSep=',';");
            script.AppendLine("SET MoneyThousandSep=' ';");
            script.AppendLine("SET MoneyDecimalSep=',';");
            script.AppendLine("SET MoneyFormat='# ##0,00р.;-# ##0,00р.';");
            script.AppendLine("SET TimeFormat='h:mm:ss';");
            script.AppendLine("SET DateFormat='DD.MM.YYYY';");
            script.AppendLine("SET TimestampFormat='DD.MM.YYYY h:mm:ss[.fff]';");
            script.AppendLine("SET MonthNames='Январь;Февраль;Март;Апрель;Май;Июнь;Июль;Август;Сентябрь;Октябрь;Ноябрь;Декабрь';");
            script.AppendLine("SET DayNames='Пн;Вт;Ср;Чт;Пт;Сб;Вс';");
            script.AppendLine();
            //script.AppendLine("OLEDB CONNECT32 TO [Provider=OraOLEDB.Oracle.1;Persist Security Info=True;User ID=qlikview;Data Source=devle;Extended Properties=\"\"] (XPassword is JPdWTYFORDbB);");
            //// для Ленэнерго
            script.AppendLine("OLEDB CONNECT32 TO [Provider=OraOLEDB.Oracle.1;Persist Security Info=False;User ID=click;Data Source=alpha;Extended Properties=\"\"] (XPassword is EccFaRRNBbYWWLA);");
            // для Казани
            //script.AppendLine("OLEDB CONNECT32 TO [Provider=OraOLEDB.Oracle.1;Persist Security Info=False;User ID=click;Data Source=asuse_o;Extended Properties=\"\"] (XPassword is EccFaRRNBbYWWLA);");
            
            var all_qv_columns = queries.SelectMany(query => new []
            {
                query.Element("select").Elements("column")
                    .Where(col => XmlReports.GetXAttributeValue(col, "qlikview") == "1"),
                query.Element("select").Elements("call")
                    .Where(col => XmlReports.GetXAttributeValue(col, "qlikview") == "1")
            }.SelectMany(col => col));

            foreach (var query in queries)
            {
                script.AppendLine();

                script.AppendLine(String.Format("\"{0}\":",
                    XmlReports.GetXAttributeValue(query, "qv_title")));

                script.AppendLine("SQL SELECT");

                var columns = all_qv_columns.Where(col => col.Parent.Parent == query);

                var rename_fields = new Dictionary<string, string>();
                foreach (var column in columns)
                {
                    var qv_title = XmlReports.GetXAttributeValue(column, "qv_title");
                    if (qv_title.Length > 30)
                    {
                        rename_fields.Add(qv_title.Substring(0,30), qv_title);
                        qv_title = qv_title.Substring(0, 30);
                    }

                    script.AppendLine(String.Format("\t\"{0}\" AS \"{1}\"{2}",
                        XmlReports.GetXAttributeValue(column, "as").ToUpper(),
                        qv_title,
                        //XmlReports.GetXAttributeValue(column, "qv_title"),
                        columns.Last() != column ? "," : ""));
                }
                // надо придумать проверку на случай одинаковых заголовков после substr. может обрезать еще при создании qv_title?
                script.AppendLine(String.Format("FROM CLICK.\"VR_{0}\";",
                    XmlReports.GetXAttributeValue(query, "name").ToUpper()));

                if (rename_fields.Count > 0)
                {
                    script.AppendLine();
                    var fields = String.Join(", ", rename_fields.Select(rf => string.Format(@"""{0}"" TO ""{1}""", rf.Key, rf.Value)));
                    script.AppendLine(string.Format("RENAME FIELDS {0};", fields));
                }

                script.AppendLine();

                script.AppendLine(String.Format("STORE \"{0}\" into \".\\infoenergo.qvd\\{0}.qvd\" (qvd);",
                    XmlReports.GetXAttributeValue(query, "qv_title")));

                script.AppendLine(String.Format("DROP TABLE \"{0}\";",
                    XmlReports.GetXAttributeValue(query, "qv_title")));

                // для загрузки данных из файлов
                scriptdata.AppendLine();
                scriptdata.AppendLine(String.Format(@"LOAD * FROM [infoenergo\infoenergo.qvd\{0}.qvd] (qvd);", XmlReports.GetXAttributeValue(query, "qv_title")));
            }

            GenerateScriptDateSplits(all_qv_columns, script, scriptdata);

            return script.ToString();
        }

        private static void GenerateScriptDateSplits(IEnumerable<XElement> columns, StringBuilder script, StringBuilder scriptdata)
        {
            // Достаем все уникальные имена колонок с атрибутом qv_split = 1
            var split_col_names = columns
                .Where(el => XmlReports.GetXAttributeValue(el,"qv_split") == "1")
                .Select(el => el.Attribute("qv_title").Value)
                .Distinct();

            // Групируем по qv_title - получаем для каждого qv_title отдельный список колонок 
            // (для случая, если колонка в нескольких таблицах)
            var grouped_split_cols = columns
                .Where(col => split_col_names.Contains(col.Attribute("qv_title").Value))
                .GroupBy((col) => col.Attribute("qv_title").Value, 
                         (qv_title, cols) => cols.Where(col => col.Attribute("qv_title").Value == qv_title));

            foreach (var split_cols in grouped_split_cols)
            {
                var first_column = split_cols.First();

                //!! 1 !! Собираем все даты в одну общую временную таблицу
                foreach (var split_col in split_cols)
                {
                    var split_qv_title = XmlReports.GetXAttributeValue(split_col, "qv_title");
                    var query = split_col.Parent.Parent;

                    // UNION
                    if (split_col == first_column)
                    {
                        script.AppendLine();
                        script.AppendLine(String.Format("\"{0} (temp)\":", split_qv_title));
                    }
                    else
                    {
                        script.AppendLine(String.Format("CONCATENATE (\"{0} (temp)\")", split_qv_title));
                    }

                    script.AppendLine("SQL SELECT");

                    // Дата
                    script.AppendLine(String.Format("\t\"{0}\" AS \"{1}\"",
                            XmlReports.GetXAttributeValue(split_col, "as").ToUpper(),
                            split_qv_title));

                    script.AppendLine(String.Format("FROM CLICK.\"VR_{0}\";",
                        XmlReports.GetXAttributeValue(query, "name").ToUpper()));
                }

                //!! 2 !! Из временной таблицы загружаем все уникальные даты
                var qv_title = XmlReports.GetXAttributeValue(first_column, "qv_title");

                script.AppendLine();

                script.AppendLine(String.Format("\"{0}\":", qv_title));
                script.AppendLine("LOAD");

                // Дата
                script.AppendLine(String.Format("\tDISTINCT \"{0}\" AS \"{0}\",", qv_title));
                // Год
                script.AppendLine(String.Format("\tYear(\"{0}\") AS \"{1}\",", qv_title, GetSplitDateTitle(qv_title, 0)));

                // Квартал
                script.AppendLine(String.Format("\tIf(Month(\"{0}\") <= 3, 'I квартал',{2}\t\tIf(Month(\"{0}\") <= 6, 'II квартал',{2}\t\tIf(Month(\"{0}\") <= 9, 'III квартал', 'IV квартал'))){2}\tAS \"{1}\",",
                        qv_title, GetSplitDateTitle(qv_title, 1), Environment.NewLine));

                // Месяц
                script.AppendLine(String.Format("\tMonth(\"{0}\") AS \"{1}\",", qv_title, GetSplitDateTitle(qv_title, 2)));

                // День
                script.AppendLine(String.Format("\tDay(\"{0}\") AS \"{1}\"", qv_title, GetSplitDateTitle(qv_title, 3)));

                script.AppendLine(String.Format("RESIDENT \"{0} (temp)\";", qv_title));

                script.AppendLine();

                script.AppendLine(String.Format("STORE \"{0}\" into \".\\infoenergo.qvd\\{0}.qvd\" (qvd);",
                    XmlReports.GetXAttributeValue(first_column, "qv_title")));

                script.AppendLine(String.Format("DROP TABLE \"{0}\";",
                    XmlReports.GetXAttributeValue(first_column, "qv_title")));

                //!! 3 !! Удаляем временную таблицу с датами
                script.AppendLine();
                script.AppendLine(String.Format("DROP TABLE \"{0} (temp)\";", qv_title));

                // для загрузки данных из файлов
                scriptdata.AppendLine();
                scriptdata.AppendLine(String.Format(@"LOAD * FROM [infoenergo\infoenergo.qvd\{0}.qvd] (qvd);", XmlReports.GetXAttributeValue(first_column, "qv_title")));
            }
        }

        private static void ShowInQlikView(XElement element, string title)
        {
            XmlReports.SetXElementAttribute(element, "qv_title", title);
            if (element.Attribute("qlikview") == null)
                XmlReports.SetXElementAttribute(element, "qlikview", "1");
            
        }
        private static string GetSplitDateTitle(string qv_title, int type)
        {
            // type: 0 - год, 1 - квартал, 2 - месяц, 3 - день

            var names = new[] {"Год", "Квартал", "Месяц", "День"};
            if (type > names.Length - 1) return qv_title;

            if (qv_title.Trim().StartsWith("Дата"))
            {
                return qv_title.Replace("Дата", names[type]);
            }
            else if (qv_title.Contains("дата"))
            {
                return qv_title.Replace("дата", names[type].ToLower());
            }
            else return String.Format("{0} ({1})", qv_title, names[type].ToLower());
        }

        private static XElement GetQueryXml(string query_name, IEnumerable<VSXElement> scheme)
        {
            // исходный запрос
            var non_compiled_query = new XElement( XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements().First(que => que.Attribute("name").Value == query_name));

            // колонки после обработки запроса
            //var compiled_columns = XElement.Parse(XmlReports.getItemProcessedXml("query", query_name, false).InnerXml)
            //    .Element("query").Element("select").Elements()
            //    .Where(el => el.Name.LocalName == "column" || el.Name.LocalName == "call")
            //    .OrderBy(col => col.Attribute("as").Value).ToArray();

            var compiled_columns = 
               Compiler.compileQuery(query_name, scheme)
               .Element("query").Element("select").Elements()
               .Where(el => el.Name.LocalName == "column" || el.Name.LocalName == "call")
               .OrderBy(col => col.Attribute("as").Value).ToArray();

            // колонки до обработки запроса
            var non_compiled_columns = non_compiled_query.Element("select").Elements()
                .Where(el => el.Name.LocalName == "column" || el.Name.LocalName == "call")
                .OrderBy(col => col.Attribute("as").Value).ToArray();
            
            // так быть не должно
            if (compiled_columns.Length != non_compiled_columns.Length) return null;

            // берем title из обработаных колонок и засовываем в необработанные
            //for (int i = 0; i < compiled_columns.Length; i++)
            //{
            //    Cmn.CopyAttribute(compiled_columns[i], non_compiled_columns[i], "title");
            //    if (XmlReports.GetXAttributeValue(compiled_columns[i], "title") == "") continue;
            //    XmlReports.SetXElementAttribute(non_compiled_columns[i], "title", XmlReports.GetXElementTitle(compiled_columns[i]));
            //}


            non_compiled_query.Element("select").Elements().Remove();
            non_compiled_query.Element("select").Add(compiled_columns);
            // необработанный запрос с заменеными title
            return non_compiled_query;
        }
        private static IEnumerable<XElement> GetRefColumns(XElement column, IEnumerable<XElement> all_columns)
        {
            var column_query = column.Parent.Parent;
            var main_table_names = new[] { "a", "*", "this" };

            //// Получаем список колонок, которые ссылаются на текущую и для которых есть xml-описание запроса
            //return all_columns.Where(col => col.Parent.Parent.Attribute("name").Value != column.Parent.Parent.Attribute("name").Value
            //    && XmlReports.GetXAttributeValue(col, "reference") == column.Parent.Parent.Attribute("name").Value && XmlReports.GetXAttributeValue(col, "refcol") == XmlReports.GetXAttributeValue(column, "as")
            //    && all_queries.Select(que => XmlReports.GetXAttributeValue(que, "name")).Contains(XmlReports.GetXAttributeValue(col, "reference")))
            //    .ToArray();
           
            return all_columns.Where(col =>
            {
                // Запрос, в котором сидит связанная колонка
                var ref_column_query = col.Parent.Parent;

                if (column_query == ref_column_query) return false;

                // Все связи с таблицами того запроса, в котором сидит связанная колонка
                var all_rel_queries = new [] 
                {
                    ref_column_query.Element("from") != null 
                        ? ref_column_query.Element("from").Elements("query").Where(que => XmlReports.GetXAttributeValue(que,"qlikview") != "0")
                        : Enumerable.Empty<XElement>(), 
                    ref_column_query.Element("push") != null 
                        ? ref_column_query.Element("push").Element("from").Elements("query").Where(que => XmlReports.GetXAttributeValue(que,"qlikview") != "0")
                        : Enumerable.Empty<XElement>()
                }.SelectMany(que => que);

                // Если есть наследование - достаем имена родительских запросов
                var inherit_names = new List<string>();
                var inherit_query = column_query;
                while(true)
                {
                    var inherit_name = XmlReports.GetXAttributeValue(inherit_query, "inherit");
                    if (inherit_name == "") break;

                    inherit_names.Add(inherit_name);
                    inherit_query = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements().First(que => que.Attribute("name").Value == inherit_name);
                } 

                // Описание связи с таблицей (родительской либо текущей) текущей колонки (если есть)
                var ref_query = all_rel_queries.FirstOrDefault(que => que.Attribute("name").Value == column_query.Attribute("name").Value
                                                                   || inherit_names.Contains(que.Attribute("name").Value ));
                if(ref_query != null)
                {
                    // Проверка происходит ли связь через те самые колонки
                    var rel_cols = ref_query.Descendants("column");
                    return rel_cols.Any(rel => main_table_names.Contains(rel.Attribute("table").Value) && rel.Attribute("column").Value == XmlReports.GetXElementName(col))
                        && rel_cols.Any(rel => rel.Attribute("table").Value == XmlReports.GetXElementName(ref_query) && rel.Attribute("column").Value == XmlReports.GetXElementName(column));
                }
                else return false;
            }).ToArray();
        }
        private static IEnumerable<XElement> GetQlikViewQueries(string qvproject = null)
        {
            var scheme = XmlReports.Environment.Manager.GetScheme();
            // Получаем список xml-описаний всех запросов для QlikView
            return scheme
                    .Elements("qvprojects")
                    .Elements("qvproject")
                    // все если qvproject = null
                    .Where(el => qvproject == null || el.Attribute("name").Value == qvproject)
                    .Elements("queries")
                    .Elements("query")
                    .Select(q => GetQueryXml(q.Attribute("name").Value, scheme))
                    .ToArray();
        }
        #endregion
    }
}
