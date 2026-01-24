using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

using sql.builder.Print.Xlsx.RowsProcessors;

namespace sql.builder.Print.Xlsx
{
    internal class WorksheetPrint : IDisposable
    {
        internal string RID { get; private set; }
        internal string FilePath { get; private set; }
        internal ExcelWorksheet Worksheet { get; private set; }
        internal ExcelPrintEnv Env { get; private set; }

        private ExcelWorksheetMerges _merges;
        internal ExcelWorksheetMerges Merges { get { return _merges ?? Worksheet.Merges; } }
        internal ExcelWorksheetCols Cols { get { return Worksheet.Cols; } }

        // переделать на примере merges
        // пока не реализовано
        //private XElement _xbreaks;
        //private Dictionary<XElement, List<XElement>> _breaks;

        //private Dictionary<string, string> _sharedFormulas;

        private class HyperlinkInfo
        {
            public string Cell;
            public string Target;
        }
        private List<HyperlinkInfo> _hyperLinksInfo = new List<HyperlinkInfo>();
        internal void AddHyperlink(string cell, string target)
        {
            var hi = new HyperlinkInfo();
            hi.Cell = cell;
            hi.Target = target;
            _hyperLinksInfo.Add(hi);
        }


        private HashSet<ExcelRow> _printedRows;
        internal IEnumerable<ExcelRow> PrintedRows { get { return _printedRows.AsEnumerable(); } }
        internal IEnumerable<ExcelRow> NotPrintedRows
        {
            get
            {
                return Worksheet.Rows.Where(r => !_printedRows.Contains(r)).AsEnumerable();
            }
        }

        private HashSet<ExcelRow> _deletedRows;
        internal IEnumerable<ExcelRow> DeletedRows { get { return _deletedRows.AsEnumerable(); } }

        internal ExcelRow LastPrintedRow { get; set; }
        internal int LastPrintedRowID { get; set; }

        StreamWriter _writer;

        internal MergeDownColumnsProcessor MergeDownColsP { get; private set; }

        internal WorksheetPrint(string rid, string file_path, ExcelWorksheet worksheet, ExcelPrintEnv env)
        {
            RID = rid;
            FilePath = file_path;
            Worksheet = worksheet;
            Env = env;

            MergeDownColsP = new MergeDownColumnsProcessor(worksheet.MergeDownCols);

            // переделать на примере merges
            //_breaks = new Dictionary<XElement, List<XElement>>();

            //_sharedFormulas = new Dictionary<string, string>();
            _printedRows = new HashSet<ExcelRow>();
            _deletedRows = new HashSet<ExcelRow>();
            LastPrintedRowID = 0;

            // буфер 4Мб чтобы постоянно не лезть на диск
            _writer = new StreamWriter(new FileStream(file_path, FileMode.Create), Encoding.UTF8, 4194304);
        }

        internal void MarkRowAsDeleted(ExcelRow row)
        {
            if (!_deletedRows.Contains(row)) _deletedRows.Add(row);
        }
        internal void UnmarkRowAsDeleted(ExcelRow row)
        {
            _deletedRows.Remove(row);
        }

        internal void CopyRowMerge(ExcelRow row, int newId)
        {
            if (_merges == null && Worksheet.Merges != null)
            {
                _merges = new ExcelWorksheetMerges(Worksheet.Merges);
            }

            if (Merges == null) return;

            Merges.CopyRowMerge(int.Parse(row.ID), newId);
        }
        internal void RemoveRowMerge(ExcelRow row)
        {
            if (_merges == null && Worksheet.Merges != null)
            {
                _merges = new ExcelWorksheetMerges(Worksheet.Merges);
            }

            if (Merges == null) return;

            Merges.DeleteRowMerge(int.Parse(row.ID));
        }

        /*internal void CopyRowBreak(ExcelRow row, int newId)
        {

        }*/
        /*internal void RemoveRowBreak(ExcelRow row)
        {

        }*/
        /*internal XElement CompileBreaksXml()
        {
            if (_xbreaks == null) return null;

            foreach (KeyValuePair<XElement, List<XElement>> breakInfo in _breaks)
            {
                breakInfo.Key.AddAfterSelf(breakInfo.Value);
                breakInfo.Key.Remove();
            }

            return _xbreaks;
        }*/

        internal void PrintText(string text)
        {
            _writer.Write(text);
        }
        internal void PrintRow(ExcelRow originalRow, XElement xml)
        {
            if (!_printedRows.Contains(originalRow)) _printedRows.Add(originalRow);

            MergeDownColsP.ProcessRow(originalRow, xml);

            PrintText(xml.ToString(SaveOptions.DisableFormatting));
        }
        ExcelWorksheetRels _worksheetRels = null;
        internal ExcelWorksheetRels GetWorksheetRels()
        {

            if (_worksheetRels == null)
            {
                //this.NativeSheetFileName

                var path = Path.Combine(Path.GetDirectoryName(FilePath), "_rels", Path.GetFileName(Worksheet.FilePath) + ".rels");

                var newPath = Path.Combine(Path.GetDirectoryName(FilePath), "_rels", Path.GetFileName(FilePath) + ".rels");

                if (path != newPath)
                {
                    File.Copy(path, newPath);
                }

                _worksheetRels = new ExcelWorksheetRels(newPath);
                Env.AddFileToList(_worksheetRels);
            }
            return _worksheetRels;
        }
       
        internal XElement GetHyperlinksXml()
        {
            if (_hyperLinksInfo.Count == 0) {
                return null;
            }
            //<hyperlinks>
            //    <hyperlink ref="B3" r:id="rId1"/>
            //</hyperlinks>
            XElement xml = new XElement(ns.Main.hyperlinks);
			int _hyperLinksCount = 0;
            foreach (var hi in _hyperLinksInfo)
            {
                // плохая идея, т.к. файл sheet.xml.rels не вседа присутствует
                // но если поменять параметры печати шаблона, то этот файл появится
                var rid = /*Worksheet.*/GetWorksheetRels().CreateHyperlinkRel(hi.Target);
                XElement xhl = new XElement(ns.Main.hyperlink);
                xhl.Add(new XAttribute(ns.None.ref_, hi.Cell));
                xhl.Add(new XAttribute(ns.Relsd.id, rid));
                xhl.Add(new XAttribute(ns.None.tooltip, "Открыть детализацию"));
				if (_hyperLinksCount == 65530) {
					break;
				}
				_hyperLinksCount += 1;
                xml.Add(xhl);
             
            }
            return xml;
        }

        internal XElement GetMergesXml()
        {
            var me = MergeDownColsP.GetMerges().ToArray();
            if (me.Length != 0)            {
                if (_merges == null && Worksheet.Merges != null)
                {
                    _merges = new ExcelWorksheetMerges(Worksheet.Merges);
                }

                var merged = new HashSet<string>();
                foreach (Tuple<ExcelCellInfo, ExcelCellInfo> mergeInfo in me)
                {
                    if (merged.Contains(mergeInfo.Item2.CellName)) continue;

                    var merge = Merges.GetContainedMerge(mergeInfo.Item1);
                    if(merge == null)
                    {
                        Merges.CreateMerge(mergeInfo.Item1, mergeInfo.Item2);
                    }
                    else
                    {
                        // если две merge_down колонки уже смержены - приходится химичить
                        // например в 42415
                        mergeInfo.Item2.SetColumnFrom(merge.LastCell);
                        Merges.RemoveMergesInRange(mergeInfo.Item1, mergeInfo.Item2);
                        Merges.CreateMerge(mergeInfo.Item1, mergeInfo.Item2);

                        // чтобы не мержить уже смерженные
                        for (int i = mergeInfo.Item1.ColumnID; i <= mergeInfo.Item2.ColumnID; i++)
                        {
                            merged.Add(ExcelUtils.GetColumnName(i) + mergeInfo.Item2.RowID);   
                        }
                    }
                }   
            }

            return Merges.GetXml();
        }

        public void Dispose()
        {
            if (_writer != null) {
                _writer.Flush();
                _writer.Close();
                _writer = null;
            }
        }
    }
}