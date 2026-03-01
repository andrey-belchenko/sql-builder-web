using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    public class ExcelWorksheetRels : ExcelBaseFile
    {
        private List<ExcelRel> _rels;
        private int last_id;
        public ExcelWorksheetRels(string file_path)
            : base(file_path)
        {
            this._rels = new List<ExcelRel>();
            this.last_id = 0;
            foreach (XElement xrel in this.xml.Root.Elements(ns.Relsp.Relationship))
            {
                string rid = xrel.Attribute(ns.None.Id).Value;
                string type = xrel.Attribute(ns.None.Type).Value;
                string target = xrel.Attribute(ns.None.Target).Value;
                int id = int.Parse(rid.Substring(3));
                if (this.last_id < id)
                {
                    this.last_id = id;
                }
                this._rels.Add(new ExcelRel(rid, type, target));
            }
        }
        //  <Relationship Id="rId1" Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink" Target="start_CryptoApi.ini" TargetMode="External"/>
        public string CreateHyperlinkRel(string target)
        {
            string type = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/hyperlink";
            this.last_id++;
            string rid = "rId" + this.last_id.ToString();
            XElement el = new XElement(ns.Relsp.Relationship);
            el.Add(new XAttribute(ns.None.Id, rid));
            el.Add(new XAttribute(ns.None.Type, type));
            el.Add(new XAttribute(ns.None.Target, target));
            el.Add(new XAttribute(ns.None.TargetMode, "External"));
            this.XmlChanged.Root.Add(el);
            this._rels.Add(new ExcelRel(rid, type, target));
            return rid;
        }
    }
}