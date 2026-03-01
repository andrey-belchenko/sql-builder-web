// Cross-platform: Image is Windows-only, using object instead
namespace sql.builder.UI
{
    public interface IVBarItem : IVControl, IVVisibleControl, IVEnabledControl
    {

        void SetCaption(string value);
        void SetImage(object value);
        //void SetVisible(bool value);
        //void SetEnabled(bool value);
        void SetIsRight(bool value);

    }
}
