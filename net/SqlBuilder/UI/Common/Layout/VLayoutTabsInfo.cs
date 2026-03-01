namespace sql.builder.UI
{
    public class VLayoutTabsInfo : VLayoutContainerInfo
    {
        public VLayoutTabsInfo(VLayout controller, VLayoutGroupInfo parentGroup)
            : base(controller, parentGroup)
        {
            IsFiller = true;

        }
        public override void ResetTabs()
        {

            //if (control != null)
            //{
            //    GetTypedControl().SelectFirst();
            //}
            //base.ResetTabs();

        }

        //public IVLayoutTabs GetTypedControl()
        //{
        //    return GetControl() as IVLayoutTabs;
        //}

        public override void Show()
        {
        }
        public override void AddChild(VLayoutNodeInfo node)
        {
            base.AddChild(node);
            node.IsFiller = true;
            (node as VLayoutGroupInfo).HasBorder = false;
        }
        public bool IsTabSelected(VLayoutGroupInfo tab)
        {
            return false;
            //if (UIStatic.IsWeb())
            //{
            //    return true;
            //}
            //return GetTypedControl().GetSelectedTab() == tab;
        }
        public override void InitControl()
        {
            //GetTypedControl().Init(this);
        }
        public override void ResetAndCalculateChilds()
        {

            foreach (VLayoutGroupInfo node in Nodes)
            {
                if (node.IsExpanded())
                {
                    //node.height = GetClientHeight()-GetTypedControl().BorderHeight();


                    //node.width = GetClientWidth()-GetTypedControl().BorderWidth();

                    //не учтен magrin групп находящихся в nodes
                    node.ResetAndCalculateChilds();
                }
            }

        }

        public void SelectedTabChanged()
        {
            //var gr = GetTypedControl().GetSelectedTab();
            //if (gr != null)
            //{
            //    gr.RaiseTabSelected();
            //}
            //GetLayoutController().hasChanges = true;
            //GetLayoutController().RefreshLayoutIfNeed();
        }

        public override int GetMarginLeft()
        {
            return 1;
        }
        public override int GetMarginRight()
        {
            return 1;
        }
        public override int GetMarginBottom()
        {
            return 1;
        }
        public override int GetMarginTop()
        {
            return 1;
        }
    }


}
