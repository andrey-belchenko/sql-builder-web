using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.ExcelApi
{
    public sealed class VExcelCell : VExcelObject
    {
        public readonly VExcelRow Row;
        public VExcelCell(XElement element, VExcelRow row)
            : base(element)
        {
            this.Row = row;
        }
        public VExcelSheet Sheet
        {
            get
            {
                return this.Row.Sheet;
            }
        }
        public int RowIndex
        {
            get
            {
                return this.Row.Index;
            }
        }
        public int Index
        {
            get
            {
                return VExcelCommon.GetIndex(this.Element);
            }
        }
        public string Value
        {
            get
            {
                XElement e = this.Element.Element(VExcelNS.SpreadSheet.Data);
                if (e == null)
                {
                    return null;
                }
                else
                {
                    return e.Value;
                }
            }
            /*set {
                if (value == null) {
                    this.Element.Elements().Remove();
                    return;
                }
                XElement d = this.Element.Element(VExcelNS.SpreadSheet.Data);
                if (d == null) {
                    d = new XElement(VExcelNS.SpreadSheet.Data);
                    this.Element.Add(d);
                }
                VExcelCell cell = value as VExcelCell;
                if (cell != null) {
                    this.Element.Elements().Remove();
                    XElement data = cell.Element.Elements().FirstOrDefault();
                    if (data != null) {
                        this.Element.Add(new XElement(data));
                    }
                    Cmn.CopyAttribute(cell.Element, this.Element, VExcelNS.SpreadSheet.StyleID);
                    Cmn.CopyAttribute(cell.Element, this.Element, VExcelNS.SpreadSheet.MergeAcross);
                    Cmn.CopyAttribute(cell.Element, this.Element, VExcelNS.SpreadSheet.MergeDown);
                    Cmn.CopyAttribute(cell.Element, this.Element, VExcelNS.SpreadSheet.Formula);
                } else {
                    d.Value = value.ToString();
                    d.SetAttributeValue(VExcelNS.SpreadSheet.Type, "String"); // Сделать определение типа
                }
            }*/
        }
        public void SetValue(string text)
        {
            if (text == null)
            {
                this.Element.Elements().Remove();
                return;
            }
            XElement data = this.Element.Element(VExcelNS.SpreadSheet.Data);
            if (data == null)
            {
                data = new XElement(VExcelNS.SpreadSheet.Data, new XAttribute(VExcelNS.SpreadSheet.Type, "String"));
                this.Element.Add(data);
            }
            else
            {
                data.SetAttrValue(VExcelNS.SpreadSheet.Type, "String"); // Сделать определение типа
            }
            data.Value = text;
        }
        public void SetValue(VExcelCell cell)
        {
            if (cell == null)
            {
                this.Element.Elements().Remove();
                return;
            }
            this.Element.Elements().Remove();
            XElement data = cell.Element.Elements().FirstOrDefault();
            if (data != null)
            {
                this.Element.Add(new XElement(data));
            }
            Cmn.CopyAttribute(cell.Element, this.Element, VExcelNS.SpreadSheet.StyleID);
            Cmn.CopyAttribute(cell.Element, this.Element, VExcelNS.SpreadSheet.MergeAcross);
            Cmn.CopyAttribute(cell.Element, this.Element, VExcelNS.SpreadSheet.MergeDown);
            Cmn.CopyAttribute(cell.Element, this.Element, VExcelNS.SpreadSheet.Formula);
        }
        public VExcelRange Insert(VExcelRange range) // Пока только со сдвигом вправо
        {
            List<List<VExcelCell>> rows = range.Data();
            List<VExcelColumn> columns = range.Columns();
            int cellRowIndex = this.RowIndex;
            int cellColIndex = this.Index;
            int j1 = cellColIndex;
            for (int j = 0; j < columns.Count; j++)
            {
                range.FirstCell.Sheet.InsertColumn(j1, columns[j]);
                j1++;
            }
            int i1 = cellRowIndex;
            for (int i = 0; i < rows.Count; i++)
            {
                j1 = cellColIndex;
                VExcelRow tagRow = this.Sheet.Row(i1);
                for (int j = 0; j < rows[i].Count; j++)
                {
                    bool merge = false;
                    int mergeAdd = 0;
                    if (rows[i][0] != null)
                    {
                        if (rows[i][0].Element.ToString().Contains("[merge]"))
                        {
                            merge = true;
                            // mergeAdd = VExcelCommon.GetMergeAcrossAttrVal(rows[i][j].Element); // !!! Пытался учесть merge - не получилось, пока таких случаев (примера) нет
                        }
                        else if (VExcelCommon.GetMergeAcrossAttrVal(rows[i][0].Element) > rows[i].Count - 1)
                        {
                            merge = true;
                        }
                    }
                    if (merge)
                    {
                        int incr = 1 + mergeAdd;
                        VExcelCommon.SetMergeAcrossAttrVal(rows[i][0].Element, VExcelCommon.GetMergeAcrossAttrVal(rows[i][0].Element) + incr);
                        VExcelCommon.IncrementIndexAfter(this.Sheet.Row(i1), j1, 1);
                        //for (int i2 = 0; i2 <= VExcelCommon.GetMergeDownAttrVal(rows[i][0].Element); i2++)
                        //{
                        //    VExcelRow r = rows[i][0].Row.Sheet.Row(i2 + i1);
                        //    VExcelCommon.IncrementIndexAfter(r, j1,incr);
                        //}
                    }
                    else
                    {
                        if (rows[i][j] != null)
                        {
                            tagRow.InsertCell(j1, rows[i][j]);
                        }
                        else
                        {
                            if (rows[i][0] == null)
                            {
                                XElement lastBefore = null;
                                VExcelCell cell1 = this.Sheet.Row(i1).Cell(j1, ref lastBefore, true);
                                if (lastBefore != null)
                                {
                                    // VExcelCommon.IncrementIndexAfter(lastBefore, 1);
                                    int m = VExcelCommon.GetMergeAcrossAttrVal(lastBefore);
                                    int ind = VExcelCommon.GetIndex(lastBefore);
                                    if (ind + m >= cellColIndex)
                                    {
                                        VExcelCommon.SetMergeAcrossAttrVal(lastBefore, m + 1);
                                    }
                                    //} else {
                                    //    VExcelCommon.IncrementIndexAfter(this.Row.Sheet.Row(i1), j1, 1);
                                }
                            }
                            else
                            {
                                // VExcelCommon.IncrementIndexAfter(this.Row.Sheet.Row(i1), j1, 1);
                            }
                            VExcelCommon.IncrementIndexAfter(this.Sheet.Row(i1), j1, 1);
                        }
                    }
                    j1++;
                }
                i1++;
            }
            VExcelCell firstCell = this;
            VExcelRange newRange = new VExcelRange(this.Row.Cell(cellColIndex, ref ret), this.Sheet.Row(cellRowIndex + rows.Count - 1).Cell(cellColIndex + columns.Count - 1, ref ret));
            return newRange;
        }
        private XElement ret; // чтобы не обявлять каждый раз при вызове .Cell
        public void Remove()
        {
            int i = VExcelCommon.GetMergeAcrossAttrVal(this.Element);
            //VExcelCommon.IncrementIndexAfter(this.Element, -1-i);
            VExcelCommon.IncrementIndexAfter(this.Element, -1);
            if (i > 0)
            {
                VExcelCommon.SetMergeAcrossAttrVal(this.Element, i - 1);
            }
            else
            {
                this.Element.Remove();
            }
        }
    }
}