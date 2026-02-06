using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using Npgsql;
using sql.builder;
using sql.builder.Clean;
using sql.builder.Clean.Extensions;
using sql.builder.DataApi;

namespace SqlBuilderLib.DevTools
{
    public static partial class TsBuilder
    {
        private static string ProcessReport(VUseReport useReport)
        {
            if (useReport.P_Visible == "0" || useReport.P_Report.Contains("journal"))
            {
                return null;
            }
            var repFullName = $"{useReport.P_Project}.{useReport.P_Report}";
            var form = VSXElement.Get(CleanSqlBuilder.GetFormConfig(repFullName)) as VForm;

            var rep = new CleanExpressReport();
            rep.OpenDocumentAfterPrint = false;
            rep.Initialize(repFullName);

            var fields = rep.GetParamFields().ToArray();

            foreach (var field in fields)
            {
                field.Control.PrepareListSource();
                field.Control.PrepareDefaultSource();
            }
            ProcessForm(form);



            return null;

        }


    }
}
