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

        /// <summary>
        /// Normalizes a string to lowercase, handling null values.
        /// </summary>
        private static string NormalizeToLower(string value)
        {
            return string.IsNullOrEmpty(value) ? value : value.ToLowerInvariant();
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
                                ObjectName = reader.IsDBNull(0) ? null : NormalizeToLower(reader.GetString(0)),
                                UsedObjectName = reader.IsDBNull(1) ? null : NormalizeToLower(reader.GetString(1))
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
                                ObjectName = reader.IsDBNull(0) ? null : NormalizeToLower(reader.GetString(0)),
                                ObjectType = reader.IsDBNull(1) ? null : DbObjectTypeExtensions.FromDatabaseString(reader.GetString(1)),
                                Processed = reader.IsDBNull(2) ? false : reader.GetBoolean(2)
                            });
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Checks if a dependency already exists (case-insensitive comparison).
        /// </summary>
        private static bool DependencyExists(string objectName, string usedObjectName, NpgsqlConnection connection)
        {
            // Check cache first (case-insensitive)
            bool existsInCache = _cachedDependencies.Any(d =>
                string.Equals(d.ObjectName, objectName, StringComparison.OrdinalIgnoreCase) &&
                string.Equals(d.UsedObjectName, usedObjectName, StringComparison.OrdinalIgnoreCase));

            if (existsInCache)
                return true;

            // Check database (case-insensitive)
            using (var checkCommand = new NpgsqlCommand())
            {
                checkCommand.Connection = connection;
                checkCommand.CommandText = "SELECT COUNT(*) FROM report_dev_sqlb.dependencies WHERE LOWER(object_name) = LOWER(@object_name) AND LOWER(used_object_name) = LOWER(@used_object_name)";
                checkCommand.Parameters.AddWithValue("@object_name", objectName ?? (object)DBNull.Value);
                checkCommand.Parameters.AddWithValue("@used_object_name", usedObjectName ?? (object)DBNull.Value);
                var count = Convert.ToInt32(checkCommand.ExecuteScalar());
                return count > 0;
            }
        }

        /// <summary>
        /// Checks if a DbObject exists (case-insensitive comparison).
        /// </summary>
        private static bool DbObjectExists(string objectName, NpgsqlConnection connection)
        {
            // Check cache first (case-insensitive)
            bool existsInCache = _cachedDbObjects.Any(db =>
                string.Equals(db.ObjectName, objectName, StringComparison.OrdinalIgnoreCase));

            if (existsInCache)
                return true;

            // Check database (case-insensitive)
            using (var checkCommand = new NpgsqlCommand())
            {
                checkCommand.Connection = connection;
                checkCommand.CommandText = "SELECT COUNT(*) FROM report_dev_sqlb.db_objects WHERE LOWER(object_name) = LOWER(@object_name)";
                checkCommand.Parameters.AddWithValue("@object_name", objectName ?? (object)DBNull.Value);
                var count = Convert.ToInt32(checkCommand.ExecuteScalar());
                return count > 0;
            }
        }

        /// <summary>
        /// Result of saving dependencies, including information about new vs existing db_objects.
        /// </summary>
        public class DependencySaveResult
        {
            public List<AnalyzerDependency> NewDependencies { get; set; } = new List<AnalyzerDependency>();
            public List<AnalyzerDependency> ExistingDependencies { get; set; } = new List<AnalyzerDependency>();
            public List<string> NewDbObjects { get; set; } = new List<string>();
            public List<string> ExistingDbObjects { get; set; } = new List<string>();
        }

        /// <summary>
        /// Saves dependencies and returns information about which ones are new vs existing, including db_objects.
        /// </summary>
        /// <returns>A DependencySaveResult containing lists of new/existing dependencies and db_objects</returns>
        public static DependencySaveResult SaveDependencies(IEnumerable<AnalyzerDependency> dependencies)
        {
            var result = new DependencySaveResult();

            lock (_lockObject)
            {
                if (!_isInitialized)
                    InitializeCache();

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    foreach (var dep in dependencies)
                    {
                        // Normalize object names to lowercase
                        string normalizedObjectName = NormalizeToLower(dep.ObjectName);
                        string normalizedUsedObjectName = NormalizeToLower(dep.UsedObjectName);

                        // Check if dependency already exists (case-insensitive)
                        bool dependencyExists = DependencyExists(normalizedObjectName, normalizedUsedObjectName, connection);

                        if (dependencyExists)
                        {
                            result.ExistingDependencies.Add(dep);
                            continue; // Skip saving existing dependency
                        }

                        // Check if DbObject exists for used_object_name, create if not exists
                        if (!string.IsNullOrEmpty(normalizedUsedObjectName) && dep.UsedObjectType.HasValue)
                        {
                            bool dbObjectExists = DbObjectExists(normalizedUsedObjectName, connection);

                            if (!dbObjectExists)
                            {
                                // Create new DbObject
                                using (var dbObjectCommand = new NpgsqlCommand())
                                {
                                    dbObjectCommand.Connection = connection;
                                    dbObjectCommand.CommandText = "INSERT INTO report_dev_sqlb.db_objects (object_name, object_type, processed) VALUES (@object_name, @object_type, @processed)";

                                    dbObjectCommand.Parameters.AddWithValue("@object_name", normalizedUsedObjectName ?? (object)DBNull.Value);
                                    dbObjectCommand.Parameters.AddWithValue("@object_type", dep.UsedObjectType.Value.ToDatabaseString() ?? (object)DBNull.Value);
                                    dbObjectCommand.Parameters.AddWithValue("@processed", false);
                                    dbObjectCommand.ExecuteNonQuery();
                                }

                                // Update cache
                                _cachedDbObjects.Add(new AnalyzerDbObject
                                {
                                    ObjectName = normalizedUsedObjectName,
                                    ObjectType = dep.UsedObjectType,
                                    Processed = false
                                });

                                result.NewDbObjects.Add(normalizedUsedObjectName);
                            }
                            else
                            {
                                result.ExistingDbObjects.Add(normalizedUsedObjectName);
                            }
                        }

                        // Save dependency (without type columns)
                        using (var command = new NpgsqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText = "INSERT INTO report_dev_sqlb.dependencies (object_name, used_object_name) VALUES (@object_name, @used_object_name)";

                            command.Parameters.AddWithValue("@object_name", normalizedObjectName ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@used_object_name", normalizedUsedObjectName ?? (object)DBNull.Value);
                            command.ExecuteNonQuery();
                        }

                        // Update cache
                        _cachedDependencies.Add(new AnalyzerDependency
                        {
                            ObjectName = normalizedObjectName,
                            UsedObjectName = normalizedUsedObjectName
                        });

                        result.NewDependencies.Add(dep);
                    }
                }
            }

            return result;
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
                        // Normalize object name to lowercase
                        string normalizedObjectName = NormalizeToLower(dbObject.ObjectName);

                        using (var command = new NpgsqlCommand())
                        {
                            command.Connection = connection;
                            command.CommandText = "INSERT INTO report_dev_sqlb.db_objects (object_name, object_type, processed) VALUES (@object_name, @object_type, @processed)";

                            command.Parameters.AddWithValue("@object_name", normalizedObjectName ?? (object)DBNull.Value);
                            command.Parameters.AddWithValue("@object_type", dbObject.ObjectType.HasValue ? dbObject.ObjectType.Value.ToDatabaseString() : (object)DBNull.Value);
                            command.Parameters.AddWithValue("@processed", dbObject.Processed);
                            command.ExecuteNonQuery();
                        }

                        // Update cache
                        _cachedDbObjects.Add(new AnalyzerDbObject
                        {
                            ObjectName = normalizedObjectName,
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

        /// <summary>
        /// Gets unprocessed database objects. If a custom query is provided, it will be used to filter the results.
        /// The custom query should select from report_dev_sqlb.db_objects table and return columns: object_name, object_type, processed.
        /// </summary>
        /// <param name="customQuery">Optional SQL query for custom filtering. If null, returns all unprocessed items.</param>
        /// <returns>List of unprocessed database objects</returns>
        public static List<AnalyzerDbObject> GetUnprocessedDbObjects(string customQuery = null)
        {
            lock (_lockObject)
            {
                if (!_isInitialized)
                    InitializeCache();

                if (string.IsNullOrWhiteSpace(customQuery))
                {
                    // Default behavior: refresh cache and return unprocessed items
                    LoadDbObjects();
                    return _cachedDbObjects.Where(db => !db.Processed).ToList();
                }
                else
                {
                    // Use custom query
                    var result = new List<AnalyzerDbObject>();
                    using (var connection = new NpgsqlConnection(ConnectionString))
                    {
                        connection.Open();
                        using (var command = new NpgsqlCommand(customQuery, connection))
                        {
                            using (var reader = command.ExecuteReader())
                            {
                                while (reader.Read())
                                {
                                    result.Add(new AnalyzerDbObject
                                    {
                                        ObjectName = reader.IsDBNull(0) ? null : NormalizeToLower(reader.GetString(0)),
                                        ObjectType = reader.IsDBNull(1) ? null : DbObjectTypeExtensions.FromDatabaseString(reader.GetString(1)),
                                        Processed = reader.IsDBNull(2) ? false : reader.GetBoolean(2)
                                    });
                                }
                            }
                        }
                    }
                    return result;
                }
            }
        }

        public static void UpdateDbObjectProcessed(string objectName, bool processed)
        {
            lock (_lockObject)
            {
                if (!_isInitialized)
                    InitializeCache();

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "UPDATE report_dev_sqlb.db_objects SET processed = @processed WHERE LOWER(object_name) = LOWER(@object_name)";
                        command.Parameters.AddWithValue("@processed", processed);
                        command.Parameters.AddWithValue("@object_name", objectName ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }

                // Update cache (case-insensitive)
                var dbObject = _cachedDbObjects.FirstOrDefault(db => string.Equals(db.ObjectName, objectName, StringComparison.OrdinalIgnoreCase));
                if (dbObject != null)
                {
                    dbObject.Processed = processed;
                }
            }
        }

        public static void UpdateDbObjectType(string objectName, DbObjectType newType)
        {
            lock (_lockObject)
            {
                if (!_isInitialized)
                    InitializeCache();

                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "UPDATE report_dev_sqlb.db_objects SET object_type = @object_type WHERE LOWER(object_name) = LOWER(@object_name)";
                        command.Parameters.AddWithValue("@object_type", newType.ToDatabaseString());
                        command.Parameters.AddWithValue("@object_name", objectName ?? (object)DBNull.Value);
                        command.ExecuteNonQuery();
                    }
                }

                // Update cache (case-insensitive)
                var dbObject = _cachedDbObjects.FirstOrDefault(db => string.Equals(db.ObjectName, objectName, StringComparison.OrdinalIgnoreCase));
                if (dbObject != null)
                {
                    dbObject.ObjectType = newType;
                }
            }
        }

        /// <summary>
        /// Fixes schemas by removing 'asuse".' prefix from object names in db_objects and dependencies tables.
        /// </summary>
        public static void FixSchemas()
        {
            lock (_lockObject)
            {
                using (var connection = new NpgsqlConnection(ConnectionString))
                {
                    connection.Open();

                    // Fix db_objects table
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "UPDATE report_dev_sqlb.db_objects SET object_name = REPLACE(object_name, 'asuse\".', '') WHERE object_name LIKE 'asuse\".%'";
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"  Fixed {rowsAffected} object names in db_objects");
                        }
                    }

                    // Fix dependencies.object_name
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "UPDATE report_dev_sqlb.dependencies SET object_name = REPLACE(object_name, 'asuse\".', '') WHERE object_name LIKE 'asuse\".%'";
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"  Fixed {rowsAffected} object names in dependencies");
                        }
                    }

                    // Fix dependencies.used_object_name
                    using (var command = new NpgsqlCommand())
                    {
                        command.Connection = connection;
                        command.CommandText = "UPDATE report_dev_sqlb.dependencies SET used_object_name = REPLACE(used_object_name, 'asuse\".', '') WHERE used_object_name LIKE 'asuse\".%'";
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            Console.WriteLine($"  Fixed {rowsAffected} used_object_names in dependencies");
                        }
                    }

                    // Reload caches to reflect changes
                    LoadDbObjects();
                    LoadDependencies();
                }
            }
        }
    }
}
