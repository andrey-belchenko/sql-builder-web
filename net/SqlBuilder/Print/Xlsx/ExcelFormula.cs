using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace sql.builder.Print.Xlsx
{
    public class ExcelFormula
    {
        string _text;

        bool _changed = false;

        List<IToken> _tokens;
        List<ExcelRefToken> _refs;
        ExcelCell _cell;

        //HashSet<string> _cellNames;
        //HashSet<string> _colNames; 

        public ExcelFormula(string text, ExcelCell cell)
        {
            _text = text;
            _cell = cell;

            // парсим формулу, выделяя диапозоны ячеек
            _tokens = new List<IToken>();
            _refs = new List<ExcelRefToken>();

            int delta = 0;
            int lastIndex = 0;
            MatchCollection matches = Regex.Matches(text, "[A-Z]+[0-9]+(:[A-Z]+[0-9]+)?");
            foreach (Match match in matches)
            {
                delta = match.Index - lastIndex;
                if (delta > 0)
                {
                    var to = new ExcelTextToken(text.Substring(lastIndex, delta));
                    _tokens.Add(to);
                }

                var rf = new ExcelRefToken(match.Value);
                _tokens.Add(rf);
                _refs.Add(rf);

                lastIndex = match.Index + match.Length;
            }

            delta = text.Length - lastIndex;
            if (delta > 0)
            {
                var to = new ExcelTextToken(text.Substring(lastIndex, delta));
                _tokens.Add(to);
            }

            //UpdateHash();
        }
        public string GetText()
        {
            if (_changed)
            {
                _text = string.Join(string.Empty, _tokens.SelectAsArray(t => t.GetText()));
                _changed = false;
            }
            return _text;
        }

        //public bool ContainsCell(string cellName)
        //{
        //    return _cellNames.Contains(cellName);
        //}

        //public bool ContainsColumn(string colName)
        //{
        //    return _colNames.Contains(colName);
        //}
        public void CopyColumns(string[] cols_to_copy)
        {
            int cols_bord1 = ExcelUtils.GetColumnNumber(cols_to_copy[0]);
            int cols_bord2 = ExcelUtils.GetColumnNumber(cols_to_copy[cols_to_copy.Length - 1]);
            // Формула в ячейке среди размазываемых колонок
            bool cell_inside_ref = (cols_bord1 <= _cell.CellInfo.ColumnID && _cell.CellInfo.ColumnID <= cols_bord2);
            foreach (ExcelRefToken rf in _refs)
            {
                int ref_bord1 = rf.Cell1.ColumnID;
                int ref_bord2 = rf.Cell2.ColumnID;

                // Диапазон включает все размазываемые колоноки
                bool ref_contains_cols = (ref_bord1 <= cols_bord1 && ref_bord2 >= cols_bord2);

                if (rf.IsRange && !cell_inside_ref && ref_contains_cols)
                {
                    // Включаем в диапазон все новые колонки
                    rf.ExtendToColumn(ref_bord2 + cols_to_copy.Length);
                    _changed = true;
                }
                else if (ref_bord1 >= cols_bord1)
                {
                    rf.Move(cols_to_copy.Length);
                    _changed = true;
                }
            }
        }
        public void DeleteColumns(IEnumerable<string> cols_to_delete)
        {
            int cols_bord1 = ExcelUtils.GetColumnNumber(cols_to_delete.First());
            int cols_bord2 = ExcelUtils.GetColumnNumber(cols_to_delete.Last());
            int[] cols = Enumerable.Range(cols_bord1, cols_bord2 - cols_bord1 + 1).ToArray();

            foreach (ExcelRefToken rf in _refs)
            {
                int ref_bord1 = rf.Cell1.ColumnID;
                int ref_bord2 = rf.Cell2.ColumnID;

                // проверяем на пересечение диапазонов ячеек и копируемых колонок
                int[] cols_deleted = Enumerable.Range(ref_bord1, ref_bord2 - ref_bord1 + 1).Intersect(cols).ToArray();
                if (cols_deleted.Length != 0)
                {
                    foreach (var c in cols_deleted)
                    {
                        rf.DeleteColumn(c);
                    }
                    // такого быть не должно
                    if (rf.IsEmpty()) throw new InvalidOperationException("Из формулы удалён весь диапазон ячеек");

                    _changed = true;
                }
                // все ячейки справа надо сдвинуть
                else if (ref_bord1 > cols_bord2)
                {
                    rf.Move(0 - cols_to_delete.Count());

                    _changed = true;
                }
            }
        }

        public void Move(int col_delta)
        {
            foreach (var r in _refs) r.Move(col_delta);
            _changed = true;
        }
    }

    public interface IToken
    {
        string GetText();
    }

    class ExcelTextToken : IToken
    {
        string _text;

        public ExcelTextToken(string text)
        {
            _text = text;
        }

        public string GetText()
        {
            return _text;
        }
    }
}