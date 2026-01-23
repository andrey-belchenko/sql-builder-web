////using System.Windows.Forms;
////using DevExpress.XtraEditors;

//namespace sql.builder.WinForms
//{
//    internal partial class frmReplaceText : XtraForm
//    {
//        public string SearchText
//        {
//            get { return memoExEdit1.Text; }
//            set { memoExEdit1.Text = value; }
//        }

//        public string ReplaceToText
//        {
//            get { return memoExEdit2.Text; }
//            set { memoExEdit2.Text = value; }
//        }

//        public bool CheckCase
//        {
//            get { return checkEdit1.Checked; }
//            set { checkEdit1.Checked = value; }
//        }

//        public frmReplaceText()
//        {
//            InitializeComponent();
//        }

//        private void btnAccept_Click(object sender, System.EventArgs e)
//        {
//            DialogResult = DialogResult.OK;
//        }

//        private void btnCancel_Click(object sender, System.EventArgs e)
//        {
//            DialogResult = DialogResult.Cancel;
//        }

//        private void frmReplaceText_Activated(object sender, System.EventArgs e)
//        {
//            memoExEdit1.Focus();
//        }
//    }
//}
