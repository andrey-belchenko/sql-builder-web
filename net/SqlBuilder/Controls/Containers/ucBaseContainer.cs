//using DevExpress.XtraEditors;
using sql.builder.UI;

namespace sql.builder.Controls.Containers
{
    public partial class ucBaseContainer
    {
        protected UIFormC _paramsC;
        public UIFormC ParamFormC
        {
            get { return _paramsC; }
        }
        public string ContainerTitle { get; set; }
        public string ContainerName { get; set; }
        protected UIFormC _parentParamsC;
        public void SetParentParamsForm(UIFormC form)
        {
            _parentParamsC = form;
        }

        public UIFormC GetParentParamsForm()
        {
            return _parentParamsC;
        }
        public ucBaseContainer()
        {
            //InitializeComponent();
        }
    }
}
