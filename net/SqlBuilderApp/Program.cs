using System;
using sql.builder;
using SqlBuilderLib.DevTools;

namespace SqlBuilderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // sql.builder.Program.TestReportsAnalysis(args);
            // sql.builder.Program.TestSqlParsing(args);
            //sql.builder.Program.Main2(args);
            AnalyzerStorage.SaveAllCollectionsToFiles();
        }
    }
}
