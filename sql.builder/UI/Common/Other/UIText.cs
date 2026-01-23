using System;
using System.Globalization;
using System.Data;
using System.Linq;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Mask;
//using DevExpress.XtraEditors.Repository;
using sql.builder.DataApi;

namespace sql.builder.UI
{
    internal partial class UIText : UIBase
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
            //beControl.SetValue(value);

            //if (ShowNulls)
            //{
            //    if (Used && Cmn.IsNull(value))
            //    {
            //        beControl.SetNullText(TextConst.NullPlaceholder);
            //    }
            //    else
            //    {
            //        beControl.SetNullText(null);
            //    }
            //}
        }

        //public override RepositoryItem GetRepositoryItem()
        //{
        //    //if (_repository == null)
        //    // {
        //    var rep = new RepositoryItemButtonEdit();
        //    rep.Buttons.Clear();
        //    addRepositoryButtons(rep.Buttons);

        //    rep.ButtonClick += RepositoryEditor_ButtonClick;
        //    // rep.Buttons.Add(createWarningButton());
        //    if (EditMask != null)
        //    {
        //        var mask = EditMask;
        //        if (mask.StartsWith("n", true, CultureInfo.CurrentCulture))
        //        {
        //            rep.Mask.MaskType = MaskType.Numeric;
        //        }
        //        else if (mask.StartsWith("s")) // заплатка
        //        {
        //            mask = mask.Substring(1, mask.Length - 1);
        //            rep.Mask.MaskType = MaskType.Simple;
        //            rep.Mask.SaveLiteral = false;

        //        }
        //        rep.Mask.EditMask = mask;
        //    }
        //    rep.Mask.UseMaskAsDisplayFormat = true;
        //    //rep.ButtonClick += (sender, args) =>
        //    //{
        //    //    MessageBox.Show("yep");
        //    //    //(sender as Control).Tag = btn_rep.Tag;
        //    //};

        //    //foreach (EditorButton btn in beControl.Properties.Buttons)
        //    //{
        //    //    rep.Buttons.Add(btn);
        //    //}
        //    // _repository = rep;
        //    //}

        //    return rep;
        //}

        public override void RefreshData()
        {
            if (this.UseDefaultQuery && this.data_set_default != null) {
                XElement master_values = this.OnNeedMasterValues(this);
                data_set_default.Refresh(master_values);
                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
                if (rows.Count > 0) {
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

        //protected override EditorButtonCollection buttonCollection()
        //{
        //    return (beControl as DevExpress.XtraEditors.ButtonEdit).Properties.Buttons; // временно
        //}

        //public override void AddEditorButton(EditorButton btn, EditorButton btn_rep, EventHandler on_click)
        //{
        //    _custom_buttons = true;

        //    beControl.Properties.Buttons.Add(btn);
        //    btn.Click += on_click;


        //    additionalButtons.Add(btn);
        //    //var rep = (_repository ?? GetRepositoryItem()) as RepositoryItemButtonEdit;
        //    //rep.Buttons.Add(btn_rep);
        //    //// если кнопок несколько - не отличить по какой был клик!
        //    //rep.Click += (sender, args) => (sender as Control).Tag = btn_rep.Tag;
        //    //rep.Click += on_click;

        //    ////beControl.Properties.TextEditStyle = TextEditStyles.DisableTextEditor;
        //    ////rep.TextEditStyle = TextEditStyles.DisableTextEditor;
        //}
    }
}
