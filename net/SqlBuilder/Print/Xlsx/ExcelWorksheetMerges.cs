using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.Print.Xlsx
{
    public class ExcelWorksheetMerges
    {
        private List<ExcelWorksheetMerge> _merges;
        public ExcelWorksheetMerges()
        {
            this._merges = new List<ExcelWorksheetMerge>();
        }
        public ExcelWorksheetMerges(XElement xitem)
        {
            Contract.Assert(xitem != null);
            Contract.Assert(xitem.Name == ns.Main.mergeCells);
            XAttribute attr = xitem.Attribute(ns.None.count);
            if (attr != null)
            {
                this._merges = new List<ExcelWorksheetMerge>(Convert.ToInt32(attr.Value));
            }
            else
            {
                this._merges = new List<ExcelWorksheetMerge>();
            }
            foreach (XElement xmerge in xitem.Elements(ns.Main.mergeCell))
            {
                this._merges.Add(new ExcelWorksheetMerge(xmerge.Attribute(ns.None.ref_).Value));
            }
        }
        public ExcelWorksheetMerges(ExcelWorksheetMerges merges)
        {
            Contract.Assert(merges != null);
            int count = merges._merges.Count;
            this._merges = new List<ExcelWorksheetMerge>(count);
            for (int index = 0; index < count; index++)
            {
                merges._merges[index].CopyTo(this._merges);
            }
        }
        public void CopyRowMerge(int rowIdOld, int rowIdNew)
        {
            for (int index = 0; index < this._merges.Count; index++)
            {
                ExcelWorksheetMerge merge = this._merges[index];
                if (merge.BeginsFromRow(rowIdOld))
                {
                    merge.CopyRowMerge(rowIdNew);
                }
            }
        }
        public void DeleteRowMerge(int rowId)
        {
            for (int index = 0; index < this._merges.Count; index++)
            {
                ExcelWorksheetMerge merge = this._merges[index];
                if (merge.BeginsFromRow(rowId))
                {
                    merge.Delete();
                }
            }
        }
        public void DeleteColumn(string colName)
        {
            for (int index = 0; index < this._merges.Count; index++)
            {
                ExcelWorksheetMerge merge = this._merges[index];
                if (!merge.Deleted)
                {
                    merge.DeleteColumn(colName);
                }
            }
        }
        public void RenameColumn(string colNameOld, string colNameNew)
        {
            for (int index = 0; index < this._merges.Count; index++)
            {
                ExcelWorksheetMerge merge = this._merges[index];
                if (!merge.Deleted)
                {
                    merge.RenameColumn(colNameOld, colNameNew);
                }
            }
        }
        public XElement GetXml()
        {
            XElement xmerges = new XElement(ns.Main.mergeCells);
            uint count = 0;
            for (int index = 0; index < this._merges.Count; index++)
            {
                count = count + this._merges[index].CopyTo(xmerges);
            }
            xmerges.Add(new XAttribute(ns.None.count, count.ToString()));
            return xmerges;
        }
        public bool IsBeginOfMerge(ExcelCellInfo cell)
        {
            return this._merges.Any(cell.IsBeginOfMerge);
        }
        public bool IsEndOfMerge(ExcelCellInfo cell)
        {
            return this._merges.Any(cell.IsEndOfMerge);
        }
        public ExcelWorksheetMerge GetContainedMerge(ExcelCellInfo cell)
        {
            //return _merges.Select(m => m.GetContainedMerge(cell)).FirstOrDefault(m => m != null);
            for (int index = 0; index < this._merges.Count; index++)
            {
                ExcelWorksheetMerge merge = this._merges[index].GetContainedMerge(cell);
                if (merge != null)
                {
                    return merge;
                }
            }
            return null;
        }
        public void ExtendMergeToColumn(ExcelCellInfo cellFrom, ExcelCellInfo cellTo)
        {
            //foreach (ExcelWorksheetMerge merge in this._merges.Where(cellFrom.IsEndOfMerge)) {
            for (int index = 0; index < this._merges.Count; index++)
            {
                ExcelWorksheetMerge merge = this._merges[index];
                if (merge.EndsWith(cellFrom))
                {
                    merge.ExtendToColumn(cellTo.ColumnName);
                }
            }
        }
        public void CreateMerge(ExcelCellInfo cellFrom, ExcelCellInfo cellTo)
        {
            //XElement xmerge = new XElement(ns.Main.mergeCell, new XAttribute("ref", cellFrom.CellName + ":" + cellTo.CellName));
            this._merges.Add(new ExcelWorksheetMerge(cellFrom.CellName + ":" + cellTo.CellName));
        }
        public bool CreateMergeChecked(ExcelCellInfo cellFrom, ExcelCellInfo cellTo)
        {
            // ���� �� ��������� ��� ������ ����� cellFrom � cellTo, �� ����� ��������� �� ������
            //if (check_exists && (this._merges.Any(m => m.ContainsCell(cellFrom) || m.ContainsCell(cellTo)))) return false;
            for (int index = 0; index < this._merges.Count; index++)
            {
                ExcelWorksheetMerge merge = this._merges[index];
                if (merge.ContainsCell(cellFrom) || merge.ContainsCell(cellTo))
                {
                    return false;
                }
            }
            this.CreateMerge(cellFrom, cellTo);
            return true;
        }
        public void RemoveMergesInRange(ExcelCellInfo cellFrom, ExcelCellInfo cellTo)
        {
            var cells = ExcelCellInfo.Range(cellFrom, cellTo).ToList();
            foreach (ExcelCellInfo cell in cells)
            {
                ExcelWorksheetMerge merge = this._merges.FirstOrDefault(cell.ContainsIn);
                if (merge != null)
                {
                    merge = merge.GetContainedMerge(cell);
                    merge.Delete();
                }
            }
        }
        public ExcelCellInfo GetNextMergedCell(ExcelCellInfo cell)
        {
            ExcelWorksheetMerge merge = this._merges.FirstOrDefault(cell.ContainsIn);
            if (merge != null)
            {
                return merge.GetNextMergedCellInfo(cell);
            }
            else
            {
                return null;
            }
        }
    }
    public class ExcelWorksheetMerge
    {
        //private bool copied;
        private bool deleted;
        private List<ExcelWorksheetMerge> _copies;
        private ExcelRefToken _refs;
        public bool Deleted { get { return this.deleted; } }
        public ExcelCellInfo FirstCell { get { return this._refs.Cell1; } }
        public ExcelCellInfo LastCell { get { return this._refs.Cell2; } }
        public ExcelWorksheetMerge(string refs)
        {
            this._refs = new ExcelRefToken(refs);
            //this._copies = new List<ExcelWorksheetMerge>();
        }
        public bool StartsWith(ExcelCellInfo cell)
        {
            if (this._copies != null)
            {
                return this._copies.Any(cell.IsBeginOfMerge);
            }
            else if (this.deleted)
            {
                return false;
            }
            else
            {
                return this.FirstCell.Equals(cell);
            }
        }
        public bool EndsWith(ExcelCellInfo cell)
        {
            if (this._copies != null)
            {
                return this._copies.Any(cell.IsEndOfMerge);
            }
            else if (this.deleted)
            {
                return false;
            }
            else
            {
                return this.LastCell.Equals(cell);
            }
        }
        public bool ContainsCell(ExcelCellInfo cell)
        {
            if (this._copies != null)
            {
                return this._copies.Any(cell.ContainsIn);
            }
            else if (this.deleted)
            {
                return false;
            }
            else
            {
                return this._refs.ContainsCell(cell);
            }
        }
        public ExcelWorksheetMerge GetContainedMerge(ExcelCellInfo cell)
        {
            if (this._copies != null)
            {
                return this._copies.FirstOrDefault(cell.ContainsIn);
            }
            else if (this.deleted)
            {
                return null;
            }
            else if (this._refs.ContainsCell(cell))
            {
                return this;
            }
            else
            {
                return null;
            }
        }
        public ExcelCellInfo GetNextMergedCellInfo(ExcelCellInfo cell)
        {
            if (this._copies != null)
            {
                ExcelWorksheetMerge copy = this._copies.FirstOrDefault(cell.ContainsIn);
                if (copy != null)
                {
                    return copy.GetNextMergedCellInfo(cell);
                }
                else
                {
                    return null;
                }
            }
            else if (this.deleted)
            {
                // �� ����� ������
                return null;
            }
            else
            {
                return this._refs.GetNextCellInfo(cell);
            }
        }
        public bool BeginsFromRow(int rowId)
        {
            return this.FirstCell.RowID == rowId;
        }
        public void CopyRowMerge(int rowIdNew)
        {
            ExcelCellInfo first_cell = this.FirstCell;
            ExcelCellInfo last_cell = this.LastCell;
            string refs = first_cell.ColumnName + rowIdNew.ToString() + ":" + last_cell.ColumnName + (last_cell.RowID + (rowIdNew - first_cell.RowID)).ToString();
            if (this._copies == null)
            {
                this._copies = new List<ExcelWorksheetMerge>(1);
            }
            this._copies.Add(new ExcelWorksheetMerge(refs));
        }
        public void DeleteColumn(string colName)
        {
            this._refs.DeleteColumn(colName);
            // ���� �� �������� ��������� ����� - �������
            if (this._refs.IsEmpty())
            {
                this.Delete();
            }
        }
        public void RenameColumn(string colNameOld, string colNameNew)
        {
            this._refs.RenameColumn(colNameOld, colNameNew);
        }
        public void ExtendToColumn(string colName)
        {
            this._refs.ExtendToColumn(colName);
        }
        public void Delete()
        {
            this.deleted = true;
            this._copies = null;
        }
        public void CopyTo(IList<ExcelWorksheetMerge> list)
        {
            if (this.deleted)
            {
                return;
            }
            else if (this._copies != null)
            {
                for (int index = 0; index < this._copies.Count; index++)
                {
                    this._copies[index].CopyTo(list);
                }
            }
            else
            {
                string refs = this._refs.GetText();
                if (refs != null)
                {
                    list.Add(new ExcelWorksheetMerge(refs));
                }
            }
        }
        public uint CopyTo(XElement xmerges)
        {
            XElement xmerge;
            if (this.deleted)
            {
                return 0;
            }
            else if (this._copies != null)
            {
                uint count = 0;
                for (int index = 0; index < this._copies.Count; index++)
                {
                    xmerge = this._copies[index].GetXml();
                    if (xmerge != null)
                    {
                        xmerges.Add(xmerge);
                        count++;
                    }
                }
                return count;
            }
            else
            {
                xmerge = this.GetXml();
                if (xmerge != null)
                {
                    xmerges.Add(xmerge);
                    return 1;
                }
                else
                {
                    return 0;
                }
            }
        }
        /*public IEnumerable<XElement> GetMergesXml()
        {
            if (this.deleted) {
                return Enumerable.Empty<XElement>();
            } else if (this.copied) {
                return this._copies.Select(m => m.GetXml()).Where(x => x != null).AsEnumerable();
            } else {
                //return Enumerable.Repeat(this.GetXml(), 1).Where(x => x != null);
                XElement el = this.GetXml();
                if (el != null) {
                    return Enumerable.Repeat(el, 1);
                } else {
                    return Enumerable.Empty<XElement>();
                }
            }
        }*/
        private XElement GetXml()
        {
            if (this.deleted)
            {
                return null;
            }
            else
            {
                string refs = this._refs.GetText();
                if (refs != null)
                {
                    return new XElement(ns.Main.mergeCell, new XAttribute(ns.None.ref_, refs));
                }
                else
                {
                    return null;
                }
            }
        }
    }
}