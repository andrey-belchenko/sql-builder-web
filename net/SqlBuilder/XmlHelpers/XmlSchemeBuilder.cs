using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Xml.Linq;
using Devart.Data.Oracle;
using infoenergo.core.Data;
//using infoenergo.core.Extensions;
using sql.builder.DataApi;

namespace sql.builder.XmlHelpers
{
    public class XmlSchemeBuilder
    {
        const string alias = "a";

        // Описываются правила на атрибуты, значения которых не должны обновляться в UpdateScheme (напиример для title у колонки)
        // описывается в виде: Родительский узел, Узел, Аттрибут
        // родительский узел может быть null
        private static string[] not_changeble_attributes =
        {
            // title колонки
            "parent_node = select; node = column; attribute = title",
            // join в связях
            "parent_node = from; node = query; attribute = join"
        };

        // Описываются правила на узлы, которые могут добавляться в UpdateScheme (напиример колонки в запросе)
        // описывается в виде: Родительский узел, Узел
        // родительский узел может быть null
        private static string[] allow_add_nodes =
        {
            // колонки запроса
            "parent_node = select; node = column",
            "parent_node = from; node = query"
        };

        public static XElement XmlTableStructure(string table)
        {
            string db_scheme, object_type;
            int pos = table.IndexOf('.');
            if (pos >= 0) {
                db_scheme = table.Substring(0, pos).ToUpper();
                table = table.Substring(pos + 1);
            } else {
                db_scheme = string.Empty;
            }
            OracleParameter[] parameters = new OracleParameter[2] { new OracleParameter("table_name", OracleDbType.VarChar, table, ParameterDirection.Input),
                                                                    new OracleParameter("db_scheme", OracleDbType.VarChar, db_scheme, ParameterDirection.Input) };
            DataTable dt = DataHelper.SqlGetTable("SELECT owner, object_type FROM all_objects WHERE object_name = UPPER(:table_name) AND owner = NVL(:db_scheme, USER) AND object_type IN ('TABLE', 'VIEW')", parameters, db.Connection);
            DataRow row;
            if (dt.Rows.Count > 0) {
                row = dt.Rows[0];
                if (string.IsNullOrEmpty(db_scheme)) {
                    db_scheme = row.Field<string>("owner");
                }
                object_type = row.Field<string>("object_type");
                Cmn.DisposeAndSetNull(ref dt);
            } else {
                Cmn.DisposeAndSetNull(ref dt);
                if (string.IsNullOrEmpty(db_scheme)) {
                    parameters = new OracleParameter[1] { new OracleParameter("table_name", OracleDbType.VarChar, table, ParameterDirection.Input) };
                    dt = DataHelper.SqlGetTable("SELECT owner, object_type FROM all_objects WHERE object_name = UPPER(:table_name) AND object_type IN ('TABLE', 'VIEW')", parameters, db.Connection);
                    if (dt.Rows.Count != 1) {
                        return null;
                    }
                    row = dt.Rows[0];
                    db_scheme = row.Field<string>("owner");
                    object_type = row.Field<string>("object_type");
                    Cmn.DisposeAndSetNull(ref dt);
                } else {
                    return null;
                }
            }
            XElement xquery = LoadQueryStruct(db_scheme, table, object_type);
            XElement xroot = new XElement(EName.root, new XElement(EName.queries, xquery));
            return xroot;
        }
        public static XElement UpdateScheme(XElement old_scheme, XElement new_scheme)
        {
            // чистим namespace чтобы не мешал
            if (old_scheme.Name.NamespaceName != "") {
                old_scheme.Attribute("xmlns").Remove();
                //old_scheme.DescendantsAndSelf().ForEach(el => el.Name = el.Name.LocalName); //sklubowicz: это не компилялось!
                foreach (var el in old_scheme.DescendantsAndSelf())
                {
                    el.Name = el.Name.LocalName;
                }
            }
            XElement query1 = old_scheme.Element(EName.queries).Element(EName.query);
            XElement query2 = new_scheme.Element(EName.queries).Element(EName.query);
            if (query1.Attribute(AName.name).Value != query2.Attribute(AName.name).Value) {
                return null;
            }
            XElement xquery_result = CompareXElements(query1, query2);
            // если был сгенерирован лишний ключ
            IList<XElement> xKeyColumns = xquery_result.Element(EName.select).Elements().Where(c => c.AttrOrDefault(AName.key, false)).ToList();
            if (xKeyColumns.Count > 1) {
                XElement xcol = xKeyColumns.LastOrDefault(c => c.AttrOrDefault(AName.function, null) == TextConst.AVFunction.RowId);
                if (xcol != null) {
                    xcol.Remove();
                }
            }
            return new XElement(EName.root, new XElement(EName.queries, xquery_result));
        }

        #region Закрытые методы
        static XElement CompareXElements(XElement old_element, XElement new_element)
        {
            // !!! Логика: атрибута не было - добавляем, атрибут был - изменяем если 
            // нет запрещающего правила (в переменной not_changeble_attributes)
            // Отсутствующие в новом файле атрибуты не удаляются из старого.

            // будет содержать результат сравнения старого и нового элементы
            var updated_element = new XElement(old_element.Name);

            // получаем имена всех атрибутов старого элемента
            var old_atr_names = old_element.Attributes().Select(atr => atr.Name.LocalName);
            // получаем имена всех атрибутов нового элемента
            var new_atr_names = new_element.Attributes().Select(atr => atr.Name.LocalName);
            // объединяем: набор уникальных имен для обоих элементов
            var atr_names = old_atr_names.Union(new_atr_names);

            // перебираем имена атрибутов
            foreach (var atr_name in atr_names)
            {
                var atr1 = old_element.Attribute(atr_name);
                var atr2 = new_element.Attribute(atr_name);

                // если в новом есть атрибут с таким именем и на этот атрибут нет правила не обновлять его - копируем его
                if (atr2 != null && (atr1 == null || !IsNotChangebleAttribute(atr1)))
                {
                    // сохраняем новое значение
                    updated_element.Add(new XAttribute(atr2.Name, atr2.Value));
                }
                else if (atr1 != null)
                {
                    // сохраняем старое значение
                    updated_element.Add(new XAttribute(atr1.Name, atr1.Value));
                }
            }

            // !!! Логика для узлов. Узел идентифицируется по имени и по нескольким атрибутам (до 2).
            // Ключевые атрибуты для узлов возвращает ф-я GetDefaultKeyAttribute.
            // Рассматриваются только узлы, которые уже существовали в старом файле.
            // Если удалось найти этот узел в новом файле (по имени и ключам), то узлы сравниваются дальше.
            // Узлы не удаляются. Новые узлы добавляются, если на них есть разрешение (в переменной allow_add_node)

            // получаем набор дочерних узлов для старого элемента с ключивыми атрибутами в наборе
            var old_el_names = old_element.Elements().Select(
                el => new Tuple<string, string, string>(el.Name.LocalName,
                    GetElementKeyAttrName(el, 0), GetElementKeyAttrName(el, 1)));

            // получаем набор дочерних узлов для нового элемента с ключивыми атрибутами в наборе
            var new_el_names = new_element.Elements().Select(
                el => new Tuple<string, string, string>(el.Name.LocalName,
                    GetElementKeyAttrName(el, 0), GetElementKeyAttrName(el, 1)));
            // объединяем: набор всех возможных узлов для обоих дочерних элементов
            var el_names = old_el_names.Union(new_el_names);

            // перебираем все дочерние элементы
            foreach (var el_name in el_names)
            {
                // набор ключевых атрибутов для узла
                var keys = GetDefaultKeyAttribute(el_name.Item1, old_element.Name.LocalName);
                var el1 = old_element.Elements(el_name.Item1).FirstOrDefault(
                    el => (keys[0] == "" || el.Attribute(keys[0]).Value == el_name.Item2)
                       && (keys[1] == "" || el.Attribute(keys[1]).Value == el_name.Item3));
                var el2 = new_element.Elements(el_name.Item1).FirstOrDefault(
                    el => (keys[0] == "" || el.Attribute(keys[0]).Value == el_name.Item2)
                       && (keys[1] == "" || el.Attribute(keys[1]).Value == el_name.Item3));

                if (el1 == null)
                {
                    if (AllowAddNode(el2))
                    {
                        updated_element.Add(el2);
                    }
                }
                // !!! элемент call function не обрабатываем (не понятно как его обрабатывать)
                else if (el2 == null || el_name.Item1 == "call")
                {
                    updated_element.Add(el1);
                }
                else updated_element.Add(CompareXElements(el1, el2));
            }
            return updated_element;
        }
        private static DataTable GetConstraintColumns(string constraint_name)
        {
            OracleParameter[] parameters = new OracleParameter[1] { new OracleParameter("constraint_name", OracleDbType.VarChar, constraint_name, ParameterDirection.Input) };
            return DataHelper.SqlGetTable("SELECT table_name, position, column_name FROM all_cons_columns WHERE constraint_name = :constraint_name ORDER BY position ASC", parameters, db.Connection);
        }
        private static XElement LoadQueryStruct(string db_scheme, string table, string object_type)
        {
            XElement xquery, xselect, xfrom;
            Factory.NewSelectFromQuery(out xquery, out xselect, out xfrom);
            xquery.Add(new XAttribute(AName.name, table));
            XElement xtable = new XElement(EName.table);
            xtable.Add(new XAttribute(AName.name, table));
            xtable.Add(new XAttribute(AName.@as, alias));
            xfrom.Add(xtable);
            // получаем наименование и описание колонок таблицы
            DataTable dtCols = db.GetTableStuct(table, db_scheme);
            List<string> pk_cols_names = new List<string>(1);
            int index;
            for (index = 0; index < dtCols.Rows.Count; index++) {
                DataRow row = dtCols.Rows[index];
                string name = row["column_name"].ToString().ToLower();
                string db_type = row["data_type"].ToString();
                string comment = row["comments"].ToString();
                //string data_type = GetType(db_type);
                string data_type;
                switch (db_type) {
                    case "NUMBER":
                    case "FLOAT":
                        data_type = TextConst.AVDataType.Number;
                        break;
                    case "VARCHAR2":
                    case "NVARCHAR2":
                    case "CHAR":
                    case "NCHAR":
                    case "LONG":
                        data_type = TextConst.AVDataType.String;
                        break;
                    case "DATE":
                        data_type = TextConst.AVDataType.Date;
                        break;
                    case "CLOB":
                    case "NCLOB":
                        data_type = TextConst.AVDataType.Clob;
                        break;
                    case "BLOB":
                    case "LONG RAW":
                        data_type = TextConst.AVDataType.Blob;
                        break;
                    default:
                        data_type = db_type;
                        break;
                }
                XElement xcolumn = Factory.NewColumn(alias, name);
                xcolumn.Add(new XAttribute(AName.type, data_type));
                if (data_type == TextConst.AVDataType.String) {
                    string length = row["data_length"].ToString();
                    xcolumn.Add(new XAttribute(AName.data_size, length));
                }
                if (row["nullable"].ToString() == "N") {
                    xcolumn.Add(new XAttribute(AName.column_mandatory, TextConst.AVBool.True));
                }
                if (!name.Contains("kod_")) {
                    xcolumn.Add(new XAttribute(AName.title, GetDefaultTitle(name)));
                }
                if (!string.IsNullOrEmpty(comment)) {
                    xcolumn.Add(new XAttribute(AName.comment, comment));
                }
                xselect.Add(xcolumn);
                if (row["is_pk"].ToString() == "1") {
                    pk_cols_names.Add(name);
                }
            }
            Cmn.DisposeAndSetNull(ref dtCols);
            if (object_type == "TABLE") {
                // генерируем колонку-ключ если в базе его нет и если table не view
                if (pk_cols_names.Count == 0 && object_type == "TABLE") {
                    XElement xcall = Factory.NewCall(TextConst.AVFunction.RowId);
                    xcall.Add(new XAttribute(AName.@as, table + "_id"));
                    xcall.Add(new XAttribute(AName.key, TextConst.AVBool.True));
                    xselect.AddFirst(xcall);
                } else if (pk_cols_names.Count > 1) {
                    // генерируем колонку-ключ если в базе он составной
                    XElement xcall = Factory.NewCall(TextConst.AVFunction.Concat);
                    xcall.Add(new XAttribute(AName.@as, table + "_id"));
                    xcall.Add(new XAttribute(AName.@type, TextConst.AVDataType.String));
                    xcall.Add(new XAttribute(AName.key, TextConst.AVBool.True));
                    index = 0;
                    while (true) {
                        xcall.Add(Factory.NewColumn(alias, pk_cols_names[index]));
                        index++;
                        if (index >= pk_cols_names.Count) {
                            break;
                        }
                        xcall.Add(Factory.NewConst("'-'"));
                    }
                    xselect.AddFirst(xcall);
                }
                //DataTable dtConstraints = db.GetTableConstraints(table, db_scheme);
                //var dtConstraintColumns = db.GetTableConstraintColumns(table, db_scheme);
                // P - primary , R - foreign, C - condition
                //var r_constraints = dtConstraints.AsEnumerable().Where(cns => (string)cns["constraint_type"] == "R").ToArray();
                //var p_constraints = dtConstraints.AsEnumerable().Where(cns => (string)cns["constraint_type"] == "P").ToArray();
                //var c_constraints = dtConstraints.AsEnumerable().Where(cns => (string)cns["constraint_type"] == "C").ToArray();
                //var c_constraints_names = c_constraints.Select(c => (string)c["constraint_name"]).ToArray();
                //var c_constraints_cols = dtConstraintColumns.AsEnumerable().Where(cns => c_constraints_names.Contains((string)cns["constraint_name"])).ToArray();
                OracleParameter[] parameters = new OracleParameter[2] { new OracleParameter("table_name", OracleDbType.VarChar, table, ParameterDirection.Input),
                                                                    new OracleParameter("db_scheme", OracleDbType.VarChar, db_scheme, ParameterDirection.Input) };
                DataTable ref_constraints = DataHelper.SqlGetTable("SELECT constraint_name, r_constraint_name FROM all_constraints WHERE table_name = UPPER(:table_name) AND owner = :db_scheme AND constraint_type = 'R'", parameters, db.Connection);
                foreach (DataRow row in ref_constraints.Rows) {
                    string constraint_name = row["constraint_name"].ToString();
                    DataTable dtColumns = GetConstraintColumns(constraint_name);
                    DataTable dtRelColumns = GetConstraintColumns(row["r_constraint_name"].ToString());
                    if (dtRelColumns.Rows.Count != 0) {
                        string rel_table_name;
                        string rel_column;
                        string column;
                        string join;
                        if (dtColumns.Rows.Count == 1) { // связь через одну колонку
                            rel_table_name = dtRelColumns.Rows[0]["table_name"].ToString().ToLower();
                            rel_column = dtRelColumns.Rows[0]["column_name"].ToString().ToLower();
                            column = dtColumns.Rows[0]["column_name"].ToString().ToLower();
                            //join = IsNotNullColumn(column, c_constraints_cols, c_constraints) ? "left inner" : "left outer";
                            join = TextConst.AVJoin.LeftOuter;
                        } else if (dtColumns.Rows.Count > 1) { // связь через несколько колонок - создаем фиктивную колонку
                            rel_table_name = dtRelColumns.Rows[0]["table_name"].ToString().ToLower();
                            rel_column = rel_table_name + "_id";
                            column = rel_column;
                            join = TextConst.AVJoin.LeftOuter;
                            //
                            XElement xcall = Factory.NewCall(TextConst.AVFunction.Concat);
                            xcall.Add(new XAttribute(AName.@as, rel_column));
                            xcall.Add(new XAttribute(AName.@type, TextConst.AVDataType.String));
                            index = 0;
                            while (true) {
                                xcall.Add(Factory.NewColumn(alias, dtColumns.Rows[index]["column_name"].ToString().ToLower()));
                                index++;
                                if (index >= dtColumns.Rows.Count) {
                                    break;
                                }
                                xcall.Add(Factory.NewConst("'-'"));
                            }
                            xselect.Add(xcall);
                        } else {
                            throw new Exception("Для констрэйнта " + constraint_name + " нет колонок");
                        }
                        XElement xsub_query = new XElement(EName.query);
                        xsub_query.Add(new XAttribute(AName.name, rel_table_name));
                        xsub_query.Add(new XAttribute(AName.@as, column));
                        xsub_query.Add(new XAttribute(AName.join, join));
                        xsub_query.Add(Factory.NewCall(TextConst.AVFunction.Equal,
                                                       Factory.NewColumn(alias, column),
                                                       Factory.NewColumn(column, rel_column)));
                        xfrom.Add(xsub_query);
                    }
                    Cmn.DisposeAndSetNull(ref dtRelColumns);
                    Cmn.DisposeAndSetNull(ref dtColumns);
                }
                Cmn.DisposeAndSetNull(ref ref_constraints);
            }
            return xquery;
        }
        private static bool IsNotNullColumn(string column_name, DataRow[] c_constraint_cols, DataRow[] c_constraints)
        {
            var constraint_col = c_constraint_cols.AsEnumerable().FirstOrDefault(c => c["column_name"].Equals(column_name.ToUpper()));
            if (constraint_col == null) return false;

            var constraint = c_constraints.AsEnumerable().First(c => c["constraint_name"].Equals(constraint_col["constraint_name"]));
            string condition = String.Format(@"""{0}"" IS NOT NULL", column_name.ToUpper());
            return constraint["search_condition"].ToString().Contains(condition);
        }
        /*private static string GetType(string type)
        {
            switch (type) {
                case "NUMBER":
                case "FLOAT":
                    return TextConst.AVDataType.Number;
                case "VARCHAR2":
                case "NVARCHAR2":
                case "CHAR":
                case "NCHAR":
                case "LONG":
                    return TextConst.AVDataType.String;
                case "DATE":
                    return TextConst.AVDataType.Date;
                default:
                    return type;
            }
        }*/
        static string GetDefaultTitle(string param_name)
        {
            switch (param_name.ToLower())
            {
                case "name": return "Наименование";
                case "abbr": return "Аббревиатура";
                default: return String.Empty;
            }
        }
        static string GetElementKeyAttrName(XElement el, int num_attr)
        {
            string parent_name = el.Parent != null ? el.Parent.Name.LocalName : "";
            string key = GetDefaultKeyAttribute(el.Name.LocalName, parent_name)[num_attr];
            return key == "" ? "" : el.Attribute(key).Value;
        }
        static string[] GetDefaultKeyAttribute(string local_name, string parent_name = "")
        {
            string key1 = "";
            string key2 = "";

            switch (local_name)
            {
                case "query":
                case "table":
                    key1 = "name";
                    key2 = "as";
                    break;
                case "call":
                    key1 = "function";
                    break;
                case "column":
                    key1 = "table";
                    if (parent_name != "function")
                    {
                        key2 = "column";
                    }
                    break;
            }

            return new[] { key1, key2 };
        }

        static bool IsNotChangebleAttribute(XAttribute attr)
        {
            var all_rules = not_changeble_attributes.Select(rule =>
            {
                var rules = rule.Split(';');
                return new
                {
                    parent_node = rules[0].Split('=')[1].Trim(),
                    node = rules[1].Split('=')[1].Trim(),
                    attribute = rules[2].Split('=')[1].Trim()
                };
            });

            return all_rules.Any(rule => attr.Parent.Parent != null
                                             ? rule.parent_node == attr.Parent.Parent.Name.LocalName
                                             : rule.parent_node == "null"
                                         && attr.Parent.Name.LocalName == rule.node
                                         && attr.Name.LocalName == rule.attribute);
        }
        static bool AllowAddNode(XElement node)
        {
            var all_rules = allow_add_nodes.Select(rule =>
            {
                var rules = rule.Split(';');
                return new
                {
                    parent_node = rules[0].Split('=')[1].Trim(),
                    node = rules[1].Split('=')[1].Trim()
                };
            });

            return all_rules.Any(rule => node.Parent != null
                                             ? rule.parent_node == node.Parent.Name
                                             : rule.parent_node == "null"
                                         && node.Name.LocalName == rule.node);
        }
        #endregion
    }
}
