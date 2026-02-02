using System;
using System.Data;
using System.Linq;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.XtraEditors.Controls;

namespace sql.builder.UI
{
    public partial class UIComboRange : UIBase, IRange
    {
        public UIComboRange()
        {
            //InitializeComponent();
        }

        public override void Initialize(XElement xfield, UIFormC form)
        {
            throw new NotImplementedException();
            BaseInitialize(xfield, form, ReturnType.SimpleRange, null, true);
            InitControl();
        }

        protected override void InitControl()
        {
        }

        public override void SetControlValue(object value, int index = 1)
        {
            //switch (index)
            //{
            //    case 1:
            //        luControlFrom.EditValue = value;
            //        break;
            //    case 2:
            //        luControlTo.EditValue = value;
            //        break;
            //}
        }
        public override void RefreshData()
        {
            base.RefreshData();
        }
        public override void SetError(string text, int index = 1)
        {
            SetErr(text);
        }
        #region IRange
        void IRange.GetText(out string value_1, out string value_2)
        {
            throw new NotImplementedException();
            //if (this.UseType == UIFormC.UseType.DataEditor) {
            //    value_1 = this.GetBoundColumn(1).GetFieldValueName();
            //    value_2 = this.GetBoundColumn(2).GetFieldValueName();
            //} else {
            //    value_1 = this.luControlFrom.Text;
            //    value_2 = this.luControlTo.Text;
            //}
        }
        #endregion
    }
}