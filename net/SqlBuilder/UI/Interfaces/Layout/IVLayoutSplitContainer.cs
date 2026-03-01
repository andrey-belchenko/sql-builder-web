namespace sql.builder.UI
{
    public interface IVLayoutSplitContainer : IVLayoutNode
    {
        void Init(VLayoutSplitContainerInfo splitContainer);
        void ShowItem(VLayoutSplitContainerInfo splitContainer, VLayoutGroupInfo item);

        int GetSplitterWidth();
        int[] GetSizes();

    }
}
