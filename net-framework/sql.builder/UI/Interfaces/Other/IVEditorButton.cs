using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace sql.builder.UI
{
    public interface IVEditorButton : IVControl, IVTagControl, IVEnabledControl
    {

        void SetCaption(string value);
        void SetToolTip(string value);
        void SetImage(Image value);
        void SetIsLeft(bool value);
        void SetKind(string value);
        IVEditorButton Copy();
        //void SetVisible(bool value);
        //void SetEnabled(bool value);
       // object Tag {get; set;} // временно
        //object Menu { get; set; } // временно
        //int GetTextWith();
        event ValueChangeEventHandler ButtonClick;
    }
}
