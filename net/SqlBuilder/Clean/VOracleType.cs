using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for OracleType - isolates Devart dependency in wrapper layer.
    /// </summary>
    public class VOracleType
    {
        private readonly OracleType _inner;

        internal VOracleType(OracleType devartType)
        {
            _inner = devartType;
        }

        internal OracleType Inner => _inner;

        public static bool TryGetObjectType(string typeName, VOracleConnection connection, out VOracleType type)
        {
            var t = OracleType.GetObjectType(typeName, connection);
            type = t != null ? new VOracleType(t) : null;
            return type != null;
        }
    }
}
