using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;
using sql.builder.DataApi;
//using sql.builder.ExcelApi;
using sql.builder.Print.Xlsx;

namespace sql.builder.Print.Xlsx
{
    /// <summary>
    /// Cтрока шаблона Excel в формате xlsx
    /// </summary>
    /// <seealso cref="sql.builder.Print.XML.ExcelPrintRow"/>
    internal class ExcelPrintRow : IExcelPrintElement
    {
        #region поля
        private readonly IExcelPrintGroup parent;
        private readonly ExcelPrintSheet sheet;
        private readonly ExcelRow row;
        private readonly Dictionary<ExcelCell, IExcelPrintValue> _vals;
        private SortedList<string, string> _varCellsNames;
        #endregion
        internal ExcelPrintRow(ExcelPrintSheet sheet, ExcelRow row, IExcelPrintGroup parent = null)
        {
            this.parent = parent;
            this.sheet = sheet;
            this.row = row;
            this._vals = new Dictionary<ExcelCell, IExcelPrintValue>();
            for (int index = 0; index < row.Cells.Count; index++) {
                ExcelCell cell = row.Cells[index];
                if (cell.Text.Contains("[:")) {
                    IExcelPrintValue val = ExcelPrintValue.Create(cell, this);
                    this._vals.Add(cell, val);
                }
            }
        }
        public IExcelPrintGroup Parent { get { return this.parent; } }
        internal SortedList<string, string> GetVarColsIndex()
        {
            if (this._varCellsNames == null) {
                this._varCellsNames = new SortedList<string, string>();
                foreach (IExcelPrintValue val in this._vals.Values) {
                    SingleExcelPrintValue single_val = val as SingleExcelPrintValue;
                    if (single_val != null) {
                        string column_name = single_val.ColumnName;
                        if (!this._varCellsNames.ContainsKey(column_name)) {
                            this._varCellsNames.Add(column_name, single_val.ExcelColumnName);
                        }
                    }
                }
            }
            return this._varCellsNames;
        }
        public void Print(WorksheetPrint pi, DataSet data, bool use_data_reader, DataRow imputedRow = null)
        {
            this.sheet.NextRow();
            var values = new Dictionary<ExcelCell, object>(this._vals.Count);
            var hyTragets = new Dictionary<ExcelCell, string>();
            foreach (KeyValuePair<ExcelCell, IExcelPrintValue> val in this._vals) {
                string hyperlinkTarget;
                object val1 = val.Value.GetValue(pi, out hyperlinkTarget);
                values.Add(val.Key, val1);
                if (!string.IsNullOrEmpty(hyperlinkTarget)) {
                    hyTragets.Add(val.Key, hyperlinkTarget);
                }
            }
            pi.Env.PrintRow(pi, this.row, values, hyTragets);
        }
        public void DeleteNode(WorksheetPrint pi)
        {
            pi.MarkRowAsDeleted(this.row);
        }
    }
}
