using System.Collections.Generic;
using System.Linq;

namespace sql.builder.DataApi
{
    internal partial class VReport
    {
        public List<VUseAction> RowActions()
        {
            return  Queries().SelectMany(e => e.RowActions()).ToList();

        }
    }
}
