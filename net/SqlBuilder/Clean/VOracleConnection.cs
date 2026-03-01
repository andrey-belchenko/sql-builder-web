using System;
using Oracle.ManagedDataAccess.Client;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper class for OracleConnection - isolates Oracle.ManagedDataAccess dependency
    /// </summary>
    public class VOracleConnection : OracleConnection
    {
        public VOracleConnection() : base()
        {
        }

        public VOracleConnection(string connectionString) : base(connectionString)
        {
        }

        /// <summary>
        /// Creates a VOracleCommand associated with this connection.
        /// Use this instead of CreateCommand() when VOracleCommand is needed.
        /// </summary>
        public new VOracleCommand CreateCommand()
        {
            var cmd = new VOracleCommand();
            cmd.Connection = this;
            return cmd;
        }

        public new VOracleConnection Clone()
        {
            return new VOracleConnection(this.ConnectionString);
        }

        /// <summary>
        /// Begins a transaction and returns VOracleTransaction.
        /// </summary>
        public new VOracleTransaction BeginTransaction()
        {
            return new VOracleTransaction(base.BeginTransaction());
        }

        public override void Open()
        {
            try
            {
                base.Open();
            }
            catch (OracleException ex)
            {
                throw new VOracleException(ex);
            }
        }

        /// <summary>
        /// Parsed from connection string for DataHelper.GetConnectionString compatibility.
        /// True if DataSource uses Easy Connect format (host:port/service).
        /// </summary>
        public bool Direct
        {
            get
            {
                var ds = DataSource ?? "";
                return ds.Contains(":") && (ds.Contains("/") || ds.Contains(":"));
            }
        }

        /// <summary>
        /// Host part from DataSource (Easy Connect) or DataSource as-is (TNS).
        /// </summary>
        public string Server => ParseServerFromDataSource(DataSource);

        /// <summary>
        /// Service name from DataSource (Easy Connect format) or null.
        /// </summary>
        public string ServiceName => ParseServiceNameFromDataSource(DataSource);

        /// <summary>
        /// Port from DataSource (Easy Connect format) or 0.
        /// </summary>
        public int Port => ParsePortFromDataSource(DataSource);

        /// <summary>
        /// SID from DataSource if present. ODP.NET Easy Connect uses service name; SID is legacy.
        /// </summary>
        public string Sid => null;

        /// <summary>
        /// User ID from connection string.
        /// </summary>
        public string UserId
        {
            get
            {
                try
                {
                    var cb = new OracleConnectionStringBuilder(ConnectionString);
                    return (string)cb["User Id"];
                }
                catch { return null; }
            }
        }

        /// <summary>
        /// Password from connection string.
        /// </summary>
        public string Password
        {
            get
            {
                try
                {
                    var cb = new OracleConnectionStringBuilder(ConnectionString);
                    return (string)cb["Password"];
                }
                catch { return null; }
            }
            set
            {
                var cb = new OracleConnectionStringBuilder(ConnectionString);
                cb["Password"] = value;
                ConnectionString = cb.ConnectionString;
            }
        }

        private static string ParseServerFromDataSource(string dataSource)
        {
            if (string.IsNullOrEmpty(dataSource)) return "";
            int colon = dataSource.IndexOf(':');
            int slash = dataSource.IndexOf('/');
            if (colon > 0)
                return dataSource.Substring(0, colon);
            if (slash > 0)
                return dataSource.Substring(0, slash);
            return dataSource;
        }

        private static int ParsePortFromDataSource(string dataSource)
        {
            if (string.IsNullOrEmpty(dataSource)) return 0;
            int colon = dataSource.IndexOf(':');
            int slash = dataSource.IndexOf('/');
            if (colon >= 0 && slash > colon + 1)
            {
                var portStr = dataSource.Substring(colon + 1, slash - colon - 1);
                return int.TryParse(portStr, out int p) ? p : 0;
            }
            return 0;
        }

        private static string ParseServiceNameFromDataSource(string dataSource)
        {
            if (string.IsNullOrEmpty(dataSource)) return null;
            int slash = dataSource.IndexOf('/');
            if (slash >= 0 && slash + 1 < dataSource.Length)
                return dataSource.Substring(slash + 1);
            return null;
        }
    }
}
