//using System;
//using System.Runtime.InteropServices;
//using Microsoft.Office.Interop.Excel;
//using Microsoft.TeamFoundation.Client.CommandLine;

//namespace sql.builder.MP
//{
//    public class ExcelMSEnvironment : IExcelEnvironment
//    {
//        Application _excel;
//        Worksheet _ws;
//        public object GetCellValue(string sheetName, int rowIndex, int columnIndex)
//        {
//            var val =
//                ((_excel.ActiveWorkbook.Sheets[sheetName] as Worksheet).Cells[rowIndex, columnIndex] as Range).Value;

//            return val;
//        }
//        public void LoadFile(string filePath)
//        {
//            _excel = new Application();
//            _excel.Workbooks.Open(filePath);
            
//        }

//        public void SetCurrentSheet(string sheetName)
//        {
//            if (sheetName == null)
//            {
//                _ws = _excel.ActiveWorkbook.Worksheets[1] as Worksheet;
//            }
//            else
//            {
//                _ws = _excel.ActiveWorkbook.Worksheets[sheetName] as Worksheet;
//            }

//        }

//        public int GetRowsCount()
//        {
//            return _ws.UsedRange.Rows.Count;
//        }

//        public int GetColumnsCount()
//        {
//            return _ws.UsedRange.Columns.Count;
//        }

//        public ExcelValue[][] GetRows(int firstRowIndex, int lastRowIndex,int firstColumnIndex)
//        {
           
//            int lastColumnIndex = _ws.UsedRange.Columns.Count + _ws.UsedRange.Column - 1;
//            int columnsCount = lastColumnIndex - firstColumnIndex + 1;

//            Range cell1 = _ws.Cells[firstRowIndex, firstColumnIndex];
//            Range cell2 = _ws.Cells[lastRowIndex, lastColumnIndex];
//            var range = _ws.Range[cell1, cell2];
//            var values = (object[,])range.Value2;

//            int rowsCount = lastRowIndex - firstRowIndex + 1;
//            ExcelValue[][] excelValues = new ExcelValue[rowsCount][];
           
//            for (int i = 0; i < rowsCount; i++)
//            {
//                excelValues[i] = new ExcelValue[columnsCount];
//                for (int j = 0; j < columnsCount; j++)
//                {
                    
//                    excelValues[i][j] = new ExcelValue(null, values[i + 1, j + 1]);
//                }
//            }

//            return excelValues;
//        }

//        public void Dispose()
//        {
//            if (_excel == null) return;

//            try
//            {
//                _excel.ActiveWorkbook.Close(false);
//                _excel.DisplayAlerts = false;
//                _excel.Visible = false;
//                _excel.Quit();
//            }
//            finally
//            {
//                foreach (Workbook workbook in _excel.Workbooks)
//                {
//                    foreach (Worksheet worksheet in workbook.Worksheets)
//                    {
//                        Marshal.ReleaseComObject(worksheet);
//                        Marshal.FinalReleaseComObject(worksheet);
//                    }

//                    Marshal.ReleaseComObject(workbook.Worksheets);
//                    Marshal.FinalReleaseComObject(workbook.Worksheets);

//                    Marshal.ReleaseComObject(workbook);
//                    Marshal.FinalReleaseComObject(workbook);
//                }

//                Marshal.ReleaseComObject(_excel.Workbooks);
//                Marshal.FinalReleaseComObject(_excel.Workbooks);

//                Marshal.ReleaseComObject(_excel);
//                Marshal.FinalReleaseComObject(_excel);

//                _excel = null;
//                _ws = null;

//                GC.Collect();
//                GC.WaitForPendingFinalizers();
//            }
//        }





        
//    }
//}