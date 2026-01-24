using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
//using System.Windows.Forms;
using System.Xml.Linq;
//using DevExpress.Utils;
//using DevExpress.Utils.Win;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using DevExpress.XtraEditors.Repository;

//using DevExpress.XtraTreeList;
//using DevExpress.XtraTreeList.Columns;
//using DevExpress.XtraTreeList.Nodes;
using sql.builder.DataApi;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
//using sql.builder.WebReports;

namespace sql.builder.UI
{
    internal partial class UIList : UIBase, IList
    {
        #region Поля
        private bool _need_set_focus;
        private bool _controlListinInted;
        private bool cancelRefresh;
        private bool auto_check;
        private bool clear_on_list_change;
        private bool expand_all;
        #endregion
        #region Свойства
        // реализация IBase.ApplyValue()
        public void ApplyValue()
        {
            if (this.SourceType == ReturnType.Array) {
                DataTable dt = this.DataTableList;
                DataColumn col_check = dt.Columns["check"];
                DataColumn col_value = dt.Columns[this.value_field_name];
                List<object> list = new List<object>();
                for (int index = 0; index < dt.Rows.Count; index++) {
                    DataRow row = dt.Rows[index];
                    if (Cmn.INT32_ONE.Equals(row[col_check])) {
                        list.Add(row[col_value]);
                    }
                }
                this.SetArraySourceValueMultiple(list, null, false);
            }
        }
        // Реализация IList.IsMultiselect()
        public bool IsMultiselect()
        {
            return this.SourceType == ReturnType.Array;
        }
        internal bool ShowFooterPanel {
            set {
                //if (value) {
                //    listEdit.ShowFooterPanel();
                //} else {
                //    listEdit.HideFooterPanel();
                //}
            }
        }
        #endregion
        #region События
        //public event EventHandler EditValueChanged; // вроде не используется
        //protected void OnEditValueChanged(object sender, EventArgs args)
        //{
        //    if (EditValueChanged != null)
        //    {
        //        EditValueChanged(sender, args);
        //    }
        //}
        #endregion
        #region Открытые методы
        public UIList()
        {
            // InitializeComponent();
            // InitializeComponent();
            //tree.Disposed += (sender, args) =>
            //{
            //    UIBase a = this;
            //};
            //Size = new Size(200, 20);
        }
        // Реализация IList.IsAutoCheck
        public bool IsAutoCheck()
        {
            return this.auto_check;
        }
        // Реализация IList.IsExpandAll
        public bool IsExpandAll()
        {
            return this.expand_all;
        }
        // Реализация IList.GetNameFieldName
        public string GetNameFieldName()
        {
            return this.name_field_name;
        }
        // Реализация IList.GetSearchFieldName
        public string GetSearchFieldName()
        {
            return this.search_field_name;
        }
        public string ParentFieldName {
            get {
                if (this.parent_field_name != null) {
                    return this.parent_field_name;
                } else {
                    VDataTable dt = this.DataTableList;
                    if (dt != null) {
                        return dt.TreeParentFieldName;
                    } else {
                        return null;
                    }
                }
            }
        }
        #endregion
        #region Закрытые методы
        protected override void InitControl()
        {
            InitControlList();
        }

        

        private void AddColumn(string columnName, string columnTitle, XElement xviewcolumn)
        {
        }
        protected override void InitControlList()
        {
            VDataTable dt = this.DataTableList;
            if (dt == null || dt.Columns.Count == 0) {
                return;
            }
            if (this.Form.FormUseType != UIFormC.UseType.SchemeEditor) {
                if (this._controlListinInted) {
                    return;
                }
                this._controlListinInted = true;
            }
            //this.listEdit.GetList().BeginViewUpdate(null);
            //this.listEdit.GetList().SetKeyFieldName(this.key_field_name);
            //this.listEdit.GetList().SetParentFieldName(this.ParentFieldName);
            if (!string.IsNullOrEmpty(this.ParentFieldName)) {
                //this.listEdit.GetList().SetShowRoot(true);
            }
            SortedList<string, XElement> viewcolumns = new SortedList<string, XElement>();
            if (this.DataTableList.Scheme != null) {
                foreach (XElement xcol in dt.Scheme.Descendants(EName.viewcolumns).Descendants(EName.column)) {
                    viewcolumns.Add(xcol.Attribute(AName.name).Value, xcol);
                }
            }
            int visibleColumnsCount = 0;
            DataColumn alternativeNameColumn = null;
            foreach (DataColumn column in dt.Columns) {
                string column_name = column.ColumnName;
                XElement xcol;
                viewcolumns.TryGetValue(column_name, out xcol);
                this.AddColumn(column_name, column.Caption, xcol);
                if (column_name != "check") {
                    object saved_width;
                    int width;
                    if (this.UserSettings.TryGetValue("colw_" + column_name, out saved_width)) {
                        width = (int)saved_width;
                    } else {
                        width = 120;
                    }
                    //object lcol = this.listEdit.GetList().GetColumnByFieldName(column_name);
                    //this.listEdit.GetList().SetColumnWidth(lcol, width);
                    bool vis = UIBase.IsColumnShouldBeVisible(column);
                    if (vis) {
                        visibleColumnsCount++;
                    }
                    //this.listEdit.GetList().SetColumnVisible(lcol, vis);
                    if (column_name != this.key_field_name && column_name != "check" && column_name != "absent" && alternativeNameColumn == null) {
                        alternativeNameColumn = column;
                    }
                }
            }
            if (visibleColumnsCount == 0) {
                if (alternativeNameColumn == null) {
                    alternativeNameColumn = dt.Columns[this.key_field_name];
                    this.AddColumn(this.key_field_name, this.key_field_name, null);
                }
                //object lcol = listEdit.GetList().GetColumnByFieldName(alternativeNameColumn.ColumnName);
                //this.listEdit.GetList().SetColumnVisible(lcol, true);
                //visibleColumnsCount++;
            }
            //this.listEdit.EndInitList();
            //this.listEdit.GetList().EndViewUpdate(null);
            this.UpdateFooterPanel();
            if ((this.SourceType == ReturnType.Array)) {
                //this.listEdit.SetMultiselect();
            }
            this.SetData(dt);
            //this.listEdit.ShowConditions(this.rows_limit == 0);
        }
        protected override void ClearSelectionList()
        {
            //IucGrid grid = this.listEdit.GetList();
            //grid.ClearViewContent(null);
            //grid.SetDataSource(null);
            if (this.SourceType == ReturnType.Array) {
                this.ClearSourceValues();
            }
        }
        protected override void ResetSourceMembers()
        {
            this.PrepareList(this.DataTableList);
            this.InitControl();
            this.BindData();
        }
        private Tuple<object, string> foundSimpleValue = new Tuple<object, string>(null, string.Empty);
        public IEnumerable<string> GetSelectedValues(bool getNames)
        {
            IEnumerable<string> selected_values = null;
            string key_field = (getNames) ? name_field_name : key_field_name;
            string name_field = (getNames) && (key_field_name == null || name_field_name != key_field_name) ? "text" : "value";
            // Режим выбора единственной записи
            if (SourceType == ReturnType.Simple) {
                object cur_value = DBNull.Value;
                cur_value = GetSimpleSourceValue();
                if (cur_value == DBNull.Value) {
                    return Array.Empty<string>();
                }
                if (getNames) {
                    var nd = "[недопустимое значение]";
                    var name = GetBoundColumn().GetFieldValueName(UseType);
                    //if (name == null && data_set_default!=null && WebReportsAdapter.IsWebItem(data_set_default.DataSetName))
                    //{
                    //    name = WebReportsAdapter.GetDisplayValue(cur_value, data_set_default);
                    //}
                    if (name == null) {
                        if (Cmn.Nvl(foundSimpleValue.Item1, string.Empty).ToString() == Cmn.Nvl(cur_value, string.Empty).ToString()) {
                            name = foundSimpleValue.Item2;
                        } else {
                            PrepareListSource();
                            if (_need_refresh && this.rows_limit == 0) {
                                ReloadListData();
                                UpdateFooterPanel();
                            }
                            if (DataTableList.HasPrimaryKey()) {
                                var row = DataTableList.Rows.Find(cur_value);
                                if (row == null) {
                                    ReloadListData(true);
                                    UpdateFooterPanel();
                                    _need_refresh = true;
                                    row = DataTableList.Rows.Find(cur_value);
                                }
                                if (row != null) {
                                    name = row[name_field_name].ToString();
                                } else {
                                    name = nd;
                                }
                            }
                        }
                    }
                    if (name == nd) {
                        foundSimpleValue = new Tuple<object, string>(null, string.Empty);
                    } else {
                        foundSimpleValue = new Tuple<object, string>(cur_value, name);
                    }
                    selected_values = new string[1] { name ?? string.Empty };
                }
                else
                {
                    selected_values = new[] { cur_value.ToString() };
                }
            }
            // Обычный режим
            else
            {
                //if (DataSetLocal.IsRefreshed)
                if (!_need_refresh)
                {
                    if (this.auto_check && !String.IsNullOrEmpty(ParentFieldName))
                    {
                        // все записи, у которых нет родительской записи или она не помечена как выбранная



                        //selected_values = listEdit.GetAllNodes()
                        //    .Where(node => (int)node["check"] == 1 && (node.ParentNode == null || (int)node.ParentNode["check"] != 1))
                        //    .Select(node => Cmn.Nvl(node[key_field], "").ToString());

                        var selected_values1 = new List<string>();
                        foreach (DataRow node in this.DataTableList.Rows) {
                            if (Cmn.INT32_ONE.Equals(node["check"])) {
                                DataRow parent = null;
                                if (!Cmn.IsNullOrDBNull(node[this.ParentFieldName])) {
                                    parent = DataTableList.Rows.Find(node[ParentFieldName]);
                                }
                                if (parent == null || !Cmn.INT32_ONE.Equals(parent["check"])) {
                                    selected_values1.Add(Cmn.Nvl(node[key_field], string.Empty).ToString());
                                }
                            }

                        }
                        selected_values = selected_values1.AsEnumerable();
                    }
                    else
                    {
                        //selected_values = listEdit.GetAllNodes()
                        //    .Where(node => (int)node["check"] == 1)
                        //    .Select(node => Cmn.Nvl(node[key_field], "").ToString());


                        // При программной установке значения чрез parameterccontrol, не всегда срабатывало , видимо из-за того что nodes не сразу обновляются
                        // для дерева переделать при необходимости
                        selected_values = DataTableList.AsEnumerable()
                            .Where(r => Cmn.INT32_ONE.Equals(r["check"]))
                            .Select(r => Cmn.Nvl(r[key_field], string.Empty).ToString());
                    }
                } else if (this.array_edit_value != null) {
                    selected_values = this.array_edit_value.AsEnumerable().Select(row => Cmn.Nvl(row[name_field], string.Empty).ToString());
                }
            }
            return selected_values;
        }

        public string FullText { get; set; }
        public void UpdateSelectedString()
        {
            var selected_values = GetSelectedValues(true);
            if (SourceType == ReturnType.Simple)
            {
                //listEdit.SetValue(GetSimpleSourceValue());
            }

            var stext = (selected_values != null) ? String.Join("; ", selected_values) : "";

            this.FullText = stext;
            //if(stext.Length > 1000) stext = stext.Substring(0, 997) + "...";
			if (stext.Length > 250) stext = "Выбрано: " + selected_values.Count();

            //if (this.FieldName == "p_kod_direct1")
            //{
            //}
            //listEdit.SetText(stext);
            this.SetCtrlText(stext);
        }
        public void UpdateFooterPanel()
        {
            if (this.rows_limit > 0) {
                if (DataTableList.Rows.Count >= this.rows_limit) {
                    //listEdit.SetListRowsLimit(this.rows_limit);
                } else {
                    //listEdit.SetListRowsLimit(0);
                }
            }
        }
        //private void UpdateCheckColumnWidth()
        //{
        //    if (tree.Nodes.Count == 0) return;

        //    int step = 20;
        //    int min_width = 50;

        //    int width = 20 + step * (GetAllNodes().Max(n => n.Level) + 1);
        //    tree.Columns["check"].Width = width > min_width ? width : min_width;
        //}
        private void UpdateControlSize()
        {
            object value;
            if (this.UserSettings.TryGetValue("popup_width", out value)) {
                //listEdit.SetPopupWidth((int)value);
            }
            int height;
            if (this.UserSettings.TryGetValue("popup_height", out value)) {
                height = (int)value;
            } else {
                height = 0;
            }
        }
        /*private void ClosePopup()
        {
            listEdit.Close_Popup();
            //if (popupContainerControl.OwnerEdit != null) popupContainerControl.OwnerEdit.ClosePopup();
            //else popupContainerEdit.ClosePopup();
        }*/
        //private void ShowPopup()
        //{
        //    listEdit.Show_Popup();
        //    //popupContainerEdit.ShowPopup();
        //}
        private void ClearFocus()
        {
            //listEdit.ClearListFocus();
        }
        #endregion
        #region Обработчики событий
        public override void DataLocal_RowValueChanged(object sender, DataRowChangeEventArgs e)
        {
            if (this.UseType == UIFormC.UseType.DataEditor) {
                return; // для DataEditor применяется при закрытии списка
            }
            //var dt = sender as VDataTable;
            //if (dt.SuppressChangedEvent) return;
            DataRow row = e.Row;
            object name = row[this.name_field_name];
            var value = new Tuple<object, string, bool>(row[this.value_field_name], Cmn.IsNullOrDBNull(name) ? string.Empty : name.ToString(), Cmn.INT32_ONE.Equals(row["check"]));
            SetSourceValue(value);
        }
        /*private class NodeInfo
        {
            public object Id = null;
            public NodeInfo Parent = null;
            public List<NodeInfo> Childs = new List<NodeInfo>();
            //Dispose ??
        }*/
        //private NodeInfo treeRoot = null;
        //private SortedList<object, NodeInfo> allNodes = null;
        /*private void alnalizeTree()
        {
            if (ParentFieldName == null) return;
            treeRoot = new NodeInfo();
            allNodes = new SortedList<object, NodeInfo>();
            foreach (DataRow row in DataTableList.AsEnumerable())
            {
                NodeInfo ni = null;
                if (allNodes.ContainsKey(row[KeyFieldName]))
                {
                    ni = allNodes[row[KeyFieldName]];
                }
                else
                {
                    ni = new NodeInfo();
                    ni.Id = row[KeyFieldName];
                    allNodes.Add(ni.Id, ni);
                }

                var parId = Cmn.Nvl(row[ParentFieldName], null);
                if (parId == null)
                {
                    treeRoot.Childs.Add(ni);
                    ni.Parent = treeRoot;
                }
                else
                {
                    NodeInfo parNi = null;
                    if (allNodes.ContainsKey(parId))
                    {
                        parNi = allNodes[parId];
                    }
                    else
                    {
                        parNi = new NodeInfo();
                        parNi.Id = parId;
                        allNodes.Add(parNi.Id, parNi);
                        parNi.Childs.Add(ni);
                    }
                }
            }
        }*/
        public void SetFocusIfNeed()
        {
        }
        public void PopupContainerEdit_Closed()
        {
            // чтобы освободить коннекшн если запрос выполняется долго
            if (this.DataTableList.AsyncLoad) {
                this.DataTableList.CancelAsyncExecuteReader();
            }
            this.ApplyValue();
        }
        public bool IsNeedRefresh()
        {
            return this._need_refresh;
        }
        public void LoadList()
        {
            if (this.IsNeedRefresh()) {
                this.refreshList();
            }
        }
        public void LoadListForNames(IEnumerable<string> names)
        {
            this.refreshList(false, names);
        }
        public void LoadValueText()
        {
            this.refreshList(true);
            this._need_refresh = true;
        }
        private void refreshList(bool onlyForselectedValue = false,IEnumerable<string> names=null)
        {
            // для репозиториев
            //if (_custom_buttons)
            //{
            //    e.Cancel = true;// перенес сюда, чтобы не грузился list для customного выбора, посмотрим что получится
            //    //_need_refresh = false;
            //    return;
            //}
            //var edit = (sender as PopupContainerEdit);
            //edit.Properties.PopupControl = listEdit.popupContainerControl;
            ///////////////////////////////////////////////////////////////////////
            BeginUpdate();
            ReloadListData(onlyForselectedValue, names==null, names);

            UpdateFooterPanel();
            if (SourceType == ReturnType.Array)
            {
                ApplyArrayValueToControl();
                _need_set_focus = true;
            }
            EndUpdate();

         

            UpdateTreeView();
        }
        private void UpdateTreeView()
        {
            //var le = listEdit as sql.builder.UI.WinForms.VListEdit;
            //if (le != null) {
            //    le.UpdateTreeView();
            //}
        }
        public void RefreshList()
        {
            this.refreshList();
            //ShowPopup();
        }
		public void ShowPopup()
		{
            //if (listEdit is sql.builder.UI.WinForms.VListEdit)
            //{
            //    (listEdit as sql.builder.UI.WinForms.VListEdit).SetUseCurrent();
            //}
            //this.listEdit.PopupShow();
            if (this.rows_limit > 0 && this.SourceType == ReturnType.Array) {
                //this.listEdit.SetFocusToFilter();
			}
		}
        public void PopupContainerEdit_DeleteValue()
        {
            bool read_only = this.GetSourceReadOnly();
            if (read_only) {
                return;
            }
            if (!this.mandatory) {
                this.ClearSourceValues();
            }
        }
        public Color GetListCellColor(object rowId, string columnName) // временно. вместо этого нужно обсчитывать цвета заранее и отправлять контролу
        {
            DataTable dt = this.DataTableList;
            if (dt == null || !dt.HasPrimaryKey()) {
                return Color.Empty;
            }
            Color color;
            DataRow row = dt.Rows.Find(rowId);
            if (Convert.ToBoolean(row["absent"])) {
                color = Color.DarkRed;
            } else if (this.UseType == UIFormC.UseType.SchemeEditor && this.FieldName == "Color") {
                VColor.ParseRGB((string)row["rgb"], out color);
            } else {
                color = Color.Empty;
            }
            return color;
        }
        public void ProcessFilter()
        {
            if (this.rows_limit > 0) {
                this.StartFiltering(null, null);
            }
        }
        public override void CancelRowsLimit()
        {
            this.rows_limit = int.MaxValue;          
        }
        public void LoadAllListRows()
        {
            this.rows_limit = int.MaxValue;
            //RefreshAsyncMode();
            this.StartFiltering(null, null);
        }
        private void StartFiltering(string column, object value)
        {
        }
        public bool IsServerFilter()
        {
            return this.rows_limit > 0;
        }
        public void Tree_ColumnFilterChanged()
        {
            if (this.rows_limit > 0) {
                if (!this.cancelRefresh) {
                    // при сбросе фильтра через интерфейс
                    //if (_auto_filtering) {
                        //_auto_filtering = false;
                        //if (tree.VisibleColumns.All(c => tree.Nodes.AutoFilterNode.GetDisplayText(c) == ""))
                        //{
                        this.StartFiltering(null, null);
                        //}
                    //}
                }
            }
        }
        private void processAutoFilter()
        {
            //listEdit.BeginUpdate();
            this.ReloadListData();
            if (this.DataTableList.AsyncLoad) {
                //this.listEdit.SetFocusToFilter();
            } else {
                this.processAutoFilterComplete();
            }
        }
        private void processAutoFilterComplete()
        {
            this.UpdateFooterPanel();
            //listEdit.EndUpdate();
            if (this.SourceType == ReturnType.Array) {
                ApplyArrayValueToControl();
            }
            this.UpdateTreeView();
            if (this.rows_limit > 0) {
                //this.listEdit.SetFocusToFilter();
            }
            //_auto_filtering = true;
        }
        private void ClearFilter()
        {
            this.FilterValues.Clear();
            //this.listEdit.ClearFilter();
        }
        public override void ReloadListData(bool onlyForselectedValue = false,bool allowAsync=true,IEnumerable<string> names=null)
        {
           
            
            if (names != null)
            {
                cancelRefresh = true;
                ClearFilter();
                cancelRefresh = false;
            }
            //listEdit.BeginLoadData();
            base.ReloadListData(onlyForselectedValue,allowAsync,names);
            //listEdit.GetList().EndUpdateData();

        }

        protected override void ReloadListComplete()
        {
            base.ReloadListComplete();

            if (DataTableList.AsyncLoad) processAutoFilterComplete();
            //listEdit.EndLoadData();
            prepareListIfNeed();
        }

        protected override void ReloadListCanceled()
        {
            base.ReloadListCanceled();
            if (this.rows_limit > 0)
            {
               EndUpdate();
            }
            //listEdit.EndLoadData();
            prepareListIfNeed();

        }

        private void prepareListIfNeed()
        {
            if (_needPrepareList)
            {
                //listEdit.PrepareList();
                _needPrepareList = false;
               
            }
        }

        public void listEdit_AutoFilterChanged()
        {

            processAutoFilter();

        }

        #endregion
        #region Работа с деревом


        public bool TrySelectValueByName(string text)
        {
            //var tree = (listEdit as sql.builder.UI.WinForms.VListEdit).treeWFTmp;//  пока заплатка
            // нужно решить где будет выполняться поиск с учетом того что в UIList список храниться не будет

            if (_need_refresh && !_hasCustomButtons)// Временное решение - если _custom_buttons то список не обновляется. Пока нет , но может быть _custom_buttons и список- обработать такую ситуацию когда понадобится
            {

                BeginUpdate();
                // tree.BeginUpdate();
                ReloadListData();
                UpdateFooterPanel();
                if (SourceType == ReturnType.Array) ApplyArrayValueToControl();
                //tree.EndUpdate();
               EndUpdate();
                UpdateTreeView();
            }
            return true;
        }
        public void SingeValueSelect(object id)
        {
            if (this.SourceType == ReturnType.Simple) {
                object val;
                if (this.key_field_name == this.value_field_name) {
                    val = id;
                } else {
                    DataRow row = this.DataTableList.Rows.Find(id);
                    val = row[this.value_field_name];
                }
                //this.listEdit.SetValue(val);

                this.SetCtrlValue(val);
                if (id != null) {
                    this.SetSourceValue(val);
                }
                //this.listEdit.Close_Popup();
            }
        }
        #endregion
        #region Override
        public override void Initialize(XElement xfield, UIFormC form)
        {
            ReturnType source_type = (xfield.Attribute(AName.controlType).Value == TextConst.AVControlType.Combo || form.FormUseType == UIFormC.UseType.SchemeEditor) ? ReturnType.Simple : ReturnType.Array;
            string stype = xfield.AttrOrDefault(AName.type, string.Empty);
            Type t;
            //Ильина А. 19.06.2022 
            if (!string.IsNullOrEmpty(stype)) {
                t = Cmn.GetTypeFromStringType(stype, null);
            } else {
                t = null;
            }
            if (source_type == ReturnType.Array) {
                // Режим, когда чек родителя означает, что должны быть чекнуты все потомки
                this.auto_check = xfield.AttrOrDefault(AName.auto_check, false);
                this.clear_on_list_change = xfield.AttrOrDefault(TextConst.AName.ClearOnListChange, false);
                this.expand_all = xfield.AttrOrDefault(AName.expand_all, false);
            }
            base.BaseInitialize(xfield, form, source_type, t, source_type == ReturnType.Simple);
            this.InitControl();
            // Если поставить раньше, то почему-то перезатирается FieldName. Что за магия?
            this.UpdateControlSize();
        }

        public override void RefreshData()
        {
            _need_refresh = true;

            if (this.clear_on_list_change)
            {
                ClearSourceValues();
            }
            if (this.UseDefaultQuery && this.data_set_default != null) {
                XElement master_values = this.OnNeedMasterValues(this);
                this.data_set_default.Refresh(master_values);
                DataRowCollection rows = this.data_set_default.Tables[0].Rows;
                if (this.SourceType == ReturnType.Simple) {
                    if (rows.Count > 0) {
                        SetSourceValue(rows[0][0]);
                    } else if (this.mandatory && !this.Form.WithBehavior) {
                        // Если дефолтное значение по какой-то причине пришло пустым
                        if (this.DataTableList.Rows.Count == 0) {
                            this.ReloadListData();
                        }
                        if (this.DataTableList.Rows.Count > 0) {
                            SetSourceValue(this.DataTableList.Rows[0][0]);
                        }
                    }
                } else if (this.SourceType == ReturnType.SimpleRange) {
                    if (rows.Count > 0) {
                        object value = rows[0][0];
                        SetSourceValue(value, 1);
                        SetSourceValue(value, 2);
                    }
                } else if (this.SourceType == ReturnType.Array) {
                    var values = new List<object>();
                    var names = new List<string>();
                    foreach (DataRow row in rows) {
                        string name = (row.ItemArray.Length > 1 ? row[1].ToString() : row[0].ToString());
                        values.Add(row[0]);
                        names.Add(name);
                    }
                    this.array_edit_value.SuppressChangeEvent();
                    var changes = SetArraySourceValueMultiple(values, names, true);
                    if (changes)
                    {
                        RaiseChanged();
                    }
                }
            } else if (this.mandatory && Form.DefaultParams == null) {
                ReloadListData();
                UpdateFooterPanel();
            }

            if (SourceType == ReturnType.Array) ApplyArrayValueToControl();
            UpdateTreeView();

            base.RefreshData();
        }

        public override string GetText()
        {
            if (UseType==UIFormC.UseType.DataEditor)
            {
                return this.GetBoundColumn().GetFieldValueName();
            }

            return this.GetCtrlText();
        }
        public override IEnumerable<string> GetParamsNames()
        {
            var names = new List<string>();
            if (this.data_set_list != null) {
                names.AddRange(this.data_set_list.GetParNames());
            }
            if (this.data_set_default != null) {
                names.AddRange(this.data_set_default.GetParNames());
            }
            return names.Distinct();
        }
        void rep_QueryPopUp(object sender, CancelEventArgs e)
        {
            //if ((listEdit as sql.builder.UI.WinForms.VListEdit).currentEdit != (sender as PopupContainerEdit))
            //{
            //    e.Cancel = true;
            //}
            //else
            //{

            //}
            PopupContainerEdit_ComboButtonClick_pr();
            //(listEdit as sql.builder.UI.WinForms.VListEdit).PreparePopup((sender as PopupContainerEdit));
            //(listEdit as sql.builder.UI.WinForms.VListEdit).PopUpProcessing(); 
           
        
        }

        bool _needPrepareList = false;
        public void PopupProcessing()
        {
            if (IsNeedRefresh())
            {
                _needPrepareList = true;
                RefreshList();
              
            }
            else
            {
                
              //listEdit.PrepareList();
                ApplyArrayValueToControlDataEditorList();
                // это какая то химия для редактирования данных моногие ко многим через UIList.  Применялось только а тестовых формах, скорее всего уже сломано
                
            }
        }
        // реализация IList.ApplyArrayValueToControlDataEditorList()
        public void ApplyArrayValueToControlDataEditorList()
        {
            if (this.UseType == UIFormC.UseType.DataEditor && this.SourceType == ReturnType.Array) {
                this.ApplyArrayValueToControl();
            }
        }
        //void rep_Closed(object sender, ClosedEventArgs e)
        //{
        //    //throw new NotImplementedException();
        //}


        public override void SetDisplayValue(object text, int index = 1)
        {
            UpdateSelectedString();
        }
        protected override void SaveUserSettings()
        {
        }
        public override void SetError(string text, int index = 1)
        {
            SetErr(text);
            //SetError(popupContainerEdit, text);
            // popupContainerEdit.ErrorText = text;
        }


        protected override void attachTableEvents(VDataTable table)
        {
            base.attachTableEvents(table);
            // table.ColumnTextChanged += VDataTable_OnColumnTextChanged;
        }

        protected override void detachTableEvents(VDataTable table)
        {
            base.detachTableEvents(table);
            // table.ColumnTextChanged -= VDataTable_OnColumnTextChanged;
        }

        public override void ColumnTextChanged(DataRow row)
        {
            if (this.isInGrid && !UIStatic.IsWeb()) return;
            if (row == null) return;
            if (row != ((VDataTable)row.Table).CurrentRow) return;
            // if ((VDataColumn)args.Column != GetBoundColumn()) return;

            SetDisplayValue(null);
        }

        #endregion
        protected override void BeginUpdate()
        {
            //listEdit.GetList().CloseEditor();
            //listEdit.GetList().BeginControlUpdate();
            //listEdit.GetList().DxLockReloadNodes();
        }
        protected override void EndUpdate()
        {
            //listEdit.GetList().DxUnlockReloadNodes();
            //listEdit.GetList().EndControlUpdate();
            //listEdit.GetList().DxEndCurrentEdit();
        }
        public void PopupContainerEdit_Enter()
        {
            Form.LastActiveField = this;
        }
        private DataTable _listData=null;
        private bool _colsInited=false;
        public void SetData(DataTable data)
        {
            
            if (_listData != data)
            {
                 //IVTableDataAdapter vtda = new sql.builder.DataApi.TableDataAccessor.Adapters.VTDADataTable(data);
                //listEdit.GetList().SetDataSource(vtda);
                if (!_colsInited)
                {
                    //listEdit.GetList().InitColumnsEditors();
                    _colsInited = true;
                }
            
            }
        }

        //protected override EditorButtonCollection buttonCollection()
        //{
        //    return (listEdit as sql.builder.UI.WinForms.VListEdit).popupContainerEdit.Properties.Buttons; //временно
        //}
        private bool _hasCustomButtons;

        public bool HasCustomButtons()
        {
            return _hasCustomButtons;
        }
        /*private void popupContainerEdit_ButtonClick(object sender, ButtonPressedEventArgs e)// пока оставил для repository
        {
            //if (e.Button.Caption == "" && e.Button.Kind == ButtonPredefines.Combo)
            //{
            //    PopupContainerEdit_ComboButtonClick_pr();
            //    (listEdit as sql.builder.UI.WinForms.VListEdit).Show_Popup((sender as PopupContainerEdit));
            //}
        }*/
        private void PopupContainerEdit_ComboButtonClick_pr()
        {
            // Емцов - только для комбика? поставил ограничение на SourceType == ReturnType.Simple
            if (SourceType == ReturnType.Simple) {
                //tree.ClearColumnsFilter();
                var col = GetBoundColumn();
                var tbl = col.GetTable();
                var row = tbl.CurrentRow;
                object val = DBNull.Value;
                if (row != null) {
                    val = col.GetValue(row);
                }
                //listEdit.SetValue(val);// химия чтобы выделялась строка с текущим значением в комбике при работе с гридом
            }
        }
        // Реализация IList.GetValFieldName()
        public string GetValFieldName()
        {
            return value_field_name;
        }
        // Реализация IList.GetKeyFieldName()
        public string GetKeyFieldName()
        {
            return key_field_name;
        }
        // Реализация IList.SuppressChangeEvent()
        public void SuppressChangeEvent()
        {
            this.array_edit_value.SuppressChangeEvent();
        }
        // Реализация IList.ResumeChangeEvent()
        public void ResumeChangeEvent()
        {
            this.array_edit_value.ResumeChangeEvent();
        }
        // Реализация IList.RaiseChanged()
        public void RaiseChanged()
        {
            this.Changed();
        }
    }
}