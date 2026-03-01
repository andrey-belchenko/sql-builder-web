using System.Data;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Mask;
//using DevExpress.XtraEditors.Repository;
using sql.builder.DataApi;

namespace sql.builder.UI
{
    public partial class UINumber : UIBase
    {
        private decimal step;
        public UINumber()
        {
            //InitializeComponent();
        }
        public override void Initialize(XElement xfield, UIFormC form)
        {
            this.BaseInitialize(xfield, form, ReturnType.Simple, typeof(decimal), true);
            this.step = decimal.Parse(xfield.AttrOrDefault(AName.step, "0"));
            this.InitControl();
        }
        protected override void InitControl()
        {
            //seControl.SetStep(this.step);
            //seControl.ValueChanged += seControl_ValueChanged;
            //seControl.EditValueChanged += (sender, args) => SetSourceValue(seControl.EditValue);
        }
        void seControl_ValueChanged(object value)
        {
            SetSourceValue(value);
        }

        public override void SetControlValue(object value, int index = 1)
        {
            setValue(value);
            //seControl.EditValue = value;
        }
        public override void RefreshData()
        {
            if (this.UseDefaultQuery && this.data_set_default != null)
            {
                XElement master_values = this.OnNeedMasterValues(this);
                this.data_set_default.Refresh(master_values);
                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
                if (rows.Count > 0)
                {
                    this.setValue(rows[0][0]);
                    // seControl.EditValue = DataTableDefault.Rows[0][0];
                }
            }
            base.RefreshData();
        }
        //private object _value = null;
        private void setValue(object value)
        {
            this.SetCtrlValue(value);
            //seControl.SetValue(value);
        }

        public override string GetText()
        {
            if (this.GetCtrlValue() == null)
            {
                return string.Empty;
            }
            return this.GetCtrlValue().ToString();
            //return (seControl as DevExpress.XtraEditors.SpinEdit).Text;
        }

        public override void SetError(string text, int index = 1)
        {
            SetErr(text);
            // SetError(seControl, text);
            //seControl.ErrorText = text;
        }
        void seControl_Cleared()
        {

            var read_only = GetSourceReadOnly();
            if (read_only) return;

            if (!this.mandatory)
            {
                ClearSourceValues();
            }
            //throw new System.NotImplementedException();
        }
    }
}
