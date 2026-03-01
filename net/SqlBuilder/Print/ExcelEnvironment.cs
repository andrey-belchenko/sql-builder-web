using System;
using System.Collections.Generic;
using System.IO;
//using DevExpress.Compression;

namespace sql.builder
{
    public static class ExcelEnvironment
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
            _temp_directory = Path.Combine(sql.builder.Clean.Settings.GetInstance().TempPath, "sql.builder.printing");

            _sheet_path = Path.Combine(_temp_directory, "xl", "worksheets", "sheet1.xml");
            _strings_path = Path.Combine(_temp_directory, "xl", "sharedStrings.xml");
        }
        public static void BeginPrintBigData()
        {
            throw new NotImplementedException();
        }
        public static void EndPrintBigData(string output_path)
        {
            throw new NotImplementedException();
        }
        public static int InternStringAndGetIndex(string text)
        {
            int index = 0;
            if (!_strings.TryGetValue(text, out index))
            {
                index = _strings.Count;
                _strings.Add(text, index);
            }
            return index;
        }
        public enum BigDataFormats
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
