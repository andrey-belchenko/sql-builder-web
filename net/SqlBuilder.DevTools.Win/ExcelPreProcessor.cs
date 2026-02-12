using System;
using System.IO;
using System.Runtime.InteropServices;
using Microsoft.Office.Interop.Excel;

namespace SqlBuilder.DevTools.Win
{
    public static class ExcelPreProcessor
    {
        public static string ConvertToXlsx(string templatePath)
        {
            Application exApp = null;
            try
            {
                exApp = new Application();
                exApp.DisplayAlerts = false;
                Workbook wb = exApp.Workbooks.Open(templatePath);

                string outputPath = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xlsx");
                wb.SaveAs(outputPath,
                    XlFileFormat.xlOpenXMLWorkbook,
                    ReadOnlyRecommended: false,
                    AccessMode: XlSaveAsAccessMode.xlNoChange,
                    ConflictResolution: XlSaveConflictResolution.xlLocalSessionChanges);

                return outputPath;
            }
            finally
            {
                if (exApp != null)
                {
                    try
                    {
                        exApp.DisplayAlerts = false;
                        exApp.Visible = false;
                        exApp.Quit();
                    }
                    finally
                    {
                        foreach (Workbook workbook in exApp.Workbooks)
                        {
                            foreach (Worksheet worksheet in workbook.Worksheets)
                            {
                                Marshal.ReleaseComObject(worksheet);
                            }
                            Marshal.ReleaseComObject(workbook.Worksheets);
                            Marshal.ReleaseComObject(workbook);
                        }
                        Marshal.ReleaseComObject(exApp.Workbooks);
                        Marshal.ReleaseComObject(exApp);
                    }
                }
            }
        }
    }
}
