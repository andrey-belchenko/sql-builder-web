using System;
using System.Collections.Generic;
using System.Text;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder
{
    /*public static class StringBuilderExt
    {
        public static int IndexOf(this StringBuilder sb, char value)
        {
            Contract.Assert(sb != null);
            int index = 0;
            int len = sb.Length;
            while (index < len) {
                if (sb[index] == value) {
                    return index;
                }
                index++;
            }
            return -1;
        }
        public static int IndexOf(this StringBuilder sb, char value, int startIndex)
        {
            Contract.Assert(sb != null);
            Contract.Assert(startIndex >= 0 && startIndex < sb.Length);
            int index = startIndex;
            int len = sb.Length;
            while (index < len) {
                if (sb[index] == value) {
                    return index;
                }
                index++;
            }
            return -1;
        }
    }*/
    public static partial class StringExt
    {
        /// <summary>
        /// Ищет в строке <paramref name="str"/> первый пробельный символ, начиная с позиции <paramref name="startIndex"/> и
        /// возвращает его позицию или -1, если не найден.
        /// </summary>
        /// <param name="str">Строка для поиска</param>
        /// <param name="startIndex">Начальная позиция</param>
        /// <returns>Позиция первого встреченого пробельного символа или -1, если он не найден</returns>
        public static int IndexOfWhiteSpace(this string str, int startIndex)
        {
            Contract.Assert(str != null);
            Contract.Assert(startIndex >= 0 && startIndex < str.Length);
            int index = startIndex;
            int len = str.Length;
            while (index < len) {
                if (char.IsWhiteSpace(str, index)) {
                    return index;
                }
                index++;
            }
            return -1;
        }
        /// <summary>
        /// Возвращает подстроку строки <paramref name="str"/> до первого
        /// вхождения символа <paramref name="ch"/>. Если символа в строке нет,
        /// Возвращает всю строку <paramref name="str"/>.<br/>
        /// Например: StringExt.SubstringBefore("a.b.c", '.') =&gt; "a"<br/>StringExt.SubstringBefore(":end", ':') =&gt; "".
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="ch">символ-раделитель</param>
        /// <returns>подстрока до первого вхождения символа <paramref name="ch"/>.</returns>
        public static string SubstringBefore(this string str, char ch)
        {
            Contract.Assert(str != null);
            int pos = str.IndexOf(ch);
            if (pos >= 0) {
                return str.Substring(0, pos);
            } else {
                return str;
            }
        }
        /// <summary>
        /// Возвращает подстроку строки <paramref name="str"/> после последнего
        /// вхождения символа <paramref name="ch"/>. Если символа в строке нет,
        /// Возвращает всю строку <paramref name="str"/>.<br/>
        /// Например: StringExt.SubstringAfter("a.b.c", '.') =&gt; "c"<br/>StringExt.SubstringAfter("begin:", ':') =&gt; "".
        /// </summary>
        /// <param name="str">строка</param>
        /// <param name="ch">символ-раделитель</param>
        /// <returns>подстрока после последнего вхождения символа <paramref name="ch"/>.</returns>
        public static string SubstringAfter(this string str, char ch)
        {
            Contract.Assert(str != null);
            int pos = str.LastIndexOf(ch);
            if (pos >= 0) {
                return str.Substring(pos + 1);
            } else {
                return str;
            }
        }
    }
}
