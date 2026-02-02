using System.IO;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    public sealed class VExcelWorkbook
    {
        public readonly XDocument Document;
        public VExcelWorkbook(XDocument xml)
        {
            this.Document = xml;
        }
        public VExcelWorkbook(string filename)
            : this(XDocument.Load(filename))
        {
        }
        public VExcelWorkbook(Stream stream)
            : this(XDocument.Load(stream))
        {
        }
        public int SheetsCount()
        {
            return this.Document.Descendants(VExcelNS.SpreadSheet.Worksheet).Count();
        }
        public VExcelSheet Sheet(int index)
        {
            XElement xml = this.Document.Descendants(VExcelNS.SpreadSheet.Worksheet).ElementAt(index);
            VExcelSheet sheet = new VExcelSheet(xml);
            return sheet;
        }
        public void Save(string filename)
        {
            this.Document.Save(filename);
        }
        public Stream SaveToStream()
        {
            MemoryStream stream = new MemoryStream();
            this.Document.Save(stream);
            stream.Flush();
            stream.Position = 0;
            return stream;
        }
    }
}