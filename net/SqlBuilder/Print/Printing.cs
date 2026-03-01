using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Linq;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Views.Grid;
//using infoenergo.ui.win.Grid;
//using Microsoft.Office.Interop.Excel;
//using Microsoft.Vbe.Interop;
using sql.builder.DataApi;
using sql.builder.ExcelApi;
using Contract = System.Diagnostics.Contracts.Contract;
//using DevExpress.Spreadsheet;
//using FlexCel.Core;
//using FlexCel.XlsAdapter;
//
//using reports.word.XmlPrint;
//using sql.builder.Test;
using DataTable = System.Data.DataTable;
//using Application = Microsoft.Office.Interop.Excel.Application;
//using Workbook = Microsoft.Office.Interop.Excel.Workbook;
//using Worksheet = Microsoft.Office.Interop.Excel.Worksheet;
//using Range = Microsoft.Office.Interop.Excel.Range;
//using DWorkbook = DevExpress.Spreadsheet.Workbook;
//using DWorksheet = DevExpress.Spreadsheet.Worksheet;
//using DRange = DevExpress.Spreadsheet.Range;
//using FormatCondition = Microsoft.Office.Interop.Excel.FormatCondition;

namespace sql.builder
{
    public static partial class Printing
    {
        public static string templatesFolder;
        private static string _outputFolder;

        public static string outputFolder
        {
            get
            {
                if (string.IsNullOrEmpty(_outputFolder))
                {
                    return sql.builder.Clean.Settings.GetInstance().TempPath;
                }
                else
                {
                    return _outputFolder;
                }
            }
            set
            {
                _outputFolder = value;
            }
        }
        public static bool CreateRefs = false;
        public static string Print(DataSet dataSet, string temlplateName, string outputName)
        {
            XmlDocument xmldoc = new XmlDocument();
            xmldoc.LoadXml("<template name=\"\" title=\"\"  print-proc=\"2\" />");
            xmldoc.FirstChild.Attributes["name"].Value = temlplateName;
            xmldoc.FirstChild.Attributes["title"].Value = outputName;
            return Print(null, dataSet, xmldoc.FirstChild);
        }
        public static string Print(XmlNode data, DataSet dataSet, XmlNode temlplateInfo, XDocument xTemplate = null, bool show_messages = true)
        {
            WaitUIHelper.LastUsedUIHelper.Show("Формирование файла", WaitUIMode.WaitPanel, true);
            //Wait.Show("Формирование файла", over: true);
            string templateType = temlplateInfo.ParentNode.Name;
            string templatePath = Path.Combine(templatesFolder, templateType, temlplateInfo.Attributes[TextConst.AName.Name].Value);
#if DEBUG
            // в режиме отладки добавляем шаблон в проект sql.builder.templates и ТФС
            //if (XmlReports.IsDeveloperMode()) {
            //    ExcelPrintDocument.ReloadExcelTemplate(templatePath, templateType);
            //}
#endif
            string title = temlplateInfo.Attributes[TextConst.AName.Title].Value;
            string fileName;
            if (templateType == "word")
            {
                throw new NotImplementedException();
                //fileName = PrintWord(dataSet, templatePath, title, temlplateInfo);
            }
            else
            {
                fileName = printExcel(data, dataSet, templatePath, title, temlplateInfo, xTemplate, show_messages);
            }
            WaitUIHelper.LastUsedUIHelper.Hide();
            //Wait.Hide();
            return fileName;
        }

        private static XmlNamespaceManager excelNamespaseManager;
        public static ExcelPrintDocument.ExcelPrintErrors printExcel(XDocument template, DataSet dataSet, string filename)
        {
            Contract.Assert(!string.IsNullOrEmpty(filename));
            var doc = new sql.builder.Print.XML.ExcelPrintDocument(template);
            return doc.Print(filename, dataSet, false, false);
        }
        public static string printExcel(XmlNode data, DataSet dataSet, string templatePath, string title, XmlNode templateInfo, XDocument Template = null, bool show_empty_message = true)
        {
            bool print_big_data = ((dataSet.Tables[0] is VDataTable) && (dataSet.Tables[0] as VDataTable).Reader != null);
            bool convertToOpenXml;
            XmlAttribute attr = templateInfo.Attributes[TextConst.AName.ConvertToOpenXml];
            if (print_big_data)
            {
                if (attr != null && attr.Value == TextConst.AVBool.False)
                {
                    convertToOpenXml = false;
                }
                else
                {
                    convertToOpenXml = true;
                }
            }
            else
            {
                if (attr != null && attr.Value == TextConst.AVBool.True)
                {
                    convertToOpenXml = true;
                }
                else
                {
                    convertToOpenXml = false;
                }
            }
            //
            attr = templateInfo.Attributes["print-proc"];
            bool isNewProc = (attr != null && attr.Value == "2");
            //
            bool multipage = templateInfo.AttrOrDefault("multipage", false);
            //
            XDocument xTemplate;
            if (Template != null)
            {
                xTemplate = Template;
            }
            else
            {
                using (FileStream templateStream = File.Open(templatePath, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    xTemplate = XDocument.Load(templateStream);
                    templateStream.Close();
                }
            }
            xTemplate = colsProcessing(data, xTemplate);
            if (templateInfo.AttrOrDefault(TextConst.AName.DelCols, false))
            { // Пока удаление колонок только для нового варианта, а колонки по измерениям только для старого
                xTemplate = delUnused(dataSet, xTemplate);
            }
            //
            XmlAttribute output_format_attr = templateInfo.Attributes["output-format"];
            string output_format;
            if (output_format_attr == null)
            {
                output_format = "xlsx";
            }
            else
            {
                output_format = output_format_attr.Value;
                if (output_format != "pdf")
                {
                    output_format = "xlsx";
                }
            }
            string format;
            if (convertToOpenXml /*|| ExcelPrintDocument.autoConvert*/)
            {
                format = "xlsx";
            }
            else
            {
                format = "xml";
            }
            string fileName = GetFreeName(outputFolder, title, output_format);
            string fileNameX = GetFreeName(outputFolder, title, format);
            ExcelPrintDocument.ExcelPrintErrors err = ExcelPrintDocument.ExcelPrintErrors.None;
            sql.builder.Print.XML.ExcelPrintDocument doc = null;
            if (isNewProc)
            {
                doc = new sql.builder.Print.XML.ExcelPrintDocument(xTemplate);
                xTemplate = null;
                if (convertToOpenXml)
                {
                    ExcelEnvironment.BeginPrintBigData();
                    err = doc.Print(null, dataSet, print_big_data, true);
                    ExcelEnvironment.EndPrintBigData(fileName);
                }
                else
                {
                    err = doc.Print(fileNameX, dataSet, print_big_data, false);
                    doc = null;
                }
            }
            else
            {
                XmlDocument template = new XmlDocument();
                template.PreserveWhitespace = true;
                template.Load(templatePath);
                using (MemoryStream xmlStream = new MemoryStream())
                {
                    xTemplate.Save(xmlStream);
                    xmlStream.Flush();
                    xmlStream.Position = 0;
                    template.Load(xmlStream);
                }
                excelNamespaseManager = new XmlNamespaceManager(template.NameTable);
                excelNamespaseManager.AddNamespace("ss", "urn:schemas-microsoft-com:office:spreadsheet");
                XmlNode sheet = template.SelectSingleNode(".//*[name()='ss:Worksheet']");
                // sheet.SelectSingleNode(".//*[name()='Table']").Attributes["ss:ExpandedColumnCount"].Value = (getLastColumn(sheet)+1).ToString();
                applySource(sheet, 1, data.SelectSingleNode(".//data"), "", null, multipage, new SortedList<string, SortedList<string, Tuple<int, XmlNode>>>());
                XmlNodeList sheets = template.SelectNodes(".//*[name()='ss:Worksheet']");
                foreach (XmlNode sheet1 in sheets)
                {
                    XmlNodeList rows = sheet1.SelectSingleNode(".//*[name()='ss:Table']").SelectNodes(".//*[name()='ss:Row']");
                    foreach (XmlNode row in rows)
                    {
                        row.RemoveAttribute("ss:Index");
                    }
                    sheet1.SelectSingleNode(".//*[name()='ss:Table']").Attributes["ss:ExpandedRowCount"].Value = (rows.Count + 1).ToString();
                }
                if (sheets.Count < 1)
                {
                    return string.Empty;
                }
                /* XmlNodeList markers = template.SelectNodes("//*[name()='Data' and contains(.,'begin:')]");
                 foreach (XmlNode marker in markers) {
                     int i= getMarkerColumn( marker);
                 }
                 */
                template.Save(fileNameX);
            }
            if (err == ExcelPrintDocument.ExcelPrintErrors.NoData)
            {
                //if (show_empty_message) XtraMessageBox.Show("По заданным условиям нет данных для печати");
                // зачем этот файл сохраняется??
                if (File.Exists(fileNameX)) File.Delete(fileNameX);
                return string.Empty;
            }
            if (convertToOpenXml)
            {
                bool xlsb = (output_format_attr != null && output_format_attr.Value == "xlsb");
                fileName = ExcelPrintDocument.PostProcessBigData(fileNameX, templatePath, xlsb);
            }
            else
            {
                fileName = ExcelPrintDocument.PostProcess(fileNameX, output_format, fileName, null);
            }
            return fileName;
        }
        private static XElement ret;// чтобы не обявлять каждый раз при вызове .Cell



        public static XDocument delUnused(DataSet data, XDocument template)
        {
            //MemoryStream xmlStream = new MemoryStream();


            //template.Save(xmlStream);

            //xmlStream.Flush();
            //xmlStream.Position = 0;


            VExcelWorkbook wb = new VExcelWorkbook(template);
            for (int si = 0; si < wb.SheetsCount(); si++)
            {

                VExcelSheet sheet = wb.Sheet(si);

                int rowsCount = Convert.ToInt32(sheet.Element.Descendants().Attributes(VExcelNS.SpreadSheet.ExpandedRowCount).First().Value);

                List<int> indexesForDelete = new List<int>();

                foreach (DataTable table in data.Tables)
                {
                    string alias = table.TableName;
                    List<VExcelCell> cells = sheet.FindCells("[:" + alias + ".");
                    indexesForDelete.Clear();
                    foreach (VExcelCell cell in cells)
                    {
                        //if (cell.Value.ToString() == "[:a.i.pow_cnt_proch_wait]")
                        //{
                        //}
                        MatchCollection matches = Regex.Matches(cell.Value, @"\[:" + alias + @"\.[A-z0-9_\.]*\]");


                        foreach (Match m in matches)
                        {


                            string[] colNameA = m.Value.ToString().Replace("[:" + alias + ".", "").Replace("]", "").Split('.');

                            string colName = colNameA[colNameA.Length - 1];

                            if (!table.Columns.Contains(colName))
                            {
                                int i = cell.Index;
                                if (!indexesForDelete.Contains(i))
                                {
                                    indexesForDelete.Add(i);
                                }
                                break;
                            }
                        }




                    }


                    int i1 = 0;
                    foreach (int i in indexesForDelete.OrderByDescending(e => e))
                    {

                        VExcelCell c1 = sheet.Row(1).Cell(i);
                        VExcelRow r2 = sheet.Row(rowsCount);
                        VExcelCell c2 = r2.Cell(i);
                        VExcelRange range = new VExcelRange(c1, c2);

                        //  range.Remove();
                        if (i1 == 6)
                        {

                            range.Remove();
                            //  break;
                        }
                        else
                        {
                            range.Remove();
                        }
                        i1++;
                    }

                    //template
                    //template.Load(wb.SaveToStream());
                    //wb.Document.Save(@"C:\Users\abelchenko\Desktop\Новая папка\test.xml");




                }
            }


            return wb.Document;


        }



        /*public static XElement ExtractHeadInfo(string templateName)
        {
            string templatePath = Path.Combine(templatesFolder, "excel", templateName);
            XDocument doc = XDocument.Load(templatePath);
            return ExtractHeadInfo(doc);
        }*/

        /*public static XElement ExtractHeadInfo(XDocument template)
        {
            XElement xroot = new XElement(TextConst.EName.ExcelTemplate);
            VExcelWorkbook wb = new VExcelWorkbook(template);
            for (int si = 0; si < wb.SheetsCount(); si++) {
                VExcelSheet sheet = wb.Sheet(si);
                var xsheet = new XElement(TextConst.EName.ExcelSheet);
                xroot.Add(xsheet);
                xsheet.SetAttributeValue(TextConst.AName.Title, sheet.GetName());
                List<VExcelCell> cells = sheet.FindCells(TextConst.ExcelMarks.HeadMarker);
                List<int> headIndexes = new List<int>();
                foreach (VExcelCell cell in cells) {
                    headIndexes.Add(cell.RowIndex);
                }
                int lastHeadRowIndex = headIndexes.Max();
                SortedList<int, List<string>> colVarList = new SortedList<int, List<string>>();
                for (int i = 0; i < cells[0].Index; i++) {
                    colVarList.Add(i, new List<string>());
                }
                cells = sheet.FindCells("[:");
                foreach (VExcelCell cell in cells) {
                    if (cell.RowIndex > lastHeadRowIndex) {
                        MatchCollection matches = Regex.Matches(cell.Value, @"\[:[A-z0-9_\.]*\]");
                        int index = cell.Index;
                        if (!colVarList.ContainsKey(index)) {
                            colVarList.Add(index, new List<string>());
                        }
                        foreach (Match m in matches) {
                            string svar = m.Value.Replace("[:", "").Replace("]", "");
                            if (!colVarList[index].Contains(svar)) {
                                colVarList[index].Add(svar);
                            }
                        }
                    }
                }
                SortedList<int, string> prevTitles = new SortedList<int, string>();
                foreach (int i in colVarList.Keys) {
                    string stitle = string.Empty;
                    string q = string.Empty;
                    foreach (int j in headIndexes) {
                        VExcelCell cell = sheet.Row(j).Cell(i);
                        string stitle1;
                        if (cell != null) {
                            stitle1 = cell.Value ?? string.Empty;
                        } else if (prevTitles.ContainsKey(j)) {
                            stitle1 = prevTitles[j];
                        } else {
                            stitle1 = string.Empty;
                        }
                        if (!string.IsNullOrEmpty(stitle1)) {
                            if (!prevTitles.ContainsKey(j)) {
                                prevTitles.Add(j, string.Empty);
                            }
                            prevTitles[j] = stitle1;
                            stitle += q + stitle1;
                        }
                        q = " / ";
                    }
                    XElement colInfo = new XElement(TextConst.EName.ExcelColumn, new XAttribute(TextConst.AName.Title, stitle));
                    foreach (string svar in colVarList[i].Distinct()) {
                        colInfo.Add(new XElement(TextConst.EName.Column, new XText(svar)));
                    }
                    xsheet.Add(colInfo);
                }
            }
            return xroot;
        }*/
        public static XDocument colsProcessing(XmlNode data, XDocument template)
        {
            //MemoryStream xmlStream = new MemoryStream();
            // template.Save(xmlStream);
            // xmlStream.Flush();
            // xmlStream.Position = 0;
            VExcelWorkbook wb = new VExcelWorkbook(template);
            for (int sheetIndex = 0; sheetIndex < wb.SheetsCount(); sheetIndex++)
            {
                VExcelSheet sheet = wb.Sheet(sheetIndex);
                VExcelCell cell1 = sheet.FindCell("cbegin");
                while (cell1 != null)
                {
                    string tableName = cell1.Value.Split(' ')[0].Split(':')[1];
                    VExcelCell cell2 = sheet.FindCell("cend:" + tableName, cell1.Index);
                    if (cell2 == null)
                    {
                        // template.Load(wb.SaveToStream());
                        //template.Save(@"C:\tfs\all\sql.builder\sql.builder\printTemplate\excel\25499-test3.xml");
                        cell2 = sheet.FindCell("cend:" + tableName, cell1.Index);
                    }
                    cell1.SetValue(string.Empty);
                    cell2.SetValue(string.Empty);
                    int rangeWidth = cell2.Index - cell1.Index;
                    VExcelRange range = new VExcelRange(cell1, cell2);
                    VExcelRange newRange = range;
                    int i = 0;
                    XmlNode dimsNode = data.SelectSingleNode("root/scheme//table/dimension-values[@table='" + tableName + "']");
                    if (dimsNode != null)
                    { // null может быть при исключении колонок через colset
                        XmlNodeList dims = dimsNode.SelectNodes(".//val");
                        foreach (XmlNode dim in dims)
                        {
                            if (i < dims.Count - 1)
                            {
                                VExcelCell tagCell = newRange.FirstCell.Row.Cell(newRange.LastCell.Index + 1, ref ret);
                                newRange = tagCell.Insert(range);
                            }
                            i++;
                        }
                        i = 0;
                        newRange = range;
                        VExcelCell cell11 = cell1;
                        VExcelCell cell22 = cell2;
                        foreach (XmlNode dim in dims)
                        {
                            newRange.Replace("[" + tableName + ".title]", dim.Attributes["title"].Value);
                            if (dim.Attributes["columnpref"] != null)
                            {
                                newRange.Replace("[" + tableName + ".columnpref]", dim.Attributes["columnpref"].Value);
                            }
                            newRange.Replace("[" + tableName + ".pfx]", "_" + dim.Attributes["value"].Value.Replace("-", "_").Replace(".", "_").Replace(",", "_"));
                            newRange.Replace("[merge]", "");
                            cell11 = cell1.Row.Cell(cell11.Index + rangeWidth + 1, ref ret);
                            cell22 = cell22.Row.Cell(cell22.Index + rangeWidth + 1, ref ret);
                            newRange = new VExcelRange(cell11, cell22);
                            i++;
                        }
                    }
                    // range.Remove();
                    cell1 = sheet.FindCell("cbegin");
                }
            }
            //wb.Document.Save(@"C:\Users\abelchenko\Desktop\Новая папка\test.xml");
            return wb.Document;
        }
        //private static XmlNode rootData;
        //private static bool multipage = false;
        //private static SortedList<string, SortedList<string, int>> columnsIndexesList; // = new SortedList<string, SortedList<string, int>>();
        //private static SortedList<string, SortedList<string, XmlNode>> columnsInfoList; // = new SortedList<string, SortedList<string, XmlNode>>();
        public static bool applySource(XmlNode templatePart, int sourceIndex, XmlNode data, string parentName, XmlNode rootData, bool multipage, SortedList<string, SortedList<string, Tuple<int, XmlNode>>> columnsInfo)
        {
            string tableName = null;
            if (rootData == null)
            {
                rootData = data.SelectSingleNode("//root/data");
            }
            XmlNode beginMarker = null;
            if (sourceIndex == 1)
            {
                beginMarker = templatePart.SelectSingleNode(".//*[name()='ss:Data' and starts-with(.,'begin:')]");
            }
            else
            {
                beginMarker = templatePart.SelectSingleNode(".//*[name()='ss:Data' and contains(.,'" + parentName + ".begin:')]");
            }
            XmlNode endMarker;
            if (beginMarker == null)
            {
                return false;
            }
            else
            {
                tableName = beginMarker.InnerText.Split(' ')[0].Split(':')[1];
                endMarker = templatePart.SelectSingleNode(".//*[name()='ss:Data' and contains(.,'end:" + tableName + ";')]");
            }


            //string tableName;
            XmlNode table = data.SelectSingleNode("table[@as='" + tableName + "']");

            if (table == null & !data.Equals(rootData))
            {
                data = rootData;
                table = data.SelectSingleNode("table[@as='" + tableName + "']");

                string ss = table.Name;// чтобы вылетало
            }



            XmlNode buffer = templatePart.OwnerDocument.CreateElement("bufer");
            XmlNode workBuffer = templatePart.OwnerDocument.CreateElement("bufer");
            XmlNode last = null;
            XmlNode parent = null;
            if (sourceIndex == 1 & multipage)
            {
                XmlNode sheet = templatePart;
                parent = sheet.ParentNode;
                buffer.AppendChild(sheet);

            }
            else
            {
                XmlNode beginRow = beginMarker.ParentNode.ParentNode;
                XmlNode endRow = endMarker.ParentNode.ParentNode;
                XmlNode row = beginRow;
                parent = endRow.ParentNode;

                last = endRow.NextSibling;

                while (!row.Equals(last))
                {
                    XmlNode row1 = row.NextSibling;
                    buffer.AppendChild(row);
                    row = row1;
                    if (row == null)
                    {
                        break;
                    }
                }
            }

            if (beginMarker.ParentNode.ParentNode != null)
            {
                beginMarker.ParentNode.ParentNode.RemoveChild(beginMarker.ParentNode);
            }
            if (endMarker.ParentNode.ParentNode != null)
            {
                endMarker.ParentNode.ParentNode.RemoveChild(endMarker.ParentNode);
            }
            // beginMarker.InnerXml = "";
            // endMarker.InnerXml = "";


            if (table == null)
            {
                return false;
            }
            //SortedList<string, int> columnsIndexes;
            //SortedList<string, XmlNode> columnsInfo;
            SortedList<string, Tuple<int, XmlNode>> columns;
            if (columnsInfo.TryGetValue(tableName, out columns))
            {
                //columnsIndexes = columnsIndexesList[tableName];
                //columnsInfo = columnsInfoList[tableName];
            }
            else
            {
                //columnsIndexes = new SortedList<string, int>();
                //columnsInfo = new SortedList<string, XmlNode>();
                columns = new SortedList<string, Tuple<int, XmlNode>>();
                int i = 0;
                foreach (XmlNode dataColumn in data.SelectNodes("//scheme//table[@as='" + tableName + "']/columns/column"))
                {
                    string column_name = dataColumn.Attributes["name"].Value;
                    columns.Add(column_name, new Tuple<int, XmlNode>(i, dataColumn));
                    //columnsIndexes.Add(column_name, i);
                    //columnsInfo.Add(column_name, dataColumn);
                    i++;
                }
                columnsInfo.Add(tableName, columns);
                //columnsIndexesList.Add(tableName, columnsIndexes);
                //columnsInfoList.Add(tableName, columnsInfo);
            }
            foreach (XmlNode dataRow in table.SelectNodes("data/tr"))
            {
                workBuffer.InnerXml = "";
                copyChilds(buffer, workBuffer);
                applyValues(workBuffer, tableName, sourceIndex, dataRow, columns); //columnsIndexes, columnsInfo);
                XmlNode nextData = dataRow.SelectSingleNode("childs");
                if (nextData == null)
                {
                    nextData = rootData;
                }

                while (applySource(workBuffer, sourceIndex + 1, nextData, tableName, rootData, multipage, columnsInfo))
                {
                }
                ;
                while (workBuffer.ChildNodes.Count > 0)
                {
                    if (last != null)
                    {
                        parent.InsertBefore(workBuffer.ChildNodes[0], last);
                    }
                    else
                    {
                        parent.AppendChild(workBuffer.ChildNodes[0]);
                    }
                }
            }
            // workBuffer.InnerXml = buffer.InnerXml;
            //copyChilds(buffer, workBuffer);
            //copyChilds(buffer, workBuffer);

            return true;


        }
        public static void applyValues(XmlNode templatePart, string tableName, int sourceIndex, XmlNode row, SortedList<string, Tuple<int, XmlNode>> columns)//SortedList<string, int> columnsIndexes, SortedList<string, XmlNode> columnsInfo)
        {
            XmlNodeList values = templatePart.SelectNodes(".//*[name()='ss:Data' and contains(.,'value:" + tableName + ".')] | .//*[name()='ss:Cell' and contains(@ss:Formula,'value:" + tableName + ".')] ", excelNamespaseManager);
            XmlNode cells = row.SelectSingleNode("cells");
            if (templatePart.FirstChild.Attributes["ss:Name"] != null)
            {
                //<NamedRange ss:Name="Print_Titles" ss:RefersTo="='прил 5'!R6:R8"/>
                string oldSheetName = templatePart.FirstChild.Attributes["ss:Name"].Value;
                string newSheetName = cells.ChildNodes[columns["sid"].Item1].InnerText.Split('|')[1];//в качестве имени листа берется часть id строки, не крсиво - желательно переделать
                templatePart.FirstChild.Attributes["ss:Name"].Value = newSheetName;
                foreach (XmlNode namedRange in templatePart.SelectNodes(".//*[name()='ss:NamedRange']"))
                {
                    namedRange.Attributes["ss:RefersTo"].Value = namedRange.Attributes["ss:RefersTo"].Value.Replace(oldSheetName, newSheetName);
                }
            }

            foreach (XmlNode val in values)
            {
                string sData;
                if (val.Name == "ss:Data")
                {
                    sData = val.InnerText;
                }
                else
                {
                    sData = val.Attributes["ss:Formula"].Value;
                    sData = sData.Replace("+", " + ");
                    sData = sData.Replace("-", " - ");
                    sData = sData.Replace("/", " / ");
                    sData = sData.Replace("*", " * ");
                    sData = sData.Replace("(", " ( ");
                    sData = sData.Replace(")", " ) ");
                    sData = sData.Replace(";", " ; ");
                }
                int valBeg = sData.IndexOf("value:" + tableName + ".");
                string type = "String";

                while (valBeg > -1)
                {
                    int valEnd = sData.IndexOf(" ", valBeg);
                    if (valEnd == -1)
                    {
                        valEnd = sData.Length;
                    }
                    string sVal = sData.Substring(valBeg, valEnd - valBeg);
                    string columnName = sVal.Split(' ')[0].Split(':')[1].Split('.')[1];
                    Tuple<int, XmlNode> col_info = columns[columnName];
                    string value = cells.ChildNodes[col_info.Item1].InnerText;
                    if (valBeg == 0 & valEnd == sData.Length)
                    {
                        switch (col_info.Item2.Attributes["type"].Value)
                        {
                            case "number":
                                type = "Number";
                                value = value.Replace(",", ".");
                                break;
                            case "bool":
                                type = "Number";
                                value = value.Replace(",", ".");
                                break;
                            case "date":
                                type = "DateTime";
                                if (value != "")
                                {
                                    DateTime dat = Convert.ToDateTime(value);
                                    DateTime minExcelDate = new DateTime(1901, 1, 1);
                                    if (dat < minExcelDate)
                                    {
                                        dat = minExcelDate;
                                    }
                                    value = dat.ToString("yyyy-MM-dd");//+"T00:00:00.000";
                                }
                                break;
                        }
                    }
                    if (val.Name != "ss:Data")
                    {
                        if (value == "")
                        {
                            value = "R1C1000";  // !!! предполагается что это пустая ячейка
                        }
                    }
                    sData = sData.Replace(sVal, value);
                    valBeg = sData.IndexOf("value:" + tableName + ".");
                }
                if (val.Name == "ss:Data")
                {
                    val.Attributes["ss:Type"].Value = type;
                    if (!sData.Equals(""))
                    {
                        val.InnerText = sData;
                    }
                    else
                    {
                        val.ParentNode.RemoveChild(val);
                    }
                }
                else
                {
                    val.Attributes["ss:Formula"].Value = sData;
                }
            }
        }
        public static void copyChilds(XmlNode src, XmlNode tag)
        {
            XmlNode buffer = tag.OwnerDocument.CreateElement("bufer");
            buffer.InnerXml = src.InnerXml;
            while (buffer.ChildNodes.Count > 0)
            {
                tag.AppendChild(buffer.ChildNodes[0]);
            }
        }
        /*public static int getLastColumn(XmlNode workbook)
        {
            XmlNode beginMarker = workbook.SelectSingleNode(".//*[name()='ss:Data' and contains(.,'begin[1]')]");

            return getMarkerColumn(beginMarker);

        }



        public static int getMarkerColumn(XmlNode marker)
        {
            XmlNode cell = marker.ParentNode;
            XmlNode row = cell.ParentNode;
            int i = 0;
            //"ss:Index"
            foreach (XmlNode cell1 in row.ChildNodes)
            {
                XmlAttribute indexAtt = cell1.Attributes["ss:Index"];
                if (indexAtt == null)
                {
                    i++;
                }
                else
                {
                    i = Convert.ToInt32(indexAtt.Value);
                }
                if (cell1.Equals(cell))
                {
                    return i;
                }

            }
            return 0;
        }*/

        public static string GetFreeName(string path, string name, string ext)
        {

            string s = "";
            int i = 1;

            if (!string.IsNullOrEmpty(ext))
            {
                ext = "." + ext;
            }

            // корректировка xx.06.2021 YShmyreva
            // длина имени выходного файла не должна превышать ограничения Windows[260]/MsOffice[218]
            int max_lenFileName = 200;
            int name_maxLen = max_lenFileName - path.Length - 1;
            if (name.Length > name_maxLen)
            {
                name = name.Substring(0, name_maxLen);
            }
            ;

            string newName = name + ext;
            while (File.Exists(Path.Combine(path, newName)) || Directory.Exists(Path.Combine(path, newName)))
            {
                s = "(" + i + ")";
                newName = name + s + ext;
                i++;
            }
            return Path.Combine(path, newName);

        }

        // ищет подходящее имя для файла с новым расширением
        public static string GetFreeNameChangeExt(string filePath, string newExt)
        {
            string mask = @"(.*\\)([^\(]*)(\([0-9]*\))?(\..*)?";

            Match match = Regex.Match(filePath, mask);
            filePath = GetFreeName(match.Groups[1].Value, match.Groups[2].Value, newExt);

            return filePath;
        }
    }

    /// <summary>
    /// Настройки печати
    /// </summary>
    public class PrintOptions
    {
        /// <summary>
        /// Возвращает или задает настройку выбора строк для печати
        /// </summary>
        public PrintRowsMode PrintRowsMode { get; set; }

        /// <summary>
        /// Настройки по умолчанию
        /// </summary>
        public static PrintOptions Default
        {
            get
            {
                return new PrintOptions()
                {
                    PrintRowsMode = PrintRowsMode.AllRows
                };
            }
        }
    }

    /// <summary>
    /// Строки для печати: видимые или выделенные
    /// </summary>
    public enum PrintRowsMode
    {
        /// <summary>
        /// Печатаем все видимые строки
        /// </summary>
        AllRows,
        /// <summary>
        /// Печатаем только выделеные строки
        /// </summary>
        SelectedRows
    }
}
