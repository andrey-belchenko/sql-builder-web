using System;
using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;

namespace sql.builder.DataApi
{
    /// <summary>
    /// &lt;const&gt;'ABC'&lt;/const&gt;
    /// </summary>
    /// <seealso cref="VArray"/>
    /// <seealso cref="VScalarConst"/>
    internal abstract class VConst : VSXElement
    {
        protected VConst(XName name)
            : base(name)
        {
        }
        private VColumn TypedColumn()
        {
            //if (this.GetParent() is VDefault) {
            //    return (VColumn)this.GetParent().GetParent();
            //}
            XElement col = this.Parent.Element(EName.column);
            if (col != null) {
                return VSXElement.Get<VColumn>(col);
            } else {
                return null;
            }
        }
        protected VDataTable SelectionListTable()
        {
            VColumn col = this.TypedColumn();
            if (col != null) {
                VDataSet ds = col.SelectionListDataSet();
                if (ds != null) {
                    return (ds.Tables[0] as VDataTable);
                }
            }
            return null;
        }
        public override object GetRuntimeValue(VDataSet dataSe, DataRow rowt, VDataColumn col)
        {
            string v = this.Value;
            return ValueToObject(v);
        }
        internal static object ValueToObject(string value)
        {
            string v = value;
            if (v == "null") {
                return null;
            }
            if (v.StartsWith("'")) {
                return v.Substring(1, v.Length - 2);
            }
            if (Cmn.IsNumeric(v)) {
                return Cmn.ToDecimal(v);
            } else {
                return v;
            }
        }
        #region DataType
        public override bool P_DataType_Exists()
        {
            return true;
        }
        #endregion
        #region NodeText
        public override string GetNodeInfo()
        {
            return this.GetNodeOtherInfo();
        }
        public override string GetNodeOtherInfo()
        {
            string s = this.P_ConstValue;
            if (this.P_Alias_Exists()) {
                s += " as " + this.P_Alias;
            }
            string fact = this.P_Fact;
            if (!string.IsNullOrEmpty(fact)) {
                s += " fact:" + Bold(fact);
            }
            string dimension = this.P_Dimension;
            if (!string.IsNullOrEmpty(dimension)) {
                s += " dim " + Bold(dimension);
            }
            return s;
        }
        #endregion
        #region Свойства
        #region ConstValue
        public override string P_ConstValue {
            get {
                return this.Value;
            }
            set {
                this.Value = value ?? string.Empty;
            }
        }
        public override bool P_ConstValue_Exists()
        {
            return true;
        }
        #endregion
        #region Alias
        public override bool P_Alias_Exists()
        {
            return this.GetParent() is VOutputElement;
        }
        #endregion
        #endregion
    }
}