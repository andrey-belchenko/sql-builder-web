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
            var form = CleanSqlBuilder.GetFormConfig(repFullName);
            var formName = form.GetAttributeValue(TextConst.AName.Name);

            var formsXmlPath = Path.Combine(BasePath, "forms-xml");

            // Create directory if it doesn't exist
            if (!Directory.Exists(formsXmlPath))
            {
                Directory.CreateDirectory(formsXmlPath);
            }

            var filePath = Path.Combine(formsXmlPath, $"{formName}.xml");
            var newContent = form.ToString();

            // Check if file already exists
            if (File.Exists(filePath))
            {
                var existingContent = File.ReadAllText(filePath, Encoding.UTF8);

                // Compare content (normalize whitespace for XML comparison)
                if (!string.Equals(existingContent.Trim(), newContent.Trim(), StringComparison.Ordinal))
                {
                    throw new InvalidOperationException(
                        $"Form file '{filePath}' already exists but content is different. " +
                        $"Cannot overwrite existing form configuration.");
                }

                // Content is the same, no need to write
                return $"Form '{formName}' already exists with same content.";
            }

            // File doesn't exist, save it
            File.WriteAllText(filePath, newContent, Encoding.UTF8);
            return $"Saved form '{formName}' to '{filePath}'";
        }


    }
}
