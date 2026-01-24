
//using DevExpress.Utils.Extensions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using sql.builder.Controls.Containers;
using sql.builder.DataApi;
using sql.builder.WebReports.Client;
using sql.builder.XmlHelpers;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics.Contracts;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Linq;
using sql.builder;
using sql.builder.UI;
using sql.builder.Controls;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Tab;

namespace sql.builder.WebReports
{
    internal static partial class WebReportsAdapter
    {

        private static JObject GetFormValues(string reportId, VDataSet paramsDs, IEnumerable<string> fieldNames = null)
        {
            
            var paramsTable = paramsDs.ParamsTable;
            var paramsRow = paramsTable.Rows[0];
            var form = WebReportsClient.GetForm(reportId);
            var jparams = new JObject();

            foreach (var field in form.Fields)
            {
                if (fieldNames != null)
                {
                    if (!fieldNames.Contains(field.Name))
                    {
                        continue;
                    }
                }
                if (field.Editor is SelectEditor && !(field.Editor as SelectEditor).SingleSelection)
                {
                    var valueTable = paramsDs.GetTable(field.Name);
                    var value = new JArray();
                    foreach (DataRow row in valueTable.Rows)
                    {
                        value.Add(new JValue(row["value"]));
                    }
                    jparams[field.Name] = value;
                }
                else
                {
                    jparams[field.Name] = new JValue(paramsRow[field.Name]);
                }

            }
            ConvertDatesToUtc(jparams);
            return jparams;
        }


        private static JObject GetFormText(string reportId, UIFormC formC)
        {
            var buffer = new VDataSet();
            ucMainReports.CopyParsToResult(formC, buffer);
            var paramsTable = buffer.Tables[0];
            var paramsRow = paramsTable.Rows[0];
            var form = WebReportsClient.GetForm(reportId);
            var jvalues = new JObject();

            foreach (var field in form.Fields)
            {
                jvalues[field.Name] = new JValue(paramsRow[field.Name.ToUpper()+"_TEXT"]);
            }
            return jvalues;
        }









    }
}
