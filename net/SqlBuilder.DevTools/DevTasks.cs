using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Npgsql;
using sql.builder;
using sql.builder.Clean;

namespace SqlBuilderLib.DevTools
{
    public static class DevTasks
    {
        public static void AnalyzeSql()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // var sqlFileName = ".sql";
            var sqlFileName = "ng_rep_other.sql";
            string procedureName = null;

            string sqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Sql", sqlFileName);

            if (!File.Exists(sqlFilePath))
            {
                Console.WriteLine($"Error: SQL file not found: {sqlFilePath}");
                return;
            }
            Console.WriteLine($"Reading SQL from: {sqlFilePath}");

            string plsqlText = File.ReadAllText(sqlFilePath, Encoding.UTF8);

            string sql = plsqlText;

            // plsqlText  =    Regex.Replace(plsqlText, @"\bas\s+end(?![a-zA-Z0-9_])", "as \"end\"", RegexOptions.IgnoreCase);
            plsqlText = plsqlText.Replace("stragg_dist", "max");
            Console.WriteLine($"Reading SQL from: {sqlFilePath}");
            Console.WriteLine();

            // Extract tables
            var result = DevSqlParserAntlr.GetSourceTables(plsqlText, sql, procedureName);

            Console.WriteLine("Extracted source tables:");
            foreach (var tableName in result.TableNames.OrderBy(t => t))
            {
                Console.WriteLine($"  - {tableName}");
                if (result.Details.TryGetValue(tableName, out var positions))
                {
                    foreach (var pos in positions)
                    {
                        Console.WriteLine($"      Line {pos.Line}, Col {pos.Column} (index {pos.StartIndex}-{pos.StopIndex})");
                    }
                }
            }
            Console.WriteLine($"Total: {result.TableNames.Count} tables");
            Console.WriteLine();

            // Extract procedures
            // var procedureNames = DevSqlParserAntlr.GetSourceProcedures(plsqlText, procedureName);
            // Console.WriteLine("Extracted source procedures:");
            // foreach (var procName in procedureNames.OrderBy(p => p))
            // {
            //     Console.WriteLine($"  - {procName}");
            // }
            // Console.WriteLine($"Total: {procedureNames.Count} procedures");
            // Console.WriteLine();
            // Console.WriteLine("done");
        }

        public static void TestTableRename()
        {
            Console.OutputEncoding = Encoding.UTF8;

            // var sqlFileName = "2.sql";
            var sqlFileName = "ng_rep_other.sql";
            //   var sqlFileName = "nv_account.sql";
            // var sqlFileName = "nv_account_sost_nal.sql";
            string procedureName = null;

            string sqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Sql", sqlFileName);

            if (!File.Exists(sqlFilePath))
            {
                Console.WriteLine($"Error: SQL file not found: {sqlFilePath}");
                return;
            }
            Console.WriteLine($"Reading SQL from: {sqlFilePath}");
            Console.WriteLine();

            string plsqlText = File.ReadAllText(sqlFilePath, Encoding.UTF8);
            plsqlText = plsqlText.Replace("stragg_dist", "max");
            string sql = plsqlText;

            var result = DevSqlParserAntlr.GetSourceTables(plsqlText, sql, procedureName);

            Console.WriteLine("Extracted source tables:");
            foreach (var tableName in result.TableNames.OrderBy(t => t))
            {
                Console.WriteLine($"  - {tableName}");
            }
            Console.WriteLine($"Total: {result.TableNames.Count} tables");
            Console.WriteLine();

            var renameDict = LoadRenameDictFromTableSchemas();

            Console.WriteLine("Rename mapping:");
            foreach (var kvp in renameDict)
            {
                Console.WriteLine($"  {kvp.Key} -> {kvp.Value}");
            }
            Console.WriteLine();

            string renamedSql = DevTableRenamer.RenameTables(plsqlText, result.Details, renameDict);

            string outputFileName = Path.GetFileNameWithoutExtension(sqlFileName) + "-processed" + Path.GetExtension(sqlFileName);
            string outputFilePath = Path.Combine(Path.GetDirectoryName(sqlFilePath), outputFileName);
            File.WriteAllText(outputFilePath, renamedSql, new UTF8Encoding(false));
            Console.WriteLine($"Saved to: {outputFilePath}");

            Console.WriteLine("--- Original SQL (first 500 chars) ---");
            Console.WriteLine(plsqlText.Length > 500 ? plsqlText.Substring(0, 500) + "..." : plsqlText);
            Console.WriteLine();
            Console.WriteLine("--- Renamed SQL (first 500 chars) ---");
            Console.WriteLine(renamedSql.Length > 500 ? renamedSql.Substring(0, 500) + "..." : renamedSql);
            Console.WriteLine();
            Console.WriteLine("done");
        }

        public static void AnalyzeReportDraft()
        {
            DevUtilsProvider.Instance = new DevUtilsProviderImpl();
            DevAnalyzer.Enabled = true;
            DevAnalyzer.PrepareOnly = true;
            DevAnalyzer.DoSave = false;
            DevAnalyzer.ClearTempFolder();
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            XmlReports.SetGlobalParValue("dep", 3580m);
            var pars = new Dictionary<string, object>();


            pars.Add("p_dep", 3580m);
            pars.Add("p_ym_beg", 2025.06m);

            var path = CleanSqlBuilder.ExecReportGetPath("ryazan.76607", pars, "76607.xlsx");

            Console.WriteLine("Extracted source tables:");
            foreach (var tableName in DevAnalyzer.TableNames.OrderBy(t => t))
            {
                Console.WriteLine($"  - {tableName}");
            }
            Console.WriteLine($"Total: {DevAnalyzer.TableNames.Count} tables");
            Console.WriteLine();


            Console.WriteLine("Extracted source procedures:");
            foreach (var procName in DevAnalyzer.ProcNames.OrderBy(p => p))
            {
                Console.WriteLine($"  - {procName}");
            }
            Console.WriteLine($"Total: {DevAnalyzer.ProcNames.Count} procedures");
            Console.WriteLine();
            Console.WriteLine("done");

        }

        public static void AnalyzeReports()
        {
            DevAnalyzer.AnalyzeReports();

        }

        public static void AnalyzeReport()
        {
            DevAnalyzer.AnalyzeReports("asuse2.24557");
        }

        private static Dictionary<string, string> LoadRenameDictFromTableSchemas()
        {
            var schemaPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Data", "table-schemas.xml");
            if (!File.Exists(schemaPath))
            {
                Console.WriteLine($"Warning: table-schemas.xml not found at {schemaPath}, using empty rename dict");
                return new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            }
            var doc = XDocument.Load(schemaPath);
            var dict = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (var record in doc.Root.Elements("DATA_RECORD"))
            {
                var tableName = record.Element("table_name")?.Value;
                var fullName = record.Element("full_name")?.Value;
                if (!string.IsNullOrEmpty(tableName) && !string.IsNullOrEmpty(fullName))
                {
                    dict[tableName] = fullName;
                }
            }

            dict["nv_account_sost_nal"] = "report_dev.nv_account_sost_nal";
            dict["nv_account"] = "report_dev.nv_account";
            return dict;
        }

    }
}
