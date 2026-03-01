namespace sql.builder.UI
{
    public interface IVBar : IVControl
    {
        void AddBarButton(IVBarItem button, bool beginGroup);
        void SetVisible(bool val);
        void HideAllItems();
    }
}
