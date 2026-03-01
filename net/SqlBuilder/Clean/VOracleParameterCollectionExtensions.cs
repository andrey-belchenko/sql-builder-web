using System.Data;
using System.Data.Common;
using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Extensions for OracleParameterCollection - isolates Devart in wrapper layer
    /// </summary>
    public static class VOracleParameterCollectionExtensions
    {
        public static bool TryGetParameter(this OracleParameterCollection parameters, string parameter_name, out OracleParameter parameter)
        {
            if (parameters == null)
            {
                parameter = null;
                return false;
            }
            int index = parameters.IndexOf(parameter_name);
            if (index >= 0)
            {
                parameter = parameters[index];
                return true;
            }
            parameter = null;
            return false;
        }

        public static VOracleDbType GetOracleDbType(this DbParameter parameter)
        {
            if (parameter is OracleParameter op)
            {
                return (VOracleDbType)(int)op.OracleDbType;
            }
            return VOracleDbType.VarChar;
        }
    }
}
