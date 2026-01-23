//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Xml.Linq;

//using sql.builder.Controls;
//using sql.builder.DataApi;
//using sql.builder.FieldInfo;
//using sql.builder.Controls.FormFields;
//using sql.builder.Controls.Grids;
//using sql.builder.XmlHelpers;
//using sql.builder.Exceptions;
//using sql.builder;
//using sql.builder.UI;
//using sql.builder.DashboardUtils;

//namespace sql.builder.VForms
//{
//	class VFrmRegExtract
//	{
//		UIFormC _form = null;
//		public VFrmRegExtract()
//		{
//			UIStatic.LoadProject("system");
//			_form = UIStatic.CreateSystemForm("sys_extract_reg", true);

//			_form.controls["p_proj"].PrepareListSource();
//			_form.controls["p_proj"].DataTableList.CustomFill += DataTableList_CustomFill;
//			_form.DataSource.GetTable("a").CustomFill += VFrmRegExtract_CustomFill;
//			_form.DataSource.GetTable(null).MyColumnChanged += VFrmRegExtract_MyColumnChanged;
//		}

//		void VFrmRegExtract_DataSourceSelectionChanged()
//		{
//			throw new NotImplementedException();
//		}
//		internal string curProj = "";
//		internal string newName = "";

//		void VFrmRegExtract_MyColumnChanged(object sender, DataColumnChangeEventArgs e)
//		{
//			if (e.Column.ColumnName == "p_proj")
//			{
//				curProj = (string)_form.GetParamField("p_proj").GetValue();
//				_form.DataSource.GetTable("a").Refresh();
//			}
//			if (e.Column.ColumnName == "p_name")
//			{
//				_form.GetParamField("p_replace").SetValue(0m);
//				var name = (string)_form.GetParamField("p_name").GetValue();
//				decimal isReplace = 0m;

//				if (name != "")
//				{
//					if (_form.DataSource.GetTable("a").Rows.Find(name) != null)
//					{
//						isReplace = 1m;
//					}
//				}
//				_form.GetParamField("p_need_replace").SetValue(isReplace);
//				newName = name;
//			}

//		}



//		void DataTableList_CustomFill(VDataTable table)
//		{
//			string[] projectsList = DataExtractHelper.GetProjectList();
//            for (int i = 0; i < projectsList.Length; i++) {
//				var row = table.NewRow();
//				row["id"] = projectsList[i];
//				row["name"] = projectsList[i];
//				table.Rows.Add(row);
//			}
//		}

//		void VFrmRegExtract_CustomFill(VDataTable table)
//		{
//			if (curProj != "") {
//				string[] dataExtractList = DataExtractHelper.GetDataExtractNameListByProject(curProj);
//				var row = table.NewRow();
//				row["id"] = "";// фики=тивная строка чтобы по умолчаню было пустое имя
//				row["name"] = row["id"];
//				table.Rows.Add(row);
//                for (int i = 0; i < dataExtractList.Length; i++) {
//					row = table.NewRow();
//					row["id"] = dataExtractList[i];
//					row["name"] = dataExtractList[i];
//					table.Rows.Add(row);
//				}
//			}
//		}

//		public System.Windows.Forms.DialogResult Show()
//		{
//			_form.LoadData(null);
//			// _form.RefreshData();
//			_form.GetParamField("p_need_replace").SetValue(0m);
//			var v = _form.ShowDialog();

//			return v;
//		}

//		//internal string getNewName()
//		//{
//		//	return (string)_form.GetParamField("p_name").GetValue();
//		//}
//	}
//}
