using System;
using System.Collections.Generic;
//using System.Windows.Forms;
namespace sql.builder.UI
{
    public class VLayout : IDisposable
    {
        public enum Dock
        {
            Right,
            Fill
        }

        //Timer timer = new Timer() { Interval = 100 };

        public void ResetTabs()
        {
            GetMainGroup().ResetTabs();
        }

        public VLayout()
        {

            mainGroup = new VLayoutGroupInfo(this, null);
            GetMainGroup().IsFiller = true;
            GetMainGroup().HasBorder = false;

            //timer.Tick += (sender, args) =>
            //{
            //    timer.Stop();
            //    hasChanges = true;
            //    RefreshLayoutIfNeed();
            //};
        }
        public SortedList<int, VLayoutControlContainerInfo> containedControls = new SortedList<int, VLayoutControlContainerInfo>();

        public VLayoutControlContainerInfo GetItemByControl(object control)
        {
            return containedControls[control.GetHashCode()];
        }

        private List<VLayoutGroupInfo> groups = new List<VLayoutGroupInfo>();

        public VLayoutGroupInfo[] GetAllGroups()
        {
            return groups.ToArray();
        }

        public SortedList<int, VLayoutNodeInfo> nodesByTag = new SortedList<int, VLayoutNodeInfo>();
        public SortedList<int, VLayoutNodeInfo> nodesById = new SortedList<int, VLayoutNodeInfo>();
        private int idCounter = 0;


        private void AddNodeTag(VLayoutNodeInfo node, object tag)
        {
            if (tag != null)
            {
                nodesByTag.Add(tag.GetHashCode(), node);
            }
        }

        public void AddNodeId(VLayoutNodeInfo node)
        {
            node.id = idCounter;
            idCounter++;
            nodesById.Add(node.id, node);
        }



        public VLayoutNodeInfo GetNodeByTag(object Tag)
        {
            return nodesByTag[Tag.GetHashCode()];
        }

        public VLayoutNodeInfo GetNodeById(object id)
        {
            return nodesById[Convert.ToInt32(id)];
        }

        public VLayoutGroupInfo CreateGroup(VLayoutContainerInfo parent, object Tag)
        {
            if (parent == null)
            {
                parent = GetMainGroup();
            }



            var gr = new VLayoutGroupInfo(this, parent);


            groups.Add(gr);
            AddNodeTag(gr, Tag);
            AddNodeId(gr);

            hasChanges = true;
            return gr;
        }

        public VLayoutTabsInfo CreateTabContainer(VLayoutGroupInfo parent, object Tag)
        {
            if (parent == null)
            {
                parent = GetMainGroup();
            }
            var obj = new VLayoutTabsInfo(this, parent);
            AddNodeTag(obj, Tag);
            AddNodeId(obj);
            hasChanges = true;
            return obj;
        }
        public VLayoutSplitContainerInfo CreateSplitContainer(VLayoutGroupInfo parent, object Tag)
        {
            if (parent == null)
            {
                parent = GetMainGroup();
            }
            var obj = new VLayoutSplitContainerInfo(this, parent);
            AddNodeTag(obj, Tag);
            AddNodeId(obj);
            hasChanges = true;
            return obj;
        }

        public VLayoutControlContainerInfo CreateItem(VLayoutGroupInfo parent, object control, object Tag)
        {
            if (parent == null)
            {
                parent = GetMainGroup();
            }

            var obj = new VLayoutControlContainerInfo(parent, control);
            AddNodeTag(obj, Tag);
            AddNodeId(obj);
            hasChanges = true;
            return obj;

        }

        public VLayoutLabelInfo CreateLabel(VLayoutGroupInfo parent, object Tag)
        {
            if (parent == null)
            {
                parent = GetMainGroup();
            }

            var obj = new VLayoutLabelInfo();

            parent.AddChild(obj);
            AddNodeTag(obj, Tag);
            AddNodeId(obj);
            hasChanges = true;
            return obj;

        }
        private VLayoutGroupInfo mainGroup;

        //public IVControlsFactory GetControlsFactory()
        //{
        //    return UIStatic.GetControlsfactory();
        //    //return new sql.builder.UI.WinForms.VControlsFactoryWinForms();
        //}

        public VLayoutGroupInfo GetMainGroup()
        {
            return mainGroup;
        }
        public bool hasChanges = true;
        public bool Suspended = false;
        public void RefreshLayoutIfNeed()
        {
            if (Suspended) return;
            if (hasChanges)
            {
                RefreshLayout();
            }

            hasChanges = false;
        }

        //public void RefreshLayoutWithDelay()
        //{
        //    timer.Stop();
        //    timer.Start();
        //}


        public void RefreshLayout()
        {
        }
        public void HideLayout()
        {
            //GetMainGroup().GetTypedControl().BeginLayoutChange();
            //GetMainGroup().Hide();
            //GetMainGroup().GetTypedControl().EndLayoutChange();
        }

        public void Dispose()
        {
            //GetMainGroup().GetControl().Dispose();
        }
    }

}
