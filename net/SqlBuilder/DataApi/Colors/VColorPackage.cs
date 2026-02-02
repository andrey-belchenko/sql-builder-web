using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;color-package name="" &gt;
    /// </summary>
    /// <seealso cref="VColor"/>
    public sealed class VColorPackage : VSXElement, IVParent
    {
        public VColorPackage()
            : base(EName.color_package)
        {
        }
        /*public VColor GetColor(string name)
        {
            IList<VSXElement> colors = this.GetElementsP();
            for (int index = 0; index < colors.Count; index++) {
                VSXElement color = colors[index];
                if (color.P_Name == name) {
                    return (VColor)color;
                }
            }
            return null;
        }*/
        private static string[] child_nodes = { TextConst.AName.Color };
        IList<string> IVParent.AllowedChildNodes()
        {
            return child_nodes;
        }
        public override string GetNodeInfo()
        {
            return this.P_NodeName + " " + Bold(this.P_IdName);
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