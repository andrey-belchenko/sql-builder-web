using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.Print.Xlsx.RowsProcessors
{
    /// <summary>
    /// ��������� ��������� merge_down �� ������ MergeDownColumn ����������� �����
    /// </summary>
    public class MergeDownColumnProcessor
    {
        public MergeDownColumn MergeDownCol { get; private set; }
        public MergeDownColumnProcessor Prev { get; set; }
        public string LastProcessedText { get; private set; }
        public int ProcessedCellsCount { get { return _cellsCount; } }
        public bool IsActive { get; private set; }

        private ExcelCellInfo _cellFirst;
        private ExcelCellInfo _cellLast;
        private int _cellsCount = 0;
        private List<Tuple<ExcelCellInfo, ExcelCellInfo>> _merges;

        public MergeDownColumnProcessor(MergeDownColumn mergeDownColumn)
        {
            MergeDownCol = mergeDownColumn;
            _merges = new List<Tuple<ExcelCellInfo, ExcelCellInfo>>();
        }

        public void Process(XElement xcell, ExcelCellInfo cell)
        {
            bool needMerge = false;
            string text = "";
            XElement xv = xcell.Element(ns.Main.v);
            if (xv != null)
            {
                text = xv.Value;
            }
            // !!! ���� ������ ������ ���������� (� �������) �� �� ����� � xml � ��� �� ��������� c �������
            // !!! ������ � ������� ���� ����������, �� ����� ������ - ���������
            // !!! ��� ��� ��� �� ��� ����, �� �������� ������ ����� ���� ������ ������ ���-���� ����� � xml
            if (//!string.IsNullOrEmpty(LastProcessedText) && !string.IsNullOrEmpty(text) && 
                string.Equals(LastProcessedText, text))
            {
                // � ������� ����� ������ ���� ��������
                if (Prev == null || Prev.ProcessedCellsCount > 1)
                {
                    needMerge = true;
                }
            }
            if (needMerge)
            {
                _cellLast = cell;
                _cellsCount++;
            }
            else
            {
                SaveMerge();
                _cellFirst = cell;
                _cellLast = cell;
                _cellsCount = 1;
            }
            // ������� �������� ������ �� ���� ���������� ����� ����� ������
            if (_cellsCount > 1 && xcell.Attribute(ns.None.t) == null && xv != null)
            {
                xv.Remove();
            }
            LastProcessedText = text;
        }

        /// <summary>
        /// ���������� �������� ����� ����������, ������� ���������� ��������
        /// </summary>
        /// <returns></returns>
        public IEnumerable<Tuple<ExcelCellInfo, ExcelCellInfo>> GetMerges()
        {
            SaveMerge();

            var merges = _merges.ToArray();

            return merges;
        }

        /// <summary>
        /// ����������� ���� � ������� � �������� �����
        /// </summary>
        public void SaveMerge()
        {
            if (_cellsCount > 1)
            {
                _merges.Add(new Tuple<ExcelCellInfo, ExcelCellInfo>(_cellFirst, _cellLast));
            }

            _cellFirst = null;
            _cellLast = null;
            _cellsCount = 0;
        }

        public void Start()
        {
            IsActive = true;
        }

        public void Stop()
        {
            IsActive = false;
            SaveMerge();
        }
    }
}