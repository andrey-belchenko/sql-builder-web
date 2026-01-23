using System.Linq;
using System.Xml.Linq;
//using infoenergo.core.Extensions;
using System.Collections.Generic;
using System.Reflection;
using sql.builder.Exceptions;
using _AName = sql.builder.DataApi.AName;
using sql.builder.WebReports;
using sql.builder.Clean.Extensions;

namespace sql.builder.DataApi
{
    internal partial class VQuery
    {

        private static SortedList<string, SortedList<string, string>> ColumnsTempNames = new SortedList<string, SortedList<string, string>>();
        private static SortedList<string, SortedList<string, int>> TypesInd = new SortedList<string, SortedList<string, int>>();
        public string GetColumnTempName(string columnName, string dataType)
        {
            return GetColumnTempName(GetMainIE().P_IdName, columnName, dataType);
        }

        public static string GetColumnTempName(string queryName, string columnName, string dataType = null)
        {



            SortedList<string, int> typesInd1 = null;
            if (!TypesInd.ContainsKey(queryName))
            {
                typesInd1 = new SortedList<string, int>();
                TypesInd.Add(queryName, typesInd1);
            }
            else
            {
                typesInd1 = TypesInd[queryName];
            }
            SortedList<string, string> columnsTempNames1 = null;

            if (!ColumnsTempNames.ContainsKey(queryName))
            {
                columnsTempNames1 = new SortedList<string, string>();
                ColumnsTempNames.Add(queryName, columnsTempNames1);
            }
            else
            {
                columnsTempNames1 = ColumnsTempNames[queryName];
            }
            string s = "";
            if (dataType == null)
            {
                s = columnsTempNames1[columnName];
            }
            else if (!columnsTempNames1.ContainsKey(columnName))
            {
                if (dataType == "")
                {
                    dataType = XmlReports.Environment.GetQuery(queryName).SearchColumn(columnName).XDataType();
                    if (dataType == "")
                    {
                        throw new VCompilerException("Не определен тип", null, null);// обработать нормально
                    }
                }
                string tpr = Compiler.getTyprPr(dataType);

                if (!typesInd1.ContainsKey(tpr))
                {
                    typesInd1.Add(tpr, 0);
                }
                typesInd1[tpr]++;
                columnsTempNames1[columnName] = tpr + typesInd1[tpr].ToString();
            }
            s = columnsTempNames1[columnName];

            return s;
        }
        internal static XElement CreateFilteredQuery(XElement xquery, int rows_limit)
        {

            if (WebReportsAdapter.IsWebQuery(xquery))
            {
                return xquery;
            }
            xquery = new XElement(xquery);
            xquery.RemoveAttribute(_AName.name);
            xquery.RemoveAttribute(_AName.inherit);
            XElement xparams = xquery.Element(EName.@params);
            if (xparams == null)
            {
                xparams = new XElement(EName.@params);
                xquery.AddFirst(xparams);
            }
            XElement xfrom = xquery.Element(EName.from);
            XElement xwhere = xquery.Element(EName.where);
            if (xwhere == null)
            {
                xwhere = new XElement(EName.where);
                xfrom.AddAfterSelf(xwhere);
            }
            //var xcall1 = new XElement("call",
            //                new XAttribute("function", "and"),
            //                new XElement("call",
            //                    new XAttribute("function", "true"))
            //                );
            //var xcall2 = new XElement("call",
            //               new XAttribute("function", "or")
            //               );
            //xcall2.Add(xcall1);
            XElement xcall = Factory.NewCall(TextConst.AVFunction.And,
                                        Factory.NewCall(TextConst.AVFunction.LessOrEqual,
                                                        Factory.NewCall(TextConst.AVFunction.RowNum),
                                                        Factory.NewUseParam(TextConst.AVParam.RowsLimit)));
            xcall.Add(xwhere.Elements());
            //xcall.Add(xcall2);
            xwhere.RemoveAll();
            xwhere.Add(xcall);
            //var xcolumns = query.Element("select").Elements("column").Where(col => col.AttrOrDef("title", "") != "");
            xparams.Add(Factory.NewParam(TextConst.DBParams.PrimaryKeyParam, TextConst.AVDataType.Array));
            xcall.Add(
                   new XElement(EName.call,
                       new XAttribute(_AName.function, TextConst.AVFunction.In),
                       new XAttribute(_AName.optional, TextConst.AVBool.True),
                       Factory.NewColumn(TextConst.AVTable.Ths, xquery.Elements(EName.select).Elements().First(e => e.AttrOrDefault(_AName.key, false)).Attribute(_AName.@as).Value),
                       Factory.NewUseParam(TextConst.DBParams.PrimaryKeyParam)));
            XElement nameColumn = xquery.Elements(EName.select).Elements().FirstOrDefault(e => e.AttrOrDefault(TextConst.AName.IsNameColumn, false));
            if (nameColumn != null)
            {
                xparams.Add(Factory.NewParam(TextConst.DBParams.ObjNameParam, TextConst.AVDataType.Array));
                xcall.Add(new XElement(EName.call,
                              new XAttribute(_AName.function, TextConst.AVFunction.In),
                              new XAttribute(_AName.optional, TextConst.AVBool.True),
                              Factory.NewColumn(TextConst.AVTable.Ths, nameColumn.Attribute(TextConst.AName.As).Value),
                              Factory.NewUseParam(TextConst.DBParams.ObjNameParam)));
            }
            foreach (XElement col in xquery.Element(EName.select).Elements(EName.column))
            {
                string name = col.Attribute(_AName.@as).Value;
                string field_name = name + "_filter";
                xparams.Add(Factory.NewParam(field_name, TextConst.AVDataType.String));
                xcall.Add(
                    new XElement(EName.call,
                        new XAttribute(_AName.function, "like"),
                        new XAttribute(_AName.optional, TextConst.AVBool.True),
                        Factory.NewColumn(TextConst.AVTable.Ths, name),
                        Factory.NewUseParam(field_name)));
            }
            xparams.Add(Factory.NewParam(TextConst.AVParam.RowsLimit, TextConst.AVDataType.Number));
            // параметр с общим условием
            // var cond_par_name = xquery.AttrOrDef(TextConst.AName.CondParName, null);
            // if (cond_par_name != null)
            // {
            //    xparams.Add(
            //        new XElement("param",
            //            new XAttribute("name", cond_par_name),
            //            new XAttribute("type", "string")));
            //}
            return xquery;
        }
        //public static XElement CreateNameSearchQuery(XElement xquery,string keyName,string paramName, string columnName)
        //{
        //    xquery = new XElement(xquery);

        //    var xcolKey= xquery.Element("select").Elements ().Where(e=>e.Attribute("as").Value!=keyName ).First();
        //    var xcolName= xquery.Element("select").Elements ().Where(e=>e.Attribute("as").Value!=columnName ).First();
        //       xquery.Element("select").Elements ().Remove();

        //       xquery.Element("select").Add(xcolName);
        //        xquery.Element("select").Add(xcolKey);
        //    xquery.Attribute("name").Remove();
        //    xquery.Attributes("inherit").Remove();

        //    var xparams = xquery.Element("params");
        //    if (xparams == null)
        //    {
        //        xparams = new XElement("params");
        //        xquery.AddFirst(xparams);
        //    }

        //    var xwhere = xquery.Element("where");
        //    if (xwhere == null)
        //    {
        //        xwhere = new XElement("where");
        //        xquery.Element("from").AddAfterSelf(xwhere);
        //    }

        //    var xcall = new XElement("call", new XAttribute("function", "="));
        //    xcall.Add(new XElement("column", new XAttribute("table", "this"), new XAttribute("column", keyName)));
        //    xcall.Add(new XElement("useparam", new XAttribute("name", paramName)));
        //    xwhere.RemoveAll();
        //    xwhere.Add(xcall);


        //    xparams.Add(
        //          new XElement("param",
        //              new XAttribute("name", paramName),
        //                new XAttribute("type", "number"))
        //              );

        //    return xquery;
        //}

        //public XElement CreateNameByKeyQuery()
        //{
        //    string alias="a";
        //    var key=KeyColumn();
        //    var name=NameColumn();
        //    var xkeyCol=new XElement(TextConst.EName.Column
        //                ,new XAttribute(TextConst.AName.Table,alias)
        //                    ,new XAttribute(TextConst.AName.Column,name.XName)
        //                );

        //    XElement xquery = new XElement(TextConst.EName.Query,
        //         new XElement(TextConst.EName.Params,
        //             new XElement(TextConst.EName.Param,
        //                 new XAttribute(TextConst.AName.Name,key.XName)
        //                 )
        //             ),
        //        new XElement(TextConst.EName.Select,
        //            xkeyCol

        //            ),
        //            new XElement(TextConst.EName.From,

        //                new XElement(TextConst.EName.Query,
        //                    new XAttribute(TextConst.AName.Name,P_IdName),
        //                    new XAttribute(TextConst.AName.As,alias)
        //                )

        //            ),
        //            new XElement(TextConst.EName.Where,

        //                new XElement(TextConst.EName.Call
        //                    ,new XAttribute(TextConst.AName.Function,TextConst.AVFunc.Equal)
        //                    ,xkeyCol
        //                    ,new XElement(TextConst.EName.UseParam
        //                        ,new XAttribute(TextConst.AName.Name,key.XName)
        //                )
        //               )
        //               )

        //      );



        //    return xquery;



        //}


        //public static List<string> ExtractColNamesFromOrderStr(string s)
        //{
        //    //потом периписать по умному
        //    var ss = s.Split(new char[] {',',' '});
        //    var list = new List<string>();
        //    foreach (var s1 in ss)
        //    {
        //        var s2 = s1;
        //        var ss1 = s1.Split('.');
        //        if (ss1.Length > 1)
        //        {
        //            s2 = ss1[1];
        //        }
        //        if (s2.ToLower() != "desc")
        //        {
        //            list.Add(s2);
        //        }
        //    }
        //    list = list.Distinct().ToList();
        //    return list;

        //}

        public XElement AsListQuery()
        {

            if (WebReportsAdapter.IsWebQuery(this))
            {
                return new XElement(this);
            }
            var keyCol = KeyColumn();
            XElement xquery = null;
            //var ordColsNames = ExtractColNamesFromOrderStr(P_Order); // не получилось
            if (keyCol == null || P_Order != "" || GetElementsP(EName.@params).Any())
            {
                xquery = new XElement(XmlReports.Environment.GetPrecompiledQuery(this.XName));
            }
            else
            {
                xquery = new XElement(TextConst.EName.Query);

                Cmn.copyAttributes(this, xquery);

                xquery.Add(new XElement(TextConst.EName.Select));

                xquery.Add(new XElement(TextConst.EName.From,
                      new XElement(TextConst.EName.Query

                          , new XAttribute(TextConst.AName.Name, this.XName)
                          , new XAttribute(TextConst.AName.As, "a")
                          )


                     )
                     );

                var colsAll = Columns();

                var cols = keyCol.AsList();



                var cols1 = colsAll.Where(e => e.P_IsListColumn == TextConst.AVBool.True).ToList();

                if (!cols1.Any())
                {
                    cols1 = colsAll.Where(e => e.P_Title != "").ToList();
                }

                if (!cols1.Any())
                {
                    cols1 = colsAll.Where(e => Cmn.GetAttrValue(e, TextConst.AName.Sys) != TextConst.AVBool.True).ToList();
                }

                if (!cols1.Where(c => cols.Contains(c)).Any())
                {

                    cols.AddRange(cols1);
                }
                else
                {
                    cols = cols1;
                }

                var tpc = TreeParentColumn();
                if (tpc != null)
                {
                    if (!cols.Contains(tpc))
                    {
                        cols.Add(tpc);
                    }
                }

                //var ordCols = colsAll.Where(e => ordColsNames.Contains(e.XName)).ToList(); // не получилось
                //foreach (var ocol in ordCols)
                //{
                //    if (!cols.Contains(ocol))
                //    {
                //        cols.Add(ocol);
                //    }
                //}

                // bool wasKey = false;
                foreach (VSXElement col in cols)
                {
                    var tablename = "a";
                    var colname = col.XName;


                    var xcol = new XElement(TextConst.EName.Column);
                    xcol.SetAttributeValue(TextConst.AName.Table, tablename);
                    xcol.SetAttributeValue(TextConst.AName.Column, colname);
                    xcol.SetAttributeValue(TextConst.AName.As, col.XName);
                    xcol.SetAttributeValue(TextConst.AName.Title, col.P_Title);
                    if (keyCol == col)
                    {
                        //if (!wasKey)
                        //{
                        xcol.SetAttributeValue(TextConst.AName.Key, TextConst.AVBool.True);
                        //    xcol.SetAttributeValue(TextConst.AName.Title, "");
                        //}
                        //else
                        //{
                        //    xcol.SetAttributeValue(TextConst.AName.As, col.XName + "_");
                        //}
                        //wasKey = true;
                    }
                    xquery.Element(TextConst.EName.Select).Add(xcol);
                    if (col is VColumn && col != keyCol)
                    {
                        var xcol1 = (col as VColumn).AsNameColumnOrSelf();

                        if (xcol1.Attribute(TextConst.AName.Table) != null && xcol1.Attribute(TextConst.AName.Table).Value != col.P_Table)
                        {
                            var xlink = new XElement(TextConst.EName.Link);
                            xlink.SetAttributeValue(TextConst.AName.Name, xcol1.Attribute(TextConst.AName.Table).Value);
                            xlink.SetAttributeValue(TextConst.AName.As, xcol1.Attribute(TextConst.AName.Table).Value);
                            xquery.Element(TextConst.EName.From).Elements().First().Add(xlink);
                            colname = xcol1.Attribute(TextConst.AName.Column).Value;
                            tablename = xcol1.Attribute(TextConst.AName.Table).Value;
                            xcol = new XElement(TextConst.EName.Column);
                            xcol.SetAttributeValue(TextConst.AName.Table, tablename);
                            xcol.SetAttributeValue(TextConst.AName.Column, colname);
                            xcol.SetAttributeValue(TextConst.AName.As, col.XName + TextConst.Pfx.ExtValName);
                            xcol.SetAttributeValue(TextConst.AName.Title, col.P_Title);
                            xquery.Element(TextConst.EName.Select).Add(xcol);

                        }


                    }


                }
                if (cols.Count == 1)
                {
                    XElement xcol = new XElement(EName.column);
                    xcol.SetAttributeValue(_AName.table, "a");
                    xcol.SetAttributeValue(_AName.column, keyCol.XName);
                    xcol.SetAttributeValue(_AName.@as, keyCol.XName + "1");
                    xquery.Element(EName.select).Add(xcol);
                }
                // xquery = new XElement(this);
                //var xselEls = xquery.Elements("select").Elements();
                //xselEls.First().SetAttributeValue("mark", "1");
                //var els = xselEls.Where(e => Cmn.GetAttrValue(e, "vid") == "1").ToList();

                //if (els.Count() > 0)
                //{
                //    els.ForEach(e => e.SetAttributeValue("mark", "1"));
                //}
                //else
                //{
                //    var pcol = this.P_ParentFieldName;
                //    foreach (XElement col in xselEls)
                //    {
                //        if (Cmn.GetAttrValue(col, TextConst.AName.Title) != "")
                //        {
                //            col.SetAttributeValue("mark", "1");
                //        }
                //        if (pcol != "")
                //        {
                //            var alias = Cmn.GetAttrValue(col, TextConst.AName.As);
                //            if (alias == pcol)
                //            {
                //                col.SetAttributeValue("mark", "1");
                //            }
                //            else if (alias == "" && Cmn.GetAttrValue(col, TextConst.AName.Column)==pcol)
                //            {
                //                col.SetAttributeValue("mark", "1");
                //            }


                //        }
                //    }


                //}
                //xselEls.Where(e1 => Cmn.GetAttrValue(e1, "mark") != "1").Remove();
                //xselEls.Attributes("mark").Remove();
            }
            xquery.Attributes(TextConst.AName.Name).Remove();
            xquery.Attributes(TextConst.AName.Inherit).Remove();
            return xquery;

        }

        private XElement createListFilterFields(VSXElement flink)
        {
            var link = (VLink)flink;
            var fld = new XElement(TextConst.EName.Field);
            //
            string qryName = null;
            var rel = link.GetRelation();
            VQueryCall qc = null;
            if (rel != null)
            {
                var col = rel.ChildColumnSource() as VColumn;
                if (col != null)
                {
                    qc = col.ListQueryCallElement();
                    //if (qc != null)
                    //{
                    //    qryName = qc.P_CalledQuery;
                    //}
                }
            }
            var qry = link.Query();
            if (qryName == null)
            {
                qryName = qry.XName;
            }
            var keyCol = qry.KeyColumn();

            fld.SetAttributeValue(TextConst.AName.Name, TextConst.Pfx.ParamVar + link.XName);
            fld.SetAttributeValue(TextConst.AName.Type, keyCol.XDataType());
            fld.SetAttributeValue(TextConst.AName.ShowNulls, TextConst.AVBool.True);
            fld.SetAttributeValue(TextConst.AName.ControlType, TextConst.AVControlType.List);
            var title = link.XTitle;
            if (title == "")
            {
                title = link.XName;
            }
            fld.SetAttributeValue(TextConst.AName.Title, title);
            XElement lq = null;
            if (qc == null)
            {
                lq = new XElement(TextConst.EName.ListQuery
                   , new XElement(TextConst.EName.Query, new XAttribute(TextConst.AName.Name, qryName))

                   );
            }
            else
            {
                lq = new XElement(qc.GetParent());
                lq.Elements().First().Elements().Remove();// удаление параметров, костыль
            }
            lq.SetAttributeValue(TextConst.AName.UseColPreset, TextConst.AVBool.True);

            fld.Add(lq);
            return fld;

        }
        private XElement createRangeFilterFields(VSXElement fcol)
        {
            var grp = new XElement(TextConst.EName.FieldGroup);
            grp.SetAttributeValue(TextConst.AName.Title, fcol.P_Title);

            var fld = new XElement(TextConst.EName.Field);
            fld.SetAttributeValue(TextConst.AName.Name, TextConst.Pfx.ParamVar + fcol.XName + "1");
            var tp = fcol.XDataType();
            fld.SetAttributeValue(TextConst.AName.Type, tp);
            fld.SetAttributeValue(TextConst.AName.ControlType, fcol.ControlType());
            fld.SetAttributeValue(TextConst.AName.Title, "С");
            fld.SetAttributeValue(TextConst.AName.WidthPerc, "50");
            var ft = fcol.XFormat();

            if (ft == "" && tp == TextConst.AVDataType.Number)
            {
                ft = TextConst.AVEditMask.N2;
            }

            if (ft != "")
            {
                fld.SetAttributeValue(TextConst.AName.EditMask, ft); // не уверен что так корректно
            }


            grp.Add(fld);

            fld = new XElement(fld);
            fld.SetAttributeValue(TextConst.AName.Name, TextConst.Pfx.ParamVar + fcol.XName + "2");
            fld.SetAttributeValue(TextConst.AName.Title, "По");

            grp.Add(fld);
            return grp;
        }

        private XElement createStringFilterFields(VSXElement fcol)
        {
            var fld = new XElement(TextConst.EName.Field);
            fld.SetAttributeValue(TextConst.AName.Name, TextConst.Pfx.ParamVar + fcol.XName);
            fld.SetAttributeValue(TextConst.AName.Type, fcol.XDataType());
            fld.SetAttributeValue(TextConst.AName.ControlType, fcol.ControlType());
            fld.SetAttributeValue(TextConst.AName.Title, fcol.P_Title);
            fld.SetAttributeValue(TextConst.AName.ShowNulls, TextConst.AVBool.True);
            return fld;
        }
        public List<VSXElement> ElementsForAutoFilter()
        {

            if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null))
            {
                return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as List<VSXElement>);
            }

            var flinksList = new SortedList<string, VSXElement>();

            var allLinks = AllSources();
            var list = new List<VSXElement>();
            foreach (var l in allLinks)
            {
                if (l.P_AutoFilter == TextConst.AVBool.True)
                {
                    if (!flinksList.ContainsKey(l.XName))
                    {
                        flinksList.Add(l.XName, l);
                    }
                }
            }

            foreach (VSXElement col in Columns())
            {
                if (col.P_AutoFilter == TextConst.AVBool.True)
                {
                    list.Add(col);
                }
                else
                {
                    if (flinksList.Keys.Contains(col.P_Table))
                    {
                        list.Add(flinksList[col.P_Table]);
                        flinksList.Remove(col.P_Table);

                    }
                }
            }

            list.AddRange(flinksList.Values);
            AddCashValue(list, MethodBase.GetCurrentMethod().ToString(), null);

            return list;
        }

        public override XElement GetFormXElement()
        {
            // сделать чтобы работало
            var form = Form();
            if (form != null)
            {
                return form.GetFormXElement();
            }
            // var compiled = Compiler.PreCompileQuery(new XElement(this),true);
            var compiled = XmlReports.Environment.GetPrecompiledQuery(this.XName);
            var cnt = compiled.Element(TextConst.EName.Content);
            bool isAutoFilter = (P_AutoFilter == TextConst.AVBool.True);
            if (cnt != null || isAutoFilter)
            {
                var xform1 = base.GetFormXElement();
                if (cnt == null)
                {
                    cnt = new XElement(TextConst.EName.Content);
                }
                else
                {
                    cnt = new XElement(cnt);
                }
                xform1.Add(cnt);



                if (isAutoFilter)
                {
                    var elsForFilter = ElementsForAutoFilter();
                    foreach (var fel in elsForFilter)
                    {
                        XElement xflds = null;
                        VSXElement col = null;
                        if (fel is VQueryCall)
                        {
                            xflds = createListFilterFields(fel);
                            col = (fel as VQueryCall).UsedColumns().FirstOrDefault();
                        }
                        else
                        {
                            col = fel;
                            switch (fel.XDataType())
                            {
                                case TextConst.AVDataType.Date:
                                    xflds = createRangeFilterFields(fel);
                                    break;
                                case TextConst.AVDataType.Number:
                                    xflds = createRangeFilterFields(fel);
                                    break;
                                case TextConst.AVDataType.String:
                                    xflds = createStringFilterFields(fel);
                                    break;
                                case TextConst.AVDataType.Clob:
                                    xflds = createStringFilterFields(fel);
                                    break;

                            }
                        }

                        foreach (var xfld in xflds.DescendantsAndSelf(TextConst.AName.Field))
                        {
                            xfld.SetAttributeValue(TextConst.AName.ColumnVisible, TextConst.AVBool.False);
                        }

                        List<string> bandInfo = new List<string>();

                        if (col != null)
                        {
                            var band = col.GetParent();
                            while (band is VBand)
                            {
                                bandInfo.Add(band.P_Title);
                                band = band.GetParent();
                            }

                        }

                        bandInfo.Reverse();

                        var xfieldgroup = cnt;

                        foreach (var sband in bandInfo)
                        {
                            var xfieldgroup1 = xfieldgroup.Elements().FirstOrDefault(e => Cmn.GetAttrValue(e, TextConst.AName.Title) == sband);
                            if (xfieldgroup1 == null)
                            {
                                xfieldgroup1 = new XElement(TextConst.EName.FieldGroup);
                                xfieldgroup1.SetAttributeValue(TextConst.AName.Title, sband);
                                xfieldgroup.Add(xfieldgroup1);
                            }
                            xfieldgroup = xfieldgroup1;
                        }

                        xfieldgroup.Add(xflds);


                    }
                }

                form = VSXElement.Get<VForm>(xform1);
                form.VirtualParent = this.GetParent();
                //form.environment = this.GetEnvironment();
            }

            if (form != null)
            {
                return form.GetFormXElement();
            }

            // то что ниже вроде бы не используется
            var xform = base.GetFormXElement();


            var xfields = XmlReports.Environment.Manager.GetScheme().SelectMany(v => v.GetElementsP(EName.fields)).SelectMany(f => f.GetElementsP(EName.field)).ToList();

            var xparams = GetElementsP(EName.@params).FirstOrDefault();
            if (xparams != null)
            {
                foreach (var vsxelement in xparams.GetElementsP())
                {
                    var field_atr = vsxelement.Attribute(TextConst.AName.Field);
                    if (field_atr == null) continue;

                    var xfield = xfields.First(c => c.Attribute(TextConst.AName.Id).Value == field_atr.Value);

                    var el = new XElement(xfield);

                    el.CopyAttributes(vsxelement.Attributes());
                    // перетираем id
                    el.SetAttributeValue(TextConst.AName.Id, vsxelement.BaseElementOrSelf().GetUniqueKey());
                    foreach (var layout_option in TextConst.ANameArray.AllLayoutOptions)
                    {
                        el.SetAttributeValue(layout_option, vsxelement.AttrOrDef(layout_option, null));
                    }

                    xform.Add(el);
                }
            }

            return xform;
        }


        #region DxExport
        public override bool P_DxExport_Exists()
        {
            return IsReport();
        }
        #endregion

    }
}
