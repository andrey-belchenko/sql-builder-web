using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VReportQueries : VFrom, IVParent
    {
        public VReportQueries()
            : base(EName.queries)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Query, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
    }
}