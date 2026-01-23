////using System.Windows.Forms;
////using DevExpress.XtraEditors;
//using sql.builder.UI;
//using sql.builder.Controls;
//namespace sql.builder.UI.WinForms
//{
//    internal partial class VLayoutControlContainer : XtraUserControl, IVLayoutControlContainer
//    {
//        public VLayoutControlContainer()
//        {
//            InitializeComponent();
//        }
//
//        public void Show(VLayoutControlContainerInfo item)
//        {
//           // var itemControl = (VLayoutControlContainer)item.GetControl();
//            Control containedControl = null;
//
//            var cc = item.GetContainedControl();
//            //containedControl = (cc is UIBase) ? (cc as UIBase).GetRootControl() : cc as Control;
//            containedControl = cc as Control;
//            if (containedControl == null)
//            {
//                //if (cc is ucTableViewerContainer)
//                //{
//                //    containedControl = (Control)(cc as ucTableViewerContainer).GetControl();
//                //}
//                //else
//                //{
//                    return;
//                //}
//
//            }
//
//            if (containedControl.Parent == null)
//            {
//                if (item.ControlDock == VLayout.Dock.Right)
//                {
//                    containedControl.Dock = DockStyle.Right;
//                }
//                else
//                {
//                    containedControl.Dock = DockStyle.Fill;
//                }
//                Controls.Add(containedControl);
//                //setpainttest(containedControl);
//
//                //foreach (Control ctrl in containedControl.Controls)
//                //{
//                //    ctrl.Paint += panelControl2_Paint;
//                //}
//            }
//            //containedControl.Width = item.GetWidth();
//           // containedControl.Height = 20;
//        }
//        int height = -1;
//        public int GetHeight(VLayoutControlContainerInfo item)
//        {
//            if (height != -1)
//            {
//                return height;
//            }
//
//            var btn = item.GetContainedControl() as SimpleButton;
//            var uibase = item.GetContainedControl() as UIBase;
//            if (btn != null && btn.Image!=null )
//            {
//                height = btn.CalcBestSize().Height;
//                return height;
//            }
//            else if (uibase != null)
//            {
//                return uibase.GetHeight();
//            }
//            else
//            {
//                return 20;
//            }
//            
//        }
//
//
//        public int WidthDisplacement(VLayoutControlContainerInfo item)
//        {
//            var cc = item.GetContainedControl() as VCheckContainer;
//            if (cc != null)
//            {
//                return cc.GetWidthDisplacement();
//            }
//            else
//            {
//                return 0;
//            }
//           
//            
//        }
//        //private void setpainttest(Control ctrl)
//        //{
//        //    ctrl.Paint += panelControl2_Paint;
//
//        //    foreach (Control ctrl1 in ctrl.Controls)
//        //    {
//        //        setpainttest(ctrl1);
//        //    }
//        //}
//
//        //private void panelControl2_Paint(object sender, PaintEventArgs e)
//        //{
//        //    e.Graphics.Clear(Color.Aqua);
//        //}
//
//
//        public void SetLabel(IVLayoutLabel label)
//        {
//            //throw new System.NotImplementedException();
//            // нужно для веба
//        }
//		public void SetHint(string hint)
//		{
//			//throw new System.NotImplementedException();
//			// нужно для веба
//		}
//    }
//}
