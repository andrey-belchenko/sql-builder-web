using System;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Devart.Data.Oracle;
//using DevExpress.Utils.Drawing.Helpers;
using sql.builder.DataApi;
using System.Collections.Generic;


namespace sql.builder.XmlHelpers
{
    internal static class DokGenerationASUTP
    {

        public static void GeterateDoc()
        {
            throw new NotImplementedException();
   //         XmlReports.Environment.LoadProject("kido_asutp");
   //         var ds = new DataSet();
   //         var clsTbl = new DataTable("a");
   //         ds.Tables.Add(clsTbl);
   //         clsTbl.Columns.Add("name");
   //         clsTbl.Columns.Add("title");
   //         var fldTbl = new DataTable("b");
   //         ds.Tables.Add(fldTbl);
   //         fldTbl.Columns.Add("class");
   //         fldTbl.Columns.Add("name");
   //         fldTbl.Columns.Add("type");
   //         fldTbl.Columns.Add("title");
   //         fldTbl.Columns.Add("ref");
			//fldTbl.Columns.Add("comment");
   //         ds.Relations.Add(clsTbl.Columns["name"], fldTbl.Columns["class"]);
   //         var queries =
   //             XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => q.P_IdName.StartsWith("svc_"));

   //         foreach (VQuery qry in queries)
   //         {
   //             clsTbl.Rows.Add(qry.P_IdName, qry.P_Title);
   //             foreach (var fld in qry.Columns())
   //             {
   //                 fldTbl.Rows.Add(qry.P_IdName, fld.XName, fld.XDataType(), fld.P_Title,fld.P_CalledQuery, fld.P_Comment);
   //             }
   //         }
   //         string fullPath = Printing.PrintWord(ds, "50399.docx", "Описание данных");

   //         Cmn.OpenPrintedFile(fullPath);
        }

    }
}
