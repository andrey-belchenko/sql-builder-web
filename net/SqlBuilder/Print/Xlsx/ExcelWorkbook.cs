using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Linq;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx
{
    public class ExcelWorkbook : ExcelBaseFile
    {
        public ExcelWorkbook(string file_path)
            : base(file_path)
        {
            Contract.Assert(this.xml.Root.Name == ns.Main.workbook);
        }
        public string GetNativeWorksheetName(string rid)
        {
            XElement xsheet = this.xml.Root.Element(ns.Main.sheets).Elements(ns.Main.sheet).SearchByAttribute(ns.Relsd.id, rid);
            if (xsheet != null) {
                return xsheet.Attribute(ns.None.name).Value;
            } else {
                return null;
            }
        }
        public void ChangeNativeWorksheetName(string rid, string name)
        {
            XElement root = this.XmlChanged.Root;
            XElement xsheet = root.Element(ns.Main.sheets).Elements(ns.Main.sheet).SearchByAttribute(ns.Relsd.id, rid);
            if (xsheet != null) {
                XAttribute attr = xsheet.Attribute(ns.None.name);
                string oldName = attr.Value;
                attr.Value = name;
                oldName += "'!";
                foreach (XElement dn in root.Elements(ns.Main.definedNames).Elements()) {
                    string val = dn.Value;
                    if (val.Contains(oldName)) {
                        string newName = name + "'!";
                        string newVal = val.Replace(oldName, newName);
                        dn.Value = newVal;
                    }
                }
            }
        }
        public void AddWorksheet(string rid, string name, string rid_prev = null)
        {
            XElement xsheets = this.XmlChanged.Root.Element(ns.Main.sheets);
            int sheet_id = xsheets.Elements(ns.Main.sheet).Max(e => int.Parse(e.Attribute(ns.None.sheetId).Value)) + 1;
            XElement xsheet = new XElement(ns.Main.sheet);
            xsheet.Add(new XAttribute(ns.None.name, name));
            xsheet.Add(new XAttribute(ns.None.sheetId, sheet_id));
            xsheet.Add(new XAttribute(ns.Relsd.id, rid));
            XElement xsheet_prev;
            if (rid_prev != null) {
                xsheet_prev = xsheets.Elements(ns.Main.sheet).SearchByAttribute(ns.Relsd.id, rid_prev);
            } else {
                xsheet_prev = null;
            }
            if (xsheet_prev == null) {
                xsheets.Add(xsheet);
            } else {
                xsheet_prev.AddAfterSelf(xsheet);
            }
        }
        public void DeleteWorksheet(string rid)
        {
            XElement xsheet = this.XmlChanged.Root.Element(ns.Main.sheets).Elements(ns.Main.sheet).SearchByAttribute(ns.Relsd.id, rid);
            if (xsheet != null) {
                xsheet.Remove();
            }
        }
    }
}