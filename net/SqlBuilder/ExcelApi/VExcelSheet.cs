using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.ExcelApi
{
    public sealed class VExcelSheet : VExcelObject
    {
        public VExcelSheet(XElement element)
            : base(element)
        {
            this.reSpanColumns();
        }
        public string GetName()
        {
            return this.Element.Attribute(VExcelNS.SpreadSheet.Name).Value;
        }
        private void reSpanColumns()
        {
            foreach (XElement col in this.Element.Descendants(VExcelNS.SpreadSheet.Column).Where(e => e.Attribute(VExcelNS.SpreadSheet.Span) != null)) {
                XAttribute attr = col.Attribute(VExcelNS.SpreadSheet.Span);
                if (attr != null) {
                    int span = Convert.ToInt32(attr.Value);
                    attr.Remove();
                    for (int i = 0; i < span; i++) {
                        XElement colNew = new XElement(col);
                        colNew.RemoveAttribute(VExcelNS.SpreadSheet.Index);
                        col.AddAfterSelf(colNew);
                    }
                }
            }
        }
        public VExcelRow Row(int index)
        {
            XElement lastBefore=null;
            XElement xrow = VExcelCommon.GetElementByIndex(this.Element, VExcelNS.SpreadSheet.Row, index, ref lastBefore);
            if (xrow == null) {
                xrow = new XElement(VExcelNS.SpreadSheet.Row, new XAttribute(VExcelNS.SpreadSheet.Index, (index + 1).ToString()));
                if (lastBefore == null) {
                    this.Element.AddFirst(xrow);
                } else {
                    lastBefore.AddAfterSelf(xrow);
                }
                XAttribute attr = this.Element.Element(VExcelNS.SpreadSheet.Table).Attribute(VExcelNS.SpreadSheet.ExpandedRowCount);
                int i = Convert.ToInt32(attr.Value);
                if (i <= index) {
                    attr.Value = (index + 1).ToString();
                }
            }
            VExcelRow row = new VExcelRow(xrow, this);
            return row;
        }
        public VExcelColumn Column(int index)
        {
            XElement lastBefore = null;
            XElement xcolumn = VExcelCommon.GetElementByIndex(this.Element, VExcelNS.SpreadSheet.Column, index, ref lastBefore);
            if (xcolumn == null) {
                xcolumn = createColumnElement(index + 1);
                // lastBefore.AddAfterSelf(xcolumn);
                if (lastBefore == null) {
                    this.Element.Element(VExcelNS.SpreadSheet.Table).AddFirst(xcolumn);
                } else {
                    lastBefore.AddAfterSelf(xcolumn);
                }
                XAttribute attr = this.Element.Element(VExcelNS.SpreadSheet.Table).Attribute(VExcelNS.SpreadSheet.ExpandedColumnCount);
                int i = Convert.ToInt32(attr.Value);
                if (i <= index) {
                    attr.Value = (index + 1).ToString();
                }
            }
            VExcelColumn column = new VExcelColumn(xcolumn, this);
            return column;
        }
        public VExcelCell FindCell(string value, int startColIndex = 0)
        {
            IList<XElement> xcells = this.Element.Descendants(VExcelNS.SpreadSheet.Cell).Where(e => e.Value.Contains(value)).ToList();
            int colIndex = 0;
            XElement xcell = null;
            foreach (XElement xcell1 in xcells) {
                colIndex = VExcelCommon.GetIndex(xcell1);
                if (colIndex >= startColIndex) {
                    xcell = xcell1;
                    break;
                }
            }
            VExcelCell cell = null;
            if (xcell != null) {
                int rowIndex = VExcelCommon.GetIndex(xcell.Parent);
                cell = this.Row(rowIndex).Cell(colIndex, ref this.ret);
            }
            return cell;
        }
        private XElement ret;
        public List<VExcelCell> FindCells(string value)
        {
            List<VExcelCell> cells = new List<VExcelCell>();
            foreach (XElement xcell in this.Element.Descendants(VExcelNS.SpreadSheet.Cell).Where(e => e.Value.Contains(value))) {
                    int rowIndex = VExcelCommon.GetIndex(xcell.Parent);
                    int colIndex = VExcelCommon.GetIndex(xcell);
                    cells.Add(this.Row(rowIndex).Cell(colIndex, ref this.ret));
            }
            return cells;
        }
        public void Replace(string value, string newValue)
        {
            List<VExcelCell> cells = this.FindCells(value);
            foreach (VExcelCell cell in cells) {
                cell.SetValue(cell.Value.Replace(value, newValue));
            }
        }
        private VExcelColumn insertColumn(int index)
        {
            VExcelColumn column1 = this.Column(index);
            XElement xcolumn = createColumnElement(index + 1);
            column1.Element.AddBeforeSelf(xcolumn);
            VExcelCommon.IncrementIndexAfter(xcolumn);
            column1 = this.Column(index);
            XAttribute attr = this.Element.Element(VExcelNS.SpreadSheet.Table).Attribute(VExcelNS.SpreadSheet.ExpandedColumnCount);
            int i = Convert.ToInt32(attr.Value);
            attr.Value = (i + 1).ToString();
            return column1;
        }
        public VExcelColumn InsertColumn(int index, VExcelColumn column)
        {
            VExcelColumn column1 = insertColumn(index);
            Cmn.CopyAttribute(column.Element, column1.Element, VExcelNS.SpreadSheet.AutoFitWidth);
            Cmn.CopyAttribute(column.Element, column1.Element, VExcelNS.SpreadSheet.Width);
            Cmn.CopyAttribute(column.Element, column1.Element, VExcelNS.SpreadSheet.Hidden);
            return column1;
        }
        private static XElement createColumnElement(int index)
        {
            XElement column = new XElement(VExcelNS.SpreadSheet.Column, new XAttribute(VExcelNS.SpreadSheet.Index, index.ToString()));
            return column;
        }
    }
}
