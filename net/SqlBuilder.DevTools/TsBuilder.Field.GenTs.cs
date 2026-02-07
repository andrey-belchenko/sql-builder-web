
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
        private static string ProcessFieldGenTs(VForm form, VField field, IEnumerable<FieldProps> fieldsProps, out FormGenerationState formState, string indent = "        ")
        {
            formState = null;
            var fieldName = field.P_Name;
            if (string.IsNullOrEmpty(fieldName))
            {
                return null;
            }

            var fieldsList = fieldsProps.ToList();
            if (fieldsList.Count == 0)
            {
                return null;
            }

            formState = GetOrCreateFormState(form);

            // Generate inline field or field group code
            string fieldCode;
            if (fieldsList.Count == 1)
            {
                fieldCode = GenerateInlineFieldCode(fieldsList[0], indent);
            }
            else
            {
                fieldCode = GenerateInlineFieldGroupCode(fieldsList, field, indent);
            }

            // Update form state with imports
            UpdateFormStateImports(formState, fieldsList);

            return fieldCode;
        }


        private static bool HasQueryMethod(MethodInfo methodInfo)
        {
            return methodInfo != null && !string.IsNullOrEmpty(methodInfo.queryName);
        }

        private static void UpdateFormStateImports(FormGenerationState formState, List<FieldProps> fieldsProps)
        {
            foreach (var props in fieldsProps)
            {
                if (props.editor != null)
                {
                    formState.EditorTypes.Add(props.editor.editorType);
                }

                // Check if any method uses queryName (needs execQueryByName import)
                if (HasQueryMethod(props.defaultValue))
                {
                    formState.NeedsExecQueryByName = true;
                }

                if (props.editor is SelectEditorProps selectEditor && HasQueryMethod(selectEditor.listItems))
                {
                    formState.NeedsExecQueryByName = true;
                }

                // Check for getFirstValue usage - any property with queryName and isSingleValue = true
                // will generate code that uses getFirstValue
                if (props.defaultValue?.isSingleValue == true && HasQueryMethod(props.defaultValue))
                {
                    formState.NeedsGetFirstValue = true;
                }
                if (props.editor is SelectEditorProps selectEditor2 && selectEditor2.listItems?.isSingleValue == true && HasQueryMethod(selectEditor2.listItems))
                {
                    formState.NeedsGetFirstValue = true;
                }
                if (props.required?.isSingleValue == true && HasQueryMethod(props.required))
                {
                    formState.NeedsGetFirstValue = true;
                }
                if (props.validation?.isSingleValue == true && HasQueryMethod(props.validation))
                {
                    formState.NeedsGetFirstValue = true;
                }
                if (props.enabled?.isSingleValue == true && HasQueryMethod(props.enabled))
                {
                    formState.NeedsGetFirstValue = true;
                }
                if (props.visible?.isSingleValue == true && HasQueryMethod(props.visible))
                {
                    formState.NeedsGetFirstValue = true;
                }
                if (props.exists?.isSingleValue == true && HasQueryMethod(props.exists))
                {
                    formState.NeedsGetFirstValue = true;
                }
            }
        }

        private static string GenerateInlineFieldCode(FieldProps props, string indent)
        {
            var sb = new StringBuilder();
            var editorType = props.editor?.editorType ?? "TextEditor";

            sb.Append(indent);
            sb.AppendLine("new Field({");

            // Label
            if (!string.IsNullOrEmpty(props.label))
            {
                sb.Append(indent);
                sb.Append("    label: '");
                sb.Append(EscapeString(props.label));
                sb.AppendLine("',");
            }

            // Name
            sb.Append(indent);
            sb.Append("    name: '");
            sb.Append(props.name);
            sb.AppendLine("',");

            // Editor
            sb.Append(indent);
            sb.Append("    editor: new ");
            sb.Append(editorType);
            sb.AppendLine("({");
            GenerateEditorCode(sb, props.editor, indent + "        ", false);
            sb.Append(indent);
            sb.AppendLine("    }),");

            // Other field properties
            GenerateFieldProperty(sb, "defaultValue", props.defaultValue, indent + "    ");
            GenerateFieldPropertyArray(sb, "defaultValueDeps", props.defaultValueDeps, indent + "    ");
            GenerateFieldProperty(sb, "required", props.required, indent + "    ");
            GenerateFieldPropertyArray(sb, "requiredDeps", props.requiredDeps, indent + "    ");
            GenerateFieldProperty(sb, "validation", props.validation, indent + "    ");
            GenerateFieldPropertyArray(sb, "validationDeps", props.validationDeps, indent + "    ");
            GenerateFieldProperty(sb, "exists", props.exists, indent + "    ");
            GenerateFieldPropertyArray(sb, "existsDeps", props.existsDeps, indent + "    ");
            GenerateFieldProperty(sb, "enabled", props.enabled, indent + "    ");
            GenerateFieldPropertyArray(sb, "enabledDeps", props.enabledDeps, indent + "    ");
            GenerateFieldProperty(sb, "visible", props.visible, indent + "    ");
            GenerateFieldPropertyArray(sb, "visibleDeps", props.visibleDeps, indent + "    ");

            sb.Append(indent);
            sb.Append("})");

            return sb.ToString();
        }

        private static string GenerateInlineFieldGroupCode(List<FieldProps> fieldsProps, VField field, string indent)
        {
            var sb = new StringBuilder();

            sb.Append(indent);
            sb.AppendLine("new FieldGroup({");

            // Group label - use field title or generate from field name
            var groupLabel = field.P_Title ?? field.P_Name ?? "";
            if (string.IsNullOrEmpty(groupLabel))
            {
                groupLabel = fieldsProps.FirstOrDefault()?.label ?? "";
            }
            sb.Append(indent);
            sb.Append("    label: '");
            sb.Append(EscapeString(groupLabel));
            sb.AppendLine("',");

            sb.Append(indent);
            sb.AppendLine("    items: [");

            // Generate each field
            for (int i = 0; i < fieldsProps.Count; i++)
            {
                var props = fieldsProps[i];
                var fieldCode = GenerateInlineFieldCode(props, indent + "        ");
                sb.Append(fieldCode);

                if (i < fieldsProps.Count - 1)
                {
                    sb.AppendLine(",");
                }
                else
                {
                    sb.AppendLine();
                }
            }

            sb.Append(indent);
            sb.Append("    ]");
            sb.AppendLine();
            sb.Append(indent);
            sb.Append("})");

            return sb.ToString();
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
