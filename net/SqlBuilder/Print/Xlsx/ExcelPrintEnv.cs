using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml.Linq;
//using DevExpress.DashboardCommon.Native;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx
{
    public class ExcelPrintEnv : IDisposable
    {
        #region поля
        private string _print_directory;
        private List<WorksheetPrint> _printers;
        private List<ExcelBaseFile> _files;
        private ExcelSharedStrings shared_strings;
        private ExcelWorkbook workbook;
        private ExcelWorkbookRels workbook_rels;
        private ExcelContentTypes content_types;
        private ExcelStyles styles;
        private List<ExcelWorksheet> worksheets;
        #endregion
        public ExcelSharedStrings SharedStrings { get { return this.shared_strings; } }
        public ExcelWorkbook Workbook { get { return this.workbook; } }
        public ExcelWorkbookRels WorkbookRels { get { return this.workbook_rels; } }
        //public ExcelCore Core { get; private set; }
        //public ExcelApp App { get; private set; }
        //public ExcelContentTypes ContentTypes { get { return this.content_types; } }
        //public ExcelStyles Styles { get { return this.styles; } }
        public List<ExcelWorksheet> Worksheets { get { return this.worksheets; } }

        private string _numericMask = string.Format(@"^[+-]?[0-9]+(\{0}[0-9]+)?$", CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator);
        public void AddFileToList(ExcelBaseFile file)
        {
            _files.Add(file);
        }
        public ExcelPrintEnv(string template_path, ExcelPrintOptions options, DataSet data)
        {
            this._print_directory = ExcelUtils.GetPrintDirectory();
            this._printers = new List<WorksheetPrint>();
            this._files = new List<ExcelBaseFile>();
            // чтобы не падало если файл уже открыт в Excel
            if (options.CopyTemplate) {
                string template_path_new = Printing.GetFreeName(Printing.outputFolder, Path.GetFileNameWithoutExtension(template_path) + "-template", ".xlsx");
                File.Copy(template_path, template_path_new);
                template_path = template_path_new;
            }
            // загрузка базового xlsx и распаковка во временную дирректорию
            using (ZipArchive xlsx = ZipFile.OpenRead(template_path)) {
                foreach (ZipArchiveEntry entry in xlsx.Entries) {
                    string destinationPath = Path.GetFullPath(Path.Combine(_print_directory, entry.FullName));
                    if (destinationPath.StartsWith(_print_directory, StringComparison.Ordinal)) {
                        string directory = Path.GetDirectoryName(destinationPath);
                        if (!Directory.Exists(directory)) {
                            Directory.CreateDirectory(directory);
                        }
                        if (!string.IsNullOrEmpty(entry.Name)) {
                            entry.ExtractToFile(destinationPath, overwrite: true);
                        }
                    }
                }
            }
            this.shared_strings = new ExcelSharedStrings(Path.Combine(_print_directory, "xl", "sharedStrings.xml"));
            this._files.Add(this.shared_strings);
            this.workbook = new ExcelWorkbook(Path.Combine(_print_directory, "xl", "workbook.xml"));
            this._files.Add(this.workbook);
            this.styles = new ExcelStyles(Path.Combine(_print_directory, "xl", "styles.xml"));
            this._files.Add(this.styles);
            this.workbook_rels = new ExcelWorkbookRels(Path.Combine(_print_directory, "xl", "_rels", "workbook.xml.rels"));
            this._files.Add(this.workbook_rels);
            ExcelCore core = new ExcelCore(Path.Combine(_print_directory, "docProps", "core.xml"));
            this._files.Add(core);
            //ExcelApp app = new ExcelApp(Path.Combine(_print_directory, "docProps", "app.xml"));
            ExcelBaseFile app = new ExcelBaseFile(Path.Combine(_print_directory, "docProps", "app.xml"));
            this._files.Add(app);
            this.content_types = new ExcelContentTypes(Path.Combine(_print_directory, "[Content_Types].xml"));
            this._files.Add(this.content_types);
            //
            string[] file_names = Directory.GetFiles(Path.Combine(_print_directory, "xl", "worksheets"));
            this.worksheets = new List<ExcelWorksheet>(file_names.Length);
            for (int index = 0; index < file_names.Length; index++) {
                ExcelWorksheet worksheet = new ExcelWorksheet(file_names[index], this);
                this.worksheets.Add(worksheet);
                // разворачиваем формулы
                worksheet.ExpandRefFormulas();
                worksheet.ProcessPivotColumns(data, options.PivotColumn_WidthSource);
                if (options.DeleteUnusedColumns) {
                    worksheet.DeleteUnusedColumns(options.UsedVariables);
                }
            }
            // необязательный файл - проще удалить, чем генерировать ручками
            string path = Path.Combine(_print_directory, "xl", "calcChain.xml");
            if (File.Exists(path)) {
                File.Delete(path);
            }
            this._files.AddRange(this.worksheets);
        }
        /*private DataTable getColumnsInfoFromTransposeInfo(string dimName, DataSet data)// размазывание группировкой в колонках
        {
            foreach (DataTable dt in data.Tables)
            {
                if (dt is VDataTable)
                {
                    var vdt = dt as VDataTable;
                    if (vdt.TransposeStructure != null)
                    {
                       
                        break;
                    }
                }
            }
            return null;

        }*/
        public WorksheetPrint BeginPrint(ExcelWorksheet worksheet, string name = null)
        {
            string rid = null;
            string filename = null;
            WorksheetPrint printerLast = this._printers.LastOrDefault(p => p.Worksheet.NativeSheetRID == worksheet.NativeSheetRID);
            if (printerLast != null) {
                // печать листа больше одного раза
                this.workbook_rels.CreateWorksheetRel(worksheet, out rid, out filename);
                // чтобы размноженные листы не были выделены все сразу
                foreach (XElement sv in worksheet.XmlChanged.Element(ns.Main.worksheet).Element(ns.Main.sheetViews).Elements(ns.Main.sheetView)) {
                    sv.RemoveAttribute(ns.None.tabSelected);
                }
                this.content_types.AddWorksheet(filename);
                this.workbook.AddWorksheet(rid, name, printerLast.RID);
            } else {
                // печать листа первый раз
                rid = worksheet.NativeSheetRID;
                filename = worksheet.NativeSheetFileName;
                if (name != null) {
                    this.workbook.ChangeNativeWorksheetName(rid, name);
                }
            }
            string filePath = Path.Combine(_print_directory, "xl", "worksheets", filename + ".xml");
            WorksheetPrint pi = new WorksheetPrint(rid, filePath, worksheet, this);
            this._printers.Add(pi);
            // переделал чтобы таких формул небыло
            //pi.InitSharedFormulas();
            XElement xsheet = new XElement(pi.Worksheet.XmlChanged.Root);
            //xsheet.Elements(ns.Main.autoFilter).Remove();
            // колонки печатаются в шапке
            // если нет описания колонок - родительский узел тоже не нужен
            XElement xcols = pi.Cols.GetXml();
            if (xcols.HasElements) {
                xsheet.Element(ns.Main.sheetData).AddBeforeSelf(xcols);
            }

            // печатаем header
            Match match = Regex.Match(xsheet.ToString(), @"(.*?)<sheetData />.*", RegexOptions.Singleline);
            string header = match.Groups[1].Value;
            pi.PrintText(string.Format("{0}{2}{1}{2}<sheetData>", worksheet.XmlChanged.Declaration, header, Environment.NewLine));

            return pi;
        }
        public void DeleteWorksheet(ExcelWorksheet worksheet)
        {
            this.workbook_rels.DeleteWorksheet(worksheet.NativeSheetRID);
            this.content_types.DeleteWorksheet(worksheet.NativeSheetFileName);
            this.workbook.DeleteWorksheet(worksheet.NativeSheetRID);
            File.Delete(worksheet.FilePath);
            this.worksheets.Remove(worksheet);
        }
        public void PrintRow(WorksheetPrint pi, ExcelRow row, Dictionary<ExcelCell, object> values, Dictionary<ExcelCell, string> hyperlinkTargets)
        {
            ExcelRow cur = row;
            // если строки печатаются несколько раз подряд, учитывать отступ только для первой
            int delta = (pi.LastPrintedRow == row) ? 1 : cur.PrevIDDelta;
            // доделать учет удаленных строк
            //while (cur.PrevRow != null && pi.DeletedRows.Contains(cur.PrevRow))
            //{
            //    cur = cur.PrevRow;
            //    delta += cur.PrevIDDelta;
            //    // вроде должно сработать
            //    pi.UnmarkRowAsDeleted(cur);
            //}
            pi.LastPrintedRow = row;
            pi.LastPrintedRowID += delta;
            //pi.LastPrintedRowID = int.Parse(row.ID);
            pi.CopyRowMerge(row, pi.LastPrintedRowID);
            // не реализовано!
            //pi.CopyRowBreak(row, pi.LastPrintedRowID);
            XElement rowXml = new XElement(ns.Main.row);
            Cmn.copyAttributes(row.Xml, rowXml);
            rowXml.Attribute("r").SetValue(pi.LastPrintedRowID);
            for (int index = 0; index < row.Cells.Count; index++) {
                ExcelCell cell = row.Cells[index];
                if (cell.HasSharedString) {
                    if (cell.Text.Contains("begin:") || cell.Text.Contains("end:")) continue;
                }
                XElement cellXml = new XElement(cell.Xml);
                string cellName = cell.CellInfo.ColumnName + pi.LastPrintedRowID.ToString();
                cellXml.Attribute("r").SetValue(cellName);
                object value = null;
                values.TryGetValue(cell, out value);
                var formula = value as ExcelPrintFormula;
                // вместо значения подставляем формулу
                if (formula != null) {
                    cellXml.Elements(ns.Main.v).Remove();
                    XElement xf = cellXml.Element(ns.Main.f);
                    if (xf == null) {
                        xf = new XElement(ns.Main.f);
                        cellXml.Add(xf);
                    }
                    xf.SetValue(formula.Formula);
                } else {
                    ExcelStyle style = this.styles.GetStyle(cell.StyleID);
                    bool isNumeric = (style != null) && style.IsNumeric;
                    // если decimal - проверять не нужно
                    if (isNumeric && !(value is decimal)) {
                        // сделал через маску, тк decimal.TryParse возвращает true для строк типа "14-" или "02 03"
                        string text = (value != null) ? value.ToString() : cell.Text;
                        // проверяем что значение валидное число либо отсутствует
                        if (!string.IsNullOrEmpty(text) && !Regex.IsMatch(text, _numericMask)) {
                            isNumeric = false;
                        }
                    }
                    if (value != null) {
                        this.ProcessCellValue(cellXml, cell, value, isNumeric);
                    }
                    if (cell.HasFormula) {
                        ProcessCellFormula(cellXml, cell, pi.LastPrintedRowID - int.Parse(row.ID));
                    }
                }
                string target;
                if (hyperlinkTargets.TryGetValue(cell, out target)) {
                    pi.AddHyperlink(cellName, target);
                }
                rowXml.Add(cellXml);
            }
            pi.PrintRow(row, rowXml);
        }
        public static void EndPrint(WorksheetPrint pi)
        {
            foreach (ExcelRow row in pi.NotPrintedRows) {
                pi.RemoveRowMerge(row);
                //pi.RemoveRowBreak(row);
            }
            var xsheet = new XElement(pi.Worksheet.XmlChanged.Root);
            var xpart = pi.GetHyperlinksXml();
            if (xpart != null) {
                // порядок важен
                XElement x = xsheet.Element(ns.Main.dataValidations) ?? xsheet.Elements(ns.Main.conditionalFormatting).LastOrDefault()
                        ?? xsheet.Element(ns.Main.autoFilter) ?? xsheet.Element(ns.Main.sheetData);
                x.AddAfterSelf(xpart);
            }
            xpart = pi.GetMergesXml();
            // excel крашится если есть этот узел, но мержей нет
            if (xpart.HasElements) {
                // порядок важен
                XElement x = xsheet.Element(ns.Main.autoFilter) ?? xsheet.Element(ns.Main.sheetData);
                x.AddAfterSelf(xpart);
            }
            //XElement xbreaks = pi.CompileBreaksXml();
            //if (xbreaks != null) {
            //    xsheet.Element(ns.Main.breaksCells).ReplaceWith(xbreaks);
            //}
            // сразу печатаем footer
            Match match = Regex.Match(xsheet.ToString(), @".*<sheetData />(.*)", RegexOptions.Singleline);
            string footer = match.Groups[1].Value;
            pi.PrintText(string.Format("</sheetData>{0}{1}", Environment.NewLine, footer));
            pi.Dispose();
        }
        private void ProcessCellValue(XElement cellXml, ExcelCell cell, object value, bool isNumeric)
        {
            string svalue = value.ToString();
            // пустые значения можно не хранить
            if (svalue == "") {
                cellXml.RemoveAttribute(ns.None.t);
                cellXml.Elements(ns.Main.v).Remove();
                return;
            }
            // внутри excel десятичный разделитель всегда точка, независимо от настроек ОС!
            if (isNumeric) {
                svalue = svalue.Replace(',', '.');
            }
            if (cell.HasSharedString)
            {
                if (isNumeric)
                {
                    cellXml.RemoveAttribute(ns.None.t);
                }
                else
                {
                    svalue = this.shared_strings.InternStringAndGetIndex(svalue).ToString();
                }
            }

            XElement xv = cellXml.Element(ns.Main.v);
            if (xv == null) {
                xv = new XElement(ns.Main.v);
                cellXml.Add(xv);
            }

            xv.SetValue(svalue);
        }
        private static void ProcessCellFormula(XElement cellXml, ExcelCell cell, int row_delta)
        {
            // удаляем сообщение об ошибке в формуле
            XAttribute at = cellXml.Attribute(ns.None.t);
            if (at != null && at.Value == "e") {
                at.Remove();
                XElement xv = cellXml.Element(ns.Main.v);
                if (xv != null) {
                    xv.Value = "";
                }
            }
            //string formula = (cell.HasSharedFormula) ? pi.GetSharedFormulaInfo(cell) : xf.Value;
            //string formula =  xf.Value;
            string formula = cell.Formula.GetText();
            // корректируем ссылки в формуле
            //int row_delta = last_printed_row_id - int.Parse(row.ID);
            // TODO: засунуть это в ExcelFormula, чтобы не парсить туда-сюда
            formula = ExcelUtils.CorrectFormulaReferences(formula, 0, row_delta);
            XElement xf = cellXml.Element(ns.Main.f);
            xf.RemoveAttributes();
            xf.SetValue(formula);
        }
        public void Save(string output_path)
        {
            for (int index = 0; index < this._files.Count; index++) {
                this._files[index].Save();
            }
            if (File.Exists(output_path)) {
                File.Delete(output_path);
            }
            ZipFile.CreateFromDirectory(_print_directory, output_path);
        }
        // для отладки - посмотреть что получилось после размазывания колонок и т.д.
        public void SaveTemplate(string output_path)
        {
            foreach (ExcelWorksheet excelWorksheet in this.worksheets) {
                using (WorksheetPrint pi = this.BeginPrint(excelWorksheet)) {
                    foreach (ExcelRow excelRow in excelWorksheet.Rows) {
                        pi.PrintRow(excelRow, excelRow.Xml);
                    }
                    EndPrint(pi);
                }
            }
            this.Save(output_path);
        }
        public void Dispose()
        {
            bool success = false;
            for (int i = 0; i < 3; i++) {
                try {
                    if (Directory.Exists(_print_directory)) {
                        Directory.Delete(_print_directory, true);
                    }
                    success = true;
                } catch (IOException) {
                    foreach (var p in _printers) {
                        p.Dispose();
                    }
                    // мы честно пытались
                }
                if (!success) Thread.Sleep(1000);
                else break;
            }
        }
    }
}