
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


/////////////////////
// "UIComboRange"
// "UIList"
// "UICombo"
// "UIDate"
// "UIText"
// "UINumber"
// "UICheck"
// "UIDateRange"



namespace SqlBuilderLib.DevTools
{

    public static partial class TsBuilder
    {
        public static HashSet<string> devAttrNames = new HashSet<string>();
        public static HashSet<string> devControlTypes = new HashSet<string>();
        private static string ProcessField(VForm form, VField field)
        {
            var fieldName = field.P_Field;
            var fieldFileName = $"field_{ClearName(fieldName)}.ts";

            
            ProcessQuery(field.ListQuery());
            ProcessQuery(field.DefaultQuery());

            devControlTypes.Add(field.P_ControlType);

            foreach (var attr in field.Attributes())
            {
                devAttrNames.Add(attr.Name.LocalName);
            }
            return null;

        }

        public static FieldInfo GetFieldInfo(VField field)
        {
            var fieldInfo = new FieldInfo();

            // Apply fields - using P_ properties
            fieldInfo.Title = field.P_Title ?? string.Empty;
            fieldInfo.Name = field.P_Name ?? string.Empty;
            fieldInfo.Editable = field.P_Editable ?? string.Empty;
            fieldInfo.ColumnEditable = field.P_ColumnEditable ?? string.Empty;
            fieldInfo.Default = field.P_Default ?? string.Empty;
            fieldInfo.Valid = field.P_Valid ?? string.Empty;
            fieldInfo.Visible = field.P_Visible ?? string.Empty;
            fieldInfo.ColumnVisible = field.P_ColumnVisible ?? string.Empty;
            fieldInfo.Mandatory = field.P_Mandatory ?? string.Empty;
            fieldInfo.ColumnMandatory = field.P_ColumnMandatory ?? string.Empty;
            fieldInfo.ControlType = field.P_ControlType ?? string.Empty;
            fieldInfo.RowsLimit = field.P_RowsLimit ?? string.Empty;

            // Fields without P_ properties - using AttrOrEmpty
            fieldInfo.Valuequery = field.AttrOrEmpty(AName.valuequery);
            fieldInfo.ValFieldName = field.AttrOrEmpty(AName.val_field_name);

            // Not implemented fields - using P_ properties where available
            fieldInfo.Hint = field.P_Hint ?? string.Empty;
            fieldInfo.Format = field.P_Format ?? string.Empty;
            fieldInfo.Step = field.P_Step ?? string.Empty;
            fieldInfo.ExpandAll = field.P_ExpandAll ?? string.Empty;
            fieldInfo.ParentFieldName = field.P_ParentFieldName ?? string.Empty;
            fieldInfo.EditMask = field.P_EditMask ?? string.Empty;

            // Fields without P_ properties - using AttrOrEmpty
            fieldInfo.SearchFieldName = field.AttrOrEmpty(AName.search_field_name);
            fieldInfo.NameFieldName = field.AttrOrEmpty(AName.name_field_name);

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
