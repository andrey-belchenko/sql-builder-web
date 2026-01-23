//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraBars;
//using System.Drawing;
//namespace sql.builder.UI.WinForms
//{
//    class VBarButton : BarButtonItem,IVBarButton
//    {
//
//        public VBarButton()
//        {
//
//            this.ItemClick += VBarButton_ItemClick;
//        
//        }
//
//        void VBarButton_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            if (ButtonClick != null)
//            {
//                ButtonClick(this);
//            }
//        }
//        public void SetCaption(string value)
//        {
//
//            this.Caption = value;
//        }
//
//        public void SetImage(Image value)
//        {
//            this.Glyph = value;
//        }
//        public void SetVisible(bool value)
//        {
//           // this.Visibility = BarItemVisibility.Always;
//           
//            this.Visibility = (value) ? BarItemVisibility.Always : BarItemVisibility.Never;
//        }
//
//        public void SetEnabled(bool value)
//        {
//            this.Enabled= (value);
//        }
//
//        public event ValueChangeEventHandler ButtonClick;
//
//
//
//
//        public void SetIsRight(bool value)
//        {
//            if (value)
//            {
//                Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Right;
//            }
//            else
//            {
//                Alignment = DevExpress.XtraBars.BarItemLinkAlignment.Left;
//            }
//        }
//    }
//}
