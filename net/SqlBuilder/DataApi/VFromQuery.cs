using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VFromQuery : VQueryCall, IVParent
    {
        internal VFromQuery()
            : base(EName.query)
        {
        }
        IList<string> IVParent.AllowedChildNodes()
        {
            List<string> child_nodes = new List<string>(10);
            child_nodes.Add(TextConst.EName.WithParams);
            child_nodes.Add(TextConst.EName.Link);
            child_nodes.Add(TextConst.EName.DLink);
            child_nodes.Add(TextConst.EName.ELink);
            child_nodes.Add(TextConst.EName.Where);
            VSXElement parent = this.GetParent();
            VSourcedElement root_query = this.RootQuery();
            if ((this.IsCall() && parent is VFrom) || (parent is VQueryCall && root_query is VReport)) {
                child_nodes.Add(TextConst.EName.Call);
                child_nodes.Add(TextConst.EName.Using);
            }
            child_nodes.Add(TextConst.EName.ExtendWhere);
            if (root_query is VReport) {
                child_nodes.Add(TextConst.EName.Events);
                child_nodes.Add(TextConst.EName.Query);
            }
            child_nodes.Add(TextConst.EName.DimLink);
            if (root_query is VForm) {
                child_nodes.Add(TextConst.EName.Qube);
            }
            return child_nodes;
        }
    }
}
