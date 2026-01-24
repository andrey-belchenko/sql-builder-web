using System;
using System.Collections.Generic;
using System.Xml.Linq;
using AName_ = sql.builder.DataApi.AName;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;expression-package name="" /&gt;
    /// </summary>
    internal sealed class VExpressionPackage : VOutputElement, IVParent
    {
        internal VExpressionPackage()
            : base(EName.expression_package)
        {
        }
        /*internal VExpression GetExpression(string name)
        {
            IList<VSXElement> list = this.GetElementsP();
            for (int index = 0; index < list.Count; index++) {
                VSXElement e = list[index];
                if (e.XName == name) {
                    return e as VExpression;
                }
            }
            return null;
        }*/
        private static string[] child_nodes = { TextConst.EName.Call, TextConst.EName.UsePart };
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