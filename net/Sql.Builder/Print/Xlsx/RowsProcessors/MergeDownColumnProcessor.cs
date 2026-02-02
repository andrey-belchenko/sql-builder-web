using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.Print.Xlsx.RowsProcessors
{
    /// <summary>
    /// Позволяет проводить merge_down на основе MergeDownColumn конкретного листа
    /// </summary>
    internal class MergeDownColumnProcessor
    {
        internal MergeDownColumn MergeDownCol {get; private set;}
        internal MergeDownColumnProcessor Prev { get; set; }
        internal string LastProcessedText { get; private set; }
        internal int ProcessedCellsCount { get { return _cellsCount; } }
        internal bool IsActive { get; private set; }

        private ExcelCellInfo _cellFirst;
        private ExcelCellInfo _cellLast;
        private int _cellsCount = 0;
        private List<Tuple<ExcelCellInfo, ExcelCellInfo>> _merges;

        public MergeDownColumnProcessor(MergeDownColumn mergeDownColumn)
        {
            MergeDownCol = mergeDownColumn;
            _merges = new List<Tuple<ExcelCellInfo, ExcelCellInfo>>();
        }

        internal void Process(XElement xcell, ExcelCellInfo cell)
        {
            bool needMerge = false;
            string text = "";
            XElement xv = xcell.Element(ns.Main.v);
            if (xv != null) {
                text = xv.Value;
            }
            // !!! если ячейка пустая изначально (в шаблоне) ее не будет в xml и она не смержится c другими
            // !!! ячейки в которых были переменные, но стали пустые - смержатся
            // !!! это как раз то что надо, но возможны лишние мержи если пустая ячейка все-таки будет в xml
            if (//!string.IsNullOrEmpty(LastProcessedText) && !string.IsNullOrEmpty(text) && 
                string.Equals(LastProcessedText, text)) {
                // в колонке слева ячейки тоже смержены
                if (Prev == null || Prev.ProcessedCellsCount > 1) {
                    needMerge = true;
                }
            }
            if (needMerge) {
                _cellLast = cell;
                _cellsCount++;
            } else {
                SaveMerge();
                _cellFirst = cell;
                _cellLast = cell;
                _cellsCount = 1;
            }
            // удаляем числовые данные из всех смерженных ячеек кроме первой
            if (_cellsCount > 1 && xcell.Attribute(ns.None.t) == null && xv != null) {
                xv.Remove();
            }
            LastProcessedText = text;
        }

        /// <summary>
        /// Возвращает итоговый набор диапазонов, которые необходимо смержить
        /// </summary>
        /// <returns></returns>
        internal IEnumerable<Tuple<ExcelCellInfo, ExcelCellInfo>> GetMerges()
        {
            SaveMerge();

            var merges = _merges.ToArray();

            return merges;
        }

        /// <summary>
        /// Заканчивает мерж в колонке и начинает новый
        /// </summary>
        internal void SaveMerge()
        {
            if (_cellsCount > 1)
            {
                _merges.Add(new Tuple<ExcelCellInfo, ExcelCellInfo>(_cellFirst, _cellLast));
            }

            _cellFirst = null;
            _cellLast = null;
            _cellsCount = 0;
        }

        internal void Start()
        {
            IsActive = true;
        }

        internal void Stop()
        {
            IsActive = false;
            SaveMerge();
        }
    }
}