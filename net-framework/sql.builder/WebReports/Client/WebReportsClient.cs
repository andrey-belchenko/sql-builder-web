using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sql.builder;
//using Microsoft.Office.Interop.Word;

namespace sql.builder.WebReports.Client
{
    internal static partial class WebReportsClient
    {

        private static string PostRaw(string endpoint, string body)
        {
            var apiUrl = "http://localhost:3111/";
            var request = (HttpWebRequest)WebRequest.Create(apiUrl + endpoint);
            request.Method = "POST";
            request.ContentType = "application/json";
            var timeout = 1000 * 60 * 60 * 5;
            request.Timeout = timeout; 
            request.ReadWriteTimeout = timeout;
            var bodyBytes = Encoding.UTF8.GetBytes(body);
            using (var stream = request.GetRequestStream())
            {
                stream.Write(bodyBytes, 0, bodyBytes.Length);
            }
            var response = (HttpWebResponse)request.GetResponse();

            using (var reader = new StreamReader(response.GetResponseStream()))
            {
                return reader.ReadToEnd();
            }
        }
        private static JObject Post(string endpoint, object body, string reportId)
        {
            var bodyText = JsonConvert.SerializeObject(body);
            var value = WebReportsCache.Get(reportId,endpoint,bodyText);
            if (value != null)
            {
                return (JObject)value;
            }
            var responseText = PostRaw(endpoint, bodyText);
            var responseData = JsonConvert.DeserializeObject<JObject>(responseText);
            WebReportsCache.Add(reportId,endpoint,responseData, bodyText);    
            return responseData;
        }




        //public static JObject GetReportsConfig(string navigatorId)
        //{
        //    return Post("reports-config",new { navigatorId });
        //}


        private static ConfigItem ParseINavigatorItem(JObject jitem)
        {

            if (jitem == null)
            {
                return null;
            }
            ConfigItem item = null;
            var children = new List<NavigatorItem>();
            var className = (string)jitem["className"];
            JArray jchildren = null;

            if ((new[] { "Navigator", "Folder" }).Contains(className))
            {
                jchildren = (JArray)jitem["items"];
                foreach (JObject jchild in jchildren)
                {
                    var child = ParseINavigatorItem(jchild);
                    if (child != null)
                    {
                        children.Add((NavigatorItem)child);
                    }

                }

            }

            switch (className)
            {
                case "Navigator":
                    if (!children.Any()) break;
                    item = new Navigator() { Items = children };

                    break;
                case "Folder":
                    if (!children.Any()) break;
                    item = new Folder() { Items = children, Title = (string)jitem["title"] };

                    children.ForEach(it => it.Parent = (Folder)item);

                    break;
                case "RegularReport":
                    if (!(bool)jitem["desktop"]) break;
                    item = new Report() { Title = (string)jitem["title"] };
                    break;
            }

            if (item != null)
            {
                item.Id = (string)jitem["id"];
            }
            return item;
        }

        public static Navigator GetNavigator(string navigatorId)
        {
            var config = Post("reports-config", new { navigatorId }, null);
            return ParseINavigatorItem(config["data"] as JObject) as Navigator;
        }

        public static JToken CallMethod(string reportId, string configItemId, string methodName, JObject formValues = null, JObject formText = null)
        {
            var body = new JObject();
            body["methodName"] = methodName;
            body["configItemId"] = configItemId;
            var pars = new JObject();
            body["params"] = pars;
            //pars["departmentId"] = new JValue(XmlReports.GetGlobalParValue("dep"));
            if (formValues != null)
            {
                pars["formValues"] = formValues;
            }

            if (formText != null)
            {
                pars["formText"] = formText;
            }
            var response = Post("config-item/method/call", body, reportId);
            return response["data"];

        }

        private static JObject GetConfigItem(string reportId, string configItemId)
        {
            var value = WebReportsCache.Get(reportId, configItemId);
            if (value != null)
            {
                return value as JObject;
            }
            var body = new JObject();
            body["configItemId"] = configItemId;
            var response = Post("config-item", body, reportId);
            return response["data"] as JObject;
        }

        public static Editor GetEditor(string reportId, string editorId)
        {
            return ParseEditor(GetConfigItem( reportId, editorId));
        }

        public static Field GetField(string reportId, string fieldId)
        {
            return ParseField(GetConfigItem(reportId,fieldId));
        }

        public static FileInfo ExecuteReport(string reportId, JObject formValues, JObject formText)
        {
            var result = (JObject)CallMethod(null, reportId, "execute", formValues, formText);
            return new FileInfo() { FileId = result["view"]["fileId"].ToString(), FileName = result["view"]["fileName"].ToString() };
        }

        public static FileInfo ExecuteView(string reportId, JObject formValues, JObject formText)
        {
            var result = (JObject)CallMethod(null, reportId, "executeView", formValues, formText);
            return new FileInfo() { FileId = result["view"]["fileId"].ToString(), FileName = result["view"]["fileName"].ToString() };
        }

        public static void PreparData(string reportId, JObject formValues,  JObject formText)
        {
            CallMethod(null, reportId, "prepareData", formValues, formText);
        }

        public static byte[] GetFileData(string fileId)
        {
            var result = Post("file", new { fileId }, null);
            return Convert.FromBase64String(result["fileData"].ToString());
        }

        private static Editor ParseEditor(JObject jeditor)
        {
            Editor editor;
            var editorClassName = jeditor["className"].ToString();
            switch (editorClassName)
            {
                case "DateEditor":
                    editor = new DateEditor();
                    break;
                case "SelectEditor":
                    editor = new SelectEditor()
                    {
                        SingleSelection = (bool)jeditor["singleSelection"],
                        KeyField = jeditor["keyField"].ToString(),
                        DisplayField = jeditor["displayField"].ToString(),
                        Columns = ((JArray)jeditor["columns"]).Select(it => it.ToString()).ToList(),
                        ListItemsDeps = ((JArray)jeditor["listItemsDeps"]).Select(it => it.ToString()).ToList()
                    };
                    break;
                default: throw new Exception("Unhandled edtor "+ editorClassName);
            }
            editor.Id = jeditor["id"].ToString();

            return editor;
        }

        private static Field ParseField(JObject jfield)
        {
            var jeditor = (JObject)jfield["editor"];
            Editor editor = ParseEditor(jeditor);
            var fieldMethodNames = ((JArray)jfield["methodNames"]).Select(it => it.ToString()).ToList();
            return new Field()
            {
                Id = jfield["id"].ToString(),
                Label = jfield["label"].ToString(),
                Name = jfield["name"].ToString(),
                Editor = editor,
                HasDefaultValue = fieldMethodNames.Contains("defaultValue"),
                HasRequiredOption = fieldMethodNames.Contains("required"),
                DefaultValueDeps = ((JArray)jfield["defaultValueDeps"]).Select(it => it.ToString()).ToList()
            };
        }

        public static ParamsForm GetForm(string reportId)
        {
            var jform = CallMethod(reportId, reportId, "paramsForm");
            var form = new ParamsForm()
            {
                Id = jform["id"].ToString(),
            };
            foreach (JObject jfield in (JArray)jform["fields"])
            {
               
                var field = ParseField(jfield);
                WebReportsCache.Add(reportId, field.Id,jfield);
                WebReportsCache.Add(reportId, field.Editor.Id, jfield["editor"]);
                form.Fields.Add(field);
            }
            return form;

        }


    }
}
