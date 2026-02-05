using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql;
using sql.builder;
using sql.builder.Clean;
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {

        static string BasePath = @"C:\Repos\ai\asuse-ai\asuse-ai-reports\reports-config\sql-builder";

        public static bool Enabled = false;
        public static void Initialize()
        {

            Enabled = true;
            DevUtilsProvider.Instance = new DevUtilsProviderImpl();
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";
            // var conStr = "User Id=asuse;Password=kl0pik;Server=REALKAZN;Pooling=False;Sid=REALKAZN;Port=1521";
            // TNS format connection string
            // var conStr = "User Id=asuse;Password=learning;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.60.32.80)(PORT=1521)))(CONNECT_DATA=(SID=nata)))";
            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);
        }


        private static string EscapeString(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return input.Replace("'", "\\'").Replace("\n", "\\n").Replace("\r", "");
        }


        public static void DeleteGenerated()
        {
            var sqlBuilderPath = BasePath;

            if (Directory.Exists(sqlBuilderPath))
            {
                Directory.Delete(sqlBuilderPath, recursive: true);
                Console.WriteLine($"Deleted directory: {sqlBuilderPath}");
            }
        }

        private static string ClearName(string value)
        {
            if (string.IsNullOrEmpty(value))
                return value;

            // Convert to lowercase
            value = value.ToLowerInvariant();

            // Replace all symbols except latin letters and digits with _
            var sb = new StringBuilder();
            foreach (char c in value)
            {
                if ((c >= 'a' && c <= 'z') || (c >= '0' && c <= '9'))
                {
                    sb.Append(c);
                }
                else
                {
                    sb.Append('_');
                }
            }
            value = sb.ToString();

            // Replace __ with _ while there is __
            while (value.Contains("__"))
            {
                value = value.Replace("__", "_");
            }

            return value;
        }

    }
}
