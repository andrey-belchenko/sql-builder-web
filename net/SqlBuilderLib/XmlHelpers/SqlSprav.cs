using System.Text;

namespace sql.builder.XmlHelpers
{
    internal static class SqlSprav
    {
        const int _null_replacement = 0;

        public static string GetPackageSql(decimal kod_struct)
        {
            StructInfo si = GetStructInfo(kod_struct);
            if (si == null) return null;

            StringBuilder sql = new StringBuilder();

            sql.AppendLine(string.Format("CREATE OR REPLACE PACKAGE VG_SPRAV{0}", kod_struct));
            sql.AppendLine("IS");
            sql.AppendLine("\tPROCEDURE UPDATE_RESULT_TABLE;");
            sql.AppendLine("END;");
            sql.AppendLine("/");
            sql.AppendLine(string.Format("CREATE OR REPLACE PACKAGE BODY VG_SPRAV{0}", kod_struct));
            sql.AppendLine("IS");
            sql.AppendLine("\tPROCEDURE UPDATE_RESULT_TABLE");
            sql.AppendLine("\tIS");
            sql.AppendLine("\tBEGIN");
            sql.AppendLine();
            sql.AppendLine(string.Format("\t\tdelete from {0} where kod_tree_struct = {1};", si.ResultTableName, kod_struct));
            sql.AppendLine();
            sql.AppendLine(string.Format("\t\tinsert into {0}(kod_tree_struct,{2},{1})", si.ResultTableName, si.DataKeyName, si.TreeKeyName));
            sql.AppendLine("\t\t(");
            sql.AppendLine("\t\t\t-- отсеиваем только листья (самые нижние узлы) дерева и по условиям на справочники объединяем с данными");
            sql.AppendLine(string.Format("\t\t\tselect distinct c.kod_tree_struct, c.{1}, z.{0}", si.DataKeyName, si.TreeKeyName));
            sql.AppendLine("\t\t\tfrom");
            sql.AppendLine("\t\t\t(");
            sql.AppendLine("\t\t\t\t-- для каждого узла определяем ближайший родительский узел с условиями по конкретному справочнику");
            sql.AppendLine("\t\t\t\t-- если условий по справочнику нет ни в одном родительском узле - вернется null - значит по этому справочнику не фильтруем");
            sql.AppendLine("\t\t\t\tselect b.*,");

            for (int i = 0; i < si.Spravs.Length; i++)
            {
                sql.AppendLine(string.Format("\t\t\t\t\tlast_value(case when sprav{0}_exists = 1 then {1} end) over(partition by kod_root order by sprav{0}_exists, lvl rows between unbounded preceding and unbounded following) as {1}{0},", i + 1, si.TreeKeyName));
            }
            // удаляем последнюю запятую
            sql.Remove(sql.Length - 3, 1);

            sql.AppendLine("\t\t\t\tfrom");
            sql.AppendLine("\t\t\t\t(");
            sql.AppendLine("\t\t\t\t\t-- обходим дерево определяя какие узлы являются листьями и собирая информацию для каких узлов есть условия по каким справочникам");
            sql.AppendLine(string.Format("\t\t\t\t\tselect level as lvl, connect_by_root({0}) kod_root, connect_by_isleaf as is_leaf, kod_tree_struct, {0}, name,", si.TreeKeyName));

            for (int i = 0; i < si.Spravs.Length; i++)
            {
                sql.AppendLine(string.Format("\t\t\t\t\tcase when exists (select 1 from {0} where {2} = a.{2}) then 1 else 0 end as sprav{1}_exists,", si.Spravs[i].SpravTableName, i + 1, si.TreeKeyName));
            }
            // удаляем последнюю запятую
            sql.Remove(sql.Length - 3, 1);

            sql.AppendLine(string.Format("\t\t\t\t\tfrom {0} a", si.TreeTableName));
            sql.AppendLine("\t\t\t\t\t-- для каждой структуры генерируется свой набор справочников");
            sql.AppendLine(string.Format("\t\t\t\t\twhere kod_tree_struct = {0}", kod_struct));
            sql.AppendLine("\t\t\t\t\tstart with kod_parent is null");
            sql.AppendLine(string.Format("\t\t\t\t\tconnect by prior {0} = kod_parent", si.TreeKeyName));
            sql.AppendLine("\t\t\t\t) b");
            sql.AppendLine("\t\t\t) c");

            for (int i = 0; i < si.Spravs.Length; i++)
            {
                sql.AppendLine(string.Format("\t\t\tleft join {0} sprav{1} on sprav{1}.{2} = c.{2}{1}", si.Spravs[i].SpravTableName, i + 1, si.TreeKeyName));
            }

            sql.Append(string.Format("\t\t\tinner join {0} z on", si.DataTableName));

            for (int i = 0; i < si.Spravs.Length; i++)
            {
                sql.Append(string.Format(" (c.kod_tree_sprav{0} is null or nvl(z.{1},{2}) = nvl(sprav{0}.{1},{2})) and", i + 1, si.Spravs[i].SpravKeyName, _null_replacement));
            }
            // удаляем последний and
            sql.Remove(sql.Length - 4, 4);

            sql.AppendLine();
            sql.AppendLine("\t\t\twhere is_leaf = 1");
            sql.AppendLine("\t\t);");
            sql.AppendLine();
            sql.AppendLine("\t\tcommit;");
            sql.AppendLine();
            sql.AppendLine("\tEND; -- update_result_table");
            sql.AppendLine("END;");
            sql.AppendLine("/");
            sql.AppendLine();

            return sql.ToString();
        }

        public static StructInfo GetStructInfo(decimal kod_struct)
        {
            if (kod_struct == 1)
            {
                return new StructInfo()
                {
                    TreeTableName = "vr_tree_sprav",
                    TreeKeyName = "kod_tree_sprav",
                    DataTableName = "ipr_ipr_data",
                    DataKeyName = "kod_ipr",
                    ResultTableName = "vr_sprav_ipr",

                    Spravs = new[]
                    {
                        new SpravInfo()
                        {
                            SpravTableName = "vr_sprav_krit_minenergo",
                            SpravKeyName = "kod_krit_minenergo"
                        },
                        new SpravInfo()
                        {
                            SpravTableName = "vr_sprav_razdel_ip",
                            SpravKeyName = "kod_razdel"
                        }
                    }
                };
            }

            if (kod_struct == 2)
            {
                return new StructInfo()
                {
                    TreeTableName = "vr_tree_sprav",
                    TreeKeyName = "kod_tree_sprav",
                    DataTableName = "ipr_ipr_data",
                    DataKeyName = "kod_ipr",
                    ResultTableName = "vr_sprav_ipr",

                    Spravs = new[]
                    {
                        new SpravInfo()
                        {
                            SpravTableName = "vr_sprav_klass_titul",
                            SpravKeyName = "kod_klass"
                        },
                        new SpravInfo()
                        {
                            SpravTableName = "vr_sprav_razdel_ip",
                            SpravKeyName = "kod_razdel"
                        }
                    }
                };
            }

            return null;
        }

        internal class StructInfo
        {
            public string TreeTableName;
            public string TreeKeyName;
            public string DataTableName;
            public string DataKeyName;
            public string ResultTableName;

            public SpravInfo[] Spravs;
        }

        internal class SpravInfo
        {
            public string SpravTableName;
            public string SpravKeyName;
        }
    }
}