using System.Data.Common;
using Oracle.ManagedDataAccess.Client;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper for OracleTransaction - isolates Oracle.ManagedDataAccess dependency in wrapper layer.
    /// </summary>
    public class VOracleTransaction : DbTransaction
    {
        private readonly OracleTransaction _inner;

        internal VOracleTransaction(OracleTransaction oraTransaction)
        {
            _inner = oraTransaction;
        }

        public override void Commit() => _inner.Commit();
        public override void Rollback() => _inner.Rollback();

        protected override DbConnection DbConnection => _inner.Connection;

        public override System.Data.IsolationLevel IsolationLevel => _inner.IsolationLevel;

        internal OracleTransaction Inner => _inner;
    }
}
