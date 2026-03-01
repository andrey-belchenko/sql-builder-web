using System;
using System.Collections.Generic;
using System.Threading;
using System.ComponentModel;

namespace infoenergo.sys
{
    /// <summary>
    /// Глобальный класс настроек приложения, содержит глобальные св-ва, в которых хранятся значимые для работы приложения данные
    /// </summary>
    public static class Global
    {
        private static sql.builder.Clean.VOracleConnection _defaultConnection;

        /// <summary>
        /// Per-request connection for web/async context. When set, Connection getter returns this instead of _defaultConnection.
        /// </summary>
        public static readonly AsyncLocal<sql.builder.Clean.VOracleConnection> RequestConnection = new AsyncLocal<sql.builder.Clean.VOracleConnection>();

        public static sql.builder.Clean.VOracleConnection Connection
        {
            get => RequestConnection.Value ?? _defaultConnection;
            set => _defaultConnection = value;
        }
    }
}
