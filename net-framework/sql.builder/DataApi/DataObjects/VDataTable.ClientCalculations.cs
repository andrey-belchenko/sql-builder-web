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
        public bool HasClientCalculations;
      
        public void DoClientCalculationsForRow(VClientCalculations.DataAccessor dataAccessor)
        {
            if (!HasClientCalculations) return;
            SuppressChangeEvent();
            foreach (VDataColumn col in Columns.Cast<VDataColumn>().Where(c => c.IsClientCalculations))
            {
                col.EvaluateExpression(dataAccessor);
            }
            ResumeChangeEvent();
        }

        public void DoClientCalculations()
        {
           
            if (!HasClientCalculations) return;
            var lastRowsOfGrset = new SortedList<string, DataRow>();

            foreach (DataRow row in Rows)
            {
                var dataAccessor = new VClientCalculations.DataAccessor();
                dataAccessor.Row = row;
                if (this.Columns.Contains(TextConst.AVSpecColumnGrset.GrSetName))
                {
                    dataAccessor.LastRowsOfGrset = lastRowsOfGrset;
                }
                DoClientCalculationsForRow(dataAccessor);

                if (dataAccessor.LastRowsOfGrset!=null)
                {
                    lastRowsOfGrset[row[TextConst.AVSpecColumnGrset.GrSetName].ToString()] = row;
                }
            }

        }

       
    }

    internal interface IClientCalculationCall
    {
        object Evaluate(VClientCalculations.DataAccessor dataAccessor);
    }

   
    internal static partial class VClientCalculations
    {
        public enum Errors { NeedDataAccessor };
        private static FactParam ParseExpression(XElement xexpression)
        {
            FactParam ret = null;
            switch (xexpression.Name.LocalName)
            {
                case TextConst.EName.Call: ret=ParseCall(xexpression);
                    break;
                case TextConst.EName.Const: ret=ParseConst(xexpression);
                    break;
                case TextConst.EName.Column: ret= ParseColumn(xexpression);
                    break;
            }
            
            return ret;
        }

        public static Call ParseCall(XElement xexpression)
        {
            var call = new Call();
            var funcName = xexpression.Attribute(TextConst.AName.Function).Value;
            //var xfunc = XmlReports.Environment.GetElement(TextConst.EName.Functions, funcName);
            call.FunctionName = funcName;
            if (xexpression.Attribute(TextConst.AName.RowSelector) != null)
            {
                call.RowSelector = ParseRowSelector(xexpression.Attribute(TextConst.AName.RowSelector).Value);
            }
            foreach (XElement par in xexpression.Elements())
            {
                var p = ParseExpression(par);
                call.Pars.Add(p);
            }

            return call;
        }


        private static FactParamConst ParseConst(XElement xexpression)
        {
            var c = new FactParamConst();
            c.Value = Cmn.EvaluateOracleConst(xexpression.Value);
            return c;
        }

        private static FactParamColumn ParseColumn(XElement xexpression)
        {

            var c = new FactParamColumn();
            c.Name = xexpression.Attribute(TextConst.AName.Column).Value;
            if (xexpression.Attribute(TextConst.AName.RowSelector) != null)
            {
                c.RowSelector = ParseRowSelector(xexpression.Attribute(TextConst.AName.RowSelector).Value);
            }
            return c;
        }




        public static DataAccessor FindRowUsingRowSelector_Parent(DataAccessor dataAccessor)
        {
            var newDA = dataAccessor.Copy();
            if (newDA.Row != null)
            {
                var tbl = (VDataTable)newDA.Row.Table;
                var parentKey = newDA.Row[tbl.TreeParentFieldName];
                newDA.Row = tbl.Rows.Find(parentKey);
            }
            else
            {
                newDA.TableReference = newDA.TableReference.GetParentTableReference();
            }
            return newDA;
        }


        public static DataAccessor FindRowUsingRowSelector_Prev(DataAccessor dataAccessor)
        {
            var newDA = dataAccessor.Copy();
            newDA.MoveToPrevious();
            return newDA;
        }


        public static DataAccessor FindRowUsingRowSelector_PrevSibling(DataAccessor dataAccessor)
        {
            var newDA = dataAccessor.Copy();
            newDA.MoveToPreviousSibling();
            return newDA;
        }
        public static DataAccessor FindRowUsingRowSelector_PrevRow(DataAccessor dataAccessor)
        {
            var newDA = dataAccessor.Copy();
            newDA.MoveToPreviousRow();
            return newDA;
        }
        public static DataAccessor FindRowUsingRowSelector_IsGrset(DataAccessor dataAccessor, string[] selectorParam)
        {
            if (!selectorParam.Contains(dataAccessor.GetValue(TextConst.AVSpecColumnGrset.OrigGrSetName).ToString()))
            {
                var newDA = dataAccessor.Copy();
                newDA.ClearSource();
                return newDA;
            }
            else
            {
                return dataAccessor;
            }
        }

        public static List<RowSelector> ParseRowSelector(string rowSelectorExpr)
        {
            var rslist = rowSelectorExpr.Split('.');
            var rowSelectorList = new List<RowSelector>();
            
            foreach (var rs in rslist)
            {
                var ss = rs.Split(new char[] { '(', ')' });
              
                var rowSelector = new RowSelector();
                rowSelector.Name = ss[0];
                if (ss.Length > 1)
                {
                    rowSelector.Param = ss[1].Split(',');
                }

                rowSelectorList.Add(rowSelector);
               
            }
            return rowSelectorList;
        }
        internal class FactParam
        {
            internal List<RowSelector> RowSelector;
            internal DataAccessor dataAccessor;
            public virtual object Evaluate(DataAccessor dataAccessor)
            {
                return null;
            }
            internal object Evaluate()
            {                
                return Evaluate(this.dataAccessor);
            }
            protected DataAccessor ApplySelector(DataAccessor dataAccessor)
            {
                if (RowSelector != null)
                {
                 
                    foreach (var rs in RowSelector)
                    {
                        dataAccessor=rs.Evaluate(dataAccessor);
                        if (!dataAccessor.HasSource()) break;
                    }
                   
                }
                return dataAccessor;
            }
            internal static bool ValueIsNull(FactParam p)
            {
                return Cmn.IsNullOrDBNull(p.Evaluate());
            }
        }
        internal class Call : FactParam, IClientCalculationCall
        {
            public string FunctionName = null;
            public List<FactParam> Pars = new List<FactParam>();
            public override object Evaluate(DataAccessor dataAccessor)
            {
                

                ApplySelector( dataAccessor);



                if (dataAccessor != null)
                {
                    if (!dataAccessor.HasSource())
                    {
                        return null;
                    }
                }

                InitFuncsImpementation();
                var parvals = new List<object>();
                foreach (FactParam par in Pars)
                {
                    par.dataAccessor = dataAccessor;
                }
                var val = AllFuncs[FunctionName](Pars.ToArray());
                return val;

            }
        }

       
       

        internal class FactParamColumn : FactParam
        {
            public string Name = null;

            public override object Evaluate(DataAccessor dataAccessor)
            {
                
               dataAccessor= ApplySelector( dataAccessor);

               if (dataAccessor == null)
               {
                   return Errors.NeedDataAccessor;
               }

                if (!dataAccessor.HasSource())
                {
                    return null;
                }
               
                var name = Name;
                if (dataAccessor.DimensionName != null)
                {
                    VDataTable tbl = null;
                    if (dataAccessor.Row != null)
                    {
                        tbl = (VDataTable)dataAccessor.Row.Table;
                    }
                    else
                    {
                        tbl = (VDataTable)dataAccessor.TableReference.Table;
                    }
                    var col = tbl.GetColumn(Name);
                    name = tbl.TransposeStructure.GetColumnNameForForDimValue(name, dataAccessor.DimensionValue);
                }
                object val = dataAccessor.GetValue(name);
                
                return val;
                
            }
        }

        internal class FactParamConst : FactParam
        {
            public object Value = null;
            public override object Evaluate(DataAccessor dataAccessor)
            {
                return Value;
            }
        }

        internal class DataAccessor
        {
            public string DimensionName = null;
            public string DimensionValue = null;
           public DataRow Row = null;
           public TableReference TableReference = null;
           public SortedList<string, DataRow> LastRowsOfGrset = null;
           public DataAccessor Copy()
           {
               var da = new DataAccessor();
               da.DimensionValue = DimensionValue;
               da.DimensionName = DimensionName;
               da.Row = Row;
               da.TableReference = TableReference;
               da.LastRowsOfGrset = LastRowsOfGrset;
               da.isPrevious = isPrevious;
               return da;
           }

           public bool IsColumnExists(string columnName)
           {
               if (Row != null)
               {
                   return Row.Table.Columns.Contains(columnName);
               }
               else
               {
                   return TableReference.IsCurrentRowValueExists(columnName);
               }
           }
            public object GetValue(string columnName)
            {
                object val = null;
                if (Row != null)
                {
                    val= Row[columnName];
                }
                else
                {
                    
                    if (isPrevious)
                    {
                        val = TableReference.GetPrevRowValue(columnName);
                       
                    }
                    else
                    {
                        if (TableReference.IsCurrentRowExists())
                        {
                            val = TableReference.GetCurrentRowValue(columnName);
                        }
                        else if (canUsePrevious)
                        {
                            val = TableReference.GetPrevRowValue(columnName);
                        }
                    }
                }
                return Cmn.Nvl(val, null);
            }

            public object TryGetValue(string columnName)
            {
                object val = null;
                if (IsColumnExists(columnName))
                {
                    val = GetValue(columnName);
                }
                return Cmn.Nvl(val, null);
            }


            public void SetValue(string columnName,object value)
            {
                if (Row != null)
                {
                     Row[columnName]=value;
                }
                else
                {
                     TableReference.SetCurrentRowValue(columnName,value);
                }
            }
            public void ClearSource()
            {
                Row = null;
                TableReference = null;
            }
            public bool HasSource()
            {
                return ((Row != null) || (TableReference != null));
            }

            private bool isPrevious = false;
            private bool canUsePrevious = false;
            public void MoveToPreviousRow()
            {
                if (Row != null)
                {
                    var index = Row.Table.Rows.IndexOf(Row);
                    if (index > 0)
                    {
                        Row = Row.Table.Rows[index - 1];
                    }
                    else
                    {
                        ClearSource();
                    }

                }
                else
                {
                    if (TableReference == TableReference.LastReadRowTableReference)
                    {
                        isPrevious = true;

                    }
                    else
                    {
                        canUsePrevious = true;
                        if (TableReference.LastReadRowTableReference != null && TableReference.MainTableName == TableReference.LastReadRowTableReference.MainTableName)
                        {
                            TableReference = TableReference.LastReadRowTableReference;
                        }
                        else
                        {
                            TableReference = null;
                        }
                    }
                    
                }

            }
            public void MoveToPreviousSibling()
            {
                string parentId=null;
                if (IsColumnExists(TextConst.AVSpecColumnGrset.GrSetName))
                {
                    parentId = GetValue(TextConst.AVSpecColumnGrset.ParentGrRowId).ToString();
                }
                MoveToPrevious();
                if (parentId != null)
                {
                    if (HasSource())
                    {
                        if (Cmn.Nvl(TryGetValue(TextConst.AVSpecColumnGrset.ParentGrRowId), "").ToString() != parentId)
                        {
                            ClearSource();
                        }
                    }
                }
            }
            public void MoveToPrevious()
            {
                if (Row != null)
                {
                    if (LastRowsOfGrset == null)
                    {
                        var index = Row.Table.Rows.IndexOf(Row);
                        if (index > 0)
                        {
                            Row = Row.Table.Rows[index - 1];
                        }
                        else
                        {
                            ClearSource();
                        }
                    }
                    else
                    {
                        var gsetId = Row[TextConst.AVSpecColumnGrset.GrSetName].ToString();
                        if (LastRowsOfGrset.ContainsKey(gsetId))
                        {
                            Row = LastRowsOfGrset[gsetId];
                        }
                        else
                        {
                            Row = null;
                        }
                    }

                }
                else
                {
                    isPrevious = true;
                }

            }
        }

        internal class RowSelector
        {
            public string Name = null;
            public string[] Param = null;
            public DataAccessor Evaluate(DataAccessor dataAccessor)
            {
                switch (Name)
                {
                    case TextConst.AVRowSelector.Parent: return FindRowUsingRowSelector_Parent(dataAccessor); break;
                    case TextConst.AVRowSelector.IsGrset: return FindRowUsingRowSelector_IsGrset(dataAccessor, Param); break;
                    case TextConst.AVRowSelector.Prev: return FindRowUsingRowSelector_Prev(dataAccessor); break;
                    case TextConst.AVRowSelector.PrevSibling: return FindRowUsingRowSelector_PrevSibling(dataAccessor); break;
                    case TextConst.AVRowSelector.PrevRow: return FindRowUsingRowSelector_PrevRow(dataAccessor); break;
                    default: throw new NotImplementedException();
                }
             
               
            }
        }
       
        

        
    }
}
