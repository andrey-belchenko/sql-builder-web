using System;
using System.Xml;      // XmlWriter
using System.Xml.Linq; // XElement, XAttribute
using Contract = System.Diagnostics.Contracts.Contract;
//using System.Collections.Generic;
//using System.Text;
using System.IO;

namespace sql.builder
{
    /// <summary>
    /// Набор методов расширения для <see cref="XmlWriter"/>
    /// </summary>
    public static class XmlWriterExtensions
    {
        /// <summary>
        /// Записывает имя элемента <paramref name="name"/> во <paramref name="writer"/>.
        /// 
        /// </summary>
        /// <param name="writer">XML-writer</param>
        /// <param name="name">Имя записываемого элемента</param>
        public static void WriteStartElement(this XmlWriter writer, XName name)
        {
            Contract.Assert(writer != null);
            Contract.Assert(name != null);
            Contract.Requires(writer.WriteState == WriteState.Element || writer.WriteState == WriteState.Prolog);
            Contract.Ensures(writer.WriteState == WriteState.Element);
            string ns_name = name.NamespaceName;
            string ns_prefix = writer.LookupPrefix(ns_name);
            writer.WriteStartElement(ns_prefix, name.LocalName, ns_name);
        }
        /// <summary>
        /// Записывает имя атрибута <paramref name="name"/> во <paramref name="writer"/>
        /// </summary>
        /// <param name="writer">XML-writer</param>
        /// <param name="name">Имя записываемого атрибута</param>
        public static void WriteStartAttribute(this XmlWriter writer, XName name)
        {
            Contract.Assert(writer != null);
            Contract.Assert(name != null);
            Contract.Requires(writer.WriteState == WriteState.Element);
            Contract.Ensures(writer.WriteState == WriteState.Attribute);
            string ns_name = name.NamespaceName;
            string local_name = name.LocalName;
            if (string.IsNullOrEmpty(ns_name) && local_name == "xmlns") {
                writer.WriteStartAttribute(string.Empty, local_name, XNamespace.Xmlns.NamespaceName);
            } else {
                string prefix = writer.LookupPrefix(ns_name);
                writer.WriteStartAttribute(prefix, local_name, ns_name);
            }
        }
        /// <summary>
        /// Записывает атрибут <paramref name="attr"/> в <paramref name="writer"/>
        /// </summary>
        /// <param name="writer">XML-writer</param>
        /// <param name="attr">Записываемый атрибут</param>
        public static void WriteAttributeString(this XmlWriter writer, XAttribute attr)
        {
            Contract.Assert(attr != null);
            Contract.Assert(writer != null);
            Contract.Requires(writer.WriteState == WriteState.Element);
            Contract.Ensures(writer.WriteState == WriteState.Element);
            WriteStartAttribute(writer, attr.Name);
            writer.WriteString(attr.Value);
            writer.WriteEndAttribute();
        }

        public static long SerializeToFile(this object obj, string file_name)
        {
            long file_size;
            using (System.IO.FileStream stream = File.Open(file_name, FileMode.CreateNew, FileAccess.Write, FileShare.Write)) {
#pragma warning disable SYSLIB0011 // BinaryFormatter is obsolete
                System.Runtime.Serialization.IFormatter formatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();
                formatter.Serialize(stream, obj);
#pragma warning restore SYSLIB0011
                stream.Flush();
                file_size = stream.Length;
                stream.Close();
            }
            return file_size;
        }
    }
}
