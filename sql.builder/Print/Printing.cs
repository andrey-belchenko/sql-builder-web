using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
using System.Diagnostics;
using Contract = System.Diagnostics.Contracts.Contract;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Views.Grid;
//using infoenergo.ui.win.Grid;
//using Microsoft.Office.Interop.Excel;
//using Microsoft.Vbe.Interop;
using sql.builder.DataApi;
using sql.builder.ExcelApi;  
//using DevExpress.Spreadsheet;
//using FlexCel.Core;
//using FlexCel.XlsAdapter;
//
//using reports.word.XmlPrint;
using sql.builder.Print.Xlsx;
using sql.builder.Test;
using sql.builder.WinForms;
using sql.builder.XmlHelpers;
using DataTable = System.Data.DataTable;
//using Application = Microsoft.Office.Interop.Excel.Application;
//using Workbook = Microsoft.Office.Interop.Excel.Workbook;
//using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;
//using Range = Microsoft.Office.Interop.Excel.Range;
//using DWorkbook = DevExpress.Spreadsheet.Workbook;
//using DWorksheet = DevExpress.Spreadsheet.Worksheet;
//using DRange = DevExpress.Spreadsheet.Range;
//using FormatCondition = Microsoft.Office.Interop.Excel.FormatCondition;

namespace sql.builder
{
    public static partial class Printing
    {
        public static string templatesFolder;
        private static string _outputFolder;

        public static string outputFolder {
            get {
                if (string.IsNullOrEmpty(_outputFolder)) {
                    return Path.GetTempPath();
                } else {
                    return _outputFolder;
                }
            }
            set {
                _outputFolder = value;
            }
        }
        public static bool CreateRefs = false;

        //public static string Print(GridControl grid, string output_path = null, bool xlsx = true, PrintOptions options = null)
        //{
        //    // достаём view
        //    var baseGridControlInfo = typeof(GridControl).GetField("baseGridControl", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
        //    var gridViewInfo = baseGridControlInfo.FieldType.GetField("View", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
        //    var baseGridControl = baseGridControlInfo.GetValue(grid);
        //    var view = (GridView)gridViewInfo.GetValue(baseGridControl);

        //    return Print(view, grid.Caption, output_path, xlsx, options);
        //}
        //public static string Print(GridView view, string caption, string output_path = null, bool xlsx = true, PrintOptions options = null, bool prepare = true, ExcelPrintOptions excelPrintOptions = null)
        //{
        //    options = options ?? PrintOptions.Default;

        //    // копировать все данные не очень хорошая идея
        //    // но придется добавлять в DataTable свои колонки из-за lookup-ов
        //    // и делать это на изначальном DataTable идея еще хуже  
        //    DataTable data = null;
        //    if (prepare)
        //    {
        //        data = ExcelTemplate.PrepareData(view, options);
        //    }
        //    else
        //    {
        //        data = (view.GridControl.DataSource as DataTable);
        //    }

        //    if (data.DataSet == null)
        //    {
        //        var ds = new DataSet();
        //        ds.Tables.Add(data);
        //    }
        //    // шаблон


        //    output_path = output_path ?? Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ((xlsx) ? ".xlsx" : ".xls"));

        //    // формируем временный файл, в который будет сохраняться напечатанный отчёт
        //    var print_big_data = ((data.DataSet.Tables[0] is VDataTable) && (data.DataSet.Tables[0] as VDataTable).Reader != null);
        //    //if (print_big_data) // вроде, не актуально, было нужно для отчета ВСЯ БАЗА, а теперь он выгружается по ручному шаблону
        //    //{
        //    //    var xtemplate = ExcelTemplate.CreateExcelTemplate(view, data.TableName, caption);
        //    //    var printer = new ExcelPrintDocument(xtemplate);
        //    //    ExcelEnvironment.BeginPrintBigData();
        //    //    printer.Print(null, data.DataSet, true, true);
        //    //    ExcelEnvironment.EndPrintBigData(output_path);
        //    //}
        //    //else
        //    //{
        //    if (prepare)
        //    {
        //        var output_path_xml = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xml");
        //        var xtemplate = ExcelTemplate.CreateExcelTemplate(view, data.TableName, caption);
        //        var printer = new sql.builder.Print.XML.ExcelPrintDocument(xtemplate);
        //        printer.Print(output_path_xml, data.DataSet, print_big_data, false);
        //        var exApp = new Application();
        //        exApp.DisplayAlerts = false;
        //        var wb = exApp.Workbooks.Open(output_path_xml);
        //        foreach (Worksheet sh in wb.Worksheets)
        //        {
        //            sh.Rows.WrapText = true;
        //            sh.Rows.AutoFit();
        //        }
        //        wb.SaveAs(output_path,
        //            (xlsx ? XlFileFormat.xlOpenXMLWorkbook : XlFileFormat.xlExcel8),
        //            ReadOnlyRecommended: false,
        //            AccessMode: XlSaveAsAccessMode.xlNoChange,
        //            ConflictResolution: XlSaveConflictResolution.xlLocalSessionChanges);
        //        exApp.Quit();
        //    }
        //    else// этот новый вариант будет только при экспорте отчета, для остальных случаев оставляю по старому
        //    {

        //        string templatePath = ExcelTemplate.CreateXlsxTemplate(view, data.TableName, caption);
        //        var po = excelPrintOptions;
        //        if (po == null)
        //        {
        //            po = ExcelPrintOptions.Default;
        //        }

        //        if (print_big_data)
        //        {
        //            po = new ExcelPrintOptions();
        //            po.UseDataReader = true;
        //            po.NeedPostProcess = false;
        //        }

        //        ExcelPrintDocument.PrintNew(templatePath, output_path, data.DataSet, po);
        //    }

        //    //}

        //    return output_path;
        //}
        
        public static string Print(DataSet dataSet, string temlplateName, string outputName)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<template name=\"\" title=\"\"  print-proc=\"2\" />");
            xmldoc.FirstChild.Attributes["name"].Value = temlplateName;
            xmldoc.FirstChild.Attributes["title"].Value = outputName;
            return Print(null, dataSet, xmldoc.FirstChild);
        }

        //public static string Print(GridControl[] grids, string[] sheetNames = null, string output_path = null, bool xlsx = true, PrintOptions options = null)
        //{
        //    // достаём view
        //    var baseGridControlInfo = typeof(GridControl).GetField("baseGridControl", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);
        //    var gridViewInfo = baseGridControlInfo.FieldType.GetField("View", BindingFlags.GetField | BindingFlags.Instance | BindingFlags.NonPublic);

        //    var views = new List<GridView>();
        //    foreach (var grid in grids)
        //    {
        //        var baseGridControl = baseGridControlInfo.GetValue(grid);
        //        var view = (GridView)gridViewInfo.GetValue(baseGridControl);
        //        views.Add(view);
        //    }

        //    string[] captions = grids.Select(g => g.Caption);
        //    return Print(views.ToArray(), captions, sheetNames, output_path, xlsx, options);
        //}
        //public static string Print(GridView[] views, string[] captions, string[] sheetNames = null, string output_path = null, bool xlsx = true, PrintOptions options = null)
        //{
        //    options = options ?? PrintOptions.Default;
        //    if (sheetNames == null) sheetNames = captions;

        //    // копировать все данные не очень хорошая идея
        //    // но придется добавлять в DataTable свои колонки из-за lookup-ов
        //    // и делать это на изначальном DataTable идея еще хуже  
        //    var ds = new DataSet();
        //    for (int i = 0; i < views.Length; i++)
        //    {
        //        var dt = ExcelTemplate.PrepareData(views[i], options);
        //        dt.TableName = "a" + i;
        //        ds.Tables.Add(dt);
        //    }

        //    // шаблон
        //    output_path = output_path ?? Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ((xlsx) ? ".xlsx" : ".xls"));

        //    // формируем временный файл, в который будет сохраняться напечатанный отчёт
        //    var output_path_xml = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xml");
        //    var tableNames = ds.Tables.Cast<DataTable>().Select(t => t.TableName).ToArray();
        //    var xtemplate = ExcelTemplate.CreateExcelTemplate(views, tableNames, captions, sheetNames);
        //    var printer = new sql.builder.Print.XML.ExcelPrintDocument(xtemplate);
        //    printer.Print(output_path_xml, ds, false, false);
        //    var exApp = new Application() { DisplayAlerts = false };
        //    exApp.DisplayAlerts = false;
        //    var wb = exApp.Workbooks.Open(output_path_xml);
        //    foreach (Worksheet sh in wb.Worksheets)
        //    {
        //        sh.Rows.WrapText = true;
        //        sh.Rows.AutoFit();
        //    }
        //    wb.SaveAs(output_path,
        //        (xlsx ? XlFileFormat.xlOpenXMLWorkbook : XlFileFormat.xlExcel8),
        //        ReadOnlyRecommended: false,
        //        AccessMode: XlSaveAsAccessMode.xlNoChange,
        //        ConflictResolution: XlSaveConflictResolution.xlLocalSessionChanges);
        //    exApp.Quit();

        //    return output_path;
        //}
        //public static string PrintWord(DataSet dataSet, string temlplateName, string outputName)
        //{
        //    WaitUIHelper.LastUsedUIHelper.Show("Формирование файла", WaitUIMode.WaitPanel, true);
        //    string templatePath = Path.Combine(templatesFolder, "word", temlplateName);
        //    #if DEBUG
        //    // В режиме отладки подтягиваем файл шаблона без необходимости повторной перекомпиляции
        //    if (XmlReports.IsDeveloperMode()) {
        //        string template_path_new = GetFreeName(outputFolder, Path.GetFileNameWithoutExtension(templatePath) + "-template", ".docx");
        //        File.Copy(templatePath, template_path_new);
        //        templatePath = template_path_new;
        //    }
        //    #endif
        //    string fileName = GetFreeName(outputFolder, outputName, "docx");
        //    var template = new WordTemplate(templatePath);
        //    using (var report = new WordReport())
        //    {
        //        report.Print(template, dataSet);
        //        report.SaveToFile(fileName);
        //    }

        //    WaitUIHelper.LastUsedUIHelper.Hide();
        //    //Wait.Hide();

        //    return fileName;
        //}
       
        
        public static string Print(XmlNode data, DataSet dataSet, XmlNode temlplateInfo, XDocument xTemplate = null, bool show_messages = true)
        {
            WaitUIHelper.LastUsedUIHelper.Show("Формирование файла", WaitUIMode.WaitPanel, true);
            //Wait.Show("Формирование файла", over: true);
            string templateType = temlplateInfo.ParentNode.Name;
            string templatePath = Path.Combine(templatesFolder, templateType, temlplateInfo.Attributes[TextConst.AName.Name].Value);
            #if DEBUG
            // в режиме отладки добавляем шаблон в проект sql.builder.templates и ТФС
            //if (XmlReports.IsDeveloperMode()) {
            //    ExcelPrintDocument.ReloadExcelTemplate(templatePath, templateType);
            //}
            #endif
            string title = temlplateInfo.Attributes[TextConst.AName.Title].Value;
            string fileName;
            if (templateType == "word") {
                throw new NotImplementedException();
                //fileName = PrintWord(dataSet, templatePath, title, temlplateInfo);
            } else {
                fileName = printExcel(data, dataSet, templatePath, title, temlplateInfo, xTemplate, show_messages);
            }
            WaitUIHelper.LastUsedUIHelper.Hide();
            //Wait.Hide();
            return fileName;
        }

        private static XmlNamespaceManager excelNamespaseManager;
        // 

        ///// <summary>
        ///// Перенос формата из шаблона в результирующий отчет
        ///// </summary>
        ///// <param name="templatePath">путь к шаблону</param>
        ///// <param name="fileName">путь к отчету</param>
        //internal static void FormattingFile(string templatePath, Workbook workbook)
        //{
        //    Worksheet destScheet = (Worksheet)workbook.ActiveSheet;

        //    // сделал копию xlsx, так как xml не открывается при запуске из TaskScheduler %)
        //    //if (templatePath.EndsWith("41293.xml")) templatePath = Path.ChangeExtension(templatePath, ".xlsx");

        //    Workbook srcBook = workbook.Application.Workbooks.Open(templatePath);
        //    Worksheet srcScheet = (Worksheet)srcBook.ActiveSheet;

        //    //определяем диапазон шапки в шаблоне
        //    Range endrange = srcScheet.Cells.Find("begin");
        //    var last = ExcelUtils.GetColumnName(endrange.Column) + ((endrange.Row > 1) ? (endrange.Row - 1) : 1);

        //    Range srcRange = srcScheet.Range["A1", last];
        //    Range destRange = destScheet.Range["A1", last];
        //    srcRange.Copy(destRange);

        //    //проставляем ширину колонок (при вставке не работает)
        //    for (int i = 1; i <= endrange.Column - 1; i++)
        //    {
        //        destScheet.Cells[1, i].ColumnWidth = srcScheet.Cells[1, i].ColumnWidth;
        //    }
        //}
        
        
        public static ExcelPrintDocument.ExcelPrintErrors printExcel(XDocument template, DataSet dataSet, string filename)
        {
            Contract.Assert(!string.IsNullOrEmpty(filename));
            var doc = new sql.builder.Print.XML.ExcelPrintDocument(template);
            return doc.Print(filename, dataSet, false, false);
        }
        public static string printExcel(XmlNode data, DataSet dataSet, string templatePath, string title, XmlNode templateInfo, XDocument Template = null, bool show_empty_message = true)
        {
            bool print_big_data = ((dataSet.Tables[0] is VDataTable) && (dataSet.Tables[0] as VDataTable).Reader != null);
            bool convertToOpenXml;
            XmlAttribute attr = templateInfo.Attributes[TextConst.AName.ConvertToOpenXml];
            if (print_big_data) {
                if (attr != null && attr.Value == TextConst.AVBool.False) {
                    convertToOpenXml = false;
                } else {
                    convertToOpenXml = true;
                }
            } else {
                if (attr != null && attr.Value == TextConst.AVBool.True) {
                    convertToOpenXml = true;
                } else {
                    convertToOpenXml = false;
                }
            }
            //
            attr = templateInfo.Attributes["print-proc"];
            bool isNewProc = (attr != null && attr.Value == "2");
            //
            bool multipage = templateInfo.AttrOrDefault("multipage", false);
            //
            XDocument xTemplate;
			if (Template != null) {
				xTemplate = Template;
			} else {
                using (FileStream templateStream = File.Open(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)) {
                    xTemplate = XDocument.Load(templateStream);
                    templateStream.Close();
                }                
			}
            xTemplate = colsProcessing(data, xTemplate);
            if (templateInfo.AttrOrDefault(TextConst.AName.DelCols, false)) { // Пока удаление колонок только для нового варианта, а колонки по измерениям только для старого
                xTemplate = delUnused(dataSet, xTemplate);
            }
            //
            XmlAttribute output_format_attr = templateInfo.Attributes["output-format"];
            string output_format;
            if (output_format_attr == null) {
                output_format = "xlsx";
            } else {
                output_format = output_format_attr.Value;
                if (output_format != "pdf") {
                    output_format = "xlsx";
                }
            }
            string format;
            if (convertToOpenXml /*|| ExcelPrintDocument.autoConvert*/) {
                format = "xlsx";
            } else {
                format = "xml";
            }
            string fileName = GetFreeName(outputFolder, title, output_format);
            string fileNameX = GetFreeName(outputFolder, title, format);
            ExcelPrintDocument.ExcelPrintErrors err = ExcelPrintDocument.ExcelPrintErrors.None;
            sql.builder.Print.XML.ExcelPrintDocument doc = null;
            if (isNewProc) {
                doc = new sql.builder.Print.XML.ExcelPrintDocument(xTemplate);
                xTemplate = null;
                if (convertToOpenXml) {
                    ExcelEnvironment.BeginPrintBigData();
                    err = doc.Print(null, dataSet, print_big_data, true);
                    ExcelEnvironment.EndPrintBigData(fileName);
                } else {
                    err = doc.Print(fileNameX, dataSet, print_big_data, false);
                    doc = null;
                }
            } else {
                XmlDocument template = new XmlDocument();
                template.PreserveWhitespace = true;
                template.Load(templatePath);
                using (MemoryStream xmlStream = new MemoryStream()) {
                    xTemplate.Save(xmlStream);
                    xmlStream.Flush();
                    xmlStream.Position = 0;
                    template.Load(xmlStream);
                }
                excelNamespaseManager = new XmlNamespaceManager(template.NameTable);
                excelNamespaseManager.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
                XmlNode sheet = template.SelectSingleNode(".//*[name()='ss:Worksheet']");
                // sheet.SelectSingleNode(".//*[name()='Table']").Attributes["ss:ExpandedColumnCount"].Value = (getLastColumn(sheet)+1).ToString();
                applySource(sheet, 1, data.SelectSingleNode(".//data"), "", null, multipage, new SortedList<string, SortedList<string, Tuple<int, XmlNode>>>());
                XmlNodeList sheets = template.SelectNodes(".//*[name()='ss:Worksheet']");
                foreach (XmlNode sheet1 in sheets) {
                    XmlNodeList rows = sheet1.SelectSingleNode(".//*[name()='ss:Table']").SelectNodes(".//*[name()='ss:Row']");
                    foreach (XmlNode row in rows) {
                        row.RemoveAttribute("ss:Index");
                    }
                    sheet1.SelectSingleNode(".//*[name()='ss:Table']").Attributes["ss:ExpandedRowCount"].Value = (rows.Count + 1).ToString();
                }
                if (sheets.Count < 1) {
                    return string.Empty;
                }
                /* XmlNodeList markers = template.SelectNodes("//*[name()='Data' and contains(.,'begin:')]");
                 foreach (XmlNode marker in markers) {
                     int i= getMarkerColumn( marker);
                 }
                 */
                template.Save(fileNameX);
            }
            if (err == ExcelPrintDocument.ExcelPrintErrors.NoData) {
                //if (show_empty_message) XtraMessageBox.Show("По заданным условиям нет данных для печати");
                // зачем этот файл сохраняется??
                if (File.Exists(fileNameX)) File.Delete(fileNameX);
                return string.Empty;
            }
            if (convertToOpenXml) {
                bool xlsb = (output_format_attr != null && output_format_attr.Value == "xlsb");
                fileName = ExcelPrintDocument.PostProcessBigData(fileNameX, templatePath, xlsb);
            } else {
                fileName = ExcelPrintDocument.PostProcess(fileNameX, output_format, fileName, null);
            }
            return fileName;
        }
        //public static string PrintWord(DataSet dataSet, string templatePath, string title, XmlNode templateInfo)
        //{
        //    // формируем путь к временному файлу, в который будет сохраняться напечатанный отчёт
        //    string output_path = GetFreeName(outputFolder, title, "docx");
        //    #if DEBUG
        //    if (XmlReports.IsDeveloperMode()) {
        //        string templatePathNew = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".docx");
        //        File.Copy(templatePath, templatePathNew);
        //        templatePath = templatePathNew;
        //    }
        //    #endif           
        //    WordTemplate template = null;
        //    WordReport report = null;
        //    try {
        //        // инициализируем отчёт
        //        template = new WordTemplate(templatePath);
        //        report = new WordReport();
        //        // заполняем шаблон данными и печатаем результат в буфер
        //        report.Print(template, dataSet);
        //        // report.Print(template, dataSet.Tables["obj"].Rows[0]);
        //        // сохраняем итоговый документ из буфера в файл
        //        report.SaveToFile(output_path);
        //    } finally {
        //        // освобождаем ресурсы
        //        if (report != null) {
        //            Cmn.DisposeAndSetNull(ref report);
        //        }
        //        if (template != null) {
        //            Cmn.DisposeAndSetNull(ref template);
        //        }
        //    }
        //    return output_path;
        //}
       
        
        //internal static void MergeDownWorksheet(Worksheet sh, int first_row_index = 1)
        //{
        //    Range cur_cell = null;
        //    string prev_val = null;
        //    string cur_val = null;
        //    IList<Range> cells_to_merge = new List<Range>();
        //    Range range_used = sh.UsedRange;
        //    int cols_count = range_used.Columns.Count;
        //    int rows_count = range_used.Rows.Count - first_row_index + 1;
        //    // битовая карта: для каждой строки true - можно объединять, false - нельзя
        //    IDictionary<string, bool[]> bords_dict = new Dictionary<string, bool[]>();
        //    // перебираем колонки
        //    for (int cn = 1; cn <= cols_count; cn++) {
        //        // ищем признак, что эту колонку надо обрабатывать
        //        Range merge_down_cell = sh.Columns[cn].Find(TextConst.ExcelMarks.MergeDown, Type.Missing, XlFindLookIn.xlValues, XlLookAt.xlPart, XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false, false, false);
        //        if (merge_down_cell == null) {
        //            bords_dict.Add("col" + cn, null);
        //            continue;
        //        }
        //        // достаем key, если есть
        //        Match result = Regex.Match(merge_down_cell.Text, @"\[key:([a-zA-Z0-9_]+)\]");
        //        string key = (result.Success) ? result.Groups[1].Value : ("col" + cn);
        //        // достаем use_key, если есть
        //        result = Regex.Match(merge_down_cell.Text, @"\[use_key:([a-zA-Z0-9_]+)\]");
        //        string use_key = (result.Success) ? result.Groups[1].Value : null;
        //        bool[] cur_bords;
        //        if (use_key != null) {
        //            cur_bords = bords_dict[use_key];
        //        } else {
        //            //var bords = bords_dict.Values.LastOrDefault(v => v != null);
        //            //cur_bords = bords ?? Enumerable.Repeat(true, rows_count).ToArray();
        //            cur_bords = bords_dict.Values.LastOrDefault(v => v != null);
        //            if (cur_bords == null) {
        //                cur_bords = new bool[rows_count];
        //                Array.Fill<bool>(cur_bords, true);
        //            }
        //        }
        //        // затираем признак
        //        merge_down_cell.Value = null;
        //        // храним предыдущею ячейку и ее значение в переменных
        //        prev_val = null;
        //        int last_row_index = Math.Min(merge_down_cell.Row - 1, range_used.Rows.Count);
        //        for (int rn = first_row_index; rn <= last_row_index; rn++) {
        //            // текущая ячейка
        //            cur_cell = sh.Cells[rn, cn];
        //            // текущее значение                    
        //            cur_val = cur_cell.Value != null ? cur_cell.Value.ToString() : null;
        //            bool allow_merge = cur_bords[rn - first_row_index];
        //            bool same_values = (
        //                //prev_val != null && cur_val != null && // Бельченко 26062017 убрал, чтобы пустые тоже объединялись, но возможно будут проблемы в других отчетах!?
        //                prev_val == cur_val);
        //            if (cells_to_merge.Count == 0 || (allow_merge && same_values)) {
        //                cells_to_merge.Add(cur_cell);
        //            } else {
        //                MergeCells(sh, cells_to_merge);
        //                cells_to_merge.Clear();
        //                cells_to_merge.Add(cur_cell);
        //                // запрещаем объединять с этой строкой
        //                cur_bords[rn - first_row_index] = false;
        //            }
        //            prev_val = cur_val;
        //        }
        //        MergeCells(sh, cells_to_merge);
        //        bords_dict.Add(key, cur_bords);
        //        cells_to_merge.Clear();
        //    }
        //}
        //internal static void MergeRightWorksheet(DWorksheet sh, int first_column_index = 0)
        //{
        //    Cell cur_cell = null;
        //    string prev_val = null;
        //    string cur_val = null;
        //    IList<Cell> merge_rights_cells = sh.Search(TextConst.ExcelMarks.MergeRight).ToList();
        //    foreach (Cell merge_rights_cell in merge_rights_cells) {
        //        var cells_to_merge = new List<DRange>();
        //        prev_val = null;
        //        for (int cn = first_column_index; cn < merge_rights_cell.ColumnIndex; cn++) {
        //            cur_cell = sh.Cells[merge_rights_cell.RowIndex, cn].First();
        //            cur_val = cur_cell.DisplayText;
        //            bool same_values = (prev_val != null && cur_val != null && prev_val == cur_val);
        //            if (cells_to_merge.Count == 0 || same_values) {
        //                cells_to_merge.Add(cur_cell);
        //            } else {
        //                MergeCells(sh, cells_to_merge);
        //                cells_to_merge.Clear();
        //                cells_to_merge.Add(cur_cell);
        //            }
        //            prev_val = cur_val;
        //        }
        //        // затираем признак
        //        merge_rights_cell.SetValue(null);
        //    }
        //}
        //internal static void MergeRightWorksheet(XlsFile excel, TCellAddress[] mergeright, int first_column_index = 1)
        //{
        //    object prev_val = null;
        //    object cur_val = null;

        //    foreach (var cell in mergeright)
        //    {
        //        var cells_to_merge = new List<Tuple<int, int>>();
        //        prev_val = null;

        //        for (int cn = first_column_index; cn < cell.Col; cn++)
        //        {
        //            if (cn == 7 && cell.Row == 9)
        //            {

        //            }

        //            cur_val = excel.GetStringFromCell(cell.Row, cn);

        //            var same_values = (prev_val != null && cur_val != null && prev_val.Equals(cur_val));
        //            if (cells_to_merge.Count == 0 || same_values)
        //            {
        //                cells_to_merge.Add(new Tuple<int, int>(cell.Row, cn));
        //            }
        //            else
        //            {
        //                if (cells_to_merge.Count > 1) {
        //                    var c1 = cells_to_merge[0];
        //                    var c2 = cells_to_merge[cells_to_merge.Count - 1];
        //                    excel.MergeCells(c1.Item1, c1.Item2, c2.Item1, c2.Item2);
        //                }

        //                cells_to_merge.Clear();
        //                cells_to_merge.Add(new Tuple<int, int>(cell.Row, cn));
        //            }
        //            prev_val = cur_val;
        //        }

        //        // затираем признак
        //        excel.SetCellValue(cell.Row, cell.Col, null);
        //    }
        //}
        //private static void MergeCells(DWorksheet sh, IList<DRange> cells)
        //{
        //    int count = cells.Count;
        //    if (count > 1) {
        //        DRange first = cells[0];
        //        DRange last = cells[count - 1];
        //        DRange range = sh.Range.FromLTRB(first.LeftColumnIndex, first.TopRowIndex, last.RightColumnIndex, last.BottomRowIndex);
        //        range.Merge();
        //    }
        //}
        //private static void MergeCells(Worksheet sh, IList<Range> cells)
        //{
        //    int count = cells.Count;
        //    if (count > 1) {
        //        Range first = cells[0];
        //        Range last = cells[count - 1];
        //        Range range = sh.Range[first, last];
        //        range.Merge();
        //    }
        //}
        //internal static void DeleteRanges(DWorksheet sh)
        //{
        //    var namesLeft = new List<string>();
        //    var namesUp = new List<string>();// пятый параметр =1 !delete:2,2,2,3,1
        //    var options = new SearchOptions() { SearchIn = SearchIn.Values };
        //    IList<Cell> cells = sh.Search(TextConst.ExcelMarks.DeleteRanges + ":", options).ToList();
        //    foreach (Cell cell in cells) {
        //        string cords_str = cell.Value.ToString().SubstringAfter(':');
        //        //IList<int> cords = cords_str.Split(',').Select(s => int.Parse(s) - 1);
        //        string[] str_cords = cords_str.Split(',');
        //        int[] cords = new int[str_cords.Length];
        //        for (int index_2 = 0; index_2 < str_cords.Length; index_2++) {
        //            cords[index_2] = int.Parse(str_cords[index_2]) - 1;
        //        }
        //        var range = sh.Range.FromLTRB(cords[1], cords[0], cords[3], cords[2]);
        //        if (cords.Length > 4 && cords[4] == 0) {
        //            namesUp.Add(range.GetReferenceA1());
        //        } else {
        //            namesLeft.Add(range.GetReferenceA1());
        //        }
        //    }
        //    if (namesLeft.Count != 0) {
        //        var range_to_delete = sh.Range[string.Join(", ", namesLeft)];
        //        range_to_delete.Delete(DeleteMode.ShiftCellsLeft);
        //        //range_to_delete.FillColor = Color.Red;
        //    }
        //    if (namesUp.Count != 0) {
        //        var range_to_delete = sh.Range[string.Join(", ", namesUp)];
        //        range_to_delete.Delete(DeleteMode.ShiftCellsUp);
        //        //range_to_delete.FillColor = Color.Red;
        //    }
        //}
        //private static IList<Range> FindMark(Worksheet sh, string mark)
        //{
        //    List<Range> ranges = new List<Range>();
        //    Range currentFind = sh.UsedRange.Find(mark, Type.Missing, XlFindLookIn.xlValues, XlLookAt.xlPart, XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false);
        //    string first_address = null;
        //    while (currentFind != null) {
        //        string address = currentFind.Address[XlReferenceStyle.xlA1];
        //        if (first_address == null) {
        //            first_address = address;
        //        } else if (address == first_address) {
        //            break;
        //        }
        //        ranges.Add(currentFind);
        //        currentFind = sh.UsedRange.FindNext(currentFind);
        //    }
        //    return ranges;
        //}
        ///// <summary>
        ///// Обработка пометки !deleterow
        ///// </summary>
        ///// <param name="sh">лист Excel</param>
        ///// <seealso cref="TextConst.ExcelMarks.DeleteRow"/>
        //internal static void DeleteRows(Worksheet sh)
        //{
        //    SortedSet<int> rows = new SortedSet<int>();
        //    Range currentFind = sh.UsedRange.Find(TextConst.ExcelMarks.DeleteRow, Type.Missing, XlFindLookIn.xlValues, XlLookAt.xlPart, XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false);
        //    string first_address = null;
        //    while (currentFind != null) {
        //        string address = currentFind.Address[XlReferenceStyle.xlA1];
        //        if (first_address == null) {
        //            first_address = address;
        //        } else if (address == first_address) {
        //            break;
        //        }
        //        int row = currentFind.Row;
        //        if (!rows.Contains(row)) {
        //            rows.Add(row);
        //        }
        //        currentFind = sh.UsedRange.FindNext(currentFind);
        //    }
        //    using (IEnumerator<int> e = rows.Reverse().GetEnumerator()) {
        //        while (e.MoveNext()) {
        //            sh.Rows[e.Current].Delete();
        //        }
        //    }
        //}
        ///// <summary>
        ///// Обработка пометки !rowheight:&lt;число&gt;
        ///// </summary>
        ///// <param name="sh">лист Excel</param>
        ///// <seealso cref="TextConst.ExcelMarks.RowHeight"/>
        //internal static void SetRowsHeight(Worksheet sh)
        //{
        //    string mark = TextConst.ExcelMarks.RowHeight + ":";
        //    IList<Range> ranges = FindMark(sh, mark);
        //    /*var ranges = new List<Range>();
        //    Range currentFind = sh.UsedRange.Find(TextConst.ExcelMarks.RowHeight, Type.Missing, XlFindLookIn.xlValues, XlLookAt.xlPart, XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false);
        //    Range firstFind = null;
        //    while (currentFind != null) {
        //        // Keep track of the first range you find. 
        //        if (firstFind == null) {
        //            firstFind = currentFind;
        //        } else if (currentFind.Address[XlReferenceStyle.xlA1] == firstFind.Address[XlReferenceStyle.xlA1]) { // If you didn't move to a new range, you are done.
        //            break;
        //        }
        //        // вынес изменение высоты в отдельный цикл, т.к. лезла ошибка 36257(1) 
        //        ranges.Add(currentFind);
        //        currentFind = sh.UsedRange.FindNext(currentFind);
        //    }*/
        //    CultureInfo ci = CultureInfo.InvariantCulture;
        //    for (int index = 0; index < ranges.Count; index++) {
        //        Range range = ranges[index];
        //        string val = range.Value2.ToString();
        //        //object height = Cmn.ToDecimal(val.Split(':')[1]);
        //        int pos = val.IndexOf(mark);
        //        Contract.Assert(pos >= 0);
        //        val = val.Substring(pos + mark.Length);
        //        if (val.IndexOf(',') >= 0) {
        //            val = val.Replace(',', '.');
        //        }
        //        double height;
        //        if (double.TryParse(val, NumberStyles.AllowDecimalPoint, ci, out height)) {
        //            range.EntireRow.RowHeight = (object)height;
        //            range.Value2 = null;
        //        }
        //    }
        //}
        ///// <summary>
        ///// Обработка пометки !columnwidth:&lt;число&gt;
        ///// </summary>
        ///// <param name="sh">лист Excel</param>
        ///// <seealso cref="TextConst.ExcelMarks.ColumnWidth"/>
        //internal static void SetColumnsWidth(Worksheet sh)
        //{
        //    string mark = TextConst.ExcelMarks.ColumnWidth + ":";
        //    IList<Range> ranges = FindMark(sh, mark);
        //    CultureInfo ci = CultureInfo.InvariantCulture;
        //    for (int index = 0; index < ranges.Count; index++) {
        //        Range range = ranges[index];
        //        string val = range.Value2.ToString();
        //        int pos = val.IndexOf(mark);
        //        Contract.Assert(pos >= 0);
        //        val = val.Substring(pos + mark.Length);
        //        double width;
        //        if (double.TryParse(val, NumberStyles.AllowDecimalPoint, ci, out width)) {
        //            range.EntireColumn.ColumnWidth = (object)width;
        //            range.Value2 = null;
        //        }
        //    }
        //}
     
        //internal static void SetAutoWidth(Worksheet sh)
        //{
        //    Range firstFind = null;
        //    Range currentFind = sh.UsedRange.Find(TextConst.ExcelMarks.AutoRowHeight, Missing.Value,
        //        XlFindLookIn.xlValues, XlLookAt.xlPart,
        //        XlSearchOrder.xlByRows, XlSearchDirection.xlNext, false,
        //        Missing.Value, Missing.Value);
        //    while (currentFind != null) {
        //        if (firstFind == null) {
        //            firstFind = currentFind;
        //        } else if (currentFind.get_Address(XlReferenceStyle.xlA1) == firstFind.get_Address(XlReferenceStyle.xlA1)) {
        //            break;
        //        }
        //        currentFind.Clear();
        //        currentFind.Rows.AutoFit();
        //        // иногда по странной причине падает с Exception-ом
        //        try {
        //            currentFind = sh.UsedRange.FindNext(currentFind);
        //        } catch (Exception) {
        //            currentFind = null;
        //        }
        //    }
        //}
       
        //internal static void ProtectSheet(DWorksheet sh)
        //{
        //    var options = new SearchOptions() { SearchIn = SearchIn.Values };
        //    var cells = sh.GetUsedRange().Search(TextConst.ExcelMarks.ProtectSheet, options);

        //    string pass = "qqq";
        //    foreach (var cell in cells)
        //    {
        //        var val = cell.Value.TextValue;
        //        var items = val.Split(':');
        //        if (items.Length > 1) pass = items[1];

        //        cell.Clear();
        //    }

        //    sh.Protect(pass, WorksheetProtectionPermissions.Default);
        //}

        //internal static void SetPageBreaks(DWorksheet sh)
        //{
        //    var options = new SearchOptions() { SearchIn = SearchIn.Values };
        //    var cells = sh.GetUsedRange().Search(TextConst.ExcelMarks.PageBreak, options).ToArray();

        //    foreach (var cell in cells)
        //    {
        //        sh.HorizontalPageBreaks.Add(cell.RowIndex);
        //        cell.Clear();
        //    }
        //}

        //internal static void SetPrintTitleRows(DWorksheet sh)
        //{
        //    var options = new SearchOptions();
        //    options.SearchIn = SearchIn.Values;
        //    var cells = sh.GetUsedRange().Search(TextConst.ExcelMarks.PrintTitleRows, options).ToList();
        //    sh.PrintOptions.PrintTitles.SetRows(cells[0].RowIndex, cells[cells.Count - 1].RowIndex);
        //    foreach (var cell in cells) {
        //        cell.Clear();
        //    }
        //}

        //internal class BlocksRules
        //{
        //    internal int ColumnsCount { get; private set; }

        //    internal BlocksRules(string text)
        //    {
        //        var match = Regex.Match(text, @"\[columns\-count\:([0-9])]");
        //        ColumnsCount = (match.Success) ? Int32.Parse(match.Groups[1].Value) : 0;
        //    }
        //}

        //internal static void ProcessBlocks(DWorksheet dsh)
        //{
        //    string BlockTopLeft = "[block:top-left]";
        //    string BlockBottomRight = "[block:bottom-right]";

        //    var options = new SearchOptions() { SearchIn = SearchIn.Values };
        //    var cellsBlockRules = dsh.GetUsedRange().Search("[blocks-rules", options).FirstOrDefault();
        //    if (cellsBlockRules == null) return;

        //    var blocksRules = new BlocksRules(cellsBlockRules.Value.ToString());
        //    cellsBlockRules.Value = null;

        //    var cellsTopLeft = dsh.GetUsedRange().Search(BlockTopLeft, options).ToArray();
        //    var cellsBottomRight = dsh.GetUsedRange().Search(BlockBottomRight, options).ToArray();

        //    var blockRanges = new List<DRange>();

        //    for (int i = 0; i < cellsTopLeft.Length; i++)
        //    {
        //        var cellTopLeft = cellsTopLeft[i];
        //        var cellBottomRight = cellsBottomRight[i];

        //        var range = dsh.Range.FromLTRB(
        //            cellTopLeft.ColumnIndex + 1, 
        //            cellTopLeft.RowIndex + 1, 
        //            cellBottomRight.ColumnIndex - 1, 
        //            cellBottomRight.RowIndex - 1);

        //        blockRanges.Add(range);

        //        cellTopLeft.Value = null;
        //        cellBottomRight.Value = null;
        //    }

        //    // алгоритм только для конкретного случая (43841)

        //    int rowNumber = 1;
        //    int colNumber = 1;
        //    int rowCord = 0;
        //    int colCord = 0;
        //    int maxRowsInBlock = 0;
        //    bool first = true;

        //    foreach (DRange range in blockRanges)
        //    {
        //        if (first)
        //        {
        //            rowCord = range.TopRowIndex;
        //            colCord = range.LeftColumnIndex;
        //            first = false;
        //        }
        //        else
        //        {
        //            int tempColumn = 1000;
        //            int tempRow = 1000;

        //            // если копируемый диапазон и куда копируем пересекаются, то будут глюки
        //            // поэтому копируем в 2 этапа
        //            DRange destRange = dsh.Cells[rowCord, colCord];
        //            range.MoveTo(dsh.Cells[tempRow, tempColumn]);

        //            var range2 = dsh.Range.FromLTRB(
        //                tempColumn,
        //                tempRow,
        //                tempColumn + range.ColumnCount - 1,
        //                tempRow + range.RowCount - 1);
        //            range2.MoveTo(destRange);
        //        }

        //        if (range.RowCount > maxRowsInBlock)
        //        {
        //            maxRowsInBlock = range.RowCount;
        //        }

        //        colNumber++;
        //        colCord += range.ColumnCount;

        //        if (colNumber > blocksRules.ColumnsCount)
        //        {
        //            rowNumber++;
        //            rowCord += maxRowsInBlock;

        //            colNumber = 1;
        //            colCord = blockRanges[0].LeftColumnIndex;

        //            maxRowsInBlock = 0;
        //        }
        //    }
        //}

        private static XElement ret;// чтобы не обявлять каждый раз при вызове .Cell



        public static XDocument delUnused(DataSet data, XDocument template)
        {
            //MemoryStream xmlStream = new MemoryStream();


            //template.Save(xmlStream);

            //xmlStream.Flush();
            //xmlStream.Position = 0;


            VExcelWorkbook wb = new VExcelWorkbook(template);
            for (int si = 0; si < wb.SheetsCount(); si++)
            {

                VExcelSheet sheet = wb.Sheet(si);

                int rowsCount = Convert.ToInt32(sheet.Element.Descendants().Attributes(VExcelNS.SpreadSheet.ExpandedRowCount).First().Value);

                List<int> indexesForDelete = new List<int>();

                foreach (DataTable table in data.Tables)
                {
                    string alias = table.TableName;
                    List<VExcelCell> cells = sheet.FindCells("[:" + alias + ".");
                    indexesForDelete.Clear();
                    foreach (VExcelCell cell in cells) {
                        //if (cell.Value.ToString() == "[:a.i.pow_cnt_proch_wait]")
                        //{
                        //}
                        MatchCollection matches = Regex.Matches(cell.Value, @"\[:" + alias + @"\.[A-z0-9_\.]*\]");


                        foreach (Match m in matches)
                        {


                            string[] colNameA = m.Value.ToString().Replace("[:" + alias + ".", "").Replace("]", "").Split('.');

                            string colName = colNameA[colNameA.Length - 1];

                            if (!table.Columns.Contains(colName))
                            {
                                int i = cell.Index;
                                if (!indexesForDelete.Contains(i)) {
                                    indexesForDelete.Add(i);
                                }
                                break;
                            }
                        }




                    }


                    int i1 = 0;
                    foreach (int i in indexesForDelete.OrderByDescending(e => e))
                    {

                        VExcelCell c1 = sheet.Row(1).Cell(i);
                        VExcelRow r2 = sheet.Row(rowsCount);
                        VExcelCell c2 = r2.Cell(i);
                        VExcelRange range = new VExcelRange(c1, c2);

                        //  range.Remove();
                        if (i1 == 6)
                        {

                            range.Remove();
                            //  break;
                        }
                        else
                        {
                            range.Remove();
                        }
                        i1++;
                    }

                    //template
                    //template.Load(wb.SaveToStream());
                    //wb.Document.Save(@"C:\Users\abelchenko\Desktop\Новая папка\test.xml");




                }
            }


            return wb.Document;


        }



        /*public static XElement ExtractHeadInfo(string templateName)
        {
            string templatePath = Path.Combine(templatesFolder, "excel", templateName);
            XDocument doc = XDocument.Load(templatePath);
            return ExtractHeadInfo(doc);
        }*/

        /*public static XElement ExtractHeadInfo(XDocument template)
        {
            XElement xroot = new XElement(TextConst.EName.ExcelTemplate);
            VExcelWorkbook wb = new VExcelWorkbook(template);
            for (int si = 0; si < wb.SheetsCount(); si++) {
                VExcelSheet sheet = wb.Sheet(si);
                var xsheet = new XElement(TextConst.EName.ExcelSheet);
                xroot.Add(xsheet);
                xsheet.SetAttributeValue(TextConst.AName.Title, sheet.GetName());
                List<VExcelCell> cells = sheet.FindCells(TextConst.ExcelMarks.HeadMarker);
                List<int> headIndexes = new List<int>();
                foreach (VExcelCell cell in cells) {
                    headIndexes.Add(cell.RowIndex);
                }
                int lastHeadRowIndex = headIndexes.Max();
                SortedList<int, List<string>> colVarList = new SortedList<int, List<string>>();
                for (int i = 0; i < cells[0].Index; i++) {
                    colVarList.Add(i, new List<string>());
                }
                cells = sheet.FindCells("[:");
                foreach (VExcelCell cell in cells) {
                    if (cell.RowIndex > lastHeadRowIndex) {
                        MatchCollection matches = Regex.Matches(cell.Value, @"\[:[A-z0-9_\.]*\]");
                        int index = cell.Index;
                        if (!colVarList.ContainsKey(index)) {
                            colVarList.Add(index, new List<string>());
                        }
                        foreach (Match m in matches) {
                            string svar = m.Value.Replace("[:", "").Replace("]", "");
                            if (!colVarList[index].Contains(svar)) {
                                colVarList[index].Add(svar);
                            }
                        }
                    }
                }
                SortedList<int, string> prevTitles = new SortedList<int, string>();
                foreach (int i in colVarList.Keys) {
                    string stitle = string.Empty;
                    string q = string.Empty;
                    foreach (int j in headIndexes) {
                        VExcelCell cell = sheet.Row(j).Cell(i);
                        string stitle1;
                        if (cell != null) {
                            stitle1 = cell.Value ?? string.Empty;
                        } else if (prevTitles.ContainsKey(j)) {
                            stitle1 = prevTitles[j];
                        } else {
                            stitle1 = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(stitle1)) {
                            if (!prevTitles.ContainsKey(j)) {
                                prevTitles.Add(j, string.Empty);
                            }
                            prevTitles[j] = stitle1;
                            stitle += q + stitle1;
                        }
                        q = " / ";
                    }
                    XElement colInfo = new XElement(TextConst.EName.ExcelColumn, new XAttribute(TextConst.AName.Title, stitle));
                    foreach (string svar in colVarList[i].Distinct()) {
                        colInfo.Add(new XElement(TextConst.EName.Column, new XText(svar)));
                    }
                    xsheet.Add(colInfo);
                }
            }
            return xroot;
        }*/
        public static XDocument colsProcessing(XmlNode data, XDocument template)
        {
            //MemoryStream xmlStream = new MemoryStream();
            // template.Save(xmlStream);
            // xmlStream.Flush();
            // xmlStream.Position = 0;
            VExcelWorkbook wb = new VExcelWorkbook(template);
            for (int sheetIndex = 0; sheetIndex < wb.SheetsCount(); sheetIndex++) {
                VExcelSheet sheet = wb.Sheet(sheetIndex);
                VExcelCell cell1 = sheet.FindCell("cbegin");
                while (cell1 != null) {
                    string tableName = cell1.Value.Split(' ')[0].Split(':')[1];
                    VExcelCell cell2 = sheet.FindCell("cend:" + tableName, cell1.Index);
                    if (cell2 == null) {
                        // template.Load(wb.SaveToStream());
                        //template.Save(@"C:\tfs\all\sql.builder\sql.builder\printTemplate\excel\25499-test3.xml");
                        cell2 = sheet.FindCell("cend:" + tableName, cell1.Index);
                    }
                    cell1.SetValue(string.Empty);
                    cell2.SetValue(string.Empty);
                    int rangeWidth = cell2.Index - cell1.Index;
                    VExcelRange range = new VExcelRange(cell1, cell2);
                    VExcelRange newRange = range;
                    int i = 0;
                    XmlNode dimsNode = data.SelectSingleNode("root/scheme//table/dimension-values[@table='" + tableName + "']");
                    if (dimsNode != null) { // null может быть при исключении колонок через colset
                        XmlNodeList dims = dimsNode.SelectNodes(".//val");
                        foreach (XmlNode dim in dims) {
                            if (i < dims.Count - 1) {
                                VExcelCell tagCell = newRange.FirstCell.Row.Cell(newRange.LastCell.Index + 1, ref ret);
                                newRange = tagCell.Insert(range);
                            }
                            i++;
                        }
                        i = 0;
                        newRange = range;
                        VExcelCell cell11 = cell1;
                        VExcelCell cell22 = cell2;
                        foreach (XmlNode dim in dims) {
                            newRange.Replace("[" + tableName + ".title]", dim.Attributes["title"].Value);
                            if (dim.Attributes["columnpref"] != null) {
                                newRange.Replace("[" + tableName + ".columnpref]", dim.Attributes["columnpref"].Value);
                            }
                            newRange.Replace("[" + tableName + ".pfx]", "_" + dim.Attributes["value"].Value.Replace("-", "_").Replace(".", "_").Replace(",", "_"));
                            newRange.Replace("[merge]", "");
                            cell11 = cell1.Row.Cell(cell11.Index + rangeWidth + 1, ref ret);
                            cell22 = cell22.Row.Cell(cell22.Index + rangeWidth + 1, ref ret);
                            newRange = new VExcelRange(cell11, cell22);
                            i++;
                        }
                    }
                    // range.Remove();
                    cell1 = sheet.FindCell("cbegin");
                }
            }
            //wb.Document.Save(@"C:\Users\abelchenko\Desktop\Новая папка\test.xml");
            return wb.Document;
        }
        //private static XmlNode rootData;
        //private static bool multipage = false;
        //private static SortedList<string, SortedList<string, int>> columnsIndexesList; // = new SortedList<string, SortedList<string, int>>();
        //private static SortedList<string, SortedList<string, XmlNode>> columnsInfoList; // = new SortedList<string, SortedList<string, XmlNode>>();
        internal static bool applySource(XmlNode templatePart, int sourceIndex, XmlNode data, string parentName, XmlNode rootData, bool multipage, SortedList<string, SortedList<string, Tuple<int, XmlNode>>> columnsInfo)
        {
            string tableName = null;
            if (rootData == null) {
                rootData = data.SelectSingleNode("//root/data");
            }
            XmlNode beginMarker = null;
            if (sourceIndex == 1) {
                beginMarker = templatePart.SelectSingleNode(".//*[name()='ss:Data' and starts-with(.,'begin:')]");
            } else {
                beginMarker = templatePart.SelectSingleNode(".//*[name()='ss:Data' and contains(.,'" + parentName + ".begin:')]");
            }
            XmlNode endMarker;
            if (beginMarker == null)
            {
                return false;
            }
            else
            {
                tableName = beginMarker.InnerText.Split(' ')[0].Split(':')[1];
                endMarker = templatePart.SelectSingleNode(".//*[name()='ss:Data' and contains(.,'end:" + tableName + ";')]");
            }


            //string tableName;
            XmlNode table = data.SelectSingleNode("table[@as='" + tableName + "']");

            if (table == null & !data.Equals(rootData))
            {
                data = rootData;
                table = data.SelectSingleNode("table[@as='" + tableName + "']");

                string ss = table.Name;// чтобы вылетало
            }



            XmlNode buffer = templatePart.OwnerDocument.CreateElement("bufer");
            XmlNode workBuffer = templatePart.OwnerDocument.CreateElement("bufer");
            XmlNode last = null;
            XmlNode parent = null;
            if (sourceIndex == 1 & multipage)
            {
                XmlNode sheet = templatePart;
                parent = sheet.ParentNode;
                buffer.AppendChild(sheet);

            }
            else
            {
                XmlNode beginRow = beginMarker.ParentNode.ParentNode;
                XmlNode endRow = endMarker.ParentNode.ParentNode;
                XmlNode row = beginRow;
                parent = endRow.ParentNode;

                last = endRow.NextSibling;

                while (!row.Equals(last))
                {
                    XmlNode row1 = row.NextSibling;
                    buffer.AppendChild(row);
                    row = row1;
                    if (row == null)
                    {
                        break;
                    }
                }
            }

            if (beginMarker.ParentNode.ParentNode != null)
            {
                beginMarker.ParentNode.ParentNode.RemoveChild(beginMarker.ParentNode);
            }
            if (endMarker.ParentNode.ParentNode != null)
            {
                endMarker.ParentNode.ParentNode.RemoveChild(endMarker.ParentNode);
            }
            // beginMarker.InnerXml = "";
            // endMarker.InnerXml = "";


            if (table == null)
            {
                return false;
            }
            //SortedList<string, int> columnsIndexes;
            //SortedList<string, XmlNode> columnsInfo;
            SortedList<string, Tuple<int, XmlNode>> columns;
            if (columnsInfo.TryGetValue(tableName, out columns)) {
                //columnsIndexes = columnsIndexesList[tableName];
                //columnsInfo = columnsInfoList[tableName];
            } else {
                //columnsIndexes = new SortedList<string, int>();
                //columnsInfo = new SortedList<string, XmlNode>();
                columns = new SortedList<string, Tuple<int, XmlNode>>();
                int i = 0;
                foreach (XmlNode dataColumn in data.SelectNodes("//scheme//table[@as='" + tableName + "']/columns/column")) {
                    string column_name = dataColumn.Attributes["name"].Value;
                    columns.Add(column_name, new Tuple<int, XmlNode>(i, dataColumn));
                    //columnsIndexes.Add(column_name, i);
                    //columnsInfo.Add(column_name, dataColumn);
                    i++;
                }
                columnsInfo.Add(tableName, columns);
                //columnsIndexesList.Add(tableName, columnsIndexes);
                //columnsInfoList.Add(tableName, columnsInfo);
            }
            foreach (XmlNode dataRow in table.SelectNodes("data/tr"))
            {
                workBuffer.InnerXml = "";
                copyChilds(buffer, workBuffer);
                applyValues(workBuffer, tableName, sourceIndex, dataRow, columns); //columnsIndexes, columnsInfo);
                XmlNode nextData = dataRow.SelectSingleNode("childs");
                if (nextData == null)
                {
                    nextData = rootData;
                }

                while (applySource(workBuffer, sourceIndex + 1, nextData, tableName, rootData, multipage, columnsInfo))
                {
                };
                while (workBuffer.ChildNodes.Count > 0)
                {
                    if (last != null)
                    {
                        parent.InsertBefore(workBuffer.ChildNodes[0], last);
                    }
                    else
                    {
                        parent.AppendChild(workBuffer.ChildNodes[0]);
                    }
                }
            }
            // workBuffer.InnerXml = buffer.InnerXml;
            //copyChilds(buffer, workBuffer);
            //copyChilds(buffer, workBuffer);

            return true;


        }
        internal static void applyValues(XmlNode templatePart, string tableName, int sourceIndex, XmlNode row, SortedList<string, Tuple<int, XmlNode>> columns)//SortedList<string, int> columnsIndexes, SortedList<string, XmlNode> columnsInfo)
        {
            XmlNodeList values = templatePart.SelectNodes(".//*[name()='ss:Data' and contains(.,'value:" + tableName + ".')] | .//*[name()='ss:Cell' and contains(@ss:Formula,'value:" + tableName + ".')] ", excelNamespaseManager);
            XmlNode cells = row.SelectSingleNode("cells");
            if (templatePart.FirstChild.Attributes["ss:Name"] != null) {
                //<NamedRange ss:Name="Print_Titles" ss:RefersTo="='прил 5'!R6:R8"/>
                string oldSheetName = templatePart.FirstChild.Attributes["ss:Name"].Value;
                string newSheetName = cells.ChildNodes[columns["sid"].Item1].InnerText.Split('|')[1];//в качестве имени листа берется часть id строки, не крсиво - желательно переделать
                templatePart.FirstChild.Attributes["ss:Name"].Value = newSheetName;
                foreach (XmlNode namedRange in templatePart.SelectNodes(".//*[name()='ss:NamedRange']")) {
                    namedRange.Attributes["ss:RefersTo"].Value = namedRange.Attributes["ss:RefersTo"].Value.Replace(oldSheetName, newSheetName);
                }
            }

            foreach (XmlNode val in values)
            {
                string sData;
                if (val.Name == "ss:Data")
                {
                    sData = val.InnerText;
                }
                else
                {
                    sData = val.Attributes["ss:Formula"].Value;
                    sData = sData.Replace("+", " + ");
                    sData = sData.Replace("-", " - ");
                    sData = sData.Replace("/", " / ");
                    sData = sData.Replace("*", " * ");
                    sData = sData.Replace("(", " ( ");
                    sData = sData.Replace(")", " ) ");
                    sData = sData.Replace(";", " ; ");
                }
                int valBeg = sData.IndexOf("value:" + tableName + ".");
                string type = "String";

                while (valBeg > -1)
                {
                    int valEnd = sData.IndexOf(" ", valBeg);
                    if (valEnd == -1)
                    {
                        valEnd = sData.Length;
                    }
                    string sVal = sData.Substring(valBeg, valEnd - valBeg);
                    string columnName = sVal.Split(' ')[0].Split(':')[1].Split('.')[1];
                    Tuple<int, XmlNode> col_info = columns[columnName];
                    string value = cells.ChildNodes[col_info.Item1].InnerText;
                    if (valBeg == 0 & valEnd == sData.Length) {
                        switch (col_info.Item2.Attributes["type"].Value) {
                            case "number":
                                type = "Number";
                                value = value.Replace(",", ".");
                                break;
                            case "bool":
                                type = "Number";
                                value = value.Replace(",", ".");
                                break;
                            case "date":
                                type = "DateTime";
                                if (value != "")
                                {
                                    DateTime dat = Convert.ToDateTime(value);
                                    DateTime minExcelDate = new DateTime(1901, 1, 1);
                                    if (dat < minExcelDate) {
                                        dat = minExcelDate;
                                    }
                                    value = dat.ToString("yyyy-MM-dd");//+"T00:00:00.000";
                                }
                                break;
                        }
                    }
                    if (val.Name != "ss:Data") {
                        if (value == "") {
                            value = "R1C1000";  // !!! предполагается что это пустая ячейка
                        }
                    }
                    sData = sData.Replace(sVal, value);
                    valBeg = sData.IndexOf("value:" + tableName + ".");
                }
                if (val.Name == "ss:Data") {
                    val.Attributes["ss:Type"].Value = type;
                    if (!sData.Equals("")) {
                        val.InnerText = sData;
                    } else {
                        val.ParentNode.RemoveChild(val);
                    }
                } else {
                    val.Attributes["ss:Formula"].Value = sData;
                }
            }
        }
        public static void copyChilds(XmlNode src, XmlNode tag)
        {
            XmlNode buffer = tag.OwnerDocument.CreateElement("bufer");
            buffer.InnerXml = src.InnerXml;
            while (buffer.ChildNodes.Count > 0)
            {
                tag.AppendChild(buffer.ChildNodes[0]);
            }
        }
        /*public static int getLastColumn(XmlNode workbook)
        {
            XmlNode beginMarker = workbook.SelectSingleNode(".//*[name()='ss:Data' and contains(.,'begin[1]')]");

            return getMarkerColumn(beginMarker);

        }



        public static int getMarkerColumn(XmlNode marker)
        {
            XmlNode cell = marker.ParentNode;
            XmlNode row = cell.ParentNode;
            int i = 0;
            //"ss:Index"
            foreach (XmlNode cell1 in row.ChildNodes)
            {
                XmlAttribute indexAtt = cell1.Attributes["ss:Index"];
                if (indexAtt == null)
                {
                    i++;
                }
                else
                {
                    i = Convert.ToInt32(indexAtt.Value);
                }
                if (cell1.Equals(cell))
                {
                    return i;
                }

            }
            return 0;
        }*/

        public static string GetFreeName(string path, string name, string ext)
        {

            string s = "";
            int i = 1;

            if (!string.IsNullOrEmpty(ext))
            {
                ext = "." + ext;
            }

        // корректировка xx.06.2021 YShmyreva
        // длина имени выходного файла не должна превышать ограничения Windows[260]/MsOffice[218]
            int max_lenFileName = 200;
            int name_maxLen = max_lenFileName - path.Length - 1;
            if (name.Length > name_maxLen)
            {
                name = name.Substring(0, name_maxLen);
            };

            string newName = name + ext;
            while (File.Exists(Path.Combine(path, newName)) || Directory.Exists(Path.Combine(path, newName)))
            {
                s = "(" + i + ")";
                newName = name + s + ext;
                i++;
            }
            return Path.Combine(path, newName);

        }

        // ищет подходящее имя для файла с новым расширением
        public static string GetFreeNameChangeExt(string filePath, string newExt)
        {
            string mask = @"(.*\\)([^\(]*)(\([0-9]*\))?(\..*)?";

            Match match = Regex.Match(filePath, mask);
            filePath = GetFreeName(match.Groups[1].Value, match.Groups[2].Value, newExt);

            return filePath;
        }

        //// сравнение двух excel-файлов поячеечно специально обученым interop-ом
        //public static bool CompareExcelFiles(string file1, string file2, string output_file = null)
        //{
        //    var exApp = new Application { DisplayAlerts = false };
        //    try
        //    {
        //        Workbook doc1 = exApp.Workbooks.Open(file1);
        //        Workbook doc2 = exApp.Workbooks.Open(file2);
        //        Workbook doc3 = exApp.Workbooks.Add();

        //        doc1.Worksheets[1].Copy(After: doc3.Worksheets[1]);
        //        doc2.Worksheets[1].Copy(After: doc3.Worksheets[2]);

        //        doc3.Worksheets[1].Name = "Сравнение";
        //        doc3.Worksheets[2].Name = "Новый";
        //        doc3.Worksheets[3].Name = "Старый";
        //        doc3.Worksheets[1].Select();

        //        var range1 = doc1.Worksheets[1].UsedRange;
        //        int cols1 = range1.Columns.Count;
        //        int rows1 = range1.Rows.Count;

        //        var range2 = doc2.Worksheets[1].UsedRange;
        //        int cols2 = range2.Columns.Count;
        //        int rows2 = range2.Rows.Count;

        //        var range = doc3.Worksheets[1].Range[
        //            doc3.Worksheets[1].Cells[1, 1],
        //            doc3.Worksheets[1].Cells[Math.Max(rows1, rows2), Math.Max(cols1, cols2)]];

        //        // нахимичил, тк ячейки могут визуально быть пустыми, но сравнение при этом не проходит
        //        //range.FormulaR1C1 = "=IF(IF(ISBLANK(Новый!RC),\"\",TRIM(IF(ISTEXT(Новый!RC),Новый!RC,TEXT(Новый!RC,\"@\"))))=IF(ISBLANK(Старый!RC),\"\",TRIM(IF(ISTEXT(Старый!RC),Старый!RC,TEXT(Старый!RC,\"@\")))),1,0)";
        //        //Бельченко, добавил просто сравнение на равенство
        //        range.FormulaR1C1 = "=IF(OR(IF(ISBLANK(Новый!RC),\"\",TRIM(IF(ISTEXT(Новый!RC),Новый!RC,TEXT(Новый!RC,\"@\"))))=IF(ISBLANK(Старый!RC),\"\",TRIM(IF(ISTEXT(Старый!RC),Старый!RC,TEXT(Старый!RC,\"@\")))),Новый!RC=Старый!RC),1,0)";
        //        bool result = ((object[,])range.Value).Cast<object>().All(v => v.ToString() == "1");

        //        string condition = "=Сравнение!RC=0";
        //        var cond1 = (FormatCondition)doc3.Worksheets[2].UsedRange.FormatConditions.Add(XlFormatConditionType.xlExpression, Formula1: condition);
        //        cond1.Interior.Color = ColorTranslator.ToOle(Color.Red);
        //        var cond2 = (FormatCondition)doc3.Worksheets[3].UsedRange.FormatConditions.Add(XlFormatConditionType.xlExpression, Formula1: condition);
        //        cond2.Interior.Color = ColorTranslator.ToOle(Color.Red);

        //        string output_path = output_file ?? file1.Replace(".xlsx", "_res.xlsx");

        //        doc3.SaveAs(output_path,
        //                XlFileFormat.xlOpenXMLWorkbook,
        //                ReadOnlyRecommended: false,
        //                AccessMode: XlSaveAsAccessMode.xlNoChange,
        //                ConflictResolution: XlSaveConflictResolution.xlLocalSessionChanges);

        //        doc1.Close();
        //        doc2.Close();
        //        doc3.Close();

        //        return result;
        //    }
        //    finally
        //    {
        //        // вызов quit не убивает процесс excel.exe
        //        Cmn.CloseExcel(ref exApp);
        //    }
        //}
        //public static void MergeExcelFiles(string[] srcFilePaths, string outputFilePath)
        //{
        //    var exApp = new Application { DisplayAlerts = false };
        //    try
        //    {
        //        Workbook mainDoc = exApp.Workbooks.Open(srcFilePaths.First());

        //        foreach (string srcFilePath in srcFilePaths.Skip(1))
        //        {
        //            Workbook copyDoc = exApp.Workbooks.Open(srcFilePath);
        //            foreach(Worksheet sheet in copyDoc.Worksheets)
        //            {
        //                sheet.Copy(After: mainDoc.Worksheets[mainDoc.Worksheets.Count]);
        //            }
        //            copyDoc.Close(false);
        //        }

        //        // делаем активным первый лист
        //        (mainDoc.Worksheets[1] as Worksheet).Activate();

        //        mainDoc.SaveAs(outputFilePath,
        //            XlFileFormat.xlOpenXMLWorkbook,
        //            ReadOnlyRecommended: false,
        //            AccessMode: XlSaveAsAccessMode.xlNoChange,
        //            ConflictResolution: XlSaveConflictResolution.xlLocalSessionChanges);

        //        mainDoc.Close();
        //    }
        //    finally
        //    {
        //        // вызов quit не убивает процесс excel.exe
        //        Cmn.CloseExcel(ref exApp);
        //    }
        //}

        //public static Range[] SearchAllText(Range range, string text)
        //{
        //    var list = new List<Range>();

        //    Range currentFind = range.Find(text, Type.Missing, XlFindLookIn.xlValues, XlLookAt.xlPart, XlSearchOrder.xlByRows,
        //        XlSearchDirection.xlNext, false, false, false);
        //    Range firstFind = null;

        //    while (currentFind != null)
        //    {
        //        list.Add(currentFind);

        //        // Keep track of the first range you find. 
        //        if (firstFind == null)
        //        {
        //            firstFind = currentFind;
        //        }

        //        // If you didn't move to a new range, you are done.
        //        else if (currentFind.Address[XlReferenceStyle.xlA1] == firstFind.Address[XlReferenceStyle.xlA1])
        //        {
        //            break;
        //        }

        //        currentFind = range.FindNext(currentFind);
        //    }

        //    return list.ToArray();
        //}
    
    
    }

    /// <summary>
    /// Настройки печати
    /// </summary>
    public class PrintOptions
    {
        /// <summary>
        /// Возвращает или задает настройку выбора строк для печати
        /// </summary>
        public PrintRowsMode PrintRowsMode { get; set; }

        /// <summary>
        /// Настройки по умолчанию
        /// </summary>
        public static PrintOptions Default
        {
            get
            {
                return new PrintOptions()
                {
                    PrintRowsMode = PrintRowsMode.AllRows
                };
            }
        }
    }

    /// <summary>
    /// Строки для печати: видимые или выделенные
    /// </summary>
    public enum PrintRowsMode
    {
        /// <summary>
        /// Печатаем все видимые строки
        /// </summary>
        AllRows,
        /// <summary>
        /// Печатаем только выделеные строки
        /// </summary>
        SelectedRows
    }
}
