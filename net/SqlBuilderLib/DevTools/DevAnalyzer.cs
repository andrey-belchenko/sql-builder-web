using System;
using System.Collections.Generic;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Devart.Data.Oracle;
using sql.builder;
using sql.builder.Clean;
using sql.builder.Clean.Extensions;
using sql.builder.DataApi;
using sql.builder.UI;


namespace SqlBuilderLib.DevTools
{

    internal static class DevAnalyzer
    {
        public static bool Enabled = false;

        public static bool PrepareOnly = false;

        public static HashSet<string> TableNames = new HashSet<string>();
        public static HashSet<string> ProcNames = new HashSet<string>();


        public static AnalyzerReportInfo ReportInfo = null;


        public static IEnumerable<string> SkipReports = new[]{
            "ies_garant.64650_2", // не парсится процедура скорее всего в ней ошибки
            "asuse2.10653(45)-new", // казань тепло, sql не распарсился он некорректный
            "kazan_el.74988","kazan_el.74989" // казань какие то проблемы с формой (вроде UIList без списка)
            };


        public static void AnalyzeReports()
        {
            DevAnalyzer.Enabled = true;
            DevAnalyzer.PrepareOnly = true;
            DevAnalyzer.ClearTempFolder();
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            // var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";
            var conStr = "User Id=asuse;Password=kl0pik;Server=REALKAZN;Pooling=False;Sid=REALKAZN;Port=1521";
            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);


            var navs = XmlReports.Environment.GetElements(TextConst.EName.Navigators).Cast<VNavigator>()
            .Where(it =>
            //  it.P_IdName == "nav310"
               it.P_IdName == "nav10"
             )
            .ToList();

            // Count total reports first
            int totalReports = navs
                .SelectMany(nav => nav.GetDescedantsP(EName.usereport).Cast<VUseReport>().Where(it => it.P_Invisible != TextConst.AVBool.True))
                .Count();

            int currentReport = 0;

            foreach (var nav in navs)
            {
                var usereps = nav.GetDescedantsP(EName.usereport).Cast<VUseReport>()
                .Where(it => it.P_Invisible != TextConst.AVBool.True);
                foreach (var userep in usereps)
                {
                    currentReport++;
                    var path = "";
                    var folder = userep.Parent as VFolder;
                    while (folder != null)
                    {
                        path = folder.P_Title + "/" + path;
                        folder = folder.Parent as VFolder;
                    }
                    var fullName = $"{userep.P_Project}.{userep.P_Report}";

                    if (SkipReports.Contains(fullName)) continue;
                    var info = new AnalyzerReportInfo()
                    {
                        Name = fullName,
                        Title = userep.P_Title,
                        Path = path,
                        NavId = nav.P_IdName,
                        NavInfo = nav.P_Title ?? nav.P_Comment
                    };
                    Console.WriteLine($"Analyze report: {info.Name} ({currentReport} of {totalReports})");
                    var isNew = DevAnalyzer.AnalyzeRep(info);
                    if (isNew)
                    {
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
                    else
                    {
                        Console.WriteLine($"{info.Name} SKIPPED");
                    }

                }
            }

            Console.WriteLine("done");

        }


        public static bool AnalyzeRep(AnalyzerReportInfo repInfo)
        {

            if (AnalyzerStorage.IsReportExists(repInfo))
            {
                return false;
            }
            SetReport(repInfo);
            var rep = new CleanExpressReport();
            rep.OpenDocumentAfterPrint = false;
            rep.Initialize(repInfo.Name);


            foreach (var p in rep.GetParamFields())
            {
                object value = null;

                switch (p.Control.GetType().Name)
                {
                    case nameof(UIText):
                        value = "dummy";
                        break;
                    case nameof(UINumber):
                        value = 0m;
                        break;
                    case nameof(UIDate):
                    case nameof(UIDateTime):
                        value = DateTime.Now;
                        break;
                    case nameof(UIDateRange):
                        value = new object[] { DateTime.Now, DateTime.Now };
                        break;
                    case nameof(UICombo):
                        value = 0m;
                        break;
                    case nameof(UICheck):
                        value = 0m;
                        break;
                    case nameof(UIList):
                        // For UIList, use empty list or check ValueType
                        Type valueType = p.GetValueType();
                        if (p.IsArray())
                        {
                            value = new List<object>();
                        }
                        else
                        {
                            // Use default value based on ValueType
                            if (valueType == typeof(string))
                            {
                                value = "dummy";
                            }
                            else if (valueType == typeof(decimal))
                            {
                                value = 0m;
                            }
                            else if (valueType == typeof(DateTime))
                            {
                                value = DateTime.Now;
                            }
                            else
                            {
                                value = Activator.CreateInstance(valueType);
                            }
                        }
                        break;
                    default:
                        // Fallback: use ValueType to determine dummy value
                        Type type = p.GetValueType();
                        if (type == typeof(string))
                        {
                            value = "dummy";
                        }
                        else if (type == typeof(decimal))
                        {
                            value = 0m;
                        }
                        else if (type == typeof(DateTime))
                        {
                            value = DateTime.Now;
                        }
                        else if (type==null)
                        {
                            value = DBNull.Value;
                        }
                        else if (type.IsValueType)
                        {
                            value = Activator.CreateInstance(type);
                        }
                        break;
                }
                p.SetValue(value);
            }

            rep.ExecuteReport();

            SaveReportAnalysisResults();
            return true;
        }

        public static void SaveReportAnalysisResults()
        {
            AnalyzerStorage.SaveReports(new[] { ReportInfo });
            AnalyzerStorage.SaveDependencies(GetReportDependencyRecords());
        }

        public static IEnumerable<AnalyzerDependency> GetReportDependencyRecords()
        {
            var list = new List<AnalyzerDependency>();
            foreach (var tbl in TableNames)
            {
                list.Add(new AnalyzerDependency()
                {
                    ObjectName = ReportInfo.Name,
                    ObjectType = "report",
                    UsedObjectName = tbl,
                    UsedObjectType = "table or view"
                });
            }

            foreach (var proc in ProcNames)
            {
                list.Add(new AnalyzerDependency()
                {
                    ObjectName = ReportInfo.Name,
                    ObjectType = "report",
                    UsedObjectName = proc,
                    UsedObjectType = "procedure"
                });
            }
            return list;
        }

        public static void SetReport(AnalyzerReportInfo repInfo)
        {
            ReportInfo = repInfo;
            TableNames = new HashSet<string>();
            ProcNames = new HashSet<string>();
        }
        public static void AnalyzeExecSql(string sql)
        {

        }

        public static void AnalyzeCmdSql(string sql)
        {
            if (!Enabled) return;
            var tableNames = DevSqlParserAntlr.GetSourceTables(Cmn.ClearUndefined(sql));
            TableNames.UnionWith(tableNames);

            if (tableNames.Overlaps(new[] { "adr_m", "k_house", "kr_calc" }))
            {

            }

            var procNames = DevSqlParserAntlr.GetSourceProcedures(Cmn.ClearUndefined(sql));
            ProcNames.UnionWith(procNames);
            // LogSql(sql);
        }

        private static void LogSql(string sql)
        {

            if (string.IsNullOrEmpty(sql)) return;

            // Get project root directory (where SqlBuilder.slnx is located)
            string projectRoot = GetProjectRoot();
            if (string.IsNullOrEmpty(projectRoot)) return;

            // Ensure Temp folder exists
            string tempFolder = Path.Combine(projectRoot, "Temp");
            Directory.CreateDirectory(tempFolder);

            // Generate filename with current fileIndex
            string fileName = $"{fileIndex}.sql";
            string filePath = Path.Combine(tempFolder, fileName);

            // Write SQL to file
            File.WriteAllText(filePath, sql, Encoding.UTF8);

            // Increment fileIndex for next call
            fileIndex++;
        }


        public static void AnalyzeReport(XElement xelement, string name = null)
        {
            if (!Enabled) return;
            // var xtables = xelement.Descendants(TextConst.EName.Table);
            // foreach (var xtable in xtables)
            // {
            //     var xtext = xelement.Element(TextConst.EName.Text);
            //     if (xtext == null)
            //     {
            //         TableNames.Add(xtable.GetAttributeValue(TextConst.AName.Name));
            //     }
            // }
            LogXElement(xelement, name);
        }
        public static void LogXElement(XElement element, string name = null)
        {
            if (!Enabled) return;
            if (element == null) return;

            // If name is not provided, try to find it from the element's name attribute
            if (string.IsNullOrEmpty(name))
            {
                var nameAttr = element.Attribute("name");
                if (nameAttr == null)
                {
                    // Search in descendants for a name attribute
                    var elementWithName = element.DescendantsAndSelf()
                        .FirstOrDefault(e => e.Attribute("name") != null);
                    nameAttr = elementWithName?.Attribute("name");
                }

                if (nameAttr != null)
                {
                    name = nameAttr.Value;
                }
                else
                {
                    // Fallback to element name if no name attribute found
                    name = element.Name.LocalName;
                }
            }

            // Get project root directory (where SqlBuilder.slnx is located)
            string projectRoot = GetProjectRoot();
            if (string.IsNullOrEmpty(projectRoot)) return;

            // Ensure Temp folder exists
            string tempFolder = Path.Combine(projectRoot, "Temp");
            Directory.CreateDirectory(tempFolder);

            // Generate filename with index if file already exists
            string baseFileName = $"{name}-par-val.xml";
            string filePath = Path.Combine(tempFolder, baseFileName);

            // If file exists, add index to filename
            if (File.Exists(filePath))
            {
                int index = 1;
                string fileNameWithoutExt = Path.GetFileNameWithoutExtension(baseFileName);
                string extension = Path.GetExtension(baseFileName);

                do
                {
                    string indexedFileName = $"{fileNameWithoutExt}-{index}{extension}";
                    filePath = Path.Combine(tempFolder, indexedFileName);
                    index++;
                } while (File.Exists(filePath));
            }

            element.Save(filePath);
        }

        private static int fileIndex = 1;
        public static void ClearTempFolder()
        {
            fileIndex = 1;
            // Get project root directory (where SqlBuilder.slnx is located)
            string projectRoot = GetProjectRoot();
            if (string.IsNullOrEmpty(projectRoot)) return;

            // Get Temp folder path
            string tempFolder = Path.Combine(projectRoot, "Temp");

            // Check if Temp folder exists
            if (!Directory.Exists(tempFolder)) return;

            try
            {
                // Delete all files in the Temp folder
                string[] files = Directory.GetFiles(tempFolder);
                foreach (string file in files)
                {
                    File.Delete(file);
                }

                // Optionally delete all subdirectories
                string[] directories = Directory.GetDirectories(tempFolder);
                foreach (string directory in directories)
                {
                    Directory.Delete(directory, true);
                }
            }
            catch (Exception ex)
            {
                // Log or handle the exception as needed
                // For now, silently fail or you could throw/rethrow
                throw new IOException($"Failed to clear Temp folder: {ex.Message}", ex);
            }
        }

        private static string GetProjectRoot()
        {
            try
            {
                // Start from the assembly location
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                if (string.IsNullOrEmpty(assemblyLocation))
                {
                    // Fallback to AppContext.BaseDirectory for .NET 8
                    assemblyLocation = AppContext.BaseDirectory;
                }

                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(assemblyLocation));

                // Navigate up the directory tree to find SqlBuilder.slnx
                while (dir != null)
                {
                    if (File.Exists(Path.Combine(dir.FullName, "SqlBuilder.slnx")))
                    {
                        return dir.FullName;
                    }
                    dir = dir.Parent;
                }
            }
            catch
            {
                // Return empty string if we can't determine the root
            }

            return string.Empty;
        }
    }
}
