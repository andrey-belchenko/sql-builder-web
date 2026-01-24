using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Linq;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx
{
    internal class ExcelContentTypes : ExcelBaseFile
    {
        internal ExcelContentTypes(string file_path)
            : base(file_path)
        {
            Contract.Assert(this.xml.Root.Name == ns.CT.Types);
        }
        internal void AddWorksheet(string file_name)
        {
            string partName = "/xl/worksheets/" + file_name + ".xml";
            string contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml";
            XElement xml = new XElement(ns.CT.Override);
            xml.Add(new XAttribute(ns.None.PartName, partName));
            xml.Add(new XAttribute(ns.None.ContentType, contentType));
            this.XmlChanged.Root.Add(xml);
        }
        internal void DeleteWorksheet(string file_name)
        {
            XElement el = this.XmlChanged.Root.Elements(ns.CT.Override).SearchByAttribute(ns.None.PartName, "/xl/worksheets/" + file_name + ".xml");
            if (el != null) {
                el.Remove();
            }
        }
    }
}