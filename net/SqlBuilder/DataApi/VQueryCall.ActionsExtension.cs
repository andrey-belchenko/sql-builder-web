using System.Collections.Generic;
using System.Linq;

namespace sql.builder.DataApi
{
    public partial class VQueryCall
    {
        public IList<VUseAction> RowActions()
        {
            return this.GetElementsP(EName.rowactions).SelectMany(VSXElement.GetElementsP).Cast<VUseAction>().ToList();
        }
    }
}
