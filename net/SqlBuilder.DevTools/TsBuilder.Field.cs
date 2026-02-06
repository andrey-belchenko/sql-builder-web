
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
        public static HashSet<string> attrNames =  new HashSet<string>();
        private static string ProcessField(VForm form, VField field)
        {
            var fieldName = field.P_Field;
            var fieldFileName = $"field_{ClearName(fieldName)}.ts";
            ProcessQuery(field.ListQuery());
            ProcessQuery(field.DefaultQuery());

            foreach (var attr in field.Attributes()){
               attrNames.Add(attr.Name.LocalName); 
            }
            return null;

        }
    }
}
