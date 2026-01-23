using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using System.Xml.Xsl;
using System.Xml.XPath;
using System.IO;
//using System.Windows.Forms;
using Devart.Data.Oracle;
using sql.builder;
using sql.builder.Controls;
using sql.builder.FieldInfo;
using System.Reflection;
using sql.builder.UI;
using sql.builder.WinForms;

namespace sql.builder.DataApi
{
    //internal static class VDocumenting
    //{
    //    public static string GetQueryColumnType(VSXElement column)
    //    {
    //        if (column == null) return "";
    //        VLookupAnalyzer anl = new VLookupAnalyzer();
    //        anl.CheckReturn = (VSXElement element) =>
    //        {
    //            if (element.IsListColumn())
    //            {
    //                return true;
    //            }
    //            return false;
    //        };

    //        anl.CheckStop = (VSXElement element) =>
    //        {
    //            return anl.CheckReturn(element);
              
    //        };


    //        VSXElement vidCol = column.LookUpSources(anl).FirstOrDefault();
    //        if (vidCol != null)
    //        {
    //            return TextConst.TVType.Link;
    //        }

    //        switch (column.XDataType())
    //        {
    //            case TextConst.AVType.Date:
    //                return TextConst.TVType.Date;
    //            case TextConst.AVType.String:
    //                return TextConst.TVType.String;
    //            case TextConst.AVType.Number:
    //                return TextConst.TVType.Number;
    //            default:
    //                return  TextConst.TVType.String;
    //        }
           
    //    }

    //    public static string GetFormFieldType(VSXElement field)
    //    {

    //        if (field.P_ControlType == typeof(UICombo).Name)
    //        {
    //            return TextConst.TVType.Link;
    //        }
    //        if (field.P_ControlType == typeof(UIList).Name)
    //        {
    //            return TextConst.TVType.Link;
    //        }
    //        if (field.P_ControlType == typeof(UIDate).Name)
    //        {
    //            return TextConst.TVType.Date;
    //        }
    //        if (field.P_ControlType == typeof(UICheck).Name)
    //        {
    //            return TextConst.TVType.Check;
    //        }

    //        return TextConst.TVType.String;

    //    }
    //    public static VDataSet CreateReportInfoDataSet(VReport report)
    //    {
    //        var templ = (VPrintTemplate)report.GetEnvironment().GetElement(TextConst.EName.ExcelTemplates, report.PrintTemplates().First().XName);
    //        VDataSet dataSet = new VDataSet();

    //        VDataTable tblSheet = new VDataTable();
    //        tblSheet.TableName = "sheet";
    //        dataSet.Tables.Add(tblSheet);
    //        tblSheet.Columns.Add(new VDataColumn("title"));

    //        VDataTable tblCol = new VDataTable();
    //        tblCol.TableName = "column";
    //        dataSet.Tables.Add(tblCol);
    //        tblCol.Columns.Add(new VDataColumn("sheet"));
    //        tblCol.Columns.Add(new VDataColumn("title"));
    //        tblCol.Columns.Add(new VDataColumn("descr"));
    //        tblCol.Columns.Add(new VDataColumn("data_type"));
    //        tblCol.Columns.Add(new VDataColumn("source"));
    //        tblSheet.ChildRelations.Add(new DataRelation("",  tblSheet.Columns["title"],tblCol.Columns["sheet"]));

    //        foreach (VSXElement sheet in templ.GetElementsApplyingParts())
    //        {
    //            tblSheet.Rows.Add(sheet.P_SelfTitle);
    //            foreach (VPrintColumn column in sheet.GetElementsApplyingParts())
    //            {
    //                tblCol.Rows.Add(sheet.P_SelfTitle, column.P_SelfTitle, column.P_DescriptionSearched, column.P_DocDataType, column.P_NavigationInfo);
    //            }
    //        }

    //        VDataTable tblParam = new VDataTable();
    //        tblParam.TableName = "param";
    //        dataSet.Tables.Add(tblParam);
    //        tblParam.Columns.Add(new VDataColumn("title"));
    //        tblParam.Columns.Add(new VDataColumn("data_type"));
    //        tblParam.Columns.Add(new VDataColumn("descr"));
    //        tblParam.Columns.Add(new VDataColumn("val"));

    //        var frm = new frmExpressReport(true);
    //         frm.Initialize(report.Attribute(TextConst.AName.Name).Value);
    //        foreach (VSXElement fld in report.Form().Fields())
    //        {
    //           var  val = fld.P_DefValInfoSearched;
    //           if (val == "")
    //           {
    //               UIBase ctrl = frm.GetUIForm().controls[fld.Attribute(TextConst.AName.Name).Value];
    //               val = ctrl.GetText();
    //               if (ctrl is UICheck)
    //               {
    //                   if (val == TextConst.TVValue.Yes)
    //                   {
    //                       val = TextConst.TVValue.Checked;
    //                   }

    //                   if (val == TextConst.TVValue.No)
    //                   {
    //                       val = TextConst.TVValue.NotChecked;
    //                   }

    //               }

    //           }
    //            tblParam.Rows.Add(fld.P_SelfTitle, fld.P_DocDataType, fld.P_DescriptionSearched,val);
    //        }
    //        frm.Dispose();


           

    //        return dataSet;



    //    }

    //    //public static XElement MakeAlbumItem(string reportName)
    //    //{
    //    //    VReport rep = XmlReports.environment.GetReport(reportName);
    //    //    VPrintTemplateCall templ = rep.PrintTemplates().First();


    //    //    XElement templateInfo = Printing.ExtractHeadInfo(templ.XName);

    //    //    foreach (XElement colinfo in templateInfo.Elements(TextConst.EName.ExcelColumn).ToList())
    //    //    {
    //    //        List<string> varNames = new List<string>();
    //    //         List<VSXElement> columns = new List<VSXElement>();
    //    //         foreach (XElement xvar in colinfo.Elements(TextConst.EName.Column).ToList())
    //    //        {
    //    //            string[] vv = xvar.Value.Split('.');
    //    //            string qname = vv[0];
    //    //            string cname = vv[vv.Length - 1];
    //    //            string vname = qname + "." + cname;

    //    //            if (!varNames.Contains(vname))
    //    //            {
    //    //                varNames.Add(vname);
    //    //                xvar.Value = vname;
    //    //                columns.Add(rep.GetQuery(qname).Query().SearchColumn(cname));
                
    //    //            }
    //    //            else
    //    //            {
    //    //                xvar.Remove();
    //    //            }
    //    //        }
    //    //         colinfo.SetAttributeValue(TextConst.AName.DataType,GetQueryColumnType(columns.First()));
    //    //    }
    //    //    return templateInfo;
            
    //    //}
    //}

    
   
}
