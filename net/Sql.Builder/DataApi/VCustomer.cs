using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    internal sealed class VCustomer : VSXElement
    {
        internal VCustomer()
            : base(EName.customer)
        {
            this.KeyField = AName_.id;
        }
        #region SelfTitle
        public override string P_SelfTitle_Title()
        {
            return "Наименование";
        }
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region IdName
        public override string P_IdName_Title()
        {
            return "Код";
        }
        public override bool P_IdName_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            return this.P_IdName + " " + Italic(this.P_SelfTitle);
        }
        #endregion
    }
}