using System.Data;
using System.Diagnostics;
using System.Text;
using Devart.Data.Oracle;
using infoenergo.core.Data; // DataHelper, OracleSqlException
using infoenergo.sys;
using sql.builder.Clean;
using sql.builder.DataApi; // TextConst
using SqlBuilderLib.DevTools;

namespace sql.builder.Core
{
    public class ArrayStorage
    {
        #region static
        /// <summary>
        /// Тип ASUSETYPES.NUMBER$TABLE
        /// </summary>
        private static OracleType _number_table_type;
        /// <summary>
        /// Тип ASUSETYPES.VARCHAR2$TABLE
        /// </summary>
        private static OracleType _varchar2_table_type;
        private static bool TryGetOracleType(string type_name, ref OracleType type)
        {
            //try {
            type = OracleType.GetObjectType(type_name, db.Connection);
            return true;
        }
        static ArrayStorage()
        {
            TryGetOracleType("ASUSETYPES.NUMBER$TABLE", ref _number_table_type);
            TryGetOracleType("ASUSETYPES.VARCHAR2$TABLE", ref _varchar2_table_type);
        }
        public static void ClearStoredValues(string id)
        {
            VOracleParameter par = new VOracleParameter("array_id", VOracleDbType.VarChar, id, ParameterDirection.Input);
            DataHelper.SqlExecute("DELETE FROM vr_array_storage WHERE array_id = :array_id", new VOracleParameter[1] { par }, Global.Connection);
        }
        #endregion
        private string _id;
        private string _value_column;
        //private string _datatype;
        private object[] _values;
        public ArrayStorage(string id)
        {
            this._id = id;
        }
        /*public void PrepareValues(object[] values)
        {
            this._values = values;
        }
        public void ApplyValues()
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
            VOracleDbType data_type;
            OracleType array_type;
            if (values[0] is string)
            {
                this._value_column = "sval";
                data_type = VOracleDbType.NVarChar;
                array_type = _varchar2_table_type;
            }
            else
            {
                this._value_column = "nval";
                data_type = VOracleDbType.Number;
                array_type = _number_table_type;
            }
#if DEBUG
            Stopwatch sw = new Stopwatch();
            sw.Start();
#endif
            VOracleCommand cmd = null;
            try
            {
                try
                {
                    if (array_type != null)
                    {
                        // FORALL INSERT INTO примерно в 48 раз быстрее простого INSERT'а
                        cmd = new VOracleCommand("DECLARE\n" +
                                                "  s_array_id vr_array_storage.array_id%TYPE;\n" +
                                                "BEGIN\n" +
                                                "  s_array_id := :array_id;\n" +
                                                "  delete from vr_array_storage where array_id = s_array_id;\n" +
                                                "  FORALL i IN 1..:count\n" +
                                                "    INSERT INTO vr_array_storage (array_id, " + this._value_column + ")\n" +
                                                "      VALUES (s_array_id, :value(i));\n" +
                                                "END;", Global.Connection);
                        cmd.Parameters.Add(new VOracleParameter("array_id", VOracleDbType.NVarChar, this._id, ParameterDirection.Input));
                        cmd.Parameters.Add(new VOracleParameter("count", VOracleDbType.Integer, values.Length, ParameterDirection.Input));
                        OracleArray array = new OracleArray(array_type, values);
                        cmd.Parameters.Add(new VOracleParameter("value", VOracleDbType.Array, array, ParameterDirection.Input));
                        DevUtilsProvider.Instance.AnalyzeExecSql(cmd.CommandText);
                        cmd.ExecuteNonQuery();
                    }
                    else
                    {
                        cmd = new VOracleCommand("delete from vr_array_storage where array_id = :array_id", Global.Connection);
                        VOracleParameter par_array_id = new VOracleParameter("array_id", VOracleDbType.NVarChar, this._id, ParameterDirection.Input);
                        cmd.Parameters.Add(par_array_id);
                        DevUtilsProvider.Instance.AnalyzeExecSql(cmd.CommandText);
                        cmd.ExecuteNonQuery();
                        //
                        cmd.CommandText = "insert into vr_array_storage (array_id, " + this._value_column + ") values (:array_id, :value)";
                        cmd.Parameters.Clear();
                        cmd.Parameters.Add(par_array_id);
                        VOracleParameter par_value = new VOracleParameter("value", data_type, null, ParameterDirection.Input);
                        cmd.Parameters.Add(par_value);
                        cmd.Prepare();
                        DevUtilsProvider.Instance.AnalyzeExecSql(cmd.CommandText);
                        for (int index = 0; index < values.Length; index++)
                        {
                            par_value.Value = values[index];
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
                catch (Devart.Data.Oracle.OracleException ex)
                {

                    throw ex;
                    //throw new infoenergo.core.Data.OracleSqlException(ex, cmd.CommandText, cmd.Parameters);
                }
            }
            finally
            {
                if (cmd != null)
                {
                    Cmn.DisposeAndSetNull(ref cmd);
                }
            }
#if DEBUG
            sw.Stop();
            // Debug.WriteLine("ArrayStorage.SetStoredValues(): Вставка в vr_array_storage." + this._value_column + " " + values.Length.ToString() + " значений с array_id=\"" + this._id + "\" за " + sw.ElapsedTicks.ToString() + " тактов = " + sw.ElapsedMilliseconds.ToString() + " мс");
#endif
        }
        public void SetValues(object[] values)
        {
            if (values.Length > 30)
            { // раньше было 999 , и получался очень большой текст sql
                this.SetStoredValues(values);
            }
            else
            {
                this.SetInlinedValues(values);
            }
        }
        public void SetValues(object[] values, string mode)
        {
            if (mode == TextConst.AVArrayParamModes.Store)
            {
                this.SetStoredValues(values);
            }
            else if (mode == TextConst.AVArrayParamModes.Inline)
            {
                this.SetInlinedValues(values);
            }
            else
            {
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
        /*public string GetSql()
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
        public string GetSql()
        {
            if (this._values == null)
            {
                return " (select " + this._value_column + " from vr_array_storage where array_id = '" + this._id + "') ";
            }
            else if (this._values.Length == 0)
            {
                return Cmn.undefinedString;
            }
            else
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(" (");
                sb.Append(Cmn.ToOracleString(this._values[0]));
                for (int index = 1; index < this._values.Length; index++)
                {
                    sb.Append(',');
                    sb.Append(Cmn.ToOracleString(this._values[index]));
                }
                sb.Append(')');
                return sb.ToString();
            }
        }
        /*public static string DataColumnName(string datatype)
        {
            return (datatype == "number") ? "nval"
                 : (datatype == "string") ? "sval"
                 : null;
        }
        public static string TypedValueString(object value, string datatype)
        {
            return (datatype == "number") ? Cmn.ToOracleString(value)
                 : (datatype == "string") ? ("\'" + value + "\'")
                 : null;
        }*/
    }
}