using System;
using sql.builder;
using SqlBuilderLib.DevTools;

namespace SqlBuilderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            // DevUtils.TestReportsAnalysis();
            DevTasks.AnalyzeReport();
            // DevUtils.TestSqlParsing();
            // sql.builder.Program.Main2(args);
            //             AnalyzerStorage.SaveAllCollectionsToFiles();

            //             DevAnalyzer.SetConnectionAndSourceFolder();
            //             string query = @"
            //        select
            // 	*
            // from
            // 	report_dev_sqlb.db_objects
            // where
            // 	processed = false
            // 	and
            // 	object_name in (
            // 		select
            // 			distinct used_object_name
            // 		from
            // 			report_dev_sqlb.report_dependencies
            // 		where
            // 			nav_id = 'nav101'
            // 	)
            // --and object_name='vv_day'

            // and object_name not in ('raise_application_error','a_pmax', 'edo','sumdog', 'sumobj','sumdog0','sumobj0','prop','prop0','prop1','t_row','a','dual','all_indexes','dbms_mview.refresh','o')

            // ";

            //             DbObjectDependencyLoader.LoadDependencies(query);


            // LogsLoader.LoadLogsFromCsvFiles();
        }
    }
}
