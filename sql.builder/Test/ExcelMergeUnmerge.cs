//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using Excel = Microsoft.Office.Interop.Excel;



//namespace sql.builder.Test
//{
//    internal class ExcelDocumentMM
//    {
//        private Excel.Application _application = null;
//        private Excel.Workbook _workBook = null;
//        private Excel.Worksheet _workSheet = null;
//        private object _missingObj = System.Reflection.Missing.Value;

//        //КОНСТРУКТОР
//        public ExcelDocumentMM()
//        {
//            _application = new Microsoft.Office.Interop.Excel.Application();
//            _workBook = _application.Workbooks.Add(_missingObj);
//            _workSheet = (Excel.Worksheet)_workBook.Worksheets.get_Item(1);
//        }

//        public ExcelDocumentMM(string pathToTemplate)
//        {
//            object pathToTemplateObj = pathToTemplate;

//            _application = new Microsoft.Office.Interop.Excel.Application();
//            _workBook = _application.Workbooks.Add(pathToTemplateObj);
//            _workSheet = (Excel.Worksheet)_workBook.Worksheets.get_Item(1);
//        }

//        // ВИДИМОСТЬ ДОКУМЕНТА
//        public bool Visible
//        {
//            get
//            {
//                return _application.Visible;
//            }
//            set
//            {
//                _application.Visible = value;
//            }
//        }


//        public Excel.Range GetRange(int row1, int col1, int row2, int col2)
//        {
//            return (Excel.Range)_workSheet.Range[
//                _workSheet.Cells[row1, col1],
//                  _workSheet.Cells[row2, col2]
//                  ];
//        }
//        public void UnMerge()
//        {
//            int i = 0;
//           // var cc = _workSheet.UsedRange.Cells;
//            var rr = GetRange(2, 2, 3, 3);
//            var clc = _workSheet.UsedRange.Cells.Count;
//            foreach (Excel.Range c1 in _workSheet.UsedRange.Cells)
//            {
//                Excel.Range ma = c1.MergeArea;
//                var rc = ma.Rows.Count;
//                if (rc > 1)
//                {
//                    var c = ma.Column;
//                    var r1 = ma.Row;

//                    var cc = ma.Columns.Count;
                  

//                    c1.MergeArea.UnMerge();



//                    foreach (Excel.Range r in ma.Rows)
//                    {
//                        var v = r.Cells[1].Value;
//                        r.Clear();
//                        r.Merge();
//                        r.Value = v;
//                    }

//                    //ma.Value = c1.MergeArea.Cells[1].Value;
//                }
//                i++;
//            }

            
//        }


//        public void Merge()
//        {
//            foreach (Excel.Range col in _workSheet.UsedRange.Columns)
//            {
//                string prevValue = null;
//                var mergeFromRow = 0;
//                var mergeFromCol = 0;
     
//                var colsCountToMerge = 1;
//                var r = 1;
//                bool merged = false;
//                bool prevRowMerged = false;
//                int prevColMergeBegRowPrev = 0;
//                int shift=-1;
//                foreach (Excel.Range cell in col.Cells)
//                {
//                    cell.Select();
//                    if (cell.MergeArea.Rows.Count > 1 && cell.MergeArea.Columns.Count==1)// служебная колонка 
//                    {
//                        continue;
//                    }
//                    string value = GetRange(cell.Row, cell.Column, cell.Row, cell.Column).Value;
//                    if (value == "[месяц]")
//                    {

//                    }
//                    else
//                    {

//                    }
//                    int prevColMergeBegRow = 0;
//                    if (cell.Column != 1)
//                    {
//                        prevColMergeBegRow = GetRange(cell.Row,cell.Column-1,cell.Row,cell.Column-1).MergeArea.Row;
//                    }

//                    if (cell.Column != cell.MergeArea.Column)
//                    {
//                        merged = true;
//                    }
//                    else
//                    {
//                        merged = false;
//                    }

//                    if (r == col.Cells.Count)
//                    {
//                        shift = 0;
//                    }
//                    bool beg = false;
//                    if (merged || prevRowMerged)
//                    {
//                        beg = true;
//                    }
//                    if (prevValue != value || shift==0 || prevColMergeBegRow!=prevColMergeBegRowPrev)
//                    {
//                        if (mergeFromRow > 0  && !prevRowMerged)
//                        {
//                            var rangeToMerge = GetRange(mergeFromRow, mergeFromCol, r + shift, mergeFromCol + colsCountToMerge - 1);
                            
//                            rangeToMerge.UnMerge();
//                            rangeToMerge.Clear();
//                            rangeToMerge.Merge();
//                            rangeToMerge.Value = prevValue;
//                        }
//                        beg = true;
//                    }

//                    if (beg)
//                    {
//                        mergeFromRow = r;
//                        mergeFromCol = cell.Column;
//                        prevValue = value;
//                       // colsCountToMerge = 1;
//                        colsCountToMerge = cell.MergeArea.Columns.Count;
//                        shift = -1;
//                    }

//                    prevRowMerged = merged;

//                    //var mergedColsCount = cell.MergeArea.Columns.Count;
//                    //if (mergedColsCount > colsCountToMerge)
//                    //{
                      
//                    //}
//                    prevColMergeBegRowPrev = prevColMergeBegRow;
                   
//                    r++;
//                }
//            }
//        }

//        int[] arr1_n_row = new int[10000];
//        int[] arr2_n_col = new int[10000];
//        int[] arr1_f_row = new int[10000];
//        String[] array_m = new String[1000];
//        int[] arr2_f_col = new int[10000];
//            public void Merge_2()
//        {

//           // var cc = _workSheet.UsedRange.Cells;
//            int width = 0;
//            int length = 0;
//           String ma_1 = null;
//          String ma_2 = null; 
//            int i = 1;
//            String ma_3 = null;

//            arr1_n_row[1] = 0;
//            arr2_n_col[1] = 0;


//            int z = 0;
//            int sch_row = 0;
//            foreach (Excel.Range c1 in _workSheet.UsedRange.Rows)
//            {
         

//                while (c1.Columns[i]!=null)
//                {


//                    sch_row++;
                  
//                    ma_1 = c1.Columns[i].Value;
//                    ma_2 = c1.Columns[i + 1].Value;
//                    if (ma_1 == ma_2&&( array_m[i]==ma_1))
//                        i++;
//                    else
//                    {

//                        arr1_f_row[z] = sch_row; arr1_n_row[sch_row + 1] = sch_row + 1;  //Строки
//                        arr2_f_col[z] = i + 1; z++; arr2_n_col[z] = i + 2; ma_3 = ma_1; // Колонки

//                    }
//                    array_m[i] = ma_1; //Сохраняем значения по колонкам  одной строчке
//                }
               
           
              
        
//            }
//            for (i = 0; arr1_f_row[i] != null; i++)
//            {
//                for (int d = 0; arr1_n_row[d] != null; d++)
//                    GetRange(arr1_n_row[i], arr2_n_col[d], arr1_f_row[i], arr2_f_col[d]).MergeArea.Merge();
            
//            }
               
//        }

     


//        // ВСТАВКА ЗНАЧЕНИЯ В ЯЧЕЙКУ
//        public void SetCellValue(string cellValue, int rowIndex, int columnIndex)
//        {
//            _workSheet.Cells[rowIndex, columnIndex] = cellValue;
//            Excel.Range _excelCells = (Excel.Range)_workSheet.get_Range("B1", "B10").Cells;

//            // Производим объединение
//            _excelCells.Merge(Type.Missing);
//        }

//        public void Close()
//        {
//            _workBook.Close(false, _missingObj, _missingObj);

//            _application.Quit();

//            System.Runtime.InteropServices.Marshal.ReleaseComObject(_application);

//            _application = null;
//            _workBook = null;
//            _workSheet = null;

//            System.GC.Collect();
//        }
//    }
//}
