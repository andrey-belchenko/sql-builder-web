using System.IO;
using System.Linq;

using  sql.builder.MP.Tools;

namespace sql.builder.MP
{
    public class MPDataToFile: IMPDataStore
    {
        public void SaveData(IMPDataReader reader)
        {
            using (var writer = new StreamWriter(new FileStream(MPEnvironment.GetDataFilePath(), FileMode.Create, FileAccess.Write), MPEnvironment.DefaultEncoding))
            {
                bool first = true;
                while (reader.ReadNext(MPEnvironment.DataToFileBlockSize))
                {
                    // преобразуем в одну большую строку данных с разделителями строк и полей
                    string data = ((first) ? "" : MPEnvironment.RowsDelimiter) 
                        + string.Join(MPEnvironment.RowsDelimiter, 
                            reader.GetCurrentRows().Select(row => string.Join(MPEnvironment.FieldsDelimiter,
                                row.GetCells().Select(MPCellToString.Convert))));

                    writer.Write(data);

                    if (first) first = false;
                }
            }
        }
    }
}