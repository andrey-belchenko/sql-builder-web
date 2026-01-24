using System;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;call&gt; внутри &lt;expression-package&gt; или &lt;expressions&gt;
    /// </summary>
    internal sealed class VExpression : VCall, IVParent
    {
        internal VExpression()
            : base()
        {
        }
        private IList<VFact> GetFacts()
        {
            IList<VSXElement> list = this.GetDescedantsP(EName.fact);
            VFact[] facts = new VFact[list.Count];
            for (int index = 0; index < list.Count; index++) {
                facts[index] = (VFact)list[index];
            }
            return facts;
        }
        internal IList<VSXElement> GetDimensions()
        {
            return this.GetDescedantsP(EName.column);
        }
        public override List<VSXElement> GetFactColumns()
        {
            IList<VFact> facts = this.GetFacts();
            List<VSXElement> sources = new List<VSXElement>(facts.Count);
            for (int index = 0; index < facts.Count; index++) {
                VSXElement src = facts[index].GetFactSource();
                VExpression expr = src as VExpression;
                if (expr == null) {
                    sources.Add(src);
                } else {
                    sources.AddRange(expr.GetFactColumns());
                }
            }
            return sources;
        }
        /*private IList<string> GetDimensionsNames()
        {
            IList<VSXElement> dims = this.GetDimensions();
            List<string> names = new List<string>(dims.Count);
            int index;
            for (index = 0; index < dims.Count; index++) {
                names.Add(dims[index].P_Table);
            }
            dims = null;
            //
            IList<VFact> facts = this.GetFacts();
            for (index = 0; index < facts.Count; index++) {
                VExpression expr = facts[index].GetFactSource() as VExpression;
                if (expr != null) {
                    names.AddRange(expr.GetDimensionsNames());
                }
            }
            //
            string cumulate = this.P_Cumulate;
            if (!string.IsNullOrEmpty(cumulate)) {
                names.Add(cumulate);
            }
            return names;
        }*/
        /*private IList<VSXElement> GetDimensionsWithChilds(List<int> parents = null)
        {
            IList<VSXElement> dims = this.GetDimensions();
            List<VSXElement> list = new List<VSXElement>(dims.Count);
            if (parents == null) {
                parents = new List<int>();
            }
            int key = this.GetUniqueKey();
            parents.Add(key);
            list.AddRange(dims);
            IList<VFact> facts = this.GetFacts();
            for (int index = 0; index < facts.Count(); index++) {
                VExpression expr = facts[index].GetFactSource() as VExpression;
                if (expr != null) {
                    if (!parents.Contains(expr.GetUniqueKey())) {
                        list.AddRange(expr.GetDimensionsWithChilds(parents));
                    } else {
                        throw new System.InvalidOperationException("Цикл в выражениях:" + expr.XName);
                    }
                }
            }
            parents.Remove(key);
            return list;
        }*/
        public static List<VSXElement> GetDimensionDependentFacts(VSXElement dimension)
        {
            var parentExp = (VExpression)dimension.GetAncestorsAndSelf(EName.call).Last();
            return parentExp.GetFactColumns();
        }
        //public void GetFactDimDependanceInfo(SortedList<string, VFact.FactDependantceInfo> infoList, List<string> conditions, List<string> outputDimensions, List<string> nonOutputDimensions)
        //{
        //    var list = new SortedList<string, VFact.FactDependantceInfo>();
        //    var dims = GetDimensions();
        //    var odims = outputDimensions.ToList();
        //    foreach (VSXElement dim in dims)
        //    {
        //        if (!odims.Contains(dim.P_Table))
        //        {
        //            odims.Add(dim.P_Table);
        //        }
        //    }
        //    var facts = GetFacts();
        //    foreach (VFact fact in facts)
        //    {
        //        fact.GetFactDimDependanceInfo(infoList, conditions, odims, nonOutputDimensions);               
        //    }
        //}
        internal XElement BuildExpression(XElement factPars, SortedList<string, VFact.FactDependantceInfo> infoList, List<string> conditions, List<string> outputDimensions, List<string> nonOutputDimensions, SortedList<string, int> names)
        {
            XElement elExpr = new XElement(this.Name);
            VSXElement expr = VSXElement.Get(new XElement(this));
            expr.VirtualParent = this.GetParent();
            VPart.ApplyParams(expr, factPars, expr.GetElementsP(EName.@params).FirstOrDefault());
            Cmn.copyAttributes(expr, elExpr);
            var dims = this.GetDimensions();
            var odims = outputDimensions.ToList();
            foreach (VSXElement dim in dims) {
                if (!odims.Contains(dim.P_Table)) {
                    odims.Add(dim.P_Table);
                }
            }
            BuildExpressionLevel(expr, elExpr, infoList, conditions, odims, nonOutputDimensions, names);
            return elExpr;
        }
        //public SortedList<string, List<string>> GetFactDimDependanceInfo()
        //{
        //    var list = new SortedList<string, List<string>>();
        //    var dims = GetDimensionsWithChilds();
        //    var facts = GetFactColumns();
        //    foreach (VSXElement fact in facts)
        //    {
        //        if (!list.ContainsKey(fact.P_Fact))
        //        {
        //            list.Add(fact.P_Fact, new List<string>());
        //        }
        //    }
        //    foreach (VSXElement dim in dims)
        //    {
        //        var depFacts = GetDimensionDependentFacts(dim);
        //        foreach (VSXElement fact in depFacts)
        //        {
        //            if (!list[fact.P_Fact].Contains(dim.P_Table))
        //            {
        //                list[fact.P_Fact].Add(dim.P_Table);
        //            }
        //        }
        //    }
        //    return list;
        //}
        /*internal IList<string> GetCumulateDimensionsNames()
        {
            IList<VFact> facts = this.GetFacts();
            List<string> names = new List<string>();
            for (int index = 0; index < facts.Count(); index++) {
                VExpression expr = facts[index].GetFactSource() as VExpression;
                if (expr != null) {
                    names.AddRange(expr.GetCumulateDimensionsNames());
                }
            }
            string cumulate = this.P_Cumulate;
            if (!string.IsNullOrEmpty(cumulate)) {
                names.Add(cumulate);
            }
            return names;
        }*/
        //public XElement BuildExpressionOld(XElement factPars, string pfx)
        //{
        //    var elExpr = new XElement(this.Name.LocalName);
        //    var expr = VSXElement.Get(new XElement(this));
        //    expr.VirtualParent = this.GetParent();
        //    expr.environment = this.GetEnvironment();
        //    VPart.ApplyParams(expr, factPars, expr.GetElementsApplyingParts(TextConst.EName.Params).FirstOrDefault());
        //    Cmn.copyAttributes(expr, elExpr);
        //    BuildExpressionLevel(expr, elExpr, pfx);
        //    return elExpr;
        //}
        internal IList<VParam> FormalParams()
        {
            IList<VSXElement> pars = this.GetElementsP(EName.@params);
            if (pars.Count != 0) {
                IList<VSXElement> p = pars[0].GetElementsP();
                List<VParam> list = new List<VParam>(p.Count);
                for (int index = 0; index < p.Count; index++) {
                    list.Add((VParam)p[index]);
                }
                return list;
            } else {
                return new VParam[0];
            }
        }
        private void BuildExpressionLevel(VSXElement source, XElement target, SortedList<string, VFact.FactDependantceInfo> infoList, List<string> conditions, List<string> outputDimensions, List<string> nonOutputDimensions, SortedList<string, int> names)
        {
            foreach (VSXElement el in source.GetElementsP()) {
                if (el.Name == EName.@params) continue;
                XElement elExpr = null;
                VFact fact = el as VFact;
                if (fact != null) {
                    var odims = outputDimensions.ToList();
                    VExpression condSrc = null;
                    if (fact.P_Condition != "") {
                        condSrc = fact.GetConditionSource() as VExpression;
                        if (condSrc.P_DontPushpred == TextConst.AVBool.True) { // Условия которые нужно преобразовать в If, нужно собрать измерения из условия
                            var dims = condSrc.GetDimensions(); //использование именованного предиката в предикате не обрабатывается, доделать
                            foreach (VSXElement dim in dims) {
                                if (!odims.Contains(dim.P_Table)) {
                                    odims.Add(dim.P_Table);
                                }
                             }
                         } else {
                             condSrc = null;
                         }
                    }
                    var conds = conditions.ToList();
                    fact.GetCondInfo(conds);
                    VSXElement elSrc1 = fact.GetFactSource();
                    if (elSrc1 == null) {
                        throw new InvalidOperationException("Не найден факт " + fact.P_Column + " использованный в " + fact.GetMainParent().Name.LocalName + " " + fact.GetMainParent().P_IdName);
                    }
                    VExpression elSrc = elSrc1 as VExpression;
                    if (elSrc != null) {
                        elExpr = elSrc.BuildExpression(el.Element(EName.withparams), infoList, conds, odims, nonOutputDimensions,names); // Пока параметры подставляются только для верх. ур. Доделать если будет нужно. // Вроде, доделал
                        elExpr.RemoveAttribute(AName_.group);
                    } else {
                        elExpr = (el as VFact).BuildExpression(elSrc1, infoList, conds, odims, nonOutputDimensions, names);
                    }
                    if (condSrc != null) {
                        XElement cndFullExpr = condSrc.BuildExpression(null /*если нужны будут параметры, обработать тут*/, infoList, conds, outputDimensions, nonOutputDimensions, names);
                        XElement expr1 = new XElement(EName.call);
                        Cmn.copyAttributes(elExpr, expr1);
                        expr1.SetAttributeValue(AName_.function, TextConst.AVFunction.If);
                        expr1.Add(new XElement(cndFullExpr));
                        expr1.Add(elExpr);
                        elExpr = expr1;
                    }
                }
                if (elExpr == null) {
                    elExpr = new XElement(el.Name);
                    Cmn.copyAttributes(el, elExpr);
                    if (el is VConst) {
                        elExpr.Value = el.Value;
                    } else {
                        BuildExpressionLevel(el, elExpr, infoList, conditions, outputDimensions, nonOutputDimensions, names);
                    }
                }
                target.Add(elExpr);
            }
        }
        private static string[] child_nodes = { TextConst.EName.Call, TextConst.EName.Fact, TextConst.EName.Const, TextConst.EName.Array, TextConst.EName.Column, TextConst.EName.Params, TextConst.EName.UseParam, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region Title
        public override string P_Title {
            get {
                return this.P_SelfTitle;
            }
        }
        #endregion
        #region Fact
        public override string P_Fact {
            get {
               return this.XName;
            }
        }
        public override bool P_Fact_Editable()
        {
            return false;
        }
        public override bool P_Fact_Exists()
        {
            return true;
        }
        #endregion
        #region FactDimension
        public override bool P_FactDimension_Exists()
        {
            return true;
        }
        #endregion
        #region DontPushpred
        public override bool P_DontPushpred_Exists()
        {
            return this.P_DataTypeS == TextConst.AVDataType.Bool;
        }
        #endregion
        #region DontPush
        public override bool P_DontPush_Exists()
        {
            return this.P_DataTypeS == TextConst.AVDataType.Bool;
        }
        #endregion
    }
}