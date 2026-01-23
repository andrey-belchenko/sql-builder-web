using System;
using System.Xml.Linq;
using System.Linq;

namespace sql.builder.DataApi
{
    internal sealed class VReportProc : VSXElement
    {
        internal VReportProc()
            : base(EName.procedure)
        {
        }
        #region Text
        /// <summary>
        /// Текст хранимой процедуры
        /// </summary>
        public override string P_Text {
            get {
                XElement node = this.Elements(EName.text).FirstOrDefault(EPredicate.IsNotExcuded);
                if (node != null) {
                    return node.Value;
                } else {
                    return this.Value;
                }
            }
            set {
                XElement node = this.Elements(EName.text).FirstOrDefault(EPredicate.IsNotExcuded);
                if (node != null) {
                    node.Value = value;
                } else {
                    this.RemoveNodes();
                    this.Add(new XCData(value));
                }
            }
        }
        public override bool P_Text_Exists()
        {
            return true;
        }
        public override string P_Text_FieldGroup()
        {
            return TextConst.SchEdirorFieldGr.MainMain;
        }
        #endregion
    }
}