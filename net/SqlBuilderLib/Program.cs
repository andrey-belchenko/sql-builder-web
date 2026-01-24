using System;
using System.Diagnostics;
using System.IO;
using System.Data;
using System.Linq;
using System.Threading;
//using System.Windows.Forms;
using System.Xml.Linq;
using Devart.Data.Oracle;
//using DevExpress.LookAndFeel;
//using DevExpress.Skins;
//using DevExpress.UserSkins;
//using DevExpress.XtraEditors;
using infoenergo.core;
using infoenergo.core.Data;
using infoenergo.sys;
//using infoenergo.ui.win;
//using sql.builder.Controls.Testing;
//using sql.builder.Properties;
using sql.builder.DataApi;
using sql.builder.UI.CommandItems;
using sql.builder.WinForms;
using sql.builder.XmlHelpers;
//using infoenergo.framework.Extensions.Oracle;
using System.Collections.Generic;
using sql.builder.Clean;

// Basic usage


namespace sql.builder
{
    public static class Program 
    {

        public static void Main(string[] args)
        {
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            //var conStr = infoenergo.framework.Global2.BuildConnectionString("asuse", "kl0pik", "realkazn");
            var conStr = "User Id=asuse;Password=kl0pik;Server=REALKAZN;Pooling=False;Sid=REALKAZN;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            var pars = new Dictionary<string, object>();


            pars.Add("p_date_s", new DateTime(2020, 1, 8));
            pars.Add("p_date_po", new DateTime(2025, 1, 8));
            pars.Add("p_kodp", new List<int> { 1172, 1210, 1211, 1212, 1214, 1215
                //, 1216, 1217, 1218, 1219
            });

            var path =  CleanSqlBuilder.ExecReportGetPath("asuse2.65211", pars, "65211.xlsx");
            Process.Start(path);
            Console.WriteLine("done");

        }
    }
}
