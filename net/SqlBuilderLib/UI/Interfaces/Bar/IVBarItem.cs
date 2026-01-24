using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace sql.builder.UI
{
    public interface IVBarItem:IVControl,IVVisibleControl,IVEnabledControl
    {

        void SetCaption(string value);
        void SetImage(Image value);
        //void SetVisible(bool value);
        //void SetEnabled(bool value);
        void SetIsRight(bool value);
       
    }
}
