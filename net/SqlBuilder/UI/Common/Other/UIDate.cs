using System;
using System.Collections.Generic;
using System.Globalization;
using System.Data;
using System.Linq;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.Utils;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Mask;
//using DevExpress.XtraEditors.Repository;
using sql.builder.DataApi;

namespace sql.builder.UI
{
    public partial class UIDate : UIBase
    {
        #region static
        public static object ConvertToDateTime(object value)
        {
            if (value is DateTime) {
                return value;
            } else if (Cmn.IsNullOrDBNull(value)) {
                return null;
            } else {
                return (object)Convert.ToDateTime(value);
            }
        }
        public static string ValueToString(object value)
        {
            if (Cmn.IsNullOrDBNull(value)) {
                return null;
            } else if (value is DateTime) {
                return ((IFormattable)value).ToString("dd.MM.yyyy", CultureInfo.InvariantCulture);
            } else {
                return value.ToString();
            }
        }
        #endregion
        private object _value;
        public UIDate()
        {
           // InitializeComponent();
        }
        public override void Initialize(XElement xfield, UIFormC form)
        {
            this.BaseInitialize(xfield, form, ReturnType.Simple, typeof(DateTime), true, true);
            this.InitControl();
        }
        protected override void InitControl()
        {
            /*if (this.mandatory) {
                this.deControl.SetCanClear(false);
                //deControl.Properties.ShowClear = false;
                //deControl.Properties.AllowNullInput = DefaultBoolean.False;
            } else {
                deControl.SetCanClear(true);
            }*/
            //this.deControl.SetCanClear(!this.mandatory);
            //this.deControl.ValueChanged += this.deControl_ValueChanged;
            //this.deControl.FocusLost += this.deControl_FocusLost;
        }
        private void deControl_FocusLost()
        {
            this.OnFocusLost();
        }
        private void deControl_ValueChanged(object value)
        {
            this.SetSourceValue(value);
        }
        private void setValue(object value)
        {
            this._value = value;
            //this.deControl.SetValue(_value);
        }
        public override void SetControlValue(object value, int index = 1)
        {
            this.setValue(value);
        }
        public override void RefreshData()
        {
            if (this.UseDefaultQuery && this.data_set_default != null) {
                XElement master_values = this.OnNeedMasterValues(this);
                this.data_set_default.Refresh(master_values);
                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
                if (rows.Count > 0) {
                    object value = ConvertToDateTime(rows[0][0]);
                    this.setValue(value);
                }
            }
            // Емцов - затирает сохраненные значение
            //else if  ( string.IsNullOrEmpty(TableName)) //(Form.FormUseType == UIFormC.UseType.ParamEditor)
            //{
            //    if (Cmn.Nvl(deControl.EditValue ,null) == null && this.Mandatory == "1")
            //    {
            //        deControl.EditValue = DateTime.Now;
            //    }
            //}
            base.RefreshData();
        }
        public override string GetText()
		{
            return UIDate.ValueToString(this._value);
		}
        public override void SetError( string text, int index = 1)
        {
            //SetError(deControl, text);
            this.SetErr(text);
        }
    }
}