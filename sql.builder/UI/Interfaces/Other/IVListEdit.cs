//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Data;
//using System.Xml.Linq;
//namespace sql.builder.UI
//{
//    public interface IVListEdit : IVControl, IVNormalControl
//    {
//        void SetText(object value);
//        void SetValue(object value);
//        string GetText();
//        void PopupShow();
//        void PrepareList();
//        void SetMultiselect();
//        void SetPopupHeight(int val);
//        void SetPopupWidth(int val);
        
//        void SetListRowsLimit(int value);
//        void SetFocusToFilter();

//        void ShowFooterPanel();


//        void HideFooterPanel();

     

//        int GetPopupHeight();
//        int GetPopupWidth();
        
//        IucGrid GetList();
//        void SetController(IList controller);
       
//        void ClearListFocus();
//        void EndInitList();
//        void Close_Popup();
        
//        void EndInit();
//        void DoPlacement();
       
//        void BeginLoadData();
//        void EndLoadData();

//        void ShowConditions(bool val);

//        void ClearFilter();
//        Dictionary<string, string> GetFilterValues();

//    }
//}
