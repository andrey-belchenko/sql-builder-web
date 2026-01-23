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
//    internal partial class ucTableViewerContainer : /*IReportGrid,*/ IControlWithTableSource
//    {

//        private ucTableViewerContainerWF getControlAsWFControl()
//        {
//            return (ucTableViewerContainerWF)_control;
//        }


//        private void initControl(ControlMode mode)
//        {
//            //getControlAsWFControl().Disposed += OnDisposed;
//            //getControlAsWFControl().cbTableLevels.EditValueChanged += new System.EventHandler(this.cbTableLevels_EditValueChanged);
//            ////getControlAsWFControl().rcbTableLevels.CustomDisplayText += new DevExpress.XtraEditors.Controls.CustomDisplayTextEventHandler(this.rcbTableLevels_CustomDisplayText);//!!!
//            ////getControlAsWFControl().Load += new System.EventHandler(this.ucReportGridNew_Load);
//            //getControlAsWFControl().ricMerge.CheckedChanged += new System.EventHandler(this.ricMerge_CheckedChanged);
//            //getControlAsWFControl().cbViewMode.EditValueChanged += new System.EventHandler(this.cbViewMode_EditValueChanged);



//            getControl().VDisposed += OnDisposed;
//            getControl().TableLevelChanged += cbTableLevels_EditValueChanged;
//            getControl().AllowMergeChanged += ricMerge_CheckedChanged;
//            getControl().ViewModeChanged += cbViewMode_EditValueChanged;
//            _mode = mode;
//            getControl().SetMode(_mode);
//            //if (_mode == ControlMode.Data || _mode == ControlMode.Select)
//            //{

//            //    getControlAsWFControl().barTopToolbar.Visible = false;
//            //    getControlAsWFControl().barFooter.Visible = false;
//            //}
//        }

//        //private void ricMerge_CheckedChanged(object sender, EventArgs e)
//        //{
//        //    if (grid != null) grid.SetAllowMerge(getControlAsWFControl().ricMerge.ValueChecked == getControlAsWFControl().beMerge.EditValue);
//        //}


//        private void ricMerge_CheckedChanged(object value)
//        {
//            if (grid != null) grid.SetAllowMerge((bool)value);
//        }

//        private void setControlViewModels(VGCcbItem [] items) 
//        {
//            getControl().SetViewModels(items);
//              //getControlAsWFControl().rcbViewMode.Items.Clear();
//              //getControlAsWFControl().rcbViewMode.Items.AddRange(items);
          
//        }
//        private void clearControlTableLevels()
//        {
//            getControl().ClearTableLevels();
//            //getControlAsWFControl().rcbTableLevels.Items.Clear();
//        }
//        private void setControlTableLevels(VGCcbItem[] items)
//        {
//            //getControlAsWFControl().rcbTableLevels.Items.AddRange(items);
//            getControl().SetTableLevels(items);
//        }
//        internal void SetPrintingTime(string time)
//        {
//            ucTableViewerContainerWF control = this.getControlAsWFControl();
//            control.barFooter.Visible = true;
//            if (string.IsNullOrEmpty(time)) {
//                control.lPrintingTime.Visibility = BarItemVisibility.Never;
//            } else {
//                control.lPrintingTime.Visibility = BarItemVisibility.Always;
//                control.lPrintingTime.Caption = "Файл напечатан за " + time;
//            }
//        }
//        private string _forming_time;
//        internal string GetFormingTime()
//        {
//            return _forming_time;
//        }
//        internal void SetFormingTime(string time)
//        {
//            this._forming_time = time;
//            ucTableViewerContainerWF control = this.getControlAsWFControl();
//            control.barFooter.Visible = true;
//            if (string.IsNullOrEmpty(time)) {
//                control.lFormingTime.Visibility = BarItemVisibility.Never;
//            } else {
//                control.lFormingTime.Visibility = BarItemVisibility.Always;
//                control.lFormingTime.Caption = "Сформирован за " + time;
//            }
//        }
//        #if DEBUG
//        internal void SetAvgFormingTime(string time)
//        {
//            ucTableViewerContainerWF control = this.getControlAsWFControl();
//            control.barFooter.Visible = true;
//            if (string.IsNullOrEmpty(time)) {
//                control.lAvgFormingTime.Visibility = BarItemVisibility.Never;
//            } else {
//                control.lAvgFormingTime.Visibility = BarItemVisibility.Always;
//                control.lAvgFormingTime.Caption = "Среднее время формирования " + time;
//            }
//        }
//        #endif
//        private void closeForm()
//        {
//            getControlAsWFControl().FindForm().Close();
//        }

//        private void addChildToControl(object control)
//        {
//            getControl().AddChild(control);
//            //getControlAsWFControl().Controls.Add(control);
//        }

//        private void setControlViewSelectVisibility(bool val)
//        {
//            getControl().SetViewSelectVisibility(val);
//            //getControlAsWFControl().cbViewMode.Visibility = val ? BarItemVisibility.Always : BarItemVisibility.Never;
//        }

//        private void setControlTableSelectVisibility(bool val)
//        {
//            getControl().SetTableSelectVisibility(_table_select);
//            //getControlAsWFControl().cbTableLevels.Visibility = _table_select ? BarItemVisibility.Always : BarItemVisibility.Never;
         
//        }

//        private void setControlAllowMergeVisibility(bool val)
//        {
//            getControl().SetAllowMergeVisibility(val);
//            //if (val)
//            //{
//            //    getControlAsWFControl().beMerge.Visibility = BarItemVisibility.Always;
//            //}
//            //else
//            //{
//            //    getControlAsWFControl().beMerge.Visibility = BarItemVisibility.Never;
//            //}

//        }

//        private void setControlFooterVisibility(bool val)
//        {
//            getControl().SetFooterVisibility(val);
//            //getControlAsWFControl().barFooter.Visible = val;

//        }
//        private object getControlViewModeValue()
//        {
//            return getControlAsWFControl().cbViewMode.EditValue;

//        }

//        private object getControlTableLevelsValue()
//        {
//            return getControlAsWFControl().cbTableLevels.EditValue;

//        }

//        private string getControlTableLevelsValueName()
//        {
//            return ((VGCcbItem)getControlTableLevelsValue()).Name;

//        }

//        private TableViewMode getControlViewModeValueAsMode()
//        {
//            return (TableViewMode)((VGCcbItem)getControlViewModeValue()).ID;

//        }
//        private void setControlViewModeValue(VGCcbItem value)
//        {
//            getControl().SetViewMode(value);
//            //getControlAsWFControl().cbViewMode.EditValue = value;

//        }



//        private VGCcbItem _top_table = null;
//        private void setControlTableLevelsValue(VGCcbItem top_table_new)
//        {
//            if (!top_table_new.Equals(/*getControlAsWFControl().cbTableLevels.EditValue*/_top_table))
//            {
//                getControl().SetTopTable(top_table_new);
//                _top_table=top_table_new;
//                //getControlAsWFControl().cbTableLevels.EditValue = top_table_new;
//            }
//            else
//            {
//                // даже если значение не поменялось, событие должно сработать
//                //cbTableLevels_EditValueChanged(null, null);
//                cbTableLevels_EditValueChanged(top_table_new.Name);
//            }

//        }


//        private void addControlBarFooterToList(List<Bar> visible_bars)
//        {
//            if (UIStatic.IsWeb())
//            {
//                return;
//            }
//            visible_bars.Add(getControlAsWFControl().barFooter);
//        }

//        public void SetIsInForm()
//        {
//            getControl().SetInFormStyle();
//        }

//        public void SetControlName(string val)
//        {
//            getControl().SetName(val);
          
//        }

        

//    }

  
//}

