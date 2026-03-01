using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for OracleArray - isolates Devart dependency in wrapper layer.
    /// </summary>
    public class VOracleArray
    {
        private readonly OracleArray _inner;

        internal VOracleArray(OracleArray devartArray)
        {
            _inner = devartArray;
        }

        internal OracleArray Inner => _inner;

        public void Add(object value) => _inner.Add(value);

        public static VOracleArray Create(string typeName, VOracleConnection connection)
        {
            return new VOracleArray(new OracleArray(typeName, connection));
        }

        public static VOracleArray Create(VOracleType type, object[] values)
        {
            return new VOracleArray(new OracleArray(type.Inner, values));
        }
    }
}
