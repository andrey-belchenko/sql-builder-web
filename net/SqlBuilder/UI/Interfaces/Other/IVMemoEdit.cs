namespace sql.builder.UI
{
    public interface IVMemoEdit : IVControl, IVNormalControl
    {

        void BeginInit();
        void EndInit();
        void SetValue(object value);
        event ValueChangeEventHandler ValueChanged;
        //event SimpleEventHandler Entered;
    }
}
