using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    internal static class VExcelNS
    {
        /*<ss:Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" xmlns:html="http://www.w3.org/TR/REC-html40">*/

        internal static XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";
        internal static XNamespace o = "urn:schemas-microsoft-com:office:office";
        internal static XNamespace x = "urn:schemas-microsoft-com:office:excel";
        internal static XNamespace html = "http://www.w3.org/TR/REC-html40";
        internal static class SpreadSheet
        {
            // Элементы
            internal static XName Cell        = VExcelNS.ss.GetName("Cell");
            internal static XName Column      = VExcelNS.ss.GetName("Column");
            internal static XName Data        = VExcelNS.ss.GetName("Data");
            internal static XName Row         = VExcelNS.ss.GetName("Row");
            internal static XName Table       = VExcelNS.ss.GetName("Table");
            internal static XName Workbook    = VExcelNS.ss.GetName("Workbook");
            internal static XName Worksheet   = VExcelNS.ss.GetName("Worksheet");
            // Аттрибуты
            internal static XName AutoFitWidth         = VExcelNS.ss.GetName("AutoFitWidth");
            internal static XName ExpandedColumnCount  = VExcelNS.ss.GetName("ExpandedColumnCount");
            internal static XName ExpandedRowCount     = VExcelNS.ss.GetName("ExpandedRowCount");
            internal static XName Formula              = VExcelNS.ss.GetName("Formula");
            internal static XName Index                = VExcelNS.ss.GetName("Index");
            internal static XName Hidden               = VExcelNS.ss.GetName("Hidden");
            internal static XName HRef                 = VExcelNS.ss.GetName("HRef");
            internal static XName MergeAcross          = VExcelNS.ss.GetName("MergeAcross");
            internal static XName MergeDown            = VExcelNS.ss.GetName("MergeDown");
            internal static XName Name                 = VExcelNS.ss.GetName("Name");
            internal static XName Span                 = VExcelNS.ss.GetName("Span");
            internal static XName StyleID              = VExcelNS.ss.GetName("StyleID");
            internal static XName Type                 = VExcelNS.ss.GetName("Type");
            internal static XName Width                = VExcelNS.ss.GetName("Width");
        }
        internal static class Excel
        {
            // Элементы
            internal static XName PageBreaks  = VExcelNS.x.GetName("PageBreaks");
            internal static XName Row         = VExcelNS.x.GetName("Row");
            internal static XName RowBreak    = VExcelNS.x.GetName("RowBreak");
            internal static XName RowBreaks   = VExcelNS.x.GetName("RowBreaks");
            // Аттрибуты
            internal static XName HRefScreenTip   = VExcelNS.x.GetName("HRefScreenTip");
        }
    }
}