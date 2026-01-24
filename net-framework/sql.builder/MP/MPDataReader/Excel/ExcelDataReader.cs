using System.Collections.Generic;
using System.Linq;
using  sql.builder.MP.Tools;
namespace sql.builder.MP
{
    public class ExcelDataReader : IMPDataReader
    {
        public IExcelEnvironment Env { get; private set; }
        public string FilePath { get; private set; }

        MPColumn[] _columns;
        MPRow[] _currentRows;

        int _lastReadRowIndex;

         int _firstDataRowIndex;
         int _firstDataColumnIndex;
         int _rowsCount;

        public ExcelDataReader(IExcelEnvironment env, string filePath)
        {
            Env = env;
            FilePath = filePath;

            
            Env.LoadFile(filePath);

            
        }

        public object GetCellValue(string sheetName, int rowIndex, int columnIndex)
        {
            return Env.GetCellValue(sheetName, rowIndex, columnIndex);
        }

        public void SetOptions(string sheetName,MPColumn[] columns, int firstDataRowIndex , int firstDataColumnIndex )
        {
           Env.SetCurrentSheet(sheetName);
            _columns = columns;
            _firstDataRowIndex = firstDataRowIndex;

            _firstDataColumnIndex = firstDataColumnIndex;
            _rowsCount = Env.GetRowsCount();
            
            Reset();
        }



        public IEnumerable<MPRow> GetCurrentRows()
        {
            return _currentRows.AsEnumerable();
        }

        public IEnumerable<MPColumn> GetColumns()
        {
            return _columns.AsEnumerable();
        }

        public bool ReadNext(int rowsCount)
        {
            int curRowIndex = _lastReadRowIndex + 1;

            if (!TryMoveCursor(rowsCount))
            {
                _currentRows = new MPRow[0];
                return false;
            }

            ExcelValue[][] rowsValues = Env.GetRows(curRowIndex, _lastReadRowIndex,_firstDataColumnIndex);

            _currentRows = new MPRow[_lastReadRowIndex - curRowIndex + 1];

            int rowID = curRowIndex;
            for (var i = 0; i < rowsValues.Length; i++)
            {
                _currentRows[i] = new MPRow(_columns, rowID.ToString());
                for (int j = 0; j < _columns.Length ; j++)
                {
                    object value = ExcelValueToNetType.Convert(rowsValues[i][j], _columns[j].Type);
                    _currentRows[i].GetCell(_columns[j]).SetValue(value);
                }

                rowID++;
            }

            return true;
        }

        public bool SkipNext(int rowsCount)
        {
            _currentRows = new MPRow[0];
            return TryMoveCursor(rowsCount);
        }

        private bool TryMoveCursor(int rowsCount)
        {
            if (_lastReadRowIndex < _rowsCount)
            {
                _lastReadRowIndex += rowsCount;

                if (_lastReadRowIndex > _rowsCount)
                {
                    _lastReadRowIndex = _rowsCount;
                }

                return true;
            }
            else
            {
                return false;
            }
        }

        public void Reset()
        {
            _lastReadRowIndex = _firstDataRowIndex - 1;
            _currentRows = new MPRow[0];
        }
    }
}