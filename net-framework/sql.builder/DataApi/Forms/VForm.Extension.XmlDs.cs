using System;
using System.Diagnostics; // Debug, Stopwatch
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;
using sql.builder.FieldInfo;
using sql.builder.XmlHelpers;
using sql.builder.Exceptions;
using _AName = sql.builder.DataApi.AName; // из-за конфликта с экземплярным методом VForm.AName()
//using sql.builder.WebReports;

namespace sql.builder.DataApi
{
    internal partial class VForm
    {
        internal static void MultiplicateSources(XElement compiledQuery, VDataSet dataSet, string keyDimension)
        {
            HashSet<string> tabsToMultiplicate = new HashSet<string>();
            for (int index = 0; index < dataSet.Tables.Count; index++) {
                VDataTable dt = (VDataTable)(dataSet.Tables[index]);
                if (dt.NewRowsVisForOtherTbls) {
                    tabsToMultiplicate.Add(dt.UpdateableTableName);
                }
            }
            List<XElement> mpQrys = new List<XElement>();
            foreach (XElement q in compiledQuery.Descendants(EName.query)) {
                if (q.AttrOrDefault(_AName.multiplicate_point, false) || q.AttrOrDefault(_AName.link_mp_point, false)) {
                    mpQrys.Add(q);
                }
            }
            foreach (XElement qry in mpQrys)
            {
                XElement[] tables = qry.Descendants(EName.table)
                    .Where(t => tabsToMultiplicate.Contains(t.AttrOrDefault(_AName.name, string.Empty)) && t.AttrOrDefault(_AName.dimension, string.Empty) != keyDimension).ToArray();
                //var tblNames = tables
                //        .Select(t1=>t1.Attribute(TextConst.AName.Name).Value).Distinct().ToList();
                List<string> tblNames = tables.Select(t1 => t1.Ancestors(EName.query).First().Attribute(_AName.name).Value).Distinct().ToList();
                int tblsColunt = tblNames.Count;
                if (tblsColunt > 0)
                {
                    //var variantsCount = Math.Pow(2, tblsColunt);
                    int variantsCount = 1 << tblsColunt;  // комбинация всех вариантов из temp и оригинальной таблицы
                    for (int i = 0; i < variantsCount; i++)
                    {
                            var qry1 = new XElement(qry);
                            var bits = new BitArray(BitConverter.GetBytes(i));
                            for (int j = 0; j < tblsColunt; j++)
                            {
                               // var tbls = qry1.Descendants(TextConst.EName.Table).Where(t => Cmn.GetAttrValue(t, TextConst.AName.Name) == tblNames[j]).ToList();
                                var qrys = qry1.Descendants(EName.query).Where(q => q.AttrOrDefault(_AName.name, string.Empty) == tblNames[j]).ToArray();
                                var tbls = qrys.Elements(EName.from).Elements(EName.table).ToList();
                                foreach (XElement tbl in tbls)
                                {
                                    tbl.SetAttrValue(_AName.is_from_temp, bits[j]);
                                }
                            }
                            if (qry.AttrOrDefault(_AName.link_mp_point, false))
                            {
                                string alias = qry.Attribute(_AName.@as).Value;
                                string newAlias = alias;
                                if (i > 0)
                                {
                                    newAlias = alias + TextConst.Pfx.MpVariant + i.ToString();
                                    qry1.RemoveAttribute(_AName.link_mp_point);
                                    qry1.SetAttributeValue(_AName.@as, newAlias);
                                }
                                var joinCols = qry1.Elements(EName.call).Descendants(EName.column).Where(e => e.AttrOrDefault(_AName.table, string.Empty) == alias).ToList();
                                foreach (XElement col in joinCols)
                                {
                                    col.SetAttributeValue(_AName.table, newAlias);
                                }
                            }
                            qry.AddAfterSelf(qry1);
                    }
                    qry.Remove();
                }
            }
            List<XElement> mpLinkQrys = new List<XElement>();
            foreach (XElement q in compiledQuery.Descendants(EName.query)) {
                if (q.AttrOrDefault(_AName.link_mp_point, false)) {
                    mpLinkQrys.Add(q);
                }
            }
            foreach (XElement qry in mpLinkQrys)
            {
                List<string> tblNames = qry.Descendants(EName.table)
                        .Where(t => tabsToMultiplicate.Contains(t.AttrOrDefault(_AName.name, string.Empty)) && t.AttrOrDefault(_AName.dimension, string.Empty) != keyDimension)
                        .Attributes(_AName.name).Select(APredicate.AttributeValue).Distinct().ToList();
                int tblsColunt = tblNames.Count;
                if (tblsColunt > 0)
                {
                    // var variantsCount = Math.Pow(2, tblsColunt);
                    int variantsCount = 1 << tblsColunt;  // комбинация всех вариантов из temp и оригинальной таблицы
                    if (qry.AttrOrDefault(_AName.link_mp_point, false))
                    {
                        XElement queryUser = qry.Parent.Parent;
                        List<XElement> xcolsAll = Compiler.getQueryColumns(queryUser).ToList();
                        List<XElement> xcolsAllJoin = Compiler.getQueryJoinColumns(queryUser).ToList();
                        xcolsAll.AddRange(xcolsAllJoin);
                        string alias = qry.Attribute(_AName.@as).Value;
                        List<XElement> usedColumns = xcolsAll.Where(e => e.Attribute(_AName.table).Value == alias).ToList();
                        foreach (XElement col in usedColumns)
                        {
                            var expr = new XElement(EName.call, new XAttribute(_AName.function, TextConst.AVFunction.Coalesce));
                            expr.CopyAttributes(col.Attributes().Where(APredicate.IsCallAttributes));
                            expr.CopyAttributes(col.Attributes(_AName.group));
                            col.RemoveAttribute(_AName.group);
                            for (int i = 0; i < variantsCount; i++)
                            {
                                string newAlias = alias;
                                if (i > 0)
                                {
                                    newAlias = alias + TextConst.Pfx.MpVariant + i.ToString();
                                }
                                var newCol = new XElement(col);
                                newCol.SetAttributeValue(_AName.table, newAlias);
                                expr.Add(newCol);
                            }
                            expr = Compiler.expression(expr, null).First() as XElement;
                            col.AddAfterSelf(expr);
                            col.Remove();
                        }
                    }
                }
                if (mpLinkQrys.Any())
                {
                    Compiler.CutIdentifiersTo30(compiledQuery);
                }
            }
        }
        private static XElement CreateDataSetAndFormInfo(string name,  bool use_cache = true)
        {
            string cashName = name;


            //if (WebReportsAdapter.IsWebItem(name))
            //{
            //    VForm vform = XmlReports.Environment.GetFormOrQueryAsForm(name).GetPreprocessed();
            //    return vform.CreateDataSetAndFormInfo();
            //}

            if (!Cache.UseFormsCache || !use_cache)
            {
                VCashUtils.ClearCash();
            }
            if (IsStaticCashValueExists(MethodBase.GetCurrentMethod().ToString(), cashName))
            {
                return (GetStaticCashValue(MethodBase.GetCurrentMethod().ToString(), cashName) as XElement);
            }
            XElement xroot = null;
            if (Cache.UseFormsCache && use_cache)
            {
                xroot = Cache.GetActualForm(cashName);
            }
            if (xroot == null)
            {
                VForm vform = XmlReports.Environment.GetFormOrQueryAsForm( name).GetPreprocessed();
                VExceptionController.BeginProcessingElement(vform);
                xroot = vform.CreateDataSetAndFormInfo();
                //ds = vform.ProcessAndCreateDataSet();
                //xform = vform.GetFormXElement();
                VExceptionController.EndProcessingElement(vform);
            }
            AddStaticCashValue(xroot, MethodBase.GetCurrentMethod().ToString(), cashName);
            return xroot;
        }
        private XElement CreateDataSetAndFormInfo()
        {
            #if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #endif
            string cashName = this.P_IdName;
            //if (!Cache.UseFormsCache || !use_cache)
            //{
            //    VCashUtils.ClearCash();
            //}
            //if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), cashName))
            //{
            //    return (GetCashValue(MethodBase.GetCurrentMethod().ToString(), cashName) as XElement);
            //}
            // кэш
            XElement xroot = null; 
            //if (Cache.UseFormsCache && use_cache)
            //{
            //    xroot = Cache.GetActualForm(cashName);
            //}
            //if (xroot != null)
            //{
            //    AddCashValue(xroot, MethodBase.GetCurrentMethod().ToString(), cashName);
            //    return xroot;
            //}
            xroot = new XElement(EName.root);
            XElement xform = this.GetFormXElement();
            xroot.Add(xform);
            VSXElement vparams = this.ParamsElement();
            if (vparams != null)
            {
                var xparams = vparams.AsXElementApplyingParts();
                xroot.Add(xparams);
            }
            XElement xds = null;
            xds = new XElement(EName.dataset);
            xroot.Add(xds);
            xds.Add(ParamsElement());
            var xfields = new XElement(EName.fields);
            foreach (VSXElement fld in Fields())
            {
                XElement xfield = AddNewField(xfields, fld.P_FormalParNameS, fld.XDataType());
                xfield.Add(new XAttribute(_AName.title, fld.P_Title));
                //if (fld.P_ColumnEditable != "")
                //{
                //    xfield.SetAttributeValue(TextConst.AName.ColumnEditable, fld.P_ColumnEditable);
                //}
                WriteColumnBehaviorInfo(fld, xfield, TextConst.DsAName.NewVal, false);
                WriteColumnBehaviorInfo(fld, xfield, TextConst.DsAName.Editable, false);
                WriteColumnBehaviorInfo(fld, xfield, TextConst.DsAName.Mandatory, false);
                WriteColumnBehaviorInfo(fld, xfield, TextConst.DsAName.Default, false);
                WriteColumnBehaviorInfo(fld, xfield, TextConst.DsAName.Visible, false);
                WriteColumnBehaviorInfo(fld, xfield, TextConst.DsAName.Valid, false);
            }
            AddNewField(xfields, TextConst.AVParam.FormValid,    TextConst.AVDataType.String);
            AddNewField(xfields, TextConst.AVParam.FormValidNot, TextConst.AVDataType.String);
            foreach (VQueryCall qry in this.MainAndRelatedQueries())
            {
                string name = qry.XName + TextConst.AVParam.HasChanges;
                AddNewField(xfields, name, TextConst.AVDataType.String);
            }
            xds.Add(xfields);
            var queryCalls = new SortedList<string, VQueryCall>();
            var xtables = new SortedList<string, XElement>();
            var refreshedColumns = new SortedList<string, List<VColumn>>();
            var updatableColumns = new SortedList<string, List<VColumn>>();
            foreach (VQueryCall query in this.MainQueries())
            {
                queryCalls.Add(query.XName, query);
                XElement xtbl = this.CreateTableInfo(query);
                xtables.Add(query.XName, xtbl);
                xtbl.SetAttributeValue(_AName.is_top, TextConst.AVBool.True);
                xds.Add(xtbl);
                foreach (VELink elink in query.GetDescedantsP(EName.elink))
                {
                    queryCalls.Add(elink.XName, elink);
                    xtbl = this.CreateTableInfo(elink);
                    xtables.Add(elink.XName, xtbl);
                    xds.Add(xtbl);
                }
            }
            VDataSet dataSet = VForm.CreateDataSetPre(xds);
            var actionsCalls = VSXElement.GetDescedantsP(this).Where(e => e is VAction).ToList();
            var resetActionsCalls = actionsCalls.Where(e => e.P_ActionType == TextConst.AVActionType.ResetColumn).Select(e1=>e1.P_CalledObject+"."+e1.P_Column).ToArray();
            foreach (VQueryCall queryCall in queryCalls.Values)
            {
                XElement xtable = xtables[queryCall.XName];
                VDataTable table = (VDataTable)dataSet.Tables[xtable.Attribute(_AName.name).Value];
                #region singleRow
                string key = xtable.AttrOrDefault(_AName.key, string.Empty);
                List<XElement> xupdatebleColumns = new List<XElement>();
                List<XElement> xupdatebleColumnsExt = new List<XElement>();
                List<XElement> xkeyColumns = new List<XElement>();
                List<XElement> xrefreshedColumns = new List<XElement>();
                List<XElement> xresetedColumns = new List<XElement>();
                foreach (XElement e in xtable.Element(EName.columns).Elements()) {
                    if (e.AttrOrDefault(_AName.is_updateable, false)) {
                        xupdatebleColumns.Add(e);
                    }
                    if (e.AttrOrDefault(_AName.is_updateable_ext, false)) {
                        xupdatebleColumnsExt.Add(e);
                    }
                    string col_name = e.AttrOrDefault(_AName.name, string.Empty);
                    if (col_name == key) {
                        xkeyColumns.Add(e);
                    }
                    if (e.AttrOrDefault(_AName.is_refreshed, false)) {
                        xrefreshedColumns.Add(e);
                    }
                    if (resetActionsCalls.Contains(e.AttrOrDefault(_AName.table, string.Empty) + "." + col_name)) {
                        xresetedColumns.Add(e);
                    }
                }
                //var pars = getOracleParams(xupdatebleColumns).ToArray();
                List<VColumn> columns = this.getColumns(queryCall);
                XElement xscheme = this.createTableScheme(queryCall, columns, false);
                WriteAttrAsElem(xtable, EName.scheme, xscheme);
                VColumn keyColumn = columns.Where(e => e.IsKey).FirstOrDefault();
                // VColumn keySource = (keyColumn.SourceColumn().First() as VColumn);
                // string tableName = keySource.Source().Attribute(TextConst.AName.Name).Value;
                var vtbls = new SortedList<string, VSXElement>();
                VTable vtbl = null;
                if (keyColumn != null)
                {
                    vtbl = (keyColumn.SourceColumn().First().RootQuery() as VQuery).SourceTable();
                    //if (vtbl != null)
                    //{
                    //    vtbls.Add(keyColumn.P_Table,vtbl);
                    //}
                    foreach (XElement col in xupdatebleColumns)
                    {
                        string tblName = col.AttrOrDefault(_AName.source_table, string.Empty);
                        if (tblName != string.Empty && !vtbls.ContainsKey(tblName))
                        {
                            VSXElement keyCol = XmlReports.Environment.GetQuery(tblName).KeyColumn();
                            vtbls.Add(tblName, keyCol);
                        }
                    }
                }
                string tableName = null;
                VDimension qdim = queryCall.Query().GetDimension();
                string keyDimension = null;
                if (qdim != null)
                {
                    keyDimension = qdim.P_IdName;
                    xtable.SetAttributeValue(_AName.key_dimension, keyDimension);
                }
                if (vtbl != null || vtbls.Count > 0) 
                {
                //foreach (var tt in vtbls)
                //{
                    if (vtbl != null)
                    {
                        tableName = vtbl.P_Name;
                    }
                    var qry = this.createTableQuery(queryCall, columns, true, true);
                    var singleRowQuery = new XElement(qry);
                    XElement xparams = singleRowQuery.Element(EName.@params);
                    if (xparams == null)
                    {
                        xparams = new XElement(EName.@params);
                        singleRowQuery.AddFirst(xparams);
                    }
                    string keyParName = keyColumn.XName + TextConst.Pfx.PrimaryKeyParam;
                    XElement xKeyPar = AddNewParam(xparams, keyParName, TextConst.AVDataType.Number);
                    AddNewParam(xparams, TextConst.DBParams.IsNewRowParam, TextConst.AVDataType.Number);
                    AddNewParam(xparams, TextConst.DBParams.FormId, TextConst.AVDataType.Number);
                    AddNewParam(xparams, TextConst.DBParams.TempRowId, TextConst.AVDataType.Number);
                    singleRowQuery.Elements(EName.where).Remove();
                    singleRowQuery.Elements(EName.having).Remove();
                    if (vtbls.Count > 0)
                    {
                        var xwhere = new XElement(EName.where,
                                         Factory.NewCall(TextConst.AVFunction.In,
                                             Factory.NewColumn(table: keyColumn.P_Table, column: keyColumn.P_Column),
                                             Factory.NewUseParam(keyParName)
                                         )
                                     );
                        singleRowQuery.Add(xwhere);
                    }
                    VReport.PreprocessSimpleParams(singleRowQuery);
                    var preCompiledQuery = this.GetCompiledQuery(singleRowQuery, queryCall.XName + "-single");
                    var singleRowPreCompiledQuery = new XElement(preCompiledQuery);
                    MultiplicateSources(singleRowPreCompiledQuery, table.GetDataSet(), keyDimension);
                    if (vtbl != null)
                    {
                        ChangeQueryTableForUsingTemp(singleRowPreCompiledQuery, tableName, table, null, true, keyParName, keyDimension);
                    }
                    foreach (VDataTable tbl in table.DataSet.Tables)
                    {
                        if (!string.IsNullOrEmpty(tbl.UpdateableTableName))
                        {
                            // var otherQueryCall = queryCalls[tbl.TableName];
                            ChangeQueryTableForUsingTemp(singleRowPreCompiledQuery, tbl.UpdateableTableName, tbl, null, false, null, keyDimension);
                        }
                    }
                    XElement singleRowCompiledQuery = Compiler.FinalProcessingQuery(singleRowPreCompiledQuery);
                    table.SingleRowRefreshCommand = VDBSelectCommand.CreateFromCompiledQuery(singleRowQuery, singleRowCompiledQuery);
                    XElement xml = table.SingleRowRefreshCommand.ToXml();
                    WriteAttrAsElem(xtable, EName.single_row_refresh_cmd, xml);
                    // table.SingleRowRefreshCommand = VDBSelectCommand.FromXml(xml);
                #endregion
                #region ValueRefresh
                foreach (XElement xcol in xrefreshedColumns)
                {
                        string col_name = xcol.Attribute(_AName.name).Value;
                        VColumn col = columns.First(e => e.XName == col_name);
                        XElement colValQuery = createTableQueryForSpcifiedColumn(col, keyColumn);
                        xparams = colValQuery.Element(EName.@params);
                        if (xparams == null) {
                            xparams = new XElement(EName.@params);
                            colValQuery.AddFirst(xparams);
                        }
                        xparams.Add(new XElement(xKeyPar));
                        AddNewParam(xparams, TextConst.DBParams.IsNewRowParam, TextConst.AVDataType.Number);
                        AddNewParam(xparams, TextConst.DBParams.FormId, TextConst.AVDataType.Number);
                        AddNewParam(xparams, TextConst.DBParams.TempRowId, TextConst.AVDataType.Number);
                        colValQuery.Elements(TextConst.EName.Where).Remove();
                        colValQuery.Elements(TextConst.EName.Having).Remove();
                        foreach (var tt in vtbls)
                        {// VSXElement
                            AddNewParam(xparams, tt.Key + TextConst.Pfx.PrimaryKeyParam, TextConst.AVDataType.Number);
                        }
                        if (vtbls.Count > 0)
                        {
                            var xwhere = new XElement(EName.where,
                                             Factory.NewCall(TextConst.AVFunction.Equal,
                                                 Factory.NewColumn(table: keyColumn.P_Table, column: keyColumn.P_Column),
                                                 Factory.NewUseParam(keyParName)
                                             )
                                         );
                            colValQuery.Add(xwhere);
                        }
                        VReport.PreprocessSimpleParams(colValQuery);
                        XElement compiledColValQuery = GetCompiledQuery(colValQuery, queryCall.XName + "-" + col.XName);
                        MultiplicateSources(compiledColValQuery, table.GetDataSet(), keyDimension);
                        var curDataCol = (VDataColumn)table.Columns[col.XName];
                        if (vtbl != null)
                        {
                            ChangeQueryTableForUsingTemp(compiledColValQuery, tableName, table, curDataCol, true, keyParName, keyDimension);
                        }
                        foreach (VDataTable tbl in table.DataSet.Tables)
                        {
                            if (!string.IsNullOrEmpty(tbl.UpdateableTableName))
                            {
                                // var otherQueryCall = queryCalls[tbl.TableName];
                                ChangeQueryTableForUsingTemp(compiledColValQuery, tbl.UpdateableTableName, tbl, curDataCol, false, null, keyDimension);
                            }
                        }
                        foreach (var tt in vtbls)
                        {
                            ChangeQueryTableForUsingTemp(compiledColValQuery, tt.Key, table, curDataCol, true, keyParName, keyDimension, false, tt.Value.XName, tt.Key + TextConst.Pfx.PrimaryKeyParam, tt.Value.GetColumnTempName());
                        }
                        compiledColValQuery = Compiler.FinalProcessingQuery(compiledColValQuery);
                        VDBSelectCommand cmd = VDBSelectCommand.CreateFromCompiledQuery(colValQuery, compiledColValQuery);
                        curDataCol.ValueRefreshCommand = cmd;
                        xml = curDataCol.ValueRefreshCommand.ToXml();
                        WriteAttrAsElem(xcol, EName.value_refresh_cmd, xml);
                }
                foreach (var xcol in xresetedColumns)
                {
                        var col = (VColumn)columns.First(e => e.XName == xcol.Attribute(TextConst.AName.Name).Value);
                        XElement colValQuery = createTableQueryForSpcifiedColumn(col, keyColumn);
                        xparams = colValQuery.Element(EName.@params);
                        if (xparams == null)
                        {
                            xparams = new XElement(EName.@params);
                            colValQuery.AddFirst(xparams);
                        }
                        xparams.Add(new XElement(xKeyPar));
                        colValQuery.Elements(TextConst.EName.Where).Remove();
                        colValQuery.Elements(TextConst.EName.Having).Remove();
                        VReport.PreprocessSimpleParams(colValQuery);
                        XElement compiledColValQuery = GetCompiledQuery(colValQuery, queryCall.XName + "-" + col.XName);
                        var curDataCol = (VDataColumn)table.Columns[col.XName];
                        if (vtbl != null)
                        {
                            ChangeQueryTableForUsingTemp(compiledColValQuery, tableName, table, curDataCol, true, keyParName, keyDimension,true);
                        }
                        var compiledColValQuery1 = Compiler.FinalProcessingQuery(new XElement(compiledColValQuery));
                        VDBSelectCommand cmd1 = VDBSelectCommand.CreateFromCompiledQuery(new XElement(colValQuery), compiledColValQuery1);
                        curDataCol.ValueResetCommand = cmd1;
                        var xml1 = curDataCol.ValueResetCommand.ToXml();
                        WriteAttrAsElem(xcol, EName.value_reset_cmd, xml1);
                    }
                #endregion
                }
            }
            IList<VQueryCall> qCalls = queryCalls.Values;
            //qCalls = new List<VQueryCall>();
            foreach (VQueryCall queryCallMain in qCalls)
            {
                #region DependantsRefresh 
                // !! 3-й раз тоже самое. Но могут быть отличия. Потом объединить
                XElement xtable = xtables[queryCallMain.XName];
                var tableMain = (VDataTable)dataSet.Tables[xtable.Attribute(_AName.name).Value];
                var xcols = xtable.Element(EName.columns).Elements();
                foreach (VDataColumn colMain in tableMain.Columns.Cast<VDataColumn>().Where(c =>c.Dependants!=null && c.Dependants.Any()).ToList())
                {
                    XElement xcol = xcols.First(e => e.Attribute(_AName.name).Value == colMain.ColumnName);
                    var dependants = colMain.Dependants;
                    List<VDataTable> depTables = dependants.SelectAsArray(d => d.GetTable()).Distinct().ToList();
                    foreach (var table in depTables)
                    {
                        List<VDataColumn> depCols = dependants.Where(d => d.GetTable() == table).ToList();
                        VQueryCall queryCall = queryCalls[table.TableName];
                        string[] depColsNames = depCols.SelectAsArray(Cmn.GetDataColumnName);
                        List<VColumn> columns1 = this.getColumns(queryCall);
                        VDimension qdim = queryCall.Query().GetDimension();
                        string keyDimension = null;
                        if (qdim != null)
                        {
                            keyDimension = qdim.P_IdName;
                        }
                        VColumn keyColumn = columns1.FirstOrDefault(e => e.IsKey);
                        List<VColumn> xdepCols = columns1.Where(e => depColsNames.Contains(e.XName)).ToList();
                        XElement colValQuery = this.createTableQueryForSpcifiedColumns(queryCall, xdepCols, keyColumn);
                        VTable vtbl = null;
                        if (keyColumn != null)
                        {
                            vtbl = (keyColumn.SourceColumn().First().RootQuery() as VQuery).SourceTable();
                        }
                        string tableName = null;
                        if (vtbl != null)
                        {
                            tableName = vtbl.P_Name;
                            XElement xparams = colValQuery.Element(EName.@params);
                            if (xparams == null)
                            {
                                xparams = new XElement(EName.@params);
                                colValQuery.AddFirst(xparams);
                            }
                            string keyParName = keyColumn.XName + TextConst.Pfx.PrimaryKeyParam;
                            XElement xKeyPar = AddNewParam(xparams, keyParName, TextConst.AVDataType.Number);
                            AddNewParam(xparams, TextConst.DBParams.IsNewRowParam, TextConst.AVDataType.Number);
                            AddNewParam(xparams, TextConst.DBParams.FormId, TextConst.AVDataType.Number);
                            AddNewParam(xparams, TextConst.DBParams.TempRowId, TextConst.AVDataType.Number);
                            colValQuery.Elements(TextConst.EName.Where).Remove();
                            colValQuery.Elements(TextConst.EName.Having).Remove();
                            VReport.PreprocessSimpleParams(colValQuery);
                            XElement compiledColValQuery = this.GetCompiledQuery(colValQuery, queryCall.XName + "-DEP-" + colMain.ColumnName);
                            MultiplicateSources(compiledColValQuery, table.GetDataSet(), keyDimension);
                            VDataColumn curDataCol = null;// (VDataColumn)table.Columns[col.XName];
                            if (vtbl != null)
                            {
                                ChangeQueryTableForUsingTemp(compiledColValQuery, tableName, table, curDataCol, true, keyParName, keyDimension);
                            }
                            foreach (VDataTable tbl in table.DataSet.Tables)
                            {
                                if (!string.IsNullOrEmpty(tbl.UpdateableTableName))
                                {
                                    // var otherQueryCall = queryCalls[tbl.TableName];
                                    ChangeQueryTableForUsingTemp(compiledColValQuery, tbl.UpdateableTableName, tbl, curDataCol, false, null, keyDimension);
                                }
                            }
                            compiledColValQuery = Compiler.FinalProcessingQuery(compiledColValQuery);
                            VDBSelectCommand cmd = VDBSelectCommand.CreateFromCompiledQuery(colValQuery, compiledColValQuery);
                            //curDataCol.ValueRefreshCommand = cmd;
                            XElement xml = cmd.ToXml();
                            xml.SetAttributeValue(_AName.table, table.TableName);
                            xcol.Add(new XElement(EName.dep_refresh_cmd, xml));
                            //WriteAttrAsElem(xcol, TextConst.DsEName.DepRefreshCommand, xml);
                        }
                    }
                    //compiledColValQuery = Compiler.FinalProcessingQuery(compiledColValQuery);
                    //var cmd = new VDBSelectCommand(colValQuery, compiledColValQuery);
                    //curDataCol.ValueRefreshCommand = cmd;
                    //xml = curDataCol.ValueRefreshCommand.ToXml();
                    //WriteAttrAsElem(xcol, TextConst.DsEName.ValueRefreshCmd, xml);
                }
                #endregion
            }
            foreach (VDataTable table in dataSet.Tables)
            {
                if (table == dataSet.ParamsTable) continue;
                foreach (VDataColumn col in table.Columns)
                {
                    XElement xtable = xtables[table.TableName];
                    XElement xcol = xtable.Element(EName.columns).Elements().First(e => e.Attribute(_AName.name).Value == col.ColumnName);
                    if (col.Dependants != null)
                    {
                        var xdependants = new XElement(EName.dependants);
                        foreach (VDataColumn dep in col.Dependants)
                        {
                            var xdependant = new XElement(EName.dependant);
                            xdependant.Add(new XAttribute(_AName.name, dep.ColumnName));
                            xdependant.Add(new XAttribute(_AName.table, dep.Table.TableName));
                            xdependants.Add(xdependant);
                        }
                        xcol.Add(xdependants);
                    }
                }
            }
            this.AddCashValue(xroot, MethodBase.GetCurrentMethod().ToString(), cashName);
            //if (!XmlReports.IsInfoenergo) {
            //    if (!WebReportsAdapter.IsWebItem(cashName))
            //    {
            //        Cache.SaveNotActualForm(xroot, cashName);
            //    }
              
            //}
            #if DEBUG
            sw.Stop();
            Debug.WriteLine("VForm.CreateDataSetAndFormInfo(), " + cashName + ": " + sw.ElapsedMilliseconds.ToString() + " мс");
            #endif
            return xroot;
        }
        private XElement CreateTableInfo(VQueryCall queryCall)
        {
            VQuery srcQuery = queryCall.Query();
            var xtbl = new XElement(EName.table);
            xtbl.Add(new XAttribute(_AName.name,               queryCall.XName));
            xtbl.Add(new XAttribute(_AName.auto_refresh,        queryCall.P_AutoRefresh));
            xtbl.Add(new XAttribute(_AName.async,              queryCall.P_Async));
            xtbl.Add(new XAttribute(_AName.only_visible_refresh, queryCall.P_OnlyVisibleRefresh));
            xtbl.Add(new XAttribute(_AName.only_force_refresh,   queryCall.P_OnlyForceRefresh));
            if (srcQuery.IsNonDb())
            {
                xtbl.Add(new XAttribute(_AName.non_db, TextConst.AVBool.True));
            }
            if (queryCall.P_Column != null)
            {
                xtbl.Add(new XAttribute(_AName.ref_column, queryCall.P_Column));
            }
            if (queryCall.P_MultiSelectColumn != null)
            {
                xtbl.Add(new XAttribute(_AName.multi_select_column, queryCall.P_MultiSelectColumn));
            }
            if (queryCall.P_MultiSelectTarget != null)
            {
                xtbl.Add(new XAttribute(_AName.multi_select_target, queryCall.P_MultiSelectTarget));
            }
            if (queryCall.P_NewRowsVisForOtherTbls == TextConst.AVBool.True)
            {
                xtbl.Add(new XAttribute(_AName.new_rows_vis_for_other_tbls, TextConst.AVBool.True));
            }
            List<VColumn> columns = this.getColumns(queryCall);
            VColumn keyColumn = columns.Where(e => e.IsKey).FirstOrDefault();
            if (keyColumn != null)
            {
                xtbl.Add(new XAttribute(_AName.update_target, srcQuery.GetMainIE().P_IdName));
            }
            if (queryCall.P_ColumnEditable != "")
            {
                xtbl.Add(new XAttribute(_AName.column_editable, queryCall.P_ColumnEditable));
            }
            if (srcQuery.P_DeleteValidation != "")
            {
                xtbl.Add(new XAttribute(_AName.delete_validation, srcQuery.P_DeleteValidation.SubstringAfter('.')));
            }
            if (queryCall.P_BackColor != "")
            {
                xtbl.Add(new XAttribute(_AName.color, queryCall.P_BackColor));
            }
            if (queryCall.P_CanBeChecked != "")
            {
                xtbl.Add(new XAttribute(_AName.can_be_checked, queryCall.P_CanBeChecked));
            }
            var xscheme = this.createTableScheme(queryCall, columns, false);
            WriteAttrAsElem(xtbl, EName.scheme, xscheme);
            if (keyColumn != null)
            {
                xtbl.Add(new XAttribute(_AName.key, keyColumn.XName));
            }
            if (queryCall is VELink)
            {
                string parentTableName = queryCall.GetParent().XName;
                VRelation rel = queryCall.GetRelation();
                string foreinKey = rel.ChildColumnSource().XName;
                xtbl.Add(new XAttribute(_AName.parent_table, parentTableName));
                xtbl.Add(new XAttribute(_AName.parent_key, foreinKey));
            }
            XElement qry = this.createTableQuery(queryCall, columns, false, true);
            VReport.PreprocessSimpleParams(qry);
            //table.XQuery = qry;
            qry.SetAttributeValue(_AName.comment, "form " + this.XName);
            XElement preCompiledQuery = this.GetCompiledQuery(qry, queryCall.XName);
            XElement compiledQuery = Compiler.FinalProcessingQuery(new XElement(preCompiledQuery));
            string selectText = Compiler.GetQuerySelectStatmentFromCompiledQuery(compiledQuery);
            WriteAttrAsElem(xtbl, EName.select_text, selectText);
            string procText = Compiler.GetQuerProcedureFromCompiledQuery(compiledQuery);
            if (procText != null)
            {
                WriteAttrAsElem(xtbl, EName.proc_text, procText);
            }
            var updatebleColumns = new SortedList<string, List<VColumn>>();
            var updatebleColumnsExt = new SortedList<string, List<VColumn>>();
            var xupdatebleColumnsExt = new SortedList<string, List<XElement>>();
            var otherUpdateableColumns = new SortedList<string, List<XElement>>();
            var refreshedColumns = new List<VColumn>();
            var updQueries = new SortedList<string, VQueryCall>();
            var mainQname=queryCall.XName;
            var usedUpdColumns = new SortedList<string, List<string>>();
            usedUpdColumns.Add(mainQname, new List<string>());
            updatebleColumns.Add(mainQname, new List<VColumn>());
            updatebleColumnsExt.Add(mainQname, new List<VColumn>());
            xupdatebleColumnsExt.Add(mainQname,new List<XElement>());
            otherUpdateableColumns.Add(mainQname, new List<XElement>());
            updQueries.Add(mainQname, queryCall);
            XElement xcolumns = new XElement(EName.columns);
            xtbl.Add(xcolumns);
           // SortedList<string, int> typesInd = new SortedList<string, int>();
            List<string> jColsNames = srcQuery.AllSources()
                .Where(e2 => e2.P_Updateable==TextConst.AVBool.True)
                .SelectMany(q => q.GetElementsP().First().GetDescedantsP(EName.column))
                .Where(e => e.AttrOrDefault(_AName.table, string.Empty) == TextConst.AVTable.Ths)
                .Select(e1 => e1.AttrOrDefault(_AName.column, string.Empty)).Distinct().ToList();
            var xtraKeys = new SortedList<string, string>();
            foreach (VColumn col in columns)
            {
                VQueryCall updQry = null;
                XElement xcol = this.CreateColumnInfo(col, queryCall, ref updQry, jColsNames);
                var srcCol = col.SourceColumn().FirstOrDefault();
                xcolumns.Add(xcol);
                string srcName;
                if (updQry == null) {
                    srcName = mainQname;
                } else {
                    srcName = updQry.XName;
                    if (!updQueries.ContainsKey(srcName)) {
                        usedUpdColumns.Add(srcName, new List<string>());
                        updatebleColumns.Add(srcName, new List<VColumn>());
                        updatebleColumnsExt.Add(srcName, new List<VColumn>());
                        xupdatebleColumnsExt.Add(srcName, new List<XElement>());
                        otherUpdateableColumns.Add(srcName, new List<XElement>());
                        updQueries.Add(srcName, updQry);
                        if (mainQname != srcName) {
                            string keyName = updQry.Query().KeyColumn().XName;
                            xtraKeys.Add(srcName, keyName);
                        }
                    }
                    xcol.SetAttributeValue(_AName.source_table, updQry.Query().SourceTable().P_Name);
                }
                if (xcol.AttrOrDefault(_AName.is_updateable, false))
                {
                    updatebleColumns[srcName].Add(col);
                }
                if (xcol.AttrOrDefault(_AName.is_updateable_ext, false))
                {
                    updatebleColumnsExt[srcName].Add(col);
                    xupdatebleColumnsExt[srcName].Add(xcol);
                    string un = srcCol.P_Column;
                    if (un == string.Empty)
                    {
                        un = srcCol.XName;
                    }
                    usedUpdColumns[srcName].Add(un);
                    //usedUpdColumns[srcName].Add(srcCol.XName);
                }
                ///////////
                //if (srcName != mainQname)
                //{
                //    if (Cmn.GetAttrValue(xcol, TextConst.DsAName.IsUpdateable) == TextConst.AVBool.True)
                //    {
                //        updatebleColumns[mainQname].Add(col as VColumn);
                //    }
                //    if (Cmn.GetAttrValue(xcol, TextConst.DsAName.IsUpdateableExt) == TextConst.AVBool.True)
                //    {
                //        updatebleColumnsExt[mainQname].Add(col as VColumn);
                //        xupdatebleColumnsExt[mainQname].Add(xcol);
                //        usedUpdColumns[mainQname].Add(srcCol.XName);
                //    }
                //}
                ///////////
                if (xcol.Attribute(_AName.is_updateable_ext) != null)
                {
                    // var srcCol = (col as VColumn).SourceColumn().First();
                    string tempName = srcCol.GetColumnTempName();
                    xcol.SetAttributeValue(_AName.update_target, srcCol.XName);
                    string tpr = Compiler.getTyprPr(xcol.Attribute(_AName.type).Value);
                    //if (!typesInd.ContainsKey(tpr))
                    //{
                    //    typesInd.Add(tpr, 0);
                    //}
                    //typesInd[tpr]++;
                    xcol.SetAttributeValue(_AName.temp_col_name, tempName);
                }
            }
            var xtraKeysCol = new SortedList<string, VColumn>();
            foreach (KeyValuePair<string, VQueryCall> qc in updQueries)
            {
                var mQry = qc.Value.Query();
                if (qc.Key != mainQname)
                {
                    string xKeyName = xtraKeys[qc.Key];
                    string keyAlias = qc.Value.UsedColumns().First(c => c.Parent.Name == EName.select && c.P_Column == xKeyName).XName;
                    VColumn xkeyCol = queryCall.UsedColumns().First(c => c.P_Column == keyAlias);
                    xtraKeysCol.Add(qc.Key, xkeyCol);
                    xtbl.Add(new XElement(EName.extra_key, 
                                 new XAttribute(_AName.table, qc.Value.XName),
                                 new XAttribute(_AName.column, xkeyCol.XName)));
                    // если ошибка, наверное нужно вывести ключ дочерней редактируемой таблицы
                }
                foreach (VSXElement col in mQry.NativeColumns())
                {
                    if (!usedUpdColumns[qc.Key].Contains(col.P_Column))
                    {
                        otherUpdateableColumns[qc.Key].Add(col);
                    }
                }
            }
            var updText = new StringBuilder();
            bool hasUpd = false;
            updText.AppendLine("begin");
            xtbl.SetAttributeValue(_AName.is_ms_upd, TextConst.AVBool.False);
            foreach (KeyValuePair<string, VQueryCall> qc in updQueries)
            {
                if (qc.Key != mainQname)
                {
                    var joinInfo = new Dictionary<string, string>();
                    string commandText = VForm.getMergeText(qc.Value, updatebleColumns[qc.Key], ref joinInfo).Replace('\r', ' ');
                    updText.Append(commandText);
                    updText.AppendLine(";");
                    VColumn xkeyCol = xtraKeysCol[qc.Key];
                    commandText = VForm.getUpdateTempText(qc.Value, xupdatebleColumnsExt[qc.Key], otherUpdateableColumns[qc.Key], keyColumn, xkeyCol, mainQname, joinInfo).Replace('\r', ' ');
                    WriteAttrAsElem(xtbl, EName.update_temp_text, commandText);
                    commandText = VForm.getClearTempText(qc.Value, mainQname);
                    WriteAttrAsElem(xtbl, EName.clear_temp_text, commandText);
                    hasUpd=true;
                    //var xExtUpdTbl = new XElement(TextConst.EName.Table);
                    //WriteAttrAsElem(xExtUpdTbl, TextConst.DsEName.UpdateText, commandText);
                    //xtbl.Add(xExtUpdTbl);
                }
                else if (keyColumn != null)
                {
                    if (xupdatebleColumnsExt[qc.Key].Count > 0) {
                        string commandText = VForm.getUpdateText(qc.Value, updatebleColumns[qc.Key], keyColumn).Replace('\r', ' ');
                        updText.Append(commandText);
                        updText.AppendLine(";");
                        xtbl.SetAttributeValue(TextConst.DsAName.IsMainSourceUpdateable, TextConst.AVBool.True);
                        hasUpd = true;
                        commandText = VForm.getInsertText(qc.Value, updatebleColumns[qc.Key], keyColumn).Replace('\r', ' ');
                        WriteAttrAsElem(xtbl, EName.insert_text, commandText);
                        commandText = VForm.getDeleteText(qc.Value, keyColumn);
                        WriteAttrAsElem(xtbl, EName.delete_text, commandText);
                        commandText = VForm.getUpdateTempText(qc.Value, xupdatebleColumnsExt[qc.Key], otherUpdateableColumns[qc.Key], keyColumn).Replace('\r', ' ');
                        WriteAttrAsElem(xtbl, EName.update_temp_text, commandText);
                        commandText = VForm.getClearTempText(qc.Value);
                        WriteAttrAsElem(xtbl, EName.clear_temp_text, commandText);
                    }
                }
            }
            updText.AppendLine("end;");
            if (hasUpd)
            {
                WriteAttrAsElem(xtbl, EName.update_text, updText.ToString());
            }
            updText.Clear();
            List<VSXElement> eventsSections = srcQuery.GetNamedSections(TextConst.EName.Events);
            XElement xevents = queryCall.GetElementsP(EName.events).FirstOrDefault();
            if (xevents != null) {
                xevents = new XElement(xevents);
                xtbl.Add(xevents);
            } else if (eventsSections.Count > 0) {
                xevents = new XElement(EName.events);
                xtbl.Add(xevents);
            }
            foreach (VSXElement eventsSection in eventsSections)
            {
                foreach (VSXElement evnt in eventsSection.GetElementsP())
                {
                    xevents.Add(evnt);
                }
            }
            return xtbl;
        }
        private XElement CreateColumnInfo(VColumn col, VQueryCall queryCall, ref VQueryCall updQuery, List<string> jColsNames)
        {
            var xcol = new XElement(EName.column);
            xcol.Add(new XAttribute(_AName.name, col.XName));
            xcol.Add(new XAttribute(_AName.table, col.P_Table));
            xcol.Add(new XAttribute(_AName.type, col.XDataType()));
            xcol.Add(new XAttribute(_AName.title, col.P_Title));
            if (!string.IsNullOrEmpty(col.P_ParName)) {
                xcol.Add(new XAttribute(_AName.parname, col.P_ParName));
            }
            if (jColsNames.Contains(col.XName))
            {
                xcol.Add(new XAttribute(_AName.is_join_col, TextConst.AVBool.True));
            }
            if (col.IsAddisionForName)
            {
                //string name = col.XName;
                //string kodName = name.Substring(0, name.Length - TextConst.Pfx.ExtValName.Length);
                string kodName = col.TextSourceFor.ToString();
                xcol.SetAttributeValue(_AName.text_source_for, kodName);
            }
            //bool isEditableLinkCol = false;
            //if ((col as VColumn).Source().P_ColumnEditable != "" && (col as VColumn).SourceColumn().Any())
            //{
            //    isEditableLinkCol = true;
            //}
            bool isMain = (col.P_Table == queryCall.XName);
            WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.Exists, isMain);
            if (!IsPropFalse(xcol, TextConst.DsAName.Exists))
            {
                WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.Visible, isMain);
                if (!IsPropFalse(xcol, TextConst.DsAName.Visible))
                {
                    WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.Editable, isMain);
                    if ((col.P_Table == queryCall.XName && !IsPropFalse(xcol, TextConst.DsAName.Editable)) || IsPropTrue(xcol, TextConst.DsAName.Editable) /*|| isEditableLinkCol*/)
                    {
                        xcol.SetAttributeValue(_AName.is_user_editable, TextConst.AVBool.True);
                        this.WriteColumnListInfo(xcol, col);
                    }
                }
                WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.Default, isMain);
                WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.Mandatory, isMain);
                WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.Valid, isMain);
                WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.NewVal, isMain);
                WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.FontColor, isMain);
                WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.BackColor, isMain);
                // WriteColumnBehaviorInfo(col, xcol, TextConst.DsAName.TextSource, isMain);
                bool isUpdateable = false;
                VSXElement srcCol = null;
                bool isSys = false;
                if (col.P_Table == queryCall.XName /*|| isEditableLinkCol*/)
                {
                    if (!(col is VFact))
                    {
                        srcCol = col.SourceColumn().First();
                        if (srcCol.Attribute(_AName.sys) != null)
                        {
                            isSys = true;
                        }
                        if (srcCol is VColumn)
                        {
                            //if (srcCol.P_Table == TextConst.AVTable.Ths)
                            //{
                            //    var ssrcCol = (srcCol as VColumn).SourceColumn().First();
                            //}
                            VQueryCall src = (srcCol as VColumn).Source();
                            if (src != null)
                            {
                                if (src is VTable)
                                {
                                    isUpdateable = true;
                                }
                                else if (src.P_Updateable == TextConst.AVBool.True)
                                {
                                    isUpdateable = true;
                                    updQuery = src;
                                }
                            }
                            //else if (srcCol.P_Table == TextConst.AVTable.Ths)
                            //{
                            //    var ssrcCol = (srcCol as VColumn).SourceColumn() .First();
                            //    if (ssrcCol is VColumn)
                            //    {
                            //        if ((ssrcCol as VColumn).Source() is VTable)
                            //        {
                            //            isUpdateable = true;
                            //        }
                            //    }
                            //}
                        }
                    }
                }
                if (!isSys) 
                {
                    if (isUpdateable)
                    {
                        xcol.SetAttributeValue(_AName.is_updateable, TextConst.AVBool.True);
                        xcol.SetAttributeValue(_AName.is_updateable_ext, TextConst.AVBool.True);
                    }
                    else if (IsPropFalseNot_Cl(xcol, TextConst.DsAName.Editable))
                    {
                        xcol.SetAttributeValue(_AName.is_updateable_ext, TextConst.AVBool.True);
                    }
                }
                if (xcol.Attribute(_AName.is_updateable) == null && !isSys && (srcCol == null || srcCol.P_Key != TextConst.AVBool.True))
                {
                    xcol.SetAttributeValue(_AName.is_refreshed, TextConst.AVBool.True);
                }
                else if (col.IsRefreshedByAction())
                {
                    xcol.SetAttributeValue(_AName.is_refreshed, TextConst.AVBool.True);
                }
                if (xcol.Attribute(_AName.is_updateable_ext) != null)
                {
                    xcol.SetAttributeValue(_AName.update_target, col.SourceColumn().First().XName);
                }
            }
            return xcol;
        }
        internal static void WriteAttrAsElem(XElement element, string attrName, string value)
        {
            WriteAttrAsElem(element, (XName)attrName, value);
        }
        internal static void WriteAttrAsElem(XElement element, XName name, string value)
        {
            element.Elements(name).Remove();
            XElement el = new XElement(name);
            element.Add(el);
            el.Add(new XText(value));
        }
        private static void WriteAttrAsElem(XElement element, XName name, XElement value)
        {
            element.Elements(name).Remove();
            XElement el = new XElement(name);
            element.Add(el);
            el.Add(value);
        }
        private void WriteColumnListInfo(XElement xcol, VColumn col)
        {
            VSXElement src = col.SourceColumns().FirstOrDefault();
            //var tCol = (VColumn)col;
            XElement slQueryOrig = null;
            VWithParams clientFactParsElement = null;
            VQueryCall ql = col.ListQueryCallElement();
            if (ql != null)
            {
                slQueryOrig = col.TypeQueryAsListQuery();
            }
            if (slQueryOrig != null)
            {
                clientFactParsElement = (ql.GetElementsP(EName.withparams).FirstOrDefault() as VWithParams);
                if (clientFactParsElement != null)
                {
                    clientFactParsElement = (VSXElement.Get(new XElement(clientFactParsElement)) as VWithParams);
                    IEnumerable<XElement> formalPars = null;
                    int i = 0;
                    foreach (VSXElement factPar in clientFactParsElement.GetElementsP())
                    {
                        if (VSXElement.HasParameterName(factPar)) {
                            break;
                        }
                        if (formalPars == null)
                        {
                            formalPars = slQueryOrig.Elements(EName.@params).Elements().ToArray();
                        }
                        XElement formalPar = formalPars.ElementAt(i);
                        factPar.P_ParName = formalPar.Attribute(_AName.name).Value;
                        i++;
                    }
                }
            }
            else if (src is VColumn)
            {
                slQueryOrig = (src as VColumn).TypeQueryAsListQuery();
            }
            if (slQueryOrig != null)
            {
                slQueryOrig = new XElement(slQueryOrig);
                slQueryOrig = Compiler.PreCompileQuery(slQueryOrig, true);
                XElement slQuery = slQueryOrig;
                string rl = col.SelectionListRowsLimit();
                if (rl != "0" && rl != "")
                {
                    slQuery = VQuery.CreateFilteredQuery(slQuery, int.Parse(rl));
                }
                //  var slQuery1 = new XElement(slQuery);
                VReport repPre = XmlReports.Environment.GetPrecompiledReport(slQuery);
                XElement rep1 = new XElement(repPre);
                XElement compiled = repPre.Compile(2, false, null);
                WriteAttrAsElem(xcol, EName.sel_list_compiled, compiled);
                WriteAttrAsElem(xcol, EName.sel_list_report, rep1);
                //var rep = this.GetEnvironment().GetPrecompiledReport(rep1);
                // rep.Add(new XElement(TextConst.EName.Compiled, compiled));
                //dataCol.SelectionList = rep.Result(2, false);
                if (ql == null)
                {
                    XElement pars = new XElement(EName.@params);
                    pars.Add(slQueryOrig.Elements(EName.@params).Elements());
                    WriteAttrAsElem(xcol, EName.sel_list_pars, pars);
                    //foreach (XElement par in slQueryOrig.Elements(TextConst.EName.Params).Elements())
                    //{
                    //    //var ssrccol = Cmn.GetAttrValue(par, TextConst.AName.Column);
                    //    //(dataTable.Columns[ssrccol] as VDataColumn).AddDependantSelList(col.XName);
                    //    //dataCol.SelectionList.InputParams[Cmn.GetAttrValue(par, TextConst.AName.Name)].SourceColumn = ssrccol;
                    //}
                } 
                else 
                {
                    WriteAttrAsElem(xcol, EName.sel_list_cl_fact_pars, clientFactParsElement);
                    // dataCol.SelectionList.FactParamsElement = clientFactParsElement;
                }
                XAttribute treeParentFieldNameAttr = slQueryOrig.Attribute(_AName.parent_field_name);
                if (treeParentFieldNameAttr != null)
                {
                    xcol.SetAttributeValue(_AName.sel_list_parent_field_name, treeParentFieldNameAttr.Value);
                    //(dataCol.SelectionList.Tables[0] as VDataTable).TreeParentFieldName = treeParentFieldNameAttr.Value;
                }
            }
        }
        private static bool IsPropFalseNot_Cl(XElement xcol, string propName)
        {
            if (IsPropFalseNot(xcol, propName)) {
                return true;
            }
            string v = xcol.AttrOrDefault(TextConst.Pfx.BehaviorClient + propName, string.Empty);
            return v != TextConst.AVBool.False && v != string.Empty;
        }
        private static bool IsPropFalseNot(XElement xcol, string propName)
        {
            string v = xcol.AttrOrDefault(TextConst.Pfx.BehaviorPropCol + propName, string.Empty);
            return v != TextConst.AVBool.False && v != string.Empty;
        }
        private static bool IsPropFalse(XElement xcol, string propName)
        {
            return xcol.AttrOrDefault(TextConst.Pfx.BehaviorPropCol + propName, null) == TextConst.AVBool.False;
        }
        private static bool IsPropTrue(XElement xcol, string propName)
        {
            return xcol.AttrOrDefault(TextConst.Pfx.BehaviorPropCol + propName, null) == TextConst.AVBool.True;
        }
        private static string GetClientTextSourceName(VSXElement col)
        {
            string s = (string)VFieldInfo.GetValue(col, VSXElement.PropPfx + TextConst.DsAName.TextSource + TextConst.Pfx.BehaviorPropRes);
            string[] ss = s.Split(':');
            if (ss.Length == 2)
            {
                if (ss[0] == TextConst.EName.Param)
                {
                    return ss[1];
                }
            }
            return null;
        }
        private static string GetSourceTextSourceName(VSXElement col)
        {
            string s = (string)VFieldInfo.GetValue(col, VSXElement.PropPfx + TextConst.DsAName.TextSource + TextConst.Pfx.BehaviorPropRes);
            string[] ss = s.Split(':');
            if (ss.Length == 2)
            {
                if (ss[0] == TextConst.EName.Column)
                {
                    return ss[1];
                }
            }
            return null;
        }
        private static void WriteColumnBehaviorInfo(VColumn col, XElement xcol, string propName, bool isMain)
        {
            if (!col.IsAddision) {
                WriteColumnBehaviorInfo((VSXElement)col, xcol, propName, isMain);
            }
        }
        private static void WriteColumnBehaviorInfo (VSXElement col, XElement xcol, string propName, bool isMain)
        {
            string prop_name = VSXElement.PropPfx + propName + TextConst.Pfx.BehaviorPropRes;
            if (!VFieldInfo.Exists(col, prop_name)) return;
            string s = (string)VFieldInfo.GetValue(col, prop_name);
            if (string.IsNullOrEmpty(s)) return;
            if (s[0] == '!')
            {
                xcol.SetAttributeValue(propName  + TextConst.Pfx.BehaviorPropInv, TextConst.AVBool.True);
                s = s.Substring(1);
            }
            string[] ss = s.Split(':');
            if (ss.Length == 2)
            {
                string stype = ss[0];
                string sval = ss[1];
                if ((!isMain) && stype == TextConst.EName.Column)
                {
                    stype = string.Empty;
                }
                switch (stype)
                {
                    case TextConst.EName.Query:
                        xcol.SetAttributeValue(TextConst.Pfx.BehaviorPropCol + propName, sval);
                        break;
                    case TextConst.EName.Column:
                        xcol.SetAttributeValue(propName, sval);
                        break;
                    case TextConst.EName.Param:
                        xcol.SetAttributeValue(TextConst.Pfx.BehaviorClient + propName, sval);
                        break;
                }
            }
        }
        /// <summary>
        /// Добавляет в <paramref name="parent"/> тэг &lt;field&gt;
        /// с атрибутами name (<paramref name="field_name"/>), type (<paramref name="data_type"/>) и
        /// parname (<paramref name="field_name"/>)
        /// </summary>
        /// <param name="parent">родительский тэг</param>
        /// <param name="field_name">значение атрибутов name и parname</param>
        /// <param name="data_type">значение атрибута type, используйте значения из <see cref="TextConst.AVDataType"/></param>
        /// <returns>добавленный тег</returns>
        private static XElement AddNewField(XElement parent, string field_name, string data_type)
        {
            Contract.Assert(parent != null);
            Contract.Assert(!string.IsNullOrEmpty(field_name));
            Contract.Assert(!string.IsNullOrEmpty(data_type));
            XElement xfield = new XElement(EName.field);
            xfield.Add(new XAttribute(_AName.type, data_type));
            xfield.Add(new XAttribute(_AName.name,     field_name));
            xfield.Add(new XAttribute(_AName.parname,  field_name));
            parent.Add(xfield);
            return xfield;
        }
        /// <summary>
        /// Добавляет в <paramref name="parent"/> тэг &lt;param&gt; 
        /// с атрибутами name (<paramref name="param_name"/>) и type (<paramref name="data_type"/>)
        /// </summary>
        /// <param name="parent">родительский тэг</param>
        /// <param name="param_name">значение атрибута name</param>
        /// <param name="data_type">значение атрибута type, используйте значения из <see cref="TextConst.AVDataType"/></param>
        /// <returns>добавленный тег</returns>
        private static XElement AddNewParam(XElement parent, string param_name, string data_type)
        {
            Contract.Assert(parent != null);
            Contract.Assert(!string.IsNullOrEmpty(param_name));
            Contract.Assert(!string.IsNullOrEmpty(data_type));
            XElement xpar = new XElement(EName.param);
            xpar.Add(new XAttribute(_AName.name,     param_name));
            xpar.Add(new XAttribute(_AName.type, data_type));
            parent.Add(xpar);
            return xpar;
        }
    }
}
