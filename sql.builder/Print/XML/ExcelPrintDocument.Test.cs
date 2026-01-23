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

//namespace sql.builder.Print.XML
//{
//#if DEBUG
//    public partial class ExcelPrintDocument
//    {
//        /// <summary>
//        /// Набор unit-тестов для класса <see cref="sql.builder.Print.XML.ExcelPrintDocument"/>
//        /// </summary>
//        [TestClass]
//        public class Test
//        {
//            private static DataSet LoadDataSet(string file_name)
//            {
//                DataSet dataset;
//                using (FileStream stream = File.Open(file_name, FileMode.Open, FileAccess.Read, FileShare.Read)) {
//                    IFormatter formatter = new BinaryFormatter();
//                    dataset = (DataSet)formatter.Deserialize(stream);
//                    stream.Close();
//                }
//                return dataset;
//            }
//            [TestMethod]
//            public void Report_54148()
//            {
//                string path = @"C:\team_commerc\text\SQL Builder\Report\54148\";
//                DataSet dataset = LoadDataSet(path + "54148.dat");
//                //
//                Stopwatch sw = new Stopwatch();
//                Assert.AreEqual(GC.MaxGeneration, 2);
//                int gen_0_before = GC.CollectionCount(0);
//                int gen_1_before = GC.CollectionCount(1);
//                int gen_2_before = GC.CollectionCount(2);
//                long mem_before = GC.GetTotalMemory(false);
//                sw.Start();
//                //
//                ExcelPrintDocument doc = new ExcelPrintDocument(@"C:\team_commerc\text\SQL Builder\Report\54148\54148.xml");
//                string result_file = path + DateTime.Now.ToString("yyyy'_'MM'_'dd'_'HH'_'mm'_'ss'.xml'");
//                doc.Print(result_file, dataset, false, false);
//                doc = null;
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
//                sql.builder.ExcelPrintDocument.PostProcess(result_file, "xlsx", null, null);
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
//            public void Report_10653_37_5()
//            {
//                DataSet dataset = LoadDataSet(@"C:\team_commerc\text\SQL Builder\Report\10653_37\10653_37_5.dat");
//                //
//                Stopwatch sw = new Stopwatch();
//                Assert.AreEqual(GC.MaxGeneration, 2);
//                int gen_0_before = GC.CollectionCount(0);
//                int gen_1_before = GC.CollectionCount(1);
//                int gen_2_before = GC.CollectionCount(2);
//                long mem_before = GC.GetTotalMemory(false);
//                sw.Start();
//                //
//                ExcelPrintDocument doc = new ExcelPrintDocument(@"C:\team_commerc\text\SQL Builder\Report\10653_37\10653(37)-5-new.xml");
//                string result_file = @"C:\team_commerc\text\SQL Builder\Report\10653_37\Result_2022_04_11_12_15.xml";
//                doc.Print(result_file, dataset, false, false);
//                doc = null;
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
//                sql.builder.ExcelPrintDocument.PostProcess(result_file, "xlsx", null, null);
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
//            public void Report_29265()
//            {
//                DataSet dataset = LoadDataSet(@"C:\team_commerc\text\SQL Builder\Report\29265\29265.dat");
//                //
//                Stopwatch sw = new Stopwatch();
//                Assert.AreEqual(GC.MaxGeneration, 2);
//                int gen_0_before = GC.CollectionCount(0);
//                int gen_1_before = GC.CollectionCount(1);
//                int gen_2_before = GC.CollectionCount(2);
//                long mem_before = GC.GetTotalMemory(false);
//                sw.Start();
//                //
//                ExcelPrintDocument doc = new ExcelPrintDocument(@"C:\team_commerc\text\SQL Builder\Report\29265\29265.xml");
//                string result_file = @"C:\team_commerc\text\SQL Builder\Report\29265\Result_2022_04_11_16_50.xml";
//                doc.Print(result_file, dataset, false, false);
//                doc = null;
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
//                sql.builder.ExcelPrintDocument.PostProcess(result_file, "xlsx", null, null);
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
