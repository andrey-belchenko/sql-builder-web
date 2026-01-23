//using System;
//using System.Collections.Generic;
//using System.Data;
////using System.Windows.Forms;
//using System.Xml.Linq;
//using Devart.Data.Oracle;
//using DevExpress.XtraBars;
//using DevExpress.XtraGrid.Views.BandedGrid;
//using DevExpress.XtraGrid.Views.Grid;
//using sql.builder.DataApi;
//using sql.builder.UI;

//namespace sql.builder.Controls.Grids
//{
//    /// <summary>
//    /// Для быстрого переключения между ucReportGrid и ucReportGridNew
//    /// </summary>
//    //internal interface IReportGrid  // Этот интерфейс лишний т.к. ucReportGrid (переименован в ucReportGridOld (удалить) ) уже не используется
//    //{
//    //    string OriginalName { get; }
//    //    string ReportName { get; set; }
//    //    string ReportTitle { get; }
//    //    string ReportForm { get; }
//    //    bool IsTemplate { get; }
//    //    bool FromFile { get; }

//    //    bool IsCompareMode { get; }
//    //    //bool IsVisible { get; }

//    //    VDataSet DataSource { get; set; }

//    //    void BeginUpdate();
//    //    void EndUpdate();
//    //    void ExportToXlsx(string path, string title);

//    //    GridView CurrentView { get; }
//    //    GridView GetMainView();

//    //    void SetComparedMode(VDataSet ds);

//    //    void AddDataSourceChangedHandler(Action action, EventHandler action2);
//    //    void RemoveDataSourceChangedHandler(Action action, EventHandler action2);

//    //    void AddUIEventHandler(UIEventHandler action);
//    //    void RemoveUIEventHandler(UIEventHandler action);

//    //    void AddLayoutChangedHandler(Action action, EventHandler action2);
//    //    void RemoveLayoutChangedHandler(Action action, EventHandler action2);

//    //    void AddHasMessageHandler(Action<string> action, HasMessageHandler action2);
//    //    void RemoveHasMessageHandler(Action<string> action, HasMessageHandler action2);

//    //    TableViewMode ViewMode { get; }
//    //    void Initialize(Dictionary<string, string> report_info, bool from_file);
//    //    void AddEventTag(string eventName, VSXElement tag, string tableName = "");


//    //    Control GetControl();
//    //    BarManager GetBarManager();

//    //    void SetFormingTime(string time);
//    //    void SetPrintingTime(string time);
//    //    string GetFormingTime();
//    //    void SetAvgFormingTime(string time);
//    //    Dictionary<string, string> GetReportInfo();

//    //    void GenerateLayoutChanged();

//    //    void SaveSchemeSettingsToXml(XElement xroot);
//    //    void LoadSchemeSettingsFromXml(XElement xroot);

//    //    string GetTopTableName();

//    //    void ShowExcel(string path);
//    //    void AllowOpenExcel();
//    //    void SetText(string text);

//    //    void SetParent(IReportGrid parent);

//    //    void SetToolbarButtonVisible(string name, bool visible);

//    //    void SetTopToolbarVisible(bool visible);
//    //    void SetSummaryVisible(bool visible);
//    //    void SetMultiselect(bool multiselect);
//    //    void SetTitle(string title);

//    //    void UpdateEvents(XElement xevents);
//    //    void UpdateToolbar(XElement xtoolbar, BarItem[] items);
//    //    void UpdatePopupMenu(XElement xmenu, BarItem[] items);

//    //    void AcceptChanges();
//    //    void DismissChanges(Dictionary<DataRow, OracleException> error_rows, bool get_my_errors = false);
//    //    void AcceptSelection();
//    //    void SetSelection(IEnumerable<object> values);

//    //    void RestoreViewChanges();
//    //    void HoldViewChanges();

//    //    void SetSelectMode();
//    //    bool IsSelectMode();
//    //    void SetColumnVisibility(string columnName, bool value);
//    //    VVariableDepandantceController GetVariableDepandantceController();
//    //    //void UpdateDummyColumnWidth();

//    //    event EventHandler OpenExcel;
//    //    void OnOpenExcel(object sender, EventArgs args);
//    //}
//}
