using System;
namespace sql.builder.UI
{
    public interface IVForm : IVControl, IVNormalControl
    {

        event EventHandler Disposed;
        void AddChild(IVControl control);
        void ClearChilds();
        void BeginInitialize();
        void EndInitialize();
        void SetTitle(string title);
        IVBar GetToolBar();
        void ShowDialog(object owner);
        void ShowForm();
        void SetController(IForm controller);

    }
}
