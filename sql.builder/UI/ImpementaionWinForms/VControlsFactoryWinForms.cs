//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
////using System.Windows.Forms;
//using sql.builder.DataApi;
//using sql.builder;
//using sql.builder.Controls.Grids.ReportViewModes;
//using sql.builder.Controls.Grids;
//using sql.builder.Controls;
//namespace sql.builder.UI.WinForms
//{
//    class VControlsFactoryWinForms : IVControlsFactory
//    {
//        //public int GroupPaddingTop()
//        //{
//        //    return 0;
//        //}
//        //public int GroupPaddingLeft()
//        //{
//        //    return 5;
//        //}
//        //public int GroupPaddingRight()
//        //{
//        //    return 5;
//        //}
//
//        #region Layout
//
//
//
//        public int GroupBorderWidth()
//        {
//            return 2;
//        }
//
//
//        public int LabelHeight()
//        {
//            return 18;
//        }
//        public int ScrollBarWidth()
//        {
//            return 18;
//        }
//
//        public IVLayoutLabel CreateLabel()
//        {
//            return new VLayoutLabel();
//        }
//        public IVLayoutGroup CreateBorderedGroup()
//        {
//            return new VLayoutBorderedGroup();
//        }
//        public IVLayoutGroup CreateGroup(VLayoutGroupInfo info)
//        {
//            if ((info.GetProperty(TextConst.AName.ShowToolBar) as string) == TextConst.AVBool.True)
//            {
//                return new VLayoutBarGroup();
//            }
//            else
//            {
//                return new VLayoutGroup();
//            }
//
//        }
//        public IVLayoutControlContainer CreateControlContainer()
//        {
//            return new VLayoutControlContainer();
//        }
//        public IVLayoutTabs CreateTabs()
//        {
//            return new VLayoutTabs2();
//        }
//        public IVLayoutSplitContainer CreateSplitContainer()
//        {
//            return new VLayoutSplitContainer();
//        }
//        #endregion
//
//
//      
//
//        public IVCheckContainer CreateCheckContainer(IBase uibase)
//        {
//            return new VCheckContainer(uibase);
//        }
//       
//
//        public IVForm CreateForm()
//        {
//            return new UIFormControl();
//        }
//
//
//        public IVTextEdit CreateTextEdit()
//        {
//            return new VTextEdit();
//        }
//
//        public IVDateEdit CreateDateEdit()
//        {
//            return new VDateEdit();
//
//        }
//
//        public IVListEdit CreateListEdit(IucGrid list)
//        {
//            return new VListEdit(list);
//        }
//
//
//        public IVBarButton CreateBarButton()
//        {
//           return new VBarButton();
//            
//        }
//
//
//
//
//        public IVMemoEdit CreateMemoEdit()
//        {
//            return new VMemoEdit();
//        }
//
//
//        public IVFileEdit CreateFileEdit()
//        {
//            return new VFileEdit();
//        }
//
//        public IVButton CreateButton()
//        {
//            return new VButton();
//        }
//
//
//        public IucGridContainer CreateGridContainer(bool isTree)
//        {
//            return new ucGridContainerWF(isTree);
//        }
//
//        public IucTableViewerContainer CreateTableViewer(IControlWithTableSourcePubl controller)
//        {
//            return new ucTableViewerContainerWF( controller);
//        }
//
//
//        public IVBarMenu CreateBarMenu()
//        {
//            return new VBarMenu();
//        }
//
//
//        public IucGrid CreateGrid()
//        {
//            return new ucGridWF();
//        }
//
//        public IucGrid CreateTree()
//        {
//            return new ucTreeWF();
//        }
//
//
//        public string GetClipboardText()
//        {
//            string s = Clipboard.GetText();
//            return s;
//        }
//
//		public void SetClipboardText(string text)
//		{
//			Clipboard.SetText(text);
//		}
//
//
//        public IVEditorButton CreateEditorButton()
//        {
//            return new VEditorButton();
//        }
//
//
//        public IVBarEditContainer CreateBarEditContainer()
//        {
//            return new VBarEditContainer();
//        }
//
//
//        public IVPopupMenu CreatePopupMenu()
//        {
//            return new VPopupMenu();
//        }
//
//
//        public IVLinkEdit CreateLinkEdit()
//        {
//            return new VLinkEdit();
//        }
//
//
//        public IVTextEdit CreateNumberEdit()
//        {
//            return new VTextEdit();
//        }
//
//
//        private VDialogManager _dialogManager;
//        public IVDialogManager GetDialogManager(object clientId)
//        {
//            if (_dialogManager == null)
//            {
//                _dialogManager = new VDialogManager();
//            }
//            return _dialogManager;
//        }
//		private VNotificationManager _notificationManager;
//		public IVNotificationManager GetNotificationManager(object clientId) 
//		{
//			if (_notificationManager == null)
//			{
//				_notificationManager = new VNotificationManager();
//			}
//			return _notificationManager;
//		}
//    }
//}
