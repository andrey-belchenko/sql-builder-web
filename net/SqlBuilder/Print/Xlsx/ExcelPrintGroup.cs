using System.Collections.Generic;
using System.Xml.Linq;
using System.IO;
using System.Data;
using sql.builder.Print.Xlsx;

namespace sql.builder.Print.Xlsx
{
    /// <summary>
    /// Группа строк шаблона Excel в формате xlsx
    /// </summary>
    /// <seealso cref="sql.builder.Print.XML.ExcelPrintGroup"/>
    public class ExcelPrintGroup : IExcelPrintGroup, IExcelPrintEl
    {
        #region поля
        private readonly ExcelPrintGroup parent;
        //private readonly ExcelPrintSheet sheet;
        private readonly TableReference main_table_reference;
        private readonly IDictionary<string, TableReference> table_references;
        private readonly IList<IExcelPrintElement> сhilds;
        private readonly bool dont_remove;
        #endregion
        public ExcelPrintGroup(ExcelPrintSheet sheet, List<ExcelRow> rows, bool dontRemove, ExcelPrintGroup parent = null, string tableName = "", ExcelCell beginCell = null)
        {
            //this.sheet = sheet;
            this.parent = parent;
            this.dont_remove = dontRemove;
            if (string.IsNullOrEmpty(tableName)) {
                this.table_references = new Dictionary<string, TableReference>(0);
            } else {
                string[] tabnames = tableName.Split(',');
                this.table_references = new Dictionary<string, TableReference>(tabnames.Length);
                for (int index = 0; index < tabnames.Length; index++) {
                    string table_name = tabnames[index];
                    TableReference tr = new TableReference(this, table_name);
                    this.table_references.Add(table_name, tr);
                    if (index == 0) {
                        this.main_table_reference = tr;
                    }
                }
            }
            this.сhilds = sheet.makeChildsList(tableName + '.', rows, this, beginCell);
        }
        public IExcelPrintGroup Parent { get { return this.parent; } }
        public IList<IExcelPrintElement> Childs { get { return this.сhilds; } }
        public void Print(WorksheetPrint pi, DataSet data, bool use_data_reader, DataRow row)
        {
            // Dictionary<TKey, TValue>.ValueCollection.Enumerator поддерживает Reset(),
            // поэтому используем его трижды чтобы не плодить лишние объекты в памяти
            using (IEnumerator<TableReference> enumerator = this.table_references.Values.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    enumerator.Current.OpenRows(data, row, use_data_reader);
                }
                bool hasAnyRows = true;
                bool first = true;
                while (hasAnyRows) {
                    hasAnyRows = false;
                    enumerator.Reset();
                    while (enumerator.MoveNext()) {
                        bool hasRows = enumerator.Current.NextRow(use_data_reader);
                        if (hasRows) {
                            hasAnyRows = true;
                        }
                    }
                    if (first && !hasAnyRows) {
                        this.DeleteNode(pi);
                    } else if (hasAnyRows) {
                        for (int index = 0; index < this.сhilds.Count; index++) {
                            this.сhilds[index].Print(pi, data, use_data_reader, null);
                        }
                    }
                    first = false;
                }
                if (!use_data_reader) {
                    enumerator.Reset();
                    while (enumerator.MoveNext()) {
                        enumerator.Current.CloseRows(data, use_data_reader);
                    }
                }
            }
        }
        public void DeleteNode(WorksheetPrint pi)
        {
            for (int index = 0; index < this.сhilds.Count; index++) {
                this.сhilds[index].DeleteNode(pi);
            }
        }
        #region реализация IExcelPrintEl
        bool IExcelPrintEl.DontRemove { get { return this.dont_remove; } }
        bool IExcelPrintEl.HasParent { get { return this.parent != null; } }
        TableReference IExcelPrintEl.GetParentTableReference()
        {
            if (this.parent == null) {
                return null;
            } else {
                return this.parent.GetTableReference();
            }
        }
        #endregion
        public TableReference GetTableReference()
        {
            return this.main_table_reference;
        }
        public TableReference GetTableReference(string table_name)
        {
            TableReference tr;
            if (!this.table_references.TryGetValue(table_name, out tr)) {
                if (this.parent != null) {
                    tr = this.parent.GetTableReference(table_name);
                    if (tr == null) {
                        throw new KeyNotFoundException();
                    }
                }
            }
            return tr;
        }
        /*public int GetRowsCount(DataSet dataSet)
        {
            int rowsCount = 0;
            for (int index = 0; index < this.table_references.Length; index++) {
                int rc = this.table_references[index].GetRowsCount(dataSet);
                if (rc > rowsCount) {
                    rowsCount = rc;
                }
            }
            return rowsCount;
        }*/
        /*public int CalculateRowsCount(DataSet dataSet)
        {
            int rowsCount = 0;
            for (int index = 0; index < this.table_references.Length; index++) {
                int rc = this.table_references[index].CalculateRowsCount(dataSet);
                if (rc > rowsCount) {
                    rowsCount = rc;
                }
            }
            return rowsCount;
        }*/
        public List<DataRow> GetRowsByParent(DataSet dataSet, bool print_big_data)
        {
            List<DataRow> res = new List<DataRow>();
            TableReference tr = this.GetTableReference();
            DataTable tbl = null;
            if (print_big_data) {
                tbl = new DataTable();
                tbl.TableName = tr.MainTableName;
            }
            tr.OpenRows(dataSet, null, print_big_data, tbl);
            bool hasAnyRows = true;
            while (hasAnyRows) {
                hasAnyRows = tr.NextRow(print_big_data);
                if (hasAnyRows) {
                    DataRow row = null;
                    if (print_big_data) {
                        row = tbl.Rows.Add();
                        for (int index = 0; index < tbl.Columns.Count; index++) {
                            DataColumn col = tbl.Columns[index];
                            row[col] = tr.GetCurrentRowValue(col.ColumnName);
                        }
                    } else {
                        row = tr.GetCurrentRow();
                    }
                    res.Add(row);
                }
            }
            if (!print_big_data) {
                tr.CloseRows(dataSet, print_big_data);
            }
            return res;
        }
    }
}
