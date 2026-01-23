//using System;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Xml.Linq;
//using Devart.Data.Oracle;
////using DevExpress.DataAccess.Native.ExpressionEditor;
////using DevExpress.Utils.Extensions;
//using Microsoft.Office.Interop.Excel;
//using sql.builder.WinForms;
//using Application = System.Windows.Forms.Application;
//using ExcelApplication = Microsoft.Office.Interop.Excel.Application;
//using  sql.builder.MP;
//using  sql.builder.MP.Tools;
//using sql.builder.MP.Forms;
//using System.Collections.Generic;
//using FileInfo = DevExpress.DataAccess.Wizard.Presenters.FileInfo;
//using infoenergo.framework.Extensions.Oracle;

//namespace sql.builder.MP.Test
//{
//    internal static class Test
//    {
//        internal static void CreateXml()
//        {
//            //var con = new OracleConnection("data source=alpha;user id=alphin;password=qwaser");
//            string onnectionString = infoenergo.sys.Global.BuildConnectionString("alphin", "qwaser", "alpha");
//            OracleConnection con = new OracleConnection(onnectionString);
//            con.Open(useGlobalSettings: true);
//            var cmd = con.CreateCommand("select * from all_tab_cols where owner = 'PLAN' and table_name = 'IS_AD_ALL'");
//            var reader = cmd.ExecuteReader();
//            var dt = new System.Data.DataTable();
//            dt.Load(reader);

//            var xtable = new XElement("query");
//            var app = new ExcelApplication();
//            app.Workbooks.Open(@"C:\root\main\all\sql.builder.templates\sql.builder\printTemplate\excel\41293.xlsx");
//            var sheet = app.Workbooks[1].Worksheets[1] as Worksheet;

//            foreach (Range column in sheet.UsedRange.Columns)
//            {
//                string val = (column.Rows[4] as Range).Value2.ToString();
//                if(!val.StartsWith("[:a.")) continue;

//                val = val.Replace("[:a.", "").Replace("]", "");

//                var xcolumn = new XElement("column");
//                xcolumn.Add(new XAttribute("table", "a"));
//                xcolumn.Add(new XAttribute("column", val));

//                DataRow drow = dt.Rows.Cast<DataRow>().First(r => r["COLUMN_NAME"].ToString() == val.ToUpper());

              
//                if(drow["DATA_TYPE"].ToString() == "VARCHAR2")
//                {
//                    xcolumn.Add(new XAttribute("type", "string"));
//                    xcolumn.Add(new XAttribute("data-size", drow["DATA_LENGTH"].ToString()));   
//                }
//                else if (drow["DATA_TYPE"].ToString() == "NUMBER")
//                {
//                    xcolumn.Add(new XAttribute("type", "number"));
//                }
//                else if (drow["DATA_TYPE"].ToString() == "DATE")
//                {
//                    xcolumn.Add(new XAttribute("type", "date"));
//                }
//                else
//                {
//                    throw new Exception();
//                }

//                xtable.Add(xcolumn);
//            }
//            app.Quit();

//            con.Close();

//            xtable.Save(@"D:\test\vem_is_ad_all.xml");
//        }
//        internal static void Test2()
//        {
//            var xquery = XElement.Load("Test\\vem_is_ad_all.xml");

//            string destTableName = xquery.Attribute("name").Value;
//            string srcTableName = destTableName + "_imp";
//            var columns = MPColumns.FromXml(xquery);

//            var announcer = new Announcer();

//            var task = new TableLoadTask(null, srcTableName, columns);
//            task.DestTableName = destTableName;
//            var controller = new UpdateDataController(task, announcer);
//            var frm = new frmUpdateData(controller);
//            Application.Run(frm);
//        }
//        internal static void Test3()
//        {
//            //var tableName = "bav_test_old_lk_info";
//            //var s = ExcelLoadUtils.TableScriptFromExcel(@"C:\Users\abelchenko\Desktop\Выгрузка2.xlsx", tableName);
//            MPEnvironment.WorkFolder = Printing.outputFolder;

//            MPEnvironment.DBName = XmlReports.Environment.Connection.GetAlias();
//             MPEnvironment.DBUserName = XmlReports.Environment.Connection.UserId;
//                   MPEnvironment.DBPassword = XmlReports.Environment.Connection.Password;

//                   var xquery = XmlHelpers.XmlSchemeBuilder.XmlTableStructure("bav_test_old_lk_info").Elements().Elements().FirstOrDefault();

//            string destTableName = xquery.Attribute("name").Value;
//            string srcTableName = destTableName;
//            var columns = MPColumns.FromXml(xquery);

//            var announcer = new Announcer();
//            var task = new TableLoadTask(null, srcTableName, columns);
          
//            var controller = new UpdateDataController(task, announcer);
         
//            var frm = new frmUpdateData(controller);
//            frm.Show();
//        }
//        internal static void Test4()// пока оставлю тут
//        {
//            var fileName = sql.builder.MP.Forms.frmUpdateData.SelectExcelFile();
//            if (string.IsNullOrEmpty(fileName)) return;
//            var fname = Path.GetFileName(fileName);
//            var wi = Wait.ShowPanel("Загрузка файла ", true, 0);
          
//            try
//            {
//                MPEnvironment.WorkFolder = Printing.outputFolder;

//                MPEnvironment.DBName = XmlReports.Environment.Connection.GetAlias();
//                MPEnvironment.DBUserName = XmlReports.Environment.Connection.UserId;
//                MPEnvironment.DBPassword = XmlReports.Environment.Connection.Password;
//                XmlReports.Environment.LoadProject("mped");

//                var xquery = new XElement(XmlReports.Environment.GetQuery("va_sp_zatr_buf"));
//                string destTableName = xquery.Attribute(sql.builder.DataApi.TextConst.AName.Name).Value;
//                string srcTableName = destTableName;
//                var columns = MPColumns.FromXml(xquery);
//                var colsZatr = string.Join(",", columns.Select(MPColumn.GetName));
//                var announcer = new Announcer();
//                List<TableLoadTask> tasks = new List<TableLoadTask>();
//                tasks.Add(new TableLoadTask("Затраты", srcTableName, columns, 14, 3));

//                xquery = new XElement(XmlReports.Environment.GetQuery("va_sp_doh_ras_buf"));
//                destTableName = xquery.Attribute(sql.builder.DataApi.TextConst.AName.Name).Value;
//                srcTableName = destTableName;
//                columns = MPColumns.FromXml(xquery);
//                var colsDR = string.Join(",", columns.Select(MPColumn.GetName));
//                tasks.Add(new TableLoadTask("Доходы и расходы", srcTableName, columns, 14, 3));

//                var controller = new UpdateDataController(tasks.ToArray(), announcer);
//                controller.OpenFile(fileName);
//                var yearZatr = Cmn.ToDecimal(controller.GetCellValue("Затраты", 3, 4));
//                var yearDR = Cmn.ToDecimal(controller.GetCellValue("Доходы и расходы", 3, 4));
//                controller.ExecuteLoad();

//                var sb = new StringBuilder();
//                sb.AppendLine("begin");
//                sb.AppendLine("delete va_sp_zatr where god=:god;");
//                sb.AppendLine(string.Format("insert into va_sp_zatr ({0},god) select {0},:god from va_sp_zatr_buf;",
//                    colsZatr));
//                sb.AppendLine("commit;");
                
//                /*
//                sb.AppendLine("delete va_sp_doh_ras where god=:god;");
//                sb.AppendLine(
//                    string.Format("insert into va_sp_doh_ras ({0},god) select {0},:god from va_sp_doh_ras_buf;", colsDR));
//                sb.AppendLine("commit;");
//                */
//                sb.AppendLine("vb_refs.load_va_sp_doh_ras (:god_dr);");

//                sb.AppendLine(
//                    "insert into va_sp_zatr_load_log (filename,filetime ,god ) values (:filename,:filetime,:god_any);");
//                sb.AppendLine("commit;");

//                sb.AppendLine("end;");
//                var fi = new System.IO.FileInfo(fileName);
//                var cmd = new OracleCommand(Cmn.ClearSql(sb.ToString()), XmlReports.Environment.Connection);
               
//                try
//                {

//                    cmd.Parameters.Add("god", yearZatr);
//                    cmd.Parameters.Add("god_dr", yearDR);
//                    cmd.Parameters.Add("god_any", yearZatr == 0 ? yearDR : yearZatr);
//                    cmd.Parameters.Add("filename", fname);
//                    cmd.Parameters.Add("filetime", fi.LastWriteTime);
//                    cmd.ExecuteNonQuery();
//                }
//                finally
//                {
//                    cmd.Dispose();
//                }




//                //db.ExecuteNonQuery(Cmn.ClearSql(sb.ToString()));
//                //WaitUIHelper.LastUsedUIHelper.Hide();

//            }
//            finally
//            {
//                Wait.Hide(wi);

//            }
//            ShowMessage.ShowInformation("Файл \"" + fname + "\" загружен");

          
//        }
//        internal static void LoadKE30SapTempl()// пока оставлю тут
//        {
//            var fileName = sql.builder.MP.Forms.frmUpdateData.SelectExcelFile();
//            if (string.IsNullOrEmpty(fileName)) return;
//            var fname = Path.GetFileName(fileName);
//            var wi = Wait.ShowPanel("Загрузка файла ", true, 0);

//            try
//            {
//                MPEnvironment.WorkFolder = Printing.outputFolder;

                
//                if (XmlReports.Environment.Connection.GetAlias().ToLower().Contains("tgkkido")) 
//                {
//                    //По другому никак, иначе sql loader пытается читать дескриптор из локального tnsnames клиента
//                    //TODO: изменить процедуру DBUserName = XmlReports.Environment.Connection.DBName - получать не имя БД, а полностью дескриптор
//                    MPEnvironment.DBName = "(DESCRIPTION = (ADDRESS_LIST = (ADDRESS = (PROTOCOL = TCP)(HOST = 172.20.19.80)(PORT = 1521))) (CONNECT_DATA = (SERVICE_NAME = KIDO)))";
//                }
//                else 
//                {
//                    MPEnvironment.DBName = XmlReports.Environment.Connection.GetAlias();    
//                }

//                MPEnvironment.DBUserName = XmlReports.Environment.Connection.UserId;
//                MPEnvironment.DBPassword = XmlReports.Environment.Connection.Password;
//                XmlReports.Environment.LoadProject("mped");

//                var xquery = new XElement(XmlReports.Environment.GetQuery("va_sp_calc_buf"));
//                string destTableName = xquery.Attribute(sql.builder.DataApi.TextConst.AName.Name).Value;
//                string srcTableName = destTableName;
//                var columns = MPColumns.FromXml(xquery);
//                var colsCalc = string.Join(",", columns.Select(MPColumn.GetName));
//                var announcer = new Announcer();
                
                
//                Microsoft.Office.Interop.Excel.Application app = new Microsoft.Office.Interop.Excel.Application();
//                Microsoft.Office.Interop.Excel.Workbook wb = app.Workbooks.Open(fileName);                
//                Microsoft.Office.Interop.Excel.Worksheet excelWorkSheet = wb.Worksheets[1];
                
//                var sheetName = excelWorkSheet.Name;

//                wb.Close(false, false, false);
//                excelWorkSheet = null;
//                app.Quit();                
//                wb = null;                             
//                app = null;
//                GC.Collect();
                
//                List<TableLoadTask> tasks = new List<TableLoadTask>();
//                tasks.Add(new TableLoadTask(sheetName, srcTableName, columns, 2, 1));                

//                var controller = new UpdateDataController(tasks.ToArray(), announcer);
//                controller.OpenFile(fileName);
//                controller.ExecuteLoad();
//                /* --ВРЕМЕННО
//                try
//                {
//                    controller.ExecuteLoad();
//                }
//                catch
//                {
//                    var err = new StringBuilder();
//                    err.AppendLine("Файл \"" + fname + "\" не загружен!");
//                    err.AppendLine("Проверьте формат листа \"" + sheetName + "\"");
//                    err.AppendLine();
//                    err.AppendLine("1. В первой строке должна быть шапка таблицы, со второй строки данные.");
//                    err.AppendLine("2. Список колонок в корректном порядке:");
//                    err.AppendLine("   Вид деятельности, Объект генерации, Период/год, Субъект РФ, Заказ, МВЗ,");
//                    err.AppendLine("   МВП: партнер, Элемент затрат, МВП, Выручка, ОбъемСбыта, Затраты, ТУТ,");
//                    err.AppendLine("   РасхВод М3, РасхВод Т, РезТеплНаг, ПрисТепНаг");
//                    ShowMessage.ShowError(err.ToString());
//                    return;
//                }
//                 */

//                var sb = new StringBuilder();
//                sb.AppendLine("begin");                
//                sb.AppendLine("vb_refs.load_calc_data(:filename, :filetime, null);");
//                sb.AppendLine("end;");

//                var fi = new System.IO.FileInfo(fileName);
//                var cmd = new OracleCommand(Cmn.ClearSql(sb.ToString()), XmlReports.Environment.Connection);

//                try
//                {
//                    cmd.Parameters.Add("filename", fname);
//                    cmd.Parameters.Add("filetime", fi.LastWriteTime);
//                    cmd.ExecuteNonQuery();
//                }
//                finally
//                {
//                    cmd.Dispose();                   
//                }                
//            }
//            finally
//            {
//                Wait.Hide(wi);

//            }
//            ShowMessage.ShowInformation("Файл \"" + fname + "\" загружен");
//        }
//    }
//}