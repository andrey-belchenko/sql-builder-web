//using System.Collections.Generic;
//using System.Linq;

//using FlexCel.Core;
//using FlexCel.XlsAdapter;

//namespace sql.builder
//{
//    internal static class FelxCelHelper
//    {
//        static double ROWS_AUTOFIT_ADJUSTMENT = 1.5D;
//        static double COlS_AUTOFIT_ADJUSTMENT = 1.0D;

//        public static TCellAddress FindFirstCellWithText(this XlsFile excel, string text)
//        {
//            return excel.Find(text, null, null, false, false, false, false);
//        }

//        public static TCellAddress[] FindAllCellsWithText(this XlsFile excel, string text)
//        {
//            var cells = new List<TCellAddress>();
//            TCellAddress cell = null; //initialize the value to null to start searching.
//            do
//            {
//                cell = excel.Find(text, null, cell, false, false, false, false); //find the next value.
//                if (cell != null) cells.Add(cell);
//            }
//            while (cell != null); //until it doesn't find any more matches.

//            return cells.ToArray();
//        }

//        public static void DeleteColumn(this XlsFile excel, int col)
//        {
//            excel.DeleteRange(new TXlsCellRange(1, col, 1, col), TFlxInsertMode.ShiftColRight); ;
//        }

//        public static void AutoFitRow(this XlsFile excel, int row)
//        {
//            int minRow;
//            int maxRow;
//            if (row == 0)
//            {
//                minRow = 1;
//                maxRow = excel.RowCount;
//            }
//            else
//            {
//                minRow = row;
//                maxRow = row;
//            }

//            // третий параметр true - иначе некоторые строки пропускает :\
//            excel.AutofitRow(minRow, maxRow, true, false, ROWS_AUTOFIT_ADJUSTMENT);
//        }
//        public static void AutoFitAllColumns(this XlsFile excel)
//        {
//            excel.AutofitCol(1, excel.ColCount, false, COlS_AUTOFIT_ADJUSTMENT);
//        }

//        public static void AutoFitAllRows(this XlsFile excel)
//        {
//            excel.AutoFitRow(0);
//        }
//    }
//}