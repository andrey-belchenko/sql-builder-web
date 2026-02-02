using System;
using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    internal static class ns
    {
        internal static XNamespace relsp = "http://schemas.openxmlformats.org/package/2006/relationships";
        internal static XNamespace relsd = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        internal static XNamespace main = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        internal static XNamespace ct = "http://schemas.openxmlformats.org/package/2006/content-types";
        internal static XNamespace cp = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";
        internal static XNamespace dc = "http://purl.org/dc/elements/1.1/";
        internal static void InitXNameStaticFields(XNamespace ns, Type type)
        {
            System.Reflection.FieldInfo[] fields = type.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            for (int index = 0; index < fields.Length; index++) {
                System.Reflection.FieldInfo field_info = fields[index];
                if (field_info.GetValue(null) == null) {
                    field_info.SetValue(null, ns.GetName(field_info.Name));
                }
            }
        }
        internal static class None
        {
            internal static readonly XName activeCell;
            internal static readonly XName bestFit;
            internal static readonly XName ContentType;
            internal static readonly XName count;
            internal static readonly XName customWidth;
            internal static readonly XName f;
            internal static readonly XName hidden;
            internal static readonly XName Id;
            internal static readonly XName max;
            internal static readonly XName min;
            internal static readonly XName name;
            internal static readonly XName outlineLevel;
            internal static readonly XName PartName;
            internal static readonly XName r;
            internal static readonly XName ref_ = XNamespace.None.GetName("ref");
            internal static readonly XName s;
            internal static readonly XName sheetId;
            internal static readonly XName si;
            internal static readonly XName spans;
            internal static readonly XName sqref;
            internal static readonly XName style;
            internal static readonly XName t;
            internal static readonly XName tabSelected;
            internal static readonly XName Target;
            internal static readonly XName TargetMode;
            internal static readonly XName tooltip;
            internal static readonly XName Type;
            internal static readonly XName width;
            internal static readonly XName uniqueCount;
            static None()
            {
                ns.InitXNameStaticFields(XNamespace.None, typeof(ns.None));
            }
        }
        internal static class Main
        {
            internal static readonly XName autoFilter;
            internal static readonly XName breaksCells;
            internal static readonly XName cellXfs;
            internal static readonly XName c;
            internal static readonly XName col;
            internal static readonly XName cols;
            internal static readonly XName conditionalFormatting;
            internal static readonly XName dataValidations;
            internal static readonly XName definedNames;
            internal static readonly XName dimension;
            internal static readonly XName f;
            internal static readonly XName hyperlink;
            internal static readonly XName hyperlinks;
            internal static readonly XName mergeCell;
            internal static readonly XName mergeCells;
            internal static readonly XName row;
            internal static readonly XName selection;
            internal static readonly XName si;
            internal static readonly XName sheet;
            internal static readonly XName sheetData;
            internal static readonly XName sheets;
            internal static readonly XName sheetView;
            internal static readonly XName sheetViews;
            internal static readonly XName sst;
            internal static readonly XName styleSheet;
            internal static readonly XName t;
            internal static readonly XName v;
            internal static readonly XName workbook;
            internal static readonly XName worksheet;
            internal static readonly XName xf;
            static Main()
            {
                ns.InitXNameStaticFields(ns.main, typeof(ns.Main));
            }
        }
        internal static class Relsp
        {
            internal static XName Relationship   = ns.relsp.GetName("Relationship");
            internal static XName Relationships  = ns.relsp.GetName("Relationships");
        }
        internal static class Relsd
        {
            internal static XName id = ns.relsd.GetName("id");
        }
        internal static class CT
        {
            internal static XName Override  = ns.ct.GetName("Override");
            internal static XName Types     = ns.ct.GetName("Types");
        }
        internal static class DC
        {
            internal static XName creator       = ns.dc.GetName("creator");
            internal static XName description   = ns.dc.GetName("description");
        }
        internal static class CP
        {
            internal static XName coreProperties   = ns.cp.GetName("coreProperties");
            internal static XName lastModifiedBy   = ns.cp.GetName("lastModifiedBy");
        }
        internal static class Xml
        {
            internal static XName space = XNamespace.Xml.GetName("space");
        }
    }
}