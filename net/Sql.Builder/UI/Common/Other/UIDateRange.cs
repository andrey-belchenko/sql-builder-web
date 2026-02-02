using System;
using System.Diagnostics.Contracts;
using System.Data;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.Utils;
//using DevExpress.XtraEditors; // DateEdit

namespace sql.builder.UI
{
    internal partial class UIDateRange : UIBase, IRange
    {
        public UIDateRange()
        {
            //InitializeComponent();
        }
        public override void Initialize(XElement xfield, UIFormC form)
        {
            this.BaseInitialize(xfield, form, ReturnType.SimpleRange, typeof(DateTime), true);
            this.InitControl();
        }
        protected override void InitControl()
        {
            //this.deControlFrom.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            //this.deControlTo.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            //if (this.mandatory) {
            //    this.deControlFrom.Properties.ShowClear = false;
            //    this.deControlTo.Properties.ShowClear = false;
            //    this.deControlFrom.Properties.AllowNullInput = DefaultBoolean.False;
            //    this.deControlTo.Properties.AllowNullInput = DefaultBoolean.False;
            //}
            //this.deControlFrom.EditValueChanged += this.FromEditValueChanged;
            //this.deControlTo.EditValueChanged += this.ToEditValueChanged;
        }
        private void FromEditValueChanged(object sender, EventArgs e)
        {
            //Contract.Assert(object.ReferenceEquals(sender, this.deControlFrom));
            //this.SetSourceValue(this.deControlFrom.EditValue, 1);
        }
        private void ToEditValueChanged(object sender, EventArgs e)
        {
            //Contract.Assert(object.ReferenceEquals(sender, this.deControlTo));
            //this.SetSourceValue(this.deControlTo.EditValue, 2);
        }

        public override void SetControlValue(object value, int index)
        {

            var val = this.GetCtrlValue();
            if (val == null) {
                val = new object[] { null, null };
                this.SetCtrlValue(val);
            }

            (val as object[])[index] = value;
        }
        public override void RefreshData()
        {
            if (this.UseDefaultQuery && this.data_set_default != null) {
                XElement master_values = this.OnNeedMasterValues(this);
                this.data_set_default.Refresh(master_values);
                DataTable dt = this.data_set_default.Tables[0];
                if (dt.Rows.Count > 0) {
                    DataRow row = dt.Rows[0];
                    object from_value = UIDate.ConvertToDateTime(row[0]);
                    object to_value;
                    if (dt.Columns.Count > 1) {
                        to_value = UIDate.ConvertToDateTime(row[1]);
                    } else {
                        to_value = from_value;
                    }

                    this.SetControlValue(from_value, 0);
                    this.SetControlValue(to_value, 1);
                    //this.deControlFrom.EditValue = from_value;
                    //this.deControlTo.EditValue = to_value;
                }
            } else if (this.Form.FormUseType == UIFormC.UseType.ParamEditor && this.mandatory) {
                //if (Cmn.IsNullOrDBNull(this.deControlTo.EditValue)) {
                //    this.deControlTo.EditValue = DateTime.Now;
                //}
                //if (Cmn.IsNullOrDBNull(this.deControlFrom.EditValue)) {
                //    this.deControlFrom.EditValue = DateTime.Now;
                //}
            }
            base.RefreshData();
        }
        public override void SetError(string text, int index = 1)
        {
            this.SetErr(text);
        }
        #region IRange
        void IRange.GetText(out string value_1, out string value_2)
        {
            var val = this.GetCtrlValue() as object[];
            value_1 = UIDate.ValueToString(val[0]);
            value_2 = UIDate.ValueToString(val[1]); 
        }
        #endregion
    }
}