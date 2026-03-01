namespace sql.builder.UI
{
    public interface IVTextEdit : IVControl, IVNormalControl, IVMaskControl
    {

        void BeginInit();
        void EndInit();
        void SetValue(object value);
        event ValueChangeEventHandler ValueChanged;

        void SetUsePlaneString();
        //event SimpleEventHandler Entered;
    }
}
