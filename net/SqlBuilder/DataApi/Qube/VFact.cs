using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
using System.Reflection;
using AName_ = sql.builder.DataApi.AName;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.DataApi
{
    public sealed class VFact : VColumn, IVParent
    {
        public VFact()
            : base(EName.fact)
        {
        }
        private static string[] child_nodes = { TextConst.EName.If, TextConst.EName.Buttons, TextConst.EName.WithParams, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        public VSXElement GetFactSource()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as VSXElement);
            }
            string column = this.P_Column;
            VSXElement el = null;
            VSourcedElement qry = this.RootQuery();
            if (qry != null) {
                el = qry.SearchExpression(column);
            }
            if (el == null) {
                el = XmlReports.Environment.GetFactSource(column);
            }
            AddCashValue(el, MethodBase.GetCurrentMethod().ToString(), null);
            return el;
        }
        public VSXElement GetConditionSource()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as VSXElement);
            }
            string condition = this.P_Condition;
            VSXElement el = null;
            VSourcedElement qry = this.RootQuery();
            if (qry != null) {
                el = qry.SearchExpression(condition);
            }
            if (el == null) {
                el = XmlReports.Environment.GetFactSource(condition);
            }
            AddCashValue(el, MethodBase.GetCurrentMethod().ToString(), null);
            return el;
        }
        public override List<VSXElement> SourceColumn()
        {
            VSXElement srcCol = GetFactSource();
            if (srcCol != null) {
                return srcCol.AsList();
            } else {
                return new List<VSXElement>();
            }
        }
        public override List<VSXElement> GetFactColumns()
        {
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }
            List<VSXElement> sources;
            VSXElement source = this.GetFactSource();
            VExpression expr = source as VExpression;
            if (expr != null) {
                IList<VSXElement> cols = expr.GetFactColumns();
                sources = new List<VSXElement>(cols.Count);
                sources.AddRange(cols);
            } else {
                sources = new List<VSXElement>(1);
                sources.Add(source);
            }
            AddCashValue(source, MethodBase.GetCurrentMethod().ToString(), null);
            return sources;
        }
        /*public List<string> GetDimensionsNames()
        {
            var names = new List<string>();
            VSXElement source = GetFactSource();

            if (source is VExpression)
            {
                names.AddRange((source as VExpression).GetDimensionsNames());
            }

            var scope = Scope();
            if (scope != null)
            {
                names.Add(scope.P_Cumulate);
            }

            return names;
        }*/
        public string GetFactId()
        {
            if (this.GetElementsP(EName.withparams).Count != 0) {
                return this.GetUniqueKey().ToString(); // заменить на анализ значений параметров
            } else {
                return this.P_Table + "." + this.P_Column;
            }
        }
        public class FactDependantceInfo
        {
            public FactDependantceInfo(string name)
            {
                Name = name;
            }
            public string Name;
            public string ObjectFact;
            public string Alias;

            public List<string> OutputDimensions = new List<string>();
            public List<string> NonOutputDimensions = new List<string>();
            public List<string> Conditions = new List<string>();
            public VSXElement Column = null;

            public bool IsObject()
            {
                return Name == "";
            }
            public string GetInfoId()
            {
                var s = Name;
                if (s == "")
                {
                    s = ObjectFact + ".";
                }
                OutputDimensions.Sort();
                NonOutputDimensions.Sort();
                Conditions.Sort();
                var od = string.Join(",", OutputDimensions);
                var nd = string.Join(",", NonOutputDimensions);
                var c = string.Join(",", Conditions);
                s += "(" + od + ")";
                s += "(" + nd + ")";
                s += "(" + c + ")";
                return s;
            }





            public FactSourceQueryInfo GetSourceQueryInfo(VEnvironment env, SortedList<string, FactSourceQueryInfo> qryList)
            {


                FactSourceQueryInfo qi = null;
                VSourcedElement qry = null;
                if (Name != "")
                {
                    var fact = env.GetFactColumn(Name);
                    qry = fact.RootQuery().GetMainE();
                }
                else
                {
                    qry = env.GetQueryByKeyDimensionName(ObjectFact);
                }


                var id = qry.P_Name;
                Conditions.Sort();
                var c = string.Join(",", Conditions);
                id += "(" + c + ")";

                if (!qryList.ContainsKey(id))
                {
                    qi = new FactSourceQueryInfo();
                    qi.Name = qry.P_Name;
                    qi.Conditions = Conditions.ToList();
                    qi.Query = (VQuery)qry;
                    qryList.Add(id, qi);

                }
                else
                {
                    qi = qryList[id];
                }

                qi.Facts.Add(this);
                return qi;
            }

        }
        public class FactSourceQueryInfo
        {
            public string Name;
            public List<string> Conditions;
            public List<VFact.FactDependantceInfo> Facts = new List<FactDependantceInfo>();
            public List<string> Dimensions = new List<string>();
            public VQuery Query;

        }
        /*private string Cumulate()
        {
            var scope = Scope();
            if (scope != null)
            {
                return scope.P_Cumulate;
            }
            return null;
        }*/
        /*public string CumulateAgg()
        {
            var scope = Scope();
            var s = "";
            if (scope != null)
            {
                s = scope.P_AggregationS;
            }

            if (s == "")
            {
                s = TextConst.AVGroup.Sum;
            }
            return s;
        }*/
        /*public string CumulateInfo()
        {
            var cumulate = Cumulate();
            var name = "";
            if (cumulate != null)
            {
                name += "," + cumulate;
            }
            return name;
        }*/
        /*public VDimSet GetDimset()
        {

            if (P_Table != "")
            {
                var qube = ((RootQuery() as VQuery).GetQubeElement());
                if (qube != null)
                {
                    return (VDimSet)qube.GetDimSet(P_Table);
                }
            }
            return null;
        }*/
        public XElement BuildFullExpression(XElement factPars, SortedList<string, FactDependantceInfo> infoList, List<string> conditions, List<string> outputDimensions, List<string> nonOutputDimensions /*не используется - можно убрать*/, SortedList<string, int> names)
        {
            VSXElement source = this.GetFactSource();
            if (source == null) {
                VSXElement main_parent = this.GetMainParent();
                throw new InvalidOperationException("Не найден факт " + this.P_Column + " использованный в " + main_parent.Name.LocalName + " " + main_parent.P_IdName);
            }
            var conds = conditions.ToList();
            this.GetCondInfo(conds);
            XElement expr;
            VExpression e = source as VExpression;
            if (e != null) {
                expr = e.BuildExpression(factPars, infoList, conds, outputDimensions, nonOutputDimensions, names);
            } else {
                expr = this.BuildExpression(source, infoList, conds, outputDimensions, nonOutputDimensions, names);
            }
            expr = this.ApplySpecAggregation(expr);
            return expr;
        }
        public void GetCondInfo(List<string> conditions)
        {
            string condition = this.P_Condition;
            if (!string.IsNullOrEmpty(condition)) {
                if (this.GetConditionSource().P_DontPushpred != TextConst.AVBool.True) {
                    if (!conditions.Contains(condition)) {
                        conditions.Add(condition);
                    }
                }
            }
        }
        public XElement BuildExpression(VSXElement srcCol, SortedList<string, VFact.FactDependantceInfo> infoList, List<string> conditions, List<string> outputDimensions, List<string> nonOutputDimensions, SortedList<string, int> names)
        {
            var info = new FactDependantceInfo(this.P_Column);
            info.Conditions = conditions.ToList();
            info.OutputDimensions = outputDimensions.ToList();
            info.Column = srcCol;
            string id = info.GetInfoId();
            if (!infoList.ContainsKey(id)) {
                if (!names.ContainsKey(info.Name)) {
                    names.Add(info.Name, 0);
                }
                names[info.Name]++;
                info.Alias = info.Name + names[info.Name].ToString();
                infoList.Add(id, info);
            } else {
                info = infoList[id];
            }
            string columnName = info.Alias;
            XElement elExpr = Factory.NewColumn(TextConst.Pfx.QubeQueryAlias, columnName);
            //Cmn.CopyOrReplaceAttributes(this, elExpr, Compiler.columnAttributesNamesCanDub);
            elExpr.CopyAttributes(this.Attributes().Where(APredicate.IsColumnAttributeCanDub));
            return elExpr;
        }
        #region Специальные способы агрегации
        private XElement ApplySpecAggregation(XElement elExpr)
        {
            VSXElement factSource = this.GetFactSource();
            if (factSource.P_AggregationS == TextConst.AVGroup.List) {
                XElement expr = Factory.NewCall(TextConst.AVFunction.Listagg);
                expr.CopyAttributes(elExpr.Attributes().Where(APredicate.IsColumnAttributeCanDub));
                elExpr.RemoveAttribute(AName_.group);
                expr.Add(elExpr);
                expr.Add(Factory.NewConst("'; '"));
                expr.Add(elExpr);
                return expr;
            } else {
                return elExpr;
            }
        }
        #endregion
        /*public List<string> GetCumulateDimensionsNames()
        {
            var names = new List<string>();
            VSXElement source = GetFactSource();

            if (source is VExpression)
            {
                names.AddRange((source as VExpression).GetCumulateDimensionsNames());
            }
            var scope = Scope();
            if (scope != null)
            {
                names.Add(scope.P_Cumulate);
            }

            return names;
        }*/
        /*private VScope Scope()
        {
            IList<VSXElement> list = this.GetElementsP(TextConst.EName.Scope);
            if (list.Count == 0) {
                return null;
            } else {
                return list[0] as VScope;
            }
        }*/
        public override void LookUpNextSources(List<VSXElement> list, VLookupAnalyzer analyzer)
        {
            if (!analyzer.CheckAndReturn(list, this)) {
                return;
            }
            VSXElement fSrc = this.GetFactSource();
            if (fSrc != null) {
                fSrc.LookUpNextSources(list, analyzer);
            }
        }
        #region Table
        public override void P_Table_ListRefresh(VDataTable table)
        {
            VSourcedElement root_query = this.RootQuery();
            if (root_query is VForm) {
                base.P_Table_ListRefresh(table);
            } else {
                table.Rows.Clear();
                VQuery query = root_query as VQuery;
                if (query != null) {
                    VQube qube = query.GetQubeElement();
                    if (qube != null) {
                        IList<VSXElement> dimsets = qube.DimSets();
                        for (int index = 0; index < dimsets.Count; index++) {
                            VDimSet dimset = (VDimSet)dimsets[index];
                            string alias = dimset.P_Alias;
                            table.AddRow(alias, alias);
                        }
                    }
                }
            }
        }
        #endregion
        #region Column
        public override void P_Column_List(VDataTable table)
        {
            table.ClearColumns();
            table.AddColumn("id");
            table.AddColumn("name", "Имя");
            table.AddColumn("title", "Заголовок");
            table.AddColumn("table", "Таблица");
            table.AddColumn("pack", "Пакет");
        }
        public override void P_Column_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            var list = new SortedList<string, string>();
            VSourcedElement root_query = this.RootQuery();
            if (root_query != null) {
                foreach (VExpression exp in root_query.Expressions()) {
                    string name = exp.XName;
                    list.Add(name, null);
                    table.AddRow(name, name, exp.P_Title, TextConst.AVTable.Ths, null);
                }
            }
            foreach (VExpression exp in XmlReports.Environment.GetExpressions()) {
                string name = exp.XName;
                list.Add(name, null);
                table.AddRow(name, name, exp.P_Title, null, exp.GetParent().P_IdName);

            }
            foreach (VSXElement exp in XmlReports.Environment.GetFactColumns()) {
                string name = exp.P_Fact;
                list.Add(name, null);
                table.AddRow(name, name, exp.P_Title, exp.RootQuery().P_IdName, null);
            }
        }
        #endregion
        #region Visible
        public override bool P_Visible_Exists()
        {
            return this.GetParent() is VOutputElement;
        }
        #endregion
        #region Default
        public override bool P_Default_Exists()
        {
            return false;
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
        #region Condition
        public override string P_Condition {
            get {
                return this.AttrOrEmpty(AName_.condition);
            }
            set {
                this.SetAttributeValue(AName_.condition, value);
            }
        }
        public override bool P_Condition_Exists()
        {
            return true;
        }
        public void P_Condition_List(VDataTable table)
        {
            table.AddColumn("id", "Имя");
            table.AddColumn("title", "Заголовок");
            table.AddColumn("table", "Таблица");
            table.AddColumn("pack", "Пакет");
        }
        public void P_Condition_ListRefresh(VDataTable table)
        {
            table.Rows.Clear();
            VSourcedElement root_query = this.RootQuery();
            IList<VExpression> expressions;
            VExpression exp;
            int index;
            if (root_query != null) {
                expressions = root_query.Expressions();
                for (index = 0; index < expressions.Count; index++) {
                    exp = expressions[index];
                    if (exp.P_DataTypeS == TextConst.AVDataType.Bool) {
                        table.AddRow(exp.XName, exp.P_Title, TextConst.AVTable.Ths, null);
                    }
                }
            }
            expressions = XmlReports.Environment.GetExpressions();
            for (index = 0; index < expressions.Count; index++) {
                exp = expressions[index];
                if (exp.P_DataTypeS == TextConst.AVDataType.Bool) {
                    table.AddRow(exp.XName, exp.P_Title, null, exp.GetParent().P_IdName);
                }
            }
        }
        #endregion
    }
}