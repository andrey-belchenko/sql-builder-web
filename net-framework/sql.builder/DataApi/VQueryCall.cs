using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using System.Diagnostics;
using sql.builder.FieldInfo;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal abstract partial class VQueryCall : VSXElement 
    {
        protected VQueryCall(XName name)
            : base(name)
        {
        }
        protected VQueryCall(XElement element)
            : base(element)
        {
        }
        //public VLink AddLink(string name)
        //{
        //    VLink link= new VLink();
        //    link.SetAttributeValue("name", name);
        //    this.Add(link);
        //    return link;
        //}
        //public VDLink AddDLink(string name)
        //{
        //    VDLink link = new VDLink();
        //    link.SetAttributeValue("name", name);
        //    this.Add(link);
        //    return link;
        //}
        //internal VQuery GetOwnerQuery()
        //{
        //    return (VQuery)this.GetAncestor(EName.from).Parent;
        //}
        internal string GetRelChildColumnName() // колонка, которая ссылается на родителя, для query в отчете, частный случай для генерации кода
        {
            VSXElement call = this.GetElementsP(EName.call).FirstOrDefault();
            if (call != null) {
                VSXElement col = call.GetDescedantsP(EName.column).FirstOrDefault(c => c.P_Table == this.XName);
                if (col != null) {
                    return col.XName;
                }
            }
            return null;
        }
        // См. перегруженные версии в VQuery и VParam
        public virtual VQuery Query()
        {
            VQuery q = XmlReports.Environment.GetQuery(this.SName());
            return q;
        }
        public override string XName
        {
            get {
                XAttribute attr = this.Attribute(AName_.@as);
                if (attr == null) {
                    attr = this.Attribute(AName_.name);
                }
                if (attr != null) {
                    return attr.Value;
                } else {
                    return null;
                }
            }
        }
        public virtual string XTitle
        {
            get {
                XAttribute attr = this.Attribute(AName_.title);
                if (attr != null) {
                    return attr.Value;
                } else {
                    VRelation rel = this.GetRelation();
                    if (rel != null) {
                        string title = rel.XTitle;
                        if (!string.IsNullOrEmpty(title)) {
                            return title;
                        }
                    }
                    VQuery query = this.Query();
                    if (query != null) {
                        return query.Title();
                    } else {
                        return null;
                    }
                }
            }
        }
        protected VQuery LinkParentQuery()
        {
            VSXElement parent = GetParent();          
            if (parent is VLinks) {
                return (VQuery)this.RootQuery();
            }
            VQueryCall qc = parent as VQueryCall;
            if (qc != null) {
                return qc.Query();
            } else {
                return null;
            }
        }
        /*public VQueryCall LinkMainQuery()
        {
            var qry = this;
            var parent = qry.LinkParent();
            while (parent != null)
            {
                qry = parent;
                parent = qry.LinkParent();
            }
            return qry;
        }*/
        private VQueryCall LinkParent()
        {
            return this.GetParent() as VQueryCall;
        }
        public string PName()
        {
            XAttribute attr = this.Attribute(AName_.@as);
            if (attr != null) {
                return attr.Value;
            } else {
                return this.AttrOrEmpty(AName_.name);
            }
        }
        public virtual VRelation GetRelation()
        {
            return null;
        }
        public VQueryCall GetDimensionLink()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as VQueryCall);
            }
            VQueryCall l = (VQueryCall)this.GetAncestorsAndSelf().FirstOrDefault(e => e.GetParent() is VQube || e.GetParent() is VFrom || e.GetParent() is VDimSet);
            if (l != null) {
                if (l.GetParent() is VFrom) {
                    l = null;
                }
            }
            AddCashValue(l, MethodBase.GetCurrentMethod().ToString(), null);
            return l;
        }
        internal List<VQueryCall> Links(VSourcedElement heir)
        {
            var links = new List<VQueryCall>();
            foreach (VQueryCall link in this.GetElementsP().Where(e1=> e1 is VQueryCall  & !(e1 is VDimSet))) {
                links.Add(link);
            }
            if (this is VTable || (this.PreviousNode == null && this.Name == EName.query && this.P_Join == "")) {
                foreach (VQueryCall link in this.RootQuery().GetNamedSections(TextConst.EName.Links).SelectMany(VSXElement.GetElementsP).Where(e1 => e1 is VQueryCall).ToList()) {
                    links.Add(link);
                }
                if (heir != null) {
                    foreach (VQueryCall link in heir.RootQuery().GetNamedSections(TextConst.EName.Links).SelectMany(VSXElement.GetElementsP).Where(e1 => e1 is VQueryCall).ToList()) {
                        links.Add(link);
                    }
                }
            }
            return links;
        }
        internal IList<VQueryCall> SelfAndELinks()
        {
            IList<VSXElement> links = this.GetDescedantsP(EName.elink);
            var list = new List<VQueryCall>(links.Count + 1);
            list.Add(this);
            for (int index = 0; index < links.Count; index++) {
                VQueryCall link = (VQueryCall)links[index];
                list.Add(link);
            }
            return list;
        }
        private IList<VQueryCall> MasterLinks()
        {
            IList<VQueryCall> links = this.Links(null);
            var list = new List<VQueryCall>(links.Count);
            for (int index = 0; index < links.Count; index++) {
                VQueryCall link = links[index];
                if ((link.GetType() != typeof(VELink))) {
                    list.Add(link);
                }
            }
            return list;
        }
        internal IList<VQueryCall> SelfAndAllMasterLinks()
        {
            var list = new List<VQueryCall>(1);
            list.Add(this);
            IList<VQueryCall> links = this.MasterLinks();
            for (int index = 0; index < links.Count; index++) {
                VQueryCall link = links[index];
                list.AddRange(link.SelfAndAllMasterLinks());
            }
            return list;
        }
        public virtual List<VQueryCall> AllLinks(VSourcedElement heir)
        {
            var links = new List<VQueryCall>();
            foreach (VQueryCall link in Links(heir)) {
                links.Add(link);
                foreach (VQueryCall link1 in link.AllLinks(heir)) {
                    links.Add(link1);
                }
            }
            return links;
        }
        //public VColumn AddColumn(string name)
        //{
        //    return this.GetOwnerQuery().AddColumn(this.XName, name);
        //}
        internal string SName()
        {
            return this.AttrOrEmpty(AName_.name);
        }
        //public List<VColumn> UsedColumns(string columnName)
        //{
        //    return UsedColumns().Where(e => e.P_Column == columnName).ToList();
        //}
        public override List<VColumn> UsedColumns()
        {
            List<VColumn> cols = new List<VColumn>();
            VSourcedElement rootQuery = RootQuery();
            if (rootQuery == null) {
                return cols;
            }
            IEnumerable<XElement> cols1 = rootQuery.AllUsedColumns().Where(e1 => e1.AttrOrEmpty(AName_.table) == this.XName);
            //.Where(e => !e.Ancestors("query").First(e2=>e2.Element("select")!=null).IsAfter(rootQuery)) 
            foreach (VColumn col in cols1) {
                cols.Add(col);
            }
            return cols;
        }
        /*public void ChangeAlias(string newAlias)
        {
            //string newAlias1 = newAlias;
            //if (newAlias == "")
            //{
            //    newAlias1 = GetAttrValue("name");
            //}
            //  List<VColumn> listCol = UsedColumns();
            if (newAlias == "") {
                this.RemoveAttribute(AName_.As);
            } else {
                this.SetAttrValue(AName_.As, newAlias);
            }
            //foreach (VColumn col in listCol)
            //{
            //    col.P_Table_Set(newAlias1);
            //}
        }*/
        /*public void ChangeName(string newName)
        {
            //List<VColumn> listCol = null;
            //if (this.Attribute("as") == null) {
            //     listCol = UsedColumns();
            //}
            this.SetAttrValue(AName_.name, newName);
            //return;
            //if (this.Attribute("as") == null)
            //{
            //    foreach (VColumn col in listCol)
            //    {
            //        col.P_Table_Set(newName);
            //    }
            //}
        }*/
        //public bool IsQuery()
        //{
        //    return !(this is VForm);
        //}
        protected bool IsCall()
        {
            return !(this.IsMainElement());
        }
        internal bool IsQube()
        {
            return this.Elements(EName.select).Elements(EName.column).Any(e => e.AttrOrEmpty(AName_.table) == "*");
        }
        public override void LookUpNextSources(List<VSXElement> list, VLookupAnalyzer analyzer)
        {

            var qry = Query();

            if (qry == null)
            {
                return;
            }

            if (!analyzer.CheckAndReturn(list, this))
            {
                return;
            }
        

            if (qry != this.RootQuery())
            {
                qry.LookUpNextSources(list, analyzer);
            }
            else
            {
                var qry1 = qry.GetMainIE();
                if (qry1 != this.RootQuery())
                {
                    qry1.LookUpNextSources(list, analyzer);
                }
                else
                {

                    var qry2 = qry.MainSource();
                    if (qry2!=null && qry2.Query() != this.RootQuery())
                    {
                        qry2.LookUpNextSources(list, analyzer);
                    }
                }
            }
        }
        #region CalledQuery
        public override string P_CalledQuery {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetAttrValue(AName_.name, value);
            }
        }
        public override void P_CalledQuery_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
            table.AddColumn("query", "Таблица");
        }
        public override void P_CalledQuery_ListRefresh(VDataTable dt)
        {
            dt.Rows.Clear();
            HashSet<string> names = new HashSet<string>();
            foreach (XElement el in XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries).Elements(EName.query)) {
                if (el.Attribute(AName_.extend) == null) {
                    string name = el.AttrOrEmpty(AName_.name);
                    if (names.Contains(name)) {
                        Debug.WriteLine("Запрос c именем \"" + name + "\" дублируется, используйте XPath //queries/query[@name=\"" + name + "\"] для поиска дублей.");
                    } else {
                        string title = el.AttrOrEmpty(AName_.title);
                        XElement xtable = el.Elements(EName.from).Elements(EName.table).FirstOrDefault();
                        string table;
                        if (xtable != null) {
                            table = xtable.AttrOrEmpty(AName_.name);
                        } else {
                            table = string.Empty;
                        }
                        dt.AddRow(name, name, title, table);
                        names.Add(name);
                    }
                }
            }
        }
        public override bool P_CalledQuery_Exists()
        {
            return this.IsCall();
        }
        public override bool IsElementUser()
        {
            return true;
        }
        public override List<VSXElement> GetUsedElements()
        {
            VQuery query = this.Query();
            // 12.07.2017 Емцов - на qube dimset и прочих вылетает
            if (query == null) {
                return new List<VSXElement>();
            }
            if (query != this) {
                return query.GetExtensionsAndParentAndMain().Cast<VSXElement>().ToList();
            }
            return null;
        }
        public override string P_CalledQuery_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            string s = Bold(this.AttrOrEmpty(AName_.name));
            if (this is VRelation) {
                if ((this as VRelation).ParentQuery() == null) {
                    s += "[missing]";
                } else {
                    s += ColorGray("-" + this.P_DXName);
                }
            }
            string alias = this.AttrOrEmpty(AName_.@as);
            if (!string.IsNullOrEmpty(alias)) {
                s += " as " + Bold(alias);
            }
            string dimension = this.P_Dimension;
            if (!string.IsNullOrEmpty(dimension)) {
                s += " dim " + Bold(dimension);
            }
            string update_target = this.P_UpdateTarget;
            if (!string.IsNullOrEmpty(update_target)) {
                s += " into " + Bold(update_target);
            }
            string title = XTitle;
            if (!string.IsNullOrEmpty(title)) {
                s += " " + Italic(title);
            }
			if (this.P_OnlyForCond == TextConst.AVBool.True) {
				s += " " + ColorGold("conditions only");
			}
			//P_OnlyForCond==TextConst.AVBool.True
            if (this.P_AutoFilter == TextConst.AVBool.True) {
				s += " " + ColorGold("autofilter");
			}
            return s;
        }
        #endregion
        #region Alias
        public override string P_Alias {
            get {
                return this.AttrOrEmpty(AName_.@as);
            }
            set {
                this.SetAttributeNotEmpty(AName_.@as, value);
            }
        }
        public override bool P_Alias_Exists()
        {
            return true;
        }
        public override string P_Alias_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region Join
        public override string P_Join {
            get {
                return this.AttrOrEmpty(AName_.join);
            }
            set {
                this.SetAttributeNotEmpty(AName_.join, value);
            }
        }
        public void P_Join_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public void P_Join_ListRefresh(VDataTable table)
        {
            if (table.Rows.Count == 0) {
                VSXElement.FillDataTableFromStringArray(table, new string[] { TextConst.AVJoin.Inner, TextConst.AVJoin.LeftOuter, "right outer", "left inner", TextConst.AVJoin.Cross, "full outer", string.Empty });
            }
        }
        public override bool P_Join_Exists()
        {
            if (this.Attribute(AName_.join) != null) {
                return true;
            } else if (this.PreviousNode != null && this.IsCall()) {
                return true;
            } else {
                return false;
            }
        }
        #endregion
        #region ConstrDelOption
        public override string P_ConstrDelOption {
            get {
                return this.AttrOrEmpty("ConstrDelOption");
            }
            set {
                this.SetAttributeNotEmpty("ConstrDelOption", value);
            }
        }
        public void P_ConstrDelOption_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name");
        }
        public void P_ConstrDelOption_ListRefresh(VDataTable table)
        {
            VSXElement.FillDataTableFromStringArray(table, TextConst.AVConstrDelOptionsArray.All);
        }
        public override bool P_ConstrDelOption_Exists()
        {
            return this.PreviousNode != null && this.IsCall();
        }
        #endregion
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
           return true;
        }
        #endregion
        #region ColumnEditable
        public override bool P_ColumnEditable_Exists()
        {
            return this.RootQuery() is VForm;
        }
        #endregion
        #region BackColor
        public override bool P_BackColor_Exists()
        {
            return this.RootQuery() is VForm;
        }
        #endregion
        #region CanBeChecked
        public override bool P_CanBeChecked_Exists()
        {
            return this.RootQuery() is VForm;
        }
        #endregion
        #region MultiSelectColumn
        public override string P_MultiSelectColumn {
            get {
                return this.AttrOrEmpty(AName_.multi_select_column);
            }
            set {
                this.SetAttributeNotEmpty(AName_.multi_select_column, value);
            }
        }
        public void P_MultiSelectColumn_List(VDataTable table)
        {
            table.ClearColumns();
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
            table.AddColumn("etype", "Ссылка");
        }
        public void P_MultiSelectColumn_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            IList<VColumn> cols = this.UsedColumns();
            HashSet<string> check = new HashSet<string>();
            for (int index = 0; index < cols.Count; index++) {
                VColumn col = cols[index];
                if (!check.Contains(col.XName))
                {
                    check.Add(col.XName);
                    VSXElement.AddColumnInfoToList(table, col.XName, col);
                }
              
            }
        }
        public override bool P_MultiSelectColumn_Exists()
        {
            return this.P_ColumnEditable_Exists();
        }
        #endregion
        #region MultiSelectTarged
        public override string P_MultiSelectTarget {
            get {
                return this.AttrOrEmpty(AName_.multi_select_target);
            }
            set {
                this.SetAttributeNotEmpty(AName_.multi_select_target, value);
            }
        }
        public void P_MultiSelectTarget_List(VDataTable table)
        {
            table.AddColumn("name", "Форма");
        }
        public void P_MultiSelectTarget_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            VForm frm = this.RootQuery() as VForm;
            IList<VQueryCall> list = frm.MainAndRelatedQueries();
            for (int index = 0; index < list.Count; index++) {
                VQueryCall qry = list[index];
                table.AddRow(qry.XName);
            }
        }
        public override bool P_MultiSelectTarget_Exists()
        {
            return !string.IsNullOrEmpty(this.P_MultiSelectColumn);
        }
        #endregion
        #region Column // колонка с кодом на который дается ссылка при multiselect
        public override void P_Column_ListRefresh(VDataTable table) 
        {
            table.Rows.Clear();
            IList<VColumn> cols = this.UsedColumns();
            for (int index = 0; index < cols.Count; index++) {
                VColumn col = cols[index];
                VSXElement.AddColumnInfoToList(table, col.XName, col);
            }
        }
        public override bool P_Column_Exists()
        {
           return P_ColumnEditable_Exists();
        }
        #endregion
        #region NewRowsVisForOtherTbls
        public override string P_NewRowsVisForOtherTbls {
            get {
                return this.AttrOrEmpty(TextConst.AName.NewRowsVisForOtherTbls);
            }
            set {
                this.SetAttributeNotEmpty(TextConst.AName.NewRowsVisForOtherTbls, value);
            }
        }
        public override bool P_NewRowsVisForOtherTbls_Exists()
        {
            return this.RootQuery() is VForm;
        }
        #endregion
        #region Async
        public override string P_Async {
            get {
                return this.AttrOrEmpty(AName_.async);
            }
            set {
                this.SetAttributeNotEmpty(AName_.async, value);
            }
        }
        public override bool P_Async_Exists()
        {
            return this.RootQuery() is VForm;
        }
        #endregion
        #region AutoRefresh
        public override string P_AutoRefresh {
            get {
                return this.AttrOrEmpty(AName_.auto_refresh);
            }
            set {
                this.SetAttributeNotEmpty(AName_.auto_refresh, value);
            }
        }
        public override bool P_AutoRefresh_Exists()
        {
            return this.RootQuery() is VForm;
        }
        #endregion
        #region OnlyVisibleRefresh
        public override string P_OnlyVisibleRefresh {
            get {
                return this.AttrOrEmpty(TextConst.AName.OnlyVisibleRefresh);
            }
            set {
                this.SetAttributeNotEmpty(TextConst.AName.OnlyVisibleRefresh, value);
            }
        }
        public override bool P_OnlyVisibleRefresh_Exists()
        {
            return this.RootQuery() is VForm;
        }
        #endregion
        #region OnlyForceRefresh
        public override string P_OnlyForceRefresh {
            get {
                return this.AttrOrEmpty(TextConst.AName.OnlyForceRefresh);
            }
            set {
                this.SetAttributeNotEmpty(TextConst.AName.OnlyForceRefresh, value);
            }
        }
        public override bool P_OnlyForceRefresh_Exists()
        {
            return this.RootQuery() is VForm;
        }
        #endregion
        #region Dimension
        public override string P_Dimension {
            get {
                if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                    return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as string);
                }
                string t = string.Empty;
                if (!string.IsNullOrEmpty(this.P_PrDimension)) {
                    VQuery qry = this.Query();
                    if (qry != null) {
                        qry = (VQuery)(qry.GetMainE());
                        VDimension dim = qry.GetDimension();
                        if (dim != null) {
                            t = dim.P_Name;
                        }
                    }
                }
                AddCashValue(t, MethodBase.GetCurrentMethod().ToString(), null);
                return t;
            }
        }
        public override bool P_Dimension_Exists()
        {
            return true;
        }
        #endregion
        #region PrDimension
        public override string P_PrDimension {
            get {
                if (!string.IsNullOrEmpty(this.AttrOrEmpty(AName_.dimension))) {
                    return TextConst.AVBool.True;
                } else {
                    return string.Empty;
                }
            }
            set {
                if (value != string.Empty && this.AttrOrEmpty(AName_.dimension) != value) {
                    this.SetAttributeNotEmpty(AName_.is_private_dimension, value);
                    this.SetAttributeNotEmpty(AName_.is_final_dimension, value);
                }
                this.SetAttributeNotEmpty(AName_.dimension, value);
            }
        }
        public override bool P_PrDimension_Exists()
        {
            return true;
        }
        #endregion
        #region UpdateTarget
        public override bool P_UpdateTarget_Exists()
        {
            if (this.GetMainParent() is VReport) {
                return true;
            } else {
                return base.P_UpdateTarget_Exists();
            }
        }
        #endregion
        #region Updateable
        public override string P_Updateable {
            get {
                return this.AttrOrEmpty(AName_.updateable);
            }
            set {
                this.SetAttributeNotEmpty(AName_.updateable, value);
            }
        }
        public override bool P_Updateable_Exists()
        {
            return true;
        }
        #endregion
        #region Order
        public override bool P_Order_Exists()
        {
            return this.GetMainParent() is VForm;
        }
        #endregion
    }
}