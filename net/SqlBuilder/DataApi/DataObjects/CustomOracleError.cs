using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;
////using System.Windows.Forms;
using System.Xml.Linq;
using Devart.Data.Oracle;
//using DevExpress.XtraEditors;
//using infoenergo.ui.win.Forms;
using sql.builder.Controls;
using sql.builder.WinForms;
using sql.builder.DataApi;

namespace sql.builder.DataApi.DataObjects
{
    internal class CustomOracleError
    {
        internal static readonly int[] Codes = { 2292 };

        internal static void HandleIfNeed(DataRow row, OracleException ex)
        {
            switch (ex.Code) {
                case 2292:
                    Handle2292(row, ex);
                    break;
            }
        }
        private static void Handle2292(DataRow row, OracleException ex)
        {
            // парсим сообщение, типа такого
            // ORA-02292: integrity constraint (ASUSE.UR_GRAF_OPL_UR_GRAF) violated - child record found
            Match result = Regex.Match(ex.Message, @".*\((([a-zA-Z_]*)\.([a-zA-Z_]*))\).*");
            if (result.Groups.Count != 4) return;

            string schema_name = result.Groups[2].Value;
            string constraint_name = result.Groups[3].Value;

            DataTable dt = db.SelectConstraintTableColumns(schema_name, constraint_name);
            int count = dt.Rows.Count;
            if (count <= 0) return;

            VQuery query = XmlReports.Environment.GetQuery(dt.Rows[0].Field<string>("TABLE_NAME").ToLower());
            if (query == null) return;

            XElement xquery = query.AsListQuery();
            xquery = Compiler.PreCompileQuery(xquery, true);

            IEnumerable<XElement> cols = query.Element(EName.select).Elements(EName.column);
            IList<XElement> xcolumns = new List<XElement>(count);
            for (int index = 0; index < count; index++) {
                string column_name = dt.Rows[index].Field<string>("COLUMN_NAME");
                XElement col = cols.FirstOrDefault(c => string.Compare(c.AttrOrEmpty(AName.column), column_name, true) == 0);
                if (col != null) {
                    xcolumns.Add(col);
                }
            }
            if (xcolumns.Count == 0) return;

            XElement xwhere = xquery.Element(EName.where);
            if (xwhere == null) {
                xwhere = new XElement(EName.where);
                xquery.Add(xwhere);
            }
            XElement xand = xquery.Elements(EName.call).SearchByAttribute(AName.function, TextConst.AVFunction.And);
            if (xand == null) {
                xand = Factory.NewCall(TextConst.AVFunction.And);
                xwhere.Add(xand);
            }
            foreach (XElement xcolumn in xcolumns) {
                DataRowVersion rv = (row.RowState != DataRowState.Deleted) ? DataRowVersion.Default : DataRowVersion.Original;
                string value = row[xcolumn.Attribute(TextConst.AName.Column).Value, rv].ToString();
                string str = (xcolumn.Attribute(AName.type).Value == TextConst.AVDataType.String) ? "'" + value + "'" : value;
                xand.Add(Factory.NewCall(TextConst.AVFunction.Equal, xcolumn, Factory.NewConst(str)));
            }
            VReport report = XmlReports.Environment.GetPrecompiledReport(xquery);
            VDataSet ds = report.Result(2, false);
            ds.Refresh();
            string title = query.P_Title;
            if (title == string.Empty) {
                title = query.P_Name;
            }
            //frmDataError.Show(frmDataError.ErrorType.ChildRecordsExist,ds,title);
        }
    }
}