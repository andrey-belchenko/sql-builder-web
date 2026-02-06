
using System.Collections.Generic;
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
