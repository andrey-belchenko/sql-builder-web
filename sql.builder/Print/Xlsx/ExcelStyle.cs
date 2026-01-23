using System;
using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    internal class ExcelStyle
    {
        private bool is_numeric;
        internal bool IsNumeric { get { return this.is_numeric; } }
        internal ExcelStyle(XElement xitem)
        {
            XAttribute an = xitem.Attribute("applyNumberFormat");
            XAttribute af = xitem.Attribute("numFmtId");
            // numFmtId = 49 - в формате €чейки указан '“екстовый'
            // не знаю почему Excel считает нужным ставить дл€ таких €чеек applyNumberFormat=1
            // и если, не дай бог, текст в такой €чейке похож на число (типа 11,22) - форматировать его как число :\
            this.is_numeric = (an != null && an.Value == "1" && af.Value != "49");
        }
    }
}