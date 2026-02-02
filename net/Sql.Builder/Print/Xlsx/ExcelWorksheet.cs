using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Text.RegularExpressions;
using System.Linq;
using System.Xml.Linq;
//using infoenergo.core.Extensions;
using sql.builder.DataApi;
using sql.builder.Print.Xlsx.RowsProcessors;

namespace sql.builder.Print.Xlsx
{
    internal class ExcelWorksheet : ExcelBaseFile
    {
        private string native_sheet_rid;
        private string native_sheet_name;
        private string native_sheet_filename;
        private List<ExcelRow> _rows;
        private ExcelWorksheetCols cols;
        private ExcelWorksheetMerges merges;
        private MergeDownColumns mergedowncols;
        //internal string NativeSheetID { get; private set; }
        internal string NativeSheetRID { get { return this.native_sheet_rid; } }
        internal string NativeSheetName { get { return this.native_sheet_name; } }
        internal string NativeSheetFileName { get { return this.native_sheet_filename; } }
        internal ExcelWorksheetMerges Merges { get { return this.merges; } }
        internal ExcelWorksheetCols Cols { get { return this.cols; } }
        internal MergeDownColumns MergeDownCols { get { return this.mergedowncols; } }
        internal IList<ExcelRow> Rows
        {
            get { return this._rows; }
        }
        internal ExcelWorksheet(string file_path, ExcelPrintEnv env)
            : base(file_path)
        {
            this._rows = new List<ExcelRow>();
            this.native_sheet_filename = Path.GetFileNameWithoutExtension(file_path);
            this.native_sheet_rid = env.WorkbookRels.GetNativeWorksheetRID(file_path);
            this.native_sheet_name = env.Workbook.GetNativeWorksheetName(this.native_sheet_rid);
            IEnumerable<XElement> xrows = this.xml.Root.Element(ns.Main.sheetData).Elements(ns.Main.row);
            ExcelRow prev = null;
            foreach (XElement xrow in xrows) {
                ExcelRow row = new ExcelRow(xrow, env, prev, this);
                this._rows.Add(row);
                prev = row;
            }
            XElement root = this.XmlChanged.Root;
            IEnumerable<XElement> xselections = root.Element(ns.Main.sheetViews).Elements(ns.Main.sheetView).Elements(ns.Main.selection);
            foreach (XElement xselection in xselections) {
                xselection.SetAttributeValue(ns.None.activeCell, "A1");
                xselection.SetAttributeValue(ns.None.sqref, "A1");
            }
            XElement xmergeCells = root.Element(ns.Main.mergeCells);
            if (xmergeCells == null) {
                this.merges = new ExcelWorksheetMerges();
            } else {
                xmergeCells.Remove();
                this.merges = new ExcelWorksheetMerges(xmergeCells);
            }
            XElement xcols = root.Element(ns.Main.cols);
            if (xcols == null) {
                this.cols = new ExcelWorksheetCols();
            } else {
                xcols.Remove();
                this.cols = new ExcelWorksheetCols(xcols);
            }
            // для печати header-а и footer-а
            root.Element(ns.Main.sheetData).RemoveNodes(); // остаётся только <sheetData/>
            root.Elements(ns.Main.dimension).Remove();
            this.mergedowncols = new MergeDownColumns(this._rows);
        }
        internal override void Save()
        {
            //if (_worksheetRels != null)
            //{
            //    _worksheetRels.Save();
            //}
        }
        private void DeleteColumns(IList<string> cols_to_delete)
        {
            // правим формулы
            foreach (ExcelRow r in this._rows) {
                foreach (ExcelCell c in r.Cells) {
                    if (c.HasFormula) {
                        c.Formula.DeleteColumns(cols_to_delete);
                    }
                }
            }
            // бардак.

            //1 вычисляем новые имена оставшихся колонок и сдвигаем
            var cols_to_rename = new List<Tuple<string, string>>();

            int deleted_cols_count = 0;
            foreach (IGrouping<string, ExcelCell> col_info in GetColumns())
            {
                if (cols_to_delete.Contains(col_info.Key))
                {
                    deleted_cols_count++;
                }
                else
                {
                    // если перед ячейкой были удалённые колонки - её нужно передвинуть
                    string name_old = col_info.Key;
                    string name_new = name_old;

                    if (deleted_cols_count > 0)
                    {
                        // вычисляем новое имя колонки с учётом ранее удалённых
                        int col_num = ExcelUtils.GetColumnNumber(name_new);
                        name_new = ExcelUtils.GetColumnName(col_num - deleted_cols_count);
                    }

                    // переименовать колонку
                    if (name_old != name_new) cols_to_rename.Add(new Tuple<string, string>(name_old, name_new));
                }
            }

            //2 удаляем колонки
            // съезжали shared формулы - не стал разбираться
            // это ужасно, но одно влияет на другое, поэтому сделал кучу циклов - разобраться в зависимостях

            var columns = GetColumns().ToArray();
            // удаляем ячейки колонки
            foreach (string col_name in cols_to_delete) {
                foreach (var c in columns.First(c => c.Key == col_name).ToArray()) {
                    c.Row.DeleteCell(c);
                }
                this.merges.DeleteColumn(col_name);
            }
            foreach (Tuple<string, string> col_name in cols_to_rename) {
                foreach (ExcelRow r in this._rows) {
                    foreach (ExcelCell c in r.Cells) {
                        if (c.CellInfo.ColumnName == col_name.Item1) {
                            c.ChangeColumnName(col_name.Item2);
                        }
                    }
                }
                this.merges.RenameColumn(col_name.Item1, col_name.Item2);
            }

            //foreach (Tuple<string, string> col_name in cols_to_rename)
            //{
            //    Rows.SelectMany(r => r.Cells.Where(c => c.HasColumnFormulaRef(col_name.Item1))).ToArray().ForEach(c => c.RenameColumnFormulaRef(col_name.Item1, col_name.Item2));
            //}

            this.cols.DeleteColumns(cols_to_delete);
        }
        private string[] CopyColumns(string[] cols_to_copy)
        {
            // правим формулы для всех ячеек, кроме копируемых
            foreach (ExcelCell c in this._rows.SelectMany(r => r.Cells.Where(c => c.HasFormula && !cols_to_copy.Contains(c.CellInfo.ColumnName)))) {
                c.Formula.CopyColumns(cols_to_copy);
            }
            List<string> newNames = new List<string>();
            int col_replace_index = ExcelUtils.GetColumnNumber(cols_to_copy.Last());
            string col_replace = ExcelUtils.GetColumnName(++col_replace_index);
            // освобождаем место под новые колонки сдвигом вправо
            MoveColumnsToRight(col_replace, cols_to_copy.Length);
          
            var columns = GetColumns().ToArray();
            // копируем колонки и вставляем на освобождённое место
            foreach (string col_name in cols_to_copy)
            {
                col_replace = ExcelUtils.GetColumnName(col_replace_index++);
                newNames.Add(col_replace);

                // оптимизация
                var cells = columns.First(c => c.Key == col_name);
                foreach (ExcelCell cell in cells)
                {
                    var copy = cell.Row.CopyCell(cell, col_replace);
                    if (copy.HasFormula) copy.Formula.Move(cols_to_copy.Length);
                }
            }

            this.cols.CopyColumns(cols_to_copy);

            return newNames.ToArray();
        }
        private void MoveColumnsToRight(string first_column_name, int delta)
        {
            // колонки справа, которые надо сдвинуть
            var cols_right = GetColumns().SkipWhile(c => c.Key != first_column_name).Reverse().ToArray();
            foreach (IGrouping<string, ExcelCell> col_info in cols_right) {
                var cell = col_info.FirstOrDefault();
                int col_num = (cell != null) ? cell.CellInfo.ColumnID : ExcelUtils.GetColumnNumber(col_info.Key);
                int col_num_new = col_num + delta;
                string name_new = ExcelUtils.GetColumnName(col_num_new);
                foreach (var c in col_info) {
                    c.ChangeColumnName(name_new);
                }
                this.merges.RenameColumn(col_info.Key, name_new);
            }
        }
        /// <summary>
        /// Список объектов [имя_колонки, список её ExcelCell] в порядке слева направо
        /// Пересчитывается при каждом вызове!
        /// </summary>
        /// <returns></returns>
        private IEnumerable<IGrouping<string, ExcelCell>> GetColumns()
        {
            // наборы ExcelCell сгруппированные по именам колонки
            IEnumerable<IGrouping<string, ExcelCell>> cols_info = this._rows
                .SelectMany(r => r.Cells)
                .GroupBy(c => c.CellInfo.ColumnName)
                .OrderBy(g => ExcelUtils.GetColumnNumber(g.Key));

            return cols_info;
        }
        //internal void RemoveColumn(string column_name)
        //{
        //    DeleteColumns(new SortedDictionary<int, string>()
        //    {
        //        {ExcelUtils.GetColumnNumber(column_name), column_name}
        //    });
        //}
        internal void DeleteUnusedColumns(string[] used_variables)
        {
            IList<string> cols_to_delete = new List<string>();
            foreach (IGrouping<string, ExcelCell> col_info in this.GetColumns()) {
                IList<string> col_variables = col_info.SelectMany(c => ExcelUtils.GetVariablesNames(c.Text)).Distinct().ToList();
                foreach (string vv in col_variables) {
                    if (!used_variables.Contains(vv)) {
                        cols_to_delete.Add(col_info.Key);
                        break;
                    }
                }
            }
            if (cols_to_delete.Count > 0) {
                this.DeleteColumns(cols_to_delete);
            }
        }
        // Избавляемся от свёрнутых формул (одна на несколько ячеек идущих подряд), т.к. дико неудобно это обрабатывать при печати
        internal void ExpandRefFormulas()
        {
            // все ячейки, которые хранят формулы для диапазона
            var cells_with_ref = this._rows.SelectMany(r => r.Cells).Where(c => c.FormulaRef != null);
            foreach (ExcelCell cell_with_ref in cells_with_ref) {
                ExcelRefToken fr = cell_with_ref.FormulaRef;
                // ячейки, на которые действует формула
                var cells = this._rows
                    .Where(r => r.RowID >= fr.Cell1.RowID && r.RowID <= fr.Cell2.RowID)
                    .SelectMany(r => r.Cells.Where(c => c.CellInfo.ColumnID >= fr.Cell1.ColumnID && c.CellInfo.ColumnID <= fr.Cell2.ColumnID));
                string formula = cell_with_ref.Xml.Element(ns.Main.f).Value;
                foreach (ExcelCell cell in cells) {
                    // формулу подставляем в ячейку, на которую действует формула, только если в ячейке есть ссылка на ячейку с формулой
                    XElement f = cell.Xml.Element(ns.Main.f);
                    if (f != null && f.AttrOrDefault(ns.None.si, null) == cell_with_ref.Xml.Element(ns.Main.f).Attribute(ns.None.si).Value) {
                        // корректируем формулу и подставляем в ячейку напрямую
                        string formula2 = ExcelUtils.CorrectFormulaReferences(formula, cell.CellInfo.ColumnID - cell_with_ref.CellInfo.ColumnID, cell.CellInfo.RowID - cell_with_ref.CellInfo.RowID);
                        cell.SetFormula(formula2);
                    }
                }
                // убираем информацию, что в ячейке свёрнутая формула
                cell_with_ref.DeleteFormulaRef();
            }
        }
        internal void ProcessPivotColumns(DataSet data, string width_column_name)
        {
            string cbegin_name = null;
            List<IGrouping<string, ExcelCell>> columns_list = new List<IGrouping<string, ExcelCell>>();
            // найденные пивоты - название + набор размазываемых колонок
            List<Tuple<string, IGrouping<string, ExcelCell>[]>> pivots = new List<Tuple<string, IGrouping<string, ExcelCell>[]>>();
            // реализовал простой вариант без вложенных циклов
            foreach (IGrouping<string, ExcelCell> col_info in this.GetColumns()) {
                if (cbegin_name == null) {
                    // ищем ячейку в колонке, нач с cbegin
                    var ci_begin = col_info.Select(c => new Tuple<ExcelCell, Match>(c, Regex.Match(c.Text, "^cbegin:(.*)"))).FirstOrDefault(t => t.Item2.Success);
                    if (ci_begin != null) {
                        cbegin_name = ci_begin.Item2.Groups[1].Value;
                        columns_list.Add(col_info);
                        ci_begin.Item1.SetValue(string.Empty);
                    }
                }
                // если первый if сработал, всё равно надо проверить
                if (cbegin_name != null) {
                    if (!columns_list.Contains(col_info)) {
                        columns_list.Add(col_info);
                    }
                    // ищем ячейку в колонке, нач с cbegin
                    ExcelCell cell_cend = col_info.FirstOrDefault(c => c.Text.StartsWith(string.Format("cend:{0};", cbegin_name)));
                    // нашли конец, можно обрабатывать найденные колонки
                    if (cell_cend != null) {
                        pivots.Add(new Tuple<string, IGrouping<string, ExcelCell>[]>(cbegin_name, columns_list.ToArray()));
                        columns_list.Clear();
                        cbegin_name = null;
                        cell_cend.SetValue(string.Empty);
                    }
                }
            }
            // обрабатываем найденные размазываемые диапазоны в обратном порядке, чтобы не съезжала нумерация колонок
            for (int index = pivots.Count - 1; index >= 0; index--) {
                Tuple<string, IGrouping<string, ExcelCell>[]> pivot = pivots[index];
                DataTable columns_info = data.Tables[pivot.Item1];
                // автоматическая генерация таблицы с измерениями
                // преобразуем старый вариант к новому - используется например в 33324-11, 33324-10
                if (columns_info == null) {
                    columns_info = getColumnsInfoFromPivotInfo(pivot.Item1, ((VDataSet)data).Scheme);
                }
                this.ProcessPivotColumns(pivot.Item2, pivot.Item1, columns_info, width_column_name);
            }
        }
        // размазывание кейсами
        private static DataTable getColumnsInfoFromPivotInfo(string dimName, XElement scheme)
        {
            DataTable columns_info = new DataTable(dimName);
            columns_info.Columns.Add(new DataColumn("pfx", typeof(string)));
            columns_info.Columns.Add(new DataColumn("title", typeof(string)));
            XElement dimvalEl = scheme.Descendants(TextConst.EName.DimensionValues).SearchByAttribute(AName.table, dimName);
            if (dimvalEl != null) {
                List<XElement> xdim_vals = dimvalEl.Elements(TextConst.EName.Val).ToList();
                for (int index = 0; index < xdim_vals.Count; index++) {
                    XElement xval = xdim_vals[index];
                    columns_info.Rows.Add(
                        "_" + xval.Attribute(TextConst.AName.Value).Value.Replace('.', '_').Replace(',', '_').Replace('-','_'),
                        xval.Attribute(AName.title).Value);
                }
            }
            return columns_info;
        }
        private void ProcessPivotColumns(IEnumerable<IGrouping<string, ExcelCell>> cols_info, string cname, DataTable pivot_info, string width_column_name)
        {
            string[] col_names = cols_info.Select(c => c.Key).ToArray();
            if (pivot_info is VDataTable) {
                (pivot_info as VDataTable).ReadAll();
            }
            // нет данных для размазывания
            if (pivot_info.Rows.Count <= 0) {
                this.DeleteColumns(col_names);
            } else {
                // ��������� � �������� ������� ������, � ������� ���� pivot ���������
                var cols_with_vars = new Dictionary<string, Tuple<ExcelCell, string>[]>();
                foreach (DataColumn column in pivot_info.Columns) {
                    string subst_var = "[" + cname + "." + column.ColumnName + "]";
                    Tuple<ExcelCell, string>[] cells_with_title = cols_info
                        .SelectMany(ci => ci)
                        .Where(c => c.Text.Contains(subst_var))
                        .Select(c => new Tuple<ExcelCell, string>(c, c.Text))
                        .ToArray();
                    cols_with_vars.Add(column.ColumnName, cells_with_title);
                }
                DataColumn width_column = pivot_info.Columns[width_column_name];
                // вставляем справа налево, чтобы не заморочиваться с изменением нумерации колонок
                for (int row_index = pivot_info.Rows.Count - 1; row_index >= 0; row_index--) {
                    DataRow pv = pivot_info.Rows[row_index];
                    foreach (var col_info in cols_with_vars) {
                        string subst_var = "[" + cname + "." + col_info.Key + "]";
                        string key_value = pv[col_info.Key].ToString();
                        foreach (var cell_info in col_info.Value) {
                            string text = cell_info.Item2;
                            text = text.Replace(subst_var, key_value);
                            cell_info.Item1.SetValue(text);
                        }
                    }
                    string[] new_col_names;
                    if (pv != pivot_info.AsEnumerable().First()) {
                        new_col_names = this.CopyColumns(col_names);
                    } else {
                        new_col_names = col_names;
                    }
                    if (width_column != null && !pv.IsNull(width_column)) {
                        int val = Convert.ToInt32(pv[width_column]);
                        foreach (string cn in new_col_names) {
                            this.cols.SetColumnWidth(cn, val);
                        }
                    }
                }
            }
            var col_names_pivot = new List<string>();
            int column_index = ExcelUtils.GetColumnNumber(col_names.First());
            for (int i = 0; i < col_names.Length * pivot_info.Rows.Count; i++) {
                col_names_pivot.Add(ExcelUtils.GetColumnName(column_index++));
            }
            this.MergeHeaderCells(col_names_pivot.ToArray());
        }
        private void MergeHeaderCells(string[] col_names)
        {
            // формируем двумерный массив ячеек для удобства обработки
            ExcelRow[] rows = this._rows.Where(r => r.IsHeadRow).ToArray();
            ExcelCell[,] cells = new ExcelCell[rows.Length, col_names.Length];
            for (int i = 0; i < rows.Length; i++) {
                ExcelRow row = rows[i];
                for (int j = 0; j < col_names.Length; j++) {
                    string col_name = col_names[j];
                    ExcelCell cell = row.Cells.First(c => c.CellInfo.ColumnName == col_name);

                    cells[i, j] = cell;
                }
            }

            int id_new = 1;
            int[,] map = new int[rows.Length, col_names.Length];
            var merges_info = new Dictionary<int, MergeArea>();

            for (int i = 0; i < rows.Length; i++) {
                for (int j = 0; j < col_names.Length; j++) {

                    if (cells[i, j].Text != "") {
                        if (i > 0) {
                            // смотрим сверху
                            if (cells[i, j].Text == cells[i - 1, j].Text) {
                                // ищем правые и левые границы
                                // если они равны - мержим с ячейками верхней строки

                                // правые
                                int b1 = -1;
                                int b2 = -1;
                                for (int x = j; x < col_names.Length; x++) {
                                    if (cells[i - 1, j].Text == cells[i - 1, x].Text) b1 = x;
                                    else break;
                                }
                                for (int x = j; x < col_names.Length; x++) {
                                    if (cells[i, j].Text == cells[i, x].Text) b2 = x;
                                    else break;
                                }

                                // левые
                                int b3 = -1;
                                int b4 = -1;
                                for (int x = j; x >= 0; x--) {
                                    if (cells[i - 1, j].Text == cells[i - 1, x].Text) b3 = x;
                                    else break;
                                }
                                for (int x = j; x >= 0; x--) {
                                    if (cells[i, j].Text == cells[i, x].Text) b4 = x;
                                    else break;
                                }

                                // правые границы совпадают, а левые пересекаются - мержим с верхними
                                if (b1 == b2 && b3 >= b4) {
                                    map[i, j] = map[i - 1, j];
                                    merges_info[map[i, j]].BottomRow = i;
                                    continue;
                                }
                            }
                        }

                        // смотрим слева
                        if (j > 0 && cells[i, j].Text == cells[i, j - 1].Text) {
                            // проверяем что ячейки в строке выше смержены
                            if (i == 0 || map[i - 1, j - 1] == map[i - 1, j]) {
                                // мержим с левыми
                                map[i, j] = map[i, j - 1];
                                merges_info[map[i, j]].RightColumn = j;
                                continue;
                            }
                        }

                        // не нашли ничего, с чем можно смержить
                        map[i, j] = id_new++;
                        merges_info.Add(map[i, j], new MergeArea(i, j));
                    }
                }
            }

            foreach (MergeArea area in merges_info.Values) {
                // одна ячейка в группе - игнорируем
                if (area.TopRow == area.BottomRow && area.RightColumn == area.LeftColumn) continue;

                var cell1 = cells[area.TopRow, area.LeftColumn].CellInfo;
                var cell2 = cells[area.BottomRow, area.RightColumn].CellInfo;
                bool success = this.merges.CreateMergeChecked(cell1, cell2);
                if (!success) {
                    throw new InvalidOperationException(string.Format("Не удалось объединить диапазон {0}:{1}", cell1.CellName, cell2.CellName));
                }
            }
        }
        private class MergeArea
        {
            internal int LeftColumn;
            internal int RightColumn;
            internal int TopRow;
            internal int BottomRow;
            public MergeArea(int row, int column)
            {
                this.RightColumn = column;
                this.LeftColumn = column;
                this.TopRow = row;
                this.BottomRow = row;
            }
        }
    }
}