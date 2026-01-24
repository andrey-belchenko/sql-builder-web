using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VScrollArea : VFieldGroup
    {
        internal VScrollArea()
            : base(EName.scrollarea)
        {
        }
    }
}