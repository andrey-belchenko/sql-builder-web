using System.Linq;
using System.Text.RegularExpressions;

namespace sql.builder.Print.Xlsx.RowsProcessors
{
    /// <summary>
    /// Информация о колонке для проведения merge_down
    /// </summary>
    internal class MergeDownColumn
    {
        internal string ColumnName { get; private set; }
        internal int ColumnID { get; private set; }
        internal string Key { get; private set; }
        internal string UseKey { get; private set; }
        internal bool WithStartMarks { get; private set; }
        /// <summary>
        /// Колонка от которой зависят мерджи в текущей
        /// </summary>
        internal MergeDownColumn Prev { get; set; }

        public MergeDownColumn(int columnId, string mark, bool withStartMarks)
        {
            ColumnName = ExcelUtils.GetColumnName(columnId);
            ColumnID = columnId;
            WithStartMarks = withStartMarks; 

            // достаем key, если есть
            var result = Regex.Match(mark, @"\[key:([a-zA-Z0-9_]+)\]");
            Key = result.Success ? result.Groups[1].Value : "col" + ColumnID;

            // достаем use_key, если есть
            result = Regex.Match(mark, @"\[use_key:([a-zA-Z0-9_]+)\]");
            if (result.Success) UseKey = result.Groups[1].Value;
        }
    }
}