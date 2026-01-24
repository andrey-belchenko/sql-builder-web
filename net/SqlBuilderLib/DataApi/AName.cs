using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /// <summary>
    /// Набор имён используемых xml-атрибутов для использования вместо <see cref="TextConst.AName"/> и <see cref="TextConst.DsAName"/>
    /// </summary>
    internal static class AName
    {
        internal static readonly XName action_type;                 //= TextConst.AName.ActionType
        internal static readonly XName agg;                         //= TextConst.AName.Agg;
        internal static readonly XName allow_select_move_columns;   //= TextConst.AName.AllowSelectMoveColumns;
        internal static readonly XName all_rows;                    //= TextConst.AName.AllRows;
        internal static readonly XName @as;                         //= TextConst.AName.As;
        internal static readonly XName assembly;
        internal static readonly XName assigned;                    //= "assigned";
        internal static readonly XName async;                       //= TextConst.AName.Async;
        internal static readonly XName autobands;                   //= "autobands";
        internal static readonly XName auto_check;                  //= TextConst.AName.AutoCheck;
        internal static readonly XName auto_filter;                 //= TextConst.AName.AutoFilter;
        internal static readonly XName auto_merge;                  //= TextConst.AName.AutoMerge;
        internal static readonly XName auto_refresh;                //= TextConst.AName.AutoRefresh;
        internal static readonly XName allow_save;                  //= TextConst.AName.AllowSave;
        internal static readonly XName band_title;                  //= "band-title";
        internal static readonly XName band_type;                   //= "band-type"; Значения: "dimband", "valband" или "band"
        internal static readonly XName button_type;                 //= TextConst.AName.ButtonType;
        internal static readonly XName calctree;                    //= TextConst.AName.CalculateTree;
        internal static readonly XName call;                        //= TextConst.AName.Call;
        internal static readonly XName can_be_checked;              //= TextConst.AName.CanBeChecked;
        internal static readonly XName @checked;                    //= TextConst.AName.Checked;
        internal static readonly XName @class;                      //= "class"
        internal static readonly XName class_title;                 //= TextConst.AName.ClassTitle;
        internal static readonly XName class_type;                  //= TextConst.AName.ClassType;
        internal static readonly XName clear_on_list_change;        //= TextConst.AName.ClearOnListChange;
        internal static readonly XName client_calc;                 //= TextConst.AName.ClientCalulation;
        internal static readonly XName client_view;                 //= TextConst.AName.ClientView;
        internal static readonly XName c_master;                    //= TextConst.AName.CMaster;
        internal static readonly XName c_master_key;                //= TextConst.AName.CMasterKey;
        internal static readonly XName color;                       //= TextConst.AName.Color;
        internal static readonly XName colset;                      //= TextConst.AName.Colset;
        internal static readonly XName column;                      //= TextConst.AName.Column;
        internal static readonly XName column_editable;             //= TextConst.AName.ColumnEditable;
        internal static readonly XName column_mandatory;            //= TextConst.AName.ColumnMandatory;
        internal static readonly XName column_visible;              //= TextConst.AName.ColumnVisible;
        internal static readonly XName columnpref;                  //= "columnpref";
        internal static readonly XName comment;                     //= TextConst.AName.Comment;
        internal static readonly XName condition;                   //= TextConst.AName.Condition
        // Для EName.UICommand значения см. TextConst.AVFormButtonType
        internal static readonly XName control;                     //= TextConst.AName.Control;
        internal static readonly XName control_name;                //= TextConst.AName.ControlName;
        internal static readonly XName controlType;                 //= TextConst.AName.ControlType;
        internal static readonly XName cumulate;                    //= TextConst.AName.Cumulate;
        internal static readonly XName special_type;                //= TextConst.AName.SpecialType;
        internal static readonly XName data_size;                   //= TextConst.AName.DataSize;
        internal static readonly XName datareader;                  //= TextConst.AName.DataReader;
        internal static readonly XName @default;                    //= TextConst.AName.Default;
        internal static readonly XName defaulteditor;               //= "defaulteditor"
        //internal static readonly XName datatype                  = TextConst.AName.DataType;  // значения см. TextConst.AVDataType.XXXX
        internal static readonly XName delete_validation;           //= TextConst.AName.DeleteValidation;
        internal static readonly XName dgroup;                      //= TextConst.AName.Dgroup
        internal static readonly XName dimension;                   //= TextConst.AName.Dimension;
        internal static readonly XName dimension_column;            //= TextConst.AName.DimensionColumn;
        internal static readonly XName dimension_value;             //= TextConst.AName.DimensionValue;
        internal static readonly XName dimname;                     //= TextConst.AName.Dimname;
        internal static readonly XName directory;                   //= "directory";
        internal static readonly XName dname;                       //= TextConst.AName.DName;
        internal static readonly XName dont_push;                   //= TextConst.AName.DontPush;
        internal static readonly XName dx_export;                   //= TextConst.AName.DxExport;
        internal static readonly XName edit_mask;                   //= TextConst.AName.EditMask;
        internal static readonly XName edit_columns;                //= TextConst.AName.EditColumns;
        internal static readonly XName editable;                    //= TextConst.AName.Editable;
        internal static readonly XName editor;                      //= "editor";
        internal static readonly XName event_name;                  //= TextConst.EName.EventName;
        internal static readonly XName excel_calc;                  //= TextConst.AName.ExcelCalulation;
        internal static readonly XName exclude;                     //= TextConst.AName.Exclude;
        internal static readonly XName expand_all;                  //= TextConst.AName.ExpandAll;
        internal static readonly XName expanded;                    //= TextConst.AName.Expanded;
        //internal static readonly XName Exists                  = TextConst.DsAName.Exists;
        internal static readonly XName extend;                      //= TextConst.AName.Extend;
        internal static readonly XName fact;                        //= TextConst.AName.Fact;
        internal static readonly XName fact_dimension;              //= TextConst.AName.FactDimension;
        internal static readonly XName field;                       //= TextConst.AName.Field;
        internal static readonly XName file;                        //= TextConst.AName.File;
        internal static readonly XName fill_height;                 //= TextConst.AName.FillHeight;
        internal static readonly XName fixed_side;                  //= TextConst.AName.FixedSide;
        internal static readonly XName folder;                      //= TextConst.AName.Folder;
        internal static readonly XName font_color;                  //= TextConst.AName.FontColor;
        internal static readonly XName form;                        //= TextConst.AName.Form;
        internal static readonly XName format;                      //= TextConst.AName.Format;
        internal static readonly XName function;                    //= TextConst.AName.Function;
        internal static readonly XName group;                       //= TextConst.AName.Group;
        internal static readonly XName halign;                      //= TextConst.AName.HAlign;
        internal static readonly XName hint;                        //= TextConst.AName.Hint;
        internal static readonly XName icon;                        //= TextConst.AName.Icon;
        internal static readonly XName id;                          //= TextConst.AName.Id;
        internal static readonly XName @if;                         //= TextConst.AName.If
        internal static readonly XName index;                       //= TextConst.AName.Index;
        internal static readonly XName info;                        //= "info";
        internal static readonly XName invisible_in_column_chooser; //= TextConst.AName.InvisibleInColumnChooser;
        internal static readonly XName inherit;                     //= TextConst.AName.Inherit;
        internal static readonly XName ins_by_loop;                 //= TextConst.AName.InsByLoop
        internal static readonly XName intern;                      //= TextConst.AName.Intern;
        internal static readonly XName into;                        //= TextConst.AName.Into;
        internal static readonly XName is_fact;                     //= TextConst.AName.IsFact;
        internal static readonly XName is_fact_use;                 //= TextConst.AName.IsFactUse;
        internal static readonly XName is_final_dimension;          //= TextConst.AName.IsFinalDimension
        internal static readonly XName is_form;                     //= TextConst.AName.IsForm;
        internal static readonly XName is_from_temp;                //= TextConst.ANameSpec.IsFromTemp;
        internal static readonly XName is_join_col;                 //= TextConst.DsAName.IsJoinCol;
        internal static readonly XName is_layout_block;             //= TextConst.AName.IsLayoutBlock;
        internal static readonly XName is_ms_upd;                   //= TextConst.DsAName.IsMainSourceUpdateable;
        internal static readonly XName is_private_dimension;        //= TextConst.AName.IsPrivateDimension;
        internal static readonly XName is_refreshed;                //= TextConst.DsAName.IsRefreshed;
        internal static readonly XName is_report;                   //= TextConst.AName.IsReport;
        internal static readonly XName is_ret;                      //= TextConst.AName.IsRet;
        internal static readonly XName is_top;                      //= TextConst.DsAName.IsTop;
        internal static readonly XName is_updateable;               //= TextConst.DsAName.IsUpdateable;
        internal static readonly XName is_updateable_ext;           //= TextConst.DsAName.IsUpdateableExt;
        internal static readonly XName is_user_editable;            //= TextConst.DsAName.IsUserEditable;
        internal static readonly XName is_vertical;                 // = TextConst.AName.IsVertical;
        internal static readonly XName join;                        //= TextConst.AName.Join;
        internal static readonly XName joinexp;                     //= TextConst.AName.JoinExp;
        internal static readonly XName key;                         //= TextConst.AName.Key;
        internal static readonly XName key_dimension;               //= TextConst.AName.KeyDimension;
        internal static readonly XName key_name;                    //= TextConst.AName.KeyName;
        internal static readonly XName link;                        //= TextConst.AName.Link
        internal static readonly XName link_mp_point;               //= TextConst.AName.LinkMultiplicatePoint;
        internal static readonly XName main;                        //= "main";
        internal static readonly XName mandatory;                   //= TextConst.AName.Mandatory;
        internal static readonly XName materialize;                 //= TextConst.AName.Materialize
        internal static readonly XName max_length;                  //= TextConst.AName.MaxLength;
        internal static readonly XName max_size;                    //= TextConst.AName.MaxSize;
        internal static readonly XName min_size;                    //= TextConst.AName.MinSize;
        internal static readonly XName merge_key;                   //= TextConst.AName.MergeKey;
        internal static readonly XName merge_dimsets;               //= TextConst.AName.MergeDimsets;
        internal static readonly XName message;                     //= TextConst.AName.Message;
        internal static readonly XName mode;                        //= TextConst.AName.ViewMode 
        internal static readonly XName multiple;                    //= TextConst.AName.Multiple;
        internal static readonly XName multiplicate_point;          //= TextConst.AName.MultiplicatePoint;
        internal static readonly XName mp;                          //= TextConst.AName.Multiplicer;
        internal static readonly XName multi_select;                //= TextConst.AName.MultiSelect;
        internal static readonly XName multi_select_column;          //= TextConst.DsAName.MultiselectColumn;
        internal static readonly XName multi_select_target;          //= TextConst.DsAName.MultiselectTarget;
        internal static readonly XName name;                        //= TextConst.AName.Name;
        internal static readonly XName name_field_name;             //= TextConst.AName.NameFieldName;
        internal static readonly XName navigator;                   //= TextConst.AName.Navigator;
        internal static readonly XName new_rows_vis_for_other_tbls; //= TextConst.AName.NewRowsVisForOtherTbls;
        internal static readonly XName new_val;                     //= TextConst.AName.NewVal;
        internal static readonly XName noborder;                    //= TextConst.AName.NoBorder;
        internal static readonly XName node_id;                     //= TextConst.AName.NodeId;
        internal static readonly XName nogrid;                      //= TextConst.AName.NoGrid;
        internal static readonly XName non_db;                      //= TextConst.DsAName.NonDb;
        internal static readonly XName notification;                //= TextConst.AName.Notification;
        internal static readonly XName nullif;                      //= TextConst.AName.NullIf;
        internal static readonly XName null_as_undefined;           //= TextConst.AName.NullAsUndefined;
        internal static readonly XName nvl;                         //= TextConst.AName.Nvl;
        internal static readonly XName @object;                     //= TextConst.AName.Object;
        internal static readonly XName only_force_refresh;          //= TextConst.AName.OnlyForceRefresh;
        internal static readonly XName only_visible_refresh;        //= TextConst.AName.OnlyVisibleRefresh;
        internal static readonly XName optional;                    //= TextConst.AName.Optional;
        internal static readonly XName order;                       //= TextConst.AName.Order;
        internal static readonly XName order_field_name;            //= TextConst.AName.OrderFieldName;
        internal static readonly XName params_customization;        //= TextConst.AName.ParamsCustomization;
        internal static readonly XName param_type;                  //= TextConst.AName.ParamType;
        internal static readonly XName parent_node_id;              //= TextConst.AName.ParentNodeId;
        internal static readonly XName parent_field_name;           //= TextConst.AName.ParentFieldName;
        internal static readonly XName parent_key;                  //= TextConst.DsAName.ParentKey;
        internal static readonly XName parent_table;                //= TextConst.DsAName.ParentTable;
        internal static readonly XName parname;                     //= TextConst.AName.ParName;
        internal static readonly XName part;                        //= TextConst.AName.Part;
        internal static readonly XName part_id;                     //= TextConst.AName.PartId;
        internal static readonly XName pivot;                       //= XNamespace.None.GetName("pivot");
        internal static readonly XName position;                    //= TextConst.AName.Position;
        internal static readonly XName post_process;                //= TextConst.AName.PostProcess;
        internal static readonly XName prep_merge;                  //= TextConst.AName.PrepareMerge;
        internal static readonly XName print_xlsx;                  //= TextConst.AName.PrintXlsx;
        internal static readonly XName project;                     //= TextConst.AName.Project;
        internal static readonly XName prompt;                      //= TextConst.AName.Prompt;
        internal static readonly XName pth;                         //= TextConst.AName.Pth;
        internal static readonly XName push;                        //= XNamespace.None.GetName("push");
        internal static readonly XName ref_column;                  //= TextConst.DsAName.RefColumn;
        internal static readonly XName removeable;                  //= TextConst.AName.Removeable
        internal static readonly XName report;                      //= TextConst.AName.Report
        internal static readonly XName rgb;                         //= TextConst.AName.Rgb;
        internal static readonly XName rows_limit;                  //= TextConst.AName.RowsLimit;
        internal static readonly XName quickview = XNamespace.None.GetName("qlikview");
        internal static readonly XName qv_split = XNamespace.None.GetName("qv_split");
        internal static readonly XName save_compiled;               //= TextConst.AName.SaveCompiled;
        internal static readonly XName search_field_name;           //= TextConst.AName.SearchFieldName;
        internal static readonly XName sections;                    //
        internal static readonly XName security_id;                 //= TextConst.AName.SecurityId;
        // Не ошибка: TextConst.DsEName.SelListParentFieldName действительно используется как имя аттрибута
        internal static readonly XName sel_list_parent_field_name;  //= TextConst.DsEName.SelListParentFieldName;
        // Значения uicommand@side - значения см. TextConst.AVSides
        internal static readonly XName side;                        //= TextConst.AName.Side;
        internal static readonly XName single_way;                  //= TextConst.AName.SingleWay;
        internal static readonly XName size;                        //= TextConst.AName.Size;
        internal static readonly XName show_agg_panel;              //= TextConst.AName.ShowAggPanel;
        internal static readonly XName show_checkbox;               //= TextConst.AName.ShowCheckbox;
        internal static readonly XName show_bottom_toolbar;         //= TextConst.AName.ShowBottomToolBar;
        internal static readonly XName show_footer;                 //= TextConst.AName.ShowFooter;
        internal static readonly XName show_nulls;                  //= TextConst.AName.ShowNulls;
        internal static readonly XName show_toolbar;                //= TextConst.AName.ShowToolBar;
        internal static readonly XName source_table;                //= TextConst.DsAName.SourceTable;
        internal static readonly XName star_scheme;                 //= TextConst.AName.StarScheme;
        internal static readonly XName step;                        //= TextConst.AName.Step;
        internal static readonly XName sys;                         //= TextConst.AName.Sys;
        internal static readonly XName table;                       //= TextConst.AName.Table;
        internal static readonly XName target;                      //= TextConst.AName.Target;
        internal static readonly XName temp_col_name;               //= TextConst.DsAName.TempColumnName;
        internal static readonly XName template_name;               //= TextConst.AName.TemplateName
        internal static readonly XName text_location;               //= TextConst.AName.TextLocation;
        internal static readonly XName textsource;                  //= TextConst.AName.TextSource;
        internal static readonly XName text_source_for;             //= TextConst.DsAName.TextSourceFor;
        internal static readonly XName text_visible;                //= TextConst.AName.TextVisible;
        internal static readonly XName title;                       //= TextConst.AName.Title;
        internal static readonly XName time_type;                   //= TextConst.AName.TimeType;
        internal static readonly XName timeline;                    //= TextConst.AName.Timeline;
        internal static readonly XName timestamp;                   //= TextConst.AName.TimeStamp;
        internal static readonly XName transposed;                  //= transposed;
        internal static readonly XName txtype;                      //= "txtype";
        // Значения uicommand@type - enum DevExpress.XtraEditors.Controls.ButtonPredefines
        // Значения column@type - TextConst.AVDataType.XXXX
        internal static readonly XName type;                        //= TextConst.AName.Type; //= TextConst.AName.DataType;  // значения см. TextConst.AVDataType.XXXX
        internal static readonly XName type_name;
        internal static readonly XName uncollapsible;               //= TextConst.AName.Uncollapsible;
        internal static readonly XName union;                       //= "union";
        internal static readonly XName updateable;                  //= TextConst.AName.Updateable;
        internal static readonly XName update_target;               //= TextConst.AName.UpdateTarget;
        internal static readonly XName used;                        //= "used";
        internal static readonly XName use_flexcel;                 //= TextConst.AName.UseFlexCel;
        internal static readonly XName use_repository;              //= TextConst.AName.UseRepository;
        internal static readonly XName use_temp;                    //= TextConst.AName.UseTemp;
        internal static readonly XName val_field_name;              //= TextConst.AName.ValFieldName;
        internal static readonly XName valid;                       //= TextConst.AName.Valid;
        internal static readonly XName value;                       //= TextConst.AName.Value;
        internal static readonly XName value_column;                //= TextConst.AName.ValueColumn;
        internal static readonly XName value_title;                 //= "value-title";
        internal static readonly XName valuequery;                  //= TextConst.AName.ValueQuery;
        internal static readonly XName view;                        //= TextConst.AName.View;
        internal static readonly XName visible;                     //= TextConst.AName.Visible;
        internal static readonly XName width;                       //= TextConst.AName.Width;
        internal static readonly XName width_fixed;                 //= TextConst.AName.WidthFixed;
        internal static readonly XName width_perc;                  //= TextConst.AName.WidthPerc;
        internal static readonly XName window;                      //= TextConst.AName.Window;
        internal static readonly XName with_behavior;               //= TextConst.AName.WithBehavior;
        static AName()
        {
            EName.InitXNameStaticFields(XNamespace.None, typeof(AName));
            /*System.Diagnostics.Debug.WriteLine("== .cctor of sql.builder.DataApi.AName: start ==");
            System.Reflection.FieldInfo[] fields = typeof(sql.builder.DataApi.AName).GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            System.Diagnostics.Debug.WriteLine(fields.Length.ToString() + " members");
            int non_standard = 0;
            for (int index = 0; index < fields.Length; index++) {
                System.Reflection.FieldInfo field_info = fields[index];
                XName name = (XName)field_info.GetValue(null);
                if (name.LocalName != field_info.Name.Replace('_', '-')) {
                    System.Diagnostics.Debug.WriteLine("internal static readonly XName " + field_info.Name + " = XNamespace.None.GetName(\"" + name.LocalName + "\");");
                    non_standard++;
                }
            }
            System.Diagnostics.Debug.WriteLine(non_standard.ToString() + " non standard members");
            System.Diagnostics.Debug.WriteLine("== .cctor of sql.builder.DataApi.AName: end   ==");
            */
        }
    }
}
