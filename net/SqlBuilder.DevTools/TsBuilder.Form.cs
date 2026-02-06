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
using sql.builder.UI;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {
        private static string ProcessForm(VForm form)
        {
            var content = form.ContentElement();

            // UIFormC
            ProcessContentChildren(form, content);

            DebugSaveFormXML(form);
            return null;

        }

        private static void ProcessContentChildren(VForm form, VSXElement parent)
        {
            foreach (var element in parent.GetElementsP())
            {
                if (element is VField field)
                {
                    ProcessField(form, field);
                }
                else if (element is VFieldGroup fieldGroup)
                {
                    ProcessFieldGroup(form, fieldGroup);
                    ProcessContentChildren(form, fieldGroup); // Recursive call
                }
            }
        }


        private static void DebugSaveFormXML(VForm form)
        {
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
                Console.WriteLine($"Form '{formName}' already exists with same content.");
            }

            // File doesn't exist, save it
            File.WriteAllText(filePath, newContent, Encoding.UTF8);
            Console.WriteLine($"Saved form '{formName}' to '{filePath}'");
        }

    }
}
