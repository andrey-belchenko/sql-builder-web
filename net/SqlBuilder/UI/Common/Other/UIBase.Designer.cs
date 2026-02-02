using System;
//using System.Windows.Forms;
//using DevExpress.XtraEditors;
using System.Xml.Linq;
using System.Xml;
namespace sql.builder.UI
{
    public partial class UIBase
    {
        protected object GetValueUnchecked()
        {
            return Cmn.DECIMAL_ZERO;
        }

        protected object GetValueChecked()
        {
            return Cmn.DECIMAL_ONE;
        }
        public void SetChecked(bool value)
        {
            this.used = value;
            //this.checkContainer.SetChecked(used);
            //ceUsed1.Checked = value;
        }
        public void SetUnChecked()
        {
            //ceUsed1.Checked = false;
            this.used = false;
        }
        protected enum PartDest
        {
            Root,
            Editors,
            Settings
        }
    }
}
