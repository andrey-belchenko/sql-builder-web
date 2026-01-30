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

            DevAnalyzer.SetConnectionAndSourceFolder();
            string query = @"
       select
	*
from
	report_dev_sqlb.db_objects
where
	processed = false
	and
	object_name in (
		select
			distinct used_object_name
		from
			report_dev_sqlb.report_dependencies
		where
			nav_id = 'nav310'
	)
--and object_name='sqlb_rep_61880_10.fill_table'

and object_name not in ('RAISE_APPLICATION_ERROR')

";

            DbObjectDependencyLoader.LoadDependencies(query);
        }
    }
}
