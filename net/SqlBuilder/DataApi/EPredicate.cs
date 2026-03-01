using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.DataApi
{
    /// <summary>
    /// Набор часто используемых предикатов типа Func&lt;XElement, bool&gt; 
    /// </summary>
    public static class EPredicate
    {
        public static bool IsNotExcuded(XElement e)
        {
            return !(e.AttrOrDefault(AName.exclude, false));
        }
        public static bool IsNotColumns(XElement e)
        {
            Contract.Assert(e != null);
            return e.Name != EName.columns;
        }
        public static bool IsNotConst(XElement e)
        {
            Contract.Assert(e != null);
            return e.Name != EName.@const;
        }
        public static bool IsNotQueryOrCall(XElement e)
        {
            // e => !(new string[] { "query", "call" }).Contains(e.Name.LocalName)
            Contract.Assert(e != null);
            return e.Name != EName.query && e.Name != EName.call;
        }
        public static bool IsToolbar(XElement e)
        {
            Contract.Assert(e != null);
            return e.Name == EName.toolbar;
        }
        public static bool IsContentOrForm(XElement e)
        {
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.content) || (name == EName.form);
        }
        public static bool IsFieldGroupOrFieldOrTabContainer(XElement e)
        {
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.fieldgroup) || (name == EName.field) || (name == EName.tabcontainer);
        }
        public static bool IsColumnOrFact(XElement e)
        {
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.column) || (name == EName.fact);
        }
        public static bool IsFieldOrUseField(XElement e)
        {
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.field) || (name == EName.usefield);
        }
        public static bool IsSelectOrDimensionOrMeasures(XElement e)
        {
            // "select", "dimension", "measures"
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.select) || (name == EName.dimension) || (name == EName.measures);
        }
        public static bool IsSelectOrWhereHavingOrStartOrConnectOrDimensionOrMeasures(XElement e)
        {
            // "select", "where", "connect", "start", "having", "dimension", "measures"
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.select) || (name == EName.where) || (name == EName.having) ||
                   (name == EName.start) || (name == EName.connect) ||
                   (name == EName.dimension) || (name == EName.measures);
        }
        public static bool IsQueryOrTable(XElement e)
        {
            // (new string[] { "query", "table" }).Contains(e.Name.LocalName)
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.query) || (name == EName.table);
        }
        public static bool IsAnyLink(XElement e)
        {
            // (new string[] { "link", "dlink", "elink", "slink" }).Contains(e.Name.LocalName))
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.link) || (name == EName.dlink) || (name == EName.elink) || (name == EName.slink);
        }
        public static bool IsLinkOrSLink(XElement e)
        {
            // (new string[] { "link", "slink" }).Contains(e.Name.LocalName)
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.link) || (name == EName.slink);
        }
        public static bool IsLinkOrDLinkOrSLink(XElement e)
        {
            // (new string[] { "link", "dlink", "slink" }).Contains(e.Name.LocalName)
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.link) || (name == EName.dlink) || (name == EName.slink);
        }
        public static bool IsQueryOrLinkOrDLinkOrSLink(XElement e)
        {
            // (new string[] { "query", "link", "dlink", "slink" }).Contains(e.Name.LocalName)
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.query) || (name == EName.link) || (name == EName.dlink) || (name == EName.slink);
        }
        public static bool IsCallOfWindowFunction(XElement e)
        {
            // public static string[] windowFuncNames = new string[] { TextConst.AVFunction.Over, TextConst.AVFunction.RowNumber, TextConst.AVFunction.DenseRank };
            Contract.Assert(e != null);
            if (e.Name != EName.call)
            {
                return false;
            }
            string func = e.AttrOrDefault(AName.function, null);
            return (func == TextConst.AVFunction.Over) || (func == TextConst.AVFunction.RowNumber) || (func == TextConst.AVFunction.DenseRank);
        }
        public static bool IsDimensionOrMeasures(XElement e)
        {
            // "dimension", "measures"
            Contract.Assert(e != null);
            XName name = e.Name;
            return (name == EName.dimension) || (name == EName.measures);
        }
        public static bool IsChildOfReportOrQuery(XElement e)
        {
            // e => (new string[] { "report", "query" }).Contains(e.Parent.Name.LocalName)
            XElement parent = e.Parent;
            if (parent == null)
            {
                return false;
            }
            else
            {
                XName parent_name = parent.Name;
                return (parent_name == EName.report) || (parent_name == EName.query);
            }
        }
        public static bool IsReport(XElement e)
        {
            Contract.Assert(e != null);
            return e.AttrOrDefault(AName.is_report, false);
        }
        #region Предикаты (типа Func&lt;XElement, string&gt;)
        public static string ElementValue(XElement e)
        {
            Contract.Assert(e != null);
            return e.Value;
        }
        #endregion
    }
}
