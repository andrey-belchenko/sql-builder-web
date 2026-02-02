using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    public sealed class VSplitter : VSXElement
    {
        public VSplitter()
            : base(EName.splitter)
        {
        }
        #region IsVertical
        public override bool P_IsVertical_Exists()
        {
            return true;
        }
        #endregion
    }
}