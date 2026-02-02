using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;
using System.Data;
//using System.Linq;
using System.Text;
using sql.builder;
using ExcelPrintErrors = sql.builder.ExcelPrintDocument.ExcelPrintErrors;

namespace sql.builder.Print.Xlsx
{
    internal partial class ExcelPrintDocument
    {
        #region поля
        private readonly ExcelPrintEnv env;
        private readonly ExcelPrintSheet first_sheet;
        private int printed_sheets_count;
        private int printed_rows_count;
        #endregion
        internal ExcelPrintDocument(ExcelPrintEnv env)
        {
            this.env = env;
            IList<ExcelWorksheet> list = this.env.Worksheets;
            Contract.Assume(list.Count > 0);
            ExcelPrintSheet prev_sheet = null;
            for (int index = 0; index < list.Count; index++) {
                ExcelWorksheet worksheet = list[index];
                if (!worksheet.NativeSheetName.StartsWith("--")) {
                    ExcelPrintSheet sheet = ExcelPrintSheet.Create(worksheet, this, prev_sheet);
                    if (this.first_sheet == null) {
                        this.first_sheet = sheet;
                    }
                    prev_sheet = sheet;
                }
            }
        }
        #region свойства
        internal ExcelPrintEnv Env { get { return this.env; } }
        #endregion
        #region событие Printing
        internal event Action<ExcelPrintDocument, ExcelPrintSheet, int, int> Printing;
        protected void OnPrinting(ExcelPrintSheet sheet)
        {
            if (this.Printing != null) {
                this.Printing.Invoke(this, sheet, this.printed_sheets_count, this.printed_rows_count);
            }
        }
        #endregion
        internal void NextPrintedSheet(ExcelPrintSheet sheet)
        {
            this.printed_sheets_count++;
            this.OnPrinting(sheet);
        }
        internal void NextPrintedRow(ExcelPrintSheet sheet)
        {
            this.printed_rows_count++;
            this.OnPrinting(sheet);
        }
        private void MarkUnprinted()
        {
            ExcelPrintSheet sheet = this.first_sheet;
            while (sheet != null) {
                sheet.MarkUnprinted();
                sheet = sheet.NextSheet;
            }
            this.printed_sheets_count = 0;
            this.printed_rows_count = 0;
        }
        internal ExcelPrintErrors Print(DataSet data, bool use_data_reader)
        {
            this.MarkUnprinted();
            ExcelPrintSheet sheet = this.first_sheet;
            while (sheet != null) {
                this.OnPrinting(sheet);
                sheet = sheet.Print(data, use_data_reader);
            }
            // удаляем ненапечатанные размазываемые листы
            sheet = this.first_sheet;
            while (sheet != null) {
                if (sheet is ExcelPrintMultiplicatedSheet && !sheet.Printed) {
                    this.env.DeleteWorksheet(sheet.Worksheet);
                }
                sheet = sheet.NextSheet;
            }
            if (this.printed_sheets_count > 0) {
                return ExcelPrintErrors.None;
            } else {
                return ExcelPrintErrors.NoData;
            }
        }
        internal void Save(string output_path)
        {
            this.env.Save(output_path);
        }
    }
}
