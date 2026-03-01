using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.Print.Xlsx
{
    public class ExcelSharedStrings : ExcelBaseFile
    {
        [ThreadStatic]
        private static StringBuilder buffer;
        private static string GetSafeExcelText(string text)
        {
            if (buffer == null)
            {
                buffer = new StringBuilder(256);
            }
            Contract.Assume(buffer.Length == 0);
            buffer.Append(text);
            /*buffer.Replace("\x001F", "");
            buffer.Replace("\x001E", "");
            buffer.Replace("\n\r", "\r");
            buffer.Replace("\r\n", "\r");
            buffer.Replace('\n', '\r');*/
            Cmn.RefineExcelText(buffer);
            text = buffer.ToString();
            buffer.Clear();
            return text;
        }
        // <���������� ������, <������� ������, xml �� �������>>
        private Dictionary<string, Tuple<int, XElement>> _strings;
        public ExcelSharedStrings(string file_path)
            : base(file_path)
        {
            XElement root = this.xml.Root;
            Contract.Assume(root.Name == ns.Main.sst);
            XAttribute attr = root.Attribute(ns.None.uniqueCount);
            if (attr != null)
            {
                this._strings = new Dictionary<string, Tuple<int, XElement>>(Convert.ToInt32(attr.Value));
            }
            else
            {
                this._strings = new Dictionary<string, Tuple<int, XElement>>();
            }
            foreach (XElement xsi in root.Elements(ns.Main.si))
            {
                XElement xt = xsi.Element(ns.Main.t);
                string text;  // ����� ��� �����������
                if (xt != null)
                {
                    text = xt.Value;
                }
                else
                {
                    // xsi ����� ��������� ���� r � ����������� ������� - ������� �� ��� �����
                    text = xsi.ToString(SaveOptions.DisableFormatting);
                }
                this._strings.Add(GetSafeExcelText(text), new Tuple<int, XElement>(_strings.Count, new XElement(xsi)));
            }
            root.RemoveNodes();
            this.xml = null;
        }
        public int InternStringAndGetIndex(string text)
        {
            Tuple<int, XElement> info = null;
            text = GetSafeExcelText(text);
            if (!this._strings.TryGetValue(text, out info))
            {
                XElement xsi;
                // ����� � xml �����������
                if (text.StartsWith("<si"))
                {
                    xsi = XElement.Parse(text);
                }
                else
                {
                    // ��������� ������� xml:space = preserve
                    XElement xt = new XElement(ns.Main.t);
                    xt.Add(new XAttribute(ns.Xml.space, "preserve"));
                    xt.Add(new XText(text));
                    xsi = new XElement(ns.Main.si, xt);
                }
                info = new Tuple<int, XElement>(this._strings.Count, xsi);
                this._strings.Add(text, info);
            }
            return info.Item1;
        }
        public string GetStringByIndex(string index)
        {
            return this._strings.Keys.ElementAt(int.Parse(index));
        }
        /// <summary>
        /// ��������� ��������� xml-���� ������ zip-������ *.xslx
        /// </summary>
        public override void Save()
        {
            // ����� ���������� ���� �� ������������ ����� XmlWriter, 
            // ����� ����� ���� ������������ XElement.WriteTo(XmlWriter)
            /*using (StreamWriter writer = new StreamWriter(this.FilePath)) {
                writer.Write(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?>");
                writer.Write(@"<sst xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"">");
                foreach (Tuple<int, XElement, bool> v in this._strings.Values) {
                    // ������� �������� xml � InternStringAndGetIndex
                    //var xsi = new XElement("si", new XElement("t", new XAttribute(XNamespace.Xml + "space", "preserve"), str));
                    writer.Write(v.Item2.ToString(SaveOptions.DisableFormatting));
                    //v.Item2.Save(writer, SaveOptions.DisableFormatting); // �� ��������, �������� �� �������� ����� xlsx
                }
                writer.Write("</sst>");
                writer.Close();
            }*/
            XmlWriterSettings settings = new XmlWriterSettings();
#if !FRAMEWORK_40
            settings.WriteEndDocumentOnClose = false;
#endif
            using (XmlWriter writer = XmlWriter.Create(this.file_path, settings))
            {
                writer.WriteStartDocument(true);
                writer.WriteStartElement("sst", ns.main.NamespaceName);
                foreach (Tuple<int, XElement> v in this._strings.Values)
                {
                    v.Item2.WriteTo(writer);
                }
                writer.WriteEndElement();
                writer.WriteEndDocument();
                writer.Close();
            }
        }
    }
}