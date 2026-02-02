using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VDLink : VELink
    {
        internal VDLink()
            : base(EName.dlink)
        {
        }
    }
}