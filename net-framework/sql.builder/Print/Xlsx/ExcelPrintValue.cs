using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml.Linq;
using System.Globalization;
using Contract = System.Diagnostics.Contracts.Contract;
using sql.builder.ExcelApi;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx
{
    internal static class ExcelPrintValue
    {
        internal const string rowIndStr = "[ind]";
        internal static IExcelPrintValue Create(ExcelCell cell, ExcelPrintRow parent)
        {
            string text = cell.Text;
            int len = text.Length;
            if (len > 3 && text[0] == '[' && text[1] == ':' && text.IndexOf(']', 2) == (len - 1)) {
                int pos = text.LastIndexOf('.');
                if (pos >= 0) {
                    string table_name = string.Intern(text.Substring(2, pos - 2));
                    string column_name = text.Substring(pos + 1, len - pos - 2);
                    return new SingleExcelPrintValue(cell, parent, table_name, column_name);
                }
            }
            return new ComplexExcelPrintValue(cell, parent);
        }
        /*
        #region поля
        internal SortedList<string, List<string>> TableColumns;
        private bool SingleValue;
        private ExcelPrintRow parent;
        private ExcelCell cell;
        private string _formula;
        #endregion
        internal ExcelPrintValue(ExcelCell cell, ExcelPrintRow parent)
        {
            this.cell = cell;
            this.parent = parent;
            string text = cell.Text;
            this.TableColumns = new SortedList<string, List<string>>(1);
            List<string> vars = Cmn.ExtractParamsFromString(text);
            this.SingleValue = (vars.Count == 1) && (text == "[:" + vars[0] + "]");
            for (int index = 0; index < vars.Count; index++) {
                string str = vars[index];
                int len = str.Length;
                int pos = str.LastIndexOf('.');
                if (pos >= 0) {
                    string table_name = string.Intern(str.Substring(0, pos));
                    string column_name = str.Substring(pos + 1);
                    List<string> cols;
                    if (!this.TableColumns.TryGetValue(table_name, out cols)) {
                        cols = new List<string>(1);
                        cols.Add(column_name);
                        this.TableColumns.Add(table_name, cols);
                    } else if (!cols.Contains(column_name)) {
                        cols.Add(column_name);
                    }
                }
            }
        }
        internal ExcelCell Cell { get { return this.cell; } }
        // 15.05.17 постарался оптимизировать ф-ю
        internal object GetValue(WorksheetPrint pi, ref string hyperlinkTarget)
        {
            string text = cell.Text;
            TableReference tr = null;
            DataTable table = null;
            string columnName = null;
            object val = null;
            string val_text = string.Empty;
            foreach (KeyValuePair<string, List<string>> tc in this.TableColumns) {
                string tableName = tc.Key;
                tr = this.parent.Parent.GetTableReference(tableName);
                table = tr.Table;
                for (int index = 0; index < tc.Value.Count; index++) {
                    string colName = tc.Value[index];
                    if (colName == TextConst.AVSpecColumn.RowId) {
                        columnName = table.PrimaryKey[0].ColumnName;
                    } else {
                        columnName = colName;
                    }
                    val_text = string.Empty;
                    val = null;
                    if (pi != null) {
                        if (this._formula == null) {
                            XElement xformula = tr.GetColumnFormula(columnName);
                            if (xformula != null) {
                                SortedList<string, string> varCellsNames = parent.GetVarColsIndex();
                                foreach (XElement xcol in xformula.Descendants(AName.Column)) {
                                    string colName1 = xcol.Attribute(AName.Column).Value;
                                    string sref = varCellsNames[colName1] + rowIndStr;
                                    xcol.Value = sref;
                                }
                                this._formula = xformula.Value;
                            } else {
                                this._formula = string.Empty;
                            }
                        }
                        if (this._formula != string.Empty) {
                            var f = new ExcelPrintFormula();
                            f.Formula = this._formula.Replace(rowIndStr, (pi.LastPrintedRowID + 1).ToString())
                            .Replace(" ", string.Empty);// для красоты, но на будкщее придумать корректный вариант, могут быть строковые константы
                            return f;
                        }
                    }
                    if (tr.IsCurrentRowExists()) {
                        val = tr.GetCurrentRowValue(columnName);
                        val_text = val.ToString();
                    }
                    if (!this.SingleValue) {
                        text = text.Replace("[:" + tableName + "." + colName + "]", val_text);
                    }
                }
            }
            if (this.SingleValue) {
                if (val_text == string.Empty) {
                    if (tr != null && !tr.IsCurrentRowExists() && table.Columns[columnName].DataType == typeof(decimal)) {
                        return ZERO;
                    } else {
                        return val_text;
                    }
                }
                VDataTable vt = table as VDataTable;
                if (vt != null) {
                    if (tr != null && tr.IsCurrentRowExists()) {
                        if (Printing.CreateRefs && !vt.GetColumn(columnName).IsEmptyEvent && (vt.HasRowEvents || vt.GetColumn(columnName).HasCellEvents)) {
                            hyperlinkTarget = "http://" + table.TableName + "." + columnName + ".[" + tr.GetCurrentRowValue(table.PrimaryKey[0].ColumnName).ToString() + "]";
                        }
                    }
                }
                Type type = table.Columns[columnName].DataType;
                if (type == typeof(decimal)) {
                    if (val is decimal) {
                        return val;
                    } else {
                        return Convert.ToDecimal(val_text);
                    }
                } else if (type == typeof(DateTime)) {
                    // время тоже выведется если есть
                    DateTime? dat = null;
                    decimal oaDate = 0M;
                    try {
                        dat = Convert.ToDateTime(val);
                        oaDate = (decimal)dat.GetValueOrDefault().ToOADate();
                    } catch {
                        return (dat.HasValue) ? dat.GetValueOrDefault().ToString("dd.MM.yyyy") : null;
                    }
                    return oaDate;
                } else {
                    return val_text;
                }
            } else {
                return text;
            }
        }*/
        internal static void GetFormula(TableReference tr, string column_name, ExcelPrintRow row, out string formula)
        {
             XElement xformula = tr.GetColumnFormula(column_name);
             if (xformula == null) {
                 formula = string.Empty;
                 return;
             }
             SortedList<string, string> varCellsNames = row.GetVarColsIndex();
             foreach (XElement xcol in xformula.Descendants(AName.column)) {
                 string name = xcol.Attribute(AName.column).Value;
                 string sref = varCellsNames[name] + rowIndStr;
                 xcol.Value = sref;
             }
             formula = xformula.Value;
        }
    }
    internal interface IExcelPrintValue
    {
        object GetValue(WorksheetPrint pi, out string hyperlinkTarget);
    }
    /// <summary>
    /// Ячейка отчёта, текст которой полностью заменяется на значение из БД
    /// </summary>
    internal class SingleExcelPrintValue : IExcelPrintValue
    {
        private string table_name;
        private string column_name;
        private string formula;
        private ExcelPrintRow parent;
        private ExcelCell cell;
        internal static string HyperlinkSlashPlaceholder = "66bce492";
        internal SingleExcelPrintValue(ExcelCell cell, ExcelPrintRow parent, string table_name, string column_name)
        {
            this.cell = cell;
            this.parent = parent;
            this.table_name = table_name;
            this.column_name = column_name;
        }
        internal string ColumnName { get { return this.column_name; } }
        internal string ExcelColumnName { get { return this.cell.CellInfo.ColumnName; } }
        public object GetValue(WorksheetPrint pi, out string hyperlinkTarget)
        {
            hyperlinkTarget = null;
            TableReference tr = this.parent.Parent.GetTableReference(this.table_name);
            DataTable table = tr.Table;
            DataColumn column;
            if (this.column_name == TextConst.AVSpecColumn.RowId) {
                column = table.PrimaryKey[0];
            } else {
                column = table.Columns[this.column_name];
            }
            object val = null;
            string val_text = string.Empty;
            if (pi != null) {
                if (this.formula == null) {
                    ExcelPrintValue.GetFormula(tr, column.ColumnName, this.parent, out this.formula);
                }
                if (this.formula != string.Empty) {
                    var f = new ExcelPrintFormula();
                    f.Formula = this.formula.Replace(ExcelPrintValue.rowIndStr, (pi.LastPrintedRowID + 1).ToString())
                    .Replace(" ", string.Empty);// для красоты, но на будкщее придумать корректный вариант, могут быть строковые константы
                    return f;
                }
                if (tr.IsCurrentRowExists()) {
                    val = tr.GetCurrentRowValue(column);
                    val_text = val.ToString();
                }
            }
            Type type = column.DataType;
            if (val_text == string.Empty) {
                if (!tr.IsCurrentRowExists() && type == typeof(decimal)) {
                    return Cmn.DECIMAL_ZERO;
                } else {
                    return string.Empty;
                }
            }
            VDataTable vt = table as VDataTable;
            if (vt != null) {
                VDataColumn vc = (VDataColumn)column;
                if (tr != null && tr.IsCurrentRowExists()) {
                    if (Printing.CreateRefs && (!vc.IsEmptyEvent) && (vt.HasRowEvents || vc.HasCellEvents))
                    {
                        var rowVal = tr.GetCurrentRowValue(table.PrimaryKey[0].ColumnName)
                            .ToString()
                            .Replace("/", HyperlinkSlashPlaceholder);
                        hyperlinkTarget = "http://" + table.TableName + "." + column.ColumnName + ".[" + rowVal + "]";
                    }
                }
            }
            if (type == typeof(decimal)) {
                if (val is decimal) {
                    return val;
                } else {
                    return Convert.ToDecimal(val_text);
                }
            } else if (type == typeof(DateTime)) {
                // время тоже выведется если есть
                DateTime? dat = null;
                decimal oaDate = decimal.Zero;
                try {
                    dat = Convert.ToDateTime(val);
                    oaDate = (decimal)dat.GetValueOrDefault().ToOADate();
                } catch {
                    if (dat.HasValue) {
                        return dat.GetValueOrDefault().ToString("dd.MM.yyyy");
                    } else {
                        return null;
                    }
                }
                return oaDate;
            } else {
                return val_text;
            }
        }
    }
    internal class ComplexExcelPrintValue : IExcelPrintValue
    {
        [ThreadStatic]
        private static StringBuilder buffer;
        private SortedList<string, List<string>> table_columns;
        private string formula;
        private ExcelPrintRow parent;
        private ExcelCell cell;
        internal ComplexExcelPrintValue(ExcelCell cell, ExcelPrintRow parent)
        {
            this.cell = cell;
            this.parent = parent;
            this.table_columns = new SortedList<string, List<string>>(1);
            List<string> vars = Cmn.ExtractParamsFromString(this.cell.Text);
            for (int index = 0; index < vars.Count; index++) {
                string str = vars[index];
                int len = str.Length;
                int pos = str.LastIndexOf('.');
                if (pos >= 0) {
                    string table_name = string.Intern(str.Substring(0, pos));
                    string column_name = str.Substring(pos + 1);
                    List<string> cols;
                    if (!this.table_columns.TryGetValue(table_name, out cols)) {
                        cols = new List<string>(1);
                        cols.Add(column_name);
                        this.table_columns.Add(table_name, cols);
                    } else if (!cols.Contains(column_name)) {
                        cols.Add(column_name);
                    }
                }
            }
        }
        public object GetValue(WorksheetPrint pi, out string hyperlinkTarget)
        {
            hyperlinkTarget = null;
            if (buffer == null) {
                buffer = new StringBuilder(256);
            }
            Contract.Assume(buffer.Length == 0);
            buffer.Append(cell.Text);
            foreach (KeyValuePair<string, List<string>> tc in this.table_columns) {
                string table_name = tc.Key;
                TableReference tr = this.parent.Parent.GetTableReference(table_name);
                DataTable table = tr.Table;
                for (int index = 0; index < tc.Value.Count; index++) {
                    string column_name = tc.Value[index];
                    DataColumn column;
                    if (column_name == TextConst.AVSpecColumn.RowId) {
                        column = table.PrimaryKey[0];
                        column_name = column.ColumnName;
                    } else {
                        column = table.Columns[column_name];
                    }
                    if (pi != null) {
                        if (this.formula == null) {
                            ExcelPrintValue.GetFormula(tr, column_name, this.parent, out this.formula);
                        }
                        if (this.formula != string.Empty) {
                            var f = new ExcelPrintFormula();
                            f.Formula = this.formula.Replace(ExcelPrintValue.rowIndStr, (pi.LastPrintedRowID + 1).ToString())
                            .Replace(" ", string.Empty);// для красоты, но на будкщее придумать корректный вариант, могут быть строковые константы
                            return f;
                        }
                    }
                    string val_text;
                    if (tr.IsCurrentRowExists()) {
                        object val = tr.GetCurrentRowValue(column);
                        val_text = val.ToString();
                    } else {
                        val_text = string.Empty;
                    }
                    buffer.Replace("[:" + table_name + "." + column_name + "]", val_text);
                }
            }
            string text = buffer.ToString();
            buffer.Clear();
            return text;
        }
    }
    internal class ExcelPrintFormula
    {
        internal string Formula;
    }
}
