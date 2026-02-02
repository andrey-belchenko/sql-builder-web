using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;table name="" as="" /&gt;
    /// </summary>
    public sealed class VTable : VQueryCall, IVParent
    {
        public VTable()
            : base(EName.table)
        {
        }
        public override VQuery Query()
        {
            return (VQuery)this.RootQuery();
        }
        private static string[] child_nodes = { TextConst.EName.Link, TextConst.EName.DLink, TextConst.EName.ELink, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region CalledQuery
        public override bool P_CalledQuery_Exists()
        {
            return false;
        }
        #endregion
        #region Name
        public override bool P_Name_Exists()
        {
            return true;
        }
        #endregion
        #region TableCode
        public override bool P_TableCode_Exists()
        {
            return true;
        }
        public bool P_TableCode_Editable()
        {
            return true;
        }
        #endregion
        #region ClientCalulation
        public override bool P_ClientCalulation_Exists()
        {
            return true;
        }
        #endregion
    }
}