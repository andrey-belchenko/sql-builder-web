namespace sql.builder.DataApi.TableDataAccessor
{
    /*public static class VTDAUtils
    {
        public static VTDAColumnInfo[] GetColumnsArray(IVTableDataAdapter tda)
        {
            var list = new List<VTDAColumnInfo>();
            var cc = tda.GetColumnsCount();
            for (int i = 0; i < cc; i++)
            {
                var col = new VTDAColumnInfo();
                col.Index = i;
                col.FieldName = tda.GetFieldName(i);
                list.Add(col);
            }
            return list.ToArray();
        }

        public static object[][] GetDataArray(IVTableDataAdapter tda)
        {
            var list = new List<object[]>();
            var rc = tda.GetRowsCount();
            var cc = tda.GetColumnsCount();
            for (int ri = 0; ri < rc; ri++)
            {
                var row = new List<object>();
                for (int ci = 0; ci < cc; ci++)
                {
                    row.Add(tda.GetValue(ri, ci));
                    
                }
                list.Add(row.ToArray());
            }
            return list.ToArray();
        }

        
        public static object[] GetRowArray(IVTableDataAdapter tda,int ri)
        {
        
           
            var cc = tda.GetColumnsCount();
           
            var row = new List<object>();
            for (int ci = 0; ci < cc; ci++)
            {
                row.Add(tda.GetValue(ri, ci));

            }
            return row.ToArray();
           
           
        }

        
      
    }

    public class VTDAColumnInfo
    {
        public string FieldName = null;
        public int Index;
    }*/
}
