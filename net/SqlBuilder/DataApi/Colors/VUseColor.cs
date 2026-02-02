using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;use-color color="" /&gt;
    /// </summary>
    /// <seealso cref="VColor"/>
    public sealed class VUseColor : VSXElement
    {
        public VUseColor()
            : base(EName.use_color)
        {
        }
        //public VColor Color()
        //{
        //    return XmlReports.Environment.GetColor(this.P_Color);
        //}
        #region Color
        public override string P_Color {
            get {
                return this.AttrOrEmpty(AName_.color);
            }
            set {
                this.SetAttributeValue(AName_.color, value);
            }
        }
        public override bool P_Color_Exists()
        {
            return true;
        }
        public string P_Color_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        public void P_Color_List(VDataTable table)
        {
            table.AddColumn("id", "Имя");
            table.AddColumn("pkg", "Пакет");
            table.AddColumn("rgb");
        }
        public void P_Color_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            //IList<VColor> colors = XmlReports.Environment.GetColors();
            //for (int index = 0; index < colors.Count; index++) {
            //    VColor el = colors[index];
            //    table.AddRow(el.P_Name, el.GetParent().P_IdName, el.P_Rgb);
            //}
            foreach (XElement pkg in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.color_packages).Elements(EName.color_package)) {
                string package = pkg.AttrOrEmpty(AName_.name);
                foreach (XElement color in pkg.Elements(EName.color)) {
                    if (color.Attribute(AName_.exclude) == null) {
                        table.AddRow(color.AttrOrEmpty(AName_.name), package, color.AttrOrEmpty(AName_.rgb));
                    }
                }
            }
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            string color = this.P_Color;
            string s = Bold(color);
            VColor clr = XmlReports.Environment.GetColor(color);
            if (clr != null) {
                return s += " " + clr.P_Rgb;
            }
            return s;
        }
        #endregion
    }
}