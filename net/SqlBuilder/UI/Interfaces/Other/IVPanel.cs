namespace sql.builder.UI
{
    public interface IVPanel : IVControl, IVNormalControl
    {
        void AppendChild(IVControl control);
        void BeginInit();
        void EndInit();
    }
}
