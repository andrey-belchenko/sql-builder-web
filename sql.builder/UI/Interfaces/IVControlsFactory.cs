//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using sql.builder.Controls;
//namespace sql.builder.UI
//{
//    public interface IVControlsFactory
//    {

//        #region Layout
//        IVLayoutGroup CreateBorderedGroup();
//        IVLayoutControlContainer CreateControlContainer();
//        IVLayoutLabel CreateLabel();
//        IVLayoutTabs CreateTabs();
//        IVLayoutSplitContainer CreateSplitContainer();
//        IVLayoutGroup CreateGroup(VLayoutGroupInfo info);
        
//        int GroupBorderWidth();
//        int LabelHeight();
//        int ScrollBarWidth();
//        #endregion



//        IVCheckContainer CreateCheckContainer(IBase uibase);
//        IVTextEdit CreateNumberEdit();
//        IVTextEdit CreateTextEdit();
//        IVLinkEdit CreateLinkEdit();
//        IVMemoEdit CreateMemoEdit();
//        IVDateEdit CreateDateEdit();
//        IVListEdit CreateListEdit(IucGrid list);
//        IVForm CreateForm();
//        IVBarButton CreateBarButton();
//        IVBarMenu CreateBarMenu();
//        IVBarEditContainer CreateBarEditContainer();
//        IVPopupMenu CreatePopupMenu();
//        IVFileEdit CreateFileEdit();
//        IVButton CreateButton();
//        IVEditorButton CreateEditorButton();
//        IucGridContainer CreateGridContainer(bool isTree);
//        IucTableViewerContainer CreateTableViewer(IControlWithTableSourcePubl controller);
//        IucGrid CreateGrid();
//        IucGrid CreateTree();
//		void SetClipboardText(string text);
//        string GetClipboardText();
//        IVDialogManager GetDialogManager(object clientId);
//		IVNotificationManager GetNotificationManager(object clientId);

//    }
//}
