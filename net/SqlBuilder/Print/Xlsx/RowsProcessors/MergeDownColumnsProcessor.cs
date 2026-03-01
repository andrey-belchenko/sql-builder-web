using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using sql.builder.DataApi;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.Print.Xlsx.RowsProcessors
{
    /// <summary>
    /// Позволяет проводить merge_down на основе MergeDownColumns конкретного листа
    /// </summary>
    public class MergeDownColumnsProcessor
    {
        MergeDownColumns _mergeDownColumns;
        public MergeDownColumnProcessor[] Processors { get; private set; }
        public MergeDownColumnsProcessor(MergeDownColumns mdc)
        {
            _mergeDownColumns = mdc;

            //MergeDownColumnProcessor lastProcessor = null;
            var list = new List<MergeDownColumnProcessor>();
            foreach (var mergeDownColumn in mdc.Columns)
            {
                var processor = new MergeDownColumnProcessor(mergeDownColumn);

                //if (lastProcessor != null && lastProcessor.MergeDownCol == mergeDownColumn.Prev)
                //{
                //    processor.Prev = lastProcessor;
                //}
                //lastProcessor = processor;

                if (mergeDownColumn.Prev != null)
                {
                    processor.Prev = list.First(p => p.MergeDownCol == mergeDownColumn.Prev);
                }

                list.Add(processor);

                if (!processor.MergeDownCol.WithStartMarks)
                {
                    processor.Start();
                }
            }

            Processors = list.ToArray();
        }

        public void ProcessRow(ExcelRow originalRow, XElement xrow)
        {
            if (Processors.Length == 0 || originalRow.RowID < _mergeDownColumns.BeginMergeRowsId)
            {
                return;
            }

            // стоит маркер что строка в мержен не учавствует
            var mergeIgnore = originalRow.Cells.FirstOrDefault(c => c.Text.StartsWith(TextConst.ExcelMarks.MergeIgnore));
            if (mergeIgnore != null)
            {
                foreach (var processor in Processors)
                {
                    processor.SaveMerge();
                }

                // затираем марку
                xrow.Elements(ns.main + "c").First(c => c.Value == mergeIgnore.Value).Value = "";

                return;
            }

            // алгоритм
            // MergeDownColumnProcessor создается для каждой колонки с меткой merge_down
            // xrow содержит набор ячеек для некоторых колонок (только для тех колонок в которых есть данные!)
            // перебираем ячейки xrow - если для ячейки есть MergeDownColumnProcessor, передаем в метод Process ячейку для обработки
            // MergeDownColumnProcessor-ы, для которых нет ячеек с данными в текущей xrow, оповещаются методом SaveMerge - чтобы мердж по колонке не шел дальше

            var rowIdLength = xrow.Attribute("r").Value.Length;
            int rowId = int.Parse(xrow.Attribute("r").Value);

            XElement xcellCurrent = null;
            int _mergeColumnIdCurrent = 0;

            var originalCells = originalRow.Cells.ToArray();

            int i = 0;
            // перебираем ячейки с данными текущей строки
            foreach (var xcell in xrow.Elements(ns.main + "c"))
            {
                var originalCell = originalCells[i++];

                string cellName = xcell.Attribute("r").Value;
                string columnName = cellName.Substring(0, cellName.Length - rowIdLength);
                int columnId = ExcelUtils.GetColumnNumber(columnName);

                // ищем для текущей ячеки мержащуюся колонку
                while (_mergeColumnIdCurrent < Processors.Length)
                {
                    var processor = Processors[_mergeColumnIdCurrent];

                    if (columnId > processor.MergeDownCol.ColumnID)
                    {
                        processor.SaveMerge();
                        _mergeColumnIdCurrent++;
                    }
                    else if (columnId == processor.MergeDownCol.ColumnID)
                    {
                        if (originalCell.Text.StartsWith("[merge_start"))
                        {
                            processor.Start();
                            xcell.Value = "";
                            _mergeColumnIdCurrent++;
                            continue;
                        }
                        else if (originalCell.Text.StartsWith("[merge_down"))
                        {
                            processor.Stop();
                            xcell.Value = "";
                            _mergeColumnIdCurrent++;
                            continue;
                        }

                        if (!processor.IsActive)
                        {
                            _mergeColumnIdCurrent++;
                            continue;
                        }

                        var cell = new ExcelCellInfo(rowId, columnId);
                        Contract.Assume(cell.ColumnName == columnName);
                        Contract.Assume(cell.CellName == cellName);

                        processor.Process(xcell, cell);
                        _mergeColumnIdCurrent++;
                    }
                    else
                    {
                        break;
                    }
                }

                // закончили
                if (_mergeColumnIdCurrent == Processors.Length) break;
            }
        }
        public IEnumerable<Tuple<ExcelCellInfo, ExcelCellInfo>> GetMerges()
        {
            return Processors.SelectMany(p => p.GetMerges());
        }
    }
}