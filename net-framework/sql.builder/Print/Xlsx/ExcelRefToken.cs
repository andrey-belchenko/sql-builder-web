using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;
using System.Linq;

namespace sql.builder.Print.Xlsx
{
    internal class ExcelRefToken : IToken
    {
        private Dictionary<string, ExcelCellInfo> _refCells;
        private bool is_range;
        private ExcelCellInfo cell_1, cell_2;
        //private bool changed;
        internal ExcelCellInfo Cell1 { get { return this.cell_1; } }
        internal ExcelCellInfo Cell2 { get { return this.cell_2; } }
        // расшир€ть можно только диапазоны
        internal bool IsRange { get { return this.is_range; } }
        internal ExcelRefToken(string ref_range)
        {
            Contract.Assert(!string.IsNullOrEmpty(ref_range));
            // ref_range может иметь вид как A1:D2 так и просто A1
            int colon_pos = ref_range.IndexOf(':');
            ExcelCellInfo cell;
            if (colon_pos < 0) {
                this.is_range = false;
                cell = new ExcelCellInfo(ref_range);
                this.cell_1 = cell;
                this.cell_2 = cell;
                this._refCells = new Dictionary<string, ExcelCellInfo>(1);
                this._refCells.Add(ref_range, cell);
            } else {
                this.is_range = true;
                string cell_name_1 = ref_range.Substring(0, colon_pos);
                string cell_name_2 = ref_range.Substring(colon_pos + 1);
                int beg_row, beg_col, end_row, end_col;
                string column_name;
                ExcelUtils.ParseCellName(cell_name_1, out beg_row, out beg_col, out column_name);
                ExcelUtils.ParseCellName(cell_name_2, out end_row, out end_col, out column_name);
                this._refCells = new Dictionary<string, ExcelCellInfo>();
                for (int row = beg_row; row <= end_row; row++) {
                    for (int col = beg_col; col <= end_col; col++) {
                        cell = new ExcelCellInfo(row, col);
                        this._refCells.Add(cell.CellName, cell);
                        if (row == beg_row && col == beg_col) {
                            this.cell_1 = cell;
                        }
                        if (row == end_row && col == end_col) {
                            this.cell_2 = cell;
                        }
                    }
                }
                //this.UpdateBorderCells();
            }
        }
        public override string ToString()
        {
            return this.GetText();
        }
        public string GetText()
        {
            //this.changed = false;
            // все €чейки из диапазона удалены
            if (this.IsEmpty()) {
                return null;
            }
            if (this.is_range) {
                return this.Cell1.CellName + ":" + this.Cell2.CellName;
            } else {
                return this.Cell1.CellName;
            }
        }
        internal void DeleteColumn(string colName)
        {
            this.DeleteColumn(ExcelUtils.GetColumnNumber(colName));
        }
        internal void DeleteColumn(int colId)
        {
            // оптимизаци€
            if (colId < this.cell_1.ColumnID || colId > this.cell_2.ColumnID) return;
            IList<ExcelCellInfo> cells = this._refCells.Values.Where(c => c.ColumnID == colId).ToList();
            if (cells.Count > 0) {
                for (int index = 0; index < cells.Count; index++) {
                    ExcelCellInfo cell = cells[index];
                    this._refCells.Remove(cell.CellName);
                    //this.changed = true;
                }
                this.UpdateBorderCells();
            }
        }
        internal void RenameColumn(string colNameOld, string colNameNew)
        {
            this.RenameColumn(ExcelUtils.GetColumnNumber(colNameOld), ExcelUtils.GetColumnNumber(colNameNew));
        }
        internal void RenameColumn(int colIdOld, int colIdNew)
        {
            // оптимизаци€
            if (colIdOld < this.cell_1.ColumnID || colIdOld > this.cell_2.ColumnID) return;
            IList<ExcelCellInfo> cells = this._refCells.Values.Where(c => c.ColumnID == colIdOld).ToList();
            for (int index = 0; index < cells.Count; index++) {
                ExcelCellInfo cell = cells[index];
                this._refCells.Remove(cell.CellName);
                cell.ColumnID = colIdNew;
                this._refCells.Add(cell.CellName, cell);
            }
            //this.changed = true;
        }
        internal void ExtendToColumn(string colName)
        {
            this.ExtendToColumn(ExcelUtils.GetColumnNumber(colName));
        }
        internal void ExtendToColumn(int col2)
        {
            for (int i = this.cell_1.RowID; i <= this.cell_2.RowID; i++) {
                for (int j = this.cell_2.ColumnID + 1; j <= col2; j++) {
                    ExcelCellInfo cell = new ExcelCellInfo(i, j);
                    this._refCells.Add(cell.CellName, cell);
                }
            }
            this.UpdateBorderCells();
            //this.changed = true;
        }
        internal ExcelCellInfo GetNextCellInfo(ExcelCellInfo cell)
        {
            // вернет следующую за cell €чейку. ≈сли cell последн€€ или ее нет в списке - вернет null
            //return _refCells.Values.SkipWhile(r => r.CellName != cell.CellName).Skip(1).FirstOrDefault();
            if (this.IsEmpty()) {
                return null;
            }
            int row, col;
            cell.GetRowAndColumn(out row, out col);
            ExcelCellInfo rightCell = new ExcelCellInfo(row, col + 1);
            if (this._refCells.ContainsKey(rightCell.CellName)) {
                return rightCell;
            }
            ExcelCellInfo bottomCell = new ExcelCellInfo(row + 1, this.cell_1.ColumnID);
            if (this._refCells.ContainsKey(bottomCell.CellName)) {
                return bottomCell;
            } else {
                return null;
            }
        }
        internal void Move(int col_delta)
        {
            IList<ExcelCellInfo> cells = this._refCells.Values.ToList();
            this._refCells.Clear();
            for (int index = 0; index < cells.Count; index++) {
                ExcelCellInfo cell = cells[index];
                cell.ColumnID = cell.ColumnID + col_delta;
                this._refCells.Add(cell.CellName, cell);
            }
            //this.changed = true;
        }
        internal bool ContainsColumn(int colId)
        {
            return this._refCells.Values.Any(c => c.ColumnID == colId);
        }
        internal bool ContainsCell(ExcelCellInfo cell)
        {
            return this._refCells.ContainsKey(cell.CellName);
        }
        internal bool IsEmpty()
        {
            return this._refCells.Count == 0;
        }
        private void UpdateBorderCells()
        {
            this.cell_1 = null;
            this.cell_2 = null;
            foreach (var cell in _refCells.Values) {
                if (this.cell_1 == null || cell.RowID < this.cell_1.RowID || (cell.ColumnID < this.cell_1.ColumnID && cell.RowID == this.cell_1.RowID)) {
                    this.cell_1 = cell;
                }
                if (this.cell_2 == null || cell.RowID > this.cell_2.RowID || (cell.ColumnID > this.cell_2.ColumnID && cell.RowID == this.cell_2.RowID)) {
                    this.cell_2 = cell;
                }
            }
        }
    }
}