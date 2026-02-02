using System.Collections.Generic;
using System.Linq;

namespace sql.builder.DataApi
{
    internal partial class VQueryCall
    {
        internal IList<VUseAction> RowActions()
        {
            return this.GetElementsP(EName.rowactions).SelectMany(VSXElement.GetElementsP).Cast<VUseAction>().ToList();
        }
    }
}
