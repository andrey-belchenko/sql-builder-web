using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx
{
    internal class ExcelRow : ExcelBaseItem
    {
        private ExcelWorksheet _worksheet;
        private List<ExcelCell> _cells;
        private ExcelRow prev_row;
        private int row_id;
        private bool head_row;
        internal IList<ExcelCell> Cells { get { return this._cells; } }
        internal ExcelRow PrevRow { get { return this.prev_row; } }
        internal int RowID { get { return this.row_id; } }
        internal bool IsHeadRow { get { return this.head_row; } }
        internal int PrevIDDelta {
            get {
                return (this.prev_row != null) ? (int.Parse(this.ID) - int.Parse(this.prev_row.ID)) : int.Parse(this.ID);
            }
        }
        public ExcelRow(XElement xml, ExcelPrintEnv env, ExcelRow prev, ExcelWorksheet worksheet)
            : base(xml, env)
        {
            this.prev_row = prev;
            this._worksheet = worksheet;
            xml.RemoveAttribute(ns.None.spans);
            this.ID = xml.Attribute(ns.None.r).Value;
            this.row_id = int.Parse(ID);
            this._cells = new List<ExcelCell>();
            IEnumerable<XElement> xcells = xml.Elements(ns.Main.c);
            foreach (XElement xcell in xcells) {
                this._cells.Add(new ExcelCell(xcell, env, this));
            }
            if (this._cells.Any(ExcelCell.IsHeadMarker)) {
                this.head_row = true;
                // нужно в PostProcess чтобы знать откуда начинать merge_down
                //cells_headmarker.ForEach(c => c.SetValue(""));
            }
        }
        internal void DeleteCell(ExcelCell cell)
        {
            this.Xml.Elements(ns.Main.c).First(c => c.Attribute(ns.None.r).Value == cell.CellInfo.CellName).Remove();
            this._cells.Remove(cell);
            // перебрасываем формулу на след ячейку
            //if (cell.HasRefFormula)
            //{
            //    ExcelCellInfo cell_next_info = cell.FormulaRef.GetNextCellInfo(cell);
            //    if (cell_next_info != null)
            //    {
            //        ExcelCell cell_next = _worksheet.Rows
            //            .First(r => r.ID == cell_next_info.RowID.ToString())
            //            .Cells
            //            .First(c => c.ColumnName == cell_next_info.ColumnName);
            //        var xf = cell.Xml.Element(ns.main + "f");
            //        xf.Value = ExcelUtils.CorrectFormulaReferences(xf.Value,
            //            ExcelUtils.GetColumnNumber(cell_next.ColumnName) - ExcelUtils.GetColumnNumber(cell.ColumnName),
            //            cell_next.RowID - cell.RowID);
            //        cell_next.CopyFormula(cell);
            //    }
            //}
            // в смерженных ячейках первая хранит значение, если она удаляется - надо перенести значение в следующую за ней
            bool has_merge_value = this._worksheet.Merges.IsBeginOfMerge(cell.CellInfo);
            if (has_merge_value) {
                ExcelCellInfo cell_next_info = this._worksheet.Merges.GetNextMergedCell(cell.CellInfo);
                if (cell_next_info != null) {
                    ExcelCell cell_next = this._worksheet.Rows
                        .First(r => r.ID == cell_next_info.RowID.ToString())
                        .Cells
                        .First(c => c.CellInfo.ColumnName == cell_next_info.ColumnName);
                    cell_next.CopyValue(cell);   
                }
            }
        }
        internal ExcelCell CopyCell(ExcelCell cell, string column_name)
        {
            ExcelCell cell_copy = cell.Copy(this, column_name);
            int column_id = ExcelUtils.GetColumnNumber(column_name);
            ExcelCell cell_before = this._cells.TakeWhile(c => c.CellInfo.ColumnID < column_id).LastOrDefault();
            if (cell_before != null) {
                cell_before.Xml.AddAfterSelf(cell_copy.Xml);
                int index = this._cells.IndexOf(cell_before);
                this._cells.Insert(index + 1, cell_copy);
            } else {
                this.Xml.Add(cell_copy.Xml);
                this._cells.Add(cell_copy);
            }
            this._worksheet.Merges.ExtendMergeToColumn(cell.CellInfo, cell_copy.CellInfo);
            return cell_copy;
        }
    }
}