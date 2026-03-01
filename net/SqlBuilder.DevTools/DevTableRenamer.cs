using System.Collections.Generic;
using System.Text;

namespace SqlBuilderLib.DevTools
{
    /// <summary>
    /// Renames table references in SQL text using position details from the parser.
    /// </summary>
    public static class DevTableRenamer
    {
        /// <summary>
        /// Replaces table names in SQL text according to the rename dictionary.
        /// </summary>
        /// <param name="sqlText">Original SQL text</param>
        /// <param name="details">Table name to position mappings (from GetSourceTablesResult.Details)</param>
        /// <param name="renameDict">Old table name -> new table name mapping</param>
        /// <returns>SQL text with table names replaced</returns>
        public static string RenameTables(
            string sqlText,
            Dictionary<string, List<PositionInfo>> details,
            Dictionary<string, string> renameDict)
        {
            if (string.IsNullOrEmpty(sqlText))
                return sqlText;
            if (details == null || details.Count == 0)
                return sqlText;
            if (renameDict == null || renameDict.Count == 0)
                return sqlText;

            // Build list of replacements: (startIndex, stopIndex, newName)
            // StopIndex is exclusive (position after last char)
            var replacements = new List<(int Start, int Stop, string NewName)>();

            foreach (var kvp in renameDict)
            {
                var oldName = kvp.Key;
                var newName = kvp.Value;
                if (string.IsNullOrEmpty(newName))
                    continue;

                if (details.TryGetValue(oldName, out var positions))
                {
                    foreach (var pos in positions)
                    {
                        replacements.Add((pos.StartIndex, pos.StopIndex, newName));
                    }
                }
            }

            if (replacements.Count == 0)
                return sqlText;

            // Sort by Start descending so we replace from end to start (avoids index shifting)
            replacements.Sort((a, b) => b.Start.CompareTo(a.Start));

            var sb = new StringBuilder(sqlText);
            foreach (var (start, stop, newName) in replacements)
            {
                if (start >= 0 && stop <= sb.Length && start < stop)
                {
                    sb.Remove(start, stop - start);
                    sb.Insert(start, newName);
                }
            }

            return sb.ToString();
        }
    }
}
