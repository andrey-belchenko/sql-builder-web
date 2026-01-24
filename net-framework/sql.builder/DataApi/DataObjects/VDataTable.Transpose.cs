using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using System.Text;
using sql.builder;
using System.Data;
namespace sql.builder.DataApi
{
    internal partial class VDataTable : DataTable
    {
        private XElement _groupingInfo = null;
        public XElement GetGroupingInfo()
        {
            if (_groupingInfo == null)
            {

                XElement groupingQuery = XmlReports.Environment.GetQuery(this.QueryName).AsXElementApplyingParts();

                VQuery sourceQuery = null;
                if (groupingQuery.Element(TextConst.EName.Grouping) != null)
                {
                    groupingQuery = new XElement(groupingQuery);
                    Compiler.addColumnsAlias(groupingQuery,false,true);
                    Compiler.prepareSelfGrsets(groupingQuery, null);
                    sourceQuery =(VQuery) VSXElement.Get( groupingQuery);
                    _groupingInfo = sourceQuery.Descendants(TextConst.EName.Grsets).First();
                }
                else
                {
                    _groupingInfo = groupingQuery.Descendants(TextConst.EName.Grsets).First();
                    sourceQuery = XmlReports.Environment.GetQuery(_groupingInfo.Parent.Attribute(TextConst.AName.Name).Value);
                }
                _groupingInfo = new XElement(_groupingInfo);
                //preProcessingIGroup(element);

                var allFacts = sourceQuery.Columns().Where(e => Compiler.grFuncsNames.Contains( Cmn.GetAttrValue(e, TextConst.AName.Group))).ToList();

                foreach (XElement grset in _groupingInfo.Descendants(TextConst.EName.Grset))
                {
                    var xcolsInfo = new XElement(TextConst.EName.Dimensions);
                    grset.Add(xcolsInfo);
                    XElement xfactsInfo = null;
                    if (!grset.Elements(TextConst.EName.Facts).Any())
                    {
                        xfactsInfo = new XElement(TextConst.EName.Facts);
                        grset.Add(xfactsInfo);
                    }
                   
                    var keys = grset.Attribute(TextConst.AName.Level).Value.Split(',').ToList();
                    foreach (string key in keys)
                    {
                        if (key != "")
                        {
                            var keyColumn = sourceQuery.Columns().First(e => Cmn.GetAttrValue(e, TextConst.AName.Group) == key);
                            var xcolInfo = new XElement(TextConst.EName.Column, new XAttribute(TextConst.AName.Column, keyColumn.XName)
                                , new XAttribute(TextConst.AName.Key, TextConst.AVBool.True)
                                );

                            xcolsInfo.Add(xcolInfo);
                            foreach (VSXElement attrCol in sourceQuery.Columns().Where(
                                e => Cmn.GetAttrValue(e, TextConst.AName.Group) == keyColumn.XName  
                                    ||
                                     Cmn.GetAttrValue(e, TextConst.AName.Master) == keyColumn.XName  
                                    ).ToList())
                            {
                                xcolInfo = new XElement(TextConst.EName.Column, new XAttribute(TextConst.AName.Column, attrCol.XName));

                                xcolsInfo.Add(xcolInfo);
                            }
                        }
                    }
                    if (xfactsInfo != null)
                    {
                        foreach (VSXElement factCol in allFacts)
                        {
                            var xcolInfo = new XElement(TextConst.EName.Column, new XAttribute(TextConst.AName.Column, factCol.XName));
                            xfactsInfo.Add(xcolInfo);
                        }
                    }
                }
               

            
                
            }
            return _groupingInfo;
        }

        public  bool IsOnColsGrouping()
        {
           // return false;
            if (this.Columns.Contains(TextConst.AVSpecColumnGrset.OnColsColId)) // !!! не очень удачно - коcвенный признак, пока так
            {
                return true;
            }
            else
            {
                return false;
            }
        }


        //public SortedList<string, string> ColumnDimsQueries = null;

        //public void GenerateColumnsDimsQueries(XElement compiled)
        //{
        //    if (!IsOnColsGrouping()) return;
        //    ColumnDimsQueries = new SortedList<string, string>();

        //    foreach (XElement xgrset in GetGroupingInfo().Element(TextConst.EName.OnColumns).Descendants(TextConst.EName.Grset).ToList())
        //    {
               
        //    }
        //}
        public ITransposeDataStructure TransposeStructure = null;
    }

    internal interface ITransposeDataStructure
    {
        string GetColumnNameForForDimValue(string originalName, string dimensionValue);
    }
    class VDataTableTransposeUtils
    {
        
        
        
        public static void TransposeIfNeed(VDataTable table)
        {
            //return;
            if (table.IsOnColsGrouping())
            {
                Transpose(table);
            }
        }

        private class DimensionValue
        {
            private string _id;
            public string Id{
                get
                {
                    return _id;
                }
                set
                {
                    _id = value;
                    if (_id == "")
                    {
                    }
                }
            
            }
            public Dictionary<string, string> Atributes = new Dictionary<string, string>();
            public SortedList<string, string> NewColumnsNames = new SortedList<string, string>();
            public decimal NumericValue = 0; //для интервалов
        }

        private class RowSet
        {
            public DataRow MainRow;
            public SortedList<string, SortedList<string, List<DataRow>>> OtherRows = new SortedList<string, SortedList<string, List<DataRow>>>();// главная строка->идентификаторы измерений->строки для идентификатора
           

        }

        private class GrsetData
        {
            public SortedList<string, DimensionValue> DimValues=new SortedList<string,DimensionValue>();
            public string Name = null;

            public XElement XInfo = null;
            public List<string> DimColumnsNames = new List<string>();
            public Dictionary<string, FactColumnInfo> FactColumnsInfo = new Dictionary<string, FactColumnInfo>();
            public bool IsWithDimQuery = false;
            public string IntervalBeginColumnName = null;
            public string IntervalEndColumnName = null;
            public bool FillDimvalTable = false; //для reader пока не реализовано
            public string  DimvalTableName = null;
        }

        private class FactColumnInfo
        {
            
            public string Name = null;
         
            

        }

        private class TransposeDataStructure : ITransposeDataStructure
        {
            public SortedList<string, GrsetData> Grsets = new SortedList<string, GrsetData>();
            public SortedList<string, RowSet> RowSets = new SortedList<string, RowSet>();
            public SortedList<string, SortedList<string, string>> NewColumnsNames = new SortedList<string, SortedList<string, string>>();
            // поучается что ключи измерений для колонок с одинаковым фактом не могут повторяться, подругому не применить вычисения на клиенте
            public string GetColumnNameForForDimValue(string originalName, string dimensionValue)
            {
                return NewColumnsNames[dimensionValue][originalName];


            }


           
        }

        private static string clearColId(string val)
        {
            return val.Replace(".", "_").Replace(",", "_").Replace("#", "_"); 
        }
        private static void AnalyzeDataWithFullLoad(TransposeDataStructure data, VDataTable table)
        {


            foreach (var grs in data.Grsets.Values)
            {
                TryPreAnalyzeDataWithDimQuery(grs, table); 
            }
            foreach (DataRow row in table.AsEnumerable())
            {
                var rowGrsetName = row[TextConst.AVSpecColumnGrset.OnColsGrSetId].ToString();
                GrsetData grset = null;
                if (!string.IsNullOrEmpty(rowGrsetName))
                {
                    grset = data.Grsets[rowGrsetName];
                }

                var onRowsRowId = row[TextConst.AVSpecColumnGrset.OnRowsGrRowId].ToString();
                RowSet rowset = null;
                if (!data.RowSets.ContainsKey(onRowsRowId))
                {
                    rowset = new RowSet();
                    rowset.OtherRows = new SortedList<string, SortedList<string, List<DataRow>>>();
                    data.RowSets.Add(onRowsRowId, rowset);
                }
                else
                {
                    rowset = data.RowSets[onRowsRowId];
                }

                if (grset == null)
                {
                    rowset.MainRow = row;
                }
                else
                {
                  
                   
                    
                    
                    string dimValId="";
                    if (grset.IsWithDimQuery)
                    {
                        dimValId = getDimValIdStringFromRow(grset, row, null, null);
                    }
                    else
                    {
                         dimValId = addDimValFromRow(grset, row, null, null,null);
                    }
                    if (!rowset.OtherRows.ContainsKey(rowGrsetName))
                    {
                        rowset.OtherRows.Add(rowGrsetName, new SortedList<string, List<DataRow>>());
                         
                    }
                     List<DataRow> rows=null;
                     if (rowset.OtherRows[rowGrsetName].ContainsKey(dimValId))
                     {
                         rows = rowset.OtherRows[rowGrsetName][dimValId];
                     }
                     else
                     {
                         rows = new List<DataRow>();
                         rowset.OtherRows[rowGrsetName].Add(dimValId, rows);
                     }

                     rows.Add(row);
                }
            }
        }


        private static string getDimValIdStringFromRow(GrsetData grset, DataRow row, string keyColumnName, List<string> otherCols)
        {
           
            var pfx = "";
            if (keyColumnName == null)
            {
                keyColumnName = TextConst.AVSpecColumnGrset.OnColsColId;
                otherCols = grset.DimColumnsNames.ToList();
            }
            else
            {
                pfx = "_";
            }
            var dimValId = clearColId(pfx + row[keyColumnName].ToString());
            return dimValId;
        }

        private static object readRowOrReaderVal(DataRow row, Devart.Data.Oracle.OracleDataReader tr, string columnName)
        {
            if (row != null)
            {
                return row[columnName];
            }
            else
            {
                return tr[columnName];
            }
        }

        private static bool rowOrTrColumnExists(DataRow row, TableReference tr, string columnName)
        {
            if (row != null)
            {
                return row.Table.Columns.Contains(columnName);
            }
            else
            {
                return tr.IsCurrentRowValueExists (columnName);
            }
        }

        private static string addDimValFromRow(GrsetData grset, DataRow row, string keyColumnName, List<string> otherCols,Devart.Data.Oracle.OracleDataReader tr)
        {
            if (grset.DimValues == null)
            {
                grset.DimValues = new SortedList<string, DimensionValue>();
            }
            var pfx = "";
            if (keyColumnName == null)
            {
                keyColumnName = TextConst.AVSpecColumnGrset.OnColsColId;
                otherCols = grset.DimColumnsNames.ToList();
            }
            else
            {
                pfx = "_";
            }
            var dimValId = clearColId(pfx+
                readRowOrReaderVal(row,tr,keyColumnName)
                .ToString());
            if (!grset.DimValues.ContainsKey(dimValId))
            {
                var dimVal = new DimensionValue();
                dimVal.Id = dimValId;
                foreach (string dimColName in otherCols)
                {
                    if (dimColName != grset.IntervalEndColumnName)
                    {
                        var attrVal = readRowOrReaderVal( row,tr,dimColName).ToString();
                        dimVal.Atributes.Add(dimColName, attrVal);
                    }
                }
                if (grset.IntervalBeginColumnName != null)
                {
                    var ibc = grset.IntervalBeginColumnName;
                    if (row != null)
                    {
                        if (!row.Table.Columns.Contains(ibc))
                        {
                            ibc = keyColumnName;
                        }
                    }
                    else
                    {
                        
                        //if (!tr.IsCurrentRowValueExists(ibc))
                        //{
                        //    ibc = keyColumnName;
                        //}
                    }
                    var v = readRowOrReaderVal(row,tr,ibc);
                    if (!(v is decimal))
                    {
                        v = (decimal)Cmn.ToDecimal(readRowOrReaderVal(row, tr, ibc).ToString());
                    }
                    dimVal.NumericValue = (decimal)v;
                }
                grset.DimValues.Add(dimValId, dimVal);
                if (row != null)
                {
                    UpdateDimTableIfNeed(grset, dimVal, row.Table.DataSet);
                }
            }
            return dimValId;
        }

        private static void UpdateDimTableIfNeed(GrsetData grset,  DimensionValue dimVal,DataSet ds)
        {
            
            if (!grset.FillDimvalTable) return;
            var tbl = ds.Tables[grset.DimvalTableName];


            if (tbl == null)
            {
                tbl = new DataTable();
                tbl.TableName = grset.DimvalTableName;
                tbl.Columns.Add(new DataColumn(dimIdColName));
                tbl.Columns.Add(new DataColumn(dimPfxColName));
                tbl.PrimaryKey = new DataColumn[] { tbl.Columns[dimIdColName] };
                ds.Tables.Add(tbl);
            }

            var row = tbl.Rows.Find(dimVal.Id);


            if (row == null)
            {
                row = tbl.NewRow();
                row[dimIdColName] = dimVal.Id;
                row[dimPfxColName] = dimVal.Id;

                foreach (var aa in dimVal.Atributes)
                {
                    if (!tbl.Columns.Contains(aa.Key))
                    {
                        tbl.Columns.Add(aa.Key);
                       
                    }
                    row[aa.Key] = aa.Value;
                }
                tbl.Rows.Add(row);
            }

        }
        private static void PreAnalyzeDataWithPartialLoad(TransposeDataStructure data, VDataTable table)
        {
            foreach (GrsetData grset in data.Grsets.Values)
            {
                grset.DimValues = new SortedList<string, DimensionValue>();
                TryPreAnalyzeDataWithDimQuery(grset, table);
                if (grset.IsWithDimQuery)
                {
                    continue;
                }
                var cmdText = table.cmd.CommandText;
                var sColumns = TextConst.AVSpecColumnGrset.OnColsColId;
                var q = ",";


                foreach (string colName in grset.DimColumnsNames)
                {
                    sColumns += q + colName;

                }
                cmdText = "select " + sColumns + " from (" + cmdText + ") where "
                    + TextConst.AVSpecColumnGrset.OnColsGrSetId + "='" + grset.Name + "'" + " group by " + sColumns;

                var sOrder = Cmn.GetAttrValue(grset.XInfo, TextConst.AName.Order);

                if (sOrder != "")
                {
                    cmdText += " order by " + sOrder;
                }
                var tbl = db.ExecuteDataTable(cmdText);

                foreach (DataRow row in tbl.Rows)
                {
                    addDimValFromRow(grset, row,null,null,null);
                }
            }
        }

        private static string getDimQueryName(VDataTable table, string grSetName)
        {
            var xqryCall = table.GetDataSet().Report.Element(TextConst.EName.Queries).Descendants(TextConst.EName.Query).Where(q => Cmn.GetAttrValue(q, TextConst.AName.As) == table.TableName).First();
            var xDimQryInfo = xqryCall.Elements(TextConst.EName.DimQuery).Where(e => Cmn.GetAttrValue(e, TextConst.AName.Dimension) == grSetName).FirstOrDefault();
            if (xDimQryInfo != null)
            {
                return xDimQryInfo.Attribute(TextConst.AName.Table).Value;
            }
            else
            {
                return null;
            }
        }



        private static string dimIdColName = "id";
        private static string dimPfxColName = "pfx";
        private static void TryPreAnalyzeDataWithDimQuery(GrsetData grset, VDataTable table)
        {
            var dimQryName = getDimQueryName(table, grset.Name);
            //if (dimQryName == null)
            //{
            //    return;
            //}

            if (dimQryName == null)
            {
                dimQryName = grset.Name;
            }


            if (table.GetDataSet().Tables[dimQryName]==null)
            { //наоборот, таблица заполняетя в соответствии с фактическими значениями измерений, нужно для вывода в excel
                grset.DimvalTableName = dimQryName;
                grset.FillDimvalTable = true;
                return;
            }
            grset.IsWithDimQuery = true;
            var dimTable = table.GetDataSet().GetTable(dimQryName);
            var otherCols = new List<string>();
            foreach (VDataColumn col in dimTable.Columns)
            {
                if (col.GetVisibility(null) && col.Caption != col.ColumnName) // с caption заплатка, обеспечить корректную работу GetVisibility тут
                {
                    otherCols.Add(col.ColumnName);
                }
            }

            dimTable.ReadAll();
            //if (dimTable.IsReader)
            //{
            //    while (dimTable.Reader.Read())
            //    {
            //        var row = dimTable.NewRow();
            //        foreach (DataColumn col in dimTable.Columns)
            //        {
            //            row[col.ColumnName] = dimTable.Reader[col.ColumnName];
            //        }
            //        addDimValFromRow(grset, row, row.Table.Columns[0].ColumnName,otherCols);
            //    }

            //}
            //else
            //{
                foreach (DataRow row in table.GetDataSet().Tables[dimQryName].Rows)
                {
                    addDimValFromRow(grset, row, row.Table.Columns[0].ColumnName, otherCols,null);
                }
            //}

          
        }

        private static TransposeDataStructure AnalyzeData(VDataTable table,XElement goupingInfo )
        {
            var data = new TransposeDataStructure();
            foreach (XElement xgrset in goupingInfo.Element(TextConst.EName.OnColumns).Descendants(TextConst.EName.Grset).ToList())
            {
               
                var name = xgrset.Attribute(TextConst.AName.As).Value;
                var grset = new GrsetData();
                grset.XInfo = xgrset;
                grset.Name = name;
                var dimCols = grset.XInfo.Element(TextConst.EName.Dimensions).Elements();
                grset.DimColumnsNames = dimCols.Select(e => e.Attribute(TextConst.AName.Column).Value).ToList();

                if (Cmn.GetAttrValue(xgrset, TextConst.AName.Intervals) == TextConst.AVBool.True)
                {
                    var keyCols = dimCols.Where(c => Cmn.GetAttrValue(c, TextConst.AName.Key) == TextConst.AVBool.True);
                    grset.IntervalBeginColumnName = keyCols.First().Attribute(TextConst.AName.Column).Value;
                    grset.IntervalEndColumnName = keyCols.Last().Attribute(TextConst.AName.Column).Value;
                }

                foreach (XElement xcol in grset.XInfo.Element(TextConst.EName.Facts).Elements().ToList())
                { 
                   
                    var fci = new FactColumnInfo();
                    fci.Name=xcol.Attribute(TextConst.AName.Column).Value;
                
                    grset.FactColumnsInfo.Add(fci.Name, fci);
                }
             
               
                data.Grsets.Add(name,grset); 
            }
            if (!table.IsReader)
            {
                AnalyzeDataWithFullLoad(data, table);
            }
            else
            {
                PreAnalyzeDataWithPartialLoad(data, table);
                
            }

            return data;

        }

        private static string combineColumnName(TransposeDataStructure data, GrsetData grset, string factColName, DimensionValue dval)
        {
            if (!dval.NewColumnsNames.ContainsKey(factColName))
            {
                dval.NewColumnsNames[factColName] = factColName + /*"_" + grset.Name +*/ dval.Id; 
                // есть вероятность получить неуникальное имя, решать путем добавлния того же факта с другим именем
                // добавление в имя grset.Name создает много неудобства для вычислений на клиенте, и оформлении шаблона excel
                if (!data.NewColumnsNames.ContainsKey(dval.Id)){
                    data.NewColumnsNames[dval.Id] = new SortedList<string, string>();
                }
                data.NewColumnsNames[dval.Id][factColName] = dval.NewColumnsNames[factColName];// для вычислений на клиенте
            }

            return dval.NewColumnsNames[factColName];
        }

        private static void addColumn(string newName,bool dimOnTop, VDataTable table, string factColName, DimensionValue dval, string prevColName)
        {

            var factTitle = table.Columns[factColName].Caption;
            var newTitle = "";
            var bandTitle = "";
            var q = "";
            var dimTitle = "";
            foreach (string aval in dval.Atributes.Values)
            {
                dimTitle += q + aval;
                q = " | ";
            }

            if (dimOnTop)
            {
                newTitle = factTitle;
                bandTitle = dimTitle;
            }
            else
            {
                newTitle = dimTitle;
                bandTitle = factTitle;
            }
            copyColumn(table, factColName, newName, newTitle,bandTitle, prevColName);
            
        }
        //private static string addColumnIfNeed(VDataTable table, GrsetData grset, string factColName, DimensionValue dval, string prevColName)
        //{
        //    var newName = combineColumnName(grset, factColName, dval);
        //    if (!table.Columns.Contains(newName))
        //    {
        //        var newTitle = table.Columns[factColName].Caption;
        //        foreach (string aval in dval.Atributes.Values)
        //        {
        //            newTitle += " | " + aval;
        //        }
        //        copyColumn(table, factColName, newName, newTitle, prevColName);
        //    }
        //    return newName;
        //}


        private static void processColumns(VDataTable table, TransposeDataStructure data)
        {
            bool dimOnTop = false;
            dimOnTop = true;
            SortedList<string, string> lastFactColInDim = new SortedList<string, string>();
            foreach (GrsetData grset in data.Grsets.Values)
            {
                foreach (var factCol in grset.FactColumnsInfo.Values)
                {
                    if (!table.Columns.Contains(factCol.Name)) continue;// для зароса с выбором колонок 
                    var factColName = factCol.Name;
                    string prevColName = null;
                    foreach (DimensionValue dval in grset.DimValues.Values)
                    {

                        var newName = combineColumnName(data,grset, factColName, dval);
                        string insertAfter = null;
                        if (dimOnTop)
                        {
                            if (lastFactColInDim.ContainsKey(dval.Id))
                            {
                                insertAfter = lastFactColInDim[dval.Id];
                            }
                            else
                            {
                                if (prevColName == null)
                                {
                                    insertAfter = grset.DimColumnsNames.First();
                                }
                                else
                                {
                                    insertAfter = prevColName;
                                }
                            }
                        }
                        else
                        {
                            insertAfter = prevColName;
                        }
                        addColumn(newName,dimOnTop, table, factColName, dval, insertAfter);

                        var origCol = (VDataColumn)table.Columns[factColName];
                        var newCol = (VDataColumn)table.Columns[newName];
                        newCol.OriginalNameForPivotColumn = origCol.ColumnName;
                        newCol.PivotDimensionName = grset.Name;
                        newCol.PivotDimensionValue = dval.Id;
                        newCol.IsClientCalculations = origCol.IsClientCalculations;
                        lastFactColInDim[dval.Id] = newName;
                        prevColName = newName;
                    }
                }
            }
        }


        public static bool PartialReadTransposedRow(TableReference tr)
        {
            foreach (DataColumn col in tr.Table.Columns)
            {
                if (!tr.IsCurrentRowValueExists(col.ColumnName))
                {
                    tr.SetCurrentRowValue(col.ColumnName, DBNull.Value);
                }
            }
            bool read = true;
            var rowId = tr.reader[TextConst.AVSpecColumnGrset.GrRowId].ToString();
            var res = true;
            var table = (tr.Table as VDataTable);
            var tri = (TransposeDataStructure)table.TransposeStructure;
            while (read)
            {
                var hasRow = tr.reader.Read();
                if (hasRow)
                {
                    var rowId1 = tr.reader[TextConst.AVSpecColumnGrset.OnRowsGrRowId].ToString();
                    if (rowId1 != rowId)
                    {
                        read = false;
                        res = true;
                    }
                    else
                    {
                      
                        var grsetId = tr.reader[TextConst.AVSpecColumnGrset.OnColsGrSetId].ToString();
                        var dimId = clearColId(tr.reader[TextConst.AVSpecColumnGrset.OnColsColId].ToString());
                        var grset = tri.Grsets[grsetId];
                        if (!grset.DimValues.ContainsKey(dimId))
                        {
                            addDimValFromRow(grset, null, null, null, tr.reader);
                        }
                        var dval = grset.DimValues[dimId];
                        foreach (var fi in grset.FactColumnsInfo.Values)
                        {
                            if (!table.Columns.Contains(fi.Name)) continue;// для запроса с выбором колонок
                            if (table.GetColumn(fi.Name).IsClientCalculations) continue;
                            var colName = combineColumnName(tri, grset, fi.Name, dval);


                            if (grset.IntervalBeginColumnName == null)
                            {
                                tr.SetCurrentRowValue(colName, tr.reader[fi.Name]);
                            }
                            else
                            {
                                
                                var dvalIndex1 = grset.DimValues.IndexOfKey(dimId);
                                object endValue = tr.reader[grset.IntervalEndColumnName];
                                decimal valToAdd = (decimal)Cmn.Nvl(tr.reader[fi.Name], (decimal)0);

                                updateRowWithIntervalValue(dvalIndex1, grset, endValue, null, tr, fi.Name, valToAdd, tri);
                            }

                        }

                        
                    }
                }
                else
                {
                    read = false;
                    res = false;
                }
            }

            return res;
        }


        private static void updateRowWithIntervalValue(int dvalIndex1,GrsetData grset,object endValue, DataRow targetRow,TableReference tr, string factColName,decimal valToAdd, TransposeDataStructure data) 
        {
            var dvalIndex = dvalIndex1;
            while (true) // сальдо добавляется во все колонки за период, в течении которого оно действует
            {
                
                DimensionValue dval = grset.DimValues.Values.ElementAt(dvalIndex);
                if (endValue != DBNull.Value)
                {
                    if (dval.NumericValue >= Convert.ToDecimal(endValue))
                    {
                        break;
                    }
                }
                //if (dval.NumericValue)
                var newColName = combineColumnName(data, grset, factColName, dval);
                if (rowOrTrColumnExists(targetRow,tr,newColName))
                {
                    if (targetRow != null)
                    {
                        targetRow[newColName] = (decimal)Cmn.Nvl(targetRow[newColName], (decimal)0) + valToAdd;
                    }
                    else
                    {
                        var oldval = (decimal)Cmn.Nvl(tr.GetCurrentRowValue(newColName), (decimal)0);
                        var newVal = oldval + valToAdd;
                        tr.SetCurrentRowValue(newColName, newVal);
                    }
                }
                dvalIndex++;
                if (dvalIndex == grset.DimValues.Count)
                {
                    break;
                }

            }
        }
        private static void processRows(VDataTable table, TransposeDataStructure data)
        {
            table.SuppressChangeEvent();
            foreach (RowSet rs in data.RowSets.Values)
            {
                foreach (GrsetData grset in data.Grsets.Values)
                {

                    foreach (var factCol in grset.FactColumnsInfo.Values)
                    {
                        if (!table.Columns.Contains(factCol.Name)) continue;// для зароса с выбором колон
                        if (table.GetColumn(factCol.Name).IsClientCalculations) continue;
                        var factColName = factCol.Name;
                        string prevColName = null;
                        if (grset.IntervalBeginColumnName == null)
                        {
                            foreach (DimensionValue dval in grset.DimValues.Values)
                            {
                                var newColName = combineColumnName(data,grset, factColName, dval);
                                if (rs.OtherRows.ContainsKey(grset.Name))
                                {
                                    if (rs.OtherRows[grset.Name].ContainsKey(dval.Id))
                                    {
                                        rs.MainRow[newColName] = rs.OtherRows[grset.Name][dval.Id][0][factColName];
                                    }
                                }
                               
                                prevColName = newColName;
                            }
                        }
                        else if (rs.OtherRows.ContainsKey(grset.Name))
                        {
                            foreach (var othr in rs.OtherRows[grset.Name]) 
                            {
                                
                                if (!grset.DimValues.ContainsKey(othr.Key))
                                {
                                    addDimValFromRow(grset, othr.Value[0], null, null, null);
                                }
                                var dvalIndex1 = grset.DimValues.IndexOfKey(othr.Key);

                                
                                foreach (DataRow row in othr.Value) 
                                {
                                    object endValue =  row[grset.IntervalEndColumnName];
                                    decimal valToAdd = (decimal)Cmn.Nvl(row[factColName], (decimal)0);
                                    var targetRow = rs.MainRow;
                                    updateRowWithIntervalValue(dvalIndex1, grset, endValue, targetRow,null, factColName, valToAdd, data);
                                }
                            }
                        }
                    }
                }
                foreach (var rr in rs.OtherRows)
                {
                    foreach (var r in rr.Value.Values.SelectMany(rows => rows))
                    {
                        table.Rows.Remove(r);
                    }

                }
               
            }
            table.ResumeChangeEvent();
        }

        private static void Transpose(VDataTable table)
        {
            XElement ginfo = table.GetGroupingInfo();
            var data = AnalyzeData(table, ginfo);

            if (!table.IsReader)
            {
                processColumns(table, data);
                processRows(table, data);
            }
            else
            {
                processColumns(table, data);
            }
            data.RowSets = null;// чтобы не хранить кучу данных, разделять лень
            table.TransposeStructure = data;
            if (!table.IsReader)
            {
                postProcessbands(table.Scheme.Element(TextConst.EName.ViewColumns),data);
            }

        }

        private static void postProcessbands(XElement viewcolumns,TransposeDataStructure data)
        {
            var factNames = data.Grsets.Values.SelectMany(g => g.FactColumnsInfo.Values).Select(fi => fi.Name).ToArray();
            // <band title="name1 | name2">...</band> в  
            //<band title="name1"> <band title="name2"> ...</band></band>

            viewcolumns.Elements(TextConst.AName.Column).Where(e=>factNames.Contains( Cmn.GetAttrValue(e,TextConst.AName.Name))).Remove();// частный случай, убирается колонка с аггрегированным в целом по строке значением, доделать

            foreach (var xband in viewcolumns.Elements(TextConst.EName.Band).ToArray())
            {
                var bname = Cmn.GetAttrValue(xband, TextConst.AName.Title);
                var bnamea = bname.Split(new string[]{" | "},StringSplitOptions.None);

            
                if (bnamea.Length > 1)
                {
                    var els = xband.Elements().ToArray();

                    XElement parent = null;
                    foreach (var s in bnamea)
                    {
                        var xband1 = new XElement(TextConst.EName.Band, new XAttribute(TextConst.AName.Title, s));
                        if (parent != null)
                        {
                            parent.Add(xband1);
                        }
                        else
                        {
                            xband.AddAfterSelf(xband1);
                        }
                        parent = xband1;

                    }
                    if (parent != null)
                    {
                        xband.Remove();
                        if (els.Length == 1 && els.First().Name.LocalName == TextConst.AName.Column) // если один факт то его наименование не выводится, это корректно только для частного случая, доделать
                        {
                            els.Remove();
                            parent.ReplaceWith(els);
                            els.First().Attributes(TextConst.AName.Title).Remove();
                            els.First().Add(parent.Attribute(TextConst.AName.Title));
                        }
                        else
                        {

                            parent.Add(els);
                        }
                    }
                }
            }

            //// merge бендов, работает, но пока убрал
            //var go = true;
            //while (go)
            //{
            //    XElement xel1 = null;
            //    go = false;
            //    foreach (var xel in viewcolumns.Elements().ToArray())
            //    {

            //        if (xel1 != null && xel1.Name.LocalName == TextConst.EName.Band &&
            //            xel1.Name.LocalName == TextConst.EName.Band &&
            //            Cmn.GetAttrValue(xel1, TextConst.AName.Title) == Cmn.GetAttrValue(xel, TextConst.AName.Title))
            //        {
            //            xel.Remove();
            //            xel1.Add(xel.Elements());
            //            go = true;
            //        }
            //        else
            //        {
            //            xel1 = xel;
            //        }
            //    }
            //}
        }

        private static void copyColumn(VDataTable table,string name, string newName, string newTitle,string bandTitle,string prevColName=null)
        {
            var xcolumns = table.Scheme.Element(TextConst.EName.Columns);
            var xviewcolumns = table.Scheme.Element(TextConst.EName.ViewColumns);

            var xcolumn=xcolumns.Elements().First(e=>Cmn.GetAttrValue(e,TextConst.AName.Name)==name);
            var newxcolumn = new XElement(xcolumn);
            newxcolumn.SetAttributeValue(TextConst.AName.Name, newName);
            newxcolumn.SetAttributeValue(TextConst.AName.Title, newTitle);
            xcolumns.Add(newxcolumn);

            xcolumn = xviewcolumns.Descendants(TextConst.EName.Column).First(e => Cmn.GetAttrValue(e, TextConst.AName.Name) == name);
            newxcolumn = new XElement(xcolumn);
            newxcolumn.SetAttributeValue(TextConst.AName.Name, newName);
            newxcolumn.SetAttributeValue(TextConst.AName.Title, newTitle);

            XElement xprevElement = null;
            
            if (prevColName != null)
            {
                 xprevElement = xviewcolumns.Descendants(TextConst.EName.Column).First(e => Cmn.GetAttrValue(e, TextConst.AName.Name) == prevColName);
            }
            else
            {
                 xprevElement = xcolumn;
               
            }

            XElement xparentElement = null;
            if (bandTitle != null)
            {
                bool bandExists=false;
                if (xprevElement.Parent.Name.LocalName == TextConst.EName.Band)
                {
                    if (Cmn.GetAttrValue(xprevElement.Parent, TextConst.AName.Title) == bandTitle)
                    {
                        xparentElement = xprevElement.Parent;
                        bandExists = true;
                    }
                    else
                    {
                        xprevElement = xprevElement.Parent;
                    }
                    
                }
                if (!bandExists)
                {
                    var xband = new XElement(TextConst.EName.Band);
                    xband.SetAttributeValue(TextConst.AName.Title, bandTitle);
                    xprevElement.AddAfterSelf(xband);
                    xparentElement = xband;
                }
               
            }


            if (xparentElement != null)
            {
                xparentElement.Add(newxcolumn);
            }
            else
            {
                xprevElement.AddAfterSelf(newxcolumn);
            }
            DataColumn column = table.Columns[name];
            table.AddColumn(newName, column.DataType, newTitle);
        }
    }
}
