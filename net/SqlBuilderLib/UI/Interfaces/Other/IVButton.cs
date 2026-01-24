using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
// Cross-platform: Image is Windows-only, using object instead
namespace sql.builder.UI
{
    public interface IVButton : IVControl, IVEnabledControl,IVVisibleControl, IVNormalControl,IVTagControl
    {

        void SetCaption(string value);
        void SetToolTip(string value);
        void SetImage(object value);
        //void SetVisible(bool value);
        //void SetEnabled(bool value);
        
        object Menu { get; set; } // временно
        int GetTextWith();

        event ValueChangeEventHandler ButtonClick;
    }
}
