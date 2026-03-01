using System;
using System.Data;
using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper class for OracleParameter - isolates Oracle.ManagedDataAccess dependency.
    /// Uses composition since OracleParameter is sealed in ODP.NET.
    /// </summary>
    public class VOracleParameter
    {
        private readonly OracleParameter _inner;

        internal OracleParameter Inner => _inner;

        private static object UnwrapValue(object value)
        {
            var va = value as VOracleArray;
            return va != null ? va.Inner : value;
        }

        public VOracleParameter()
        {
            _inner = new OracleParameter();
        }

        public VOracleParameter(string parameterName, object value)
        {
            _inner = new OracleParameter(parameterName, value);
        }

        public VOracleParameter(string parameterName, VOracleDbType dbType)
        {
            _inner = new OracleParameter(parameterName, dbType.ToOracle());
        }

        public VOracleParameter(string parameterName, VOracleDbType dbType, ParameterDirection direction)
        {
            _inner = new OracleParameter(parameterName, dbType.ToOracle(), 0, direction);
        }

        public VOracleParameter(string parameterName, VOracleDbType dbType, object value, ParameterDirection direction)
        {
            _inner = new OracleParameter(parameterName, dbType.ToOracle(), 0, direction);
            _inner.Value = UnwrapValue(value);
        }

        internal VOracleParameter(OracleParameter inner)
        {
            _inner = inner ?? throw new ArgumentNullException(nameof(inner));
        }

        public DbParameter GetDbParameter()
        {
            return _inner;

        }

        public string ParameterName { get => _inner.ParameterName; set => _inner.ParameterName = value; }
        public object Value { get => _inner.Value; set => _inner.Value = value; }
        public ParameterDirection Direction { get => _inner.Direction; set => _inner.Direction = value; }
        public string SourceColumn { get => _inner.SourceColumn; set => _inner.SourceColumn = value; }
        public DbType DbType { get => _inner.DbType; set => _inner.DbType = value; }

        public VOracleDbType OracleDbType
        {
            get => MapFromOracle(_inner.OracleDbType);
            set => _inner.OracleDbType = value.ToOracle();
        }

        private static VOracleDbType MapFromOracle(Oracle.ManagedDataAccess.Client.OracleDbType oraType)
        {
            switch (oraType)
            {
                case Oracle.ManagedDataAccess.Client.OracleDbType.Decimal: return VOracleDbType.Number;
                case Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2: return VOracleDbType.VarChar;
                case Oracle.ManagedDataAccess.Client.OracleDbType.Date: return VOracleDbType.Date;
                case Oracle.ManagedDataAccess.Client.OracleDbType.Clob: return VOracleDbType.Clob;
                case Oracle.ManagedDataAccess.Client.OracleDbType.Blob: return VOracleDbType.Blob;
                case Oracle.ManagedDataAccess.Client.OracleDbType.NClob: return VOracleDbType.NClob;
                case Oracle.ManagedDataAccess.Client.OracleDbType.NVarchar2: return VOracleDbType.NVarChar;
                case Oracle.ManagedDataAccess.Client.OracleDbType.IntervalDS: return VOracleDbType.IntervalDS;
                case Oracle.ManagedDataAccess.Client.OracleDbType.Int32: return VOracleDbType.Integer;
                case Oracle.ManagedDataAccess.Client.OracleDbType.Array: return VOracleDbType.Array;
                default: return VOracleDbType.VarChar;
            }
        }
    }
}
