using System.Xml.Linq;
//using System.Windows.Forms;
using System.Collections.Generic;
//using DevExpress.XtraLayout;
//using DevExpress.XtraLayout.Utils;
//using infoenergo.core.Extensions;
using sql.builder.DataApi;

namespace sql.builder.UI
{
    public partial class UIFormC : IForm
    {
        private List<TabContainerItem> tabContainerItems;
        private SortedList<string, TabItem> namedTabs;
        //private TabContainerItem AddTabContainerItem(TabbedGroup tabContainer, TabItem parentTab)
        //{
        //    var tabContainerItem = new TabContainerItem();
        //    tabContainerItem.TabContainer = tabContainer;
        //    if (parentTab != null)
        //    {
        //        parentTab.ChildTabContainerItems.Add(tabContainerItem);
        //        tabContainerItem.ParentTab = parentTab;
        //    }
        //    else
        //    {
        //        if (tabContainerItems == null)
        //        {
        //            tabContainerItems = new List<TabContainerItem>();
        //        }
        //        tabContainerItems.Add(tabContainerItem);
        //    }
        //    return tabContainerItem;
        //}
        //private TabItem AddTabItem(LayoutGroup tab, TabContainerItem containerItem, string name)
        //{
        //    var tabItem = new TabItem();
        //    tabItem.Tab = tab;
        //    tabItem.ContainerItem = containerItem;
        //    containerItem.TabItems.Add(tabItem);
        //    if (name != null)
        //    {
        //        if (namedTabs == null)
        //        {
        //            namedTabs = new SortedList<string, TabItem>();
        //        }
        //        namedTabs.Add(name, tabItem);
        //    }
        //    return tabItem;
        //}
        private void SelectTabs(List<TabContainerItem> tabContainerItems1)
        {
            if (tabContainerItems1 != null)
            {
                foreach (TabContainerItem tabContainerItem in tabContainerItems1)
                {
                    if (tabContainerItem.TabItems.Count > 1)
                    {
                        var tabItem = tabContainerItem.TabItems[0];
                        tabItem.Activate();
                        foreach (TabItem tabItem1 in tabContainerItem.TabItems)
                        {
                            SelectTabs(tabItem1.ChildTabContainerItems);
                        }
                    }
                }
            }
        }
        internal void ActivateTab(string name)
        {
            var tab = namedTabs[name];
            while (tab != null)
            {
                tab.Activate();
                tab = tab.ContainerItem.ParentTab;
            }
        }
        internal class TabContainerItem
        {
            public TabItem ParentTab;
            //public TabbedGroup TabContainer;
            public List<TabItem> TabItems = new List<TabItem>();
        }
        internal class TabItem
        {
            //public LayoutGroup Tab;
            public TabContainerItem ContainerItem;
            public List<TabContainerItem> ChildTabContainerItems = new List<TabContainerItem>();
            public void Activate()
            {
                //if (Tab.Visibility != LayoutVisibility.Never)
                //{
                //    ContainerItem.TabContainer.SelectedTabPage = Tab;
                //}
            }
            public string EnabledSource = null;
            public void SetEnabled(bool val)
            {
                //Tab.Enabled = val;
            }
        }
    }
}
