using System;
using Contract = System.Diagnostics.Contracts.Contract;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using sql.builder.FieldInfo;
using sql.builder.UI;
using SqlBuilderLib.DevTools;

namespace sql.builder.DataApi
{
    public partial class VDataColumn : DataColumn
    {
        public static VDataColumn Create(XElement scheme)
        {
            Contract.Assert(scheme != null);
            Contract.Assert(scheme.Name == EName.column);
            string column_name = scheme.Attribute(AName.name).Value;
            Type data_type = Cmn.GetTypeFromStringType(scheme.Attribute(AName.type).Value, typeof(string));
            string caption;
            XAttribute attr = scheme.Attribute(AName.title);
            if (attr != null && (!string.IsNullOrEmpty(attr.Value)) && scheme.Attribute("hidden") == null)
            {
                caption = attr.Value;
            }
            else
            {
                caption = column_name;  // временное решение, не показываются колонки у которых заголовок равен имени (считаем что нет заголовка)
            }
            VDataColumn column;
            if (data_type == typeof(string) && (column_name == TextConst.AVSpecColumnGrset.GrSetTitle ||
                                                column_name == TextConst.AVSpecColumnGrset.GrSetName ||
                                                column_name == TextConst.AVSpecColumnGrset.OrigGrSetName ||
                                                column_name == TextConst.AVSpecColumnGrset.ParentGrSetId ||
                                                scheme.AttrOrDefault(AName.intern, false)))
            {
                column = new VInternedStringDataColumn(column_name, caption);
            }
            else
            {
                column = new VDataColumn(column_name, data_type, caption);
            }
            column.scheme = scheme;
            return column;
        }
        #region поля
        private bool editableOld;
        private XElement scheme;
        public string DbColumnName;
        public string TempColumnName;
        public bool Visible;
        public bool IsHtml;
        private VDataSet selectionList;
        private List<UIBase> _bound_controls;
        #endregion
        /// <summary>
        /// Создаёт колонку типа <paramref name="type"/> с наименованием <paramref name="column_name"/> и заголовком <paramref name="caption"/>
        /// </summary>
        /// <param name="column_name">наименование колонки</param>
        /// <param name="type">тип данных колонки</param>
        /// <param name="caption">заголовок</param>
        public VDataColumn(string column_name, Type type, string caption)
            : this(column_name, type)
        {
            this.Caption = caption;
        }
        /// <summary>
        /// Создаёт колонку с наименованием <paramref name="column_name"/> типа <paramref name="type"/>
        /// </summary>
        /// <param name="column_name">наименование колонки</param>
        /// <param name="type">тип данных колонки</param>
        public VDataColumn(string column_name, Type type)
            : base(column_name, type)
        {
            this._bound_controls = new List<UIBase>();
        }
        /// <summary>
        /// Создаёт строковую колонку с наименованием <paramref name="column_name"/>
        /// </summary>
        /// <param name="column_name">наименование колонки</param>
        public VDataColumn(string column_name)
            : this(column_name, typeof(string))
        {
        }
        #region свойства
        public new Type DataType { get { return base.DataType; } }
        public XElement Scheme { get { return this.scheme; } }
        public bool EditableOld
        {
            get
            {
                return this.editableOld;
            }
            set
            {
                this.editableOld = value;
                if (this.scheme != null)
                {
                    this.scheme.SetAttrValue(AName.editable, value);
                    XElement viewcolumn = this.GetViewColumn();
                    if (viewcolumn != null && viewcolumn.Attribute(AName.editable) == null)
                    {
                        viewcolumn.SetAttrValue(AName.editable, value);
                    }
                }
            }
        }
        public VDataSet SelectionList
        {
            get
            {
                return this.selectionList;
            }
            set
            {
                this.selectionList = value;
            }
        }
        #endregion
        public VDataTable GetTable()
        {
            return this.Table as VDataTable;
        }
        public object GetValue(DataRow row)
        {
            if (row.RowState == DataRowState.Deleted)
            {
                return row[this, DataRowVersion.Original];
            }
            else
            {
                return row[this];
            }
        }
        public bool SetValue(DataRow row, object value, bool isUser = false)
        {
            if (row.RowState == DataRowState.Deleted)
            {
                return false;
            }
            VDataTable table = this.GetTable();
            if (table.GetDataSet().IsColumnChanging(this))
            {
                return false;
            }
            if (Cmn.IsNullOrDBNull(row[this]) && Cmn.IsNullOrDBNull(value))
            {
                return false;
            }
            if (this._bound_controls.Count > 0 && this._bound_controls[0] is UICheck && Cmn.ToDecimal(row[this]) == Cmn.ToDecimal(value))
            {
                return false;
            }
            if (Cmn.IsNullOrDBNull(row[this]))
            {
                table.SuppressChangeEvent();

                // костыль для web (анализ отчетов)
                if (DevUtilsProvider.Instance.IsAnalyzerEnabled())
                {
                    if (value.GetType() != this.DataType)
                    {
                        if (this.DataType == typeof(decimal))
                        {
                            value = 0m;
                        }
                        else if (this.DataType == typeof(DateTime))
                        {
                            value = DateTime.Now;
                        }
                    }
                }


                row[this] = value;// почему то событие columnchange срабатывает дважды , сделал отмену и вызов вручную
                table.ResumeChangeEvent();
                if (isUser)
                {
                    var args = new DataColumnChangeEventArgs(row, this, value);
                    table.RaiseUserChangedData(this, args);
                }
                table.RaiseColumnChanged(this, row);
                return true;
            }
            else
            {
                if (value.GetType() == typeof(double))
                {
                    value = Convert.ToDecimal(value);
                }
                if (!row[this].Equals(value))
                {
                    table.SuppressChangeEvent();
                    row[this] = value;// почему то событие columnchange срабатывает дважды , сделал отмену и вызов вручную
                    table.ResumeChangeEvent();
                    if (isUser)
                    {
                        var args = new DataColumnChangeEventArgs(row, this, value);
                        table.RaiseUserChangedData(this, args);
                    }
                    table.RaiseColumnChanged(this, row);
                    return true;
                }
            }
            return false;
        }
        private XElement GetViewColumn()
        {
            if (this.scheme != null)
            {
                return this.scheme.Parent.Parent.Elements(EName.viewcolumns).Descendants(EName.column).SearchByAttribute(AName.name, this.scheme.Attribute(AName.name).Value);
            }
            else
            {
                return null;
            }
        }
        /*public void MakeAttributes()
        {
            XElement viewcolumn = this.GetViewColumn();
            string editor;
            if (this.scheme == null) {
                editor = string.Empty;
            } else {
                editor = this.scheme.AttrOrDefault("editor", string.Empty);
            }
            if (editor == string.Empty) {
                editor = ((VDataSet)(this.Table.DataSet)).Report.GetEnvironment().GetVDataType(this.scheme.Attribute(AName.DataType).Value).EditorName();
                if (this.scheme != null) {
                    this.scheme.SetAttrValue("editor", editor);
                }
            }
            if (viewcolumn != null && viewcolumn.Attribute("editor") == null) {
                viewcolumn.SetAttrValue("editor", editor);
            }
        }*/
        #region Динамические свойства поля для создания интерфейса
        public VFieldStateAndOtherInfo GetFieldState()
        {
            return this.GetFieldInfo(new[] { VFieldInfo.InfoTypesToGet.State });
        }
        public string GetFieldValueName(UIFormC.UseType useType)
        {
            string s = null;
            switch (useType)
            {
                case UIFormC.UseType.SchemeEditor:
                    s = this.GetFieldInfo(new[] { VFieldInfo.InfoTypesToGet.ValueName }).ValueName;
                    break;
                case UIFormC.UseType.DataEditor:
                    s = this.GetFieldValueName((Table as VDataTable).CurrentRow);
                    break;
                default:
                    s = null;
                    break;
            }
            return s;
        }
        public string GetFieldValueName(DataRow row = null)
        {
            string s = null;
            if (row == null)
            {
                row = this.GetTable().CurrentRow;
            }
            if (row != null)
            {
                if (this.TextSourceSource != null)
                {
                    object o = row[this.TextSourceSource];
                    if (!Cmn.IsNullOrDBNull(o))
                    {
                        s = o.ToString();
                    }
                    else
                    {
                        s = string.Empty;
                    }
                }
                else
                {
                    s = string.Empty;
                }
            }
            return s;
        }
        public void SetFieldValueName(string name)
        {
            DataRow row = this.GetTable().CurrentRow;
            if (row != null)
            {
                if (string.IsNullOrEmpty(name))
                {
                    row[this.TextSourceSource] = DBNull.Value;
                }
                else
                {
                    row[this.TextSourceSource] = name;
                }
            }
        }
        //public void SetFieldValueName(string value)
        //{
        //    (Table as VDataTable).CurrentRow[ColumnName + TextConst.Pfx.ExtValName] = value;
        //}
        private VFieldStateAndOtherInfo GetFieldInfo(VFieldInfo.InfoTypesToGet[] getWhat)
        {
            VDataTable dt = (VDataTable)this.Table;
            if (dt.CustomFieldInfoProc != null)
            {
                return dt.CustomFieldInfoProc(this, dt.CurrentRow, getWhat);
            }
            else
            {
                return null;
            }
        }
        #endregion
        #region Binding + Validation
        public List<UIBase> BoundControls
        {
            get { return this._bound_controls; }
        }
        public void BindControl(UIBase ctrl)
        {
            if (!this._bound_controls.Contains(ctrl))
            {
                _bound_controls.Add(ctrl);
                this.GetTable().HasControls = true;
            }
        }
        public void UnbindControl(UIBase ctrl)
        {
            if (this._bound_controls.Contains(ctrl))
            {
                this._bound_controls.Remove(ctrl);
            }
        }
        //public void SetCellError(DataRow row, string error_text)
        //{
        //    row.SetColumnError(this, error_text);
        //}
        //public string GetCellError(DataRow row)
        //{
        //    if (row == null) return null;
        //    return row.GetColumnError(this);
        //}
        // static, чтобы можно было использовать как условный предикат: Where(VDataColumn.HasBoundControl)
        public static bool HasBoundControl(VDataColumn col)
        {
            return !List.IsNullOrEmpty(col._bound_controls);
        }
        #endregion
    }
    public class VInternedStringDataColumn : VDataColumn
    {
        /// <summary>
        /// Создаёт колонку типа <paramref name="type"/> с наименованием <paramref name="column_name"/> и заголовком <paramref name="caption"/>
        /// </summary>
        /// <param name="column_name">наименование колонки</param>
        /// <param name="caption">заголовок</param>
        public VInternedStringDataColumn(string column_name, string caption)
            : base(column_name, typeof(string), caption)
        {
        }
        /// <summary>
        /// Создаёт колонку с наименованием <paramref name="column_name"/> типа <paramref name="type"/>
        /// </summary>
        /// <param name="column_name">наименование колонки</param>
        public VInternedStringDataColumn(string column_name)
            : base(column_name, typeof(string))
        {
        }
    }
}