using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
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

            var sqlFileName = "2.sql";
            string procedureName = null;

            string sqlFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Sql",sqlFileName);
           
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
            var tableNames = DevSqlParserAntlr.GetSourceTables(plsqlText,sql, procedureName);

            
            Console.WriteLine("Extracted source tables:");
            foreach (var tableName in tableNames.OrderBy(t => t))
            {
                Console.WriteLine($"  - {tableName}");
            }
            Console.WriteLine($"Total: {tableNames.Count} tables");
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
            DevAnalyzer.AnalyzeReports( "asuse2.24557");
        }

    }
}
