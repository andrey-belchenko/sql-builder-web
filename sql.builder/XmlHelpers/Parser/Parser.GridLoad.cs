//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Globalization;
//using System.Linq;
//using System.Xml.Linq;
////using DevExpress.Data;
////using DevExpress.DataAccess.Native;
////using DevExpress.Utils;
////using DevExpress.XtraGrid;
////using DevExpress.XtraGrid.Columns;
////using DevExpress.XtraGrid.Views.BandedGrid;
////using DevExpress.XtraGrid.Views.Grid;
////using DevExpress.XtraPivotGrid;

////using DevExpress.XtraTreeList;
////using DevExpress.XtraTreeList.Columns;
//using infoenergo.core.Extensions;
//using sql.builder.Controls.Grids.ReportViewModes;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using FixedStyle = DevExpress.XtraGrid.Columns.FixedStyle;
//using SummaryItemType = DevExpress.Data.SummaryItemType;

//namespace sql.builder.XmlHelpers
//{
//    //using PivotFieldsByTable = Dictionary<string, IEnumerable<PivotGridField>>;
//    //using TreeColumnsByTable = Dictionary<string, IEnumerable<TreeListColumn>>;

//    internal partial class Parser
//    {
//        // ReportGrid.Загрузка
//        //public static void LoadGridSettingsFromXml(XElement xScheme, GridControl grid, bool banded = true)
//        //{
//        //    // от утечек
//        //    // https://www.devexpress.com/Support/Center/Question/Details/Q534989
//        //    grid.ViewCollection.Cast<GridView>().ToArray().ForEach(v => v.Dispose());
//        //    foreach (var xTable in xScheme.Element("scheme").Elements("table"))
//        //    {
//        //        GridView view = (banded) ? new BandedGridView() : new GridView();
//        //        GridDesigner.SetViewSettings(view);
//        //        grid.ViewCollection.Add(view);
//        //        if (grid.MainView == null) grid.MainView = view;
//        //        // рекурсивно настраиваем отображение в гриде
//        //        FillGridViewFromXml(grid, view, xTable, banded);
//        //    }
//        //}
//        internal static void LoadGridSettingsFromXml(XElement xScheme, IucGrid grid, bool banded = true)
//        {
//            grid.Clear();
//            foreach (var xTable in xScheme.Elements(EName.table)) {
//                string viewName = xTable.Attribute(AName.@as).Value;
//                grid.CreateView(viewName, banded);
//                // рекурсивно настраиваем отображение в гриде
//                FillGridViewFromXml(grid, viewName, xTable, banded);
//            }
//        }
//        private static void FillGridViewFromXml(IucGrid grid, string viewName, XElement xTable, bool banded)
//        {
//            //BandedGridView bview = view as BandedGridView;
//            //view.BeginUpdate();
//            grid.BeginViewUpdate(viewName);
//            XAttribute attr = xTable.Attribute(AName.title);
//            if (attr == null) {
//                attr = xTable.Attribute(AName.name);
//            }
//            grid.SetViewTitle(viewName, attr.Value);
//            //
//            if (xTable.AttrOrDefault("main", false)) {
//                grid.SetMainView(viewName);
//            }
//            // обязательно должно стоять после grid.MainView = view, иначе глючит 38360
//            grid.ClearViewContent(viewName);
//            XElement view_columns = xTable.Element(EName.viewcolumns);
//            if (banded) {
//                object empty_band = null;
//                foreach (XElement xElement in view_columns.Elements()) {
//                    XName name = xElement.Name;
//                    if (name == EName.column) {
//                        if (empty_band == null)  {
//                            empty_band = grid.CreateBand();
//                            grid.AddBandToView(viewName,empty_band);
//                        }
//                        object column = grid.CreateBandColumn();
//                        grid.AddColumnToBand(empty_band, column);
//                        FillGridColumnFromXml(grid, column, viewName, xElement);
//                    } else if (name == EName.band) {
//                        empty_band = null;
//                        grid.SetShowBands(viewName, true);
//                        object band = grid.CreateBand();
//                        grid.AddBandToView(viewName, band);
//                        FillGridBandFromXml(grid, band, viewName, xElement);
//                    }
//                }
//                grid.AddBandedDummyColumnAndHideEmptyBands(viewName);
//            } else {
//                foreach (XElement xElement in view_columns.Descendants(EName.column)) {
//                    object column = grid.CreateColumn();
//                    grid.AddColumnToView(viewName, column);
//                    FillGridColumnFromXml(grid, column, viewName, xElement);
//                }
//                // grid.AddDummyColumn(viewName);
//            }
//            if (!UIStatic.IsWeb()) {
//                //var cols = (grid as ucGridWF).GetViewByName(viewName).Columns;
//                FillGroupsAndSortingFromXml(grid, viewName, view_columns);
//            }
//            // отдельно заполняем группировки и сортировки
//            XElement xChilds = xTable.Element(EName.childs);
//            if (xChilds != null) {
//                foreach (XElement xChildTable in xChilds.Elements(EName.table)) {
//                    string viewName1 = xChildTable.Attribute(AName.@as).Value;
//                    grid.CreateView(viewName1, banded);
//                    FillGridViewFromXml(grid, viewName1, xChildTable, true);
//                    grid.SetParentView(viewName1, viewName);
//                }
//            }
//            XAttribute pid = xTable.Attribute(TextConst.AName.ParentFieldName);
//            if (pid != null) {
//                grid.SetParentFieldName(pid.Value);
//                object col = grid.GetColumnByFieldName(pid.Value);
//                grid.SetColumnVisible(col, false); // без этого на web колонка остается видимой
//            }
//            grid.EndViewUpdate(viewName);
//        }
//        private static void FillGridBandFromXml(IucGrid grid ,object band, string viewName, XElement xBand)
//        {
//            grid.SetBandTitle(band, xBand.Attribute(AName.title).Value);
//            XAttribute attr = xBand.Attribute(AName.id);
//            if (attr != null) { // для  бендов AName.Name нельзя, удаляются доченние колонки при applyQube
//                grid.SetBandName(band, attr.Value);
//            }
//            attr = xBand.Attribute(AName.width);
//            if (attr != null) {
//                grid.SetBandWidth(band, Convert.ToInt32(attr.Value));
//            }
//            bool is_parent = xBand.Element(EName.band) != null;
//            object empty_band = null;
//            foreach (XElement xElement in xBand.Elements()) {
//                XName name = xElement.Name;
//                if (name == EName.column) {
//                    if (is_parent && empty_band == null) {
//                        empty_band = grid.CreateBand();
//                        grid.AddBandToBand(band, empty_band);                       
//                    }
//                    object column = grid.CreateBandColumn();
//                    FillGridColumnFromXml(grid, column, viewName, xElement);
//                    if (!is_parent) {
//                        grid.AddColumnToBand(band, column);
//                    } else {
//                        grid.AddColumnToBand(empty_band, column);
//                    }
//                } else if (name == EName.band) {
//                    empty_band = null;
//                    object child_band = grid.CreateBand();
//                    grid.AddBandToBand(band, child_band);
//                    FillGridBandFromXml(grid, child_band, viewName, xElement);
//                }
//                grid.AnalizeBandVisibility(band);
//            }
//        }
//        internal static void FillGridColumnFromXml(IucGrid grid, object column, string viewName, XElement xViewColumn)
//        {
//            string name = xViewColumn.Attribute(AName.name).Value;
//            grid.SetColumnFieldName(column, name);
//            if (xViewColumn.AttrOrDefault("node-id", false)) {
//                grid.SetKeyFieldName(name);
//            }
//            if (xViewColumn.AttrOrDefault("parent-node-id", false)) {
//                grid.SetParentFieldName(name);
//            }
//            XAttribute attr = xViewColumn.Attribute(AName.width);
//            if (attr != null) {
//                grid.SetColumnWidth(column, Convert.ToInt32(attr.Value));
//            }
//            string format = xViewColumn.AttrOrDefault(AName.format, null);
//            string type = xViewColumn.AttrOrEmpty(AName.type);
//            //
//            string agg;
//            attr = xViewColumn.Attribute(AName.agg);
//            if (attr != null) { // почему то "no" превратилось в ""
//                agg = attr.Value;
//                if (string.IsNullOrEmpty(agg)) {
//                    agg = TextConst.AVGroup.No;
//                }
//            } else {
//                agg = null;
//            }
//            //
//            grid.SetColumnTypeFormatAndSummary(viewName, column, type, format, agg);
//            string title = xViewColumn.AttrOrEmpty(AName.title);
//            if (!string.IsNullOrEmpty(title)) {
//                grid.SetColumnTitle(column, title);
//            } else {
//                grid.SetColumnTitle(column, name);
//            }
//            grid.SetColumnVisible(column, xViewColumn.AttrOrDefault(AName.visible, TextConst.AVBool.True) != TextConst.AVBool.False);
//			if (xViewColumn.AttrOrDefault(TextConst.AName.InvisibleInColumnChooser, false)) {
//				grid.SetColumnInvisibleInColumnChooser(column);
//			}
//            attr = xViewColumn.Attribute(AName.fixed_side);
//            if (attr != null) {
//                string val = attr.Value;
//                grid.SetColumnExtOption(column, TextConst.AName.FixedSide, val);
//                if (val == TextConst.AVFixedSide.Left) {
//                    grid.SetColumnFixedLeft(column);
//                } else if (val == TextConst.AVFixedSide.Right) {
//                    grid.SetColumnFixedRight(column);
//                }
//                //// не работает для BandedGridView
//                //column.Fixed = (val == TextConst.AVFixedSide.Left) ? FixedStyle.Left :
//                //               (val == TextConst.AVFixedSide.Right) ? FixedStyle.Right :
//                //               FixedStyle.None;
//            }
//            //
//            attr = xViewColumn.Attribute(AName.halign);
//            if (attr != null) {
//                grid.SetColumnHAlign(column, attr.Value);
//            }
//            //if (xViewColumn.Attribute(TextConst.AName.MergeKey) != null)
//            //{
//            //    column.OptionsColumn.AllowMerge = DefaultBoolean.True;
//            //}
//        }
//        //private static void FillGroupsAndSortingFromXml(IEnumerable<GridColumn> columns, XElement xColumns)
//        //{
//        //    // !!! Сделал так, чтобы коректно обрабатывалась ситуация, когда индексы групп колонок идут не по порядку
//        //    // формируем массив картежей с информацией о колонках (имя, индекс группы, сортировка),
//        //    // упорядоченный по возрастанию индекса группы
//        //    var columns_info = xColumns.Descendants("column")
//        //        .Select(xcol => new Tuple<string, int, string>(
//        //            xcol.Attribute("name").Value,
//        //            xcol.Attribute("group") != null ? Convert.ToInt32(xcol.Attribute("group").Value) : -1,
//        //            xcol.Attribute("sort") != null ? xcol.Attribute("sort").Value : "none"))
//        //        .OrderBy(col_info => col_info.Item2);
//        //    foreach (var column_info in columns_info)
//        //    {
//        //        var column = columns.First(col => col.FieldName == column_info.Item1);
//        //        column.GroupIndex = column_info.Item2;
//        //        switch (column_info.Item3)
//        //        {
//        //            case "ascending": column.SortOrder = ColumnSortOrder.Ascending; break;
//        //            case "descending": column.SortOrder = ColumnSortOrder.Descending; break;
//        //            default: column.SortOrder = ColumnSortOrder.None; break;
//        //        }
//        //    }
//        //}
//        private static void FillGroupsAndSortingFromXml(IucGrid grid, string tableName, XElement xColumns)
//        {
//            // !!! Сделал так, чтобы коректно обрабатывалась ситуация, когда индексы групп колонок идут не по порядку
//            // формируем массив картежей с информацией о колонках (имя, индекс группы, сортировка),
//            // упорядоченный по возрастанию индекса группы
//            var columns_info = xColumns.Descendants(EName.column)
//                .Select(xcol => new Tuple<string, int, string>(xcol.Attribute(AName.name).Value, xcol.AttrOrDefault(AName.group, -1), xcol.AttrOrDefault("sort", "none")))
//                .OrderBy(col_info => col_info.Item2);
//            foreach (var column_info in columns_info) {
//                object column = grid.GetColumnByFieldName(tableName, column_info.Item1);
//                grid.SetColumnGroupIndex(column, column_info.Item2);
//                grid.SetColumnSortOrder(column, column_info.Item3);
//            }
//        }
//    }
//}