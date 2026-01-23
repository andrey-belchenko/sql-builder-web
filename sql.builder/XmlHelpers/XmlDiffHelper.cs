//using System.IO;
//using System.Text;
//using System.Xml;
//using System.Xml.Linq;
//using Microsoft.XmlDiffPatch;

//namespace sql.builder.XmlHelpers
//{
//    internal static class XmlDiffHelper
//    {
//        public static XElement GetDiff(XElement xmlOld, XElement xmlNew)
//        {
//            using(var memoryBuffer = new MemoryStream())
//            {
//                using (var xmlWriter = new XmlTextWriter(memoryBuffer, Encoding.UTF8))
//                {
//                    var xmlDiff = new XmlDiff(XmlDiffOptions.None);
//                    xmlDiff.Algorithm = XmlDiffAlgorithm.Precise;
//                    xmlDiff.Compare(xmlOld.CreateReader(ReaderOptions.None), xmlNew.CreateReader(ReaderOptions.None), xmlWriter);
//                    memoryBuffer.Seek(0, SeekOrigin.Begin);
//                    var xdiff = XElement.Load(memoryBuffer);

//                    return xdiff;
//                }
//            }
//        }

//        public static XElement ResoreXml(XElement xml, XElement xmlDiff)
//        {
//            var xmlPatch = new XmlPatch();
//            using (var memoryBuffer = new MemoryStream())
//            {
//                xmlPatch.Patch(xml.CreateReader(ReaderOptions.None), memoryBuffer, xmlDiff.CreateReader());
//                memoryBuffer.Seek(0, SeekOrigin.Begin);
//                var xmlNew = XElement.Load(memoryBuffer);

//                return xmlNew;
//            }
//        }
//    }
//}