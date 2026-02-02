//using DevExpress.XtraEditors;
using sql.builder.UI;

namespace sql.builder.Controls.Containers
{
    internal partial class ucBaseContainer
    {
        protected UIFormC _paramsC;
        internal UIFormC ParamFormC
        {
            get { return _paramsC; }
        }
        public string ContainerTitle { get; set; }
        public string ContainerName { get; set; }
        protected UIFormC _parentParamsC;
        internal void SetParentParamsForm(UIFormC form)
        {
            _parentParamsC = form;
        }

        internal UIFormC GetParentParamsForm()
        {
            return _parentParamsC;
        }
        public ucBaseContainer()
        {
            //InitializeComponent();
        }
    }
}
