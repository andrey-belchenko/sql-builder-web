using System;
using System.IO;
using System.Text;

namespace sql.builder
{
    /// <summary>
    /// Тоже самое что и <see cref="System.IO.StringWriter" />, за исключением того,
    /// что его свойство <see cref="StringWriterUTF8.Encoding" /> возвращает
    /// кодировку utf-8 вместо utf-16, как у базового класса.
    /// Необходимо для того, чтобы сформировать правильный заголовок &lt;?xml version="1.0" encoding="utf-8"?&gt;
    /// </summary>
    public class StringWriterUTF8 : StringWriter
    {
        public StringWriterUTF8() : base() { }
        public StringWriterUTF8(IFormatProvider formatProvider) : base(formatProvider) { }
        public StringWriterUTF8(StringBuilder sb) : base(sb) { }
        public StringWriterUTF8(StringBuilder sb, IFormatProvider formatProvider) : base(sb, formatProvider) { }
        /// <summary>
        /// Возвращает кодировку utf-8 вместо utf-16 у базового класса.
        /// </summary>
        public override Encoding Encoding
        {
            get
            {
                return Encoding.UTF8;
            }
        }
    }
}
