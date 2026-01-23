using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public interface IVDateEdit : IVControl, IVNormalControl, IVMaskControl
    {

        void BeginInit();
        void EndInit();
        void SetValue(object value);
        void SetShowTime(bool value);
        void SetCanClear(bool value);
        event ValueChangeEventHandler ValueChanged;
        event SimpleEventHandler FocusLost;
        //event SimpleEventHandler Entered;
    }
}
