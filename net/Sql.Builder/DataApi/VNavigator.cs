using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;navigator name="" &gt;
    /// </summary>
    internal sealed class VNavigator : VSXElement, IVParent
    {
        internal VNavigator()
            : base(EName.navigator)
        {
        }
        internal static string[] child_nodes = { TextConst.EName.Folder, TextConst.EName.UseReport, TextConst.EName.UseForm, TextConst.EName.UsePart };
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
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.P_NodeName + ": " + Bold(this.P_Name);
        }
        #endregion
    }
}