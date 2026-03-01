using System.Collections.Generic;

namespace sql.builder.DataApi
{
    public sealed class VFolder : VSXElement, IVParent
    {
        public VFolder()
            : base(EName.folder)
        {
        }
        private static string[] child_nodes = { TextConst.AName.Folder, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            if (this.GetMainParent() is VNavigator)
            {
                return VNavigator.child_nodes;
            }
            else
            {
                return child_nodes;
            }
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.P_NodeName + " " + Bold(this.P_Name) + " " + Italic(this.P_SelfTitle);
        }
        #endregion
        #region SecurityId
        public override bool P_SecurityId_Exists()
        {
            return true;
        }
        #endregion
        #region Name
        public override bool P_Name_Exists()
        {
            return true;
        }
        #endregion
    }
}