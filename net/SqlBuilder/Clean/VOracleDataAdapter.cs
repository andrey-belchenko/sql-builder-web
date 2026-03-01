using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper class for OracleDataAdapter - isolates Devart dependency
    /// </summary>
    public class VOracleDataAdapter : OracleDataAdapter
    {
        public VOracleDataAdapter() : base()
        {
        }

        public VOracleDataAdapter(OracleCommand selectCommand) : base(selectCommand)
        {
        }

        public VOracleDataAdapter(string selectCommandText, OracleConnection selectConnection) : base(selectCommandText, selectConnection)
        {
        }

        public VOracleDataAdapter(string selectCommandText, string selectConnectionString) : base(selectCommandText, selectConnectionString)
        {
        }
    }
}
