using System.Collections.Generic;
using System.Linq;

namespace sql.builder.MP
{
    public class MPRow
    {
        public string ID { get; private set; }

        MPColumn[] _columns;

        MPCell[] _cells;
        Dictionary<string, MPCell> _cellsByColName;
        Dictionary<MPColumn, MPCell> _cellsByCol;

        public MPRow(MPColumn[] columns, string id)
        {
            ID = id;

            _columns = columns;

            _cellsByCol = new Dictionary<MPColumn, MPCell>(_columns.Length);
            _cellsByColName = new Dictionary<string, MPCell>(_columns.Length);

            _cells = new MPCell[columns.Length];
            for (var i = 0; i < columns.Length; i++)
            {
                _cells[i] = new MPCell(columns[i], this);
                _cellsByCol.Add(columns[i], _cells[i]);
                _cellsByColName.Add(columns[i].Name, _cells[i]);
            }
        }

        public IEnumerable<MPCell> GetCells()
        {
            return _cells.AsEnumerable();
        }

        public MPCell GetCell(int index)
        {
            return _cells[index];
        }

        public MPCell GetCell(MPColumn column)
        {
            return _cellsByCol[column];
        }

        public MPCell GetCell(string columnName)
        {
            return _cellsByColName[columnName];
        }
    }
}
