using System;
using System.Drawing;
namespace sql.builder.UI
{
    public interface IBase: IDisposable
    {
        string GetText();
        bool IsEditable();

        
       // bool IsButtonCustomVisibility(object button); // костыль, чтобы не приятать кнопки для нередактируемого поля если видимость установлена через переменную
    }
    internal interface IRange : IBase
    {
        void GetText(out string value_1, out string value_2);
    }
    public interface IForm 
    {
        string GetTitle();
        string GetFormName();
        void SetDialogContainer(object value);
        void LayoutSuspend();
        void LayoutResume();
        object GetDialogContainer();

    }

    public interface IList :  IBase
    {
        bool IsMultiselect();
        void ProcessFilter();
        bool TrySelectValueByName(string text);
       // void ListItemSelected(object[] changedNodes);
        void RefreshList();
      
        bool IsNeedRefresh();
        void ApplyValue();
        bool IsServerFilter();
        bool IsAutoCheck();
        void UpdateSelectedString();
        void SetNeedGetData(bool value);
        void SingeValueSelect(object id);
        bool IsExpandAll();
        string GetValFieldName();
        string GetKeyFieldName();
        void SetFocusIfNeed(); // Емцов - костыль для населенного пункта, если что переделать нормально
        void PrepareListSource();
        void ApplyArrayValueToControlDataEditorList();
       // void AddPart(IVControl control);
        ///
        void listEdit_AutoFilterChanged();
        void PopupContainerEdit_Closed();
        void PopupProcessing();
        //void PopupContainerEdit_ComboButtonClick();
        void PopupContainerEdit_Enter();
        void PopupContainerEdit_DeleteValue();
        void Tree_ColumnFilterChanged();
        void LoadAllListRows();
        Color GetListCellColor(object rowId, string columnName);

        void SuppressChangeEvent();
        void ResumeChangeEvent();
        void RaiseChanged();
        string GetSearchFieldName();
        string GetNameFieldName();
        bool HasCustomButtons();

    }
}