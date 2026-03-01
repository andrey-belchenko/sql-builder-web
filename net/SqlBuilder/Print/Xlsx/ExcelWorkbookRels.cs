using System.Collections.Generic;
//using System.Text.RegularExpressions;
using System.Xml.Linq;
using sql.builder.DataApi;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.Print.Xlsx
{
    public class ExcelWorkbookRels : ExcelBaseFile
    {
        private const string SHEET_TYPE = "http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet";
        private List<ExcelRel> _rels;
        private int last_id;
        private int last_sheet_num;
        public ExcelWorkbookRels(string file_path)
            : base(file_path)
        {
            XElement root = this.xml.Root;
            Contract.Assume(root.Name == ns.Relsp.Relationships);
            this._rels = new List<ExcelRel>();
            this.last_id = 0;
            this.last_sheet_num = 0;
            foreach (XElement xrel in root.Elements(ns.Relsp.Relationship))
            {
                string rid = xrel.Attribute(ns.None.Id).Value;
                string type = xrel.Attribute(ns.None.Type).Value;
                string target = xrel.Attribute(ns.None.Target).Value;
                int id;
                Contract.Assume(rid.StartsWith("rId"));
                if (int.TryParse(rid.Substring(3), out id))
                {
                    if (this.last_id < id)
                    {
                        this.last_id = id;
                    }
                }
                if (type == SHEET_TYPE)
                {
                    //int num = int.Parse(Regex.Match(target, @".*sheet([0-9]+)\.xml").Groups[1].Value);
                    Contract.Assume(target.StartsWith("worksheets/sheet"));
                    Contract.Assume(target.EndsWith(".xml"));
                    int num;
                    if (int.TryParse(target.Substring(16, target.Length - 20), out num))
                    {
                        if (this.last_sheet_num < num)
                        {
                            this.last_sheet_num = num;
                        }
                    }
                }
                this._rels.Add(new ExcelRel(rid, type, target));
            }
        }
        public string GetNativeWorksheetRID(string file_name)
        {
            file_name = file_name.Replace('\\', '/');
            for (int index = 0; index < this._rels.Count; index++)
            {
                ExcelRel r = this._rels[index];
                if (file_name.EndsWith(r.Target))
                {
                    return r.ID;
                }
            }
            return null;
        }
        // rId �� ����� Rels, name - ��� ����� 
        public void CreateWorksheetRel(ExcelWorksheet worksheet, out string rid, out string name)
        {
            this.last_id++;
            rid = "rId" + this.last_id.ToString();
            this.last_sheet_num++;
            name = "sheet" + this.last_sheet_num.ToString();
            string target = "worksheets/" + name + ".xml";
            XElement xml = new XElement(ns.Relsp.Relationship);
            xml.Add(new XAttribute(ns.None.Id, rid));
            xml.Add(new XAttribute(ns.None.Type, SHEET_TYPE));
            xml.Add(new XAttribute(ns.None.Target, target));
            this.XmlChanged.Root.Add(xml);
            this._rels.Add(new ExcelRel(rid, SHEET_TYPE, target));
        }
        public void DeleteWorksheet(string rid)
        {
            XElement el = this.XmlChanged.Root.Elements(ns.Relsp.Relationship).SearchByAttribute("Id", rid);
            if (el != null)
            {
                el.Remove();
            }
            for (int index = 0; index < this._rels.Count; index++)
            {
                ExcelRel r = this._rels[index];
                if (r.ID == rid)
                {
                    this._rels.RemoveAt(index);
                    break;
                }
            }
        }
    }
    public class ExcelRel
    {
        private string id;
        private string type;
        private string target;
        public ExcelRel(string id, string type, string target)
        {
            this.id = id;
            this.type = type;
            this.target = target;
        }
        public string ID { get { return this.id; } }
        public string Type { get { return this.type; } }
        public string Target { get { return this.target; } }
    }
}