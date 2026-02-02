using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
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

    public partial class Parser
    {
        #region Закрытые переменные
        private static SortedList<string, SortedList<string, List<DataRow>>> _parentIndexes;
        #endregion

        #region ReportGrid.Сохранение
        public static void SaveReportDataToXml(XElement xRoot, DataSet ds)
        {
            _parentIndexes = new SortedList<string, SortedList<string, List<DataRow>>>();

            var xData = xRoot.GetOrCreateXElement("data");
            xData.RemoveAll();
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
        public static void SaveReportInfoToXml(XElement xRoot, Dictionary<string, string> settings, XElement scheme)
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
        public static Dictionary<string, string> LoadReportInfoFromXml(XElement xRoot)
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
        public static XElement RepairParams(XElement xpars1, XElement xpars2, XElement xform, bool hide_new_fields = true)
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