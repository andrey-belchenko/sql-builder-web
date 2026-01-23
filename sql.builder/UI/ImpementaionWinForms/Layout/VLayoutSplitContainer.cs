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
//
//namespace sql.builder.UI.WinForms
//{
//    internal partial class VLayoutSplitContainer : XtraUserControl, IVLayoutSplitContainer
//    {
//        public VLayoutSplitContainer()
//        {
//           
//            InitializeComponent();
//        }
//        SplitterControl splitter = new SplitterControl();
//        VLayoutSplitContainerInfo SplitContainer = null;
//        public void Init(VLayoutSplitContainerInfo splitContainer)
//        {
//            SplitContainer = splitContainer;
//            var dockStyle = DockStyle.Bottom;
//            if (splitContainer.IsVertical)
//            {
//                dockStyle = DockStyle.Right;
//            }
//            splitter.Dock = dockStyle;
//        }
//
//        bool first = true;
//        public void ShowItem(VLayoutSplitContainerInfo splitContainer, VLayoutGroupInfo item)
//        {
//
//          
//            var itemControl = (Control)item.GetControl();
//            PanelControl panel = null;
//            if (itemControl.Parent != null)
//            {
//                panel = (PanelControl)itemControl.Parent;
//            }
//            else
//            {
//                var dockStyle = DockStyle.Bottom;
//                if (splitContainer.IsVertical)
//                {
//                    dockStyle = DockStyle.Right;
//                }
//                panel = new PanelControl();
//                panel.Padding = new Padding(0);
//                panel.Margin = new Padding(0);
//               
//                first = false;
//                panel.BorderStyle = BorderStyles.NoBorder;
//                panel.Controls.Add(itemControl);
//                this.Controls.Add(panel);
//                if (item.GetNext() != null)
//                {
//                    splitter = new SplitterControl();
//                    splitter.Dock = dockStyle;
//                    splitter.SplitterMoved += splitterControl1_SplitterMoved;
//                    this.Controls.Add(splitter);
//                }
//
//                if (item.GetPrevious() == null)
//                {
//                    panel.Dock = DockStyle.Fill;
//                }
//                else
//                {
//                    panel.Dock = dockStyle;
//                }   
//            }
//            itemControl.Height = item.GetHeight();
//            itemControl.Width = item.GetWidth();
//            itemControl.Top = item.GetTop();
//            itemControl.Left = item.GetLeft();
//            panel.Height = item.GetHeight() + item.GetMarginTop() + item.GetMarginBottom();
//            panel.Width = item.GetWidth() + item.GetMarginRight() + item.GetMarginLeft();
//        }
//
//        public int [] GetSizes()
//        {
//            var list = new List<int>();
//            foreach (PanelControl panel in SplitContainer.Nodes.Select(n => ((Control)n.GetControl()).Parent))
//            {
//                if (SplitContainer.IsVertical)
//                {
//                    list.Add(panel.Width);
//                }
//                else
//                {
//                    list.Add(panel.Height);
//                }
//
//            }
//            return list.ToArray();
//        }
//
//       
//
//        public int GetSplitterWidth()
//        {
//            if (SplitContainer.IsVertical)
//            {
//                return splitter.Width;
//            }
//            else
//            {
//                return splitter.Height;
//            }
//        }
//
//       
//        private void splitterControl1_SplitterMoved(object sender, SplitterEventArgs e)
//        {
//            SplitContainer.SplitSizeChanged();
//        }
//
//       
//
//        
//  
//    }
//}
