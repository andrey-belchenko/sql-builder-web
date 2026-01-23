//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
////using System.Windows.Forms;
////using DevExpress.XtraEditors;
//using infoenergo.core.Data;
//using infoenergo.sys;

//namespace sql.builder.Special.Report
//{
//    static class BatchUnloadRep  // Пакетная выгрузка отчета
//    {
//        #region FormationReport
//        public static bool? FormationReport(object[] p_dep, decimal p_ym_beg, decimal p_ym_end, object[] p_kod_adr_m, int p_flag_sum)      
//        {
//            try
//            {
//                string sPath = string.Empty;
//                string sFolderPath = string.Empty;
//                int countRegion = 0;
//                string strSqlRegion = string.Empty;
//                string sFileRepName = string.Empty;
//                int flagSum = p_flag_sum;
               
//                var pars = new Dictionary<string, object>();
//                DataTable dtRegion = null;
//                var region = new Dictionary<int, string>();

//                var showPopupWaitForms = SqlBuilder.ShowPopupWaitForms;
//                SqlBuilder.ShowPopupWaitForms = true;

//                string strSqlDep = @"SELECT distinct sname FROM kr_org WHERE kodp = " + p_dep[0].ToString() + "";
//                // Название папки: Dep_Region
//                string sDepName = DataHelper.SqlGetValue(strSqlDep, null, Global.Connection).ToString();
//                string sFolderName = sDepName;

//                int countYm = 0;  
                
//                pars.Add("p_dep", p_dep);
//                pars.Add("ym1", p_ym_beg);
//                pars.Add("ym2", p_ym_end);
//                pars.Add("kod_adr_m", null);

//                infoenergo.sys.YM.YM ym = new infoenergo.sys.YM.YM(Convert.ToDouble(p_ym_beg)); // отчетный период
              
//                if (p_kod_adr_m != null)
//                {
//                    strSqlRegion = @"SELECT kod_m, name_s FROM adr_m WHERE kod_m in (" + HelperRep.getStrFromObj(p_kod_adr_m) + ")";
//                    dtRegion = DataHelper.SqlGetTable(strSqlRegion, Global.Connection);
//                    countRegion = dtRegion.Rows.Count;
//                }
//                // Если периоды ссумировать не нужно, то по каждому периоду отдельный отчет
//                if (flagSum == 0)
//                {
//                    decimal ym_rep = p_ym_beg;
//                    while ( ym_rep <= p_ym_end)   
//                    {
//                        pars["ym1"] = ym_rep;
//                        pars["ym2"] = pars["ym1"];

//                        //Подготовка, формирование и сохранение в нужном месте
//                        prepare_param_and_exec_rep(countRegion, dtRegion, pars, sDepName, p_kod_adr_m);
//                        countYm += 1;
//                        ym_rep = ym.Add(countYm).ToDecimal();

//                    }  // end for(int numYm = 1; numYm < countYm + 1; numYm++ )
//                }
//                else
//                {
//                    //Подготовка, формирование и сохранение в нужном месте
//                    prepare_param_and_exec_rep(countRegion, dtRegion, pars, sDepName, p_kod_adr_m);
//                }

//                SqlBuilder.ShowPopupWaitForms = showPopupWaitForms;
//                XtraMessageBox.Show("Выгрузка отчета завершена.", "Формирование отчетов", MessageBoxButtons.OK);
//                return true;
//            }
//            catch (Exception ex)
//            {
//                infoenergo.ui.win.ExceptionHandler.HandleException(ex);
//                return false;
//            }    
//        }
//        #endregion

//        //Подготовка, формирование и сохранение в нужном месте
//        private static void prepare_param_and_exec_rep(int countRegion, DataTable dtRegion, Dictionary<string, object> pars,  string sDepName, object[] p_kod_adr_m)
//        {
//           string sFolderName = sDepName;

//           if (countRegion > 0)
//           {
//                //для каждого выбранного субъета РФ формируем отдельный отчет в отдельную папку
//                for (int i = 0; i < countRegion; i++)
//                {
//                 //region[Convert.ToInt16(dtRegion.Rows[i]["kod_m"])] = dtRegion.Rows[i]["name_s"].ToString();
//                 pars["kod_adr_m"] = new object[1] { p_kod_adr_m[i] };
//                 sFolderName = sDepName + " " + dtRegion.Rows[i]["name_s"].ToString(); //region[Convert.ToInt16(p_kod_adr_m[i])].ToString();
//                 // Формируем отчет и перемещаем в нужное место
//                 exec_rep_move(pars, sFolderName);
//               }
//            }
//          else
//            {
//             // Формируем отчет по всему отделению и перемещаем в нужное место
//             exec_rep_move(pars, sFolderName);
//            }  // end if(countRegion > 0)
//         }


//        // Формируем отчет и переносим в нужное место
//        private static void exec_rep_move(Dictionary<string, object> pars, string sFolderName)
//        {
//            string sPath = SqlBuilder.ExecReportGetPath("ies_garant.61880_9_v3_batch", pars);
//            // формируем новый путь
//            string sFolderPath = Path.Combine(sql.builder.SqlBuilder.GetWorkFolderPath(), sFolderName);
//            //ym для названия файла (если период один, то берем период, иначе одна дата (с-по))
//            string strYm = pars["ym1"] == pars["ym2"] ? Convert.ToString(pars["ym1"]) : Convert.ToString(pars["ym1"]) + "-" + Convert.ToString(pars["ym2"]);
//            string sFileRepName = StringExt.SubstringAfter(sPath, '\\').ToString();
//            //sFileRepName.Replace(".", strYm + ".");
//            if (!Directory.Exists(sFolderPath))
//            {
//                try { Directory.CreateDirectory(sFolderPath); }
//                catch (Exception ex)
//                {
//                    infoenergo.ui.win.ExceptionHandler.HandleException(ex);                    
//                }
//            }
//            string sRepFullPathNew = Path.Combine(sFolderPath, sFileRepName.Replace(".", "_" + strYm.Replace(",","") + "."));
//            if (File.Exists(sRepFullPathNew)) File.Delete(sRepFullPathNew);

//            File.Move(sPath, sRepFullPathNew);
//        }

//    }
//}
