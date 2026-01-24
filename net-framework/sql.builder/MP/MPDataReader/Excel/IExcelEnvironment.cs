using System;

namespace sql.builder.MP
{
    public interface IExcelEnvironment: IDisposable
    {
        void LoadFile(string filePath);
        void SetCurrentSheet(string sheetName);
        int GetRowsCount();
        object GetCellValue(string sheetName, int rowIndex, int columnIndex);
        ExcelValue[][] GetRows(int firstRowIndex, int lastRowIndex,int firstColumnIndex);
    }
}