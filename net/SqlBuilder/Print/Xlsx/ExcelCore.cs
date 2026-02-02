using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    internal class ExcelCore : ExcelBaseFile
    {
        public ExcelCore(string file_path)
            : base(file_path)
        {
            Contract.Assert(this.xml.Root.Name == ns.CP.coreProperties);
            XElement root = this.XmlChanged.Root;            
            XElement xCreator = root.Element(ns.DC.creator);
            if (xCreator != null) {
                xCreator.Value = "SqlBuilder";
            }
            XElement xlastModified = root.Element(ns.CP.lastModifiedBy);
            if (xlastModified != null) {
                xlastModified.Value = "SqlBuilder";
            }
            // 14.06.2017 - ����� - ����� � ���������� ������ �� � exe 
            XElement xDescription = root.Element(ns.DC.description);
            if (xDescription == null) {
                xDescription = new XElement(ns.DC.description);
                root.Add(xDescription);
            }
            string db_info = Cmn.GetDBInfo();
            string exe_info = Cmn.GetExeInfo();
            xDescription.Value = db_info + "\r\n" + exe_info;
        }
    }
}