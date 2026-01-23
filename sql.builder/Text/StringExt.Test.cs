//using System;
//using System.Collections.Generic;
//using Microsoft.VisualStudio.TestTools.UnitTesting;

//namespace sql.builder
//{
//#if DEBUG
//    internal partial class StringExt
//    {
//        /// <summary>
//        /// Набор unit-тестов для класса <see cref="Cmn"/>
//        /// </summary>
//        [TestClass]
//        public class Test
//        {
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="StringExt.SubstringBefore(string, char)"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("String")]
//            public void SubstringBefore()
//            {
//                string test_string = ":a.b.c";
//                string result = StringExt.SubstringBefore(test_string, ':');
//                Assert.AreEqual(result, string.Empty);
//                result = StringExt.SubstringBefore(test_string, '.');
//                Assert.AreEqual(result, ":a");
//                result = StringExt.SubstringBefore(test_string, ';');
//                Assert.AreEqual(result, test_string);
//            }
//            /// <summary>
//            /// Метод для unit-тестирования <see cref="StringExt.SubstringAfter(string, char)"/>
//            /// </summary>
//            [TestMethod]
//            [TestCategory("String")]
//            public void SubstringAfter()
//            {
//                string test_string = "a.b.c:";
//                string result = StringExt.SubstringAfter(test_string, ':');
//                Assert.AreEqual(result, string.Empty);
//                result = StringExt.SubstringAfter(test_string, '.');
//                Assert.AreEqual(result, "c:");
//                result = StringExt.SubstringAfter(test_string, ';');
//                Assert.AreEqual(result, test_string);
//            }
//        }
//    }
//#endif
//}
