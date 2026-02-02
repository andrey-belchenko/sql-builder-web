using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sql.builder.UI
{
    public interface IVPanel : IVControl, IVNormalControl
    {
        void AppendChild(IVControl control);
        void BeginInit();
        void EndInit();
    }
}
