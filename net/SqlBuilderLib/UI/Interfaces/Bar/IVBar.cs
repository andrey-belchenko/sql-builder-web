using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public interface IVBar:IVControl
    {
        void AddBarButton(IVBarItem button,bool beginGroup);
        void SetVisible(bool val);
        void HideAllItems();
    }
}
