using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using sql.builder;
using sql.builder.Clean.Extensions;
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public class FormGenerationState
    {
        public List<string> Items { get; set; } = new List<string>();
        public HashSet<string> EditorTypes { get; set; } = new HashSet<string>();
        public bool NeedsExecQueryByName { get; set; }
        public bool NeedsGetFirstValue { get; set; }
    }

    public static partial class TsBuilder
    {
        private static Dictionary<VForm, FormGenerationState> formStates = new Dictionary<VForm, FormGenerationState>();

        private static FormGenerationState GetOrCreateFormState(VForm form)
        {
            if (!formStates.ContainsKey(form))
            {
                formStates[form] = new FormGenerationState();
            }
            return formStates[form];
        }

        private static string ProcessForm(VForm form, CleanExpressReport rep)
        {
            var content = form.ContentElement();

            // UIFormC

            var formClearedName = ClearName(form.P_IdName);
            var fileName = $"form_{formClearedName}.ts";
            ProcessContentChildren(form, content, rep);

            //DebugSaveFormXML(form);
            GenerateFormTypeScript(form, formClearedName, fileName);

            return formClearedName;
        }

        private static void GenerateFormTypeScript(VForm form, string formClearedName, string fileName)
        {
            var formsPath = Path.Combine(BasePath, "forms");

            // Create directory if it doesn't exist
            if (!Directory.Exists(formsPath))
            {
                Directory.CreateDirectory(formsPath);
            }

            var filePath = Path.Combine(formsPath, fileName);

            var formState = formStates.ContainsKey(form) ? formStates[form] : new FormGenerationState();

            var sb = new StringBuilder();

            // Generate imports
            GenerateFormImports(sb, formState);

            sb.AppendLine();
            sb.AppendLine("export default new Form({");
            sb.AppendLine("    items: [");

            // Add items
            for (int i = 0; i < formState.Items.Count; i++)
            {
                sb.Append(formState.Items[i]);
                if (i < formState.Items.Count - 1)
                {
                    sb.AppendLine(",");
                }
                else
                {
                    sb.AppendLine();
                }
            }

            sb.AppendLine("    ],");
            sb.AppendLine("});");

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            Console.WriteLine($"Generated form TypeScript file: {filePath}");

            // Clean up form state after generation
            formStates.Remove(form);
        }

        private static void GenerateFormImports(StringBuilder sb, FormGenerationState formState)
        {
            // Form import
            sb.AppendLine("import { Form } from '@/system/reports/types/Form';");

            // Field imports (always needed if there are items)
            if (formState.Items.Count > 0)
            {
                sb.AppendLine("import { Field } from '@/system/reports/types/Field';");

                // Check if any items are FieldGroups (they contain "new FieldGroup")
                bool hasFieldGroup = formState.Items.Any(item => item.Contains("new FieldGroup"));
                if (hasFieldGroup)
                {
                    sb.AppendLine("import { FieldGroup } from '@/system/reports/types/FieldGroup';");
                }
            }

            // Editor imports
            foreach (var editorType in formState.EditorTypes.OrderBy(x => x))
            {
                switch (editorType)
                {
                    case "SelectEditor":
                        sb.AppendLine("import { SelectEditor } from '@/system/reports/types/editors/SelectEditor';");
                        break;
                    case "DateEditor":
                        sb.AppendLine("import { DateEditor } from '@/system/reports/types/editors/DateEditor';");
                        break;
                    case "TextEditor":
                        sb.AppendLine("import { TextEditor } from '@/system/reports/types/editors/TextEditor';");
                        break;
                    case "NumberEditor":
                        sb.AppendLine("import { NumberEditor } from '@/system/reports/types/editors/NumberEditor';");
                        break;
                    case "CheckEditor":
                        sb.AppendLine("import { CheckEditor } from '@/system/reports/types/editors/CheckEditor';");
                        break;
                }
            }

            // Utils imports
            if (formState.NeedsExecQueryByName)
            {
                sb.AppendLine("import { execQueryByName } from '../../utils';");
            }

            if (formState.NeedsGetFirstValue)
            {
                sb.AppendLine("import { getFirstValue } from '../../utils';");
            }
        }

        private static void ProcessContentChildren(VForm form, VSXElement parent, CleanExpressReport rep)
        {
            var formState = GetOrCreateFormState(form);

            foreach (var element in parent.GetElementsP())
            {
                if (element is VField field)
                {
                    ProcessField(form, field, rep);
                }
                else if (element is VFieldGroup fieldGroup)
                {
                    var groupCode = ProcessFieldGroup(form, fieldGroup, rep);
                    if (groupCode != null)
                    {
                        formState.Items.Add(groupCode);
                    }
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
