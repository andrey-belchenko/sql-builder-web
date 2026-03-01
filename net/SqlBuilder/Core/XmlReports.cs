using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading;
////using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
//using DevExpress.DashboardCommon.Native;
//using DevExpress.XtraEditors;

using infoenergo.core.Data;
//using infoenergo.core.Extensions;
//using infoenergo.framework.Extensions.Oracle;
using sql.builder.Clean;
using sql.builder.DataApi;
//using sql.builder.WebReports;
//using static System.Net.Mime.MediaTypeNames;
using SqlBuilderLib.DevTools;

namespace sql.builder
{
    public static partial class XmlReports
    {
        public static readonly Type numberType = typeof(Decimal);
        public static readonly char num_sep = CultureInfo.CurrentCulture.NumberFormat.CurrencyDecimalSeparator[0];
        public static readonly string key_name = "sid";
        public static readonly string parent_key_name = "sparentid";
        //public static readonly decimal kod_menu_default = 121001M;
        private static string pathXsltSys = (SourceFolder ?? CleanUtils.GetRootPath()) + "\\" + NativeProductName + "\\xslt\\";
        //public static string pathXsltSys = @"C:\infoenergo_root\root\main\all\sql.builder\sql.builder\xslt\";
        //private static ProgressBarControl progressBar = null;
        public static string schemeName = null;
        public static string customerId = null;
        //private static int compilerVersion = 2;
        public static readonly string SourceFolderName = "projects";
        public static readonly string FormsCacheFolderName = "FormsCache";
        public static readonly string QubesCacheFolderName = "QubesCache";
        public static readonly string QueriesCacheFolderName = "QueriesCache";
        public static XElement TestFile;
        public static string TestName;
        public static bool? TestCompare;
        private static VEnvironment _environment;

        /// <summary>
        /// Per-request environment for web/async context. When set, Environment getter returns this instead of _environment.
        /// </summary>
        internal static readonly AsyncLocal<VEnvironment> RequestEnvironment = new AsyncLocal<VEnvironment>();

        public static VEnvironment Environment
        {
            set
            {
                _environment = value;
            }
            get
            {
                if (RequestEnvironment.Value != null)
                    return RequestEnvironment.Value;
                if (_environment == null)
                {
                    Init();
                }
                return _environment;
            }
        }
        public static bool Init(bool force_reload = false, string source_folder = null)
        {
#if DEBUG
            Stopwatch sw;
            sw = new Stopwatch();
            sw.Restart();
#endif
            // The following line provides localization for data formats. 
            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("ru-RU");
            // The following line provides localization for the application's user interface. 
            System.Threading.Thread.CurrentThread.CurrentUICulture = new System.Globalization.CultureInfo("ru-RU");
            //db.ExecuteNonQuery("ALTER SESSION SET session_cached_cursors = 300");
            string customer_id, scheme;
            using (DataTable dt = db.ExecuteDataTable("select customer_id, scheme from rs_rep_sets", null, false))
            {
                DataRow row = dt.Rows[0];
                customer_id = row["customer_id"].ToString();
                scheme = row["scheme"].ToString();
            }
            ////if (IsDeveloperMode())
            ////{
            //    customer_id = "501";
            //    sql.builder.UI.UIStatic.IsMpep = true;
            //    sql.builder.Controls.ucMainReports.MainPanelName = "Главная";
            ////}
            //customer_id = "29";
            if (customer_id == "11")
            {
                //sql.builder.UI.UIStatic.IsMpep = true;
                //sql.builder.Controls.ucMainReports.MainPanelName = "Отчеты";

            }
            LoadXml(scheme, customer_id, InputParams, force_reload: force_reload, source_folder: source_folder);
            VCashUtils.ClearCash();
#if DEBUG
            sw.Stop();
            Debug.Write("XmlReports.Init(): ");
            if (force_reload)
            {
                Debug.Write("полная ");
            }
            // Debug.WriteLine("перезагрузка схемы за " + sw.ElapsedTicks.ToString() + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
#endif
            return true;
        }
        public static bool IsDeveloperMode()
        {
#if DEBUG
            // return false;
            return IsNative || HasDevelopRights;
#else
            return false;
#endif
        }
#if DEBUG
        public static bool UseProjectSourceFolder
        {
            get
            {
                return Directory.Exists(GetDefaultContentFolder());
            }
        }
#else
        public const bool UseProjectSourceFolder = false;
#endif
        public static bool IsInfoenergo
        {
            get { return false; }
        }
        public static bool IsNative
        {
            get
            {
                //return (NativeProductName == Application.ProductName);
                return true;
            }
        }
        private static bool HasDevelopRights
        {
            get
            {
                //(NetProjectsWithDevelopRights.Contains(Application.ProductName));
                return false;

            }
        }
        private static string[] NetProjectsWithDevelopRights =
        {
            "plan.economic.analytics"
        };
        /*public static void GenerateNavigators()
        {
            var env = Environment;

            // загрузка всех проектов
            foreach (var p in Environment.Manager.GetAllProjects()) p.LoadIfNeed();

            // Отчёты
            var xreports_new = Environment.Manager.GetScheme()
                .Elements(TextConst.EName.Reports)
                .Elements(TextConst.EName.Report)
                .Where(e => e.Attribute(TextConst.AName.Title) != null)
                .ToArray();

            var xreports_old = Environment.Manager.GetOldScheme()
                .Elements(TextConst.EName.Reports)
                .Elements(TextConst.EName.Report)
                .Where(e => e.Attribute(TextConst.AName.Title) != null)
                .ToArray();

            //var aa1 = xreports_old.Where(r => r.AttrOrDef("visible","1") != "0").Elements("customers").Elements("customer").Select(c => c.Attribute("id").Value).Distinct().ToArray();

            var xreports_from_queries = Environment.Manager.GetScheme()
                .Elements(TextConst.EName.Queries)
                .Elements()
                .Where(e => e.Attribute(TextConst.AName.Title) != null && Cmn.GetAttrValue(e, TextConst.AName.IsReport) == TextConst.AVBool.True)
                .ToArray();

            var xreports_from_forms = Environment.Manager.GetScheme()
                .Elements(TextConst.EName.Forms)
                .Elements()
                .Where(e => e.Attribute(TextConst.AName.Title) != null && Cmn.GetAttrValue(e, TextConst.AName.IsReport) == TextConst.AVBool.True)
                .ToArray();


            //var aa = string.Join(",", Compiler.SchemeRoot.Concat(Compiler.schemeRootOld).Descendants("customer").Select(c => c.AttrOrDef("id", null)).Distinct().OrderBy(a => a).ToArray());

            // Папки - берем строго из native, чтобы не обрезались notforcustomers
            var xfolders_root = Environment.Manager.GetNativeScheme().Elements(TextConst.EName.Folders).Elements(TextConst.EName.Folder).ToArray();

            var xnavigators = new XElement(TextConst.EName.Navigators);
            foreach (var xcustomer in Environment.Manager.GetScheme().Elements(TextConst.EName.Customers).Elements(TextConst.EName.Customer))
            {
                string cust_id = xcustomer.Attribute(TextConst.AName.Id).Value;
                var xnavigator = new XElement(TextConst.EName.Navigator, new XAttribute(TextConst.AName.Name, "nav" + cust_id), new XAttribute(TextConst.AName.Title, xcustomer.Attribute(TextConst.AName.Title).Value));

                var folders_dict = new Dictionary<string, XElement>();
                var queue = new Queue<XElement>();

                IEnumerable<XElement> xfolders = xfolders_root.Where(f => !f.AttrOrDef("notforcustomers", "").Split(',').Contains(cust_id) && f.AttrOrDef("forcustomers", cust_id).Split(',').Contains(cust_id));
                foreach (var f in xfolders) queue.Enqueue(new XElement(f));

                while (queue.Count > 0)
                {
                    XElement xfolder = queue.Dequeue();

                    var xcopy = new XElement("folder");
                    xcopy.SetAttributeValue("name", xfolder.GetAttributeValue("name"));
                    xcopy.SetAttributeValue("title", xfolder.GetAttributeValue("title"));
                    xcopy.SetAttributeValue("security-id", xfolder.GetAttributeValue("kod-menu"));

                    folders_dict.Add(xfolder.Attribute(TextConst.AName.Name).Value, xcopy);

                    xfolders = xfolder.Elements(TextConst.EName.Folder).Where(f => !f.AttrOrDef("notforcustomers", "").Split(',').Contains(cust_id) && f.AttrOrDef("forcustomers", cust_id).Split(',').Contains(cust_id));
                    foreach (var f in xfolders) queue.Enqueue(f);

                    var xparent = (xfolder.Parent != null) ? folders_dict[xfolder.Parent.Attribute(TextConst.AName.Name).Value] : xnavigator;
                    xparent.Add(xcopy);
                }

                var xitems = xreports_new.Concat(xreports_old).Concat(xreports_from_queries).Concat(xreports_from_forms).Where(r => r.Elements(TextConst.EName.Customers).Elements(TextConst.EName.Customer).Any(c => c.AttrOrDef(TextConst.AName.Id, null) == cust_id));
                foreach (var xitem in xitems)
                {
                    var xuse = new XElement((xitem.Name.LocalName == TextConst.EName.Form) ? TextConst.EName.UseForm : TextConst.EName.UseReport);
                    string proj = (xitem.Attribute(TextConst.AName.File).Value.StartsWith(@"\asuse1")) ? "asuse1" : "asuse2";
                    xuse.Add(
                        new XAttribute(TextConst.AName.Project, proj),
                        new XAttribute((xitem.Name.LocalName == TextConst.EName.Form) ? TextConst.EName.Form : TextConst.EName.Report, xitem.Attribute(TextConst.AName.Name).Value),
                        new XAttribute(TextConst.AName.Title, xitem.Attribute(TextConst.AName.Title).Value));

                    string vis = xitem.AttrOrDef(TextConst.AName.Visible, "1");
                    if (vis == "0")
                    {
                        xuse.Add(new XAttribute(TextConst.AName.Visible, vis));
                    }

                    string folder = xitem.AttrOrDef(TextConst.AName.Folder, null);
                    if (folder == null) xnavigator.Add(xuse);
                    else if (folders_dict.ContainsKey(folder)) folders_dict[folder].Add(xuse);
                }

                // сортируем по заголовку
                foreach (var xel in xnavigator.Descendants().ToArray())
                {
                    var sorted = xel.Elements().OrderBy(e => e.Attribute(TextConst.AName.Title).Value).ToArray();
                    sorted.Remove();
                    xel.Add(sorted);
                }

                xnavigators.Add(xnavigator);
            }

            // удаляем пустые папки без элементов
            xnavigators.Descendants(TextConst.AName.Folder).Where(f => !f.Descendants(TextConst.EName.UseReport).Any() && !f.Descendants(TextConst.EName.UseForm).Any()).Remove();
            //xnavigators.Descendants(TextConst.AName.Folder).Attributes("forcustomers").Remove();
            //xnavigators.Descendants(TextConst.AName.Folder).Attributes("notforcustomers").Remove();

            var xroot = new XElement("root");
            xroot.Add(xnavigators);

            var common = Environment.Manager.GetProject("common");
            xroot.Save(Path.Combine(common.ProjectPath, "navigators.xml"));

            Environment = null;
        }*/
        public static Dictionary<string, string> GetReportInfo(string repname)
        {
            return GetReportInfo(GetReport(repname));
        }
        public static Dictionary<string, string> GetReportInfo(XElement xreport)
        {
            var dict = new Dictionary<string, string>();
            string name = xreport.AttrOrEmpty(AName.name);
            dict.Add("repname", name);
            dict.Add("original_name", name);
            dict.Add("title", xreport.AttrOrEmpty(AName.title));
            dict.Add("form", xreport.AttrOrEmpty(AName.form));
            dict.Add("editable", xreport.AttrOrEmpty(AName.editable));
            dict.Add("folder", xreport.AttrOrEmpty(AName.folder));
            dict.Add("nogrid", xreport.AttrOrEmpty(AName.nogrid));
            dict.Add("item_type", "usereport");
            dict.Add("is_template", "0");
            dict.Add("visible", xreport.AttrOrDefault(AName.visible, "1"));
            dict.Add("kod_menu", xreport.AttrOrEmpty(TextConst.AName.KodMenu));
            dict.Add("changed", "0");
            dict.Add("kod_gs", null);
            dict.Add("data", null);
            dict.Add("old", false.ToString());
            if (dict["visible"].Equals("1"))
            {
                switch (dict["item_type"])
                {
                    case "folder": dict.Add("image_id", "0"); break;
                    case "usereport": dict.Add("image_id", "1"); break;
                    case "useform": dict.Add("image_id", "2"); break;
                }
            }
            else
            {
                dict.Add("image_id", "4");
            }
            return dict;
        }
        public static XElement GetReport(string name)
        {
            //if (WebReportsAdapter.IsWebItem(name))
            //{
            //    return WebReportsAdapter.GetReportXml(name);
            //}
            IList<VSXElement> scheme = Environment.Manager.GetScheme();
            XElement xreport = scheme.Elements(EName.reports).Elements(EName.report).SearchByAttribute(AName.name, name);
            if (xreport == null)
            {
                xreport = Environment.Manager.GetOldScheme().Elements(EName.reports).Elements(EName.report).SearchByAttribute(AName.name, name);
                if (xreport == null)
                {
                    xreport = scheme.Elements(EName.queries).Elements(EName.query).SearchByAttribute(AName.name, name);
                }
            }
            return xreport;
        }
        public static bool IsFormWithBehavior(string name, string repname)
        {
            if (XmlReports.Environment.Manager.IsOldOnly())
            {
                return false;
            }
            XElement element;
            if (string.IsNullOrEmpty(name))
            {
                element = XmlReports.Environment.GetReportOrQuery(repname);
            }
            else
            {
                element = XmlReports.Environment.Manager.GetScheme().Elements(EName.forms).Elements(EName.form).SearchByAttribute(AName.name, name);
            }
            if (element == null)
            {
                return true;
            }
            else
            {
                return element.AttrOrDefault(AName.with_behavior, true);
            }
        }
        public static XElement GetForm(string name, string repname)
        {
            XElement xform;

            // TODO: костыль для web. Пока не понял почему в оригинальном решении форма подтягивается не смотря на то что явно она не задана, а тут нет
            if (name == "empty")
            {
                name = repname;
            }
            if (string.IsNullOrEmpty(name))
            {
                if (XmlReports.Environment.Manager.IsOldOnly())
                {
                    xform = Environment.Manager.GetScheme().Elements(EName.forms).Elements(EName.form).SearchByAttribute(AName.name, repname);
                }
                else
                {
                    VSXElement element = XmlReports.Environment.GetReportOrQuery(repname);
                    xform = element.GetFormXElement();
                }
            }
            else
            {
                xform = Environment.Manager.GetScheme().Elements(EName.forms).Elements(EName.form).SearchByAttribute(AName.name, name);
            }
            return xform;
        }
        /*public static XmlDocument repair(string fromFileName, string toFolderName)
        {
            XmlDocument srcDoc = new XmlDocument();
            srcDoc.Load(fromFileName);
            repair(srcDoc, toFolderName);
            return srcDoc;
        }*/
        /*public static void repair(XmlDocument srcDoc, string toFolderName)
        {
            DirectoryInfo tagDir = new DirectoryInfo(toFolderName);
            repairFolder(srcDoc, tagDir);
        }*/
        /*private static void repairFolder(XmlDocument srcDoc, DirectoryInfo tagDir)
        {
            foreach (FileInfo file in tagDir.GetFiles("*.xml"))
            {
                XmlDocument tagDoc = new XmlDocument();
                tagDoc.Load(file.FullName);


                repairItems(srcDoc, tagDoc, "queries", "name");
                repairItems(srcDoc, tagDoc, "reports", "name");
                repairItems(srcDoc, tagDoc, "forms", "name");
                repairItems(srcDoc, tagDoc, "functions", "name");
                repairItems(srcDoc, tagDoc, "views", "name");
                repairItems(srcDoc, tagDoc, "parts", "id");
                tagDoc.Save(file.FullName);

            }

            foreach (DirectoryInfo dir in tagDir.GetDirectories())
            {
                repairFolder(srcDoc, tagDir);
            }
        }*/
        /*public static void repairItems(XmlDocument srcDoc, XmlDocument tagDoc, string itemType, string keyName)
        {
            foreach (XmlNode srcItem in srcDoc.SelectNodes("root/" + itemType + "/*"))
            {
                XmlNode tagItem = tagDoc.SelectSingleNode(String.Format("root/{1}/*[@{2}='{0}'] | root/{1}/region/*[@{2}='{0}']  ", srcItem.Attributes[keyName].Value, itemType, keyName));
                repairItem(srcItem, tagItem);
            }
        }*/
        /*private static void repairItem(XmlNode srcItem, XmlNode tagItem)
        {
            if (tagItem != null) {
                if (srcItem.AttrOrDefault("found", string.Empty) == string.Empty) {
                    tagItem.Attributes.RemoveAll();
                    copyAttributes(srcItem, tagItem);
                    tagItem.InnerXml = srcItem.InnerXml;
                    srcItem.SetAttrValue("found", "1");
                } else {
                    srcItem.SetAttrValue("dublers", "1");
                }
            }
        }*/
        //#endregion repair
        #region loadXmlOld
        /*public static void combineQuery(XmlNode query, XmlNode extention)
        {
            copyChildNodes(extention.SelectSingleNode("select"), query.SelectSingleNode("select"));
            copyChildNodes(extention.SelectSingleNode("from"), query.SelectSingleNode("from"));
            XmlNode links = extention.SelectSingleNode("links");
            if (links != null)
            {
                XmlNode table = query.SelectSingleNode("from/table");
                if (table != null)
                {
                    copyChildNodes(links, table);
                }
            }

            XmlNode where = extention.SelectSingleNode("where");
            if (@where != null)
            {
                XmlNode where1 = addChildNode(query, "where");
                where1.InnerXml = @where.InnerXml;
            }

            copyAttribute(extention, query, "pushpred");
            XmlNode pars = extention.SelectSingleNode("params");
            if (pars != null)
            {
                XmlNode tagPars = query.OwnerDocument.CreateElement("params");
                query.InsertBefore(tagPars, query.FirstChild);
                copyChildNodes(pars, tagPars);
            }
        }*/
        /*public static void copyChildNodes(XmlNode src, XmlNode tag)
        {
            if (src == null || tag == null)
            {
                return;
            }
            XmlNode buf = tag.OwnerDocument.CreateElement("buf");
            foreach (XmlNode node in src.SelectNodes("*"))
            {
                buf.InnerXml = node.OuterXml;
                tag.AppendChild(buf.FirstChild);
            }
        }*/
        /*public static XmlDocument loadXmlfromFiles(string path, XmlDocument inDoc, bool isScheme = false)
        {
            XmlDocument mainDoc;
            if (inDoc != null) {
                mainDoc = inDoc;
            } else {
                mainDoc = new XmlDocument();
                mainDoc.Load(path + @"\main.xml");
            }
            DirectoryInfo dir = new DirectoryInfo(path);
            foreach (FileInfo f in dir.GetFiles()) {
                if (!f.Name.ToLower().Equals("main.xml")) {
                    XmlDocument doc = new XmlDocument();
                    string sxml = Cmn.OpenText(f.FullName);
                    doc.LoadXml(sxml.Replace("xmlns=\"sqlbuilder\"", ""));
                    XmlNodeList mainGroups = mainDoc.FirstChild.ChildNodes;
                    foreach (XmlNode node1 in mainGroups) {
                        XmlNodeList nodes2 = doc.SelectNodes("//root/" + node1.Name + "/*[not(name()='region')] | //root/" + node1.Name + "/region/* | //query/" + node1.Name + "/*");
                        string s2 = "";
                        foreach (XmlNode node2 in nodes2) {
                            if (isScheme) {
                                node2.SetAttrValue("class", "1");
                            }
                            node2.SetAttrValue("file", f.FullName);
                            XmlNode query1;
                            if (node2.Name.Equals("query")) {
                                query1 = mainDoc.SelectSingleNode("root/queries/query[@name='" + node2.Attributes["name"].Value + "']");
                            } else {
                                query1 = null;
                            }
                            if (query1 == null) {
                                s2 += node2.OuterXml;
                            } else {
                                combineQuery(query1, node2);
                            }
                        }
                        node1.InnerXml = node1.InnerXml + s2;
                    }
                }
            }
            return mainDoc;
        }*/
        #endregion
        private static XmlDocument transformXml(XmlDocument inputDocument, XmlDocument xsltTemplate)
        {
            XslCompiledTransform transform = new XslCompiledTransform();
            transform.Load(xsltTemplate);
            StringWriter stringWriter = new StringWriter();
            transform.Transform(inputDocument, null, stringWriter);
            XmlDocument outputDocument = new XmlDocument();
            outputDocument.LoadXml(stringWriter.ToString());
            return outputDocument;
        }
        private static XmlDocument transformXml(XmlDocument inputDocument, string xsltTemplateName)
        {
            XmlDocument xsltTemplate = new XmlDocument();

            // В.Емцов Заглушка, для работы WEB
            try
            {
                xsltTemplate.Load(pathXsltSys + xsltTemplateName + ".xslt");
            }
            catch
            {
                xsltTemplate.Load(@"C:\infoenergo_root\root\main\all\sql.builder\sql.builder\xslt\" + xsltTemplateName + ".xslt");
            }

            return transformXml(inputDocument, xsltTemplate);
        }
        public static XmlDocument getItemProcessedXml(string itemType, string itemName, bool noMat, XmlDocument xmldoc, XmlDocument xmldoc_old)
        {
            XmlDocument source = null;
            IList<VSXElement> scheme;
            XmlNode node = xmldoc.SelectSingleNode("//root/*/" + itemType + "[@name='" + itemName + "']");
            if (node != null)
            {
                source = xmldoc;
                scheme = Environment.Manager.GetScheme();
            }
            else
            {
                source = xmldoc_old;
                scheme = Environment.Manager.GetOldScheme();
            }
            XmlDocument outputDocument = null;
            /*if (compilerVersion == 1) {
                XmlDocument xsltTemplate = new XmlDocument();
                xsltTemplate.Load(pathXsltSys + "step1" + ".xslt");

                XmlNamespaceManager manager = new XmlNamespaceManager(xsltTemplate.NameTable);
                manager.AddNamespace("xsl", "http://www.w3.org/1999/XSL/Transform");

                XmlNode param = xsltTemplate.SelectSingleNode("//xsl:stylesheet/xsl:variable[@name='" + itemType + "-name']/xsl:text", manager);
                param.InnerText = itemName;
                param = xsltTemplate.SelectSingleNode("//xsl:stylesheet/xsl:variable[@name='item-type']/xsl:text", manager);
                param.InnerText = itemType;
                outputDocument = transformXml(source, xsltTemplate);
            } else {*/
            XElement colmpiledQuery;
            if (itemType == "query")
            {
                colmpiledQuery = Compiler.compileQuery(itemName, scheme);
                outputDocument = new XmlDocument();
                outputDocument.LoadXml("" + colmpiledQuery);
            }
            /*else
            {
                colmpiledQuery=Compiler.compileReport(itemName);
                if (noMat)
                {
                    IEnumerable<XElement> queries = colmpiledQuery.Elements("query").Where(e => Compiler.getAttrValue(e, "materialize") == "1").ToArray();
                    if (queries.Count() == 1)
                    {
                        colmpiledQuery.Elements("query").Where(e => Compiler.getAttrValue(e, "materialize") != "1").Remove();
                        queries.First().Attributes("materialize").Remove();
                    }
                }
                 outputDocument = new XmlDocument();
             outputDocument.LoadXml("" + colmpiledQuery);

            }*/
            //}
            outputDocument = finalProcessing(outputDocument);
            return outputDocument;
        }
        public static XElement getItemProcessedXml2(string itemType, string itemName, bool noMat)
        {
            bool is_new_scheme = Environment.Manager.GetScheme().Elements(EName.queries).Elements(EName.query).SearchByAttribute(AName.name, itemName) != null;
            IList<VSXElement> scheme = is_new_scheme ? Environment.Manager.GetScheme() : Environment.Manager.GetOldScheme();
            if (itemType == "query")
            {
                return finalProcessing(Compiler.compileQuery(itemName, scheme));
            }
            else
            {
                return null;
            }
        }
        private static XmlDocument finalProcessing(XmlDocument outputDocument)
        {
            outputDocument = transformXml(outputDocument, "step6");
            outputDocument = transformXml(outputDocument, "step7");
            return outputDocument;
        }
        public static XElement finalProcessing(XElement outputDocument)
        {
            Compiler.SimplifySubquery(outputDocument);
            Compiler.Step6(outputDocument);
            Compiler.Step7(outputDocument);
            return outputDocument;
        }
        public static string getQuerySql(XmlNode query)
        {
            //if (Compiler.old_compile)
            //{
            //    XmlDocument inputDocument = new XmlDocument();
            //    inputDocument.InnerXml = "<root>" + query.OuterXml + "</root>";
            //    XmlDocument outputDocument = transformXml(inputDocument, "step8");
            //    return outputDocument.InnerText;
            //}
            //else
            //{
            return Compiler.GetSql(XElement.Parse("<root>" + query.OuterXml + "</root>"));
            //}
        }
        //public static string getQuerySql(string queryName)
        //{
        //    XmlNode queryXml = getItemProcessedXml("query", queryName, false);
        //    return getQuerySql(queryXml);
        //}
        public static string getProcedureSqlOld(XmlDocument report)
        {
            string procedureSql = "begin";
            foreach (XmlNode query in report.SelectNodes("root/query[@materialize=1 and not(@is-done)]"))
            {
                //foreach (XmlNode ccColumn in query.SelectNodes("//*[@client-calc=1]"))
                //{
                //    ccColumn.ParentNode.RemoveChild(ccColumn);
                //}
                procedureSql += getQuerySql(query);
            }
            procedureSql += "end;";
            return Compiler.normalizeWhitespace(procedureSql);
        }
        /*public static DataTable executeDataTable(XmlNode queryXml)
        {
            string querySql = getQuerySql(queryXml);
            DataTable dataTable = db.ExecuteDataTable(querySql, db.Connection);
            foreach (XmlNode columnNode in queryXml.SelectNodes("root/query/select/*[@title]"))
            {
                dataTable.Columns[columnNode.Attributes["as"].Value].Caption = columnNode.Attributes["title"].Value;

            }
            XmlNodeList keyColumns = queryXml.SelectNodes("root/query/select/*[@key=1]");
            DataColumn[] primaryKey = new DataColumn[keyColumns.Count];
            for (int i = 0; i < keyColumns.Count; i++)
            {
                primaryKey[i] = dataTable.Columns[keyColumns[i].Attributes["as"].Value];
            }

            dataTable.PrimaryKey = primaryKey;
            return dataTable;
        }*/
        //public static DataTable executeDataTable(string queryName)
        //{
        //    XmlNode queryXml = getItemProcessedXml("query", queryName, false);
        //    DataTable dataTable = executeDataTable(queryXml);
        //    return dataTable;
        //}
        /*public static XmlNode executeXml(string querySql, OracleConnection connection, XmlNode parentNode)
        {
            DataTable dataTable = db.ExecuteDataTable(querySql, connection);
            return dataTableToXml(dataTable, parentNode);
        }*/
        private static void setProgress(double value)
        {
        }
        //public static XmlDocument executeReport(string reportName)
        //{
        //    return executeReportOld(reportName, false, null);
        //}
        //public static XmlDocument getReportScheme(string reportName)
        //{
        //    return executeReportOld(reportName, true, null);
        //}
        public static XmlDocument executeReportOld(string reportName, bool schemeOnly, XmlDocument report)
        {
            var xmldoc = new XmlDocument();
            var xroot1 = new XElement(EName.root);
            xroot1.Add(Environment.Manager.GetScheme().Elements());
            xmldoc.LoadXml(xroot1.ToString());

            var xmldoc_old = new XmlDocument();
            xroot1 = new XElement(EName.root);
            xroot1.Add(Environment.Manager.GetOldScheme().Elements());

            xmldoc_old.LoadXml(xroot1.ToString());

            setProgress(0);

            XmlNode reportNode;
            XmlDocument source = xmldoc;
            reportNode = source.SelectSingleNode(String.Format("root/reports/report[@name='{0}']", reportName));

            if (reportNode == null)
            {
                source = xmldoc_old;
                reportNode = source.SelectSingleNode(String.Format("root/reports/report[@name='{0}']", reportName));
            }


            if (report == null)
            {
                report = getItemProcessedXml("report", reportName, false, xmldoc, xmldoc_old);
            }

            setProgress(10);
            //Application.DoEvents();
            //Application.DoEvents();


            if (!schemeOnly)
            {
                string procedureSql = getProcedureSqlOld(report);
                db.Connection.BeginTransaction();
                executeNonQuery(procedureSql, db.Connection);
            }


            XmlDocument outputDocument = new XmlDocument();
            XmlNode root = outputDocument.CreateElement("root");
            outputDocument.AppendChild(root);
            XmlNode schemeNode = addChildNode(root, "scheme");
            XmlNode dataNode = addChildNode(root, "data");


            foreach (XmlNode query in reportNode.SelectNodes("queries/query"))
            {
                getReportQueryScheme(query, report, schemeNode);
            }

            if (!schemeOnly)
            {
                SortedList<string, string> rabFields = new SortedList<string, string>();

                foreach (XmlNode nodeColumn in schemeNode.SelectNodes("//column[@into]"))
                {
                    if (rabFields.IndexOfKey(nodeColumn.Attributes["into"].Value) < 0)
                    {
                        rabFields.Add(nodeColumn.Attributes["into"].Value, nodeColumn.Attributes["into"].Value);
                    }
                }

                string connectSql = "select level,skod,rn";
                foreach (string col in rabFields.Values)
                {
                    connectSql += "," + col;
                }


                string firstLevelQueries = " (";
                string q = "";
                foreach (XmlNode nodeQuery in reportNode.SelectNodes("queries/query"))
                {
                    firstLevelQueries += q + "'" + nodeQuery.Attributes["name"].Value + "'";
                    q = ",";

                }

                firstLevelQueries += ") ";
                connectSql += " from rr_temp  connect by prior sid=sparentid start with skod in " + firstLevelQueries + " ORDER SIBLINGS BY skod,RN";



                setProgress(50);
                DataTable connectedTable = db.ExecuteDataTable(connectSql, db.Connection);
                setProgress(pr1);
                SortedList<string, string> sqlList = new SortedList<string, string>();
                namedQueryNodes = new SortedList<string, XmlNode>();
                namedColIndexes = new SortedList<string, int>();
                int i = 0;
                foreach (XmlNode query in schemeNode.SelectNodes("table"))
                {
                    i = executeReportQuery(query, report, dataNode, null, sqlList, db.Connection, connectedTable, i);
                }
                db.Connection.Commit();


                moveTransposedToParent(outputDocument);
                moveUnitedToParent(outputDocument);

            }

            setColumnsVisibility(outputDocument);
            return outputDocument;
        }
        private static double pr1 = 60;
        private static double pr2 = 80;
        // См. перевод на LINQ: Compiler.getReportQueryScheme()
        private static void getReportQueryScheme(XmlNode query, XmlNode report, XmlNode outputParent)
        {
            //Переделать на xslt
            XmlNode processedQuery = report.SelectSingleNode(String.Format("root/query[@name='{0}' and @materialize='1']", query.Attributes["name"].Value));
            XmlNode processedQuery1 = report.SelectSingleNode(String.Format("root/query[@name='{0}'and not(@materialize='1')]", query.Attributes["name"].Value));

            if (processedQuery == null)
            {
                processedQuery = processedQuery1;

            }
            else
            {
                XmlNode buf = processedQuery.OwnerDocument.CreateElement("buf");
                XmlNode processedQuerySel = processedQuery.SelectSingleNode("select");
                foreach (XmlNode srcColumn in processedQuery1.SelectNodes("select/*[@as]"))
                {
                    XmlNode srcColumn1 = processedQuery.SelectSingleNode("select/*[@as=\"" + srcColumn.Attributes["as"].Value + "\"]");
                    if (srcColumn1 == null)
                    {
                        buf.InnerXml = srcColumn.OuterXml;
                        processedQuerySel.AppendChild(buf.FirstChild);
                    }
                }

            }
            XmlNode table = addChildNode(outputParent, "table");

            copyAttribute(query, table, TextConst.AName.CalculateTree);
            copyAttribute(query, table, TextConst.AName.PrepareMerge);
            XmlNode joinCall = query.SelectSingleNode("call");
            if (joinCall != null)
            {
                XmlNode join = addChildNode(table, "joinon");
                join.InnerXml = joinCall.OuterXml;
            }

            table.SetAttrValue("name", processedQuery.Attributes["name"].Value);
            table.SetAttrValue("as", query.Attributes["as"].Value);
            copyAttribute(query, table, "main");
            copyAttribute(query, table, "title");

            if (query.Attributes["union"] != null)
            {
                table.SetAttrValue("union", query.Attributes["union"].Value);
            }
            XmlNode columns = addChildNode(table, "columns");


            XmlNode nodeTranspose = query.SelectSingleNode("transpose");

            if (nodeTranspose != null)
            {
                table.SetAttrValue("transposed", "1");
                XmlNode dimensionColumns = addChildNode(table, "dimension-сolumns");
                XmlNode valueColumns = addChildNode(table, "value-сolumns");
                string sql = getQuerySql(processedQuery1);
                XmlNode dimensionValues = null;

                string q = "";
                string colsPref = query.Attributes["as"].Value;
                string colKey = "";// "'" + colsPref + "'";
                string colTitle = "";
                string dimColName = "";
                foreach (XmlNode nodeCol in nodeTranspose.SelectNodes("dimension/column"))
                {
                    dimensionValues = addChildNode(table, "dimension-values");

                    XmlNode srcColumn = processedQuery.SelectSingleNode(String.Format("select/*[@as='{0}']", nodeCol.Attributes["column"].Value));

                    XmlNode column = addChildNode(dimensionColumns, "column");
                    dimColName = srcColumn.Attributes["as"].Value;//для начала возможно только одно измерение
                    colKey = "'" + dimColName + "'";
                    column.SetAttrValue("name", srcColumn.Attributes["as"].Value);
                    column.SetAttrValue(TextConst.AName.DataType, srcColumn.AttrOrDefault(TextConst.AName.DataType, TextConst.AVDataType.String));
                    XmlNode intoInfo = report.SelectSingleNode(String.Format("//query[@name='{0}']/insert/column[@info='{1}']", query.Attributes["name"].Value, srcColumn.Attributes["as"].Value));
                    if (intoInfo != null)
                    {
                        column.SetAttrValue("into", intoInfo.Attributes["column"].Value);
                    }
                    else
                    {
                        column.SetAttrValue("into", srcColumn.Attributes["as"].Value);
                    }


                    colKey += "||'_'||" + nodeCol.Attributes["column"].Value;
                    colTitle += q + nodeCol.Attributes["title"].Value;
                    q = "||'.'||";
                }

                sql = "select" + colKey + " as key, max (" + colTitle + ") as title, max(" + dimColName + ")||'' as dim_val from  (select rownum rn, a.* from (" + sql + ") a ) group by " + colKey + " order by min(rn)";

                DataTable transposeColumns = db.ExecuteDataTable(sql, db.Connection);

                int colIndex = 0;
                var i = 0;
                foreach (XmlNode nodeCol in nodeTranspose.SelectNodes("values/column"))
                {

                    XmlNode srcColumn = processedQuery.SelectSingleNode(String.Format("select/*[@as='{0}']", nodeCol.Attributes["column"].Value));
                    XmlNode intoInfo = report.SelectSingleNode(String.Format("//query[@name='{0}']/insert/column[@info='{1}']", query.Attributes["name"].Value, srcColumn.Attributes["as"].Value));

                    XmlNode column1 = addChildNode(valueColumns, "column");

                    column1.SetAttrValue("name", srcColumn.Attributes["as"].Value);
                    column1.SetAttrValue(TextConst.AName.DataType, srcColumn.AttrOrDefault(TextConst.AName.DataType, TextConst.AVDataType.String));
                    if (intoInfo != null)
                    {
                        column1.SetAttrValue("into", intoInfo.Attributes["column"].Value);
                    }
                    else
                    {
                        column1.SetAttrValue("into", srcColumn.Attributes["as"].Value);
                    }


                    foreach (DataRow dataRow in transposeColumns.Rows)
                    {

                        if (i == 0)
                        {
                            XmlNode dimVal = addChildNode(dimensionValues, "val");
                            dimVal.SetAttrValue("value", dataRow["dim_val"].ToString());
                            dimVal.SetAttrValue("title", dataRow["title"].ToString());
                            dimVal.SetAttrValue("columnpref", dataRow["key"].ToString() + "_");
                        }



                        XmlNode column = addChildNode(columns, "column");

                        column.SetAttrValue("name", dataRow["key"].ToString() + "_" + srcColumn.Attributes["as"].Value);
                        column.SetAttrValue("dimension-column", dimColName);
                        column.SetAttrValue("value-column", srcColumn.Attributes["as"].Value);
                        column.SetAttrValue("dimension-value", dataRow["dim_val"].ToString());
                        if (srcColumn.Attributes["title"] != null)
                        {
                            column.SetAttrValue("title", srcColumn.Attributes["title"].Value);

                        }

                        column.SetAttrValue("band-title", dataRow["title"].ToString());
                        column.SetAttrValue("table", query.Attributes["as"].Value);
                        column.SetAttrValue(TextConst.AName.DataType, srcColumn.AttrOrDefault(TextConst.AName.DataType, TextConst.AVDataType.String));

                        if (intoInfo != null)
                        {
                            column.SetAttrValue("into", intoInfo.Attributes["column"].Value);
                        }
                        else
                        {
                            column.SetAttrValue("into", srcColumn.Attributes["as"].Value);
                        }
                        column.SetAttrValue("index", colIndex.ToString());
                        colIndex++;
                    }
                    i++;

                }
            }
            else
            {
                //      int iiii=1;
                foreach (XmlNode srcColumn in processedQuery.SelectNodes("select/*[@as]"))
                {
                    XmlNode column = addChildNode(columns, "column");
                    column.SetAttrValue("name", srcColumn.Attributes["as"].Value);
                    if (srcColumn.Attributes["title"] != null)
                    {
                        var tt = srcColumn.Attributes["title"].Value;
                        if (tt == "-")
                        {
                            srcColumn.Attributes["title"].Value = "";
                        }
                        column.SetAttrValue("title", srcColumn.Attributes["title"].Value);
                    }
                    column.SetAttrValue(TextConst.AName.DataType, srcColumn.AttrOrDefault(TextConst.AName.DataType, TextConst.AVDataType.String));
                    copyAttribute(srcColumn, column, "editor");
                    copyAttribute(srcColumn, column, "agg");
                    copyAttribute(srcColumn, column, "format");
                    copyAttribute(srcColumn, column, TextConst.AName.CMaster);
                    copyAttribute(srcColumn, column, TextConst.AName.CMasterKey);
                    copyAttribute(srcColumn, column, "pivot");
                    copyAttribute(srcColumn, column, "dimname");
                    copyAttribute(srcColumn, column, "visible");
                    copyAttribute(srcColumn, column, TextConst.AName.Intern);
                    XmlNode intoInfo = report.SelectSingleNode(String.Format("//query[@name='{0}']/insert/column[@info='{1}']", query.Attributes["name"].Value, srcColumn.Attributes["as"].Value));
                    if (intoInfo != null)
                    {
                        column.SetAttrValue("into", intoInfo.Attributes["column"].Value);
                    }
                    else
                    {
                        column.SetAttrValue("into", srcColumn.Attributes["as"].Value);
                    }
                    copyAttribute(srcColumn, column, "key");


                    copyAttribute(srcColumn, column, "value-column");
                    copyAttribute(srcColumn, column, "dimension-column");
                    copyAttribute(srcColumn, column, "dimension-value");
                    copyAttribute(srcColumn, column, "band-title");
                    copyAttribute(srcColumn, column, "value-title");
                    copyAttribute(srcColumn, column, "class-title");
                    copyAttribute(srcColumn, column, TextConst.AName.ClientCalulation);
                    copyAttribute(srcColumn, column, TextConst.AName.ExcelCalulation);
                    foreach (string attrName in Compiler.AdditionalAttributes)
                    {
                        copyAttribute(srcColumn, column, attrName);
                    }



                }
            }
            XmlNode childs = null;

            foreach (XmlNode childQuery in query.SelectNodes("query"))
            {
                if (childs == null)
                {
                    childs = addChildNode(table, "childs");
                }
                getReportQueryScheme(childQuery, report, childs);
            }
            // XmlNode tablecolumns = table.SelectSingleNode("columns");
            if (table.AttrOrDefault("transposed", string.Empty) != "1")
            {
                applyColumnsPreset(query, report, table);
                //} else {
                // XmlNode reportColumns = addChildNode(table, "columns");
                // reportColumns.InnerXml = tablecolumns.InnerXml; 
            }
            // querycolumns.ParentNode.RemoveChild(querycolumns);
        }
        // См. перевод на LINQ: Compiler.applyColumnsPreset()
        private static void applyColumnsPreset(XmlNode query, XmlNode report, XmlNode table)
        {
            XmlNode reportColumns = addChildNode(table, "viewcolumns");
            XmlNode reportSrcColumns = query.SelectSingleNode("columns");

            if (reportSrcColumns == null)
            {

                XmlNode repScheme = query.SelectSingleNode("ancestor::report");
                XmlNode firstQuery = repScheme.SelectSingleNode("queries/query");
                if (firstQuery.Equals(query))
                {
                    reportSrcColumns = repScheme.SelectSingleNode("columns");
                }

            }

            if (reportSrcColumns != null)
            {
                reportColumns.InnerXml = reportSrcColumns.InnerXml;
            }

            foreach (XmlNode band in reportColumns.SelectNodes(".//band"))
            {
                // обрабатывается только частный сучай - недоделано

                if (band.Attributes["table"] != null && band.Attributes["column"] != null)
                {
                    XmlNode transpTbl = table.SelectSingleNode(String.Format("childs/table[@as='{0}' and dimension-сolumns/column/@name='{1}']", band.Attributes["table"].Value, band.Attributes["column"].Value));

                    if (transpTbl != null)
                    {
                        foreach (XmlNode dimVal in transpTbl.SelectNodes("dimension-values/*"))
                        {
                            XmlNode valBand = addChildNode(band.ParentNode, "band");
                            band.ParentNode.InsertBefore(valBand, band);
                            valBand.InnerXml = band.InnerXml;
                            copyAttributes(band, valBand);
                            copyAttributes(dimVal, valBand);
                            valBand.SetAttrValue("band-type", "dimband");
                            foreach (XmlNode col in valBand.SelectNodes(".//column"))
                            {
                                col.SetAttrValue("name", transpTbl.SelectSingleNode("dimension-сolumns/column").Attributes["name"].Value + "_" + dimVal.Attributes["value"].Value + "_" + col.Attributes["name"].Value);
                            }
                        }
                        band.ParentNode.RemoveChild(band);
                    }
                    else
                    {

                        transpTbl = table.SelectSingleNode(String.Format("childs/table[@as='{0}' and value-сolumns/column/@name='{1}']", band.Attributes["table"].Value, band.Attributes["column"].Value));
                        if (transpTbl != null)
                        {
                            band.SetAttrValue("band-type", "valband");
                            foreach (XmlNode dimVal in transpTbl.SelectNodes("dimension-values/*"))
                            {
                                XmlNode valBandedCol = addChildNode(band, "column");
                                valBandedCol.SetAttrValue("table", band.Attributes["table"].Value);
                                copyAttribute(band, valBandedCol, "format");
                                copyAttribute(band, valBandedCol, "default");
                                valBandedCol.SetAttrValue("name", transpTbl.SelectSingleNode("dimension-сolumns/column").Attributes["name"].Value + "_" + dimVal.Attributes["value"].Value + "_" + band.Attributes["column"].Value);
                            }
                        }
                        else
                        {
                            band.SetAttrValue("band-type", "band");
                        }
                    }

                }
                else
                {
                    band.SetAttrValue("band-type", "band");
                }

            }

            int presetCount = reportColumns.SelectNodes("column[@table='" + query.Attributes["as"].Value + "']").Count;

            foreach (XmlNode tblCol in table.SelectNodes("columns/* | childs/table[@transposed]/columns/*"))
            {

                string tablePname = tblCol.ParentNode.ParentNode.Attributes["as"].Value;
                XmlNode repCol = reportColumns.SelectSingleNode(String.Format(".//column[@table='{0}' and @name='{1}']", tablePname, tblCol.Attributes["name"].Value));

                if (repCol == null)
                {

                    repCol = addChildNode(reportColumns, "column");
                    if (presetCount > 0)
                    {
                        repCol.SetAttrValue("visible", "0");
                    }

                }
                copyAttributeNoReplace(tblCol, repCol, "visible");

                if (repCol.ParentNode.Attributes["band-type"] != null)
                {
                    if (repCol.ParentNode.Attributes["band-type"].Value.Equals("dimband"))
                    {
                        if (repCol.ParentNode.Attributes["title"] == null)
                        {
                            repCol.ParentNode.SetAttrValue("title", tblCol.Attributes["band-title"].Value);
                        }
                    }
                    if (repCol.ParentNode.Attributes["band-type"].Value.Equals("valband"))
                    {
                        if (repCol.ParentNode.Attributes["title"] == null)
                        {
                            repCol.ParentNode.SetAttrValue("title", tblCol.Attributes["title"].Value);
                        }
                        if (tblCol.Attributes["band-title"] != null)
                        {
                            repCol.SetAttrValue("title", tblCol.Attributes["band-title"].Value);
                        }
                    }

                }

                repCol.SetAttrValue("table", tblCol.ParentNode.ParentNode.Attributes["as"].Value);
                copyAttribute(tblCol, repCol, "name");
                // copyAttribute(tblCol, repCol, "format");
                copyAttribute(tblCol, repCol, "editable");
                copyAttribute(tblCol, repCol, "pivot");
                copyAttribute(tblCol, repCol, "dimname");
                copyAttribute(repCol, tblCol, "default");
                copyAttributeNoReplace(tblCol, repCol, "agg");
                copyAttribute(repCol, tblCol, "agg");
                copyAttributeNoReplace(tblCol, repCol, "format");
                copyAttributeNoReplace(tblCol, repCol, TextConst.AName.CMaster);
                copyAttributeNoReplace(tblCol, repCol, TextConst.AName.CMasterKey);
                copyAttribute(repCol, tblCol, "format");

                copyAttribute(tblCol, repCol, "class-title");
                copyAttributeNoReplace(tblCol, repCol, "title");
                copyAttribute(tblCol, repCol, "type");
                copyAttribute(tblCol, repCol, "into");
                copyAttribute(tblCol, repCol, TextConst.AName.ClientCalulation);
                copyAttribute(tblCol, repCol, TextConst.AName.ExcelCalulation);
                repCol.SetAttrValue("assigned", "1");

                // 09.02.15 В.Емцов  Добавил обработку node-id и parent-node-id
                //var que = report.SelectSingleNode("root/query");

                string name = repCol.Attributes[TextConst.AName.Name].Value;
                if (query.AttrOrDefault(TextConst.AName.ParentNodeId, string.Empty) == string.Empty)
                {
                    if (name == TextConst.SpecCols.ParentGRowId || name == TextConst.SpecCols.GRowId)
                    {
                        query.SetAttrValue(TextConst.AName.ParentNodeId, TextConst.SpecCols.ParentGRowId);
                        query.SetAttrValue(TextConst.AName.NodeId, TextConst.SpecCols.GRowId);
                    }
                }
                string node_id = query.AttrOrDefault("node-id", string.Empty);
                if (name == node_id)
                {
                    repCol.SetAttrValue("node-id", "1");
                }
                string pnode_id = query.AttrOrDefault("parent-node-id", string.Empty);
                if (name == pnode_id)
                {
                    repCol.SetAttrValue("parent-node-id", "1");
                    tblCol.SetAttrValue(TextConst.AName.ParentNodeId, "1");
                }
                foreach (string attrName in Compiler.AdditionalAttributes)
                {
                    copyAttribute(tblCol, repCol, attrName);
                }
            }
        }
        private static SortedList<string, XmlNode> namedQueryNodes = null;
        private static SortedList<string, int> namedColIndexes = null;
        private static int executeReportQuery(XmlNode query, XmlNode report, XmlNode outputParent, string parentId, SortedList<string, string> sqlList, VOracleConnection connection, DataTable dataTable, int rowIndex)
        {
            // проверил работу на трехуровневой структуре report = temp-1 , по одному элементу на каждом уровне... работа при наличии 2-х и более дочерних таблиц не проверена (могут быть проблемы).

            if (dataTable.Rows.Count <= rowIndex)
            {
                return rowIndex;
            }

            if (!dataTable.Rows[rowIndex]["skod"].ToString().Equals(query.Attributes["name"].Value))
            {
                return rowIndex;
            }


            if (parentId == null)
            {
                namedColIndexes = new SortedList<string, int>();
            }
            int i = rowIndex;
            XmlNode nodeTable = addChildNode(outputParent, "table");
            XmlNode nodeData = addChildNode(nodeTable, "data");
            if (i == dataTable.Rows.Count)
            {
                return i;
            }

            XmlNode nodeRow = null;
            XmlNode nodeCells = null;
            XmlNode dimensionColumns = query.SelectSingleNode("dimension-сolumns");


            int level = Convert.ToInt32(dataTable.Rows[rowIndex]["level"].ToString());

            nodeTable.SetAttrValue("name", query.Attributes["name"].Value);
            nodeTable.SetAttrValue("as", query.Attributes["as"].Value);


            bool loopExit = false;

            XmlNode childs = null;
            XmlNode valueColumns = null;
            if (dimensionColumns != null)
            {
                valueColumns = query.SelectSingleNode("value-сolumns");
                nodeRow = addChildNode(nodeData, "tr");
                nodeRow.SetAttrValue("id", parentId);
                nodeTable.SetAttrValue("transposed", "1");
                nodeCells = addChildNode(nodeRow, "cells");
                foreach (XmlNode column in query.SelectNodes("columns//column"))
                {
                    XmlNode nodeCell = addChildNode(nodeCells, "td");
                }
            }


            while (!loopExit)
            {


                setProgress(pr1 + (pr2 - pr1) / dataTable.Rows.Count * i);
                DataRow row = dataTable.Rows[i];

                if (dimensionColumns == null)
                {
                    nodeRow = addChildNode(nodeData, "tr");
                    nodeRow.SetAttrValue("id", row[dataTable.Columns["sid"]].ToString());
                    nodeCells = addChildNode(nodeRow, "cells");
                    foreach (XmlNode column in query.SelectNodes("columns//column"))
                    {
                        XmlNode nodeCell = addChildNode(nodeCells, "td");
                        nodeCell.InnerText = row[column.Attributes["into"].Value].ToString();

                    }
                }
                else
                {

                    // string colName1 = query.Attributes["as"].Value;
                    string colName1 = ""; //пока только одно измерение ;
                    foreach (XmlNode nodeCol in dimensionColumns.ChildNodes)
                    {
                        colName1 = nodeCol.Attributes["name"].Value;
                        colName1 += "_" + row[nodeCol.Attributes["into"].Value].ToString();
                    }
                    string colName;
                    foreach (XmlNode nodeCol in valueColumns.ChildNodes)
                    {

                        colName = colName1 + "_" + nodeCol.Attributes["name"].Value;
                        int colIndex;
                        //вероятно тормозит(ло) это место, 10000 строк пара минут. 
                        string colFullName = query.Attributes["as"].Value + "_" + colName;
                        if (namedColIndexes.IndexOfKey(colFullName) < 0)
                        {
                            colIndex = Convert.ToInt32(query.SelectSingleNode(String.Format("columns//column[@name='{0}']", colName)).Attributes["index"].Value);
                            namedColIndexes.Add(colFullName, colIndex);
                        }
                        else
                        {
                            colIndex = namedColIndexes[colFullName];
                        }
                        // string colIndex = "1";//query.SelectSingleNode( string.Format( "columns/column[@name='{0}']", colName)).Attributes["index"].Value;
                        XmlNode nodeCell = nodeCells.ChildNodes[colIndex];
                        nodeCell.InnerText = row[nodeCol.Attributes["into"].Value].ToString();
                    }

                }



                i++;
                childs = null;
                if (i < dataTable.Rows.Count)
                {
                    bool loopExit2 = false;
                    while (!loopExit2)
                    {
                        DataRow nextRow = dataTable.Rows[i];
                        string nextQueryName = nextRow["skod"].ToString();
                        if (!nextQueryName.Equals(query.Attributes["name"].Value))
                        {
                            int nextLevel = Convert.ToInt32(nextRow["level"].ToString());
                            if (nextLevel > level)
                            {
                                if (childs == null)
                                {
                                    childs = addChildNode(nodeRow, "childs");
                                }
                                XmlNode nextQuery;
                                if (namedQueryNodes.IndexOfKey(nextQueryName) < 0)
                                {
                                    nextQuery = query.SelectSingleNode(String.Format("//scheme//table[@name='{0}']", nextQueryName));
                                    namedQueryNodes.Add(nextQueryName, nextQuery);
                                }
                                else
                                {
                                    nextQuery = namedQueryNodes[nextQueryName];
                                }

                                i = executeReportQuery(nextQuery, report, childs, row[dataTable.Columns["sid"]].ToString(), sqlList, connection, dataTable, i);
                                if (i == dataTable.Rows.Count)
                                {
                                    loopExit2 = true;
                                }
                                /* else
                                 {
                                     nextRow = dataTable.Rows[i];
                                     nextQueryName = nextRow["skod"].ToString();
                                     nextLevel = Convert.ToInt32(nextRow["level"].ToString());
                                     if (nextLevel==level &  !nextQueryName.Equals() ){

                                     }
                                     nextRow["skod"].ToString()
                                 }*/
                            }
                            else
                            {
                                loopExit2 = true;
                                loopExit = true;
                            }
                        }
                        else
                        {
                            loopExit2 = true;
                        }
                    }

                }
                if (i == dataTable.Rows.Count)
                {
                    loopExit = true;
                }
            }
            return i;
            //return nodeTable;


            /* string sql;
             if (sqlList.IndexOfKey(query.Attributes["name"].Value) < 0)
             {
                 XmlNode processedQuery = report.SelectSingleNode(String.Format("root/query[@name='{0}' and not(@materialize='1')]", query.Attributes["name"].Value));
                 sql = getQuerySql(processedQuery);
                 sqlList.Add(query.Attributes["name"].Value, sql);
             }
             else
             {
                 sql = sqlList[query.Attributes["name"].Value];
             }
             if (parentId != null)
             {
                 sql = string.Format("select * from ({0}) where sparentid='{1}'", sql, parentId);
             }

            XmlNode result= executeXml(sql, CurrentConnection, outputParent);
            addAttribute(result, "name", query.Attributes["name"].Value);
            foreach (XmlNode childQuery in query.SelectNodes("query"))
            {
                foreach (XmlNode resultRow in result.SelectNodes("data/tr"))
                {
                    XmlNode childs = resultRow.SelectSingleNode("childs");
                    if (childs == null)
                    {
                        childs = addChildNode(resultRow, "childs");
                    }
                    rowIndex=executeReportQuery(childQuery, report, childs, resultRow.Attributes["id"].Value, sqlList, CurrentConnection, dataTable, rowIndex);
                }
            }*/

        }
        // См. перевод на LINQ: Compiler.moveTransposedToParent()
        private static void moveTransposedToParent(XmlDocument reportData)
        {

            foreach (XmlNode band in reportData.SelectNodes("//viewcolumns//band[not(@title)]"))
            {
                band.ParentNode.RemoveChild(band);
            }
            foreach (XmlNode parentCells in reportData.SelectNodes("root/data//table/data/tr[childs/table[@transposed='1']]/cells"))
            {
                foreach (XmlNode childCell in parentCells.ParentNode.SelectNodes("childs/table[@transposed='1']/data/tr/cells/td"))
                {
                    parentCells.AppendChild(childCell);
                }
            }

            foreach (XmlNode parentCols in reportData.SelectNodes("root/scheme//table[childs/table[@transposed='1']]/columns"))
            {
                foreach (XmlNode childCol in parentCols.ParentNode.SelectNodes("childs/table[@transposed='1']/columns/column"))
                {
                    parentCols.AppendChild(childCol);
                }
            }


            foreach (XmlNode parentTable in reportData.SelectNodes("root/scheme//table[childs/table[@transposed='1']]"))
            {
                foreach (XmlNode dimInfo in parentTable.SelectNodes("childs/table[@transposed='1']/dimension-values"))
                {
                    dimInfo.SetAttrValue("table", dimInfo.ParentNode.Attributes["as"].Value);
                    parentTable.AppendChild(dimInfo);
                }
            }



            foreach (XmlNode transposedTable in reportData.SelectNodes("//table[@transposed='1']"))
            {
                XmlNode parent = transposedTable.ParentNode;
                parent.RemoveChild(transposedTable);
                if (!parent.HasChildNodes)
                {
                    parent.ParentNode.RemoveChild(parent);
                }
            }
        }
        // См. перевод на LINQ: Compiler.moveUnitedToParent
        private static void moveUnitedToParent(XmlDocument reportData)
        {
            //Пока только для случая объединения таблиц на верхнем уровне

            foreach (XmlNode unTable in reportData.SelectNodes("root/scheme/table[@union='1']"))
            {
                XmlNode mainTable = unTable.SelectSingleNode("preceding::table[not(@union='1')]");
                SortedList<int, int> colCor = new SortedList<int, int>();
                SortedList<string, int> unColInd = new SortedList<string, int>();
                int mainIndex = 0;
                int unIndex = 0;
                foreach (XmlNode unCol in unTable.SelectNodes("columns/column"))
                {
                    unColInd.Add(unCol.Attributes["name"].Value, unIndex);
                    unIndex++;
                }
                foreach (XmlNode mainCol in mainTable.SelectNodes("columns/column"))
                {
                    if (unColInd.IndexOfKey(mainCol.Attributes["name"].Value) > -1)
                    {
                        colCor.Add(mainIndex, unColInd[mainCol.Attributes["name"].Value]);
                    }
                    mainIndex++;
                }
                XmlNode mainData = reportData.SelectSingleNode(String.Format("root/data/table[@as='{0}']/data", mainTable.Attributes["as"].Value));
                unTable.ParentNode.RemoveChild(unTable);
                XmlNode unTableData = reportData.SelectSingleNode(String.Format("root/data/table[@as='{0}']", unTable.Attributes["as"].Value));
                if (unTableData != null)
                {
                    foreach (XmlNode unRow in unTableData.SelectNodes("data/tr/cells"))
                    {
                        mainIndex = 0;
                        XmlNode tr = addChildNode(mainData, "tr");
                        copyAttributes(unRow.ParentNode, tr);
                        XmlNode trCells = addChildNode(tr, "cells");
                        foreach (XmlNode mainCol in mainTable.SelectNodes("columns/column"))
                        {
                            XmlNode td = addChildNode(trCells, "td");

                            if (colCor.IndexOfKey(mainIndex) > -1)
                            {
                                XmlNode unCell = unRow.ChildNodes[colCor[mainIndex]];
                                td.InnerXml = unCell.InnerXml;
                            }
                            mainIndex++;
                        }
                    }

                    unTableData.ParentNode.RemoveChild(unTableData);
                }

            }

            foreach (XmlNode parentCells in reportData.SelectNodes("root/data//table/data/tr[childs/table[@transposed='1']]/cells"))
            {
                foreach (XmlNode childCell in parentCells.ParentNode.SelectNodes("childs/table[@transposed='1']/data/tr/cells/td"))
                {
                    parentCells.AppendChild(childCell);
                }
            }

            foreach (XmlNode parentCols in reportData.SelectNodes("root/scheme//table[childs/table[@transposed='1']]/columns"))
            {
                foreach (XmlNode childCol in parentCols.ParentNode.SelectNodes("childs/table[@transposed='1']/columns/column"))
                {
                    parentCols.AppendChild(childCol);
                }
            }



            foreach (XmlNode transposedTable in reportData.SelectNodes("//table[@transposed='1']"))
            {

                transposedTable.ParentNode.RemoveChild(transposedTable);


            }
        }
        // См. перевод на LINQ: Compiler.setColumnsVisibility()
        private static void setColumnsVisibility(XmlDocument reportData)
        {
            foreach (XmlNode col in reportData.SelectNodes("root/scheme//viewcolumns//column[not(@title) or @title='' and not(@visible)]"))
            {
                col.SetAttrValue(TextConst.AName.Visible, TextConst.AVBool.False);
            }
        }
        public static void executeNonQuery(string sql, VOracleConnection connection, VOracleParameter[] pars = null, bool analyze = true)
        {
            string sql1 = Cmn.ClearUndefined(sql);
            // для пск
            //sql = sql.Replace(TextConst.DBObjects.TempTable, "rr_temp11");
            //connection = XmlReports.Environment.Connection.Clone();
            //connection.Open(useGlobalSettings: true);
            VOracleCommand command = null;
            try
            {
                command = new VOracleCommand(sql1, connection);
                if (!Array.IsNullOrEmpty(pars))
                {
                    command.Parameters.AddRange(DataHelper.ToOracleParameters(pars));
                }
                //var con = new OracleConnection();
                //con.Server = "asuse_kg";
                //con.UserId = "belchenkoav";
                //con.Password = "qqqq";
                //con.Open(useGlobalSettings: true);
                //var cmd = new OracleCommand(VDBCommand.GetCmdParametrizedText(command), con);
                //cmd.ExecuteNonQuery();
                if (analyze)
                {
                    DevUtilsProvider.Instance.AnalyzeExecSql(sql1);
                }

                command.ExecuteNonQuery();
            }
            catch (Exception e)
            {
                throw e;
            }
            finally
            {
                Cmn.DisposeAndSetNull(ref command);
            }
        }
        /*public static DataTable executeDataTable(XmlNode query, OracleConnection connection)
        {
            string querySql = getQuerySql(query);

            DataTable dataTable = db.ExecuteDataTable(querySql, connection);
            return dataTable;
        }*/
        /*public static DataTable xmlToDataTable(XmlNode nodeTable)
        {



            XmlNodeList nodesColumns = nodeTable.SelectNodes("//scheme/table[not(@union)]/columns/* | //scheme/table[not(@union)]/childs/table[@transposed]/columns/*");

            XmlNode nodeTableScheme = nodeTable.SelectSingleNode("//scheme/table[not(@union)]");
            XmlNode nodeData = nodeTable.SelectSingleNode(String.Format("//data/table[@name='{0}']/data", nodeTableScheme.Attributes["name"].Value));
            // XmlNodeList nodesColumns = nodeTable.SelectNodes("//scheme/columns/*");
            DataTable dataTable = new DataTable();
            foreach (XmlNode nodeColumn in nodesColumns)
            {
                DataColumn column = dataTable.Columns.Add(nodeColumn.Attributes["name"].Value);

                if (nodeColumn.Attributes["type"].Value.Equals("number"))
                {
                    column.DataType = numberType;
                }
                if (nodeColumn.Attributes["type"].Value.Equals("bool"))
                {
                    column.DataType = numberType;
                }
                if (nodeColumn.Attributes["type"].Value.Equals("date"))
                {
                    column.DataType = (new DateTime()).GetType();
                }
            }
            int j = 0;
            if (nodeData != null)
            {
                XmlNodeList nodeRows = nodeData.SelectNodes("tr");

                foreach (XmlNode nodeRow in nodeRows)
                {
                    setProgress(pr2 + (95 - pr2) / nodeRows.Count * j);
                    j++;

                    object[] rowData = new object[nodesColumns.Count];
                    int i = 0;
                    foreach (XmlNode nodeCell in nodeRow.SelectNodes("cells/td | childs/table[@transposed]/data/tr/cells/td"))
                    {
                        if (nodeCell.InnerText.Equals(""))
                        {
                            rowData[i] = null;
                        }
                        else
                        {
                            if (nodesColumns[i].Attributes["type"].Value.Equals("number") | nodesColumns[i].Attributes["type"].Value.Equals("bool"))
                            {

                                rowData[i] = Convert.ToDecimal(nodeCell.InnerText);

                            }
                            else
                            {
                                rowData[i] = nodeCell.InnerText;
                            }
                        }
                        i++;
                    }
                    DataRow row = dataTable.Rows.Add(rowData);
                }
            }


            XmlNodeList unionTables = nodeTable.SelectNodes("//scheme/table[@union='1']");

            foreach (XmlNode unionTable in unionTables)
            {
                j = 0;
                XmlNodeList unionTableColumns = unionTable.SelectNodes("columns/* | childs/table[@transposed]/columns/*");
                XmlNode unionTableData = nodeTable.SelectSingleNode(String.Format("//data/table[@name='{0}']/data", unionTable.Attributes["name"].Value));

                SortedList<string, int> cols = new SortedList<string, int>();

                foreach (XmlNode nodeColumn in unionTableColumns)
                {
                    cols.Add(nodeColumn.Attributes["name"].Value, j);
                    j++;
                }
                XmlNodeList unionTableRows = unionTableData.SelectNodes("tr");
                foreach (XmlNode nodeRow in unionTableRows)
                {
                    object[] rowData = new object[dataTable.Columns.Count];
                    int i = 0;
                    XmlNodeList unionTableCells = nodeRow.SelectNodes("cells/td | childs/table[@transposed]/data/tr/cells/td");
                    foreach (DataColumn dataColumn in dataTable.Columns)
                    {
                        if (cols.IndexOfKey(dataColumn.ColumnName) < 0)
                        {
                            rowData[i] = null;
                        }
                        else
                        {
                            XmlNode nodeCell = unionTableCells[cols[dataColumn.ColumnName]];
                            if (nodeCell.InnerText.Equals(""))
                            {
                                rowData[i] = null;
                            }
                            else
                            {
                                if (nodesColumns[i].Attributes["type"].Value.Equals("number") | nodesColumns[i].Attributes["type"].Value.Equals("bool"))
                                {

                                    rowData[i] = Convert.ToDecimal(nodeCell.InnerText);

                                }
                                else
                                {
                                    rowData[i] = nodeCell.InnerText;
                                }
                            }
                        }
                        i++;
                    }
                    DataRow row = dataTable.Rows.Add(rowData);
                }
            }

            j = 0;
            foreach (XmlNode repCol in nodeTable.SelectNodes("//scheme/columns//column[@assigned]"))
            {
                DataColumn dataColumn = dataTable.Columns[repCol.Attributes["name"].Value];
                if (repCol.Attributes["title"] != null)
                {
                    dataColumn.Caption = repCol.Attributes["title"].Value;
                }
                if (repCol.Attributes["hidden"] != null)
                {
                    dataColumn.Caption = dataColumn.ColumnName;// временное решение , не показываются колонки у которых заголовок равен имени (считаем что нет заголовка)
                }
                dataColumn.SetOrdinal(j);
                j++;
            }


            setProgress(98);
            return dataTable;
        }*/
        /*public static XmlNode dataTableToXml(DataTable dataTable, XmlNode parentNode)
        {
            return dataTableToXml(dataTable, parentNode, 0, dataTable.Rows.Count - 1);
        }*/
        /*public static XmlNode dataTableToXml(DataTable dataTable, XmlNode parentNode, int startIndex, int endIndex)
        {
            XmlNode nodeTable = addChildNode(parentNode, "table");
            XmlNode nodeData = addChildNode(nodeTable, "data");
            for (int i = startIndex; i <= endIndex; i++) {
                DataRow row = dataTable.Rows[i];
                XmlNode nodeRow = addChildNode(nodeData, "tr");
                nodeRow.SetAttrValue("id", row[dataTable.Columns["sid"]].ToString());
                XmlNode nodeCells = addChildNode(nodeRow, "cells");
                foreach (DataColumn column in dataTable.Columns) {
                    XmlNode nodeCell = addChildNode(nodeCells, "td");
                    nodeCell.InnerText = row[column].ToString();
                }
            }
            return nodeTable;
        }*/
        public static XmlNode addChildNode(XmlNode nodeParent, string name)
        {
            XmlNode nodeChild = nodeParent.OwnerDocument.CreateElement(name);
            nodeParent.AppendChild(nodeChild);
            return nodeChild;
        }
        /*private static string getAttribute(XmlNode node, string name)
        {
            if (node == null) {
                return string.Empty;
            } else {
                return node.AttrOrDefault(name, string.Empty);
            }
        }*/
        private static void copyAttribute(XmlNode nodeSrc, XmlNode nodeTag, string name)
        {
            XmlAttribute attribute = nodeSrc.Attributes[name];
            if (attribute != null)
            {
                XmlAttribute attribute1 = nodeTag.Attributes[name];
                if (attribute1 == null)
                {
                    attribute1 = nodeTag.OwnerDocument.CreateAttribute(name);
                    nodeTag.Attributes.Append(attribute1);
                }
                attribute1.Value = attribute.Value;
            }
        }
        private static void copyAttributes(XmlNode nodeSrc, XmlNode nodeTag)
        {
            foreach (XmlAttribute attribute in nodeSrc.Attributes)
            {
                copyAttribute(nodeSrc, nodeTag, attribute.Name);
            }
        }
        private static void copyAttributeNoReplace(XmlNode nodeSrc, XmlNode nodeTag, string name)
        {
            XmlAttribute attribute = nodeSrc.Attributes[name];
            if (attribute != null)
            {
                XmlAttribute attribute1 = nodeTag.Attributes[name];
                if (attribute1 == null)
                {
                    attribute1 = nodeTag.OwnerDocument.CreateAttribute(name);
                    nodeTag.Attributes.Append(attribute1);
                    attribute1.Value = attribute.Value;
                }
            }
        }
        public static XmlNode XElementToXmlNode(XElement element)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(element.ToString());
            return doc.FirstChild;
        }
        public static string[] GetInputFolderNames()
        {
            string[] folder_names = { };
            if (InputParams != null)
            {
                XElement folder = InputParams.Elements(EName.param).SearchByAttribute(AName.name, "folder");
                if (folder != null)
                {
                    folder_names = folder.Elements(EName.@const).Select(EPredicate.ElementValue).ToArray();
                    folder.Remove();
                }
            }
            return folder_names;
        }
        public static string GetInputReportName()
        {
            string report_name = string.Empty;
            if (InputParams != null)
            {
                XElement report = InputParams.Elements(EName.param).SearchByAttribute(AName.name, "report");
                if (report != null)
                {
                    report_name = report.Element(EName.@const).Value;
                    report.Remove();
                }
            }
            return report_name;
        }
        public static string DepTitle = string.Empty;
        public static string GetMainTitle()
        {
            string title = null;
            if (InputParams != null)
            {
                XElement xtitle = InputParams.Elements(EName.param).SearchByAttribute(AName.name, "title");
                if (xtitle != null)
                {
                    title = xtitle.Element(EName.@const).Value;
                    xtitle.Remove();
                }
            }
            if (sql.builder.UI.UIStatic.IsMpep)
            {
                title = "Мониторинг планово-экономической деятельности";
            }
            else if (title == null)
            {
                title = "Отчёты";
            }
            if (!IsInfoenergo)
            {
                //title = title + " (" + db.Connection.GetAlias() + ") " + DepTitle;
            }
            return title;
        }
        public static XElement GetCurrentNavigator(bool compiled = true)
        {
            IList<VSXElement> scheme = (compiled) ? Environment.Manager.GetScheme() : Environment.Manager.GetNativeScheme();
            XElement xcustomer = scheme.Elements(EName.customers).Elements(EName.customer).First(c => c.Attribute(AName.id).Value == customerId);
            string navigator_name = xcustomer.Attribute(AName.navigator).Value;
            XElement xnavigator = scheme.Elements(EName.navigators).Elements(EName.navigator).First(n => n.Attribute(AName.name).Value == navigator_name);
            return xnavigator;
        }
        public static XElement GetNavigator(string navName, bool compiled = true)
        {
            IList<VSXElement> scheme = (compiled) ? Environment.Manager.GetScheme() : Environment.Manager.GetNativeScheme();
            XElement xnavigator = scheme.Elements(EName.navigators).Elements(EName.navigator).First(n => n.Attribute(AName.name).Value == navName);
            return xnavigator;
        }
        public static XElement GetUseReport(string report_name)
        {
            return GetCurrentNavigator().Descendants(EName.usereport).SearchByAttribute(AName.report, report_name);
        }
    }
}