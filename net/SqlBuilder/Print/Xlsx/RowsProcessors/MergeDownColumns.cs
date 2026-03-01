using System.Collections.Generic;
using System.Linq;
using sql.builder.DataApi;

namespace sql.builder.Print.Xlsx.RowsProcessors
{
    /// <summary>
    /// Аккумулирует всю необходимую информацию для проведения merge_down в листе
    /// </summary>
    public class MergeDownColumns
    {
        public int BeginMergeRowsId { get; private set; }

        public MergeDownColumn[] Columns { get; private set; }
        public MergeDownColumns(IEnumerable<ExcelRow> rows)
        {
            var list = new List<MergeDownColumn>();

            var cols = rows.SelectMany(r => r.Cells)
                .Where(c => c.Text.StartsWith("[merge_down") || c.Text.StartsWith("[merge_start"))
                .OrderBy(c => c.CellInfo.ColumnID)
                .GroupBy(c => c.CellInfo.ColumnID)
                .ToArray();

            MergeDownColumn prev = null;
            foreach (var col in cols)
            {
                bool withStartMarks = col.Any(c => c.Text.StartsWith("[merge_start"));
                // пока usekey только в последней
                var mark = col.Select(c => c.Text).Last(t => t.StartsWith("[merge_down"));
                var mdCol = new MergeDownColumn(col.Key, mark, withStartMarks);

                // правая колонка зависит от левой если они соседние либо связаны по key-usekey
                if (mdCol.UseKey != null)
                {
                    mdCol.Prev = list.First(c => c.Key == mdCol.UseKey);
                }
                else if (prev != null && (col.Key - prev.ColumnID == 1))
                {
                    mdCol.Prev = prev;
                }

                list.Add(mdCol);
                prev = mdCol;

                // затираем метку
                //cell.SetValue("");

                //EndMergeRowsId = cell.CellInfo.RowID - 1;
            }

            Columns = list.ToArray();

            if (Columns.Length == 0)
            {
                return;
            }

            List<ExcelCell> cells2 = rows.SelectMany(r => r.Cells).Where(c => c.Text.StartsWith(TextConst.ExcelMarks.HeadMarker)).ToList();
            BeginMergeRowsId = (cells2.Count != 0) ? cells2.Max(c => c.CellInfo.RowID) + 1 : 1;
        }
    }
}