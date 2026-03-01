using System;
using System.Data;
//using System.Collections.Generic;
using System.Xml;
//using System.Text;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.Print.XML
{
    public class XmlSerializer : IDisposable
    {
        private XmlWriter writer;
        private DataSet dataset;
        private bool print_big_data;
        private DataRow row;
        public XmlSerializer(string file_name, DataSet dataset, bool print_big_data)
            : base()
        {
            Contract.Assert(!string.IsNullOrEmpty(file_name));
            Contract.Assert(dataset != null);
            XmlWriterSettings settings = new XmlWriterSettings();
            //settings.Encoding = Encoding.UTF8;
            //settings.OmitXmlDeclaration = false;
            //settings.Indent = false;
            //settings.NewLineOnAttributes = false;
            //settings.NewLineChars = "\n";
            settings.NewLineHandling = NewLineHandling.None;
#if !FRAMEWORK_40
            settings.WriteEndDocumentOnClose = false;
#endif
            this.writer = XmlWriter.Create(file_name, settings);
            this.dataset = dataset;
            this.print_big_data = print_big_data;
        }
        public XmlWriter Writer { get { return this.writer; } }
        public DataSet DataSet { get { return this.dataset; } }
        public DataRow Row { get { return this.row; } }
        public bool PrintBigData { get { return this.print_big_data; } }
        public void SetRow(DataRow row)
        {
            this.row = row;
        }
        public void Close()
        {
            this.writer.Close();
        }
        public void Dispose()
        {
#if FRAMEWORK_40
            ((IDisposable)this.writer).Dispose();
#else
            this.writer.Dispose();
#endif
        }
    }
}
