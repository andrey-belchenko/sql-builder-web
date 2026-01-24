using System;
using System.Data;
using System.Linq;
using System.Reflection;
using Contract = System.Diagnostics.Contracts.Contract;
using sql.builder.DataApi;

namespace sql.builder.FieldInfo
{
    internal static class VFieldInfo
    {
        //public delegate bool DReadonly();
        //public DReadonly Readonly;
        //public delegate string DName();
        //public DName Name;
        internal static string Title(object obj, string propName)
        {
            return getInfo<string>(obj, "Title", propName);
        }
        internal static string ControlType(object obj, string propName)
        {
            return getInfo<string>(obj, "ControlType", propName);
        }
        internal static string FieldGroup(object obj, string propName)
        {
            return getInfo<string>(obj, "FieldGroup", propName);
        }
        internal static bool VisibleInTable(object obj, string propName)
        {
            return getInfo<bool>(obj, "VisibleInTable", propName);
        }
        internal static bool VisibleInForm(object obj, string propName)
        {
            return getInfo<bool>(obj, "VisibleInForm", propName);
        }
        internal static bool Exists(object obj, string propName)
        {
            return getInfo<bool>(obj, "Exists", propName);
        }
        internal static bool IsHtml(object obj, string propName)
        {
            return getInfo<bool>(obj, TextConst.PInfo.IsHtml, propName);
        }
        internal static bool Editable(object obj, string propName)
        {
            return getInfo<bool>(obj, TextConst.PInfo.Editable, propName);
        }
        internal static VSXElement UsedEl(object obj, string propName)
        {
            return getInfo<VSXElement>(obj, TextConst.PInfo.UsedEl, propName);
        }
        internal static VSXElement Source(object obj, string propName)
        {
            return getInfo<VSXElement>(obj, TextConst.PInfo.Source, propName);
        }
        internal static int Order(object obj, string propName)
        {
            return getInfo<int>(obj, "Order", propName);
        }
        private static T getInfo<T>(object obj, string infoName, string propName)
        {
            Type type = obj.GetType();
            MethodInfo mi = type.GetMethod(propName + "_" + infoName);
            object ret;
            if (mi != null) {
                ret = mi.Invoke(obj, null);
            } else {
                mi = type.GetMethod("Default_" + infoName);
                if (mi.GetParameters().Length != 0) {
                    ret = mi.Invoke(obj, new object[] { propName });
                } else {
                    ret = mi.Invoke(obj, null);
                }
            }
            return (T)ret;
        }
        /*public static bool ValueInfoExists(object obj, string propName)
        {
            MethodInfo mi = obj.GetType().GetMethod(propName + "_ValueInfo" );
       
            if (mi != null)
            {
               
                return true;
            }
            return false;
        }*/
        internal static string GetValueInfo(object obj, string propName)
        {
            MethodInfo mi = obj.GetType().GetMethod(propName + "_ValueInfo");
            if (mi != null) {
                object value = GetValue(obj, propName);
                object ret = mi.Invoke(obj, new object[] { value });
                return ret.ToString();
            } else {
                //return null;
                return GetValue(obj, propName).ToString();
            }
        }
        internal static object GetValue(object obj, string propName)
        {
            return Cmn.GetProperty(obj, propName, null);
        }
        internal static void SetValue(VSXElement obj, string propName, object value)
        {
            if (obj.Row != null) {
                string name = VSXElement.RemovePropPfx(propName);
                obj.Row[name] = value;
            } else {
                Cmn.SetProperty(obj, propName, value);
            }
        }
        /// <summary>
        /// Создаёт пустой VDataTable и вызывает у <paramref name="obj"/> метод P_XXX_List для создания колонкок
        /// </summary>
        /// <param name="obj">объект VSXElement</param>
        /// <param name="propName">наименование свойства с префиксом "P_"</param>
        /// <returns></returns>
        internal static VDataSet GetList(object obj, string propName)
        {
            string method_name = propName + "_List";
            Type type = obj.GetType();
            MethodInfo mi = type.GetMethod(method_name);
            VDataSet ds = new VDataSet();
            VDataTable dt = new VDataTable();
            dt.TableName = type.FullName + "." + method_name;
            ds.Tables.Add(dt);
            if (mi == null) {
                return ds;
            }
            mi.Invoke(obj, new object[] { dt });
            if (!dt.HasPrimaryKey() && dt.Columns.Count != 0) {
                dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
            }
            return ds;
        }
        internal static void RefreshListMethod(VDataSet dataSet)
        {
            VDataTable ownerTable = (dataSet.OwnerColumn.Table as VDataTable);
            object obj = ownerTable.CurrentRow["node"]; //!! пока только частный случай для VSXElement
            Type type = obj.GetType();
            string propName = VSXElement.PropPfx + dataSet.OwnerColumn.ColumnName;
            MethodInfo list_refresh = type.GetMethod(propName + "_ListRefresh");
            if (list_refresh != null) {
                VDataTable dt = (VDataTable)dataSet.Tables[0];
                string list_method = propName + "_List";
                string table_name = type.FullName + "." + list_method;
                if (dt.TableName != table_name) {
                    MethodInfo list = type.GetMethod(list_method);
                    Contract.Assert(list != null);
                    dt.Rows.Clear();
                    dt.ClearColumns();
                    list.Invoke(obj, new object[] { dt });
                    dt.TableName = table_name;
                    if (!dt.HasPrimaryKey() && dt.Columns.Count != 0) {
                        dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
                    }
                }
                list_refresh.Invoke(obj, new object[] { dt });
                if (!dt.HasPrimaryKey() && dt.Columns.Count != 0) {
                    dt.PrimaryKey = new DataColumn[] { dt.Columns[0] };
                }
                dataSet.RaiseSchemeChanged();
            }
        }
        internal static VFieldStateAndOtherInfo GetFieldInfoForDataTableOfVSXElementCell(VDataColumn col, DataRow r, InfoTypesToGet[] getWhat)
        {
            var obj = r["node"]; //!! пока только частный случай для VSXElement
            VFieldStateAndOtherInfo fs = new VFieldStateAndOtherInfo();
            if (getWhat.Contains(InfoTypesToGet.State)) {
                fs.Exists = Exists(obj, VSXElement.PropPfx + col.ColumnName);
                if (fs.Exists) {
                    fs.VisibleInForm = VisibleInForm(obj, VSXElement.PropPfx + col.ColumnName);
                    fs.Editable = Editable(obj, VSXElement.PropPfx + col.ColumnName);
                } else {
                    fs.VisibleInForm = false;
                    fs.Editable = true;
                }
            }
            if (getWhat.Contains(InfoTypesToGet.ValueName)) {
                fs.ValueName=GetValueInfo(obj, VSXElement.PropPfx + col.ColumnName);
            }           
            return fs;
        }
        internal enum InfoTypesToGet
        {
            State,
            ValueName
        }
        internal enum StateValue
        {
            True,
            False,
            Inherit
        }
    }
    internal class VFieldStateAndOtherInfo // Для передачи контролу
    {
        internal bool Exists;
        internal bool VisibleInForm;
        internal bool Editable;
        internal string ValueName;
        //public bool NoBorder;
        internal VFieldStateAndOtherInfo() { }
        internal VFieldStateAndOtherInfo(bool exists)
        {
            Exists = exists;
            VisibleInForm = exists;           
        }
    }
    internal class VFieldState  // Для хранения
    {
        internal VFieldInfo.StateValue Editable = VFieldInfo.StateValue.Inherit;
        internal bool Required = false;
        internal bool Valid = true;
    }
}