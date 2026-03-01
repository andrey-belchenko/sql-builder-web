#region Assembly infoenergo.core, Version=1.0.0.0, Culture=neutral, PublicKeyToken=97d3c227515ca15e
// C:\WINDOWS\Microsoft.Net\assembly\GAC_MSIL\infoenergo.core\v4.0_1.0.0.0__97d3c227515ca15e\infoenergo.core.dll
// Decompiled with ICSharpCode.Decompiler 9.1.0.7988
#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.Common;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using Devart.Data.Oracle;
using SqlBuilderLib.DevTools;
using sql.builder.Clean;


namespace infoenergo.core.Data
{
    public static class DataHelper
    {
        public static object SqlGetValue(string sql, VOracleParameter[] parameters, VOracleConnection connection, bool analyze = true)
        {
            object result = null;
            VOracleCommand oracleCommand = new VOracleCommand(sql, connection);
            try
            {
                if (parameters != null && parameters.Length != 0)
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).AddRange((Array)parameters);
                }

                try
                {
                    if (analyze)
                    {
                        DevUtilsProvider.Instance.AnalyzeExecSql(sql);
                    }
                    result = ((DbCommand)(object)oracleCommand).ExecuteScalar();
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sql, oracleCommand.Parameters);
                }
                finally
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).Clear();
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return result;
        }

        public static object SqlGetValue1(string sql, VOracleParameter parameter, VOracleConnection connection)
        {
            return SqlGetValue(sql, new VOracleParameter[1] { parameter }, connection);
        }

        public static decimal? SqlGetDecimal(string sql, VOracleParameter[] parameters, VOracleConnection connection)
        {
            object obj = SqlGetValue(sql, parameters, connection);
            try
            {
                if ((obj == null) | (obj == DBNull.Value))
                {
                    return null;
                }

                return Convert.ToDecimal(obj);
            }
            catch (FormatException ex)
            {
                if (parameters != null && parameters.Length != 0)
                {
                    VOracleParameterCollection oracleParameterCollection = new VOracleParameterCollection();
                    ((DbParameterCollection)(object)oracleParameterCollection).AddRange((Array)parameters);
                }

                throw new FormatException($"Запрос вернул не числовое значение:\"{obj}\" \r\n{ex.Message} \r\nSQL-запрос:\r\n{sql}");
            }
        }

        public static decimal? SqlGetDecimal(string sql, VOracleConnection connection, VOracleParameter parameter)
        {
            return SqlGetDecimal(sql, new VOracleParameter[1] { parameter }, connection);
        }

        public static decimal? SqlGetDecimal(string sql, VOracleConnection connection)
        {
            return SqlGetDecimal(sql, new VOracleParameter[0], connection);
        }

        public static string SqlGetString(string sql, VOracleParameter[] parameters, VOracleConnection connection, bool analyze = true)
        {
            object obj = SqlGetValue(sql, parameters, connection, analyze);
            if ((obj == null) | (obj == DBNull.Value))
            {
                return null;
            }

            return (obj != null) ? obj.ToString() : string.Empty;
        }

        public static string SqlGetString(string sql, VOracleConnection connection, VOracleParameter parameter, bool analyze = true)
        {
            return SqlGetString(sql, new VOracleParameter[1] { parameter }, connection, analyze);
        }

        public static string SqlGetString(string sql, VOracleConnection connection, bool analyze = true)
        {
            return SqlGetString(sql, new VOracleParameter[0], connection, analyze);
        }

        public static DateTime? SqlGetDate(string sql, VOracleParameter[] parameters, VOracleConnection connection)
        {
            DateTime? dateTime = null;
            VOracleCommand oracleCommand = new VOracleCommand(sql, connection);
            try
            {
                if (parameters != null && parameters.Length != 0)
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).AddRange((Array)parameters);
                }

                try
                {
                    DevUtilsProvider.Instance.AnalyzeExecSql(sql);
                    using (VOracleDataReader oracleDataReader = oracleCommand.ExecuteReaderWrapped())
                    {
                        if (((DbDataReader)(object)oracleDataReader).Read())
                        {
                            if (!((DbDataReader)(object)oracleDataReader).IsDBNull(0))
                            {
                                dateTime = ((DbDataReader)(object)oracleDataReader).GetDateTime(0);
                                return dateTime;
                            }

                            dateTime = null;
                        }
                    }
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sql, oracleCommand.Parameters);
                }
                catch (FormatException ex)
                {
                    throw new FormatException($"Запрос вернул не дату:\"{dateTime}\" \r\n{ex.Message} \r\nSQL-запрос:\r\n{sql}");
                }
                finally
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).Clear();
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return dateTime;
        }

        public static DateTime? SqlGetDate(string sql, VOracleConnection connection, VOracleParameter parameter)
        {
            return SqlGetDate(sql, new VOracleParameter[1] { parameter }, connection);
        }

        public static DateTime? SqlGetDate(string sql, VOracleConnection connection)
        {
            VOracleParameter[] array = new VOracleParameter[0];
            return SqlGetDate(sql, new VOracleParameter[0], connection);
        }

        public static OracleArray ConvertDecimalArrayToOracle(decimal[] array, VOracleConnection connection, string oracleArrayTypeName = "ASUSETYPES.NUMBER$TABLE", bool forceEmptyArray = false)
        {
            if (!forceEmptyArray && array == null)
            {
                return null;
            }

            OracleArray oracleArray = new OracleArray(oracleArrayTypeName, connection);
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    oracleArray.Add(array[i]);
                }
            }

            return oracleArray;
        }

        public static OracleArray ConvertStringArrayToOracle(string[] array, VOracleConnection connection, string oracleArrayTypeName = "ASUSETYPES.VARCHAR2$TABLE", bool forceEmptyArray = false)
        {
            if (!forceEmptyArray && array == null)
            {
                return null;
            }

            OracleArray oracleArray = new OracleArray(oracleArrayTypeName, connection);
            if (array != null)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    oracleArray.Add(array[i]);
                }
            }

            return oracleArray;
        }

        public static OracleLob SqlGetLob(string sql, VOracleParameter[] parameters, VOracleConnection connection)
        {
            OracleLob result = null;
            VOracleCommand oracleCommand = new VOracleCommand(sql, connection);
            try
            {
                if (parameters != null && parameters.Length != 0)
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).AddRange((Array)parameters);
                }

                try
                {
                    DevUtilsProvider.Instance.AnalyzeExecSql(sql);
                    VOracleDataReader oracleDataReader = oracleCommand.ExecuteReaderWrapped();
                    if (((DbDataReader)(object)oracleDataReader).Read())
                    {
                        result = oracleDataReader.GetOracleLob(0);
                    }
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sql, oracleCommand.Parameters);
                }
                finally
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).Clear();
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return result;
        }

        public static DataTable SqlGetTable(string sql, VOracleParameter[] parameters, VOracleConnection connection, bool analyze = true)
        {
            DataTable dataTable = new DataTable();
            object[] array = null;
            VOracleCommand oracleCommand = new VOracleCommand(sql, connection);
            try
            {
                if (parameters != null && parameters.Length != 0)
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).AddRange((Array)parameters);
                }

                try
                {
                    if (analyze)
                    {
                        DevUtilsProvider.Instance.AnalyzeExecSql(sql);
                    }

                    using (VOracleDataReader oracleDataReader = oracleCommand.ExecuteReaderWrapped())
                    {
                        if (dataTable.Columns.Count == 0)
                        {
                            for (int i = 0; i < ((DbDataReader)(object)oracleDataReader).FieldCount; i++)
                            {
                                dataTable.Columns.Add(((DbDataReader)(object)oracleDataReader).GetName(i), ConvertDataType(((DbDataReader)(object)oracleDataReader).GetFieldType(i)));
                            }
                        }

                        while (((DbDataReader)(object)oracleDataReader).Read())
                        {
                            array = new object[((DbDataReader)(object)oracleDataReader).FieldCount];
                            ((DbDataReader)(object)oracleDataReader).GetValues(array);
                            object[] array2 = new object[((DbDataReader)(object)oracleDataReader).FieldCount];
                            for (int j = 0; j < array.Length; j++)
                            {
                                if (array[j] == DBNull.Value)
                                {
                                    array2[j] = array[j];
                                }
                                else if (IsNumericType(dataTable.Columns[j].DataType))
                                {
                                    array2[j] = Convert.ToDecimal(array[j]);
                                }
                                else
                                {
                                    array2[j] = array[j];
                                }
                            }

                            dataTable.Rows.Add(array2);
                        }

                        dataTable.AcceptChanges();
                    }
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sql, oracleCommand.Parameters);
                }
                finally
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).Clear();
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return dataTable;
        }

        public static DataTable SqlGetTable(string sql, VOracleParameter[] parameters, OracleTransaction transaction)
        {
            DataTable dataTable = new DataTable();
            object[] array = null;
            VOracleCommand oracleCommand = new VOracleCommand(sql, transaction);
            try
            {
                if (parameters != null && parameters.Length != 0)
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).AddRange((Array)parameters);
                }

                try
                {
                    DevUtilsProvider.Instance.AnalyzeExecSql(sql);
                    using (VOracleDataReader oracleDataReader = oracleCommand.ExecuteReaderWrapped())
                    {
                        if (dataTable.Columns.Count == 0)
                        {
                            for (int i = 0; i < ((DbDataReader)(object)oracleDataReader).FieldCount; i++)
                            {
                                dataTable.Columns.Add(((DbDataReader)(object)oracleDataReader).GetName(i), ConvertDataType(((DbDataReader)(object)oracleDataReader).GetFieldType(i)));
                            }
                        }

                        while (((DbDataReader)(object)oracleDataReader).Read())
                        {
                            array = new object[((DbDataReader)(object)oracleDataReader).FieldCount];
                            ((DbDataReader)(object)oracleDataReader).GetValues(array);
                            object[] array2 = new object[((DbDataReader)(object)oracleDataReader).FieldCount];
                            for (int j = 0; j < array.Length; j++)
                            {
                                if (array[j] == DBNull.Value)
                                {
                                    array2[j] = array[j];
                                }
                                else if (IsNumericType(dataTable.Columns[j].DataType))
                                {
                                    array2[j] = Convert.ToDecimal(array[j]);
                                }
                                else
                                {
                                    array2[j] = array[j];
                                }
                            }

                            dataTable.Rows.Add(array2);
                        }

                        dataTable.AcceptChanges();
                    }
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sql, oracleCommand.Parameters);
                }
                finally
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).Clear();
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return dataTable;
        }

        public static DataTable SqlGetTable(string sql, VOracleConnection connection, bool analyze = true)
        {
            return SqlGetTable(sql, null, connection, analyze);
        }

        public static bool SqlExecute(string sqlCommand, VOracleParameter[] parameters, VOracleConnection connection, bool analyze = true)
        {
            bool result = false;
            VOracleCommand oracleCommand = connection.CreateCommand();
            try
            {
                ((DbCommand)(object)oracleCommand).CommandText = sqlCommand;
                if (parameters != null && parameters.Length != 0)
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).AddRange((Array)parameters);
                }

                try
                {
                    if (analyze)
                    {
                        DevUtilsProvider.Instance.AnalyzeExecSql(sqlCommand);
                    }

                    ((DbCommand)(object)oracleCommand).ExecuteNonQuery();
                    result = true;
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sqlCommand, oracleCommand.Parameters);
                }
                finally
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).Clear();
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return result;
        }

        public static bool SqlExecute(string sqlCommand, OracleParameterCollection parameters, VOracleConnection connection)
        {
            bool result = false;
            VOracleCommand oracleCommand = connection.CreateCommand();
            try
            {
                ((DbCommand)(object)oracleCommand).CommandText = sqlCommand;
                if (parameters != null && ((DbParameterCollection)(object)parameters).Count > 0)
                {
                    MoveParameters(parameters, oracleCommand.Parameters);
                }

                try
                {
                    DevUtilsProvider.Instance.AnalyzeExecSql(sqlCommand);
                    ((DbCommand)(object)oracleCommand).ExecuteNonQuery();
                    result = true;
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sqlCommand, oracleCommand.Parameters);
                }
                finally
                {
                    MoveParameters(oracleCommand.Parameters, parameters);
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return result;
        }

        private static void MoveParameters(OracleParameterCollection sourceCollection, OracleParameterCollection destinationCollection)
        {
            DbParameter[] array = new DbParameter[((DbParameterCollection)(object)sourceCollection).Count];
            for (int i = 0; i < ((DbParameterCollection)(object)sourceCollection).Count; i++)
            {
                array[i] = sourceCollection[i];
            }

            DbParameter[] array2 = array;
            foreach (DbParameter value in array2)
            {
                ((DbParameterCollection)(object)sourceCollection).Remove((object)value);
                destinationCollection.Add(value);
            }
        }

        public static bool SqlExecute(string sqlCommand, VOracleParameter[] parameters, CommandType commandType, VOracleConnection connection)
        {
            bool result = false;
            VOracleCommand oracleCommand = connection.CreateCommand();
            try
            {
                ((DbCommand)(object)oracleCommand).CommandText = sqlCommand;
                ((DbCommand)(object)oracleCommand).CommandType = commandType;
                if (parameters != null && parameters.Length != 0)
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).AddRange((Array)parameters);
                }

                try
                {
                    DevUtilsProvider.Instance.AnalyzeExecSql(sqlCommand);
                    ((DbCommand)(object)oracleCommand).ExecuteNonQuery();
                    result = true;
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sqlCommand, oracleCommand.Parameters);
                }
                finally
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).Clear();
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return result;
        }

        public static bool SqlExecute(string sqlCommand, OracleParameterCollection parameters, CommandType commandType, VOracleConnection connection)
        {
            bool result = false;
            VOracleCommand oracleCommand = connection.CreateCommand();
            try
            {
                ((DbCommand)(object)oracleCommand).CommandText = sqlCommand;
                ((DbCommand)(object)oracleCommand).CommandType = commandType;
                if (parameters != null && ((DbParameterCollection)(object)parameters).Count > 0)
                {
                    MoveParameters(parameters, oracleCommand.Parameters);
                }

                try
                {
                    DevUtilsProvider.Instance.AnalyzeExecSql(sqlCommand);
                    ((DbCommand)(object)oracleCommand).ExecuteNonQuery();
                    result = true;
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sqlCommand, oracleCommand.Parameters);
                }
                finally
                {
                    MoveParameters(oracleCommand.Parameters, parameters);
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return result;
        }

        public static bool SqlExecute(string sqlCommand, VOracleParameter[] parameters, CommandType commandType, OracleTransaction transaction)
        {
            bool result = false;
            OracleCommand oracleCommand = (OracleCommand)transaction.Connection.CreateCommand();
            try
            {
                oracleCommand.Transaction = transaction;
                ((DbCommand)(object)oracleCommand).CommandText = sqlCommand;
                ((DbCommand)(object)oracleCommand).CommandType = commandType;
                if (parameters != null && parameters.Length != 0)
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).AddRange((Array)parameters);
                }

                try
                {
                    DevUtilsProvider.Instance.AnalyzeExecSql(sqlCommand);
                    ((DbCommand)(object)oracleCommand).ExecuteNonQuery();
                    result = true;
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException(innerException, sqlCommand, oracleCommand.Parameters);
                }
                finally
                {
                    ((DbParameterCollection)(object)oracleCommand.Parameters).Clear();
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return result;
        }

        public static void SqlExecute(string sqlCommand, VOracleConnection connection)
        {
            VOracleParameter[] parameters = new VOracleParameter[0];
            SqlExecute(sqlCommand, parameters, connection);
        }

        public static void SqlExecutePLSQL(string sqlCommand, VOracleParameter[] parameters, VOracleConnection connection)
        {
            string sqlCommand2 = (Regex.IsMatch(sqlCommand, "^\\s*(DECLARE|BEGIN|UPDATE|INSERT|DELETE|ALTER)", RegexOptions.IgnoreCase) ? sqlCommand : ("BEGIN " + sqlCommand + (Regex.IsMatch(sqlCommand, "\\;\\s*$", RegexOptions.IgnoreCase) ? "" : ";") + " END;"));
            SqlExecute(sqlCommand2, parameters, connection);
        }

        public static void SqlExecutePLSQL(string sqlCommand, VOracleConnection connection)
        {
            VOracleParameter[] parameters = new VOracleParameter[0];
            SqlExecutePLSQL(sqlCommand, parameters, connection);
        }

        public static bool SetCurrentSchema(string schema, VOracleConnection connection)
        {
            bool result = false;
            if (!Regex.IsMatch(schema, "^\\w+$") || schema.Length <= 0)
            {
                throw new ArgumentException("Недопустимое название схемы: " + schema);
            }

            string sqlCommand = "ALTER SESSION SET current_schema=" + schema;
            VOracleCommand oracleCommand = new VOracleCommand(sqlCommand, connection);
            try
            {
                DevUtilsProvider.Instance.AnalyzeExecSql(sqlCommand);
                ((DbCommand)(object)oracleCommand).ExecuteNonQuery();
                result = true;
            }
            catch (Devart.Data.Oracle.OracleException innerException)
            {
                throw new OracleSqlException(innerException, ((DbCommand)(object)oracleCommand).CommandText);
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return result;
        }

        public static Type ConvertDataType(Type type)
        {
            return OracleConvention.ConvertDataType(type);
        }

        public static bool IsNumericType(Type type)
        {
            return OracleConvention.IsNumber(type);
        }

        public static string TranslateOracleException(Exception e)
        {
            string empty = string.Empty;
            if (e is Devart.Data.Oracle.OracleException oraEx)
            {
                switch (oraEx.Code)
                {
                    case 1:
                        return "Значение должно быть уникальным для данного поля!";
                    case 9911:
                        return "Неправильный пароль";
                    case 1017:
                        return "Неправильно введено имя пользователя или пароль";
                    case 1476:
                        return "Деление на ноль";
                    case 1407:
                        return "Данные не введены. Поле является обязательным и не может быть пустым!";
                    case 2291:
                        return "Данные введены неверно (не найдено родительской записи)";
                    case 2292:
                        return "Обнаружены дочерние записи для данного поля!";
                    case 1400:
                        return "Нельзя вставлять пустое значение " + ExtractInfoOra01400(e.Message);
                    case 6564:
                        return "Нет прав на выполнение данной функции";
                    case 942:
                        return "Нет прав на чтение данных";
                    case 904:
                        return "Нет прав чтение или выполнение, либо в запросе указан неправильный идентификатор";
                    case 1031:
                        return "Нет прав";
                    case 54:
                        return "Заблокировано другим пользователем";
                    case 28000:
                        return "Доступ в базу заблокирован из-за ошибок при вводе пароля. Обратитесь к администратору.";
                    case 28001:
                        return "Пароль устарел";
                    case 12526:
                        return "База данных заблокирована. Вероятно, база находится на техобслуживании. Попробуйте войти позже или обратитесь к администратору базы данных.";
                    case 4068:
                        return "Программа была изменена. Перезапустите программу или попробуйте операцию еще раз.";
                    case 12571:
                        return "Ошибка соединения с базой данных. Вероятнее всего, разрыв связи по сети (ORA-12571: TNS:packet writer failure). Обратитесь к администратору сети или базы данных.";
                    case 3114:
                        return "Оборвано соединение с базой данных (ORA-03114: not connected to ORACLE). Обратитесь к администратору сети или базы данных.";
                    case 3135:
                        return "Оборвано соединение с базой данных (ORA-03135: connection lost contact). Обратитесь к администратору сети или базы данных.";
                    case 12899:
                        return TranslateORA12899(e.Message);
                    case 1401:
                    case 1438:
                        return "Данные не помещаются в базу";
                    default:
                        return oraEx.Message;
                }
            }

            return e.Message;
        }

        public static string TranslateORA12899(string message)
        {
            string result = message;
            string pattern = "(?<msg1>value too large for column\\s*)(?<schema>\"\\w+\")\\.(?<table>\"\\w+\")\\.(?<column>\"\\w+\")\\s*\\(actual:\\s*(?<actual>\\d+), maximum:\\s*(?<maximum>\\d+)\\)(?<reminder>.*)";
            Match match = Regex.Match(message, pattern, RegexOptions.IgnoreCase | RegexOptions.Singleline);
            if (match.Success)
            {
                result = string.Format("значение не помещается в колонку {0}.{1}.{2} (значение {3}, максимум {4}){5} ", match.Groups["schema"], match.Groups["table"], match.Groups["column"], match.Groups["actual"], match.Groups["maximum"], match.Groups["reminder"]);
            }

            return result;
        }

        public static string TranslateOracleRaisedApplicatonException(Exception e)
        {
            string text = null;
            Regex regex = new Regex("(ORA-[0-9]{3,5}:)(.*)");
            MatchCollection matchCollection = regex.Matches(e.Message);
            if (matchCollection.Count > 0)
            {
                for (int i = 0; i < matchCollection.Count; i++)
                {
                    if (Regex.IsMatch(matchCollection[i].Value, "ORA-20[0-9]{3,4}"))
                    {
                        text = matchCollection[i].Groups[2].Value;
                        break;
                    }
                }
            }

            if (string.IsNullOrEmpty(text))
            {
                text = e.Message;
            }

            return text;
        }

        public static string ExtractInfoOra01400(string message)
        {
            string result = string.Empty;
            try
            {
                Match match = Regex.Match(message, "\\([^\\(]*?$");
                if (match != null && match.Groups.Count > 0 && match.Groups[0].Length > 0)
                {
                    result = match.Groups[0].ToString();
                }
            }
            catch
            {
            }

            return result;
        }

        public static string TranslateConnectionErrorMessage(Devart.Data.Oracle.OracleException ex, string dataSource, VOracleConnection connection = null)
        {
            string empty = string.Empty;
            if (connection != null)
            {
            }

            switch (ex.Code)
            {
                case 12154:
                    return $"В файле настроек базы данных TNSNAMES.ORA нет базы с именем {dataSource}\r\n\r\nОбратитесь к администратору.\r\nПроверьте, доступен ли tnsnames.ora, есть ли в нем указанный дескриптор базы, правильно ли он описан.\r\nПроверьте, настроен ли файл SQLNET.ORA. Возможно, в нем не хватает записи \"NAMES.DEFAULT_DOMAIN=WORLD\".\r\n\r\n{ex.Message}";
                case 12520:
                    return string.Format("Обратитесь к администратору базы данных {0}.\r\n\r\nВероятно, в базе задано слишком низкое значение параметра 'processes'. \r\n\r\n Администратору базы данных следует проверить значение параметра, и, возможно, увеличить его. Например: \r\n\r\nalter system set processes=500 scope=spfile;", dataSource, ex.Message);
                case 12638:
                    return $"{ex.Message}\r\n\r\nОбратитесь к администратору.\r\n\r\nНайдите в папке с клиентом oracle файл SQLNET.ORA, попробуйте исправить в нем строчку \r\nSQLNET.AUTHENTICATION_SERVICES= (NTS) \r\n на \r\nSQLNET.AUTHENTICATION_SERVICES= (NONE)";
                case 12170:
                    return string.Format("Не удалось соединиться с базой данных {0}.\r\n\r\nУбедитесь, что файле настроек TNSNAMES.ORA для базы с именем {0} указан правильный адрес и порт.\r\nОбратитесь к администратору.\r\n\r\n{1}", dataSource, ex.Message);
                case 12514:
                    return string.Format("Клиент oracle смог соединиться с сервером, но на этом сервере нет требуемой базы данных.\r\n\r\nПричина: либо в файле TNSNAMES.ORA неправильно указан параметр SID (или SERVICE_NAME) для алиаса \"{0}\", либо в системе установлен еще один клиент oracle, в котором тоже есть алиас \"{0}\", но с неактуальным адресом сервера или SID.\r\n\r\nОбратитесь к администратору.\r\n\r\n{1}", dataSource, ex.Message);
                case 12526:
                    return "База данных заблокирована. Вероятно, она находится на техобслуживании. Попробуйте войти позже или обратитесь к администратору базы данных.";
                default:
                    if (ex.ErrorCode == -2147467259 && ex.Message == "Server did not respond within the specified timeout interval")
                    {
                        return string.Format("Не удалось соединиться с базой данных {0}.\r\n\r\nОбратитесь к администратору.\r\n\r\nУбедитесь, что файле настроек TNSNAMES.ORA для базы с именем {0} указан правильный адрес и порт.\r\nУбедитесь, что сервер Oracle доступен с клиентской машины по порту, указанному в TNSNAMES.ORA.\r\nУбедитесь, что в файле настроек SQLNET.ORA указано \"NAMES.DEFAULT_DOMAIN= WORLD\".\r\n\r\nТекст ошибки:\r\n{1}", dataSource, ex.Message);
                    }

                    return TranslateOracleException(ex) + ((ex.InnerException != null && ex.InnerException.Message != null) ? ("\r\n" + ex.InnerException.Message + ex.InnerException.StackTrace) : "");
            }
        }

        public static bool GetSessionInfo(out decimal? sid, out decimal? serial, out decimal? spid, VOracleConnection connection)
        {
            bool result = false;
            sid = null;
            serial = null;
            spid = null;
            string commandText = "SELECT s.sid, s.SERIAL#, p.SPID\r\n                        FROM v$session s   \r\n                            INNER JOIN v$process p ON p.addr = s.paddr\r\n                        WHERE \r\n                        s.AUDSID = Sys_Context('USERENV', 'SESSIONID') ";
            OracleCommand oracleCommand = new VOracleCommand(commandText, connection);
            try
            {
                DevUtilsProvider.Instance.AnalyzeExecSql(commandText);
                using (OracleDataReader oracleDataReader = oracleCommand.ExecuteReader())
                {
                    if (((DbDataReader)(object)oracleDataReader).Read())
                    {
                        if (((DbDataReader)(object)oracleDataReader)["SID"] == DBNull.Value)
                        {
                            sid = null;
                        }
                        else
                        {
                            sid = Convert.ToDecimal(((DbDataReader)(object)oracleDataReader)["SID"]);
                        }

                        if (((DbDataReader)(object)oracleDataReader)["SERIAL#"] == DBNull.Value)
                        {
                            serial = null;
                        }
                        else
                        {
                            serial = Convert.ToDecimal(((DbDataReader)(object)oracleDataReader)["SERIAL#"]);
                        }

                        if (((DbDataReader)(object)oracleDataReader)["SPID"] == DBNull.Value)
                        {
                            spid = null;
                        }
                        else
                        {
                            spid = Convert.ToDecimal(((DbDataReader)(object)oracleDataReader)["SPID"]);
                        }

                        result = true;
                    }
                }
            }
            finally
            {
                ((IDisposable)oracleCommand)?.Dispose();
            }

            return result;
        }

        public static bool TraceSession(bool startTrace, decimal? sid, decimal? serial, VOracleConnection connection)
        {
            bool result = false;
            if (sid.HasValue && serial.HasValue)
            {
                string text = ((!startTrace) ? $"BEGIN SYS.DBMS_MONITOR.session_trace_disable(session_id=>{sid}, serial_num=>{serial} ); END;" : $"BEGIN SYS.DBMS_MONITOR.session_trace_enable(session_id=>{sid}, serial_num=>{serial}, waits=>TRUE, binds=>TRUE); END;");
                try
                {
                    SqlExecute(text, connection);
                    result = true;
                }
                catch (OracleSqlException ex)
                {
                    if ((ex.InnerException as OracleException).Code == 30)
                    {
                        throw new OracleSqlException(string.Format("Такой сессии не существует: SID={0}, SERIAL#={1}" + Environment.NewLine + ex.Message, sid, serial), ex, text);
                    }

                    if ((ex.InnerException as OracleException).Code == 6550)
                    {
                        text = "ALTER SESSION SET sql_trace = " + (startTrace ? "true" : "false");
                        try
                        {
                            SqlExecute(text, connection);
                            result = true;
                        }
                        catch (Devart.Data.Oracle.OracleException innerException)
                        {
                            throw new OracleSqlException((Devart.Data.Oracle.OracleException)innerException, text);
                        }
                    }
                }
            }

            return result;
        }

        public static bool ChangePassword(string newPassword, VOracleConnection connection)
        {
            bool result = false;
            if (newPassword != null && newPassword.Length > 0)
            {
                try
                {
                    string password = connection.Password;
                    SqlExecutePLSQL("ALTER USER " + connection.UserId.ToUpper() + " IDENTIFIED BY \"" + newPassword + "\" REPLACE \"" + password + "\"", connection);
                    connection.Close();
                    connection.Password = newPassword;
                    connection.ConnectionString = connection.ConnectionString.Replace($"password={password};", $"password={newPassword};");
                    connection.Open();
                    result = true;
                }
                catch (OracleException ex)
                {
                    throw new OracleSqlException("Ошибка при попытке сменить пароль: " + ex.Message, ex);
                }
            }

            return result;
        }

        public static bool ChangePassword(string user, string newPassword, VOracleConnection connection)
        {
            bool result = false;
            if (user != null && user.Length > 0 && newPassword != null && newPassword.Length > 0)
            {
                try
                {
                    if (user.ToUpper() == connection.UserId.ToUpper())
                    {
                        result = ChangePassword(newPassword, connection);
                    }
                    else
                    {
                        newPassword = setPasswordCharCase(newPassword);
                        string commandText = "ALTER USER " + user + " IDENTIFIED BY \"" + newPassword + "\"";
                        OracleCommand oracleCommand = new VOracleCommand(commandText, connection);
                        DevUtilsProvider.Instance.AnalyzeExecSql(commandText);
                        ((DbCommand)(object)oracleCommand).ExecuteNonQuery();
                        result = true;
                        ((Component)(object)oracleCommand).Dispose();
                    }
                }
                catch (OracleException ex)
                {
                    throw new OracleSqlException("Ошибка при попытке сменить пароль: " + ex.Message, ex);
                }
            }

            return result;
        }

        public static bool IsValidPassword(string password, out string err)
        {
            if (isPasswordEmpty(password))
            {
                err = "Пароль не может быть пустым";
                return false;
            }

            if (isPasswordTooShort(password, 8))
            {
                err = $"Пароль не может быть меньше {8} символов";
                return false;
            }

            if (isPasswordConsistOfSameChars(password))
            {
                err = "Пароль не может состоять из одинаковых символов";
                return false;
            }

            if (isPasswordSequence(password))
            {
                err = "Пароль не может состоять из простой последовательности символов (например, abcdefgh)";
                return false;
            }

            if (isPasswordCyrillicChars(password))
            {
                err = "Пароль не должен содержать русских символов";
                return false;
            }

            if (isPasswordWellKnown(password))
            {
                err = "Этот пароль слишком распространенный";
                return false;
            }

            err = null;
            return true;
        }

        private static string setPasswordCharCase(string password)
        {
            char[] array = password.ToLower().ToCharArray();
            int num = 0;
            for (int i = 0; i < array.Length; i++)
            {
                if (Regex.IsMatch(array[i].ToString(), "[a-zA-Z]"))
                {
                    num++;
                }

                if (num == 1)
                {
                    array[i] = char.ToUpper(array[i]);
                }
            }

            return new string(array);
        }

        private static bool isPasswordWellKnown(string password)
        {
            return Regex.IsMatch(password, "qwertyui(o(p(\\[(\\])?)?)?)?", RegexOptions.IgnoreCase) || Regex.IsMatch(password, "asdfghjk(l(;(\\'(\\\\)?)?)?)?", RegexOptions.IgnoreCase) || Regex.IsMatch(password, "zxcvbnm\\,(\\.(\\/)?)?", RegexOptions.IgnoreCase) || Regex.IsMatch(password, "qwerty12(3(4(5(6)?)?)?)?", RegexOptions.IgnoreCase) || password == "1234567890";
        }

        private static bool isPasswordCyrillicChars(string password)
        {
            return Regex.IsMatch(password, "[а-яА-Я]");
        }

        private static bool isPasswordConsistOfSameChars(string password)
        {
            char[] array = password.ToCharArray();
            int num = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] == array[i - 1])
                {
                    num++;
                }
            }

            if (num + 1 >= array.Length)
            {
                return true;
            }

            return false;
        }

        private static bool isPasswordBeginsWithDigit(string password)
        {
            return Regex.IsMatch(password, "^[^a-zA-Z]+[^0-9]");
        }

        private static bool isPasswordTooShort(string password, int minLength)
        {
            return password.Length < minLength;
        }

        private static bool isPasswordEmpty(string password)
        {
            return password == null || password.Length <= 0;
        }

        private static bool isPasswordSequence(string password)
        {
            char[] array = password.ToCharArray();
            int num = 0;
            int num2 = 0;
            for (int i = 1; i < array.Length; i++)
            {
                if (array[i] - array[i - 1] == 1)
                {
                    num++;
                }
                else if (array[i] - array[i - 1] == -1)
                {
                    num2++;
                }
            }

            if (num + 1 >= array.Length || num2 + 1 >= array.Length)
            {
                return true;
            }

            return false;
        }

        public static bool IsAccountLocked(string user, VOracleConnection connection)
        {
            bool result = false;
            if (user != null && user.Length > 0)
            {
                try
                {
                    decimal? num = SqlGetDecimal($"SELECT CASE WHEN account_status LIKE '%LOCK%' THEN 1 ELSE 0 END FROM dba_users WHERE UPPER(username) = UPPER('{user}')", connection);
                    if (num.HasValue)
                    {
                        decimal? num2 = num;
                        decimal num3 = 1;
                        result = (((num2.GetValueOrDefault() == num3) & num2.HasValue) ? true : false);
                    }
                    else
                    {
                        result = false;
                    }
                }
                catch (Devart.Data.Oracle.OracleException innerException)
                {
                    throw new OracleSqlException($"Не удалось определить, заблокирован пользователь {user} или нет", innerException);
                }
            }

            return result;
        }

        public static DateTime? GetAccountLockDate(string user, VOracleConnection connection)
        {
            DateTime? result = null;
            try
            {
                DateTime? dateTime = SqlGetDate($"SELECT LOCK_DATE FROM user_users WHERE UPPER(username) = UPPER('{user}')", connection);
                if (dateTime.HasValue)
                {
                    return dateTime.Value;
                }
            }
            catch (Devart.Data.Oracle.OracleException innerException)
            {
                throw new OracleSqlException("Не удалось определить дату блокирования пользователя " + user, innerException);
            }

            return result;
        }

        public static bool LockAccount(string user, bool lockAccount, VOracleConnection connection)
        {
            bool flag = false;
            try
            {
                SqlExecute(string.Format("ALTER USER {0} ACCOUNT {1}", user, lockAccount ? "LOCK" : "UNLOCK"), connection);
                return true;
            }
            catch (Devart.Data.Oracle.OracleException innerException)
            {
                throw new OracleSqlException(string.Format("Не удалось {0} пользователя {1}", lockAccount ? "заблокировать" : "разблокировать", user), innerException);
            }
        }

        public static bool HasAlterUserPrivilege(VOracleConnection connection)
        {
            bool flag = false;
            try
            {
                decimal? num = SqlGetDecimal("SELECT COUNT(*) FROM user_sys_privs WHERE privilege = 'ALTER USER'", connection);
                if (num.HasValue)
                {
                    decimal? num2 = num;
                    decimal num3 = 1;
                    return ((num2.GetValueOrDefault() >= num3) & num2.HasValue) ? true : false;
                }

                return false;
            }
            catch (Devart.Data.Oracle.OracleException innerException)
            {
                throw new OracleSqlException($"Не удалось определить, может ли пользователь {connection.UserId} менять параметры других пользователей.", innerException);
            }
        }

        public static int CopyRow(DataRow row)
        {
            int num = -1;
            if (row == null)
            {
                return -1;
            }

            if (row.Table == null)
            {
                return -1;
            }

            try
            {
                DataRow dataRow = row.Table.NewRow();
                num = row.Table.Rows.IndexOf(row);
                dataRow.ItemArray = row.ItemArray;
                row.Table.Rows.InsertAt(dataRow, num + 1);
            }
            catch
            {
                return -1;
            }

            return num + 1;
        }

        public static string GetConnectionString(VOracleConnection connection)
        {
            if (connection == null)
            {
                return null;
            }

            OracleConnectionStringBuilder oracleConnectionStringBuilder = new OracleConnectionStringBuilder();
            if (connection.Direct)
            {
                oracleConnectionStringBuilder.Direct = true;
                oracleConnectionStringBuilder.Server = connection.Server;
            }
            else
            {
                oracleConnectionStringBuilder.Server = connection.DataSource;
            }

            if (!string.IsNullOrEmpty(connection.ServiceName))
            {
                oracleConnectionStringBuilder.ServiceName = connection.ServiceName;
            }

            oracleConnectionStringBuilder.Port = connection.Port;
            if (!string.IsNullOrEmpty(connection.Sid))
            {
                oracleConnectionStringBuilder.Sid = connection.Sid;
            }

            if (!string.IsNullOrEmpty(connection.UserId))
            {
                oracleConnectionStringBuilder.UserId = connection.UserId;
            }

            if (!string.IsNullOrEmpty(connection.Password))
            {
                oracleConnectionStringBuilder.Password = connection.Password;
            }

            oracleConnectionStringBuilder.Pooling = false;
            return ((DbConnectionStringBuilder)(object)oracleConnectionStringBuilder).ConnectionString;
        }

        public static void SetSessionModuleAndAction(VOracleConnection connection, string module, string action)
        {
            VOracleParameter[] parameters = new VOracleParameter[2]
            {
            new VOracleParameter("module", module),
            new VOracleParameter("action", action)
            };
            try
            {
                SqlExecute("BEGIN DBMS_APPLICATION_INFO.SET_MODULE(:module, :action); END;", parameters, connection);
            }
            catch (Devart.Data.Oracle.OracleException innerException)
            {
                throw new OracleSqlException("Не удалось установить параметры module и action в сессии oracle", innerException);
            }
        }

        public static void GetSessionModuleAndAction(VOracleConnection connection, out string module, out string action)
        {
            module = null;
            action = null;
            VOracleParameter oracleParameter = new VOracleParameter("module", VOracleDbType.VarChar, ParameterDirection.Output);
            VOracleParameter oracleParameter2 = new VOracleParameter("action", VOracleDbType.VarChar, ParameterDirection.Output);
            VOracleParameter[] parameters = new VOracleParameter[2] { oracleParameter, oracleParameter2 };
            try
            {
                if (SqlExecute("BEGIN DBMS_APPLICATION_INFO.READ_MODULE(:module, :action); END;", parameters, connection))
                {
                    if (((DbParameter)(object)oracleParameter).Value != DBNull.Value)
                    {
                        module = Convert.ToString(((DbParameter)(object)oracleParameter).Value);
                    }

                    if (((DbParameter)(object)oracleParameter2).Value != DBNull.Value)
                    {
                        action = Convert.ToString(((DbParameter)(object)oracleParameter2).Value);
                    }
                }
            }
            catch (Devart.Data.Oracle.OracleException innerException)
            {
                throw new OracleSqlException("Не удалось прочитать параметры module и action в сессии oracle", innerException);
            }
        }
    }


    public class OracleException : DbException, ISerializable
    {
        private int m_a;

        private int b;

        private OracleErrorCollection c;

        private bool d;

        //
        // Summary:
        //     Gets a code that identifies the type of error.
        //
        // Value:
        //     A code that identifies the type of error. For example, for ORA-12345 error it
        //     returns 12345.
        public int Code => this.m_a;

        //
        // Summary:
        //     Gets a position of the incorrect symbol at the SQL statement.
        //
        // Value:
        //     A position of the incorrect symbol at the SQL statement.
        public int Offset => b;

        //
        // Summary:
        //     This property specifies a collection of one or more Devart.Data.Oracle.OracleError
        //     objects that contain information about exceptions generated by the Oracle database.
        //
        //
        // Value:
        //     An Devart.Data.Oracle.OracleErrorCollection.
        public OracleErrorCollection Errors
        {
            get
            {
                if (c == null)
                {
                    OracleError oracleError = new OracleError(-1, Code, Message, sql.builder.Clean.OracleObjectType.Unknown, null, null);
                    oracleError.IsRecoverable = d;
                    c = new OracleErrorCollection(new OracleError[1] { oracleError });
                }

                return c;
            }
        }

        //
        // Summary:
        //     Determines whether an application can chose to re-submit the existing transaction
        //     based on the current transaction status or it must roll-back, re-execute, and
        //     re-submit the current transaction.
        //
        // Value:
        //     true if application can chose to re-submit the existing transaction based on
        //     the current transaction status; otherwise, false.
        public bool IsRecoverable
        {
            get
            {
                if (c != null && c.Count > 0)
                {
                    return c[0].IsRecoverable;
                }

                return d;
            }
            set
            {
                d = value;
            }
        }

        public OracleException(int A_0, string A_1)
            : this(A_0, A_1, A_2: false)
        {
        }

        public OracleException(int A_0, string A_1, bool A_2)
            : base(A_1)
        {
            this.m_a = A_0;
            d = A_2;
        }

        public OracleException(int A_0, string A_1, Exception A_2)
            : base(A_1, A_2)
        {
            this.m_a = A_0;
        }

        protected OracleException(SerializationInfo A_0, StreamingContext A_1)
            : base(A_0, A_1)
        {
            this.m_a = A_0.GetInt32("code");
        }

        public void a(int A_0)
        {
            b = A_0;
        }

        public void a(OracleErrorCollection A_0)
        {
            c = A_0;
        }
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("code", this.m_a);
        }
    }

    public class OracleError
    {
        private int m_a;

        private int b;

        private string c;

        private int d;

        private int e;

        private string f;

        private string g;

        private sql.builder.Clean.OracleObjectType h;

        private bool i;

        //
        // Summary:
        //     Represents row offset within DML array binding operation that generates the error.
        //
        //
        // Value:
        //     Returns one-based row index that generates the error.
        public int ArrayBindIndex
        {
            get
            {
                return this.m_a;
            }
        }

        //
        // Summary:
        //     Gets a code that identifies the type of the error.
        //
        // Value:
        //     A code that identifies the type of the error.
        public int Code
        {
            get
            {
                return b;
            }
        }

        //
        // Summary:
        //     Gets a message that describes the current exception.
        //
        // Value:
        //     The error message that explains the reason for the exception, or an empty string
        //     ("").
        public string Message
        {
            get
            {
                return c;
            }
        }

        //
        // Summary:
        //     Gets number of line where source of the error is located.
        //
        // Value:
        //     Number of line with error.
        public int LineNumber
        {
            get
            {
                return d;
            }
        }

        //
        // Summary:
        //     Gets number of column in a line where source of the error is located.
        //
        // Value:
        //     Column number.
        public int LinePosition
        {
            get
            {
                return e;
            }
        }

        //
        // Summary:
        //     Gets the type of the object.
        //
        // Value:
        //     One of the Devart.Data.Oracle.OracleObjectType values.
        public sql.builder.Clean.OracleObjectType ObjectType
        {
            get
            {
                return h;
            }
        }

        //
        // Summary:
        //     Gets the name of the object.
        //
        // Value:
        //     The name of the object.
        public string ObjectName
        {
            get
            {
                return f;
            }
        }

        //
        // Summary:
        //     Gets the owner of the object.
        //
        // Value:
        //     The owner of the object.
        public string ObjectOwner
        {
            get
            {
                return g;
            }
        }

        //
        // Summary:
        //     Indicates whether the error is recoverable, i. e. the application can choose
        //     to re-submit the existing transaction based on the current transaction status.
        //
        //
        // Value:
        //     true if the error is recoverable; otherwise, false.
        public bool IsRecoverable
        {
            get
            {
                return i;
            }
             set
            {
                i = value;
            }
        }

        public OracleError(int A_0, int A_1, string A_2, sql.builder.Clean.OracleObjectType A_3, string A_4, string A_5)
        {
            h = A_3;
            f = A_4;
            this.m_a = A_0;
            b = A_1;
            c = A_2;
            g = A_5;
        }

        public override string ToString()
        {
            return c;
        }

        public void a(int A_0, int A_1)
        {
            d = A_0;
            e = A_1;
        }
    }

    public class OracleErrorCollection : ICollection
    {
        private OracleError[] a;

        //
        // Summary:
        //     Gets the number of errors in the collection.
        //
        // Value:
        //     The total number of errors in the collection.
        public int Count
        {
            get
            {
                return a.Length;
            }
        }
        public OracleError this[int index]
        {
            get
            {
                return a[index];
            }
        }

        bool ICollection.IsSynchronized
        {
            get
            {
                return ((ICollection)a).IsSynchronized;
            }
        }

        object ICollection.SyncRoot
        {
            get
            {
                return ((ICollection)a).SyncRoot;
            }
        }

        public OracleErrorCollection()
            : this(null)
        {
        }

        public OracleErrorCollection(OracleError[] A_0)
        {
            a = A_0;
        }

        public IEnumerator GetEnumerator()
        {
            return a.GetEnumerator();
        }
        public void CopyTo(Array array, int index)
        {
            a.CopyTo(array, index);
        }
    }


    public class OracleSqlException : Exception, ISerializable
    {
        private string sqlText = string.Empty;

        public string SqlText => sqlText;

        private string getTranslatedOrOriginalMessage()
        {
            int num = 0;
            string empty = string.Empty;
            if (base.InnerException is OracleException)
            {
                num = (base.InnerException as OracleException).Code;
                if (num >= 20000 && num <= 20999)
                {
                    return DataHelper.TranslateOracleRaisedApplicatonException(base.InnerException).Replace("\n", "\r\n");
                }

                return Message;
            }

            return Message;
        }

        public override string ToString()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine("OracleSqlException:");
            stringBuilder.AppendLine(getTranslatedOrOriginalMessage());
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("SQL-запрос:");
            stringBuilder.AppendLine(SqlText);
            stringBuilder.AppendLine("");
            stringBuilder.AppendLine("Время: " + DateTime.Today.ToShortDateString() + " " + DateTime.Now.ToLongTimeString());
            stringBuilder.AppendLine("Пользователь OS: " + Environment.UserName);
            stringBuilder.AppendLine("Командная строка: " + Environment.CommandLine);
            if (base.InnerException != null && base.InnerException is OracleException && (base.InnerException as OracleException).Errors != null)
            {
                stringBuilder.AppendLine("Стек вызовов Oracle:");
                stringBuilder.AppendLine((base.InnerException as OracleException).Errors.ToString());
            }

            stringBuilder.AppendLine("Стек:");
            stringBuilder.AppendLine(StackTrace);
            if (base.InnerException != null && base.InnerException is OracleException)
            {
                stringBuilder.AppendLine("=== Стек внутреннего исключения (Devart.OracleException): ===");
                stringBuilder.AppendLine(base.InnerException.StackTrace);
                stringBuilder.AppendLine("=== Конец стека внутреннего исключения (Devart.OracleException) ===");
            }

            return stringBuilder.ToString();
        }

        public OracleSqlException()
        {
        }

        public OracleSqlException(string message)
            : base(message)
        {
        }

        public OracleSqlException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        public OracleSqlException(string message, Exception innerException, string sql)
            : this(message, innerException)
        {
            sqlText = sql;
        }

        public OracleSqlException(OracleException innerException, string sql)
            : this(innerException.Message, innerException, sql)
        {
        }

        public OracleSqlException(OracleException innerException, string sql, OracleParameterCollection parameters)
            : this(innerException.Message + Environment.NewLine + Environment.NewLine + formatParameters(parameters), innerException, sql)
        {
        }

        public OracleSqlException(Devart.Data.Oracle.OracleException innerException, string sql, OracleParameterCollection parameters)
            : this(innerException.Message + Environment.NewLine + Environment.NewLine + formatParameters(parameters), innerException, sql)
        {
        }

        public OracleSqlException(Devart.Data.Oracle.OracleException innerException, string sql)
            : this(innerException.Message, innerException, sql)
        {
        }

        public OracleSqlException(string message, string sql, OracleParameterCollection parameters)
            : this(message + Environment.NewLine + Environment.NewLine + formatParameters(parameters), null, sql)
        {
        }

        protected OracleSqlException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            sqlText = info.GetString("SqlText");
        }

        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("SqlText", sqlText);
        }

        public static string formatParameters(OracleParameterCollection parameters)
        {
            StringBuilder stringBuilder = new StringBuilder(string.Empty);
            if (((DbParameterCollection)(object)parameters).Count > 0)
            {
                stringBuilder.AppendLine("Список параметров:");
            }

            if (parameters != null)
            {
                foreach (DbParameter item in (DbParameterCollection)(object)parameters)
                {
                    if (item != null)
                    {
                        var dbType = item.GetOracleDbType();
                        stringBuilder.AppendLine(string.Concat(str2: (((DbParameter)(object)item).Value == null) ? "" : ((((DbParameter)(object)item).Value == DBNull.Value) ? ((DbParameter)(object)item).Value.ToString() : ((dbType == VOracleDbType.Blob) ? ("[BLOB length=" + ((OracleBinary)((DbParameter)(object)item).Value).Length + "]") : ((dbType != VOracleDbType.Clob) ? ((DbParameter)(object)item).Value.ToString() : "[CLOB]"))), str0: ((DbParameter)(object)item).ParameterName, str1: "="));
                    }
                }
            }

            return stringBuilder.ToString();
        }
    }


    public class OracleConvention
    {
        private static string GetNetTypeName(string DataBaseType)
        {
            string result = "object";
            switch (DataBaseType.ToUpper())
            {
                case "INT":
                case "NUMBER":
                    result = "decimal?";
                    break;
                case "LONG":
                    result = "long?";
                    break;
                case "DOUBLE":
                case "DECIMAL":
                case "FLOAT":
                    result = "decimal?";
                    break;
                case "VARCHAR2":
                case "CHAR":
                    result = "string";
                    break;
                case "DATE":
                case "DATETIME":
                case "TIMESTAMP":
                    result = "DateTime?";
                    break;
                case "BOOLEAN":
                    result = "bool?";
                    break;
                case "BLOB":
                    result = "Byte[]";
                    break;
                case "CLOB":
                    result = "string";
                    break;
            }

            return result;
        }

        public static string GetNetTypeName(string DataBaseType, int? DATA_PRECISION, int? DATA_SCALE)
        {
            if (!DATA_PRECISION.HasValue)
            {
                DATA_PRECISION = 0;
            }

            if (!DATA_SCALE.HasValue)
            {
                DATA_SCALE = 0;
            }

            if (DATA_PRECISION == 0 && DATA_SCALE == 0)
            {
                return GetNetTypeName(DataBaseType);
            }

            string result = "object";
            switch (DataBaseType.ToUpper())
            {
                case "INT":
                    result = "decimal?";
                    break;
                case "NUMBER":
                    result = ((DATA_PRECISION == 0 && DATA_SCALE == 0) ? "int?" : ((DATA_SCALE == 0 && DATA_PRECISION < 10) ? "int?" : ((DATA_SCALE != 0 || !(DATA_PRECISION >= 10) || DATA_PRECISION == 38) ? "decimal?" : "long?")));
                    break;
                case "LONG":
                    result = "string";
                    break;
                case "DECIMAL":
                    result = "decimal?";
                    break;
                case "FLOAT":
                    result = "decimal?";
                    break;
                case "VARCHAR2":
                case "CHAR":
                    result = "string";
                    break;
                case "DATE":
                case "DATETIME":
                case "TIMESTAMP":
                case "INTERVAL DAY TO SECOND":
                case "INTERVAL YEAR TO MONTH":
                    result = "DateTime?";
                    break;
                case "BOOLEAN":
                    result = "bool?";
                    break;
                case "BLOB":
                    result = "Byte[]";
                    break;
                case "CLOB":
                    result = "string";
                    break;
            }

            return result;
        }

        public static Type ConvertDataType(Type s_type)
        {
            Type typeFromHandle = typeof(decimal);
            if (s_type == typeof(short))
            {
                return typeFromHandle;
            }

            if (s_type == typeof(int))
            {
                return typeFromHandle;
            }

            if (s_type == typeof(long))
            {
                return typeFromHandle;
            }

            if (s_type == typeof(double))
            {
                return typeFromHandle;
            }

            if (s_type == typeof(byte))
            {
                return typeFromHandle;
            }

            if (s_type == typeof(float))
            {
                return typeFromHandle;
            }

            if (s_type == typeof(long))
            {
                return typeFromHandle;
            }

            return s_type;
        }

        public static bool IsNumber(Type type)
        {
            Type typeFromHandle = typeof(decimal);
            if (type == typeof(short))
            {
                return true;
            }

            if (type == typeof(int))
            {
                return true;
            }

            if (type == typeof(long))
            {
                return true;
            }

            if (type == typeof(double))
            {
                return true;
            }

            if (type == typeof(byte))
            {
                return true;
            }

            if (type == typeof(float))
            {
                return true;
            }

            if (type == typeof(long))
            {
                return true;
            }

            return false;
        }
    }


}

