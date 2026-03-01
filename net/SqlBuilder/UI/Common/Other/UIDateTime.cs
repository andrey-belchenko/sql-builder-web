using System;
using System.Data;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.Utils;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Mask;
//using DevExpress.XtraEditors.Repository;

namespace sql.builder.UI
{
    public partial class UIDateTime : UIBase
    {
        public UIDateTime()
        {
            // InitializeComponent();
        }

        public override void Initialize(XElement xfield, UIFormC form)
        {
            BaseInitialize(xfield, form, ReturnType.Simple, typeof(DateTime), true);
            InitControl();
        }

        protected override void InitControl()
        {
        }

        void deControl_FocusLost()
        {
            OnFocusLost();
        }
        void deControl_ValueChanged(object value)
        {
            SetSourceValue(value);

        }
        private object _value = null;
        private void setValue(object value)
        {
            _value = value;
            //deControl.SetValue(_value);
        }
        public override void SetControlValue(object value, int index = 1)
        {
            setValue(value);
        }
        public override void RefreshData()
        {
            if (this.UseDefaultQuery && this.data_set_default != null)
            {
                XElement master_values = OnNeedMasterValues(this);
                this.data_set_default.Refresh(master_values);
                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
                if (rows.Count > 0)
                {
                    object value = UIDate.ConvertToDateTime(rows[0][0]);
                    this.setValue(value);
                }
            }
            else if (string.IsNullOrEmpty(TableName))
            { //(Form.FormUseType == UIFormC.UseType.ParamEditor)
                if (Cmn.IsNullOrDBNull(this._value) && this.mandatory)
                {
                    setValue(DateTime.Now);
                }
            }
            base.RefreshData();
        }
        public override string GetText()
        {
            if (Cmn.IsNullOrDBNull(this._value))
            {
                return null;
            }
            else
            {
                return this._value.ToString();
            }
        }
        public override void SetError(string text, int index = 1)
        {
            // SetError(deControl, text);
            this.SetErr(text);
            //deControl.ErrorText = text;
        }
    }
}