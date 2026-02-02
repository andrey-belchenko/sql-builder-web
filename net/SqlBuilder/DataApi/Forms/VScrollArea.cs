using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VScrollArea : VFieldGroup
    {
        public VScrollArea()
            : base(EName.scrollarea)
        {
        }
    }
}