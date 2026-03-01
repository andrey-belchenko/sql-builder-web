//using System.Windows.Forms;
// Cross-platform: Drawing2D and Imaging are Windows-only, commented out
//using System.Drawing.Drawing2D;
//using System.Drawing.Imaging;
//using sql.builder.Properties;
//using sql.builder.Controls.Grids;
namespace sql.builder.UI
{
    // В этом файле временная реализация , в процессе перехода на класс не привязанный к winforms
    public partial class UIFormC : IForm
    {
        private void TmpBarManagetInitialize()
        {
            //var ctrl = TmpGetControlAsWinFormCtrl();
            //if (ctrl != null)
            //{
            //    ctrl.TmpGetBarManager().ForceLinkCreate();
            //    ctrl.TmpGetBarManager().ForceInitialize();
            //}
        }
        //public BarManager TmpGetBarManager()
        //{
        //    var ctrl = TmpGetControlAsWinFormCtrl();
        //    if (ctrl == null)
        //    {
        //        return null;
        //    }
        //    return ctrl.TmpGetBarManager();
        //}
        public IVBar GetToolBar()
        {
            //var ctrl = TmpGetControlAsWinFormCtrl();
            //if (ctrl == null)
            //{
            //    return null;
            //}
            //return GetControl().GetToolBar();
            return null;
        }
        //public sql.builder.UI.WinForms.UIFormControl TmpGetControlAsWinFormCtrl()
        //{
        //    return GetControl() as sql.builder.UI.WinForms.UIFormControl;
        //}
        //public BarButtonItem TmpGetBarButton(string btn)
        //{
        //    return TmpGetControlAsWinFormCtrl().TmpGetBarButton(btn);
        //}
    }
}
