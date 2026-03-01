using Devart.Data.Oracle;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for OracleLob - isolates Devart dependency in wrapper layer.
    /// </summary>
    public class VOracleLob
    {
        private readonly OracleLob _inner;

        internal VOracleLob(OracleLob devartLob)
        {
            _inner = devartLob;
        }

        internal OracleLob Inner => _inner;
    }
}
