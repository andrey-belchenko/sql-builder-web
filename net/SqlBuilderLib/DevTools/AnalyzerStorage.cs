using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Npgsql;

namespace SqlBuilderLib.DevTools
{
    public static class AnalyzerStorage
    {
        private static string ConnectionString = "Host=asusejs-dev.infoenergo.loc;Port=5432;Database=asuse;Username=asuse;Password=kl0pik";
        
        private static List<AnalyzerDependency> _cachedDependencies = new List<AnalyzerDependency>();
        private static List<AnalyzerReportInfo> _cachedReports = new List<AnalyzerReportInfo>();
        private static List<AnalyzerDbObject> _cachedDbObjects = new List<AnalyzerDbObject>();
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
                LoadDbObjects();
                _isInitialized = true;
            }
        }

        private static void LoadDependencies()
        {
            _cachedDependencies.Clear();
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT object_name, used_object_name FROM report_dev_sqlb.dependencies", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _cachedDependencies.Add(new AnalyzerDependency
                            {
                                ObjectName = reader.IsDBNull(0) ? null : reader.GetString(0),
                                UsedObjectName = reader.IsDBNull(1) ? null : reader.GetString(1)
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

        private static void LoadDbObjects()
        {
            _cachedDbObjects.Clear();
            using (var connection = new NpgsqlConnection(ConnectionString))
            {
                connection.Open();
                using (var command = new NpgsqlCommand("SELECT object_name, object_type, processed FROM report_dev_sqlb.db_objects", connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            _cachedDbObjects.Add(new AnalyzerDbObject
                            {
                                ObjectName = reader.IsDBNull(0) ? null : reader.GetString(0),
                                ObjectType = reader.IsDBNull(1) ? null : DbObjectTypeExtensions.FromDatabaseString(reader.GetString(1)),
                                Processed = reader.IsDBNull(2) ? false : reader.GetBoolean(2)
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
                if (!_isInitialized)
                    InitializeCache();

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    foreach (var dep in dependencies)
                    {
                        // Check if DbObject exists for used_object_name, create if not exists
                        if (!string.IsNullOrEmpty(dep.UsedObjectName) && dep.UsedObjectType.HasValue)
                        {
                            bool dbObjectExists = _cachedDbObjects.Any(db => db.ObjectName == dep.UsedObjectName);
                            
                            if (!dbObjectExists)
                            {
                                // Check database if not in cache
                                using (var checkCommand = new NpgsqlCommand())
                                {
                                    checkCommand.Connection = connection;
                                    checkCommand.CommandText = "SELECT COUNT(*) FROM report_dev_sqlb.db_objects WHERE object_name = @object_name";
                                    checkCommand.Parameters.AddWithValue("@object_name", dep.UsedObjectName ?? (object)DBNull.Value);
                                    var count = Convert.ToInt32(checkCommand.ExecuteScalar());
                                    dbObjectExists = count > 0;
                                }
                                
                                if (!dbObjectExists)
                                {
                                    // Create new DbObject
                                    using (var dbObjectCommand = new NpgsqlCommand())
                                    {
                                        dbObjectCommand.Connection = connection;
                                        dbObjectCommand.CommandText = "INSERT INTO report_dev_sqlb.db_objects (object_name, object_type, processed) VALUES (@object_name, @object_type, @processed)";
                                        
                                        dbObjectCommand.Parameters.AddWithValue("@object_name", dep.UsedObjectName ?? (object)DBNull.Value);
                                        dbObjectCommand.Parameters.AddWithValue("@object_type", dep.UsedObjectType.Value.ToDatabaseString() ?? (object)DBNull.Value);
                                        dbObjectCommand.Parameters.AddWithValue("@processed", false);
                                        dbObjectCommand.ExecuteNonQuery();
                                    }
                                    
                                    // Update cache
                                    _cachedDbObjects.Add(new AnalyzerDbObject
                                    {
                                        ObjectName = dep.UsedObjectName,
                                        ObjectType = dep.UsedObjectType,
                                        Processed = false
                                    });
                                }
                            }
                        }
                        
                        // Save dependency (without type columns)
                        using (var command = new NpgsqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText = "INSERT INTO report_dev_sqlb.dependencies (object_name, used_object_name) VALUES (@object_name, @used_object_name)";
                            
                            command.Parameters.AddWithValue("@object_name", dep.ObjectName ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@used_object_name", dep.UsedObjectName ?? (object)DBNull.Value);
                            command.ExecuteNonQuery();
                        }
                        
                        // Update cache
                        _cachedDependencies.Add(new AnalyzerDependency
                        {
                            ObjectName = dep.ObjectName,
                            UsedObjectName = dep.UsedObjectName
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

        public static void SaveDbObjects(IEnumerable<AnalyzerDbObject> dbObjects)
        {
            lock (_lockObject)
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    foreach (var dbObject in dbObjects)
                    {
                        using (var command = new NpgsqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText = "INSERT INTO report_dev_sqlb.db_objects (object_name, object_type, processed) VALUES (@object_name, @object_type, @processed)";
                            
                            command.Parameters.AddWithValue("@object_name", dbObject.ObjectName ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@object_type", dbObject.ObjectType.HasValue ? dbObject.ObjectType.Value.ToDatabaseString() : (object)DBNull.Value);
                            command.Parameters.AddWithValue("@processed", dbObject.Processed);
                            command.ExecuteNonQuery();
                        }
                        
                        // Update cache
                        _cachedDbObjects.Add(new AnalyzerDbObject
                        {
                            ObjectName = dbObject.ObjectName,
                            ObjectType = dbObject.ObjectType,
                            Processed = dbObject.Processed
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

        private static string GetDataFolderPath()
        {
            // Get the path to the Data folder relative to the project root
            // Find project root by looking for SqlBuilder.slnx
            string projectRoot = GetProjectRoot();
            
            if (string.IsNullOrEmpty(projectRoot))
            {
                // Fallback: use current directory if project root cannot be determined
                projectRoot = Directory.GetCurrentDirectory();
            }
            
            var dataFolder = Path.Combine(projectRoot, "Data");
            
            // Ensure the Data folder exists
            if (!Directory.Exists(dataFolder))
            {
                Directory.CreateDirectory(dataFolder);
            }
            
            return dataFolder;
        }

        private static string GetProjectRoot()
        {
            try
            {
                // Start from the assembly location
                string assemblyLocation = Assembly.GetExecutingAssembly().Location;
                if (string.IsNullOrEmpty(assemblyLocation))
                {
                    // Fallback to AppContext.BaseDirectory for .NET 8
                    assemblyLocation = AppContext.BaseDirectory;
                }

                DirectoryInfo dir = new DirectoryInfo(Path.GetDirectoryName(assemblyLocation));

                // Navigate up the directory tree to find SqlBuilder.slnx
                while (dir != null)
                {
                    if (File.Exists(Path.Combine(dir.FullName, "SqlBuilder.slnx")))
                    {
                        return dir.FullName;
                    }
                    dir = dir.Parent;
                }
            }
            catch
            {
                // Return empty string if we can't determine the root
            }

            return string.Empty;
        }

        public static void SaveDependenciesToFile(string fileName = "dependencies.json")
        {
            lock (_lockObject)
            {
                if (!_isInitialized)
                    InitializeCache();

                var dataFolder = GetDataFolderPath();
                var filePath = Path.Combine(dataFolder, fileName);
                
                var json = JsonConvert.SerializeObject(_cachedDependencies, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public static void SaveReportsToFile(string fileName = "reports.json")
        {
            lock (_lockObject)
            {
                if (!_isInitialized)
                    InitializeCache();

                var dataFolder = GetDataFolderPath();
                var filePath = Path.Combine(dataFolder, fileName);
                
                var json = JsonConvert.SerializeObject(_cachedReports, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public static void SaveDbObjectsToFile(string fileName = "db_objects.json")
        {
            lock (_lockObject)
            {
                if (!_isInitialized)
                    InitializeCache();

                var dataFolder = GetDataFolderPath();
                var filePath = Path.Combine(dataFolder, fileName);
                
                var json = JsonConvert.SerializeObject(_cachedDbObjects, Formatting.Indented);
                File.WriteAllText(filePath, json);
            }
        }

        public static void SaveAllCollectionsToFiles(string dependenciesFileName = "dependencies.json", string reportsFileName = "reports.json", string dbObjectsFileName = "db_objects.json")
        {
            lock (_lockObject)
            {
                SaveDependenciesToFile(dependenciesFileName);
                SaveReportsToFile(reportsFileName);
                SaveDbObjectsToFile(dbObjectsFileName);
            }
        }
    }
}
