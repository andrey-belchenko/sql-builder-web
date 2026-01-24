using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    internal sealed class VExcelRange
    {
        internal readonly VExcelCell FirstCell;
        internal readonly VExcelCell LastCell;
        private readonly VExcelSheet Sheet;
        private int firstRowIndex;
        private int lastRowIndex;
        private int firstColIndex;
        private int lastColIndex;
        internal VExcelRange(VExcelCell firstCell, VExcelCell lastCell)
        {
            this.FirstCell = firstCell;
            this.LastCell = lastCell;
            this.Sheet = firstCell.Sheet;
        }
        internal List<List<VExcelCell>> Data()
        {
            this.firstRowIndex = this.FirstCell.RowIndex;
            this.lastRowIndex = this.LastCell.RowIndex;
            this.firstColIndex = this.FirstCell.Index;
            this.lastColIndex = this.LastCell.Index;
            List<List<VExcelCell>> rows = new List<List<VExcelCell>>();
            XElement ret = null;
            for (int i = firstRowIndex; i <= lastRowIndex; i++) {
                List<VExcelCell> row = new List<VExcelCell>();
                VExcelRow sourceRow = Sheet.Row(i);
                rows.Add(row);
                for (int j = firstColIndex; j <= lastColIndex; j++) {
                    VExcelCell cell = sourceRow.Cell(j, ref ret, true);
                    row.Add(cell);
                }
            }
            return rows;
        }
        internal List<VExcelColumn> Columns()
        {
            int firstColIndex = this.FirstCell.Index;
            int lastColIndex = this.LastCell.Index;
            List<VExcelColumn> columns = new List<VExcelColumn>();
            for (int j = firstColIndex; j <= lastColIndex; j++) {
                VExcelColumn column = this.Sheet.Column(j);
                columns.Add(column);
            }
            return columns;
        }
        internal void Remove()
        {
            foreach (VExcelColumn col in this.Columns()) {
                col.Remove();
            }
            int ir = 0;
            foreach (List<VExcelCell> row in this.Data()) {
                int ic = 0;
                foreach (VExcelCell cell in row) {
                    if (cell != null) {
                        cell.Remove();
                    } else {
                        XElement lastBefore=null;
                        VExcelCell cell1 = this.Sheet.Row(ir+firstRowIndex).Cell(ic+firstColIndex,ref lastBefore,true);
                        if (lastBefore != null) {
                            VExcelCommon.IncrementIndexAfter(lastBefore, -1);
                            int m = VExcelCommon.GetMergeAcrossAttrVal(lastBefore);
                            if (m > 0) {
                                VExcelCommon.SetMergeAcrossAttrVal(lastBefore, m - 1);
                            }
                        } else {
                            XElement cellElem = this.Sheet.Row(ir + firstRowIndex).Element.Elements(VExcelNS.SpreadSheet.Cell).FirstOrDefault();
                            if (cellElem != null) {
                                int index = VExcelCommon.GetIndex(cellElem);
                                cellElem.SetAttributeValue(VExcelNS.SpreadSheet.Index, (index + 1).ToString());
                                VExcelCommon.IncrementIndexAfter(cellElem, -1);
                                cellElem.SetAttributeValue(VExcelNS.SpreadSheet.Index, index.ToString());
                            }
                        }
                    }
                    ic++;
                }
                ir++;
            }
        }
        internal void Replace(string value, string newValue)
        {
            List<List<VExcelCell>> rows = Data();
            foreach (List<VExcelCell> row in rows) {
                foreach (VExcelCell cell in row) {
                    if (cell != null) {
                        if (cell.Value != null) {
                            cell.SetValue(cell.Value.Replace(value, newValue));
                        }
                    }
                }
            }
        }
    }
}