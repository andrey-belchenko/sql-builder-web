using System;
using System.Xml.Linq;
namespace sql.builder.DataApi
{
    public partial class VDataColumn
    {


        public bool IsExcelCalculations = false;
        private bool _clientCalculaton = false;
        private XElement excelFormula = null;
        public XElement GetExcelFormula()
        {
            if (!IsExcelCalculations)
            {
                return null;
            }

            if (XExpression == null)
            {
                findAndSetAndParseClientCalculationsExpr();
                var ret = new XElement(TextConst.EName.Root);
                Compiler.eFunction(XExpression, ret, "excel");
                // Compiler.eFunction(XExpression, ret,"excel");
                excelFormula = ret;
            }
            return excelFormula;

        }

        public bool IsClientCalculations
        {
            get
            {
                return _clientCalculaton;
            }
            set
            {
                _clientCalculaton = value;
                if (_clientCalculaton)
                {
                    if (OriginalNameForPivotColumn == null)
                    {
                        if (XExpression == null)
                        {
                            findAndSetAndParseClientCalculationsExpr();
                        }
                        GetTable().HasClientCalculations = true;
                    }
                }
            }
        }

        private void findAndSetClientCalculationsExpr()
        {
            var xquery = XmlReports.Environment.GetQuery(this.GetTable().QueryName);
            var colName = OriginalNameForPivotColumn;
            if (colName == null)
            {
                colName = ColumnName;
            }
            var xcol = new XElement(xquery.SearchColumn(colName));
            Compiler.processingArrays(xcol);
            XExpression = xcol;


        }

        private void findAndSetAndParseClientCalculationsExpr()
        {
            findAndSetClientCalculationsExpr();
            ParsedExpression = VClientCalculations.ParseCall(XExpression);

        }
        public IClientCalculationCall ParsedExpression = null;
        public XElement XExpression = null;

        public void EvaluateExpression(VClientCalculations.DataAccessor dataAccessor)
        {

            dataAccessor.DimensionName = PivotDimensionName;
            if (dataAccessor.DimensionName != null)
            {
                dataAccessor.DimensionValue = PivotDimensionValue.ToString();
            }
            var exp = ParsedExpression;
            if (OriginalNameForPivotColumn != null)
            {
                exp = GetTable().GetColumn(OriginalNameForPivotColumn).ParsedExpression;
            }
            var val = exp.Evaluate(dataAccessor);
            val = Cmn.Nvl(val, DBNull.Value);
            dataAccessor.SetValue(this.ColumnName, val);

        }





    }
}
