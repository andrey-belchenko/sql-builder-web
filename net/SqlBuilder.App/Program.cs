using System;
using System.Collections.Generic;
using System.Text;
using sql.builder;
using sql.builder.Clean;
using SqlBuilderLib.DevTools;

using static System.Runtime.InteropServices.JavaScript.JSType;
using static sql.builder.DataApi.VClientCalculations;

namespace SqlBuilderApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;

            //AnalyzerStorage.SaveAllCollectionsToFiles();
            //DevUtils.TestReportsAnalysis();
            //DevTasks.AnalyzeReport();
            DevTasks.AnalyzeReports();
            //DevUtils.TestSqlParsing();


            // sql.builder.Program.Main(args);


            //             DevAnalyzer.Initialize();
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



            //// Генерация форм
            //TsBuilder.DeleteGenerated();
            //TsBuilder.Initialize();
            //TsBuilder.BuildNavigators();


            ////////////////

            //var pars = new Dictionary<string, object>();
            //pars.Add("p_dep", 3580m);
            //pars.Add("p_ym_beg", 2025.06m);
            ////pars.Add("p_dog", new List<int> { 76111, 76108});


            //var globPars = new Dictionary<string, object>();
            ////pars.Add("dep", 3580m);

            //CleanSqlBuilder.ExecuteReport("ryazan.76607", "76607.xlsx", pars, globPars);
            ;
        }
    }
}
