using System;
using System.Xml.Linq;

namespace sql.builder.ExcelApi
{
    internal sealed class VExcelRow : VExcelObject
    {
        internal readonly VExcelSheet Sheet;
        internal VExcelRow(XElement element, VExcelSheet sheet)
            : base (element)
        {
            this.Sheet = sheet;
        }
        internal int Index {
            get {
                return VExcelCommon.GetIndex(this.Element);
            }
        }
        public VExcelCell Cell(int index)
        {
            XElement r = null;
            return this.Cell(index, ref r, false);
        }
        public VExcelCell Cell(int index, ref XElement lastBefore, bool doNotCreate = false)
        {
           // XElement lastBefore = null;
            lastBefore = null;
            XElement xcell = VExcelCommon.GetElementByIndex(this.Element, VExcelNS.SpreadSheet.Cell, index, ref lastBefore);
            if (xcell == null) {
                if (doNotCreate) {
                    return null;
                }
                xcell = createCellElement(index+1);
                if (lastBefore == null) {
                    this.Element.AddFirst(xcell);
                } else {
                    if (VExcelCommon.GetMergeAcrossAttrVal(lastBefore) != 0) {
                        return null;
                    }
                    lastBefore.AddAfterSelf(xcell);
                }
                // VExcelCommon.IncrementIndexAfter(xcell);
                XAttribute attr = this.Sheet.Element.Element(VExcelNS.SpreadSheet.Table).Attribute(VExcelNS.SpreadSheet.ExpandedColumnCount);
                int i = Convert.ToInt32(attr.Value);
                if (i <= index) {
                    attr.Value = (index + 1).ToString();
                }
            }
            VExcelCell cell = new VExcelCell(xcell, this);
            return cell;
        }
        private static XElement createCellElement(int index)
        {
            XElement cell = new XElement(VExcelNS.SpreadSheet.Cell);
            cell.Add(new XAttribute(VExcelNS.SpreadSheet.Index, index.ToString()));
            cell.Add(new XElement(VExcelNS.SpreadSheet.Data, new XAttribute(VExcelNS.SpreadSheet.Type, "String")));
            return cell;
        }
        private XElement ret;// чтобы не обявлять каждый раз при вызове .Cell
       /* private  VExcelCell insertCell(int index)
        {
            VExcelCell cell1 = this.Cell(index,ref ret);
            VExcelCell cell2 = null;
            if (cell1 != null)
            {
                VExcelCell prev =new VExcelCell( cell1.Element.ElementsBeforeSelf().Last());
                if (!Cmn.Nvl(prev.Value, "").ToString().Contains("[merge]"))
                {
                    XElement xcell = createCellElement(index + 1);
                   
                    cell1.Element.AddBeforeSelf(xcell);
                    VExcelCommon.IncrementIndexAfter(xcell);
                    cell1 = this.Cell(index, ref ret);
                    cell2 = cell1;
                }
                else
                {
                    VExcelCommon.SetMergeAcrossAttrVal(prev.Element, VExcelCommon.GetMergeAcrossAttrVal(prev.Element)+1);
                    VExcelCommon.IncrementIndexAfter(cell1.Element);
                    cell2 = cell1;
                    cell1 = null;
                }
                
            }
            else
            {
              //  VExcelCell 
                cell2 = new VExcelCell(ret);
               // int i =cell2.Index();
                VExcelCommon.SetMergeAcrossAttrVal(ret,VExcelCommon.GetMergeAcrossAttrVal(ret) + 1);
                VExcelCommon.IncrementIndexAfter(ret);
                for (int i = 1; i <= VExcelCommon.GetMergeDownAttrVal(cell2.Element); i++)
                {
                    VExcelRow r = this.Sheet.Row(i + this.Index());
                    VExcelCommon.IncrementIndexAfter(r, index);
                }
            }
            

                return cell1;
        }*/
        private VExcelCell insertCell(int index)
        {
            VExcelCell prevCell = this.Cell(index - 1, ref ret, true);//!!!Для 1 колонки работать не будет, потом доделать
            //!!! изменил doNotCreate на true, может что-то сломаться
            XElement prevElement = null;
            if (prevCell != null) {
                prevElement = prevCell.Element;
            } else {
                prevElement = ret;
            }
            XElement xcell = createCellElement(index + 1);
            prevElement.AddAfterSelf(xcell);
            VExcelCommon.IncrementIndexAfter(xcell);
            VExcelCell newCell = this.Cell(index, ref ret);
            //VExcelCell cell2 = null;
            //if (prevCell != null)
            //{
            //    VExcelCell prev = new VExcelCell(prevCell.Element.ElementsBeforeSelf().Last());
            //    if (!Cmn.Nvl(prev.Value, "").ToString().Contains("[merge]"))
            //    {
            //        prevCell = this.Cell(index, ref ret);
            //        cell2 = prevCell;
            //    }
            //    else
            //    {
            //        VExcelCommon.SetMergeAcrossAttrVal(prev.Element, VExcelCommon.GetMergeAcrossAttrVal(prev.Element) + 1);
            //        VExcelCommon.IncrementIndexAfter(prevCell.Element);
            //        cell2 = prevCell;
            //        prevCell = null;
            //    }
            //}
            //else
            //{
            //    //  VExcelCell 
            //    cell2 = new VExcelCell(ret);
            //    // int i =cell2.Index();
            //    VExcelCommon.SetMergeAcrossAttrVal(ret, VExcelCommon.GetMergeAcrossAttrVal(ret) + 1);
            //    VExcelCommon.IncrementIndexAfter(ret);
            //    for (int i = 1; i <= VExcelCommon.GetMergeDownAttrVal(cell2.Element); i++)
            //    {
            //        VExcelRow r = this.Sheet.Row(i + this.Index());
            //        VExcelCommon.IncrementIndexAfter(r, index);
            //    }
            //}
            return newCell;
        }
       public VExcelCell InsertCell(int index, VExcelCell cell)
       {
           VExcelCell cell1 = null;
           if (cell != null) {
               cell1 = this.insertCell(index);
               if (cell1 != null) {
                   cell1.SetValue(cell);
                   //VExcelCommon.SetMergeDownAttrVal(cell1.Element, VExcelCommon.GetMergeDownAttrVal(cell.Element));
                   //int ma = VExcelCommon.GetMergeAcrossAttrVal(cell1.Element);
                   //if (ma > 0) {
                       //VExcelCommon.IncrementIndexAfter(cell1.Row,index+ma, ma);
                       //var ee = 888;
                       //int ii = VExcelCommon.GetMergeAcrossAttrVal(cell.Element);
                       //IEnumerable<XElement> cells = cell.Row.Element.Elements().Where(e => VExcelCommon.GetIndexAttrVal(e) >= index + 1);
                       //foreach (XElement el in cells)
                       //{
                       //    int index1 =VExcelCommon.GetIndexAttrVal(el);
                       //    el.SetAttributeValue(VExcelNS.SpreadSheet.Index, index1 + ii);
                       //}
                   //}
                   //for (int i = 1; i <= VExcelCommon.GetMergeDownAttrVal(cell.Element); i++)
                   //{
                   //    VExcelRow r = this.Sheet.Row(i + this.Index());
                   //    VExcelCommon.IncrementIndexAfter(r, index+ma, ma+ 1);
                   //}
               }
           }
            return cell1;
       }
    }
}
