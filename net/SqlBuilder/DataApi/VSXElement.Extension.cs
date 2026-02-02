using System;
using System.Linq;
using System.Xml.Linq;
//using infoenergo.core.Extensions;
using sql.builder.Clean.Extensions;
using sql.builder.UI;
using sql.builder.Exceptions;
namespace sql.builder.DataApi
{
    public partial class VSXElement : VXElement
    {
        public string GetColumnTempName()
        {
            var rc = (RootQuery() as VQuery);
            var vc = (this as VColumn);
            if (vc != null)
            {
                if (vc.Source().P_Updateable==TextConst.AVBool.True)
                {
                    return vc.SourceColumn()[0].GetColumnTempName();
                }
            }
            return rc.GetColumnTempName(XName, XDataType());

        }

        public virtual VSXElement GetDummyOrSelf()
        {
            
            return this;
        }
        public VField CreateFieldFromQueryColumn()
        {

            var tcol = (this as VColumn);
            var element = new XElement(TextConst.EName.Field);
            element.SetAttributeValue(TextConst.AName.Table, tcol.MasterSource().XName);
            element.SetAttributeValue(TextConst.AName.Name, XName);
            element.SetAttributeValue(TextConst.AName.Title, P_Title);
            if (P_ColumnWidth != "") element.SetAttributeValue(TextConst.AName.Width, P_ColumnWidth);

            string ct = ControlType();
            element.SetAttributeValue(TextConst.AName.ControlType, ct);
            // по умолчанию два знака после запятой
            //if (ct == TextConst.AVControlType.Number)
            //{
            //    element.SetAttributeValue(TextConst.AName.EditMask, tcol.AttrOrDef(TextConst.AName.Format, "n2"));
            //}
            // Емцов - UINumber всегда показывает кнопку - переделал на UIText
            
            var format = tcol.XFormat();
            if (format != "")
            {
                element.SetAttributeValue(TextConst.AName.Format, format);
                element.SetAttributeValue(TextConst.AName.EditMask, format);// путаница не понятно что для чего, без этого  не работает в гриде
            }
            else
            {
                if (tcol.XDataType() == TextConst.AVDataType.Number && ct == TextConst.AVControlType.Text)
                {
                    element.SetAttributeValue(TextConst.AName.EditMask, tcol.AttrOrDef(TextConst.AName.Format, "N2"));
                }
            }
            //if (tcol.XDataType() == TextConst.AVType.Number && ct == TextConst.AVControlType.Text)
            //{
            //    element.SetAttributeValue(TextConst.AName.EditMask, tcol.AttrOrDef(TextConst.AName.Format, "N2"));
            //}


            if (ct != TextConst.AVControlType.List)
            {
                var rowsLimit = SelectionListRowsLimit();

                element.SetAttributeValue(TextConst.AName.RowsLimit, rowsLimit);
            }
            // аттрибуты для дизайнера
            foreach (var layout_option in TextConst.ANameArray.AllLayoutOptions)
            {
                element.SetAttributeValue(layout_option, this.AttrOrDef(layout_option, null));
            }
            //element.Add(Elements());
            return VSXElement.Get<VField>(element);
        }
        public string SelectionListRowsLimit()
        {

            var s = P_RowsLimit;

            if (s == "" && (this is VColumn))
            {
                //var c = (this as VColumn).SourceColumn().FirstOrDefault();
                //if (c == null)
                //{
                //    VCashUtils.ClearCash();
                //    c = (this as VColumn).SourceColumn().FirstOrDefault();
                //}

                var col = (this as VColumn).SourceColumn().FirstOrDefault();
                if (col == null)
                {
                    throw new VCompilerException("Колонка не найдена", GetMainParent(), this);
                }
                s = col.P_RowsLimit;


                
            }

            if (s != "")
            {
                return s;
            }


            return "100";
        }
        public string ControlType()
        {


            var ct = P_ControlType;
            if (ct != "")
            {
                return ct;
            }

            VQuery typeQry=null;
            VColumn col = null;
            if (this is VColumn)
            {
                col = (this as VColumn);
            }

            if (col == null)
            {
                return "";
            }

            if (col.P_IsHyperlink == TextConst.AVBool.True)
            {
                throw new NotImplementedException();
                //return typeof(UILink).Name;
            }


            var src = col.SourceColumn().FirstOrDefault();

            if (src != this)
            {
                if (src != null)
                {
                    ct = src.ControlType();
                }
                if (ct != "")
                {
                    return ct;
                }
            }



            typeQry = col.TypeQuery();
           
            if (typeQry != null)
            {
                if (typeQry.P_SpecTable == TextConst.AVSpecTable.File)
                {
                    throw new NotImplementedException();
                    //return typeof(UIFile).Name;
                }
                else
                {
                    return typeof(UICombo).Name;
                }
            }
            else
            {
                switch (col.XDataType())
                {
                    case TextConst.AVDataType.Date: return typeof(UIDate).Name;
                    case TextConst.AVDataType.Clob:
                        throw new NotImplementedException();
                        //return typeof(UITextEx).Name;
                    //case TextConst.AVType.Number: return typeof(UINumber).Name;
                    default: return typeof(UIText).Name;
                }
            }
        }

        public string GetPositionId()
        {
            VSXElement el = this;
            string s = "";
            string q = "";
            while (!el.IsMainElement())
            {

                s = el.ElementsBeforeSelf().Count().ToString() + q+ s;
                q = "/";
              
                el = el.GetParent();
                
            }
            return s;
        }


        public VSXElement GetElementByPositionId(string posId)
        {
            var ss = posId.Split('/');

            VSXElement element = this;

            foreach (string s in ss)
            {
                if (s =="") break;
                int i = Convert.ToInt32(s);

                if (element.Elements().Count() > i)
                {

                    element = Get( element.Elements().ElementAt(i));

                }
                else
                {
                    break;
                }

            }
            return element;


        }


        public virtual XElement GetFormXElement()
        {
            var xform = new XElement(TextConst.EName.Form);

            Cmn.copyAttributes(this, xform);

           
            return xform;
        }

        public string GetProjectName()
        {
            // неудачный вариант, переделать если изменится структура хранения
            var mp = GetMainParent();

            var fl = Cmn.GetAttrValue(mp, TextConst.AName.File);

            var ss = fl.Split('\\').ToList();

            int i1 = ss.IndexOf("projects");

            var name = ss[i1 + 1];

            return name;
        }

    }



   




}
