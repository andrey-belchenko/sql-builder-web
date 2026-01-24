using System.Collections.Generic;

namespace sql.builder.MP
{
    public interface IMPDataReader
    {
        IEnumerable<MPRow> GetCurrentRows();
        IEnumerable<MPColumn> GetColumns();

        bool ReadNext(int rowsCount);
        bool SkipNext(int rowsCount);

        void Reset();
    }
}
