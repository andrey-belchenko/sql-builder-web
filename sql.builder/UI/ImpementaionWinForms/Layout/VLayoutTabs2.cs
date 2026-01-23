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
////using DevExpress.XtraTab;
//using sql.builder.UI;
//using sql.builder.WinForms;
//
//namespace sql.builder.UI.WinForms
//{
//    internal partial class VLayoutTabs2 : XtraUserControl, IVLayoutTabs
//    {
//        public VLayoutTabs2()
//        {
//            InitializeComponent();
//           
//        }
//        VLayoutTabsInfo Tabs = null;
//        Dictionary<int, XtraTabPage> tabControlsByInfo = new Dictionary<int, XtraTabPage>();
//
//        public void SelectFirst()
//        {
//            this.xtraTabControl1.SelectedTabPageIndex = 0;
//        }
//        public void Init(VLayoutTabsInfo tabs)
//        {
//            bool allNew = true;
//            foreach (VLayoutGroupInfo tab in tabs.Nodes)
//            {
//                var tabId = tab.GetHashCode();
//                if (!tabControlsByInfo.ContainsKey(tabId))
//                {
//
//                    var itemControl = (Control)tab.GetControl();
//
//                    itemControl.Padding = new System.Windows.Forms.Padding(0);
//                    itemControl.Margin = new System.Windows.Forms.Padding(0);
//                    itemControl.Dock = DockStyle.Fill;
//
//
//                    var tab1 = new XtraTabPage();
//
//                    this.xtraTabControl1.TabPages.Add(tab1);
//                    // 
//                    // xtraTabPage1
//                    // 
//
//
//                    tab1.Margin = new System.Windows.Forms.Padding(0);
//
//                    tab1.Text = tab.GetText();
//                    tab1.Controls.Add(itemControl);
//
//                    tab1.Tag = tab;
//                    tabControlsByInfo.Add(tabId, tab1);
//                    tab1.PageVisible = false;
//                }
//                else
//                {
//                    allNew = false;
//                }
//            }
//            if (allNew)
//            {
//                this.xtraTabControl1.SelectedTabPageIndex = 0;
//            }
//           
//            Tabs = tabs;
//        }
//
//        public XtraTabPage GetLayoutItemByTabInfo(VLayoutGroupInfo tab)
//        {
//            return tabControlsByInfo[tab.GetHashCode()];
//        }
//        public void ShowTab(VLayoutGroupInfo tab)
//        {
//            var tabControl = GetLayoutItemByTabInfo(tab);
//            tabControl.PageVisible = true;
//           
//           
//        }
//        public void HideTab(VLayoutGroupInfo tab)
//        {
//            var tabControl = GetLayoutItemByTabInfo(tab);
//
//
//            if (xtraTabControl1.SelectedTabPage == tabControl)
//            {
//                var newSelIndex = xtraTabControl1.SelectedTabPageIndex - 1;
//
//                if (newSelIndex < 0)
//                {
//                    newSelIndex = xtraTabControl1.SelectedTabPageIndex + 1;
//                }
//
//                if (newSelIndex < xtraTabControl1.TabPages.Count)
//                {
//                    xtraTabControl1.SelectedTabPageIndex = newSelIndex;
//                }
//            }
//          
//            tabControl.PageVisible = false;
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
//        public VLayoutGroupInfo GetSelectedTab()
//        {
//            if (xtraTabControl1.SelectedTabPage == null)
//            {
//                return null;
//            }
//            if (xtraTabControl1.SelectedTabPage.Controls.Count==0)
//            {
//                return null;
//            }
//
//
//            return (VLayoutGroupInfo)xtraTabControl1.SelectedTabPage.Tag;
//
//
//        }
//       
//
//        public int BorderHeight()
//        {
//            return 24;// уточнить
//        }
//        public int BorderWidth()
//        {
//            return 2; // уточнить
//        }
//
//        public int WidthDisplacement()
//        {
//            return xtraTabControl1.Width - xtraTabControl1.DisplayRectangle .Width- BorderWidth();
//            //if (xtraTabControl1.SelectedTabPage == null)
//            //{
//            //   return 0;
//            //}
//            //return 0;
//            //return xtraTabControl1.Width - xtraTabControl1.SelectedTabPage.Width - BorderWidth();
//        }
//        public int HeightDisplacement()
//        {
//            //if (xtraTabControl1.SelectedTabPage == null)
//            //{
//            //    return 0;
//            //}
//            //return 0;
//            return xtraTabControl1.Height - xtraTabControl1.DisplayRectangle.Height - BorderHeight();
//        }
//
//        private void xtraTabControl1_SelectedPageChanged(object sender, TabPageChangedEventArgs e)
//        {
//            if (Tabs != null)
//            {
//                Tabs.SelectedTabChanged();
//            }
//            //if (e.Page != null)
//            //{
//            //    MessageBox.Show(xtraTabControl1.Width.ToString() + "|" + xtraTabControl1.ClientSize.Width.ToString() + "|" + this.Width.ToString() + "|" + this.ClientSize.Width.ToString() + "|" + e.Page.Width.ToString());
//            //}
//        }
//
//        private void xtraTabControl1_SelectedPageChanging(object sender, TabPageChangingEventArgs args)
//        {
//            if (args.PrevPage != null)
//            {
//                var uiformControl = Cmn.GetChildControlsOfType<sql.builder.UI.WinForms.UIFormControl>(args.PrevPage).FirstOrDefault();
//
//                UIFormC uiform = null;
//
//                if (uiformControl != null)
//                {
//                    uiform = uiformControl.GetVForm();
//                }
//
//                if (uiform != null && uiform.IsModifiedSelfOrSub())
//                {
//                    var result = ShowMessage.Show(ShowMessage.MType.UnsavedChangesQuestion);
//                    if (result == DialogResult.Cancel)
//                    {
//                        args.Cancel = true;
//                        return;
//                    }
//                    else if (result == DialogResult.No)
//                    {
//                        uiform.RefreshSource();
//                    }
//                    else if (result == DialogResult.Yes)
//                    {
//                        if (!uiform.SaveData(false))
//                        {
//                            args.Cancel = true;
//                            return;
//                        }
//                    }
//                }
//            }
//
//            //if (args.Page != null)
//            //{
//            //    Cmn.GetChildControlsOfType<UIFormC>(args.Page).Where(f => f.FormUseType == UIFormC.UseType.DataEditor).ForEach(f => f.RefreshData());
//            //}
//        }
//
//    }
//}
