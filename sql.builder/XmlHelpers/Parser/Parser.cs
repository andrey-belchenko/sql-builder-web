using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
//using DevExpress.Data;
//using DevExpress.DataAccess.Native;
//using DevExpress.Utils;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraPivotGrid;

//using DevExpress.XtraTreeList;
//using DevExpress.XtraTreeList.Columns;
//using infoenergo.core.Extensions;
using sql.builder.Clean.Extensions;
//using sql.builder.Controls.Grids.ReportViewModes;
using sql.builder.DataApi;
using sql.builder.UI;
//using FixedStyle = DevExpress.XtraGrid.Columns.FixedStyle;
//using SummaryItemType = DevExpress.Data.SummaryItemType;

namespace sql.builder.XmlHelpers
{
    //using PivotFieldsByTable = Dictionary<string, IEnumerable<PivotGridField>>;
    //using TreeColumnsByTable = Dictionary<string, IEnumerable<TreeListColumn>>;

    internal partial class Parser
    {
        #region Закрытые переменные
        private static SortedList<string, SortedList<string, List<DataRow>>> _parentIndexes;
        #endregion

        #region ReportGrid.Сохранение
        //public static void SaveGridSettingsToXml(XElement xScheme, GridControl grid)
        //{
        //    //if (xScheme.Element("table") != null) return; // Бельченко 13.01.2015
        //    var top_views = grid.ViewCollection.Cast<BandedGridView>().Where(v => v.ParentView == null);
        //    foreach (BandedGridView view in top_views)
        //    {
        //        PutGridViewToXml(grid, view, xScheme);
        //    }
        //}
        //!!! Вызывать только после SaveGridSettingsToXml(). Отдельно работать не будет,
        // т.к. невозможно определить иерархию таблиц.
        //public static void SavePivotSettingsToXml(XElement xRoot, PivotFieldsByTable fields_by_table)
        //{
        //    var xScheme = xRoot.Element("scheme");

        //    foreach (var fields in fields_by_table)
        //    {
        //        PutPivotFieldsToXml(fields.Value, fields.Key, xScheme);
        //    }
        //}
        //public static void SaveTreeSettingsToXml(XElement xScheme, TreeColumnsByTable columns_by_table)
        //{
        //    //
        //}
        public static void SaveReportDataToXml(XElement xRoot, DataSet ds)
        {
            _parentIndexes = new SortedList<string, SortedList<string, List<DataRow>>>();

            var xData = xRoot.GetOrCreateXElement("data");
            xData.RemoveAll();

            // Прописываем columns для всех таблиц в схеме
            //var xSchemeTables = xRoot.Element("scheme").Descendants("table");
            //foreach (DataTable dt in ds.Tables)
            //{
            //    var xSchemeTable = xSchemeTables.First(xtable => xtable.Attribute("as").Value == dt.TableName);
            //    if (xSchemeTable.Element("columns") != null) continue;

            //    var xSchemeColumns = new XElement("columns");
            //    foreach (DataColumn column in dt.Columns)
            //    {
            //        // дополнительное общее описание колонок
            //        var xSchemeColumn = new XElement("column",
            //            new XAttribute("name", column.ColumnName),
            //            column.ColumnName != column.Caption ? new XAttribute("title", column.Caption) : null,
            //            new XAttribute("type",
            //                (column.DataType == XmlReports.numberType)
            //                    ? "number"
            //                    : (column.DataType == typeof(DateTime) ? "date" : "string")),
            //            new XAttribute("key", dt.PrimaryKey.Contains(column) ? "1" : "0"));
            //        xSchemeColumns.Add(xSchemeColumn);
            //    }
            //    xSchemeTable.Add(xSchemeColumns);
            //}

            // перебираем описания таблиц
            foreach (var xSchemeTable in xRoot.Element("scheme").Elements("table"))
            {
                // каждому описанию сопоставляем DataTable
                var dt = ds.Tables[xSchemeTable.Attribute("as").Value];

                // создаем описание данных таблицы
                var xTable = new XElement("table",
                                new XAttribute(("as"), dt.TableName));

                // записываем данные
                PutDataTableToXml(dt, xSchemeTable, xTable);

                xData.Add(xTable);
            }
        }
        public static void SaveReportParamsToXml(XElement xRoot, UIFormC form)
        {
            if (form == null) return;

            xRoot.Add(form.GetReportParams());
        }
        //public static void SaveReportParamsToXml(XElement xRoot, UIFormC2 form)
        //{
        //    if (form == null) return;

        //    xRoot.Add(form.GetReportParams());
        //}
        internal static void SaveReportInfoToXml(XElement xRoot, Dictionary<string, string> settings, XElement scheme)
        {
            XElement xScheme = new XElement(scheme);
            xScheme.SetAttrValue("repname", settings["repname"]);
            xScheme.SetAttrValue(AName.title, settings["title"]);
            xScheme.SetAttrValue("item_type", settings["item_type"]);
            xScheme.SetAttrValue("original_name", settings["original_name"]);
            xScheme.SetAttrValue(AName.visible, settings["visible"]);
            xRoot.Add(xScheme);
        }
        #endregion
        #region ReportGrid.Загрузка

        //public static void LoadPivotSettingsFromXml(XElement xRoot, PivotFieldsByTable fields_by_table)
        //{
        //    fields_by_table.Clear();

        //    foreach (var xTable in xRoot.Element("scheme").Elements("table"))
        //    {
        //        FillPivotFieldsFromXml(fields_by_table, xTable);
        //    }
        //}
        public static VDataSet LoadReportDataFromXml(XElement xRoot, VDataSet ds = null, string repname = null)
        {
            if (ds == null)
            {
                ds = new VDataSet()
                {
                    Report = !String.IsNullOrEmpty(repname)
                        ? XmlReports.Environment.GetPrecompiledReport(repname)
                        : null
                };
            }

            ds.Relations.Clear();
            foreach (DataTable dt in ds.Tables)
            {
                dt.Constraints.Clear();
            }
            ds.Tables.Clear();

            // ищем узел с данными 
            var xData = xRoot.Element("data");

            // рекурсивно заполняем 
            foreach (var xTable in xRoot.Element("scheme").Elements("table"))
            {
                GetDataTableFromXml(ds, xTable, xData);
            }

            // Схему сохраняем отдельно
            ds.Scheme = new VXElement(xRoot.Element("scheme"));

            return ds;
        }
        public static void LoadReportParamsFromXml(XElement xRoot, UIFormC form)
        {
            if (form == null) return;

            var xparams = xRoot.Element("params");
            if (xparams != null)
            {
                //form.SetReportParams(xparams);
                form.RefreshData(xparams);
            }
        }
        internal static Dictionary<string, string> LoadReportInfoFromXml(XElement xRoot)
        {
            var settings = new Dictionary<string, string>();
            XElement xScheme = xRoot.Element(EName.scheme);
            string repname = xScheme.AttrOrEmpty("repname");
            settings.Add("repname", repname);
            settings.Add("title", xScheme.AttrOrEmpty(AName.title));
            settings.Add("form", xScheme.AttrOrEmpty("form"));
            settings.Add("item_type", xScheme.AttrOrEmpty("item_type"));
            settings.Add("is_template", xScheme.AttrOrDefault("is_template", TextConst.AVBool.False));
            settings.Add("original_name", xScheme.AttrOrDefault("original_name", repname));
            settings.Add("visible", xScheme.AttrOrDefault(AName.visible, TextConst.AVBool.True));
            settings.Add("old", false.ToString());
            return settings;
        }
        public static string MainTableFromScheme(XElement xscheme)
        {
            var default_main = xscheme.Descendants("table").FirstOrDefault(table => XmlReports.GetXAttributeValue(table, "main") == "1");
            string main_table = xscheme.Attribute("main") != null
                ? xscheme.Attribute("main").Value
                : default_main != null
                    ? default_main.Attribute("as").Value
                    : xscheme.Descendants("table").First().Attribute("as").Value;
            return main_table;
        }
        public static bool TableSelectFromScheme(XElement xscheme)
        {
            string table_select = xscheme.Attribute("table_select") != null
                ? xscheme.Attribute("table_select").Value
                : xscheme.Descendants("table").Any(el => el.Attribute("title") == null) ? "0" : "1";

            return (table_select == "1");
        }
        public static bool ChildTabsFromScheme(XElement xscheme)
        {
            string child_tabs = xscheme.Attribute("child_tabs") != null
                ? xscheme.Attribute("child_tabs").Value
                : xscheme.Descendants("table").Any(el => el.Attribute("title") == null) ? "0" : "1";

            return (child_tabs == "1");
        }
        #endregion

        #region Прочее
        private static bool IsTable1(XElement table)
        {
            return table.Attribute(AName.@as).Value == "Table1";
        }
        private static bool IsNotTable1(XElement table)
        {
            return table.Attribute(AName.@as).Value != "Table1";
        }
        internal static XElement RepairParams(XElement xpars1, XElement xpars2, XElement xform, bool hide_new_fields = true)
        {
            // берем актуальное описание параметров xpars2 и модифицируем его в соответствии с описанием параметров из шаблона xpars1

            IList<XElement> xsimplecols1 = xpars1.Element(EName.scheme).Elements(EName.table).First(IsTable1).Element(EName.columns).Elements(EName.column).ToList();
            IList<XElement> xsimplecells1 = xpars1.Element(EName.data).Elements(EName.table).First(IsTable1).Element(EName.data).Element(EName.tr).Element(EName.cells).Elements(EName.td).ToList();
            IList<XElement> xarraytables1 = xpars1.Element(EName.scheme).Elements(EName.table).Where(IsNotTable1).ToList();
            IList<XElement> xfields1 = xpars1.Element(EName.content).Descendants(EName.field).ToList();
            IList<XElement> xfieldgroups1 = xpars1.Element(EName.content).Descendants(EName.fieldgroup).ToList();

            IList<XElement> xsimplecols2 = xpars2.Element(EName.scheme).Elements(EName.table).First(IsTable1).Element(EName.columns).Elements(EName.column).ToList();
            IList<XElement> xsimplecells2 = xpars2.Element(EName.data).Elements(EName.table).First(IsTable1).Element(EName.data).Element(EName.tr).Element(EName.cells).Elements(EName.td).ToList();
            IList<XElement> xarraytables2 = xpars2.Element(EName.scheme).Elements(EName.table).Where(IsNotTable1).ToList();
            IList<XElement> xfields2 = xpars2.Element(EName.content).Descendants(EName.field).ToList();
            IList<XElement> xfieldgroups2 = xpars2.Element(EName.content).Descendants(EName.fieldgroup).ToList();
            xsimplecols2 = xsimplecols2.Where(c => !TextConst.AVParamArray.FormExtPars.Contains(c.AttrOrEmpty(AName.name))).ToList();
            // обработка simple
            foreach (XElement xcolumn2 in xsimplecols2)
            {
                string col_name = xcolumn2.Attribute(AName.name).Value;
                // ищем поле
                XElement xcolumn1 = xsimplecols1.SearchByAttribute(AName.name, col_name);
                if (xcolumn1 == null)
                {
                    XElement xfield_native = xform.Descendants(EName.field).First(f => f.Attribute(AName.name).Value == col_name);
                    XAttribute oldnames = xfield_native.Attribute(TextConst.AName.OldNames);
                    if (oldnames != null)
                    {
                        xcolumn1 = xsimplecols1.FirstOrDefault(c => oldnames.Value.Split(',').Contains(c.Attribute(AName.name).Value));
                    }
                }
                if (xcolumn1 != null)
                {
                    // данные подменяем
                    XElement xtd1 = xsimplecells1[xcolumn1.ElementsBeforeSelf(EName.column).Count()];
                    XElement xtd2 = xsimplecells2[xcolumn2.ElementsBeforeSelf(EName.column).Count()];
                    xtd2.ReplaceWith(xtd1);

                    // копируем видимость
                    XElement xfield1 = xfields1.First(e => e.Attribute(AName.name).Value == xcolumn1.Attribute(AName.name).Value || e.Attribute(AName.name).Value == xcolumn1.Attribute(AName.name).Value.TrimEnd('1', '2'));
                    XElement xfield2 = xfields2.First(e => e.Attribute(AName.name).Value == xcolumn2.Attribute(AName.name).Value || e.Attribute(AName.name).Value == xcolumn2.Attribute(AName.name).Value.TrimEnd('1', '2'));

                    xfield2.SetAttributeValue(AName.visible, xfield1.AttrOrDefault(AName.visible, "1"));
                }
                else if (hide_new_fields)
                {  // если в шаблоне не было такого параметра - делаем его невидимым
                    var xfield2 = xfields2.First(e => e.Attribute(AName.name).Value == xcolumn2.Attribute(AName.name).Value || e.Attribute(AName.name).Value == xcolumn2.Attribute(AName.name).Value.TrimEnd('1', '2'));
                    xfield2.SetAttributeValue(AName.visible, "0");
                }
            }
            // обработка array
            foreach (XElement xtable2 in xarraytables2)
            {
                string col_alias = xtable2.Attribute(AName.@as).Value;
                // ищем поле
                XElement xtable1 = xarraytables1.SearchByAttribute(AName.@as, col_alias);
                if (xtable1 == null)
                {
                    XElement xfield_native = xform.Descendants(EName.field).First(f => f.Attribute(AName.name).Value == col_alias);
                    XAttribute oldnames = xfield_native.Attribute(TextConst.AName.OldNames);
                    if (oldnames != null)
                    {
                        xtable1 = xarraytables1.FirstOrDefault(c => oldnames.Value.Split(',').Contains(c.Attribute(AName.@as).Value));
                    }
                }
                if (xtable1 != null)
                {
                    string alias_1 = xtable1.Attribute(AName.@as).Value;
                    string alias_2 = xtable2.Attribute(AName.@as).Value;
                    // данные подменяем
                    IList<XElement> xarraydata1 = xpars1.Element(EName.data).Elements(EName.table).Where(IsNotTable1).ToList();
                    XElement xdata1 = xarraydata1.First(d => d.Attribute(AName.@as).Value == alias_1).Element(EName.data);
                    //
                    IList<XElement> xarraydata2 = xpars2.Element(EName.data).Elements(EName.table).Where(IsNotTable1).ToList();
                    XElement xdata2 = xarraydata2.First(d => d.Attribute(AName.@as).Value == alias_2).Element(EName.data);
                    xdata2.ReplaceWith(xdata1);
                    // копируем видимость
                    XElement xfield1 = xfields1.First(e => e.Attribute(AName.name).Value == alias_1);
                    XElement xfield2 = xfields2.First(e => e.Attribute(AName.name).Value == alias_2);
                    xfield2.SetAttributeValue(AName.visible, xfield1.AttrOrDefault(AName.visible, TextConst.AVBool.True));
                }
                else if (hide_new_fields)
                {   // если в шаблоне не было такого параметра - делаем его невидимым
                    XElement xfield2 = xfields2.First(e => e.Attribute(AName.name).Value == xtable2.Attribute(AName.@as).Value);
                    xfield2.SetAttributeValue(AName.visible, TextConst.AVBool.False);
                }
            }
            // обработка групп
            foreach (var xfieldgroup2 in xfieldgroups2)
            {
                // если у групп хоть один field совпадает, то считаем что это та же группа
                var childnames = xfieldgroup2.Elements(TextConst.EName.Field).Select(f => f.Attribute(TextConst.AName.Name).Value);
                var xfieldgroup1 = xfieldgroups1.FirstOrDefault(fg => fg.Elements(TextConst.EName.Field).Any(f => childnames.Contains(f.Attribute(TextConst.AName.Name).Value)));
                if (xfieldgroup1 == null)
                {
                    // ищем совпадение по заголовку
                    xfieldgroup1 = xfieldgroups1.FirstOrDefault(fg => fg.Attribute(TextConst.AName.Title).Value == xfieldgroup2.Attribute(TextConst.AName.Title).Value);
                }

                if (xfieldgroup1 != null)
                {
                    // копируем видимость
                    xfieldgroup2.SetAttributeValue(TextConst.AName.Visible, xfieldgroup1.AttrOrDef(TextConst.AName.Visible, "1"));
                    // копируем развернутость
                    xfieldgroup2.SetAttributeValue(TextConst.AName.Expanded, xfieldgroup1.AttrOrDef(TextConst.AName.Expanded, "1"));
                }
            }

            return xpars2;
        }
        //internal static XElement RepairScheme(XElement xscheme1, XElement xscheme2)
        //{
        //    var xtables2 = xscheme2.Elements(EName.table);
        //    var xtables1 = xscheme1.Elements(EName.table);
        //    xscheme2.CopyAttributes(xscheme1.Attributes());
        //    foreach (var xtable2 in xtables2)
        //    {
        //        var xtable1 = xtables1.First(t => t.Attribute(TextConst.AName.As).Value == xtable2.Attribute(TextConst.AName.As).Value);
        //        Cmn.CopyAttribute(xtable1, xtable2, "main");

        //        IList<XElement> xviewcolumns2 = xtable2.Element(TextConst.EName.ViewColumns).Descendants(TextConst.EName.Column).ToList();
        //        IList<XElement> xviewcolumns1 = xtable1.Element(TextConst.EName.ViewColumns).Descendants(TextConst.EName.Column).ToList();

        //        foreach (XElement xviewcolumn2 in xviewcolumns2)
        //        {
        //            XElement xviewcolumn1 = xviewcolumns1.SearchByAttribute(AName.name, xviewcolumn2.Attribute(AName.name).Value);
        //            if (xviewcolumn1 == null)
        //            {
        //                XAttribute oldnames = xviewcolumn2.Attribute(TextConst.AName.OldNames);
        //                if (oldnames != null)
        //                {
        //                    xviewcolumn1 = xviewcolumns1.FirstOrDefault(c => oldnames.Value.Split(',').Contains(c.Attribute(TextConst.AName.Name).Value));
        //                }
        //            }
        //            if (xviewcolumn1 != null)
        //            {
        //                Cmn.CopyAttribute(xviewcolumn1, xviewcolumn2, AName.visible);
        //                Cmn.CopyAttribute(xviewcolumn1, xviewcolumn2, AName.width);
        //                Cmn.CopyAttribute(xviewcolumn1, xviewcolumn2, "sort");
        //                Cmn.CopyAttribute(xviewcolumn1, xviewcolumn2, AName.group);
        //            }
        //            else
        //            {
        //                xviewcolumn2.Remove();
        //            }
        //        }
        //        // если можно менять колонки, то заголовки бэндов тоже могут меняться
        //        if (xscheme1.AttrOrDef("params-customization", null) == "1")
        //        {
        //            var xbands2 = xtable2.Element(EName.viewcolumns);
        //            var xbands1 = xtable1.Element(EName.viewcolumns);
        //            xbands2.Descendants(EName.band).Where(e => !e.Descendants(EName.column).Any()).Remove();
        //            xbands1.Descendants(EName.band).Where(e => !e.Descendants(EName.column).Any()).Remove();
        //            RepairBandsTitles(xbands1, xbands2);
        //        }
        //        else
        //        {
        //            IList<XElement> xbands2 = xtable2.Element(EName.viewcolumns).Descendants(EName.band).ToList();
        //            IList<XElement> xbands1 = xtable1.Element(EName.viewcolumns).Descendants(EName.band).ToList();
        //            // обработка бэндов
        //            foreach (XElement xband2 in xbands2)
        //            {
        //                // если у бэндов хоть один column совпадает, то считаем что это тот же бэнд
        //                var childnames = xband2.Elements(EName.column).Select(f => f.Attribute(TextConst.AName.Name).Value);
        //                XElement xband1 = xbands1.FirstOrDefault(fg => fg.Elements(EName.column).Any(f => childnames.Contains(f.Attribute(AName.name).Value)));
        //                if (xband1 == null)
        //                {
        //                    // ищем совпадение по заголовку
        //                    xband1 = xbands1.SearchByAttribute(AName.title, xband2.Attribute(AName.title).Value);
        //                }
        //                if (xband1 == null || !xband1.HasElements || !xband2.HasElements)
        //                {
        //                    xband2.Remove();
        //                }
        //            }
        //        }
        //        // обработка pivot fields
        //        var xpivotfields1 = xtable1.Element(TextConst.EName.PivotFields);
        //        if (xpivotfields1 == null) continue;

        //        var xpivotfields2 = xtable2.Element(TextConst.EName.PivotFields);
        //        if (xpivotfields2 != null) xpivotfields2.ReplaceWith(xpivotfields1);
        //        else xtable2.Add(new XElement(xpivotfields1));

        //        // те поля, которые есть в схеме - обрабатываем
        //        IList<XElement> xpivotfields = xtable2.Element(TextConst.EName.PivotFields).Descendants(EName.field).ToList();
        //        xviewcolumns2 = xtable2.Element(EName.viewcolumns).Descendants(EName.column).ToList();
        //        foreach (XElement xviewcolumn2 in xviewcolumns2)
        //        {
        //            XElement xpivotfield = xpivotfields.SearchByAttribute(AName.name, xviewcolumn2.Attribute(AName.name).Value);
        //            if (xpivotfield == null)
        //            {
        //                XAttribute oldnames = xviewcolumn2.Attribute(TextConst.AName.OldNames);
        //                if (oldnames != null)
        //                {
        //                    xpivotfield = xpivotfields.FirstOrDefault(c => oldnames.Value.Split(',').Contains(c.Attribute(AName.name).Value));
        //                }
        //            }

        //            if (xpivotfield == null)
        //            {
        //                xpivotfield = new XElement(EName.field);
        //                xtable2.Element(TextConst.EName.PivotFields).Element(TextConst.EName.FilterArea).Add(xpivotfield);
        //            }
        //            Cmn.CopyAttribute(xviewcolumn2, xpivotfield, AName.name);
        //            Cmn.CopyAttribute(xviewcolumn2, xpivotfield, AName.type);
        //            string title = (xviewcolumn2.Attribute(AName.title) != null) ? GetPivotColumnNamePrefix(xviewcolumn2) + xviewcolumn2.Attribute(AName.title).Value : null;
        //            xpivotfield.SetAttributeValue(AName.title, title);
        //        }
        //        // поля, которых нет в схеме - выкидываем
        //        string[] names_exist = xviewcolumns2.SelectAsArray(vc => vc.Attribute(AName.name).Value);
        //        xpivotfields.Where(pf => !names_exist.Contains(pf.Attribute(AName.name).Value)).Remove();
        //    }

        //    return xscheme2;
        //}

        //public static void RepairBandsTitles(XElement xroot1, XElement xroot2)
        //{
        //    var xbands2 = xroot2.Elements(TextConst.EName.Band).ToArray();
        //    var xbands1 = xroot1.Elements(TextConst.EName.Band).ToArray();

        //    // обработка бэндов
        //    for (int i = 0; i < xbands2.Length; i++)
        //    {
        //        //if (!xbands2[i].Descendants("column").Any())
        //        //{
        //        //    xbands2[i].Remove();
        //        //    continue;
        //        //}

        //        if (xbands1.IsValidIndex(i))
        //        {
        //            xbands2[i].SetAttributeValue("title", xbands1[i].GetAttributeValue("title"));

        //            RepairBandsTitles(xbands1[i], xbands2[i]);
        //        }
        //    }
        //}
        #endregion

        #region Закрытые методы
        // DataTable
        private static DataTable GetDataTableFromXml(VDataSet ds, XElement xTable, XElement xData)
        {
            string table_name = xTable.Attribute(AName.@as).Value;
            // формируем DataTable с псевдонимом таблицы
            VDataTable dt = (VDataTable)ds.Tables[table_name] ?? new VDataTable(xTable, false, table_name);

            dt.EditableOld = false;
            dt.ExtendedProperties.Add("name", xTable.Attribute("as") != null ? xTable.Attribute("as").Value : String.Empty);
            dt.ExtendedProperties.Add("title", xTable.Attribute("title") != null ? xTable.Attribute("title").Value : String.Empty);

            // получаем описание колонок и заполняем по нему колонки в DataTable
            FillTableColumnsFromXml(dt, xTable.Element("columns"));
            // получаем данные и заполняем ими DataTable
            if (xData != null)
            {
                FillTableDataFromXml(dt, xData);
            }

            // добавляем таблицу в DataSet
            ds.Tables.Add(dt);

            // если есть колонки для связей, формируем связи
            if (dt.Columns.Contains(XmlReports.key_name) && dt.Columns.Contains(XmlReports.parent_key_name))
            {
                // если есть дочерние таблицы
                if (xTable.Element("childs") != null)
                {
                    // перебираем все дочерние таблицы
                    foreach (var xchild_table in xTable.Element("childs").Elements("table"))
                    {
                        // формируем дочернюю таблицу
                        var dt_child = GetDataTableFromXml(ds, xchild_table, xData);
                        // добавляем связь в DataSet
                        DataRelation dr = ds.Relations.Add(dt.Columns[XmlReports.key_name], dt_child.Columns[XmlReports.parent_key_name]);
                        dr.RelationName = dt_child.TableName;
                    }
                }
            }

            return dt;
        }
        private static void PutDataTableToXml(DataTable dt, XElement xSchemeTable, XElement xTable, string sparent = null)
        {
            // колонки для связи
            string key_column = dt.Columns.Contains(XmlReports.key_name) ? XmlReports.key_name : null;
            string parent_column = dt.Columns.Contains(XmlReports.parent_key_name) ? XmlReports.parent_key_name : null;

            var xData = new XElement("data");
            // если колонки sparent нет, просто берем все строки, иначе только те, у которых sparent соответствует указаному
            List<DataRow> rows;

            if (parent_column == null)
            {
                rows = dt.AsEnumerable().ToList();
            }
            else
            {
                if (!_parentIndexes.ContainsKey(dt.TableName))
                {
                    rows = dt.AsEnumerable().OrderBy(row => row[parent_column]).ToList();
                    var parentIndex = new SortedList<string, List<DataRow>>();
                    string spOld = "-1";
                    List<DataRow> rows1 = null;
                    foreach (DataRow row in rows)
                    {

                        string spNew = Cmn.Nvl(row[parent_column], "").ToString();
                        if (spNew != spOld)
                        {
                            if (rows1 != null)
                            {
                                parentIndex.Add(spOld, rows1);
                            }
                            rows1 = new List<DataRow>();

                        }
                        rows1.Add(row);
                        spOld = spNew;
                    }
                    if (rows1 != null)
                    {
                        parentIndex.Add(spOld, rows1);
                    }
                    _parentIndexes.Add(dt.TableName, parentIndex);

                }

                if (_parentIndexes[dt.TableName].ContainsKey(Cmn.Nvl(sparent, "").ToString()))
                {
                    rows = _parentIndexes[dt.TableName][Cmn.Nvl(sparent, "").ToString()];
                }
                else
                {
                    rows = new List<DataRow>();
                }
            }

            //rows = parent_column != null
            //      ? dt.AsEnumerable().Where(row => row[parent_column].Equals((object)sparent ?? DBNull.Value))
            //      : dt.AsEnumerable();


            // перебираем строки 
            foreach (var row in rows)
            {
                // создаем описание строки данных
                var xTr = new XElement("tr",
                    key_column != null ? new XAttribute("id", row[key_column]) : null);

                // создаем описание ячеек
                var xCells = new XElement("cells");

                var xSchemeColumns = xSchemeTable.Element("columns");
                // перебираем колонки из описания схемы таблицы и берем данные из нужной ячейки
                foreach (var xSchemeColumn in xSchemeColumns.Elements("column"))
                {
                    if (!dt.Columns.Contains(xSchemeColumn.Attribute("name").Value)) continue;

                    var value = row[xSchemeColumn.Attribute("name").Value];
                    if (value != DBNull.Value)
                    {
                        // преобразуем в стоку нужного типа
                        switch (xSchemeColumn.Attribute("type").Value)
                        {
                            case "number":
                                value = value.ToString().Replace(',', XmlReports.num_sep);
                                break;
                            case "date":
                                value = ((DateTime)value).ToString(CultureInfo.CurrentCulture);
                                break;
                        }
                    }
                    xCells.Add(new XElement("td", value));
                }
                xTr.Add(xCells);

                if (key_column != null)
                {
                    // Перебираем описания дочерних таблиц, если они есть
                    var xSchemeChilds = xSchemeTable.Element("childs");
                    if (xSchemeChilds != null)
                    {
                        var xChilds = new XElement("childs");
                        foreach (var xSchemeChildTable in xSchemeChilds.Elements("table"))
                        {
                            // Получаем дочерний DataTable
                            var child_dt = dt.DataSet.Tables[xSchemeChildTable.Attribute("as").Value];
                            // создаем описание данных таблицы
                            var xChildTable = new XElement("table",
                                new XAttribute(("as"), child_dt.TableName));
                            // рекурсивно заполняем данные в дочерних таблицах
                            PutDataTableToXml(child_dt, xSchemeChildTable, xChildTable, (string)row[key_column]);
                            xChilds.Add(xChildTable);
                        }
                        xTr.Add(xChilds);
                    }
                }

                xData.Add(xTr);
            }
            xTable.Add(xData);
        }

        private static void FillTableColumnsFromXml(VDataTable dt, XElement xColumns)
        {
            foreach (XElement xcolumn in xColumns.Elements())
            {
                if (xcolumn.Name == EName.column)
                {
                    VDataColumn col = VDataColumn.Create(xcolumn);
                    dt.Columns.Add(col);
                    XAttribute attr = xcolumn.Attribute(AName.@default);
                    if (attr != null)
                    {
                        col.DefaultValue = attr.Value;
                    }
                    if (xcolumn.AttrOrDefault(AName.key, false))
                    {
                        // добавляем колонку в массив ключей
                        dt.PrimaryKey = (new List<DataColumn>(dt.PrimaryKey) { col }).ToArray();
                    }
                    foreach (VDataColumn col1 in dt.PrimaryKey)
                    {
                        col1.AllowDBNull = true;
                    }
                }
                else if (xcolumn.Name == EName.band)
                {
                    FillTableColumnsFromXml(dt, xcolumn);
                }
            }
        }
        private static void FillTableDataFromXml(VDataTable dt, XElement xData)
        {
            var table_name = dt.TableName;

            // Достаем описание всех строк для таблицы
            var xtrs = (from el in xData.Descendants("table")
                        where el.Attribute("as").Value == table_name
                        select el.Element("data").Elements("tr"))
                .SelectMany(tr => tr);

            //int i = 0;
            //int bb = 11;
            // заполняем таблицу данными
            foreach (var xtr in xtrs)
            {
                var row = dt.NewRow();
                int col_num = 0;

                // Перебираем данные, добавляя их по порядку в DataRow, попутно приводя к типу колонки
                foreach (var xtd in xtr.Element("cells").Elements("td"))
                {
                    if (!xtd.Value.Equals(""))
                    {
                        if (dt.Columns[col_num].DataType == XmlReports.numberType)
                        {
                            // для корректной обработки разделителей
                            row[col_num] = Cmn.ToDecimal(xtd.Value);
                        }
                        else
                        {
                            row[col_num] = Convert.ChangeType(xtd.Value, dt.Columns[col_num].DataType);
                        }
                    }
                    col_num++;
                }

                dt.Rows.Add(row);

                //if (i++ > bb) break;
            }
        }

        // DefaultGrid
        //private static void PutGridViewToXml(GridControl grid, BandedGridView view, XElement xParent)
        //{
        //    // ищем узел с таблицей. если нет - создаем его

        //    //var xTable = XmlReports.GetOrCreateXElement(xParent, "table", new[] { new Tuple<string, string>("name", view.ViewCaption), 
        //    //                                                            new Tuple<string, string>("as"  , view.Name) });
        //    var xTable = xParent.Descendants("table").FirstOrDefault(a => a.Attribute("as").Value == view.Name);

        //    if (view == grid.MainView)
        //    {
        //        XmlReports.SetXElementAttribute(xTable, "main", "1");
        //    }

        //    // сохраняем дополнительные параметры колонок
        //    var xViewColumns = xTable.Element("viewcolumns");
        //    foreach (GridColumn column in view.Columns)
        //    {
        //        var xColumn = xViewColumns.Descendants("column").FirstOrDefault(a => a.Attribute("name").Value == column.FieldName);
        //        if (xColumn == null) return;

        //        XmlReports.SetXElementAttribute(xColumn, "width", column.Width.ToString());
        //        if (column.GroupIndex != -1) XmlReports.SetXElementAttribute(xColumn, "group", column.GroupIndex.ToString());
        //        XmlReports.SetXElementAttribute(xColumn, "sort", column.SortOrder.ToString().ToLower());
        //    }

        //    // не актуально 16.01.2015
        //    //var xViewColumns = XmlReports.GetOrCreateXElement(xTable, "viewcolumns");


        //    //foreach (GridBand band in view.Bands)
        //    //{
        //    //    if (band.Caption == "")
        //    //    {
        //    //        var top_columns = view.Columns.Cast<BandedGridColumn>().Where(col => col.OwnerBand == band);
        //    //        putGridColumnsToXml(top_columns, xViewColumns);
        //    //    }
        //    //    else
        //    //    {
        //    //        putGridBandToXml(band, xViewColumns);
        //    //    }
        //    //}

        //    var child_views = grid.ViewCollection.Cast<BandedGridView>().Where(v => v.ParentView == view);
        //    if (child_views.Any())
        //    {
        //        var xChilds = xTable.GetOrCreateXElement("childs");
        //        foreach (BandedGridView child_view in child_views)
        //        {
        //            PutGridViewToXml(grid, child_view, xChilds);
        //        }
        //    }
        //}
        //internal static HorzAlignment SHAllignToDxHallign(string val)
        //{
        //    switch (val)
        //    {
        //        case TextConst.AVHAlign.Left:
        //            return HorzAlignment.Near;
        //        case TextConst.AVHAlign.Right:
        //            return HorzAlignment.Far;
        //        case TextConst.AVHAlign.Center:
        //            return HorzAlignment.Center;
        //        default:
        //            return HorzAlignment.Default;
        //    }
        //}
        //// PivotGrid
        //private static void PutPivotFieldsToXml(IEnumerable<PivotGridField> fields, string table_name, XElement xScheme)
        //{
        //    var xTable = xScheme.Descendants("table")
        //        .FirstOrDefault(xtable => xtable.Attribute("as").Value == table_name);

        //    var xPivotFields = xTable.Element("pivot_fields");
        //    if (xPivotFields == null)
        //    {
        //        xPivotFields = new XElement("pivot_fields");
        //        xTable.Add(xPivotFields);
        //    }

        //    xPivotFields.RemoveAll();

        //    var xFilterArea = new XElement("filter_area");
        //    var filter_fields = fields.Where(field => field.Area == PivotArea.FilterArea);
        //    foreach (var filter_field in filter_fields)
        //    {
        //        PutPivotFieldToXml(filter_field, xFilterArea);
        //    }
        //    xPivotFields.Add(xFilterArea);

        //    var xColumnArea = new XElement("column_area");
        //    var column_fields = fields.Where(field => field.Area == PivotArea.ColumnArea);
        //    foreach (var column_field in column_fields)
        //    {
        //        PutPivotFieldToXml(column_field, xColumnArea);
        //    }
        //    xPivotFields.Add(xColumnArea);

        //    var xRowArea = new XElement("row_area");
        //    var row_fields = fields.Where(field => field.Area == PivotArea.RowArea);
        //    foreach (var row_field in row_fields)
        //    {
        //        PutPivotFieldToXml(row_field, xRowArea);
        //    }
        //    xPivotFields.Add(xRowArea);

        //    var xDataArea = new XElement("data_area");
        //    var data_fields = fields.Where(field => field.Area == PivotArea.DataArea);
        //    foreach (var data_field in data_fields)
        //    {
        //        PutPivotFieldToXml(data_field, xDataArea);
        //    }
        //    xPivotFields.Add(xDataArea);
        //}
        //private static void PutPivotFieldToXml(PivotGridField field, XElement xParent)
        //{
        //    var xField = new XElement("field",
        //            new XAttribute("name", field.FieldName),
        //            field.Caption != String.Empty ? new XAttribute("title", field.Caption) : null,
        //            new XAttribute("width", field.Width));

        //    switch (field.UnboundType)
        //    {
        //        case UnboundColumnType.Decimal:
        //            xField.Add(new XAttribute("type", "number"));
        //            break;
        //        case UnboundColumnType.DateTime:
        //            xField.Add(new XAttribute("type", "date"));
        //            break;
        //        case UnboundColumnType.String:
        //            xField.Add(new XAttribute("type", "string"));
        //            break;
        //        default:
        //            xField.Add(new XAttribute("type", "unknown"));
        //            break;
        //    }

        //    // формат
        //    if (field.CellFormat != null && field.CellFormat.FormatString != String.Empty)
        //    {
        //        xField.Add(new XAttribute(TextConst.AName.Format, field.CellFormat.FormatString));
        //    }

        //    //// группировки
        //    //if (column.GroupIndex != -1)
        //    //{
        //    //    xViewColumn.Add(new XAttribute("group", column.GroupIndex));
        //    //}

        //    //// сортировки
        //    //if (column.SortOrder != ColumnSortOrder.None)
        //    //{
        //    //    xViewColumn.Add(new XAttribute("sort", column.SortOrder.ToString().ToLower()));
        //    //}

        //    xParent.Add(xField);
        //}

        //private static void FillPivotFieldsFromXml(PivotFieldsByTable fields_by_table, XElement xTable)
        //{
        //    // коллекция полей
        //    var fields = new List<PivotGridField>();

        //    var xPivotFields = xTable.Element("pivot_fields");
        //    var xViewColumns = xTable.Element("viewcolumns");

        //    if (xPivotFields != null)
        //    {
        //        // описание нераспределенных полей
        //        var xFilterFields = xPivotFields.Element("filter_area").Elements();
        //        foreach (var xFilterField in xFilterFields)
        //        {
        //            var field = new PivotGridField();
        //            fields.Add(field);

        //            var xViewColumn = xViewColumns.Descendants("column")
        //                .FirstOrDefault(f => f.Attribute("name").Value == xFilterField.Attribute("name").Value);

        //            FillPivotFieldFromXml(field, xFilterField, xViewColumn, PivotArea.FilterArea);
        //        }

        //        // описание полей-колонок
        //        var xColumnFields = xPivotFields.Element("column_area").Elements();
        //        foreach (var xColumnField in xColumnFields)
        //        {
        //            var field = new PivotGridField();
        //            fields.Add(field);

        //            var xViewColumn = xViewColumns.Descendants("column")
        //                .FirstOrDefault(f => f.Attribute("name").Value == xColumnField.Attribute("name").Value);

        //            FillPivotFieldFromXml(field, xColumnField, xViewColumn, PivotArea.ColumnArea);
        //        }

        //        // описание полей-строк
        //        var xRowFields = xPivotFields.Element("row_area").Elements();
        //        foreach (var xRowField in xRowFields)
        //        {
        //            var field = new PivotGridField();
        //            fields.Add(field);

        //            var xViewColumn = xViewColumns.Descendants("column")
        //                .FirstOrDefault(f => f.Attribute("name").Value == xRowField.Attribute("name").Value);

        //            FillPivotFieldFromXml(field, xRowField, xViewColumn, PivotArea.RowArea);
        //        }

        //        // описание полей-данных
        //        var xDataFields = xPivotFields.Element("data_area").Elements();
        //        foreach (var xDataField in xDataFields)
        //        {
        //            var field = new PivotGridField();
        //            fields.Add(field);

        //            var xViewColumn = xViewColumns.Descendants("column")
        //                .FirstOrDefault(f => f.Attribute("name").Value == xDataField.Attribute("name").Value);

        //            FillPivotFieldFromXml(field, xDataField, xViewColumn, PivotArea.DataArea);
        //        }
        //    }
        //    // если нет описания для pivot, берем данные из информации о колонках
        //    else
        //    {
        //        foreach (var xColumn in xTable.Element("viewcolumns").Descendants("column"))
        //        {
        //            var field = new PivotGridField();
        //            fields.Add(field);

        //            string name_prefix = GetPivotColumnNamePrefix(xColumn);
        //            FillPivotFieldFromXml(field, xColumn, xColumn, PivotArea.FilterArea, name_prefix);
        //        }
        //    }

        //    // Добавляем в словарь имя таблицы и коллекцию полей
        //    fields_by_table.Add(xTable.Attribute("as").Value, fields);

        //    // Для дочерних таблиц проделываем то же самое
        //    var xChilds = xTable.Element("childs");
        //    if (xChilds != null)
        //    {
        //        var xChildTables = xChilds.Elements("table");
        //        foreach (XElement xChildTable in xChildTables)
        //        {
        //            FillPivotFieldsFromXml(fields_by_table, xChildTable);
        //        }
        //    }
        //}
        //private static void FillPivotFieldFromXml(PivotGridField field, XElement xField, XElement xViewColumn, PivotArea area, string name_prefix = "")
        //{
        //    field.Name = "_" + xField.Attribute("name").Value;
        //    field.FieldName = xField.Attribute("name").Value;
        //    field.Area = area;

        //    if (xField.Attribute("width") != null)
        //    {
        //        field.Width = Convert.ToInt32(xField.Attribute("width").Value);
        //    }

        //    switch (xField.Attribute("type").Value)
        //    {
        //        case "number":
        //            field.UnboundType = UnboundColumnType.Decimal;
        //            field.CellFormat.FormatType = FormatType.Numeric;
        //            break;
        //        case "date":
        //            field.UnboundType = UnboundColumnType.DateTime;
        //            field.CellFormat.FormatType = FormatType.DateTime;
        //            break;
        //        default:
        //            field.UnboundType = UnboundColumnType.String;
        //            break;
        //    }

        //    if (xField.Attribute(TextConst.AName.Format) != null)
        //    {
        //        field.CellFormat.FormatString = xField.Attribute(TextConst.AName.Format).Value;
        //    }

        //    if (xField.Attribute("sort") != null)
        //    {
        //        switch (xField.Attribute("sort").Value)
        //        {
        //            case "ascending": field.SortOrder = PivotSortOrder.Ascending; break;
        //            case "descending": field.SortOrder = PivotSortOrder.Descending; break;
        //                //default: field.SortOrder = PivotSortOrder.None; break;
        //        }
        //    }

        //    if (xField.Attribute("title") != null)
        //    {
        //        field.Caption = name_prefix + xField.Attribute("title").Value;
        //    }

        //    field.Visible = (xViewColumn != null) && xViewColumn.AttrOrDef("visible", "1") == "1";
        //}

        //public static void LoadTreeSettingsFromXmlOld(XElement xRoot, TreeColumnsByTable columns_by_table)
        //{
        //    columns_by_table.Clear();

        //    foreach (var xTable in xRoot.Element("scheme").Descendants("table"))
        //    {
        //        var columns = new List<TreeListColumn>();
        //        FillTreeColumnsFromXmlOld(columns, xTable);
        //        columns_by_table.Add(xTable.Attribute("as").Value, columns);
        //    }
        //}
        //private static void FillTreeColumnsFromXmlOld(List<TreeListColumn> columns, XElement xTable)
        //{
        //    int visible_index = 0;
        //    foreach (XElement xElement in xTable.Element("viewcolumns").Descendants("column"))
        //    {
        //        var column = new TreeListColumn();
        //        columns.Add(column);

        //        FillTreeColumnFromXmlOld(column, xElement);
        //        if (column.Visible) column.VisibleIndex = visible_index++;
        //    }
        //}
        //private static void FillTreeColumnFromXmlOld(TreeListColumn column, XElement xViewColumn)
        //{
        //    column.FieldName = xViewColumn.Attribute("name").Value;

        //    if (xViewColumn.Attribute("width") != null)
        //    {
        //        column.Width = Convert.ToInt32(xViewColumn.Attribute("width").Value);
        //    }

        //    switch (Cmn.GetAttrValue(xViewColumn.Attribute("type")))
        //    {
        //        case "number":
        //            column.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.Decimal;
        //            column.Format.FormatType = FormatType.Numeric;

        //            column.AllNodesSummary = true;
        //            column.SummaryFooter = DevExpress.XtraTreeList.SummaryItemType.Sum;
        //            if (xViewColumn.Attribute(TextConst.AName.Format) != null)
        //            {
        //                column.SummaryFooterStrFormat = xViewColumn.Attribute(TextConst.AName.Format).Value;
        //            }
        //            break;
        //        case "date":
        //            column.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.DateTime;
        //            column.Format.FormatType = FormatType.DateTime;
        //            //column.ShowButtonMode = DevExpress.XtraTreeList.ShowButtonModeEnum.ShowOnlyInEditor;
        //            break;
        //        default:
        //            column.UnboundType = DevExpress.XtraTreeList.Data.UnboundColumnType.String;
        //            break;
        //    }

        //    if (xViewColumn.Attribute(TextConst.AName.Format) != null)
        //    {
        //        column.Format.FormatString = xViewColumn.Attribute(TextConst.AName.Format).Value;
        //    }

        //    if (xViewColumn.Attribute("title") != null)
        //    {
        //        column.Caption = xViewColumn.Attribute("title").Value;
        //    }

        //    column.Visible = (xViewColumn.AttrOrDef("visible", "1") != "0");
        //    //column.OptionsColumn.AllowEdit = (xViewColumn.AttrOrDef("editable", "1") != "1");

        //    if (XmlReports.GetXAttributeValue(xViewColumn, "node-id") == "1")
        //    {
        //        column.Name = "node-id";
        //    }

        //    if (XmlReports.GetXAttributeValue(xViewColumn, "parent-node-id") == "1")
        //    {
        //        column.Name = "parent-node-id";
        //    }
        //}
        #endregion

        public static string GetPivotColumnNamePrefix(XElement xviewcolumn)
        {
            string name_prefix = "";
            var xParent = xviewcolumn.Parent;
            while (xParent.Name.LocalName == TextConst.EName.Band)
            {
                name_prefix = string.Format(@"{0} \ {1}", Cmn.CutString(xParent.Attribute("title").Value), name_prefix);
                xParent = xParent.Parent;
            }

            return name_prefix;
        }
    }
}