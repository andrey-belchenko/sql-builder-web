using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Devart.Data.Oracle;
using sql.builder.Core;
using sql.builder.XmlHelpers;
using AName_ = sql.builder.DataApi.AName;
//using sql.builder.WebReports;

namespace sql.builder.DataApi
{
    internal sealed partial class VReport : VSourcedElement, IVParent
    {
        internal VReport()
            : base(EName.report)
        {
        }
        internal bool Complicated = false;
        internal bool Pivot = false;
        private XElement scheme = null;
        internal VXElement GetSchemeWithColumnsPreset()
        {
            VXElement sch = new VSXElement(this.Scheme);
            foreach (XElement cps in this.Descendants(EName.columnspreset)) {
                string tableName = cps.Parent.Attribute(AName_.@as).Value;
                XElement tbl = sch.Descendants(EName.table).First(e => e.Attribute(AName_.@as).Value == tableName);
                XElement oldViewColumns = tbl.Element(EName.viewcolumns);
                XElement newViewColumns = new XElement(EName.viewcolumns);
                newViewColumns.Add(cps.Elements());
                foreach (XElement newCol in newViewColumns.Descendants(EName.column)) {
                    XElement oldCol = oldViewColumns.Descendants(EName.column).First(e => e.Attribute(AName_.name).Value == newCol.Attribute(AName_.name).Value);
                    oldCol.RemoveAttribute(AName_.title);
                    newCol.CopyAttributes(oldCol.Attributes());
                }
                XmlReports.CorrectScheme(newViewColumns, oldViewColumns);
                oldViewColumns.ReplaceWith(newViewColumns);
            }
            return sch;
        }
        internal XElement Scheme {
            get {
                if (this.scheme == null) {
                    scheme = Result(2, true).Scheme;
                }
                return this.scheme;
            }
            set {
                scheme = value;
            }
        }
        private VDataSet nonParamResultCash;
        //private bool Autobands;
        private bool IsOld = false;
        internal VReport(VEnvironment enviroment, string name)
            : base(EName.report)
        {
            XElement report = getReportOrQuery(name, enviroment.Manager.GetScheme());
            if (enviroment.Manager.IsOldOnly()) {
                this.IsOld = true;
            } else if (report == null) {
                report = getReportOrQuery(name, XmlReports.Environment.Manager.GetOldScheme());
                this.IsOld = true;
            }
            Compiler.CutIdentifiersTo30(report);
            Compiler.copyAttributes(report, this);
            Compiler.copyContent(report, this);
            //this.Autobands = this.AttrOrDefault("autobands", false);
            //EditColumns = (Cmn.GetAttrValue(this, "edit-columns") == "1");
            //AllowSave = (Cmn.GetAttrValue(this, "allow-save") == "1");
            //ParamsCustomization = (Cmn.GetAttrValue(this, "params-customization") == "1");
            //this.Mode = this.AttrOrEmpty(AName_.mode);
            alnalizeParams();
        }
        internal VReport(VEnvironment enviroment, XElement element)
            : base(EName.report)
        {
            XElement report = getReportOrQuery(element);
            //this.enviromentScheme = environment.Scheme;
            Compiler.CutIdentifiersTo30(report);
            Compiler.copyAttributes(report, this);
            Compiler.copyContent(report, this);
            //this.Autobands = this.AttrOrDefault("autobands", false);
            //EditColumns = (Cmn.GetAttrValue(this, "edit-columns") == "1");
            //AllowSave = (Cmn.GetAttrValue(this, "allow-save") == "1");
            //ParamsCustomization = (Cmn.GetAttrValue(this, "params-customization") == "1");
            //this.Mode = this.AttrOrEmpty(AName_.mode);
            alnalizeParams();
        }
        private static XElement reportFromQuery(XElement query)
        {
            Contract.Assert(query != null);
            string name = query.AttrOrEmpty(AName_.name);
            XElement report = new XElement(EName.report, new XAttribute(AName_.name, name));
            XElement qryCall = null;
            if (name != string.Empty && query.AttrOrEmpty("noname") != "1") {
                qryCall = new XElement(EName.query, new XAttribute(AName_.name, name));
                if (query.Element(EName.columns) != null) {
                    qryCall.Add(query.Elements(EName.columns));
                }
                //if (query.Elements(TextConst.EName.ViewColumns).Any()) //TextConst.EName.Columns  TextConst.EName.ViewColumns TextConst.EName.ColumnsPreset вроде все похоже по смыслу навести порядок
                //{
                //    qryCall.Add(new XElement(TextConst.EName.Columns));
                //    qryCall.Element(TextConst.EName.Columns).Add(query.Elements(TextConst.EName.ViewColumns).Elements());
                //}
                //if (qryCall.Elements(TextConst.EName.Columns).Any())
                //{
                //    foreach (XElement col in qryCall.Elements(TextConst.EName.Columns).Descendants(TextConst.EName.Column))
                //    {
                //        col.SetAttributeValue(TextConst.AName.Table, "a");
                //    }
                //}
                if (query.Element(EName.columnspreset) != null) {
                    qryCall.Add(query.Elements(EName.columnspreset));
                    foreach (XElement col in qryCall.Elements(EName.columnspreset).Descendants(EName.column)) {
                        col.SetAttributeValue(AName_.table, "a");
                    }
                }
            } else {
                if (name == string.Empty) {
                    name = "a";
                }
                qryCall = query;
                qryCall.SetAttrValue(AName_.name, name);
            }
            qryCall.SetAttributeValue(AName_.@as, "a");
            qryCall.CopyAttributes(query.Attributes(TextConst.AName.ParentNodeId));
            qryCall.CopyAttributes(query.Attributes(AName_.update_target));
            if (qryCall.Element(EName.columns) != null) {
                foreach (XElement col in qryCall.Elements(EName.columns).Descendants(EName.column)) {
                    col.SetAttributeValue(AName_.table, "a");
                }
            }
            report.Add(new XElement(EName.queries, qryCall));
            XElement query_params = query.Element(EName.@params);
            if (query_params != null) {
                report.Add(new XElement(query_params));
                XElement wpr = new XElement(EName.withparams);
                qryCall.Add(wpr);
                foreach (XElement el in query_params.Elements()) {
                    wpr.Add(new XElement(EName.useparam, new XAttribute(AName_.name, el.Attribute(AName_.name).Value)));
                }
            }
            if (query.Element(EName.events) != null) {
                qryCall.Add(query.Elements(EName.events));
                foreach (XElement col in query.Elements(EName.events).Descendants(EName.column)) {
                    col.SetAttributeValue(AName_.table, "a");
                }
            }
            if (query.Element(EName.menu) != null) {
                qryCall.Add(query.Elements(EName.menu));
                foreach (XElement col in query.Elements(EName.menu).Descendants(EName.column)) {
                    col.SetAttributeValue(AName_.table, "a");
                }
            }
            report.CopyAttributes(query.Attributes());
            report.RemoveAttribute(AName_.update_target);
            report.Add(query.Elements(EName.print_templates));
            report.Add(query.Elements(EName.procedure));
            return report;
        }
        internal bool IsSimpleParams = false;
        private static string[] simpleParamsTypes = new string[] { TextConst.AVDataType.Number, TextConst.AVDataType.Date, TextConst.AVDataType.String, TextConst.AVDataType.Array };
        private void alnalizeParams()
        {
            if (this.Elements(EName.@params).Elements(EName.param).All(e => simpleParamsTypes.Contains(e.AttrOrEmpty(AName_.type)))) {
                this.IsSimpleParams = true;
            }
            if (this.P_SaveCompiled == TextConst.AVBool.True) {
                this.IsSimpleParams = true;
            }
            if (this.IsSimpleParams) {
                PreprocessSimpleParams(this);
            }
        }
        internal static void PreprocessSimpleParams(XElement element)
        {
            foreach (XElement param in element.Elements(EName.@params).Elements(EName.param).ToList()) {
                param.Elements().Where(e => !EPredicate.IsAnyLink(e)).Remove();
                param.Add(Factory.NewConst(":" + param.Attribute(AName_.name).Value + " "));
            }
        }
        internal static XElement getReportOrQuery(string name, IEnumerable<VSXElement> scheme)
        {

            
            XElement report = scheme.Elements(EName.reports).Elements(EName.report).SearchByAttribute(AName_.name, name);
            if (report == null) {
                XElement query = null;
                //if (WebReportsAdapter.IsWebItem(name))
                //{
                //    query = WebReportsAdapter.GetQueryXml(name);
                //}
                //else
                //{
                //    query = scheme.Elements(EName.queries).Elements(EName.query).SearchByAttribute(AName_.name, name);
                //}
                query = scheme.Elements(EName.queries).Elements(EName.query).SearchByAttribute(AName_.name, name);

                if (query != null) {
                    report = reportFromQuery(query);
                }
            }
            return report;
        }
        internal static XElement getReportOrQuery(XElement element)
        {
            if (element.Name == EName.report) {
                return element;
            } else {
                return reportFromQuery(element);
            }
        }
        //public List<VSXElement> enviromentScheme;
        private void applyParams(XElement pars)
        {
            if (pars != null) {
                XElement formalPars = this.Element(EName.@params);
                if (!IsSimpleParams) {
                    if (pars != null) {
                        pars = new XElement(pars);
                        foreach (XElement par in pars.Elements()) {
                            XElement arrcall = par.Elements(EName.call).FirstOrDefault(e => e.AttrOrEmpty(AName_.function) == TextConst.AVFunction.Array);
                            if (arrcall != null) {
                                object[] arrVal = VDataSet.ArrayParamXElementContentToObjectArray(arrcall);
                                ArrayStorage arrayStorage = new ArrayStorage(par.Attribute(AName_.name).Value);
                                arrayStorage.SetValues(arrVal); // может выполнить запись в базу, не уверен что, это правильно делать тут.
                                par.Elements().Remove();
                                par.Add(Factory.NewConst(arrayStorage.GetSql()));
                            }
                        }
                    }
                    bool useDefaults;
                    if (formalPars != null) {
                        useDefaults = formalPars.AttrOrDefault("use-defaults", false);
                    } else {
                        useDefaults = false;
                    }
                    Cmn.setParams(formalPars, pars, useDefaults);
                } else {
                    if (formalPars != null) {
                        PreprocessSimpleParams(this);
                        string[] factParsNames = pars.Elements().Attributes(AName_.name).Select(APredicate.AttributeValue).Distinct().ToArray();
                        foreach (XElement par in formalPars.Elements().Where(e => !factParsNames.Contains(e.Attribute(AName_.name).Value)).ToList()) {
                            par.Elements().Remove();
                            par.Add(new XElement(EName.undefined));
                        }
                    }
                }
            } else {
                foreach (XElement formalParam in this.Elements(EName.@params).Elements()) {
                    if (!formalParam.HasElements) {
                        formalParam.Add(new XElement(EName.undefined));
                    }
                }
            }
        }
        internal static void ApplySimpleParams(XElement pars, VDataSet dataSet, XElement formalParams)
        {
            if (formalParams == null) {
                return;
            }
            //dataSet.InputParams = new SortedList<string, OracleParameter>();\
            dataSet.ClearInputParams();
            int i = 0;
            // this.Elements("params")
            foreach (XElement par in formalParams.Elements()) {
                string par_name = par.Attribute(AName_.name).Value;
                string par_mode = par.AttrOrDefault(AName_.mode, TextConst.AVArrayParamModes.Auto);
                OracleParameter dbPar = new OracleParameter();
                dbPar.ParameterName = par_name;
                string parType = par.AttrOrEmpty(AName_.type);
                if (string.IsNullOrEmpty(parType) && par.Attribute(AName_.class_type) != null) {
                    parType = TextConst.AVDataType.Number;
                }
                dbPar.OracleDbType = Cmn.GetDBType(parType);
                object val1 = null;
                object val = null;
                if (pars != null) {
                    XElement factParam = pars.Elements().SearchByAttribute(AName_.name, par_name);
                    if (factParam == null || factParam.Value == Cmn.undefinedString) {
                        val = Cmn.undefinedString;
                        if (parType == TextConst.AVDataType.Array && par_mode == TextConst.AVArrayParamModes.Store) {
                            ArrayStorage.ClearStoredValues(par_name);
                        }
                    } else {
                        if (parType == TextConst.AVDataType.Array) {
                            object[] arrVal = VDataSet.ArrayParamXElementContentToObjectArray(factParam.Elements().First());
                            ArrayStorage arrayStorage = new ArrayStorage(par_name);
                            arrayStorage.SetValues(arrVal, par_mode);
                            val1 = arrVal;
                            val = arrayStorage.GetSql();
                            //// сохранение значений в бд
                            //if (factParam.AttrOrDef(TextConst.AName.StoreInDB, "0") == "1")
                            //{
                            //    var arrayStorage = new ArrayStorage(par.Attribute("name").Value);
                            //    //arrayStorage.Clear();
                            //    object[] vals = factParam.Descendants("const").Select(e => (object)decimal.Parse(e.Value)).ToArray();
                            //    arrayStorage.SetValues(vals);
                            //    val = arrayStorage.GetSql();
                            //}
                            //else
                            //{
                            //    val = "(" + string.Join(",", factParam.Descendants("const").Select(e => e.Value)) + ")";   
                            //}
                        } else if (dbPar.DbType == DbType.Decimal) {
                            val = Cmn.ToDecimal(factParam.Value);
                        } else {
                            val = factParam.Value;
                        }
                    }
                    dbPar.Value = val;
                }
                if (val1 == null) {
                    val1 = val;
                }
                dataSet.AddInputParam(par.Attribute(AName_.name).Value, dbPar,val1);
                i++;
            }
        }
        public XElement Compile(int useRepository, bool noPivot, XElement pars)
        {
            XElement compiled = this.Elements(TextConst.EName.Compiled).Elements().FirstOrDefault();
            string cid = TextConst.EName.Report + "." + this.P_IdName;
            bool saveCache = false;
            bool cacheSaving = false;
            if (compiled == null) {
                if (this.P_SaveCompiled == TextConst.AVBool.True) {
                    cacheSaving = true;
                    compiled = Cache.GetQueryInfoFromCache(cid, DateTime.MaxValue, false);
                    if (compiled == null) {
                        saveCache = true;
                    }
                }
            }
            bool doFinalProcess = false;
            //var cacheSaved = false;
            if (compiled == null) {
                bool noPivot1 = noPivot;
                if (saveCache) {
                    noPivot1 = true;
                }
                doFinalProcess = true;
               // var scheme = (IsOld) ? XmlReports.Environment.Manager.GetOldScheme() : XmlReports.Environment.Manager.GetScheme();
                XmlReports.Environment.Manager.PushOldOnly(IsOld);
                compiled = Compiler.compileReport(this, useRepository, noPivot1,pars);
                XmlReports.Environment.Manager.PopOldOnly();
                IList<XElement> queries = compiled.Elements(EName.query).Where(e => e.AttrOrEmpty(AName_.materialize) == "1").ToList();
                if (queries.Count == 1 && this.AttrOrEmpty(AName_.materialize) != "1" && this.P_UseTemp != TextConst.AVBool.True) {
                    compiled.Elements(EName.query).Where(e => e.AttrOrEmpty(AName_.materialize) != "1").Remove();
                    queries[0].RemoveAttribute(AName_.materialize);
                }
            }
            bool isEditColumns = (this.AttrOrEmpty(AName_.edit_columns) != string.Empty); // может быть 2 для colsets;
            bool hasPivots = compiled.Descendants(EName.pivot).Any();
            var formParamsQuery = compiled.Descendants(EName.query).Where(e => e.AttrOrEmpty(AName_.name).StartsWith("form:")).ToList();
            if (cacheSaving && (hasPivots || isEditColumns || formParamsQuery.Count != 0)) {
                XElement xpivotQueies = null;
                if (saveCache) {
                    if (hasPivots) {
                        xpivotQueies = new XElement("pivot-queries");
                        foreach (var q in Compiler.pivotQueries) {
                            var xq = q.Value;
                            xq.Elements(EName.@params).Remove();
                            xq.AddFirst(Elements(EName.@params).First());
                            xq.SetAttributeValue(AName_.dimname, q.Key);
                            xpivotQueies.Add(xq);
                        }
                        compiled.Add(xpivotQueies);
                    }
                    Cache.SaveQueryInfoToCache(compiled, cid);
                    saveCache = false;
                }
                if (hasPivots) {
                    xpivotQueies = compiled.Element("pivot-queries");
                    xpivotQueies.Remove();
                }
                if (isEditColumns) {
                    var qnames = this.Element(EName.queries).Descendants(EName.query).Attributes(AName_.name).Select(APredicate.AttributeValue).Distinct().ToArray();
                    compiled.Descendants().Attributes(AName_.used).Remove();
                    foreach (XElement query in compiled.Elements(EName.query)) {
                        if (qnames.Contains(query.Attribute(AName_.name).Value)) {
                            Compiler.setQueryUsed(query, this, compiled);
                        }
                    }
                    Compiler.MarkUnused(compiled);
                    Compiler.DeleteUnused(compiled);
                }
                if (formParamsQuery.Count != 0 && pars != null) {
                    XElement fpq = null;
                    if (formParamsQuery.Count > 1) {
                        fpq = formParamsQuery.First(e => e.AttrOrEmpty(AName_.materialize) == TextConst.AVBool.True);
                    } else {
                        fpq = formParamsQuery[0];
                    }
                    XElement newFpq = Compiler.compileQuery(fpq.Attribute(AName_.name).Value, XmlReports.Environment.Manager.GetScheme()).Elements().First();
                    newFpq.RemoveAttributes();
                    Cmn.copyAttributes(fpq, newFpq);
                    foreach (XElement col in fpq.Elements(EName.select).Elements().Where(e => e.AttrOrEmpty(AName_.into) != string.Empty).ToList()) {
                        XElement newCol = newFpq.Elements(EName.select).Elements().First(e => e.AttrOrEmpty(AName_.@as) == col.AttrOrEmpty(AName_.@as));
                        newCol.SetAttributeValue(AName_.into, col.Attribute(AName_.into));
                    }
                    fpq.ReplaceWith(newFpq);
                }
                if (!noPivot) {
                    if (hasPivots) {
                        Compiler.pivotQueries = new SortedList<string, XElement>();
                        foreach (XElement xq in xpivotQueies.Elements()) {
                            Compiler.pivotQueries.Add(xq.Attribute(AName_.dimname).Value, xq);
                        }
                        Compiler.isProcessingPivots = true;
                        Compiler.processingPivots(compiled, pars);
                        Compiler.isProcessingPivots = false;
                    }
                }
                doFinalProcess = true;
            }
            if (doFinalProcess) {
                Compiler.processingMaterializedByHint(compiled);
                Compiler.CutIdentifiersTo30(compiled);
                if (this.AttrOrEmpty(AName_.ins_by_loop) == TextConst.AVBool.True) {
                    foreach (XElement ins in compiled.Elements(EName.query)) {
                        ins.SetAttributeValue(AName_.ins_by_loop, TextConst.AVBool.True);
                    }
                }
                XmlReports.finalProcessing(compiled);
            }
            if (saveCache) {
                Cache.SaveQueryInfoToCache(compiled, cid);
            }
            foreach (var sel in compiled.Descendants(EName.select)) {
                var aliaces = new HashSet<string>();
                foreach (var el in sel.Elements()) {
                    XAttribute attr = el.Attribute(AName_.@as);
                    if (attr != null) {
                        string alias = attr.Value;
                        //if (aliaces.Contains(alias)) {
                        //}
                        aliaces.Add(alias);
                    }
                }
            }
            return compiled;
        }
        internal VDataSet Result(int useRepository, bool useCash)
        {
            if (nonParamResultCash == null || useCash == false) {
                nonParamResultCash = result(null, useRepository, useCash);
            }
            return nonParamResultCash;
        }
        internal VDataSet Result(XElement pars, int useRepository, VDataSet dataSet = null, bool noPivot = true, VXElement schemePreset = null)
        {
            applyPreset(schemePreset);
            applyParams(pars);
            VDataSet res = result(pars, useRepository, false, dataSet, noPivot,schemePreset);
            cancelPreset();
            if (schemePreset != null) {
                res.SchemePreset = schemePreset;
            }
            return res;
        }
        private XElement original = null;
        private void applyPreset(VXElement schemePreset)
        {
            if (this.AttrOrEmpty(AName_.edit_columns) == string.Empty) {
                return;
            }
            if (schemePreset == null) {
                return;
            }
            original = new XElement(this);
            foreach (XElement query in this.Descendants(EName.query).Where(e => e.Parent.Name == EName.queries || e.Parent.Name == EName.query).ToList()) {
                XElement qpeset = new XElement(schemePreset.Descendants(EName.table).SearchByAttribute(AName_.@as, query.Attribute(AName_.@as).Value));
                foreach (XElement band in qpeset.Elements(EName.viewcolumns).Descendants(EName.band).Where(e => e.Attribute(AName_.name) != null).ToList()) {
                    XElement col = new XElement(EName.column, band.Attributes());
                    band.ReplaceWith(col);
                }
                XElement xcolumns = query.Element(EName.columns);
                if (xcolumns == null) {
                    xcolumns = new XElement(EName.columns);
                    query.Add(xcolumns);
                } else {
                    xcolumns.Elements().Remove();
                }
                qpeset.Element(EName.viewcolumns).Elements().Attributes(AName_.agg).Remove();
                xcolumns.Add(qpeset.Element(EName.viewcolumns).Elements());
                var grp = qpeset.Elements(EName.grouping).ToList();
                if (grp.Count != 0) {
                    query.Elements(EName.grouping).Remove();
                    query.Add(grp);
                }               
            }
        }
        private void cancelPreset()
        {
            if (this.AttrOrEmpty(AName_.edit_columns) != "1") {
                return;
            }
            if (original == null) {
                return;
            }
            IList<XElement> tqs = this.Descendants(EName.query).Where(EPredicate.IsChildOfReportOrQuery).ToList();
            IList<XElement> oqs = original.Descendants(EName.query).Where(EPredicate.IsChildOfReportOrQuery).ToList();
            foreach (XElement query in tqs) {
                XElement oquery = oqs.SearchByAttribute(AName_.@as, query.Attribute(AName_.@as).Value);
                query.Elements(EName.columns).Remove();
                if (oquery.Elements(EName.columns) != null) { // странное условие...
                    query.Add(oquery.Elements(EName.columns));
                }
            }
        }
        private VDataSet result(XElement pars, int useRepository, bool useCash/*только схема*/, VDataSet inDataSet = null, bool noPivot = true,VXElement schemePreset = null)
        {
            if (this.Descendants("transpose").Any() || this.Descendants(EName.query).Where(e => e.AttrOrEmpty("union") == "1" && !e.Ancestors(EName.from).Any()).Any()
               // && this.Attribute("name").Value != ""// !!! Заплатка, признак Complicated присваивался для атоматически созданного отчета, содержащего query с union
                ) {
                Complicated = true;
                useCash = false;
            }
            // XmlDocument compiled1 = null;//Compiler.old_compile ? (XmlDocument)Compile(noPivot) : null;

            XElement compiled = null;
            bool cashExists = false;
            string cashPath = null;
            XElement cashElement = null;
            string projCacheName = this.P_IdName + "-scheme";
            if (useCash) {
                cashElement = Cache.GetQueryInfoFromCache(projCacheName, DateTime.MaxValue,true); 
                //теперь схема кешируется в проекте, но не для всех отчетов кеш уже есть, поэтому старый вариант тоже остается
                if (cashElement == null) {
                    var d1 = XmlReports.SchemeChangeTime();
                    cashPath = Cmn.GetCashDirectoryName() + "\\" + this.P_IdName;
                    if (File.Exists(cashPath)) {
                        var fi = new FileInfo(cashPath);
                        if (fi.LastWriteTime > XmlReports.SchemeChangeTime()) {
                            cashElement = XDocument.Load(cashPath).Root;
                            if (cashElement.AttrOrEmpty("scheme-timestamp") == XmlReports.Environment.Manager.GetNativeScheme().First().AttrOrEmpty(AName_.timestamp)) {
                                cashExists = true;
                            }
                        }
                    }
                } else {
                    cashExists = true;
                }
            }
            XElement scheme = null;
            VReport fetchSrcRep = null;
            VDataSet fetchSrcDS = null;
            if (cashExists) {
                //  compiled = cashDoc.Root.Elements().First();
                //  scheme = new VXElement(cashDoc.Root.Elements().Last(), this.environment);
                scheme = new VXElement(cashElement.Elements().First());
                XAttribute fetchSrcAttr = scheme.Descendants(EName.table).Elements(EName.select).Attributes(AName_.call).FirstOrDefault();
                if (fetchSrcAttr != null) {
                    fetchSrcRep = XmlReports.Environment.GetPrecompiledReport(fetchSrcAttr.Value);
                }
                if (fetchSrcRep != null) {
                    //fetchSrcDS = fetchSrcRep.Result(null, useRepository, null, true, schemePreset);
                    fetchSrcDS = fetchSrcRep.Result(useRepository, useCash);
                    XElement fetchSrcScheme = fetchSrcDS.Scheme;
                    string qName = fetchSrcAttr.Parent.Parent.Attribute(AName_.name).Value;
                    XElement xtbl = scheme.Descendants(EName.table).First(e => e.AttrOrEmpty(AName_.name) == qName);
                    xtbl.Elements().Remove();
                    xtbl.Add(fetchSrcScheme.Descendants(EName.table).First().Elements());
                }
            } else {
                compiled = (XElement)Compile(useRepository, noPivot, pars); //Compiler.old_compile ? XDocument.Parse(compiled1.OuterXml).Root : (XElement)Compile(noPivot);
                scheme = Compiler.MakeResultScheme(this, compiled);
                XAttribute fetchSrcAttr = compiled.Elements(EName.query).Elements(EName.select).Attributes(AName_.call).FirstOrDefault();
                if (fetchSrcAttr != null) { /// повтор. вынести
                    fetchSrcRep = XmlReports.Environment.GetPrecompiledReport(fetchSrcAttr.Value);
                }
                if (fetchSrcRep != null) {
                    //fetchSrcDS = fetchSrcRep.Result(null, useRepository, null, true, schemePreset);
                    fetchSrcDS = fetchSrcRep.Result(useRepository, useCash);
                    XElement fetchSrcScheme = fetchSrcDS.Scheme;
                    string qName = fetchSrcAttr.Parent.Parent.Attribute(AName_.name).Value;
                    XElement xtbl = scheme.Descendants(EName.table).First(e => e.AttrOrEmpty(AName_.name) == qName);
                    xtbl.SetAttributeValue(AName_.call, fetchSrcAttr.Value);
                    xtbl.Elements().Remove();
                    xtbl.Add(fetchSrcScheme.Descendants(EName.table).First().Elements());
                }
                if (useCash) {
                    var xroot = new XElement(EName.root);
                    xroot.Add(scheme);
                    if (XmlReports.IsDeveloperMode()) {
                        Cache.SaveQueryInfoToCache(xroot, projCacheName);
                    } else {
                        var cashDoc = new XDocument();
                        cashDoc.Add(xroot);
                        // xroot.Add(compiled);
                        cashDoc.Root.SetAttributeValue("scheme-timestamp", Cmn.GetAttrValue(XmlReports.Environment.Manager.GetNativeScheme().First(), "timestamp"));
                        cashDoc.Save(cashPath);
                    }
                }
            }
            //"transpose" и "pivot" по смыслу однsо и тоже:"transpose"-старый вариант настраивается для report, выполняется на клиенте, "pivot"-выполняется  в процессе компиляции запроса
            if (!cashExists) {
                if (compiled.Descendants(EName.pivot).Any()) {
                    this.Pivot = true;
                }
            }
            scheme.SetAttributeValue(AName_.mode, this.P_ViewMode);
            scheme.SetAttributeValue(AName_.params_customization, this.P_ParamsCustomization);
            VDataSet dataSet = VDataSet.FromXml(new XElement(EName.@params, scheme), inDataSet);
            dataSet.Scheme = scheme;
            dataSet.addBandsForTransposedPre();
            if (fetchSrcDS != null) {
                var tbl = dataSet.GetAllTables().First();
                tbl.DataSetForFetch = fetchSrcDS;
                fetchSrcDS.GetAllTables().First().UseDeferredFetch = true;
            }
            if (this.IsSimpleParams) {
                ApplySimpleParams(pars, dataSet, this.Element(EName.@params));
            }
            if (!cashExists) {
                if (Complicated) {
                    dataSet.CompiledReport = new XmlDocument();
                    dataSet.CompiledReport.LoadXml(compiled.ToString());
                }
            }
            dataSet.Report = this;
            if (!cashExists) {
                if (P_UseTemp == TextConst.AVBool.True || compiled.Elements(EName.query).Any(e => e.AttrOrEmpty(AName_.materialize) == "1")) {
                    dataSet.UseTempTable = true;
                    if (P_UseTemp == TextConst.AVBool.True) {
                        dataSet.UpdateTempTable = true;
                    }
                } else {
                    dataSet.UseTempTable = false;
                }
                compiled.Descendants().Where(e => e.AttrOrEmpty(AName_.client_calc) == TextConst.AVBool.True).Remove();
                if (dataSet.UseTempTable) {
                    dataSet.MatQueriesNames = compiled.Elements(EName.query)
                        .Where(e => e.AttrOrEmpty(AName_.materialize) == TextConst.AVBool.True && e.AttrOrEmpty(TextConst.AName.IsDone) != TextConst.AVBool.True)
                        .Select(e1 => e1.Attribute(AName_.name).Value).ToList();
                    dataSet.ProcedureText = XmlReports.getProcedureSqlOld(XmlReports.XElementToXmlNode(compiled).OwnerDocument);
                }
            }
            if (!Complicated) {
                foreach (VDataTable table in dataSet.Tables) {
                    if (this.Attribute(AName_.client_calc) != null) {
                        table.IsNonDb = true;
                    }
                    IEnumerable<XElement> keyCols = table.Scheme.Elements(EName.columns).Elements().Where(e => e.AttrOrEmpty(AName_.key) == TextConst.AVBool.True);
                    //table.Columns.AddRange(new VDataColumn[] {new VDataColumn("rn")});
                    if (keyCols.Count() != 1) {
                        keyCols = table.Scheme.Elements(EName.columns).Elements().Where(e => e.AttrOrEmpty(AName_.name) == TextConst.AVColumn.Sid);
                    }
                    DataColumn[] primaryKey = new DataColumn[keyCols.Count()];
                    int i = 0;
                    foreach (XElement el in keyCols) {
                        el.SetAttributeValue(AName_.key, TextConst.AVBool.True);
                        primaryKey[i] = table.Columns[el.Attribute(AName_.name).Value];
                        i++;
                    }
                    table.PrimaryKey = primaryKey;
                    foreach (DataColumn col in primaryKey) {
                        col.AllowDBNull = true;
                    }
                    if (!table.IsNonDb) {
                        if (!cashExists) {
                            XElement compiledQuery;
                            if (dataSet.UseTempTable) {
                                compiledQuery = compiled.Elements(EName.query).First(e => e.Attribute(AName_.name).Value == table.QueryName & e.AttrOrEmpty(AName_.materialize) == "1");
                            } else {
                                compiledQuery = compiled.Elements(EName.query).First(e => e.Attribute(AName_.name).Value == table.QueryName);
                            }
                            table.DataAdapter.SelectCommand.CommandText = getQuerySelectText(compiledQuery, dataSet.UseTempTable);
                        }
                    }
                    //XElement columns = null;
                    //if (Compiler.getAttrValue(this, "editable") == "1")
                    //{
                    //    columns = getEditableColumns(compiledQuery);
                    //}
                    //if (columns != null)
                    //{
                    //    table.EditableOld = true;
                    //    XElement keyColumn = columns.Elements().Where(e => Compiler.getAttrValue(e, "key") == "1").FirstOrDefault();
                    //    List<XElement> listColumns = columns.Elements().ToList();
                    //    string tableName = columns.Attribute("table").Value;
                    //    //table.DataAdapter.UpdateCommand = getUpdateCommand(listColumns, keyColumn, tableName);
                    //    //table.DataAdapter.InsertCommand = getInsertCommand(listColumns, keyColumn, tableName);
                    //    //table.DataAdapter.DeleteCommand = getDeleteCommand(keyColumn, tableName);
                    //    foreach (XElement col in columns.Elements())
                    //    {
                    //        VDataColumn dcol = (VDataColumn)table.Columns[col.Attribute("as").Value];
                    //        if (col.Attribute("reference") != null)
                    //        {
                    //            dcol.ReferenceName = col.Attribute("reference").Value;
                    //        }
                    //        if (col.Attribute("refcol") != null)
                    //        {
                    //            dcol.ReferenceColumnName = col.Attribute("refcol").Value;
                    //        }
                    //        dcol.EditableOld = true;
                    //        dcol.MakeAttributes();
                    //    }
                    //    table.EditableOld = true;
                    //}
                    //else
                    //{
                    //    table.EditableOld = false;
                    //}
                    table.EditableOld = false;
                }
            }
            return dataSet;
        }
        // getDeleteCommand, getUpdateCommand ,getInsertCommand - методы  д быть переписаны в VForm
        //, эти методы и вообще функционал в Vreport отвечающий за редатирование должны стать неактуальными
        // public OracleCommand getDeleteCommand( XElement keyColumn,string tableName)
        // {
        //     OracleCommand cmd = new OracleCommand();
        //     //XElement keyColumn = columns.Elements().Where(e => Compiler.getAttrValue(e, "key") == "1").FirstOrDefault();
        //     OracleParameter par = cmd.Parameters.Add(":" + keyColumn.Attribute("as").Value, GetDBType(keyColumn.Attribute("type").Value));
        //     par.SourceColumn = keyColumn.Attribute("as").Value;
        //     cmd.CommandText = string.Format("delete from  {0}  where {1}={2}", tableName, keyColumn.Attribute("sourcecolumn").Value, ":" + keyColumn.Attribute("as").Value);
        //     return cmd;
        // }
        // public OracleCommand getUpdateCommand(List<XElement> columns, XElement keyColumn, string tableName)
        // {
        //     OracleCommand cmd = new OracleCommand();
        //     string cols = "";
        ////     string pars = "";
        //     string q="";
        //     foreach (XElement col in columns)
        //     {
        //         cols += q + col.Attribute("sourcecolumn").Value+"=";
        //         string parName=":"+ col.Attribute("as").Value;
        //         cols += parName;
        //         q=",";
        //         OracleParameter par = cmd.Parameters.Add(parName, GetDBType(col.Attribute("type").Value));
        //         par.SourceColumn = col.Attribute("as").Value;
        //     }
        //   //  XElement keyColumn=columns.Elements().Where(e=>Compiler.getAttrValue(e,"key")=="1").FirstOrDefault();
        //     cmd.CommandText = string.Format("update {0} set {1} where {2}={3}", tableName, cols, keyColumn.Attribute("sourcecolumn").Value, ":" + keyColumn.Attribute("as").Value);
        //     return cmd;
        // }
        // public OracleCommand getInsertCommand(List<XElement> columns, XElement keyColumn, string tableName)
        // {
        //     OracleCommand cmd = new OracleCommand();
        //     string cols = "";
        //     string pars = "";
        //     string q = "";
        //     OracleParameter keyPar=null;
        //     //XElement keyColumn = columns.Elements().Where(e => Compiler.getAttrValue(e, "key") == "1").FirstOrDefault();
        //     foreach (XElement col in columns)
        //     {
        //         cols += q + col.Attribute("sourcecolumn").Value;
        //         string parName = ":" + col.Attribute("as").Value;
        //         pars += q+parName;
        //         q = ",";
        //         OracleParameter par = cmd.Parameters.Add(parName, GetDBType(col.Attribute("type").Value));
        //         par.SourceColumn = col.Attribute("as").Value;
        //         if (col.Attribute("as").Value == keyColumn.Attribute("as").Value)
        //         {
        //             keyPar = par;
        //         }
        //     }
        //     keyPar.Direction = ParameterDirection.InputOutput;
        //     cmd.CommandText = string.Format("insert into {0} ({1}) values ({2}) returning {3} into {4}", tableName, cols, pars, keyPar.SourceColumn, ":" + keyPar.ParameterName);
        //     return cmd;
        // }
        internal static string GetStringType(OracleDbType type)
        {
            switch (type) {
                case OracleDbType.Number:
                    return TextConst.AVDataType.Number;
                case OracleDbType.Date:
                    return TextConst.AVDataType.Date;
                case OracleDbType.Array:
                    return TextConst.AVDataType.Array;
                default:
                    return TextConst.AVDataType.String;
            }
        }
        /*public XElement getEditableColumns(XElement compiledQuery)
        {
            XElement keyColumn = compiledQuery.Elements("select").Elements("column").Where(e => Compiler.getAttrValue(e, "key") == "1" & Compiler.getAttrValue(e, "sourcetable") != "").FirstOrDefault();

            if (keyColumn != null)
            {

                XElement srcTable = compiledQuery.DescendantsAndSelf("query").Where(e => e.Attribute("path").Value == keyColumn.Attribute("sourcetable").Value).Elements("from").Elements("table").FirstOrDefault();
                if (srcTable != null)
                {
                    XElement columns = new XElement("columns", new XAttribute("table", srcTable.Attribute("name").Value));
                    foreach (XElement col in compiledQuery.Elements("select").Elements("column").Where(e => Compiler.getAttrValue(e, "sourcetable") == keyColumn.Attribute("sourcetable").Value))
                    {
                        columns.Add(new XElement(col));
                    }

                    return columns;
                }
                else
                {
                    return null;
                }
            }
            else
            {
                return null;
            }
        }*/
        private static string getQuerySelectText(XElement compiledQuery, bool fromTemp)
        {
            if (fromTemp) {
                compiledQuery = compiledQuery.Parent.Elements(EName.query).First(e => e.Attribute(AName_.name).Value == compiledQuery.Attribute(AName_.name).Value & e.AttrOrEmpty(AName_.materialize) != "1");
            } //else {
                //Манипуляции чтобы получить из текста процедуры select. Когда-нибудь переделать.
                //  compiledQuery.Elements("text").Remove();
                //  compiledQuery.Element("select").Element("text").Value = "select ";
                //  compiledQuery.SetAttributeValue("materialize", null);
                //compiledQuery.Elements("insert").Remove();
            //}
            string text;
            // text = Compiler.GetSql(Parse("<root>" + compiledQuery + "</root>"));
            XElement compiledQuery1 = new XElement(EName.root);
            compiledQuery1.Add(new XElement(compiledQuery));
            //compiledQuery1.Descendants().Where(e => Cmn.GetAttrValue(e, TextConst.AName.ClientCalulation) == TextConst.AVBool.True).Remove();
            text = Compiler.GetSql(compiledQuery1);
            if (fromTemp) {
                text += " order by rn";
            }
            /* if (!fromTemp)
            {
               /* if (compiledQuery.Attribute("order") != null)
                {
                    text += " order by " + compiledQuery.Attribute("order").Value;
                }
                text += ") mtr";
            }
              */
            return text;
        }
        /*internal static string[] ExtractParamsFromSqlText(string sqlExpr)
        {
            //Доделать, не будет работать при наличии ':' в строковых константах
            var pars = new List<string>();

            string pattern = ":[a-zA-Z0-9_]{1,}";
            string[] not_allowed_names = { ":mi", ":ss" };

            foreach (Match match in Regex.Matches(sqlExpr, pattern, RegexOptions.IgnoreCase))
            {
                if (not_allowed_names.Contains(match.Value.ToLower())) continue;

                if (match.Index < 4)
                {
                    pars.Add(match.Value.Replace(":", ""));   
                }
                else if (match.Index >= 4 && sqlExpr.Substring(match.Index - 4, 4) != "form")
                {
                    pars.Add(match.Value.Replace(":", ""));
                }   
            }
            return pars.Distinct().ToArray();
        }*/
    }
}