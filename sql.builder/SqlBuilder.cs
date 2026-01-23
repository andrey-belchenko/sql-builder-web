//using System;
//using System.Collections.Generic;
//using System.Data;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using System.IO;
//using Devart.Data.Oracle;
//using infoenergo.core;
//using DataHelper = infoenergo.core.Data.DataHelper;
//using infoenergo.sys; // Global
//using sql.builder.DataApi;
//using sql.builder.UI.WinForms; // UIFormControl
//using sql.builder.WinForms;
//using sql.builder.XmlHelpers;
//using sql.builder.UI;
//using infoenergo.framework.Extensions.Oracle;
//using sql.builder.WebReports;

//namespace sql.builder
//{
//    /// <summary>
//    /// Методы выгрузки отчетов
//    /// </summary>
//    public static class SqlBuilder
//    {
//        /// <summary>
//        /// Показывает превью отчета: в экспресс-форме или в полноэкранное форме
//        /// </summary>
//        /// <param name="report_name">Имя отчета</param>
//        /// <param name="express_mode">True - экспресс-форма; false - полноэкранная форма</param>
//        /// <param name="auto_execute">True - отчет будет выполнен автоматически; false - будет показано только превью без выполнения отчета</param>
//        public static void PreviewReport(string report_name, bool express_mode = true, bool auto_execute = false)
//        {
//            if (String.IsNullOrEmpty(report_name))
//            {
//                throw new ArgumentNullException("report_name");
//            }
//            //временный костыль.нет прав на кидо
//            // Емцов - изменил вызов в кидо
//            //if (report_name == "ipr.41656")
//            //{
//            //    express_mode = false;
//            //}
//            if (express_mode)
//            {
//                frmExpressReport.ShowForm(report_name, auto_execute);
//            }
//            else
//            {
//                LoadProjectForReport(report_name);
//                frmMain.ShowForm();
//            }
//        }
//        /// <summary>
//        /// Показывает экспресс форму отчета с заданными параметрами
//        /// </summary>
//        /// <param name="repName"></param>
//        /// <param name="param">Словарь параметров</param>
//        public static void PreviewReport(string repName, Dictionary<string, object> param)
//        {
//            var rep = new ExpressReport();
//            rep.Initialize(repName);
//            foreach (var p in param)
//            {
//                rep.GetParamField(p.Key).SetValue(p.Value);
//            }
//            rep.ShowPreview();
//        }
//        /// <summary>
//        /// Загружает проект, в котором находится отчет
//        /// </summary>
//        /// <param name="report_name">Имя отчета</param>
//        internal static void LoadProjectForReport(string report_name)
//        {
//            string project;
//            int pos = report_name.LastIndexOf('.');
//            if (pos >= 0)
//            {
//                project = report_name.Substring(0, pos);
//                report_name = report_name.Substring(pos + 1);
//            }
//            else
//            {
//                project = frmExpressReport.GetProjectNameFromNavigator(report_name);
//            }
//            XmlReports.Environment.Manager.LoadProjectIfNeed(project);
//            XmlReports.SetInputParameter("report", report_name);
//        }
//        /// <summary>
//        /// Показывает полноэкранное превью со списком отчетов из указанной папки навигатора
//        /// </summary>
//        /// <param name="folder_name">Имя папки навигатора</param>
//        /// <param name="mdiParent">MDI контейнер</param>
//        public static void PreviewReports(string folder_name = null, Form mdiParent = null)
//        {
//            if (folder_name != null)
//            {
//                XmlReports.SetInputParameter("folder", folder_name.Replace("folder:", ""));
//            }
//            frmMain.ShowForm(mdiParent);
//        }
//        /// <summary>
//        /// Преобразует значение типа string в значение типа decimal
//        /// </summary>
//        /// <param name="str">Значение переменной типа string</param>
//        /// <returns>Значение переменной типа decimal</returns>
//        public static object ToDecimal(string str)
//        {
//            if (string.Empty.Equals(str))
//            {
//                return Cmn.DECIMAL_ZERO;
//            }
//            else
//            {
//                return Cmn.ToDecimal(str);
//            }
//        }
//        /// <summary>
//        /// Выполняет sql-запрос с возвратом скалярного значения (первая колонка, первая строка)
//        /// </summary>
//        /// <param name="sql">Текст sql-запроса</param>
//        /// <param name="pars">Параметры запроса</param>
//        /// <returns>Скалярный результат запроса</returns>
//        public static object ExecuteScalar(string sql, Dictionary<string, object> pars = null)
//        {
//            return db.ExecuteObject(sql, null, pars);
//        }
//        /// <summary>
//        /// Возвращает результат отчета в виде DataSet
//        /// </summary>
//        /// <param name="projectName">Имя проекта</param>
//        /// <param name="reportName">Имя отчета</param>
//        /// <param name="pars">Параметры отчета</param>
//        /// <returns>DataSet отчета</returns>
//        internal static DataSet ExecuteReport_RetDS(string projectName, string reportName, object[] pars = null)
//        {
//            var rep = XmlReports.Environment.GetPrecompiledReport(reportName, projectName);
//            var ds = rep.Result(2, false);
//            ds.Refresh(pars);
//            return ds;
//        }
//        /*
//		/// <summary>
//        /// Возвращает результат отчета в виде DataSet
//		/// </summary>
//		/// <param name="projectName">Имя проекта</param>
//		/// <param name="reportName">Имя отчета</param>
//        /// <param name="pars">Параметры отчета</param>
//        /// <returns>DataSet отчета</returns>
//		internal static DataSet ExecuteReport_RetDS(string projectName, string reportName, Dictionary<string,object> pars)
//		{
//			var rep = XmlReports.Environment.GetPrecompiledReport(reportName, projectName);
//			var ds = rep.Result(2, false);
//			//pars dictionary to xml pars
//			var xpars = parsDictionaryToParsXelement(pars, ds.Report.Element("params"));
//			ds.Refresh(xpars);
//			return ds;
//		}
//        */
//        // Похожий для просто массива есть в VDataSet.ParsObjectArrayToXelement , но не уверен, что это должно быть там.
//        private static XElement parsDictionaryToParsXelement(Dictionary<string, object> pars, XElement xformalParams)
//        {
//            if (xformalParams == null) return null;
//            XElement xpars = new XElement(xformalParams);
//            xpars.Elements().Remove();

//            foreach (KeyValuePair<string, object> par in pars)
//            {
//                //XElement xpar = new XElement(TextConst.EName.Param, new XAttribute(TextConst.AName.Name, "\"" + par.Key + "\""));
//                XElement xpar = new XElement(TextConst.EName.Param, new XAttribute(TextConst.AName.Name, par.Key));
//                if (par.Value is object[])
//                {
//                    XElement xarr = new XElement(TextConst.EName.Call, new XAttribute(TextConst.AName.Function, TextConst.AVFunction.Array));
//                    foreach (var objArrVal in (par.Value as object[]))
//                    {
//                        xarr.Add(new XElement(TextConst.EName.Const, new XText((string)objArrVal)));
//                    }
//                    xpar.Add(xarr);
//                }
//                else
//                {
//                    xpar.Add(new XElement(TextConst.EName.Const, new XText((string)par.Value)));
//                }

//                xpars.Add(xpar);
//            }
//            return xpars;
//        }
//        /// <summary>
//        /// Возвращает результат отчета в виде DataTable
//        /// </summary>
//        /// <param name="projectName">Имя проекта</param>
//        /// <param name="reportName">Имя отчета</param>
//        /// <param name="pars">Параметры отчета</param>
//        /// <returns>DataTable отчета</returns>
//        public static DataTable ExecuteReport_RetTable(string projectName, string reportName, object[] pars = null)
//        {
//            DataSet ds = ExecuteReport_RetDS(projectName, reportName, pars);
//            return ds.Tables[0];
//        }
//        /// <summary>
//        /// Возвращает скалярный результат отчета в виде object
//        /// </summary>
//        /// <param name="projectName">Имя проекта</param>
//        /// <param name="reportName">Имя отчета</param>
//        /// <param name="pars">Параметры отчета</param>
//        /// <returns>Скалярный результат отчета: первая таблица, первая строка, первая колонка</returns>
//        public static object ExecuteReport_RetObject(string projectName, string reportName, object[] pars = null)
//        {
//            DataSet ds = ExecuteReport_RetDS(projectName, reportName, pars);
//            object val;
//            if (ds.Tables[0].Rows.Count > 0)
//            {
//                val = ds.Tables[0].Rows[0][0];
//            }
//            else
//            {
//                val = null;
//            }
//            return Cmn.Nvle(val, null);
//        }
//        /// <summary>
//        /// Возвращает путь рабочей директории для сохранения отчетов
//        /// </summary>
//        /// <returns>Путь рабочей директории для сохранения отчетов</returns>
//        internal static string GetWorkFolderPath()
//        {
//            var fold1 = SettingsHelper.WorkFolder;
//            // убрать со временем
//            var fold2 = Path.GetTempPath(); //Settings.Default.workFolder;
//            if (String.IsNullOrEmpty(fold1) && !String.IsNullOrEmpty(fold2))
//            {
//                SettingsHelper.WorkFolder = fold2;
//                return fold2;
//            }
//            return fold1;
//        }
//        //public static DataSet GetDataSet(string report_name, IEnumerable<string> columns, IEnumerable<decimal> codes)
//        //{
//        //    var pars = new XElement("params",
//        //                    new XElement("param",
//        //                        new XAttribute("name", "kod_mat_pp"),
//        //                        new XElement("call",
//        //                            new XAttribute("function", "array"),
//        //                            codes.Select(c => new XElement("const", c)))));
//        //    var ds = XmlReports.Environment.GetPrecompiledReport(report_name).Result(2, false);
//        //    ds.SchemePreset.Element("table")
//        //         .Element("viewcolumns")
//        //         .Descendants("column")
//        //         .Where(c => !columns.Contains(c.Attribute("name").Value))
//        //         .Remove();
//        //    ds = ds.Report.Result(pars, 2, null, true, ds.SchemePreset);
//        //    ds.Refresh(2);
//        //    return ds;
//        //}
//        //public static decimal TestReflection()
//        //{
//        //    MessageBox.Show("успешно");
//        //    return 4M;
//        //}
//        /// <summary>
//        /// Изменяет подключение к Oracle БД
//        /// </summary>
//        /// <param name="con">Подключение</param>
//        /// <param name="source_folder">Директория-источник xml-файлов отчета для web-версии sql.builder</param>
//        public static void ChangeConnection(OracleConnection con, string source_folder = null)
//        {
//            db.Connection = con;
//            XmlReports.Init(source_folder: source_folder);
//        }

//        /// <summary>
//        /// Изменяет подключение к Oracle БД
//        /// </summary>
//        /// <param name="connectionString"></param>
//        public static void ChangeConnectionString(string connectionString)
//        {
//            var connection = new OracleConnection(connectionString);
//            connection.Open(useGlobalSettings: true);
//            ChangeConnection(connection);
//        }




//        /*/// <summary>
//        /// Формирует sql-запрос из xml объекта query
//        /// </summary>
//        /// <param name="con">Подключение</param>
//        /// <param name="query_name">Имя объекта query</param>
//        /// <returns>Sql-запрос</returns>
//        internal static string GetQuerySql(OracleConnection con, string query_name)
//        {
//            db.Connection = con;
//            var env = XmlReports.Environment;
//            return Compiler.GetSql(XmlReports.getItemProcessedXml2("query", query_name, false));
//        }
//        */
//        /// <summary>
//        /// Формирует DataTable объектов навигатора: отчеты, формы, папки
//        /// </summary>
//        /// <param name="root_folders">Имя папки, по объектам которой формируется DataTable</param>
//        /// <param name="with_templates">True - подгружать шаблоны настраиваемых отчетов; false - не подгружать шаблоны</param>
//        /// <param name="navName">Имя навигатора</param>
//        /// <returns>DataTable объектов навигатора: отчеты, формы, папки</returns>
//        public static DataTable GetReportsDataTable(string[] root_folders = null, bool with_templates = false, string navName = null)
//        {
//            XElement xnavigator;
//            if (navName != null)
//            {
//                xnavigator = XmlReports.GetNavigator(navName, false);
//            }
//            else
//            {
//                xnavigator = XmlReports.GetCurrentNavigator(false);
//            }
//            DataTable dt = new DataTable();
//            DataColumn col_name = dt.Columns.Add("name", typeof(string));
//            col_name.Caption = "Идентификатор";
//            DataColumn col_title = dt.Columns.Add("title", typeof(string));
//            col_title.Caption = "Наименование";
//            DataColumn col_item_type = dt.Columns.Add("item_type", typeof(string));
//            col_item_type.Caption = "Тип элемента";
//            DataColumn col_parent = dt.Columns.Add("parent", typeof(string));
//            col_parent.Caption = "Родительский узел";
//            DataColumn col_visible = dt.Columns.Add("visible", typeof(bool));
//            col_visible.Caption = "Видимость";
//            DataColumn col_kod_menu = dt.Columns.Add("kod_menu", typeof(decimal));
//            col_kod_menu.Caption = "Код права";
//            DataColumn col_original_name = dt.Columns.Add("original_name", typeof(string));
//            DataColumn col_changed = dt.Columns.Add("changed", typeof(decimal));
//            col_changed.DefaultValue = Cmn.DECIMAL_ZERO;
//            DataColumn col_kod_gs = dt.Columns.Add("kod_gs", typeof(decimal));
//            DataColumn col_data = dt.Columns.Add("data", typeof(XElement));
//            DataColumn col_image_id = dt.Columns.Add("image_id", typeof(decimal));
//            DataColumn col_project = dt.Columns.Add("project", typeof(string));
//            DataColumn col_old = dt.Columns.Add("old", typeof(bool));
//            DataColumn col_has_access = dt.Columns.Add("has_access", typeof(bool));
//            dt.PrimaryKey = new DataColumn[1] { col_name };
//            DataRow row;
//            foreach (XElement xitem in xnavigator.Descendants())
//            {
//                bool visible = xitem.AttrOrDefault(AName.visible, true);
//                if (!XmlReports.IsDeveloperMode() && !visible)
//                { //  в релизе невидимые не нужны
//                    continue;
//                }
//                XName item_type = xitem.Name;
//                string name;
//                object image_id;
//                if (item_type == EName.folder)
//                {
//                    name = xitem.Attribute(AName.name).Value;
//                    image_id = Cmn.DECIMAL_ZERO;
//                }
//                else if (item_type == EName.usereport)
//                {
//                    name = xitem.Attribute(AName.report).Value;
//                    image_id = Cmn.DECIMAL_ONE;
//                }
//                else if (item_type == EName.useform)
//                {
//                    name = xitem.Attribute(AName.form).Value;
//                    image_id = Cmn.DECIMAL_TWO;
//                }
//                else
//                {
//                    name = string.Empty;
//                    image_id = Cmn.DECIMAL_MINUS_ONE;
//                }
//                if (!visible)
//                {
//                    image_id = Cmn.DECIMAL_FOUR;
//                }
//                row = dt.NewRow();
//                row[col_name] = name;
//                row[col_title] = xitem.AttrOrEmpty(AName.title);
//                row[col_item_type] = item_type.LocalName;
//                row[col_parent] = (xitem.Parent.Name == EName.folder) ? xitem.Parent.Attribute(AName.name).Value : string.Empty;
//                row[col_visible] = visible;
//                XAttribute attr = xitem.Attribute(TextConst.AName.KodMenu);
//                if (attr != null)
//                {
//                    decimal kod_menu = Convert.ToDecimal(attr.Value);
//                    row[col_kod_menu] = (object)kod_menu;
//                    row[col_has_access] = Security.HasPermission(Convert.ToDouble(kod_menu), Security.Permission.Read);
//                }
//                else
//                {
//                    row[col_kod_menu] = DBNull.Value;
//                    row[col_has_access] = Cmn.BOOLEAN_TRUE;
//                }
//                row[col_original_name] = name;
//                row[col_changed] = Cmn.DECIMAL_ZERO;
//                row[col_image_id] = image_id;
//                row[col_project] = xitem.AttrOrEmpty(AName.project);
//                row[col_old] = xitem.AttrOrDefault(TextConst.AName.Old, false);
//                dt.Rows.Add(row);
//            }

//            WebReportsAdapter.FillReportsDataTable(dt);
//            // если указана папка, относительно которой нужно вывести дерево с отчётами
//            // обрезаем отчёты и папки, которые не лежат в указанной папке
//            //CutReportsByFolders2(dt, root_folders);
//            int index;
//            DataRow parent_row;
//            if (!Array.IsNullOrEmpty(root_folders))
//            {
//                for (index = dt.Rows.Count - 1; index >= 0; index--)
//                {
//                    row = dt.Rows[index];
//                    // указанную корневую папку не удаляем
//                    if (root_folders.Contains(row[col_name].ToString()))
//                    {
//                        continue;
//                    }
//                    // проверяем лежит ли папка либо отчет в указанной папке
//                    // идем по дереву вверх и проверяем имя, пока не дойдем до корня, 
//                    // либо не найдем корневую папку
//                    parent_row = row;
//                    do
//                    {
//                        object parent = parent_row[col_parent];
//                        if (Cmn.IsNull(parent))
//                        {
//                            row.Delete();
//                            break;
//                        }
//                        if (root_folders.Contains(parent.ToString()))
//                        {
//                            break;
//                        }
//                        parent_row = dt.Rows.Find(parent);
//                    } while (parent_row != null);
//                }
//                dt.AcceptChanges();
//            }
//            // обрезаем элементы, на которые нет прав
//            //CutReportsByRights2(dt);
//            if (XmlReports.schemeName == "asuse2")
//            {
//                for (index = dt.Rows.Count - 1; index >= 0; index--)
//                {
//                    row = dt.Rows[index];
//                    parent_row = row;
//                    do
//                    {
//                        bool has_access = Convert.ToBoolean(row[col_has_access]);
//                        if (!has_access)
//                        {
//                            row.Delete();
//                            break;
//                        }
//                        object parent = parent_row[col_parent];
//                        if (Cmn.IsNull(parent))
//                        {
//                            parent_row = null;
//                        }
//                        else
//                        {
//                            parent_row = dt.Rows.Find(parent);
//                        }
//                    } while (parent_row != null);
//                }
//                dt.AcceptChanges();
//            }
//            // подгружаем шаблоны
//            if (with_templates)
//            {
//                /*
//                DataTable dt2 = db.SelectAllVisibleSettings();
//                var report_names = dt.AsEnumerable().Where(r => (string)r["item_type"] == TextConst.EName.UseReport).Select(r => (string)r["name"]).ToArray();
//                var rows = dt2.AsEnumerable().Where(r => !report_names.Contains((string)r["original_name"])).ToArray();
//                foreach (var r in rows) dt2.Rows.Remove(r);
//                dt.Merge(dt2);
//                */
//                DataTable dt2 = DataHelper.SqlGetTable("SELECT kod_gs, repname as original_name, name as title " +
//                                                       "FROM vr_grid_settings " +
//                                                       "WHERE visible = 1 " +
//                                                       "ORDER BY repname, kod_gs", Global.Connection);
//                DataColumn col2_original_name = dt2.Columns["original_name"];
//                DataColumn col2_title = dt2.Columns["title"];
//                DataColumn col2_kod_gs = dt2.Columns["kod_gs"];
//                for (index = 0; index < dt2.Rows.Count; index++)
//                {
//                    DataRow row2 = dt2.Rows[index];
//                    string original_name = row2.Field<string>(col2_original_name);
//                    DataRow report_row = dt.Rows.Find(original_name);
//                    if (report_row != null && report_row.Field<string>(col_item_type) == EName.usereport.LocalName && report_row.IsNull(col_kod_gs))
//                    {
//                        object kod_gs = row2[col2_kod_gs];
//                        string name = original_name + "_gs" + kod_gs.ToString();
//                        row = dt.NewRow();
//                        row[col_name] = name;
//                        row[col_title] = row2[col2_title];
//                        row[col_item_type] = TextConst.EName.UseTemplate;
//                        row[col_parent] = original_name;
//                        row[col_visible] = Cmn.BOOLEAN_TRUE;
//                        row[col_original_name] = original_name;
//                        row[col_changed] = Cmn.DECIMAL_ZERO;
//                        row[col_image_id] = Cmn.DECIMAL_THREE;
//                        row[col_kod_gs] = kod_gs;
//                        row[col_project] = report_row[col_project];
//                        row[col_old] = report_row[col_old];
//                        dt.Rows.Add(row);
//                    }
//                }
//                dt.AcceptChanges();
//            }
//            return dt;
//        }
//        /*private static void CutReportsByFolders2(DataTable dt, string[] root_folders)
//        {
//            if (Array.IsNullOrEmpty(root_folders)) {
//                return;
//            }
//            var rows_to_delete = new List<DataRow>();
//            // перебираем все папки и отчёты
//            int index;
//            for (index = 0; index < dt.Rows.Count; index++) {
//                DataRow rep_row = dt.Rows[index];
//                // указанную корневую папку не удаляем
//                if (root_folders.Contains(rep_row["name"].ToString())) {
//                    continue;
//                }
//                // флаг - лежит ли папка либо отчет в указанной папке
//                bool is_child = false;
//                // проверяем лежит ли папка либо отчет в указанной папке
//                // идем по дереву вверх и проверяем имя, пока не дойдем до корня, 
//                // либо не найдем корневую папку
//                DataRow parent_row = rep_row;
//                while (!Cmn.IsNull(parent_row["parent"])) {
//                    if (root_folders.Contains(parent_row["parent"].ToString())) {
//                        is_child = true;
//                        break;
//                    }
//                    parent_row = dt.Rows.Find(parent_row["parent"]);
//                }
//                // если папка либо отчёт не лежит в указанной папке - добавляем в список на удаление
//                if (!is_child) {
//                    rows_to_delete.Add(rep_row);
//                }
//            }
//            // удаляем все папки и отчёты, которые не лежат в указанной корневой папке
//            for (index = 0; index < rows_to_delete.Count; index++) {
//                rows_to_delete[index].Delete();
//            }
//            dt.AcceptChanges();
//        }*/
//        /*private static void CutReportsByRights2(DataTable dt)
//        {
//            //bool access_default = true;//Security.HasPermission((double)XmlReports.kod_menu_default, Security.Permission.Read);
//            var rows_to_delete = new List<DataRow>();
//            int index;
//            DataRow row;
//            for (index = 0; index < dt.Rows.Count; index++) {
//                row = dt.Rows[index];
//                if (Cmn.IsNullOrDBNull(row["kod_menu"])) {
//                    row["has_access"] = Cmn.BOOLEAN_TRUE;
//                } else {
//                    decimal kod_menu = (decimal)row["kod_menu"];
//                    row["has_access"] = Security.HasPermission(Convert.ToDouble(row["kod_menu"]), Security.Permission.Read);
//                }
//            }
//            for (index = 0; index < dt.Rows.Count; index++) {
//                row = dt.Rows[index];
//                bool has_access = true;
//                DataRow parent_row = row;
//                while (parent_row != null) {
//                    if (XmlReports.schemeName == "asuse2") {
//                        if (!(bool)parent_row["has_access"]) {
//                            has_access = false;
//                            break;
//                        }
//                    }
//                    if (Cmn.IsNull(parent_row["parent"])) {
//                        parent_row = null;
//                    } else {
//                        parent_row = dt.Rows.Find(parent_row["parent"]);
//                    }
//                }
//                if (!has_access) {
//                    rows_to_delete.Add(row);
//                }
//            }
//            for (index = 0; index < rows_to_delete.Count; index++) {
//                rows_to_delete[index].Delete();
//            }
//            dt.AcceptChanges();
//        }*/
//        /*internal static DataTable CreateSourceDataTable2()
//        {
//            var dt = new DataTable();
//            dt.Columns.AddRange(new[]
//            {
//                new DataColumn("name", typeof(string)) { Caption = "Идентификатор" }, 
//                new DataColumn("title", typeof(string)) { Caption = "Наименование" }, 
//                new DataColumn("item_type", typeof(string)) { Caption = "Тип элемента" },
//                new DataColumn("parent", typeof(string)) { Caption = "Родительский узел" }, 
//                new DataColumn("visible", typeof(bool)) { Caption = "Видимость" },
//                new DataColumn("kod_menu", typeof(decimal)) { Caption = "Код права" },
//                new DataColumn("original_name", typeof(string)),
//                new DataColumn("changed", typeof(decimal)){ DefaultValue = Cmn.DECIMAL_ZERO },
//                new DataColumn("kod_gs",typeof(decimal)),
//                new DataColumn("data", typeof(XElement)),
//                new DataColumn("image_id", typeof(decimal)),
//                new DataColumn("project", typeof(string)),
//                new DataColumn("old", typeof(bool))
//            });

//            dt.PrimaryKey = new[] { dt.Columns["name"] };

//            return dt;
//        }*/
//        //public static DataTable GetReportsDataTable(string[] root_folders = null, bool developer_mode = false)
//        //{
//        //    var env = XmlReports.Environment;
//        //    root_folders = root_folders ?? new string[] { };
//        //    // загружаем информацию об отчетах и папках
//        //    var tmp_dt1 = XmlReports.GetReportList(XmlReports.customerId, developer_mode);
//        //    // подгружаем информацию о настройках отчётов
//        //    DataTable tmp_dt2 = db.SelectAllVisibleSettings();
//        //    // модифицируем таблицу с отчетами, чтобы перенести туда строки с настройками
//        //    tmp_dt1.Columns.AddRange(new[]
//        //    {
//        //        new DataColumn() {ColumnName = "original_name",  DataType = typeof (string) },
//        //        new DataColumn() {ColumnName = "changed", DataType = typeof (decimal), DefaultValue = 0M },
//        //        new DataColumn() {ColumnName = "is_template", DataType = typeof (decimal), DefaultValue = 0M },
//        //        new DataColumn() {ColumnName = "kod_gs",  DataType = typeof (decimal) },
//        //        new DataColumn() {ColumnName = "data", DataType = typeof(string) },
//        //        new DataColumn() {ColumnName = "image_id", DataType = typeof (int)}
//        //    });
//        //    tmp_dt1.PrimaryKey = new[] { tmp_dt1.Columns["name"] };
//        //    // устанавливаем картинки
//        //    foreach (var row in tmp_dt1.AsEnumerable())
//        //    {
//        //        row["original_name"] = row["name"];
//        //        if (!row["visible"].Equals("0"))
//        //        {
//        //            switch ((string)row["item_type"])
//        //            {
//        //                case "folder": row["image_id"] = 0; break;
//        //                case "nogrid":
//        //                case "report": row["image_id"] = 1; break;
//        //                case "editable": row["image_id"] = 2; break;
//        //            }
//        //        }
//        //        else
//        //        {
//        //            row["image_id"] = 4;
//        //        }
//        //    }
//        //    var report_names = tmp_dt1.AsEnumerable().Select(row => (string)row["name"]);
//        //    // выбираем только те настройки, у которых есть отчет
//        //    var settings_with_reports = tmp_dt2.AsEnumerable().Where(row => report_names.Contains((string)row["folder"]));
//        //    // переносим строки с настройками в таблицу с отчетами
//        //    foreach (var row in settings_with_reports)
//        //    {
//        //        tmp_dt1.ImportRow(row);
//        //    }
//        //    tmp_dt1.AcceptChanges();
//        //    // если указана папка, относительно которой нужно вывести дерево с отчётами
//        //    // обрезаем отчёты и папки, которые не лежат в указанной папке
//        //    CutReportsByFolders(tmp_dt1, root_folders);
//        //    // обрезаем элементы, на которые нет прав
//        //    CutReportsByRights(tmp_dt1);
//        //    return tmp_dt1;
//        //}
//        //private static void CutReportsByFolders(DataTable dt, string[] root_folders)
//        //{
//        //    if (root_folders == null || root_folders.IsEmpty()) return;
//        //    var all_rows = dt.AsEnumerable().ToArray();
//        //    var rows_to_delete = new List<DataRow>();
//        //    // перебираем все папки и отчёты
//        //    foreach (var rep_row in all_rows)
//        //    {
//        //        // указанную корневую папку не удаляем
//        //        if (root_folders.Contains(rep_row["name"].ToString())) continue;
//        //        // флаг - лежит ли папка либо отчет в указанной папке
//        //        bool is_child = false;
//        //        // проверяем лежит ли папка либо отчет в указанной папке
//        //        // идем по дереву вверх и проверяем имя, пока не дойдем до корня, 
//        //        // либо не найдем корневую папку
//        //        var parent_row = rep_row;
//        //        while (parent_row["folder"] != DBNull.Value && (string)parent_row["folder"] != "")
//        //        {
//        //            if (root_folders.Contains(parent_row["folder"].ToString()))
//        //            {
//        //                is_child = true;
//        //                break;
//        //            }
//        //            parent_row = all_rows.First(r => (string)r["name"] == (string)parent_row["folder"]);
//        //        }
//        //        // если папка либо отчёт не лежит в указанной папке - добавляем в список на удаление
//        //        if (!is_child) rows_to_delete.Add(rep_row);
//        //    }
//        //    // удаляем все папки и отчёты, которые не лежат в указанной корневой папке
//        //    rows_to_delete.ForEach(row => row.Delete());
//        //    dt.AcceptChanges();
//        //}
//        //private static void CutReportsByRights(DataTable dt)
//        //{
//        //    var access_default = true;//Security.HasPermission((double)XmlReports.kod_menu_default, Security.Permission.Read);
//        //    var all_rows = dt.AsEnumerable().ToArray();
//        //    var rows_to_delete = new List<DataRow>();
//        //    if (!dt.Columns.Contains("has_access"))
//        //    {
//        //        dt.Columns.Add(new DataColumn("has_access", typeof(bool)));
//        //    }
//        //    foreach (var rep_row in all_rows)
//        //    {
//        //        double kod_menu = !Cmn.IsNull(rep_row["kod_menu"])
//        //            ? Convert.ToDouble(rep_row["kod_menu"])
//        //            : 0D;
//        //        rep_row["has_access"] = (kod_menu > 0D)
//        //            ? Security.HasPermission(kod_menu, Security.Permission.Read)
//        //            : access_default;
//        //    }
//        //    foreach (var rep_row in all_rows)
//        //    {
//        //        bool has_access = true;
//        //        var parent_row = rep_row;
//        //        while (parent_row != null)
//        //        {
//        //            if (XmlReports.schemeName == "asuse2")
//        //            {
//        //                if (!(bool)parent_row["has_access"])
//        //                {
//        //                    has_access = false;
//        //                    break;
//        //                }
//        //            }
//        //            parent_row = !Cmn.IsNull(parent_row["folder"])
//        //                   ? all_rows.FirstOrDefault(row => (string)row["name"] == (string)parent_row["folder"])
//        //                   : null;
//        //        }
//        //        if (!has_access) rows_to_delete.Add(rep_row);
//        //    }
//        //    rows_to_delete.ForEach(row => row.Delete());
//        //    dt.AcceptChanges();
//        //}
//        /// <summary>
//        /// True - показать форму ожидания; false - скрыть форму ожидания
//        /// </summary>
//        public static bool ShowPopupWaitForms = true;
//        /// <summary>
//        /// Выполняет отчет без формы с параметрами, открывает в новой книге Excel без сохранения
//        /// </summary>
//        /// <param name="repName">Полное имя отчета (проект.отчет)</param>
//        /// <param name="param">Параметры отчета</param>
//        public static void ExecReport(string repName, Dictionary<string, object> param)
//        {
//            //var rep = new ExpressReport
//            //{
//            //    OpenDocumentAfterPrint = false
//            //};
//            //rep.Initialize(repName);
//            //foreach (var p in param)
//            //{
//            //    rep.GetParamField(p.Key).SetValue(p.Value);
//            //}
//            //var path = "";
//            //rep.ReportOpening += (obj, sender) =>
//            //{
//            //    path = sender.Path;
//            //};
//            //rep.ExecuteReport();
//            string path = ExecReportGetPath(repName, param);
//            var app = new Microsoft.Office.Interop.Excel.Application();
//            var wb = app.Workbooks.Open(path);
//            wb.Sheets.Copy();
//            wb.Close();
//            app.Visible = true;
//            //Process.Start(path);
//            File.Delete(path);
//        }
//        /// <summary>
//        /// Выполняет отчет без формы с параметрами, возвращает путь к файлу отчета
//        /// </summary>
//        /// <param name="repName">Полное имя отчета (проект.отчет)</param>
//        /// <param name="param">Словарь параметров</param>
//        /// <returns>Путь к файлу отчета</returns>
//        public static string ExecReportGetPath(string repName, Dictionary<string, object> param)
//        {
//            var rep = new ExpressReport();
//            rep.OpenDocumentAfterPrint = false;
//            rep.Initialize(repName);
//            foreach (var p in param)
//            {
//                rep.GetParamField(p.Key).SetValue(p.Value);
//            }
//            string path = string.Empty;
//            rep.ReportOpening += (obj, sender) =>
//            {
//                path = sender.Path;
//            };
//            rep.ExecuteReport();
//            return path;
//        }
//        /// <summary>
//        /// Проверяет есть ли в данный момент открытая в режиме Dialog форма
//        /// </summary>
//        /// <returns>True - есть; false - нет</returns>
//        public static bool IsDialogOpen()
//        {
//            return UIFormControl.IsDialogOpen();
//        }
//    }
//    /// <summary>
//    /// Методы модификации данных (работы с формами)
//    /// </summary>
//    public static class DataEditor
//    {
//        internal static void ExecuteAction(string projectName, string actionName, object[] pars, frmDynamicEditor frm)
//        {
//            // временное решение
//            XmlReports.Environment.Manager.LoadProjectIfNeed(projectName);
//            VAction action = XmlReports.Environment.GetAction(actionName);
//            action.Execute(null, null, null, null, null, pars, frm);
//        }
//        /// <summary>
//        /// Выполнить действите (объект action в sql.builder)
//        /// </summary>
//        /// <param name="projectName">Имя проекта</param>
//        /// <param name="actionName">Имя объекта action</param>
//        /// <param name="pars">Параметры</param>
//        public static void ExecuteAction(string projectName, string actionName, object[] pars = null)
//        {
//            // временное решение
//            XmlReports.Environment.Manager.LoadProjectIfNeed(projectName);
//            VAction action = XmlReports.Environment.GetAction(actionName);
//            action.Execute(null, null, null, null, null, pars, null);
//            XmlReports.Environment.Connection.Commit();
//        }
//        /// <summary>
//        /// Открывает форму, настроенную в sql.builder
//        /// </summary>
//        /// <param name="projectName">Имя проекта</param>
//        /// <param name="formName">Имя формы</param>
//        /// <param name="isCreation">Открывается в режиме создания</param>
//        /// <param name="pars">Параметры формы</param>
//        /// <param name="isDialog">Модальное окно</param>
//        /// <param name="mdiParentForm"></param>
//        public static object OpenForm(string projectName, string formName, bool isCreation = false, object[] pars = null, bool isDialog = false, Form mdiParentForm = null)
//        {
//            UIStatic.LoadProject(projectName);
//            string action_type;
//            if (isCreation)
//            {
//                action_type = TextConst.AVActionType.DynamicFormCreate;
//            }
//            else
//            {
//                action_type = TextConst.AVActionType.DynamicForm;
//            }
//            VAction action = new VAction();
//            action.Add(new XAttribute(AName.action_type, action_type));
//            action.Add(new XAttribute(AName.call, formName));
//            if (isDialog)
//            {
//                action.SetAttributeValue(TextConst.AName.Modal, TextConst.AVBool.True);
//            }
//            object res = action.Execute(null, null, null, null, null, pars, mdiParentForm: mdiParentForm);
//            XmlReports.Environment.Connection.Commit();
//            return res;
//            //  <action name="open_ur_folders_list" call="ur_folders_list" action-type="dynamic-form" file="sql.builder.templates\sql.builder\projects\asuse2\reports\arbitrage\actions.xml" elid="2643" ord="2643" leaf="1" lvl="2" pelid="" />
//            //UIFormC form = UIStatic.GetForm(formName, isDialog, true);
//            //UIStatic.UpdateForm(form, pars, isCreation);
//            //form.ApplyVisibitlity();
//            //form.GetControl().ShowForm();
//        }
//    }
//}