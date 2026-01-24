using System;
using System.Data;
using System.Globalization;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Mask;
//using DevExpress.XtraEditors.Repository;
using sql.builder.DataApi;

namespace sql.builder.UI
{
    internal partial class UINumber : UIBase
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

        //public override RepositoryItem GetRepositoryItem()
        //{
        //    var rep = new RepositoryItemSpinEdit();
        //    //rep.Buttons.Add(createWarningButton());
        //    addRepositoryButtons(rep.Buttons);
        //    if (EditMask != null)
        //    {
        //        if (EditMask.StartsWith("n", true, CultureInfo.CurrentCulture)) rep.Mask.MaskType = MaskType.Numeric;
        //        rep.Mask.EditMask = EditMask;
        //    }
        //    rep.Mask.UseMaskAsDisplayFormat = true;
        //    if (this.step > 0) {
        //        rep.Increment = this.step;
        //    } else {
        //        rep.Buttons[0].Visible = false;
        //    }
        //    rep.KeyDown += seControl_KeyDown;

        //   // _repository = rep;

        //    return rep;
        //}
        
        
        public override void RefreshData()
        {
            if (this.UseDefaultQuery && this.data_set_default != null) {
                XElement master_values = this.OnNeedMasterValues(this);
                this.data_set_default.Refresh(master_values);
                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
                if (rows.Count > 0) {
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

        //private void seControl_KeyDown(object sender, KeyEventArgs e)
        //{
        //    // режим readonly
        //    //var read_only = GetSourceReadOnly();
        //    //if (read_only) return;

        //    //if (!Mandatory && e.KeyCode == Keys.Delete)
        //    //{
        //    //    ClearSourceValues();
        //    //}

        //    if (e.KeyCode == Keys.Delete)
        //    {
        //        seControl_Cleared();
        //    }
        //}

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
        //protected override EditorButtonCollection buttonCollection()
        //{
        //    return (seControl as DevExpress.XtraEditors.SpinEdit).Properties.Buttons;
        //}
        //public override void AddEditorButton(EditorButton btn, EditorButton btn_rep, EventHandler on_click)
        //{
        //    _custom_buttons = true;

        //    seControl.Properties.Buttons.Add(btn);
        //    btn.Click += on_click;

        //    var rep = (_repository ?? GetRepositoryItem()) as RepositoryItemButtonEdit;
        //    rep.Buttons.Add(btn_rep);
        //    // если кнопок несколько - не отличить по какой был клик!
        //    rep.Click += (sender, args) => (sender as Control).Tag = btn_rep.Tag;
        //    rep.Click += on_click;

        //    //beControl.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        //    //rep.TextEditStyle = TextEditStyles.DisableTextEditor;
        //}
    }
}
