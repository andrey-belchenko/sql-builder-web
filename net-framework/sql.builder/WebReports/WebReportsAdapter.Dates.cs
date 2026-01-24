
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



    
        public static void ConvertDatesToLocalTime(JToken token)
        {
            if (token is JObject)
            {
                // Iterate through the properties of the JObject
                foreach (var property in (token as JObject).Properties())
                {
                    ConvertDatesToLocalTime(property.Value);
                }
            }
            else if (token is JArray)
            {
                // Iterate through the elements of the JArray
                for (int i = 0; i < (token as JArray).Count; i++)
                {
                    ConvertDatesToLocalTime((token as JArray)[i]);
                }
            }
            else if (token is JValue && (token as JValue ).Type == JTokenType.Date)
            {
                // Handle standalone JValue dates
                if ((token as JValue ).Value is DateTime)
                {
                    (token as JValue).Value = ((DateTime)((token as JValue).Value)).ToLocalTime();
                }
                else if ((token as JValue ).Value is DateTimeOffset)
                {
                    (token as JValue).Value = ((DateTimeOffset)((token as JValue).Value)).ToLocalTime();
                }
            }
        }

        public static void ConvertDatesToUtc(JToken token)
        {
            if (token is JObject)
            {
                // Iterate through the properties of the JObject
                foreach (var property in (token as JObject).Properties())
                {
                    ConvertDatesToUtc(property.Value);
                }
            }
            else if (token is JArray)
            {
                // Iterate through the elements of the JArray
                for (int i = 0; i <  (token as JArray).Count; i++)
                {
                    ConvertDatesToUtc((token as JArray)[i]);
                }
            }
            else if (token is JValue && (token as JValue).Type == JTokenType.Date)
            {
                // Handle standalone JValue dates
                if ((token as JValue).Value is DateTime )
                {
                    (token as JValue).Value = ((DateTime)((token as JValue).Value)).ToUniversalTime();
                }
                else if ((token as JValue).Value is DateTimeOffset )
                {
                    (token as JValue).Value = ((DateTimeOffset)((token as JValue).Value)).ToUniversalTime();
                }
            }
        }
    }
}
