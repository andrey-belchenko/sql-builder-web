using System.Collections.Generic;
using sql.builder.UI;
namespace sql.builder.Controls.Containers
{
    internal partial class ucFormContainer : ucBaseContainer
    {
        internal ucFormContainer()
        {
            //InitializeComponent();
        }

        internal void Initialize(Dictionary<string, string> report_info)
        {
            _paramsC = UIStatic.CreateForm(report_info["repname"], null, false, false, true, null, null, report_info["project"]);

            ContainerTitle = report_info["title"];
            ContainerName = report_info["repname"];
            //this.Controls.Add(_paramsC.TmpGetControlAsWinFormCtrl() as Control);
        }
    }
}
