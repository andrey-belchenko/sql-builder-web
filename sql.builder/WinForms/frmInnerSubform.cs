
//using System;
//using System.Collections.Generic;
////using System.Windows.Forms;
//using sql.builder.Controls;

//namespace sql.builder.WinForms
//{
//    [Obsolete("Переделал на frmDynamicEditor")]
//    internal partial class frmInnerSubform : frmBaseSqlBuilder
//    {
//        public frmInnerSubform()
//        {
//            InitializeComponent();
//        }

//        private void barButtonItem1_ItemClick(object sender, DevExpress.XtraBars.ItemClickEventArgs e)
//        {
//            var grids = Cmn.GetChildControlsOfType<ucGridBase>(this);
//            foreach (var grid in grids)
//            {
//                grid.AcceptChanges();
//            }

//            this.Hide();
//        }
//    }
//}
