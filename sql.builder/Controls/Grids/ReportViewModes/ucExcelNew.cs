//using System;
//using System.Collections.Generic;
//using System.Data;
////using System.Windows.Forms;
//using DevExpress.DashboardCommon;
//using DevExpress.XtraEditors;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraSpreadsheet;
//using sql.builder.DataApi;
//using sql.builder.Print.Xlsx;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucExcelNew : XtraUserControl
//    {
//        #region DataSource
//        private VDataSet _source;
//        public void SetDataSource(VDataSet source)
//        {
//            // чтобы небыло утечек
//            if (_source != null) DetachDataSourceEvents();

//            _source = source;

//            if (_source != null) AttachDataSourceEvents();
//        }
//        public VDataSet GetDataSource()
//        {
//            return _source;
//        }

//        private void AttachDataSourceEvents()
//        {
//            //
//        }
//        private void DetachDataSourceEvents()
//        {
//            //
//        }


//        #endregion
//        #region EventTags
//        public event UIEventHandler2 UIEvent2;
//        protected void RaiseUIEvent2(string tableName, string name, DataRow row, VDataColumn col)
//        {
//            if (UIEvent2 != null)
//            {
//                var args = new UIEventArgs2()
//                {
//                    TableName = tableName,
//                    Name = name,
//                    Table = (VDataTable)_source.Tables[tableName] ,
//                    Column = col,
//                    Row = row
//                };

//                UIEvent2(this, args);
//            }
//        }
//        #endregion

//        public ucExcelNew()
//        {
//            InitializeComponent();
//            foreach (Control ctrl in this.Controls)
//            {
//                ctrl.Visible = false;
//            }
//        }


//        private void excel_HyperlinkClick(object sender, HyperlinkClickEventArgs args)
//        {
//            args.Handled = true;

            

//           // var cell = excel.SelectedCell;
//            var ss = args.TargetUri.Replace("http://", "").Split('[');
//            var ss1 = ss[0].Split('.');
//            var tableName = ss1[0];
//            var columnName = ss1[1];
//            var keyValue = ss[1].Replace("]", "").Replace("/", "").Replace(SingleExcelPrintValue.HyperlinkSlashPlaceholder,"/");

//            var table = (VDataTable)_source.Tables[tableName];
//            var column = table.GetColumn(columnName);
//            var row = table.Rows.Find(keyValue);



//            // если синхронно, то глюк с выделением
//            var t = new Timer();
//            t.Interval = 100;
//            t.Tick += (sender1, e) =>
//            {
//                t.Stop();
//                RaiseUIEvent2(tableName, TextConst.AVEventName.DoubleClick, row, column);
               
//                t.Dispose();
//            };
//            t.Start();
            
          
//          //  excel.SelectedCell = cell;
            
//        }

    
//        public void LoadDocument(string path)
//        {// excel.Options.Import.ThrowExceptionOnInvalidDocument = true
//            excel.LoadDocument(path);
//            foreach (Control ctrl in this.Controls)
//            {
//                ctrl.Visible = true;
//            }
//        }

//        private void spreadsheetFormulaBarControl1_Load(object sender, EventArgs e)
//        {

//        }

//        private void excel_CellBeginEdit(object sender, SpreadsheetCellCancelEventArgs e)
//        {
//            e.Cancel = true;
//        }

//        private void excel_KeyDown(object sender, KeyEventArgs e)
//        {
//            e.Handled = true;
//        }

        
//    }
//}
