using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    public sealed class VExcelColumn : VExcelObject
    {
        public readonly VExcelSheet Sheet;
        public VExcelColumn(XElement element, VExcelSheet sheet)
            : base(element)
        {
            this.Sheet = sheet;
        }
        public void Remove()
        {
            VExcelCommon.IncrementIndexAfter(this.Element, -1);
            this.Element.Remove();
        }
    }
}