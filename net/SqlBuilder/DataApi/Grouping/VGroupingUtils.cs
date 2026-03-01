using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public static class VGroupingUtils
    {
        // private static string checkPfx = "_check";
        public static void ExtendQueryAsGroupingDetail(XElement query, XElement groupingQuery, string groupName, DataRow row, DataColumn column, bool applyReportConds, bool use_zeros = false)
        {



            bool isOuterCond = false;
            bool isCheckExpr = false;

            string detExtaCondAlias = "det_xtra_cond";
            //XElement ifCond = null;

            if (applyReportConds || column != null)
            {
                var srcQube = groupingQuery.Descendants(TextConst.EName.Qube).FirstOrDefault();

                if (srcQube != null)
                {
                    var trgQube = query.Descendants(TextConst.EName.Qube).FirstOrDefault();
                    if (trgQube != null)
                    {
                        if (applyReportConds)
                        {
                            var xwhere = srcQube.Elements(TextConst.EName.Where);
                            if (!trgQube.Elements(TextConst.EName.Where).Any())
                            {
                                trgQube.Add(xwhere);
                            }
                            foreach (var srcDimset in srcQube.Elements(TextConst.EName.DimSet))
                            {
                                xwhere = srcDimset.Elements(TextConst.EName.Where);
                                var trgDimset = trgQube.Elements(TextConst.EName.DimSet).FirstOrDefault(e => Cmn.GetAttrValue(e, TextConst.AName.As) == Cmn.GetAttrValue(srcDimset, TextConst.AName.As));
                                if (trgDimset != null)
                                {
                                    if (!trgDimset.Elements(TextConst.EName.Where).Any())
                                    {
                                        trgDimset.Add(xwhere);
                                    }
                                }
                            }
                        }
                        if (column != null)
                        {
                            trgQube.Parent.Parent.Add(srcQube.Parent.Parent.Elements(TextConst.EName.Expressions));
                        }
                    }
                }
            }

            //  var checkPfx1 = "";
            var colCondname = "";
            if (column != null)
            {

                colCondname = column.ColumnName;
                IList<XElement> flds = query.Elements(EName.select).Elements().ToList();
                XElement checkExpr = flds.SearchByAttribute(AName.@as, column.ColumnName);
                string sAnyColumn = "[column]";
                if (checkExpr == null)
                {
                    checkExpr = flds.SearchByAttribute(AName.@as, sAnyColumn);
                    if (checkExpr == null)
                    {
                        checkExpr = flds.FirstOrDefault(e => e.Descendants().Any(e1 => e1.AttrOrEmpty(AName.@as) == sAnyColumn));
                        if (checkExpr != null)
                        {
                            colCondname = checkExpr.Attribute(AName.@as).Value;
                        }
                    }
                    else
                    {
                        checkExpr.SetAttributeValue(AName.@as, column.ColumnName);
                    }
                    if (checkExpr != null)
                    {
                        foreach (XElement col in checkExpr.Descendants())
                        {
                            if (col.AttrOrEmpty(AName.@as) == sAnyColumn)
                            {
                                col.SetAttributeValue(AName.@as, column.ColumnName);
                            }
                        }
                        XElement column2 = query.Elements(EName.columns).Descendants(EName.column).SearchByAttribute(AName.name, sAnyColumn);
                        if (column2 != null)
                        {
                            column2.SetAttributeValue(AName.name, column.ColumnName);
                        }
                    }
                }

                var groupingQueryNTh = Compiler.copyThisColumns(new XElement(groupingQuery)); // только , чтобы выражение colExpr было уже с учетом подставленными this.[col], не оптимально

                var colExpr = groupingQueryNTh.Elements(TextConst.EName.Select).Elements().First(e => Cmn.GetAttrValue(e, TextConst.AName.As) == column.ColumnName);

                colExpr = new XElement(colExpr);
                colExpr.Attributes(TextConst.AName.Group).Remove();
                //checkPfx1 = checkPfx;
                colExpr.SetAttributeValue(TextConst.AName.As, column.ColumnName);
                colExpr.SetAttributeValue(TextConst.AName.Title, "");
                colExpr.Attributes(TextConst.AName.Removeable).Remove();// откуда то берется
                if (checkExpr == null)
                {

                    query.Element(TextConst.EName.Select).Add(colExpr);
                    var colsInfo = query.Element(TextConst.EName.Columns);
                    if (colsInfo != null)
                    {
                        colsInfo.Add(new XElement(TextConst.EName.Column, new XAttribute(TextConst.AName.Name, column.ColumnName)));
                    }
                    checkExpr = colExpr;
                    isCheckExpr = false;
                }
                else
                {
                    isCheckExpr = true;
                    var colsToReplace = checkExpr.Descendants().Where(e => Cmn.GetAttrValue(e, TextConst.AName.As) == column.ColumnName).ToArray();

                    foreach (var col in colsToReplace)
                    {
                        col.ReplaceWith(new XElement(colExpr));
                    }
                }

                if (checkExpr.DescendantsAndSelf().Any(EPredicate.IsCallOfWindowFunction))
                {
                    if (isCheckExpr)
                    {
                        var xtraCondCol = new XElement(colExpr);
                        xtraCondCol.SetAttributeValue(TextConst.AName.Removeable, TextConst.AVBool.False);
                        xtraCondCol.SetAttributeValue(TextConst.AName.As, detExtaCondAlias);
                        query.Element(TextConst.EName.Select).Add(xtraCondCol);
                    }
                    isOuterCond = true;
                }

            }




            var grsetNode = groupingQuery.Descendants(TextConst.EName.Grsets).Descendants(TextConst.EName.Grset) // !!! если будет usepart - облом. Доделать! 
                .First(e => e.Attribute(TextConst.AName.As).Value == groupName);
            // !!! учесть новый вариант - grouping

            var grsets = grsetNode.AncestorsAndSelf(TextConst.EName.Grset);

            var levels = grsets.Attributes(TextConst.AName.Level).SelectMany(a => a.Value.Split(',')).Where(v => v != "").Distinct().ToList();


            var grColumns = groupingQuery.Elements(TextConst.EName.Select).Elements().Where(e => levels.Contains(Cmn.GetAttrValue(e, TextConst.AName.Group))).ToList();

            var grColNames = new List<string>();

            foreach (XElement col in grColumns)
            {
                var s = Cmn.GetAttrValue(col, TextConst.AName.As);
                if (s == "")
                {
                    s = Cmn.GetAttrValue(col, TextConst.AName.Column);
                }
                grColNames.Add(s);

            }

            var cond = new XElement(TextConst.EName.Call);

            cond.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.And);
            List<XElement> conds = new List<XElement>();
            foreach (string colName in grColNames)
            {
                var cond1 = new XElement(TextConst.EName.Call);

                if (row[colName] != DBNull.Value)
                {
                    cond1.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.Equal);

                }
                else
                {
                    cond1.SetAttributeValue(TextConst.AName.Function, TextConst.AVFunction.IsNull);
                }

                var col = new XElement(TextConst.EName.Column);
                col.SetAttributeValue(TextConst.AName.Table, TextConst.AVTable.Ths);
                col.SetAttributeValue(TextConst.AName.Column, colName);
                cond1.Add(col);


                cond.Add(cond1);

                if (row[colName] != DBNull.Value)
                {
                    var cnst = new XElement(TextConst.EName.Const);
                    cnst.Value = (Cmn.ToOracleString(row[colName]));
                    cond1.Add(cnst);

                }
                if (!query.Elements(TextConst.EName.Where).Any())
                {
                    query.Add(new XElement(TextConst.EName.Where));
                }


            }
            conds.Add(cond);

            var newCond = grsets.Elements(TextConst.EName.Where).Elements().Select(e => new XElement(e)).ToList();





            if (newCond.Any())
            {
                foreach (XElement col in newCond.Descendants(TextConst.EName.Column).ToList())
                {
                    col.SetAttributeValue(TextConst.AName.Table, TextConst.AVTable.Ths);
                }
                //query.Element(TextConst.EName.Where).Add(newCond);
            }
            conds.AddRange(newCond);
            var colCondTargs = new List<Tuple<XElement, string>>();
            if (column != null
                //  && !isOuterCond
                )
            {
                colCondTargs.Add(new Tuple<XElement, string>(query, colCondname));
            }

            var condTarg = query;


            if (isOuterCond)
            {

                Compiler.AddQueryLevel(query, "a1");
                condTarg = query.Elements(TextConst.EName.From).Elements().First();

                //condTarg.SetAttributeValue(TextConst.AName.Hint, TextConst.AVHint.Materialize);
                condTarg.SetAttributeValue(TextConst.AName.Materialize, TextConst.AVBool.True);
                // Compiler.addMatrializeId(condTarg);
                if (isCheckExpr)
                {
                    colCondTargs.Add(new Tuple<XElement, string>(condTarg, detExtaCondAlias));
                }
            }
            if (conds.Any())
            {
                if (!condTarg.Elements(TextConst.EName.Where).Any())
                {
                    condTarg.Add(new XElement(TextConst.EName.Where));
                }
                condTarg.Element(TextConst.EName.Where).Add(conds);
            }
            foreach (var qc in colCondTargs)
            {
                var qry1 = qc.Item1;
                var condColName = qc.Item2;
                if (!qry1.Elements(TextConst.EName.Where).Any())
                {
                    qry1.Add(new XElement(TextConst.EName.Where));
                }
                var colCond = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.IsNotNull));
                var xcol = new XElement(TextConst.EName.Column);
                xcol.SetAttributeValue(TextConst.AName.Table, TextConst.AVTable.Ths);
                xcol.SetAttributeValue(TextConst.AName.Column, condColName);
                if (!use_zeros)
                {
                    var nifCond = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.NullIf));
                    colCond.Add(nifCond);
                    nifCond.Add(xcol);
                    var sconst = "''";
                    if (column.DataType == XmlReports.numberType)
                    {
                        sconst = "0";
                    }
                    nifCond.Add(new XElement(TextConst.EName.Const, new XText(sconst)));
                }
                else
                {
                    colCond.Add(xcol);
                }
                qry1.Element(TextConst.EName.Where).Add(colCond);
            }
            Compiler.addMatrializeId(query);
        }
    }
}