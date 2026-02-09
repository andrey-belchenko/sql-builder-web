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
using sql.builder.Clean.Extensions;
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {
        private static string ProcessReport(VUseReport useReport)
        {
            if (useReport.P_Visible == "0" || useReport.P_Report.Contains("journal"))
            {
                return null;
            }
            var repFullName = $"{useReport.P_Project}.{useReport.P_Report}";
            var form = VSXElement.Get(CleanSqlBuilder.GetFormConfig(repFullName)) as VForm;

            var rep = new CleanExpressReport();
            rep.OpenDocumentAfterPrint = false;
            rep.Initialize(repFullName);

            var fields = rep.GetParamFields().ToArray();

            foreach (var field in fields)
            {
                field.Control.PrepareListSource();
                field.Control.PrepareDefaultSource();
            }
            var formClearedName = ProcessForm(form, rep);

            if (string.IsNullOrEmpty(formClearedName))
            {
                return null;
            }

            var reportClearedName = ClearName(useReport.P_Report);
            var reportTitle = EscapeString(useReport.P_Title ?? useReport.P_SelfTitle ?? "");


            //GenerateTableReportTypeScript(reportClearedName, formClearedName, reportTitle);

            var report = rep._frm._report;
            var suffix = "";
            var templates = report.PrintTemplates().ToArray();
            var index = 1;
            foreach (var template in templates)
            {
                if (templates.Count() > 1 || report.P_NoGrid != "1")
                {
                    suffix = "_"+ index.ToString();
                }
                GenerateFileReportTypeScript(reportClearedName, formClearedName, reportTitle, template.P_Name, template.P_Title, suffix);
                index++;
            }

            if (report.P_NoGrid != "1")
            {
                GenerateTableReportTypeScript(reportClearedName, formClearedName, reportTitle);
            }
            return reportClearedName;
        }

        private static void GenerateFileReportTypeScript(
            string reportClearedName, 
            string formClearedName, 
            string reportTitle, 
            string templateName, 
            string templateTitle,
            string tsFileSuffix
            )
        {
            if (!string.IsNullOrEmpty(tsFileSuffix))
            {
                reportTitle = reportTitle + "("+ templateTitle+")";
            }
            
        }

        private static void GenerateTableReportTypeScript(string reportClearedName, string formClearedName, string reportTitle)
        {
            var reportsPath = Path.Combine(BasePath, "reports");

            // Create directory if it doesn't exist
            if (!Directory.Exists(reportsPath))
            {
                Directory.CreateDirectory(reportsPath);
            }

            var fileName = $"report_{reportClearedName}.ts";
            var filePath = Path.Combine(reportsPath, fileName);

            var sb = new StringBuilder();
            sb.AppendLine("import { RegularReport } from '@/system/reports/types/reports/RegularReport';");
            sb.AppendLine($"import form_{formClearedName} from '../forms/form_{formClearedName}';");
            sb.AppendLine();
            sb.AppendLine("export default new RegularReport({");
            sb.AppendLine("    definedIn: __filename,");
            sb.AppendLine($"    title: '{reportTitle}',");
            sb.AppendLine($"    paramsForm: form_{formClearedName},");
            sb.AppendLine("});");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            Console.WriteLine($"Generated report TypeScript file: {filePath}");
        }


    }
}
