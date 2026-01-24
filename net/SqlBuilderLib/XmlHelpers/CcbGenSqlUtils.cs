using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Oracle.ManagedDataAccess.Client;
//using DevExpress.Utils.Drawing.Helpers;
//using DevExpress.XtraBars.Docking2010.Base;
using sql.builder.DataApi;
using System.Collections.Generic;


namespace sql.builder.XmlHelpers
{
    internal static class CcbGenSqlUtils
    {

		

        public static void Generate()
        {
            XmlReports.Environment.LoadProject("bar_ccb");
            var queries =
              XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => q.P_IdName.StartsWith("vci_"));
      

        
            var sb = new StringBuilder();


            var sb1 = new StringBuilder();

            sb1.AppendLine("begin");
            //	sb.Append(GeneratePackage("svc_zayav_ka", testScheme));
            string[] names = Array.Empty<string>();
            // names = new string[] { "vci_hr_point_ini", "vci_hr_point_pu", "vci_hr_pu_u", "vci_hr_point_en" };
            // names = new string[] { "vci_hr_point_ini" };
            foreach (VQuery qry in queries) {
                if (names.Length == 0 || names.Contains(qry.XName)) {

                    var script = SqlReportPkg.Generate(qry.XName, false, false, false);

                    script = script.Replace("sqlb_" + qry.XName, qry.XName + "_pkg");
                    script = script.Replace(qry.XName + "_pkg_tbl", qry.XName);
                    script = script.Replace("vci_", "vcl_");
                    script = script.Replace("ON COMMIT PRESERVE ROWS;", "");
                    script = script.Replace("GLOBAL TEMPORARY", "");
                    var tname = qry.XName.Replace("vci_", "vcl_");
                    var repExpr = "delete " + tname+";";
                    if (!script.Contains(repExpr) )
                    {
                        throw  new Exception();
                    }
                    script = script.Replace(repExpr, repExpr + Environment.NewLine + "vccb_dog_info_load.put_log_begin ('"+tname+"');");


                    repExpr = "END fill_table;";
                    if (!script.Contains(repExpr))
                    {
                        throw new Exception();
                    }
                    script = script.Replace(repExpr, "vccb_dog_info_load.put_log_end ('" + tname + "');" + Environment.NewLine + repExpr);
                    
                    
                    sb.Append(script);

                    sb1.AppendLine(qry.XName.Replace("vci_", "vcl_") + "_pkg.fill_table;");
                }

          
            }
            sb1.AppendLine("end;");

            var filename = Cmn.writeScriptFile("ccb_sql", sb.ToString());


            Process.Start(filename);

            filename = Cmn.writeScriptFile("ccb_start", sb1.ToString());
            Process.Start(filename);
        }

      

    }
}
