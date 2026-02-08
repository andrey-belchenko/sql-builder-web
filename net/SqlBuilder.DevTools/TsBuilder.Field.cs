
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using sql.builder;
using sql.builder.DataApi;
using sql.builder.UI;



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
// "name-field-name"


// Not implemented
// "hint"
// "format"
// "step"
// "search-field-name"
// "expand-all"
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
        private static string ProcessField(VForm form, VField field, CleanExpressReport rep)
        {






            // devControlTypes.Add(field.P_ControlType);

            // foreach (var attr in field.Attributes())
            // {
            //     devAttrNames.Add(attr.Name.LocalName);
            // }

            var fieldsInfos = GetFieldInfo(field, rep).ToList();

            //ProcessQuery(fieldsInfos[0].ListQuery);
            //ProcessQuery(fieldsInfos[0].Default);
            var fieldsProps = fieldsInfos.Select(it => FIeldInfoToFieldProps(it)).ToList();
            var fieldCode = ProcessFieldGenTs(form, field, fieldsProps, out FormGenerationState formState);
            if (fieldCode != null && formState != null)
            {
                formState.Items.Add(fieldCode);
            }
            return null;



        }

        public static IEnumerable<SqlbFieldInfo> GetFieldInfo(VField field, CleanExpressReport rep)
        {

            // var fieldNames = new[] { field.P_Name };
            var fieldNames = new Dictionary<string, string>();
            if (field.P_ControlType.Contains("Range"))
            {

                fieldNames.Add(field.P_Name + "1", "С");
                fieldNames.Add(field.P_Name + "2", "По");
            }
            else
            {
                fieldNames.Add(field.P_Name, field.P_Title);
            }

            var fieldInfos = new List<SqlbFieldInfo>();

            foreach (var fld in fieldNames)
            {
                var fieldInfo = new SqlbFieldInfo();
                fieldInfos.Add(fieldInfo);

                var ctrl = rep.GetParamField(fld.Key).Control;

                fieldInfo.Title = fld.Value;
                fieldInfo.Name = fld.Key;
                fieldInfo.Default = ProcessQuery(ctrl.query_name_default);
                fieldInfo.Editable = field.P_Editable ?? string.Empty;
                fieldInfo.ColumnEditable = ProcessQuery(field.P_ColumnEditable);
                fieldInfo.Valid = field.P_Valid ?? string.Empty;
                fieldInfo.Visible = field.P_Visible ?? string.Empty;
                fieldInfo.ColumnVisible = ProcessQuery(field.P_ColumnVisible);
                fieldInfo.Mandatory = field.P_Mandatory ?? string.Empty;
                fieldInfo.ColumnMandatory = ProcessQuery(field.P_ColumnMandatory);
                fieldInfo.ControlType = ctrl.GetType();
                fieldInfo.RowsLimit = ctrl.rows_limit;


                fieldInfo.ValFieldName = ctrl.value_field_name;

                fieldInfo.Hint = field.P_Hint ?? string.Empty;
                fieldInfo.Format = field.P_Format ?? string.Empty;
                fieldInfo.Step = field.P_Step ?? string.Empty;
                fieldInfo.ExpandAll = field.P_ExpandAll ?? string.Empty;
                fieldInfo.ParentFieldName = ctrl.parent_field_name;
                fieldInfo.EditMask = field.P_EditMask ?? string.Empty;
                fieldInfo.SearchFieldName = ctrl.search_field_name;
                fieldInfo.NameFieldName = ctrl.name_field_name;

                if (string.IsNullOrWhiteSpace(fieldInfo.NameFieldName))
                {
                    fieldInfo.NameFieldName = fieldInfo.ValFieldName;
                }

                fieldInfo.ListQuery = ProcessQuery(ctrl.query_name);





                if (ctrl.data_set_list != null)
                {
                    foreach (System.Data.DataColumn col in ctrl.data_set_list.Tables[0].Columns)
                    {
                        if (UIBase.IsColumnShouldBeVisible(col) && col.ColumnName != "check")
                        {
                            fieldInfo.ListColumns.Add(col.ColumnName, col.Caption);
                        }

                    }
                }

                fieldInfo.Dependencies = ctrl.Masters.Keys.ToList();

            }
            return fieldInfos;
        }

        public static FieldProps FIeldInfoToFieldProps(SqlbFieldInfo fieldInfo)
        {
            var props = new FieldProps();

            // Basic properties
            props.label = fieldInfo.Title;
            props.name = fieldInfo.Name;

            props.hint = new MethodInfo() { value = fieldInfo.Hint };

            // Parse expressions to MethodInfo
            props.defaultValue = CreateMethodInfo(fieldInfo.Default);

            props.defaultValueDeps = fieldInfo.Dependencies?.ToList();

            props.required = CreateMethodInfo(fieldInfo.ColumnMandatory, fieldInfo.Mandatory, false);
            if (props.required?.fieldRef != null)
            {
                props.requiredDeps = (new[] { props.required.fieldRef }).ToList();
            }


            props.validation = CreateMethodInfo(null, fieldInfo.Valid);
            if (props.validation?.fieldRef != null)
            {
                props.validationDeps = (new[] { props.validation.fieldRef }).ToList();
            }

            props.enabled = CreateMethodInfo(fieldInfo.ColumnEditable, fieldInfo.Editable, true);
            if (props.enabled?.fieldRef != null)
            {
                props.enabledDeps = (new[] { props.enabled.fieldRef }).ToList();
            }

            props.visible = CreateMethodInfo(fieldInfo.ColumnVisible, fieldInfo.Visible, true);
            if (props.visible?.fieldRef != null)
            {
                props.visibleDeps = (new[] { props.visible.fieldRef }).ToList();
            }

            //// exists might map to column-visible or similar, using ColumnVisible for now
            //props.exists = CreateMethodInfo(fieldInfo.ColumnVisible);
            //if (props.exists?.fieldRef != null)
            //{
            //    props.existsDeps = (new[] { props.exists.fieldRef }).ToList();
            //}

            // Create editor based on ControlType
            props.editor = CreateEditor(fieldInfo);

            if (props.defaultValue != null)
            {
                if (props.editor is SelectEditorProps sep && (!sep.singleSelection.HasValue || !sep.singleSelection.Value))
                {
                    props.defaultValue.isSingleValue = false;
                }
                else
                {
                    props.defaultValue.isSingleValue = true;
                }
            }



            return props;
        }

        private static bool? ToBool(string value)
        {
            if (value == "0")
            {
                return false;
            }

            if (value == "false")
            {
                return false;
            }

            if (value == "true")
            {
                return true;
            }

            if (value == "1")
            {
                return true;
            }

            return null;
        }

        private static MethodInfo CreateMethodInfo(string colExp, string fieldExpr = null, bool dflt = true)
        {
            var methodInfo = new MethodInfo();
            if (string.IsNullOrWhiteSpace(fieldExpr))
            {
                fieldExpr = null;
            }
            var expr = fieldExpr ?? colExp;
            var boolValue = ToBool(expr);

            if (boolValue != null)
            {
                if (boolValue != dflt)
                {
                    methodInfo.value = boolValue.Value;
                }

                return methodInfo;
            }


            if (!string.IsNullOrEmpty(fieldExpr))
            {
                methodInfo.fieldRef = fieldExpr;
                return methodInfo;
            }

            if (!string.IsNullOrEmpty(colExp))
            {
                methodInfo.queryName = ClearName(colExp);
                if (methodInfo.queryName == "1")
                {

                }
                return methodInfo;
            }

            return null;

        }

        private static EditorProps CreateEditor(SqlbFieldInfo fieldInfo)
        {
            var controlTypeName = fieldInfo.ControlType.Name;
            var editorType = "";
            var isSingle = true;
            if (controlTypeName == "UICombo")
            {
                editorType = "SelectEditor";
            }
            if (controlTypeName == "UIList")
            {
                isSingle = false;
                editorType = "SelectEditor";
            }
            if (controlTypeName == "UIDate")
            {
                editorType = "DateEditor";
            }

            if (controlTypeName == "UIText")
            {
                editorType = "TextEditor";
            }

            if (controlTypeName == "UICheck")
            {
                editorType = "CheckEditor";
            }

            if (controlTypeName == "UINumber")
            {
                editorType = "NumberEditor";
            }

            if (editorType == "SelectEditor")
            {
                var selectEditor = new SelectEditorProps();
                selectEditor.columns = fieldInfo.ListColumns?.Select(kvp => new ColumnInfo
                {
                    dataField = kvp.Key,
                    caption = kvp.Value
                }).ToList();

                if (selectEditor.columns.Count() == 0)
                {
                    selectEditor.columns.Add(new ColumnInfo
                    {
                        dataField = fieldInfo.NameFieldName,
                        caption = " "
                    });
                }

                selectEditor.keyField = fieldInfo.ValFieldName;
                selectEditor.displayField = fieldInfo.NameFieldName;
                if (isSingle)
                {
                    selectEditor.singleSelection = isSingle;
                }
                selectEditor.listItems = CreateMethodInfo(fieldInfo.ListQuery);
                if (fieldInfo.RowsLimit > 0)
                {
                    selectEditor.remoteOperations = true;
                    selectEditor.listItems.useLoadOptions = true;
                }


                selectEditor.listItemsDeps = fieldInfo.Dependencies;
                selectEditor.listItems.isSingleValue = false;
                selectEditor.editorType = editorType;
                return selectEditor;
            }
            return new EditorProps() { editorType = editorType, format = fieldInfo.Format, editMask = fieldInfo.EditMask };
        }
    }

    public class SqlbFieldInfo
    {
        // Apply fields
        public string Title;
        public string Name;
        public string Editable;
        public string ColumnEditable;
        public string Default;
        public string Valid;
        public string Visible;
        public string ColumnVisible;
        public string Mandatory;
        public string ColumnMandatory;
        public Type ControlType;
        public string ValFieldName;
        public int RowsLimit;

        // Not implemented fields
        public string Hint;
        public string Format;
        public string Step;
        public string SearchFieldName;
        public string ExpandAll;
        public string NameFieldName;
        public string ParentFieldName;
        public string EditMask;

        public string ListQuery;
        public Dictionary<string, string> ListColumns = new Dictionary<string, string>();
        public List<string> Dependencies = new List<string>();
    }

    public class MethodInfo
    {
        public object value;
        public string fieldRef;
        public string queryName;

        public bool isSingleValue = true;

        public bool useLoadOptions = false;

        public string ToTs()
        {
            if (value != null)
            {
                string formattedValue;
                if (value is bool boolValue)
                {
                    formattedValue = boolValue.ToString().ToLower();
                }
                else if (value is string stringValue)
                {
                    formattedValue = $"'{stringValue}'";
                }
                else
                {
                    formattedValue = value.ToString();
                }
                return $"() => {formattedValue}";
            }

            if (fieldRef != null)
            {
                return $"($) => $.formValues['{fieldRef}']";
            }

            if (queryName != null)
            {
                if (isSingleValue)
                {
                    return $"async $ => getFirstValue(await execQueryByName('{queryName}', $) as [])";
                }
                else
                {
                    if (useLoadOptions)
                    {
                        return $"$ => execQueryByName('{queryName}', $, true)";
                    }
                    else
                    {
                        return $"$ => execQueryByName('{queryName}', $)";
                    }

                }

            }
            return null;
        }
    }

    public class ColumnInfo
    {
        public string dataField;
        public string caption;
    }

    public class FormItemProps
    {
        // Base properties for form items can be added here if needed
    }

    public class FieldProps
    {
        public string label;
        public string name;
        public EditorProps editor; public MethodInfo defaultValue;
        public List<string> defaultValueDeps;
        public MethodInfo required;
        public List<string> requiredDeps;
        public MethodInfo validation;
        public List<string> validationDeps;
        public MethodInfo exists;
        public List<string> existsDeps;
        public MethodInfo enabled;
        public List<string> enabledDeps;
        public MethodInfo visible;
        public List<string> visibleDeps;

        public MethodInfo hint;
    }

    public class EditorProps
    {
        public string editorType;
        public string format;
        public string editMask;
    }

    public class SelectEditorProps : EditorProps
    {
        public List<ColumnInfo> columns;
        public MethodInfo listItems;
        public List<string> listItemsDeps;
        public string keyField;
        public string displayField;
        public bool? remoteOperations;
        public bool? singleSelection;
        public string searchField;
        public string expandAll;
        public string parentKeyField;
    }
}


