using System;
using System.Diagnostics; // Debug, Stopwatch
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Xml;
using System.Data;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
//using infoenergo.core.Extensions;
using sql.builder.DataApi;
using sql.builder.XmlHelpers;
using sql.builder.Exceptions;
using sql.builder.Clean.Extensions;

//using DevExpress.DashboardCommon.Native;

namespace sql.builder
{
    internal static partial class Compiler
    {
        //public static List<VSXElement> SchemeRoot = new List<VSXElement>();
        //public static List<VSXElement> schemeRootOld = new List<VSXElement>();

        //internal static XElement compileQuery(string name, IEnumerable<VSXElement> scheme)
        //{
        //    XElement query = XmlReports.Environment.Manager.GetScheme()
        //        .Elements("queries").Elements("query").FirstOrDefault(q => q.Attribute("name").Value == name);

        //    return compileQuery(query, true, null, scheme);
        //}
        internal static bool isProcessingPivots = false;
        private static bool isProcessingMatDummies = false;
        private static bool doMatrializeByHint = true;
        private static int icounter = 0;
        private static int nextIndex()
        {
            icounter++;
            return icounter;
        }

        internal static XElement compileQuery(XElement query, bool clearMatSet, XElement rep)
        {
            Reset();

            //processingTitles(query);
            processingAddNames(query);
            if (Cmn.GetAttrValue(query, "use-repository") == "1")
            {
                if (!dontUseRepositories)
                {
                    useRepositories = true;
                }
            }

            if (clearMatSet)
            {
                ClearMatSetAndPivotQueriesList();
            }
            if (getAttrValue(query, "pushpred") == "1")
            {
                pushpred = true;
            }
            else
            {
                pushpred = false;
            }
            XElement ret = new XElement("root");
            ret.Add(main(query, rep));
            ret.Descendants("withparams").Remove();
            //
            if (!isProcessingPivots)
            {
                pivotMatQueriesNames = new List<string>();
                isProcessingPivots = true;
                processingPivots(ret, null);
                ProcessingNestedOversAndSiblingsSort(ret);
                isProcessingPivots = false;

                if (doMatrializeByHint)
                {
                    processingMaterializedByHint(ret);
                }
            }
            else
            {
                //if (!isProcessingMatDummies)
                //{
                //    ProcessingNestedOvers(ret);
                //}
                ProcessingNestedOversAndSiblingsSort(ret);
            }


            ClearDoubleGroup(ret);
            if (!isProcessingPivots)
            {
                useRepositories = false;
            }
            return ret;
        }

        internal static string[] AdditionalAttributes = new string[] { TextConst.AName.Colset, TextConst.AName.ParName, TextConst.AName.Color, TextConst.AName.FontColor, TextConst.AName.ClientCalulation, TextConst.AName.ExcelCalulation, TextConst.AName.HAlign, TextConst.AName.MergeKey, TextConst.AName.IsFactUse };
        private static IEnumerable<XElement> getColumnsWithAdditionalAttr(XElement qry)
        {
            return qry.Elements(EName.select).Elements().Where(e => e.Attributes().Any(APredicate.IsAdditionalAttribute));
        }
        private static List<XElement> getQueryCalls(XElement qry)
        {
            if (qry.Attribute("name") != null)
            {
                return XmlReports.Environment.Manager.GetScheme().Elements("queries").Descendants("from").Elements().Where(e => getAttrValue(e, "name") == getAttrValue(qry, "name")).ToList();
            }
            else
            {
                List<XElement> list = new List<XElement>();
                list.Add(qry);
                return list;
            }
        }
        private static IEnumerable<XElement> getQueryCallSelColumns(XElement qryCall)
        {
            return qryCall.Ancestors("query").First().Elements("select").Descendants("column").Where(e => getAttrValue(e, "table") == getAttrValue(qryCall, "as"));
        }

        private static IEnumerable<XElement> getQueryCallSelColumns(XElement qryCall, XElement sourceColumn)
        {
            return getQueryCallSelColumns(qryCall).Where(e => getAttrValue(e, "column") == getAttrValue(sourceColumn, "as"));
        }
        private static void collectAdditionalAttrs(XElement el)
        {
            List<XElement> qryWithAttrCols = el.DescendantsAndSelf(EName.query).Where(q => getColumnsWithAdditionalAttr(q).FirstOrDefault() != null).ToList();
            while (qryWithAttrCols.Count != 0) {
                XElement qry1 = qryWithAttrCols[0];
                foreach (XElement qry2 in getQueryCalls(qry1)) {
                    foreach (XElement col1 in getColumnsWithAdditionalAttr(qry1).ToList()) {
                        foreach (XElement col2 in getQueryCallSelColumns(qry2, col1)) {
                            foreach (XAttribute attr1 in col1.Attributes().Where(APredicate.IsAdditionalAttribute)) {
                                if (col2.Attribute(attr1.Name) == null) {
                                    col2.SetAttributeValue(attr1.Name, attr1.Value);
                                    if (col2.Parent.Name == EName.select) {
                                        if (!qryWithAttrCols.Contains(col2.Parent.Parent)) {
                                            qryWithAttrCols.Add(col2.Parent.Parent);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
                qryWithAttrCols.Remove(qry1);
            }
        }
        private static string[] pivAttrsNames = new string[] { "value-column", "dimension-column", "dimension-value", "band-title", "value-title" };
        private static void ProcessingNestedOversAndSiblingsSort(XElement el)
        {
            normalizeMultiplicers(el);
            processingNestedOver(el);
            // processingWindowWithSiblingsSort(el);  // пока убираю не удалось применить
        }

        private static void normalizeMultiplicers(XElement el)
        {

            el.Descendants().Where(e => e.Attribute(TextConst.AName.Multiplicer) != null).ToArray().Descendants().Attributes(TextConst.AName.Multiplicer).Distinct().Remove();
        }
        
        private static void processingNestedOver(XElement el)
        {
            //  return;
            int additionId = 1;
            int exprId = 1;
           


         //  el.Descendants().Attributes(TextConst.AName.Group).Where(a => a.Value == TextConst.AVGroup.Inner).Remove();


          
            while (true)
            {
                List<XElement> queries = el.Descendants(EName.query).Where(q => q.AttrOrDefault(TextConst.AName.Materialize, string.Empty) != "2").Where(e3 =>
                    e3.Elements(EName.select).Descendants().Where(EPredicate.IsCallOfWindowFunction).Where(e1 =>
                    e1.Ancestors().FirstOrDefault(EPredicate.IsCallOfWindowFunction) != null ||
                    e1.AncestorsAndSelf(EName.call).Where(e2 => e2.AttrOrDefault(AName.group, string.Empty) != string.Empty).FirstOrDefault() != null

                    ).FirstOrDefault() != null).Distinct().ToList();






                if (queries.Count == 0)
                {
                    return;
                }
                foreach (XElement qry in queries)
                {

                    
                    var listaggs = qry.Descendants(TextConst.EName.Call).Where(e => e.Attribute(TextConst.AName.Function).Value == TextConst.AVFunction.Listagg && Cmn.GetAttrValue(e, TextConst.AName.Group) == TextConst.AVGroup.Inner).ToArray();

                    foreach (var lagg in listaggs)
                    {
                        foreach (var pp in lagg.Elements())
                        {
                            if (pp.Name.LocalName != TextConst.EName.Text && pp.Name.LocalName != TextConst.EName.Const)
                            {
                                pp.SetAttributeValue(TextConst.AName.Group, TextConst.AVGroup.Inner);
                            }
                        }
                        lagg.Attributes(TextConst.AName.Group).Remove();
                    }
                   
                    
                    string qalias = "ovr" + additionId.ToString();
                    additionId++;
                    XElement parentQry = new XElement("query", copyAttribute(qry, "name"), copyAttribute(qry, "materialize"), copyAttribute(qry, "as"), copyAttribute(qry, "hint"), copyAttribute(qry, "join"), copyAttribute(qry, "order"),
                        new XElement("select"),
                        new XElement("from")
                        );
                    parentQry.Add(qry.Elements("call"));
                    parentQry.Add(qry.Elements("group"));
                    parentQry.Add(qry.Elements("having"));

                    qry.Elements("call").Remove();
                    qry.Elements("group").Remove();
                    qry.Elements("having").Remove();
                    qry.Attributes("hint").Remove();
                    qry.Attributes("join").Remove();
                    qry.Attributes("order").Remove();
                    qry.Attributes("name").Remove();
                    qry.Attributes("materialize").Remove();
                    bool hasGrNode = false;
                    if (parentQry.Element("group") != null)
                    {
                        hasGrNode = true;
                    }

                    List<XElement> grSpecCols = new List<XElement>();
                    bool hasGr = qry.Elements("select").Descendants().Attributes(TextConst.AName.Group).Any();
                    foreach (XElement col in qry.Elements("select").Elements().ToList())
                    {


                        var newCol = makeColumnCall(col, qalias, hasGrNode, grSpecCols);
                        if (!hasGr)//20171207 подставлялась левая группировка назаначенная через agg для факта, попробуем так
                        {
                            newCol.Attributes(TextConst.AName.Group).Remove();
                        }
                        parentQry.Element("select").Add(newCol);

                    }



                    List<XElement> childOvers = qry.Elements("select").Descendants("call").Where(EPredicate.IsCallOfWindowFunction)
                        // первый уровень вложенности под группировкой или over
                        .Where(e =>


                               (e.Ancestors().Where(EPredicate.IsCallOfWindowFunction)
                                .Where(e2 => !e2.AncestorsAndSelf().Where(e3 => e3.AttrOrDefault(AName.group, string.Empty) != string.Empty).Any()) // поменял 17.10.16 ,условие  07.10 было некорректным не работал отчет по исполнителю
                                .Count() == 1
                                   // && e.Ancestors("call").Where(e1 => getAttrValue(e1, "group") != "").FirstOrDefault() == null // добавил 07.10.16
                              )

                            ||


                            ((!e.Ancestors().Any(EPredicate.IsCallOfWindowFunction)) &&
                            e.AncestorsAndSelf("call").Where(e1 => getAttrValue(e1, "group") != "").FirstOrDefault() != null)
                        ).Distinct().ToList();
                    List<XElement> otherCallsWithNoGroup = null;
                    if (qry.Elements("select").Elements().Where(e => getAttrValue(e, "group") != "").Any())
                    {
                        List<XElement> otherCallsWithNoGroup1 = qry.Elements(EName.select).Elements(EName.call).Where(e => e.AttrOrDefault(AName.group, string.Empty) == string.Empty)
                            .Where(e1 => !e1.DescendantsAndSelf().Any(EPredicate.IsCallOfWindowFunction))
                            .ToList();

                        otherCallsWithNoGroup1.AddRange(qry.Elements(TextConst.EName.Having).Elements().ToList());
                        if (otherCallsWithNoGroup1.Descendants().Where(e => getAttrValue(e, "group") != "" && getAttrValue(e, "group") != TextConst.AVGroup.Sum).Any())
                        {
                            otherCallsWithNoGroup = otherCallsWithNoGroup1.Descendants().Where(e => getAttrValue(e, "group") != "").ToList();

                        }

                    }

                    List<XElement> fields = childOvers.SelectAsArray(childOver => childOver.AncestorsAndSelf().First(e => e.Parent.Name == EName.select)).Distinct().ToList();

                    if (otherCallsWithNoGroup != null && otherCallsWithNoGroup.Count != 0)// Обычные функции с группировкой не на верхнем уровне, если буде такой случай вместе с over ничего не получится
                    {
                        foreach (XElement childOver in otherCallsWithNoGroup)
                        {
                            childOver.SetAttributeValue("group1", getAttrValue(childOver, "group"));
                        }
                        childOvers.AddRange(otherCallsWithNoGroup);
                        List<XElement> fields1 = otherCallsWithNoGroup.SelectAsArray(childOver => childOver.AncestorsAndSelf().First(e => e.Parent.Name == EName.select)).Distinct().ToList();
                        foreach (XElement field in fields1)
                        {
                            field.SetAttributeValue(TextConst.AName.Group, "");
                        }
                        fields.AddRange(fields1);
                    }



                    // List<string> expNames = new List<string>();
                    List<string> expNames = qry.Elements("select").Elements().Attributes("as").Select(a => a.Value).ToList();//.Where(s=>!s.StartsWith("expr")).ToList()


                    // пытаюсь избавиться от одинаковых выражений
                    SortedList<string, string> readyExprs = new SortedList<string, string>();


                    foreach (XElement childOver in childOvers)
                    {
                        string exprName = null;
                        if (childOver.Parent.Name.LocalName == "select")
                        {
                            childOver.SetAttributeValue("group1", getAttrValue(childOver, "group"));
                            childOver.Attributes("group").Remove();

                        }
                        else
                        {

                            var exprString = childOver.ToString();
                            bool isReady = readyExprs.TryGetValue(exprString, out exprName);

                            if (!isReady)
                            {
                                exprName = null;
                            }

                            if (!isReady)
                            {
                                if (childOver.Attribute("thissrc") != null)
                                {
                                    exprName = "expr" + childOver.Attribute("thissrc").Value;
                                }
                                else
                                {
                                    exprName = "expr" + exprId.ToString();
                                    exprId++;
                                }
                                if (!expNames.Contains(exprName))
                                {
                                    readyExprs.Add(exprString, exprName);
                                    XElement newCol = new XElement(childOver);
                                    newCol.SetAttributeValue("as", exprName);
                                    newCol.Attributes("key").Remove();

                                    copyAttribute(childOver, newCol, "thissrc");
                                    qry.Element("select").Add(newCol);
                                    expNames.Add(exprName);
                                }
                            }

                            XElement newColCall = new XElement("column", new XAttribute("table", qalias), new XAttribute("column", exprName));

                            if (childOver.Attribute("group1") != null)
                            {
                                newColCall.SetAttributeValue("group", childOver.Attribute("group1").Value);
                                childOver.Attributes("group1").Remove();
                            }

                            childOver.ReplaceWith(newColCall);




                            //  exprId++;// лишнее ?
                        }
                    }


                    readyExprs = new SortedList<string, string>();

                    foreach (XElement field in fields)
                    {

                        XElement field1 = parentQry.Elements("select").Elements().Where(e => getAttrValue(e, "column") == field.Attribute("as").Value).First();

                        if (field.Attribute("group1") != null)
                        {
                            field1.SetAttributeValue("group", field.Attribute("group1").Value);
                            field.Attributes("group1").Remove();
                        }
                        else
                        {
                            List<XElement> cols = field.Descendants("column").Where(e => getAttrValue(e, "table") != qalias).ToList();
                            foreach (XElement col in cols)
                            {


                                string exprName;

                                var exprString = col.ToString();
                                bool isReady = readyExprs.TryGetValue(exprString, out exprName);

                                if (!isReady)
                                {
                                    exprName = null;
                                }

                                if (!isReady)
                                {
                                    if (col.Attribute("thissrc") != null)
                                    {
                                        exprName = "expr" + col.Attribute("thissrc").Value;
                                    }
                                    else
                                    {
                                        exprName = "expr" + exprId.ToString();
                                        exprId++;
                                    }
                                    if (!expNames.Contains(exprName))
                                    {
                                        readyExprs.Add(exprString, exprName);
                                        XElement newCol = new XElement(col);
                                        newCol.SetAttributeValue("as", exprName);
                                        copyAttribute(col, newCol, "thissrc");
                                        qry.Element("select").Add(newCol);
                                        expNames.Add(exprName);
                                    }
                                }

                                col.RemoveAttributes();

                                col.SetAttributeValue("table", qalias);
                                col.SetAttributeValue("column", exprName);




                            }
                            field.Remove();
                            if (field.Attribute("group") == null)
                            {

                                if (field.Attribute("as").Value.EndsWith(cumulNextPfx) || field.Attribute("as").Value.EndsWith(cumulFirstPfx))
                                {
                                    field.SetAttributeValue("group", "1");
                                }
                                else
                                {
                                    //if (field.Ancestors("call").Where(e => getAttrValue(e, "function") == "partition by").FirstOrDefault() == null)
                                    //{
                                    if (field.Attribute("agg") != null)
                                    {
                                        field.SetAttributeValue("group", field.Attribute("agg").Value);
                                    }
                                    //}
                                    //else
                                    //{
                                    //    field.SetAttributeValue("group", "1");
                                    //}
                                }
                            }
                            field1.ReplaceWith(field);
                        }
                    }

                    qry.ReplaceWith(parentQry);
                    if (qry.Element("from").Elements().Where(e => getAttrValue(e, "additon") == "1").FirstOrDefault() != null)
                    {
                        qry.Element("select").Descendants().Attributes("group").Remove();
                    }
                    else
                    {
                        if (qry.Elements("from").Elements().Where(e => getAttrValue(e, "as") == bbAlias).FirstOrDefault() != null)
                        {
                            List<string> grNames = qry.Element("select").Descendants("column").Where(e => e.Attribute("table").Value == bbAlias && getAttrValue(e, "group") == "1").Select(e1 => getAttrValue(e1, "column")).Distinct().ToList();
                            foreach (XElement col in qry.Element("select").Descendants("column"))
                            {
                                if (col.AncestorsAndSelf().Where(e => getAttrValue(e, "group") != "").FirstOrDefault() == null)
                                {
                                    if (col.Attribute("table").Value == bbAlias)
                                    {
                                        if (!grNames.Contains(col.Attribute("column").Value))
                                        {
                                            col.SetAttributeValue("group", "sum");
                                        }

                                    }
                                    else
                                    {
                                        col.SetAttributeValue("group", "max");
                                    }
                                }
                            }
                        }
                        else
                        {
                            qry.Element("select").Descendants().Attributes("group").Remove();
                        }
                    }

                    foreach (XElement hcol in parentQry.Elements("having").Descendants("column")) //!!! Бельченко, 04.05.2016, чтобы работал отчет 33984-1. Может вызвать ошибки в других.
                    {
                        if (hcol.Attribute(TextConst.AName.As) != null)
                        {
                            hcol.SetAttributeValue(TextConst.EName.Column, hcol.Attribute(TextConst.AName.As).Value);
                        }
                    }

                    grSpecCols.AddRange(parentQry.Elements("having").Descendants("column"));
                    grSpecCols.AddRange(parentQry.Elements("group").Descendants("column"));
                    List<string> existingCols = qry.Elements("select").Elements().Select(e => getAttrValue(e, "as")).ToList();
                    List<string> newCols = new List<string>();
                    foreach (XElement col in grSpecCols.AsEnumerable().DescendantsAndSelf("column").ToList())
                    {
                        if (!newCols.Contains(col.Attribute("column").Value) && !existingCols.Contains(col.Attribute("column").Value))
                        {
                            XElement newCol = new XElement(col);
                            newCol.Attributes("group").Remove();
                            qry.Element("select").Add(newCol);
                            newCols.Add(col.Attribute("column").Value);
                        }
                        if (getAttrValue(col, "table") != qalias)
                        {
                            col.SetAttributeValue("table", qalias);
                        }
                    }


                    parentQry.Element("from").Add(qry);
                    qry.SetAttributeValue("as", qalias);
                    qry.SetAttributeValue("additon", "1");
                    //  qry.Descendants().Attributes(TextConst.EName.Group).Where(gr => gr.Value == "").Remove();
                }
            }

            
        }
        /* private static void processingWindowWithSiblingsSort(XElement el)// пока не используется
        {
            //оракл не позволяет наличие over и order siblings by в одном запросе
            List<XElement> queries = el.Descendants(EName.Query)
                .Where(q => q.AttrOrDefault(TextConst.AName.Materialize, string.Empty) != "2"
                            && q.Element(EName.Connect) != null
                            && q.AttrOrDefault(AName.Order, string.Empty) != string.Empty
                            && q.Elements(EName.Select).Elements().Any(EPredicate.IsCallOfWindowFunction)).ToList();
            foreach (XElement qry in queries) {
                XAttribute attr = qry.Attribute(AName.Order);
                string sorder = attr.Value;
                attr.Remove();
                List<XElement> treeEls = qry.Elements().Where(e => e.Name == EName.Connect || e.Name == EName.Start).ToList();
                treeEls.Remove();
                string qalias = "p";
                foreach (XElement col in treeEls.Descendants(AName.Column)) {
                    col.SetAttrValue(AName.Table, qalias);
                }
                XElement newQuery = CopyAndAddQueryLevel(qry, qalias);
                newQuery.SetAttrValue(AName.Order, sorder);
                newQuery.Add(treeEls);
            }
        }*/
        private static XElement makeColumnCall(XElement col, string qalias, bool hasGrNode, List<XElement> grSpecCols)
        {
            Contract.Assert(col != null);
            XElement newCol = null;
            bool isGrSpeCol = false;
            if (hasGrNode) {
                if (col.DescendantsAndSelf(EName.call).FirstOrDefault(e => gsetsFuncNames.Contains(e.AttrOrDefault(AName.function, string.Empty))) != null || gsetsSpecColsNames.Contains(col.AttrOrEmpty(AName.@as))) {
                    isGrSpeCol = true;
                }
            }
            if (!isGrSpeCol) {
                string alias = col.Attribute(AName.@as).Value;
                newCol = Factory.NewColumn(qalias, alias);
                newCol.CopyAttributes(col.Attributes(AName.group));
                newCol.CopyAttributes(col.Attributes(AName.into));
                copyAttributes(col, newCol, pivAttrsNames);
                if (newCol.Attribute(AName.group) == null) {
                    if (alias.EndsWith(cumulNextPfx) || alias.EndsWith(cumulFirstPfx)) {
                        newCol.SetAttrValue(AName.group, "1");
                    } else if (col.Attribute(AName.agg) != null) {
                        if (grFuncsNames.Contains(col.Attribute(AName.agg).Value)) {
                            newCol.SetAttrValue(AName.group, col.Attribute(AName.agg).Value);
                        }
                    }
                }
                newCol.Add(new XAttribute(AName.@as, alias)); 
                copyAttribute(col, newCol, TextConst.AName.Level);
                copyAttribute(col, newCol, TextConst.AName.TreeLevelColumn);
                copyAttribute(col, newCol, TextConst.AName.TreeOriginalColumn);
                copyAttribute(col, newCol, TextConst.AName.Removeable2);
                copyAttribute(col, newCol, TextConst.AName.Nvl);
                newCol.CopyAttributes(col.Attributes().Where(APredicate.IsColumnAttributeCanDub));
                newCol.CopyAttributes(col.Attributes().Where(APredicate.IsAdditionalAttribute));
                newCol.CopyAttributes(col.Attributes(AName.intern));
            } else {
                newCol = col;
                col.Remove();
                grSpecCols.Add(newCol);
            }
            return newCol;
        }
        internal static void AddQueryLevel(XElement query, string qalias, string[] fieldsToMoveUp = null)
        {
            var newQuery = CopyAndAddQueryLevel(query, qalias, fieldsToMoveUp);

           

            var newElemens = newQuery.Elements().ToArray();
            newElemens.Remove();
            query.RemoveAttributes();
            query.Elements().Remove();
            query.Add(newElemens);
            copyAttributes(newQuery, query);
        }

        private static XElement CopyAndAddQueryLevel(XElement query, string qalias, string[] fieldsToMoveUp = null)
        {
            if (fieldsToMoveUp == null)
            {
                fieldsToMoveUp = new string[] { };
            }
            var queryCopy = new XElement(query);
           

            
            var newQuery = new XElement(TextConst.EName.Query);
            Cmn.copyAttributes(queryCopy, newQuery);
            var newSelect = new XElement(TextConst.EName.Select);
            var newFrom = new XElement(TextConst.EName.From);

            //newSelect.Add(
            //    new XElement(TextConst.AName.Column, new XAttribute(TextConst.AName.Table, qalias), new XAttribute(TextConst.AName.Column, TextConst.AVColumn.All))
            //    );

            foreach (XElement col in queryCopy.Elements(TextConst.EName.Select).Elements().ToArray())
            {
                var colAlias = col.Attribute(TextConst.AName.As).Value;
                XElement newCol = null;
                if (fieldsToMoveUp.Contains(colAlias))
                {
                    newCol = col;
                    col.Remove();
                    foreach (XAttribute attr in col.Descendants(TextConst.AName.Column).Attributes(TextConst.AName.Table).ToArray())
                    {
                        attr.Value = TextConst.AVTable.Ths;
                    }
                }
                else
                {
                    newCol = makeColumnCall(col, qalias, false, null);
                    newCol.Attributes(TextConst.AName.Group).Remove();
                   
                }
                newSelect.Add(newCol);

            }

            queryCopy.RemoveAttributes();
            queryCopy.SetAttributeValue(TextConst.AName.As, qalias);

            newFrom.Add(queryCopy);

            newQuery.Add(newSelect);
            newQuery.Add(newFrom);


            var pars = query.Elements(TextConst.EName.Params).ToArray();// нужно при использовании в VGroupingUtils
            //pars.Remove();
            newQuery.Add(pars);

            var xcols = query.Elements(TextConst.EName.Columns).ToArray();// нужно при использовании в VGroupingUtils
            //xcols.Remove();
            newQuery.Add(xcols);

             xcols = query.Elements(TextConst.EName.ViewColumns).ToArray();// нужно при использовании в VGroupingUtils
           // xcols.Remove();
            newQuery.Add(xcols);

            //20171123 Вроде правильно так
            queryCopy.Elements(TextConst.EName.Params).Remove();
            queryCopy.Elements(TextConst.EName.Columns).Remove();
            queryCopy.Elements(TextConst.EName.ViewColumns).Remove();



            //query.ReplaceWith(newQuery);
            return newQuery;
        }

       
        private static void ClearDoubleGroup(XElement el)
        {

            foreach (XElement query in el.DescendantsAndSelf("query"))
            {
                List<string> groups = new List<string>();

                foreach (XElement col in query.Elements("select").Descendants().Where(e => getAttrValue(e, "group") == "1").ToList())
                {
                    string grName = "";
                    if (col.Ancestors("pivot").FirstOrDefault() == null && col.Ancestors(TextConst.EName.Query).FirstOrDefault() == query)
                    {
                        if (col.Name.LocalName == "column")
                        {
                            grName = getAttrValue(col, "table") + "." + getAttrValue(col, "column");
                        }
                        else
                        {
                            if (col.Attribute("thissrc") != null)
                            {
                                grName = col.Attribute("thissrc").Value;
                            }
                            else
                            {
                                grName = getAttrValue(col, "as");
                            }
                        }


                        if (!groups.Contains(grName))
                        {
                            if (grName != "")
                            {
                                groups.Add(grName);
                            }
                        }
                        else
                        {
                            col.Attributes("group").Remove();
                        }
                    }

                }


            }
        }


        internal static void processingMaterializedByHint(XElement query)
        {
            // return;
            // List<string> names = new List<string>();
            //  IEnumerable<XElement> matQrys =
            XElement qry = query.Descendants().FirstOrDefault(e => getAttrValue(e, "hint").Contains("materialize") && e.Parent.Name.LocalName != "with");

            XElement xwith = null;
            XElement pQry = null;
            int matIndex = 0;
            string matPfx = "mat";

            var readyInfo = new SortedList<string,SortedList<string,HashSet<string>>>();
            var readyAliaces = new SortedList<string, string>();
            while (qry != null)
            {
                var qname = getAttrValue(qry, TextConst.AName.Name);
                string readyAliace = null;
                  var colsInfo = new HashSet<string>();
                if (qname != "")// чтобы не матерализовывать одно и то же
                {
                  

                    foreach (var col in qry.Elements(TextConst.EName.Select).Elements())
                    {
                        var calias = getAttrValue(col, TextConst.AName.As);

                        colsInfo.Add(calias);
                    }

                    if (readyInfo.ContainsKey(qname))
                    {
                        foreach (var qliace in readyInfo[qname].Keys)
                        {
                            var difference = false;
                            if (colsInfo.Count != readyInfo[qname][qliace].Count)
                            {
                                difference = true;
                            }
                            foreach (var col in colsInfo)
                            {
                                if (!readyInfo[qname][qliace].Contains(col))
                                {
                                    difference = true;
                                    break;
                                }
                            }
                            if (!difference)
                            {
                                readyAliace = qliace;
                                break;
                            }
                        }

                    }
                    else
                    {
                        readyInfo.Add(qname, new SortedList<string, HashSet<string>>());
                    }
                }
                string alias = null;
                if (readyAliace != null)
                {
                    alias = readyAliace;
                }
                else
                {
                    matIndex++;
                    alias = matPfx + matIndex.ToString();
                    if (qname != "")
                    {
                        readyInfo[qname].Add(alias, colsInfo);
                    }
                }
                xwith = null;
                pQry = null;
                if (readyAliace == null)
                {
                    if (xwith == null)
                    {

                        pQry = qry.Parent;
                        bool found = false;

                        while (pQry != null && !found)
                        {
                            if (pQry.Name.LocalName == TextConst.EName.With)
                            {
                                found = true;
                                xwith = pQry;
                                pQry = null;
                            }

                            else
                            {
                                pQry = pQry.Parent;
                            }
                        }

                        if (!found)
                        {
                            pQry = qry.Parent;
                            while (!found)
                            {
                                if (pQry.Name.LocalName == TextConst.EName.Query && /*getAttrValue(pQry, "hint") != "materialize" && pQry.Attribute(TextConst.AName.Name) != null && */ pQry.Parent.Name.LocalName == TextConst.EName.Root)
                                {
                                    found = true;
                                }
                                else
                                {
                                    pQry = pQry.Parent;
                                }
                            }
                        }



                        // pQry = qry.Ancestors("query").Where(e1 => e1.Parent != null).Where(eh => getAttrValue(eh, "hint") != "materialize").Where(e => e.Attribute("name") != null || e.Parent.Name.LocalName == "root").First();
                        if (xwith == null)
                        {
                            if (pQry.Element("with") == null)
                            {
                                xwith = new XElement("with");
                                pQry.AddFirst(xwith);
                            }
                            else
                            {
                                xwith = pQry.Element("with");
                            }
                        }
                    }

                    if (!xwith.Elements().Any(e => e.Attribute("as").Value == qry.Attribute("as").Value))
                    {
                        XElement qry1 = new XElement(qry);
                        qry1.Attributes("join").Remove();
                        qry1.Elements("call").Remove();
                        qry1.SetAttributeValue(TextConst.AName.As, alias);
                        xwith.AddFirst(qry1);
                    }
                }
                //XElement qry2 = new XElement("table", new XAttribute("name", qry.Attribute("as").Value), new XAttribute("as", qry.Attribute("as").Value));
                XElement qry2 = new XElement("table", new XAttribute("name", alias), new XAttribute("as", qry.Attribute("as").Value));

                copyAttribute(qry, qry2, "join");
                qry2.Add(qry.Elements("call"));
                qry.ReplaceWith(qry2);
                // matQrys = query.Descendants().Where(e => getAttrValue(e, "hint") == "materialize" && e.Parent.Name.LocalName != "with").ToArray();
                qry = query.Descendants().FirstOrDefault(e => getAttrValue(e, "hint").Contains("materialize") && e.Parent.Name.LocalName != "with");
            }


        }


        internal static XElement compileQuery(string name, IEnumerable<VSXElement> scheme)
        {
            // чтобы скопировать
            //var buf = SchemeRoot.ToList();
            ////SchemeRoot.Clear();
            //SchemeRoot = scheme.Select(VSXElement.Get).ToList();
            XElement query = scheme.Elements("queries").Elements("query").FirstOrDefault(q => q.Attribute("name").Value == name);
            XElement qry = compileQuery(new XElement(query), true, null);
            //SchemeRoot = buf;


            return qry;
        }
        /*private static XElement compileReport(string name, IEnumerable<VSXElement> scheme, int useRepository, bool noPivot = false)
        {
            XElement element = XmlReports.Environment.Manager.GetScheme().Elements("reports").Elements("report").FirstOrDefault(q => q.Attribute("name").Value == name);
            return compileReport(element, useRepository, noPivot);
        }*/
        private static bool useRepositories = false;
        private static bool dontUseRepositories = false;
        internal static XElement compileReport(XElement element, int useRepository, bool noPivot = false, XElement pars = null)
        {
            Contract.Assert(element != null);
            Reset();
            if (element.AttrOrDefault("use-repository", false)) { 
                if (useRepository != 0) {
                    useRepositories = true;
                    dontUseRepositories = false;
                } else {
                    dontUseRepositories = true;
                }
            }
            pushpred = element.AttrOrDefault("pushpred", false);

            XElement ret = report(element);
            // processingMaterializedByHint(ret);

            if (!isProcessingPivots)
            {
                pivotMatQueriesNames = new List<string>();
                if (!noPivot)
                {
                    isProcessingPivots = true;
                    processingPivots(ret, pars);

                }
                ProcessingNestedOversAndSiblingsSort(ret);
                isProcessingPivots = false;
                if (!noPivot)
                {
                    if (doMatrializeByHint)
                    {
                        processingMaterializedByHint(ret);
                    }
                }

            }
            else
            {
                if (!isProcessingMatDummies)
                {
                    ProcessingNestedOversAndSiblingsSort(ret);
                }

            }

            if (pivotMatQueriesNames != null)
            {
                foreach (string redyMatName in pivotMatQueriesNames)
                {
                    if (redyMatName != "a")
                    {
                        var qry = ret.Elements(TextConst.EName.Query).FirstOrDefault(e => Cmn.GetAttrValue(e, TextConst.AName.Materialize) == TextConst.AVBool.True
                        && Cmn.GetAttrValue(e, TextConst.AName.Name) == redyMatName);
                        if (qry != null)
                        {
                            qry.SetAttributeValue(TextConst.AName.IsDone, TextConst.AVBool.True);
                        }
                    }
                }
            }

            useRepositories = false;
            ClearDoubleGroup(ret);
            return ret;
        }
        private static bool dontReset = false;
        private static void Reset()
        {
            hasChangeSources = false;
            icounter = 0;
            if (dontReset) return;
            //  pivotColumns = new SortedList<string, XElement>();
            //  readyPivots = new SortedList<string, VDataSet>();
        }


        


        internal static void ResetAfterError()
        {
              readyPivots = new SortedList<string, VDataSet>();
              pivotMatQueriesNames = null;
              isProcessingPivots = false;
              isProcessingMatDummies = false;
              ClearMatSetAndPivotQueriesList();
        }

        //static bool reportCompilation = false;


        private class ProcessingCollections
        {
            internal SortedList<string, XElement> matQueries = new SortedList<string, XElement>();
            internal SortedList<string, XElement> matQueriesDummies = new SortedList<string, XElement>();
            internal SortedList<string, XElement> storedQueries = new SortedList<string, XElement>();
            internal XElement matOrderForNames = new XElement("root");
        }

        private static ProcessingCollections _processingCollections = new ProcessingCollections();

        private static Stack<ProcessingCollections> _processingCollectionsStack = new Stack<ProcessingCollections>();

        //static SortedList<string, XElement> matQueries = new SortedList<string, XElement>();
        //static SortedList<string, XElement> matQueriesDummies = new SortedList<string, XElement>();
        //static SortedList<string, XElement> storedQueries = new SortedList<string, XElement>();
        //static XElement matOrderForNames = new XElement("root");


        private static void ResetProcessingCollections()
        {
            _processingCollections = new ProcessingCollections();
            _processingCollectionsStack.Clear();
        }


        private static void PushProcessingCollections()
        {
           
            _processingCollectionsStack.Push(_processingCollections);
            _processingCollections = new ProcessingCollections();
        }

        private static void PopProcessingCollections()
        {

            _processingCollections = _processingCollectionsStack.Pop();
           
        }



        private static void ClearMatSetAndPivotQueriesList()
        {
            _processingCollections.matQueries = new SortedList<string, XElement>();
            _processingCollections.matOrderForNames = new XElement("root");
            _processingCollections.matQueriesDummies = new SortedList<string, XElement>();
            _processingCollections.storedQueries = new SortedList<string, XElement>();
          
            if (!isProcessingPivots)
            {
                resetPivotQueriesList();
            }
        }

        private static XElement report(XElement element)
        {
            //reportCompilation = true;
            ClearMatSetAndPivotQueriesList();
            //matQueries = new SortedList<string, XElement>();
            //matOrderForNames = new XElement("root"); ;
            //matQueriesDummies = new SortedList<string, XElement>();
            //storedQueries = new SortedList<string, XElement>();
            XElement ret = new XElement("root");
            XElement reportParams = element.Element("params");
            XElement v = (XElement)applyParams(element, null, false).First();
            v = applyPart(v);
            XElement report1 = new XElement("cont");
            foreach (XElement el in v.Element("queries").Descendants("query").Where(e => e.Ancestors().FirstOrDefault(e1 => (new string[] { "from", "select", "where" }).Contains(e1.Name.LocalName)) == null))
            {

                //if (getAttrValue(el, "qv_title") == "Начисления и оплаты")
                //{
                //    string aa = "";
                //}
                XElement qry = reportQuery(el, element);
                //   clearReportQuery(qry, el);
                report1.Add(qry);
            }


            enableMatOrdering = false;
            foreach (XElement el in v.Element("queries").Descendants("query").Where(e => e.Ancestors().FirstOrDefault(e1 => (new string[] { "from", "select", "where" }).Contains(e1.Name.LocalName)) == null))
            {
                XElement qry = reportQuerySel(el, reportParams, element);
                //   clearReportQuery(qry, el);
                report1.Add(qry);
            }
            enableMatOrdering = true;


            AddMaterializedToResult(report1);
            /*  XElement matQueriesSet = new XElement("cont");
              foreach (XElement el in report1.Elements().Descendants("query").Where(e =>
                   getAttrValue(e, "materialize") == "1" &
                   e.Ancestors("query").Where(e1 => getAttrValue(e1, "materialize") == "2").Count() == 0
                  ))
              {
                  string name = getAttrValue(el, "name");
                  if (matQueriesSet.Elements().Where(e => getAttrValue(e, "name") == name).Count() == 0)
                  {
                      XElement qry = new XElement("qry");
                      string queryName = getAttrValue(el, "name");
                      if (queryName != "" && getAttrValue(el, "noname")!="1")
                      {
                          copyAttribute(el, qry, "name");
                          qry.Add(
                              copyElement(el.Element("withparams"))
                              );
                      }
                      else
                      {
                          qry.Add(new XElement(el));
                      }
                      matQueriesSet.AddFirst  (qry);
                  }
              }

              foreach (XElement el in matQueriesSet.Elements())
              {
                  XElement pars = el.Element("withparams");
                  XElement srcQuery;
                  if (el.Attribute("name") != null)
                  {
                      srcQuery = getQueryScheme(getAttrValue(el, "name"));
                  }
                  else
                  {
                      srcQuery = el.Element("query");
                  }
                  //srcQuery.Attributes("materialize").Remove();
                  XElement qry = mainMaterialized(srcQuery, pars,element);
                  ret.Add(qry);
              }
           
             */
            ret.Add(report1.Nodes());
            ret.Descendants("withparams").Remove();
            //  reportCompilation = false;
            return ret;
        }


        private static void AddMaterializedToResult(XElement result)
        {
            List<XElement> firstLevelMatDummies = result.Elements().Where(e => getAttrValue(e, "materialize") == "1").ToList();

            firstLevelMatDummies.Remove();
            foreach (XElement name in _processingCollections.matOrderForNames.Elements().OrderByDescending(e => Convert.ToInt32(e.Attribute("ord").Value)))
            {
                XElement qry = _processingCollections.matQueries[name.Attribute("name").Value];
                result.AddFirst(qry.Elements());
            }
        }

        private static string[] fixedColsNames = new string[] { "lvl", "rwn" };
        /* public static void clearReportQuery(XElement query, XElement queryCall)
         {
             ///return;
             bool wasDel = false;
             if (queryCall.Elements("columns").Descendants("column").Where(e=>getAttrValue(e,"table")==queryCall.Attribute("as").Value).Count()!=0 )
             {
                 foreach (XElement queryColumn in query.Elements("select").Elements().Where(e => 
         //e.Attribute("key").Value != "1" &
         getAttrValue(e, "fixed")!="1").ToArray())
                 {
                     if (!sysColNames.Contains(queryColumn.Attribute("as").Value) && !fixedColsNames.Contains(queryColumn.Attribute("as").Value))
                     {
                    
                         if (queryCall.Element("columns").Descendants("column").Where(e => e.Attribute("name").Value == queryColumn.Attribute("as").Value).Count() == 0)
                         {
                             if (query.Elements("order").Elements("column").Where(e => e.Attribute("column").Value == queryColumn.Attribute("as").Value).Count() == 0)
                             {
                                 queryColumn.Remove();
                                 wasDel = true;
                             }
                           
                          
                         }
                     }
                 }
             }
             if (wasDel)
             {
                 query.Descendants().Attributes("used").Remove();
                 setUsed(query,null);
                 copy6(query);
             }

         }*/

        /*private static XElement mainMaterialized(XElement element, XElement inParams, XElement rep)
        {
            XElement query = new XElement(EName.Query);
            copyAttribute(element, query, "name");
            query.SetAttrValue(TextConst.AName.Materialize, "1");
            query = getQuery(element, query, inParams);
            query = mainCommon(element, query, rep);
            return query;
        }*/
        private static XElement reportQuery(XElement element, XElement rep)
        {
            Contract.Assert(element != null);
            XElement query = new XElement(EName.query);
            query.CopyAttributes(element.Attributes(AName.name));
            query.SetAttrValue(TextConst.AName.Materialize, "1");
            query = getReportQuery(element, query, rep);
            query = mainCommon(element, query, rep);
            return query;
        }
        private static XElement reportQuerySel(XElement element, XElement inParams, XElement rep)
        {
            Contract.Assert(element != null);
            XElement query = new XElement(EName.query);
            query.CopyAttributes(element.Attributes(AName.name));
            query = getReportQuerySel(element, query, inParams);
            query = mainCommon(element, query, rep);
            return query;
        }
        private static XElement getReportQuery(XElement element, XElement qry, XElement rep)
        {
            Contract.Assert(element != null);
            string queryName = element.AttrOrDefault(AName.name, string.Empty);
            XElement pars = element.Element(EName.withparams);
            XElement queryCall = new XElement(element);
            XElement query;
            XElement queryScheme = null;
            if (queryCall.Element(EName.select) == null) {
                queryScheme = new XElement(getQueryScheme(queryName));
            } else {
                queryScheme = new XElement(element);
                //copyAttribute(queryCall.Element("from").Element("query"), queryScheme, "name");
                //queryScheme.SetAttributeValue("name", queryScheme.Attribute("as").Value);
            }
            pivotDummies(queryScheme, element, rep, pars);
            applyDimensions(queryScheme, element, rep, pars);
            ProcessQueryDlinkConditions(queryScheme);
            applyLinks(queryScheme, null);
            if (element.Parent.Name == EName.query) {
                query = new XElement(EName.query);
                query.Add(new XAttribute(AName.name, queryName));
                query.Add(new XAttribute("noname", "1"));
                query.CopyAttributes(queryScheme.Attributes(AName.order));
                XElement order = queryScheme.Element("order");
                if (order != null) {
                    query.Add(new XElement(order));
                }
                XElement select = new XElement(EName.select);
                select.Add(Factory.NewColumn(queryCall.Attribute(AName.@as).Value, TextConst.AVColumn.All));
                XElement col = Factory.NewColumn(element.Parent.Attribute(AName.@as).Value, TextConst.AVColumn.Sid);
                col.Add(new XAttribute(AName.@as, TextConst.AVColumn.SparentId));
                select.Add(col);
                query.Add(select);
                XElement from = new XElement(EName.from);
                XElement query_1 = new XElement(EName.query);
                query_1.CopyAttributes(queryScheme.Attributes(AName.name));
                query_1.Add(new XAttribute(AName.@as, queryCall.Attribute(AName.@as).Value));
                query_1.Add(queryCall.Elements().Where(EPredicate.IsNotQueryOrCall));
                from.Add(query_1);
                XElement query_2 = new XElement(EName.query);
                query_2.CopyAttributes(element.Parent.Attributes(AName.name));
                query_2.Add(new XAttribute(AName.@as, element.Parent.Attribute(AName.@as).Value));
                query_2.Add(new XAttribute(AName.join, TextConst.AVJoin.Inner));
                query_2.Add(new XAttribute(TextConst.AName.Materialize, "2"));
                query_2.Add(element.Parent.Elements().Where(EPredicate.IsNotQueryOrCall));
                query_2.Add(new XElement(queryCall.Element(EName.call)));
                from.Add(query_2);
                query.Add(from);
            } else {
                query = new XElement(queryScheme);
                query.Elements(EName.query).Remove();
                query.Elements(EName.call).Remove();
                // query = getQueryGroupLevel(query);
                // setGroupLevel(queryCall, query);
            }
            query = getQueryCommon(query, qry, pars, rep);
            //  query.SetAttributeValue("name", queryCall.Attribute("as").Value);
            return query;
        }


        private static XElement getReportQuerySel(XElement element, XElement qry, XElement reportParams)
        {
            string queryName = getAttrValue(element, "name");
            XElement pars = element.Element("withparams");
            XElement queryCall = element;


            XElement queryScheme = null;

            if (queryCall.Element("select") == null)
            {
                queryScheme = getQueryScheme(queryName);
            }
            else
            {
                queryScheme = new XElement(element);

                //copyAttribute(queryCall.Element("from").Element("query"), queryScheme, "name");
            }
            XElement query = new XElement(EName.query);
            query.Add(new XAttribute(AName.name, queryName + "-rep"));
            query.Add(new XAttribute("noname", "1"));
            XElement select = new XElement("select");
            query.Add(
                new XElement("select",
                       new XElement("column",
                               new XAttribute("table", queryCall.Attribute("as").Value),
                                new XAttribute("column", "sid")
                              ),
                        new XElement("column",
                                new XAttribute("table", queryCall.Attribute("as").Value),
                                new XAttribute("column", "sparentid")
                                ),
                        new XElement("column",
                                new XAttribute("table", queryCall.Attribute("as").Value),
                                new XAttribute("column", "*")
                                )
                        )

                );

            query.Add(new XElement("from"));
            if (queryCall.Element("select") == null)
            {
                query.Element("from").Add(
                        new XElement("query",
                                new XAttribute("name", queryName),
                                new XAttribute("as", queryCall.Attribute("as").Value),
                                new XAttribute("materialize", "2"),
                                copyElement(pars)
                                )
                    );
            }
            else
            {
                //  element.SetAttributeValue("name", queryCall.Attribute("as").Value);

                queryScheme.Elements("query").Remove();

                queryScheme.Elements("call").Remove();
                queryScheme.SetAttributeValue("materialize", "2");
                query.Element("from").Add(queryScheme);
            }
            query.Add(copyElement(queryScheme.Element("order")));
            query = getQueryCommon(query, qry, pars, null);
            //     query.SetAttributeValue("name", queryCall.Attribute("as").Value);
            return query;
        }

        private static XElement getQueryScheme(string queryName)
        {
            
            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), queryName))
            {
                var qs = (GetCashValue(MethodBase.GetCurrentMethod().ToString(), queryName) as Tuple<XElement, string>);
                if (qs.Item2 == XmlReports.Environment.GetGuid())
                {
                    return qs.Item1;
                }
                
            }

            var qry = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query").FirstOrDefault(q => q.Attribute("name").Value == queryName);
            if (qry == null)
            {
                qry = XmlReports.Environment.Manager.GetOldScheme()
                    .Elements("queries").Elements("query").FirstOrDefault(q => q.Attribute("name").Value == queryName);
            }

            if (qry != null)
            {
                var qs = new Tuple<XElement, string>(qry, XmlReports.Environment.GetGuid());

                AddCashValue(qs, MethodBase.GetCurrentMethod().ToString(), queryName);
            }
           
            return qry;
        }

        private static XElement main(XElement element, XElement rep)
        {
            XElement query = new XElement("query", (element.Attributes().Select(attr => new XAttribute(attr.Name.LocalName, attr.Value))));
            //if (Cmn.GetAttrValue(element, "name") == "36703-graf_d")
            //{
            //}
            query = getQuery(element, query, null);


            XElement ret = mainCommon(element, query, rep);
            return ret;
        }
        private static bool hasChangeSources = false;
        private static XElement getQuery(XElement element, XElement qry, XElement inPars)
        {
            addPath(element);
            XElement pars = inPars;
            if (pars == null) {
                pars = element.Element(EName.withparams);
            }
            XElement query = null;
            if (element.Attribute(AName.name) != null && getAttrValue(element, "noname") != "1") {
                string queryName = element.Attribute(AName.name).Value;
                if (hasChangeSources) {
                    var chsss = element.Ancestors().Elements(TextConst.EName.ChangeSources).ToList();  //!!! замедляет?
                    if (chsss.Count != 0) {
                        XElement chs = chsss.Elements().SearchByAttribute(AName.name, queryName);
                        if (chs != null) {
                            queryName = chs.Attribute(AName.call).Value;
                        }
                    }
                }
                query = new XElement(XmlReports.Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).SearchByAttribute(AName.name, queryName));
            } else {
                query = new XElement(element);
            }
            // applyExtensions(query);
            if (element.Attribute(TextConst.AName.Dimension) != null) {
                var xtable = query.Descendants(TextConst.EName.Table).FirstOrDefault();
                if (xtable != null) {
                    copyAttribute(element, xtable, TextConst.AName.Dimension);
                }
            }
            copyAttribute(element, query, "path");
            copyAttribute(element, query, "hint");
            copyAttribute(element, query, "noname");
            copyAttribute(element, query, "haskeys");
            copyAttributes(element, query, TextConst.ENameArray.ANewQueryAttributes);
            setGroupLevel(element, query);
            extendWhere(query, element);
            pivotDummies(query, element, null, pars);
            applyDimensions(query, element, null, pars);
            ProcessQueryDlinkConditions(query);
            applyLinks(query, null);
            //processingPivot(query);
            XElement ret = getQueryCommon(query, qry, pars, null);
            return ret;
        }
        private static void extendWhere(XElement query, XElement queryCall)
        {
            foreach (XElement extwhere in queryCall.Elements(EName.extendwhere).ToList())
            {
                XElement whereCont;
                XElement whereEl;
                if (extwhere.AttrOrDefault(AName.push, string.Empty) != "0") {
                    whereCont = query.Element(EName.push);
                } else {
                    whereCont = null;
                }
                if (whereCont == null) {
                    whereCont = query;
                }
                string tgt;
                XAttribute attr = extwhere.Attribute(AName.target);
                if (attr != null) {
                    tgt = attr.Value;
                    string tgts;
                    int pos = tgt.IndexOf('/');
                    if (pos < 0) {
                        tgts = string.Empty;
                    } else {
                        tgts = tgt.Substring(pos + 1);
                        tgt = tgt.Substring(0, pos);
                    }
                    XElement tagQry = whereCont.Element(EName.from).Elements().SearchByAttribute(AName.@as, tgt);
                    whereEl = new XElement(EName.extendwhere);
                    if (tgts != string.Empty) {
                          whereEl.Add(new XAttribute(AName.target, tgts));
                    }
                    tagQry.Add(whereEl);
                } else {
                    whereEl = whereCont.Element(EName.where);
                    if (whereEl == null) {
                        whereEl = new XElement(EName.where);
                        whereCont.Add(whereEl);
                    }
                }
                XElement extlinks = queryCall.Element(EName.extendlinks);
                if (extlinks != null) {
                    tgt = extlinks.Attribute(AName.target).Value;
                    XElement tagQry = whereCont.Element(EName.from).Elements().SearchByAttribute(AName.@as, tgt);
                    tagQry.Add(extlinks.Elements());
                }
                XElement call = Factory.NewCall(TextConst.AVFunction.And);
                whereEl.Elements().ChangeParent(call);
                whereEl.Add(call);
                extwhere.Elements().ChangeParent(call);
                extwhere.Remove();
            }
        }
        private static void setGroupLevel(XElement element, XElement query)
        {
            // основная обработка перенесена в precompile

            if (element.Attribute("grouplevel") != null || query.Attribute("grouplevel") != null)
            {
                query.Elements("select").Elements().Attributes("group").Remove();

                /* string groupLevel = element.Attribute("grouplevel").Value;
                 if (groupLevel == "no")
                 {
                     foreach (XElement el in query.Elements("select").Elements())
                     {
                         removeAttribute(el, "group");
                     }

                 }
                 else
                 {

                     IEnumerable<XElement> elements = query.Elements("select").Elements().Where(e => Cmn.IsNumeric(getAttrValue(e, "group"))).ToArray();
                     elements.Where(e => Convert.ToInt32(e.Attribute("group").Value) > Convert.ToInt32(groupLevel)).Remove();
                     foreach (XElement el in query.Elements("select").Elements().Where(e => Cmn.IsNumeric(getAttrValue(e, "group"))).ToArray())
                     {
                         el.SetAttributeValue("group", "1");
                     }
                 }*/
            }


        }

        private static XElement mainCommon(XElement element, XElement query, XElement rep)
        {

            XElement ret = loop(query, rep);

            ret = copyAddPath(ret);
            ret = copySelfKeys(ret);
            ret = copySourceKeys(ret);
            ret = copyAddTypes(ret);
            copyAddColInto(ret);
            ret = copyCheckKeys(ret);
            return ret;
        }


        private static XElement loop(XElement element, XElement rep)
        {

            XElement ret = copyThisColumns(element);
            /* ret = copy4(ret);
             ret = copy5(ret);*/
            ret = copySelfKeys(ret);
            ret = copySourceKeys(ret);
            ret = copyAddPath(ret);
            setUsed(ret, rep, null);
            ret = DeleteUnused(ret);
            ret = copy(ret);





            if (ret.Descendants("query").Count(e => !e.Elements("select").Any() && !e.Elements("query").Any()) > 0)
            {
                ret = loop(ret, rep);
            }
            else
            {
                ret = copyThisColumns(element);
                setUsed(ret, rep, null);
                ret = copy(ret);
                return ret;
            }
            return ret;
        }


        private static XElement createThisColumn(XElement src, XElement trg)
        {
            var col = new XElement(src);
            var el = trg;
            if (col.Attribute("as") != null)
            {
                col.SetAttributeValue("thissrc", col.Attribute("as").Value);
            }
            copyAttribute(el, col, "as");
            copyAttribute(el, col, "group");

            Cmn.CopyAttributeNotEmpty(el, col, "mp");

            copyAttribute(el, col, "dimname");
            copyAttribute(el, col, "title");
            copyAttribute(el, col, "cumulate");
            copyAttribute(el, col, "timeline");
            copyAttribute(el, col, "pivtarg");
            copyAttribute(el, col, "colset");
            copyAttribute(el, col, TextConst.AName.Color);
            copyAttribute(el, col, TextConst.AName.HAlign);
            copyAttribute(el, col, TextConst.AName.MergeKey);
            copyAttribute(el, col, TextConst.AName.ColumnEditable);
            copyAttribute(el, col, TextConst.AName.Sys);
            col.Add(el.Elements(TextConst.EName.Pivot));
            //copyAttribute(el, col, "window");
            if (el.Attribute(TextConst.AName.RowSelector) != null)
            {
                col.Elements().Remove();
            }
            return col;
        }

        internal static XElement copyThisColumns(XElement element, bool virtOnly = false)
        {
            IEnumerable<XElement> thisCols = element.DescendantsAndSelf(EName.column).Where(e => e.AttrOrDefault(AName.table, string.Empty) == TextConst.AVTable.Ths).ToArray();
            //   thisCols = thisCols.Where(e1 => !e1.Ancestors("withparams").Any()).ToArray();

            int i = 0;
            bool wasFlag = false;
            while (thisCols.Any())
            {
                foreach (XElement el in thisCols)
                {

                    if (el.Ancestors(TextConst.AName.Column).All(e => getAttrValue(e, TextConst.AName.Table) != TextConst.AVTable.Ths))// !!!! не понятно зачем это
                    {
                        XElement col = thisColumn(el);
                        if (getAttrValue(el, "column") == "col_i_check")
                        {

                        }

                        if (!virtOnly || getAttrValue(col, "virtual") == "1")
                        {

                            if (el.Attribute(TextConst.AName.RowSelector) != null || col.Descendants(TextConst.AName.Column).All(e => getAttrValue(e, TextConst.AName.Table) != TextConst.AVTable.Ths))
                            {
                                col = createThisColumn(col, el);
                                el.ReplaceWith(col);
                            }
                            

                        }
                        else
                        {
                            wasFlag = true;
                            el.SetAttributeValue("tprocflag", "1");
                        }
                    }
                }
                thisCols = element.DescendantsAndSelf("column").Where(e => e.AttrOrDefault(AName.table, string.Empty) == TextConst.AVTable.Ths && e.AttrOrDefault("tprocflag", string.Empty) != "1").ToArray();
                i++;

                if (i > 100)
                {
                    var col = thisCols.First();
                    var qry = col.Ancestors(TextConst.EName.Query).First();
                    throw new VCompilerException("Цикл в выражении с использованием this", qry, col);
                }
            }
            if (wasFlag)
            {
                element.Descendants().Attributes("tprocflag").Remove();
            }
            return element;
        }


        private static XElement thisColumn(XElement element)
        {

            if (element.Ancestors("withparams").FirstOrDefault() != null)
            {
                return element;
            }
            else
            {
                string colName = element.Attribute("column").Value;
                XElement querySub = element.Ancestors().FirstOrDefault(e => (new string[] { "query", "where","start", "select", "connect", "having", "dimension", "measures" }).Contains(e.Name.LocalName)); // в xlt был last 
                XElement query = querySub.Ancestors("query").FirstOrDefault(e => e.Element("select") != null);
                XElement sourceCol = query.Element(EName.select).Elements().SearchByAttribute(AName.@as, colName);
                if (sourceCol == null) {
                    sourceCol = query.Element(EName.select).Elements().SearchByAttribute(AName.column, colName);
                }
                // ВЕмцов - если this колонка не найдена - для отладки
                if (sourceCol == null) {
                    throw new VCompilerException("Поле this." + colName + "не найдено", element.Ancestors(TextConst.EName.Query).First(), element);
                }

                return sourceCol;
            }
        }

        private static XElement copyAddPath(XElement element)
        {

            foreach (XElement el in element.DescendantsAndSelf("query"))
            {
                addPath(el);
            }
            return element;
        }

        private static void addPath(XElement element)
        {
            if (element == null)
                return;
            string s = "";
            string s1 = ".";
            
            foreach (XElement el in element.AncestorsAndSelf("query").Where(e => e.Attribute("as") != null))
            {
                s = "/" + el.Attribute("as").Value + s;
            }
            
            s = s1 + s;
            element.SetAttrValue("path", s);

            XElement table = element.Elements("from").Elements("table").FirstOrDefault();
            if (table != null)
            {
                foreach (XElement col in element.Elements("select").Elements("column").Where(e => getAttrValue(e, "table") == table.Attribute("as").Value))
                {
                    col.SetAttributeValue("sourcetable", s);
                    col.SetAttributeValue("sourcecolumn", col.Attribute("column").Value);
                }
            }


        }



        private static XElement copy(XElement element)
        {
            IEnumerable<XElement> els = element.DescendantsAndSelf("query").Where(e =>
                e.Element("select") == null &&
                 e.Element("query") == null
                ).ToArray();

            foreach (XElement el in els)
            {
                el.ReplaceWith(queryFull(el));



            }
            return element;
        }



        private static XElement TryGetReadyMaretialized(XElement element)
        {

            XElement qry = null;
            if (getAttrValue(element, TextConst.AName.Materialize) == "2")
            {
                var name = getAttrValue(element, TextConst.AName.Name);
                if (name != "")
                {

                    if (_processingCollections.matQueriesDummies.ContainsKey(name))
                    {
                        qry = new XElement(_processingCollections.matQueriesDummies[name]);
                       // qry.SetAttributeValue(TextConst.AName.Materialize, "2");
                        qry.CopyAttributes(element.Attributes());
                        foreach (XElement el in element.Elements())
                        {
                            qry.Add(expression(el, null));
                        }
                    }
                }
            }
            
            return qry;
        }

        private static XElement queryFull(XElement element)
        {
            //if (getAttrValue(element, "name") == "ipr_fin_body_united")
            //{
            //    var ddd = "asdas";
            //}

            XElement ret1 = TryGetReadyMaretialized(element);

            if (ret1 != null)
            {
                //if (getAttrValue(element, TextConst.AName.Name) == "32274-razdel")
                //{
                //    return ret1;
                //    // 
                //}
                //else
                //{
                //    return ret1;
                //   // return ret1;
                //}
                return ret1;
            }

          

            var ret = new XElement("query");
            copyAttributes(element, ret);

            //if (getAttrValue(element, "name") == "ips_razdel_ip_stored")
            //{

            //}

            ret = getQuery(element, ret, null);
            if (element.Element("withparams") != null)
            {
                ret.Add(new XElement(element.Element("withparams")));
            }
            if (element.Element("extendwhere") != null)
            {
                ret.Add(element.Elements("extendwhere").Select(e => new XElement(e)));
            }
            if (element.Element(TextConst.EName.ExtendLinks) != null)
            {
                ret.Add(element.Elements(TextConst.EName.ExtendLinks).Select(e => new XElement(e)));
            }

            //if (element.Element(TextConst.EName.QubeContent) != null)
            //{
            //    if (ret.Element(TextConst.EName.QubeContent) == null)
            //    {
            //        ret.Add(element.Elements(TextConst.EName.QubeContent));
            //    }
            //}

           // linkedQueryCall.Add(link.Elements(TextConst.EName.QubeContent));
            foreach (XElement el in element.Elements())
            {
                ret.Add(expression(el, null));
            }
            //if (ret1 != null)
            //{
            //    if (getAttrValue(element, TextConst.AName.Name) == "32274-titul")
            //    {
            //    }
            //}
            return ret;
        }

        private static XElement copySelfKeys(XElement element)
        {
            //  string ss = "";
            //.Where(e => (new string[] { "const", "column", "call" }).Contains(e.Name.LocalName) & e.Attribute("as") != null)
            foreach (XElement el in element.DescendantsAndSelf().Elements("select").Where(e => e.Elements().Any(e1 => e1.Attribute("key") == null)))
            {
                bool hasKeys = false;
                if (el.Elements().Any(e => getAttrValue(e, "key") == "1"))
                {
                    hasKeys = true;
                }
                else
                {
                    foreach (XElement el1 in el.Elements())
                    {
                        if (selfKeys(el1))
                        {
                            hasKeys = true;
                        }

                    }
                }
                if (hasKeys)
                {
                    foreach (XElement col in el.Elements().Where(e1 => getAttrValue(e1, "key") != "1"))
                    {
                        col.SetAttributeValue("key", "0");
                    }
                }

            }


            return element;
        }

        private static bool selfKeys(XElement element)
        {
            if (getAttrValue(element, "group") == "1")
            {
                element.SetAttributeValue("key", "1");
                return true;
            }
            else
            {
                string tableAlias = getAttrValue(element, "table");
                if (!element.ElementsBeforeSelf().Any())
                {
                    if (element.Parent.Parent.Elements("from").Elements("table").Count(e => getAttrValue(e, "as") == (tableAlias)) == 1)
                    {

                        element.SetAttributeValue("key", "1");
                        return true;
                    }
                    else
                    {
                        if (element.Parent.Parent.Parent != null)
                        {
                            if (element.Parent.Parent.Parent.Name.LocalName == ("query"))
                            {
                                element.SetAttributeValue("key", "1");
                                return true;
                            }
                        }
                    }
                }

            }
            return false;

        }



        private static XElement copySourceKeys(XElement element)
        {

            //  int i = 1;
            /*<xsl:if test="name(..)='select' and not(../*[@key=1])">*/
            //  foreach (XElement el in element.DescendantsAndSelf("select").Elements().Where(e => e.Attribute("key") == null))
            //  {

            sourceKey(element);
            //  }
            return element;
        }


        private static void sourceKey(XElement element)
        {
            bool noKeys = false;
            foreach (XElement element1 in element.DescendantsAndSelf("select").Elements().Where(e => e.Attribute("key") == null))
            {

                //if (getAttrValue(element, "name") == "25499-dat")
                //{
                //    //   string aa = "";
                //}
                XElement keyInf = keyInfo(element1);
                if (keyInf != null)
                {
                    if (keyInf.Attribute("key").Value == ("1"))
                    {
                        copyAttribute(keyInf, element1, "key");
                        element1.SetAttributeValue("keypath", keyInf.Attribute("table").Value + "." + keyInf.Attribute("column").Value);
                        //element1.Add(new XAttribute("keypath", keyInf.Attribute("table").Value + "." + keyInf.Attribute("column").Value));
                    }
                    else
                    {

                        if (keyInf.Attribute("key").Value == ("0"))
                        {
                            copyAttribute(keyInf, element1, "key");
                        }
                    }
                }
                else
                {
                    noKeys = true;
                    break;
                }
            }
            if (noKeys)
            {
                // element.DescendantsAndSelf("select").Elements().Attributes("key").Remove();
                // Если подгрузились не все дочерние запросы не можем достоверно получить ключи
                // из за зачистки ключей удаляются неиспользуемые ключевые колонки, из-за этого проблемы с pivot

                element.DescendantsAndSelf("select").Where(e => getAttrValue(e.Parent, "haskeys") != "1").Elements().Attributes("key").Remove();


            }



        }

        private static XElement keyInfo(XElement element)
        {
            string table = getAttrValue(element, "table");
            string column = getAttrValue(element, "column");

            IEnumerable<XElement> colSourceNodes = null;


            if (element != null && (element.Parent.Parent.Element("dimension") == null || element.Parent.Name.LocalName != "select"))
            {
                XElement sourceQuery = element.Parent.Parent.Elements("from").Elements("query").FirstOrDefault(e => e.Attribute("as").Value == (table));



                if (sourceQuery != null & !IsSysColumnName(column))
                {
                    while (sourceQuery.Element("select") == null)
                    {
                        sourceQuery = sourceQuery.Element("query");
                        if (sourceQuery == null)
                        {
                            return null;
                        }
                    }
                }
                if (sourceQuery != null)
                {
                    colSourceNodes = sourceQuery.Elements("select");
                }
            }
            else if (element != null)
            {
                colSourceNodes = element.Parent.Parent.Elements().Where(EPredicate.IsDimensionOrMeasures);
            }

            if (colSourceNodes != null & !IsSysColumnName(column))
            {

                XElement sourceColumn = colSourceNodes.Elements().FirstOrDefault(e => e.Attribute("as").Value == (column));
                if (getAttrValue(sourceColumn, "key") != (""))
                {

                    return new XElement("key-info",
                        new XAttribute("key", getAttrValue(sourceColumn, "key")),
                        new XAttribute("table", getAttrValue(sourceColumn, "path")),
                         new XAttribute("column", getAttrValue(sourceColumn, "as"))
                        );
                }
                else
                {
                    return keyInfo(sourceColumn);
                }

            }
            else
            {
                return new XElement("key-info", new XAttribute("key", "0"));
            }

        }






        private static void setUsed(XElement element, XElement rep, XElement compiled)
        {


            foreach (XElement usedElement in element.DescendantsAndSelf().Where(e => e.Attribute("used") != null).ToArray())
            {
                usedElement.Attribute("used").Remove();
            }
            XElement query = element.DescendantsAndSelf("query").First();

            setQueryUsed(query, rep, compiled);

            /* foreach (XElement el in element.DescendantsAndSelf("query").Where(e => getAttrValue(e, "used") == "1" & e.Elements("select").Elements().Where(e1 => getAttrValue(e1, "used") == "1").Count()==0))
             {
                 XElement col = el.Elements("select").Elements().FirstOrDefault();
                 if (col != null)
                 {
                     setFieldUsed(col, el);
                 }
             }*/
            MarkUnused(element);

        }

        internal static void MarkUnused(XElement element)
        {
            foreach (XElement el in element.DescendantsAndSelf(EName.select).Elements()) {
                if (el.Attribute("used") == null) {
                    el.Add(new XAttribute("used", "0"));
                }
            }
            foreach (XElement el in element.DescendantsAndSelf(EName.query)) {
                if (el.Attribute("used") == null) {
                    el.Add(new XAttribute("used", "0"));
                }
            }
        }
        private static bool IsSysColumnName(string name)
        {
            return (name == TextConst.AVColumn.Sid) || (name == TextConst.AVColumn.SparentId);
        }
        private static void setFieldUsed(XElement column, XElement query, XElement compiled)
        {



            if (getAttrValue(column, "used") == "1")
            {
                return;
            }

            string masterName = getAttrValue(column, "master");
            if (masterName != "")
            {
                XElement masterField = query.Elements("select").Elements().FirstOrDefault(e => e.Attribute("as").Value == masterName);
                if (masterField != null)
                {
                    setFieldUsed(masterField, query, compiled);
                }
            }
            if (column.Name != EName.query) {
                column.SetAttrValue("used", "1");
                if (column.Name == EName.column) {
                    setColUsed(column, query, compiled);
                } else {
                    foreach (XElement childCol in getFieldColumns(column))
                    {
                        setColUsed(childCol, query, compiled);
                    }
                    foreach (XElement childQyery in column.DescendantsAndSelf("call").Elements("query"))
                    {
                        setFieldUsed(childQyery, query, compiled);
                    }
                }


                //!!! Добавил чтобы можно было использовать подзапросы в выражении для pivot
                // т.к. помечаются левые запросы ниже
                //foreach (XElement childQyery in column.Elements("pivot").Descendants("query"))
                //{
                //    setQueryUsed(childQyery);
                //}

                foreach (XElement childQyery in column.DescendantsAndSelf().Elements("pivot").Elements("query"))
                {
                    setQueryUsed(childQyery, null, compiled);
                }
            }
            else
            {
                column.Elements("select").Elements().First().SetAttributeValue("fixed", "1");

                List<string> qryNames = new List<string>();
                foreach (XElement qry in query.Elements("from").Elements())
                {
                    qryNames.Add(getAttrValue(qry, "as"));
                }

                List<string> qryNames2 = new List<string>();
                foreach (XElement qry in column.Elements("from").Elements())
                {
                    qryNames2.Add(getAttrValue(qry, "as"));
                }


                foreach (XElement childCol in getQueryColumns(column).Where(e => qryNames.Contains(e.Attribute("table").Value) && !qryNames2.Contains(e.Attribute("table").Value)))
                {
                    setColUsed(childCol, query, compiled);
                }

                setQueryUsed(column, null, compiled);
            }

            if (getAttrValue(column.Parent.Parent, "union") != "") // !!! Тоже самое что и в setColUsed, setColUsed не вызывается для key и fixed
            {
                if (!column.Parent.Parent.ElementsBeforeSelf().Any())
                {
                    int colIndex = column.ElementsBeforeSelf().Count();


                    //IEnumerable<XElement> sourceColumns = sourceQueries.Elements("select").Elements().Where(e => e.Attribute("as").Value == column.Attribute("column").Value);
                    foreach (XElement query1 in column.Parent.Parent.ElementsAfterSelf())
                    {
                        XElement sourceColumn1 = query1.Elements("select").Elements().ElementAt(colIndex);
                        setFieldUsed(sourceColumn1, query1, compiled);
                    }
                }
            }


        }

        private static void setColUsed(XElement column, XElement query, XElement compiled)
        {

            if (IsSysColumnName(column.Attribute("column").Value))
            {
                return;
            }

            IEnumerable<XElement> sourceQueries = query.Elements("from").Elements().Where(e1=>
                e1.Name.LocalName!=TextConst.EName.QubeContent 
                && e1.Name.LocalName!=TextConst.EName.ChangeSources)
                .Where(e =>  e.Attribute("as").Value == getAttrValue(column, "table")).ToArray();
            if (sourceQueries.Any())
            {
                XElement sourceQuery1 = sourceQueries.First();

                setQueryUsed(sourceQuery1, null, compiled);


                var sourceQuery2 = getFullSource(sourceQuery1, compiled);

                var sqList = new List<XElement>();
                sqList.Add(sourceQuery1);

                if (sourceQuery1 != sourceQuery2)
                {
                    sqList.Add(sourceQuery2);
                }
                foreach (XElement sourceQuery in sqList)
                {
                    if (sourceQuery.Name.LocalName == "query")
                    {

                        while (!sourceQueries.Elements("select").Any() & sourceQueries.Elements("query").Any())
                        {
                            sourceQueries = sourceQueries.Elements("query");
                        }
                        if (sourceQueries.Elements("select").Any())
                        {
                            XElement sourceColumn = sourceQueries.Elements("select").Elements().FirstOrDefault(e => getAttrValue(e, "as") == column.Attribute("column").Value);

                            if (sourceColumn == null)
                            {
                                if (sourceQueries.Count() == 1)
                                {
                                    if (getAttrValue(column, "virtual") == "1")
                                    {
                                        sourceColumn = new XElement("const", new XAttribute("as", column.Attribute("column").Value), new XElement("text", new XText("null")));
                                        sourceQueries.ToList()[0].Element("select").Add(sourceColumn);
                                    }
                                }
                            }
                            if (sourceColumn == null)
                            {
                                throw new VCompilerException("Колонка не найдена", query, column);
                            }
                            //ошибка может произойти , если есть запрос с хранилищем и колонками типа column="*" и были изменеия в подзапросе- зайти в описание этого запрос и сохранить чтобы обновился timestamp , иначе система не пересобирает его кеш
                            int colIndex = sourceColumn.ElementsBeforeSelf().Count();

                            //IEnumerable<XElement> sourceColumns = sourceQueries.Elements("select").Elements().Where(e => e.Attribute("as").Value == column.Attribute("column").Value);
                            foreach (XElement sq in sourceQueries)
                            {
                                XElement sourceColumn1 = sq.Elements("select").Elements().ElementAt(colIndex);
                                setFieldUsed(sourceColumn1, sq, compiled);
                            }
                        }
                    }
                }
            }
        }

        private static XElement getFullSource(XElement query, XElement compiled)
        {
            if (compiled != null)
            {
                if (getAttrValue(query, TextConst.AName.Materialize) != "")
                {
                    var query1 = compiled.Elements(TextConst.EName.Query).First(q => getAttrValue(q, TextConst.AName.Materialize) == "1" && q.Attribute(TextConst.AName.Name).Value == query.Attribute(TextConst.AName.Name).Value);
                    query = query1;
                }
            }
            return query;
        }
        internal static void setQueryUsed(XElement query, XElement rep, XElement compiled)
        {
            if (query == null) {
                return;
            }
            if (query.AttrOrEmpty(AName.used) == "1") {
                return;
            }
            //if (getAttrValue(query, "name") == "un-dogplat")
            //{
            //    //  string aa = "";
            //}
            foreach (XElement el in query.Elements(EName.query)) {
                setQueryUsed(el, null, compiled);
            }
            /*foreach (XElement el in query.Elements("from").Where(e => e.Elements().Where(e1=> getAttrValue(e1, "join") == "cross").Count()>0).Elements("query"))
            {
                setQueryUsed(el);
            }*/
            //  setQueryUsed(query.Elements("from").Elements().FirstOrDefault());
            foreach (XElement col in query.Elements(EName.select).Elements().Where(e => e.AttrOrEmpty(TextConst.AName.Removeable2) == "0" ||
               e.AttrOrEmpty(AName.removeable) == "0" || (e.AttrOrEmpty("fixed") == "1" && (e.AttrOrEmpty(AName.removeable) != "1" && rep == null))/* ||getAttrValue(e,"key")=="1"*/)) {
                setFieldUsed(col, query, compiled);
            }
            foreach (XElement col in query.Elements("order").Elements()) {
                XElement field = query.Elements(EName.select).Elements().SearchByAttribute(AName.@as, col.AttrOrEmpty(AName.column));
                if (field != null) {
                    setFieldUsed(field, query, compiled);
                }
            }
            query.SetAttrValue(AName.used, "1");
            if ((query.Attribute(AName.@as) == null && query.Ancestors(EName.query).FirstOrDefault() == null) || query.AttrOrEmpty(AName.materialize) == "1" || query.Parent.Name == EName.pivot) {
                IList<XElement> reportColumns = null;
                if (rep != null) {
                    reportColumns = rep.Elements(EName.queries).Descendants(EName.query).Where(q => q.AttrOrEmpty(AName.name) == query.AttrOrEmpty(AName.name)).Elements(EName.columns).Descendants(EName.column).ToList();
                } else {
                    reportColumns = Array.Empty<XElement>();
                }
                foreach (XElement col in query.Elements(EName.select).Elements())
                {//
                    bool noUseInReport = false;
                    string col_alias = col.AttrOrDefault(AName.@as, null);
                    if (col_alias == null) {
                        throw new VCompilerException("У выражения должен быть указан псевдоним", query, col);
                    }
                    if (!gsetsSpecColsNames.Contains(col_alias) && !IsSysColumnName(col_alias) && !fixedColsNames.Contains(col_alias)) {
                        if (reportColumns.Count > 0) {
                            if (reportColumns.SearchByAttribute(AName.name, col_alias) == null) {
                                noUseInReport = true;
                            }
                        }
                    }
                    if (!noUseInReport) {
                        setFieldUsed(col, query, compiled);
                    }
                }
            } else {
                foreach (XElement joinCol in query.Elements(EName.call).Descendants(EName.column).Where(e => e.Attribute(AName.table).Value == query.Attribute(AName.@as).Value)) {
                    if (query.Element(EName.select) != null) {
                        XElement col = query.Element(EName.select).Elements().First(e => e.Attribute(AName.@as).Value == joinCol.Attribute(AName.column).Value);
                        setFieldUsed(col, query, compiled);
                    }
                }
            }
            foreach (XElement joinCol in query.Elements(EName.call).Descendants(EName.column).Where(e => e.Attribute(AName.table).Value != query.Attribute(AName.@as).Value)) {
                XElement joinQuery = query.Parent.Elements().FirstOrDefault(e =>e.AttrOrEmpty(AName.@as) == joinCol.Attribute(AName.table).Value);
                if (joinQuery != null) {
                    setQueryUsed(joinQuery, null, compiled);
                    if (joinQuery.Element(EName.select) != null) {
                        XElement col = joinQuery.Element(EName.select).Elements().First(e => e.Attribute(AName.@as).Value == joinCol.Attribute(AName.column).Value);
                        // если ошибка, возможно требуется прописать измерение для query
                        setFieldUsed(col, joinQuery, compiled);
                    }
                }
            }
            foreach (XElement whereCol in query.Elements(EName.select).Elements().Elements(EName.pivot).Elements(EName.column)) {
                setColUsed(whereCol, query, compiled);
            }
            foreach (XElement whereCol in query.Elements(EName.select).Elements().Elements(EName.pivot).Elements(EName.call).Descendants(EName.column)) {
                setColUsed(whereCol, query, compiled);
            }
            XElement first = query.Elements(EName.from).Elements().FirstOrDefault();
            if (first != null) {
                if (Cmn.GetAttrValue(first, TextConst.AName.Name) == TextConst.AVTable.Dual) {
                    setQueryUsed(first, null, compiled);
                }
            }
            /*foreach (XElement whereCol in query.Elements("where").Descendants("column"))
            {
                setColUsed(whereCol, query);
            }


            foreach (XElement whereCol in query.Elements("connect").Descendants("column"))
            {
                setColUsed(whereCol, query);
            }
            foreach (XElement whereCol in query.Elements("start").Descendants("column"))
            {
                setColUsed(whereCol, query);
            }
             * */
            //убираю после добавления inner join по not null FK
            //вернул
            foreach (XElement whereCol in query.Elements(EName.connect).Descendants(EName.column)) {
                setColUsed(whereCol, query, compiled);
            }
            //foreach (XElement whereCol in query.Elements("start").Descendants("column"))
            //{
            //    setColUsed(whereCol, query);
            //}
            foreach (XElement whereCol in query.Elements(EName.having).Descendants(EName.column)) {
                setColUsed(whereCol, query, compiled);
            }
            //Для использование dlink pushpred в разделе where, нужно переписать эту часть 
            foreach (XElement whereCol in getQueryColumnsWhere(query)) {
                setColUsed(whereCol, query, compiled);
            }
            foreach (XElement whereCol in getQueryColumnsStart(query)) {
                setColUsed(whereCol, query, compiled);
            }
            foreach (XElement whereCol in query.Elements(EName.from).Elements(EName.query).Where(e => e.AttrOrEmpty(AName.join) == TextConst.AVJoin.Inner).Elements(EName.call).Descendants(EName.column)) {
                setColUsed(whereCol, query, compiled);
            }
            foreach (XElement whereCol in query.Elements(EName.where).Descendants(EName.call).Elements(EName.query).Where(e => !e.Ancestors(EName.query).First().IsAfter(query))) {
                setFieldUsed(whereCol, query, compiled);
            }
            foreach (XElement whereCol in query.Elements(EName.start).Descendants(EName.call).Elements(EName.query).Where(e => !e.Ancestors(EName.query).First().IsAfter(query))) {
                setFieldUsed(whereCol, query, compiled);
            }
            foreach (XElement whereCol in query.Elements(EName.group).Descendants(EName.column)) {
                setFieldUsed(whereCol, query, compiled);
            }
            foreach (XElement el in query.Elements(EName.from).Elements()) {
                if (el.AttrOrEmpty("fixed") == "1") {
                    setQueryUsed(el, null, compiled);
                }
            }
            foreach (XElement whereCol in query.Descendants(EName.extendwhere).Descendants(EName.call).Elements(EName.query).Where(e => !e.Ancestors(EName.query).First().IsAfter(query))) {
                setFieldUsed(whereCol, query, compiled);
            }
            var query1 = getFullSource(query, compiled);
            if (query1 != query)
            {
                setQueryUsed(query1, rep, compiled);
            }
        }
        internal static XElement DeleteUnused(XElement element)
               {
            //var qq = element.DescendantsAndSelf().Elements("from").Elements().Where(e => getAttrValue(e, "used") == ("0") && getAttrValue(e, "as") == "kod_dogplat_a_d").ToList();
            //if (qq.Any())
            //{
            //}
            element.DescendantsAndSelf().Elements("from").Elements().Where(e => getAttrValue(e, "used") == ("0")).Remove();

            element.DescendantsAndSelf().Elements("select").Elements().Where(e => getAttrValue(e, "used") == ("0")).Remove();

            return element;
        }

        private static XElement copyAddTypes(XElement element)
        {
            // element.DescendantsAndSelf("select").Elements() заменил на  getQueryColumnsSel(element)
            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("type") == null & getAttrValue(e, "used") != "0"))
            {
                addType(el);
            }

            foreach (XElement el in getQueryFieldsSel(element).Where(e => getAttrValue(e, "used") != "0"
                 && e.Attribute("title") == null
                 )
                )
            {
                addTitle(el);
                //string s = searchQueryAttrVal(el, "editor");
                //if (s != "")
                //{
                //    el.SetAttributeValue("editor", s);
                //}
            }

            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("visible") == null && getAttrValue(e, "used") != "0"))
            {

                string s = searchQueryAttrVal(el, "visible", false);
                if (s != "")
                {
                    el.SetAttributeValue("visible", s);
                }
            }

            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("class-title") == null && getAttrValue(e, "used") != "0"))
            {
                string classTitle = searchQueryAttrVal(el, "class-title");
                if (classTitle != "")
                {
                    el.SetAttributeValue("class-title", classTitle);
                }

            }

            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("pivot") == null && getAttrValue(e, "used") != "0"))
            {
                string s = searchQueryAttrVal(el, "pivot");
                if (s != "")
                {
                    el.SetAttributeValue("pivot", s);
                }

            }


            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("dimname") == null && getAttrValue(e, "used") != "0"))
            {
                string s = searchQueryAttrVal(el, "dimname");
                if (s != "")
                {
                    el.SetAttributeValue("dimname", s);
                }

            }






            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("editor") == null && getAttrValue(e, "used") != "0"))
            {
                string s = searchQueryAttrVal(el, "editor");
                if (s != "")
                {
                    el.SetAttributeValue("editor", s);
                }
            }

            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("agg") == null && getAttrValue(e, "used") != "0"))
            {
                string s = searchQueryAttrVal(el, "agg");
                if (s != "")
                {
                    el.SetAttributeValue("agg", s);
                }
            }

            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("format") == null && getAttrValue(e, "used") != "0"))
            {
                string s = searchQueryAttrVal(el, "format");
                if (s != "")
                {
                    el.SetAttributeValue("format", s);
                }

                s = searchQueryAttrVal(el,TextConst.AName.HAlign);
                if (s != "")
                {
                    el.SetAttributeValue(TextConst.AName.HAlign, s);
                }

            }

            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute(TextConst.AName.CMaster) == null && getAttrValue(e, "used") != "0"))
            {
                string s = searchQueryAttrVal(el, TextConst.AName.CMaster);
                if (s != "")
                {
                    el.SetAttributeValue(TextConst.AName.CMaster, s);
                }
            }

            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute(TextConst.AName.CMasterKey) == null && getAttrValue(e, "used") != "0"))
            {
                string s = searchQueryAttrVal(el, TextConst.AName.CMasterKey);
                if (s != "")
                {
                    el.SetAttributeValue(TextConst.AName.CMasterKey, s);
                }
            }

            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("sourcetable") == null && getAttrValue(e, "used") != "0"))
            {
                string s = searchQueryAttrVal(el, "sourcetable");
                if (s != "")
                {
                    el.SetAttributeValue("sourcetable", s);
                    el.SetAttributeValue("sourcecolumn", searchQueryAttrVal(el, "sourcecolumn"));
                }

            }
            foreach (XElement el in getQueryFieldsSel(element).Where(e => e.Attribute("reference") == null && getAttrValue(e, "used") != "0"))
            {
                string s = searchQueryAttrVal(el, "reference");
                if (s != "")
                {
                    el.SetAttributeValue("reference", s);
                    el.SetAttributeValue("refcol", searchQueryAttrVal(el, "refcol"));
                }
            }
            return element;
        }
        private static void addType(XElement element)
        {
            string s = searchQueryAttrVal(element, AName.type);
            if (!string.IsNullOrEmpty(s)) {
                element.Add(new XAttribute(AName.type, s));
            }
        }
        private static void addTitle(XElement element)
        {
            string s = searchQueryAttrVal(element, AName.title);
            if (!string.IsNullOrEmpty(s)) {
                element.SetAttributeValue(AName.title, s);
            }
        }
        //private static string searchQueryAttrVal(XElement element, string attrName, bool g = true)
        private static string searchQueryAttrVal(XElement element, XName attrName, bool g = true)
        {
            string ret = string.Empty;
            if (element == null) {
                return string.Empty;
            }
            XAttribute attr = element.Attribute(attrName);
            if (attr != null) {
                ret = attr.Value;
            } else {
                if (IsSysColumnName(element.AttrOrEmpty(AName.@as))) {
                    ret = string.Empty;
                } else {
                    if (element.Name == EName.query) {
                        ret = searchQueryAttrVal(element.Element(EName.select).Elements().First(), attrName);
                    } else {
                        if ((element.Name == EName.call) && g) {
                            string s_function = element.Attribute(AName.function).Value;
                            string val;
                            if (s_function == TextConst.AVFunction.If || s_function == "no dublers" || s_function == TextConst.AVFunction.Decode) {
                                val = searchQueryAttrVal(element.Elements().LastOrDefault(e => !(new String[] { TextConst.EName.Text, TextConst.EName.Pivot }).Contains(e.Name.LocalName)), attrName, g);
                                if (val != string.Empty) {
                                    ret = val;
                                }
                            } else if (attrName == AName.type && s_function == TextConst.AVFunction.Coalesce) {
                                val = searchQueryAttrVal(element.Elements().FirstOrDefault(e => e.Name != EName.text), attrName, g);
                                if (val != string.Empty) {
                                    ret = val;
                                }
                            } else {
                                foreach (XElement col in element.Descendants(EName.column)) {
                                    val = searchQueryAttrVal(col, attrName, g);
                                    if (val != string.Empty) {
                                        ret = val;
                                        break;
                                    }
                                }
                            }
                        } else {
                            attr = element.Attribute(AName.column);
                            if (attr == null) {
                                ret = string.Empty;
                            } else {
                                IEnumerable<XElement> colSourceNodes = null;
                                string column = attr.Value;
                                if (element.Parent.Parent.Element(EName.dimension) == null || element.Parent.Name != EName.select) {
                                    attr = element.Attribute(AName.table);
                                    if (attr == null) {
                                        ret = string.Empty;
                                    } else {
                                        string table = attr.Value;
                                        List<XElement> srcQueries = element.Ancestors(EName.query).First().Element(EName.from).Elements(EName.query).Where(e => e.Attribute(AName.@as).Value == table).ToList();
                                        if (srcQueries.Count != 0) {
                                            while (!srcQueries.Elements(EName.select).Any()) {
                                                srcQueries = srcQueries.Elements(EName.query).ToList();
                                            }
                                            colSourceNodes = srcQueries.Elements(EName.select);
                                        }
                                    }
                                } else {
                                    colSourceNodes = element.Parent.Parent.Elements().Where(EPredicate.IsDimensionOrMeasures);
                                }
                                if (colSourceNodes != null) {
                                    XElement srcColumn = colSourceNodes.Elements().FirstOrDefault(e => (e.AttrOrEmpty(AName.@as) == column) && (e.Attribute(attrName) != null || e.Name != EName.@const));
                                    ret = searchQueryAttrVal(srcColumn, attrName, g);
                                } else {
                                    ret = string.Empty;
                                }
                            }
                        }
                    }
                }
            }
            if (attrName == AName.title) {
                XElement pivot = element.Element(EName.pivot);
                if (pivot != null) {
                    ret += pivot.AttrOrDefault(AName.title, string.Empty);
                }
            }
            return ret;
        }
        /// <summary>
        /// Для всех query c materialize="1" в <paramref name="element"/> назначает колонкам в select аттрибут into (поле временной таблицы rr_temp)
        /// </summary>
        /// <param name="element"></param>
        private static void copyAddColInto(XElement element)
        {
            // SELECT SUBSTR(column_name, 1, 1), MAX(cut_num(column_name))
            // FROM   all_tab_cols
            // WHERE  owner = 'ASUSE' AND table_name = 'RR_TEMP'
            //    AND REGEXP_LIKE(column_name, '^[NDST]\d+$')
            // GROUP BY SUBSTR(column_name, 1, 1)
            Dictionary<string, int> col_count = new Dictionary<string, int>(4);
            foreach (XElement query in element.DescendantsAndSelf(EName.query)) {
                if (query.AttrOrEmpty(AName.materialize) == "1") {
                    foreach (XElement col in query.Elements(EName.select).Elements()) {
                        string data_type = col.AttrOrEmpty(AName.type);
                        string alias = col.AttrOrEmpty(AName.@as);
                        string name;
                        if (IsSysColumnName(alias)) {
                            name = alias;
                        } else {
                            string prefix = getTyprPr(data_type);
                            int pos;
                            if (col_count.TryGetValue(prefix, out pos)) {
                                pos = pos + 1;
                                col_count[prefix] = pos;
                            } else {
                                col_count.Add(prefix, 1);
                                pos = 1;
                            }
                            name = string.Intern(prefix + pos.ToString());
                        }
                        col.SetAttrValue(AName.into, name);
                    }
                    col_count.Clear();
                }
            }
        }
        private static string badTypePref = " ";
        internal static string getTyprPr(string data_type)
        {
            switch (data_type) {
                case TextConst.AVDataType.Number:
                case TextConst.AVDataType.Bool:
                    return "n";
                case TextConst.AVDataType.String:
                    return "s";
                case TextConst.AVDataType.Date:
                    return "d";
                case TextConst.AVDataType.Clob:
                    return "t";
                default:
                    return badTypePref;
                // Емцов. Чтобы вылетал эксепшн в момент компиляции и не строился заведомо инвалидный запрос
                //default:
                //    string msg = (string.IsNullOrEmpty(typ)) ? "Не определен тип для колонки rr_temp" : "Неизвестный тип колонки для rr_temp \"" + typ + "\"";
                //    throw new ArgumentException(msg);
            }
        }
        private static XElement copyCheckKeys(XElement element)
        {

            /*foreach (XElement el in element.DescendantsAndSelf("query").Elements("select").Where(
                e => e.Elements().Where(e1 => e1.Attribute("keypath") == null & e1.Attribute("key")==("1")).Count()==0
                ))
            {
                checkKeys(el);
            }*/
            return element;
        }
        private static XElement getQueryCommon(XElement element, XElement qry, XElement inPars, XElement rep)
        {
            XElement v = (XElement)applyParams(element, inPars, false).FirstOrDefault();
            XElement v1 = (XElement)applyPart(v);
            XElement ret = query(v1, qry, rep);

            return ret;
        }

        internal static IEnumerable<XNode> applyParams(XElement element, XElement inPars, bool isPart)
        {
            XElement formalParams = element.Element("params");
            XElement ret = new XElement("ret");
            if (formalParams != null)
            {
                ret.Add(applyParamsNext(element, inPars, formalParams, null, isPart));
            }
            else
            {
                ret.Add(new XElement("cont", new XElement(element)).Elements());
            }

            foreach (XElement el1 in ret.Descendants("call").Where(e => e.Attribute("function").Value == "nvlu"))
            {
                var undefined = el1.Elements().FirstOrDefault();
                if (undefined != null && undefined.Name == "undefined")
                {
                    undefined.ReplaceWith(new XElement("const", new XText("null")));
                }
                //foreach (XElement un in el1.Elements("undefined").ToArray().First())
                //{
                //    un.ReplaceWith(new XElement("const", new XText("null")));
                //}
            }

            foreach (XElement el1 in ret.Descendants("call").Where(e => e.Attribute("function").Value == "coalesceu"))
            {
                var el_val = el1.Elements().FirstOrDefault(e => e.Name != "undefined");
                if (el_val == null) continue;

                el1.Elements().Where(e => e != el_val).Remove();
            }

            foreach (XElement el1 in ret.Descendants("call").Where(e => e.Attribute("function").Value == "is undefined"))
            {

                foreach (XElement un in el1.Descendants().ToArray())
                {
                    if (un.Name.LocalName == "undefined")
                    {
                        un.ReplaceWith(new XElement("const", new XText("1")));
                    }
                    else if (un.Name.LocalName == "const")
                    {
                        un.RemoveNodes();
                        un.Add(new XText("0"));
                    }
                }

            }


            foreach (XElement el in ret.Descendants("call").Where(e => getAttrValue(e, "optional") == "1").ToArray())
            {




                if (el.Descendants("undefined").Any())
                {
                    el.Remove();
                }
            }

            return ret.Elements();
        }
        private static XElement applyPart(XElement element)
        {
            if (element.Descendants(EName.usepart).Any()) {
                XElement procElement = (XElement)copyApplyPart(element).First();
                XElement ret = applyPart(procElement);
                return ret;
            } else {
                return new XElement(element);
            }
        }
        private static IEnumerable<XNode> copyApplyPart(XElement element)
        {
            if (element.Name == EName.usepart) {
                return usepart(element);
            } else {
                XElement newEl = new XElement(element.Name);
                newEl.CopyAttributes(element.Attributes());
                foreach (XNode node in element.Nodes()) {
                    if (node.NodeType == XmlNodeType.Element) {
                        newEl.Add(copyApplyPart((XElement)node));
                    } else if (node.NodeType == XmlNodeType.Text) {
                        newEl.Add(new XText((XText)node));
                    }
                }
                XElement[] ret = new XElement[1];
                ret[0] = newEl;
                return ret;
            }
        }
        private static IEnumerable<XNode> usepart(XElement element)
        {
            string partId = element.AttrOrDefault(AName.part, null);
            XElement usepart = element;
            XElement part;
            if (!string.IsNullOrEmpty(partId)) {
                part = (XmlReports.Environment.Manager.GetScheme().Elements(EName.parts).Elements(EName.part).Where(el => el.Attribute(AName.id).Value == partId)).FirstOrDefault();
                if (part == null) {
                    var partEls = XmlReports.Environment.Manager.GetScheme().Descendants().Where(e => e.AttrOrDefault(AName.part_id, null) == partId);
                    if (!partEls.Any() && !preColmpiling) {
                        throw new VCompilerException("Не найдена часть " + partId, element.Ancestors().Where(e => e.Parent != null && e.Parent.Parent != null && e.Parent.Parent.Name == EName.root).FirstOrDefault(), element);
                    }
                    part = new XElement(EName.part);
                    part.Add(partEls);
                    part.Descendants().Attributes(AName.part_id).Remove();
                }
            } else {
                part = usepart.Element(EName.content).Elements().FirstOrDefault();
                if (part == null) {
                    return null;
                }
            }
            XElement factParams = new XElement("fact-params");
            foreach (XElement el in usepart.Elements()) {
                if (el.Name != EName.content) {
                    factParams.Add(applyPart(new XElement(el)));
                }
            }
            XElement v = new XElement("var");
            XElement firstParam = System.Xml.XPath.Extensions.XPathSelectElement(part, "params/param");
            if (firstParam != null && firstParam.AttrOrDefault(AName.multiple, false)) {
                int i = 0;
                foreach (XElement el in factParams.Elements().FirstOrDefault().Elements())
                {
                    string index = i.ToString();
                    XElement pars = new XElement(EName.@params);
                    pars.Add(new XElement(el));
                    foreach (XElement el1 in factParams.Elements().Where(el2 => el2.ElementsBeforeSelf().Any())) {
                        pars.Add(new XElement(el1));
                    }
                    v.Add(applyParamsToPart(part, pars, index));
                    i++;
                }
            } else {
                v.Add(applyParamsToPart(part, factParams, null));
            }
            XAttribute attr = element.Attribute(AName.@as);
            if (attr != null) {
                v.Elements().FirstOrDefault().SetAttributeValue(AName.@as, attr.Value);
            }
            attr = element.Attribute(AName.group);
            if (attr != null) {
                foreach (XElement el in v.Elements()) {
                    el.SetAttributeValue(AName.group, attr.Value);
                }
            }
            attr = element.Attribute(AName.title);
            if (attr != null) {
                v.Elements().FirstOrDefault().Add(new XAttribute(AName.title, attr.Value));
            }
            v = applyPart(v);
            List<XNode> ret = new List<XNode>();
            foreach (XElement el in v.Elements()) {
                IEnumerable<XNode> l = expression(el, null);
                if (l != null) {
                    ret.AddRange(l);
                }
            }
            return ret;
        }
        private static IEnumerable<XElement> applyParamsToPart(XElement element, XElement inParams, string index)
        {
            XElement ret = new XElement("ret");
            XElement formalParams = element.Element(EName.@params);
            if (formalParams != null) {
                foreach (XElement el in element.Elements()) {
                    if (el.Name != EName.@params) {
                        ret.Add(applyParamsNext(el, inParams, formalParams, index, true));
                    }
                }
            } else {
                foreach (XElement el in element.Elements()) {
                    ret.Add(new XElement(el));
                }
            }
            return ret.Elements();
        }
        private static bool preColmpiling = false;
        internal static IEnumerable<XNode> expression(XElement element, XElement parentColumns, string env = null)
        {

            XElement ret = new XElement("ret");
            if (preColmpiling)
            {
                ret.Add(new XElement(element));
            }
            else
            {
                switch (element.Name.LocalName)
                {
                    case "column":
                        ret.Add(column(element));
                        break;

                    case "call":
                        ret.Add(call(element, env));
                        break;

                    case "const":
                        ret.Add(eConst(element));
                        break;


                    case "usepart":
                        ret.Add(usepart(element));
                        break;

                    case "query":
                        ret.Add(source(element));
                        break;

                    case "group":
                        ret.Add(eGroup(element));
                        break;


                    case "union":
                        XElement v1 = applyPart(element);
                        ret.Add(sourceMain(v1));
                        break;
                    case "field":
                        ret.Add(element);
                        break;

                    case "extension":
                        ret.Add(element);
                        break;
                    case TextConst.EName.QubeContent:
                        ret.Add(element);
                        break;
                    //case TextConst.EName.DimQuery:
                    //    ret.Add(element);
                    //    break;
                }
            }

            return ret.Nodes();
        }
        private static IEnumerable<XNode> column(XElement element)
        {
            Contract.Assume(element != null);
            if (element.AttrOrDefault(AName.column, string.Empty) == TextConst.AVColumn.All) {
                return allColumns(element);
            } else {
                return new XElement[1] { singleColumn(element) };
            }
        }
        private static IEnumerable<XNode> allColumns(XElement element)
        {

            // запускает пересборку запроса из которого берутся колонки, 
            // в итоге запрос обрабатывается дважды
            //кешировать !?
            IEnumerable<XElement> colSourceNodes = null;
            string queryPname = element.Attribute("table").Value;

            var usedAliaces = new List<string>();
            foreach (var sc in element.Parent.Elements())
            {
                var alias = getAttrValue(sc, TextConst.AName.As);
                if (alias == "")
                {
                    alias = getAttrValue(sc, TextConst.AName.Column);
                }
                if (alias != "*")
                {
                    usedAliaces.Add(alias);
                }
            }
            if (element.Parent.Parent.Element("dimension") == null || element.Parent.Name.LocalName != "select")
            {





                XElement query = element.Parent.Parent.Element("from").Elements("query").FirstOrDefault(el => el.Attribute("as").Value == (queryPname));
                addPath(query);

                XElement pars = query.Element("withparams");
                XElement colSource = getColSource(query);
                colSource = (XElement)applyParams(colSource, pars, false).FirstOrDefault();
                colSource = applyPart(colSource);
                copyAttribute(query, colSource, "path");
                //processingPivot(colSource);

                colSourceNodes = colSource.Elements("select");
            }
            else
            {
                colSourceNodes = element.Parent.Parent.Elements().Where(EPredicate.IsDimensionOrMeasures);
            }

            XElement columns = new XElement("select");

            XElement columns1 = new XElement("select");
            foreach (XElement col in colSourceNodes.Elements())
            {
                var alias = getAttrValue(col, TextConst.AName.As);
                if (alias == "")
                {
                    alias = getAttrValue(col, TextConst.AName.Column);
                }
                if (!usedAliaces.Contains(alias))
                {
                    columns.Add(expression(col, null));
                }
                else
                {
                }
            }

        
            XElement newCol = null;
            foreach (XElement col in columns.Elements())
            {
                if (element.Parent.Elements().All(e => getAttrValue(e, "as") != getAttrValue(col, "as")))
                {
                     newCol = new XElement("column",
                        new XAttribute("table", queryPname), new XAttribute("column", columnPnameVal(col)), columnPname(col));
                    if (getAttrValue(element, "group") == "inherit")
                    {
                        copyAttribute(col, newCol, "group");
                    }

                    if (getAttrValue(element.Parent.Parent, "haskeys") == "")
                    {
                        copyAttribute(col, newCol, "key");
                        copyAttribute(col, newCol, "fixed");
                    }
                    copyAttribute(col, newCol, "nvl");
                    newCol.CopyAttributes(col.Attributes().Where(APredicate.IsAdditionalAttribute));
                    columns1.Add(newCol);
                }
            }
           
            return columns1.Elements();
        }


        private static IEnumerable<XAttribute> columnPname(XElement element)
        {
            XElement ret = new XElement("ret");
            if (element.Parent.Name.LocalName == ("select") || element.Attribute("as") != null)
            {
                ret.Add(new XAttribute("as", columnPnameVal(element)));
            }
            copyAttribute(element, ret, "type");
            copyAttribute(element, ret, "title");
            copyAttribute(element, ret, "class-title");
            copyAttribute(element, ret, "editor");
            copyAttribute(element, ret, "master");
            copyAttribute(element, ret, "agg");
            copyAttribute(element, ret, "format");
            copyAttribute(element, ret, TextConst.AName.CMaster);
            copyAttribute(element, ret, TextConst.AName.CMasterKey);
            copyAttribute(element, ret, "removeable");
            copyAttribute(element, ret, "pivot");
            copyAttribute(element, ret, "dimname");
            return ret.Attributes();
        }
        private static string columnPnameVal(XElement element)
        {
            XAttribute attr = element.Attribute(AName.@as);
            if (attr != null) {
                return attr.Value;
            } else {
                return element.Attribute(AName.column).Value;
            }
        }
        private static XElement getColSource(XElement element)
        {

            XElement sourceQuery = null;
            if (element.Elements().Any(e => (new string[] { "select", "union", "query" }).Contains(e.Name.LocalName)))
            //if ((new string[] { "select", "union", "query" }).Contains(element.Name.LocalName))
            {
                sourceQuery = element;
            }
            else
            {
                string queryName = element.Attribute("name").Value;
                sourceQuery = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query").FirstOrDefault(q => q.Attribute("name").Value == (queryName));
                if(sourceQuery == null)
                {
                    sourceQuery = XmlReports.Environment.Manager.GetOldScheme().Elements("queries").Elements("query").FirstOrDefault(q => q.Attribute("name").Value == (queryName)); 
                }



            }
            XElement contentQuery = sourceQuery.Elements().FirstOrDefault(e => (new string[] { "union", "query" }).Contains(e.Name.LocalName));
            if (contentQuery != null)
            {
                return getColSource(contentQuery);
            }
            else
            {
                bool matExists = false;
                if (getAttrValue(element, TextConst.AName.Materialize) == "2")
                {
                    var name = getAttrValue(element, TextConst.AName.Name);
                    if (name != "")
                    {
                        if (_processingCollections.matQueries.ContainsKey(name))
                        {
                            sourceQuery = _processingCollections.matQueries[name].Elements().First();
                            matExists = true;
                        }
                    }
                   
                
                }
                if (!matExists)
                {
                    XElement ret = new XElement(sourceQuery);
                    setGroupLevel(element, sourceQuery);
                    sourceQuery = new XElement(
                        sourceQuery

                        );
                    applyDimensions(sourceQuery, null, null, null,true);
                   // applyQube(sourceQuery, null, null, null);
                }
                //applyQube(sourceQuery, null, null, element); // 2 Бельченко 20161031 Это новое, не проверенное , используется только для демонстрации поиска, можно убрать если что
                return
                    sourceQuery
                   ;
            }

        }
        private static XElement singleColumn(XElement element)
        {
            XElement column = new XElement(EName.column);
            column.CopyAttributes(element.Attributes());
            foreach (XElement pivot in element.Elements(EName.pivot)) {
                XElement pivot1 = new XElement(pivot);
                foreach (XElement pqry in pivot1.Elements(EName.query).ToList())
                {
                    pqry.ReplaceWith(expression(pqry, null));
                }
                column.Add(pivot1);
            }
            //   column.Add(element.Elements("pivot").Select(e => new XElement(e)));
            setAttributes(column, columnPname(element));
            XAttribute attr = element.Attribute(AName.group);
            if (attr != null) {
                column.SetAttributeValue(AName.group, attr.Value);
            }
            return column;
        }
        //private static string[] CallAttributes = new string[] { "function", "type", "title", "class-title", "as", "joinexp", "mp", "agg", "format", "nvl", "nullif", "key", "qlikview", "qv_split", TextConst.AName.CMaster, TextConst.AName.CMasterKey, TextConst.AName.Colset, TextConst.AName.Color, TextConst.AName.HAlign, TextConst.AName.MergeKey };
        private static XElement call(XElement element, string env = null)
        {
            XElement call = new XElement(EName.call);
            call.CopyAttributes(element.Attributes().Where(APredicate.IsCallAttributes));
            call.CopyAttributes(element.Attributes(AName.group));
            eFunction(element, call, env);
            return call;
        }
        internal static void eFunction(XElement element, XElement ret, string env = null)
        {
            //string functionName;
            //#if DEBUG
            //functionName = element.Attribute(AName.Function).Value;
            //Stopwatch sw = new Stopwatch();
            //sw.Start();
            //#endif
            XElement function;
            if (element.Element(EName.text) != null) {
                function = new XElement(element);
            } else {
                string functionName = element.Attribute(AName.function).Value;
                if (env != null) {   // временное решение
                    functionName = env + ":" + functionName;
                }
                XElement functionScheme = XmlReports.Environment.Manager.GetScheme().Elements(EName.functions).Elements(EName.function).SearchByAttribute(AName.name, functionName);
                if (functionScheme == null) {
                    throw new VCompilerException("Не найдена функция \"" + functionName + "\"", element, null);
                }
                function = new XElement(functionScheme);
                XAttribute attr = element.Attribute(AName.pth);
                if (attr != null) {
                    ret.Add(new XAttribute(AName.pth, attr.Value));
                } else {
                    ret.Add(new XAttribute(AName.pth, function.AttrOrDefault(AName.pth, string.Empty)));
                }
                ret.CopyAttributes(element.Attributes(AName.optional));
                ret.CopyAttributes(element.Attributes(AName.pivot));
                ret.CopyAttributes(element.Attributes(AName.dimname));
                ret.CopyAttributes(element.Attributes().Where(APredicate.IsAdditionalAttribute));
                foreach (XElement pivot in element.Elements(EName.pivot)) {
                    XElement pivot1 = new XElement(pivot);
                    foreach (XElement pqry in pivot1.Elements(EName.query).ToList()) {
                        pqry.ReplaceWith(expression(pqry, null));
                    }
                    ret.Add(pivot1);
                }
                string data_type = function.AttrOrDefault(AName.type, TextConst.AVDataType.Variant);
                if (element.Attribute(AName.type) == null && data_type != TextConst.AVDataType.Variant) {
                    ret.SetAttrValue(AName.type, data_type);
                }
                int i = 0;
                XElement prevVal = null;
                IList<XElement> pars = element.Elements().Where(e => (e.Name != EName.undefined) && (e.Name != EName.pivot)).ToList();
                IList<XElement> funcElements = function.Elements().ToList();
                if (funcElements.Count > 0) {
                    foreach (XElement el in pars){
                        // Емцов - оптимизировал
                        //XElement val_old = function.Elements().Where(e => e.ElementsBeforeSelf().Count() == i).Elements(EName.Val).FirstOrDefault();
                        XElement val = (funcElements.IsValidIndex(i)) ? funcElements[i].Elements(EName.val).FirstOrDefault() : null;
                        if (val == null) {
                            XElement parSceme = functionScheme.Elements().Elements(EName.val).Last();
                            prevVal.Parent.AddAfterSelf(new XElement(parSceme.Parent));
                            // Емцов - оптимизировал
                            //val_old = function.Elements().Where(e => e.ElementsBeforeSelf().Count() == i).Elements(EName.Val).First();
                            funcElements = function.Elements().ToList();
                            val = funcElements[i].Elements(EName.val).First();
                        }
                        //if(val != val_old) throw new Exception();
                        if (el.Attribute(AName.optional) != null) {
                            val.Parent.AddFirst(new XText("{"));
                            val.Parent.Add(new XText("}"));
                        }
                        IEnumerable<XNode> par = expression(el, null, env);
                        foreach (XElement val1 in val.Parent.Elements(EName.val)) {
                            val1.AddAfterSelf(par);
                        }
                        prevVal = val;
                        i++;
                    }
                }
                function.Elements().Where(e => e.ElementsBeforeSelf().Count() >= pars.Count).Remove();
                function.Descendants(EName.val).Remove();
                foreach (XElement el in function.Elements().ToList()) {
                    el.ReplaceWith(el.Nodes());
                }
                StringBuilder sb = new StringBuilder(256);
                foreach (XNode txt in function.DescendantNodes().ToList()) {
                    if (txt.NodeType == XmlNodeType.Text && txt.Parent.Name != EName.text) {
                        //
                        string text = ((XText)txt).Value;
                        int len = text.Length;
                        sb.EnsureCapacity(len + 2);
                        sb.Append(' ');
                        sb.Append(text);
                        sb.Append(' ');
                        sb.Replace('\t', ' ');
                        sb.Replace('\n', ' ');
                        sb.Replace('\r', ' ');
                        loopReplace(sb, "  ", " ");
                        text = sb.ToString();
                        sb.Clear();
                        //
                        string tеxt_type;
                        if (txt.Parent.Name == EName.@const) {
                            tеxt_type = "const";
                        } else {
                            tеxt_type = "func";
                        }
                        XElement t = new XElement(EName.text);
                        t.Add(new XAttribute(AName.txtype, tеxt_type));
                        t.Add(new XText(text));
                        txt.ReplaceWith(t);
                    }
                }
            }
            foreach (var e in function.Elements("nnull").ToList()) e.ReplaceWith(new XText(TextConst.NullConsts.NNULL));
            foreach (var e in function.Elements("snull").ToList()) e.ReplaceWith(new XText(TextConst.NullConsts.SNULL));
            foreach (var e in function.Elements("dnull").ToList()) e.ReplaceWith(new XText(TextConst.NullConsts.DNULL));
            ret.Add(function.Nodes());
            //#if DEBUG
            //sw.Stop();
            //Debug.WriteLine("Compiler.eFunction() function=\"" + functionName + "\": " + sw.ElapsedTicks + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
            //#endif
        }
        private static XElement eConst(XElement element)
        {
            XElement ret = new XElement(EName.@const);
            ret.CopyAttributes(element.Attributes());
            if (element.Element(EName.text) == null) {
                ret.Add(new XElement(EName.text, new XText(element.Value)));
            } else {
                copyContent(element, ret);
            }
            return ret;
        }
        private static XAttribute eGroup(XElement element)
        {
            XAttribute attr = element.Attribute(AName.group);
            if (attr != null) {
                return new XAttribute(attr);
            } else {
                return null;
            }
        }
        private static XElement sourceMain(XElement element)
        {
            XElement ret = new XElement("query");
            copyAttribute(element, ret, "materialize");
            copyAttribute(element, ret, "grouplevel");
            copyAttribute(element, ret, "hint");
            copyAttribute(element, ret, "noname");
            copyAttribute(element, ret, "haskeys");
            copyAttribute(element, ret, "nvl");
            copyAttribute(element, ret, "nullif");
            copyAttribute(element, ret, "mp");
            copyAttribute(element, ret, "agg");
            copyAttribute(element, ret, "format");
            copyAttribute(element, ret, TextConst.AName.CMaster);
            copyAttribute(element, ret, TextConst.AName.CMasterKey);
            copyAttribute(element, ret, "name");
            copyAttributes(element, ret, TextConst.ENameArray.ANewQueryAttributes);


            ret.Add(tableAs(element));
            copyAttribute(element, ret, "join");
            if (element.Attribute("union") != null)
            {
                ret.Add(new XAttribute("union", "1"));
            }
            else
            {

                if (element.Parent != null)
                {
                    if (element.Parent.Name.LocalName == "union")
                    {
                        if (getAttrValue(element.Parent, "all") == "0")
                        {
                            ret.Add(new XAttribute("union", "2"));
                        }
                        else
                        {
                            ret.Add(new XAttribute("union", "1"));
                        }
                    }
                }
            }

            if (element.Element("withparams") != null)
            {
                ret.Add(new XElement(element.Element("withparams")));
            }

            if (element.Element("extendwhere") != null)
            {
                ret.Add(element.Elements("extendwhere").Select(e => new XElement(e)));
            }

            if (element.Element(TextConst.EName.ExtendLinks) != null)
            {
                ret.Add(element.Elements(TextConst.EName.ExtendLinks).Select(e => new XElement(e)));
            }


            if (element.Element("select") != null)
            {
                ret = query(element, ret, null);
            }
            else
            {
                if (element.Element("where") != null)
                {
                    ret.Add(new XElement(element.Element("where")));
                }
            }

            foreach (XElement el in element.Elements())
            {
                ret.Add(expression(el, null));
            }



            return ret;

        }
        private static XAttribute tableAs(XElement element)
        {
            XAttribute ret = null;
            XElement aliasSource = element.AncestorsAndSelf().FirstOrDefault(e => e.Attribute(AName.@as) != null);
            if (aliasSource != null) {
                ret = new XAttribute(aliasSource.Attribute(AName.@as));
            }
            return ret;
        }
        #region copyAttributes(), copyAttribute()
        internal static void copyAttributes(XElement src, XElement tag)
        {
            Contract.Assert(src != null);
            Contract.Assert(tag != null);
            //var aa = tag.Attributes().Select(at2 => at2.Name.LocalName).ToArray();
            //tag.Add(src.Attributes().Where(at1 => !aa.Contains(at1.Name.LocalName)).Select(at => new XAttribute(at.Name.LocalName, at.Value)));
            foreach (XAttribute attr in src.Attributes()) {
                XName name = attr.Name;
                if (tag.Attribute(name) == null) {
                    tag.Add(new XAttribute(name, attr.Value));
                }
            }
        }
        internal static void copyAttributes(XElement src, XElement tag, string[] attrNames)
        {
            Contract.Assert(src != null);
            Contract.Assert(tag != null);
            Contract.Assert(attrNames != null);
            //var attrs = (src.Attributes().Where(at => attrNames.Contains(at.Name.LocalName)).Select(at => new XAttribute(at.Name.LocalName, at.Value))).ToArray();
            //var aa = attrs.Select(at1 => at1.Name).ToArray();
            //tag.Attributes().Where(at => aa.Contains(at.Name)).Remove();
            //tag.Add(attrs);
            for (int index = 0; index < attrNames.Length; index++) {
                XName attr_name = attrNames[index];
                XAttribute src_attr = src.Attribute(attr_name);
                if (src_attr != null) {
                    XAttribute dest_attr = tag.Attribute(attr_name);
                    if (dest_attr != null) {
                        dest_attr.Value = src_attr.Value;
                    } else {
                        tag.Add(new XAttribute(attr_name, src_attr.Value));
                    }
                }
            }
        }
        private static void copyAttribute(XElement src, XElement tag, string name)
        {
            if (src != null) {
                XAttribute attr = src.Attribute(name);
                if (attr != null) {
                    tag.SetAttributeValue(attr.Name, attr.Value);
                }
            }
        }
        internal static XAttribute copyAttribute(XElement src, string name)
        {
            XAttribute attr = src.Attribute(name);
            if (attr != null) {
                return new XAttribute(attr.Name, attr.Value);
            } else {
                return null;
            }
        }
        #endregion
        internal static void copyContent(XElement src, XElement tag)
        {
            Contract.Assert(src != null);
            Contract.Assert(tag != null);
            foreach (XNode node in src.Nodes()) {
                if (node.NodeType == XmlNodeType.Element) {
                    tag.Add(new XElement((XElement)node));
                } else if (node.NodeType == XmlNodeType.Text) {
                    tag.Add(new XText(((XText)node).Value));
                }
            }
        }
        private static XElement source(XElement element)
        {
            XName name = element.Name;
            XElement el = null;
            if (name == EName.query) {
                el = sourceMain(element);
            } else if (name == EName.table) {
                el = table(element);
            } else if (name == EName.usepart) {
                //return usepart(element).Cast<XElement>().FirstOrDefault();
                foreach (XNode node in usepart(element)) {
                    el = node as XElement;
                    if (el != null) break;
                }
            }
            return el;
        }
        /* private static XElement source(XElement element)
        {

            XElement ret = new XElement("ret");
            switch (element.Name.LocalName)
            {
                case "table":
                    ret.Add(table(element));
                    break;

                case "usepart":
                    ret.Add(usepart(element));
                    break;

                case "query":
                    ret.Add(sourceMain(element));
                    break;

            }
            return ret.Elements().FirstOrDefault();
        }*/
        private static XElement query(XElement element, XElement qry, XElement rep)
        {
            XAttribute mtr = copyAttribute(element, "materialize");

            //if (Cmn.GetAttrValue(element, "name") == "36703-graf_d")
            //{
            //}
            //if (getAttrValue(element, "name") == "ur_graf_dp")
            //{

            //}

            if (element.Attribute("stored") == null || !useRepositories)
            {


                if (mtr != null)
                {
                    if (qry.Attribute("materialize") == null) {
                        qry.SetAttrValue(mtr.Name, mtr.Value);
                    }
                }



                if ((new string[] { "1", "2" }).Contains(getAttrValue(qry, "materialize")))
                {

                    // if (mtr.Value == "1")
                    // {


                    string name = matDummyProcessing(element, qry, rep);

                    if (_processingCollections.matQueriesDummies.ContainsKey(name))
                    {

                        XElement ret = new XElement(_processingCollections.matQueriesDummies[name]);

                        if (qry.Attribute("as") != null)
                        {
                            ret.SetAttributeValue("as", qry.Attribute("as").Value);
                            ret.SetAttributeValue("materialize", qry.Attribute("materialize").Value);
                        }

                        if (qry.Attribute("join") != null)
                        {
                            ret.SetAttributeValue("join", qry.Attribute("join").Value);

                        }

                        return ret;
                    }

                    //  }

                }
            }
            else
            {

                XElement ret = storedProcessing(element, qry, rep);

                if (qry.Attribute("as") != null)
                {
                    ret.SetAttributeValue("as", qry.Attribute("as").Value);
                    ret.Attributes("materialize").Remove();
                }

                if (qry.Attribute("join") != null)
                {
                    ret.SetAttributeValue("join", qry.Attribute("join").Value);

                }

                return ret;



            }
            XElement wpr = element.Element("withparams");
            if (wpr != null)
            {
                qry.Add(new XElement(wpr));
            }

            wpr = element.Element("extendwhere");
            if (wpr != null)
            {

                qry.Add(element.Elements("extendwhere").Select(e => new XElement(e)));
            }

            wpr = element.Element(TextConst.EName.ExtendLinks);
            if (wpr != null)
            {

                qry.Add(element.Elements(TextConst.EName.ExtendLinks).Select(e => new XElement(e)));
            }
            copyAttribute(element, qry, "order");
            copyAttribute(element, qry, "fixed");
            qry.Add(copyElement(element.Element("order")));

            copyAttribute(element, qry, "hint");
            copyAttribute(element, qry, "noname");
            copyAttribute(element, qry, "haskeys");
            copyAttribute(element, qry, "grouplevel");

            copyAttributes(element, qry, TextConst.ENameArray.ANewQueryAttributes);

            if (element.Element("union") != null)
            {
                qry = unionQuery(element, qry);
            }
            else
            {
                qry = nonUnionQuery(element, qry);
            }




            return qry;
        }
        //!!! Навести порядок с распространением атрибутов используя этот список. Возможно список неполный, дополнить.
        private static string[] columnAttributesNames = new string[] { "type", "agg", "format", TextConst.AName.CMaster, TextConst.AName.CMasterKey, "title", "class-title", "mp", "dimname", "pivot", TextConst.AName.IsFactUse };//Бельченко 17.08.2015  добавил  "dimame", "pivot" 
        //private static string[] columnAttributesNamesCanDub = new string[] { "type", "agg", "format", TextConst.AName.CMaster, TextConst.AName.CMasterKey, "title", "class-title", "dimname", "pivot", TextConst.AName.IsFactUse };//убрал mp его нельзя повторять

        private static int matOrder = 0;
      
        private static bool enableMatOrdering = true;
        private static void setMatOrdreLast(string name)
        {
            if (!enableMatOrdering) return;
            matOrder++;
            XElement ord = _processingCollections.matOrderForNames.Elements().FirstOrDefault(e => e.Attribute("name").Value == name);

            if (ord == null)
            {
                _processingCollections.matOrderForNames.Add(new XElement("ord", new XAttribute("name", name), new XAttribute("ord", matOrder.ToString())));
            }
            else
            {
                ord.SetAttributeValue("ord", matOrder.ToString());
            }
        }

        private static string matDummyProcessing(XElement element, XElement qry1, XElement rep)
        {
            if (_processingCollections.matQueries.Count == 0)
            {
                _processingCollections.matOrderForNames.Elements().Remove();
                matOrder = 0;
            }

            string name = getAttrValue(element, TextConst.AName.Name);

            if (name == "")//???
            {
                name = element.Attribute(TextConst.AName.MaterializeId).Value;
            }


            if (_processingCollections.matQueries.ContainsKey(name))
            {

                // matQueries[element.Attribute("name").Value].SetAttributeValue("matord", matOrder.ToString());
                //  чтобы был правильный порядок, с учетом зависимостей, выполнять с конца
                // setMatOrdreLast(element.Attribute("name").Value);
                return name;
            }
            matOrder++;
            element = new XElement(element);
            element.SetAttributeValue(TextConst.AName.Name, name);
            element.SetAttributeValue("materialize", "1");
            element.SetAttributeValue("noname", "1");
            _processingCollections.matQueries.Add(name, null);
            //  setMatOrdreLast(element.Attribute("name").Value);
            bool isProcessingPivots1 = isProcessingPivots;
            isProcessingPivots = true;
            isProcessingMatDummies = true;
            XElement compiled = compileQuery(element, false, rep);
            isProcessingMatDummies = false;
            isProcessingPivots = isProcessingPivots1;
            // compiled.SetAttributeValue("materialize", "1");
            _processingCollections.matQueries[name] = compiled;

            XElement qry = new XElement("query", new XElement("select"), new XElement("from"));

            copyAttributes(compiled.Element("query"), qry);
            //  copyAttribute(qry1, qry, "as");

            foreach (XElement tbl in compiled.Element("query").Element("from").Elements())
            {

                qry.Element("from").Add(new XElement("table", new XAttribute("name", tbl.Attribute("as").Value), new XAttribute("as", tbl.Attribute("as").Value)));

            }


            qry.Element("select").Add(compiled.Element("query").Element("select").Elements()); // могут быть проблемы если в разделе select есть query
            _processingCollections.matQueriesDummies.Add(name, qry);
            setMatOrdreLast(name);

            return name;

        }
        private static XElement storedProcessing(XElement element, XElement qry1, XElement rep)
        {
            XElement compiled = null;
            string storedName = element.Attribute("stored").Value;
            if (!_processingCollections.storedQueries.ContainsKey(element.Attribute("name").Value)) {
                element = new XElement(element);
                element.Attributes("stored").Remove();
                // element.SetAttributeValue("materialize", "1");
                element.SetAttributeValue("noname", "1");
                // matQueries.Add(element.Attribute("name").Value, null);
                //  setMatOrdreLast(element.Attribute("name").Value);
                bool isProcessingPivots1 = isProcessingPivots;
                isProcessingPivots = true;
                DateTime changeTime;
                XAttribute attr = element.Attribute(AName.timestamp);
                if (attr != null) {
                    changeTime = DateTime.Parse(attr.Value);
                } else {
                    changeTime = DateTime.MaxValue;
                }
                string name = element.Attribute(TextConst.AName.Name).Value;
                compiled = Cache.GetQueryInfoFromCache(name, changeTime, false);
                if (compiled == null) {
                    compiled = compileQuery(element, false, rep);
                    Cache.SaveQueryInfoToCache(compiled, name);
                }
                isProcessingPivots = isProcessingPivots1;
                // compiled.SetAttributeValue("materialize", "1");
                // matQueries[element.Attribute("name").Value] = compiled;
                //storedQueries.Add(element.Attribute("name").Value, compiled);  !!! Пока убираю - проблемы если stored запрос встречается несколько раз
            } else {
                compiled = _processingCollections.storedQueries[element.Attribute("name").Value];
            }

            XElement qry = new XElement("query", new XElement("select"), new XElement("from",

                new XElement("table", new XAttribute("name", storedName), new XAttribute("as", "st"))
                ));

            copyAttributes(compiled.Element("query"), qry);

            if (element.Elements("push").Elements("where").Any())
            {


                List<XElement> pExts = element.Element("push").Elements("from").Elements().ToList();

                foreach (XElement pExt in pExts)
                {
                    foreach (XAttribute att in pExt.Descendants().Attributes("table").Where(a => a.Value == "*").ToList())
                    {
                        var el = att.Parent;

                        att.Remove();
                        el.SetAttributeValue("table", "st");
                    }

                    qry.Element("from").Add(expression(pExt, null));
                }


                pExts = element.Element("push").Elements("where").Elements().ToList();

                foreach (XElement pExt in pExts)
                {
                    foreach (XAttribute att in pExt.Descendants().Attributes("table").Where(a => a.Value == "*").ToList())
                    {
                        var el = att.Parent;

                        att.Remove();
                        el.SetAttributeValue("table", "st");
                    }
                    qry.Add(
                        new XElement("where",
                        expression(pExt, null)
                        )


                        );
                }
            }
            foreach (XElement col in compiled.Element("query").Element("select").Elements())
            {
                XElement newCol = new XElement("column", new XAttribute("table", "st"), new XAttribute("column", col.Attribute("as").Value));


                copyAttributes(col, newCol, new string[] { "as", "title", "class-title", "agg", "format", TextConst.AName.CMaster, TextConst.AName.CMasterKey, "type", "key" });

                qry.Element("select").Add(newCol);

            }
            return qry;


        }
        private static XElement table(XElement element)
        {
            XElement ret = new XElement(EName.table);
            copyAttributes(element, ret);
            if (element.AttrOrDefault(AName.view, false)) {
                string name = element.Attribute(AName.name).Value;
                XElement view = XmlReports.Environment.Manager.GetScheme().Elements(EName.views).Elements(EName.view).SearchByAttribute(AName.name, name);
                if (view == null) {
                    throw new VCompilerException("Представление (view) " + name + " не найдено", element.Ancestors(EName.query).First(), element);
                }
                XElement text = new XElement(EName.text);
                text.Add(new XText("(" + view.Value + ")"));
                ret.Add(text);
            }
            // ret = (XElement)applyPart(ret);
            applyLinks(ret, null);
            return ret;
        }
        private static XElement unionQuery(XElement element, XElement qry)
        {
            foreach (XElement el in element.Element("union").Elements())
            {
                qry.Add(sourceMain(el));
            }
            return qry;
        }
        private static XElement nonUnionQuery(XElement element, XElement qry)
        {


            //if (Cmn.GetAttrValue(element, "name") == "36703-graf_d")
            //{
            //}

            //if (Cmn.GetAttrValue(element, "name") == "36703-fin-dat")
            //{
            //}

            XElement el = null;
            el = new XElement("select");

            Cmn.copyAttributes(element.Element(TextConst.EName.Select), el);
            
            qry.Add(el);
            columns(element, el);

            el = new XElement("from");
            qry.Add(el);
            if (element.Elements(TextConst.EName.ChangeSources).Any())
            {
                hasChangeSources = true;
            }
            foreach (XElement el1 in element.Element("from").Elements())
            {
                el.Add(source(el1));
            }


            el.Add(element.Element("from").Elements(TextConst.EName.QubeContent));

            if (element.Element("where") != null)
            {
                el = new XElement("where");
                qry.Add(el);

                foreach (XElement el1 in element.Elements("where").Elements())
                {
                    el.Add(expression(el1, null));
                }
            }

            el.Add(element.Elements(TextConst.EName.ChangeSources));

            if (element.Element("connect") != null)
            {
                el = new XElement("connect");
                qry.Add(el);

                foreach (XElement el1 in element.Elements("connect").Elements())
                {
                    el.Add(expression(el1, null));
                }
            }





            if (element.Element("start") != null)
            {
                el = new XElement("start");
                qry.Add(el);

                foreach (XElement el1 in element.Elements("start").Elements())
                {
                    el.Add(expression(el1, null));
                }
            }

            if (element.Element("group") != null)
            {

                int i = 0;
                XElement prev = null;

                foreach (XElement el2 in element.Elements("group"))
                {
                    if (i > 0)
                    {
                        if (i == 1)
                        {
                            prev.SetAttributeValue("gset", "first");
                        }
                        else
                        {
                            prev.SetAttributeValue("gset", (i + 1).ToString());
                        }
                    }

                    el = new XElement("group");

                    qry.Add(el);
                    foreach (XElement el1 in el2.Elements())
                    {
                        el.Add(expression(el1, null));
                    }


                    prev = el;
                    i++;
                }
                if (i > 1)
                {
                    el.SetAttributeValue("gset", "last");
                }
            }

            if (element.Element("having") != null)
            {
                el = new XElement("having");
                qry.Add(el);

                foreach (XElement el1 in element.Elements("having").Elements())
                {
                    el.Add(expression(el1, null));
                }
            }


            if (element.Element("dimension") != null)
            {
                el = new XElement("dimension");
                qry.Add(el);

                foreach (XElement el1 in element.Elements("dimension").Elements())
                {
                    el.Add(expression(el1, null));
                }
            }

            if (element.Element("measures") != null)
            {
                el = new XElement("measures");
                qry.Add(el);

                foreach (XElement el1 in element.Elements("measures").Elements())
                {
                    el.Add(expression(el1, null));
                }
            }

            return qry;
        }
        private static XElement columns(XElement element, XElement ret)
        {
            foreach (XElement el1 in element.Element(EName.select).Elements())
            {
                ret.Add(expression(el1, null));
            }
            return ret;
        }
        private static IEnumerable<XElement> applyParamsNext(XElement element, XElement inParams, XElement inFormalParams, string index, bool isPart)
        {
            XElement cont;
            IEnumerable<XElement> ret;
            if (element.Name == EName.useparam || element.Name == EName.useglobparam) {
                ret = applyParamToNode(element, inParams, inFormalParams, index, isPart);
            } else {
                cont = new XElement(element.Name);
                foreach (XAttribute attr in element.Attributes()) {
                    string attrVal = attr.Value;
                    applyParamToAttr(ref attrVal, inParams, inFormalParams);
                    XAttribute newAttr = new XAttribute(attr.Name, attrVal);
                    cont.Add(newAttr);
                }
                foreach (XNode node in element.Nodes()) {
                    if (node.NodeType == XmlNodeType.Element) {
                        IEnumerable<XNode> nodes = applyParamsNext((XElement)node, inParams, inFormalParams, index, isPart);
                        cont.Add(nodes);
                    } else if (node.NodeType == XmlNodeType.Text) {
                        cont.Add(node);
                    }
                }
                ret = new XElement[1] { cont };
            }
            return ret;
        }
        private static IEnumerable<XElement> applyParamToNode(XElement element, XElement inParams, XElement inFormalParams, string index, bool isPart)
        {
            IList<XElement> inFormalParams1 = new List<XElement>();
            IList<XElement> inParams1 = new List<XElement>();
            if (inFormalParams != null) {
                inFormalParams1.Add(inFormalParams);
            }
            if (inParams != null) {
                inParams1.Add(inParams);
            }
            if (element.Name == EName.useglobparam) {
                inParams1 = new List<XElement>();
                inFormalParams1 = XmlReports.Environment.Manager.GetScheme().Elements(EName.globalparams).ToList();
            }
            if (inParams1.Count == 0) {
                inParams1 = inFormalParams1;
            }
            string parFullName = element.Attribute(AName.name).Value;
            string parFullIndex = substringBetween(parFullName, '[', ']');
            string parName;
            if (string.IsNullOrEmpty(parFullIndex)) {
                parName = parFullName;
            } else {
                parName = substringBefore(parFullName, '[');
            }
            XElement formalParam = (inFormalParams1.Elements().Where(par => par.Attribute(AName.name).Value == parName)).FirstOrDefault();
            XElement param1 = null;
            if (!inParams1.Elements().Attributes(AName.parname).Any()) {
                if (formalParam != null) {
                    string formalParamPos = formalParam.ElementsBeforeSelf().Count().ToString();
                    param1 = (inParams1.Elements().Where(par => par.ElementsBeforeSelf().Count().ToString() == formalParamPos)).FirstOrDefault();
                }
            } else {
                param1 = (inParams1.Elements().Where(par => par.AttrOrDefault(AName.parname, string.Empty) == parName)).FirstOrDefault();
            }
            if (param1 == null && isPart) {
                // Если в part есть useparam но нет соответствующего param оставляем useparam
                return new XElement[1] { new XElement(element) };
            }
            //XElement param = new XElement("nodes");
            List<XElement> ret = new List<XElement>(1);
            if (string.IsNullOrEmpty(parFullIndex)) {
                if (param1 != null) {
                    if (element.Name == EName.useglobparam) {
                        ret.Add(Factory.NewConst(TextConst.Pfx.Param + TextConst.Pfx.GlobParam + parName));
                    } else if (param1.Name == EName.param) {
                        if (param1.HasElements) {
                            ret.AddRange(param1.Elements());
                        }
                    } else {
                        ret.Add(param1);
                    }
                }
            } else {
                if (parFullIndex.Contains("@")) {
                    string attrName = substringAfter(parFullIndex, '@');
                    string attrVal = param1.Attribute(attrName).Value;
                    ret.Add(Factory.NewConst(attrVal));
                    //param.Add(new XElement("const", attrVal));
                } else {
                    string parBeginIndex1;
                    if (parFullIndex.Contains("..")) {
                        parBeginIndex1 = substringBefore(parFullIndex, "..");
                    } else {
                        parBeginIndex1 = parFullIndex;
                    }
                    string parBeginIndex;
                    if (parBeginIndex1 == ":index") {
                        parBeginIndex = index;
                    } else {
                        parBeginIndex = parBeginIndex1;
                    }
                    string parEndIndex1;
                    if (parFullIndex.Contains("..")) {
                        parEndIndex1 = substringAfter(parFullIndex, "..");
                    } else {
                        parEndIndex1 = parFullIndex;
                    }
                    string parEndIndex;
                    if (parEndIndex1 == ":index") {
                        parEndIndex = index;
                    } else {
                        parEndIndex = parEndIndex1;
                    }
                    XElement param2;
                    if (param1 != null) {
                        param2 = param1;
                    } else {
                        param2 = formalParam;
                    }
                    foreach (XElement el in param2.Elements()) {
                        int count = el.ElementsBeforeSelf().Count() + 1;
                        if (count >= Convert.ToInt16(parBeginIndex.Replace('*', '0')) && count <= Convert.ToInt16(parEndIndex.Replace("*", "10000"))) {
                            ret.Add(new XElement(el));
                        }
                    }
                }
            }
            if (ret.Count == 0) {
                if (formalParam == null) {
                    throw new VCompilerException("Переменная не найдена", inFormalParams1.First().Parent, element);
                }
                if (!formalParam.HasElements) {
                    formalParam.Add(new XElement(EName.undefined));
                }
                ret.AddRange(formalParam.Elements());
            }
            foreach (XAttribute attr in element.Attributes()) {
                if (attr.Name == AName.pth || attr.Name == AName.parname) {
                    foreach (XElement rel in ret) {
                        rel.SetAttributeValue(attr.Name, attr.Value);
                    }
                }
            }
            return ret;
        }
        /// <summary>
        /// <para>Заменяет в строке <paramref name="attrValue"/> подстановочные символы 
        /// [:параметр] и [:параметр.атрибут] на их значения из <paramref name="inParams"/>.</para>
        /// <para>":параметр" и ":параметр.атрибут" в <paramref name="attrValue"/> эквивалентно
        /// "[:параметр]" и "[:параметр.атрибут]", т.е. всё значение заменяется значением формального параметра.</para>
        /// </summary>
        /// <param name="attrValue">значение атрибута, в котором производится замена подстановочных символов</param>
        /// <param name="inParams">фактические параметры</param>
        /// <param name="inFormalParams">формальные параметры</param>
        private static void applyParamToAttr(ref string attrValue, XElement inParams, XElement inFormalParams)
        {
            if (string.IsNullOrEmpty(attrValue)) {
                return;
            }
            string parFullName;
            int pos_1 = 0;
            int pos_2 = 0;
            while (true) {
                bool prIn = attrValue[0] != ':';
                if (!prIn) {
                    if (attrValue.Length == 1) {
                        break;
                    }
                    parFullName = attrValue.Substring(1);
                } else {
                    pos_1 = attrValue.IndexOf("[:");
                    if (pos_1 < 0) {
                        break;
                    }
                    int pos_param_name = pos_1 + 2;
                    pos_2 = attrValue.IndexOf(']', pos_param_name);
                    if (pos_2 < 0 || pos_2 == pos_param_name) {
                        break;
                    }
                    parFullName = attrValue.Substring(pos_param_name, pos_2 - pos_param_name);
                }
                Contract.Assert(!string.IsNullOrEmpty(parFullName));
                string parChildName;
                string parName;
                // Если подстановочный символ содержит точку, 
                // то до точки указывается наименование параметра,
                // а после неё - наименование атрибута, чьим значением его нужно заменить
                int dot_pos = parFullName.IndexOf('.');
                if (dot_pos < 0) {
                    parName = parFullName;
                    parChildName = null;
                } else {
                    parName = parFullName.Substring(0, dot_pos);
                    parChildName = parFullName.Substring(dot_pos + 1);
                }
                XElement formalParam = inFormalParams.Elements().SearchByAttribute(AName.name, parName);
                if (formalParam == null) {
                    break;
                }
                XElement param;
                if (inParams != null) {
                    int formalParamPos = formalParam.ElementsBeforeSelf().Count();
                    param = inParams.Elements().FirstOrDefault(e => e.ElementsBeforeSelf().Count() == formalParamPos);
                } else {
                    param = formalParam.Elements().FirstOrDefault();
                }
                string param_value;
                if (param == null) {
                    param_value = string.Empty;
                } else if (!string.IsNullOrEmpty(parChildName)) {
                    param_value = param.Attribute(parChildName).Value;
                } else {
                    if (param.FirstNode == null) {
                        break;
                    }
                    param_value = param.Value.Replace("'", string.Empty);
                }
                if (prIn) {
                    // Здесь pos_1 содержит позицию символа '[', а pos_2 - символа ']'
                    Contract.Assert(attrValue[pos_1] == '[');
                    Contract.Assert(attrValue[pos_2] == ']');
                    attrValue = attrValue.Substring(0, pos_1) + param_value + attrValue.Substring(pos_2 + 1);
                } else {
                    attrValue = param_value;
                    break;
                }
            }
        }
        #region обработка строк
        private static string substringBefore(string str, char ch)
        {
            Contract.Assert(str != null);
            int i = str.IndexOf(ch);
            if (i >= 0) {
                return str.Substring(0, i);
            } else {
                return null;
            }
        }
        private static string substringBefore(string s1, string s2)
        {
            Contract.Assert(s1 != null);
            int i = s1.IndexOf(s2);
            if (i >= 0) {
                return s1.Substring(0, i);
            } else {
                return null;
            }
        }
        private static string substringAfter(string str, char ch)
        {
            if (str == null) {
                return null;
            }
            int i = str.IndexOf(ch);
            if (i >= 0) {
                return str.Substring(i + 1);
            } else {
                return string.Empty;
            }
        }
        private static string substringAfter(string s1, string s2)
        {
            if (s1 == null) {
                return null;
            }
            int i = s1.IndexOf(s2);
            if (i >= 0) {
                return s1.Substring(i + s2.Length);
            } else {
                return string.Empty;
            }
        }
        /// <summary>
        /// <para>Возвращает текст из строки <paramref name="str"/>, 
        /// заключённый между скобками <paramref name="ch_1"/> и <paramref name="ch_2"/>.</para>
        /// <para>Если хотя бы одна из скобок не найдена или они расположены в обратном порядке, возвращается null.</para>
        /// <para>Например, <code>substringBetween("ABC[index]", '[', ']')</code> возвращает "index".</para>
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="ch_1">левая (открывающая) скобка</param>
        /// <param name="ch_2">правая (закрывающая) скобка</param>
        /// <returns>текст, заключённый между скобками <paramref name="ch_1"/> и <paramref name="ch_2"/></returns>
        private static string substringBetween(string str, char ch_1, char ch_2)
        {
            Contract.Assert(str != null);
            int pos_1 = str.IndexOf(ch_1);
            if (pos_1 < 0) {
                return null;
            }
            pos_1 = pos_1 + 1;
            int pos_2 = str.IndexOf(ch_2, pos_1);
            if (pos_2 < 0) {
                return null;
            }
            return str.Substring(pos_1, pos_2 - pos_1);
        }
        private static void loopReplace(StringBuilder sb, string oldValue, string newValue)
        {
            Contract.Assert(oldValue != null);
            Contract.Assert(newValue == null || newValue.Length != oldValue.Length);
            int len = sb.Length;
            while (true) {
                sb.Replace(oldValue, newValue);
                int new_len = sb.Length;
                if (new_len == len) {
                    break;
                }
                len = new_len;
            };
        }
        internal static string normalizeWhitespace(string s)
        {
            StringBuilder sb = new StringBuilder(s);
            loopReplace(sb, "  ", " ");
            loopReplace(sb, "\r\r", "\r");
            loopReplace(sb, "\n\n", "\n");
            loopReplace(sb, "\r\n\r\n", "\r\n");
            return sb.ToString();
        }
        #endregion
        private static string getAttrValue(XElement el, XName name_1, XName name_2)
        {
            XAttribute attr = el.Attribute(name_1);
            if (attr == null) {
                attr = el.Attribute(name_2);
                if (attr == null) {
                    return string.Empty;
                }
            }
            return attr.Value;
        }
        internal static string getAttrValue(XElement el, string attrName)
        {
            if (el == null) {
                return string.Empty;
            } else {
                return el.AttrOrEmpty(attrName);
            }
        }
        private static XElement copyElement(XElement el)
        {
            if (el != null) {
                return new XElement(el);
            } else {
                return null;
            }
        }
        private static void setAttributes(XElement el, IEnumerable<XAttribute> attrs)
        {
            foreach (XAttribute attr in attrs)
            {
                el.SetAttributeValue(attr.Name, attr.Value);
            }
        }
        // NEW
        private static bool pushpred = false;
        private static bool useNL = false;
        private static void applyLinks(XElement query, string inhName)
        {

            if (pushpred)
            {
                foreach (XElement link in query.Descendants("dlink").ToArray())
                {
                    link.SetAttributeValue("pushpred", "1");
                }
            }

            IEnumerable<XElement> links = query.DescendantsAndSelf().Elements().Where(EPredicate.IsQueryOrTable)
                .Descendants().Where(e => EPredicate.IsLinkOrDLinkOrSLink(e) && e.AttrOrDefault("pushpred", string.Empty) == "1");
            foreach (XElement link in links.ToArray())
            {
                applyDlinkPush(query, link);
            }

            foreach (XElement link in links.ToArray())
            {
                if (link.Parent != null)
                {
                    link.Remove();
                }
            }
            //   links.Remove();
            links = query.DescendantsAndSelf().Elements().Where(EPredicate.IsQueryOrTable).Elements().Where(e => EPredicate.IsAnyLink(e) && getAttrValue(e, "pushpred") != "1");
            while (links.Any())
            {
                foreach (XElement link in links.ToArray())
                {
                    XElement linkerQuery = link.Parent;

                    XElement linkerQueryScheme = null;

                    if (linkerQuery.Name.LocalName == TextConst.AName.Table //чтобы обрабатывались линки на связи у наследников, не проверено

                        && query.Attribute("name") != null && query.Attribute("name").Value!="a"/*заплатка такое может быть у списков*/) // была ошибка
                    {
                        linkerQueryScheme = getQueryScheme(query.Attribute("name").Value);
                    }
                    else
                    {
                        linkerQueryScheme = getQueryScheme(linkerQuery.Attribute("name").Value);
                    }

                    

                    string linkerMainQueryAlias = getAttrValue(linkerQueryScheme.Elements("from").Elements().First(), "as");
                    if (link.Name.LocalName == "link")
                    {
                        XElement linkedQuery = linkerQueryScheme.Elements("from").Elements("query").FirstOrDefault(e => e.Attribute("as").Value == link.Attribute("name").Value);

                        if (linkedQuery == null)
                        {
                            linkedQuery = linkerQueryScheme.Elements("push").Elements("from").Elements("query").First(e => e.Attribute("as").Value == link.Attribute("name").Value);
                        }
                        linkedQuery = new XElement(linkedQuery);
                        linkedQuery.Elements("link").Remove();

                        IEnumerable<XElement> linkerQueryColumns = linkedQuery.Descendants("column").Where(e => e.Attribute("table").Value == linkerMainQueryAlias || e.Attribute("table").Value == "this" || e.Attribute("table").Value == "*").ToArray();
                        linkedQuery.SetAttrValue(AName.@as, link.Attribute(AName.@as).Value);
                        IEnumerable<XElement> linkedQueryColumns = linkedQuery.Descendants("column").Where(e => e.Attribute("table").Value == link.Attribute("name").Value).ToArray();
                        foreach (XElement col in linkerQueryColumns)
                        {
                            col.SetAttrValue(AName.table, linkerQuery.Attribute(AName.@as).Value);
                        }
                        foreach (XElement col in linkedQueryColumns)
                        {
                            col.SetAttrValue(AName.table, link.Attribute(AName.@as).Value);
                        }
                        linkedQuery.Add(link.Elements("link"));
                        linkedQuery.Add(link.Elements("dlink"));
                        linkedQuery.Add(link.Elements("elink"));
                        linkedQuery.Add(link.Elements("slink"));

                        //if (linkedQuery.Attribute(TextConst.AName.Dimension) != null)
                        //{

                        //}

                        var keyDimCol = link.Attributes(TextConst.AName.Dimension).FirstOrDefault(e => e.Value != "1");



                        if (keyDimCol != null)
                        {
                            linkedQuery.SetAttributeValue(TextConst.AName.Dimension, keyDimCol.Value);

                        }
                        else
                        {
                            linkedQuery.Attributes(TextConst.AName.Dimension).Remove();
                        }
                        copyAttribute(link, linkedQuery, TextConst.AName.LinkMultiplicatePoint);
                        linkerQuery.AddAfterSelf(linkedQuery);

                        //Cmn.CopyAttribute(link, linkerQuery, TextConst.AName.MainInEditor);

                        if (getAttrValue(link, "hint") != "")
                        {
                            string sHint = getAttrValue(linkedQuery, "hint");
                            sHint += " " + getAttrValue(link, "hint");
                            linkedQuery.SetAttributeValue("hint", sHint);
                        }

                        if (link.Element("where") != null)
                        {
                            if (linkedQuery.Element("call").Attribute("function").Value != "and")
                            {
                                XElement jexp = new XElement(linkedQuery.Element("call"));
                                XElement newJexp = new XElement("call", new XAttribute("function", "and"), jexp);
                                linkedQuery.Element("call").ReplaceWith(newJexp);
                            }
                            linkedQuery.Element("call").Add(link.Element("where").Elements());
                        }

                        link.Remove();

                        if (link.Attribute("usenl") != null)
                        {
                            if (getAttrValue(link, "usenl") == "1" || useNL)
                            {
                                string queryMainQueryAlias = query.Elements("from").Elements().First().Attribute("as").Value;
                                string sHint = getAttrValue(query, "hint");
                                sHint += string.Format(" use_nl({0} {1})", queryMainQueryAlias, link.Attribute("as").Value);
                                query.SetAttributeValue("hint", sHint);
                            }
                        }

                    }


                    if (link.Name.LocalName == "elink")
                    {
                        // Можно переписать с учетом addLinkInfo
                        //XElement linkedQueryCall = schemeRoot.Elements("queries").Elements("query").Elements("from").Elements("query").FirstOrDefault(e => getAttrValue(e, "dname") == link.Attribute("name").Value && getAttrValue(e, "name") == linkerQuery.Attribute("name").Value);
                        XElement linkedQueryCall = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query").Elements("from").Elements("query").FirstOrDefault(e => getAttrValue(e, "dname") == link.Attribute("name").Value && getAttrValue(e, "name") == link.Attribute("parent").Value);

                        XElement linkedQuery;
                        if (linkedQueryCall != null)
                        {

                            linkedQuery = linkedQueryCall.Parent.Parent;
                        }
                        else
                        {
                            linkedQueryCall = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query").Elements("push").Elements("from").Elements("query").FirstOrDefault(e => getAttrValue(e, "dname") == link.Attribute("name").Value && getAttrValue(e, "name") == linkerQuery.Attribute("name").Value);
                            linkedQuery = linkedQueryCall.Parent.Parent.Parent;
                        }
                        string linkedMainQueryAlias = getAttrValue(linkedQuery.Elements("from").Elements().First(), "as");

                        linkedQueryCall = new XElement(linkedQueryCall);

                        // XElement linkedQuery = new XElement(linkerQueryScheme.Elements("from").Elements("query").Where(e => e.Attribute("as").Value == link.Attribute("name").Value).First());


                        IEnumerable<XElement> linkerQueryColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedQueryCall.Attribute("as").Value).ToArray();
                        IEnumerable<XElement> linkedQueryColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedMainQueryAlias || e.Attribute("table").Value == "this" || e.Attribute("table").Value == "*").ToArray();

                        // IEnumerable<XElement> linkerQueryColumns = linkedQuery.Descendants("column").Where(e => e.Attribute("table").Value == linkerMainQueryAlias | e.Attribute("table").Value == "this");

                        linkedQueryCall.SetAttrValue(AName.name, linkedQuery.Attribute(AName.name).Value);
                        linkedQueryCall.SetAttrValue(AName.@as, link.Attribute(AName.@as).Value);

                        //   IEnumerable<XElement> linkedQueryColumns = linkedQuery.Descendants("column").Where(e => e.Attribute("table").Value == link.Attribute("name").Value);
                        foreach (XElement col in linkerQueryColumns.ToArray()) {
                            col.SetAttrValue(AName.table, linkerQuery.Attribute(AName.@as).Value);
                        }
                        foreach (XElement col in linkedQueryColumns.ToArray()) {
                            col.SetAttrValue(AName.table, link.Attribute(AName.@as).Value);
                        }
                        linkedQueryCall.Add(link.Elements("link"));
                        linkedQueryCall.Add(link.Elements("dlink"));
                        linkedQueryCall.Add(link.Elements("elink"));
                        linkedQueryCall.Add(link.Elements("slink"));
                        linkedQueryCall.Add(link.Elements("extendwhere"));
                        linkedQueryCall.Add(link.Elements(TextConst.EName.ExtendLinks));
                        //linkedQueryCall.Add(link.Elements(TextConst.EName.QubeContent));

                        var keyDimCol = link.Attributes(TextConst.AName.Dimension).FirstOrDefault(e => e.Value != "1");



                        if (keyDimCol != null)
                        {
                            linkedQueryCall.SetAttributeValue(TextConst.AName.Dimension, keyDimCol.Value);

                        }
                        else
                        {
                            linkedQueryCall.Attributes(TextConst.AName.Dimension).Remove();
                        }

                        copyAttribute(link, linkedQueryCall, TextConst.AName.LinkMultiplicatePoint);
                        if (link.Element("where") != null)
                        {
                            var call = linkedQueryCall.Element("call");
                            if (call != null)
                            {
                                if (call.Attribute("function").Value != "and")
                                {
                                    XElement jexp = new XElement(call);
                                    XElement newJexp = new XElement("call", new XAttribute("function", "and"), jexp);
                                    call.ReplaceWith(newJexp);
                                }
                                linkedQueryCall.Element("call").Add(link.Element("where").Elements());
                            }
                        }

                        linkerQuery.AddAfterSelf(linkedQueryCall);
                        link.Remove();


                        


                        if (link.Attribute("usenl") != null)
                        {
                            if (getAttrValue(link, "usenl") == "1" || useNL)
                            {
                                string queryMainQueryAlias = query.Elements("from").Elements().First().Attribute("as").Value;
                                string sHint = getAttrValue(query, "hint");
                                sHint += string.Format(" use_nl({0} {1})", queryMainQueryAlias, link.Attribute("as").Value);
                                query.SetAttributeValue("hint", sHint);
                            }
                        }
                    }
                    if (link.Name == EName.slink) {
                        XElement linkExpr = makeSlinkExpr(link);
                        XElement lparent = link.Parent;
                        linkerQuery.AddAfterSelf(linkExpr);
                        link.Remove();

                        if (link.Attribute("usenl") != null || link.Attribute("usehash") != null) //  !!! не глядя
                        {
                            string h = "use_hash";
                            if (link.Attribute("usenl") != null)
                            {
                                h = "use_nl";
                            }

                            if (getAttrValue(link, "usenl") == "1" || getAttrValue(link, "usehash") == "1" || useNL)
                            {
                                // string queryMainQueryAlias = query.Elements("from").Elements().First().Attribute("as").Value;

                                string queryMainQueryAlias = lparent.Attribute("as").Value;
                                string sHint = getAttrValue(query, "hint");
                                sHint += string.Format(" {0}({1} {2})", h, queryMainQueryAlias, link.Attribute("as").Value);
                                query.SetAttributeValue("hint", sHint);
                            }
                        }

                    }
                    if (link.Name.LocalName == "dlink")
                    {

                        applyDlink(query, link, linkerQuery, inhName);
                    }
                }
                links = query.DescendantsAndSelf().Elements("query").Elements().Where(e => EPredicate.IsAnyLink(e) && getAttrValue(e, "pushpred") != "1");
            }
        }
        /// <summary>
        /// Возвращает значение атрибута "table" элемента <paramref name="el"/>, если он есть, или атрибута "name"
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private static string getLQTableName(XElement el)
        {
            Contract.Assume(el != null);
            XAttribute attr = el.Attribute(AName.table);
            if (attr == null) {
                attr = el.Attribute(AName.name);
            }
            return attr.Value;
        }
        /// <summary>
        /// Возвращает значение атрибута "as" элемента <paramref name="el"/>, если он есть, или атрибута "column"
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private static string getAliasOrColumn(XElement el)
        {
            Contract.Assume(el != null);
            XAttribute attr = el.Attribute(AName.@as);
            if (attr == null) {
                attr = el.Attribute(AName.column);
            }
            return attr.Value;
        }
        /// <summary>
        /// Возвращает значение атрибута "as" элемента <paramref name="el"/>, если он есть, или атрибута "name"
        /// </summary>
        /// <param name="el"></param>
        /// <returns></returns>
        private static string getAliasOrName(XElement el)
        {
            Contract.Assume(el != null);
            XAttribute attr = el.Attribute(AName.@as);
            if (attr == null) {
                attr = el.Attribute(AName.name);
            }
            return attr.Value;
        }
        private static XElement makeSlinkExpr(XElement link)
        {
            Contract.Assert(link != null);
            XElement linkExpr = new XElement(EName.query);
            linkExpr.Add(new XAttribute(AName.name, link.Attribute(AName.name).Value));
            linkExpr.Add(new XAttribute(AName.@as, link.Attribute(AName.@as).Value));
            linkExpr.Add(new XAttribute(AName.join, TextConst.AVJoin.LeftOuter));
            XElement cond = Factory.NewCall(TextConst.AVFunction.And);
            linkExpr.Add(cond);
            XElement parenQueryScheme = getQueryScheme(getLQTableName(link.Parent));
            if (parenQueryScheme.Elements(EName.select).Elements(EName.column).Any(e => e.AttrOrDefault(AName.column, string.Empty) == TextConst.AVColumn.All)) {
                parenQueryScheme = compileQuery(parenQueryScheme, true, null).Element(EName.query);
            } else {
                markQueryKeys(parenQueryScheme);
            }
            foreach (XElement colPar in parenQueryScheme.Element(EName.select).Elements()) {
                if (colPar.AttrOrDefault(AName.key, false)) {
                    string col_name = colPar.Attribute(AName.@as).Value;
                    XElement call = Factory.NewCall(TextConst.AVFunction.Equal);
                    call.Add(Factory.NewColumn(link.Parent.Attribute(AName.@as).Value, col_name));
                    call.Add(Factory.NewColumn(link.Attribute(AName.@as).Value, col_name));
                    cond.Add(call);
                }
            }
            linkExpr.Add(link.Elements(EName.link));
            linkExpr.Add(link.Elements(EName.dlink));
            linkExpr.Add(link.Elements(EName.elink));
            linkExpr.Add(link.Elements(EName.slink));
            linkExpr.Add(link.Elements(EName.withparams));
            linkExpr = addLinkWhere(link, linkExpr);
            return linkExpr;
        }
        private static XElement addLinkWhere(XElement link, XElement expr)
        {
            Contract.Assert(link != null);
            IList<XElement> where = link.Elements(EName.where).ToList<XElement>();
            if (where.Count == 0) {
                return expr;
            }
            Contract.Assert(expr != null);
            string alias = link.Attribute(AName.@as).Value;
            XElement newExpr = new XElement(EName.query, new XAttribute(AName.@as, alias));
            expr.Attributes(AName.join).ChangeParent(newExpr);
            newExpr.Add(new XElement(EName.select, Factory.NewColumn(alias, TextConst.AVColumn.All)));
            newExpr.Add(new XElement(EName.from, expr));
            newExpr.Add(where);
            expr.Elements(EName.call).ChangeParent(newExpr);
            return newExpr;
        }
        private static void applyDlink(XElement query, XElement link, XElement linkerQuery, string inhName)
        {


            /*  IEnumerable<XElement> pushedPredicates = query.Elements("where").Descendants("call").Where(e => getAttrValue(e, "pushpred") == "1");

              foreach (XElement pred in pushedPredicates)
              {
                  foreach (XElement col in pred.Descendants("column"))
                  {
                      XElement colSource = query.Elements().Where(e => (new string[] { "query", "link" }).Contains(e.Name.LocalName) &&  getAttrValue(e,"as")==col.Attribute("table").Value).First();

                      List<XElement> linkBranch = new List<XElement>();

                      XElement brachCursor = colSource;
                      while ()

                  }
                           
              }*/

           // Можно переписать с учетом addLinkInfo
           // 
           // string inhName = null;
            if (inhName == null)
            {
                if (query.Attribute(TextConst.AName.Inherit) != null)
                {
                    if (query.Attribute(TextConst.AName.Name) != null)
                    {
                        inhName = query.Attribute(TextConst.AName.Name).Value;
                    }
                }
            }

            XElement linkedQuery = null;
            XElement linkedQueryCall = null;
            if (inhName != null)
            {
                linkedQueryCall = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query").Elements("from").Elements("query").FirstOrDefault(e => getAttrValue(e, "dname") == link.Attribute("name").Value && getAttrValue(e, "name") == inhName);
            }

            if (linkedQueryCall == null)
            {
                linkedQueryCall = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query").Where(q => q.Attribute(TextConst.AName.Inherit) == null).Elements("from").Elements("query").FirstOrDefault(e => getAttrValue(e, "dname") == link.Attribute("name").Value && getAttrValue(e, "name") == linkerQuery.Attribute("name").Value);
            }

            if (linkedQueryCall == null)
            {
                linkedQueryCall = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query").Elements("from").Elements("query").FirstOrDefault(e => getAttrValue(e, "dname") == link.Attribute("name").Value && getAttrValue(e, "name") == linkerQuery.Attribute("name").Value);
            }



            if (linkedQueryCall != null)
            {

                linkedQuery = linkedQueryCall.Parent.Parent;
            }
            else
            {
                linkedQueryCall = XmlReports.Environment.Manager.GetScheme().Elements("queries").Elements("query").Elements("push").Elements("from").Elements("query").FirstOrDefault(e => getAttrValue(e, "dname") == link.Attribute("name").Value && getAttrValue(e, "name") == linkerQuery.Attribute("name").Value);
                if (linkedQueryCall != null) // может быть если ссылка на query из другого проекта, костыль, подумать
                {
                    linkedQuery = linkedQueryCall.Parent.Parent.Parent;
                }
            
            }
            if (linkedQuery != null)// может быть если ссылка на query из другого проекта, костыль, подумать
            {
                string linkedMainQueryAlias = null;

                var attrMainQueryAlias = linkedQuery.Elements("from").Elements().First().Attribute("as");
                if (attrMainQueryAlias != null)
                {
                    linkedMainQueryAlias = attrMainQueryAlias.Value;

                }

                linkedQueryCall = new XElement(linkedQueryCall);


                IEnumerable<XElement> linkerQueryColumns =
                    linkedQueryCall.Descendants("column")
                        .Where(e => e.Attribute("table").Value == linkedQueryCall.Attribute("as").Value)
                        .ToArray();
                IEnumerable<XElement> linkedQueryColumns =
                    linkedQueryCall.Descendants("column")
                        .Where(
                            e =>
                                e.Attribute("table").Value == linkedMainQueryAlias ||
                                e.Attribute("table").Value == "this" || e.Attribute("table").Value == "*")
                        .ToArray();

                foreach (XElement col in linkerQueryColumns) {
                    col.SetAttrValue(AName.table, linkerQuery.Attribute(AName.@as).Value);
                }
                foreach (XElement col in linkedQueryColumns) {
                    col.SetAttrValue(AName.table, link.Attribute(AName.@as).Value);
                }
                XElement dlinkExpr = new XElement(EName.query);
                dlinkExpr.Add(new XAttribute(AName.join, TextConst.AVJoin.LeftOuter));
                dlinkExpr.Add(new XAttribute(AName.@as, link.Attribute(AName.@as).Value));

                dlinkExpr.Add(
                    new XElement("select"),
                    new XElement("from"),
                    linkedQueryCall.Elements("call").First()
                    );

                dlinkExpr.Add(link.Elements("where"));

                XElement dlinkExprQry = new XElement("query",
                    new XAttribute("name", linkedQuery.Attribute("name").Value),
                    new XAttribute("as", link.Attribute("as").Value));
                dlinkExprQry.Add(link.Elements("withparams"));
                dlinkExprQry.Add(link.Elements("extendwhere"));
                dlinkExprQry.Add(link.Elements(TextConst.EName.ExtendLinks));
                dlinkExprQry.Add(link.Elements("link"));
                dlinkExprQry.Add(link.Elements("dlink"));
                dlinkExprQry.Add(link.Elements("elink"));
                dlinkExprQry.Add(link.Elements("slink"));

                dlinkExpr.Element("from").Add(dlinkExprQry);




                /*string parentJoinAlias = link.Attribute("as").Value + "_p";

            XElement parentJoin=new XElement("query",new XAttribute("as",parentJoinAlias));*/


                foreach (XElement col in linkedQueryColumns)
                {
                    XElement col1 = new XElement(col);
                    col1.SetAttributeValue("group", "1");
                    dlinkExpr.Element("select").Add(col1);
                }
                SortedList<string, string> colNames = new SortedList<string, string>();
                string colAlias;
                IEnumerable<XElement> selColumns =
                    getQueryColumns(query)
                        .Where(e1 => e1.Attribute("table").Value == link.Attribute("as").Value)
                        .ToArray();
                foreach (XElement col in selColumns)
                {

                    // Может тормозить!!!!!! переписать, наверное
                    if (col.Attribute("as") != null)
                    {
                        colAlias = col.Attribute("as").Value;
                    }
                    else
                    {

                        XElement colWithAlias =
                            selColumns.FirstOrDefault(
                                e1 =>
                                    e1.Attribute("column").Value == col.Attribute("column").Value &
                                    e1.Attribute("as") != null);
                        if (colWithAlias != null)
                        {

                            colAlias = colWithAlias.Attribute("as").Value;
                        }
                        else
                        {
                            colAlias = col.Attribute("column").Value;
                        }
                    }


                    if (!colNames.Keys.Contains(colAlias))
                    {

                        XElement col1 = new XElement("column");

                        col1.SetAttributeValue("table", link.Attribute("as").Value);

                        string dgr = getAttrValue(col, "dgroup");
                        if (dgr == "")
                        {
                            dgr = "sum";
                        }
                        col1.SetAttributeValue("group", dgr);
                        col1.SetAttributeValue("column", col.Attribute("column").Value);
                        col1.SetAttributeValue("as", colAlias);
                        dlinkExpr.Element("select").Add(col1);
                        colNames.Add(colAlias, colAlias);
                    }
                    else
                    {
                        colAlias = colNames[colAlias];
                    }
                    col.SetAttributeValue("column", colAlias);
                }

                foreach (XElement childLink in link.Descendants().Where(EPredicate.IsLinkOrDLinkOrSLink)) {
                    colNames.Clear();
                    IEnumerable<XElement> childSelCols = getQueryColumns(query).Where(
                        e1 => e1.Attribute("table").Value == childLink.Attribute("as").Value
                        ).ToArray();

                    foreach (XElement col in childSelCols)
                    {

                        // Может тормозить!!!!!! переписать, наверное
                        if (col.Attribute("as") != null)
                        {
                            colAlias = col.Attribute("as").Value;
                        }
                        else
                        {

                            XElement colWithAlias =
                                childSelCols.FirstOrDefault(
                                    e1 =>
                                        e1.Attribute("column").Value == col.Attribute("column").Value &
                                        e1.Attribute("as") != null);
                            if (colWithAlias != null)
                            {

                                colAlias = colWithAlias.Attribute("as").Value;
                            }
                            else
                            {
                                colAlias = col.Attribute("column").Value;
                            }
                        }

                        if (!colNames.Keys.Contains(colAlias))
                        {

                            XElement col1 = new XElement("column");

                            col1.SetAttributeValue("table", childLink.Attribute("as").Value);
                            col1.SetAttributeValue("group", col.Attribute("dgroup").Value);
                            col1.SetAttributeValue("dgroup", col.Attribute("dgroup").Value);
                            col1.SetAttributeValue("column", col.Attribute("column").Value);
                            col1.SetAttributeValue("as", colAlias);
                            dlinkExpr.Element("select").Add(col1);
                            colNames.Add(colAlias, colAlias);
                        }
                        else
                        {
                            colAlias = colNames[colAlias];
                        }
                        col.SetAttributeValue("column", colAlias);
                        col.SetAttributeValue("table", link.Attribute("as").Value);
                    }


                }

                if (link.Attribute("usenl") != null)
                {
                    if (getAttrValue(link, "usenl") == "1" || useNL)
                    {
                        string queryMainQueryAlias = query.Elements("from").Elements().First().Attribute("as").Value;
                        string sHint = getAttrValue(query, "hint");
                        sHint += string.Format(" use_nl({0} {1})", queryMainQueryAlias, link.Attribute("as").Value);
                        query.SetAttributeValue("hint", sHint);
                    }
                }


                string inhName1 = null;

                if (linkedQuery.Attribute(TextConst.AName.Inherit) != null)
                {
                    inhName1 = linkedQuery.Attribute(TextConst.AName.Inherit).Value; // может все сломать
                }
                applyLinks(dlinkExpr, inhName1);

                linkerQuery.AddAfterSelf(dlinkExpr);
            }
            link.Remove();


            /*IEnumerable<XElement> linkerQueryColumns = linkedQuery.Descendants("column").Where(e => e.Attribute("table").Value == linkerMainQueryAlias);
                       
            linkedQuery.Add(link.Elements("link"));


            linkerQuery.AddAfterSelf(linkedQuery);
            link.Remove();*/

        }
        private static void applyDlinkPush(XElement query, XElement link)
        {
            XElement firstLevelLink = link.AncestorsAndSelf().Where(e => e.Parent != null).First(e => EPredicate.IsQueryOrTable(e.Parent));
            XElement ownerQuery = firstLevelLink.Parent.Ancestors(EName.query).First();

            List<string> tableAliaces = new List<string>(16);
            tableAliaces.Add(link.Attribute(AName.@as).Value);
            IList<XElement> childLinks = link.Elements().Where(EPredicate.IsLinkOrSLink).ToList();
            while (childLinks.Count > 0) {
                foreach (XElement childLink in childLinks) {
                    tableAliaces.Add(childLink.Attribute(AName.@as).Value);
                }
                childLinks = childLinks.Elements().Where(EPredicate.IsLinkOrSLink).ToList();
            }
            IEnumerable<XElement> selColumns = getQueryColumns(ownerQuery).Where(e1 => tableAliaces.Contains(e1.Attribute("table").Value)).ToList();


            if (!selColumns.Any())
            {
                return;
            }
            XElement linkerQuery = firstLevelLink.Parent;
            IEnumerable<XElement> parentColumns;
            IEnumerable<XElement> childColumns;
            //  XElement linkedQuery;
            XElement linkedQueryCall;
            //XElement linkerQueryScheme;
            //string linkerMainQueryAlias;
            if (firstLevelLink.Name == EName.slink) {
                linkedQueryCall = makeSlinkExpr(link);
            } else {
                /*linkedQueryCall = schemeRoot.Elements("queries").Elements("query").Elements("from").Elements("query").First(e => getAttrValue(e, "dname") == firstLevelLink.Attribute("name").Value && getAttrValue(e, "name") == linkerQuery.Attribute("name").Value);

                linkedQuery = linkedQueryCall.Parent.Parent;

              // linkerQueryScheme=linkedQuery;
          
                 string linkedMainQueryAlias = linkedQuery.Elements("from").Elements().First().Attribute("as").Value;

                linkedQueryCall = new XElement(linkedQueryCall);


                linkerQueryColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedQueryCall.Attribute("as").Value).ToArray();
                linkedQueryColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedMainQueryAlias).ToArray();*/

                XElement queryScheme = getQueryScheme(firstLevelLink.Attribute("child").Value);
                linkedQueryCall = queryScheme.Elements("from").Elements("query").FirstOrDefault(e => e.Attribute("as").Value == firstLevelLink.Attribute("field").Value);
                string mainQueryAlias = null;
                if (linkedQueryCall == null)
                {
                    linkedQueryCall = queryScheme.Elements("push").Elements("from").Elements("query").First(e => e.Attribute("as").Value == firstLevelLink.Attribute("field").Value);
                    mainQueryAlias = "*";
                }
                else
                {
                    mainQueryAlias = queryScheme.Elements("from").Elements().First().Attribute("as").Value;
                }

                linkedQueryCall = new XElement(linkedQueryCall);






                if (firstLevelLink.Attribute("name").Value == firstLevelLink.Attribute("back").Value)
                {
                    parentColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedQueryCall.Attribute("as").Value).ToArray();
                    childColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == mainQueryAlias).ToArray();

                }
                else
                {
                    childColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedQueryCall.Attribute("as").Value).ToArray();
                    parentColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == mainQueryAlias).ToArray();
                }
                foreach (XElement col in parentColumns) {
                    col.SetAttrValue(AName.table, firstLevelLink.Parent.Attribute(AName.@as).Value);
                }
                foreach (XElement col in childColumns){
                    col.SetAttrValue(AName.table, firstLevelLink.Attribute(AName.@as).Value);
                }
            }
            XElement dlinkExpr = new XElement(EName.query);
            dlinkExpr.Add(
                 new XElement("select"),
                 new XElement("from"),
                 new XElement("where",
                      new XElement("call", new XAttribute("function", "and"))
                      )
            );
            dlinkExpr.Element("where").Element("call").Add(firstLevelLink.Elements("where").Elements());


            XElement joinExp = linkedQueryCall.Elements("call").First();
            joinExp.SetAttributeValue("joinexp", "1");
            dlinkExpr.Element("where").Element("call").Add(joinExp);






            XElement dlinkExprQry = new XElement("query", new XAttribute("name", firstLevelLink.Attribute("table").Value), new XAttribute("as", firstLevelLink.Attribute("as").Value));
            //  dlinkExprQry.Add(firstLevelLink.Elements("withparams"));
            //  dlinkExprQry.Add(firstLevelLink.Elements("link"));


            dlinkExpr.Element("from").Add(dlinkExprQry);


            //  XElement firstLevelLinkI = new XElement(firstLevelLink);

            var links = firstLevelLink.Elements().Select(e => new XElement(e)).ToList();

            dlinkExprQry.Add(links);
            string newSelfAlias = "";

            SortedList<string, string> aliasDecode = new SortedList<string, string>();

            foreach (XElement nextLinkI in dlinkExprQry.DescendantsAndSelf().Where(EPredicate.IsQueryOrLinkOrDLinkOrSLink).ToList()) {
                string linkAlias = nextLinkI.Attribute("as").Value;

                nextLinkI.Attribute("as").Value = nextLinkI.Attribute("as").Value + TextConst.Pfx.DubDlinkPush + nextIndex().ToString();

                if (linkAlias == link.Attribute("as").Value)
                {
                    newSelfAlias = nextLinkI.Attribute("as").Value;
                }


                aliasDecode.Add(linkAlias, nextLinkI.Attribute("as").Value);

                foreach (XElement col in dlinkExpr.Descendants("where").Descendants("column").Where(e => getAttrValue(e, "table") == linkAlias).ToArray())
                {
                    col.Attribute("table").Value = nextLinkI.Attribute("as").Value;
                }


            }

            XElement linkCopy = dlinkExprQry.Descendants(TextConst.EName.DLink).FirstOrDefault(e => e.Attribute("as").Value == newSelfAlias);

            if (linkCopy != null)
            {
                foreach (XElement linkCopyCursor in linkCopy.AncestorsAndSelf().ToList())
                {
                    if (linkCopyCursor.Name.LocalName == TextConst.EName.DLink)
                    {
                        dlinkExpr.Element("where").Element("call").Add(linkCopyCursor.Elements(TextConst.EName.Where).Elements());
                        var elink = new XElement(TextConst.EName.ELink);

                        copyAttributes(linkCopyCursor, elink);
                        elink.Add(linkCopyCursor.Elements());
                        elink.Elements(TextConst.EName.Where).Remove();
                        elink.Attributes(TextConst.AName.Pushpred).Remove();
                        linkCopyCursor.ReplaceWith(elink);
                    }

                }
            }







            applyLinks(dlinkExpr, null);







            foreach (XElement col in selColumns)
            {
                XElement dlinkExpr1 = new XElement(dlinkExpr);

                // Может тормозить!!!!!! переписать, наверное
                /*if (col.Attribute("as") != null)
                {
                    dlinkExpr1.SetAttributeValue  ("as", col.Attribute("as").Value);
                }*/
                dlinkExpr1.SetAttrValue(AName.@as, getAliasOrColumn(col));
                XElement col1 = new XElement("column");
                copyAttribute(col, col1, "title");
                copyAttribute(col, col1, "class-title");
                copyAttribute(col, col1, "type");
                copyAttribute(col, dlinkExpr1, "nvl");
                copyAttribute(col, dlinkExpr1, "nullif");
                copyAttribute(col, dlinkExpr1, "mp");
                copyAttribute(col, dlinkExpr1, "agg");
                copyAttribute(col, dlinkExpr1, "format");
                copyAttribute(col, dlinkExpr1, TextConst.AName.CMaster);
                copyAttribute(col, dlinkExpr1, TextConst.AName.CMasterKey);
                col1.SetAttributeValue("table", aliasDecode[col.Attribute(TextConst.AName.Table).Value]);

                if (col.Attribute("dgroup") != null)
                {
                    col1.SetAttributeValue("group", col.Attribute("dgroup").Value);
                }
                col1.SetAttributeValue(AName.column, col.Attribute(AName.column).Value);
                col1.SetAttrValue(AName.@as, getAliasOrColumn(col));
                dlinkExpr1.Element("select").Add(col1);

                col.AddAfterSelf(dlinkExpr1);
                col.Remove();




            }



            /*  foreach (XElement childLink in link.Descendants().Where(e => (new string[] { "dlink", "link" }).Contains(e.Name.LocalName)))
              {
                  colNames.Clear();
                  IEnumerable<XElement> childSelCols = query.Elements().Where(e => (new string[] { "select", "where" }).Contains(e.Name.LocalName)).Descendants("column").Where(
                     e1 => e1.Attribute("table").Value == childLink.Attribute("as").Value
                 ).ToArray();

                  foreach (XElement col in childSelCols)
                  {

                      // Может тормозить!!!!!! переписать, наверное
                      if (col.Attribute("as") != null)
                      {
                          colAlias = col.Attribute("as").Value;
                      }
                      else
                      {

                          XElement colWithAlias = childSelCols.Where(e1 => e1.Attribute("column").Value == col.Attribute("column").Value & e1.Attribute("as") != null).FirstOrDefault();
                          if (colWithAlias != null)
                          {

                              colAlias = colWithAlias.Attribute("as").Value;
                          }
                          else
                          {
                              colAlias = col.Attribute("column").Value;
                          }
                      }

                      if (!colNames.Keys.Contains(colAlias))
                      {

                          XElement col1 = new XElement("column");

                          col1.SetAttributeValue("table", childLink.Attribute("as").Value);
                          col1.SetAttributeValue("group", col.Attribute("dgroup").Value);
                          col1.SetAttributeValue("dgroup", col.Attribute("dgroup").Value);
                          col1.SetAttributeValue("column", col.Attribute("column").Value);
                          col1.SetAttributeValue("as", colAlias);
                          dlinkExpr.Element("select").Add(col1);
                          colNames.Add(colAlias, colAlias);
                      }
                      else
                      {
                          colAlias = colNames[colAlias];
                      }
                      col.SetAttributeValue("column", colAlias);
                      col.SetAttributeValue("table", link.Attribute("as").Value);
                  }


              }

              if (link.Attribute("usenl") != null)
              {
                  string queryMainQueryAlias = query.Elements("from").Elements().First().Attribute("as").Value;
                  string sHint = getAttrValue(query, "hint");
                  sHint += string.Format(" use_nl({0} {1})", queryMainQueryAlias, link.Attribute("as").Value);
                  query.SetAttributeValue("hint", sHint);
              }

              applyLinks(dlinkExpr);
              linkerQuery.AddAfterSelf(dlinkExpr);
            
              */
            //link.Remove();

        }
        /* private static void applyDlinkPushOld(XElement query, XElement link, IEnumerable<VSXElement> scheme)
        {
            XElement firstLevelLink = link.AncestorsAndSelf().Where(e => e.Parent != null).First(e => EPredicate.IsQueryOrTable(e.Parent));
            XElement ownerQuery = firstLevelLink.Parent.Ancestors(TextConst.EName.Query).First();

            XElement linkCursor = link;

            List<XElement> links = new List<XElement>();
            while (linkCursor.Attribute("as").Value != firstLevelLink.Parent.Attribute("as").Value)
            {
                links.Add(linkCursor);
                linkCursor = linkCursor.Parent;
            }

            for (int i = links.Count - 1; i >= 0; i--)
            {
                if (links[i].Name.LocalName != "dlink" && getAttrValue(links[i], "pushpred") != "1")
                {
                    links.Remove(links[i]);
                }
                else
                {
                    break;
                }
            }

            if (links.Count > 0)
            {
                firstLevelLink = links[links.Count - 1];
                links.Remove(links[links.Count - 1]);
            }




            //  links.Clear();

            XElement linkerQuery = firstLevelLink.Parent;
            IEnumerable<XElement> parentColumns;
            IEnumerable<XElement> childColumns;
            //  XElement linkedQuery;
            XElement linkedQueryCall;
            XElement linkerQueryScheme;
            string linkerMainQueryAlias;
            if (firstLevelLink.Name == EName.SLink) {
                linkedQueryCall = makeSlinkExpr(link);
            } else {
              //  linkedQueryCall = schemeRoot.Elements("queries").Elements("query").Elements("from").Elements("query").First(e => getAttrValue(e, "dname") == firstLevelLink.Attribute("name").Value && getAttrValue(e, "name") == linkerQuery.Attribute("name").Value);
              //  linkedQuery = linkedQueryCall.Parent.Parent;
              // 
              // linkerQueryScheme=linkedQuery;
              //
              //   string linkedMainQueryAlias = linkedQuery.Elements("from").Elements().First().Attribute("as").Value;
              //  linkedQueryCall = new XElement(linkedQueryCall);
              //  linkerQueryColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedQueryCall.Attribute("as").Value).ToArray();
              //  linkedQueryColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedMainQueryAlias).ToArray();
                XElement queryScheme = getQueryScheme(firstLevelLink.Attribute("child").Value);
                linkedQueryCall = new XElement(queryScheme.Elements("from").Elements("query").First(e => e.Attribute("as").Value == firstLevelLink.Attribute("field").Value));
                string mainQueryAlias = queryScheme.Elements("from").Elements().First().Attribute("as").Value;



                if (firstLevelLink.Attribute("name").Value == firstLevelLink.Attribute("back").Value)
                {
                    parentColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedQueryCall.Attribute("as").Value).ToArray();
                    childColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == mainQueryAlias).ToArray();

                }
                else
                {
                    childColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == linkedQueryCall.Attribute("as").Value).ToArray();
                    parentColumns = linkedQueryCall.Descendants("column").Where(e => e.Attribute("table").Value == mainQueryAlias).ToArray();
                }
                foreach (XElement col in parentColumns) {
                    col.SetAttrValue(AName.Table, firstLevelLink.Parent.Attribute(AName.As).Value);
                }
                foreach (XElement col in childColumns) {
                    col.SetAttrValue(AName.Table, firstLevelLink.Attribute(AName.As).Value);
                }
            }
            XElement linkedQueryCallForConnect = null;

            if (link.Attribute("recursive") != null)
            {
                linkerQueryScheme = getQueryScheme(link.Attribute("table").Value);
                linkerMainQueryAlias = linkerQueryScheme.Elements("from").Elements().First().Attribute("as").Value;
                linkedQueryCallForConnect = linkerQueryScheme.Elements("from").Elements("query").FirstOrDefault(e => e.Attribute("as").Value == link.Attribute("recursive").Value);

                if (linkedQueryCallForConnect == null)
                {
                    linkedQueryCallForConnect = linkerQueryScheme.Elements("from").Elements("query").FirstOrDefault(e => getAttrValue(e, "dname") == link.Attribute("recursive").Value);
                    linkedQueryCallForConnect = new XElement(linkedQueryCallForConnect);
                    parentColumns = linkedQueryCallForConnect.Descendants("column").Where(e => e.Attribute("table").Value == linkedQueryCallForConnect.Attribute("as").Value).ToArray();
                    childColumns = linkedQueryCallForConnect.Descendants("column").Where(e => e.Attribute("table").Value == linkerMainQueryAlias).ToArray();

                }
                else
                {
                    linkedQueryCallForConnect = new XElement(linkedQueryCallForConnect);
                    childColumns = linkedQueryCallForConnect.Descendants("column").Where(e => e.Attribute("table").Value == linkedQueryCallForConnect.Attribute("as").Value).ToArray();
                    parentColumns = linkedQueryCallForConnect.Descendants("column").Where(e => e.Attribute("table").Value == linkerMainQueryAlias).ToArray();
                }
                foreach (XElement col in parentColumns) {
                    col.SetAttrValue(AName.Table, link.Attribute(AName.As).Value);
                    col.SetAttrValue("prior", "1");
                }
                foreach (XElement col in childColumns) {
                    col.SetAttrValue(AName.Table, link.Attribute(AName.As).Value);
                }
            }
            XElement dlinkExpr = new XElement(EName.Query);
            dlinkExpr.Add(
                 new XElement("select"),
                 new XElement("from"),
                 new XElement("where",
                      new XElement("call", new XAttribute("function", "and"))
                      )
            );
            dlinkExpr.Element("where").Element("call").Add(firstLevelLink.Elements("where").Elements());

            if (link.Attribute("recursive") != null)
            {
                dlinkExpr.Add(
                new XElement("connect",
                         linkedQueryCallForConnect.Elements("call").First()
                         ),

                new XElement("start",
                         linkedQueryCall.Elements("call").First()
                         )
                );
            }
            else
            {
                XElement joinExp = linkedQueryCall.Elements("call").First();
                joinExp.SetAttributeValue("joinexp", "1");
                dlinkExpr.Element("where").Element("call").Add(joinExp);


            }


            if (!dlinkExpr.Element("where").Element("call").Elements().Any())
            {
                dlinkExpr.Element("where").Remove();
            }




            //   dlinkExpr.Add(link.Elements("where"));

            XElement dlinkExprQry = new XElement("query", new XAttribute("name", firstLevelLink.Attribute("table").Value), new XAttribute("as", firstLevelLink.Attribute("as").Value));
            //  dlinkExprQry.Add(firstLevelLink.Elements("withparams"));
            //  dlinkExprQry.Add(firstLevelLink.Elements("link"));

            //     dlinkExprQry.Descendants(TextConst.EName.DLink).Remove();

            dlinkExpr.Element("from").Add(dlinkExprQry);
            //string parentJoinAlias = link.Attribute("as").Value + "_p";
            // XElement parentJoin=new XElement("query",new XAttribute("as",parentJoinAlias));
            //foreach (XElement col in linkedQueryColumns)
            //{
            //    XElement col1 = new XElement(col);
            //    col1.SetAttributeValue("group", "1");
            //    dlinkExpr.Element("select").Add(col1);
            //}
            //  linkCursor = link;
            XElement newLinkCursor = dlinkExprQry;
            //   links = new List<XElement>();
            //  while (linkCursor.Attribute("as").Value != firstLevelLink.Attribute("as").Value)
            //  {
            //      links.Add(linkCursor);
            //      linkCursor = linkCursor.Parent;
            //  }

            XElement firstLevelLinkI = new XElement(firstLevelLink);

            string newSelfAliasForRemove = "";

            foreach (XElement nextLinkI in firstLevelLinkI.Descendants().Where(EPredicate.IsLinkOrDLinkOrSLink).ToList())
            {
                string linkAlias = nextLinkI.Attribute("as").Value;



                nextLinkI.Attribute("as").Value = nextLinkI.Attribute("as").Value + TextConst.Pfx.DubDlinkPush + nextIndex().ToString();

                if (linkAlias == link.Attribute("as").Value)
                {
                    newSelfAliasForRemove = nextLinkI.Attribute("as").Value;
                }

                foreach (XElement col in dlinkExpr.Elements("where").Descendants("column").Where(e => getAttrValue(e, "table") == linkAlias).ToArray())
                {
                    col.Attribute("table").Value = nextLinkI.Attribute("as").Value;
                }

                foreach (XElement col in nextLinkI.Elements("where").Descendants("column").Where(e => getAttrValue(e, "table") == linkAlias).ToArray()) // !!! Добавил эту часть вслепую
                {
                    col.Attribute("table").Value = nextLinkI.Attribute("as").Value;
                }
            }


            // 23/03/2015 Бельченко был безконечный цикл в отчете 25499 из за того , что применялось exdendWhere к pushpred dlink, исключил where из копирования

            dlinkExprQry.Add(firstLevelLinkI.Elements().Where(e => !(new string[] { "where" }).Contains(e.Name.LocalName)));


            dlinkExprQry.Descendants(TextConst.EName.DLink).Where(e => e.Attribute("as").Value == newSelfAliasForRemove).Remove();

            foreach (XElement link1 in firstLevelLink.Elements("link"))
            {

                dlinkExprQry.Add(new XElement(link1));

            }

            foreach (XElement link1 in firstLevelLink.Elements("slink"))
            {

                dlinkExprQry.Add(new XElement(link1));

            }

            for (int i = links.Count - 1; i >= 0; i--)
            {
                linkCursor = links[i];
                XElement newLink = null;

                ///////////


                newLink = new XElement(linkCursor);

                if (newLink.Name.LocalName == "dlink")
                {
                    newLink.Name = "elink";
                    newLink.Attributes("pushpred").Remove();

                }





                foreach (XElement nextLinkI in newLink.Descendants().Where(EPredicate.IsLinkOrDLinkOrSLink).ToList())
                {
                    string linkAlias = nextLinkI.Attribute("as").Value;

                    nextLinkI.Attribute("as").Value = nextLinkI.Attribute("as").Value + TextConst.Pfx.DubDlinkPush + nextIndex().ToString();

                    foreach (XElement col in newLink.Elements("where").Descendants("column").Where(e => getAttrValue(e, "table") == linkAlias).ToArray())
                    {
                        col.Attribute("table").Value = nextLinkI.Attribute("as").Value;
                    }

                    foreach (XElement col in dlinkExpr.Elements("where").Descendants("column").Where(e => getAttrValue(e, "table") == linkAlias).ToArray())
                    {
                        col.Attribute("table").Value = nextLinkI.Attribute("as").Value;
                    }
                }
                foreach (XElement link1 in linkCursor.Elements("link"))
                {

                    newLink.Add(new XElement(link1));

                }

                foreach (XElement link1 in linkCursor.Elements("slink"))
                {

                    newLink.Add(new XElement(link1));

                }


                //foreach (XElement el in linkCursor.Elements("withparams"))
                //{
                //    newLink.Add(new XElement(el));
                //}
                foreach (XElement el in newLink.Elements("where").Elements())
                {
                    dlinkExpr.Element("where").Element("call").Add(new XElement(el));
                }
                newLink.Elements("where").Remove();
                newLinkCursor.Add(newLink);
                newLinkCursor = newLink;

            }



            applyLinks(dlinkExpr, null);

            List<string> tableAliaces = new List<string>();

            tableAliaces.Add(link.Attribute(AName.As).Value);

            IList<XElement> childLinks = link.Elements().Where(EPredicate.IsLinkOrSLink).ToList();
            while (childLinks.Count > 0) {
                foreach (XElement childLink in childLinks) {
                    tableAliaces.Add(childLink.Attribute(AName.As).Value);
                }
                childLinks = childLinks.Elements().Where(EPredicate.IsLinkOrSLink).ToList();
            }

            IEnumerable<XElement> selColumns = getQueryColumns(ownerQuery).Where(e1 => tableAliaces.Contains(e1.Attribute("table").Value)).ToArray();
            foreach (XElement col in selColumns)
            {
                XElement dlinkExpr1 = new XElement(dlinkExpr);

                // Может тормозить!!!!!! переписать, наверное
                // if (col.Attribute("as") != null)
                // {
                //     dlinkExpr1.SetAttributeValue  ("as", col.Attribute("as").Value);
                // }
                dlinkExpr1.SetAttrValue(AName.As, getAliasOrColumn(col));
                XElement col1 = new XElement("column");
                copyAttribute(col, col1, "title");
                copyAttribute(col, col1, "class-title");
                copyAttribute(col, col1, "type");
                copyAttribute(col, dlinkExpr1, "nvl");
                copyAttribute(col, dlinkExpr1, "nullif");
                copyAttribute(col, dlinkExpr1, "mp");
                copyAttribute(col, dlinkExpr1, "agg");
                copyAttribute(col, dlinkExpr1, "format");
                copyAttribute(col, dlinkExpr1, TextConst.AName.CMaster);
                copyAttribute(col, dlinkExpr1, TextConst.AName.CMasterKey);
                col1.SetAttributeValue("table", col.Attribute("table").Value);

                if (col.Attribute("dgroup") != null)
                {
                    col1.SetAttributeValue("group", col.Attribute("dgroup").Value);
                }
                col1.SetAttrValue(AName.Column, col.Attribute(AName.Column).Value);
                col1.SetAttrValue(AName.As, getAliasOrColumn(col));
                dlinkExpr1.Element("select").Add(col1);

                col.AddAfterSelf(dlinkExpr1);
                col.Remove();




            }
        }*/
        /* public static void applyExtensions(XElement query, XElement queryCall)
         {
             if (queryCall != null)
             {
                 if (queryCall.Elements("extension").Count() > 0)
                 {
                     if (query.Element("extensions") == null)
                     {
                         query.Add(new XElement("extensions"));
                        
                     }
                     query.Element("extensions").Add(queryCall.Elements("extension"));
                 }
             }



             foreach (XElement ext in query.Elements("extensions").Elements())
             {
                 applyExtension(query, ext);
             }

         }

         public static void applyExtension( XElement query,XElement extension)
         {
             XElement extensionScheme = getQueryScheme(extension.Attribute("name").Value);
             applyExtension(query, extension, new XElement( extensionScheme));
         }

         public static void applyExtension(XElement query, XElement extension, XElement extensionScheme)
         {
             extensionScheme =(XElement) applyParams(extensionScheme, extension.Element("withparams")).First();

             if (query.Element("select") == null)
             {
                 query.Add(new XElement("select"));
             }
            
           


             query.Element("select").Add(extensionScheme.Elements("select").Elements());
             if (query.Element("from") == null)
             {
                 query.Add(new XElement("from"));
             }
             query.Element("from").Add(extensionScheme.Elements("from").Elements());
            
             if (extensionScheme.Element("where") != null)
             {
                 if (query.Element("where") == null)
                 {
                     query.Add(new XElement("where"));
                 }
                 query.Element("where").Add(extensionScheme.Elements("where").Elements());
             }
        
            
         }*/
        internal static IEnumerable<XElement> getQueryColumns(XElement query)
        {
            IEnumerable<XElement> selColumns = query.Elements().Where(EPredicate.IsSelectOrWhereHavingOrStartOrConnectOrDimensionOrMeasures)
                                                    .Descendants().Where(e1 => EPredicate.IsColumnOrFact(e1) && !e1.Ancestors(EName.query).First().IsAfter(query));
            return selColumns;
        }
        internal static IEnumerable<XElement> getQueryColumnsWithGr(XElement query) // добавил колонки из group в копию ф-ции на всякий случай
        {
            IEnumerable<XElement> selColumns = query
               .Elements().Where(e => EPredicate.IsSelectOrWhereHavingOrStartOrConnectOrDimensionOrMeasures(e) || (e.Name == EName.group))
               .Descendants().Where(e1 => EPredicate.IsColumnOrFact(e1) && !e1.Ancestors(EName.query).First().IsAfter(query));
            return selColumns;
        }
        internal static IEnumerable<XElement> getQueryEditableCalls(XElement query)
        {
            IEnumerable<XElement> selColumns = query.Elements().Where(EPredicate.IsSelectOrWhereHavingOrStartOrConnectOrDimensionOrMeasures)
               .Descendants(EName.call).Where(e1 => e1.Attributes(AName.column_editable) != null && !e1.Ancestors(EName.query).First().IsAfter(query));
            return selColumns;
        }
        private static IEnumerable<XElement> getFieldColumns(XElement field)
        {
            IEnumerable<XElement> selColumns = field.Descendants(EName.column).Where(e => !e.Ancestors(EName.query).First().IsAfter(field));
            return selColumns;
        }
        private static IEnumerable<XElement> getQueryFieldsSel(XElement query)
        {
            IEnumerable<XElement> selColumns = query.DescendantsAndSelf().Where(EPredicate.IsSelectOrDimensionOrMeasures)
                // .Descendants("column").Where(e => !e.Ancestors("query").First().IsAfter(query))
              .Elements();
            return selColumns;
        }
        internal static IEnumerable<XElement> getQueryColumnsSel(XElement query)
        {
            IEnumerable<XElement> selColumns = query.Elements(EName.select).Descendants(EName.column).Where(e => !e.Ancestors(EName.query).First().IsAfter(query));
            return selColumns;
        }
        private static IEnumerable<XElement> getQueryColumnsWhere(XElement query)
        {
            IEnumerable<XElement> selColumns = query.Elements(EName.where).Descendants(EName.column).Where(e => !e.Ancestors(EName.query).First().IsAfter(query));
            return selColumns;
        }
        private static IEnumerable<XElement> getQueryColumnsStart(XElement query)
        {
            IEnumerable<XElement> selColumns = query.Elements(EName.start).Descendants(EName.column).Where(e => !e.Ancestors(EName.query).First().IsAfter(query));
            return selColumns;
        }
        internal static IEnumerable<XElement> getQueryJoinColumns(XElement query)
        {
            IEnumerable<XElement> selColumns = query.Elements(EName.from).Elements(EName.query).Elements(EName.call).Descendants(EName.column).Where(e => !e.Ancestors(EName.query).First().Ancestors(EName.query).First().IsAfter(query));
            return selColumns;
        }
        /*public static void applyDimensions(XElement query)
        {
            if (query.Elements("select").Descendants("dimension").Count() < 1)
            {
                return;
            }

            XElement backbone = new XElement("query", new XAttribute("as", "backbone"), new XElement("union"));




            List<string> names = new List<string>();

            foreach (XElement childQuery in query.Elements("from").Elements("query").ToArray())
            {
                XElement queryExpr = new XElement("query", new XElement("select"), new XElement("from", new XElement(childQuery)), new XElement("call", new XAttribute("function", "and")));
                XElement unQueryExpr = new XElement("query", new XElement("select"), new XElement("from", new XElement(childQuery)));

                queryExpr.SetAttributeValue("as", childQuery.Attribute("as").Value);
                queryExpr.SetAttributeValue("join", "left outer");




                names.Clear();
                foreach (XElement dim in query.Elements().Where(e => (new string[] { "select", "where", "connect", "start" }).Contains(e.Name.LocalName)).Descendants("dimension"))
                {
                    if (!names.Contains(dim.Attribute("name").Value))
                    {

                        string colName = searchAddDimension(childQuery.DescendantsAndSelf().Where(e => e.Attribute("name") != null).First().Attribute("name").Value, dim.Attribute("name").Value);
                        XElement col = new XElement("column", new XAttribute("table", childQuery.Attribute("as").Value), new XAttribute("column", colName), new XAttribute("as", dim.Attribute("name").Value), new XAttribute("group", "1"));
                        queryExpr.Element("select").Add(col);
                        unQueryExpr.Element("select").Add(new XElement(col));
                        XElement call = new XElement("call", new XAttribute("function", "="));
                        call.Add(new XElement("column", new XAttribute("table", childQuery.Attribute("as").Value), new XAttribute("column", dim.Attribute("name").Value)));
                        call.Add(new XElement("column", new XAttribute("table", "backbone"), new XAttribute("column", dim.Attribute("name").Value)));
                        queryExpr.Element("call").Add(call);
                        names.Add(dim.Attribute("name").Value);
                    }

                }
                names.Clear();
                foreach (XElement col in query.Elements().Where(e => (new string[] { "select", "where", "connect", "start" }).Contains(e.Name.LocalName)).Descendants("column").Where(e => e.Attribute("table").Value == childQuery.Attribute("as").Value))
                {
                    if (!names.Contains(col.Attribute("column").Value))
                    {
                        queryExpr.Element("select").Add(new XElement(col));
                        names.Add(col.Attribute("column").Value);
                    }
                }


                backbone.Element("union").Add(unQueryExpr);
                childQuery.AddAfterSelf(queryExpr);
                childQuery.Remove();

            }
            names.Clear();
            foreach (XElement dim in query.Elements().Where(e => (new string[] { "select", "where", "connect", "start" }).Contains(e.Name.LocalName)).Descendants("dimension").ToArray())
            {
                if (!names.Contains(dim.Attribute("name").Value))
                {
                    XElement col = new XElement("column", new XAttribute("table", "backbone"), new XAttribute("column", dim.Attribute("name").Value), new XAttribute("as", dim.Attribute("as").Value), new XAttribute("group", "1"));
                    dim.AddAfterSelf(col);
                    dim.Remove();
                    names.Add(dim.Attribute("name").Value);
                }

            }


            query.Elements("select").Descendants("column").Attributes("group").Remove();

            query.Elements("from").Elements().First().AddBeforeSelf(backbone);

            string s = "";
        }
        */
        private static string cumulNextPfx = TextConst.Pfx.CumulNext;
        private static string cumulFirstPfx = "_fst";
        private static string bbAlias = "backbone";
        private static void applyQube(XElement query, XElement rep, XElement inPars, XElement queryCall)
        {
            XElement fact = query.Descendants(TextConst.EName.Fact).FirstOrDefault();

            if (fact == null)
            {
                fact = query.Descendants(TextConst.EName.Qube).FirstOrDefault();
            }

            if (fact != null)
            {
                //if (queryCall != null && queryCall.Ancestors(TextConst.EName.Pivot).Any())
                //{

                //}



                XElement query1 = null;

                if (inPars != null)
                {
                    query1 = (XElement)applyParams(query, inPars, false).FirstOrDefault();
                }
                else
                {
                    //query1 = new XElement(query);
                    //query1.Descendants(TextConst.EName.UseParam).ToList().ForEach(e=>e.ReplaceWith(new XElement(TextConst.EName.Undefined)));
                    query1 = (XElement)applyParams(query, inPars, false).FirstOrDefault();
                }
                XElement qExtraContentWhere = null;
                if (queryCall != null)
                {
                    //qExtraContentWhere = queryCall.Elements(TextConst.EName.QubeContent).Elements(TextConst.EName.Where).FirstOrDefault();
                    var ccc = queryCall.AncestorsAndSelf().FirstOrDefault(e => e.Elements(TextConst.EName.QubeContent).Any());
                    if (ccc != null)
                    {
                        qExtraContentWhere = ccc.Elements(TextConst.EName.QubeContent).Elements(TextConst.EName.Where).FirstOrDefault();
                    }

                }
                if (qExtraContentWhere != null)
                {
                    var xqube = query1.Elements(TextConst.EName.From).Elements(TextConst.EName.Qube).FirstOrDefault();
                    if (xqube != null)
                    {
                        var qwhere = xqube.Elements(TextConst.EName.Where).FirstOrDefault();
                        if (qwhere == null)
                        {
                            qwhere = new XElement(TextConst.EName.Where);
                            xqube.Add(qwhere);
                            // qwhere.Add(qExtraContentWhere.Elements());
                        }
                        qExtraContentWhere.Descendants().Where(e => getAttrValue(e, TextConst.AName.DontPush) == "1").Remove();

                        qwhere.Add(qExtraContentWhere.Elements());
                        //qExtraContentWhere.Remove();

                        //foreach (XElement el in qExtraContentWhere.Elements().ToList())
                        //{
                        //    qwhere.Add(expression(el, null));
                        //}

                    }
                }




                fact = query1.Descendants(TextConst.EName.Fact).FirstOrDefault();
                var undfList = query1.Descendants(TextConst.EName.Undefined).ToList();
                var undfList2 = query1.Descendants(TextConst.EName.Const).Where(e => e.Value == Cmn.undefinedString).ToList();
                undfList.AddRange(undfList2);
                foreach (XElement undf in undfList)
                {
                    var optCall = undf.Ancestors(TextConst.EName.Call).FirstOrDefault(e => Cmn.GetAttrValue(e, TextConst.AName.Optional) == TextConst.AVBool.True);
                    if (optCall != null && optCall.Parent != null)
                    {
                        optCall.Remove();
                    }
                }


                var owthers = query1.Descendants(TextConst.EName.Call).Where(e => Cmn.GetAttrValue(e, TextConst.AName.UseOnlyWithOther) == TextConst.AVBool.True).ToList();
                foreach (XElement call in owthers)
                {
                    if (call.Parent != null)
                    {
                        if (call.Parent.Elements().Count() == 1)
                        {
                            if (call.Parent.Parent != null)
                            {
                                call.Parent.Remove();
                            }

                        }
                    }
                }


                var andToDel = query1.Descendants(TextConst.EName.Where).Descendants(TextConst.EName.Call).Where(e => e.Attribute(TextConst.AName.Function).Value == TextConst.AVFunction.And && !e.Elements().Any()).ToList();

                while (andToDel.Count != 0) {
                    andToDel.Remove();
                    andToDel = query1.Descendants(TextConst.EName.Where).Descendants(TextConst.EName.Call).Where(e => e.Attribute(TextConst.AName.Function).Value == TextConst.AVFunction.And && !e.Elements().Any()).ToList();
                }

                foreach (XElement xwhere in query1.Descendants(TextConst.EName.Where).ToList())
                {
                    var els = xwhere.Elements().ToList();
                    if (xwhere.Elements().Count() > 1) {
                        els.Remove();
                        xwhere.Add(new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.And)));
                        xwhere.Elements().First().Add(els);
                    } else if (els.Count == 0) {
                        xwhere.Remove();
                    }
                }


                var dimsetToDel = new List<string>();
                foreach (XElement xcall in query1.Descendants(TextConst.EName.DimSet).Elements(EName.where)
                    .Descendants(EName.call).Where(c => getAttrValue(c, TextConst.AName.ClientCalulation) ==
                        TextConst.AVBool.True).ToList())
                {
                    if (!xcall.Descendants(TextConst.EName.UseParam).Any())
                    {
                        var vcall = VClientCalculations.ParseCall(xcall);


                        var val = vcall.Evaluate();

                        if (val is bool)
                        {
                            if ((bool)val)
                            {
                                xcall.ReplaceWith(new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.True)));

                            }
                            else
                            {


                                List<string> deletedLinks = new List<string>();
                                var par = xcall.Parent;

                                while (par.Name.LocalName != TextConst.EName.DimSet)
                                {
                                    par = par.Parent;
                                }
                                var dsName = Cmn.GetAttrValue(par, TextConst.AName.As);
                                if (!dimsetToDel.Contains(dsName))
                                {
                                    dimsetToDel.Add(dsName);
                                }



                            }

                        }
                    }
                }

                if (dimsetToDel.Count != 0)
                {
                    query1 = VSXElement.Get(new XElement(query1));
                }
                foreach (var dsName in dimsetToDel)
                {
                    var ds = ((VQuery)query1).GetQubeElement().GetDimSet(dsName);


                    var commonLinksNames = ds.Parent.Elements(TextConst.EName.Link).Select(l => Cmn.GetAttrValueNvl(l, TextConst.AName.As, TextConst.AName.Name)).Distinct().ToList();

                    var delLinks = ds.Elements(TextConst.EName.Link).Where(l => !commonLinksNames.Contains(Cmn.GetAttrValueNvl(l, TextConst.AName.As, TextConst.AName.Name))).ToList();
                    List<string> deletedLinks = new List<string>();
                    foreach (var link in delLinks)
                    {
                        foreach (var link1 in link.DescendantsAndSelf())
                        {
                            if ((new string[] { TextConst.EName.Link, TextConst.EName.ELink }).Contains(link1.Name.LocalName))
                            {
                                deletedLinks.Add(Cmn.GetAttrValueNvl(link1, TextConst.AName.As, TextConst.AName.Name));
                            }

                        }
                    }

                    var cols = getQueryColumnsWithGr(query1).ToList();

                    var colsToDel = cols.Where(e => e.Name.LocalName == TextConst.EName.Column && deletedLinks.Contains(Cmn.GetAttrValue(e, TextConst.EName.Table))).ToList();
                    var factsToDel = cols.Where(e => e.Name.LocalName == TextConst.EName.Fact && Cmn.GetAttrValue(e, TextConst.EName.Table) == dsName);

                    colsToDel.AddRange(factsToDel);
                    var exprTmpl = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Dummy), new XElement(TextConst.EName.Const, new XText("null")));
                    foreach (VSXElement col in colsToDel.SelectAsArray(VSXElement.Get)) {
                        var expr = new XElement(exprTmpl);

                        copyAttributes(col, expr, columnAttributesNames);
                        expr.CopyAttributes(col.Attributes().Where(APredicate.IsAdditionalAttribute));

                        expr.SetAttributeValue(TextConst.AName.Type, col.XDataType());
                        expr.SetAttributeValue(TextConst.AName.As, col.XName);
                        col.ReplaceWith(expr);

                    }
                    ds.Remove();// удаление dimset
                }


                //bool isGrouping = false;
                VQuery tquery = null;
                if (query1.Descendants(TextConst.AName.Link).Any(e => getAttrValue(e, TextConst.AName.IsTree) != ""))
                {
                    tquery = VSXElement.Get<VQuery>(new XElement(query1));
                    //tquery.environment = XmlReports.Environment;

                    VQubeUtils.ProcessQubeTrees(tquery);

                    query1 = tquery;
                }





                if (query1.Element(TextConst.EName.Grouping) != null)
                {

                    applySelfGrsets(query1, queryCall);

                    //isGrouping = true;
                }
                fact = query1.Descendants(TextConst.EName.Fact).FirstOrDefault();
                if (fact == null)
                {
                    fact = query1.Descendants(TextConst.EName.Qube).FirstOrDefault();
                }
                var query2 = fact.Ancestors(TextConst.EName.Query).First(e => e.Element(TextConst.EName.Select) != null); //!! e.Element(TextConst.EName.Select)!=null не совсем корректное условие - цель обработать факты под joinom
                var query3 = new XElement(query2);


                List<string> usednames = null;

                bool check1 = false;
                if (rep != null)
                {
                    var repQry = rep.Element(TextConst.EName.Queries).Descendants(TextConst.EName.Query).FirstOrDefault(e => Cmn.GetAttrValue(e, TextConst.AName.Name) == Cmn.GetAttrValue(query3, TextConst.AName.Name));
                    if (repQry != null)
                    {
                        if (repQry.Element(TextConst.EName.Columns) != null)
                        {
                            usednames = repQry.Element(TextConst.EName.Columns).Descendants(TextConst.EName.Column).Select(c => getAttrValue(c, TextConst.AName.Name)).Distinct().ToList();

                        }
                    }
                }
                else
                {

                    //  Бельченко 20171122, вернул, немного исправил, ТЕСТИРОВАТЬ
                    // Бельченко 20161103 убираю - сломались отчеты с ГПЗ, 33654 и др
                    if (Cmn.GetAttrValue(query, TextConst.AName.Materialize) != TextConst.AVBool.True)
                    {
                        if (queryCall != null) // Бельченко 20161031 Это новое, не проверенное , используется только для демонстрации поиска, можно убрать если что
                        {
                            if (queryCall.Parent != null)
                            {
                                if (queryCall.Parent.Parent != null)
                                {
                                    usednames = new List<string>();
                                    var cols = getQueryColumnsWithGr(queryCall.Parent.Parent).ToList();
                                    //if (!cols.Where(c => getAttrValue(c, TextConst.AName.Column) == TextConst.AVColumn.All).Any())
                                    //{
                                        var cols1 = getQueryJoinColumns(queryCall.Parent.Parent).ToList();
                                        cols.AddRange(cols1);
                                        foreach (var col in cols)
                                        {
                                            var cname = getAttrValue(col, TextConst.AName.Column);

                                            usednames.Add(cname);
                                        }

                                        usednames = usednames.Distinct().ToList();
                                        check1 = true;
                                    //}
                                }
                            }
                        }
                    }
                }

                if (usednames != null)
                {
                    //List<XElement> cols = null;
                    //List<XElement> cols1 = null;
                    //if (check1)
                    //{
                    //    cols = getQueryColumnsWithGr(queryCall.Parent.Parent).ToList();
                    //    cols1 = getQueryJoinColumns(queryCall.Parent.Parent).ToList();
                    //    cols.AddRange(cols1);
                    //}

                    foreach (var el in query3.Elements(EName.select).Elements().Where(e => !usednames.Contains(getAliasOrColumn(e))).ToList()) {

                        if (!check1 || Cmn.GetAttrValue(el, TextConst.AName.Group) == "")// химия, нужно выкидывать но не всегда, наличие группировки косвенный призак
                        {
                            bool rem = true;
                            if (check1) {
                                string alias = getAliasOrColumn(el);
                                var useByThis = query1.Descendants(EName.column).Where(e => Cmn.GetAttrValue(e, TextConst.AName.Table) == TextConst.AVTable.Ths && Cmn.GetAttrValue(e, TextConst.AName.Column) == alias);
                                if (useByThis.Any()) {
                                    // нужно бы проверять используется ли колонка определенноая через this, но это сложновато пока так
                                    // + сейчас проверка по всему запросу с подзапросами, это не правильно.
                                    rem = false;
                                }
                            }

                            if (rem)
                            {
                                el.Remove();
                                if (check1)
                                {

                                }
                            }

                        }
                    }

                    foreach (var el in query3.Elements(TextConst.EName.Where).Descendants(TextConst.EName.Column).Where(c => getAttrValue(c, TextConst.AName.Table) == TextConst.AVTable.Ths).Where(e =>
                        !usednames.Contains(e.Attribute(TextConst.AName.Column).Value)
                        ).ToArray())
                    {


                        if (!check1)
                        {
                            el.Remove();

                        }
                    }

                }
                var factPivCols = query3.Descendants(TextConst.EName.Fact).Where(e => e.Elements(TextConst.EName.WithParams).Descendants(TextConst.AName.Column).Any(c => getAttrValue(c, TextConst.AName.Table) == "dim")).ToList();
                if (factPivCols.Count != 0) {
                    isProcessingPivots = true;
                    foreach (XElement col in factPivCols)
                    {
                        processingPivot(col, true, null);
                    }
                    isProcessingPivots = false;
                }

                query3.Elements(TextConst.EName.Content).Remove();
                tquery = VSXElement.Get<VQuery>(query3);
                //tquery.environment = XmlReports.Environment;

                if (XmlReports.IsDeveloperMode())
                {
                    VExceptionController.BeginProcessingElement(query);
                }
                var xquery = VQubeUtils.CreateQubeQuery(tquery, rep);



                PreCompileQuery(xquery, true);


                if (XmlReports.IsDeveloperMode())
                {
                    VExceptionController.EndProcessingElement(query);
                }

                query2.Elements().Remove();
                query2.Add(xquery.Elements());

                query.Elements().Remove();
                query.Add(query1.Elements());
                Cmn.CopyAttributeNoReplace(xquery, query, TextConst.AName.Order);
                Cmn.CopyAttributeNoReplace(query1, query, TextConst.AName.Order);
                //copyAttribute(query1, query, TextConst.AName.Order);// при automerge сортировка устанавливается при обработке куба
                copyAttribute(query1, query, "haskeys");

            }
            
        }

        internal static SortedList<string, XElement> pivotQueries = null;
        private static void resetPivotQueriesList()
        {
            pivotQueries = new SortedList<string, XElement>();
        }
        private static void pivotDummies(XElement query, XElement queryCall, XElement rep, XElement inPars)
        {
            var pivots = query.Descendants(TextConst.EName.Pivot);

            foreach (XElement pivotQuery in pivots.Elements(TextConst.EName.Query).Where(e => e.Attribute("is-dummy") == null).ToList())
            {
                pivotQuery.AddAfterSelf(new XElement(TextConst.EName.Query
                    , new XElement(TextConst.EName.Select)
                     , new XElement(TextConst.EName.From)
                     , new XAttribute("is-dummy", "1")
                    ));
                var dimname = pivotQuery.Parent.Parent.Attribute(TextConst.AName.Dimname).Value;
                if (!pivotQueries.ContainsKey(dimname))
                {
                    var q1 = new XElement(pivotQuery);
                    q1.AddFirst(query.Elements(TextConst.EName.Params));
                    q1 = applyParams(q1, inPars, false).First() as XElement;
                    pivotQueries.Add(dimname, q1);
                }
                pivotQuery.Remove();
            }
        }
        private static void applyDimensions(XElement query, XElement queryCall, XElement rep, XElement inPars, bool newOnly = false)
        {
            if (query.Descendants(TextConst.EName.Fact).Any() || query.Descendants(TextConst.EName.Qube).Any())
            {
                applyQube(query, rep, inPars, queryCall);
                return;
            }
            else if (query.Elements(TextConst.EName.Grouping).Any())
            {
                applySelfGrsets(query, queryCall);
                return;
            }
            if (newOnly)
            {
                return;
            }


            XElement qubeCol = query.Descendants("column").FirstOrDefault(e => getAttrValue(e, "table") == "*" && e.Parent.Name.LocalName == "select");


            if (qubeCol == null)
            {
                return;
            }


            if (getAttrValue(query, "stored") != "" && useRepositories)
            {
                return;
            }

            var query1 = qubeCol.Parent.Parent;


            if (query1 != query)
            {
                query = query1;
                queryCall = query;
            }

            //if (query.Elements("select").Descendants("column").Where(e => getAttrValue(e, "table") == "*").Count() < 1)
            //{
            //    return;
            //}

            string cstyle = getAttrValue(query, "cstyle");

            if (cstyle == "")
            {
                cstyle = "join";
            }
            //cstyle=join ,с помощью union формируется запрос backbone содержащий все коды , к нему с поомощю join добавляются запросы с фактами
            //cstyle=union ,с помощью union формируется запрос backbone он же содержит все факты, отсутствующие факты в запросах замещаются null


            XElement columnsUser = null;
            string alias = "";
            bool isInReport = false;
            if (queryCall.Attribute("as") != null)
            {
                if (queryCall.Parent.Parent.Name.LocalName == "report")
                {

                    columnsUser = queryCall;
                    isInReport = true;
                }
                else
                {
                    if (queryCall.AncestorsAndSelf("query").Any(e => getAttrValue(e, "materialize") == "2"))
                    {
                        return;
                    }
                    columnsUser = queryCall.Parent.Parent;
                    alias = queryCall.Attribute("as").Value;
                }
            }
            List<string> usedColsNames = null;
            List<string> usedColsNamesJoin = null;
            bool useAll = false;
            List<string> usedMasterNames = new List<string>();
            IList<string> usedColsNames2 = Array.Empty<string>();
            if (columnsUser != null)
            {
                if (!isInReport)
                {
                    usedColsNames = getQueryColumns(columnsUser).Where(e => e.Attribute("table").Value == alias).Select(e1 => e1.Attribute("column").Value).ToList();
                    usedColsNamesJoin = getQueryJoinColumns(columnsUser).Where(e => e.Attribute("table").Value == alias).Select(e1 => e1.Attribute("column").Value).ToList();
                }
                else
                {
                    usedColsNames = columnsUser.Elements("columns").Elements().Select(e1 => e1.Attribute("name").Value).ToList();
                    if (usedColsNames.Count == 0)
                    {
                        useAll = true;
                    }
                    usedColsNamesJoin = new List<string>();
                }
            }
            else
            {
                useAll = true;
            }

            if (!useAll)
            {
                //!!! запутался, заплатки, привести в порядок
                List<XElement> usedCols = query.Elements("select").Elements().Where(e1 => usedColsNames.Contains(e1.Attribute("as").Value) || usedColsNamesJoin.Contains(e1.Attribute("as").Value)).ToList();
                List<XElement> usedCols1 = usedCols.ToList();
                usedMasterNames = usedCols.Where(e => e.Attribute("master") != null).Select(e1 => e1.Attribute("master").Value).ToList();
                usedMasterNames.AddRange(usedCols.DescendantsAndSelf().Where(e => e.Attribute("cumulate") != null).Select(e1 => e1.Attribute("cumulate").Value).ToList());
                //foreach (string s1 in usedCols.Where(e => e.Attribute("window") != null).Select(e1 => e1.Attribute("window").Value).ToList())
                //{
                //    usedMasterNames.AddRange(s1.Split(',').ToList());
                //}

                List<string> usedThisNames = new List<string>();
                usedThisNames.AddRange(usedColsNames);
                int c1 = 0;
                while (c1 != usedCols1.Count) {
                    c1 = usedCols1.Count;
                    List<string> usedThisNames1 = usedCols1.DescendantsAndSelf("column").Where(e => e.Attribute("table").Value == "this").Where(e2 => !usedThisNames.Contains(e2.Attribute("column").Value)).Select(e1 => e1.Attribute("column").Value).ToList();
                    usedThisNames.AddRange(usedThisNames1);
                    usedCols1 = query.Elements("select").Elements().Where(e1 => usedThisNames.Contains(e1.Attribute("as").Value) || usedColsNamesJoin.Contains(e1.Attribute("as").Value)).ToList();

                }
                usedCols = usedCols1;
                usedColsNames = usedThisNames;
                usedColsNames2 = usedCols.Attributes(AName.@as).Select(APredicate.AttributeValue).ToList();
            } else {
                usedColsNames2 = query.Elements(EName.select).Elements().Attributes(AName.@as).Select(APredicate.AttributeValue).ToList();
            }
            List<string> usedQueriesNames = query.Elements("select").Elements("column").Where(e1 => usedColsNames2.Contains(e1.Attribute("as").Value) || usedMasterNames.Contains(e1.Attribute("as").Value)).Select(e => e.Attribute("table").Value).ToList();

            IEnumerable<XElement> usedCmnQueries = query.Elements("push").Elements("from").Descendants().Where(EPredicate.IsQueryOrLinkOrDLinkOrSLink).Where(e => usedQueriesNames.Contains(getAttrValue(e, "as"))).Select(e1 => e1.AncestorsAndSelf("query").First());
            List<string> usedColsNames3 = usedCmnQueries.Descendants("column").Where(e => e.Attribute("table").Value == "*").Select(e1 => e1.Attribute("column").Value).ToList();

            List<string> usedColsNames4 = query.Elements("push").Elements("where").Descendants("column").Where(e => e.Attribute("table").Value == "*").Select(e1 => e1.Attribute("column").Value).ToList();
            usedColsNames3.AddRange(usedColsNames4);



            query.Elements("select").Elements().Where(e1 => !usedColsNames2.Contains(e1.Attribute("as").Value) && !usedMasterNames.Contains(e1.Attribute("as").Value) && !usedColsNames3.Contains(getAttrValue(e1, "column"))).Remove();

            usedQueriesNames = query.Elements("select").Elements("column").Select(e => e.Attribute("table").Value).ToList();

            //   if (query.Elements("select").Elements("column").Where(e => e.Attribute("table").Value == "*" && aggFuncsNames.Contains(e.Attribute("group").Value)).Count() == 0)
            //   {
            //!!! НЕ ГОДИТСЯ ПРЕДУСМОТРЕТЬ СПЕЦ. ПРИЗНАК, ВСЕГДА ОСТАВЛЯТЬ НЕЛЬЗЯ
            query.Elements("from").Elements("query").Where(e1 => !usedQueriesNames.Contains(e1.Attribute("as").Value) && getAttrValue(e1, "fixed") != "1").Remove();
            //  }
            // }




            foreach (XElement col in query.Elements("column").Where(e => getAttrValue(e, "group") == "1").ToList())
            {
                // col.SetAttributeValue("fixed", "1");
            }

            List<string> cumulNames = new List<string>();

            foreach (XElement col in query.Descendants(EName.column).Where(e => e.Attribute("cumulate") != null).ToList()) {
                string timeline = query.Elements(EName.select).Elements().First(e => e.Attribute("as").Value == col.Attribute("cumulate").Value).AttrOrDefault("timeline", string.Empty);
                List<string> dimColsNames = query.Elements(EName.select).Elements().Where(e => getAttrValue(e, "group") == "1" && e.Attribute("as").Value != col.Attribute("cumulate").Value && (timeline == "" || getAttrValue(e, "timeline") != timeline)).Select(e1 => e1.Attribute("as").Value).ToList();
                XElement[] dimCols = dimColsNames.SelectAsArray(e => new XElement(EName.column, new XAttribute(AName.table, TextConst.AVTable.Ths), new XAttribute(AName.column, e)));
                XElement colCopy = new XElement(col);
                colCopy.RemoveAttribute(AName.cumulate);
                colCopy.RemoveAttribute(AName.@as);
                string aggFunc = col.AttrOrEmpty(AName.agg);
                if (string.IsNullOrEmpty(aggFunc)) {
                    aggFunc = TextConst.AVFunction.Sum;
                }
                XElement overExpr = new XElement(EName.call,
                        new XAttribute(AName.function, TextConst.AVFunction.Over),
                        new XElement(EName.call,
                            new XAttribute(AName.function, aggFunc),
                            colCopy
                            ),
                       new XElement(EName.call,
                            new XAttribute(AName.function, TextConst.AVFunction.PartitionBy),
                              new XElement(EName.@const, "1"),
                                dimCols
                            ),
                       new XElement(EName.call,
                            new XAttribute(AName.function, TextConst.AVFunction.OrderBy2),
                                new XElement(EName.call,
                                    new XAttribute(AName.function, "asc nulls first"),
                                         new XElement(EName.column, new XAttribute(AName.table, TextConst.AVTable.Ths), new XAttribute(AName.column, col.Attribute(AName.cumulate).Value))
                                         )
                                )
                    );

                copyAttributes(col, overExpr, new string[] { "as" });
                col.ReplaceWith(overExpr);

                if (!cumulNames.Contains(col.Attribute("cumulate").Value))
                {
                    cumulNames.Add(col.Attribute("cumulate").Value);
                    XElement lastExpr = new XElement("call",
                       new XAttribute("function", "over"),
                       new XElement("call",
                            new XAttribute("function", "lead"),
                                 new XElement("column", new XAttribute("table", "this"), new XAttribute("column", col.Attribute("cumulate").Value), new XAttribute("group", "max"))
                            )
                       , new XElement("call",
                            new XAttribute("function", "partition by"),
                            new XElement(TextConst.EName.Const, "1"),
                                dimCols
                            )
                       , new XElement("call",
                            new XAttribute("function", "order by 2"),
                        // new XElement("call", new XAttribute("function", "nulls last"),
                        //new XElement("call", new XAttribute("function", "desc"),
                                    new XElement("column", new XAttribute("table", "this"), new XAttribute("column", col.Attribute("cumulate").Value))
                        //)
                        // )
                            )

                    );
                    lastExpr.SetAttributeValue("as", col.Attribute("cumulate").Value + cumulNextPfx);
                    query.Element("select").Add(lastExpr);
                }

            }
            XElement backbone = new XElement("query", new XAttribute("as", bbAlias), new XElement("union"));
            if (cstyle == "join")
            {
                backbone.Element("union").Add(new XAttribute("all", "0"));
            }
            IEnumerable<XElement> cmnFrom = query.Elements("push").Elements("from").Elements();
            XElement buffer;

            List<string> names = new List<string>();

            List<string> childQueryNames = query.Elements("from").Elements("query").Select(e => e.Attribute("as").Value).ToList();

            foreach (XElement childQuery in query.Elements("from").Elements("query").ToArray())
            {
                XElement unQueryExpr = new XElement("query", new XElement("select"), new XElement("from", new XElement(childQuery)));
                XElement queryExpr = null;

                if (cstyle == "join")
                {
                    queryExpr = new XElement("query", new XElement("select"), new XElement("from", new XElement(childQuery)), new XElement("call", new XAttribute("function", "and")));



                    queryExpr.SetAttributeValue("as", childQuery.Attribute("as").Value);
                    queryExpr.SetAttributeValue("join", "left outer");


                    queryExpr.Add(query.Elements("push").Elements("where").Select(e => new XElement(e)));



                    foreach (XElement wc in queryExpr.Elements("where").Descendants("column").Where(e => e.Attribute("table").Value == "*"))
                    {
                        wc.Attribute("table").Value = childQuery.Attribute("as").Value;
                    }
                }

                unQueryExpr.Add(query.Elements("push").Elements("where").Select(e => new XElement(e)));

                bool hasNonExistent = false;

                foreach (XElement wc in unQueryExpr.Elements("where").Descendants("column").Where(e => e.Attribute("table").Value == "*").ToArray())
                {
                    wc.Attribute("table").Value = childQuery.Attribute("as").Value;
                    if (wc.Ancestors("call").FirstOrDefault(e => getAttrValue(e, "selective") == "1") != null)
                    {

                        XElement qry = null;
                        if (childQuery.Attribute("name") != null)
                        {
                            qry = getQueryScheme(childQuery.Attribute("name").Value);
                        }
                        else
                        {
                            qry = childQuery;
                        }

                        XElement col = qry.Element("select").Elements().FirstOrDefault(e => getAttrValue(e, "as") == wc.Attribute("column").Value);

                        if (col == null)
                        {
                            wc.ReplaceWith(new XElement("nonexistent"));
                            hasNonExistent = true;
                        }

                    }

                }
                if (hasNonExistent)
                {
                    foreach (XElement el in unQueryExpr.Elements("where").Descendants("call").Where(e => getAttrValue(e, "selective") == "1" && e.Descendants("nonexistent").FirstOrDefault() != null).ToArray())
                    {

                        el.Remove();
                    }
                }


                buffer = new XElement("buf", cmnFrom);
                foreach (XElement col in buffer.Descendants("column").Where(e => e.Attribute("table").Value == "*"))
                {
                    col.Attribute("table").Value = childQuery.Attribute("as").Value;
                }

                if (cstyle == "join")
                {
                    queryExpr.Element("from").Add(buffer.Elements());
                }
                unQueryExpr.Element("from").Add(buffer.Elements());

                names.Clear();



                foreach (XElement dim in query.Elements("select").Elements("column").Where(e => e.Attribute("table").Value == "*").ToArray())
                {
                    if (!names.Contains(dim.Attribute("column").Value))
                    {

                        // string colName = searchAddDimension(childQuery.DescendantsAndSelf().Where(e => e.Attribute("name") != null).First().Attribute("name").Value, dim.Attribute("name").Value);
                        XElement col = new XElement("column", new XAttribute("table", childQuery.Attribute("as").Value), new XAttribute("column", dim.Attribute("column").Value), /*new XAttribute("as", dim.Attribute("as").Value), */new XAttribute("group", "1"), new XAttribute("fixed", "1"), new XAttribute("virtual", "1"));
                        // col.SetAttributeValue("group", "1");
                        unQueryExpr.Element("select").Add(new XElement(col));

                        if (cstyle == "join")
                        {
                            queryExpr.Element("select").Add(col);

                            XElement call = new XElement("call", new XAttribute("function", "=nvl"));
                            call.Add(new XElement("column", new XAttribute("table", childQuery.Attribute("as").Value), new XAttribute("column", dim.Attribute("column").Value)));
                            call.Add(new XElement("column", new XAttribute("table", bbAlias), new XAttribute("column", dim.Attribute("column").Value)));
                            queryExpr.Element("call").Add(call);
                        }



                        names.Add(dim.Attribute("column").Value);
                    }

                }

                //if (queryExpr.Element("select").Elements().Count()==0)
                //{
                //    queryExpr.Element("call").Add(new XElement("call", new XAttribute("function", "true")));
                //    queryExpr.Element("select").Add(new XElement("const")
                //}
                if (cstyle == "join")
                {

                    if (!queryExpr.Element("select").Elements().Any())
                    {
                        queryExpr.Element("call").Remove();
                        queryExpr.SetAttributeValue("join", "cross");

                    }
                }

                names.Clear();


                if (cstyle == "join")
                {
                    foreach (XElement col in getQueryColumns(query).Where(e => e.Attribute("table").Value == childQuery.Attribute("as").Value))
                    {
                        if (!names.Contains(col.Attribute("column").Value))
                        {
                            XElement col1 = new XElement(col);
                            col1.Attributes("as").Remove();
                            queryExpr.Element("select").Add(col1);
                            names.Add(col.Attribute("column").Value);
                        }
                    }
                }


                if (cstyle == "union")
                {
                    foreach (XElement col in getQueryColumns(query).Where(e => childQueryNames.Contains(e.Attribute("table").Value)))
                    {
                        XElement col1 = null;


                        if (!names.Contains(col.Attribute("table").Value + "-" + col.Attribute("as").Value))
                        {
                            if (col.Attribute("table").Value == childQuery.Attribute("as").Value)
                            {
                                col1 = new XElement(col);
                                // col1.Attributes("as").Remove();

                            }
                            else
                            {
                                col1 = new XElement("const");

                                col1.Add(new XText("null"));
                            }
                            col1.SetAttributeValue("as", col.Attribute("as").Value);
                            names.Add(col.Attribute("table").Value + "-" + col.Attribute("column").Value);
                            unQueryExpr.Element("select").Add(col1);
                        }
                    }
                    unQueryExpr.Elements("select").Elements().Attributes("group").Remove();
                }


                backbone.Element("union").Add(unQueryExpr);




                if (cstyle == "join")
                {
                    childQuery.AddAfterSelf(queryExpr);

                }
                childQuery.Remove();

            }



            foreach (XElement dim in query.Elements("select").Elements("column").Where(e => e.Attribute("table").Value == "*").ToArray())
            {

                XElement col = new XElement(dim);
                col.SetAttributeValue("table", bbAlias);
                // col.SetAttributeValue("fixed", "1");
                col.SetAttributeValue("removeable", "0");
                dim.AddAfterSelf(col);
                dim.Remove();


            }



            if (cstyle == "union")
            {

                foreach (XElement dim in getQueryColumns(query).Where(e => childQueryNames.Contains(e.Attribute("table").Value)).ToArray())
                {
                    //   if (dim.Attribute("as")!=null){
                    XElement col = new XElement(dim);
                    col.SetAttributeValue("table", bbAlias);
                    col.SetAttributeValue("column", col.Attribute("as").Value);
                    dim.AddAfterSelf(col);
                    dim.Remove();



                }
            }

            //query.Elements("select").Descendants("column").Attributes("group").Remove();


            if (query.Elements("from").Elements().FirstOrDefault() != null)
            {
                query.Elements("from").Elements().First().AddBeforeSelf(backbone);
            }
            else
            {
                query.Element("from").Add(backbone);
            }

            buffer = new XElement("buf", cmnFrom);
            foreach (XElement col in buffer.Descendants("column").Where(e => e.Attribute("table").Value == "*"))
            {
                col.Attribute("table").Value = bbAlias;
            }
            query.Element("from").Add(buffer.Elements());

            query.Elements("push").Remove();
            //  string s = "";
        }



        /* public static string searchAddDimension(string queryName,string dimensionName)
         {
             XElement query = schemeRoot.Elements("queries").Elements("query").Where(e => e.Attribute("name").Value == queryName).First();
             XElement col = query.Elements("select").Elements().Where(e => getAttrValue(e, "dimension") == dimensionName).FirstOrDefault();
             string colName;
             if (col != null)
             {
                 colName = col.Attribute("as").Value;
             }
             else
             {
                 string nextQueryName = query.Elements("from").Elements("query").First().Attribute("name").Value;
                 colName = searchAddDimension(nextQueryName, dimensionName);
                 query.Element("select").Add(new XElement("column", new XAttribute("table", query.Elements("from").Elements().First().Attribute("as").Value)),new XAttribute("column",colName),new XAttribute("dimension",dimensionName));
             }
             return colName;
         }*/
        private static void addQuerySysColumns(XElement query)
        {

            var tbl = query.Elements(TextConst.EName.From).Elements(TextConst.EName.Table).FirstOrDefault();
            if (tbl != null)
            {
                //if (tbl.Attribute(TextConst.AName.FromTemp) == null)
                //{
                var sel = query.Elements(TextConst.EName.Select).FirstOrDefault();
                if (sel != null)
                {
                    if (getAttrValue(tbl, TextConst.AName.Name) != TextConst.AVTable.Dual)
                    {
                        if (getAttrValue(tbl, TextConst.AName.View) != TextConst.AVBool.True)
                        {

                            if (sel.Elements().All(e => getAttrValue(e, TextConst.AName.Column) != TextConst.AVColumnArray.SysColNamesForEditedObject[0]))
                            {

                                foreach (string name in TextConst.AVColumnArray.SysColNamesForEditedObject)
                                {
                                    var col = VQuery.CreateVirtualSysColumnElement(tbl.Attribute(TextConst.AName.As).Value, name);
                                    sel.Add(col);
                                }
                            }
                        }
                    }
                }
                //}

            }

        }
        private static void addQueryAlias(XElement element)
        {

            foreach (XElement query in element.Descendants("query").Where(e => e.Parent.Name.LocalName == "from" && e.Attribute("as") == null))
            {
                query.SetAttributeValue("as", getAttrValue(query, "name"));
            }

        }
        private static void addCountersToQube(XElement element)
        {
            if (element.Elements(TextConst.EName.Push).Any() && element.Attribute("qcaplied") == null)
            {

                element.SetAttributeValue("qcaplied", "1");
                foreach (XElement rel in element.Elements(TextConst.EName.Push).Elements(TextConst.EName.From).Elements())
                {
                    if (rel.Descendants(TextConst.EName.Column).Count() < 3)
                    {
                        string parentQueryName = rel.Attribute(TextConst.AName.Name).Value;

                        string relAlias = getAttrValue(rel, TextConst.AName.As);
                        if (relAlias == "")
                        {
                            relAlias = parentQueryName;
                        }
                        //if (relAlias == "kod_smet_sub")
                        //{
                        //    relAlias = "kod_smet_sub";
                        //}

                      
                        XElement parentColumn = rel.Descendants(
                          TextConst.EName.Column
                          ).First(c => c.Attribute(TextConst.AName.Table) == null || getAttrValue(c, TextConst.AName.Table) == getAttrValue(rel, TextConst.AName.As));

                        XElement childColumn = rel.Descendants(
                         TextConst.EName.Column
                         ).First(c => c.Attribute(TextConst.AName.Table) == null || getAttrValue(c, TextConst.AName.Table) != getAttrValue(rel, TextConst.AName.As));


                        XElement qry = new XElement(TextConst.EName.Query
                              , new XAttribute(TextConst.AName.As, relAlias + TextConst.Pfx.QubeCounter)
                              , new XElement(TextConst.EName.Select)
                              , new XElement(TextConst.EName.From)
                            );

                        XElement qryCall = new XElement(TextConst.EName.Query
                            , new XAttribute(TextConst.AName.Name, parentQueryName)
                              , new XAttribute(TextConst.AName.As, parentQueryName)
                            );


                        qry.Element(TextConst.EName.From).Add(qryCall);

                        qry.Element(TextConst.EName.Select).Add(
                            new XElement(TextConst.EName.Column
                                , new XAttribute(TextConst.AName.Table, parentQueryName)
                                  , new XAttribute(TextConst.AName.Column, parentColumn.Attribute(TextConst.AName.Column).Value)
                                   , new XAttribute(TextConst.AName.As, childColumn.Attribute(TextConst.AName.Column).Value)

                                   )
                            );
                        qry.Element(TextConst.EName.Select).Add(
                            new XElement(TextConst.EName.Const
                                , new XAttribute(TextConst.AName.DataType, TextConst.AVDataType.Number)
                                  , new XAttribute(TextConst.AName.As, relAlias + TextConst.Pfx.QubeCounter)

                                , new XText("1")
                                )
                            );

                        element.Element(TextConst.EName.From).Add(qry);
                        element.Element(TextConst.EName.Select).Add(
                             new XElement(TextConst.EName.Column
                                , new XAttribute(TextConst.AName.Table, relAlias + TextConst.Pfx.QubeCounter)
                                  , new XAttribute(TextConst.AName.Column, relAlias + TextConst.Pfx.QubeCounter)
                                   , new XAttribute(TextConst.AName.Group, TextConst.AVGroup.Sum)
                                    , new XAttribute(TextConst.AName.Stored, "0")
                                )

                        );
                    }

                }
            }

        }
        private static void processingQuickLinks(XElement element)
        {
            // пока один уровень
            foreach (XElement column in element.Descendants(TextConst.EName.Select).Descendants(TextConst.EName.Column).
                Where(e => getAttrValue(e, TextConst.AName.Table).Contains(".")).ToArray())
            {


                string[] ss = column.Attribute(TextConst.AName.Table).Value.Split('.');
                string queryName = ss[0];
                XElement srcQuery = column.Ancestors(TextConst.EName.Select).First().Parent
                  .Elements(TextConst.EName.From).Descendants()
                  .Where(e => (new string[] { TextConst.EName.Query, TextConst.EName.Link }).Contains(e.Name.LocalName)).First(e1 => getAliasOrName(e1) == queryName);

                string linkName = "";
                string linkAlias = "";
                for (int i = 1; i < ss.Length; i++)
                {
                     linkName = ss[i];
                     linkAlias = linkName + TextConst.Pfx.ExtValName;

                    XElement link = srcQuery.Elements(TextConst.EName.Link).FirstOrDefault(e => getAttrValue(e, TextConst.AName.As) == linkAlias);

                    if (link == null)
                    {
                        link = new XElement(TextConst.EName.Link
                            , new XAttribute(TextConst.AName.Name, linkName)
                            , new XAttribute(TextConst.AName.As, linkName + TextConst.Pfx.ExtValName)
                              );
                        srcQuery.Add(link);
                    }
                    srcQuery = link;

                }
                column.SetAttributeValue(TextConst.AName.Table, linkAlias);
            }
        }
        private static void processingParamsLinks(XElement element)
        {
            foreach (XElement param in element.Elements(TextConst.EName.Params).Elements().
                    Where(e => e.Attribute(TextConst.AName.ClassType) != null).ToList()) {
                string paramTableAlias = getAliasOrName(param);
                XElement fromQuery = element.Elements(EName.from).Descendants().FirstOrDefault(e => TextConst.ENameArray.ALinksAndQuery.Contains(e.Name.LocalName) && getAttrValue(e, TextConst.AName.As) == paramTableAlias);
                List<XElement> links = param.Elements().Where(e => TextConst.ENameArray.ALinks.Contains(e.Name.LocalName)).ToList();
                if (fromQuery == null) {
                    XElement querySceme = getQueryScheme(param.Attribute(TextConst.AName.ClassType).Value);

                    XElement keyCol = querySceme.Element(TextConst.EName.Select).Elements().FirstOrDefault(e => getAttrValue(e, TextConst.AName.Key) == TextConst.AVBool.True);
                    if (keyCol == null)
                    {
                        keyCol = querySceme.Element(TextConst.EName.Select).Elements().First();
                    }

                    var namedSources = new SortedList<string, XElement>();
                    string pqarmQueryName = param.Attribute(TextConst.AName.ClassType).Value;



                    namedSources.Add(paramTableAlias, param);


                    foreach (XElement link in param.Descendants().Where(e => TextConst.ENameArray.ALinks.Contains(e.Name.LocalName)))
                    {
                        namedSources.Add(getAliasOrName(link), link);
                    }




                    copyAttribute(keyCol, param, TextConst.AName.DataType);
                    string keyName = getAliasOrColumn(keyCol);

                    XElement queryExpr = new XElement(TextConst.EName.Query
                            , new XElement(TextConst.EName.Select
                                )
                            , new XElement(TextConst.EName.From
                                , new XElement(TextConst.EName.Query, new XAttribute(TextConst.AName.Name, pqarmQueryName), new XAttribute(TextConst.AName.As, paramTableAlias))
                                )
                            , new XElement(TextConst.EName.Where
                               , new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.In)
                                    , new XElement(TextConst.EName.Column, new XAttribute(TextConst.AName.Table, paramTableAlias), new XAttribute(TextConst.AName.Column, keyName), new XAttribute(TextConst.AName.Flag, "1"))
                                    , new XElement(TextConst.EName.UseParam, new XAttribute(TextConst.AName.Name, param.Attribute(TextConst.AName.Name).Value))
                                    )
                                )
                            );



                    queryExpr.Element(TextConst.EName.From).Element(TextConst.EName.Query).Add(
                      links
                        );




                    foreach (string name in namedSources.Keys)
                    {


                        var cols = element.Descendants(TextConst.EName.Column).Where(e => getAttrValue(e, TextConst.AName.Flag) != "1" && getAttrValue(e, TextConst.AName.Table) == name).ToList();
                        // var cols =getQueryColumns(element).Where(e =>   getAttrValue(e, TextConst.AName.Table) == name).ToList();
                        //может быть ошибка если есть подзапросы
                        foreach (XElement col in cols)
                        {
                            XElement queryExpr1 = new XElement(queryExpr);
                            queryExpr1.Element(TextConst.EName.Select).Add(new XElement(col));
                            //   queryExpr1.Element(TextConst.EName.Select).Elements().First().SetAttributeValue(TextConst.AName.Fixed, "1");

                            col.ReplaceWith(queryExpr1);
                        }
                    }


                }
                links.Remove();
                element.Descendants().Attributes(TextConst.AName.Flag).Remove();
            }
        }
        private static void removeExcludedElements(XElement element)
        {
            IList<XElement> list = element.Elements().ToList();
            for (int index = 0; index < list.Count; index++) {
                XElement e = list[index];
                if (e.Attribute(AName.exclude) != null) {
                    e.Remove();
                } else {
                    removeExcludedElements(e);
                }
            }
        }
        internal static void addMatrializeId(XElement query, string name = null)
        {


            int matId = 0;
            foreach (XElement matquery in query.Descendants(TextConst.EName.Query).Where(e => e.Attribute(TextConst.AName.Materialize) != null).ToList())
            {
                if (matquery.Attribute(TextConst.AName.Name) == null && matquery.Attribute(TextConst.AName.MaterializeId) == null)
                {
                    if (name == null)
                    {
                        name = getAttrValue(query, TextConst.AName.Name);
                    }

                    string mid = name + TextConst.Pfx.Materialized + matId.ToString();
                    mid = mid.Replace("-", "_");
                    matquery.SetAttributeValue(TextConst.AName.MaterializeId, mid);
                    matId++;
                }

            }



        }
        private static void addAggForQubes(XElement query)
        {

            foreach (XAttribute attr in query.Descendants().Attributes(TextConst.AName.Agg).Where(e => e.Value == TextConst.AVGroup.No).ToList())
            {
                attr.Value = TextConst.AVGroup.Empty;// может быть так нельзя
            }

            foreach (XAttribute attr in query.Descendants().Attributes(TextConst.AName.Group).Where(e => e.Value == TextConst.AVGroup.No).ToList())
            {
                attr.Value = TextConst.AVGroup.Empty;// может быть так нельзя
            }

            if (query.Elements("select").Elements("column").FirstOrDefault(e => e.Attribute("table") != null && e.Attribute("table").Value == "*") != null)
            {
                foreach (XElement col in query.Elements("select").Elements())
                {
                    string gr = getAttrValue(col, "group");
                    if (gr == "" || grFuncsNames.Contains(gr))
                    {
                        if (col.Attribute("agg") == null)
                        {
                            if (!grFuncsNamesNative.Contains(gr))
                            {
                                gr = "sum";
                            }

                            col.SetAttributeValue("agg", gr);
                        }
                    }
                }
            }
        }
        internal static void addColumnsAlias(XElement query, bool isOld, bool isSingle)
        {
            foreach (XElement col in query.Descendants().Where(e => (new string[] { "select", "dimension", "measures", "pivot" }).Contains(e.Name.LocalName)).Elements(EName.column)) {
                if (col.Attribute(AName.@as) == null) {
                    col.Add(new XAttribute(AName.@as, col.Attribute(AName.column).Value));
                }
            }
            foreach (XElement col in query.Descendants().Where(e => (new string[] { "select", "dimension", "measures", "pivot" }).Contains(e.Name.LocalName)).Elements(EName.fact)) {
                if (col.Attribute(AName.@as) == null) {
                    col.Add(new XAttribute(AName.@as, col.Attribute(AName.column).Value));
                }
            }
            foreach (XElement col in query.Descendants().Where(e => (new string[] { "select", "dimension", "measures", "pivot" }).Contains(e.Name.LocalName)).Elements(EName.column)) {
                XAttribute attr = col.Attribute(AName.@as);
                if (attr != null && attr.Value.StartsWith("+")) {
                    attr.Value = col.Attribute(AName.column).Value + attr.Value.Substring(1);
                }
            }
            foreach (XElement col in query.Descendants(EName.select).Elements(EName.fact)) {
                col.SetAttrValue(TextConst.AName.IsFactUse, TextConst.AVBool.True);
            }
            if (!isOld && isSingle) {
                foreach (XElement sel1 in query.Descendants(EName.select)) {
                    HashSet<string> names = new HashSet<string>();
                    foreach (XElement col in sel1.Elements(EName.column)) {
                        var name = col.Attribute(AName.@as).Value;
                        if (names.Contains(name)) {
                            throw new VCompilerException("Повторяющееся имя колонки", query, col);
                        }
                        //возможно измерение прописано дважды
                        names.Add(name);
                    }
                }
            }
        }
        /* private static string searchColumnAttrVal(XElement element, string attrName, IEnumerable<VSXElement> scheme)
        {
            if (element == null)
            {
                return "";
            }
            if (element.Attribute(attrName) != null)
            {
                return element.Attribute(attrName).Value;
            }
            if (element.Attribute("table") == null)
            {
                return "";
            }
            string table = element.Attribute("table").Value;
            string column = element.Attribute("column").Value;
            XElement srcQuery = element.Parent.Parent.Element("from").Elements("query").FirstOrDefault(e => e.Attribute("as").Value == (table));

            if (srcQuery != null)
            {
                srcQuery = getQueryScheme(srcQuery.Attribute("name").Value);

                XElement srcColumn = srcQuery.Element("select").Elements().FirstOrDefault(e => e.Attribute("as").Value == (column));
                if (srcColumn.Attribute(attrName) != null)
                {
                    return srcColumn.Attribute(attrName).Value;
                }
                else
                {
                    return searchQueryAttrVal(srcColumn, attrName);
                }
            }
            else
            {
                return "";
            }
        }*/

        //precompile

        /*private void loadFields()
        {
            this.Element("fields").Elements().Remove();

            XElement query = Compiler.getQueryScheme(this.Attribute("name").Value);
            query = Compiler.applyPart(query);
            Compiler.addColumnsAlias(query);
            Compiler.applyLinks(query);
            int i = 0;
            foreach (XElement column in query.Element("select").Elements("column"))
            {

                VEntityTypeField field = new VEntityTypeField("field");
                field.SetAttributeValue("name", column.Attribute("as").Value);
                Compiler.copyAttribute(column, field, "vid");

                field.SetAttributeValue("type", Compiler.searchColumnAttrVal(column, "type"));
                field.SetAttributeValue("title", Compiler.searchColumnAttrVal(column, "title"));

                this.Element("fields").Add(field);
                if (i == 0)
                {
                    // KeyField = field;
                    field.SetAttributeValue("key", "1");
                }
                else
                {
                    XElement link = findFieldReference(column);
                    if (link != null)
                    {
                        field.SetAttributeValue("reference", link.Attribute("name").Value);
                    }
                }
                i++;

            }
        }*/


        internal static bool DontPrecompile = false;
        internal static void PreCompile(bool isOld, VSXElement compiling)
        {
            Contract.Assert(compiling != null);
            #if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #endif
            //newColIndex = 1;

            if (DontPrecompile) return;

            preColmpiling = true;

            removeExcludedElements(compiling);
           // clearFuncMultiSpaces(compiling);
            var queries = compiling.Elements(EName.queries).Elements(EName.query).ToArray();

            foreach (XElement el in queries)
            {
                processingMoveLinks(el);
                addQuerySysColumns(el);
                processingExtentions(el);
                addMatrializeId(el);
            }

            processingParts(compiling);
            processingUseColor(compiling);

            queries = compiling.Elements(EName.queries).Elements(EName.query).ToArray();
            foreach (XElement el in queries)
            {
                ApplyAddition(el);
                ProcessQueryBands(el);
            }

            preProccessingForms(compiling);
            addFormInfoQueries(compiling);


            queries = compiling.Elements(EName.queries).Elements(EName.query).ToArray();

            foreach (XElement el in queries)
            {
                addParamsNodeUsingForm(el);
                AddQueryAutoFilterParsAndConds(el);

                processingRecordSets(el);
            }

            queries = compiling.Elements(EName.queries).Elements(EName.query).ToArray();
            foreach (XElement el in queries)
            {
                processingParamsLinks(el);
                addQueryAlias(el);
                processingQuickLinks(el);

                processingAddSelfNames(el, isOld);
                addCountersToQube(el);
                addAggForQubes(el);
                addColumnsAlias(el, isOld, false);

                setReferences(el);
                processingWindow(el);
                collectVirtualFields(el);
                addFactNames(el);
            }

            foreach (XElement el in queries)
            {
                processingIfsPre(el);
                processingCumulSectionsAndVirtuals(el);
                processingIfs(el);
            }

            //addReportsForReferences();

            addReportForms(compiling);
            addRepColTbleAlias(compiling);
            foreach (XElement el in queries)
            {
                processingOrderColumns(el);
            }

            processingUsing(compiling);

            foreach (XElement el in queries)
            {
                Inheritance(el, null);
            }
            foreach (XElement el in queries)
            {
                addRefBackNames(el);
            }

            foreach (XElement el in queries)
            {
                addLinkInfo(el);
                changeExtLinksAlias(el);
                fakeParams(el);
                grSetSpecColumns(el);
            }

            processingArrays(compiling);
            preProcessingIGroup(compiling);
            applyGroupLevel(compiling, null);
            applyGroupLevel(compiling, null);
            addColumnsAlias(compiling, isOld, false);

            foreach (XElement el in queries)
            {
                markQueryKeys(el);
                preProcessingPivots(el);
                addClassTitles(el);
            }

            setFormFieldsVisible(compiling);

            preColmpiling = false;

            collectAdditionalAttrs(compiling);

            VCashUtils.ClearCash();
            #if DEBUG
            sw.Stop();
            Debug.WriteLine("Compiler.PreCompile(): " + sw.ElapsedMilliseconds.ToString() + " мс");
            #endif
        }
        internal static XElement PreCompileOther(XElement element, bool selfOnly)
        {
            Contract.Assert(element != null);
            Contract.Assert(element.Name != EName.query);
            #if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #endif
            preColmpiling = true;
            removeExcludedElements(element);
            processingParts(element);
            processingUseColor(element);
            processingUsing(element);
            processingArrays(element);
            preColmpiling = false;
            #if DEBUG
            sw.Stop();
            Debug.WriteLine("Compiler.PreCompileOther(): " + sw.ElapsedMilliseconds.ToString() + " мс");
            #endif
            return element;
        }
        internal static XElement PreCompileQuery(XElement query, bool selfOnly)
        {
            Contract.Assert(query != null);
            Contract.Assert(query.Name == EName.query);
            #if DEBUG
            string query_name = query.AttrOrDefault(AName.name, null);
            if (string.IsNullOrEmpty(query_name)) {
                query_name = query.AttrOrDefault(AName.comment, string.Empty);
                //if (string.IsNullOrEmpty(query_name)) {
                //    query_name = query.AttrOrDefault(AName.File, string.Empty);
                //}
            }
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #endif
            XElement el = query;
            preColmpiling = true;
            removeExcludedElements(query);
            processingMoveLinks(query);
            addQuerySysColumns(query);
            addMatrializeId(query);
            processingParts(el);
            processingUseColor(el);

            ApplyAddition(el);
            ProcessQueryBands(el);
            //preProccessingForm(el);
            if (!selfOnly)
            {
                createAndAddToSchemeFormFromQuery(el, true);
            }
            el = processingRecordSets(el);
            //processingAddNames(el);
            addParamsNodeUsingForm(el);
          
            AddQueryAutoFilterParsAndConds(el);
            processingParamsLinks(el);

            addQueryAlias(el);
            processingQuickLinks(el);
            processingAddSelfNames(el, false);
            addCountersToQube(el);
            addAggForQubes(el);
            addColumnsAlias(el, false, true);

            setReferences(el);
            processingWindow(el);
            collectVirtualFields(el);
            addFactNames(el);
            processingIfsPre(el);
            processingCumulSectionsAndVirtuals(el);
            processingIfs(el);
            //addReportsForReferences();

            //addReportForms();
            //addRepColTbleAlias();
            processingOrderColumns(el);

            processingUsing(el);
            Inheritance(el, null);
            addRefBackNames(el);



            addLinkInfo(el);
            changeExtLinksAlias(el);
            fakeParams(el);

            grSetSpecColumns(el);

            processingArrays(el);
            preProcessingIGroup(el);
            //applySelfGrsets(el);


            applyGroupLevel(el, null);



            addColumnsAlias(el, false, true);



            markQueryKeys(el);
            preProcessingPivots(el);
            addClassTitles(el);
            //collectAdditionalAttrs(el);
            // setFormFieldsVisible();

            preColmpiling = false;
            #if DEBUG
            sw.Stop();
            Debug.WriteLine("Compiler.PreCompileQuery(), " + query_name + ": " + sw.ElapsedMilliseconds.ToString() + " мс");
            #endif
            return el;
            // addKeyMarks();
        }
        private static XElement createFormFromQueryContent(XElement cnt)
        {
           
            
                
                var xfrm = new XElement(TextConst.EName.Form);
                xfrm.SetAttributeValue(TextConst.AName.Name, cnt.Parent.Attribute(TextConst.AName.Name).Value);
                copyAttribute(cnt.Parent, xfrm, TextConst.AName.WithBehavior);
                
                //cnt.Remove();
                xfrm.Add(cnt);
                return xfrm;
        }
        private static void createAndAddToSchemeFormFromQuery(XElement qry, bool isSingle)
        {
            var cnt = qry.Element(TextConst.EName.Content);
            if (cnt != null)
            {
                var wb = Cmn.GetAttrValue(qry, TextConst.AName.WithBehavior);
                if (wb == TextConst.AVBool.True || wb == "")
                {
                    return;
                }

                var xfrm = createFormFromQueryContent(cnt);
                cnt.Parent.SetAttributeValue(TextConst.AName.Form, cnt.Parent.Attribute(TextConst.AName.Name).Value);

                // Емцов - вроде должно получиться
                var xforms = qry.Ancestors().Last().Element(TextConst.EName.Forms);

                if (isSingle)
                {

                    var name = Cmn.GetAttrValue(cnt.Parent, TextConst.AName.Name);
                    if (name == "")
                    {
                        return;
                    }
                    var form = xforms.Elements(TextConst.EName.Form).FirstOrDefault(e => e.Attribute(TextConst.AName.Name).Value == name);
                    if (form != null)
                    {
                        form.Remove();
                    }
                }

                xforms.Add(xfrm);

                if (isSingle)
                {
                    preProccessingForm(xfrm);
                }
                //cnt.Remove();// как выяснилось так нельзя
            }
        
           
        }
        private static void preProccessingForm(XElement xfrm)
        {
            if (xfrm.Name.LocalName != TextConst.EName.Form) return;
            if (xfrm.Element("content") == null)
            {
                List<XElement> els = xfrm.Elements().ToList();
                xfrm.Add(new XElement("content"));
                els.Remove();
                xfrm.Element("content").Add(els);
            }

            foreach (XElement xfld in xfrm.Element("content").Descendants("usefield").Where(f => f.Attribute("field") != null).ToList())
            {
                //!!!было не native!!!E:\InfoenergoTFS\root\main\all\sql.builder.templates\sql.builder\projects\ies_garant\reports\61759_1.xml
                XElement fldSource = XmlReports.Environment.Manager.GetNativeScheme().Elements("fields").Elements("field").First(f => f.Attribute("id").Value == xfld.Attribute("field").Value);

                XElement newFld = new XElement(fldSource);

                newFld.Attribute("id").Remove();
                xfld.Attributes(TextConst.AName.Field).Remove();
                newFld.CopyAttributes(xfld.Attributes());
                //замена узла defaultquery
                if (xfld.Element(TextConst.EName.DefaultQuery) != null)
                {
                    newFld.Elements(TextConst.EName.DefaultQuery).Remove();
                    newFld.Add(xfld.Element(TextConst.EName.DefaultQuery));
                }
                if (xfld.Element(TextConst.EName.ListQuery) != null)
                {
                    newFld.Elements(TextConst.EName.ListQuery).Remove();
                    newFld.Add(xfld.Element(TextConst.EName.ListQuery));
                }
                xfld.ReplaceWith(newFld);


            }
        }

        private static void preProccessingForms(VSXElement compiling)
        {

            foreach (XElement cnt in compiling.Elements(TextConst.EName.Queries).Elements(TextConst.EName.Query).Elements(TextConst.EName.Content).ToList())
            {

                createAndAddToSchemeFormFromQuery(cnt.Parent, false);

            }
            foreach (XElement xfrm in compiling.Elements("forms").Elements("form"))
            {
                
                preProccessingForm(xfrm);
            }
        }

        private static void addParamsNodeUsingForm(XElement el)
        {
            XElement form = null;
            if (el.Element(TextConst.EName.Params) == null)
            {

                if (el.Attribute(TextConst.AName.Form) != null)
                {
                    form = XmlReports.Environment.Manager.GetScheme().Elements(TextConst.EName.Forms).Elements(TextConst.EName.Form).FirstOrDefault(e => e.Attribute("name").Value == el.Attribute(TextConst.AName.Form).Value);

                }
                else
                {
                    var cnt = el.Element(TextConst.EName.Content);
                    if (cnt != null)
                    {
                        
                        form = createFormFromQueryContent(cnt);
                        preProccessingForm(form);
                    }
                   
                }

            }
            bool isParTypes = Cmn.GetAttrValue(el, TextConst.AName.CanUseSimpleParams) == TextConst.AVBool.True;
            if (form != null)
            {
                var xpars = new XElement(TextConst.EName.Params);
                foreach (XElement fld in form.Descendants(TextConst.EName.Field))
                {
                    var xpar = new XElement(EName.param);
                    xpar.CopyAttributes(fld.Attributes());
                    if (isParTypes)
                    {
                        if (fld.Attribute(TextConst.AName.ControlType).Value == TextConst.AVControlType.List)
                        {
                            xpar.SetAttributeValue(TextConst.AName.Type, TextConst.AVDataType.Array);
                        }
                    }
                    else
                    {

                        xpar.Attributes(TextConst.AName.Type).Remove(); // !!! Влияет на признак IsSimpleParams. При наличии которого не удаляются опциональные уловия при компиляции
                       
                    }
                    xpars.Add(xpar);
                }
                el.AddFirst(xpars);

            }
        }
        /// <summary>
        /// Добавляет для каждой формы &lt;query name="form:имя формы" &gt; по dual с названиями всех её полей
        /// Этот query анализируется в методе <see cref="VDataSet.GetParamsAsXml"/>
        /// </summary>
        /// <param name="compiling">компилируемая схема</param>
        /// <seealso cref="VDataSet.GetParamsAsXml"/>
        private static void addFormInfoQueries(VSXElement compiling)
        {
            XElement queries = compiling.Element(EName.queries);
            foreach (XElement form in compiling.Elements(EName.forms).Elements(EName.form)) {
                List<XElement> fields = new List<XElement>();
                foreach (XElement field in form.Descendants(EName.field)) {
                    if (field.AttrOrDefault(AName.visible, true) && field.AttrOrDefault(AName.column_visible, true)) {
                        fields.Add(field);
                    }
                }
                if (fields.Count != 0) {
                    XElement query, select, from, dual, value, field;
                    if (fields.Count == 1) {
                        Factory.NewSelectFromDualQuery(out query, out select, out from, out dual);
                        dual.Add(new XAttribute(AName.@as, TextConst.AVTable.Dual));
                        //
                        field = fields[0];
                        value = Factory.NewConst("'" + field.AttrOrEmpty(AName.title) + "'");
                        value.Add(new XAttribute(AName.type, TextConst.AVDataType.String));
                        value.Add(new XAttribute(AName.@as, "title"));
                        value.Add(new XAttribute(AName.title, "Параметр"));
                        select.Add(value);
                        //
                        value = Factory.NewConst("'" + field.AttrOrEmpty(AName.name) + "'");
                        value.Add(new XAttribute(AName.type, TextConst.AVDataType.String));
                        value.Add(new XAttribute(AName.@as, "name"));
                        value.Add(new XAttribute(AName.key, TextConst.AVBool.True));
                        select.Add(value);
                        //
                        value = Factory.NewConst("NULL");
                        value.Add(new XAttribute(AName.type, TextConst.AVDataType.String));
                        value.Add(new XAttribute(AName.@as, "text"));
                        value.Add(new XAttribute(AName.title, "Значение"));
                        select.Add(value);
                    } else {
                        Factory.NewSelectFromQuery(out query, out select, out from);
                        XElement union_query, union;
                        Factory.NewUnionQuery(out union_query, out union);
                        union_query.Add(new XAttribute(AName.@as, "u"));
                        from.Add(union_query);
                        //
                        XElement column = Factory.NewColumn("u", "title");
                        column.Add(new XAttribute(AName.title, "Параметр"));
                        select.Add(column);
                        //
                        column = Factory.NewColumn("u", "name");
                        column.Add(new XAttribute(AName.key, TextConst.AVBool.True));
                        select.Add(column);
                        //
                        column = Factory.NewColumn("u", "text");
                        column.Add(new XAttribute(AName.title, "Значение"));
                        select.Add(column);
                        //
                        for (int index = 0; index < fields.Count; index++) {
                            field = fields[index];
                            XElement subquery, subquery_select, subquery_from;
                            Factory.NewSelectFromDualQuery(out subquery, out subquery_select, out subquery_from, out dual);
                            //
                            value = Factory.NewConst("'" + field.AttrOrEmpty(AName.title) + "'");
                            if (index == 0) {
                                value.Add(new XAttribute(AName.type, TextConst.AVDataType.String));
                                value.Add(new XAttribute(AName.@as, "title"));
                            }
                            subquery_select.Add(value);
                            //
                            value = Factory.NewConst("'" + field.AttrOrEmpty(AName.name) + "'");
                            if (index == 0) {
                                value.Add(new XAttribute(AName.type, TextConst.AVDataType.String));
                                value.Add(new XAttribute(AName.@as, "name"));
                            }
                            subquery_select.Add(value);
                            //
                            value = Factory.NewConst("NULL");
                            if (index == 0) {
                                value.Add(new XAttribute(AName.type, TextConst.AVDataType.String));
                                value.Add(new XAttribute(AName.@as, "text"));
                            }
                            subquery_select.Add(value);
                            //
                            union.Add(subquery);
                        }
                    }
                    query.Add(new XAttribute(AName.name, "form:" + form.Attribute(AName.name).Value));
                    //XAttribute file = form.Attribute(AName.file);
                    //if (file != null) {
                    //    query.Add(new XAttribute(file));
                    //}
                    queries.Add(query);
                }
            }
        }
        /// <summary>
        /// Преобразует в union select по dual запросы, у которых непосредственно под &lt;query&gt; располагаются только &lt;const&gt;
        /// </summary>
        /// <param name="element"></param>
        /// <returns></returns>
        private static XElement processingRecordSets(XElement element)
        {
            List<XElement> els = element.DescendantsAndSelf(EName.query).Where(e => e.Element(EName.@const) != null).ToList();
            foreach (XElement el1 in els) {
                // Собираем все <const> с as
                List<XElement> fields = new List<XElement>(2);
                foreach (XElement e in el1.Elements()) {
                    Contract.Assume(e.Name == EName.@const);
                    if (e.Attribute(AName.@as) != null) {
                        fields.Add(e);
                    }
                }
                int field_count = fields.Count();
                Contract.Assume(field_count > 0);
                XElement qry, select, from;
                Factory.NewSelectFromQuery(out qry, out select, out from);
                select.Add(Factory.NewColumn("u", TextConst.AVColumn.All));
                XElement union_query = new XElement(EName.query, new XAttribute(AName.@as, "u"));
                from.Add(union_query);
                XElement union = new XElement(EName.union);
                union_query.Add(union);
                qry.CopyAttributes(el1.Attributes());
                //
                int field = field_count;
                //int row = 0;
                XElement subquery = null;
                XElement subquery_select = null;
                XElement subquery_from, dual;
                foreach (XElement cell in el1.Elements()) {
                    Contract.Assume(cell.Name == EName.@const);
                    if (field == field_count) {
                        //row++;
                        Factory.NewSelectFromDualQuery(out subquery, out subquery_select, out subquery_from, out dual);
                        //subquery.Add(new XAttribute(AName.@as, "r" + row.ToString()));
                        union.Add(subquery);
                        field = 0;
                    }
                    subquery_select.Add(new XElement(cell));
                    field++;
                }
                el1.ReplaceWith(qry);
                if (el1 == element && els.Count == 1) {
                    return qry;
                }
            }
            return element;
        }
        private static void processingIfsPre(XElement query)
        {

            SortedList<string, XElement> ifs = new SortedList<string, XElement>();

            XElement qry = query;
            // foreach (XElement qry in schemeRoot.Elements("queries").Elements())
            // {
            foreach (XElement ifEl in qry.Descendants("select").Descendants().Where(e => e.Attribute("if") != null).ToArray())
            {


                if (ifEl.Element("if") != null || ifEl.Element("section") != null)
                {

                    // !!! переписать нормально, так не будет работать при наличии if и section одновременно
                    if (ifEl.Element("if") != null)
                    {
                        if (!ifs.ContainsKey(qry.Attribute("name").Value + "-" + ifEl.Attribute("if").Value))
                        {
                            ifs.Add(qry.Attribute("name").Value + "-" + ifEl.Attribute("if").Value, ifEl.Element("if"));
                        }
                    }
                    if (ifEl.Element("section") != null)
                    {
                        if (!ifs.ContainsKey(qry.Attribute("name").Value + "-" + ifEl.Attribute("if").Value))
                        {
                            ifs.Add(qry.Attribute("name").Value + "-" + ifEl.Attribute("if").Value, ifEl.Element("section"));
                        }
                    }
                }
                else
                {
                    string[] ifnames = ifEl.Attribute("if").Value.Split(',');

                    if (ifnames.Length > 1)
                    {
                        string title = "";
                        string pfx = "";
                        XElement ifEl1 = new XElement("if", new XElement("call", new XAttribute("function", "and")));
                        foreach (string ifname in ifnames)
                        {


                            XElement ifEl0 = ifs[qry.Attribute("name").Value + "-" + ifname];
                            title += getAttrValue(ifEl0, "title");
                            pfx += getAttrValue(ifEl0, "pfx");
                            ifEl1.Element("call").Add(ifEl0.Element("call"));

                        }
                        ifEl1.SetAttributeValue("title", title);
                        ifEl1.SetAttributeValue("pfx", pfx);
                        ifEl.Add(ifEl1);
                    }
                    else
                    {
                        ifEl.Add(ifs[qry.Attribute("name").Value + "-" + ifnames[0]]);
                    }



                }

            }

            //}

        }

        private static void processingIfs(XElement query)
        {



            //}
            foreach (XElement ifEl in query.Descendants("if").ToArray())
            {
                //if (ifEl.Parent.Attribute("as")!=null  && ifEl.Parent.Attribute("as").Value == "nzs_sp_pg")
                //{
                    
                //}


              

                XElement col = new XElement(ifEl.Parent);
                col.Elements("if").Remove();
                var events = col.Elements(TextConst.EName.Events);
                events.Remove();
                col.Attributes().Where(a => !(new string[] { "table", "column", "type", "dgroup", "function" }).Contains(a.Name.LocalName)).Remove();

                //if (ifEl.Parent.Attributes(TextConst.AName.Multiplicer).Any() 
                //    &&
                //    getAttrValue(col,TextConst.AName.Table)==TextConst.AVTable.Ths
                //    )
                //{
                //    col.SetAttributeValue(TextConst.AName.Multiplicer, "");
                //}
               
                XElement expr = new XElement("call",

                            new XAttribute("function", "if"),
                            ifEl.Elements(),
                            col, events
                    );
                copyAttributes(ifEl.Parent, expr);

                expr.Attributes("table").Remove();
                expr.Attributes("column").Remove();
                expr.Attributes("if").Remove();
              
                //if (expr.Attributes(TextConst.AName.Multiplicer).Any())
                //{
                //    expr.Attributes(TextConst.AName.Multiplicer).Remove();
                //}
                ifEl.Parent.ReplaceWith(expr);
                XElement fld = expr;
                if (expr.Parent.Name.LocalName != "select")
                {
                    fld = expr.Ancestors().First(e => e.Parent.Name.LocalName == "select");

                }

                if (getAttrValue(fld, "ifaplyed") == "")
                {
                    fld.SetAttributeValue("ifaplyed", "1");
                    if (getAttrValue(expr, "title") != "")// !!!Не будет работать для полей с пустыми заголовками. Доделать.
                    {
                        fld.SetAttributeValue("title", getAttrValue(fld, "title") + getAttrValue(ifEl, "title"));
                    }
                    if (getAttrValue(ifEl, "pfx") != "")
                    {
                        fld.SetAttributeValue("as", getAttrValue(fld, "as") + getAttrValue(ifEl, "pfx"));
                    }
                }
            }
        }
        private static void processingWindow(XElement query)
        {
            Contract.Assert(query != null);
            foreach (XElement col in query.Descendants().ToList()) {
                XAttribute attr_window = col.Attribute(AName.window);
                if (attr_window != null) {
                    XAttribute attr_agg = col.Attribute(AName.agg);
                    string func_name;
                    if (attr_agg != null) {
                        func_name = attr_agg.Value;            // аггрегатная функция: min, max, sum, avg etc
                    } else {
                        func_name = TextConst.AVFunction.Sum;  // по умолчанию - сумма
                    }
                    attr_window.Remove();
                    XElement agg_func = Factory.NewCall(func_name);
                    XElement part_by  = Factory.NewCall(TextConst.AVFunction.PartitionBy);
                    XElement overExpr = Factory.NewCall(TextConst.AVFunction.Over, agg_func, part_by);
                    col.Attributes(AName.@as).ChangeParent(overExpr);
                    col.Attributes("virtual").ChangeParent(overExpr);
                    agg_func.Add(new XElement(col));
                    string[] dimColsNames = attr_window.Value.Split(',');
                    attr_window = null;
                    for (int index = 0; index < dimColsNames.Length; index++) {
                        string col_name = string.Intern(dimColsNames[index].Trim());
                        Contract.Assert(!string.IsNullOrWhiteSpace(col_name));
                        XElement partCol = Factory.NewColumn(TextConst.AVTable.Ths, dimColsNames[index]);
                        partCol.Add(new XAttribute(AName.group, TextConst.AVGroup.Group)); // TextConst.AVGroup.Group="1"
                        part_by.Add(partCol);
                    }
                    col.ReplaceWith(overExpr);
                }
            }
        }
        private static SortedList<string, SortedList<string, XElement>> virtualFields = new SortedList<string, SortedList<string, XElement>>();

        private static void addFactNames(XElement query)
        {
            foreach (XElement field in query.Elements("select").Elements().Where(e => getAttrValue(e, TextConst.AName.IsFact) == "1" && getAttrValue(e, TextConst.AName.Fact) == ""))
            {
                field.SetAttributeValue(TextConst.AName.Fact
                    , query.Attribute(TextConst.AName.Name).Value + "_" + field.Attribute(TextConst.AName.As).Value);
            }
        }

        private static void collectVirtualFields(XElement query)
        {
            foreach (XElement field in query.Elements("select").Elements().Where(e => getAttrValue(e, "virtual") == "1"))
            {
                if (!virtualFields.ContainsKey(query.Attribute("name").Value))
                {
                    virtualFields.Add(query.Attribute("name").Value, new SortedList<string, XElement>());
                }
                if (!virtualFields[query.Attribute("name").Value].ContainsKey(field.Attribute("as").Value))
                {
                    XElement expr = copyThisColumns(field, true);
                    virtualFields[query.Attribute("name").Value].Add(field.Attribute("as").Value, expr);
                }
            }
        }

        //private static string[] sectRoleAttrsNames = new string[] { "cumulate", "sections" };
        private static XAttribute searchSectAttr(XElement srcQuery, XElement srcCol)
        {
            List<XElement> cols = new List<XElement>(1) { srcCol };
            XElement cumCol = null;
            XAttribute cumAttr = null;
            while (cumAttr == null) {
                List<string> names = cols.DescendantsAndSelf().Where(e => e.AttrOrEmpty(AName.table) == TextConst.AVTable.Ths).Select(e1 => e1.AttrOrEmpty(AName.column)).ToList();
                cols = srcQuery.Element(EName.select).Elements().Where(e => names.Contains(e.Attribute(AName.@as).Value)).ToList();
                if (cols.Count == 0) {
                    break;
                }
                cumCol = cols[0];
                cumAttr = cols.Attributes().FirstOrDefault(APredicate.IsSectRoleAttribute);
            }
            return cumAttr;
        }
        private static void processingCumulSectionsAndVirtuals(XElement query)
        {
            List<XElement> sourcesWithVirFields = query.Elements("from").Elements().Where(e => virtualFields.ContainsKey(getAttrValue(e, "name"))).ToList();

            foreach (XElement src in sourcesWithVirFields)
            {
                XElement srcQuery = getQueryScheme(src.Attribute("name").Value);
                SortedList<string, XElement> virtualFields1 = virtualFields[src.Attribute("name").Value];
                foreach (XElement col in query.Descendants("column").Where(e => src.Attribute("as").Value == getAttrValue(e, "table") && virtualFields1.ContainsKey(getAttrValue(e, "column"))).ToList())
                {
                    // XElement srcCol = srcQuery.Element("select").Elements().Where(e => e.Attribute("as").Value == col.Attribute("column").Value).First();
                    XElement srcCol = virtualFields1[col.Attribute("column").Value];

                    XElement expr = new XElement(srcCol);

                    foreach (XElement col1 in expr.Descendants("column").ToList())
                    {



                        string dimName = "";
                        if (col.Element("section") != null || col.Element("pivot") != null || col.Attribute("if") != null || col.Attribute("dimname") != null)
                        {

                            //if (getAttrValue(query, "name") == "26630-dat")
                            //{
                            //    query.SetAttributeValue("name", getAttrValue(query, "name"));
                            //}

                            XAttribute srcAttr = col1.Attributes().FirstOrDefault(APredicate.IsSectRoleAttribute);

                            if (srcAttr == null)
                            {

                                srcAttr = searchSectAttr(srcQuery, col1);
                            }

                            if (srcAttr != null)
                            {
                                dimName = srcAttr.Value;
                            }

                        }
                        if (dimName != "")
                        {
                            if (col.Element("section") != null)
                            {
                                col1.Add(new XElement(col.Element("section")));
                            }
                            copyAttribute(col, col1, "if");

                            if (col.Element("pivot") != null)
                            {
                                if (expr.Element("pivot") == null)
                                {
                                    expr.Add(new XElement(col.Element("pivot")));
                                }
                            }
                            copyAttribute(col, expr, "dimname");
                            if (col.Element("pivot") != null || col.Attribute("dimname") != null)
                            {
                                col1.SetAttributeValue("pivtarg", "1");
                            }

                        }

                        col1.SetAttributeValue("table", col.Attribute("table").Value);

                    }
                    expr.SetAttributeValue("as", col.Attribute("as").Value);
                    copyAttribute(col, expr, "group");
                    copyAttributes(col, expr, columnAttributesNames);
                    expr.CopyAttributes(col.Attributes().Where(APredicate.IsAdditionalAttribute));
                    expr.Attribute("virtual").Remove();
                    col.ReplaceWith(expr);

                }
            }


            foreach (XElement sect in query.Descendants("section").ToList())
            {
                XElement col = sect.Ancestors("column").FirstOrDefault();

                if (col == null)
                {
                    XElement col1 = sect.Ancestors().First(e => e.Parent.Name.LocalName == "select");
                    col = col1.Descendants().First(e => getAttrValue(e, "pivtarg") == "1");

                }

                string tablePName = col.Attribute("table").Value;
                XElement qCall = query.Elements("from").Elements("query").First(e => e.Attribute("as").Value == tablePName);

                XElement srcQuery = getQueryScheme(qCall.Attribute("name").Value);

                XElement srcCol = srcQuery.Element("select").Elements().First(e => e.Attribute("as").Value == col.Attribute("column").Value);

                XAttribute srcAttr = srcCol.Attributes().FirstOrDefault(APredicate.IsSectRoleAttribute);

                if (srcAttr == null)
                {
                    srcAttr = srcCol.Descendants().Attributes().FirstOrDefault(APredicate.IsSectRoleAttribute);
                    XElement srcCol1 = null;
                    if (srcAttr != null)
                    {
                        srcCol1 = srcAttr.Parent;
                    }

                    //      XElement srcCol1 = srcCol.Descendants().Where(e => e.Attributes().Where(a => sectRoleAttrsNames.Contains(a.Name)).FirstOrDefault()!=null)).FirstOrDefault();
                    if (srcCol1 != null)
                    {
                        srcCol = srcCol1;
                    }
                    else
                    {
                        srcAttr = searchSectAttr(srcQuery, srcCol);
                        srcCol = srcAttr.Parent;
                    }
                }

                string dimName = srcAttr.Value;

                // XElement col1=new XElement( col);
                // col1.Elements().Remove();
                // col1.Attributes("group").Remove();
                XElement expr = null;

                if (srcAttr.Name == "cumulate")
                {
                    if (!sect.Elements("last").Any())
                    {
                        expr = new XElement("if",
                                new XElement("call", new XAttribute("function", TextConst.AVFunction.Or),
                                      new XElement("call", new XAttribute("function", TextConst.AVFunction.Equal),
                                        new XElement("column", new XAttribute("table", tablePName), new XAttribute("column", dimName)),
                                        new XElement(sect.Elements().First())
                                      ),
                                      new XElement("call", new XAttribute("function", TextConst.AVFunction.And),
                                          new XElement("call", new XAttribute("function", TextConst.AVFunction.Less),
                                            new XElement("column", new XAttribute("table", tablePName), new XAttribute("column", dimName)),
                                            new XElement(sect.Elements().First())
                                          )
                                          , new XElement("call", new XAttribute("function", TextConst.AVFunction.Or),
                                              new XElement("call", new XAttribute("function", TextConst.AVFunction.Greater),
                                                new XElement("column", new XAttribute("table", tablePName), new XAttribute("column", dimName + cumulNextPfx)),
                                                new XElement(sect.Elements().First())

                                              ),
                                              new XElement("call", new XAttribute("function", TextConst.AVFunction.IsNull),
                                                new XElement("column", new XAttribute("table", tablePName), new XAttribute("column", dimName + cumulNextPfx))
                                              )
                                          )
                            //,new XElement("call", new XAttribute("function", "="),
                            //      new XElement("column", new XAttribute("table", tablePName), new XAttribute("column", dimName + cumulNextPfx)),
                            //      new XElement("column", new XAttribute("table", tablePName), new XAttribute("column", dimName))

                                          //    )
                                        ),
                                        new XElement("call", new XAttribute("function", "is null"),
                                            new XElement("column", new XAttribute("table", tablePName), new XAttribute("column", dimName))
                                          )
                                    )
                            );
                    }
                    else
                    {
                        // !!! Переделать аналогично для применения </last> когда появится необходимость, так теперь работать не будет
                        expr = new XElement("if",
                                new XElement("call", new XAttribute("function", "="),
                                            new XElement("column", new XAttribute("table", tablePName), new XAttribute("column", dimName + cumulNextPfx)),
                                            new XElement("const", new XText("1"))
                                          )

                            );
                    }


                }
                if (srcAttr.Name == "sections")
                {
                    expr = new XElement("if",
                           new XElement("call", new XAttribute("function", "="),
                                       new XElement("column", new XAttribute("table", tablePName), new XAttribute("column", dimName)),
                                       new XElement(sect.Elements().First())
                                     )

                       );
                }

                //copyAttributes(col, expr, new string[] { "as", "title", "type", "class-title", "agg", "format","group" });


                if (sect.Parent.Name.LocalName != "pivot")
                {
                    XElement ifEl = sect.Parent.Elements("if").FirstOrDefault();
                    if (ifEl != null)
                    {
                        XElement andEl = new XElement("call", new XAttribute("function", "and"));
                        XElement condEl = ifEl.Elements().First();
                        condEl.Remove();
                        andEl.Add(condEl);
                        andEl.Add(expr.Elements());
                        ifEl.Add(andEl);
                    }
                    else
                    {
                        copyAttributes(col.Element("section"), expr, new string[] { "pfx", "title" });
                        col.Add(expr);
                    }
                }
                else
                {

                    sect.Parent.AddFirst(expr.Elements());

                }

                sect.Remove();





            }

            ///////////////////


        }

        private static void setFormFieldsVisible(VSXElement compiling)
        {
            foreach (var xform in compiling.Elements("forms").Elements("form"))
            {
                var xfields = xform.Descendants("field");
                var xfieldgroups = xform.Descendants("fieldgroups");

                bool any_visible = xfields.Any(f => f.AttrOrDef("visible", "") == "1")
                                || xfieldgroups.Any(fg => fg.AttrOrDef("visible", "") == "1");
                bool any_invisible = xfields.Any(f => f.AttrOrDef("visible", "") == "0")
                                || xfieldgroups.Any(fg => fg.AttrOrDef("visible", "") == "0");

                var without_attr = new[] { xfields, xfieldgroups }.SelectMany(f => f).Where(f => f.AttrOrDef("visible", "") == "");
                if (any_visible && !any_invisible)
                {
                    foreach (XElement xfield in without_attr)
                    {
                        xfield.Add(new XAttribute("visible", "0"));
                    }
                }
                else if (!any_visible && any_invisible)
                {
                    foreach (XElement xfield in without_attr)
                    {
                        xfield.Add(new XAttribute("visible", "1"));
                    }
                }
            }
        }

        private static void processNavigators(IEnumerable<VSXElement> scheme)
        {
            foreach (var n in scheme.Elements("navigators").Elements("navigator"))
            {
                processNavigator(n, scheme);
            }
        }

        private static void processNavigator(XElement xnavigator, IEnumerable<VSXElement> scheme)
        {
            scheme = XmlReports.Environment.Manager.GetNativeScheme();

            string project = xnavigator.Attribute("file").Value.Split('\\')[1];
            foreach (var xitem in xnavigator.Descendants())
            {
                if (xitem.Name == TextConst.EName.UseForm)
                {
                    xitem.SetAttributeValue("project", project);

                    XElement xform = scheme.Elements("forms").Elements("form")
                        .First(f => f.Attribute("name").Value == xitem.Attribute("report").Value);

                    xitem.SetAttributeValue("title", xform.GetAttributeValue("title"));
                }
                else if (xitem.Name == TextConst.EName.UseReport)
                {
                    xitem.SetAttributeValue("project", project);

                    XElement xreport = scheme.Elements("reports").Elements("report")
                        .First(r => r.Attribute("name").Value == xitem.Attribute("report").Value);

                    xitem.SetAttributeValue("title", xreport.GetAttributeValue("title"));
                    xitem.SetAttributeValue("visible", xreport.GetAttributeValue("visible"));
                }
            }
        }

        private static void setReferences(XElement query)
        {
            //foreach (XElement query in schemeRoot.Elements("queries").Elements("query"))
            //{
            int i = 0;
            foreach (XElement column in query.Elements("select").Elements("column").Where(e => e.Attribute("reference") == null))
            {
                if (i > 0)
                {
                    IEnumerable<XElement> links = column.Parent.Parent.Elements("from").Elements("query").Where(
                    e1 => e1.Elements("call").Descendants("column").Any(e => e.Attribute("table").Value == column.Attribute("table").Value & e.Attribute("column").Value == column.Attribute("column").Value)
                    );
                    if (links.Count() == 1)
                    {
                        if (links.First().Attribute("name") != null)
                        {
                            column.SetAttributeValue("reference", links.First().Attribute("name").Value);
                            XElement refCol = links.First().Descendants("column").FirstOrDefault(e => e.Attribute("table").Value == links.First().Attribute("as").Value);
                            if (refCol != null)
                            {
                                column.SetAttributeValue("refcol", refCol.Attribute("column").Value);
                            }
                        }
                    }
                }
                i++;
            }
            //}
        }
        private static void processingOrderColumns(XElement query)
        {
            // foreach (XElement query in schemeRoot.Elements("queries").Elements("query").Where(e => e.Attribute("order") != null))
            // {
            if (query.Attribute("order") != null)
            {
                XElement order = new XElement("order");
                query.Add(order);
                foreach (string colname in extractColNamesFrormOrderString(query.Attribute("order").Value))
                {
                    order.Add(new XElement("column", new XAttribute("column", colname)));
                }
            }
            // }
        }

        private static string[] extractColNamesFrormOrderString(string s)
        {
            s = s.Replace(" ", "").Replace("desc", "");
            return s.Split(',');
        }

        private static void addRefBackNames(XElement query)
        {

            if (query.Attribute("name") == null) return;

            List<string> ss = new List<string>();



            foreach (XElement queryC in query.Elements("from").Elements("query").Elements("call").Where(e => e.Parent.Attribute("dname") != null))
            {
                ss.Add(queryC.Parent.Attribute("name").Value + "-" + queryC.Parent.Attribute("dname").Value);
            }

            foreach (XElement queryC in query.Elements("push").Elements("from").Elements("query").Elements("call").Where(e => e.Parent.Attribute("dname") != null))
            {
                ss.Add(queryC.Parent.Attribute("name").Value + "-" + queryC.Parent.Attribute("dname").Value);
            }

            foreach (XElement queryC in query.Elements("from").Elements("query").Elements("call").Where(e => e.Parent.Attribute("dname") == null))
            {
                if (queryC.Parent.Attribute("name") != null)
                {
                    if (!ss.Contains(queryC.Parent.Attribute("name").Value + "-" + queryC.Parent.Parent.Parent.Attribute("name").Value))
                    {
                        queryC.Parent.SetAttributeValue("dname", queryC.Parent.Parent.Parent.Attribute("name").Value);
                    }
                }
            }

            foreach (XElement queryC in query.Elements("push").Elements("from").Elements("query").Elements("call").Where(e => e.Parent.Attribute("dname") == null))
            {
                if (queryC.Parent.Attribute("name") != null)
                {
                    if (!ss.Contains(queryC.Parent.Attribute("name").Value + "-" + queryC.Parent.Parent.Parent.Parent.Attribute("name").Value))
                    {
                        queryC.Parent.SetAttributeValue("dname", queryC.Parent.Parent.Parent.Parent.Attribute("name").Value);
                    }
                }
            }
        }

        private static void addReportForms(VSXElement compiling)
        {
            foreach (XElement rep in compiling.Elements("reports").Elements("report").Where(e => e.Attribute("form") == null))
            {
                rep.SetAttributeValue("form", "empty");
            }
        }

        private static void addRepColTbleAlias(VSXElement compiling)
        {

            foreach (XElement el in compiling.Elements("reports").Elements("report").Descendants("query").Elements("columns").Descendants("column").Where(e => e.Attribute("table") == null))
            {

                el.SetAttributeValue("table", el.Ancestors("query").First().Attribute("as").Value);
            }
        }
        internal static void forCustomersProcessing(IEnumerable<VSXElement> element, string customer)
        {
            element.Descendants().Where(e => e.Attribute("forcustomers") != null && !e.Attribute("forcustomers").Value.Split(',').Contains(customer.ToString())).Remove();
            element.Descendants().Where(e => e.Attribute("notforcustomers") != null && e.Attribute("notforcustomers").Value.Split(',').Contains(customer.ToString())).Remove();
        }
        private static void changeExtLinksAlias(XElement query)
        {
            foreach (XElement extLinks in query.Descendants(TextConst.EName.ExtendLinks))
            {
                var xwheres = extLinks.Parent.Elements(TextConst.EName.ExtendWhere).ToList();

                foreach (XElement link in extLinks.Descendants().Where(e => TextConst.ENameArray.ALinks.Contains(e.Name.LocalName)).ToList())
                {
                    string newAlias = link.Attribute(TextConst.AName.As).Value + TextConst.Pfx.ExtLink;
                    foreach (XElement col in xwheres.Descendants(TextConst.EName.Column).Where(e => getAttrValue(e, TextConst.AName.Table) == link.Attribute(TextConst.AName.As).Value).ToList())
                    {
                        col.SetAttributeValue(TextConst.AName.Table, newAlias);
                    }

                    link.SetAttributeValue(TextConst.AName.As, newAlias);
                }

            }
        }
        private static void addLinkInfo(XElement root)
        {
            Contract.Assume(root != null);
            foreach (XElement xfrom in root.Descendants(EName.from)) {
                XElement query = xfrom.Ancestors().First();
                foreach (XElement qry in xfrom.Elements()) {
                    if (EPredicate.IsQueryOrTable(qry)) {
                        XElement classScheme = getQueryScheme(qry.AttrOrDefault(AName.name, string.Empty));
                        if (classScheme != null) {
                            qry.CopyAttributes(classScheme.Attributes(AName.title));
                        }
                        foreach (XElement el in qry.Elements()) {
                            if (EPredicate.IsAnyLink(el)) {
                                addLinkInfoI(el, query, null);
                            }
                        }
                        foreach (XElement el in qry.Elements(TextConst.EName.ExtendLinks).Elements()) {
                            if (EPredicate.IsAnyLink(el)) {
                                addLinkInfoI(el, query, null);
                            }
                        }
                    }
                }
            }
        }
        private static void addLinkInfoI(XElement link, XElement query, IEnumerable<XElement> selColumns)
        {
            Contract.Assume(link != null);
            Contract.Assume(EPredicate.IsAnyLink(link));
            Contract.Assume(link.Attribute(AName.name) != null);
            string link_name = link.Attribute(AName.name).Value;
            if (link.Attribute(AName.@as) == null) {
                link.Add(new XAttribute(AName.@as, link_name));
            }
            string linkerTableName;
            string linkerTableName1 = null;
            XElement parent = null;
            if (link.Parent.Name.LocalName == TextConst.EName.ExtendLinks) {
                // смущает меня это место - модифицируются элементы из схемы
                XElement query2 = getQueryScheme(link.Parent.Parent.Attribute(AName.name).Value);
                var froms = query2.Elements(EName.from).ToList();
                froms.AddRange(query2.Elements(EName.push).Elements(EName.from));
                parent = froms.Elements(EName.query).First(e => e.AttrOrEmpty(AName.@as) == link.Parent.Attribute(TextConst.AName.Target).Value);
            } else {
                parent = link.Parent;
            }
            if (parent.Name == AName.table && query.Attribute(AName.inherit) != null) {
                linkerTableName = query.AttrOrDefault(AName.name, string.Empty);
                linkerTableName1 = getLQTableName(parent);
            } else {
                linkerTableName = getLQTableName(parent);
            }
            if (link.Name == EName.slink) {
                link.SetAttributeValue(AName.table, link_name);
            } else {
                XElement linkerQueryScheme = null;
                XElement linkedQuery = null;
                if (link.Attribute("recursive") != null) {
                    link.SetAttributeValue("pushpred", 1);
                }
                if (link.Name == EName.link) {
                    linkerQueryScheme = getQueryScheme(linkerTableName);
                    if (linkerQueryScheme != null) {
                        linkedQuery = linkerQueryScheme.Elements(EName.from).Elements(EName.query).FirstOrDefault(e => e.Attribute(AName.@as).Value == link_name);
                        if (linkedQuery == null) {
                            linkedQuery = linkerQueryScheme.Elements(EName.push).Elements(EName.from).Elements(EName.query).FirstOrDefault(e => e.Attribute(AName.@as).Value == link_name);
                        }
                        if (linkedQuery == null) {
                            throw new VCompilerException("Не найдена связь ", query, link);
                        }
                        linkedQuery = new XElement(linkedQuery);
                        link.SetAttributeValue(AName.table, linkedQuery.Attribute(AName.name).Value);
                    }
                } else {
                    //var tabName = link.Attribute("table").Value;
                    if (getAttrValue(query, "pushpred") == "1") {
                        link.SetAttributeValue("pushpred", 1);
                    }
                    if (linkerTableName1 == null) {
                        var qscheme = getQueryScheme(linkerTableName);
                        if (qscheme.Attribute(AName.inherit) != null) {
                            linkerTableName1 = qscheme.Attribute(AName.inherit).Value;
                        }
                    }
                    if (linkerTableName1 != null) {
                        linkedQuery = XmlReports.Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).Elements(EName.from).Elements(EName.query).FirstOrDefault(e => e.AttrOrDefault(TextConst.AName.DName, string.Empty) == link_name && e.AttrOrDefault(AName.name, string.Empty) == linkerTableName);
                        if (linkedQuery == null) {
                            linkerTableName = linkerTableName1;
                        }
                    }
                    if (linkedQuery == null) {
                        linkedQuery = XmlReports.Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).Where(q => q.Attribute(AName.inherit) == null).Elements(EName.from).Elements(EName.query).FirstOrDefault(e => e.AttrOrDefault(TextConst.AName.DName, string.Empty) == link_name && e.AttrOrDefault(AName.name, string.Empty) == linkerTableName);
                    }
                    if (linkedQuery == null) {
                        linkedQuery = XmlReports.Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).Elements(EName.from).Elements(EName.query).FirstOrDefault(e => e.AttrOrDefault(AName.dname, string.Empty) == link_name && e.AttrOrDefault(AName.name, string.Empty) == linkerTableName);
                    }
                    if (linkedQuery == null) {
                        linkedQuery = XmlReports.Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).Elements(EName.push).Elements(EName.from).Elements(EName.query).FirstOrDefault(e => e.AttrOrDefault(AName.dname, string.Empty) == link_name && e.AttrOrDefault(AName.name, string.Empty) == linkerTableName);
                        if (linkedQuery != null) {
                            linkerQueryScheme = linkedQuery.Parent.Parent.Parent;
                        }
                    } else {
                        linkerQueryScheme = linkedQuery.Parent.Parent;
                    }
                    if (linkerQueryScheme != null) {
                        link.SetAttributeValue(AName.table, linkerQueryScheme.Attribute(AName.name).Value);
                    }
                }
                if (linkedQuery != null) {
                    link.SetAttributeValue("child", linkerQueryScheme.Attribute(AName.name).Value);
                    link.SetAttributeValue("field", linkedQuery.Attribute(AName.@as).Value);
                    link.SetAttributeValue("parent", linkedQuery.Attribute(AName.name).Value);
                    link.SetAttributeValue("back", linkedQuery.Attribute(AName.dname).Value);
                }
            }
            XAttribute tattr = link.Attribute(AName.table);
            if (tattr != null) {
                XElement classScheme = getQueryScheme(tattr.Value);
                if (classScheme != null) {
                    link.CopyAttributes(classScheme.Attributes(AName.title));
                }
                if (link.Parent.Attribute(AName.title) != null) {
                    foreach (XElement vidCol in classScheme.Elements(EName.select).Elements().Where(e => getAttrValue(e, "vid") == "1")) {
                        if (selColumns == null) {
                            selColumns = getQueryColumnsSel(query);
                        }
                    }
                }
            }
            foreach (XElement el in link.Elements()) {
                if (EPredicate.IsAnyLink(el)) {
                    addLinkInfoI(el, query, selColumns);
                }
            }
        }
        /// <summary>
        /// Преобразовывает массивы &lt;array&gt;1,2,3&lt;/array&gt; и 
        /// &lt;array&gt;&lt;const&gt;1&lt;/const&gt;&lt;const&gt;2&lt;/const&gt;&lt;const&gt;3&lt;/const&gt;&lt;/array&gt;
        /// к виду &lt;call function="array"&gt;&lt;const&gt;1&lt;/const&gt;&lt;const&gt;2&lt;/const&gt;&lt;const&gt;3&lt;/const&gt;&lt;/call&gt;
        /// </summary>
        /// <param name="root">Элемент, в котором нужно преобразовать массивы</param>
        internal static void processingArrays(XElement root)
        {
            Contract.Assert(root != null);
            IList<XElement> arrays = root.Descendants(EName.array).ToList(); 
            for (int index = 0; index < arrays.Count; index++) {
                XElement el = arrays[index];
                XElement call = Factory.NewCall(TextConst.AVFunction.Array);
                if (!el.HasElements) {
                    // переобразование <array>1,2,3</array> в <call function="array"><const>1</const><const>2</const><const>3</const></call>
                    string s = el.Value;
                    if (!string.IsNullOrEmpty(s)) {
                        el.Value = string.Empty;
                        string[] items = s.Split(',');
                        for (int index_2 = 0; index_2 < items.Length; index_2++) {
                            call.Add(Factory.NewConst(items[index_2].Trim()));
                        }
                    }
                } else {
                    el.Elements().ChangeParent(call);
                }
                el.ReplaceWith(call);
            }
        }
        private static void processingUsing(XElement element)
        {
            Contract.Assert(element != null);
            IList<XElement> usings = element.Descendants(EName.@using).ToList();
            for (int index = 0; index < usings.Count; index++) {
                XElement el = usings[index];
                string alias;
                XElement query = el.Parent;
                Contract.Assert(query != null && query.Name == EName.query);
                XElement from = query.Parent;
                Contract.Assert(from != null && from.Name == EName.from);
                if (from.Parent.Name != EName.push) {
                    // алиас первого query в том же from
                    alias = from.Elements().First().Attribute(AName.@as).Value;
                } else {
                    alias = "*";
                }
                string alias_2 = query.Attribute(AName.@as).Value;
                XElement call = Factory.NewCall(TextConst.AVFunction.And);
                foreach (XElement el_col in el.Elements()) {
                    string col_name = el_col.Attribute(AName.column).Value;
                    XElement call_col = Factory.NewCall(TextConst.AVFunction.Equal,
                        Factory.NewColumn(table: alias_2, column: col_name),
                        Factory.NewColumn(table: alias,   column: col_name)
                    );
                    call.Add(call_col);
                }
                el.ReplaceWith(call);
            }
        }
        private static void processingParts(XElement root)
        {
            //пока предварительная обработка только для форм, сделать для всех с учетом параметров
            IList<XElement> list = root.Descendants(EName.form).Where(e => e.Descendants(EName.usepart).Any()).ToList();
            int index;
            XElement el;
            for (index = 0; index < list.Count; index++) {
                el = list[index];
                IEnumerable<XNode> el1 = copyApplyPart(el);
                el.ReplaceWith(el1);
            }
            list = root.Descendants(EName.usepart).Where(e => !e.Descendants(EName.useparam).Any() && !e.Descendants(EName.usepart).Any()).ToList();
            for (index = 0; index < list.Count; index++) {
                el = list[index];
                IEnumerable<XNode> el1 = usepart(el);
                el.ReplaceWith(el1);
            }
        }
        /// <summary>
        /// Заменяет названия цветов на их числовые значения RGB,
        /// например &lt;use-color color="black" /&gt; заменяет на &lt;const&gt;'0,0,0'&lt;/const&gt;.
        /// Основные цвета указаны в sql.builder.templates\sql.builder\projects\common\colors.xml
        /// </summary>
        /// <param name="root">Элемент, в котором нужно заменить названия цветов</param>
        private static void processingUseColor(XElement root)
        {
            IList<XElement> list = root.Descendants(EName.use_color).ToList();
            if (list.Count > 0) {
                IDictionary<string, string> colors = new Dictionary<string, string>();
                for (int index = 0; index < list.Count; index++) {
                    XElement el = list[index];
                    string color = el.Attribute(AName.color).Value;
                    string rbg;
                    if (!colors.TryGetValue(color, out rbg)) {
                        XElement xclr = XmlReports.Environment.Manager.GetNativeScheme().Elements(EName.color_packages).Elements(EName.color_package).Elements(EName.color).SearchByAttribute(AName.name, color);
                        if (xclr == null) {
                            throw new VCompilerException("Не найден цвет \"" + color + "\".", null, el);
                        }
                        rbg = xclr.Attribute(AName.rgb).Value;
                        colors.Add(color, rbg);
                    }
                    XElement xcnst = Factory.NewConst("'" + rbg + "'");
                    el.ReplaceWith(xcnst);
                }
            }
        }
        private static void fakeParams(XElement query)
        {
            if (query.Element(EName.@params) == null && query.Descendants(EName.useglobparam).Any()) {
                query.Add(new XElement(EName.@params));
            }
        }
        private static void grSetSpecColumns(XElement query)
        {

            //if (getAttrValue(query, TextConst.AName.Name) == "10653(40)-new")
            //{

            //}

            foreach (XElement grsets in query.Descendants("grsets"))
            {
                XElement queryCall = grsets.Parent;

                if (queryCall.Parent != null)
                {

                    XElement parentQuery = queryCall.Parent.Parent;

                    if (queryCall.Parent.Name.LocalName == TextConst.EName.From)
                    {

                        string queryCallAlias = queryCall.Attribute("as").Value;
                        XElement col = parentQuery.Element("select").Elements("column").FirstOrDefault(e => e.Attribute("table").Value == queryCallAlias && e.Attribute("column").Value == "*");

                        if (col == null)
                        {
                            foreach (string specColName in gsetsSpecColsNamesOnlyOnRows)
                            {
                                col = new XElement("column", new XAttribute("table", queryCallAlias), new XAttribute("column", specColName));
                                parentQuery.Element("select").AddFirst(col);
                            }
                        }

                        // это нужно чтобы при формировании excel через datareader соответствовал порядок родительских и дочерних строк
                        // могут быть побочные ошибки
                        var cnct = new XElement(EName.connect,
                            new XElement(EName.call, new XAttribute(AName.function, TextConst.AVFunction.Equal)
                                , new XElement(EName.column, new XAttribute(AName.table, queryCall.Attribute(AName.@as).Value), new XAttribute(AName.column, TextConst.AVSpecColumnGrset.ParentGrRowId))
                                , new XElement(EName.column, new XAttribute(AName.table, queryCall.Attribute(AName.@as).Value), new XAttribute(AName.column, TextConst.AVSpecColumnGrset.GrRowId), new XAttribute(TextConst.AName.Prior, TextConst.AVBool.True))
                          ));
                        parentQuery.Add(cnct);
                        var strt = new XElement(EName.start,
                             new XElement(EName.call, new XAttribute(AName.function, TextConst.AVFunction.IsNull)
                                 , new XElement(EName.column, new XAttribute(AName.table, queryCall.Attribute(AName.@as).Value), new XAttribute(AName.column, TextConst.AVSpecColumnGrset.ParentGrSetId))
                           ));
                        parentQuery.Add(strt);
                        parentQuery.SetAttributeValue(TextConst.AName.Order, TextConst.AVSpecColumnGrset.GrRowNum);
                    }



                }
            }


        }

        private static void ProcessQueryBandsLevel(XElement parent, XElement viewColsParent, List<XElement> selCols) 
        {
            foreach (var el in parent.Elements())
            {
                if (el.Name.LocalName == TextConst.EName.Band)
                {
                    var xband = new XElement(TextConst.EName.Band);
                    copyAttributes(el, xband);
                    viewColsParent.Add(xband);
                    ProcessQueryBandsLevel(el, xband, selCols);
                }
                else
                {
                    selCols.Add(el);

                    var alias = getAttrValue(el, TextConst.AName.As);
                    if (alias == "")
                    {
                        alias = getAttrValue(el, TextConst.AName.Column);
                    }
                    var xcol = new XElement(TextConst.EName.Column);
                    xcol.SetAttributeValue(TextConst.AName.Name, alias);
                    viewColsParent.Add(xcol);
                }
            }
        }
        private static void ProcessQueryBands(XElement query) // !!! Последить за временем
        {
            if (getAttrValue(query, TextConst.AName.IsReport) != TextConst.AVBool.True)
            {
                return;
            }

            if (!query.Elements(TextConst.EName.Select).Elements(TextConst.EName.Band).Any())
            {
                return;
            }

            var xcols = new XElement(TextConst.EName.Columns);
            var selCols = new List<XElement>();

            ProcessQueryBandsLevel(query.Element(TextConst.EName.Select), xcols, selCols);
            query.Element(TextConst.EName.Select).Elements().Remove();
            query.Element(TextConst.EName.Select).Add(selCols);
            query.AddFirst(xcols);
        }
        private static void ApplyAddition(XElement query) // !!! Последить за временем
        {
            var additionsSchemes = new List<XElement>();
            if (query.Elements(TextConst.EName.Additions).Any())
            {
                foreach (XElement addition in query.Elements(TextConst.EName.Additions).Elements())
                {
                    var qry = getQueryScheme(getAttrValue(addition, TextConst.AName.Name));
                    //additionsSchemes.Add(new XElement(qry));
                    additionsSchemes.Add(new XElement(qry));
                }
                ApplyAddition(query, additionsSchemes);
                query.Elements(TextConst.EName.Additions).Remove();
            }


        }

        private static void ApplyAdditionLinks(XElement target, List<XElement> source) // !!! Последить за временем
        {
            if (source.Elements(TextConst.EName.From).Elements(TextConst.EName.Qube).Any())
            {

                //foreach (XElement el in sourceContent.Elements(TextConst.EName.Link))
                //{
                //    el.SetAttributeValue(TextConst.AName.OnlyForCond, TextConst.AVBool.True);
                //}

                if (!target.Elements(TextConst.EName.From).Any())
                {
                    target.Add(new XElement(TextConst.EName.From));
                }
                if (!target.Elements(TextConst.EName.From).Elements(TextConst.EName.Qube).Any())
                {
                    target.Add(new XElement(TextConst.EName.Qube));
                }

                var sourceContentElements = new List<XElement>();
                var qubeWhereByAddition = new SortedList<string, XElement>();
                var qubeByAddition = new SortedList<string, XElement>();
                var whereByAddition = new SortedList<string, XElement>();
                XElement targetContent = target.Element(TextConst.EName.From).Element(TextConst.EName.Qube);
                qubeWhereByAddition.Add("", targetContent.Element(TextConst.EName.Where));
                whereByAddition.Add("", target.Element(TextConst.EName.Where));
                foreach (XElement source1 in source)
                {
                    var sourceContent1 = source1.Elements(TextConst.EName.From).Elements(TextConst.EName.Qube).Elements(TextConst.EName.Link).Select(e => new XElement(e)).ToList();
                    foreach (XElement el in sourceContent1.DescendantsAndSelf())
                    {
                        el.SetAttributeValue(TextConst.AName.Addition, source1.Attribute(TextConst.AName.Name).Value);
                    }
                    sourceContentElements.AddRange(sourceContent1);
                    var xwhere = source1.Elements(TextConst.EName.From).Elements(TextConst.EName.Qube).Elements(TextConst.EName.Where).FirstOrDefault();
                    if (xwhere != null)
                    {
                        xwhere = new XElement(xwhere);
                    }
                    qubeWhereByAddition.Add(source1.Attribute(TextConst.AName.Name).Value, xwhere);
                    xwhere = source1.Elements(TextConst.EName.Where).FirstOrDefault();
                    if (xwhere != null)
                    {
                        xwhere = new XElement(xwhere);
                    }
                    whereByAddition.Add(source1.Attribute(TextConst.AName.Name).Value, xwhere);
                    qubeByAddition.Add(source1.Attribute(TextConst.AName.Name).Value, source1.Elements(TextConst.EName.From).Elements(TextConst.EName.Qube).FirstOrDefault());
                }
                targetContent.Add(sourceContentElements);
                var trgElements = targetContent.Elements(TextConst.EName.Link).ToList();

                while (trgElements.Count != 0)
                {
                    var idXElement = new SortedList<string, XElement>();
                    var idAlias = new SortedList<string, XElement>();
                    var nameCounter = new SortedList<string, int>();
                    foreach (XElement grp in trgElements.ToList())
                    {
                        var id = grp.Name.LocalName;
                        string name = "";
                        name = getAttrValue(grp, TextConst.AName.Name);
                        id += "|" + name;
                        var additionName = getAttrValue(grp, TextConst.AName.Addition);
                        bool isNew = false;
                        if (!idXElement.ContainsKey(id))
                        {
                            idXElement.Add(id, grp);
                            isNew = true;
                        }
                        else
                        {
                            if (idXElement[id].Parent == grp.Parent)
                            {
                                idXElement[id].Add(grp.Elements());
                                trgElements.Remove(grp);
                                grp.Remove();
                            }
                            else
                            {
                                idXElement[id] = grp;
                                isNew = true;
                            }
                        }
                        string alias = null;
                        if (isNew)
                        {
                            if (!nameCounter.ContainsKey(name))
                            {
                                nameCounter.Add(name, 0);

                            }
                            else
                            {
                                nameCounter[name]++;
                            }
                            if (additionName != "")
                            {
                                grp.SetAttributeValue(TextConst.AName.OnlyForCond, TextConst.AVBool.True);
                                grp.Attributes(TextConst.AName.AllRows).Remove();
                            }

                        }

                        alias = name;
                        if (nameCounter[name] > 0)
                        {
                            alias += "_" + nameCounter[name].ToString();
                        }
                        var oldAlias = getAttrValue(grp, TextConst.AName.As);
                        if (oldAlias == "")
                        {
                            oldAlias = getAttrValue(grp, TextConst.AName.Name);
                        }


                        var qubeXwhere = qubeWhereByAddition[additionName];
                        if (qubeXwhere != null)
                        {
                            foreach (XElement col in qubeXwhere.Descendants(TextConst.EName.Column).Where(c => getAttrValue(c, TextConst.AName.Table) == oldAlias).ToList())
                            {
                                col.SetAttributeValue(TextConst.EName.Table, alias);
                            }
                        }

                        var xwhere = whereByAddition[additionName];
                        if (xwhere != null)
                        {
                            foreach (XElement col in xwhere.Descendants(TextConst.EName.Column).Where(c => getAttrValue(c, TextConst.AName.Table) == oldAlias).ToList())
                            {
                                col.SetAttributeValue(TextConst.EName.Table, alias);
                            }


                        }

                        if (additionName == "")
                        {

                            foreach (XElement col in target.Elements(TextConst.EName.Select).Descendants(TextConst.EName.Column).Where(c => getAttrValue(c, TextConst.AName.Table) == oldAlias).ToList())
                            {
                                col.SetAttributeValue(TextConst.EName.Table, alias);
                            }
                        }



                        if (isNew)
                        {
                            grp.SetAttributeValue(TextConst.AName.As, alias);
                        }



                    }
                    trgElements = trgElements.Elements(TextConst.EName.Link).ToList();
                }



                foreach (string additionName in qubeWhereByAddition.Keys)
                {
                    XElement xwhere = null;
                    if (additionName != "")
                    {
                        xwhere = qubeWhereByAddition[additionName];
                    }
                    if (xwhere != null)
                    {
                        if (!targetContent.Elements(TextConst.EName.Where).Any())
                        {
                            targetContent.Add(new XElement(TextConst.EName.Where));
                        }
                        targetContent.Element(TextConst.EName.Where).Add(xwhere.Elements());
                    }
                }

                foreach (string additionName in whereByAddition.Keys)
                {
                    XElement xwhere = null;
                    if (additionName != "")
                    {
                        xwhere = whereByAddition[additionName];
                    }
                    if (xwhere != null)
                    {

                        //List<string> dimnames = qubeByAddition[additionName].Elements(TextConst.EName.Link).Where(e => e.Attribute(TextConst.AName.OnlyForCond) == null).Select(e1 => e1.Attribute(TextConst.AName.Name).Value).ToList();
                        //dimnames.OrderBy(e2 => e2);
                        //string dimsid = string.Join(",", dimnames);
                        //foreach (XElement col in xwhere.Descendants(TextConst.EName.Fact))
                        //{
                        //    col.SetAttributeValue(TextConst.EName.DimSet, dimsid);
                        //}

                        if (!target.Elements(TextConst.EName.Where).Any())
                        {
                            target.Add(new XElement(TextConst.EName.Where));
                        }
                        target.Element(TextConst.EName.Where).Add(xwhere.Elements());
                    }
                }
                targetContent.Descendants().Attributes(TextConst.AName.Addition).Remove();


            }

        }
        private static void ApplyAdditionContent(XElement target, List<XElement> source) // !!! Последить за временем
        {
            if (source.Elements(TextConst.EName.Content).Any())
            {
                var sourceContent = source.Elements(TextConst.EName.Content).ToList();
                if (!target.Elements(TextConst.EName.Content).Any())
                {
                    target.Add(new XElement(TextConst.EName.Content));
                }
                var targetContent = target.Element(TextConst.EName.Content);

                if (targetContent.Element(TextConst.EName.TabContainer) != null)
                {

                    targetContent = targetContent.Element(TextConst.EName.TabContainer);
                }

                targetContent.Add(sourceContent.Elements());
                var trgElements = targetContent.Elements(TextConst.EName.FieldGroup).ToList();

                while (trgElements.Count != 0)
                {
                    var names = new SortedList<string, XElement>();
                    foreach (XElement grp in trgElements.ToList())
                    {
                        var title = getAttrValue(grp, TextConst.AName.Title);
                        if (!names.ContainsKey(title))
                        {
                            names.Add(title, grp);
                        }
                        else
                        {
                            if (names[title].Parent == grp.Parent)
                            {
                                names[title].Add(grp.Elements());
                                trgElements.Remove(grp);
                                grp.Remove();
                            }
                            else
                            {
                                names[title] = grp;
                            }
                        }

                    }
                    trgElements = trgElements.Elements(TextConst.EName.FieldGroup).ToList();
                }



            }
        }

        private static void ApplyAddition(XElement target, List<XElement> source) // !!! Последить за временем
        {
            ApplyAdditionContent(target, source);
            ApplyAdditionLinks(target, source);
        }
        private static void Inheritance(XElement query, XElement parent)
        {

            //foreach (XElement query in schemeRoot.Descendants("queries").Elements("query").Where(e => e.Attribute("inherit")!=null))

            if (query.Attribute("inherit") != null)
            {


                //if (query.Attribute(TextConst.AName.Name).Value == "ipr_contr_steps_val_dt_mat")
                //{

                //}
                XElement extensionScheme = parent;
                if (extensionScheme == null)
                {
                    extensionScheme = getQueryScheme(query.Attribute("inherit").Value);
                }
            
                if (query.Element("select") == null)
                {
                    query.Add(new XElement("select"));
                }

                if (extensionScheme.Element("params") != null)
                {
                    if (query.Element("params") == null)
                    {
                        query.AddFirst(new XElement("params"));
                    }
                    query.Element("params").Add(extensionScheme.Elements("params").Elements().Select(e => new XElement(e)));
                }

                SortedList<string, XElement> usedNames = new SortedList<string, XElement>();
                foreach (XElement fld in query.Element(TextConst.EName.Select).Elements())
                {
                   
                    if (usedNames.ContainsKey(fld.Attribute(TextConst.AName.As).Value))
                    {
                        throw new VCompilerException("Повторяющееся имя колонки", query, fld);
                    }
                    usedNames.Add(fld.Attribute(TextConst.AName.As).Value, fld);
                }
                
               List<XElement> colsToAdd = new List<XElement>();
               foreach (XElement el1 in extensionScheme.Elements("select").Elements().ToArray())
               {
                   if (el1.Attribute(TextConst.AName.As) == null)
                   {
                        throw new VCompilerException("Недопустимый элемент", extensionScheme, el1);
                   }
                   if (!usedNames.ContainsKey(el1.Attribute(TextConst.AName.As).Value))
                   {
                       colsToAdd.Add(el1);
                   }
                   else
                   {
                       var trgCol = usedNames[el1.Attribute(TextConst.AName.As).Value];
                       if (Cmn.GetAttrValue(trgCol, TextConst.AName.Table) == TextConst.AVTable.Ths)
                       {
                           trgCol.Remove();
                           var col = new XElement(el1);
                          trgCol.Attributes(TextConst.AName.Table).Remove();
                          trgCol.Attributes(TextConst.AName.Column).Remove();
                   
                          col.CopyAttributes(trgCol.Attributes());
                          colsToAdd.Add(col);
                      
                       }
                   }
               }
               
                query.Element("select").AddFirst(colsToAdd);
                 
                if (query.Element("from") == null)
                {
                    query.Add(new XElement("from"));
                }

                var srcs = extensionScheme.Elements("from").Elements().Select(e => new XElement(e)).ToList();


                srcs.Attributes(TextConst.AName.DName).Where(e => e.Value == query.Attribute("inherit").Value).Remove();

                query.Element("from").AddFirst(srcs);

                if (extensionScheme.Element("where") != null)
                {
                    XElement el = query.Element("where");
                    if (el == null)
                    {
                        query.Add(new XElement("where"));
                        el = query.Element("where");
                    }
                    else
                    {
                        el = query.Element("where").Elements("call").FirstOrDefault(e => getAttrValue(e, "function") == "and");
                        if (el == null)
                        {
                            el = new XElement("call", new XAttribute("function", "and"));


                            el.Add(query.Element("where").Elements("call"));
                            query.Element("where").ReplaceAll(el);
                        }
                    }
                    el.Add(extensionScheme.Elements("where").Elements().Select(e => new XElement(e)));
                }

                query.Elements("from").Elements().First().Add(query.Elements("links").Elements());



            }

        }



        /*  private static void addKeyMarks()
          {
              foreach (XElement query in schemeRoot.Descendants("queries").Elements("query").Where(e=>e.Elements("from").Elements("table").Count()!=0))
              {
                  query.Element("select").Elements().First().SetAttributeValue("key", "1");
                  foreach (XElement col in query.Element("select").Elements().Where(e1 => getAttrValue(e1, "key") != "1"))
                  {
                      col.SetAttributeValue("key", "0");
                  }
              }

              foreach (XElement query in schemeRoot.Descendants("queries").Elements("query").Where(e => e.Element("select").Elements().Where(e1=>getAttrValue(e1,"key")=="1").Count()==0))
              {
                  foreach (XElement col in query.Element("select").Elements().Where(e1 => getAttrValue(e1, "group") == "1"))
                  {
                      col.SetAttributeValue("key", "0");
                  }
              }
           

          }*/

        private static void markQueryKeys(XElement query)
        {
            if (query == null)
            {
                return;
            }

            //if (getAttrValue(query, "name") == "20498_cumulative")
            //{
            //    //     int iiii = 1;
            //}

            bool hasKeys = false;
            if (query.Element("select") != null && query.Elements("select").Elements().All(e1 => getAttrValue(e1, "key") != "1")) //!!!Можно ускорить
            {
                bool grKey = false;
                if (query.Elements("from").Elements("table").Count() != 0)
                {
                    query.Element("select").Elements().First().SetAttributeValue("key", "1");
                    query.Element("select").Elements().First().SetAttributeValue("fixed", "1");
                    hasKeys = true;
                }
                else
                {
                    if (getAttrValue(query, "grouplevel") != "no")
                    {
                        bool hasDimensions = query.Element("select").Elements().Any(e1 => getAttrValue(e1, "table") == "*");
                        foreach (XElement col in query.Element("select").Elements().Where(e1 => getAttrValue(e1, "group") == "1"))
                        {
                            grKey = true;
                            hasKeys = true;
                            col.SetAttributeValue("key", "1");
                            if (!hasDimensions)
                            {
                                col.SetAttributeValue("fixed", "1");
                            }
                        }
                    }
                    if (!grKey)
                    {
                        XElement sourceQuery = query.Elements("from").Elements("query").FirstOrDefault();

                        if (sourceQuery == null)
                        {
                            return;
                        }

                        XElement sourceQuery1 = sourceQuery;
                        //while (nvl(sourceQuery.Element("query"), sourceQuery.Element("union")) != null)
                        //    sourceQuery = (XElement)nvl(sourceQuery.Element("query"), sourceQuery.Element("union"));
                        //}
                        while (true) {
                            XElement e = sourceQuery.Element(EName.query);
                            if (e == null) {
                                e = sourceQuery.Element("union");
                                if (e == null) {
                                    break;
                                }
                            }
                            sourceQuery = e;
                        }
                        if (sourceQuery.Element("select") == null)
                        {
                            sourceQuery = getQueryScheme(getAttrValue(sourceQuery, "name"));
                        }
                        if (sourceQuery == null)
                        {
                            return;
                        }

                        markQueryKeys(sourceQuery);

                        IEnumerable<XElement> sourceKeys = sourceQuery.Elements("select").Elements().Where(e1 => getAttrValue(e1, "key") == "1").ToArray();

                        int keysCounter = 0;
                        foreach (XElement sourceKey in sourceKeys)
                        {
                            XElement keyCol = query.Element("select").Elements("column").FirstOrDefault(e => getAttrValue(e, "table") == sourceQuery1.Attribute("as").Value && e.Attribute("column").Value == sourceKey.Attribute("as").Value);
                            if (keyCol != null)
                            {
                                keyCol.SetAttributeValue("key", 1);

                                copyAttribute(sourceKey, keyCol, "fixed");
                                //  keyCol.SetAttributeValue("fixed", "1");
                                hasKeys = true;
                                keysCounter++;
                            }
                        }
                        if (keysCounter < sourceKeys.Count())
                        {
                            foreach (XElement col in query.Element("select").Elements())
                            {
                                //             grKey = true;
                                col.SetAttributeValue("key", "0");
                                col.Attributes("fixed").Remove();
                            }
                            hasKeys = false;
                        }


                    }
                }
                foreach (XElement col in query.Element("select").Elements().Where(e1 => getAttrValue(e1, "key") != "1"))
                {
                    col.SetAttributeValue("key", "0");
                }

            }
            else
            {
                if (query.Element("select") != null)
                {
                    hasKeys = true;
                }
            }

            if (hasKeys)
            {
                query.SetAttributeValue("haskeys", "1");
                /*if (query.Parent.Name.LocalName == "union")
                {
                    foreach (XElement query1 in query.Parent.Elements())
                    {
                        query1.SetAttributeValue("haskeys", "1");
                        int i = 0;
                        foreach (XElement col in query1.Element("select").Elements())
                        {
                            copyAttribute(query.Elements().ElementAt(i), col, "key");
                               
                            i++;

                        }
                    }
                }
                else
                {
                     query.SetAttributeValue("haskeys", "1");
                }*/
            }

        }



        /*
         
        public static XElement copySelfKeys(XElement element)
        {
            //.Where(e => (new string[] { "const", "column", "call" }).Contains(e.Name.LocalName) & e.Attribute("as") != null)
            foreach (XElement el in element.DescendantsAndSelf().Elements("select").Where(e => e.Elements().Where(e1 => getAttrValue(e1, "key") == "1").Count() == 0))
            {
                bool hasKeys = false;

                foreach (XElement el1 in el.Elements())
                {
                    if (selfKeys(el1))
                    {
                        hasKeys = true;
                    }
                    
                }
                if (hasKeys)
                {
                    foreach (XElement col in el.Elements().Where(e1 => getAttrValue(e1, "key") != "1"))
                    {
                        col.SetAttributeValue("key", "0");
                    }
                }
                
            }
           
            
            return element;
        }

        public static bool selfKeys(XElement element)
        {
            if (getAttrValue(element,"group") == "1")
            {
                element.SetAttributeValue("key", "1");
                return true;
            }
            else
            {
                string tableAlias = getAttrValue(element, "table");
                if (element.ElementsBeforeSelf().Count() == 0)
                {
                    if (element.Parent.Parent.Elements("from").Elements("table").Where(e => getAttrValue(e, "as") == (tableAlias)).Count() == 1)
                    {
                       
                        element.SetAttributeValue("key", "1");
                        return true;
                    }
                    else
                    {
                        if (element.Parent.Parent.Parent != null)
                        {
                            if (element.Parent.Parent.Parent.Name.LocalName == ("query"))
                            {
                                element.SetAttributeValue("key", "1");
                                return true;
                            }
                        }
                    }
                }

            }
            return false;

        }



        public static XElement copySourceKeys(XElement element)
        {
         
            foreach (XElement el in element.DescendantsAndSelf("select").Elements().Where(e => e.Attribute("key") == null))
            {

                sourceKey(el);
            }
            return element;
        }


        public static void sourceKey(XElement element)
        {

            if (getAttrValue(element, "column") == "kodp")
            {
                 string s="";
            }

            XElement keyInf = keyInfo(element);
            if (keyInf != null)
            {
                if (keyInf.Attribute("key").Value == ("1"))
                {
                    copyAttribute(keyInf, element, "key");
                    element.Add(new XAttribute("keypath", keyInf.Attribute("table").Value + "." + keyInf.Attribute("column").Value));
                }
                else
                {

                    if (keyInf.Attribute("key").Value == ("0"))
                    {
                        copyAttribute(keyInf, element, "key");
                    }
                }
            }

        }

        public static XElement keyInfo(XElement element)
        {
            string table = getAttrValue( element,"table");
            string column = getAttrValue(element, "column");
            XElement sourceQuery = element.Parent.Parent.Elements("from").Elements("query").Where(e => e.Attribute("as").Value == (table)).FirstOrDefault();

            if (sourceQuery != null & !sysColNames.Contains(column))
            {
                while (sourceQuery.Element("select") == null)
                {
                    sourceQuery = sourceQuery.Element("query");
                    if (sourceQuery == null)
                    {
                        return null;
                    }
                }
                XElement sourceColumn = sourceQuery.Element("select").Elements().Where(e => e.Attribute("as").Value == (column)).FirstOrDefault();
                if (getAttrValue(sourceColumn, "key") != (""))
                {

                    return new XElement("key-info",
                        new XAttribute("key", getAttrValue(sourceColumn, "key")),
                        new XAttribute("table", getAttrValue(sourceColumn, "path")),
                         new XAttribute("column", getAttrValue(sourceColumn, "as"))
                        );
                }
                else
                {
                    return keyInfo(sourceColumn);
                }

            }
            else
            {
                return new XElement("key-info", new XAttribute("key", "0"));
            }

        }


         */

        //static int newColIndex = 1;
        private static XElement getQueryGroupLevel(XElement element, string matName, bool isFromSelfGrsets = false)
        {
            bool isGrSets = element.Element("grsets") != null; // возможно тоже самое что isFromSelfGrsets

            //if (element.Attribute("name").Value == "35210-data")
            //{

            //}


            XElement queryScheme = null;

            if (element.Element(TextConst.EName.Select) == null)
            {
                queryScheme = getQueryScheme(element.Attribute("name").Value);
            }
            else
            {
                queryScheme = element;
            }
            bool isSimple = true;
            string groupLevel = "";
            if (!isGrSets)
            {
                groupLevel = element.Attribute("grouplevel").Value;



                if (groupLevel.Contains(",") || groupLevel == "")
                {
                    isSimple = false;
                }
            }
            else
            {
                isSimple = false;
            }

            
                if (element.Attribute(TextConst.AName.As) == null)
                {
                    element.SetAttributeValue(TextConst.AName.As, "grpd");
                }
            
            

            XElement query = new XElement("query",

                new XAttribute("as", element.Attribute("as").Value),
                 copyAttribute(element, "title"),
                 copyAttribute(element, "main"),
                new XElement("select")
                    ,
                new XElement("from")
                    , element.Elements("where")
                    .Where(e=>!e.Descendants(TextConst.EName.Fact).Any())// добавил условие т.к. при наличии фактов они не могут обработаться в верхнем запросе, скорее всего это копирование вообще не нужно, т.к. where остается и в подзапросе
                   , queryScheme.Elements("having")

                );


           

            copyAttribute(element, query, "join");
            //!!! having НЕ проверено для query с grouplevel
            if (!isFromSelfGrsets)
            {
                foreach (XElement el in query.Elements("having").Descendants("column"))
                {
                    el.Attribute("table").Value = query.Attribute("as").Value;
                }
            }
            foreach (XElement col in queryScheme.Elements("select").Elements().ToList())
            {
                XElement newCol = null;
                if (getAttrValue(col, TextConst.AName.Group) == TextConst.AVGroup.Outer)
                {
                    newCol = new XElement(col);
                    // newCol.Attributes(TextConst.AName.Group).Remove();
                    //col.Remove();
                }
                else if (getAttrValue(col, "function") == "row_num")
                {
                    newCol = new XElement(col);
                    newCol.Elements().Remove();

                    newCol.Add(
                        new XElement("column"
                           , new XAttribute("table", element.Attribute("as").Value)
                             , new XAttribute("column", col.Attribute("as").Value)
                              , new XAttribute("group", "max")
                              )
                        );
                    newCol.SetAttributeValue("group", "max");

                    if (getAttrValue(element, "grouplevel") == "no")
                    {
                        newCol.Elements().Attributes("group").Remove();
                    }
                } else {
                    string col_name = col.Attribute(AName.@as).Value;
                    newCol = Factory.NewColumn(element.Attribute(AName.@as).Value, col_name);
                    newCol.Add(new XAttribute(AName.@as, col_name));
                    newCol.Add(new XAttribute(AName.group, col.AttrOrDefault(AName.group, string.Empty)));
                    newCol.CopyAttributes(col.Attributes(TextConst.AName.Master));
                    newCol.CopyAttributes(col.Attributes(AName.c_master));
                    newCol.CopyAttributes(col.Attributes(AName.c_master_key));
                    newCol.CopyAttributes(col.Attributes(TextConst.AName.TreeOriginalColumn));
                    newCol.CopyAttributes(col.Attributes(TextConst.AName.TreeLevelColumn));
                    newCol.CopyAttributes(col.Attributes(TextConst.AName.Level));
                    newCol.CopyAttributes(col.Attributes(TextConst.AName.Removeable));
                    newCol.CopyAttributes(col.Attributes(TextConst.AName.Removeable2));
                    newCol.CopyAttributes(col.Attributes(AName.intern));
                    newCol.CopyAttributes(col.Attributes().Where(APredicate.IsAdditionalAttribute));
                }



                if (newCol.Attribute("group") != null && newCol.Attribute("group").Value == "")
                {
                    XAttribute gr = col.Descendants().Attributes("group").FirstOrDefault();
                    if (gr != null)
                    {
                        newCol.SetAttributeValue("group", gr.Value);
                    }
                }
                query.Element("select").Add(newCol);
            }

            element = new XElement(element);

            if (element.Element(TextConst.EName.Select) != null)
            {
                element.Attributes(TextConst.AName.Name).Remove();
                element.Elements(TextConst.EName.Having).Remove();
            }


            if (/*hasPivots &&*/ element.Element("withparams") != null)
            {
                query.Add(queryScheme.Elements("params"));
                query = (XElement)applyParams(query, element.Element("withparams"), false).First();
            }

            var element1 = new XElement(element);
            element1.Elements(TextConst.EName.Grsets).Remove();
            element1.Elements(TextConst.EName.Select).Elements().Attributes(TextConst.AName.Group).Remove();
            query.Element("from").Add(element1);

            query.Add(query.Element("from").Element("query").Elements("query"));
            query.Add(query.Element("from").Element("query").Elements("call"));

            query.Element("from").Element("query").Elements("query").Remove();
            query.Element("from").Element("query").Attributes("grouplevel").Remove();
            query.Element("from").Element("query").Elements("call").Remove();


            //foreach (XElement el in queryScheme.Elements("select").Elements())
            //{

            //}


            if (groupLevel == "no")
            {
                foreach (XElement el in query.Elements("select").Elements())
                {
                    el.RemoveAttribute(AName.group);
                }

            }
            else
            {

                int grLev = -1;
                List<string> grLevs = null;
                if (isGrSets)
                {
                    grLevs = element.Element("grsets").Descendants("grset").SelectMany(e => e.Attribute("level").Value.Split(',')).ToList();
                }
                else
                {
                    if (isSimple)
                    {
                        grLev = Convert.ToInt32(groupLevel);
                    }
                    else
                    {
                        grLev = -1;
                        grLevs = groupLevel.Split(',').ToList();
                    }
                }

                // Колонки аггрегацией sum убираются если детализация глубже чем master
                foreach (XElement el in query.Elements("select").Elements().Where(e => e.Attribute("master") != null).ToArray())
                {
                    XElement master = query.Elements("select").Elements().First(e => e.Attribute("as").Value == el.Attribute("master").Value);
                    if (el.Attribute("group").Value == "sum")
                    {
                        if (isSimple)
                        {

                            if (Convert.ToInt32(master.Attribute("group").Value) < grLev)
                            {
                                el.Remove();
                            }
                            else
                            {
                                //el.Attribute("master").Remove();
                            }
                        }
                        else
                        {
                            if (!grLevs.Contains(master.Attribute("group").Value))
                            {
                                el.Remove();
                            }
                            else
                            {
                                if (!isGrSets)
                                {
                                    // el.Attribute("master").Remove();
                                }
                            }

                        }
                    }
                    else
                    {
                        if (isSimple)
                        {

                            if (Convert.ToInt32(master.Attribute("group").Value) > grLev)
                            {
                                el.Remove();
                            }
                            else
                            {
                                //el.Attribute("master").Remove();
                            }
                        }
                        else
                        {
                            if (!grLevs.Contains(master.Attribute("group").Value))
                            {
                                el.Remove();
                            }
                            else
                            {
                                if (!isGrSets)
                                {
                                    //el.Attribute("master").Remove();
                                }
                            }

                        }
                    }
                }
                IEnumerable<XElement> elements = null;
                if (isSimple)
                {
                    elements = query.Elements("select").Elements().Where(e => Cmn.IsNumeric(getAttrValue(e, "group"))).ToArray();
                    elements.Where(e => Convert.ToInt32(e.Attribute("group").Value) > grLev).Remove();
                }
                else
                {
                    elements = query.Elements("select").Elements().Where(e => Cmn.IsNumeric(getAttrValue(e, "group"))).ToArray();
                    elements.Where(e => !grLevs.Contains(e.Attribute("group").Value)).Remove();
                }






                query.Elements("select").Elements().Where(e => getAttrValue(e, "group") == "").Remove();

                if (isGrSets)
                {
                    applyGroupingSets(element, query, matName);

                }
                else
                {

                    if (grLev == 0 || groupLevel == "")
                    {
                        query.Elements("select").Elements().Where(e => getAttrValue(e, "group") == "stragg_dist").Remove();//заглушка может получиться слишком большая строка
                    }


                    if (isSimple)
                    {
                        foreach (XElement el in query.Elements("select").Elements().Where(e => Cmn.IsNumeric(getAttrValue(e, "group"))).ToArray())
                        {
                            el.SetAttributeValue("group", "1");
                        }
                    }
                    else
                    {
                        foreach (XElement el in query.Elements("select").Elements().Where(e => grLevs.Contains(getAttrValue(e, "group"))).ToArray())
                        {
                            el.SetAttributeValue("group", "1");
                        }
                    }

                    List<string> aggColNames = query.Element("select").Elements().Select(e3 => e3.Attribute("as").Value).ToList();
                    query.Elements("select").Elements().Where(e1 => getAttrValue(e1, "group") != "1").Where(e => !aggFuncsNames.Contains(getAttrValue(e, "group"))).Where(e2 =>

                        !aggColNames.Contains(getAttrValue(e2, "group"))

                        ).Remove();

                }


            }

            var qqq = query.Descendants(TextConst.EName.Query).FirstOrDefault(e => getAttrValue(e, TextConst.AName.As) == grQueryAlias);

            if (qqq == null)
            {
                qqq = query;
            }

            qqq.Elements("select").Elements().Where(e => getAttrValue(e, "function") == "row_num").Attributes("group").Remove();
            qqq.Elements("select").Elements().Attributes(TextConst.AName.Group).Where(a => a.Value == TextConst.AVGroup.Outer).Remove();
            return query;
        }
        private static string[] gsetsFuncNames = new string[] { "grouping", "grouping_id" };
        private static string[] gsetsSpecColsNames = new string[] { "parent_growid", "parent_grsetid", "grsetid", "origgrsetid", TextConst.AVSpecColumnGrset.GrSetTitle, "growid", TextConst.AVSpecColumnGrset.GrRowNum, TextConst.AVSpecColumnGrset.OnRowsGrSetId, TextConst.AVSpecColumnGrset.OnRowsGrRowId, TextConst.AVSpecColumnGrset.OnColsColId, TextConst.AVSpecColumnGrset.OnColsGrSetId, TextConst.AVSpecColumnGrset.GroupingId, TextConst.AVSpecColumnGrset.ParentGroupingId };
        private static string[] gsetsSpecColsNamesOnlyOnRows = new string[] { "parent_growid", "parent_grsetid", "grsetid", "origgrsetid", TextConst.AVSpecColumnGrset.GrSetTitle, "growid", TextConst.AVSpecColumnGrset.GrRowNum, TextConst.AVSpecColumnGrset.GroupingId, TextConst.AVSpecColumnGrset.ParentGroupingId };
        private static void applyGroupingSets(XElement element, XElement query, string matName)
        {
            XElement xtraHaving = new XElement(EName.where, new XElement(EName.call, new XAttribute(AName.function, TextConst.AVFunction.And)));
            string qalias = query.Element(EName.from).Element(EName.query).Attribute(AName.@as).Value;
            XElement xgrsets = element.Element(TextConst.EName.Grsets);
            bool isOnColumns = false;
            if (xgrsets.Elements(TextConst.EName.OnRows).Any()) {
                var xgrsets1 = new XElement(TextConst.EName.Grsets);
                Cmn.copyAttributes(xgrsets, xgrsets1);
                xgrsets1.Add(xgrsets.Elements(TextConst.EName.OnRows).Elements());
                if (xgrsets.Elements(TextConst.EName.OnColumns).Any()) {
                    isOnColumns = true;
                    var oncolsElement = new XElement(xgrsets.Element(TextConst.EName.OnColumns));
                    foreach (XElement xgrsetOnRow in xgrsets1.Descendants(TextConst.EName.Grset).ToList()) {
                        var name1 = xgrsetOnRow.Attribute(AName.@as).Value;
                        var oncolsElement1 = new XElement(oncolsElement);
                        foreach (XElement xgrsetOnCols in oncolsElement1.Descendants(TextConst.EName.Grset).ToList()) {
                            var name2 = xgrsetOnCols.Attribute(AName.@as).Value;
                            var fullName = name1 + "_" + name2;
                            xgrsetOnCols.SetAttributeValue(AName.@as, fullName);
                            xgrsetOnCols.SetAttributeValue(TextConst.AName.OnColumns, TextConst.AVBool.True);
                            xgrsetOnCols.SetAttributeValue(TextConst.AName.OnColsGrsetId, name2);
                            xgrsetOnCols.SetAttributeValue(TextConst.AName.OrigGrsetId, name2);
                        }
                        xgrsetOnRow.AddFirst(oncolsElement1.Elements());
                    }
                }
                xgrsets = xgrsets1;
            }
            //if (getAttrValue(element, TextConst.AName.Name) == "26630-dat-vvod-new-2")
            //{
            //}
            bool ttblMat = getAttrValue(xgrsets, TextConst.AName.MaterializeType) == TextConst.AVMaterializeType.TempTable;
            XAttribute matAttr;
            if (ttblMat) {
                matAttr = new XAttribute(AName.materialize, "1");
            } else if (matName != "max_tr_prop_datavv") { // 73962: добавлено условие, чтобы убрать HINT (сильно тормозит)
                matAttr = new XAttribute(AName.hint, TextConst.AVHint.Materialize);
            } else {
                matAttr = null;
            }
            XElement qry1 = new XElement(EName.query, new XAttribute(AName.@as, qalias)
               , matAttr
                , new XElement(EName.select,
                    new XElement(EName.column, new XAttribute(AName.table, qalias), new XAttribute(AName.column, "*"))
                   ),
               new XElement(EName.from,
                   query.Element(EName.from).Element(EName.query)
                   )
               );

            query.Element(EName.from).Element(EName.query).ReplaceWith(qry1);
            if (ttblMat) {
                if (element.Attribute(AName.name) != null) {
                    addMatrializeId(query, element.Attribute(AName.name).Value);
                } else {
                    addMatrializeId(query, matName);
                }
            }
            //List<string> grLevs = element.Element("grsets").Descendants().SelectMany(e => e.Attribute("level").Value.Split(',')).ToList();
            //IEnumerable<XElement> elements = query.Elements("select").Elements().Where(e => Cmn.IsNumeric(getAttrValue(e, "group"))).ToArray();
            //elements.Where(e => !grLevs.Contains(e.Attribute("group").Value)).Remove();
            SortedList<string, XElement> grCols = new SortedList<string, XElement>();

            IEnumerable<XElement> elements = query.Elements("select").Elements().Where(e => Cmn.IsNumeric(getAttrValue(e, "group"))).ToArray();

            foreach (XElement col in elements)
            {
                grCols.Add(col.Attribute("group").Value, col);
            }

            elements.Attributes("group").Remove();
            int groipId = 1;
            XElement rowNumExpr = Factory.NewCall(TextConst.AVFunction.RowNumber);
            rowNumExpr.Add(new XAttribute(AName.@as, TextConst.AVSpecColumnGrset.GrRowNum));
            rowNumExpr.Add(Factory.NewCall(TextConst.AVFunction.OrderBySimple, Factory.NewConst("null")));
            XElement idExpr = new XElement("call", new XAttribute("function", TextConst.AVFunction.Concat), new XAttribute("as", "growid"), new XAttribute("type", "string"), new XAttribute("key", "1"), new XAttribute("title", ""));

            XElement onColsColIddExpr = null;
            if (isOnColumns)
            {
                onColsColIddExpr = new XElement("call", new XAttribute("function", "decode"), new XAttribute("as", TextConst.AVSpecColumnGrset.OnColsColId), new XAttribute("type", "string"), new XAttribute("title", ""),
                     new XElement("column", new XAttribute("table", "this"), new XAttribute("column", "grsetid"))
                   );
            }
            XElement havingExpr = null;
            //  XElement parentIdExpr = new XElement("call", new XAttribute("function", "case"), new XAttribute("as", "grsetid"), new XAttribute("type", "number"));


            List<XElement> grSets = xgrsets.Descendants("grset").ToList();

            SortedList<string, List<string>> grSetsColumns = new SortedList<string, List<string>>();
            SortedList<string, XElement> visibleExps = new SortedList<string, XElement>();

            foreach (XElement grSet in grSets)
            {
                //   XElement visibleExpr = null;

                string sGroupId = null;

                if (grSet.Attribute("as") == null)
                {
                    sGroupId = groipId.ToString();
                }
                else
                {
                    sGroupId = grSet.Attribute("as").Value;
                }
                grSet.SetAttributeValue("id", sGroupId);
                grSet.SetAttributeValue("nid", groipId.ToString());

                if (grSet.Attribute(TextConst.AName.OnColsGrsetId) == null)
                {
                    grSet.SetAttributeValue(TextConst.AName.OrigGrsetId, sGroupId);
                }
                groipId++;
            }

            var groupElementsById = new SortedList<string, XElement>();

            foreach (XElement grSet in grSets)
            {
               //




                string sGroupId = null;

                sGroupId = grSet.Attribute("id").Value;
                groipId = Convert.ToInt32(grSet.Attribute("nid").Value);
                string sParGroupId = "";
                //if (grSet.Attribute(TextConst.AName.Parent) != null)
                //{
                //    sParGroupId = grSet.Attribute(TextConst.AName.Parent).Value;
                //}
                //else
                    if (grSet.Parent.Name.LocalName != "grsets")
                {
                    sParGroupId = grSet.Parent.Attribute("id").Value;
                }
                var sOnRowsGrsetId = "";
                if (grSet.Attribute(TextConst.AName.OnColumns) != null)
                {
                    sOnRowsGrsetId = grSet.Ancestors().First(g => g.Attribute(TextConst.AName.OnColumns) == null).Attribute(TextConst.AName.As).Value;
                }
                else
                {
                    sOnRowsGrsetId = sGroupId;
                }


                var sOnColsGrsetId = "";
                if (grSet.Attribute(TextConst.AName.OnColumns) != null)
                {
                    sOnColsGrsetId = grSet.Attribute(TextConst.AName.OnColsGrsetId).Value;
                }
                else
                {
                    sOnColsGrsetId = "";
                }

                string grColName = "gr" + sGroupId;
                string grParColName = "pgr" + sGroupId;
                

                string sGroupOrigId = sGroupId;
                if (grSet.Attribute(TextConst.AName.Name) != null)
                {
                    sGroupOrigId = grSet.Attribute(TextConst.AName.Name).Value;
                }

                // вроде не нужно - куча лишних колонок
                //qry1.Element("select").Add(new XElement("const", new XAttribute(TextConst.AName.Type, TextConst.AVType.String), new XAttribute("as", grColName), new XText("'" + sGroupId + "'")));
                //qry1.Element("select").Add(new XElement("const", new XAttribute(TextConst.AName.Type, TextConst.AVType.String), new XAttribute("as", grParColName), new XText("'" + sParGroupId + "'")));

                grSetsColumns.Add(sGroupId, new List<string>());
                XElement grSet1 = grSet;
                XElement gr = new XElement("group", new XAttribute("id", sGroupId)
                    , new XAttribute("name", sGroupOrigId)
                    , new XAttribute("parid", sParGroupId)
                    , new XAttribute(TextConst.AName.OnRowsGrsetId, sOnRowsGrsetId)
                     , new XAttribute(TextConst.AName.OnColsGrsetId, sOnColsGrsetId),
                    copyAttribute(grSet, "title")
                    //   , new XElement("column", new XAttribute("table", qalias), new XAttribute("column", grColName))
                );

                copyAttribute(grSet, gr, TextConst.AName.TreeLevel);
                groupElementsById.Add(sGroupId, gr);
                //if (!String.IsNullOrEmpty(grParColName))
                //{
                //    gr.Add(new XElement("column", new XAttribute("table", qalias), new XAttribute("column", grParColName)));
                //}
                List<string> cols1 = new List<string>();
                List<string> colsOnCols = new List<string>();
                while (grSet1.Name.LocalName != "grsets")
                {
                    var parI = new List<string>();

                    if (grSet1.Attribute(TextConst.AName.ParentLevel) != null)
                    {
                        parI.AddRange(grSet1.Attribute(TextConst.AName.ParentLevel).Value.Split(',').ToArray());
                    }

                    foreach (string gri in grSet1.Attribute("level").Value.Split(','))
                    {
                        if (gri != "")
                        {
                            // если тут ошибка - проверь что ключевые колонки всех измерений, по которым есть группировки, есть в селекте
                            XElement col = new XElement(grCols[gri]);

                            if (parI.Contains(gri))
                            {
                                col.SetAttributeValue(TextConst.AName.Parent, TextConst.AVBool.True);
                            }
                            copyAttribute(grSet1, col, TextConst.AName.OnColumns);
                            if (grSet1.Attribute(TextConst.AName.OnColumns) != null)
                            {
                                colsOnCols.Add(col.Attribute("as").Value);
                            }
                            gr.Add(col);
                            cols1.Add(col.Attribute("as").Value);
                        }
                    }

                    grSet1 = grSet1.Parent;
                }
                //XElement grIdExprW = new XElement("call", new XAttribute("function", "when"), new XElement("call", new XAttribute("function", "and")));

                foreach (XElement col in elements)
                {

                    //  XElement grIdExprE = new XElement("call", new XAttribute("function", "="), new XElement("call", new XAttribute("function", "grouping"), new XElement(col)));
                    //  string v = "1";
                    if (cols1.Contains(col.Attribute("as").Value))
                    {
                        col.Attributes(TextConst.AName.OnColumns).Remove();
                        if (colsOnCols.Contains(col.Attribute("as").Value))
                        {
                            col.SetAttributeValue(TextConst.AName.OnColumns, TextConst.AVBool.True);
                        }
                        grSetsColumns[sGroupId].Add(col.Attribute("as").Value);
                        //     v = "0";
                    }
                    // grIdExprE.Add(new XElement("const", new XText(v)));
                    //  grIdExprW.Element("call").Add(grIdExprE);

                }
                if (grSet.Element(TextConst.EName.Having) != null)
                {
                    if (grSet.Element("where") == null)
                    {
                        grSet.Add(new XElement(TextConst.EName.Where, new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.True))));
                    }
                }



                

                
                string visPref = "vis";

                if (grSet.Element("where") != null)
                {
                    
                    string visId = grSet.Attribute(TextConst.AName.OrigGrsetId).Value;
                   
                    if (!visibleExps.ContainsKey(visId))
                    {
                        string visColName = visPref + visId;

                        if (havingExpr == null)
                        {
                            havingExpr = new XElement("having", new XElement("call", new XAttribute("function", "and")));
                        }


                        var visibleExpr = new XElement("call", new XAttribute("function", "if"), new XAttribute("as", visColName), new XAttribute(TextConst.AName.Type, TextConst.AVDataType.Number),

                           new XElement("call", new XAttribute("function", "and"), new XElement("call", new XAttribute("function", "is not null")
                               , new XElement("const", new XText(groipId.ToString()))), grSet.Element("where").Elements())
                            // чтобы выражение для разных уровней было разным иначе оракл понимает когда оно одинаковое , метит grouping=1 лишние колонки, не получается правильно отнести строку к группе
                            // хрень, переделать, по другому определять id группы, не множить условия
                            ,

                            new XElement("const", new XText("1")), new XElement("const", new XText("0")));
                        //  query.Element("select").Add(visibleExpr);

                        visibleExps.Add(visId, visibleExpr);
                        qry1.Element("select").Add(visibleExpr);
                        var visExp1 = new XElement("call", new XAttribute("function", "="),
                                         new XElement("call", new XAttribute("function", TextConst.AVFunction.Coalesce),
                                        new XElement("column", new XAttribute("table", qalias), new XAttribute("column", visColName)),
                                        new XElement("const", new XText("1"))
                                        ),
                                        new XElement("const", new XText("1"))
                                     );
                        havingExpr.Element("call").Add(visExp1);

                        query.Element(TextConst.EName.Select).Add(new XElement("column"
                            , new XAttribute("table", qalias)
                            , new XAttribute("column", visColName)
                                , new XAttribute(TextConst.AName.As, visColName)
                            ));
                    }
                }

                XElement el = grSet;
                
                while (el.Name.LocalName != "grsets")
                {
                    //visId
                    if (visibleExps.ContainsKey(el.Attribute(TextConst.AName.OrigGrsetId).Value))
                    {
                        var xcol1 = new XElement("column", new XAttribute("table", qalias), new XAttribute("column", visPref + el.Attribute(TextConst.AName.OrigGrsetId).Value));
                        if (el.Attribute(TextConst.AName.TreeLevel) != null)
                        {
                            xcol1.SetAttributeValue(TextConst.AName.DontUseForGroupingKey, TextConst.AVBool.True);
                            // Для деревьев применение условий не возможно
                            // Может быть искажение данных
                            //, т.к. условия не включаются в ключ
                            // только условия сформированные автоматически
                            // при необходимости можно доделать
                               
                        }
                        gr.Add(xcol1);
                    }
                    el = el.Parent;
                }

                query.AddFirst(gr);


                string origId = "";
                if (isOnColumns)
                {
                }

                origId = grSet.Attribute("id").Value;
                if (grSet.Attribute(TextConst.AName.Name) != null)
                {
                    origId = grSet.Attribute(TextConst.AName.Name).Value;
                }
                XElement idExprC = null;
                if (isOnColumns && sOnColsGrsetId != "")
                {
                    onColsColIddExpr.Add(new XElement("const", new XText("'" + grSet.Attribute("id").Value + "'")));
                    idExprC = new XElement("call", new XAttribute("function", "||")
                        , new XElement("const", new XText("''"))
                    );
                    //keys = null;

                    bool br = false;
                    foreach (XElement col in elements.Where(c => c.Attribute(TextConst.AName.OnColumns) != null))
                    {
                        var gsId = grSet.Attribute("id").Value;
                        var colAlias = col.Attribute("as").Value;
                     
                        if (grSetsColumns[gsId].Contains(colAlias))
                        {
                            bool add = false;
                            if (Cmn.GetAttrValue(grSet, TextConst.AName.Intervals) == TextConst.AVBool.True)
                            {
                                
                                    add = true;
                                    br = true;

                            }
                            else
                            {
                                add = true;

                            }
                            if (add)
                            {
                               
                                idExprC.Add(new XElement("const", new XText("'#'")));
                                idExprC.Add(new XElement("call", new XAttribute("function", "to_char"), new XElement(col)));
                                if (br)
                                {
                                    break;
                                }
                            }
                        }

                        //else
                        //{
                        //    idExprC.Add(new XElement("const", new XText("' '")));
                        //}
                    }
                    //if (!added)
                    //{
                    //}
                    onColsColIddExpr.Add(idExprC);

                }

            }
            var hOld = query.Elements(TextConst.EName.Having).FirstOrDefault();
            if (hOld != null && havingExpr!=null)
            {
                var ohEls = hOld.Elements().ToArray();
                ohEls.Remove();

                var hCall = new XElement(TextConst.EName.Call,
                    new XAttribute(TextConst.AName.Function, TextConst.AVFunction.And));
                hCall.Add(ohEls);
                hOld.Add(hCall);
                hCall.Add(havingExpr.Elements());
            }
            else
            {
                query.Add(havingExpr);
            }
            if (element.Element("grsets").Elements().Count() == 1)
            {
                //  grCols.Remove(element.Element("grsets").Elements().First().Attribute("level").Value);

            }
            query.Element("select").Add(rowNumExpr);
            query.SetAttributeValue("haskeys", "1");
            foreach (XElement col in grCols.Values)
            {

                // значения с group = sum показываем всегда
                IEnumerable<XElement> dependantColumns = query.Elements("select").Elements().Where(e => getAttrValue(e, "master") == col.Attribute("as").Value && getAttrValue(e, "group") == "max").ToArray();

                XElement ifExpr = new XElement("call", new XAttribute("function", "if"),
                   new XElement("call", new XAttribute("function", "="),
                        new XElement("call", new XAttribute("function", "grouping"),
                           new XElement(col))
                           ,
                       new XElement("const", new XText("0"))
                       )
                   );
                foreach (XElement col1 in dependantColumns)
                {
                    XElement newCol = new XElement(col1);
                    // newCol.Attributes("group").Remove();

                    if (newCol.Attribute(TextConst.AName.Master) != null)
                    {
                        newCol.SetAttributeValue(TextConst.AName.CMaster, newCol.Attribute(TextConst.AName.Master).Value);
                        newCol.Attributes(TextConst.AName.Master).Remove();
                    }


                    newCol.Attributes("as").Remove();
                    XElement ifExpr1 = new XElement(ifExpr);
                    copyAttributes(col1, ifExpr1, new string[] { "as", "title", "type", "class-title", "agg", "format", TextConst.AName.CMaster, TextConst.AName.CMasterKey, TextConst.AName.HAlign, TextConst.AName.MergeKey, TextConst.AName.Colset, TextConst.AName.Color, TextConst.AName.TreeOriginalColumn, TextConst.AName.TreeLevelColumn, TextConst.AName.Level });


                    ifExpr1.Add(newCol);
                    col1.ReplaceWith(ifExpr1);
                }

            }


            string sorder = "";
            
           // IEnumerable<XElement> grss = element.Elements("grsets").Elements("grset").ToArray();
            IEnumerable<XElement> grss = xgrsets.Elements("grset").ToArray();
           
            string q = "";
            while (grss.Count() > 0)
            {
                foreach (XElement grs in grss)
                {
                    if (grs.Attribute("order") != null)
                    {
                        sorder += q + grs.Attribute("order").Value;
                        q = ",";
                    }
                    List<string> lvs = grs.Attribute("level").Value.Split(',').ToList();
                    foreach (string lv in lvs)
                    {
                        if (lv != "")
                        {
                            sorder += q + grCols[lv].Attribute("as").Value + " nulls first";
                            q = ",";
                        }
                    }

                }
                grss = grss.Elements("grset");
            }

            foreach (var xcol in query.Elements(TextConst.EName.Select).Elements().Where(e => e.Attribute(TextConst.AName.Master) != null).ToList())
            {
                xcol.SetAttributeValue(TextConst.AName.CMaster, xcol.Attribute(TextConst.AName.Master).Value);
                xcol.Attribute(TextConst.AName.Master).Remove();
            }
            //query.Elements("select").Elements().Attributes("master").Remove();

            query.SetAttributeValue("order", sorder);
            var allGroupColumns = query.Elements("group").Elements("column").ToList();
            List<string> allGroupColumnsNames = new List<string>();
            var allGroupColumnsNamesOrig = new SortedList<string, string>();
            foreach (var col1 in allGroupColumns)
            {
                if (col1.Attribute(TextConst.AName.DontUseForGroupingKey) != null) continue;
                var colname = col1.Attribute(TextConst.AName.Column).Value;

                
                if (!allGroupColumnsNames.Contains(colname))
                {
                    allGroupColumnsNames.Add(colname);
                    if (col1.Attribute(TextConst.AName.TreeOriginalColumn) != null)
                    {

                        var origname = col1.Attribute(TextConst.AName.TreeOriginalColumn).Value.Replace("!", "");
                        //if (origname != colname)
                        //{
                        allGroupColumnsNamesOrig.Add(colname, origname);
                        //}
                    }
                   
                }
            }
           // List<string> allGroupColumnsNames = query.Elements("group").Elements("column").Attributes("column").Select(c => c.Value).Distinct().ToList();
            int i = 0;
            var allGroupColumnsRev = allGroupColumnsNames.ToList();
            allGroupColumnsRev.Reverse();
            XElement parIdExpr2 = new XElement("call"
                , new XAttribute("function", TextConst.AVFunction.Concat)
                //, new XAttribute("as", TextConst.AVSpecColumnGrset.ParentGrRowId)
                //, new XAttribute("type", "string")
               // , new XAttribute("title", "")
                );

            XElement parIdExpr = new XElement("call"
                , new XAttribute("function", TextConst.AVFunction.If)
                , new XAttribute("as", TextConst.AVSpecColumnGrset.ParentGrRowId)
                , new XAttribute("type", "string")
                , new XAttribute("title", "")
                , new XElement(TextConst.EName.Call
                    , new XAttribute(TextConst.AName.Function, TextConst.AVFunction.IsNull)
                    , new XElement(TextConst.EName.Column
                        , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                        , new XAttribute(TextConst.AName.Column, TextConst.AVSpecColumnGrset.ParentGroupingId)
                        )
                    )
                 , new XElement(TextConst.EName.Const, new XText("null"))
                 , parIdExpr2
                );

           // var treeExprsCond = new SortedList<string, XElement>();
            var treeExprsVal = new SortedList<string, XElement>();
            var treeExprsValPar = new SortedList<string, XElement>();
            foreach (var colname in allGroupColumnsRev)
            {
                bool isTreeCol = false;
            
                XElement xcontCall = null;
                XElement xcontCallPar = null;
                if (allGroupColumnsNamesOrig.ContainsKey(colname))
                {
                    isTreeCol = true;
                    if (!treeExprsVal.ContainsKey(allGroupColumnsNamesOrig[colname]))
                    {
                        idExpr.Add(new XElement("const", new XText("'#'")));
                        parIdExpr2.Add(new XElement("const", new XText("'#'")));
                        var xcc = new XElement("call", new XAttribute("function", TextConst.AVFunction.Coalesce));
                        idExpr.Add(xcc);
                        treeExprsVal[allGroupColumnsNamesOrig[colname]] = xcc;
                        xcc = new XElement(xcc);
                        parIdExpr2.Add(xcc);
                        treeExprsValPar[allGroupColumnsNamesOrig[colname]] = xcc;
                    }
                    xcontCall = treeExprsVal[allGroupColumnsNamesOrig[colname]];
                    xcontCallPar = treeExprsValPar[allGroupColumnsNamesOrig[colname]];
                  
                }
                else
                {

                    xcontCall = idExpr;
                    xcontCallPar = parIdExpr2;
                    idExpr.Add(new XElement("const", new XText("'#'")));
                    parIdExpr2.Add(new XElement("const", new XText("'#'")));
                }
                var col = new XElement(TextConst.AName.Column
                        , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                        , new XAttribute(TextConst.AName.Column, colname)
                        );
                var charcol = new XElement("call", new XAttribute("function", "to_char"), new XElement(col));
               
                   
                    
                    var xcolval = new XElement("call", new XAttribute("function", TextConst.AVFunction.Coalesce)
                            , charcol
                            , new XElement(TextConst.EName.Const, "' '")
                            );
                    var v = i;// allGroupColumns.Count() - i - 1;
                    var xval = new XElement(TextConst.EName.Const, new XText(Math.Pow(2, i).ToString()));

                    var xif = new XElement(TextConst.EName.Call);
                    xif.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.If);
                    var xcond = new XElement(TextConst.EName.Call);
                    xcond.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Equal);
                    xif.Add(xcond);
                    xcond.Add(new XElement(TextConst.EName.Const, new XText("0")));
                    var xcall = new XElement(TextConst.EName.Call);
                    xcond.Add(xcall);
                    xcall.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Bitand);
                    xcall.Add(xval);
                    var xggcol = new XElement(TextConst.AName.Column
                       , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                       , new XAttribute(TextConst.AName.Column, TextConst.AVSpecColumnGrset.GroupingId)
                       );
                    xcall.Add(
                         xggcol
                        );
                    xif.Add(xcolval);

                    var xif1 = new XElement(xif);
                   
                    //idExpr.Add(xif1);
                    if (isTreeCol)
                    {
                        xcontCall.AddFirst(xif1);
                    }
                    else
                    {
                        xcontCall.Add(xif1);
                    }
                    xggcol.SetAttributeValue(TextConst.AName.Column, TextConst.AVSpecColumnGrset.ParentGroupingId);
                    xif1 = new XElement(xif);

                    if (isTreeCol)
                    {
                        xcontCallPar.AddFirst(xif1);
                    }
                    else
                    {
                        xcontCallPar.Add(xif1);
                    }
                    //parIdExpr2.Add(xif1);
           
               i++;
            }
            // чтобы не могло быть пробела в конце
            idExpr.Add(new XElement("const", new XText("'#'")));
            parIdExpr2.Add(new XElement("const", new XText("'#'")));


            XElement grouping_id_expr = new XElement("call", new XAttribute("function", "grouping_id"), new XAttribute("as", TextConst.AVSpecColumnGrset.GroupingId));
            foreach (string name in allGroupColumnsNames)
            {
                grouping_id_expr.Add(new XElement("column", new XAttribute("table", qalias), new XAttribute("column", name)));

            }
            XElement grouping_id = new XElement("column", new XAttribute("table", TextConst.AVTable.Ths), new XAttribute("column", TextConst.AVSpecColumnGrset.GroupingId));

            XElement grIdExpr = new XElement("call", new XAttribute("function", "decode"), new XAttribute("as", "grsetid"), new XAttribute("type", "string"), new XAttribute("title", ""), grouping_id);

            XElement treeLevelExpr = null;

            bool hasTree = xgrsets.Descendants().Attributes(TextConst.AName.TreeLevel).Any();

            if (hasTree)
            {
                treeLevelExpr = new XElement("call", new XAttribute("function", "decode"), new XAttribute("as", TextConst.AVSpecColumnGrset.GrTreeLevel), new XAttribute("type", "number"), new XAttribute("title", ""), grouping_id);
            }


            XElement parentGroupingIdExpr = new XElement("call", new XAttribute("function", "decode"), new XAttribute("as", TextConst.AVSpecColumnGrset.ParentGroupingId), new XAttribute("type", "number"), new XAttribute("title", ""), grouping_id);



            XElement grOrigIdExpr = new XElement("call", new XAttribute("function", "decode"), new XAttribute("as", "origgrsetid"), new XAttribute("type", "string"), new XAttribute("title", ""), grouping_id);

            XElement onRowsGrsetIdExpr = null;
            if (isOnColumns)
            {
                onRowsGrsetIdExpr = new XElement("call", new XAttribute("function", "decode"), new XAttribute("as", TextConst.AVSpecColumnGrset.OnRowsGrSetId), new XAttribute("type", "string"), new XAttribute("title", ""), grouping_id);
            }
            XElement onColsGrsetIdExpr = null;
            if (isOnColumns)
            {
                onColsGrsetIdExpr = new XElement("call", new XAttribute("function", "decode"), new XAttribute("as", TextConst.AVSpecColumnGrset.OnColsGrSetId), new XAttribute("type", "string"), new XAttribute("title", ""), grouping_id);

            }

            XElement grNameExpr = new XElement("call", new XAttribute("function", "decode"), new XAttribute("as", TextConst.AVSpecColumnGrset.GrSetTitle), new XAttribute("type", "string"), grouping_id);
            if (xgrsets.Descendants(TextConst.EName.Grset).Attributes(TextConst.AName.Title).Any())
            {
                grNameExpr.SetAttributeValue(TextConst.AName.Title, "Уровень группировки");
            }
            XElement grParIdExpr = new XElement("call", new XAttribute("function", "decode"), new XAttribute("as", "parent_grsetid"), new XAttribute("type", "string"), new XAttribute("title", ""), grouping_id);

            foreach (XElement gr in groupElementsById.Values)
            {
                List<string> groupColumns = gr.Elements("column").Attributes("column").Select(c => c.Value).Distinct().ToList();
                string grid = getGroupingId(allGroupColumnsNames, groupColumns).ToString();
                gr.SetAttributeValue(TextConst.AName.Groupingid, grid);


                groupColumns = gr.Elements("column").Where(c1 => getAttrValue(c1, TextConst.AName.Parent) == TextConst.AVBool.True).Attributes("column").Select(c => c.Value).Distinct().ToList();
                if (groupColumns.Count != 0)
                {
                    grid = getGroupingId(allGroupColumnsNames, groupColumns).ToString();
                    gr.SetAttributeValue(TextConst.AName.ParentGroupingid, grid);
                }

            }

            foreach (XElement gr in groupElementsById.Values)
            {
                var parid = gr.Attribute("parid").Value;
                if (parid != "")
                {
                    var pgr = groupElementsById[parid];
                    gr.SetAttributeValue(TextConst.AName.ParentGroupingid, pgr.Attribute(TextConst.AName.Groupingid).Value);
                }
            }

            foreach (XElement grSet in grSets)
            {
                //
                if (grSet.Element(TextConst.EName.Having) != null)
                {

                    //!!! НЕ роверено нет примеров
                    var grSetAr = new XElement[] { grSet };

                    //if (sOnColsGrsetId != "")
                    //{
                    //    grSetAr = xgrsets.Descendants(TextConst.EName.Grset).Where(e => Cmn.GetAttrValue(e, TextConst.AName.OnColsGrsetId) == sOnColsGrsetId).ToArray();
                    //}


                    //var groupingId = groupElementsById[grSet.Attribute(TextConst.AName.Id).Value].Attribute(TextConst.AName.Groupingid).Value;
               

                    List<XElement> gids = grSetAr.DescendantsAndSelf("grset").Select(e => new XElement("const"
                        , new XText(
                           // "'" +

                            groupElementsById[e.Attribute(TextConst.AName.Id).Value].Attribute(TextConst.AName.Groupingid).Value
                            
                           // + "'"
                            )
                        
                        
                        )).ToList();

                    havingExpr
                    //xtraHaving
                        
                        .Element("call").Add(

                          new XElement("call", new XAttribute("function", "or"),
                               grSet.Element(TextConst.EName.Having).Elements(),
                              new XElement("call", new XAttribute("function", "not in"),
                        //visibleExpr,
                                 new XElement("column", new XAttribute("table", "this"), new XAttribute("column", TextConst.AVSpecColumnGrset.GroupingId)),
                                  new XElement("call", new XAttribute("function", "array"), gids)
                              )
                           )

                     );
                    //visExp2 = new XElement("call", new XAttribute("function", "and"), grSet.Element(TextConst.EName.Having).Elements());
                }
            }

            foreach (XElement gr in query.Elements("group"))
            {
                List<string> groupColumns = gr.Elements("column").Attributes("column").Select(c => c.Value).Distinct().ToList();
                string grid = getGroupingId(allGroupColumnsNames, groupColumns).ToString();

                if (gr.Attribute(TextConst.AName.ParentGroupingid) != null)
                {
                    parentGroupingIdExpr.Add(new XElement("const", new XText("'" + grid.ToString() + "'")));
                    parentGroupingIdExpr.Add(new XElement("const", new XText("'" + gr.Attribute(TextConst.AName.ParentGroupingid).Value + "'")));
                }

                grIdExpr.Add(new XElement("const", new XText("'" + grid.ToString() + "'")));
                grIdExpr.Add(new XElement("const", new XText("'" + gr.Attribute("id").Value + "'")));



                grOrigIdExpr.Add(new XElement("const", new XText("'" + grid.ToString() + "'")));
                grOrigIdExpr.Add(new XElement("const", new XText("'" + gr.Attribute(TextConst.AName.Name).Value + "'")));

                if (gr.Attribute(TextConst.AName.TreeLevel) != null)
                {
                    treeLevelExpr.Add(new XElement("const", new XText("'" + grid.ToString() + "'")));
                    treeLevelExpr.Add(new XElement("const", new XText(gr.Attribute(TextConst.AName.TreeLevel).Value)));
                }

                grNameExpr.Add(new XElement("const", new XText("'" + grid.ToString() + "'")));
                grNameExpr.Add(new XElement("const", new XText("'" + getAttrValue(gr, "title") + "'")));
                if (isOnColumns)
                {
                    onRowsGrsetIdExpr.Add(new XElement("const", new XText("'" + grid.ToString() + "'")));
                    onRowsGrsetIdExpr.Add(new XElement("const", new XText("'" + gr.Attribute(TextConst.AName.OnRowsGrsetId).Value + "'")));

                    onColsGrsetIdExpr.Add(new XElement("const", new XText("'" + grid.ToString() + "'")));
                    onColsGrsetIdExpr.Add(new XElement("const", new XText("'" + gr.Attribute(TextConst.AName.OnColsGrsetId).Value + "'")));
                }
                if (gr.Attribute("parid").Value != "")
                {
                    grParIdExpr.Add(new XElement("const", new XText("'" + grid.ToString() + "'")));
                    grParIdExpr.Add(new XElement("const", new XText("'" + gr.Attribute("parid").Value + "'")));
                }

                // gr.SetAttributeValue("oid", grid);
            }
            query.Element("select").Add(idExpr);
            query.Element("select").Add(parIdExpr);
            query.Element("select").Add(grouping_id_expr);
            query.Element("select").Add(grIdExpr);
            if (hasTree)
            {
                query.Element("select").Add(treeLevelExpr);
            }
          
            query.Element("select").Add(grOrigIdExpr);
            query.Element("select").AddFirst(grNameExpr);
            if (isOnColumns)
            {
                query.Element("select").AddFirst(onRowsGrsetIdExpr);
                query.Element("select").AddFirst(onColsGrsetIdExpr);
            }
            if (grParIdExpr.Elements().Count() > 1)
            {
                query.Element("select").Add(grParIdExpr);
                query.Element("select").Add(parentGroupingIdExpr);
            }
            else
            {
                //  as="parent_grsetid" type="string"

                query.Element("select").Add(new XElement("const", new XAttribute("as", "parent_grsetid"), new XAttribute("type", "string"), new XText("null")));
                query.Element("select").Add(new XElement("const", new XAttribute("as", TextConst.AVSpecColumnGrset.ParentGroupingId), new XAttribute("type", "number"), new XText("null")));
            }





            if (isOnColumns)
            {

                var xggcol = new XElement(TextConst.AName.Column
                   , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                   , new XAttribute(TextConst.AName.Column, TextConst.AVSpecColumnGrset.GroupingId)
                   );
                var scol = new XElement(TextConst.AName.Column
                   , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                   , new XAttribute(TextConst.AName.Column, TextConst.AVSpecColumnGrset.GrRowId)
                   );
                var pcol = new XElement(TextConst.AName.Column
                 , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                 , new XAttribute(TextConst.AName.Column, TextConst.AVSpecColumnGrset.ParentGrRowId)
                 );
                var onRowsIdExpr = new XElement(TextConst.EName.Call);
                onRowsIdExpr.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.If);
                var xcond = new XElement(TextConst.EName.Call);
                xcond.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.In);
                onRowsIdExpr.Add(xcond);
                xcond.Add(xggcol);



                var gids =
                   groupElementsById.Values.Where(e => getAttrValue(e, TextConst.AName.OnColsGrsetId) != "")
                    .Select(e1 => new XElement(TextConst.EName.Const, new XText(e1.Attribute(TextConst.AName.Groupingid).Value))).ToArray();

                var xcarr = new XElement(TextConst.EName.Call);
                xcarr.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Array);
                xcarr.Add(gids);
                xcond.Add(xcarr);



                onRowsIdExpr.Add(pcol);
                onRowsIdExpr.Add(scol);

                onRowsIdExpr.SetAttributeValue(TextConst.AName.As, TextConst.AVSpecColumnGrset.OnRowsGrRowId);
                onRowsIdExpr.SetAttributeValue(TextConst.AName.Type, TextConst.AVDataType.String);
                query.Element("select").Add(onRowsIdExpr);


                query.Element("select").Add(onColsColIddExpr);
            }

            //if (query.DescendantsAndSelf ("query").Where(e=>Cmn.GetAttrValue(e,"name").Contains( "38486")).Any())
            //{
              
            //}
            var fieldsToMoveUp = gsetsSpecColsNames.ToList();
            
            fieldsToMoveUp.Remove(TextConst.AVSpecColumnGrset.GroupingId);
            AddQueryLevel(query, grQueryAlias, fieldsToMoveUp.ToArray());

            fieldsToMoveUp.Remove(TextConst.AVSpecColumnGrset.ParentGroupingId);
            AddQueryLevel(query, "p1", fieldsToMoveUp.ToArray());
            if (isOnColumns)
            {
                fieldsToMoveUp.Remove(TextConst.AVSpecColumnGrset.ParentGrRowId);
                fieldsToMoveUp.Remove(TextConst.AVSpecColumnGrset.GrRowId);
                fieldsToMoveUp.Remove(TextConst.AVSpecColumnGrset.GrSetName);
                fieldsToMoveUp.Remove(TextConst.AVSpecColumnGrset.GrTreeLevel); // ??
                AddQueryLevel(query, "p2", fieldsToMoveUp.ToArray());
            }
           
            
            if (hasTree)
            {
                fieldsToMoveUp.Clear();
                AddQueryLevel(query, "p3", fieldsToMoveUp.ToArray());
            }

            if (xtraHaving.Elements().Elements().Any())
            {
                query.Add(xtraHaving);
            }
        }
        private static string grQueryAlias = "grsets_query";// потом делается поиск, по этому псевдониму, химия
        //Вычисляет значение grouping_id в зависимости от allColumns в заданном порядке и колонок по которым прошла группировка в конкретной строке так же как это делает oracle
        private static decimal getGroupingId(List<string> allColumns, List<string> groupColumns)
        {
            decimal v = 0;
            decimal mp = 1;
            for (int i = allColumns.Count - 1; i > -1; i--)
            {
                decimal v1;
                if (groupColumns.Contains(allColumns[i]))
                {
                    v1 = 0;
                }
                else
                {
                    v1 = 1;
                }

                v += v1 * mp;

                if ((mp * 2) < 0)
                {

                }
                mp = mp * 2;


            }

            return v;
        }
        private static void applyQueryGroupLevel(XElement element, string matName, bool isInReport = false)
            {
            //if (Cmn.GetAttrValue(element, TextConst.AName.Name) == "41050-dat")
            //{

            //}
            XElement query = getQueryGroupLevel(element, matName);
            //query.Elements("from").Elements("query").Elements("select").Attributes("group").Remove();
            if (isInReport)
            {
                query.SetAttributeValue("name", element.Attribute("name").Value + "-gr-" + getAttrValue(element, "grouplevel"));
            }
            element.ReplaceWith(query);
        }

        private static string grkeyPfx="_gr_key";



        internal static void prepareSelfGrsets(XElement element, XElement inQueryCall)
        {
            if (element.Element(TextConst.EName.Grouping) != null)
            {
                bool noGrouping = false;
                if (inQueryCall != null)
                {
                    if (getAttrValue(inQueryCall, TextConst.AName.NoGrouping) == TextConst.AVBool.True)
                    {
                        noGrouping = true;
                    }


                    var callGrp = inQueryCall.Elements(TextConst.EName.Grouping).ToList();
                    if (callGrp.Count != 0) {
                        element.Element(TextConst.EName.Grouping).ReplaceWith(callGrp);
                    }
                }

                if (noGrouping || !element.Element(TextConst.EName.Grouping).Elements().Any())// Заплатка , чтобы убирать grouping при отладке. Чтобы убрать, нужно исключить све из под grouping
                {
                    element.Element(TextConst.EName.Grouping).Remove();
                    element.Element(TextConst.EName.Select).Descendants().Attributes(TextConst.AName.Group).Remove();
                    return;
                }

                var grPontsList = new SortedList<string, int>();
                int i = 0;

              
                var gsets = element.Element(TextConst.EName.Grouping).Elements(TextConst.EName.Grset).ToList();
                if (gsets.Count == 0) // on-columns, on-rows
                {
                    gsets = element.Element(TextConst.EName.Grouping).Elements().Elements(TextConst.EName.Grset).ToList();
                }
                var levelsKeys = new SortedList<int, string>();


                bool addKeys=false;


                if (Cmn.GetAttrValue(element,TextConst.AName.EditColumns)!="")
                {
                    addKeys=true;
                }

                VQuery vqry = null;
                HashSet<string> processedVlinks = new HashSet<string>();
                List<VQueryCall> allVlinks=null;
                while (gsets.Count != 0)
                {
                    foreach (XElement gset in gsets)
                    {
                        

                        var lev = "";
                        var parLev = "";
                        var q = "";
                        var pQ = "";
                        foreach (XElement gpoint in gset.Elements(TextConst.EName.Group).Elements())
                        {
                            
                            string name = "";

                            if (gpoint.Name.LocalName == TextConst.EName.SourceLink)
                            {
                                name = gpoint.Attribute(TextConst.AName.Table).Value;

                                if (addKeys)
                                {
                                    if (vqry == null)
                                    {
                                        vqry = VSXElement.Get<VQuery>(new XElement(element));
                                    }
                                    if (!processedVlinks.Contains(name))
                                    {
                                        if (allVlinks == null)
                                        {
                                            allVlinks = vqry.AllSources();
                                        }
                                        var vlink = allVlinks.First(e => e.XName == name);
                                        processedVlinks.Add(name);
                                        var keyName= vlink.Query().KeyColumn().XName;
                                        var newCol = new XElement(TextConst.AName.Column);

                                        newCol.SetAttributeValue(TextConst.AName.Table, name);
                                        newCol.SetAttributeValue(TextConst.AName.Column, keyName);
                                       // newCol.SetAttributeValue(TextConst.AName.As, keyName+grkeyPfx);
                                        newCol.SetAttributeValue(TextConst.AName.As, name + grkeyPfx);
                                        newCol.SetAttributeValue(TextConst.AName.Removeable2,TextConst.AVBool.False);
                                        newCol.SetAttributeValue(TextConst.AName.Fixed, TextConst.AVBool.True);
                                        element.Element(TextConst.EName.Select).AddFirst(newCol);
                                    }
                                 

                                }

                            }
                            else if (gpoint.Name.LocalName == TextConst.EName.Column)
                            {
                                name = gpoint.Attribute(TextConst.AName.Column).Value;
                            }
                            if (!grPontsList.ContainsKey(name))
                            {
                                i++;
                                grPontsList.Add(name, i);
                            }
                            lev += q + grPontsList[name].ToString();
                            q = ",";

                            if (getAttrValue(gpoint, TextConst.AName.Parent) == TextConst.AVBool.True)
                            {
                                parLev += pQ + grPontsList[name].ToString();
                                pQ = ",";
                            }

                        }
                        gset.SetAttributeValue(TextConst.AName.Level, lev);

                        if (parLev != "")
                        {
                            gset.SetAttributeValue(TextConst.AName.ParentLevel, parLev);
                        }
                        gset.Elements(TextConst.EName.Group).Remove();
                    }

                    gsets = gsets.Elements(TextConst.EName.Grset).ToList();

                }


                foreach (XElement field in element.Element(TextConst.EName.Select).Elements().Where(e => !e.DescendantsAndSelf().Any(e1 => getAttrValue(e1, TextConst.EName.Group) != "")).ToList())
                {
                    var levs = new List<int>();
                    bool thisCols = false;

                    bool isGrExpr = false;
                    if ((field.Name.LocalName == TextConst.EName.Call

                        || field.Name.LocalName == TextConst.EName.Column// 20171204 
                        
                        ) 
                        && grPontsList.ContainsKey(Cmn.GetAttrValue(field, TextConst.AName.As)))
                    {
                        isGrExpr = true;
                        levs.Add(grPontsList[Cmn.GetAttrValue(field, TextConst.AName.As)]);
                    }

                    if (!isGrExpr)
                    {
                        foreach (XElement column in field.DescendantsAndSelf(TextConst.EName.Column).ToList())
                        {
                            var tbln = column.Attribute(TextConst.AName.Table).Value;

                            if (tbln != TextConst.AVTable.Ths)
                            {
                                XElement queryCall = null;
                                bool isGrouped = true;
                                while (!grPontsList.ContainsKey(tbln)) {
                                    if (queryCall == null) {
                                        queryCall = element.Elements(EName.from).Descendants().First(e => getAttrValue(e, AName.@as, AName.name) == tbln);
                                    }
                                    //if (queryCall.Parent.Name.LocalName == TextConst.EName.From)
                                    //{
                                    //    var qube = queryCall.Parent.Element(TextConst.EName.Qube);
                                    //    var allQubeLinks = qube.Descendants(TextConst.EName.Link);
                                    //    var tnames = queryCall.Element(TextConst.EName.Call).Descendants(TextConst.EName.Columns).Select(c => getAttrValue(c, TextConst.EName.Table)).ToList();
                                    //    var connectedLink
                                    //}

                                    queryCall = queryCall.Parent;

                                    if (queryCall.Name.LocalName == TextConst.EName.Qube || queryCall.Name.LocalName == TextConst.EName.From)
                                    {
                                        isGrouped = false;
                                        break;
                                    }
                                    tbln = getAttrValue(queryCall, AName.@as, AName.name);

                                }
                                if (isGrouped)
                                {
                                    levs.Add(grPontsList[tbln]);
                                }
                            }
                            else
                            {
                                thisCols = true;
                            }
                        }
                    }
                    if (levs.Count != 0)
                    {
                        int im = levs.Max();
                        if (!levelsKeys.ContainsKey(im))//!!! ключем считается первая колонка, неправильно, брать ключ из линка и добавлять его при отсутствии
                        {
                            field.SetAttributeValue(TextConst.AName.Group, im.ToString());
                            levelsKeys.Add(im, getAttrValue(field, AName.@as, AName.column));
                        }
                        else
                        {
                            field.SetAttributeValue(TextConst.AName.Group, levelsKeys[im]);
                        }
                    }
                    else
                    {
                        if (thisCols)
                        {
                            field.SetAttributeValue(TextConst.AName.Group, TextConst.AVGroup.Outer);
                        }
                    }
                }

                var grsets = new XElement(TextConst.EName.Grsets);
                grsets.Add(element.Element(TextConst.EName.Grouping).Elements());
                copyAttributes(element.Element(TextConst.EName.Grouping), grsets);
                element.Element(TextConst.EName.Grouping).ReplaceWith(grsets);
               
                preProcessingIGroup(element);

            }
        }
        private static void applySelfGrsets(XElement element, XElement queryCall)
        {
            prepareSelfGrsets(element, queryCall);

            if (element.Element(TextConst.EName.Grsets) != null)
            {

                var pars = element.Elements(TextConst.EName.Params).ToList();

                pars.Remove();
                var xwhere = element.Elements(TextConst.EName.Where).FirstOrDefault();
                if (xwhere != null)
                {
                    xwhere.Remove();
                    element.SetAttributeValue(TextConst.AName.GroupingSource, TextConst.AVBool.True);
                }
                XElement query = getQueryGroupLevel(element, Cmn.GetAttrValue(element, TextConst.AName.Name),true);
             


                var origColsForTreeNames = query.Elements(TextConst.EName.Select).Elements().Attributes(TextConst.AName.TreeOriginalColumn).Select(a => a.Value).Distinct().ToList();

                foreach (string name in origColsForTreeNames)
                {
                    var cols = query.Elements(TextConst.EName.Select).Elements().Where(e => getAttrValue(e, TextConst.AName.TreeOriginalColumn) == name).ToList();
                    int i = 0;
                    var name1 = name.Replace("!", "");
                    var col1 = cols.First(c => getAttrValue(c, TextConst.AName.Column) == name1);
                    col1.Remove();
                    cols.Remove(col1);
                    XElement newExpr = null;
                    //cols.Reverse();
                    foreach (XElement col in cols)
                    {
                        if (i == 0)
                        {

                            newExpr = new XElement(TextConst.EName.Call);
                            newExpr.CopyAttributes(col.Attributes().Where(APredicate.IsCallAttributes));

                            var lvlCol = new XElement(TextConst.AName.Column);
                            //Cmn.CopyAttribute(col, lvlCol, TextConst.AName.Table);
                            lvlCol.SetAttributeValue(TextConst.AName.Table, TextConst.AVTable.Ths);
                            //lvlCol.SetAttributeValue(TextConst.AName.Column, col.Attribute(TextConst.AName.TreeLevelColumn).Value);
                            lvlCol.SetAttributeValue(TextConst.AName.Column, TextConst.AVSpecColumnGrset.GrTreeLevel);
                            newExpr.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Decode);

                            newExpr.SetAttributeValue(TextConst.AName.As, name1);
                            newExpr.Add(lvlCol);
                            col.AddBeforeSelf(newExpr);
                            // col.AddAfterSelf(newExpr);
                        }
                        newExpr.Add(new XElement(TextConst.EName.Const, new XText(col.Attribute(TextConst.AName.Level).Value)));
                        newExpr.Add(col);


                        i++;
                    }
                    newExpr.Add(col1);
                    if (!name.Contains("!"))
                    {
                        cols.Remove();
                    }

                }
               
               
                if (xwhere != null)
                {
                    var qqq1 = query.Descendants(TextConst.EName.Query).First(e => getAttrValue(e, TextConst.AName.GroupingSource) == TextConst.AVBool.True);
                    var xwhere1 = qqq1.Elements(TextConst.EName.Where).FirstOrDefault();
                    if (xwhere1 != null)
                    {
                        xwhere1.Remove();
                    }

                    xwhere = extendWhereByAnd(xwhere1, xwhere.Elements().First());
                    qqq1.Attributes(TextConst.AName.GroupingSource).Remove();
                    qqq1.Add(xwhere);
                }
                // после того как сделал запрос с groupingsets многоуровневым здесь сломалось, 
                //вроде теперь тоже самое, не зависимо от количества уровней 
               // foreach (XElement innerGroupExpr in query.Element(TextConst.EName.From).Element(TextConst.EName.Query).Element(TextConst.EName.From).Element(TextConst.EName.Query).Element(TextConst.EName.Select).Elements().Where
                var qqq = query.Descendants(TextConst.EName.Query).First(e => getAttrValue(e, TextConst.AName.As) == grQueryAlias);
                foreach (XElement innerGroupExpr in qqq.Element(TextConst.EName.From).Element(TextConst.EName.Query).Element(TextConst.EName.From).Element(TextConst.EName.Query).Element(TextConst.EName.Select).Elements().Where

                    (e => e.Descendants().Any(e1 => getAttrValue(e1, TextConst.EName.Group) != "")).ToList()
                    )// Выражения с группировкой внутри
                {
                    innerGroupExpr.Remove();

                    var oldCol = qqq.Element(TextConst.EName.Select).Elements().First(e => getAttrValue(e, TextConst.AName.As) == getAttrValue(innerGroupExpr, TextConst.AName.As));
                    oldCol.AddAfterSelf(innerGroupExpr);
                    oldCol.Remove();

                    // query.Element(TextConst.EName.Select).Add(innerGroupExpr);
                }


                var xhavings = query.Elements(EName.having).ToList();
                if (xhavings.Count != 0) {
                    // это не понятно что, 
                    //вроде просто оборачивается в and
                    //не понятно как может ,быть having в query
                    //после изменений в groupingsets наверняка сломалось
                    XElement xhaving = new XElement(EName.having);
                    XElement xcall = Factory.NewCall(TextConst.AVFunction.And);
                    xcall.Add(xhavings.Elements());
                    xhaving.Add(xcall);
                    xhavings[xhavings.Count - 1].AddAfterSelf(xhaving);
                    xhavings.Remove();
                }


                var els = query.Elements().ToList();
                els.Remove();
                element.Elements().Remove();
                element.Add(pars);
                element.Add(els);

                element.Attributes(AName.@as).Remove();
                copyAttribute(query, element, TextConst.AName.Order);
                copyAttribute(query, element, "haskeys");

                addColumnsAlias(element, false, true);






                markQueryKeys(element);
                preProcessingPivots(element);
                addClassTitles(element);
                collectAdditionalAttrs(element);
                //element.ReplaceWith(query);
            }
        }
        private static void applyGroupLevel(XElement root, string matName)
        {

            foreach (XElement element in root.Descendants("from").Elements("query").Where(e => e.Attribute("grouplevel") != null || e.Element("grsets") != null).ToArray())
            {

                applyQueryGroupLevel(element, matName);
            }
            IEnumerable<XElement> elements = root.DescendantsAndSelf("report").Descendants("query").Where(e => e.Attribute("grouplevel") != null || e.Element("grsets") != null).ToArray();

            while (elements.Any())
            {
                foreach (XElement element in elements)
                {
                    applyQueryGroupLevel(element, matName, true);
                }
                elements = XmlReports.Environment.Manager.GetScheme().Descendants("reports").Descendants("query").Where(e => e.Attribute("grouplevel") != null).ToArray();
            }

            IEnumerable<XElement> qrys = XmlReports.Environment.Manager.GetScheme().Descendants("queries").Descendants("query").Where(e => e.Attribute("grouplevel") != null).ToArray();
            qrys.Elements("having").Remove();
            elements = qrys.Elements("select").Elements().ToArray();
            elements.Where(e => getAttrValue(e, "needdel") == "1").Remove();
            //elements.Attributes("dimname").Remove();
            //elements.Elements("pivot").Remove();


        }

        /*public static void processingPivot(XElement query)
        {
            IEnumerable<XElement> pivotColumns1 = query.Elements("select").Elements("column").Where(e => e.Elements("column").Count() > 0).ToArray();
            if (pivotColumns1.Count() == 0)
            {
                return;
            }

            foreach (XElement pivotCol in pivotColumns1)
            {
                string pivotId = query.Attribute("path").Value + "/" + pivotCol.Attribute("as").Value;
                XElement pivotContent = null;
                if (pivotColumns.Keys.Contains(pivotId))
                {
                    pivotContent = pivotColumns[pivotId];
                }
                else
                {
                    dontReset = true;
                    pivotContent = new XElement("root");
                    pivotColumns.Add(pivotId, pivotContent);
                    XElement dimQuery = pivotCol.Elements("column").Elements("query").First();
                    //XElement compiledDimQuery = compileQuery(dimQuery);
                    VReport rep = XmlReports.environment.GetReport(dimQuery);
                    VDataSet ds = rep.Result();
                    ds.Refresh(false);
                    dontReset = false;
                    int keyIndex = ds.Tables[0].Columns.Count - 2;
                    int titleIndex = ds.Tables[0].Columns.Count - 1;



                    XElement colExpr = new XElement("call"
                            , new XAttribute("function", "if")

                            );
                    copyAttribute(pivotCol, colExpr, "group");
                    copyAttribute(pivotCol, colExpr, "title");
                    XElement condExpr = new XElement("call"
                       , new XAttribute("function", "=nvl")
                       );

                    XElement dimCol = new XElement("column");
                    copyAttributes(pivotCol.Element("column"), dimCol);

                    condExpr.Add(dimCol);
                    XElement cnst = new XElement("const");
                    condExpr.Add(cnst);


                    colExpr.Add(condExpr);

                    XElement col = new XElement("column");
                    copyAttribute(pivotCol, col, "table");
                    copyAttribute(pivotCol, col, "column");

                    colExpr.Add(col);

                    foreach (DataRow r in ds.Tables[0].Rows)
                    {


                        cnst.Value = r[keyIndex].ToString();
                        colExpr.SetAttributeValue("as", pivotCol.Attribute("as").Value + "_" + r[keyIndex].ToString().Replace("-", "_"));
                        pivotContent.Add(new XElement(colExpr));
                      
                    }
                }
                pivotCol.ReplaceWith(pivotContent.Elements());
            }
        }*/
        private static void preProcessingPivots(XElement root)
        {
            //int iii = 0; ;
            //if (getAttrValue(root, "name") == "25499-dat-test")
            //{
            //    iii = 1;


            //}
            //var ddd = iii;
            foreach (XElement pivCol in root.Descendants().Where(e0 => getAttrValue(e0, "dimname") != "").ToArray())
            {
                XElement piv = pivCol.Element("pivot");
                if (piv == null)
                {
                    piv = pivCol.Parent.Elements().Where(e0 => getAttrValue(e0, "dimname") == getAttrValue(pivCol, "dimname")).Elements("pivot").FirstOrDefault();
                    if (piv == null)
                    {
                        piv = pivCol.Ancestors().Last().Descendants().Where(e0 => getAttrValue(e0, "dimname") == getAttrValue(pivCol, "dimname")).Elements("pivot").FirstOrDefault();
                    }
                    pivCol.Add(new XElement(piv));
                }
                var pfx = getAttrValue(piv, TextConst.AName.Pfx);

                if (pfx != "")
                {
                    var alias = getAttrValue(pivCol, TextConst.AName.As);
                    pivCol.SetAttributeValue(TextConst.AName.As, alias + pfx);
                }
            }


            foreach (XElement piv in root.Descendants("pivot"))
            {
                piv.Parent.SetAttributeValue("pivot", "1");

            }
            //foreach (XElement piv in element.Descendants("pivot").Where(e=>e.Ancestors("query").Where(e1=>getAttrValue(e1,"materialize")=="2").Count()==0).ToArray())
            //{
            //    processingPivot(piv);
            //    hasPivots = true;
            //}
            //if (hasPivots)
            //{
            //    element.Descendants("column").Where(e => getAttrValue(e, "as") == "jkey").Remove();// !!! Заплатка, После обработки  makePivotDimQuery остаются левые колоки
            //}
        }
        private static List<string> pivotMatQueriesNames = null;
        internal static void processingPivots(XElement element, XElement pars)
        {
            bool hasPivots = false;
            readyPivots.Clear();

            foreach (XElement pivCol in element.Descendants().Where(e0 => e0.Elements("pivot").Any()).Where(e => e.Ancestors("query").All(e1 => getAttrValue(e1, "materialize") != "2")).ToArray())
            {
                processingPivot(pivCol, false, pars);
                hasPivots = true;
            }

            if (hasPivots)// !!! Вроде можно это удалить
            {
                element.Descendants("column").Where(e => getAttrValue(e, "as") == "jkey").Remove();// !!! Заплатка, После обработки  makePivotDimQuery остаются левые колоки
                reassignInto(element);
            }

        }
        private static void reassignInto(XElement element)
        {
            foreach (XElement select in element.Descendants(EName.query).Elements(EName.select).Where(e => e.Elements().Any(e1 => e1.Attribute(AName.into) != null)).ToList()) {
                List<string> types = select.Elements().Attributes(AName.type).Select(e => e.Value).Distinct().ToList();
                foreach (string t in types) {
                    int i = 1;
                    string prefix = getTyprPr(t);
                    foreach (XElement col in select.Elements().Where(e => e.AttrOrDefault(AName.type, string.Empty) == t && e.AttrOrDefault(AName.into, string.Empty) != string.Empty).ToList()) {
                        if (!IsSysColumnName(col.Attribute(AName.into).Value)) {
                            col.SetAttributeValue(AName.into, string.Intern(prefix + i.ToString()));
                            i++;
                        }
                    }
                }
            }
        }
        private static SortedList<string, VDataSet> readyPivots = new SortedList<string, VDataSet>();
        private static void processingPivot(XElement pivCol, bool isFact, XElement pars)
        {
            string dimName = getAttrValue(pivCol, "dimname");
            if (dimName == "")
            {
                dimName = pivCol.Element("pivot").Elements().First().Attribute("as").Value;
            }
            VDataSet ds = null;
            XElement piv = null;
            piv = pivCol.Element("pivot");


            if (readyPivots.ContainsKey(dimName))
            {
                ds = readyPivots[dimName];
            }
            else
            {


                XElement dimQuery = makePivotDimQuery(piv);
                PushProcessingCollections();
                VReport rep = XmlReports.Environment.GetPrecompiledReport(dimQuery);
                ds = rep.Result(2, false);
              


                if (ds.MatQueriesNames != null)
                {
                    pivotMatQueriesNames.AddRange(ds.MatQueriesNames);
                }
                if (pars != null)
                {
                    ds.Refresh(pars); //!!! 20170104 для применения параметров у запроса с сохраненной в проекте скомпилированной моделью
                }
                else
                {
                    ds.Refresh();
                }
                if (piv.Element("query") != null) //!!! заплатка доделать с учетом параметров, через path
                {
                    readyPivots.Add(dimName, ds);
                }
                PopProcessingCollections();
                //ClearMatSetAndPivotQueriesList();
            }





            XElement pivotContent = new XElement("root", new XAttribute("as", pivCol.Attribute("as").Value)/*,new XAttribute("title",getAttrValue(piv,"title"))*/);
            int startColIndex = 0;

            if (ds.Tables[0].Columns[0].ColumnName == "sid")
            {
                startColIndex += 2;
            }

            int keyIndex = startColIndex;// ds.Tables[0].Columns.Count - 2;
            int titleIndex = startColIndex + 1;//ds.Tables[0].Columns.Count - 1;

            bool isPivChilds = false;

            List<XElement> pivChilds = pivCol.Descendants().Where(e => getAttrValue(e, "pivtarg") == "1").ToList();
            if (pivChilds.Count > 0)
            {
                isPivChilds = true;
            }

            XElement colExpr = null;
            if (!isFact)
            {
                colExpr = new XElement("call"
                       , new XAttribute("function", "if")
                       );
            }
            else
            {
                colExpr = new XElement(pivCol);
                colExpr.Elements(TextConst.EName.Pivot).Remove();
                colExpr.Attributes(TextConst.AName.Dimname).Remove();
            }

            XElement colExpr1 = null;
            //copyAttribute(pivotCol, colExpr, "group");

            if (!isPivChilds)
            {
                copyAttribute(pivCol, colExpr, "title");
                copyAttribute(pivCol, colExpr, "type");
                copyAttribute(pivCol, colExpr, "nvl");
                copyAttribute(pivCol, colExpr, TextConst.AName.Format);
                //copyAttributes(pivCol, colExpr, AdditionalAttributes);
                colExpr1 = colExpr;
            }
            else
            {
                colExpr1 = new XElement(pivCol);
                colExpr1.Elements("pivot").Remove();

            }




            XElement condExpr;


            XElement dimCol = new XElement(piv.Elements().First());


            bool isSpecCond = false;
            string dimQueryAlias = "dim";//getAttrValue(piv.Elements("query").FirstOrDefault(), "as");
            List<XElement> specCondParams = new List<XElement>();
            if (!isFact)
            {
                specCondParams = piv.Elements().First().Descendants("column").Where(e => getAttrValue(e, "table") == dimQueryAlias).ToList();
                if (specCondParams.Count != 0)
                {
                    isSpecCond = true;
                }
            }

            //XElement dimval = dimCol.Descendants("dimvalue").FirstOrDefault();
            XElement cnst = new XElement("const");
            XElement colExpr2 = null;
            if (!isFact)
            {
                if (!isSpecCond)
                {


                    condExpr = new XElement("call"
                       , new XAttribute("function", "=")
                       );


                    //  copyAttributes(pivotCol.Element("pivot").Element("column"), dimCol);

                    condExpr.Add(dimCol);

                    condExpr.Add(cnst);
                }
                else
                {
                    condExpr = dimCol;

                    // dimval.ReplaceWith(cnst);
                }

                colExpr.Add(condExpr);
            }
            else
            {
                var dCol = colExpr.Descendants(TextConst.AName.Column).First(e => getAttrValue(e, TextConst.AName.Table) == dimQueryAlias);
                dCol.ReplaceWith(cnst);
            }
            XElement col = new XElement(pivCol);
            //  copyAttribute(pivotCol, col, "table");
            //  copyAttribute(pivotCol, col, "column");
            col.Elements("pivot").Remove();

            if (!isFact)
            {
                if (!isPivChilds)
                {
                    col.Attributes("group").Remove();

                    colExpr.Add(col);
                    copyAttribute(pivCol, colExpr, "group");
                }
            }




            XElement colExpr5 = null;
            if (isSpecCond)
            {
                colExpr2 = new XElement(colExpr1);
                colExpr5 = new XElement(colExpr);
            }

            if (!isFact)
            {
                if (!isSpecCond)
                {
                    pivChilds.Add(col);
                }
            }



            foreach (DataRow r in ds.Tables[0].Rows)
            {
                string v1;

                if (ds.Tables[0].Columns[keyIndex].DataType == typeof(string))
                {
                    v1 = "'" + r[keyIndex].ToString() + "'";
                }
                else
                {
                    v1 = r[keyIndex].ToString().Replace(",", ".");
                }
                if (!isSpecCond)
                {

                    cnst.Value = v1;

                    if ((new string[] { "is", "=" }).Contains(getAttrValue(cnst.Parent, "function")))
                    {
                        if (cnst.Value == "")
                        {
                            cnst.Value = "null";
                            cnst.Parent.SetAttributeValue("function", "is");
                        }
                        else
                        {
                            cnst.Parent.SetAttributeValue("function", "=");
                        }
                    }
                }
                else
                {
                    specCondParams = colExpr.Descendants("column").Where(e => getAttrValue(e, "table") == dimQueryAlias).ToList();
                    foreach (XElement specCondParam in specCondParams)
                    {
                        v1 = Cmn.ToOracleString(r[specCondParam.Attribute("column").Value]);
                        specCondParam.ReplaceWith(new XElement("const", new XText(v1)));
                    }
                }

                string s_pfx = "_" + r[keyIndex].ToString().Replace("-", "_").Replace(".", "_").Replace(",", "_");

                colExpr1.SetAttributeValue("as", pivCol.Attribute("as").Value + s_pfx);



                //если есть атрибут agg то проверяем на наличие префикса и заменяем
                if (col.Attribute("agg") != null)
                    col.Attribute("agg").SetValue(col.Attribute("agg").Value.Replace("[pfx]", s_pfx));

                string title = r[titleIndex].ToString();
                if (title == "")
                {
                    title = "-";
                }

                if (isPivChilds)
                {
                    pivChilds = colExpr1.Descendants().Where(e => getAttrValue(e, "pivtarg") == "1").ToList();
                    XElement colExpr4 = new XElement(colExpr);
                    foreach (XElement pivChild in pivChilds)
                    {
                        colExpr = new XElement(colExpr4);
                        colExpr.Add(new XElement(pivChild));
                        colExpr = (XElement)expression(new XElement(colExpr), null).First();
                        pivChild.ReplaceWith(colExpr);
                    }

                }


                XElement colExpr3 = null;
                if (!isFact)
                {
                    colExpr3 = (XElement)expression(new XElement(colExpr1), null).First();
                }
                else
                {
                    colExpr3 = new XElement(colExpr1);
                }
                colExpr3.SetAttributeValue("value-column", pivCol.Attribute("as").Value);
                colExpr3.SetAttributeValue("dimension-column", dimName);
                colExpr3.SetAttributeValue("dimension-value", r[keyIndex].ToString());
                colExpr3.SetAttributeValue("band-title", title);
                colExpr3.SetAttributeValue("value-title", getAttrValue(pivCol, "title"));

                if (pivCol.Attribute(TextConst.AName.Into) != null)
                {
                    colExpr3.SetAttributeValue(TextConst.AName.Into, getAttrValue(pivCol, TextConst.AName.Into));
                }



                //   copyAttribute( pivCol,colExpr1, "group");


                pivotContent.Add(colExpr3);

                if (isSpecCond)
                {

                    colExpr1 = new XElement(colExpr2);
                    if (!isPivChilds)
                    {
                        colExpr = colExpr1;
                    }
                    else
                    {
                        colExpr = new XElement(colExpr5);
                    }
                }


            }



            XElement pushedAncestor = pivCol.Ancestors("query").FirstOrDefault(e => e.Parent != null && // 20161508
                !(new string[] { "query", "from", "root", "with" }).Contains(e.Parent.Name.LocalName));
            XElement pushedField = null;

            if (pushedAncestor != null)
            {
                pivCol.SetAttributeValue("curpivotcol", "1");
                pushedField = pushedAncestor.AncestorsAndSelf().Where(e => e.Parent != null).First(e1 => e1.Parent.Name.LocalName == "select");
                XElement pivotContent1 = new XElement(pivotContent);
                foreach (XElement colExpr3 in pivotContent.Elements().ToArray())
                {
                    pivotContent1.Elements().Remove();
                    pivotContent1.Add(new XElement(colExpr3));

                    XElement pushedField1 = new XElement(pushedField);

                    string alias = pushedField1.Attribute("as").Value;
                    pushedField1.SetAttributeValue("value-column", alias);


                    pushedField1.Attribute("as").Value = alias + "_" + colExpr3.Attribute("dimension-value").Value.Replace("-", "_").Replace(".", "_").Replace(",", "_");
                    if (pushedField1.Attribute("title") != null)
                    {
                        string title = pushedField1.Attribute("title").Value;
                        pushedField1.SetAttributeValue("value-title", title);

                        pushedField1.Attribute("title").Value = title + " " + colExpr3.Attribute("band-title").Value;
                    }
                    pushedField.AddBeforeSelf(pushedField1);
                    XElement pivotCol1 = pushedField1.Descendants().First(e => getAttrValue(e, "curpivotcol") == "1");
                    pivotCol1.Attributes("curpivotcol").Remove();

                    colExpr3.Remove();
                    //pivotCol.AddBeforeSelf(colExpr1);
                    pivotCol1.ReplaceWith(colExpr3);


                    var processed = new HashSet<XElement>();

                    XElement query = colExpr3.Ancestors("query").First();
                    processed.Add(query);
                    expandPivotNext(pivotContent1, query, processed);


                }
                pushedField.Remove();
            }
            else
            {
                XElement query = pivCol.Ancestors("query").First();
                pivCol.ReplaceWith(pivotContent.Elements());

                var processed = new HashSet<XElement>();
                processed.Add(query);
                expandPivotNext(pivotContent, query, processed);
            }




        }
        private static void expandPivotNext(XElement childPivotContent, XElement childQuery, HashSet<XElement> processed)
        {
            XElement parentQuery = childQuery.Ancestors("query").FirstOrDefault();



            if (parentQuery != null)
            {
                //!!! Может быть нужно сделать аналогичные манипуляции для else, пока нет примера

                XElement parent1 = parentQuery;
                bool isUnion = false;
                while (parentQuery.Element("select") == null)
                {
                    isUnion = true;
                    parentQuery = parentQuery.Ancestors("query").FirstOrDefault();
                }
                processed.Add(parentQuery);
                expandPivot(childPivotContent, childQuery, parentQuery, processed);
                if (isUnion)
                {
                    foreach (XElement qry in parent1.Elements())
                    {
                        XElement col1 = qry.Elements("select").Elements("const").FirstOrDefault(e => e.Attribute("as").Value == childPivotContent.Attribute("as").Value);
                        if (col1 != null)
                        {
                            XElement buf = new XElement("buf");
                            foreach (XElement col2 in childPivotContent.Elements())
                            {
                                XElement col3 = new XElement(col1);
                                col3.Attribute("as").Value = col2.Attribute("as").Value;
                                buf.Add(col3);
                            }
                            col1.ReplaceWith(buf.Elements());
                        }

                    }
                }
            }
            else
            {

                if (getAttrValue(childQuery, "materialize") == "1")
                {
                    foreach (XElement parentQuery1 in childQuery.Ancestors().Last().Descendants("query").Where(
                        q => getAttrValue(q, "name") == childQuery.Attribute("name").Value && getAttrValue(q, "materialize") != ""// && getAttrValue(q, "materialize") == "2"
                        ))
                    {
                        if (!processed.Contains(parentQuery1))
                        {
                            var pivotCalls = parentQuery1.Elements(TextConst.EName.Select).Elements(TextConst.EName.Call).Elements(TextConst.EName.Pivot).ToList();

                            if (pivotCalls.Count != 0)
                            {
                                foreach (var piv in pivotCalls)
                                {
                                    var col = new XElement(TextConst.EName.Column);
                                    Cmn.copyAttributes(piv.Parent, col);
                                    piv.Parent.ReplaceWith(col);
                                }

                            }
                            processed.Add(parentQuery1);
                            expandPivot(childPivotContent, childQuery, parentQuery1, processed);
                        }
                    }
                }
            }


        }
        private static void expandPivot(XElement childPivotContent, XElement childQuery, XElement parentQuery, HashSet<XElement> processed)
        {
            string childAlias = "";
            if (childQuery.Attribute("as") != null)
            {
                childAlias = childQuery.Attribute("as").Value;
            }
            else
            {
                if (getAttrValue(childQuery, "materialize") != "1")
                {
                    childAlias = childQuery.Ancestors("query").First(e => e.Attribute("as") != null).Attribute("as").Value;
                }
            }

            IEnumerable<XElement> parentPivotCols = null;
            if (getAttrValue(childQuery, "materialize") != "1")
            {
                parentPivotCols = getQueryColumnsSel(parentQuery).Where(
                         e => e.Attribute("table").Value == childAlias &&
                            e.Attribute("column").Value == childPivotContent.Attribute("as").Value).ToArray();
            }
            else
            {
                parentPivotCols = getQueryColumnsSel(parentQuery).Where(
                        e =>
                             getAttrValue(e, "as") == childPivotContent.Attribute("as").Value).ToArray();

            }

            foreach (XElement parentPivotCol in parentPivotCols)
            {
                //!!! Не обработан случай? когда одна и та же колонка присутствует в одном поле несколько раз например при использовании if >0
                XElement pivotField = parentPivotCol.AncestorsAndSelf().Where(e => e.Parent != null).First(e1 => e1.Parent.Name.LocalName == "select");
                XElement parentPivotContent = new XElement("root", new XAttribute("as", pivotField.Attribute("as").Value)/*, new XAttribute("title", childPivotContent.Attribute("title").Value)*/);

                //bool wasLast = false;
                XElement newPivotField = null;
                foreach (XElement childPivotColumn in childPivotContent.Elements())
                {
                    parentPivotCol.SetAttributeValue(TextConst.EName.Column, childPivotColumn.Attribute("as").Value);
                     newPivotField = new XElement(pivotField);

                    string alias = pivotField.Attribute("as").Value;
                    newPivotField.SetAttributeValue("value-column", alias);
                    newPivotField.Elements(TextConst.EName.Pivot).Remove();
                    string s_pfx = "_" + childPivotColumn.Attribute("dimension-value").Value.Replace("-", "_").Replace(".", "_").Replace(",", "_");
                    if (String.Format("{0}{1}", alias, s_pfx) == "power_per_1000_1000")
                    {
                    }
                    newPivotField.Attribute("as").Value = String.Format("{0}{1}", alias, s_pfx);

                    //если есть атрибут agg то проверяем на наличие префикса и заменяем
                    if (newPivotField.Attribute("agg") != null)
                        newPivotField.Attribute("agg").SetValue(newPivotField.Attribute("agg").Value.Replace("[pfx]", s_pfx));

                    if (newPivotField.Attribute("title") != null)
                    {
                        string title = pivotField.Attribute("title").Value;
                        //if (string.IsNullOrEmpty(title))
                        //{
                        //    title = "   ";
                        //}
                        newPivotField.SetAttributeValue("value-title", title /*+ childPivotContent.Attribute("title").Value*/);
                        newPivotField.Attribute("title").Value = title + " " + childPivotColumn.Attribute("band-title").Value;
                    }

                    copyAttribute(childPivotColumn, newPivotField, "dimension-value");
                    copyAttribute(childPivotColumn, newPivotField, "band-title");
                    copyAttribute(childPivotColumn, newPivotField, "dimension-column");

                    parentPivotContent.Add(newPivotField);
                }

               
                pivotField.ReplaceWith(parentPivotContent.Elements());
                expandPivotNext(parentPivotContent, parentQuery, processed);
            }


        }
        private static XElement makePivotDimQuery(XElement element)
        {

            XElement readyQuery = element.Element("query");
            if (readyQuery != null)
            {
                var dimname = element.Parent.Attribute(TextConst.AName.Dimname).Value;
                readyQuery = pivotQueries[dimname];
                return new XElement(readyQuery);
            }
            XElement srcQuery = new XElement(element.Ancestors("query").First());

            if (srcQuery.Attribute("as") == null)
            {
                srcQuery.SetAttributeValue("as", "a");
            }

            XElement query = new XElement("query", new XElement("select"), new XElement("from", srcQuery));

            int i = 0;
            foreach (XElement col in element.Elements())
            {
                XElement newCol = new XElement("column");
                XElement newCol1 = new XElement(col);
                if (i == 0)
                {

                    newCol.SetAttributeValue("group", "1");
                    newCol.SetAttributeValue("column", "id");
                    newCol.SetAttributeValue("as", "id");
                    newCol1.SetAttributeValue("as", "id");
                }
                if (i == 1)
                {
                    newCol.SetAttributeValue("group", "max");
                    newCol.SetAttributeValue("column", "title");
                    newCol.SetAttributeValue("as", "title");
                    newCol1.SetAttributeValue("as", "title");
                }
                if (i == 2)
                {
                    newCol.SetAttributeValue("group", "max");
                    newCol.SetAttributeValue("column", "ord");
                    newCol1.SetAttributeValue("as", "ord");
                    query.SetAttributeValue("order", "ord");
                }
                newCol.SetAttributeValue("table", srcQuery.Attribute("as").Value);
                query.Element("select").Add(newCol);
                srcQuery.Element("select").Add(newCol1);
                i++;
            }
            if (query.Attribute("order") == null)
            {
                query.SetAttributeValue("order", "title");
            }

            XElement parentQuery = element.Ancestors("query").First().Ancestors("query").FirstOrDefault();
            srcQuery.Elements("call").Remove();
            srcQuery.Attributes("join").Remove();
            if (parentQuery != null)
            {
                changeChildAliases(srcQuery, "_x0");
                XElement restQuery = makePivotDimQueryRestriction(element);


                if (restQuery != null)
                {
                    // XElement condCont = extendWhere(srcQuery);
                    // condCont.Add(restQuery.Elements("where").Elements("call").Elements("call").Where(e => e.Attribute("function").Value == "exists"));
                    XElement restJoin = restQuery.Element("from").Elements().Last();
                    srcQuery.Element("from").Add(restJoin);
                }
                /*XElement keyCol = srcQuery.Elements("select").Elements().Where (e => getAttrValue(e, "key") == "1").First();
                condCont.Add(new XElement("call", new XAttribute("function", "in"),
                   
                    new XElement (keyCol),
                    restQuery
                    ));*/
            }


            query.Descendants().Attributes("used").Remove();
            query.Descendants("pivot").Remove();
            doMatrializeByHint = false;
            query = compileQuery(query, true, null).Elements().First();

            doMatrializeByHint = true;

            return query;
        }
        /*internal static void addMultiKeyColumns(XElement element)
        {
            foreach (XElement querySelect in element.Descendants("query").Elements("select").Where(e => e.Elements().Count(e1 => getAttrValue(e1, "key") == "1") > 1))
            {
                // foreach(XEelement keyCol in) !!!дописать 
            }
        }*/
        /// <summary>
        /// Дополняет условие в where запроса <paramref name="query"/> условием &lt;call function="and" /&gt;
        /// </summary>
        /// <param name="query">исходный запрос</param>
        /// <returns>условие &lt;call function="and" /&gt;</returns>
        private static XElement extendWhereByAnd(XElement query)
        {
            Contract.Assert(query != null);
            XElement xwhere = query.Element(EName.where);
            XElement xcall, xand;
            if (xwhere == null) {
                xwhere = new XElement(EName.where);
                query.Add(xwhere);
                xcall = null;
            } else {
                xcall = xwhere.Elements(EName.call).FirstOrDefault();
            }
            if (xcall != null && xcall.AttrOrDefault(AName.function, null) == TextConst.AVFunction.And) {
                xand = xcall;
            } else {
                xand = Factory.NewCall(TextConst.AVFunction.And);
                xwhere.Elements().ChangeParent(xand);
                xwhere.Add(xand);
            }
            return xand;
        }
        internal static XElement extendWhereByAnd(XElement whereElement, XElement newCondition)
        {
            if (whereElement == null) {
                whereElement = new XElement(EName.where);
                whereElement.Add(newCondition);
                return whereElement;
            }
            if (!whereElement.HasElements) {
                // Если в where нет условий, то просто дополняем его новым условием
                whereElement.Add(newCondition);
                return whereElement;
            }
            XElement xcall = whereElement.Elements(EName.call).FirstOrDefault();
            XElement xand;
            if (xcall != null && xcall.AttrOrDefault(AName.function, null) == TextConst.AVFunction.And) {
                xand = xcall;
            } else {
                xand = Factory.NewCall(TextConst.AVFunction.And);
                whereElement.Elements().ChangeParent(xand);
                whereElement.Add(xand);
            }
            xand.Add(newCondition);
            return whereElement;
        }
        // выворачивает запрос на изнанку, чтобы получить записи подзапроса участвующие в результатах основного запроса, есть сомнения что учтены все случаи.
        // скорее всего не будет работать с множественными ключами и join ом выполненным не про ключу основного запроса
        // !!! Доделать, когда будет пример

        /*  public static XElement makePivotDimQueryRestriction(XElement element)
          {
              List<XElement> queryList = element.Ancestors("query").ToList();
              XElement parentQuery = null;
              XElement query1=null; 
              for (int i = queryList.Count - 1; i > -1; i--)
              {
                  query1 = new XElement(queryList[i]);

              
               


                  XElement thisQueryInParent =null;
                  bool pushed = false;
                  if (parentQuery != null)
                  {
                  
                      if (!(new string[] { "query", "from" }).Contains(queryList[i].Parent.Name.LocalName))//Подзапрос  полученный через dlinkPush
                      {
                          pushed = true;
                         // string targetAlias = queryList[0].Attribute("as").Value;
                          XElement lastQuery = query1.Element("from").Elements().Where(e => e.Attribute("as").Value == queryList[0].Attribute("as").Value).First();
                          XElement lastQueryKey = lastQuery.Element("select").Elements().Where(e => e.Attribute("key").Value == "1").First();
                          query1.Element("select").Add(
                              new XElement("column",

                                          new XAttribute("table", queryList[0].Attribute("as").Value),
                                           new XAttribute("column", lastQueryKey.Attribute("as").Value),
                                           new XAttribute("as", lastQueryKey.Attribute("as").Value),
                                           new XAttribute("key", "1")
                                  )
                              );
                          XElement joinExp = query1.Elements("where").Descendants("call").Where(e => getAttrValue(e, "joinexp") == "1").First();
                          query1.Add(new XElement(joinExp));
                          joinExp.Remove();
                      }

                      thisQueryInParent = new XElement(query1);
                  }

                  query1.Element("select").Elements().Where(e => getAttrValue(e, "key") == "0").Remove();
                  query1.Descendants().Attributes("used").Remove();
                  query1.Descendants("pivot").Remove();
                  query1.Attributes("name").Remove();
                  query1.Attributes("join").Remove();
                  string alias = getAttrValue(query1, "as");
                  if (alias == "")
                  {
                      alias = "root";
                  }
                  query1.Attributes("as").Remove();
                  query1.Elements("call").Remove();
              
                  if (parentQuery != null)
                  {

                     // XElement thisQueryInParent = parentQuery.Elements("from").Elements("query").Where(e => e.Attribute("as").Value == alias).FirstOrDefault();

                   

                      //foreach (XElement col in getQueryColumns(parentQuery).Where(e => e.Attribute("table").Value == alias).ToArray())
                      //{
                      //    col.Attribute("table").Value = alias + "_forpiv";
                      //}
                      string alias2 = "";
                      if (!pushed)
                      {
                          alias2 = alias + "_forpiv";
                          thisQueryInParent.Attribute("as").Value = alias2;
                          thisQueryInParent.SetAttributeValue("join", "inner");
                          thisQueryInParent.Elements("where").Remove();
                          thisQueryInParent.Elements("select").Descendants().Attributes("group").Remove();

                          parentQuery.Element("from").Add(thisQueryInParent);
                          foreach (XElement col in thisQueryInParent.Elements("call").Descendants("column").Where(e => e.Attribute("table").Value == alias).ToArray())// !!! проблемы при подзапросах в условиях
                          {
                              col.Attribute("table").Value = alias2;
                          }
                      }
                 
                      XElement andNode = extendWhere(query1);
                   
                   


                      XElement andNode1 = extendWhere(parentQuery);

                      if (!pushed)
                      {
                          XElement condExpr1 = new XElement("call", new XAttribute("function", "="),
                              // thisQueryInParent.Element("select").Elements().Where(e=>getAttrValue(e,"key")=="1"),
                                   query1.Element("select").Elements().Where(e => getAttrValue(e, "key") == "1").First()
                                  );

                          XElement key2 = thisQueryInParent.Element("select").Elements().Where(e => getAttrValue(e, "key") == "1").First();
                          // key2.Attribute("table").Value = alias2;
                          condExpr1.Add(new XElement("column", new XAttribute("table", alias2), new XAttribute("column", key2.Attribute("as").Value)));
                          andNode1.Add(
                              condExpr1
                              );

                      }
                      else
                      {
                          andNode1.Add(thisQueryInParent.Elements("call"));
                      }
                      XElement condExpr = new XElement("call", new XAttribute("function", "exists"),
                           parentQuery
                       );
                      andNode.Add(condExpr);
                  
                  }

                 // query1.Descendants().Attributes("materialize").Remove();
             

                  query1 = compileQuery(query1).Elements().First();
                  parentQuery = query1;
               
                  //VReport rep = XmlReports.environment.GetReport(query1);
                  //VDataSet ds = rep.Result();
                  //ds.Refresh(false);

                  query1.SetAttributeValue("as", alias);

              }


          

              return query1;
          }
           */
        private static void changeChildAliases(XElement query, string pfx)
        {
            foreach (XElement childQuery in query.Element(EName.from).Elements()) {
                XAttribute attr = childQuery.Attribute(AName.@as);
                string oldAlias = attr.Value;
                string newAlias = oldAlias + pfx;
                attr.Value = newAlias;
                XElement parent = childQuery.Ancestors(EName.query).First();
                foreach (XElement col in parent.Element(EName.from).Elements().Elements(EName.call).Descendants(EName.column).ToList()) { // !!! проблемы при подзапросах в условиях
                    attr = col.Attribute(AName.table);
                    if (attr.Value == oldAlias) {
                        attr.Value = newAlias;
                    }
                }
                foreach (XElement col in getQueryColumns(parent).ToList()) {
                    attr = col.Attribute(AName.table);
                    if (attr.Value == oldAlias) {
                        attr.Value = newAlias;
                    }
                }
            }
        }
        /*  public static XElement makePivotDimQueryRestriction(XElement element)
          {
              List<XElement> queryList = element.Ancestors("query")
                  .Where(
                  e=>e.AncestorsAndSelf("query").Where(
                      e1=>e1.Element("where")!=null ||
                      e.Elements("from").Elements().Where(e2 => getAttrValue(e2, "join") == "inner").Count() > 0  //еще нужно исключить след уровень 
                      ).Count()>0
                  )
                  .ToList();
              XElement parentQuery = null;
              XElement query1 = null;
              for (int i = queryList.Count - 1; i > -1; i--)
              {
                  query1 = new XElement(queryList[i]);



                  string alias = getAttrValue(query1, "as");
                  if (alias == "")
                  {
                      alias = "root";
                  }

                  XElement thisQueryInParent = null;
                  bool pushed = false;
                  if (parentQuery != null)
                  {

                      if (!(new string[] { "query", "from" }).Contains(queryList[i].Parent.Name.LocalName))//Подзапрос  полученный через dlinkPush
                      {
                          pushed = true;
                          // string targetAlias = queryList[0].Attribute("as").Value;
                          XElement lastQuery = query1.Element("from").Elements().Where(e => e.Attribute("as").Value == queryList[0].Attribute("as").Value).First();
                          XElement lastQueryKey = lastQuery.Element("select").Elements().Where(e => e.Attribute("key").Value == "1").First();
                          query1.Element("select").Add(
                              new XElement("column",

                                          new XAttribute("table", queryList[0].Attribute("as").Value),
                                           new XAttribute("column", lastQueryKey.Attribute("as").Value),
                                           new XAttribute("as", lastQueryKey.Attribute("as").Value),
                                           new XAttribute("key", "1")
                                  )
                              );
                          XElement joinExp = query1.Elements("where").Descendants("call").Where(e => getAttrValue(e, "joinexp") == "1").First();
                          query1.Add(new XElement(joinExp));
                          joinExp.Remove();
                          thisQueryInParent = new XElement(query1);
                      }

                      //
                      else
                      {
                          thisQueryInParent = parentQuery.Element("from").Elements().Where(e => getAttrValue(e, "as") == alias+"_x"+(i+1).ToString()).First();

                      }
                  }
                  query1.Element("select").Elements().Where(e => getAttrValue(e, "key") == "0").Remove();
                  query1.Descendants().Attributes("used").Remove();
                  query1.Descendants("pivot").Remove();
                  query1.Attributes("name").Remove();
                  query1.Attributes("order").Remove();
                  query1.Attributes("materialize").Remove();
                  query1.Attributes("join").Remove();
                  query1.DescendantsAndSelf().Attributes("grouplevel").Remove();
               
                  query1.Attributes("as").Remove();
                  query1.Elements("call").Remove();

                  XElement nextQuery = null;
                  if (i > 0)
                  {
                      nextQuery = query1.Element("from").Elements().Where(e => e.Attribute("as").Value == queryList[i - 1].Attribute("as").Value).FirstOrDefault();
                      if (nextQuery != null)
                      {
                          nextQuery.SetAttributeValue("fixed", "1");
                      }
                  }

                  if (!pushed)
                  {

                      changeChildAliases(query1, "_x" + i.ToString());
                   



                  }



                  if (parentQuery != null)
                  {

                      // XElement thisQueryInParent = parentQuery.Elements("from").Elements("query").Where(e => e.Attribute("as").Value == alias).FirstOrDefault();



                   
                   
                   

                      XElement andNode = extendWhere(query1);




                      XElement andNode1 = extendWhere(parentQuery);

                      if (!pushed)
                      {
                          XElement condExpr1 = new XElement("call", new XAttribute("function", "and"));

                          foreach (XElement key1 in query1.Element("select").Elements().Where(e => getAttrValue(e, "key") == "1"))
                          {
                              XElement condExpr2 = new XElement("call", new XAttribute("function", "="),

                                   key1
                                  );
                              XElement key2 = thisQueryInParent.Element("select").Elements().Where(e => getAttrValue(e, "as") == key1.Attribute("as").Value).First();
                              condExpr2.Add(new XElement("column", new XAttribute("table", thisQueryInParent.Attribute("as").Value), new XAttribute("column", key2.Attribute("as").Value)));
                              condExpr1.Add(condExpr2);
                          }

                      
                          andNode1.Add(
                              condExpr1
                              );

                      }
                      else
                      {
                          andNode1.Add(thisQueryInParent.Elements("call"));
                      }
                      XElement condExpr = new XElement("call", new XAttribute("function", "exists"),
                           parentQuery
                       );
                      andNode.Add(condExpr);

                  }

               
               
                  // query1.Descendants().Attributes("materialize").Remove();

                  query1.Descendants ("text").Where(e=>getAttrValue(e,"txtype")=="func").Remove();
                  doMatrializeByHint = false;
               
                  query1 = compileQuery(query1).Elements().First();
                  doMatrializeByHint = true;
                  parentQuery = query1;

                 VReport rep = XmlReports.environment.GetReport(query1);
                 VDataSet ds = rep.Result();
                 ds.Refresh(false);

                  query1.SetAttributeValue("as", alias);

              }




              return query1;
          }
           */
        private static XElement makePivotDimQueryRestriction(XElement element)
        {
            List<XElement> queryList = element.Ancestors("query")
                .Where(
                e => e.AncestorsAndSelf(
                    "query"  //еще нужно исключить след уровень 
                    ).Any(e1 => e1.Element("where") != null ||
                    e.Elements("from").Elements().Any(e2 => getAttrValue(e2, "join") == "inner"))
                )
                .ToList();
            XElement parentQuery = null;
            XElement query1 = null;
            for (int i = queryList.Count - 1; i > -1; i--)
            {
                query1 = new XElement(queryList[i]);



                string alias = getAttrValue(query1, "as");
                if (alias == "")
                {
                    alias = "root";
                }

                XElement thisQueryInParent = null;
                bool pushed = false;
                if (parentQuery != null)
                {

                    if (!(new string[] { "query", "from" }).Contains(queryList[i].Parent.Name.LocalName))//Подзапрос  полученный через dlinkPush
                    //!!!Скорее всего, сломалось, после того, как переписал кусок !pushed
                    {
                        pushed = true;
                        // string targetAlias = queryList[0].Attribute("as").Value;
                        XElement lastQuery = query1.Element("from").Elements().First(e => e.Attribute("as").Value == queryList[0].Attribute("as").Value);
                        XElement lastQueryKey = lastQuery.Element("select").Elements().First(e => e.Attribute("key").Value == "1");
                        query1.Element("select").Add(
                            new XElement("column",

                                        new XAttribute("table", queryList[0].Attribute("as").Value),
                                         new XAttribute("column", lastQueryKey.Attribute("as").Value),
                                         new XAttribute("as", lastQueryKey.Attribute("as").Value),
                                         new XAttribute("key", "1")
                                )
                            );
                        XElement joinExp = query1.Elements("where").Descendants("call").First(e => getAttrValue(e, "joinexp") == "1");
                        query1.Add(new XElement(joinExp));
                        joinExp.Remove();
                        thisQueryInParent = new XElement(query1);
                    }

                    //
                    else
                    {
                        thisQueryInParent = parentQuery.Element("from").Elements().First(e => getAttrValue(e, "as") == alias + "_x" + (i + 1).ToString());

                    }
                }
                query1.Element("select").Elements().Where(e => getAttrValue(e, "key") == "0").Remove();
                query1.Descendants().Attributes("used").Remove();
                query1.Descendants("pivot").Remove();
                query1.Attributes("name").Remove();
                query1.Attributes("order").Remove();
                query1.Attributes("materialize").Remove();
                query1.Attributes("join").Remove();
                query1.DescendantsAndSelf().Attributes("grouplevel").Remove();
                getQueryColumnsSel(query1).Attributes("group").Remove();// !!!! ? , не изменит ли алгоритм применения условий
                query1.Attributes("as").Remove();
                query1.Elements("call").Remove();

                XElement nextQuery = null;
                if (i > 0)
                {
                    nextQuery = query1.Element("from").Elements().FirstOrDefault(e => e.Attribute("as").Value == queryList[i - 1].Attribute("as").Value);

                    int keyIndex = 1;
                    foreach (XElement keyCol in nextQuery.Element("select").Elements().Where(e => getAttrValue(e, "key") == "1"))
                    {
                        XElement selCol = new XElement("column",
                             new XAttribute("table", nextQuery.Attribute("as").Value),
                             new XAttribute("column", keyCol.Attribute("as").Value),
                             new XAttribute("as", "key" + keyIndex.ToString())
                            );

                        query1.Element("select").Add(selCol);
                        keyIndex++;
                    }
                    //if (nextQuery != null)
                    //{
                    //    nextQuery.SetAttributeValue("fixed", "1");
                    //}
                }

                if (!pushed)
                {

                    changeChildAliases(query1, "_x" + i.ToString());




                }



                if (parentQuery != null)
                {

                    // XElement thisQueryInParent = parentQuery.Elements("from").Elements("query").Where(e => e.Attribute("as").Value == alias).FirstOrDefault();

                    // parentQuery.SetAttributeValue("hint", "materialize");
                    parentQuery.SetAttributeValue("materialize", "1");
                    string newParentAlias = "prt" + i.ToString(); //parentQuery.Attribute("as").Value;
                    //XElement queryExpr=new XElement("query",new XAttribute("as",newParentAlias),new XAttribute("join","inner"),

                    //    new XElement("select",
                    //        new XElement("const",new XAttribute("as","a"),new XText("1"))
                    //        ),
                    //        new XElement("from", parentQuery)
                    //    );

                    XElement queryExpr = parentQuery;
                    queryExpr.SetAttributeValue("as", newParentAlias);
                    queryExpr.Add(new XAttribute("join", "inner"));

                    //  XElement andNodeForExists = extendWhere(query1);
                    //  XElement andNodeForJoin = extendWhere(queryExpr);

                    if (!pushed)
                    {
                        XElement joinExpr = new XElement("call", new XAttribute("function", "and"));

                        int keyIndex = 1;
                        foreach (XElement joinCol in query1.Element("select").Elements().Where(e => getAttrValue(e, "key") == "1"))
                        {
                            XElement joinCol1 = parentQuery.Element("select").Elements().First(e => getAttrValue(e, "as") == "key" + keyIndex);

                            XElement condExpr2 = new XElement("call", new XAttribute("function", "=")


                                );
                            // XElement key2 = thisQueryInParent.Element("select").Elements().Where(e => getAttrValue(e, "as") == joinCol.Attribute("as").Value).First();
                            condExpr2.Add(new XElement("column", new XAttribute("table", joinCol.Attribute("table").Value), new XAttribute("column", joinCol.Attribute("column").Value)));
                            //condExpr2.Add(new XElement("column", new XAttribute("table", parentQuery.Attribute("as").Value), new XAttribute("column", joinCol1.Attribute("as").Value)));
                            condExpr2.Add(new XElement("column", new XAttribute("table", newParentAlias), new XAttribute("column", joinCol1.Attribute("as").Value)));
                            joinExpr.Add(condExpr2);
                            keyIndex++;
                        }
                        queryExpr.Add(
                            joinExpr
                            );

                    }
                    else
                    {
                        XElement andNodeForJoin = extendWhereByAnd(queryExpr);
                        andNodeForJoin.Add(thisQueryInParent.Elements("call")); //  !!! Не проверено , скорее всего сломал
                    }
                    //XElement condExpr = new XElement("call", new XAttribute("function", "exists"),
                    //     queryExpr
                    // );
                    //andNodeForExists.Add(condExpr);

                    query1.Element("from").Add(queryExpr);

                }



                // query1.Descendants().Attributes("materialize").Remove();

                query1.Descendants("text").Where(e => getAttrValue(e, "txtype") == "func").Remove();
                doMatrializeByHint = false;

                query1 = compileQuery(query1, true, null).Elements().First();
                query1.SetAttributeValue("name", "rest" + i.ToString());
                query1.SetAttributeValue("noname", "1");
                doMatrializeByHint = true;
                parentQuery = query1;

                //VReport rep = XmlReports.environment.GetReport(query1);
                ////  rep.SetAttributeValue("materialize", "1");
                //VDataSet ds = rep.Result();
                //ds.Tables[0].PrimaryKey = null;
                //ds.Refresh(false);

                query1.SetAttributeValue("as", alias);

            }




            return query1;
        }
        private static void addClassTitles(XElement element)
        {
        }
        //
        private static string[] grFuncsNamesNative = new string[] { "sum", "min", "max", "count", "avg" };
        internal static string[] grFuncsNames = new string[] { "sum", "min", "max", "count", "count_dist", "avg", "sumnvl", "stragg", "stragg_dist" };
        // lj
        internal static string[] aggFuncsNames = new string[] { "sum", "min", "max", "count", "count_dist", "avg", "sumnvl", "stragg", "stragg_dist", "inherit", TextConst.AVGroup.Inner, TextConst.AVGroup.List, TextConst.AVGroup.Outer, "no"/*добавил , но может нельзя*/, "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" };
        //internal static string[] aggFuncsNamesNum = new string[] { "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12", "13", "14" };
        //
        private static void preProcessingIGroup(XElement root)
        {

            foreach (XElement field in root.DescendantsAndSelf("query").Elements("select").Elements().Where(e => e.Attribute("group") != null && !Cmn.IsNumeric(e.Attribute("group").Value) && !aggFuncsNames.Contains(e.Attribute("group").Value) && !e.Attribute("group").Value.Contains(".")))
            {
                XElement sel = field.Ancestors("query").First().Elements("select").FirstOrDefault();
                if (sel != null)
                {
                    //XElement master = sel.Elements().FirstOrDefault(e => e.Attribute("as").Value == field.Attribute("group").Value);
                    //if (master != null)
                    //{
                    foreach (XElement field1 in sel.Elements())
                    {

                       field1.SetAttributeValue("removeable", "1");

                        if (Cmn.IsNumeric(getAttrValue(field1, TextConst.AName.Group)))
                        {
                            field1.SetAttributeValue(TextConst.AName.CMasterKey, field1.Attribute(TextConst.AName.As).Value);
                        }
                    }

                    field.SetAttributeValue("master", field.Attribute("group").Value);
                    field.SetAttributeValue(TextConst.AName.CMaster, field.Attribute(TextConst.AName.Master).Value);
                    field.SetAttributeValue("group", "max");
                    //}
                }
            }




        }
        private static void processingExtentions(XElement query)
        {
            if (query.Attribute("extend") != null)
            {
                //if (query.Attribute("extend").Value == "ur_mat")
                //{

                //}
                XmlReports.FindAndExtentdQuery(query, query.Parent);
                query.Remove();
            }
            
        }
        private static void processingMoveLinks(XElement query)
        {
            if (query.Attribute(AName.extend) == null && query.Attribute(AName.inherit) == null) {
                if (query.Element(EName.links) != null) {
                    XElement mainSource = query.Elements(EName.from).Elements(EName.table).FirstOrDefault();
                    if (mainSource != null) {
                        IList<XElement> links = query.Elements(EName.links).ToList();
                        links.Remove();
                        mainSource.Add(links.Elements());
                    }
                }
            }
        }
        private static void processingAddSelfNames(XElement query, bool isOld)
        {
            // return;
            if (isOld) return;
            //!!! Последить за скоростью
            VQuery qry = null;
            if (query.Attribute(TextConst.AName.Name) != null)
            {

                qry = XmlReports.Environment.GetQuery(query.Attribute(TextConst.AName.Name).Value);
                if (qry != null)
                {
                    if (qry.IsExtNameColumn())// Своя колонка с именем
                    {
                        query.Element(TextConst.EName.Select).Add(new XElement(qry.NameColumn()));
                    }
                }

            }
        }
        private static void processingAddNames(XElement query)
        {
            //!!! Последить за скоростью
            VQuery qry = null;


            if (query.Attribute(TextConst.AName.AddNames) != null)// Имена по ссылкам
            {

                qry = XmlReports.Environment.GetQuery(query.Attribute(TextConst.AName.Name).Value);
                var newCols = qry.CreateNameColumns();
                if (newCols.Count != 0)
                {
                    query.Element(TextConst.EName.Select).Add(newCols);

                    processingQuickLinks(query);
                    query.Attribute(TextConst.AName.AddNames).Remove();
                }
                // Временное решения для вычисления заголовков, потом убрать старый вариант
                foreach (XElement el in query.Elements(TextConst.EName.Select).Elements())
                {
                    if (el.Attribute(TextConst.AName.As) != null)
                    {
                        var col = qry.SearchColumn(el.Attribute(TextConst.AName.As).Value);
                        if (col != null)
                        {
                            el.SetAttributeValue(TextConst.AName.Title, col.P_Title);
                        }
                    }
                }


            }
        }
        private static List<XElement> CollectDirectLinks(XElement element)
        {
            List<XElement> list = new List<XElement>();
            list.Add(element);
            CollectDirectLinksNext(element, list);
            return list;
        }
        private static void CollectDirectLinksNext(XElement element, List<XElement> list)
        {
            foreach (XElement link in element.Elements()) {
                XName name = link.Name;
                if (name == EName.link || name == EName.elink || name == EName.slink) {
                    list.Add(link);
                    CollectDirectLinksNext(link, list);
                }
            }
        }
        private static void ProcessQueryDlinkConditions(XElement query)
        {
            foreach (XElement dlink in query.Descendants(EName.dlink).ToList()) {
                XElement query1 = dlink.Ancestors(EName.query).First().Ancestors(EName.query).FirstOrDefault();
                if (query1 != null) {
                    ProcessDlinkConditions(query1, dlink);
                }
            }
        }
        private static void ProcessDlinkConditions(XElement query, XElement dlink)
        {
            var list = CollectDirectLinks(dlink);

            var names = list.Attributes(TextConst.AName.As).Select(e => e.Value).Distinct().ToList();

            var cols = query.Elements(TextConst.EName.Where).Descendants(TextConst.EName.Column)
                .Where(e => names.Contains(getAttrValue(e, TextConst.AName.Table))
                && e.Attribute(TextConst.AName.Dgroup) == null).ToList();

            if (cols.Count == 0) return;
            //   XElement callAnd = new XElement(TextConst.EName.Call,new XAttribute(TextConst.AName.Function,TextConst.AVFunc.And));


            var processdCols = new List<XElement>();
            int index = 1;
            foreach (XElement col in cols)
            {
                if (!processdCols.Contains(col))
                {
                    XElement dlinkCopy = new XElement(dlink);
                    dlinkCopy.SetAttributeValue(TextConst.AName.Pushpred, TextConst.AVBool.True);
                    dlink.AddAfterSelf(dlinkCopy);
                    var links = CollectDirectLinks(dlinkCopy);
                    var orw = col.Ancestors(
                       ).First(e => e.Name.LocalName == TextConst.EName.Where ||
                       (new string[] { TextConst.AVFunction.Or, TextConst.AVFunction.And }).Contains(getAttrValue(e, TextConst.AName.Function)));
                    var parentCall = col.Ancestors().First(e => e.Parent == orw);
                    foreach (XElement linkCopy in links)
                    {
                        string newAlias = getAttrValue(linkCopy, TextConst.AName.As) + TextConst.Pfx.DubDlinkCond + index;


                        var localCols = parentCall.Descendants(TextConst.EName.Column).Where(e => getAttrValue(e, TextConst.AName.Table) == getAttrValue(linkCopy, TextConst.AName.As)).ToList();
                        var localCols1 = dlinkCopy.Descendants(TextConst.EName.Column).Where(e => getAttrValue(e, TextConst.AName.Table) == getAttrValue(linkCopy, TextConst.AName.As)).ToList();
                        linkCopy.SetAttributeValue(TextConst.AName.As, newAlias);
                        processdCols.AddRange(localCols);

                        foreach (XElement localCol in localCols)
                        {
                            localCol.SetAttributeValue(TextConst.AName.Table, newAlias);
                        }



                        foreach (XElement localCol in localCols1)
                        {
                            localCol.SetAttributeValue(TextConst.AName.Table, newAlias);
                        }
                    }







                    var newCond = new XElement(TextConst.EName.Call
                        , new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Exists)
                        , new XElement(TextConst.EName.Column
                            , new XAttribute(TextConst.AName.Table, col.Attribute(TextConst.AName.Table).Value)
                            , new XAttribute(TextConst.AName.Column, col.Attribute(TextConst.AName.Column).Value)
                              , new XAttribute(TextConst.AName.Dgroup, TextConst.AVGroup.Empty)

                            )
                        );


                    copyAttribute(parentCall, newCond, TextConst.AName.Optional);

                    index++;
                    parentCall.Attributes(TextConst.AName.Optional).Remove();
                    parentCall.ReplaceWith(newCond);

                    // callAnd.Add(parentCall);
                    var tagParent = dlinkCopy.Elements(TextConst.EName.Where).FirstOrDefault();

                    if (tagParent == null)
                    {
                        tagParent = new XElement(TextConst.EName.Where);
                        dlinkCopy.Add(tagParent);
                    }
                    else
                    {
                        var tagParent1 = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.And));
                        var els = tagParent.Elements().ToList();
                        els.Remove();
                        tagParent1.Add(els);
                        tagParent.Add(tagParent1);
                        tagParent = tagParent1;
                    }
                    tagParent.Add(parentCall);
                }
            }
        }
    }
}