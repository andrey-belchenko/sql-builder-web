//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Text;
//using System.Data;
//using sql.builder.Controls.Grids.ReportViewModes;
//using sql.builder.DataApi;
//namespace sql.builder.UI
//{
//    public interface IucGrid: IVControl, IVNormalControl
//    {
//        void DefaultSelection();
//        void InitColumnsEditors();
//        void SetShowRoot(bool value);
//        void SetShowAutoFilterRow(bool value);
//		void SetAllowSelectMoveColumns(bool value);
//        void SetColumnEditable(object column, bool value);
//        void SetColumnAutoFilterCondition_BeginsWith(object column);
//        void SetColumnAutoFilterCondition_Contains(object column);
        
//        Dictionary<string, int> GetColumnsWidth();
       
//         void PrepareForData();
//         void PrepareForList();
//         void Clear();
//         void CreateView(string name, bool banded);
//         void BeginViewUpdate(string name);
//         void EndViewUpdate(string name);
//         void DxLockReloadNodes();
//         void DxUnlockReloadNodes();
//         void DxEndCurrentEdit();
//         void SetViewTitle(string name,string value);
//         void SetMainView(string name);
//         void ClearViewContent(string name);
//         object CreateBand();
//         object CreateBandColumn();
//         object CreateColumn();
//         void AddColumnToBand(object band, object column);
//         void AddBandToView(string viewName, object band);
//         void AddBandToBand(object parenBand, object childBand);
//        void SetShowBands(string viewName, bool value);

//        void AddBandedDummyColumnAndHideEmptyBands(string viewName);// костыль
//        //void AddDummyColumn(string viewName);// костыль

//        void AddColumnToView(string viewName, object column);
//        void SetParentView(string childViewName, string parentViewName);



//        void SetBandTitle(object band,string value);
//        void SetBandName(object band, string value);
//        void SetBandWidth(object band, int value);
//        void AnalizeBandVisibility(object band);// костыль
//        void SetColumnGroupIndex(object column, int value);
//        void SetColumnSortOrder(object column, string value);
//        void SetColumnFieldName(object column, string value);
//        void SetColumnWidth(object column, int value);
//        void SetColumnExtOption(object column, string optionName,string value);

//        void SetColumnTypeFormatAndSummary(string viewName, object column, string type, string format, string agg);

//        void SetColumnTitle(object column, string value);
//        void SetColumnTitle(string tableName, string columnName, string value);
//        object GetColumnByFieldName(string tableName, string columnName);
//        object GetColumnByFieldName( string columnName);
//        void SetColumnVisible(object column, bool value);
//		void SetColumnInvisibleInColumnChooser(object column);
//        void SetColumnFixedLeft(object column);
//        void SetColumnFixedRight(object column);
//        void SetColumnHAlign(object column, string value);
       
//        void SetDataSource(IVTableDataAdapter dataSource);


//        void EndUpdateData();

//        string[] GetVisibleColumnsNames(string tableName);

//        void EndEdit();
//        void CloseEditor();// то же самое по сути - разобраться
//        void BeginControlUpdate();

//        void EndControlUpdate();

//        DataRow GetRowByHandle(int handle);
//        object[] GetColumns(object view = null);
//        object[] GetViews();
//        bool IsColumnSumSummaryType(object column);
//        string GetColumnFieldName(object column);
//        string GetColumnName(object column);
//        string GetViewName(object view);
//        void SetColumnCustomSummaryType(object column);
//        void SetColumnSumSummaryType(object column);
//        void SetColumnEditor(object column,IVCheckContainer editor);
//        void ExportToXlsx(string fullpath,bool dxExport, string caption = null);
//        void SetAllowCellMerge(bool value);
//        void ClearSelection();
//        void SelectRow(DataRow row);
//        void SetFocusedRow(DataRow row);
//		void ResetFocus();
//        DataRow GetFocusetDataRow();
//        int GetColumnWidth(object column);
//        void SetFocusedCell(string columnName, DataRow row);
//        DataRow[] GetSelectedRows();
//        int[] GetSelectedRowsHandles();
//        int GetRowsCount();
//        void SetColumnAllowSort(object column, bool value);
//        //void SetColumnSort(object column, bool ascending);
//        void SetColumnSortIndex(object column, int value);
//        void DxSetEditingValEqFocusedVal();// какая-то девэкспресовская химия
//        void BeginSort();
//        void EndSort();
//        void SetSelectionMode(int mode/*0 - Report , 1-Data, 2-Select*/);
//        void SetMultiSelect( bool value);
//        void SetShowFooter(bool value);
//        void FitHeaderHeight(object view);
//        void SetGridTopTable(DataTable dt);
//        void SetEnableMasterViewMode(bool value);
//        void SetShowDetailTabs(bool value);

//        void ControlForceInitialize();
//        void DisposeViews();
//        void DxRefreshViewData();
//        void DxRefreshRow(int handle);
//        bool GetColumnVisible(object column);
//        int GetColumnVisibleIndex(object column);
//        int GetColumnGroupIndex(object column);
//        string GetColumnSortOrder(object column);
//        void SetColumnVisibleIndex(object column, int value);
//       // void SetColumnGroupIndex(object column, int value);
//        void DxUpdateSummary();
//        void DxExpandAllGroups();
//        object DxGetListSourceRowCellValue(object row, string columnName);
//        void WfAttachViewEvents();
//        void WfDetachViewEvents();
//        object GetViewByName(string viewName);
//        event CustomProcessEventHandler ViewCustomRowFilter;
//        event ValueChangeEventHandler ViewSelectionChanged;
//        event CellEventHandler CellDoubleClick;
//        void UpdateDummyColumnWidth();
//        void SetParentFieldName(string value);
//        void SetKeyFieldName(string value);
//        void ExpandAllNodes();
//		void ShowLoadingControl();
//		void HideLoadingControl();
//    }
//}
