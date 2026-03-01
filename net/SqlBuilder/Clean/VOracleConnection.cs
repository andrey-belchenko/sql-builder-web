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
    }
}
