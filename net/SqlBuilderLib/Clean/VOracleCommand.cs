using System;
using System.Data;
using System.Data.Common;
using Devart.Data.Oracle;

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
            : base(commandText, connection, transaction)
        {
            OnCommandTextChanged(null, commandText);
        }

        /// <summary>
        /// Raises the CommandTextChanged event
        /// </summary>
        protected virtual void OnCommandTextChanged(string oldValue, string newValue)
        {
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
