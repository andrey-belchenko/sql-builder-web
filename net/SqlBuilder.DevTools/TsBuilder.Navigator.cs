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
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {
        private class FolderProcessResult
        {
            public string FolderCode { get; set; }
            public HashSet<string> ReportImports { get; set; } = new HashSet<string>();
            public List<string> DirectReports { get; set; } = new List<string>();
        }

        private static FolderProcessResult ProcessFoldersRecursive(VSXElement parent, ref int folderIdCounter, int indentLevel = 0)
        {
            var indent = new string(' ', indentLevel * 4);
            var items = new List<string>();
            var allReportImports = new HashSet<string>();
            var currentReports = new List<string>();

            foreach (var item in parent.GetElementsP())
            {
                if (item is VFolder folder)
                {
                    var folderId = folderIdCounter++;
                    var folderTitle = EscapeString(folder.P_Title ?? folder.P_SelfTitle ?? "");
                    var childResult = ProcessFoldersRecursive(folder, ref folderIdCounter, indentLevel + 1);

                    // Collect report imports from children
                    foreach (var import in childResult.ReportImports)
                    {
                        allReportImports.Add(import);
                    }

                    var folderCode = $"{indent}new Folder({{";
                    folderCode += $"\n{indent}    title: '{folderTitle}',";
                    folderCode += $"\n{indent}    folderId: {folderId},";
                    folderCode += $"\n{indent}    items: [";
                    
                    var folderItems = new List<string>();
                    
                    // Add child folders
                    if (!string.IsNullOrWhiteSpace(childResult.FolderCode))
                    {
                        folderItems.Add(childResult.FolderCode);
                    }

                    // Add direct reports from this folder
                    var childIndent = new string(' ', (indentLevel + 1) * 4);
                    foreach (var reportName in childResult.DirectReports)
                    {
                        folderItems.Add($"{childIndent}report_{reportName}");
                    }

                    if (folderItems.Count > 0)
                    {
                        folderCode += $"\n{string.Join(",\n", folderItems)}";
                        folderCode += $"\n{indent}    ";
                    }
                    folderCode += "],";
                    folderCode += $"\n{indent}}}),";

                    items.Add(folderCode);
                }
                else if (item is VUseReport useReport)
                {
                    var reportClearedName = ProcessReport(useReport);
                    if (!string.IsNullOrEmpty(reportClearedName))
                    {
                        currentReports.Add(reportClearedName);
                        allReportImports.Add(reportClearedName);
                    }
                }
            }

            // Add current level reports to items (these are reports at the root navigator level)
            foreach (var reportName in currentReports)
            {
                items.Add($"{indent}report_{reportName}");
            }

            return new FolderProcessResult
            {
                FolderCode = string.Join(",\n", items),
                ReportImports = allReportImports,
                DirectReports = currentReports
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
                    sb.AppendLine($"import report_{reportName} from '../reports/report_{reportName}';");
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
        public static void BuildNavigators()
        {
            var navs = GetVNavigators();
            var sqlBuilderPath = Path.Combine(BasePath, "navigators");

            // Create directory if it doesn't exist
            if (!Directory.Exists(sqlBuilderPath))
            {
                Directory.CreateDirectory(sqlBuilderPath);
            }

            var initialFolderId = 10000;
            var folderIdCounter = initialFolderId;
            foreach (var nav in navs)
            {
                var navigatorId = ExtractNavigatorId(nav.P_IdName);
                var fileName = $"nav_{navigatorId}.ts";
                var filePath = Path.Combine(sqlBuilderPath, fileName);

                var result = ProcessFoldersRecursive(nav, ref folderIdCounter, indentLevel: 3);
                var tsContent = GenerateTypeScriptFile(nav, result);

                File.WriteAllText(filePath, tsContent, Encoding.UTF8);
                Console.WriteLine($"Generated file: {filePath}");
            }
        }

    }
}
