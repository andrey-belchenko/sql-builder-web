////using System.Windows.Forms;
//using DevExpress.XtraEditors;
//using DevExpress.XtraTreeList;
//using sql.builder.UI;

//namespace sql.builder.Controls.Testing
//{
//    public partial class ucReportsAndParams : XtraUserControl
//    {
//        public bool ShowTimeColumns
//        {
//            get { return bandTime.Visible; }
//            set { bandTime.Visible = value; }
//        }

//        public bool ShowResultColumns
//        {
//            get { return bandResult.Visible; }
//            set { bandResult.Visible = value; }
//        }

//        public bool ShowParamsForm
//        {
//            get { return gcParams.Visible; }
//            set 
//            { 
//                gcParams.Visible = value;
//                splitterControl1.Visible = value; 
//            }
//        }

//        public bool EnableParamsForm
//        {
//            get { return gcParams.Enabled; }
//            set
//            {
//                gcParams.Enabled = value;
//            }
//        }

//        public TreeList TreeControl
//        {
//            get { return tlReports; }
//        }

//        public ucReportsAndParams()
//        {
//            InitializeComponent();
//        }

//        public void ShowParams(UIFormC form)
//        {
//            if(form != null)
//            {
//                var control = form.GetControl() as Control;
//                control.Dock = DockStyle.Fill;

//                gcParams.Controls.Clear();
//                gcParams.Controls.Add(control);   
//            }
//            else
//            {
//                gcParams.Controls.Clear();
//            }
//        }
//    }
//}
