//using System;
//using System.Data;
//using System.Diagnostics;
//using System.Linq;
//using System.Text;
//using System.Xml.Linq;
//using Devart.Data.Oracle;
////using DevExpress.XtraBars.Docking2010.Base;
//using sql.builder.DataApi;
//using System.Collections.Generic;


//namespace sql.builder.XmlHelpers
//{
//    internal static class PackageGenerationASUTP
//    {

//		private const string tempTablePostfix = "_TT";

//        public static void Generate(bool testScheme=false)
//        {
//            XmlReports.Environment.LoadProject("kido_asutp");

//            var queries =
//                XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => q.P_IdName.StartsWith("svc_"));
//            var sb = new StringBuilder();
           
//            //    sb.Append(GeneratePackage("svc_podr",testScheme));
             
//                sb.AppendLine("/");

//			//	sb.Append(GeneratePackage("svc_zayav_ka", testScheme));
//				foreach (VQuery qry in queries)
//				{
//                    bool readOnly = XmlReports.Environment.GetQuery(qry.P_IdName).AttrOrEmpty(AName.comment).Contains("read_only");

//					var sbTempTable = GenerateTempTable(qry.P_IdName, testScheme);
//					if (!readOnly) sb.Append(sbTempTable);
//					if (sbTempTable.ToString() != "")
//					{
//						sb.Append(GeneratePackage(qry.P_IdName, testScheme, true));
//					}
//					else
//					{
//						sb.Append(GeneratePackage(qry.P_IdName, testScheme, false));
//					}

//					sb.AppendLine("/");
//				}


//            var filename = Cmn.writeScriptFile("svc_pkg", sb.ToString());


//            Process.Start(filename);
//        }

//        private static StringBuilder GeneratePackage(string queryName, bool testScheme, bool manualInsUpd)
//        {

//            var sbBody = new StringBuilder();
//            var sbHead = new StringBuilder();
//            var pfx = "SVC";
//            if (testScheme)
//            {
//                pfx = "SVCT";
//            }
//            var className = classNameFromQueryName(queryName);
//            sbHead.AppendLine(string.Format("CREATE OR REPLACE PACKAGE {1}_{0} IS", className, pfx));
//            sbBody.AppendLine(string.Format("CREATE OR REPLACE PACKAGE BODY {1}_{0} IS", className, pfx));

//            // CREATE OR REPLACE PACKAGE BODY svc_form_sob IS
//			var funcs = GenegateMethods(queryName, testScheme, manualInsUpd);
//            foreach (var sbf in funcs)
//            {
//                sbHead.AppendLine(ExtractHead(sbf.ToString()));
//                sbBody.Append(sbf);

//            }
//            sbHead.AppendLine("end;");
//            sbBody.AppendLine("end;");

//            var sb = new StringBuilder();
//            sb.Append(sbHead);
//            sb.AppendLine("/");
//            sb.AppendLine(string.Format("GRANT EXECUTE ON {1}_{0} TO ASUTP", className, pfx));
//            sb.AppendLine("/");
//            sb.Append(sbBody);
//            sb.AppendLine("/");

//            return sb;
//            //var s = Cmn.FormatSql(sb.ToString());


//        }


//        private static string ExtractHead(string methodBody)
//        {
//            var i = methodBody.ToLower().IndexOf(" is ");
//			string head;
//			if (i >= 0)
//			{
//				head = methodBody.Substring(0, i) + ";";
//			}
//			else
//			{
//				head = "";
//			}
//            return head;
//        }


//		private static IEnumerable<StringBuilder> GenegateMethods(string queryName, bool testScheme, bool manualInsUpd)
//        {

//			bool readOnly = XmlReports.Environment.GetQuery(queryName).AttrOrEmpty(AName.comment).Contains("read_only");
			
//            var list = new List<StringBuilder>();
//            list.Add(Generate_GetAll(queryName));
//			list.Add(Generate_GetCount(queryName));
//            list.Add(Generate_GetById(queryName));
//			list.Add(Generate_GetChageInfo(queryName, testScheme, manualInsUpd));
//			list.Add(Generate_GetModified(queryName, testScheme, manualInsUpd));
//			if (!readOnly)
//			{
//				list.Add(Generate_Insert(queryName, testScheme, manualInsUpd));
//				list.Add(Generate_Update(queryName, testScheme, manualInsUpd));
//				list.Add(Generate_Put(queryName, manualInsUpd));
//				list.Add(Generate_Del(queryName, testScheme, manualInsUpd));
//			}
//            var byParent = Generate_GetByParentList(queryName);
//            list.AddRange(byParent);
//            return list;
//        }

//        private static StringBuilder Generate_GetById(string queryName)
//        {
//            var className = classNameFromQueryName(queryName);

//            var funcName = "get_by_id";

//            var qry = (XElement)XmlReports.Environment.GetPrecompiledQuery(queryName);

//            var keyName = className + "_id";
//            var keyParName = "p_" + keyName;
//            qry = ExtendQuery_GetByField(qry, keyName, keyParName);

//            return Generate_GetMethod(funcName, qry);
//        }
//        private static IEnumerable<StringBuilder> Generate_GetByParentList(string queryName)
//        {
//            var list = new List<StringBuilder>();
//            var qryN = XmlReports.Environment.GetQuery(queryName);
//            var qry = (XElement)XmlReports.Environment.GetPrecompiledQuery(queryName);
         
//            foreach (VSXElement col in qryN.Columns().Where(c=>c.P_CalledQuery!="").ToArray())
//            {
        

//                var funcName = col.XName;
//                if (funcName.EndsWith("_id"))
//                {
//                    funcName = funcName.Substring(0, funcName.Length - 3);
//                }
//                funcName = "get_by_" + funcName;
//                var qry1 = ExtendQuery_GetByField(qry, col.XName, "p_" + col.XName);
//                var sb = Generate_GetMethod(funcName, qry1);
//                list.Add(sb);
//            }
//            return list;
//        }

//        private static StringBuilder Generate_GetMethod(string funcName, XElement query)
//        {
//            var sb = new StringBuilder();
//            var ri = SqlReportPkg.PrepareReportInfo(query, false);
//            var parsStr = string.Join(",", ri.ParsDefinition);
//            if (!string.IsNullOrEmpty(parsStr))
//            {
//                parsStr = "(" + parsStr + ")";
//            }
//            var sql = ri.DataSet.GetAllTables().First().DataAdapter.SelectCommand.CommandText;

//            sb.AppendLine(string.Format("function {0} {1} RETURN SYS_REFCURSOR is ", funcName, parsStr));
//            sb.AppendLine("v_cur SYS_REFCURSOR;");
//            sb.AppendLine("begin");
//            sb.AppendLine("OPEN v_cur FOR");
//            sb.AppendLine(sql + ";");
//            sb.AppendLine("RETURN v_cur;");
//            sb.AppendLine("end;");
//            return sb;
//        }


//		private static StringBuilder Generate_InsertMethod(string funcName, XElement query, bool testScheme, bool manualInsUpd)
//        {
//            var sb = new StringBuilder();
//            var ri = SqlReportPkg.PrepareReportInfo(new XElement(query), false);
//            var parsStr = string.Join(",", ri.ParsDefinition);
//            if (!string.IsNullOrEmpty(parsStr))
//            {
//                parsStr = "(" + parsStr + ")";
//            }
//            sb.AppendLine(string.Format("function {0} {1} RETURN sys_refcursor is ", funcName, parsStr));
//            sb.AppendLine("v_id   number;");
//            sb.AppendLine("v_cur   SYS_REFCURSOR;");
//            sb.AppendLine("begin");
//			sb.AppendLine("	DBMS_SESSION.SET_CONTEXT('CLIENTCONTEXT', 'vr_hist_all_context', 'ASUTP_packages' );");

//			if (manualInsUpd)
//			{
//				sb.Append(generateInsManual(query, testScheme).ToString()
//					+ System.Environment.NewLine
//					+ "v_id := "
//					+ (testScheme ? "SVCT" : "SVC")
//					+ "_MANUAL.INS('"
//					+ query.Attribute(TextConst.AName.UpdateTarget).Value
//					+ "');"
//					+ System.Environment.NewLine
//					+ "commit;"
//					+ System.Environment.NewLine);
//			}
//			else
//			{
//				var sql = SqlReportPkg.GenerateInsertStatementForProc(query, "v_id");
//				sb.AppendLine(sql);
//			}
//			sb.AppendLine("	DBMS_SESSION.SET_CONTEXT('CLIENTCONTEXT', 'vr_hist_all_context', '' );");
//            sb.AppendLine("OPEN v_cur FOR select v_id from dual;");
//            sb.AppendLine("RETURN v_cur;");
//            sb.AppendLine("end;");

//            return sb;
//        }

//		private static StringBuilder Generate_UpdateMethod(string funcName, XElement query, bool testScheme, bool manualInsUpd, StringBuilder checks)
//        {
//            var sb = new StringBuilder();
//			var ri = SqlReportPkg.PrepareReportInfo(new XElement(query), false);
//            var parsStr = string.Join(",", ri.ParsDefinition);
//            if (!string.IsNullOrEmpty(parsStr))
//            {
//                parsStr = "(" + parsStr + ")";
//            }
//            sb.AppendLine(string.Format("procedure {0} {1}  is ", funcName, parsStr));


//			sb.AppendLine("begin");
//			sb.AppendLine("	DBMS_SESSION.SET_CONTEXT('CLIENTCONTEXT', 'vr_hist_all_context', 'ASUTP_packages' );");
//			sb.Append(checks);
//			if (manualInsUpd)
//			{
//				sb.Append("begin"
//					+ System.Environment.NewLine
//					+ generateInsManual(query, testScheme).ToString()
//					+ System.Environment.NewLine
//					+ (testScheme ? "SVCT" : "SVC") 
//					+ "_MANUAL.UPD('" 
//					+ query.Attribute(TextConst.AName.UpdateTarget).Value 
//					+ "');"
//					+ System.Environment.NewLine
//					+ "commit;"
//					+ System.Environment.NewLine
//					+ "end;"
//					+ System.Environment.NewLine);
//			}
//			else
//			{
//				StringBuilder sql = new StringBuilder(SqlReportPkg.GenerateUpdateStatementForProc(query));
//				sb.AppendLine(sql.ToString());
//			}
//			sb.AppendLine("	DBMS_SESSION.SET_CONTEXT('CLIENTCONTEXT', 'vr_hist_all_context', '' );");
//			sb.AppendLine("end;");
//            return sb;
//        }

//        private static StringBuilder Generate_PutMethod(string funcName, XElement query, string tableName,
//			string keyColumnName, string keyParName)
//        {
//            var sb = new StringBuilder();
//            var ri = SqlReportPkg.PrepareReportInfo(new XElement(query), false);
//            var parsStr = string.Join(",", ri.ParsDefinition);
//            if (!string.IsNullOrEmpty(parsStr))
//            {
//                parsStr = "(" + parsStr + ")";
//            }

//            var parsUseStr = string.Join(",", ri.Pars);
//            var parsUseNoKeyStr = string.Join(",", ri.Pars.Where(p => p.ToString() != keyParName));
//            sb.AppendLine(string.Format("function {0} {1} RETURN sys_refcursor is ", funcName, parsStr));
//            sb.AppendLine("v_cur   SYS_REFCURSOR;");

//			//if (manualInsUpd)
//			//{
//			//	sb.AppendLine("v_id   NUMBER;");
//			//	sb.AppendLine("begin");
//			//	sb.Append(generateInsManual(query, testScheme).ToString()
//			//		+ System.Environment.NewLine
//			//		+ "v_id := "
//			//		+ (testScheme ? "SVCT" : "SVC") 
//			//		+ "_MANUAL.PUT('" 
//			//		+ query.Attribute(TextConst.AName.UpdateTarget).Value 
//			//		+ "');"
//			//		+ System.Environment.NewLine
//			//		+ "OPEN v_cur FOR select v_id from dual;"
//			//		+ System.Environment.NewLine
//			//		+ "RETURN v_cur;"
//			//		+ System.Environment.NewLine
//			//		);
//			//}
//			//else
//			//{
//				sb.AppendLine("v_exists   NUMBER;");
//				sb.AppendLine("begin");
//				sb.AppendLine(string.Format("SELECT COUNT (*) INTO v_exists FROM {0} WHERE {1} = {2};", tableName,
//					keyColumnName, keyParName));
//				sb.AppendLine("IF v_exists = 0 THEN");
//				sb.AppendLine(string.Format("RETURN ins ({0});", parsUseNoKeyStr));
//				sb.AppendLine("ELSE");
//				sb.AppendLine(string.Format("upd ({0});", parsUseStr));
//				sb.AppendLine(string.Format("OPEN v_cur FOR select {0} from dual;", keyParName));
//				sb.AppendLine("RETURN v_cur;");
//				sb.AppendLine("END IF;");
//			//}
//            sb.AppendLine("end;");

//            return sb;
//        }

//        private static StringBuilder Generate_DelMethod(string funcName, string tableName, string keyColumnName,
//			string keyParName, bool testScheme, bool manualInsUpd, XElement query)
//        {
//            var sb = new StringBuilder();
//            sb.AppendLine(string.Format("procedure {0} ({1} number) is ", funcName, keyParName));
//            sb.AppendLine("begin");
//			sb.AppendLine("	DBMS_SESSION.SET_CONTEXT('CLIENTCONTEXT', 'vr_hist_all_context', 'ASUTP_packages' );");
//			if (manualInsUpd)
//			{
//				sb.AppendLine((testScheme ? "SVCT" : "SVC")
//					+ "_MANUAL.DEL('"
//					+ query.Attribute(TextConst.AName.Name).Value
//					+ "'," + keyParName + ");");
//				//sb.AppendLine("RAISE_APPLICATION_ERROR(-20999, 'Deletion not implemented.');");
//			}
//			else
//			{
//				sb.AppendLine(string.Format("delete {0} WHERE {1} = {2};", tableName, keyColumnName, keyParName));
//			}
            
//            sb.AppendLine("commit;");
//			sb.AppendLine("	DBMS_SESSION.SET_CONTEXT('CLIENTCONTEXT', 'vr_hist_all_context', '' );");
//            sb.AppendLine("end;");

//            return sb;
//        }

//        private static string classNameFromQueryName(string name)
//        {
//            return name.Replace("svc_", "");
//        }

//        private static StringBuilder Generate_GetAll(string queryName)
//        {
//            var className = classNameFromQueryName(queryName);

//            var funcName = "get_all";
//            var qry = XmlReports.Environment.GetPrecompiledQuery(queryName);
//            return Generate_GetMethod(funcName, qry);
//        }
//		private static StringBuilder Generate_GetCount(string queryName)
//		{
//			var className = classNameFromQueryName(queryName);

//			var funcName = "get_count";
//			var qry = XmlReports.Environment.GetPrecompiledQuery(queryName);

//			var sb = new StringBuilder();
//			var ri = SqlReportPkg.PrepareReportInfo(qry, false);
//			var parsStr = string.Join(",", ri.ParsDefinition);
//			if (!string.IsNullOrEmpty(parsStr))
//			{
//				parsStr = "(" + parsStr + ")";
//			}
//			var sql = ri.DataSet.GetAllTables().First().DataAdapter.SelectCommand.CommandText;

//			sb.AppendLine(string.Format("function {0} {1} RETURN SYS_REFCURSOR is ", funcName, parsStr));
//			sb.AppendLine("v_cur SYS_REFCURSOR;");
//			sb.AppendLine("begin");
//			sb.AppendLine("OPEN v_cur FOR");
//			sb.AppendLine("SELECT COUNT(*) FROM (");
//			sb.AppendLine(sql + ");");
//			sb.AppendLine("RETURN v_cur;");
//			sb.AppendLine("end;");
//			return sb;

//		}





      

//        private static StringBuilder Generate_GetModified(string queryName, bool testScheme, bool manualInsUpd)
//        {
//            var className = classNameFromQueryName(queryName);

//            var funcName = "get_modified";

//            var qry = XmlReports.Environment.GetPrecompiledQuery(queryName);
//            var tableName = qry.SearchSourceTable().GetMainParent().P_Name;
//			var qry1 = ExtendQuery_GetModified(qry, className, tableName);
//			StringBuilder sbGetModified;
//			if (manualInsUpd)
//			{
//				sbGetModified = new StringBuilder();
//				sbGetModified.Append(
//					"FUNCTION GET_MODIFIED (P_FROM_DATE DATE) RETURN SYS_REFCURSOR IS " + System.Environment.NewLine
//					+ "	V_CUR SYS_REFCURSOR;" + System.Environment.NewLine
//					+ "BEGIN" + System.Environment.NewLine
//					+ "RETURN SVC" + (testScheme ? "T" : "") + "_MANUAL.GET_MODIFIED('" + queryName + "',P_FROM_DATE);" + System.Environment.NewLine
//					+ "END;" + System.Environment.NewLine
//					);
//			}
//			else
//			{
//				sbGetModified = Generate_GetMethod(funcName, qry1);
//			}
//			return sbGetModified;
//        }

//        private static StringBuilder Generate_GetChageInfo(string queryName, bool testScheme, bool manualInsUpd)
//        {
//			var qry = XmlReports.Environment.GetPrecompiledQuery(queryName);
//			var tableName = qry.SearchSourceTable().GetMainParent().P_Name;
//			var keyName = qry.KeyColumn().XName;
//			string s;
//			if (manualInsUpd)
//			{
//				s = @"FUNCTION get_change_info (p_from_date DATE)
//						RETURN SYS_REFCURSOR IS 
//						v_cur   SYS_REFCURSOR;
//					BEGIN
//						RETURN SVC{0}_MANUAL.GET_CHANGE_INFO('{1}',p_from_date);
//					END;";
//				s = string.Format(s, testScheme ? "T" : "", queryName);
//			}
//			else
//			{
//				s = @"FUNCTION get_change_info (p_from_date DATE)
//						RETURN SYS_REFCURSOR IS 
//						v_cur   SYS_REFCURSOR;
//					BEGIN
//						OPEN v_cur FOR
//								SELECT key AS {1},
//										MAX (dat) date_edit,
//										MAX (DECODE (op_type, 'D', 1, 0)) is_delete
//								FROM (select key,dat,op_type from  vr_hist_all WHERE tbl = '{0}' AND dat >= p_from_date AND user_login !='ASUTP'
//										union all
//										select key,dat,op_type from  vr_hist_all_arch WHERE tbl = '{0}' AND dat >= p_from_date AND user_login !='ASUTP')
//							GROUP BY key;
//						RETURN v_cur;
//					END;";
//				s = string.Format(s, tableName,keyName);
//			}



//			var sb = new StringBuilder();
//			sb.AppendLine(s);
//			return sb;
//        }


//		private static StringBuilder Generate_Insert(string queryName, bool testScheme, bool manualInsUpd)
//        {
//            var qry = (XElement) XmlReports.Environment.GetPrecompiledQuery(queryName);
//            qry = ExtendQuery_InsUpd(qry, false, manualInsUpd);

//            var funcName = "ins";
//			return Generate_InsertMethod(funcName, qry, testScheme, manualInsUpd);
//        }


//		private static StringBuilder Generate_Update(string queryName, bool testScheme, bool manualInsUpd)
//        {
//            var qry = (XElement) XmlReports.Environment.GetPrecompiledQuery(queryName);
//			StringBuilder parentChangeChecks = manualInsUpd ? new StringBuilder() : Generate_Update_Parent_Change_Check(qry);
//			StringBuilder dogDoneChecks = manualInsUpd ? new StringBuilder() : Generate_Update_Dogovor_Done_Check(qry);

//			StringBuilder checks = new StringBuilder();
//			checks.Append(dogDoneChecks);
//			checks.Append(parentChangeChecks);
//			qry = ExtendQuery_InsUpd(qry, true, manualInsUpd);
//            var funcName = "upd";
//			return Generate_UpdateMethod(funcName, qry, testScheme, manualInsUpd, checks);
//        }

//		private static StringBuilder Generate_Update_Dogovor_Done_Check(XElement qry)
//		{
//			StringBuilder checks = new StringBuilder();
//			//{0} DOG KEY PARAM NAME
//			string pattern = @"
//				DECLARE
//					v_dog_status is_dop_contract.status%type;
//				begin
//					if (p_{0} is not null) then
//						select status into v_dog_status from is_dop_contract where kod_dop_contract = p_{0};
//						if (nvl(v_dog_status,0) not in (29,0,1)) then
//							ROLLBACK;
//							RAISE_APPLICATION_ERROR(-20999, 'svc_dog status not in (29,0,1). All changes prohibited');
//						end if;	
//					end if;
//				end;
//";
//			var dogColumns = qry.Element(TextConst.EName.Select).Elements(TextConst.EName.Column).Where(el => ((el.Attribute(TextConst.AName.Link) != null) && el.Attribute(TextConst.AName.Link).Value == "svc_dog")).ToList();
//			foreach (var dogColumn in dogColumns)
//			{
//				string dogColumnName = (dogColumn.Attribute(TextConst.AName.As) == null) ? dogColumn.Attribute(TextConst.AName.Column).Value : dogColumn.Attribute(TextConst.AName.As).Value;
//				checks.Append(string.Format(pattern, dogColumnName));
//			}
//			return checks;
//		}
		
//		private static StringBuilder Generate_Update_Parent_Change_Check(XElement qry)
//		{
//			StringBuilder checks = new StringBuilder();
//			//{0} PARENT COLUMN NAME
//			//{1} TABLE NAME
//			//{2} KEY COLUMN NAME
//			//{3} KEY COLUMN PARAM NAME
//			//{4} PARENT COLUMN PARAM NAME
//			//{5} package name
//			string pattern = @"
//				DECLARE
//					V_{0}_OLD		{1}.{0}%TYPE;
//				BEGIN
//					select {0} into V_{0}_OLD from {1} WHERE {2} = {3};
//					if (nvl(V_{0}_OLD,-1) != nvl({4},-1)) then --will fail in case one -1 and another null
//						ROLLBACK;
//						RAISE_APPLICATION_ERROR(-20999, '{5} {4} change prohibited');
//					end if;
//				END;
//";
//			string tableName = qry.Element(TextConst.EName.From).Element(TextConst.EName.Query).Attribute(TextConst.AName.Name).Value;
//			string keyColumnName = qry.Element(TextConst.EName.Select).Elements(TextConst.EName.Column).First(el => (el.Attribute(TextConst.AName.Key).Value == "1")).Attribute(TextConst.AName.Column).Value;
//			string keyColumnParamName = "p_" + qry.Element(TextConst.EName.Select).Elements(TextConst.EName.Column).First(el => (el.Attribute(TextConst.AName.Key).Value == "1")).Attribute(TextConst.AName.As).Value;
//			string packageName = qry.Attribute(TextConst.AName.Name).Value;

//			var parentColumns = qry.Element(TextConst.EName.Select).Elements(TextConst.EName.Column).Where(el => ((el.Attribute(TextConst.AName.Comment) != null) && el.Attribute(TextConst.AName.Comment).Value.Contains("update_prohibited"))).ToList();
//			foreach (var parentColumn in parentColumns)
//			{
//				string parentColumnName = parentColumn.Attribute(TextConst.AName.Column).Value;
//				string parentColumnParamName = "p_" + parentColumn.Attribute(TextConst.AName.As).Value;
//				checks.Append(string.Format(pattern, parentColumnName, tableName, keyColumnName, keyColumnParamName, parentColumnParamName, packageName));
//			}
//			return checks;
//		}


//		private static StringBuilder Generate_Put(string queryName, bool manualInsUpd)
//        {
//            var qry = XmlReports.Environment.GetPrecompiledQuery(queryName);
//            var tableName = qry.SearchSourceTable().P_Name;
//            var keyName = (qry.KeyColumn() as VColumn).SearchSourceDbColumn().P_Column;
//            var keyParName = "p_" + qry.KeyColumn().XName;
//			var eqry = ExtendQuery_InsUpd(qry, true, manualInsUpd);
//            var funcName = "put";
//			return Generate_PutMethod(funcName, eqry, tableName, keyName, keyParName);
//        }

//		private static StringBuilder Generate_Del(string queryName, bool testScheme, bool manualInsUpd)
//        {
//            var qry = XmlReports.Environment.GetPrecompiledQuery(queryName);
//            var tableName = qry.SearchSourceTable().P_Name;
//            var keyName = (qry.KeyColumn() as VColumn).SearchSourceDbColumn().P_Column;
//            var keyParName = "p_" + qry.KeyColumn().XName;
//            var funcName = "del";
//			return Generate_DelMethod(funcName, tableName, keyName, keyParName, testScheme, manualInsUpd, qry);
//        }

//		private static StringBuilder GenerateTempTable(string queryName, bool testScheme)
//		{
//			var sbTempTable = new StringBuilder();
//			bool tempTableRequired = false;

//			var qry = (XElement)XmlReports.Environment.GetPrecompiledQuery(queryName);
//            var tqry = VSXElement.Get<VQuery>(qry);

//			foreach (var col in tqry.Columns())
//			{
//				if (col is VColumn && (col.P_Table != tqry.MainSource().XName))
//				{
//					tempTableRequired = true;
//				}
//				if (!(col is VColumn))
//				{
//					tempTableRequired = true;
//				}
//			}
//			if (tqry.Element("from").Descendants().Count() > 1)
//			{
//				tempTableRequired = true;
//			}


//			if (tempTableRequired)
//			{
//				var newQueryName = queryName + tempTablePostfix;
//				if (testScheme)
//				{
//					newQueryName = queryName.Replace("svc_", "svct_") + tempTablePostfix;
//				}
//				sbTempTable = new StringBuilder(SqlSchemeBuilder.GenerateTableScript(tqry, true, true, true, true, false));
//				if (testScheme)
//				{
//					sbTempTable.Replace(queryName, newQueryName);
//				}
//				/*
//				sbTempTable.AppendLine(string.Format("GRANT SELECT ON {0} TO ASUTP", newQueryName));
//				sbTempTable.AppendLine("/");
//				sbTempTable.AppendLine(string.Format("GRANT INSERT ON {0} TO ASUTP", newQueryName));
//				sbTempTable.AppendLine("/");
//				sbTempTable.AppendLine(string.Format("GRANT UPDATE ON {0} TO ASUTP", newQueryName));
//				sbTempTable.AppendLine("/");
//				sbTempTable.AppendLine(string.Format("GRANT DELETE ON {0} TO ASUTP", newQueryName));
//				sbTempTable.AppendLine("/");
//				*/

				
//			}

//			return sbTempTable;
//		}
//		private static StringBuilder generateInsManual(XElement query, bool testScheme)
//		{
//			StringBuilder sbInto = new StringBuilder("INSERT INTO " + query.Attribute(TextConst.AName.UpdateTarget).Value + tempTablePostfix + " (");
//			if (testScheme)
//			{
//				sbInto.Replace("svc_", "svct_");
//			}
//			StringBuilder sbValues = new StringBuilder("VALUES (");
//			foreach (var xmlCall in query.Element(TextConst.EName.Select).Elements(TextConst.EName.Call))
//			{
//				sbInto.Append(xmlCall.Attribute(TextConst.AName.As).Value + ", ");
//				sbValues.Append(xmlCall.Element(TextConst.EName.UseParam).Attribute(TextConst.AName.Name).Value + ", ");
//			}
//			sbInto.Remove(sbInto.Length - 2, 2);
//			sbValues.Remove(sbValues.Length - 2, 2);
//			sbInto.Append(") ");
//			sbValues.Append(");" + System.Environment.NewLine);
//			sbInto.Append(sbValues);
//			return sbInto;
//		}




//        #region extend query

//        private static XElement PrepareQueryForExtend(XElement query)
//        {
//            query = new XElement(query);
//            query.Attributes(TextConst.AName.Name).Remove();
//            return query;
//        }

//        private static XElement ExtendQuery_GetByField(XElement query, string columnName, string paramName)
//        {


//            query = PrepareQueryForExtend(query);
//            var keyName = columnName;
//            var keyParName = paramName;

//            var xpars = new XElement(TextConst.EName.Params
//                , new XElement(TextConst.EName.Param
//                    , new XAttribute(TextConst.AName.DataType, TextConst.AVDataType.Number)
//                    , new XAttribute(TextConst.AName.Name, keyParName)
//                    )
//                );

//            query.AddFirst(xpars);

//            var xwhere = new XElement(TextConst.EName.Where,
//                new XElement(TextConst.EName.Call
//                    , new XAttribute(TextConst.AName.Function, TextConst.AVFunction.And)
//                    , query.Elements(TextConst.EName.Where).Elements()
//                    ,new XElement(TextConst.EName.Call,
//                         new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Equal)
//                        , new XElement(TextConst.EName.Column
//                            , new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
//                            , new XAttribute(TextConst.AName.Column, keyName)
//                            )
//                        , new XElement(TextConst.EName.UseParam
//                            , new XAttribute(TextConst.AName.Name, keyParName)
//                            )
//                        )
//                    )

//                );
//            query.Elements(TextConst.EName.Where).Remove();
//            query.Add(xwhere);
//            return query;
//        }

//		private static XElement ExtendQuery_InsUpd(XElement query, bool withKey, bool manualInsUpd)
//        {


//            var resQuery = new XElement(TextConst.EName.Query);
//            var tqry = VSXElement.Get<VQuery>(query);
//            var xpars = new XElement(TextConst.EName.Params);
//            var xselect = new XElement(TextConst.EName.Select);

//        //    var tqryA = manualInsUpd ? tqry. : tqry.Columns();
//            foreach (var col in tqry.Columns())
//            {
//                if ((col.P_Key != TextConst.AVBool.True || withKey) && col is VColumn)
//                {
//                    var tcol = (VColumn) col;
//                    var srcCol = tcol.SearchSourceDbColumn();
//                    var parName = "p_" + col.XName;
//                    var xpar = new XElement(TextConst.EName.Param
//                        , new XAttribute(TextConst.AName.DataType, col.XDataType())
//                        , new XAttribute(TextConst.AName.Name, parName)
//                        );
//                    xpars.Add(xpar);
//					var srcColName = srcCol.P_Column;
//					if (manualInsUpd)
//					{
//						srcColName = col.XName;
//					}
//					var xcol = new XElement(TextConst.EName.Call
//						, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Dummy)
//						, new XAttribute(TextConst.AName.As, srcColName)
//						, new XAttribute(TextConst.AName.DataType, col.XDataType())
//						, new XElement(TextConst.EName.UseParam
//							, new XAttribute(TextConst.AName.Name, parName)
//							)
//						);
					
//                    xselect.Add(xcol);
//                }
//                else if ((col.P_Key != TextConst.AVBool.True || withKey) && manualInsUpd)
//                {
//                  //  var tcol = (VCall)col;
//                  //  var srcCol = tcol.SearchSourceDbColumn();
//                    var parName = "p_" + col.XName;
//                    var xpar = new XElement(TextConst.EName.Param
//                        , new XAttribute(TextConst.AName.DataType, col.XDataType())
//                        , new XAttribute(TextConst.AName.Name, parName)
//                        );
//                    xpars.Add(xpar);
//                  //  var srcColName = srcCol.P_Column;
//                    var srcColName = col.XName; 
//                    var xcol = new XElement(TextConst.EName.Call
//                        , new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Dummy)
//                        , new XAttribute(TextConst.AName.As, srcColName)
//                        , new XAttribute(TextConst.AName.DataType, col.XDataType())
//                        , new XElement(TextConst.EName.UseParam
//                            , new XAttribute(TextConst.AName.Name, parName)
//                            )
//                        );

//                    xselect.Add(xcol);
//                }
//            }

//            var xfrom = new XElement(TextConst.EName.From
//                , new XElement(TextConst.EName.Table
//                    , new XAttribute(TextConst.AName.Name, TextConst.AVTable.Dual)
//                    , new XAttribute(TextConst.AName.As, "a")
//                    )
//                );
//			if (manualInsUpd)
//			{
//				resQuery.SetAttributeValue(TextConst.AName.UpdateTarget, tqry.Attribute(TextConst.AName.Name).Value);
//			}
//			else
//			{
//				resQuery.SetAttributeValue(TextConst.AName.UpdateTarget, tqry.SearchSourceTable().P_IdName);
//			}
//            resQuery.Add(xpars);
//            resQuery.Add(xselect);
//            resQuery.Add(xfrom);

//            return resQuery;
//        }

//		private static XElement ExtendQuery_GetModified(XElement query, string className, string tableName)
//        {
       
//            query = PrepareQueryForExtend(query);
//            var keyName = className + "_id";
//            var parName = "p_from_date";
//            var xpars = new XElement(TextConst.EName.Params
//                , new XElement(TextConst.EName.Param
//                    , new XAttribute(TextConst.AName.DataType, TextConst.AVDataType.Date)
//                    , new XAttribute(TextConst.AName.Name, parName)
//                    )
//                );

//            query.AddFirst(xpars);
//			tableName = Cmn.Nvle(tableName, "null").ToString();
//			XElement xwhere;
//			xwhere = new XElement(TextConst.EName.Where
//				, new XElement(TextConst.EName.Call
//					, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.And)
//					, query.Elements(TextConst.EName.Where).Elements()
//					, new XElement(TextConst.EName.Call
//						, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.In)
//						, new XElement(TextConst.EName.Column
//							, new XAttribute(TextConst.AName.Table, TextConst.AVTable.Ths)
//							, new XAttribute(TextConst.AName.Column, keyName)
//							)
//						, new XElement(TextConst.EName.Const,
//							new XText(
//									string.Format("(SELECT i.key FROM vr_hist_all i  WHERE  i.tbl = '{0}' AND i.dat >= p_from_date AND user_login !='ASUTP' union all SELECT ia.key FROM vr_hist_all_arch ia  WHERE  ia.tbl = '{0}' AND ia.dat >= p_from_date AND user_login !='ASUTP')", tableName)
//								)
//							)
//						)
//					)

//				);
			
//            query.Elements(TextConst.EName.Where).Remove();
//            query.Add(xwhere);
//            return query;
//        }

//        #endregion

//    }
//}
