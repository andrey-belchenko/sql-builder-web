//using System;
//using System.Data;
//using System.Linq;
////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.Utils;
////using DevExpress.XtraEditors.Repository;

//namespace sql.builder.UI
//{
//    internal partial class UITextEx : UIBase
//    {
//        public UITextEx()
//        {
//           // InitializeComponent();
//        }

//        public override void Initialize(XElement xfield, UIFormC form)
//        {
//            base.BaseInitialize(xfield, form, ReturnType.Simple, typeof (string), true, true);
//            InitControl();
//        }

//        protected override void InitControl()
//        {
          
//            //meControl.Properties.CloseUpKey = new KeyShortcut(Keys.Return);
//           // meControl.EditValueChanged += (sender, args) => SetSourceValue(meControl.EditValue);

//            meControl.ValueChanged += meControl_ValueChanged;
//            //meControl.CustomDisplayText += CustomDisplayText;
//        }

//        void meControl_ValueChanged(object value)
//        {
//            SetSourceValue(value);
//        }

//        private void setValue(object value)
//        {
//            meControl.SetValue(value);
//        }

//        public override void SetControlValue(object value, int index = 1)
//        {
//            setValue( value);
//        }

//        public override RepositoryItem GetRepositoryItem()
//        {
//            var rep = new RepositoryItemMemoExEdit()
//            {
//                CloseUpKey = KeyShortcut.Empty
//            };

//            rep.ShowIcon = false;
//            rep.CustomDisplayText += CustomDisplayText;
//           // rep.Buttons[0].IsLeft = true;
//           // _repository = rep;
//            addRepositoryButtons(rep.Buttons);
//            return rep;
//        }

//        void CustomDisplayText(object sender, DevExpress.XtraEditors.Controls.CustomDisplayTextEventArgs e)
//        {
//            var text = (Cmn.Nvl(e.Value, null)) != null
//                   ? e.Value.ToString().Replace(Environment.NewLine, " ")
//                   : "";

//            e.DisplayText = text;
//        }
//        public override void RefreshData()
//        {
//            if (this.UseDefaultQuery && this.data_set_default != null) {
//                XElement master_values = this.OnNeedMasterValues(this);
//                this.data_set_default.Refresh(master_values);
//                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
//                if (rows.Count > 0) {
//                    this.setValue(rows[0][0]);
//                   // meControl.EditValue = DataTableDefault.Rows[0][0];
//                }
//            }
//            base.RefreshData();
//        }
//        public override void SetError(string text, int index = 1)
//        {
//            SetErr(text);
//            //SetError(meControl, text);
//            //meControl.ErrorText = text;
//        }
//    }
//}
