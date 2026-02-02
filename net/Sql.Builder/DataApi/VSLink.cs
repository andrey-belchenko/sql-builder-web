using System;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VSLink : VQueryCall
    {
        internal VSLink()
            : base(EName.slink)
        {
        }
    }
}