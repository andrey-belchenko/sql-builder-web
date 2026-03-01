using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using sql.builder;
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {
        // Track mapping from report names to form names
        private static Dictionary<string, string> ReportToFormMap = new Dictionary<string, string>();

        private class FolderProcessResult
        {
            public string FolderCode { get; set; }
            public HashSet<string> ReportImports { get; set; } = new HashSet<string>();
            public HashSet<string> FormImports { get; set; } = new HashSet<string>();
            public HashSet<string> DirectReports { get; set; } = new HashSet<string>();
        }

        private static FolderProcessResult ProcessFoldersRecursive(VSXElement parent, ref int folderIdCounter, int indentLevel = 0)
        {
            var skip = new[] { "61880_9_v3_ryaz_gp" };
            var indent = new string(' ', indentLevel * 4);
            var allReportImports = new HashSet<string>();
            var allFormImports = new HashSet<string>();
            var directReports = new HashSet<string>(); // Reports that are DIRECT children of THIS folder only
            var folderItems = new List<string>(); // Items in order: folders and reports as they appear in source

            // Process items in the order they appear in the source
            foreach (var item in parent.GetElementsP())
            {
                if (item is VFolder folder)
                {
                    var folderId = folderIdCounter++;
                    var folderTitle = EscapeString(folder.P_Title ?? folder.P_SelfTitle ?? "");
                    var childResult = ProcessFoldersRecursive(folder, ref folderIdCounter, indentLevel + 1);

                    // Collect report imports from children (for import statements)
                    foreach (var import in childResult.ReportImports)
                    {
                        allReportImports.Add(import);
                    }

                    // Collect form imports from children
                    foreach (var formImport in childResult.FormImports)
                    {
                        allFormImports.Add(formImport);
                    }

                    // Build the child folder code
                    var childIndent = new string(' ', (indentLevel + 1) * 4);
                    var folderCode = $"{childIndent}new Folder({{";
                    folderCode += $"\n{childIndent}    title: '{folderTitle}',";
                    folderCode += $"\n{childIndent}    folderId: {folderId},";
                    folderCode += $"\n{childIndent}    items: [";

                    // Add child folders and reports in order (from childResult.FolderCode)
                    // Note: FolderCode already contains all items including direct reports in correct order
                    if (!string.IsNullOrWhiteSpace(childResult.FolderCode))
                    {
                        folderCode += $"\n{childResult.FolderCode}";
                        folderCode += $"\n{childIndent}    ";
                    }
                    folderCode += "],";
                    folderCode += $"\n{childIndent}}})";

                    // Add folder to items in order
                    folderItems.Add(folderCode);
                }
                else if (item is VUseReport useReport && !skip.Contains(useReport.P_Report))
                {
                    var reportNames = ProcessReport(useReport);
                    if (reportNames != null && reportNames.Count > 0)
                    {
                        foreach (var reportName in reportNames)
                        {
                            // Only add if not already added (prevent duplicates from source)
                            if (directReports.Add(reportName))
                            {
                                allReportImports.Add(reportName);

                                // Track form used by this report
                                if (ReportToFormMap.TryGetValue(reportName, out var formName))
                                {
                                    allFormImports.Add(formName);
                                }

                                // Add report to items in order (maintain source order)
                                folderItems.Add($"{indent}report_{reportName}");
                            }
                        }
                    }
                }
            }

            // Join items in order - maintain source order, no sorting
            var folderCodeResult = folderItems.Count > 0 ? string.Join(",\n", folderItems) : "";

            return new FolderProcessResult
            {
                FolderCode = folderCodeResult,
                ReportImports = allReportImports,
                FormImports = allFormImports,
                DirectReports = directReports
            };
        }

        private static string GenerateTypeScriptFile(VNavigator nav, FolderProcessResult result)
        {
            var navigatorId = ExtractNavigatorId(nav.P_IdName);
            var customerId = navigatorId;
            var appId = 10;
            var useReportBuilder = false;
            var title = "Отчеты. Навигатор";

            var sb = new StringBuilder();
            sb.AppendLine("import { Navigator } from '@/system/reports/types/Navigator';");
            sb.AppendLine("import { Folder } from '@/system/reports/types/Folder';");

            // Add report imports
            if (result.ReportImports.Count > 0)
            {
                var sortedImports = result.ReportImports.OrderBy(x => x).ToList();
                foreach (var reportName in sortedImports)
                {
                    sb.AppendLine($"import report_{reportName} from './reports/report_{reportName}';");
                }
            }

            sb.AppendLine();
            sb.AppendLine("export default async () =>");
            sb.AppendLine("    new Navigator({");
            sb.AppendLine($"        title: '{title}',");
            sb.AppendLine($"        navigatorId: {navigatorId},");
            sb.AppendLine($"        appId: {appId},");
            sb.AppendLine($"        customerId: {customerId},");
            sb.AppendLine($"        useReportBuilder: {useReportBuilder.ToString().ToLower()},");
            sb.AppendLine("        items: [");

            if (!string.IsNullOrWhiteSpace(result.FolderCode))
            {
                sb.AppendLine(result.FolderCode);
            }

            sb.AppendLine("        ],");
            sb.AppendLine("    });");

            return sb.ToString();
        }


        private static int ExtractNavigatorId(string idName)
        {
            // Extract number from "nav10" -> 10, "nav310" -> 310, etc.
            var match = Regex.Match(idName, @"\d+");
            if (match.Success && int.TryParse(match.Value, out int navId))
            {
                return navId;
            }
            return 0;
        }


        private static IEnumerable<VNavigator> GetVNavigators()
        {
            var navNames = new[] { "nav310", "nav10", "nav101" };
            return XmlReports.Environment.GetElements(TextConst.EName.Navigators).Cast<VNavigator>()
                .Where(it => navNames.Contains(it.P_IdName))
                .ToList();
        }
        private static string UpdateReportFileImports(string content)
        {
            // Update utils imports: '../../utils' -> '@/system/sql-builder'
            // Match both single and double quotes
            // Path: generated/nav_10/reports/report.ts -> generated/utils = @/system/sql-builder (3 levels up)
            content = Regex.Replace(content, @"from\s+['""]\.\.\/\.\.\/utils['""]", m =>
            {
                var quote = m.Value.Contains("'") ? "'" : "\"";
                return $"from {quote}@/system/sql-builder{quote}";
            });

            // Form imports stay '../forms/' - no change needed

            return content;
        }

        private static string UpdateFormFileImports(string content)
        {
            // Update utils imports: '../../utils' -> '@/system/sql-builder'
            // Match both single and double quotes
            // Path: generated/nav_10/forms/form.ts -> generated/utils = @/system/sql-builder (3 levels up)
            content = Regex.Replace(content, @"from\s+['""]\.\.\/\.\.\/utils['""]", m =>
            {
                var quote = m.Value.Contains("'") ? "'" : "\"";
                return $"from {quote}@/system/sql-builder{quote}";
            });

            return content;
        }

        private static void CopyReportToNavigatorFolder(string reportName, string navigatorFolderPath)
        {
            var sourceReportsPath = Path.Combine(BasePath, "reports");
            var sourceFilePath = Path.Combine(sourceReportsPath, $"report_{reportName}.ts");
            var destReportsPath = Path.Combine(navigatorFolderPath, "reports");
            var destFilePath = Path.Combine(destReportsPath, $"report_{reportName}.ts");

            if (!Directory.Exists(destReportsPath))
            {
                Directory.CreateDirectory(destReportsPath);
            }

            if (File.Exists(sourceFilePath))
            {
                var content = File.ReadAllText(sourceFilePath, Encoding.UTF8);
                content = UpdateReportFileImports(content);
                File.WriteAllText(destFilePath, content, Encoding.UTF8);
                Console.WriteLine($"Copied report: {destFilePath}");
            }
        }

        private static void CopyFormToNavigatorFolder(string formName, string navigatorFolderPath)
        {
            var sourceFormsPath = Path.Combine(BasePath, "forms");
            var sourceFilePath = Path.Combine(sourceFormsPath, $"form_{formName}.ts");
            var destFormsPath = Path.Combine(navigatorFolderPath, "forms");
            var destFilePath = Path.Combine(destFormsPath, $"form_{formName}.ts");

            if (!Directory.Exists(destFormsPath))
            {
                Directory.CreateDirectory(destFormsPath);
            }

            if (File.Exists(sourceFilePath))
            {
                var content = File.ReadAllText(sourceFilePath, Encoding.UTF8);
                content = UpdateFormFileImports(content);
                File.WriteAllText(destFilePath, content, Encoding.UTF8);
                Console.WriteLine($"Copied form: {destFilePath}");
            }
        }

        public static void BuildNavigators()
        {
            // Clear the report-to-form mapping at the start
            ReportToFormMap.Clear();

            var navs = GetVNavigators();

            var initialFolderId = 10000;
            var folderIdCounter = initialFolderId;
            foreach (var nav in navs)
            {
                var navigatorId = ExtractNavigatorId(nav.P_IdName);

                // Create navigator-specific folder directly under BasePath
                var navigatorFolderPath = Path.Combine(BasePath, $"nav_{navigatorId}");
                if (!Directory.Exists(navigatorFolderPath))
                {
                    Directory.CreateDirectory(navigatorFolderPath);
                }

                var result = ProcessFoldersRecursive(nav, ref folderIdCounter, indentLevel: 3);
                var tsContent = GenerateTypeScriptFile(nav, result);

                // Write navigator file to navigator-specific folder
                var fileName = $"nav_{navigatorId}.ts";
                var filePath = Path.Combine(navigatorFolderPath, fileName);
                File.WriteAllText(filePath, tsContent, Encoding.UTF8);
                Console.WriteLine($"Generated file: {filePath}");

                // Copy reports to navigator folder
                foreach (var reportName in result.ReportImports)
                {
                    CopyReportToNavigatorFolder(reportName, navigatorFolderPath);
                }

                // Copy forms to navigator folder
                foreach (var formName in result.FormImports)
                {
                    CopyFormToNavigatorFolder(formName, navigatorFolderPath);
                }
            }

            // Clean up root forms and reports folders after copying to navigator folders
            var rootFormsPath = Path.Combine(BasePath, "forms");
            var rootReportsPath = Path.Combine(BasePath, "reports");
            var oldNavigatorsPath = Path.Combine(BasePath, "navigators");

            if (Directory.Exists(rootFormsPath))
            {
                Directory.Delete(rootFormsPath, recursive: true);
                Console.WriteLine($"Deleted root forms folder: {rootFormsPath}");
            }

            if (Directory.Exists(rootReportsPath))
            {
                Directory.Delete(rootReportsPath, recursive: true);
                Console.WriteLine($"Deleted root reports folder: {rootReportsPath}");
            }

            // Clean up old navigators folder if it exists
            if (Directory.Exists(oldNavigatorsPath))
            {
                Directory.Delete(oldNavigatorsPath, recursive: true);
                Console.WriteLine($"Deleted old navigators folder: {oldNavigatorsPath}");
            }
        }

    }
}
