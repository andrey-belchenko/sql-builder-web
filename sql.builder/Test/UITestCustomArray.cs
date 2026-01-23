//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;

//using infoenergo.core.Data;
//using infoenergo.core.Extensions;
//using sql.builder.UI;

//namespace sql.builder.Test
//{
//    internal partial class UITestCustomArray : UserControl, ICustom
//    {
//        bool _ignore_changes;
//        DataTable _dt;

//        public UITestCustomArray()
//        {
//            InitializeComponent();

//            _dt = DataHelper.SqlGetTable("select sname, kodp from kr_org where kod_ecls = 4 and kod_separator = 2 order by sname", db.Connection);
//            gridControl1.DataSource = _dt;
//        }
//        public Control GetControl()
//        {
//            return this;
//        }
//        public int GetControlHeight()
//        {
//            return 400;
//        }
//        public object GetDataValue()
//        {
//            var value = gridView1.GetSelectedRows().Select(i => gridView1.GetDataRow(i)["sname"]);
//            return value;
//        }
//        public void SetDataValue(object value)
//        {
//            _ignore_changes = true;

//            gridView1.ClearSelection();

//            IEnumerable<object> collection = value as IEnumerable<object>;
//            foreach (object val in collection)
//            {
//                DataRow row = _dt.Rows.Find(val);
//                gridView1.SelectRow(gridView1.GetRowHandle(_dt.Rows.IndexOf(row)));
//            }

//            _ignore_changes = false;

//            RaiseValueChanged();
//        }
//        public bool IsDataEmpty()
//        {
//            return gridView1.GetSelectedRows().IsEmpty();
//        }
//        public void ClearDataValue()
//        {
//            _ignore_changes = true;

//            gridView1.ClearSelection();

//            _ignore_changes = false;

//            RaiseValueChanged();
//        }
//        public event EventHandler ValueChanged;

//        private void gridView1_SelectionChanged(object sender, DevExpress.Data.SelectionChangedEventArgs e)
//        {
//            if (_ignore_changes) return;

//            RaiseValueChanged();
//        }

//        void RaiseValueChanged()
//        {
//            if (ValueChanged != null)
//            {
//                ValueChanged(this, EventArgs.Empty);
//            }
//        }
//    }
//}
