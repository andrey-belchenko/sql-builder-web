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
        ///// <summary>
        ///// Oracle Direct Mode
        ///// </summary>
        //public static bool DirectMode
        //{
        //    get
        //    {
        //        return infoenergo.framework.Global2.DirectMode == true;
        //    }
        //    set
        //    {
        //        infoenergo.framework.Global2.DirectMode = value;
        //    }
        //}

        ///// <summary>
        ///// Путь к файлу tnsnames.ora
        ///// </summary>
        //public static string TnsNamesPath
        //{
        //    get
        //    {
        //        return infoenergo.framework.Global2.TnsNamesPath;
        //    }
        //    set
        //    {
        //        infoenergo.framework.Global2.TnsNamesPath = value;
        //    }
        //}
        ///// <summary>
        ///// Конструктор по-умолчванию
        ///// </summary>
        //static Global()
        //{
        //    if (LicenseManager.UsageMode == LicenseUsageMode.Designtime
        //        || System.Diagnostics.Process.GetCurrentProcess().ProcessName.ToLower().Contains("devenv"))
        //        return;
        //}

        //#region Кэш прикладных переменных
        ///// <summary>
        ///// <para>Возвращает или принудительно задает Схему, в которой расположена ASUSE.</para>
        ///// <para>Если же вы хотите получить ссылку на схему из таблицы RS_ESYS, следует воспользоваться методом <see cref="infoenergo.sys.Global.GetSchema"/>.</para>
        ///// </summary>
        //public static string Schema
        //{
        //    set
        //    {
        //        schema = value;
        //    }
        //    get
        //    {
        //        return schema;
        //    }
        //}

        ///// <summary>
        ///// Возвращает Схему, в которой расположена ASUSE, из поля USERNAME таблицы RS_ESYS, полагаясь при этом на PUBLIC SYNONYM.
        ///// </summary>
        ///// <returns>Значение поля  USERNAME таблицы RS_ESYS</returns>
        //public static string GetSchema()
        //{
        //    string result = "";
        //    if (_connection != null)
        //    {
        //        result = CoreSql.SqlGetString("SELECT username FROM rs_esys", _connection);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// обеспечивает проперть Version
        ///// </summary>
        //private static string _version = null;

        ///// <summary>
        ///// <para>Возвращает версию программы в БД (rs_esys.version).</para>
        ///// <para>В Centura - String: sversion</para>
        ///// </summary>
        //public static string Version
        //{
        //    get
        //    {
        //        // если версия не проинициализирована
        //        if (_version == null)
        //        {
        //            if (_connection != null)
        //            {
        //                try //на случай отсутствия таблицы rs_esys в базе (например, TGK/SALES)
        //                {
        //                    if (schema != null && schema.Length > 0)
        //                        _version = CoreSql.SqlGetString("SELECT version FROM " + schema + ".rs_esys", _connection);
        //                    else
        //                        _version = CoreSql.SqlGetString("SELECT version FROM rs_esys", _connection);
        //                }
        //                catch { _version = "1.0.0.0"; }
        //            }
        //        }
        //        // вместо null всегда возвращаем пустую строку.
        //        if (_version == null)
        //        {
        //            return string.Empty;
        //        }
        //        else
        //            return _version;
        //    }
        //    set
        //    {
        //        _version = value;
        //    }
        //}

        ///// <summary>
        ///// Код энергосистемы (rs_esys.kod_esys)
        ///// В Centura - Number: nglob_esys
        ///// </summary>
        //public static double? Kod_esys
        //{
        //    get
        //    {
        //        object result = null;
        //        if (kod_esys == null)
        //        {
        //            try
        //            {
        //                if (_connection != null)
        //                    if (schema != null && schema.Length > 0)
        //                        result = CoreSql.SqlGetValue("SELECT kod_esys FROM " + schema + ".rs_esys", null, _connection);
        //                    else
        //                        result = CoreSql.SqlGetValue("SELECT kod_esys FROM rs_esys", null, _connection);
        //            }
        //            catch { result = null; } //если таблицы rs_esys нет в базе.
        //            if (result == null || result == DBNull.Value)
        //                kod_esys = null;
        //            else
        //                kod_esys = Convert.ToDouble(result);
        //        }
        //        return kod_esys; // до 13.04.2012 возвращалось значение, либо 0
        //    }
        //}

        ///// <summary>
        ///// Код глобального отделения (kr_org.kodp)
        ///// В Centura - Number: nglob_podr
        ///// </summary>
        //public static int? DepartmentKodp { get; set; }


        ///// <summary>
        ///// Название глобального отделения сокращенно
        ///// В Centura - String: sglob_podr
        ///// </summary>
        //public static string DepartmentShortName { get; set; }

        ///// <summary>
        ///// Название глобального отделения полностью
        ///// В Centura - Long String: sglob_fullp
        ///// </summary>
        //public static string DepartmentName { get; set; }

        ///// <summary>
        ///// Код подразделения
        ///// </summary>
        //public static int? PodrKodp { get; set; }

        ///// <summary>
        ///// Глобальный отчетный период, заполняется при старте программы по выбранному отделению (kr_calc.ym)
        ///// В Centura - Number: nglob_ym
        ///// </summary>
        //public static double? Ym { get; set; }

        ///// <summary>
        ///// Юр. лицо - абонент (kr_payer.kodp)
        ///// В Centura - Number: nglob_kodp
        ///// </summary>
        //public static int? Kodp { get; set; }

        ///// <summary>
        ///// № абонента (kr_payer.nump)
        ///// В Centura - String: snump
        ///// </summary>
        //public static string Nump { get; set; }

        ///// <summary>
        ///// Договор (kr_dogovor.kod_dog)
        ///// В Centura - Number: nglob_kod_dog
        ///// </summary>
        //public static int? Kod_dog { get; set; }

        ///// <summary>
        ///// № договора (kr_dogovor.ndog)
        ///// В Centura - String: sndog
        ///// </summary>
        //public static string Ndog { get; set; }

        ///// <summary>
        ///// Текущий потребитель договора (kr_numobj.kod_numobj)
        ///// В Centura - Number: nglob_kod_numobj
        ///// </summary>
        //public static int? Kod_numobj { get; set; }

        ///// <summary>
        ///// № объекта (kr_numobj.num_obj)
        ///// В Centura - String: snum_obj
        ///// </summary>
        //public static string Num_obj { get; set; }

        ///// <summary>
        ///// Наименование объекта (kr_numobj.name)
        ///// В Centura - String: sname_obj
        ///// </summary>
        //public static string ObjectName { get; set; }

        ///// <summary>
        ///// Объект потребителя (kr_object.kod_obj)
        ///// В Centura - Number: nglob_kod_obj
        ///// </summary>
        //public static int? Kod_obj { get; set; }

        ///// <summary>
        ///// ТУ (%r_point.kod_point)
        ///// В Centura - Number: nglob_kod_point
        ///// </summary>
        //public static int? Kod_point { get; set; }

        ///// <summary>
        ///// Наименование ТУ (%r_point.name)
        ///// В Centura - String: s_glob_point
        ///// </summary>
        //public static string PointName { get; set; }

        ///// <summary>
        ///// подразделение по умолчанию конкретного юзера (kr_org, ks_user ???)
        ///// В Centura - Number: nglob_podr_user
        ///// </summary>
        //[Obsolete("Use Kod_Emp instead.")]
        //[EditorBrowsable(EditorBrowsableState.Never)]
        //public static int? PodrUserKodp
        //{
        //    get
        //    {
        //        return Kod_Emp;
        //    }
        //    set
        //    {
        //        Kod_Emp = value;
        //    }
        //} //TODO блин, оно должно было называться PodrUserKod_emp или Kod_emp

        ///// <summary>
        ///// подразделение по умолчанию конкретного юзера (kr_org, ks_user ???)
        ///// В Centura - Number: nglob_podr_user
        ///// </summary>
        //public static int? Kod_Emp // TODO Кириллову: тут надо однозначно double делать / sklubowicz
        //{
        //    get
        //    {
        //        if (kod_emp.HasValue)
        //            return kod_emp;
        //        else
        //        {
        //            object result;
        //            if (_connection != null)
        //            {
        //                result = CoreSql.SqlGetValue(string.Format(@"SELECT emp.kod_emp FROM kr_employee emp INNER JOIN ks_user u ON emp.kod_emp  = u.kod_emp WHERE UPPER(u.puser) = USER", _connection.UserId), null, _connection);
        //                if (result == null || result == DBNull.Value)
        //                    kod_emp = null;
        //                else
        //                    kod_emp = Convert.ToInt32(result);
        //            }
        //            return kod_emp;
        //        }
        //    }
        //    set { kod_emp = value; }
        //} // заполнять так: SELECT b.fio, a.kod_emp FROM kr_employee b  LEFT OUTER JOIN ks_user a ON a.kod_emp  = b.kod_emp WHERE upper(a.puser) = upper(:SqlUser) ")

        ///// <summary>
        ///// ФИО текущего пользователя
        ///// </summary>
        //public static string UserName
        //{
        //    get
        //    {
        //        if (userName != null && userName.Length > 0)
        //            return userName;
        //        else
        //        {
        //            double? kod_emp_ = Kod_Emp;
        //            if (kod_emp_.HasValue)
        //            {
        //                if (_connection != null)
        //                {
        //                    string sql = string.Format("SELECT e.fio FROM kr_employee e WHERE e.kod_emp = {0}", kod_emp_.Value.ToString());
        //                    userName = CoreSql.SqlGetString(sql, _connection); // TODO заменить на запрос с биндом

        //                }
        //            }
        //            return userName;
        //        }
        //    }
        //    set { userName = value; }
        //}

        ///// <summary>
        ///// признак эл.(0)тепл.(1) потребителя
        ///// В Centura - Number: nglob_tep_el
        ///// </summary>
        //public static int? Tep_el { get; set; }

        ///// <summary>
        ///// Начальный адрес для окна поиска адреса (adr_m.kod_m)
        ///// В Centura - Number: nglob_kod_m
        ///// </summary>
        //public static int? Kod_m { get; set; }

        //private static infoenergo.data.asuse.RG_GLOB_SELECT_RS_ESYS.TypedCursor rs_esys_record = null;
        ///// <summary>
        ///// Запись из таблицы RS_ESYS. Заполняется один раз по глобальному соединению с базой infoenergo.sys.Global.Connection, далее сидит в кеше.
        ///// </summary>
        //public static infoenergo.data.asuse.RG_GLOB_SELECT_RS_ESYS.TypedCursor RS_ESYS
        //{
        //    get
        //    {
        //        if (rs_esys_record == null)
        //            rs_esys_record = infoenergo.data.appgeneral.Asuse.GetRS_ESYS(Global.Connection);
        //        return rs_esys_record;
        //    }
        //}
        ///// <summary>
        ///// Текущие параметры БД
        ///// </summary>
        //public static TnsEntry CurrentEntry
        //{
        //    get
        //    {
        //        return infoenergo.framework.Global2.CurrentEntry;
        //    }
        //    set
        //    {
        //        infoenergo.framework.Global2.CurrentEntry = value;
        //    }
        //}
        ///// <summary>
        ///// Все источники данных
        ///// </summary>
        //public static List<TnsEntry> AllEnries
        //{
        //    get
        //    {
        //        return infoenergo.framework.Global2.AllEnries;
        //    }
        //    set
        //    {
        //        infoenergo.framework.Global2.AllEnries = value;
        //    }
        //}

        //#endregion

        //#region Oracle connection
        //static remotedata _RemoteConnection;
        ///// <summary>
        ///// Попытка возврата подключения к БД
        ///// </summary>
        //public static remotedata RemoteConnection
        //{
        //    get
        //    {
        //        if (_RemoteConnection != null && _RemoteConnection.ConnectionString == GetConnectionString)
        //            return _RemoteConnection;
        //        _RemoteConnection = new remotedata(GetConnectionString);
        //        return _RemoteConnection;
        //    }
        //    set
        //    {
        //        _RemoteConnection = value;
        //    }
        //    #region old
        //    /*rget
        //    {
        //        emotedata result = null;
        //        // попытка дернуть из нулевой, главной формы указатель на подключение к базе
        //        if (Application.OpenForms.Count > 0)
        //        {
        //            if (Application.OpenForms[0] is BaseXtraForm)
        //                result = (Application.OpenForms[0] as BaseXtraForm).Connection;
        //            else
        //                if (Application.OpenForms[0] is BaseForm)
        //                    result = (Application.OpenForms[0] as BaseForm).Connection;
        //        }
        //         return result;
        //    }*/
        //    #endregion
        //}

        ////private static OracleConnection _connection = null;
        //private static OracleConnection _connection
        //{
        //    get
        //    {
        //        return infoenergo.framework.connection.CurrentConnection;
        //    }
        //    set
        //    {
        //        infoenergo.framework.connection.CurrentConnection = value;
        //    }
        //}
        ///// <summary>
        ///// Глобальное кэшированное подключение к базе данных. Его надо запоминать сразу после соединения с базой данных
        ///// </summary>
        ///// 

        public static OracleConnection Connection = null;


        //private static OracleTransaction _transaction = null;
        ///// <summary>
        ///// Глобальный объект, по которому можно проводить транзакции внутри глобального соединения <see cref="infoenergo.sys.Global.Connection"/>
        ///// </summary>
        //public static OracleTransaction Transaction
        //{
        //    get { return _transaction; }
        //}

        ///// <summary>
        ///// Начинает транзакцию внутри соединения <see cref="infoenergo.sys.Global.Connection"/> и кладет ее в <see cref="infoenergo.sys.Global.Transaction"/>
        ///// </summary>
        ///// <returns>Ссылка на созданную транзакцию. Она же указыает на <see cref="infoenergo.sys.Global.Transaction"/></returns>
        //public static OracleTransaction BeginTransaction()
        //{
        //    if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
        //    {
        //        _transaction = _connection.BeginTransaction();
        //        return _transaction;
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Возвращает текущую или создает новую глобальную транзакцию
        ///// </summary>
        ///// <returns>Объект-транзакция <see cref="infoenergo.sys.Global.Transaction"/></returns>
        //public static OracleTransaction GetOrBeginTransaction()
        //{
        //    if (_connection != null && _connection.State == System.Data.ConnectionState.Open)
        //    {
        //        if (_transaction == null || _transaction.Connection == null)
        //        {
        //            BeginTransaction();
        //        }
        //        return _transaction;
        //    }
        //    return null;
        //}

        ///// <summary>
        ///// Соединяется с базой данных и запоминает соединение в <see cref="infoenergo.sys.Global.Connection"/>
        ///// </summary>
        ///// <param name="user">Пользователь</param>
        ///// <param name="password">Пароль</param>
        ///// <param name="dataSource">База данных (tnsname)</param>
        ///// <returns>true, если успех</returns>
        //public static bool Connect(string user, string password, string dataSource, string serviceName = null, string sid = null)
        //{
        //    bool result = false;
        //    string connnectionString = BuildConnectionString(user, password, dataSource, serviceName, sid);
        //    _connection = new OracleConnection(connnectionString);
        //    try
        //    {
        //        _connection.Open(useGlobalSettings: true);
        //        if (_connection.State == System.Data.ConnectionState.Open)
        //        {
        //            //infoenergo.framework.DataHelper.setNlsDateFormat(_connection);
        //            result = true;
        //        }
        //    }
        //    catch (OracleException ex)
        //    {
        //        MessageBox.Show("Не получилось соединиться с базой данных: " + ex.Message, "Ошибка соединения", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    return result;
        //}
        ///// <summary>
        ///// Соединяется с базой данных и запоминает соединение в <see cref="infoenergo.sys.Global.Connection"/>
        ///// </summary>
        ///// <param name="user">Пользователь</param>
        ///// <param name="password">Пароль</param>
        ///// <param name="entry">Данные сервера БД</param>
        //public static bool Connect(string user, string password, TnsEntry entry)
        //{
        //    bool result = false;
        //    string connnectionString = BuildConnectionString(user, password, entry);
        //    _connection = new OracleConnection(connnectionString);
        //    try
        //    {
        //        _connection.Open(useGlobalSettings: true);
        //        if (_connection.State == System.Data.ConnectionState.Open)
        //        {
        //            result = true;
        //            Global.CurrentEntry = entry; //ВАЖНО. Если успещно, то считаем этот коннект главным
        //        }
        //    }
        //    catch (OracleException ex)
        //    {
        //        MessageBox.Show("Не получилось соединиться с базой данных: " + ex.Message, "Ошибка соединения", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    return result;
        //}

        ///// <summary>
        ///// Строит и возвращает строку соединения по логину, паролю и алиасу базы
        ///// </summary>
        ///// <param name="user">Пользователь</param>
        ///// <param name="password">Пароль</param>
        ///// <param name="dataSource">База данных</param>
        ///// <returns>Строка соединения</returns>
        //public static string BuildConnectionString(string user, string password, string dataSource, string serviceName = null, string sid = null)
        //{
        //    return infoenergo.framework.Global2.BuildConnectionString(user, password, dataSource, serviceName, sid);
        //}
        ///// <summary>
        ///// Строит и возвращает строку соединения по логину, паролю и алиасу базы
        ///// </summary>
        ///// <param name="user">Пользователь</param>
        ///// <param name="password">Пароль</param>
        ///// <param name="entry">Данные сервера БД</param>
        ///// <returns>Строка соединения</returns>
        //public static string BuildConnectionString(string user, string password, TnsEntry entry)
        //{
        //    return infoenergo.framework.Global2.BuildConnectionString(user, password, entry);
        //}
        ///// <summary>
        ///// Возвращает атрибуты подключения по алиасу сервера БД
        ///// </summary>
        ///// <param name="dataSource">Интересующий источник данных</param>
        ///// <returns></returns>
        //public static TnsEntry FindEntry(string dataSource)
        //{
        //    return infoenergo.framework.Global2.FindEntry(dataSource);
        //}

        ///// <summary>
        ///// Возвращает ConnectionString - строку текущего соединения к базе
        ///// </summary>
        //public static string GetConnectionString
        //{
        //    get
        //    {
        //        return infoenergo.framework.Global2.GetConnectionString;
        //    }
        //}

        ///// <summary>
        ///// Возвращает пользователя, которым он авторизован на данный момент в базе данных
        ///// </summary>
        //public static string User
        //{
        //    get
        //    {
        //        if (_connection != null)
        //            return _connection.UserId;
        //        else
        //            return null;
        //    }
        //}

        ///// <summary>
        ///// <para>Устанавливает текущую схему в сессии oracle, т.е. выполняет ALTER SESSION SET CURRENT_SCHEMA = ...</para>
        ///// <para>По умолчанию (если параметр <paramref name="schema"/> не задан) берет схему поля USER из таблицы RS_ESYS</para>
        ///// </summary>
        ///// <param name="schema">имя схемы, например "ASUSE"</param>
        //public static void SetCurrentEsysSchema(string schema = null)
        //{
        //    string newSchema = schema ?? Global.GetSchema();

        //    if (!string.IsNullOrEmpty(newSchema))
        //        infoenergo.core.Data.DataHelper.SetCurrentSchema(newSchema, Global.Connection);
        //}
        //#endregion;

        //#region Права на меню и прочие объекты приложений
        ///// <summary>
        ///// Содержит в себе разрешения на запуск меню приложения (и в перспективе форм)
        ///// </summary>
        //[Obsolete("Этот класс еще никем не использовался и скоро переедет в infoenergo.core")]
        //public static class Security
        //{
        //    /// <summary>
        //    /// Права пользователя на главное меню (в перспективе - и на другие объекты приложения - кнопки, формы и т.п.)
        //    /// </summary>
        //    public static Dictionary<double, Permission> Menu = new Dictionary<double, Permission>();

        //    /// <summary>
        //    /// <para>Загружает разрешения пользователя <param name="user"></param> на меню из базы (из таблицы RR_USERMENU)</para>
        //    /// <para>возможно, это надо перенести в infoenergo.framework, либо добавить extension method?</para>
        //    /// </summary>
        //    public static bool LoadPermissions(string user)
        //    {
        //        bool result = false;
        //        string sql = "SELECT * FROM rr_usermenu WHERE puser = UPPER(:USERNAME)";
        //        OracleCommand cmd = new OracleCommand(sql, _connection);
        //        cmd.Parameters.Add(new OracleParameter("USERNAME", user));
        //        try
        //        {
        //            using (OracleDataReader reader = cmd.ExecuteReader())
        //            {
        //                while (reader.Read())
        //                {
        //                    try
        //                    {
        //                        Menu.Add(Convert.ToDouble(reader["KOD_MENU"]), (Permission)Convert.ToInt32(reader["KOD_RIGHTS"]));
        //                    }
        //                    catch (Exception ex)
        //                    {
        //                        MessageBox.Show(String.Format("Ошибка при чтении прав на меню: {0}, код меню={1}, права={2}", ex.Message, reader["KOD_MENU"].ToString(), reader["KOD_RIGHTS"].ToString()),
        //                            "Права на меню",
        //                            MessageBoxButtons.OK,
        //                            MessageBoxIcon.Error
        //                        );
        //                    }
        //                }
        //                if (reader.HasRows)
        //                {
        //                    result = true;
        //                }
        //            }
        //        }
        //        catch (OracleException ex)
        //        {
        //            // показываем стандартный диалог об ошибке
        //            OracleSqlExceptionHandler.HandleException(new OracleSqlException(string.Format("Не удалось прочитать права пользователя {0} на меню: ", user) + ex.Message, ex, sql));
        //        }
        //        cmd.Dispose();
        //        return result;
        //    }

        //    /// <summary>
        //    /// Проверяет, есть ли у текущего пользователя разрешение <paramref name="permission"/> на объект (меню) с кодом <paramref name="objectId"/>
        //    /// </summary>
        //    /// <param name="objectId">Код объекта (меню)</param>
        //    /// <param name="permission">Разрешение</param>
        //    /// <returns>true, если разрешение есть; false, если нет.</returns>
        //    public static bool HasPermission(double objectId, Permission permission)
        //    {
        //        bool result = false;
        //        Permission foundPermission;
        //        if (Menu != null && Menu.TryGetValue(objectId, out foundPermission))
        //        {
        //            if (foundPermission >= permission)
        //                result = true;
        //        }
        //        return result;
        //    }

        //    /// <summary>
        //    /// <para>Применить разрешения на объекты к пунктам меню <paramref name="items"/>.</para>
        //    /// <para>Разрешения для текущего пользователя уже должны быть загружены к этому моменту методом Global.Security.LoadPermissions(Global.User).</para>
        //    /// </summary>
        //    /// <param name="items">Коллекция элементов меню</param>
        //    public static void ApplyPermissionsToBarItems(DevExpress.XtraBars.BarItems items)
        //    {
        //        foreach (DevExpress.XtraBars.BarItem item in items)
        //        {
        //            double requiredId = 0;
        //            Permission requiredPermission = Permission.None;
        //            bool permit = false; // по умолчанию запрещено
        //            // достаем из тэга в менюшке требуемое разрешение
        //            if (item.Tag != null)
        //            {
        //                if (TryParseTag(item.Tag.ToString(), out requiredId, out requiredPermission))
        //                {
        //                    // сравниваем требуемое разрешение с тем, что есть у пользователя
        //                    if (HasPermission(requiredId, requiredPermission))
        //                    {
        //                        permit = true;
        //                    }
        //                }
        //            }
        //            else
        //                permit = true; // если Tag вообще не выставлен, то считаем, что меню разрешено всегда
        //            item.Enabled = permit;
        //            //item.Links
        //        }
        //    }

        //    /// <summary>
        //    /// Пока не работает :(
        //    /// Проблема в том, что на событии FormLoad коллекцтй itemLinks в баре почему-то еще пустая
        //    /// </summary>
        //    /// <param name="menuBar"></param>
        //    public static void ApplyPermissionsToBar(DevExpress.XtraBars.Bar menuBar)
        //    {
        //        var t = menuBar.ItemLinks[0].OwnerItem;
        //        foreach (DevExpress.XtraBars.BarItemLink itemLink in menuBar.ItemLinks)
        //        {
        //            DevExpress.XtraBars.BarItem item = itemLink.Item;
        //            double requiredId = 0;
        //            Permission requiredPermission = Permission.None;
        //            bool permit = false; // по умолчанию запрещено
        //            // достаем из тэга в менюшке требуемое разрешение
        //            if (item.Tag != null)
        //            {
        //                if (TryParseTag(item.Tag.ToString(), out requiredId, out requiredPermission))
        //                {
        //                    // сравниваем требуемое разрешение с тем, что есть у пользователя
        //                    if (HasPermission(requiredId, requiredPermission))
        //                    {
        //                        permit = true;
        //                        itemLink.OwnerItem.Enabled = true;
        //                    }
        //                }
        //            }
        //            else
        //                permit = true; // если Tag вообще не выставлен, то считаем, что меню разрешено всегда
        //            item.Enabled = permit;
        //            //item.Links
        //        }
        //    }


        //    /// <summary>
        //    /// Пытается разобрать строку <paramref name="tag"/>. Строка должна быть вида [1234;1], где 1234 - код меню, 1 - право (0 нет прав, 1 - чтение, 2 - запись)
        //    /// </summary>
        //    /// <param name="tag">Строка вида [1234;1], где 1234 - код меню, 1 - право (0 нет прав, 1 - чтение, 2 - запись)</param>
        //    /// <param name="id">Полученный код меню</param>
        //    /// <param name="permission">Полученное разрешение</param>
        //    /// <returns>true, если удалось разобрать оба числа; false если не получилось или строка tag пустая</returns>
        //    private static bool TryParseTag(string tag, out double id, out Permission permission)
        //    {
        //        bool result = false;
        //        id = 0;
        //        permission = Permission.None;
        //        if (tag != null)
        //        {
        //            if (tag.Length > 0)
        //            {
        //                System.Text.RegularExpressions.Match match = System.Text.RegularExpressions.Regex.Match(tag, @"^[(\d+)(;|,)(\d{1,1})]$");
        //                if (match.Groups.Count == 4)
        //                {
        //                    id = Convert.ToDouble(match.Groups[1].ToString());
        //                    permission = (Permission)Convert.ToDouble(match.Groups[3].ToString());
        //                    result = true;
        //                }
        //            }
        //        }
        //        return result;
        //    }

        //    /// <summary>
        //    /// Прикладные разрешения для меню, форм и других объектов приложения (нет прав, чтение, запись)
        //    /// </summary>
        //    public enum Permission
        //    {
        //        /// <summary>
        //        /// Нет прав
        //        /// </summary>
        //        None = 0,
        //        /// <summary>
        //        /// Только на чтение
        //        /// </summary>
        //        Read = 1,
        //        /// <summary>
        //        /// Запись и Чтение
        //        /// </summary>
        //        Write = 2
        //    };
        //}
        //#endregion

        //#region ссылка на главную форму приложения
        //private static System.Windows.Forms.Form mainForm = null;

        ///// <summary>
        ///// Ссылка на главную форму приложения. Заполняет сам программист при создании главной формы приложения.
        ///// </summary>
        //public static System.Windows.Forms.Form MainForm
        //{
        //    get { return mainForm; }
        //    set
        //    {
        //        mainForm = value;
        //        mainForm.FormClosed += mainForm_FormClosed;
        //    }
        //}

        //// при закрытии главной формы очищаем ссылку на нее
        //private static void mainForm_FormClosed(object sender, FormClosedEventArgs e)
        //{
        //    mainForm.FormClosed -= mainForm_FormClosed;
        //    mainForm = null;
        //}


        //#endregion

        ///// <summary>
        ///// Возвращает полное описание приложения (с версией, базой, пользователем и доп.информацией)
        ///// </summary>
        ///// <param name="appDescription">Описание приложения для заголовка</param>
        ///// <param name="additionalInfo">Дополнительная информация (например, текущий период)</param>
        ///// <returns></returns>
        //public static string GetApplictionCaption(string appDescription, string additionalInfo = "")
        //{
        //    return infoenergo.ui.win.Forms.FormBase.FormatApplicationCaption(appDescription,
        //            (CurrentEntry != null ? CurrentEntry.ShortName : Connection.GetAlias()),
        //            (UserName != null ? UserName : Connection.UserId),
        //            Version,
        //            additionalInfo
        //           );
        //}

        //#region skins
        ///// <summary>
        ///// Устанавливает либо скин ИНФОЭНЕРГО, либо для некоторых праздников - соответствующий скин
        ///// </summary>
        //public static void SetDefaultSkin()
        //{
        //    DevExpress.Skins.SkinManager.Default.RegisterAssembly(typeof(DevExpress.UserSkins.infoenergoskins).Assembly); // register skin infoenergo
        //    DevExpress.Skins.SkinManager.EnableFormSkins();
        //    DevExpress.Skins.SkinManager.EnableMdiFormSkins();
        //    setDefaultTodaySkin();
        //}

        ///// <summary>
        ///// Возвращает имя скина для сегодняшней даты
        ///// </summary>
        ///// <returns>имя скина</returns>
        //public static string GetTodaySkinName()
        //{
        //    int month = DateTime.Today.Month;
        //    int day = DateTime.Today.Day;
        //    string skinName = "infoenergo"; // скин по умолчанию ИНФОЭНЕРГО
        //    // Halloween
        //    //if (month == 10 && day >= 30)
        //    //    skinName = "Pumpkin";
        //    //// Новый год
        //    //if (month == 12 && day >= 28)
        //    //    skinName = "Xmas 2008 Blue";
        //    //// 8 марта
        //    //else if (month == 3 && day >= 7 && day <= 8)
        //    //    skinName = "Springtime";
        //    // день св. Валентина
        //    //else if (month == 2 && day == 14)
        //    //    skinName = "Valentine"; // слишком тяжелый
        //    return skinName;
        //}

        //// устанавливает скин по умолчанию
        //private static void setDefaultTodaySkin()
        //{
        //    string skinName = GetTodaySkinName();
        //    DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle(skinName);

        //}
        //#endregion skins

        ///// <summary>
        ///// Возвращает true, если пользователь работает в домене разработчика (ИНФОЭНЕРГО)
        ///// </summary>
        ///// <returns>true, если пользователь работает в домене разработчика (ИНФОЭНЕРГО)</returns>
        //public static bool IsInDeveloperDomain()
        //{
        //    return (System.Environment.UserDomainName.ToUpper() == "INFOENERGO"
        //        || System.Environment.UserDomainName.ToUpper() == "NECR-PC" // комп Игнатьевского в Cofite. Странно, но говорит, что так.
        //        );
        //}

        //#region privates
        ///// <summary>
        ///// обеспечивает проперть Schema
        ///// </summary>
        //private static string schema = null;
        ///// <summary>
        ///// Обеспечивает проперть Kod_esys
        ///// </summary>
        //private static double? kod_esys = null;

        ///// <summary>
        ///// обеспечивает проперть Kod_emp
        ///// </summary>
        //private static int? kod_emp;

        ///// <summary>
        ///// Обеспечивает проперть UserName
        ///// </summary>
        //private static string userName;

    }
}
