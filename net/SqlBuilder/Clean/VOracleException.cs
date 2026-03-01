using System;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for Oracle exception - isolates Oracle.ManagedDataAccess dependency in wrapper layer.
    /// </summary>
    public class VOracleException : Exception
    {
        private readonly Oracle.ManagedDataAccess.Client.OracleException _inner;

        internal VOracleException(Oracle.ManagedDataAccess.Client.OracleException oraException) : base(oraException?.Message, oraException)
        {
            _inner = oraException;
        }

        public int Code => _inner?.Number ?? -1;
        public int ErrorCode => _inner?.Number ?? 0;
        public object Errors => _inner?.Errors;
    }
}
