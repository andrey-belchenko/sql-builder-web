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
using System.Text;
using sql.builder.Clean;
using SqlBuilderLib.DevTools;

// Basic usage


namespace sql.builder
{
    public static class Program
    {

        public static void Main(string[] args)
        {
            DevAnalyzer.Enabled = true;
            DevAnalyzer.ClearTempFolder();
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            XmlReports.SetGlobalParValue("dep", 3580m);
            var pars = new Dictionary<string, object>();


            pars.Add("p_dep", 3580m);
            pars.Add("p_ym_beg", 2025.06m);

            var path = CleanSqlBuilder.ExecReportGetPath("ryazan.76607", pars, "76607.xlsx");
            // Output as file URI for VS Code debug console to recognize as clickable link
            //var fileUri = new Uri(path).ToString();
            Console.WriteLine(path);
            var tablenames = DevAnalyzer.tablenames;
            Console.WriteLine("done");

        }

        public static void Main2(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            XmlReports.SetGlobalParValue("dep", 3580m);
            var pars = new Dictionary<string, object>();


            pars.Add("p_dep", 3580m);
            pars.Add("p_ym_beg", 2025.06m);

            var path =  CleanSqlBuilder.ExecReportGetPath("ryazan.76607", pars, "76607.xlsx");
            // Output as file URI for VS Code debug console to recognize as clickable link
            //var fileUri = new Uri(path).ToString();
            Console.WriteLine(path);
            Console.WriteLine("done");

        }

        public static void Main1(string[] args)
        {
            Console.OutputEncoding = Encoding.UTF8;
            XmlReports.SourceFolder = @"C:\Repos\ai-tfs\root\main\all\sql.builder.templates";
            var conStr = "User Id=asuse;Password=kl0pik;Server=REALKAZN;Pooling=False;Sid=REALKAZN;Port=1521";
            //var conStr = "User Id=asuse;Password=kl0pik;Server=realryaz;Pooling=False;Sid=realryaz;Port=1521";

            CleanSqlBuilder.ChangeConnectionString(conStr);
            Console.WriteLine(conStr);

            var pars = new Dictionary<string, object>();
          

            pars.Add("p_date_s", new DateTime(2020, 1, 8));
            pars.Add("p_date_po", new DateTime(2025, 1, 8));
            pars.Add("p_kodp", new List<int> { 1172, 1210, 1211, 1212, 1214, 1215
                //, 1216, 1217, 1218, 1219
            });

            var path = CleanSqlBuilder.ExecReportGetPath("asuse2.65211", pars, "65211.xlsx");
            // Output as file URI for VS Code debug console to recognize as clickable link
            var fileUri = new Uri(path).ToString();
            Console.WriteLine(fileUri); // VS Code will make this clickable
            Console.WriteLine("done");

        }
    }
}
