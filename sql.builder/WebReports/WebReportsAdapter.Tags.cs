
////using DevExpress.XtraSpreadsheet.Utils;
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
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
//using System.Xml;
//using System.Xml.Linq;
//using sql.builder;

//namespace sql.builder.WebReports
//{
//    internal static partial class WebReportsAdapter
//    {

        

//        private static string SerializeTag(object value)
//        {
//            return JsonConvert.SerializeObject(value);
//        }

//        private static JObject DeserializeTag(string value)
//        {
//            return JsonConvert.DeserializeObject<JObject>(value);
//        }

//        private static string CreateTag(string reportId)
//        {
//            return SerializeTag(new { reportId });
//        }



//        private static string CreateFieldMethodTag(string reportId,string fieldId, string methodName)
//        {
//            var jobj = new JObject();
//            jobj["reportId"] = reportId;
//            jobj["itemId"] = fieldId;
//            jobj["fieldId"] = fieldId;
//            jobj["methodName"] = methodName;
//            return SerializeTag(jobj);
//        }

//        private static string CreateEditorMethodTag(string reportId, string fieldId, string editorId, string methodName)
//        {
//            var jobj = new JObject();
//            jobj["reportId"] = reportId;
//            jobj["itemId"] = editorId;
//            jobj["editorId"] = editorId;
//            jobj["fieldId"] = fieldId;
//            jobj["methodName"] = methodName;
//            return SerializeTag(jobj);
//        }


//        public static bool IsWebReport(Dictionary<string, string> reportInfo)
//        {
//            if (reportInfo == null)
//            {
//                return false;
//            }

//            if (!reportInfo.ContainsKey("repname"))
//            {
//                return false;
//            }

//            return IsWebItem(reportInfo["repname"]);
//        }

//        public static bool IsWebReport(ucReportContainer container)
//        {
//            if (container == null)
//            {
//                return false;

//            }
//            if (container.Grid == null)
//            {
//                return false;
//            }
//            return IsWebItem(container.Grid.ReportName);
//        }

//        public static bool IsWebItem(string value)
//        {
//            if (value == null)
//            {
//                return false;
//            }
//            return value.StartsWith("{");
//        }

       

//        private static string ExtractIdFromTag(string value)
//        {
//            return DeserializeTag(value)["reportId"].ToString();
//        }


        


//    }
//}
