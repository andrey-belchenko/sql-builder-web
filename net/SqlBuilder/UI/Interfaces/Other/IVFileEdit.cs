namespace sql.builder.UI
{
    public interface IVFileEdit : IVControl, IVNormalControl
    {

        void BeginInit();
        void EndInit();
        void SetValue(object value);
        //event ValueChangeEventHandler ValueChanged;
        event ValueChangeEventHandler FileSelected;
        event SimpleEventHandler FileOpenPressed;
        event SimpleEventHandler Cleared;
        //event SimpleEventHandler Entered;
    }
}
