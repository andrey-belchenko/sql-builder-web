
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.Clean.Extensions
{

    public static class Extensions
    {
        public static int FindIndex<T>(this IList<T> list, Predicate<T> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate(list[i]))
                {
                    return i;
                }
            }

            return -1;
        }

        public static bool IsValidIndex<T>(this IList<T> array, int index)
        {
            if (array != null && index >= 0)
            {
                return index < array.Count;
            }

            return false;
        }

        public static int GetValidIndex<T>(this IList<T> array, int index)
        {
            return Math.Max(0, Math.Min(index, array.Count - 1));
        }

        public static bool TryGetValue<T>(this IList<T> array, int index, out T value)
        {
            if (array.IsValidIndex(index))
            {
                value = array[index];
                return true;
            }

            value = default(T);
            return false;
        }

        public static string GetAttributeValue(this XElement element, string attributeName)
        {
            var attr = element.Attribute(attributeName);
            if (attr == null) return null;
            return attr.Value;
        }

        public static IEnumerable<T> Append<T>(this IEnumerable<T> enumerable, T value)
        {
            var list = enumerable.ToList();
            list.Add(value);
            return list;
        }


    }

    public static class XMLExtensions
    {
        public static string AttrOrDef(this XElement el, string attr_name, string def)
        {
            return (el != null) ? (((string)el.Attribute(attr_name)) ?? def) : def;
        }

        public static int AttrOrDef(this XElement el, string attr_name, int def)
        {
            if (el != null)
            {
                XAttribute xAttribute = el.Attribute(attr_name);
                if (xAttribute != null)
                {
                    return (int)xAttribute;
                }
            }

            return def;
        }

        public static bool AttrOrDef(this XElement el, string attr_name, bool def)
        {
            if (el != null)
            {
                XAttribute xAttribute = el.Attribute(attr_name);
                if (xAttribute != null)
                {
                    return (bool)xAttribute;
                }
            }

            return def;
        }

        public static string ValOrDef(this XElement el, string el_name, string def)
        {
            XElement xElement = el.Element(el_name);
            return (xElement != null) ? xElement.Value : def;
        }
    }



}