//using System;
//using System.Drawing;
//using System.Drawing.Text;
////using System.Windows.Forms;
//using System.Xml.Linq;

//namespace sql.builder
//{
//    internal class AwesomeImages
//    {
     

//        public static Font FontAwesome(float size)
//        {
//            PrivateFontCollection privateFonts = new PrivateFontCollection();
//            privateFonts.AddFontFile(Application.StartupPath+@"\sql.builder\font\fontawesome-webfont.ttf");
//            Font font = new Font(privateFonts.Families[0], size);

//            return font;

//        }

//        public static Image GetAwesomeImage(XElement ximage,float size ,Color textColor, Color backColor)
//        {
//            string rotateFlip=null;
//            if (ximage.Attribute("rotate-flip") != null)
//            {
//                rotateFlip = ximage.Attribute("rotate-flip").Value;
//            }
//            return DrawText(ximage.Attribute("char").Value, FontAwesome(size), textColor, backColor,rotateFlip);
//        }


//        // Properties.Settings.Default.skinName

        

//        public static Image DrawText(String text, Font font, Color textColor, Color backColor, string rotateFlip=null)
//        {
//            //first, create a dummy bitmap just to get a graphics object



//            Image img = new Bitmap(1, 1);
//            Graphics drawing = Graphics.FromImage(img);

//            //measure the string to see how big the image needs to be
//            SizeF textSize = drawing.MeasureString(text, font);

//            //free up the dummy image and old graphics object
//            img.Dispose();
//            drawing.Dispose();

//            //create a new image of the right size
//            img = new Bitmap((int)textSize.Width, (int)textSize.Height);

//            drawing = Graphics.FromImage(img);

//            //paint the background
//            drawing.Clear(backColor);

//            //create a brush for the text
//            Brush textBrush = new SolidBrush(textColor);

//            drawing.DrawString(text, font, textBrush, 0, 0);

//            drawing.Save();

//            textBrush.Dispose();
//            drawing.Dispose();
//            if (rotateFlip != null)
//            {
//                img.RotateFlip((RotateFlipType)Enum.Parse(typeof(RotateFlipType), rotateFlip));
//            }
//         //   bitmap1.RotateFlip(RotateFlipType.Rotate180FlipY);
//            return img;

//        }

//    }

  


  
//}
