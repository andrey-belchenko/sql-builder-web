using System;
using System.Globalization;


namespace sql.builder.MP.Tools
{
    public static class MPCellToString
    {
        public static string Convert(MPCell cell)
        {
            if (cell.GetValue() == null) return "";

            var stringType = cell.Column.Type as MPStringType; 
            if(stringType != null) return AsString(cell, stringType);

            var numberType = cell.Column.Type as MPNumberType; 
            if(numberType != null) return AsNumberString(cell, numberType);

            var dateType = cell.Column.Type as MPDateType;
            if (dateType != null) return AsDateString(cell, dateType);

            var dateTimeType = cell.Column.Type as MPDateTimeType;
            if (dateTimeType != null) return AsDateTimeString(cell, dateTimeType);

            throw new NotImplementedException();
        }

        private static string AsNumberString(MPCell cell, MPNumberType type)
        {
            // todo: обработать ситуацию если тип не decimal - если понадобится
            decimal nval = (decimal)cell.GetValue();

            string format = "F";
            if (type.DecimalPlacesCount.HasValue)
            {
                format = format + type.DecimalPlacesCount.Value;
            }
            return nval.ToString(format, CultureInfo.GetCultureInfo("en-US"));
        }

        private static string AsDateString(MPCell cell, MPDateType type)
        {
            // todo: обработать ситуацию если тип не DateTime - если понадобится
            DateTime dval = (DateTime)cell.GetValue();
            return dval.ToString("dd.MM.yyyy");
        }

        private static string AsDateTimeString(MPCell cell, MPDateTimeType type)
        {
            // todo: обработать ситуацию если тип не DateTime - если понадобится
            DateTime dval = (DateTime)cell.GetValue();
            return dval.ToString("dd.MM.yyyy hh24:mm:ss");
        }

        private static string AsString(MPCell cell, MPStringType type)
        {
            string sval = cell.GetValue().ToString().Replace("\"", "\"\"");
            if (sval.Length > type.MaxLength) sval = sval.Substring(0, type.MaxLength);
            return string.Format("\"{0}\"", sval);
        }
    }
}