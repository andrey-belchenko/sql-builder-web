using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    internal sealed class VExcelWorkbook
    {
        internal readonly XDocument Document;
        internal VExcelWorkbook(XDocument xml)
        {
            this.Document = xml;
        }
        internal VExcelWorkbook(string filename)
            : this(XDocument.Load(filename))
        {
        }
        internal VExcelWorkbook(Stream stream)
            : this(XDocument.Load(stream))
        {
        }
        internal int SheetsCount()
        {
            return this.Document.Descendants(VExcelNS.SpreadSheet.Worksheet).Count();
        }
        internal VExcelSheet Sheet(int index)
        {
            XElement xml = this.Document.Descendants(VExcelNS.SpreadSheet.Worksheet).ElementAt(index);
            VExcelSheet sheet = new VExcelSheet(xml);
            return sheet;
        }
        internal void Save(string filename)
        {
            this.Document.Save(filename);
        }
        internal Stream SaveToStream()
        {
            MemoryStream stream = new MemoryStream();
            this.Document.Save(stream);
            stream.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}