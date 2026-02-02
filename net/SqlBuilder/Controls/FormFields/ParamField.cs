using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using sql.builder.DataApi;
using sql.builder.UI;
using sql.builder.WinForms;

namespace sql.builder.Controls.FormFields
{
    /// <summary>
    /// Класс для управления полями формы
    /// </summary>
    public abstract class ParamField
    {
        internal static ParamField Create(UIBase control)
        {
            UIList list = control as UIList;
            if (list != null) {
                return new ListField(list);
            } else {
                return new ScalarField(control);
            }
        }
        internal/* protected */ abstract UIBase Control { get; }
        /// <summary>
        /// Возвращает тип поля
        /// </summary>
        /// <returns>тип поля</returns>
        public Type GetValueType()
        {
            return this.Control.ValueType;
        }
        /// <summary>
        /// Проверяет тип поля
        /// </summary>
        /// <returns>Возвращает true для полей типа array иначе - false.</returns>
        public bool IsArray()
        {
            return this.Control.SourceType == ReturnType.Array;
        }
        public abstract void LoadList();
        public abstract void LoadValueText();
        public abstract void ShowPopupList();
        /// <summary>
        /// Устанавливает значение поля
        /// </summary>
        /// <param name="val">значение поля</param>
        public void SetValue(object val)
        {
            UIBase control = this.Control;
            if (control.SourceType == ReturnType.Array) {
                if (Cmn.IsNullOrDBNull(val)) {
                    this.ClearValue();
                } else {
                    IEnumerable e = val as IEnumerable;
                    if (e == null) {
                        throw new ArgumentException(string.Format("Значение имеет недопустимый тип {0}. Ожидается тип {1}", val.GetType(), GetValueType()));
                    }
                    IList<object> list = val as IList<object>;
                    if (list == null) {
                        list = new List<object>();
                        foreach (object o in e) {
                            list.Add(o);
                        }
                    }

                    //TODO: костыль для web
                    if (control.ArrayEditValue != null)
                    {
                        control.ArrayEditValue.SuppressChangeEvent();
                        control.SetArraySourceValueMultiple(list, null, true);

                    }
                   
                    // _control.SetArraySourceValue((val as IEnumerable).ConvertAll(v => v).ToArray());   
                }
            } else {
                if (val == DBNull.Value) {
                    val = null;
                } else if (val != null && val.GetType() != this.GetValueType()) {

                    if (this.GetValueType() == typeof(DateTime) && val.Equals(0m))
                    {
                        // TODO: еще один костыль для предвариательной обработки. См. DevAnalyzer.cs
                        val = DateTime.Now;

                    }
                    //val = null;
                    //throw new ArgumentException(string.Format("Значение имеет недопустимый тип {0}. Ожидается тип {1}", val.GetType(), this.GetValueType()));
                }
                control.SetSourceValue(val);   
            }
        }
        /// <summary>
        /// Возвращает значение поля
        /// </summary>
        /// <returns>значение поля</returns>
        public object GetValue()
        {
            UIBase control = this.Control;
            if (this.IsArray()) {
                return control.GetArraySourceValue();
            } else {
                object val = control.GetSimpleSourceValue();
                return (val != DBNull.Value ? val : null);
            }
        }
        /// <summary>
        /// Возвращает текстовое представление значения поля.
        /// Для скалярных полей тип возвращаемого значения string,
        /// для полей типа array - string[]
        /// </summary>
        /// <returns>Текстовое представление значения поля</returns>
        public abstract object GetValueNames();
        /// <summary>
        /// Устанавливает заголовок (метку) поля
        /// </summary>
        /// <param name="value">заголовок (метка) поля</param>
        public void SetTitle(string value)
        {
            throw new NotImplementedException();
            //UIBase control = this.Control;
            //VLayoutLabelInfo label = control.Form.Layout.GetItemByControl(control.GetRootControl()).label;
            //if (label != null) {
            //    label.SetText(value);
            //    control.Caption = value;
            //    control.GetBoundColumn().Caption = value;
            //}
        }
        /// <summary>
        /// Очищает значение поля
        /// </summary>
        public void ClearValue()
        {
            UIBase control = this.Control;
            control.Used = false;
            control.ClearSourceValues();
        }
        public bool IsUsed()
        {
            return this.Control.Used;
        }
    }
    internal sealed class ScalarField : ParamField
    {
        private UIBase control;
        internal ScalarField(UIBase control)
        {
                 this.control = control;
        }
        internal/* protected */ override UIBase Control { get { return this.control; } }
        public override void LoadList()
        {
        }
        public override void LoadValueText()
		{
		}
		public override void ShowPopupList()
		{
		}
        public override object GetValueNames()
        {
            return this.control.GetText();
        }
    }
    internal sealed class ListField : ParamField
    {
        private UIList list;
        internal ListField(UIList list)
        {
            this.list = list;
        }
        internal/* protected */ override UIBase Control { get { return this.list; } }
        public override void LoadList()
        {
            this.list.LoadList();
        }
        public override void LoadValueText()
		{
            if (this.list.RowsLimit > 0) {
                this.list.LoadValueText();
            } else {
                this.list.LoadList();
            }
		}
		public override void ShowPopupList()
		{
            this.list.ShowPopup();
		}
        public override object GetValueNames()
        {
            IEnumerable<string> e = this.list.GetSelectedValues(true);
            string[] arr = e as string[];
            if (arr == null) {
                arr = e.ToArray<string>();
            }
            return arr;
        }
        internal void SetValueByNames(string[] values)
        {
            if (list.RowsLimit > 0) {
                list._need_refresh = true;
                list.LoadListForNames(values);
            } else {
                list.LoadList();
            }
            SortedList<string, bool> inputSL = new SortedList<string, bool>(values.Length);
            int index;
            for (index = 0; index < values.Length; index++) {
                string value = values[index];
                if ((!string.IsNullOrEmpty(value)) && !inputSL.ContainsKey(value)) {
                    inputSL.Add(value, false);
                }
            }
            List<object> resultList = new List<object>(inputSL.Count);
            VDataTable dt = list.DataTableList;
            DataColumn col_name = dt.Columns[list.GetNameFieldName()]; // колонка c отображаемым имененем
            DataColumn col_key = dt.Columns[list.GetValFieldName()];   // колонка с ключом
            DataRowCollection rows = dt.Rows;
            for (index = 0; index < rows.Count; index++) {
                DataRow row = rows[index];
                string name = row[col_name].ToString();
                if (inputSL.ContainsKey(name)) {
                    resultList.Add(row[col_key]);
                    inputSL[name] = true;
                }
            }
            this.SetValue(resultList);
            string missedValueNames = string.Empty;
            foreach (KeyValuePair<string, bool> de in inputSL) {
                if (!de.Value) {
                    missedValueNames = missedValueNames + de.Key.ToString() + Environment.NewLine;
                }
            }
            if (missedValueNames != string.Empty) {
                missedValueNames = "Неопознанные значения:" + Environment.NewLine + missedValueNames + Environment.NewLine + "Значения должны быть указаны по-одному в каждой строке. (Например, можно скопировать столбец из Excel)";
                //sql.builder.WinForms.ShowMessage.ShowAdvancedMessage(missedValueNames);
            }
        }
    }
}