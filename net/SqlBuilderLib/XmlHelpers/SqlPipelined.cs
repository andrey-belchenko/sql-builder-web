using System;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.XmlHelpers
{
    internal static class SqlPipelined
    {
        internal static string Generate(string qname, bool with_temp_table = false)
        {
            var xquery = new XElement(XmlReports.Environment.GetPrecompiledQuery(qname));
            xquery.Attributes(TextConst.AName.Name).Remove();

            var xparams = xquery.Elements("params").Elements("param").ToArray();
            foreach (var xparam in xparams)
            {
                xparam.RemoveNodes();
                xparam.Add(new XElement("const", xparam.Attribute("name").Value));
            }

            xquery = Compiler.GetCompiledAndProcessedQuery(xquery);
            string procText = Compiler.GetQuerProcedureFromCompiledQuery(xquery);
            string selectText = Compiler.GetQuerySelectStatmentFromCompiledQuery(xquery);
            var selQuery = xquery.Elements("query").First(e => e.Attribute(TextConst.AName.Materialize) == null);
            string[] record_fields = selQuery.Element(TextConst.EName.Select).Elements().Select(c => string.Format("\t\t{0} {1}", c.Attribute(TextConst.AName.As).Value, OraType(c.Attribute(AName.type).Value, true))).ToArray();
            string[] pars = xparams.Select(p => string.Format("\t\t{0} {1}", p.Attribute(AName.name).Value, OraType(p.Attribute(AName.type).Value, false)));

            var columns = selQuery.Element(TextConst.EName.Select).Elements().Select(c =>
                string.Format("\t\t{0}", c.Attribute(TextConst.AName.As).Value)).ToArray();

            string qname_safe = qname.Replace("-", "_").Replace("(", "_").Replace(")", "_");

            var sb = new StringBuilder();

            sb.AppendLine("--- Скрипт сгенерирован автоматически с помощью Sql.Builder ---");
            if (with_temp_table) GenerateTempTable(qname_safe, record_fields, sb);
            GeneratePackageSpecification(qname_safe, xquery, pars, record_fields, sb, with_temp_table);
            GeneratePackageBody(qname_safe, selectText,procText, pars, columns, sb, with_temp_table);
            GenerateGrants(qname_safe, sb, with_temp_table);
            GenerateSynonyms(qname_safe, sb, with_temp_table);
            sb.AppendLine("--- Конец автоматически сгенерированного скрипта ---");

            return sb.ToString();
        }

        internal static void GenerateTempTable(string qname_safe, string[] record_fields, StringBuilder sb)
        {
            sb.AppendLine(string.Format("TRUNCATE TABLE sqlb_{0}_tbl;", qname_safe));
            sb.AppendLine(string.Format("DROP TABLE sqlb_{0}_tbl;", qname_safe));
            sb.AppendLine("/");
            sb.AppendLine(string.Format("CREATE GLOBAL TEMPORARY TABLE sqlb_{0}_tbl", qname_safe));
            sb.AppendLine("(");
            sb.AppendLine(string.Join(",\r\n", record_fields));
            sb.AppendLine(") ON COMMIT PRESERVE ROWS;");
            sb.AppendLine("/");
            sb.AppendLine(string.Format("GRANT SELECT ON sqlb_{0}_tbl TO public;", qname_safe));
            sb.AppendLine("/");
            sb.AppendLine(string.Format("CREATE OR REPLACE PUBLIC SYNONYM sqlb_{0}_tbl FOR sqlb_{0}_tbl;", qname_safe));
            sb.AppendLine("/");
            sb.AppendLine();
        }
        private static void GeneratePackageSpecification(string qname_safe, XElement xquery, string[] pars, string[] record_fields, StringBuilder sb, bool with_temp_table)
        {
            sb.AppendLine(string.Format("CREATE OR REPLACE PACKAGE sqlb_{0}", qname_safe));
            sb.AppendLine("IS");
            sb.AppendLine("\tTYPE rec IS RECORD");
            sb.AppendLine("\t(");
            sb.AppendLine(string.Join(",\r\n", record_fields));
            sb.AppendLine("\t);");
            sb.AppendLine("\tTYPE tbl IS TABLE OF rec;");

            if (pars.Length != 0)
            {
                sb.AppendLine("\tFUNCTION get");
                sb.AppendLine("\t(");
                sb.AppendLine(string.Join(",\r\n", pars));
                sb.AppendLine("\t) RETURN tbl PIPELINED;");
            }
            else
            {
                sb.AppendLine("\tFUNCTION get RETURN tbl PIPELINED;");
            }

            if (with_temp_table)
            {
                sb.AppendLine();
                if (pars.Length != 0)
                {
                    sb.AppendLine("\tPROCEDURE fill_table");
                    sb.AppendLine("\t(");
                    sb.AppendLine(string.Join(",\r\n", pars));
                    sb.AppendLine("\t);");   
                }
                else
                {
                    sb.AppendLine("\tPROCEDURE fill_table;");
                }
            }

            sb.AppendLine(string.Format("END sqlb_{0};", qname_safe));
            sb.AppendLine("/");
            sb.AppendLine();
        }
        private static void GeneratePackageBody(string qname_safe, string selectText,string procText, string[] pars, string[] columns, StringBuilder sb, bool with_temp_table)
        {
            sb.AppendLine(string.Format("CREATE OR REPLACE PACKAGE BODY sqlb_{0}", qname_safe));
            sb.AppendLine("IS");

            if (pars.Length != 0)
            {
                sb.AppendLine("\tFUNCTION get");
                sb.AppendLine("\t(");
                sb.AppendLine(string.Join(",\r\n", pars));
                sb.AppendLine("\t) RETURN tbl PIPELINED");
            }
            else
            {
                sb.AppendLine("\tFUNCTION get RETURN tbl PIPELINED");
            }

            sb.AppendLine("\tIS");
            sb.AppendLine("\tBEGIN");

            if (procText != null)
            {
                sb.AppendLine(string.Format("\t\t{0}", procText));
            }

            sb.AppendLine("\t\tFOR curr IN");
            sb.AppendLine("\t\t(");
            sb.AppendLine(string.Format("\t\t{0}", selectText));
            sb.AppendLine("\t\t)");
            sb.AppendLine("\t\tLOOP");
            sb.AppendLine("\t\t\tPIPE ROW(curr);");
            sb.AppendLine("\t\tEND LOOP;");
            sb.AppendLine("\t\treturn;");
            sb.AppendLine("\tEND get;");

            if (with_temp_table)
            {
                sb.AppendLine();
                if (pars.Length != 0)
                {
                    sb.AppendLine("\tPROCEDURE fill_table");
                    sb.AppendLine("\t(");
                    sb.AppendLine(string.Join(",\r\n", pars));
                    sb.AppendLine("\t)");
                }
                else
                {
                    sb.AppendLine("\tPROCEDURE fill_table");
                }
                sb.AppendLine("\tIS");
                //sb.AppendLine("\t\tserr varchar2(200);");
                //sb.AppendLine("\t\tnerr number;");
                sb.AppendLine("\tBEGIN");


                if (procText != null)
                {
                    sb.AppendLine(string.Format("\t\t{0}", procText));
                }


                sb.AppendLine(string.Format("\t\tDELETE FROM sqlb_{0}_tbl;", qname_safe));
                sb.AppendLine(string.Format("\t\tINSERT INTO sqlb_{0}_tbl({1})", qname_safe, string.Join(",", columns)));
                sb.AppendLine(string.Format("\t\t{0};", selectText));
                //sb.AppendLine();
                //sb.AppendLine("\t\tEXCEPTION");
                //sb.AppendLine("\t\t\tWHEN OTHERS THEN");
                //sb.AppendLine("\t\t\t\tnerr := SQLCODE;");
                //sb.AppendLine("\t\t\t\tserr := SUBSTR (SQLERRM, 1, 200);");
                sb.AppendLine("\tEND fill_table;");
            }

            sb.AppendLine(string.Format("END sqlb_{0};", qname_safe));
            sb.AppendLine("/");
            sb.AppendLine();
        }
        private static void GenerateGrants(string qname_safe, StringBuilder sb, bool with_temp_table)
        {
            sb.AppendLine(string.Format("GRANT EXECUTE ON sqlb_{0} TO public;", qname_safe));
            if (with_temp_table)
            {
                sb.AppendLine("/");
                sb.AppendLine(string.Format("GRANT SELECT ON sqlb_{0}_tbl TO public;", qname_safe));
            }
            sb.AppendLine("/");
            sb.AppendLine();
        }
        private static void GenerateSynonyms(string qname_safe, StringBuilder sb, bool with_temp_table)
        {
            sb.AppendLine(string.Format("CREATE OR REPLACE PUBLIC SYNONYM sqlb_{0} FOR sqlb_{0};", qname_safe));
            
            sb.AppendLine("/");
            sb.AppendLine();
        }
        private static string OraType(string type, bool with_precision)
        {
            switch (type)
            {
                case "string": return (with_precision) ? "VARCHAR2(4000)" : "VARCHAR2";
                case "number": return "NUMBER";
                case "date": return "DATE";
                default: throw new ArgumentException(string.Format("Что за подозрительный тип {0}?", type));
            }
        }
    }
}
