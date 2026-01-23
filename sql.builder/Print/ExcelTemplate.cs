//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.Spreadsheet;
////using DevExpress.Utils;
////using DevExpress.XtraEditors.Repository;
////using DevExpress.XtraGrid;
////using DevExpress.XtraGrid.Columns;
////using DevExpress.XtraGrid.Views.BandedGrid;
////using DevExpress.XtraGrid.Views.Grid;

//using infoenergo.ui.FormatProviders;
//using sql.builder.Controls.Grids;
//using sql.builder.XmlHelpers;
//using Excel = DevExpress.Spreadsheet;
//using ex = Microsoft.Office.Interop.Excel;
//using sql.builder.Controls;
//using sql.builder.Controls.Grids.ReportViewModes;
//namespace sql.builder
//{
//    internal class ExcelTemplate
//    {
//        private static Cell _format_title;
//        private static Cell _format_bands;
//        private static Cell _format_columns;
//        private static Cell _format_data;
//        private static Cell _format_summary;

//        public static XDocument CreateExcelTemplate(ucTableViewerContainer grid)
//        {
            

//            DevExpress.XtraGrid.Views.Grid.GridView view = null;
//            if (grid.GetGridControl() != null)
//            {
//                view = (grid.GetGridControl() as sql.builder.Controls.Grids.ReportViewModes.ucGridWF).GetMainView();
//            }

//            if (view == null)
//            {
//                //var temp = new GridControl();
//                var temp = new ucGridWF();
//                Parser.LoadGridSettingsFromXml(grid.DataSource.Scheme, temp);
//                view = temp.MainView as GridView;
//            }

//            var existed = grid.DataSource.Tables[0].Columns.Cast<DataColumn>().Select(c => c.ColumnName).ToArray();
//            var columns = view.Columns.Where(c => c.Visible && !existed.Contains(c.FieldName));
//            foreach (var c in columns) c.Visible = false;

//            return CreateExcelTemplate(view, grid.ReportTitle);
//        }

//        public static string CreateXlsxTemplate(GridView view, string table_name, string title = "")
//        {
//            var output_path_xlsx = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xlsx");
          
//            using (var book = new Workbook())
//            {
//                CreateHeader(book.Worksheets[0], view, table_name, title);
//                book.SaveDocument(output_path_xlsx, DocumentFormat.OpenXml);
//            }

//           //  пересохранение в валидном формате xlsx
//            ex.Application exapp = new ex.Application();
//            ex.Workbook wb = exapp.Workbooks.Open(output_path_xlsx);
//            foreach (ex.Worksheet sh in wb.Worksheets)
//            {
//                sh.Rows.AutoFit();
//            }
//            wb.Save();
           
//            exapp.Workbooks.Close();
//            exapp.Quit();
//            return output_path_xlsx;

//        }

//        public static XDocument CreateExcelTemplate(GridView view, string tableName, string caption = null)
//        {
//            return CreateExcelTemplate(new[] { view }, new[] { tableName }, new[] { caption }, new[] { caption });
//        }

//        public static XDocument CreateExcelTemplate(GridView[] views, string[] tableNames, string[] captions, string[] sheetNames)
//        {
//            var output_path_xlsx = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xlsx");
//            var output_path_xml = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xml");

//            using (var book = new Workbook())
//            {
//                for (int i = 0; i < views.Length; i++)
//                {
//                    if(book.Worksheets.Count < i + 1)
//                    {
//                        book.Worksheets.Add();
//                    }

//                    CreateHeader(book.Worksheets[i], views[i], tableNames[i], captions[i]);

//                    if (!string.IsNullOrEmpty(sheetNames[i])) book.Worksheets[i].Name = sheetNames[i].Length > 31 ? sheetNames[i].Substring(0, 31) : sheetNames[i];
//                }
//                book.Worksheets.ActiveWorksheet = book.Worksheets[0];
//                book.SaveDocument(output_path_xlsx, DocumentFormat.OpenXml);
//            }

//            // пересохранение в формате xml
//            ex.Application exapp = new ex.Application();
//            ex.Workbook wb = exapp.Workbooks.Open(output_path_xlsx);
//            wb.SaveAs(output_path_xml, Microsoft.Office.Interop.Excel.XlFileFormat.xlXMLSpreadsheet,
//                        null,
//                        null,
//                        null,
//                        null,
//                        Microsoft.Office.Interop.Excel.XlSaveAsAccessMode.xlNoChange,
//                        Microsoft.Office.Interop.Excel.XlSaveConflictResolution.xlLocalSessionChanges,
//                        null,
//                        null,
//                        null,
//                        false);
//            exapp.Workbooks.Close();
//            exapp.Quit();

//            File.Delete(output_path_xlsx);

//            return XDocument.Load(output_path_xml);
//        }

//        static void CreateHeader(Worksheet sheet, GridView view, string table_name, string title)
//        {           
//            int ncol = 0;
//            int nrow = 0;
//            // view.VisibleColumns не успевает заполниться в режиме nogrid
//            int nwidth = view.Columns.Count(c => c.Visible);

//            LoadFormats();

//            // заголовок
//            var range = sheet.Range.FromLTRB(ncol, nrow, nwidth - 1, nrow);
//            range.CopyFrom(_format_title, PasteSpecial.Formats);
//            range.RowHeight = _format_title.RowHeight;
//            range.Value = title;
//            range.Merge();

//            // бэнды
//            nrow++;
//            var bview = view as BandedGridView;
//            // есть бэнды
//            if (bview != null)
//            {
//                int max_bands_level = GetMaxBandLevel(bview);
//                // 1px = 0.75 points
//                double band_row_height = 4D * bview.BandPanelRowHeight / 3D;

//                // форматирование назначаем до мерджа бэндов, иначе эксэпшн
//                range = sheet.Range.FromLTRB(0, 1, nwidth - 1, nrow + max_bands_level);
//                range.CopyFrom(_format_bands, PasteSpecial.Formats);
//                if (bview.BandPanelRowHeight > 0) range.RowHeight = band_row_height;
//                else range.RowHeight = _format_bands.RowHeight;

//                // битовая карта, т.к. бэнд может занимать более одной строки
//                // нужно хранить информ. об уже занятых ячейках 
//                var matrix = new BandsMatrix(nrow, nwidth, max_bands_level + 1);

//                var bands = bview.Bands.Where(b => b.Visible);
//                for (; nrow <= max_bands_level + 1; nrow++)
//                {
//                    ncol = 0;
//                    // печатаем уровень бэндов
//                    foreach (var b in bands) ncol = PrintBand(sheet, b, ncol, nrow, max_bands_level, matrix);
//                    // получаем бэнды следующего уровня
//                    bands = bands.SelectMany(b => b.Children.Where(b2 => b2.Visible));
//                }
//            }

//            range = sheet.Range.FromLTRB(0, nrow, nwidth - 1, nrow);
//            range.CopyFrom(_format_columns, PasteSpecial.Formats);
//            range.RowHeight = _format_columns.RowHeight;
//            // данные
//            range = sheet.Range.FromLTRB(0, nrow + 1, nwidth - 1, nrow + 1);
//            range.CopyFrom(_format_data, PasteSpecial.Formats);
//            // для варианта с бэндами
//            if (view.ColumnPanelRowHeight > 0) range.RowHeight = 4D*view.ColumnPanelRowHeight/3D;
//            else range.RowHeight = _format_columns.RowHeight;

//            // колонки
//            ncol = 0;
//            foreach (var c in view.Columns.Where(c => c.Visible).OrderBy(c => c.VisibleIndex)) ncol = PrintColumn(sheet, c, ncol, nrow, table_name);
//            //
//            sheet.Cells[nrow + 1, ncol].Value = string.Format("begin:{0} end:{0};", table_name);
            
//            // итог
//            if (view.OptionsView.ShowFooter)
//            {
//                ncol = 0;
//                nrow += 2;

//                range = sheet.Range.FromLTRB(0, nrow, nwidth - 1, nrow);
//                range.CopyFrom(_format_summary, PasteSpecial.Formats);
//                range.RowHeight = _format_summary.RowHeight;

//                foreach (var c in view.Columns.Where(c => c.Visible).OrderBy(c => c.VisibleIndex)) ncol = PrintSummary(sheet, c, ncol, nrow);
//            }
//        }
//        static int GetBandColumnsCount(GridBand root_band)
//        {
//            int cols_count = 0;

//            var stack = new Stack<GridBand>();
//            stack.Push(root_band);

//            while (stack.Count > 0)
//            {
//                var band = stack.Pop();
//                cols_count += band.Columns.Cast<GridColumn>().Count(c => c.Visible);

//                foreach (GridBand b in band.Children) stack.Push(b);
//            }

//            return cols_count;
//        }
//        static int GetMaxBandLevel(BandedGridView view)
//        {
//            int max_band_level = 0;

//            var stack = new Stack<GridBand>();
//            foreach (GridBand b in view.Bands) stack.Push(b);

//            while (stack.Count > 0)
//            {
//                var band = stack.Pop();
//                if (band.BandLevel > max_band_level) max_band_level = band.BandLevel;

//                foreach (GridBand b in band.Children) stack.Push(b);
//            }

//            return max_band_level;
//        }
//        static int PrintBand(Worksheet sheet, GridBand band, int ncol, int nrow, int max_bands_level, BandsMatrix matrix)
//        {
//            var nwidth = GetBandColumnsCount(band);
//            // возможно текущая колонка занята бэндом более высокого уровня
//            // тогда ищем ближайшую свободную ячейку в текущей строке
//            int prev = ncol;
//            ncol = matrix.GetFreeColumnInRow(ncol, nrow);
//            // последний бэнд без колонок - игнорируем
//            if (ncol == -1) return prev;

//            var nheight = 1;
//            // последний бэнд должен растянуться, если у колонки бэндов меньше, чем максимально возможно
//            if (band.Columns.Count > 0)
//            {
//                nheight += max_bands_level - band.BandLevel;
//            }

//            // заполняем информацию о ячейках, которые занял бэнд
//            matrix.SetUsed(ncol, nrow, nwidth, nheight);

//            var range = sheet.Range.FromLTRB(ncol, nrow, ncol + nwidth - 1, nrow + nheight - 1);
//            range.Merge();
//            range.Value = band.Caption;

//            return (ncol + nwidth);
//        }
//        static int PrintColumn(Worksheet sheet, GridColumn column, int ncol, int nrow, string table_name)
//        {
//            var cell_column = sheet.Cells[nrow, ncol];
//            var cell_data = sheet.Cells[nrow + 1, ncol];

//            cell_column.SetValue(column.Caption);

//            // для lookup колонок нужно выводить имена а не коды
//            string fieldname = GetFieldName(column);

//            cell_data.NumberFormat = GetExecelCellFormat(column);
            
//            cell_data.SetValue(String.Format("[:{0}.{1}]", table_name, fieldname));
                       
//            sheet.Columns[ncol].WidthInPixels = column.Width;

//            return (ncol + 1);
//        }
//        static int PrintSummary(Worksheet sheet, GridColumn column, int ncol, int nrow)
//        {
//            var cell_summary = sheet.Cells[nrow, ncol];
//            cell_summary.NumberFormat = GetExecelCellFormat(column);
//            if (column.SummaryItem.SummaryValue != null)
//            {
//                cell_summary.SetValue(column.SummaryItem.SummaryValue);
//            }

//            return (ncol + 1);
//        }
//        static void LoadFormats()
//        {
//            if (_format_title != null) return;

//            var template = new Workbook();
//            template.LoadDocument(Properties.Resources.formats, DocumentFormat.OpenXml);
//            _format_title = template.Worksheets[0].Cells[0, 0];
//            _format_bands = template.Worksheets[0].Cells[1, 0];
//            _format_columns = template.Worksheets[0].Cells[2, 0];
//            _format_data = template.Worksheets[0].Cells[3, 0];
//            _format_summary = template.Worksheets[0].Cells[4, 0];
//        }
//        // преобразование форматов devexpress в excel
//        static string GetExecelCellFormat(GridColumn col)
//        {
//            if (col.DisplayFormat.FormatType != FormatType.Custom)
//            {
//                var num0 = new[] {"{0:n0}", "n0", "n", "{0:N0}", "N0", "N"};
//                var num1 = new[] {"{0:n1}", "n1", "{0:N1}", "N1"};
//                var num2 = new[] {"{0:n2}", "n2", "{0:N2}", "N2"};
//                var num3 = new[] {"{0:n3}", "n3", "{0:N3}", "N3"};
//                var num4 = new[] {"{0:n4}", "n4", "{0:N4}", "N4"};
//                var num5 = new[] {"{0:n5}", "n5", "{0:N5}", "N5"};
//                var num6 = new[] {"{0:n6}", "n6", "{0:N6}", "N6"};
//                var curr = new[] {"{0:c2}", "c2", "c", "{0:C2}", "C2", "C"};
//                var perc = new[] {"{0:p2}", "p2", "p", "{0:P2}", "P2", "P"};

//                var fs = col.DisplayFormat.FormatString;
//                switch (col.ColumnType.Name)
//                {
//                    case "DateTime":
//                        return "DD.MM.YYYY";
//                    case "Decimal":
//                    case "Double":
//                    case "Float":
//                    case "Int32":
//                    case "Int64":
//                        // в Excel запятая будет восприниматься как пробел!
//                        if (num0.Contains(fs)) return "#,##0";
//                        else if (num1.Contains(fs)) return "#,##0.0";
//                        else if (num2.Contains(fs)) return "#,##0.00";
//                        else if (num3.Contains(fs)) return "#,##0.000";
//                        else if (num4.Contains(fs)) return "#,##0.0000";
//                        else if (num5.Contains(fs)) return "#,##0.00000";
//                        else if (num6.Contains(fs)) return "#,##0.000000";
//                        else if (curr.Contains(fs)) return "#,##0.00р.";
//                        else if (perc.Contains(fs)) return "0.00%";
//                        else return "";
//                    default:
//                        return "#"; // текст
//                }
//            }
//            else 
//            {
//                // грид Кресса
//                var col2 = col as infoenergo.ui.win.Grid.Column.GridColumn;
//                if (col2 == null) return "";
//                //var precision = typeof(infoenergo.ui.win.Grid.Column.GridColumn).GetProperty("DecimalFormatPrecision").GetValue(col, null);
//                //if (precision == null) return "";

//                var fp = col.DisplayFormat.Format;
//                if (fp is DateFormatter)
//                {
//                    return "DD.MM.YYYY";
//                }
//                else if (fp is DateTimeFormatter)
//                {
//                    return "DD.MM.YYYY h:mm:ss";
//                }
//                else if (fp is TimeFormatter)
//                {
//                    return "h:mm:ss";
//                }
//                else if (fp is IntFormatter || fp is NumberFormatter || fp is CustomDecimalFormatter || fp is NumberFormatterThousSplit)
//                {
//                    var str = "#,##0";
//                    if (col2.DecimalFormatPrecision > 0)
//                    {
//                        str += "." + new string('0', col2.DecimalFormatPrecision);
//                    }
//                    return str;
//                }
//                else if (fp is CurrencyFormatter)
//                {
//                    return "#,##0.00р.";
//                }
//                else if (fp is YYYYmmFormatter)
//                {
//                    return "#0.00";
//                }
//                else if (fp is mmYYYYFormatter)
//                {
//                    return "#0.0000";
//                }
//                else return "";
//            }
//        }

//        public static string GetFieldName(GridColumn column)
//        {
//            // для колонки с lookup добавляем __DisplayMember
//            var lookup = column.ColumnEdit as RepositoryItemLookUpEditBase;
//            return (lookup != null) ? string.Format("{0}__{1}", column.FieldName, lookup.DisplayMember) : column.FieldName;
//        }
//        public static DataTable PrepareData(GridView view, PrintOptions options)
//        {
//            // добавляем данные из lookup-ов в отдельную колонку и ссылаемся на нее

//            //data = data.AsEnumerable().CopyToDataTable();
//            //var ds = new DataSet();
//            // чтобы учесть сортировку и фильтрацию
//            // данные получаем через view. если данных нет CopyToDataTable даст исключение, поэтому изощряемся
//            var data = (view.RowCount == 0 && (view.GridControl.DataSource as DataTable) != null) ? Cmn.ToVDataTable(view.GridControl.DataSource as DataTable)
//                     : (options.PrintRowsMode == PrintRowsMode.AllRows) ? Enumerable.Range(0, view.DataRowCount).Select(view.GetDataRow).CopyToDataTable() 
//                     : (options.PrintRowsMode == PrintRowsMode.SelectedRows) ? view.GetSelectedRows().Select(view.GetDataRow).CopyToDataTable() 
//                     : null; 

//            data.TableName = "a";
//            //ds.Tables.Add(data);

//            data.BeginLoadData();
//            var info = new List<Tuple<string, string, Dictionary<object, object>>>();
//            foreach (var column in view.Columns.Where(c => c.Visible))
//            {
//                var lookup = column.ColumnEdit as RepositoryItemLookUpEditBase;
//                if (lookup == null) continue;

//                string name = GetFieldName(column);

//                DataTable dt = lookup.DataSource as DataTable;
//                if(dt != null)
//                {
//                    Type type = dt.Columns[lookup.DisplayMember].DataType;
//                    data.Columns.Add(name, type);

//                    // для скорости обработки перекладываем данные из DataTable в Dictionary (чтобы использовать поиск по хэшу)
//                    var dict = dt.AsEnumerable().ToDictionary(r => r[lookup.ValueMember], r => r[lookup.DisplayMember]);
//                    info.Add(new Tuple<string, string, Dictionary<object, object>>(column.FieldName, name, dict));
//                    continue;
//                }

//                // добавил для конкретного случая Риты (37740)
//                // возможно нужно переделать в общем виде
//                BindingSource bs = lookup.DataSource as BindingSource;
//                if (bs != null)
//                {
//                    Dictionary<decimal, string> dict1 = bs.DataSource as Dictionary<decimal, string>;
//                    if (dict1 != null)
//                    {
//                        data.Columns.Add(name, typeof(string));

//                        // для скорости обработки перекладываем данные из DataTable в Dictionary (чтобы использовать поиск по хэшу)
//                        var dict = dict1.AsEnumerable().ToDictionary(r => (object)r.Key, r => (object)r.Value);
//                        info.Add(new Tuple<string, string, Dictionary<object, object>>(column.FieldName, name, dict));
//                        continue;   
//                    }
//                }
//            }

//            if (!info.Any())
//            {
//                data.EndLoadData();
//                return data;
//            }

//            // item1 - fieldname
//            // item2 - fieldname__displaymember
//            // item3 - data
//            foreach (var row in data.AsEnumerable())
//            {
//                foreach (var lookup_info in info)
//                {
//                    object value = null;
//                    lookup_info.Item3.TryGetValue(row[lookup_info.Item1], out value);
//                    row[lookup_info.Item2] = value;
//                }
//            }

//            data.EndLoadData();
//            return data;
//        }

//        class BandsMatrix
//        {
//            private int _row_delta;
//            private int _nwidth;
//            private int _nheight;


//            private bool[,] _matrix;

//            public BandsMatrix(int row_delta, int nwidth, int nheight)
//            {
//                _row_delta = row_delta;
//                _nheight = nheight;
//                _nwidth = nwidth;

//                _matrix = new bool[nwidth, nheight];
//            }

//            public void SetUsed(int ncol, int nrow, int nwidth, int nheight)
//            {
//                for (int i = ncol; i < ncol + nwidth; i++)
//                {
//                    for (int j = nrow - _row_delta; j < nrow - _row_delta + nheight; j++)
//                    {
//                        _matrix[i, j] = true;
//                    }
//                }
//            }
//            public int GetFreeColumnInRow(int ncol, int nrow)
//            {
//                for (int i = ncol; i < _nwidth; i++)
//                {
//                    if (!_matrix[i, nrow - _row_delta]) return i;
//                }

//                return -1;
//            }
//        }
//    }
//}
