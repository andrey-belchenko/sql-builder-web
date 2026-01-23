//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraBars;
//using System.Drawing;
//namespace sql.builder.UI.WinForms
//{
//    class VBarEditContainer : BarEditItem,IVBarEditContainer
//    {
//
//        public VBarEditContainer()
//        {
//
//            PaintStyle = BarItemPaintStyle.Caption;
//        
//        }
//
//        
//        public void SetCaption(string value)
//        {
//            this.Caption = value;
//        }
//
//        public void SetImage(Image value)
//        {
//            this.Glyph = value;
//        }
//
//        public void SetEdit(object value)
//        {
//
//            this.Edit = (DevExpress.XtraEditors.Repository.RepositoryItem)value;
//        }
//
//        public void SetBarManager(BarManager value)
//        {
//            value.RepositoryItems.Add(this.Edit);
//            this.Manager = value;
//        }
//        public void SetVisible(bool value)
//        {
//        
//           
//            this.Visibility = (value) ? BarItemVisibility.Always : BarItemVisibility.Never;
//        }
//
//        public void SetEnabled(bool value)
//        {
//            this.Enabled= (value);
//        }
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
