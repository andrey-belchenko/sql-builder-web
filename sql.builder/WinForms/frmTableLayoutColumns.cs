//using System;
//using System.Data;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using infoenergo.core.Extensions;
//using infoenergo.ui.win.Forms;
//using sql.builder.DataApi;

//namespace sql.builder.WinForms
//{
//    partial class frmTableLayoutColumns : FormBase
//    {
//        DataTable _dt_types;
//        DataTable _dt_cols;
//        bool _default_cols;

//        public frmTableLayoutColumns()
//        {
//            InitializeComponent();
//            InitTable();
//        }

//        public void Initialize(XElement xfieldgroup)
//        {
//            var xlayoutcolumns = xfieldgroup.Element(TextConst.EName.LayoutColumns);
//            if (xlayoutcolumns == null) SetDefaultCols();
//            else
//            {
//                viewColsSettings.BeginUpdate();
//                _dt_cols.Clear();
//                foreach (var xcolumn in xlayoutcolumns.Elements(TextConst.EName.LayoutColumn))
//                {
//                    var index = _dt_cols.Rows.Count + 1;
//                    var type = xcolumn.AttrOrDef("type", "AutoSize");
//                    var value = Convert.ToDecimal(xcolumn.AttrOrDef("value", "0"));
//                    _dt_cols.Rows.Add(index, type, value);
//                }
//                viewColsSettings.EndUpdate();
//            }
//        }
//        public XElement GetLayoutColumns()
//        {
//            if (_default_cols) return null;

//            viewColsSettings.CloseEditor();

//            var xlayoutcolumns = new XElement(TextConst.EName.LayoutColumns);
//            foreach (var row in _dt_cols.AsEnumerable())
//            {
//                xlayoutcolumns.Add(
//                    new XElement(TextConst.EName.LayoutColumn, 
//                        new XAttribute(TextConst.AName.Type, row["type"]),
//                        new XAttribute(TextConst.AName.Value, Cmn.Nvl(row["value"], 0M))));
//            }

//            return xlayoutcolumns;
//        }

//        void InitTable()
//        {
//            _dt_types = new DataTable();
//            _dt_types.Columns.Add("name");
//            _dt_types.Rows.Add("AutoSize");
//            _dt_types.Rows.Add("Absolut");
//            _dt_types.Rows.Add("Percent");
//            rleType.DataSource = _dt_types;

//            _dt_cols = new DataTable();
//            _dt_cols.Columns.Add("index", typeof(int));
//            _dt_cols.Columns.Add("type");
//            _dt_cols.Columns.Add("value", typeof(decimal));

//            grColsSettings.DataSource = _dt_cols;
//        }

//        void SetDefaultCols()
//        {
//            viewColsSettings.BeginUpdate();
//            _dt_cols.Clear();
//            _dt_cols.Rows.Add(1, "AutoSize", 0M);
//            _default_cols = true;
//            viewColsSettings.EndUpdate();
//        }

//        private void viewColsSettings_CellValueChanged(object sender, DevExpress.XtraGrid.Views.Base.CellValueChangedEventArgs e)
//        {
//            _default_cols = false;
//        }

//        private void btnAddCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            _dt_cols.Rows.Add(_dt_cols.Rows.Count + 1, "AutoSize", 0M);
//        }
//        private void btnRemoveCol_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            if (_dt_cols.Rows.Count == 1) return;

//            var row = viewColsSettings.GetFocusedDataRow();
//            if (row == null) return;

//            _dt_cols.Rows.Remove(row);
//        }
//        private void btnSetDefault_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            SetDefaultCols();
//        }

//        private void btnCancel_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            DialogResult = DialogResult.Cancel;
//        }
//        private void btnAccept_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            DialogResult = DialogResult.OK;
//        }
//    }
//}
