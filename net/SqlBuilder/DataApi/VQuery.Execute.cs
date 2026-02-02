using System.Linq;
using System.Data;
using System;
using System.Collections.Generic;
using System.Xml.Linq;
//using infoenergo.core.Extensions;
using System.Text;
using System.Reflection;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    public partial class VQuery
    {
        public VDBSelectCommand GetSelectCommand(bool useCache)
         {
            if (useCache) {
                 if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), null)) {
                     return GetCashValue(MethodBase.GetCurrentMethod().ToString(), null) as VDBSelectCommand;
                 }
            }
            //
            var scheme = XmlReports.Environment.Manager.GetScheme();
            XElement query = new XElement(this);
            VReport.PreprocessSimpleParams(query);
            query.RemoveAttribute(AName_.name);
            XElement compiledQuery = Compiler.GetCompiledQuery(query);
            compiledQuery = Compiler.FinalProcessingQuery(compiledQuery);
            VDBSelectCommand cmd = VDBSelectCommand.CreateFromCompiledQuery(query, compiledQuery);
            //
            if (useCache) {
                 AddCashValue(cmd, MethodBase.GetCurrentMethod().ToString(), null);
            }       
            return cmd;
         }
         public VDBSelectCommand GetSelectCommandWithTemp(VDataSet ds,string keyDimension) 
         {
             var qry = new XElement(this);
             qry.Attributes(TextConst.AName.Name).Remove();


             var xpar = new XElement(TextConst.EName.Param);
             xpar.SetAttributeValue(TextConst.AName.Name, TextConst.DBParams.IsNewRowParam);
             xpar.SetAttributeValue(TextConst.AName.DataType, TextConst.AVDataType.Number);
             qry.Element(TextConst.EName.Params).Add(xpar);


             xpar = new XElement(TextConst.EName.Param);
             xpar.SetAttributeValue(TextConst.AName.Name, TextConst.DBParams.FormId);
             xpar.SetAttributeValue(TextConst.AName.DataType, TextConst.AVDataType.Number);
             qry.Element(TextConst.EName.Params).Add(xpar);

             xpar = new XElement(TextConst.EName.Param);
             xpar.SetAttributeValue(TextConst.AName.Name, TextConst.DBParams.TempRowId);
             xpar.SetAttributeValue(TextConst.AName.DataType, TextConst.AVDataType.Number);
             qry.Element(TextConst.EName.Params).Add(xpar);


             VReport.PreprocessSimpleParams(qry);
             XElement compiledQuery = Compiler.GetCompiledQuery(qry);
             VForm.MultiplicateSources(compiledQuery, ds, null);
             foreach (VDataTable tbl in ds.Tables)
             {
                 if (!string.IsNullOrEmpty(tbl.UpdateableTableName))
                 {
                   VForm .ChangeQueryTableForUsingTemp(compiledQuery, tbl.UpdateableTableName, tbl, null,false, null,null);
                 }
             }
             compiledQuery = Compiler.FinalProcessingQuery(compiledQuery);
             return VDBSelectCommand.CreateFromCompiledQuery(qry, compiledQuery);
         }        
         public VDBSelectCommand GetInsertCommand(string targetName)
         {

             if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), targetName))
             {
                 return GetCashValue(MethodBase.GetCurrentMethod().ToString(), targetName) as VDBSelectCommand;
             }




             VDBSelectCommand cmd = GetSelectCommand(false);

             var targetQuery = XmlReports.Environment.GetQuery(targetName);
             var q = "";

             var sql = new StringBuilder();
             var sqlCols = new StringBuilder();
             var sqlVals = new StringBuilder();
             var cursor = "r";

             foreach (VSXElement col in Columns())
             {
                 var tagCol = targetQuery.SearchColumn(col.XName);
                 if (tagCol != null)
                 {
                     sqlCols.Append(q);
                     sqlCols.AppendLine(tagCol.XName);

                     sqlVals.Append(q);
                     sqlVals.AppendLine(cursor + "." + tagCol.XName);
                     //cols += q + tagCol.XName;
                     q = ",";

                   
                 }
             }
             var cls = (VQuery)targetQuery.GetMainIE();

             var keyName = cls.KeyColumn().P_Column;

             sql.AppendLine("begin");
             sql.AppendLine("for " + cursor + " in (");
             sql.AppendLine(cmd.GetCommandText());
             sql.AppendLine(") loop");
             sql.AppendLine("insert into " + cls.P_IdName);
             sql.AppendLine("(" + sqlCols.ToString() + ")");
             sql.AppendLine(" values ");
             sql.AppendLine("(" + sqlVals.ToString() + ")");
             sql.AppendLine(" returning ");
             sql.AppendLine(keyName + " into " +  TextConst.Pfx.Param + TextConst.DBParams.PrimaryKeyParam + ";");
             sql.AppendLine("end loop;");
             sql.AppendLine("end;");


            
             string text = sql.ToString();


          

            

             cmd.SetCommandText(text);
             cmd.CreateRetParam();

             AddCashValue(cmd, MethodBase.GetCurrentMethod().ToString(), targetName);

      
             return cmd;
         }
         public VDBSelectCommand GetUpdateCommand(string targetName)
         {
             if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), targetName)) {
                 return GetCashValue(MethodBase.GetCurrentMethod().ToString(), targetName) as VDBSelectCommand;
             }
             VDBSelectCommand cmd = GetSelectCommand(false);
             VQuery targetQuery = XmlReports.Environment.GetQuery(targetName);
             VQuery cls = (VQuery)targetQuery.GetMainIE();
             string keyName = cls.KeyColumn().P_Column;
             var sql = new StringBuilder();
             sql.AppendLine("begin");
             sql.AppendLine("for r in (");
             sql.AppendLine(cmd.GetCommandText());
             sql.AppendLine(") loop");
             sql.Append("update ");
             sql.AppendLine(cls.P_IdName);
             sql.Append(" set ");
             int count = 0;
             IList<VSXElement> cols = this.Columns();
             for (int index = 0; index < cols.Count; index++) {
                 VSXElement tagCol = targetQuery.SearchColumn(cols[index].XName);
                 if (tagCol != null) {
                     if (count > 0) {
                         sql.Append(", ");
                     }
                     string column_name = tagCol.XName;
                     sql.Append(column_name);
                     sql.Append(" = r.");
                     sql.Append(column_name);
                     count++;
                 }
             }
             sql.AppendLine();
             sql.Append(" where ");
             sql.Append(keyName);
             sql.Append(" = r.");
             sql.Append(keyName);
             sql.AppendLine(";");
             sql.AppendLine("end loop;");
             sql.AppendLine("end;");
             string text = sql.ToString();
             cmd.SetCommandText(text);
             AddCashValue(cmd, MethodBase.GetCurrentMethod().ToString(), targetName);
             return cmd;
         }
         public VDBSelectCommand GetDeleteCommand(string targetName)
         {

             if (IsCashValueExists(MethodBase.GetCurrentMethod().ToString(), targetName))
             {
                 return GetCashValue(MethodBase.GetCurrentMethod().ToString(), targetName) as VDBSelectCommand;
             }
             VDBSelectCommand cmd = GetSelectCommand(false);

             var targetQuery = XmlReports.Environment.GetQuery(targetName);
        

             var sql = new StringBuilder();
     
     

            
             var cls = (VQuery)targetQuery.GetMainIE();

             var keyName = cls.KeyColumn().P_Column;
             sql.AppendLine("begin");
             sql.AppendLine("delete " + cls.P_IdName + " where "+keyName+ " in (");
             sql.AppendLine(cmd.GetCommandText());
             sql.AppendLine(");");
 
             sql.AppendLine("end;");



             string text = sql.ToString();
             cmd.SetCommandText(text);
             // cmd.CreateRetParam();
             AddCashValue(cmd, MethodBase.GetCurrentMethod().ToString(), targetName);
             return cmd;
         }
         public static object ExecuteQueryReturnScalar(string name)
         {
             VQuery query = XmlReports.Environment.GetQuery(name);
             VDBSelectCommand selCmd = query.GetSelectCommand(true);
             object result;
             using (DataTable tbl = selCmd.ExecuteDataTable(XmlReports.Environment.Connection)) {
                 if (tbl.Rows.Count == 0) {
                     result = DBNull.Value;
                 } else {
                     result = tbl.Rows[0][0];
                 }
             }
             return result;
         }
    }
}