//using System;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Xml.Linq;
////using System.Windows.Forms;
//using Devart.Data.Oracle;
//using DevExpress.XtraBars;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraGrid.Views.Grid;
//using infoenergo.core.Extensions;
//using sql.builder.Controls.Grids;
//using sql.builder.Controls.Grids.ReportViewModes;
//using sql.builder.Controls.Grids.ReportViewModes.Dashboard;
//using sql.builder.DataApi;
//using sql.builder.UI;
//using sql.builder.XmlHelpers;

//namespace sql.builder.Controls
//{
//    internal partial class ucTableViewerContainerWF : XtraUserControl, IucTableViewerContainer
//    {
//        private IControlWithTableSourcePubl _controller;
//        public ucTableViewerContainerWF(IControlWithTableSourcePubl controller)
//        {
//            InitializeComponent();
//            this.Dock = DockStyle.Fill;
//            this.Disposed += ucGridContainerWF_Disposed;
//            this._controller = controller;
//        }
//        public event SimpleEventHandler VDisposed;
//        private void ucGridContainerWF_Disposed(object sender, EventArgs e)
//        {
//            if (this.VDisposed != null) {
//                this.VDisposed();
//            }
//        }
//        public event ValueChangeEventHandler TableLevelChanged;
//        private void cbTableLevels_EditValueChanged(object sender, EventArgs e)
//        {
//            if (this.TableLevelChanged != null) {
//                this.TableLevelChanged(((VGCcbItem)cbTableLevels.EditValue).Name);
//            }
//        }
//        public event ValueChangeEventHandler AllowMergeChanged;
//        private void ricMerge_CheckedChanged(object sender, EventArgs e)
//        {
//            if (this.AllowMergeChanged != null) {
//                this.AllowMergeChanged(ricMerge.ValueChecked == beMerge.EditValue);
//            }
//        }
//        public event ValueChangeEventHandler ViewModeChanged;
//        private void cbViewMode_EditValueChanged(object sender, EventArgs e)
//        {
//            if (this.ViewModeChanged != null) {
//                this.ViewModeChanged((TableViewMode)((VGCcbItem)cbViewMode.EditValue).ID);
//            }
//        }
//        private void rcbTableLevels_CustomDisplayText(object sender, CustomDisplayTextEventArgs e)
//        {
//            e.DisplayText = e.DisplayText.Trim();
//        }
//        public void SetMode(ControlMode mode)
//        {
//            if (mode == ControlMode.Data || mode == ControlMode.Select) {
//                this.HideBars();
//            }
//        }
//        public void HideBars()
//        {
//            this.barToolbar.Visible = false;
//            this.barFooter.Visible = false;
//        }
//        public void SetViewSelectVisibility(bool val)
//        {
//            this.cbViewMode.Visibility = val ? BarItemVisibility.Always : BarItemVisibility.Never;
//        }
//        public void SetTableSelectVisibility(bool val)
//        {
//            this.cbTableLevels.Visibility = val ? BarItemVisibility.Always : BarItemVisibility.Never;
//        }
//        public void SetAllowMergeVisibility(bool val)
//        {
//            this.beMerge.Visibility = val ? BarItemVisibility.Always : BarItemVisibility.Never;
//        }
//        public void SetViewModels(VGCcbItem[] items)
//        {
//            this.rcbViewMode.Items.Clear();
//            this.rcbViewMode.Items.AddRange(items);
//        }
//        public void SetFooterVisibility(bool val)
//        {
//            this.barFooter.Visible = val;
//        }
//        public void SetInFormStyle()
//        {
//            this.Margin = new System.Windows.Forms.Padding(0);
//        }
//        public void SetName(string val)
//        {
//            this.Name = val;
//        }
//        public void SetViewMode(VGCcbItem value)
//        {
//            this.cbViewMode.EditValue = value;
//        }
//        public void ClearTableLevels()
//        {
//            this.rcbTableLevels.Items.Clear();
//        }
//        public void SetTableLevels(VGCcbItem[] items)
//        {
//            this.rcbTableLevels.Items.AddRange(items);
//        }
//        public void SetTopTable(VGCcbItem value)
//        {
//            this.cbTableLevels.EditValue = value;
//        }
//        public void SetFocus()
//        {
//            this.Focus();
//        }
//        public void VDispose()
//        {
//            this.Dispose();
//        }
//        public void AddChild(object child)
//        {
//            this.Controls.Add((Control)child);
//        }
//        public void SetVisibleInLayout(bool value)
//        {
//            this._controller.SetVisibleInLayout(value);
//        }
//    }
//}