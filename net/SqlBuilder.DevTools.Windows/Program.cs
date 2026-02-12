using System;
using System.IO;

namespace SqlBuilder.DevTools.Windows
{
    class Program
    {
        static void Main()
        {
            var excelFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates\sql.builder\printTemplate\excel";
            ExcelPreProcessor.ConvertAllXmlToXlsx(excelFolder);
            Console.WriteLine("Done.");
        }
    }
}
