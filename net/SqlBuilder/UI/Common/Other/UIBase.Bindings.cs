using System;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using System.Collections;
using System.Collections.Generic;

using sql.builder.DataApi;
using Devart.Data.Oracle;
//using DevExpress.XtraEditors;
//using DevExpress.XtraEditors.Controls;
//using System.Windows.Forms;
namespace sql.builder.UI
{
    public partial class UIBase
    {
        #region поля
        protected VDataTable array_edit_value;
        public VDataSet data_set_list;
        protected VDataSet data_set_default;
        private DataTable TempTable;
        #endregion
        #region Свойства
        /// <summary>
        /// Таблица для хранения значений массива и их текстового представления для контролов с SourceType == ReturnType.Array. Включается в DataSet формы.
        /// Имеет две колонки: "value" (типа элемента массива) и "text" (строковая).
        /// Колонка "value" устанавливается ключевой, в ней допускаются значения NULL.
        /// Создаётся методом <see cref="UIBase.CreateBoundTable" />.
        /// </summary>
        public VDataTable ArrayEditValue {
            get {
                return this.array_edit_value;
            }
        }
        public VDataTable DataTableList {
            get {
                if (this.data_set_list == null) {
                    return null;
                } else {
                    return this.data_set_list.Tables[0] as VDataTable;
                }
            }
        }
        #endregion
        #region Открытые методы
        public void BindData()
        {
            if (this.SourceType == ReturnType.Simple) {
                this.CreateBoundColumn(this.field_name);
            } else if (this.SourceType == ReturnType.SimpleRange) {
                this.CreateBoundColumn(this.field_name + "1");
                this.CreateBoundColumn(this.field_name + "2");
            } else if (this.SourceType == ReturnType.Array) {
                //if (this is UICustom) {
                //    this.CreateBoundTable(this.field_name);
                //}
                if (!this.Form.WithBehavior) {
                    if (this.DataTableList == null || this.DataTableList.Columns.Count == 0) {
                        return;
                    }
                }
                this.CreateBoundTable(this.field_name);
                if (this.xfield.AttrOrEmpty(AName.type) == TextConst.AVDataType.String) {
                    if (this.xfield.AttrOrDefault(TextConst.AName.IsScalar, false)) {
                        this.array_edit_value.IsArrayParamStringUse = true;
                    }
                }
            }
            // Дополнительные настройки
            this.ShowCheck = (this.UseType == UIFormC.UseType.ParamEditor && this.xfield.AttrOrDefault(AName.show_checkbox, true));
            if (this.mandatory) {
                //this.SetCheckEnabled(false);
                // ceUsed.Enabled = false;
                this.Used = true;
            } else {
                this.Used = !this.ShowCheck;
            }
        }
        public void UpdateControlData()
        {



            if (SourceType == ReturnType.Simple)
            {
                UpdateBaseEditValue(GetBoundColumn(), GetSimpleSourceValue());
            }
            else if (SourceType == ReturnType.SimpleRange)
            {
                if (Form.DataSource.GetTable(this.table_name).CurrentRow == null)
                {
                    return;
                }
                UpdateBaseEditValue(GetBoundColumn(1), GetSimpleSourceValue(1));
                UpdateBaseEditValue(GetBoundColumn(2), GetSimpleSourceValue(2));
            }
            else if (SourceType == ReturnType.Array)
            {
                if (Form.DataSource.GetTable(this.table_name).CurrentRow == null)
                {
                    return;
                }
                UpdateBaseEditValue();
            }


        }


        public void UpdateControlValidation(DataRow row)
        {
            // TODO: если понадобится, реализовать для Range
            if (SourceType == ReturnType.Simple || SourceType == ReturnType.Array)
            {
                var column = GetBoundColumn();
                if (column == null) return;

                bool isCurrentRow = false;

                if (row == null)
                {
                    row = ((VDataTable)column.Table).CurrentRow;
                }
                if (row == ((VDataTable)column.Table).CurrentRow)
                {
                    isCurrentRow = true;
                }

                if (row == null) return;


                var error_text = column.GetValidation(row);

                if (isCurrentRow)
                {
                    SetError(error_text);
                }
            }


        }


        public bool GetVisibilityFromSource(bool useExists=true)
        {
            if (SourceType == ReturnType.Simple || SourceType == ReturnType.Array)
            {
                var column = GetBoundColumn();
                if (column == null) return true;

                var row = ((VDataTable)column.Table).CurrentRow;
                // if (row == null) return true;
                //if (column.ColumnName == "abon_is_filial")
                //{
                //    System.Windows.Forms.MessageBox.Show("1");
                //}
                //if (column.ColumnName == "abon_kpp_po")
                //{
                //    System.Windows.Forms.MessageBox.Show("1");
                //}
                return column.GetVisibility(row,useExists);


            }
            else
            {
                return true;
            }
        }


        public void UpdateControlVisibitity()
        {
            if (SourceType == ReturnType.Simple || SourceType == ReturnType.Array)
            {
                var column = GetBoundColumn();
                if (column == null) return;

                var row = ((VDataTable)column.Table).CurrentRow;
                //if (row == null) return;

                //var vis_cur = column.GetVisibility(row);
                //Form.SetControlVisibility(this, vis_cur);
                //if (column.ColumnName == "kod_folders_isp")
                //{

                //}
                Form.ItemVisibleChanged(this);
            }


        }

        // Параметр временно для редактора схемы
        public void UpdateControlEditable(DataRow row)
        {
            // TODO: если понадобится, реализовать для Range
            if (SourceType == ReturnType.Simple || SourceType == ReturnType.Array)
            {
                var column = GetBoundColumn();
                if (column == null) return;

                bool currentRow = false;

                if (row == null)
                {
                    row = ((VDataTable)column.Table).CurrentRow;
                }
                if (row == ((VDataTable)column.Table).CurrentRow)
                {
                    currentRow = true;
                }
                bool read_only;
                if (row == null)
                {
                    read_only = true;
                    currentRow = true;
                }
                else
                {


                    if (Form.FormUseType == UIFormC.UseType.SchemeEditor)
                    {
                        read_only = !column.GetFieldState().Editable;
                    }
                    else
                    {
                        read_only = !column.GetEditable(row);
                    }
                }
                if (currentRow)
                {
                    //ceUsed1.ReadOnly = read_only; 
                    if (this.GetType().Name != typeof(UIList).Name || read_only)
                    {
                        //SetCheckReadOnly(read_only);
                    }
                    //checkContainer.SetEnabled(!read_only);
                    //if (!UIStatic.IsWeb())
                    //{


                    //}
                }
            }
        }

        public bool HasValue()
        {
            if (this.Form.NoData)
            {
                return false;
            }

            if (SourceType == ReturnType.Simple)
            {
                var val = !GetNullObj().Equals(GetSimpleSourceValue());
                //if(val && Caption.Contains("АСКУЭ"))
                //{

                //}
                return val;
            }
            else if (SourceType == ReturnType.SimpleRange)
            {
                return (GetSimpleSourceValue(1) != DBNull.Value || GetSimpleSourceValue(2) != DBNull.Value);
            }
            else if (SourceType == ReturnType.Array)
            {
                return (this.array_edit_value != null && this.array_edit_value.Rows.Count > 0);
            }
            return false;
        }
        public bool IsVisible()
        {
            if (SourceType == ReturnType.Simple)
            {
                var column = GetBoundColumn();
                if (column == null) return false;

                var row = ((VDataTable)column.Table).CurrentRow;
                if (row == null) return false;

                var vis_cur = column.GetVisibility(row);
                return vis_cur;
            }
            return false;
        }
        #endregion
        #region Закрытые методы
        public void SetSourceValue(object value, int index = 1)
        {
            if (this.SourceType == ReturnType.Simple) {
                if (this.GetSimpleSourceValue() != value) {
                    bool force = (this.key_field_name != this.value_field_name);
                    this.SetSimpleSourceValue(value, this.field_name, force);
                }
            } else if (this.SourceType == ReturnType.SimpleRange) {
                if (this.GetSimpleSourceValue(index) != value) {
                    this.SetSimpleSourceValue(value, this.field_name + index);
                }
            } else if (this.SourceType == ReturnType.Array) {
                this.SetArraySourceValue(value);
            }
        }
        public void ClearSourceValues()
        {
            if (this.SourceType == ReturnType.Simple) {
                this.SetSourceValue(DBNull.Value);
            } else if (SourceType == ReturnType.SimpleRange) {
                this.SetSourceValue(DBNull.Value, 1);
                this.SetSourceValue(DBNull.Value, 2);
            } else if (this.SourceType == ReturnType.Array) {
                if (this.array_edit_value != null) {
                    this.array_edit_value.Rows.Clear();
                }
                this.UpdateSimpleValueForArray();
            }
        }
        private void CreateBoundColumn(string column_name)
        {
            VDataTable table = this.Form.DataSource.GetTable(this.table_name);
            VDataColumn column = (VDataColumn)table.Columns[column_name];
            if (column == null) {
                column = table.AddColumn(column_name, this.ValueType);
            }
            //if (Form.FormUseType == UIFormC.UseType.ParamEditor) // !!! не заработало, потом доделать. 
            //{
            //    VForm.SetColumnProperties(XField, column);
            //    Form.DataSource.AddVariableColumn(FieldName, column);
            //}
            column.BindControl(this);
            //    table.MyColumnChanged += VDataTable_OnColumnChanged;
            this.attachTableEvents(table);
        }
        private void CreateBoundTable(string table_name)
        {
            VDataTable table = null;
            if (this.array_edit_value == null) {
                table = Form.DataSource.GetTable(this.table_name);
                this.array_edit_value = Form.DataSource.GetTable(table_name);
                if (this.array_edit_value != null) {
                    this.array_edit_value.AllowAsyncRefresh = false;
                } else {
                    Type value_type;
                    if (this.Form.WithBehavior) {
                        if (xfield.Attribute(AName.type)==null)
                        {
                            xfield.SetAttributeValue(AName.type, TextConst.AVDataType.Number);
                        }
                        value_type = Cmn.GetTypeFromStringType(this.xfield.Attribute(AName.type).Value, XmlReports.numberType);
                    } else if (this.DataTableList != null) {
                        value_type = this.DataTableList.Columns[value_field_name].DataType;
                    } else {
                        value_type = this.ValueType;
                    }
                    this.array_edit_value = new VDataTable(true);
                    this.array_edit_value.TableName = table_name;
                    VDataColumn pk = this.array_edit_value.AddColumn("value", value_type);
                    this.array_edit_value.AddColumn("text", typeof(string));
                    this.array_edit_value.PrimaryKey = new DataColumn[1] { pk };
                    pk.AllowDBNull = true;
                    this.Form.DataSource.Tables.Add(this.array_edit_value);
                }
                // Поведение заявязано на колонку, поэтому создается еще и фиктивная колонка
                //VDataTable table1 = Form.DataSource.GetTable(this.table_name);
                VDataColumn column = (VDataColumn)table.Columns[table_name]; //table.Columns.Cast<VDataColumn>().FirstOrDefault(e => e.ColumnName == table_name);
                if (column != null) {
                    column.BindControl(this);
                }
                this.array_edit_value.StructureType = StructureType.Array;
                this.array_edit_value.Control = this;
                if (UseType != UIFormC.UseType.DataEditor) {
                    this.array_edit_value.Changed += VDataTable_Changed;
                }
                //column = table.Columns.Cast<VDataColumn>().FirstOrDefault(e => e.ColumnName == table_name);
                if (column != null) {
                    attachTableEvents(table);
                }
            }
            this.array_edit_value.IsArrayEditValue = true;
        }
        protected virtual void attachTableEvents(VDataTable table)
        {
            //table.ColumnEditableChanged += VDataTable_OnColumnEditableChanged;
            //table.ColumnValidChanged += VDataTable_OnColumnValidChanged;
            //table.ColumnVisibleChanged += VDataTable_OnColumnVisibleChanged;
            //table.ColumnSelListChanged += VDataTable_OnColumnSelListChanged;
            //table.ColumnFontColorChanged += VDataTable_OnColumnFontColorChanged;
        }

        protected virtual void detachTableEvents(VDataTable table)
        {
            //table.ColumnEditableChanged -= VDataTable_OnColumnEditableChanged;
            //table.ColumnValidChanged -= VDataTable_OnColumnValidChanged;
            //table.ColumnVisibleChanged -= VDataTable_OnColumnVisibleChanged;
            //table.ColumnSelListChanged -= VDataTable_OnColumnSelListChanged;
        }


        protected void UpdateBaseEditValue(VDataColumn column = null, object value = null)
        {
            if (SourceType == ReturnType.Simple)
            {
                if (column.ColumnName == this.field_name) {
                    SetControlValue(value);
                    SetDisplayValue(column.GetFieldValueName(UseType));

                    //var error = GetSourceError();
                    //SetError(error);

                    bool newUsed = false;

                    if (this is UICheck)
                    {
                        if (value == null || value.ToString() == "0")
                        {
                            newUsed = false;
                        }
                        else
                        {
                            newUsed = true;
                        }
                    }
                    else
                    {
                        if (!Cmn.IsNullOrDBNull(value)) {
                            newUsed = true;
                        } else if (this.null_as_undefined && Cmn.IsNullOrDBNull(value)) {
                            // Емцов - для бытовых поисковиков с range и like в одном поле
                            newUsed = false;
                        }
                    }

                    if (newUsed != Used)
                    {
                        Used = newUsed;
                    }

                    //if (Cmn.Nvl(value, null) != null && !Used) Used = true;
                    //// Емцов - для бытовых поисковиков с range и like в одном поле
                    //else if (NullAsUndefined && Cmn.Nvl(value, null) == null && Used) Used = false;




                }
            }
            else if (SourceType == ReturnType.SimpleRange)
            {
                if (column.ColumnName == this.field_name + "1")
                {
                    SetControlValue(value, 1);
                    SetDisplayValue(column.GetFieldValueName(UseType), 1);

                    //var error = GetSourceError(1);
                    //SetError(error);
                } else if (column.ColumnName == this.field_name + "2") {
                    SetControlValue(value, 2);
                    SetDisplayValue(column.GetFieldValueName(UseType), 2);

                    //var error = GetSourceError(2);
                    //SetError(error);
                }

                if (Cmn.Nvl(value, null) != null && !Used) Used = true;
            }
            else if (SourceType == ReturnType.Array)
            {
                // Только для внешних изменений в таблице (не из контрола)
                if (_need_get_data)
                {
                    ApplyArrayValueToControl();
                    SetDisplayValue(null);
                }

                if (this.special_type == TextConst.AVSpecType.ColSets) {
                    var data = this.array_edit_value.Select().Select(row => row["text"].ToString());
                    OnSpecialTypeChanged("colsets", data);
                }
            }

            //Заплатка для 32274, возможность выбирать отчет в форме для выбора параметров
            if (this.special_type == TextConst.AVSpecType.SelectRep) {
                SpecialTypeChanged(TextConst.AVSpecType.SelectRep, value);
            }
            if (EditValueChanged != null) {
                EditValueChanged(this, EventArgs.Empty);
            }
            if (this.Form.TitleVariables.Contains(this.full_name)) {
                this.Form.UpdateTitle();
            }
        }
        // Simple
        public VDataColumn GetBoundColumn(int index = 1)
        {
            VDataColumn column = null;
            if (SourceType == ReturnType.Simple) {
                column = (VDataColumn)Form.DataSource.GetTable(this.table_name).Columns[this.field_name];
            } else if (SourceType == ReturnType.SimpleRange) {
                column = (VDataColumn)Form.DataSource.GetTable(this.table_name).Columns[this.field_name + index];
            } else if (SourceType == ReturnType.Array) {
                if (Form.DataSource.GetTable(this.table_name).Columns.Contains(this.field_name)) {
                    column = (VDataColumn)Form.DataSource.GetTable(this.table_name).Columns[this.field_name];
                }
            }
            return column;
        }
        public object GetSimpleSourceValue(int index = 1)
        {
            object value = null;
            if (SourceType == ReturnType.Simple)
            {
                var table = Form.DataSource.GetTable(this.table_name);

                // Емцов - с первого раза приходит null?? ТГК 36927 первый параметр не прогружается
                var cur_row = table.CurrentRow;
                cur_row = table.CurrentRow;

                value = (cur_row != null && cur_row.RowState != DataRowState.Deleted) ? cur_row[this.field_name] : DBNull.Value;
            }
            else if (SourceType == ReturnType.SimpleRange)
            {
                var table = Form.DataSource.GetTable(this.table_name);
                var cur_row = table.CurrentRow;
                value = (cur_row != null) ? cur_row[this.field_name + index] : DBNull.Value;
            }

            return value;
        }
        protected void SetSimpleSourceValue(object value, string column_name, bool forceChanges = false)
        {
            var table = Form.DataSource.GetTable(this.table_name);
            DataRow[] rows = null;

            if (Form.FormUseType == UIFormC.UseType.SchemeEditor) //Бельченко: Множественное редактирование строк в редакторе схемы
            {
                if (table.SelectedRows != null)
                {
                    if (table.SelectedRows.Count > 0)
                    {
                        rows = table.SelectedRows.ToArray();
                    }
                }
            }
            if (rows == null)
            {
                rows = new DataRow[] { table.CurrentRow };
            }

            foreach (DataRow row in rows)
            {

                if (value != null && value.ToString() == "")
                {
                    value = DBNull.Value;
                }
                object srcValue = DBNull.Value;
                if (row != null)
                {
                    srcValue = row[column_name];
                }
                value = value ?? DBNull.Value;
                // 19.12.16 Емцов добавил условие на Mandatory, иначе дата устанавливалась в DBNull
                var changed = false;
                if (value == DBNull.Value && !this.mandatory)
                {
                    if (srcValue != DBNull.Value)
                    {
                        changed = (row.Table as VDataTable).GetColumn(column_name).SetValue(row, value);
                        // row[column_name] = value;
                        if (!table.IsBackgroundRefreshProcessing() && !Form.IsRefreshig)
                        {
                            Form.LayoutRefresh();
                        }

                    }
                }
                else
                {
                    bool success = true;
                    if (ValueType == typeof(DateTime))
                    {
                        DateTime date_value;
                        success = DateTime.TryParse(value.ToString(), out date_value);
                        // пока что так: если приходит 01.01.0001 значит значение инвалидное и берем старое
                        if (success && date_value == DateTime.MinValue)
                        {
                            UpdateControlData();
                            return;
                        }

                        value = date_value;
                    }
                    else if (ValueType == typeof(decimal))
                    {
                     
                        if (Cmn.Nvl(value, null) != null)
                        {
                            decimal num_value;
                            success = decimal.TryParse(value.ToString(), out num_value);
                            value = num_value;
                        }
                       
                    }

                    if (success && !value.Equals(srcValue) && row != null)
                    {

                        if (row != null) //!!! Почему то приходит row=null
                        {
                            changed = table.GetColumn(column_name).SetValue(row, value,true);

                           

                            if (!table.IsBackgroundRefreshProcessing() && changed && !Form.IsRefreshig)
                            {
                                Form.LayoutRefresh();
                            }
                            //row[column_name] = value;
                        }
                    }


                }
                if (!changed && forceChanges)
                {
                    table.RaiseColumnChanged(table.GetColumn(column_name), row);
                }
            }
        }
        public  void ReloadListDataIfNeed(bool onlyForselectedValue = false, bool allowAsync = true)
        {
            if (_need_refresh)
            {
                ReloadListData(onlyForselectedValue,allowAsync);
            }

        }

        public void SetNeedRefreshList()
        {
            _need_refresh = true;
        }
        public virtual void CancelRowsLimit()
        {
           
        }


        public virtual void ReloadListData(bool onlyForselectedValue = false,bool allowAsync=true,IEnumerable<string> names=null )
        {
            PrepareListSource();

            if (Form.NoData) return;

            bool disableAsync = false;
            if (UIStatic.IsWeb() ||((onlyForselectedValue || !allowAsync || names!=null) && DataTableList.AsyncLoad))
            {
                DataTableList.AsyncLoad = false;
                disableAsync = true;
            }
            

            _need_refresh = false;
            // Значения по умолчанию должны остаться
            _need_put_data = false;

            SaveTempValues();
            XElement xparams = null;//!!! перенести эту логику в DataSet
            xparams = OnNeedMasterValues(this);
            if (UseType == UIFormC.UseType.DataEditor) {
                xparams = new XElement(TextConst.EName.Params);
                if (this.data_set_list.FactParamsElement != null) { // listquery описан на уровне формы
                    foreach (VSXElement el in this.data_set_list.FactParamsElement.GetElementsP()) {
                        var val = el.GetRuntimeValue(Form.DataSource, null, null);
                        var xpar = new XElement(TextConst.EName.Param);
                        xpar.SetAttributeValue(TextConst.AName.Name, el.P_ParName);

                        var xconst = new XElement(TextConst.EName.Const);
                        xpar.Add(xconst);
                        xconst.Value = Cmn.ToOracleString(val);
                        xparams.Add(xpar);
                        //   DataSetLocal.InputParams[el.P_ParName].Value = val;
                    }
                }
                else  // listquery описан на уровне класса (запроса)
                {

                    foreach (OracleParameter par in this.data_set_list.InputParams.Values)
                    {
                        if (!string.IsNullOrEmpty(par.SourceColumn))
                        {
                            var xpar = new XElement(TextConst.EName.Param);
                            xpar.SetAttributeValue(TextConst.AName.Name, par.ParameterName);
                            var val = Form.DataSource.GetTable(this.table_name).CurrentRow[par.SourceColumn];
                            var xconst = new XElement(TextConst.EName.Const);
                            xpar.Add(xconst);
                            xconst.Value = Cmn.ToOracleString(val);
                            xparams.Add(xpar);
                        }
                    }
                }
            }
            else
            {
                if (Form.FormUseType == UIFormC.UseType.DataEditor)
                {

                    foreach (var m in Masters)
                    {
                        if (m.Value == null)// нет нужного поля с параметром
                        {
                            var col = Form.DataSource.GetVariableColumn(m.Key);
                            object val = null;
                            bool found = false;
                            if (col != null)
                            {
                                val = Form.DataSource.GetVariableValue(m.Key);
                                found = true;
                            }
                            else
                            {
                                if (Form.DataSource.InputParams != null)
                                {
                                    if (Form.DataSource.InputParams.ContainsKey(m.Key))
                                    {
                                        val = Form.DataSource.InputParams[m.Key].Value;
                                        found = true;
                                    }
                                }
                            }
                            if (found)
                            {

                                var xpar = new XElement(TextConst.EName.Param);
                                xpar.SetAttributeValue(TextConst.AName.Name, m.Key);

                                var xconst = new XElement(TextConst.EName.Const);
                                xpar.Add(xconst);
                                xconst.Value = Cmn.ToOracleString(val);
                                xparams.Add(xpar);

                            }
                        }
                    }

                }
            }

            if (this.rows_limit > 0)
            {
                ApplyFilterParams(xparams, onlyForselectedValue,names);
            }

            this.data_set_list.Refresh(xparams);

            if (!DataTableList.AsyncLoad)
            {
                ReloadListComplete();
            }

            

            if (disableAsync)
            {
                DataTableList.AsyncLoad = true;
            }
        }
        protected virtual void ReloadListComplete()
        {
            LoadTempValues();

            if (this.mandatory && DataTableList.Rows.Count > 0) {
                if (SourceType == ReturnType.Simple) {
                    if (GetSimpleSourceValue() == DBNull.Value) SetSourceValue(DataTableList.Rows[0][0]);
                }
                else if (SourceType == ReturnType.SimpleRange)
                {
                    if (GetSimpleSourceValue(1) == DBNull.Value) SetSourceValue(DataTableList.Rows[0][0], 1);
                    if (GetSimpleSourceValue(2) == DBNull.Value) SetSourceValue(DataTableList.Rows[0][0], 2);
                }
            }

            _need_put_data = true;
        }
        protected virtual void ReloadListCanceled()
        {
            //TempTable.Clear();
            //_need_put_data = true;
        }
        private void VDataTable_OnAsyncLoadComplete(object sender, EventArgs args)
        {
            this.ReloadListComplete();
        }
        private void VDataTable_OnAsyncLoadCanceled(object sender, EventArgs args)
        {
            this.ReloadListCanceled();
        }
        private DataColumn getArrayEditValueKeyColumn()
        {
            VDataTable dt = this.array_edit_value;
            string column_name = dt.ArrayEditValueRefColumn;
            if (column_name != null) {
                return dt.Columns[column_name];
            } else {
                return dt.Columns[0];
            }
        }
        public void ApplyArrayValueToControl()
        {
            if (this.Form.NoData) {
                return;
            }
            //PrepareListSource();
            if (this.UseType == UIFormC.UseType.DataEditor) {
                this.array_edit_value.Refresh();
            }
            // обновляем таблицу контрола значениями из главной

            //TODO: костыль для web
            if (this.array_edit_value == null) return;
            IList<DataRow> rows = this.array_edit_value.GetRowsForCurrentParent();
            HashSet<object> keys = new HashSet<object>();
            int index;
            DataRow row;
            DataColumn key_column = this.getArrayEditValueKeyColumn();
            for (index = 0; index < rows.Count; index++) {
                row = rows[index];
                if (row.RowState != DataRowState.Deleted) {
                    object key = row[key_column];
                    if (!keys.Contains(key)) {
                        keys.Add(key);
                    }
                }
            }
            //UICustom custom = this as UICustom;
            //if (custom != null) {
            //    object[] arr = new object[keys.Count];
            //    keys.CopyTo(arr, 0);
            //    custom.SetControlValue(arr);
            //} else {
                VDataTable dt = this.DataTableList;
                if (dt != null) {
                    key_column = dt.Columns[this.value_field_name];
                    DataColumn check_column = dt.Columns["check"];
                    this.BeginUpdate();
                    for (index = 0; index < dt.Rows.Count; index++) {
                        row = dt.Rows[index];
                        int old_val = (int)row[check_column];
                        if (old_val != (keys.Contains(row[key_column]) ? 1 : 0)) {
                            // инвертируем значение
                            object new_val = (old_val == 0) ? Cmn.INT32_ONE : Cmn.INT32_ZERO;
                            row[check_column] = new_val;
                        }
                    }
                    this.EndUpdate();
                }
            //}
        }
        private void addArrayEditValue(object key, string name)
        {
            VDataTable dt = this.array_edit_value;
            DataRow row;
            if (this.UseType == UIFormC.UseType.DataEditor) {
                DataColumn col = this.getArrayEditValueKeyColumn();
                row = dt.NewRow();
                row[col] = key;
                dt.Rows.Add(row);
            } else {
                row = dt.Rows.Find(key);
                if (row == null) {
                    dt.Rows.Add(key, name);
				}
            }
        }
        public bool SetArraySourceValueMultiple(IList<object> values, IList<string> names, bool resumeChange)
        {
            string[] svalue = new string[values.Count];
            List<object> value1 = new List<object>(values.Count);
            int index;
            for (index = 0; index < values.Count; index++) {
                object value = values[index];
                svalue[index] = value.ToString();
                value1.Add(value);
            }
            var rows = this.array_edit_value.GetRowsForCurrentParent().Where(r => r.RowState != DataRowState.Deleted).ToList();
            bool changes = false;
            for (index = 0; index < rows.Count; index++) {
                DataRow r = rows[index];
                if (!svalue.Contains(r[getArrayEditValueKeyColumn()].ToString())) {
                    //ArrayEditValue.DeleteRow(r);
                    r.Delete();
                    changes = true;
                } else {
                    value1.Remove(r[getArrayEditValueKeyColumn()]);
                }
            }
            this.array_edit_value.Changed -= VDataTable_Changed;
            if (value1.Count > 0) {
                Dictionary<object, string> valNames;
                if (names != null) {
                    valNames = new Dictionary<object, string>();
                    for (index = 0; index < values.Count; index++) {
                        valNames.Add(values[index], names[index]);
                    }
                } else {
                    valNames = null;
                }
                for (index = 0; index < value1.Count; index++) {
                    object v = value1[index];
                    string valName;
                    if (valNames != null) {
                        valName = valNames[v];
                    } else {
                        valName = null;
                    }
                    addArrayEditValue(v, valName);
                    changes = true;
                }
            }
            this.array_edit_value.Changed += VDataTable_Changed;
            if (resumeChange) { // костыль чтобы не повторять то что ниже
                this.array_edit_value.ResumeChangeEvent();
            }
            UpdateBaseEditValue();
            if (changes) {
                UpdateSimpleValueForArray();
                VDataTable dt = this.array_edit_value.GetParentTable();
                if (dt != null) {
                    dt.RaiseUserChangedData(null, null);
                }
            }
            if (resumeChange) {
                Changed();
            }
            return changes;
        }
        //public void SetArraySourceValue(object[] values)
        //{
        //    ClearSourceValues();
        //    foreach (var value in values)
        //    {
        //        SetArraySourceValue(new Tuple<object, string, bool>(value, "", true));
        //    }
        //}
        public object[] GetArraySourceValue()
        {
            var rows = this.array_edit_value.GetRowsForCurrentParent().Where(r => r.RowState != DataRowState.Deleted).ToArray();
            object[] values = rows.Select(r => r[getArrayEditValueKeyColumn()]);
            return values;
        }
        protected void UpdateSimpleValueForArray()
        {
            if (Form.FormUseType == UIFormC.UseType.DataEditor) {
                DataRow row = this.array_edit_value.AsEnumerable().FirstOrDefault(r => r.RowState != DataRowState.Deleted);
                if (row != null) {
                    MarkUsed(true);
                    SetSimpleSourceValue(Cmn.DECIMAL_ONE, this.field_name, true);
                } else {
                    MarkUsed(false);
                    SetSimpleSourceValue(DBNull.Value, this.field_name, true);
                }
            }
        }
        private void SetArraySourceValue(object value)
        {
            var array_value = (Tuple<object, string, bool>)value;

            var item_value = array_value.Item1;
            var item_name = array_value.Item2;
            var item_checked = array_value.Item3;

            if (!_need_put_data) return;

            // если быстро щелкать, можно ненароком вызвать событие DataSource_Changed
            bool tmp = _need_get_data;
            _need_get_data = false;

            DataRow source_row = null;
            //if (UseType == UIFormC.UseType.DataEditor)
            //{
            //    //индексировать
            //    source_row = ArrayEditValue.AsEnumerable().Where(r =>
            //         r.RowState != DataRowState.Deleted &&
            //        r[getArrayEditValueKeyColumn()].ToString() == item_value.ToString()).FirstOrDefault();
            //}
            //else
            //{
            //ArrayEditValue.SuppressChangeEvent();
            source_row = this.array_edit_value.Rows.Find(item_value);
            //}
            if (item_checked && source_row == null)
            {
                try
                {
                    addArrayEditValue(item_value, item_name);
                    //ArrayEditValue.Rows.Add(item_value, item_name);
                }
                catch
                {

                }

            }
            else if (!item_checked && source_row != null)
            {
                source_row.Delete();
            }


           // ArrayEditValue.ResumeChangeEvent();
            if (Form.FormUseType == UIFormC.UseType.DataEditor) // чтобы обрабатывалась валидация обязательных полей
            {
                //if (this.UseType != UIFormC.UseType.DataEditor)
                //{
                UpdateSimpleValueForArray();
                //}
            }
       
          //  Changed();
            _need_get_data = tmp;
        }
        private void SaveTempValues()
        {
            if (this.TempTable == null) {
                // Clone() не работает - пока так копируем DataTable
                // TODO: переопределить Clone() для VDataTable
                this.TempTable = new DataTable();
                // копируем колонки
                for (int index = 0; index < this.DataTableList.Columns.Count; index++) {
                    DataColumn col_src = this.DataTableList.Columns[index];
                    DataColumn col_desc = new DataColumn(col_src.ColumnName, col_src.DataType);
                    col_desc.AllowDBNull = col_src.AllowDBNull;
                    this.TempTable.Columns.Add(col_desc);
                }
                // копируем primary key
                Cmn.CopyPrimaryKey(this.DataTableList, this.TempTable);
                if (this.show_nulls) {
                    foreach (DataColumn pk in this.TempTable.PrimaryKey) {
                        pk.AllowDBNull = true;
                    }
                }
            }

            TempTable.Clear();
            // Сохраняем выбранные значения во временной таблице

            Dictionary<object, string> vals = new Dictionary<object, string>();


            if (SourceType == ReturnType.Simple)
            {
                if (UseType == UIFormC.UseType.DataEditor && this.rows_limit != 0)
                {
                    var value = GetSimpleSourceValue();
                    if (value != DBNull.Value)
                    {
                        vals.Add(value, GetText());
                    }
                }
            } else {
                foreach (DataRow row in this.array_edit_value.Rows) {
                    vals.Add(row[0], row[1].ToString());
                }
            }
            int tempId = -999999;
            foreach (var val in vals)
            {
                tempId--;
                DataRow row = null;
                if (key_field_name == value_field_name && DataTableList.HasPrimaryKey())
                {

                    row = DataTableList.Rows.Find(val.Key);


                }
                else
                {
                    /* заплатка , но по нормальному переделать слишком сложно получается*/
                    row = DataTableList.AsEnumerable().FirstOrDefault(r => r[value_field_name] == val.Key);
                }

                if (row != null)
                {
                    row["absent"] = (row[key_field_name] != DBNull.Value);
                    TempTable.ImportRow(row);
                }
                else
                {
                    row = TempTable.NewRow();
                    if (key_field_name != value_field_name)
                    {
                        /* заплатка , но по нормальному переделать слишком сложно получается*/
                        row[key_field_name] = tempId;
                    }
                    row[value_field_name] = val.Key;

                    object val1 = val.Value;
                    if (Cmn.Nvle(val.Value, null) != null)
                    {
                        row[name_field_name] = val1;
                    }

                    //.ToString();// бельченко 13.07.2017
                    row["check"] = 1;
                    row["absent"] = (row[key_field_name] != DBNull.Value);
                    TempTable.Rows.Add(row);
                }

            }
        }
        private void LoadTempValues()
        {
            if (SourceType == ReturnType.Array || (SourceType == ReturnType.Simple && UseType == UIFormC.UseType.DataEditor && this.rows_limit != 0))
            {
                if (TempTable.Columns.Count == 0) return;

                var value_keys = DataTableList.Rows.Cast<DataRow>().Select(row => row[value_field_name]);//заменил KeyFieldName на ValFieldName т.к. key не сохраняется

                var temp_rows_delete = TempTable.AsEnumerable().Where(row => value_keys.Contains(row[value_field_name])).ToList();
                foreach (var temp_row in temp_rows_delete)
                {
                    temp_row.Delete();
                }


                TempTable.AcceptChanges();


                foreach (DataColumn col in DataTableList.Columns) // Без этого возникала ошибка "В столбце Column  превышено ограничение, которое задает MaxLength.". Вроде сброс MaxLength помог
                {
                    if (col.DataType == typeof(string))
                    {
                        col.MaxLength = -1;
                    }
                }
                // Добавляем ранее сохраненные выбранные записи в новый список
                DataTableList.Merge(TempTable);
            }

        }
        #endregion
        #region Virtual
        public virtual void SetControlValue(object value, int index = 1)
        {
            //
        }
        public virtual void SetDisplayValue(object text, int index = 1)
        {
            //
        }
        public virtual void SetError(string text, int index = 1)
        {
            //
        }

        //public  void SetError(ButtonEdit ctrl, string text)
        //{
        //    ctrl.ErrorText = text;
        //    ctrl.ErrorIcon = Cmn.ImageWarning14;
        //    ctrl.ErrorIconAlignment = ErrorIconAlignment.MiddleLeft;
        //}
        string _errorText = "";
        string _warningText = "";
        public void SetErr(string text)
        {

            var ctext = VDataTable.ClearValidationMessage(text);
            _warningText = "";

            if (VDataTable.IsValidationMessgeNeedAlert(text))
            {
                _warningText = ctext;
            }
            else
            {
                //checkContainer.SetError(ctext);
            }
            _errorText = text;
            //var ctrl = (Cmn.GetChildControlsOfType < ButtonEdit>(checkContainer as sql.builder.UI.WinForms.VCheckContainer)).FirstOrDefault(); // временно
            ////if (ctrl != null)
            ////{
            //    ctrl.ErrorText = text;
            //    ctrl.ErrorIcon = Cmn.ImageWarning14;
            //    ctrl.ErrorIconAlignment = ErrorIconAlignment.MiddleLeft;
            ////}

        }

        #endregion
        #region Обработчики событий
        // Simple

        public void ColumnChanged(DataRow row)
        {
            if (this.isInGrid) return;
            var column = this.GetBoundColumn();
            var curRow = ((VDataTable)column.Table).CurrentRow;
            if (curRow != row) return;


            // if (column != GetBoundColumn()) return;

            UpdateBaseEditValue(column, column.GetValue(row));
        }
        public void ColumnEditableChanged(DataRow row)
        {

            // if ((VDataColumn)args.Column != GetBoundColumn()) return;

            UpdateControlEditable(row); // !!! переделать также для остальных свойств по необходимости
        }
        public void ColumnValidChanged(DataRow row)
        {
            // if (args.Row != ((VDataTable)args.Column.Table).CurrentRow) return;
            // if ((VDataColumn)args.Column != GetBoundColumn()) return;

            UpdateControlValidation(row);// !!! переделать также для остальных свойств по необходимости
        }




        public void ColumnVisibleChanged(DataRow row)
        {
            //if (row == null) return;
            // if (row != ((VDataTable)row.Table).CurrentRow) return;
            if (row != GetBoundColumn().GetTable().CurrentRow) return;
            //if ((VDataColumn)args.Column != GetBoundColumn()) return;

            UpdateControlVisibitity();
        }

        public void ColumnSelListChanged(DataRow row)
        {
            if (row == null) return;
            if (row != ((VDataTable)row.Table).CurrentRow) return;
            // if ((VDataColumn)args.Column != GetBoundColumn()) return;
            RefreshData();
            //UpdateControlEditable();
        }
        public virtual void ColumnTextChanged(DataRow row)
        {
        }
        public void ColumnFontColorChanged(DataRow row)
        {
            //if (args.Row != ((VDataTable)args.Column.Table).CurrentRow) return;
            // if ((VDataColumn)args.Column != GetBoundColumn()) return;


        }
        private void VDataTable_Changed(object sender, EventArgs e)
        {
            //if (UseType != UIFormC.UseType.DataEditor)
            //{
            UpdateBaseEditValue();
            //}
        }
        #endregion
    }
}
