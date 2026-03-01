using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for OracleConnectionStringBuilder - isolates Oracle.ManagedDataAccess dependency in wrapper layer.
    /// Builds ODP.NET connection string from Devart-style properties.
    /// </summary>
    public class VOracleConnectionStringBuilder
    {
        private readonly OracleConnectionStringBuilder _inner = new OracleConnectionStringBuilder();
        private bool _direct;
        private string _server;
        private string _serviceName;
        private int _port;
        private string _sid;

        public VOracleConnectionStringBuilder()
        {
        }

        public bool Direct { get => _direct; set => _direct = value; }
        public string Server { get => _server; set => _server = value; }
        public string ServiceName { get => _serviceName; set => _serviceName = value; }
        public int Port { get => _port; set => _port = value; }
        public string Sid { get => _sid; set => _sid = value; }
        public string UserId { get => (string)_inner["User Id"]; set => _inner["User Id"] = value; }
        public string Password { get => _inner.Password; set => _inner.Password = value; }
        public bool Pooling { get => _inner.Pooling; set => _inner.Pooling = value; }

        public string ConnectionString
        {
            get
            {
                var sb = new OracleConnectionStringBuilder();
                sb["User Id"] = _inner["User Id"];
                sb["Password"] = _inner["Password"];
                sb.Pooling = _inner.Pooling;

                if (!string.IsNullOrEmpty(_server))
                {
                    if (_direct)
                    {
                        sb.DataSource = _port > 0 && (!string.IsNullOrEmpty(_serviceName) || !string.IsNullOrEmpty(_sid))
                            ? _server + ":" + _port + "/" + (!string.IsNullOrEmpty(_serviceName) ? _serviceName : _sid)
                            : _server;
                    }
                    else
                    {
                        sb.DataSource = _server;
                    }
                }

                return sb.ConnectionString;
            }
        }
    }
}
