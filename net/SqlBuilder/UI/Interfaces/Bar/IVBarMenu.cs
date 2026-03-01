namespace sql.builder.UI
{
    public interface IVBarMenu : IVControl, IVBarItem
    {

        void AddButton(IVBarItem button, bool beginGroup);
    }
}
