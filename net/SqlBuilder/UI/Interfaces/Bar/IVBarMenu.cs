using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
namespace sql.builder.UI
{
    public interface IVBarMenu : IVControl, IVBarItem
    {

        void AddButton(IVBarItem button, bool beginGroup);
    }
}
