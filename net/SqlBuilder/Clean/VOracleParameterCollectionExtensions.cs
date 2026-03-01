using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Linq;
using Oracle.ManagedDataAccess.Client;

namespace sql.builder.Clean
{
    /// <summary>
    /// Extensions for OracleParameterCollection - isolates Oracle.ManagedDataAccess in wrapper layer
    /// </summary>
    public static class VOracleParameterCollectionExtensions
    {
        public static void AddRange(this OracleParameterCollection parameters, IEnumerable<VOracleParameter> vParams)
        {
            if (parameters == null || vParams == null) return;
            foreach (var vp in vParams)
                parameters.Add(vp.Inner);
        }

        public static int Add(this OracleParameterCollection parameters, VOracleParameter vParam)
        {
            if (parameters == null || vParam == null) return -1;
            parameters.Add(vParam.Inner);
            return parameters.Count - 1;
        }

        public static IEnumerable<VOracleParameter> AsVOracleParameters(this OracleParameterCollection parameters)
        {
            if (parameters == null) yield break;
            foreach (OracleParameter p in parameters)
                yield return new VOracleParameter(p);
        }

        public static bool TryGetParameter(this OracleParameterCollection parameters, string parameter_name, out VOracleParameter parameter)
        {
            if (parameters == null)
            {
                parameter = null;
                return false;
            }
            int index = parameters.IndexOf(parameter_name);
            if (index >= 0)
            {
                parameter = new VOracleParameter(parameters[index]);
                return true;
            }
            parameter = null;
            return false;
        }

        public static bool TryGetParameter(this VOracleCommand cmd, string parameter_name, out VOracleParameter parameter)
        {
            return cmd.Parameters.TryGetParameter(parameter_name, out parameter);
        }

        public static VOracleDbType GetOracleDbType(this VOracleParameter parameter)
        {
            return parameter?.OracleDbType ?? VOracleDbType.VarChar;
        }

        public static VOracleDbType GetOracleDbType(this DbParameter parameter)
        {
            var op = parameter as OracleParameter;
            if (op != null)
            {
                switch (op.OracleDbType)
                {
                    case OracleDbType.Decimal: return VOracleDbType.Number;
                    case OracleDbType.Varchar2: return VOracleDbType.VarChar;
                    case OracleDbType.Date: return VOracleDbType.Date;
                    case OracleDbType.Clob: return VOracleDbType.Clob;
                    case OracleDbType.Blob: return VOracleDbType.Blob;
                    case OracleDbType.NClob: return VOracleDbType.NClob;
                    case OracleDbType.NVarchar2: return VOracleDbType.NVarChar;
                    case OracleDbType.IntervalDS: return VOracleDbType.IntervalDS;
                    case OracleDbType.Int32: return VOracleDbType.Integer;
                    default: return VOracleDbType.VarChar;
                }
            }
            return VOracleDbType.VarChar;
        }

        /// <summary>
        /// Gets display string for parameter value (handles BLOB/CLOB without exposing provider types).
        /// </summary>
        public static string GetParameterValueDisplay(DbParameter parameter, VOracleDbType dbType)
        {
            var val = parameter?.Value;
            if (val == null) return "";
            if (val == DBNull.Value) return val.ToString();
            if (dbType == VOracleDbType.Blob)
            {
                if (val is byte[] bytes)
                    return "[BLOB length=" + bytes.Length + "]";
                return val.ToString();
            }
            if (dbType == VOracleDbType.Clob) return "[CLOB]";
            return val.ToString();
        }
    }
}
