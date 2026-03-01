using System;
using Oracle.ManagedDataAccess.Client;
using SqlBuilderLib.DevTools;

namespace sql.builder.Clean
{
    /// <summary>
    /// Wrapper class for OracleCommand that intercepts CommandText assignments
    /// </summary>
    public class VOracleCommand : OracleCommand
    {
        /// <summary>
        /// Event raised when CommandText is set
        /// </summary>
        public event EventHandler<CommandTextChangedEventArgs> CommandTextChanged;

        /// <summary>
        /// Override CommandText property to intercept assignments
        /// </summary>
        public override string CommandText
        {
            get => base.CommandText;
            set
            {
                string oldValue = base.CommandText;
                base.CommandText = value;

                // Raise event when CommandText is set
                OnCommandTextChanged(oldValue, value);
            }
        }

        /// <summary>
        /// Default constructor
        /// </summary>
        public VOracleCommand() : base()
        {
        }

        /// <summary>
        /// Constructor with command text
        /// </summary>
        public VOracleCommand(string commandText) : base(commandText)
        {
            OnCommandTextChanged(null, commandText);
        }

        /// <summary>
        /// Constructor with command text and connection
        /// </summary>
        public VOracleCommand(string commandText, OracleConnection connection) : base(commandText, connection)
        {
            OnCommandTextChanged(null, commandText);
        }

        /// <summary>
        /// Constructor with command text, connection, and transaction
        /// </summary>
        public VOracleCommand(string commandText, OracleConnection connection, OracleTransaction transaction)
            : base(commandText, connection)
        {
            Transaction = transaction;
            OnCommandTextChanged(null, commandText);
        }

        /// <summary>
        /// Constructor with command text and transaction (uses transaction.Connection)
        /// </summary>
        public VOracleCommand(string commandText, OracleTransaction transaction)
            : base(commandText, transaction.Connection)
        {
            Transaction = transaction;
            OnCommandTextChanged(null, commandText);
        }

        /// <summary>
        /// Constructor with command text and VOracleTransaction
        /// </summary>
        public VOracleCommand(string commandText, VOracleTransaction transaction)
            : base(commandText, transaction.Inner.Connection)
        {
            Transaction = transaction.Inner;
            OnCommandTextChanged(null, commandText);
        }

        /// <summary>
        /// Executes ExecuteReader and wraps result in VOracleDataReader
        /// </summary>
        public VOracleDataReader ExecuteReaderWrapped()
        {
            try
            {
                return new VOracleDataReader((OracleDataReader)ExecuteReader());
            }
            catch (OracleException ex)
            {
                throw new VOracleException(ex);
            }
        }

        public override object ExecuteScalar()
        {
            try
            {
                return base.ExecuteScalar();
            }
            catch (OracleException ex)
            {
                throw new VOracleException(ex);
            }
        }

        public override int ExecuteNonQuery()
        {
            try
            {
                return base.ExecuteNonQuery();
            }
            catch (OracleException ex)
            {
                throw new VOracleException(ex);
            }
        }

        protected override System.Data.Common.DbDataReader ExecuteDbDataReader(System.Data.CommandBehavior behavior)
        {
            try
            {
                return base.ExecuteDbDataReader(behavior);
            }
            catch (OracleException ex)
            {
                throw new VOracleException(ex);
            }
        }

        /// <summary>
        /// When true, Parameters collection is populated when CommandText is set (ODP.NET BindByName behavior).
        /// </summary>
        public bool ParameterCheck
        {
            get => BindByName;
            set => BindByName = value;
        }

        /// <summary>
        /// Raises the CommandTextChanged event
        /// </summary>
        protected virtual void OnCommandTextChanged(string oldValue, string newValue)
        {
            DevUtilsProvider.Instance.AnalyzeCmdSql(newValue);
            CommandTextChanged?.Invoke(this, new CommandTextChangedEventArgs(oldValue, newValue));
        }
    }

    /// <summary>
    /// Event arguments for CommandTextChanged event
    /// </summary>
    public class CommandTextChangedEventArgs : EventArgs
    {
        public string OldValue { get; }
        public string NewValue { get; }

        public CommandTextChangedEventArgs(string oldValue, string newValue)
        {
            OldValue = oldValue;
            NewValue = newValue;
        }
    }
}
