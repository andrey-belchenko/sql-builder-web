using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.Utils;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Mask;
//using DevExpress.XtraEditors.Repository;

namespace sql.builder.UI
{
    internal partial class UIDateTime : UIBase
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
            //if (this.mandatory)
            //{
            //    deControl.SetCanClear(false);
            //}
            //else
            //{
            //    deControl.SetCanClear(true);
            //}
            //deControl.ValueChanged += deControl_ValueChanged;
            //deControl.FocusLost += deControl_FocusLost;

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

        //public override RepositoryItem GetRepositoryItem()
        //{
        //    var rep = new RepositoryItemDateEdit
        //    {
        //        CloseUpKey = KeyShortcut.Empty
        //    };
        //    // Противная штука не показывает время без фокуса
        //    addRepositoryButtons(rep.Buttons);
        //    rep.Mask.MaskType = MaskType.RegEx;
        //    rep.Mask.EditMask =
        //        @"([012]?[1-9]|[123]0|31)\.(0?[1-9]|1[012])\.([123][0-9])?[0-9][0-9] (0?[0-9]|1[0-9]|2[0-4]):[0-5][0-9]";
        //    rep.DisplayFormat.FormatString = "";
        //    rep.DisplayFormat.FormatType = FormatType.None;
        //    rep.EditFormat.FormatString = "";
        //    rep.EditFormat.FormatType = FormatType.None;
        //    rep.Mask.UseMaskAsDisplayFormat = true;
        //    //rep.Buttons[0].IsLeft = true;

        //    // грязный хак, пока лучше не придумал
        //    // чтобы при неверном значении не сбрасывалось в 01.01.0001 
        //    string last_value = null;
        //    rep.EditValueChanged += (sender, args) =>
        //    {
        //        var edit = (sender as DateEdit);
        //        last_value = edit.EditValue != null ? edit.EditValue.ToString() : null;
        //    };
        //    rep.Validating += (sender, args) =>
        //    {
        //        var edit = (sender as DateEdit);
        //        if (edit.EditValue == null || last_value == "")
        //        {
        //            setValue(DBNull.Value);

        //             edit.EditValue = DBNull.Value;
        //        }
        //        else if (edit.EditValue.Equals(DateTime.MinValue))
        //        {
        //            edit.EditValue = _value;
        //        }
        //    };
        //    //_repository = rep;
        //    rep.ButtonClick += RepositoryEditor_ButtonClick;
        //    return rep;
        //}
        
        public override void RefreshData()
        {
            if (this.UseDefaultQuery && this.data_set_default != null) {
                XElement master_values = OnNeedMasterValues(this);
                this.data_set_default.Refresh(master_values);
                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
                if (rows.Count > 0) {
                    object value = UIDate.ConvertToDateTime(rows[0][0]);
                    this.setValue(value);
                }
            } else if  ( string.IsNullOrEmpty(TableName)) { //(Form.FormUseType == UIFormC.UseType.ParamEditor)
                if (Cmn.IsNullOrDBNull(this._value) && this.mandatory) {
                    setValue(DateTime.Now);
                }
            }
            base.RefreshData();
        }
        public override string GetText()
		{
            if (Cmn.IsNullOrDBNull(this._value)) {
                return null;
            } else {
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