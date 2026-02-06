
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;
using sql.builder;
using sql.builder.DataApi;
using sql.builder.UI;


namespace SqlBuilderLib.DevTools
{

    public static partial class TsBuilder
    {
        private static string ProcessFieldGenTs(VForm form, VField field, IEnumerable<FieldProps> fieldsProps)
        {

            var fieldName = field.P_Field;
            if (!string.IsNullOrEmpty(fieldName))
            {
                var fieldFileName = $"field_{ClearName(fieldName)}.ts";
            }





            return null;

        }
    }
}


