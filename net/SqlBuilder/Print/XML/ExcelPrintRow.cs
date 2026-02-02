using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using Contract = System.Diagnostics.Contracts.Contract;
using sql.builder.ExcelApi;

namespace sql.builder.Print.XML
{
    /// <summary>
    /// Cтрока шаблона Excel в формате xml
    /// </summary>
    /// <seealso cref="sql.builder.Print.Xlsx.ExcelPrintRow"/>
    public class ExcelPrintRow : IExcelPrintElement
    {
        #region поля
        private readonly IList<IExcelPrintValue> values;
        /// <summary>
        /// Номер строки шаблона
        /// </summary>
        private readonly int row_index;
        private readonly XElement row;
        private readonly IExcelPrintGroup parent;
        private readonly ExcelPrintSheet sheet;
        #endregion
        public ExcelPrintRow(ExcelPrintSheet sheet, int row_index, XElement row, IExcelPrintGroup parent)
        {
            Contract.Assert(row.Name == VExcelNS.SpreadSheet.Row);
            this.sheet = sheet;
            this.parent = parent;
            this.row = row;
            this.values = new List<IExcelPrintValue>();
            this.row_index = row_index;
            List<XElement> list = row.Descendants(VExcelNS.SpreadSheet.Data).ToList();
            XElement xval;
            StringBuilder sb = null;
            for (int index = 0; index < list.Count; index++) {
                xval = list[index];
                string str = xval.Value;
                if (str.Contains("[:")) {
                    this.values.Add(ExcelPrintValue.Create(xval, this));
                } else if (str.IndexOf('\n') >= 0) {
                    if (sb == null) {
                        sb = new StringBuilder(str.Length);
                    }
                    sb.Append(str);
                    sb.Replace("\n\r", "\r");
                    sb.Replace('\n', '\r');
                    //sb.Replace("\r", ExcelPrintValue.brSpecStr);
                    xval.Value = sb.ToString();
                    sb.Clear();
                }
            }
        }
        public IExcelPrintGroup Parent { get { return this.parent; } }
        public void Print(XmlWriter writer, DataSet dataset, DataRow row, bool print_big_data)
        {
            this.sheet.NextRow();
            for (int index = 0; index < this.values.Count; index++) {
                this.values[index].Print();
            }
            if (writer == null) {
                ExcelEnvironment.Writer.Write(this.ToXlsxRow(this.row));
            } else {
                this.row.WriteTo(writer);
                this.sheet.AddBreakIfNeeded(this.row_index);
            }
        }
        public void ClearData()
        {
            for (int index = 0; index < this.values.Count; index++) {
                this.values[index].Clear();
            }
        }
        private string ToXlsxRow(XElement row)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@"<row>");
            foreach (XElement xval in row.Descendants(VExcelNS.SpreadSheet.Cell)) {
                XElement xdata = xval.Element(VExcelNS.SpreadSheet.Data);
                if (xdata != null) {
                    switch (xdata.Attribute(VExcelNS.SpreadSheet.Type).Value) {
                        case "String":
                            sb.Append(string.Format(@"<c s=""{0}"" t=""s""><v>{1}</v></c>",
                                (int)ExcelEnvironment.BigDataFormats.String,
                                ExcelEnvironment.InternStringAndGetIndex(xdata.Value/*.Replace(ExcelPrintValue.brSpecStr, "\r")*/)));
                            break;
                        case "Number":
                            sb.Append(string.Format(@"<c s=""{0}""><v>{1}</v></c>",
                                (int)ExcelEnvironment.BigDataFormats.Default,
                                xdata.Value));
                            break;
                        case "DateTime":
                            sb.Append(string.Format(@"<c s=""{0}""><v>{1}</v></c>",
                                (int)ExcelEnvironment.BigDataFormats.Date,
                                (int)DateTime.Parse(xdata.Value).ToOADate()));
                            break;
                        default:
                            sb.Append(string.Format(@"<c s=""{0}"" t=""s""><v>{1}</v></c>",
                                (int)ExcelEnvironment.BigDataFormats.Default,
                                ExcelEnvironment.InternStringAndGetIndex(xdata.Value)));
                            break;
                    }
                } else {
                    sb.Append("<c/>");
                }
            }
            sb.Append(@"</row>");
            return sb.ToString();
        }
    }
}
