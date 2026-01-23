//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Text;
//using sql.builder.DataApi;
//using System.Xml.Linq;

//namespace sql.builder.XmlHelpers
//{
//    internal static class SqlSchemeBuilder
//    {
//        internal static string GenerateTableScript(VQuery query, bool with_drops = false,bool isTemporary=false, bool longStrings = false, bool genSynonim = true, bool genGrants = true)
//        {
//            var text = new StringBuilder();
//            string tableName = query.P_Name;
//            if (with_drops) {
//                GenerateDrops(ref text, query, tableName, isTemporary);
//            }
//			GenerateTable(text, query, tableName, isTemporary, longStrings);
//            if (!isTemporary) {
//                GeneratePrimaryKey(ref text, query);
//                GenerateForeignKeys(ref text, query);
//                GenerateSequence(ref text, query);
//                GenerateTrigger(ref text, query);
//            }
//			if (genSynonim) {
//				GeneratePublicSynonym(ref text, query, tableName);
//			}
//            GenerateComments(ref text, query, tableName,isTemporary);
//			if (genGrants) {
//				GenerateGrants(ref text, query, tableName,false,true); 
//			}
//            GenerateIndexes(ref text, query, tableName);
//            return text.ToString();
//        }
//        internal static void GenerateIndexes(ref StringBuilder text, VQuery query, string tableName, VSXElement[] cols = null)
//        {
//            string s = GenerateIndexesString(query, query.P_IdName, tableName, cols);
//            text.Append(s);
//        }
//        internal static string GenerateIndexesString(VSXElement query, string name, string tableName, VSXElement[] cols = null)
//        {
//            var text = new StringBuilder();
//            var qname = query.P_Name;
      
//            // XmlReports.loadXml(null, null, SqlBuilder.InputParams);
//            VQuery q = XmlReports.Environment.GetPrecompiledQuery(qname);
//            var idxstr = @"CREATE INDEX {2} ON {0} ({1}  ASC )";
//            List<string> colsWithIndex=new List<string>();
//            if (cols == null)
//            {
//                var colsWithIndex1 = q.Elements(TextConst.EName.From).Descendants(TextConst.EName.Column)
//                    .Where(e1 => Cmn.GetAttrValue(e1, TextConst.AName.Table) == TextConst.AVTable.Ths)
//                    .Select(e2 => e2.Attribute(TextConst.AName.Column).Value).Distinct().ToList();
//                colsWithIndex.AddRange(colsWithIndex1);
//                colsWithIndex.AddRange(
//                    (query as VQuery).Columns().Where(e => e.P_Index != "").Select(c => c.XName).Distinct().ToList());
//            }
//            else
//            {
//                colsWithIndex.AddRange(cols.Where(e => e.P_Index != "").Select(c => c.XName).Distinct().ToList());
//            }

           
          
//            colsWithIndex = colsWithIndex.Distinct().ToList();

//            int i = 1;
//            foreach (string col in colsWithIndex)
//            {
//                var iname = name + "_" + col;
//                if (iname.Length > 30)
//                {
//                    iname = name + "_" + i.ToString();
//                    i++;
//                }
//                var idxstr1 = string.Format(idxstr, tableName, col, iname);
//                text.AppendLine(idxstr1);
//                text.AppendLine(@"/");
//            }

//            return text.ToString();
//        }







//        private static void GenerateDrops(ref StringBuilder text, VQuery query,string tableName, bool isTemporary = false)
//        {
//            text.AppendLine("-- Уничтожение");
//            text.AppendLine(string.Format("DROP TABLE {0};", tableName));
//            text.AppendLine(string.Format("DROP PUBLIC SYNONYM {0};", tableName));
//            if (!isTemporary)
//            {
//                text.AppendLine(string.Format("DROP SEQUENCE sq{0};", tableName));
//            }
//            text.AppendLine("/");
//        }

//        private static VSXElement[] GetTableCols(VQuery query, bool isTemporary)
//        {
//            VSXElement[] cols = null;
//            if (!isTemporary)
//            {
//                cols = query.NativeColumns().OfType<VColumn>().Where(VColumn.IsTableColumn).ToArray();
//            }
//            else
//            {

//                cols = query.Columns().SelectMany(VColumn.SelfOrMultipleSource).ToArray();
//            }
//            return cols;

//        }
//		private static void GenerateTable(StringBuilder text, VQuery query, string tableName, bool isTemporary = false, bool longStrings = false)
//        {
//            text.AppendLine("-- Таблица");
//            text.Append("CREATE");
//            if (isTemporary) {
//                text.Append(" GLOBAL TEMPORARY");
//            }
//            text.Append(" TABLE ");
//            text.AppendLine(tableName);
//            text.AppendLine(" (");
//            VSXElement[] cols = GetTableCols(query,isTemporary);
//            for (int index = 0; index < cols.Length; index++) {
//                VSXElement col = cols[index];
//                switch (col.XDataType()) {
//                    case TextConst.AVDataType.String:
//                        string size = col.XDataSize();
//                        if (size == string.Empty) {
//							if (longStrings) {
//								size = "4000";
//							} else {
//								size = "300";
//							}
//                        }
//                        text.Append('\t');
//                        text.Append(col.XName);
//                        text.Append(" VARCHAR2(");
//                        text.Append(size);
//                        text.Append(')');
//                        break;
//                    case TextConst.AVDataType.Number:
//                        text.Append('\t');
//                        text.Append(col.XName);
//                        text.Append(" NUMBER");
//                        break;
//                    case TextConst.AVDataType.Date:
//                        text.Append('\t');
//                        text.Append(col.XName);
//                        text.Append(" DATE");
//                        break;
//                    case TextConst.AVDataType.Clob:
//                        text.Append('\t');
//                        text.Append(col.P_Column);
//                        text.Append(" CLOB");
//                        break;
//					case TextConst.AVDataType.Blob:
//                        text.Append('\t');
//                        text.Append(col.P_Column);
//                        text.Append(" BLOB");
//						break;
//                }
//                if (col.P_ColumnMandatory == TextConst.AVBool.True) {
//                    text.Append(" NOT NULL");
//                }
//                if (index < cols.Length - 1) {
//                    text.Append(',');
//                }
//                text.AppendLine();
//            }
//            text.AppendLine(")");
//            if (isTemporary) {
//                text.AppendLine("ON COMMIT PRESERVE ROWS");
//            }
//            text.AppendLine("/");
//        }
//        private static void GeneratePrimaryKey(ref StringBuilder text, VQuery query, bool check_exists = false)
//        {
//            string constraint_name = "xpk" + query.P_Name;
//            if (check_exists)
//            {
//                bool exists = db.GetTableConstraints(query.P_Name).AsEnumerable().Any(r => (string)r["CONSTRAINT_NAME"] == constraint_name.ToUpper());
//                if (exists) return;
//            }

//            text.AppendLine("-- Уникальный ключ");
//            text.AppendLine(string.Format("ALTER TABLE {0} ADD CONSTRAINT {2} PRIMARY KEY ({1}) USING INDEX;", query.P_Name, query.KeyColumn().P_Name, constraint_name));
//            text.AppendLine("/");
//        }
//        private static void GenerateForeignKeys(ref StringBuilder text, VQuery query, bool check_exists = false)
//        {
           
//            var dt_constraints = (check_exists) ? db.GetTableConstraints(query.P_Name) : null;

//            var cols = query.NativeColumns().OfType<VColumn>().Where(VColumn.IsTableColumn).ToArray();
//            foreach (VColumn col in cols)
//            {
//                var rel = col.TypeRelation();
//                if (rel != null)
//                {
//                    var parent = rel.ParentQuery();
//                    if (parent == null) continue;
//                    // фиктивный запрос без таблицы в бд 
//                    if (!parent.Elements(TextConst.EName.Select).Any()) continue;

//                    string identifier_name = string.Format("{0}_{1}", query.P_Name, rel.XName);
//                    // максимум 30, но нужно учесть xifk
//                    if (identifier_name.Length > 26) identifier_name = identifier_name.Substring(0, 26);

//                    string constraint_name = "xfk" + identifier_name;
//                    string index_name = "xfk" + identifier_name;

//                    if(check_exists)
//                    {
//                        bool exists = dt_constraints.AsEnumerable().Any(r => (string)r["CONSTRAINT_NAME"] == constraint_name.ToUpper());
//                        if(exists) continue;
//                    }

//                    text.AppendLine(string.Format("--Связь {0} с {1}", query.P_Name, parent.P_Name));
//                    text.Append(string.Format("ALTER TABLE {0} ADD CONSTRAINT {4} FOREIGN KEY ({2}) REFERENCES {1} ({3})", query.P_Name, parent.P_Name, col.P_Name, parent.KeyColumn().P_Name, constraint_name));
//                    if (rel.P_ConstrDelOption != "") text.Append(" ON DELETE " + rel.P_ConstrDelOption);
//                    text.AppendLine(";");
//                    text.AppendLine(string.Format("CREATE INDEX {2} ON {0} ({1});", query.P_Name, col.P_Name, index_name));
//                    text.AppendLine("/");
//                }
//            }
//        }
//        private static void GenerateSequence(ref StringBuilder text, VQuery query)
//        {
//            text.AppendLine("-- Сиквенс");
//            text.AppendLine(string.Format("CREATE SEQUENCE sq{0} INCREMENT BY 1 START WITH 1;", query.P_Name));
//            text.AppendLine("/");
//        }
//        private static string mpepProjName = "mped";
//        private static void GenerateTrigger(ref StringBuilder text, VQuery query)
//        {
//            var table = query.Childs().OfType<VFrom>().First().Childs().OfType<VTable>().First();
//            text.AppendLine("-- Триггер");
//            text.AppendLine(string.Format("CREATE OR REPLACE TRIGGER t_{0}", query.P_Name));
//            text.AppendLine(string.Format("BEFORE INSERT OR DELETE OR UPDATE ON {0}", query.P_Name));
//            text.AppendLine("REFERENCING NEW AS NEW OLD AS OLD FOR EACH ROW");
//            text.AppendLine("DECLARE");

//            if (table.P_TableCode != "")
//            {
//                text.AppendLine(string.Format("\ttk NUMBER := {0}; -- код из таблицы rk_izm_tab", table.P_TableCode));
//                text.AppendLine("\tbchanged BOOLEAN;");
//            }
//            text.AppendLine("BEGIN");
//            if (query.GetProjectName() != mpepProjName)
//            {
               
//                text.AppendLine("\tIF(USER = kg_common.repadmin) THEN");
//                text.AppendLine("\t\tRETURN;");
//                text.AppendLine("\tEND IF;");
//                text.AppendLine();
//            }

//            if (table.P_TableCode != "")
//            {
//                text.AppendLine("\tIF(kg_common.get_pravo(USER,tk)) != 2 THEN");
//                text.AppendLine("\t\traise_application_error(-20101, kg_error.msg_netprav);");
//                text.AppendLine("\tEND IF;");
//                text.AppendLine();
//            }

//            text.AppendLine(string.Format("\tIF(INSERTING and :NEW.{0} IS NULL) THEN", query.KeyColumn().P_Name));
//            text.AppendLine(string.Format("\t\tSELECT sq{0}.nextval INTO :NEW.{1} FROM DUAL;", query.P_Name, query.KeyColumn().P_Name));
//            text.AppendLine("\tEND IF;");
//            text.AppendLine();

//            string[] sys_cols_names = { "u_m", "d_m", "t_m" };
//            IList<VColumn> sys_cols = query.NativeColumns().OfType<VColumn>().Where(c => VColumn.IsTableColumn(c) && (sys_cols_names.Contains(c.P_Name))).ToList();
//            if (sys_cols.Count > 0) {
//                text.AppendLine("\tIF rg_kor_util.enabled AND NOT DELETING THEN");
//                if (sys_cols.Any(c => c.P_Name == "u_m")) text.AppendLine("\t\tSELECT USER INTO :NEW.u_m FROM DUAL;");
//                if (sys_cols.Any(c => c.P_Name == "d_m")) text.AppendLine("\t\tSELECT SYSDATE INTO :NEW.d_m FROM DUAL;");
//                if (sys_cols.Any(c => c.P_Name == "t_m")) text.AppendLine("\t\tSELECT 'Terminal: '||SYS_CONTEXT('USERENV', 'TERMINAL')||', OS User: '||SYS_CONTEXT('USERENV', 'OS_USER') INTO :NEW.t_m FROM DUAL;");
//                text.AppendLine("\tEND IF;");
//                text.AppendLine();
//            }

//            if (table.P_TableCode != "")
//            {
//                text.AppendLine("\tIF(INSERTING) THEN");
//                text.AppendLine(string.Format("\t\trg_kor1.ins(tk,:NEW.{0},NULL,'{1}',NULL,NULL,NULL,NULL);", query.KeyColumn().P_Name, query.P_Name));
//                text.AppendLine("\tELSIF(UPDATING) THEN");

//                var changeble_cols = query.NativeColumns().OfType<VColumn>().Where(c => VColumn.IsTableColumn(c) && c != query.KeyColumn() && c.P_Name != "u_m" && c.P_Name != "d_m");
//                foreach (var col in changeble_cols)
//                {
//                    text.AppendLine(string.Format("\t\t-- {0}", col.P_Name));
//                    text.AppendLine(string.Format("\t\trg_kor1.upd(tk,:NEW.{0},NULL,TO_CHAR(:OLD.{1}),TO_CHAR(:OLD.{1}),'{2}.{1}',NULL,NULL,NULL,NULL,bchanged);",
//                        query.KeyColumn().P_Name, col.P_Name, query.P_Name));
//                }

//                text.AppendLine("\tELSIF(DELETING) THEN");
//                text.AppendLine(string.Format("\t\trg_kor1.del(tk,:OLD.{0},NULL,'{1}',NULL,NULL,NULL,NULL);", query.KeyColumn().P_Name, query.P_Name));
//                text.AppendLine("\tEND IF;");
//            }

//            text.AppendLine("END;");
//            text.AppendLine("/");
//        }
//        private static void GeneratePublicSynonym(ref StringBuilder text, VQuery query, string tableName)
//        {
//            //if (query.GetProjectName() != mpepProjName)
//            //{
//                text.AppendLine("-- Синоним");
//                text.AppendLine(string.Format("CREATE PUBLIC SYNONYM {0} FOR {0};", tableName));
//                text.AppendLine("/");
//            //}
//        }
//        private static void GenerateComments(ref StringBuilder text, VQuery query, string tableName,bool isTemporary)
//        {
//            text.AppendLine("-- Комментарии");
//            if (query.P_Title != "")
//            {
//                text.AppendLine(string.Format("COMMENT ON TABLE {0} IS '{1}';", tableName, query.P_Title));
//            }
//            VSXElement[] cols = GetTableCols(query, isTemporary);

//            cols = cols.Where(c => c.P_Title != "").ToArray();
//            foreach (VSXElement col in cols)
//            {
//                text.AppendLine(string.Format("COMMENT ON COLUMN {0}.{1} IS '{2}';", tableName, col.XName, col.P_Title));
//            }
//            text.AppendLine("/");
//        }
//        public static void GenerateGrants(ref StringBuilder text, VQuery query, string tableName, bool isPackage = false, bool isTemporary=false)
//        {
//            text.AppendLine("-- Гранты");

//            if (isPackage)
//            {
//                text.AppendLine(string.Format("GRANT EXECUTE ON {0} TO WRITE_ADMIN;", tableName));
//                text.AppendLine(string.Format("GRANT EXECUTE ON {0} TO WRITE_K;", tableName));
//                text.AppendLine(string.Format("GRANT EXECUTE ON {0} TO WRITE_U;", tableName));
//                text.AppendLine(string.Format("GRANT EXECUTE ON {0} TO WRITE_T;", tableName));
//                text.AppendLine(string.Format("GRANT EXECUTE ON {0} TO WRITE_H;", tableName));
//                text.AppendLine(string.Format("GRANT EXECUTE ON {0} TO WRITE_S;", tableName));

//            }
//            //else if (!query.P_Name.StartsWith("vr_"))
//            else if (!isTemporary)
//            {
//                text.AppendLine(string.Format("GRANT SELECT ON {0} TO V_RW_ALL;", tableName));
//                text.AppendLine(string.Format("GRANT INSERT ON {0} TO V_RW_ALL;", tableName));
//                text.AppendLine(string.Format("GRANT UPDATE ON {0} TO V_RW_ALL;", tableName));
//                text.AppendLine(string.Format("GRANT DELETE ON {0} TO V_RW_ALL;", tableName));
//            }
//            else
//            {
//                text.AppendLine(string.Format("GRANT SELECT,INSERT,UPDATE,DELETE ON {0} TO PUBLIC;", tableName));
           
//            }
//            //else
//            //{
//            //    text.AppendLine(string.Format("GRANT SELECT ON {0} TO READ_ALL;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT SELECT ON {0} TO READ_K;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT SELECT ON {0} TO READ_U;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT SELECT ON {0} TO READ_T;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT SELECT ON {0} TO READ_H;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT SELECT ON {0} TO READ_S;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT SELECT ON {0} TO REPADMIN;", query.P_Name));
//            //    text.AppendLine("/");
//            //    text.AppendLine(string.Format("GRANT INSERT ON {0} TO WRITE_ADMIN;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT INSERT ON {0} TO WRITE_K;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT INSERT ON {0} TO WRITE_U;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT INSERT ON {0} TO WRITE_T;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT INSERT ON {0} TO WRITE_H;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT INSERT ON {0} TO WRITE_S;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT INSERT ON {0} TO REPADMIN;", query.P_Name));
//            //    text.AppendLine("/");
//            //    text.AppendLine(string.Format("GRANT UPDATE ON {0} TO WRITE_ADMIN;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT UPDATE ON {0} TO WRITE_K;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT UPDATE ON {0} TO WRITE_U;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT UPDATE ON {0} TO WRITE_T;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT UPDATE ON {0} TO WRITE_H;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT UPDATE ON {0} TO WRITE_S;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT UPDATE ON {0} TO REPADMIN;", query.P_Name));
//            //    text.AppendLine("/");
//            //    text.AppendLine(string.Format("GRANT DELETE ON {0} TO WRITE_ADMIN;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT DELETE ON {0} TO WRITE_K;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT DELETE ON {0} TO WRITE_U;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT DELETE ON {0} TO WRITE_T;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT DELETE ON {0} TO WRITE_H;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT DELETE ON {0} TO WRITE_S;", query.P_Name));
//            //    text.AppendLine(string.Format("GRANT DELETE ON {0} TO REPADMIN;", query.P_Name));
//            //}
//            text.AppendLine("/");
//        }

//        internal static string GenerateAlterTableScript(VQuery query)
//        {
//            var text = new StringBuilder();
//            var tableName = query.P_Name;
//            var cols=GenerateAlterTable(ref text, query);
//            GenerateComments(ref text, query,query.P_Name,false);
//            GeneratePrimaryKey(ref text, query, true);
//            GenerateForeignKeys(ref text, query, true);
//            GenerateIndexes(ref text, query, tableName, cols.Cast<VSXElement>().ToArray());
//            return text.ToString();
//        }

//        private static XElement[] GenerateAlterTable(ref StringBuilder text, VQuery query)
//        {
//            text.AppendLine("-- Новые колонки");

//            var cols_exist = db.GetTableColumns(query.P_Name);
//            var cols = query.NativeColumns().OfType<VColumn>()
//                .Where(c => VColumn.IsTableColumn(c) && cols_exist.AsEnumerable().All(r => (string)r["COLUMN_NAME"] != c.P_Column.ToUpper()))
//                .ToArray();

//            foreach (VColumn col in cols)
//            {
//                text.AppendFormat("ALTER TABLE {0} ADD", query.P_Name);
//                switch (col.P_DataType)
//                {
//                    case TextConst.AVDataType.String:
//                        text.Append(string.Format(" {0} VARCHAR2({1})", col.P_Column, col.P_DataSize));
//                        break;
//                    case TextConst.AVDataType.Number:
//                        text.Append(string.Format(" {0} NUMBER", col.P_Column));
//                        break;
//                    case TextConst.AVDataType.Date:
//                        text.Append(string.Format(" {0} DATE", col.P_Column));
//                        break;
//                    case TextConst.AVDataType.Clob:
//                        text.Append(string.Format(" {0} CLOB", col.P_Column));
//                        break;
//					case TextConst.AVDataType.Blob:
//						text.Append(string.Format(" {0} BLOB", col.P_Column));
//						break;
//                }

//                if (col.P_ColumnMandatory == TextConst.AVBool.True)
//                {
//                    text.Append(" NOT NULL");
//                }

//                text.AppendLine(";");
//            }
         
//            text.AppendLine("/");
//            return cols;
//        }

//		internal static string GenerateHistoryTriggerScript(VQuery query)
//		{
//			const string historyTable = "vr_hist_all";
//			const string historyTableColumns = "(tbl, key, op_type, dat, col_name, val_str, val_num, val_dat, val_str_old, val_num_old, val_dat_old, user_login, context)";

//			string tblName = query.P_Name;
//			string tblKey = query.KeyColumn().P_Name;
//			string caseP =
//				"	IF INSERTING THEN" + System.Environment.NewLine
//				+ "{0}" + System.Environment.NewLine
//				+ "	ELSIF UPDATING THEN" + System.Environment.NewLine
//				+ "{1}" + System.Environment.NewLine
//				+ "	ELSIF DELETING THEN" + System.Environment.NewLine
//				+ "{2}" + System.Environment.NewLine
//				+ "	END IF;" + System.Environment.NewLine;
//			/*
//			{0} - INSERT ACTION
//			{1} - UPDATE ACTION
//			{2} - DELETE ACTION
//			*/
//			string whenP =
//				"		IF {0} THEN" + System.Environment.NewLine
//				+ "{1}"

//				+ "		END IF;" + System.Environment.NewLine;
//			/*
//			{0} - condition //cUpdP|cUpdFastP
//			{1} - insert statement
//			*/
//			string updCondP = "((:OLD.{0} != :NEW.{0}) OR (XOR((:OLD.{0} IS NULL),(:NEW.{0} IS NULL))))"; //column name
//			string updCondFastP = "(updating('{0}'))"; //column name
//			string insCondP = "(:NEW.{0} is not null)"; //column name
//			string delCondP = "(:OLD.{0} is not null)"; //column name
//			string insP = 
//				"			INSERT INTO " + historyTable + " " + historyTableColumns + System.Environment.NewLine +
//				"				VALUES ('" + tblName + "', {0}." + tblKey + ", '{1}', SYSDATE, '{2}', {3}, {4}, {5}, {6}, {7}, {8}, USER, SYS_CONTEXT('CLIENTCONTEXT', 'vr_hist_all_context'));" + System.Environment.NewLine;
//			/*
//			{0} - :OLD for update/delete, :NEW for insert
//			{1} - operation type I/U/D insert/update/delet
//			{2} - column name
//			{3} - val_str
//			{4} - val_num 
//			{5} - val_dat 
//			{6} - val_str_old
//			{7} - val_num_old
//			{8} - val_dat_old
//			*/

//			StringBuilder sbTr = new StringBuilder();
//			StringBuilder sbIn = new StringBuilder();
//			StringBuilder sbUp = new StringBuilder();
//			StringBuilder sbDl = new StringBuilder();
//			if (tblName.Length > 22)
//			{
//				System.Windows.Forms.MessageBox.Show("Table name cropped : " + tblName);
//			}
//			sbTr.Append(
//				"CREATE OR REPLACE TRIGGER TR_HIST_" + ((tblName.Length > 22) ? tblName.Substring(0, 22) : tblName) + System.Environment.NewLine
//				+ "	AFTER INSERT OR UPDATE  OR DELETE" + System.Environment.NewLine
//				+ "ON " + tblName + "	REFERENCING OLD AS OLD NEW AS NEW" + System.Environment.NewLine
//				+ "	FOR EACH ROW" + System.Environment.NewLine
//				+ "BEGIN" + System.Environment.NewLine
//				);
//			sbIn.Append(string.Format(insP, ":NEW", "I", tblKey, "NULL", ":NEW." + tblKey, "NULL", "NULL", "NULL", "NULL"));
//			sbDl.Append(string.Format(insP, ":OLD", "D", tblKey, "NULL", "NULL", "NULL", "NULL", ":OLD." + tblKey, "NULL"));

//			VSXElement[] cols = null;
//			cols = query.NativeColumns().OfType<VColumn>().Where(c => VColumn.IsTableColumn(c) && (c.P_Name != tblKey)).ToArray();
//			foreach (VColumn col in cols)
//			{
//				switch (col.P_DataType)
//				{
//					case TextConst.AVDataType.String:
//						sbIn.Append(string.Format(whenP, string.Format(insCondP, col.P_Name), string.Format(insP, ":NEW", "I", col.P_Name, ":NEW." + col.P_Name, "NULL", "NULL", "NULL", "NULL", "NULL")));
//						sbUp.Append(string.Format(whenP, string.Format(updCondP, col.P_Name), string.Format(insP, ":OLD", "U", col.P_Name, ":NEW." + col.P_Name, "NULL", "NULL", ":OLD." + col.P_Name, "NULL", "NULL")));
//						sbDl.Append(string.Format(whenP, string.Format(delCondP, col.P_Name), string.Format(insP, ":OLD", "D", col.P_Name, "NULL", "NULL", "NULL", ":OLD." + col.P_Name, "NULL", "NULL")));
//						break;
//					case TextConst.AVDataType.Number:
//						sbIn.Append(string.Format(whenP, string.Format(insCondP, col.P_Name), string.Format(insP, ":NEW", "I", col.P_Name, "NULL", ":NEW." + col.P_Name, "NULL", "NULL", "NULL", "NULL")));
//						sbUp.Append(string.Format(whenP, string.Format(updCondP, col.P_Name), string.Format(insP, ":OLD", "U", col.P_Name, "NULL", ":NEW." + col.P_Name, "NULL", "NULL", ":OLD." + col.P_Name, "NULL")));
//						sbDl.Append(string.Format(whenP, string.Format(delCondP, col.P_Name), string.Format(insP, ":OLD", "D", col.P_Name, "NULL", "NULL", "NULL", "NULL", ":OLD." + col.P_Name, "NULL")));
//						break;
//					case TextConst.AVDataType.Date:
//						sbIn.Append(string.Format(whenP, string.Format(insCondP, col.P_Name), string.Format(insP, ":NEW", "I", col.P_Name, "NULL", "NULL", ":NEW." + col.P_Name, "NULL", "NULL", "NULL")));
//						sbUp.Append(string.Format(whenP, string.Format(updCondP, col.P_Name), string.Format(insP, ":OLD", "U", col.P_Name, "NULL", "NULL", ":NEW." + col.P_Name, "NULL", "NULL", ":OLD." + col.P_Name)));
//						sbDl.Append(string.Format(whenP, string.Format(delCondP, col.P_Name), string.Format(insP, ":OLD", "D", col.P_Name, "NULL", "NULL", "NULL", "NULL", "NULL", ":OLD." + col.P_Name)));
//						break;
//					case TextConst.AVDataType.Clob:
//					case TextConst.AVDataType.Blob:
//						// cUpdFastP instead cUpdP
//						sbIn.Append(string.Format(whenP, string.Format(insCondP, col.P_Name), string.Format(insP, ":NEW", "I", col.P_Name, "NULL", "NULL", "NULL", "NULL", "NULL", "NULL")));
//						sbUp.Append(string.Format(whenP, string.Format(updCondFastP, col.P_Name), string.Format(insP, ":OLD", "U", col.P_Name, "NULL", "NULL", "NULL", "NULL", "NULL", "NULL")));
//						sbDl.Append(string.Format(whenP, string.Format(delCondP, col.P_Name), string.Format(insP, ":OLD", "D", col.P_Name, "NULL", "NULL", "NULL", "NULL", "NULL", "NULL")));
//						break;
//					//default:
//					//	sbIn.Append(string.Format(whenP, string.Format(insCondP, col.P_Name), string.Format(insP, ":NEW", "I", col.P_Name, "NULL", "NULL", "NULL", "NULL", "NULL", "NULL")));
//					//	sbUp.Append(string.Format(whenP, string.Format(updCondFastP, col.P_Name), string.Format(insP, ":OLD", "U", col.P_Name, "NULL", "NULL", "NULL", "NULL", "NULL", "NULL")));
//					//	sbDl.Append(string.Format(whenP, string.Format(delCondP, col.P_Name), string.Format(insP, ":OLD", "D", col.P_Name, "NULL", "NULL", "NULL", "NULL", "NULL", "NULL")));
//					//	break;
//				}
//			}

//			sbTr.Append(string.Format(caseP, sbIn.ToString(), sbUp.ToString(), sbDl.ToString()));

//			sbTr.Append(
//				"END;" + System.Environment.NewLine
//				+ "/");

//			return sbTr.ToString();
//		}
//    }
//}
