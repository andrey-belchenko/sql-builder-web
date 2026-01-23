//using System;
//using System.Data;
////using System.Windows.Forms;
//using System.Xml.Linq;
////using DevExpress.XtraEditors;
////using DevExpress.XtraEditors.Controls;
////using DevExpress.XtraEditors.Repository;
//using sql.builder.UI.WinForms;
//namespace sql.builder.UI
//{
//    internal partial class UILink : UIBase
//    {
     

//        public UILink()
//        {
//           // InitializeComponent();
//        }

//        public override void Initialize(XElement xfield, UIFormC form)
//        {
//            BaseInitialize(xfield, form, ReturnType.Simple, typeof(string), true);
//            InitControl();
//        }
//        public const string TooltipText = "ctrl + клик чтобы перейти по ссылке";
//        protected override void InitControl()
//        {
//            // heControl.ErrorIconAlignment = ErrorIconAlignment.MiddleRight;
//            heControl.ValueChanged += heControl_ValueChanged;
//           // heControl.ToolTip = TooltipText;
//        }

     

//        public override void SetControlValue(object value, int index = 1)
//        {
//            heControl.SetValue(value);
//        }

//        public override RepositoryItem GetRepositoryItem()
//        {
//            //if (_repository == null)
//            //{
//                var rep = new RepositoryItemHyperLinkEdit();
//                addRepositoryButtons(rep.Buttons);
//                rep.TextEditStyle = TextEditStyles.Standard;
//                rep.SingleClick = true;
//                rep.MouseEnter += VLinkEdit.heControl_MouseEnter;
//                rep.MouseLeave += VLinkEdit.heControl_MouseLeave;
//                rep.MouseMove += VLinkEdit.heControl_MouseMove;
//                rep.OpenLink += VLinkEdit.heControl_OpenLink;
               
           
//              //  _repository = rep;
//            //}

//            return rep;
//        }
//        public override void RefreshData()
//        {
//            if (this.UseDefaultQuery && this.data_set_default != null) {
//                XElement master_values = this.OnNeedMasterValues(this);
//                this.data_set_default.Refresh(master_values);
//                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
//                if (rows.Count > 0) {
//                    this.heControl.SetValue(rows[0][0]);
//                }
//            }
//            base.RefreshData();
//        }
//        private string _text = "";
//        public override string GetText() // Переделать , Все таки будем брать напрямую из контрола
//        {
//            return _text;
//        }
//        void heControl_ValueChanged(object value)
//        {
//            SetSourceValue(value);
//            _text = Cmn.Nvl(value, "").ToString();
//        }
       

//        public override void SetError(string text, int index = 1)
//        {
//             SetErr(text);
//            //SetError(heControl, text);
//            //heControl.ErrorText = text;
//        }

//        private void teControl_Enter(object sender, EventArgs e)
//        {
//            Form.LastActiveField = this;
//        }
//        //protected override EditorButtonCollection buttonCollection()
//        //{
//        //    return heControl.Properties.Buttons;
//        //}
        

//        //private void heControl_MouseMove(object sender, MouseEventArgs e)
//        //{
//        //    UpdateCursor(sender as HyperLinkEdit);
//        //}

//        //private void heControl_MouseEnter(object sender, EventArgs e)
//        //{
//        //    UpdateCursor(sender as HyperLinkEdit);
//        //}

//        //private void heControl_MouseLeave(object sender, EventArgs e)
//        //{
//        //    UpdateCursor(sender as HyperLinkEdit);
//        //}

//        //private void UpdateCursor(HyperLinkEdit edit)
//        //{
//        //    if ((Control.ModifierKeys & Keys.Control) != 0 && edit.Text != "") edit.Controls[0].Cursor = Cursors.Hand;
//        //    else edit.Controls[0].Cursor = Cursors.Default;
//        //}

//        //private void heControl_OpenLink(object sender, OpenLinkEventArgs e)
//        //{
//        //    if ((Control.ModifierKeys & Keys.Control) == 0)
//        //    {
               
//        //        e.Handled = true;
//        //    }
//        //}
//    }
//}
