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
            // AnalyzerStorage.SaveAllCollectionsToFiles();

            string query = @"
        SELECT object_name, object_type, processed
        FROM report_dev_sqlb.db_objects
        WHERE processed = false
            AND object_name IN (
                SELECT DISTINCT used_object_name
                FROM report_dev_sqlb.report_dependencies
                WHERE nav_id = 'nav310'
            )";

            DbObjectDependencyLoader.LoadDependencies(query);
        }
    }
}
