using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;from&gt;...&lt;/from&gt;
    /// </summary>
    /// <seealso cref="VReportQueries"/>
    public class VFrom : VSXElement, IVParent
    {
        protected VFrom(XName name)
            : base(name)
        {
        }
        public VFrom()
            : base(EName.from)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Query, TextConst.EName.Table, TextConst.EName.Qube, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region StarScheme
        public override bool P_StarScheme_Exists()
        {
            return this.RootQuery() is VForm;
        }
        #endregion
        #region SingleWay
        public override bool P_SingleWay_Exists()
        {
            return true;
        }
        #endregion
    }
}