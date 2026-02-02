using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx
{
    public class ExcelWorksheetCols
    {
        // храним индексы в виде чисел, иначе бьет по производительности
        private List<ExcelWorksheetCol> _cols;
        public ExcelWorksheetCols()
        {
            this._cols = new List<ExcelWorksheetCol>();
        }
        public ExcelWorksheetCols(XElement xitem)
        {
            Contract.Assert(xitem != null);
            Contract.Assert(xitem.Name == ns.Main.cols);
            int ind_last = 0;
            // разворачиваем список колонок
            this._cols = new List<ExcelWorksheetCol>();
            foreach (XElement xcol in xitem.Elements(ns.Main.col)) {
                int ind1 = Int32.Parse(xcol.Attribute(ns.None.min).Value);
                int ind2 = Int32.Parse(xcol.Attribute(ns.None.max).Value);
                // описание колонки может отсутствовать в xml, тогда она обрабатывается по умолчанию
                // сохраняем информацию о таких колонках
                if (ind1 - ind_last > 1) {
                    for (int i = ind_last + 1; i < ind1; i++) {
                        this._cols.Add(new ExcelWorksheetCol(i, i, null));
                    }
                }
                for (int i = ind1; i <= ind2; i++) {
                    this._cols.Add(new ExcelWorksheetCol(i, i, new XElement(xcol)));
                }
                ind_last = ind2;
            }
        }
        public void DeleteColumns(IEnumerable<string> col_names)
        {
            var indexes = col_names.Select(ExcelUtils.GetColumnNumber);
            foreach (var col in _cols.ToArray()) {
                if (indexes.Contains(col.ColMin)) {
                    _cols.Remove(col);
                } else {
                    int delta = indexes.Count(i => i < col.ColMin);
                    if (delta > 0) {
                        col.ColMin = (col.ColMin - delta);
                        col.ColMax = (col.ColMax - delta);
                    }
                }
            }
        }
        public void CopyColumns(IEnumerable<string> col_names)
        {
            var indexes = col_names.Select(ExcelUtils.GetColumnNumber).ToArray();
            var list = new List<ExcelWorksheetCol>();
            foreach (var col in _cols.ToArray()) {
                if (indexes.Contains(col.ColMin)) {
                    XElement xcol;
                    if (col.XCol != null) {
                        xcol = new XElement(col.XCol);
                    } else {
                        xcol = null;
                    }
                    list.Add(new ExcelWorksheetCol(col.ColMin + indexes.Length, col.ColMax + indexes.Length, xcol));
                }
                if (col.ColMin == indexes.Last()) {
                    this._cols.InsertRange(this._cols.IndexOf(col) + 1, list);
                } else if (col.ColMin > indexes.Last()) {
                    col.ColMin += indexes.Length;
                    col.ColMax += indexes.Length;
                }
            }
        }
        public void SetColumnWidth(string column_name, decimal width)
        {
            int num = ExcelUtils.GetColumnNumber(column_name);
            var col = this._cols.FirstOrDefault(c => c.ColMin == num);
            if (col != null) {
                // может отсутствовать xml - описание
                XElement xcol = col.XCol;
                if (xcol == null) {
                    xcol = new XElement(ns.Main.col);
                    xcol.Add(new XAttribute(ns.None.min, num));
                    xcol.Add(new XAttribute(ns.None.max, num));
                    col.XCol = xcol;
                }
                xcol.SetAttributeValue(ns.None.width, width.ToString(CultureInfo.InvariantCulture));
                xcol.SetAttributeValue(ns.None.customWidth, "1");
            }
        }
        public XElement GetXml()
        {
            XElement xcols = new XElement(ns.Main.cols);
            if (this._cols.Count > 0) {
                // схлопываем список колонок
                ExcelWorksheetCol col = this._cols[0];
                int cols_count = this._cols.Count;
                if (cols_count > 16384) {
                    cols_count = 16384; // максимум 16384 колонок иначе не откроется
                }
                for (int i = 1; i < cols_count; i++) {
                    ExcelWorksheetCol next_col = this._cols[i];
                    if (EqualCols(col.XCol, next_col.XCol)) {
                        col.ColMax = next_col.ColMax;
                    } else {
                        if (col.XCol != null) {
                            XElement xcol = new XElement(col.XCol);
                            xcol.SetAttributeValue(ns.None.min, col.ColMin);
                            xcol.SetAttributeValue(ns.None.max, col.ColMax);
                            xcols.Add(xcol);
                        }
                        col = next_col;
                    }
                }
                // описание колонки может отсутствовать в xml, тогда она обрабатывается по умолчанию
                if (col.XCol != null) {
                    XElement xlastCol = new XElement(col.XCol);
                    xlastCol.SetAttributeValue(ns.None.min, col.ColMin); // Бельченко ,  чтобы убрать лишние пустые колонки в конце которые остаются после удаления
                    xlastCol.SetAttributeValue(ns.None.max, col.ColMax);
                    xcols.Add(xlastCol);
                }
            }
            return xcols;
        }
        private static bool EqualCols(XElement xcol1, XElement xcol2)
        {
            return (xcol1 == null && xcol2 == null)
                || (xcol1 != null && xcol2 != null
                    && (xcol1.AttrOrDefault(ns.None.width, null) == xcol2.AttrOrDefault(ns.None.width, null))
                    && (xcol1.AttrOrDefault(ns.None.style, null) == xcol2.AttrOrDefault(ns.None.style, null))
                    && (xcol1.AttrOrDefault(ns.None.customWidth, null) == xcol2.AttrOrDefault(ns.None.customWidth, null))
                    && (xcol1.AttrOrDefault(ns.None.bestFit, null) == xcol2.AttrOrDefault(ns.None.bestFit, null))
                    && (xcol1.AttrOrDefault(ns.None.outlineLevel, null) == xcol2.AttrOrDefault(ns.None.outlineLevel, null))
                    && (xcol1.AttrOrDefault(ns.None.hidden, null) == xcol2.AttrOrDefault(ns.None.hidden, null)));
        }
    }
    public class ExcelWorksheetCol
    {
        public XElement XCol;
        public int ColMin;
        public int ColMax;
        public ExcelWorksheetCol(int col_min, int col_max, XElement xcol)
        {
            this.ColMin = col_min;
            this.ColMax = col_max;
            this.XCol = xcol;
        }
    }
}