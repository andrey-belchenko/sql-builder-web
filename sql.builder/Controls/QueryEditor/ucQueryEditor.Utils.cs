//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.Drawing;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Text.RegularExpressions;
////using System.Windows.Forms;
//using System.Xml;
//using System.Xml.Linq;
//using Devart.Data.Oracle;
//using DevExpress.DataProcessing;
//using DevExpress.XtraBars;
//using DevExpress.XtraBars.Docking2010;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;

//using DevExpress.XtraTreeList;
//using DevExpress.XtraTreeList.Columns;
//using DevExpress.XtraTreeList.Nodes;
////using FastReport.Utils;
//using infoenergo.core.Extensions;
//using sql.builder.DataApi;
//using sql.builder.FieldInfo;
//using sql.builder.Properties;
//using sql.builder.Test;
//using sql.builder.UI;
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;


//namespace sql.builder
//{
//    partial class ucQueryEditor : ucBase
//    {
//        private void CreateMatViewScript()
//        {
//            var qname = Query.P_Name;
//            //Wait.Show(String.Format("Генерация скрипта {0}", qname));
//            WaitUIHelper.LastUsedUIHelper.Show(String.Format("Генерация скрипта {0}", qname), WaitUIMode.WaitPanel);
//            // XmlReports.loadXml(null, null, SqlBuilder.InputParams);
//            VQuery q = XmlReports.Environment.GetPrecompiledQuery(qname);
//            var qsname = string.Empty;
//            if (q.Attribute("stored") != null)
//                qsname = q.Attribute("stored").Value.ToString();
//            else
//                qsname = q.Attribute("name").Value.ToString();

//            var qry = new XElement(q);
//            qry.Attributes(TextConst.AName.Stored).Remove();
//            qry.Attributes(TextConst.AName.Name).Remove();

//            var cqry = Compiler.GetCompiledAndProcessedQuery(qry);
//            // var sql = Compiler.GetSql(XmlReports.getItemProcessedXml2("query", qname, false));
//            var sql = Compiler.GetSql(cqry);
//            string sqlbegin = string.Format(@"
//-- Start of DDL Script for Materialized View ASUSE.M_VIEW
//-- Generated {1}

//CREATE OR REPLACE VIEW {0}_V 
//AS ", qsname, DateTime.Now.ToString());

//            string sqlend = string.Format(@"/
//-- Synonym for View
//create or replace public synonym {0}_V for {0}_V
///
//-- Grants for View
//GRANT SELECT ON {0}_V TO public 
///

//DROP MATERIALIZED VIEW {0} 
///
//CREATE MATERIALIZED VIEW {0} 
//NOLOGGING
//BUILD IMMEDIATE
//REFRESH FORCE ON DEMAND 
//--REFRESH START WITH ROUND(SYSDATE+1) NEXT SYSDATE + 1
//AS select * from {0}_v
///

//-- Synonym for View
//create or replace public synonym {0} for {0}
///
//-- Grants for View
//GRANT SELECT ON {0} TO public 
///
//", qsname);


////            var idxstr = @"CREATE INDEX {0}_{1} ON {0} ({1}  ASC )
////";
////            var colsWithIndex = q.Elements(TextConst.EName.From).Descendants(TextConst.EName.Column)
////                .Where(e1 => Cmn.GetAttrValue(e1, TextConst.AName.Table) == TextConst.AVTable.Ths)
////                .Select(e2 => e2.Attribute(TextConst.AName.Column).Value).Distinct().ToList();
////            colsWithIndex.AddRange((Query as VQuery).Columns().Where(e => e.P_Index != "").Select(c => c.XName).Distinct().ToList());
////            colsWithIndex = colsWithIndex.Distinct().ToList();
////            foreach (string col in colsWithIndex)
////            {
////                var idxstr1 = string.Format(idxstr, qsname, col) + Environment.NewLine;
////                sqlend += idxstr1;
////            }
//            sqlend += SqlSchemeBuilder.GenerateIndexesString(Query, qsname,qsname);
//            // создаем в том же каталоге скрипт для вьюхи
//            var sdata = sqlbegin + Cmn.ClearUndefined(sql) + sqlend;

//            if (Query.P_Interval != "")
//            {
//                var sjob_tmp = @"declare
//    v_job_name varchar2(30):='{0}';
//    v_view_name varchar2(30):='{1}';
//    v_interval varchar2(30):='{2}';
//begin
//    for j in ( SELECT * FROM dba_scheduler_jobs WHERE job_name =upper(v_job_name)) loop
//     DBMS_SCHEDULER.DROP_JOB(v_job_name);
//    end loop;
//    dbms_scheduler.create_job
//    (
//        job_name=>upper(v_job_name),
//        job_type        => 'PLSQL_BLOCK',
//        job_action=>'begin vg_mview.refresh('''||v_view_name||'''); end;',
//        start_date => sysdate,
//        enabled => true,
//        auto_drop => false,
//        repeat_interval=>v_interval
//    );
//end;
///";
//                var sjob = string.Format(sjob_tmp, "VJ_" + qsname, qsname, Query.P_Interval);
//                sdata += Environment.NewLine + sjob;
//            }
//            //writeScriptFile(qsname, sdata);
//            var namefile = Cmn.writeScriptFile(qsname, sdata);

//            WaitUIHelper.LastUsedUIHelper.Hide();
//            //Wait.Hide();
//            Process.Start(namefile);
//        }
//        private void CreateReportPackageScript()
//        {
//            var s = SqlReportPkg.Generate(Query.XName,false,false,false);
//            var filename = Cmn.writeScriptFile(Query.XName, s);
//            Process.Start(filename);
//        }
//    }
//}
