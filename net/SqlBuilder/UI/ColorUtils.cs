using System.Collections.Generic;
using System.Drawing;
//using DevExpress.Skins;
//using DevExpress.XtraEditors;
namespace sql.builder
{
    class VColorUtils
    {
        private static SortedList<string, Color> _parsedColors = new SortedList<string, Color>();
        //private static SortedList<string, Color> _parsedSelectionColors = new SortedList<string, Color>();
        public static Color GetColorFromRGBString(string scolor)
        {
            if (!_parsedColors.ContainsKey(scolor))
            {
                var items = scolor.Split(',');
                int red = int.Parse(items[0]);
                int green = int.Parse(items[1]);
                int blue = int.Parse(items[2]);
                _parsedColors[scolor] = Color.FromArgb(red, green, blue);
            }
            return _parsedColors[scolor];
        }

        public static Color GetSelectionColorFromRGBString(string scolor, Color defaultSelColor)
        {
            //if (!_parsedSelectionColors.ContainsKey(scolor))
            //{


            var color = GetColorFromRGBString(scolor);
            //var fcolor = GetColorFromRGBString("0,0,0");
            //var fcolor = CommonSkins.GetSkin(LookAndFeel)[CommonSkins.SkinSelection].Color.BackColor;
            var vcolor = defaultSelColor.MixColors(color, 0.3F);


            //_parsedSelectionColors[scolor] = vcolor;
            //}
            //return _parsedSelectionColors[scolor];
            return vcolor;
        }
    }
}
