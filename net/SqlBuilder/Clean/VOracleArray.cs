using System;
using System.Collections.Generic;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for Oracle array/collection types - isolates Oracle.ManagedDataAccess dependency in wrapper layer.
    /// ODP.NET Managed Driver does not support OracleDbType.Array; use UDT custom types for nested tables.
    /// This implementation supports fallback: ArrayStorage uses row-by-row when VOracleType.TryGetObjectType returns false.
    /// Create() throws for external callers (ConvertDecimalArrayToOracle, ConvertStringArrayToOracle) - use row-by-row or UDT.
    /// </summary>
    public class VOracleArray
    {
        private readonly List<object> _values = new List<object>();

        internal VOracleArray()
        {
        }

        internal object Inner => _values;

        public void Add(object value) => _values.Add(value);

        /// <summary>
        /// ODP.NET Managed does not support OracleArray(typeName, connection).
        /// Throws NotSupportedException. Use row-by-row or implement UDT custom types.
        /// </summary>
        public static VOracleArray Create(string typeName, VOracleConnection connection)
        {
            throw new NotSupportedException(
                "Oracle.ManagedDataAccess.Core does not support OracleArray. " +
                "Use DataHelper row-by-row approach or implement UDT custom types for ASUSETYPES.NUMBER$TABLE/VARCHAR2$TABLE.");
        }

        /// <summary>
        /// Creates array from VOracleType and values. Only used when VOracleType.TryGetObjectType succeeds.
        /// Since TryGetObjectType returns false for ODP.NET, this path is not reached.
        /// </summary>
        public static VOracleArray Create(VOracleType type, object[] values)
        {
            throw new NotSupportedException(
                "Oracle.ManagedDataAccess.Core does not support OracleArray. " +
                "ArrayStorage uses row-by-row fallback when UDT types are unavailable.");
        }
    }
}
