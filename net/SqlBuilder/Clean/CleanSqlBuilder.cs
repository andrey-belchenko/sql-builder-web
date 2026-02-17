using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Xml.Linq;
using Devart.Data.Oracle;
using infoenergo.sys;
using sql.builder.DataApi;
using sql.builder.UI;
using sql.builder.WinForms;
using SqlBuilderLib.DevTools;

namespace sql.builder.Clean
{

    public static class CleanSqlBuilder
    {

        public static string ExecReportGetPath(string repName, Dictionary<string, object> param, string templateName)
        {
            var rep = new CleanExpressReport();
            rep.OpenDocumentAfterPrint = false;
            rep.Initialize(repName);
            foreach (var p in param)
            {
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

        public static VDataSet ExecReportGetDs(string repName, Dictionary<string, object> param)
        {
            var rep = new CleanExpressReport();
            rep.OpenDocumentAfterPrint = false;
            rep.Initialize(repName);
            foreach (var p in param)
            {
                rep.GetParamField(p.Key).SetValue(p.Value);
            }
            string path = string.Empty;
            rep.ReportOpening += (obj, sender) =>
            {
                path = sender.Path;
            };
            return rep.ExecuteReportGetDs();
        }
        public static void ChangeConnection(OracleConnection con, string source_folder = null)
        {
            db.Connection = con;
            Global.Connection = con;
            XmlReports.Init(source_folder: source_folder);
        }

        public static void ChangeConnectionString(string connectionString)
        {
            var connection = new OracleConnection(connectionString);
            connection.Open();
            ChangeConnection(connection);
        }

        public static void Init(){
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";
            ChangeConnectionString(conStr);
            Console.WriteLine(conStr);
            XmlReports.SetGlobalParValue("dep", 3580m);
        }
        public static XElement GetFormConfig(string reportName)
        {
            var projRep =  CleanFrmExpressReport.GetProjectFromReportName(reportName);
            string project = projRep.Item1;
            reportName = projRep.Item2;
            var report = XmlReports.Environment.GetPrecompiledReport(reportName, project);
            var xform = XmlReports.GetForm(report.P_Form, reportName);
            return xform;
        }

        public static string ExecuteReport(string reportName, string templateName, Dictionary<string, object> pars, Dictionary<string, object> globPars)
        {
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";
            ChangeConnectionString(conStr);
            return ExecuteReport(reportName, templateName, pars, globPars, connection: null);
        }

        /// <summary>
        /// Execute report with per-request connection for web/async context. When connection is provided, uses request-scoped isolation for concurrent execution.
        /// </summary>
        public static string ExecuteReport(string reportName, string templateName, Dictionary<string, object> pars, Dictionary<string, object> globPars, OracleConnection connection)
        {
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";

            if (connection != null)
            {
                Global.RequestConnection.Value = connection;
                try
                {
                    XmlReports.Init(source_folder: null);
                    foreach (var globPar in globPars)
                    {
                        XmlReports.SetGlobalParValue(globPar.Key, globPar.Value);
                    }
                    return ExecReportGetPath(reportName, pars, templateName);
                }
                finally
                {
                    Global.RequestConnection.Value = null;
                    XmlReports.RequestEnvironment.Value = null;
                }
            }
            else
            {
                var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";
                ChangeConnectionString(conStr);
                foreach (var globPar in globPars)
                {
                    XmlReports.SetGlobalParValue(globPar.Key, globPar.Value);
                }
                return ExecReportGetPath(reportName, pars, templateName);
            }
        }


        public static VDataSet ExecuteReportGetDs(string reportName, Dictionary<string, object> pars, Dictionary<string, object> globPars, OracleConnection connection)
        {
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";

            if (connection != null)
            {
                Global.RequestConnection.Value = connection;
                try
                {
                    XmlReports.Init(source_folder: null);
                    foreach (var globPar in globPars)
                    {
                        XmlReports.SetGlobalParValue(globPar.Key, globPar.Value);
                    }
                    return ExecReportGetDs(reportName, pars);
                }
                finally
                {
                    Global.RequestConnection.Value = null;
                    XmlReports.RequestEnvironment.Value = null;
                }
            }
            else
            {
                var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";
                ChangeConnectionString(conStr);
                foreach (var globPar in globPars)
                {
                    XmlReports.SetGlobalParValue(globPar.Key, globPar.Value);
                }
                return ExecReportGetDs(reportName, pars);
            }
        }

    }

}