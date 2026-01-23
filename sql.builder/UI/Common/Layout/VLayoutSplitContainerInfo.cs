using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public class VLayoutSplitContainerInfo : VLayoutContainerInfo
    {
        public VLayoutSplitContainerInfo(VLayout controller, VLayoutGroupInfo parentGroup)
            :base(controller,parentGroup)
        {
            IsFiller = true;
            
        }


        //public IVLayoutSplitContainer GetTypedControl()
        //{
        //    return GetControl() as IVLayoutSplitContainer;
        //}

        public override void Show()
        {
            foreach (var node in Nodes)
            {
                //GetTypedControl().ShowItem (this, (VLayoutGroupInfo)node);
                node.Show();
            }
        }
        public override void AddChild(VLayoutNodeInfo node)
        {
            base.AddChild(node);
            
            node.sizePercent = 1;
            node.IsFiller = true;
            
        }

        public decimal GetAllSize()
        {
            decimal size = 0;

            foreach (var node in Nodes)
            {
                size += node.sizePercent;
            }
            return size;
        }

       
        public bool IsVertical = false;


        public override int GetMarginLeft()
        {
            return 0;
        }
        public override int GetMarginRight()
        {
            return 0;
        }
        public override int GetMarginTop()
        {
            return 0;
        }
        public override int GetMarginBottom()
        {
            return 0;
        }
        //public override IVLayoutNode GetControl()
        //{
        //    if (control == null)
        //    {
        //        control = layoutController.GetControlsFactory().CreateSplitContainer();
        //        GetTypedControl().Init(this);
        //    }
        //    return control;
        //}

        public int GetClientSize()
        {
            return 0;
            //var splitterWidth = GetTypedControl().GetSplitterWidth();

            //var val = - splitterWidth * (Nodes.Count() - 1);
           
            //if (IsVertical)
            //{
            //    val+= GetClientWidth();
            //}
            //else
            //{
            //    val+= GetClientHeight();
            //}
            //return val;
        }

        private  void processCollapsed()
        {
            //var allSize = (decimal)GetAllSize();
            //var clientSize = GetClientSize();
           
            //if (!IsVertical)
            //{
                
            //    decimal aditionalSize = 0;
            //    decimal otherSize = 0;
            //    foreach (VLayoutGroupInfo node in Nodes)
            //    {

            //        if (!node.IsExpanded())
            //        {
            //            var h = GetParentGroup().GetTypedControl().CollapsedHeight() + node.GetMarginTop() + node.GetMarginBottom();
            //            var adds = node.sizeTemp - h;
            //            if (adds < 0)
            //            {
            //                adds = 0;
            //            }
            //            aditionalSize += adds;
            //            node.sizeTemp = h;
            //        }
            //        else
            //        {
            //            otherSize += node.sizeTemp;
            //        }
            //    }
            //    if (aditionalSize > 0)
            //    {
            //        foreach (VLayoutGroupInfo node in Nodes)
            //        {
            //            if (node.IsExpanded())
            //            {
            //                node.sizeTemp += (node.sizeTemp / otherSize) * aditionalSize;
            //            }
            //        }
            //    }
            //}
        }
        public override void ResetAndCalculateChilds()
        {
            
            var allSize=(decimal)GetAllSize();
            var clientSize=GetClientSize();
            int valItog = 0;
            foreach (VLayoutGroupInfo node in Nodes)
            {

                node.sizeTemp = Convert.ToInt32(clientSize * (decimal)node.sizePercent / allSize);

            }
            processCollapsed();
            foreach (VLayoutGroupInfo node in Nodes)
            {
                int val = Convert.ToInt32( node.sizeTemp);
                valItog += val;

                if (node.GetNext() == null)
                {
                    val += (clientSize - valItog);
                }
               
                
                node.left = node.GetMarginLeft();
                node.top = node.GetMarginTop();
                if (IsVertical)
                {
                   
                    node.width =  val-node.GetMarginLeft()-node.GetMarginRight();
                    node.height = GetClientHeight()-node.GetMarginTop()-node.GetMarginBottom();
                }
                else
                {
                    node.height = val - node.GetMarginTop() - node.GetMarginBottom();
                    node.width = GetClientWidth() - node.GetMarginLeft() - node.GetMarginRight();
                }
                //не учтен magrin групп находящихся в nodes
                node.ResetAndCalculateChilds();
            }
           // GetTypedControl().SetSizes(sizes.ToArray());
        }

        
       
        public void SplitSizeChanged() 
        {
            //if (!Nodes.Where(n => !(n as VLayoutGroupInfo).IsExpanded()).Any())
            //{// корректно будет работать только для 2-х пока больше не нужно
            //    int i = 0;
            //    var sizes = GetTypedControl().GetSizes();
            //    foreach (VLayoutGroupInfo node in Nodes)
            //    {

            //        node.sizePercent = sizes[i];

            //        i++;
            //    }
            //}
            //GetLayoutController().hasChanges = true;
            //GetLayoutController().RefreshLayoutIfNeed();
        }
    }
    
    
}
