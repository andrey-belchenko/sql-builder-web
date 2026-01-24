using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VGrset : VSXElement, IVParent
    {
        internal VGrset()
            : base(EName.grset)
        {
        }
        private VGroup GroupElement()
        {
            return this.GetElementsP(EName.group).FirstOrDefault() as VGroup;
        }
        private List<VSourceLink> GroupLinks()
        {
            List<VSourceLink> list;
            VGroup grEl = this.GroupElement();
            if (grEl == null) {
                list = new List<VSourceLink>();
            } else {
                list = grEl.GroupLinks();
            }
            return list;
        }
        private static string[] child_nodes = { TextConst.EName.Group, TextConst.EName.Grset, TextConst.EName.Where, TextConst.EName.Having, TextConst.EName.UsePart };
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
        #region Alias

        public override bool P_Alias_Exists()
        {

            return true;

        }

        #endregion
        #region NodeText
        public override string GetNodeOtherInfo()
        {
            string s = string.Empty;
            IList<VSourceLink> grl = this.GroupLinks();
            if (grl.Count != 0) {
                s += "(";
                string q = string.Empty;
                foreach (VSourceLink l in grl) {
                    s+= q + l.P_Table;
                    q = ",";
                }
                s += ")";
            }
            s += " " + Bold(this.P_Alias) + " " + Italic(this.P_Title);
            return s;
        }
        #endregion
        #region Order
        public override bool P_Order_Exists()
        {
            return true;
        }
        #endregion
    }
}