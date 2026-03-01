namespace sql.builder.UI
{
    public interface IVLayoutTabs : IVLayoutNode
    {
        void Init(VLayoutTabsInfo tabs);
        // void ShowTab(VLayoutTabsInfo tabs, VLayoutGroupInfo tab);
        VLayoutGroupInfo GetSelectedTab();
        void HideTab(VLayoutGroupInfo tab);
        void ShowTab(VLayoutGroupInfo tab);
        int BorderHeight();
        int BorderWidth();
        void SelectFirst();
        //int WidthDisplacement();
        //int HeightDisplacement();
    }
}
