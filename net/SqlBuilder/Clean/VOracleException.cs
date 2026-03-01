using System;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for Oracle exception - isolates Devart dependency in wrapper layer.
    /// </summary>
    public class VOracleException : Exception
    {
        private readonly Devart.Data.Oracle.OracleException _inner;

        internal VOracleException(Devart.Data.Oracle.OracleException devartException) : base(devartException?.Message, devartException)
        {
            _inner = devartException;
        }

        public int Code => _inner?.Code ?? -1;
        public int ErrorCode => _inner?.ErrorCode ?? 0;
        public object Errors => _inner?.Errors;
    }
}
