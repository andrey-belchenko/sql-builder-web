using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;
using sql.builder.DataApi; // XElementExtensions

namespace sql.builder.ExcelApi
{
    public static class VExcelCommon
    {
        public static int GetIndex(XElement element)
        {
            Contract.Assert(element != null);
            Contract.Assert(element.Name.Namespace == VExcelNS.ss);
            XAttribute attr = element.Attribute(VExcelNS.SpreadSheet.Index);
            if (attr != null) {
                return Convert.ToInt32(attr.Value) - 1;
            }
            XName name = element.Name;
            XElement elementWithIndex = element.ElementsBeforeSelf(name).LastOrDefault(IsHasIndex);
            int i = 0;
            if (elementWithIndex != null) {
                i = GetIndexAttrVal(elementWithIndex) - 1;
            } else {
                elementWithIndex = element.ElementsBeforeSelf(name).FirstOrDefault();
                if (elementWithIndex == null) {
                    return 0;
                }
            }
            int index = i + element.ElementsBeforeSelf(name).Where(e => e.IsAfter(elementWithIndex)).Count() + 1;
            if (name == VExcelNS.SpreadSheet.Cell) {
                int mergedCount = GetMergeAcrossAttrVal(elementWithIndex);
                foreach (XElement el in element.ElementsBeforeSelf(name).Where(e => e.IsAfter(elementWithIndex))) {
                    mergedCount += GetMergeAcrossAttrVal(el);
                }
                index += mergedCount;
            }
            return index;
        }
        public static XElement GetElementByIndex(XElement parentElement, XName name, int index, ref XElement lastBefore)
        {
            XElement element = null;
            lastBefore = null;
            int i = 0;
            foreach (XElement el in parentElement.Descendants(name)) {
                int elInd = GetIndexAttrVal(el);
                if (elInd != -1) {
                    i = elInd - 1;
                }
                if (i == index) {
                    return el;
                }
                i++;
                i += GetMergeAcrossAttrVal(el);
                if (i > index) {
                    if (GetMergeAcrossAttrVal(el) != 0 && elInd <= index) {
                       lastBefore = el;
                    }
                    return null;
                }
                lastBefore = el;
            }
            return element;
        }
      /*  public static XElement GetElementByIndex(XElement parentElement, string elementName,int index, ref XElement lastBefore )
        {

            XElement elementWithIndex = parentElement.Descendants(VExcelNS.ss + elementName).LastOrDefault(e =>  IsHasIndex(e) && GetIndexAttrVal(e)-1 <= index );
            XElement element=null;
             XElement nextElementWithIndex;
             int index1=0;
            if (elementWithIndex != null)
            {
                 index1 = GetIndexAttrVal(elementWithIndex) - 1;

                if (index1 == index)
                {
                    return elementWithIndex;
                }
                nextElementWithIndex = elementWithIndex.ElementsAfterSelf(VExcelNS.ss + elementName).FirstOrDefault(e =>  IsHasIndex(e));
            }
            else
            {

                 nextElementWithIndex = parentElement.Descendants(VExcelNS.ss + elementName).FirstOrDefault(e =>  IsHasIndex(e));
             
            }

            if (nextElementWithIndex != null)
            {
                if (elementWithIndex != null)
                {
                    element = GetXElementByIndex(elementWithIndex.ElementsAfterSelf(VExcelNS.ss + elementName).Where(e => e.IsBefore(nextElementWithIndex)), index - index1 - 1);
                    if (element == null)
                    {
                        lastBefore = elementWithIndex.ElementsAfterSelf(VExcelNS.ss + elementName).Where(e => e.IsBefore(nextElementWithIndex)).LastOrDefault();
                    }
                }
                else
                {
                    element = GetXElementByIndex(parentElement.Descendants(VExcelNS.ss + elementName).Where(e => e.IsBefore(nextElementWithIndex)), index);
                    if (element == null)
                    {
                        lastBefore = parentElement.Descendants(VExcelNS.ss + elementName).Where(e => e.IsBefore(nextElementWithIndex)).LastOrDefault();
                    }
                }
            }
            else
            {
                if (elementWithIndex != null)
                {
                    element = GetXElementByIndex(elementWithIndex.ElementsAfterSelf(VExcelNS.ss + elementName), index - index1 - 1);
                    if (element == null)
                    {
                        lastBefore = elementWithIndex.ElementsAfterSelf(VExcelNS.ss + elementName).LastOrDefault();
                    }

                }
                else
                {
                    element = GetXElementByIndex(parentElement.Descendants(VExcelNS.ss + elementName), index);
                    if (element == null)
                    {
                        lastBefore = parentElement.Descendants(VExcelNS.ss + elementName).LastOrDefault();
                    }
                }

            }
            if (element == null)
            {
                if (lastBefore == null)
                {
                    lastBefore = elementWithIndex;
                }
            }
                
            
            return element;
        }*/
        public static void IncrementIndexAfter(XElement cell, int i = 1)
        {
            XName name = cell.Name;
            XElement next = cell.ElementsAfterSelf(name).FirstOrDefault();
            if (next != null) {
                if (IsHasIndex(cell) && !IsHasIndex(next) && i < 0 ) {
                    next.SetAttrValue(VExcelNS.SpreadSheet.Index, GetIndexAttrVal(cell) + 1 + GetMergeAcrossAttrVal(cell));
                }
            }
            foreach (XElement el in cell.ElementsAfterSelf(name).Where(IsHasIndex)) {
                int index = GetIndexAttrVal(el);
                el.SetAttrValue(VExcelNS.SpreadSheet.Index, index + i);
            }
        }
        public static void IncrementIndexAfter(VExcelRow row, int start, int i = 1)
        {
            IEnumerable<XElement> cells = row.Element.Elements().Where(e => GetIndexAttrVal(e) > start);
            foreach (XElement el in cells) {
                int index = GetIndexAttrVal(el);
                el.SetAttrValue(VExcelNS.SpreadSheet.Index, index + i);
            }
        }
        public static int GetIndexAttrVal(XElement element)
        {
            return element.AttrOrDefault(VExcelNS.SpreadSheet.Index, -1);
        }
        public static int GetMergeAcrossAttrVal(XElement element)
        {
            return element.AttrOrDefault(VExcelNS.SpreadSheet.MergeAcross, 0);
        }
        public static int GetMergeDownAttrVal(XElement element)
        {
            return element.AttrOrDefault(VExcelNS.SpreadSheet.MergeDown, 0);
        }
        public static void SetMergeAcrossAttrVal(XElement element, int val)
        {
            if (val == 0) {
                element.RemoveAttribute(VExcelNS.SpreadSheet.MergeAcross);
            } else {
                element.SetAttrValue(VExcelNS.SpreadSheet.MergeAcross, val);
            }
        }
        public static void SetMergeDownAttrVal(XElement element, int val)
        {
            if (val == 0) {
                element.RemoveAttribute(VExcelNS.SpreadSheet.MergeDown);
            } else {
                element.SetAttrValue(VExcelNS.SpreadSheet.MergeDown, val);
            }
        }
        public static bool IsHasIndex(XElement element)
        {
            return element.Attribute(VExcelNS.SpreadSheet.Index) != null;
        }
        public static XElement GetXElementByIndex(IEnumerable<XElement> elements, int index)
        {
            if (elements.Count() > index) {
                return elements.ElementAt(index);
            } else {
                return null;
            }
        }
    }
}
