using System;
using System.Collections.Generic;
using System.Linq;
using Npgsql;

namespace SqlBuilderLib.DevTools
{
    internal static class AnalyzerStorage
    {
        private static string ConnectionString = "Host=asusejs-dev.infoenergo.loc;Port=5432;Database=asuse;Username=asuse;Password=kl0pik";
        
        private static List<AnalyzerDependency> _cachedDependencies = new List<AnalyzerDependency>();
        private static List<AnalyzerReportInfo> _cachedReports = new List<AnalyzerReportInfo>();
        private static bool _isInitialized = false;
        private static readonly object _lockObject = new object();

        static AnalyzerStorage()
        {
            InitializeCache();
        }

        private static void InitializeCache()
        {
            lock (_lockObject)
            {
                if (_isInitialized)
                    return;

                LoadDependencies();
                LoadReports();
                _isInitialized = true;
            }
        }

        private static void LoadDependencies()
        {
            _cachedDependencies.Clear();
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT object_name, object_type, used_object_name, used_object_type FROM report_dev_sqlb.dependencies", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _cachedDependencies.Add(new AnalyzerDependency
                            {
                                ObjectName = reader.IsDBNull(0) ? null : reader.GetString(0),
                                ObjectType = reader.IsDBNull(1) ? null : reader.GetString(1),
                                UsedObjectName = reader.IsDBNull(2) ? null : reader.GetString(2),
                                UsedObjectType = reader.IsDBNull(3) ? null : reader.GetString(3)
                            });
                        }
                    }
                }
            }
        }

        private static void LoadReports()
        {
            _cachedReports.Clear();
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT name, title, path, nav_id, nav_info FROM report_dev_sqlb.reports", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _cachedReports.Add(new AnalyzerReportInfo
                            {
                                Name = reader.IsDBNull(0) ? null : reader.GetString(0),
                                Title = reader.IsDBNull(1) ? null : reader.GetString(1),
                                Path = reader.IsDBNull(2) ? null : reader.GetString(2),
                                NavId = reader.IsDBNull(3) ? null : reader.GetString(3),
                                NavInfo = reader.IsDBNull(4) ? null : reader.GetString(4)
                            });
                        }
                    }
                }
            }
        }

        public static void SaveDependencies(IEnumerable<AnalyzerDependency> dependencies)
        {
            lock (_lockObject)
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
                        
                        // Update cache
                        _cachedDependencies.Add(new AnalyzerDependency
                        {
                            ObjectName = dep.ObjectName,
                            ObjectType = dep.ObjectType,
                            UsedObjectName = dep.UsedObjectName,
                            UsedObjectType = dep.UsedObjectType
                        });
                    }
                }
            }
        }

        public static void SaveReports(IEnumerable<AnalyzerReportInfo> reports)
        {
            lock (_lockObject)
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
                        
                        // Update cache
                        _cachedReports.Add(new AnalyzerReportInfo
                        {
                            Name = report.Name,
                            Title = report.Title,
                            Path = report.Path,
                            NavId = report.NavId,
                            NavInfo = report.NavInfo
                        });
                    }
                }
            }
        }

        public static bool IsReportExists(AnalyzerReportInfo report)
        {
            lock (_lockObject)
            {
                if (!_isInitialized)
                    InitializeCache();

                return _cachedReports.Any(r =>
                    (r.Name == report.Name || (r.Name == null && report.Name == null)) &&
                    (r.Title == report.Title || (r.Title == null && report.Title == null)) &&
                    (r.Path == report.Path || (r.Path == null && report.Path == null)) &&
                    (r.NavId == report.NavId || (r.NavId == null && report.NavId == null)) &&
                    (r.NavInfo == report.NavInfo || (r.NavInfo == null && report.NavInfo == null)));
            }
        }
    }
}
