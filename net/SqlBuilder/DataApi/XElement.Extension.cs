using System;
using System.Collections.Generic;
using System.Globalization; // DateTimeStyles
using System.Xml.Linq; // XElement, XAttribute
using Contract = System.Diagnostics.Contracts.Contract;
using Enumerable = System.Linq.Enumerable;

namespace sql.builder.DataApi
{
    /// <summary>
    /// Набор методов расширения для <see cref="XElement"/>
    /// </summary>
    public static class XElementExtensions
    {
        /// <summary>
        /// Формат дат по умолчанию
        /// </summary>
        private const string date_format = "dd.MM.yyyy HH:mm:ss";
        /// <summary>
        /// Формат дат ISO
        /// </summary>
        private const string iso_date_format = "O";
        /// <summary>
        /// Возвращает значение аттрибута с именем <paramref name="attr_name"/>.
        /// Если атрибута с таким именем нет, возвращается <paramref name="default_value"/>.
        /// </summary>
        /// <param name="el">элемент</param>
        /// <param name="attr_name">имя аттрибута</param>
        /// <param name="default_value">значение по умолчанию</param>
        /// <returns>Значение аттрибута или <paramref name="default_value"/>, если он отсутствует</returns>
        public static string AttrOrDefault(this XElement el, XName attr_name, string default_value)
        {
            Contract.Assert(el != null);
            XAttribute attr = el.Attribute(attr_name);
            if (attr != null)
            {
                return attr.Value;
            }
            else
            {
                return default_value;
            }
        }
        public static bool AttrOrDefault(this XElement el, XName attr_name, bool default_value)
        {
            Contract.Assert(el != null);
            XAttribute attr = el.Attribute(attr_name);
            if (attr != null)
            {
                return attr.Value == TextConst.AVBool.True;
            }
            else
            {
                return default_value;
            }
        }
        public static DateTime AttrOrDefault(this XElement el, XName attr_name, DateTime default_value)
        {
            Contract.Assert(el != null);
            XAttribute attr = el.Attribute(attr_name);
            if (attr == null)
            {
                return default_value;
            }
            else
            {
                string str_value = attr.Value;
                string format;
                if (str_value.Length == date_format.Length)
                {
                    format = date_format;
                }
                else
                {
                    format = iso_date_format;
                }
                DateTime value;
                if (DateTime.TryParseExact(str_value, format, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind, out value))
                {
                    return value;
                }
                else
                {
                    return default_value;
                }
            }
        }
        public static DateTime AttrOrDefault(this XElement el, XName attr_name, DateTime default_value, string format, IFormatProvider provider = null, DateTimeStyles style = DateTimeStyles.RoundtripKind)
        {
            Contract.Assert(el != null);
            XAttribute attr = el.Attribute(attr_name);
            DateTime value;
            if (attr == null)
            {
                return default_value;
            }
            else if (DateTime.TryParseExact(attr.Value, format, provider, style, out value))
            {
                return value;
            }
            else
            {
                return default_value;
            }
        }
        public static int AttrOrDefault(this XElement el, XName attr_name, int default_value)
        {
            Contract.Assert(el != null);
            XAttribute attr = el.Attribute(attr_name);
            if (attr != null)
            {
                return Convert.ToInt32(attr.Value);
            }
            else
            {
                return default_value;
            }
        }
        public static string AttrOrEmpty(this XElement el, XName attr_name)
        {
            return AttrOrDefault(el, attr_name, string.Empty);
        }
        /// <summary>
        /// Создаёт в <paramref name="el"/> новый аттрибут с именем <paramref name="attr_name"/> и значением <paramref name="value"/>.
        /// Если аттрибут с таким именем существует, то меняет его значение на новое.
        /// </summary>
        /// <param name="el">элемент</param>
        /// <param name="attr_name">имя аттрибута</param>
        /// <param name="value">значение аттрибута</param>
        public static void SetAttrValue(this XElement el, XName attr_name, string value)
        {
            Contract.Assert(el != null);
            Contract.Assert(value != null);
            //value = string.Intern(value);
            XAttribute attr = el.Attribute(attr_name);
            if (attr != null)
            {
                attr.Value = value;
            }
            else
            {
                el.Add(new XAttribute(attr_name, value));
            }
        }
        public static void SetAttrValue(this XElement el, XName attr_name, bool value)
        {
            Contract.Assert(el != null);
            SetAttrValue(el, attr_name, value ? TextConst.AVBool.True : TextConst.AVBool.False);
            //el.SetAttributeValue(attr_name, value ? TextConst.AVBool.True : TextConst.AVBool.False);
        }
        public static void SetAttrValue(this XElement el, XName attr_name, int value)
        {
            Contract.Assert(el != null);
            SetAttrValue(el, attr_name, value.ToString());
        }
        public static void SetAttrValue<T>(this XElement el, XName attr_name, T value, string format, IFormatProvider provider)
            where T : IFormattable
        {
            Contract.Assert(el != null);
            SetAttrValue(el, attr_name, value.ToString(format, provider));
        }
        public static void SetAttrValue(this XElement el, XName attr_name, DateTime value)
        {
            Contract.Assert(el != null);
            SetAttrValue(el, attr_name, value.ToString(date_format, CultureInfo.InvariantCulture));
        }
        /// <summary>
        /// Копирует в <paramref name="dest"/> аттрибуты из <paramref name="src"/>
        /// Если в <paramref name="dest"/> есть атрибут с тем же именем,
        /// его значение заменяется на новое.
        /// </summary>
        /// <param name="dest">Элемент, в который надо переместить</param>
        /// <param name="src">Список атрибутов, который надо скопировать</param>
        public static void CopyAttributes(this XElement dest, IEnumerable<XAttribute> src)
        {
            Contract.Assert(dest != null);
            Contract.Assert(src != null);
            foreach (XAttribute src_attr in src)
            {
                XAttribute dest_attr = dest.Attribute(src_attr.Name);
                if (dest_attr == null)
                {
                    dest.Add(new XAttribute(src_attr.Name, src_attr.Value));
                }
                else
                {
                    dest_attr.Value = src_attr.Value;
                }
            }
        }
        /// <summary>
        /// Удаляет аттрибут <paramref name="attr_name"/> из элемента
        /// </summary>
        /// <param name="el">элемент</param>
        /// <param name="attr_name">имя убираемого аттрибута</param>
        public static void RemoveAttribute(this XElement el, XName attr_name)
        {
            Contract.Assert(el != null);
            XAttribute attr = el.Attribute(attr_name);
            if (attr != null)
            {
                attr.Remove();
            }
        }
        /// <summary>
        /// Удаляет элемент <paramref name="element_name"/> из элемента <paramref name="el"/>.
        /// Если таких несколько, удаляется только первый из них.
        /// </summary>
        /// <param name="el">элемент</param>
        /// <param name="element_name">наименование удаляемого элемента</param>
        public static void RemoveElement(this XElement el, XName element_name)
        {
            Contract.Assert(el != null);
            XElement element = el.Element(element_name);
            if (element != null)
            {
                element.Remove();
            }
        }
        /// <summary>
        /// Перемещает элементы от одного родителя в дргугого
        /// </summary>
        /// <param name="nodes">элементы</param>
        /// <param name="new_parent">новый родитель</param>
        public static void ChangeParent(this IEnumerable<XNode> nodes, XContainer new_parent)
        {
            Contract.Assert(nodes != null);
            Contract.Assert(new_parent != null);
            IList<XNode> childs = Enumerable.ToList<XNode>(nodes);
            for (int index = 0; index < childs.Count; index++)
            {
                XNode e = childs[index];
                e.Remove();
                new_parent.Add(e);
            }
        }
        /// <summary>
        /// Перемещает аттрибуты от одного родителя в дргугого
        /// </summary>
        /// <param name="attrs">аттрибуты</param>
        /// <param name="new_parent">новый родитель</param>
        public static void ChangeParent(this IEnumerable<XAttribute> attrs, XContainer new_parent)
        {
            Contract.Assert(attrs != null);
            Contract.Assert(new_parent != null);
            IList<XAttribute> childs = Enumerable.ToList<XAttribute>(attrs);
            for (int index = 0; index < childs.Count; index++)
            {
                XAttribute attr = childs[index];
                attr.Remove();
                new_parent.Add(attr);
            }
        }
        /// <summary>
        /// Ищет в списке <paramref name="elements"/> первый элемент,
        /// у которого атрибут <paramref name="attr_name"/> имеет значение <paramref name="attr_value"/>.
        /// У всех элементов из <paramref name="elements"/> должен присутсвовать атрибут <paramref name="attr_name"/>.
        /// </summary>
        /// <param name="elements">список элементов</param>
        /// <param name="attr_name">наименование атрибута</param>
        /// <param name="attr_value">значение атрибута</param>
        /// <returns>Искомый элемент или null, если он не найден</returns>
        public static XElement SearchByAttribute(this IEnumerable<XElement> elements, XName attr_name, string attr_value)
        {
            Contract.Assert(elements != null);
            Contract.Assert(attr_name != null);
            foreach (XElement e in elements)
            {
                XAttribute attr = e.Attribute(attr_name);
                //Contract.Assert(attr != null);
                if (attr != null && attr.Value == attr_value)
                {
                    return e;
                }
            }
            return null;
        }
        public static XElement GetAncestor(this XElement element, XName name)
        {
            Contract.Assert(element != null);
            Contract.Assert(name != null);
            XElement parent = element.Parent;
            while (parent != null)
            {
                if (parent.Name == name)
                {
                    break;
                }
                parent = parent.Parent;
            }
            return parent;
        }
    }
}
