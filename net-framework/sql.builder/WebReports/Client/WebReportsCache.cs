using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql.builder.WebReports.Client
{
    internal static class WebReportsCache
    {
        private static Dictionary<string, Dictionary<string, JToken>> Data = new Dictionary<string, Dictionary<string, JToken>>();


        public static void Remove(string reportId)
        {
            if (Data.ContainsKey(reportId))
            {
                Data.Remove(reportId);
            }
        }

        public static void Add(string reportId, string key, JToken item, string pars = null)
        {
            if (reportId == null)
            {
                return;
            }
            var fullKey = key+pars;
            if (!Data.ContainsKey(reportId))
            {
                Data.Add(reportId, new Dictionary<string, JToken>());
            }
            Data[reportId][fullKey] = item;
        }

        public static JToken Get(string reportId, string key, string pars = null)
        {

            if (reportId == null)
            {
                return null;
            }
            if (!Data.ContainsKey(reportId))
            {
                return null;
            }
            var fullKey = key+pars;
            if (!Data[reportId].ContainsKey(fullKey))
            {
                return null;
            }
            return Data[reportId][fullKey];
        }




    }
}
