//using System;
//using System.Collections.Concurrent;
//using System.Collections.Generic;
//using System.Data;
//using System.Linq;
//using System.Xml.Linq;
//using DevExpress.Utils;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Repository;
//using DevExpress.XtraGrid.Views.Base;
//using DevExpress.XtraGrid.Views.Grid;

//using sql.builder.DataApi;
//using DevExpress.XtraBars;
//using System.Drawing;
////using System.Windows.Forms;
//using Devart.Data.Oracle;
//using infoenergo.ui.win.Base;
//using sql.builder.Controls.Grids;
//using  sql.builder.UI;
//namespace sql.builder.Controls
//{
//    internal partial class ucGridBase : ucBase
//    {
//        #region Закрытые переменные
//        protected VDataSet _source;
//        private GridView _view;

//        protected string _original_name;
//        protected string _report_name;
//        protected string _report_title;
//        protected string _report_form;
//        protected string _item_type;
//        protected string _is_template;
//        protected string _visible;

//        protected bool _from_file;
//        protected bool _compare_mode;
//        // флаг предотвращает лишнее срабатывание событий
//        protected bool _layout_reloading;

//        protected Dictionary<string,  Dictionary<string, VSXElement>> eventsTags;
//        #endregion
//        #region Свойства
//        public PopupMenu Menu;
//        public VDataSet DataSource
//        {
//            get { return _source; }
//            set
//            {
//                if (BeforeDataSourceChanged != null)
//                {
//                    BeforeDataSourceChanged();
//                }

//                _source = value;
//                UpdateDataSourceEventOptions();

//                if (DataSourceChanged != null)
//                {
//                    DataSourceChanged();
//                }

//            }
//        }

//        public GridView MainView
//        {
//            get
//            {
//                if (_view == null)
//                {
//                    _view = GetMainGridView();                  
//                }
//                return _view;
//            }
//        }

//        public string OriginalName
//        {
//            get { return _original_name; }
//        }
//        public string ReportName
//        {
//            get { return _report_name; }
//                 set {  _report_name=value; }
//        }
//        public string ReportTitle
//        {
//            get { return _report_title; }
//        }
//        public string ReportForm
//        {
//            get { return _report_form; }
//        }
//        public bool IsTemplate
//        {
//            get { return (_is_template != "0"); }
//        }
//        public bool FromFile
//        {
//            get { return _from_file; }
//        }

//        public bool IsCompareMode
//        {
//            get { return _compare_mode; }
//        }
//        public bool IsVisible
//        {
//            get { return _visible != "0"; }
//        }

//     //   protected ConcurrentDictionary<Tuple<string, int>, RepositoryItem> _repositories;

//        protected ConcurrentDictionary<string, RepositoryItem> _repositoriesReadonly;
//        protected ConcurrentDictionary<string, RepositoryItem> _repositoriesEditable;
//        #endregion
//        #region События
//        public event Action LayoutChanged;
//        public event Action DataSourceChanged;
//        public event Action BeforeDataSourceChanged;
//        public event Action<string> HasMessage;
//        protected void RaiseHasMessage(string text)
//        {
//            if (HasMessage != null)
//            {
//                HasMessage(text);
//            }
//        }

//        public event CellEvent CellRightClick;
//        public delegate void CellEvent(ucGridBase sender, CellEventArgs e);
//        public class CellEventArgs : object
//        {
//            public CellEventArgs(DataRow row, string columnName)
//            {
//                Row = row;
//                ColumnName = columnName;
//            }
//            public DataRow Row;
//            public string ColumnName;
//        }
//        protected void RaiseCellRightClick(DataRow row, string columnName)
//        {
//            if (CellRightClick != null)
//            {
//                CellRightClick(this, new CellEventArgs(row, columnName));
//            }
//        }

//        public event UIEventHandler UIEvent;
//        protected bool RaiseUIEvent(string tableName, string name, DataRow row, VDataColumn col)
//        {
//            if (UIEvent != null)
//            {
//                if (eventsTags != null)
//                {
//                    if (eventsTags.ContainsKey(tableName))
//                    {
//                        var fullName = name;
//                        if (col != null)
//                        {
//                            if (col.OriginalNameForPivotColumn != null)
//                            {
//                                fullName = col.OriginalNameForPivotColumn + "|" + fullName;
//                            }
//                            else
//                            {
//                                fullName = col.ColumnName + "|" + fullName;
//                            }
                            
//                        }
//                        if (!eventsTags[tableName].ContainsKey(fullName))
//                        {
//                            fullName = name;
//                        }

//                        if (eventsTags[tableName].ContainsKey(fullName))
//                        {
//                            UIEvent(this, new UIEventArgs(name, eventsTags[tableName][fullName], SourceTable(), row, col));
//                            return true;
//                        }
//                    }
//                }
//            }
//            return false;
//        }

//        public event delegateModifiedData ModifiedData;
//        protected void RaiseModifiedData(object sender, ModifiedDataEventArgs args)
//        {
//            if (ModifiedData == null) return;
            
//            ModifiedData(sender, args);
//        }
//        public event delegateAfterRefreshRows RefreshedData;
//        protected void RaiseRefreshedData(object sender, GridDataRefreshEventArgs args)
//        {
//            if (RefreshedData == null) return;

//            RefreshedData(sender, args);
//        }
//        public event delegateAccepted CommitedData;
//        protected void RaiseCommitedData(object sender, EventArgs args)
//        {
//            if (CommitedData == null) return;

//            CommitedData(sender, args);
//        }
//        #endregion
//        #region Открытые методы
//        public ucGridBase()
//        {
//            InitializeComponent();
//            BeforeDataSourceChanged += OnBeforeDataSourceChanged;
//            DataSourceChanged += OnDataSourceChanged;

//            //_repositories = new ConcurrentDictionary<Tuple<string, int>, RepositoryItem>();
//            _repositoriesEditable = new ConcurrentDictionary<string, RepositoryItem>();
//            _repositoriesReadonly = new ConcurrentDictionary<string, RepositoryItem>();
//            Disposed += OnDisposed;
//        }

//        protected virtual void OnDisposed(object sender, EventArgs args)
//        {
//            detachViewEvents(MainView);
//            // чтобы разорвать связь с обработчиками из UIFormC
//            Menu = null;

//            if (_source != null)
//            {
//                _source.Changed -= OnSourceOnChanged;
//                _source.NeedSelection -= OnSourceOnNeedSelection;
//            }
//            _repositoriesEditable.Clear();
//            _repositoriesReadonly.Clear();
//            //_repositories.Clear();
//        }

//        public bool Editable = true;
//        public virtual void SetEditable(bool val)
//        {
//            Editable = val;
//        }

//        public virtual BarManager GetBarManager()
//        {
//            return null;
//        }

//        public void AddEventTag(string eventName, VSXElement tag,string tableName="")
//        {
//            if (eventsTags == null)
//            {
//                eventsTags = new Dictionary<string, Dictionary<string, VSXElement>>();
//            }

//            if (!eventsTags.ContainsKey(tableName) )
//            {
//                eventsTags[tableName] = new Dictionary<string, VSXElement>();
//            }

//            eventsTags[tableName].Add(eventName, tag);

//            UpdateDataSourceEventOptions();
//        }

//        public void UpdateDataSourceEventOptions()// Для создания линков при экспорте в excel
//        {
//            if (eventsTags == null || DataSource == null) return;

//            foreach (var kv in eventsTags)
//            {
//                if (kv.Key != "")
//                {
//                    foreach (string s in kv.Value.Keys)
//                    {
//                        string[] ss = s.Split('|');
//                        if (ss.Length == 1)
//                        {
//                            (DataSource.Tables[kv.Key] as VDataTable).HasRowEvents = true;
//                        }
//                        else
//                        {
//                            foreach (VDataColumn col in (DataSource.Tables[kv.Key] as VDataTable).GetColumnsByPivotOriginalName(ss[0]))
//                            {
//                                col.HasCellEvents = true;
//                            }


//                        }
//                    }
//                }
//            }
            
//        }
       
//        public virtual void Initialize(Dictionary<string, string> report_info, bool from_file)
//        {
//            if (report_info == null) return;

//            _original_name = report_info["original_name"];
//            _report_name = report_info["repname"];
//            _report_title = report_info["title"];
//            _report_form = report_info["form"];
//            _item_type = report_info["item_type"];
//            _is_template = report_info["is_template"];
//            _visible = report_info["visible"];
//            _from_file = from_file;
//        }



//        public Dictionary<string, string> GetReportInfo()
//        {
//            var settings = new Dictionary<string, string>();

//            settings.Add("original_name", _original_name);
//            settings.Add("repname", _report_name);
//            settings.Add("title", _report_title);
//            settings.Add("form", _report_form);
//            settings.Add("item_type", _item_type);
//            settings.Add("is_template", _is_template);
//            settings.Add("visible", _visible);

//            return settings;
//        }
//        protected void SetLayoutChanged()
//        {
//            // Сгенерировать события, что визуальные настройки изменены
//            if (_layout_reloading) return;

//            if (LayoutChanged != null)
//            {
//                LayoutChanged();
//            }
//        }

//        #region Repository


//        //private List<int> rowindexes = new List<int>(); // для отладки
//        public RepositoryItem GetCellRepository(string column_name, int row_index/*, bool allowCreate*/)
//        {
//            //return null;
//            if (column_name == null)// системная колонка с чекбоксом
//            {
//                return null;
//            }
//            RepositoryItem rep = null;
//            //if (!rowindexes.Contains(row_index))
//            //{
//            //    rowindexes.Add(row_index);
//            //}
//            //var key = new Tuple<string, int>(column_name, row_index);
//            var key = column_name;
//            //if (column_name == "prichina_otz")
//            //{

//            //}

//            var vtable = (VDataTable)_source.Tables[MainView.Name];
//            var vcol = (VDataColumn)vtable.Columns[column_name];
//            if (vcol == null) return null;// системная колонка с чекбоксом
//           // if (!vcol.BoundControls.Any()) return;// потом убрать это
//          //  var ctrl = vcol.BoundControls.First();
//            var row=vtable.Rows[row_index];
//            var read_only = !Editable || !vcol.GetEditable(row);
//            vcol.GetVisibility(row);// чтобы зачистились невидимые значения
//            if (read_only)
//            {
//                _repositoriesReadonly.TryGetValue(key, out rep);
//            }
//            else
//            {
//                _repositoriesEditable.TryGetValue(key, out rep);
//            }

//            //rep = rep ?? GetCellRepository(column_name, row_index, true);
//            //if (rep == null)
//            //{
//            //    return;
//            //}

//            //_repositories.TryGetValue(key, out rep);
//            if (rep == null) {
//                //if (!allowCreate)
//                //{
//                //    return null;
//                //}
//                //var vtable = (VDataTable)_source.Tables[MainView.Name];
//                //var vcol = (VDataColumn)vtable.Columns[column_name];
//                //if (vcol == null) return null;// системная колонка с чекбоксом

//                if (!VDataColumn.HasBoundControl(vcol)) { // потом убрать это
//                    return null;
//                }
//                UIBase ctrl = vcol.BoundControls[0];
//                rep = ctrl.GetRepositoryItem();

                
//                MainView.GridControl.RepositoryItems.Add(rep);

//                // чтобы время отображалось в ячейках без фокуса
//                if (ctrl is UIDateTime)
//                {
//                    var gcol = MainView.Columns[vcol.ColumnName];
//                    gcol.DisplayFormat.FormatType = FormatType.DateTime;
//                    gcol.DisplayFormat.FormatString = "g";
//                }

//                // сразу обновляем редактируемость и валидацию
//                //UpdateCellEditable(column_name, row_index, rep);
//                //UpdateCellValidation(column_name, row_index, rep);
//                //var row = vtable.Rows[row_index];
//                // проблема: когда колонка _x_n, ошибка сидит в ней, а тут ошибка вешается не на ту колонку. Закоментил - вроде и так работает
//                //row.SetColumnError(vcol, vcol.GetValidation(row));
//                SetRepositoryEditable(rep, read_only, ctrl);

//                if (read_only)
//                {
//                    _repositoriesReadonly.TryAdd(key, rep);
//                }
//                else
//                {
//                    _repositoriesEditable.TryAdd(key, rep);
//                }
//                //_repositories.TryAdd(key, rep);
//            }

//            return /*(RepositoryItemButtonEdit)*/rep;
//        }

//        //private static void setStatusButtonProps(EditorButtonCollection buttons,string msg)
//        //{
            
//        //    var valid = Cmn.Nvl(msg, "").ToString() == "";
//        //    var statusButton = buttons.Cast<EditorButton>().FirstOrDefault(b => b.Tag is string && b.Tag.ToString() == "status");
//        //    if (!valid)
//        //    {
//        //        statusButton.Caption = msg;
//        //        (statusButton.SuperTip.Items[0] as ToolTipItem).Text = msg;
//        //        statusButton.Visible = true;
//        //    }
//        //    else
//        //    {
//        //        if (statusButton != null)
//        //        {
//        //            statusButton.Visible = false;
//        //        }
//        //    }
              
            

//        //}
//        //public void UpdateCellValidation(string column_name, int row_index, RepositoryItem rep = null)
//        //{

//        //    var vtable = (VDataTable)_source.Tables[MainView.Name];
//        //    var vcol = (VDataColumn)vtable.Columns[column_name];
//        //    if (vcol == null) return;// системная колонка с чекбоксом
//        //    if (!vcol.BoundControls.Any()) return;// потом убрать это
//        //    var ctrl = vcol.BoundControls.First();

        
//        //    var msg = vcol.GetValidation (vtable.Rows[row_index]);
//        //  // остальное делается при отрисовке ячейки
//        //   // rep = rep ?? GetCellRepository(column_name, row_index);
//        //   // rep.Tag = msg;
//        //    // list, combobox
//        //    //if (ctrl is UIList)
//        //    //{
//        //    //    var edit = rep as RepositoryItemPopupContainerEdit;

//        //    //    setStatusButtonProps(edit.Buttons,msg);



//        //    //}
//        //    //else
//        //    //{
              

//        //    //    var edit = rep as RepositoryItemButtonEdit;
//        //    //    setStatusButtonProps(edit.Buttons, msg);
//        //    //}

//        //}

//        public void SetRepositoryEditable(RepositoryItem rep, bool read_only, UIBase ctrl)
//        {
//            // list, combobox
//            if (ctrl is UIList)
//            {
//                var edit = rep as RepositoryItemPopupContainerEdit;
//                // edit.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

//                edit.UseReadOnlyAppearance = read_only;

//                var statusButton = edit.Buttons.Cast<EditorButton>().FirstOrDefault(b => b.Tag is string && b.Tag.ToString() == "status");

//                foreach (var b in edit.Buttons.Cast<EditorButton>().Where(b => b != statusButton)) b.Visible = !read_only;
//            }
//            // file
//            else if (ctrl is UIFile)
//            {
//                var edit = rep as RepositoryItemButtonEdit;
//                edit.UseReadOnlyAppearance = read_only;
//                // в режиме readonly можно просматривать файлы
//                foreach (var b in edit.Buttons.Cast<EditorButton>().Where(b => b.Kind != ButtonPredefines.Search)) b.Visible = !read_only;
//            }
//            // Остальные
//            else
//            {
//                rep.ReadOnly = read_only;

//                var edit = rep as RepositoryItemButtonEdit;
//                if (edit != null)
//                {
//                    foreach (var b in edit.Buttons.Cast<EditorButton>()) b.Visible = !read_only;
//                }
//            }
//        }

//        //public void UpdateCellEditable(string column_name, int row_index, RepositoryItem rep = null)
//        //{
//        //    var vtable = (VDataTable)_source.Tables[MainView.Name];
//        //    var vcol = (VDataColumn)vtable.Columns[column_name];
//        //    if (vcol == null) return;// системная колонка с чекбоксом
//        //    if (!vcol.BoundControls.Any()) return;// потом убрать это
//        //    var ctrl = vcol.BoundControls.First();

//        //    var read_only = !Editable || !vcol.GetEditable(vtable.Rows[row_index]);

//        //    rep = rep ?? GetCellRepository(column_name, row_index,true);
//        //    if (rep == null)
//        //    {
//        //        return;
//        //    }
//        //    // list, combobox
//        //    if (ctrl is UIList)
//        //    {
//        //        var edit = rep as RepositoryItemPopupContainerEdit;
//        //       // edit.ButtonsStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder;

//        //        edit.UseReadOnlyAppearance = read_only;

//        //        var statusButton = edit.Buttons.Cast<EditorButton>().FirstOrDefault(b => b.Tag is string && b.Tag.ToString() == "status");

//        //        edit.Buttons.Cast<EditorButton>().Where(b => b != statusButton).ForEach(b => b.Visible = !read_only);
//        //    }
//        //    // file
//        //    else if (ctrl is UIFile)
//        //    {
//        //        var edit = rep as RepositoryItemButtonEdit;
//        //        edit.UseReadOnlyAppearance = read_only;
//        //        // в режиме readonly можно просматривать файлы
//        //        edit.Buttons.Cast<EditorButton>().Where(b => b.Kind != ButtonPredefines.Search).ForEach(b => b.Visible = !read_only);
//        //    }
//        //    // Остальные
//        //    else
//        //    {
//        //        rep.ReadOnly = read_only;

//        //        var edit = rep as RepositoryItemButtonEdit;
//        //        if (edit != null)
//        //        {
//        //            edit.Buttons.Cast<EditorButton>().ForEach(b => b.Visible = !read_only);
//        //        }
//        //    }
//        //}
//        #endregion

//        public VDataTable SourceTable()
//        {
//            if (_source == null) return null;
//            return (VDataTable) ((MainView != null) ? _source.Tables[MainView.Name] : _source.Tables[0]);
//        }
//        public virtual void AcceptSelection()
//        {
          
//        }
//        public virtual void SetSelection(IEnumerable<object> values)
//        {

//        }
//        public void UpdateDataSourceSelectedRows()
//        {
//            List<DataRow> list = new List<DataRow>();
//            foreach (int i in MainView.GetSelectedRows())
//            {
//                list.Add(MainView.GetDataRow(i));
//            }
//            SourceTable().SelectedRows = list;

//        }
//        #endregion
//        #region Virtual
//        public virtual GridView GetMainGridView() { return null; }

//        public virtual void BeginUpdate() { }
//        public virtual void EndUpdate() { }

//        public virtual void SetGlobalSettings(Dictionary<string, string> global_settings) { }
//        public virtual Dictionary<string, string> GetGlobalSettings() { return null; }

//        public virtual void ExportToXlsx(string fullpath, string caption = null) { }

//        public virtual void SetFormingTime(string time) { }
//        public virtual void SetAvgFormingTime(string time) { }

//        public virtual void SetComparedMode(VDataSet compared_ds) { }
//        //public virtual void RaiseLoad()
//        //{

//        //}
//        protected virtual void OnDataSourceChanged()
//        {
//            if (_source == null) return;
//            _source.Changed += OnSourceOnChanged;
//            _source.NeedSelection += OnSourceOnNeedSelection;
//        }
//        protected virtual void OnBeforeDataSourceChanged()
//        {
//            // чтобы не было утечек
//            if (_source == null) return;
//            _source.Changed -= OnSourceOnChanged;
//            _source.NeedSelection -= OnSourceOnNeedSelection;
//        }
//        private void OnSourceOnNeedSelection(object sender, EventArgs args)
//        {
//            UpdateDataSourceSelectedRows();
//        }
//        private void OnSourceOnChanged(object sender, EventArgs args)
//        {
//            if (MainView.IsEditing)
//            {
//                MainView.EditingValue = MainView.GetFocusedValue();
//            }
//        }

//        public virtual void GenerateLayoutChanged() { }
//        public virtual void SetMasterControl(ucGridBase parentControl) { }

//        public virtual void LoadSchemeSettingsFromXml(XElement xRoot) { }
//        public virtual void SaveSchemeSettingsToXml(XElement xRoot) { }

//        public virtual void AcceptChanges()
//        {
//            UpdateDataSourceSelectedRows();
//        }
//        public virtual void DismissChanges(Dictionary<DataRow, OracleException> error_rows, bool get_my_errors = false)
//        {
            
//        }

//        public virtual bool IsModifiedData() { return false;}
//        public virtual bool SetUnmodified()
//        {
//            return false;
//        }
//        public virtual void SetToolbarVisible(bool val)
//        {

//        }
//        public virtual void SetTitle(string title)
//        {

//        }
//        public virtual void SetToolbarButtonVisible(string name, bool visible) { }
//        public virtual void HideToolbarButtons() { }
//        public virtual void AddToolBarItem(BarItem item) { }

//        public virtual void SetSummaryVisible(bool val) { }
//        public virtual void SetMultiselect(bool val) { }
//        #endregion
//        #region Обработчики событий




//        List<GridView> viewsWithEvents = new List<GridView>();

//        protected void detachViewEvents(GridView view)
//        {
//            if (viewsWithEvents.Contains(view))
//            {
//                viewsWithEvents.Remove(view);
//                view.FocusedRowChanged -= view_FocusedRowChanged;
//                view.CustomDrawCell -= view_CustomDrawCell;
//                view.CustomRowCellEdit -= view_CustomRowCellEdit;

//                view.ValidatingEditor -= view_ValidatingEditor;
//                view.CustomColumnDisplayText -= view_OnCustomColumnDisplayText;
//            }
//        }
//        protected  void attachViewEvents(GridView view)
//        {
//            if (!viewsWithEvents.Contains(view))
//            {
//                viewsWithEvents.Add(view);
//                view.FocusedRowChanged += view_FocusedRowChanged;
//                view.CustomDrawCell += view_CustomDrawCell;
//                view.CustomRowCellEdit += view_CustomRowCellEdit;
//                // отключаем штатную валидацию, т.к. у нас своя с блэк джеком и прочим
//                view.ValidatingEditor += view_ValidatingEditor;
//                view.CustomColumnDisplayText += view_OnCustomColumnDisplayText;
//            }
//        }

//        void view_ValidatingEditor(object sender, BaseContainerValidateEditorEventArgs e)
//        {
//            e.Valid = true;
//        }

//        void ucGridBase_Load(object sender, EventArgs e)
//        {
//            var view = MainView;
            
//            if (view != null)
//            {
//                attachViewEvents(view);
//            }

//            CellRightClick += Grid_CellRightClick;
//        }

//        void view_FocusedRowChanged(object sender, FocusedRowChangedEventArgs args)
//        {
//            var view = (sender as GridView);
//            var dt = (VDataTable)DataSource.Tables[view.Name];
           
//            dt.CurrentRow = view.GetFocusedDataRow();
//        }
//        void view_CustomDrawCell(object sender, RowCellCustomDrawEventArgs args)
//        {
//            //
//        }
//        void view_CustomRowCellEdit(object sender, CustomRowCellEditEventArgs args)
//        {
//            // Чтобы не мешалось
//            if (this.GetType() != typeof (ucReferenceGrid)) return;

//            var view = sender as GridView;
//            if (!view.IsValidRowHandle(args.RowHandle) || args.RowHandle < 0) return;
//            //if ((this as ucReferenceGrid).IsFetchProcessing) return;
           
//            var column_name = args.Column.FieldName.Replace(TextConst.Pfx.ExtValName,"");
//            var row_index = view.GetDataSourceRowIndex(args.RowHandle);
//            //var r = view.GetDataRow(args.RowHandle);
//            RepositoryItem rep = null;
//            //var vis = true;
//            //if (r != null)
//            //{
//                //var table = r.Table;
//                //var colName = args.Column.FieldName;
//                //if (!string.IsNullOrEmpty(column_name))
//                //{
//                //    var column = table.Columns[colName] as VDataColumn;
//                //    vis=column.GetVisibility(r);
//                //    if (!vis)
//                //    {
//                //        return;
//                //    }
//                //}
//            //}

//            rep = GetCellRepository(column_name, row_index/*, true*/);
//            //(rep as RepositoryItemButtonEdit).Buttons[0].Appearance.Image
//            args.RepositoryItem = rep;
//            //(rep as RepositoryItemButtonEdit).ButtonsStyle = BorderStyles.Simple;
//            //(rep as RepositoryItemButtonEdit).BorderStyle = BorderStyles.NoBorder;
        
          
//        }
//        void view_OnCustomColumnDisplayText(object sender, CustomColumnDisplayTextEventArgs args)
//        {
//            // заглушка для UIDateTime, т.к. настройки репозитория не влияют на показ значения без фокуса
//            // 17.03.2016 Убрал, тк даты выводились со временем  в отчёте 35316-check1-detail
//            //var val = args.Value as DateTime?;
//            //if(val != null && (val.Value.Hour > 0 || val.Value.Minute > 0))
//            //{
//            //    args.DisplayText = val.Value.ToString("g");
//            //}
//        }
//        #endregion
//        public void Grid_CellRightClick(ucGridBase sender, CellEventArgs e)
//        {
//            //Бельченко 27.03.16 сделал метод  статическим, чтобы испоьзовать для грида в отчетах - получилось криво - переделать при случае.
//            //Емцов - статические обработчики держат в памяти объект
//            if (Menu == null) return;
//            sender.Menu.ShowPopup(new Point(Cursor.Position.X, Cursor.Position.Y));
//        }

//        public virtual string GetTopTableName()
//        {
//            return null;
//        }

//        public void UpdateEvents(XElement xevents)
//        {
//            foreach (XElement xcmd in xevents.Elements(TextConst.EName.UseAction))
//            {
//                var action = Cmn.GetActionInfo(xcmd);
//                AddEventTag(xcmd.Attribute(TextConst.AName.EventName).Value, action);
//            }
//        }

//        public void UpdateToolbar(XElement xtoolbar, BarItem[] items)
//        {
//            if (Cmn.GetAttrValue(xtoolbar, TextConst.AName.ColumnVisible) == TextConst.AVBool.False)
//            {
//                HideToolbarButtons();
//            }

//            // настраиваем дефолтные кнопки
//            foreach (var xcmd in xtoolbar.Elements(TextConst.EName.UICommand).Where(xcmd => xcmd.Attribute(TextConst.AName.ControlName) != null))
//            {
//                var visible = (Cmn.GetAttrValue(xcmd, TextConst.AName.ColumnVisible) == TextConst.AVBool.True);
//                SetToolbarButtonVisible(xcmd.Attribute(TextConst.AName.ControlName).Value, visible);
//            }

//            foreach (var barItem in items) AddToolBarItem(barItem);
//        }
//        public virtual void UpdatePopupMenu(XElement xmenu, BarItem[] items)
//        {
            
//        }

//        public virtual void RestoreViewChanges() {}
//        public virtual void HoldViewChanges() {}

//    }
//    public class VGCcbItem /*скорее всего нужно избавиться от этого класса, или сделать его не публичным, пока так*/
//    {
//        public VGCcbItem()
//        {
//            ID = -1;
//            Level = -1;
//        }

//        public int ID { get; set; }
//        public string Name { get; set; }
//        public string Caption { get; set; }
//        public int Level { get; set; }

//        public override string ToString()
//        {
//            string str = (String.IsNullOrEmpty(Caption) ? Name : Caption);

//            return (Level == -1) ? str : String.Format("{0}{1}", new String('\t', Level), str);
//        }
//    }

   
//}
