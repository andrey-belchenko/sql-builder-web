//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Diagnostics;
//using System.IO;
//using System.Globalization;
//using System.Linq;
//using System.Reflection;
//using System.Text;
//using System.Threading.Tasks;
////using System.Windows.Forms;
////using DevExpress.XtraEditors;
//using infoenergo.core.Data;
//using infoenergo.Excel;
//using infoenergo.sys;
//using infoenergo.sys.YM;
//using infoenergo.ui.win;
////using Excel = Microsoft.Office.Interop.Excel;
//using FlexCel.Core;
//using FlexCel.XlsAdapter;
//using sql.builder;


//namespace sql.builder.Special.Report
//{
//     static class VerticalRep
//    {
//      // -- Формирование отчета № 58 (для Рязани) 
//      // -- Пока что все в одном файле. При добавлении работы еще с другим отчетом нужно: 
//      // -- CreateReport оставить название (оно используется в билдере при вызове action) и добавить параметр определения отчета
//      // -- вынести отдельно формирование конкретного отчета и создать helper для пока приватных функций

//         //-- нужно еще допилить: вывести повтор кода в отделые процедуры с умом  i+ i- есть тонкости

//      /// <summary>
//      /// 
//      /// </summary>
//      /// <param name="p_ym_beg"></param>
//      /// <param name="p_ym_end"></param>
//      /// <returns></returns>
//         #region CreateReport
//         public static bool? CreateReport(decimal p_ym_beg, decimal p_ym_end)
//        {
//            ExcelHelper xls = null;
//            string sFileRepName = "Фактическая покупка-продажа ООО 'РГМЭК' электрической энергии и мощности.xlsx";
//            // --- получаем путь из интерфейса для сохранения отчета
//            string sRepFullPathNew = Path.Combine(sql.builder.SqlBuilder.GetWorkFolderPath(), sFileRepName);
             
//            try
//            {
//                string s_err = null;
//                string sPerOrFromTo;    // период отчета (за период или с... по)

//                int iHeaderRowStart = 3; //старт начала заголовка               
//                int iHeaderColStart = 6;
//                int iDataRowStart = 12; // номер строки начала данных в таблице
//                int iRowStop = 138; // номер строки окончания ранга для копирования
//                int iColCount = 0;
//                string sTemplateFileName = "72589.xlsx";
//                int iCountZoneGroup = 0;
//                int iCountGPGroup = 0;
//                FlexCel.Core.TFormula p;   // формула 
//                string strP = string.Empty;
                
//                FlexCel.Core.TXlsCellRange range = null;
//                string templatePath = Path.Combine(Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location), "sql.builder", "printTemplate", "excel", sTemplateFileName);
             
//                if (!File.Exists(templatePath))
//                {
//                    s_err = "Файл шаблона '" + templatePath + "' не найден";
//                    XtraMessageBox.Show(s_err, "Ошибка", MessageBoxButtons.OK);
//                    return false;
//                }
//                xls = new ExcelHelper();
//                xls.Open(templatePath);

//                 //-------------------
//                // ----------------- Получаем данные для заполнения
//                string sYmBeg = p_ym_beg.ToString().Replace(",",".");
//                string sYmEnd = p_ym_end.ToString().Replace(",", ".");

//                string sQuery = @"SELECT * from table(ng_rep_other.get_tbl_ats_data(" + sYmBeg + ", " + sYmEnd + "))  where gtp_gp_name <> '-' order by kod_price_zone asc, gtp_gp_name, date_admission";
//                DataTable dtRes = DataHelper.SqlGetTable(sQuery, Global.Connection);

//                string sQuery2 = @"SELECT * from table(ng_rep_other.get_tbl_ats_data(" + sYmBeg + ", " + sYmEnd + "))  where gtp_gp_name = '-' ";
//                DataTable dtRes2 = DataHelper.SqlGetTable(sQuery2, Global.Connection);


//                //------------------------ Заполняем данными шаблон
//                //----- Заполняем title отчета. Лист 1
//                infoenergo.sys.YM.YM ym = new infoenergo.sys.YM.YM((decimal)(p_ym_beg));
//                DateTime  dateYm = ym.FirstDay();
//                string  strPerPeriod = dateYm.ToString("MMMM ", CultureInfo.CreateSpecificCulture("ru-RU")) + dateYm.Year;

//                sPerOrFromTo = p_ym_beg == p_ym_end ? "за " + strPerPeriod + " г."  : "c " + sYmBeg + " по " + sYmEnd;
//                string sTitle = xls.Workbook.GetCellValue(1,1).ToString();
//                xls.Workbook.SetCellValue(1,1, sTitle.Replace("[per_or_from_to]", sPerOrFromTo));
//                xls.Workbook.HeadingColWidth = 100;   
//                iColCount = dtRes.Rows.Count;  // количество строк в таблице соответствует количеству столбцов в отчете

//                // --- Если есть данные
//                if (iColCount > 0)
//                {
//                    DataRow dr;
//                //    string sCellVal;

//                    // --- идем по колонкам шаблона
//                    for (int c = 0; c < iColCount; c++)
//                    {
//                        //------ копируем столбец вместе с формулами если не последний столбец
//                        if (c < iColCount - 1)
//                        {
//                            range = new FlexCel.Core.TXlsCellRange(iHeaderRowStart, iHeaderColStart + c, iRowStop, iHeaderColStart + c);
//                            xls.Workbook.InsertAndCopyRange(range,  // ранг что копируем
//                                                              iHeaderRowStart,
//                                                              iHeaderColStart + c + 1,  // следующая колонка
//                                                              1, // количество копий
//                                                          FlexCel.Core.TFlxInsertMode.ShiftRangeRight,
//                                                          FlexCel.Core.TRangeCopyMode.All,   // -- и формат и формулы
//                                                          xls.Workbook, 1);
                       
//                               // -----  устанавливаем ширину колонки. что-то пошло не так
//                    //    var Range_ = xls.Workbook.SheetType.GetType().InvokeMember("Range", BindingFlags.GetProperty,
//                    //        null, xls.Workbook, new object[] { range });
//                    //    object[] args = new object[] { 50 };
//                    //    Range_.GetType().InvokeMember("ColumnWidth", BindingFlags.SetProperty, null, Range_, args);
//                    //          xls.Workbook.SetColWidth(xls.Workbook.ColCount, 3000); //последняя колонка не выравнивается
//        //----------------
//                        }
//                        dr = dtRes.Rows[c];
//                        //--- идем по строкам шаблона внутри одного столбца и заполняем данными
//                        set_value_in_column(xls.Workbook, dr, iHeaderRowStart, iHeaderColStart, iRowStop, c);
        

//                    } // end for per columns
//       // -----------------------------------

//                    // --- Ценовая зона: группировка MergeCells без нумерации
//                   iCountZoneGroup = merge_cells_right(xls.Workbook, iHeaderRowStart, iHeaderColStart, iColCount, false);

//                    // --- Область (пока ГТП ГП): группировка MergeCells с нумерацией
//                   iCountGPGroup = merge_cells_right(xls.Workbook, iHeaderRowStart + 1, iHeaderColStart, iColCount, true, iHeaderRowStart + 2);

//                    // --- нумерация колонок по ГТР
//                    range_numbering(xls.Workbook, iHeaderRowStart + 6, iHeaderColStart, iColCount);
                   
//                    //--- Обязательно нужно пересчитать все формулы
//                    xls.Workbook.Recalc();
                    
//                    //--- Добавляем итоговые столбцы 
//                    //--- сначала для всего, потом по группам на уровне ценовой зоны (добавляем итоговые столбцы для каждой группы даже если одна колонка в группе)
//                    string sValueFirst = xls.Workbook.GetCellValue(iHeaderRowStart, iHeaderColStart).ToString();
//                    string sValueNext = string.Empty;
//                    int iColCountWork = iHeaderColStart + iColCount - 1; // всего количество колонок в отчете
//                    int iColCountInGr = iColCount;  // кол-во колонок внутри одной группы (для всей таблицы группа одна)
//                    decimal sumForGr = 0;
//                    object objVal;

//                    // --- добавляем итоговый столбец для всего
//                    range = new FlexCel.Core.TXlsCellRange(iHeaderRowStart, iColCountWork , iRowStop, iColCountWork);
//                    xls.Workbook.InsertAndCopyRange(range,  // ранг что копируем
//                                                      iHeaderRowStart,
//                                                      iColCountWork + 1,  // следующая колонка с учетом итоговых
//                                                      1, // количество копий
//                                                  FlexCel.Core.TFlxInsertMode.ShiftRangeRight,
//                                               //   FlexCel.Core.TRangeCopyMode.Formats,   // только формат для итогов
//                                                 FlexCel.Core.TRangeCopyMode.All,   // -- и формат и формулы
//                                                  xls.Workbook, 1);
//                    // --- объединяем ячейки в шапке по вертикали
//                    xls.Workbook.MergeCells(iHeaderRowStart, iColCountWork + 1, iHeaderRowStart + 6, iColCountWork + 1);
//                    // --- добавляем название шапки
//                    xls.Workbook.SetCellValue(iHeaderRowStart, iColCountWork + 1, "Итого ");
//                    int j = iColCountWork;
//                    //------------------------------------
//                    // --- добавляем данные в строки по итоговому столбцу
//                    for (int iRow = iDataRowStart; iRow < iRowStop; iRow++)
//                    {
//                        if (xls.Workbook.GetCellValue(iRow, j ) != null)
//                        {
//                            p = (xls.Workbook.GetCellValue(iRow, j)) as FlexCel.Core.TFormula;
//                            strP = p == null ? "" : p.Text;  // получаем формулу
//                            if (strP.StartsWith("="))       // -- если значение начинается с "=", это формула => пропускаем строку, затем просто пересчитаем формулу
//                            {
//                                 continue;
//                            }
//                            sumForGr = 0;
//                            // --- вычисляем сумму для итого по строке в группе по ценовым зонам
//                            for (int iCol = j - iColCountInGr + 1; iCol < j + 1; iCol++)
//                              {
//                                 objVal = xls.Workbook.GetCellValue(iRow, iCol);
//                                 sumForGr += objVal == null ? 0 : Convert.ToDecimal(objVal);
//                              }
//                              xls.Workbook.SetCellValue(iRow, j + 1, sumForGr);
//                         }
//                     }
                   
//                    //------ Подитоги по ценовым зонам
//                   iColCountInGr = 0;

//                   for (int i = iHeaderColStart; i < iColCountWork + iCountZoneGroup + 1; i++)  // для последнего столбца тоже нужен итоговый стобец 
//                    {
//                        objVal = xls.Workbook.GetCellValue(iHeaderRowStart, i);
//                        sValueNext = objVal == null ? " " : objVal.ToString();
//                        if (sValueFirst != sValueNext)
//                        {
//                            range = new FlexCel.Core.TXlsCellRange(iHeaderRowStart, i - 1, iRowStop, i - 1);
//                            // --- добавляем итоговый столбец для группы
//                            xls.Workbook.InsertAndCopyRange(range,  // ранг что копируем
//                                                              iHeaderRowStart,
//                                                              i,  // следующая колонка
//                                                              1, // количество копий
//                                                          FlexCel.Core.TFlxInsertMode.ShiftRangeRight,
//                                                         // FlexCel.Core.TRangeCopyMode.Formats,   // только формат для итогов
//                                                          FlexCel.Core.TRangeCopyMode.All,   // -- и формат и формулы
//                                                          xls.Workbook, 1);
                             
//                            // --- объединяем ячейки в шапке по вертикали
//                            xls.Workbook.MergeCells(iHeaderRowStart, i, iHeaderRowStart + 6, i);
//                            // --- добавляем название шапки
//                            xls.Workbook.SetCellValue(iHeaderRowStart, i, "Итого " + sValueFirst);
//                            // --- добавляем данные в строки по итоговому столбцу
//                             for (int iRow = iDataRowStart; iRow < iRowStop; iRow++)
//                            {
//                                if (xls.Workbook.GetCellValue(iRow, i - 1) != null)
//                                {
//                                    p = (xls.Workbook.GetCellValue(iRow, j)) as FlexCel.Core.TFormula;
//                                    strP = p == null ? "" : p.Text;  // получаем формулу
//                                    if (strP.StartsWith("="))       // -- если значение начинается с "=", это формула => пропускаем строку, затем просто пересчитаем формулу
//                                    {
//                                        continue;
//                                    }
//                                    sumForGr = 0;
//                                    // --- вычисляем сумму для итого по сторке в группе по ценовым зонам
//                                    for (int iCol = i - iColCountInGr; iCol < i; iCol++)
//                                    {
//                                        objVal = xls.Workbook.GetCellValue(iRow, iCol);
//                                        sumForGr += objVal == null ? 0 : Convert.ToDecimal(objVal);
//                                    }
//                                    xls.Workbook.SetCellValue(iRow, i, sumForGr);
//                                }
//                            }
//                            // --- 
//                            sValueFirst = sValueNext;
//                            iColCountInGr = 0;                           
//                   }
//                        else 
//                        {
//                            iColCountInGr += 1;
//                        }
//                    }
                              
//                 // --- второй лист
//                xls.SetActiveSheet(2); //установить второй лист активным
//                string sTitle2 = xls.Workbook.GetCellValue(1, 1).ToString();
//                xls.Workbook.SetCellValue(1, 1, sTitle2.Replace("[per_or_from_to]", sPerOrFromTo));

//                //--- идем по строкам шаблона внутри столбца ГП и заполняем данными
                
//                if (dtRes2.Rows.Count > 0)
//                {
//                set_value_in_column(xls.Workbook, dtRes2.Rows[0], iHeaderRowStart, iHeaderColStart, iRowStop, 0);
//                }
//                else      // -- если нет данных по ГП
//                    {    // -- удаляем служебные данные в столбце ГП
//                       clear_cell(xls.Workbook, iHeaderRowStart, iRowStop, iHeaderColStart);
//                    }

     
//                // --- копируем итоги из предыдущего листа
//                for (int i = iDataRowStart; i < iRowStop; i++)
//                {
//                  xls.Workbook.CopyCell(xls.Workbook, 1, 2, i, iColCountWork + iCountZoneGroup + 1
//                                       , i-3   // -- строка куда копируем данные (на втором листе смещение на 3 строки в шаблоне)
//                                       , 7     // -- колонка куда копируем данные
//                                       , FlexCel.Core.TRangeCopyMode.All);
//                }
             
//               }
//               else
//                  {  //---- нет данных. Удаляем служебные данные из файла

//                      clear_cell(xls.Workbook, iHeaderRowStart, iRowStop, iHeaderColStart);
                    
//                  }
//         //----------------------------------------------------------
               
//                xls.SetActiveSheet(1); //установить первый лист активным

//         //-------------------- Сохраняем заполненный шаблон
//                bool res = xls.Save(sRepFullPathNew, false);
//                if (!res)
//                {                   
//                   return false;
//                }
//                xls = null;
              
//               // Cmn.OpenPrintedFile(sRepFullPathNew);
//                if (!string.IsNullOrEmpty(sRepFullPathNew))
//                {
//                    if (XtraMessageBox.Show("Открыть файл " + sRepFullPathNew + "?", "Выгрузка завершена", MessageBoxButtons.YesNo) == DialogResult.Yes)
//                    {
//                        Process.Start(sRepFullPathNew);
//                    }
//                }
//                return true;
//            }
//            catch (Exception ex)
//            {
//                infoenergo.ui.win.ExceptionHandler.HandleException(ex);
//                return false;
//            }    
//        }
//        #endregion

//         /// <summary>
//         /// Merge ячейки в строке. Возвращает количество получившихся групп в строке
//         /// </summary>
//         /// <param name="xlsWkbk">ExcelHelper.Workbook</param>
//         /// <param name="iRowStartGr">номер строки</param>
//         /// <param name="iColStart">начальный номер колонки</param>
//         /// <param name="iColCount">количество колонок для прохода</param>
//         /// <param name="flagNum">флаг необходимости нумерации отдельной строки для данной группировки</param>
//         /// <param name="iRowForNumGr">номер строки для нумерации группировки</param>
//        private static int merge_cells_right(XlsFile xlsWkbk, int iRowStartGr, int iColStart, int iColCount, bool flagNum = false, int iRowForNumGr = -1 )
//          {
//            // --- получаем значение первой ячейки
//            string strValueFirst = xlsWkbk.GetCellValue(iRowStartGr, iColStart).ToString();
//            string strValueNext = string.Empty;         
//            int iColStartGr = iColStart;
//            int iValForNum = 1;
             
//            for (int i = iColStart + 1; i < iColStart + iColCount; i++)
//              {
//                strValueNext = xlsWkbk.GetCellValue(iRowStartGr, i).ToString();
//                if (flagNum) { xlsWkbk.SetCellValue(iRowForNumGr, iColStartGr, iValForNum); }
//                if (strValueFirst == strValueNext)
//                  {
//                    xlsWkbk.MergeCells(iRowStartGr, iColStartGr, iRowStartGr, iColStartGr + 1);
//                    // --- если установлен флаг нумерации столбцов группы, то объединяем строку с нумерацией и проставляем номер
//                    if (flagNum)   { xlsWkbk.MergeCells(iRowForNumGr, iColStartGr, iRowForNumGr, iColStartGr + 1);     }
//                  }
//                else 
//                  { 
//                    strValueFirst = strValueNext; 
//                    iValForNum += 1; 
//                   }                    
//                iColStartGr += 1;
//              }   // end for
//            // -- не забываем про последний столбец
//            if (flagNum) { xlsWkbk.SetCellValue(iRowForNumGr, iColStartGr, iValForNum); }
//            return iValForNum; 
//          }   //------------ end procedure merge_cells_right()

//        /// <summary>
//        /// Проставление нумерации в строке диапазон колонок 
//        /// </summary>
//        /// <param name="xlsWkbk">книга excel</param>
//        /// <param name="iRowForNum">номер строки для работы</param>
//        /// <param name="iColStart">номер стартовой колонки для нумерации</param>
//        /// <param name="iColCount">количество колонок для нумерации</param>
//        private static void range_numbering(XlsFile xlsWkbk, int iRowForNum, int iColStart, int iColCount)
//          {
//            int iValForNum = 1;
//            int iColStop = iColStart + iColCount;
//            for (int i = iColStart; i < iColStop; i++)
//              {
//                xlsWkbk.SetCellValue(iRowForNum, i, iValForNum); 
//                iValForNum += 1;
//              }   // end for
//        }   //------------ end procedure range_numbering

        
//         /// <summary>
//         /// Заполнение столбца данными
//         /// </summary>
//         /// <param name="xlsWkbk">книга</param>
//         /// <param name="dr">datarow с данными </param>
//         /// <param name="iHeaderRowStart"></param>
//         /// <param name="iHeaderColStart"></param>
//         /// <param name="iRowStop"></param>
//         /// <param name="c"> номер колонки (относительно только данных, а не листа) для записи данных</param>
//        private static void set_value_in_column(XlsFile xlsWkbk, DataRow dr, 
//                                                int iHeaderRowStart, int iHeaderColStart, int iRowStop, int c)
//        {
//            string sCellVal;

//             //--- идем по строкам шаблона внутри одного столбца 
//             for (int r = iHeaderRowStart; r < iRowStop; r++)
//                        {
//                            // --- читаем значение столбца в шаблоне
//                            if (xlsWkbk.GetCellValue(r, iHeaderColStart + c) == null)
//                            {
//                                continue;
//                            }
//                            sCellVal = xlsWkbk.GetCellValue(r, iHeaderColStart + c).ToString();
//                            if (sCellVal.StartsWith("["))  // -- если первый символ "[", то внутри скобочек имя столбца, откуда нужно взять значение
//                            {
//                                string tempforCheck = dr[sCellVal.Trim(new char[] { '[', ']' })].ToString();
//                                xlsWkbk.SetCellValue(r, iHeaderColStart + c, dr[sCellVal.Trim(new char[] { '[', ']' })]);
//                            }
//                        }  // --- end for per rows in excel       
//        }

//      /// <summary>
//      /// Удаляем служебные данные из столбца с данными
//      /// </summary>
//      /// <param name="xlsWkbk"></param>
//      /// <param name="iHeaderRowStart"></param>
//      /// <param name="iRowStop"></param>
//      /// <param name="iCol"></param>
//      private static void  clear_cell(XlsFile xlsWkbk, int iHeaderRowStart, int iRowStop, int iCol)
//       {
//           for (int i = iHeaderRowStart; i < iRowStop; i++)
//                      {
//                          xlsWkbk.SetCellValue(i, iCol, ""); 
//                      }
//       }
//    }  // --- end class
//}
