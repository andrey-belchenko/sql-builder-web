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
    public static class TsBuilder
    {

        static string BasePath = @"C:\Repos\ai\asuse-ai\asuse-ai-reports\reports-config";
        public static void Initialize()
        {
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";
            // var conStr = "User Id=asuse;Password=kl0pik;Server=REALKAZN;Pooling=False;Sid=REALKAZN;Port=1521";
            // TNS format connection string
            // var conStr = "User Id=asuse;Password=learning;Data Source=(DESCRIPTION=(ADDRESS_LIST=(ADDRESS=(PROTOCOL=TCP)(HOST=10.60.32.80)(PORT=1521)))(CONNECT_DATA=(SID=nata)))";
            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);
        }

        private static IEnumerable<VNavigator> GetVNavigators()
        {
            var navNames = new[] { "nav310", "nav10", "nav101" };
            return XmlReports.Environment.GetElements(TextConst.EName.Navigators).Cast<VNavigator>()
                .Where(it => navNames.Contains(it.P_IdName))
                .ToList();
        }
        public static void BuildNavigators()
        {

            var navs = GetVNavigators();
            var sqlBuilderPath = Path.Combine(BasePath, "sql-builder", "navigators");

            // Create directory if it doesn't exist
            if (!Directory.Exists(sqlBuilderPath))
            {
                Directory.CreateDirectory(sqlBuilderPath);
            }

            var initialFolderId = 10000;
            foreach (var nav in navs)
            {
                var fileName = $"{nav.P_IdName}.ts";
                var filePath = Path.Combine(sqlBuilderPath, fileName);
                File.WriteAllText(filePath, "", Encoding.UTF8);
                Console.WriteLine($"Created file: {filePath}");

                foreach (var item in nav.GetElementsP())
                {
                    if (item is VFolder)
                    {
                        Console.WriteLine("Folder example:" + item.P_Title);
                        foreach (var childItem in item.GetElementsP())
                        {
                            if (childItem is VFolder)
                            {
                                Console.WriteLine("Child folder example:" + childItem.P_Title);
                            }

                            if (childItem is VUseReport)
                            {
                                Console.WriteLine("Child report example:" + childItem.P_Title);
                            }
                        }
                    }

                    if (item is VUseReport)
                    {
                        Console.WriteLine("Report example:" + item.P_Title);
                    }

                }
            }
        }

        public static void DeleteGenerated()
        {
            var sqlBuilderPath = Path.Combine(BasePath, "sql-builder");

            if (Directory.Exists(sqlBuilderPath))
            {
                Directory.Delete(sqlBuilderPath, recursive: true);
                Console.WriteLine($"Deleted directory: {sqlBuilderPath}");
            }
        }

    }
}
