

namespace sql.builder.MP
{
    public interface IMPDataLoader
    {
        bool FillTable(string tableName, MPColumn[] columns);
    }
}