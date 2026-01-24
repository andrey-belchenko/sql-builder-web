using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Color = System.Drawing.Color;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;color name="" rgb="" /&gt;
    /// </summary>
    /// <seealso cref="VColorPackage"/>
    /// <seealso cref="VUseColor"/>
    internal sealed class VColor : VSXElement
    {
        /// <summary>
        /// Разбирает строку <paramref name="rgb"/> в экземпляр класса <see cref="System.Drawing.Color"/>
        /// </summary>
        /// <param name="rgb">строка в формате "r,g,b", где r, g и b - целые числа от 0 до 255.</param>
        /// <param name="color">цвет</param>
        /// <returns>true, если разбор прошёл успешно</returns>
        internal static bool ParseRGB(string rgb, out Color color)
        {
            color = Color.Empty;
            if (string.IsNullOrEmpty(rgb)) {
                return false;
            }
            byte red, green, blue;
            int pos_1 = rgb.IndexOf(',');
            if (pos_1 <= 0 || !byte.TryParse(rgb.Substring(0, pos_1), out red)) {
                return false;
            }
            pos_1++;
            int pos_2 = rgb.IndexOf(',', pos_1);
            if (pos_2 < 0 || !byte.TryParse(rgb.Substring(pos_1, pos_2 - pos_1), out green)
                          || !byte.TryParse(rgb.Substring(pos_2 + 1), out blue)) {
                return false;
            }
            color = Color.FromArgb(red, green, blue);
            return true;
        }
        internal VColor()
            : base (EName.color)
        {
        }
        #region Name
        public override bool P_Name_Exists()
        {
            return true;
        }
        #endregion
        #region Rgb
        public override string P_Rgb {
            get {
                return this.AttrOrEmpty(AName_.rgb);
            }
            set {
                this.SetAttributeNotEmpty(AName_.rgb, value);
            }
        }
        public override bool P_Rgb_Exists()
        {
            return true;
        }
        public string P_Rgb_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            string s;
            string rgb = this.P_Rgb;
            Color color;
            if (ParseRGB(rgb, out color)) {
                string hex = "#" + (color.ToArgb() & 0x00FFFFFF).ToString("X6");
                string fore_color;
                // Calculate brightness using standard luminance formula (cross-platform alternative to GetBrightness)
                double brightness = (0.299 * color.R + 0.587 * color.G + 0.114 * color.B) / 255.0;
                if (brightness >= 0.5) {
                    fore_color = "black";
                } else {
                    fore_color = "white";
                }
                s = "<span style='color: " + fore_color + "; background-color: " + hex + ";'>&nbsp;&nbsp;" + hex + "&nbsp;&nbsp;</span>";
            } else {
                s = string.Empty;
            }
            return s + " " + this.P_NodeName + " " + Bold(this.P_Name) + " " + this.P_Rgb;
        }
        #endregion
    }
}