using System;
using System.Collections.Generic;
using Devart.Data.Oracle;
using System.ComponentModel;

namespace infoenergo.sys
{
    /// <summary>
    /// Глобальный класс настроек приложения, содержит глобальные св-ва, в которых хранятся значимые для работы приложения данные
    /// </summary>
    public static class Global
    {
        public static OracleConnection Connection = null;
    }
}
