using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;dimension-package name="" &gt;
    /// </summary>
    internal sealed class VDimensionPackage : VOutputElement, IVParent
    {
        internal VDimensionPackage()
            : base(EName.dimension_package)
        {
        }
        /*internal VDimension GetDimension(string name)
        {
            IList<VSXElement> dimensions = this.GetElementsP();
            for (int index = 0; index < dimensions.Count; index++) {
                VSXElement dimension = dimensions[index];
                if (dimension.XName == name) {
                    return (VDimension)dimension;
                }
            }
            return null;
        }*/
        /*internal VDimension GetDimensionByQueryName(string name)
        {
            IList<VSXElement> dimensions = this.GetElementsP();
            for (int index = 0; index < dimensions.Count; index++) {
                VSXElement dimension = dimensions[index];
                if (dimension.P_CalledQuery == name) {
                    return (VDimension)dimension;
                }
            }
            return null;
        }*/
        internal IList<VDimension> GetDimensions()
        {
            IList<VSXElement> dimensions = this.GetElementsP();
            VDimension[] arr = new VDimension[dimensions.Count];
            for (int index = 0; index < dimensions.Count; index++) {
                arr[index] = (VDimension)dimensions[index];
            }
            return arr;
        }
        private static string[] child_nodes = { TextConst.EName.Dimension, TextConst.EName.UsePart };
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
        public new bool P_IdName_Exists()
        {
            return true;
        }
        #endregion
    }
}