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
using System.Reflection;
////using System.Windows.Forms;
using Devart.Data.Oracle;
//using DevExpress.XtraBars.Ribbon;
//using DevExpress.XtraEditors;
using sql.builder.Controls;
using sql.builder.WinForms;
using sql.builder.XmlHelpers;
using sql.builder.UI;

namespace sql.builder.DataApi
{
    internal partial class VUseAction
    {
        

        //public static void ExecuteAction(string formName, VUseAction action, VDataSet dataSet, UIFormC senderForm, VDataTable table, DataRow inrow, VDataColumn col, ucMainReports reportsForm = null,UIFormC paramsForm=null)
        //{
        //    bool cnc = false;
        //    if (dataSet != null)
        //    {
        //        cnc = dataSet.ChangesNotCompleted;
        //        dataSet.ChangesNotCompleted = true;
        //    }
        //    if (formName != null)
        //    {
        //        var form = (VForm)XmlReports.Environment.GetElement(EName.forms, formName, TextConst.AName.Name);

        //        action.VirtualParent = form;
        //    }
        //    //action.environment = XmlReports.Environment;
           
        //    action.Execute(dataSet, senderForm, table, inrow,col, reportsForm:reportsForm,paramsForm:paramsForm);
        //    if (dataSet != null)
        //    {
                
        //        dataSet.ChangesNotCompleted = cnc;
        //        if (!cnc && dataSet.Report != null)
        //        {
        //            dataSet.RaiseChangeCompleted();
        //        }
        //    }
        //}


       
    


    }
}
