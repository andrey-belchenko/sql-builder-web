namespace sql.builder.DataApi
{
    /*public sealed partial class VReport
    {
        // См. перевод на LINQ: Compiler.MakeResultScheme()
        private static XElement MakeResultScheme(XElement report, XElement compiled)
        {
            Contract.Assert(compiled != null);
            Contract.Assert(compiled.Name == EName.root);
            #if DEBUG
            string report_name = report.Attribute(AName_.name).Value;
            Stopwatch sw = new Stopwatch();
            sw.Start();
            #endif
            XmlDocument outputDocument = new XmlDocument(); //!!! избавиться от XmlDocument
            XmlNode root = outputDocument.CreateElement("root");
            outputDocument.AppendChild(root);
            XmlNode schemeNode = XmlReports.addChildNode(root, "scheme");
            XmlNode dataNode = XmlReports.addChildNode(root, "data");
            XmlDocument docCompiled = new XmlDocument();
            docCompiled.LoadXml(compiled.ToString());
            XmlDocument docReport = new XmlDocument();
            docReport.LoadXml(report.ToString());
            foreach (XmlNode query in docReport.FirstChild.SelectNodes("queries/query")) {
                XmlReports.getReportQueryScheme(query, docCompiled, schemeNode);
            }
            XmlReports.setColumnsVisibility(outputDocument);
            XmlReports.moveTransposedToParent(outputDocument);
            XmlReports.moveUnitedToParent(outputDocument);
            XElement xscheme = XDocument.Parse(schemeNode.OuterXml).Root;
            if (report.AttrOrDefault("autobands", false)) {
                addBandsForClassTitles(xscheme);
            }
            if (report.AttrOrEmpty(AName_.auto_merge) == TextConst.AVBool.True) {
                foreach (XElement table in xscheme.Descendants(EName.table)) {
                    var colNames = table.Elements(EName.columns).Elements().Select(c => c.Attribute(AName_.name).Value).ToArray();
                    var colToRem = table.Elements(EName.viewcolumns).Descendants(EName.column).Where(c => !colNames.Contains(c.Attribute(AName_.name).Value)).ToArray();
                    colToRem.Remove();
                }
            }
            #if DEBUG
            sw.Stop();
            Debug.WriteLine("VReport.MakeResultScheme() report name=\"" + report_name + "\": " + sw.ElapsedTicks + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
            XElement xscheme_2 = Compiler.MakeResultScheme(report, compiled);
            bool is_equal = XNode.DeepEquals(xscheme, xscheme_2);
            #endif
            return xscheme;
        }
        public static void addBandsForClassTitles(XElement scheme)
        {
            if (!scheme.Descendants().Attributes(AName_.class_title).Any()) {
                return;
            }
            foreach (XElement table in scheme.Descendants(EName.table)) {
                XElement viewColumns = table.Element(EName.viewcolumns);
                string classTitle = string.Empty;
                string prevClassTitle = string.Empty;
                List<XElement> cols = new List<XElement>();
                foreach (XElement col in viewColumns.Elements()) {
                    classTitle = col.AttrOrEmpty(AName_.class_title);
                    if (classTitle != prevClassTitle) {
                        if (prevClassTitle != string.Empty) {
                            XElement band = new XElement(EName.band, new XAttribute(AName_.title, prevClassTitle));
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
                    XElement band = new XElement(EName.band, new XAttribute(AName_.title, prevClassTitle));
                    viewColumns.Add(band);
                    foreach (XElement col1 in cols) {
                        col1.Remove();
                        band.Add(col1);
                    }
                }
            }
        }
    }*/
}