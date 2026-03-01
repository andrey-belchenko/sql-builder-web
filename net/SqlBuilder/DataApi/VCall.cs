using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public class VCall : VSXElement, IVParent
    {
        public VCall()
            : base(EName.call)
        {
        }
        protected override bool ChildDependant { get { return true; } }
        public VFunction Function()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as VFunction);
            }
            VFunction f = XmlReports.Environment.GetFunction(this.AttrOrEmpty(AName_.function));
            AddCashValue(f, MethodBase.GetCurrentMethod().ToString(), null);
            return f;
        }
        public override bool IsElementUser()
        {
            return true;
        }
        public override List<VSXElement> GetUsedElements()
        {
            var list = new List<VSXElement>();
            VFunction fnc = this.Function();
            if (fnc != null)
            {
                list.Add(fnc);
            }
            return list;
        }
        public override string XDataType()
        {
            string s = this.DataType();
            if (s == string.Empty)
            {
                VFunction func = this.Function();
                s = func.DataType();
                if (s == TextConst.AVDataType.Variant)
                {
                    foreach (VSXElement factPar in this.GetElementsP())
                    {
                        string s1 = factPar.XDataType();
                        if (s1 == TextConst.AVDataType.Date || s1 == TextConst.AVDataType.Number || s1 == TextConst.AVDataType.String || s1 == TextConst.AVDataType.Clob || s1 == TextConst.AVDataType.Blob)
                        {
                            s = s1;
                            break;
                        }
                    }
                }
            }
            return s;
        }
        public override List<VColumn> UsedColumns()
        {
            return this.GetDescedantsP(EName.column).Cast<VColumn>().ToList();
        }
        public override void LookUpNextSources(List<VSXElement> list, VLookupAnalyzer analyzer)
        {
            if (!analyzer.CheckAndReturn(list, this))
            {
                return;
            }
            IList<VSXElement> els = this.GetElementsP();
            foreach (VSXElement el in els)
            {
                el.LookUpNextSources(list, analyzer);
            }
        }
        private static string[] child_nodes = { TextConst.EName.Call, TextConst.EName.Column, TextConst.EName.Const, TextConst.EName.Array, TextConst.EName.UseParam, TextConst.EName.UseGlobParam, TextConst.EName.Query, TextConst.EName.Fact, TextConst.EName.UseColor, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region Свойства
        #region Alias

        public override bool P_Alias_Exists()
        {

            return true;

        }

        #endregion
        #region IsListColumn
        public override bool P_IsNameColumn_Exists()
        {

            return true;

        }
        #endregion
        #region AutoFilter
        public override bool P_AutoFilter_Exists()
        {

            return GetParent() is VOutputElement;

        }
        #endregion
        #region IsListColumn
        public override bool P_IsListColumn_Exists()
        {

            return true;

        }
        #endregion
        #region NodeText

        public override string GetNodeInfo()
        {
            return GetNodeOtherInfo();
        }

        public override string GetNodeOtherInfo()
        {

            string s;
            if (TreeNodeExpandedWithParents())
            {
                s = ColorGray(Bold(P_Function));
            }
            else
            {
                s = GetCallInfo();
            }
            var mp = P_Multiplicer;
            if (mp != "")
            {
                s += "*10" + Sup(mp);
            }

            var ss = "";



            if (P_Fact != "" && !(GetParent() is VExpressionPackage) && !(GetParent() is VExpressions))
            {
                ss = " fact:" + Bold(P_Fact) + "";


            }
            else
            {

            }
            string alias = this.AttrOrEmpty(AName_.@as);
            if (!string.IsNullOrEmpty(alias))
            {
                s += " as " + ColorBrown(Bold(alias));
            }
            s += ss;

            if (P_Dimension != "")
            {
                s += " dim " + Bold(P_Dimension) + "";
            }
            if (GetParent() is VOutputElement)
            {
                var p = P_ParName;
                if (p != "")
                {
                    s += Bold(" :" + p);
                }
            }
            if (P_Title_Exists())
            {


                var t = P_Title;
                if (t != P_SelfTitle)
                {
                    t = ColorGray(t);
                }
                s += " " + Italic(t);
            }

            var g = P_Group;
            if (P_Group != "")
            {
                if (g == "1")
                {
                    g = "group";
                }
                s = " " + ColorGroup(Italic(g)) + " " + s;
            }




            if (P_Optional == TextConst.AVBool.True || P_UseOnlyWithOther == TextConst.AVBool.True /*|| P_ExcludeIfSet == TextConst.AVBool.True*/)
            {
                s = "{" + s + "}";
            }

            if (P_Key != "")
            {
                s += " " + ColorGold("pk");
            }

            return s;
        }
        private string GetCallInfo()
        {
            VFunction f = this.Function();
            string text = f.AttrOrEmpty("text");
            if (string.IsNullOrEmpty(text))
            {
                return this.GetCallInfoByStructure();
            }
            else if (text == "*")
            {
                return this.GetCallInfoByText(f.MakeText());
            }
            else
            {
                return this.GetCallInfoByText(text);
            }
        }
        private string GetCallInfoByStructure()
        {
            string s = Bold(this.AttrOrEmpty(AName_.function));
            IList<VSXElement> args = this.Childs();
            if (args.Count != 0)
            {
                string q = "(";
                for (int index = 0; index < args.Count; index++)
                {
                    VSXElement el = args[index];
                    VColumn col = el as VColumn;
                    string arg;
                    if (col != null)
                    {
                        arg = col.GetNodeText(true);
                    }
                    else
                    {
                        arg = el.P_NodeText;
                    }
                    s = s + q + arg;
                    q = ", ";
                }
                s = s + ")";
            }
            return ColorGray(s);
        }
        private string GetCallInfoByText(string text)
        {
            string s = text;
            int i = 1;
            foreach (VSXElement el in this.Childs())
            {
                s = s.Replace("[par" + i.ToString() + "]", el.P_NodeText);
                i++;
            }
            return s;
        }
        #endregion
        #region Group
        public override bool P_Group_Exists()
        {

            return true;

        }
        #endregion
        #region Function
        public override bool P_Function_Exists()
        {
            return true;
        }
        public override string P_Function_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        public void P_Function_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("comment", "Комментарии");
        }
        public void P_Function_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            foreach (XElement el in XmlReports.Environment.Manager.GetScheme().Elements(EName.functions).Elements(EName.function))
            {
                string name = el.Attribute(AName_.name).Value;
                table.Rows.Add(name, name, el.AttrOrDefault(AName_.comment, string.Empty));
            }
        }
        #endregion
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            if (SelfParentOrUsepartParent() is VOutputElement) return true;
            return false;

        }
        #endregion
        #region Title
        public override bool P_Title_Exists()
        {
            if (SelfParentOrUsepartParent() is VOutputElement) return true;
            return false;

        }
        #endregion
        #region Optional


        public override bool P_Optional_Exists()
        {

            return true;

        }
        #endregion
        #region UseOnlyWithOther


        public override bool P_UseOnlyWithOther_Exists()
        {

            return P_Optional_Exists();

        }
        #endregion
        //#region ExcludeIfSet


        //public override bool P_ExcludeIfSet_Exists()
        //{

        //    return P_Optional_Exists();

        //}
        //#endregion
        #region Selective
        public override bool P_Selective_Exists()
        {
            var root = ExtendedOrRootQuery();
            if (root != null)
            {
                return ExtendedOrRootQuery().IsQube();
            }
            return false;



        }
        #endregion
        #region Valid

        public override bool P_Valid_Exists()
        {

            return GetParent() is VOutputElement;

        }
        #endregion
        #endregion
        #region ControlType
        public override bool P_ControlType_Exists()
        {
            return this.GetParent() is VOutputElement;
        }
        #endregion
        #region DataType
        public override bool P_DataType_Exists()
        {
            return true;
        }
        public override bool P_DataTypeS_Exists()
        {
            return true;
        }
        #endregion
        #region HAlign
        public override bool P_HAlign_Exists()
        {
            return true;
        }
        #endregion
        #region Format
        public override bool P_FormatS_Exists()
        {
            return true;
        }
        #endregion
        #region NullIf
        public override bool P_NullIf_Exists()
        {
            return true;
        }
        #endregion
        #region Multiplicer
        public override bool P_Multiplicer_Exists()
        {
            return true;
        }
        #endregion
        #region DontPush
        public override bool P_DontPush_Exists()
        {
            return true;
        }
        #endregion
        #region ClientCalulation



        public override bool P_ClientCalulation_Exists()
        {

            return true;

        }

        #endregion
        #region ExcelCalulation


        public override bool P_ExcelCalulation_Exists()
        {

            return true;

        }

        #endregion
        #region Table


        public override bool P_Table_Exists()
        {
            var par = GetParent();
            return par is VFormContent || par is VGridColumns || par is VGridBand || par is VFieldGroup;

        }


        #endregion
        #region ParName


        public override bool P_ParName_Exists()
        {

            return base.P_ParName_Exists() || P_Table_Exists();



        }
        #endregion
        #region Index
        public override bool P_Index_Exists()
        {
            return (this.GetParent() is VOutputElement) && (this.GetMainParent() is VQuery);
        }
        #endregion
        #region intern
        public override bool P_Intern_Exists()
        {
            return this.GetParent() is VSelect;
        }
        public override bool P_Intern_Editable()
        {
            return this.XDataType() == TextConst.AVDataType.String;
        }
        #endregion
    }
}