using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
//using System.Windows.Forms;
using Devart.Data.Oracle;
using sql.builder.FieldInfo;
using System.Reflection;
using sql.builder.UI;

namespace sql.builder.DataApi
{
    internal partial class VSXElement : VXElement
    {

        public bool IsStringAddision(string s)
        {
            if (s.Contains(TextConst.AVTitle.Add))
            {
                return true;
            }
            return false;
        }

        public  string MakeMultidefinedPropertyValue(List<string> strings)
        {
            string s="";
            foreach (string s1 in strings)
            {
                if (s != "")
                {
                    if (!s.Contains(TextConst.AVTitle.Add))
                    {
                        break;
                    }
                }
                if (s1 != "")
                {
                    if (s.Contains(TextConst.AVTitle.Add))
                    {
                        s = s.Replace(TextConst.AVTitle.Add, s1);
                        // s+=s1.Substring(1,s1.Length-1);
                    }
                    else
                    {
                        s = s1;
                    }
                }
            }
            return s;
        }

        //public List<VSXElement> GetNavigationFields()
        //{
        //    if (!(this.GetParent() is VOutputElement))
        //    {
        //        return new List<VSXElement>();
        //    }
        //    var rootQuery = this.RootQuery();
        //    if (rootQuery == null) return new List<VSXElement>();
        //    string table = this.RootQuery().XName;
        //    List<VSXElement> list = GetEnvironment().SchemeNative.Elements(TextConst.EName.NavigationItems)
        //        .Descendants(TextConst.EName.NavigationField)
        //        .Where(e => Cmn.GetAttrValue(e, TextConst.AName.Table) == table)
        //        .Where(e1 => Cmn.GetAttrValue(e1, TextConst.AName.Column) == "*" || Cmn.GetAttrValue(e1, TextConst.AName.Column) == XName)
        //        .ToList().Select(e3 => VSXElement.Get(e3)).ToList();
        //    return list;
        //}

        //#region DocDataType
        //public virtual string P_DocDataType
        //{
        //    get
        //    {
        //        return "";
        //    }

        //}
        //public virtual string P_DocDataType_FieldGroup()
        //{

        //    return TextConst.SchEdirorFieldGr.Documenting;

        //}
        //public virtual string P_DocDataType_Title()
        //{

        //    return "Тип данных для описания";

        //}

        //public virtual string P_DocDataType_ControlType()
        //{

        //    return typeof(UIText).Name;

        //}

        //public virtual bool P_DocDataType_Exists()
        //{
            
        //    return false;

        //}
        //#endregion


        //#region Description
        //public virtual string P_Description
        //{
        //    get
        //    {
        //        return GetAttrValue(TextConst.AName.Description);
        //    }
        //    set
        //    {
        //        SetAttributeNotEmpty(TextConst.AName.Description, value);
        //    }
        //}

        //public virtual string P_Description_FieldGroup()
        //{

        //    return TextConst.SchEdirorFieldGr.Documenting;

        //}

        //public virtual string P_Description_Title()
        //{

        //    return "Описание (собственное)";

        //}

        //public virtual string P_Description_ControlType()
        //{

        //    return typeof(UIText).Name;

        //}

        //public virtual bool P_Description_Exists()
        //{
        //    var parent = SelfParentOrUsepartParent();
        //    if (parent is VSelect || this is VField) return true;


        //    if (this is VQueryCall)
        //    {
        //        return (this as VQueryCall).IsQuery();
        //    }

        //    return false;
        //}

         

        //#endregion


        //#region DescriptionSearched
        //public virtual string P_DescriptionSearched
        //{
        //    get
        //    {
        //        return MakeMultidefinedPropertyValue(  P_DescriptionSearched_Search().Select(e=>e.P_Description).ToList());
                
        //    }
            
        //}

        //public virtual bool P_DescriptionSearched_IsHtml()
        //{
        //    return true;
        //}

        //public virtual bool P_DescriptionSearched_Editable()
        //{
        //    return false;
        //}

        //public virtual List<VSXElement> P_DescriptionSearched_Search()
        //{
        //    VLookupAnalyzer anl = new VLookupAnalyzer();
        //    anl.CheckReturn = (VSXElement element) =>
        //    {
        //        if (element.P_Description != "") return true;
        //        return false;
        //    };
        //    anl.CheckStop = (VSXElement element) =>
        //    {
        //        return anl.CheckReturn(element) && !IsStringAddision(element.P_Description);
        //    };

        //    return LookUpSources(anl);
        //}
        //public virtual string P_DescriptionSearched_FieldGroup()
        //{

        //    return TextConst.SchEdirorFieldGr.Documenting;

        //}
        //public virtual string P_DescriptionSearched_Title()
        //{

        //    return "Описание";

        //}

        //public virtual string P_DescriptionSearched_ControlType()
        //{

        //    return typeof(UIText).Name;

        //}

        //public virtual bool P_DescriptionSearched_Exists()
        //{
           
        //    return P_Description_Exists();

        //}
        //#endregion


        //#region NavigationInfo

       

        //public virtual string P_NavigationInfo
        //{
        //    get
        //    {
        //        //return MakeMultidefinedPropertyValue(P_NavigationInfo_Search().Select(e => e.P_Description).ToList());
        //        var els = P_NavigationInfo_Search();
        //        if (els.Count > 0)
        //        {

        //            foreach (VNavigationField fld in els.ToList())
        //            {
        //                if (fld.P_Column == TextConst.AVColumn.All)
        //                {
        //                    VNavigationField fld1 = (VNavigationField)els.Where(e =>  e.P_Column != TextConst.AVColumn.All && e.Ancestors().Where(e1=>e1== fld.GetParent()).FirstOrDefault()!=null ).FirstOrDefault();
        //                    if (fld1 != null)
        //                    {
        //                        els.Remove(fld);
        //                    }
        //                }
        //            }


        //            return string.Join(Environment.NewLine, els.Cast<VNavigationField>().Select(e => e.GetNavigationPath()).Distinct());
        //        }
        //        return "";
             
        //    }

        //}

        //public virtual bool P_NavigationInfo_IsHtml()
        //{
        //    return true;
        //}

        //public virtual bool P_NavigationInfo_Editable()
        //{
        //    return false;
        //}

        //public virtual List<VSXElement> P_NavigationInfo_Search()
        //{
          
        //    VLookupAnalyzer anl = new VLookupAnalyzer();

        //    anl.Return = (VSXElement element) =>
        //    {
        //        var nFields = element.GetNavigationFields();

        //        if (nFields.Count() > 0)
        //        {
        //            anl.Tag = true;
        //            return nFields;
        //        }
        //        else
        //        {
        //            if (element is VColumn && !(element is VFact))
        //            {
        //                var col = (VColumn)element;
        //                var src=col.Source();
        //                if (src is VLink)
        //                {
        //                    var rel = (src as VLink).GetRelation();
        //                    if (rel != null)
        //                    {
        //                        var col1 = rel.ChildColumnSource();
        //                        if (col1 != null)
        //                        {
        //                            nFields = col1.GetNavigationFields();
        //                            if (nFields.Count() > 0)
        //                            {
        //                                anl.Tag = true;
        //                                return nFields;
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        anl.Tag = false;
        //        return null;
        //    };

        //    anl.CheckStop = (VSXElement element) =>
        //    {
        //        return (bool) Cmn.Nvl( anl.Tag,false);
        //    };

        //    return LookUpSources(anl);
        //}
        //public virtual string P_NavigationInfo_FieldGroup()
        //{

        //    return TextConst.SchEdirorFieldGr.Documenting;

        //}
        //public virtual string P_NavigationInfo_Title()
        //{

        //    return "Источник данных для документации";

        //}

        //public virtual string P_NavigationInfo_ControlType()
        //{

        //    return typeof(UIText).Name;

        //}

        //public virtual bool P_NavigationInfo_Exists()
        //{
         
        //    return P_Description_Exists();

        //}
        //#endregion




        //#region DefValInfoSearched
        //public virtual string P_DefValInfoSearched
        //{
        //    get
        //    {
        //        return "";

        //    }

        //}

        //public virtual bool P_DefValInfoSearched_Editable()
        //{
        //    return false;
        //}

        //public virtual List<VSXElement> P_DefValInfoSearched_Search()
        //{
           
        //    return new List<VSXElement>();
        //}
        //public virtual string P_DefValInfoSearched_FieldGroup()
        //{

        //    return TextConst.SchEdirorFieldGr.Documenting;

        //}
        //public virtual string P_DefValInfoSearched_Title()
        //{

        //    return "Значение по умолчанию для параметра";

        //}

    

        //public virtual bool P_DefValInfoSearched_Exists()
        //{
  
        //    return false;

        //}
        //#endregion

    }



   




}
