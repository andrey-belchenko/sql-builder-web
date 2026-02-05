using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /// <summary>
    /// Набор имён используемых xml-атрибутов для использования вместо <see cref="TextConst.AName"/> и <see cref="TextConst.DsAName"/>
    /// </summary>
    public static class AName
    {
        public static readonly XName action_type;                 //= TextConst.AName.ActionType
        public static readonly XName agg;                         //= TextConst.AName.Agg;
        public static readonly XName allow_select_move_columns;   //= TextConst.AName.AllowSelectMoveColumns;
        public static readonly XName all_rows;                    //= TextConst.AName.AllRows;
        public static readonly XName @as;                         //= TextConst.AName.As;
        public static readonly XName assembly;
        public static readonly XName assigned;                    //= "assigned";
        public static readonly XName async;                       //= TextConst.AName.Async;
        public static readonly XName autobands;                   //= "autobands";
        public static readonly XName auto_check;                  //= TextConst.AName.AutoCheck;
        public static readonly XName auto_filter;                 //= TextConst.AName.AutoFilter;
        public static readonly XName auto_merge;                  //= TextConst.AName.AutoMerge;
        public static readonly XName auto_refresh;                //= TextConst.AName.AutoRefresh;
        public static readonly XName allow_save;                  //= TextConst.AName.AllowSave;
        public static readonly XName band_title;                  //= "band-title";
        public static readonly XName band_type;                   //= "band-type"; Значения: "dimband", "valband" или "band"
        public static readonly XName button_type;                 //= TextConst.AName.ButtonType;
        public static readonly XName calctree;                    //= TextConst.AName.CalculateTree;
        public static readonly XName call;                        //= TextConst.AName.Call;
        public static readonly XName can_be_checked;              //= TextConst.AName.CanBeChecked;
        public static readonly XName @checked;                    //= TextConst.AName.Checked;
        public static readonly XName @class;                      //= "class"
        public static readonly XName class_title;                 //= TextConst.AName.ClassTitle;
        public static readonly XName class_type;                  //= TextConst.AName.ClassType;
        public static readonly XName clear_on_list_change;        //= TextConst.AName.ClearOnListChange;
        public static readonly XName client_calc;                 //= TextConst.AName.ClientCalulation;
        public static readonly XName client_view;                 //= TextConst.AName.ClientView;
        public static readonly XName c_master;                    //= TextConst.AName.CMaster;
        public static readonly XName c_master_key;                //= TextConst.AName.CMasterKey;
        public static readonly XName color;                       //= TextConst.AName.Color;
        public static readonly XName colset;                      //= TextConst.AName.Colset;
        public static readonly XName column;                      //= TextConst.AName.Column;
        public static readonly XName column_editable;             //= TextConst.AName.ColumnEditable;
        public static readonly XName column_mandatory;            //= TextConst.AName.ColumnMandatory;
        public static readonly XName column_visible;              //= TextConst.AName.ColumnVisible;
        public static readonly XName columnpref;                  //= "columnpref";
        public static readonly XName comment;                     //= TextConst.AName.Comment;
        public static readonly XName condition;                   //= TextConst.AName.Condition
        // Для EName.UICommand значения см. TextConst.AVFormButtonType
        public static readonly XName control;                     //= TextConst.AName.Control;
        public static readonly XName control_name;                //= TextConst.AName.ControlName;
        public static readonly XName controlType;                 //= TextConst.AName.ControlType;
        public static readonly XName cumulate;                    //= TextConst.AName.Cumulate;
        public static readonly XName special_type;                //= TextConst.AName.SpecialType;
        public static readonly XName data_size;                   //= TextConst.AName.DataSize;
        public static readonly XName datareader;                  //= TextConst.AName.DataReader;
        public static readonly XName @default;                    //= TextConst.AName.Default;
        public static readonly XName defaulteditor;               //= "defaulteditor"
        //public static readonly XName datatype                  = TextConst.AName.DataType;  // значения см. TextConst.AVDataType.XXXX
        public static readonly XName delete_validation;           //= TextConst.AName.DeleteValidation;
        public static readonly XName dgroup;                      //= TextConst.AName.Dgroup
        public static readonly XName dimension;                   //= TextConst.AName.Dimension;
        public static readonly XName dimension_column;            //= TextConst.AName.DimensionColumn;
        public static readonly XName dimension_value;             //= TextConst.AName.DimensionValue;
        public static readonly XName dimname;                     //= TextConst.AName.Dimname;
        public static readonly XName directory;                   //= "directory";
        public static readonly XName dname;                       //= TextConst.AName.DName;
        public static readonly XName dont_push;                   //= TextConst.AName.DontPush;
        public static readonly XName dx_export;                   //= TextConst.AName.DxExport;
        public static readonly XName edit_mask;                   //= TextConst.AName.EditMask;
        public static readonly XName edit_columns;                //= TextConst.AName.EditColumns;
        public static readonly XName editable;                    //= TextConst.AName.Editable;
        public static readonly XName editor;                      //= "editor";
        public static readonly XName event_name;                  //= TextConst.EName.EventName;
        public static readonly XName excel_calc;                  //= TextConst.AName.ExcelCalulation;
        public static readonly XName exclude;                     //= TextConst.AName.Exclude;
        public static readonly XName expand_all;                  //= TextConst.AName.ExpandAll;
        public static readonly XName expanded;                    //= TextConst.AName.Expanded;
        //public static readonly XName Exists                  = TextConst.DsAName.Exists;
        public static readonly XName extend;                      //= TextConst.AName.Extend;
        public static readonly XName fact;                        //= TextConst.AName.Fact;
        public static readonly XName fact_dimension;              //= TextConst.AName.FactDimension;
        public static readonly XName field;                       //= TextConst.AName.Field;
        public static readonly XName src_field;                       //= TextConst.AName.Field;
        public static readonly XName file;                        //= TextConst.AName.File;
        public static readonly XName fill_height;                 //= TextConst.AName.FillHeight;
        public static readonly XName fixed_side;                  //= TextConst.AName.FixedSide;
        public static readonly XName folder;                      //= TextConst.AName.Folder;
        public static readonly XName font_color;                  //= TextConst.AName.FontColor;
        public static readonly XName form;                        //= TextConst.AName.Form;
        public static readonly XName format;                      //= TextConst.AName.Format;
        public static readonly XName function;                    //= TextConst.AName.Function;
        public static readonly XName group;                       //= TextConst.AName.Group;
        public static readonly XName halign;                      //= TextConst.AName.HAlign;
        public static readonly XName hint;                        //= TextConst.AName.Hint;
        public static readonly XName icon;                        //= TextConst.AName.Icon;
        public static readonly XName id;                          //= TextConst.AName.Id;
        public static readonly XName @if;                         //= TextConst.AName.If
        public static readonly XName index;                       //= TextConst.AName.Index;
        public static readonly XName info;                        //= "info";
        public static readonly XName invisible_in_column_chooser; //= TextConst.AName.InvisibleInColumnChooser;
        public static readonly XName inherit;                     //= TextConst.AName.Inherit;
        public static readonly XName ins_by_loop;                 //= TextConst.AName.InsByLoop
        public static readonly XName intern;                      //= TextConst.AName.Intern;
        public static readonly XName into;                        //= TextConst.AName.Into;
        public static readonly XName is_fact;                     //= TextConst.AName.IsFact;
        public static readonly XName is_fact_use;                 //= TextConst.AName.IsFactUse;
        public static readonly XName is_final_dimension;          //= TextConst.AName.IsFinalDimension
        public static readonly XName is_form;                     //= TextConst.AName.IsForm;
        public static readonly XName is_from_temp;                //= TextConst.ANameSpec.IsFromTemp;
        public static readonly XName is_join_col;                 //= TextConst.DsAName.IsJoinCol;
        public static readonly XName is_layout_block;             //= TextConst.AName.IsLayoutBlock;
        public static readonly XName is_ms_upd;                   //= TextConst.DsAName.IsMainSourceUpdateable;
        public static readonly XName is_private_dimension;        //= TextConst.AName.IsPrivateDimension;
        public static readonly XName is_refreshed;                //= TextConst.DsAName.IsRefreshed;
        public static readonly XName is_report;                   //= TextConst.AName.IsReport;
        public static readonly XName is_ret;                      //= TextConst.AName.IsRet;
        public static readonly XName is_top;                      //= TextConst.DsAName.IsTop;
        public static readonly XName is_updateable;               //= TextConst.DsAName.IsUpdateable;
        public static readonly XName is_updateable_ext;           //= TextConst.DsAName.IsUpdateableExt;
        public static readonly XName is_user_editable;            //= TextConst.DsAName.IsUserEditable;
        public static readonly XName is_vertical;                 // = TextConst.AName.IsVertical;
        public static readonly XName join;                        //= TextConst.AName.Join;
        public static readonly XName joinexp;                     //= TextConst.AName.JoinExp;
        public static readonly XName key;                         //= TextConst.AName.Key;
        public static readonly XName key_dimension;               //= TextConst.AName.KeyDimension;
        public static readonly XName key_name;                    //= TextConst.AName.KeyName;
        public static readonly XName link;                        //= TextConst.AName.Link
        public static readonly XName link_mp_point;               //= TextConst.AName.LinkMultiplicatePoint;
        public static readonly XName main;                        //= "main";
        public static readonly XName mandatory;                   //= TextConst.AName.Mandatory;
        public static readonly XName materialize;                 //= TextConst.AName.Materialize
        public static readonly XName max_length;                  //= TextConst.AName.MaxLength;
        public static readonly XName max_size;                    //= TextConst.AName.MaxSize;
        public static readonly XName min_size;                    //= TextConst.AName.MinSize;
        public static readonly XName merge_key;                   //= TextConst.AName.MergeKey;
        public static readonly XName merge_dimsets;               //= TextConst.AName.MergeDimsets;
        public static readonly XName message;                     //= TextConst.AName.Message;
        public static readonly XName mode;                        //= TextConst.AName.ViewMode 
        public static readonly XName multiple;                    //= TextConst.AName.Multiple;
        public static readonly XName multiplicate_point;          //= TextConst.AName.MultiplicatePoint;
        public static readonly XName mp;                          //= TextConst.AName.Multiplicer;
        public static readonly XName multi_select;                //= TextConst.AName.MultiSelect;
        public static readonly XName multi_select_column;          //= TextConst.DsAName.MultiselectColumn;
        public static readonly XName multi_select_target;          //= TextConst.DsAName.MultiselectTarget;
        public static readonly XName name;                        //= TextConst.AName.Name;
        public static readonly XName name_field_name;             //= TextConst.AName.NameFieldName;
        public static readonly XName navigator;                   //= TextConst.AName.Navigator;
        public static readonly XName new_rows_vis_for_other_tbls; //= TextConst.AName.NewRowsVisForOtherTbls;
        public static readonly XName new_val;                     //= TextConst.AName.NewVal;
        public static readonly XName noborder;                    //= TextConst.AName.NoBorder;
        public static readonly XName node_id;                     //= TextConst.AName.NodeId;
        public static readonly XName nogrid;                      //= TextConst.AName.NoGrid;
        public static readonly XName non_db;                      //= TextConst.DsAName.NonDb;
        public static readonly XName notification;                //= TextConst.AName.Notification;
        public static readonly XName nullif;                      //= TextConst.AName.NullIf;
        public static readonly XName null_as_undefined;           //= TextConst.AName.NullAsUndefined;
        public static readonly XName nvl;                         //= TextConst.AName.Nvl;
        public static readonly XName @object;                     //= TextConst.AName.Object;
        public static readonly XName only_force_refresh;          //= TextConst.AName.OnlyForceRefresh;
        public static readonly XName only_visible_refresh;        //= TextConst.AName.OnlyVisibleRefresh;
        public static readonly XName optional;                    //= TextConst.AName.Optional;
        public static readonly XName order;                       //= TextConst.AName.Order;
        public static readonly XName order_field_name;            //= TextConst.AName.OrderFieldName;
        public static readonly XName params_customization;        //= TextConst.AName.ParamsCustomization;
        public static readonly XName param_type;                  //= TextConst.AName.ParamType;
        public static readonly XName parent_node_id;              //= TextConst.AName.ParentNodeId;
        public static readonly XName parent_field_name;           //= TextConst.AName.ParentFieldName;
        public static readonly XName parent_key;                  //= TextConst.DsAName.ParentKey;
        public static readonly XName parent_table;                //= TextConst.DsAName.ParentTable;
        public static readonly XName parname;                     //= TextConst.AName.ParName;
        public static readonly XName part;                        //= TextConst.AName.Part;
        public static readonly XName part_id;                     //= TextConst.AName.PartId;
        public static readonly XName pivot;                       //= XNamespace.None.GetName("pivot");
        public static readonly XName position;                    //= TextConst.AName.Position;
        public static readonly XName post_process;                //= TextConst.AName.PostProcess;
        public static readonly XName prep_merge;                  //= TextConst.AName.PrepareMerge;
        public static readonly XName print_xlsx;                  //= TextConst.AName.PrintXlsx;
        public static readonly XName project;                     //= TextConst.AName.Project;
        public static readonly XName prompt;                      //= TextConst.AName.Prompt;
        public static readonly XName pth;                         //= TextConst.AName.Pth;
        public static readonly XName push;                        //= XNamespace.None.GetName("push");
        public static readonly XName ref_column;                  //= TextConst.DsAName.RefColumn;
        public static readonly XName removeable;                  //= TextConst.AName.Removeable
        public static readonly XName report;                      //= TextConst.AName.Report
        public static readonly XName rgb;                         //= TextConst.AName.Rgb;
        public static readonly XName rows_limit;                  //= TextConst.AName.RowsLimit;
        public static readonly XName quickview = XNamespace.None.GetName("qlikview");
        public static readonly XName qv_split = XNamespace.None.GetName("qv_split");
        public static readonly XName save_compiled;               //= TextConst.AName.SaveCompiled;
        public static readonly XName search_field_name;           //= TextConst.AName.SearchFieldName;
        public static readonly XName sections;                    //
        public static readonly XName security_id;                 //= TextConst.AName.SecurityId;
        // Не ошибка: TextConst.DsEName.SelListParentFieldName действительно используется как имя аттрибута
        public static readonly XName sel_list_parent_field_name;  //= TextConst.DsEName.SelListParentFieldName;
        // Значения uicommand@side - значения см. TextConst.AVSides
        public static readonly XName side;                        //= TextConst.AName.Side;
        public static readonly XName single_way;                  //= TextConst.AName.SingleWay;
        public static readonly XName size;                        //= TextConst.AName.Size;
        public static readonly XName show_agg_panel;              //= TextConst.AName.ShowAggPanel;
        public static readonly XName show_checkbox;               //= TextConst.AName.ShowCheckbox;
        public static readonly XName show_bottom_toolbar;         //= TextConst.AName.ShowBottomToolBar;
        public static readonly XName show_footer;                 //= TextConst.AName.ShowFooter;
        public static readonly XName show_nulls;                  //= TextConst.AName.ShowNulls;
        public static readonly XName show_toolbar;                //= TextConst.AName.ShowToolBar;
        public static readonly XName source_table;                //= TextConst.DsAName.SourceTable;
        public static readonly XName star_scheme;                 //= TextConst.AName.StarScheme;
        public static readonly XName step;                        //= TextConst.AName.Step;
        public static readonly XName sys;                         //= TextConst.AName.Sys;
        public static readonly XName table;                       //= TextConst.AName.Table;
        public static readonly XName target;                      //= TextConst.AName.Target;
        public static readonly XName temp_col_name;               //= TextConst.DsAName.TempColumnName;
        public static readonly XName template_name;               //= TextConst.AName.TemplateName
        public static readonly XName text_location;               //= TextConst.AName.TextLocation;
        public static readonly XName textsource;                  //= TextConst.AName.TextSource;
        public static readonly XName text_source_for;             //= TextConst.DsAName.TextSourceFor;
        public static readonly XName text_visible;                //= TextConst.AName.TextVisible;
        public static readonly XName title;                       //= TextConst.AName.Title;
        public static readonly XName time_type;                   //= TextConst.AName.TimeType;
        public static readonly XName timeline;                    //= TextConst.AName.Timeline;
        public static readonly XName timestamp;                   //= TextConst.AName.TimeStamp;
        public static readonly XName transposed;                  //= transposed;
        public static readonly XName txtype;                      //= "txtype";
        // Значения uicommand@type - enum DevExpress.XtraEditors.Controls.ButtonPredefines
        // Значения column@type - TextConst.AVDataType.XXXX
        public static readonly XName type;                        //= TextConst.AName.Type; //= TextConst.AName.DataType;  // значения см. TextConst.AVDataType.XXXX
        public static readonly XName type_name;
        public static readonly XName uncollapsible;               //= TextConst.AName.Uncollapsible;
        public static readonly XName union;                       //= "union";
        public static readonly XName updateable;                  //= TextConst.AName.Updateable;
        public static readonly XName update_target;               //= TextConst.AName.UpdateTarget;
        public static readonly XName used;                        //= "used";
        public static readonly XName use_flexcel;                 //= TextConst.AName.UseFlexCel;
        public static readonly XName use_repository;              //= TextConst.AName.UseRepository;
        public static readonly XName use_temp;                    //= TextConst.AName.UseTemp;
        public static readonly XName val_field_name;              //= TextConst.AName.ValFieldName;
        public static readonly XName valid;                       //= TextConst.AName.Valid;
        public static readonly XName value;                       //= TextConst.AName.Value;
        public static readonly XName value_column;                //= TextConst.AName.ValueColumn;
        public static readonly XName value_title;                 //= "value-title";
        public static readonly XName valuequery;                  //= TextConst.AName.ValueQuery;
        public static readonly XName view;                        //= TextConst.AName.View;
        public static readonly XName visible;                     //= TextConst.AName.Visible;
        public static readonly XName width;                       //= TextConst.AName.Width;
        public static readonly XName width_fixed;                 //= TextConst.AName.WidthFixed;
        public static readonly XName width_perc;                  //= TextConst.AName.WidthPerc;
        public static readonly XName window;                      //= TextConst.AName.Window;
        public static readonly XName with_behavior;               //= TextConst.AName.WithBehavior;
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
                    System.Diagnostics.Debug.WriteLine("public static readonly XName " + field_info.Name + " = XNamespace.None.GetName(\"" + name.LocalName + "\");");
                    non_standard++;
                }
            }
            System.Diagnostics.Debug.WriteLine(non_standard.ToString() + " non standard members");
            System.Diagnostics.Debug.WriteLine("== .cctor of sql.builder.DataApi.AName: end   ==");
            */
        }
    }
}
