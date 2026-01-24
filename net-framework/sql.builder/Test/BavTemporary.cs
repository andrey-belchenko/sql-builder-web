using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.Data;
//using DevExpress.Utils;
//using DevExpress.XtraExport;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Columns;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraPivotGrid;


namespace sql.builder
{
    class BavTemporary
    {
        public static Type numberType = typeof(Decimal);

        public static DataSet getDataSetFromXml(XElement xRoot)
        {
            // берем главную таблицу
            var xTable = (from xel in xRoot.Element("scheme").Elements("table")
                          where xel.Attribute("union") == null
                          select xel).First();
            // ищем узел с данными 
            var xData = xRoot.Element("data");

            var dsReport = new DataSet();

            // рекурсивно заполняем и настраиваем отображение в гриде
            getDataTableFromXml(ref dsReport, xTable, xData);

            return dsReport;
        }

        public static void prepareGridControl(ref GridControl grid, DataTable dataTable)
        {
            // Для главной таблицы уже есть view, чистим побочные, но оставляем главный
            var main_view = (BandedGridView)grid.MainView;
            grid.ViewCollection.Clear();
            grid.ViewCollection.Add(main_view);

            // рекурсивно настраиваем отображение в гриде
            fillGridViewsTree(ref grid, null, ref main_view, dataTable);
        }
        public static void preparePivotGridControl(ref PivotGridControl pivot_grid, DataTable dt)
        {
            pivot_grid.Fields.Clear();

            foreach (DataColumn dt_column in dt.Columns)
            {
                var pivot_field = pivot_grid.Fields.Add();

                pivot_field.Name = "col_" + dt_column.ColumnName;
                pivot_field.FieldName = dt_column.ColumnName;
                pivot_field.Caption = dt_column.Caption;
                pivot_field.Visible = (pivot_field.FieldName != pivot_field.Caption);
                pivot_field.Area = PivotArea.FilterArea;

                // если числовая колонка и есть сведения о формате
                if (dt_column.DataType == typeof(Decimal))
                {
                    pivot_field.CellFormat.FormatType = FormatType.Numeric;

                    string format = (string)dt_column.ExtendedProperties["format"];
                    if (format != String.Empty)
                    {
                        pivot_field.CellFormat.FormatString = format;
                    }

                    //pivot_field.SummaryItem.SummaryType = SummaryItemType.Sum;
                }
            }
        }

        #region Закрытые методы
        private static void fillGridViewsTree(ref GridControl grid, BandedGridView parent_view, ref BandedGridView view, DataTable dt)
        {
            // настраиваем GridView для таблицы
            prepareBandedGridView(ref view, dt);

            // если view не является основным, добавляем новый view в иерархию грида
            if (parent_view != null)
            {
                // для родительской view добавляем дочернюю
                grid.LevelTree.Find(parent_view).Nodes.Add(new GridLevelNode()
                {
                    LevelTemplate = view,
                    RelationName = dt.TableName
                });

                // добавляем новую view в коллекцию грида
                grid.ViewCollection.Add(view);
            }

            // если есть дочерние таблицы
            if (dt.ChildRelations.Count > 0)
            {
                // перебираем все дочерние таблицы
                foreach (DataRelation relation in dt.ChildRelations)
                {
                    // генерируем и настраиваем новую view
                    BandedGridView child_view = createChildView();
                    // формируем view для дочерней таблицы
                    fillGridViewsTree(ref grid, view, ref child_view, relation.ChildTable);
                }

                // если дочерних таблиц больше одной - отображаем закладки
                if (dt.ChildRelations.Count > 1)
                {
                    view.OptionsDetail.ShowDetailTabs = true;
                }

                // если хотябы одна колонка привязана к непустому band-у, отображаем band-ы
                if (dt.Columns.Cast<DataColumn>().Any(column => (string)column.ExtendedProperties["bands_info"] != "empty"))
                {
                    view.OptionsView.ShowBands = true;
                }
            }
        }

        private static DataTable getDataTableFromXml(ref DataSet ds, XElement xtable, XElement xdata)
        {
            // формируем DataTable с псевдонимом таблицы
            var dt = new DataTable()
            {
                TableName = xtable.Attribute("as").Value
            };

            // получаем описание колонок и заполняем по нему колонки в DataTable
            fillTableColumnsFromXml(ref dt, xtable.Element("columns"));
            // получаем данные и заполняем ими DataTable
            fillTableDataFromXml(ref dt, xdata);

            // добавляем таблицу в DataSet
            ds.Tables.Add(dt);

            // если есть дочерние таблицы
            if (xtable.Element("childs") != null)
            {
                // перебираем все дочерние таблицы
                foreach (var xchild_table in xtable.Element("childs").Elements("table"))
                {
                    // формируем дочернюю таблицу
                    var dt_child = getDataTableFromXml(ref ds, xchild_table, xdata);
                    // добавляем связь в DataSet
                    DataRelation dr = ds.Relations.Add(dt.Columns["sid"], dt_child.Columns["sparentid"]);
                    dr.RelationName = dt_child.TableName;
                }
            }

            return dt;
        }

        private static void fillTableColumnsFromXml(ref DataTable dt, XElement xcolumns, string bands_info = "empty")
        {
            foreach (var xcolumn in xcolumns.Elements())
            {
                if (xcolumn.Name == "column")
                {
                    var col = new DataColumn(xcolumn.Attribute("name").Value);

                    switch (xcolumn.Attribute("type").Value)
                    {
                        case "number":
                            col.DataType = typeof(Decimal);
                            break;
                        case "bool":
                            col.DataType = typeof(Decimal);
                            break;
                        case "date":
                            col.DataType = typeof(DateTime);
                            break;
                        default:
                            col.DataType = typeof(String);
                            break;
                    }

                    if (xcolumn.Attribute("title") != null && xcolumn.Attribute("hidden") == null)
                    {
                        col.Caption = xcolumn.Attribute("title").Value;
                    }
                    else
                    {
                        // временное решение , не показываются колонки у которых заголовок равен имени (считаем что нет заголовка)
                        col.Caption = col.ColumnName;
                    }

                    // дополнительные сведения
                    // формат
                    col.ExtendedProperties.Add("format", (string)xcolumn.Attribute("format") ?? String.Empty);
                    // информация по Band-ам
                    col.ExtendedProperties.Add("bands_info", bands_info);

                    dt.Columns.Add(col);
                }
                else if (xcolumn.Name == "band")
                {
                    fillTableColumnsFromXml(ref dt, xcolumn, String.Format("{0};{1}", bands_info, xcolumn.Attribute("title").Value));
                }
            }

        }
        private static void fillTableDataFromXml(ref DataTable dt, XElement xdata)
        {
            var table_name = dt.TableName;
            // Достаем описание всех строк для таблицы
            var xtrs = (from el in xdata.Descendants("table")
                        where el.Attribute("as").Value == table_name
                        select el.Element("data").Elements("tr"))
                .SelectMany(tr => tr);

            // заполняем таблицу данными
            foreach (var xtr in xtrs)
            {
                var row = dt.NewRow();
                int col_num = 0;

                // Перебираем данные, добавляя их по порядку в DataRow, попутно приводя к типу колонки
                foreach (var xtd in xtr.Element("cells").Elements("td"))
                {
                    // пустые строки надо преобразовывать в 0 для числового типа
                    if (dt.Columns[col_num].DataType == numberType && String.IsNullOrEmpty(xtd.Value))
                    {
                       // xtd.Value = "0";
                    }

                    if (!xtd.Value.Equals(""))
                    {
                        row[col_num] = Convert.ChangeType(xtd.Value, dt.Columns[col_num].DataType);
                    }
                    col_num++;
                }

                dt.Rows.Add(row);
            }
        }

        private static void prepareBandedGridView(ref BandedGridView view, DataTable dt)
        {
            view.Columns.Clear();
            view.Bands.Clear();
            view.GroupSummary.Clear();
            if (dt.TableName == "a1")
            {

            }
            // формируем колонки на основе DataTable
            view.PopulateColumns(dt);
            // создаем пустой Band и приписываем к нему все колонки
            var empty_band = view.Bands.Add();

            var current_band = empty_band;

            foreach (BandedGridColumn column in view.Columns)
            {
                var dt_column = dt.Columns[column.FieldName];

                column.Name = column.FieldName;
                column.Caption = dt_column.Caption;
                column.Visible = (column.Name != column.Caption);

                // получаем список бэндов, к которым привязана колонка в виде "empty1;band1;band2 ..."
                var column_band_names = ((string)dt_column.ExtendedProperties["bands_info"]).Split(';');

                // уровень текущего бэнда. 1 - самый верхний
                int cur_level = current_band.BandLevel + 1;

                // если число бэндов, к которым привязана колонка, больше уровня текущего бэнда
                if (column_band_names.Length > cur_level)
                {
                    // создаем для каждого уровня новый бэнд
                    for (int i = 0; i < column_band_names.Length - cur_level; i++)
                    {
                        // если текущий бэнд empty, цепляем новый бэнд к view
                        if (current_band != empty_band) current_band = current_band.Children.Add();
                        else current_band = view.Bands.Add();

                        current_band.Caption = column_band_names[cur_level + i];
                    }
                }
                else if (column_band_names.Length < cur_level)
                {
                    for (int i = 0; i < cur_level - column_band_names.Length; i++)
                    {
                        if (current_band != empty_band) current_band = current_band.ParentBand;
                        //else current_band = empty_band;
                    }
                }
                else if (current_band.Caption != column_band_names.Last())
                {
                    if (current_band.ParentBand != null) current_band = current_band.ParentBand.Children.Add();
                    else current_band = empty_band;

                    current_band.Caption = column_band_names.Last();
                }

                // если числовая колонка и есть сведения о формате
                if (dt_column.DataType == typeof(Decimal))
                {
                    column.DisplayFormat.FormatType = FormatType.Numeric;

                    string format = (string)dt_column.ExtendedProperties["format"];
                    if (format != String.Empty)
                    {
                        column.DisplayFormat.FormatString = format;
                    }

                    //column.SummaryItem.SummaryType = SummaryItemType.Sum;
                }

                current_band.Columns.Add(column);
            }

            empty_band.Caption = "";
        }

        private static BandedGridView createChildView()
        {
            var view = new BandedGridView();

            view.Appearance.HeaderPanel.Options.UseTextOptions = true;
            view.Appearance.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;

            view.Appearance.BandPanel.Options.UseTextOptions = true;
            view.Appearance.BandPanel.TextOptions.WordWrap = WordWrap.Wrap;

            view.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
            view.AppearancePrint.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;

            view.AppearancePrint.BandPanel.Options.UseTextOptions = true;
            view.AppearancePrint.BandPanel.TextOptions.WordWrap = WordWrap.Wrap;

            view.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
            view.OptionsBehavior.Editable = false;

            view.OptionsView.ColumnAutoWidth = false;
            view.OptionsView.ShowGroupPanel = false;
            view.OptionsView.ShowBands = true;
            //view.OptionsView.ShowFooter = true;
            view.OptionsView.AllowHtmlDrawHeaders = true;
            view.OptionsView.RowAutoHeight = true;

            view.OptionsSelection.MultiSelect = true;
            view.OptionsSelection.MultiSelectMode = GridMultiSelectMode.CellSelect;
            // view.OptionsView.ShowColumnHeaders = false;

            view.OptionsMenu.ShowGroupSummaryEditorItem = true;

            view.OptionsPrint.AutoWidth = false;

            view.OptionsDetail.ShowDetailTabs = false;

            return view;
        }
        #endregion
    }
}
