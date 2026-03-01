using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics; // Debug, Stopwatch
using System.Linq;
using System.Xml;
using System.Xml.Linq;
//using DevExpress.Utils.CodedUISupport;
using sql.builder.DataApi;
using sql.builder.ExcelApi;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.Print.XML
{
    /// <seealso cref="sql.builder.Print.Xlsx.ExcelPrintSheet"/>
    public class ExcelPrintSheet
    {
        #region static
        public static ExcelPrintSheet Create(XElement worksheet, ExcelPrintDocument parent, ExcelPrintSheet prev_sheet)
        {
            ExcelPrintSheet sheet;
            string name = worksheet.AttrOrDefault(VExcelNS.SpreadSheet.Name, string.Empty);
            int pos_1 = name.IndexOf('{');
            if (pos_1 >= 0)
            {
                pos_1 = pos_1 + 1;
                int pos_2 = name.IndexOf('}', pos_1);
                if (pos_2 < 0)
                {
                    pos_2 = name.Length;
                }
                sheet = new ExcelPrintMultiplicatedSheet(worksheet, parent, name.Substring(pos_1, pos_2 - pos_1));
            }
            else
            {
                sheet = new ExcelPrintSheet(worksheet, parent);
            }
            if (prev_sheet != null)
            {
                prev_sheet.next_sheet = sheet;
            }
            return sheet;
        }
        #endregion
        #region поля
        protected readonly ExcelPrintDocument document;
        protected ExcelPrintSheet next_sheet;
        /// <summary>
        /// Элемент &lt;ss:Worksheet /&gt;
        /// </summary>
        protected XElement element;
        protected bool printed;
        /// <summary>
        /// Строки отчёта
        /// </summary>
        protected IList<IExcelPrintElement> childs;
        /// <summary>
        /// Элемент &lt;x:PageBreaks /&gt;
        /// </summary>
        protected XElement page_breaks_element;
        /// <summary>
        /// Номера строк шаблона, после которых нужно сделать разрыв страницы
        /// </summary>
        protected IList<int> row_page_breaks;
        /// <summary>
        /// Счётчик строк шаблона
        /// </summary>
        protected int template_row_index;
        /// <summary>
        /// Элемент &lt;ss:Row /&gt;, служащий маркером для вставки строк отчёта 
        /// </summary>
        protected XElement row_marker;
        /// <summary>
        /// Счётчик строк генерируемого листа
        /// </summary>
        protected int row_count;
        //public static SortedList<int, string> PrintColumn = new SortedList<int, string>();
        #endregion
        public ExcelPrintSheet(XElement sheet, ExcelPrintDocument document)
        {
            Contract.Assert(sheet.Name == VExcelNS.SpreadSheet.Worksheet);
            this.document = document;
            this.element = sheet;
            //List<XElement> headMarkers = sheet.Descendants(VExcelNS.SpreadSheet.Data).Where(e => e.Value.Contains("headmarker")).ToList();
            // headMarkers.ForEach(e1 => e1.Remove());
            sheet.Descendants().Attributes(VExcelNS.SpreadSheet.ExpandedRowCount).Remove();
            sheet.Descendants().Attributes(VExcelNS.SpreadSheet.ExpandedColumnCount).Remove();
            /*string name = sheet.AttrOrDefault(VExcelNS.SpreadSheet.Name, string.Empty);
            int pos_1 = name.IndexOf('{');
            if (pos_1 >= 0) {
                pos_1 = pos_1 + 1;
                int pos_2 = name.IndexOf('}', pos_1);
                if (pos_2 < 0) {
                    pos_2 = name.Length;
                }
                this.multiplicated = true;
                this.name_variable = name.Substring(pos_1, pos_2 - pos_1);
            } else {
                this.multiplicated = false;
                this.name_variable = null;
            }*/
            //
            this.page_breaks_element = sheet.Element(VExcelNS.Excel.PageBreaks);
            if (this.page_breaks_element != null)
            {
                XElement row_breaks = this.page_breaks_element.Element(VExcelNS.Excel.RowBreaks);
                if (row_breaks != null)
                {
                    this.row_page_breaks = new List<int>(0);
                    foreach (XElement row_break in row_breaks.Elements(VExcelNS.Excel.RowBreak).Elements(VExcelNS.Excel.Row))
                    {
                        this.row_page_breaks.Add(Convert.ToInt32(row_break.Value));
                    }
                    row_breaks.Remove();
                }
                this.page_breaks_element.Remove();
            }
            // 
            XElement table = sheet.Element(VExcelNS.SpreadSheet.Table);
            IList<XElement> rows = table.Elements(VExcelNS.SpreadSheet.Row).ToList();
            int row_index = 0;
            int index;
            for (index = 0; index < rows.Count; index++)
            {
                XElement row = rows[index];
                row.RemoveAttribute(VExcelNS.SpreadSheet.Span);
                row_index++;
                XAttribute attr = row.Attribute(VExcelNS.SpreadSheet.Index);
                if (attr != null)
                {
                    int ind = Convert.ToInt32(attr.Value);
                    while (row_index < ind)
                    {
                        row.AddBeforeSelf(new XElement(VExcelNS.SpreadSheet.Row));
                        row_index++;
                    }
                    attr.Remove();
                }
            }
            rows = table.Elements(VExcelNS.SpreadSheet.Row).ToList();
            this.childs = this.makeChildsList(string.Empty, rows, null);
            Contract.Assume(rows.Count > 0);
            this.row_marker = new XElement(VExcelNS.SpreadSheet.Row);
            rows[0].AddBeforeSelf(this.row_marker); // Как маркер для вставки строк
            for (index = 0; index < rows.Count; index++)
            {
                rows[index].Remove();
            }
            // foreach (XElement cell in sheet.Descendants().Where(e1 => !e1.HasElements).Where(e => e.Value.Contains("end:")).ToList()) {
            //     cell.Parent.Remove();
            // }
            // foreach (XElement cell in sheet.Descendants().Where(e1 => !e1.HasElements).Where(e => e.Value.Contains("begin:")).ToList()) {
            //     cell.Parent.Remove();
            // }
            foreach (XElement cell in rows.Descendants().ToList())
            {
                if ((!cell.HasElements) && (cell.Value.Contains("begin:") || cell.Value.Contains("end:")))
                {
                    cell.Parent.Remove();
                }
            }
        }
        #region свойства
        /// <summary>
        /// Ссылка на следующий лист
        /// </summary>
        public ExcelPrintSheet NextSheet { get { return this.next_sheet; } }
        /// <summary>
        /// Число напечатаных строк в отчёте
        /// </summary>
        public int RowCount { get { return this.row_count; } }
        //public bool Printed { get { return this.printed; } }
        //public ExcelPrintDocument Document { get { return this.document; } }
        //public bool Multiplicated { get { return this.multiplicated; } }
        #endregion
        public void MarkUnprinted()
        {
            this.printed = false;
            this.row_count = 0;
        }
        public void NextRow()
        {
            this.row_count++;
        }
        private void ResetPageBreaks()
        {
            if (this.page_breaks_element != null)
            {
                XElement row_breaks = this.page_breaks_element.Element(VExcelNS.Excel.RowBreaks);
                if (row_breaks != null)
                {
                    row_breaks.Remove();
                }
            }
        }
        /// <summary>
        /// Добавляет разрыв страницы после текущей строки генерируемого файла (<see cref="row_count"/>),
        /// если это предусмотрено для строки шаблона <paramref name="template_row"/>
        /// </summary>
        /// <param name="template_row">Номер строки шаблона</param>
        public void AddBreakIfNeeded(int template_row)
        {
            if (this.row_page_breaks == null)
            {
                return;
            }
            if (this.row_page_breaks.Contains(template_row))
            {
                Contract.Assume(this.page_breaks_element != null);
                XElement rb = this.page_breaks_element.Element(VExcelNS.Excel.RowBreaks);
                if (rb == null)
                {
                    rb = new XElement(VExcelNS.Excel.RowBreaks);
                    this.page_breaks_element.Add(rb);
                }
                rb.Add(new XElement(VExcelNS.Excel.RowBreak, new XElement(VExcelNS.Excel.Row, new XText(this.row_count.ToString()))));
            }
        }
        #region XSLX format
        public virtual ExcelPrintSheet Print(DataSet dataset, bool print_big_data)
        {
            this.PrintData(null, dataset, null, print_big_data);
            return this.next_sheet;
        }
        #endregion
        #region XML format
        public virtual ExcelPrintSheet Print(XmlSerializer serializer)
        {
            serializer.SetRow(null);
            this.Write(serializer);
            return this.next_sheet;
        }
        /// <summary>
        /// Сериализация в XML аттрибутов элемента <paramref name="element"/>.
        /// </summary>
        /// <param name="serializer">объект, содержащий контекст сериализации</param>
        /// <param name="element">сериализуемый элемент</param>
        protected virtual void WriteAttributes(XmlSerializer serializer, XElement element)
        {
            XmlWriter writer = serializer.Writer;
            XAttribute attr = element.FirstAttribute;
            while (attr != null)
            {
                writer.WriteAttributeString(attr);
                attr = attr.NextAttribute;
            }
        }
        /// <summary>
        /// Сериализация в XML элемента <paramref name="element"/>.
        /// При сериализации &lt;ss:Worksheet /&gt; (поле <see cref="element"/>)
        /// в конец дописывается &lt;x:PageBreaks /&gt; (поле <see cref="page_breaks_element"/>), если есть.
        /// </summary>
        /// <param name="serializer">объект, содержащий контекст сериализации</param>
        /// <param name="element">сериализуемый элемент</param>
        protected void Write(XmlSerializer serializer, XElement element)
        {
            XmlWriter writer = serializer.Writer;
            writer.WriteStartElement(element.Name);
            // Записываем аттрибуты
            this.WriteAttributes(serializer, element);
            // Записываем узлы
            XNode node = element.FirstNode;
            while (node != null)
            {
                Write(serializer, node);
                node = node.NextNode;
            }
            if (object.ReferenceEquals(element, this.element) && this.page_breaks_element != null)
            {
                Write(serializer, this.page_breaks_element);
            }
            writer.WriteEndElement();
        }
        /// <summary>
        /// Сериализация в XML узла <paramref name="node"/>.
        /// Узел <see cref="row_marker"/> замещается гнерируемыми строками отчёта.
        /// </summary>
        /// <param name="serializer">объект, содержащий контекст сериализации</param>
        /// <param name="node">сериализуемый узел</param>
        protected void Write(XmlSerializer serializer, XNode node)
        {
            if (object.ReferenceEquals(node, this.row_marker))
            {
                this.PrintData(serializer.Writer, serializer.DataSet, serializer.Row, serializer.PrintBigData);
            }
            else if (node.NodeType == XmlNodeType.Element)
            {
                this.Write(serializer, (XElement)node);
            }
            else
            {
                node.WriteTo(serializer.Writer);
            }
        }
        /// <summary>
        /// Сериализация в XML всего листа
        /// </summary>
        /// <param name="serializer">объект, содержащий контекст сериализации</param>
        protected void Write(XmlSerializer serializer)
        {
#if DEBUG
            Stopwatch sw = new Stopwatch();
            Contract.Assume(GC.MaxGeneration == 2);
            int gen_0_before = GC.CollectionCount(0);
            int gen_1_before = GC.CollectionCount(1);
            int gen_2_before = GC.CollectionCount(2);
            long mem_before = GC.GetTotalMemory(false);
            sw.Start();
#endif
            this.Write(serializer, this.element);
#if DEBUG
            sw.Stop();
            long mem_after = GC.GetTotalMemory(false);
            int gen_0_after = GC.CollectionCount(0);
            int gen_1_after = GC.CollectionCount(1);
            int gen_2_after = GC.CollectionCount(2);
            Debug.WriteLine("sql.builder.Print.XML.ExcelPrintSheet.Write(XmlSerializer): " + sw.ElapsedMilliseconds.ToString() + " мс, " + this.row_count.ToString() + " строк");
            Debug.WriteLine("  Занятая память: " + (mem_after - mem_before).ToString());
            Debug.WriteLine("  Сборок мусора (0/1/2): " + (gen_0_after - gen_0_before).ToString() + " / " + (gen_1_after - gen_1_before).ToString() + " / " + (gen_2_after - gen_2_before).ToString());
#endif
        }
        #endregion
        /*private void PrintSingle(XmlWriter writer, DataSet dataSet, DataRow row, bool print_big_data = false)
        {
            if (writer != null) {
                string header;
                if (row != null && !string.IsNullOrEmpty(this.name_variable)) {
                    header = this.header.Replace("{" + this.name_variable + "}", row[this.name_variable].ToString());
                } else {
                    header = this.header;
                }
                writer.WriteRaw(header);
                header = null;
            }
            //
            this.document.NextPrintedSheet();
            this.printed = true;
            this.ResetPageBreaks();
            // this.row_count накручивается при печати строки, rowsCount оперделяется расчетным методом добавил чтобы поместить в headder, по идее лолжны совпадать
            this.row_count = 0;
            int index;
            for (index = 0; index < this.childs.Count; index++) {
                this.childs[index].ClearData();
            }
            for (index = 0; index < this.childs.Count; index++) {
                IExcelPrintElement node = this.childs[index];
                bool other = false;
                if (row != null) {
                    ExcelPrintGroup pe = node as ExcelPrintGroup;
                    if (pe != null && pe.GetTableReference().MainTableName != row.Table.TableName) {
                        node.Print(writer, dataSet, false, null, print_big_data);
                        other = true;
                    }
                }
                if (!other) {
                    node.Print(writer, dataSet, false, row, print_big_data);
                }
            }
            //
            if (writer != null) {
                string footer;
                if (this.PageBreaksElement != null) {
                    footer = this.footer.Replace("{PAGE_BREAKS}", this.PageBreaksElement.ToString());
                } else {
                    footer = this.footer;
                }
                writer.WriteRaw(footer);
            }
        }*/
        protected void PrintData(XmlWriter writer, DataSet dataset, DataRow row, bool print_big_data)
        {
            this.document.NextPrintedSheet();
            this.printed = true;
            this.ResetPageBreaks();
            // this.row_count накручивается при печати строки, rowsCount оперделяется расчетным методом добавил чтобы поместить в headder, по идее лолжны совпадать
            this.row_count = 0;
            int index;
            for (index = 0; index < this.childs.Count; index++)
            {
                this.childs[index].ClearData();
            }
            for (index = 0; index < this.childs.Count; index++)
            {
                this.childs[index].Print(writer, dataset, row, print_big_data);
            }
        }
        /// <seealso cref="sql.builder.Print.Xlsx.ExcelPrintSheet.makeChildsList"/>
        public IList<IExcelPrintElement> makeChildsList(string table_prefix, IList<XElement> rows, ExcelPrintGroup parent)
        {
            IList<IExcelPrintElement> childs = new List<IExcelPrintElement>();
            List<XElement> childRows = null;
            XElement begMarker = null;
            string table_name = null;
            bool dontRemove = false;
            for (int index = 0; index < rows.Count; index++)
            {
                XElement row = rows[index];
                if (begMarker == null)
                {
                    begMarker = row.Descendants().FirstOrDefault(e => (!e.HasElements) && e.Value.StartsWith(table_prefix + "begin:"));
                    if (begMarker != null)
                    {
                        string text = begMarker.Value;
                        int pos_1 = table_prefix.Length + 6;
                        Contract.Assume(text.Substring(0, pos_1) == table_prefix + "begin:");
                        int pos_2 = text.IndexOfWhiteSpace(pos_1);
                        if (pos_2 < 0)
                        {
                            pos_2 = text.Length;
                        }
                        int pos_3 = pos_2 - 3;
                        if (text[pos_3] == '(' && text[pos_3 + 1] == '+' && text[pos_3 + 2] == ')')
                        {
                            Contract.Assume(text.Substring(pos_3, 3) == "(+)");
                            dontRemove = true;
                            pos_2 = pos_3;
                        }
                        table_name = text.Substring(pos_1, pos_2 - pos_1);
                    }
                }
                if (begMarker == null)
                {
                    this.template_row_index++;
                    childs.Add(new ExcelPrintRow(this, this.template_row_index, row, parent));
                }
                else
                {
                    if (childRows == null)
                    {
                        childRows = new List<XElement>(1);
                    }
                    childRows.Add(row);
                    if (table_name != null)
                    {
                        XElement endMarker = row.Descendants().FirstOrDefault(e => (!e.HasElements) && e.Value.Contains("end:" + table_name + ";"));
                        if (endMarker != null)
                        {
                            childs.Add(new ExcelPrintGroup(this, childRows, dontRemove, parent, table_name));
                            begMarker = null;
                            dontRemove = false;
                            childRows = null;
                        }
                    }
                }
            }
            return childs;
        }
    }
    public class ExcelPrintMultiplicatedSheet : ExcelPrintSheet
    {
        #region поля
        private string name_variable;
        #endregion
        public ExcelPrintMultiplicatedSheet(XElement sheet, ExcelPrintDocument document, string name_variable)
            : base(sheet, document)
        {
            Contract.Assert(!string.IsNullOrEmpty(name_variable));
            this.name_variable = name_variable;
        }
        private void GetMultipleSet(IList<Tuple<ExcelPrintMultiplicatedSheet, string, DataRow>> list, DataSet dataSet, bool print_big_data)
        {
            ExcelPrintGroup el = this.childs.OfType<ExcelPrintGroup>().First();
            List<DataRow> rows = el.GetRowsByParent(dataSet, print_big_data);
            for (int index = 0; index < rows.Count; index++)
            {
                DataRow row = rows[index];
                list.Add(new Tuple<ExcelPrintMultiplicatedSheet, string, DataRow>(this, row[this.name_variable].ToString(), row));
            }
        }
        public override ExcelPrintSheet Print(DataSet dataset, bool print_big_data)
        {
            if (this.printed)
            {
                return this.next_sheet;
            }
            else
            {
                // Печать всех множащихся листов стоящих подряд с сортировкой по имени
                var list = new List<Tuple<ExcelPrintMultiplicatedSheet, string, DataRow>>();
                ExcelPrintSheet sheet;
                ExcelPrintMultiplicatedSheet mp_sheet = this;
                do
                {
                    mp_sheet.GetMultipleSet(list, dataset, print_big_data);
                    sheet = mp_sheet.next_sheet;
                    mp_sheet = sheet as ExcelPrintMultiplicatedSheet;
                } while (mp_sheet != null);
                if (list.Count > 1 && !print_big_data)
                {
                    list = list.OrderBy(r => r.Item2).ToList();
                }
                for (int index = 0; index < list.Count; index++)
                {
                    Tuple<ExcelPrintMultiplicatedSheet, string, DataRow> rec = list[index];
                    rec.Item1.PrintData(null, dataset, rec.Item3, print_big_data);
                }
                return sheet;
            }
        }
        public override ExcelPrintSheet Print(XmlSerializer serializer)
        {
            if (this.printed)
            {
                return this.next_sheet;
            }
            else
            {
                XmlWriter writer = serializer.Writer;
                DataSet dataset = serializer.DataSet;
                bool print_big_data = serializer.PrintBigData;
                var list = new List<Tuple<ExcelPrintMultiplicatedSheet, string, DataRow>>();
                ExcelPrintSheet sheet;
                ExcelPrintMultiplicatedSheet mp_sheet = this;
                do
                {
                    mp_sheet.GetMultipleSet(list, dataset, print_big_data);
                    sheet = mp_sheet.next_sheet;
                    mp_sheet = sheet as ExcelPrintMultiplicatedSheet;
                } while (mp_sheet != null);
                //
                if (!print_big_data)
                {
                    list = list.OrderBy(r => r.Item2).ToList();
                }
                for (int index = 0; index < list.Count; index++)
                {
                    Tuple<ExcelPrintMultiplicatedSheet, string, DataRow> rec = list[index];
                    serializer.SetRow(rec.Item3);
                    rec.Item1.Write(serializer);
                }
                serializer.SetRow(null);
                return sheet;
            }
        }
        protected override void WriteAttributes(XmlSerializer serializer, XElement element)
        {
            DataRow row = serializer.Row;
            XmlWriter writer = serializer.Writer;
            XAttribute attr = element.FirstAttribute;
            while (attr != null)
            {
                writer.WriteStartAttribute(attr.Name);
                string value = attr.Value;
                if (value.IndexOf('{') >= 0)
                {
                    Contract.Assume(row != null);
                    string variable_value = row[this.name_variable].ToString();
                    value = value.Replace("{" + this.name_variable + "}", variable_value);
                }
                writer.WriteString(value);
                writer.WriteEndAttribute();
                attr = attr.NextAttribute;
            }
        }
    }
}
