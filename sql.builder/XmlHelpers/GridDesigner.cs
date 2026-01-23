//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text.RegularExpressions;
////using DevExpress.Data;
////using DevExpress.Utils;
////using DevExpress.Utils.Drawing;
////using DevExpress.XtraEditors.Repository;
////using DevExpress.XtraGrid;
////using DevExpress.XtraGrid.Columns;
////using DevExpress.XtraGrid.Views.BandedGrid;
////using DevExpress.XtraGrid.Views.Grid;
////using DevExpress.XtraTreeList;
////using DevExpress.XtraTreeList.Columns;
//using infoenergo.core.Extensions;
//using sql.builder.DataApi;
//using SummaryItemType = DevExpress.Data.SummaryItemType;

//namespace sql.builder.XmlHelpers
//{
//    internal static class GridDesigner
//    {
//        #region Сравнение отчётов
//        internal static VDataSet CompareDataSets(VDataSet ds1, VDataSet ds2)
//        {
//            // перебираем все таблицы во втором (загруженом) датасете
//            foreach (VDataTable dt2 in ds2.Tables)
//            {
//                if (ds1.Tables.Cast<VDataTable>().Count(e => e.TableName == dt2.TableName) > 0)
//                {
//                    // добавляем в датасет 2 колонки, которые отстутствуют в датасете 1
//                    var dt1 = ds1.Tables[dt2.TableName];
//                    var columns_names1 = dt1.Columns.Cast<VDataColumn>().Select(col => col.ColumnName);
//                    var columns_names2 = dt2.Columns.Cast<VDataColumn>().Select(col => col.ColumnName);
//                    var miss_columns = columns_names1.Except(columns_names2);
//                    foreach (var column_name in miss_columns) {
//                        DataColumn col = dt1.Columns[column_name];
//                        dt2.AddColumn(col.ColumnName, col.DataType);
//                    }
//                    miss_columns = columns_names2.Except(columns_names1);
//                    foreach (var column_name in miss_columns)
//                    {
//                        dt1.Columns.Add(new VDataColumn(dt2.Columns[column_name].ColumnName, dt1.Columns[column_name].DataType));
//                    }

//                    // перебираем колонки в таблице второго датасета
//                    foreach (VDataColumn col2 in dt2.Columns.Cast<VDataColumn>().ToArray())
//                    {
//                        // колонки с ключами не трогаем
//                        if (col2.ColumnName.EqualsAny(XmlReports.key_name, XmlReports.parent_key_name) || dt2.PrimaryKey.Contains(dt2.Columns[col2.ColumnName])) {
//                            continue;
//                        }
//                        // колонка, в которую будет записано альтернативное значение таблицы из первого датасета
//                        dt2.AddColumn(col2.ColumnName, col2.DataType);
//                        // переименовываем такую же колонку во втором датасете
//                        col2.ColumnName += "_comp";
//                    }
//                    dt2.Merge(dt1);
//                }
//            }
//            // Мерджим датасеты: вуаля! данные из первого датасета послушно ложатся во вновь добавленные колонки второго
//            return ds2;
//        }
//        // dict_key_columns - список со списком колонок-ключей. Лучше не придумал. 
//        // В колонках грида не хранится информация о ключевых колонках, поэтому приходится ее передовать отдельно
//        internal static void SetComparedGridView(GridControl grid, Dictionary<string, string[]> dict_key_columns, string timestamp1, string timestamp2)
//        {
//            // перебираем все view грида
//            foreach (BandedGridView grid_view in grid.ViewCollection)
//            {
//                var key_columns = dict_key_columns[grid_view.Name];
//                // будем хранить связь в словаре, пока предварительно не обработаем все колонки
//                var bands_dict = new Dictionary<GridBand, GridBand>();
//                grid_view.OptionsView.ShowBands = true;
//                foreach (BandedGridColumn col in grid_view.Columns.Cast<BandedGridColumn>().Where(c => c.Name != GetDummyColumnName()).ToArray())
//                {
//                    // колонки с ключами не трогаем
//                    if (col.FieldName.EqualsAny(XmlReports.key_name, XmlReports.parent_key_name)
//                        || key_columns.Contains(col.FieldName)
//                        || col.OwnerBand == null) continue;

//                    // делаем специальный бэнд для каждой колонки, в нем будут сидеть три колонки 
//                    // с оригинальным значением, сравниваемым значением и результатом сравнения.
//                    // Даем ему заголовок колонки
//                    var compared_band = new GridBand()
//                    {
//                        Caption = col.Caption,
//                        Visible = col.Visible
//                    };
//                    grid_view.Bands.Add(compared_band);

//                    bands_dict.Add(compared_band, col.OwnerBand);
//                    col.OwnerBand.Columns.Remove(col);
//                    compared_band.Columns.Add(col);

//                    // колонка со сравниваемым значением - копируем параметры оригинальной
//                    var col_comp = new BandedGridColumn()
//                    {
//                        Caption = timestamp2,
//                        FieldName = col.FieldName + "_comp",
//                        Width = col.Width,
//                        Visible = col.Visible,
//                        UnboundType = col.UnboundType
//                    };
//                    compared_band.Columns.Add(col_comp);
//                    // Формат отображения данных
//                    col_comp.DisplayFormat.FormatType = col.DisplayFormat.FormatType;
//                    col_comp.DisplayFormat.FormatString = col.DisplayFormat.FormatString;
//                    if (col_comp.UnboundType == UnboundColumnType.Decimal)
//                    {
//                        col_comp.SummaryItem.SummaryType = SummaryItemType.Sum;
//                        grid_view.GroupSummary.Add(new GridGroupSummaryItem(SummaryItemType.Sum, col_comp.FieldName, col_comp, ""));
//                    }

//                    // Колонка с результатом сравнения
//                    var col_result = new BandedGridColumn()
//                    {
//                        Caption = "(1)!=(2)",
//                        FieldName = col.FieldName + "_result",
//                        Visible = col.Visible,
//                        Width = 44,
//                        UnboundType = UnboundColumnType.Decimal,
//                        ColumnEdit = new RepositoryItemCheckEdit()
//                        {
//                            ValueChecked = Cmn.DECIMAL_ONE,
//                            ValueUnchecked = Cmn.DECIMAL_ZERO
//                        }
//                    };
//                    compared_band.Columns.Add(col_result);
//                    col_result.SummaryItem.SummaryType = SummaryItemType.Sum;
//                    grid_view.GroupSummary.Add(new GridGroupSummaryItem(SummaryItemType.Sum, col_result.FieldName, col_result, ""));

//                    // Меняем начальную колонку
//                    col.Caption = timestamp1;

//                    col_result.UnboundExpression = String.Format("Iif([{0}] != [{1}], 1, 0)", col.FieldName, col_comp.FieldName);
//                }


//                // Добавление бэнда с колонкой, в которой будет результат сравнения по строке
//                var band_row_comp = new GridBand();
//                grid_view.Bands.Insert(0, band_row_comp);

//                var col_row_comp = new BandedGridColumn()
//                {
//                    Caption = "!=",
//                    FieldName = grid_view.Name + "_row_comp",
//                    Width = 30,
//                    Visible = true,
//                    UnboundType = UnboundColumnType.Decimal,
//                    ColumnEdit = new RepositoryItemCheckEdit()
//                    {
//                        ValueChecked = Cmn.DECIMAL_ONE,
//                        ValueUnchecked = Cmn.DECIMAL_ZERO
//                    }
//                };
//                band_row_comp.Columns.Add(col_row_comp);
//                col_row_comp.SummaryItem.SummaryType = SummaryItemType.Sum;
//                grid_view.GroupSummary.Add(new GridGroupSummaryItem(SummaryItemType.Sum, col_row_comp.FieldName, col_row_comp, ""));

//                var str_conditions = grid_view.Columns.Cast<BandedGridColumn>()
//                    .Where(col => col.FieldName.EndsWith("_result")).Select(col => "[" + col.FieldName + "] = 1");
//                col_row_comp.UnboundExpression = String.Format("Iif(({0}), 1, 0)", String.Join(" or ", str_conditions));

//                // когда все колонки обработаны, связываем бэнды
//                foreach (var bands_relation in bands_dict)
//                {
//                    bands_relation.Value.Children.Add(bands_relation.Key);
//                }
//            }
//        }
//        // убрал возможность - больно глючная
//        //public static void RestoreComparedGridView(GridControl grid)
//        //{
//        //    // перебираем все view грида
//        //    foreach (BandedGridView grid_view in grid.ViewCollection)
//        //    {
//        //        // будем хранить связь в словаре, пока предварительно не обработаем все колонки
//        //        var bands_dict = new Dictionary<BandedGridColumn, GridBand>();
//        //        // все колонки view
//        //        foreach (BandedGridColumn col in grid_view.Columns.Cast<BandedGridColumn>().ToArray())
//        //        {
//        //            // колонки с ключами не трогаем
//        //            //if (col.FieldName == _key || col.FieldName == _parent_key) continue;

//        //            var owner_band = col.OwnerBand;
//        //            if (!col.Name.EndsWith("_result") && !col.Name.EndsWith("_comp"))
//        //            {
//        //                col.Caption = owner_band.Caption;
//        //                bands_dict.Add(col, owner_band.ParentBand);
//        //            }
//        //            else
//        //            {
//        //                grid_view.Columns.Remove(col);
//        //            }
//        //            owner_band.Columns.Remove(col);
//        //            if (owner_band.Columns.Count == 0)
//        //            {
//        //                owner_band.ParentBand.Children.Remove(owner_band);
//        //            }
//        //        }

//        //        // grid_view.Columns.Clear();
//        //        // когда все колонки обработаны, связываем бэнды
//        //        foreach (var bands_relation in bands_dict)
//        //        {
//        //            bands_relation.Value.Columns.Add(bands_relation.Key);
//        //        }

//        //        foreach (var grid_band in bands_dict.Values.Distinct())
//        //        {
//        //            grid_band.Width = (int)(grid_band.Width * 0.7);
//        //        }

//        //        grid_view.OptionsView.ShowBands = grid_view.Bands.Count > 1;
//        //    }
//        //}
//        #endregion
//        //public static void SetComplexColumnsCaptions(VDataSet data_set, IEnumerable<GridView> views)
//        //{
//        //    var tables = data_set.Tables.Cast<DataTable>();
//        //    // Маска для поиска в заголовке вхождения [a.id], где 
//        //    // a - идентификатор таблицы
//        //    // id - идентификатор колонки, имя которой должно быть подставлено
//        //    var regex = new Regex(@"\[[a-zA-Z0-9_]{1,30}\.[a-zA-Z0-9_]{1,30}\]");
//        //    // Отбираем все колонки со сложными заголовками
//        //    var parameterezed_columns = views
//        //        .SelectMany(v => v.Columns)
//        //        .Where(c => regex.IsMatch(c.Caption));

//        //    // Перебор колонок
//        //    foreach (var parameterezed_column in parameterezed_columns)
//        //    {
//        //        // Сохроняем оригинальный заголовок колонки
//        //        if (parameterezed_column.Tag == null)
//        //        {
//        //            parameterezed_column.Tag = new Dictionary<string, string>();
//        //        }
//        //        ((Dictionary<string, string>)parameterezed_column.Tag).Add("original_title", parameterezed_column.Caption);

//        //        // Ищем вхождения маски для замены
//        //        foreach (Match match in regex.Matches(parameterezed_column.Caption))
//        //        {
//        //            // Текст типа [a.id] для замены
//        //            var text_replace = match.Value;
//        //            // 0 - таблица; 1 - колонка
//        //            var array = text_replace.Trim('[', ']').Split('.');
//        //            // Ищем view по имени таблицы
//        //            DataTable tbl = tables.FirstOrDefault(v => v.TableName == array[0]);
//        //            //   var grid_view = views.FirstOrDefault(v => v.Name == array[0]);
//        //            if (tbl == null) continue;
//        //            // Ищем колонку в найденой view
//        //            DataColumn col = tbl.Columns.Cast<DataColumn>().FirstOrDefault(c => c.ColumnName == array[1]);
//        //            if (col == null) continue;
//        //            // Подстовляем значение из первой строки в заголовок
//        //            if (tbl.Rows.Count > 0)
//        //            {
//        //                parameterezed_column.Caption = parameterezed_column.Caption.Replace(text_replace, tbl.Rows[0][col].ToString());
//        //            }
//        //            else
//        //            {
//        //                parameterezed_column.Caption = parameterezed_column.Caption.Replace(text_replace, "...");
//        //            }
//        //        }
//        //    }
//        //}

//        //public static void SetGridTopTable(GridControl grid, DataTable dt)
//        //{
//        //    CreateLevelTreeNode(ref grid, dt, null);
//        //}
//        private static void SetAlignmentAndWordWrap(TextOptions options, VertAlignment valign, HorzAlignment halign, WordWrap word_wrap)
//        {
//            options.WordWrap = word_wrap;
//            options.VAlignment = valign;
//            options.HAlignment = halign;
//        }
//        internal static void SetViewSettings(GridView view)
//        {
//            var bview = view as BandedGridView;
//            if (bview != null) {
//                bview.OptionsView.ShowBands = false;
//                bview.Appearance.BandPanel.Options.UseTextOptions = true;
//                bview.OptionsCustomization.AllowBandMoving = false;
//                SetAlignmentAndWordWrap(bview.Appearance.BandPanel.TextOptions, VertAlignment.Center, HorzAlignment.Near, WordWrap.Wrap);
//                //bview.Appearance.BandPanel.TextOptions.WordWrap = WordWrap.Wrap;
//                //bview.Appearance.BandPanel.TextOptions.VAlignment = VertAlignment.Center;
//                //bview.Appearance.BandPanel.TextOptions.HAlignment = HorzAlignment.Near;
//                bview.AppearancePrint.BandPanel.Options.UseTextOptions = true;
//                SetAlignmentAndWordWrap(bview.AppearancePrint.BandPanel.TextOptions, VertAlignment.Center, HorzAlignment.Near, WordWrap.Wrap);
//                //bview.AppearancePrint.BandPanel.TextOptions.WordWrap = WordWrap.Wrap;
//                //bview.AppearancePrint.BandPanel.TextOptions.VAlignment = VertAlignment.Center;
//                //bview.AppearancePrint.BandPanel.TextOptions.HAlignment = HorzAlignment.Near;
//            } else {
//                // не работает с бэндами
//                view.OptionsView.ColumnHeaderAutoHeight = DefaultBoolean.True;
//            }

//            view.Appearance.HeaderPanel.Options.UseTextOptions = true;
//            SetAlignmentAndWordWrap(view.Appearance.HeaderPanel.TextOptions, VertAlignment.Center, HorzAlignment.Center, WordWrap.Wrap);
//            //view.Appearance.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;
//            //view.Appearance.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
//            //view.Appearance.HeaderPanel.TextOptions.VAlignment = VertAlignment.Center;

//            view.AppearancePrint.HeaderPanel.Options.UseTextOptions = true;
//            SetAlignmentAndWordWrap(view.AppearancePrint.HeaderPanel.TextOptions, VertAlignment.Center, HorzAlignment.Center, WordWrap.Wrap);
//            //view.AppearancePrint.HeaderPanel.TextOptions.WordWrap = WordWrap.Wrap;
//            //view.AppearancePrint.HeaderPanel.TextOptions.HAlignment = HorzAlignment.Center;
//            //view.AppearancePrint.HeaderPanel.TextOptions.VAlignment = VertAlignment.Center;

//            view.AppearancePrint.Row.TextOptions.WordWrap = WordWrap.Wrap;

//            view.OptionsBehavior.Editable = false;
//            view.OptionsBehavior.AllowAddRows = DefaultBoolean.False;
//            view.OptionsBehavior.ReadOnly = true;

//            view.OptionsView.ColumnAutoWidth = false;
//            view.OptionsView.ShowGroupPanel = false;
//            view.OptionsView.ShowFooter = true;
//            view.OptionsView.AllowHtmlDrawHeaders = true;
//            view.OptionsView.RowAutoHeight = false;

//            // не менять
//            view.OptionsBehavior.EditorShowMode = EditorShowMode.MouseDown;

//            view.OptionsMenu.ShowGroupSummaryEditorItem = true;

//            view.OptionsPrint.AutoWidth = false;

//            view.OptionsDetail.ShowDetailTabs = false;
//            view.OptionsDetail.AllowZoomDetail = false;

//            view.OptionsCustomization.AllowColumnMoving = XmlReports.IsDeveloperMode();

//            // чтобы double click обрабатывался в режиме EditorShowMode = EditorShowMode.MouseDown
//            view.ForceDoubleClick = true;
//        }
//        //public static void GridColumnsBestHeight(GridView view)
//        //{
//        //    int max_height = 80;
//        //    int nWidthDelta = 14;
//        //    int nHeightDelta = 0;

//        //    int nHeightHeader = 13;
//        //    int nSubtrahend = 23;
//        //    int nWidth = 0;
//        //    int nHeight = 0;

//        //    StringFormat stringFormat = new StringFormat() { Trimming = StringTrimming.None };

//        //    foreach (GridColumn col in view.Columns)
//        //    {
//        //        if (!col.Visible) continue;

//        //        nWidth = col.Width > nSubtrahend ? col.Width - nSubtrahend : col.Width;

//        //        nHeight = (int)col.AppearanceHeader.CalcTextSize(view.GridControl.CreateGraphics(), stringFormat, col.Caption, nWidth + nWidthDelta).Height;
//        //        if (nHeight > max_height) nHeight = max_height;

//        //        nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//        //    }

//        //    nHeightDelta = (nHeightHeader == 13) ? 0 : 6;
//        //    view.ColumnPanelRowHeight = nHeightHeader + nHeightDelta;

//        //    BandedGridView bview = view as BandedGridView;
//        //    if (bview != null)
//        //    {
//        //        nHeightHeader = 13;
//        //        foreach (GridBand band in bview.Bands)
//        //        {
//        //            if (!band.Visible) continue;

//        //            nSubtrahend = 23;
//        //            nWidth = band.Width > nSubtrahend ? band.Width - nSubtrahend : band.Width;

//        //            nHeight = (int)band.AppearanceHeader.CalcTextSize(bview.GridControl.CreateGraphics(), stringFormat, band.Caption, nWidth + nWidthDelta).Height;
//        //            if (nHeight > max_height) nHeight = max_height;

//        //            nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//        //        }

//        //        nHeightDelta = (nHeightHeader == 13) ? 0 : 6;
//        //        bview.BandPanelRowHeight = nHeightHeader + nHeightDelta;
//        //    }
//        //}
//        internal static void TreeColumnsBestHeight(TreeList tree)
//        {
//            int max_height = 80;
//            int nWidthDelta = 14;
//            int nHeightDelta = 0;

//            int nHeightHeader = 13;
//            int nSubtrahend = 23;
//            int nWidth = 0;
//            int nHeight = 0;

//            StringFormat stringFormat = new StringFormat() { Trimming = StringTrimming.None };

//            foreach (TreeListColumn col in tree.Columns)
//            {
//                if (!col.Visible) continue;

//                nWidth = col.Width > nSubtrahend ? col.Width - nSubtrahend : col.Width;

//                nHeight = (int)col.AppearanceHeader.CalcTextSize(tree.CreateGraphics(), stringFormat, col.Caption, nWidth + nWidthDelta).Height;
//                if (nHeight > max_height) nHeight = max_height;

//                nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//            }

//            nHeightDelta = (nHeightHeader == 13) ? 0 : 6;
//            tree.ColumnPanelRowHeight = nHeightHeader + nHeightDelta;

//            nHeightHeader = 13;
//            foreach (TreeListBand band in tree.Bands)
//            {
//                if (!band.Visible) continue;

//                nSubtrahend = 23;
//                nWidth = band.Width > nSubtrahend ? band.Width - nSubtrahend : band.Width;

//                nHeight = (int)band.AppearanceHeader.CalcTextSize(tree.CreateGraphics(), stringFormat, band.Caption, nWidth + nWidthDelta).Height;
//                if (nHeight > max_height) nHeight = max_height;

//                nHeightHeader = nHeight >= nHeightHeader ? nHeight : nHeightHeader;
//            }

//            nHeightDelta = (nHeightHeader == 13) ? 0 : 6;
//            tree.BandPanelRowHeight = nHeightHeader + nHeightDelta;
//        }
//        internal static int GetMinDummyWidth()
//        {
//            return 100;
//        }
//        internal static string GetDummyColumnName()
//        {
//            return "_dummy_";
//        }
//        internal static TreeListColumn CreateDummyTreeColumn()
//        {
//            // чтобы удобно было менять ширину последней колонки
//            var col = new TreeListColumn();
//            col.Name = GetDummyColumnName();
//            col.Visible = true;
//            col.Width = GetMinDummyWidth();
//            col.Caption = " ";
//            col.OptionsColumn.FixedWidth = true;
//            col.OptionsColumn.AllowEdit = false;
//            col.OptionsColumn.AllowMove = false;
//            col.OptionsColumn.AllowSize = false;
//            col.OptionsColumn.AllowSort = false;
//            return col;
//        }
//        /*
//        public static GridColumn CreateDummyGridColumn()
//        {

//            // чтобы удобно было менять ширину последней колонки
//            var col = new GridColumn
//            {
//                Name = GetDummyColumnName(),
//                Visible = true,
//                Width = GetMinDummyWidth(),
//                Caption = " "
//            };
//            col.OptionsColumn.ShowCaption = false;
//            col.OptionsColumn.FixedWidth = true;
//            col.OptionsColumn.AllowEdit = false;
//            col.OptionsColumn.AllowMove = false;
//            col.OptionsColumn.AllowShowHide = false;
//            col.OptionsColumn.AllowSize = false;
//            col.OptionsColumn.AllowGroup = DefaultBoolean.False;
//            col.OptionsColumn.AllowSort = DefaultBoolean.False;

//            return col;
//        }
//        public static BandedGridColumn CreateDummyBGridColumn()
//        {
//            // чтобы удобно было менять ширину последней колонки
//            var col = new BandedGridColumn
//            {
//                Name = GetDummyColumnName(),
//                Visible = true,
//                Width = GetMinDummyWidth(),
//                Caption = " "
//            };
//            col.OptionsColumn.ShowCaption = false;
//            col.OptionsColumn.FixedWidth = true;
//            col.OptionsColumn.AllowEdit = false;
//            col.OptionsColumn.AllowMove = false;
//            col.OptionsColumn.AllowShowHide = false;
//            col.OptionsColumn.AllowSize = false;
//            col.OptionsColumn.AllowGroup = DefaultBoolean.False;
//            col.OptionsColumn.AllowSort = DefaultBoolean.False;

//            return col;
//        }
//        */
//        internal static int GetDefaultGridColumnWidth()
//        {
//            return 75;
//        }
//        /*internal static void ConvertColumnType(DataTable dt, string column_name, Type new_type)
//        {
//            var old_column = dt.Columns[column_name];
//            if (old_column.DataType == new_type) return;
//            int ord = old_column.Ordinal;
//            old_column.ColumnName = column_name + "_old";
//            dt.Columns.Add(new DataColumn(column_name)
//            {
//                Caption = old_column.Caption,
//                DefaultValue = old_column.DefaultValue,
//                DataType = new_type
//            });
//            dt.Columns[column_name].SetOrdinal(ord);
//            foreach (var row in dt.AsEnumerable())
//            {
//                row[column_name] = Convert.ChangeType(row[column_name + "_old"], new_type);
//            }
//            dt.Columns.Remove(column_name + "_old");
//        }*/
//        internal static void DataSourceRightJoin(DataTable old_data, DataTable updated_data, string pk_column)
//        {
//            old_data.Merge(updated_data);
//            // вычисляем удаленные записи и убираем их из списка вручную (Merge() делает full join, а надо right join)
//            // крутизна в том, что фокус не сбрасывается!
//            // обновленный список ID 
//            object[] new_ids = updated_data.Select().Select(row => row[pk_column]);
//            // старый список ID
//            object[] old_ids = old_data.Select().Select(row => row[pk_column]);
//            // ID которые были в старом, но отсутствуют в новом
//            object[] deleted_ids = old_ids.Except(new_ids).ToArray();
//            // записи с ID, отсутствующими в новом
//            DataRow[] deleted_rows = old_data.Select().Where(row => deleted_ids.Contains(row[pk_column])).ToArray();
//            // удаляем эти записи
//            foreach (var row in deleted_rows) {
//                old_data.Rows.Remove(row);
//            }
//        }
//        /*public static Color GetGrayedColor(Color color, int delta = 40)
//        {
//            byte red = color.R < delta ? (byte)0 : (byte)(color.R - delta);
//            byte green = color.G < delta ? (byte)0 : (byte)(color.G - delta);
//            byte blue = color.B < delta ? (byte)0 : (byte)(color.B - delta);
//            return Color.FromArgb(red, green, blue);
//        }*/
//        //#region Закрытые методы
//        //private static void CreateLevelTreeNode(ref GridControl grid, DataTable dt, GridLevelNode parent_node)
//        //{
//        //    var view = grid.ViewCollection.Cast<GridView>().FirstOrDefault(v => v.Name == dt.TableName);
//        //    if (view == null) return;
//        //    GridLevelNode node = null;
//        //    if (parent_node != null)
//        //    {
//        //        node = parent_node.Nodes.Add(dt.TableName, view);
//        //    }
//        //    else
//        //    {
//        //        grid.MainView = view;
//        //        node = grid.LevelTree.Find(grid.MainView);
//        //        node.Nodes.Clear();
//        //    }
//        //    foreach (DataRelation relation in dt.ChildRelations)
//        //    {
//        //        CreateLevelTreeNode(ref grid, relation.ChildTable, node);
//        //    }
//        //}
//        //#endregion
//    }
//}
