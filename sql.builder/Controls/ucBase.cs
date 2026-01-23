//using System;
//using System.Collections.Generic;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using DevExpress.Utils;
//using DevExpress.Utils.Taskbar.Core;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Docking2010;
//using DevExpress.XtraBars.Docking2010.Views.Tabbed;
//using DevExpress.XtraBars.Ribbon;
//using DevExpress.XtraEditors;
//using sql.builder.DataApi;
//using sql.builder.WinForms;
//using DevExpress.XtraBars.Docking2010.Views;

//using infoenergo;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;

//namespace sql.builder
//{
//    internal partial class ucBase : XtraUserControl
//    {
//        public ucBase()
//        {
//            InitializeComponent();

//            this.ParentChanged += OnParentChanged;
//            this.Enter += OnBaseEnter;
//            this.Disposed += OnDisposed;

//            DoubleBuffered = true;
//            //this.VisibleChanged += OnEnter;
//        }

//        public bool Multiple = true;

//        public virtual void Initialize() { }
//        #region TabbedLayout
//        public static DocumentGroup AddTab(TabbedView tabbedView, Control control, string title, DocumentGroup group = null)
//        {
//            if (group == null)
//            {
//                group = new DocumentGroup();
//            }


//            tabbedView.DocumentGroups.Add(group);
//            var doc = (Document)tabbedView.AddDocument(control);
//            doc.Caption = title;

//            tabbedView.Controller.Dock(doc, group, 0);
//            doc.Properties.AllowClose = DefaultBoolean.False;
//            doc.Properties.AllowFloat = DefaultBoolean.False;

//            doc.Properties.AllowDock = DefaultBoolean.True;

//            if (control is ucBase)
//            {
//                (control as ucBase).OnParentChanged(null, null);
//            }

//            return group;
//        }

//        public static XElement GetTabbedViewLayoutAsXml(TabbedView tabbedView)
//        {
//            // MemoryStream stream = new MemoryStream();
//            //tabbedView.SaveLayoutToXml(stream);
//            //XDocument xdoc = XDocument.Load(stream);
//            //return xdoc.Root;
//            XElement xlayout = new XElement(TextConst.EName.Layout);

//            foreach (DocumentGroup group in tabbedView.DocumentGroups)
//            {
//                XElement xgroup = new XElement(TextConst.EName.TabGroupLayout);
//                xlayout.Add(xgroup);
//                xgroup.SetAttributeValue(TextConst.AName.Size, group.GroupLength);

//                foreach (Document doc in group.Items)
//                {
//                    XElement xdoc = new XElement(TextConst.EName.TabLayout);
//                    xgroup.Add(xdoc);

//                    string controlId = getControlIdForTabbedView(doc.Control);

//                    xdoc.SetAttributeValue(TextConst.AName.Name, controlId);
//                    xdoc.SetAttributeValue(TextConst.AName.Title, doc.Caption);

//                }

//            }
//            return xlayout;

//        }
//        public static void SetTabbedViewLayoutFromXml(TabbedView tabbedView, XElement xlayout)
//        {
//            tabbedView.BeginUpdate();

//            var controls = getTabbedViewAllControls(tabbedView);
//            tabbedView.DocumentGroups.Clear();
//            //tabbedView.Documents.Clear();
//            var groups = new List<DocumentGroup>();

//            foreach (XElement xgroup in xlayout.Elements("tabgrouplayout"))
//            {
//                DocumentGroup group = null;
//                foreach (XElement xdoc in xgroup.Elements().Reverse())
//                {
//                    var control = controls.First(c => getControlIdForTabbedView(c) == xdoc.Attribute(TextConst.AName.Name).Value);
//                    if (group == null)
//                    {
//                        group = AddTab(tabbedView, control, xdoc.Attribute(TextConst.AName.Title).Value);
//                        groups.Add(group);
//                    }
//                    else
//                    {
//                        AddTab(tabbedView, control, xdoc.Attribute(TextConst.AName.Title).Value, group);
//                    }
//                }
//            }

//            int i = 0;
//            foreach (XElement xgroup in xlayout.Elements("tabgrouplayout"))
//            {
//                groups[i].GroupLength = Convert.ToInt32(xgroup.Attribute(TextConst.AName.Size).Value);
//                i++;
//            }

//            tabbedView.EndUpdate();
//        }

//        private static List<Control> getTabbedViewAllControls(TabbedView tabbedView)
//        {

//            return tabbedView.Documents.Cast<Document>()
//                    .Select(d => d.Control).ToList();

//        }

//        public virtual string ControlIdForTabbedView()
//        {

//            return this.Name;
//        }

//        private static string getControlIdForTabbedView(Control control)
//        {
//            if (control is ucBase)
//            {
//                return (control as ucBase).ControlIdForTabbedView();
//            }
//            else
//            {
//                return control.Name;
//            }
//        }
//        #endregion

//       // public List<Control> ControlsWithAttachedEvents = new List<Control>();

//        public bool IsTabbedVisible()
//        {
//            ucBase control = this;
//            DocumentContainer ctr = control.SearchDocumentContainer();
//            while (true)
//            {
//                if (ctr == null)
//                {
//                    return true;
//                }
//                else
//                {
//                    // Емцов - валилось. если ctr.Document == null или ctr.Document.Parent пока что вернет false
//                    if (ctr.Document == null) return false;
//                    if (((Document)ctr.Document).Parent == null) return false;

//                    if (((Document)ctr.Document).Parent.SelectedDocument == ctr.Document)
//                    {
//                        ctr = ucBase.SearchControlDocumentContainer(ctr.Parent);
//                    }
//                    else
//                    {
//                        return false;
//                    }
//                }
//            }
//        }

//        private bool wasShown = false;

//        public void OnBaseEnter(object sender, EventArgs e)
//        {

//            foreach (ucBase control in GetRibbonSource().BroControls.Values)
//            {
//                if (!control.IsTabbedVisible())
//                {
//                    control.Deactivate();
//                }
//            }


//			if (this.RibbonOwner == 0)
//			{
//				OnParentChanged(null, null);
//			}
//            if (this.RibbonOwner != 0)
//            {
//                SetCurrentControl(this);

//                RaiseFirstEnter();
//                GetRibbonSource().ShowRibbon();
//                UpdateRibbon();
//            }


//            foreach (ucBase control in ChildControls.Values)
//            {


//                if (control.IsTabbedVisible())
//                {
//                    control.OnBaseEnter(null, null);
//                }

//            }
            
//            //SearchParent();
//        }




//        public void Deactivate()
//        {

//            if (GetCurrentControl() == null) return;
//            if (GetCurrentControl().GetHashCode() == this.GetHashCode())
//            {
//                OnDeactivate();
//            }

//            foreach (ucBase control in ChildControls.Values)
//            {

//                control.Deactivate();
//            }

//        }

//        public virtual void UpdateRibbon()
//        {

//        }
//        bool _enterProcessing = false;
//        public void RaiseFirstEnter()
//        {
//            if (_enterProcessing) return;
//            _enterProcessing=true;
//            OnEnter();
//            if (!wasShown)
//            {
//                OnFirstEnter();
//                wasShown = true;
//            }
//            _enterProcessing = false;
//        }
//        public virtual void OnFirstEnter()
//        {

//        }

//        public virtual void OnEnter()
//        {

//        }

//        public virtual void OnDeactivate()
//        {

//        }

//        public void OnDisposed(object sender, EventArgs e)
//        {
//            if (GetCurrentControl() == this)
//            {
//                OnDeactivate();
//            }
//            if (ParentControl != null)
//            {
//                if (ParentControl.ChildControls.ContainsKey(this.GetHashCode()))
//                {
//                    ParentControl.ChildControls.Remove(this.GetHashCode());
//                }
//            }


//            if (GetRibbonSource() != null)
//            {

//                if (GetRibbonSource().BroControls.ContainsKey(this.GetHashCode()))
//                {
//                    GetRibbonSource().BroControls.Remove(this.GetHashCode());
//                }
//                if (GetRibbonSource().BroControls.Count == 0 || GetCurrentControl() == this)
//                {
//                    GetRibbonSource().HideRibbon();

//                    SetCurrentControl(null);
//                }



//            }

//            //if(XmlReports.IsInfoenergo) GlobalValues.MainForm.DocumentManager.DocumentActivate -= MainForm_DocumentActivate;
//        }


//        private bool ribbonHidden = false;
//        public void HideRibbon()
//        {


//            RibbonPageGroup group = null;
//            foreach (BarItemInfo item in barItems)
//            {
//                group = item.Group;
//                item.Link.Visible = false;
//            }

//            if (group != null)
//            {
//                UpdateRibbonVisibility(group.Ribbon);
//            }
//            ribbonHidden = true;
//        }

//        public void ShowRibbon()
//        {
//            if (!ribbonHidden) return;
//            RibbonPageGroup group = null;
//            foreach (BarItemInfo item in barItems)
//            {
//                group = item.Group;
//                item.Link.Visible = true;
//            }

//            if (group != null)
//            {
//                UpdateRibbonVisibility(group.Ribbon);
//            }
//            ribbonHidden = false;
//        }

//        private void UpdateRibbonVisibility(RibbonControl ribbon)
//        {
//            foreach (RibbonPage page in ribbon.Pages)
//            {
//                foreach (RibbonPageGroup group in page.Groups)
//                {
//                    // Емцов rpgDebug не трогаем
//                    if (group.Name == "rpgDebug") continue;

//                    bool canBeVisible = IsGroupCanBeVisible(group);
//                    if (canBeVisible)
//                    {

//                        group.Visible = (group.ItemLinks.Any(e => e.Visible));
//                    }
//                }
//            }

//            //int visibleGroupsCount = 0;
//            //foreach (var pageGroup in page.Groups.Cast<RibbonPageGroup>().ToList())
//            //{
//            //    if (pageGroup.ItemLinks.Cast<BarItemLink>().Any(e => e.Visible))
//            //    {
//            //        visibleGroupsCount++;
//            //        pageGroup.Visible = true;
//            //    }
//            //    else
//            //    {
//            //        pageGroup.Visible = false;
//            //    }
//            //}

//            //if (visibleGroupsCount > 0)
//            //{
//            //    // page.Visible = true;  
//            //}
//            //else
//            //{
//            //    //page.Visible = false;
//            //}
//            //}
//        }

//        public bool IsRibbonOnly;

//        public void OnParentChanged(object sender, EventArgs e)
//        {
//            InitializeOnParentChange();
//        }
//        public void InitializeOnParentChange()
//        {
//            //return;

//            ucBase newParentControl = SearchParent();
//            if (ParentControl != newParentControl)
//            {
//                if (ParentControl != null)
//                {
//                    if (ParentControl.ChildControls.ContainsKey(this.GetHashCode()))
//                    {
//                        ParentControl.ChildControls.Remove(this.GetHashCode());
//                    }
//                }
//                ParentControl = newParentControl;
//                if (ParentControl != null)
//                {
//                    if (!ParentControl.ChildControls.ContainsKey(this.GetHashCode()))
//                    {
//                        ParentControl.ChildControls.Add(this.GetHashCode(), this);
//                    }
//                }
//            }

//            //Бельченко: вместо этого пока сделал  вызов InitializeOnParentChange в нужных местах "вручную"
//            // это заплатка т.к. обработан только частный случай

//            ////  мегаутечка
//            //foreach (Control control in ControlsWithAttachedEvents)
//            //{

//            //    control.ParentChanged -= OnParentChanged;

//            //}
//            //ControlsWithAttachedEvents.Clear();

//            //Control control1 = this.Parent;
//            //while (control1 != null)
//            //{

//            //    control1.ParentChanged += OnParentChanged;
//            //    ControlsWithAttachedEvents.Add(control1);
//            //    control1 = control1.Parent;

//            //}



            

//            if (SearchForm() == null)
//            {
//                return;
//            }


//            if (this is ucQueryEditorContextList)
//            {

//            }


//            bool isNewRibbon = false;




//            RibbonControl mainRib = SearchTopRibbon();

//            RibbonControl rib = null;

//            if (mainRib != null)
//            {

//                if (mainRib.Parent != this)
//                {
//                    ucBase oldRibbonSource = RibbonSource;
//                    RibbonSource = GetRibbonSourceByType(mainRib.Parent.GetHashCode(), this.GetType().Name);

//                    if (RibbonSource == null)
//                    {

//                        if (Multiple)
//                        {
//                            RibbonSource = (Activator.CreateInstance(this.GetType()) as ucBase);
//                            RibbonSource.IsRibbonOnly = true;

//                            // из infoenergo.exe запускается во вкладке
//                            if (XmlReports.IsInfoenergo && (RibbonSource.SearchRibbon() != null))
//                            {
//                                // подписываемся только для RibbonSource каждого класса
//                                GlobalValues.MainForm.DocumentManager.View.DocumentActivated += RibbonSource.MainForm_DocumentActivate;
//                                GlobalValues.MainForm.DocumentManager.View.DocumentDeactivated += RibbonSource.MainForm_DocumentDeactivate;
//                                GlobalValues.MainForm.DocumentManager.View.DocumentClosing += RibbonSource.MainForm_DocumentClosing;
//                                RibbonsVisible[GetType()] = true;
//                            }
//                        }
//                        else
//                        {
//                            RibbonSource = this;
//                        }
//                        SetRibbonSourceByType(mainRib.Parent.GetHashCode(), this.GetType().Name, RibbonSource);

//                        rib = RibbonSource.SearchRibbon();

//                        RibbonSource.RibbonOwner = mainRib.Parent.GetHashCode();
//                        //RibbonSourcesByType.Add(this.GetType().Name, ribSource);
//                        if (rib != null)
//                        {
//                            CopyRibbon(RibbonSource, rib, mainRib);
//                        }
//                        isNewRibbon = true;
//                    }

//                    if (oldRibbonSource != RibbonSource)
//                    {
//                        isNewRibbon = true;
//                    }


//                }
//                else
//                {
//                    if (RibbonSource != this)
//                    {
//                        RibbonSource = this;
//                        isNewRibbon = true;
//                    }

//                }
//                //}
//                //else
//                //{
//                //    this.SearchRibbon().Visible = false;
//                //}
//                rib = this.SearchRibbon();
//                //MessageBox.Show("0");
//                if (rib != null && rib != mainRib)
//                {
//                    rib.Visible = false;
//                }

//				RibbonOwner = mainRib.Parent.GetHashCode();
//                GetRibbonSource().ShowRibbon();
//                //MessageBox.Show("1");
//                if (!GetRibbonSource().BroControls.ContainsKey(this.GetHashCode()))
//                {
//                    GetRibbonSource().BroControls.Add(this.GetHashCode(), this);
//                }
//                //MessageBox.Show("2");
//                if (GetCurrentControl() == null)
//                {
//                    SetCurrentControl(this);
//                }

//                if (isNewRibbon)
//                {
//                    AfterChangeRibbon();
//                }
//            }

//        }

//        protected virtual void AfterChangeRibbon()
//        {

//        }



//        public void CopyRibbon(ucBase ribSource, RibbonControl source, RibbonControl target)
//        {
//            var ignorable = GetIgnorablePageTexts().ToArray();
//            foreach (RibbonPage page in source.Pages.Cast<RibbonPage>().ToArray())
//            {
//                if (ignorable.Contains(page.Text)) continue;

//                RibbonPage tpage = target.Pages.GetPageByText(page.Text);
//                bool moveAllPage = false;
//                if (tpage == null)
//                {
//                    tpage = page;
//                    target.Pages.Add(tpage);
//                    moveAllPage = true;
//                }


//                foreach (RibbonPageGroup pageGroup in page.Groups.Cast<RibbonPageGroup>().ToArray())
//                {
//                    RibbonPageGroup tpageGroup = null;
//                    if (moveAllPage)
//                    {
//                        tpageGroup = pageGroup;

//                    }
//                    else
//                    {
//                        tpageGroup = tpage.Groups.GetGroupByText(pageGroup.Text);
//                    }
//                    bool moveAllGroup = false;
//                    if (tpageGroup == null)
//                    {
//                        tpageGroup = pageGroup;
//                        tpage.Groups.Add(tpageGroup);
//                        moveAllGroup = true;
//                    }
//                    foreach (BarItemLink titemLink in pageGroup.ItemLinks.ToArray())
//                    {
//                        BarItem titem = titemLink.Item;
//                        if (!moveAllGroup && !moveAllPage)
//                        {
//                            target.Items.Add(titem);
//                            tpageGroup.ItemLinks.Add(titem);
//                        }
//                        ribSource.barItems.Add(new BarItemInfo(titemLink, pageGroup));
//                    }

//                }
                

//            }

//            var page_text = GetDefaultPageText();
//            if (page_text != null)
//            {
//                var page = target.Pages.GetPageByText(page_text);
//                if (page != null) target.SelectedPage = page;
//            }
//        }
//        public void SetCurrentControl(ucBase control)
//        {
//            GetRibbonSource().CurrentControl = control;
//            //SetCurrentControlByType(this.RibbonOwner, this.GetType().Name, control);
//        }
//        private ucBase GetCurrentControl()
//        {
//            return this.GetRibbonSource().CurrentControl;
//        }
//        // Все обращения из рибона д. быть через этот метод
//        protected T GetCurrentControl<T>()
//            where T : ucBase  
//        {
//            return (this.GetCurrentControl() as T);
//        }
//        private ucBase GetRibbonSource()
//        {
//            if (this.RibbonSource != null) {
//                return this.RibbonSource;
//            } else {
//                return this;
//            }
//        }
//        // Все обращения к контролам рибона д. быть через этот метод
//        protected T GetRibbonSource<T>()
//            where T : ucBase
//        {
//            return (this.GetRibbonSource() as T);
//        }
//        private RibbonControl SearchRibbon()
//        {
//            for (int index = 0; index < this.Controls.Count; index++) {
//                RibbonControl ribbon = this.Controls[index] as RibbonControl;
//                if (ribbon != null) {
//                    return ribbon;
//                }
//            }
//            return null;
//        }        
//        public RibbonControl SearchTopRibbon()
//        {
//            Control parent = null;

//            if (UIStatic.RibbonParent != null)
//            {
//                parent = (Control)UIStatic.RibbonParent;
//            }
//            else if (XmlReports.IsInfoenergo)
//            {
//                parent = GlobalValues.MainForm as Control;
//            }
//            else
//            {
//                parent = this;
//                while (parent.Parent != null) parent = parent.Parent;
//            }

//            return Cmn.GetChildControlsOfType<RibbonControl>(parent).FirstOrDefault();

//            ////return rib;
//            //var controls = new Dictionary<int, Control>();

//            //Control control1 = this;
//            //int i = 0;
//            //while (control1 != null)
//            //{
//            //    controls.Add(i, control1);
//            //    control1 = control1.Parent;
//            //    i++;
//            //}

//            //RibbonControl rib = null;
//            //foreach (Control control2 in controls.OrderByDescending(e => e.Key).Select(e1 => e1.Value))
//            //{
//            //    rib = (RibbonControl)control2.Controls.Cast<Control>().FirstOrDefault(e => e is RibbonControl);
//            //    if (rib != null)
//            //    {
//            //        break;
//            //        //return rib;
//            //    }
//            //}


//            //var t = (rib == rib2);

//            //return rib;
//        }



//        public Form SearchForm()
//        {
//            Control control = this.Parent;

//            while (control != null)
//            {

//                if (control is Form)
//                {
//                    return (Form)control;
//                }
//                control = control.Parent;
//            }
//            return null;

//        }





//        public DocumentContainer SearchDocumentContainer()
//        {


//            Control control = this.Parent;

//            return SearchControlDocumentContainer(control);
           

//        }

//        public static DocumentContainer SearchControlDocumentContainer(Control control)
//        {


            

//            while (control != null)
//            {

//                if (control is DocumentContainer)
//                {
//                    return ((DocumentContainer)control);
//                }
//                control = control.Parent;
//            }
//            return null;

//        }

//        public ucBase SearchParent()
//        {

//            Control control = this.Parent;

//            while (control != null)
//            {

//                if (control is ucBase)
//                {
//                    return (ucBase)control;
//                }
//                control = control.Parent;
//            }
//            return null;

//        }

//        public static void SetCClistValue(Dictionary<int, Dictionary<string, ucBase>> list, int main, string typeName, ucBase control)
//        {
//            if (!list.ContainsKey(main))
//            {
//                list.Add(main, new Dictionary<string, ucBase>());
//            }

//            if (!list[main].ContainsKey(typeName))
//            {
//                list[main].Add(typeName, control);
//            }
//            list[main][typeName] = control;
//        }


//        public static ucBase GetCClistValue(Dictionary<int, Dictionary<string, ucBase>> list, int main, string typeName)
//        {
//            if (!list.ContainsKey(main))
//            {
//                return null;
//            }

//            if (!list[main].ContainsKey(typeName))
//            {
//                return null;
//            }
//            return list[main][typeName];
//        }

//        public int RibbonOwner = 0;
//        //public static void SetCurrentControlByType(int  main, string typeName, ucBase control)
//        //{
//        //    SetCClistValue(CurrentControlsByType, main, typeName, control);
//        //}

//        public static void SetRibbonSourceByType(int main, string typeName, ucBase control)
//        {
//            SetCClistValue(RibbonSourcesByType, main, typeName, control);
//        }

//        //public static ucBase GetCurrentControlByType(int main, string typeName)
//        //{
//        //    return GetCClistValue(CurrentControlsByType, main, typeName);
//        //}

//        public static ucBase GetRibbonSourceByType(int main, string typeName)
//        {
//            return GetCClistValue(RibbonSourcesByType, main, typeName);
//        }

//        //private static Dictionary<int, Dictionary<string, ucBase>> CurrentControlsByType = new Dictionary<int, Dictionary<string, ucBase>>();

//        private static Dictionary<int, Dictionary<string, ucBase>> RibbonSourcesByType = new Dictionary<int, Dictionary<string, ucBase>>();

//        private List<BarItemInfo> barItems = new List<BarItemInfo>();
//        private Dictionary<int, ucBase> BroControls = new Dictionary<int, ucBase>();
//        private ucBase CurrentControl = null;
//        private ucBase RibbonSource = null;
//        private ucBase ParentControl = null;
//        private Dictionary<int, ucBase> ChildControls = new Dictionary<int, ucBase>();
        

//        internal enum ucBaseEventType
//        {
//            None,
//            ExecuteStart,
//            ExecuteComplete,
//            Exception
//        }
//        public void OnBaseEvent(ucBaseEventType event_type, string info = null)
//        {
//            Form form = FindForm();
//            switch (event_type)
//            {
//                case ucBaseEventType.ExecuteStart:
//                    WaitUIHelper.LastUsedUIHelper.Show(info, WaitUIMode.WaitPanel, true);
//                    //Wait.Show(info, over:true, form: form);

//                    // текущая версия devexpress на XP дает exception
//                    // https://www.devexpress.com/Support/Center/Question/Details/T285814
//                    if (Cmn.OSWin7AndNewer())
//                    {
//                        var frm = form as frmBaseSqlBuilder;
//                        if (frm != null)
//                        {
//                            frm.TaskBarAssistent.ProgressMode = TaskbarButtonProgressMode.Indeterminate;
//                        }
//                    }
//                    break;

//                case ucBaseEventType.ExecuteComplete:
//                    WaitUIHelper.LastUsedUIHelper.Hide();
//                    //Wait.Hide();
//                    // текущая версия devexpress на XP дает exception
//                    // https://www.devexpress.com/Support/Center/Question/Details/T285814
//                    if (Cmn.OSWin7AndNewer())
//                    {
//                        var frm = form as frmBaseSqlBuilder;
//                        if (frm != null)
//                        {
//                            frm.TaskBarAssistent.ProgressMode = TaskbarButtonProgressMode.NoProgress;
//                        }
//                    }
//                    break;

//                case ucBaseEventType.Exception:
//                    WaitUIHelper.LastUsedUIHelper.ForceHide();
//                    //Wait.ForceHide();
//                    // текущая версия devexpress на XP дает exception
//                    // https://www.devexpress.com/Support/Center/Question/Details/T285814
//                    if (Cmn.OSWin7AndNewer())
//                    {
//                        var frm = form as frmBaseSqlBuilder;
//                        if (frm != null)
//                        {
//                            frm.TaskBarAssistent.ProgressMode = TaskbarButtonProgressMode.NoProgress;
//                        }
//                    }
//                    break;
                        
//            }
//        }

//        private void ucBase_Load(object sender, EventArgs e)
//        {
//            //
//        }

//        public virtual void SaveState()
//        {
//        }
//        public virtual void RestoreState()
//        {
//        }

//        private void MainForm_DocumentDeactivate(object sender, DocumentEventArgs args)
//        {
//            UpdateInfoenergoRibbon(args.Document.Control, false);
//            //if (args.Document.Control is frmMain)  = new SqlReportsContextGroup();

//            //if (args.Document.Control != null)
//            //{
//            //    Cmn.GetChildControlsOfType<UIFormC>(args.Document.Control).Where(f => f.FormUseType == UIFormC.UseType.DataEditor).ForEach(f => f.RefreshData());
//            //}
//        }
//        private void MainForm_DocumentActivate(object sender, DocumentEventArgs args)
//        {
//            UpdateInfoenergoRibbon(args.Document.Control, true);

//            //if (args.Document.Control != null)
//            //{
//            //    Cmn.GetChildControlsOfType<UIFormC>(args.Document.Control).Where(f => f.FormUseType == UIFormC.UseType.DataEditor).ForEach(f => f.RefreshData());
//            //}
//        }
//        private void MainForm_DocumentClosing(object sender, DocumentCancelEventArgs args)
//        {
//            //UpdateInfoenergoRibbon(false);
//            //if (args.Document.Control != null)
//            //{
//            //    Cmn.GetChildControlsOfType<UIFormC>(args.Document.Control).Where(f => f.FormUseType == UIFormC.UseType.DataEditor).ForEach(f => f.RefreshData());
//            //}

//            // наивно полагаю, что ближайший контрол и будет самой верхней формой и достаточно проверить ее
//            // старые контролы форм
//            var uiformCtrl_unsaved = Cmn.GetChildControlsOfType<sql.builder.UI.WinForms.UIFormControl>(args.Document.Control).FirstOrDefault(f => f.GetVForm().FormUseType == UIFormC.UseType.DataEditor && f.GetVForm().IsModifiedSelfOrSub());

//            UIFormC uiform_unsaved = null;
//            if (uiformCtrl_unsaved!=null)
//            {
//                uiform_unsaved = uiformCtrl_unsaved.GetVForm();
//            }
//            //if (uiform_unsaved == null)
//            //{
//            //    // новые контролы форм
//            //    var control = Cmn.GetChildControlsOfType<UIFormC2Control>(args.Document.Control).FirstOrDefault(f => f.Form.FormUseType == UIFormC.UseType.DataEditor && f.Form.IsModifiedSelfOrSub());
//            //    if (control != null) uiform_unsaved = control.Form;
//            //}

//            // если есть несохраненные изменения - спрашиваем
//            if (uiform_unsaved != null)
//            {
//                var result = ShowMessage.Show(ShowMessage.MType.UnsavedChangesQuestion);
//                if (result == DialogResult.Cancel)
//                {
//                    args.Cancel = true;
//                    return;
//                }
//                else if (result == DialogResult.Yes)
//                {
//                    if (!uiform_unsaved.SaveData(false))
//                    {
//                        args.Cancel = true;
//                        return;
//                    }
//                }
//            }
//        }

//        static Dictionary<Type, bool> RibbonsVisible = new Dictionary<Type, bool>();
//        static List<RibbonPage> InvisiblePages = new List<RibbonPage>();
//        static List<RibbonPageGroup> InvisibleGroups = new List<RibbonPageGroup>();
//        static List<BarItemLink> InvisibleLinks = new List<BarItemLink>();
//        void UpdateInfoenergoRibbon(Control root, bool visible)
//        {
//            // проверяем, содержит ли активированая вкладка видимые контролы текущего типа
//            //var method = typeof(Cmn).GetMethod("GetChildControlsOfType");
//            //var generic = method.MakeGenericMethod(GetType());
//            //var ctrls = (generic.Invoke(null, new object[] { root, true }) as IEnumerable<ucBase>).ToArray();
//            List<ucBase> ctrls = Cmn.GetChildControlsOfType<ucBase>(root, true).ToList();
//            if (ctrls.Count > 0) {
//                if (visible) {
//                    if (!RibbonsVisible[GetType()]) {
//                        ChangeRibbonItemsVisible(true);
//                        RibbonsVisible[GetType()] = true;
//                    }
//                    var page_text = GetDefaultPageText();
//                    if (page_text != null) {
//                        var mainRib = SearchTopRibbon();
//                        var page_default = mainRib.Pages.GetPageByText(page_text);
//                        if (page_default != null) mainRib.SelectedPage = page_default;
//                    }
//                } else {
//                    if (!RibbonsVisible[GetType()]) return;
//                    ChangeRibbonItemsVisible(false);
//                    RibbonsVisible[GetType()] = false;
//                    var mainRib = SearchTopRibbon();
//                    if (mainRib.SelectedPage == null && mainRib.Pages.Count > 0) mainRib.SelectedPage = mainRib.Pages[0];
//                }
//            }
//            foreach (var c in ctrls) {
//                c.UpdateRibbon();
//            }
//        }
//        private static void SetDefaultGroupVisibility(RibbonPageGroup group)  // химия, чтобы невидимые по умолчанию группы не показывались, толком не проверено
//        {
//            if (group.Tag == null)
//            {
//                if (group.Visible)
//                {
//                    group.Tag = 1;

//                }
//                else
//                {
//                    group.Tag = 0;
//                }
//            }
//        }

//        private static int GetDefaultGroupVisibility(RibbonPageGroup group)
//        {
//           return (int)group.Tag;
//        }

//        private static bool IsGroupCanBeVisible(RibbonPageGroup group)
//        {
//            SetDefaultGroupVisibility(group);
//            bool canBeVisible = group.Visible;
//            if (!canBeVisible)
//            {
//                if (GetDefaultGroupVisibility(group) == 1)
//                {
//                    canBeVisible = true;
//                }
//            }
//            return canBeVisible;
//        }
            
           

//        void ChangeRibbonItemsVisible(bool visible)
//        {
//            var mainRib = SearchTopRibbon();
//            var rib = SearchRibbon();

//            mainRib.Manager.BeginUpdate();
            
//            foreach (RibbonPage page in rib.Pages)
//            {
//                var mpage = mainRib.Pages.GetPageByText(page.Text);
//                if (mpage == null) continue;
//                bool anyVisible = false;
//                foreach (RibbonPageGroup group in page.Groups)
//                {
//                    bool canBeVisible = IsGroupCanBeVisible(group);
                    

                    
//                    var mgroup = mpage.Groups.GetGroupByText(group.Text);
//                    if (mgroup == null) continue;

//                    foreach (BarItemLink link in group.ItemLinks)
//                    {
//                        var mlink = mgroup.ItemLinks.FirstOrDefault(l => l.Caption == link.Caption);
//                        if (mlink == null) continue;

//                        if (!visible && !mlink.Visible) InvisibleLinks.Add(mlink);
//                        else if (visible && InvisibleLinks.Contains(mlink)) InvisibleLinks.Remove(mlink);
//                        else mlink.Visible = visible;
//                    }

//                    if (!canBeVisible)
//                    {
//                        visible = false;
//                    }
//                    if (visible)
//                    {
//                        anyVisible = true;
//                        if (mgroup.ItemLinks.Any(l => l.Visible))
//                        {
//                            if (InvisibleGroups.Contains(mgroup)) InvisibleGroups.Remove(mgroup);
//                            else mgroup.Visible = true;
//                        }
//                    }
//                    else
//                    {
//                        if (mgroup.ItemLinks.All(l => !l.Visible))
//                        {
//                            if(!mgroup.Visible) InvisibleGroups.Add(mgroup);
//                            else mgroup.Visible = false;
//                        }
//                    }
//                }


//                if (anyVisible)
//                {
//                    if (mpage.Groups.Cast<RibbonPageGroup>().Any(g => g.Visible))
//                    {
//                        if (InvisiblePages.Contains(mpage)) InvisiblePages.Remove(mpage);
//                        else mpage.Visible = true;
//                    }
//                }
//                else
//                {
//                    if (mpage.Groups.Cast<RibbonPageGroup>().All(g => !g.Visible))
//                    {
//                        if (!mpage.Visible) InvisiblePages.Add(mpage);
//                        else mpage.Visible = false;
//                    }
//                }
//            }


//            mainRib.Manager.EndUpdate();
//        }
//        protected virtual string GetDefaultPageText()
//        {
//            return null;
//        }
//        protected virtual IEnumerable<string> GetIgnorablePageTexts()
//        {
//            return Enumerable.Empty<string>();
//        }

//        //protected override CreateParams CreateParams
//        //{
//        //    get
//        //    {
//        //        var handleParam = base.CreateParams;
//        //        handleParam.ExStyle |= 0x02000000;   // WS_EX_COMPOSITED       
//        //        return handleParam;
//        //    }
//        //}

//        public static void SearchRibbonButton(Type type, string button_name)
//        {
            
//        }
//    }

//    internal class BarItemInfo
//    {
//        public BarItemInfo(BarItemLink link, RibbonPageGroup group)
//        {
//            Link = link;
//            Group = group;
//        }
//        public BarItemLink Link;
//        public RibbonPageGroup Group;
//    }
//}
