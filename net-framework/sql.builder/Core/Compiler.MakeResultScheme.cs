using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Diagnostics; // Debug, Stopwatch
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Data;
using sql.builder.DataApi;

namespace sql.builder
{
    internal static partial class Compiler
    {
        // Перевод VReport.MakeResultScheme()
        internal static XElement MakeResultScheme(XElement report, XElement compiled)
        {
            Contract.Assert(compiled != null);
            Contract.Assert(compiled.Name == EName.root);
            #if DEBUG
            string report_name = report.Attribute(AName.name).Value;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #endif
            XElement root = new XElement(EName.root);
            XElement xscheme = new XElement(EName.scheme);
            root.Add(xscheme);
            XElement data = new XElement(EName.data);
            root.Add(data);
            foreach (XElement query in report.Elements(EName.queries).Elements(EName.query)) {
                getReportQueryScheme(query, compiled, xscheme);
            }
            setColumnsVisibility(root);
            moveTransposedToParent(root);
            moveUnitedToParent(root);
            xscheme.Remove();
            if (report.AttrOrDefault(AName.autobands, false)) {
                addBandsForClassTitles(xscheme);
            }
            if (report.AttrOrEmpty(AName.auto_merge) == TextConst.AVBool.True) {
                HashSet<string> colNames = new HashSet<string>();
                foreach (XElement table in xscheme.Descendants(EName.table)) {
                    foreach (XElement col in table.Elements(EName.columns).Elements()) {
                        colNames.Add(col.Attribute(AName.name).Value);
                    }
                    foreach (XElement col in table.Elements(EName.viewcolumns).Descendants(EName.column).ToList()) {
                        if (!colNames.Contains(col.Attribute(AName.name).Value)) {
                            col.Remove();
                        }
                    }
                    colNames.Clear();
                }
            }
            #if DEBUG
            sw.Stop();
            Debug.WriteLine("Compiler.MakeResultScheme() report name=\"" + report_name + "\": " + sw.ElapsedTicks + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
            #endif
            return xscheme;
        }
        // Скопировано из VReport.addBandsForClassTitles()
        internal static void addBandsForClassTitles(XElement scheme)
        {
            if (!scheme.Descendants().Attributes(AName.class_title).Any()) {
                return;
            }
            foreach (XElement table in scheme.Descendants(EName.table)) {
                XElement viewColumns = table.Element(EName.viewcolumns);
                string classTitle = string.Empty;
                string prevClassTitle = string.Empty;
                List<XElement> cols = new List<XElement>();
                foreach (XElement col in viewColumns.Elements()) {
                    classTitle = col.AttrOrEmpty(AName.class_title);
                    if (classTitle != prevClassTitle) {
                        if (prevClassTitle != string.Empty) {
                            XElement band = new XElement(EName.band, new XAttribute(AName.title, prevClassTitle));
                            col.AddBeforeSelf(band);
                            foreach (XElement col1 in cols) {
                                col1.Remove();
                                band.Add(col1);
                            }
                        }
                        cols.Clear();
                    }
                    if (classTitle != string.Empty) {
                        cols.Add(col);
                    }
                    prevClassTitle = classTitle;
                }
                if (prevClassTitle != string.Empty) {
                    XElement band = new XElement(EName.band, new XAttribute(AName.title, prevClassTitle));
                    viewColumns.Add(band);
                    foreach (XElement col1 in cols) {
                        col1.Remove();
                        band.Add(col1);
                    }
                }
            }
        }
        /*private static XElement GetSelectExpression(XElement query, string column_alias)
        {
            // select/*[@as='{0}']
            return query.Elements(EName.select).Elements().SearchByAttribute(AName.As, column_alias);
        }*/
        private static XElement GetIntoInfo(XElement report, string query_name, string column_info)
        {
            // //query[@name='{0}']/insert/column[@info='{1}']
            XElement query = report.Descendants(EName.query).SearchByAttribute(AName.name, query_name);
            if (query == null) {
                return null;
            } else {
                return query.Elements(EName.insert).Elements(EName.column).SearchByAttribute(AName.info, column_info);
            }
        }
        // Перевод XmlReports.getReportQueryScheme()
        private static void getReportQueryScheme(XElement query, XElement report, XElement outputParent)
        {
            Contract.Assert(query != null);
            Contract.Assert(query.Name == EName.query);
            Contract.Assert(report != null);
            Contract.Assert(report.Name == EName.root);
            Contract.Assert(outputParent != null);
            Contract.Assert(outputParent.Name == EName.scheme || outputParent.Name == EName.childs);
            // line 1120
            string query_name = query.Attribute(AName.name).Value;
            // root/query[@name='{0}' and @materialize='1']
            XElement processedQuery = report.Elements(EName.query).FirstOrDefault(e => e.AttrOrEmpty(AName.name) == query_name && e.AttrOrEmpty(AName.materialize) == "1");
            // root/query[@name='{0}' and not(@materialize='1')]
            XElement processedQuery1 = report.Elements(EName.query).FirstOrDefault(e => e.AttrOrEmpty(AName.name) == query_name && e.AttrOrEmpty(AName.materialize) != "1");
            XAttribute attr;
            XElement processedQuerySel;
            if (processedQuery == null) {
                //processedQuery = processedQuery1;
                processedQuerySel = processedQuery1.Element(EName.select);
            } else {
                processedQuerySel = processedQuery.Element(EName.select);
                bool altered = false;
                // select/*[@as]
                foreach (XElement srcColumn in processedQuery1.Elements(EName.select).Elements()) {
                    attr = srcColumn.Attribute(AName.@as);
                    if (attr != null) {
                        // select/*[@as='{0}']
                        //XElement srcColumn1 = GetSelectExpression(processedQuery, attr.Value);
                        XElement srcColumn1 = processedQuerySel.Elements().SearchByAttribute(AName.@as, attr.Value);
                        if (srcColumn1 == null) {
                            if (!altered) {
                                processedQuerySel = new XElement(processedQuerySel); // чтобы не изменилось дерево элементов report
                                altered = true;
                            }
                            processedQuerySel.Add(new XElement(srcColumn));
                        }
                    }
                }
            }
            // line 1143
            XElement table = new XElement(EName.table);
            outputParent.Add(table);
            Cmn.CopyAttribute(query, table, AName.calctree);
            Cmn.CopyAttribute(query, table, AName.prep_merge);
            XElement joinCall = query.Element(EName.call);
            if (joinCall != null) {
                XElement join = new XElement(EName.joinon);
                join.Add(new XElement(joinCall));
                table.Add(join);
            }
            table.Add(new XAttribute(AName.name, query_name));
            string query_alias = query.Attribute(AName.@as).Value;
            table.Add(new XAttribute(AName.@as, query_alias));
            Cmn.CopyAttribute(query, table, AName.main);
            Cmn.CopyAttribute(query, table, AName.title);
            Cmn.CopyAttribute(query, table, AName.union);
            // line 1163
            XElement columns = new XElement(EName.columns);
            table.Add(columns);
            // line 1166
            XElement nodeTranspose = query.Element(EName.transpose);
            if (nodeTranspose != null) {
                // line 1170
                table.SetAttrValue(AName.transposed, TextConst.AVBool.True);
                XElement dimensionColumns = new XElement(EName.dimension_сolumns);
                table.Add(dimensionColumns);
                XElement valueColumns = new XElement(EName.value_сolumns);
                table.Add(valueColumns);
                // line 1173
                XElement r = new XElement(EName.root);
                r.Add(new XElement(processedQuery1));
                string sql = Compiler.GetSql(r);
                r = null;
                // line 1174
                XElement dimensionValues = null;
                string q = string.Empty;
                //string colsPref = query_alias;
                string dimColName = string.Empty;
                string colKey = string.Empty;
                string colTitle = string.Empty;
                foreach (XElement nodeCol in nodeTranspose.Elements(EName.dimension).Elements(EName.column)) {
                    // line 1183
                    dimensionValues = new XElement(EName.dimension_values);
                    table.Add(dimensionValues);
                    // select/*[@as='{0}']
                    string col_name = nodeCol.Attribute(AName.column).Value;
                    // XElement srcColumn = GetSelectExpression(processedQuery, col_name);
                    XElement srcColumn = processedQuerySel.Elements().SearchByAttribute(AName.@as, col_name);
                    // NB: col_name == srcColumn.Attributes["as"].Value
                    XElement column = new XElement(EName.column);
                    dimensionColumns.Add(column);
                    dimColName = col_name; // для начала возможно только одно измерение
                    colKey = "'" + dimColName + "'";
                    column.Add(new XAttribute(AName.name, col_name));
                    column.Add(new XAttribute(AName.type, srcColumn.AttrOrDefault(AName.type, TextConst.AVDataType.String)));
                    // line 1192
                    // query[@name='{0}']/insert/column[@info='{1}']
                    XElement intoInfo = GetIntoInfo(report, query_name, col_name);
                    string into;
                    if (intoInfo != null) {
                        into = intoInfo.Attribute(AName.column).Value;
                    } else {
                        into = col_name;
                    }
                    column.Add(new XAttribute(AName.into, into));
                    // line 1203
                    colKey += "||'_'||" + col_name;
                    colTitle += q + nodeCol.Attribute(AName.title).Value;
                    q = "||'.'||";
                }
                // line 1208
                sql = "select" + colKey + " as key, max (" + colTitle + ") as title, max(" + dimColName + ")||'' as dim_val from  (select rownum rn, a.* from (" + sql + ") a ) group by " + colKey + " order by min(rn)";
                DataTable transposeColumns = db.ExecuteDataTable(sql, db.Connection);
                int colIndex = 0;
                int i = 0;
                foreach (XElement nodeCol in nodeTranspose.Elements(EName.values).Elements(EName.column)) {
                    // line 1217
                    // select/*[@as='{0}']
                    string col_name = nodeCol.Attribute(AName.column).Value;
                    //XElement srcColumn = GetSelectExpression(processedQuery, col_name);
                    XElement srcColumn = processedQuerySel.Elements().SearchByAttribute(AName.@as, col_name);
                    // NB: col_name == srcColumn.Attributes["as"].Value
                    XElement intoInfo = GetIntoInfo(report, query_name, col_name);
                    XElement column1 = new XElement(EName.column);
                    valueColumns.Add(column1);
                    column1.Add(new XAttribute(AName.name, col_name));
                    column1.Add(new XAttribute(AName.type, srcColumn.AttrOrDefault(AName.type, TextConst.AVDataType.String)));
                    string into;
                    if (intoInfo != null) {
                        into = intoInfo.Attribute(AName.column).Value;
                    } else {
                        into = col_name;
                    }
                    column1.Add(new XAttribute(AName.into, into));
                    // line 1234
                    foreach (DataRow dataRow in transposeColumns.Rows) {
                        // line 1237
                        string dim_val = dataRow.Field<string>("dim_val");
                        string title = dataRow.Field<string>("title");
                        string key = dataRow.Field<string>("key");
                        if (i == 0) {
                            XElement dimVal = new XElement(EName.val);
                            dimensionValues.Add(dimVal);
                            dimVal.Add(new XAttribute(AName.value, dim_val));
                            dimVal.Add(new XAttribute(AName.title, title));
                            dimVal.Add(new XAttribute(AName.columnpref, key + "_"));
                        }
                        // line 1247
                        XElement column = new XElement(EName.column);
                        columns.Add(column);
                        column.Add(new XAttribute(AName.name, key + "_" + col_name));
                        column.Add(new XAttribute(AName.dimension_column, dimColName));
                        column.Add(new XAttribute(AName.value_column, col_name));
                        column.Add(new XAttribute(AName.dimension_value, dim_val));
                        attr = srcColumn.Attribute(AName.title);
                        if (attr != null) {
                            column.Add(new XAttribute(AName.title, attr.Value));
                        }
                        column.Add(new XAttribute(AName.band_title, title));
                        column.Add(new XAttribute(AName.table, query_alias));
                        column.Add(new XAttribute(AName.type, srcColumn.AttrOrDefault(AName.type, TextConst.AVDataType.String)));
                        column.Add(new XAttribute(AName.into, into));
                        column.Add(new XAttribute(AName.index, colIndex.ToString()));
                        colIndex++;
                    }
                    i++;
                }
            } else {
                // line 1280
                foreach (XElement srcColumn in processedQuerySel.Elements()) {
                    string column_alias = srcColumn.AttrOrDefault(AName.@as, null);
                    if (column_alias != null) {
                        XElement column = new XElement(EName.column);
                        columns.Add(column);
                        column.Add(new XAttribute(AName.name, column_alias));
                        attr = srcColumn.Attribute(AName.title);
                        if (attr != null) {
                            if (attr.Value == "-") {
                                attr.Value = string.Empty;
                            }
                            column.Add(new XAttribute(AName.title, attr.Value));
                        }
                        column.Add(new XAttribute(AName.type, srcColumn.AttrOrDefault(AName.type, TextConst.AVDataType.String)));
                        Cmn.CopyAttribute(srcColumn, column, AName.editor);
                        Cmn.CopyAttribute(srcColumn, column, AName.agg);
                        Cmn.CopyAttribute(srcColumn, column, AName.format);
                        Cmn.CopyAttribute(srcColumn, column, AName.c_master);
                        Cmn.CopyAttribute(srcColumn, column, AName.c_master_key);
                        Cmn.CopyAttribute(srcColumn, column, AName.pivot);
                        Cmn.CopyAttribute(srcColumn, column, AName.dimname);
                        Cmn.CopyAttribute(srcColumn, column, AName.visible);
                        Cmn.CopyAttribute(srcColumn, column, AName.intern);
                        // query[@name='{0}']/insert/column[@info='{1}']
                        XElement intoInfo = report.Elements(EName.query).Where(e => e.AttrOrEmpty(AName.name) == query_name).Elements(EName.insert).Elements(EName.column).SearchByAttribute("info", column_alias);
                        string into;
                        if (intoInfo != null) {
                            into = intoInfo.Attribute(AName.column).Value;
                        } else {
                            into = column_alias;
                        }
                        column.Add(new XAttribute(AName.into, into));
                        Cmn.CopyAttribute(srcColumn, column, AName.key);
                        Cmn.CopyAttribute(srcColumn, column, AName.value_column);
                        Cmn.CopyAttribute(srcColumn, column, AName.dimension_column);
                        Cmn.CopyAttribute(srcColumn, column, AName.dimension_value);
                        Cmn.CopyAttribute(srcColumn, column, AName.band_title);
                        Cmn.CopyAttribute(srcColumn, column, AName.value_title);
                        Cmn.CopyAttribute(srcColumn, column, AName.class_title);
                        Cmn.CopyAttribute(srcColumn, column, AName.client_calc);
                        Cmn.CopyAttribute(srcColumn, column, AName.excel_calc);
                        column.CopyAttributes(srcColumn.Attributes().Where(APredicate.IsAdditionalAttribute));
                    }
                }
            }
            // line 1333
            XElement childs = null;
            foreach (XElement childQuery in query.Elements(EName.query)) {
                if (childs == null) {
                    childs = new XElement(EName.childs);
                    table.Add(childs);
                }
                getReportQueryScheme(childQuery, report, childs);
            }
            if (!table.AttrOrDefault(AName.transposed, false)) {
                applyColumnsPreset(query, report, table);
            }
        }
        // Перевод XmlReports.applyColumnsPreset(), line 1350...1515
        private static void applyColumnsPreset(XElement query, XElement report, XElement table)
        {
            XElement reportColumns = new XElement(EName.viewcolumns);
            table.Add(reportColumns);
            XElement reportSrcColumns = query.Element(EName.columns);
            if (reportSrcColumns == null) {
                // ancestor::report
                XElement repScheme = query.GetAncestor(EName.report);
                XElement firstQuery = repScheme.Elements(EName.queries).Elements(EName.query).First();
                if (object.ReferenceEquals(firstQuery, query)) {
                    reportSrcColumns = repScheme.Element(EName.columns);
                }
            }
            if (reportSrcColumns != null) {
                foreach (XElement el in reportSrcColumns.Elements()) {
                    reportColumns.Add(new XElement(el));
                }
            }
            // line 1372
            foreach (XElement band in reportColumns.Descendants(EName.band)) {
                // обрабатывается только частный сучай - недоделано
                string table_name = band.AttrOrDefault(AName.table, null);
                string column_name = band.AttrOrDefault(AName.column, null);
                if (table_name != null && column_name != null) {
                    // childs/table[@as='{0}' and dimension-сolumns/column/@name='{1}']
                    XElement transpTbl = table.Elements(EName.childs).Elements(EName.table).SearchByAttribute(AName.@as, table_name);
                    if (transpTbl != null && table.Elements(EName.dimension_сolumns).Elements(EName.column).SearchByAttribute(AName.name, column_name) != null) {
                        // line 1382
                        foreach (XElement dimVal in transpTbl.Elements(EName.dimension_values).Elements()) {
                            XElement valBand = new XElement(band);
                            band.AddBeforeSelf(valBand);
                            valBand.CopyAttributes(dimVal.Attributes());
                            valBand.SetAttrValue(AName.band_type, "dimband");
                            foreach (XElement col in valBand.Elements(EName.column)) {
                                XAttribute attr = col.Attribute(AName.name);
                                attr.Value = transpTbl.Elements(EName.dimension_values).Elements(EName.column).First().Attribute(AName.name).Value + "_" + dimVal.Attribute("value").Value + "_" + attr.Value;
                            }
                        }
                        band.Remove();
                    } else {
                        // line 1400
                        // childs/table[@as='{0}' and value-сolumns/column/@name='{1}']
                        transpTbl = table.Elements(EName.childs).Elements(EName.table).SearchByAttribute(AName.@as, table_name);
                        if (transpTbl != null && transpTbl.Elements(EName.value_сolumns).Elements(EName.column).SearchByAttribute(AName.name, column_name) != null) {
                            band.SetAttrValue(AName.band_type, "valband");
                            foreach (XElement dimVal in transpTbl.Elements(EName.dimension_values).Elements()) {
                                XElement valBandedCol = new XElement(EName.column);
                                band.Add(valBandedCol);
                                valBandedCol.Add(new XAttribute(AName.table, table_name));
                                Cmn.CopyAttribute(band, valBandedCol, AName.format);
                                Cmn.CopyAttribute(band, valBandedCol, AName.@default);
                                valBandedCol.Add(new XAttribute(AName.name, transpTbl.Elements(EName.dimension_сolumns).Elements(EName.column).First().Attribute(AName.name).Value + "_" + dimVal.Attribute("value").Value + "_" + column_name));
                            }
                        } else {
                            band.SetAttrValue(AName.band_type, "band");
                        }
                    }
                } else {
                    band.SetAttrValue(AName.band_type, "band");
                }
            }
            // line 1427
            int presetCount = reportColumns.Elements(EName.column).Count(e => e.AttrOrEmpty(AName.table) == query.Attribute(AName.@as).Value);
            // columns/* | childs/table[@transposed]/columns/*
            foreach (XElement tblCol in Enumerable.Repeat(table, 1).Union(table.Elements(EName.childs).Elements(EName.table).Where(e => e.Attribute(AName.transposed) != null)).Elements(EName.columns).Elements()) {
                string tablePname = tblCol.Parent.Parent.Attribute(AName.@as).Value;
                string column_name = tblCol.Attribute(AName.name).Value;
                // .//column[@table='{0}' and @name='{1}']
                XElement repCol = reportColumns.Descendants(EName.column).FirstOrDefault(e => e.AttrOrEmpty(AName.table) == tablePname && e.AttrOrEmpty(AName.name) == column_name);
                // NB: column_name == repCol.Attribute(AName.name).Value
                if (repCol == null) {
                    repCol = new XElement(EName.column);
                    reportColumns.Add(repCol);
                    if (presetCount > 0) {
                        repCol.Add(new XAttribute(AName.visible, TextConst.AVBool.False));
                    }
                }
                // line 1445
                Cmn.CopyAttributeNoReplace(tblCol, repCol, AName.visible);
                string band_type = repCol.Parent.AttrOrDefault(AName.band_type, null);
                if (band_type == "dimband") {
                    if (repCol.Parent.Attribute(AName.title) == null) {
                        repCol.Parent.Add(new XAttribute(AName.title, tblCol.Attribute(AName.band_title).Value));
                    }
                } else if (band_type == "valband") {
                    if (repCol.Parent.Attribute(AName.title) == null) {
                        repCol.Parent.Add(new XAttribute(AName.title, tblCol.Attribute(AName.title).Value));
                    }
                    XAttribute attr = tblCol.Attribute(AName.band_title);
                    if (attr != null) {
                        repCol.SetAttrValue(AName.title, attr.Value);
                    }
                }
                // line 1470
                repCol.SetAttrValue(AName.table, tablePname);
                Cmn.CopyAttribute(tblCol, repCol, AName.name);
                // copyAttribute(tblCol, repCol, "format");
                Cmn.CopyAttribute(tblCol, repCol, AName.editable);
                Cmn.CopyAttribute(tblCol, repCol, AName.pivot);
                Cmn.CopyAttribute(tblCol, repCol, AName.dimname);
                Cmn.CopyAttribute(repCol, tblCol, AName.@default);
                Cmn.CopyAttributeNoReplace(tblCol, repCol, AName.agg);
                Cmn.CopyAttribute(repCol, tblCol, AName.agg);
                Cmn.CopyAttributeNoReplace(tblCol, repCol, AName.format);
                Cmn.CopyAttributeNoReplace(tblCol, repCol, AName.c_master);
                Cmn.CopyAttributeNoReplace(tblCol, repCol, AName.c_master_key);
                Cmn.CopyAttribute(repCol, tblCol, AName.format);
                Cmn.CopyAttribute(tblCol, repCol, AName.class_title);
                Cmn.CopyAttributeNoReplace(tblCol, repCol, AName.title);
                Cmn.CopyAttribute(tblCol, repCol, AName.type);
                Cmn.CopyAttribute(tblCol, repCol, AName.into);
                Cmn.CopyAttribute(tblCol, repCol, AName.client_calc);
                Cmn.CopyAttribute(tblCol, repCol, AName.excel_calc);
                repCol.SetAttrValue(AName.assigned, "1");
                // line 1495
                if (string.IsNullOrEmpty(query.AttrOrDefault(AName.parent_node_id, null))) {
                    if (column_name == TextConst.SpecCols.ParentGRowId || column_name == TextConst.SpecCols.GRowId) {
                        query.SetAttrValue(AName.parent_node_id, TextConst.SpecCols.ParentGRowId);
                        query.SetAttrValue(AName.node_id, TextConst.SpecCols.GRowId);
                    }
                }
                // line 1502
                string node_id = query.AttrOrEmpty(AName.node_id);
                if (column_name == node_id) {
                    repCol.SetAttrValue(AName.node_id, "1");
                }
                string pnode_id = query.AttrOrEmpty(AName.parent_node_id);
                if (column_name == pnode_id) {
                    repCol.SetAttrValue(AName.parent_node_id, "1");
                    tblCol.SetAttrValue(AName.parent_node_id, "1");
                }
                repCol.CopyAttributes(tblCol.Attributes().Where(APredicate.IsAdditionalAttribute));
            }
        }
        // Перевод XmlReports.setColumnsVisibility()
        private static void setColumnsVisibility(XElement report)
        {
            Contract.Assert(report != null);
            Contract.Assert(report.Name == EName.root);
            foreach (XElement col in report.Element(EName.scheme).Descendants(EName.viewcolumns).Descendants(EName.column)) {
                if (string.IsNullOrEmpty(col.AttrOrDefault(AName.title, null)) && col.Attribute(AName.visible) == null) {
                    col.Add(new XAttribute(AName.visible, TextConst.AVBool.False));
                }
            }
        }
        // XPath: [@transposed='1']
        private static bool IsTransposed(XElement el)
        {
            return el.AttrOrDefault(AName.transposed, false);
        }
        // XPath: [childs/table[@transposed='1']]
        private static bool HasChildTableTransposed(XElement el)
        {
            return el.Elements(EName.childs).Elements(EName.table).Any(IsTransposed);
        }
        // Перевод XmlReports.moveTransposedToParent()
        private static void moveTransposedToParent(XElement reportData)
        {
            Contract.Assert(reportData != null);
            Contract.Assert(reportData.Name == EName.root);
            // Убираем все band без title
            foreach (XElement band in reportData.Descendants(EName.viewcolumns).Descendants(EName.band).ToList()) {
                if (band.Attribute(AName.title) == null) {
                    band.Remove();
                }
            }
            // root/data//table/data/tr[childs/table[@transposed='1']]/cells
            foreach (XElement parentCells in reportData.Element(EName.data).Descendants(EName.table).Elements(EName.data).Elements(EName.tr)
                                                       .Where(HasChildTableTransposed).Elements(EName.cells)) {
                //childs/table[@transposed='1']/data/tr/cells/td
                foreach (XElement childCell in parentCells.Parent.Elements(EName.childs).Elements(EName.table).Where(IsTransposed).Elements(EName.data).Elements(EName.tr).Elements(EName.cells).Elements(EName.td).ToList()) {
                    childCell.Remove();
                    parentCells.Add(childCell);
                }
            }
            // root/scheme//table[childs/table[@transposed='1']]/columns
            foreach (XElement parentCols in reportData.Element(EName.scheme).Descendants(EName.table).Where(HasChildTableTransposed).Elements(EName.columns)) {
                // childs/table[@transposed='1']/columns/column
                foreach (XElement childCol in parentCols.Parent.Elements(EName.childs).Elements(EName.table).Where(IsTransposed).Elements(EName.columns).Elements(EName.column).ToList()) {
                    childCol.Remove();
                    parentCols.Add(childCol);
                }
            }
            // root/scheme//table[childs/table[@transposed='1']]
            foreach (XElement parentTable in reportData.Element(EName.scheme).Descendants(EName.table).Where(HasChildTableTransposed)) {
                // childs/table[@transposed='1']/dimension-values
                foreach (XElement dimInfo in parentTable.Elements(EName.childs).Elements(EName.table).Where(IsTransposed).Elements(EName.dimension_values).ToList()) {
                    dimInfo.SetAttrValue(AName.table, dimInfo.Parent.Attribute(AName.@as).Value);
                    dimInfo.Remove();
                    parentTable.Add(dimInfo);
                }
            }
            reportData.Descendants(EName.table).Where(IsTransposed).Remove();
            reportData.Descendants(EName.table).Elements(EName.childs).Where(e => !e.HasElements).Remove();
        }
        private static XElement GetDataByAlias(XElement el, string alias)
        {
            // root/data/table[@as='{0}']
            return el.Elements(EName.data).Elements(EName.table).SearchByAttribute(AName.@as, alias);
        }
        // Перевод XmlReports.moveUnitedToParent(), line 1771...1849
        private static void moveUnitedToParent(XElement reportData)
        {
            Contract.Assert(reportData != null);
            Contract.Assert(reportData.Name == EName.root);
            // line 1775
            //Пока только для случая объединения таблиц на верхнем уровне
            // root/scheme/table[@union='1']
            XElement mainTable = null;
            foreach (XElement unTable in reportData.Elements(EName.scheme).Elements(EName.table)) {
                // line 1777
                //XElement mainTable = unTable.SelectSingleNode("preceding::table[not(@union='1')]");
                if (!unTable.AttrOrDefault(AName.union, false)) {
                    mainTable = unTable;
                } else {
                    // line 1778
                    SortedList<int, int> colCor = new SortedList<int, int>();
                    SortedList<string, int> unColInd = new SortedList<string, int>();
                    int mainIndex = 0;
                    int unIndex = 0;
                    foreach (XElement unCol in unTable.Elements(EName.columns).Elements(EName.column)) {
                        unColInd.Add(unCol.Attribute(AName.name).Value, unIndex);
                        unIndex++;
                    }
                    int index;
                    foreach (XElement mainCol in mainTable.Elements(EName.columns).Elements(EName.column)) {
                        if (unColInd.TryGetValue(mainCol.Attribute(AName.name).Value, out index)) {
                            colCor.Add(mainIndex, index);
                        }
                        mainIndex++;
                    }
                    // line 1795
                    unTable.Remove();
                    XElement unTableData = GetDataByAlias(reportData, unTable.Attribute(AName.@as).Value);
                    if (unTableData != null) {
                        XElement mainData = GetDataByAlias(reportData, mainTable.Attribute(AName.@as).Value).Element(EName.data);
                        // data/tr/cells
                        foreach (XElement unRow in unTableData.Elements(EName.data).Elements(EName.tr).Elements(EName.cells)) {
                            mainIndex = 0;
                            XElement tr = new XElement(EName.tr);
                            mainData.Add(tr);
                            tr.CopyAttributes(unRow.Attributes());
                            XElement trCells = new XElement(EName.cells);
                            tr.Add(trCells);
                            List<XElement> data = unRow.Elements(EName.td).ToList();
                            foreach (XElement mainCol in mainTable.Elements(EName.columns).Elements(EName.column)) {
                                XElement td;
                                if (colCor.TryGetValue(mainIndex, out index)) {
                                    td = new XElement(data[index]);
                                } else {
                                    td = new XElement(EName.td);
                                }
                                trCells.Add(td);
                                mainIndex++;
                            }
                        }
                        unTableData.Remove();
                    }
                }
            }
            // line 1824
            // root/data//table/data/tr[childs/table[@transposed='1']]/cells
            foreach (XElement parentCells in reportData.Elements(EName.root).Elements(EName.data).Descendants(EName.table).Elements(EName.data).Elements(EName.tr).Where(HasChildTableTransposed).Elements(EName.cells)) {
                // childs/table[@transposed='1']/data/tr/cells/td
                foreach (XElement childCell in parentCells.Parent.Elements(EName.childs).Elements(EName.table).Where(IsTransposed).Elements(EName.data).Elements(EName.tr).Elements(EName.cells).Elements(EName.td).ToList()) {
                    childCell.Remove();
                    parentCells.Add(childCell);
                }
            }
            // line 1832
            // root/scheme//table[childs/table[@transposed='1']]/columns
            foreach (XElement parentCols in reportData.Elements(EName.root).Elements(EName.scheme).Descendants(EName.table).Where(HasChildTableTransposed).Elements(EName.columns)) {
                // childs/table[@transposed='1']/columns/column
                foreach (XElement childCol in parentCols.Parent.Elements(EName.childs).Elements(EName.table).Where(IsTransposed).Elements(EName.columns).Elements(EName.column).ToList()) {
                    childCol.Remove();
                    parentCols.Add(childCol);
                }
            }
            // line 1842
            // //table[@transposed='1']
            reportData.Descendants(EName.table).Where(IsTransposed).Remove();
        }
    }
}