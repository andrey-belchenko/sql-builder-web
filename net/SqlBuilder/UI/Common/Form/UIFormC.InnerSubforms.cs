using System;
//using System.Windows.Forms;
using System.Collections.Generic;
//using DevExpress.XtraLayout;
//using DevExpress.XtraLayout.Utils;
//using infoenergo.core.Extensions;
//using sql.builder.Test;

namespace sql.builder.UI
{
    public partial class UIFormC : IForm
    {
        //public Dictionary<string, LayoutControl> innerSubForms = new Dictionary<string, LayoutControl>();
        public Dictionary<string, InnerSubFormInfo> innerSubFormsNew = new Dictionary<string, InnerSubFormInfo>();
        //frmDynamicEditor activeSubForm = null;
        public void ShowInnerSubformNew(string name)
        {
            throw new NotImplementedException();
        }
        public void HideInnerSubform()
        {
            //if (activeSubForm != null)
            //{
            //    activeSubForm.Hide();
            //}
        }
        public class InnerSubFormInfo
        {
            public VLayout Layout { get; set; }
            public IDisposable Parent { get; set; }
        }
    }
}
