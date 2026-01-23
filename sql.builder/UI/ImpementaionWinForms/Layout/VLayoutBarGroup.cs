//using System;
//using System.Collections.Generic;
//using System.ComponentModel;
//using System.Drawing;
//using System.Data;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
////using DevExpress.XtraEditors;
////using DevExpress.XtraEditors.Controls;
////using DevExpress.XtraEditors.Mask;
////using DevExpress.XtraEditors.Repository;
//
////using DevExpress.XtraTreeList;
//using System.Runtime.InteropServices;
//using System.ComponentModel;
//using System.Diagnostics;
//using sql.builder.UI;
//using System.Reflection;
//
//namespace sql.builder.UI.WinForms
//{
//
//    internal partial class VLayoutBarGroup : XtraUserControl, IVLayoutGroup
//    {
//        [DllImport("user32.dll")]
//        public static extern int SendMessage(IntPtr hWnd, Int32 wMsg, bool wParam, Int32 lParam);
//
//        private const int WM_SETREDRAW = 11;
//        public VLayoutBarGroup()
//        {
//            InitializeComponent();
//
//            //barButtonItem1.Glyph = UIFormC.GetIcon("Refresh_24");
//        }
//
//        public void SetScrollHeight(int value)
//        {
//
//        }
//        public int CollapsedHeight()
//        {
//            return 25;
//        }
//        VLayoutGroupInfo Group = null;
//        public VLayoutGroupInfo GetInfo()
//        {
//            return Group;
//        }
//        int borderHeight = -1;
//        public int BorderHeight()
//        {
//            if (borderHeight == -1)
//            {
//                BarManager.ForceInitialize();
//                System.Reflection.FieldInfo fi = typeof(DevExpress.XtraBars.Bar).GetField("barControl", BindingFlags.NonPublic | BindingFlags.Instance);
//                DevExpress.XtraBars.Controls.CustomBarControl barControl = fi.GetValue(tbMain) as DevExpress.XtraBars.Controls.CustomBarControl;
//                Size s = barControl.Size;
//                borderHeight = s.Height;
//            }
//         
//            return borderHeight;
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
//      
//        public void SetText(string text)
//        {
//            //layoutControlGroup1.Text = text;
//        }
//
//        public void ShowNode(VLayoutNodeInfo node)
//        {
//            var itemControl = (Control)node.GetControl();
//            if (itemControl.Parent != panelControl2)
//            {
//                if (itemControl.Parent != null)
//                {
//                    itemControl.Parent.Controls.Remove(itemControl);
//                }
//            }
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
//        public static void SetNodeWidth(VLayoutGroupInfo group, VLayoutNodeInfo node)
//        {
//            var displacement = 0;
//            if (node is VLayoutTabsInfo)
//            {
//                displacement = (node.GetControl() as VLayoutTabs2).WidthDisplacement();
//            }
//
//            var itemControl = (Control)node.GetControl();
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
//            SendMessage(panelControl2.Handle, WM_SETREDRAW, false, 0);
//        }
//
//        public void EndLayoutChange()
//        {
//            //if (Parent != null) Cmn.GetChildControlsOfType<Control>(Parent).ForEach(c => c.ResumeLayout());
//            //layoutControl1.EndUpdate();
//            //panelControl2.ResumeLayout();
//            SendMessage(panelControl2.Handle, WM_SETREDRAW, true, 0);
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
//
//
//        public IVBar GetBar()
//        {
//            return tbMain;
//        }
//    }
//}
