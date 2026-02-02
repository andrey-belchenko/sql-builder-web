using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /// <summary>
    /// Набор имён используемых xml-тегов для использования вместо <see cref="TextConst.EName"/> и <see cref="TextConst.DsEName"/>
    /// </summary>
    public static class EName
    {
        public static readonly XName action;               //= TextConst.EName.Action;
        public static readonly XName actions;              //= TextConst.EName.Actions;
        public static readonly XName addition;             //= TextConst.EName.Addition;
        public static readonly XName additions;            //= TextConst.EName.Additions;
        public static readonly XName array;                //= TextConst.EName.Array;
        public static readonly XName band;                 //= TextConst.EName.Band;
        public static readonly XName button_type;          //= TextConst.EName.ButtonType;
        public static readonly XName button_types;         //= TextConst.EName.ButtonTypes;
        public static readonly XName buttons;              //= TextConst.EName.Buttons;
        public static readonly XName call;                 //= TextConst.EName.Call;
        public static readonly XName cells;                //= TextConst.EName.Cells;
        public static readonly XName childs;               //= TextConst.EName.Childs;
        public static readonly XName clear_temp_text;      //= TextConst.DsEName.ClearTempText;
        public static readonly XName col_dim_val;          //= TextConst.EName.ColDimVal;
        public static readonly XName color;                //= TextConst.EName.Color;
        public static readonly XName color_package;        //= TextConst.EName.ColorPackage;
        public static readonly XName color_packages;       //= TextConst.EName.ColorPackages;
        public static readonly XName column;               //= TextConst.EName.Column;
        public static readonly XName columnspreset;        //= TextConst.EName.ColumnsPreset
        public static readonly XName columns;              //= TextConst.EName.Columns;
        public static readonly XName compiled;             //= TextConst.EName.Compiled;
        public static readonly XName @const;               //= TextConst.EName.Const;
        public static readonly XName content;              //= TextConst.EName.Content;
        public static readonly XName connect;              //= TextConst.EName.Connect;
        public static readonly XName customer;             //= TextConst.EName.Customer;
        public static readonly XName customers;            //= TextConst.EName.Customers;
        public static readonly XName data;                 //= TextConst.EName.Data;
        public static readonly XName dataset;              //= TextConst.DsEName.DataSet;
        public static readonly XName datatype;             //= "datatype";
        public static readonly XName datatypes;            //= "datatypes";
        public static readonly XName defaultquery;         //= TextConst.EName.DefaultQuery;
        public static readonly XName delete_text;          //= TextConst.DsEName.DeleteText;
        public static readonly XName dependant;            //= TextConst.DsEName.Dependant;
        public static readonly XName dependants;           //= TextConst.DsEName.Dependants;
        public static readonly XName dep_refresh_cmd;      //= TextConst.DsEName.DepRefreshCommand;
        public static readonly XName dimension;            //= TextConst.EName.Dimension;
        public static readonly XName dimension_сolumns;    //= "dimension-сolumns";
        public static readonly XName dimension_package;    //= TextConst.EName.DimensionPackage;
        public static readonly XName dimension_packages;   //= TextConst.EName.DimensionPackages;
        public static readonly XName dimension_values;     //= TextConst.EName.DimensionValues;
        public static readonly XName dimlink;              //= TextConst.EName.DimLink;
        public static readonly XName dimset;               //= TextConst.EName.DimSet;
        public static readonly XName dlink;                //= TextConst.EName.DLink;
        public static readonly XName elink;                //= TextConst.EName.ELink;
        public static readonly XName empty_item;           //= TextConst.EName.EmptyItem;
        public static readonly XName events;               //= TextConst.EName.Events;
        public static readonly XName excel;                //= TextConst.EName.Excel;
        //public static readonly XName excel_templates;    //= TextConst.EName.ExcelTemplates;
        public static readonly XName expression_package;   //= TextConst.EName.ExpressionPackage;
        public static readonly XName expression_packages;  //= TextConst.EName.ExpressionPackages;
        public static readonly XName expressions;          //= TextConst.EName.Expressions;
        public static readonly XName extendlinks;          //= TextConst.EName.ExtendLinks;
        public static readonly XName extendwhere;          //= TextConst.EName.ExtendWhere;
        public static readonly XName extra_key;            //= TextConst.DsEName.ExtraKey;
        public static readonly XName fact;                 //= TextConst.EName.Fact;
        public static readonly XName factlinks;            //= TextConst.EName.Factlinks;
        public static readonly XName field;                //= TextConst.EName.Field;
        public static readonly XName fieldgroup;           //= TextConst.EName.FieldGroup;
        public static readonly XName fields;               //= TextConst.EName.Fields;
        public static readonly XName folders;              //= TextConst.EName.Folders;
        public static readonly XName folder;               //= TextConst.EName.Folder;
        public static readonly XName form;                 //= TextConst.EName.Form;
        public static readonly XName format;               //= TextConst.EName.Format;
        public static readonly XName format_package;       //= TextConst.EName.FormatPackage;
        public static readonly XName format_packages;      //= TextConst.EName.FormatPackages;
        public static readonly XName forms;                //= TextConst.EName.Forms;
        public static readonly XName from;                 //= TextConst.EName.From;
        public static readonly XName function;             //= TextConst.EName.Function;
        public static readonly XName functions;            //= TextConst.EName.Functions;
        public static readonly XName globalparams;         //= TextConst.EName.GlobalParams;
        public static readonly XName grid;                 //= TextConst.EName.Grid;
        public static readonly XName group;                //= TextConst.EName.Group;
        public static readonly XName grouping;             //= TextConst.EName.Grouping;
        public static readonly XName grset;                //= TextConst.EName.Grset;
        public static readonly XName having;               //= TextConst.EName.Having;
        public static readonly XName @if;                  //= TextConst.EName.If;
        public static readonly XName insert;               //= "insert";
        public static readonly XName insert_text;          //= TextConst.DsEName.InsertText;
        public static readonly XName joinon;               //= "joinon";
        public static readonly XName label;                //= TextConst.EName.Label;
        public static readonly XName link;                 //= TextConst.EName.Link;
        public static readonly XName links;                //= TextConst.EName.Links;
        public static readonly XName listquery;            //= TextConst.EName.ListQuery;
        public static readonly XName menu;                 //= TextConst.EName.Menu;
        public static readonly XName measures;             //= TextConst.EName.Measures;
        public static readonly XName multireference;       //= TextConst.EName.Multireference;
        public static readonly XName navigator;            //= TextConst.EName.Navigator;
        public static readonly XName navigators;           //= TextConst.EName.Navigators;
        public static readonly XName qube;                 //= TextConst.EName.Qube;
        public static readonly XName queries;              //= TextConst.EName.Queries;
        public static readonly XName query;                //= TextConst.EName.Query;
        public static readonly XName param;                //= TextConst.EName.Param;
        public static readonly XName @params;              //= TextConst.EName.Params;
        public static readonly XName part;                 //= TextConst.EName.Part;
        public static readonly XName parts;                //= TextConst.EName.Parts;
        public static readonly XName pivot;                //= TextConst.EName.Pivot;
        public static readonly XName print_templates;      //= TextConst.EName.PrintTemplates
        public static readonly XName proc_text;            //= TextConst.DsEName.ProcText;
        public static readonly XName procedure;            //= TextConst.EName.ReportProc;
        public static readonly XName project;              //= TextConst.EName.Project;
        public static readonly XName projects;             //= TextConst.EName.Projects;
        public static readonly XName push;                 //= TextConst.EName.Push;
        public static readonly XName reference;            //= TextConst.EName.Reference;
        public static readonly XName references;           //= TextConst.EName.References;
        public static readonly XName report;               //= TextConst.EName.Report;
        public static readonly XName reports;              //= TextConst.EName.Reports;
        public static readonly XName role;                 //= TextConst.EName.Role;
        public static readonly XName root;                 //= TextConst.EName.Root;
        public static readonly XName rowactions;           //= TextConst.EName.Rowactions;
        public static readonly XName scope;                  //= TextConst.EName.Scope;
        public static readonly XName scheme;                 //= TextConst.EName.Scheme;
        public static readonly XName scrollarea;             //= TextConst.EName.ScrollArea;
        public static readonly XName section;                //= TextConst.EName.Section;
        public static readonly XName select;                 //= TextConst.EName.Select;
        public static readonly XName select_text;            //= TextConst.DsEName.SelectText;
        public static readonly XName sel_list_cl_fact_pars;  //= TextConst.DsEName.SelListClFactPars;
        public static readonly XName sel_list_compiled;      //= TextConst.DsEName.SelListCompiled;
        public static readonly XName sel_list_pars;          //= TextConst.DsEName.SelListPars;
        public static readonly XName sel_list_report;        //= TextConst.DsEName.SelListReport;
        public static readonly XName security_package;       //= TextConst.EName.SecurityPackage;
        public static readonly XName single_row_refresh_cmd; //= TextConst.DsEName.SingleRowRefreshCmd;
        public static readonly XName slink;                  //= TextConst.EName.SLink;
        public static readonly XName sourcelink;             //= TextConst.EName.SourceLink;
        public static readonly XName splitcontainer;         //= TextConst.EName.SplitContainer;
        public static readonly XName splitter;               //= TextConst.EName.Splitter;
        public static readonly XName start;                  //= TextConst.EName.Start;
        public static readonly XName tabcontainer;           //= TextConst.EName.TabContainer;
        public static readonly XName table;                  //= TextConst.EName.Table;
        public static readonly XName template;               //= TextConst.EName.Template
        public static readonly XName text;                   //= TextConst.EName.Text;
        public static readonly XName toolbar;                //= TextConst.EName.Toolbar;
        public static readonly XName td;                     //= TextConst.EName.Td;
        public static readonly XName tr;                     //= TextConst.EName.Tr;
        public static readonly XName transpose;              //= "transpose";
        public static readonly XName uicommand;              //= TextConst.EName.UICommand;
        public static readonly XName undefined;              //= TextConst.EName.Undefined;
        public static readonly XName union;                  //= TextConst.EName.Union;
        public static readonly XName update_temp_text;       //= TextConst.DsEName.UptadeTempText;
        public static readonly XName update_text;            //= TextConst.DsEName.UpdateText;
        public static readonly XName use_color;              //= TextConst.EName.UseColor;
        public static readonly XName use_object;             //= TextConst.EName.UseObject;
        public static readonly XName use_role;               //= TextConst.EName.UseRole;
        public static readonly XName useaction;              //= TextConst.EName.UseAction;
        public static readonly XName usefield;               //= TextConst.EName.UseField;
        public static readonly XName useform;                //= TextConst.EName.UseForm;
        public static readonly XName useglobparam;           //= TextConst.EName.UseGlobParam;
        public static readonly XName useparam;               //= TextConst.EName.UseParam;
        public static readonly XName usepart;                //= TextConst.EName.UsePart;
        public static readonly XName usereport;              //= TextConst.EName.UseReport;
        public static readonly XName @using;                 //= TextConst.EName.Using;
        public static readonly XName val;                    //= TextConst.EName.Val;
        public static readonly XName value_сolumns;          //= "value-сolumns";
        public static readonly XName value_refresh_cmd;      //= TextConst.DsEName.ValueRefreshCmd;
        public static readonly XName value_reset_cmd;        //= TextConst.DsEName.ValueResetCmd;
        public static readonly XName values;                 //= "values";
        public static readonly XName view;                   //= "view";
        public static readonly XName viewcolumns;            //= TextConst.EName.ViewColumns;
        public static readonly XName views;                  //= "views"
        public static readonly XName withparams;             //= TextConst.EName.WithParams;
        public static readonly XName where;                  //= TextConst.EName.Where;
        public static readonly XName word;                   //= TextConst.EName.Word;
        //
        public static void InitXNameStaticFields(XNamespace ns, Type type)
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
                    System.Diagnostics.Debug.WriteLine("public static readonly XName " + field_info.Name + " = XNamespace.None.GetName(\"" + name.LocalName + "\");");
                    non_standard++;
                }
            }
            System.Diagnostics.Debug.WriteLine(non_standard.ToString() + " non standard members");
            System.Diagnostics.Debug.WriteLine("== .cctor of sql.builder.DataApi.EName: end   =="); */
        }
    }
}
