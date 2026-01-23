//using System;
////using System.Windows.Forms;
////using DevExpress.XtraEditors;
//using System.Runtime.InteropServices;
//
//namespace sql.builder.UI.WinForms
//{
//
//    internal partial class VLayoutGroup : XtraUserControl, IVLayoutGroup
//    {
//        [DllImport("user32.dll")]
//        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
//
//        private const int WM_SETREDRAW = 11;
//        public VLayoutGroup()
//        {
//            InitializeComponent();
//        }
//
//        public IVBar GetBar()
//        {
//            return null;
//        }
//        VLayoutGroupInfo Group = null;
//        public VLayoutGroupInfo GetInfo()
//        {
//            return Group;
//        }
//
//        public void SetScrollHeight(int value)
//        {
//            trickPanel.Top = value - trickPanel.Height - panelControl2.VerticalScroll.Value;
//        }
//        public int CollapsedHeight()
//        {
//            return 25;
//        }
//        public void Init(VLayoutGroupInfo group)
//        {
//            Group = group;
//            
//         //   layoutControlGroup1.GroupBordersVisible = group.HasBorder;
//           
//            this.DoubleBuffered = true;
//            //SetText(group.GetText());
//        }
//
//        public int ClientWidth(VLayoutGroupInfo group)
//        {
//          //  нужен размер без учета скрол баров
//            return panelControl2.Size.Width;
//        }
//
//        public int ClientHeight(VLayoutGroupInfo group)
//        {
//            //  нужен размер без учета скрол баров
//            return panelControl2.Size.Height;
//        }
//        public int BorderHeight()
//        {
//            return 0;
//        }
//        public void SetText(string text)
//        {
//            //layoutControlGroup1.Text = text;
//        }
//
//        public void ShowNode(VLayoutNodeInfo node)
//        {
//            var itemControl = (Control)node.GetControl();
//
//            if (itemControl.Parent != panelControl2)
//            {
//                if (itemControl.Parent != null)
//                {
//                    itemControl.Parent.Controls.Remove(itemControl);
//                }
//            }
//
//            if(itemControl.Parent == null) panelControl2.Controls.Add(itemControl);
//            itemControl.Visible = true;
//        }
//
//        public void HideNode(VLayoutNodeInfo node)
//        {
//            var itemControl = (Control)node.GetControl();
//            //panelControl2.Controls.Remove(itemControl);
//            itemControl.Visible = false;
//        }
//
//        public void SetNodeTop( VLayoutNodeInfo node)
//        {
//            var itemControl = (Control)node.GetControl();
//            itemControl.Top = node.GetTop() - panelControl2.VerticalScroll.Value;
//        }
//
//        public void SetNodeHeight(VLayoutNodeInfo node)
//        {
//            VLayoutGroup.SetNodeHeight(Group, node);
//        }
//
//     
//        public void SetNodeWidth(VLayoutNodeInfo node)
//        {
//            VLayoutGroup.SetNodeWidth(Group, node);
//        }
//
//        public void SetNodeLeft(VLayoutNodeInfo node)
//        {
//            var itemControl = (Control)node.GetControl();
//            itemControl.Left = node.GetLeft();
//        }
//
//
//        public static void SetNodeHeight(VLayoutGroupInfo group, VLayoutNodeInfo node)
//        {
//            var displacement = 0;
//            if (node is VLayoutTabsInfo)
//            {
//                displacement = (node.GetControl() as VLayoutTabs2).HeightDisplacement();
//            }
//            var itemControl = (Control)node.GetControl();
//            itemControl.Height = node.GetHeight() + displacement;
//        }
//
//
//        public static void SetNodeWidth(VLayoutGroupInfo group, VLayoutNodeInfo node)
//        {
//            var displacement = 0;
//            if (node is VLayoutTabsInfo)
//            {
//                displacement = (node.GetControl() as VLayoutTabs2).WidthDisplacement();
//             
//            }
//
//            if (node is VLayoutControlContainerInfo)
//            {
//                displacement = (node.GetControl() as VLayoutControlContainer).WidthDisplacement((VLayoutControlContainerInfo)node);
//            }
//             
//            var itemControl = (Control)node.GetControl();
//
//
//            
//            itemControl.Width = node.GetWidth() + displacement;
//        }
//
//        public void SetExpanded(bool value)
//        {
//           // layoutControlGroup1.Expanded = value;
//           
//        }
//        public void SetBold(bool value)
//        {
//            //if (value)
//            //{
//            //    layoutControlGroup1.AppearanceGroup.Font = new Font(layoutControlGroup1.AppearanceGroup.Font,FontStyle.Bold);
//            //}
//            //else
//            //{
//            //    layoutControlGroup1.AppearanceGroup.Font = new Font(layoutControlGroup1.AppearanceGroup.Font, FontStyle.Regular);
//            
//            //}
//
//
//        }
//        public bool Expanded()
//        {
//
//            return true;// layoutControlGroup1.Expanded;
//        }
//
//
//        public void BeginLayoutChange()
//        {
//            //if(Parent != null) Cmn.GetChildControlsOfType<Control>(Parent).ForEach(c => c.SuspendLayout());
//            //panelControl2.SuspendLayout();
//            //layoutControl1.BeginUpdate();
//            //this.Sto
//            //try
//            //{
//            //    SendMessage(panelControl2.Handle, WM_SETREDRAW, false, 0);
//            //}
//            //finally
//            //{
//            //}
//            if (!panelControl2.IsDisposed)
//            {
//                SendMessage(panelControl2.Handle, WM_SETREDRAW, false, 0);
//            }
//        }
//
//        
//
//        public void EndLayoutChange()
//        {
//            //if (Parent != null) Cmn.GetChildControlsOfType<Control>(Parent).ForEach(c => c.ResumeLayout());
//            //layoutControl1.EndUpdate();
//            //panelControl2.ResumeLayout();
//            if (!panelControl2.IsDisposed)
//            {
//                SendMessage(panelControl2.Handle, WM_SETREDRAW, true, 0);
//            }
//            this.Refresh();
//        }
//
//        private void layoutControl1_GroupExpandChanged(object sender, DevExpress.XtraLayout.Utils.LayoutGroupEventArgs e)
//        {
//            Group.ExpandChanged();
//            
//        
//        }
//
//        private void VLayoutGroup_Resize(object sender, EventArgs e)
//        {
//            if (this.FindForm() != null)
//            {
//
//                Group.SizeChanged();
//            }
//        }
//
//       
//        public void SetVerticalScrollBarVisibility(bool value)
//        {
//            verticalScrollBarVisible = value;
//        }
//        private bool verticalScrollBarVisible=false;
//
//       
//       
//
//        private void panelControl2_Layout_1(object sender, LayoutEventArgs e)
//        {
//            panelControl2.HorizontalScroll.Visible = false;
//            panelControl2.VerticalScroll.Visible = verticalScrollBarVisible;
//        }
//    }
//}
