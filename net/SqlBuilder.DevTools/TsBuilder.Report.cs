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
        private static List<string> ProcessReport(VUseReport useReport)
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

            var generatedReportNames = new List<string>();

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
                var reportFileName = GenerateFileReportTypeScript(reportClearedName, formClearedName, reportTitle, repFullName, template.P_Name, template.P_Title, suffix);
                generatedReportNames.Add(reportFileName);
                index++;
            }

            if (report.P_NoGrid != "1")
            {
                GenerateTableReportTypeScript(reportClearedName, formClearedName, reportTitle, repFullName);
                // Table report uses the base name without suffix
                generatedReportNames.Add(reportClearedName);
            }

            // Track form name for each generated report
            foreach (var reportName in generatedReportNames)
            {
                if (!ReportToFormMap.ContainsKey(reportName))
                {
                    ReportToFormMap[reportName] = formClearedName;
                }
            }

            return generatedReportNames;
        }

        private static string GenerateFileReportTypeScript(
            string reportClearedName, 
            string formClearedName, 
            string reportTitle, 
            string repFullName,
            string templateName, 
            string templateTitle,
            string tsFileSuffix
            )
        {
            var reportsPath = Path.Combine(BasePath, "reports");

            // Create directory if it doesn't exist
            if (!Directory.Exists(reportsPath))
            {
                Directory.CreateDirectory(reportsPath);
            }

            var fileName = $"report_{reportClearedName}{tsFileSuffix}.ts";
            var filePath = Path.Combine(reportsPath, fileName);

            // Generate file name from template title (sanitized)
            var fileDisplayName = string.IsNullOrEmpty(templateTitle) ? "Report.xlsx" : $"{templateTitle}.xlsx";
            var escapedFileDisplayName = EscapeString(fileDisplayName);

            // Update report title if suffix is present
            var finalReportTitle = reportTitle;
            if (!string.IsNullOrEmpty(tsFileSuffix))
            {
                finalReportTitle = reportTitle + " (" + EscapeString(templateTitle) + ")";
            }

            var sb = new StringBuilder();
            sb.AppendLine("import { RegularReport } from '@/system/reports/types/reports/RegularReport';");
            sb.AppendLine();
            sb.AppendLine($"import form_{formClearedName} from '../forms/form_{formClearedName}';");
            sb.AppendLine("import { FileViewer } from '@/system/reports/types/views/FileViewer';");
            sb.AppendLine("import { executeSqlbExcelReport } from '../../utils';");
            sb.AppendLine("import { buildFileId, downloadFile, saveFile } from '@/system/reports/utils/file';");
            sb.AppendLine("import { postprocessExcel } from '@/system/sql-builder/excel-post-process';");
            sb.AppendLine();
            sb.AppendLine("export default new RegularReport({");
            sb.AppendLine("    definedIn: __filename,");
            sb.AppendLine($"    title: '{finalReportTitle}',");
            sb.AppendLine($"    paramsForm: form_{formClearedName},");
            sb.AppendLine("    attrs: {");
            sb.AppendLine($"        name: '{EscapeString(repFullName)}',");
            sb.AppendLine($"        template: '{EscapeString(templateName)}',");
            sb.AppendLine("    },");
            sb.AppendLine("    view: async context => {");
            sb.AppendLine($"        const fileName = '{escapedFileDisplayName}';");
            sb.AppendLine("        const fileId = buildFileId(context, fileName);");
            sb.AppendLine("        await executeSqlbExcelReport({");
            sb.AppendLine($"            reportName: '{EscapeString(repFullName)}',");
            sb.AppendLine($"            templateName: '{EscapeString(templateName)}',");
            sb.AppendLine("            fileName,");
            sb.AppendLine("            parameters: context.formValues,");
            sb.AppendLine("            fileId,");
            sb.AppendLine("        });");
            sb.AppendLine();
            sb.AppendLine("        const file = await downloadFile(fileId);");
            sb.AppendLine();
            sb.AppendLine("        const fileInfo = await saveFile({");
            sb.AppendLine("            fileName,");
            sb.AppendLine("            fileData: await postprocessExcel(file?.fileData),");
            sb.AppendLine("            context,");
            sb.AppendLine("        });");
            sb.AppendLine();
            sb.AppendLine("        return new FileViewer(fileInfo);");
            sb.AppendLine("    },");
            sb.AppendLine("});");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            Console.WriteLine($"Generated report TypeScript file: {filePath}");

            // Return the base name without "report_" prefix for import purposes
            return $"{reportClearedName}{tsFileSuffix}";
        }

        private static void GenerateTableReportTypeScript(string reportClearedName, string formClearedName, string reportTitle, string repFullName)
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
            sb.AppendLine("import { executeSqlbReport, prepareTableSettings } from '@/system/sql-builder';");
            sb.AppendLine("import { buildDataSetId } from '@/system/reports/utils/mongo';");
            sb.AppendLine("import { ReportTable } from '@/system/reports/types/views/ReportTable';");
            sb.AppendLine();
            sb.AppendLine("export default new RegularReport({");
            sb.AppendLine("    definedIn: __filename,");
            sb.AppendLine($"    title: '{reportTitle}',");
            sb.AppendLine($"    paramsForm: form_{formClearedName},");
            sb.AppendLine("    attrs: {");
            sb.AppendLine($"        name: '{EscapeString(repFullName)}',");
            sb.AppendLine("    },");
            sb.AppendLine("    view: async context => {");
            sb.AppendLine("        const dataSetId = buildDataSetId(context);");
            sb.AppendLine("        await executeSqlbReport({");
            sb.AppendLine($"            reportName: '{EscapeString(repFullName)}',");
            sb.AppendLine("            dataSetId,");
            sb.AppendLine("            parameters: context.formValues,");
            sb.AppendLine("        });");
            sb.AppendLine();
            sb.AppendLine("        const tableSettings = await prepareTableSettings(dataSetId);");
            sb.AppendLine();
            sb.AppendLine("        return new ReportTable({");
            sb.AppendLine("            ...tableSettings,");
            sb.AppendLine("        });");
            sb.AppendLine("    },");
            sb.AppendLine("});");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            Console.WriteLine($"Generated report TypeScript file: {filePath}");
        }


    }
}
