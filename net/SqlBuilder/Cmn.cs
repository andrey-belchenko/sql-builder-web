using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
//using System.Net.Http;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
////using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
//using DevExpress.Compression;
//using DevExpress.LookAndFeel;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Repository;
//using DevExpress.XtraGrid;
//using DevExpress.XtraGrid.Views.Grid;
//using DevExpress.XtraRichEdit;
//using DevExpress.XtraTreeList;
//using TreeListNode = DevExpress.XtraTreeList.Nodes.TreeListNode;
using Microsoft.Win32;
using sql.builder.DataApi;
using sql.builder.XmlHelpers;
using System.Text.RegularExpressions;
//using DevExpress.XtraBars;
//using infoenergo.core.Extensions;
using sql.builder.UI;
using sql.builder.Clean;
// Cross-platform: Drawing2D and Imaging are Windows-only, commented out
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
using System.Security.Principal;
//using DevExpress.Skins;
//using DevExpress.XtraEditors.Controls;
using infoenergo.core.Data;
//using Microsoft.Office.Interop.Excel;
//using sql.builder.TFS;
using sql.builder.WinForms;
//using Application = System.Windows.Forms.Application;
using DataTable = System.Data.DataTable;
using Rectangle = System.Drawing.Rectangle;
//using Resources = infoenergo.ui.resources.Properties.Resources;
//using sql.builder.Properties;
namespace sql.builder
{
    public static partial class Cmn
    {
        #region object-константы (во избежания лишнего боксинга)
        public static readonly object DECIMAL_MINUS_ONE = (object)decimal.MinusOne;
        public static readonly object DECIMAL_ZERO = (object)decimal.Zero;
        public static readonly object DECIMAL_ONE = (object)decimal.One;
        public static readonly object DECIMAL_TWO = (object)2M;
        public static readonly object DECIMAL_THREE = (object)3M;
        public static readonly object DECIMAL_FOUR = (object)4M;
        public static readonly object INT32_MINUS_ONE = (object)-1;
        public static readonly object INT32_ZERO = (object)0;
        public static readonly object INT32_ONE = (object)1;
        public static readonly object INT32_TWO = (object)2;
        public static readonly object INT32_THREE = (object)3;
        public static readonly object BOOLEAN_FALSE = (object)false;
        public static readonly object BOOLEAN_TRUE = (object)true;
        #endregion
        public static string[] SplitString(string s)
        {
            var splitChars = new string[] {" ", ","};
            var ss = s.Split(splitChars,StringSplitOptions.None).Where(s1=>!string.IsNullOrEmpty(s1)).ToArray();
            return ss;
        }
        public static string OpenText(string filename)
        {
            using (var reader = new StreamReader(filename)) {
                return reader.ReadToEnd();
            }
        }
        public static void CreateFolder(string name)
        {
            string[] ss = name.Split('\\');
            var name1 = "";
            for (int i = 0; i < ss.Length - 1; i++) {
                var s = ss[i];
                name1 += s + "\\";
                if (!Directory.Exists(name1)) {
                    Directory.CreateDirectory(name1);
                }
            }
        }
        public static void SaveTextWithCheckOut(string text, string filename)
        {
        }
     
        public static void SaveText(string text, string filename, Encoding encoding)
        {
            //try {
            //    using (var sw = new StreamWriter(new FileStream(filename, FileMode.Create), encoding)) {
            //        sw.Write(text);
            //        sw.Close();
            //    }
            //} catch {
            //    XtraMessageBox.Show("Не удалось сохранить изменения.", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            //}
        }
        public static string GetAttrValue(XAttribute attr)
        {
            string ret = "";
            if (attr != null)
            {
                ret = attr.Value;
            }
            return ret;
        }

        public static bool IsGreater(object obj1, object obj2)
        {
            if (IsNullOrDBNull(obj1)) {
                return false;
            } else if (IsNullOrDBNull(obj2)) {
                return true;
            } else if (obj1 is decimal) {
                return Convert.ToDecimal(obj1) > Convert.ToDecimal(obj2);
            } else if (obj1 is int) {
                return Convert.ToInt32(obj1) > Convert.ToInt32(obj2);
            } else if (obj1 is DateTime) {
                return (DateTime)(obj1) > (DateTime)(obj2);
            } else {
                return (obj1.ToString().CompareTo(obj2.ToString()) > 0);
            }
        }
        public static bool IsLess(object obj1, object obj2)
        {
            if (IsNullOrDBNull(obj1)) {
                return false;
            } else if (IsNullOrDBNull(obj2)) {
                return true;
            } else if (obj1 is decimal) {
                return Convert.ToDecimal(obj1) < Convert.ToDecimal(obj2);
            } else if (obj1 is int) {
                return Convert.ToInt32(obj1) < Convert.ToInt32(obj2);
            } else if (obj1 is DateTime) {
                return (DateTime)(obj1) < (DateTime)(obj2);
            } else {
                return obj1.ToString().CompareTo(obj2.ToString()) < 0;
            }
        }
        /*public static void SetAttrNotEmpty(XElement el, string attributeName, string value)
        {
            if (string.IsNullOrEmpty(value)) {
                el.RemoveAttribute(attributeName);
            } else {
                el.SetAttributeValue(attributeName, value);
            }
        }*/
        public static string GetAttrValue(XElement el, string attrName)
        {
            return el.AttrOrEmpty(attrName);
        }
        public static string GetAttrValueNvl(XElement el, string attrName1, string attrName2)
        {
            string ret = "";
            XAttribute attr = el.Attribute(attrName1);
            if (attr == null)
            {
                attr = el.Attribute(attrName2);
            }
            if (attr != null)
            {
                ret = attr.Value;
            }
            return ret;
        }

        public static void CopyAttributeNotEmpty(XElement src, XElement tag, XName name)
        {
            if (src.Attribute(name) != null)
            {
                if (src.Attribute(name).Value == "")
                {
                    tag.Attributes(name).Remove();
                }
                else
                {
                    tag.SetAttributeValue(name, src.Attribute(name).Value);
                }
            }

        }
        // = XmlReports.copyAttribute()
        public static void CopyAttribute(XElement src, XElement dest, XName name)
        {
            Contract.Assert(src != null);
            XAttribute attr = src.Attribute(name);
            if (attr != null) {
                dest.SetAttributeValue(name, attr.Value);
            }
        }
        // = XmlReports.copyAttributeNoReplace()
        public static void CopyAttributeNoReplace(XElement src, XElement tag, XName name)
        {
            XAttribute attr = src.Attribute(name);
            if (attr != null && tag.Attribute(name) == null) {
                tag.Add(new XAttribute(name, attr.Value));
            }
        }
        public static void copyAttributes(XElement src, XElement tag)
        {
            if (src == null) return;
            tag.Add(src.Attributes().Select(at => new XAttribute(at.Name.LocalName, at.Value)));
        }
        public static void CopyAttributesNoReplace(XElement src, XElement tag)
        {
            foreach (XAttribute attr in src.Attributes()) {
                if (tag.Attribute(attr.Name) == null) {
                    tag.Add(new XAttribute(attr));
                }
            }
        }
        public static decimal ToDecimal(object val)
        {
            if (IsNullOrDBNull(val)) {
                return decimal.Zero;
            }
            if (val is decimal) {
                return (decimal)val;
            }
            if (val is int) {
                return Convert.ToDecimal(val);
            }
            if (string.Empty.Equals(val)) {
                return Decimal.Zero;
            }
            return (decimal)ToDecimal(val.ToString());
        }

        public static decimal NumToDecimal(object val)
        {
            if (val is decimal)
            {
                return (decimal)val;
            }

            if (val is int)
            {
                return Convert.ToDecimal(val);
            }

            return 0;

        }

        public enum CheckResultTFU
        {
            True, False, Uncknown
        }

        public static CheckResultTFU IsDecimal(string str)
        {
            var a = XmlReports.numberType;
            if (str == "null")
            {
                return CheckResultTFU.Uncknown;
            }
            if (str == "")
            {
                return CheckResultTFU.Uncknown;
            }
            var ci = new CultureInfo("ru-ru");
            var sps = new[] { ',', '.' };
            var sp1 = str.IndexOfAny(sps);
            var sp2 = str.LastIndexOfAny(sps);
            if (sp1 >= 0)
            {
                ci.NumberFormat.NumberDecimalSeparator = str.Substring(sp2, 1);
            }
            if (sp2 >= 0 && sp1 != sp2)
            {
                ci.NumberFormat.NumberGroupSeparator = str.Substring(sp1, 1);
            }
            bool sucsess = true;
            try
            {
                Convert.ToDecimal(str, ci);
            }
            catch (FormatException)
            {
                sucsess = false;
            }
            if (sucsess)
            {
                return CheckResultTFU.True;
            }
            else
            {
                return CheckResultTFU.False;
            }
        }

        public static object ToDecimal(string str)
        {
            var a = XmlReports.numberType;
            if (str == "null")
            {
                return DBNull.Value;
            }
            if (str == "")
            {
                return DBNull.Value;
            }
            var ci = new CultureInfo("ru-ru");
            var sps = new[] { ',', '.' };
            var sp1 = str.IndexOfAny(sps);
            var sp2 = str.LastIndexOfAny(sps);
            if (sp1 >= 0)
            {
                ci.NumberFormat.NumberDecimalSeparator = str.Substring(sp2, 1);
            }
            if (sp2 >= 0 && sp1 != sp2)
            {
                ci.NumberFormat.NumberGroupSeparator = str.Substring(sp1, 1);
            }
            return Convert.ToDecimal(str, ci);
        }

        public static object ToObject(string value)
        {
            value = value.Trim('\'');

            if (value == "true" || value == "false")
            {
                return Convert.ToBoolean(value);
            }
            else
            {
                return Convert.ToInt32(value);
            }
        }

        public static bool IsNumeric(object s)
        {
            float output;
            return Single.TryParse(s.ToString(), out output);
        }

        public class VStringParamName// чтобы отличать строку от имени рараметра и не заключать его в кавычки
        {
            public VStringParamName(string value)
            {
                _value = value;
            }
            private string _value;
            public override string ToString()
            {
                return _value;
            }
        }
        public static string ToOracleString(object val)
        {
            if (IsNullOrDBNull(val)) {
                return "null";
            }
            Type type = val.GetType();
            string sval = val.ToString();
            if (type == XmlReports.numberType) {
                return sval.Replace(",", ".");
            }
            if (type == typeof(string)) {
                // Емцов - параметры массивы типа string уже обернуты в кавычки
                if (sval.Length > 0 && sval[0] != '\'') {
                    return "'" + sval + "'";                    
                } else {
                    return sval;
                }
            }
            if (type == typeof(DateTime)) {
                return String.Format("to_date('{0}','DD.MM.YYYY')", ((DateTime)val).ToString("dd.MM.yyyy"));
            }
            //if (type == typeof(VStringParamName)) {
            //    return sval;
            //}
            return sval;
        }        
        public static object EvaluateOracleConst(string val)
        {
            if (val.Contains("to_date"))
            {
                return ExtractDateFromOracleToDateString(val);
            }

            if (val.Contains("'"))
            {
                return ExtractStringFromOracleString(val);
            }

            if (val.Contains("null"))
            {
                return ExtractStringFromOracleString(val);
            }
            return ToDecimal(val);
        }

        public static object ExtractDateFromOracleToDateString(string val)
        {

            CultureInfo provider = CultureInfo.InvariantCulture;
            string sd = val.Replace("to_date('", "").Replace("','DD.MM.YYYY')", "");

            if (sd == "null")
            {
                return DBNull.Value;
            }
            DateTime d = DateTime.ParseExact(sd.Substring(0, 10), "dd.MM.yyyy", provider);
            return d;
        }

        public static object ExtractStringFromOracleString(string val)
        {

            if (val == "null")
            {
                return DBNull.Value;
            }
            if (val.StartsWith("'"))
            {
                val = val.Substring(1, val.Length - 2);
            }

            return val;
        }

        public static string CutString(string caption, int max_size = 30)
        {
            return (caption.Length <= max_size)
                ? caption
                : String.Format("{0}...{1}",
                    caption.Substring(0, max_size / 2 - 2),
                    caption.Substring(caption.Length - (max_size / 2 - 2), max_size / 2 - 2));
        }
        public static void setParams(XElement formalParams, XElement factParams, bool useDefaults)
        {
            if (formalParams == null) return;
            if (factParams == null) {
                factParams = new XElement(EName.globalparams);
            }
            foreach (XElement formalParam in formalParams.Elements()) {
                string param_name = formalParam.Attribute(AName.name).Value;
                XElement factParam = factParams.Elements().SearchByAttribute(AName.name, param_name);
                if (factParam != null) {
                    formalParam.Elements().Remove();
                    Compiler.copyContent(factParam, formalParam);
                } else if (useDefaults) {
                    formalParam.Elements().Remove();
                    formalParam.Add(new XElement(EName.undefined));
                } else {
                    formalParam.Elements().Remove();
                    formalParam.Add(new XElement(EName.undefined));
                }
            }
        }
        // Cross-platform: Image/Bitmap are Windows-only, replaced with object
        private static object _imageWarning14;
        private static object _imageEdit12;
        //private static object _imageCheck12;



        public static object ImageWarning14
        {
            get
            {
                if (_imageWarning14 == null)
                {
                    //_imageWarning14 = ResizeImage(infoenergo.ui.resources.Properties.Resources.Warning_16, 14, 14);
                }
                return _imageWarning14;
            }
        }

        public static object ImageEdit12
        {
            get
            {
                if (_imageEdit12 == null)
                {
            
                    //_imageEdit12 = Properties.Resources.cell_edit.ToBitmap();
                }
                return _imageEdit12;
            }
            
        }
        /*public static Image ImageCheck12
        {
            get
            {
                if (_imageCheck12 == null)
                {
                    //_imageCheck12 = new Bitmap(Application.StartupPath + @"\sql.builder\doc\check.ico");
                    _imageCheck12 = Properties.Resources.check.ToBitmap();
                }
                return _imageCheck12;
            }
        }
        private static Image _imageCommitAndClose24 = null;
        public static Image ImageCommitAndClose24
        {
            get
            {
                if (_imageCommitAndClose24 == null)
                {
                    //_imageCommitAndClose24 = new Bitmap(Application.StartupPath + @"\sql.builder\doc\CommitAndClose_24.png");
                    _imageCommitAndClose24 = Properties.Resources.CommitAndClose_24;
                }
                return _imageCommitAndClose24;
            }
        }*/
        //public static Image ImageWarning16 = GetIcon(TextConst.Images.Warning16);

        // Cross-platform: Bitmap/Graphics/ImageAttributes are Windows-only (System.Drawing.Common)
        // This method is not used (only called in commented code)
        /*
        public static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }
        */

        public const string undefinedString = "$undefined$";
        private static string undefNvluConst = "/*nvlu*/ $undefined$";
        public static string ClearUndefined(string s, out bool undefined_without_brace)
        {
            undefined_without_brace = false;
            s = s.Replace(undefNvluConst, "null");
            int i1 = s.IndexOf(undefinedString);
            while (i1 >= 0) {
                int i0 = s.LastIndexOf('{', i1);
                if (i0 < 0) {
                    undefined_without_brace = true;
                    s = s.Substring(0, i1) + "null" + s.Substring(i1 + undefinedString.Length);
                } else {
                    int i2 = s.IndexOf('}', i1);
                    s = s.Remove(i0, i2 - i0);
                }
                i1 = s.IndexOf(undefinedString);
            }
            s = s.Replace("{", "");
            s = s.Replace("}", "");
            //s = s.Replace(" and  (    )", " ");
            //s = s.Replace(" or  (    )", " ");
            s = s.Replace('\r', ' ');
            return s;
        }
        public static string ClearUndefined(string s)
        {
            s = s.Replace(undefNvluConst, "null");
            //Протестировать производительность, оптимизировать
            int i1 = s.IndexOf(undefinedString);
            while (i1 >= 0) {
                int i0 = s.LastIndexOf('{', i1);
                if (i0 < 0) {
                    return "";
                }
                int i2 = s.IndexOf('}', i1);
                s = s.Remove(i0, i2 - i0);
                i1 = s.IndexOf(undefinedString);
            }
            s = s.Replace("{", "");
            s = s.Replace("}", "");
            //s = s.Replace(" and  (    )", " ");
            //s = s.Replace(" or  (    )", " ");
            s = s.Replace('\r', ' ');
            return s;
        }
        public static string ClearSql(string sql)
        {
            if (sql == null) {
                return null;
            }
            return sql.Replace('\r', ' ');
        }
        public static void HtmlOutput(string content, string filename)
        {

            string fullName = Printing.GetFreeName(Path.GetTempPath(), filename, "html");


            File.WriteAllText(fullName, content);

            Process.Start(fullName);
        }
        /*public static void XmlOutput(XElement content, string filename)
        {
            string fullName = Printing.GetFreeName(Path.GetTempPath(), filename, "xml");
            File.WriteAllText(fullName, content.ToString());
            Process.Start(fullName);
        }*/
        public static void TxtOutput(string content, string filename)
        {

            string fullName = Printing.GetFreeName(Path.GetTempPath(), filename, "txt");


            File.WriteAllText(fullName, content);

            Process.Start(fullName);
        }

        public static void SqlOutput(string content)
        {

            //Cmn.SaveText(content, Settings.Default.testQueryS2, Encoding.Unicode);
            //Process.Start(Settings.Default.testQueryS2);
        }
        public static bool IsNullOrDBNull(object val)
        {
            return (val == null) || Convert.IsDBNull(val);
        }
        public static object Nvl(object v1, object v2)
        {
            if (IsNullOrDBNull(v1)) {
                return v2;
            } else {
                return v1;
            }
        }
        public static object Nvle(object v1, object v2)
        {
            if (IsNullOrDBNull(v1) || string.Empty.Equals(v1)) {
                return v2;
            } else {
                return v1;
            }
        }
        public static List<string> XElementsToDataTableSysFieldsNames = new List<string>(new[] { "elid", "pelid", "selid", "leaf", "ord", "lvl", "node_name", "node" });
        public static DataTable XElementsToDataTable(IEnumerable<XElement> elements, bool recursive = true, bool addNodeToTable = false)
        {
            IEnumerable<XElement> all_elements = recursive ? elements.DescendantsAndSelf() : elements;
            int i = 0;
            foreach (XElement node in all_elements) {
                string si = i.ToString();
                node.SetAttributeValue("elid", si);
                node.SetAttributeValue("ord", si);
                node.SetAttributeValue("leaf", node.HasElements ? TextConst.AVBool.False : TextConst.AVBool.True);
                node.SetAttributeValue("lvl", node.Ancestors().Count());
                i++;
            }
            foreach (XElement node in all_elements) {
                node.SetAttributeValue("pelid", node.Parent.AttrOrEmpty("elid"));
            }
            DataTable dt = new DataTable();
            dt.Columns.Add(new DataColumn("node_name", typeof(string)));
            if (addNodeToTable) {
                dt.Columns.Add(new DataColumn("node", typeof(XElement)));
            }
            var attrNames = all_elements.Attributes().Select(e => e.Name.LocalName).Distinct();
            foreach (string name in attrNames) {
                if (!dt.Columns.Contains(name)) {
                    Type type;
                    if (name == "ord" || name == "lvl") {
                        type = typeof(Int32);
                    } else {
                        type = typeof(string);
                    }
                    dt.Columns.Add(new DataColumn(name, type));
                }
            }
            //all_elements = recursive ? elements.DescendantsAndSelf() : elements;
            foreach (XElement node in all_elements) {
                DataRow row = dt.Rows.Add();
                row["node_name"] = node.Name.LocalName;
                foreach (XAttribute attr in node.Attributes()) {
                    row[attr.Name.LocalName] = attr.Value;
                }
                if (addNodeToTable) {
                    row["node"] = node;
                }
            }
            return dt;
        }
        public static List<XElement> DataTableToXElements(DataTable tbl)
        {
            List<XElement> elements = new List<XElement>();

            var rootRows = tbl.AsEnumerable().OrderBy(r => Convert.ToInt32(r["ord"])).Where(r1 => string.IsNullOrEmpty(r1["pelid"].ToString())).ToList();

            foreach (DataRow row in rootRows)
            {
                XElement element = DataRowToXElement(row);
                elements.Add(element);
                DataTableToXElements(element, tbl);
            }
            return elements;
        }

        // емцов - без проверки что pelid пустой
        public static List<XElement> DataTableToXElements2(DataTable tbl)
        {
            List<XElement> elements = new List<XElement>();
            var rows = tbl.AsEnumerable().OrderBy(r => Convert.ToInt32(r["ord"]));
            foreach (DataRow row in rows)
            {
                XElement element = DataRowToXElement(row);
                elements.Add(element);
                DataTableToXElements(element, tbl);
            }
            return elements;
        }

        private static void DataTableToXElements(XElement element, DataTable tbl)
        {

            foreach (DataRow childRow in tbl.AsEnumerable().OrderBy(r => Convert.ToInt32(r["ord"])).Where(r1 => r1["pelid"].ToString() == element.Attribute("elid").Value))
            {
                XElement child = DataRowToXElement(childRow);
                element.Add(child);
                DataTableToXElements(child, tbl);
            }

        }

        private static XElement DataRowToXElement(DataRow row)
        {

            XElement element = new XElement(row["node_name"].ToString());

            // Емцов - ошибка при редактировании колонок шаблона
            //foreach (DataColumn col in row.Table.Columns.Cast<DataColumn>().Where(c => !XElementsToDataTableSysFieldsNames.Contains(c.ColumnName.ToLower())))
            foreach (DataColumn col in row.Table.Columns)
            {
                if (row[col].ToString() != "")
                {
                    element.Add(new XAttribute(col.ColumnName.ToLower(), row[col].ToString()));
                }
            }
            return element;

        }
        /*
        /// <summary>
        /// Возвращает строку, содержащую XML-документ <paramref name="doc"/>.
        /// В отличии от <see cref="XDocument.ToString(SaveOptions)"/> с параметром SaveOptions.DisableFormatting
        /// в начале строки возвращает заголовок &lt;?xml version="1.0" encoding="utf-8"?&gt;
        /// </summary>
        /// <param name="doc">XML-документ</param>
        /// <param name="file_size">размер файла, из которого был загружен документ. Используется для прогнозирования размера выходной строки
        /// </param>
        /// <returns>строка, содержащая XML-документ</returns>
        public static string XDocumentToString(XDocument doc, long file_size = 0)
        {
            long estimated_length;
            if (file_size <= 0) {
                estimated_length = 32 * 1024;
            } else {
                estimated_length = file_size + (file_size >> 5);    // Для SaveOptions.DisableFormatting длина строки оказывается примерно в 1,03125 раза больше длины файла
                //estimated_length = file_size + (file_size >> 1); // Для SaveOptions.None длина строки оказывается примерно в полтора раза больше длины файла
                if (estimated_length > Int32.MaxValue) {
                    estimated_length = Int32.MaxValue;
                }
            }
            string data;
            using (StringWriter writer = new StringWriterUTF8(new StringBuilder((int)estimated_length))) {
                doc.Save(writer, SaveOptions.None); // DisableFormatting
                writer.Close();
                data = writer.ToString();
                writer.Dispose();
            }
            return data;
        }*/
        /*public static string XDocumentToString(XDocument doc)
        {
            MemoryStream stream = new MemoryStream();
            doc.Save(stream);
            stream.Flush();
            stream.Position = 0;
            StreamReader sr = new StreamReader(stream);
            string str = sr.ReadToEnd();
            sr.Close();
            sr.Dispose();
            sr = null;
            stream.Dispose();
            stream = null;
            return str;
        }*/
        public static object GetProperty(object obj, string fieldName, object def)
        {

            string[] path = fieldName.Split('.');
            int i;

            for (i = 0; i < path.Length - 1; i++)
            {
                PropertyInfo pi = null;
                /*try
                {
                    pi = obj.GetType().GetProperty(path[i]);
                }
                finally
                {

                }*/
                PropertyInfo[] pis = obj.GetType().GetProperties(); // !!! Переписать, так медленно?
                for (int i1 = 0; i1 < pis.Length; i1++)
                {
                    pi = pis[i1];
                    if (pi.Name.Equals(path[i]))
                    {
                        i1 = pis.Length;
                    }
                }
                obj = pi.GetValue(obj, null);

            }
            //obj.GetType().GetProperty(path[i]).SetValue(obj, value, null);
            PropertyInfo pi1 = obj.GetType().GetProperty(path[i]);
            if (pi1 != null)
            {
                object value = pi1.GetValue(obj, null);
                return value;
            }
            else
            {

                return def;
            }
        }
        public static void FocusFile(string path)
        {

            System.Diagnostics.Process.Start("explorer.exe", @"/select, " + path);

        }

        public static object GetProperty(object obj, string fieldName)
        {

            string[] path = fieldName.Split('.');
            int i;

            for (i = 0; i < path.Length - 1; i++)
            {
                PropertyInfo pi = null;
                /*try
                {
                    pi = obj.GetType().GetProperty(path[i]);
                }
                finally
                {

                }*/
                PropertyInfo[] pis = obj.GetType().GetProperties(); // !!! Переписать, так медленно?
                for (int i1 = 0; i1 < pis.Length; i1++)
                {
                    pi = pis[i1];
                    if (pi.Name.Equals(path[i]))
                    {
                        i1 = pis.Length;
                    }
                }
                obj = pi.GetValue(obj, null);

            }

            var flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;
            //obj.GetType().GetProperty(path[i]).SetValue(obj, value, null);
            PropertyInfo pi1 = obj.GetType().GetProperty(path[i], flags);
            if (pi1 != null)
            {
                object value = pi1.GetValue(obj, null);
                return value;
            }
            else
            {
                return obj.GetType().GetField(path[i], flags).GetValue(obj);
            }
        }
        public static void SetProperty(object obj, string fieldName, object value)
        {
            if (IsNullOrDBNull(value)) {
                value = null;
            }
            string[] path = fieldName.Split('.');
            int i;

            for (i = 0; i < path.Length - 1; i++)
            {
                PropertyInfo pi = null;
                /*try
                {
                    pi = obj.GetType().GetProperty(path[i]);
                }
                finally
                {

                }*/
                PropertyInfo[] pis = obj.GetType().GetProperties();
                for (int i1 = 0; i1 < pis.Length; i1++)
                {
                    pi = pis[i1];
                    if (pi.Name.Equals(path[i]))
                    {
                        i1 = pis.Length;
                    }
                }
                obj = pi.GetValue(obj, null);

            }
            if (obj.GetType().GetProperty(path[i]) != null)
            {

                obj.GetType().GetProperty(path[i]).SetValue(obj, value, null);
            }
            else
            {
                obj.GetType().GetField(path[i]).SetValue(obj, value);
            }


        }
        public static bool IsNull(object val)
        {
            return val == null || val == DBNull.Value || string.Empty.Equals(val);
        }
        public static XDocument OpenXmlClearNS(string fileName)
        {
			try
			{
				return XDocument.Parse(OpenText(fileName).Replace("xmlns=\"sqlbuilder\"", ""));
			}
			catch (XmlException ex)
			{
				throw new System.Exception("Ошибка при разборе xml файла " + fileName + " ." + Environment.NewLine  + ex.Message);
			}
        }
        /// <summary>
        /// Сохраняет XML-документ <paramref name="node"/> в файл <paramref name="file_name"/>
        /// и делает check out на TFS
        /// </summary>
        /// <param name="node">XML-документ или элемент</param>
        /// <param name="file_name">наименование файла</param>
        public static void SaveXmlWithCheckOut(XNode node, string file_name)
        {
        }
        public static VDataTable CopyTableStructure(VDataTable source)
        {
            VDataTable target = new VDataTable();
            CopyTableStructure(source, target);
            return target;
        }
        private static void CopyTableStructure(VDataTable source, VDataTable target)
        {
            foreach (DataColumn col in source.Columns)
            {
                DataColumn col1 = new DataColumn(col.ColumnName, col.DataType);
                col1.Caption = col.Caption;
                target.Columns.Add(col1);
            }
        }
        public static void CopyTable(VDataTable source, VDataTable target)
        {
            target.ClearColumns();
            target.Clear();
            List<DataColumn> pk = new List<DataColumn>();
            foreach (DataColumn col in source.Columns)
            {
                DataColumn col1 = new DataColumn(col.ColumnName, col.DataType);
                col1.Caption = col.Caption;
                target.Columns.Add(col1);
                foreach (DataColumn colpk in source.PrimaryKey)
                {

                    if (colpk.ColumnName == col1.ColumnName)
                    {
                        pk.Add(col1);
                    }

                }

            }
            target.PrimaryKey = pk.ToArray();




            target.Merge(source);

        }
        public static XElement GetFakeGlobalParams()
        {
            return XElement.Parse("<params><param name=\"dep\"><const>null</const></param><param name=\"tep_el\"><const>1</const></param></params>");
        }

        #region Реестр. Запись и чтение
        private static RegistryKey getRegistryKey(string path)
        {
            var _reg_path = String.Format(@"{0}\sql.builder\{1}", Environment.UserName, path);
            return Registry.CurrentUser.CreateSubKey(_reg_path, RegistryKeyPermissionCheck.ReadWriteSubTree);
        }

        /*public static List<XElement> ReadXElementsFromRegistry(string path, string valueName)
        {
            XElement root = ReadXElementFromRegistry(path, valueName);

            if (root != null)
            {
                return root.Elements().ToList();
            }
            else
            {
                return new List<XElement>();
            }
        }*/
        public static List<XElement> ReadAllSubXElementsFromRegistry(string path)
        {
            var list = new List<XElement>();
            using (var key = getRegistryKey(path))
            {
                list.AddRange(key.GetValueNames().Select(subkey_name => XDocument.Parse(key.GetValue(subkey_name).ToString()).Root));
            }
            return list;
        }
        public static XElement ReadXElementFromRegistry(string path, string valueName)
        {
            string s = ReadStringFromRegistry(path, valueName);
            if (s != null)
            {
                return XDocument.Parse(s).Root;
            }
            else
            {
                return null;
            }
        }
        public static string ReadStringFromRegistry(string path, string valueName)
        {
            object o = getRegistryKey(path).GetValue(valueName);
            if (o != null)
            {
                return o.ToString();
            }
            return null;
        }

        public static void WriteXElementToRegistry(string path, string valueName, XElement data)
        {
            string s = null;
            if (data != null)
            {
                s = data.ToString();
            }
            WriteStringToRegistry(path, valueName, s);
        }
        public static void WriteStringToRegistry(string path, string valueName, string data)
        {
            if (data != null)
            {
                getRegistryKey(path).SetValue(valueName, data);
            }
            else
            {
                RegistryKey rk = getRegistryKey(path);
                if (rk.GetValue(valueName) != null)
                {
                    rk.DeleteValue(valueName);
                }
            }
        }
        /*public static void ClearRegistry(string path = "")
        {
            if (!string.IsNullOrEmpty(path))
            {
                path += @"\";
            }
            string _reg_path = String.Format(@"{0}\sql.builder" + path, Environment.UserName);

            Registry.CurrentUser.DeleteSubKeyTree(_reg_path);
        }*/
        #endregion
        public static List<string> ExtractParamsFromString(string str)
        {
            if (str == null) {
                return null;
            }
            int pos_1 = str.IndexOf("[:");
            if (pos_1 < 0) {
                return null;
            }
            int len = str.Length;
            List<string> pars = new List<string>(4);
            do {
                int pos_2 = str.IndexOf(']', pos_1 + 2);
                if (pos_2 < 0) {
                    break;
                }
                Contract.Assert(str[pos_1] == '[');
                Contract.Assert(str[pos_1 + 1] == ':');
                Contract.Assert(str[pos_2] == ']');
                pos_1 = pos_1 + 2;
                int param_len = pos_2 - pos_1;
                if (len > 0) {
                    string param = string.Intern(str.Substring(pos_1, pos_2 - pos_1));
                    if (!pars.Contains(param)) {
                        pars.Add(param);
                    }
                }
                pos_1 = str.IndexOf("[:", pos_2 + 1);  // если (pos_2 + 1) == str.Length, функция не даёт ArgumentOutOfRangeException, а возвращает -1
            } while (pos_1 >= 0);
            return pars;
        }
        /*
        /// <summary>
        /// Интервал времени в полсекунды
        /// </summary>
        public static readonly TimeSpan HalfOfSecond = new TimeSpan(500L * TimeSpan.TicksPerMillisecond);
        */ 
        /// <summary>
        /// Записывает наиболее позднюю из двух дат в <paramref name="last_date"/>
        /// </summary>
        /// <param name="last_date"></param>
        /// <param name="date"></param>
        public static void GetLastDate(ref DateTime last_date, DateTime date)
        {
            if (date > last_date) {
                last_date = date;
            }
        }
        /*
        public static DateTime GetDirectoryLastChange(string directoryPath)
        {
            var d = DateTime.MinValue;
            var di = new DirectoryInfo(directoryPath);

            foreach (FileInfo fi in di.GetFiles())
            {
                if (fi.LastWriteTime > d)
                {
                    d = fi.LastWriteTime;
                }
            }

            foreach (DirectoryInfo di1 in di.EnumerateDirectories())
            {
                var d1 = GetDirectoryLastChange(di1.FullName);
                if (d1 > d)
                {
                    d = d1;
                }
            }
            return d;
        }
        */
        public static string GetCashDirectoryName()
        {
            string name = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), XmlReports.NativeProductName);
            if (!Directory.Exists(name))
            {
                Directory.CreateDirectory(name);
            }
            return name;
        }


        /*public static XElement GetXmlFromZip(ZipArchive zip, string file_name)
        {
            if (zip[file_name] == null) return null;

            var stream = zip[file_name].Open(useGlobalSettings: true);
            using (var reader = new StreamReader(stream))
            {
                return XDocument.Parse(reader.ReadToEnd()).Root;
            }
        }
        public static void PutXmlToZip(ZipArchive zip, XElement xml, string file_name)
        {
            // если файл уже есть, просто обновляем текст в нем
            if (zip[file_name] != null)
            {
                zip.UpdateText(file_name, xml.ToString());
            }
            // иначе добавляем новый файл с текстом в архив
            else
            {
                zip.AddText(file_name, xml.ToString());
            }
        }
        public static void CopyZipFile(ZipArchive dest, ZipArchive src, string file_name, string file_name_new = null)
        {
            file_name_new = file_name_new ?? file_name;
            dest.AddStream(file_name_new, src[file_name].Open());
        }*/

        /*private static void UpdateVForm(XElement xroot, List<VSXElement> applying_parts)
        {
            foreach (var xitem in xroot.Elements())
            {
                var vitem = applying_parts.FirstOrDefault(item => item.BaseElementOrSelf().GetUniqueKey().ToString() == xitem.Attribute("id").Value);
                if (vitem == null) continue;

                // если аттрибута в xitem нет - то он должен удаляться и из src_xitem
                foreach (var layout_option in TextConst.ANameArray.CustomLayoutOptions)
                {
                    if (xitem.AttrOrDef(layout_option, null) != null)
                    {

                    }
                    vitem.SetAttributeValue(layout_option, xitem.AttrOrDef(layout_option, null));
                }

                // колонки грида не обрабатываем, тк. к ним запарно генерировать id
                if (xitem.Name.LocalName == TextConst.EName.Grid || xitem.Name.LocalName == TextConst.EName.Field) continue;

                UpdateVForm(xitem, applying_parts);
            }
        }*/
        public static void SyncWithVForm(XElement xroot, List<VSXElement> applying_parts)
        {
            foreach (XElement xitem in xroot.Elements()) {
                VSXElement vitem = applying_parts.FirstOrDefault(item => item.BaseElementOrSelf().GetUniqueKey().ToString() == xitem.Attribute(AName.id).Value);
                if (vitem == null) {
                    continue;
                }
                bool custom_layout = (vitem.Attribute(AName.size) != null);
                if (custom_layout) {
                    if (xitem.Attribute(AName.size) == null) {
                        xitem.Add(new XAttribute(AName.size, vitem.Attribute(AName.size).Value));
                    }
                } else {
                    vitem.Attributes().Where(APredicate.IsCustomLayoutOptions).Remove();
                    xitem.Attributes().Where(APredicate.IsCustomLayoutOptions).Remove();
                }
                xitem.SetAttributeValue(AName.width_perc, vitem.AttrOrDefault(AName.width_perc, null));
                xitem.SetAttributeValue(AName.width_fixed, vitem.AttrOrDefault(AName.width_fixed, null));
                xitem.SetAttributeValue(AName.text_visible, vitem.AttrOrDefault(AName.text_visible, null));
                xitem.SetAttributeValue(AName.text_location, vitem.AttrOrDefault(AName.text_location, null));
                //xitem.SetAttributeValue(TextConst.AName.LayoutMode, vitem.AttrOrDef(TextConst.AName.LayoutMode, null));
                // колонки грида не обрабатываем, тк. к ним запарно генерировать id
                if (xitem.Name == EName.grid || xitem.Name == EName.field) {
                    continue;
                }
                SyncWithVForm(xitem, applying_parts);
            }
        }
        public static bool OSWin7AndNewer()
        {
            return ((Environment.OSVersion.Version.Major >= 6 && Environment.OSVersion.Version.Minor >= 1) ||
                    Environment.OSVersion.Version.Major >= 10);
        }
        public static string GetAvgReportFormingTime(string repname)
        {
            #if DEBUG
            DateTime d1 = DateTime.Now;
            #endif
            TimeSpan? value = db.AverageReportFormingTime(repname);
            #if DEBUG
            DateTime d2 = DateTime.Now;
            #endif
            if (value.HasValue) {
                #if DEBUG
                if (XmlReports.IsDeveloperMode() && !WCFHelper.IsClient && !WCFHelper.IsClient && (d2 - d1).TotalMilliseconds > 1000) {
                    //ShowMessage.ShowExclamation("Ахтунг! Расчет среднего времени формирования занял " + (d2 - d1).TotalMilliseconds.ToString(CultureInfo.InvariantCulture) + " мс !");
                }
                #endif
                return value.GetValueOrDefault().ToString(@"hh\:mm\:ss\.fff");
            } else {
                return null;
            }
        }
  
        public static Color MixColors(this Color source, Color target, float percent)
        {
            float amountSource = 1.0f - percent;

            return Color.FromArgb(
                (int)(source.A * amountSource + target.A * percent),
                (int)(source.R * amountSource + target.R * percent),
                (int)(source.G * amountSource + target.G * percent),
                (int)(source.B * amountSource + target.B * percent));

        }
        private static IVBarButton CreateBarButtonControl(XElement xcmd, VVariableDepandantceController vdc,ValueChangeEventHandler  handler)
        {
            throw new NotImplementedException();
        }
        // Cross-platform: Image is Windows-only, replaced with object
        public static object GetIcon(XElement xcmd)
        {
            XAttribute xicon = xcmd.Attribute(AName.icon);
            if (xicon == null) {
                return null;
            } else {
                return GetIcon(xicon.Value);
            }
        }
        private static object GetIcon(string name)
        {
            return null;
            //PropertyInfo pi = typeof(infoenergo.ui.resources.Properties.Resources).GetProperty(name, BindingFlags.Public | BindingFlags.Static | BindingFlags.GetProperty);
            //if (pi != null) {
            //    return pi.GetValue(null, null) as object;
            ////} else if (name == "CommitAndClose_24") {
            ////    return ImageCommitAndClose24;
            //} else {
            //    return null;
            //}
        }
        public static VSXElement GetActionInfo(XElement xcmd)
        {
            return VSXElement.Get(new XElement(xcmd));
        }

        /*public static IVPopupMenu CreatePopupMenu(XElement xmenu, BarManager bm, UIFormC form, ValueChangeEventHandler handler,
            EventHandler ctrlEditValueChangedHandler, EventHandler repEditValueChangedHandler, VVariableDepandantceController vdc)
        {
            //Бельченко 27.03.16 сделал метод и все, что он вызывает статическим, чтобы испоьзовать для грида в отчетах - получилось криво - переделать при случае.

            var menu = UIStatic.GetControlsfactory().CreatePopupMenu();
            if (!UIStatic.IsWeb())
            {
                (menu as sql.builder.UI.WinForms.VPopupMenu).SetBarManager(bm);
            }
          

            var items = Cmn.CreateBarItems(xmenu, bm, handler, ctrlEditValueChangedHandler, repEditValueChangedHandler, form, vdc);
            foreach (var item in items)
            {
                menu.AddButton(item,false);
            }
            //link.UserPaintStyle = BarItemPaintStyle.Caption;

            return menu;
        }*/
        /*public static SimpleButton CreateButtonControl(XElement xcmd, EventHandler Button_Click)
        {
            var btn = new SimpleButton();// BarButtonItem(null, xcmd.Attribute(TextConst.AName.Title).Value);

            btn.Name = "a" + btn.GetHashCode().ToString();// xcmd.Attribute(TextConst.AName.Name).Value;
            btn.Image = Cmn.GetIcon(xcmd);
            if (btn.Image != null)
            {
                btn.ToolTip = xcmd.Attribute(TextConst.AName.Title).Value;

                //btn.BorderStyle = BorderStyles.UltraFlat;
            }
            else
            {
                btn.Text = xcmd.Attribute(TextConst.AName.Title).Value;
            }
            //btn.AutoWidthInLayoutControl = false;
            btn.AutoSize = true;
            btn.MaximumSize = btn.CalcBestSize();
            var action = GetActionInfo(xcmd);
            btn.Tag = action;
            btn.Click += Button_Click;

            return btn;
        }*/
        /*public static EditorButton CreateEditorButton(XElement xcmd, string fieldName, string default_side = TextConst.AVSides.Left)
        {
            var action = GetActionInfo(xcmd);
            action.SetAttributeValue(TextConst.AName.Field, fieldName);
            var btn = new EditorButton();
            btn.ToolTip = btn.Caption = action.Attribute(TextConst.AName.Title).Value;
            var side = action.AttrOrDef(TextConst.AName.Side, default_side);
            btn.IsLeft = (side == TextConst.AVSides.Left);
            var xkind = action.Attribute(TextConst.AName.Type);
            if (xkind != null)
            {
                btn.Kind = (ButtonPredefines)Enum.Parse(typeof(ButtonPredefines), xkind.Value, true);
                if (btn.Kind == ButtonPredefines.Glyph)
                {
                    btn.Image = Cmn.GetIcon(action);
                }
            }

            btn.Tag = action;

            return btn;
        }*/

        public static DataSet ToDataSet(VDataSet vds)
        {
            var ds = new DataSet(vds.DataSetName);

            foreach (VDataTable vtable in vds.Tables)
            {
                ds.Tables.Add(ToDataTable(vtable));
            }

            return ds;
        }
        public static VDataSet ToVDataSet(DataSet ds)
        {
            var vds = new VDataSet();
            vds.DataSetName = ds.DataSetName;

            foreach (DataTable table in ds.Tables)
            {
                vds.Tables.Add(ToVDataTable(table));
            }

            return vds;
        }
        private static DataTable ToDataTable(VDataTable vdt)
        {
            DataTable dt = new DataTable(vdt.TableName);
            int index;
            for (index = 0; index < vdt.Columns.Count; index++) {
                DataColumn column = vdt.Columns[index];
                dt.Columns.Add(new DataColumn(column.ColumnName, column.DataType));
            }
            for (index = 0; index < vdt.Rows.Count; index++) {
                dt.ImportRow(vdt.Rows[index]);
            }
            CopyPrimaryKey(vdt, dt);
            /*DataColumn[] vdt_pk = vdt.PrimaryKey;
            int count = vdt_pk.Length;
            if (count > 0) {
                DataColumn[] pk = new DataColumn[count];
                for (index = 0; index < count; index++) {
                    pk[index] = dt.Columns[vdt_pk[index].ColumnName];
                }
                vdt.PrimaryKey = pk;
            }*/
            return dt;
        }
        public static VDataTable ToVDataTable(DataTable dt)
        {
            VDataTable vdt = new VDataTable();
            vdt.TableName = dt.TableName;
            int index;
            for (index = 0; index < dt.Columns.Count; index++) {
                DataColumn column = dt.Columns[index];
                vdt.AddColumn(column.ColumnName, column.DataType);
            }
            for (index = 0; index < dt.Rows.Count; index++) {
                vdt.ImportRow(dt.Rows[index]);
            }
            CopyPrimaryKey(dt, vdt);
            /*DataColumn[] dt_pk = dt.PrimaryKey;
            int count = dt_pk.Length;
            if (count > 0) {
                DataColumn[] pk = new DataColumn[count];
                for (index = 0; index < count; index++) {
                    pk[index] = vdt.Columns[dt_pk[index].ColumnName];
                }
                vdt.PrimaryKey = pk;
            }*/
            return vdt;
        }
        /// <summary>
        /// Копирует первичный ключ из <paramref name="src"/> в <paramref name="dest"/>
        /// </summary>
        /// <param name="src">Исходный DataTable</param>
        /// <param name="dest">DataTable, у которого надо установить первичный ключ</param>
        public static void CopyPrimaryKey(DataTable src, DataTable dest)
        {
            Contract.Assert(src != null);
            Contract.Assert(dest != null);
            DataColumn[] src_pk = src.PrimaryKey;
            DataColumn[] pk;
            int count = src_pk.Length;
            if (count == 0) {
                pk = Array.Empty<DataColumn>();
            } else {
                pk = new DataColumn[count];
                for (int index = 0; index < count; index++) {
                    DataColumn col = dest.Columns[src_pk[index].ColumnName];
                    Contract.Assume(col != null);
                    pk[index] = col;
                }
            }
            dest.PrimaryKey = pk;
        }
        /// <summary>
        /// Интернирует значения в строковой колонки <paramref name="column_name"/> в таблице <paramref name="table"/>
        /// </summary>
        /// <param name="table">Таблица с данными</param>
        /// <param name="column_name">Наименование интернируемой колонки</param>
        /// <returns>Количество уникальных значений в колонке</returns>
        public static int InternStringColumn(DataTable table, string column_name)
        { 
            Contract.Assert(table != null);
            Contract.Assert(!string.IsNullOrEmpty(column_name));
            DataColumn column = table.Columns[column_name];
            Contract.Assume(column != null);
            Contract.Assume(column.DataType == typeof(string));
            IDictionary<string, string> set = new Dictionary<string, string>(StringComparer.InvariantCulture); // ReferenceEqualityComparer.Instance
            //HashSet<string> set = new HashSet<string>(StringComparer.InvariantCulture); // ReferenceEqualityComparer.Instance
            DataRowCollection rows = table.Rows;
            for (int index = 0; index < rows.Count; index++) {
                DataRow row = rows[index];
                if (!row.IsNull(column)) {
                    string old_value = (string)row[column];
                    string new_value;
                    if (!set.TryGetValue(old_value, out new_value)) {
                        // Если строка интернирована, используем интернированное значение
                        new_value = string.IsInterned(old_value);
                        if (new_value == null) {
                            // А если строка не интернирована, используем значение из DataTable,
                            // чтобы не забивать мусором таблицу интернированых строк
                            // и чтобы эти строки могли быть удалены сборщиком мусора.
                            new_value = old_value;
                        }
                        set.Add(new_value, new_value);
                    }
                    if (!object.ReferenceEquals(old_value, new_value)) {
                        row.BeginEdit();
                        row[column] = new_value;
                        row.EndEdit();
                        row.AcceptChanges();
                    }
                }
            }
            return set.Count;
        }
        public static XElement LoadDefaultReportParams(string repname)
        {
            string data = db.SelectDefaultSettingData(repname);
            if (data == null) {
                return null;
            } else {
                return XElement.Parse(data).Element(EName.@params);
            }
        }
        //public static Color GetHighlightColor()
        //{

        //    return CommonSkins.GetSkin(UserLookAndFeel.Default).Colors[CommonColors.Highlight];
        //}

        public static string ExtractProjectName(string path)
        {
            string[] parts = path.Split('\\');
            // первая папка после папки source
            var proj = parts.SkipWhile(p => p != XmlReports.SourceFolderName).Skip(1).FirstOrDefault();
            // если в пути нет папки source - первая папка
            if (proj == null) {
                proj = parts.First(p => p != "");
            }
            return proj;
        }
        public static string GetProjectPath(XElement xproject)
        {
            string name = xproject.Attribute(AName.name).Value;
            XAttribute attr = xproject.Attribute(AName.directory);
            if (attr == null) {
                return Path.Combine(XmlReports.GetDefaultSourceFolder(), name);
            } else {
                return Path.Combine(XmlReports.GetRootPath(), attr.Value, name);
            }
        }
        //public static Color GetFocusedBackColor()
        //{
        //    return CommonSkins.GetSkin(UserLookAndFeel.Default)[CommonSkins.SkinSelection].Color.BackColor;
        //}

        public static bool IsAdministrator()
        {
            var identity = WindowsIdentity.GetCurrent();
            var principal = new WindowsPrincipal(identity);
            return principal.IsInRole(WindowsBuiltInRole.Administrator);
        }

        /*public static string[] GetTableColumnsArray(string table_name)
        {
            var owner = (string)db.GetTablePublicSynonyms(table_name).Rows[0]["TABLE_OWNER"];
            return db.GetTableColumns(table_name, owner).AsEnumerable().Select(r => ((string)r["COLUMN_NAME"]).ToLower()).ToArray();
        }*/

        public static Type GetTypeFromStringType(string type, Type def)
        {
            if (string.IsNullOrEmpty(type)) {
                return def;
            }
            switch (type) {
                case TextConst.AVDataType.Number:
                    return XmlReports.numberType;
                case TextConst.AVDataType.Bool:
                    return XmlReports.numberType;
                case TextConst.AVDataType.Date:
                    return typeof(DateTime);
                case TextConst.AVDataType.String:
                    return typeof(string);
                case TextConst.AVDataType.Clob:
                    return typeof(string);
				case TextConst.AVDataType.Blob:
					return typeof(byte[]);
                default:
                    return def;
            }
        }
        public static string OracleTypeDefinitionFromType(Type type, int length = 0)
        {

            if (type == XmlReports.numberType)
            {
                return "NUMBER";
            }

            if (type == typeof(DateTime))
            {
                return "DATE";
            }

            if (type == typeof(string))
            {
                if (length == -1)
                {
                    return "VARCHAR2";
                }
                if (length == 0)
                {
                    length = 300;
                }
                return "VARCHAR2(" + length.ToString() + ")";
            }

			if (type == typeof(byte[]))
			{
				return "BLOB";
			}

            return "";

        }

        private static string _exe_info;
        public static string GetExeInfo()
        {
            try
            {
                if (_exe_info == null)
                {
                    var assembly = Assembly.GetExecutingAssembly();
                    var fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

                    _exe_info = string.Format("{0} {1}", fvi.OriginalFilename, fvi.FileVersion);
                }

                return _exe_info;

            }
            catch
            {
                return "";
            }
        }

        private static string _db_info;
        public static string GetDBInfo()
        {
            try
            {
                if (_db_info == null)
                {
                    _db_info = DataHelper.SqlGetString(@"select UPPER(sys_context('userenv','instance_name'))||' '||version from rs_esys", db.Connection, false);
                }

                return _db_info;
            }
            catch
            {
                return "";
            }
        }

        /*public static IEnumerable<IEnumerable<T>> Batch<T>(this IEnumerable<T> items, int maxItems)
        {
            return items.Select((item, inx) => new { item, inx })
                .GroupBy(x => x.inx / maxItems)
                .Select(g => g.Select(x => x.item));
        }*/
        public static Devart.Data.Oracle.OracleDbType GetDBType(string type)
        {
            switch (type) {
                case "number":
                    return Devart.Data.Oracle.OracleDbType.Number;
                case "bool":
                    return Devart.Data.Oracle.OracleDbType.Number;
                case "date":
                    return Devart.Data.Oracle.OracleDbType.Date;
                case "array":
                    return Devart.Data.Oracle.OracleDbType.Array;
                case "clob":
                    return Devart.Data.Oracle.OracleDbType.Clob;
				case "blob":
					return Devart.Data.Oracle.OracleDbType.Blob;
				default:
                    return Devart.Data.Oracle.OracleDbType.VarChar;
            }
        }
        public static Devart.Data.Oracle.OracleDbType GetDBType(Type type)
        {
            if (type == typeof(Decimal)) {
                return Devart.Data.Oracle.OracleDbType.Number;
            } else if (type == typeof(DateTime)) {
                return Devart.Data.Oracle.OracleDbType.Date;
            } else {
                return Devart.Data.Oracle.OracleDbType.VarChar;
            }
        }
        public static string writeScriptFile(string name, string data)
        {
            var namefile = string.Format("{0}\\{1}_{2}_{3}_ddl.sql", Path.GetDirectoryName(Path.GetTempPath()), DateTime.Now.ToString("yyMMdd"), Environment.MachineName, name);
            using (var sw = new StreamWriter(new FileStream(namefile, FileMode.Create), Encoding.GetEncoding(1251)))
            {
                sw.Write(data);
            }
            return namefile;
        }


        public static string WriteFileToTemp(string name, string data)
        {
            var namefile = string.Format("{0}\\{1}", Path.GetDirectoryName(Path.GetTempPath()), name);
            using (var sw = new StreamWriter(new FileStream(namefile, FileMode.Create), Encoding.GetEncoding(1251)))
            {
                sw.Write(data);
            }
            return namefile;
        }

        public static StringBuilder BuildCodeOfXmlString(string sxml)
        {
            var el = XElement.Parse(sxml);
            var sb = new StringBuilder();
            sb.Append(BuildCodeOfXmlStringLevel(el));
            return sb;
        }

        private static StringBuilder BuildCodeOfXmlStringLevel(XElement element)
        {


            var sb = new StringBuilder();
            sb.AppendLine(string.Format("new XElement({0}", BuildCodeOfXmlString_GetElementName(element)));
 
            foreach (var  attr in element.Attributes())
            {
                sb.Append(",");
                sb.AppendLine(string.Format("new XAttribute({0},{1})", BuildCodeOfXmlString_GetAttrName(attr),
                    BuildCodeOfXmlString_GetAttrVal(attr)));
            
            }

      
            foreach (var el in element.Elements())
            {
                sb.Append(",");
                sb.Append(BuildCodeOfXmlStringLevel(el));
          
            }
            sb.AppendLine(string.Format(")"));
            return sb;
        }


        private static string BuildCodeOfXmlString_FindConst(Type cls,string value)
        {
            if (cls != null)
            {
                foreach (var p in cls.GetFields())
                {
                    var val = p.GetValue(null);
                    if (val.ToString() == value)
                    {
                        return typeof (TextConst).Name + "." + cls.Name + "." + p.Name;
                    }
                }
            }
            
            return string.Format("\"{0}\"", value);

        }

        private static string BuildCodeOfXmlString_GetElementName(XElement element)
        {
            
            var name = BuildCodeOfXmlString_FindConst(typeof (TextConst.EName),
                element.Name.LocalName);

            return name;
           
        }
        private static string BuildCodeOfXmlString_GetAttrName(XAttribute attr)
        {
            var name = BuildCodeOfXmlString_FindConst(typeof(TextConst.AName), 
               attr.Name.LocalName);

            return name;

        }

        private static string BuildCodeOfXmlString_GetAttrVal(XAttribute attr)
        {
#if DEBUG
            var attrName = BuildCodeOfXmlString_GetAttrName(attr).SubstringAfter('.');
            //Type cls = typeof(TextConst).GetTypeInfo().DeclaredNestedTypes()..GetNestedType("AV" + attrName);
            Type cls =
                typeof(TextConst).GetTypeInfo()
                    .DeclaredNestedTypes.Where(ti => ti.Name == "AV" + attrName).Select(ti1 => ti1.AsType()).FirstOrDefault();

            return BuildCodeOfXmlString_FindConst(cls, attr.Value);
#else

            return null;
#endif

        }
        /// <summary>
        /// Удаляет из <paramref name="sb"/> недопустимые символы XML (см. https://www.w3.org/TR/2006/REC-xml-20060816/#charsets) и
        /// заменяет все переводы строки ("\n\r", "\r\n" и "\n") на "\r".
        /// </summary>
        /// <param name="sb"></param>
        public static void RefineExcelText(StringBuilder sb)
        {
            Contract.Assert(sb != null);
            sb.Replace("\n\r", "\r");
            sb.Replace("\r\n", "\r");
            int index = 0;
            while (index < sb.Length) {
                char ch = sb[index];
                if (XmlConvert.IsXmlChar(ch)) {
                    if (ch == '\n') {
                        sb[index] = '\r';
                    }
                    index++;
                } else {
                    sb.Remove(index, 1);
                }
            }
        }
        public static bool TryGetParameter(this Devart.Data.Oracle.OracleParameterCollection parameters, string parameter_name, out Devart.Data.Oracle.OracleParameter parameter)
        {
            Contract.Assert(parameters != null);
            int index = parameters.IndexOf(parameter_name);
            if (index >= 0) {
                parameter = parameters[index];
                return true;
            } else {
                parameter = null;
                return false;
            }
        }
        public static string[] GetParameterNames(DbParameterCollection parameters)
        {
            Contract.Assume(parameters != null);
            string[] param_names;
            int pаram_count = parameters.Count;
            if (pаram_count == 0) {
                param_names = Array.Empty<string>();
            } else {
                param_names = new string[pаram_count];
                for (int index = 0; index < pаram_count; index++) {
                    param_names[index] = parameters[index].ParameterName;
                }
            }
            return param_names;
        }
        public static string[] GetParameterNames<T>(IList<T> parameters)
            where T : DbParameter
        {
            Contract.Assume(parameters != null);
            string[] param_names;
            int pаram_count = parameters.Count;
            if (pаram_count == 0) {
                param_names = Array.Empty<string>();
            } else {
                param_names = new string[pаram_count];
                for (int index = 0; index < pаram_count; index++) {
                    param_names[index] = parameters[index].ParameterName;
                }
            }
            return param_names;
        }
        /// <summary>
        /// Парсит текст запроса <paramref name="sql"/> и возвращает имена использованных в нём bind-переменных
        /// </summary>
        /// <param name="sql">текст запроса</param>
        /// <returns>массив bind-переменных в запросе <paramref name="sql"/></returns>
        public static string[] ExtractParameterNamesFromSQL(string sql)
        {
            string[] param_names;
            using (Devart.Data.Oracle.OracleCommand cmd = new VOracleCommand()) {
                cmd.ParameterCheck = true; // чтобы коллекция Parameters заполнилась при установке CommandText
                cmd.CommandText = sql;
                param_names = Cmn.GetParameterNames(cmd.Parameters);
            }
            return param_names;
        }
        #region Для использования в качестве аргумента Select() и SelectAsArray()
        public static string GetDataColumnName(DataColumn col)
        {
            return col.ColumnName;
        }
        public static string GetParameterName(DbParameter col)
        {
            return col.ParameterName;
        }
        #endregion
        public static bool HasPrimaryKey(this DataTable dt)
        {
            Contract.Assert(dt != null);
            return !Array.IsNullOrEmpty(dt.PrimaryKey);
        }
        public static DataRow AddRow(this DataTable dt, object value)
        {
            Contract.Assert(dt != null);
            Contract.Assert(dt.Columns.Count >= 1);
            DataRow row = dt.NewRow();
            row[0] = value;
            dt.Rows.Add(row);
            return row;
        }
        public static DataRow AddRow(this DataTable dt, object value_1, object value_2)
        {
            Contract.Assert(dt != null);
            Contract.Assert(dt.Columns.Count >= 2);
            DataRow row = dt.NewRow();
            row[0] = value_1;
            row[1] = value_2;
            dt.Rows.Add(row);
            return row;
        }
        public static DataRow AddRow(this DataTable dt, object value_1, object value_2, object value_3)
        {
            Contract.Assert(dt != null);
            Contract.Assert(dt.Columns.Count >= 3);
            DataRow row = dt.NewRow();
            row[0] = value_1;
            row[1] = value_2;
            row[2] = value_3;
            dt.Rows.Add(row);
            return row;
        }
        public static DataRow AddRow(this DataTable dt, object value_1, object value_2, object value_3, object value_4)
        {
            Contract.Assert(dt != null);
            Contract.Assert(dt.Columns.Count >= 4);
            DataRow row = dt.NewRow();
            row[0] = value_1;
            row[1] = value_2;
            row[2] = value_3;
            row[3] = value_4;
            dt.Rows.Add(row);
            return row;
        }
        public static DataRow AddRow(this DataTable dt, object value_1, object value_2, object value_3, object value_4, object value_5)
        {
            Contract.Assert(dt != null);
            Contract.Assert(dt.Columns.Count >= 5);
            DataRow row = dt.NewRow();
            row[0] = value_1;
            row[1] = value_2;
            row[2] = value_3;
            row[3] = value_4;
            row[4] = value_5;
            dt.Rows.Add(row);
            return row;
        }
        public static DataRow[] ToArray(this DataRowCollection rows)
        {
            int count = rows.Count;
            if (count == 0) {
                return Array.Empty<DataRow>(); // Используем единственный экземпляр пустого массива, чтобы не захламлять память
            } else {
                DataRow[] arr = new DataRow[count];
                rows.CopyTo(arr, 0);
                return arr;
            }
        }
        public static void DisposeAndSetNull<T>(ref T disposable)
            where T : class, IDisposable
        {
            if (disposable!=null)
            {
                disposable.Dispose();
            }
           
            disposable = null;
        }
        public static void RaiseEvent<TEventArgs>(ref EventHandler<TEventArgs> event_delegate, object sender, TEventArgs args)
        {
            EventHandler<TEventArgs> handler = System.Threading.Volatile.Read(ref event_delegate);
            if (handler != null) {
                handler.Invoke(sender, args);
            }
        }
        // Предикат для сортировки по длине строки
        public static int LengthOfString(string s)
        {
            Contract.Assume(s != null);
            return s.Length;
        }
        /// <summary>
        /// Предикат Comparison&lt;string&gt; для обратной сортировки по длине строки
        /// </summary>
        /// <param name="x">первая строка</param>
        /// <param name="y">вторая строка</param>
        /// <returns>положительное число, если <paramref name="y" /> длинее <paramref name="x" />, ноль, если <paramref name="y" /> и <paramref name="x" /> равной длины и отрицательное число, если <paramref name="y" /> короче <paramref name="x" /></returns>
        public static int DescComparsionByLength(string x, string y)
        {
            Contract.Assume(x != null);
            Contract.Assume(y != null);
            return y.Length - x.Length;
        }
        // Предикат для поиска
        public static bool IsNotNull(object obj)
        {
            return obj != null;
        }
    }

    public class XElementEventArgs : EventArgs
    {
        public List<XElement> Elements;
        public XElementEventArgs(List<XElement> elements)
            : base()
        {
            Elements = elements;
        }

    }

    public delegate void XElementEventHandler(Object sender, XElementEventArgs e);
}
