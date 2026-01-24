using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using DataTable = System.Data.DataTable;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;
//using DWorkbook = DevExpress.Spreadsheet.Workbook;

using sql.builder.ExcelApi;
using sql.builder.DataApi;
//using sql.builder.TFS;
using sql.builder.XmlHelpers;
using ExcelPrintEnv = sql.builder.Print.Xlsx.ExcelPrintEnv;
using ExcelPrintOptions = sql.builder.Print.Xlsx.ExcelPrintOptions;
using ExcelUtils = sql.builder.Print.Xlsx.ExcelUtils;

namespace sql.builder
{
    /// <summary>
    /// Печать sql.builder
    /// </summary>
    public class ExcelPrintDocument : sql.builder.Print.XML.ExcelPrintDocument
    {
        /// <summary>
        /// Результаты печати файла, информирующие о наличии или отсутствии данных
        /// </summary>
        public enum ExcelPrintErrors
        {
            /// <summary>
            /// Напечатан хотя бы один лист
            /// </summary>
            None,
            /// <summary>
            /// Данные отсутствуют
            /// </summary>
            NoData
        };
        #region static
        private static string last_printed_file_path;
        // лучше не придумал, чтобы не сломать существующее
        /// <summary>
        /// Возвращает или задает путь последней печати файла
        /// </summary>
        internal static string LastPrintedFilePath {
            get {
                return last_printed_file_path;
            }
            set {
                last_printed_file_path = value;
            }
        }
        private static long prev_notify_time;
        //#if DEBUG
        //private static int prev_printed_rows;
        //#endif
        internal static void OnPrintingHandler(object document, object sheet, int printed_sheets, int printed_rows)
        {
            long now = Environment.TickCount;
            ulong elapsed;
            unchecked {
                elapsed = (ulong)(now - prev_notify_time);
            }
            if (elapsed >= 1000UL) {
                string message = "Формирование листа " + (printed_sheets + 1).ToString() + ", строка " + printed_rows.ToString();
                WaitUIHelper.LastUsedUIHelper.SetDescription(message);
                prev_notify_time = now;
            }
        }
        /// <summary>
        /// Печать DataSet в файл
        /// </summary>
        /// <param name="template_path">Путь к шаблону</param>
        /// <param name="output_path">Путь для сохранения итогового файла</param>
        /// <param name="data">DataSet для печати</param>
        /// <returns>Значение из перечисления: None - данные напечатаны, NoData - нет данных для печати</returns>
        public static ExcelPrintErrors PrintNew(string template_path, string output_path, DataSet data)
        {
            return PrintNew(template_path, output_path, data, ExcelPrintOptions.Default);
        }
        /// <summary>
        /// Печать DataSet в файл
        /// </summary>
        /// <param name="template_path">Путь к шаблону</param>
        /// <param name="output_path">Путь для сохранения итогового файла</param>
        /// <param name="data">DataSet для печати</param>
        /// <param name="options">Настройки печати</param>
        /// <returns>Значение из перечисления: None - данные напечатаны, NoData - нет данных для печати</returns>
        public static ExcelPrintErrors PrintNew(string template_path, string output_path, DataSet data, ExcelPrintOptions options)
        // используется в:
        //    \root\main\all\docs.approval\Helper\ExportDataExcel.cs, метод GetExcelFile1()
        //    \root\main\all\orgs.contracts.tep.sales.print\AgreementController.cs, метод CreateExcelDocSqlBuilder() - здесь можно использовать вариант без последнего параметра
        //    \root\main\all\orgs.contracts.tep.sales.print\PrintClass.cs, метод PrintAgreement() и PrintSingleContract() - здесь можно использовать вариант без последнего параметра
        {

            LastPrintedFilePath = null;
            #if DEBUG
            // в режиме отладки добавляем шаблон в проект sql.builder.templates и ТФС
            //if (XmlReports.IsDeveloperMode()) {
            //    ReloadExcelTemplate(template_path, "excel");
            //}
            #endif
            // попутно преобразуем в формат xlsx
            if (options.NeedConvert) {
                WaitUIHelper.LastUsedUIHelper.SetDescription("Предобработка файла Excel...");
                template_path = PreProcess(template_path, data);
            }
            ExcelPrintErrors result = ExcelPrintErrors.None;
            WaitUIHelper.LastUsedUIHelper.SetDescription("Формирование файла Excel...");
            try {
                if (options.OnlyColumns) {
                    options.DeleteUnusedColumns = false;
                }
                using (var env = new ExcelPrintEnv(template_path, options, data)) {
                    if (options.OnlyColumns) {
                        env.SaveTemplate(output_path);
                    } else {
                        var doc = new sql.builder.Print.Xlsx.ExcelPrintDocument(env);
                        doc.Printing += OnPrintingHandler;
                        result = doc.Print(data, options.UseDataReader);
                        doc.Printing -= OnPrintingHandler;
                        if (result != ExcelPrintErrors.NoData) {
                            doc.Save(output_path);
                        }
                    }
                }
            } finally {
                if (options.NeedConvert) {
                    File.Delete(template_path);
                }
            }
            if (result != ExcelPrintErrors.NoData) {
                if (options.NeedPostProcess) {
                    WaitUIHelper.LastUsedUIHelper.SetDescription("Постобработка файла Excel...");
                    string ext = options.OutputFormat.ToString().ToLower();
                    if (options.UseFlexCel) {
                        output_path = PostProcessFlexCel(output_path, ext);
                    } else {
                        output_path = PostProcess(output_path, ext, null, options.FormatSource);
                    }
                } else if (options.OutputFormat != ExcelPrintOptions.FileFormat.Xlsx) {
                    output_path = ChangeExcelFileFormat(output_path, options.OutputFormat);
                }
            }
            // пришлось сделать так, чтобы не поломать существующий код
            // правильно было бы сделать output_path out параметром, т.к. он может измениться внутри ф-и
            // при изменении формата на xlsb например
            LastPrintedFilePath = output_path;
            WaitUIHelper.LastUsedUIHelper.SetDescription("Формирование файла Excel завершено");
            return result;
        }

        private static string PreProcess(string template_path, DataSet data)
        {
            throw new NotImplementedException();
            //var exApp = new Microsoft.Office.Interop.Excel.Application();
            //try {
            //    exApp.DisplayAlerts = false;
            //    Workbook wb = exApp.Workbooks.Open(template_path);

            //    // старый вариант - сделал удаление на уровне xml
            //    //if (options.DeleteUnusedColumns) RemoveUnusedColumns(wb, data);

            //    string template_path_new = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xlsx");
            //    wb.SaveAs(template_path_new,
            //            XlFileFormat.xlOpenXMLWorkbook,
            //            ReadOnlyRecommended: false,
            //            AccessMode: XlSaveAsAccessMode.xlNoChange,
            //            ConflictResolution: XlSaveConflictResolution.xlLocalSessionChanges);
            //    return template_path_new;
            //} finally {
            //    // вызов quit не убивает процесс excel.exe
            //    Cmn.CloseExcel(ref exApp);
            //}
        }
        
        #endregion
        #region Конструкторы
        /// <summary>
        /// Конструктор класса печати sql.builder
        /// </summary>
        /// <param name="template">Шаблон</param>
        public ExcelPrintDocument(XDocument template) : base(template) { }
        /// <summary>
        /// Конструктор класса печати sql.builder
        /// </summary>
        /// <param name="template_file_name">Имя файла шаблона Excel в формате *.xml</param>
        public ExcelPrintDocument(string template_file_name) : base(template_file_name) {}
        #endregion
        /// <summary>
        /// Выполняет постобработку файла (формат файла, метки, форматирование)
        /// </summary>
        /// <param name="file_name">Имя файла</param>
        /// <returns>Полное имя обработанного файла</returns>
        public static string PostProcess(string file_name)
        {
            return PostProcess(file_name, "xlsx", null, null);
        }
        /// <summary>
        /// Выполняет постобработку файла (формат файла, метки, форматирование)
        /// </summary>
        /// <param name="file_name">Имя файла</param>
        /// <param name="output_format">Требуемый формат файла после пересохранения: "xlsx", "xlsb" или "pdf"</param>
        /// <returns>Полное имя обработанного файла</returns>
        public static string PostProcess(string file_name, string output_format)
        {
            return PostProcess(file_name, output_format, null, null);
        }
        /// <summary>
        /// Выполняет постобработку файла (формат файла, метки, форматирование)
        /// </summary>
        /// <param name="file_name">Имя файла</param>
        /// <param name="output_format">Требуемый формат файла после пересохранения: "xlsx", "xlsb" или "pdf"</param>
        /// <param name="file_name_new">Новое имя файла</param>
        /// <param name="format_source">см. <see cref="ExcelPrintOptions.FormatSource"/></param>
        /// <returns>Полное имя обработанного файла</returns>
        internal static string PostProcess(string file_name, string output_format, string file_name_new, string format_source)
        {
            if (Logger.IsAcive) Logger.Log("Постобработка файла...");
            //var exApp = new Application();
            try {
                //exApp.DisplayAlerts = false;
                if (string.IsNullOrEmpty(file_name_new)) {
                    file_name_new = Path.ChangeExtension(file_name, output_format);
                }
                if (file_name != file_name_new)
                {
                    File.Copy(file_name, file_name_new);
                }
                
                //Workbook wb = exApp.Workbooks.Open(file_name);
                //Workbook wbFormat = null;
                //if (!string.IsNullOrEmpty(format_source)) {
                //    wbFormat = exApp.Workbooks.Open(format_source);
                //}
                //// чтобы пересчитались значения всех формул
                //exApp.CalculateFull();
                //// в идеале надо проверять по атрибутам в xml отчета
                //bool merge_down_exists = false;
                //bool merge_right_exists = false;
                //bool headmarker_exists = false;
                //bool delete_exists = false;
                ////bool rowheight_exists = false;
                //bool rowid_exists = false;
                //bool protectsheet_exists = false;
                //bool autorowheight_exist = false;
                //bool no_autorowheight_exist = false;
                //bool autocolwidth_exist = false;
                //bool pagebreak_exists = false;
                //bool blocks_exists = false;
                //bool printtitlerows_exists = false;
                //foreach (Worksheet sh in wb.Sheets) {
                //    if (!headmarker_exists && sh.UsedRange.Find(TextConst.ExcelMarks.HeadMarker) != null) headmarker_exists = true;
                //    if (!merge_down_exists && sh.UsedRange.Find(TextConst.ExcelMarks.MergeDown) != null) merge_down_exists = true;
                //    if (!merge_right_exists && sh.UsedRange.Find(TextConst.ExcelMarks.MergeRight) != null) merge_right_exists = true;
                //    if (!delete_exists && sh.UsedRange.Find(TextConst.ExcelMarks.DeleteRanges) != null) delete_exists = true;
                //    if (!rowid_exists && sh.UsedRange.Find(TextConst.ExcelMarks.RowID) != null) rowid_exists = true;
                //    if (!protectsheet_exists && sh.UsedRange.Find(TextConst.ExcelMarks.ProtectSheet) != null) protectsheet_exists = true;
                //    if (!pagebreak_exists && sh.UsedRange.Find(TextConst.ExcelMarks.PageBreak) != null) pagebreak_exists = true;
                //    if (!printtitlerows_exists && sh.UsedRange.Find(TextConst.ExcelMarks.PrintTitleRows) != null) printtitlerows_exists = true;
                //    if (!blocks_exists && sh.UsedRange.Find("[block") != null) blocks_exists = true;
                //    Range[] headmarkers = new Range[0];
                //    if (headmarker_exists) {
                //        headmarkers = Printing.SearchAllText(sh.UsedRange, TextConst.ExcelMarks.HeadMarker);
                //    }
                //    if (merge_down_exists) {
                //        //если есть маркер headmarker то автоподбор ширины делаем после него, иначе по всему листу
                //        if (headmarkers.Length > 0) {
                //            var rh_row = headmarkers.Max(r => r.Row);
                //            Printing.MergeDownWorksheet(sh, rh_row + 1);
                //        } else {
                //            Printing.MergeDownWorksheet(sh);
                //        }
                //    }
                //    Printing.DeleteRows(sh);
                //    autorowheight_exist = (sh.Rows.Find(TextConst.ExcelMarks.AutoRowHeight) != null);
                //    no_autorowheight_exist = (sh.Rows.Find(TextConst.ExcelMarks.NoAutoRowHeight) != null);
                //    if (autorowheight_exist) {
                //        Printing.SetAutoWidth(sh);
                //    } else if (no_autorowheight_exist) {
                //        RemoveMark(sh, TextConst.ExcelMarks.NoAutoRowHeight); // Удаление пометок !noautorowheight
                //    } else {
                //        sh.Rows.AutoFit();
                //    }
                //    autocolwidth_exist = (sh.Rows.Find(TextConst.ExcelMarks.AllColsAutoFit) != null);
                //    if (autocolwidth_exist) {
                //        RemoveMark(sh, TextConst.ExcelMarks.AllColsAutoFit); // Удаление пометок !allcolumsautofit
                //        sh.Columns.AutoFit();
                //    }
                //    // высота строк обрабатывается интеропом для отчета по исполнителю 
                //    // devexpress не справляется с большими файлами
                //    Printing.SetRowsHeight(sh);
                //    Printing.SetColumnsWidth(sh);
                //    //pagebreak_exists = (sh.Rows.Find(TextConst.ExcelMarks.PageBreak) != null);
                //    //if (pagebreak_exists) Printing.SetPageBreaks(sh);
                //    if (headmarker_exists) {
                //        // не понимаю зачем удаляется колонка, но так было в старом варианте
                //        if (headmarkers.Length > 0) {
                //            var rh_col = headmarkers.Max(r => r.Column);
                //            //удаляем колонку с headmarker 
                //            sh.Columns[rh_col].Delete();
                //        }
                //    }
                //    // 29.01.18 пришлось переписать на Devexpress, тк ножно выполнять после DeleteRanges 
                //    //if (sh.UsedRange.Find("[block") != null)
                //    //{
                //    //    Printing.ProcessBlocks(sh);
                //    //}
                //    if (wbFormat != null) {
                //        foreach (Worksheet sh1 in wbFormat.Sheets) {
                //            if (sh1.Name == sh.Name) {
                //                sh1.Cells.Copy();
                //                Range r = sh.Cells[1, 1];
                //                r.PasteSpecial(Paste: XlPasteType.xlPasteFormats);
                //                r.Copy();
                //                r.PasteSpecial(Paste: XlPasteType.xlPasteFormats);
                //                break;
                //            }
                //        }
                //    }
                //}
                //if (wbFormat != null) {
                //    ((_Worksheet)wb.Sheets[1]).Activate();
                //    wbFormat.Close();
                //}
                //// по мере необходимости переписать на interop
                //if (merge_right_exists || delete_exists || rowid_exists || protectsheet_exists || pagebreak_exists || blocks_exists || printtitlerows_exists) {
                //    // пересохраняем интеропом в формате xlsx, тк девэкспресс не умеет загружать xml
                //    var temp_output_path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xlsx");
                //    wb.SaveAs(temp_output_path,
                //        XlFileFormat.xlOpenXMLWorkbook,
                //        ReadOnlyRecommended: false,
                //        AccessMode: XlSaveAsAccessMode.xlNoChange,
                //        ConflictResolution: XlSaveConflictResolution.xlLocalSessionChanges);
                //    wb.Close();
                //    using (var dwb = new DWorkbook()) {
                //        dwb.LoadDocument(temp_output_path, DocumentFormat.OpenXml);
                //        // чтобы SetRowsHeight правильноустановил размеры
                //        dwb.Unit = DocumentUnit.Point;
                //        //File.Delete(temp_output_path);
                //        foreach (var dsh in dwb.Worksheets) {
                //            if (merge_right_exists) {
                //                Printing.MergeRightWorksheet(dsh);
                //            }
                //            if (delete_exists) {
                //                Printing.DeleteRanges(dsh);
                //            }
                //            //if (rowid_exists) {
                //            //    Printing.ProcessRowIdColumn(dsh);
                //            //}
                //            if (protectsheet_exists) {
                //                Printing.ProtectSheet(dsh);
                //            }
                //            if (pagebreak_exists) {
                //                Printing.SetPageBreaks(dsh);
                //            }
                //            if (printtitlerows_exists) {
                //                Printing.SetPrintTitleRows(dsh);
                //            }
                //            if (blocks_exists) {
                //                Printing.ProcessBlocks(dsh);
                //            }
                //        }
                //        if (output_format == "pdf") {
                //            dwb.ExportToPdf(file_name_new);
                //        } else if (output_format == "xlsb") {
                //            throw new ArgumentException("xlsb не поддерживается");
                //        } else {
                //            dwb.SaveDocument(file_name_new, DocumentFormat.OpenXml);
                //        }
                //    }
                //} else {
                //    if (output_format == "pdf") {
                //        file_name_new = Printing.GetFreeNameChangeExt(file_name_new, "pdf");
                //        wb.ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, file_name_new);
                //        wb.Save();
                //    } else if (output_format == "xlsb") {
                //        file_name_new = Printing.GetFreeNameChangeExt(file_name_new, "xlsb");
                //        wb.SaveAs(file_name_new, XlFileFormat.xlExcel12);
                //    } else {
                //        wb.SaveAs(file_name_new, XlFileFormat.xlOpenXMLWorkbook);
                //    }
                //    wb.Close();
                //}
                ////Custom'ный формат не поддерживается при сохранении девэкспрессом. Устанавливаем фоормат, сохраняем интеропом
                ////Для отчетов 45676 "Информация об исполнении договоров на ТП Перечня Минэнерго РФ"
                //if (!string.IsNullOrEmpty(format_source)) {
                //    //string[] fn = format_source.Split('\\');
                //    //if (fn.LastOrDefault().IndexOf("45676") != -1) {
                //    int n_pos = format_source.IndexOf('\\');
                //    string f;
                //    if (n_pos < 0) {
                //        f = format_source;
                //    } else {
                //        f = format_source.Substring(n_pos + 1);
                //    }
                //    if (f.IndexOf("45676") != -1) {
                //        wb = exApp.Workbooks.Open(file_name);
                //        Worksheet wb_sh = wb.Worksheets["Форма ТП 2"];
                //        wb_sh.Range["O45", "O54"].NumberFormat = "#,##0;(#,##0)";
                //        wb_sh.Range["R45", "R54"].NumberFormat = "#,##0;(#,##0)";
                //        wb_sh.Range["O62", "O71"].NumberFormat = "#,##0;(#,##0)";
                //        wb_sh.Range["R62", "R71"].NumberFormat = "#,##0;(#,##0)";
                //        wb_sh.Range["O79", "O88"].NumberFormat = "#,##0;(#,##0)";
                //        wb_sh.Range["R79", "R88"].NumberFormat = "#,##0;(#,##0)";
                //        ((_Worksheet)wb.Sheets[1]).Activate();
                //        wb.Save();
                //        wb.Close();
                //    }
                //}
                //if (file_name != file_name_new) File.Delete(file_name);
                //if (Logger.IsAcive) Logger.Log("Постобработка завершена");
                return file_name_new;
            } finally {
                //// вызов quit не убивает процесс excel.exe
                //try {
                //    Cmn.CloseExcel(ref exApp);
                //} catch { }
            }
        }
        //private static void RemoveMark(Worksheet sh, string mark)
        //{
        //    while (true)
        //    {
        //        Range r = sh.UsedRange.Find(What: mark,
        //                                    LookIn: XlFindLookIn.xlValues,
        //                                    LookAt: XlLookAt.xlPart,
        //                                    SearchOrder: XlSearchOrder.xlByRows,
        //                                    SearchDirection: XlSearchDirection.xlNext,
        //                                    MatchCase: false);
        //        if (r == null)
        //        {
        //            break;
        //        }
        //        r.Clear();
        //    }
        //}
        /// <summary>
        ///  На основе FlexCel
        /// </summary>
        internal static string PostProcessFlexCel(string file_name, string output_format = "xlsx", string file_name_new = null)
        {
            throw new NotImplementedException();
            //if (Logger.IsAcive) Logger.Log("Постобработка файла...");

            //// для поддержки остальных форматов
            //if (Path.GetExtension(file_name) != ".xlsx")
            //{
            //    file_name = ChangeExcelFileFormat(file_name, ExcelPrintOptions.FileFormat.Xlsx);
            //}

            //var excel = new XlsFile(file_name);

            //// чтобы пересчитались значения всех формул
            //excel.Recalc();

            //for (int i = 1; i <= excel.SheetCount; i++)
            //{
            //    excel.ActiveSheet = i;

            //    // autorowheight
            //    var autorowheight = excel.FindAllCellsWithText(TextConst.ExcelMarks.AutoRowHeight);
            //    if (autorowheight.Length != 0) {
            //        foreach (var cell in autorowheight)
            //        {
            //            excel.AutoFitRow(cell.Row);
            //            excel.SetCellValue(cell.Row, cell.Col, null);
            //        }
            //    }
            //    else
            //    {
            //        excel.AutoFitAllRows();
            //    }

            //    // rowheight
            //    var rowheight = excel.FindAllCellsWithText(TextConst.ExcelMarks.RowHeight);
            //    if (rowheight.Length != 0) {
            //        foreach (var cell in rowheight)
            //        {
            //            int height = (int)((decimal)(Cmn.ToDecimal(excel.GetCellValue(cell.Row, cell.Col).ToString().Split(':')[1])) * 20M);
            //            excel.SetRowHeight(cell.Row, height);
            //            excel.SetCellValue(cell.Row, cell.Col, null);
            //        }
            //    }

            //    // autocolwidth
            //    var autocolwidth = excel.FindAllCellsWithText(TextConst.ExcelMarks.AllColsAutoFit);
            //    if (autocolwidth.Length != 0) {
            //        excel.AutoFitAllColumns();
            //        foreach (var cell in autocolwidth)
            //        {
            //            excel.SetCellValue(cell.Row, cell.Col, null);
            //        }
            //    }

            //    // deleteranges
            //    var deleteranges = excel.FindAllCellsWithText(TextConst.ExcelMarks.DeleteRanges);
            //    if (deleteranges.Length != 0) {
            //        foreach (var cell in deleteranges.Reverse())
            //        {
            //            var cords_str = excel.GetCellValue(cell.Row, cell.Col).ToString().Split(':')[1];
            //            int[] cords = cords_str.Split(',').Select(int.Parse);
            //            var range = new TXlsCellRange(cords[0], cords[1], cords[2], cords[3]);
            //            excel.DeleteRange(range, TFlxInsertMode.ShiftRangeRight);
            //            excel.SetCellValue(cell.Row, cell.Col, null);
            //        }
            //    }

            //    // pagebreaks
            //    var pagebreaks = excel.FindAllCellsWithText(TextConst.ExcelMarks.PageBreak);
            //    if (pagebreaks.Length != 0) {
            //        foreach (var cell in pagebreaks)
            //        {
            //            excel.InsertHPageBreak(cell.Row);
            //            excel.SetCellValue(cell.Row, cell.Col, null);
            //        }
            //    }


            //    // headmarker
            //    var headmarker = excel.FindAllCellsWithText(TextConst.ExcelMarks.HeadMarker);
            //    if (headmarker.Length != 0) {
            //        // не понимаю зачем удаляется колонка, но так было в старом варианте
            //        //удаляем колонку с headmarker 
            //        excel.DeleteColumn(headmarker.Max(r => r.Col));
            //    }

            //    // mergedown
            //    // обрабатывается в новой версии на этапе печати!

            //    // mergeright
            //    var mergeright = excel.FindAllCellsWithText(TextConst.ExcelMarks.MergeRight);
            //    if (mergeright.Length != 0) {
            //        Printing.MergeRightWorksheet(excel, mergeright);
            //    }

            //    // protectsheet
            //    var protectsheet = excel.FindAllCellsWithText(TextConst.ExcelMarks.ProtectSheet);
            //    if (protectsheet.Length != 0) {
            //        var options = new TSheetProtectionOptions(false)
            //        {
            //            Contents = true,
            //            Objects = true,
            //            Scenarios = true,
            //            SelectLockedCells = true,
            //            SortCellRange = true,
            //            EditAutoFilters = true,
            //            SelectUnlockedCells = true
            //        };
            //        excel.Protection.SetSheetProtection("qqq", options);
            //        foreach (var cell in protectsheet)
            //        {
            //            excel.SetCellValue(cell.Row, cell.Col, null);
            //        }
            //    }
            //}

            //excel.AllowOverwritingFiles = true;

            //file_name_new = file_name_new ?? Path.ChangeExtension(file_name, output_format);
            //switch (Path.GetExtension(file_name_new))
            //{
            //    case ".pdf":
            //        using (var pdf = new FlexCelPdfExport(excel, true))
            //        {
            //            pdf.Export(file_name_new);
            //        }
            //        File.Delete(file_name);
            //        break;

            //    case ".xlsb":
            //        excel.Save(file_name);
            //        file_name_new = ChangeExcelFileFormat(file_name, ExcelPrintOptions.FileFormat.Xlsb);
            //        break;

            //    case ".xlsx":
            //        excel.Save(file_name_new);
            //        break;

            //    // хз что за формат - сохраняем в xlsx
            //    default:
            //        file_name_new = file_name;
            //        excel.Save(file_name_new);
            //        break;
            //}

            //if (Logger.IsAcive) Logger.Log("Постобработка завершена");

            //return file_name_new;
        }
        internal static string PostProcessBigData(string file_name, string template_path, bool xlsb)
        {

            throw new NotImplementedException();
            //string file_name_new = null;

            //var exApp = new Application { DisplayAlerts = false };
            //try
            //{
            //    exApp.Workbooks.Open(file_name);
            //    Printing.FormattingFile(template_path, exApp.Workbooks[1]);

            //    if (xlsb)
            //    {
            //        file_name_new = Printing.GetFreeName(Path.GetDirectoryName(file_name), Path.GetFileNameWithoutExtension(file_name), "xlsb");
            //        exApp.Workbooks[1].SaveAs(file_name_new, XlFileFormat.xlExcel12);
            //    }
            //    else
            //    {
            //        file_name_new = file_name;
            //        exApp.Workbooks[1].Save();
            //    }
            //}
            //finally
            //{
            //    // вызов quit не убивает процесс excel.exe
            //    Cmn.CloseExcel(ref exApp);
            //}

            //if (xlsb)
            //{
            //    try
            //    {
            //        File.Delete(file_name);
            //    }
            //    catch (IOException)
            //    {
            //        // не удалось удалить xlsx
            //    }
            //}

            //return file_name_new;
        }
        internal static string ChangeExcelFileFormat(string filePath, ExcelPrintOptions.FileFormat formatNew)
        {
            throw new NotImplementedException();
            //if (Logger.IsAcive) Logger.Log(string.Format("Пересохранение файла в формат {0}...", formatNew.ToString().ToLower()));

            //string filePathNew = null;

            //var exApp = new Application { DisplayAlerts = false };
            //try
            //{
            //    exApp.Workbooks.Open(filePath);

            //    bool changed = false;

            //    switch (formatNew)
            //    {
            //        case ExcelPrintOptions.FileFormat.Xlsx:
            //            filePathNew = Printing.GetFreeNameChangeExt(filePath, "xlsx");//Path.ChangeExtension(filePath, ".xlsx");
            //            exApp.Workbooks[1].SaveAs(filePathNew, XlFileFormat.xlOpenXMLWorkbook);
            //            changed = true;
            //            break;

            //        case ExcelPrintOptions.FileFormat.Xlsb:
            //            filePathNew = Printing.GetFreeNameChangeExt(filePath, "xlsb");//Path.ChangeExtension(filePath, ".xlsb");
            //            exApp.Workbooks[1].SaveAs(filePathNew, XlFileFormat.xlExcel12);
            //            changed = true;
            //            break;

            //        case ExcelPrintOptions.FileFormat.Pdf:
            //            filePathNew = Printing.GetFreeNameChangeExt(filePath, "pdf");//Path.ChangeExtension(filePath, ".pdf");
            //            exApp.Workbooks[1].ExportAsFixedFormat(XlFixedFormatType.xlTypePDF, filePathNew);
            //            changed = true;
            //            break;
            //    }

            //    if (changed) File.Delete(filePath);
            //}
            //finally
            //{
            //    // вызов quit не убивает процесс excel.exe
            //    Cmn.CloseExcel(ref exApp);
            //}

            //if (Logger.IsAcive) Logger.Log("Пересохранение завершено");

            //return filePathNew;
        }
        internal static string GetProjectExcelTemplates(string templateType)
        {
            return Path.Combine(XmlReports.GetCurrentContentFolder(), @"printTemplate\" + templateType);
        }
        #if DEBUG
        //internal static void ReloadExcelTemplate(string templatePath, string templateType)
        //{
        //    // если шаблон пришел извне, его никуда добавлять не нужно
        //    if (!templatePath.StartsWith(GetProjectExcelTemplates(templateType))) {
        //        return;
        //    }
        //    VSProjectHelper.RestoreContent(templatePath);
        //    using (var tfs = new TFSServer()) {
        //        tfs.AddFile(templatePath);
        //    }
        //}
        #endif
    }
}
