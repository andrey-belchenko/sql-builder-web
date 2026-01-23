
//using DevExpress.DataProcessing.InMemoryDataProcessor;
using Newtonsoft.Json.Linq;
using sql.builder.DataApi;
using sql.builder.WebReports.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;

namespace sql.builder.WebReports
{
    internal static partial class WebReportsAdapter
    {
        private static void RefreshDatasetMethod(VDataSet dataSet)
        {
            throw new NotImplementedException();
            //var jtag = DeserializeTag(dataSet.DataSetName);
            //var fieldId = jtag["fieldId"]!=null ? jtag["fieldId"].ToString() : null;
            //var reportId = jtag["reportId"].ToString();
            //Field field = WebReportsClient.GetField(reportId,fieldId);
            //var methodName = jtag["methodName"].ToString();
            //var itemId = jtag["itemId"].ToString();

            //var deps = new List<string>();

            //if (methodName == "defaultValue")
            //{
            //    deps.AddRange(field.DefaultValueDeps);
            //}

            //if (methodName == "listItems")
            //{
            //    deps.AddRange((field.Editor as SelectEditor).ListItemsDeps);
            //}

            //var formValues = GetFormValues(reportId, dataSet.ParamsDataSet, deps);
            //var jresult = WebReportsClient.CallMethod(reportId, itemId, methodName, formValues);
            //ConvertDatesToLocalTime(jresult);
            //VDataTable dt = (VDataTable)dataSet.Tables[0];
            //dt.BeginLoadData();
            //dt.Rows.Clear();


            //if (field.Editor is SelectEditor && methodName == "listItems")
            //{

            //    var editor = field.Editor as SelectEditor;
            //    var allCols = editor.AllColumns();

            //    foreach (JObject jrow in ((JArray)jresult))
            //    {
            //        var valList = new List<object>();
            //        foreach (var colName in allCols)
            //        {
            //            valList.Add(jrow[colName]);
            //        }
            //        dt.Rows.Add(valList.ToArray());
            //    }

            //}
            //else if (field.Editor is SelectEditor && methodName == "defaultValue")
            //{
            //    foreach (JObject jrow in ((JArray)jresult))
            //    {
            //        dt.Rows.Add(jrow.Properties().ElementAt(0).Value, jrow.Properties().ElementAt(1).Value);
            //    }
            //}
            //else
            //{
            //    dt.Rows.Add(jresult);

            //}
            //dt.EndLoadData();
            //dataSet.RaiseSchemeChanged();
        }

        public static void PrepareDataSet(VDataSet dataSet, string queryName, VDataSet paramsDataSet)
        {
            dataSet.DataSetName = queryName;
            dataSet.CustomRefresh = RefreshDatasetMethod;
            dataSet.ParamsDataSet = paramsDataSet;
        }



        public static string GetDisplayValue(object value, VDataSet defaultDataSet)
        {

            var table = defaultDataSet.Tables[0];
            if (table.Columns.Count < 2)
            {
                return null;
            }
            foreach (DataRow row in table.Rows)
            {
                if (value.Equals(row[0]))
                {
                    return row[1]!=null ? row[1].ToString() : null;
                }
            }
            return null;

        }


    }
}
