
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using sql.builder;
using sql.builder.DataApi;
using sql.builder.UI;


namespace SqlBuilderLib.DevTools
{

    public static partial class TsBuilder
    {
        private static HashSet<string> processedFields = new HashSet<string>();

        private static string ProcessFieldGenTs(VForm form, VField field, IEnumerable<FieldProps> fieldsProps)
        {
            var fieldName = field.P_Field;
            if (string.IsNullOrEmpty(fieldName))
            {
                return null;
            }

            var clearedName = ClearName(fieldName);
            
            // Skip if field with same name already processed
            if (processedFields.Contains(clearedName))
            {
                return clearedName;
            }

            var fieldsList = fieldsProps.ToList();
            if (fieldsList.Count == 0)
            {
                return null;
            }

            var fieldFileName = $"field_{clearedName}.ts";
            var fieldsPath = Path.Combine(BasePath, "fields");

            // Create directory if it doesn't exist
            if (!Directory.Exists(fieldsPath))
            {
                Directory.CreateDirectory(fieldsPath);
            }

            var filePath = Path.Combine(fieldsPath, fieldFileName);

            var sb = new StringBuilder();
            
            // Generate imports
            GenerateFieldImports(sb, fieldsList);
            
            sb.AppendLine("//required");
            sb.AppendLine();

            // Generate field or field group code
            if (fieldsList.Count == 1)
            {
                GenerateSingleFieldCode(sb, fieldsList[0]);
            }
            else
            {
                GenerateFieldGroupCode(sb, fieldsList, field);
            }

            File.WriteAllText(filePath, sb.ToString(), Encoding.UTF8);
            Console.WriteLine($"Generated field TypeScript file: {filePath}");

            // Mark field as processed
            processedFields.Add(clearedName);

            return clearedName;
        }

        private static void GenerateFieldImports(StringBuilder sb, List<FieldProps> fieldsProps)
        {
            var editorTypes = new HashSet<string>();
            var needsExecQueryByName = false;
            var needsGetFirstValue = false;

            foreach (var props in fieldsProps)
            {
                if (props.editor != null)
                {
                    editorTypes.Add(props.editor.editorType);
                }

                // Check if any method uses queryName
                if (HasQueryMethod(props.defaultValue))
                {
                    needsExecQueryByName = true;
                }

                if (props.editor is SelectEditorProps selectEditor && HasQueryMethod(selectEditor.listItems))
                {
                    needsExecQueryByName = true;
                }

                if (props.defaultValue?.isSingleValue == true && props.defaultValue?.queryName != null)
                {
                    needsGetFirstValue = true;
                }
            }

            // Generate editor imports
            foreach (var editorType in editorTypes.OrderBy(x => x))
            {
                switch (editorType)
                {
                    case "SelectEditor":
                        sb.AppendLine("import { PartialSelectEditorProps, SelectEditor } from '@/system/reports/types/editors/SelectEditor';");
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

            // Field imports
            if (fieldsProps.Count == 1)
            {
                sb.AppendLine("import { Field, PartialFieldProps } from '@/system/reports/types/Field';");
            }
            else
            {
                sb.AppendLine("import { Field, PartialFieldProps } from '@/system/reports/types/Field';");
                sb.AppendLine("import { FieldGroup } from '@/system/reports/types/FieldGroup';");
            }

            // Utils imports
            if (needsExecQueryByName)
            {
                sb.AppendLine("import { execQueryByName } from './utils';");
            }

            if (needsGetFirstValue)
            {
                sb.AppendLine("import { getFirstValue } from './utils';");
            }

            sb.AppendLine();
        }

        private static bool HasQueryMethod(MethodInfo methodInfo)
        {
            return methodInfo != null && !string.IsNullOrEmpty(methodInfo.queryName);
        }

        private static void GenerateSingleFieldCode(StringBuilder sb, FieldProps props)
        {
            var editorType = props.editor?.editorType ?? "";
            var isSelectEditor = editorType == "SelectEditor";
            var partialEditorType = isSelectEditor ? "PartialSelectEditorProps" : "";

            if (isSelectEditor)
            {
                sb.AppendLine($"export default (overrideProps: PartialFieldProps = {{}}, overrideEditorProps: {partialEditorType} = {{}}) =>");
            }
            else
            {
                sb.AppendLine("export default (overrideProps: PartialFieldProps = {}) =>");
            }
            sb.AppendLine("    new Field({");
            sb.AppendLine("        ...{");

            // Label
            if (!string.IsNullOrEmpty(props.label))
            {
                sb.AppendLine($"            label: '{EscapeString(props.label)}',");
            }

            // Name
            sb.AppendLine($"            name: '{props.name}',");

            // Editor
            sb.Append("            editor: new ");
            sb.Append(editorType);
            sb.AppendLine("({");
            GenerateEditorCode(sb, props.editor, "                ", isSelectEditor);
            if (isSelectEditor)
            {
                sb.AppendLine("                ...overrideEditorProps,");
            }
            sb.AppendLine("            }),");

            // Other field properties
            GenerateFieldProperty(sb, "defaultValue", props.defaultValue, "            ");
            GenerateFieldPropertyArray(sb, "defaultValueDeps", props.defaultValueDeps, "            ");
            GenerateFieldProperty(sb, "required", props.required, "            ");
            GenerateFieldPropertyArray(sb, "requiredDeps", props.requiredDeps, "            ");
            GenerateFieldProperty(sb, "validation", props.validation, "            ");
            GenerateFieldPropertyArray(sb, "validationDeps", props.validationDeps, "            ");
            GenerateFieldProperty(sb, "exists", props.exists, "            ");
            GenerateFieldPropertyArray(sb, "existsDeps", props.existsDeps, "            ");
            GenerateFieldProperty(sb, "enabled", props.enabled, "            ");
            GenerateFieldPropertyArray(sb, "enabledDeps", props.enabledDeps, "            ");
            GenerateFieldProperty(sb, "visible", props.visible, "            ");
            GenerateFieldPropertyArray(sb, "visibleDeps", props.visibleDeps, "            ");

            sb.AppendLine("        },");
            sb.AppendLine("        ...overrideProps,");
            sb.AppendLine("    });");
        }

        private static void GenerateFieldGroupCode(StringBuilder sb, List<FieldProps> fieldsProps, VField field)
        {
            sb.AppendLine("export default (overrideProps: PartialFieldProps = {}) =>");
            sb.AppendLine("    new FieldGroup({");
            
            // Group label - use field title or generate from field name
            var groupLabel = field.P_Title ?? field.P_Name ?? "";
            if (string.IsNullOrEmpty(groupLabel))
            {
                groupLabel = fieldsProps.FirstOrDefault()?.label ?? "";
            }
            sb.AppendLine($"        label: '{EscapeString(groupLabel)}',");
            
            sb.AppendLine("        items: [");

            // Generate each field
            for (int i = 0; i < fieldsProps.Count; i++)
            {
                var props = fieldsProps[i];
                sb.AppendLine("            new Field({");
                sb.AppendLine("                ...{");

                // Label
                if (!string.IsNullOrEmpty(props.label))
                {
                    sb.AppendLine($"                    label: '{EscapeString(props.label)}',");
                }

                // Name
                sb.AppendLine($"                    name: '{props.name}',");

                // Editor
                sb.Append("                    editor: new ");
                sb.Append(props.editor?.editorType ?? "TextEditor");
                sb.AppendLine("({");
                GenerateEditorCode(sb, props.editor, "                        ", false);
                sb.AppendLine("                    }),");

                // Other field properties
                GenerateFieldProperty(sb, "defaultValue", props.defaultValue, "                    ");
                GenerateFieldPropertyArray(sb, "defaultValueDeps", props.defaultValueDeps, "                    ");
                GenerateFieldProperty(sb, "required", props.required, "                    ");
                GenerateFieldPropertyArray(sb, "requiredDeps", props.requiredDeps, "                    ");
                GenerateFieldProperty(sb, "validation", props.validation, "                    ");
                GenerateFieldPropertyArray(sb, "validationDeps", props.validationDeps, "                    ");
                GenerateFieldProperty(sb, "exists", props.exists, "                    ");
                GenerateFieldPropertyArray(sb, "existsDeps", props.existsDeps, "                    ");
                GenerateFieldProperty(sb, "enabled", props.enabled, "                    ");
                GenerateFieldPropertyArray(sb, "enabledDeps", props.enabledDeps, "                    ");
                GenerateFieldProperty(sb, "visible", props.visible, "                    ");
                GenerateFieldPropertyArray(sb, "visibleDeps", props.visibleDeps, "                    ");

                sb.AppendLine("                },");
                sb.AppendLine("                ...overrideProps,");
                sb.AppendLine("            })");

                if (i < fieldsProps.Count - 1)
                {
                    sb.AppendLine(",");
                }
            }

            sb.AppendLine("        ],");
            sb.AppendLine("    });");
        }

        private static void GenerateEditorCode(StringBuilder sb, EditorProps editor, string indent, bool includeOverride)
        {
            if (editor == null)
            {
                return;
            }

            if (editor is SelectEditorProps selectEditor)
            {
                GenerateSelectEditorCode(sb, selectEditor, indent);
            }
            // Other editor types (DateEditor, TextEditor, NumberEditor, CheckEditor) have empty props
            // So we don't need to generate anything for them
        }

        private static void GenerateSelectEditorCode(StringBuilder sb, SelectEditorProps editor, string indent)
        {
            // Columns
            if (editor.columns != null && editor.columns.Count > 0)
            {
                sb.Append(indent);
                sb.AppendLine("columns: [");
                foreach (var col in editor.columns)
                {
                    sb.Append(indent);
                    sb.Append("    { dataField: '");
                    sb.Append(EscapeString(col.dataField));
                    sb.Append("', caption: '");
                    sb.Append(EscapeString(col.caption));
                    sb.AppendLine("' },");
                }
                sb.Append(indent);
                sb.AppendLine("],");
            }

            // KeyField
            if (!string.IsNullOrEmpty(editor.keyField))
            {
                sb.Append(indent);
                sb.AppendLine($"keyField: '{EscapeString(editor.keyField)}',");
            }

            // DisplayField
            if (!string.IsNullOrEmpty(editor.displayField))
            {
                sb.Append(indent);
                sb.AppendLine($"displayField: '{EscapeString(editor.displayField)}',");
            }

            // ListItems
            if (editor.listItems != null)
            {
                var tsCode = editor.listItems.ToTs();
                if (!string.IsNullOrEmpty(tsCode))
                {
                    // Remove await and getFirstValue for listItems - it should return array directly
                    tsCode = tsCode.Replace("await ", "");
                    // Remove getFirstValue wrapper if present (listItems should return array)
                    if (tsCode.Contains("getFirstValue("))
                    {
                        // Extract the inner expression
                        var startIdx = tsCode.IndexOf("getFirstValue(") + "getFirstValue(".Length;
                        var endIdx = tsCode.LastIndexOf(")");
                        if (endIdx > startIdx)
                        {
                            tsCode = tsCode.Substring(startIdx, endIdx - startIdx);
                        }
                    }
                    sb.Append(indent);
                    sb.AppendLine($"listItems: {tsCode},");
                }
            }

            // ListItemsDeps
            if (editor.listItemsDeps != null && editor.listItemsDeps.Count > 0)
            {
                sb.Append(indent);
                sb.Append("listItemsDeps: [");
                for (int i = 0; i < editor.listItemsDeps.Count; i++)
                {
                    sb.Append($"'{editor.listItemsDeps[i]}'");
                    if (i < editor.listItemsDeps.Count - 1)
                    {
                        sb.Append(", ");
                    }
                }
                sb.AppendLine("],");
            }

            // SingleSelection
            if (editor.singleSelection.HasValue)
            {
                sb.Append(indent);
                sb.AppendLine($"singleSelection: {editor.singleSelection.Value.ToString().ToLower()},");
            }

            // RemoteOperations
            if (editor.remoteOperations.HasValue)
            {
                sb.Append(indent);
                sb.AppendLine($"remoteOperations: {editor.remoteOperations.Value.ToString().ToLower()},");
            }
        }

        private static void GenerateFieldProperty(StringBuilder sb, string propName, MethodInfo methodInfo, string indent)
        {
            if (methodInfo == null)
            {
                return;
            }

            var tsCode = methodInfo.ToTs();
            if (string.IsNullOrEmpty(tsCode))
            {
                return;
            }

            // For defaultValue, remove await and getFirstValue wrapper if present
            // Based on examples, defaultValue uses execQueryByName directly
            if (propName == "defaultValue")
            {
                tsCode = tsCode.Replace("await ", "");
                // Remove getFirstValue wrapper if present
                if (tsCode.Contains("getFirstValue("))
                {
                    var startIdx = tsCode.IndexOf("getFirstValue(") + "getFirstValue(".Length;
                    var endIdx = tsCode.LastIndexOf(")");
                    if (endIdx > startIdx)
                    {
                        tsCode = tsCode.Substring(startIdx, endIdx - startIdx);
                    }
                }
            }

            sb.Append(indent);
            sb.AppendLine($"{propName}: {tsCode},");
        }

        private static void GenerateFieldPropertyArray(StringBuilder sb, string propName, List<string> deps, string indent)
        {
            if (deps == null || deps.Count == 0)
            {
                return;
            }

            sb.Append(indent);
            sb.Append($"{propName}: [");
            for (int i = 0; i < deps.Count; i++)
            {
                sb.Append($"'{deps[i]}'");
                if (i < deps.Count - 1)
                {
                    sb.Append(", ");
                }
            }
            sb.AppendLine("],");
        }
    }
}
