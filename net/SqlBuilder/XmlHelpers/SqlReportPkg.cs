using System.Collections.Generic;
using System.Linq;
using System.Text;
// Cross-platform: System.Windows.Input (WPF) is Windows-only, commented out
//using System.Windows.Input;
using System.Xml.Linq;
using sql.builder.DataApi;


namespace sql.builder.XmlHelpers
{
    public static class SqlReportPkg
    {
        private static string keyPfx = "p_key_";

        public class ReportInfo
        {
            public VReport Report;
            public VDataSet DataSet;
            public List<string> ParsDefinition;
            public List<object> Pars;
            public bool TempUsing = true;
        }

        public static ReportInfo PrepareReportInfo(string name, bool isAnonimusBlock)
        {
            var rep = XmlReports.Environment.GetPrecompiledReport(name);
            return PrepareReportInfo(rep, isAnonimusBlock);
        }



        public static ReportInfo PrepareReportInfo(XElement report, bool isAnonimusBlock)
        {
            report = VReport.getReportOrQuery(report);
            var rep = VSXElement.Get<VReport>(report);
            var pars = new List<object>();
            var parsDefinition = new List<string>();
            if (!isAnonimusBlock)
            {
                rep.IsSimpleParams = false; // чтобы подставолись имена параметров без :
                foreach (var xformalPar in rep.FormalParams())
                {
                    var pname = xformalPar.Attribute(TextConst.AName.Name).Value;
                    pars.Add(new Cmn.VStringParamName(xformalPar.Attribute(TextConst.AName.Name).Value));
                    var pdef = pname + " " + Cmn.OracleTypeDefinitionFromType(Cmn.GetTypeFromStringType(xformalPar.Attribute(TextConst.AName.Type).Value, null), -1);
                    parsDefinition.Add(pdef);
                }
            }
            XElement xpars = null;
            if (pars.Any())
            {
                xpars = VDataSet.ParsObjectArrayToXelement(pars.ToArray(), rep.GetElementsP(EName.@params).First());
            }
            var ds = rep.Result(xpars, 2, null);
            var res = new ReportInfo();
            if (ds.Tables.Count == 1 && rep.P_UseTemp != TextConst.AVBool.True)
            {
                res.TempUsing = false;
            }


            res.Report = rep;
            res.DataSet = ds;
            res.ParsDefinition = parsDefinition;
            res.Pars = pars;
            return res;

        }


        public static string Generate(string name, bool isAnonimusBlock, bool hasReturn, bool allowMerge,
            bool isDelete = false)
        {

            return DoGenerate(name, null, isAnonimusBlock, isAnonimusBlock, hasReturn, allowMerge, isDelete, false, null);
        }


        public static string GenerateInsertStatementForProc(XElement query, string keyVarName)
        {

            var s = DoGenerate(null, query, true, false, false, false, false, false, keyVarName);
            return s;
        }

        public static string GenerateUpdateStatementForProc(XElement query)
        {

            var s = DoGenerate(null, query, true, false, false, false, false, true, null);
            return s;
        }

        private static string DoGenerate(string name, XElement qry, bool isAnonimusBlock, bool parStyleAnonimus, bool hasReturn, bool allowMerge, bool isDelete, bool isUpdate, string keyVarName)
        {
            ReportInfo ri = null;
            if (qry == null)
            {
                ri = PrepareReportInfo(name, parStyleAnonimus);
            }
            else
            {
                ri = PrepareReportInfo(qry, parStyleAnonimus);
            }


            var rep = ri.Report;
            var ds = ri.DataSet;
            var parsDefinition = ri.ParsDefinition;
            var pars = ri.Pars;
            var sb = new StringBuilder();
            var prfx = "sqlb_";
            var pfname = prfx + name;

            if (!isAnonimusBlock)
            {
                sb.AppendLine("--- Скрипт сгенерирован автоматически с помощью SqlBuilder");
                sb.AppendLine("drop package " + pfname);
                sb.AppendLine("/");
                //sb.AppendLine("drop package body " + pfname);
                //sb.AppendLine("/");
                sb.AppendLine("drop public synonym " + pfname);
                sb.AppendLine("/");

                if (!rep.Descendants().Attributes(TextConst.AName.UpdateTarget).Any())
                {
                    foreach (VDataTable tbl in ds.Tables)
                    {
                        var tname = pfname;
                        if (ds.Tables.Count > 1)
                        {
                            tname += "_" + tbl.TableName;
                        }
                        tname += "_tbl";
                        sb.AppendLine(string.Format("TRUNCATE TABLE {0};", tname));
                        sb.AppendLine("drop table " + tname);
                        sb.AppendLine("/");
                        sb.AppendLine("drop public synonym " + tname);
                        sb.AppendLine("/");
                    }
                    foreach (VDataTable tbl in ds.Tables)
                    {
                        var record_fields = new List<string>();
                        foreach (VDataColumn col in tbl.Columns)
                        {
                            if (TextConst.AVColumn.IsNotRepDsSysColumn(col.ColumnName))
                            {
                                string colDef = col.ColumnName + " " + Cmn.OracleTypeDefinitionFromType(col.DataType);
                                record_fields.Add(colDef);
                            }
                        }

                        var tname = name;
                        if (ds.Tables.Count > 1)
                        {
                            tname += "_" + tbl.TableName;
                        }
                        GenerateTempTable(tname, record_fields.ToArray(), sb);


                    }
                }


                GeneratePackageSpecification(name, parsDefinition.ToArray(), sb);

                GeneratePackageBodyBegin(name, sb);


                if (ds.ProcedureText != null)
                {
                    GenerateProcBegin("fill_temp", sb, isAnonimusBlock, parsDefinition.ToArray());
                    sb.AppendLine(string.Format("\t\t{0}", ds.ProcedureText));
                    GenerateProcEnd("fill_temp", sb);
                }
            }
            else
            {
                sb.AppendLine("begin");
                if (ds.ProcedureText != null)
                {
                    sb.AppendLine("begin");
                    sb.AppendLine(string.Format("\t\t{0}", ds.ProcedureText));
                    sb.AppendLine("end;");
                }

            }
            List<string> vars = new List<string>();
            SortedList<string, string> keysVarNames = new SortedList<string, string>();
            if (!isDelete)
            {
                foreach (VDataTable tbl in ds.Tables)
                {

                    var varName = keyPfx + tbl.TableName;
                    if (keyVarName != null)
                    {
                        varName = keyVarName;
                    }
                    keysVarNames.Add(tbl.TableName, varName);
                    if (keyVarName == null)
                    {
                        vars.Add(varName + " number");
                    }
                }
            }

            GenerateProcBegin("fill_table", sb, isAnonimusBlock, parsDefinition.ToArray(), vars.ToArray());

            if (!isAnonimusBlock)
            {

                if (ds.ProcedureText != null)
                {
                    GenerateProcCall("fill_temp", sb, pars.ToArray());
                }
            }
            else
            {

                sb.AppendLine("begin");
            }



            if (isDelete)
            {
                var tbl = ds.Tables[0];
                var vtbl = rep.AllSources().Where(e => e.XName == tbl.TableName).First();
                var uqry = XmlReports.Environment.GetQuery(vtbl.P_UpdateTarget);
                var keyName = uqry.KeyColumn().P_Column;
                var tempKeyCol = (tbl.Columns[keyName] as VDataColumn).TempColumnName;
                var tname = uqry.MainSource().P_CalledQuery;
                sb.AppendLine(string.Format("delete {0} where {1} in (select {2} from {3});", tname, keyName, tempKeyCol,
                    TextConst.DBObjects.TempTable));
            }
            else
            {
                SortedList<string, string> storedRowsSelects = new SortedList<string, string>();

                SortedList<string, string> updTrgts = new SortedList<string, string>();

                var allUsedTempColsNames =
                    ds.GetAllTables()
                        .SelectMany(t => t.Columns.Cast<VDataColumn>())
                        .Select(c => c.TempColumnName)
                        .Distinct()
                        .ToList();
                allUsedTempColsNames.Add(TextConst.DBObjects.TempTableTableIdColumn);
                var sb1 = new StringBuilder();

                if (ri.TempUsing)
                {
                    sb1.AppendLine("select");
                    sb1.AppendLine(string.Join(",", allUsedTempColsNames));
                    sb1.AppendLine("from " + TextConst.DBObjects.TempTable);
                    sb1.AppendLine("where  " + TextConst.DBObjects.TempTableTableIdColumn + "  in (");
                    var q = "";
                    foreach (VDataTable tbl in ds.Tables)
                    {
                        sb1.AppendLine(q + "'" + tbl.QueryName + "'");
                        q = ",";
                    }
                    sb1.AppendLine(")");
                    if (ds.Relations.Count > 0)
                    {
                        sb1.AppendLine("connect by prior " + TextConst.AVColumn.Sid + "=" + TextConst.AVColumn.SparentId);
                        // убрать если нет вложенности
                        sb1.AppendLine("start with  " + TextConst.AVColumn.SparentId + " is null");
                    }

                }
                else
                {
                    sb1.AppendLine((ds.Tables[0] as VDataTable).DataAdapter.SelectCommand.CommandText);
                }


                var cmnSelText = sb1.ToString();

                var recAlias = TextConst.DBObjects.TempTable + "_rec";
                foreach (VDataTable tbl in ds.Tables)
                {
                    var vtbl = rep.AllSources().Where(e => e.XName == tbl.TableName).First();
                    if (vtbl.P_UpdateTarget == "")
                    {
                        var talias = "_" + tbl.TableName;
                        if (ds.Tables.Count == 1)
                        {
                            talias = "";
                        }
                        var tname = "sqlb_" + name + talias + "_tbl";
                        sb.AppendLine("delete " + tname + ";");
                    }
                }
                var sb2 = new StringBuilder();
                sb2.AppendLine("for " + recAlias + " in (" + cmnSelText + ") loop");
                bool isFirst = true;

                // delete from vc_user_login a where user_id in (select user_id from rr_temp) and not exists (select * from rr_temp t where a.user_id=t.user_id and a.kod_ul_zayav=t.kod_ul_zayav )


                foreach (VDataTable tbl in ds.Tables)
                {



                    string keyName = null;
                    VQuery uqry = null;
                    var vtbl = rep.AllSources().Where(e => e.XName == tbl.TableName).First();
                    string tname = "";
                    bool isChild = false;
                    string refColName = null;
                    string prtKeyVarName = null;
                    if (vtbl.P_UpdateTarget != "")
                    {
                        uqry = XmlReports.Environment.GetQuery(vtbl.P_UpdateTarget);
                        keyName = uqry.KeyColumn().P_Column;
                        tname = uqry.MainSource().P_CalledQuery;
                        updTrgts.Add(tbl.TableName, tname);

                        isChild = (tbl.ParentRelations != null && tbl.ParentRelations.Count != 0);


                        if (isChild)
                        {


                            var parTname = tbl.ParentRelations[0].ParentTable.TableName;
                            var qc = uqry.AllSources().First(e => e.P_CalledQuery == updTrgts[parTname]);
                            refColName =
                                qc.GetDescedantsP(EName.column).First(c => c.P_Table == qc.XName).P_Column;
                            prtKeyVarName = keysVarNames[parTname];

                        }

                    }
                    else
                    {
                        var talias = "_" + tbl.TableName;
                        if (ds.Tables.Count == 1)
                        {
                            talias = "";
                        }
                        tname = "sqlb_" + name + talias + "_tbl";
                    }


                    var columns = new List<string>();
                    var tempColumns = new List<string>();
                    string keyInputValue = null;
                    string keyInputValueO = null;
                    string refInputValue = null;
                    foreach (VDataColumn col in tbl.Columns)
                    {
                        if (TextConst.AVColumn.IsNotRepDsSysColumn(col.ColumnName))
                        {
                            bool use = true;
                            if (vtbl.P_UpdateTarget != "")
                            {
                                if (keyName == col.ColumnName && uqry.SearchColumn(col.ColumnName) != null && (allowMerge || isUpdate))
                                {
                                    keyInputValue = recAlias + "." + col.TempColumnName;
                                    keyInputValueO = col.TempColumnName;
                                    use = false;
                                }
                                if (refColName == col.ColumnName && uqry.SearchColumn(col.ColumnName) != null)
                                {
                                    refInputValue = col.TempColumnName;
                                    //use = false;
                                }
                                if (uqry.SearchColumn(col.ColumnName) == null || refColName == col.ColumnName)
                                {
                                    use = false;
                                }
                                else
                                {

                                }
                            }
                            if (use)
                            {
                                columns.Add(col.ColumnName);
                                var colName = col.ColumnName;
                                if (ri.TempUsing)
                                {
                                    colName = col.TempColumnName;
                                }
                                tempColumns.Add(recAlias + "." + colName);
                            }
                        }
                    }

                    var scols = string.Join(",", columns);
                    var stempcols = string.Join(",", tempColumns);
                    if (isFirst)
                    {
                        if (ri.TempUsing)
                        {
                            sb2.Append("if ");
                        }

                    }
                    else
                    {
                        if (refInputValue != null && keyInputValueO != null && allowMerge)
                        {
                            var ss = string.Format(
                                @"delete from {0} a where 
                                {1} in (select {2} from {3}) 
                                and not exists (select * from {3} t where a.{1}=t.{2} and a.{4}=t.{5} );", tname,
                                refColName, refInputValue, TextConst.DBObjects.TempTable, keyName, keyInputValueO);
                            sb.AppendLine(ss);
                        }
                        if (ri.TempUsing)
                        {
                            sb2.Append("elsif ");
                        }
                    }


                    var scols1 = scols;
                    var stempcols1 = stempcols;
                    if (!string.IsNullOrEmpty(refColName))
                    {
                        scols1 += "," + refColName;
                        stempcols1 += "," + prtKeyVarName;
                    }

                    if (ri.TempUsing)
                    {
                        sb2.AppendLine(recAlias + "." + TextConst.DBObjects.TempTableTableIdColumn + " ='" + tbl.QueryName +
                                   "' then");
                    }

                    if (scols1 == "")
                    {
                        sb2.AppendLine(string.Format("{0}:={1};", keysVarNames[tbl.TableName], keyInputValue));
                    }
                    else
                    {
                        sb2.AppendLine(string.Format("{0}:=null;", keysVarNames[tbl.TableName]));

                        if (keyInputValue != null && allowMerge)
                        {

                            sb2.AppendLine(
                                string.Format(
                                    " UPDATE {0} SET ({1}) =(select {2} from dual ) where {3}=nullif({4},0) RETURNING {3} INTO {5};",
                                    tname, scols1, stempcols1, keyName, keyInputValue, keysVarNames[tbl.TableName]));
                            sb2.AppendLine(string.Format("if {0} is null then", keysVarNames[tbl.TableName]));

                        }


                        if (!isUpdate)
                        {
                            sb2.AppendLine("insert into " + tname + " (" + scols1);
                            sb2.Append(") values (" + stempcols1);
                            sb2.Append(") ");
                            if (!string.IsNullOrEmpty(keyName))
                            {
                                sb2.Append(" returning " + keyName + "  into " + keysVarNames[tbl.TableName]);
                            }
                            sb2.AppendLine(";");
                        }
                        else
                        {
                            sb2.AppendLine(
                                string.Format(
                                    "UPDATE {0} SET ({1}) =(select {2} from dual ) where {3}={4};",
                                    tname, scols1, stempcols1, keyName, keyInputValue));

                        }



                        if (keyInputValue != null && allowMerge)
                        {
                            sb2.AppendLine("end if;");
                        }
                    }

                    if (isFirst && hasReturn)
                    {

                        if (parStyleAnonimus)
                        {
                            sb2.Append(":" + TextConst.DBParams.PrimaryKeyParam + ":=" + keysVarNames[tbl.TableName] + ";");
                        }


                    }
                    isFirst = false;
                }

                if (ri.TempUsing)
                {
                    sb2.AppendLine("end if;");
                }
                sb2.AppendLine("end loop;");
                if (!parStyleAnonimus && hasReturn)
                {

                    sb2.AppendLine("commit;");
                    sb2.Append("return " + keysVarNames.Values.First() + ";");



                }
                sb.Append(sb2);
            }

            if (!isAnonimusBlock)
            {
                GenerateProcEnd("fill_table", sb);
                GeneratePackageBodyEnd(name, sb);



                sb.AppendLine("--- Конец автоматически сгенерированного скрипта ---");
            }
            else
            {
                sb.AppendLine("end;");
                if (!parStyleAnonimus && hasReturn)
                {
                }
                else
                {
                    sb.AppendLine("commit;");
                }

                sb.AppendLine("end;");
            }

            return sb.ToString();

        }


        private static void GeneratePackageSpecification(string repname, string[] pars, StringBuilder sb)
        {
            sb.AppendLine(string.Format("CREATE OR REPLACE PACKAGE sqlb_{0}", repname));
            sb.AppendLine("IS");
            sb.AppendLine("---Пакет сгенерирован автоматически с помощью SqlBuilder");
            sb.AppendLine();
            if (pars.Length != 0)
            {
                sb.AppendLine("\tPROCEDURE fill_table");
                sb.AppendLine("\t(");
                sb.AppendLine(string.Join(",\r\n", pars));
                sb.AppendLine("\t);");
            }
            else
            {
                sb.AppendLine("\tPROCEDURE fill_table;");
            }

            sb.AppendLine(string.Format("END sqlb_{0};", repname));
            sb.AppendLine("/");
            sb.AppendLine(string.Format("GRANT EXECUTE ON sqlb_{0} TO public;", repname));
            sb.AppendLine("/");
            sb.AppendLine(string.Format("CREATE OR REPLACE PUBLIC SYNONYM sqlb_{0} FOR sqlb_{0};", repname));
            sb.AppendLine("/");
            sb.AppendLine();
        }
        private static void GenerateProcCall(string procName, StringBuilder sb, object[] pars)
        {

            sb.AppendLine(procName);
            if (pars.Length != 0)
            {
                sb.AppendLine("\t(");
                sb.AppendLine(string.Join(",\r\n", pars));
                sb.AppendLine("\t)");
            }
            sb.AppendLine("\t;");

        }
        private static void GenerateProcBegin(string procName, StringBuilder sb, bool isAnonimusBlock, string[] pars, string[] vars = null)
        {
            if (!isAnonimusBlock)
            {
                if (pars.Length != 0)
                {

                    sb.AppendLine("\tPROCEDURE " + procName);
                    sb.AppendLine("\t(");
                    sb.AppendLine(string.Join(",\r\n", pars));
                    sb.AppendLine("\t)");
                }
                else
                {
                    sb.AppendLine("\tPROCEDURE " + procName);
                }
                sb.AppendLine("\tIS");
            }
            else
            {
                if (vars.Length != 0)
                {
                    sb.AppendLine("\tdeclare");
                }
            }

            if (vars != null)
            {
                foreach (var s in vars)
                {
                    sb.AppendLine("\r\n " + s + ";");
                }
            }
            //sb.AppendLine("\t\tserr varchar2(200);");
            //sb.AppendLine("\t\tnerr number;");
            if (!isAnonimusBlock)
            {
                sb.AppendLine("\tBEGIN");
            }
        }

        private static void GeneratePackageBodyBegin(string repname, StringBuilder sb)
        {
            sb.AppendLine(string.Format("CREATE OR REPLACE PACKAGE BODY sqlb_{0}", repname));
            sb.AppendLine("IS");
            sb.AppendLine("---Пакет сгенерирован автоматически с помощью SqlBuilder");
            sb.AppendLine();


        }

        public static void GenerateTempTable(string qname_safe, string[] record_fields, StringBuilder sb)
        {



            sb.AppendLine(string.Format("CREATE GLOBAL TEMPORARY TABLE sqlb_{0}_tbl", qname_safe));
            sb.AppendLine("(");
            sb.AppendLine(string.Join(",\r\n", record_fields));
            sb.AppendLine(") ON COMMIT PRESERVE ROWS;");
            //sb.AppendLine(") ON COMMIT DELETE ROWS;");
            sb.AppendLine("/");
            sb.AppendLine(string.Format("GRANT SELECT ON sqlb_{0}_tbl TO public;", qname_safe));
            sb.AppendLine("/");
            sb.AppendLine(string.Format("CREATE OR REPLACE PUBLIC SYNONYM sqlb_{0}_tbl FOR sqlb_{0}_tbl;", qname_safe));
            sb.AppendLine("/");
            sb.AppendLine();
        }
        private static void GenerateProcEnd(string procName, StringBuilder sb)
        {

            sb.AppendLine("\tEND " + procName + ";");
        }

        private static void GeneratePackageBodyEnd(string repname, StringBuilder sb)
        {


            sb.AppendLine(string.Format("END sqlb_{0};", repname));
            sb.AppendLine("/");
            sb.AppendLine();
        }


    }
}
