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
            var repFullName = $"{useReport.P_Project}.{useReport.P_Report}";
            var form = CleanSqlBuilder.GetFormConfig(repFullName);
            var formName = form.GetAttributeValue(TextConst.AName.Name);
            return $"";
        }


    }
}
