using System;
using System.Data;
using Oracle.ManagedDataAccess.Client;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper class for OracleDataAdapter - isolates Oracle.ManagedDataAccess dependency.
    /// Uses composition since OracleDataAdapter is sealed in ODP.NET.
    /// </summary>
    public class VOracleDataAdapter : IDisposable
    {
        private readonly OracleDataAdapter _inner;

        public VOracleDataAdapter()
        {
            _inner = new OracleDataAdapter();
        }

        public VOracleDataAdapter(OracleCommand selectCommand)
        {
            _inner = new OracleDataAdapter(selectCommand);
        }

        public VOracleDataAdapter(string selectCommandText, OracleConnection selectConnection)
        {
            _inner = new OracleDataAdapter(selectCommandText, selectConnection);
        }

        public VOracleDataAdapter(string selectCommandText, string selectConnectionString)
        {
            _inner = new OracleDataAdapter(selectCommandText, selectConnectionString);
        }

        public OracleCommand SelectCommand
        {
            get => _inner.SelectCommand as OracleCommand;
            set => _inner.SelectCommand = value;
        }

        public OracleCommand InsertCommand
        {
            get => _inner.InsertCommand as OracleCommand;
            set => _inner.InsertCommand = value;
        }

        public OracleCommand UpdateCommand
        {
            get => _inner.UpdateCommand as OracleCommand;
            set => _inner.UpdateCommand = value;
        }

        public OracleCommand DeleteCommand
        {
            get => _inner.DeleteCommand as OracleCommand;
            set => _inner.DeleteCommand = value;
        }

        public int Fill(DataTable dataTable)
        {
            return _inner.Fill(dataTable);
        }

        public int Fill(DataSet dataSet)
        {
            return _inner.Fill(dataSet);
        }

        public int Fill(DataSet dataSet, string srcTable)
        {
            return _inner.Fill(dataSet, srcTable);
        }

        public void Dispose()
        {
            _inner?.Dispose();
        }
    }
}
