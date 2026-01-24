using System.Xml.Linq;

namespace sql.builder.DataApi
{

    public interface IVTableDataAdapter
    {
        object[] GetSelectedCellsValues();

        object GetSource();

        int GetColumnsCount();

        int GetRowsCount();

        string GetFieldName(int columnIndex);

        object GetValue(int rowIndex, int columnIndex);
        void SetFocusedRow(int rowIndex);
        void SetValue(int rowIndex, string columnName,object value);
        VTDARowState GetRowState(int rowIndex);
        event sql.builder.UI.CellChangeEventHandler CellValueChanged;
        event sql.builder.UI.RowEventHandler RowAdded;
   
    }

    
  
      
    public enum VTDARowState
    {
        Unchanged,
        Added,
        Modified,
        Deleted,
       
    }
}
