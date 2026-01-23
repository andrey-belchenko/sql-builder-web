//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using Microsoft.Office.Interop.Excel;

//using System.IO;
//using System.Diagnostics;

//namespace sql.builder.XmlHelpers
//{
//    class ExcelLoadUtils
//    {
//        public static string TableScriptFromExcel(string filename, string tableName=null)
//        {
//            //var filename = @"C:\Users\alugovykh\Downloads\Выгрузка2 - копия.xlsx";
           
//            var app = new Application();
//            app.Visible = true;//чтобы видеть прогресс
//            var script = new StringBuilder();
//            try
//            {


//                Workbook wb = app.Workbooks.Open(filename);
//                try
//                {
//                    Worksheet sh = wb.Sheets[1];
//                    sh.Activate();//чтобы видеть прогресс
//                    Range uRange = sh.UsedRange;
//                    Range r1 = uRange.Rows[1];
//                    if (tableName == null)
//                    {
//                        tableName = ModifyName(wb.Name).ToString();
//                    }

//                    script.AppendLine(string.Format("create table {0} \n(", tableName));

//                    foreach (Range c in r1.Cells)
//                    {
//                        c.Select();//чтобы видеть прогресс
//                        var dType = OracleDataType(c.Offset[1, 0].NumberFormat);
//                        if (dType == "General")
//                        {
//                            //int i = 0;
//                            var fRes = ((Range) uRange.Columns[c.Column]).Find("*",SearchDirection:XlSearchDirection.xlPrevious);
//                            //foreach (Range c2 in ((Range) uRange.Columns[c.Column]).Cells)
//                            foreach (Range c2 in fRes.Cells)
//                            {
//                                //i++;
//                                //if (i > 1000)
//                                //{
//                                //    c2.Select();
//                                //    i = 0;
//                                //}
//                                c2.Select();//чтобы видеть прогресс
//                                if (c2.Row != r1.Row && (c2.Value != null || c2.NumberFormat != "General"))
//                                {
//                                    dType = OracleDataType(c2.NumberFormat);
//                                    if (dType == "General")
//                                    {
//                                        if (c2.Value is string) dType = "VARCHAR2(4000)";
//                                        if (c2.Value is DateTime) dType = "DATE";
//                                        if (Cmn.IsNumeric(c2.Value)) dType = "NUMBER";
//                                    }
//                                    break;
//                                }
//                            }
//                        }
//                        if (dType == "General")
//                        {
//                            dType = "VARCHAR2(1)"; 
//                        }

//                       // if (dType != "General")
//                            script.Append(string.Format("\r\n\t{0} {1},", ModifyName(c.Value), dType));
//                    }

//                    if (script.Length > 1) script.Replace(",", "", script.Length - 1, 1);

//                    script.AppendLine("\r\n)");
//                }
//                finally
//                {
//                    wb.Close(0);
//                }

//                //Process.Start(writeScriptFile("ExcelLoad", script.ToString()));
              
//            }
//            finally
//            {
//                app.Quit();
//            }
//            return script.ToString();
//        }

//        private static string ModifyName(string name)
//        {
//            name = AddUnderline(name);
//            var modName = new StringBuilder(name.ToUpper());
//            modName.Replace(".XLSX", "")
//                    .Replace(".XLS", "")
//                    .Replace(".XLSB", "")
//                    .Replace(".XLSM", "")
//                    .Replace(" ", "_")
//                    .Replace("-", "_");
            
          
//            var s = ReplaceToTranslit(modName.ToString()).ToString();
//            while (s.Contains("__"))
//            {
//                s = s.Replace("__", "_");
//            }
//            if (s.Length > 30)
//            {
//                s = s.Substring(0,30);
//            }
//            return s;
//        }


//        private static string AddUnderline(string name)
//        {
//            var s = "";
//            int i = 0;
//            //if (name == "TBMaxNewPower")
//            //{
                
//            //}
//            bool prevL = false;
//            var cheskStrU = "QWERTYUIOPASDFGHJKLZXCVBNMЙЦУКЕНГШЩЗХФЫВАПРОЛДЖЭЯЧСМИТБЮ";

//            foreach (var c in name)
//            {
//                var q = "";
//                bool nextL = false;
//                if (i < name.Length - 1)
//                {
//                    var cn = name[i + 1];
//                    if (cheskStrU.ToLower().Contains(cn))
//                    {
//                        nextL = true;
//                    }
//                }
//                if (cheskStrU.Contains(c))
//                {
//                    if ((prevL || nextL) && i > 0)
//                    {
//                        q = "_";
//                    }
//                    prevL = false;

//                }
//                else
//                {
//                    prevL = true;
                    
                  
//                }
//                s += q + c;
//               i++;
//            }
//            return s;
            
          
//        }

//        private static string[] arrToTranslit = new string[] {"А A", "Б B", "В V", "Г G", "Д D", "Е E", "Ё YO", "Ж ZH", "З Z", "И I", "Й Y", "К K", "Л L", "М M",
//                                                            "Н N", "О O", "П P", "Р R", "С S", "Т T", "У U", "Ф F", "Х KH", "Ц TS", "Ч CH", "Ш SH", "Щ SCH",
//                                                            "Ъ ", "Ы Y", "Ь ", "Э E", "Ю YU", "Я YA"};
        
//        private static StringBuilder ReplaceToTranslit(string str)
//        {
//            var result = new StringBuilder();
            
//            foreach (var s in str)
//            {
//                result.Append(s);

//                foreach (var a in arrToTranslit)
//                {
//                    var c = a.Split(' ');
//                    if (s.ToString() == c[0])
//                    {
//                        result.Remove(result.Length - 1, 1);
//                        result.Append(c[1]);
//                        break;
//                    }
//                }
//            }
            
//            return result;
//        }

//        private static string OracleDataType(string exType)
//        {
//            var result = "";

//            if (exType == "General") result = "General";
//            else
//                if (exType == "@") result = "VARCHAR2(4000)";
//                else
//                    if (exType.IndexOf("y") > 0) result = "DATE";
//                    else result = "NUMBER";

//            return result;
//        }

//        //private static string writeScriptFile(string name, string data)
//        //{
//        //    var namefile = string.Format("{0}\\{1}_{2}_{3}_ddl.sql", Path.GetDirectoryName(Path.GetTempPath()), DateTime.Now.ToString("yyMMdd"), Environment.MachineName, name);
//        //    using (var sw = new StreamWriter(new FileStream(namefile, FileMode.Create), Encoding.GetEncoding(1251)))
//        //    {
//        //        sw.Write(data);
//        //    }
//        //    return namefile;
//        //}
        
//    }
//}
