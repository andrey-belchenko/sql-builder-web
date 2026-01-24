using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;
//using System.Net.Http;
using System.Net;
using System.IO;
using System.Xml;
using System.Web.Script.Serialization;


namespace sql.builder.DashboardUtils
{
	//Must be refactored with webRequest (for net4.0)
	class DataExtractHelper

	{
		internal static string MergeSqlScheme(string sql, XElement scheme)
		{
			SortedList columnsList = new SortedList();
			string mergedSQL = "SELECT ";
			XElement[] column_array = scheme.Element(DataApi.TextConst.EName.Table)
				.Element(DataApi.TextConst.EName.ViewColumns)
				.Descendants(DataApi.TextConst.EName.Column)
				.Where(c => c.Attributes(DataApi.TextConst.AName.Title)
				.Any()).ToArray();
			foreach(XElement column in column_array)
			{
				//mergedSQL = mergedSQL + column.Attribute("name").Value;
				//mergedSQL = mergedSQL + " as \"";
				string name;
				string title = column.Attribute(DataApi.TextConst.AName.Title).Value;
				if (column.Attribute(DataApi.TextConst.AName.Type).Value == "number")
				{
					//ROUND (number_var, 27 - LENGTH (TRUNC (number_var)))
					name = "ROUND (" + column.Attribute(DataApi.TextConst.AName.Name).Value + ", 27 - LENGTH (TRUNC (" + column.Attribute(DataApi.TextConst.AName.Name).Value + ")))";
				}
				else
				{
					name = column.Attribute(DataApi.TextConst.AName.Name).Value;
				}
				
				XElement _column = column;
				while(_column.Parent.Name == DataApi.TextConst.EName.Band)
				{
					title = _column.Parent.Attribute(DataApi.TextConst.AName.Title).Value + "/" + title;
					_column = _column.Parent;
				}
				//replace quotes and cut string
				title = title.Replace("\"", "'");
				if (title.Length > 30)
				{
					title = title.Substring(0, 30);
				}
				if (columnsList.Contains(title))
				{
					//throw new NotImplementedException();
				}
				else
				{
					columnsList.Add(title, name);
				}
			}
			for (int i = 0; i < columnsList.Count; i++)
			{
				mergedSQL = mergedSQL + columnsList.GetByIndex(i) + " as \"" + columnsList.GetKey(i) + "\", ";
			}
			mergedSQL = mergedSQL.Substring(0, mergedSQL.Length - 2);
			mergedSQL = mergedSQL + "FROM (" + sql + ")";
			return mergedSQL;
		}

		internal static void RegisterDataExtract(string fixedNamesSql, string project, string extractName, string allowReplace = "false")
		{
			//Must be refactored with webRequest (for net4.0)
#if DEBUG

			HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(@"http://192.168.47.118/DashboardManagerWebApi.asmx");
			//webRequest.Headers.Add(@"SOAP:Action");
			webRequest.ContentType = "application/soap+xml;charset=\"utf-8\"";
			webRequest.Accept = "application/soap+xml";
			webRequest.Method = "POST";


			XNamespace soap12 = "http://www.w3.org/2003/05/soap-envelope";
			XNamespace dsb = "http://dashboard.infoenergo.loc/";
			XDocument soapEnvelopeXml =
			new XDocument(
				new XDeclaration("1.0", "utf-8", "yes"),
				new XElement(soap12 + "Envelope",
					new XAttribute(XNamespace.Xmlns + "xsi", "http://www.w3.org/2001/XMLSchema-instance"),
					new XAttribute(XNamespace.Xmlns + "xsd", "http://www.w3.org/2001/XMLSchema"),
					new XAttribute(XNamespace.Xmlns + "soap12", "http://www.w3.org/2003/05/soap-envelope"),
					new XElement(soap12 + "Body",
						new XElement(dsb + "RegisterDataExtract",
							new XElement(dsb + "query", fixedNamesSql),
							new XElement(dsb + "project", project),
							new XElement(dsb + "extractName", extractName),
							new XElement(dsb + "allowReplace", allowReplace)))
			));


			//webRequest.ContentLength = tsoapEnvelopeXml;

			using (Stream stream = webRequest.GetRequestStream()) 
			{ 
				soapEnvelopeXml.Save(stream); 
			}
			using (WebResponse response = webRequest.GetResponse())
			{
				if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
				{
					throw new NotImplementedException();
				}
			}

			using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
			{
				//string extractName = DateTime.UtcNow.ToString("yyyyMMddHHmmssfff");
				client.BaseAddress = new Uri("http://192.168.47.118");



				//update extract
				var values = new Dictionary<string, string>
				{
					{ "extractName", extractName },
					{ "project", project}
				};
				var content = new System.Net.Http.FormUrlEncodedContent(values);
				var result = client.PostAsync("/DashboardManagerWebApi.asmx/RefreshDataExtract", content);
				result.Wait();
				if (!result.Result.IsSuccessStatusCode)
				{
					throw new NotImplementedException();
				}
			}
#endif
		}

		internal static string[] GetProjectList()
		{

			string[] projectList = null;
//Must be refactored with webRequest (for net4.0)
#if DEBUG
			using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
			{
				client.BaseAddress = new Uri("http://192.168.47.118");
				var values = new Dictionary<string, string>{};
				var content = new System.Net.Http.FormUrlEncodedContent(values);
				var result = client.PostAsync("/DashboardManagerWebApi.asmx/GetProjectListJ", content);
				//result.RunSynchronously();
				result.Wait();
				if (!result.Result.IsSuccessStatusCode)
				{
					throw new NotImplementedException();
				}
				else {
					var projectListObject = new JavaScriptSerializer().DeserializeObject(result.Result.Content.ReadAsStringAsync().Result);
                    projectList = new string[((object[])projectListObject).Length];
                    for (int i = 0; i < ((object[])projectListObject).Length; i++)
					{
						projectList[i] = (string)((object[])projectListObject)[i];
					}
				}
			}
#endif
			if (projectList == null)
			{
                projectList = Array.Empty<string>();
			}

			return projectList;
		}

		internal static string[] GetDataExtractNameListByProject(string project)
		{

			string[] extractList = null;
			//Must be refactored with webRequest (for net4.0)
#if DEBUG
			using (System.Net.Http.HttpClient client = new System.Net.Http.HttpClient())
			{
				client.BaseAddress = new Uri("http://192.168.47.118");
				var values = new Dictionary<string, string> { { "project", project } };
				var content = new System.Net.Http.FormUrlEncodedContent(values);
				var result = client.PostAsync("/DashboardManagerWebApi.asmx/GetDataExtractNameListByProjectJ", content);
				//result.RunSynchronously();
				result.Wait();
				if (!result.Result.IsSuccessStatusCode)
				{
					throw new NotImplementedException();
				}
				else
				{
					var extractListObject = new JavaScriptSerializer().DeserializeObject(result.Result.Content.ReadAsStringAsync().Result);
                    extractList = new string[((object[])extractListObject).Length];
                    for (int i = 0; i < ((object[])extractListObject).Length; i++) {
						extractList[i] = (string)((object[])extractListObject)[i];
					}
				}
			}
#endif
			if (extractList == null)
			{
                extractList = Array.Empty<string>();
			}

			return extractList;
		}
	}
}
