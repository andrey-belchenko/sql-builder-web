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
        public static string LastPrintedFilePath {
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
        public static void OnPrintingHandler(object document, object sheet, int printed_sheets, int printed_rows)
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
        public static string PostProcess(string file_name, string output_format, string file_name_new, string format_source)
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
                return file_name_new;
            } finally {
                //// вызов quit не убивает процесс excel.exe
                //try {
                //    Cmn.CloseExcel(ref exApp);
                //} catch { }
            }
        }
        public static string PostProcessFlexCel(string file_name, string output_format = "xlsx", string file_name_new = null)
        {
            throw new NotImplementedException();
        }
        public static string PostProcessBigData(string file_name, string template_path, bool xlsb)
        {

            throw new NotImplementedException();
        }
        public static string ChangeExcelFileFormat(string filePath, ExcelPrintOptions.FileFormat formatNew)
        {
            throw new NotImplementedException();
        }
        public static string GetProjectExcelTemplates(string templateType)
        {
            return Path.Combine(XmlReports.GetCurrentContentFolder(), @"printTemplate\" + templateType);
        }
        #if DEBUG
        #endif
    }
}
