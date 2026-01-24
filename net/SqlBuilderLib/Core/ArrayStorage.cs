using System;
using System.Diagnostics; 
using Contract = System.Diagnostics.Contracts.Contract;
using System.Text;
using System.Data;
using Oracle.ManagedDataAccess.Client;
using infoenergo.sys;
using infoenergo.core.Data; // DataHelper, OracleSqlException
using sql.builder.DataApi; // TextConst

namespace sql.builder.Core
{
    internal class ArrayStorage
    {
        #region static
        // Note: Oracle.ManagedDataAccess.Core doesn't support UDTs (User-Defined Types) like OracleType
        // We'll use the fallback path with individual inserts instead of FORALL with array types
        private static object _number_table_type = null; // Always null - triggers fallback path
        private static object _varchar2_table_type = null; // Always null - triggers fallback path
        static ArrayStorage()
        {
            // OracleType.GetObjectType() is not available in ODP.NET Core
            // Array operations will use the fallback path (individual inserts)
        }
        internal static void ClearStoredValues(string id)
        {
            OracleParameter par = new OracleParameter("array_id", OracleDbType.Varchar2, id, ParameterDirection.Input);
            DataHelper.SqlExecute("DELETE FROM vr_array_storage WHERE array_id = :array_id", new OracleParameter[1] { par }, Global.Connection);
        }
        #endregion
        private string _id;
        private string _value_column;
        //private string _datatype;
        private object[] _values;
        internal ArrayStorage(string id)
        {
            this._id = id;
        }
        /*internal void PrepareValues(object[] values)
        {
            this._values = values;
        }
        internal void ApplyValues()
        {
            SetValues(_values);
        }*/
        private void SetInlinedValues(object[] values)
        {
            this._values = values;
            this._value_column = null;
        }
        private void SetStoredValues(object[] values)
        {
                this._values = null;
                OracleDbType data_type;
                // Note: Oracle.ManagedDataAccess.Core doesn't support UDTs, so we always use the fallback path
                if (values.Length > 0 && values[0] is string) {
                    this._value_column = "sval";
                    data_type = OracleDbType.NVarchar2;
                } else {
                    this._value_column = "nval";
                    data_type = OracleDbType.Decimal;
                }
                #if DEBUG
                Stopwatch sw = new Stopwatch();
                sw.Start();
                #endif
                OracleCommand cmd = null;
                try {
                try
                {
                    // ODP.NET Core doesn't support OracleArray/UDTs, so we use individual inserts
                    // This is slower than FORALL but works with the free provider
                    cmd = new OracleCommand("delete from vr_array_storage where array_id = :array_id", Global.Connection);
                    OracleParameter par_array_id = new OracleParameter("array_id", OracleDbType.NVarchar2, this._id, ParameterDirection.Input);
                    cmd.Parameters.Add(par_array_id);
                    cmd.ExecuteNonQuery();
                    //
                    cmd.CommandText = "insert into vr_array_storage (array_id, " + this._value_column + ") values (:array_id, :value)";
                    cmd.Parameters.Clear();
                    cmd.Parameters.Add(par_array_id);
                    OracleParameter par_value = new OracleParameter("value", data_type, null, ParameterDirection.Input);
                    cmd.Parameters.Add(par_value);
                    cmd.Prepare();
                    for (int index = 0; index < values.Length; index++)
                    {
                        par_value.Value = values[index];
                        cmd.ExecuteNonQuery();
                    }
                }
                catch (Oracle.ManagedDataAccess.Client.OracleException ex)
                {
                    throw ex;
                    //throw new infoenergo.core.Data.OracleSqlException(ex, cmd.CommandText, cmd.Parameters);
                }
                } finally {
                    if (cmd != null) {
                        Cmn.DisposeAndSetNull(ref cmd);
                    }
                }
                #if DEBUG
                sw.Stop();
                Debug.WriteLine("ArrayStorage.SetStoredValues(): Вставка в vr_array_storage." + this._value_column + " " + values.Length.ToString() + " значений с array_id=\"" + this._id + "\" за " + sw.ElapsedTicks.ToString() + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
                #endif
        }
        internal void SetValues(object[] values)
        {
            if (values.Length > 30) { // раньше было 999 , и получался очень большой текст sql
                this.SetStoredValues(values);
            } else {
                this.SetInlinedValues(values);
            }
        }
        internal void SetValues(object[] values, string mode)
        {
            if (mode == TextConst.AVArrayParamModes.Store) {
                this.SetStoredValues(values);
            } else if (mode == TextConst.AVArrayParamModes.Inline) {
                this.SetInlinedValues(values);
            } else {
                this.SetValues(values);
            }
        }
        //public void Clear()
        //{
        //    db.ClearArrayStorage(_id);
        //}
        //public decimal[] Get()
        //{
        //    var dt = db.GetFromArrayStorage(_id);
        //    return dt.AsEnumerable().Select(r => (decimal)r["VAL"]).ToArray();
        //}
        /*internal string GetSql()
        {
            string sql;
            if (this._values != null) {
                if (this._values.Length == 0) {
                    sql = Cmn.undefinedString;
                } else {
                    sql = " (" + string.Join(",", _values.Select(Cmn.ToOracleString)) + ")";
                }
            } else {
                //sql = string.Format(" (select {1} from vr_array_storage where array_id = '{0}') ", _id, DataColumnName(_datatype));
                sql = " (select " + this._value_column + " from vr_array_storage where array_id = '" + this._id + "') ";
            }
            return sql;
        }*/
        internal string GetSql()
        {
            if (this._values == null) {
                return " (select " + this._value_column + " from vr_array_storage where array_id = '" + this._id + "') ";
            } else if (this._values.Length == 0) {
                return Cmn.undefinedString;
            } else {
                StringBuilder sb = new StringBuilder();
                sb.Append(" (");
                sb.Append(Cmn.ToOracleString(this._values[0]));
                for (int index = 1; index < this._values.Length; index++) {
                    sb.Append(',');
                    sb.Append(Cmn.ToOracleString(this._values[index]));
                }
                sb.Append(')');
                return sb.ToString();
            }
        }
        /*internal static string DataColumnName(string datatype)
        {
            return (datatype == "number") ? "nval"
                 : (datatype == "string") ? "sval"
                 : null;
        }
        internal static string TypedValueString(object value, string datatype)
        {
            return (datatype == "number") ? Cmn.ToOracleString(value)
                 : (datatype == "string") ? ("\'" + value + "\'")
                 : null;
        }*/
    }
}