
using System.Collections.Generic;
using System.Xml.Linq;
using sql.builder.DataApi;



// Apply
// "title"
// "name"
// "editable"
// "column-editable"
// "default"
// "valid"
// "visible"
// "column-visible"
// "mandatory"
// "column-mandatory"
// "valuequery"
// "controlType"
// "val-field-name"
// "rows-limit"


// Not implemented
// "hint"
// "format"
// "step"
// "search-field-name"
// "expand-all"
// "name-field-name"
// "parent-field-name"
// "edit-mask"

// Incompatible
// "special-type"
// "clear-on-list-change"
// "width-perc"
// "halign"
// "checked"
// "show-checkbox"
// "auto-check"
// "type"
// "show-nulls"


// Unnecessary
// "part-id"
// "comment"
// "class"
// "field"
// "id"


namespace SqlBuilderLib.DevTools
{

    public static partial class TsBuilder
    {
        public static HashSet<string> attrNames = new HashSet<string>();
        private static string ProcessField(VForm form, VField field)
        {
            var fieldName = field.P_Field;
            var fieldFileName = $"field_{ClearName(fieldName)}.ts";
            ProcessQuery(field.ListQuery());
            ProcessQuery(field.DefaultQuery());

            foreach (var attr in field.Attributes())
            {
                attrNames.Add(attr.Name.LocalName);
            }
            return null;

        }

        public static FieldInfo GetFieldInfo(VField field)
        {
            var fieldInfo = new FieldInfo();

            foreach (var attr in field.Attributes())
            {
                string attrName = attr.Name.LocalName;
                string attrValue = attr.Value;

                switch (attrName)
                {
                    // Apply fields
                    case "title":
                        fieldInfo.Title = attrValue;
                        break;
                    case "name":
                        fieldInfo.Name = attrValue;
                        break;
                    case "editable":
                        fieldInfo.Editable = attrValue;
                        break;
                    case "column-editable":
                        fieldInfo.ColumnEditable = attrValue;
                        break;
                    case "default":
                        fieldInfo.Default = attrValue;
                        break;
                    case "valid":
                        fieldInfo.Valid = attrValue;
                        break;
                    case "visible":
                        fieldInfo.Visible = attrValue;
                        break;
                    case "column-visible":
                        fieldInfo.ColumnVisible = attrValue;
                        break;
                    case "mandatory":
                        fieldInfo.Mandatory = attrValue;
                        break;
                    case "column-mandatory":
                        fieldInfo.ColumnMandatory = attrValue;
                        break;
                    case "valuequery":
                        fieldInfo.Valuequery = attrValue;
                        break;
                    case "controlType":
                        fieldInfo.ControlType = attrValue;
                        break;
                    case "val-field-name":
                        fieldInfo.ValFieldName = attrValue;
                        break;
                    case "rows-limit":
                        fieldInfo.RowsLimit = attrValue;
                        break;

                    // Not implemented fields
                    case "hint":
                        fieldInfo.Hint = attrValue;
                        break;
                    case "format":
                        fieldInfo.Format = attrValue;
                        break;
                    case "step":
                        fieldInfo.Step = attrValue;
                        break;
                    case "search-field-name":
                        fieldInfo.SearchFieldName = attrValue;
                        break;
                    case "expand-all":
                        fieldInfo.ExpandAll = attrValue;
                        break;
                    case "name-field-name":
                        fieldInfo.NameFieldName = attrValue;
                        break;
                    case "parent-field-name":
                        fieldInfo.ParentFieldName = attrValue;
                        break;
                    case "edit-mask":
                        fieldInfo.EditMask = attrValue;
                        break;
                }
            }

            return fieldInfo;
        }
    }

    public class FieldInfo
    {
        // Apply fields
        public string Title { get; set; }
        public string Name { get; set; }
        public string Editable { get; set; }
        public string ColumnEditable { get; set; }
        public string Default { get; set; }
        public string Valid { get; set; }
        public string Visible { get; set; }
        public string ColumnVisible { get; set; }
        public string Mandatory { get; set; }
        public string ColumnMandatory { get; set; }
        public string Valuequery { get; set; }
        public string ControlType { get; set; }
        public string ValFieldName { get; set; }
        public string RowsLimit { get; set; }

        // Not implemented fields
        public string Hint { get; set; }
        public string Format { get; set; }
        public string Step { get; set; }
        public string SearchFieldName { get; set; }
        public string ExpandAll { get; set; }
        public string NameFieldName { get; set; }
        public string ParentFieldName { get; set; }
        public string EditMask { get; set; }
    }
}
