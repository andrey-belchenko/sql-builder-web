using System;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading;
//using System.Windows.Forms;
using System.Xml.Linq;
using Devart.Data.Oracle;
//using DevExpress.LookAndFeel;
//using DevExpress.Skins;
//using DevExpress.UserSkins;
//using DevExpress.XtraEditors;
using infoenergo.core;
using infoenergo.core.Data;
using infoenergo.sys;
//using infoenergo.ui.win;
//using sql.builder.Controls.Testing;
//using sql.builder.Properties;
using sql.builder.DataApi;
using sql.builder.UI.CommandItems;
using sql.builder.WinForms;
using sql.builder.XmlHelpers;
//using infoenergo.framework.Extensions.Oracle;
using System.Collections.Generic;
using System.Text;
using sql.builder.Clean;
using SqlBuilderLib.DevTools;

// Basic usage


namespace sql.builder
{
    public static class Program
    {

        public static void TestSqlParsing(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            var sqlFileName = "17.sql";
            string procedureName = null;
            // string procedureName = "dog_obj";

            // Read SQL from file - try multiple possible paths
            string sqlFilePath = null;
            string[] possiblePaths = new[]
            {
                Path.Combine(Directory.GetCurrentDirectory(), "Sql", sqlFileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "..", "..", "..", "..", "Sql",sqlFileName),
                Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Sql", sqlFileName),
                Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "Sql", sqlFileName))
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    sqlFilePath = path;
                    break;
                }
            }

            if (sqlFilePath == null || !File.Exists(sqlFilePath))
            {
                Console.WriteLine("Error: SQL file not found. Tried paths:");
                foreach (var path in possiblePaths)
                {
                    Console.WriteLine($"  - {path}");
                }
                return;
            }

            string plsqlText = File.ReadAllText(sqlFilePath, Encoding.UTF8);
            Console.WriteLine($"Reading SQL from: {sqlFilePath}");
            Console.WriteLine();

            // Extract tables
            var tableNames = DevSqlParserAntlr.GetSourceTables(plsqlText, procedureName);
            Console.WriteLine("Extracted source tables:");
            foreach (var tableName in tableNames.OrderBy(t => t))
            {
                Console.WriteLine($"  - {tableName}");
            }
            Console.WriteLine($"Total: {tableNames.Count} tables");
            Console.WriteLine();

            // Extract procedures
            var procedureNames = DevSqlParserAntlr.GetSourceProcedures(plsqlText, procedureName);
            Console.WriteLine("Extracted source procedures:");
            foreach (var procName in procedureNames.OrderBy(p => p))
            {
                Console.WriteLine($"  - {procName}");
            }
            Console.WriteLine($"Total: {procedureNames.Count} procedures");
            Console.WriteLine();
            Console.WriteLine("done");
        }

        public static void TestReportAnalysis(string[] args)
        {
            DevAnalyzer.Enabled = true;
            DevAnalyzer.PrepareOnly = true;
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


        public static void TestReportsAnalysis(string[] args)
        {
            DevAnalyzer.Enabled = true;
            DevAnalyzer.PrepareOnly = true;
            DevAnalyzer.ClearTempFolder();
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";
            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);


            var navs = XmlReports.Environment.GetElements(TextConst.EName.Navigators).Cast<VNavigator>()
            .Where(it => it.P_IdName == "nav310")
            .ToList();

            foreach (var nav in navs)
            {
                var usereps = nav.GetDescedantsP(EName.usereport).Cast<VUseReport>();
                foreach (var userep in usereps)
                {
                    var path = "";
                    var folder = userep.Parent as VFolder;
                    while (folder != null)
                    {
                        path = path + "/" + folder.P_Title;
                        folder = folder.Parent as VFolder;
                    }
                    var fullName = $"{userep.P_Project}.{userep.P_Report}";
                    var info = new AnalyzerReportInfo()
                    {
                        Name = fullName,
                        Title = userep.P_Title,
                        Path = path,
                        NavId = nav.P_IdName,
                        NavInfo = nav.P_Title ?? nav.P_Comment
                    };
                    Console.WriteLine($"Analyze report: {info.Name}");
                    DevAnalyzer.AnalyzeRep(info);
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
                }

            }




            Console.WriteLine("done");

        }

        public static void Main(string[] args)
        {
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
            // Output as file URI for VS Code debug console to recognize as clickable link
            //var fileUri = new Uri(path).ToString();
            Console.WriteLine(path);
            Console.WriteLine("done");

        }

        public static void Main1(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=REALKAZN;Pooling=False;Sid=REALKAZN;Port=1521";
            //var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            var pars = new Dictionary<string, object>();


            pars.Add("p_date_s", new DateTime(2020, 1, 8));
            pars.Add("p_date_po", new DateTime(2025, 1, 8));
            pars.Add("p_kodp", new List<int> { 1172, 1210, 1211, 1212, 1214, 1215
                //, 1216, 1217, 1218, 1219
            });

            var path = CleanSqlBuilder.ExecReportGetPath("asuse2.65211", pars, "65211.xlsx");
            // Output as file URI for VS Code debug console to recognize as clickable link
            var fileUri = new Uri(path).ToString();
            Console.WriteLine(fileUri); // VS Code will make this clickable
            Console.WriteLine("done");

        }
    }
}
