using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;
using sql.builder.Clean;
//using Vertica.Data.VerticaClient;
using sql.builder.Core;
//using sql.builder.WebReports;

namespace sql.builder.DataApi
{
    public partial class VEnvironment
    {
        private ProjectManager manager;
        private VOracleConnection сonnection;
        private SortedList<string, VReport> reportsCash;
        public VEnvironment(VOracleConnection connection)
        {
            this.manager = new ProjectManager();
            this.сonnection = connection;
            this.reportsCash = new SortedList<string, VReport>();
        }
        public VOracleConnection Connection { get { return this.сonnection; } }
        public ProjectManager Manager { get { return this.manager; } }
        /*public VSXElement ForAdd(IEnumerable<VSXElement> scheme)
        {
            return scheme.First();
        }*/
        public DateTime GetLastSchemeAssembleTime()
        {
            string val = this.manager.GetNativeScheme().First().AttrOrDefault(AName.timestamp, null);
            return (val == null) ? DateTime.MinValue : DateTime.Parse(val);
        }
        public void UpdateLastSchemeAssembleTime()
        {
            this.manager.GetNativeScheme().First().SetAttributeValue(AName.timestamp, DateTime.Now);
        }
        public string GetGuid()
        {
            return GetHashCode().ToString();
        }
        public VReport GetPrecompiledReport(string name, string project = null)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VReport);
            }
            if (!string.IsNullOrEmpty(project)) {
                this.manager.LoadProjectIfNeed(project);
            }
            var rep = new VReport(this, name);
            AddCashValue(rep, MethodBase.GetCurrentMethod().ToString(), name);
            return rep;
        }
        public VReport GetPrecompiledReport(XElement element)
        {
            return new VReport(this, element);
        }
        public VQuery GetPrecompiledQuery(string name)
        {
            return VQuery.GetOrCreate(this, name);
        }
        /*public VUseReport GetUseReport(string report_name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), report_name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), report_name) as VUseReport);
            }
            XElement xusereport = XmlReports.GetUseReport(report_name);
            if (xusereport == null) {
                return null;
            }
            VUseReport userep = VSXElement.Get<VUseReport>(xusereport);
            AddCashValue(userep, MethodBase.GetCurrentMethod().ToString(), report_name);
            return userep;
        }*/
        /*public VUseForm GetUseForm(string form_name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), form_name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), form_name) as VUseForm);
            }
            var useform = VUseForm.Get(form_name);
            AddCashValue(useform, MethodBase.GetCurrentMethod().ToString(), form_name);
            return useform;
        }*/
        // Емцов - добавил параметр имя узла
        public string GetTypeParenElementtName(string elementType)
        {
            XElement parentElement = this.manager.GetNativeScheme().Elements().First(e1 => e1.AttrOrEmpty(TextConst.AName.ChildName) == elementType);
            return parentElement.Name.LocalName;
        }
        public string GetKeyName(string elementType)
        {
            var parentElements = this.manager.GetNativeScheme().Elements().Where(e1 => e1.AttrOrEmpty(TextConst.AName.ChildName) == elementType);
            string keyName = parentElements.Attributes(AName.key_name).First().Value;
            return keyName;
        }
        public T GetElement<T>(XName parent_name, XName key_name, string key)
            where T : VSXElement
        {
            XElement element = this.manager.GetNativeScheme().Elements(parent_name).Elements().SearchByAttribute(key_name, key);
            if (element != null) {
                return VSXElement.Get<T>(element);
            } else {
                return null;
            }
        }
        public VSXElement GetElement(XName parent_name, string id, string keyName, string file = null, string name = null)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), parent_name.LocalName + "|" + id)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), parent_name.LocalName + "|" + id) as VSXElement);
            }
            var parentElements = this.manager.GetNativeScheme().Elements(parent_name);
            if (keyName == null) {
                var parentKeyNameAttr = parentElements.Attributes(AName.key_name).FirstOrDefault();
                if (parentKeyNameAttr != null) {
                    keyName = parentKeyNameAttr.Value;
                } else {
                    keyName = TextConst.AName.Name;
                }
            }
            XElement element = null;
            if (parentElements.Any()) {
                element = parentElements.Elements()
                   .Where(e => (file == null || e.AttrOrEmpty(AName.file) == file)
                            && (name == null || e.Name.LocalName == name))
                   .FirstOrDefault(e1 => e1.AttrOrEmpty(keyName) == id);
            }
            VSXElement el;
            if (element != null) {
                el = VSXElement.Get(element);
            } else {
                el = null;
            }
            AddCashValue(el, MethodBase.GetCurrentMethod().ToString(), parent_name.LocalName + "|" + id);
            return el;
        }
        public List<VSXElement> GetElements(string parentName = null)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), parentName)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), parentName) as List<VSXElement>);
            }
            IEnumerable<XElement> list1;
            if (parentName != null) {
                list1 = this.manager.GetNativeScheme().Elements(parentName);
            } else {
                list1 = this.manager.GetNativeScheme().Elements();
            }
            var list = list1.Elements().ToList().SelectAsArray(VSXElement.Get).ToList();
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), parentName);
            return list;
        }
        public List<VSXElement> GetUsePartElements(string partName)
		{
			if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), partName)) {
				return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), partName) as List<VSXElement>);
			}
            IEnumerable<XElement> list1 = this.manager.GetNativeScheme().Descendants(TextConst.EName.UsePart).Where(e => e.Attribute(AName.part).Value == partName);
            List<VSXElement> list = list1.ToList().SelectAsArray(VSXElement.Get).ToList();
			AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), partName);
			return list;
		}
        public List<VSXElement> GetUseFieldElements(string fieldName)
		{
			if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), fieldName)) {
				return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), fieldName) as List<VSXElement>);
			}
            IEnumerable<XElement> list1 = this.manager.GetNativeScheme().Descendants(EName.usefield).Where(e => e.Attribute(AName.field).Value == fieldName);
            List<VSXElement> list = list1.ToList().SelectAsArray(VSXElement.Get).ToList();
			AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), fieldName);
			return list;
		}
        public List<VSourcedElement> GetSourcedElements()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSourcedElement>);
            }
            var list1 = GetElements().OfType<VSourcedElement>().ToList();
            AddCashValue(list1, MethodBase.GetCurrentMethod().ToString(), null);
            return list1;
        }
        public List<VQueryCall> GetQueryCallsInFields()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VQueryCall>);
            }
            var list2 = GetElements(TextConst.EName.Forms);
            var list3 = GetElements(TextConst.EName.Fields);
            list2.AddRange(list3);
            list3 = GetElements(TextConst.EName.Queries);
            list2.AddRange(list3);
            var list4 = list2.SelectMany(e => e.GetDescedantsP(e1 => (e1.Name == EName.listquery) || (e1.Name == EName.defaultquery))).ToList();
            var  list5 = list4.SelectAsArray(e1 => e1.GetElementsP().FirstOrDefault() as VQueryCall).ToList();
            AddCashValue(list5, MethodBase.GetCurrentMethod().ToString(), null);
            return list5;
        }
        public List<VSXElement> GetReportsAndQReports()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            var list = GetElements(TextConst.EName.Reports);
            var list1 = GetElements(TextConst.EName.Queries).Where(EPredicate.IsReport).ToList();
            list.AddRange(list1);
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        public List<VSXElement> GetQReports()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            var list = GetElements(TextConst.EName.Queries).Where(EPredicate.IsReport).ToList();
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        /*public VSXElement CreateExcelTemplateElement(string id)
        {
            XElement templateInfo = Printing.ExtractHeadInfo(id);
            templateInfo = Documenting.VPrintTemplate.ProcessTemplateInfo(templateInfo);
            XElement parent = Manager.GetNativeScheme().Elements(EName.excel_templates).FirstOrDefault();
            if (parent == null) {
                parent = new XElement(EName.excel_templates);
                ForAdd(Manager.GetNativeScheme()).Add(parent);
            }
            parent.Add(templateInfo);
            XElement templateCall = Manager.GetNativeScheme().Descendants(EName.excel).Elements(EName.template).First(e => e.Attribute(AName.name).Value == id);
            VReport rep = VSXElement.Get<VReport>(templateCall.Ancestors(EName.report).First());
            templateInfo.SetAttributeValue(AName.report, rep.P_IdName);
            templateInfo.SetAttributeValue(AName.name, id);
            templateInfo = VSXElement.Get(templateInfo);
            return (VSXElement)templateInfo;
        }*/
        public VQuery GetQuery(string name)
        {
            //if (WebReportsAdapter.IsWebItem(name))
            //{
            //    return VSXElement.Get<VQuery>(WebReportsAdapter.GetQueryXml(name)); 
            //}
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VQuery);
            }
            XElement q = this.manager.GetNativeScheme().Elements(EName.queries).Elements(EName.query).SearchByAttribute(AName.name, name);
            if (q == null) {
                return null;
            }
            VQuery qry = VSXElement.Get<VQuery>(q);
            AddCashValue(qry, MethodBase.GetCurrentMethod().ToString(), name);
            return qry;
        }
        public VQuery GetQueryByKeyDimensionName(string name)
        {
            if (string.IsNullOrEmpty(name)) {
                return null;
            }
            VDimension dim = this.GetDimension(name);
            if (dim == null) {
                return null;
            }
            VQuery q = this.GetQuery(dim.P_CalledQuery);
            return q;
        }
        /// <summary>
        /// Возвращает измерение по имени запроса
        /// </summary>
        /// <param name="name">наименование запроса</param>
        /// <returns>найденое измерение или null</returns>
        public VDimension GetDimensionByQueryName(string name)
        {
            if (string.IsNullOrEmpty(name)) {
                return null;
            }
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VDimension);
            }
            XElement el = this.manager.GetNativeScheme().Elements(EName.dimension_packages).Elements(EName.dimension_package).Elements(EName.dimension).Where(EPredicate.IsNotExcuded).SearchByAttribute(AName.class_type, name);
            if (el == null) {
                return null;
            }
            VDimension dim = VSXElement.Get<VDimension>(el);
            AddCashValue(dim, MethodBase.GetCurrentMethod().ToString(), name);
            return dim;
        }
        public IList<VDimension> GetDimensions()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as IList<VDimension>);
            }
            IList<XElement> dims = this.manager.GetNativeScheme().Elements(EName.dimension_packages).Elements(EName.dimension_package).Elements(EName.dimension).Where(EPredicate.IsNotExcuded).ToList();
            VDimension[] arr = new VDimension[dims.Count];
            for (int index = 0; index < dims.Count; index++) {
                arr[index] = VSXElement.Get<VDimension>(dims[index]);
            }
            AddCashValue(arr, MethodBase.GetCurrentMethod().ToString(), null);
            return arr;
        }
        /// <summary>
        /// Возвращает измерение по его наименованию
        /// </summary>
        /// <param name="name">наименование измерения</param>
        /// <returns>найденое измерение или null</returns>
        public VDimension GetDimension(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VDimension);
            }
            XElement el = this.manager.GetNativeScheme().Elements(EName.dimension_packages).Elements(EName.dimension_package).Elements(EName.dimension).Where(EPredicate.IsNotExcuded).SearchByAttribute(AName.name, name);
            if (el == null) {
                return null;
            }
            VDimension dim = VSXElement.Get<VDimension>(el);
            AddCashValue(dim, MethodBase.GetCurrentMethod().ToString(), name);
            return dim;
        }
        public VAction GetAction(string name)
        {
            if (this.IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (this.GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VAction);
            }
            VAction action = this.GetElement<VAction>(EName.actions, AName.name, name);
            this.AddCashValue(action, MethodBase.GetCurrentMethod().ToString(), name);
            return action;
        }
        public VField GetField(string id)
        {
            if (this.IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), id)) {
                return (this.GetCashValue(MethodBase.GetCurrentMethod().ToString(), id) as VField);
            }
            VField field = this.GetElement<VField>(EName.fields, AName.id, id);
            this.AddCashValue(field, MethodBase.GetCurrentMethod().ToString(), id);
            return field;
        }
        public VColor GetColor(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VColor);
            }
            XElement el = this.manager.GetNativeScheme().Elements(EName.color_packages).Elements(EName.color_package).Elements(EName.color).Where(EPredicate.IsNotExcuded).SearchByAttribute(AName.name, name);
            if (el == null) {
                return null;
            }
            VColor clr = VSXElement.Get<VColor>(el);
            AddCashValue(clr, MethodBase.GetCurrentMethod().ToString(), name);
            return clr;
        }
        /*public IList<VColor> GetColors()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as IList<VColor>);
            }
            IList<XElement> colors = this.Manager.GetNativeScheme().Elements(EName.color_packages).Elements(EName.color_package).Elements(EName.color).Where(EPredicate.IsNotExcuded).ToList();
            VColor[] arr = new VColor[colors.Count];
            for (int index = 0; index < colors.Count; index++) {
                arr[index] = VSXElement.Get<VColor>(colors[index]);
            }
            AddCashValue(arr, MethodBase.GetCurrentMethod().ToString(), null);
            return arr;
        }*/
        /*public VFormat GetFormat(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VFormat);
            }
            VFormat clr = null;
            foreach (VFormatPackage pkg in GetElements(TextConst.EName.FormatPackages)) {
                clr = pkg.GetFormat(name);
                if (clr != null) {
                    break;
                }
            }
            AddCashValue(clr, MethodBase.GetCurrentMethod().ToString(), name);
            return clr;
        }*/
        /*public IList<VFormat> GetFormats()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as IList<VFormat>);
            }
            List<VFormat> list = new List<VFormat>();
            foreach (VFormatPackage expPack in GetElements(TextConst.EName.FormatPackages)) {
                foreach (VFormat exp in expPack.GetElementsP()) {
                    list.Add(exp);
                }
            }
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }*/
        /// <summary>
        /// Возвращает все выражения (&lt;call&gt) из &lt;expression_packages&gt;
        /// </summary>
        /// <returns></returns>
        public IList<VExpression> GetExpressions()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as IList<VExpression>);
            }
            IList<XElement> expressions = this.manager.GetNativeScheme().Elements(EName.expression_packages).Elements(EName.expression_package).Elements(EName.call).Where(EPredicate.IsNotExcuded).ToList();
            VExpression[] arr = new VExpression[expressions.Count];
            for (int index = 0; index < expressions.Count; index++) {
                arr[index] = VSXElement.Get<VExpression>(expressions[index]);
            }
            AddCashValue(arr, MethodBase.GetCurrentMethod().ToString(), null);
            return arr;
        }
        public List<VRole> GetRoles()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VRole>);
            }
            List<VRole> list = new List<VRole>();
            foreach (VSXElement expPack in GetElements(TextConst.EName.SecurityPackages)) {
                foreach (VRole exp in expPack.GetElementsP()) {
                    list.Add(exp);
                }
            }
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        public List<VSXElement> GetFactColumns()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            List<VSXElement> list = new List<VSXElement>();
            foreach (VSXElement exp in GetElements(TextConst.EName.Queries).SelectMany(e => (e as VQuery).FactColumns()).Distinct()) {
                list.Add(exp);
            }
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        /// <summary>
        /// Возвращает выражение из &lt;expression_packages&gt; по его наименованию
        /// </summary>
        /// <param name="name">наименование выражения (call@as)</param>
        /// <returns>найденое выражение или null</returns>
        public VExpression GetExpression(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VExpression);
            }
            XElement el = this.manager.GetNativeScheme().Elements(EName.expression_packages).Elements(EName.expression_package).Elements(EName.call).Where(EPredicate.IsNotExcuded).SearchByAttribute(AName.@as, name);
            if (el == null) {
                return null;
            }
            VExpression exp = VSXElement.Get<VExpression>(el);
            AddCashValue(exp, MethodBase.GetCurrentMethod().ToString(), name);
            return exp;
        }
        public VRole GetRole(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VRole);
            }
            var exp = GetRoles().FirstOrDefault(r => r.P_Name == name);
            AddCashValue(exp, MethodBase.GetCurrentMethod().ToString(), name);
            return exp;
        }
        public List<VSXElement> GetQueryUseByColumnLink(string queryName)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), queryName)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), queryName) as List<VSXElement>);
            }
            var cols = this.manager.GetScheme().Elements(EName.queries).Elements(EName.query).Elements(EName.select).Elements().Where(e => e.AttrOrEmpty(AName.link) == queryName);
            List<VSXElement> list = new List<VSXElement>();
            foreach (XElement col in cols) {
                XElement qry = col.Ancestors(EName.query).First();
                VSourcedElement vqry = this.GetQuery(qry.Attribute(AName.name).Value).GetMainE();
                string colName = col.AttrOrEmpty(AName.@as);
                if (string.IsNullOrEmpty(colName)){
                    colName = col.AttrOrEmpty(AName.column);
                }
                VSXElement vcol = vqry.SearchColumn(colName);
                list.Add(vcol);
            }            
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), queryName);
            return list;
        }
        public VSXElement GetFactColumn(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VSXElement);
            }
            VSXElement col = null;
            var compiledQueries = this.manager.GetScheme().Elements(EName.queries).Elements(EName.query).Elements(EName.select).Elements().Where(e => e.AttrOrEmpty(AName.fact) == name);
            XElement compiled;
            if (compiledQueries.Count() > 1) {
                compiled = compiledQueries.First(e => e.Parent.Parent.Attribute(AName.inherit) == null);
            } else {
                compiled = compiledQueries.FirstOrDefault();
            }
            if (compiled != null) {
                var queryName = compiled.Parent.Parent.Attribute(AName.name).Value;
                col = GetQuery(queryName).SearchColumnByFactName(name);
            }
            //foreach (VQuery query in GetElements(TextConst.EName.Queries))
            //{
            //    VSXElement col1 = query.SearchColumnByFactName(name);
            //    if (col1 != null)
            //    {
            //        col = col1;
            //    }
            //}
            //   SchemeNative.Descendants().First(e => Cmn.GetAttrValue(e, TextConst.AName.Fact) == name);
            AddCashValue(col, MethodBase.GetCurrentMethod().ToString(), name);
            return col;
        }
        public VSXElement GetFactSource(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VSXElement);
            }
            VSXElement source = this.GetExpression(name);
            if (source == null) {
                source = this.GetFactColumn(name);
            }
            AddCashValue(source, MethodBase.GetCurrentMethod().ToString(), name);
            return source;
        }
        public List<VSXElement> GetSecurityObjects()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            var list = new List<VSXElement>();
            foreach (XElement el in this.manager.GetNativeScheme().Descendants().Where(e => e.Attribute(TextConst.AName.SecurityId) != null).ToList()) {
                var vel = VSXElement.Get(el);
                list.Add(vel);
            }
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        public VSXElement GetSecurityObject(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VSXElement);
            }
            XElement el = this.manager.GetNativeScheme().Descendants().SearchByAttribute(AName.security_id, name);
            if (el == null) {
                return null;
            }
            VSXElement vel = VSXElement.Get(el);
            AddCashValue(vel, MethodBase.GetCurrentMethod().ToString(), name);
            return vel;
        }
        /*public List<VDimension> GetDimensionsByTimeline(string name)
        {

            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as List<VDimension>);
            }

            var list = new List<VDimension>();


            foreach (VDimensionPackage pkg in GetElements(TextConst.EName.DimensionPackages))
            {

                foreach (VDimension dim in pkg.GetDimensions())
                {


                    if (dim.P_Timeline == name)
                    {
                        list.Add(dim);
                    }
                }
            }

            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), name);
            return list;
        }*/
        public VReport GetReport(string name)
        {
            XElement q = this.manager.GetNativeScheme().Elements(EName.reports).Elements(EName.report).SearchByAttribute(AName.name, name);
            return VSXElement.Get<VReport>(q);
        }
        public VSXElement GetReportOrQuery(string name)
        {
            VSXElement ret = GetReport(name);
            if (ret == null) {
                ret = GetQuery(name);
            }
            return ret;
        }
        public VForm GetForm(string name)
        {
            //if (WebReportsAdapter.IsWebItem(name))
            //{
            //    var reportXml = WebReportsAdapter.GetFormXml(name);
            //    return VSXElement.Get<VForm>(reportXml);
            //}
            XElement q = this.manager.GetNativeScheme().Elements(EName.forms).Elements(EName.form).SearchByAttribute(AName.name, name);
            if (q == null) {
                return null;
            } else {
                return VSXElement.Get<VForm>(q);
            }
        }
        public VSXElement GetFormOrQuery(string name)
        {
            VSXElement vform = this.GetForm(name);
            if (vform == null) {
                vform = this.GetQuery(name);
            }
            return vform;
        }
        public void LoadProject(string name)
        {
            this.manager.LoadProjectIfNeed(name);
        }
        public VForm GetFormOrQueryAsForm(string name)
        {
            VForm vform = this.GetForm(name);
            if (vform == null) {
                vform = this.GetFormFromQuery(name);
            }
            return vform;
        }
        public VForm GetFormFromQuery(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VForm);
            }
            VQuery qr = this.GetQuery(name);
            if (qr == null) {
                return null;
            }
            var xform = qr.GetFormXElement();
            VForm form = VSXElement.Get<VForm>(xform);
            //form.environment = qr.GetEnvironment();
            AddCashValue(form, MethodBase.GetCurrentMethod().ToString(), name);
            return form;
        }
        public VFunction GetFunction(string name)
        {
            XElement q = this.manager.GetNativeScheme().Elements(EName.functions).Elements(EName.function).SearchByAttribute(AName.name, name);
            return VSXElement.Get<VFunction>(q);
        }
        public VPart GetPart(string id)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), id)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), id) as VPart);//! без кешмрования baseelement будет работать некорректно
            }
            List<XElement> q = this.manager.GetNativeScheme().Elements(EName.parts).Elements(EName.part).Where(e => e.Attribute(AName.id).Value == id).ToList();
            VPart part = null;
            if (q.Count == 0) {
                q = this.manager.GetNativeScheme().Descendants().Where(e => e.Attribute(AName.part_id) != null && e.Attribute(AName.part_id).Value == id).ToList().SelectAsArray<XElement, XElement>(VSXElement.Get).ToList(); // было Select(e => (XElement)VSXElement.Get(e))
                XElement p = new XElement(EName.part);
                p.Add(q);
                part = VSXElement.Get<VPart>(p);
                //part.environment = this;
                part.VirtualParent = p;
            } else {
                part = VSXElement.Get<VPart>(q[0]);
            }
            AddCashValue(part, MethodBase.GetCurrentMethod().ToString(), id);
            return part;
        }
        public List<VSXElement> GetParts()
        {
            var list = this.manager.GetNativeScheme().Elements(EName.parts).Elements(EName.part).Select(VSXElement.Get).ToList();
            var virtParts = this.manager.GetNativeScheme().Descendants().Where(e => e.Attribute(AName.part_id) != null).Select(VSXElement.Get).ToList();
            list.AddRange(virtParts);
            return list;
        }
        public VSXElement CreateElement(string elementType, string templateName)
        {
            XElement parentElement = this.manager.GetNativeScheme().Elements().First(e1 => e1.AttrOrEmpty(TextConst.AName.ChildName) == elementType);
            XElement template;
            if (templateName != null) {
                template = parentElement.Elements().First(e1 => e1.AttrOrEmpty(AName.template_name) == templateName);
            } else {
                template = null;
            }
            VSXElement el = CreateElement(elementType, template);
            return el;
        }
        public string GetElementTypeKeyAttrName(string elementType)
        {
            XElement parentElement = this.manager.GetNativeScheme().Elements().First(e1 => e1.AttrOrEmpty(TextConst.AName.ChildName) == elementType);
            string keyName = parentElement.Attribute(AName.key_name).Value;
            return keyName;
        }
        public VSXElement CreateElement(string elementType, XElement template, string fileName = null)
        {
            XElement parentElement = this.manager.GetNativeScheme().Elements().First(e1 => Cmn.GetAttrValue(e1, TextConst.AName.ChildName) == elementType);
            XElement element;
            string name;
            XName keyName = XNamespace.None.GetName(parentElement.Attribute(AName.key_name).Value);
            if (template != null) {
                XAttribute attr = template.Attribute(AName.template_name);
                if (attr != null) {
                    name = attr.Value;
                } else {
                    name = template.Attribute(keyName).Value;
                }
                element = new XElement(template);
                element.RemoveAttribute(AName.template_name);
                element.RemoveAttribute(AName.file);
            } else {
                name = elementType;
                element = new XElement(elementType);
            }
            NameCheck ncheck = new NameCheck(
                this.manager.GetNativeScheme().Elements(parentElement.Name).Elements().Attributes(keyName).Select(APredicate.AttributeValue).ToList()
                // parentElement.Elements().Attributes(keyName).Select(a => a.Value).ToList()
                );
            name = ncheck.GetName(name);
            element.SetAttributeValue(keyName, name);
            parentElement.Add(element);
            var el = VSXElement.Get(element);
            if (fileName != null) {
                el.SetAttributeValue(AName.file, fileName);
            }
            return el;
        }
        /*public VQuery CreateForm()
        {
            XElement element = new XElement(EName.form);
            Manager.GetNativeScheme().Elements(EName.forms).First().Add(element);
            return VSXElement.Get<VQuery>(element);
        }*/
        public void SyncNavigators(VSXElement e)
        {
            throw new NotImplementedException();
            if (!(e is VQuery) && !(e is VReport) && !(e is VForm)) return;
            if (e.Attribute(AName.file) == null) return;
            var navigators = this.manager.GetNativeScheme().Elements(EName.navigators).Elements(EName.navigator).Select(VSXElement.Get).ToArray();
            foreach (var vsxElement in navigators) {
                //ucQueriesEditorItem.UpdateElementFileInfo(vsxElement);
            }
            string project = Cmn.ExtractProjectName(e.Attribute(AName.file).Value);
            var changed = new HashSet<VSXElement>();
            var elements = Array.Empty<VSXElement>();
            if (e is VReport || e.P_IsReport == TextConst.AVBool.True) {
                elements = navigators.Descendants(EName.usereport)
                    .Where(x => x.Attribute(AName.project).Value == project && x.Attribute(AName.report).Value == e.P_Name)
                    .ToList()
                    .SelectAsArray(VSXElement.Get);
                foreach (VSXElement element in elements) {
                    element.P_SelfTitle = e.P_SelfTitle;
                    element.P_Invisible = e.P_Invisible;
                    element.P_SecurityId = e.P_SecurityId;
                    var parent = element.GetMainParent();
                    if (!changed.Contains(parent)) {
                        changed.Add(parent);
                    }
                }
            } else if (e is VForm) {
                elements = navigators.Descendants(EName.useform).Where(x => x.Attribute(AName.project).Value == project && x.Attribute(AName.form).Value == e.P_Name).ToList().SelectAsArray(VSXElement.Get);
                foreach (VSXElement element in elements) {
                    element.P_SelfTitle = e.P_SelfTitle;
                    var parent = element.GetMainParent();
                    if (!changed.Contains(parent)) {
                        changed.Add(parent);
                    }
                }
            }
            foreach (VSXElement n in changed) {
                n.SaveInSourceFile();
            }
        }
    }
}