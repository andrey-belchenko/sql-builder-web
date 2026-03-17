using System;
using System.IO;
using System.Text;
using sql.builder;
using sql.builder.Clean;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {

        static string BasePath = @"C:\Repos\ai\asuse-ai\asuse-ai-reports\reports-config\sql-builder\generated";

        public static bool Enabled = false;
        public static void Initialize(bool withConnection = true)
        {

            Enabled = true;
            DevUtilsProvider.Instance = new DevUtilsProviderImpl();
            Console.OutputEncoding = Encoding.UTF8;
            DevAnalyzer.PrepareOnly = true;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            // Oracle.ManagedDataAccess.Core format: Data Source=host:port/sid
            var conStr = "Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=ryazan-ora.infoenergo.loc)(PORT=1521)))(CONNECT_DATA=(SERVER=DEDICATED)(SERVICE_NAME=realryaz)));User Id=asuse;Password=kl0pik;Pooling=False";
            // var conStr = "Data Source=REALKAZN:1521/REALKAZN;User Id=asuse;Password=kl0pik;Pooling=False";
            // TNS format (alternative)
            // var conStr = "User Id=asuse;Password=learning;Data Source=(DESCRIPTION=(ADDRESS=(PROTOCOL=TCP)(HOST=10.60.32.80)(PORT=1521))(CONNECT_DATA=(SID=nata)))";
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
