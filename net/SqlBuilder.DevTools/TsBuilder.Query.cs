
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {
        private static void ProcessQuery(VQueryCall queryCall)
        {
            if (queryCall == null) return;

            var queryFileName = $"query_{ClearName(queryCall.P_Name)}.sql";
            var query = queryCall.Query();
            var sql = query.GetSql();



        }



    }
}
