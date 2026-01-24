using System.Data;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx
{
    /// <summary>
    /// Настройки вывода в Excel
    /// </summary>
    public class ExcelPrintOptions
    {
        #region Поля
        private bool only_columns;
        private bool delete_unused_columns;
        private bool use_data_reader;
        private bool need_convert;
        private bool need_post_process;
        private bool use_flex_cel;
        private bool copy_template;
        private string pivot_column_width_source;
        private string[] used_variables;
        private FileFormat output_format;
        private string format_source;
        #endregion
        #region свойства
        /// <summary>
        /// Выполняется только функция обработки колонок шаблона, шаблон не заполняется данными. Для отладки
        /// </summary>
        internal bool OnlyColumns { get { return this.only_columns; } set { this.only_columns = value; } }
        /// <summary>
        /// Определяет имя поля содержащего ширину колонки Excel 
        /// </summary>
        internal string PivotColumn_WidthSource { get { return this.pivot_column_width_source; } /*set { this.pivot_column_width_source = value; }*/ }
        /// <summary>
        /// Удалять колонки, для которых нет данных
        /// </summary>
        internal bool DeleteUnusedColumns { get { return this.delete_unused_columns; } set { this.delete_unused_columns = value; } }
        /// <summary>
        /// Список используемых переменных вида "table_name.variable_name". Используется если DeleteUnusedColumns = true
        /// </summary>
        internal string[] UsedVariables { get { return this.used_variables; } set { this.used_variables = value; } }
        /// <summary>
        /// Печать данных без предварительной загрузки на клиент
        /// </summary>
        internal bool UseDataReader { get { return this.use_data_reader; } set { this.use_data_reader = value; } }
        /// <summary>
        /// Автоматическая конвертация шаблона из формата, отличного от xlsx
        /// </summary>
        public bool NeedConvert { get { return this.need_convert; } set { this.need_convert = value; } }
        /// <summary>
        /// Обрабатывать файл после печати
        /// </summary>
        internal bool NeedPostProcess { get { return this.need_post_process; } set { this.need_post_process = value; } }
        /// <summary>
        /// Формат итогового файла
        /// </summary>
        internal FileFormat OutputFormat { get { return this.output_format; } set { this.output_format = value; } }
        /// <summary>
        /// Файл из которого будет скопировано форматирование, по листам для с одинаковым названием
        /// </summary>
        internal string FormatSource { get { return this.format_source; } set { this.format_source = value; } }
        /// <summary>
        /// Использовать библиотеку FlexCel для обработки вместо Interop
        /// </summary>
        public bool UseFlexCel { get { return this.use_flex_cel; } set { this.use_flex_cel = value; } }
        /// <summary>
        /// Перед печатью создает копию файла шаблона. Нужно чтобы печатать, даже если шаблон открыт в Excel
        /// </summary>
        internal bool CopyTemplate { get { return this.copy_template; } set { this.copy_template = value; } }
        #endregion
        public ExcelPrintOptions()
        {
            this.pivot_column_width_source = "column_width";
            this.delete_unused_columns = false;
            this.use_data_reader = false;
            this.need_convert = false;
            this.need_post_process = true;
            this.output_format = FileFormat.Xlsx;
            this.use_flex_cel = false;
            this.copy_template = false;
        }
        internal ExcelPrintOptions(XElement xtemplate)
        {
            this.pivot_column_width_source = "column_width";
            this.delete_unused_columns = xtemplate.AttrOrDefault(TextConst.AName.DelCols, false);
            this.use_data_reader = false;
            this.need_convert = false;
            this.need_post_process = true;
            this.output_format = FileFormat.Xlsx;
            this.use_flex_cel = xtemplate.AttrOrDefault(TextConst.AName.UseFlexCel, false);
            this.copy_template = false;
        }
        /// <summary>
        /// Значения по умолчанию
        /// </summary>
        public static ExcelPrintOptions Default {
            get {
                return new ExcelPrintOptions();
            }
        }
        internal enum FileFormat
        {
            Xlsx,
            Xlsb,
            Pdf
        }
    }
}