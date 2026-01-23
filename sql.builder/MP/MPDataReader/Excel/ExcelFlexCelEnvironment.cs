//using FlexCel.Core;
//using FlexCel.XlsAdapter;

//namespace sql.builder.MP
//{
//    /// <summary>
//    /// ультра медленно на больших объемах
//    /// </summary>
//    public class ExcelFlexCelEnvironment : IExcelEnvironment
//    {
//        XlsFile _file;

//        public void LoadFile(string filePath)
//        {
//            _file = new XlsFile(filePath);
//        }

//        public int GetRowsCount()
//        {
//            return _file.RowCount;
//        }

//        public ExcelValue[][] GetRows(int firstRowIndex, int lastRowIndex,int firstColumnIndex)
//        {
//            int rowsCount = lastRowIndex - firstRowIndex + 1;
//            ExcelValue[][] excelValues = new ExcelValue[rowsCount][];
//            for (int row = firstRowIndex; row <= lastRowIndex; row++)
//            {
//                excelValues[row - firstRowIndex] = new ExcelValue[_file.ColCountOnlyData];
//                for (int col = 1; col <= _file.ColCountOnlyData; col++)
//                {
//                    object val = _file.GetCellValue(row, col);
//                    TFlxFormat format = _file.GetFormat(_file.GetCellFormat(row, col));
//                    ExcelValue excelValue = new ExcelValue(format.Format, val);
//                    excelValues[row - firstRowIndex][col - 1] = excelValue;
//                }
//            }

//            return excelValues;
//        }

//        public void Dispose()
//        {
//            _file = null;
//        }


//        public void SetCurrentSheet(string sheetName)
//        {
//            throw new System.NotImplementedException();
//        }


//        public object GetCellValue(string sheetName, int rowIndex, int columnIndex)
//        {
//            throw new System.NotImplementedException();
//        }
//    }
//}