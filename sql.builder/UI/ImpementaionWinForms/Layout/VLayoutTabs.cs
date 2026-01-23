//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
////using DevExpress.Utils;
////using DevExpress.XtraEditors;
////using DevExpress.XtraGrid.Columns;
////using DevExpress.XtraGrid.Views.Grid;
////using DevExpress.XtraLayout;
////using DevExpress.XtraLayout.Utils;
//
////using DevExpress.XtraBars;
////using DevExpress.XtraBars.Docking2010.Views;
////using DevExpress.XtraEditors.Controls;
////using DevExpress.XtraLayout.Customization;
//
//
//namespace sql.builder.UI.WinForms
//{
//    internal partial class VLayoutTabs : XtraUserControl, IVLayoutTabs
//    {
//        public VLayoutTabs()
//        {
//            InitializeComponent();
//            TabbedGroup.RemoveTabPage(layoutControlGroup2);
//        }
//        VLayoutTabsInfo Tabs = null;
//        SortedList<int, LayoutGroup> tabControlsByInfo = new SortedList<int, LayoutGroup>();
//     
//        public void Init(VLayoutTabsInfo tabs)
//        {
//
//            foreach (VLayoutGroupInfo tab in tabs.Nodes)
//            {
//                
//                var itemControl = (Control)tab.GetControl();
//                //var panel = new PanelControl();
//                //panel.Controls.Add(itemControl);
//                //panel.Margin = new System.Windows.Forms.Padding(0);
//                //panel.Padding = new System.Windows.Forms.Padding(0);
//                //panel.Appearance.BorderColor = Color.Transparent;
//                //panel.Appearance.Options.UseBorderColor = true;
//                itemControl.Padding = new System.Windows.Forms.Padding(0);
//                itemControl.Margin = new System.Windows.Forms.Padding(0);
//               // itemControl.Dock = DockStyle.Fill;
//                var tab1 = TabbedGroup.AddTabPage();
//                tab1.Padding = new DevExpress.XtraLayout.Utils.Padding(0);
//              
//                tab1.Text = tab.GetText();
//                tab1.Tag = tab;
//                var item = tab1.AddItem("", itemControl);
//                item.TextVisible = false;
//                tabControlsByInfo.Add(tab.GetHashCode(), tab1);
//                tab1.Visibility = LayoutVisibility.Never;
//            }
//            TabbedGroup.SelectedTabPageIndex = 0;
//            Tabs = tabs;
//        }
//
//        public LayoutGroup GetLayoutItemByTabInfo(VLayoutGroupInfo tab)
//        {
//            return tabControlsByInfo[tab.GetHashCode()];
//        }
//        public void ShowTab(VLayoutGroupInfo tab)
//        {
//            var tabControl = GetLayoutItemByTabInfo(tab);
//            tabControl.Visibility = LayoutVisibility.Always;
//           
//        }
//
//      
//        public void HideTab(VLayoutGroupInfo tab)
//        {
//            var tabControl = GetLayoutItemByTabInfo(tab);
//           
//            
//            if (tabControl.ParentTabbedGroup.SelectedTabPage == tabControl)
//            {
//                var newSelIndex = tabControl.ParentTabbedGroup.SelectedTabPageIndex - 1;
//
//                if (newSelIndex < 0)
//                {
//                    newSelIndex = tabControl.ParentTabbedGroup.SelectedTabPageIndex + 1;
//                }
//
//                if (newSelIndex < tabControl.ParentTabbedGroup.TabPages.Count)
//                {
//                    tabControl.ParentTabbedGroup.SelectedTabPageIndex = newSelIndex;
//                }
//            }
//            tabControl.Visibility = LayoutVisibility.Never;
//            
//
//        }
//
//        //public void ShowTab(VLayoutTabsInfo tabs, VLayoutGroupInfo tab)
//        //{
//
//            
//
//
//        //}
//
//        public VLayoutGroupInfo GetSelectedTab()
//        {
//            if (TabbedGroup.SelectedTabPage == null)
//            {
//                return null;
//            }
//            if (!TabbedGroup.SelectedTabPage.Items.Any())
//            {
//                return null;
//            }
//
//
//            return (VLayoutGroupInfo)TabbedGroup.SelectedTabPage.Tag;
//
//            
//        }
//
//        public int BorderHeight()
//        {
//            return 32;// уточнить
//        }
//        public int BorderWidth()
//        {
//            return 10; // уточнить
//        }
//
//
//        //public int WidthDisplacement()
//        //{
//        //    return 5;
//        //}
//        //public int HeightDisplacement()
//        //{
//        //    return 3;
//        //}
//        
//        private void tabbedControlGroup1_SelectedPageChanged(object sender, LayoutTabPageChangedEventArgs e)
//        {
//            if (Tabs != null)
//            {
//                Tabs.SelectedTabChanged();
//            }
//        }
//
//
//        public void SelectFirst()
//        {
//            TabbedGroup.SelectedTabPageIndex = 0;
//           // throw new NotImplementedException();
//        }
//    }
//}
