using System;
using System.Collections.Generic;
using Npgsql;

namespace SqlBuilderLib.DevTools
{
    internal static class AnalyzerStorage
    {
        private static string ConnectionString = "Host=asusejs-dev.infoenergo.loc;Port=5432;Database=asuse;Username=asuse;Password=kl0pik";

        public static void SaveDependencies(IEnumerable<AnalyzerDependency> dependencies)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                foreach (var dep in dependencies)
                {
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO report_dev_sqlb.dependencies (object_name, object_type, used_object_name, used_object_type) VALUES (@object_name, @object_type, @used_object_name, @used_object_type)";
                        
                        command.Parameters.AddWithValue("@object_name", dep.ObjectName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@object_type", dep.ObjectType ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@used_object_name", dep.UsedObjectName ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@used_object_type", dep.UsedObjectType ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }

        public static void SaveReports(IEnumerable<AnalyzerReportInfo> reports)
        {
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                foreach (var report in reports)
                {
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "INSERT INTO report_dev_sqlb.reports (name, title, path, nav_id, nav_info) VALUES (@name, @title, @path, @nav_id, @nav_info)";
                        
                        command.Parameters.AddWithValue("@name", report.Name ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@title", report.Title ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@path", report.Path ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nav_id", report.NavId ?? (object)DBNull.Value);
                        command.Parameters.AddWithValue("@nav_info", report.NavInfo ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }
            }
        }
    }
}
