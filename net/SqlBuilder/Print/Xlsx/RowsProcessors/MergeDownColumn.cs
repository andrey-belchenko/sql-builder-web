using System.Linq;
using System.Text.RegularExpressions;

namespace sql.builder.Print.Xlsx.RowsProcessors
{
    /// <summary>
    /// Информация о колонке для проведения merge_down
    /// </summary>
    public class MergeDownColumn
    {
        public string ColumnName { get; private set; }
        public int ColumnID { get; private set; }
        public string Key { get; private set; }
        public string UseKey { get; private set; }
        public bool WithStartMarks { get; private set; }
        /// <summary>
        /// Колонка от которой зависят мерджи в текущей
        /// </summary>
        public MergeDownColumn Prev { get; set; }

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