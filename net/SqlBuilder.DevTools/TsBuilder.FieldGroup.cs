using System.Collections.Generic;
using System.Linq;
using System.Text;
using sql.builder;
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {
        private static string ProcessFieldGroup(VForm form, VFieldGroup fieldGroup, CleanExpressReport rep, string indent = "        ")
        {
            // Collect child items
            var childItems = new List<string>();
            // Deeper indent for items inside FieldGroup
            var itemsIndent = indent + "            ";

            // Process children recursively
            foreach (var element in fieldGroup.GetElementsP())
            {
                if (element is VField field)
                {
                    var fieldsProps = GetFieldInfo(field, rep).Select(it => FIeldInfoToFieldProps(it)).ToList();
                    // Use deeper indent for fields inside FieldGroup
                    var fieldCode = ProcessFieldGenTs(form, field, fieldsProps, out FormGenerationState _, itemsIndent);
                    if (fieldCode != null)
                    {
                        childItems.Add(fieldCode);
                    }
                }
                else if (element is VFieldGroup nestedGroup)
                {
                    // Use deeper indent for nested FieldGroup
                    var nestedGroupCode = ProcessFieldGroup(form, nestedGroup, rep, itemsIndent);
                    if (nestedGroupCode != null)
                    {
                        childItems.Add(nestedGroupCode);
                    }
                }
            }

            if (childItems.Count == 0)
            {
                return null;
            }

            // Generate FieldGroup code
            var sb = new StringBuilder();
            sb.Append(indent);
            sb.AppendLine("new FieldGroup({");

            // Group label
            var groupLabel = fieldGroup.P_Title ?? fieldGroup.P_Name ?? "";
            if (string.IsNullOrEmpty(groupLabel))
            {
                groupLabel = "Group";
            }
            sb.Append(indent);
            sb.Append("    label: '");
            sb.Append(EscapeString(groupLabel));
            sb.AppendLine("',");

            sb.Append(indent);
            sb.AppendLine("    items: [");

            for (int i = 0; i < childItems.Count; i++)
            {
                // Child items are already properly indented
                sb.Append(childItems[i]);

                if (i < childItems.Count - 1)
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
    }
}
