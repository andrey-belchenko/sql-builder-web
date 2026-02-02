using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    internal sealed class VExcelColumn : VExcelObject
    {
        internal readonly VExcelSheet Sheet;
        internal VExcelColumn(XElement element, VExcelSheet sheet)
            : base(element)
        {
            this.Sheet = sheet;
        }
        internal void Remove()
        {
            VExcelCommon.IncrementIndexAfter(this.Element, -1);
            this.Element.Remove();
        }
    }
}