using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;format-package name="" &gt;
    /// </summary>
    public sealed class VFormatPackage : VSXElement, IVParent
    {
        public VFormatPackage()
            : base(EName.format_package)
        {
        }
        public VFormat GetFormat(string name)
        {
            IList<VSXElement> formats = this.GetElementsP();
            for (int index = 0; index < formats.Count; index++) {
                VSXElement format = formats[index];
                if (format.P_Name == name) {
                    return (VFormat)format;
                }
            }
            return null;
        }
        private static string[] child_nodes = { TextConst.AName.Format };
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