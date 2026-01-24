using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public interface IVCheckContainer : IVControl, IVEnabledControl, IVNormalControl
    {
        void AddChild(IVControl control);
        void BeginInit();
        void EndInit();
        void SetChecked(bool value);
        void SetCheckEnabled(bool value);
        void SetCheckVisible(bool value);
       // void SetEnabled(bool value);
        void SetName(string value);
        void SetError(string value);
        void AddButton(IVEditorButton button);
        event ValueChangeEventHandler CheckedChanged;

    }
}
