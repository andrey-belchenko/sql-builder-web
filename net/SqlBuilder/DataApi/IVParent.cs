using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public interface IVParent
    {
        IList<string> AllowedChildNodes();
    }
}
