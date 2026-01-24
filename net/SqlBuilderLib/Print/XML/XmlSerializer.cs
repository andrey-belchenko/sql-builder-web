using System;
//using System.Collections.Generic;
using System.Xml;
using System.Xml.Linq;
using System.Data;
//using System.Text;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.Print.XML
{
    internal class XmlSerializer : IDisposable
    {
        private XmlWriter writer;
        private DataSet dataset;
        private bool print_big_data;
        private DataRow row;
        internal XmlSerializer(string file_name, DataSet dataset, bool print_big_data)
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
        internal XmlWriter Writer { get { return this.writer; } }
        internal DataSet DataSet { get { return this.dataset; } }
        internal DataRow Row { get { return this.row; } }
        internal bool PrintBigData { get { return this.print_big_data; } }
        internal void SetRow(DataRow row)
        {
            this.row = row;
        }
        internal void Close()
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
