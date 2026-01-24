using System.Linq;


namespace sql.builder.MP
{
    public class MPTableFiller: IMPTableFiller
    {
        public string SrcTableName { get; set; }
        public MPColumn[] Columns { get; set; }

        public MPTableFiller(string srcTableName, MPColumn[] columns)
        {
            SrcTableName = srcTableName;
            Columns = columns;
        }

        public bool FillTable(string tableName)
        {
            var colNames = Columns.Where(c => c.IsMain).Select(c => c.Name).ToArray();
            return MPDB.FillData(SrcTableName, tableName, colNames);
        }
    }
}