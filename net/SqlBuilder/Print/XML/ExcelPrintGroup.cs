using System;
using System.Collections.Generic;
using System.IO;
using System.Data;
//using System.Linq;
using System.Xml;
using System.Xml.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql.builder.Print.XML
{
    /// <summary>
    /// Группа строк шаблона Excel в формате xml
    /// </summary>
    /// <seealso cref="sql.builder.Print.Xlsx.ExcelPrintGroup"/>
    public class ExcelPrintGroup : IExcelPrintGroup, IExcelPrintEl
    {
        #region поля
        private readonly ExcelPrintGroup parent;
        //private readonly ExcelPrintSheet sheet;
        private readonly TableReference main_table_reference; 
        private readonly IDictionary<string, TableReference> table_references;
        private readonly IList<IExcelPrintElement> childs;
        private readonly bool dont_remove;
        #endregion
        public ExcelPrintGroup(ExcelPrintSheet sheet, List<XElement> rows, bool dontRemove, ExcelPrintGroup parent = null, string tableName = "")
        {
            //this.sheet = sheet;
            this.parent = parent;
            this.dont_remove = dontRemove;
            if (string.IsNullOrEmpty(tableName)) {
                this.table_references = new Dictionary<string, TableReference>(0);
            } else {
                string[] tabnames = tableName.Split(',');
                this.table_references = new Dictionary<string, TableReference>(tabnames.Length);
                int index;
                for (index = 0; index < tabnames.Length; index++) {
                    string table_name = tabnames[index];
                    TableReference tr = new TableReference(this, table_name);
                    this.table_references.Add(table_name, tr);
                    if (index == 0) {
                        this.main_table_reference = tr;
                    }
                }
            }
            this.childs = sheet.makeChildsList(tableName + '.', rows, this);
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
        public IExcelPrintGroup Parent { get { return this.parent; } }
        public IList<IExcelPrintElement> Childs { get { return this.childs; } }
        private TableReference GetTableReference()
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
        public void Print(XmlWriter writer, DataSet dataset, DataRow row, bool print_big_data)
        {
            if (row != null && this.GetTableReference().MainTableName != row.Table.TableName) {
                row = null;
            }
            // Dictionary<TKey, TValue>.ValueCollection.Enumerator поддерживает Reset(),
            // поэтому используем его трижды чтобы не плодить лишние объекты в памяти
            using (IEnumerator<TableReference> enumerator = this.table_references.Values.GetEnumerator()) {
                while (enumerator.MoveNext()) {
                    enumerator.Current.OpenRows(dataset, row, print_big_data);
                }
                bool hasAnyRows = true;
                while (hasAnyRows) {
                    hasAnyRows = false;
                    enumerator.Reset();
                    while (enumerator.MoveNext()) {
                        if (enumerator.Current.NextRow(print_big_data)) {
                            hasAnyRows = true;
                        }
                    }
                    if (hasAnyRows) {
                        for (int index = 0; index < this.childs.Count; index++) {
                            this.childs[index].Print(writer, dataset, null, print_big_data);
                        }
                    }
                }
                if (!print_big_data) {
                    enumerator.Reset();
                    while (enumerator.MoveNext()) {
                        enumerator.Current.CloseRows(dataset, print_big_data);
                    }
                }
            }
        }
        public void ClearData()
        {
            foreach (TableReference tr in this.table_references.Values) {
                tr.ClearData();
            }
            for (int index = 0; index < this.childs.Count; index++) {
                this.childs[index].ClearData();
            }
        }
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
