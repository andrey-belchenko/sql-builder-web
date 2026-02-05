
using System;
using System.IO;
using System.Text;
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {
        
        private static void ProcessQuery(VQueryCall queryCall)
        {
            if (queryCall == null) return;

            var fileName = $"query_{ClearName(queryCall.P_Name)}.sql";
            var folderPath = Path.Combine(BasePath, "queries");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(BasePath, "queries", fileName);
            var query = queryCall.Query();


            var cmd = query.GetSelectCommand(false);

            var sql = cmd.GetCommandText();

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, sql, Encoding.UTF8);
                Console.WriteLine($"Extracted query: {filePath}");
            }

        }



    }
}
