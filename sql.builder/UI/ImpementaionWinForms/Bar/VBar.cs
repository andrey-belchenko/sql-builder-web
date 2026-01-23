//using DevExpress.XtraBars;
//namespace sql.builder.UI.WinForms
//{
//    class VBar : Bar, IVBar
//    {
//        public void AddBarButton(IVBarItem button, bool beginGroup)
//        {
//            var btn = (BarItem)button;
//            this.Manager.Items.Add(btn);
//            //this.LinksPersistInfo.Add(new LinkPersistInfo(btn, beginGroup));
//            this.ItemLinks.Add(btn,beginGroup);
//            btn.Visibility = BarItemVisibility.Never;
//            if (button is VBarEditContainer)
//            {
//                (button as VBarEditContainer).SetBarManager(this.Manager);
//            }
//            btn.Manager = this.Manager;
//           // this.Manager.ForceInitialize();
//        }
//
//        public void SetVisible(bool val)
//        {
//            this.Visible = val;
//        }
//
//        public void HideAllItems()
//        {
//            foreach (BarItemLink link in this.ItemLinks)
//            {
//
//                link.Item.Visibility = BarItemVisibility.Never;
//
//            }
//        }
//    }
//}
