using System.Data.Common;
using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for OracleConnectionStringBuilder - isolates Devart dependency in wrapper layer.
    /// </summary>
    public class VOracleConnectionStringBuilder
    {
        private readonly OracleConnectionStringBuilder _inner;

        public VOracleConnectionStringBuilder()
        {
            _inner = new OracleConnectionStringBuilder();
        }

        public bool Direct { get => _inner.Direct; set => _inner.Direct = value; }
        public string Server { get => _inner.Server; set => _inner.Server = value; }
        public string ServiceName { get => _inner.ServiceName; set => _inner.ServiceName = value; }
        public int Port { get => _inner.Port; set => _inner.Port = value; }
        public string Sid { get => _inner.Sid; set => _inner.Sid = value; }
        public string UserId { get => _inner.UserId; set => _inner.UserId = value; }
        public string Password { get => _inner.Password; set => _inner.Password = value; }
        public bool Pooling { get => _inner.Pooling; set => _inner.Pooling = value; }
        public string ConnectionString => ((DbConnectionStringBuilder)_inner).ConnectionString;
    }
}
