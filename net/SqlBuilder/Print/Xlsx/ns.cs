using System;
using System.Xml.Linq;

namespace sql.builder.Print.Xlsx
{
    public static class ns
    {
        public static XNamespace relsp = "http://schemas.openxmlformats.org/package/2006/relationships";
        public static XNamespace relsd = "http://schemas.openxmlformats.org/officeDocument/2006/relationships";
        public static XNamespace main = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
        public static XNamespace ct = "http://schemas.openxmlformats.org/package/2006/content-types";
        public static XNamespace cp = "http://schemas.openxmlformats.org/package/2006/metadata/core-properties";
        public static XNamespace dc = "http://purl.org/dc/elements/1.1/";
        public static void InitXNameStaticFields(XNamespace ns, Type type)
        {
            System.Reflection.FieldInfo[] fields = type.GetFields(System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Public);
            for (int index = 0; index < fields.Length; index++)
            {
                System.Reflection.FieldInfo field_info = fields[index];
                if (field_info.GetValue(null) == null)
                {
                    field_info.SetValue(null, ns.GetName(field_info.Name));
                }
            }
        }
        public static class None
        {
            public static readonly XName activeCell;
            public static readonly XName bestFit;
            public static readonly XName ContentType;
            public static readonly XName count;
            public static readonly XName customWidth;
            public static readonly XName f;
            public static readonly XName hidden;
            public static readonly XName Id;
            public static readonly XName max;
            public static readonly XName min;
            public static readonly XName name;
            public static readonly XName outlineLevel;
            public static readonly XName PartName;
            public static readonly XName r;
            public static readonly XName ref_ = XNamespace.None.GetName("ref");
            public static readonly XName s;
            public static readonly XName sheetId;
            public static readonly XName si;
            public static readonly XName spans;
            public static readonly XName sqref;
            public static readonly XName style;
            public static readonly XName t;
            public static readonly XName tabSelected;
            public static readonly XName Target;
            public static readonly XName TargetMode;
            public static readonly XName tooltip;
            public static readonly XName Type;
            public static readonly XName width;
            public static readonly XName uniqueCount;
            static None()
            {
                ns.InitXNameStaticFields(XNamespace.None, typeof(ns.None));
            }
        }
        public static class Main
        {
            public static readonly XName autoFilter;
            public static readonly XName breaksCells;
            public static readonly XName cellXfs;
            public static readonly XName c;
            public static readonly XName col;
            public static readonly XName cols;
            public static readonly XName conditionalFormatting;
            public static readonly XName dataValidations;
            public static readonly XName definedNames;
            public static readonly XName dimension;
            public static readonly XName f;
            public static readonly XName hyperlink;
            public static readonly XName hyperlinks;
            public static readonly XName mergeCell;
            public static readonly XName mergeCells;
            public static readonly XName row;
            public static readonly XName selection;
            public static readonly XName si;
            public static readonly XName sheet;
            public static readonly XName sheetData;
            public static readonly XName sheets;
            public static readonly XName sheetView;
            public static readonly XName sheetViews;
            public static readonly XName sst;
            public static readonly XName styleSheet;
            public static readonly XName t;
            public static readonly XName v;
            public static readonly XName workbook;
            public static readonly XName worksheet;
            public static readonly XName xf;
            static Main()
            {
                ns.InitXNameStaticFields(ns.main, typeof(ns.Main));
            }
        }
        public static class Relsp
        {
            public static XName Relationship = ns.relsp.GetName("Relationship");
            public static XName Relationships = ns.relsp.GetName("Relationships");
        }
        public static class Relsd
        {
            public static XName id = ns.relsd.GetName("id");
        }
        public static class CT
        {
            public static XName Override = ns.ct.GetName("Override");
            public static XName Types = ns.ct.GetName("Types");
        }
        public static class DC
        {
            public static XName creator = ns.dc.GetName("creator");
            public static XName description = ns.dc.GetName("description");
        }
        public static class CP
        {
            public static XName coreProperties = ns.cp.GetName("coreProperties");
            public static XName lastModifiedBy = ns.cp.GetName("lastModifiedBy");
        }
        public static class Xml
        {
            public static XName space = XNamespace.Xml.GetName("space");
        }
    }
}