using System.Collections.Generic;

namespace sql.builder.WebReports.Client
{
    internal class SelectEditor : Editor
    {
        public List<string> Columns { get; set; }
        public string KeyField { get; set; }
        public string DisplayField { get; set; }

        public bool SingleSelection { get; set; }

        public List<string> ListItemsDeps { get; set; }

        public List<string> AllColumns()
        {
            var allCols = new List<string>();

            allCols.AddRange(Columns);

            if (!allCols.Contains(KeyField))
            {
                allCols.Add(KeyField);
            }

            if (!allCols.Contains(DisplayField))
            {
                allCols.Add(DisplayField);
            }
            return allCols;
        }
    }
}
