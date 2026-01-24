using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    internal class VExcelObject
    {
        internal readonly XElement Element;
        protected VExcelObject(XElement element)
        {
            this.Element = element;
        }
    }
}
