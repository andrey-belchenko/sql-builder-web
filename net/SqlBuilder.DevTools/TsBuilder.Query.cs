
using System;
using System.IO;
using System.Linq;
using System.Text;
using sql.builder;
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {

        private static string ProcessQuery(string name)
        {
            if (name == null) return null;

            if (name == "") return null;

            if (name == "1") return "1";
            if (name == "0") return "0";

            if (new[] { "ym-end-date" }.Contains(name)) return null;


            var query = XmlReports.Environment.GetElements(TextConst.EName.Queries).Cast<VQuery>()
                .Where(it => it.P_IdName == name)
                .First();

            var fileName = $"query_{ClearName(name)}.sql";
            var folderPath = Path.Combine(BasePath, "queries");
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }
            var filePath = Path.Combine(BasePath, "queries", fileName);
            // var query = queryCall.Query();


            var cmd = query.GetSelectCommand(false);

            var sql = cmd.GetCommandText();

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, sql, Encoding.UTF8);
                Console.WriteLine($"Extracted query: {filePath}");
            }
            return name;

        }



    }
}
