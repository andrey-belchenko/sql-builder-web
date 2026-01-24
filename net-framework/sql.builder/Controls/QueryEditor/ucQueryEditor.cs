//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
////using System.Windows.Forms;
//using System.Xml;
//using System.Xml.Linq;
//using DevExpress.Xpo.DB.Helpers;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Docking2010;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;

//using DevExpress.XtraTreeList;
//using DevExpress.XtraTreeList.Columns;
//using DevExpress.XtraTreeList.Nodes;
//using infoenergo.core.Extensions;
//using sql.builder.DataApi;
//using sql.builder.FieldInfo;
//using sql.builder.Properties;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
//using DevExpress.XtraEditors.Repository;
//using sql.builder.Test;

namespace sql.builder
{
    internal partial class ucQueryEditor //: ucBase
    {
        //        internal VSXElement Query;
        //        internal bool FreezeOperations { get; set; }
        //        private frmReplaceText _frm_replace_text;
        //        internal event FocusedNodeChangedEventHandler FocusedQueryChanged;
        //        internal event EventHandler NeedClose;
        //        internal event EventHandler NeedRefreshScheme;
        //        internal bool queryLoaded { get; private set; }
        //        internal VSXElement SelectTarget = null;
        //        public ucQueryEditor()
        //        {
        //            InitializeComponent();
        //            tlQueryScheme.OptionsView.AutoWidth = false;
        //            //TabbedView tabbedView = new TabbedView();
        //            //documentManager1.ViewCollection.Add(tabbedView);
        //            tlQueryScheme.MouseClick += tlQueryScheme_OnMouseClick;
        //            tabbedView1.DocumentGroups.Clear();
        //            AddTab(tabbedView1, pcProps, "Свойства узла");
        //            var gr = AddTab(tabbedView1, clUses, "Менеджер ссылок");
        //            AddTab(tabbedView1, contextManagers, "Списки контекстного добавления элементов", gr);
        //            AddTab(tabbedView1, tlQueryScheme, "Схема элемента", gr);
        //            InitUndoRedoFunctionality();
        //        }
        //        private void tlQueryScheme_OnMouseClick(object sender, MouseEventArgs args)
        //        {
        //            if (args.Button == MouseButtons.Right) {
        //                popupMenu.ShowPopup(tlQueryScheme.PointToScreen(args.Location));
        //            }
        //        }
        //        internal static string AddPathToFilename(string filename)
        //        {
        //            return VSXElement.AddPathToFilename(filename);
        //            //return Path.Combine(Cmn.SourcePath(), filename);
        //        }
        internal static string remPathFromFilename(string filename)
        {
            //return filename.Replace(Cmn.SourcePath(), "");
            return filename.Replace(XmlReports.GetRootPath() + "\\", "");
        }
        //        //public void OpenQuery(string queryName)
        //        //{
        //        //    if (Query != null)
        //        //    {
        //        //        return;
        //        //    }
        //        //    XElement query = XmlReports.environment.SchemeNative.Element("queries").Elements("query").Where(e1 => Cmn.GetAttrValue(e1, "name") == queryName).First();
        //        //   // XDocument xdoc = XDocument.Parse(Cmn.OpenText(fileName).Replace("xmlns=\"sqlbuilder\"", ""));
        //        //   // query = xdoc.Elements().First().Element("queries").Elements("query").Where(e1 => Cmn.GetAttrValue(e1, "name") == queryName).First();
        //        //    OpenQuery( query);
        //        //}
        //        //public XElement SchemeColumns = null;
        //        #region Undo/Redo
        //        List<XmlDelta> _query_history;
        //        int _operation = 0;
        //        int _current_index = -1;
        //        Timer _operation_timer;
        //        XElement _query_prev;
        //        private void InitUndoRedoFunctionality()
        //        {
        //            _query_history = new List<XmlDelta>();
        //            _operation = 0;
        //            _current_index = -1;

        //            _operation_timer = new Timer() { Interval = 1000 };
        //            _operation_timer.Tick += OperationTimerOnTick;
        //        }
        //        private void BeginOperation()
        //        {
        //            if (FreezeOperations) return;

        //            _operation++;
        //        }
        //        private void EndOperation()
        //        {
        //            _operation--;
        //            if (_operation == 0)
        //            {
        //                FixOperation();
        //            }
        //        }
        //        private void FixOperation()
        //        {
        //            if (_operation > 0 || FreezeOperations) return;

        //            SetChanged(true);// Бельченко. Доделал вроде

        //            _operation_timer.Stop();

        //            int tail_length = _query_history.Count - _current_index - 1;
        //            if (tail_length > 0)
        //            {
        //                _query_history.RemoveRange(_current_index + 1, tail_length);
        //            }

        //            XElement query_new = new XElement(Query);

        //            _query_prev.RemoveAttributes();
        //            query_new.RemoveAttributes();

        //            var delta = new XmlDelta(_query_prev, query_new);
        //            _query_history.Add(delta);
        //            // приходится держать в памяти последнее состояни Query
        //            // иначе не понятно с чем сравнивать на событие Query.Changed
        //            _query_prev = new XElement(Query);
        //            _current_index++;

        //            UpdateUndoRedoButtonsState();
        //        }
        //        private void FixOperationWithDelay()
        //        {
        //            if (_operation > 0 || FreezeOperations) return;

        //            SetChanged(true);// Бельченко. Доделал вроде

        //            _operation_timer.Stop();
        //            _operation_timer.Start();
        //        }
        //        private void OperationTimerOnTick(object sender, EventArgs args)
        //        {
        //            FixOperation();
        //        }
        //        public void Undo()
        //        {
        //            if (_current_index < 0) return;
        //            //_current_index--;

        //            RestoreQueryState(true);

        //            UpdateUndoRedoButtonsState();
        //        }
        //        public void Redo()
        //        {
        //            if (_current_index >= _query_history.Count - 1) return;
        //            //_current_index++;

        //            RestoreQueryState(false);

        //            UpdateUndoRedoButtonsState();
        //        }
        //        public void RestoreQueryState(bool undo)
        //        {
        //            FreezeOperations = true;

        //            // блокировка лишних событий
        //            tlQueryScheme.LockReloadNodes();
        //            tlQueryScheme.BeginUpdate();
        //            (tlQueryScheme.DataSource as VDataTable).SuppressChangeEvent();

        //            XElement query_new = new XElement(Query);
        //            query_new.RemoveAttributes();

        //            XElement prev = null;
        //            XElement query_hist = (undo)
        //                ? _query_history[_current_index].GetPrev(query_new)
        //                : _query_history[_current_index + 1].GetNext(query_new);

        //            foreach (var c in Query.Childs()) c.Delete();
        //            foreach (XElement el_hist in query_hist.Elements().ToArray())
        //            {
        //                // добавляем новый элемент вместо старого со всеми дочерними
        //                VSXElement vel_new = Query.InsertChild(el_hist, prev, true, false);
        //                prev = vel_new;
        //            }

        //            // восстанавливаем порядок
        //            Query.UpdateChildOrder();

        //            // разблокировка событий
        //            (tlQueryScheme.DataSource as VDataTable).UnsuppressChangeEvent();
        //            tlQueryScheme.EndUpdate();
        //            tlQueryScheme.UnlockReloadNodes();

        //            if (undo) _current_index--;
        //            else _current_index++;

        //            FreezeOperations = false;
        //        }
        //        private void UpdateUndoRedoButtonsState()
        //        {
        //            ucQueryEditor rib = this.GetRibbonSource<ucQueryEditor>();
        //            rib.btnUndoChange.Enabled = (_current_index >= 0);
        //            rib.btnRedoChange.Enabled = (_current_index < _query_history.Count - 1);
        //        }
        //        private void btnRedoChange_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            var ctrl = GetCurrentControl<ucQueryEditor>();
        //            if (!ctrl.tlQueryScheme.HasFocus) return;

        //            ctrl.Redo();
        //        }
        //        private void btnUndoChange_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            var ctrl = GetCurrentControl<ucQueryEditor>();
        //            if (!ctrl.tlQueryScheme.HasFocus) return;

        //            ctrl.Undo();
        //        }
        //        private class XmlDelta
        //        {
        //            private XElement deltaToPrev;
        //            private XElement deltaToNext;
        //            internal XmlDelta(XElement xmlOld, XElement xmlNew)
        //            {
        //                this.deltaToNext = XmlDiffHelper.GetDiff(xmlOld, xmlNew);
        //                this.deltaToPrev = XmlDiffHelper.GetDiff(xmlNew, xmlOld);
        //            }
        //            internal XElement GetNext(XElement xmlOld)
        //            {
        //                return XmlDiffHelper.ResoreXml(xmlOld, deltaToNext);
        //            }
        //            internal XElement GetPrev(XElement xmlNew)
        //            {
        //                return XmlDiffHelper.ResoreXml(xmlNew, deltaToPrev);
        //            }
        //        }
        //        #endregion
        //        private static bool _loading = false;
        //        private VEnvironment _environment = null;
        //        private void LoadQuery()
        //        {
        //            if (_loading) {
        //                return;
        //            }
        //            _loading = true;
        //            if (this.queryLoaded) {
        //                if (this._environment != XmlReports.Environment) {
        //                    bool refresh = true;
        //                    if (this.IsChanged()) {
        //                        if (ShowMessage.ShowQuestion("Схема была изменена. Перезагрузить?") != DialogResult.Yes) {
        //                            refresh = false;
        //                        } else {
        //                            this.SetChanged(false);
        //                        }
        //                    }
        //                    if (refresh) {
        //                        this._formCreated = false;
        //                        this.fieldGroupsProcessed = false;
        //                        XElement info = this.makeInfo();
        //                        this.DetachEvents();
        //                        this.Query = ucQueriesEditor.FindElementByInfo(XmlReports.Environment, info);
        //                        ucQueriesEditorItem.UpdateElementFileInfo(this.Query);
        //                        if (this.SelectTarget == null) {
        //                            this.SelectTarget = ucQueriesEditor.FindTargetByInfo(this.Query, info);
        //                        }
        //                    } else {
        //                        this.CheckSetSelection();
        //                        _loading = false;
        //                        return;
        //                    }
        //                } else {
        //                    this.CheckSetSelection();
        //                    _loading = false;
        //                    return;
        //                }
        //            }
        //            this.PerformLayout();
        //            this.pcProps.Focus();
        //            if (this.Query.Parent != null) {
        //                this.Query.ParentName = this.Query.Parent.Name.LocalName;
        //            }
        //            WaitUIHelper.LastUsedUIHelper.Show("Загрузка элемента", WaitUIMode.WaitPanel);
        //            //Wait.Show("Загрузка элемента");
        //            this.Query.Renamed += OnRenamed;
        //            DataTable dt = this.Query.AsDataTable();
        //            this.tlQueryScheme.DataSource = dt;
        //            this.Query.Changed += this.Query_Changed;
        //            //FixOperation();
        //            Cmn.AddHtmlEditors(this.tlQueryScheme, dt);
        //            //tlQueryScheme.Columns["id"].Visible = false;
        //            //tlQueryScheme.Columns["parent_id"].Visible = false;
        //            //tlQueryScheme.Columns["node"].Visible = false;
        //            //tlQueryScheme.Columns["ord"].Visible = false;
        //            int index;
        //            for (index = 0; index < this.tlQueryScheme.Columns.Count; index++) {
        //                TreeListColumn col = this.tlQueryScheme.Columns[index];
        //                col.Visible = false;
        //                col.OptionsColumn.AllowSort = false;
        //                col.OptionsColumn.AllowEdit = true;
        //            }
        //            IList<string> property_names = this.Query.GetPropNames();
        //            for (index = 0; index < property_names.Count; index++) {
        //                string name = property_names[index];
        //                tlQueryScheme.Columns[name].Visible = VFieldInfo.VisibleInTable(Query, VSXElement.PropPfx + name);
        //            }
        //            this.tlQueryScheme.Columns["NodeText"].ColumnEdit = repositoryItemRichTextEdit1;
        //            this.tlQueryScheme.Columns["NodeText"].OptionsColumn.AllowEdit = false;
        //            this.tlQueryScheme.Columns["ord"].SortOrder = SortOrder.Ascending;
        //            //if (SchemeColumns != null) SetSchemeColumnsSettings(SchemeColumns);
        //            //  this.documentGroup2.GroupLength = 150;// Почему то размеры из дизайнера не сохраняются
        //            this.showFileName();
        //            this.tlQueryScheme.ForceInitialize();
        //            this.CheckSetSelection();
        //            this.OpenLayout();
        //            this.queryLoaded = true;
        //            this.SetChanged(false);
        //            this.SaveState();
        //            WaitUIHelper.LastUsedUIHelper.Hide();
        //            //Wait.Hide();
        //            _loading = false;
        //            this._environment = XmlReports.Environment;
        //            // this.Visible = true;
        //            this.ResumeLayout();
        //            this._query_prev = new XElement(this.Query);
        //        }
        //        private void Query_Changed(object sender, XObjectChangeEventArgs e)
        //        {
        //            this.FixOperationWithDelay();
        //        }
        //        private void CheckSetSelection()
        //        {
        //            if (this.SelectTarget != null) {
        //                if (this.SelectTarget != this.Query) {
        //                    this.SetSelection(this.SelectTarget);
        //                }
        //            }
        //            this.SelectTarget = null;
        //        }
        //        public override void OnEnter()
        //        {
        //            this.LoadQuery();
        //        }
        //        //public void CreateQuery(VEnvironment env,string elementType, string templateName)
        //        //{
        //        //    if (Query != null)
        //        //    {
        //        //        return;
        //        //    }
        //        //    VSXElement qry = XmlReports.environment.CreateQuery(elementType,templateName);
        //        //    OpenQuery(qry,null);
        //        //}
        //        private bool processingSelection = false;
        //        private UIFormC GetUIForm()
        //        {
        //            if (this.pcProps.Controls.Count == 0) {
        //                return null;
        //            } else {
        //                return ((sql.builder.UI.WinForms.UIFormControl)pcProps.Controls[0]).GetVForm();
        //            }
        //        }
        //        private void UpdateSourceSelectedRows()
        //        {
        //            UIFormC frm = this.GetUIForm();
        //            if (frm == null) {
        //                return;
        //            }
        //            VDataTable tbl = frm.DataSource.ParamsTable;
        //            if (tbl == null) {
        //                return;
        //            }
        //            tbl.SelectedRows = this.tlQueryScheme.Selection.Cast<TreeListNode>().Select(Cmn.GetNodeRow).ToList();
        //        }
        //        private bool _formCreated;
        //        //private void SetCurrentRow(VSXElement el)
        //        //{
        //        //    var table = (tlQueryScheme.DataSource as VDataTable);
        //        //    table.CurrentRow = el.Row;
        //        //}
        //        //private void   RaiseCurrentRowChanged()
        //        //{
        //        //    var table = (tlQueryScheme.DataSource as VDataTable);
        //        //    table.RaiseCurrentRowChanged ();
        //        //}
        //        private SortedList<string, VLayoutGroupInfo> fieldgroups = new SortedList<string, VLayoutGroupInfo>();
        //        private SortedList<string, string> fldGrp = new SortedList<string, string>();
        //        private void ItemSelected(TreeListNode node)
        //        {
        //            object obj = node["node"];
        //            if (Cmn.IsNullOrDBNull(obj) || this.processingSelection) {
        //                return;
        //            }
        //            this.UpdateSourceSelectedRows();
        //            this.processingSelection = true;
        //            VSXElement el = (VSXElement)obj;
        //            UIFormC form;
        //            //var ftabs = new SortedList<string, XElement>();
        //            //var fgroups = new SortedList<string, XElement>();
        //            if (!this._formCreated) {
        //                this._formCreated = true;
        //                this.fieldGroupsProcessed = false;
        //                this.fieldgroups = new SortedList<string, VLayoutGroupInfo>();
        //                this.fldGrp = new SortedList<string, string>();
        //                XElement formElement = new XElement(EName.form);
        //                XElement xtabcont = new XElement(EName.tabcontainer);
        //                formElement.Add(xtabcont);
        //                IList<string> property_names = el.GetPropNames();
        //                int index;
        //                for (index = 0; index < property_names.Count; index++) {
        //                    string propName = property_names[index];
        //                    string propName1 = VSXElement.PropPfx + propName;
        //                    //string grTabName = VFieldInfo.FieldGroup(el, propName1);
        //                    //string tabName = grTabName.Split('/').First();
        //                    //string grName = "";
        //                    //if (grTabName != tabName)
        //                    //{
        //                    //    grName = grTabName;
        //                    //}
        //                    //if (!ftabs.ContainsKey(tabName))
        //                    //{
        //                    //    var xgr = new XElement(TextConst.EName.FieldGroup);
        //                    //    xgr.SetAttributeValue(TextConst.AName.Title, tabName);
        //                    //    ftabs.Add(tabName, xgr);
        //                    //    xtabcont.Add(xgr);
        //                    //}
        //                    //if (grName != "")
        //                    //{
        //                    //    if (!fgroups.ContainsKey(grName))
        //                    //    {
        //                    //        var xgr = new XElement(TextConst.EName.FieldGroup);
        //                    //        xgr.SetAttributeValue(TextConst.AName.Title, grName.Split('/').Last());
        //                    //        fgroups.Add(grName, xgr);
        //                    //        ftabs[tabName].Add(xgr);
        //                    //    }
        //                    //}
        //                    XElement xfld = new XElement(EName.field);
        //                    xfld.Add(new XAttribute(AName.name, propName));
        //                    xfld.Add(new XAttribute(AName.controlType, VFieldInfo.ControlType(el, propName1)));
        //                    xfld.Add(new XAttribute(AName.title, VFieldInfo.Title(el, propName1)));
        //                    //if (grName == "")
        //                    //{
        //                    //    ftabs[tabName].Add(xfld);
        //                    //}
        //                    //else
        //                    //{
        //                    //    fgroups[grName].Add(xfld);
        //                    //}
        //                    formElement.Add(xfld);
        //                }
        //                VDataSet ds = new VDataSet();
        //                VDataTable dt = (VDataTable)this.tlQueryScheme.DataSource;
        //                ds.Tables.Add(dt);
        //                ds.ParamsTable = dt;
        //                //  form = (XmlReports.UseNewForms) ? (UIFormC)new UIFormC2(ds, UIFormC.UseType.SchemeEditor) : (UIFormC)new UIFormC(ds, UIFormC.UseType.SchemeEditor);
        //                form = (UIFormC)new UIFormC(ds, UIFormC.UseType.SchemeEditor);
        //                //form = new UIFormC(ds, UIFormC.UseType.SchemeEditor);
        //                form.Initialize(formElement);
        //                Control[] oldCtrls = new Control[this.pcProps.Controls.Count];
        //                this.pcProps.Controls.CopyTo(oldCtrls, 0);
        //                Control ctrl = form.TmpGetControlAsWinFormCtrl() as Control;
        //                this.pcProps.Controls.Add(ctrl);
        //                // ctrl.Focus();
        //                for (index = 0; index < oldCtrls.Length; index++) {
        //                    this.pcProps.Controls.Remove(oldCtrls[index]);
        //                }
        //                dt.CurrentRowChanged += this.ucQueryEditor_CurrentRowChanged;  
        //            } else {
        //                //form = ((sql.builder.UI.WinForms.UIFormControl)pcProps.Controls[0]).GetVForm();
        //                //var table = (form.DataSource.Tables[0] as VDataTable);
        //                //DataRow r = Cmn.GetNodeRow(node);
        //                //table.CurrentRow = r;
        //                form = this.GetUIForm();
        //                VDataTable table = (form.DataSource.Tables[0] as VDataTable);
        //                DataRow r = Cmn.GetNodeRow(node);
        //                form.LayoutSuspend();
        //                table.CurrentRow = r;
        //                form.LayoutResume();
        //            }
        //            //ProcessFieldGroups(el);
        //            //form.ApplyVisibitlity();
        //            if (!fieldGroupsProcessed) {
        //                form.LayoutSuspend();
        //                this.ProcessFieldGroups(el);
        //                form.LayoutResume();
        //                this.GetUIForm().ApplyVisibitlity();
        //            }
        //            //List<DataTable> lists = el.GetChildContextLists();
        //            this.contextManagers.UpdateChildLists(el);
        //            this.clUses.SetCurrentElement(this, el);
        //            this.UpdateRibbonContextDepend();
        //            this.processingSelection = false;
        //        }
        //        private bool fieldGroupsProcessed = false;
        //        private void ucQueryEditor_CurrentRowChanged(object sender, DataRowChangeEventArgs e)
        //        {
        //            VSXElement el = (VSXElement)e.Row["node"];
        //            this.ProcessFieldGroups(el);
        //            this.GetUIForm().ApplyVisibitlity();
        //        }
        //        public void UnsubscribeDataTableEvents()
        //        {
        //            DataRow row = this.Query.Row;
        //            if (row != null) {
        //                ((VDataTable)row.Table).CurrentRowChanged -= this.ucQueryEditor_CurrentRowChanged;
        //            }
        //        }
        //        private void ProcessFieldGroups(VSXElement element)
        //        {
        //            fieldGroupsProcessed = true;
        //            var form = GetUIForm();
        //            form.LayoutSuspend();
        //            var tabs = (VLayoutTabsInfo)form.Layout.GetMainGroup().GetFirstVisible();
        //            foreach (string propName in element.GetPropNames()) {
        //                string propName1 = VSXElement.PropPfx + propName;
        //                string grTabName = VFieldInfo.FieldGroup(element, propName1);
        //                string tabName = grTabName.SubstringBefore('/');
        //                string grName = "";
        //                var ttl = VFieldInfo.Title(element, propName1);
        //                //if (ttl == "Измерение") {
        //                //}
        //                var oldGrp = "";
        //                if (fldGrp.ContainsKey(propName)) {
        //                    oldGrp = fldGrp[propName];
        //                }
        //                if (grTabName != tabName) {
        //                    grName = grTabName.SubstringAfter('/');
        //                }
        //                if (!fieldgroups.ContainsKey(tabName)) {
        //                    var tab = CreateTab(tabs, tabName);
        //                    fieldgroups.Add(tabName, tab);
        //                }
        //                if (!fieldgroups.ContainsKey(grTabName)) {
        //                    var gr = CreateFieldGroup(fieldgroups[tabName], grName, (grTabName == TextConst.SchEdirorFieldGr.MainMain));
        //                    fieldgroups.Add(grTabName, gr);
        //                }
        //                if (oldGrp != grTabName) {
        //                    form.Layout.hasChanges = true;
        //                    form.MoveFieldToOtherLayoutGroup(propName, fieldgroups[grTabName]);
        //                    fldGrp[propName] = grTabName;
        //                }
        //            }
        //            tabs.InitControl();
        //            form.LayoutResume();
        //        }
        //        private static VLayoutGroupInfo CreateFieldGroup(VLayoutGroupInfo parent, string title, bool first)
        //        {
        //            VLayoutGroupInfo item_info = parent.GetLayoutController().CreateGroup(parent, title);
        //            item_info.SetText(title);
        //            if (first) {
        //                parent.AddFirst(item_info);
        //            }
        //            return item_info;
        //        }
        //        private static VLayoutGroupInfo CreateTab(VLayoutTabsInfo tabs, string title)
        //        {
        //            VLayoutGroupInfo item_info = tabs.GetLayoutController().CreateGroup(tabs, title);
        //            item_info.SetText(title);
        //            return item_info;
        //        }
        //        private void tlQueryScheme_KeyDown(object sender, KeyEventArgs e)
        //        {
        //            if (e.KeyCode == Keys.Insert) {
        //                if (e.Shift) {
        //                    this.addChild();
        //                } else {
        //                    this.addNext();
        //                }
        //                return;
        //            }
        //            if (e.Control && (e.KeyCode == Keys.C || e.KeyCode == Keys.X)) {
        //                string s = string.Empty;
        //                foreach (VSXElement el in this.CurrentElements()) {
        //                    if (!el.Ancestors().Any(a => this.CurrentElements().Contains(a))) s += el.ToString() + Environment.NewLine;
        //                }
        //                Clipboard.SetText(s);
        //            }
        //            if (e.Control && e.KeyCode == Keys.V) {
        //                string s = Clipboard.GetText();
        //                s = "<root>" + s + "</root>";
        //                XDocument doc;
        //                try {
        //                    doc = XDocument.Parse(s);
        //                } catch (Exception) {
        //                    return;
        //                }
        //                XElement element = doc.Root;
        //                var list = element.Elements().ToList();
        //                list.Reverse();
        //                InsertChild(list, !e.Shift, false);
        //            }
        //            if (e.KeyCode == Keys.Delete || (e.Control && e.KeyCode == Keys.X)) {
        //                this.GetCurrentControl<ucQueryEditor>().deleteSelected();
        //            }
        //            if (e.Control && e.KeyCode == Keys.Z) {
        //                this.GetRibbonSource<ucQueryEditor>().btnUndoChange.PerformClick();
        //            }
        //            if (e.Control && e.KeyCode == Keys.Y) {
        //                this.GetRibbonSource<ucQueryEditor>().btnRedoChange.PerformClick();
        //            }
        //        }
        //        private void InsertChild(List<XElement> elements, bool next, bool last)
        //        {

        //            TreeListNode node = tlQueryScheme.Selection[0];
        //            DataRow r = Cmn.GetNodeRow(node);
        //            VSXElement el = (r["node"] as VSXElement);
        //            XElement prev = null;
        //            if (next)
        //            {
        //                prev = el;
        //                el = (prev.Parent as VSXElement);
        //            }

        //            else if (last)
        //            {
        //                prev = el.Elements().LastOrDefault();

        //            }





        //            tlQueryScheme.LockReloadNodes();

        //            tlQueryScheme.BeginUpdate();



        //            el.InsertChilds(elements, prev);


        //            tlQueryScheme.EndUpdate();
        //            tlQueryScheme.UnlockReloadNodes();

        //        }
        //        private void contextManagers_ChildItemSelected(object sender, XElementEventArgs e)
        //        {
        //            e.Elements.Reverse();
        //            InsertChild(e.Elements, false, true);
        //        }
        //        private void btnSave_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            ucQueryEditor control = this.GetCurrentControl<ucQueryEditor>();
        //            if (control != null) {
        //                control.SaveQuery();
        //                control.SetChanged(false);
        //			    sql.builder.UI.CommandItems.UIFormsPool.Reset();
        //            }
        //        }
        //        public void SaveQuery()
        //        {
        //            if (Query.SourceFileName == "")
        //            {
        //                selectFile();
        //            }

        //            if (Query.SourceFileName != "")
        //            {
        //                Query.SaveInSourceFile();
        //            }
        //            XmlReports.UpdateElementInCompiledScheme(Query);
        //            SyncNavigators();
        //            Query.ClearCash();
        //        }

        //        public void Close()
        //        {
        //            if (NeedClose != null)
        //            {
        //                NeedClose(this, null);
        //            }
        //        }

        //        public void DetachEvents()
        //        {
        //            if (Query != null)
        //            {
        //                Query.Changed -= Query_Changed; 
        //                Query.Renamed -= OnRenamed;
        //				var _table = Query.SearchTable();
        //				foreach (DataRow _row in _table.Rows)
        //				{
        //					 ((VSXElement)_row["node"]).Row = null;
        //				}
        //				_table.Dispose();
        //            }

        //        }

        //        public void DeleteQuery()
        //        {
        //            var res = ShowMessage.ShowQuestion("Удалить " + Query.P_IdName + "?");
        //            if (res != DialogResult.Yes)
        //            {
        //                return;
        //            }

        //            Query.ClearCash();

        //            if (Query.SourceFileName != "")
        //            {




        //                Query.DeleteSaved();

        //                XmlReports.DeleteElementInCompiledScheme(Query);


        //            }
        //            Close();
        //        }

        //        private void tlQueryScheme_FocusedNodeChanged(object sender, FocusedNodeChangedEventArgs e)
        //        {
        //            if (FocusedQueryChanged == null) return;

        //            FocusedQueryChanged(this, e);
        //        }

        //        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            //SearchTopRibbon(this);
        //        }

        //        private void repositoryItemButtonEdit1_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
        //        {
        //            e.DisplayText = e.Value != null ? Cmn.CutString(e.Value.ToString(), 48) : String.Empty;
        //        }

        //        private bool fileButtonPressed = false;
        //        private void repositoryItemButtonEdit1_ButtonPressed(object sender, ButtonPressedEventArgs e)
        //        {
        //            this.GetRibbonSource<ucQueryEditor>().fileButtonPressed = true;
        //        }
        //        private void repositoryItemButtonEdit1_Click_1(object sender, EventArgs e)
        //        {
        //            ucQueryEditor rib = this.GetRibbonSource<ucQueryEditor>();
        //            if (rib.fileButtonPressed) {
        //                this.GetCurrentControl<ucQueryEditor>().selectFile();
        //                return;
        //            }
        //            string s = rib.beFile.EditValue.ToString();
        //            if (s != "") {
        //                Process.Start(s);
        //            }
        //        }
        //        private void selectFile()
        //        {
        //            string filePath = this.GetRibbonSource<ucQueryEditor>().beFile.EditValue.ToString();
        //            //    openFileDialog1.RestoreDirectory = true;
        //            //    openFileDialog1.AutoUpgradeEnabled = false;
        //            if (string.IsNullOrEmpty(filePath)) {
        //                this.openFileDialog1.InitialDirectory = XmlReports.GetDefaultSourceFolder();
        //                this.openFileDialog1.FileName = string.Empty;
        //            } else {
        //                FileInfo fi = new FileInfo(filePath);
        //                this.openFileDialog1.InitialDirectory = fi.DirectoryName;
        //                this.openFileDialog1.FileName = fi.Name;
        //            }
        //            if (this.openFileDialog1.ShowDialog() == DialogResult.OK) {
        //                string filename = this.openFileDialog1.FileName;
        //                if (this.Query != null) {
        //                    this.Query.SourceFileName = filename;
        //                    this.Query.SetAttributeValue(AName.file, remPathFromFilename(filename));
        //                }
        //                this.showFileName();
        //            }
        //            this.GetRibbonSource<ucQueryEditor>().fileButtonPressed = false;
        //        }
        //        private void UpdateBtnState()
        //        {
        //            bool isConstQuery = false;
        //            IList<VSXElement> list = this.CurrentElements();
        //            if (list.Count != 0) {
        //                VQuery qry = list[0] as VQuery;
        //                if (qry != null && !qry.IsInherit() && !qry.IsExtension()) {
        //                    if (!qry.Elements().Any(EPredicate.IsNotConst)) {
        //                        isConstQuery = true;
        //                    }
        //                }
        //            }
        //            this.GetRibbonSource<ucQueryEditor>().btnEditData.Enabled = isConstQuery;
        //        }
        //        private void UpdateRibbonContextDepend()
        //        {
        //            ucQueryEditor rib = this.GetRibbonSource<ucQueryEditor>();
        //            rib.btnOpenLink.Enabled = IsElementUsers();
        //            rib.btnUpdateNavigators.Enabled = (Query is VReport || Query is VForm || (Query is VQuery && Query.P_IsReport == "1"));
        //            UpdateBtnState();
        //        }
        //        private void showFileName()
        //        {
        //            BarEditItem edit = this.GetRibbonSource<ucQueryEditor>().beFile;
        //            if (this.Query != null) {
        //                edit.EditValue = Query.SourceFileName;
        //            } else {
        //                edit.EditValue = string.Empty;
        //            }
        //        }
        //        public override void UpdateRibbon()
        //        {
        //            this.showFileName();
        //            this.UpdateRibbonContextDepend();
        //            this.UpdateUndoRedoButtonsState();
        //            BarButtonItem button = this.GetRibbonSource<ucQueryEditor>().btnRepPkgSctipt;
        //            if (this.Query is VReport || this.Query is VQuery) { //сделать для остальных кнопок
        //                button.Visibility = BarItemVisibility.Always;
        //            } else {
        //                button.Visibility = BarItemVisibility.Never;
        //            }
        //        }
        //        internal event VRenameEventHandler Renamed;
        //        private void OnRenamed(object sender, VRenameEventArgs e)
        //        {
        //            if (this.Renamed != null) {
        //                this.Renamed(this, e);
        //            }
        //        }
        //        private TreeListNode CurrentNode()
        //        {
        //            return this.tlQueryScheme.Selection[0];
        //        }
        //        private List<TreeListNode> CurrentNodes()
        //        {
        //            return this.tlQueryScheme.Selection.Cast<TreeListNode>().ToList();
        //        }
        //        private List<VSXElement> CurrentElements()
        //        {
        //            return this.CurrentNodes().SelectAsArray(ucQueryEditor.getXNode).ToList();
        //        }
        //        internal VSXElement CurrentElement()
        //        {
        //            return getXNode(this.tlQueryScheme.FocusedNode);
        //            // Емцов - selection изменяется с запаздыванием после события focusednodechanged
        //            // GetPositionID возвращал неправильное значение
        //            //return CurrentElements().FirstOrDefault();
        //        }
        //        private void btnAddNext_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().addNext();
        //        }
        //        private void btnAddChild_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().addChild();
        //        }
        //        private void addChild()
        //        {
        //            VSXElement el = getXNode(this.CurrentNode());
        //            if (el is IVParent) {
        //                this.BeginOperation();
        //                VSXElement child = el.AddChild();
        //                //SetSelection(child);
        //                //SetCurrentRow(child);
        //                //RaiseCurrentRowChanged();
        //                this.EndOperation();
        //            }
        //        }
        //        private void wrapToFunc()
        //        {
        //            this.BeginOperation();
        //            this.tlQueryScheme.LockReloadNodes();
        //            this.tlQueryScheme.BeginUpdate();
        //            VSXElement el = getXNode(this.CurrentNode());
        //            //VSXElement child = el.AddChild();
        //            VSXElement el1 = VSXElement.Get(new XElement(el));
        //            VSXElement funcCall = VSXElement.Get(new XElement(EName.call));
        //            if (el.GetParent() is VSelect) {
        //                funcCall.P_Alias = el.XName;
        //            }
        //            funcCall.P_DataType = el.DataType();
        //            funcCall.P_SelfTitle = el.P_SelfTitle;
        //            funcCall.P_Group = el.P_Group;
        //            el1.P_Alias = string.Empty;
        //            el1.P_DataType = string.Empty;
        //            el1.P_SelfTitle = string.Empty;
        //            el1.P_Group = string.Empty;
        //            el.AddPrev(funcCall);
        //            funcCall.AddChild(el1);
        //            el.Delete();
        //            this.tlQueryScheme.EndUpdate();
        //            this.tlQueryScheme.UnlockReloadNodes();
        //            //SetSelection(child);
        //            //SetCurrentRow(child);
        //            //RaiseCurrentRowChanged();
        //            this.EndOperation();
        //            this.SetSelection(funcCall);
        //        }
        //        private void addNext()
        //        {
        //            this.BeginOperation();
        //            VSXElement el = getXNode(this.CurrentNode());
        //            VSXElement child = el.AddNext();
        //            //SetSelection(child);
        //            //SetCurrentRow(child);
        //            //RaiseCurrentRowChanged();
        //            this.EndOperation();
        //        }
        //        private void btnMakeSQL_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().MakeSql();
        //        }
        //        private void CreateTableScript(bool onlyChanges)
        //        {
        //            string script = null;

        //            bool exists = false;
        //            if (onlyChanges)
        //            {
        //                exists = db.CheckTableExists(Query.P_Name);
        //            }
        //            script = (exists)
        //                ? SqlSchemeBuilder.GenerateAlterTableScript(Query as VQuery)
        //                : SqlSchemeBuilder.GenerateTableScript(Query as VQuery, with_drops: true);

        //            var filename = Cmn.writeScriptFile(Query.XName, script);

        //            Process.Start(filename);
        //        }
        //        private VQuery getVQuery()
        //        {
        //            var qry = this.Query as VQuery;
        //            if (qry == null) {
        //                var frm = Query as VForm;
        //                if (frm != null) {
        //                    var qryCall = this.CurrentElement() as VQueryCall;
        //                    if (qryCall != null) {
        //                        string name = qryCall.Name.LocalName;
        //                        if (name == TextConst.EName.Query || name == TextConst.EName.ELink) {
        //                            qry = VSXElement.Get<VQuery>(frm.CreateTableQuery(qryCall));
        //                        }
        //                    }
        //                }
        //            }
        //            return qry;
        //        }
        //        private void MakeSql()
        //        {
        //            var qry = this.getVQuery();
        //            if (qry != null) {
        //                string s = qry.GetSql((bool)this.GetRibbonSource<ucQueryEditor>().checkTitleAsNameB.EditValue);
        //                if (!string.IsNullOrEmpty(s)) {
        //                    string fname = Path.Combine(SqlBuilder.GetWorkFolderPath(), "query.sql");
        //                    Cmn.SaveText(s, fname, Encoding.Unicode);
        //                    Process.Start(fname);
        //                }
        //            }
        //        }

        //        private void MakeQubeInfo()
        //        {
        //            var qry = getVQuery();
        //            XElement info = null;
        //            if (qry!=null)
        //            {

        //                info = qry.GetQubeInfo();
        //            }

        //            if (info != null)
        //            {
        //                var filename = Settings.Default.testQueryS2 + ".xml";// !!!передалать
        //                Cmn.SaveText(info.ToString(), filename, Encoding.UTF8);
        //                Process.Start(filename);
        //            }


        //        }

        //        private void tlQueryScheme_SelectionChanged(object sender, EventArgs e)
        //        {

        //            if (tlQueryScheme.DataSource != null && (tlQueryScheme.DataSource as VDataTable).IsChangeEventSuppressed()) return;
        //            if (tlQueryScheme.Selection.Count > 0)
        //            {
        //                ItemSelected(tlQueryScheme.Selection[0]);
        //            }
        //        }

        //        private void beFile_ItemClick(object sender, ItemClickEventArgs e)
        //        {

        //        }

        //        private void btnDel_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().deleteSelected();
        //        }
        //        private static VSXElement getXNode(TreeListNode node)
        //        {
        //            DataRow r = Cmn.GetNodeRow(node);
        //            if (r == null) {
        //                return null;
        //            }
        //            VSXElement el = r["node"] as VSXElement;
        //            return el;
        //        }
        //        private void deleteSelected()
        //        {
        //            VSXElement first = this.CurrentElements().FirstOrDefault();
        //            VSXElement newselection = null;
        //            if (first != null) {
        //                newselection = first.PreviousNode as VSXElement;
        //                if (newselection == null) {
        //                    newselection = first.Parent as VSXElement;
        //                }
        //            }
        //            BeginOperation();
        //            tlQueryScheme.LockReloadNodes();
        //            tlQueryScheme.BeginUpdate();
        //            foreach (var e in CurrentElements()) {
        //                e.Delete();
        //            }
        //            tlQueryScheme.EndUpdate();
        //            tlQueryScheme.UnlockReloadNodes();
        //            EndOperation();
        //            if (newselection != null) {
        //                SetSelection(newselection);
        //            }
        //        }
        //        private void MoveUp()
        //        {
        //            List<VSXElement> elements = CurrentElements();
        //            foreach (VSXElement parent in elements.SelectAsArray(e => e.GetParent()).Distinct())
        //            {
        //                List<VSXElement> elements1 = elements.Where(e => e.GetParent() == parent).ToList();

        //                int i = Int32.MaxValue;
        //                foreach (VSXElement el in elements1)
        //                {
        //                    int pos = el.ElementsBeforeSelf().Count();

        //                    if (pos < i) i = pos;

        //                }
        //                VSXElement prev = null;
        //                if (i > 1)
        //                {
        //                    prev = (VSXElement)parent.Elements().ElementAt(i - 2);
        //                }
        //                foreach (VSXElement el in elements1)
        //                {

        //                    el.MoveAfter(prev);
        //                    prev = el;

        //                }
        //                parent.UpdateChildOrder();

        //            }

        //        }
        //        private List<VSXElement> GetCurrentUsedElements()
        //        {
        //            return this.CurrentElements().SelectMany(e => e.GetUsedElements().Where(e1 => e1 != null).Select(e2 => e2.BaseElementOrSelf())).Where(e1 => e1 != null).ToList();
        //        }
        //        private bool IsElementUsers()
        //        {
        //            return this.CurrentElements().Any(e1 => e1.IsElementUser());
        //        }
        //        internal event XElementEventHandler OpenUses;
        //        public void RaiseOpenUses()
        //        {
        //            this.OpenUses(this, new XElementEventArgs(GetCurrentUsedElements().Cast<XElement>().ToList()));
        //        }
        //        public void RaiseOpenFieldUses()
        //        {
        //            VSXElement usedEl = null;
        //            if (GetUIForm().LastActiveField != null)
        //            {
        //                string propName1 = VSXElement.PropPfx + GetUIForm().LastActiveField.FieldName;

        //                usedEl = VFieldInfo.UsedEl(CurrentElement(), propName1);
        //            }
        //            if (usedEl != null) {
        //                this.OpenUses(this, new XElementEventArgs(new List<XElement>(1) { usedEl }));
        //            }
        //        }
        //        private void btnOpenLink_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().RaiseOpenUses();
        //        }
        //        private void btnUp_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().MoveUp();
        //        }
        //        private void btnRefresh_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            ucQueryEditor control = this.GetCurrentControl<ucQueryEditor>();
        //            if (control != null) {
        //                control.RefreshQuery();
        //                control.SetChanged(false);
        //            }
        //        }
        //        private void RefreshQuery()
        //        {
        //            VDataTable dt = tlQueryScheme.DataSource as VDataTable;
        //            if (dt != null) {
        //                foreach (DataRow r in dt.Rows) {
        //                    var el = (r["node"] as VSXElement);
        //                    el.UpdateDataRow();
        //                }
        //            }
        //        }
        //        private void RefreshQueryColumn(string name)
        //        {
        //            var dt = (tlQueryScheme.DataSource as VDataTable);
        //            foreach (DataRow r in dt.Rows) {
        //                var el = (r["node"] as VSXElement);
        //                el.UpdateDataCell(name);
        //            }
        //        }
        //        internal void SetSelection(VSXElement element)
        //        {
        //            if (element == null) {
        //                return;
        //            }
        //            TreeListNode node = Cmn.GetNodeByRowRecursive(tlQueryScheme, element.BaseElementOrSelf().Row);
        //            tlQueryScheme.Selection.Clear();
        //            if (node != null) {
        //                node.Selected = true;
        //                tlQueryScheme.SetFocusedNode(node);
        //            }
        //        }
        //        private void btnSchemeColumns_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().selectSchemeColumns();
        //        }
        //        private void selectSchemeColumns()
        //        {
        //            VDataTable tbl = (VDataTable)tlQueryScheme.DataSource;
        //            VDataSet dataSet = (VDataSet)tbl.DataSet;
        //            XElement scheme = VDataSet.GetXmlSchemeFromDataSet(dataSet);
        //            XElement xViewColumns_old = scheme.Element(EName.table).Element(EName.columns);
        //            XElement xViewColumns_preset = new XElement(xViewColumns_old);
        //            foreach (XElement xcol in xViewColumns_preset.Elements(EName.column).ToList()) {
        //                VDataColumn dataColumn = (VDataColumn)tbl.Columns[xcol.Attribute(AName.name).Value];
        //                if (!dataColumn.Visible) {
        //                    xcol.Remove();
        //                }
        //            }
        //            using (var frm = new frmColumnsEditorNew()) {
        //                frm.Initialize(xViewColumns_old, xViewColumns_preset);
        //                if (frm.ShowDialog() == DialogResult.OK) {
        //                    XElement xViewColumns_new = frm.GetColumnsXml();
        //                    setSchemeColumns(xViewColumns_new, xViewColumns_old);
        //                }
        //            }
        //        }
        //        private void setSchemeColumns(XElement xViewColumns_new, XElement xViewColumns_old)
        //        {
        //            if (xViewColumns_old != null) {
        //                VDataTable tbl = (VDataTable)tlQueryScheme.DataSource;
        //                foreach (XElement xcol in xViewColumns_old.Elements(EName.column).ToList()) {
        //                    VDataColumn dataColumn = (VDataColumn)tbl.Columns[xcol.Attribute(AName.name).Value];
        //                    XElement newXCol = xViewColumns_new.Elements(EName.column).SearchByAttribute(AName.name, dataColumn.ColumnName);
        //                    TreeListColumn gCol = tlQueryScheme.Columns[dataColumn.ColumnName];
        //                    bool oldVisible = dataColumn.Visible;
        //                    if (newXCol != null) {
        //                        dataColumn.Visible = true;
        //                        if (!oldVisible) {
        //                            RefreshQueryColumn(dataColumn.ColumnName);
        //                        }
        //                        gCol.Visible = true;
        //                    } else {
        //                        dataColumn.Visible = false;
        //                        gCol.Visible = false;
        //                    }
        //                }
        //            }
        //            var i = 0;
        //            foreach (XElement xcol in xViewColumns_new.Elements(EName.column).ToList()) {
        //                TreeListColumn gCol = tlQueryScheme.Columns[xcol.Attribute(AName.name).Value];
        //                gCol.VisibleIndex = i;
        //                i++;
        //            }
        //        }
        //        //private void btnRepInfo_ItemClick(object sender, ItemClickEventArgs e)
        //        //{
        //        //    thisControl().MakeRepInfo();
        //        //}
        //        //private void MakeRepInfo()
        //        //{
        //        //    if (!(this.Query is VReport))
        //        //    {
        //        //        return;
        //        //    }
        //        //    VDataSet ds = VDocumenting.CreateReportInfoDataSet((this.Query as VReport));
        //        //    string fullPath = Printing.Print(ds, "report_info.xml", "Описание отчета " + this.Query.P_Title);
        //        //    Cmn.OpenPrintedFile(fullPath);
        //        //}
        //        private void btnSaveLayout_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().SaveLayout();
        //        }
        //        private void SaveLayout()
        //        {
        //            XElement el = GetTabbedViewLayoutAsXml(tabbedView1);
        //            el.Add(GetSchemeColumnsSettings());
        //            Cmn.WriteXElementToRegistry(TextConst.RegPath.ScemeEditorLayout, Query.GetParent().Name.LocalName, el);
        //        }
        //        private void btnOPenLayout_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().OpenLayout();
        //        }
        //        private void OpenLayout()
        //        {
        //            XElement el = Cmn.ReadXElementFromRegistry(TextConst.RegPath.ScemeEditorLayout, Query.GetParent().Name.LocalName);
        //            if (el != null)
        //            {
        //                SetTabbedViewLayoutFromXml(tabbedView1, el);

        //                var xschemecolumns = el.Element("scheme-columns");
        //                if (xschemecolumns != null)
        //                {
        //                    SetSchemeColumnsSettings(xschemecolumns);
        //                    return;
        //                }
        //            }

        //            tlQueryScheme.BestFitColumns();
        //        }

        //        private void btnClearLayout_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().ClearLayout();
        //        }

        //        private void ClearLayout()
        //        {
        //            Cmn.WriteXElementToRegistry(TextConst.RegPath.ScemeEditorLayout, Query.GetParent().Name.LocalName, null);

        //        }

        //        private void btnDesignForm_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            // все сломалось с новым layout
        //            //thisControl<ucQueryEditor>().OpenFormForDesign();
        //        }
        //        /*private void SortElements(XElement xitem)
        //        {
        //            var sorted_children = xitem.Elements()
        //                .OrderBy(item => item.Attribute("position").Value.Split(';')[1])
        //                .ThenBy(item => item.Attribute("position").Value.SubstringBefore(';'))
        //                .ToArray();

        //            xitem.Elements().Remove();
        //            xitem.Add(sorted_children);

        //            foreach (var xchild_item in xitem.Elements())
        //            {
        //                // колонки грида не обрабатываем, тк. к ним запарно генерировать id
        //                if (xchild_item.Name == "grid") continue;

        //                SortElements(xchild_item);
        //            }
        //        }*/
        //        private void btnSQLText_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            VCashUtils.ClearCash();
        //            GetCurrentControl<ucQueryEditor>().OpentetsForm();

        //        }
        //        private void OpentetsForm()
        //        {
        //            var frm1 = new Test.TestParameters();
        //            if (Query is VQuery)
        //            {
        //                frm1.QueryName = Query.XName;
        //                frm1.Initialize();
        //                frm1.ShowDialog();
        //            }
        //        }


        //        private void btnExpSchemeToXl_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            GetCurrentControl<ucQueryEditor>().ExportSchemeToExcel();
        //        }

        //        public void ExportSchemeToExcel()
        //        {


        //            string full_path = Printing.GetFreeName(SqlBuilder.GetWorkFolderPath(), Query.P_IdName, "xlsx");
        //            tlQueryScheme.ExportToXlsx(full_path);
        //            Cmn.OpenPrintedFile(full_path);

        //        }

        //        private void btnOpenFieldLink_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().RaiseOpenFieldUses();
        //        }

        //        private void btnLoadFromDb_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            GetCurrentControl<ucQueryEditor>().LoadSelectedFromDb();
        //        }

        //        private void LoadSelectedFromDb()
        //        {
        //            bool changes = false;
        //            foreach (VSXElement el in CurrentElements())
        //            {
        //                if (el is VRelation)
        //                {
        //                    var rel = el as VRelation;
        //                    WaitUIHelper.LastUsedUIHelper.Show("Загрузка " + rel.ParentName, WaitUIMode.WaitPanel);
        //                    ucQueriesEditor.MakeQuerySchemeFromDb(rel.ParentName, el.GetProjectName());
        //                    WaitUIHelper.LastUsedUIHelper.Hide();
        //                    changes = true;
        //                }
        //            }
        //            if (changes)
        //            {
        //                NeedRefreshScheme(this, null);
        //                LoadQuery();
        //            }

        //            //  XmlReports.Init(true);
        //            //VCashUtils.ClearCash();

        //            //foreach (VSXElement el in CurrentElements())
        //            //{
        //            //    if (el is VRelation)
        //            //    {
        //            //        //el.ClearCash();
        //            //        el.UpdateDataRow();

        //            //    }
        //            //}
        //        }
        //        private XElement GetSchemeColumnsSettings()
        //        {
        //            var xSchemeColumns = new XElement("scheme-columns");
        //            var columns = tlQueryScheme.Columns.Where(c => c.Visible).OrderBy(c => c.VisibleIndex);
        //            foreach (TreeListColumn column in columns) {
        //                XElement xColumn = new XElement(EName.column);
        //                xColumn.Add(new XAttribute(AName.name, column.FieldName));
        //                xColumn.Add(new XAttribute(AName.width, column.Width));
        //                // сортировки
        //                if (column.SortOrder != SortOrder.None) {
        //                    xColumn.Add(new XAttribute("sort", column.SortOrder.ToString().ToLower()));
        //                }
        //                xSchemeColumns.Add(xColumn);
        //            }
        //            return xSchemeColumns;
        //        }
        //        private void SetSchemeColumnsSettings(XElement xSchemeColumns)
        //        {
        //            tlQueryScheme.BeginUpdate();

        //            var dt = tlQueryScheme.DataSource as VDataTable;

        //            foreach (TreeListColumn c in tlQueryScheme.Columns) c.Visible = false;

        //            var xschemecolumns = xSchemeColumns.Elements("column").Reverse().ToArray();
        //            foreach (var xcolumn in xschemecolumns)
        //            {
        //                var dcolumn = (dt.Columns[xcolumn.Attribute("name").Value] as VDataColumn);
        //                var column = tlQueryScheme.Columns[xcolumn.Attribute("name").Value];

        //                column.Visible = dcolumn.Visible = true;

        //                int width = int.Parse(xcolumn.Attribute("width").Value);
        //                // у первой колонки ширина ставится автоматически
        //                // если ее поменять, это влияет на ширину второй колонки
        //                if (xcolumn != xschemecolumns[0] || column.MinWidth < width)
        //                {
        //                    column.Width = width;
        //                }

        //                var sort = xcolumn.Attribute("sort");
        //                if (sort != null)
        //                {
        //                    switch (sort.Value)
        //                    {
        //                        case "ascending": column.SortOrder = SortOrder.Ascending; break;
        //                        case "descending": column.SortOrder = SortOrder.Descending; break;
        //                        default: column.SortOrder = SortOrder.None; break;
        //                    }
        //                }
        //                RefreshQueryColumn(dcolumn.ColumnName);
        //            }

        //            tlQueryScheme.EndUpdate();
        //        }
        //        private string makeName()
        //        {
        //            var name = string.Format("{0} {1}", Query.Name.LocalName, Query.P_IdName);
        //            return name;
        //        }
        //        private XElement makeInfo()
        //        {
        //            var name = makeName();
        //            var cur_el = CurrentElement();
        //            var xinfo = new XElement("element",
        //                            new XAttribute("parent-name", Query.GetParent().Name.LocalName),
        //                            new XAttribute("file", Query.AttrOrEmpty(AName.file)),
        //                            new XAttribute("key-name", Query.KeyField.LocalName),
        //                            new XAttribute("key", Query.AttrOrEmpty(this.Query.KeyField)),
        //                            new XAttribute("name", Query.Name.LocalName),
        //                            new XAttribute("current-node", (cur_el != null) ? cur_el.GetPositionId() : ""));
        //            return xinfo;
        //        }
        //        public new void SaveState()
        //        {
        //            var name = makeName();
        //            var xroot = new XElement(EName.root);
        //            var xinfo = makeInfo();
        //            xroot.Add(xinfo);
        //            Cmn.WriteXElementToRegistry(TextConst.RegPath.SchemeOpenItems, name, xroot);
        //        }
        //        private void btnQubeInfo_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().MakeQubeInfo();
        //        }
        //        private void ucQueryEditor_Load(object sender, EventArgs e)
        //        {

        //        }
        //        private void barButtonItem8_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().CreateTableScript(true);
        //        }
        //        private void btnTextReplace_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().TextReplace();
        //        }
        //        private void TextReplace()
        //        {
        //            if (_frm_replace_text == null) _frm_replace_text = new frmReplaceText();

        //            var result = _frm_replace_text.ShowDialog();
        //            if (result != DialogResult.OK || _frm_replace_text.SearchText == "") return;

        //            // старый элемент (удаленный), новый элемент (добавленный)
        //            var dict_old_new = new Dictionary<VSXElement, VSXElement>();
        //            // запоминаем фокус
        //            VSXElement focus = (VSXElement)tlQueryScheme.FocusedNode["node"];

        //            string text = null;

        //            try
        //            {
        //                // блокировка лишних событий
        //                BeginOperation();
        //                tlQueryScheme.LockReloadNodes();
        //                tlQueryScheme.BeginUpdate();
        //                (tlQueryScheme.DataSource as VDataTable).SuppressChangeEvent();

        //                // выбранные элементы (кроме корневого) в порядке от листьев к корню
        //                // берем только самые верхние элементы, тк они уже включают xml нижних
        //                List<VSXElement> vels = CurrentElements();
        //                vels = vels.Where(el => tlQueryScheme.Nodes[0]["node"] != el && el.Ancestors().All(a => !vels.Contains(a)))
        //                         .OrderByDescending(el => tlQueryScheme.GetVisibleIndexByNode(tlQueryScheme.FindNodeByFieldValue("node", el)))
        //                         .ToList();

        //                // замена текста в выбранных элементах
        //                foreach (var vel in vels)
        //                {
        //                    // замена текста xml через replace
        //                    text = Regex.Replace(vel.ToString(), _frm_replace_text.SearchText, _frm_replace_text.ReplaceToText,
        //                        (_frm_replace_text.CheckCase ? RegexOptions.None : RegexOptions.IgnoreCase));

        //                    // приведение текста к XElement - тут может возникнуть XmlException
        //                    var el_new = XElement.Parse(text);

        //                    // если сосед был заменен - берем вместо него нового
        //                    VSXElement prev = VSXElement.Get(vel.ElementsBeforeSelf().LastOrDefault());
        //                    if (prev != null && prev.Parent == null)
        //                    {
        //                        prev = dict_old_new[prev];
        //                    }

        //                    // если родитель был заменен - берем вместо него нового
        //                    VSXElement parent = vel.Parent as VSXElement;
        //                    if (parent != null && parent.Parent == null)
        //                    {
        //                        parent = dict_old_new[parent];
        //                    }

        //                    // старый со всеми дочерними удаляем 
        //                    vel.Delete();

        //                    // добавляем новый элемент вместо старого со всеми дочерними
        //                    VSXElement vel_new = parent.InsertChild(el_new, prev, true, false);
        //                    // сохраняем старый и новый элементы со всеми дочерними
        //                    VSXElement[] arr1 = vel.DescendantsAndSelf().Select(VSXElement.Get).ToArray();
        //                    VSXElement[] arr2 = vel_new.DescendantsAndSelf().Select(VSXElement.Get).ToArray();
        //                    for (int i = 0; i < arr1.Length; i++)
        //                    {
        //                        dict_old_new.Add(arr1[i], arr2[i]);
        //                    }

        //                    // восстанавливаем порядок
        //                    parent.UpdateChildOrder();
        //                }
        //            }
        //            catch (XmlException)
        //            {
        //                XtraMessageBox.Show(text,
        //                    "Невалидный xml",
        //                    MessageBoxButtons.OK,
        //                    MessageBoxIcon.Error);
        //            }
        //            finally
        //            {
        //                // разблокировка событий
        //                (tlQueryScheme.DataSource as VDataTable).UnsuppressChangeEvent();
        //                tlQueryScheme.EndUpdate();
        //                tlQueryScheme.UnlockReloadNodes();
        //                EndOperation();

        //                // восстанавливаем фокус
        //                if (dict_old_new.ContainsKey(focus)) focus = dict_old_new[focus];
        //                tlQueryScheme.FocusedNode = tlQueryScheme.FindNodeByFieldValue("node", focus);

        //                // восстанавливаем выделение
        //                var nodes = dict_old_new.Values.Select(el => tlQueryScheme.FindNodeByFieldValue("node", el));
        //                tlQueryScheme.Selection.Add(nodes);
        //            }
        //        }
        //        private void tlQueryScheme_AfterExpand(object sender, NodeEventArgs e)
        //        {
        //            var el = getXNode(e.Node);
        //            el.TreeNodeExpanded = e.Node.Expanded;
        //            el.UpdateDataCell("NodeText");
        //            tlQueryScheme.Refresh();
        //        }
        //        private void tlQueryScheme_AfterCollapse(object sender, NodeEventArgs e)
        //        {
        //            var el = getXNode(e.Node);

        //            el.TreeNodeExpanded = e.Node.Expanded;
        //            //el.UpdateDataRow();
        //            el.UpdateDataCell("NodeText");
        //            tlQueryScheme.Refresh();
        //        }
        //        public bool IsChanged()
        //        {
        //            var doc = (Parent.Parent as DocumentContainer).Document;
        //            return doc.Caption.EndsWith("*");
        //        }
        //        public void SetChanged(bool val)
        //        {
        //            if (!queryLoaded || Parent == null) return;

        //            var doc = (Parent.Parent as DocumentContainer).Document;
        //            if (IsChanged() && !val)
        //            {
        //                _operation_timer.Stop();
        //                doc.Caption = doc.Caption.TrimEnd('*');
        //            }
        //            else if (!IsChanged() && val)
        //            {
        //                doc.Caption = doc.Caption + "*";
        //            }
        //        }
        //        private void barButtonItem1_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().CreateMatViewScript();
        //        }
        //        private void barButtonItem6_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().CreateReportPackageScript();
        //        }
        //        private void tlQueryScheme_CustomNodeCellEdit(object sender, GetCustomNodeCellEditEventArgs e)
        //        {
        //            if (tlQueryScheme.GetDataRecordByNode(e.Node) == null)
        //            {
        //                e.RepositoryItem = new RepositoryItemTextEdit();
        //            }
        //        }
        //        private void btnUpdateNavigators_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().SyncNavigators();
        //        }
        //        private void SyncNavigators()
        //        {
        //            XmlReports.Environment.SyncNavigators(Query);
        //        }
        //        private void barButtonItem6_ItemClick_1(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().CreateTableScript(false);
        //        }
        //        private void btnEditData_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().EditConstQueryData();
        //        }
        //        private void EditConstQueryData()
        //        {
        //            var frm = new frmDynamicEditor();
        //            var editor = new ucExcelDataEditor();
        //            editor.SetSpreadsheetDataFromXml(Query.Elements().ToList());
        //            editor.Dock = DockStyle.Fill;
        //            frm.Controls.Add(editor);
        //            frm.Text = Query.XName;
        //            if (frm.ShowDialog() == DialogResult.OK)
        //            {
        //                var items = editor.GetXml();


        //                BeginOperation();
        //                tlQueryScheme.LockReloadNodes();
        //                tlQueryScheme.BeginUpdate();

        //                foreach (var e1 in Query.Elements().Select(VSXElement.Get).ToList()) e1.Delete();
        //                items.Reverse();
        //                Query.InsertChilds(items, null);

        //                tlQueryScheme.EndUpdate();
        //                tlQueryScheme.UnlockReloadNodes();
        //                EndOperation();
        //            }
        //            frm.Dispose();
        //        }
        //        private void btnDelete_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            ucQueryEditor control = this.GetCurrentControl<ucQueryEditor>();
        //            if (control != null) {
        //                control.DeleteQuery();
        //            }
        //        }
        //        private void btnRename_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().RenameUses();
        //        }
        //        private void RenameUses()
        //        {
        //            UIStatic.LoadProject("system");
        //            var form = UIStatic.CreateSystemForm("sys_str_input", true);
        //            form.SetTitle("Переименование");
        //            if (form.IsNew) {
        //                form.OnButtonClick += form_OnButtonClick;
        //                form.GetParamField("value").SetTitle("Имя");
        //            }
        //            form.LoadData(null);
        //            string oldVal = Query.Attribute(Query.GetAttrForRename()).Value;
        //            form.GetParamField("value").SetValue(oldVal);
        //            form.ShowDialog();
        //        }
        //        void form_OnButtonClick(UIFormC form, XElement value)
        //        {
        //            switch (Cmn.GetAttrValue(value, TextConst.AName.Name))
        //            {
        //                case "ok":
        //                    string oldName = Query.Attribute(Query.GetAttrForRename()).Value;
        //                    var newName = form.GetParamField("value").GetValue().ToString();
        //                    if (ShowMessage.ShowQuestion("Изменить имя \"" + oldName + "\" на \"" + newName + "\"? Перед операцией рекомендуется выполнить возврат, т.к. отметить изменения будет невозожно.") == DialogResult.Yes)
        //                    {
        //                        VCashUtils.ClearCash();
        //                        Query.RenameUses(newName);
        //                        ucQueriesEditor.GetInstance().SaveAll();
        //                        SaveState();
        //                        //  ucQueriesEditor.GetInstance().EditorSaveState();
        //                        // ucQueriesEditor.GetInstance().SaveAll();
        //                        // Element.UpdateDataRow();
        //                        clUses.Refresh();

        //                        // ucQueriesEditor.GetInstance()
        //                    }
        //                    break;
        //            }
        //        }
        //        private void btnTempTbl_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().CreateTempTableScript();
        //        }
        //        private void CreateTempTableScript()
        //        {
        //           var script= SqlSchemeBuilder.GenerateTableScript(Query as VQuery,  true,true);
        //           var filename = Cmn.writeScriptFile(Query.XName, script);
        //           //var script = CodeGenerationUtils.UpdateTempFromObject(Query.XName);
        //           //var filename = writeScriptFile(Query.XName + "_tmp", script);
        //           Process.Start(filename);
        //        }
        //        private void btnGetCode_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().GetCodeOfCurrentElement();
        //        }
        //        private void GetCodeOfCurrentElement()
        //        {
        //            var el = CurrentElements().FirstOrDefault();
        //            if (el == null) return;
        //            var s = Cmn.BuildCodeOfXmlString(el.ToString());
        //            var filename = "xmlcode.txt";
        //            filename = Cmn.WriteFileToTemp(filename, s.ToString());
        //            Process.Start(filename);
        //        }
        //        private void barButtonItem10_ItemClick(object sender, ItemClickEventArgs e)
        //        {
        //            this.GetCurrentControl<ucQueryEditor>().wrapToFunc();
        //        }
    }
}
