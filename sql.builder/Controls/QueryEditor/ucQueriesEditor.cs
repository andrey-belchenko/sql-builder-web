//using System;
//using System.Collections;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using System.Xml.XPath;
//using DevExpress.Utils;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Docking2010.Views;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraGrid.Columns;

//using DevExpress.XtraTreeList;
//using infoenergo.core.Extensions;
//using infoenergo.ui.win.Forms;
//using sql.builder.Controls;
//using sql.builder.Core;
//using sql.builder.DataApi;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
//using sql.builder.Exceptions;
//using sql.builder.TFS;
//using HasMessageArgs = sql.builder.Core.HasMessageArgs;

//using sql.builder.UI;
//namespace sql.builder
//{
//    internal partial class ucQueriesEditor : ucBase
//    {
//        #region static
//        private static ucQueriesEditor _instance;
//        internal static ucQueriesEditor GetInstance()
//        {
//            return _instance.GetCurrentControl<ucQueriesEditor>();
//        }
//        #endregion
//        private List<Tuple<int, int>> _move_history;
//        private int _current_position = -1;
//        private bool creationListReady;
//        private Dictionary<string, ucQueriesEditorItem> openQueries;
//        protected override void AfterChangeRibbon()
//        {
//            this.ResetProjButtons();
//        }
//        protected void ResetProjButtons()
//        {
//            ucQueriesEditor rib = this.GetRibbonSource<ucQueriesEditor>();
//            rib.lcLoadFromDB.ClearLinks();
//            foreach (Project proj in XmlReports.Environment.Manager.GetLoadedProjects()) {
//                BarButtonItem btn = new BarButtonItem();
//                btn.Caption = proj.Name;
//                btn.ItemClick += this.btnLoadFromDB_ItemClick;
//                BarItemLink link = rib.lcLoadFromDB.AddItem(btn);
//                link.BeginGroup = true;
//            }
//        }
//        public ucQueriesEditor()
//        {
//            InitializeComponent();
//            this.openQueries = new Dictionary<string, ucQueriesEditorItem>();
//            this._move_history = new List<Tuple<int, int>>();
//            _instance = this;// надоело протягивать события
//        }
//        public override void Initialize()
//        {
//        }
//        public void RefreshQueryList()
//        {
//            gcQueries.DataSource = null;
//            IEnumerable<XElement> qrys = XmlReports.Environment.Manager.GetNativeScheme().Elements().Elements();
//            IList<ucQueryEditor> qes = new List<ucQueryEditor>();
//            foreach (ucQueriesEditorItem qei in openQueries.Values) {
//                ucQueryEditor editor = qei.QueryEditor;
//                if (editor != null) {
//                    qes.Add(editor);
//                }
//            }
//            int index;
//            for (index = 0; index < qes.Count; index++) {
//                qes[index].FreezeOperations = true;
//            }
//            DataTable dt = Cmn.XElementsToDataTable(qrys, false, true);
//            dt.Columns.Add(new DataColumn("id_name"));
//            dt.Columns.Add(new DataColumn("mark"));
//            dt.Columns.Add(new DataColumn("node_name_x"));
//            dt.Columns.Add(new DataColumn("proj"));
//            foreach (DataRow r in dt.Rows) {
//                VSXElement el = VSXElement.Get((XElement)r["node"]);
//                r["id_name"] = Cmn.Nvl(r["id"], r["name"]).ToString();
//                r["node"] = el;
//                r["mark"] = "0";
//                r["proj"] = el.GetProjectName();
//                string nnx = r["node_name"].ToString();
//                if (nnx == TextConst.EName.Query) {
//                    if (el.P_IsReport == TextConst.AVBool.True) {
//                        nnx += "(report)";
//                    } else if (el.Elements(EName.from).Elements(EName.table).Any()) {
//                        nnx += "(table)";
//                    }
//                }
//                r["node_name_x"] = nnx;
//            }
//            gcQueries.DataSource = dt;
//            for (index = 0; index < qes.Count; index++) {
//                qes[index].FreezeOperations = false;
//            }
//        }
//        ucQueryEditor ActiveEditor()
//        {
//            var doc = tabbedView1.ActiveDocument;
//            if (doc == null) {
//                return null;
//            } else {
//                return openQueries.Values.First(q => q.Doc == doc).QueryEditor;
//            }
//        }
//        private ucQueriesEditorItem CreateQueryItem(string name, XElement xquery, VSXElement target)
//        {
//            ucQueriesEditorItem item;
//            if (!this.openQueries.TryGetValue(name, out item)) {
//                BaseDocument doc = documentManager1.View.AddDocument(new PanelControl() { Dock = DockStyle.Fill });
//                doc.Properties.AllowFloat = DefaultBoolean.False;
//                item = new ucQueriesEditorItem(doc, xquery, target, this.ucQueriesEditor_FocusedQueryChanged, this.Query_OpenUses, this.Query_Renamed, this.queryEditor_NeedClose, this.queryEditor_NeedRefreshScheme);
//                openQueries.Add(name, item);
//            }
//            return item;
//        }
//        private void queryEditor_NeedClose(object sender, EventArgs e)
//        {
//            var qe = (ucQueriesEditorItem)sender;
//            documentManager1.View.Controller.Close(qe.Doc);
//        }
//        private void queryEditor_NeedRefreshScheme(object sender, EventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucQueriesEditor>();
//            ctrl.ReloadAndRefreshQueryList();
//        }
//        public ucQueriesEditorItem OpenQuery(VSXElement element, VSXElement target, bool activate = true)
//        {
//            ucQueriesEditorItem item = null;
//            if (openQueries.ContainsKey(element.GetFullName()))
//            {
//                item = openQueries[element.GetFullName()];

//                if (item.Loaded) item.QueryEditor.SetSelection(target);
//                else item.Target = target;
//            }
//            else
//            {
//                item = CreateQueryItem(element.GetFullName(), element, target);
//            }

//            item.UpdateCaption();

//            if (activate)
//            {
//                documentManager1.View.ActivateDocument(item.Doc.Control);
//            }

//            return item;
//        }
//        void Query_Renamed(object sender, VRenameEventArgs e)
//        {
//            if (!openQueries.ContainsKey(e.NewName))
//            {
//                string oldName = e.Element.GetFullName(e.OldName);
//                ucQueriesEditorItem item = openQueries[oldName];

//                RemoveEditorFromList(oldName);
//                openQueries.Add(e.Element.GetFullName(e.NewName), item);
//                item.UpdateCaption(e.NewName);
//            }
//            else
//            {
//                e.Cancel = true;
//            }
//        }


//        public void OpenElementByErrorInfo(VErrorInfo errorInfo)
//        {
//            if (errorInfo.ElementInfo == null) return;
//            var elemetType = errorInfo.ElementInfo.Name.LocalName;
//            var keyName = XmlReports.Environment.GetKeyName(elemetType);
//            var prEl = VExceptionController.GetProcessedElementAndClear();

//            if (errorInfo.ElementInfo.Attribute(keyName) == null)
//            {
//                var sc = Cmn.GetAttrValue(errorInfo.ElementInfo,TextConst.AName.Comment);
//                if (sc.StartsWith("form ")) {
//                    elemetType = TextConst.AName.Form;
//                    keyName = XmlReports.Environment.GetKeyName(elemetType);
//                    errorInfo.ElementInfo.SetAttributeValue(keyName, sc.Split(' ')[1]);
//                }
//            }

//            if (errorInfo.ElementInfo.Attribute(keyName) == null)
//            {
//                errorInfo.ElementInfo = prEl;
//                if (errorInfo.ElementInfo == null) return;
//                elemetType = errorInfo.ElementInfo.Name.LocalName;
//                keyName = XmlReports.Environment.GetKeyName(elemetType);
//            }


//            var parentName = XmlReports.Environment.GetTypeParenElementtName(elemetType);
           

//            var id = errorInfo.ElementInfo.Attribute(keyName).Value;
//            var element = XmlReports.Environment.GetElement(parentName, id, null);

//            if (element == null)
//            {
//                if (parentName == TextConst.EName.Forms)
//                {
//                    parentName = TextConst.EName.Queries;
//                    element = XmlReports.Environment.GetElement(EName.queries, id, TextConst.AName.Name);
//                }
//            }

//            List<VSXElement> nodes=new List<VSXElement>();
//            string[] columnElsNames = new string[]{

//                TextConst.EName.Column,
//                TextConst.EName.Fact,
//                TextConst.EName.Multireference,
//                TextConst.EName.Field
               
                
//            };

//            string[]  fieldElsNames = new string[]{
//                TextConst.EName.Field,
//                TextConst.EName.UseField
//            };
//            string[] qryCallElsNames = new string[]{

//                TextConst.EName.Link,
//                TextConst.EName.DLink,
//                TextConst.EName.ELink,
//                TextConst.EName.Query,
//                TextConst.EName.SLink
                
//            };
//            string[] idAttributes = new string[]{

//                TextConst.AName.As,
//                TextConst.AName.Name
                
//            };

//            if (errorInfo.NodeInfo != null)
//            {
//                if (errorInfo.NodeInfo.Name == EName.useparam) {
//                    string name = errorInfo.NodeInfo.AttrOrEmpty(AName.name);
//                    var list = element.GetDescedantsP(EName.useparam).Where(f => f.P_Name == name);
//                    nodes.AddRange(list);
//                }
//                else
//                {
//                    if (element is VSourcedElement)
//                    {
//                        var qry = element as VSourcedElement;





//                        if (fieldElsNames.Contains(errorInfo.NodeInfo.Name.LocalName))
//                        {
//                            var alias = Cmn.GetAttrValue(errorInfo.NodeInfo, TextConst.AName.Name);

//                            var fields = qry.ParamFields().Where(f => f.P_FormalParNameS == alias);

//                            nodes.AddRange(fields);

//                        }
//                        if (nodes.Count == 0) {
//                            if (columnElsNames.Contains(errorInfo.NodeInfo.Name.LocalName)) {
//                                var alias = Cmn.GetAttrValue(errorInfo.NodeInfo, TextConst.AName.As);
//                                var tableName = Cmn.GetAttrValue(errorInfo.NodeInfo, TextConst.AName.Table);
//                                if (alias != "")
//                                {

//                                    nodes.AddRange(qry.AllUsedColumnsByTableNameAndColumnAlias(tableName, alias));
//                                }
//                                else
//                                {

//                                    var colName = Cmn.GetAttrValue(errorInfo.NodeInfo, TextConst.AName.Column);
//                                    if (colName == "")
//                                    {
//                                        colName = Cmn.GetAttrValue(errorInfo.NodeInfo, TextConst.AName.Name);
//                                        nodes.AddRange(qry.AllUsedColumnsByTableNameAndColumnAlias(tableName, colName));
//                                    }
//                                    else
//                                    {

//                                        nodes.AddRange(qry.AllUsedColumnsByTableNameAndColumnName(tableName, colName));
//                                    }


//                                }
//                            }
//                            else if (qryCallElsNames.Contains(errorInfo.NodeInfo.Name.LocalName))
//                            {
//                                var alias = Cmn.GetAttrValue(errorInfo.NodeInfo, TextConst.AName.As);
//                                if (alias == "")
//                                {
//                                    alias = Cmn.GetAttrValue(errorInfo.NodeInfo, TextConst.AName.Name);
//                                }
//                                nodes.AddRange(qry.GetSourceByAlias(alias));
//                            }

//                            else
//                            {
//                                foreach (var found1 in qry.GetExtensionsAndParentAndMain().SelectMany(q => q.GetDescedantsP(errorInfo.NodeInfo.Name)))
//                                {
//                                    bool found = true;
//                                    foreach (string an in idAttributes)
//                                    {
//                                        if (Cmn.GetAttrValue(found1, an) != Cmn.GetAttrValue(found1, an))
//                                        {
//                                            found = false;
//                                            break;
//                                        }
//                                    }
//                                    if (found)
//                                    {
//                                        nodes.Add(found1);
//                                    }
//                                }
//                            }
//                        }
//                    }
//                }
//                VSXElement.SetElemntsWithError(nodes);
//            }
//            if (nodes.Count != 0) {
//                VSXElement node = nodes[0];
//                OpenQuery(node.GetMainParent(), node);
//            } else {
//                OpenQuery(element, null);
//            }
//        }
//        void Query_OpenUses(object sender, XElementEventArgs e)
//        {
//            ucQueriesEditorItem item = null;
//            foreach (VSXElement el in e.Elements)
//            {
//                item = OpenElement(el.GetMainParent(), el);
//            }
//            if (item != null) item.QueryEditor.RaiseFirstEnter();
//        }
//        void Query_Closed(object sender)
//        {
//            var doc = documentManager1.GetDocument((Control)sender);
//            var tab_info = openQueries.Values.First(x => x.Doc == doc);
//            tab_info.DetachEvents();
//            RemoveEditorFromList(tab_info.Query.GetFullName());
//            tab_info.QueryEditor.UnsubscribeDataTableEvents();

//            // чистим из реестра
//            var name = string.Format("{0} {1}", tab_info.Query.Name.LocalName, tab_info.Query.P_IdName);
//            Cmn.WriteXElementToRegistry(TextConst.RegPath.SchemeOpenItems, name, null);
//        }
//        string GetFullName(VSXElement element, string name)
//        {
//            return element.Name.LocalName + " " + name;
//        }

//        void CreateElement(string elementType, string templateName)
//        {
//            var element = XmlReports.Environment.CreateElement(elementType, templateName);

//            OpenQuery(element, null);
//        }
//        ucQueriesEditorItem OpenElement(VSXElement element, VSXElement target, bool activate = true)
//        {
//            return OpenQuery(element, target, activate);
//        }
//        void CopyElement()
//        {

//            var editor = ActiveEditor();
//            if (editor == null) return;

//            string elementType = editor.Query.Name.LocalName;

//            VSXElement element = XmlReports.Environment.CreateElement(elementType, editor.Query, editor.Query.AttrOrEmpty(AName.file));

//            OpenQuery(element, null);
//        }
//        internal static VSXElement FindElementByInfo(VEnvironment environment, XElement info)
//        {
//            return environment.GetElement(
//                info.Attribute("parent-name").Value,
//                info.Attribute(AName.key).Value,
//                info.Attribute(AName.key_name).Value,
//                info.Attribute(AName.file).Value,
//                info.AttrOrDefault(AName.name, null));
//        }
//        void RemoveEditorFromList(string name)
//        {
//            openQueries.Remove(name);
//        }
//        void SearchElements(string searchString)
//        {
//            // словил эксепшн? вот тебе пример синтаксиса 
//            // //*[@function="ipr_tituls_display.getSumCharValues_ByKS14"]
//            // не благодари :)


//            //string expression = "/*" + searchString;
//            string expression = searchString;

//            //foundElements = System.Xml.XPath.Extensions.XPathSelectElements(GetEnvironment().SchemeNative.First(), "/root/*" + searchString).ToList().Select(e => VSXElement.Get(e).GetMainParent()).Distinct().ToList();

//            var hashSet = new HashSet<VSXElement>();
//            foreach (var e1 in XmlReports.Environment.Manager.GetNativeScheme().ToArray())
//            {
//                foreach (var e2 in e1.XPathSelectElements(expression).ToArray())
//                {
//                    var main = VSXElement.Get(e2).GetMainParent();
//                    if (!hashSet.Contains(main)) hashSet.Add(main);
//                }
//            }

//            foreach (DataRow row in (gcQueries.DataSource as DataTable).Rows)
//            {
//                var element = (VSXElement)row["node"];
//                if (hashSet.Contains(element))
//                {
//                    row["mark"] = "1";
//                }
//                else
//                {
//                    row["mark"] = "0";
//                }
//            }

//            ColumnFilterInfo filter = new ColumnFilterInfo("[mark] = '1'");
//            gvQueries.ActiveFilter.Add(gvQueries.Columns["mark"], filter);
//        }

//        void UpdateCreationList()
//        {
//            if (creationListReady) return;
//            btnMenuCreate.ClearLinks();
//            List<string> types = new List<string>();
//            foreach (XElement el in XmlReports.Environment.Manager.GetNativeScheme().Elements().Where(e1 => e1.Attribute(TextConst.AName.ChildName) != null))
//            {
//                if (!types.Contains(el.Attribute(TextConst.AName.ChildName).Value))
//                {

//                    types.Add(el.Attribute(TextConst.AName.ChildName).Value);
//                    var xtemps = el.Elements().Where(e1 => e1.Attribute(TextConst.AName.TemplateName) != null);

//                    if (xtemps.Count() > 0)
//                    {
//                        BarSubItem si = AddMenuItem(btnMenuCreate);
//                        si.Caption = el.Attribute(TextConst.AName.ChildName).Value;
//                        foreach (XElement xtemp in xtemps)
//                        {
//                            var info = new Tuple<string, string>(el.Attribute(TextConst.AName.ChildName).Value, xtemp.Attribute(TextConst.AName.TemplateName).Value);
//                            BarButtonItem btn = AddMenuButton(si);
//                            btn.Caption = xtemp.Attribute(TextConst.AName.TemplateName).Value;
//                            btn.Tag = info;
//                            btn.ItemClick += createClick;
//                        }
//                    }
//                    else
//                    {
//                        var info = new Tuple<string, string>(el.Attribute(TextConst.AName.ChildName).Value, null);
//                        BarButtonItem btn = AddMenuButton(btnMenuCreate);
//                        btn.Caption = el.Attribute(TextConst.AName.ChildName).Value;
//                        btn.ItemClick += createClick;
//                        btn.Tag = info;
//                    }
//                }
//            }
//            creationListReady = true;
//        }
//        BarSubItem AddMenuItem(BarSubItem parent)
//        {

//            var item = new BarSubItem();
//            item.Caption = "yep";
//            parent.AddItem(item);
//            return item;
//        }
//        BarButtonItem AddMenuButton(BarSubItem parent)
//        {

//            var item = new BarButtonItem();
//            parent.AddItem(item);
//            return item;
//        }

//        private void ucQueriesEditor_FocusedQueryChanged(object sender, FocusedNodeChangedEventArgs e)
//        {
//            var editor = (ucQueryEditor)sender;
//            var xquery = editor.Query;
//            var xtarget = editor.CurrentElement();

//            if (_current_position > -1)
//            {
//                var mh_item = _move_history[_current_position];
//                if (mh_item.Item1 == xquery.GetHashCode() && mh_item.Item2 == ((xtarget != null) ? xtarget.GetHashCode() : 0)) return;
//            }

//            AddToMoveHistory(xquery, xtarget);
//            UpdateMoveHistoryButtonsState();

//            if (editor.queryLoaded) editor.SaveState();
//        }


        
//        private void tabbedView1_DocumentActivated(object sender, DocumentEventArgs e)
//        {
//            GetCurrentControl<ucQueriesEditor>().SaveGlobalInfoInReg();
//        }
//        private void ucQueriesEditor_Load(object sender, EventArgs e)
//        {
//            OnBaseEvent(ucBaseEventType.ExecuteStart, "Загрузка редактора");
//            this.Visible = false;

//            XmlReports.Environment.Manager.GetController().LoadState();

//            RefreshQueryList();
//            OpenItemsUsingRegInfo();
//            LoadGlobalInfoInReg();
//            UpdateMoveHistoryButtonsState();
//            this.Visible = true;

//            OnBaseEvent(ucBaseEventType.ExecuteComplete);
//        }
//        void tabbedView1_DocumentClosing(object sender, DocumentCancelEventArgs e)
//        {
//            Query_Closed(e.Document.Control);
//        }
//        void gvQueries_RowClick(object sender, RowClickEventArgs e)
//        {
//            if (e.RowHandle != -1)
//            {
//                object node = gvQueries.GetRowCellValue(e.RowHandle, "node");

//                VSXElement el = VSXElement.Get((node as XElement));
//                gvQueries.SetRowCellValue(e.RowHandle, "node", el);
//                if (node != null)
//                {
//                    OpenQuery(el, null);
//                }
//            }
//        }

//        void ribbonControl1_Click(object sender, EventArgs e)
//        {

//        }
//        void btnMenuCreate_ItemPress(object sender, ItemClickEventArgs e)
//        {
//            this.GetRibbonSource<ucQueriesEditor>().UpdateCreationList();
//        }
//        void createClick(object sender, ItemClickEventArgs e)
//        {
//            var info = (Tuple<string, string>)(e.Item.Tag);
//            GetCurrentControl<ucQueriesEditor>().CreateElement(info.Item1, info.Item2);
//        }
//        void bthCopyElement_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucQueriesEditor>().CopyElement();
//        }
//        void btneSearchElement_ButtonClick(object sender, DevExpress.XtraEditors.Controls.ButtonPressedEventArgs e)
//        {
//            var val = btneSearchElement.EditValue ?? "";
//            SearchElements(val.ToString());
//        }

//        #region Сохранение и загрузка
//        private void SaveGlobalInfoInReg()
//        {
//            var doc = tabbedView1.ActiveDocument;
//            if (doc == null) return;

//            var editor = ActiveEditor();
//            if (editor == null) return;

//            var data = new XElement(TextConst.EName.Root, new XAttribute("current-tab-name", doc.Caption.TrimEnd('*')));
//            Cmn.WriteXElementToRegistry(TextConst.RegPath.SchemeEditor, TextConst.RegVal.Global, data);
//        }
//        private void LoadGlobalInfoInReg()
//        {
//            var data = Cmn.ReadXElementFromRegistry(TextConst.RegPath.SchemeEditor, TextConst.RegVal.Global);
//            var cur_tab_name = data.AttrOrDef("current-tab-name", "");

//            ucQueryEditor cur_query = null;
//            if (openQueries.Keys.Contains(cur_tab_name))
//            {
//                cur_query = openQueries[cur_tab_name].QueryEditor;
//            }
//            else if (openQueries.Any())
//            {
//                cur_query = openQueries.First().Value.QueryEditor;
//            }
//            if (cur_query == null) return;

//            cur_query.RaiseFirstEnter();
//            tabbedView1.ActivateDocument(cur_query.Parent);
//        }



//        private void OpenItemsUsingRegInfo()
//        {
//            var list = Cmn.ReadAllSubXElementsFromRegistry(TextConst.RegPath.SchemeOpenItems);
//            foreach (XElement info in list)
//            {
//                XElement xschemecolumns = null;
//                XElement el = null;
//                if (info.Name.LocalName == "root")
//                {
//                    el = info.Element("element");
//                    //xschemecolumns = info.Element("scheme-columns");
//                }
//                else
//                {
//                    // поддержка загрузки старого варианта
//                    // потом убрать
//                    el = info;
//                }

//                VSXElement element = FindElementByInfo(XmlReports.Environment, el);

//                if (element != null)
//                {
                   
//                    VSXElement target = FindTargetByInfo(element, el);
//                    OpenElement(element, target, false);
//                }
//            }
//        }

//        public static VSXElement FindTargetByInfo(VSXElement element,XElement info)
//        {
//            string posId = Cmn.GetAttrValue(info, TextConst.AName.CurrentNode);
//            VSXElement target = element.GetElementByPositionId(posId);
//            return target;
//        }

//        private void btnTest2_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            //SaveItemInfoInReg(thisControl<ucQueriesEditor>().openQueries.First().Value.QueryEditor);
//        }
//        #endregion

//        #region Вперёд/Назад

//        Dictionary<int, VSXElement> _move_cash = new Dictionary<int, VSXElement>();
//        void AddToMoveHistory(VSXElement xquery, VSXElement xtarget)
//        {
//            if (_current_position > -1)
//            {
//                _move_history.RemoveRange(_current_position, _move_history.Count - _current_position - 1);
//            }

//            int hashQuery = xquery.GetHashCode();
//            _move_cash[hashQuery] = xquery;
//            int hashTarget = (xtarget != null) ? xtarget.GetHashCode() : 0;
//            _move_cash[hashTarget] = xtarget;

//            var mh_item = new Tuple<int, int>(hashQuery, hashTarget);
//            _move_history.Add(mh_item);
//            _current_position++;
//        }
//        void BackMoveHistory()
//        {
//            if (_current_position <= 0) return;

//            _current_position--;
//            var mh_item = _move_history[_current_position];
//            OpenQuery(_move_cash[mh_item.Item1], _move_cash[mh_item.Item2]);
//        }
//        void ForwardMoveHistory()
//        {
//            if (_current_position >= _move_history.Count - 1) return;

//            _current_position++;
//            var mh_item = _move_history[_current_position];
//            OpenQuery(_move_cash[mh_item.Item1], _move_cash[mh_item.Item2]);
//        }

//        void ClearMoveHistory()
//        {
//            _current_position = -1;
//            _move_history.Clear();
//            _move_cash.Clear();
//            UpdateMoveHistoryButtonsState();
//        }

//        void UpdateMoveHistoryButtonsState()
//        {
//            //ribSrcControl<ucQueriesEditor>().ribbonControl1.Minimized = true;
//            ucQueriesEditor rib = this.GetRibbonSource<ucQueriesEditor>();
//            rib.btnMoveBack.Enabled = (_current_position > 0);
//            rib.btnMoveForward.Enabled = (_current_position > -1 && _current_position < _move_history.Count - 1);
//        }

//        void btnMoveBack_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucQueriesEditor>();
//            ctrl.BackMoveHistory();
//            ctrl.UpdateMoveHistoryButtonsState();
//        }
//        void btnMoveForward_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucQueriesEditor>();
//            ctrl.ForwardMoveHistory();
//            ctrl.UpdateMoveHistoryButtonsState();
//        }
//        #endregion
//        public void MakeMultipleQuerySchemeFromDb(string tableNames, string project)
//        {
//            tableNames = tableNames.ToLower().Replace(',', ' ').Replace('\n', ' ').Replace('\r', ' ').Replace('\t', ' ');
//            while (tableNames.Contains("  ")) {
//              tableNames = tableNames.Replace("  ", " ");
//            }
//            string[] list = tableNames.Split(' ');
//            for (int index = 0; index < list.Length; index++) {
//                string table_name = list[index];
//                OnBaseEvent(ucBaseEventType.ExecuteStart, "Загрузка " + table_name);
//                MakeQuerySchemeFromDb(table_name, project);
//                OnBaseEvent(ucBaseEventType.ExecuteComplete);
//            }
//        }
//        public static void MakeQuerySchemeFromDb(string tableName, string project)
//        {
//            // 30.10.14 В.Емцов
//            tableName = tableName.ToLower();
           
//            var new_file = XmlSchemeBuilder.XmlTableStructure(tableName);
//            if (new_file == null) return;

//            var proj = XmlReports.Environment.Manager.GetProject(project);
//            string filename = Path.Combine(proj.ProjectPath, "scheme", "original", tableName + ".xml");

//            if (!File.Exists(filename))
//            {
//                Cmn.CreateFolder(filename);
//                Cmn.SaveXmlWithCheckOut(new XDocument(new_file), filename);

//                bool success = false;
//                while (!success)
//                {
//                    using (var tfs = new TFSServer())
//                    {
//                        success = tfs.AddFile(filename);
//                    }


//                    if (!success)
//                    {
//                        var res = ShowMessage.ShowQuestion("Не удалось добавить файл в TFS. Повторить попытку?");
//                        if (res != DialogResult.Yes) break;
//                    }
//                }
//            }
//            else
//            {
//                TFSHelper.CheckOutFile(filename);
//                var old_file = Cmn.OpenXmlClearNS(filename).Root;
//                if (!old_file.Elements().Elements().Any())
//                {
//                    File.Delete(filename);
//                    Cmn.CreateFolder(filename);
//                    Cmn.SaveXmlWithCheckOut(new XDocument(new_file), filename);
//                }
//                else
//                {
//                    var updated_file = XmlSchemeBuilder.UpdateScheme(old_file, new_file);
//                    Cmn.SaveXmlWithCheckOut(new XDocument(updated_file), filename);
//                }
//            }
//        }


//        private void barButtonItem2_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucQueriesEditor>();
//            ctrl.ReloadAndRefreshQueryList();
//            ctrl.ClearMoveHistory();
//        }



//        public void ReloadAndRefreshQueryList()
//        {
//            var ctrl = this;
//            ctrl.OnBaseEvent(ucBaseEventType.ExecuteStart, "Перезагрузка схемы и списка");

//            XmlReports.Init();
//            XmlReports.Environment.Manager.GetController().LoadState();
//            ctrl.RefreshQueryList();

//            ctrl.OnBaseEvent(ucBaseEventType.ExecuteComplete);
//        }

//        private void btnClearZipCache_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            //Cache.Clear();
//        }

//        private void btnClearMemCache_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            VCashUtils.ClearCash();
//        }
//        public void SaveAll()
//        {
//            var editors = GetCurrentControl<ucQueriesEditor>().openQueries.Values
//             .Where(q => q.Loaded && q.QueryEditor.IsChanged())
//             .Select(q => q.QueryEditor);

//            foreach (var editor in editors)
//            {
//                editor.SaveQuery();
//                editor.SetChanged(false);
//            }

//            VCashUtils.ClearCash();
//        }
//        private void btnSaveAll_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            SaveAll();
//			sql.builder.UI.CommandItems.UIFormsPool.Reset();
//            //SaveAll();
//        }

//        private void btnShowProjectManager_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            GetCurrentControl<ucQueriesEditor>().ShowProjectsManager();
//        }

//        private void ShowProjectsManager()
//        {
//            using (var frm = new FormBase() { Width = 800, Height = 600, Text = "Менеджер проектов" })
//            {
//                var controller = XmlReports.Environment.Manager.GetController();
//                var ctrl = new ucProjects() { Dock = DockStyle.Fill };
//                ctrl.Initialize(controller);
//                frm.Controls.Add(ctrl);

//                controller.ProjectsStateChanged += OnProjectsStateChanged;
//                frm.ShowDialog();
//                controller.ProjectsStateChanged -= OnProjectsStateChanged;
//            }
//        }

//        internal void OnProjectsStateChanged(object sender, ProjectsStateChangedArgs args)
//        {
//            VCashUtils.ClearCash();
//            RefreshQueryList();
//        }
//        #region Загрузить из БД (по имени)
//        private void askDbObjName(string projName)
//        {
//            UIStatic.LoadProject("system");
//            UIFormC form = UIStatic.CreateSystemForm("sys_text_input", true);
//            form.SetTitle("Загрузка объектов в проект " + projName);
//            form.SetProp("proj", projName);
//            if (form.IsNew) {
//                form.OnButtonClick += this.form_OnButtonClick;
//                form.GetParamField("value").SetTitle("Имена объектов");
//            }
//            form.LoadData(null);
//            form.ShowDialog();
//        }
//        private void form_OnButtonClick(UIFormC form, XElement value)
//        {
//            if (value.AttrOrEmpty(AName.name) == "ok") {
//                string stables = form.GetParamField("value").GetValue().ToString();
//                MakeMultipleQuerySchemeFromDb(stables, (string)form.GetPropVal("proj"));
//                ReloadAndRefreshQueryList();
//            }
//        }
//        private void lcLoadFromDB_Popup(object sender, EventArgs e)
//        {
//            this.ResetProjButtons();
//        }
//        private void btnLoadFromDB_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            var ctrl = GetCurrentControl<ucQueriesEditor>();
//            //if (string.IsNullOrEmpty(ctrl.btneSearchElement.Text))
//            //{
//            //    return;
//            //}
//            ctrl.askDbObjName(e.Item.Caption);
//            //ctrl.MakeMultipleQuerySchemeFromDb(ctrl.btneSearchElement.Text, e.Item.Caption);
//            //ReloadAndRefreshQueryList();
//        }
//        #endregion
//    }

//    internal class ucQueriesEditorItem
//    {
//        private ucQueryEditor _queryEditor;
//        public ucQueryEditor QueryEditor
//        {
//            get
//            {
//                if (!Loaded) Load();
//                return _queryEditor;
//            }
//        }

//        private BaseDocument _doc;
//        public BaseDocument Doc
//        {
//            get { return _doc; }
//        }

//        public bool Loaded
//        {
//            get
//            {
//                return (_queryEditor != null);
//            }
//        }

//        private VSXElement _query;
//        public VSXElement Query
//        {
//            get { return _query; }
//        }

//        private VSXElement _target;
//        public VSXElement Target
//        {
//            set { _target = value; }
//        }

//        private FocusedNodeChangedEventHandler _focused_changed;
//        private XElementEventHandler _open_uses;
//        private VRenameEventHandler _renamed;
//        private EventHandler _needClose;
//        private EventHandler _needRefreshScheme;
//        //private XElement _xschemecolumns;
//        //public XElement xSchemeColumns
//        //{
//        //    get { return _xschemecolumns; }
//        //}

//        public ucQueriesEditorItem(BaseDocument doc, XElement xquery, VSXElement xtarget, FocusedNodeChangedEventHandler focused_changed, XElementEventHandler open_uses, VRenameEventHandler renamed, EventHandler need_close, EventHandler need_refreshScheme)
//        {
//            _doc = doc;
//            _focused_changed = focused_changed;
//            _open_uses = open_uses;
//            _renamed = renamed;
//            _needClose = need_close;
//            _needRefreshScheme = need_refreshScheme;
//            //_xschemecolumns = xschemecolumns;

//            //string fileName = Cmn.GetAttrValue(xquery, "file");
//            //if (fileName != "")
//            //{
//            //    fileName = ucQueryEditor.AddPathToFilename(Cmn.GetAttrValue(xquery, "file"));
//            //}
//            _query = VSXElement.Get(xquery);
//            UpdateElementFileInfo(_query);
//            //_query.SourceFileName = fileName;

//            _target = xtarget;
//        }
//        void _queryEditor_NeedClose(object sender, EventArgs e)
//        {
//            _needClose(this,null);
//        }

//        void _queryEditor_NeedRefreshScheme(object sender, EventArgs e)
//        {
//            _needRefreshScheme(this, null);
//        }

//        public static void UpdateElementFileInfo(VSXElement element)
//        {
//            string fileName = Cmn.GetAttrValue(element, "file");
//            if (fileName != "")
//            {
//                fileName = ucQueryEditor.AddPathToFilename(fileName);
//            }

//            element.SourceFileName = fileName;
//        }

//        public void UpdateCaption(string name = null)
//        {
//            _doc.Caption = _query.GetFullName(name);
//        }

//        public void Load()
//        {
//            if (!Loaded)
//            {
//                _queryEditor = new ucQueryEditor() { Dock = DockStyle.Fill };
//                _queryEditor.FocusedQueryChanged += _focused_changed;
//                _queryEditor.OpenUses += _open_uses;
//                _queryEditor.Renamed += _renamed;
//                _queryEditor.Query = _query;
//                _queryEditor.SelectTarget = _target;
//                _queryEditor.NeedClose += _queryEditor_NeedClose;
//                _queryEditor.NeedRefreshScheme += _needRefreshScheme;
//                // _queryEditor.SchemeColumns = _xschemecolumns;
//                _doc.Control.Controls.Add(_queryEditor);
//            }
//        }

      


//        public void DetachEvents()
//        {
//            if (Loaded && _queryEditor!=null)
//            {
               
//                _queryEditor.FocusedQueryChanged -= _focused_changed;
//                _queryEditor.OpenUses -= _open_uses;
//                _queryEditor.Renamed -= _renamed;
               
//                _queryEditor.SelectTarget = _target;
//                _queryEditor.NeedClose -= _queryEditor_NeedClose;
//                _queryEditor.NeedRefreshScheme -= _needRefreshScheme;
//                _queryEditor.DetachEvents();
//            }
           
//        }


        

       
//    }
//}
