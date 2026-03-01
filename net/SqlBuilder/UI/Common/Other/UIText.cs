using System;
using System.Data;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Mask;
//using DevExpress.XtraEditors.Repository;

namespace sql.builder.UI
{
    public partial class UIText : UIBase
    {
        public UIText()
        {
            //InitializeComponent();
        }

        public override void Initialize(XElement xfield, UIFormC form)
        {
            InitControl();
            BaseInitialize(xfield, form, ReturnType.Simple, typeof(string), true);

        }

        protected override void InitControl()
        {
            //beControl.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
            // beControl.EditValueChanged += (sender, args) => SetSourceValue(beControl.EditValue);
            //if (this is UITextArray)
            //{
            //    beControl.SetUsePlaneString();
            //}
            //beControl.ValueChanged += beControl_ValueChanged;
        }


        private string _text = "";
        public override string GetText() // Переделать , Все таки будем брать напрямую из контрола
        {
            return _text;
        }
        void beControl_ValueChanged(object value)
        {
            _text = Cmn.Nvl(value, "").ToString();
            SetSourceValue(value);
        }

        public override void SetControlValue(object value, int index = 1)
        {
            this.SetCtrlValue(value);
        }
        public override void RefreshData()
        {
            if (this.UseDefaultQuery && this.data_set_default != null)
            {
                XElement master_values = this.OnNeedMasterValues(this);
                data_set_default.Refresh(master_values);
                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
                if (rows.Count > 0)
                {
                    //this.beControl.SetValue(rows[0][0]);
                    this.SetCtrlValue(rows[0][0]);
                }
            }
            base.RefreshData();
        }



        public override void SetError(string text, int index = 1)
        {
            //SetError(beControl, text);
            SetErr(text);
            //  beControl.ErrorText = text;
        }

        private void teControl_Enter(object sender, EventArgs e)
        {
            Form.LastActiveField = this;
        }
    }
}
