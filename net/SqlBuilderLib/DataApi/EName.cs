using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /// <summary>
    /// Набор имён используемых xml-тегов для использования вместо <see cref="TextConst.EName"/> и <see cref="TextConst.DsEName"/>
    /// </summary>
    internal static class EName
    {
        internal static readonly XName action;               //= TextConst.EName.Action;
        internal static readonly XName actions;              //= TextConst.EName.Actions;
        internal static readonly XName addition;             //= TextConst.EName.Addition;
        internal static readonly XName additions;            //= TextConst.EName.Additions;
        internal static readonly XName array;                //= TextConst.EName.Array;
        internal static readonly XName band;                 //= TextConst.EName.Band;
        internal static readonly XName button_type;          //= TextConst.EName.ButtonType;
        internal static readonly XName button_types;         //= TextConst.EName.ButtonTypes;
        internal static readonly XName buttons;              //= TextConst.EName.Buttons;
        internal static readonly XName call;                 //= TextConst.EName.Call;
        internal static readonly XName cells;                //= TextConst.EName.Cells;
        internal static readonly XName childs;               //= TextConst.EName.Childs;
        internal static readonly XName clear_temp_text;      //= TextConst.DsEName.ClearTempText;
        internal static readonly XName col_dim_val;          //= TextConst.EName.ColDimVal;
        internal static readonly XName color;                //= TextConst.EName.Color;
        internal static readonly XName color_package;        //= TextConst.EName.ColorPackage;
        internal static readonly XName color_packages;       //= TextConst.EName.ColorPackages;
        internal static readonly XName column;               //= TextConst.EName.Column;
        internal static readonly XName columnspreset;        //= TextConst.EName.ColumnsPreset
        internal static readonly XName columns;              //= TextConst.EName.Columns;
        internal static readonly XName compiled;             //= TextConst.EName.Compiled;
        internal static readonly XName @const;               //= TextConst.EName.Const;
        internal static readonly XName content;              //= TextConst.EName.Content;
        internal static readonly XName connect;              //= TextConst.EName.Connect;
        internal static readonly XName customer;             //= TextConst.EName.Customer;
        internal static readonly XName customers;            //= TextConst.EName.Customers;
        internal static readonly XName data;                 //= TextConst.EName.Data;
        internal static readonly XName dataset;              //= TextConst.DsEName.DataSet;
        internal static readonly XName datatype;             //= "datatype";
        internal static readonly XName datatypes;            //= "datatypes";
        internal static readonly XName defaultquery;         //= TextConst.EName.DefaultQuery;
        internal static readonly XName delete_text;          //= TextConst.DsEName.DeleteText;
        internal static readonly XName dependant;            //= TextConst.DsEName.Dependant;
        internal static readonly XName dependants;           //= TextConst.DsEName.Dependants;
        internal static readonly XName dep_refresh_cmd;      //= TextConst.DsEName.DepRefreshCommand;
        internal static readonly XName dimension;            //= TextConst.EName.Dimension;
        internal static readonly XName dimension_сolumns;    //= "dimension-сolumns";
        internal static readonly XName dimension_package;    //= TextConst.EName.DimensionPackage;
        internal static readonly XName dimension_packages;   //= TextConst.EName.DimensionPackages;
        internal static readonly XName dimension_values;     //= TextConst.EName.DimensionValues;
        internal static readonly XName dimlink;              //= TextConst.EName.DimLink;
        internal static readonly XName dimset;               //= TextConst.EName.DimSet;
        internal static readonly XName dlink;                //= TextConst.EName.DLink;
        internal static readonly XName elink;                //= TextConst.EName.ELink;
        internal static readonly XName empty_item;           //= TextConst.EName.EmptyItem;
        internal static readonly XName events;               //= TextConst.EName.Events;
        internal static readonly XName excel;                //= TextConst.EName.Excel;
        //internal static readonly XName excel_templates;    //= TextConst.EName.ExcelTemplates;
        internal static readonly XName expression_package;   //= TextConst.EName.ExpressionPackage;
        internal static readonly XName expression_packages;  //= TextConst.EName.ExpressionPackages;
        internal static readonly XName expressions;          //= TextConst.EName.Expressions;
        internal static readonly XName extendlinks;          //= TextConst.EName.ExtendLinks;
        internal static readonly XName extendwhere;          //= TextConst.EName.ExtendWhere;
        internal static readonly XName extra_key;            //= TextConst.DsEName.ExtraKey;
        internal static readonly XName fact;                 //= TextConst.EName.Fact;
        internal static readonly XName factlinks;            //= TextConst.EName.Factlinks;
        internal static readonly XName field;                //= TextConst.EName.Field;
        internal static readonly XName fieldgroup;           //= TextConst.EName.FieldGroup;
        internal static readonly XName fields;               //= TextConst.EName.Fields;
        internal static readonly XName folders;              //= TextConst.EName.Folders;
        internal static readonly XName folder;               //= TextConst.EName.Folder;
        internal static readonly XName form;                 //= TextConst.EName.Form;
        internal static readonly XName format;               //= TextConst.EName.Format;
        internal static readonly XName format_package;       //= TextConst.EName.FormatPackage;
        internal static readonly XName format_packages;      //= TextConst.EName.FormatPackages;
        internal static readonly XName forms;                //= TextConst.EName.Forms;
        internal static readonly XName from;                 //= TextConst.EName.From;
        internal static readonly XName function;             //= TextConst.EName.Function;
        internal static readonly XName functions;            //= TextConst.EName.Functions;
        internal static readonly XName globalparams;         //= TextConst.EName.GlobalParams;
        internal static readonly XName grid;                 //= TextConst.EName.Grid;
        internal static readonly XName group;                //= TextConst.EName.Group;
        internal static readonly XName grouping;             //= TextConst.EName.Grouping;
        internal static readonly XName grset;                //= TextConst.EName.Grset;
        internal static readonly XName having;               //= TextConst.EName.Having;
        internal static readonly XName @if;                  //= TextConst.EName.If;
        internal static readonly XName insert;               //= "insert";
        internal static readonly XName insert_text;          //= TextConst.DsEName.InsertText;
        internal static readonly XName joinon;               //= "joinon";
        internal static readonly XName label;                //= TextConst.EName.Label;
        internal static readonly XName link;                 //= TextConst.EName.Link;
        internal static readonly XName links;                //= TextConst.EName.Links;
        internal static readonly XName listquery;            //= TextConst.EName.ListQuery;
        internal static readonly XName menu;                 //= TextConst.EName.Menu;
        internal static readonly XName measures;             //= TextConst.EName.Measures;
        internal static readonly XName multireference;       //= TextConst.EName.Multireference;
        internal static readonly XName navigator;            //= TextConst.EName.Navigator;
        internal static readonly XName navigators;           //= TextConst.EName.Navigators;
        internal static readonly XName qube;                 //= TextConst.EName.Qube;
        internal static readonly XName queries;              //= TextConst.EName.Queries;
        internal static readonly XName query;                //= TextConst.EName.Query;
        internal static readonly XName param;                //= TextConst.EName.Param;
        internal static readonly XName @params;              //= TextConst.EName.Params;
        internal static readonly XName part;                 //= TextConst.EName.Part;
        internal static readonly XName parts;                //= TextConst.EName.Parts;
        internal static readonly XName pivot;                //= TextConst.EName.Pivot;
        internal static readonly XName print_templates;      //= TextConst.EName.PrintTemplates
        internal static readonly XName proc_text;            //= TextConst.DsEName.ProcText;
        internal static readonly XName procedure;            //= TextConst.EName.ReportProc;
        internal static readonly XName project;              //= TextConst.EName.Project;
        internal static readonly XName projects;             //= TextConst.EName.Projects;
        internal static readonly XName push;                 //= TextConst.EName.Push;
        internal static readonly XName reference;            //= TextConst.EName.Reference;
        internal static readonly XName references;           //= TextConst.EName.References;
        internal static readonly XName report;               //= TextConst.EName.Report;
        internal static readonly XName reports;              //= TextConst.EName.Reports;
        internal static readonly XName role;                 //= TextConst.EName.Role;
        internal static readonly XName root;                 //= TextConst.EName.Root;
        internal static readonly XName rowactions;           //= TextConst.EName.Rowactions;
        internal static readonly XName scope;                  //= TextConst.EName.Scope;
        internal static readonly XName scheme;                 //= TextConst.EName.Scheme;
        internal static readonly XName scrollarea;             //= TextConst.EName.ScrollArea;
        internal static readonly XName section;                //= TextConst.EName.Section;
        internal static readonly XName select;                 //= TextConst.EName.Select;
        internal static readonly XName select_text;            //= TextConst.DsEName.SelectText;
        internal static readonly XName sel_list_cl_fact_pars;  //= TextConst.DsEName.SelListClFactPars;
        internal static readonly XName sel_list_compiled;      //= TextConst.DsEName.SelListCompiled;
        internal static readonly XName sel_list_pars;          //= TextConst.DsEName.SelListPars;
        internal static readonly XName sel_list_report;        //= TextConst.DsEName.SelListReport;
        internal static readonly XName security_package;       //= TextConst.EName.SecurityPackage;
        internal static readonly XName single_row_refresh_cmd; //= TextConst.DsEName.SingleRowRefreshCmd;
        internal static readonly XName slink;                  //= TextConst.EName.SLink;
        internal static readonly XName sourcelink;             //= TextConst.EName.SourceLink;
        internal static readonly XName splitcontainer;         //= TextConst.EName.SplitContainer;
        internal static readonly XName splitter;               //= TextConst.EName.Splitter;
        internal static readonly XName start;                  //= TextConst.EName.Start;
        internal static readonly XName tabcontainer;           //= TextConst.EName.TabContainer;
        internal static readonly XName table;                  //= TextConst.EName.Table;
        internal static readonly XName template;               //= TextConst.EName.Template
        internal static readonly XName text;                   //= TextConst.EName.Text;
        internal static readonly XName toolbar;                //= TextConst.EName.Toolbar;
        internal static readonly XName td;                     //= TextConst.EName.Td;
        internal static readonly XName tr;                     //= TextConst.EName.Tr;
        internal static readonly XName transpose;              //= "transpose";
        internal static readonly XName uicommand;              //= TextConst.EName.UICommand;
        internal static readonly XName undefined;              //= TextConst.EName.Undefined;
        internal static readonly XName union;                  //= TextConst.EName.Union;
        internal static readonly XName update_temp_text;       //= TextConst.DsEName.UptadeTempText;
        internal static readonly XName update_text;            //= TextConst.DsEName.UpdateText;
        internal static readonly XName use_color;              //= TextConst.EName.UseColor;
        internal static readonly XName use_object;             //= TextConst.EName.UseObject;
        internal static readonly XName use_role;               //= TextConst.EName.UseRole;
        internal static readonly XName useaction;              //= TextConst.EName.UseAction;
        internal static readonly XName usefield;               //= TextConst.EName.UseField;
        internal static readonly XName useform;                //= TextConst.EName.UseForm;
        internal static readonly XName useglobparam;           //= TextConst.EName.UseGlobParam;
        internal static readonly XName useparam;               //= TextConst.EName.UseParam;
        internal static readonly XName usepart;                //= TextConst.EName.UsePart;
        internal static readonly XName usereport;              //= TextConst.EName.UseReport;
        internal static readonly XName @using;                 //= TextConst.EName.Using;
        internal static readonly XName val;                    //= TextConst.EName.Val;
        internal static readonly XName value_сolumns;          //= "value-сolumns";
        internal static readonly XName value_refresh_cmd;      //= TextConst.DsEName.ValueRefreshCmd;
        internal static readonly XName value_reset_cmd;        //= TextConst.DsEName.ValueResetCmd;
        internal static readonly XName values;                 //= "values";
        internal static readonly XName view;                   //= "view";
        internal static readonly XName viewcolumns;            //= TextConst.EName.ViewColumns;
        internal static readonly XName views;                  //= "views"
        internal static readonly XName withparams;             //= TextConst.EName.WithParams;
        internal static readonly XName where;                  //= TextConst.EName.Where;
        internal static readonly XName word;                   //= TextConst.EName.Word;
        //
        internal static void InitXNameStaticFields(XNamespace ns, Type type)
        {
            System.Reflection.FieldInfo[] fields = type.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            for (int index = 0; index < fields.Length; index++) {
                System.Reflection.FieldInfo field_info = fields[index];
                if (field_info.GetValue(null) == null) {
                    field_info.SetValue(null, ns.GetName(field_info.Name.Replace('_', '-')));
                }
            }
        }
        static EName()
        {
            InitXNameStaticFields(XNamespace.None, typeof(EName));
            /* System.Diagnostics.Debug.WriteLine("== .cctor of sql.builder.DataApi.EName: start ==");
            System.Reflection.FieldInfo[] fields = typeof(sql.builder.DataApi.EName).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            System.Diagnostics.Debug.WriteLine(fields.Length.ToString() + " members");
            int non_standard = 0;
            for (int index = 0; index < fields.Length; index++) {
                System.Reflection.FieldInfo field_info = fields[index];
                XName name = (XName)field_info.GetValue(null);
                // .ToLower()
                if (name.LocalName != field_info.Name.Replace('_', '-')) {
                    System.Diagnostics.Debug.WriteLine("internal static readonly XName " + field_info.Name + " = XNamespace.None.GetName(\"" + name.LocalName + "\");");
                    non_standard++;
                }
            }
            System.Diagnostics.Debug.WriteLine(non_standard.ToString() + " non standard members");
            System.Diagnostics.Debug.WriteLine("== .cctor of sql.builder.DataApi.EName: end   =="); */
        }
    }
}
