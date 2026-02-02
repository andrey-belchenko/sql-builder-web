using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    public class VExcelObject
    {
        public readonly XElement Element;
        protected VExcelObject(XElement element)
        {
            this.Element = element;
        }
    }
}
