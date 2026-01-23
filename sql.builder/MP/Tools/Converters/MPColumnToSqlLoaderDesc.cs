

namespace sql.builder.MP.Tools
{
    public static class MPColumnToSqlLoaderDesc
    {
        public static string Convert(MPColumn column)
        {
            var stype = column.Type as MPStringType;
            if(stype != null)
            {
                return string.Format("{0} char({1})", column.Name, stype.MaxLength);
            }

            var dtype = column.Type as MPDateType;
            if (dtype != null)
            {
                return column.Name + " date \"dd.mm.yyyy\"";
            }

            return column.Name;
        }
    }
}