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
        private static string ProcessFoldersRecursive(VSXElement parent, ref int folderIdCounter, int indentLevel = 0)
        {
            var indent = new string(' ', indentLevel * 4);
            var items = new List<string>();

            foreach (var item in parent.GetElementsP())
            {
                if (item is VFolder folder)
                {
                    var folderId = folderIdCounter++;
                    var folderTitle = EscapeString(folder.P_Title ?? folder.P_SelfTitle ?? "");
                    var childItems = ProcessFoldersRecursive(folder, ref folderIdCounter, indentLevel + 1);

                    var folderCode = $"{indent}new Folder({{";
                    folderCode += $"\n{indent}    title: '{folderTitle}',";
                    folderCode += $"\n{indent}    folderId: {folderId},";
                    folderCode += $"\n{indent}    items: [";
                    if (!string.IsNullOrWhiteSpace(childItems))
                    {
                        folderCode += $"\n{childItems}";
                        folderCode += $"\n{indent}    ";
                    }
                    folderCode += "],";
                    folderCode += $"\n{indent}}}),";

                    items.Add(folderCode);
                }
                else if (item is VUseReport useReport)
                {
                    ProcessReport(useReport);
                }
                // Skip VUseReport items as requested
            }

            return string.Join("\n", items);
        }

        private static string GenerateTypeScriptFile(VNavigator nav, string foldersCode)
        {
            var navigatorId = ExtractNavigatorId(nav.P_IdName);
            var customerId = navigatorId;
            var appId = 10;
            var useReportBuilder = false;
            var title = "Отчеты. Навигатор";

            var sb = new StringBuilder();
            sb.AppendLine("import { Navigator } from '@/system/reports/types/Navigator';");
            sb.AppendLine("import { Folder } from '@/system/reports/types/Folder';");
            sb.AppendLine();
            sb.AppendLine("export default async () =>");
            sb.AppendLine("    new Navigator({");
            sb.AppendLine($"        title: '{title}',");
            sb.AppendLine($"        navigatorId: {navigatorId},");
            sb.AppendLine($"        appId: {appId},");
            sb.AppendLine($"        customerId: {customerId},");
            sb.AppendLine($"        useReportBuilder: {useReportBuilder.ToString().ToLower()},");
            sb.AppendLine("        items: [");

            if (!string.IsNullOrWhiteSpace(foldersCode))
            {
                sb.AppendLine(foldersCode);
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
            var sqlBuilderPath = Path.Combine(BasePath, "sql-builder", "navigators");

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

                var foldersCode = ProcessFoldersRecursive(nav, ref folderIdCounter, indentLevel: 3);
                var tsContent = GenerateTypeScriptFile(nav, foldersCode);

                File.WriteAllText(filePath, tsContent, Encoding.UTF8);
                Console.WriteLine($"Generated file: {filePath}");
            }
        }

    }
}
