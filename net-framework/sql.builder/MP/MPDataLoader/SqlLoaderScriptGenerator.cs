using System.Text;
using sql.builder.MP.Tools;

namespace sql.builder.MP
{
    public static class SqlLoaderScriptGenerator
    {
        public static string Generate(string tableName, MPColumn[] columns, string dataFilePath, string badFilePath, string discardFilePath)
        {
            var sb = new StringBuilder();
            sb.AppendLine("OPTIONS");
            sb.AppendLine("(");
            sb.AppendLine("\tDIRECT = true");
            sb.AppendLine(")");
            sb.AppendLine("LOAD DATA");
            sb.AppendLine("CHARACTERSET CL8MSWIN1251");
            sb.AppendFormat(@"INFILE ""{0}"" ""STR '{1}'""", dataFilePath, MPEnvironment.RowsDelimiter);
            sb.AppendLine();
            sb.AppendFormat(@"BADFILE ""{0}""", badFilePath);
            sb.AppendLine();
            sb.AppendFormat(@"DISCARDFILE ""{0}""", discardFilePath);
            sb.AppendLine();
            sb.AppendFormat(@"INTO TABLE {0}", tableName);
            sb.AppendLine();
           // sb.AppendLine("TRUNCATE");
            sb.AppendFormat(@"FIELDS TERMINATED BY '{0}' OPTIONALLY ENCLOSED BY '""'", MPEnvironment.FieldsDelimiter);
            sb.AppendLine();
            sb.AppendLine("TRAILING NULLCOLS");
            sb.AppendLine("(");
            bool first = true;
            foreach (MPColumn column in columns)
            {
                if(first) first = false;
                else sb.AppendLine(",");

                sb.AppendFormat("\t{0}", MPColumnToSqlLoaderDesc.Convert(column));
            }
            sb.AppendLine();
            sb.AppendLine(")");

            return sb.ToString();
        }
    }
}