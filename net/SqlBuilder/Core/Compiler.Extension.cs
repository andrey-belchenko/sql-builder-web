using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using sql.builder.Clean.Extensions;
//using infoenergo.core.Extensions;
using sql.builder.DataApi;

namespace sql.builder
{
    public partial class Compiler
    {



        private static void compressTables(XElement xelement)//доработать - пока не используется
        {

            foreach (XElement xtable in xelement.Elements(TextConst.EName.Table).ToList())
            {
                var xquery = xtable.Parent.Parent;
                var xfrom = xtable.Parent;

                if (xfrom.Elements().Count() > 1) continue;

                var allowedSectionsNames = new string[] {
                TextConst.EName.Select,
                TextConst.EName.From,
                TextConst.EName.Call};



                if (xquery.Elements().Any(e => !allowedSectionsNames.Contains(e.Name.LocalName))) continue;

                if (xquery.Elements(TextConst.EName.Select).Elements().Any(e => e.Name.LocalName != TextConst.EName.Column)) continue;




                var tableAlias = xtable.Attribute(TextConst.AName.As).Value;

                if (xquery.Elements(TextConst.EName.Select).Elements().Any(e => e.Attribute(TextConst.AName.Table).Value != tableAlias)) continue;


                var xtable1 = new XElement(TextConst.EName.Table);

                Cmn.copyAttributes(xtable, xtable1);

                xtable1.CopyAttributes(xquery.Attributes());

                xtable1.Add(xquery.Elements(TextConst.EName.Call));

                xquery.ReplaceWith(xtable1);

            }

        }



        #region Step6
        public static void Step6(XElement xelement)
        {
            // compressTables(xelement);
            if (xelement.Name.LocalName == "query")
            {
                GenerateInsertForQueryM(xelement);
                GenerateGroupForQuery(xelement);
                // GenerateNullsCast(xelement);
            }
            GenerateOrderBy(xelement);

            // рекурсивная обработка дочерних элементов
            foreach (var xchild in xelement.Elements().ToArray())
            {
                // обрабатываем элемент, а если не используется - удаляем
                if (xchild.AttrOrDef("used", "") != "0")
                {
                    Step6(xchild);
                }
                else xchild.Remove();
            }
        }

        // генерация xml для записи во временную таблицу
        private static void GenerateInsertForQueryM(XElement query)
        {
            var query_materialize = query.AttrOrDef("materialize", "");
            if (GetAncestorName(query) == "root" && query_materialize == "1")
            {
                var select = query.Element("select");
                if (select == null) return;

                // генерируем описание новых колонок
                var new_cols_array = new[]
                {
                    new XElement("column",
                        new XAttribute("column", "skod"),
                        new XAttribute("info", "skod")),
                    new XElement("column",
                        new XAttribute("column", "sid"),
                        new XAttribute("info", "sid")),
                    new XElement("column",
                        new XAttribute("column", "rn"),
                        new XAttribute("info", "rn")),
                };

                // генерируем описание существующих колонок

                var cols = select.Elements();
                var badTypeCol = cols.FirstOrDefault(e => Cmn.GetAttrValue(e, "into").StartsWith(Compiler.badTypePref));
                if (badTypeCol != null)
                {
                    throw new sql.builder.Exceptions.VCompilerException("Требуется тип данных для колонки " + badTypeCol.Attribute("as").Value, query, badTypeCol);
                }
                var existed_cols_array = cols
                    .Where(e => Cmn.GetAttrValue(e, TextConst.AName.ClientCalulation) != TextConst.AVBool.True)
                    .Select(el => new XElement("column",
                        new XAttribute("column", el.Attribute("into").Value),
                        new XAttribute("info", el.Attribute("as").Value)));

                // получаем единый список всех колонок
                var all_temp_cols = new[] { new_cols_array, existed_cols_array }.SelectMany(col => col);


                // добавляем элемент insert
                query.AddFirst(new XElement("insert",
                                 new XAttribute("into", TextConst.DBObjects.TempTable),
                                 all_temp_cols));
            }
        }
        // генерация xml если есть колонки с группировкой
        private static void GenerateGroupForQuery(XElement query)
        {
            var select = query.Element("select");
            if (select == null) return;

            // XSLT like
            if (select.Elements().All(el => el.AttrOrDef("group", "") != "1"))
            {
                return;
            }

            var query_cols_group = select.Descendants()
                .Where(el => el.AttrOrDef("used", "") != "0"
                          && el.AttrOrDef("group", "") == "1"
                          // группировка под partition by не относится к запросу
                          // && el.Parent.AttrOrDef("function", "") != "partition by"
                          // групировка под pivot не относится к запросу
                          //  && !el.Ancestors("pivot").Any()
                          // колонки из подзапросов не берем
                          && el.Ancestors("query").First() == query);
            if (query.Descendants("having").Any())
            {

            }

            // последний дочерний элемент query
            var last_element = query.Elements().Last();
            // если call - вставляем перед ним, иначе - после него
            if (last_element.Name.LocalName == "call")
            {
                last_element.AddBeforeSelf(new XElement("group", query_cols_group));
            }
            else
            {
                last_element.AddAfterSelf(new XElement("group", query_cols_group));
            }
        }
        // явно указываем null-ам тип, если это возможно
        private static void GenerateNullsCast(XElement query)
        {
            XElement[] uqueries = null;

            // query под select-ом из union запроса
            var uparent = query.Ancestors("query").FirstOrDefault(q => q.AttrOrDef("union", "") == "1");
            if (uparent != null)
            {
                var cols_nulls = query.Element("select").Elements().Where(c => string.Equals(c.Value, "null", StringComparison.OrdinalIgnoreCase)).ToArray();
                if (cols_nulls.Length != 0)
                {
                    uqueries = uparent.Ancestors("query").First().Elements("query").Where(q => q.AttrOrDef("union", "") == "1").ToArray();
                    foreach (var cols_null in cols_nulls)
                    {
                        var pcol = uqueries.Elements("select").Elements().FirstOrDefault(c => c.AttrOrDef("as", "") == cols_null.Attribute("as").Value);
                        var pos = pcol.ElementsBeforeSelf().Count();

                        // все элементы в колонке с индексом pos
                        var cols = uqueries.Elements("select").Select(s => s.Elements().ElementAtOrDefault(pos)).Where(e => e != null).ToArray();
                        // нет ни одной колонки с таким индексом
                        if (cols.Length != 0)
                        {
                            // определяем тип колонки и явно приводим к нему null если это возможно
                            var type = cols.Attributes("type").Select(a => a.Value).FirstOrDefault();
                            switch (type)
                            {
                                case "number": cols_null.Value = "cast(null as number)"; break;
                                case "date": cols_null.Value = "cast(null as date)"; break;
                                default: cols_null.Value = "cast(null as number)"; break;
                            }
                        }
                    }
                }

                return;
            }

            // union запрос
            uqueries = query.Elements("query").Where(q => q.AttrOrDef("union", "") == "1").ToArray();
            if (uqueries.Length != 0)
            {
                var i = 0;
                while (true)
                {
                    i++;
                    // все элементы в колонке с индексом i
                    var cols = uqueries.Elements("select").Select(s => s.Elements().ElementAtOrDefault(i)).Where(e => e != null).ToArray();
                    // нет ни одной колонки с таким индексом
                    if (cols.Length == 0) break;
                    // содержат только null
                    var cols_nulls = cols.Where(c => string.Equals(c.Value, "null", StringComparison.OrdinalIgnoreCase)).ToArray();
                    // нет null 
                    if (cols_nulls.Length == 0) continue;

                    // определяем тип колонки и явно приводим к нему null если это возможно
                    var type = cols.Attributes("type").Select(a => a.Value).FirstOrDefault();
                    switch (type)
                    {
                        case "number": foreach (var cn in cols_nulls) cn.Value = "cast(null as number)"; break;
                        case "date": foreach (var cn in cols_nulls) cn.Value = "cast(null as date)"; break;
                    }
                }
            }
        }

        // генерация узла order для query и call
        private static void GenerateOrderBy(XElement xelement)
        {
            if (xelement.Name.LocalName == "query")
            {
                var parent_name = GetAncestorName(xelement);
                var order_atr = xelement.AttrOrDef("order", "");
                //var materialize_atr = xelement.AttrOrDef("materialize", ""); 

                if (order_atr != "" //&& materialize_atr != "1" && materialize_atr != "2"
                && (parent_name == "root" || (parent_name != "root" && !xelement.Elements("call").Any())))
                {
                    xelement.Add(new XElement("order"));
                }
                return;
            }

            if (xelement.Name.LocalName == "call")
            {
                var parent_name_atr = xelement.Parent.AttrOrDef("name", "");
                var parent_order_atr = xelement.Parent.AttrOrDef("order", "");
                var parent_materialize_atr = xelement.Parent.AttrOrDef("materialize", "");

                if ((parent_name_atr == "query" || parent_name_atr == "table")
                    && parent_order_atr != "" && parent_materialize_atr != "2")
                {
                    xelement.Add(new XElement("order"));
                }
                return;
            }
        }
        #endregion
        #region Step7
        public static void Step7(XElement xelement)
        {
            // Продолжаем развлекаться!  \O__
            //                            |
            //                           / \

            SqlGenFactory.Get(xelement.Name.LocalName).Generate(xelement);

            // рекурсивная обработка дочерних элементов
            foreach (var xElement in xelement.Elements()) Step7(xElement);
        }
        #endregion

        private static int stringId = 0;
        private static SortedList<string, int> stringsIds = new SortedList<string, int>();
        private static int getStringCode(string s)
        {

            if (!stringsIds.ContainsKey(s))
            {
                stringId++;
                stringsIds.Add(s, stringId);
            }
            return stringsIds[s];
        }


        public static void SimplifySubquery(XElement elements)
        {
            return;
            /*
            //эта часть вроде работает , сократила запрос с 4000 до 3000 строк, большого смысла нет, пока отключаю чтобы не рисковать
            foreach (XElement tbl in elements.Descendants(TextConst.EName.Table).ToArray())
            {
                var qry = tbl.Parent.Parent;

                var joinConds= qry.Elements(TextConst.EName.Call).ToArray();
                var nonJoinSections = qry.Elements().Where(e => e.Name.LocalName != TextConst.EName.Call).ToArray();

                if (qry.Name.LocalName != TextConst.EName.Query) continue;

                if (nonJoinSections.Descendants(TextConst.EName.Call).Any()) continue;

                if (nonJoinSections.Descendants(TextConst.EName.Const).Any()) continue;

                if (qry.Elements(TextConst.EName.Select).Elements().Where(e => e.Attribute(TextConst.AName.Column).Value != e.Attribute(TextConst.AName.As).Value).Any()) continue;

                var xtbl = new XElement(TextConst.EName.Table);

                Cmn.copyAttributes(qry, xtbl);
                xtbl.Add(joinConds);
                var tname = tbl.Attribute(TextConst.AName.Name).Value;

                //xtbl.SetAttributeValue(TextConst.AName.Name, "(select * from  " + tname + ")");
                xtbl.SetAttributeValue(TextConst.AName.Name, tname);
                qry.ReplaceWith(xtbl);

            }*/
        }

        public static void CutIdentifiersTo30(XElement elements)
        {
            // return;
            CutIdentifiersTo30(
                    elements.Descendants(TextConst.EName.Column).Attributes(TextConst.AName.As)
                    );

            CutIdentifiersTo30(
              elements.Descendants(TextConst.EName.Const).Attributes(TextConst.AName.As)

              );

            CutIdentifiersTo30(
              elements.Descendants(TextConst.EName.Call).Attributes(TextConst.AName.As)

              );
            CutIdentifiersTo30(
               elements.Descendants(TextConst.EName.Column).Attributes(TextConst.AName.Column)
               );
            CutIdentifiersTo30(
               elements.Descendants(TextConst.EName.Column).Attributes(TextConst.AName.Name)
               );
            CutIdentifiersTo30(
               elements.Descendants(TextConst.EName.Query).Attributes(TextConst.AName.As)
               );
            CutIdentifiersTo30(
               elements.Descendants(TextConst.EName.Column).Attributes(TextConst.AName.Table)
               );
        }


        public static void CutIdentifiersTo30(IEnumerable<XAttribute> attrs)
        {

            foreach (XAttribute attr in attrs.Where(a => a.Value.Length > 30).ToList())
            {

                attr.Value = TextConst.Pfx.CutedId + getStringCode(attr.Value).ToString();



            }
        }

        public static string GetSql(XElement xroot)
        {

            xroot.Descendants(TextConst.EName.QubeContent).Remove();
            var text = new StringBuilder();
            GetSqlFromNode(xroot, text);
            return text.ToString();
        }

        public static string GetQuerySelectStatmentFromCompiledQuery(XElement compiledQuery)
        {
            compiledQuery = new XElement(compiledQuery);
            compiledQuery.Elements().Where(e => e.Attribute(TextConst.AName.Materialize) != null).Remove();
            return Environment.NewLine + GetSql(compiledQuery);
        }

        public static string GetQuerProcedureFromCompiledQuery(XElement compiledQuery)
        {
            compiledQuery = new XElement(compiledQuery);
            compiledQuery.Elements().Where(e => e.Attribute(TextConst.AName.Materialize) == null).Remove();

            if (compiledQuery.Elements().Any())
            {
                return "begin" + Environment.NewLine + GetSql(compiledQuery) + Environment.NewLine + "end;" + Environment.NewLine;
            }
            else
            {
                return null;
            }
        }

        public static XElement GetCompiledQuery(XElement query)
        {
            XElement compiledQuery = PreCompileQuery(query, true);
            compiledQuery = compileQuery(compiledQuery, true, null);

            AddMaterializedToResult(compiledQuery);


            return compiledQuery;
        }

        public static XElement GetCompiledAndProcessedQuery(XElement query)
        {
            XElement compiledQuery = PreCompileQuery(query, true);
            compiledQuery = compileQuery(compiledQuery, true, null);

            AddMaterializedToResult(compiledQuery);

            compiledQuery = XmlReports.finalProcessing(compiledQuery);

            return compiledQuery;
        }

        public static XElement FinalProcessingQuery(XElement compiledQuery)
        {

            compiledQuery = XmlReports.finalProcessing(compiledQuery);

            return compiledQuery;
        }

        public static void GetSqlFromNode(XElement xnode, StringBuilder text)
        {
            foreach (var node in xnode.Nodes())
            {
                if (node.NodeType == XmlNodeType.Text)
                {
                    text.Append(((XText)node).Value);

                }
                else if (node.NodeType == XmlNodeType.Element)
                {
                    var xelement = (XElement)node;

                    // если узел не помечен, что его и дочерний текст обрабатывать не нужно
                    if (xelement.AttrOrDef("notext", "") != "1")
                    {
                        GetSqlFromNode(xelement, text);
                    }
                }
            }
        }

        class SqlGen_Query : SqlGen
        {


            private void GenerateInsertByLoop(XElement xelement)
            {
                // из step8
                SqlBefore.AppendLine();
                SqlBefore.AppendLine("delete from rr_temp where skod = '" + Attr("name") + "';");
                // before
                SqlBefore.AppendLine("begin");
                SqlBefore.AppendLine("for rec in");
                SqlBefore.AppendLine("(");

                // after
                SqlAfter.AppendLine(")");
                SqlAfter.AppendLine("loop");
                SqlAfter.AppendLine("insert into " + ChildAttr("insert", "into"));
                SqlAfter.AppendLine("(");

                var columns = CurrentXElement.Element("insert").Elements("column");
                foreach (var column in columns)
                {
                    SqlAfter.Append(column.Attribute("column").Value);
                    if (columns.Last() != column) SqlAfter.Append(", ");
                    SqlAfter.AppendLine("--" + column.Attribute("info").Value);
                }

                SqlAfter.AppendLine(")");
                SqlAfter.AppendLine("values (");

                foreach (var column in columns)
                {
                    SqlAfter.Append("rec." + column.Attribute("info").Value);
                    if (columns.Last() != column) SqlAfter.AppendLine(", ");
                }

                SqlAfter.AppendLine(");");
                SqlAfter.AppendLine("end loop;");
                SqlAfter.AppendLine("end;");
            }


            private void GenerateInsertDirect(XElement xelement)
            {
                // из step8
                SqlBefore.AppendLine();
                SqlBefore.AppendLine("delete from rr_temp where skod = '" + Attr("name") + "';");

                SqlBefore.AppendLine("insert into " + ChildAttr("insert", "into"));
                SqlBefore.AppendLine("(");

                var columns = CurrentXElement.Element("insert").Elements("column");
                foreach (var column in columns)
                {
                    SqlBefore.Append(column.Attribute("column").Value);
                    if (columns.Last() != column) SqlBefore.Append(", ");
                    SqlBefore.AppendLine("--" + column.Attribute("info").Value);
                }

                SqlBefore.AppendLine(")");
                //  SqlBefore.AppendLine("(");
                SqlAfter.AppendLine(";");
                //////////



            }


            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                if (ParentName == "with")
                {
                    // before
                    SqlBefore.AppendLine(Attr("as") + " as");

                    // after
                    //if (!IsLast) 
                    //    SqlAfter.AppendLine(",");
                }

                if (ParentName == "root")
                {
                    // after
                    if (Attr("materialize") == "1")
                    {
                        SqlAfter.AppendLine(") mtr");
                    }
                }

                if (OneChildExists("insert"))
                {
                    if (Cmn.GetAttrValue(CurrentXElement, TextConst.AName.InsByLoop) == TextConst.AVBool.True)
                    {
                        GenerateInsertByLoop(xelement);
                    }
                    else
                    {
                        GenerateInsertDirect(xelement);
                    }
                }

                if (!IsFirst)
                {
                    // before
                    if (Attr("union") == "1") SqlBefore.AppendLine("union all");
                    if (Attr("union") == "2") SqlBefore.AppendLine("union");
                }

                if (ParentName != "root")
                {
                    // before
                    if (Attr("mp") != "") SqlBefore.AppendLine("(");
                    if (Attr("nvl") != "")
                    {
                        SqlBefore.AppendLine("nvl(");
                    }
                    if (Attr("nullif") != "") SqlBefore.AppendLine("nullif(");
                    if (Attr("join") != "" && !IsFirst)
                    {
                        SqlBefore.Append(Attr("join") == "left inner" ? "inner" : Attr("join"));
                        SqlBefore.AppendLine(" join");
                    }
                    SqlBefore.AppendLine("(");

                    if (!ChildrenExists("call"))
                    {
                        //after
                        SqlAfter.AppendLine(")");

                        if (ParentName == "with")
                        {


                            // after
                            if (!IsLast)
                                SqlAfter.AppendLine(",");
                        }

                        if (AttrExists("nvl"))
                        {
                            SqlAfter.AppendLine("," + Attr("nvl") + ")");
                        }
                        if (AttrExists("nullif"))
                        {
                            SqlAfter.AppendLine("," + Attr("nullif") + ")");
                        }
                        if (AttrExists("mp"))
                        {
                            SqlAfter.AppendLine("*power(10," + Attr("mp") + "))");
                        }
                        if (ParentName == "from")
                        {
                            SqlAfter.AppendLine(Attr("as"));
                        }
                    }
                }

                if (ParentName != "root" && (Attr("materialize") == "1" || Attr("materialize") == "2"))
                {
                    // достаем query из схемы
                    var scheme_query = CurrentXElement.Ancestors("root").First().Elements("query")
                        .First(query => query.Attribute("name").Value == Attr("name")
                                        && query.AttrOrDef("materialize", "") == "1");

                    // получаем список колонок для селекта в rr_temp
                    var into_array = CurrentXElement.Element("select").Elements()
                        .Select(col => scheme_query.Element("select").Elements()
                            .First(scol => scol.Attribute("as").Value == col.Attribute("as").Value)
                            .Attribute("into").Value + " as " + col.Attribute("as").Value);

                    // добавляем текст
                    SqlBefore.AppendLine("select /*+ dynamic_sampling(rr_temp 10)*/");

                    if (!(new string[] { TextConst.EName.Call, TextConst.EName.Select }).Contains(ParentName))
                    {

                        SqlBefore.AppendLine("sid,");
                        SqlBefore.AppendLine("sparentid,");
                        SqlBefore.AppendLine("rn,");
                    }
                    SqlBefore.AppendLine(String.Join("," + Environment.NewLine, into_array));
                    SqlBefore.AppendLine("from rr_temp where skod = '" + Attr("name") + "'");

                    // для материализованых запросов остальной текст будет отбрасываться
                    // обработать нужно только узел call
                    foreach (var xchild in CurrentXElement.Elements().Where(el => el.Name.LocalName != "call")) xchild.SetAttributeValue("notext", "1");
                }

                // из step8
                SqlAfter.AppendLine("--\\" + Attr("name"));


                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Select : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // из step8
                if (Parent2Name == "query" || ParentName == "query")
                {
                    SqlBefore.AppendLine("--" + ParentAttr("name"));
                }

                // before
                if (Parent2Name == "root" && ParentAttr("materialize") == "1")
                {
                    // before
                    var key_columns = CurrentXElement.Elements().Where(el => el.AttrOrDef("key", "") == "1");
                    // не совсем корректно, имя может повторяться path и as тоже не подошли
                    string sid = String.Format("'{0}|{1}",
                        ParentAttr("name"),
                        key_columns.Any()
                            ? "#'||" + String.Join("||'#'||", key_columns.Select(col => "mtr." + col.Attribute("as").Value))
                            : "'");

                    SqlBefore.AppendLine("select '" + ParentAttr("name") + "' as skod,");
                    SqlBefore.AppendLine(sid + " as sid,");
                    SqlBefore.AppendLine("row_number() over (order by 1) as rn,");
                    SqlBefore.AppendLine("mtr.*");
                    SqlBefore.AppendLine("from");
                    SqlBefore.AppendLine("(");
                    SqlBefore.Append("select ");
                }
                else
                {
                    SqlBefore.Append("select ");
                }

                if (ParentAttr("hint") != "") SqlBefore.AppendLine("/*+ " + ParentAttr("hint") + "*/");

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Column : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                if (ParentName != "insert")
                {
                    // before
                    if (AttrExists("group") && Attr("group") != "1" && (HasAncestor("select") || HasAncestor("having")))
                    {
                        SqlBefore.Append(
                        (Attr("group") == "sumnvl")
                            ? "sum(nvl("
                            : (Attr("group") == "count_dist")
                                ? " count(distinct "
                                : (Attr("group") + "("));
                    }
                    if (AttrExists("mp")) SqlBefore.Append("(");
                    if (AttrExists("nvl"))
                    {
                        SqlBefore.Append("nvl(");
                    }
                    if (AttrExists("nullif")) SqlBefore.Append("nullif(");

                    if (AttrExists("prior")) SqlBefore.Append("prior ");

                    if (Attr(TextConst.AName.Sys) == TextConst.AVBool.True)
                    {

                        SqlBefore.Append(VSourcedElement.GetSysColDefaultValue(Attr(TextConst.AName.Column)));
                    }
                    else
                    {

                        if (Attr("table") != "") SqlBefore.Append(Attr("table") + ".");
                        SqlBefore.Append(Attr("column"));
                    }

                    //if (Attr("table") != "") SqlBefore.Append(Attr("table") + ".");
                    //SqlBefore.Append(Attr("column"));

                    if (AttrExists("nvl"))
                    {
                        SqlBefore.Append("," + Attr("nvl") + ") ");
                    }
                    if (AttrExists("nullif")) SqlBefore.Append("," + Attr("nullif") + ") ");
                    if (AttrExists("mp")) SqlBefore.Append("*power(10," + Attr("mp") + ")) ");
                    if (AttrExists("group") && Attr("group") != "1"
                       && (HasAncestor("select") || HasAncestor("having")))
                    {
                        SqlBefore.Append((Attr("group") == "sumnvl") ? ",0)) " : ") ");
                    }
                }



                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Call : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // before
                if (ParentName == "query" || ParentName == "table")
                {
                    if (Parent2Name == "root" && ParentAttr("materialize") == "1")
                    {
                        SqlBefore.AppendLine(") mtr");
                    }

                    if (ParentName == "query") SqlBefore.AppendLine(")");
                    SqlBefore.Append(ParentAttr("as") + " on ");
                }

                if (ParentName == "call" && Attr("pth") != "0")
                {
                    // before
                    SqlBefore.Append("(");
                }

                if (AttrExists("mp"))
                {
                    // before
                    SqlBefore.Append("((");
                    // after
                    SqlAfter.Append(" ) *power(10," + Attr("mp") + ")) ");
                }



                if (AttrExists("nullif"))
                {
                    // before
                    SqlBefore.Append("nullif(");
                    // after
                    SqlAfter.Append("," + Attr("nullif") + ") ");
                }

                if (AttrExists("group") && Attr("group") != "1")
                {
                    // before
                    SqlBefore.Append(
                        (Attr("group") == "sumnvl")
                            ? "sum(nvl("
                            : (Attr("group") == "count_dist")
                                ? " count(distinct "
                                : (Attr("group") + "("));

                }

                if (AttrExists("nvl"))
                {
                    // before
                    SqlBefore.Append("nvl(");
                    // after
                    SqlAfter.Append("," + Attr("nvl") + ") ");
                }


                if (AttrExists("group") && Attr("group") != "1")
                {


                    // after
                    SqlAfter.Append((Attr("group") == "sumnvl") ? ",0)) " : ") ");
                }

                if (ParentName == "call" && Attr("pth") != "0")
                {
                    // after
                    SqlAfter.Append(") ");
                }

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Order : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // Генерируется два узла order. Первый обрабатывать не нужно
                if (IsLast)
                {
                    // after
                    if (ParentName == "query")
                    {
                        SqlAfter.AppendLine();
                        SqlAfter.Append(((BrothersExists("connect"))
                                            ? "order siblings by " + ParentAttr("order")
                                            : "order by " + ParentAttr("order")));
                    }

                    if (ParentName == "call")
                    {
                        SqlAfter.AppendLine();
                        SqlAfter.Append("order by " + ParentAttr("order"));
                    }
                }
                else
                {
                    CurrentXElement.SetAttributeValue("notext", "1");
                }

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Table : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // before
                if (AttrExists("join") && !IsFirst)
                {
                    SqlBefore.Append(Attr("join") == "left inner" ? "inner" : Attr("join"));
                    SqlBefore.AppendLine(" join");
                }
                if (Attr("view") != "1") SqlBefore.AppendLine(Attr("name"));

                if (ParentName == "root" && Attr("materialize") == "1")
                {
                    SqlBefore.AppendLine(") mtr");
                }

                // after
                if (!ChildrenExists("call"))
                {
                    SqlAfter.AppendLine(Attr("as"));
                }

                // из step8
                SqlAfter.AppendLine("--\\" + Attr("name"));

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_From : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // before
                SqlBefore.AppendLine();
                SqlBefore.Append("from ");

                if (!xelement.Elements().Any())
                {
                    SqlBefore.AppendLine(" dual ");
                }

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_With : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // before
                SqlBefore.AppendLine("with");

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Where : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // before


                if (xelement.Elements().Any())
                {

                    SqlBefore.AppendLine("where");
                }

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Connect : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // before
                SqlBefore.AppendLine("connect by nocycle");

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Start : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // before
                SqlBefore.AppendLine();
                SqlBefore.Append("start with ");

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Group : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // before
                if (Attr("gset") == "first") SqlBefore.AppendLine("group by grouping sets ((");
                else if (AttrExists("gset")) SqlBefore.AppendLine("(");
                else SqlBefore.AppendLine(" group by");

                // after
                if (Attr("gset") == "last") SqlAfter.AppendLine("))");
                else if (AttrExists("gset")) SqlAfter.AppendLine("),");

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Having : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // before
                SqlBefore.AppendLine("having");

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Dimension : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                //before
                SqlBefore.AppendLine("model dimension by (");

                //after
                SqlBefore.AppendLine(")");

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Measures : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                //before
                SqlBefore.AppendLine("measures (");

                //after
                SqlBefore.AppendLine(")");

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Insert : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);

                // из step8
                if (Parent2Name == "query" || ParentName == "query")
                {
                    SqlAfter.AppendLine("--" + ParentAttr("name"));
                }

                GenerateDefault();
                Complete();
            }
        }
        class SqlGen_Default : SqlGen
        {
            public override void Generate(XElement xelement)
            {
                Prepare(xelement);
                //
                if (Name != "root")
                {
                    GenerateDefault();
                    Complete();
                }
            }
        }

        abstract class SqlGen
        {
            protected StringBuilder SqlBefore;
            protected StringBuilder SqlAfter;

            protected XElement CurrentXElement;

            public abstract void Generate(XElement xelement);

            protected void GenerateDefault()
            {
                // after

                if (ParentName == "select" || ParentName == "group" || ParentName == "dimension" || ParentName == "measures")
                {

                    if (AttrExists("as") && ParentName != "group")
                    {
                        SqlAfter.Append(" as " + Attr("as"));
                    }
                    if (!IsLast) SqlAfter.Append(", ");

                    if (AttrExists("title")) SqlAfter.Append("/*" + Attr("title") + "*/");
                    if (AttrExists("type")) SqlAfter.Append("/*" + Attr("type") + "*/");
                    if (Attr("key") == "1") SqlAfter.Append("/*key*/");

                    SqlAfter.AppendLine();
                }

            }

            protected void Prepare(XElement xelement)
            {
                SqlBefore = new StringBuilder();
                SqlAfter = new StringBuilder();

                CurrentXElement = xelement;
            }
            protected void Complete()
            {
                if (SqlBefore.Length > 0) CurrentXElement.AddFirst(new XText(SqlBefore.ToString()));
                if (SqlAfter.Length > 0) CurrentXElement.Add(new XText(SqlAfter.ToString()));
            }

            protected void ClearNodeText(XElement xnode)
            {
                foreach (var node in xnode.Nodes())
                {
                    if (node.NodeType == XmlNodeType.Text)
                    {
                        node.Remove();
                    }
                    else if (node.NodeType == XmlNodeType.Element)
                    {
                        ClearNodeText((XElement)node);
                    }
                }
            }
            #region Вспомогательные функции
            /// <summary>
            /// Имя текущего узла
            /// </summary>
            protected string Name
            {
                get { return CurrentXElement.Name.LocalName; }
            }
            /// <summary>
            /// Получить значение атрибута текущего узла 
            /// Если атрибут отсутствует - возвращает пустую строку
            /// </summary>
            protected string Attr(string attr_name)
            {
                // чтобы не смахивало на hint
                return CurrentXElement.AttrOrDef(attr_name, "").Replace("{+}", "");
            }
            /// <summary>
            /// Получить значение атрибута родителя текущего узла 
            /// Если родитель или атрибут отсутствуют - возвращает пустую строку
            /// </summary>
            protected string ParentAttr(string attr_name)
            {
                return CurrentXElement.Parent != null
                    ? CurrentXElement.Parent.AttrOrDef(attr_name, "")
                    : "";
            }
            /// <summary>
            /// Получить значение атрибута дочернего узла, если он один 
            /// Если атрибут отсутствуют - возвращает пустую строку
            /// </summary>
            protected string ChildAttr(string child_name, string attr_name)
            {
                return CurrentXElement.Element(child_name).AttrOrDef(attr_name, "");
            }

            /// <summary>
            /// Проверить наличие атрибута у текущего узла 
            /// </summary>
            protected bool AttrExists(string attr_name)
            {
                return CurrentXElement.Attribute(attr_name) != null;
            }
            /// <summary>
            /// Проверить наличие атрибута у родительского узла 
            /// </summary>
            protected bool ParentAttrExists(string attr_name)
            {
                return (CurrentXElement.Parent != null && CurrentXElement.Parent.Attribute(attr_name) != null);
            }
            /// <summary>
            /// Проверить наличие атрибута у дочернего узла 
            /// </summary>
            protected bool ChildAttrExists(string child_name, string attr_name)
            {
                return CurrentXElement.Element(child_name).Attribute(attr_name) != null;
            }

            /// <summary>
            /// Проверить, что есть только один дочерней узел с заданным именем
            /// </summary>
            protected bool OneChildExists(string child_name)
            {
                return CurrentXElement.Elements(child_name).Count() == 1;
            }
            /// <summary>
            /// Проверить, что существуют дочернее узлы с заданным именем
            /// </summary>
            protected bool ChildrenExists(string child_name)
            {
                return CurrentXElement.Elements(child_name).Any();
            }
            /// <summary>
            /// Проверить, что существуют узлы с заданным именем на том же уровне
            /// </summary>
            protected bool BrothersExists(string brother_name)
            {
                return (CurrentXElement.Parent != null && CurrentXElement.Parent.Elements(brother_name).Any());
            }

            /// <summary>
            /// Имя родительского узла на 1 уровень вверх
            /// </summary>
            protected string ParentName
            {
                get { return CurrentXElement.Parent.Name.LocalName; }
            }
            /// <summary>
            /// Имя родительского узла на 2 уровеня вверх
            /// </summary>
            public string Parent2Name
            {
                get { return CurrentXElement.Parent.Parent.Name.LocalName; }
            }

            /// <summary>
            /// Проверить наличие родителя на любом уровне с определенным именем
            /// </summary>
            protected bool HasAncestor(string ancestor_name)
            {
                return CurrentXElement.Ancestors(ancestor_name).Any();
            }

            /// <summary>
            /// Проверить, является ли узел первым на своем уровне
            /// </summary>
            protected bool IsFirst
            {
                get { return (CurrentXElement.Parent != null) && (CurrentXElement.Parent.Elements().First() == CurrentXElement); }
            }
            /// <summary>
            /// Проверить, является ли узел первым на своем уровне, среди узлов с таким же именем
            /// </summary>
            protected bool IsFirstWithName
            {
                get { return (CurrentXElement.Parent != null) && (CurrentXElement.Parent.Elements().First(el => el.Name.LocalName == CurrentXElement.Name.LocalName) == CurrentXElement); }
            }
            /// <summary>
            /// Проверить, является ли узел последним на своем уровне
            /// </summary>
            protected bool IsLast
            {
                get { return (CurrentXElement.Parent != null) && (CurrentXElement.Parent.Elements().Last() == CurrentXElement); }
            }
            /// <summary>
            /// Проверить, является ли узел последним на своем уровне, среди узлов с таким же именем
            /// </summary>
            protected bool IsLastWithName
            {
                get { return (CurrentXElement.Parent != null) && (CurrentXElement.Parent.Elements().Last(el => el.Name.LocalName == CurrentXElement.Name.LocalName) == CurrentXElement); }
            }
            /// <summary>
            /// Проверить, является ли узел единственным с таким именем на своем уровне
            /// </summary>
            protected bool IsSingle
            {
                get { return (CurrentXElement.Parent == null) || (CurrentXElement.Parent.Elements().Count(el => el.Name.LocalName == CurrentXElement.Name.LocalName) == 1); }
            }
            #endregion
        }

        static class SqlGenFactory
        {
            private static readonly Dictionary<string, SqlGen> cash;

            static SqlGenFactory()
            {
                cash = new Dictionary<string, SqlGen>
                {
                    {"default", new SqlGen_Default()}
                };
            }

            public static SqlGen Get(string element_name)
            {
                SqlGen sql_gen;

                cash.TryGetValue(element_name, out sql_gen);

                if (sql_gen == null)
                {
                    switch (element_name)
                    {
                        case "select": sql_gen = new SqlGen_Select(); break;
                        case "query": sql_gen = new SqlGen_Query(); break;
                        case "column": sql_gen = new SqlGen_Column(); break;
                        case "call": sql_gen = new SqlGen_Call(); break;
                        case "order": sql_gen = new SqlGen_Order(); break;
                        case "table": sql_gen = new SqlGen_Table(); break;
                        case "from": sql_gen = new SqlGen_From(); break;
                        case "with": sql_gen = new SqlGen_With(); break;
                        case "where": sql_gen = new SqlGen_Where(); break;
                        case "connect": sql_gen = new SqlGen_Connect(); break;
                        case "start": sql_gen = new SqlGen_Start(); break;
                        case "group": sql_gen = new SqlGen_Group(); break;
                        case "having": sql_gen = new SqlGen_Having(); break;
                        case "dimension": sql_gen = new SqlGen_Dimension(); break;
                        case "measures": sql_gen = new SqlGen_Measures(); break;
                        case "insert": sql_gen = new SqlGen_Insert(); break;

                        default: return cash["default"];
                    }
                    cash.Add(element_name, sql_gen);
                }
                return sql_gen;
            }
        }

        #region Other
        // Получает имя родительского узла на ancestor_level уровней вверх
        // Если родитель нужного уровня отсутствует - возвращает пустую строку
        private static string GetAncestorName(XElement xelement, int ancestor_level = 1)
        {
            XElement ancestor = xelement;
            for (int i = 0; i < ancestor_level; i++)
            {
                if (ancestor == null) return "";
                ancestor = ancestor.Parent;
            }

            return (ancestor != null)
                ? ancestor.Name.LocalName
                : "";
        }
        #endregion
    }
}
