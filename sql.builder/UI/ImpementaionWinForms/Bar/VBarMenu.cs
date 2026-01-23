//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraBars;
//using System.Drawing;
//namespace sql.builder.UI.WinForms
//{
//    class VBarMenu : BarLinkContainerItem, IVBarMenu
//    {
//
//        public VBarMenu()
//        {
//            PaintStyle = DevExpress.XtraBars.BarItemPaintStyle.CaptionGlyph;
//        }
//        public void SetCaption(string value)
//        {
//            if (this.Glyph != null)// костыль для системных кнопок в гриде
//            {
//                return;
//            }
//            this.Caption = value;
//        }
//
//        public void SetImage(Image value)
//        {
//            if (value != null)
//            {
//                this.Caption = null;
//            }
//            this.Glyph = value;
//        }
//        public void SetVisible(bool value)
//        {
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
//
//        public void AddButton(IVBarItem button, bool beginGroup)
//        {
//            var btn = (button as BarItem);
//            this.Manager.Items.Add(btn);
//            //this.LinksPersistInfo.Add(new LinkPersistInfo(btn, beginGroup));
//            this.ItemLinks.Add(btn, beginGroup);
//            if (button is VBarEditContainer)
//            {
//                (button as VBarEditContainer).SetBarManager(this.Manager);
//            }
//            else if (button is VBarMenu)
//            {
//                (button as VBarMenu).SetBarManager(this.Manager);
//            }
//            btn.Manager = this.Manager;
//        }
//
//        public void SetBarManager(BarManager value)
//        {
//            this.Manager = value;
//            foreach (BarItemLink link in this.Links)
//            {
//                var item = link.Item;
//
//                if (item is VBarEditContainer)
//                {
//                    (item as VBarEditContainer).SetBarManager(this.Manager);
//                }
//                else if (item is VBarMenu)
//                {
//                    (item as VBarMenu).SetBarManager(this.Manager);
//                }
//                item.Manager = value;
//                if (!this.Manager.Items.Contains(item))
//                {
//                    this.Manager.Items.Add(item);
//                }
//            }
//            this.Manager = value;
//        }
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
