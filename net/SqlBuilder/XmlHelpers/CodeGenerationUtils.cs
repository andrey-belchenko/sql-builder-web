using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
//using DevExpress.XtraCharts.Design;
using sql.builder.DataApi;


namespace sql.builder.XmlHelpers
{
    public static class CodeGenerationUtils
    {


        public static string ExecuteMerge(string reportName)
        {
            return ExecuteMergeOrDelete(reportName, false);
        }
        public static string ExecuteDelete(string reportName)
        {
            return ExecuteMergeOrDelete(reportName, true);
        }
        private static string ExecuteMergeOrDelete(string reportName, bool isDelete)
        {
            var sProc = SqlReportPkg.Generate(reportName, true, true, true, isDelete);
            var rep = XmlReports.Environment.GetPrecompiledReport(reportName);
            var sParams = GetParamsLine(rep);
            var sUseParams = GetCodeAddOraclePars(rep);
            var methdName = ParseToUpper(rep.P_IdName);
            var sb = new StringBuilder();
            var retType = "object";
            if (isDelete)
            {
                retType = "void";
            }
            sb.AppendLine(string.Format("public {2} {0}({1})", methdName, sParams, retType));
            sb.AppendLine("{");
            sb.AppendLine(string.Format("var cmd = new VOracleCommand();"));
            if (!isDelete)
            {
                sb.AppendLine("object ret=null;");
            }

            sb.AppendLine("try {");
            sb.AppendLine("cmd.Connection=" + ConnectionExpr() + ";");
            sb.AppendLine(sUseParams);
            sb.AppendLine(string.Format("cmd.CommandText=@\"{0}\";", sProc));

            if (!isDelete)
            {
                sb.AppendLine("var par = new VOracleParameter();");
                sb.AppendLine(string.Format("par.ParameterName = \"{0}\";", TextConst.DBParams.PrimaryKeyParam));
                sb.AppendLine("par.OracleDbType = VOracleDbType.Number;");
                sb.AppendLine("par.Direction = ParameterDirection.Output;");
                sb.AppendLine("cmd.Parameters.Add(par);");
            }

            sb.AppendLine("SqlTrace.Trace(cmd);");
            sb.AppendLine("cmd.ExecuteNonQuery();");
            if (!isDelete)
            {
                sb.AppendLine("ret=par.Value;");
            }
            sb.AppendLine("} finally {");
            sb.AppendLine("cmd.Dispose();");
            sb.AppendLine("}");
            if (!isDelete)
            {
                sb.AppendLine("return ret;");
            }
            sb.AppendLine("}");
            return sb.ToString();
        }


        public static string ExecuteSelect(string reportName)
        {
            var rep = XmlReports.Environment.GetPrecompiledReport(reportName);
            var ds = rep.Result(2, false);
            var s = GenMethodCode(rep, reportName, ds);
            return s;
        }

        public static string MethodSelect(string reportName, bool retStatus)
        {
            var rep = XmlReports.Environment.GetPrecompiledReport(reportName);
            var ds = rep.Result(2, false);
            var s = GenWebMethodCode(rep, ds, reportName, retStatus);
            return s;
        }






        public static string ReportWithErrorResultClass(VReport rep, string fieldName = null)
        {

            var s1 = @" public class {0}
        {{
            public bool IsSuccess=true;
            public string ErrorText=null;
            public {1} {2};
        
        }}";
            var className = ParseToUpper(ReplaceWebPfx(rep.P_IdName)) + "Result";
            var mqry = rep.MainSource();
            var className1 = ParseToUpper(ReplaceClsPfx(GetQueryClassNameOrPrimitiveType(mqry)));
            if (fieldName == null)
            {
                fieldName = ParseToUpper(mqry.P_Alias);
            }


            var s2 = string.Format(s1, className, className1, fieldName);

            return s2;
        }

        public static string ClassDeclaration(string[] reportNames)
        {
            var sb = new StringBuilder();
            var names = new HashSet<string>();
            foreach (var reportName in reportNames)
            {
                var rep = XmlReports.Environment.GetPrecompiledReport(reportName);
                var ds = rep.Result(2, false);
                foreach (VDataTable tbl in ds.Tables)
                {
                    var className = GetQueryClassName(rep.GetQuery(tbl.TableName));
                    if (tbl.TableName == "error")
                    {
                        var sClassDef = ReportWithErrorResultClass(rep);
                        sb.AppendLine(sClassDef);
                    }
                    else if (!names.Contains(className))
                    {

                        names.Add(className);
                        var sClassDef = GenClass(tbl, rep);
                        sb.AppendLine(sClassDef);
                    }
                }

            }

            return sb.ToString();
        }

        public static string UpdateTempFromObject(string reportName)
        {
            var rep = XmlReports.Environment.GetPrecompiledReport(reportName);
            var ds = rep.Result(2, false);

            var sb = new StringBuilder();

            var className = ParseToUpper(ReplaceClsPfx(rep.MainSource().P_CalledQuery));
            var methodName = ParseToUpper(reportName);
            var objName = ParseParam(rep.MainSource().P_Alias);
            sb.AppendLine(string.Format("public  void {0} ({1} {2})", methodName, className, objName));

            sb.AppendLine("{");
            sb.AppendLine("VOracleParameter par =null;");
            sb.AppendLine("int idCounter = 0;");
            var tbl = ds.GetTable(rep.MainSource().P_Alias);
            var qry = rep.GetQuery(tbl.TableName);
            sb.Append(ClearTempTable(qry, tbl));
            var sb1 = InsertIntoTempTable(qry, tbl, rep);
            sb.Append(sb1);
            sb.AppendLine("}");
            return sb.ToString();
        }

        private static string GetParNameForColumn(string columnName)
        {
            return "p_" + columnName;
        }

        private static string ConnectionExpr()
        {
            return "_Connection";
        }

        private static StringBuilder ClearTempTable(VQueryCall queryCall, VDataTable dt)
        {



            var tblName = queryCall.P_CalledQuery;
            var sql = string.Format("delete {0}", tblName);

            var sb = new StringBuilder();


            var className = ParseToUpper(dt.TableName);

            var cmdName = "cmd" + className + "Del";

            sb.AppendLine(string.Format("var {0} = new VOracleCommand();", cmdName));
            sb.AppendLine("try {");
            sb.AppendLine(string.Format("{0}.Connection = {1};", cmdName, ConnectionExpr()));

            sb.AppendLine(string.Format("{0}.CommandText=\"{1}\";", cmdName, sql));

            sb.AppendLine(string.Format("SqlTrace.Trace({0});", cmdName));

            sb.AppendLine(string.Format("{0}.ExecuteNonQuery();", cmdName));
            sb.AppendLine("} finally {");
            sb.AppendLine(string.Format("{0}.Dispose();", cmdName));
            sb.AppendLine("}");


            return sb;
        }
        private static StringBuilder InsertIntoTempTable(VQueryCall queryCall, VDataTable dt, VReport rep)
        {


            var pkName = queryCall.Query().KeyColumn().XName;
            string fkName = null;
            if (dt.ParentRelations.Count > 0)
            {

                fkName = queryCall.GetRelChildColumnName();
            }
            IList<VDataColumn> realColumns = new List<VDataColumn>(dt.Columns.Count);
            for (int index = 0; index < dt.Columns.Count; index++)
            {
                VDataColumn col = (VDataColumn)dt.Columns[index];
                if (TextConst.AVColumn.IsNotSysColumn(col.ColumnName))
                {
                    realColumns.Add(col);
                }
            }
            /*var otherColumns =
                dt.Columns.Cast<VDataColumn>()
                    .Where(col => col.ColumnName != pkName && col.ColumnName != fkName)
                    .ToArray();*/
            //var fldsNames = realColumns.Select(c => c.ColumnName);
            //var parNames = fldsNames.Select(col => ":" + GetParNameForColumn(col));
            //var flds = string.Join(",", fldsNames);
            //var pars = string.Join(",", parNames);
            string flds = string.Empty;
            string pars = string.Empty;
            for (int index = 0; index < realColumns.Count; index++)
            {
                string col_name = realColumns[index].ColumnName;
                string par_name = ":" + GetParNameForColumn(col_name);
                if (index == 0)
                {
                    flds = col_name;
                    pars = par_name;
                }
                else
                {
                    flds = flds + "," + col_name;
                    pars = pars + "," + par_name;
                }
            }
            var tblName = queryCall.P_CalledQuery;
            var sql = string.Format("insert into {0} ({1}) values ({2})", tblName, flds, pars);

            var sb = new StringBuilder();


            var className = ParseToUpper(dt.TableName);
            var objName = ParseParam(dt.TableName);
            var cmdName = "cmd" + className;
            sb.AppendLine(string.Format("var {0} = new VOracleCommand();", cmdName));
            sb.AppendLine("try {");
            sb.AppendLine(string.Format("{0}.Connection = {1};", cmdName, ConnectionExpr()));

            sb.AppendLine(string.Format("{0}.CommandText=\"{1}\";", cmdName, sql));
            foreach (var col in realColumns)
            {
                var stype = queryCall.Query().SearchColumn(col.ColumnName).XDataType();
                sb.AppendLine("par = new VOracleParameter();");
                sb.AppendLine(string.Format("par.ParameterName=\"{0}\";", GetParNameForColumn(col.ColumnName)));
                sb.AppendLine(string.Format("par.OracleDbType=VOracleDbType.{0};", Cmn.GetDBType(stype).ToString()));

                sb.AppendLine(string.Format("{0}.Parameters.Add(par);", cmdName));
                sb.AppendLine();
                //   var cmd = new OracleCommand();

                //var par = new OracleParameter();
                //par.Value=
            }

            string parentObjName = null;
            if (fkName != null)
            {
                parentObjName = ParseParam(dt.ParentRelations[0].ParentTable.TableName);
                sb.AppendLine(string.Format("foreach (var {0} in {1}.{2})", objName, parentObjName, className));
                sb.AppendLine("{");
            }


            sb.AppendLine("idCounter++;");
            var idName = objName + "Id";

            sb.AppendLine(string.Format("var {0}Id=idCounter;", objName));
            foreach (var col in realColumns)
            {
                sb.AppendLine(string.Format("par={0}.Parameters[\"{1}\"];", cmdName, GetParNameForColumn(col.ColumnName)));
                var fieldName = ParseToUpper(col.ColumnName);
                string val = null;
                if (col.ColumnName == pkName)
                {
                    val = idName;
                }
                else if (col.ColumnName == fkName)
                {
                    var parentIdName = parentObjName + "Id";
                    val = parentIdName;
                }
                else
                {
                    val = string.Format("{0}.{1}", objName, fieldName);
                }
                sb.AppendLine(string.Format("par.Value={0};", val));
                sb.AppendLine();

            }

            sb.AppendLine(string.Format("SqlTrace.Trace({0});", cmdName));
            sb.AppendLine(string.Format("{0}.ExecuteNonQuery();", cmdName));

            foreach (DataRelation rel in dt.ChildRelations)
            {
                sb.AppendLine();
                var cldDt = (VDataTable)rel.ChildTable;
                var cldQry = rep.GetQuery(cldDt.TableName);
                sb.Append(ClearTempTable(cldQry, cldDt));
                var sb1 = InsertIntoTempTable(cldQry, cldDt, rep);
                sb.Append(sb1);
                sb.AppendLine();
            }

            if (fkName != null)
            {
                sb.AppendLine("}");
            }
            sb.AppendLine("} finally {");
            sb.AppendLine(string.Format("{0}.Dispose();", cmdName));
            sb.AppendLine("}");
            return sb;
        }

        private static string GenerateWebMethodTotal(string name)
        {
            var rep = XmlReports.Environment.GetPrecompiledReport(name);
            var ds = rep.Result(2, false);
            var c = new StringBuilder();

            c.AppendLine(GenWebMethodCode(rep, ds, name, false));

            c.AppendLine();
            c.AppendLine();
            c.AppendLine();

            c.AppendLine(GenMethodCode(rep, name, ds));

            c.AppendLine();
            c.AppendLine();
            c.AppendLine();

            c.AppendLine(GenClass(ds, rep));

            return c.ToString();
        }

        public static string[] PfxToReaplace = new string[] { };
        public static string[] PfxToReaplaceWeb = new string[] { };
        public static string[] PfxToReaplaceCls = new string[] { };
        private static string ParseToUpper(string col)
        {
            var result = new StringBuilder(col);

            foreach (var pfx in PfxToReaplace)
            {
                result.Replace(pfx, "");
            }

            var ss = result.Replace("-", "").ToString().Split('_');
            result.Clear();

            foreach (var s in ss)
            {
                var sChanged = new StringBuilder(s);
                sChanged[0] = char.ToUpper(sChanged[0]);
                result.Append(sChanged.ToString());
            }

            return result.ToString();
        }

        private static string ParseParam(string par)
        {
            var result = new StringBuilder(par);

            foreach (var pfx in PfxToReaplace)
            {
                result.Replace(pfx, "");
            }

            var ss = result.ToString().Split('_');
            result.Clear();

            for (int i = 0; i < ss.Length; i++)
            {
                var sChanged = new StringBuilder(ss[i]);
                sChanged[0] = char.ToUpper(sChanged[0]);
                if (i == 0) sChanged[0] = char.ToLower(sChanged[0]);
                result.Append(sChanged.ToString());
            }

            return result.ToString();
        }

        private static string ReplaceWebPfx(string name)
        {
            foreach (var s in PfxToReaplaceWeb)
            {
                name = name.Replace(s, "");
            }
            return name;
        }
        private static string ReplaceClsPfx(string name)
        {
            foreach (var s in PfxToReaplaceCls)
            {
                name = name.Replace(s, "");
            }
            return name;
        }
        private static string GenWebMethodCode(VReport rep, VDataSet ds, string name, bool retStatus)
        {
            bool isWithError = ds.Tables.Contains("error");


            var result = new StringBuilder();


            if (!isWithError && retStatus)
            {
                result.AppendLine(CodeGenerationUtils.ReportWithErrorResultClass(rep, "Result"));
            }

            bool multSel = (rep.Attributes("multi-select").Any() && rep.Attribute("multi-select").Value == "1") ? true : false;
            var mainTab = GetQueryClassName(rep.GetQuery(rep.MainSource().P_Alias));
            //.P_CalledQuery;
            var mName = name;
            //foreach (var s in PfxToReaplaceWeb)
            //{
            //    mName = mName.Replace(s, "");
            //}
            mName = ReplaceWebPfx(mName);

            mName = ParseToUpper(mName);
            var cName = ParseToUpper(name);
            var paramsLine = new StringBuilder();

            foreach (var xformalPar in rep.FormalParams())
            {
                var p = new Cmn.VStringParamName(xformalPar.Attribute(TextConst.AName.Name).Value);
                paramsLine.Append(ParseParam(p.ToString()) + ", ");
            }
            if (paramsLine.Length > 1) paramsLine.Replace(", ", "", paramsLine.Length - 2, 2);



            var retClassNamePre = GetPrimitiveResultType(rep);
            string primitiveType = null;
            if (retClassNamePre == null)
            {

                retClassNamePre = mainTab;
            }


            string resClassName = ParseToUpper(ReplaceClsPfx(retClassNamePre)) + ((multSel) ? "[] " : " ");

            if (isWithError || retStatus)
            {
                resClassName = ParseToUpper(ReplaceWebPfx(name)) + "Result";
            }

            result.AppendLine(@"        [WebMethod(Description = """ + rep.P_SelfTitle + @""")]");
            result.AppendLine(@"        public " + resClassName + " " + mName + "(" + GetParamsLine(rep) + ")");
            result.AppendLine(@"        {");
            result.AppendLine(@"            try");
            result.AppendLine(@"            {");
            result.AppendLine(@"                using (var data = new BelchenkoData())");
            result.AppendLine(@"                {");
            if (!isWithError && retStatus)
            {
                result.AppendLine(string.Format("                var res= new {0}();", resClassName));
                result.AppendLine("                 res.Result=" + "data." + cName + "(" + paramsLine + ");");
                result.AppendLine("                    return res;");
            }
            else
            {
                result.AppendLine(@"                    return data." + cName + "(" + paramsLine + ");");
            }

            result.AppendLine(@"                }");
            result.AppendLine(@"            }");
            result.AppendLine(@"            catch (Exception ex)");
            result.AppendLine(@"            {");
            result.AppendLine(@"                ProcessException(ex, """ + mName + @""");");
            result.AppendLine(@"                throw;");
            result.AppendLine(@"            }");
            result.AppendLine(@"        }");

            return result.ToString();
        }
        private static string GetPrimitiveResultType(VReport rep)
        {
            if (rep.Queries().Count > 1)
            {
                return null;
            }
            return GetPrimitiveResultType(rep.GetQuery(rep.MainSource().P_Alias));
        }
        private static string GetPrimitiveResultType(VQueryCall qry)
        {
            //var cols =
            //    qry.
            //        Query()
            //        .Columns()
            //        .Where(c => !TextConst.AVColumnArray.SysColumns.Contains(c.XName));
            IList<VSXElement> cols = new List<VSXElement>();
            foreach (VSXElement col in qry.Query().Columns())
            {
                if (TextConst.AVColumn.IsNotSysColumn(col.XName))
                {
                    cols.Add(col);
                }
            }
            if (cols.Count > 1)
            {
                return null;
            }
            else
            {
                return GetNullAbleDataType(Cmn.GetTypeFromStringType(cols.First().XDataType(), null));
            }
        }

        private static string GenMethodCode(VReport rep, string name, VDataSet ds)
        {

            var mName = ParseToUpper(name);
            var mainTab = GetQueryClassName(rep.GetQuery(rep.MainSource().P_Alias));
            bool multSel = (rep.Attributes("multi-select").Any() && rep.Attribute("multi-select").Value == "1") ? true : false;

            var retClassNamePre = GetPrimitiveResultType(rep);
            string primitiveType = null;
            if (retClassNamePre == null)
            {

                retClassNamePre = mainTab;
            }
            else
            {
                primitiveType = retClassNamePre;
            }

            string resClassName = ParseToUpper(ReplaceClsPfx(retClassNamePre)) + ((multSel) ? "[] " : " ");
            bool isWithError = ds.Tables.Contains("error");
            if (isWithError)
            {
                resClassName = ParseToUpper(ReplaceWebPfx(name)) + "Result";
            }

            var result = new StringBuilder();


            //.P_CalledQuery;


            result.AppendLine(@"        public " + resClassName + " " + mName + "(" + GetParamsLine(rep) + ")");
            result.AppendLine(@"        {");

            if (ds.ProcedureText != null)
            {
                result.AppendLine(@"            var cmdText = @""" + ds.ProcedureText + @""";");
                result.AppendLine(@"            var cmd = new sql.builder.Clean.VOracleCommand(cmdText, _Connection);");
                result.AppendLine("try {");
                result.AppendLine();
                result.AppendLine(GetCodeAddOraclePars(rep));
                result.AppendLine(@"            cmd.ExecuteNonQuery();");
                result.AppendLine("} finally {");
                result.AppendLine(@"            cmd.Dispose();");
                result.AppendLine("}");
            }

            result.AppendLine();

            foreach (VDataTable t in ds.Tables)
            {
                result.AppendLine(@"            var cmdText" + ParseToUpper(t.TableName) + @" = @""" + t.DataAdapter.SelectCommand.CommandText + @""";");

                result.AppendLine(@"            var cmd" + ParseToUpper(t.TableName) + " = new sql.builder.Clean.VOracleCommand(cmdText" + ParseToUpper(t.TableName) + ", _Connection);");
                if (ds.ProcedureText == null) result.AppendLine(GetCodeAddOraclePars(rep, ParseToUpper(t.TableName)));
                result.AppendLine(@"            var dataReader" + ParseToUpper(t.TableName) + " = cmd" + ParseToUpper(t.TableName) + ".ExecuteReader();");

                result.AppendLine();
            }
            result.AppendLine("try {");

            result.AppendLine(FillFields(mainTab, rep.MainSource().P_Alias, ds, rep, primitiveType));
            var retExpr = "l" + ParseToUpper(rep.MainSource().P_Alias) + ((multSel) ? ".ToArray()" : ".FirstOrDefault()");

            if (isWithError)
            {
                var val = "\"value\"";
                result.AppendLine("var res=new " + resClassName + "();");
                result.AppendLine(@"
                if (dataReaderError.Read())
                {
                    var val = dataReaderError.GetString(" + val + @");
                    if (!string.IsNullOrEmpty(val))
                    {
                        res.ErrorText = val;
                        res.IsSuccess = false;
                    }
                }");

                //if (dataReaderError.Read())
                result.AppendLine("res." + ParseToUpper(rep.MainSource().P_Alias) + "=" + retExpr + ";");
                retExpr = "res";
            }

            result.AppendLine(@"            return " + retExpr + ";");

            result.AppendLine("} finally {");
            foreach (VDataTable t in ds.Tables)
            {

                result.AppendLine(@"cmd" + ParseToUpper(t.TableName) + ".Dispose();");

                result.AppendLine(@"dataReader" + ParseToUpper(t.TableName) + ".Close();");

                result.AppendLine();
            }
            result.AppendLine("}");
            result.AppendLine(@"        }");



            return result.ToString();
        }

        private static string GetQueryClassName(VQueryCall query)
        {
            return query.Query().GetMainIE().P_IdName;
        }

        private static string GetQueryClassNameOrPrimitiveType(VQueryCall query)
        {
            var v = GetPrimitiveResultType(query);

            if (v != null)
            {
                if (query.P_MultiSelect == TextConst.AVBool.True)
                {
                    v += "[]";
                }

                return v;

            }

            return query.Query().GetMainIE().P_IdName;
        }

        private static string GetNullAbleDataType(Type type)
        {
            var s = "";
            if (type.Name == typeof(decimal).Name || type.Name == typeof(DateTime).Name)
            {
                s = "?";
            }
            return type.Name + s;
        }
        private static string GenClass(VDataTable t, VReport rep)
        {
            var result = new StringBuilder();


            var tableName = ParseToUpper(ReplaceClsPfx(GetQueryClassName(rep.GetQuery(t.TableName))));
            result.AppendLine(@"    public class " + tableName);
            result.AppendLine(@"    {");
            string relColName = null;
            if (t.ParentRelations.Count > 0)
            {
                var qry = rep.GetQuery(t.TableName);
                relColName = qry.GetElementsP(EName.call).First().GetDescedantsP(EName.column).Where(c => c.P_Table == qry.XName).Select(c => c.P_Column).First();

            }
            foreach (DataColumn col in t.Columns)
            {
                string col_name = col.ColumnName;
                string mod;
                if (col_name == relColName || !TextConst.AVColumn.IsNotSysColumn(col_name))
                {
                    mod = "public";
                }
                else
                {
                    mod = "public";
                }
                result.AppendLine(@"        " + mod + " " + GetNullAbleDataType(col.DataType) + " " + ParseToUpper(col.ColumnName) + "=null;");
            }

            foreach (DataRelation rel in t.ChildRelations)
            {
                var relName = ParseToUpper(ReplaceClsPfx(GetQueryClassName(rep.GetQuery(rel.RelationName))));
                result.AppendLine(@"        public " + relName + "[] " + ParseToUpper(rel.RelationName) + "= new " + relName + "[]{};");
            }


            result.AppendLine(@"    }");




            return result.ToString();
        }

        private static string GenClass(VDataSet ds, VReport rep)
        {
            var result = new StringBuilder();


            foreach (VDataTable t in ds.Tables)
            {
                result.Append(GenClass(t, rep));
            }



            return result.ToString();
        }

        private static string FillFields(string nameTab, string alias, VDataSet ds, VReport rep, string primitiveType, string sid = null, string sparid = null)
        {
            var result = new StringBuilder();
            var clsName = "";
            if (primitiveType == null)
            {
                clsName = ParseToUpper(ReplaceClsPfx(nameTab));
            }
            else
            {
                clsName = primitiveType;
            }

            result.AppendLine(@"            var l" + ParseToUpper(alias) + " = new List<" + clsName + ">();");
            result.AppendLine(@"            while(dataReader" + ParseToUpper(alias) + ".Read()" + sid + sparid + ")");
            result.AppendLine(@"            {");
            if (primitiveType == null)
            {
                result.AppendLine(@"            var res" + ParseToUpper(alias) + " = new " + clsName);
            }
            else
            {
                result.AppendLine(@"            " + clsName + " res" + ParseToUpper(alias) + "=");
            }

            if (primitiveType == null)
            {
                result.Append(@"            {");
            }
            var q = "";
            foreach (DataColumn col in ds.GetTable(alias).Columns)
            {
                var stype = GetNullAbleDataType(col.DataType);
                if (primitiveType == null)
                {
                    //result.Append("\r\n\t\t\t\t" + ParseToUpper(col.ColumnName) + " = dataReader" + ParseToUpper(alias) +
                    //              ".Get" + col.DataType.Name + @"(""" + col.ColumnName + @"""),");
                    var stypeE = "";
                    if (stype == "Decimal?")
                    {
                        result.Append(
                          string.Format(
                              "{4}{0}=dataReader{1}[\"{2}\"] == DBNull.Value ? null : ({3})Convert.ToDecimal(dataReader{1}[\"{2}\"])",
                              ParseToUpper(col.ColumnName), ParseToUpper(alias), col.ColumnName, stype, q, stypeE));
                    }
                    else
                    {
                        result.Append(
                            string.Format(
                                "{4}{0}=dataReader{1}[\"{2}\"] == DBNull.Value ? null : ({3})dataReader{1}[\"{2}\"]",
                                ParseToUpper(col.ColumnName), ParseToUpper(alias), col.ColumnName, stype, q, stypeE));
                    }

                    q = ",";
                }
                else
                {
                    if (TextConst.AVColumn.IsNotSysColumn(col.ColumnName))
                    {
                        //result.Append("  dataReader" + ParseToUpper(alias) + ".Get" + col.DataType.Name + @"(""" + col.ColumnName + @""");");
                        result.Append(
                            string.Format(
                                "{0}dataReader{1}[\"{2}\"] == DBNull.Value ? null : ({3})dataReader{1}[\"{2}\"];",
                                "", ParseToUpper(alias), col.ColumnName, stype));

                    }
                }

            }

            // if (result.Length > 1) result.Replace(",", "", result.Length - 1, 1);
            result.Append("\r\n");

            if (primitiveType == null)
            {
                result.AppendLine(@"            };");
            }





            foreach (DataRelation rel in ds.GetTable(alias).ChildRelations)
            {
                var relName = GetQueryClassName(rep.GetQuery(rel.RelationName));
                // .P_CalledQuery;
                var parent = " && res" + ParseToUpper(alias) + "." + ParseToUpper(rel.ParentColumns[0].ColumnName) + " == ";
                var chilCol = ds.GetTable(rel.RelationName).GetColumn(rel.ChildColumns[0].ColumnName);
                var child = "dataReader" + ParseToUpper(rel.RelationName) + ".Get" + chilCol.DataType.Name + @"(""" + chilCol.ColumnName + @""")";
                result.AppendLine(FillFields(relName, rel.RelationName, ds, rep, null, parent, child));
                result.AppendLine(@"            res" + ParseToUpper(alias) + "." + ParseToUpper(rel.RelationName) + " = l" + ParseToUpper(rel.RelationName) + ".ToArray();");
            }


            result.AppendLine(@"            l" + ParseToUpper(alias) + ".Add(res" + ParseToUpper(alias) + ");");
            result.AppendLine(@"            }");


            return result.ToString();
        }

        private static string GetParamsLine(VReport rep)
        {
            var result = new StringBuilder();

            foreach (VSXElement xformalPar in rep.FormalParams())
            {
                var pname = xformalPar.Attribute(TextConst.AName.Name).Value;
                var type = Cmn.GetTypeFromStringType(xformalPar.Attribute(TextConst.AName.Type).Value, null);
                result.Append(type + " " + ParseParam(pname) + ", ");
            }
            if (result.Length > 1) result.Replace(", ", "", result.Length - 2, 2);

            return result.ToString();
        }


        private static string GetCodeAddOraclePars(VReport rep, string pfx = "")
        {
            var result = new StringBuilder();

            foreach (var xformalPar in rep.FormalParams())
            {
                var p = new Cmn.VStringParamName(xformalPar.Attribute(TextConst.AName.Name).Value);
                result.AppendLine(@"            cmd" + pfx + @".Parameters.Add(new VOracleParameter(""" + p.ToString() + @""", " + ParseParam(p.ToString()) + "));");
            }

            return result.ToString();
        }


    }
}
