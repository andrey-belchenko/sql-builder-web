using System;
using Oracle.ManagedDataAccess.Types;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for Oracle LOB - isolates Oracle.ManagedDataAccess dependency in wrapper layer.
    /// Wraps OracleBlob or OracleClob (ODP.NET has separate types).
    /// </summary>
    public class VOracleLob
    {
        private readonly object _inner;

        internal VOracleLob(OracleBlob blob)
        {
            _inner = blob;
        }

        internal VOracleLob(OracleClob clob)
        {
            _inner = clob;
        }

        internal object Inner => _inner;

        public long Length
        {
            get
            {
                if (_inner is OracleBlob blob)
                    return blob.Length;
                if (_inner is OracleClob clob)
                    return clob.Length;
                return 0;
            }
        }
    }
}
