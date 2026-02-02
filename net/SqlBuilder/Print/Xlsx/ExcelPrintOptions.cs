using System.Data;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx
{
    /// <summary>
    /// ��������� ������ � Excel
    /// </summary>
    public class ExcelPrintOptions
    {
        #region ����
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
        #region ��������
        /// <summary>
        /// ����������� ������ ������� ��������� ������� �������, ������ �� ����������� �������. ��� �������
        /// </summary>
        public bool OnlyColumns { get { return this.only_columns; } set { this.only_columns = value; } }
        /// <summary>
        /// ���������� ��� ���� ����������� ������ ������� Excel 
        /// </summary>
        public string PivotColumn_WidthSource { get { return this.pivot_column_width_source; } /*set { this.pivot_column_width_source = value; }*/ }
        /// <summary>
        /// ������� �������, ��� ������� ��� ������
        /// </summary>
        public bool DeleteUnusedColumns { get { return this.delete_unused_columns; } set { this.delete_unused_columns = value; } }
        /// <summary>
        /// ������ ������������ ���������� ���� "table_name.variable_name". ������������ ���� DeleteUnusedColumns = true
        /// </summary>
        public string[] UsedVariables { get { return this.used_variables; } set { this.used_variables = value; } }
        /// <summary>
        /// ������ ������ ��� ��������������� �������� �� ������
        /// </summary>
        public bool UseDataReader { get { return this.use_data_reader; } set { this.use_data_reader = value; } }
        /// <summary>
        /// �������������� ����������� ������� �� �������, ��������� �� xlsx
        /// </summary>
        public bool NeedConvert { get { return this.need_convert; } set { this.need_convert = value; } }
        /// <summary>
        /// ������������ ���� ����� ������
        /// </summary>
        public bool NeedPostProcess { get { return this.need_post_process; } set { this.need_post_process = value; } }
        /// <summary>
        /// ������ ��������� �����
        /// </summary>
        public FileFormat OutputFormat { get { return this.output_format; } set { this.output_format = value; } }
        /// <summary>
        /// ���� �� �������� ����� ����������� ��������������, �� ������ ��� � ���������� ���������
        /// </summary>
        public string FormatSource { get { return this.format_source; } set { this.format_source = value; } }
        /// <summary>
        /// ������������ ���������� FlexCel ��� ��������� ������ Interop
        /// </summary>
        public bool UseFlexCel { get { return this.use_flex_cel; } set { this.use_flex_cel = value; } }
        /// <summary>
        /// ����� ������� ������� ����� ����� �������. ����� ����� ��������, ���� ���� ������ ������ � Excel
        /// </summary>
        public bool CopyTemplate { get { return this.copy_template; } set { this.copy_template = value; } }
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
        public ExcelPrintOptions(XElement xtemplate)
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
        /// �������� �� ���������
        /// </summary>
        public static ExcelPrintOptions Default {
            get {
                return new ExcelPrintOptions();
            }
        }
        public enum FileFormat
        {
            Xlsx,
            Xlsb,
            Pdf
        }
    }
}