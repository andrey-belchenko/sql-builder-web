using System;
using System.IO;
using System.Reflection;

namespace SqlBuilder.DevTools.Win
{
    class Program
    {
        static void Main()
        {
            // Redirect Office 16 to Office 14 when only Office 2010 (v14) is installed
            AppDomain.CurrentDomain.AssemblyResolve += (sender, args) =>
            {
                var requested = new AssemblyName(args.Name);
                if (requested.Name != "office" && requested.Name != "excel")
                    return null;
                if (requested.Version?.Major != 16)
                    return null;

                var assemblyName = requested.Name;
                var gacPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    "Microsoft.NET", "assembly", "GAC_MSIL",
                    assemblyName, "14.0.0.0__71e9bce111e9429c",
                    assemblyName + ".dll");
                if (File.Exists(gacPath))
                {
                    try { return Assembly.LoadFrom(gacPath); }
                    catch { }
                }
                try
                {
                    return Assembly.Load(assemblyName + ", Version=14.0.0.0, Culture=neutral, PublicKeyToken=71e9bce111e9429c");
                }
                catch { }
                return null;
            };

            var excelFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates\sql.builder\printTemplate\excel";
            ExcelPreProcessor.ConvertAllXmlToXlsx(excelFolder);
            Console.WriteLine("Done.");
        }
    }
}
