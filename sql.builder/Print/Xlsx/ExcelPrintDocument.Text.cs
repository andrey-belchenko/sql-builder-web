//using System;
//using System.Data;
//using System.IO;
//using System.Diagnostics;
//using Contract = System.Diagnostics.Contracts.Contract;
//using System.Xml.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Collections.Generic;
//using System.Runtime.Serialization;
//using System.Runtime.Serialization.Formatters.Binary;
//using sql.builder.DataApi;
//using ExcelPrintErrors = sql.builder.ExcelPrintDocument.ExcelPrintErrors;

//namespace sql.builder.Print.Xlsx
//{
//#if DEBUG
//    internal partial class ExcelPrintDocument
//    {
//        /// <summary>
//        /// Набор unit-тестов для класса <see cref="sql.builder.Print.XML.ExcelPrintDocument"/>
//        /// </summary>
//        [TestClass]
//        public class Test
//        {
//            private static T Load<T>(string file_name)
//                where T: class
//            {
//                T dataset;
//                using (FileStream stream = File.Open(file_name, FileMode.Open, FileAccess.Read, FileShare.Read)) {
//                    IFormatter formatter = new BinaryFormatter();
//                    dataset = (T)formatter.Deserialize(stream);
//                    stream.Close();
//                }
//                return dataset;
//            }
//            private static void InternStringColumn(DataTable table, string column_name)
//            {
//                int count = Cmn.InternStringColumn(table, column_name);
//                Debug.WriteLine("Уникальных значений в колонке " + column_name + ": " + count.ToString());
//            }
//            [TestMethod]
//            public void Report_52733()
//            {
//                DataSet dataset = new DataSet();
//                DataTable table = Load<DataTable>(@"C:\team_commerc\text\SQL Builder\Report\52733\ALPHA\52733_a.dat");
//                //table.TableName = "a";
//                dataset.Tables.Add(table);
//                table = Load<DataTable>(@"C:\team_commerc\text\SQL Builder\Report\52733\ALPHA\52733_pars.dat");
//                //table.TableName = "pars";
//                dataset.Tables.Add(table);
//                //dataset.SerializeToFile(@"C:\team_commerc\text\SQL Builder\Report\52733\ALPHA\52733.dat");
                
//                string path = @"C:\team_commerc\text\SQL Builder\Report\52733\";
//                //DataSet dataset = Load<DataSet>(path + "52733.dat");
//                table = dataset.Tables[0];
//                Debug.WriteLine("Колонки: ");
//                for (int index = 0; index < table.Columns.Count; index++) {
//                    DataColumn col = table.Columns[index];
//                    Debug.WriteLine("  " + col.ColumnName + " " + col.DataType.FullName);
//                }
//                Stopwatch sw = new Stopwatch();
//                GC.Collect();
//                long mem_before = GC.GetTotalMemory(false);
//                sw.Start();
//                InternStringColumn(table, "grsetname");
//                InternStringColumn(table, "grsetid");
//                InternStringColumn(table, "origgrsetid");
//                InternStringColumn(table, "parent_grsetid");
//                InternStringColumn(table, "growid");
//                InternStringColumn(table, "parent_growid");
//                InternStringColumn(table, "fskname");
//                InternStringColumn(table, "pwrc_fdrname");
//                InternStringColumn(table, "pwrc_fdr_explname");
//                InternStringColumn(table, "pwrcname");
//                InternStringColumn(table, "real_fdr");
//                InternStringColumn(table, "fskname");
//                InternStringColumn(table, "pwrc_balsnamex");
//                InternStringColumn(table, "pwrc_fdr_balsnamex");
//                InternStringColumn(table, "srcbalname");
//                InternStringColumn(table, "srcname");
//                InternStringColumn(table, "place_namefider");
//                sw.Stop();
//                GC.Collect();
//                long mem_after = GC.GetTotalMemory(false);
//                Debug.WriteLine("Интернирование: " + sw.ElapsedMilliseconds.ToString() + " мс");
//                Debug.WriteLine("  Занятая память: " + (mem_after - mem_before).ToString());
//                //
//                string template_path = path + "52733.xlsx";
//                string output_path = path + "Накопительный итог.xlsx";
//                //
//                Assert.AreEqual(GC.MaxGeneration, 2);
//                int gen_0_before = GC.CollectionCount(0);
//                int gen_1_before = GC.CollectionCount(1);
//                int gen_2_before = GC.CollectionCount(2);
//                mem_before = GC.GetTotalMemory(false);
//                sw.Start();
//                //
//                WaitUIHelper.LastUsedUIHelper.Show("Формирование файла", WaitUIMode.WaitPanel, true);
//                ExcelPrintOptions options = ExcelPrintOptions.Default;
//                ExcelPrintEnv env = new ExcelPrintEnv(template_path, options, dataset);
//                var doc = new ExcelPrintDocument(env);
//                doc.Printing += sql.builder.ExcelPrintDocument.OnPrintingHandler;
//                ExcelPrintErrors result = doc.Print(dataset, options.UseDataReader);
//                doc.Printing -= sql.builder.ExcelPrintDocument.OnPrintingHandler;
//                if (result != ExcelPrintErrors.NoData) {
//                    doc.Save(output_path);
//                }
//                doc = null;
//                env.Dispose();
//                env = null;
//                WaitUIHelper.LastUsedUIHelper.Hide();
//                //
//                sw.Stop();
//                mem_after = GC.GetTotalMemory(false);
//                int gen_0_after = GC.CollectionCount(0);
//                int gen_1_after = GC.CollectionCount(1);
//                int gen_2_after = GC.CollectionCount(2);
//                Debug.WriteLine("Формирование отчёта: " + sw.ElapsedMilliseconds.ToString() + " мс");
//                Debug.WriteLine("  Занятая память: " + (mem_after - mem_before).ToString());
//                Debug.WriteLine("  Сборок мусора (0/1/2): " + (gen_0_after - gen_0_before).ToString() + " / " + (gen_1_after - gen_1_before).ToString() + " / " + (gen_2_after - gen_2_before).ToString());
//                //
//                /*
//                sw.Reset();
//                gen_0_before = GC.CollectionCount(0);
//                gen_1_before = GC.CollectionCount(1);
//                gen_2_before = GC.CollectionCount(2);
//                mem_before = GC.GetTotalMemory(false);
//                sw.Start();
//                //
//                sql.builder.ExcelPrintDocument.PostProcess(result_file, "xlsx", null, null);
//                //
//                sw.Stop();
//                mem_after = GC.GetTotalMemory(false);
//                gen_0_after = GC.CollectionCount(0);
//                gen_1_after = GC.CollectionCount(1);
//                gen_2_after = GC.CollectionCount(2);
//                Debug.WriteLine("ExcelPrintDocument.PostProcess: " + sw.ElapsedMilliseconds.ToString() + " мс");
//                Debug.WriteLine("  Занятая память: " + (mem_after - mem_before).ToString());
//                Debug.WriteLine("  Сборок мусора (0/1/2): " + (gen_0_after - gen_0_before).ToString() + " / " + (gen_1_after - gen_1_before).ToString() + " / " + (gen_2_after - gen_2_before).ToString());*/
//            }
//            [TestMethod]
//            public void Report_52834()
//            {
//                string path = @"C:\team_commerc\text\SQL Builder\Report\52834\";
//                DataSet dataset = Load<DataSet>(path + "52834.dat");
//                /*DataTable table = dataset.Tables[0];
//                Debug.WriteLine("Колонки: ");
//                for (int index = 0; index < table.Columns.Count; index++) {
//                    DataColumn col = table.Columns[index];
//                    Debug.WriteLine("  " + col.ColumnName + " " + col.DataType.FullName);
//                }
//                InternStringColumn(table, "grsetname");
//                InternStringColumn(table, "grsetid");
//                InternStringColumn(table, "origgrsetid");
//                InternStringColumn(table, "parent_grsetid");
//                InternStringColumn(table, "growid");
//                InternStringColumn(table, "parent_growid");*/
//                string template_path = path + "52834.xlsx";
//                //string output_path = path + "Отчёт по исполнительному производству по юридическим лицам.xlsx";
//                string output_path = path + DateTime.Now.ToString("yyyy'_'MM'_'dd'_'HH'_'mm'_'ss'.xlsx'");
//                //
//                Assert.AreEqual(GC.MaxGeneration, 2);
//                int gen_0_before = GC.CollectionCount(0);
//                int gen_1_before = GC.CollectionCount(1);
//                int gen_2_before = GC.CollectionCount(2);
//                long mem_before = GC.GetTotalMemory(false);
//                Stopwatch sw = new Stopwatch();
//                sw.Start();
//                //
//                ExcelPrintOptions options = ExcelPrintOptions.Default;
//                ExcelPrintEnv env = new ExcelPrintEnv(template_path, options, dataset);
//                var doc = new ExcelPrintDocument(env);
//                ExcelPrintErrors result = doc.Print(dataset, options.UseDataReader);
//                if (result != ExcelPrintErrors.NoData) {
//                    doc.Save(output_path);
//                }
//                doc = null;
//                env.Dispose();
//                env = null;
//                //
//                sw.Stop();
//                long mem_after = GC.GetTotalMemory(false);
//                int gen_0_after = GC.CollectionCount(0);
//                int gen_1_after = GC.CollectionCount(1);
//                int gen_2_after = GC.CollectionCount(2);
//                Debug.WriteLine("Формирование отчёта: " + sw.ElapsedMilliseconds.ToString() + " мс");
//                Debug.WriteLine("  Занятая память: " + (mem_after - mem_before).ToString());
//                Debug.WriteLine("  Сборок мусора (0/1/2): " + (gen_0_after - gen_0_before).ToString() + " / " + (gen_1_after - gen_1_before).ToString() + " / " + (gen_2_after - gen_2_before).ToString());
//                //
//                sw.Reset();
//                gen_0_before = GC.CollectionCount(0);
//                gen_1_before = GC.CollectionCount(1);
//                gen_2_before = GC.CollectionCount(2);
//                mem_before = GC.GetTotalMemory(false);
//                sw.Start();
//                //
//                sql.builder.ExcelPrintDocument.PostProcess(output_path, "xlsx", null, options.FormatSource);
//                //
//                sw.Stop();
//                mem_after = GC.GetTotalMemory(false);
//                gen_0_after = GC.CollectionCount(0);
//                gen_1_after = GC.CollectionCount(1);
//                gen_2_after = GC.CollectionCount(2);
//                Debug.WriteLine("ExcelPrintDocument.PostProcess: " + sw.ElapsedMilliseconds.ToString() + " мс");
//                Debug.WriteLine("  Занятая память: " + (mem_after - mem_before).ToString());
//                Debug.WriteLine("  Сборок мусора (0/1/2): " + (gen_0_after - gen_0_before).ToString() + " / " + (gen_1_after - gen_1_before).ToString() + " / " + (gen_2_after - gen_2_before).ToString());
//            }
//            [TestMethod]
//            public void Report_46239()
//            {
//                string path = @"C:\team_commerc\text\SQL Builder\Report\46239\";
//                DataSet dataset = Load<DataSet>(path + "46239_with_pivot.dat");
//                //dataset.Tables.Add(Load<DataTable>(@"C:\team_commerc\text\SQL Builder\Report\46239\filwhy.dat"));
//                //dataset.Tables.Add(Load<DataTable>(@"C:\team_commerc\text\SQL Builder\Report\46239\cols.dat"));

//                for (int table_index = 0; table_index < dataset.Tables.Count; table_index++) {
//                    DataTable table = dataset.Tables[table_index];
//                    Debug.WriteLine("Колонки (" + table_index.ToString() + " " + table.TableName + "):");
//                    for (int index = 0; index < table.Columns.Count; index++) {
//                        DataColumn col = table.Columns[index];
//                        Debug.WriteLine("  " + col.ColumnName + " " + col.DataType.FullName);
//                    }
//                }
//                string template_path = path + "46239.xlsx";
//                //string output_path = path + "Отчет по банковским оплатам.xlsx";
//                string output_path = path + DateTime.Now.ToString("yyyy'_'MM'_'dd'_'HH'_'mm'_'ss'.xlsx'");
//                //
//                Assert.AreEqual(GC.MaxGeneration, 2);
//                int gen_0_before = GC.CollectionCount(0);
//                int gen_1_before = GC.CollectionCount(1);
//                int gen_2_before = GC.CollectionCount(2);
//                long mem_before = GC.GetTotalMemory(false);
//                Stopwatch sw = new Stopwatch();
//                sw.Start();
//                //
//                ExcelPrintOptions options = ExcelPrintOptions.Default;
//                ExcelPrintEnv env = new ExcelPrintEnv(template_path, options, dataset);
//                var doc = new ExcelPrintDocument(env);
//                ExcelPrintErrors result = doc.Print(dataset, options.UseDataReader);
//                if (result != ExcelPrintErrors.NoData) {
//                    doc.Save(output_path);
//                }
//                doc = null;
//                env.Dispose();
//                env = null;
//                //
//                sw.Stop();
//                long mem_after = GC.GetTotalMemory(false);
//                int gen_0_after = GC.CollectionCount(0);
//                int gen_1_after = GC.CollectionCount(1);
//                int gen_2_after = GC.CollectionCount(2);
//                Debug.WriteLine("Формирование отчёта: " + sw.ElapsedMilliseconds.ToString() + " мс");
//                Debug.WriteLine("  Занятая память: " + (mem_after - mem_before).ToString());
//                Debug.WriteLine("  Сборок мусора (0/1/2): " + (gen_0_after - gen_0_before).ToString() + " / " + (gen_1_after - gen_1_before).ToString() + " / " + (gen_2_after - gen_2_before).ToString());
//                //
//                sw.Reset();
//                gen_0_before = GC.CollectionCount(0);
//                gen_1_before = GC.CollectionCount(1);
//                gen_2_before = GC.CollectionCount(2);
//                mem_before = GC.GetTotalMemory(false);
//                sw.Start();
//                //
//                sql.builder.ExcelPrintDocument.PostProcess(output_path, "xlsx", null, options.FormatSource);
//                //
//                sw.Stop();
//                mem_after = GC.GetTotalMemory(false);
//                gen_0_after = GC.CollectionCount(0);
//                gen_1_after = GC.CollectionCount(1);
//                gen_2_after = GC.CollectionCount(2);
//                Debug.WriteLine("ExcelPrintDocument.PostProcess: " + sw.ElapsedMilliseconds.ToString() + " мс");
//                Debug.WriteLine("  Занятая память: " + (mem_after - mem_before).ToString());
//                Debug.WriteLine("  Сборок мусора (0/1/2): " + (gen_0_after - gen_0_before).ToString() + " / " + (gen_1_after - gen_1_before).ToString() + " / " + (gen_2_after - gen_2_before).ToString());
//            }
//        }
//    }
//#endif
//}
