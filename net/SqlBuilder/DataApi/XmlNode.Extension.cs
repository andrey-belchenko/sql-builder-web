using System;
using System.Xml;
using System.Collections.Generic;
//using System.Text;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.DataApi
{
    /// <summary>
    /// Набор методов расширения для <see cref="XmlNode"/>
    /// </summary>
    public static class XmlNodeExtensions
    {
        /// <summary>
        /// Возвращает значение аттрибута с именем <paramref name="attr_name"/>.
        /// Если атрибута с таким именем нет, возвращается <paramref name="default_value"/>.
        /// </summary>
        /// <param name="node">XML-узел</param>
        /// <param name="attr_name">имя аттрибута</param>
        /// <param name="default_value">значение по умолчанию</param>
        /// <returns>Значение аттрибута или <paramref name="default_value"/>, если он отсутствует</returns>
        public static string AttrOrDefault(this XmlNode node, string attr_name, string default_value)
        {
            Contract.Assert(node != null);
            XmlAttribute attr = node.Attributes[attr_name];
            if (attr != null) {
                return attr.Value;
            } else {
                return default_value;
            }
        }
        /// <summary>
        /// Возвращает значение аттрибута с именем <paramref name="attr_name"/>.
        /// Если атрибута с таким именем нет, возвращается <paramref name="default_value"/>.
        /// </summary>
        /// <param name="node">XML-узел</param>
        /// <param name="attr_name">имя аттрибута</param>
        /// <param name="default_value">значение по умолчанию</param>
        /// <returns>Значение аттрибута или <paramref name="default_value"/>, если он отсутствует</returns>
        public static bool AttrOrDefault(this XmlNode node, string attr_name, bool default_value)
        {
            Contract.Assert(node != null);
            XmlAttribute attr = node.Attributes[attr_name];
            if (attr != null) {
                string value = attr.Value;
                if (value == TextConst.AVBool.True) {
                    return true;
                } else if (value == TextConst.AVBool.False) {
                    return false;
                }
            } 
            return default_value;
        }
        /// <summary>
        /// Создаёт в <paramref name="node"/> новый аттрибут с именем <paramref name="attr_name"/> и значением <paramref name="value"/>.
        /// Если аттрибут с таким именем существует, то меняет его значение на новое.
        /// </summary>
        /// <param name="node">XML-узел</param>
        /// <param name="attr_name">имя аттрибута</param>
        /// <param name="value">значение аттрибута</param>
        public static void SetAttrValue(this XmlNode node, string attr_name, string value)
        {
            Contract.Assert(node != null);
            Contract.Assert(value != null);
            XmlAttribute attr = node.Attributes[attr_name];
            if (attr == null) {
                attr = node.OwnerDocument.CreateAttribute(attr_name);
                node.Attributes.Append(attr);
            }
            attr.Value = value;
        }
        /// <summary>
        /// Удаляет аттрибут <paramref name="attr_name"/> из узла <paramref name="node"/>
        /// </summary>
        /// <param name="node">XML-узел</param>
        /// <param name="attr_name">имя убираемого аттрибута</param>
        public static void RemoveAttribute(this XmlNode node, string attr_name)
        {
            Contract.Assert(node != null);
            XmlAttribute attr = node.Attributes[attr_name];
            if (attr != null) {
                node.Attributes.Remove(attr);
            }
        }
    }
}
