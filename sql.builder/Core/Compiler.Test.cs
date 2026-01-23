//using System;
//using System.IO;
//using System.Diagnostics;
//using System.Xml.Linq;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System.Collections; // IStructuralEquatable, StructuralComparisons
//using System.Collections.Generic;
////using System.Linq;
////using System.Text;
//using Global = infoenergo.sys.Global;
//using sql.builder.DataApi;

//namespace sql.builder
//{
//#if DEBUG
//    internal partial class Cmn
//    {
//        /// <summary>
//        /// Набор unit-тестов для класса <see cref="Cmn"/>
//        /// </summary>
//        [TestClass]
//        public class Test
//        {
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="Cmn.ExtractParamsFromString(string)"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("String")]
//            public void ExtractParamsFromString()
//            {
//                Stopwatch sw = new Stopwatch();
//                sw.Start();
//                // Первый тест
//                List<string> list = Cmn.ExtractParamsFromString("[:ur_hist_mat.nump] дело [:ur_mat.num_delo]");
//                Assert.AreEqual(list.Count, 2);
//                Assert.AreEqual(list[0], "ur_hist_mat.nump");
//                Assert.AreEqual(list[1], "ur_mat.num_delo");
//                // Второй тест
//                list = Cmn.ExtractParamsFromString("Карточка по ведению ПИР [:kodp]");
//                Assert.AreEqual(list.Count, 1);
//                Assert.AreEqual(list[0], "kodp");
//                // Третий тест
//                list = Cmn.ExtractParamsFromString("[:ur_hist_mat.nump] дело [:ur_mat.num_delo] ([:ur_hist_mat.nump])");
//                Assert.AreEqual(list.Count, 2);
//                Assert.AreEqual(list[0], "ur_hist_mat.nump");
//                Assert.AreEqual(list[1], "ur_mat.num_delo");
//                // Четвёртый тест
//                list = Cmn.ExtractParamsFromString("[ur_hist_mat.nump] дело [ur_mat.num_delo]");
//                Assert.IsNull(list); // Старая реализация
//                //Assert.AreEqual(list.Count, 0); // Новая реализация
//                // Пятый тест
//                list = Cmn.ExtractParamsFromString("Карточка по ведению ПИР [:kodp");
//                Assert.AreEqual(list.Count, 0);
//                //
//                sw.Stop();
//                Debug.WriteLine("Cmn.Test.ExtractParamsFromString(): " + sw.ElapsedTicks.ToString() + " тактов, " + sw.ElapsedMilliseconds.ToString() + " мс");
//            }
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="Cmn.ExtractParameterNamesFromSQL(string)"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("DB")]
//            public void ExtractParameterNamesFromSQL()
//            {
//                IStructuralEquatable params_to_compare = new string[2] { "p_kod_dog", "p_ym" };
//                string[] param_names;
//                // Первый тест
//                param_names = Cmn.ExtractParameterNamesFromSQL(@"/* Тест:1 */SELECT * FROM tnr_account WHERE :p_kod_dog=kod_dog AND ym = :p_ym AND rym = :p_ym AND info = ' :info '");
//                //Assert.IsTrue(param_names.Length == 2 && param_names[0] == "p_kod_dog" && param_names[1] == "p_ym");
//                Assert.IsTrue(params_to_compare.Equals(param_names, StructuralComparisons.StructuralEqualityComparer));
//                // Первый тест
//                param_names = Cmn.ExtractParameterNamesFromSQL(@"DECLARE
//  p_kod_dog NUMBER;
//BEGIN
//  /* Тест:2 */
//  p_kod_dog  := :p_kod_dog;
//  SELECT MIN(ym) INTO :p_ym FROM tnr_account WHERE kod_dog = p_kod_dog;
//END;");
//                Assert.IsTrue(params_to_compare.Equals(param_names, StructuralComparisons.StructuralEqualityComparer));
//            }
//            /*
//            [TestMethod]
//            [TestCategory("String")]
//            public void XDocumentToString()
//            {
//                XmlReports.SourceFolder = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
//                string xlsx_dir = Path.Combine(XmlReports.GetDefaultContentFolder(), "printTemplate", "excel");
//                //string root = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
//                //root = Path.Combine(root, "printTemplate", "excel");
//                string file_name = Path.Combine(xlsx_dir, "26630.xml");
//                FileInfo file_info = new FileInfo(file_name);
//                Debug.WriteLine("Длина файла: " + file_info.Length.ToString() + " байт");
//                //
//                long used_before = GC.GetTotalMemory(false);
//                XDocument doc = XDocument.Load(file_name);
//                long used_after = GC.GetTotalMemory(false);
//                Debug.WriteLine("XDocument.Load(): " + (used_after - used_before).ToString() + " байт");
//                //if (doc.Declaration.Encoding == null) {
//                //    doc.Declaration.Encoding = "utf-8";
//                //}
//                //
//                used_before = GC.GetTotalMemory(false);
//                string data = Cmn.XDocumentToString(doc, file_info.Length);
//                used_after = GC.GetTotalMemory(false);
//                Debug.WriteLine("Cmn.XDocumentToString(new): " + (used_after - used_before).ToString() + " байт");
//                Debug.WriteLine("Длина строки: " + data.Length.ToString() + " символов");
//                //
//                used_before = GC.GetTotalMemory(false);
//                string data_old = Cmn.XDocumentToString(doc);
//                used_after = GC.GetTotalMemory(false);
//                Debug.WriteLine("Cmn.XDocumentToString(old): " + (used_after - used_before).ToString() + " байт");
//                Debug.WriteLine("Длина строки: " + data_old.Length.ToString() + " символов");
//                Assert.AreEqual(data.Length, data_old.Length, "Длина строк отличается");
//                int index = data.Length - 1;
//                while (index >= 0) {
//                    Assert.AreEqual(data[index], data_old[index], "Символ в позиции " + index.ToString() + " отличается");
//                    index--;
//                }
//                // infoenergo.forms\meters\Reports\analiz_elpotr_okved_SQLB.xml
//            }*/
//        }
//    }
//    internal partial class Compiler
//    {
//        /// <summary>
//        /// Набор unit-тестов для класса <see cref="Compiler"/>
//        /// </summary>
//        [TestClass]
//        public class Test
//        {
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="Compiler.extendWhereByAnd(XElement)"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("Compiler")]
//            public void extendWhereByAnd1()
//            {
//                // Первый тест
//                XElement query = XElement.Parse(@"
//<query>
//  <select>
//    <column table='d' column='kod_dog' as='kod_dog' />
//    <column table='p' column='kodp' as='kodp' />
//    <column table='p' column='name' as='name' />
//  </select>
//  <from>
//    <table name='kr_payer'   as='p' />
//    <table name='kr_dogovor' as='d' />
//  </from>
//  <where>
//    <call function='='>
//      <column table='p' column='kodp' />
//      <column table='d' column='kodp' />
//    </call>
//  </where>
//</query>");
//                XElement result = XElement.Parse(@"
//<query>
//  <select>
//    <column table='d' column='kod_dog' as='kod_dog' />
//    <column table='p' column='kodp' as='kodp' />
//    <column table='p' column='name' as='name' />
//  </select>
//  <from>
//    <table name='kr_payer'   as='p' />
//    <table name='kr_dogovor' as='d' />
//  </from>
//  <where>
//    <call function='and'>
//      <call function='='>
//        <column table='p' column='kodp' />
//        <column table='d' column='kodp' />
//      </call>
//    </call>
//  </where>
//</query>");
//                XElement call_kodp = query.Element(EName.where).Element(EName.call);
//                XElement call = Compiler.extendWhereByAnd(query);
//                Assert.IsTrue(XNode.DeepEquals(query, result));
//                Assert.IsFalse(Object.ReferenceEquals(call, call_kodp));
//                Assert.IsTrue(Object.ReferenceEquals(call, query.Element(EName.where).Element(EName.call)));
//                // Второй тест
//                query = XElement.Parse(@"
//<query>
//  <select>
//    <column table='p' column='kodp' as='kodp' />
//    <column table='p' column='name' as='name' />
//  </select>
//  <from>
//    <table name='kr_payer'   as='p' />
//  </from>
//</query>");
//                result = XElement.Parse(@"
//<query>
//  <select>
//    <column table='p' column='kodp' as='kodp' />
//    <column table='p' column='name' as='name' />
//  </select>
//  <from>
//    <table name='kr_payer'   as='p' />
//  </from>
//  <where>
//    <call function='and' />
//  </where>
//</query>");
//                call = Compiler.extendWhereByAnd(query);
//                Assert.IsTrue(XNode.DeepEquals(query, result));
//                Assert.IsTrue(Object.ReferenceEquals(call, query.Element(EName.where).Element(EName.call)));
//            }
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="Compiler.extendWhereByAnd(XElement, XElement)"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("Compiler")]
//            public void extendWhereByAnd2()
//            {
//                XElement cond = XElement.Parse("<C />");
//                // Первый тест
//                XElement where = XElement.Parse(@"
//<where>
//  <call function='and'>
//    <A />
//    <B />
//  </call>
//</where>");
//                XElement result = XElement.Parse(@"
//<where>
//  <call function='and'>
//    <A />
//    <B />
//    <C />
//  </call>
//</where>");
//                where = Compiler.extendWhereByAnd(where, cond);
//                Assert.IsTrue(XNode.DeepEquals(where, result));
//                // Второй тест
//                where = XElement.Parse(@"
//<where>
//  <call function='or'>
//    <A />
//    <B />
//  </call>
//</where>");
//                result = XElement.Parse(@"
//<where>
//  <call function='and'>
//    <call function='or'>
//      <A />
//      <B />
//    </call>
//    <C />
//  </call>
//</where>");
//                where = Compiler.extendWhereByAnd(where, cond);
//                Assert.IsTrue(XNode.DeepEquals(where, result));
//            }
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="Compiler.changeChildAliases"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("Compiler")]
//            public void changeChildAliases()
//            {
//                XElement el = XElement.Parse(@"
//<query>
//  <select>
//    <column table='d' column='kod_dog' as='kod_dog' />
//    <column table='p' column='kodp' as='kodp' />
//    <column table='p' column='name' as='name' />
//  </select>
//  <from>
//    <query as='p'>
//      <select>
//        <column table='p' column='kodp' as='kodp' />
//        <column table='p' column='name' as='name' />
//      </select>
//      <from>
//        <table name='kr_payer' as='p' />
//      </from>
//    </query>
//    <query as='d'>
//      <select>
//        <column table='d' column='kod_dog' as='kod_dog' />
//        <column table='d' column='kodp' as='kodp' />
//      </select>
//      <from>
//        <table name='kr_dogovor' as='d' />
//      </from>
//      <call function='='>
//        <column table='d' column='kodp' />
//        <column table='p' column='kodp' />
//      </call>
//    </query> 
//  </from>
//</query>");
//                XElement result = XElement.Parse(@"
//<query>
//  <select>
//    <column table='d_x0' column='kod_dog' as='kod_dog' />
//    <column table='p_x0' column='kodp' as='kodp' />
//    <column table='p_x0' column='name' as='name' />
//  </select>
//  <from>
//    <query as='p_x0'>
//      <select>
//        <column table='p' column='kodp' as='kodp' />
//        <column table='p' column='name' as='name' />
//      </select>
//      <from>
//        <table name='kr_payer' as='p' />
//      </from>
//    </query>
//    <query as='d_x0'>
//      <select>
//        <column table='d' column='kod_dog' as='kod_dog' />
//        <column table='d' column='kodp' as='kodp' />
//      </select>
//      <from>
//        <table name='kr_dogovor' as='d' />
//      </from>
//      <call function='='>
//        <column table='d_x0' column='kodp' />
//        <column table='p_x0' column='kodp' />
//      </call>
//    </query> 
//  </from>
//</query>");
//                Compiler.changeChildAliases(el, "_x0");
//                Assert.IsTrue(XNode.DeepEquals(el, result));
//                //Assert.AreEqual<bool>(true, XNode.DeepEquals(el, result));
//            }
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="Compiler.extendWhere"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("Compiler")]
//            public void extendWhere()
//            {
//                XElement query = XElement.Parse(@"
//<query>
//  <select>
//    <column table='d' column='kod_dog' as='kod_dog' />
//  </select>
//  <from>
//    <table name='kr_payer'   as='p' />
//    <table name='kr_dogovor' as='d' />
//  </from>
//  <where>
//    <call function='='>
//      <column table='p' column='kodp' />
//      <column table='d' column='kodp' />
//    </call>
//  </where>
//</query>");
//                XElement query_call = XElement.Parse(@"
//<query>
//  <extendwhere target='p'>
//    <A />
//    <B />
//  </extendwhere>
//</query>
//");
//                XElement result_query = XElement.Parse(@"
//<query>
//  <select>
//    <column table='d' column='kod_dog' as='kod_dog' />
//  </select>
//  <from>
//    <table name='kr_payer'   as='p'>
//      <extendwhere>
//        <call function='and'>
//          <A />
//          <B />
//        </call>
//      </extendwhere>
//    </table>
//    <table name='kr_dogovor' as='d' />
//  </from>
//  <where>
//    <call function='='>
//      <column table='p' column='kodp' />
//      <column table='d' column='kodp' />
//    </call>
//  </where>
//</query>");
//                XElement result_query_call = XElement.Parse(@"<query/>");
//                Compiler.extendWhere(query, query_call);
//                Assert.IsTrue(XNode.DeepEquals(query, result_query));
//                Assert.IsTrue(XNode.DeepEquals(query_call, result_query_call));
//            }
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="Compiler.copyAddColInto(XElement)"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("Compiler")]
//            public void copyAddColInto()
//            {
//                XElement query = XElement.Parse(@"
//<query materialize='1'>
//  <select>
//    <column table='d' column='kod_dog' type='number' />
//    <column table='p' column='kodp'    type='number' />
//    <column table='p' column='nump'    type='string' />
//    <column table='d' column='dat_dog' type='date' />
//    <column table='d' column='dat_fin' type='date' />
//    <column table='p' column='name'    type='string' />
//  </select>
//  <from>
//    <query name='kr_payer' as='p'>
//      <select>
//        <column table='p' column='kodp' type='number' />
//        <column table='p' column='nump' type='string' />
//      </select>
//      <from>
//        <table name='kr_payer' as='p' />
//      </from>
//    </query>
//    <query name='kr_dogovor' as='d' materialize='1'>
//      <select>
//        <column table='d' column='kod_dog' type='number' />
//        <column table='d' column='kodp' type='number' />
//        <column table='d' column='dat_dog' type='date' />
//        <column table='d' column='dat_fin' type='date' />
//      </select>
//      <from>
//        <table name='kr_dogovor' as='d' />
//      </from>
//    </query>
//  </from>
//  <where>
//    <call function='='>
//      <column table='p' column='kodp' />
//      <column table='d' column='kodp' />
//    </call>
//  </where>
//</query>");
//                XElement result = XElement.Parse(@"
//<query materialize='1'>
//  <select>
//    <column table='d' column='kod_dog' type='number' into='n1' />
//    <column table='p' column='kodp'    type='number' into='n2' />
//    <column table='p' column='nump'    type='string' into='s1' />
//    <column table='d' column='dat_dog' type='date'   into='d1' />
//    <column table='d' column='dat_fin' type='date'   into='d2' />
//    <column table='p' column='name'    type='string' into='s2' />
//  </select>
//  <from>
//    <query name='kr_payer' as='p'>
//      <select>
//        <column table='p' column='kodp' type='number' />
//        <column table='p' column='nump' type='string' />
//      </select>
//      <from>
//        <table name='kr_payer' as='p' />
//      </from>
//    </query>
//    <query name='kr_dogovor' as='d' materialize='1'>
//      <select>
//        <column table='d' column='kod_dog' type='number' into='n1' />
//        <column table='d' column='kodp' type='number' into='n2' />
//        <column table='d' column='dat_dog' type='date' into='d1' />
//        <column table='d' column='dat_fin' type='date' into='d2' />
//      </select>
//      <from>
//        <table name='kr_dogovor' as='d' />
//      </from>
//    </query>
//  </from>
//  <where>
//    <call function='='>
//      <column table='p' column='kodp' />
//      <column table='d' column='kodp' />
//    </call>
//  </where>
//</query>");
//                Compiler.copyAddColInto(query);
//                Assert.IsTrue(XNode.DeepEquals(query, result));
//            }
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="Compiler.applyParamToAttr(ref string, XElement, XElement)"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("Compiler")]
//            [TestCategory("String")]
//            public void applyParamToAttr()
//            {
//                XElement actual_params = null;
//                XElement formal_params = XElement.Parse(@"
//<params>
//  <param name='kodp' type='number'>
//    <const>1000674518</const>
//  </param>
//  <param name='nump' type='string'>
//    <const>'10006745183'</const>
//  </param>
//  <param name='name' type='string'>
//    <const>'Воропаева Марина Константиновна'</const>
//  </param>
//</params>");
//                // Первый тест
//                string attr_value = "Абонент [:nump] [:name] (kodp = [:kodp])";
//                string result = "Абонент 10006745183 Воропаева Марина Константиновна (kodp = 1000674518)";
//                Compiler.applyParamToAttr(ref attr_value, actual_params, formal_params);
//                Assert.AreEqual(attr_value, result);
//                // Второй тест
//                attr_value = ":name";
//                //result = "'Воропаева Марина Константиновна'"; // Старая реализация
//                result = "Воропаева Марина Константиновна"; // Новая реализация
//                Compiler.applyParamToAttr(ref attr_value, actual_params, formal_params);
//                Assert.AreEqual(attr_value, result);
//                // Третий тест
//                formal_params = XElement.Parse(@"
//<params>
//  <param name='payer'>
//    <data kodp='1000674518' nump='10006745183' name='Воропаева Марина Константиновна' />
//  </param>
//</params>");
//                attr_value = "Абонент [:payer.nump] [:payer.name] (kodp = [:payer.kodp])";
//                result = "Абонент 10006745183 Воропаева Марина Константиновна (kodp = 1000674518)";
//                Compiler.applyParamToAttr(ref attr_value, actual_params, formal_params);
//                Assert.AreEqual(attr_value, result);
//                // Четвёртый тест
//                attr_value = ":payer.name";
//                result = "Воропаева Марина Константиновна";
//                Compiler.applyParamToAttr(ref attr_value, actual_params, formal_params);
//                Assert.AreEqual(attr_value, result);
//                // Пятый тест
//                actual_params = XElement.Parse(@"
//<params>
//  <const>1000674518</const>
//  <const>'10006745183'</const>
//  <const>'Воропаева Марина Константиновна'</const>
//</params>
//");
//                formal_params = XElement.Parse(@"
//<params>
//  <param name='kodp' type='number' />
//  <param name='nump' type='string' />
//  <param name='name' type='string' />
//</params>");
//                attr_value = "Абонент [:nump] [:name] (kodp = [:kodp])";
//                result = "Абонент 10006745183 Воропаева Марина Константиновна (kodp = 1000674518)";
//                Compiler.applyParamToAttr(ref attr_value, actual_params, formal_params);
//                Assert.AreEqual(attr_value, result);
//                // Шестой тест
//                attr_value = ":name";
//                //result = "'Воропаева Марина Константиновна'"; // Старая реализация
//                result = "Воропаева Марина Константиновна"; // Новая реализация
//                Compiler.applyParamToAttr(ref attr_value, actual_params, formal_params);
//                Assert.AreEqual(attr_value, result);
//                // Седьмой тест
//                formal_params = XElement.Parse(@"
//<params>
//  <param name='payer' />
//</params>");
//                actual_params = XElement.Parse(@"
//<params>
//  <data kodp='1000674518' nump='10006745183' name='Воропаева Марина Константиновна' />
//</params>
//");
//                attr_value = "Абонент [:payer.nump] [:payer.name] (kodp = [:payer.kodp])";
//                result = "Абонент 10006745183 Воропаева Марина Константиновна (kodp = 1000674518)";
//                Compiler.applyParamToAttr(ref attr_value, actual_params, formal_params);
//                Assert.AreEqual(attr_value, result);
//                // Восьмой тест
//                attr_value = ":payer.name";
//                result = "Воропаева Марина Константиновна";
//                Compiler.applyParamToAttr(ref attr_value, actual_params, formal_params);
//                Assert.AreEqual(attr_value, result);
//            }
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="Compiler.normalizeWhitespace(string)"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("String")]
//            public void normalizeWhitespace()
//            {
//                string s = "SELECT   1,  2,    3,     4\r\n\r\n\r\nINTO    n1,  n2,    n3,     n4\r\r\rFROM       dual\n\n\nWHERE 1   = 1";
//                string result = "SELECT 1, 2, 3, 4\r\nINTO n1, n2, n3, n4\rFROM dual\nWHERE 1 = 1";
//                s = Compiler.normalizeWhitespace(s);
//                Assert.AreEqual(s, result);
//            }
//            /*
//            [TestMethod]
//            [TestCategory("String")]
//            public void eFunction()
//            {
//                if (Global.Connection == null) {
//                    Global.Connect("ASUSE", "kl0pik", "ASUSE.WORLD");
//                }
//                if (XmlReports.SourceFolder == null) {
//                    XmlReports.SourceFolder = @"C:\infoenergo\root\main\all\sql.builder\bin\x86\Debug";
//                }
//                XmlReports.Init(false, null);
//                //VEnvironment env = XmlReports.Environment;
//                XElement element = XElement.Parse(@"
//<call function='='>
//  <column table='kr_dogovor' column='kodp' />
//  <column table='kr_payer' column='kodp' />
//</call>");
//                XElement ret = XElement.Parse(@"<call function='=' />");
//                XElement result = XElement.Parse(@"
//<call function='=' pth='' type='bool'>
//  <column table='kr_dogovor' column='kodp' />
//  <text txtype='func'>          =  </text>
//  <column table='kr_payer' column='kodp' />
//</call>");
//                Compiler.eFunction(element, ret, null);
//                Assert.IsTrue(XNode.DeepEquals(ret, result));
//            }*/
//        }
//    }
//#endif
//}
