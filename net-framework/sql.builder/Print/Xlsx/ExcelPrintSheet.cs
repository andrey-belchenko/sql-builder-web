using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;
//using DevExpress.Utils.CodedUISupport;
using sql.builder.DataApi;
using sql.builder.ExcelApi;
using sql.builder.Print.Xlsx;

namespace sql.builder.Print.Xlsx
{
    /// <seealso cref="sql.builder.Print.XML.ExcelPrintSheet"/>
    internal class ExcelPrintSheet
    {
        #region static
        internal static ExcelPrintSheet Create(ExcelWorksheet worksheet, ExcelPrintDocument document, ExcelPrintSheet prev_sheet)
        {
            ExcelPrintSheet sheet;
            string name = worksheet.NativeSheetName;
            int pos_1 = name.IndexOf('{');
            if (pos_1 >= 0) {
                pos_1 = pos_1 + 1;
                int pos_2 = name.IndexOf('}', pos_1);
                if (pos_2 < 0) {
                    pos_2 = name.Length;
                }
                sheet = new ExcelPrintMultiplicatedSheet(worksheet, document, name.Substring(pos_1, pos_2 - pos_1));
            } else {
                sheet = new ExcelPrintSheet(worksheet, document);
            }
            if (prev_sheet != null) {
                prev_sheet.next_sheet = sheet;
            }
            return sheet;
        }
        #endregion
        #region поля
        protected bool printed;
        protected readonly ExcelPrintDocument document;
        protected readonly ExcelWorksheet worksheet;
        protected ExcelPrintSheet next_sheet;
        protected IList<IExcelPrintElement> childs;
        /// <summary>
        /// Число напечатаных строк в отчёте
        /// </summary>
        protected int row_count;
        #endregion
        internal ExcelPrintSheet(ExcelWorksheet worksheet, ExcelPrintDocument document)
        {
            this.worksheet = worksheet;
            this.document = document;
            this.childs = this.makeChildsList(string.Empty, worksheet.Rows, null, null);
        }
        #region свойства
        /// <summary>
        /// Число напечатаных строк в отчёте
        /// </summary>
        internal int RowCount { get { return this.row_count; } }
        internal bool Printed { get { return this.printed; } }
        internal ExcelWorksheet Worksheet { get { return this.worksheet; } }
        /// <summary>
        /// Ссылка на следующий лист
        /// </summary>
        internal ExcelPrintSheet NextSheet { get { return this.next_sheet; } }
        #endregion
        internal void MarkUnprinted()
        {
            this.printed = false;
            this.row_count = 0;
        }
        /// <seealso cref="sql.builder.Print.XML.ExcelPrintSheet.makeChildsList"/>
        internal IList<IExcelPrintElement> makeChildsList(string table_prefix, IList<ExcelRow> rows, ExcelPrintGroup parent, ExcelCell beginCell)
        {
            List<IExcelPrintElement> childs = new List<IExcelPrintElement>();
            List<ExcelRow> childRows = null;
            ExcelCell begMarker = null;
            ExcelCell endMarker = null;
            string table_name = string.Empty;
            bool dontRemove = false;
            for (int index = 0; index < rows.Count; index++) {
                ExcelRow row = rows[index];
                IList<ExcelCell> cells;
                if (index == 0 && beginCell != null) {
                    // для первой строки анализируем ячейки после beginCell
                    //cells = row.Cells.SkipWhile(c => c != beginCell).Skip(1).ToList();
                    IList<ExcelCell> row_cells = row.Cells;
                    int index_2 = row_cells.IndexOf(beginCell);
                    Contract.Assume(index_2 >= 0);
                    index_2++;
                    cells = new List<ExcelCell>(row_cells.Count - index_2);
                    while (index_2 < row_cells.Count) {
                        cells.Add(row_cells[index_2]);
                        index_2++;
                    }
                } else {
                    cells = row.Cells;
                }
                if (begMarker == null && cells.Count > 0) {
                    begMarker = cells.FirstOrDefault(c => c.Text.StartsWith(table_prefix + "begin:"));
                    // 17.04.2017 Емцов - в новой версии печати не обязательно указывать родительский цикл
                    if (begMarker == null && table_prefix.Length > 0 && this.document.Env != null) {
                        begMarker = cells.FirstOrDefault(c => c.Text.StartsWith("begin:"));
                        if (begMarker != null) {
                            begMarker.SetValue(table_prefix + begMarker.Text);
                        }
                    }
                    if (begMarker != null) {
                        string text = begMarker.Text;
                        int pos_1 = table_prefix.Length + 6;
                        Contract.Assume(text.Substring(0, pos_1) == table_prefix + "begin:");
                        int pos_2 = text.IndexOfWhiteSpace(pos_1);
                        if (pos_2 < 0) {
                            pos_2 = text.Length;
                        }
                        int pos_3 = pos_2 - 3;
                        if (text[pos_3] == '(' && text[pos_3 + 1] == '+' && text[pos_3 + 2] == ')') {
                            Contract.Assume(text.Substring(pos_3, 3) == "(+)");
                            dontRemove = true;
                            pos_2 = pos_3;
                        }
                        table_name = text.Substring(pos_1, pos_2 - pos_1);
                    }
                }
                if (begMarker == null) {
                    childs.Add(new ExcelPrintRow(this, row, parent));
                } else {
                    if (childRows == null) {
                        childRows = new List<ExcelRow>(1);
                    }
                    childRows.Add(row);
                    endMarker = cells.FirstOrDefault(c => c.Text.Contains("end:" + table_name + ";"));
                    if (endMarker != null) {
                        childs.Add(new ExcelPrintGroup(this, childRows, dontRemove, parent, table_name, begMarker));
                        begMarker = null;
                        dontRemove = false;
                        childRows = null;
                    }
                }
            }
            if (begMarker != null && endMarker == null) {
                throw new ExcelException("Не найдена закрывающая метка для цикла " + begMarker.Text);
            }
            return childs;
        }
        internal virtual ExcelPrintSheet Print(DataSet data, bool use_data_reader)
        {
            this.PrintData(data, use_data_reader);
            return this.next_sheet;
        }
        protected void PrintData(DataSet data, bool use_data_reader, DataRow row = null, string sheetName = null)
        {
            this.printed = true;
            using (WorksheetPrint pi = this.document.Env.BeginPrint(this.worksheet, sheetName)) {
                for (int index = 0; index < this.childs.Count; index++) {
                    IExcelPrintElement node = this.childs[index];
                    bool other = false;
                    if (row != null) {
                        ExcelPrintGroup element = node as ExcelPrintGroup;
                        if (element != null) {
                            if (element.GetTableReference().MainTableName != row.Table.TableName) {
                                node.Print(pi, data, use_data_reader, null);
                                other = true;
                            }
                        }
                    }
                    if (!other) {
                        node.Print(pi, data, use_data_reader, row);
                    }
                }
                ExcelPrintEnv.EndPrint(pi);
            }
            this.document.NextPrintedSheet(this);
        }
        internal void NextRow()
        {
            this.row_count++;
            this.document.NextPrintedRow(this);
        }
        //public bool HasMixedSources = false;
        //public int CalculateRowsCount( DataSet dataSet, DataRow row)
        //{
        //    int rowsCount = 0;
        //    if (HasMixedSources)
        //    {
        //        foreach (ExcelPrintNode node in Childs)
        //        {
        //            bool other = false;
        //            if (row != null)
        //            {
        //                if (node.GetType() == typeof(ExcelPrintElement))
        //                {
        //                    if ((node as ExcelPrintElement).TableReferences[0].MainTableName != row.Table.TableName)
        //                    {
        //                        node.Print(null, dataSet, true, null, false,false);
        //                        other = true;
        //                    }
        //                }
        //            }
        //            if (!other)
        //            {
        //                node.Print(null, dataSet, true, row, false,false);
        //            }
        //        }
        //        rowsCount = RowsCount;
        //        RowsCount = 0;
        //        return rowsCount;
        //    }
        //    foreach (ExcelPrintNode node in Childs)
        //    {
        //        if (node.GetType() == typeof(ExcelPrintText))
        //        {
        //            rowsCount++;
        //        }
        //        else
        //        {
        //            rowsCount += (node as ExcelPrintElement).CalculateRowsCount(dataSet);
        //        }
        //    }
        //    return rowsCount;
        //}
    }
    internal class ExcelPrintMultiplicatedSheet : ExcelPrintSheet
    {
        #region поля
        private string name_variable;
        #endregion
        internal ExcelPrintMultiplicatedSheet(ExcelWorksheet worksheet, ExcelPrintDocument document, string name_variable)
            : base(worksheet, document)
        {
            Contract.Assert(!string.IsNullOrEmpty(name_variable));
            this.name_variable = name_variable;
        }
        private void GetMultipleSet(IList<Tuple<ExcelPrintMultiplicatedSheet, string, DataRow>> list, DataSet dataSet, bool print_big_data, string sheetName)
        {
            ExcelPrintGroup el = this.childs.OfType<ExcelPrintGroup>().First();
            List<DataRow> rows = el.GetRowsByParent(dataSet, print_big_data);
            for (int index = 0; index < rows.Count; index++) {
                DataRow row = rows[index];
                string name = sheetName;
                name = name.Replace("{" + this.name_variable + "}", row[this.name_variable].ToString());
                list.Add(new Tuple<ExcelPrintMultiplicatedSheet, string, DataRow>(this, name, row));
            }
        }
        internal override ExcelPrintSheet Print(DataSet data, bool use_data_reader)
        {
            if (this.printed) {
                return this.next_sheet;
            } else {
                IList<Tuple<ExcelPrintMultiplicatedSheet, string, DataRow>> list = new List<Tuple<ExcelPrintMultiplicatedSheet, string, DataRow>>();
                ExcelPrintSheet sheet;
                ExcelPrintMultiplicatedSheet mp_sheet = this;
                do {
                    mp_sheet.GetMultipleSet(list, data, use_data_reader, mp_sheet.worksheet.NativeSheetName);
                    sheet = mp_sheet.next_sheet;
                    mp_sheet = sheet as ExcelPrintMultiplicatedSheet;
                } while (mp_sheet != null);
                if (list.Count > 1 && !use_data_reader) { // т.к. дочерние строки читаются в оригинальном порядке , родительские тоже нужно оставить в оригинальном, можно организовать сортировку листов Excel , после печати
                    list = list.OrderBy(r => r.Item2).ToList();
                }
                for (int index = 0; index < list.Count; index++) {
                    Tuple<ExcelPrintMultiplicatedSheet, string, DataRow> rec = list[index];
                    rec.Item1.PrintData(data, use_data_reader, rec.Item3, rec.Item2);
                }
                return sheet;
            }
        }
    }
}
