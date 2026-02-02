using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using sql.builder.Exceptions;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;column table="" column="" as="" /&gt;
    /// </summary>
    /// <seealso cref="VFact"/>
    internal partial class VColumn : VSXElement, IVParent
    {
        protected VColumn(XName name)
            : base(name)
        {
        }
        internal VColumn()
            : base(EName.column)
        {
        }
        internal string FirstTableName()
        {
            return this.P_Table.SubstringBefore('.');
        }
        internal VQueryCall Source()
        {
            VSourcedElement root = this.ExtendedOrRootQuery();
            if (root != null) {
                return root.AllSources().FirstOrDefault(e => e.XName == this.FirstTableName());
            } else {
                return null;
            }
        }
        public override VSXElement GetDummyOrSelf()
        {
            VSXElement col = this.getDummy();
            if (col == null) {
                return this;
            } else {
                return col;
            }
        }
        private VSXElement getDummy()
        {
            VSXElement vexpr = null;
            if (this.P_Column == TextConst.AVColumn.Dummy) {
                XElement xcall = this.Element(EName.call);
                XElement expr;
                if (xcall == null) {
                    expr = Factory.NewConst("null");
                } else {
                    expr = new XElement(xcall);
                }
                expr.SetAttributeValue(AName_.@as, this.XName);
                expr.SetAttributeValue(AName_.type, this.P_DataType);
                vexpr = VSXElement.Get(expr);
                //vexpr.environment = GetEnvironment();
                vexpr.VirtualParent = this.GetParent();
            }
            return vexpr;
        }
        private VSXElement getDummySourceColumn()
        {
            VSXElement col = this.getDummy();
            if (col != null) {
                IList<VQuery> list = this.SourceQuery();
                if (list.Count == 0) {
                    return null;
                }
                col.VirtualParent = VSourcedElement.GetSelfSelectSections(list[0]).First();
            }
            return col;
        }
        internal VColumn SearchSourceDbColumn()
        {
            var cols = this.SourceColumn();
            if (cols.Count != 0 && cols[0] is VColumn) {
                VColumn tcol = cols[0] as VColumn;
                if (tcol.Source() is VTable) {
                    return tcol;
                } else {
                    return tcol.SearchSourceDbColumn();
                }
            }
            return null;
        }
        public virtual List<VSXElement> SourceColumn()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            IList<VQuery> query = this.SourceQuery();
            List<VSXElement> col = null;
            var col1 = this.getDummySourceColumn();
            if (col1 != null) {
                col = new List<VSXElement>();
                col.Add(col1);
            } else {
				if (query == null) {
					return null;
				}
                col = new List<VSXElement>();
                if (query.Count > 0) {
                    VColumn exclude;
                    if (this.P_Table == TextConst.AVTable.Ths) {
                        exclude = this;
                    } else {
                        exclude = null;
                    }
                    foreach (VQuery q in query) {
                        VSXElement c = q.SearchColumn(this.P_Column, exclude);
                        if (c != null) {
                            col.Add(c);
                        }
                    }
                }
            }
            AddCashValue(col, MethodBase.GetCurrentMethod().ToString(), null);
            return col;
        }
        internal override IList<VSXElement> SourceColumns()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            var list = new List<VSXElement>();
            list.AddRange(this.SourceColumnNoCycle());
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        private IList<VSXElement> SourceColumnNoCycle()
        {
            List<VSXElement> col = this.SourceColumn();
            if (col == null) {
                return Array.Empty<VSXElement>();
            }
            List<VSXElement> list = new List<VSXElement>(col.Count);
            for (int index = 0; index < col.Count; index++) {
                VSXElement c = col[index];
                if (c != this) {
                    list.Add(c);
                }
            }
            return list;
        }
        public override void LookUpNextSources(List<VSXElement> list, VLookupAnalyzer analyzer)
        {
            if (!analyzer.CheckAndReturn(list, this)) {
                return;
            }
            IList<VSXElement> cols = this.SourceColumns();
            for (int index = 0; index < cols.Count; index++) {
                cols[index].LookUpNextSources(list, analyzer);
            }
        }
        public override List<VColumn> UsedColumns()
        {
            var list = new List<VColumn>(1);
            list.Add(this);
            return list;
        }
        /*internal override IList<VSXElement> SelfOrMultipleSource()
        {
            if (this.P_Column != TextConst.AVColumn.All) {
                return new VSXElement[1] { this };
            } else {
                return this.SourceQuery().SelectMany(q => q.Columns()).SelectMany(e => e.SelfOrMultipleSource()).ToList();
            }
        }*/
        internal static IList<VSXElement> SelfOrMultipleSource(VSXElement el)
        {
            VColumn col = el as VColumn;
            if (col == null || el.P_Column != TextConst.AVColumn.All) {
                return new VSXElement[1] { el };
            }
            return col.SourceQuery().SelectMany(q => q.Columns()).SelectMany(VColumn.SelfOrMultipleSource).ToList();
        }
        internal string TreeSourceName()
        {
            return this.P_Table.SubstringBefore('-');
        }
        internal string TreeSpecSourceName()
        {
            string[] ss = P_Table.Split('-');
            if (ss.Length < 2) {
                return null;
            } else {
                return ss[1];
            }
        }
        public virtual List<VQuery> SourceQuery()
        {
            if (this.P_Table == TextConst.AVTable.Ths || this.P_Table == "*" || this.TreeSpecSourceName() != null) {
                VSourcedElement root = this.ExtendedOrRootQuery();
                if (root == null) {
                    return new List<VQuery>();
                }
                VQuery q = root.GetMainE() as VQuery;
				if (q == null) {
					 return new List<VQuery>();
				}
                return new List<VQuery>(1) { q };
            }
            if (this.IsUnderUsing()) {
                VSourcedElement root = this.ExtendedOrRootQuery();
                if (root == null) {
                    return new List<VQuery>();
                }
                return new List<VQuery>(1) { (VQuery)root.GetMainE() };
            }
            VQueryCall source = Source();
            if (source == null) {
                return new List<VQuery>();
            } else {
                VQuery qry = source.Query();
                if (qry != null) {
                    return new List<VQuery>(1) { qry };
                } else {
                    return new List<VQuery>();
                }
            }
        }
        internal VRelation TypeRelation()
        {
            string s = string.Empty;
            return TypeRelation(ref s);
        }
        internal VRelation TypeRelation(ref string xtraPath)
        {
            VQuery query = this.SourceQuery().FirstOrDefault();
            if (query != null) {
                VEntityType et = query.EntityType;
                if (et != null) {
                    VRelation parentLink = et.ParentLink(this.XName);
                    if (parentLink == null) {
                        var sc = this.SourceColumn();
                        if (sc != null && sc.Count != 0) {
                            var scc = sc[0] as VColumn;
                            if (scc != null) {
                                VQueryCall source = scc.Source();
                                if (source != null) {
                                    if (source.P_Updateable == TextConst.AVBool.True) {
                                        xtraPath = "." + source.XName;
                                        return scc.TypeRelation();
                                    }
                                }
                               
                            }
                        }
                    }
                    return parentLink;
                }
            }
            return null;
        }
        internal VQuery TypeQuery()
        {
            VRelation parentLink = this.TypeRelation();
            if (parentLink != null) {
                return parentLink.ParentQuery();
            } else {
                return null;
            }
        }
        internal XElement AsNameColumnOrSelf()
        {
            XElement col2;
            VRelation rel = this.TypeRelation();
            if (rel != null) {
                VQuery qry = rel.Query();
                if (qry == null) {
                    throw new VCompilerException("Запрос " + rel.P_CalledQuery + " не найден", this.GetMainParent(), rel);
                }
                VSXElement relNmcol = qry.NameColumn();
                col2 = Factory.NewColumn(rel.XName, relNmcol.XName);
                col2.Add(new XAttribute(AName_.title, this.P_Title));
            } else {
                col2 = new XElement(this);
            }
            return col2;
        }
        internal XElement TypeQueryAsListQuery()
        {
            VQuery query;
            VQueryCall qq = this.ListQueryCallElement();
            if (qq != null) {
                query = qq.Query();
            } else {
                query = this.TypeQuery();
            }
            if (query != null) {
                XElement lquery = null;
                if (qq == null) {
                    lquery = query.AsListQuery();
                } else {
                    if (qq.GetElementsP(EName.withparams).Count != 0) {
                        //var ucs = ListQueryCallUsedColumns();
                        //if (ucs.Any())
                        //{
                        lquery = new XElement(query);
                        //применить еще parname
                        int i = 0;
                        foreach (VColumn col in this.ListQueryCallUsedColumns()) {
                            XElement par = lquery.Element(EName.@params).Elements().ElementAt(i);
                            par.SetAttributeValue(AName_.column, col.P_Column);
                            i++;
                        }
                        //}
                    } else {
                        lquery = query.AsListQuery();
                    }
                }
                return lquery;
            }
            return null;
        }
        internal VQueryCall ListQueryCallElement()
        {
            IList<VSXElement> list = this.GetElementsP(EName.listquery);
            if (list.Count == 0) {
                return null;
            }
            list = list[0].GetElementsP();
            if (list.Count == 0) {
                return null;
            }
            return (VQueryCall)list[0];
        }
        internal IList<VColumn> ListQueryCallUsedColumns()
        {
            VQueryCall lqc = this.ListQueryCallElement();
            if (lqc != null) {
                IList<VSXElement> cols = lqc.GetDescedantsP(EName.column);
                VColumn[] arr = new VColumn[cols.Count];
                for (int index = 0; index < cols.Count; index++) {
                    arr[index] = (VColumn)cols[index];
                }
                return arr;
            }
            return null;
        }
        internal VDataSet SelectionListDataSet()
        {
            XElement q = this.TypeQueryAsListQuery();
            if (q != null) {
                VDataSet ds = XmlReports.Environment.GetPrecompiledReport(q).Result(2, false); //мб. изменить на true
                //  ds.Refresh(false);
                return ds;
            }
            return null;
        }
        internal bool IsRefreshedByAction()
        {
            VForm form = this.RootQuery() as VForm;
            IList<VAction> actions = form.GetRefreshColumnActions();
            for (int index = 0; index < actions.Count; index++) {  // нужно еще таблицу проверить, пока оставляю так
                if (actions[index].P_Column == this.XName) {
                    return true;
                }
            }
            return false;
        }
        private string TypeTitle()
        {
            string s = string.Empty;
            VQuery query = this.SourceQuery().FirstOrDefault();
            if (query != null) {
                VEntityType et = query.EntityType;
                if (et != null) {
                    VRelation parentLink = et.ParentLink(this.P_Column);
                    if (parentLink != null) {
                        s = parentLink.Title();
                        if (string.IsNullOrEmpty(s)) {
                            VQuery parentQuery = parentLink.ParentQuery();
                            if (parentQuery != null) {
                                s = parentQuery.Title();
                            }
                        }
                    }
                }
            }
            return s;
        }
        internal string EType {
            get {
                VQuery rc = this.RootQuery() as VQuery;
                if (rc == null) {
                    return null;
                }
                VEntityType et = rc.EntityType;
                if (et != null) {
                    VRelation parentLink = et.ParentLink(P_Column);
                    if (parentLink != null) {
                        VQuery pq = parentLink.ParentQuery();
                        if (pq != null) {
                            return pq.Name;
                        } else {
                            return "[missing]" + parentLink.PName();
                        }

                    }
                }
                return null;
            }
        }
        public override string XName {
            get {
                string column = this.AttrOrEmpty(AName_.column);
                string alias = this.AttrOrDefault(AName_.@as, column);
                if (alias.Length > 0 && alias[0] == '+') {
                    alias = column + alias.Substring(1);
                }
                return alias + this.AliasPfx();
            }
        }
        public override string XDataType()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as string);
            }
            string s = this.DataType();
            if (string.IsNullOrEmpty(s)) {
                VLink sqry = (this.Source() as VLink);
                if (sqry != null && sqry.IsDimenson()) {
                    VDimension dim = sqry.LinkedDimension();
                    if (!string.IsNullOrEmpty(dim.P_TimeType)) {
                        s = dim.GetTimeAttrType(this.P_Column);
                    }
                }
                if (string.IsNullOrEmpty(s)) {
                    IList<VSXElement> cols = this.SourceColumns();
                    if (cols.Count != 0) {
                        s = cols[0].XDataType();
                    }
                }
            }
            AddCashValue(s, MethodBase.GetCurrentMethod().ToString(), null);
            return s;
        }
        public override string XFormat()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as string);
            }
            string s = this.Format();
            if (string.IsNullOrEmpty(s)) {
                IList<VSXElement> cols = this.SourceColumns();
                if (cols.Count != 0) {
                    s = cols[0].XFormat();
                }
            }
            AddCashValue(s, MethodBase.GetCurrentMethod().ToString(), null);
            return s;
        }
        public override string XHAlign()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as string);
            }
            string s = this.P_HAlign;
            if (string.IsNullOrEmpty(s)) {
                IList<VSXElement> cols = this.SourceColumns();
                if (cols.Count != 0) {
                    s = cols[0].XHAlign();
                }
            }
            AddCashValue(s, MethodBase.GetCurrentMethod().ToString(), null);
            return s;
        }
        private bool IsUnderUsing()
        {
            VSXElement parent = this.GetParent();
            return parent != null && parent.Name == EName.@using;
        }
        public override bool IsElementUser()
        {
            return true;
        }
        internal bool IsAddision;
        internal bool IsKey;
        internal bool IsAddisionForName;
        internal bool IsRelation;
        internal string TextSourceFor; // используется в момент компиляции формы
        //public bool HasButtons = false;// используется в момент компиляции формы
        public override List<VSXElement> GetUsedElements()
        {
            IList<VSXElement> cols = this.SourceColumn();
            List<VSXElement> list = new List<VSXElement>(cols.Count);
            for (int index = 0; index < cols.Count; index++) {
                VSXElement col = cols[index];
                VSXElement base_el = col.BaseElement;
                if (base_el != null) {
                    if ((base_el.GetParent() as VPart).Content().Count == 1) {
                        list.Add(col.UsePartElement);
                    } else {
                        list.Add(col);
                    }
                } else {
                    list.Add(col);
                }
            }
            return list;
        }
        internal VQueryCall MasterSource()
        {
            VSXElement src = this.Source();
            if (src == null) {
                throw new VCompilerException("Источник " + this.P_Table + " не найден", this.GetMainParent(), this);
            }
            while (src is VDLink || src is VLink) {
                src = src.GetParent();
            }
            if (src.Name == EName.query && src.GetParent().Name != EName.from) {
                src = src.GetParent();
            }
            return (VQueryCall)src;
        }
        internal VSXElement DefaultExpression()
        {
            if (string.IsNullOrEmpty(this.P_Default)) {
                return null;
            } else {
                return this.RootQuery().Columns().First(c => c.XName == this.P_Default);
            }
        }
        internal XElement CreateNameColumn()
        {
            VRelation rel = this.TypeRelation();
            if (rel == null) {
                return null;
            }
            VSXElement vidCol = rel.ParentQuery().NameColumn();
            XElement col1 = Factory.NewColumn(this.Source().XName + "." + rel.PName(), vidCol.XName);
            col1.Add(new XAttribute(AName_.@as, this.XName + TextConst.Pfx.ExtValName));
            col1.Add(new XAttribute(AName_.title, this.P_Title));
            return col1;
        }
        private static string[] child_nodes = { TextConst.EName.If, TextConst.EName.Section, TextConst.EName.ListQuery, TextConst.EName.Buttons, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region Свойства
        #region Column
        public override void P_Column_ListRefresh(VDataTable dt)
        {
            //P_Column_List(dt);
            dt.Rows.Clear();
            VSourcedElement rootQuery = this.ExtendedOrRootQuery();
            VDimension timeDim = null;
            List<VSXElement> cols = null;
            if (this.GetMainParent() is VExpressionPackage || this.GetSubMainParent() is VExpressions) {
                string table = this.P_Table;
                VDimension dim = XmlReports.Environment.GetDimension(table);
                if (!string.IsNullOrEmpty(dim.P_TimeType)) {
                    timeDim = dim;
                } else {
                    cols = XmlReports.Environment.GetQueryByKeyDimensionName(table).Columns();
                }
            } else {
                if (this.P_Table == "*") {
                    cols = rootQuery.AllSources().SelectMany(s => s.Query().Columns()).ToList();

                } else {
                    if (this.IsUnderUsing()) {
                        if ((rootQuery as VQuery).IsQube()) {
                            cols = rootQuery.Columns().Where(e => e.AttrOrEmpty(AName_.group) == TextConst.AVGroup.Group).ToList();
                        } else {
                            cols = rootQuery.AllSources().First().Query().Columns();
                        }
                    } else {
                        VQueryCall src = this.Source();
                        if (src != null) {
                            if (src.GetParent() is VQube || src.GetParent() is VDimSet) {
                                VDimension dim = XmlReports.Environment.GetDimension(src.P_CalledQuery);
                                if (!string.IsNullOrEmpty(dim.P_TimeType)) {
                                    timeDim = dim;
                                }
                            }
                        }
                        if (timeDim == null) {
                            IList<VQuery> sq = SourceQuery();
                            if (sq != null) {
                                cols = sq.SelectMany(q => q.Columns()).ToList();
                            } else {
                                cols = new List<VSXElement>();
                            }
                        }
                    }
                }
            }
            if (timeDim == null) {
                cols = cols.SelectMany(VColumn.SelfOrMultipleSource).ToList();
                List<string> names = new List<string>();
                foreach (VSXElement el in cols) {
                    string name = el.XName;
                    if (!names.Contains(name)) {
                        AddColumnInfoToList(dt, name, el);
                        names.Add(name);
                    }
                }
                dt.Rows.Add("*", "*", string.Empty, string.Empty);
            } else {
                VSXElement.FillDataTableFromStringArray(dt, TextConst.AVTimeAttrArray.All);
            }
        }
        public override bool P_Column_Exists()
        {
            return true;
        }
        public override string P_Column_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region Title
        public override string P_Title {
            get {
                string s = base.P_Title;
                if (s == string.Empty) {
                    VSourcedElement root = this.RootQuery();
                    if ((root != null) && (root is VForm || root.P_AddNames == TextConst.AVBool.True)) {
                        s = this.TypeTitle();
                    }
                }
                if (s == string.Empty && this.P_CalledQuery != string.Empty) {
                    VQuery t = XmlReports.Environment.GetQuery(this.P_CalledQuery);
					if (t != null) {
						s = t.P_Title;
					}
				}
                return s;
            }
        }
        public override bool P_Title_Exists()
        {
            return true;
        }
        #endregion
        public override bool P_DataTypeS_Exists()
        {
            return true;
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region Table
        public override bool P_Table_Exists()
        {
            return !this.IsUnderUsing();
        }
        public override void P_Table_ListRefresh(VDataTable table)
        {
            VSXElement parent = this.GetParent();
            table.Rows.Clear();
            VGrid grid = parent as VGrid;
            if (grid != null) {
                foreach (VQueryCall el in grid.Source().SelfAndAllMasterLinks()) {
                    TableListRowFromElement(table, el);
                }
            } else if (this.GetMainParent() is VExpressionPackage || this.GetSubMainParent() is VExpressions) {
                foreach (VDimension dim in XmlReports.Environment.GetDimensions()) {
                    VQuery qry = dim.Query();
                    if (qry != null) {
                        table.Rows.Add(dim.P_Name, dim.P_Name, qry.XName, qry.P_Title);
                    } else {
                        table.Rows.Add(dim.P_Name, dim.P_Name, dim.P_Timeline);
                    }
                }
            } else {
                base.P_Table_ListRefresh(table);
            }
        }
        public override string P_Table_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeText(false);
        }
        internal string GetNodeText(bool is_arg)
        {
            string s;
            if (this.P_Prior == TextConst.AVBool.True) {
                s = ColorGreen("prior ");
            } else {
                s = string.Empty;
            }
            if (!this.IsUnderUsing()) {
                s += this.AttrOrEmpty(AName_.table) + ".";
            }
            string sAs = this.P_Alias;
            string column = this.P_Column;
            if (sAs == string.Empty && this.SelfParentOrUsepartParent() is VOutputElement) {
                s += ColorBrown(Bold(column));
            } else {
                s += Bold(column);
            }
            if (this is VFact) {
                s += Bold("()");
                if (this.P_Condition != "") {
                    //RootQuery().SearchExpression(P_Condition);
                    VSXElement condExpr = (this as VFact).GetConditionSource();
                    string wi = "";
                    if (condExpr == null) {
                        wi = ColorRed("[missing]");
                    } else if (condExpr.P_DontPushpred == TextConst.AVBool.True) {
                        wi = " if ";
                    } else {
                        wi = " where ";
                    }
                    s += ColorBlue(wi) + Bold(this.P_Condition);
                }
            }
            string mp = this.P_Multiplicer;
            if (mp != string.Empty) {
                s += "*10" + Sup(mp);
            }
            if (sAs != string.Empty) {
                if (this.SelfParentOrUsepartParent() is VOutputElement) {
                    s += " as " + ColorBrown(Bold(sAs));
                } else {
                    s += " as " + ColorBrown(sAs);
                }
            }
            if (this.P_Fact != string.Empty) {
                s += " fact:" + Bold(this.P_Fact);
            }
            if (this.P_CalledQuery != string.Empty) {
                s += " link " + Bold(this.P_CalledQuery);
            }
            if (this.P_Dimension != string.Empty) {
                s += " dim " + Bold(this.P_Dimension);
            }
            if (this.SelfParentOrUsepartParent() is VOutputElement || !is_arg) {
                string p = this.P_ParName;
                if (p != string.Empty) {
                    s += Bold(" :" + p);
                }
                string t = this.P_Title;
                if (t != this.P_SelfTitle) {
                    t = ColorGray(t);
                }
                s += " " + Italic(t);
            }
            string g = this.P_Group;
            if (g != string.Empty) {
                if (g == "1") {
                    g = "group";
                }
                s = " " + ColorGroup(Italic(g)) + " " + s;
            }
            if (this.P_Key != "") {
                s += " " + ColorGold("pk");
            }
            return s;
        }
        #endregion
        #region Prior
        public override bool P_Prior_Exists()
        {
            return this.GetMainParent().GetElementsP(EName.connect).Count != 0;
        }
        #endregion
        #region Alias
        public override bool P_Alias_Exists()
        {
            return true;
        }
        #endregion
        #region IsListColumn
        public override bool P_IsListColumn_Exists()
        {
            return true;
        }
        #endregion
        #region AutoFilter
        public override bool P_AutoFilter_Exists()
        {
            return this.GetParent() is VOutputElement;
        }
        #endregion
        #region IsNameColumn
        public override bool P_IsNameColumn_Exists()
        {
            return true;
        }
        #endregion
        #region Group
        public override bool P_Group_Exists()
        {
            return true;
        }
        #endregion
        #region Dgroup
        public override bool P_Dgroup_Exists()
        {
            return this.Source() is VDLink;
        }
        #endregion
        #region Size
        public override bool P_Size_Exists()
        {
            VSXElement parent = this.GetParent();
            return (parent is VFieldGroup) || (parent is VFormContent);
        }
        #endregion
        #region Position
        public override bool P_Position_Exists()
        {
            VSXElement parent = this.GetParent();
            return (parent is VFieldGroup) || (parent is VFormContent);
        }
        #endregion
        #region If
        public override bool P_If_Exists()
        {
            return true;
        }
        #endregion
        #region Dimname
        public override bool P_Dimname_Exists()
        {
            return true;
        }
        #endregion
        #endregion
        #region Default
        public override bool P_Default_Exists()
        {
            if (this.GetParent() is VSelect) {
                return true;
            } else if (this.RootQuery() is VForm) {
                return true;
            } else {
                return false;
            }
        }
        #endregion
        #region Editable
        public override bool P_Editable_Exists()
        {
            return this.P_Default_Exists();
        }
        #endregion
        #region NewVal
        public override bool P_NewVal_Exists()
        {
            return this.P_Default_Exists();
        }
        #endregion
        #region ColumnEditable
        public override bool P_ColumnEditable_Exists()
        {
            return this.P_ColumnDefault_Exists();
        }
        #endregion
        #region ColumnMandatory
        public override bool P_ColumnMandatory_Exists()
        {
            return this.P_Default_Exists();
        }
        #endregion
        #region ColumnVisible
        public override bool P_ColumnVisible_Exists()
        {
            if (this.GetParent() is VSelect) {
                return true;
            } else if (this.RootQuery() is VForm) {
                return true;
            } else {
                return false;
            }
        }
        #endregion
        #region ParName
        public override bool P_ParName_Exists()
        {
            return base.P_ParName_Exists() || ((this.RootQuery() is VForm) && this.GetAncestorsAndSelf(EName.content).Count != 0);
        }
        #endregion
        #region RowsLimit
        public override bool P_RowsLimit_Exists()
        {
            return this.P_Default_Exists();
        }
        #endregion
        #region ControlType
        public override bool P_ControlType_Exists()
        {
            return this.GetParent() is VOutputElement;
        }
        #endregion
		#region InvisibleInColumnChooser
		public override bool P_InvisibleInColumnChooser_Exists()
		{
			return true;
		}
		#endregion
        //#region StoreInDB
        //public override bool P_StoreInDB_Exists()
        //{
        //    return (P_ControlType == TextConst.AVControlType.List);
        //}
        //#endregion
        #region ValidS
        public virtual string P_ValidS {
            get {
                string valid = this.P_Valid;
                if (string.IsNullOrEmpty(valid)) {
                    IList<VSXElement> cols = this.SourceColumn();
                    if (cols.Count != 0) {
                        return cols[0].P_Valid;
                    }
                }
                return valid;
            }
        }
        #endregion
        #region NullIf
        public override bool P_NullIf_Exists()
        {
            return true;
        }
        #endregion
        #region FixedSide
        public override bool P_FixedSide_Exists()
        {
            return true;
        }
        #endregion
        #region IsHyperlink
        public override bool P_IsHyperlink_Exists()
        {
            return true;
        }
        #endregion
        //#region Window
        //public virtual bool P_Window_Exists()
        //{
        //    return true;
        //}
        //#endregion
        #region CalledQuery
        public override string P_CalledQuery {
            get {
                return this.AttrOrEmpty(AName_.link);
            }
            set {
                this.SetAttributeNotEmpty(AName_.link, value);
            }
        }
        public override void P_CalledQuery_List(VDataTable table)
        {
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
        }
        // overrided в VMultireference
        public override void P_CalledQuery_ListRefresh(VDataTable table)
        {
            VSXElement.FillDataTableFromRealQueries(table);
        }
        public override bool P_CalledQuery_Exists()
        {
            return this.GetParent() is VOutputElement;
        }
        public override string P_CalledQuery_Title()
        {
            return "Ссылается на";
        }
        #endregion
        /// <summary>
        /// Проверяет, принадлежит ли колонка таблице
        /// </summary>
        /// <param name="col">колонка</param>
        /// <returns>Возвращает true, если в @table указана ссылка на таблицу</returns>
        internal static bool IsTableColumn(VColumn col)
        {
            VSourcedElement rc = col.RootQuery();
            if (rc == null) {
                return false;
            } else {
                VQueryCall msrc = rc.MainSource();
                return (msrc is VTable) && (msrc.XName == col.P_Table);
            }
        }
        #region Name
        public override string P_Name {
            get {
                return this.AttrOrEmpty(AName_.column);
            }
            set {
                this.SetAttributeNotEmpty(AName_.column, value);
            }
        }
        public override bool P_Name_Exists()
        {
            return IsTableColumn(this);
        }
        #endregion
        #region DataSize
        public override string P_DataSize {
            get {
                return this.AttrOrEmpty(AName_.data_size);
            }
            set {
                this.SetAttributeNotEmpty(AName_.data_size, value);
            }
        }
        public bool P_DataSize_Editable()
        {
            return this.P_DataType == TextConst.AVDataType.String;
        }
        public override bool P_DataSize_Exists()
        {
            return IsTableColumn(this);
        }
        #endregion
        #region DataType
        public override bool P_DataType_Exists()
        {
            return true;
        }
        #endregion
        #region Multiplicer
        public override bool P_Multiplicer_Exists()
        {
            return true;
        }
        #endregion
        #region Index
        public override bool P_Index_Exists()
        {
            return (this.GetParent() is VOutputElement) && (this.GetMainParent() is VQuery);
        }
        #endregion
        #region intern
        public override bool P_Intern_Exists()
        {
            return this.GetParent() is VSelect;
        }
        public override bool P_Intern_Editable()
        {
            return this.XDataType() == TextConst.AVDataType.String;
        }
        #endregion
    }
}