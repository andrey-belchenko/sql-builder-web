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


namespace sql.builder.DataApi
{
    internal partial class VQubeUtils
    {
        static string LvlPfx = "_lvl_";
        //internal class LLevelInfo
        //{
        //    string linkAlias;
        //    int globLevel;
        //    int locLavel;
        //    string keyName;
        //}
        public static void ProcessQubeTrees(VQuery query)
        {
            
            foreach (VSXElement treeLink in query.GetQubeElement().AllQubeLinks().Where(e => e.P_IsTree == TextConst.AVBool.True).ToList())
            {
                var levSql = "select max(level) l from {0} connect by prior {1}={2} start with {2} is null";

                var dim = XmlReports.Environment.GetDimension(treeLink.P_CalledQuery);
                var srcqry = dim.Query();
                var qryName = srcqry.MainSource().P_Name;
                var keyName = srcqry.KeyColumn().XName;
                var parentKeyName = dim.P_ParentFieldName;
                var levSql1 = String.Format(levSql, qryName, keyName, parentKeyName);
               
                int levels = Convert.ToInt32(db.ExecuteDataTable(levSql1).Rows[0][0]);
                ProcessQubeTree(query, treeLink, levels);

            }
        }
        public static void ProcessQubeTree(VQuery query,VSXElement treeLink , int levels)
        {
            query.Columns();
            VColumn[] cols = Compiler.getQueryColumnsSel(query).ToList().SelectAsArray(VSXElement.Get<VColumn>);
            
               
                var dim = XmlReports.Environment.GetDimension(treeLink.P_CalledQuery);
                var srcqry = dim.Query();
                var qryName = srcqry.P_Name;
                var keyName = srcqry.KeyColumn().XName;
                var parentKeyName = dim.P_ParentFieldName;
              

                var linearTreeQuery = CreateLinearTreeQuery(qryName, keyName, parentKeyName, levels);

                var treeAlias = treeLink.XName + "_tree";

                linearTreeQuery.SetAttributeValue(TextConst.AName.Join, TextConst.AVJoin.LeftOuter);
                linearTreeQuery.SetAttributeValue(TextConst.AName.As, treeAlias);

                var xcall = new XElement(TextConst.EName.Call);
                xcall.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Equal);
                var xcol = new XElement(TextConst.EName.Column);
                xcol.SetAttributeValue(TextConst.AName.Table, treeLink.XName);
                xcol.SetAttributeValue(TextConst.AName.Column, keyName);
                xcall.Add(xcol);

                xcol = new XElement(TextConst.EName.Column);
                xcol.SetAttributeValue(TextConst.AName.Table, treeAlias);
                xcol.SetAttributeValue(TextConst.AName.Column, keyName);
                xcall.Add(xcol);

                linearTreeQuery.Add(xcall);

                var fromEl = query.GetSelfFromSections().First();
                fromEl.Add(linearTreeQuery);

                var grSets = query.Elements(TextConst.EName.Grouping).Descendants(TextConst.EName.Grset)
                 .Where(
                 e => e.Elements(TextConst.EName.Group).Elements(TextConst.EName.SourceLink)
                     .Where(e1 => e1.Attribute(TextConst.AName.Table).Value == treeLink.XName).Any()
                 ).ToList();

                XElement firstGrSet = null;
                XElement pevGrSet = null;
                foreach (var grSet in grSets)
                {
                    var keyCol = cols.Where(e => Cmn.GetAttrValue(e, TextConst.AName.Table) == treeLink.XName && Cmn.GetAttrValue(e, TextConst.AName.Column)==keyName).FirstOrDefault();
                    var keyAlias = keyCol.Attribute(TextConst.AName.Column).Value;
                    if (keyCol.Attribute(TextConst.AName.As) != null)
                    {
                        keyAlias = keyCol.Attribute(TextConst.AName.As).Value;
                    }

                    var grSet1 = new XElement(grSet);
                    var childs = grSet1.Elements(TextConst.EName.Grset).ToList();
                    childs.Remove();

                    List<XElement> childsAddGrlevels = new List<XElement>();

                    List<XElement> childsAddCond = new List<XElement>();
                    string keyInfo = "";
                    var curGr = grSet;
                    int il = 0;
                    while (curGr.Name.LocalName != TextConst.EName.Grouping)
                    {
                        var grLinks = curGr.Elements(TextConst.EName.Group).Elements().ToList();
                        childsAddGrlevels.AddRange(grLinks);
                        childsAddCond.AddRange(curGr.Elements(TextConst.EName.Where).Elements().ToList());
                        curGr = curGr.Parent;
                        if (il > 0)
                        {
                            
                        }
                        var q = "";
                       
                        foreach (XElement grLink in grLinks)
                        {

                            if (grLink.Attribute(TextConst.AName.Table).Value != treeLink.XName)
                            {
                                var keyCol1 = query.Columns().Where(e => Cmn.GetAttrValue(e, TextConst.AName.Table) == grLink.Attribute(TextConst.AName.Table).Value).First();
                                var keyAlias1 = keyCol1.XName;
                                keyInfo += q + keyAlias1;
                                q = ",";
                            }
                           
                        }
                        il++;
                    }


                    for (int lev = 0; lev < levels; lev++)
                    {
                        
                        var newGrSet = new XElement(grSet1);
                        //  var grAlias = grSet.Attribute(TextConst.AName.As).Value;

                        var keyInfo1 = keyInfo;
                        if (keyInfo1 != "")
                        {
                            keyInfo1 += ",";
                        }
                        keyInfo1 += keyAlias + LvlPfx + lev.ToString();
                        newGrSet.SetAttributeValue(TextConst.AName.Key,keyInfo1);
                        newGrSet.SetAttributeValue(TextConst.AName.TreeLevel, lev.ToString());
                        if (lev == 0)
                        {
                            firstGrSet = newGrSet;
                        }
                        else
                        {
                            newGrSet.Elements(TextConst.EName.Where).Remove();

                            var xwhere = new XElement(TextConst.EName.Where);


                            var xcond = new XElement(TextConst.EName.Call);
                            xcond.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.IsNotNull);

                            xwhere.Add(xcond);
                            xcol = new XElement(TextConst.EName.Column);
                            xcol.SetAttributeValue(TextConst.AName.Table, TextConst.AVTable.Ths);
                            xcol.SetAttributeValue(TextConst.AName.Column, keyAlias+LvlPfx+lev.ToString());
                            xcond.Add(xcol);
                            newGrSet.Add(xwhere);
                            newGrSet.Attributes(TextConst.AName.Parent).Remove();
                            newGrSet.Attributes(TextConst.AName.ParentKey).Remove();
                        }
                        //if (lev + 1 < levels)
                        //{
                        //    foreach (XElement gr in newGrSet.Elements(TextConst.EName.Grset))
                        //    {


                        //        var xwhere = gr.Element(TextConst.EName.Where


                        //            );
                        //        XElement prt = null;
                        //        if (xwhere == null)
                        //        {
                        //            xwhere = new XElement(TextConst.EName.Where  );
                        //            prt = xwhere;
                                   
                        //            gr.Add(xwhere);
                        //        }
                        //        else
                        //        {
                        //            var els = xwhere.Elements();
                        //            els.Remove();
                        //            xwhere.Add(
                        //                new XElement(TextConst.EName.Call
                        //                   , new XAttribute(TextConst.AName.Function, TextConst.AVFunc.And)
                        //                   )
                        //            );
                        //            prt = xwhere.Elements().First();
                        //            prt.Add(els);
                        //        }
                        //        var xcond = new XElement(TextConst.EName.Call);
                        //        xcond.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunc.IsNull);

                        //        prt.Add(xcond);
                        //        xcol = new XElement(TextConst.EName.Column);
                        //        xcol.SetAttributeValue(TextConst.AName.Table, TextConst.AVTable.Ths);
                        //        xcol.SetAttributeValue(TextConst.AName.Column, keyAlias + LvlPfx + (lev+1).ToString());
                        //        xcond.Add(xcol);
                             
                        //    }
                        //}

                        //foreach (XElement gr1 in newGrSet.DescendantsAndSelf(TextConst.EName.Grset))
                        //{
                            var gr1 = newGrSet;
                            var alias = gr1.Attribute(TextConst.AName.As).Value;
                            var newAlias1 = alias + LvlPfx + lev.ToString();
                            gr1.SetAttributeValue(TextConst.AName.As, newAlias1);
                            if (gr1.Attribute(TextConst.AName.Name) == null)
                            {
                                gr1.SetAttributeValue(TextConst.AName.Name, alias);
                            }

                        //}
                        var gsrclink = newGrSet.Elements(TextConst.EName.Group).Elements().Where(e => e.Attribute(TextConst.AName.Table).Value == treeLink.XName).FirstOrDefault();


                        gsrclink.SetAttributeValue(TextConst.AName.Table, treeLink.XName + LvlPfx + lev.ToString());

                        if (lev > 0)
                        {
                            gsrclink.ElementsAfterSelf().Remove();
                            gsrclink.ElementsBeforeSelf().Remove();
                        }
                        if (pevGrSet != null)
                        {
                            pevGrSet.Add(newGrSet);
                        }

                        pevGrSet = newGrSet;
                    }
                    grSet.AddAfterSelf(firstGrSet);
                    grSet.Remove();


                    foreach (XElement gr in childs)
                    {
                        

                        if (childsAddGrlevels.Any())
                        {
                            if (!gr.Elements(TextConst.EName.Group).Any())
                            {
                                gr.Add(new XElement(TextConst.EName.Group));
                            }

                            foreach (var sl in childsAddGrlevels)
                            {
                                var sl1 = new XElement(sl);
                                sl1.SetAttributeValue(TextConst.AName.Parent, TextConst.AVBool.True);
                                gr.Element(TextConst.EName.Group).Add(sl1);
                            }
                            //gr.Element(TextConst.EName.Group).Add(childsAddGrlevels);
                        }

                        if (childsAddCond.Any())
                        {
                            var childsAddCond1 = childsAddCond.ToList();
                            childsAddCond1.AddRange(gr.Elements(TextConst.EName.Where).Elements().ToList());
                            gr.Elements(TextConst.EName.Where).Remove();

                            gr.Add(new XElement(TextConst.EName.Where,new XElement(TextConst.EName.Call,new XAttribute(TextConst.AName.Function,TextConst.AVFunction.And))));

                            gr.Element(TextConst.EName.Where).Elements().First().Add(childsAddCond1);

                        }

                        var keyInfo1 = keyInfo;
                        if (keyInfo1 != "")
                        {
                            keyInfo1 += ",";
                        }
                        keyInfo1 += keyAlias ;
                      
                        gr.SetAttributeValue(TextConst.AName.ParentKey, keyInfo1);
                        gr.SetAttributeValue(TextConst.AName.Parent, grSet.Attribute(TextConst.AName.As).Value);
                        query.Element(TextConst.EName.Grouping).Add(gr);
                       
                    }

                }
           

                SortedList<string,List<VSXElement>> multiplicateColumns=new SortedList<string,List<VSXElement>>();

                foreach (XElement link in treeLink.DescendantsAndSelf())
                {
                    var alias = link.Attribute(TextConst.AName.Name).Value;
                    if (link.Attribute(TextConst.AName.As) != null)
                    {
                        alias = link.Attribute(TextConst.AName.As).Value;
                    }
                    var cols2 = cols.Where(e => e.TreeSourceName() == alias).ToList();

                    var cols1 = new List<VSXElement>();
                    foreach (VColumn el in cols2)
                    {
                       
                        var el1=VSXElement.Get( el.AncestorsAndSelf().Where(e => e.Parent.Name != EName.call).First());
                        cols1.Add(el1);
                    }
                    cols1 = cols1.Distinct().ToList();
                    multiplicateColumns.Add(alias, cols1);



                }

                List<XElement> exprsToRemove = new List<XElement>();
                HashSet<string> readyMultiColumn = new HashSet<string>();
                for (int lev = 0; lev < levels; lev++)
                {
                    var qryCall = new XElement(TextConst.EName.Query);
                    qryCall.Add(treeLink.Elements());
                    var newAlias=treeLink.XName+LvlPfx+lev.ToString();
                    qryCall.SetAttributeValue(TextConst.AName.Name, qryName);
                    qryCall.SetAttributeValue(TextConst.AName.As, treeLink.XName);
                   
                    foreach (XElement link in qryCall.DescendantsAndSelf())
                    {
                        var alias = link.Attribute(TextConst.AName.Name).Value;
                        if (link.Attribute(TextConst.AName.As) != null)
                        {
                            alias = link.Attribute(TextConst.AName.As).Value;
                        }

                        var newAlias1 = alias + LvlPfx + lev.ToString();


                        var cols1 = multiplicateColumns[alias];

                        
                        foreach (var expr in cols1)
                        {
                            MultiplicateExpression(expr,alias,exprsToRemove,lev,newAlias,treeLink,readyMultiColumn,levels);
                        }
                        link.SetAttributeValue(TextConst.AName.As, newAlias);
                    }

              
             
                
                    qryCall.SetAttributeValue(TextConst.AName.Join, TextConst.AVJoin.LeftOuter);
                    xcall = new XElement(TextConst.EName.Call);
                    xcall.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Equal);
                    xcol = new XElement(TextConst.EName.Column);
                    xcol.SetAttributeValue(TextConst.AName.Table,treeAlias);
                    xcol.SetAttributeValue(TextConst.AName.Column, keyName + lev.ToString());
                    xcall.Add(xcol);

                    xcol = new XElement(TextConst.EName.Column);
                    xcol.SetAttributeValue(TextConst.AName.Table, newAlias);
                    xcol.SetAttributeValue(TextConst.AName.Column, keyName);
                    xcall.Add(xcol);
                    qryCall.Add(xcall);
                    fromEl.Add(qryCall);
                }
                exprsToRemove.Remove();



                var newLCol = new XElement(TextConst.EName.Column);
                newLCol.SetAttributeValue(TextConst.AName.Table, treeAlias);
                newLCol.SetAttributeValue(TextConst.AName.Column, keyName + TextConst.Pfx.Level);
                newLCol.SetAttributeValue(TextConst.AName.As, treeLink.XName + TextConst.Pfx.Level);
                newLCol.SetAttributeValue(TextConst.AName.Group, TextConst.AVGroup.Min);
                query.Element(TextConst.EName.Select).Add(newLCol);
               // multiplicateColumns.SelectMany(e => e.Value).Remove();

            
        }

        public static void MultiplicateExpression(
            VSXElement expr
            ,string treeLinkAlias
            ,List<XElement> exprsToRemove
            ,int lev
            ,string newTreeLinkAlias
            ,VSXElement treeLink
            ,HashSet<string> readyMultiColumn
            ,int levels)
        {
            var newExpr = VSXElement.Get(new XElement(expr));

            newExpr.VirtualParent = expr.GetParent();




            var colAlias = expr.XName;

            var newExprAlias = colAlias + LvlPfx + lev.ToString();

            if (readyMultiColumn.Contains(newExprAlias))
            {
                return;
            }else{
                readyMultiColumn.Add(newExprAlias);
            }
            XElement newExprT = newExpr;

            var origCols = expr.GetDescedantsAndSelfP(EName.column).Cast<VColumn>().Where(c => c.TreeSourceName() == treeLinkAlias).ToList();


            if (origCols.Count == 0)
            {
                var newCol = new XElement(TextConst.EName.Column);

                Cmn.copyAttributes(newExpr, newCol);
                var colExpr = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.If));


                var xcall1 = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Equal));

                var xcol1 = new XElement(TextConst.EName.Column
                    , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
                    , new XAttribute(TextConst.AName.Column, treeLink.XName + TextConst.Pfx.Level)

                    );
                var xconst = new XElement(TextConst.EName.Const, new XText(lev.ToString()));
                xcall1.Add(xcol1);
                xcall1.Add(xconst);


                newCol.SetAttributeValue(TextConst.AName.Table, TextConst.AVTable.Ths);
                colExpr.Add(xcall1);

                newCol.SetAttributeValue(TextConst.AName.Group, TextConst.AVGroup.Empty);

                colExpr.Add(newCol);

                newExprT = colExpr;
                newExprT.SetAttributeValue(TextConst.AName.Group, expr.P_Group);
            }
            else
            {

                var newCols = newExpr.GetDescedantsAndSelfP(EName.column).Cast<VColumn>().Where(c => c.TreeSourceName() == treeLinkAlias).ToList();
                int i = 0;

                foreach (VColumn newCol in newCols)
                {
                    VColumn origCol = origCols[i];
                    i++;
                    var lev1 = lev;
                    if (newCol.TreeSpecSourceName() == TextConst.TreeSources.Child)
                    {
                        lev1 += 1;
                    }
                    var newColAlias = newCol.P_Column + LvlPfx + lev1.ToString();
                    if (newCol.P_Table == TextConst.AVTable.Ths)
                    {
                        if (!readyMultiColumn.Contains(newColAlias))
                        {
                            var scol = expr.GetParent().GetElementsP().Where(e => e.XName == newCol.XName).First();


                            MultiplicateExpression(scol, treeLinkAlias, exprsToRemove, lev, newTreeLinkAlias, treeLink, readyMultiColumn, levels);
                        }


                        newCol.P_Column = newColAlias;
                    }
                    else if (newCol.TreeSpecSourceName() == null)
                    {
                        newCol.P_Table = newTreeLinkAlias;
                    }
                    else
                    {

                        if (!exprsToRemove.Contains(expr))
                        {
                            exprsToRemove.Add(expr);
                        }
                        XElement colExpr = null;

                        if (lev1 >= levels)
                        {
                            colExpr = new XElement(TextConst.EName.Const, new XText("null"));
                            newCol.ReplaceWith(colExpr);
                        }
                        else
                        {

                            if (!readyMultiColumn.Contains(newColAlias))
                            {
                                var scol = expr.GetParent().GetElementsP().Where(e => e.XName == newCol.XName).First();

                                MultiplicateExpression(scol, treeLinkAlias, exprsToRemove, lev1, newTreeLinkAlias, treeLink, readyMultiColumn, levels);
                            }


                            newCol.P_Column = newColAlias;
                            newCol.P_Table = TextConst.AVTable.Ths;
                            newCol.SetAttributeValue(TextConst.AName.Group, "");

                        }
                    }
                }
            }
            newExprT.SetAttributeValue(TextConst.AName.TreeLevelColumn, treeLink.XName + TextConst.Pfx.Level);
            expr.SetAttributeValue(TextConst.AName.TreeLevelColumn, treeLink.XName + TextConst.Pfx.Level);

            newExprT.SetAttributeValue(TextConst.AName.Level, lev.ToString());
            expr.SetAttributeValue(TextConst.AName.Level, lev.ToString());
            if (treeLink.P_IsTreeSplitCols == "1")
            {
                newExprT.SetAttributeValue(TextConst.AName.TreeOriginalColumn, colAlias + "!");
                expr.SetAttributeValue(TextConst.AName.TreeOriginalColumn, colAlias + "!");
            }
            else
            {

                newExprT.SetAttributeValue(TextConst.AName.TreeOriginalColumn, colAlias);
                expr.SetAttributeValue(TextConst.AName.TreeOriginalColumn, colAlias);
            }
            newExprT.SetAttributeValue(TextConst.AName.As, newExprAlias);

           
            expr.AddBeforeSelf(newExprT);

           

        }

        public static XElement CreateLinearTreeQuery(string queryName, string keyName, string parentKeyName, int levels)
        {

            var qry = new XElement(TextConst.EName.Query,
                new XElement (TextConst.EName.Select),
                    new XElement (TextConst.EName.From)
                );
            var qalias="a";
            var colalias = keyName;
            XElement xcol;
            xcol = new XElement(TextConst.EName.Column);
            xcol.SetAttributeValue(TextConst.AName.Table, qalias + "0");
            xcol.SetAttributeValue(TextConst.AName.Column, keyName);
            XElement xcase;
            qry.Element(TextConst.EName.Select).Add(xcol);
            for (int lev = 0; lev < levels; lev++)
            {

                var qryCall = new XElement(TextConst.EName.Query);
                qryCall.SetAttributeValue(TextConst.AName.Name, queryName);
                qryCall.SetAttributeValue(TextConst.AName.As, qalias + lev.ToString());
             
                if (lev > 0)
                {
                    qryCall.SetAttributeValue(TextConst.AName.Join, TextConst.AVJoin.LeftOuter);
                    var xcall = new XElement(TextConst.EName.Call);
                    xcall.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Equal);
                     xcol = new XElement(TextConst.EName.Column);
                    xcol.SetAttributeValue(TextConst.AName.Table, qalias + (lev - 1).ToString());
                    xcol.SetAttributeValue(TextConst.AName.Column, parentKeyName);
                    xcall.Add(xcol);

                    xcol = new XElement(TextConst.EName.Column);
                    xcol.SetAttributeValue(TextConst.AName.Table, qalias + lev.ToString());
                    xcol.SetAttributeValue(TextConst.AName.Column, keyName);
                    xcall.Add(xcol);

                    qryCall.Add(xcall);
                }

                qry.Element(TextConst.EName.From).Add(qryCall);
                 xcase = new XElement(TextConst.EName.Call);
                xcase.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Case);
                xcase.SetAttributeValue(TextConst.AName.As, colalias+lev.ToString());
                for (int lev1 = 1; lev1 < levels; lev1++)
                {
                    var xwhen = new XElement(TextConst.EName.Call);
                    xwhen.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.When);
                    var xcond = new XElement(TextConst.EName.Call);
                    xcond.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.IsNull);

                    xwhen.Add(xcond);
                     xcol = new XElement(TextConst.EName.Column);
                    xcol.SetAttributeValue(TextConst.AName.Table, qalias + lev1.ToString());
                    xcol.SetAttributeValue(TextConst.AName.Column, keyName);
                    xcond.Add(xcol);


                    var lev2 = lev1 - lev - 1;
                    if (lev2 > -1)
                    {
                        xcol = new XElement(TextConst.EName.Column);
                        xcol.SetAttributeValue(TextConst.AName.Table, qalias + (lev2).ToString());
                        xcol.SetAttributeValue(TextConst.AName.Column, keyName);
                    }
                    else
                    {
                        xcol = new XElement(TextConst.EName.Const, new XText("null"));
                    }
                    xwhen.Add(xcol);

                    xcase.Add(xwhen);
                }
                var xelse = new XElement(TextConst.EName.Call);
                xelse.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Elese);
                xcol = new XElement(TextConst.EName.Column);
                xcol.SetAttributeValue(TextConst.AName.Table, qalias + (levels-1-lev).ToString());
                xcol.SetAttributeValue(TextConst.AName.Column, keyName);
                xelse.Add(xcol);
                xcase.Add(xelse);

                qry.Element(TextConst.EName.Select).Add(xcase);
            }





            xcase = new XElement(TextConst.EName.Call);
            xcase.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Case);
            xcase.SetAttributeValue(TextConst.AName.As, colalias+TextConst.Pfx.Level);
            xcase.SetAttributeValue(TextConst.AName.Type, TextConst.AVDataType.Number);
            for (int lev1 = levels-1; lev1 >=0; lev1--)
            {
                var xwhen = new XElement(TextConst.EName.Call);
                xwhen.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.When);
                var xcond = new XElement(TextConst.EName.Call);
                xcond.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.IsNotNull);

                xwhen.Add(xcond);
                xcol = new XElement(TextConst.EName.Column);
                xcol.SetAttributeValue(TextConst.AName.Table, qalias + lev1.ToString());
                xcol.SetAttributeValue(TextConst.AName.Column, keyName);
                xcond.Add(xcol);


                xcol = new XElement(TextConst.EName.Const, new XText(lev1.ToString()));
                xwhen.Add(xcol);

                xcase.Add(xwhen);
            }
            qry.Element(TextConst.EName.Select).Add(xcase);

            return qry;

        }


    }


}
