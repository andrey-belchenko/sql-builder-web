using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Xml.Linq;
//using DevExpress.Compression;

namespace sql.builder
{
    internal static class ExcelEnvironment
    {
        private static string _temp_directory;

        private static string _sheet_path;
        private static string _strings_path;

        public static StreamWriter Writer;
        private static Dictionary<string, int> _strings;
        private static string _header;
        private static string _footer;

        static ExcelEnvironment()
        {
            _temp_directory = Path.Combine(Path.GetTempPath(), "sql.builder.printing");

            _sheet_path = Path.Combine(_temp_directory, "xl", "worksheets", "sheet1.xml");
            _strings_path = Path.Combine(_temp_directory, "xl", "sharedStrings.xml");
        }
        internal static void BeginPrintBigData()
        {
            throw new NotImplementedException();
            //if (Directory.Exists(_temp_directory)) Directory.Delete(_temp_directory, true);

            //// загрузка базового xlsx и распаковка во временную дирректорию
            //var stream = new MemoryStream(Properties.Resources.base_xlsx);
            //var xlsx = ZipArchive.Read(stream);
            //xlsx.Extract(_temp_directory);
            //xlsx.Dispose();
            //stream.Close();

            //// тут будем хранить интернированные строки
            //_strings = new Dictionary<string, int>();

            //// вырезаем header и footer
            //var sheet = File.ReadAllText(_sheet_path);
            //var match1 = Regex.Match(sheet, "<sheetData>");
            //_header = sheet.Substring(0, match1.Index + match1.Length);
            //var match2 = Regex.Match(sheet, "</sheetData>");
            //_footer = sheet.Substring(match2.Index, sheet.Length - match2.Index);

            //Writer = new StreamWriter(_sheet_path);
            //Writer.Write(_header);
        }
        internal static void EndPrintBigData(string output_path)
        {
            throw new NotImplementedException();
            //Writer.Write(_footer);
            //Writer.Dispose();

            //var writer_strings = new StreamWriter(_strings_path);
            //writer_strings.Write(@"<?xml version=""1.0"" encoding=""UTF-8"" standalone=""yes""?>");
            //writer_strings.Write(@"<sst xmlns=""http://schemas.openxmlformats.org/spreadsheetml/2006/main"">");
            //foreach (var str in _strings.Keys)
            //{
            //    // сохранять пробелы xml:space = preserve
            //    var xsi = new XElement("si", new XElement("t", new XAttribute(XNamespace.Xml + "space", "preserve"), str));
            //    writer_strings.Write(xsi.ToString());
            //}
            //writer_strings.Write("</sst>");
            //writer_strings.Dispose();

            //var xlsx = new ZipArchive();
            //xlsx.AddDirectory(_temp_directory, "/");
            //xlsx.Save(output_path);
            //xlsx.Dispose();
        }
        internal static int InternStringAndGetIndex(string text)
        {
            int index = 0;
            if (!_strings.TryGetValue(text, out index))
            {
                index = _strings.Count;
                _strings.Add(text, index);
            }
            return index;
        }
        internal enum BigDataFormats
        {
            Date = 1,
            Default = 2,
            Number0 = 3,
            Number2 = 4,
            Currency = 5,
            String = 6
        }
    }

}
