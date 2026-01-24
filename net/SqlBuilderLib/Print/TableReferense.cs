using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using sql.builder.DataApi;
using Oracle.ManagedDataAccess.Client;
using System.Xml;
using System.Xml.Linq;
using sql.builder.Print.Xlsx;
namespace sql.builder
{
    // Этот интерфейс нужен только для того,
    // чтобы TableReference мог взаимодействовать
    // и с sql.builder.Print.Xlsx.ExcelPrintElement, и с sql.builder.Print.XML.ExcelPrintGroup
    internal interface IExcelPrintEl
    {
        bool DontRemove { get; }
        bool HasParent { get; }
        TableReference GetParentTableReference();
    }
    internal class TableReference
    {
        private readonly IExcelPrintEl element;
        private readonly string fulltablename, maintablename, subtablename;
        private Dictionary<string, object> currentRowValues;  // NB: StringComparer.InvariantCultureIgnoreCase
        private Dictionary<string, object> prevRowValues;     // NB: StringComparer.InvariantCultureIgnoreCase
        private DataRow currentRow;
        private DataTable table;
        private SortedList<string, List<DataRow>> sortedRows;
        private string ParentColumnName;
        private string ParentRelatedColumnName;
        private bool currentRowExists;
        private IList<DataRow> subRows;
        internal string FullTableName { get { return this.fulltablename; } }
        internal string MainTableName { get { return this.maintablename; } }
        internal string SubTableName { get { return this.subtablename; } }
        internal DataTable Table { get { return this.table; } }
        //private ExcelPrintElement Element { get { return this.element; } }
        internal TableReference(IExcelPrintEl element, string table_name)
        {
            this.element = element;
            this.fulltablename = table_name;
            string[] tabs = table_name.Split('.');
            this.maintablename = tabs[0];
            if (tabs.Length > 1) {
                this.subtablename = tabs[1];
            } else {
                this.subtablename = null;
            }
            this.currentRowValues = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        }
        internal bool IsCurrentRowValueExists(string columnName)
        {
            return this.currentRowValues.ContainsKey(columnName);
        }
        internal XElement GetColumnFormula(string columnName)
        {
            VDataTable vtbl = (this.table as VDataTable);
            if (vtbl == null) return null;
            VDataColumn column = vtbl.GetColumn(columnName);
            if (column == null) return null;
            return column.GetExcelFormula();
        }
        internal object GetCurrentRowValue(DataColumn column)
        {
            if (this.currentRow != null) {
                // Емцов - отладка печати excel
                //if (!currentRow.Table.Columns.Contains(columnName)) return DBNull.Value;
                return this.currentRow[column];
            } else {
                return this.currentRowValues[column.ColumnName];
            }
        }
        internal object GetCurrentRowValue(string columnName)
        {
            if (this.currentRow != null) {
                // Емцов - отладка печати excel
                //if (!currentRow.Table.Columns.Contains(columnName)) return DBNull.Value;
                return this.currentRow[columnName.ToUpper()];
            } else {
                return this.currentRowValues[columnName];
            }
        }
        internal object GetPrevRowValue(string columnName)
        {
            if (this.prevRowValues != null) {
                return this.prevRowValues[columnName];
            } else {
                return DBNull.Value;
            }
        }
        internal bool IsPrevRowExists()
        {
            return this.prevRowValues != null;
        }
        internal void NewRow()
        {
            if (this.currentRowValues.Count > 0) {
                this.prevRowValues = this.currentRowValues;
            } else if (this.prevRowValues != null) {
                this.prevRowValues.Clear();
            }
            this.currentRowValues = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
        }
        internal void SetCurrentRowValue(string columnName, object value)
        {
            this.currentRowValues[columnName] = value;
        }
        internal void SetCurrentRowExists(bool value)
        {
            this.currentRowExists = value;
        }
        internal bool IsCurrentRowExists()
        {
            return this.currentRowExists;
        }
        internal void SetCurrentRow(DataRow row)
        {
            this.SetCurrentRowExists(row != null);
            this.currentRow = row;
        }
        internal DataRow GetCurrentRow()
        {
            return this.currentRow;
        }
        /*internal int GetRowsCount(DataSet dataSet)
        {
            int rowsCount = 0;
            if (String.IsNullOrEmpty(this.SubTableName)) {
                rowsCount = dataSet.Tables[this.FullTableName].Rows.Count;
            } else {
                rowsCount = GetSubRows(dataSet).Count();
            }
            return rowsCount;
        }*/
        /*internal int CalculateRowsCount(DataSet dataSet)
        {
            int rowsCount = 0;

            int acc = 1;
            if (this.element.Parent != null)
            {
                if (sortedRows == null)
                {
                    MakeIndexForParentKey(dataSet);
                }
                if (this.ParentColumnName == null)
                {
                    acc = this.element.Parent.GetRowsCount(dataSet);
                }
            }

            foreach (ExcelPrintNode node in this.element.Childs)
            {
                if (node.GetType() == typeof(ExcelPrintText))
                {

                    rowsCount += GetRowsCount(dataSet) * acc;

                    if (this.element.DontRemove)
                    {
                        if (this.element.Parent != null)
                        {
                            rowsCount += this.element.Parent.GetRowsCount(dataSet);
                        }
                        else
                        {
                            rowsCount += 1;
                        }
                    }
                }
                else
                {
                    rowsCount += (node as ExcelPrintElement).CalculateRowsCount(dataSet) * acc;
                }

            }
            return rowsCount;
        }*/
        internal void ClearData()
        {
            this.sortedRows = null;
            this.subRows = null;
        }
        private IList<DataRow> GetSubRows(DataSet dataSet)
        {
            if (this.subRows == null) {
                this.subRows = dataSet.Tables[this.MainTableName].AsEnumerable().Where(r => r["origgrsetid"].ToString() == SubTableName).ToArray();
            }
            return this.subRows;
        }
        public void AnalyzeRelation(DataSet dataSet)
        {
            if (!ParentExists(dataSet))
            {
                return;
            }

            DataTable dt = dataSet.Tables[MainTableName];

            if (String.IsNullOrEmpty(this.GetParentTableReference().SubTableName) || string.IsNullOrEmpty(SubTableName))
            {
                if (dt.ParentRelations.Count > 0)
                {
                    ParentColumnName = dt.ParentRelations[0].ChildColumns[0].ColumnName;
                    ParentRelatedColumnName = dt.ParentRelations[0].ParentColumns[0].ColumnName;
                }
                else
                {
                    ParentColumnName = null;
                }
            }
            else
            {
                ParentColumnName = "parent_growid";
                ParentRelatedColumnName = "growid";
            }
            
        }
        private void MakeIndexForParentKey(DataSet dataSet)
        {
            if (!this.ParentExists(dataSet)) {
                return;
            }
            DataTable dt = dataSet.Tables[this.MainTableName];
            sortedRows = new SortedList<string, List<DataRow>>();
            this.AnalyzeRelation(dataSet);
            List<DataRow> rows;
            if (this.ParentColumnName == null) {
                rows = dt.AsEnumerable().ToList();
            } else {
                // 18.12.17 Емцов - падало если были null-ы
                DataColumn parent_column = dt.Columns[this.ParentColumnName];
                if (parent_column.DataType == typeof(decimal)) {
                    rows = dt.AsEnumerable().OrderBy(row => Cmn.Nvl(row[parent_column], decimal.MinusOne)).ToList();
                } else {
                    IEnumerable<DataRow> rows2 = null;
                    if (String.IsNullOrEmpty(this.SubTableName)) {
                        rows2 = dt.AsEnumerable();
                    } else {
                        rows2 = this.GetSubRows(dataSet);
                    }
                    rows = rows2.OrderBy(row => row[parent_column]).ToList();   
                }
                string spOld = "-1";
                List<DataRow> rows1 = null;
                foreach (DataRow row in rows) {
                    object data = row[parent_column];
                    string spNew;
                    if (Cmn.IsNullOrDBNull(data)) {
                        spNew = string.Empty;
                    } else {
                        spNew = data.ToString();
                    }
                    if (spNew != spOld) {
                        if (rows1 != null) {
                            this.sortedRows.Add(spOld, rows1);
                        }
                        rows1 = new List<DataRow>();
                    }
                    rows1.Add(row);
                    spOld = spNew;
                }
                if (rows1 != null) {
                    this.sortedRows.Add(spOld, rows1);
                }
            }
        }
        private int RowIndex = -1;
        private bool isOpen;
        public bool IsOpen {
            get {
                return this.isOpen;
            }
            set {
                this.isOpen = value;
            }
        }
        private List<DataRow> rows = null;
        public OracleDataReader reader = null;
        private string parentId=null;
        private bool isNew = true;
        private bool isImputedRow=true;
        internal void OpenRows(DataSet dataSet, DataRow imputedRow = null, bool print_big_data = false, DataTable outputTable = null)
        {
            this.isNew = true;
            this.table = dataSet.Tables[this.MainTableName];
            if (this.table == null) {
                throw new ArgumentNullException();
            }
            this.RowIndex = -1;
			if (print_big_data /*&& Table.TableName != "pars"*/) {  // можно сделать одетльную query в отчет, в которой как столбцы вывести параметры
                //if (GetParentTableReference() != null)
                //{
                //    if (GetParentTableReference().IsCurrentRowValueExists("full_name"))
                //    {
                //        if (GetParentTableReference().GetCurrentRowValue("full_name").ToString() == "ВЛЭП 1-20 кВ (СН2)")
                //        {
                //        }
                //    }
                //}
                this.LoadThroughDataReader(dataSet, imputedRow, outputTable);
            } else {
                if (imputedRow != null) {
                    this.rows = new List<DataRow>();
                    this.rows.Add(imputedRow);
                } else {
                    this.rows = this.GetRowsByParent(dataSet);
                }
            }
			// можно сделать одетльную query в отчет, в которой как столбцы вывести параметры
			//if (print_big_data && Table.TableName == "pars")
			//{
			//	foreach (DataColumn col in rows[0].Table.Columns)
			//	{
			//		SetCurrentRowValue(col.ColumnName, rows[0][col]);
			//	}
			//}
        }
        private void LoadThroughDataReader(DataSet dataSet, DataRow imputedRow = null, DataTable outputTable = null)
        {
            if (!IsOpen || imputedRow != null) { // !!! зачищать перед печатью
                IsOpen = true;
                this.AnalyzeRelation(dataSet);
                var vdt = (this.Table as VDataTable);
                if (imputedRow == null) {
                    isImputedRow = false;
                    string cmdText = vdt.cmd.CommandText;
                    string newCmdText = cmdText;
                    bool done = false;
                    if (!string.IsNullOrEmpty(SubTableName)) {
                        int i1 = newCmdText.IndexOf("select");
                        newCmdText = newCmdText.Insert(i1 + ("select").Length, " rn,");
                        string sGrsetIdCol = TextConst.AVSpecColumnGrset.OrigGrSetName;
                        if (vdt.IsOnColsGrouping()) {
                            sGrsetIdCol = TextConst.AVSpecColumnGrset.OnRowsGrSetId;
                        }
                        newCmdText = "select * from (" + newCmdText + ") where " + sGrsetIdCol + "='" + SubTableName + "' order by rn";
                        // newCmdText+="order by "   дописать если собъется сортировка  
                        vdt.cmd.CommandText = newCmdText;
                    } else {
                        if (vdt.Reader != null && !(dataSet as VDataSet).UseTempTable) {
                            // заплатка, чтобы обработался случай, когда данные не проходят через rr_temp 
                            // отчет получается пустой
                            // предположителльно проблема из-за того, что есть другая временная таблица c on commit delete
                            // ipr.41293-cur 
                            reader = vdt.Reader;
                            done = true;
                        }
                    }
                    if (!done) {
                        vdt.cmd.FetchSize = 10;
                        reader = vdt.cmd.ExecuteReader();
                    }
                    //bufferTable = new DataTable();
                    //if (bufferTable.Columns.Count == 0)
                    //{
                    if (outputTable != null) {
                        for (int i = 0; i < reader.FieldCount; i++) {
                            outputTable.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
                        }
                        if (Table is VDataTable && (Table as VDataTable).IsOnColsGrouping()) {
                            foreach (DataColumn col in Table.Columns) {
                                if (!outputTable.Columns.Contains(col.ColumnName)) {
                                    outputTable.Columns.Add(col.ColumnName,col.DataType);
                                }
                            }
                        }
                    }
                    vdt.cmd.CommandText = cmdText;
                    //}
                } else {
                    isImputedRow = true;
                    NewRow();
                    foreach (DataColumn col in imputedRow.Table.Columns) {
                        SetCurrentRowValue(col.ColumnName, imputedRow[col]);
                    }
                }
            }
            parentId = GetParentId();
        }
        internal void CloseRows(DataSet dataSet, bool print_big_data = false)
        {
            if (print_big_data) {
                if (this.reader != null) {
                    reader.Dispose();
                    (dataSet.Tables[this.MainTableName] as VDataTable).cmd.Dispose();
                }
            }
            this.reader = null;
            this.RowIndex = -1;
            this.IsOpen = false;
            this.currentRowValues = new Dictionary<string, object>(StringComparer.InvariantCultureIgnoreCase);
            this.rows = null;
            this.table = null;
            this.sortedRows = null;
            this.subRows = null;
        }
        private bool alreadyRead = false;
        private bool alreadyHasRow = false;
        public static TableReference LastReadRowTableReference = null;
        public bool NextRow(bool print_big_data = false)
        {
            RowIndex++;
            //cnt++;
            //if (cnt == 7500)
            //{
            //}
            //if (SubTableName == "spb")
            //{
            //}
            if (print_big_data) {
                bool doClientCalc = false;
                var retVal = false;
                if (!isImputedRow) {
                    if (reader.HasRows) {
                        bool hasRow = false;
                        if (!(isNew && this.currentRowValues.Count > 0)) {
                            if (alreadyRead) {
                                hasRow = alreadyHasRow;
                            } else {
                                hasRow = reader.Read();
                            }
                            alreadyRead = false;
                            if (hasRow) {
                                //foreach (DataColumn col in Table.Columns)
                                //{
                                //    SetCurrentRowValue(col.ColumnName, reader[col.ColumnName]);
                                //}
                                //if ((Table is VDataTable) && (Table as VDataTable).IsOnColsGrouping())
                                //{
                                //    currentRowValues.Clear();
                                //}
                                NewRow();
                                for (int i = 0; i < reader.FieldCount; i++) {
                                    SetCurrentRowValue(reader.GetName(i), reader[i]);
                                }
                                if (Table is VDataTable) {
                                    if ((Table as VDataTable).IsOnColsGrouping()) {
                                        alreadyHasRow = VDataTableTransposeUtils.PartialReadTransposedRow(this);
                                        alreadyRead = true;
                                    }
                                    doClientCalc = true;
                                }
                            } else {
                                if (parentId == null) {
                                    //нужно для случая когда таблица без связей печатается несколько раз, до этого второй раз печаталась только последняя строка, возможно отразиться на других случаях 
                                    this.currentRowValues.Clear();
                                    IsOpen = false; 
                                }
                            }
                        } else {
                            hasRow = true;
                            if (Table is VDataTable) {
                                //if ((Table as VDataTable).IsOnColsGrouping()) {
                                //}
                                doClientCalc = true; // для первой строки второй и далее группы, вызывается  здесь , чтобы сначала продгрузился parent
                            }
                        }
                        isNew = false;
                        //if (RowIndex > 40000)
                        //{
                        //    SetCurrentRowExists(false);
                        //    //CurrentRow = null;
                        //    // CloseRows(dataSet, print_big_data);
                        //    return false;
                        //}
                        if (hasRow) {
                            if (parentId == null) {
                                SetCurrentRowExists(true);
                                retVal = true;
                            } else {
                                if (GetCurrentRowValue(ParentColumnName).ToString() == parentId) {
                                    SetCurrentRowExists(true);
                                    retVal= true;
                                } else {
                                    SetCurrentRowExists(false);
                                    doClientCalc = false;
                                    retVal= false;
                                }
                            }
                        } else {
                            SetCurrentRowExists(false);
                            //CurrentRow = null;
                           // CloseRows(dataSet, print_big_data);
                            retVal= false;
                        }
                    } else {
                        //CurrentRow = null;
                        SetCurrentRowExists(false);
                       // CloseRows(dataSet, print_big_data);
                        retVal= false;
                    }
                } else {
                    if (isNew) {
                        SetCurrentRowExists(true);
                        isNew = false;
                        retVal= true;
                    } else {
                        SetCurrentRowExists(false);
                      //  CloseRows(dataSet, print_big_data);
                        retVal= false;
                    }
                }
                if (doClientCalc) {
                    var dataAccessor = new VClientCalculations.DataAccessor();
                    dataAccessor.TableReference = this;
                    (Table as VDataTable).DoClientCalculationsForRow(dataAccessor);
                    LastReadRowTableReference = this;
                }
                return retVal;
            } else {
                if (rows.Count > RowIndex) {
                    SetCurrentRow(rows[RowIndex]);
                    //CurrentRow = rows[RowIndex];
                    return true;
                } else if (this.element.DontRemove && rows.Count == 0 && RowIndex == 0) { //!!! добавить это для reader если понадобится
                    SetCurrentRow(null);
                   // CurrentRow = null;
                    return true;
                } else {
                    SetCurrentRow(null);
                   // CurrentRow = null;
                    return false;
                }
            }
        }
        private bool ParentExists(DataSet dataSet)
        {
            TableReference tr = this.GetParentTableReference();
            if (tr == null) {
                return false;
            }
            DataTable dt = dataSet.Tables[this.MainTableName];
            if (String.IsNullOrEmpty(tr.SubTableName)) {
                return dt.ParentRelations.Count == 1;
            } else {
                return true;
            }
        }
         internal TableReference GetParentTableReference() // заплатка для вычислений на клиенте, если иерархия в шаблоне не будет соответсвовать иерархии grsets, работать не будет
         {
             if (!this.element.HasParent) {
                 return null;
             } else {
                 return this.element.GetParentTableReference();
             }
         }
         private string GetParentId() {
             if (this.ParentRelatedColumnName == null) {
                 return null;
             }
             TableReference tr = this.GetParentTableReference();
             if (tr == null || !tr.IsCurrentRowExists()) {
                 return null;
             }
             string parentId = tr.GetCurrentRowValue(this.ParentRelatedColumnName).ToString();
             return parentId;
         }
         private List<DataRow> GetRowsByParent(DataSet dataSet)
         {
            bool parentExists;
            //if (this.element.Parent != null) {
            if (this.element.HasParent) {
                parentExists = this.ParentExists(dataSet);
                if (parentExists && this.sortedRows == null) {
                    this.MakeIndexForParentKey(dataSet);
                }
            } else {
                parentExists = false;
            }
            List<DataRow> rows;
            if ((!this.element.HasParent) || (!parentExists) || this.ParentRelatedColumnName == null) {
            //if (this.element.Parent == null || (!parentExists) || this.ParentRelatedColumnName == null) {
                int index;
                if (String.IsNullOrEmpty(this.SubTableName)) {
                    DataRowCollection dt = dataSet.Tables[this.FullTableName].Rows;
                    rows = new List<DataRow>(dt.Count);
                    for (index = 0; index < dt.Count; index++) {
                        rows.Add(dt[index]);
                    }
                    //rows = dataSet.Tables[this.FullTableName].AsEnumerable().ToList();
                } else {
                    IList<DataRow> subrows = this.GetSubRows(dataSet);
                    rows = new List<DataRow>(subrows.Count);
                    for (index = 0; index < subrows.Count; index++) {
                        rows.Add(subrows[index]);
                    }
                    //rows = this.GetSubRows(dataSet).ToList();
                }
            } else {
                string parentId = this.GetParentId();
                if (parentId != null) {
                    if (this.sortedRows.ContainsKey(parentId)) {
                        rows = this.sortedRows[parentId];
                    } else {
                        rows = new List<DataRow>();
                    }
                } else {
                    rows = new List<DataRow>();
                }
            }
            return rows;
        }
    }
}
