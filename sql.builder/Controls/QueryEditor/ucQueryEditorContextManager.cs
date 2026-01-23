//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
//using DevExpress.XtraEditors;
//using DevExpress.XtraBars.Docking;
//using DevExpress.XtraBars.Docking2010.Views;
//using System.Xml;
//using System.Xml.Linq;
//using sql.builder.DataApi;
//namespace sql.builder
//{
//    internal partial class ucQueryEditorContextManager : XtraUserControl
//    {
//        public SortedList<string, ListPanel> Lists;
//        public string Info;
//        public ucQueryEditorContextManager()
//        {
//            this.Lists = new SortedList<string, ListPanel>();
//            this.Info = string.Empty;
//            this.InitializeComponent();
//        }
//        public void UpdateLists(VSXElement element)
//        {
//            IList<VContextListsType> allowed_types = element.ContextListAllowedTypes();
//            List<VContextListsInfo> lists = new List<VContextListsInfo>(allowed_types.Count);
//            List<string> newNames = new List<string>(allowed_types.Count);
//            foreach (VContextListsType listType in allowed_types) {
//                VContextListsInfo info = element.GetContextListInfo(listType);
//                //VDataTable table = element.GetContextListContent(listType);
//                //table.TableName = info.Title;
//                lists.Add(info);
//                newNames.Add(info.Title);
//            }
//            foreach (string name in this.Lists.Keys.Where(n=>!newNames.Contains(n)).ToList()) {
//                this.documentManager1.View.RemoveDocument(Lists[name].List);
//                this.Lists[name].List.Deactivate();
//            }
//            foreach (VContextListsInfo list in lists) {
//                this.UpdateList(list);
//            }
//        }
//        public event XElementEventHandler ItemSelected;
//        public void RaiseItemSelected(List<XElement> elements)
//        {
//            if (this.ItemSelected != null) {
//                this.ItemSelected(this, new XElementEventArgs(elements));
//            }
//        }
//        private void UpdateList(VContextListsInfo tlist)
//        {
//            //if (tlist.Columns.Count == 0) return;
//            ucQueryEditorContextList list = null;
//            ListPanel list_panel;
//            if (this.Lists.TryGetValue(tlist.Title, out list_panel)) {
//                list = list_panel.List;
//                // Lists[tlist.TableName].Panel.Form.Show();
//                //Lists[tlist.TableName].Panel.Visibility = DockVisibility.Visible;
//            } else {
//                list = new ucQueryEditorContextList();
//                list.Manager = this;
//                list.Dock = DockStyle.Fill;
//                list_panel = new ListPanel(list, tlist.Title);
//                this.Lists.Add(tlist.Title, list_panel);
//            }
//            if (documentManager1.GetDocument(list) == null) {
//                this.AddPanel(list_panel);
//            }
//            list.TryUpdateData(tlist);
//        }
//        private void AddPanel(ListPanel panelInfo)
//        {
//            BaseDocument doc = documentManager1.View.AddDocument(panelInfo.List);
//            panelInfo.List.InitializeOnParentChange();
//            doc.Caption = panelInfo.Title;
//            doc.Properties.AllowClose = DevExpress.Utils.DefaultBoolean.False;
//            doc.Properties.AllowFloat = DevExpress.Utils.DefaultBoolean.False;
//        }
//        internal class ListPanel
//        {
//            private ucQueryEditorContextList list;
//            private string title;
//            public ListPanel(ucQueryEditorContextList list, string title)
//            {
//                this.list = list;
//                this.title = title;
//            }
//            public string Title { get { return this.title; } }
//            public ucQueryEditorContextList List { get { return this.list; } }
//        }
//    }
//}