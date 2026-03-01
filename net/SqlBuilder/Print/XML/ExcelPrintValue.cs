using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Text;
using System.Xml.Linq;
using sql.builder.DataApi;
using sql.builder.ExcelApi;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.Print.XML
{
    public static class ExcelPrintValue
    {
        public static DateTime MIN_EXCEL_DATE = new DateTime(1901, 1, 1);
        public static IExcelPrintValue Create(XElement element, ExcelPrintRow parent)
        {
            Contract.Assert(parent != null);
            Contract.Assert(element != null);
            Contract.Assert(element.Name == VExcelNS.SpreadSheet.Data);
            Contract.Assert(element.Parent.Name == VExcelNS.SpreadSheet.Cell);
            string text = element.Value;
            int len = text.Length;
            if (len > 3 && text[0] == '[' && text[1] == ':' && text.IndexOf(']', 2) == (len - 1))
            {
                int pos = text.LastIndexOf('.');
                if (pos >= 0)
                {
                    string table_name = string.Intern(text.Substring(2, pos - 2));
                    string column_name = text.Substring(pos + 1, len - pos - 2);
                    element.Value = string.Empty;
                    return new SingleExcelPrintValue(element, parent, table_name, column_name);
                }
            }
            return new ComplexExcelPrintValue(element, parent);
        }
        [ThreadStatic]
        private static StringBuilder buffer;
        public static StringBuilder Buffer
        {
            get
            {
                if (buffer == null)
                {
                    buffer = new StringBuilder(256);
                }
                return buffer;
            }
        }
        public static void RefineExcelText(StringBuilder sb)
        {
            Contract.Assert(sb != null);
            sb.Replace("\n\r", "\n");
            sb.Replace("\r\n", "\n");
            int index = 0;
            while (index < sb.Length)
            {
                char ch = sb[index];
                if (System.Xml.XmlConvert.IsXmlChar(ch))
                {
                    index++;
                }
                else
                {
                    sb.Remove(index, 1);
                }
            }
        }
    }
    public interface IExcelPrintValue
    {
        /// <summary>
        /// Замещает текст ячейки значениями из БД
        /// </summary>
        void Print();
        /// <summary>
        /// Очищает текст ячейки
        /// </summary>
        void Clear();
    }
    /// <summary>
    /// Ячейка отчёта, текст которой полностью заменяется на значение из БД
    /// </summary>
    public class SingleExcelPrintValue : IExcelPrintValue
    {
        private ExcelPrintRow parent;
        //private XElement element;
        private XElement cell;
        private XElement data;
        private string table_name;
        private string column_name;
        public SingleExcelPrintValue(XElement element, ExcelPrintRow parent, string table_name, string column_name)
        {
            Contract.Assert(parent != null);
            Contract.Assert(element != null);
            Contract.Assert(element.Parent != null);
            Contract.Assert(element.Name == VExcelNS.SpreadSheet.Data);
            Contract.Assert(element.Parent.Name == VExcelNS.SpreadSheet.Cell);
            Contract.Assert(table_name != null);
            Contract.Assert(column_name != null);
            this.parent = parent;
            this.data = element;
            this.cell = element.Parent;
            element.Remove();
            this.table_name = table_name;
            this.column_name = column_name;
        }
        /// <summary>
        /// Замещает текст ячейки значением из БД
        /// </summary>
        void IExcelPrintValue.Print()
        {
            TableReference tr = this.parent.Parent.GetTableReference(this.table_name);
            DataTable table = tr.Table;
            DataColumn column;
            if (this.column_name == TextConst.AVSpecColumn.RowId)
            {
                column = table.PrimaryKey[0];
            }
            else
            {
                column = table.Columns[this.column_name];
                if (column == null)
                {
                    throw new InvalidOperationException("В наборе данных отсутствует колонка " + this.table_name + "." + this.column_name);
                }
            }
            object value;
            if (tr.IsCurrentRowExists())
            {
                value = tr.GetCurrentRowValue(column);
            }
            else
            {
                value = null;
            }
            string excel_type;
            string text = null;
            bool is_null = Cmn.IsNullOrDBNull(value);
            Type data_type = column.DataType;
            if (data_type == typeof(decimal))
            {
                excel_type = "Number";
                //data.SetAttrValue(VExcelNS.SpreadSheet.Type, "Number");
                //if (is_null) {
                //    if (!tr.IsCurrentRowExists()) {
                //        text = "0"; // 09.01.2017 36703(10)
                //    } else {
                //        text = string.Empty;
                //    }
                //} else {
                if (!is_null)
                {
                    text = ((IFormattable)value).ToString(null, CultureInfo.InvariantCulture);
                }
            }
            else if (data_type == typeof(DateTime))
            {
                excel_type = "DateTime";
                if (!is_null)
                {
                    DateTime date = Convert.ToDateTime(value);
                    if (date < ExcelPrintValue.MIN_EXCEL_DATE)
                    {
                        date = ExcelPrintValue.MIN_EXCEL_DATE;
                    }
                    if (date.Hour == 0 && date.Minute == 0)
                    {
                        text = date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture);
                    }
                    else
                    {
                        text = date.ToString("yyyy-MM-dd'T'HH:mm", CultureInfo.InvariantCulture);
                    }
                }
            }
            else
            { // data_type == typeof(string)
                excel_type = "String";
                if (!is_null)
                {
                    StringBuilder buffer = ExcelPrintValue.Buffer;
                    Contract.Assume(buffer.Length == 0);
                    buffer.Append(value.ToString());
                    ExcelPrintValue.RefineExcelText(buffer);
                    if (buffer.Length == 0)
                    {
                        text = null;
                    }
                    else
                    {
                        text = buffer.ToString();
                        buffer.Clear();
                    }
                }
            }
            if (text != null)
            {
                this.data.SetAttrValue(VExcelNS.SpreadSheet.Type, excel_type);
                //this.data.Value = text;
                this.data.RemoveNodes();
                if (text.IndexOf('\n') < 0)
                {
                    this.data.Add(new XText(text));
                }
                else
                {
                    this.data.Add(new XCData(text));
                }
                if (this.data.Parent == null)
                {
                    this.cell.Add(data);
                }
            }
            else
            {
                this.cell.RemoveNodes();
            }
            //
            VDataTable vt = table as VDataTable;
            if (vt != null && tr.IsCurrentRowExists())
            {
                VDataColumn vc = vt.GetColumn(column.ColumnName);
                if (Printing.CreateRefs && !vc.IsEmptyEvent && (vt.HasRowEvents || vc.HasCellEvents))
                {
                    //XElement cell = this.element.Parent;
                    this.cell.SetAttrValue(VExcelNS.SpreadSheet.HRef, "http://" + table.TableName + "." + column.ColumnName + ".[" + tr.GetCurrentRowValue(table.PrimaryKey[0].ColumnName).ToString() + "]");
                    this.cell.SetAttrValue(VExcelNS.Excel.HRefScreenTip, "Открыть");
                }
            }
        }
        /// <summary>
        /// Очищает текст ячейки
        /// </summary>
        void IExcelPrintValue.Clear()
        {
            //this.element.Value = string.Empty;
            this.cell.RemoveNodes();
        }
    }
    public class ComplexExcelPrintValue : IExcelPrintValue
    {
        private ExcelPrintRow parent;
        private XElement element;
        private string text;
        public IDictionary<string, List<string>> table_columns;
        public ComplexExcelPrintValue(XElement element, ExcelPrintRow parent)
        {
            Contract.Assert(parent != null);
            Contract.Assert(element != null);
            Contract.Assert(element.Name == VExcelNS.SpreadSheet.Data);
            Contract.Assert(element.Parent.Name == VExcelNS.SpreadSheet.Cell);
            this.element = element;
            this.text = element.Value;
            element.Value = string.Empty;
            this.parent = parent;
            this.table_columns = new Dictionary<string, List<string>>(1);
            List<string> vars = Cmn.ExtractParamsFromString(this.text);
            for (int index = 0; index < vars.Count; index++)
            {
                string str = vars[index];
                int len = str.Length;
                int pos = str.LastIndexOf('.');
                if (pos >= 0)
                {
                    string table_name = string.Intern(str.Substring(0, pos));
                    string column_name = str.Substring(pos + 1);
                    List<string> cols;
                    if (!this.table_columns.TryGetValue(table_name, out cols))
                    {
                        cols = new List<string>(1);
                        cols.Add(column_name);
                        this.table_columns.Add(table_name, cols);
                    }
                    else if (!cols.Contains(column_name))
                    {
                        cols.Add(column_name);
                    }
                    // создаем список печатаемых колонок
                    // ExcelPrintSheet.PrintColumn[VExcelCommon.GetIndex(element.Parent)] = column_name.ToUpper();
                }
            }
        }
        /// <summary>
        /// Замещает текст ячейки значениями из БД
        /// </summary>
        void IExcelPrintValue.Print()
        {
            StringBuilder buffer = ExcelPrintValue.Buffer;
            Contract.Assume(buffer.Length == 0);
            buffer.Append(this.text);
            foreach (var pair in this.table_columns)
            {
                string table_name = pair.Key;
                TableReference tr = this.parent.Parent.GetTableReference(table_name);
                DataTable table = tr.Table;
                for (int col_index = 0; col_index < pair.Value.Count; col_index++)
                {
                    string column_name = pair.Value[col_index];
                    DataColumn column;
                    if (column_name == TextConst.AVSpecColumn.RowId)
                    {
                        column = table.PrimaryKey[0];
                    }
                    else
                    {
                        column = table.Columns[column_name];
                    }
                    string val;
                    if (tr.IsCurrentRowExists())
                    {
                        val = tr.GetCurrentRowValue(column).ToString();
                    }
                    else
                    {
                        val = string.Empty;
                    }
                    buffer.Replace("[:" + table_name + "." + column_name + "]", val);
                }
            }
            ExcelPrintValue.RefineExcelText(buffer);
            string value = buffer.ToString();
            buffer.Clear();
            this.element.RemoveNodes();
            if (value.IndexOf('\n') < 0)
            {
                this.element.Add(new XText(value));
            }
            else
            {
                this.element.Add(new XCData(value));
            }
        }
        /// <summary>
        /// Очищает текст ячейки
        /// </summary>
        void IExcelPrintValue.Clear()
        {
            this.element.Value = string.Empty;
        }
    }
}