using System.Data;
using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper class for OracleParameter - isolates Devart dependency
    /// </summary>
    public class VOracleParameter : OracleParameter
    {
        private static Devart.Data.Oracle.OracleDbType ToDevart(VOracleDbType dbType)
        {
            return (Devart.Data.Oracle.OracleDbType)(int)dbType;
        }

        private static object UnwrapValue(object value)
        {
            var va = value as VOracleArray;
            return va != null ? va.Inner : value;
        }

        public VOracleParameter() : base()
        {
        }

        public VOracleParameter(string parameterName, object value) : base(parameterName, value)
        {
        }

        public VOracleParameter(string parameterName, VOracleDbType dbType) : base(parameterName, ToDevart(dbType))
        {
        }

        public VOracleParameter(string parameterName, VOracleDbType dbType, ParameterDirection direction) : base(parameterName, ToDevart(dbType), direction)
        {
        }

        public VOracleParameter(string parameterName, VOracleDbType dbType, object value, ParameterDirection direction) : base(parameterName, ToDevart(dbType), UnwrapValue(value), direction)
        {
        }

        public new VOracleDbType OracleDbType
        {
            get => (VOracleDbType)(int)base.OracleDbType;
            set => base.OracleDbType = ToDevart(value);
        }
    }
}
