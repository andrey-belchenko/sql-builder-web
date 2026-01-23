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
//using System.Windows.Forms;
//using DevExpress.XtraPrinting;
using sql.builder.XmlHelpers;
using sql.builder.Exceptions;

namespace sql.builder.DataApi
{
    internal partial class VQubeUtils
    {

        public static string unName = "un";
        public static string qubeName = TextConst.Pfx.QubeQueryAlias;
        public static string dimQryName = "dims";
        public static XElement GetQubeInfo(VQuery query)
        {
            XElement info = new XElement(TextConst.EName.Root);
            CreateQubeQuery(query, null, info);
            return info;
        }

        public static bool ApplyOuterExpr(VFact fact, int level=1)
        {
            var f = fact;
            var fsrc = f.GetFactSource();
            bool hasOuterExps = false;
            if (fsrc is VExpression)
            {
                if (fsrc.P_AggregationS == TextConst.AVGroup.Outer)
                {
                    var expNew = new XElement(fsrc.AsXElementApplyingParts());

                    expNew.CopyAttributes(f.Attributes());
                    expNew.RemoveAttribute(AName.table);
                    
                    if (f.P_Group == "")
                    {
                        expNew.Attributes(TextConst.AName.Group).Remove();
                    }
                    if (f.P_Table != "")
                    {
                        foreach (var f1 in expNew.Descendants(TextConst.AName.Fact).ToArray())
                        {
                            f1.SetAttributeValue(TextConst.AName.Table, f.P_Table);
                        }
                    }
                    VSXElement vexpNew = VSXElement.Get(new XElement(expNew));
                    vexpNew.VirtualParent = fact.GetParent();
                    foreach (VFact f1 in vexpNew.Descendants(EName.fact).ToList().SelectAsArray(VSXElement.Get<VFact>)) {
                        var r = ApplyOuterExpr(f1, level + 1);
                    }
                    f.ReplaceWith(vexpNew);
                    hasOuterExps = true;
                }
            }
            return hasOuterExps;
        }
        public static XElement CreateQubeQuery(VQuery query, XElement rep, XElement qubeInfo = null)
        {
            if (qubeInfo == null) CacheQubeIfNeed(query);
           
            var xquery = Compiler.copyThisColumns(query);
            var query1 = VSXElement.Get<VQuery>(new XElement(xquery));
            //query1.environment = query.environment;
            query = query1;
            var qubeElement = query.GetQubeElement();

            if (qubeElement == null)
            {
                query = recombine(query);
                qubeElement = query.GetQubeElement();
            }


            if (qubeElement.FactLinks().Any())
            {
                foreach (VLink factLink in qubeElement.FactLinks())
                {
                    var cqry = factLink.Query();
                    var xqc = new XElement(TextConst.EName.Query);

                    xqc.SetAttributeValue(TextConst.AName.Name, cqry.P_IdName);
                    xqc.SetAttributeValue(TextConst.AName.As, factLink.XName);
                    xqc.SetAttributeValue(TextConst.AName.Join, TextConst.AVJoin.LeftOuter);

                    var xcall = new XElement(TextConst.EName.Call);
                    xcall.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Equal);
                    xqc.Add(xcall);

                    var xcol = new XElement(TextConst.EName.Column);
                    xcol.SetAttributeValue(TextConst.AName.Table, factLink.XName);
                    xcol.SetAttributeValue(TextConst.AName.Column, cqry.KeyColumn().XName);
                    xcall.Add(xcol);

                    var xfact = new XElement(TextConst.EName.Fact);

                    xfact.SetAttributeValue(TextConst.AName.Column, factLink.P_CalledQuery);
                    xcall.Add(xfact);
                    qubeElement.AddAfterSelf(xqc);


                }
                qubeElement.Elements(TextConst.EName.Factlinks).Remove();
            }

            var facts1 = query.AllUsedFacts().Distinct().ToList();
            bool hasOuterExps = false;
            foreach (var f in facts1)
            {
               var r= ApplyOuterExpr(f);
               if (r)
               {
                   hasOuterExps = true;
               }
            }

            if (hasOuterExps)
            {
                facts1 = null;
            }
           


            


            if (qubeElement.P_MergeDimsets == TextConst.AVBool.True)
            {

                foreach (var ds in qubeElement.DimSets())
                {
                    if (ds.Element(TextConst.EName.Where) == null)
                    {
                        ds.Add(new XElement(TextConst.EName.Where), new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.True)));
                    }
                }
            }


            var outputLinkNames = new List<string>();


            
           
            //var factSources = new SortedList<string, VSXElement>();


            var allLinks = qubeElement.AllQubeLinks();

            bool changes = false;


            bool autoMerge = false;
           
            if (rep != null)
            {
                
                if (Cmn.GetAttrValue(rep, TextConst.AName.AutoMerge) == TextConst.AVBool.True)
                {
                    autoMerge = true;
                    
                }
            }
            var keysOfDims = new SortedList<string,string>();
            var mergerKeyAliasMergeKeyInQube = new SortedList<string, string>();

            var usedLinks = new List<VQueryCall>();
            var usedInSelectLinks = new List<VQueryCall>();
            var notUsedLinks = new SortedList<string,List<VQueryCall>>();
            var notUsedInSelectLinks = new SortedList<string, List<VQueryCall>>();
            var optionalDimensions = new SortedList<string,List<string>>();
           // var specFactsForOptionalDimensions = new HashSet<string>();

            var xqe = qubeElement.AsXElementApplyingParts();
            var usdtabnamesInWhere = xqe.Descendants(TextConst.EName.Where).Descendants(TextConst.EName.Column).Attributes(TextConst.AName.Table).Select(a=>a.Value).ToList();
            if (usdtabnamesInWhere.Contains(TextConst.AVTable.Ths))
            {
                usdtabnamesInWhere.Remove(TextConst.AVTable.Ths);
            }

            HashSet<string> tabnames = new HashSet<string>();

            var dimSetsWithFacts = new HashSet<string>();

            foreach (VDimSet ds in qubeElement.DimSets())
            {
                if (!dimSetsWithFacts.Contains(ds.P_Alias))
                {
                    if (ds.GetFacts().Any())
                    {

                        dimSetsWithFacts.Add(ds.P_Alias);

                    }
                    else
                    {
                        //считаем, что димсет с фактами, если есть link с признаком allRows и колонки из него используются, предется повторить тоже самое что ниже, наверняка можно упростить

                        var dsLinks = qubeElement.GetDimsetLinks(ds.P_Alias).Where(dsl => dsl.P_AllRows == TextConst.AVBool.True).ToArray();


                        foreach (VQueryCall link in dsLinks)
                        {

                            var allChilds = link.AllLinks(null);
                            allChilds.Add(link);
                            bool found = false;
                            foreach (VQueryCall cl in allChilds)
                            {
                                if (!(cl is VDimLink))
                                {
                                    var usedColumns = cl.UsedColumns();
                                    var usedColumnsNotDs = usedColumns.Where(c => !c.Ancestors(TextConst.EName.DimSet).Any());
                                    if (usedColumnsNotDs.Any())
                                    {
                                        dimSetsWithFacts.Add(ds.P_Alias);
                                         found = true;
                                        break;
                                    }
                                }
                            }
                            if (found)
                            {
                                break;
                            }


                        }

                    }
                }
            }
            foreach (VQueryCall link in allLinks.ToArray())
            {
                tabnames.Add(link.XName);
                var allChilds = link.AllLinks(null);
                allChilds.Add(link);
                bool hasUses = false;
                bool hasUsesInSelect = false;

                foreach (VQueryCall cl in allChilds)
                {
                    tabnames.Add(cl.XName);
                }
                var dimSet = (link.GetParent() as VDimSet);
                //bool dimSetHasFacts = false;
                string ds1name=null;
                if (dimSet != null)
                {
                    ds1name=dimSet.P_Alias;
                    //dimSetHasFacts = dimSet.GetFacts().Any();
                }
                foreach (VQueryCall cl in allChilds)
                {
                    if (!(cl is VDimLink))
                    {
                        var usedColumns = cl.UsedColumns();
                        var usedColumnsNotDs = usedColumns.Where(c => !c.Ancestors(TextConst.EName.DimSet).Any());
                        if (usedColumnsNotDs.Any() && dimSet == null)
                        {
                            hasUses = true;
                        }
                        else
                        {
                            foreach (var col in usedColumns)
                            {
                                var ds = col.Ancestors(TextConst.EName.DimSet).FirstOrDefault();
                                if (ds != null)
                                {
                                    var dsname = Cmn.GetAttrValue(ds, TextConst.AName.As);
                                    if (dimSetsWithFacts.Contains(dsname) && ((dsname == ds1name) || (dimSet == null)))
                                    {
                                        hasUses = true;
                                        break;
                                    }
                                }
                            }

                            if (!hasUses)
                            {
                                if (usedColumnsNotDs.Any())
                                {
                                    if (link.P_AllRows == TextConst.AVBool.True || dimSetsWithFacts.Contains(ds1name))
                                    {
                                        hasUses = true;
                                    }
                                }
                            }



                        }
                        //if (usedColumns.Any())
                        //{

                        //    if (dimSet != null)// есть факты, или используется признак allRows и есть используемая колонка, под where в dimset не считается
                        //    {
                        //        if (dimSetHasFacts)
                        //        {
                        //            hasUses = true;
                        //        }
                        //        else
                        //        {
                        //            if (link.P_AllRows == TextConst.AVBool.True)
                        //            {
                        //                foreach (var col in usedColumns)
                        //                {
                        //                    if (!col.Ancestors(TextConst.EName.DimSet).Any())
                        //                    {
                        //                        hasUses = true;
                        //                    }
                        //                }
                        //            }
                        //        }
                        //    }
                        //    else
                        //    {

                        //        hasUses = true;
                        //    }




                        //}

                        if (hasUses)
                        {
                            if (!autoMerge)
                            {


                                break;

                            }
                            else
                            {
                                if (usedColumns.Any(c => c.GetAncestorsAndSelf(EName.select).Count != 0)) {
                                    hasUsesInSelect = true;
                                    break;
                                }
                            }
                        }
                    }
                    
                }
                var name = link.P_CalledQuery;
                if (!hasUses)
                {
             
                    if (!notUsedLinks.ContainsKey(name))
                    {
                        notUsedLinks.Add(name, new List<VQueryCall>());
                    }
                    notUsedLinks[name].Add(link);
                }
                else
                {
                    usedLinks.Add(link);
                    
                }
                if (autoMerge)
                {
                    if (hasUsesInSelect)
                    {
                        usedInSelectLinks.Add(link);
                    }
                    else
                    {
                        if (!notUsedInSelectLinks.ContainsKey(name))
                        {
                            notUsedInSelectLinks.Add(name, new List<VQueryCall>());
                        }
                        notUsedInSelectLinks[name].Add(link);
                    }
                }
            }

            foreach (var tn in usdtabnamesInWhere)
            {
                if (!tabnames.Contains(tn))
                {
                    var ni = xqe.Descendants(TextConst.EName.Where).Descendants(TextConst.EName.Column).First(e => Cmn.GetAttrValue(e, TextConst.AName.Table) == tn);
                    var ecol = query.Descendants(TextConst.EName.Column).Where(e => Cmn.GetAttrValue(e, TextConst.EName.Table) == tn).FirstOrDefault();

                    var scondSrc = "";
                    if (ecol != null)
                    {
                        var csrcInfoAtr = ecol.Ancestors().Attributes(TextConst.AName.CondSource).FirstOrDefault();
                        if (csrcInfoAtr != null)
                        {
                            scondSrc = ". Условие перенесено из запроса " + csrcInfoAtr.Value;
                        }
                    }
                    
                    throw new VCompilerException("Не найден источник данных " + tn + " в запросе " + query.XName+scondSrc, query, ni);
                }
            }

            if (autoMerge)
            {
                foreach (VQueryCall link in notUsedInSelectLinks.SelectMany(e => e.Value))
                {
                    link.Attributes(TextConst.AName.AllRows).Remove();
                   
                    changes = true;
                }
                foreach (VQueryCall link in usedInSelectLinks.ToArray())
                {
                    var extraLinks = link.GetDescedantsP(EName.dimlink);
                    foreach (VDimLink dl in extraLinks)
                    {
                        var name = dl.P_CalledQuery;
                        if (notUsedLinks.ContainsKey(name))
                        {
                            foreach (var l in notUsedLinks[name].ToArray())
                            {
                                notUsedLinks[name].Remove(l);
                                usedLinks.Add(l);
                            }
                        }
                        if (notUsedInSelectLinks.ContainsKey(name))
                        {
                            foreach (var l in notUsedInSelectLinks[name].ToArray())
                            {
                                notUsedInSelectLinks[name].Remove(l);
                                usedInSelectLinks.Add(l);
                                if (!optionalDimensions.ContainsKey(name))
                                {
                                    optionalDimensions.Add(name, new List<string>());
                                }
                                
                                //ur_mat_isp_kod_mat_isp
                            }
                        }

                        if (optionalDimensions.ContainsKey(name))
                        {
                            //var ftn = "ur_mat_isp_kod_mat_isp";
                            //if (!optionalDimensions[name].Contains(ftn+"1"))
                            //{
                            //    optionalDimensions[name].Add(ftn+"1");
                            //    optionalDimensions[name].Add(link.P_CalledQuery);
                            //}
                            //if (!specFactsForOptionalDimensions.Contains(ftn))
                            //{
                            //    specFactsForOptionalDimensions.Add(ftn);
                            //    var specFact = new XElement(TextConst.AName.Fact);
                            //    specFact.SetAttributeValue(TextConst.AName.Column, ftn);
                            //    specFact.SetAttributeValue(TextConst.AName.As, ftn);
                            //    specFact.SetAttributeValue(TextConst.AName.Removeable, TextConst.AVBool.False);
                            //    query.Element(TextConst.EName.Select).Add(specFact);
                            //}
                            if (!optionalDimensions[name].Contains(link.P_CalledQuery))
                            {
                                optionalDimensions[name].Add(link.P_CalledQuery);
                                //optionalDimensions[name].Add(link.P_CalledQuery);
                            }
                        }

                    }
                }
                foreach (VQueryCall link in usedInSelectLinks) {
                    if (!keysOfDims.ContainsKey(link.XName) && link.P_OnlyForCond != TextConst.AVBool.True) {
                        string keyName = link.Query().KeyColumn().XName;
                        //var keyCol =  cols.Where(c => c.P_Column == keyName).FirstOrDefault();
                        //if (keyCol == null)
                        //{
                        string keyAlias = TextConst.Pfx.Id + link.XName;
                        XElement xkeyCol = Factory.NewColumn(link.XName, keyName);
                        xkeyCol.Add(new XAttribute(AName.@as, keyAlias));
                        xkeyCol.Add(new XAttribute(AName.parname, keyAlias));
                        xkeyCol.Add(new XAttribute(AName.removeable, TextConst.AVBool.False));
                        query.Element(EName.select).AddFirst(xkeyCol);
                        VColumn keyCol = VSXElement.Get<VColumn>(xkeyCol);
                        //}
                        mergerKeyAliasMergeKeyInQube.Add(keyAlias, link.P_CalledQuery);
                        keysOfDims.Add(link.XName, keyAlias);
                    }
                }
            }

            foreach (VQueryCall link in notUsedLinks.SelectMany(e => e.Value))
            {
                link.Remove();
                allLinks.Remove(link);
                changes = true;
            }

            //foreach (VQueryCall link in allLinks.ToArray())
            //{
            //    //if (link.P_AllRows != TextConst.AVBool.True) // !!! Убрал, возможно будут ошибки
            //    //{
            //        var allChilds = link.AllLinks(null);
            //        allChilds.Add(link);
            //        bool hasUses = false;
            //        bool hasUsesInSelect = false;
            //        foreach (VQueryCall cl in allChilds)
            //        {
            //            var usedColumns=cl.UsedColumns();
            //            if (usedColumns.Any())
            //            {
            //                hasUses = true;
            //                if (!autoMerge)
            //                {
            //                    break;
            //                }
            //                else
            //                {
            //                    if (usedColumns.Where(c => c.GetAncestorsAndSelf(TextConst.EName.Select).Any()).Any())
            //                    {
            //                        hasUsesInSelect = true;
            //                        break;
            //                    }
            //                }
            //            }
            //        }
            //        if (!hasUses)
            //        {
            //            link.Remove();
            //            allLinks.Remove(link);
            //            changes = true;
            //        }
            //        else
            //        {

            //            if (autoMerge && hasUsesInSelect && !keysOfDims.ContainsKey(link.XName) && link.P_OnlyForCond != TextConst.AVBool.True)
            //            {
                          
                            
            //                var keyName = link.Query().KeyColumn().XName;
            //                //var keyCol =  cols.Where(c => c.P_Column == keyName).FirstOrDefault();

            //                //if (keyCol == null)
            //                //{
            //                    var xkeyCol = new XElement(TextConst.EName.Column);
            //                    xkeyCol.SetAttributeValue(TextConst.AName.Table, link.XName);
            //                    xkeyCol.SetAttributeValue(TextConst.AName.Column, keyName);
            //                    var keyAlias = TextConst.Pfx.Id + link.XName;
            //                    xkeyCol.SetAttributeValue(TextConst.AName.As, keyAlias);
            //                    xkeyCol.SetAttributeValue(TextConst.AName.Removeable, TextConst.AVBool.False);
            //                    query.Element(TextConst.EName.Select).AddFirst(xkeyCol);
            //                    var keyCol = (VColumn)VSXElement.Get(xkeyCol);
            //                //}
            //                    mergerKeyAliasMergeKeyInQube.Add (keyAlias, link.P_CalledQuery);
            //                    keysOfDims.Add(link.XName, keyAlias);

            //            }
            //        }
            //    //}

            //}


            if (autoMerge)
            {
                var mergeKeysForDimset = new SortedList<string, List<string>>();
                var sects = query.GetAllSectionsContainingColumnsSW();
                var cols = sects.SelectMany(s => s.GetDescedantsP(EPredicate.IsColumnOrFact));
                foreach (VSXElement el in cols)
                {
                    var mergeKey = new List<string>();
                    if (el.Name.LocalName == TextConst.EName.Column)
                    {
                        var col = el as VColumn;
                        var src = col.Source();
                        var dl = src.GetDimensionLink();
                        if (dl != null)
                        {
                            mergeKey.Add(keysOfDims[dl.XName]);
                        }
                    }
                    else if (el.Name.LocalName == TextConst.EName.Fact)
                    {
                        
                        var dimsetName = el.P_Table;
                        if (!mergeKeysForDimset.ContainsKey(dimsetName))
                        {
                            foreach (var link in qubeElement.Links(null))
                            {
                                if (link.P_OnlyForCond != TextConst.AVBool.True && keysOfDims.ContainsKey(link.XName))
                                {
                                    mergeKey.Add(keysOfDims[link.XName]);
                                }
                            }
                            if (dimsetName != "")
                            {
                                foreach (var link in qubeElement.GetDimsetLinks(dimsetName))
                                {
                                    if (link.P_OnlyForCond != TextConst.AVBool.True && keysOfDims.ContainsKey(link.XName))
                                    {
                                        if (!mergeKey.Contains(keysOfDims[link.XName]))
                                        {
                                            mergeKey.Add(keysOfDims[link.XName]);
                                        }
                                    }
                                }
                            }
                            mergeKeysForDimset[dimsetName] = mergeKey;
                        }
                        else
                        {

                            mergeKey = mergeKeysForDimset[dimsetName];
                        }
                        
                    }
                    //доделать для выражений
                    var smergeKey = string.Join(",", mergeKey);

                    //if (!specFactsForOptionalDimensions.Contains(el.XName))
                    //{
                        el.SetAttributeValue(TextConst.AName.MergeKey, smergeKey);
                    //}
                    
                    
                }
               // var mergeOrder =  string.Join(",", keysOfDims.Values);
                var mergeOrder = "";
                var qq = "";
                var addedMK = new HashSet<string>();
                foreach (var l in qubeElement.AllQubeLinks()) // важен порядок
                {
                    if (keysOfDims.ContainsKey(l.XName) && !addedMK.Contains(l.XName) && l.P_OnlyForCond!=TextConst.AVBool.True)
                    {
                        addedMK.Add(l.XName);
                        mergeOrder += qq + keysOfDims[l.XName];
                        qq = ",";
                    }
                }
                //var qq = "";
                //var mergeOrder = "";// string.Join(",", keysOfDims.Values);
                //for (int i = 0; i < keysOfDims.Count; i++)  // реализация чего-то типа order siblings, м. быть можно проще
                //{
                  
                //    var mo1 = "";

                //    if (i == keysOfDims.Count - 1)
                //    {
                //        mo1 = keysOfDims.ElementAt(i).Value;
                //    }
                //    else
                //    {
                //        mo1 = TextConst.AVFunc.Coalesce + "(";
                //        var qq1 = "";
                //        for (int j = i ; j < keysOfDims.Count; j++)
                //        {
                //            mo1 += qq1 + keysOfDims.ElementAt(j).Value;
                //            qq1 = ",";
                //        }
                //        mo1 += ")";
                //    }
                //    mergeOrder += qq + mo1;
                //    qq = ",";
                //}
                query.SetAttributeValue(TextConst.AName.Order, mergeOrder); // если уже есть сортировка , она затирается, дугого варианта не вижу.
            }

            //if (changes)  // не работает, вроде из-за кеширования
            //{
            //    allLinks = qubeElement.AllQubeLinks();
            //}
            ///!!!!!!!!!!!!!!!
            ///
          
            if (facts1 == null)
            {
                facts1 = query.AllUsedFacts().Distinct().ToList();
            }

            var facts = new Dictionary<int, VFact>();

            foreach (VFact f in facts1)
            {
                facts.Add(f.GetUniqueKey(), f);
            }



            var links = qubeElement.Links(null);



            VQuery allRowsQuery = null;
            var outerDimensionNames = new SortedList<string, string>();
            var extOuterDimensionNames = new SortedList<string, string>();

            var innerDimensions = new List<string>();
            var innerOnlyDimensions = new List<string>();
            //  string mainDimension = "";
            foreach (VQueryCall link in links)
            {

                if (link.P_OnlyForCond != TextConst.AVBool.True)
                {
                    outerDimensionNames.Add(link.P_CalledQuery, link.XName);
                }

                //if (Cmn.GetAttrValue(link, TextConst.AName.AllRows) == TextConst.AVBool.True)
                //{
                //    allRowsQuery = link.GetEnvironment().GetQueryByKeyDimensionName(link.P_CalledQuery);
                //}
            }


            var allLinksPre = allLinks.ToList();

            foreach (VQueryCall link in allLinks.ToArray())
            {
                var allChilds = link.AllLinks(null);
                allChilds.Add(link);
                foreach (VQueryCall cl in allChilds)
                {
                    if (!(cl is VDimLink))
                    {
                        if (cl.UsedColumns().Where(e => !e.Ancestors(TextConst.EName.Qube).Any()).Any())
                        {
                            outputLinkNames.Add(link.P_CalledQuery);
                            break;
                        }
                    }
                }
                if (!extOuterDimensionNames.ContainsKey(link.P_CalledQuery))
                {
                    extOuterDimensionNames.Add(link.P_CalledQuery, link.XName);
                }
                else
                {
                    allLinks.Remove(link);
                }


            }



            var dimNamesByFact = new SortedList<string, List<string>>();

            var dimsetWhereDimensions = new SortedList<string, string>();
            //var factFactUse = new SortedList<string, VSXElement>();
            var cumulateDimNamesByFact = new SortedList<string, List<string>>();
            var allCumulateDimeNames = new List<string>();


            var factInfoList_N = new SortedList<string, VFact.FactDependantceInfo>();

            var names = new SortedList<string, int>();
            var namesOuter = new SortedList<string, int>();
            var factsExpressios_N = new SortedList<string, XElement>();
            var mainOutputDims_N = new List<string>();
            //var mainNonOutputDims_N = new List<string>();
            var allOutputDims_N = new List<string>();

            foreach (var link in links)
            {
                if (link.P_OnlyForCond == TextConst.AVBool.True)
                {
                    //mainNonOutputDims_N.Add(link.P_CalledQuery);
                }
                else
                {
                    mainOutputDims_N.Add(link.P_CalledQuery);
                }
            }
            foreach (var link in allLinksPre)
            {
                if (link.P_OnlyForCond != TextConst.AVBool.True)
                {
                    if (!allOutputDims_N.Contains(link.P_CalledQuery))
                    {
                        allOutputDims_N.Add(link.P_CalledQuery);
                    }
                }
            }
            
            foreach (var fact1 in facts)
            {
                if (!factsExpressios_N.ContainsKey(fact1.Value.GetFactId()))
                {
                    var alias = fact1.Value.P_Column;
                    if (!namesOuter.ContainsKey(alias))
                    {
                        namesOuter.Add(alias, 0);
                    }
                    namesOuter[alias]++;
                    alias += namesOuter[alias].ToString();
                    var factPars = fact1.Value.GetElementsP(EName.withparams).FirstOrDefault();

                    var factOutputDims_N = mainOutputDims_N.ToList();


                    var conds = new List<string>();
                    if (fact1.Value.P_Table != "")
                    {

                        var dimSet = qubeElement.GetDimSet(fact1.Value.P_Table);

                        if (dimSet == null)
                        {
                            throw new VCompilerException("Не найден набор изменений " + fact1.Value.P_Table, query, fact1.Value);
                        }
                        if (dimSet.Element(TextConst.EName.Where) != null)
                        {
                            var condName = fact1.Value.P_Table + TextConst.Pfx.Dimset;
                            conds.Add(condName);
                        }
                        foreach (VQueryCall link in qubeElement.GetDimsetLinks(fact1.Value.P_Table))
                        {
                            if (link.P_OnlyForCond != TextConst.AVBool.True)
                            {
                                factOutputDims_N.Add(link.P_CalledQuery);
                            }
                        }
                    }

                    var expr = fact1.Value.BuildFullExpression(factPars, factInfoList_N, conds, factOutputDims_N, new List<string>(), names);
                    //throw new InvalidExpressionException("Не указан способ аггрегации для "+fact.Value.P_Column);

                    expr.SetAttributeValue(TextConst.AName.As, alias);

                    factsExpressios_N.Add(fact1.Value.GetFactId(), expr);
                }

            }

           
            foreach (VQueryCall link in allLinksPre.ToArray())
            {
                if (link.P_AllRows == TextConst.AVBool.True)
                {
                     

                    var factInfo = new VFact.FactDependantceInfo("");
                    factInfo.ObjectFact = link.P_CalledQuery;
                    factInfo.OutputDimensions = mainOutputDims_N.ToList();
              
                    var dimSet=link.GetParent() as VDimSet;

                    // !!! Не дописано для allRows из dimset
                    if (dimSet != null)
                    {
                        foreach (var l in qubeElement.GetDimsetLinks(dimSet.XName))
                        {
                            if (l.P_OnlyForCond != TextConst.AVBool.True)
                            {
                                factInfo.OutputDimensions.Add(l.P_CalledQuery);
                            }
                        }
                        if (dimSet.Element(TextConst.EName.Where) != null)
                        {
                            var conds = new List<string>();
                            var condName = dimSet.XName + TextConst.Pfx.Dimset;
                            conds.Add(condName);
                            factInfo.Conditions = conds;
                        }
                    }

                    var id = factInfo.GetInfoId();
                    if (!factInfoList_N.ContainsKey(id))
                    {
                        factInfoList_N.Add(id, factInfo);
                    }
                
                   
                }
            }

            if (factInfoList_N.Count == 0)
            {
                throw new VCompilerException("Запрос к кубу не содержит ни одного факта, следует добавить факты или установить признак \"Все строки\" на одном из измерений",query,null);
            }

            foreach (var info in factInfoList_N)
            {
                foreach (string dimName in info.Value.OutputDimensions)
                {
                    if (!outputLinkNames.Contains(dimName))
                    {
                        if (!innerOnlyDimensions.Contains(dimName))
                        {
                            innerOnlyDimensions.Add(dimName);
                        }
                    }

                    //if (!allOutputDims_N.Contains(dimName))
                    //{
                        if (!innerDimensions.Contains(dimName))
                        {
                            innerDimensions.Add(dimName);
                        }
                    //}
                }

            }

            #region del
            //foreach (VFact fact1 in facts)
            //{


            //    var info = fact1.GetFactDimDependanceInfo(new List<string>());

            //    var cumulDims = fact1.GetCumulateDimensionsNames();
            //    var cumulateInfo = fact1.CumulateInfo();
            //    var tinfo = fact1.P_Table;
            //    if (tinfo != "")
            //    {
            //        tinfo = "|" + tinfo;
            //    }
            //    foreach (var item in info)
            //    {
            //        var fact = query.GetEnvironment().GetFactSource(item.Key);





            //        string factName = item.Key + cumulateInfo + tinfo;



            //        if (!dimNamesByFact.ContainsKey(factName))
            //        {
            //            dimNamesByFact[factName] = new List<string>();
            //            cumulateDimNamesByFact[factName] = new List<string>();


            //            factSources.Add(factName, fact);



            //        }



            //        var dimnames = item.Value.OutputDimensions.ToList();
            //        dimnames.AddRange(cumulDims.ToArray());

            //        foreach (string dimName in dimnames)
            //        {
            //            if (!dimNamesByFact[factName].Contains(dimName))
            //            {
            //                dimNamesByFact[factName].Add(dimName);
            //            }


            //            if (!outputLinkNames.Contains(dimName))
            //            {
            //                if (!innerOnlyDimensions.Contains(dimName))
            //                {
            //                    innerOnlyDimensions.Add(dimName);
            //                }
            //            }

            //            if (!innerDimensions.Contains(dimName))
            //            {
            //                innerDimensions.Add(dimName);
            //            }
            //        }


            //        foreach (string dimName in cumulDims)// ???
            //        {
            //            if (!cumulateDimNamesByFact[factName].Contains(dimName))
            //            {
            //                cumulateDimNamesByFact[factName].Add(dimName);
            //            }
            //            if (!allCumulateDimeNames.Contains(dimName))
            //            {
            //                allCumulateDimeNames.Add(dimName);
            //            }
            //        }

            //        if (fact1.P_Table != "")
            //        {

            //            var dimSet = qubeElement.GetDimSet(fact1.P_Table);
            //            if (dimSet.Element(TextConst.EName.Where) != null)
            //            {
            //                var dimName = fact1.P_Table + TextConst.Pfx.Dimset;
            //                if (!dimNamesByFact[factName].Contains(dimName))
            //                {
            //                    dimNamesByFact[factName].Add(dimName);
            //                }
            //                if (!dimsetWhereDimensions.ContainsKey(dimName))
            //                {
            //                    dimsetWhereDimensions.Add(dimName, fact1.P_Table);
            //                }
            //            }
            //            foreach (VQueryCall link in qubeElement.GetDimsetLinks(fact1.P_Table))
            //            {
            //                var dimName = DimensionNameInfo.BuildName(link.P_CalledQuery, link.P_OnlyForCond);
            //                if (!dimNamesByFact[factName].Contains(dimName))
            //                {
            //                    dimNamesByFact[factName].Add(dimName);
            //                }
            //            }
            //        }
            //    }






            //}
            #endregion
            var dimensionsCollections = new List<DimensionCollection>();

            //if (allRowsQuery != null)
            //{
            //    var dimCollection = new DimensionCollection();
            //    dimensionsCollections.Add(dimCollection);
            //}

            





            foreach (var factId in factInfoList_N.Keys)
            {
                bool exsists = false;

                DimensionCollection dimCollection = null;
                foreach (DimensionCollection dimCollection1 in dimensionsCollections)
                {

                    exsists = true;
                    foreach (string dimName in dimCollection1.Dimensions)
                    {
                        if (!factInfoList_N[factId].OutputDimensions.Contains(dimName))
                        {
                            exsists = false;
                            break;
                        }
                    }

                    foreach (string dimName in factInfoList_N[factId].OutputDimensions)
                    {
                        if (!dimCollection1.Dimensions.Contains(dimName))
                        {
                            exsists = false;
                            break;
                        }
                    }

                    if (exsists)
                    {
                        dimCollection = dimCollection1;
                        break;
                    }
                }
                if (dimCollection == null)
                {
                    dimCollection = new DimensionCollection();
                    
                    foreach (var dim in factInfoList_N[factId].OutputDimensions)
                    {
                        dimCollection.Dimensions.Add(dim);
                        if (!extOuterDimensionNames.ContainsKey(dim))
                        {
                            extOuterDimensionNames.Add(dim, dim);
                        }
                    }
                    dimensionsCollections.Add(dimCollection);
                }
                dimCollection.Facts.Add(factId,factInfoList_N[factId]);
            }
            //

            #region mergeDimCollections

            if (qubeElement.P_MergeDimsets==TextConst.AVBool.True)
            {

                var ftScrQryInfoList = GetSourceQueryInfoList(XmlReports.Environment, dimensionsCollections);
                var dimensionsCollections1 = new List<DimensionCollection>();
                foreach (var qryI in ftScrQryInfoList.Values)
                {
                    var dimCollection = new DimensionCollection();

                    foreach (var dim in qryI.Dimensions)
                    {
                        dimCollection.Dimensions.Add(dim);
                        if (!extOuterDimensionNames.ContainsKey(dim))
                        {
                            extOuterDimensionNames.Add(dim, dim);
                        }
                    }

                    foreach (var fi in qryI.Facts)
                    {
                        dimCollection.Facts.Add(fi.GetInfoId(), fi);
                    }
                 
                    dimensionsCollections1.Add(dimCollection);
                    //dimCollection.Facts.Add(factId, factInfoList_N[factId]);
                }
                dimensionsCollections = dimensionsCollections1;
                var dimColGroups = new List<List<DimensionCollection>>();
                dimensionsCollections1 = dimensionsCollections.ToList();
                foreach (DimensionCollection dimCollection1 in dimensionsCollections.ToArray())
                {
                    if (!dimensionsCollections1.Contains(dimCollection1)) continue;
                    foreach (DimensionCollection dimCollection2 in dimensionsCollections1.ToArray())
                    {
                        if (dimCollection1 == dimCollection2) continue;
                        var equals = true;
                        foreach (string dimName in dimCollection1.Dimensions)
                        {
                            if (!dimCollection2.Dimensions.Contains(dimName) && !outerDimensionNames.ContainsKey(dimName))
                            {
                                equals = false;
                                break;
                            }
                        }
                        foreach (string dimName in dimCollection2.Dimensions)
                        {
                            if (!dimCollection1.Dimensions.Contains(dimName) && !outerDimensionNames.ContainsKey(dimName))
                            {
                                equals = false;
                                break;
                            }
                        }

                        if (equals)
                        {
                            foreach (var ft in dimCollection2.Facts)
                            {
                                dimCollection1.Facts.Add(ft.Key, ft.Value);
                            }
                            dimensionsCollections1.Remove(dimCollection2);
                        }
                    }
                }

                dimensionsCollections = dimensionsCollections1;
            }

            #endregion
            XElement xQubeFullQuery = new XElement(TextConst.EName.Query);
            xQubeFullQuery.Add(new XElement(TextConst.EName.Select));
            xQubeFullQuery.Add(new XElement(TextConst.EName.From));
            xQubeFullQuery.SetAttributeValue(TextConst.AName.As, qubeName);

            var xQubeUnionQuery1 = new XElement(TextConst.EName.Query);
            xQubeUnionQuery1.SetAttributeValue(TextConst.AName.As, qubeName);
            xQubeUnionQuery1.Add(new XElement(TextConst.EName.Select));
            xQubeUnionQuery1.Add(new XElement(TextConst.EName.From));
            xQubeFullQuery.Element(TextConst.EName.From).Add(xQubeUnionQuery1);



            var xQubeUnionQuery = new XElement(TextConst.EName.Query);
            xQubeUnionQuery.SetAttributeValue(TextConst.AName.As, qubeName);

            xQubeUnionQuery1.Element(TextConst.EName.From).Add(xQubeUnionQuery);





            var xQubeUnion = new XElement(TextConst.EName.Union);
            xQubeUnionQuery.Add(xQubeUnion);


            foreach (var link in allLinks)
            {

                var dimName = link.P_CalledQuery;

                if (outputLinkNames.Contains(dimName))
                {
                    var col = new XElement(TextConst.EName.Column);
                    col.SetAttributeValue(TextConst.AName.Table, qubeName);
                    col.SetAttributeValue(TextConst.AName.Column, dimName);
                    col.SetAttributeValue(TextConst.AName.Group, TextConst.AVGroup.Group);
                    xQubeFullQuery.Element(TextConst.EName.Select).Add(col);

                    col = new XElement(col);
                    //if (!autoMerge)
                    //{
                        col.Attribute(TextConst.AName.Group).Remove();
                    //}
                    xQubeUnionQuery1.Element(TextConst.EName.Select).Add(col);
                }


            }



            var addedExpr = new HashSet<string>();
            foreach (var fact in facts)
            {
                var factSrc = fact.Value.GetFactSource();
                XElement expr = factsExpressios_N[fact.Value.GetFactId()];
                if (!addedExpr.Contains(fact.Value.GetFactId()))
                {
                    string gr = factSrc.P_AggregationS;
                    //if (gr == "")
                    //{
                    //    gr = TextConst.AVGroup.Sum;
                    //}

                    //!!! Рисковано нужно все проверять
                    if (gr == "")
                    {
                        throw new VCompilerException("Не указан метод аггрегации", factSrc.GetMainParent(), factSrc);
                    }
                    else if (gr == TextConst.AVGroup.List)
                    {
                        gr = TextConst.AVGroup.Inner;
                      
                    }
                    expr.SetAttributeValue(TextConst.AName.Group, gr);
                    xQubeFullQuery.Element(TextConst.EName.Select).Add(expr);
                    addedExpr.Add(fact.Value.GetFactId());
                }
                fact.Value.P_Column = expr.Attribute(TextConst.AName.As).Value; //  20160729-1
              
            }

            #region del

            //var factIds = new SortedList<string, string>();
            //var factNames = new SortedList<string, int>();
            //foreach (VFact fact in facts.Values.ToList())
            //{
            //    var fname = fact.GetFactSource().P_Fact;
            //    if (!factNames.ContainsKey(fname))
            //    {
            //        factNames.Add(fname, 0);
            //    }
            //    var factid = fact.FullColName();
            //    var factPars = fact.GetElementsApplyingParts(TextConst.EName.WithParams).FirstOrDefault();

            //    if (factPars != null)
            //    {
            //        factid += factPars.ToString();
            //    }
            //    if (!factIds.ContainsKey(factid))
            //    {
            //        factNames[fname]++;


            //        var factSrc = fact.GetFactSource();
            //        XElement expr = null;
            //        string gr = factSrc.P_AggregationS;
            //        if (gr == "")
            //        {
            //            gr = TextConst.AVGroup.Sum;
            //        }
            //        // var factName = fact.FullFactName();




            //       // expr = fact.BuildFullExpressionOld(factPars); !!!!!!!!!!!!!!!!!!!!!!!!!!

            //        var alias = fact.P_Column;
            //        if (factNames[fname] > 1)
            //        {
            //            alias += "_" + factNames[fname].ToString();

            //            fact.P_Column = alias;
            //        }
            //        factIds.Add(factid, alias);
            //        expr.SetAttributeValue(TextConst.AName.As, alias + fact.ColNamePfx());

            //        expr.SetAttributeValue(TextConst.AName.Group, gr);
            //        xQubeFullQuery.Element(TextConst.EName.Select).Add(expr);



            //    }
            //    else
            //    {

            //        if (factNames[fname] > 1)
            //        {

            //            fact.P_Column = factIds[factid];
            //        }
            //    }
            //    //xQubeQuery.Element(TextConst.EName.Select).Add(expr);

            //}
            #endregion
            var xQuery = new XElement(query);
            xQuery.Elements(TextConst.EName.Expressions).Remove();
            var xcolsAll = Compiler.getQueryColumnsWithGr(xQubeFullQuery).ToList();
            // var xcolsAllJoin = Compiler.getQueryJoinColumns(xQubeQuery).ToList();
            //xcolsAll.AddRange(xcolsAllJoin);

            foreach (string name in innerDimensions)
            {
                //if (name == "d49277_fdrid")
                //{

                //}
                var dimension = XmlReports.Environment.GetDimension(name);

                if (innerOnlyDimensions.Contains(name))
                {
                    var col = new XElement(TextConst.EName.Column);
                    col.SetAttributeValue(TextConst.AName.Table, qubeName);
                    col.SetAttributeValue(TextConst.AName.Column, name);
                    //if (autoMerge)
                    //{
                        //col.SetAttributeValue(TextConst.AName.Group, TextConst.AVGroup.Group);
                    //}
                    xQubeUnionQuery1.Element(TextConst.EName.Select).Add(col);
                }



                if (dimension.P_TimeType == "")
                {



                    var dQuery = XmlReports.Environment.GetQueryByKeyDimensionName(name);

                    var dxQuery = new XElement(TextConst.EName.Query);
                    dxQuery.SetAttributeValue(TextConst.AName.Name, dQuery.P_IdName);
                    dxQuery.SetAttributeValue(TextConst.AName.Dimension, name);
                    dxQuery.SetAttributeValue(TextConst.AName.LinkMultiplicatePoint, TextConst.AVBool.True);
                    dxQuery.SetAttributeValue(TextConst.AName.As, name);
                    dxQuery.SetAttributeValue(TextConst.AName.Join, TextConst.AVJoin.LeftOuter);//!!!!!!!!!!!!!!!!!
                    var dxCall = new XElement(TextConst.EName.Call);
                    dxCall.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Equal);

                    var jcol = new XElement(TextConst.EName.Column);
                    jcol.SetAttributeValue(TextConst.EName.Table, name);
                    jcol.SetAttributeValue(TextConst.EName.Column, dQuery.KeyColumn().XName);
                    dxCall.Add(jcol);

                    jcol = new XElement(TextConst.EName.Column);
                    jcol.SetAttributeValue(TextConst.EName.Table, qubeName);
                    jcol.SetAttributeValue(TextConst.EName.Column, name);
                    dxCall.Add(jcol);
                    dxQuery.Add(dxCall);
                    xQubeUnionQuery1.AddAfterSelf(dxQuery);
                } else {
                    string dimAlias = dimension.XName;
                    if (extOuterDimensionNames.ContainsKey(dimAlias)) {
                        dimAlias = extOuterDimensionNames[dimAlias];
                    }
                    foreach (XElement timeCol in xcolsAll.Where(e => e.AttrOrEmpty(AName.table) == dimAlias).ToList()) {
                        XElement xExpr = dimension.GetTimeAttrExpression(xQubeUnionQuery1.Attribute(AName.@as).Value, dimension.XName, timeCol.Attribute(AName.column).Value);
                        xExpr.CopyAttributes(timeCol.Attributes().Where(APredicate.IsColumnRecoveredAttribute));
                        timeCol.ReplaceWith(xExpr);
                    }
                }
            }

            foreach (var item in factInfoList_N)
            {
                if (!item.Value.IsObject())
                {
                    XElement expr = null;

                    var colname = item.Value.Alias;

                    expr = new XElement(TextConst.EName.Column);
                    expr.SetAttributeValue(TextConst.EName.Table, TextConst.Pfx.QubeQueryAlias);
                    expr.SetAttributeValue(TextConst.EName.Column, colname);
                    expr.SetAttributeValue(TextConst.AName.As, colname);
                    //if (autoMerge)
                    //{
                    //    var gr = item.Value.Column.Attribute(TextConst.AName.Agg).Value;
                    //    expr.SetAttributeValue(TextConst.AName.Group, gr);
                    //}
                    //else
                    //{
                        expr.Attributes(TextConst.AName.Group).Remove();
                    //}
                    xQubeUnionQuery1.Element(TextConst.EName.Select).Add(expr);
                }
            }
            #region del
            //foreach (var factSrcItem in factSources)
            //{

            //    var factSrc = factSrcItem.Value;
      
            //    XElement expr = null;

            //    var colname = FactInfo.FromString(factSrcItem.Key).BuildName();
             
            //    expr = new XElement(TextConst.EName.Column);
            //    expr.SetAttributeValue(TextConst.EName.Table, TextConst.Pfx.QubeQueryAlias);
            //    expr.SetAttributeValue(TextConst.EName.Column, colname);
            //    expr.SetAttributeValue(TextConst.AName.As, colname);

            //    expr.Attributes(TextConst.AName.Group).Remove();
            //    xQubeUnionQuery1.Element(TextConst.EName.Select).Add(expr);

            //}


            #endregion

            int dcIndex = 0;
            /////////////////////////////////////////////////////


            if (qubeInfo != null)
            {

                var dimsInfo = new XElement(TextConst.EName.DimSet);

                foreach (string dimName in outerDimensionNames.Values)
                {
                    dimsInfo.Add(new XElement(TextConst.EName.Dimension
                          , new XAttribute(TextConst.AName.Name, dimName)
                          ));
                }
                qubeInfo.Add(dimsInfo);
            }

            var storages = new List<VQuery>();


            foreach (XElement storage in qubeElement.Elements(TextConst.EName.Storages).Elements())
            {
                var qry = XmlReports.Environment.GetQuery(storage.Attribute(TextConst.EName.Query).Value);
                storages.Add(qry);
            }

            foreach (DimensionCollection dimCollection in dimensionsCollections) // !!! внешние измерения могут сделать разные коллекции одинаковыми - слить такие.  Например внеш (ym,kod_dog) внутр (ym,kod_dog) (ym) () - будет одна вместо 3-х
            {
               
                //VDimSet dimset = null;

                //var nonOutputDimensions = new List<string>();

                //for (int i = 0; i < dimCollection.Dimensions.Count; i++)
                //{
                //    var dim = dimCollection.Dimensions[i];
                //    if (DimensionNameInfo.IsNonoutput(dim))
                //    {
                //        dimCollection.Dimensions[i] = DimensionNameInfo.GetDimNameFromFullName(dim);
                //        nonOutputDimensions.Add(dimCollection.Dimensions[i]);
                //    }

                //}


                //foreach (VQueryCall link in links)
                //{
                //    if (link.P_OnlyForCond == TextConst.AVBool.True)
                //    {
                //        nonOutputDimensions.Add(link.P_CalledQuery);
                //    }
                //}




                
                //var links1 = links.ToList();
                //var outerDimensionNames1 = new SortedList<string, string>();

                //foreach (var kv in outerDimensionNames)
                //{
                //    outerDimensionNames1.Add(kv.Key, kv.Value);
                //}




                //foreach (string dim in dimCollection.Dimensions)
                //{
                //    if (dimsetWhereDimensions.ContainsKey(dim))
                //    {
                //        dimset = (VDimSet)qubeElement.GetDimSet(dimsetWhereDimensions[dim]);
                //        dimCollection.Dimensions.Remove(dim);
                //        break;
                //    }
                //}
                //XElement addWhere = null;
                //if (dimset != null)
                //{

                //    addWhere = dimset.GetElementsApplyingParts(TextConst.EName.Where).FirstOrDefault();
                //    foreach (VQueryCall link in qubeElement.GetDimsetLinks(dimset.XName))
                //    {
                //        links1.Add(link);
                //        outerDimensionNames1.Add(link.P_CalledQuery, link.XName);
                //    }
                //}
                //List<string> addedLinks = new List<string>();
                //foreach (VQueryCall link in qubeElement.AllDimsetsLinks())
                //{

                //    if (dimCollection.Dimensions.Contains(link.P_CalledQuery))
                //    {
                //        if (!addedLinks.Contains(link.P_CalledQuery))
                //        {
                //            links1.Add(link);
                //            addedLinks.Add(link.P_CalledQuery);
                //        }

                //        if (!outerDimensionNames1.ContainsKey(link.P_CalledQuery))
                //        {
                //            outerDimensionNames1.Add(link.P_CalledQuery, link.XName);
                //        }
                //    }
                //}

                //dcIndex++;


                //CreateQubeQueryForCollection( outerDimensionNames1, dimCollection
                //    , factSources,  links1, allLinks, qubeElement, query, xQubeUnion
                //   , innerDimensions,  qubeInfo, nonOutputDimensions,storages);
                CreateQubeQueryForCollection(extOuterDimensionNames, dimCollection
                    , factInfoList_N, allLinks, links, qubeElement, query, xQubeUnion
                   , innerDimensions, qubeInfo, null, storages);
            }

            Cmn.copyAttributes(qubeElement, xQubeFullQuery);
            xQuery.Element(TextConst.EName.From).Element(TextConst.EName.Qube).ReplaceWith(xQubeFullQuery);
          

            var xcolsAll1 = Compiler.getQueryColumnsWithGr(xQuery).ToList();
            var xcolsAllJoin1 = Compiler.getQueryJoinColumns(xQuery).ToList();
            xcolsAll1.AddRange(xcolsAllJoin1);

            //int scopeIndex = 1;


            foreach (VLink link in allLinks)
            {
                var dimension = link.LinkedDimension();
                if (outputLinkNames.Contains(dimension.XName))
                {
                    if (dimension.P_TimeType == "")
                    {

                        string name = link.P_CalledQuery;
                        var dQuery = XmlReports.Environment.GetQueryByKeyDimensionName(name);

                        var dxQuery = new XElement(TextConst.EName.Query);
                        //  Cmn.CopyAttribute(link, dxQuery, TextConst.AName.MainInEditor);

                        dxQuery.SetAttributeValue(TextConst.AName.Dimension, name);
                        dxQuery.SetAttributeValue(TextConst.AName.LinkMultiplicatePoint, TextConst.AVBool.True);
                        dxQuery.SetAttributeValue(TextConst.AName.Name, dQuery.P_IdName);
                        dxQuery.SetAttributeValue(TextConst.AName.As, link.XName);

                        var joinType = TextConst.AVJoin.LeftOuter;

                        if (link.Attribute(TextConst.AName.Join) != null)
                        {
                            joinType = link.Attribute(TextConst.AName.Join).Value;
                        }

                        dxQuery.SetAttributeValue(TextConst.AName.Join, joinType);
                        var dxCall = new XElement(TextConst.EName.Call);
                        dxCall.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Equal);

                        var jcol = new XElement(TextConst.EName.Column);
                        jcol.SetAttributeValue(TextConst.EName.Table, link.XName);
                        jcol.SetAttributeValue(TextConst.EName.Column, dQuery.KeyColumn().XName);
                        dxCall.Add(jcol);

                        jcol = new XElement(TextConst.EName.Column);
                        jcol.SetAttributeValue(TextConst.EName.Table, qubeName);
                        jcol.SetAttributeValue(TextConst.EName.Column, name);
                        dxCall.Add(jcol);
                        dxQuery.Add(dxCall);
                        dxQuery.Add(link.Links(null));
                        xQubeFullQuery.AddAfterSelf(dxQuery);
                    } else {
                        string dimAlias = link.XName;
                        foreach (XElement timeCol in xcolsAll1.Where(e => e.AttrOrEmpty(AName.table) == dimAlias).ToList()) {
                            XElement xExpr = dimension.GetTimeAttrExpression(xQubeFullQuery.Attribute(AName.@as).Value, dimension.XName, timeCol.Attribute(AName.column).Value);
                            xExpr.CopyAttributes(timeCol.Attributes().Where(APredicate.IsColumnRecoveredAttribute));
                            Cmn.CopyAttribute(timeCol, xExpr, AName.group);
                            string alias = timeCol.AttrOrEmpty(AName.@as);
                            if (string.IsNullOrEmpty(alias)) {
                                alias = timeCol.AttrOrEmpty(AName.table);
                            }
                            xExpr.SetAttributeValue(AName.@as, alias);
                            timeCol.ReplaceWith(xExpr);
                        }
                    }
                }
            }

            var factsCols = xQuery.Element(TextConst.EName.Select).Descendants(TextConst.EName.Fact).ToList();
            factsCols.AddRange(
                 xQuery.Elements(TextConst.EName.Where).Descendants(TextConst.EName.Fact).ToList()
            );

            factsCols.AddRange(
                 xQuery.Elements(TextConst.EName.Having).Descendants(TextConst.EName.Fact).ToList()
            );

            factsCols.AddRange(
                xQuery.Elements(TextConst.EName.From).Elements(TextConst.EName.Query).Descendants(TextConst.EName.Fact).ToList()
           );

            #region del
            //XElement scopeLevelQuery = null;

            //if (factsCols.Elements(TextConst.EName.Scope).Any())
            //{

            //    scopeLevelQuery = new XElement(TextConst.EName.Query);
            //    scopeLevelQuery.Add(new XElement(TextConst.EName.Select));
            //    scopeLevelQuery.Add(new XElement(TextConst.EName.From));
            //    scopeLevelQuery.SetAttributeValue(TextConst.AName.As, qubeName);
            //    var qubeQuery = xQuery.Elements(TextConst.EName.From).Elements(TextConst.EName.Query).First();

            //    qubeQuery.Remove();

            //    scopeLevelQuery.Element(TextConst.EName.From).Add(qubeQuery);
            //    xQuery.Element(TextConst.EName.From).AddFirst(scopeLevelQuery);
            //    foreach (XElement col in qubeQuery.Element(TextConst.EName.Select).Elements())
            //    {
            //        bool used = false;

            //        if (col.Attribute(TextConst.AName.Group).Value == TextConst.AVBool.True)
            //        {
            //            if (outerDimensionNames.Keys.Contains(col.Attribute(TextConst.AName.Column).Value))
            //            {
            //                used = true;
            //            }
            //            if (allLinks.Select(e => e.P_CalledQuery).Contains(col.Attribute(TextConst.AName.Column).Value))
            //            {
            //                used = true;
            //            }

            //        }
            //        else
            //        {

            //            if (col.Attribute(TextConst.AName.Cumulate) == null)
            //            {
            //                used = true;

            //            }

            //        }

            //        if (used)
            //        {


            //            var col1 = new XElement(TextConst.EName.Column);

            //            col1.SetAttributeValue(TextConst.AName.Table, qubeName);

            //            col1.SetAttributeValue(TextConst.AName.Column, (Cmn.Nvl(col.Attribute(TextConst.AName.As), col.Attribute(TextConst.AName.Column)) as XAttribute).Value);

            //            Cmn.CopyAttribute(col, col1, TextConst.AName.Group);

            //            scopeLevelQuery.Element(TextConst.EName.Select).Add(col1);
            //        }


            //    }

            //}

            //var scopes = new SortedList<string, int>();
            #endregion
            foreach (XElement fact in factsCols)
            {


                var col = new XElement(TextConst.EName.Column);
                Cmn.copyAttributes(fact, col);



                col.SetAttributeValue(TextConst.AName.Table, qubeName);

                col.SetAttributeValue(TextConst.AName.As, (Cmn.Nvl(fact.Attribute(TextConst.AName.As), fact.Attribute(TextConst.AName.Column)) as XAttribute).Value);

                var piv = fact.Elements(TextConst.EName.Pivot).ToList();
                if (piv.Any())
                {
                   
                    piv.Remove();
                    col.Add(piv);
                   
                }

                #region del
                // col.Add(fact.Elements(TextConst.EName.Pivot));
                //var sect = fact.Element(TextConst.EName.Scope);
                //if (sect != null)
                //{

                //    var dimName = sect.Attribute(TextConst.AName.Cumulate).Value;

                //    var factName = fact.Attribute(TextConst.EName.Column).Value;
                    
                //    var scopeId = factName + dimName + sect.Elements().First().ToString();


                //    bool isNew = false;
                //    if (!scopes.ContainsKey(scopeId))
                //    {
                //        scopes.Add(scopeId, scopes.Count);
                //        isNew = true;
                //    }


                //    //scopeIndex = scopes[scopeId];
                //    var colName = new FactInfo(fact.Attribute(TextConst.AName.Column).Value, dimName, Cmn.GetAttrValue(fact, TextConst.EName.Table)).BuildName(); // Для форм наверное будет ошибка
                //    var colIndName = colName + scopeIndex.ToString();
                //    col.Attribute(TextConst.AName.Column).Value = colIndName;
                //    if (isNew)
                //    {


                //        var sourceExpr = query.GetEnvironment().GetFactSource(factName);



                //        var expr = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunc.If),
                //                         new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunc.Or),
                //                               new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, "="),
                //                                 new XElement(TextConst.AName.Column, new XAttribute(TextConst.AName.Table, qubeName), new XAttribute(TextConst.AName.Column, dimName)),
                //                                 new XElement(sect.Elements().First())
                //                               ),
                //                               new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunc.And),
                //                                   new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunc.Ls),
                //                                     new XElement(TextConst.AName.Column, new XAttribute(TextConst.AName.Table, qubeName), new XAttribute(TextConst.AName.Column, dimName)),
                //                                     new XElement(sect.Elements().First())
                //                                   )
                //                                   , new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunc.Or),
                //                                       new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunc.Gt),
                //                                         new XElement(TextConst.AName.Column, new XAttribute(TextConst.AName.Table, qubeName), new XAttribute(TextConst.AName.Column, dimName + TextConst.Pfx.CumulNext)),
                //                                         new XElement(sect.Elements().First())

                //                                       ),
                //                                       new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunc.IsNull),
                //                                         new XElement(TextConst.AName.Column, new XAttribute(TextConst.AName.Table, qubeName), new XAttribute(TextConst.AName.Column, dimName + TextConst.Pfx.CumulNext))
                //                                       )
                //                                   )
                //                                 ),
                //                                 new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunc.IsNull),
                //                                     new XElement(TextConst.AName.Column, new XAttribute(TextConst.AName.Table, qubeName), new XAttribute(TextConst.AName.Column, dimName))
                //                                   )
                //                             )
                //                             );

                //        expr.Add(
                //             new XElement(TextConst.AName.Column, new XAttribute(TextConst.AName.Table, col.Attribute(TextConst.AName.Table).Value), new XAttribute(TextConst.AName.Column, colName))
                //            );
                //        Cmn.CopyOrReplaceAttributes(col, expr, TextConst.ANameArray.ColumnRecoveredAttributes);
                //        expr.SetAttributeValue(TextConst.AName.DataType, sourceExpr.XDataType());
                //        expr.SetAttributeValue(TextConst.AName.As, colIndName);
                //        expr.SetAttributeValue(TextConst.AName.Group, sourceExpr.P_AggregationS);

                //        scopeLevelQuery.Element(TextConst.EName.Select).Add(expr);
                //    }
                //}
                //else
                #endregion
                {
                    if (fact.Attribute(TextConst.AName.Column).Value == "kod_ur_state")
                    {

                    }
                    //col.Attribute(TextConst.EName.Column).Value = new FactInfo(fact.Attribute(TextConst.AName.Column).Value, null, Cmn.GetAttrValue(fact, TextConst.EName.Table)).BuildName();
                    // вроде как уже установлен , см. //  20160729-1
                
                }


                fact.ReplaceWith(col);





            }

            //foreach (XElement fact in xQuery.Elements(TextConst.EName.Where).Descendants(TextConst.EName.Fact).ToList())
            //{
            //    var col = new XElement(TextConst.EName.Column);
            //    Cmn.copyAttributes(fact, col);
            //    col.SetAttributeValue(TextConst.EName.Table, qubeName);
            //    fact.ReplaceWith(col);

            //}

            xQubeFullQuery.Descendants() .Attributes(TextConst.AName.Dimname).Remove();
            xQubeFullQuery.Descendants().Attributes("pivot").Remove();

            if (autoMerge)
            {

                var processedNames = new HashSet<string>();
                var cols = xQuery.Descendants(TextConst.AName.Column).Where(e => Cmn.GetAttrValue(e, TextConst.AName.Table) == qubeName && Cmn.GetAttrValue(e, TextConst.AName.MergeKey) != "").ToArray();


                foreach (XElement el in cols)
                {
                    var name = Cmn.GetAttrValue(el, TextConst.AName.Column);
                    if (processedNames.Contains(name)) continue;
                    processedNames.Add(name);
                    
                  //  var alias = el.Attribute(TextConst.AName.As).Value;// Cmn.GetAttrValue(el, TextConst.AName.As);

                    XElement srcCol = null;
                    //if (specFactsForOptionalDimensions.Contains(Cmn.GetAttrValue(el, TextConst.AName.As)))
                    //{
                    //    srcCol = xQubeFullQuery.Element(TextConst.EName.From).Elements().First().Element(TextConst.EName.Select).Elements().First(e => Cmn.GetAttrValue(e, TextConst.AName.As) == name);
                    //}
                    //else
                    //{

                        srcCol = xQubeFullQuery.Element(TextConst.EName.Select).Elements().First(e => Cmn.GetAttrValue(e, TextConst.AName.As) == name);
                    //}
                    

                    var xovr=new XElement(TextConst.EName.Call,new XAttribute(TextConst.AName.Function,TextConst.AVFunction.Over));
                    Cmn.CopyAttributesNoReplace(srcCol, xovr);
                    xovr.Attributes(TextConst.AName.Table).Remove();
                    xovr.Attributes(TextConst.AName.Column).Remove();
                    xovr.Attributes(TextConst.AName.Group).Remove();
                    var xmax = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Max));
                    var xccol = new XElement(srcCol);
                    xmax.Add(xccol);
                    xovr.Add(xmax);

                    var xprt = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.PartitionBy));
                    var mk = el.Attribute(TextConst.AName.MergeKey).Value;
                    var ss = mk.Split(',');
                    foreach (var s in ss)
                    {
                        var keyName = mergerKeyAliasMergeKeyInQube[s];
                        var xpcol = new XElement(TextConst.EName.Column
                               , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                              , new XAttribute(TextConst.AName.Column, keyName)
                            );
                        xprt.Add(xpcol);
                    }

                    xovr.Add(xprt);

                    srcCol.ReplaceWith(xovr);

                }


                foreach (string name in optionalDimensions.Keys)
                {



                    var xovr = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Over));
                    var nm=TextConst.Pfx.Ovr + name;
                  
                    xovr.SetAttributeValue(TextConst.AName.As, nm);
                  
                    var xmax = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Max));
                    var xcnct = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Concat));

                    xmax.Add(xcnct);
                    xovr.Add(xmax);
                    var xpcol = new XElement(TextConst.EName.Column
                              , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                             , new XAttribute(TextConst.AName.Column, name)
                           );
                    var xprt = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.PartitionBy));
                    xprt.Add(xpcol);
                    xovr.Add(xprt);

                    foreach (string name1 in optionalDimensions[name])
                    {
                      
                        var xpcol1 = new XElement(TextConst.EName.Column
                               , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                              , new XAttribute(TextConst.AName.Column, name1)
                            );
                        xcnct.Add(xpcol1);
                    }
                    xQubeFullQuery.Element(TextConst.EName.From).Elements().First().Element(TextConst.EName.Select).Add(xovr);
                    



                
                    var srcCol = xQubeFullQuery.Element(TextConst.EName.Select).Elements().First(e => Cmn.GetAttrValue(e, TextConst.AName.Column) == name);
                    var xif = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.If));
                    Cmn.CopyAttributesNoReplace(srcCol, xif);
                    xif.Attributes(TextConst.AName.Table).Remove();
                    xif.Attributes(TextConst.AName.Column).Remove();
                    if (xif.Attribute(TextConst.AName.As) == null)
                    {
                        xif.SetAttributeValue(TextConst.AName.As, srcCol.Attribute(TextConst.AName.Column).Value);
                    }
                    var xcond = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.IsNotNull));
                    xif.Add(xcond);
                    xpcol = new XElement(TextConst.EName.Column
                              , new XAttribute(TextConst.AName.Table, qubeName)
                             , new XAttribute(TextConst.AName.Column, nm)

                           );
                    xcond.Add(xpcol);


                    var xccol = new XElement(srcCol);
                    xccol.Attributes(TextConst.AName.Group).Remove();
                    xif.Add(xccol);
                    srcCol.ReplaceWith(xif);

                }
            }

            Compiler.CutIdentifiersTo30(xQuery);
            return xQuery;
        }

        
        public static void CacheQubeIfNeed(VQuery query)
        {
            // чтобы не запускалось из ipsupport 

            
            //if (!XmlReports.IsDeveloperMode() || Application.ProductName != XmlReports.NativeProductName) return;

            //var qube_time = Cache.GetLastQubeCacheTime(query.P_Name);
            //var scheme_time = XmlReports.Environment.GetLastSchemeAssembleTime();
            //if (qube_time == DateTime.MinValue || qube_time < scheme_time)
            //{
            //    var qi = GetQubeInfo(query);
            //    Cache.SaveQubeInfoToCache(qi, query.P_Name);
            //}
        }

        private static SortedList<string, VFact.FactSourceQueryInfo> GetSourceQueryInfoList(VEnvironment env, List<VQubeUtils.DimensionCollection> dimCollections)
        {
            var srcQueryInfoList_N = new SortedList<string, VFact.FactSourceQueryInfo>();

            foreach (var dc in dimCollections)
            {
                foreach (var f in dc.Facts.Values)
                {
                    var qi = f.GetSourceQueryInfo(env, srcQueryInfoList_N);
                    foreach (var dimName in dc.Dimensions)
                    {
                        if (!qi.Dimensions.Contains(dimName))
                        {
                            qi.Dimensions.Add(dimName);
                        }
                    }
                }
            }
            return srcQueryInfoList_N;
        }

        private static void CreateQubeQueryForCollection(SortedList<string, string> allDimensionNames // проверено

            , DimensionCollection dimCollection

            , SortedList<string, VFact.FactDependantceInfo> factInfoList_N 

            , List<VQueryCall> allLinks // заменил на allLinks
             , List<VQueryCall> mainLinks
            , VQube qubeElement
            , VQuery query
            , XElement xQubeUnion
     
            , List<string> allInnerDimeNames_D
   
            , XElement info
            , List<string> nonOutputDimensions_D,
            List<VQuery> storages
            )
        {

          //  var cumulateDimNames = cumulateDimNamesByFact.Where(e => dimCollection.Facts.Contains(e.Key)).SelectMany(e1 => e1.Value).Distinct().ToList();
            var dimensionNames = new SortedList<string, string>();


            foreach (var dimName in dimCollection.Dimensions)
            {
                dimensionNames.Add(dimName, allDimensionNames[dimName]);
            }

            //foreach (string dimName in outerDimensionNames.Keys)
            //{
            //    dimensionNames.Add(dimName, outerDimensionNames[dimName]);
            //}

            //foreach (string dimName in dimCollection.Dimensions)
            //{
            //    if (!dimensionNames.ContainsKey(dimName))
            //    {
            //        dimensionNames.Add(dimName, dimName);
            //    }
            //}

            //var innerDimensionNames = new List<string>();

            //innerDimensionNames.AddRange(dimCollection.Dimensions);
            /////////
            //var localFactSources = dimCollection.Facts.Select(e => factSources[e.Name]).ToList();// !!! ???
            //var factColumns = localFactSources.SelectMany(f => f.GetFactColumns()).Distinct().ToList();
            //var factSourceQueriesColumns = new SortedList<string, List<VSXElement>>();
            //var factSourceQueries = new List<VSourcedElement>();
            //foreach (VSXElement factColumn in factColumns)
            //{
            //    var factSourceQuery = factColumn.RootQuery().GetMainE();
            //    if (!factSourceQueriesColumns.ContainsKey(factSourceQuery.P_IdName))
            //    {
            //        factSourceQueriesColumns.Add(factSourceQuery.P_IdName, new List<VSXElement>());
            //        factSourceQueries.Add(factSourceQuery);
            //    }
            //    factSourceQueriesColumns[factSourceQuery.P_IdName].Add(factColumn);
            //}


            var dimCollections = new List<DimensionCollection>();
            dimCollections.Add(dimCollection);

            var srcQueryInfoList_N = GetSourceQueryInfoList(XmlReports.Environment, dimCollections);
            

            

            //!!! ???
            //if (allRowsQuery != null && dcIndex == 1)
            //{
            //    var factSourceQuery = allRowsQuery;
            //    if (!factSourceQueriesColumns.ContainsKey(factSourceQuery.P_IdName))
            //    {
            //        factSourceQueriesColumns.Add(factSourceQuery.P_IdName, new List<VSXElement>());
            //        factSourceQueries.Add(factSourceQuery);
            //    }


            //}

            XElement xQubeQuery = new XElement(TextConst.EName.Query);
            xQubeQuery.Add(new XElement(TextConst.EName.Select));
            xQubeQuery.Add(new XElement(TextConst.EName.From));


            XElement xunQuery = new XElement(TextConst.EName.Query);
            xunQuery.SetAttributeValue(TextConst.AName.As, qubeName);
            xQubeQuery.Element(TextConst.EName.From).Add(xunQuery);

            XElement xunSubQuery = new XElement(TextConst.EName.Query);
            xunSubQuery.SetAttributeValue(TextConst.AName.As, unName);

            xunQuery.Add(new XElement(TextConst.EName.Select));
            xunQuery.Add(new XElement(TextConst.EName.From));
            xunQuery.Element(TextConst.EName.From).Add(xunSubQuery);
            XElement xunion = new XElement(TextConst.EName.Union);

            xunSubQuery.Add(xunion);




            XElement element = null;



            XElement collectionInfo = null;

            if (info != null)
            {
                collectionInfo = new XElement(TextConst.EName.Qube);
                info.Add(collectionInfo);

                var dimsInfo = new XElement(TextConst.EName.DimSet);

                foreach (string dimName in dimCollection.Dimensions)
                {
                    dimsInfo.Add(new XElement(TextConst.EName.Dimension
                          , new XAttribute(TextConst.AName.Name, dimName)
                          ));
                }
                collectionInfo.Add(dimsInfo);
            }


            

            foreach (var qryInfo in srcQueryInfoList_N)
            {


                XElement addWhere=null;

                //List<string> nonOutputDimensions = new List<string>();
                var dimensionNamesExt = new SortedList<string, string>();

                foreach (var kv in dimensionNames)
                {
                    dimensionNamesExt.Add(kv.Key, kv.Value);
                }
                HashSet<string> dimsWithWhere = new HashSet<string>();
                foreach (var link in mainLinks)
                {


                    if (link.Elements(TextConst.EName.Where).Any())
                    {
                        if (!dimsWithWhere.Contains(link.P_CalledQuery))
                        {
                            dimsWithWhere.Add(link.P_CalledQuery);
                        }
                    }
                    //if (link.P_OnlyForCond != TextConst.AVBool.True) // 28.04.2017 // убрал 05.05.2017 - не добавляются линки заданные не через dimset - ошибка
                    //{
                        if (!dimensionNamesExt.ContainsKey(link.P_CalledQuery))
                        {
                            dimensionNamesExt.Add(link.P_CalledQuery, link.XName);
                        }
                    //}
                }

                if (qryInfo.Value.Conditions.Any())
                {

                    foreach (string condName in qryInfo.Value.Conditions)
                    {
                        List<XElement> expr1 = null;
                        List<string> xtraDimNames = new List<string>();
                        bool isDimset = false;
                        if (!condName.EndsWith(TextConst.Pfx.Dimset))
                        {

                            var expr = query.SearchExpression(condName);
                            if (expr == null)
                            {
                                expr = XmlReports.Environment.GetExpression(condName);
                            }
                            expr1 = new List<XElement>();
                            expr1.Add( new XElement(expr));

                            foreach (var col1 in expr.GetDimensions())
                            {
                                if (!xtraDimNames.Contains(col1.P_Table))
                                {
                                    xtraDimNames.Add(col1.P_Table);
                                }
                            }
                        }
                        else
                        {
                            isDimset = true;
                            var dimsetName = condName.Substring(0, condName.Length - TextConst.Pfx.Dimset.Length);
                            var dimset = (VDimSet)qubeElement.GetDimSet(dimsetName);
                            expr1 =  dimset.GetElementsP(EName.where).FirstOrDefault().Elements().Select(e=>new XElement(e)).ToList();
                            foreach (VQueryCall link in qubeElement.GetDimsetLinks(dimset.XName))
                            {
                                if (!xtraDimNames.Contains(link.P_CalledQuery))
                                {
                                    xtraDimNames.Add(link.P_CalledQuery);
                                }
                              
                            }
                        }

                        foreach (string xtraDimName in xtraDimNames)
                        {
                            if (!dimensionNamesExt.ContainsKey(xtraDimName))
                            {
                                dimensionNamesExt.Add(xtraDimName, xtraDimName);
                                if (allDimensionNames.ContainsKey(xtraDimName))
                                {
                                    dimensionNamesExt[xtraDimName] = allDimensionNames[xtraDimName];
                                }
                            }
                            if (!isDimset)
                            {
                                if (dimensionNamesExt[xtraDimName] != xtraDimName)
                                {

                                    foreach (XElement dcol1 in expr1.Descendants(TextConst.EName.Column).Where(e => e.Attribute(TextConst.AName.Table).Value == xtraDimName).ToList())
                                    {
                                        dcol1.Attribute(TextConst.AName.Table).Value = dimensionNamesExt[xtraDimName];
                                    }
                                }
                            }
                        }

                        foreach (XElement xcond in expr1)
                        {

                            addWhere = Compiler.extendWhereByAnd(addWhere, xcond);
                        }

                    }
                }





                VQuery factSourceQuery = qryInfo.Value.Query;

               // var factsCols = qryInfo.Value.Facts.Select(f => f.Column).ToList();
                XElement sourceInfo = null;
                if (info != null)
                {
                    sourceInfo = new XElement(TextConst.EName.Query);
                    sourceInfo.SetAttributeValue(TextConst.AName.Name, factSourceQuery.P_IdName);
                    collectionInfo.Add(sourceInfo);
                    var columnsInfo = new XElement(TextConst.EName.Columns);
                    //foreach (VSXElement fact in factSourceQueriesColumns[factSourceQuery.P_IdName])
                    foreach (var fact1 in qryInfo.Value.Facts)
                    {
                        if (!fact1.IsObject())
                        {
                            var fact = fact1.Column;
                            columnsInfo.Add(new XElement(TextConst.EName.Column
                                , new XAttribute(TextConst.AName.Column, fact.XName)
                                , new XAttribute(TextConst.AName.Fact, fact.P_Fact)
                                ));
                        }
                        else
                        {
                            columnsInfo.Add(new XElement(TextConst.EName.Column
                              , new XAttribute(TextConst.AName.Table, fact1.ObjectFact)
                             
                              ));
                        }
                    }
                    sourceInfo.Add(columnsInfo);

                }

                element = new XElement(TextConst.EName.Query);
                element.SetAttributeValue(TextConst.AName.As, unName);
                element.SetAttributeValue(TextConst.AName.MultiplicatePoint, TextConst.AVBool.True);
                element.Add(new XElement(TextConst.EName.Select));
                element.Add(new XElement(TextConst.EName.From));





                var parentLookUpNode = new DimensionPathTreeNode();

                var found = new List<DimensionPathTreeNode>();
                if (factSourceQuery.Attribute(TextConst.AName.Name).Value == "sr_opl_sf")
                {

                }
                var storages1 = storages.ToList();


                //if (dimensionNamesExt.ContainsKey("kod_graf"))
                //{
                //}
                foreach (VQueryCall link in allLinks) ///!! заплатка. чтобы использовать storage не для всех фактов переделать
                {
                    if (dimensionNamesExt.ContainsKey(link.P_Name))
                    {
                        foreach (XElement storage in link.Parent.Elements(TextConst.EName.Storages).Elements())
                        {
                            var qry = XmlReports.Environment.GetQuery(storage.Attribute(TextConst.EName.Query).Value);
                            if (!storages1.Contains(qry))
                            {
                                storages1.Add(qry);
                            }
                        }
                    }
                }

                SearchSourceLinks(factSourceQuery, dimensionNamesExt, parentLookUpNode, found, storages1,qubeElement);

                List<DimensionPathTreeNode> routs = null;
                if (found.Count != 0)
                {
                    var dimNodes = new SortedList<string, List<DimensionPathTreeNode>>();
                    foreach (DimensionPathTreeNode node in found)
                    {
                        if (!dimNodes.ContainsKey(node.DimensionName))
                        {
                            dimNodes.Add(node.DimensionName, new List<DimensionPathTreeNode>());
                        }
                        dimNodes[node.DimensionName].Add(node);
                    }

                    foreach (string dimName in dimNodes.Keys)
                    {
                        if (dimNodes[dimName].Count > 1)
                        {

                            


                            for (int i = 1; i < dimNodes[dimName].Count; i++)
                            {
                                var node = dimNodes[dimName][i];
                                var parent = DimensionPathTreeNode.SearchCommonParent(dimNodes[dimName][0], node);

                                if (parent != null)
                                {
                                    var path1 = DimensionPathTreeNode.PathFromTo(parent, node);
                                    var path2 = DimensionPathTreeNode.PathFromTo(parent, dimNodes[dimName][0]);

                                    if (path1 != path2)
                                    {
                                        parent = null;
                                    }

                                }

                                if (qubeElement.P_SingleWay == TextConst.AVBool.True)
                                {
                                    parent = null;
                                }

                                if (parent == null)
                                {

                                    parent = DimensionPathTreeNode.SearchCommonParent(dimNodes[dimName][0], node);

                                    if (parent != null)
                                    {
                                        var path1 = DimensionPathTreeNode.PathFromTo(parent, node);
                                        var path2 = DimensionPathTreeNode.PathFromTo(parent, dimNodes[dimName][0]);

                                        if (path1 != path2)
                                        {
                                            parent = null;
                                        }

                                    }

                                    throw new System.InvalidOperationException("Неоднозначный путь от " + factSourceQuery.P_IdName + " к измерению " + dimName + "\r" + parentLookUpNode.ViewAsXml().ToString().Replace("\n", ""));
                                }
                            }

                        }
                    }




                  

                    if (false)
                    {
                        routs = new List<DimensionPathTreeNode>();
                        routs.Add(parentLookUpNode);
                    }
                    else
                    {
                        var routs1 = parentLookUpNode.Split();
                        routs = routs1;
                    }
                }
                else
                {
                    routs = new List<DimensionPathTreeNode>();
                    routs.Add(parentLookUpNode);
                }

                if (collectionInfo != null)
                {
                    foreach (var lookUpNode in routs)
                    {
                        var localInfo = lookUpNode.ViewAsXml();
                        localInfo.DescendantsAndSelf().Attributes(TextConst.AName.Dimension).Where(e => !dimensionNames.Values.Contains(e.Value)).Remove();
                        sourceInfo.Add(localInfo);
                    }
                }




                XElement xunionDim = null;
                XElement xunQueryDim = null;
                var isSimpleRout = false;
                if (routs.Count > 1)
                {

                    xunQueryDim = new XElement(TextConst.EName.Query);
                    xunQueryDim.SetAttributeValue(TextConst.AName.As, dimQryName);
                    element.Element(TextConst.EName.From).Add(xunQueryDim);

                    XElement xunSubQueryDim = new XElement(TextConst.EName.Query);
                    xunSubQueryDim.SetAttributeValue(TextConst.AName.As, dimQryName);

                    xunQueryDim.Add(new XElement(TextConst.EName.Select));
                    xunQueryDim.Add(new XElement(TextConst.EName.From));
                    xunQueryDim.Element(TextConst.EName.From).Add(xunSubQueryDim);
                    xunionDim = new XElement(TextConst.EName.Union);

                    xunSubQueryDim.Add(xunionDim);
                }
                else
                {
                    if (!routs[0].AllNodes().Where(e => e.IsContainsBackReferences || e.StorageName!=null).Any())
                    {
                        isSimpleRout = true;
                    }
                }

                Dictionary<string, XElement> columnsForSimpleRout = null;
                foreach (DimensionPathTreeNode parentLookUpNode1 in routs)
                {
                    List<DimensionPathTreeNode> found1 = null;
                    List<DimensionPathTreeNode> found1Ext = null;
                    if (found.Count == 0)
                    {
                        found1 = found;
                        found1Ext = found;
                    }
                    else
                    {
                        //found1 = GetLeavs(parentLookUpNode1);
                      found1 = parentLookUpNode1.AllNodes().Where(e => e.Parent != null && e.DimensionName != null && dimensionNames.Keys.Contains(e.DimensionName)).ToList();
                      found1Ext = parentLookUpNode1.AllNodes().Where(e => e.Parent != null && e.DimensionName != null && dimensionNamesExt.Keys.Contains(e.DimensionName)).ToList();
                        //20161109 - заменил dimensionNames на dimensionNamesExt ,  иначе проблемы с веременными измерениями только для условий
                    
                    
                    }

                    XElement xDimQry = null;

                    if (!isSimpleRout)
                    {
                        xDimQry = new XElement(TextConst.EName.Query);
                        xDimQry.SetAttributeValue(TextConst.AName.As, dimQryName);
                        xDimQry.Add(new XElement(TextConst.EName.Select));
                        xDimQry.Add(new XElement(TextConst.EName.From));
                    }

                    if (routs.Count > 1)
                    {
                        xunionDim.Add(xDimQry);
                    }
                    else
                    {
                        if (!isSimpleRout)
                        {
                            element.Element(TextConst.EName.From).Add(xDimQry);
                        }
                    }


                    XElement dimFromQuery = new XElement(TextConst.EName.Query);
                    dimFromQuery.SetAttributeValue(TextConst.AName.Name, factSourceQuery.P_IdName);
                    dimFromQuery.SetAttributeValue(TextConst.AName.As, parentLookUpNode.Alias);

                    if (isSimpleRout)
                    {
                        element.Element(TextConst.EName.From).Add(dimFromQuery);
                    }
                    else
                    {
                        xDimQry.Element(TextConst.EName.From).Add(dimFromQuery);

                    }

                    XElement commonWhere = qubeElement.GetElementsP(EName.where).FirstOrDefault();
                   
                
                    if (commonWhere != null)
                    {
                        commonWhere = new XElement(commonWhere);
                    }




                    if (addWhere != null)
                    {
                        if (commonWhere == null)
                        {
                            commonWhere = addWhere;
                        }
                        else
                        {
                            Compiler.extendWhereByAnd(commonWhere, new XElement(addWhere.Elements().First()));
                        }
                    }


                    if (commonWhere != null)
                    {
                        commonWhere = new XElement(commonWhere);
                    }

                    AddLinksXml(parentLookUpNode1, dimFromQuery, allLinks, dimFromQuery.Parent,commonWhere);

                    var selfDim = factSourceQuery.GetDimension();
                    if (selfDim != null)
                    {
                        // ???
                        dimFromQuery.SetAttributeValue(TextConst.AName.Dimension, selfDim.P_IdName);
                    }

                   
                    foreach (VSXElement alink in allLinks)
                    {
                        
                        if (alink.XName == dimFromQuery.Attribute(TextConst.AName.As).Value)
                        {
                            dimFromQuery.Add(alink.Elements());
                        }
                    }




                    //XElement commonWhere = qubeElement.GetElementsApplyingParts(TextConst.EName.Where).FirstOrDefault();

                    //if (commonWhere != null)
                    //{
                    //    commonWhere = new XElement(commonWhere);
                    //}




                    //if (addWhere != null)
                    //{
                    //    if (commonWhere == null)
                    //    {
                    //        commonWhere = addWhere;
                    //    }
                    //    else
                    //    {
                    //        Compiler.extendWhereByAnd(commonWhere, new XElement(addWhere.Elements().First()));
                    //    }
                    //}


                    //if (commonWhere != null)
                    //{
                    //    commonWhere = new XElement(commonWhere);
                    //}

                    



                    if (isSimpleRout)
                    {

                        element.Add(commonWhere);

                        //if (factSourceQuery.GetQubeElement() != null) // Обработан тоько частный случай пропихивания условий
                        //{

                        //    var qc = new XElement(TextConst.EName.QubeContent);
                        //    if (commonWhere != null)
                        //    {
                        //        var cw = new XElement(commonWhere);

                        //        qc.Add(cw);
                        //    }
                        //    dimFromQuery.Add(qc);

                        //}
                        columnsForSimpleRout = new Dictionary<string, XElement>();
                        foreach (string name in dimensionNames.Keys)
                        {
                            columnsForSimpleRout.Add(name, null);
                        }
                    }
                    else
                    {

                        xDimQry.Add(commonWhere);
                    }


                    var qc = new XElement(TextConst.EName.QubeContent);
                    if (commonWhere != null)
                    {
                        var cw = new XElement(commonWhere);
                        var qname = query.XName;
                        if (string.IsNullOrEmpty(qname))
                        {
                            qname = query.P_Comment;
                        }
                        foreach (XElement cond in cw.Elements())
                        {
                            if (!cond.Attributes(TextConst.AName.CondSource).Any())
                            {
                                cond.SetAttributeValue(TextConst.AName.CondSource, qname);
                            }
                        }
                        qc.Add(cw);
                    }
                  //  dimFromQuery.Add(qc);

                    element.Element(TextConst.EName.From).Add(qc);
                    //if (isSimpleRout)
                    //{

                       

                    //    dimFromQuery.Add(qc);
                    //}
                    //else
                    //{

                    //    xDimQry.Add(qc);
                    //}


                    string colAlias = null;
                    var keyCol = factSourceQuery.KeyColumn();
                    VDimension dim = factSourceQuery.GetDimension();

                    XElement col1 = null;
                    if (keyCol != null && dim != null)
                    {
                        col1 = new XElement(TextConst.EName.Column);
                        col1.SetAttributeValue(TextConst.AName.Table, parentLookUpNode.Alias);
                        col1.SetAttributeValue(TextConst.AName.Column, keyCol.XName);
                        colAlias = factSourceQuery.GetDimension().P_IdName;
                        col1.SetAttributeValue(TextConst.AName.As, colAlias);


                        if (isSimpleRout)
                        {
                            if (columnsForSimpleRout.Keys.Contains(colAlias) && columnsForSimpleRout[colAlias] == null)
                            {
                                columnsForSimpleRout[colAlias] = col1;
                            }
                        }
                        else
                        {
                            col1.SetAttributeValue(TextConst.AName.Group, TextConst.AVGroup.Group);
                            xDimQry.Element(TextConst.EName.Select).Add(col1);
                        }



                        col1 = new XElement(col1);
                        colAlias = col1.Attribute(TextConst.AName.Column).Value + TextConst.Pfx.PrimaryKeyParam;
                        col1.SetAttributeValue(TextConst.AName.As, colAlias);

                        if (isSimpleRout)
                        {
                            if (columnsForSimpleRout.Keys.Contains(colAlias) && columnsForSimpleRout[colAlias] == null)
                            {
                                columnsForSimpleRout[colAlias] = col1;
                            }
                        }
                        else
                        {
                            xDimQry.Element(TextConst.EName.Select).Add(col1);
                        }
                    }

                    foreach (DimensionPathTreeNode node in found1.OrderBy(n=>n.DimensionName))
                    {
                        col1 = new XElement(TextConst.EName.Column);
                        string colName;
                        string tableName;
                        string alias;
                        if (node.IsLink)
                        {
                            if (node.IsBack)
                            {
                                tableName = node.Alias;
                                var keyDim = (node.Element as VQueryCall).Query().KeyColumn();
                                colName = keyDim.XName;
                            }
                            else
                            {
                                if (node.Element.GetParent() is VLink || node.Element.GetParent() is VELink || dimsWithWhere.Contains(node.DimensionName))
                                {
                                    tableName = node.Alias;
                                    var keyDim = (node.Element as VQueryCall).Query().KeyColumn();
                                    colName = keyDim.XName;
                                }
                                else
                                {

                                    tableName = node.Parent.Alias;
                                    colName = (node.Element as VLink).GetRelation().ChildColumnSource().XName;
                                }


                            }

                            alias = node.Element.P_Dimension;
                        }
                        else
                        {
                           
                            if (node.StorageName != null)
                            {
                                tableName=node.Alias;
                                var keyDim = (node.Element as VQueryCall).Query().KeyColumn();
                                colName = keyDim.XName;
                            }
                            else
                            {
                                tableName = node.Parent.Alias;
                                if (node.IsColumn)
                                {
                                    colName = node.Element.XName;
                                }
                                else
                                {
                                    colName = (node.Element as VRelation).ChildColumnSource().XName;
                                }
                            }
                            
                            alias = node.Element.P_Dimension;

                        }
                        col1.SetAttributeValue(TextConst.AName.Table, tableName);
                        col1.SetAttributeValue(TextConst.AName.Column, colName);


                        col1.SetAttributeValue(TextConst.AName.As, alias);



                        if (isSimpleRout)
                        {
                           
                            if (columnsForSimpleRout.Keys.Contains(alias) && columnsForSimpleRout[alias] == null)
                            {

                                columnsForSimpleRout[alias] = col1;
                            }
                        }
                        else
                        {
                            //if (nonOutputDimensions.Contains(node.DimensionName))
                            //{
                            //    var col2 = new XElement(TextConst.EName.Const, new XText("null"));
                            //    Cmn.CopyAttribute(col1, col2, TextConst.AName.As);
                            //    col1 = col2;
                            //}
                            col1.SetAttributeValue(TextConst.AName.Group, TextConst.AVGroup.Group);
                            xDimQry.Element(TextConst.EName.Select).Add(col1);
                        }



                        


                    }


                    foreach (DimensionPathTreeNode node in found1Ext.OrderBy(n => n.DimensionName))
                    {
                        if (node.IsColumn)
                        {
                            if (commonWhere != null)
                            {
                                var dimension = XmlReports.Environment.GetDimension(node.DimensionName);


                                if (dimension.P_TimeType != "")
                                {

                                    string colName;
                                    string tableName;
                                    if (node.StorageName != null)
                                    {
                                        // не знаю может ли быть такое , оставил на всякий случай
                                        tableName = node.Alias;
                                        var keyDim = (node.Element as VQueryCall).Query().KeyColumn();
                                        colName = keyDim.XName;
                                    }
                                    else
                                    {
                                        tableName = node.Parent.Alias;
                                        colName = node.Element.XName;
                                    }


                                    string dimAlias = dimension.XName;
                                    if (allDimensionNames.ContainsKey(dimAlias)) {
                                        dimAlias = allDimensionNames[dimAlias];
                                    }
                                    foreach (XElement timeCol in commonWhere.Descendants(EName.column).Where(e => e.AttrOrEmpty(AName.table) == dimAlias).ToList()) {
                                        XElement xExpr = dimension.GetTimeAttrExpression(tableName, colName, timeCol.Attribute(AName.column).Value);
                                        xExpr.CopyAttributes(timeCol.Attributes().Where(APredicate.IsColumnRecoveredAttribute));
                                        Cmn.CopyAttribute(timeCol, xExpr, AName.@as);
                                        timeCol.ReplaceWith(xExpr);
                                    }
                                }
                            }

                        }
                    }


                    if (xunQueryDim != null)
                    {
                        if (!xunQueryDim.Element(TextConst.EName.Select).Elements().Any())
                        {
                            foreach (XElement col2 in xDimQry.Element(TextConst.EName.Select).Elements())
                            {
                                col1 = new XElement(TextConst.EName.Column);
                                col1.SetAttributeValue(TextConst.AName.Table, dimQryName);

                                col1.SetAttributeValue(TextConst.AName.Column, col2.Attribute(TextConst.AName.As).Value);

                                col1.SetAttributeValue(TextConst.AName.As, col2.Attribute(TextConst.AName.As).Value);
                                Cmn.CopyAttribute(col2, col1, TextConst.AName.Agg);
                                xunQueryDim.Element(TextConst.EName.Select).Add(col1);
                            }
                        }
                    }


                }


                XElement fromQuery = null;
                XElement col = null;
                var selDim = factSourceQuery.GetDimension();
                if (!isSimpleRout)
                {
                    fromQuery = new XElement(TextConst.EName.Query);
                    fromQuery.SetAttributeValue(TextConst.AName.Name, factSourceQuery.P_IdName);



                    fromQuery.SetAttributeValue(TextConst.AName.Join, TextConst.AVJoin.LeftOuter);

                    XElement joinCall = new XElement(TextConst.EName.Call);
                    joinCall.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Equal);
                    fromQuery.Add(joinCall);




                    col = new XElement(TextConst.EName.Column);
                    col.SetAttributeValue(TextConst.AName.Table, factSourceQuery.P_IdName);
                    col.SetAttributeValue(TextConst.AName.Column, factSourceQuery.KeyColumn().XName);
                    joinCall.Add(col);


                    col = new XElement(TextConst.EName.Column);
                    col.SetAttributeValue(TextConst.AName.Table, dimQryName);
                    col.SetAttributeValue(TextConst.AName.Column, factSourceQuery.KeyColumn().XName + TextConst.Pfx.PrimaryKeyParam);
                    joinCall.Add(col);

                   
                    element.Element(TextConst.EName.From).Add(fromQuery);
                    if (selDim != null)
                    {
                        fromQuery.SetAttributeValue(TextConst.AName.Dimension, selDim.P_IdName);
                        
                    }

                }
                else
                {

                    if (selDim != null)
                    {
                        element.SetAttributeValue(TextConst.AName.Dimension, selDim.P_IdName);
                       
                    }
                }



                foreach (string name in dimensionNames.Keys)
                {
                    if (isSimpleRout)
                    {
                        col = columnsForSimpleRout[name];
                    }
                    else
                    {
                        col = new XElement(TextConst.EName.Column);
                        col.SetAttributeValue(TextConst.AName.Table, dimQryName);
                        col.SetAttributeValue(TextConst.AName.Column, name);
                        col.SetAttributeValue(TextConst.AName.As, name);
                    }
                    element.Element(TextConst.EName.Select).Add(col);

                }







                foreach (string key in srcQueryInfoList_N.Keys)
                {
                    foreach (var srcColInfo in srcQueryInfoList_N[key].Facts)
                    {
                        if (!srcColInfo.IsObject())
                        {
                            var srcCol = srcColInfo.Column;
                            if (key == qryInfo.Key)
                            {
                                col = new XElement(TextConst.EName.Column);

                                string fsalias = null;

                                if (isSimpleRout)
                                {
                                    fsalias = parentLookUpNode.Alias;
                                }
                                else
                                {
                                    fsalias = factSourceQuery.P_IdName;
                                }


                                col.SetAttributeValue(TextConst.AName.Table, fsalias);
                                col.SetAttributeValue(TextConst.AName.Column, srcCol.XName);
                            }
                            else
                            {
                                col = new XElement(TextConst.EName.Const);
                                col.SetAttributeValue(TextConst.AName.DataType, srcCol.XDataType());
                                col.Value = "null";
                            }
                            col.SetAttributeValue(TextConst.AName.As, srcColInfo.Alias);///////////////////// 20160729-2
                            element.Element(TextConst.EName.Select).Add(col);
                        }

                    }
                }

                xunion.Add(element);

            }


            foreach (string name in dimensionNames.Keys)
            {

                var col1 = new XElement(TextConst.EName.Column);
                col1.SetAttributeValue(TextConst.AName.Table, unName);
                col1.SetAttributeValue(TextConst.AName.Column, name);
                col1.SetAttributeValue(TextConst.AName.Group, TextConst.AVBool.True);
                xunQuery.Element(TextConst.EName.Select).Add(col1);

            }


            foreach (string key in srcQueryInfoList_N.Keys)
            {
                foreach (var srcColInfo in srcQueryInfoList_N[key].Facts)
                {
                    if (!srcColInfo.IsObject())
                    {
                        var srcCol = srcColInfo.Column;

                        var gr = srcCol.P_AggregationS;

                        if (gr == TextConst.AVGroup.List)
                        {
                            gr = TextConst.AVGroup.Group;
                          
                        }

                        if (gr == "")
                        {
                            throw new VCompilerException("Не указан метод аггрегации", srcCol.GetMainParent(), srcCol);
                        }
                        var col1 = new XElement(TextConst.EName.Column);
                        col1.SetAttributeValue(TextConst.AName.Table, unName);
                        col1.SetAttributeValue(TextConst.AName.Column, srcColInfo.Alias);// 20160729-2
                        col1.SetAttributeValue(TextConst.AName.Group, gr);
                        xunQuery.Element(TextConst.EName.Select).Add(col1);
                    }
                }
            }
           



            //foreach (var link in allLinks)
            //{
            //    XElement col1 = null;

            //    //if (outerDimensionNames.Keys.Contains(link.P_CalledQuery))
            //    if (dimCollection.Dimensions .Contains(link.P_CalledQuery))
            //    {
            //        col1 = new XElement(TextConst.EName.Column);
            //        col1.SetAttributeValue(TextConst.AName.Table, qubeName);
            //        col1.SetAttributeValue(TextConst.AName.Column, link.P_CalledQuery);
            //    }
            //    else
            //    {
            //        col1 = new XElement(TextConst.EName.Const);
            //        col1.Value = "null";
            //        col1.SetAttributeValue(TextConst.AName.As, link.P_CalledQuery);
            //    }

                
            //    xQubeQuery.Element(TextConst.EName.Select).Add(col1);


            //}
            foreach (string name in allDimensionNames.Keys)
            {
                XElement col = null;
                if (dimCollection.Dimensions.Contains(name))
                {
                    col = new XElement(TextConst.EName.Column);
                    col.SetAttributeValue(TextConst.AName.Table, qubeName);
                    col.SetAttributeValue(TextConst.AName.Column, name);
                    col.SetAttributeValue(TextConst.AName.As, name);

                }
                else
                {
                    col = new XElement(TextConst.EName.Const);
                  
                    col.Value = "null";
                    col.SetAttributeValue(TextConst.AName.As, name);
                }
                xQubeQuery.Element(TextConst.EName.Select).Add(col);

            }




           // factInfoList_N

          
            foreach (var factSrcItem in factInfoList_N)
            {
            

                XElement expr = null;
                var factSrc = factSrcItem.Value;
               
                //var factInfo = FactInfo.FromString(factSrcItem.Key);


                if (!factSrc.IsObject())
                {
                    if (dimCollection.Facts.ContainsKey(factSrcItem.Key))
                    {



                        expr = new XElement(TextConst.EName.Column);
                        expr.SetAttributeValue(TextConst.EName.Table, TextConst.Pfx.QubeQueryAlias);
                        expr.SetAttributeValue(TextConst.EName.Column, factSrc.Alias);




                    }
                    else
                    {
                        expr = new XElement(TextConst.EName.Const);
                        expr.SetAttributeValue(TextConst.AName.DataType, factSrc.Column.XDataType());
                        expr.Value = "null";

                    }


                    expr.SetAttributeValue(TextConst.AName.As, factSrc.Alias);
                    expr.Attributes(TextConst.AName.Group).Remove();
                    xQubeQuery.Element(TextConst.EName.Select).Add(expr);
                }

            }



            xQubeQuery.SetAttributeValue(TextConst.AName.As, qubeName);





            


            var xcolsAll = Compiler.getQueryColumnsWithGr(xQubeQuery).ToList();
            var xcolsAllJoin = Compiler.getQueryJoinColumns(xQubeQuery).ToList();
            xcolsAll.AddRange(xcolsAllJoin);

          

            xQubeUnion.Add(xQubeQuery);




        }




        private static void SearchSourceLinksEndStep(VQuery source, List<int> wached, List<int> wachedLocal, ref List<string> dimsToSearch
            , List<string> foundNames, SortedList<string, SecondaryLinkInfo> secondaryRoots
            ,ref List<DimensionPathTreeNode> list
             , SortedList<string, string> dimensionNames
            ,VQube qubeElement)
        {

            wached.AddRange(wachedLocal);
            wachedLocal.Clear();
            dimsToSearch = dimsToSearch.Where(e => !foundNames.Contains(e)).ToList();
            foreach (var v in secondaryRoots.Values)
            {
                v.Found = v.FoundOne;
            }



            list = list.SelectMany(e => e.Childs).ToList();
            bool notFound = false;
            var dimsToSearchNotFound = dimsToSearch.ToList();
            if (qubeElement.P_StarScheme == TextConst.AVBool.True) {
                HashSet<string> foundSecondary = new HashSet<string>();
                foreach (SecondaryLinkInfo v in secondaryRoots.Values) {
                    string dim_name = v.DimName;
                    if (!foundSecondary.Contains(dim_name)) {
                        foundSecondary.Add(dim_name);
                    }
                }
                foreach (string dn in dimsToSearchNotFound.ToArray()) {
                    if (!foundSecondary.Contains(dn)) {
                        notFound = true;
                    } else {
                        dimsToSearchNotFound.Remove(dn);
                    }
                }
            }

            if (list.Count == 0 || notFound)
            {
                if (list.Count == 0)
                {
                   dimsToSearchNotFound= dimensionNames.Keys.Where(e => !foundNames.Contains(e)).ToList();
                }
                if (dimsToSearchNotFound.Any())
                {
                    var msg = string.Format(@"Для {0} не найдены измерения: {1}",
                        source.Attribute(TextConst.AName.Name).Value,
                        string.Join(", ", dimsToSearchNotFound));

                    throw new VCompilerException(msg, source, null);
                }
            }
        }


        private static void SearchSourceLinks(VQuery source, SortedList<string, string> dimensionNames
            , DimensionPathTreeNode parentLookUpNode, List<DimensionPathTreeNode> found
            ,   List<VQuery> storages,VQube qubeElement)
        {


            //if (source.XName == "ur_inkasso")
            //{

            //}
            var wached = new List<int>();
            var wachedLocal = new List<int>();
            var foundNames = new List<string>();


            var selfDim = source.GetDimension();

            string selfDimName = null;
            bool complete = false;

            int plFound = 0;

            if (selfDim != null)
            {
                selfDimName = selfDim.P_IdName;
            }


            //if (source.P_Name == "sr_facras")
            //{

            //}

            if (selfDimName != null && dimensionNames.Keys.Contains(selfDimName))
            {
                plFound = 1;

                //foundNames.Add(selfDimension);
                //found.Add(parentLookUpNode);
                foundNames.Add(selfDimName);
                parentLookUpNode.Alias = dimensionNames[selfDimName];
                parentLookUpNode.DimensionName = selfDimName;
                if (dimensionNames.Count == 1)
                {
                    complete = true;
                }

                // found.Add(parentLookUpNode);

            }
            else
            {
                parentLookUpNode.Alias = source.P_IdName + TextConst.Pfx.AddDim;
            }


            var secondaryRoots = new SortedList<string, SecondaryLinkInfo>();



            var dimsToSearch = dimensionNames.Keys.ToList();
            if (!complete)
            {
                //if (found.Count < dimensionNames.Count)
                //{
                SearchLinkInRelationCollection(source, source.EntityType, dimensionNames, parentLookUpNode, found, wached, wachedLocal, foundNames, dimsToSearch, storages, secondaryRoots, qubeElement);
                //}
                var list = new List<DimensionPathTreeNode>();
                list.Add(parentLookUpNode);

                //dimsToSearch = dimsToSearch.Where(e => !foundNames.Contains(e)).ToList();

                //list = parentLookUpNode.Childs;

                SearchSourceLinksEndStep(source, wached, wachedLocal,ref dimsToSearch, foundNames, secondaryRoots,ref list, dimensionNames,qubeElement);

                while (foundNames.Count != dimensionNames.Count)
                {
                    foreach (DimensionPathTreeNode node in list)
                    {
                        node.SecondaryOnly = node.Parent.SecondaryOnly;
                        VEntityType entityType=null;
                        bool isSecondaryPathPart = false;
                        if (!node.IsTime)
                        {
                            if (node.Element.P_IsFinalDimension == TextConst.AVBool.True)//!!! не проверено
                            {

                                var path = node.GetPath();
                                isSecondaryPathPart = secondaryRoots.Values.Where(e => e.FinalPath != path && e.FinalPath.StartsWith(path) && !e.Found).Any();

                                var si = secondaryRoots.Values.Where(e => e.FinalPath != path).FirstOrDefault();


                                if (si == null || !si.IsFinalDimension)
                                {
                                    node.SecondaryOnly = true;
                                }
                            }
                            else if (node.SecondaryOnly)
                            {
                                var path = node.GetPath();
                                isSecondaryPathPart = secondaryRoots.Values.Where(e => e.FinalPath != path && e.FinalPath.StartsWith(path) && !e.Found).Any();

                            }
                           
                               





                            if (!node.SecondaryOnly || isSecondaryPathPart)//!!! не проверено
                            {
                                if (node.IsLink)
                                {
                                    
                                        entityType = ((node.Element as VQueryCall).Query() as VQuery).EntityType;
                                    
                                }
                                else if (!node.IsColumn)
                                {
                                       entityType = (node.Element as VQueryCall).Query().EntityType;
                                    
                                }
                                else
                                {
                                    entityType = XmlReports.Environment.GetQueryByKeyDimensionName(node.DimensionName).EntityType;
                                }



                                SearchLinkInRelationCollection(source, entityType, dimensionNames, node, found, wached, wachedLocal, foundNames, dimsToSearch, storages, secondaryRoots, qubeElement);
                            }
                        }
                    }
                    SearchSourceLinksEndStep(source, wached, wachedLocal,ref dimsToSearch, foundNames, secondaryRoots,ref list, dimensionNames,qubeElement);
                    //wached.AddRange(wachedLocal);
                    //wachedLocal.Clear();
                    //dimsToSearch = dimsToSearch.Where(e => !foundNames.Contains(e)).ToList();
                    //foreach (var v in secondaryRoots.Values)
                    //{
                    //    v.Found = v.FoundOne;
                    //}

                   

                    //list = list.SelectMany(e => e.Childs).ToList();


                    //if (list.Count == 0)
                    //{
                    //    var msg = string.Format(@"Для {0} не найдены измерения: {1}",
                    //        source.Attribute(TextConst.AName.Name).Value,
                    //        string.Join(", ", dimensionNames.Keys.Where(e => !foundNames.Contains(e))));

                    //    throw new VCompilerException(msg, source, null);
                    //}
                }
            }
            //if (source.XName == "ur_inkasso")
            //{

            //}
            //parentLookUpNode.SetUnused();
            DimensionPathTreeNode.SetUsedAndFound(found);
            parentLookUpNode.Used = true;
            parentLookUpNode.RemoveUnused();
        }


        private static DimensionPathTreeNode CopyUsed(DimensionPathTreeNode rout, DimensionPathTreeNode copy = null)
        {

            copy = rout.Copy();

            foreach (DimensionPathTreeNode child in rout.Childs)
            {
                if (child.Used)
                {
                    var childCopy = CopyUsed(child);
                    copy.Childs.Add(childCopy);
                    childCopy.Parent = copy;
                }

            }
            return copy;
        }

        private static void SetUnused(DimensionPathTreeNode rout)
        {
            rout.Used = false;

            foreach (DimensionPathTreeNode child in rout.Childs)
            {
                SetUnused(child);
            }
        }





        //private static List<LookUpTreeNode> BuildRouts(LookUpTreeNode rout, List<LookUpTreeNode> found)
        //{
        //    SetUsed(found);
        //    RemoveUnusedLooUpNodes(rout);

        //    var foundCombined = new SortedList<string, List<LookUpTreeNode>>();

        //    foreach (LookUpTreeNode node in found)
        //    {
        //        if (!foundCombined.ContainsKey(node.DimensionName))
        //        {
        //            foundCombined.Add(node.DimensionName, new List<LookUpTreeNode>());
        //        }
        //        foundCombined[node.DimensionName].Add(node);
        //    }
        //    var routPoints = new List<List<LookUpTreeNode>>();

        //    CombileRootPoints(foundCombined, routPoints, null, 0);

        //    var routs = new List<LookUpTreeNode>();


        //    //var cnt = CalculateRootsCount(parentLookUpNode);


        //    var  routs1 = new List<LookUpTreeNode>();

        //    foreach (List<LookUpTreeNode> routPoint in routPoints)
        //    {
        //        SetUnused(rout);
        //        SetUsed(routPoint);

        //        var rout1 = CopyUsed(rout);
        //        routs1.Add(rout1);
        //    }
        //    return routs1;
        //}

        //private static void CombileRootPoints(SortedList<string, List<LookUpTreeNode>> foundCombined, List<List<LookUpTreeNode>> rootPoints,List<LookUpTreeNode> rootPoint, int level)
        //{

        //    if (foundCombined.Count ==level)
        //    {
        //        return;
        //    }

        //    var nodeGroup = foundCombined.ElementAt(level).Value;
        //    if (rootPoint == null)
        //    {
        //        rootPoint = new List<LookUpTreeNode>();
        //        rootPoints.Add(rootPoint);
        //    }

        //    var inRootPoint = new List<LookUpTreeNode>();

        //    foreach (LookUpTreeNode node in rootPoint)
        //    {
        //        inRootPoint.Add(node);
        //    }


        //    int i = 0;
        //    foreach (LookUpTreeNode node in nodeGroup)
        //    {
        //        if (i != 0)
        //        {
        //            rootPoint = new List<LookUpTreeNode>();
        //            rootPoints.Add(rootPoint);
        //            foreach (LookUpTreeNode node1 in inRootPoint)
        //            {
        //                rootPoint.Add(node1);
        //            }
        //        }
        //        rootPoint.Add(node);
        //        CombileRootPoints(foundCombined, rootPoints, rootPoint, level + 1);
        //        i++;
        //    }


        //}


        //private static int CalculateRootsCount(LookUpTreeNode node)
        //{
        //   var list=new SortedList<string, int>();
        //    int cnt=1;
        //   foreach (var child in node.Childs.ToList())
        //   {
        //       var i = CalculateRootsCount(child);
        //       if (!list.ContainsKey(child.DimensionName))
        //       {
        //           list.Add(child.DimensionName, i);
        //       }
        //       else
        //       {
        //           list[child.DimensionName] = list[child.DimensionName] + i;
        //       }

        //   }
        //   foreach (int i in list.Values)
        //   {
        //       cnt = cnt * i;
        //   }

        //    return cnt;
        //}


        private static List<DimensionPathTreeNode> GetLeavs(DimensionPathTreeNode node)
        {

            var leavs = new List<DimensionPathTreeNode>();
            if (node.Childs.Count == 0)
            {
                leavs.Add(node);
            }
            else
            {

                foreach (var child in node.Childs.ToList())
                {
                    var leavs1 = GetLeavs(child);
                    foreach (var leav in leavs1)
                    {
                        leavs.Add(leav);
                    }
                }
            }
            return leavs;
        }

        private static void RemoveUnusedLooUpNodes(DimensionPathTreeNode node)
        {

            if (!node.Used)
            {
                node.Parent.Childs.Remove(node);
            }
            else
            {
                foreach (var child in node.Childs.ToList())
                {
                    RemoveUnusedLooUpNodes(child);
                }
            }
        }

        //private static Stack<XElement> _qubeContent = new Stack<XElement>();

        //public static void ResetQubeContent()
        //{
        //    _qubeContent = new Stack<XElement>();
        //}

        //public static void PopQubeContent()
        //{
        //    if (_qubeContent.Any())
        //    {
        //        _qubeContent.Pop();
        //    }
        //}
        //public static XElement PeekQubeContent()
        //{
        //    if (_qubeContent.Any())
        //    {
        //        _qubeContent.Pop();
        //    }
        //}

        private static void AddLinksXml(DimensionPathTreeNode node, XElement element, List<VQueryCall> links, XElement fromNode, XElement commonWhere)
        {
            foreach (DimensionPathTreeNode childNode in node.Childs)
            {
                if (childNode.Used && !childNode.IsTime)
                {
                    XElement link=null;
                    XElement linkRoot = null;
                    if (childNode.IsLink)
                    {
                        
                            var eName = childNode.Element.Name.LocalName;
                            if (eName == TextConst.EName.DLink)
                            {
                                eName = TextConst.EName.ELink;
                            }
                            link = new XElement(eName);
                            Cmn.copyAttributes(childNode.Element, link);

                            var parent = childNode.Element.GetParent();
                            var link1 = link;
                            linkRoot = link;
                            while ((parent is VLink) || (parent is VELink))
                            {
                                eName = parent.Name.LocalName;
                                if (eName == TextConst.EName.DLink)
                                {
                                    eName = TextConst.EName.ELink;
                                }
                                linkRoot = new XElement(eName);

                                // Это убираю, вроде сделал универсально.

                                //if ((parent is VELink))  
                                //{

                                //    if ((parent as VELink).Query().GetQubeElement() != null) // Обработан тоько частный случай пропихивания условий 2
                                //    {
                                //        var qc = new XElement(TextConst.EName.QubeContent);
                                //        var cw = new XElement(commonWhere);
                                       
                                //        qc.Add(cw);
                                //        linkRoot.Add(qc);

                                //    }
                                //}


                                Cmn.copyAttributes(parent, linkRoot);
                                linkRoot.Add(link1);
                                link1 = linkRoot;
                                link1.SetAttributeValue(TextConst.AName.As, parent.XName + TextConst.Pfx.AddDim1);
                                parent = parent.GetParent();
                            }

                           
                        element.Add(linkRoot);
                    }
                    else if (!childNode.IsColumn)
                    {
                        link = new XElement(TextConst.EName.Link);
                        link.SetAttributeValue(TextConst.AName.Name, childNode.Element.XName);
                        linkRoot = link;


                        if (childNode.StorageName != null)
                        {
                            linkRoot = new XElement(TextConst.EName.ELink, new XAttribute(TextConst.AName.Name, childNode.StorageName));
                            linkRoot.Add(link);
                        }
                        else
                        {
                            linkRoot = link;
                        }

                        element.Add(linkRoot);
                        // link.SetAttributeValue(TextConst.AName.As, childNode.Element.P_Dimension);
                    }
                    else
                    {
                        link = new XElement(TextConst.EName.Query);
                        var dim = XmlReports.Environment.GetDimension(childNode.DimensionName);
                        var qry = dim.Query();
                        link.SetAttributeValue(TextConst.AName.Name, qry.P_Name);
                        link.SetAttributeValue(TextConst.AName.Join, TextConst.AVJoin.LeftOuter);
                        var call = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Equal));

                        var col1 = new XElement(TextConst.EName.Column);

                        col1.SetAttributeValue(TextConst.EName.Table, element.Attribute(TextConst.AName.As).Value);
                        col1.SetAttributeValue(TextConst.EName.Column, childNode.Element.XName);
                        call.Add(col1);

                        col1 = new XElement(TextConst.EName.Column);

                        col1.SetAttributeValue(TextConst.EName.Table, childNode.Alias);
                        col1.SetAttributeValue(TextConst.EName.Column, qry.KeyColumn().XName);
                        call.Add(col1);

                        link.Add(call);

                        linkRoot = link;

                        fromNode.Add(link);

                       
                    }
                    link.SetAttributeValue(TextConst.AName.As, childNode.Alias);
                    //link.SetAttributeValue(TextConst.AName.LinkMultiplicatePoint, TextConst.AVBool.True);
                    // ???
                    link.SetAttributeValue(TextConst.AName.Dimension, childNode.DimensionName);
                    foreach (VSXElement alink in links)
                    {
                        if (alink.XName == link.Attribute(TextConst.AName.As).Value)
                        {
                            link.Add(alink.Elements());
                        }
                    }
                    //if (childNode.DimensionName == mainDimension)
                    //{
                    //    link.SetAttributeValue(TextConst.AName.MainInEditor, TextConst.AVBool.True);
                    //}


                    AddLinksXml(childNode, link, links, fromNode,commonWhere);
                }
            }
        }

        private class SecondaryLinkInfo
        {
           public string DimName;
           public string InitialPath;
           public string FinalPath;
           public bool FoundOne=false;
           public bool Found = false;
           public bool IsFinalDimension = false;
           
        }


        //private static bool IsSecondaryPathPart(SortedList<string, SecondaryLinkInfo> secondaryRoots, DimensionPathTreeNode node)
        //{
        //    var path = node.GetPath();
        //    var v = secondaryRoots.Values.Where(e => e.FinalPath != path && e.FinalPath.StartsWith(path)).Any();
        //    return v;

        //}

        //private static bool IsNotFinalSecondaryPathPart(SortedList<string, SecondaryLinkInfo> secondaryRoots, DimensionPathTreeNode node)
        //{
        //    var path = node.GetPath();
        //    var v= secondaryRoots.Values.Where(e =>e.FinalPath!=path && e.FinalPath.StartsWith(path)).Any();
        //    if (!v)
        //    {
        //        var si = secondaryRoots.Values.Where(e => e.FinalPath != path).FirstOrDefault();
        //        if (si != null)
        //        {
        //            v = si.IsFinalDimension;
        //        }
        //    }
        //    return v;
          
        //}
        private static bool IsSecondaryPathPart(SortedList<string, SecondaryLinkInfo> secondaryRoots,DimensionPathTreeNode parent, string dimName)
        {
            var path = parent.GetPath() + "." + dimName;
            return secondaryRoots.Values.Where(e => !e.Found && e.FinalPath.StartsWith(path)).Any();
        }

        private static void processSecondaryLink(VQueryCall rel
            , List<int> wachedLocal, SortedList<string, SecondaryLinkInfo> secondaryRoots, DimensionPathTreeNode parentLookUpNode)
        {
            wachedLocal.Add(rel.GetUniqueKey());
            var parentLink = (rel.GetParent() as VQueryCall);
            DimensionPathTreeNode parentlookUpNode2 = null;
            var pdimName = parentLink.P_Dimension;
            string parentPath = "";
            if (parentLink is VDimLink && pdimName != "")
            {
                if (!secondaryRoots.ContainsKey(pdimName))
                {
                    processSecondaryLink(parentLink, wachedLocal, secondaryRoots, parentLookUpNode);
                }
                parentPath = secondaryRoots[pdimName].FinalPath;

            }
            else
            {
               
                if (pdimName == "")
                {
                    var qry = parentLink.Query();
                    if (qry == null)
                    {
                        return;
                    }
                    pdimName = qry.GetDimension().XName;


                }
                //parentlookUpNode2 = parentLookUpNode.Childs.Where(c => c.DimensionName == pdimName).FirstOrDefault();
                //if (parentlookUpNode2 == null)
                //{
                //    throw new InvalidOperationException(TextConst.EName.DimLink + " может находиться только под ссылкой, для которой определено измерение");
                //    // доделать при необходимости


                //}

                if (pdimName == "")
                {
                    throw new InvalidOperationException(TextConst.EName.DimLink + " может находиться только под ссылкой, для которой определено измерение");
                    // доделать при необходимости
                }
                parentPath = parentLookUpNode.GetPath() + "." + pdimName;
            }

            var path = parentPath + "." + rel.P_Dimension;

            if (secondaryRoots.ContainsKey(rel.P_Dimension))
            {
                if (secondaryRoots[rel.P_Dimension].FinalPath == parentLookUpNode.GetPath() + "." + rel.P_Dimension)
                {// когда dimlink ссылается на dimlink

                    //if (rel.P_Dimension == "ipr_kod_titul_ip")
                    //{
                    //}
                    //if (rel.P_Dimension == "ipr_kod_titul_ip_teh_pr")
                    //{
                    //}
                    secondaryRoots[rel.P_Dimension].FinalPath = path;// = new Tuple<string, string>(secondaryRoots[rel.P_Dimension].Item1, path);
                }
            }
            else
            {
                var sli = new SecondaryLinkInfo();
                sli.InitialPath = parentLookUpNode.GetPath() + "." + rel.P_Dimension;
                sli.FinalPath = path;
                sli.DimName = rel.P_Dimension;
                if (rel.P_IsFinalDimension == TextConst.AVBool.True)
                {
                    sli.IsFinalDimension = true;
                }
                secondaryRoots.Add(rel.P_Dimension, sli);
            }

        }

        private static void SearchLinkInRelationCollection(VQuery source, VEntityType entityType
            , SortedList<string, string> dimensionNames, DimensionPathTreeNode parentLookUpNode
            , List<DimensionPathTreeNode> found
            , List<int> wached
            , List<int> wachedLocal
             , List<string> foundNames
            , List<string> dimsToSearch, List<VQuery> storages
            , SortedList<string, SecondaryLinkInfo> secondaryRoots
            , VQube qubeElement
            )
        {

          
            //storages.Add(entityType.Query.GetEnvironment().GetQuery("ur_graf_dp")); // пробный вариант точечного использования отдельных "хранилищ"  с небольшим набором колонок. Пока связи измерений с множественным путем
            SortedList<string, DimensionPathTreeNode> storedDimensions = new  SortedList<string, DimensionPathTreeNode>();
            if (storages.Any())
            {
                string selfDimName = null;

                var selfDim = entityType.Query.GetDimension();
                if (selfDim != null)
                {
                    selfDimName = selfDim.P_IdName;
                }

              
                if (selfDimName != null)
                {
                    foreach (VQuery qry in storages)
                    {
                        var dl = qry.EntityType.ParentDimensionLinks();
                        if (dl.Where(e => e.P_Dimension == selfDimName).Any())
                        {

                            foreach (VRelation rel in dl)
                            {
                                if (rel.P_Dimension != selfDimName)
                                {
                                    if (!storedDimensions.ContainsKey(rel.P_Dimension))
                                    {

                                        if (!wached.Contains(rel.GetUniqueKey()))
                                        {
                                            wachedLocal.Add(rel.GetUniqueKey());
                                            var lookUpNode = new DimensionPathTreeNode();
                                            lookUpNode.StorageName = qry.P_Name;
                                            lookUpNode.Element = rel;
                                            parentLookUpNode.Childs.Add(lookUpNode);
                                            lookUpNode.Parent = parentLookUpNode;
                                           
                                            if (dimsToSearch.Contains(rel.P_Dimension))
                                            {

                                               
                                                found.Add(lookUpNode);
                                                lookUpNode.Alias = dimensionNames[rel.P_Dimension];
                                            }
                                            else
                                            {
                                                lookUpNode.Alias = rel.P_Dimension + TextConst.Pfx.AddDim;
                                            }
                                            lookUpNode.DimensionName = rel.P_Dimension;
                                            storedDimensions.Add(rel.P_Dimension, lookUpNode);
                                        }
                                    }
                                }
                            }
                        }
                    }
                }


            }

            bool isAllSecondary = true;

            foreach (string name in dimsToSearch)
            {
                if (!secondaryRoots.ContainsKey(name))
                {
                    isAllSecondary = false;
                    break;
                }
            }

            var sources2 = entityType.SecondaryExtDimensionLinks();
            List<string> additionalParentsNames = new List<string>();
            foreach (VQueryCall rel in sources2)
            {

                if (isAllSecondary && !IsSecondaryPathPart(secondaryRoots, parentLookUpNode, rel.P_Dimension))
                {
                    continue;
                }

                if (rel.P_IsPrivateDimension != TextConst.AVBool.True || source.EntityType == entityType || secondaryRoots.ContainsKey(rel.P_Dimension))
                {
                    if (!storedDimensions.ContainsKey(rel.P_Dimension))
                    {

                      
                        if (!wached.Contains(rel.GetUniqueKey()) || IsSecondaryPathPart(secondaryRoots, parentLookUpNode, rel.P_Dimension))
                        {
                         
                            processSecondaryLink(rel,wachedLocal,secondaryRoots,parentLookUpNode);
                            //string parentPath = "";
                            //wachedLocal.Add(rel.GetUniqueKey());
                            //var parentLink = (rel.GetParent() as VQueryCall);
                            //DimensionPathTreeNode parentlookUpNode2 = null;
                            //var pdimName = parentLink.P_Dimension;

                            //if (parentLink is VDimLink && pdimName != "")
                            //{
                            //    parentPath = secondaryRoots[pdimName].FinalPath;

                            //}
                            //else
                            //{
                            //    if (pdimName == "")
                            //    {
                            //        pdimName = parentLink.Query().GetDimension().XName;


                            //    }
                            //    //parentlookUpNode2 = parentLookUpNode.Childs.Where(c => c.DimensionName == pdimName).FirstOrDefault();
                            //    //if (parentlookUpNode2 == null)
                            //    //{
                            //    //    throw new InvalidOperationException(TextConst.EName.DimLink + " может находиться только под ссылкой, для которой определено измерение");
                            //    //    // доделать при необходимости


                            //    //}

                            //    if (pdimName == "")
                            //    {
                            //        throw new InvalidOperationException(TextConst.EName.DimLink + " может находиться только под ссылкой, для которой определено измерение");
                            //        // доделать при необходимости
                            //    }
                            //    parentPath = parentLookUpNode.GetPath() + "." + pdimName;
                            //}

                            //var path = parentPath + "." + rel.P_Dimension;

                            //if (secondaryRoots.ContainsKey(rel.P_Dimension))
                            //{
                            //    if (secondaryRoots[rel.P_Dimension].FinalPath == parentLookUpNode.GetPath() + "." + rel.P_Dimension)
                            //    {// когда dimlink ссылается на dimlink

                            //        if (rel.P_Dimension == "kod_folders")
                            //        {
                            //        }
                            //        secondaryRoots[rel.P_Dimension].FinalPath = path;// = new Tuple<string, string>(secondaryRoots[rel.P_Dimension].Item1, path);
                            //    }
                            //}
                            //else
                            //{
                            //    var sli = new SecondaryLinkInfo();
                            //    sli.InitialPath = parentLookUpNode.GetPath() + "." + rel.P_Dimension;
                            //    sli.FinalPath = path;
                            //    sli.DimName = rel.P_Dimension;
                            //    if (rel.P_IsFinalDimension == TextConst.AVBool.True)
                            //    {
                            //        sli.IsFinalDimension = true;
                            //    }
                            //    secondaryRoots.Add(rel.P_Dimension, sli);
                            //}

                        }
                    }
                    else
                    {
                        storedDimensions[rel.P_Dimension].Checked = true;
                        if (dimsToSearch.Contains(rel.P_Dimension))
                        {
                            if (!foundNames.Contains(rel.P_Dimension))
                            {
                                foundNames.Add(rel.P_Dimension);

                            }
                        }
                    }
                }
            }

            List<VRelation> sources = entityType.ParentDimensionLinks();


            //if (entityType.Query.P_Name == "ipr_titul_ip")
            //{

            //}
            foreach (VRelation rel in sources)
            {
                if (isAllSecondary && !IsSecondaryPathPart(secondaryRoots, parentLookUpNode, rel.P_Dimension))
                {
                    continue;
                }
                var spp = IsSecondaryPathPart(secondaryRoots, parentLookUpNode, rel.P_Dimension);
                if (rel.P_IsPrivateDimension != TextConst.AVBool.True || source.EntityType == entityType || spp /*secondaryRoots.ContainsKey(rel.P_Dimension)*/)
                {
                    if (!storedDimensions.ContainsKey(rel.P_Dimension))
                    {
                        // foundLocal.Add(rel.P_Dimension);


                        if (!wached.Contains(rel.GetUniqueKey()) || spp)
                        {
                            
                            wachedLocal.Add(rel.GetUniqueKey());
                            var lookUpNode = new DimensionPathTreeNode();
                            lookUpNode.Element = rel;
                            parentLookUpNode.Childs.Add(lookUpNode);
                            lookUpNode.Parent = parentLookUpNode;

                            lookUpNode.DimensionName = rel.P_Dimension;
                            if (dimsToSearch.Contains(rel.P_Dimension))
                            {

                                if (!foundNames.Contains(rel.P_Dimension))
                                {
                                    foundNames.Add(rel.P_Dimension);

                                }
                                found.Add(lookUpNode);
                                lookUpNode.Alias = dimensionNames[rel.P_Dimension];

                            }
                            else
                            {
                                lookUpNode.Alias = rel.P_Dimension + TextConst.Pfx.AddDim;
                            }

                          
                        }
                    }
                    else
                    {
                        storedDimensions[rel.P_Dimension].Checked = true;
                        if (dimsToSearch.Contains(rel.P_Dimension))
                        {
                            if (!foundNames.Contains(rel.P_Dimension))
                            {
                                foundNames.Add(rel.P_Dimension);

                            }
                        }
                    }
                }
            }


            List<VSXElement> dimCols = entityType.ColumnDimensionLinks();

            foreach (VSXElement rel in dimCols)
            {
                if (isAllSecondary && !IsSecondaryPathPart(secondaryRoots, parentLookUpNode, rel.P_Dimension))
                {
                    continue;
                }

                var spp = IsSecondaryPathPart(secondaryRoots, parentLookUpNode, rel.P_Dimension);
                if (rel.P_IsPrivateDimension != TextConst.AVBool.True || source.EntityType == entityType || spp /*secondaryRoots.ContainsKey(rel.P_Dimension)*/)
                {
                /*if (rel.P_IsPrivateDimension != TextConst.AVBool.True || source.EntityType == entityType || secondaryRoots.ContainsKey(rel.P_Dimension))
                {*/
                    if (!storedDimensions.ContainsKey(rel.P_Dimension) )
                    {
                        //foundLocal.Add(col.P_Dimension);
                        if (!wached.Contains(rel.GetUniqueKey()) ||spp)
                        {
                            wachedLocal.Add(rel.GetUniqueKey());
                            var lookUpNode = new DimensionPathTreeNode();
                            lookUpNode.Element = rel;
                            parentLookUpNode.Childs.Add(lookUpNode);
                            lookUpNode.Parent = parentLookUpNode;
                            lookUpNode.IsColumn = true;
                            var dim = XmlReports.Environment.GetDimension(rel.P_Dimension);


                            if (dim.P_TimeType != "")
                            {
                                lookUpNode.IsTime = true;
                            }
                            else
                            {
                            }

                            if (dimsToSearch.Contains(rel.P_Dimension))
                            {
                                if (!foundNames.Contains(rel.P_Dimension))
                                {
                                    foundNames.Add(rel.P_Dimension);

                                }
                                found.Add(lookUpNode);
                                //lookUpNode.Alias = dimensionNames[col.P_Dimension];

                                lookUpNode.Alias = dimensionNames[rel.P_Dimension];

                            }
                            else
                            {
                                lookUpNode.Alias = rel.P_Dimension + TextConst.Pfx.AddDim;
                            }
                            lookUpNode.DimensionName = rel.P_Dimension;
                            //else
                            //{
                            //    lookUpNode.Alias = col.P_Dimension + TextConst.Pfx.AddDim;
                            //}
                        }
                    }
                    else
                    {
                        storedDimensions[rel.P_Dimension].Checked = true;
                        if (dimsToSearch.Contains(rel.P_Dimension))
                        {
                            if (!foundNames.Contains(rel.P_Dimension))
                            {
                                foundNames.Add(rel.P_Dimension);

                            }
                        }
                    }
                }
            }

           




            var sources1 = entityType.PrimaryExtDimensionLinks();
            foreach (VQueryCall rel in sources1)
            {
                if (isAllSecondary && !IsSecondaryPathPart(secondaryRoots, parentLookUpNode, rel.P_Dimension))
                {
                    continue;
                }
                var spp = IsSecondaryPathPart(secondaryRoots, parentLookUpNode, rel.P_Dimension);
                if (rel.P_IsPrivateDimension != TextConst.AVBool.True || source.EntityType == entityType || spp /*secondaryRoots.ContainsKey(rel.P_Dimension)*/)
                {
                //if (rel.P_IsPrivateDimension != TextConst.AVBool.True || source.EntityType == entityType || secondaryRoots.ContainsKey(rel.P_Dimension))
                //{
                    if (!storedDimensions.ContainsKey(rel.P_Dimension))
                    {
                        //foundLocal.Add(rel.P_Dimension);
                        if (!wached.Contains(rel.GetUniqueKey()) || spp)
                        {
                            wachedLocal.Add(rel.GetUniqueKey());
                            var lookUpNode = new DimensionPathTreeNode();
                            lookUpNode.Element = rel;
                            parentLookUpNode.Childs.Add(lookUpNode);
                            lookUpNode.Parent = parentLookUpNode;
                            lookUpNode.IsLink = true;
                            if (rel is VELink)
                            {
                                lookUpNode.IsBack = true;
                            }

                            if (rel.GetAncestorsAndSelf().Where(e => e is VELink).Any())
                            {
                                lookUpNode.IsContainsBackReferences = true;
                            }


                            if (dimsToSearch.Contains(rel.P_Dimension))
                            {
                                if (!foundNames.Contains(rel.P_Dimension))
                                {
                                    foundNames.Add(rel.P_Dimension);

                                }
                                found.Add(lookUpNode);
                                lookUpNode.Alias = dimensionNames[rel.P_Dimension];

                            }
                            else
                            {
                                lookUpNode.Alias = rel.P_Dimension + TextConst.Pfx.AddDim;
                            }

                            lookUpNode.DimensionName = rel.P_Dimension;
                        }
                    }
                    else
                    {
                        storedDimensions[rel.P_Dimension].Checked = true;
                        if (dimsToSearch.Contains(rel.P_Dimension))
                        {
                            if (!foundNames.Contains(rel.P_Dimension))
                            {
                                foundNames.Add(rel.P_Dimension);

                            }
                        }
                    }
                }
            }




           


            foreach (DimensionPathTreeNode node in storedDimensions.Values.Where(e => !e.Checked).ToList())
            {
                parentLookUpNode.Childs.Remove(node);
                wachedLocal.Remove(node.Element.GetUniqueKey());
                found.Remove(node);
            }


            



            foreach (DimensionPathTreeNode node in  parentLookUpNode.Childs.ToArray())
            {
                
                if (node.DimensionName != null)
                {
                    if (secondaryRoots.ContainsKey(node.DimensionName))
                    {
                        var pth = node.GetPath();
                        if (secondaryRoots[node.DimensionName].FinalPath != pth)
                        {
                            // parentLookUpNode.Childs.Remove(node);
                            // wachedLocal.Remove(node.Element.GetUniqueKey());

                            var pp = secondaryRoots.Values.Where(v => v.FinalPath.Contains(pth + ".") || v.FinalPath.Equals(pth)).ToArray();
                            foreach (var v1 in pp)
                            {
                                throw new VCompilerException("Несоответствие путей " + source.XName + v1.FinalPath + " " + source.XName + secondaryRoots[node.DimensionName].FinalPath, null, null);
                            }
                            if (found.Contains(node))
                            {
                                found.Remove(node);
                                if (!found.Where(e => e.DimensionName == node.DimensionName).Any())
                                {

                                    foundNames.Remove(node.DimensionName);
                                }

                               
                            }
                            if (parentLookUpNode.Childs.Contains(node))
                            {

                                parentLookUpNode.Childs.Remove(node);
                            }
                        }
                        else
                        {
                            if (node.DimensionName == "kod_mat_isp")
                            {
                            }
                            node.SecondaryPath = secondaryRoots[node.DimensionName].InitialPath;
                            secondaryRoots[node.DimensionName].FoundOne = true;
                        }
                    }
                    else
                    {
                        if (parentLookUpNode.SecondaryOnly)
                        {
                            if (found.Contains(node))
                            {
                                found.Remove(node);
                                if (!found.Where(e => e.DimensionName == node.DimensionName).Any())
                                {
                                    foundNames.Remove(node.DimensionName);
                                }
                            }
                        }
                    }
                }
            }


            

        }


        private static VQuery recombine(VQuery query)
        {



            VQuery newQuery = VSXElement.Get<VQuery>(new XElement(query));


            //newQuery.environment = query.environment;
            var mainLink = new XElement(TextConst.EName.Link);
            var mainSource = newQuery.MainSource();
            mainLink.SetAttributeValue(TextConst.AName.Name, mainSource.Query().GetDimension().P_IdName);
            mainLink.SetAttributeValue(TextConst.AName.As, mainSource.XName);
            //  Cmn.CopyAttribute(mainSource, mainLink, TextConst.AName.MainInEditor);

            mainLink.Add(mainSource.Elements());

            var qube = new XElement(TextConst.EName.Qube);
            qube.Add(mainLink);
            Cmn.CopyAttribute(query, qube, TextConst.AName.StarScheme);
            Cmn.CopyAttribute(query, qube, TextConst.AName.SingleWay);
            Cmn.CopyAttribute(query, qube, TextConst.AName.MergeDimsets);
            mainLink.SetAttributeValue(TextConst.AName.AllRows, TextConst.AVBool.True);

            string keyColName = mainSource.Query().KeyColumn().XName;
            mainSource.ReplaceWith(qube);

            XElement notNullCond = Factory.NewCall(TextConst.AVFunction.IsNotNull);
            notNullCond.Add(new XAttribute(AName.dont_push, TextConst.AVBool.True));
            XElement notNullCol = Factory.NewColumn(mainSource.XName, keyColName);
            notNullCond.Add(notNullCol);

            XElement origWhere = newQuery.Element(EName.where);

            if (origWhere == null) {
                newQuery.Add(new XElement(TextConst.EName.Where));
                origWhere = newQuery.Element(TextConst.EName.Where);
            }
            Compiler.extendWhereByAnd(origWhere, notNullCond);
            var xwhere = new XElement(origWhere);
            var xwherePush = new XElement(xwhere);

            // Убираются условия с фактами т.к. их нельзя протолкнуть в куб, or не проверен
            foreach (XElement fact in xwherePush.Descendants(TextConst.EName.Fact).ToList())
            {
                var prtToRem = fact.Ancestors(TextConst.EName.Call).Where(e =>
                    Cmn.GetAttrValue(e, TextConst.AName.Function) == TextConst.AVFunction.Or
                  || Cmn.GetAttrValue(e, TextConst.AName.Optional) == TextConst.AVBool.True
                    ).LastOrDefault();


                //Убрал - вроде не нужно , выкидывались все условия в poisk_byt1

                //if (prtToRem != null)
                //{
                //    if (prtToRem.Parent.Elements(TextConst.EName.Call).Where(e => TextConst.AVFuncArray.TrueFalse.Contains(Cmn.GetAttrValue(e, TextConst.AName.Function))).Any())
                //    {
                //        prtToRem = prtToRem.Parent;
                //    }
                //}

                if (prtToRem == null)
                {
                    prtToRem = fact.Ancestors(TextConst.EName.Call).Where(e => e.Parent != null && Cmn.GetAttrValue(e.Parent, TextConst.AName.Function) == TextConst.AVFunction.And).FirstOrDefault();
                    if (prtToRem == null)
                    {
                        prtToRem = fact.Ancestors(TextConst.EName.Call).Where(e => e.Parent != null && e.Parent.Name.LocalName == TextConst.EName.Where).FirstOrDefault();
                    }
                }
                if (prtToRem != null)
                {
                    if (prtToRem.Parent != null)
                    {
                        prtToRem.Remove();
                    }
                }

            }

            foreach (XElement emptyCall in xwherePush.Descendants(TextConst.EName.Call)
                .Where(e1 => Cmn.GetAttrValue(e1, TextConst.AName.Function) == TextConst.AVFunction.And).Where(e => !e.Elements().Any()).ToList())
            {
                emptyCall.Remove();
            }

            if (xwherePush.Elements().Any())
            {
                qube.Add(xwherePush);
            }


            foreach (XElement xcall in xwhere.Descendants(TextConst.EName.Call).ToList())
            {
                if (xcall.Parent.Name.LocalName == TextConst.EName.Where || Cmn.GetAttrValue(xcall.Parent, TextConst.AName.Function) == TextConst.AVFunction.And)
                {


                    if (!xcall.Descendants(TextConst.EName.Fact).Any())
                    {
                        var xor = xcall.Ancestors(TextConst.EName.Call).Where(e =>
                            Cmn.GetAttrValue(e, TextConst.AName.Function) == TextConst.AVFunction.Or
                               || Cmn.GetAttrValue(e, TextConst.AName.Optional) == TextConst.AVBool.True
                            ).LastOrDefault();
                        if (xor == null)
                        {
                            bool canRem = true;
                            if (TextConst.AVFuncArray.TrueFalse.Contains(Cmn.GetAttrValue(xcall, TextConst.AName.Function)))
                            {
                                if (xcall.Parent.Descendants(TextConst.EName.Fact).Any())
                                {
                                    canRem = false;
                                }
                            }

                            if (canRem)
                            {
                                xcall.Remove();
                            }
                        }
                    }
                }
            }

            //foreach (XElement emptyCall in xwhere.Descendants(TextConst.EName.Call)
            //   .Where(e1 => Cmn.GetAttrValue(e1, TextConst.AName.Function) == TextConst.AVFunc.And).Where(e => !e.Elements().Any()).ToList())
            //{
            //    emptyCall.Remove();
            //}

            if (xwhere.Elements().Any())
            {

                //if (!xwhere.Elements().Where(e=>Cmn.GetAttrValue(e,TextConst.AName.Optional)!="").Any())
                //{ 

                //}

                newQuery.Element(TextConst.EName.Where).ReplaceWith(xwhere);





            }
            else
            {
                newQuery.Element(TextConst.EName.Where).Remove();
            }
            newQuery.Descendants(TextConst.EName.Fact).Attributes(TextConst.EName.Table).Remove();
            return newQuery;


        }

        

        private class DimensionPathTreeNode
        {
            public VSXElement Element;
            private DimensionPathTreeNode parent =null;
            public string SecondaryPath = null;
            public DimensionPathTreeNode Parent {
                get
                {
                    if (parent != null && parent.Childs.Count == 0)
                    {

                    }
                    return parent;
                }
                set
                {
                    parent = value;

                    if (value != null && parent.Childs.Count == 0)
                    {

                    }
                }
            }
            public List<DimensionPathTreeNode> Childs = new List<DimensionPathTreeNode>();
            public bool Used = false;
            public bool Found = false;
            public string Alias = null;
            public bool IsBack = false;
            public bool IsContainsBackReferences = false;
            public bool IsLink = false;
            public bool IsColumn = false;
            public bool IsTime = false;
            public string DimensionName = null;
            public string StorageName = null;
            public object Tag = null;
            public bool Checked = false;
            public bool SecondaryOnly = false;
            public static DimensionPathTreeNode SearchCommonParent(DimensionPathTreeNode node1, DimensionPathTreeNode node2)
            {

                var parent1 = node1.Parent;
                var parent2 = node2.Parent;


                while (parent1 != null && parent2 != null && parent1 != parent2)
                {
                    parent1 = parent1.Parent;
                    parent2 = parent2.Parent;
                }



                if (parent1 == parent2)
                {
                    return parent1;
                }
                else
                {
                    return null;
                }
            }

            public DimensionPathTreeNode Copy()
            {
                var copy = new DimensionPathTreeNode();
                copy.Element = Element;
                copy.Used = Used;
                copy.Found = Found;
                copy.Alias = Alias;
                copy.IsBack = IsBack;
                copy.IsContainsBackReferences = IsContainsBackReferences;
                copy.IsLink = IsLink;
                copy.IsColumn = IsColumn;
                copy.IsTime = IsTime;
                copy.DimensionName = DimensionName;
                copy.StorageName = StorageName;
                copy.Tag = Tag;
                copy.SecondaryPath = SecondaryPath;
                return copy;
            }
            private string _path=null;
            public string GetPath()
            {
                if (_path == null)
                {
                    var s = "";
                    if (Parent != null)
                    {
                        var dname = DimensionName;
                        //if (string.IsNullOrEmpty(dname))
                        //{
                        //    dname = "@" + Alias;
                        //}
                        s = Parent.GetPath() + "." + dname;
                    }
                    _path = s;
                }
                return _path;
            }

            public DimensionPathTreeNode CopyAll()
            {

                var copy = Copy();

                foreach (DimensionPathTreeNode child in Childs)
                {
                    var childCopy = CopyUsed(child);
                    copy.Childs.Add(childCopy);
                    childCopy.Parent = copy;
                }
                return copy;
            }




            public static void SetUsedAndFound(List<DimensionPathTreeNode> routPoint)
            {


                foreach (DimensionPathTreeNode node in routPoint)
                {
                    node.Found = true;
                    var node1 = node;//.Parent;
                    while (node1 != null)
                    {
                        node1.Used = true;
                        node1 = node1.Parent;
                    }
                }
            }

            public void RemoveUnused()
            {
                var node = this;
                if (!node.Used)
                {
                    node.Parent.Childs.Remove(node);
                }
                else
                {
                    foreach (var child in node.Childs.ToList())
                    {
                        child.RemoveUnused();
                    }
                }
            }

            //public void SetUnused()
            //{
            //    this.Used=false;
            //    foreach (var child in this.Childs.ToList())
            //    {
            //        child.SetUnused();
            //    }
            //}
            public List<DimensionPathTreeNode> Split()
            {
                //var splited = SplitOne();
                var list = new List<DimensionPathTreeNode>();
                var newList = new List<DimensionPathTreeNode>();
                newList.Add(this);
                while (newList.Count > list.Count)
                {
                    list = newList;
                    newList = new List<DimensionPathTreeNode>();
                    foreach (var rout in list)
                    {
                        var listLoc = rout.SplitOne();
                        if (listLoc != null)
                        {
                            newList.AddRange(listLoc);
                        }
                        else
                        {
                            newList.Add(rout);
                        }
                    }
                }
                list = newList;
                return list;
            }

            public List<DimensionPathTreeNode> SplitOne()
            {

                var splitPoint = FindSplitPoint();
                if (splitPoint == null)
                {
                    return null;
                }


                foreach (var node in splitPoint)
                {
                    node.Tag = node.GetHashCode();
                }


                var list = new List<DimensionPathTreeNode>();

                foreach (var node in splitPoint)
                {
                    var otherMarks = splitPoint.Where(e => e != node).Select(e1 => (int)e1.Tag).ToList();
                    var newRoot = CopyAll();

                    var itemsToRemove = newRoot.AllNodes().Where(e => e.Tag != null && otherMarks.Contains((int)e.Tag));

                    foreach (var item in itemsToRemove)
                    {
                        item.Parent.Childs.Remove(item);
                    }
                    list.Add(newRoot);
                }
                return list;


            }

            public List<DimensionPathTreeNode> FindSplitPoint()
            {

                foreach (DimensionPathTreeNode node in AllNodes())
                {
                    var dimCnt = new SortedList<string, List<DimensionPathTreeNode>>();
                    foreach (DimensionPathTreeNode child in node.Childs)
                    {
                        if (!dimCnt.ContainsKey(child.DimensionName))
                        {
                            dimCnt.Add(child.DimensionName, new List<DimensionPathTreeNode>());
                        }
                        dimCnt[child.DimensionName].Add(child);
                    }

                    foreach (var list in dimCnt.Values)
                    {
                        if (list.Count > 1)
                        {
                            //var list1 =new  List<LookUpTreeNode>();
                            //foreach (LookUpTreeNode node1 in AllNodes())
                            //{

                            //    if (node1.DimensionName == list[0].DimensionName && node1.Depth()==list[0].Depth())
                            //    {
                            //        list1.Add(node1);

                            //    }
                            //}
                            //return list1;
                            return list;
                        }
                    }
                }
                return null;
            }
            public List<DimensionPathTreeNode> AllNodes()
            {
                var list = new List<DimensionPathTreeNode>();
                allNodes(list);
                return list;
            }

            private void allNodes(List<DimensionPathTreeNode> list)
            {
                list.Add(this);

                foreach (DimensionPathTreeNode child in Childs)
                {
                    child.allNodes(list);
                }

            }


            //private int Level()
            //{
            //    int i = 0;
            //    var node=this;
            //    while (node.Parent != null)
            //    {
            //        node = node.Parent;
            //        i++;
            //    }
            //    return i;
            //}

            public static string PathFromTo(DimensionPathTreeNode parent, DimensionPathTreeNode child)
            {
                string s = child.DimensionName;
                var node = child.Parent;
                while (node != parent)
                {

                    s = node.DimensionName + "." + s;
                    node = node.Parent;

                }
                return s;
            }

            public XElement ViewAsXml()
            {



                string linkInfo = "";
                if (Element != null)
                {
                    if (IsLink)
                    {


                        var parent = Element.GetParent();
                        var link1 = this.Element;
                        var q = ".";
                        linkInfo = link1.XName;

                        while ((parent is VLink) || (parent is VELink))
                        {
                            linkInfo = parent.XName + q + linkInfo;



                            parent = parent.GetParent();
                            q = ".";
                        }

                    }
                    else
                    {
                        linkInfo = Element.XName;
                        // link.SetAttributeValue(TextConst.AName.As, childNode.Element.P_Dimension);
                    }
                }





                var xelement = new XElement("node");

                xelement.SetAttributeValue("dimension", DimensionName);
                xelement.SetAttributeValue("link-info", linkInfo);
                if (SecondaryPath != null)
                {
                    xelement.SetAttributeValue("path", SecondaryPath);
                }
                if (StorageName != null)
                {
                    xelement.SetAttributeValue("used-storage", StorageName);
                }
                if (Childs != null)
                {
                    foreach (var child in Childs)
                    {
                        var xchild = child.ViewAsXml();
                        xelement.Add(xchild);
                    }
                }
                return xelement;
            }

            public static XElement ViewListAsXml(List<DimensionPathTreeNode> list)
            {
                var xelement = new XElement("root");


                foreach (var child in list)
                {
                    var xchild = child.ViewAsXml();
                    xelement.Add(xchild);
                }

                return xelement;
            }

        }

        internal class DimensionCollection
        {
            public List<string> Dimensions = new List<string>();
            public SortedList<string, VFact.FactDependantceInfo> Facts = new SortedList<string, VFact.FactDependantceInfo>();
        }



        //public class DimensionNameInfo
        //{
        //    public static string BuildName(string dimension, string isNonOutput)
        //    {
        //        if (isNonOutput == "")
        //        {
        //            return dimension;
        //        }
        //        else
        //        {
        //            return dimension + "|" + isNonOutput;
        //        }
        //    }

        //    public static string GetDimNameFromFullName(string fullName)
        //    {
        //        return fullName.Split('|')[0];
        //    }

        //    public static bool IsNonoutput(string fullName)
        //    {
        //        return fullName.Split('|').Count() > 1;
        //    }
        //}

        
        //public class FactInfo
        //{
        //    public string Name;
        //    public string Cumulate;
        //    public string Dimset;
        //    public FactInfo()
        //    {

        //    }
        //    public FactInfo(string name, string cumul, string dimset)
        //    {
        //        Name = name;
        //        Cumulate = cumul;
        //        Dimset = dimset;

        //        if (dimset != null)
        //        {

        //        }
        //    }
        //    public static FactInfo FromString(string s)
        //    {
        //        var fi = new FactInfo();
        //        var ss = s.Split(',');


        //        string[] ss1 = null;
        //        if (ss.Count() > 1)
        //        {
        //            fi.Name = ss[0];
        //            ss1 = ss[1].Split('|');
        //            fi.Cumulate = ss1[0];

        //        }
        //        else
        //        {

        //            ss1 = ss[0].Split('|');
        //            fi.Name = ss1[0];
        //        }
        //        if (ss1.Count() > 1)
        //        {
        //            fi.Dimset = ss1[1];
        //        }
        //        return fi;
        //    }

        //    public string BuildName(string fact, string cumul, string dimset)
        //    {
        //        string name = fact;
        //        if (!string.IsNullOrEmpty(cumul))
        //        {
        //            name += TextConst.Pfx.ScopeExp + cumul;
        //        }
        //        if (!string.IsNullOrEmpty(dimset))
        //        {
        //            name += TextConst.Pfx.DsExp + dimset;
        //        }
        //        return name;
        //    }
        //    public bool IsCumulate()
        //    {
        //        return !string.IsNullOrEmpty(Cumulate);
        //    }

        //    public bool IsDimset()
        //    {
        //        return !string.IsNullOrEmpty(Dimset);
        //    }

        //    public string BuildName()
        //    {
        //        return BuildName(Name, Cumulate, Dimset);
        //    }
        //    public string BuildPfx()
        //    {
        //        return BuildName("", Cumulate, Dimset);
        //    }
        //}


        


    }


}
