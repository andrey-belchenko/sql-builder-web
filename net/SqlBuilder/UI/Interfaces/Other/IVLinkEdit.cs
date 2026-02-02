using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public interface IVLinkEdit : IVControl, IVNormalControl
    {

        void BeginInit();
        void EndInit();
        void SetValue(object value);
        event ValueChangeEventHandler ValueChanged;
        //event SimpleEventHandler Entered;
    }
}
