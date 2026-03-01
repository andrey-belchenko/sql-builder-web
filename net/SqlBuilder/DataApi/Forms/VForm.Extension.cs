using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using System.Windows.Forms;
using System.Reflection;
using System.Text;
using System.Xml.Linq;
using sql.builder.Clean;
//using DevExpress.XtraVerticalGrid;
//using infoenergo.core.Extensions;
using _AName = sql.builder.DataApi.AName;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.DataApi
{
    public partial class VForm
    {
        public bool WithData
        {
            get
            {
                return this.Element(EName.from) != null;
            }
        }
        public static VDataSet CreateDataSetPre(XElement xds)
        {
            VDataSet dataSet = new VDataSet();
            dataSet.Scheme = VSXElement.Get(new XElement(EName.scheme));
            var xtables = new SortedList<string, XElement>();
            foreach (XElement xtbl in xds.Elements(EName.table))
            {
                VDataTable dataTable = VForm.createDataTable(xtbl, dataSet);
                if (xtbl.AttrOrDefault(_AName.is_top, false))
                {
                    dataSet.AddTopTable(dataTable);
                }
                xtables.Add(xtbl.Attribute(_AName.name).Value, xtbl);
            }
            VReport.ApplySimpleParams(null, dataSet, xds.Element(EName.@params));
            AddParamTableToDataSet(xds, dataSet);
            foreach (var xtbl1 in xtables.Values)
            {
                VDataTable dataTable = dataSet.Tables[xtbl1.Attribute(_AName.name).Value] as VDataTable;
                createDataTable_SetBehavior(xtbl1, dataSet, dataTable);
                setUpdateable(dataTable, xtbl1);
            }
            //AddParamTableToDataSet(xds, dataSet);
            return dataSet;
        }
        public VForm GetProcessed()
        {
            return this;
        }
        // Емцов - попытка делать это асинхронно
        private static object lock_obj = new object();
        public static Tuple<XElement, VDataSet, XElement> GetFormXelementAndDataSet(string name)
        {
            lock (lock_obj)
            {
                var xroot = VForm.CreateDataSetAndFormInfo(name);
                var xds = xroot.Element(EName.dataset);
                VDataSet dataSet = getDataSet(xds);
                var xform = xroot.Element(EName.form);
                var xparams = xroot.Element(EName.@params);
                return new Tuple<XElement, VDataSet, XElement>(xform, dataSet, xparams);
            }
        }
        private static VDataSet getDataSet(XElement xds)
        {
            VDataSet dataSet = VForm.CreateDataSetPre(xds);
            foreach (XElement xtbl1 in xds.Elements(EName.table).ToArray())
            {
                VDataTable dataTable = dataSet.Tables[xtbl1.Attribute(_AName.name).Value] as VDataTable;
                VForm.createDataAtapter(dataTable, xtbl1);
            }
            return dataSet;
        }
        private static void AddParamTableToDataSet(XElement xds, VDataSet dataSet)
        {
            VDataTable tbl = new VDataTable(true);
            dataSet.Tables.Add(tbl);
            dataSet.ParamsTable = tbl;
            var fldList = new List<Tuple<XElement, VDataColumn>>();
            foreach (XElement xfld in xds.Elements(EName.fields).Elements())
            {
                Type valType = Cmn.GetTypeFromStringType(xfld.Attribute(_AName.type).Value, null);
                VDataColumn column = tbl.AddColumn(xfld.Attribute(_AName.name).Value, valType, xfld.AttrOrDefault(_AName.title, string.Empty));
                //setColumnProperties(xfld, column);
                fldList.Add(new Tuple<XElement, VDataColumn>(xfld, column));
                XAttribute attr = xfld.Attribute(_AName.parname);
                if (attr != null)
                {
                    dataSet.AddVariableColumn(attr.Value, column);
                }
                //if (xfld.Attribute(TextConst.AName.ColumnEditable)!=null)
                //{
                //    column.ColumnEditableSource = xfld.Attribute(TextConst.AName.ColumnEditable).Value;
                //}
            }
            foreach (var f in fldList)
            {
                VForm.SetColumnProperties(f.Item1, f.Item2);
            }
            dataSet.ParamsTable.Rows.Add();
            dataSet.ParamsTable.CurrentRow = dataSet.ParamsTable.Rows[0];
        }
        private static void dataTableRowChanged(object sender, DataRowChangeEventArgs e)
        {
            var tbl = (e.Row.Table as VDataTable);
            //if (tbl.SuppressChangedEvent) return;
            //if (!tbl.PrimaryKey.Any()) return;
            if (e.Row.RowState == DataRowState.Added)
            {
                tbl.ProcessNewRow(e.Row);
            }
        }
        private static void createDataTable_SetBehavior(XElement xtbl, VDataSet dataSet, VDataTable dataTable)
        {
            var xcols = xtbl.Element(EName.columns).Elements();
            foreach (XElement xcol in xcols)
            {
                string name = xcol.Attribute(TextConst.AName.Name).Value;
                VDataColumn dataCol = (dataTable.Columns[name] as VDataColumn);
                XAttribute attr = xcol.Attribute(_AName.text_source_for);
                if (attr != null)
                {
                    string kodName = attr.Value;
                    (dataTable.Columns[kodName] as VDataColumn).TextSourceSource = name;
                    dataCol.AddDependantText((dataTable.Columns[kodName] as VDataColumn));
                }
                if (xcol.AttrOrDefault(_AName.is_user_editable, false))
                {
                    XElement slRep = ReadAttrAsElem(xcol, EName.sel_list_report);
                    if (slRep != null)
                    {
                        VReport rep = XmlReports.Environment.GetPrecompiledReport(slRep);
                        XElement compiled = ReadAttrAsElem(xcol, EName.sel_list_compiled);
                        rep.Add(new XElement(EName.compiled, compiled));
                        dataCol.SelectionList = rep.Result(2, false);
                        XElement pars = ReadAttrAsElem(xcol, EName.sel_list_pars);
                        if (pars != null)
                        {
                            foreach (XElement par in pars.Elements())
                            {
                                string ssrccol = par.AttrOrDefault(_AName.column, string.Empty);
                                //(dataTable.Columns[ssrccol] as VDataColumn).AddDependantSelList(dataCol.ColumnName);
                                if (ssrccol != string.Empty)// вылетала ошибка
                                {
                                    (dataTable.Columns[ssrccol] as VDataColumn).AddDependantSelList(dataCol);
                                    dataCol.SelectionList.InputParams[par.AttrOrDefault(_AName.name, string.Empty)].SourceColumn = ssrccol;
                                }
                            }
                        }
                        else
                        {
                            pars = ReadAttrAsElem(xcol, EName.sel_list_cl_fact_pars);
                            if (pars == null)
                            {
                                pars = new XElement(EName.withparams);
                            }
                            dataCol.SelectionList.FactParamsElement = VSXElement.Get<VWithParams>(new XElement(pars));
                            foreach (XElement par in pars.Elements())
                            {
                                string parName = par.AttrOrDefault(_AName.name, string.Empty);
                                //(dataTable.Columns[ssrccol] as VDataColumn).AddDependantSelList(dataCol.ColumnName);
                                VDataColumn srccol = dataSet.GetVariableColumn(parName);
                                if (srccol != null)
                                {
                                    srccol.AddDependantSelList(dataCol);
                                }
                                // dataCol.SelectionList.InputParams[Cmn.GetAttrValue(par, TextConst.AName.Name)].SourceColumn = ssrccol;
                            }
                        }
                        XAttribute treeParentFieldNameAttr = xcol.Attribute(_AName.sel_list_parent_field_name);
                        if (treeParentFieldNameAttr != null)
                        {
                            (dataCol.SelectionList.Tables[0] as VDataTable).TreeParentFieldName = treeParentFieldNameAttr.Value;
                        }
                    }
                }
                //setColumnProperty(xcol, dataCol, TextConst.DsAName.Visible);
                SetColumnProperties(xcol, dataCol);
            }
        }
        private static VDataTable createDataTable(XElement xtbl, VDataSet dataSet, VColumn returnIntoColumn = null, bool isRet = false)
        {
            //var columns = getColumns(queryCall);
            //if (columns.Count() == 0)
            //{
            //    return null;
            //}
            //var xtbl = CreateTableInfo(queryCall);
            //var srcQuery = queryCall.Query();
            var xcols = xtbl.Element(EName.columns).Elements();
            if (!xcols.Any())
            {
                return null;
            }
            VDataTable dataTable = new VDataTable();
            //if (queryCall.P_NewRowsVisForOtherTbls == "1")
            //{
            //    dataTable.NewRowsVisForOtherTbls = true;
            //}
            dataTable.UseDeferredFetch = true;
            string specName = xtbl.AttrOrDefault(_AName.ref_column, string.Empty);
            if (specName != string.Empty)
            {
                dataTable.ArrayEditValueRefColumn = specName;
                dataTable.UseDeferredFetch = false;
            }
            specName = xtbl.AttrOrDefault(_AName.multi_select_column, string.Empty);
            if (specName != string.Empty)
            {
                dataTable.MultiselectColumnName = specName;
            }
            specName = xtbl.AttrOrDefault(_AName.multi_select_target, string.Empty);
            if (specName != string.Empty)
            {
                dataTable.MultiselectTargetName = specName;
                dataTable.UseDeferredFetch = false;
            }
            dataTable.SetEvents(xtbl.Element(EName.events));
            if (xtbl.AttrOrDefault(_AName.new_rows_vis_for_other_tbls, false))
            {
                dataTable.NewRowsVisForOtherTbls = true;
            }
            //dataTable.TableName = queryCall.XName;
            dataTable.TableName = xtbl.Attribute(_AName.name).Value;
            dataTable.AutoRefresh = xtbl.AttrOrDefault(_AName.auto_refresh, false);
            dataTable.AsyncLoad = xtbl.AttrOrDefault(_AName.async, false);
            //if (dataTable.AutoRefresh)
            //{
            //    dataTable.AsyncLoad = true;
            //}
            dataTable.OnlyVisibleRefresh = xtbl.AttrOrDefault(_AName.only_visible_refresh, false);
            dataTable.OnlyForceRefresh = xtbl.AttrOrDefault(_AName.only_force_refresh, false);
            //dataTable.UpdateableTableName = srcQuery.P_IdName;
            if (xtbl.Attribute(_AName.non_db) != null)
            {
                dataTable.IsNonDb = true;
            }
            XAttribute attr = xtbl.Attribute(_AName.key_dimension);
            if (attr != null)
            {
                dataTable.KeyDimension = attr.Value;
            }
            attr = xtbl.Attribute(_AName.update_target);
            if (attr != null)
            {
                dataTable.UpdateableTableName = attr.Value;
                dataTable.MyRowChanged += VForm.dataTableRowChanged;
                dataTable.MyColumnChanged += VDataColumn.DataTableColumnChanged;
            }
            //if (queryCall.P_ColumnEditable != "")
            //{
            //    dataTable.ColumnEditableSource = queryCall.P_ColumnEditable;
            //}
            //if (srcQuery.P_DeleteValidation != "")
            //{
            //    dataTable.DeleteValidationSource = srcQuery.P_DeleteValidation.Split('.').Last();
            //}
            attr = xtbl.Attribute(_AName.column_editable);
            if (attr != null)
            {
                dataTable.ColumnEditableSource = attr.Value;
            }
            attr = xtbl.Attribute(_AName.color);
            if (attr != null)
            {
                dataTable.ClientBackColorSource = attr.Value;
            }
            attr = xtbl.Attribute(_AName.can_be_checked);
            if (attr != null)
            {
                dataTable.ClientCanBeCheckedSource = attr.Value;
            }
            attr = xtbl.Attribute(_AName.delete_validation);
            if (attr != null)
            {
                dataTable.DeleteValidationSource = attr.Value;
            }
            dataSet.Tables.Add(dataTable);
            foreach (XElement xcol in xcols)
            {
                Type type = Cmn.GetTypeFromStringType(xcol.AttrOrDefault(_AName.type, string.Empty), null);
                dataTable.AddColumn(xcol.Attribute(_AName.name).Value, type, xcol.AttrOrDefault(_AName.title, string.Empty));
            }
            foreach (XElement xcol in xcols)
            {
                VDataColumn dataCol = (dataTable.Columns[xcol.Attribute(_AName.name).Value] as VDataColumn);
                attr = xcol.Attribute(_AName.parname);
                if (attr != null)
                {
                    dataSet.AddVariableColumn(attr.Value, dataCol);
                }
            }
            attr = xtbl.Attribute(_AName.key);
            if (attr != null)
            {
                dataTable.PrimaryKey = new DataColumn[1] { dataTable.Columns[attr.Value] };
                dataTable.PrimaryKey[0].AllowDBNull = true;
            }
            //dataTable.SchemeNative = createTableScheme(queryCall, columns, isRet);
            dataTable.Scheme = ReadAttrAsElem(xtbl, EName.scheme);
            attr = xtbl.Attribute(_AName.parent_table);
            if (attr != null)
            {
                string parentTableName = attr.Value;
                VDataTable parentDataTable = (VDataTable)dataSet.Tables[parentTableName];
                DataColumn dparentCol = parentDataTable.PrimaryKey.First();
                DataColumn dchildCol = dataTable.Columns[xtbl.Attribute(_AName.parent_key).Value];
                dataTable.ParentRelations.Add(new DataRelation(null, dparentCol, dchildCol));
                dataTable.SetDependantRefresh(true);
                XElement xchilds = parentDataTable.Scheme.Element(EName.childs);
                if (xchilds == null)
                {
                    xchilds = new XElement(EName.childs);
                    parentDataTable.Scheme.Add(xchilds);
                }
                xchilds.Add(dataTable.Scheme);
            }
            else
            {
                dataSet.Scheme.Add(dataTable.Scheme);
            }
            //createDataAtapter(dataTable, queryCall,returnIntoColumn,columns);
            return dataTable;
        }
        public static void SetColumnProperties(XElement xcol, VDataColumn column)
        {
            foreach (string name in TextConst.DsANameArray.AllBehProps)
            {
                setColumnProperty(xcol, column, name);
            }
        }
        private static void setColumnProperty(XElement xcol, VDataColumn dataCol, string propName)
        {
            var attr = xcol.Attribute(propName + TextConst.Pfx.BehaviorPropInv);
            if (attr != null)
            {
                Cmn.SetProperty(dataCol, propName + TextConst.Pfx.BehaviorPropInv, true);
                // dataCol.VisibleInvert = attr.Value;
            }
            attr = xcol.Attribute(TextConst.Pfx.BehaviorPropCol + propName);
            if (attr != null)
            {
                Cmn.SetProperty(dataCol, TextConst.Pfx.BehaviorPropCol + propName + TextConst.Pfx.BehaviorSource, attr.Value);
                // dataCol.ColumnVisibleSource = attr.Value;
            }
            else
            {
                attr = xcol.Attribute(TextConst.Pfx.BehaviorClient + propName);
                if (attr != null)
                {
                    if (dataCol.GetTable().GetDataSet().InputParams == null
                        || !dataCol.GetTable().GetDataSet().InputParams.ContainsKey(attr.Value))
                    {
                        var masterCol = dataCol.GetTable().GetDataSet().GetVariableColumn(attr.Value);
                        if (masterCol != null)
                        {
                            masterCol.AddDependantProp(dataCol, propName);
                        }
                    }
                    Cmn.SetProperty(dataCol, TextConst.Pfx.BehaviorClient + propName + TextConst.Pfx.BehaviorSource, attr.Value);
                    //dataCol.VisibleSource = attr.Value;
                }
                else
                {
                    attr = xcol.Attribute(propName);
                    if (attr != null)
                    {
                        string val = attr.Value.SubstringAfter('.');
                        VDataColumn masterCol = dataCol.GetTable().GetColumn(val);
                        masterCol.AddDependantProp(dataCol, propName);
                        Cmn.SetProperty(dataCol, propName + TextConst.Pfx.BehaviorSource, val);
                        //dataCol.VisibleSource = attr.Value;
                    }
                }
            }
            if (propName == TextConst.DsAName.Mandatory)
            {
                dataCol.AddDependantProp(dataCol, propName);
            }
        }
        private static List<XElement> GetCoreTableListFromCompiledQuery(XElement query, string tableName, bool isEditorMain, string keyDimension)
        {
            List<XElement> list = null;
            //!!! Скорее всего условия кривые /избыточные
            if (isEditorMain)
            {
                list = query.Descendants(EName.table)
                      .Where(t => t.Attribute(_AName.name).Value == tableName && (
                          t.AttrOrDefault(_AName.dimension, string.Empty) == keyDimension || keyDimension == null)).ToList();
            }
            else
            {
                list = query.Descendants(EName.table)
                     .Where(t => t.Attribute(_AName.is_from_temp) != null && t.Attribute(_AName.name).Value == tableName &&
                         (t.AttrOrDefault(_AName.dimension, string.Empty) != keyDimension || keyDimension == null)).ToList();
            }
            return list;
            //return query.Descendants(TextConst.EName.Table)
            //      .Where(t => t.Attribute(TextConst.AName.Name).Value == tableName).ToList();   // !!!Временный вариант. Обработать ситуацию, когда есть обращение к той же таблице но с другим смыслом, например по kod_parent. 
        }
        private List<VColumn> GetRefreshedColumns(VQueryCall queryCall, List<VColumn> columns)
        {
            string method_name = MethodBase.GetCurrentMethod().ToString();
            string cashName = queryCall.XName;
            if (IsCashValueExists(method_name, cashName))
            {
                return (GetCashValue(method_name, cashName) as List<VColumn>);
            }
            var allColumns = columns;
            var refreshedColumns = new List<VColumn>();
            foreach (VColumn col in allColumns)
            {
                bool isUpdateable = false;
                VSXElement srcCol = null;
                bool isSys = false;
                if (col.P_Table == queryCall.XName)
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
                            if ((srcCol as VColumn).Source() is VTable)
                            {
                                isUpdateable = true;
                            }
                        }
                    }
                }
                if (!isSys)
                {
                    if (!isUpdateable)
                    {
                        refreshedColumns.Add(col);
                    }
                }
            }
            AddCashValue(refreshedColumns, method_name, cashName);
            return refreshedColumns;
        }
        private List<VColumn> GetUpdateableColumns(VQueryCall queryCall, List<VColumn> columns)
        {
            string method_name = MethodBase.GetCurrentMethod().ToString();
            string cashName = queryCall.XName;
            if (IsCashValueExists(method_name, cashName))
            {
                return (GetCashValue(method_name, cashName) as List<VColumn>);
            }
            var allColumns = columns;
            var updatebleColumns = new List<VColumn>();
            foreach (VColumn col in allColumns)
            {
                bool isUpdateable = false;
                VSXElement srcCol = null;
                bool isSys = false;
                if (col.P_Table == queryCall.XName)
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
                            if ((srcCol as VColumn).Source() is VTable)
                            {
                                isUpdateable = true;
                            }
                        }
                    }
                }
                if (!isSys)
                {
                    if (isUpdateable)
                    {
                        updatebleColumns.Add(col);
                    }
                }
            }
            AddCashValue(updatebleColumns, method_name, cashName);
            return updatebleColumns;
        }
        private List<VColumn> GetUpdateableColumnsExt(VDataTable table, VQueryCall queryCall, List<VColumn> columns)
        {
            string method_name = MethodBase.GetCurrentMethod().ToString();
            string cashName = queryCall.XName;
            if (IsCashValueExists(method_name, cashName))
            {
                return (GetCashValue(method_name, cashName) as List<VColumn>);
            }
            var updCols = new List<VColumn>();
            updCols.AddRange(GetUpdateableColumns(queryCall, columns));
            var refrCols = GetRefreshedColumns(queryCall, columns);
            foreach (VColumn col in refrCols)
            {
                var dataCol = table.GetColumn(col.XName);
                if (dataCol.ColumnEditableSource != null && dataCol.ColumnEditableSource != TextConst.AVBool.False)
                {
                    updCols.Add(col);
                }
            }
            AddCashValue(updCols, method_name, cashName);
            return updCols;
        }
        private static void setUpdateable(VDataTable table, XElement xtbl)
        {
            var xcols = xtbl.Element(EName.columns).Elements();
            foreach (var xcol in xcols)
            {
                VDataColumn dataCol = table.GetColumn(xcol.Attribute(_AName.name).Value);
                if (xcol.AttrOrDefault(_AName.is_updateable, false))
                {
                    dataCol.IsUpdateable = true;
                }
                if (xcol.AttrOrDefault(_AName.is_updateable_ext, false))
                {
                    dataCol.TempColumnName = xcol.Attribute(_AName.temp_col_name).Value;
                    dataCol.DbColumnName = xcol.Attribute(_AName.update_target).Value;
                }
            }
        }
        private XElement GetCompiledQuery(XElement qry, string cashId)
        {
            string method_name = MethodBase.GetCurrentMethod().ToString();
            if (IsCashValueExists(method_name, cashId))
            {
                return new XElement(GetCashValue(method_name, cashId) as XElement);
            }
            XElement cquery = Compiler.GetCompiledQuery(qry);
            AddCashValue(cquery, method_name, cashId);
            return new XElement(cquery);
        }
        private static void createDataAtapter(VDataTable table, /*VQueryCall queryCall1, VColumn returnIntoColumn1, List<VColumn> columns1,*/ XElement xtable/*, SortedList<string,VQueryCall> queryCalls*/)
        {
            VOracleDataAdapter dataAdapter = new VOracleDataAdapter();
            //XElement qry = createTableQuery(queryCall, columns, false);
            foreach (XElement xtrakey in xtable.Elements(EName.extra_key))
            {
                if (table.ExtensionKeys == null)
                {
                    table.ExtensionKeys = new Dictionary<string, string>();
                }
                table.ExtensionKeys.Add(xtrakey.Attribute(_AName.table).Value, xtrakey.Attribute(_AName.column).Value);
            }
            ////if (returnIntoColumn != null)// Не прежилось
            ////{
            ////    qry = extendTableQueryForSelection(queryCall, qry, returnIntoColumn, keyColumn);
            ////}
            //VReport.PreprocessSimpleParams(qry);
            //XElement preCompiledQuery = GetCompiledQuery(qry, queryCall.XName);
            //XElement compiledQuery = Compiler.FinalProcessingQuery(new XElement(preCompiledQuery));
            //string selectText = getSelectText(compiledQuery);
            //dataAdapter.SelectCommand = new OracleCommand(selectText);

            dataAdapter.SelectCommand = new VOracleCommand(ReadElementAsString(xtable, EName.select_text));
            // DevAnalyzer.AnalyzePrepSql(dataAdapter.SelectCommand.CommandText);
            string procText = ReadElementAsString(xtable, EName.proc_text);
            if (procText != null)
            {
                table.ProcedureCommand = new VOracleCommand(procText.Replace('\r', ' '));
            }
            table.DataAdapter = dataAdapter;
            var xcols = xtable.Element(EName.columns).Elements();
            bool isMSUpd = !xtable.AttrOrDefault(_AName.is_ms_upd, true);
            //var xupdatebleColumns = xcols.Where(e=>Cmn.GetAttrValue(e,TextConst.DsAName.IsUpdateable)==TextConst.AVBool.True).ToList();
            var xupdatebleColumns = xcols.Where(e => e.AttrOrDefault(_AName.is_updateable, false) || e.AttrOrDefault(_AName.is_join_col, false)).ToList();
            var xupdatebleColumnsExt = xcols.Where(e => e.AttrOrDefault(_AName.is_updateable_ext, false)).ToList();
            var xjoinColumns = xcols.Where(e => e.AttrOrDefault(_AName.is_join_col, false)).ToList();
            string key = xtable.AttrOrDefault(_AName.key, string.Empty);
            var xkeyColumns = xcols.Where(e => e.AttrOrDefault(_AName.name, string.Empty) == key).ToList();
            var xrefreshedColumns = xcols.Where(e => e.AttrOrDefault(_AName.is_refreshed, false)).ToList();
            if (!table.HasPrimaryKey()) return;
            string keyColName = table.PrimaryKey[0].ColumnName;
            //if (keyColumn == null) return;
            //var pars = getOracleParams(updatebleColumns).ToArray();
            var xparCols = xupdatebleColumns.ToList();
            if (isMSUpd)
            {
                xparCols.AddRange(xkeyColumns);
                xparCols = xparCols.Distinct().ToList();
            }
            // 09,01,08 Емцов, добавил ClearSql, тк падало при update с непонятной ошибкой
            var pars = getOracleParams(xparCols).ToArray();
            dataAdapter.UpdateCommand = new VOracleCommand(Cmn.ClearSql(ReadElementAsString(xtable, EName.update_text)));
            dataAdapter.UpdateCommand.Parameters.AddRange(pars);
            //pars = getOracleParams(updatebleColumns, keyColumn.XName).ToArray();
            pars = getOracleParams(xupdatebleColumns, keyColName).ToArray();
            dataAdapter.InsertCommand = new VOracleCommand(Cmn.ClearSql(ReadElementAsString(xtable, EName.insert_text)));
            dataAdapter.InsertCommand.Parameters.AddRange(pars);
            //pars = getOracleParams(updatebleColumns.Where(e => e.IsKey).ToList()).ToArray();
            pars = getOracleParams(xkeyColumns).ToArray();
            dataAdapter.DeleteCommand = new VOracleCommand(Cmn.ClearSql(ReadElementAsString(xtable, EName.delete_text)));
            dataAdapter.DeleteCommand.Parameters.AddRange(pars);
            pars = getOracleParams(xkeyColumns).ToArray();
            string cmdText = ReadElementAsString(xtable, EName.clear_temp_text);
            var par = new VOracleParameter(TextConst.Pfx.Param + TextConst.DBParams.FormId, VOracleDbType.VarChar);
            if (!string.IsNullOrEmpty(cmdText))
            {
                table.ClearTempCommand = new VOracleCommand(cmdText);
                table.ClearTempCommand.Parameters.Add(par);
            }
            var parsList = getOracleParams(xupdatebleColumnsExt);
            par = new VOracleParameter(TextConst.Pfx.Param + TextConst.DBParams.FormId, VOracleDbType.VarChar);
            parsList.Add(par);
            if (!xkeyColumns.Any(c => c.Attribute(_AName.is_updateable) != null || c.Attribute(_AName.is_updateable_ext) != null))
            {
                parsList.AddRange(pars);
            }
            par = new VOracleParameter(TextConst.Pfx.Param + TextConst.DBParams.RowStateId, VOracleDbType.VarChar);
            parsList.Add(par);
            pars = parsList.ToArray();
            string updateTempText = ReadElementAsString(xtable, EName.update_temp_text);
            if (!string.IsNullOrEmpty(updateTempText))
            {
                table.UpdateTempCommand = new VOracleCommand(updateTempText);
                table.UpdateTempCommand.Parameters.AddRange(pars);
            }
            //table.UpdateTempCommand = new OracleCommand(getUpdateTempText(table, queryCall, updatebleColumns, keyColumn));
            ////table.UpdateTempCommand.Parameters.AddRange(pars);
            foreach (XElement xcol in xrefreshedColumns)
            {
                VDataColumn curDataCol = (VDataColumn)table.Columns[xcol.Attribute(_AName.name).Value];
                if (curDataCol.ColumnEditableSource == null)
                {
                    curDataCol.ColumnEditableSource = TextConst.AVBool.False;
                }
            }
            XElement xml = ReadAttrAsElem(xtable, EName.single_row_refresh_cmd);
            if (xml != null)
            {
                table.SingleRowRefreshCommand = VDBSelectCommand.FromXml(xml);
                foreach (XElement xcol in xrefreshedColumns)
                {
                    VDataColumn dataCol = table.GetColumn(xcol.Attribute(_AName.name).Value);
                    xml = ReadAttrAsElem(xcol, EName.value_refresh_cmd);
                    dataCol.ValueRefreshCommand = VDBSelectCommand.FromXml(xml);
                }
            }
            foreach (XElement xcol in xcols)
            {
                VDataColumn dataCol = table.GetColumn(xcol.Attribute(_AName.name).Value);
                foreach (XElement xdep in xcol.Elements(EName.dependants).Elements(EName.dependant))
                {
                    VDataTable depTbl = (VDataTable)table.GetDataSet().Tables[xdep.Attribute(_AName.table).Value];
                    VDataColumn depCol = depTbl.GetColumn(xdep.Attribute(_AName.name).Value);
                    dataCol.AddDependant(depCol);
                }
                foreach (XElement depRefCmd in xcol.Elements(EName.dep_refresh_cmd).Elements())
                {
                    VDBSelectCommand cmd = VDBSelectCommand.FromXml(depRefCmd);
                    dataCol.AddDependantsRefreshCommand(depRefCmd.Attribute(_AName.table).Value, cmd);
                }
                xml = ReadAttrAsElem(xcol, EName.value_reset_cmd);
                if (xml != null)
                {
                    dataCol.ValueResetCommand = VDBSelectCommand.FromXml(xml);
                }
            }
        }
        public static void ChangeQueryTableForUsingTemp(XElement compiledQuery, string tableName, VDataTable table, VDataColumn curDataCol, bool isEditorMain, string keyParNameIn, string keyDimension, bool nativeOnly = false, string dbKeyName = null, string subKeyParName = null, string subTempRowIdCol = null)
        {
            if (subTempRowIdCol == null)
            {
                subTempRowIdCol = TextConst.DBObjects.TempTableRowIdColumn;
            }
            string tableAlias = table.TableName;
            // var allColumns = getColumns(queryCall);
            VDataColumn prKeyCol = (VDataColumn)table.PrimaryKey[0];
            if (subKeyParName == null)
            {
                subKeyParName = keyParNameIn;
            }
            string keyParName = keyParNameIn;
            // var keyColumn = allColumns.Where(e => e.IsKey).FirstOrDefault();
            // VColumn keySource = (keyColumn.SourceColumn().First() as VColumn);
            List<XElement> xtables = GetCoreTableListFromCompiledQuery(compiledQuery, tableName, isEditorMain, keyDimension);
            List<string> masterColumnsNames = new List<string>();
            foreach (XElement xtable in xtables)
            {
                if (dbKeyName == null)
                {
                    dbKeyName = prKeyCol.DbColumnName;
                }
                keyParName = keyParNameIn;
                if (keyParNameIn != null && !string.IsNullOrEmpty(keyDimension))
                {
                    XAttribute dimAttr = xtable.Ancestors(EName.query).Where(q1 => q1.Attribute(_AName.multiplicate_point) != null).Attributes(_AName.dimension).FirstOrDefault();
                    if (dimAttr != null && dimAttr.Value != keyDimension)
                    {
                        XAttribute fisrtDimAttr = xtable.Ancestors(EName.query).Attributes(_AName.dimension).First();
                        if (fisrtDimAttr == null || fisrtDimAttr.Value != keyDimension)
                        {
                            keyParName = null;
                        }
                    }
                }
                //keyParName
                XElement rootQuery = xtable.Ancestors(EName.query).First();
                //var xcols = rootQuery.Element(TextConst.EName.Select)//!!! учесть еще where и остальные узлы
                //    .Descendants(TextConst.EName.Column).ToList();
                var xcolsAll = new List<XElement>();
                List<XElement> xcolsAllSelect = Compiler.getQueryColumns(rootQuery).ToList();
                List<XElement> xcallsEditable = Compiler.getQueryEditableCalls(rootQuery).ToList();
                List<XElement> xcolsAllJoin = Compiler.getQueryJoinColumns(rootQuery).ToList();
                var xeitableCols = new List<XElement>();
                xcolsAll.AddRange(xcolsAllSelect);
                xcolsAll.AddRange(xcolsAllJoin);
                xcolsAll.AddRange(xcallsEditable);
                string table_alias = xtable.Attribute(TextConst.AName.As).Value;
                foreach (XElement col in xcolsAll)
                {
                    if (col.AttrOrDefault(_AName.table, string.Empty) != table_alias && col.Attribute(_AName.column_editable) != null)
                    {
                        xeitableCols.Add(col);
                    }
                }
                List<XElement> xcols1 = xcolsAll.Where(c => c.AttrOrDefault(_AName.table, string.Empty) == table_alias
                         // ||  c.Parent.Name.LocalName==TextConst.EName.Select
                         || xcolsAllSelect.Contains(c) || xeitableCols.Contains(c)).ToList();
                //var xcols = xcolsAll.Where(c => Cmn.GetAttrValue(c, TextConst.AName.Table) == xtable.Attribute(TextConst.AName.As).Value)
                //    .ToList();
                foreach (XElement xcol_a in xcols1.ToArray())
                {
                    XElement xcol = xcol_a;
                    string colName;
                    if (xcol.Name == EName.column)
                    {
                        colName = xcol.Attribute(_AName.column).Value;
                    }
                    else
                    {
                        colName = xcol.Attribute(_AName.@as).Value;
                    }
                    string colAlias = xcol.AttrOrEmpty(_AName.@as);
                    if (colAlias == string.Empty)
                    {
                        colAlias = colName;
                    }
                    VDataColumn parentCol;
                    if (TextConst.AVColumnArray.SysColNamesForEditedObject.Contains(colName))
                    { // чтобы обновлялись колонки зависимые от is_new
                        //parentCol = table.Columns.Cast<VDataColumn>().FirstOrDefault(c => c.ColumnName == colAlias);
                        parentCol = (VDataColumn)table.Columns[colAlias];
                    }
                    else
                    {
                        parentCol = table.Columns.Cast<VDataColumn>().FirstOrDefault(c => c.DbColumnName == colAlias);
                    }
                    //  VSXElement vparentCol = allColumns.Where(e => e.P_Column == colAlias).FirstOrDefault();
                    if (parentCol != null)
                    {
                        //string alias = parentCol.DbColumnName;
                        //  var parentCol = (VDataColumn)table.Columns[alias];
                        if (parentCol.IsUpdateable || parentCol.ColumnEditableSource != TextConst.AVBool.False /* && !TextConst.AVColumnArray.SysColNamesForEditedObject.Contains(alias)*/)
                        {
                            if (!parentCol.IsUpdateable)
                            {
                                //string curValParName = colAlias + TextConst.Pfx.CurValParam;
                                //  var xconst = new XElement(TextConst.EName.Const, new XAttribute(TextConst.AName.As, colAlias), new XAttribute(TextConst.AName.DataType, Cmn.GetAttrValue(xcol, TextConst.AName.DataType)));
                                //    xconst.Value = TextConst.Pfx.Param + curValParName;
                                //   xcol.ReplaceWith(xconst);
                                if (xcol.Name != EName.column)
                                {
                                    XElement xcol1 = new XElement(EName.column);
                                    xcol.ReplaceWith(xcol1);
                                    xcols1.Remove(xcol);
                                    xcols1.Add(xcol1);
                                    xcol1.CopyAttributes(xcol.Attributes());
                                    xcol = xcol1;
                                }
                                xcol.SetAttributeValue(_AName.table, table_alias);
                                xcol.SetAttributeValue(_AName.column, xcol.Attribute(TextConst.AName.As).Value);
                            }
                            if (curDataCol != null)
                            {
                                if (!masterColumnsNames.Contains(colAlias))
                                {
                                    masterColumnsNames.Add(colAlias);
                                    if (parentCol != table.PrimaryKey[0])
                                    {
                                        parentCol.AddDependant(curDataCol);
                                    }
                                    // xpar = new XElement(TextConst.EName.Param);
                                    // xpar.SetAttributeValue(TextConst.AName.Name, curValParName);
                                    // xpar.SetAttributeValue(TextConst.AName.DataType, vparentCol.XDataType());
                                    // xpar.SetAttributeValue(TextConst.AName.Column, colAlias);
                                    // colValQuery.Element(TextConst.EName.Params).Add(xpar);
                                }
                            }
                        }
                        else
                        {
                            xcols1.Remove(xcol);
                        }
                    }
                    else
                    {
                        if (xcol.AttrOrDefault(_AName.table, string.Empty) != table_alias)
                        {
                            xcols1.Remove(xcol);
                        }
                    }
                }
                string newText = "";
                string newTextReal = "";
                string newTextDual = "";
                string newTextMix = "";
                string newTextNative = "";
                string aliasOrig = "a";
                string aliasTemp = "t";
                string q = "";
                List<string> colNames = new List<string>();
                string mixKeyExpr = "";
                foreach (XElement xcol in xcols1.ToArray())
                {
                    string colName = xcol.Attribute(_AName.column).Value;
                    //string colAlias = xcol.AttrOrEmpty(_AName.@as);
                    //if (colAlias == string.Empty)
                    //{
                    //    colAlias = colName;
                    //}
                    var colAlias = colName; // 2023-09-21 Бельченко: убрал псевдоним т.к. похоже здесь д. быть всегда оригинальное имя колонки , была пробема в новой форме по банкротству
                    if (!colNames.Contains(colName))
                    {
                        colNames.Add(colName);
                        string vDual = null;
                        // string vNative = "";
                        VDataColumn parentCol = table.Columns.Cast<VDataColumn>().Where(c => c.DbColumnName == colAlias).FirstOrDefault();
                        //  VSXElement vparentCol = allColumns.Where(e => e.P_Column == colAlias).FirstOrDefault();
                        string vNative = VSourcedElement.GetSysColDefaultValue(colName);
                        string vReal = null;
                        if (colName == TextConst.AVColumn.IsNew)
                        {
                            vReal = "decode (" + TextConst.DBObjects.TempTableStateColumn + "," + VDataTable.addStateVal + ",1,0)";
                        }
                        else if (colName == TextConst.AVColumn.IsNotNew)
                        {
                            vReal = "decode (" + TextConst.DBObjects.TempTableStateColumn + "," + VDataTable.addStateVal + ",0,1)";
                        }
                        if (parentCol != null)
                        {
                            string alias = parentCol.DbColumnName;
                            //  string curValParName = vparentCol.XName + TextConst.Pfx.CurValParam;
                            vReal = aliasTemp + "." + parentCol.TempColumnName;
                            if (parentCol.Table.PrimaryKey[0] == parentCol)
                            {
                                vDual = "-1";
                                // vDual = aliasTemp + "." + parentCol.TempColumnName;
                            }
                            else
                            {
                                vDual = aliasTemp + "." + parentCol.TempColumnName;
                            }
                            if (!parentCol.IsUpdateable)
                            {
                                vNative = "null";  //  !!! Для неизмененных строк нужно тянуть из первоисточника сделать case выше
                                                   // vNative = aliasOrig + "." + colName;
                            }
                            else
                            {
                                vNative = aliasOrig + "." + colName;
                            }
                        }
                        else
                        {
                            if (vReal == null)
                            {
                                //vReal = aliasOrig + "." + colName;
                                vNative = aliasOrig + "." + colName;
                                vReal = aliasTemp + "." + VQuery.GetColumnTempName(tableName, colName, Cmn.GetAttrValue(xcol, TextConst.AName.Type));
                            }
                            else
                            {
                                if (vNative == null)
                                {
                                    vNative = vReal;
                                }
                            }
                            vDual = "null";
                        }
                        string vMix = string.Format("(case when ({0}) then {1} else {2} end)",
                                aliasTemp + "." + TextConst.DBObjects.TempTableTableIdColumn + " is not null",
                                vReal,
                                vNative);
                        if (dbKeyName == colName || vNative == vReal)
                        {
                            mixKeyExpr = vMix;
                        }
                        //vMix = vNative;
                        //}
                        //else
                        //{
                        //    vNative = vReal;
                        //    vDual = vReal;
                        //}
                        newTextReal += q + vReal + " as " + colAlias;
                        newTextDual += q + vDual + " as " + colAlias;
                        newTextMix += q + vMix + " as " + colAlias;
                        newTextNative += q + vNative + " as " + colAlias;
                        q = ",";
                    }
                }
                newText += "(";
                if (keyParName != null || xtable.AttrOrDefault(_AName.is_from_temp, string.Empty) == TextConst.AVBool.False)
                {
                    newText += "select ";
                    //newText += newTextReal;
                    newText += newTextNative;
                    newText += "  from ";
                    newText += tableName + " " + aliasOrig;
                    if (!nativeOnly)
                    {
                        newText += " where not exists (select * from " + TextConst.DBObjects.TempTable + " " + aliasTemp;
                        newText += " where ";
                        newText += aliasTemp + "." + TextConst.DBObjects.TempTableTableIdColumn + " ='" + tableAlias + "'";
                        newText += " and ";
                        newText += aliasTemp + "." + TextConst.DBObjects.TempTableFormIdColumn + "=" + TextConst.Pfx.Param + TextConst.DBParams.FormId + " ";
                        newText += " and ";
                        newText += aliasOrig + "." + dbKeyName + "=" + aliasTemp + "." + subTempRowIdCol;
                        newText += " )";
                    }
                    if (keyParName != null)
                    {
                        if (!nativeOnly)
                        {
                            newText += " and ";
                        }
                        else
                        {
                            newText += " where ";
                        }
                        newText += "  " + aliasOrig + "." + dbKeyName + " in " + TextConst.Pfx.Param + subKeyParName + " ";
                        //  newText += " and " + TextConst.Pfx.Param + TextConst.DBParams.IsNewRowParam + "=0 ";
                    }
                }
                //if (keyParName != null || (Cmn.GetAttrValue(xtable, TextConst.AName.NewRowsVisForOtherTbls) == TextConst.AVBool.True))
                //{
                if (!nativeOnly)
                {
                    if (keyParName != null || xtable.AttrOrDefault(_AName.is_from_temp, string.Empty) == TextConst.AVBool.True)
                    {
                        if (keyParName != null)
                        {
                            newText += "  union all ";
                        }
                        newText += " select ";
                        newText += newTextReal;
                        //newText += " from dual  where " + TextConst.Pfx.Param + TextConst.DBParams.IsNewRowParam + "=1)";
                        newText += "  from ";
                        newText += TextConst.DBObjects.TempTable + " " + aliasTemp;
                        newText += " where " + aliasTemp + "." + TextConst.DBObjects.TempTableTableIdColumn + " ='" + tableAlias + "'";
                        newText += " and " + aliasTemp + "." + TextConst.DBObjects.TempTableFormIdColumn + "=" + TextConst.Pfx.Param + TextConst.DBParams.FormId + " ";
                        newText += " and " + aliasTemp + "." + TextConst.DBObjects.TempTableStateColumn + "!=" + VDataTable.delStateVal.ToString() + " ";
                        if (keyParName != null)
                        {
                            //newText += " and " + aliasTemp + "." + TextConst.DBObjects.TempTableRowIdColumn + "=" + TextConst.Pfx.Param + TextConst.DBParams.TempRowId;// 
                            newText += " and " + aliasTemp + "." + TextConst.DBObjects.TempTableRowIdColumn + " in " + TextConst.Pfx.Param + keyParName + " ";
                            // newText += " and " + TextConst.Pfx.Param + TextConst.DBParams.IsNewRowParam + "=1 ";
                        }
                        else
                        {
                            // newText += " and " + aliasTemp + "." + TextConst.DBObjects.TempTableStateColumn + "=1 ";
                        }
                    }
                }
                newText += ")";
                xtable.SetAttributeValue(_AName.name, newText);
                rootQuery.Descendants(EName.column).Attributes(_AName.sys).Remove(); // пытаюсь сделать чтобы is_new, is_not_new можно было использовать в выражениях
                //xtable.SetAttributeValue(TextConst.AName.FromTemp, TextConst.AVBool.True);
            }
        }
        public XElement CreateTableQueryParamsExtendedForSelection(VQueryCall queryCall, VColumn column)
        {
            XElement originalParams = this.createTableQueryParams(queryCall);
            return VForm.ExtendParamsForSelection(originalParams, column);
        }
        public static XElement ExtendParamsForSelection(XElement originalParams, VColumn column)
        {
            originalParams = new XElement(originalParams);
            var columnParams = (column.RootQuery() as VForm).createTableQueryParams(column.Source());
            if (originalParams == null)
            {
                originalParams = new XElement(EName.@params);
            }
            if (columnParams != null)
            {
                originalParams.AddFirst(columnParams.Elements());
            }
            return originalParams;
        }
        public XElement CreateTableQuery(VQueryCall queryCall)
        {
            List<VColumn> columns = this.getColumns(queryCall);
            return this.createTableQuery(queryCall, columns, false, true);
        }
        private XElement createTableQuery(VQueryCall queryCall, List<VColumn> columns, bool isContext, bool allColumns)
        {
            XElement query = new XElement(EName.query);
            XElement select = new XElement(EName.select);
            query.Add(select);
            XElement from = new XElement(EName.from);
            query.Add(from);
            if (allColumns)
            {
                query.CopyAttributes(queryCall.Attributes(_AName.order));
                //Cmn.CopyAttribute(queryCall, query, TextConst.AName.Order);
            }
            // query.SetAttributeValue("hint", "first_rows");
            // VQueryCall qube = queryCall;
            VSXElement where1 = null;
            VSXElement where2 = null;
            //if (columns == null)
            //{
            //    columns = getColumns(queryCall);
            //}
            VSXElement xqube = queryCall.GetElementsP(EName.qube).FirstOrDefault();
            if (isContext)
            {
                where1 = queryCall.GetElementsP(EName.where).FirstOrDefault();
                if (where1 != null)
                {
                    where1.P_Exclude = "1";
                }
                if (xqube != null)
                {
                    where2 = xqube.GetElementsP(EName.where).FirstOrDefault();
                    if (where2 != null)
                    {
                        where2.P_Exclude = "1";
                    }
                }
            }
            //qube = queryCall.Links(null).Where(l => /*l.Query().IsQube() &&*/ l.P_Prime == TextConst.AVBool.True).FirstOrDefault();
            //if (qube != null)
            //{
            //        if (!columns.Where(c => c.P_Table == qube.XName).Any())
            //        {
            //            qube = null;
            //        }
            //}
            VSXElement qry = queryCall.Query().GetMainE();
            XElement fromQuery = null;
            XElement mainFromQuery = null;
            VDimension qdim = queryCall.Query().GetDimension();
            if (xqube == null)
            {
                fromQuery = new XElement(EName.query);
                fromQuery.Add(new XAttribute(_AName.name, qry.Attribute(_AName.name).Value));
                fromQuery.Add(new XAttribute(_AName.@as, queryCall.XName));
                mainFromQuery = fromQuery;
                if (qdim != null)
                {
                    fromQuery.Add(new XAttribute(_AName.dimension, qdim.P_IdName));
                }
                // fromQuery.SetAttributeValue(TextConst.AName.MainInEditor, TextConst.AVBool.True);
            }
            else
            {
                //xqube.Remove();
                fromQuery = new XElement(xqube);
                mainFromQuery = new XElement(EName.link);
                mainFromQuery.Add(new XAttribute(_AName.name, qdim.XName));
                mainFromQuery.Add(new XAttribute(_AName.@as, queryCall.XName));
                mainFromQuery.Add(new XAttribute(_AName.all_rows, TextConst.AVBool.True));
                fromQuery.Add(mainFromQuery);
                fromQuery.Add(new XElement(EName.dimset, new XAttribute(_AName.@as, queryCall.XName)));
            }
            foreach (XElement e in queryCall.Elements())
            {
                mainFromQuery.Add(new XElement(e));
            }
            mainFromQuery.Elements(TextConst.EName.Query).Remove();
            mainFromQuery.Elements(TextConst.EName.Qube).Remove();
            mainFromQuery.Elements(TextConst.EName.ELink).Remove();
            from.Add(fromQuery);
            foreach (VSXElement col in columns)
            {
                XElement xcol = new XElement(col.GetDummyOrSelf());
                xcol.Elements(EName.listquery).Remove();
                select.Add(xcol);
            }
            XElement xwhere = null;
            XElement xpars = createTableQueryParams(queryCall);
            if (queryCall is VELink && !isContext)
            {
                xwhere = queryCall.GetElementsP(EName.where).FirstOrDefault();
                if (xwhere != null)
                {
                    xwhere = new XElement(xwhere);
                }
                else
                {
                    xwhere = new XElement(EName.where);
                }
                VRelation rel = (queryCall as VELink).GetRelation();
                VSXElement childCol = rel.ChildColumnSource();
                XElement xcall = Factory.NewCall(TextConst.AVFunction.Equal,
                                    Factory.NewColumn(table: queryCall.XName, column: childCol.XName),
                                    Factory.NewUseParam("fk_" + childCol.XName)
                                 );
                xcall.Add(new XAttribute(_AName.dont_push, TextConst.AVBool.True));
                xwhere = Compiler.extendWhereByAnd(xwhere, xcall);
            }
            else
            {
                xwhere = queryCall.GetElementsP(EName.where).FirstOrDefault();
                //var fqw = fromQuery.Elements(TextConst.EName.Where).ToArray();
                //if (fqw.Any())
                //{
                //    fqw.Remove();// скорее всего не может быть
                //}
                //}
                //      xwhere.Remove();
                //  xwhere = new XElement(TextConst.EName.Where,queryCall.RootQuery().GetWhereSections().SelectMany(e => e.GetElementsApplyingParts()));
            }
            query.AddFirst(xpars);
            mainFromQuery.Elements(EName.where).Remove();
            //if (qube == null)
            //{
            query.Add(xwhere);
            query.Descendants(EName.elink).Remove();
            if (where1 != null)
            {
                where1.P_Exclude = "";
            }
            if (where2 != null)
            {
                where2.P_Exclude = "";
            }
            IList<VSXElement> xjoins = queryCall.GetElementsP(EName.query);
            if (xjoins.Count > 0)
            {
                from.Add(xjoins);
            }
            XElement xExpressions = this.Element(EName.expressions);
            if (xExpressions != null && select.Descendants(EName.fact).Any())
            {
                query.Add(new XElement(xExpressions));
            }
            if (this.GetElementsP(EName.from).First().P_StarScheme == TextConst.AVBool.True)
            {
                query.SetAttributeValue(_AName.star_scheme, TextConst.AVBool.True);
            }
            if (this.GetElementsP(EName.from).First().P_SingleWay == TextConst.AVBool.True)
            {
                query.SetAttributeValue(_AName.single_way, TextConst.AVBool.True);
            }
            query.SetAttributeValue(_AName.merge_dimsets, TextConst.AVBool.True);
            query.CopyAttributes(this.Attributes(_AName.use_repository));
            return query;
        }
        private XElement createTableQueryParams(VQueryCall queryCall)
        {
            XElement xpars = this.Element(EName.@params);
            if (xpars != null)
            {
                xpars = new XElement(xpars);
            }
            else
            {
                xpars = new XElement(EName.@params);
            }
            if (queryCall is VELink)
            {
                VRelation rel = (queryCall as VELink).GetRelation();
                VSXElement childCol = rel.ChildColumnSource();
                AddNewParam(xpars, "fk_" + childCol.XName, childCol.XDataType());
            }
            VForm form = (queryCall.RootQuery() as VForm);
            foreach (VSXElement el in form.ParamFields())
            {
                AddNewParam(xpars, el.P_FormalParNameS, el.XDataType());
            }
            foreach (VSXElement el in form.VariableColumns())
            {
                AddNewParam(xpars, el.P_ParName, el.XDataType());
            }
            AddNewParam(xpars, TextConst.AVParam.FormValid, TextConst.AVDataType.Number);
            AddNewParam(xpars, TextConst.AVParam.FormValidNot, TextConst.AVDataType.Number);
            foreach (VQueryCall qry in MainAndRelatedQueries())
            {
                string name = qry.XName + TextConst.AVParam.HasChanges;
                AddNewParam(xpars, name, TextConst.AVDataType.Number);
            }
            return xpars;
        }
        private XElement createTableQueryForSpcifiedColumn(VColumn column, VColumn keyColumn)
        {
            VQueryCall queryCall = column.Source();
            queryCall = (VQueryCall)queryCall.GetAncestorsAndSelf().First(e => (e is VFromQuery) || e.GetType() == typeof(VELink) || (e is VParam));
            var columns = new List<VColumn>();
            columns.Add(column);
            if (keyColumn != null)
            {
                columns.Add(keyColumn);
            }
            return this.createTableQuery(queryCall, columns, true, false);
        }
        private XElement createTableQueryForSpcifiedColumns(VQueryCall queryCall, List<VColumn> columns, VColumn keyColumn) //!!! Скопировал. Реализовать
        {
            if (keyColumn != null)
            {
                columns.Add(keyColumn);
            }
            return this.createTableQuery(queryCall, columns, true, false);
        }
        private static XElement getOrCreateBand(SortedList<string, XElement> bands, VGridBand band)
        {
            if (band == null)
            {
                return bands[""];
            }
            string path = band.GetPath();
            if (bands.ContainsKey(path))
            {
                return bands[path];
            }
            XElement parband = VForm.getOrCreateBand(bands, band.GetParent() as VGridBand);
            XElement band1 = new XElement(EName.band);
            band1.CopyAttributes(band.Attributes());
            parband.Add(band1);
            bands.Add(path, band1);
            return band1;
        }
        private XElement createTableScheme(VQueryCall queryCall, List<VColumn> columns, bool isRet = false)
        {
            XElement xtable = new XElement(EName.table);
            xtable.CopyAttributes(queryCall.Attributes(_AName.name));
            xtable.Add(new XAttribute(_AName.@as, queryCall.XName));
            XElement xcols = new XElement(EName.columns);
            xtable.Add(xcols);
            XElement xviewcols = new XElement(EName.viewcolumns);
            xtable.Add(xviewcols);
            SortedList<string, XElement> bands = new SortedList<string, XElement>();
            bands.Add(string.Empty, xviewcols);
            foreach (VColumn col in columns)
            {
                XElement xcol = Factory.NewColumn(name: col.XName, data_type: col.XDataType(), title: null);
                string f = col.XFormat();
                if (f != string.Empty)
                {
                    xcol.Add(new XAttribute(_AName.format, f));
                    xcol.Add(new XAttribute(_AName.edit_mask, f)); // путаница не понятно что для чего, поставлю на всякий случай
                }
                string halign = col.XHAlign();
                if (halign != string.Empty)
                {
                    xcol.Add(new XAttribute(_AName.halign, halign));
                }
                if (col.P_InvisibleInColumnChooser == TextConst.AVBool.True)
                {
                    xcol.Add(new XAttribute(_AName.invisible_in_column_chooser, TextConst.AVBool.True));
                }
                if ((col.IsAddision && !col.IsAddisionForName) || col.IsRelation)
                {
                    xcol.Add(new XAttribute(_AName.visible, TextConst.AVBool.False));
                }
                else
                {
                    xcol.Add(new XAttribute(_AName.title, col.P_Title));
                    if (col.P_ColumnVisible == TextConst.AVBool.False)
                    {
                        xcol.Add(new XAttribute(_AName.visible, TextConst.AVBool.False));
                    }
                }
                xcols.Add(xcol);
                xcol = new XElement(xcol);
                // фиксация колонки
                if (col.P_FixedSide != string.Empty)
                {
                    xcol.Add(new XAttribute(_AName.fixed_side, col.P_FixedSide));
                }
                if (col.IsAddisionForName)
                {
                    //string name = col.XName;
                    //TextConst.Pfx.ExtValName
                    //  string kodName = name.Substring(0, name.Length - TextConst.Pfx.ExtValName.Length);
                    string kodName = col.TextSourceFor;
                    XElement kodCol = xviewcols.Descendants(_AName.column).Where(e => e.Attribute(_AName.name).Value == kodName).FirstOrDefault();
                    if (kodCol != null)
                    {
                        // Cmn.CopyAttribute(kodCol, xcol, TextConst.AName.Visible);
                        kodCol.AddBeforeSelf(xcol);
                    }
                }
                else
                {
                    if (col.GetParent().Name == EName.columns || col.GetParent().Name == EName.band) // временное решение для сучая когда по одной колонке есть поле  в гриде и на форме
                    {
                        VGridBand band = null;
                        if (col.VirtualParent == null)
                        {
                            band = col.GetParent() as VGridBand;
                        }
                        XElement band1 = getOrCreateBand(bands, band);
                        band1.Add(xcol);
                    }
                    //xviewcols.Add(xcol);
                }
            }
            if (isRet)
            {
                XElement xcol = Factory.NewColumn(name: TextConst.SpecCols.Check, data_type: TextConst.AVDataType.Number, title: TextConst.SpecColsTitle.Check);
                xcols.AddFirst(xcol);
                xviewcols.AddFirst(xcol);
            }
            // xcols = new XElement(TextConst.EName.ViewColumns, xcols.Elements().Select(e => new XElement(e)));
            return xtable;
        }
        private static string getModifiedRowSelectTextWithOther(IEnumerable<string> columns, IEnumerable<string> otherColumns, string tableName, string keyColName, string keyPar)
        {
            var sql = new StringBuilder();
            sql.AppendLine(" ( select ");
            bool first = true;
            foreach (string col in columns)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sql.Append(',');
                }
                sql.Append(TextConst.Pfx.Param);
                sql.Append(col);
                sql.Append(" as ");
                sql.AppendLine(col);
            }
            foreach (var col in otherColumns)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sql.Append(',');
                }
                sql.Append("a.");
                sql.AppendLine(col);
            }
            sql.AppendLine();
            sql.Append("from ");
            sql.Append(tableName);
            sql.Append(" a  where ");
            sql.Append(keyColName);
            sql.Append('=');
            sql.Append(keyPar);
            sql.AppendLine(")");
            return sql.ToString();
        }
        private static string getModifiedRowSelectText(IEnumerable<string> columns, Dictionary<string, string> joinInfo = null)
        {
            var sql = new StringBuilder();
            sql.AppendLine(" ( select ");
            bool first = true;
            foreach (string alias in columns)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sql.Append(',');
                }
                sql.Append(TextConst.Pfx.Param);
                string col;
                if (joinInfo != null && joinInfo.TryGetValue(alias, out col))
                {
                    sql.Append(col);
                }
                else
                {
                    sql.Append(alias);
                }
                sql.Append(" as ");
                sql.AppendLine(alias);
            }
            sql.AppendLine();
            sql.AppendLine("from dual )");
            return sql.ToString();
        }
        private static string getModifiedRowSelectText(IEnumerable<VColumn> columns)
        {
            return getModifiedRowSelectText(columns.Select(e => e.XName));
        }
        private static string getModifiedRowSelectText(IEnumerable<XElement> columns, Dictionary<string, string> joinInfo)
        {
            return getModifiedRowSelectText(columns.Attributes(_AName.name).Select(APredicate.AttributeValue), joinInfo);
        }
        private static string getMergeText(VQueryCall query, List<VColumn> columns, ref Dictionary<string, string> joinInfo)//все алиасы у колонок должны совпадать, пока так
        {
            var sql = new StringBuilder();
            var sqlSet = new StringBuilder();
            var sqlCols = new StringBuilder();
            var sqlVals = new StringBuilder();
            var sqlJoin = new StringBuilder();
            //string q = "";
            List<VColumn> columns0 = new List<VColumn>(columns.Count);
            List<VColumn> columns1 = new List<VColumn>(columns.Count);
            List<VSXElement> columns2 = new List<VSXElement>(columns.Count);
            HashSet<string> colsNames = new HashSet<string>();
            foreach (VColumn col in columns)
            {
                columns0.Add(col);
                columns1.Add(col);
                columns2.Add(col);
                colsNames.Add(col.P_Column);
            }
            VSXElement joinCall = query.GetElementsP().First();
            List<VSXElement> jExprs = joinCall.GetElementsP(EName.call).ToList();
            if (jExprs.Count == 0)
            {
                jExprs.Add(joinCall);
            }
            //var jCols1 = jCols.Where(e => Cmn.GetAttrValue(e, TextConst.AName.Table) == query.XName).ToArray();
            //var jCols2 = jCols.Where(e => Cmn.GetAttrValue(e, TextConst.AName.Table) != query.XName).ToArray();
            int i = 0;
            var columns3 = columns1.ToList();
            //var columns4 = columns2.ToList();
            string query_name = query.XName;
            foreach (VCall jExpr in jExprs)
            {
                VSXElement jCol1 = jExpr.GetElementsP(EName.column).FirstOrDefault(e => e.AttrOrEmpty(_AName.table) == query_name);
                VSXElement jCol2 = jExpr.GetElementsP(EName.column).FirstOrDefault(e => e.AttrOrEmpty(_AName.table) != query_name);
                string sval;
                VSXElement item;
                if (jCol2 == null) // не универсально, наверное может быть выражение
                {
                    item = jExpr.GetElementsP(EName.@const).First();
                    sval = item.Value;
                }
                else
                {
                    item = jCol2;
                    sval = "p1." + jCol2.P_Column;
                }
                if (i > 0)
                {
                    sqlJoin.AppendLine("and");
                }
                if (jCol2 != null)
                {
                    VColumn xCol2 = query.UsedColumns().First(c => c.GetParent().Name == EName.select && c.P_Column == jCol2.P_Column);
                    VSXElement rCol = columns2.First(c => c.P_Column == xCol2.XName);
                    int iCol = columns2.IndexOf(rCol);
                    columns2.RemoveAt(iCol);
                    columns0.RemoveAt(iCol);
                    columns1.RemoveAt(iCol);
                    joinInfo.Add(xCol2.XName, jCol1.XName);
                }
                sqlJoin.Append("p.");
                sqlJoin.Append(jCol1.P_Column);
                sqlJoin.Append('=');
                sqlJoin.AppendLine(sval);
                if (!colsNames.Contains(jCol1.P_Column))
                {
                    columns2.Add(item);
                    columns1.Add((VColumn)jCol1);
                    //columns4.Add(item);
                    if (jCol2 != null)
                    {
                        columns3.Add((VColumn)jCol1);
                    }
                }
                i++;
            }
            bool first = true;
            foreach (VColumn col in columns0)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sqlSet.Append(',');
                }
                sqlSet.Append("p.");
                sqlSet.Append(col.SourceColumn().First().P_Column);
                sqlSet.Append("=p1.");
                sqlSet.AppendLine(col.P_Column);
            }
            string q = "";
            i = 0;
            foreach (VColumn col1 in columns1.ToList())
            {
                VSXElement col2 = columns2.ElementAt(i);
                sqlCols.Append(q);
                sqlCols.Append("p.");
                sqlCols.AppendLine(col1.SourceColumn().First().P_Column);
                sqlVals.Append(q);
                if (col2 is VConst)
                {
                    columns1.Remove(col1);
                    sqlVals.AppendLine(col2.Value);
                }
                else
                {
                    sqlVals.Append("p1.");
                    sqlVals.AppendLine(col2.P_Column);
                }
                q = ",";
                i++;
            }
            sql.Append("merge into ");
            sql.Append(query.Query().GetMainIE().P_IdName);
            sql.AppendLine(" p ");
            sql.Append("using ");
            sql.Append(getModifiedRowSelectText(columns3));
            sql.AppendLine(" p1 ");
            sql.AppendLine("on (");
            sql.AppendLine(sqlJoin.ToString());
            sql.AppendLine(")");
            sql.AppendLine("WHEN MATCHED THEN UPDATE SET");
            sql.AppendLine(sqlSet.ToString());
            sql.AppendLine("WHEN NOT MATCHED THEN INSERT ");
            sql.Append('(');
            sql.Append(sqlCols.ToString());
            sql.AppendLine(")");
            sql.AppendLine("VALUES ");
            sql.Append('(');
            sql.Append(sqlVals.ToString());
            sql.AppendLine(")");
            return sql.ToString();
            //         MERGE INTO va_dir_struct_opt p
            //USING (   SELECT :p_kod_direct kod_direct,:p_kod_dir_struct_type kod_dir_struct_type, :p_no_parent no_parent, :p_no_self no_self FROM dual) p1
            //ON (p.kod_direct = p1.kod_direct and p.kod_dir_struct_type = p1.kod_dir_struct_type )
            //WHEN MATCHED THEN UPDATE SET p.no_parent = p1.no_parent    , p.no_self = p1.no_self         
            //WHEN NOT MATCHED THEN INSERT (p.kod_direct, p.kod_dir_struct_type, p.no_parent, p.no_self)
            // VALUES (p1.kod_direct, p1.kod_dir_struct_type, p1.no_parent, p1.no_self);
        }
        private static string getUpdateText(VQueryCall queryCall, IEnumerable<VColumn> columns, VColumn keyCol)
        {
            var sql = new StringBuilder();
            sql.Append("update ");
            sql.Append(queryCall.Query().GetMainIE().P_IdName);
            sql.AppendLine(" set ");
            sql.Append('(');
            bool first = true;
            foreach (VColumn col in columns)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sql.Append(',');
                }
                sql.AppendLine(col.P_Column);
            }
            sql.AppendLine(")");
            sql.AppendLine("=");
            sql.AppendLine(getModifiedRowSelectText(columns));
            sql.AppendLine("where");
            sql.AppendLine(keyCol.P_Column + "=" + TextConst.Pfx.Param + keyCol.XName);
            return sql.ToString();
        }
        private static string getInsertText(VQueryCall queryCall, IEnumerable<VColumn> columns, VColumn keyCol)
        {
            var sql = new StringBuilder();
            sql.AppendLine("begin");
            sql.AppendLine("for r in ");
            sql.AppendLine(getModifiedRowSelectText(columns));
            sql.AppendLine("loop");
            sql.AppendLine("insert into " + queryCall.Query().GetMainIE().P_IdName);
            sql.Append('(');
            bool first = true;
            foreach (VColumn col in columns)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sql.Append(',');
                }
                sql.AppendLine(col.P_Column);
            }
            sql.AppendLine(")");
            sql.AppendLine(" values ");
            sql.Append('(');
            first = true;
            foreach (VColumn col in columns)
            {
                if (first)
                {
                    first = false;
                }
                else
                {
                    sql.Append(',');
                }
                sql.Append("r.");
                sql.AppendLine(col.XName);
            }
            sql.AppendLine(")");
            sql.AppendLine(" returning ");
            sql.Append(keyCol.P_Column);
            sql.Append(" into " + TextConst.Pfx.Param);
            sql.Append(keyCol.XName);
            sql.AppendLine(";");
            sql.AppendLine("end loop;");
            sql.AppendLine("end;");
            return sql.ToString();
        }
        private static string getDeleteText(VQueryCall queryCall, VColumn keyCol)
        {
            var sql = new StringBuilder();
            sql.Append("delete from ");
            sql.Append(queryCall.Query().GetMainIE().P_IdName);
            sql.Append(" where ");
            sql.Append(keyCol.P_Column);
            sql.Append("=" + TextConst.Pfx.Param);
            sql.Append(keyCol.XName);
            return sql.ToString();
        }
        private static string getUpdateTempText(VQueryCall queryCall, List<XElement> columns, List<XElement> otherColumns, VColumn keyCol, VColumn subKeyCol = null, string tableAlias = null, Dictionary<string, string> joinInfo = null)
        {
            return getUpdateTempTextSingle(queryCall, columns, otherColumns, keyCol, subKeyCol, tableAlias, joinInfo);
        }
        private static string getClearTempText(VQueryCall queryCall)
        {
            return getClearTempText(queryCall, queryCall.XName);
        }
        private static string getClearTempText(VQueryCall queryCall, string tableAlias)
        {
            var sql = new StringBuilder();
            sql.Append("delete from " + TextConst.DBObjects.TempTable);
            sql.Append(" where ");
            sql.Append(TextConst.DBObjects.TempTableTableIdColumn + "='");
            sql.Append(tableAlias);
            sql.Append("' and ");
            sql.Append(TextConst.DBObjects.TempTableFormIdColumn + "=" + TextConst.Pfx.Param + TextConst.DBParams.FormId);
            //sql.AppendLine("and");// вроде это не нужно чистим все
            //sql.AppendLine(TextConst.DBObjects.TempTableRowIdColumn + "=" + TextConst.Pfx.Param + keyCol.XName + ";");
            return sql.ToString();
        }
        private static string getUpdateTempTextSingle(VQueryCall queryCall, List<XElement> columns, List<XElement> otherColumns, VColumn keyCol, VColumn subKeyCol, string tableAlias, Dictionary<string, string> joinInfo)
        {
            string tableName = queryCall.Query().GetMainIE().P_Name;
            if (tableAlias == null)
            {
                tableAlias = queryCall.XName;
            }
            string subKeyCond;
            if (subKeyCol == null)
            {
                subKeyCol = keyCol;
                subKeyCond = string.Empty;
            }
            else
            {
                subKeyCond = " or " + TextConst.Pfx.Param + subKeyCol.XName + " is null";
            }
            var sql = new StringBuilder();
            sql.AppendLine("begin");
            sql.AppendLine("delete from " + TextConst.DBObjects.TempTable);
            sql.AppendLine("where");
            sql.Append(TextConst.DBObjects.TempTableTableIdColumn + "='");
            sql.Append(tableAlias);
            sql.AppendLine("'");
            sql.AppendLine("and");
            sql.AppendLine(TextConst.DBObjects.TempTableFormIdColumn + "=" + TextConst.Pfx.Param + TextConst.DBParams.FormId);
            sql.AppendLine("and");
            sql.Append(TextConst.DBObjects.TempTableRowIdColumn + "=" + TextConst.Pfx.Param);
            sql.Append(keyCol.XName);
            sql.AppendLine(";");
            // sql.AppendLine("end;");
            var sqlCols = new StringBuilder();
            var sqlVals = new StringBuilder();
            sqlCols.AppendLine(TextConst.DBObjects.TempTableTableIdColumn);
            sqlVals.AppendLine("'" + tableAlias + "'");
            sqlCols.Append(',');
            sqlCols.AppendLine(TextConst.DBObjects.TempTableFormIdColumn);
            sqlVals.Append(',');
            sqlVals.AppendLine(TextConst.Pfx.Param + TextConst.DBParams.FormId);
            sqlCols.Append(',');
            sqlCols.AppendLine(TextConst.DBObjects.TempTableRowIdColumn);
            sqlVals.Append(',');
            sqlVals.AppendLine(TextConst.Pfx.Param + keyCol.XName);
            sqlCols.Append(',');
            sqlCols.AppendLine(TextConst.DBObjects.TempTableStateColumn);
            sqlVals.Append(',');
            sqlVals.AppendLine(TextConst.Pfx.Param + TextConst.DBParams.RowStateId);
            string[] columnsNames = new string[columns.Count];
            for (int index = 0; index < columns.Count; index++)
            {
                XElement col = columns[index];
                string col_name = col.Attribute(_AName.name).Value;
                columnsNames[index] = col_name;
                sqlCols.Append(',');
                sqlCols.AppendLine(col.Attribute(_AName.temp_col_name).Value);
                sqlVals.Append(',');
                sqlVals.Append("r.");
                sqlVals.AppendLine(col_name);
            }
            // sql.AppendLine("begin");
            var sqlColsWithOther = new StringBuilder(sqlCols.ToString());
            var sqlValsWithOther = new StringBuilder(sqlVals.ToString());
            //List<string> otherColumnsNames = new List<string>();
            string[] otherColumnsNames = new string[otherColumns.Count];
            //foreach (VColumn col in otherColumns)
            for (int index = 0; index < otherColumns.Count; index++)
            {
                VColumn col = (VColumn)(otherColumns[index]);
                otherColumnsNames[index] = col.P_Column;
                sqlColsWithOther.Append(',');
                sqlColsWithOther.AppendLine(col.GetColumnTempName());
                sqlValsWithOther.Append(',');
                sqlValsWithOther.Append("r.");
                sqlValsWithOther.AppendLine(col.P_Column);
            }
            sql.Append("if " + TextConst.Pfx.Param + TextConst.DBParams.RowStateId + "=1 ");
            sql.Append(subKeyCond);
            sql.AppendLine(" then");
            sql.AppendLine("for r in ");
            sql.AppendLine(getModifiedRowSelectText(columns, joinInfo));
            sql.AppendLine("loop");
            sql.AppendLine("insert into " + TextConst.DBObjects.TempTable);
            sql.Append('(');
            sql.Append(sqlCols.ToString());
            sql.AppendLine(")");
            sql.AppendLine(" values ");
            sql.Append('(');
            sql.Append(sqlVals.ToString());
            sql.AppendLine(");");
            sql.AppendLine("end loop;");
            sql.AppendLine("else");
            sql.AppendLine("for r in ");
            VColumn scol = subKeyCol;
            VColumn dbCol = subKeyCol.SearchSourceDbColumn();
            if (dbCol != null)
            {
                scol = dbCol;
            }
            sql.AppendLine(getModifiedRowSelectTextWithOther(columnsNames, otherColumnsNames, tableName, scol.P_Column, TextConst.Pfx.Param + subKeyCol.XName));
            sql.AppendLine("loop");
            sql.AppendLine("insert into " + TextConst.DBObjects.TempTable);
            sql.Append('(');
            sql.Append(sqlColsWithOther.ToString());
            sql.AppendLine(")");
            sql.AppendLine(" values ");
            sql.Append('(');
            sql.Append(sqlValsWithOther.ToString());
            sql.AppendLine(");");
            sql.AppendLine("end loop;");
            sql.AppendLine("end if;");
            sql.AppendLine("end;");
            return sql.ToString();
        }
        private static List<VOracleParameter> getOracleParams(List<XElement> xcolumns, string retName = null)
        {
            var list = new List<VOracleParameter>(xcolumns.Count);
            foreach (XElement xcol in xcolumns)
            {
                string col_name = xcol.Attribute(_AName.name).Value;
                ParameterDirection par_direction;
                if (retName == col_name)
                {
                    par_direction = ParameterDirection.InputOutput;
                }
                else
                {
                    par_direction = ParameterDirection.Input;
                }
                VOracleParameter par = new VOracleParameter(TextConst.Pfx.Param + col_name, Cmn.GetDBType(xcol.Attribute(_AName.type).Value), par_direction);
                par.SourceColumn = col_name;
                list.Add(par);
            }
            return list;
        }
        private List<VOracleParameter> getOracleParams(List<VColumn> columns, string retName = null)
        {
            var list = new List<VOracleParameter>(columns.Count);
            foreach (VColumn col in columns)
            {
                ParameterDirection par_direction;
                if (retName == col.XName)
                {
                    par_direction = ParameterDirection.InputOutput;
                }
                else
                {
                    par_direction = ParameterDirection.Input;
                }
                VOracleParameter par = new VOracleParameter(TextConst.Pfx.Param + col.XName, Cmn.GetDBType(col.XDataType()), par_direction);
                par.SourceColumn = col.XName;
                list.Add(par);
            }
            return list;
        }
        /*public XElement AsProcessedXElementForSelect()
        {
            XElement form = this.GetFormXElement();
            string tableName = this.GetReturnTable().XName;
            XElement grid = form.Descendants(EName.grid).Where(e => e.Attribute(_AName.table).Value == tableName).First();
            XElement element = new XElement(EName.field);
            element.Add(new XAttribute(_AName.table, tableName));
            element.Add(new XAttribute(_AName.name, TextConst.SpecCols.Check));
            element.Add(new XAttribute(_AName.title, TextConst.SpecColsTitle.Check));
            element.Add(new XAttribute(_AName.controlType, typeof(UICheck).Name));
            grid.AddFirst(element);
            return form;
        }*/
        public override XElement GetFormXElement()
        {
            string method_name = MethodBase.GetCurrentMethod().ToString();
            if (this.IsCashValueExists(method_name, null))
            {
                return (this.GetCashValue(method_name, null) as XElement);
            }
            VForm frm = this.GetProcessed();
            XElement frm1 = frm.GetFormXElement1();
            this.AddCashValue(frm1, method_name, null);
            return frm1;
        }
        public XElement GetFormXElement1()
        {
            XElement xform = base.GetFormXElement();
            VSXElement content = this.GetElementsP(EName.content).FirstOrDefault();
            if (content == null)
            {
                content = this;
            }
            IList<VSXElement> elements = content.GetElementsP();
            foreach (VSXElement element in elements)
            {
                XElement el = VForm.AsProcessedXElementLevel(element);
                xform.Add(el);
            }
            xform.Add(this.GetElementsP(EName.events));
            // xform.Add(GetElementsApplyingParts(TextConst.EName.Toolbar));
            foreach (VSXElement element in this.GetElementsP(EName.toolbar))
            {
                XElement el = VForm.AsProcessedXElementLevel(element);
                xform.Add(el);
            }
            return xform;
        }
        public static XElement AsProcessedXElementLevel(VSXElement xitem_native)
        {
            XElement xitem;
            var vcolumn = xitem_native as VColumn;
            var vusefield = xitem_native as VUseField;
            var vuseform = xitem_native as VUseForm;
            //var vconst     = xitem_native as VConst;
            var vuicommand = xitem_native as VUICommand;
            // vcolumn
            if (vcolumn != null)
            {
                xitem = vcolumn.CreateFieldFromQueryColumn();
                IList<VSXElement> elements = xitem_native.GetElementsP();
                foreach (VSXElement element1 in elements)
                {
                    XElement el1 = AsProcessedXElementLevel(element1);
                    xitem.Add(el1);
                }
                VSXElement sCol = vcolumn.SourceColumn().First();
                VSXElement btns = sCol.GetElementsP(EName.buttons).FirstOrDefault();
                if (btns != null && vcolumn.GetElementsP(EName.buttons).Count == 0)
                {
                    XElement el1 = AsProcessedXElementLevel(btns);
                    xitem.Add(el1);
                }
            }
            else if (vusefield != null)
            {
                Contract.Assume(xitem_native.Name == EName.usefield);
                VField vfield = vusefield.Field();
                xitem = new XElement(vfield);
                Contract.Assume(xitem.Name == EName.field);
                xitem.RemoveAttribute(_AName.timestamp);
                xitem.RemoveAttribute(_AName.file);
                // перетираем id
                xitem.SetAttrValue(_AName.id, xitem_native.BaseElementOrSelf().GetUniqueKey().ToString());
                xitem.CopyAttributes(xitem_native.Attributes());
                //xitem.SetAttributeValue(_AName.src_field, xitem.GetAttributeValue(_AName.field.LocalName));
                xitem.RemoveAttribute(_AName.field);

                foreach (XElement el in xitem_native.Elements())
                {
                    XName name = el.Name;
                    if (name == EName.listquery || name == EName.defaultquery)
                    {
                        xitem.RemoveElement(name);
                    }
                    xitem.Add(new XElement(el));
                }
            }
            else if (vuseform != null)
            {
                xitem = new XElement(xitem_native.Name);
                xitem.CopyAttributes(xitem_native.Attributes());
                xitem.Add(vuseform.Params());
                xitem.SetAttributeValue(_AName.call, vuseform.ActionOrSelf().P_Form);
                xitem.SetAttributeValue(_AName.id, xitem_native.BaseElementOrSelf().GetUniqueKey());
                foreach (var layout_option in TextConst.ANameArray.AllLayoutOptions)
                {
                    xitem.SetAttributeValue(layout_option, xitem_native.AttrOrDefault(layout_option, null));
                }
            }
            else
            {
                xitem = new XElement(xitem_native.Name);
                xitem.CopyAttributes(xitem_native.Attributes());
                if (xitem_native is VConst)
                {
                    xitem.Value = xitem_native.Value;
                }
                else
                {
                    var elements = xitem_native.GetElementsP();
                    foreach (VSXElement element1 in elements)
                    {
                        XElement el1 = AsProcessedXElementLevel(element1);
                        xitem.Add(el1);
                    }
                }
            }
            // vuicommand
            if (vuicommand != null)
            {
                VSXElement btnt = vuicommand.ButtonType();
                if (btnt != null)
                {
                    Cmn.CopyAttributesNoReplace(btnt, xitem);
                }
                xitem.SetAttributeValue(_AName.title, xitem_native.P_Title);
                //var tt = vuicommand.Elements(TextConst.EName.Text);
                //if (tt.Any())
                //{
                //     xitem.Elements(TextConst.EName.Text).Remove();
                //     xitem.Add(tt);
                //}
                //el.SetAttributeValue(TextConst.AName.UpdateTarget, element.P_UpdateTargetS);
            }
            xitem.SetAttributeValue(_AName.id, xitem_native.BaseElementOrSelf().GetUniqueKey());
            XAttribute frmt = xitem.Attribute(_AName.format);
            if (frmt != null)
            {
                xitem.SetAttributeValue(_AName.edit_mask, frmt.Value);
            }
            return xitem;
        }
        public static string ReadElementAsString(XElement parent, XName name)
        {
            Contract.Assert(parent != null);
            XElement e = parent.Element(name);
            if (e == null)
            {
                return null;
            }
            else
            {
                return e.Value;
            }
        }
        private static XElement ReadAttrAsElem(XElement element, XName name)
        {
            //if (element.Element(name) == null) {
            //    return null;
            //} else {
            //    return element.Elements(name).Elements().FirstOrDefault();
            //}
            return element.Elements(name).Elements().FirstOrDefault();
        }
    }
}
