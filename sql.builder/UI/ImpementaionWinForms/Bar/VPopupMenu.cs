//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using DevExpress.XtraBars;
//using System.Drawing;
////using System.Windows.Forms;
//namespace sql.builder.UI.WinForms
//{
//    class VPopupMenu :PopupMenu, IVPopupMenu
//    {

//        public VPopupMenu()
//        {
            
//        }
//        public void Clear()
//        {
//            this.ClearLinks();
//        }
//        public bool IsEmpty()
//        {
//            return !this.ItemLinks.Any();
//        }
//        public void AddButton(IVBarItem button, bool beginGroup)
//        {
//            var btn = (button as BarItem);
//            this.Manager.Items.Add(btn);
//            this.ItemLinks.Add(btn, beginGroup);
//           // this.LinksPersistInfo.Add(new LinkPersistInfo(btn, beginGroup));
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

//        public void SetBarManager(object value)
//        {
//            this.Manager = (BarManager) value;
//            foreach (BarItemLink link in this.ItemLinks)
//            {
//                var item = link.Item;
//                if (item is VBarEditContainer)
//                {
//                    (item as VBarEditContainer).SetBarManager(this.Manager);
//                }
//                else if (item is VBarMenu)
//                {
//                    (item as VBarMenu).SetBarManager(this.Manager);
//                }
//                item.Manager = this.Manager;
//            }
//            this.Manager = this.Manager;
            
//        }

//        public void Show()
//        {
//            this.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
//        }



//    }
//}
