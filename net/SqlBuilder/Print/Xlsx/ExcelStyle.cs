using System;
using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    public class ExcelStyle
    {
        private bool is_numeric;
        public bool IsNumeric { get { return this.is_numeric; } }
        public ExcelStyle(XElement xitem)
        {
            XAttribute an = xitem.Attribute("applyNumberFormat");
            XAttribute af = xitem.Attribute("numFmtId");
            // numFmtId = 49 - � ������� ������ ������ '���������'
            // �� ���� ������ Excel ������� ������ ������� ��� ����� ����� applyNumberFormat=1
            // � ����, �� ��� ���, ����� � ����� ������ ����� �� ����� (���� 11,22) - ������������� ��� ��� ����� :\
            this.is_numeric = (an != null && an.Value == "1" && af.Value != "49");
        }
    }
}