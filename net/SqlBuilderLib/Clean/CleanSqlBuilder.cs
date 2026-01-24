using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Devart.Data.Oracle;
using sql.builder.DataApi;

namespace sql.builder.Clean
{

    public static class CleanSqlBuilder
    {
        public static string ExecReportGetPath(string repName, Dictionary<string, object> param, string templateName)
        {
            var rep = new CleanExpressReport();
            rep.OpenDocumentAfterPrint = false;
            rep.Initialize(repName);
            foreach (var p in param) {
                rep.GetParamField(p.Key).SetValue(p.Value);
            }
            string path = string.Empty;
            rep.ReportOpening += (obj, sender) =>
            {
                path = sender.Path;
            };
            rep.ExecuteReport(templateName);
            return path;
        }
        public static void ChangeConnection(OracleConnection con, string source_folder = null)
        {
            db.Connection = con;
            XmlReports.Init(source_folder: source_folder);
        }

        public static void ChangeConnectionString(string connectionString)
        {
            var connection = new OracleConnection(connectionString);
            connection.Open();
            ChangeConnection(connection);
        }
    }

}