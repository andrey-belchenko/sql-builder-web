using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VDLink : VELink
    {
        public VDLink()
            : base(EName.dlink)
        {
        }
    }
}