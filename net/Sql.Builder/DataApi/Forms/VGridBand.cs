using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VGridBand : VOutputElement, IVParent
    {
        internal VGridBand()
            : base(EName.band)
        {
        }
        internal string GetPath()
        {
            VGridBand pBand = this.GetParent() as VGridBand;
            string s;
            if (pBand != null) {
                s = pBand.GetPath() + "|";
            } else {
                s = string.Empty;
            }
            s += this.P_Title;
            return s;
        }
        internal static string[] child_nodes = { TextConst.EName.Band, TextConst.EName.Column, TextConst.EName.Fact, TextConst.EName.Call, TextConst.EName.UsePart };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        #region SelfTitle
        public override bool P_SelfTitle_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            return Italic(this.P_SelfTitle);
        }
        #endregion
    }
}