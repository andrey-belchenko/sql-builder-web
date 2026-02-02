using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public interface IVNumberEdit : IVControl, IVNormalControl, IVMaskControl
    {

        void BeginInit();
        void EndInit();
        void SetValue(object value);
        void SetStep(decimal value);
        event ValueChangeEventHandler ValueChanged;
        //event SimpleEventHandler Entered;
        event SimpleEventHandler Cleared;
        event SimpleEventHandler FocusLost;
    }
}
