using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Reflection;

namespace sql.builder.DataApi
{
    internal sealed partial class VEntityType
    {
        private VQuery query;
        internal VEntityType(VQuery query)
        {
            this.query = query;
        }
        internal VQuery Query { get { return this.query; } }
        /*internal List<VEntityTypeField> Fields()
        {
            List<XElement> items = this.query.Element(EName.select).Elements().ToList();
            List<VEntityTypeField> retItems = new List<VEntityTypeField>(items.Count);
            for (int index = 0; index < items.Count; index++) {
                retItems.Add(VEntityTypeField.GetOrCreate(items[index]));
            }
            return retItems;
        }*/
        private List<VSXElement> fromElementsMEI()
        {
            return this.query.GetMEIFromSections();
        }
        private List<VSXElement> fromElementsME()
        {
            return this.query.GetMEFromSections();
        }
        private List<VSXElement> selectElementsMEI()
        {
            return this.query.GetMEISelectSections();
        }
        private List<VSXElement> selectElementsME()
        {
            return this.query.GetMESelectSections();
        }
        internal List<VRelation> ParentLinks()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VRelation>);
            }
            List<VSXElement> items = fromElementsMEI().SelectMany(VSXElement.GetElementsP).Where(e => e.Attribute(AName.join) != null).ToList();
            var retItems = new SortedList<string, VRelation>();
            foreach (VRelation item in items) {
                if (retItems.ContainsKey(item.XName)) {
                    retItems.Remove(item.XName);
                }
                retItems.Add(item.XName, item);
            }
            var retItems1 = retItems.Values.ToList();
            AddCashValue(retItems1, MethodBase.GetCurrentMethod().ToString(), null);
            return retItems1;
        }
        internal List<VSXElement> AllDimensionLinks()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            List<VSXElement> items = new List<VSXElement>();
            items.AddRange(ParentDimensionLinks());
            items.AddRange(ColumnDimensionLinks());
            items.AddRange(AllExtDimensionLinks());
            AddCashValue(items, MethodBase.GetCurrentMethod().ToString(), null);
            return items;
        }
        internal List<VSXElement> ColumnDimensionLinks()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            List<VSXElement> items = selectElementsME().SelectMany(VSXElement.GetElementsP).Where(e => e.P_Dimension != "").ToList();
            AddCashValue(items, MethodBase.GetCurrentMethod().ToString(), null);
            return items;
        }
        internal List<VRelation> ParentDimensionLinks()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VRelation>);
            }
            List<VSXElement> items = this.fromElementsME().SelectMany(VSXElement.GetElementsP).Where(e => e.Attribute(AName.join) != null && e.P_Dimension != "").ToList();
            List<VRelation> retItems = new List<VRelation>(items.Count);
            for (int index = 0; index < items.Count; index++) {
                retItems.Add(VSXElement.Get<VRelation>(items[index]));
            }
            AddCashValue(retItems, MethodBase.GetCurrentMethod().ToString(), null);
            return retItems;
        }
        internal List<VQueryCall> PrimaryExtDimensionLinks()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VQueryCall>);
            }
            var list = AllExtDimensionLinks().Where(e => !(e is VDimLink)).ToList();
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        internal List<VQueryCall> SecondaryExtDimensionLinks()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VQueryCall>);
            }
            var list = AllExtDimensionLinks().Where(e => (e is VDimLink)).ToList();
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
            return list;
        }
        internal List<VQueryCall> AllExtDimensionLinks()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VQueryCall>);
            }
            List<VQueryCall> links = new List<VQueryCall>();
            if (!this.query.IsInherit()) {
                VQueryCall ms = this.query.MainSource();
                if (ms != null) {
                    links = this.query.MainSource().AllLinks(null);
                }
            }
            var linksElement = this.query.GetElementsP(EName.links).FirstOrDefault();
            if (linksElement != null) {
                foreach (VSXElement el in VSXElement.GetDescedantsAndSelfP(linksElement)) {
                    if (el is VLink || el is VELink || el is VDimLink) {
                        links.Add((VQueryCall)el);
                    }
                }
            }
            var list = links.Where(e => e.P_Dimension != "").ToList();
               if (list.Count != 0) {
                   list = list.Where(e1 => e1.RootQuery().GetMainE() == this.query.GetMainE()).ToList();
               }
               list = list.Distinct().ToList(); // попадают дубли, манипуляции выше понятны не до конца, похэтому такж
               //var list1 = list;
               //var list2=list1.Distinct().ToList();
               // if (list2.Count() != list1.Count())
               //{
               //}
               AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);
           return list;
        }
        internal VRelation ParentLink(string name)
        {
            XElement item = fromElementsMEI().Elements().ToList().SelectAsArray(VSXElement.Get).FirstOrDefault(e => e.Attribute(AName.join) != null && e.XName == name);
            if (item != null) {
                return VSXElement.Get<VRelation>(item);
            } else {
                return null;
            }
        }
        //public VRelation ParentDimensionLink(string name)
        //{
        //    XElement item = fromElements().Elements().ToList().Select(e1 => VSXElement.Get(e1)).Where(e => e.Attribute("join") != null && e.P_Dimension == name).FirstOrDefault();
        //    if (item != null)
        //    {
        //        return (VRelation)VSXElement.Get(item);
        //    }
        //    return null;
        //}
        internal List<VRelation> ChildLinks()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VRelation>);
            }
            string query_name = this.query.Name;
            List<XElement> items = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries).Elements()
                .Where(e => e.AttrOrEmpty(AName.@class) == "1")
                .Elements(EName.from).Elements(EName.query).Where(e => e.AttrOrDefault(AName.name, string.Empty) == query_name && e.Attribute(AName.join) != null).ToList();
            List<XElement> items1 = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries).Elements().Elements(EName.push)
               .Elements(EName.from).Elements(EName.query).Where(e => e.AttrOrDefault(AName.name, string.Empty) == query_name && e.Attribute(AName.join) != null).ToList();
            items.AddRange(items1);
            if (this.query.IsInherit()) {
                string inherit = this.query.AttrOrDefault(AName.inherit, string.Empty);
                items1 = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.queries).Elements()
                .Where(e => e.AttrOrEmpty(AName.@class) == "1")
                .Elements(EName.from).Elements(EName.query).Where(e => e.AttrOrDefault(AName.name, string.Empty) == inherit && e.Attribute(AName.join) != null).ToList();
                items.AddRange(items1);
            }
            items = items.Where(e => e.AttrOrDefault(AName.exclude, string.Empty) != TextConst.AVBool.True).ToList();
            List<VRelation> retItems = new List<VRelation>(items.Count);
            for (int index = 0; index < items.Count; index++) {
                retItems.Add(VSXElement.Get<VRelation>(items[index]));
            }
            AddCashValue(retItems, MethodBase.GetCurrentMethod().ToString(), null);
            return retItems;
        }
        internal VRelation ChildLink(string name)
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), name)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), name) as VRelation);
            }
            VRelation rel = null;
            foreach (VRelation r in this.ChildLinks()) {
                if (r.P_DXName == name) {
                    rel = r;
                    break;
                }
            }
            AddCashValue(rel, MethodBase.GetCurrentMethod().ToString(), name);
            return rel;
        }
        //public List<VRelation> ChildDimensionLinks()
        //{
        //    if (IsCashValueExists())
        //    {
        //        return (GetCashValue() as List<VRelation>);
        //    }
        //    var items = Query.GetEnvironment().GetElements(TextConst.EName.Queries).Select(q => (VQuery)q).SelectMany(q1 => q1.EntityType.ParentDimensionLinks())
        //        .Where(
        //        r =>r.P_AllowBackReference==TextConst.AVBool.True &&   r.P_Dimension == Query.GetDimension().P_IdName
        //        ).Distinct().ToList();
        //    AddCashValue(items);
        //    return items;
        //}
    }
}