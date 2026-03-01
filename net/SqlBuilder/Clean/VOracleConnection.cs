using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper class for OracleConnection - isolates Devart dependency
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
    }
}
