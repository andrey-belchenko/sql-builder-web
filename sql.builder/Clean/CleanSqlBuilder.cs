using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using Devart.Data.Oracle;
using sql.builder.DataApi;

namespace sql.builder.Clean
{

    internal static class CleanSqlBuilder
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

        //public static XElement GetParamsXml(Dictionary<string, object> param)
        //{
        //    var xparams =  new XElement(EName.@params);
        //    foreach (var p in param)
        //    {
        //        object value = null;
        //        if (p.Value != null)
        //        {
        //            if (p.Value is IEnumerable)
        //            {
        //                value = new XElement(EName.call, new XAttribute(AName.function, TextConst.AVFunction.Array));

        //                foreach (object v in (p.Value as IEnumerable))
        //                {
        //                    (value as XElement).Add(new XElement(EName.@const, new XText(v.ToString());
        //                }

        //            }
        //        }
        //    }
        //    return xparams;
        //}

    }

}