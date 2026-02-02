using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    public static class VExcelNS
    {
        /*<ss:Workbook xmlns="urn:schemas-microsoft-com:office:spreadsheet" xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns:ss="urn:schemas-microsoft-com:office:spreadsheet" xmlns:html="http://www.w3.org/TR/REC-html40">*/

        public static XNamespace ss = "urn:schemas-microsoft-com:office:spreadsheet";
        public static XNamespace o = "urn:schemas-microsoft-com:office:office";
        public static XNamespace x = "urn:schemas-microsoft-com:office:excel";
        public static XNamespace html = "http://www.w3.org/TR/REC-html40";
        public static class SpreadSheet
        {
            // Элементы
            public static XName Cell        = VExcelNS.ss.GetName("Cell");
            public static XName Column      = VExcelNS.ss.GetName("Column");
            public static XName Data        = VExcelNS.ss.GetName("Data");
            public static XName Row         = VExcelNS.ss.GetName("Row");
            public static XName Table       = VExcelNS.ss.GetName("Table");
            public static XName Workbook    = VExcelNS.ss.GetName("Workbook");
            public static XName Worksheet   = VExcelNS.ss.GetName("Worksheet");
            // Аттрибуты
            public static XName AutoFitWidth         = VExcelNS.ss.GetName("AutoFitWidth");
            public static XName ExpandedColumnCount  = VExcelNS.ss.GetName("ExpandedColumnCount");
            public static XName ExpandedRowCount     = VExcelNS.ss.GetName("ExpandedRowCount");
            public static XName Formula              = VExcelNS.ss.GetName("Formula");
            public static XName Index                = VExcelNS.ss.GetName("Index");
            public static XName Hidden               = VExcelNS.ss.GetName("Hidden");
            public static XName HRef                 = VExcelNS.ss.GetName("HRef");
            public static XName MergeAcross          = VExcelNS.ss.GetName("MergeAcross");
            public static XName MergeDown            = VExcelNS.ss.GetName("MergeDown");
            public static XName Name                 = VExcelNS.ss.GetName("Name");
            public static XName Span                 = VExcelNS.ss.GetName("Span");
            public static XName StyleID              = VExcelNS.ss.GetName("StyleID");
            public static XName Type                 = VExcelNS.ss.GetName("Type");
            public static XName Width                = VExcelNS.ss.GetName("Width");
        }
        public static class Excel
        {
            // Элементы
            public static XName PageBreaks  = VExcelNS.x.GetName("PageBreaks");
            public static XName Row         = VExcelNS.x.GetName("Row");
            public static XName RowBreak    = VExcelNS.x.GetName("RowBreak");
            public static XName RowBreaks   = VExcelNS.x.GetName("RowBreaks");
            // Аттрибуты
            public static XName HRefScreenTip   = VExcelNS.x.GetName("HRefScreenTip");
        }
    }
}