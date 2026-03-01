using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Xml;
using System.Xml.Linq;
using sql.builder.ExcelApi;
using Contract = System.Diagnostics.Contracts.Contract;
using ExcelPrintErrors = sql.builder.ExcelPrintDocument.ExcelPrintErrors;

namespace sql.builder.Print.XML
{
    /// <summary>
    /// Класс печати Excel по шаблону в формате *.xml
    /// </summary>
    public partial class ExcelPrintDocument
    {
        #region поля
        private XDocument template;
        private XElement worksheet_marker;
        /// <summary>
        /// Ссылка на первый лист
        /// </summary>
        private ExcelPrintSheet first_sheet;
        private int printed_sheets_count;
        #endregion
        #region Конструкторы
        /// <summary>
        /// Конструктор класса печати Excel
        /// </summary>
        /// <param name="template">Шаблон Excel в формате *.xml</param>
        public ExcelPrintDocument(XDocument template)
        {
            this.Prepare(template);
        }
        /// <summary>
        /// Конструктор класса печати Excel
        /// </summary>
        /// <param name="template_file_name">Имя файла шаблона Excel в формате *.xml</param>
        public ExcelPrintDocument(string template_file_name)
        {
            XDocument template;
            using (var stream = File.Open(template_file_name, FileMode.Open, FileAccess.Read))
            {
                template = XDocument.Load(stream);
                stream.Close();
            }
            this.Prepare(template);
        }
        #endregion
        public void Prepare(XDocument template)
        {
            this.template = template;
            Contract.Assume(template.Root.Name == VExcelNS.SpreadSheet.Workbook);
            IList<XElement> list = template.Root.Descendants(VExcelNS.SpreadSheet.Worksheet).ToList();
            Contract.Assume(list.Count > 0);
            this.worksheet_marker = new XElement(VExcelNS.SpreadSheet.Worksheet);
            list[0].AddBeforeSelf(this.worksheet_marker); // как маркер для вставки листов
            ExcelPrintSheet prev_sheet = null;
            for (int index = 0; index < list.Count; index++)
            {
                XElement worksheet = list[index];
                if (worksheet.Elements(VExcelNS.SpreadSheet.Table).Elements(VExcelNS.SpreadSheet.Row).Any())
                {
                    ExcelPrintSheet sheet = ExcelPrintSheet.Create(worksheet, this, prev_sheet);
                    if (this.first_sheet == null)
                    {
                        this.first_sheet = sheet;
                    }
                    prev_sheet = sheet;
                }
                worksheet.Remove();
            }
        }
        private void MarkUnprinted()
        {
            ExcelPrintSheet sheet = this.first_sheet;
            while (sheet != null)
            {
                sheet.MarkUnprinted();
                sheet = sheet.NextSheet;
            }
            this.printed_sheets_count = 0;
        }
        /// <summary>
        /// Печать DataSet в файл
        /// </summary>
        /// <param name="fileName">Полное имя файла</param>
        /// <param name="dataset">DataSet для печати</param>
        /// <param name="print_big_data">True - печатается больше количество данных (нужно было для отчета ВСЯ БАЗА)</param>
        /// <param name="convertToOpenXml">True - конвертировать файл в OpenXML</param>
        /// <returns>Значение из перечисления: None - данные напечатаны, NoData - нет данных для печати</returns>
        public ExcelPrintErrors Print(string fileName, DataSet dataset, bool print_big_data = false, bool convertToOpenXml = false)
        {
            if (!convertToOpenXml)
            {
                Contract.Assume(!string.IsNullOrEmpty(fileName));
                this.WriteInXMLFormat(fileName, dataset, print_big_data);
                if (this.printed_sheets_count > 0)
                {
                    return ExcelPrintErrors.None;
                }
                else
                {
                    return ExcelPrintErrors.NoData;
                }
            }
            else
            {
                Contract.Assume(string.IsNullOrEmpty(fileName));
            }
            this.MarkUnprinted();
            ExcelPrintSheet sheet = this.first_sheet;
            while (sheet != null)
            {
                sheet = sheet.Print(dataset, print_big_data);
            }
            if (this.printed_sheets_count > 0)
            {
                return ExcelPrintErrors.None;
            }
            else
            {
                return ExcelPrintErrors.NoData;
            }
        }
        #region XML format
        private void Write(sql.builder.Print.XML.XmlSerializer serializer, XElement element)
        {
            XmlWriter writer = serializer.Writer;
            writer.WriteStartElement(element.Name);
            // Записываем аттрибуты
            XAttribute attr = element.FirstAttribute;
            while (attr != null)
            {
                writer.WriteAttributeString(attr);
                attr = attr.NextAttribute;
            }
            // Записываем узлы
            this.WriteNodes(serializer, element);
            writer.WriteEndElement();
        }
        private void WriteNodes(sql.builder.Print.XML.XmlSerializer serializer, XContainer container)
        {
            XNode node = container.FirstNode;
            while (node != null)
            {
                this.Write(serializer, node);
                node = node.NextNode;
            }
        }
        private void Write(sql.builder.Print.XML.XmlSerializer serializer, XNode node)
        {
            if (object.ReferenceEquals(node, this.worksheet_marker))
            {
                ExcelPrintSheet sheet = this.first_sheet;
                while (sheet != null)
                {
                    sheet = sheet.Print(serializer);
                }
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
        private void WriteInXMLFormat(string file_name, DataSet dataset, bool print_big_data)
        {
            Contract.Assert(!string.IsNullOrEmpty(file_name));
            Contract.Assert(dataset != null);
            this.MarkUnprinted();
            using (XmlSerializer serializer = new XmlSerializer(file_name, dataset, print_big_data))
            {
                XmlWriter writer = serializer.Writer;
                writer.WriteStartDocument();
                this.WriteNodes(serializer, this.template);
                writer.WriteEndDocument();
                writer.Close();
            }
        }
        #endregion
        /// <summary>
        /// Увеличивает счётчик напечатаных листов на единицу
        /// </summary>
        public void NextPrintedSheet()
        {
            this.printed_sheets_count++;
        }
    }
}
