using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public class VLayoutContainerInfo : VLayoutNodeInfo
    {


        public VLayoutContainerInfo(VLayout controller, VLayoutContainerInfo parentGroup)
        {
            layoutController = controller;
          
            if (parentGroup != null)
            {
                parentGroup.AddChild(this);
            }
        }


        public virtual void ResetTabs()
        {
            foreach (var node in Nodes)
            {
                if (node is VLayoutContainerInfo)
                {
                    (node as VLayoutContainerInfo).ResetTabs();
                }
            }
        }
       
        protected VLayout layoutController;
        public VLayout GetLayoutController()
        {
            return layoutController;
        }
        public LinkedList<VLayoutNodeInfo> Nodes = new LinkedList<VLayoutNodeInfo>();

        public VLayoutNodeInfo GetFirstVisible()
        {
            var node = Nodes.FirstOrDefault();
            if (node != null && !node.IsSelfVisible())
            {
                node = node.GetNextVisible();
            }
           
            return node;
        }

        public  VLayoutNodeInfo [] GetVisibleNodes()
        {
            return Nodes.Where(n=>n.IsSelfVisible()).ToArray();
        }
        
        public virtual void AddChild(VLayoutNodeInfo node)
        {
            node.SetParent(this);
            
            node.linkedListNode= new LinkedListNode<VLayoutNodeInfo>(node) ;
            Nodes.AddLast(node.linkedListNode);
            
        }

        private SortedList<int, int> nodesIndexes = null;
        public int GetNodeIndex(VLayoutNodeInfo node)
        {
            if (nodesIndexes == null)
            {
                nodesIndexes = new SortedList<int, int>();
                int i = 0;
                foreach (var n in Nodes)
                {
                    var id = (int)n.GetId();
                    if (id == -1)
                    {
                         GetLayoutController().AddNodeId(n);
                    }
                    id = (int)n.GetId();
                    nodesIndexes.Add(id, i);
                    i++;
                }
            }
            return nodesIndexes[node.id];
            
        }


        public virtual void AddFirst(VLayoutNodeInfo node)
        {
            if (node.GetParent() != null)
            {
                node.GetParent().RemoveChild(node);
            }
            node.SetParent(this);
            node.linkedListNode = new LinkedListNode<VLayoutNodeInfo>(node);
            Nodes.AddFirst (node.linkedListNode);
        }

        public virtual void RemoveChild(VLayoutNodeInfo node)
        {
            node.SetParent(null);
            Nodes.Remove(node.linkedListNode);
            node.linkedListNode = null;
        }

        public void AddBefore(VLayoutNodeInfo node, VLayoutNodeInfo next)
        {
            node.SetParent(this);
            node.linkedListNode = new LinkedListNode<VLayoutNodeInfo>(node);
            Nodes.AddBefore(next.linkedListNode, node.linkedListNode);
        }
        public virtual void ResetAndCalculateChilds()
        {
            
        }
        public virtual int GetControlBorderWidth()
        {
            return 0;
        }

        public virtual int GetControlBorderHeight()
        {
            return 0;
        }

        public virtual int GetClientWidth()
        {
            var val = 0;
            val += GetWidth() - GetControlBorderWidth();
            return val;
        }


        public virtual int GetClientHeight()
        {
            var val = 0;
            val += GetHeight() - GetControlBorderHeight();
            return val;
        }


    }
    
    
}
