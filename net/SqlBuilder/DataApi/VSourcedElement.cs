using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal abstract class VSourcedElement : VQueryCall
    {
        protected VSourcedElement(XName name)
            : base(name)
        {
        }
        protected VSourcedElement(XElement element)
            : base(element)
        {
        }
        private List<VSXElement> GetAllSectionsContainingColumns() // Дополнить where from итд
        {
            List<VSXElement> list = this.GetMEISelectSections();
            list.AddRange(this.GetWhereSections());
            list.AddRange(this.GetHavingSections());
            list.AddRange(this.GetMEIFromSections());
            if (this is VForm) {
                list.AddRange(this.GetContentSections());
            }
            return list;
        }
        internal List<VSXElement> GetAllSectionsContainingColumnsSW()
        {
            List<VSXElement> list = this.GetMEISelectSections();
            list.AddRange(GetWhereSections());
            return list;
        }
        internal List<VSXElement> GetMEISelectSections()
        {
            var list = this.GetExtensionsAndParentAndMain().SelectMany(VSourcedElement.GetSelfSelectSections).ToList();
            return list;
        }
        internal List<VSXElement> GetMESelectSections()
        {
            var list = this.GetExtensionsAndMain().SelectMany(VSourcedElement.GetSelfSelectSections).ToList();
            return list;
        }
        internal List<VSXElement> GetContentSections()
        {
            var list = this.Elements(EName.content).Select(VSXElement.Get).ToList();
            list.AddRange(this.Elements(EName.toolbar).Select(VSXElement.Get).ToList());
            if (list.Count == 0 && this is VForm) {
                return this.AsList();
            }
            return list;
        }
        private IList<VSXElement> GetWhereSections()
        {
            var list = this.Elements(EName.where).Select(VSXElement.Get).ToList();
            return list;
        }
        private IList<VSXElement> GetHavingSections()
        {
            var list = this.Elements(EName.having).Select(VSXElement.Get).ToList();
            return list;
        }
        internal virtual List<VSXElement> GetMEIFromSections()
        {
            var list = this.GetExtensionsAndParentAndMain().SelectMany(e => e.GetSelfFromSections()).ToList();
            return list;
        }
        internal virtual List<VSXElement> GetMEFromSections()
        {
            var list = this.GetExtensionsAndMain().SelectMany(e => e.GetSelfFromSections()).ToList();
            return list;
        }
        // overrided in VReport
        public virtual List<VQueryCall> AllSources()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VQueryCall>);
            }
            List<VQueryCall> srcs = new List<VQueryCall>();
            VSourcedElement heir = null;
            if (this.IsInherit()) {
                heir = this;
            }
            foreach (XElement el in this.GetMEIFromSections().SelectMany(VSXElement.GetElementsP).ToArray()) {
                if (el is VQueryCall) {
                    VQueryCall qel = VSXElement.Get<VQueryCall>(el);
                    srcs.Add(qel);
                    foreach (VQueryCall el1 in qel.AllLinks(heir)) {
                        if (!(el1 is VDimLink) && !(el1 is VQube)) {
                            srcs.Add(el1);
                        }
                    }
                }
                VQube qube = el as VQube;
                if (qube != null) {
                    foreach (VQueryCall el1 in qube.FactLinks()) {
                        srcs.Add(el1);
                    }
                }
            }
            foreach (VXElement el in GetNamedSections(TextConst.EName.Params).SelectMany(VSXElement.GetElementsP).ToArray()) {
                VParam par = el as VParam;
                if (par != null) {
                    if (par.IsObject()) {
                        VQueryCall qel = VSXElement.Get<VQueryCall>(par);
                        srcs.Add(qel);
                        srcs.AddRange(qel.AllLinks(heir));
                    }
                }
            }
            foreach (VXElement el in this.GetNamedSections(TextConst.EName.Queries).SelectMany(VSXElement.GetElementsP).ToArray()) {
                VQueryCall qel = VSXElement.Get<VQueryCall>(el);
                srcs.Add(qel);
            }
            AddCashValue(srcs, MethodBase.GetCurrentMethod().ToString(), null);
            return srcs;
        }
        internal virtual VQueryCall MainSource()
        {
            IList<VQueryCall> list = this.AllSources();
            for (int index = 0; index < list.Count; index++) {
                VQueryCall el = list[index];
                if (string.IsNullOrEmpty(el.P_Join)) {
                    return el;
                }
            }
            return null;
        }
        internal IList<VQueryCall> GetSourceByAlias(string alias)
        {
            return this.AllSources().Where(e => e.XName == alias).ToList();
        }
        internal VSourcedElement GetMainIE()
        {
            if (this.IsExtension()) {
                VQuery qry = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries)
                 .Elements(EName.query).Where(e => e.Attribute(AName_.extend) == null && e.Attribute(AName_.name).Value == this.NameOrExtend())
                 .Select(VSXElement.Get<VQuery>).First();
                return qry;
            } else if (this.IsInherit()) {
                VQuery qry = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries)
                                 .Elements(EName.query).Where(e => e.Attribute(AName_.extend) == null && e.Attribute(AName_.name).Value == this.AttrOrEmpty(AName_.inherit))
                                 .Select(VSXElement.Get<VQuery>).First();
                return qry;
            } else {
                return this;
            }
        }
        internal VSourcedElement GetMainE()
        {
            if (this.IsExtension()) {
                VQuery qry = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries)
                 .Elements(EName.query).Where(e => e.Attribute(AName_.extend) == null && e.Attribute(AName_.name).Value == this.NameOrExtend())
                 .Select(VSXElement.Get<VQuery>).First();
                return qry;
            } else {
                return this;
            }
        }
        internal static string GetSysColDefaultValue(string name)
        {
            switch (name) {
                case TextConst.AVColumn.IsNew:
                    return "0";
                case TextConst.AVColumn.IsNotNew:
                    return "1";
                default:
                    return null;
            }
        }
        internal static XElement CreateVirtualSysColumnElement(string table ,string name)
        {
            XElement col = Factory.NewColumn(table, name);
            col.Add(new XAttribute(AName_.type, TextConst.AVDataType.Number));
            col.Add(new XAttribute(AName_.sys, TextConst.AVBool.True));
            if (!XmlReports.IsDeveloperMode()) {
                col.Add(new XAttribute(AName_.invisible_in_column_chooser, TextConst.AVBool.True));
            }
            return col;
        }
        private List<VSXElement> virtualSysColumns = null;
        internal List<VSXElement> VirtualSysColumns() 
        {
            if (virtualSysColumns == null) {
                virtualSysColumns = new List<VSXElement>();
                var mnsrc = this.MainSource();
                if (mnsrc is VTable) {
                    if (mnsrc.AttrOrEmpty(AName_.view) != TextConst.AVBool.True) {
                        foreach (string colName in TextConst.AVColumnArray.SysColNamesForEditedObject) {
                            VSXElement col = VSXElement.Get(CreateVirtualSysColumnElement(mnsrc.XName, colName));
                            col.VirtualParent = this;
                            virtualSysColumns.Add(col);
                        }
                    }
                }
            }
            return virtualSysColumns;
        }
        internal virtual List<VSXElement> SelfColumns()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            List<VSXElement> list = new List<VSXElement>();
            foreach (VSXElement el in this.GetMESelectSections()) {
                list.AddRange(el.GetElementsP());
            }
            // list.AddRange(VirtualSysColumns());
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        internal virtual List<VExpression> Expressions()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VExpression>);
            }
            List<VExpression> list = new List<VExpression>();
            var els = this.GetNamedSections(TextConst.EName.Expressions).SelectMany(VSXElement.GetElementsP).ToList();
            foreach (var el1 in els) {
                VCall el = el1 as VCall;
                if (el != null) {
                    list.Add((VExpression)el);
                }
            }
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        internal List<VCustomerUse> Customers()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VCustomerUse>);
            }
            var list = new List<VCustomerUse>();
            foreach (VCustomerUse el in this.GetNamedSections(TextConst.EName.Customers).SelectMany(VSXElement.GetElementsP)) {
                list.Add(el);
            }
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        internal VExpression SearchExpression(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VExpression);
            }
            VSXElement col = this.Expressions().FirstOrDefault(e => e.XName == name);
            if (col == null) {
                col = this.Columns().SelectMany(VColumn.SelfOrMultipleSource).FirstOrDefault(e => e.XName == name);
            }
            AddCashValue(col as VExpression, MethodBase.GetCurrentMethod().ToString(), name);
            return col as VExpression;
        }
        internal List<VSXElement> ColumnsWithDublers()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            List<VSXElement> list = new List<VSXElement>();
            foreach (VSXElement el in this.GetMEISelectSections()) {
                list.AddRange(el.GetElementsP());
            }
            list.AddRange(VirtualSysColumns());
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        private static void collectBandColumns(VBand band, List<VSXElement> list, List<string> names)
        {
            var cols = band.GetElementsP();
            foreach (VSXElement col in cols) {
                if (!(col is VBand)) {
                    if (!names.Contains(col.XName)) {
                        list.Add(col);
                    }
                } else {
                    collectBandColumns((VBand)col, list, names);
                }
            }
        }
        private bool IsStoredInProj()
        {
            return !this.GetElementsP().Any(EPredicate.IsNotConst);
        }
        // overrided in VForm
        public virtual List<VSXElement> Columns() 
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            List<VSXElement> list = new List<VSXElement>();
            var sList = this.GetMEISelectSections().ToList();
            var inhSect = sList.FirstOrDefault(e => e.RootQuery().IsInherit());
            var names = new List<string>();
            if (inhSect != null) {
                foreach (VSXElement el in inhSect.GetElementsP()) {
                    names.Add(el.XName);
                    list.Add(el);
                }
                sList.Remove(inhSect);
            }
            foreach (VSXElement sel in sList) {
                foreach (VSXElement el in sel.GetElementsP()) {
                    VBand band = el as VBand;
                    if (band != null) {
                        collectBandColumns(band, list, names);
                    } else {
                        if (!names.Contains(el.XName)) {
                            list.Add(el);
                        }
                    }
                }
            }
            if (list.Count == 0) {
                if (IsStoredInProj()) {
                    var ccols = this.GetElementsP().Where(e => e.P_Alias != "");
                    list.AddRange(ccols);
                }
            }
            //foreach (VSXElement el in this.GetMEISelectSections().SelectMany(e => e.GetElementsApplyingParts()))
            //{
            //    list.Add(el);
            //}
            list.AddRange(this.VirtualSysColumns());
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        internal virtual IList<VViewColumn> ViewColumns()
        {
            var list = new List<VViewColumn>();
            IList<VColumns> colSections = this.GetNamedSections(TextConst.EName.Columns).Cast<VColumns>().ToList();
            foreach (VViewColumn el in colSections.SelectMany(cols => cols.GetDescedantsP(EName.column))) {
                list.Add((VViewColumn)el);
            }
            return list;
        }
        internal VSXElement SearchColumn(string columnName, VColumn exclude = null)
        {
            List<VSXElement> cols;
            if (exclude != null) {
                 cols = this.ColumnsWithDublers();
            } else {
                cols = this.Columns();
            }
            VSXElement col = cols.FirstOrDefault(e => e != exclude && e.XName == columnName);
            if (col == null) {
                col = cols.SelectMany(VColumn.SelfOrMultipleSource).FirstOrDefault(e => e != exclude && e.XName == columnName);
            }
            if (col == null && VSourcedElement.GetSelfSelectSections(this).Count == 0) {
                col = this.GetElementsP().FirstOrDefault(e => e.XName == columnName);
            }
            return col;
        }
        internal VSXElement SearchColumnByFactName(string name)
        {
            IList<VSXElement> cols = this.Columns();
            int index;
            VSXElement col;
            for (index = 0; index < cols.Count; index++) {
                col = cols[index];
                if (col.P_Fact == name) {
                    return col;
                }
            }
            for (index = 0; index < cols.Count; index++) {
                IList<VSXElement> list = VColumn.SelfOrMultipleSource(cols[index]);
                for (int index_2 = 0; index_2 < cols.Count; index_2++) {
                    col = list[index_2];
                    if (col.P_Fact == name) {
                        return col;
                    }
                }
            }
            return null;
        }
        internal List<VColumn> AllUsedColumnsByTableNameAndColumnName(string tableName, string columnName)
        {
            return this.AllUsedColumns().Where(e => e.P_Column == columnName && e.P_Table == tableName).ToList();
        }
        internal List<VColumn> AllUsedColumnsByTableNameAndColumnAlias(string tableName, string columnAlias)
        {
            return this.AllUsedColumns().Where(e => e.XName == columnAlias && e.P_Table == tableName).ToList();
        }
        internal List<VColumn> AllUsedColumns()
        {
            // вроде выбираются колонки только из под select -неправильно
            //if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            //{
            //    return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VColumn>);
            //}
            var list = new List<VColumn>();
            foreach (VSXElement el in this.GetAllSectionsContainingColumns().SelectMany(VSXElement.GetDescedantsP)) {
                if (el is VColumn) {
                    list.Add((VColumn)el);
                }
            }
            //  AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        internal List<VFact> AllUsedFacts()
        {
            // вроде выбираются колонки только из под select -неправильно
            List<VFact> list = new List<VFact>();
            foreach (VSXElement el in this.GetAllSectionsContainingColumns().SelectMany(VSXElement.GetDescedantsP)) {
                if (el is VFact) {
                    list.Add((VFact) el);
                }
            }
            return list;
        }
        internal bool IsExtension()
        {
            return this.Attribute(AName_.extend) != null;
        }
        internal bool IsInherit()
        {
            return this.Attribute(AName_.inherit) != null;
        }
        internal List<VSourcedElement> GetExtensionsAndParentAndMain()
        {
            var list = new List<VSourcedElement>();
            if (this is VForm) {
                list.Add(this);
            } else {
                list = GetExtensionsAndParent();
                var main = GetMainIE();
                if (main != this) {
                    list.InsertRange(0, main.GetExtensionsAndParent());
                }
                list.Insert(0, main);
                //list.Add(GetMainIE());
            }
            return list;
        }
        private IList<VSourcedElement> GetExtensionsAndMain()
        {
            List<VSourcedElement> list = new List<VSourcedElement>();
            list.Add(GetMainE());
            var list1 = GetExtensions();
            list.AddRange(list1);
            return list;
        }
        private List<VSourcedElement> GetExtensionsAndParent()
        {
            if (this.IsInherit()) {
                return new List<VSourcedElement>(1) { this };
            } else {
                //объединить наследник может иметь extensions
                return this.GetExtensions();
            }
        }
        private List<VSourcedElement> GetExtensions()
        {
            string name = this.NameOrExtend();
            if (string.IsNullOrEmpty(name)) {
                return new List<VSourcedElement>(0);
            } else {
                return XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries).Elements(EName.query).Where(e => e.AttrOrEmpty(AName_.extend) == name).ToList().SelectAsArray(VSXElement.Get<VSourcedElement>).ToList();
            }
        }
        internal static IList<VSXElement> GetSelfSelectSections(VSourcedElement se)
        {
            IList<XElement> list = se.Elements(EName.select).ToList();
            VSXElement[] arr = new VSXElement[list.Count];
            for (int index = 0; index < list.Count; index++) {
                arr[index] = VSXElement.Get(list[index]);
            }
            return arr;
        }
        /*internal IList<VSXElement> GetSelfSelectSections()
        {
            var list = new List<VSXElement>();
            list.AddRange(Elements(TextConst.EName.Select).ToList().Select(VSXElement.Get).ToList());
            return list;
        }*/
        internal List<VSXElement> GetNamedSections(string sectionName)
        {
            var list = GetExtensionsAndParentAndMain().SelectMany(e => e.GetSelfNamedSections(sectionName)).Distinct().ToList();
            return list;
        }
        /*internal List<VSXElement> GetNamedSectionsME(string sectionName)
        {
            var list = GetExtensionsAndMain().SelectMany(e => e.GetSelfNamedSections(sectionName)).Distinct().ToList();
            return list;
        }*/
        protected VForm Form()
        {
            return XmlReports.Environment.GetForm(this.P_Form);
        }
        internal List<VSXElement> ParamFields()
        {
            List<VSXElement> list = null;
            list = this.GetContentSections().SelectMany(e => VSXElement.GetDescedantsP(e).Where(e1 => e1 is VField || e1 is VUseField)).ToList();
            return list;
        }
        internal IList<VSXElement> FormalParams()
        {
            var list = GetNamedSections(TextConst.EName.Params).SelectMany(e => e.GetElementsP(EName.param)).ToList();

            if (list.Count == 0)
            {
                VSourcedElement frm = Form();
                if (frm == null)
                {
                    if (this is VQuery)
                    {
                        frm = this;
                    }
                    else
                    {
                        return new List<VSXElement>();
                    }
                }
                return frm.ParamFields();
                //if (GetContentSections().Any())
                //{

                //}
                

                //if (frm != null)
                //{
                //    return frm.ParamFields();
                //}
            }
            
            return list;
        }
        internal List<VSXElement> GetSelfNamedSections(string sectionName)
        {
            var list = new List<VSXElement>();
            list.AddRange(this.GetElementsP(sectionName).Where(EPredicate.IsNotExcuded).Select(VSXElement.Get).ToList());
            return list;
        }
        internal List<VSXElement> GetSelfFromSections()
        {
            var list = this.Elements(EName.from).ToList().SelectAsArray(VSXElement.Get).ToList();
            XElement push = Element(EName.push);
            if (push != null) {
                list.Add(VSXElement.Get(push.Element(EName.from)));
            }
            return list;
        }
        internal string NameOrExtend()
        {
            XAttribute attr = this.Attribute(AName_.extend);
            if (attr == null) {
                return this.AttrOrDefault(AName_.name, string.Empty);
            } else {
                return attr.Value;
            }
        }
        internal IList<VAction> Actions()
        {
            return this.GetNamedSections(TextConst.EName.Actions).SelectMany(VSXElement.GetElementsP).Cast<VAction>().ToList();
        }
        internal VAction GetAction(string name)
        {
            IList<VAction> actions = this.Actions();
            for (int index = 0; index < actions.Count; index++) {
                VAction action = actions[index];
                if (action.P_IdName == name) {
                    return action;
                }
            }
            return null;
        }
        internal List<VParam> Params()
        {
            return this.GetElementsP(EName.@params).SelectMany(VSXElement.GetElementsP).Cast<VParam>().ToList();
        }
        #region UseRepository
        

        public override bool P_UseRepository_Exists()
        {

            return true;

        }
        #endregion
        #region IsReport
        public override bool P_IsReport_Exists()
        {
            return this.IsMainElement();
        }
        #endregion
    }
}