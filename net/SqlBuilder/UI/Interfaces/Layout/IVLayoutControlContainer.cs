namespace sql.builder.UI
{
    public interface IVLayoutControlContainer : IVLayoutNode
    {
        void Show(VLayoutControlContainerInfo item);
        int GetHeight(VLayoutControlContainerInfo item);
        void SetLabel(IVLayoutLabel label);
        void SetHint(string value);
    }
}
