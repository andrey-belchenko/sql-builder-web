using System;



namespace sql.builder.MP.Tools
{
    public static class ExcelValueToNetType
    {
        public static object Convert(ExcelValue excelValue, MPType type)
        {
            if (excelValue.Value == null) return null;

            var stype = type as MPStringType;
            if (stype != null) return ExcelValueToString(excelValue, stype);

            var ntype = type as MPNumberType;
            if (ntype != null) return ExcelValueToNumber(excelValue, ntype);

            var dtype = type as MPDateType;
            if (dtype != null) return ExcelValueToDate(excelValue, dtype);

            var dttype = type as MPDateTimeType;
            if (dttype != null) return ExcelValueToDateTime(excelValue, dttype);

            throw new NotImplementedException();
        }

        private static string ExcelValueToString(ExcelValue excelValue, MPStringType type)
        {
            var sval = excelValue.Value.ToString();
            if (sval.Length > type.MaxLength) sval = sval.Substring(0, type.MaxLength);

            return sval;
        }

        private static decimal ExcelValueToNumber(ExcelValue excelValue, MPNumberType type)
        {
            var nval = System.Convert.ToDecimal(excelValue.Value);
            if(type.DecimalPlacesCount.HasValue)
            {
                nval = Math.Round(nval, type.DecimalPlacesCount.Value);
            }

            return nval;
        }

        private static DateTime ExcelValueToDate(ExcelValue excelValue, MPDateType type)
        {
            DateTime dval = DateTime.MinValue;
            if (excelValue.Value is double)
            {
                dval = DateTime.FromOADate((double)excelValue.Value);
            }
            else 
            {
                dval =  DateTime.Parse(excelValue.Value.ToString());
            }

            return dval.Date;
        }

        private static DateTime ExcelValueToDateTime(ExcelValue excelValue, MPDateTimeType type)
        {
            DateTime dval = DateTime.MinValue;
            if (excelValue.Value is double)
            {
                dval = DateTime.FromOADate((double)excelValue.Value);
            }
            else
            {
                dval = DateTime.Parse(excelValue.Value.ToString());
            }

            return dval;
        }
    }
}