using System;
using System.Collections.Generic;

namespace sql.builder.DataApi
{
    internal interface IVParent
    {
        IList<string> AllowedChildNodes();
    }
}
