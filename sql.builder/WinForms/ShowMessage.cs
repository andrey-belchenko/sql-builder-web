////using System.Windows.Forms;
////using DevExpress.XtraEditors;
////using DevExpress.XtraBars.Alerter;
//using System;
//namespace sql.builder.WinForms
//{

    
//    internal static class ShowMessage
//    {
//        internal static DialogResult Show(MType mtype, string text = null)
//        {
//            switch (mtype)
//            {
//                    case MType.UnsavedChangesQuestion:
//                        return XtraMessageBox.Show("Сохранить изменения?", 
//                                                   "Имеются несохраненные изменения", 
//                                                   MessageBoxButtons.YesNoCancel, 
//                                                   MessageBoxIcon.Question);

//                case MType.InvalidDataExclamation:
//                    return XtraMessageBox.Show("Не все данные заполнены корректно",
//                                               "Внимание",
//                                               MessageBoxButtons.OK,
//                                               MessageBoxIcon.Warning);

//                default: return DialogResult.None;
//            }
//        }





//        internal static void ShowInformation(string text)
//        {
//            XtraMessageBox.Show(text, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Information);
//        }

//        internal static void ShowExclamation(string text)
//        {
//            XtraMessageBox.Show(text, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
//        }

//        internal static DialogResult ShowQuestion(string text)
//        {
//            return XtraMessageBox.Show(text, "Внимание", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
//        }

//        internal static void ShowError(string text)
//        {
//            XtraMessageBox.Show(text, "Внимание", MessageBoxButtons.OK, MessageBoxIcon.Error);
//        }


//        internal static DialogResult ShowStringInputBox(string text = null)
//        {
//            return DialogResult.Cancel;
            
//        }

//		internal static void ShowAdvancedMessage(string text, string header = "")
//		{
//			var frm = new frmMessageBox();

//			if (header != "")
//			{
//				frm.SetHeader(header);
//			}
//			frm.SetText(text);
//			frm.ShowDialog();
//		}

//		internal static void ShowNotification(string text)
//		{
//			int alertHeight = 150;
//			int alertWidth = 300;
//			Form currentForm = Form.ActiveForm;
//			AlertControl alertControl1 = new AlertControl();//this.components);
//			alertControl1.AllowHotTrack = false;
//			alertControl1.AutoFormDelay = 3000;
//			alertControl1.BeforeFormShow += (s, e) =>
//				{
//					e.AlertForm.Width = alertWidth;
//					e.AlertForm.Height = alertHeight;
//					e.Location = new System.Drawing.Point((Screen.PrimaryScreen.WorkingArea.Width - alertWidth) / 2, (Screen.PrimaryScreen.WorkingArea.Height - alertHeight) / 2);
//				};

//			alertControl1.Show(currentForm, "", text);
          
//		}

//        internal enum MType
//        {
//            /// <summary>
//            /// Имеются несохраненные изменения
//            /// </summary>
//            UnsavedChangesQuestion,
//            /// <summary>
//            /// Не все данные заполнены корректно
//            /// </summary>
//            InvalidDataExclamation
//        }


//    }
//}
