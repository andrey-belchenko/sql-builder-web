using System;
using System.Globalization;
using System.Linq;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Mask;
//using DevExpress.XtraEditors.Repository;
using sql.builder.DataApi;

namespace sql.builder.UI
{
    internal partial class UITextArray : UIText
    {

        public override bool IsStringToArray()
        {
            return true;
        }
    }
}
