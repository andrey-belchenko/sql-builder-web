using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace sql.builder.UI
{
    public interface IVPopupMenu : IVControl
    {

        void AddButton(IVBarItem button, bool beginGroup);
        bool IsEmpty();
        void Show();
        void Clear();
    }
}
