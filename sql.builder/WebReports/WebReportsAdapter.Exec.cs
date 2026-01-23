
////using DevExpress.Utils.Extensions;
//using Newtonsoft.Json;
//using Newtonsoft.Json.Linq;
//using sql.builder.Controls.Containers;
//using sql.builder.DataApi;
//using sql.builder.WebReports.Client;
//using sql.builder.XmlHelpers;
//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics.Contracts;
//using System.IO;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using System.Xml.Linq;
//using  sql.builder;
////using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

//namespace sql.builder.WebReports
//{
//    internal static partial class WebReportsAdapter
//    {


//        public static void ExecuteReport(ucReportContainer container)
//        {

//            WaitUIHelper.LastUsedUIHelper.Show("Формирование отчета", WaitUIMode.WaitPanel, true);
//            var paramsDs = container.ParamFormC.DataSource;
//            var reportName = container.Grid.ReportName;
//            var jtag = DeserializeTag(reportName);
//            var reportId = jtag["reportId"].ToString();
//            var jparams = GetFormValues(reportId, paramsDs);
//            var jparamsText = GetFormText(reportId, container.ParamFormC);
//            var fileInfo = WebReportsClient.ExecuteReport(reportId, jparams, jparamsText);
//            var fileData = WebReportsClient.GetFileData(fileInfo.FileId);
//            var filePath = Printing.GetFreeName(
//                Printing.outputFolder,
//                Path.GetFileNameWithoutExtension(fileInfo.FileName),
//                Path.GetExtension(fileInfo.FileName).Replace(".", "")
//                );
//            File.WriteAllBytes(filePath, fileData);
//            WaitUIHelper.LastUsedUIHelper.Hide();
//            Cmn.OpenPrintedFile(filePath);

//        }


//        public static void DevExecuteView(ucReportContainer container)
//        {

//            WaitUIHelper.LastUsedUIHelper.Show("Формирование печатной формы", WaitUIMode.WaitPanel, true);
//            var paramsDs = container.ParamFormC.DataSource;
//            var reportName = container.Grid.ReportName;
//            var jtag = DeserializeTag(reportName);
//            var reportId = jtag["reportId"].ToString();
//            var jparams = GetFormValues(reportId, paramsDs);
//            var jparamsText = GetFormText(reportId, container.ParamFormC);
//            var fileInfo = WebReportsClient.ExecuteView(reportId, jparams, jparamsText);
//            var fileData = WebReportsClient.GetFileData(fileInfo.FileId);
//            var filePath = Printing.GetFreeName(
//                Printing.outputFolder,
//                Path.GetFileNameWithoutExtension(fileInfo.FileName),
//                Path.GetExtension(fileInfo.FileName).Replace(".","")
//                );
//            File.WriteAllBytes(filePath, fileData);
//            WaitUIHelper.LastUsedUIHelper.Hide();
//            Cmn.OpenPrintedFile(filePath);

//        }

//        public static void DevPrepareData(ucReportContainer container)
//        {

//            WaitUIHelper.LastUsedUIHelper.Show("Подготовка данных", WaitUIMode.WaitPanel, true);
//            var paramsDs = container.ParamFormC.DataSource;
//            var reportName = container.Grid.ReportName;
//            var jtag = DeserializeTag(reportName);
//            var reportId = jtag["reportId"].ToString();
//            var jparams = GetFormValues(reportId, paramsDs);
//            var jparamsText = GetFormText(reportId, container.ParamFormC);
//            WebReportsClient.PreparData(reportId, jparams, jparamsText);
//            WaitUIHelper.LastUsedUIHelper.Hide();

//        }



//    }
//}
