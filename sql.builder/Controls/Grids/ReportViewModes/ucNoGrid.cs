
//using System;
//using System.Drawing;
////using System.Windows.Forms;
//using DevExpress.XtraBars;
//using DevExpress.XtraGrid.Views.Grid;
//using sql.builder.Controls.Grids;
//using sql.builder.UI;

//namespace sql.builder.Controls
//{
//    internal partial class ucNoGrid : ucGridBase/*IReportGrid,*/ 
//    {
//        public string Info
//        {
//            get { return lInfo.Text; }
//            set { lInfo.Text = value; }
//        }
//        public VVariableDepandantceController GetVariableDepandantceController()
//        {
//            return null;
//        }

//        public event EventHandler OpenExcel;
//        public void OnOpenExcel(object sender, EventArgs args)
//        {
//            throw new NotImplementedException();
//        }



        

//        public ucNoGrid()
//        {
//            InitializeComponent();
//        }

//        public Control GetControl()
//        {
//            return this;
//        }
//        public PopupMenu GetMenu()
//        {
//            throw new NotImplementedException();
//        }
//        public void SetMenu(PopupMenu menu)
//        {
//            throw new NotImplementedException();
//        }
//        public override void SetFormingTime(string time)
//        {
//            lFormingTime.Caption = time != null
//                    ? "Сформирован за " + time
//                    : String.Empty;
//        }

//        public void SetPrintingTime(string time)
//        {
//            throw new NotImplementedException();
//        }

//        public string GetFormingTime()
//        {
//            throw new NotImplementedException();
//        }

//        public override void SetAvgFormingTime(string time)
//        {
//            lAvgFormingTime.Caption = time != null
//                    ? "Среднее время формирования " + time
//                    : String.Empty;
//        }
//        public void ShowExcel(string path)
//        {
//            throw new NotImplementedException();
//        }

//        public void AllowOpenExcel()
//        {
//            throw new NotImplementedException();
//        }

//        public void SetText(string text)
//        {
//            Info = text;
//        }
//        public void SetParent(ucTableViewerContainer parent)
//        {
//            throw new NotImplementedException();
//        }
//        public void SetSelectMode()
//        {
//            throw new NotImplementedException();
//        }
//        public bool IsSelectMode()
//        {
//            throw new NotImplementedException();
//        }
//        public GridView CurrentView { get; private set; }
//        public GridView GetMainView()
//        {
//            return null;
//        }
//        public void AddDataSourceChangedHandler(Action action, EventHandler action2)
//        {
//            DataSourceChanged += action;
//        }
//        public void RemoveDataSourceChangedHandler(Action action, EventHandler action2)
//        {
//            DataSourceChanged -= action;
//        }
//        public void AddUIEventHandler(UIEventHandler action)
//        {
//            UIEvent += action;
//        }
//        public void RemoveUIEventHandler(UIEventHandler action)
//        {
//            UIEvent -= action;
//        }
//        public void AddLayoutChangedHandler(Action action, EventHandler action2)
//        {
//            LayoutChanged += action;
//        }
//        public void RemoveLayoutChangedHandler(Action action, EventHandler action2)
//        {
//            LayoutChanged -= action;
//        }
//        public TableViewMode ViewMode { get; private set; }

//        public void AddHasMessageHandler(Action<string> action, HasMessageHandler action2)
//        {
//            HasMessage += action;
//        }
//        public void RemoveHasMessageHandler(Action<string> action, HasMessageHandler action2)
//        {
//            HasMessage -= action;
//        }


//        public void SetColumnVisibility(string columnName, bool value)
//        {
           
//        }


//        public void UpdateDummyColumnWidth()
//        {
            
//        }

        
//    }
//}
