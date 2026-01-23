//using System;
//using System.Collections.Generic;
//using System.Diagnostics;
//using System.IO;
//using System.Linq;
//using System.Xml.Linq;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraPivotGrid;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls.Grids.ReportViewModes
//{
//    internal partial class ucPivotNew : XtraUserControl
//    {
//        public Dictionary<string, IEnumerable<PivotGridField>> pivot_fields;

//        private string _top_table_name = null;

//        #region DataSource
//        private VDataSet _source;
//        public event UIEventHandler UIEvent;
//        public void SetDataSource(VDataSet source)
//        {
//            // чтобы небыло утечек
//            if (_source != null) DetachDataSourceEvents();

//            _source = source;

//            if (_source != null && _top_table_name != null) UpdatePivot();

//            AttachDataSourceEvents();
//        }
//        public VDataSet GetDataSource()
//        {
//            return _source;
//        }
//        public VDataTable GetTopTable()
//        {
//            if (_source == null) return null;

//            return (VDataTable)_source.Tables[_top_table_name];
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

//        public ucPivotNew()
//        {
//            InitializeComponent();

//            pivot_fields = new Dictionary<string, IEnumerable<PivotGridField>>();
//        }
//        public void BeginUpdate()
//        {
//            //
//        }
//        public void EndUpdate()
//        {
//            //
//        }
//        public void ExportToXlsx(string fullpath, string caption = null)
//        {
//            pivot.ExportToXlsx(fullpath);
//        }
//        public void LoadFromXml(XElement xscheme)
//        {
//            Parser.LoadPivotSettingsFromXml(xscheme.Parent, pivot_fields);
//            //if(_source != null && _top_table_name != null) UpdatePivot();
//        }
//        public void SaveToXml(XElement xscheme)
//        {
//            Parser.SavePivotSettingsToXml(xscheme.Parent, pivot_fields);
//        }
//        public void SetTopTable(string top_table_name)
//        {
//            _top_table_name = top_table_name;

//            if(_source != null) UpdatePivot();
//        }

//        public void UpdatePivot()
//        {
//            pivot.BeginUpdate();
//            pivot.Fields.Clear();
//            pivot.Fields.AddRange(pivot_fields[_top_table_name].ToArray());
//            pivot.DataSource = _source.Tables[_top_table_name];
//            pivot.EndUpdate();
//        }

//        private void pivot_CellSelectionChanged(object sender, EventArgs e)
//        {
//            //if (_layout_reloading) return;

//            decimal sum = 0;
//            int count = 0;

//            for (int i = 0; i < pivot.Cells.RowCount; i++)
//            {
//                for (int j = 0; j < pivot.Cells.ColumnCount; j++)
//                {
//                    var cell = pivot.Cells.GetCellInfo(j, i);
//                    if (!cell.Selected) continue;

//                    if (cell.Value is Decimal)
//                    {
//                        sum += (decimal)cell.Value;
//                    }
//                    count++;
//                }
//            }

//            teTotalSum.EditValue = sum;
//            teTotalCount.EditValue = count;
//        }

//        private void ButtonExportExcel_ItemClick(object sender, ItemClickEventArgs e)
//        {
//            string path = Path.Combine(Path.GetTempPath(), Path.GetRandomFileName() + ".xlsx");
//            ExportToXlsx(path);
//            Process.Start(path);
//        }
//        private bool _toolbar_visible = true;
//        public void SetToolbarVisible(bool visible)
//        {
//            _toolbar_visible = visible;
//            barToolbar.Visible = visible;
//        }

//        private bool _footer_visible = true;
//        public void SetFooterVisible(bool visible)
//        {
//            _footer_visible = visible;
//            barFooter.Visible = visible;
//        }
//    }
//}
