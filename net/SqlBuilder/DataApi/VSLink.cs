using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VSLink : VQueryCall
    {
        public VSLink()
            : base(EName.slink)
        {
        }
    }
}