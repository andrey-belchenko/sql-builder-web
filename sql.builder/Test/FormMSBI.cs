//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Data;
//using System.Drawing;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using System.Windows.Forms;
//using sql.builder.WinForms;
////using DevExpress.XtraEditors.Controls;
////using DevExpress.XtraEditors;
//using sql.builder.UI;
//using System.Xml.Linq;
//using System.Xml;
//using sql.builder.DataApi;
//using sql.builder.Properties;
//using System.Diagnostics;

//namespace sql.builder.Test
//{
//	internal partial class FormMSBI : Form
//	{
//		private string msbiUserName = "MSBI_TEST";
//		//private string viewsGenerated = "";


		
//		public FormMSBI()
//		{
//			InitializeComponent();
//			fileNameEdit.EditValue = Settings.Default.testQueryS2;
//			viewsListEdit.EditValue = Cmn.ReadStringFromRegistry("all","msbi_modifications");
//			if (viewsListEdit.EditValue == null)
//			{
//				viewsListEdit.EditValue = "";
//			}
			
//			CreateRadioEditors();
			
//		}

//		private void CreateRadioEditors()
//		{
//			this.SuspendLayout();
//			radioBtnDB.Properties.Items.Add(new RadioGroupItem(0, "Test DB"));
//			radioBtnDB.Properties.Items.Add(new RadioGroupItem(1, "Production DB"));
//			radioBtnDB.SelectedIndex = 0;
//			this.ResumeLayout();
//		}

//		#region actions
//		private void radioBtnDB_SelectedIndexChanged(object sender, EventArgs e)
//		{
//			RadioGroup edit = sender as RadioGroup;
//			if (edit.SelectedIndex == 0)
//			{
//				msbiUserName = "MSBI_TEST";
//			}

//			else
//			{
//				msbiUserName = "MSBI";
//			}
//		}


//		private void btnShowDescription_Click(object sender, EventArgs e)
//		{
//            //if (viewsGenerated != msbiUserName)
//            //{
//            //    //msbiViews(viewsListEdit.Text, true);
//            //    viewsGenerated = msbiUserName;
//            //}
//			msbiMakeDoc(viewsListEdit.Text, "msbi_info_for_fts");
//		}


//		private void btnShowAllViewsDDL_Click(object sender, EventArgs e)
//		{
//			msbiViews(viewsListEdit.Text, true);
//			//viewsGenerated = msbiUserName;
//		}

//		private void btnShowAllDescription_Click(object sender, EventArgs e)
//		{
//			//if (viewsGenerated != msbiUserName)
//			//{
//			//	msbiViews(viewsListEdit.Text, true);
//			//	viewsGenerated = msbiUserName;
//			//}
//			msbiMakeDoc(viewsListEdit.Text, "msbi_info_for_fts", true);
//		}

//		private void fileNameEdit_EditValueChanged(object sender, EventArgs e)
//		{
//			Settings.Default.testQueryS2 = fileNameEdit.EditValue.ToString();
//			Settings.Default.Save();
//		}

//		private void viewsListEdit_EditValueChanged(object sender, EventArgs e)
//		{
//			Cmn.WriteStringToRegistry("all", "msbi_modifications", viewsListEdit.Text);
//		}

//		private void btnShowViewsDDL_Click(object sender, EventArgs e)
//		{
//			msbiViews(viewsListEdit.Text);
//		}

//		#endregion actions

//		private void msbiViews(string repInfoStr, bool getAll = false)
//		{
//			// UIStatic.LoadProject("ipr");
//			UIStatic.LoadProject("msbi");
//			XmlReports.Environment.Manager.GetNativeScheme().Descendants().Attributes(TextConst.AName.Materialize).Remove();
//			XmlReports.Environment.Manager.GetScheme().Descendants().Attributes(TextConst.AName.Materialize).Remove();
//			//var msbiQueries = XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => Cmn.GetAttrValue(q, TextConst.AName.Name).StartsWith("msbi_")).ToList();
//			List<string> repInfo = null;
//			repInfo = repInfoStr.Split(',').ToList();
//			var msbiQueries = getMsbiQueries(repInfo, getAll);
//			var sql = new StringBuilder();
//			var sqlRights = new StringBuilder();

//			List<string> tables = new List<string>();
//			foreach (VQuery query1 in msbiQueries)
//			{

//				//query1.GetElementsP( !!!!
//				var query = new XElement(query1);
//				sql.AppendLine("create or replace view");
//				sql.AppendLine(msbiUserName + "." + query1.P_Name.Replace("msbi_", "msbi_"));
//				sql.AppendLine("as");

//				XElement compiledQuery = Compiler.GetCompiledAndProcessedQuery(query);

//				foreach (string tbl in compiledQuery.Descendants(TextConst.EName.Table).Attributes(TextConst.AName.Name).Select(a => a.Value).Distinct().ToList())
//				{
//					if (!tables.Contains(tbl))
//					{
//						tables.Add(tbl);
//						sqlRights.AppendLine("grant select on " + tbl + " to " + msbiUserName + "  WITH GRANT OPTION");
//						sqlRights.AppendLine("/");
//					}
//				}


//				string selectText = Compiler.GetQuerySelectStatmentFromCompiledQuery(compiledQuery);
//				sql.AppendLine(selectText);
//				sql.AppendLine("/");
//				sql.AppendLine("");
//				sql.AppendLine("grant select on " + query1.P_Name + " to public");
//				sql.AppendLine("/");
//				sql.AppendLine("");
//				sql.AppendLine("");
//				sql.AppendLine("");
//			}
//			sqlRights.AppendLine("connect " + msbiUserName + "/kontek@len-oradb.energo.ru:1521/ALPHA");
//			sqlRights.AppendLine();
//            Cmn.SaveText(sqlRights.ToString() + sql.ToString(), fileNameEdit.EditValue.ToString(), Encoding.Unicode);
//			Process.Start(fileNameEdit.EditValue.ToString());
//		}

//		private void msbiMakeDoc(string repInfoStr, string templateName, bool getAll = false)
//		{

//            UIStatic.LoadProject("msbi");
//			if (string.IsNullOrEmpty(msbiUserName)) msbiUserName = "MSBI_TEST";

//			var ds = new DataSet();

//			var tblInfo = new DataTable("tbl");
//			tblInfo.Columns.Add("name");
//			tblInfo.Columns.Add("title");
//			tblInfo.Columns.Add("key");
//			tblInfo.Columns.Add("treb_info");
//			tblInfo.Columns.Add("action_info");

//			var columnInfo = new DataTable("col");
//			columnInfo.Columns.Add("table");
//			columnInfo.Columns.Add("name");
//			columnInfo.Columns.Add("title");
//			//columnInfo.Columns.Add("comment");
//			columnInfo.Columns.Add("data_type");
//			columnInfo.Columns.Add("len", XmlReports.numberType);
//			columnInfo.Columns.Add("ref");
//			columnInfo.Columns.Add("treb_info");
//			ds.Tables.Add(tblInfo);
//			ds.Tables.Add(columnInfo);

//			columnInfo.ParentRelations.Add(new DataRelation("", tblInfo.Columns["name"], columnInfo.Columns["table"]));
//			var msbiQueriesAll = XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => Cmn.GetAttrValue(q, TextConst.AName.Name).StartsWith("msbi_")).Select(q1 => (VQuery)q1).OrderBy(q => q.P_Title).ToList();
//			List<string> repInfo = null;
//			repInfo = repInfoStr.Split(',').ToList();
//			var msbiQueries = getMsbiQueries(repInfo, getAll);
//			var inModNums = new List<int>();

//			foreach (string s in repInfo)
//			{
//				Int32 modNum;
//				Int32.TryParse(s.Replace("m", ""), out modNum);
//				//var modNum = Convert.ToInt32(s.Replace("m", ""));
//				if (modNum != null)
//				{
//					inModNums.Add(modNum);
//				}
//			}
//			/*if (repInfoStr != null && !getAll)
//			{

//				msbiQueries = msbiQueries.Where(
//					 e =>
//						  Cmn.GetAttrValue(e, TextConst.AName.Comment).Split(',').Where(s => repInfo.Contains(s.Split('.')[0])).Any()
//						 ||
//						 e.Elements(TextConst.EName.Select).Elements().Where(
//						 e1 => Cmn.GetAttrValue(e1, TextConst.AName.Comment).Split(',').Where(s => repInfo.Contains(s.Split('.')[0])).Any()

//						 ).Any()
//					 ).ToList();
//			}*/

//			var sql = new StringBuilder();
//			var colsTbl = db.ExecuteDataTable(
//			"select table_name,column_name,DATA_TYPE,DATA_LENGTH from all_tab_columns where table_name like 'MSBI_%' and owner='" + msbiUserName + "'");

//			foreach (VQuery query in msbiQueries)
//			{
//				var keyCol = query.Columns().FirstOrDefault(c => Cmn.GetAttrValue(c, TextConst.AName.Key) == "1");

//				var keyName = "";
//				if (keyCol != null)
//				{
//					keyName = (keyCol as VSXElement).XName;
//				}



//				var cols = query.Columns();
//				var r1 = tblInfo.Rows.Add(query.P_Name.Replace("msbi_", "msbi_"), query.P_Title.Split('.')[1], keyName);
//				string tblTrebInfo = "";

//				if (repInfo != null && !getAll)
//				{
//					cols = cols.Where(
//							 e1 => Cmn.GetAttrValue(query, TextConst.AName.Comment).Split(',').Where(s => repInfo.Contains(s.Split('.')[0])).Any() ||
//								   Cmn.GetAttrValue(e1, TextConst.AName.Comment).Split(',').Where(s => repInfo.Contains(s.Split('.')[0])).Any()
//							 ).ToList();
//				}
//				bool isExtended = false;
//				foreach (VSXElement col in cols)
//				{
//					object linkName = DBNull.Value;
//                    string title = col.P_Title.SubstringAfter('.');

//					if (col.P_Key != "1")
//					{
//						linkName = col.P_CalledQuery;
//						VQuery linkQuery = null;
//						if (linkName == "")
//						{
//							linkQuery = msbiQueriesAll.Where(q => q.Columns().Where(c => c.P_Key == "1" && c.XName == col.XName).Any()).FirstOrDefault();

//						}
//						else
//						{
//							linkQuery = msbiQueriesAll.Where(q => q.P_Name == linkName.ToString()).First();

//						}
//						if (linkQuery != null)
//						{
//							linkName = linkQuery.XName.Replace("msbi_", "msbi_");
//							if (title == "")
//							{
//								title = linkQuery.P_Title.Split('.')[1];
//							}
//						}
//					}
//					else
//					{

//						if (title == "")
//						{
//							title = "ИД";
//						}


//					}

//					var row = colsTbl.Rows.Cast<DataRow>().Where(r => r["TABLE_NAME"].ToString() == query.P_Name.ToUpper() && r["COLUMN_NAME"].ToString() == col.XName.ToUpper()).First();

//					var r2 = columnInfo.Rows.Add(query.P_Name.Replace("msbi_", "msbi_"), col.XName, title, row["DATA_TYPE"], row["DATA_LENGTH"], linkName);
//					if (repInfo != null)
//					{

//						var modArr = col.P_Comment.Split(',').Select(s1 => s1.Split('.')[0]).Distinct().ToList();
//						bool isOld = false;
//						foreach (string s in modArr)
//						{
//							var sm = s.Split('.')[0];
//							var im = 0;
//							if (sm.Contains("m"))
//							{
//								sm = sm.Replace("m", "");
//								im = Convert.ToInt32(sm);
//							}
//							if (!inModNums.Where(i => i <= im).Any())// модификация старше всех запрошенных
//							{
//								isExtended = false;

//								break;
//							}
//							if (inModNums.Contains(im))
//							{
//								isExtended = true;
//							}
//						}


//                        var sitr_ar = col.P_Comment.Split(',').Where(s => repInfo.Contains(s.Split('.')[0]) && s.Split('.').Length > 1).Select(s1 => "(" + s1.Split('.')[1] + ")").Distinct().ToList();

//						sitr_ar.Sort();
//						var sitr =
//						   string.Join("", sitr_ar

//					   );
//						if (tblTrebInfo.IndexOf(sitr) == -1)
//						{
//							tblTrebInfo += sitr;
//						}

//						r2["treb_info"] = sitr;
//					}

//				}

//				if (repInfoStr != null)
//				{
//					var tblTrebInfoArr = query.P_Comment.Split(',').Where(s => repInfo.Contains(s.Split('.')[0])).ToArray();

//					var tblTrebInfo1 = string.Join(",",
//                       tblTrebInfoArr.Where(s => s.Split('.').Length > 1).Select(s1 => "(" + s1.Split('.')[1] + ")").Distinct().ToArray()
//					   );
//					if (tblTrebInfo.IndexOf(tblTrebInfo1) == -1)
//					{
//						tblTrebInfo += tblTrebInfo1;
//					}
//                    if (tblTrebInfoArr.Length != 0 && isExtended)
//					{
//						r1["action_info"] = "Создать представление";
//					}
//					else if (isExtended)
//					{
//						r1["action_info"] = "Расширить представление";
//					}
//					else
//					{
//						// нет колонок для расширения
//						r1["action_info"] = "Представление";
//					}



//				}

//                var trList = tblTrebInfo.Replace(" ", "").Replace("(", "").Split(')').Distinct().Where(v1=>v1!="").Select(v=>Convert.ToInt32(v)).ToList();
//                trList.Sort();
//                string[] trListS = trList.SelectAsArray(v => "(" + v.ToString() + ")");
//                tblTrebInfo = string.Join(string.Empty, trListS);
//				r1["treb_info"] = tblTrebInfo;
//			}

//			//string fullPath = Printing.Print(ds, "msbi_info.xml", "Описание данных");
//			string fullPath = Printing.PrintWord(ds, templateName + ".docx", "Описание данных");

//			Cmn.OpenPrintedFile(fullPath);
//		}

//		private List<VQuery> getMsbiQueries(List<string> repInfo, Boolean getAll)
//		{
//			var msbiQueries = XmlReports.Environment.GetElements(TextConst.EName.Queries).Where(q => Cmn.GetAttrValue(q, TextConst.AName.Name).StartsWith("msbi_")).Select(q1 => (VQuery)q1).OrderBy(q => q.P_Title).ToList();
//			if (repInfo.First() != "" && !getAll)
//			{

//				msbiQueries = msbiQueries.Where(
//					 e =>
//						  Cmn.GetAttrValue(e, TextConst.AName.Comment).Split(',').Where(s => repInfo.Contains(s.Split('.')[0])).Any()
//						 ||
//						 e.Elements(TextConst.EName.Select).Elements().Where(
//						 e1 => Cmn.GetAttrValue(e1, TextConst.AName.Comment).Split(',').Where(s => repInfo.Contains(s.Split('.')[0])).Any()

//						 ).Any()
//					 ).ToList();
//			}
		
//			return msbiQueries;
//		}


//	}
//}
