using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;

namespace sql.builder.Print.Xlsx
{
    /// <summary>
    /// ����� ������ Excel � ������� A1
    /// </summary>
    public class ExcelCellInfo : IEquatable<ExcelCellInfo>
    {
        #region ����
        private int row_id;
        private int column_id;
        private string column_name;
        #endregion
        #region ��������
        /// <summary>
        /// ���������� ����� �������, ������� � ������� (A - 1, B - 2, ..., Z - 26, AA - 27 � �. �.)
        /// </summary>
        public int ColumnID
        {
            get {
                return this.column_id;
            }
            set {
                this.column_id = value;
                this.column_name = ExcelUtils.GetColumnName(value);
            }
        }
        /// <summary>
        /// ���������� ����� ����, ������� � �������
        /// </summary>
        public int RowID
        {
            get {
                return this.row_id;
            }
            set {
                this.row_id = value;
            }
        }
        /// <summary>
        /// ��������� ����������� �������
        /// </summary>
        public string ColumnName {
            get {
                return this.column_name;
            }
            set {
                this.column_name = value;
                this.column_id = ExcelUtils.GetColumnNumber(this.column_name);
            }
        }
        /// <summary>
        /// ����� ������ � ������� A1
        /// </summary>
        public string CellName
        {
            get {
                return this.column_name + this.row_id.ToString();
            }
        }
        #endregion
        public ExcelCellInfo(string cell_name)
        {
            Contract.Assert(!string.IsNullOrEmpty(cell_name));
            ExcelUtils.ParseCellName(cell_name, out this.row_id, out this.column_id, out this.column_name);
        }
        public ExcelCellInfo(int row_id, int colunm_id)
        {
            this.row_id = row_id;
            this.column_id = colunm_id;
            this.column_name = ExcelUtils.GetColumnName(colunm_id);
        }
        public override int GetHashCode()
        {
            return this.row_id * 100000 + this.column_id;
        }
        public override string ToString()
        {
            return this.CellName;
        }
        public override bool Equals(object obj)
        {
            return this.Equals(obj as ExcelCellInfo);
        }
        public bool Equals(ExcelCellInfo obj)
        {
            if (obj == null) {
                return false;
            } else {
                return (obj.column_id == this.column_id) && (obj.row_id == this.row_id);
            }
        }
        public void GetRowAndColumn(out int row_id, out int column_id)
        {
            row_id = this.row_id;
            column_id = this.column_id;
        }
        public void SetColumnFrom(ExcelCellInfo cell)
        {
            this.column_id = cell.column_id;
            this.column_name = cell.column_name;
        }
        public static IEnumerable<ExcelCellInfo> Range(ExcelCellInfo cellFrom, ExcelCellInfo cellTo)
        {
            for (int row = cellFrom.row_id; row <= cellTo.row_id; row++) {
                for (int colunm = cellFrom.column_id; colunm <= cellTo.column_id; colunm++) {
                    yield return new ExcelCellInfo(row, colunm);
                }
            }
        }
        #region �������� ���������
        public bool IsBeginOfMerge(ExcelWorksheetMerge merge)
        {
            return merge.StartsWith(this);
        }
        public bool IsEndOfMerge(ExcelWorksheetMerge merge)
        {
            return merge.EndsWith(this);
        }
        public bool ContainsIn(ExcelWorksheetMerge merge)
        {
            return merge.ContainsCell(this);
        }
        #endregion
    }
}