using System.Xml.Linq;
using Contract = System.Diagnostics.Contracts.Contract;

namespace sql.builder.DataApi
{
    /// <summary>
    /// Статический класс в котором собраны методы для создания XML
    /// </summary>
    public static class Factory
    {
        #region call
        /// <summary>
        /// Создаёт новый тэг &lt;call&gt; с атрибутом function (<paramref name="function"/>)
        /// </summary>
        /// <param name="function">значение атрибута function, используйте <see cref="TextConst.AVFunction"/></param>
        /// <returns>созданный тег</returns>
        public static XElement NewCall(string function)
        {
            Contract.Assert(!string.IsNullOrEmpty(function));
            XElement call = new XElement(EName.call);
            call.Add(new XAttribute(AName.function, function));
            return call;
        }
        /// <summary>
        /// Создаёт новый тэг &lt;call&gt; с атрибутом function (<paramref name="function"/>) и одним аргументом
        /// </summary>
        /// <param name="function">значение атрибута function, используйте <see cref="TextConst.AVFunction"/></param>
        /// <param name="arg">аргумент</param>
        /// <returns>созданный тег</returns>
        public static XElement NewCall(string function, XElement arg)
        {
            XElement call = NewCall(function);
            call.Add(arg);
            return call;
        }
        /// <summary>
        /// Создаёт новый тэг &lt;call&gt; с атрибутом function (<paramref name="function"/>) и двумя аргументами
        /// </summary>
        /// <param name="function">значение атрибута function, используйте <see cref="TextConst.AVFunction"/></param>
        /// <param name="arg_1">первый аргумент</param>
        /// <param name="arg_2">второй аргумент</param>
        /// <returns>созданный тег</returns>
        public static XElement NewCall(string function, XElement arg_1, XElement arg_2)
        {
            XElement call = NewCall(function);
            call.Add(arg_1);
            call.Add(arg_2);
            return call;
        }
        /// <summary>
        /// Создаёт новый тэг &lt;call&gt; с атрибутом function (<paramref name="function"/>) и тремя аргументами
        /// </summary>
        /// <param name="function">значение атрибута function, используйте <see cref="TextConst.AVFunction"/></param>
        /// <param name="arg_1">первый аргумент</param>
        /// <param name="arg_2">второй аргумент</param>
        /// <param name="arg_3">третий аргумент</param>
        /// <returns>созданный тег</returns>
        public static XElement NewCall(string function, XElement arg_1, XElement arg_2, XElement arg_3)
        {
            XElement call = NewCall(function);
            call.Add(arg_1);
            call.Add(arg_2);
            call.Add(arg_3);
            return call;
        }
        #endregion
        /// <summary>
        /// Добавляет в <paramref name="parent"/> тэг &lt;field&gt;
        /// с атрибутами name (<paramref name="field_name"/>), type (<paramref name="data_type"/>) и
        /// parname (<paramref name="field_name"/>)
        /// </summary>
        /// <param name="parent">родительский тэг</param>
        /// <param name="field_name">значение атрибутов name и parname</param>
        /// <param name="data_type">значение атрибута type, используйте значения из <see cref="TextConst.AVDataType"/></param>
        /// <returns>добавленный тег</returns>
        public static XElement AddNewField(XElement parent, string field_name, string data_type)
        {
            Contract.Assert(parent != null);
            Contract.Assert(!string.IsNullOrEmpty(field_name));
            Contract.Assert(!string.IsNullOrEmpty(data_type));
            XElement xfield = new XElement(EName.field);
            xfield.Add(new XAttribute(AName.type, data_type));
            xfield.Add(new XAttribute(AName.name, field_name));
            xfield.Add(new XAttribute(AName.parname, field_name));
            parent.Add(xfield);
            return xfield;
        }
        #region column
        /// <summary>
        /// Создаёт новый тэг &lt;column&gt; с атрибутами table (<paramref name="table"/>) и column (<paramref name="column"/>)
        /// </summary>
        /// <param name="table">значение атрибута table</param>
        /// <param name="column">значение атрибута column</param>
        /// <returns>созданный тег</returns>
        public static XElement NewColumn(string table, string column)
        {
            Contract.Assert(!string.IsNullOrEmpty(table));
            Contract.Assert(!string.IsNullOrEmpty(column));
            XElement col = new XElement(EName.column);
            col.Add(new XAttribute(AName.table, table));
            col.Add(new XAttribute(AName.column, column));
            return col;
        }
        /// <summary>
        /// Создаёт новый тэг &lt;column&gt; с атрибутами name (<paramref name="name"/>), type (<paramref name="data_type"/>)
        /// и, опционально, title (<paramref name="title"/>)
        /// </summary>
        /// <param name="name">значение атрибута name</param>
        /// <param name="data_type">значение атрибута type, используйте значения из <see cref="TextConst.AVDataType"/></param>
        /// <param name="title">значение атрибута title или null</param>
        /// <returns>созданный тег</returns>
        public static XElement NewColumn(string name, string data_type, string title = null)
        {
            Contract.Assert(!string.IsNullOrEmpty(name));
            Contract.Assert(!string.IsNullOrEmpty(data_type));
            XElement col = new XElement(EName.column);
            col.Add(new XAttribute(AName.name, name));
            col.Add(new XAttribute(AName.type, data_type));
            if (!string.IsNullOrEmpty(title))
            {
                col.Add(new XAttribute(AName.title, title));
            }
            return col;
        }
        #endregion
        /// <summary>
        /// Создаёт новый тэг &lt;useparam&gt; с атрибутом name (<paramref name="name"/>)
        /// </summary>
        /// <param name="name">значение атрибута name</param>
        /// <returns>созданный тег</returns>
        public static XElement NewUseParam(string name)
        {
            Contract.Assert(!string.IsNullOrEmpty(name));
            XElement up = new XElement(EName.useparam);
            up.Add(new XAttribute(AName.name, name));
            return up;
        }
        /// <summary>
        /// Создаёт новый тэг &lt;param&gt; с атрибутами name (<paramref name="param_name"/>) и type (<paramref name="data_type"/>)
        /// </summary>
        /// <param name="param_name">значение атрибута name</param>
        /// <param name="data_type">значение атрибута type, используйте значения из <see cref="TextConst.AVDataType"/></param>
        /// <returns>созданный тег</returns>
        public static XElement NewParam(string param_name, string data_type)
        {
            Contract.Assert(!string.IsNullOrEmpty(param_name));
            Contract.Assert(!string.IsNullOrEmpty(data_type));
            XElement xpar = new XElement(EName.param);
            xpar.Add(new XAttribute(AName.name, param_name));
            xpar.Add(new XAttribute(AName.type, data_type));
            return xpar;
        }
        public static XElement NewConst(string value)
        {
            Contract.Assert(value != null);
            return new XElement(EName.@const, new XText(value));
        }
        public static void NewSelectFromQuery(out XElement query, out XElement select, out XElement from)
        {
            query = new XElement(EName.query);
            select = new XElement(EName.select);
            query.Add(select);
            from = new XElement(EName.from);
            query.Add(from);
        }
        public static void NewSelectFromDualQuery(out XElement query, out XElement select, out XElement from, out XElement dual)
        {
            NewSelectFromQuery(out query, out select, out from);
            dual = new XElement(EName.table, new XAttribute(AName.name, TextConst.AVTable.Dual));
            from.Add(dual);
        }
        public static void NewUnionQuery(out XElement query, out XElement union)
        {
            query = new XElement(EName.query);
            union = new XElement(EName.union);
            query.Add(union);
        }
    }
}
