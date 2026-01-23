////using System.Windows.Forms;
////using DevExpress.XtraEditors;
////using DevExpress.XtraBars.Alerter;
//using System;
//
//namespace sql.builder.UI.WinForms
//{
//    class VDialogManager : IVDialogManager
//    {
//    
//        public void ShowQuestion(string title, string text, Action yesAction = null, Action noAction = null, Action cancelAction = null)
//        {
//            var res = XtraMessageBox.Show(text, title, MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question);
//            Action act = null;
//            switch (res)
//            {
//                case DialogResult.Yes:
//                    act = yesAction;
//                    break;
//                case DialogResult.No:
//                    act = noAction;
//                    break;
//                case DialogResult.Cancel:
//                    act = cancelAction;
//                    break;
//            }
//            if (act != null)
//            {
//                act();
//            }
//        }
//    }
//}
