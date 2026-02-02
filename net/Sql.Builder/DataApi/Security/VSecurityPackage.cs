using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;security-package name="" &gt;
    /// </summary>
    internal sealed class VSecurityPackage : VOutputElement, IVParent
    {
        internal VSecurityPackage()
            : base(EName.security_package)
        {
        }
        private static string[] child_nodes = { TextConst.EName.Role };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region IdName
        public override string P_IdName {
            get {
                return this.AttrOrEmpty(AName_.name);
            }
            set {
                this.SetIdName(AName_.name, value);
            }
        }
        public override bool P_IdName_Exists()
        {
            return true;
        }
        #endregion
    }
}